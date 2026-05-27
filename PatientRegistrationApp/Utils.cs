using System;
using System.IO;

namespace PatientRegistrationApp
{
    internal class Utils
    {
        public static void LogAction(string action, string details, string currentUser, string logPath)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] User: {currentUser} | Action: {action} | Details: {details}";
                File.AppendAllText(logPath, logEntry + Environment.NewLine);
            }
            catch { /* Fail silently to prevent app crash on logging error */ }
        }
    }
}
