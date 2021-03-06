using Intellias.CQRS.Core.Domain;

namespace Intellias.CQRS.Tests.Utils.Pipelines.Fakes
{
    public class FakeAggregateState : AggregateState,
        IEventApplier<FakeCreateStateEvent>,
        IEventApplier<FakeUpdatedStateEvent>,
        IEventApplier<FakeDeletedStateEvent>
    {
        public FakeAggregateState()
        {
            Handles<FakeCreateStateEvent>(Apply);
            Handles<FakeUpdatedStateEvent>(Apply);
            Handles<FakeDeletedStateEvent>(Apply);
        }

        public bool IsCreated { get; private set; }

        public bool IsDeleted { get; private set; }

        public string Data { get; private set; }

        public void Apply(FakeCreateStateEvent @event)
        {
            IsCreated = true;
            Data = @event.Data;
        }

        public void Apply(FakeUpdatedStateEvent @event)
        {
            Data = @event.Data;
        }

        public void Apply(FakeDeletedStateEvent @event)
        {
            IsDeleted = true;
        }
    }
}