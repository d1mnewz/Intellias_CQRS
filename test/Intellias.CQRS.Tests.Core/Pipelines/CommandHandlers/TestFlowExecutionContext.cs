using System.Collections.Immutable;
using System.Linq;
using Intellias.CQRS.Core.Commands;
using Intellias.CQRS.Core.Events;
using Intellias.CQRS.Core.Results;

namespace Intellias.CQRS.Tests.Core.Pipelines.CommandHandlers
{
    /// <summary>
    /// Test flow execution context.
    /// </summary>
    public sealed class TestFlowExecutionContext : TestFlowExecutionContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFlowExecutionContext"/> class.
        /// </summary>
        /// <param name="host">Value for <see cref="TestFlowExecutionContextBase.Host"/>.</param>
        public TestFlowExecutionContext(SubdomainTestHost host)
            : base(host)
        {
        }

        private TestFlowExecutionContext(
            SubdomainTestHost host,
            ImmutableDictionary<string, int> aggregateVersions,
            ImmutableList<Command> commands,
            ImmutableList<IntegrationEvent> expectedEvents,
            ImmutableList<IExecutionResult> results)
            : base(host, aggregateVersions, commands, expectedEvents, results)
        {
        }

        /// <summary>
        /// Deconstructs context.
        /// </summary>
        /// <param name="executionResult">Last execution result.</param>
        /// <param name="executionContext">Current execution context.</param>
        public void Deconstruct(out IExecutionResult executionResult, out TestFlowExecutionContext executionContext)
        {
            executionResult = ExecutionResults.LastOrDefault();
            executionContext = this;
        }

        /// <summary>
        /// Adds results of command execution to context.
        /// </summary>
        /// <param name="command">Executed command.</param>
        /// <param name="integrationEvent">Expected event.</param>
        /// <param name="result">Execution result.</param>
        /// <returns>Updated execution context.</returns>
        public TestFlowExecutionContext Update(Command command, IntegrationEvent integrationEvent, IExecutionResult result)
        {
            return new TestFlowExecutionContext(
                Host,
                AggregateVersions,
                ExecutedCommands.Add(command),
                ExpectedEvents.Add(integrationEvent),
                ExecutionResults.Add(result));
        }
    }
}