using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Tasks;
using ModelsConverters.Tasks;

namespace ToDoListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITaskService tasks;

        public TasksController(ITaskService taskRepository)
        {
            this.tasks = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] ClientModels.Tasks.TaskCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                return this.BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userId = HttpContext.Items["userId"].ToString();
            var modelCreationInfo = TaskCreateInfoConverter.Convert(userId, creationInfo);
            var modelTaskInfo = await this.tasks.CreateAsync(modelCreationInfo, cancellationToken);
            var clientTaskInfo = TaskInfoConverter.Convert(modelTaskInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "taskId", clientTaskInfo.Id }
            };

            return this.CreatedAtRoute("GetTaskRoute", routeParams, clientTaskInfo);
        }

        [HttpGet]
        [Route("{taskId}", Name = "GetTaskRoute")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modeltaskId))
            {
                return this.NotFound();
            }

            Models.Tasks.MyTask modelTask = null;
            try
            {
                modelTask = await this.tasks.GetAsync(modeltaskId, cancellationToken);
            }
            catch (Models.Tasks.TaskNotFoundException)
            {
                return this.NotFound();
            }

            if (!UserHaveTaskAccess(modelTask))
            {
                return Forbid();
            }

            var clientTask = TaskConverter.Convert(modelTask);

            return this.Ok(clientTask);
        }

        [HttpDelete]
        [Route("{taskId}")]
        public async Task<IActionResult> RemoveTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var taskGuid))
            {
                return NotFound();
            }

            Models.Tasks.MyTask task = null;
            try
            {
                task = await this.tasks.GetAsync(taskGuid, cancellationToken);
            }
            catch (Models.Tasks.TaskNotFoundException)
            {
                return this.NotFound();
            }

            if(!UserHaveTaskAccess(task))
            {
                return Forbid();
            }

            try
            {
                await tasks.RemoveAsync(taskGuid, cancellationToken);
            }
            catch (Models.Tasks.TaskNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch]
        [Route("{taskId}")]
        public async Task<IActionResult> PatchTaskAsync([FromRoute] string taskId, ClientModels.Tasks.TaskPatchInfo clientPatchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (clientPatchInfo ==null)
            {
                return BadRequest();
            }
            if(!Guid.TryParse(taskId, out var taskGuid))
            {
                return NotFound();
            }
            
            Models.Tasks.MyTask task = null;
            try
            {
                task = await this.tasks.GetAsync(taskGuid, cancellationToken);
            }
            catch (Models.Tasks.TaskNotFoundException)
            {
                return this.NotFound();
            }

            if (!UserHaveTaskAccess(task))
            {
                return Forbid();
            }

            var modelPatchInfo = TaskPatchConverter.Convert(taskGuid, clientPatchInfo);
            MyTask patchTask = null;
            try
            {
                patchTask = await tasks.PatchAsync(modelPatchInfo, cancellationToken);
            }
            catch(Models.Tasks.TaskNotFoundException)
            {
                return NotFound();
            }

            return Ok(patchTask);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> SearchTasksAsync([FromQuery] ClientModels.Tasks.TaskInfoSearchQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userId = Guid.Parse(HttpContext.Items["userId"].ToString());
            
            var modelQuery = TaskInfoSearchQueryConverter.Convert(query ?? new ClientModels.Tasks.TaskInfoSearchQuery());
            var modelTasks = await tasks.SearchAsync(userId, modelQuery, cancellationToken);
            var clientTasks = modelTasks.Select(note => TaskInfoConverter.Convert(note)).ToList();
            var clientTasksList = new ClientModels.Tasks.TaskList
            {
                Tasks = clientTasks
            };

            return Ok(clientTasksList);
        }

        private bool UserHaveTaskAccess(MyTask task)
        {
            var currentUserId = HttpContext.Items["userId"].ToString();
            if (task.UserId.ToString() != currentUserId)
            {
                return false;
            }
            return true;
        }
    }
}