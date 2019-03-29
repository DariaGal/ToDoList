using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public class TaskRepository : ITaskRepository
    {

        private readonly Dictionary<Guid, MyTask> primaryKeyIndex;
        private readonly List<TaskInfo> sequenceIndex;

        public TaskRepository()
        {
            this.primaryKeyIndex = new Dictionary<Guid, MyTask>();
            this.sequenceIndex = new List<TaskInfo>();
        }

        public Task<TaskInfo> CreateAsync(TaskCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();


            var id = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var task = new MyTask
            {
                Id = id,
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                Done = false,
                Title = creationInfo.Title,
                Text = creationInfo.Text,
                Priority = creationInfo.Priority,
                DeadLine = creationInfo.DeadLine
            };

            this.primaryKeyIndex.Add(id, task);
            this.sequenceIndex.Add(task);

            return Task.FromResult<TaskInfo>(task);
        }

        public Task<MyTask> GetAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!primaryKeyIndex.TryGetValue(taskId, out var task))
            {
                throw new TaskNotFoundException(taskId);
            }

            return Task.FromResult(task);
        }

        public Task<MyTask> PatchAsync(TaskPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (!primaryKeyIndex.TryGetValue(patchInfo.Id, out var task))
            {
                throw new TaskNotFoundException(patchInfo.Id);
            }

            var updated = false;

            if (patchInfo.Title != null)
            {
                task.Title = patchInfo.Title;
                updated = true;
            }
            if (patchInfo.Text != null)
            {
                task.Text = patchInfo.Text;
                updated = true;
            }
            if (patchInfo.Priority != null)
            {
                task.Priority = patchInfo.Priority.Value;
                updated = true;
            }
            if (patchInfo.DeadLine != null)
            {
                task.DeadLine = patchInfo.DeadLine.Value;
                updated = true;
            }
            if (patchInfo.Done != null)
            {
                task.Done = patchInfo.Done.Value;
                updated = true;
            }

            if (updated)
            {
                task.LastUpdatedAt = DateTime.UtcNow;
            }

            return Task.FromResult(task);
        }

        public Task RemoveAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!primaryKeyIndex.TryGetValue(taskId, out var task))
            {
                throw new TaskNotFoundException(taskId);
            }
            primaryKeyIndex.Remove(taskId);
            sequenceIndex.Remove(task);

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<TaskInfo>> SearchAsync(Guid userId, TaskInfoSearchQuery query, CancellationToken cancelltionToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancelltionToken.ThrowIfCancellationRequested();

            var search = this.sequenceIndex.AsEnumerable();

            if (query.CreatedFrom != null)
            {
                search = search.Where(task => task.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                search = search.Where(task => task.CreatedAt <= query.CreatedTo.Value);
            }

            if (query.Offset != null)
            {
                search = search.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search = search.Take(query.Limit.Value);
            }

            if (query.Done != null)
            {
                search = search.Where(task => task.Done == query.Done.Value);
            }

            if (query.ToDeadline != null)
            {
                search = search.Where(task => task.DeadLine <= query.ToDeadline.Value);
            }

            if (query.Priority != null)
            {
                search = search.Where(task => task.Priority == query.Priority.Value);
            }

            var sort = query.Sort ?? SortType.Ascending;
            var sortBy = query.SortBy ?? TaskSortBy.Creation;


            if (sort != SortType.Ascending || sortBy != TaskSortBy.Creation)
            {
                switch (sortBy)
                {
                    case TaskSortBy.LastUpdate:
                        search = sort == SortType.Ascending ? search.OrderBy(task => task.LastUpdatedAt) 
                            : search.OrderByDescending(task => task.LastUpdatedAt);
                        break;
                    case TaskSortBy.Creation:
                        search = sort == SortType.Ascending ? search.OrderBy(task => task.CreatedAt)
                            : search.OrderByDescending(task => task.CreatedAt);
                        break;
                    case TaskSortBy.Priority:
                        search = sort == SortType.Ascending ? search.OrderBy(task => task.Priority)
                            : search.OrderByDescending(task => task.CreatedAt);
                        break;

                    default:
                        throw new ArgumentException($"Unknown note sort by value \"{sortBy}\".", nameof(query));
                }
            }

            var result = search.ToList();

            return Task.FromResult<IReadOnlyList<TaskInfo>>(result);

        }
    }
}
