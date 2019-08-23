using System;
using Intellias.CQRS.Core.Queries.Immutable;
using Intellias.CQRS.Core.Queries.Mutable;
using Intellias.CQRS.QueryStore.AzureTable.Immutable;
using Intellias.CQRS.QueryStore.AzureTable.Mutable;
using Intellias.CQRS.QueryStore.AzureTable.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MicrosoftOptions = Microsoft.Extensions.Options.Options;

namespace Intellias.CQRS.QueryStore.AzureTable
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTableQueryModelStorage(this IServiceCollection services, Action<TableStorageOptions> configure)
        {
            // Add required services.
            services.AddOptions();

            // Register Table Storage services.
            services.AddSingleton(typeof(IMutableQueryModelReader<>), typeof(MutableQueryModelTableStorage<>));
            services.AddSingleton(typeof(IMutableQueryModelWriter<>), typeof(MutableQueryModelTableStorage<>));
            services.AddSingleton(typeof(IImmutableQueryModelReader<>), typeof(ImmutableQueryModelTableStorage<>));
            services.AddSingleton(typeof(IImmutableQueryModelWriter<>), typeof(ImmutableQueryModelTableStorage<>));

            // Register Table Storage options.
            services.Configure(configure);
            services.AddSingleton<IValidateOptions<TableStorageOptions>>(new DataAnnotationValidateOptions<TableStorageOptions>(MicrosoftOptions.DefaultName));

            return services;
        }
    }
}