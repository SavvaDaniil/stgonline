using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.DTO.Payment;
using STG.Entities;
using System;
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


        public async Task<Payment> findById(int id_of_payment)
        {
            return await _dbc.Payments
                .Where(p => p.id == id_of_payment)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }
        public async Task<Payment> findByIdPreUserStatementWithoutUser(int id_of_payment)
        {
            return await _dbc.Payments
                .Where(p => p.id == id_of_payment)
                .Include(p => p.lesson)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }
        public async Task<Payment> findByTinkoffPaymentId(int tinkoffPaymentId)
        {
            return await _dbc.Payments
                .Where(p => p.tinkoffPaymentId == tinkoffPaymentId)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> findNotPayed(int id_of_payment)
        {
            return await _dbc.Payments
                .Where(p => p.id == id_of_payment && p.status == 0 && p.dateOfPayed == null)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }
        public async Task<Payment> findPayed(int id_of_payment)
        {
            return await _dbc.Payments
                .Where(p => p.id == id_of_payment && p.status == 1 && p.dateOfPayed != null)
                .Include(p => p.lesson)
                .Include(p => p.user)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }
        public async Task<Payment> findPayedForExtend(int id_of_payment)
        {
            return await _dbc.Payments
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
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> findNotPayedForLesson(User user, int id_of_payment)
        {
            return await _dbc.Payments
                .Where(p => p.user == user && p.id == id_of_payment && p.status == 0 )
                .Include(p => p.lesson)
                .Include(p => p.subscription)
                .Include(p => p.package)
                .Include(p => p.preUserWithAppointment)
                .Include(p => p.statement)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> add(User user, Lesson lesson, Subscription subscription, Package package, int is_prolongation, bool isAnyBuyedBefore = false)
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
                payment.price = package.price;
            }

            payment.isProlongation = is_prolongation;
            payment.dateOfAdd = DateTime.Now;

            await _dbc.Payments.AddAsync(payment);
            await _dbc.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> add(PreUserWithAppointment preUserWithAppointment, int priceForAppointment, bool isTest = false)
        {
            Payment payment = new Payment();
            payment.preUserWithAppointment = preUserWithAppointment;
            payment.price = priceForAppointment;
            payment.isProlongation = 0;
            payment.dateOfAdd = DateTime.Now;
            payment.isTest = isTest ? 1 : 0;

            await _dbc.Payments.AddAsync(payment);
            await _dbc.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> add(Statement statement, int priceForAppointment)
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

            await _dbc.Payments.AddAsync(payment);
            await _dbc.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> makePayedSuccess(Payment payment)
        {
            payment.dateOfPayed = DateTime.Now;
            payment.status = 1;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<Payment> addForExtended(User user, Subscription subscription, PaymentNewDTO paymentNewDTO)
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

            await _dbc.Payments.AddAsync(newPayment);
            await _dbc.SaveChangesAsync();

            return newPayment;
        }

        public async Task<bool> updateAfterExtended(Payment payment, TinkoffInitResponse tinkoffInitResponse)
        {
            payment.tinkoffPaymentId = tinkoffInitResponse.PaymentId;
            payment.tinkoffPaymentURL = tinkoffInitResponse.PaymentURL;
            payment.tinkoffErrorCode = tinkoffInitResponse.ErrorCode;

            payment.status = 1;
            payment.dateOfPayed = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateTinkoffData(Payment payment, TinkoffInitResponse tinkoffInitDTO)
        {
            payment.tinkoffPaymentId = tinkoffInitDTO.PaymentId;
            payment.tinkoffPaymentURL = tinkoffInitDTO.PaymentURL;
            payment.tinkoffErrorCode = tinkoffInitDTO.ErrorCode;
            payment.tinkoffRebillID = tinkoffInitDTO.RebillID;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> setRobokassaStatus(Payment payment)
        {
            payment.isRobokassa = 1;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateUser(Payment payment, User user)
        {
            payment.user = user;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> canselProlongation(Payment payment)
        {
            payment.isProlongation = 0;
            payment.tinkoffCanselRecurrent = 1;
            await _dbc.SaveChangesAsync();
            return true;
        }
        public async Task<bool> updateTinkoffRebuildId(Payment payment, string rebillID)
        {
            if (payment.isProlongation == 0 || payment.isItProlongation == 1) return false;

            payment.tinkoffRebillID = rebillID;

            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> sendingReceiptStart(Payment payment)
        {
            payment.isReceiptSend = 1;
            await _dbc.SaveChangesAsync();
            return true;
        }
        public async Task<bool> sentReceipt(Payment payment)
        {
            payment.isReceiptSend = 2;
            payment.dateOfSendReseipt = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> setCanselBecauseReceiptError(Payment payment)
        {
            payment.isReceiptError = 1;
            payment.dateOfReceiptError = DateTime.Now;
            await _dbc.SaveChangesAsync();

            await setCansel(payment);

            return true;
        }

        public async Task<bool> setCansel(Payment payment)
        {
            payment.isCansel = 1;
            payment.dateOfCansel = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return true;
        }
    }
}
