using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Запрос
    /// </summary>
    [JsonObject]
    public class Request
    {
        /// <summary>
        /// Название файла
        /// </summary>
        [JsonProperty]
        public string FileName { get; set; }

        /// <summary>
        /// Внутренности
        /// </summary>
        [JsonProperty]
        public string Text { get; set; }
    }
}
