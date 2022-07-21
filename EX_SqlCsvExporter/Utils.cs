using log4net;
using log4net.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace servicewithtopself
{
    public static class Utils
    {
        private static int LogDetailLevel = 6;
        private static ILog _log;
        private static readonly IConfiguration _config;
        
        static Utils()
        {            
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string sbaseLevel = _config["Appsettings:LogDetailLevel"];
            int baseLevel = (sbaseLevel == null) ? 3 : Convert.ToInt32(sbaseLevel);
            LogDetailLevel = baseLevel;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            _log = log4net.LogManager.GetLogger("log4netFileLogger");
        }
       
        public static string LogUniqueId;
        public static void LogToFile(int detailLevel, string messageType, string Message)
        {
            try
            {
                if (detailLevel > LogDetailLevel) return;
                _log.Info($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt ")} | {messageType} | {Message}");
                if (detailLevel == 0) //0 = Error
                {
                    try
                    {
                        //EmailSender.SendMailUsingMailGun(ConfigurationManager.AppSettings["EmailAddressForError"], ConfigurationManager.AppSettings["ListenerErrorEmailSubject"], Message);
                    }
                    catch (Exception ex)
                    {
                        _log.Info($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")} | {LogUniqueId} | {messageType} | {ex.Message} ");

                    }
                }

            }
            catch (Exception)
            {
            }

        }

    }
}
