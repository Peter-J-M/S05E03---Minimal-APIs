namespace iRosterApi.Helpers
{
    public static class Log
    {
        public static string LogFile { get; private set; }


        public static async Task ConfigureLogger()
        {
            LogFile =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\bp-conversions\\errors_" +
                DateTime.Now.ToString("yyyyMMdd") + ".txt";

            await InitialiseLogger();
        }
        
        private static async Task InitialiseLogger()
        {
            using (var fw = File.AppendText(LogFile))
            {
                await fw.WriteLineAsync($"API starting at {DateTime.Now:O}");
            }
            
        }


        private const int NumberOfRetries = 500;
        private const int SleepMilliseconds = 10;


        public static async Task Error(Exception e, string extraErrorMsg ="")
        {
            await Error($"ERROR {e.ToString()}\n\t Extra:  {extraErrorMsg}");
        }

        public static async Task LogMsg(string msg)
        {
            await WriteMsgToFile(msg, LogFile);
        }

        private static async Task WriteMsgToFile(string message, string file)
        {
            var msg = $"{DateTime.Now.ToString("s")} : {message}";
            for (int i = 0; i < NumberOfRetries; i++)
            {
                try
                {
                    using (var fw = File.AppendText(file))
                    {
                        await fw.WriteLineAsync(msg);
                    }

                    break;
                }
                catch (IOException e) when (i < NumberOfRetries)
                {
                    Thread.Sleep(SleepMilliseconds);
                }
            }

        }

        private static async Task Error(string errorMessage)
        {
            await WriteMsgToFile(errorMessage, LogFile);
        }



   }
}
