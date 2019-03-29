using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tasks
{
    public class TaskCreationInfo
    {
        public TaskCreationInfo(Guid userId, string title, string text, DateTime deadline, TaskPriority priority = TaskPriority.None)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.UserId = userId;
            this.Title = title;
            this.Text = text;
            this.Priority = priority;
            this.DeadLine = deadline;
        }

        public Guid UserId { get; set; }
        
        public string Title { get; set; }

        public string Text { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
