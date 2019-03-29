using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tasks
{
    public class TaskPatchInfo
    {
        public TaskPatchInfo(Guid id, string title = null, string text = null, TaskPriority? priority = null, DateTime? deadline = null, bool? done = null)
        {
            Id = id;
            Title = title;
            Text = text;
            Priority = priority;
            DeadLine = deadline;
            Done = done;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public TaskPriority? Priority { get; set; }

        public bool? Done { get; set; }

        public DateTime? DeadLine { get; set; }

        public string Text { get; set; }
    }
}
