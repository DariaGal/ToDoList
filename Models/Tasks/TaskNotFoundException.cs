using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tasks
{
    /// <summary>
    /// Исключение, которое возникает при попытке получить несуществующую заметку
    /// </summary>
    public class TaskNotFoundException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public TaskNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="noteId">Идентификатор заметки, которая не найдена</param>
        public TaskNotFoundException(Guid taskId)
            : base($"Note \"{taskId}\" is not found.")
        {
        }
    }
}
