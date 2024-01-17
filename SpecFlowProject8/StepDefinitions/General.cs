using NUnit.Framework;
using System.Diagnostics;


namespace XyzBank.StepDefinitions
{
    internal class General
    {
        private static string LOG_FILE_PATH = @"C:\logs\output.txt";
        public void WriteToLog(string message)
        {
            string currentTime = DateTime.Now.ToString();
            using (TextWriterTraceListener logListener = new(LOG_FILE_PATH))
            {                
                Trace.Listeners.Add(logListener);
                Trace.WriteLine($"{currentTime}:     {message}");                
                Trace.Listeners.Remove(logListener);
            }
        }

        internal void CheckResult(bool result, string testDescription)
        {
            if (testDescription.Equals("negativeTest"))
            {
                result = !result;
            }
            string message = string.Empty;
            if (!result)
            {
                message = "Test run failed.";
                Assert.Fail(message);
            }
            else
            {
                message = "Test run successfully.";
            }
            WriteToLog(message);
        }
    }
}
