using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ClientModels.Tasks
{
    [DataContract]
    public class TaskPatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        [DataMember(IsRequired = false)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskPriority Priority { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime DeadLine { get; set; }

        [DataMember(IsRequired = false)]
        public string Text { get; set; }

        [DataMember(IsRequired = false)]
        public bool Done { get; set; }
    }
}
