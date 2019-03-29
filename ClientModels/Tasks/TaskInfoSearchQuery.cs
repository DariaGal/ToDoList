using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels.Tasks
{
    public class TaskInfoSearchQuery
    {
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество заметок, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Минимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        public TaskPriority? Priority { get; set; }

        public DateTime? ToDeadline { get; set; }


        public bool? Done { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

        /// <summary>
        /// Аспект заметки, по которому нужно искать
        /// </summary>
        public TaskSortBy? SortBy { get; set; }
    }
}
