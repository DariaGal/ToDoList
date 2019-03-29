using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.Tasks
{
    [DataContract]
    public class TaskCreationInfo
    {

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [DataMember(IsRequired = false)]
        public TaskPriority Priority { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime DeadLine { get; set; }
    }
}
