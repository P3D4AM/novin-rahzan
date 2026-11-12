using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Protocols;

public class StaticConfigurationManager<T> : IConfigurationManager<T> where T : class
{
	private T _configuration;

	public StaticConfigurationManager(T configuration)
	{
		if (configuration == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentNullException("configuration", LogHelper.FormatInvariant("IDX20000: The parameter '{0}' cannot be a 'null' or an empty object.", "configuration")));
		}
		_configuration = configuration;
	}

	public Task<T> GetConfigurationAsync(CancellationToken cancel)
	{
		return Task.FromResult(_configuration);
	}

	public void RequestRefresh()
	{
	}
}
