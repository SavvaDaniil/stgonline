using STG.Entities;
using STG.Models.AmoCRM;
using STG.ViewModels.AmoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Factory
{
    public class AmoCRMFactory
    {
        private Dictionary<string, int> dictOfKeyLeads = new Dictionary<string, int> {
            { "newUser", 4570006 },
            { "newStatement", 4570012},
            { "newPurchaseLesson", 4570015},
            { "newPurchaseSubscriptionWithoutProlongation", 4570021},
            { "newPurchaseSubscriptionWithProlongation", 4570024},
            { "newExtend", 4570027},
            { "test", 4598692}
        };


        public AmoCRMNewContact createAmoCRMNewContact(string name, string fisrt_name, string last_name, string email)
        {
            AmoCRMNewContact amoCRMContact = new AmoCRMNewContact();
            amoCRMContact.name = name;
            amoCRMContact.first_name = (fisrt_name != null ? fisrt_name : "<Имя не указано>");
            amoCRMContact.last_name = (last_name != null ? last_name : "<Фамилия не указана>");


            AmoCRMContactDataField amoCRMContactDataField = new AmoCRMContactDataField();
            amoCRMContactDataField.value = (email != null ? email : "<Почта не указана>");
            amoCRMContactDataField.enum_code = "WORK";

            AmoCRMContactData amoCRMContactData = new AmoCRMContactData();
            amoCRMContactData.field_id = 409325;
            amoCRMContactData.field_name = "Email";
            amoCRMContactData.field_code = "EMAIL";
            amoCRMContactData.field_type = "multitext";
            amoCRMContactData.values = new AmoCRMContactDataField[]{
                amoCRMContactDataField
            };

            amoCRMContact.custom_fields_values = new AmoCRMContactData[] {
                amoCRMContactData
            };

            return amoCRMContact;
        }

        public AmoCRMNewLead createLeadTest(User user, int pipeline_id)
        {
            return createNewLead(
                "Тест от: id" + user.Id + " - " + user.Username,
                0,
                pipeline_id,
                user.id_in_amocrm
            );
        }

        public AmoCRMNewLead createLeadNewUser(User user, int pipeline_id)
        {
            return createNewLead(
                "Новый пользователь: id" + user.Id + " - " + user.Username,
                0,
                pipeline_id,
                user.id_in_amocrm
            );
        }

        public AmoCRMNewLead createLeadNewStatement(User user, Payment payment, Statement statement, int pipeline_id)
        {
            return createNewLead(
                "Заявка на наставничество id" + statement.id + " (Счет №" + payment.id + ")",
                (payment.isTest == 1 ? 0 : payment.price),
                pipeline_id,
                user.id_in_amocrm
            );
        }

        public AmoCRMNewLead createLeadNewPurchaseLesson(User user, Payment payment, Lesson lesson, int pipeline_id)
        {
            return createNewLead(
                "Покупка урока: id"+ lesson.id + " " + lesson.name + " (Счет №"+payment.id+")",
                (payment.isTest == 1 ? 0 : payment.price),
                pipeline_id,
                user.id_in_amocrm
            );
        }

        public AmoCRMNewLead createLeadNewPurchaseSubscription(User user, Payment payment, Subscription subscription, int pipeline_id)
        {
            return createNewLead(
                (payment.isProlongation == 1 ? "Подписка c автопродлением: " + subscription.name : "Подписка без автопродления: " + subscription.name) + " (Счет №" + payment.id + ")",
                (payment.isTest == 1 ? 0 : payment.price),
                pipeline_id,
                user.id_in_amocrm
            );
        }

        public AmoCRMNewLead createLeadExtend(User user, Payment payment, Subscription subscription)
        {
            return createNewLead(
                "Автопродление подписки: " + subscription.name,
                (payment.isTest == 1 ? 0 : payment.price),
                dictOfKeyLeads["newExtend"],
                user.id_in_amocrm
            );
        }



        private AmoCRMNewLead createNewLead(string name, int price, int pipeline_id, int id_in_amocrm)
        {
            AmoCRMNewLead amoCRMNewLead = new AmoCRMNewLead();
            amoCRMNewLead.name = name;
            amoCRMNewLead.price = price;
            amoCRMNewLead.pipeline_id = pipeline_id;


            AmoCRMNewLeadContact amoCRMNewLeadContact = new AmoCRMNewLeadContact();
            amoCRMNewLeadContact.id = id_in_amocrm;

            AmoCRMNewLead_embedded amoCRMNewLead_Embedded = new AmoCRMNewLead_embedded();
            amoCRMNewLead_Embedded.contacts = new AmoCRMNewLeadContact[] {
                amoCRMNewLeadContact
            };

            amoCRMNewLead._embedded = amoCRMNewLead_Embedded;

            return amoCRMNewLead;
        }
    }
}
