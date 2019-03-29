using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tasks
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
        public Guid Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя, которому принадлежит задача
        /// </summary>
        public Guid UserId { get; set; }

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
        public TaskPriority Priority { get; set; }

        public bool Done { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
