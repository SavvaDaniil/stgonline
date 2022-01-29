using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using STG.Component;
using STG.Data;
using STG.DTO.AmoCRM;
using STG.DTO.Payment;
using STG.Factory;
using STG.Interface.Facade;
using STG.Entities;
using STG.Observer;
using STG.Service;
using STG.Strategy;
using STG.ViewModels.Payment;
using STG.Models.Modulkassa;
using STG.Models.Tinkoff;
using STG.Models.Robokassa;

namespace STG.Facade
{
    public class PaymentFacade
    {
        private ApplicationDbContext _dbc;
        public PaymentFacade(ApplicationDbContext dbc)
        {
            _dbc = dbc;
        }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public PaymentFacade(ApplicationDbContext dbc, IServiceScopeFactory serviceScopeFactory)
        {
            _dbc = dbc;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private const int costOfAppointyment = 1000;

        public async Task<PaymentLiteViewModel> findNotPayed(HttpContext httpContext, int id)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            PaymentService paymentService = new PaymentService(_dbc);

            Payment payment = await paymentService.findNotPayedForLesson(user, id);
            if (payment == null) return null;

            if(payment.lesson != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.lesson);
            }
            else if (payment.subscription != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.subscription);
            }
            else if (payment.package != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.package);
            }
            else if (payment.preUserWithAppointment != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.preUserWithAppointment);
            }
            else if (payment.statement != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.statement);
            }

            return null;
        }


        public async Task<PaymentLiteViewModel> findPayed(int id) {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.findPayed(id);

            if (payment == null) return null;

            if (payment.lesson != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.lesson);
            }
            else if (payment.subscription != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.subscription);
            }
            else if (payment.package != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.package);
            }
            else if (payment.preUserWithAppointment != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.preUserWithAppointment);
            }
            else if (payment.statement != null)
            {
                return new PaymentLiteViewModel(payment.id, payment.statement);
            }
            return null;
        }





        public async Task<PaymentLiteViewModel> createNewForBuyingByUser(HttpContext httpContext, string domainHost, PaymentNewDTO paymentNewDTO)
        {
            UserFacade userFacade = new UserFacade(_dbc);
            User user = await userFacade.getCurrentOrNull(httpContext);
            if (user == null) return null;

            Payment payment = await generate(user, paymentNewDTO);
            if (payment == null) return null;

            if (paymentNewDTO.is_prolongation == 1 && user.prolongation == 0)
            {
                UserService userService = new UserService(_dbc);
                await userService.setProngationTrue(user);
            }


            PaymentService paymentService = new PaymentService(_dbc);
            //проверяем на Робокассу
            TechDataService techDataService = new TechDataService(_dbc);
            if (await techDataService.isRobokassaActive())
            {
                PaymentRobokassaStrategy paymentRobokassaStrategy = new PaymentRobokassaStrategy();
                await paymentService.setRobokassaStatus(payment);
                return new PaymentLiteViewModel(
                    paymentRobokassaStrategy.generateLink(payment),
                    0,
                    true
                );
            }


            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            TinkoffInitResponse tinkoffInitResponse = await paymentTinkoffStrategy.init(
                (user.isTest == 1 ? true : false),
                domainHost,
                payment
            );
            if (tinkoffInitResponse == null) return new PaymentLiteViewModel(payment.id, false);

            if (!await paymentService.updateTinkoffData(payment, tinkoffInitResponse)) return null;

            return new PaymentLiteViewModel(
                payment.id,
                tinkoffInitResponse.PaymentId,
                tinkoffInitResponse.PaymentURL,
                tinkoffInitResponse.ErrorCode,
                true
            );
        }

        private async Task<Payment> generate(User user, PaymentNewDTO paymentNewDTO)
        {
            Lesson lesson;
            Payment payment = null;
            PaymentFactory paymentFactory = new PaymentFactory(_dbc);
            if (paymentNewDTO.id_of_lesson != 0 && paymentNewDTO.single == 1)
            {
                LessonService lessonService = new LessonService(_dbc);
                lesson = await lessonService.findById(paymentNewDTO.id_of_lesson);
                if (lesson == null) return null;

                payment = await paymentFactory.createSingleForLesson(user, lesson, paymentNewDTO.is_prolongation);
            }

            Subscription subscription;
            if (paymentNewDTO.id_of_subscription != 0 && paymentNewDTO.single == 0)
            {
                SubscriptionService subscriptionService = new SubscriptionService(_dbc);
                subscription = await subscriptionService.findActiveById(paymentNewDTO.id_of_subscription);
                if (subscription == null) return null;

                //проверяем, если у подписки другая цена при первой покупке, есть ли уже купленные ранее подписки
                PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
                bool isAnyAlreadyBuyed = await purchaseSubscriptionService.isAnyBuyedBeforeByUser(user);

                if (paymentNewDTO.is_it_prolongation == 1 && paymentNewDTO.payment_that_extended != null)
                {
                    PaymentService paymentService = new PaymentService(_dbc);
                    payment = await paymentService.addForExtended(
                        user,
                        subscription,
                        paymentNewDTO
                    );
                }
                else
                {
                    payment = await paymentFactory.createForSubscription(user, subscription, paymentNewDTO.is_prolongation, isAnyAlreadyBuyed);
                }
            }

            Package package;
            if (paymentNewDTO.id_of_package != 0)
            {
                PackageService packageService = new PackageService(_dbc);
                package = await packageService.findById(paymentNewDTO.id_of_package);
                if (package == null) return null;
                payment = await paymentFactory.createSingleForPackage(user, package);
            }


            return payment;
        }




        public async Task<PaymentLiteViewModel> generateForPreUserAppointment(string domainHost, PreUserWithAppointment preUserWithAppointment)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.add(preUserWithAppointment, costOfAppointyment);//, (preUserWithAppointment.username == "savva.d@mail.ru" ? true : false)
            if (payment == null)return null;

            TechDataService techDataService = new TechDataService(_dbc);
            if (await techDataService.isRobokassaActive())
            {
                PaymentRobokassaStrategy paymentRobokassaStrategy = new PaymentRobokassaStrategy();
                await paymentService.setRobokassaStatus(payment);
                return new PaymentLiteViewModel(
                    paymentRobokassaStrategy.generateLink(payment),
                    0,
                    true
                );
            }

            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            TinkoffInitResponse tinkoffInitResponse = await paymentTinkoffStrategy.init(
                (payment.isTest == 1 ? true : false),
                domainHost,
                payment
            );
            if (tinkoffInitResponse == null) return new PaymentLiteViewModel(payment.id, false);

            if (!await paymentService.updateTinkoffData(payment, tinkoffInitResponse)) return null;

            return new PaymentLiteViewModel(
                payment.id,
                tinkoffInitResponse.PaymentId,
                tinkoffInitResponse.PaymentURL,
                tinkoffInitResponse.ErrorCode,
                true
            );
        }


        public async Task<PaymentLiteViewModel> generateForStatement(string domainHost, Statement statement)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.add(statement, costOfAppointyment);
            if (payment == null) return null;

            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            TinkoffInitResponse tinkoffInitResponse = await paymentTinkoffStrategy.init(
                (payment.isTest == 1 ? true : false),
                domainHost,
                payment
            );
            if (tinkoffInitResponse == null) return new PaymentLiteViewModel(payment.id, false);

            if (!await paymentService.updateTinkoffData(payment, tinkoffInitResponse)) return null;

            return new PaymentLiteViewModel(
                payment.id,
                tinkoffInitResponse.PaymentId,
                tinkoffInitResponse.PaymentURL,
                tinkoffInitResponse.ErrorCode,
                true
            );
        }




        public async Task<Payment> result(PaymentResultDTO paymentResultDTO)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.findNotPayed(paymentResultDTO.id_of_payment);
            //System.Diagnostics.Debug.WriteLine("paymentResultDTO.id: " + paymentResultDTO.id_of_payment);
            if (payment == null) return null;
            if (payment.isCansel == 1) return null;
            if (!(await checkReceiptAndSendIdIfNotSent(payment))) return null;

            return await makePayedSuccess(payment);
        }

        public async Task<Payment> resultRobokassa(RobokassaResultResponse robokassaResultResponse)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.findById(robokassaResultResponse.InvId);
            if (payment == null) return null;
            PaymentRobokassaStrategy paymentRobokassaStrategy = new PaymentRobokassaStrategy();
            if (!paymentRobokassaStrategy.isValidCrc(payment, robokassaResultResponse)) return null;
            return await makePayedSuccess(payment);
        }



        public async Task<Payment> isConfirmedByTinkoffAndUpdateIfNeed(int id_of_payment)
        {
            System.Diagnostics.Debug.WriteLine("isConfirmedByTinkoffAndUpdateIfNeed id_of_payment:" + id_of_payment);
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.findById(id_of_payment);
            if (payment == null) return null;
            if (payment.isCansel == 1)
            {
                System.Diagnostics.Debug.WriteLine("isConfirmedByTinkoffAndUpdateIfNeed payment IS CANSEL:" + id_of_payment);
                return null;
            }

            System.Diagnostics.Debug.WriteLine("isConfirmedByTinkoffAndUpdateIfNeed перед getState id_of_payment:" + id_of_payment);
            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            TinkoffGetStateDTO tinkoffGetStateDTO = await paymentTinkoffStrategy.getState((payment.isTest == 1 ? true : false), payment);
            if (tinkoffGetStateDTO == null) return null;
            if (!tinkoffGetStateDTO.Success) return null;

            if(payment.user == null && payment.preUserWithAppointment != null)
            {
                //значит заявка с регистрацией
                payment = await makePayedSuccess(payment);
                if (!(await checkReceiptAndSendIdIfNotSent(payment))) return null;
                return payment;
            } else
            {
                if (!(await checkReceiptAndSendIdIfNotSent(payment))) return null;
                return await makePayedSuccess(payment);
            }


            return await makePayedSuccess(payment);
        }

        private async Task<Payment> makePayedSuccess(Payment payment)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            if (!await paymentService.makePayedSuccess(payment)) return null;

            if (payment.lesson != null)
            {
                PurchaseLessonFacade purchaseLessonFacade = new PurchaseLessonFacade(_dbc);
                if (await purchaseLessonFacade.buyAfterSuccessfullPayment(payment, payment.user, payment.lesson) == null) return null;
                AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
                await amoCRMFacade.newPurchaseLesson(payment.user, payment, payment.lesson);
                return payment;
            }

            if (payment.subscription != null)
            {
                System.Diagnostics.Debug.WriteLine("makePayedSuccess subscription");
                PurchaseSubscriptionFacade purchaseSubscriptionFacade = new PurchaseSubscriptionFacade(_dbc);
                if (await purchaseSubscriptionFacade.buyAfterSuccessfullPayment(payment) == null) return null;
                AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
                await amoCRMFacade.newPurchaseSubscription(payment.user, payment, payment.subscription);
                return payment;
            }

            if (payment.preUserWithAppointment != null)
            {
                PreUserWithAppointmentFacade preUserWithAppointmentFacade = new PreUserWithAppointmentFacade(_dbc);
                User user = await preUserWithAppointmentFacade.registrationAfterSuccessfullPayment(payment);
                if (user == null) return null;
                if (!await paymentService.updateUser(payment, user)) return null;
                return payment;
            }

            if (payment.package != null)
            {
                PurchasePackageFacade purchasePackageFacade = new PurchasePackageFacade(_dbc);
                if (await purchasePackageFacade.buyAfterSuccessfullPayment(payment) == null) return null;

                return payment;
            }

            if (payment.statement != null)
            {
                StatementFacade statementFacade = new StatementFacade(_dbc);
                if (await statementFacade.buyAfterSuccessfullPayment(payment) == null) return null;

                return payment;
            }

            return null;
        }


        public async Task<TinkoffGetStateDTO> getState(int id_of_payment)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            Payment payment = await paymentService.findById(id_of_payment);
            if (payment == null) return null;

            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            return await paymentTinkoffStrategy.getState((payment.isTest == 1 ? true : false), payment);
        }








        public async Task<TinkoffInitResponse> initExtendForSubscription(bool isTest, string domainHost, Payment paymentForProlongation, Subscription subscription)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            /*
            Payment paymentForProlongation = await paymentService.findPayedForExtend(purchaseSubscription.payment.id);
            if (paymentForProlongation == null) return null;
            */


            PaymentNewDTO paymentNewDTO = new PaymentNewDTO();
            paymentNewDTO.id_of_subscription = paymentForProlongation.subscription.id;
            paymentNewDTO.is_prolongation = 0;
            paymentNewDTO.is_it_prolongation = 1;
            paymentNewDTO.payment_that_extended = paymentForProlongation;
            Payment paymentNewOfProlongation = await generate(paymentForProlongation.user, paymentNewDTO);
            if (paymentNewOfProlongation == null)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка paymentNewOfProlongation");
                return null;
            }

            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();

            //инициализируем новый init в банке
            TinkoffInitResponse tinkoffInitResponse = await paymentTinkoffStrategy.init(
                isTest,
                domainHost,
                paymentNewOfProlongation
            );
            if (!await paymentService.updateTinkoffData(paymentNewOfProlongation, tinkoffInitResponse))
            {
                System.Diagnostics.Debug.WriteLine("Ошибка updateTinkoffData");
                return null;
            }


            TinkoffInitResponse tinkoffInitResponseFromCharge = await paymentTinkoffStrategy.initExtendCharge(
                isTest,
                paymentForProlongation.user,
                tinkoffInitResponse.PaymentId,
                paymentForProlongation.tinkoffRebillID
            );
            if (tinkoffInitResponseFromCharge == null)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка initExtendCharge");
                return null;
            }

            if (tinkoffInitResponseFromCharge.Success)
            {
                if (!await paymentService.updateAfterExtended(paymentNewOfProlongation, tinkoffInitResponseFromCharge)) return null;

                AmoCRMFacade amoCRMFacade = new AmoCRMFacade(_dbc);
                await amoCRMFacade.newLeadExtend(paymentNewOfProlongation.user, paymentNewOfProlongation, subscription);

                return tinkoffInitResponseFromCharge;
            }


            return tinkoffInitResponseFromCharge;
        }



        private async Task<bool> canselProlongationBecauseError(Payment payment)
        {
            PaymentService paymentService = new PaymentService(_dbc);
            return await paymentService.canselProlongation(payment);
        }


        public async Task updatePaymentDataFromNotification(string requestBody)
        {
            TinkoffNotificationDTO tinkoffNotificationDTO = readNotification(requestBody);
            if (tinkoffNotificationDTO == null)
            {
                return;
            }

            LoggerComponent.writeToLog("tinkoffNotificationDTO: " + tinkoffNotificationDTO.ToString());

            //if (tinkoffNotificationDTO.Status == "CONFIRMED")
            //{
                PaymentService paymentService = new PaymentService(_dbc);
                Payment payment = await paymentService.findById(tinkoffNotificationDTO.OrderId);
                if (payment == null)
                {
                    return;
                }
            if (tinkoffNotificationDTO.RebillId != null)
            {
                if (!await paymentService.updateTinkoffRebuildId(payment, tinkoffNotificationDTO.RebillId)) return;
            }
            //}

            if (tinkoffNotificationDTO.Status == "CONFIRMED") {
                //проверяем на отправку чека
                await checkReceiptAndSendIdIfNotSent(payment);
            }

        }


        private async Task<bool> checkReceiptAndSendIdIfNotSent(Payment payment)
        {
            System.Diagnostics.Debug.WriteLine("checkReceiptAndSendIdIfNotSent");
            LoggerComponent.writeToLogModulkassa("checkReceiptAndSendIdIfNotSent payment.id : " + payment.id);
            PaymentService paymentService = new PaymentService(_dbc);

            if (payment.isReceiptSend == 0 && payment.isRobokassa == 0)
            {
                await paymentService.sendingReceiptStart(payment);

                /*
                await canselTinkoffAndPurchases(payment);
                TechDataService techDataService = new TechDataService(_dbc);
                await techDataService.launchRobokassa();
                return false;
                */


                
                ModulkassaFacade modulkassaFacade = new ModulkassaFacade(_dbc);
                ReceiptDocResponse receiptDocResponse = await modulkassaFacade.sell(payment);
                if (receiptDocResponse == null)
                {
                    //отмена покупки
                    LoggerComponent.writeToLogModulkassa("Ошибка отправки чека для платежа (null)" + payment.id);
                    TechDataService techDataService = new TechDataService(_dbc);
                    await techDataService.launchRobokassa();
                    await canselTinkoffAndPurchases(payment);
                    return false;
                }
                else if (receiptDocResponse.status == "FAILED")
                {
                    //отмена покупки
                    LoggerComponent.writeToLogModulkassa("Ошибка отправки чека для платежа FAILED" + payment.id);
                    TechDataService techDataService = new TechDataService(_dbc);
                    await techDataService.launchRobokassa();
                    await canselTinkoffAndPurchases(payment);
                    return false;
                }
                else
                {
                    LoggerComponent.writeToLogModulkassa("+ Успешно отправлен чек: " + payment.id);
                    await paymentService.sentReceipt(payment);
                }
                
                
            }
            return true;
        }


        private TinkoffNotificationDTO readNotification(string requestBody)
        {
            try
            {
                //LoggerComponent.writeToLog("Начали, полученный запрос: " + requestBody);
                JObject notificationJson = JObject.Parse(requestBody);
                //LoggerComponent.writeToLog("Получилось спарсить");

                //StringBuilder sb = new StringBuilder("");

                TinkoffNotificationDTO tinkoffNotificationDTO = new TinkoffNotificationDTO();
                foreach (var keyValue in notificationJson)
                {
                    //sb.Append(" " + keyValue.Key + ":" + keyValue.Value);

                    switch (keyValue.Key)
                    {
                        case "Status":
                            tinkoffNotificationDTO.Status = (string)keyValue.Value;
                            break;
                        case "Success":
                            tinkoffNotificationDTO.Success = (bool)keyValue.Value;
                            break;
                        case "OrderId":
                            tinkoffNotificationDTO.OrderId = int.Parse((string)keyValue.Value);
                            break;
                        case "PaymentId":
                            tinkoffNotificationDTO.PaymentId = int.Parse((string)keyValue.Value);
                            break;
                        case "RebillId":
                            tinkoffNotificationDTO.RebillId = (string)keyValue.Value;
                            break;
                        default:
                            break;

                    }
                }

                return tinkoffNotificationDTO;
            }
            catch (Exception ex)
            {
                //StatementObserver statementObserver = new StatementObserver();
                //statementObserver.sendCheck("Ошибка при попытке парсить: " + ex.Message);
                LoggerComponent.writeToLog("Ошибка при попытке парсить: " + ex.Message);
                return null;
            }
        }




        public async Task<bool> canselTinkoffAndPurchases(Payment payment)
        {
            System.Diagnostics.Debug.WriteLine("canselTinkoffAndPurchases");
            PaymentTinkoffStrategy paymentTinkoffStrategy = new PaymentTinkoffStrategy();
            TinkoffCanselResponse tinkoffCanselResponse = await paymentTinkoffStrategy.initCansel(payment);
            if (tinkoffCanselResponse == null) return false;
            if (!tinkoffCanselResponse.Success) return false;

            PaymentService paymentService = new PaymentService(_dbc);
            await paymentService.setCanselBecauseReceiptError(payment);

            //обнуляем доступы, если они есть
            PurchaseLessonService purchaseLessonService = new PurchaseLessonService(_dbc);
            await purchaseLessonService.setCanselByPayment(payment);
            PurchaseSubscriptionService purchaseSubscriptionService = new PurchaseSubscriptionService(_dbc);
            await purchaseSubscriptionService.setCanselByPayment(payment);
            PurchasePackageService purchasePackageService = new PurchasePackageService(_dbc);
            await purchasePackageService.setCanselByPayment(payment);

            return true;
        }
        
    }
}
