using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsConverters.Tasks
{
    using Model = Models.Tasks;
    using Client = ClientModels.Tasks;

    public static class TaskInfoSearchQueryConverter
    {
        public static Model.TaskInfoSearchQuery Convert(Client.TaskInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var sortBy = TaskSortByConverter.Convert(clientQuery.SortBy.Value);
            var sortType = SortTypeConverter.Convert(clientQuery.Sort.Value);
            var priority = TaskPriorityConverter.Convert(clientQuery.Priority.Value);

            var modelQuery = new Model.TaskInfoSearchQuery
            {
                Done = clientQuery.Done,
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,
                CreatedFrom = clientQuery.CreatedFrom,
                CreatedTo = clientQuery.CreatedTo,
                Priority = priority,
                ToDeadline = clientQuery.ToDeadline,
                Sort = sortType,
                SortBy = sortBy
            };

            return modelQuery;
        }
    }
}
