﻿using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Core.CrossCuttingConcerns.Logging.Log4Net
{
    public class LoggerServiceBase
    {
        ILog _log;

        public LoggerServiceBase(string name)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenRead("log4net.config"));

            ILoggerRepository loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(loggerRepository, xmlDocument["log4net"]);

            _log = LogManager.GetLogger(loggerRepository.Name, name);

        }

        public bool IsInfoEnabled => _log.IsInfoEnabled;
        public bool IsDebugEnabled => _log.IsDebugEnabled;
        public bool IsWarnEnabled => _log.IsWarnEnabled; //performans için
        public bool IsErrorEnabled => _log.IsErrorEnabled;
        public bool IsFatalEnabled => _log.IsFatalEnabled;


        public void Info(object logMessage)
        {
            if (IsInfoEnabled)
            {
                _log.Info(logMessage);
            }
        }
        public void Debug(object logMessage)
        {
            if (IsDebugEnabled)
            {
                _log.Info(logMessage);
            }
        }
        public void Warn(object logMessage)
        {
            if (IsWarnEnabled)
            {
                _log.Info(logMessage);
            }
        }
        public void Error(object logMessage)
        {
            if (IsErrorEnabled)
            {
                _log.Info(logMessage);
            }
        }
        public void Fatal(object logMessage)
        {
            if (IsFatalEnabled)
            {
                _log.Info(logMessage);
            }
        }
    }
}
