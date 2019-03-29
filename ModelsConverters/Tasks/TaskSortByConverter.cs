using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Model = Models.Tasks;
    using Client = ClientModels.Tasks;

    public static class TaskSortByConverter
    {
        public static Model.TaskSortBy Convert(Client.TaskSortBy clientSortBy)
        {
            if (!Enum.TryParse(typeof(Model.TaskSortBy), clientSortBy.ToString(), out var modelSortBy))
            {
                throw new ArgumentException($"Unknown sort by \"{clientSortBy}\".", nameof(clientSortBy));
            }

            return (Model.TaskSortBy)modelSortBy;
        }
    }
}
