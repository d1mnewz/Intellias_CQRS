﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Intellias.CQRS.Core.Queries;
using Microsoft.WindowsAzure.Storage.Table;

namespace Intellias.CQRS.QueryStore.AzureTable
{
    /// <summary>
    /// ITableQueryReader - used to read query model with azure specific features
    /// </summary>
    public interface ITableQueryReader<TQueryModel> where TQueryModel : IQueryModel, new()
    {
        /// <summary>
        /// Returns all query model items by azure table query
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<TQueryModel>> GetAllAsync(TableQuery<DynamicTableEntity> query);
    }
}