using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common
{
    public static class JsonWorker
    {
        public static string JsonSerialize<T>(T value)
        {
            try
            {
                return JsonConvert.SerializeObject(value);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T JsonDeserialize<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
