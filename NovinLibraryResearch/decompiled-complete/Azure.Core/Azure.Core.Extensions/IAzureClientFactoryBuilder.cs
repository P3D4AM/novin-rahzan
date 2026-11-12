using System;

namespace Azure.Core.Extensions;

public interface IAzureClientFactoryBuilder
{
	IAzureClientBuilder<TClient, TOptions> RegisterClientFactory<TClient, TOptions>(Func<TOptions, TClient> clientFactory) where TOptions : class;
}
