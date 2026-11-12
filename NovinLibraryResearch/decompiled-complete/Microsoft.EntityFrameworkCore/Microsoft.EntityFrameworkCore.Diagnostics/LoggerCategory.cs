using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class LoggerCategory<T>
{
	public static string Name { get; } = ToName(typeof(T));


	public override string ToString()
	{
		return Name;
	}

	public static implicit operator string([JetBrains.Annotations.NotNull] LoggerCategory<T> loggerCategory)
	{
		return loggerCategory.ToString();
	}

	private static string ToName(Type loggerCategoryType)
	{
		string text = loggerCategoryType.FullName.Replace('+', '.');
		int num = text.IndexOf(".DbLoggerCategory", StringComparison.Ordinal);
		if (num >= 0)
		{
			text = text.Substring(0, num) + text.Substring(num + ".DbLoggerCategory".Length);
		}
		return text;
	}
}
