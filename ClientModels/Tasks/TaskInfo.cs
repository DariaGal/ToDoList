using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels.Tasks
{
    public enum TaskPriority
    {
        None,
        High,
        Medium,
        Low
    }

    public class TaskInfo
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит задача
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Приоритет задачи
        /// </summary>

        [JsonConverter(typeof(StringEnumConverter))]
        public TaskPriority Priority { get; set; }

        public bool Done { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
