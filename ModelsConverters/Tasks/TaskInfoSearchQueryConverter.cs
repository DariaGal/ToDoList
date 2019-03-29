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

            var sortBy = clientQuery.SortBy.HasValue ? TaskSortByConverter.Convert(clientQuery.SortBy.Value)
                : (Model.TaskSortBy?)null;
                
            var sortType = clientQuery.Sort.HasValue ? SortTypeConverter.Convert(clientQuery.Sort.Value)
                :(Models.SortType?)null;
            var priority = clientQuery.Priority.HasValue ? TaskPriorityConverter.Convert(clientQuery.Priority.Value)
                :(Model.TaskPriority?) null;

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
