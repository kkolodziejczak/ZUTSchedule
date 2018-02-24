using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZUTSchedule.core
{
    /// <summary>
    /// Simple logging class with Caller's file path, name and line number
    /// also there are <see cref="Logger.LogLevel"/>'s that change occurrence of message on the console.
    /// This class is simple wrapper on <see cref="Debug.WriteLine"/>.
    /// <para></para>
    ///     NOTE: 
    ///         Best to use with Visual Studio Extension called "VSColorOutput"
    ///         for rich colors at Visual Studio's console.
    /// </summary>
    public static class Logger
    {
        private enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        public static void Info(string message, [CallerFilePath] string filePath = "", [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string callerName = "")
        {
            Log(message, LogLevel.Info, filePath, lineNumber, callerName);
        }

        public static void Warning(string message, [CallerFilePath] string filePath = "", [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string callerName = "")
        {
            Log(message, LogLevel.Warning, filePath, lineNumber, callerName);
        }

        public static void Error(string message, [CallerFilePath] string filePath = "", [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string callerName = "")
        {
            Log(message, LogLevel.Error, filePath, lineNumber, callerName);
        }

        private static void Log(string message, LogLevel logLevel, string filePath, int lineNumber, string callerName)
{
            string level = string.Empty;

            switch (logLevel)
            {
                case LogLevel.Info:
                    level = "+++>";
                    break;
                case LogLevel.Warning:
                    level = "[Warning]";
                    break;
                case LogLevel.Error:
                    level = "[Error]";
                    break;
            }
            Debug.WriteLine(PrepareMessage(message,level,filePath,lineNumber,callerName));

        }

        private static string PrepareMessage(string message, string level, string filePath, int lineNumber, string callerName)
        {
            var sb = new StringBuilder();

            sb.Append($"{level}\n");
            sb.Append($"{level} {DateTime.Now:dd-MMM-yyy hh:mm:ss}\n");
            sb.Append($"{level} {Path.GetFileName(filePath)}:{lineNumber} > {callerName}()\n");
            sb.Append($"{level} Message:\n");
            sb.Append($"{level} \t{message}\n");
            sb.Append($"{level}");
            return sb.ToString();
        }

    }
}
