﻿using System;
using Intellias.CQRS.Core.Domain;
using Intellias.CQRS.Core.Events;

namespace Intellias.CQRS.ProcessManager.Pipelines.Requests
{
    /// <summary>
    /// Process request.
    /// </summary>
    /// <typeparam name="TState">State request.</typeparam>
    public class ProcessRequest<TState>
        where TState : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRequest{TState}"/> class.
        /// </summary>
        /// <param name="event">Integration event.</param>
        public ProcessRequest(IIntegrationEvent @event)
        {
            Id = @event.Id;
            State = @event as TState;
            IsReplay = @event.IsReplay;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRequest{TState}"/> class.
        /// </summary>
        /// <param name="queryModel">Query model.</param>
        /// <param name="getId">Get query model snapshot id.</param>
        public ProcessRequest(TState queryModel, Func<TState, SnapshotId> getId)
        {
            var snapshotId = getId(queryModel);
            Id = $"{snapshotId.EntryId}-{snapshotId.EntryVersion}";

            State = queryModel;
        }

        /// <summary>
        /// Request id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// State.
        /// </summary>
        public TState State { get; }

        /// <summary>
        /// Is replay.
        /// </summary>
        public bool IsReplay { get; }
    }
}