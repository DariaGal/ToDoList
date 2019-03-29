using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Client = ClientModels.Tasks;
    using Model = Models.Tasks;

    public static class TaskPatchConverter
    {
        public static Model.TaskPatchInfo Convert(Guid taskId, Client.TaskPatchInfo clientPatch)
        {
            if(clientPatch == null)
            {
                throw new ArgumentNullException(nameof(clientPatch));
            }
            
            var modelPriority = TaskPriorityConverter.Convert(clientPatch.Priority);

            var modelPatch = new Model.TaskPatchInfo(
                taskId, 
                clientPatch.Title,
                clientPatch.Text,
                modelPriority,
                clientPatch.DeadLine,
                clientPatch.Done);

            return modelPatch;
        }
    }
}
