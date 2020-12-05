using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Common
{
    /// <summary>
    /// Ответ от сервера
    /// </summary>
    [JsonObject]
    public class Responce
    {
        public Responce(EResponceState state, string fileName, bool isPolinom = false)
        {
            State = state;
            IsPolinom = isPolinom;
            FileName = fileName;
        }

        /// <summary>
        /// Состояние
        /// </summary>
        [JsonProperty]
        public EResponceState State { get; }

        /// <summary>
        /// Признак полинома
        /// </summary>
        [JsonProperty]
        public bool IsPolinom { get; }

        /// <summary>
        /// Название файла
        /// </summary>
        [JsonProperty]
        public string FileName { get; }
    }
}
