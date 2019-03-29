using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Tasks
{
    public interface ITaskService
    {
        Task<TaskInfo> CreateAsync(TaskCreationInfo creationInfo, CancellationToken cancellationToken);

        Task<MyTask> GetAsync(Guid taskId, CancellationToken cancellationToken);
        Task<IReadOnlyList<TaskInfo>> SearchAsync(Guid userId, TaskInfoSearchQuery modelQuery, CancellationToken cancelltionToken);

        Task RemoveAsync(Guid taskId, CancellationToken cancellationToken);

        Task<MyTask> PatchAsync(TaskPatchInfo taskPatchInfo, CancellationToken cancellationToken);

    }
}
