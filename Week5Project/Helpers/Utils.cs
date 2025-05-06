using System.Text.Json;

namespace Week5Project.Helpers
{
    public class Utils
    {
        private static Utils _instance;
        private static readonly object _lock = new();

        private Utils() { }

        public static Utils Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Utils();
                    return _instance;
                }
            }
        }

        public string ExportToJson<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(data, options);
        }
    }
}
