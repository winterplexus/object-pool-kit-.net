//
//  EventsLog.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2021
//
using System;
using System.Globalization;
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

            logger.Info(CultureInfo.InvariantCulture, $"source method (including namespace and class): {source}");
            logger.Info(CultureInfo.InvariantCulture, $"message: {message}");
        }

        public static void WriteEvent(string source, string message, Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var logger = LogManager.GetLogger("Events");

            logger.Info(CultureInfo.InvariantCulture, $"source method (including namespace and class): {source}");
            logger.Info(CultureInfo.InvariantCulture, $"message: {message}");
            logger.Info(CultureInfo.InvariantCulture, $"exception type: {exception.GetType().Name}");
            logger.Info(CultureInfo.InvariantCulture, $"exception message: {exception.Message}");

            if (exception.InnerException != null)
            {
                logger.Info(CultureInfo.InvariantCulture, $"{singleIndent} inner exception type: {exception.InnerException.GetType().Name}");
                logger.Info(CultureInfo.InvariantCulture, $"{singleIndent} inner exception message: {exception.InnerException.Message}");
            }

            logger.Info(CultureInfo.InvariantCulture, $"exception stack trace: {ReplaceControlCharacters(exception.StackTrace)}");
        }

        private static string ReplaceControlCharacters(string input)
        {
            return string.IsNullOrEmpty(input) ? input : input.Replace(EscapeCharacters.Backspace, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.FormFeed, " | ", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.Linefeed, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.CarriageReturn, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.HorizontalTab, " ", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.VerticalTab, " | ", StringComparison.InvariantCulture);
        }
    }
}