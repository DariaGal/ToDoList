using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<MyTask> _tasks;

        public TaskService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ToDoListDB"));
            var database = client.GetDatabase("ToDoListDB");
            _tasks = database.GetCollection<MyTask>("Tasks");
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

            _tasks.InsertOne(task);

            return Task.FromResult<TaskInfo>(task);
        }

        public Task<MyTask> GetAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = _tasks.Find(t => t.Id == taskId).FirstOrDefault();

            if (task == null)
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

            var task = _tasks.Find(t => t.Id == patchInfo.Id).FirstOrDefault();

            if (task == null)
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
                _tasks.ReplaceOne(t => t.Id == task.Id, task);
            }

            return Task.FromResult(task);
        }

        public Task RemoveAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = _tasks.Find(t => t.Id == taskId).FirstOrDefault();
            if (task == null)
            {
                throw new TaskNotFoundException(taskId);
            }

            _tasks.DeleteOne(t => t.Id == taskId);

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

            FilterDefinition<MyTask> filter = CreateFieldfilter(query);

            IMongoCollection<MyTask> filteredTask = _tasks;
            if (filter != null)
            {
                filteredTask = (IMongoCollection<MyTask>)filteredTask.Find(filter);
            }

            var sort = query.Sort ?? SortType.Ascending;
            var sortBy = query.SortBy ?? TaskSortBy.Creation;

            Func<MyTask, TaskSortBy, object> setSortBy = (t, s) => SetSortByFunc(t, s);
           
            var resultTasks = (sort == SortType.Ascending)
                ? filteredTask.Find(t => true).SortBy(t => setSortBy(t, sortBy)).ToEnumerable()
                : filteredTask.Find(t => true).SortByDescending(t => setSortBy(t, sortBy)).ToEnumerable();


            if (query.Offset != null)
            {
                resultTasks = resultTasks.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                resultTasks = resultTasks.Take(query.Limit.Value);
            }

            return Task.FromResult<IReadOnlyList<TaskInfo>>(resultTasks.ToList());
        }

        private object SetSortByFunc(MyTask t, TaskSortBy sortBy)
        {
            switch (sortBy)
            {
                case TaskSortBy.LastUpdate:
                    return t.LastUpdatedAt;
                case TaskSortBy.Creation:
                    return t.CreatedAt;
                case TaskSortBy.Priority:
                    return t.Priority;

                default:
                    throw new ArgumentException($"Unknown note sort by value \"{sortBy}\".");
            }
        }

        private FilterDefinition<MyTask> CreateFieldfilter(TaskInfoSearchQuery query)
        {
            FilterDefinition<MyTask> filter = null;

            if (query.CreatedFrom != null)
            {
                filter = Builders<MyTask>.Filter.Gte(t => t.CreatedAt, query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                filter = filter & Builders<MyTask>.Filter.Lte(t => t.CreatedAt, query.CreatedTo.Value);
            }

            if (query.Done != null)
            {
                filter = filter & Builders<MyTask>.Filter.Eq(t => t.Done, query.Done.Value);
            }

            if (query.ToDeadline != null)
            {
                filter = filter & Builders<MyTask>.Filter.Lte(t => t.DeadLine, query.ToDeadline.Value);
            }

            if (query.Priority != null)
            {
                filter = filter & Builders<MyTask>.Filter.Eq(t => t.Priority, query.Priority.Value);
            }

            return filter;
        }
    }
}
