using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Model = Models.Tasks;
    using Client = ClientModels.Tasks;

    public static class TaskPriorityConverter
    {
        public static Client.TaskPriority Convert(Model.TaskPriority modelPriority)
        {
            if(!Enum.TryParse(typeof(Client.TaskPriority), modelPriority.ToString(), out var clientPriority))
            {
                throw new ArgumentException($"Unknown priority \"{modelPriority}\".", nameof(modelPriority));
            }

            return (Client.TaskPriority)clientPriority;
        }

        public static Model.TaskPriority Convert(Client.TaskPriority clientPriority)
        {
            if (!Enum.TryParse(typeof(Model.TaskPriority), clientPriority.ToString(), out var modelPriority))
            {
                throw new ArgumentException($"Unknown priority \"{clientPriority}\".", nameof(clientPriority));
            }

            return (Model.TaskPriority)modelPriority;
        }
    }
}
