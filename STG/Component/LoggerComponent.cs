using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Component
{
    public class LoggerComponent
    {
        private static string logFilePath = Directory.GetCurrentDirectory() + "\\log.txt";
        private static string logModulkassaFilePath = Directory.GetCurrentDirectory() + "\\log_Modulkassa.txt";
        private static string logTinkoffFilePath = Directory.GetCurrentDirectory() + "\\log_Tinkoff.txt";
        private static string logErrorPath = Directory.GetCurrentDirectory() + "\\log_Errors.txt";

        public static string writeToLog(string text)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(DateTime.Now + "\n" + text + "\n");
                    sw.Dispose();
                }

                return "ok";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


        public static string writeToLogModulkassa(string text)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logModulkassaFilePath))
                {
                    sw.WriteLine(DateTime.Now + "\n" + text + "\n");
                    sw.Dispose();
                }

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string writeToLogTinkoff(string text)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logTinkoffFilePath))
                {
                    sw.WriteLine(DateTime.Now + "\n" + text + "\n");
                    sw.Dispose();
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string writeToLogError(string text)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(logErrorPath))
                {
                    sw.WriteLine(DateTime.Now + "\n" + text + "\n");
                    sw.Dispose();
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
