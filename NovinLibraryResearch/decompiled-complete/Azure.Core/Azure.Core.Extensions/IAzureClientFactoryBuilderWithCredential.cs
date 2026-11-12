using System;

namespace Azure.Core.Extensions;

public interface IAzureClientFactoryBuilderWithCredential
{
	IAzureClientBuilder<TClient, TOptions> RegisterClientFactory<TClient, TOptions>(Func<TOptions, TokenCredential, TClient> clientFactory, bool requiresCredential = true) where TOptions : class;
}
