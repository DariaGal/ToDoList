using System;

namespace ModelsConverters
{
    using Client = ClientModels;
    using Model = Models;

    public static class SortTypeConverter
    {
        public static Model.SortType Convert(Client.SortType clientSortType)
        {
            if (!Enum.TryParse(typeof(Model.SortType), clientSortType.ToString(), out var modelSortType))
            {
                throw new ArgumentException($"Unknown sort by \"{clientSortType}\".", nameof(clientSortType));
            }

            return (Model.SortType)modelSortType;
        }
    }
}
