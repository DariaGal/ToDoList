using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    public static class TaskCreateInfoConverter
    {
        public static Models.Tasks.TaskCreationInfo Convert(string clientUserId, ClientModels.Tasks.TaskCreationInfo clientCreationInfo)
        {
            if (clientUserId == null)
            {
                throw new ArgumentNullException(nameof(clientUserId));
            }

            if (clientCreationInfo == null)
            {
                throw new ArgumentNullException(nameof(clientCreationInfo));
            }

            if (!Guid.TryParse(clientUserId, out var modelUserId))
            {
                throw new ArgumentException($"The client user id \"{clientUserId}\" is invalid.", nameof(clientUserId));
            }

            var modelPriority = TaskPriorityConverter.Convert(clientCreationInfo.Priority);

            var modelCreationInfo = new Models.Tasks.TaskCreationInfo(
                modelUserId,
                clientCreationInfo.Title,
                clientCreationInfo.Text,clientCreationInfo.DeadLine, modelPriority);

            return modelCreationInfo;
        }
    }
}
