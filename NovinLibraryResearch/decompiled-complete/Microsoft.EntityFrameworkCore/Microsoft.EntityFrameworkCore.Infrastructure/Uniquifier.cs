using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class Uniquifier
{
	public static string Uniquify<T>([JetBrains.Annotations.NotNull] string currentIdentifier, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<string, T> otherIdentifiers, int maxLength)
	{
		string text = Truncate(currentIdentifier, maxLength);
		int num = 1;
		while (otherIdentifiers.ContainsKey(text))
		{
			text = Truncate(currentIdentifier, maxLength, num++);
		}
		return text;
	}

	public static string Uniquify<TKey, TValue>([JetBrains.Annotations.NotNull] string currentIdentifier, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<TKey, TValue> otherIdentifiers, [JetBrains.Annotations.NotNull] Func<string, TKey> keySelector, int maxLength)
	{
		string text = Truncate(currentIdentifier, maxLength);
		int num = 1;
		while (otherIdentifiers.ContainsKey(keySelector(text)))
		{
			text = Truncate(currentIdentifier, maxLength, num++);
		}
		return text;
	}

	public static string Truncate([JetBrains.Annotations.NotNull] string identifier, int maxLength, int? uniquifier = null)
	{
		int length = GetLength(uniquifier);
		int num = maxLength - length;
		StringBuilder stringBuilder = new StringBuilder();
		if (identifier.Length <= num)
		{
			stringBuilder.Append(identifier);
		}
		else
		{
			stringBuilder.Append(identifier, 0, num - 1);
			stringBuilder.Append("~");
		}
		if (uniquifier.HasValue)
		{
			stringBuilder.Append(uniquifier.Value);
		}
		return stringBuilder.ToString();
	}

	private static int GetLength(int? number)
	{
		if (!number.HasValue)
		{
			return 0;
		}
		int num = 0;
		do
		{
			number /= 10;
			num++;
		}
		while (number.Value >= 1);
		return num;
	}
}
