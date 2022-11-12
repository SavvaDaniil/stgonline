using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Payment;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PaymentService
    {
        private ApplicationDbContext _dbc;
        public PaymentService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }


        public Payment findById(int id_of_payment)
        {
            return _dbc.Payments
                .Where(p => p.id == id_of_payment)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }
        public Payment findByIdPreUserStatementWithoutUser(int id_of_payment)
        {
            return _dbc.Payments
                .Where(p => p.id == id_of_payment)
                .Include(p => p.lesson)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }
        public Payment findByTinkoffPaymentId(int tinkoffPaymentId)
        {
            return _dbc.Payments
                .Where(p => p.tinkoffPaymentId == tinkoffPaymentId)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public Payment findNotPayed(int id_of_payment)
        {
            return _dbc.Payments
                .Where(p => p.id == id_of_payment && p.status == 0 && p.dateOfPayed == null)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }
        public Payment findPayed(int id_of_payment)
        {
            return _dbc.Payments
                .Where(p => p.id == id_of_payment && p.status == 1 && p.dateOfPayed != null)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }
        public Payment findPayedForExtend(int id_of_payment)
        {
            return _dbc.Payments
                .Where(
                    p => p.id == id_of_payment
                    && p.status == 1 
                    && p.dateOfPayed != null 
                    && p.isProlongation == 1 
                    && p.isItProlongation == 0 
                    && p.subscription != null
                    && p.tinkoffRebillID != null
                )
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }

        public Payment findNotPayedForLesson(User user, int id_of_payment)
        {
            return _dbc.Payments
                .Where(p => p.user == user && p.id == id_of_payment && p.status == 0 )
                .Include(p => p.lesson)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .FirstOrDefault();
        }

        public Payment add(User user, Lesson lesson, Subscription subscription, Package package, bool withChat, int is_prolongation, bool isAnyBuyedBefore = false)
        {
            Payment payment = new Payment();
            payment.user = user;

            if (user.isTest == 1) payment.isTest = 1;

            if (lesson != null)
            {
                payment.lesson = lesson;
                payment.price = lesson.price;
            }
            if (subscription != null)
            {
                payment.subscription = subscription;
                payment.price = (!isAnyBuyedBefore && subscription.is_discount_for_first_time == 1 ? subscription.price_for_first_time : subscription.price);
            }
            if (package != null)
            {
                payment.package = package;
                payment.price = (withChat ? package.priceWithChat : package.price);
                if (withChat)payment.isWithChat = 1;
            }

            payment.isProlongation = is_prolongation;
            payment.dateOfAdd = DateTime.Now;

            _dbc.Payments.Add(payment);
            _dbc.SaveChanges();

            return payment;
        }

        public Payment add(PreUserWithAppointment preUserWithAppointment, int priceForAppointment, bool isTest = false)
        {
            Payment payment = new Payment();
            payment.preUserWithAppointment = preUserWithAppointment;
            payment.price = priceForAppointment;
            payment.isProlongation = 0;
            payment.dateOfAdd = DateTime.Now;
            payment.isTest = isTest ? 1 : 0;

            _dbc.Payments.Add(payment);
            _dbc.SaveChanges();

            return payment;
        }

        public Payment add(Statement statement, int priceForAppointment)
        {
            Payment payment = new Payment();
            payment.statement = statement;
            payment.user = statement.user;

            if (statement.user != null)
            {
                payment.isTest = payment.user.isTest == 1 ? 1 : 0;
            }

            payment.price = priceForAppointment;
            payment.isProlongation = 0;
            payment.dateOfAdd = DateTime.Now;

            _dbc.Payments.Add(payment);
            _dbc.SaveChanges();

            return payment;
        }

        public bool makePayedSuccess(Payment payment)
        {
            payment.dateOfPayed = DateTime.Now;
            payment.status = 1;

            _dbc.SaveChanges();

            return true;
        }

        public Payment addForExtended(User user, Subscription subscription, PaymentNewDTO paymentNewDTO)
        {
            Payment newPayment = new Payment();
            newPayment.status = 0;
            newPayment.user = user;
            newPayment.subscription = subscription;
            newPayment.price = subscription.price;
            newPayment.isTest = user.isTest;

            newPayment.isProlongation = 0;
            newPayment.isItProlongation = 1;
            newPayment.id_of_payment_that_extended = paymentNewDTO.payment_that_extended;
            newPayment.dateOfAdd = DateTime.Now;

            _dbc.Payments.Add(newPayment);
            _dbc.SaveChanges();

            return newPayment;
        }

        public bool updateAfterExtended(Payment payment, TinkoffInitResponse tinkoffInitResponse)
        {
            payment.tinkoffPaymentId = tinkoffInitResponse.PaymentId;
            payment.tinkoffPaymentURL = tinkoffInitResponse.PaymentURL;
            payment.tinkoffErrorCode = tinkoffInitResponse.ErrorCode;

            payment.status = 1;
            payment.dateOfPayed = DateTime.Now;
            _dbc.SaveChanges();
            return true;
        }

        public bool updateTinkoffData(Payment payment, TinkoffInitResponse tinkoffInitDTO)
        {
            payment.tinkoffPaymentId = tinkoffInitDTO.PaymentId;
            payment.tinkoffPaymentURL = tinkoffInitDTO.PaymentURL;
            payment.tinkoffErrorCode = tinkoffInitDTO.ErrorCode;
            payment.tinkoffRebillID = tinkoffInitDTO.RebillID;

            _dbc.SaveChanges();

            return true;
        }

        public bool setRobokassaStatus(Payment payment)
        {
            payment.isRobokassa = 1;
            _dbc.SaveChanges();
            return true;
        }

        public bool updateUser(Payment payment, User user)
        {
            payment.user = user;

            _dbc.SaveChanges();

            return true;
        }

        public bool canselProlongation(Payment payment)
        {
            payment.isProlongation = 0;
            payment.tinkoffCanselRecurrent = 1;
            _dbc.SaveChanges();
            return true;
        }
        public bool updateTinkoffRebuildId(Payment payment, string rebillID)
        {
            if (payment.isProlongation == 0 || payment.isItProlongation == 1) return false;

            payment.tinkoffRebillID = rebillID;

            _dbc.SaveChanges();
            return true;
        }

        public bool sendingReceiptStart(Payment payment)
        {
            payment.isReceiptSend = 1;
            _dbc.SaveChanges();
            return true;
        }
        public bool sentReceipt(Payment payment)
        {
            payment.isReceiptSend = 2;
            payment.dateOfSendReseipt = DateTime.Now;
            _dbc.SaveChanges();
            return true;
        }

        public bool setCanselBecauseReceiptError(Payment payment)
        {
            payment.isReceiptError = 1;
            payment.dateOfReceiptError = DateTime.Now;
            _dbc.SaveChanges();

            setCansel(payment);

            return true;
        }

        public bool setCansel(Payment payment)
        {
            payment.isCansel = 1;
            payment.dateOfCansel = DateTime.Now;
            _dbc.SaveChanges();
            return true;
        }

        public List<Payment> listAllPayedByDates(DateTime dateFrom, DateTime dateTo)
        {
            return _dbc.Payments
                .Include(p => p.user)
                .Include(p => p.lesson)
                .Include(p => p.package)
                .Include(p => p.subscription)
                .Include(p => p.statement)
                .Where(p => p.status == 1 && p.dateOfPayed >= dateFrom && p.dateOfPayed <= dateTo)
                .OrderByDescending(p => p.id)
                .ToList();
        }
    }
}
