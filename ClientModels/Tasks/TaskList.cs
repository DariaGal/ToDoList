using System;
using System.Collections.Generic;
using System.Text;

namespace ClientModels.Tasks
{
    public class TaskList
    {
        public IReadOnlyCollection<TaskInfo> Tasks { get; set; }
    }
}
