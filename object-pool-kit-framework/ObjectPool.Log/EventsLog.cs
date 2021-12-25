//
//  EventsLog.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
using System;
using NLog;
using ObjectPool.Utility;

namespace ObjectPool.Log
{
    public static class EventsLog
    {
        private const string singleIndent = "  -";

        public static void WriteEvent(string source, string message)
        {
            var logger = LogManager.GetLogger("Events");

            logger.Info($"source method (including namespace and class): {source}");
            logger.Info($"message: {message}");
        }

        public static void WriteEvent(string source, string message, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var logger = LogManager.GetLogger("Events");

            logger.Info($"source method (including namespace and class): {source}");
            logger.Info($"message: {message}");
            logger.Info($"exception type: {exception.GetType().Name}");
            logger.Info($"exception message: {exception.Message}");

            if (exception.InnerException != null)
            {
                logger.Info($"{singleIndent} inner exception type: {exception.InnerException.GetType().Name}");
                logger.Info($"{singleIndent} inner exception message: {exception.InnerException.Message}");
            }

            logger.Info($"exception stack trace: {ReplaceControlCharacters(exception.StackTrace)}");
        }

        private static string ReplaceControlCharacters(string input)
        {
            return string.IsNullOrEmpty(input) ? input : input.Replace(EscapeCharacters.Backspace, "").
                                                               Replace(EscapeCharacters.FormFeed, " | ").
                                                               Replace(EscapeCharacters.Linefeed, "").
                                                               Replace(EscapeCharacters.CarriageReturn, "").
                                                               Replace(EscapeCharacters.HorizontalTab, " ").
                                                               Replace(EscapeCharacters.VerticalTab, " | ");
        }
    }
}