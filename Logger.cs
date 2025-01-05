using System.Configuration;

namespace OSPract3
{
    public static class Logger
    {
        public static Label? Label { get; set; }
        private static string? info;
        private static StreamWriter? _logStream;
        private static bool _useLog = ConfigurationManager.AppSettings.Get("useLog") == "1";

        static Logger()
        {
            if (_useLog)
                _logStream = new StreamWriter(File.Open("Log.txt", FileMode.Append, FileAccess.Write));
            UpdateMessage();
        }

        public static void Close()
        {
            _logStream?.Flush();
            _logStream?.Dispose();
        }

        private static object _lock = new object();

        private static async void UpdateMessage()
        {
            while (true)
            {
                lock (_lock)
                {
                    if (info != null && Label != null)
                    {
                        Label.Text = info;
                        _logStream?.Write($"(DateTime :: {DateTime.Now}): {info}\n");
                        info = null;
                    }
                }
                await Task.Delay(100);
            }
        }

        public static void Log(string message)
        {
            lock (_lock)
            {
                //Импользование средств для обеспечения потокобезопасности
                info = message;
            }
        }
    }
}
