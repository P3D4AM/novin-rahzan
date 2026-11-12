using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public abstract class NamedConnectionStringResolverBase : INamedConnectionStringResolver
{
	private const string DefaultSection = "ConnectionStrings:";

	protected abstract IServiceProvider ApplicationServiceProvider { get; }

	public virtual string ResolveConnectionString(string connectionString)
	{
		string text = TryGetConnectionName(connectionString);
		if (text == null)
		{
			return connectionString;
		}
		IConfiguration configuration = ApplicationServiceProvider?.GetService<IConfiguration>();
		return (configuration?[text] ?? configuration?["ConnectionStrings:" + text]) ?? throw new InvalidOperationException(RelationalStrings.NamedConnectionStringNotFound(text));
	}

	private static string TryGetConnectionName(string connectionString)
	{
		int num = connectionString.IndexOf('=');
		if (num < 0)
		{
			return null;
		}
		if (connectionString.IndexOf('=', num + 1) >= 0)
		{
			return null;
		}
		if (!connectionString.Substring(0, num).Trim().Equals("name", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		return connectionString.Substring(num + 1).Trim();
	}
}
