using STG.Entities;
using STG.Models.Robokassa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STG.Strategy
{
    public class PaymentRobokassaStrategy
    {
        private const string sMrchLogin = "XXXXXXXXXXX";
        private const string sMrchPass1 = "XXXXXXXXXXX";
        private const string sMrchPass2 = "XXXXXXXXXXX";
        private const string sMrchPass1Demo = "XXXXXXXXXXX";
        private const string sMrchPass2Demo = "XXXXXXXXXXX";

        public string generateLink(Payment payment)
        {
            decimal nOutSum = payment.price;
            int nInvId = payment.id;
            string sDesc = "Счёт №" + nInvId;
            int IsTest = (payment.isTest == 1 ? 1 : 0);

            string sOutSum = nOutSum.ToString("0.00", CultureInfo.InvariantCulture);
            string sCrcBase = string.Format("{0}:{1}:{2}:{3}", sMrchLogin, sOutSum, nInvId, (payment.isTest == 1 ? sMrchPass1Demo : sMrchPass1));

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

            StringBuilder sbSignature = new StringBuilder();
            foreach (byte b in bSignature)
                sbSignature.AppendFormat("{0:x2}", b);

            string sCrc = sbSignature.ToString();
            return "https://auth.robokassa.ru/Merchant/Index.aspx?" +
                                        "MerchantLogin=" + sMrchLogin +
                                        "&OutSum=" + sOutSum +
                                        "&InvId=" + nInvId +
                                        //"&Description=" + sDesc +
                                        "&IsTest=" + IsTest +
                                        "&SignatureValue=" + sCrc;
        }

        public bool isValidCrc(Payment payment, RobokassaResultResponse robokassaResultResponse)
        {
            string sCrcBase = string.Format("{0}:{1}:{2}", robokassaResultResponse.OutSum, robokassaResultResponse.InvId, (payment.isTest == 1 ? sMrchPass2Demo : sMrchPass2));

            // build own CRC
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

            StringBuilder sbSignature = new StringBuilder();
            foreach (byte b in bSignature)
                sbSignature.AppendFormat("{0:x2}", b);

            string sMyCrc = sbSignature.ToString();

            return (sMyCrc.ToUpper() == robokassaResultResponse.SignatureValue.ToUpper());
        }
    }
}
