﻿namespace Intellias.CQRS.Core.Domain
{
    /// <summary>
    /// Entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id of entity.
        /// </summary>
        string Id { get; }
    }
}
