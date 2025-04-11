//
//  EventsLog.cs
//
//  Copyright (c) Code Construct System 2018-2025
//
using System;
using NLog;
using ObjectPool.Utility;

[assembly: CLSCompliant(true)]
namespace ObjectPool.Log
{
    public static class EventsLog
    {
        private const string singleIndent = "  -";

        public static void WriteEvent(string source, string message)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(message);

            var logger = LogManager.GetLogger("Events");

            logger.Info($"source method (including namespace and class): {source}");
            logger.Info($"message: {message}");
        }

        public static void WriteEvent(string source, string message, Exception exception)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(exception);

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
            return string.IsNullOrEmpty(input) ? input : input.Replace(EscapeCharacters.Backspace, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.FormFeed, " | ", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.Linefeed, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.CarriageReturn, "", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.HorizontalTab, " ", StringComparison.InvariantCulture).
                                                               Replace(EscapeCharacters.VerticalTab, " | ", StringComparison.InvariantCulture);
        }
    }
}