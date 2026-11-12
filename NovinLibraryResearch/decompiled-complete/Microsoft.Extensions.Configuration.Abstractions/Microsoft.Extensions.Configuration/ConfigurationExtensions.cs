using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
	public static IConfigurationBuilder Add<TSource>(this IConfigurationBuilder builder, Action<TSource>? configureSource) where TSource : IConfigurationSource, new()
	{
		TSource val = new TSource();
		configureSource?.Invoke(val);
		return builder.Add(val);
	}

	public static string? GetConnectionString(this IConfiguration configuration, string name)
	{
		return configuration?.GetSection("ConnectionStrings")[name];
	}

	public static IEnumerable<KeyValuePair<string, string?>> AsEnumerable(this IConfiguration configuration)
	{
		return configuration.AsEnumerable(makePathsRelative: false);
	}

	public static IEnumerable<KeyValuePair<string, string?>> AsEnumerable(this IConfiguration configuration, bool makePathsRelative)
	{
		Stack<IConfiguration> stack = new Stack<IConfiguration>();
		stack.Push(configuration);
		int prefixLength = ((makePathsRelative && configuration is IConfigurationSection configurationSection) ? (configurationSection.Path.Length + 1) : 0);
		while (stack.Count > 0)
		{
			IConfiguration config = stack.Pop();
			if (config is IConfigurationSection configurationSection2 && (!makePathsRelative || config != configuration))
			{
				yield return new KeyValuePair<string, string>(configurationSection2.Path.Substring(prefixLength), configurationSection2.Value);
			}
			foreach (IConfigurationSection child in config.GetChildren())
			{
				stack.Push(child);
			}
		}
	}

	public static bool Exists([NotNullWhen(true)] this IConfigurationSection? section)
	{
		if (section == null)
		{
			return false;
		}
		if (section.Value == null)
		{
			return section.GetChildren().Any();
		}
		return true;
	}

	public static IConfigurationSection GetRequiredSection(this IConfiguration configuration, string key)
	{
		System.ThrowHelper.ThrowIfNull(configuration, "configuration");
		IConfigurationSection section = configuration.GetSection(key);
		if (section.Exists())
		{
			return section;
		}
		throw new InvalidOperationException(System.SR.Format(System.SR.InvalidSectionName, key));
	}
}
