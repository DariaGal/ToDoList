using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Model = Models.Tasks;
    using Client = ClientModels.Tasks;

    public static class TaskConverter
    {
        public static Client.MyTask Convert(Model.MyTask modelTask)
        {
            if(modelTask == null)
            {
                throw new ArgumentNullException(nameof(modelTask));
            }

            var clientPriority = TaskPriorityConverter.Convert(modelTask.Priority);
            var clientTask = new Client.MyTask
            {
                Id = modelTask.Id.ToString(),
                UserId = modelTask.UserId.ToString(),
                Title = modelTask.Title,
                CreatedAt = modelTask.CreatedAt,
                LastUpdatedAt = modelTask.LastUpdatedAt,
                Priority = clientPriority,
                Done = modelTask.Done,
                DeadLine = modelTask.DeadLine,
                Text = modelTask.Text
            };
            return clientTask;
        }
    }
}
