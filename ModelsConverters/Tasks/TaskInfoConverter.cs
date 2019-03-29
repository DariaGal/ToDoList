using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Model = Models.Tasks;
    using Client = ClientModels.Tasks;

    public static class TaskInfoConverter
    {
        public static Client.TaskInfo Convert(Model.TaskInfo modelTaskInfo)
        {
            if (modelTaskInfo == null)
            {
                throw new ArgumentNullException(nameof(modelTaskInfo));
            }
            
            var clientPriority = TaskPriorityConverter.Convert(modelTaskInfo.Priority);

            var clientTaskInfo = new Client.TaskInfo
            {
                Id = modelTaskInfo.Id.ToString(),
                UserId = modelTaskInfo.UserId.ToString(),
                Title = modelTaskInfo.Title,
                CreatedAt = modelTaskInfo.CreatedAt,
                LastUpdatedAt = modelTaskInfo.LastUpdatedAt,
                Priority = clientPriority,
                Done = modelTaskInfo.Done,
                DeadLine = modelTaskInfo.DeadLine
            };

            return clientTaskInfo;
        }
    }
}
