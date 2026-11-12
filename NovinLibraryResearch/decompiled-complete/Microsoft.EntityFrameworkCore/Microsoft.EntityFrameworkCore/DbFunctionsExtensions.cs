using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore;

public static class DbFunctionsExtensions
{
	private static readonly char[] _regexSpecialChars = new char[12]
	{
		'.', '$', '^', '{', '[', '(', '|', ')', '*', '+',
		'?', '\\'
	};

	private static readonly string _defaultEscapeRegexCharsPattern = BuildEscapeRegexCharsPattern(_regexSpecialChars);

	private static readonly TimeSpan _regexTimeout = TimeSpan.FromMilliseconds(1000.0);

	public static bool Like([JetBrains.Annotations.CanBeNull] this DbFunctions _, [JetBrains.Annotations.CanBeNull] string matchExpression, [JetBrains.Annotations.CanBeNull] string pattern)
	{
		return LikeCore(matchExpression, pattern, null);
	}

	public static bool Like([JetBrains.Annotations.CanBeNull] this DbFunctions _, [JetBrains.Annotations.CanBeNull] string matchExpression, [JetBrains.Annotations.CanBeNull] string pattern, [JetBrains.Annotations.CanBeNull] string escapeCharacter)
	{
		return LikeCore(matchExpression, pattern, escapeCharacter);
	}

	private static string BuildEscapeRegexCharsPattern(IEnumerable<char> regexSpecialChars)
	{
		return string.Join("|", regexSpecialChars.Select((char c) => "\\" + c));
	}

	private static bool LikeCore(string matchExpression, string pattern, string escapeCharacter)
	{
		char? singleEscapeCharacter = ((escapeCharacter == null || escapeCharacter.Length == 0) ? null : new char?(escapeCharacter.First()));
		if (matchExpression == null || pattern == null)
		{
			return false;
		}
		if (matchExpression.Equals(pattern, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (matchExpression.Length == 0 || pattern.Length == 0)
		{
			return false;
		}
		string pattern2 = ((!singleEscapeCharacter.HasValue) ? _defaultEscapeRegexCharsPattern : BuildEscapeRegexCharsPattern(_regexSpecialChars.Where((char c) => c != singleEscapeCharacter)));
		string text = Regex.Replace(pattern, pattern2, (Match c) => "\\" + c, RegexOptions.None, _regexTimeout);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			char c2 = text[i];
			bool flag = i > 0 && text[i - 1] == singleEscapeCharacter;
			switch (c2)
			{
			case '_':
				stringBuilder.Append(flag ? '_' : '.');
				continue;
			case '%':
				stringBuilder.Append(flag ? "%" : ".*");
				continue;
			}
			if (c2 != singleEscapeCharacter)
			{
				stringBuilder.Append(c2);
			}
		}
		text = stringBuilder.ToString();
		return Regex.IsMatch(matchExpression, "\\A" + text + "\\s*\\z", RegexOptions.IgnoreCase | RegexOptions.Singleline, _regexTimeout);
	}
}
