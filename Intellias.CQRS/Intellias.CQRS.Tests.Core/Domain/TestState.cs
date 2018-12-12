﻿using Intellias.CQRS.Core.Domain;
using Intellias.CQRS.Tests.Core.Events;

namespace Intellias.CQRS.Tests.Core.Domain
{
    /// <summary>
    /// Demo State
    /// </summary>
    public class TestState : AggregateState,
        IEventApplier<TestCreatedEvent>,
        IEventApplier<TestUpdatedEvent>,
        IEventApplier<TestDeletedEvent>
    {
        /// <summary>
        /// TestData
        /// </summary>
        public string TestData { get; private set; }

        /// <summary>
        /// Demo State
        /// </summary>
        public TestState()
        {
            Handles<TestCreatedEvent>(Apply);
            Handles<TestUpdatedEvent>(Apply);
            Handles<TestDeletedEvent>(Apply);
        }

        /// <inheritdoc />
        public void Apply(TestCreatedEvent @event)
        {
            TestData = @event.TestData;
        }

        /// <inheritdoc />
        public void Apply(TestUpdatedEvent @event)
        {
            TestData = @event.TestData;
        }

        /// <inheritdoc />
        public void Apply(TestDeletedEvent @event)
        {
            // There nothing to do for now, item is deactivated
        }
    }
}
