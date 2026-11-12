using System;

namespace Microsoft.Extensions.Logging;

internal static class LoggerRuleSelector
{
	public static void Select(LoggerFilterOptions options, Type providerType, string category, out LogLevel? minLevel, out Func<string, string, LogLevel, bool> filter)
	{
		filter = null;
		minLevel = options.MinLevel;
		string alias = ProviderAliasUtilities.GetAlias(providerType);
		LoggerFilterRule loggerFilterRule = null;
		foreach (LoggerFilterRule item in options.RulesInternal)
		{
			if (IsBetter(item, loggerFilterRule, providerType.FullName, category) || (!string.IsNullOrEmpty(alias) && IsBetter(item, loggerFilterRule, alias, category)))
			{
				loggerFilterRule = item;
			}
		}
		if (loggerFilterRule != null)
		{
			filter = loggerFilterRule.Filter;
			minLevel = loggerFilterRule.LogLevel;
		}
	}

	private static bool IsBetter(LoggerFilterRule rule, LoggerFilterRule current, string logger, string category)
	{
		if (rule.ProviderName != null && rule.ProviderName != logger)
		{
			return false;
		}
		string categoryName = rule.CategoryName;
		if (categoryName != null)
		{
			int num = categoryName.IndexOf('*');
			if (num != -1 && categoryName.IndexOf('*', num + 1) != -1)
			{
				throw new InvalidOperationException(System.SR.MoreThanOneWildcard);
			}
			ReadOnlySpan<char> value;
			ReadOnlySpan<char> value2;
			if (num == -1)
			{
				value = MemoryExtensions.AsSpan(categoryName);
				value2 = default(ReadOnlySpan<char>);
			}
			else
			{
				value = MemoryExtensions.AsSpan(categoryName, 0, num);
				value2 = MemoryExtensions.AsSpan(categoryName, num + 1);
			}
			if (!MemoryExtensions.AsSpan(category).StartsWith(value, StringComparison.OrdinalIgnoreCase) || !MemoryExtensions.AsSpan(category).EndsWith(value2, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
		}
		if (current != null && current.ProviderName != null)
		{
			if (rule.ProviderName == null)
			{
				return false;
			}
		}
		else if (rule.ProviderName != null)
		{
			return true;
		}
		if (current != null && current.CategoryName != null)
		{
			if (rule.CategoryName == null)
			{
				return false;
			}
			if (current.CategoryName.Length > rule.CategoryName.Length)
			{
				return false;
			}
		}
		return true;
	}
}
