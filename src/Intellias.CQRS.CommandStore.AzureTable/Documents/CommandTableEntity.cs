﻿using Microsoft.Azure.Cosmos.Table;

namespace Intellias.CQRS.CommandStore.AzureTable.Documents
{
    /// <summary>
    /// CommandTableEntity.
    /// </summary>
    public class CommandTableEntity : TableEntity
    {
        /// <summary>
        /// Keeps serialized command itself.
        /// </summary>
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Keeps an event type.
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Expected version of command.
        /// </summary>
        public int ExpectedVersion { get; set; }
    }
}
