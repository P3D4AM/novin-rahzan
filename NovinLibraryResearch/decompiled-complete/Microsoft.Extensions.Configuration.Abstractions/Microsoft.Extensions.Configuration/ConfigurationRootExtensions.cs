using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationRootExtensions
{
	public static string GetDebugView(this IConfigurationRoot root)
	{
		return root.GetDebugView(null);
	}

	public static string GetDebugView(this IConfigurationRoot root, Func<ConfigurationDebugViewContext, string>? processValue)
	{
		IConfigurationRoot root2 = root;
		Func<ConfigurationDebugViewContext, string> processValue2 = processValue;
		StringBuilder stringBuilder2 = new StringBuilder();
		RecurseChildren(stringBuilder2, root2.GetChildren(), "");
		return stringBuilder2.ToString();
		void RecurseChildren(StringBuilder stringBuilder, IEnumerable<IConfigurationSection> children, string indent)
		{
			foreach (IConfigurationSection child in children)
			{
				(string, IConfigurationProvider) valueAndProvider = GetValueAndProvider(root2, child.Path);
				if (valueAndProvider.Item2 != null)
				{
					string text;
					if (processValue2 == null)
					{
						(text, _) = valueAndProvider;
					}
					else
					{
						text = processValue2(new ConfigurationDebugViewContext(child.Key, child.Path, valueAndProvider.Item1, valueAndProvider.Item2));
					}
					string value = text;
					stringBuilder.Append(indent).Append(child.Key).Append('=')
						.Append(value)
						.Append(" (")
						.Append(valueAndProvider.Item2)
						.AppendLine(")");
				}
				else
				{
					stringBuilder.Append(indent).Append(child.Key).AppendLine(":");
				}
				RecurseChildren(stringBuilder, child.GetChildren(), indent + "  ");
			}
		}
	}

	private static (string Value, IConfigurationProvider Provider) GetValueAndProvider(IConfigurationRoot root, string key)
	{
		foreach (IConfigurationProvider item in root.Providers.Reverse())
		{
			if (item.TryGet(key, out string value))
			{
				return (Value: value, Provider: item);
			}
		}
		return (Value: null, Provider: null);
	}
}
