using System;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class EscapeSequence
{
	private readonly char startChar;

	private readonly char endChar;

	private readonly int startIndex;

	public static readonly EscapeSequence DoubleQuotedEscapeSequence = new EscapeSequence('"', '"');

	public static readonly EscapeSequence BracketedEscapeSequence = new EscapeSequence('[', ']');

	public static readonly EscapeSequence SingleQuotedEscapeSequence = new EscapeSequence('\'', '\'');

	private static readonly EscapeSequence DoubleQuotedSkipOneEscapeSequence = new EscapeSequence('"', '"', 1);

	private static readonly EscapeSequence SingleQuotedSkipOneEscapeSequence = new EscapeSequence('\'', '\'', 1);

	private EscapeSequence(char start, char end)
		: this(start, end, 0)
	{
	}

	private EscapeSequence(char start, char end, int startIndex)
	{
		startChar = start;
		endChar = end;
		this.startIndex = startIndex;
	}

	private bool Matches(string value)
	{
		if (startIndex >= value.Length)
		{
			return false;
		}
		return value[startIndex] == startChar;
	}

	public string Escape(string value)
	{
		char[] array = new char[value.Length * 2 + 2];
		int length = 0;
		array[length++] = startChar;
		foreach (char c in value)
		{
			array[length++] = c;
			if (c == endChar)
			{
				array[length++] = endChar;
			}
		}
		array[length++] = endChar;
		return new string(array, 0, length);
	}

	public string Unescape(string value)
	{
		char[] array = new char[value.Length];
		int length = 0;
		for (int i = startIndex + 1; i < value.Length; i++)
		{
			char c = value[i];
			if (c == endChar)
			{
				if (i == value.Length - 1)
				{
					break;
				}
				if (value[i + 1] == endChar)
				{
					i++;
				}
			}
			array[length++] = c;
		}
		return new string(array, 0, length);
	}

	public static bool RequiresEscaping(string value)
	{
		if (value.Length == 0)
		{
			return false;
		}
		if (!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsIdentifierStart(value[0]))
		{
			return true;
		}
		for (int i = 1; i < value.Length; i++)
		{
			if (!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsIdentifierCharacter(value[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static string UnescapeIdentifier(string value)
	{
		EscapeSequence escapeSequence = IdentifyEscapeSequence(value);
		if (escapeSequence != null)
		{
			return escapeSequence.Unescape(value);
		}
		return value;
	}

	public static string UnescapeLiteral(string value, out bool isUnicode)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		EscapeSequence escapeSequence = IdentifyLiteralEscapeSequence(value);
		if (escapeSequence != null)
		{
			isUnicode = escapeSequence.startIndex == 1;
			return escapeSequence.Unescape(value);
		}
		isUnicode = false;
		return value;
	}

	public static EscapeSequence IdentifyEscapeSequence(string value)
	{
		if (BracketedEscapeSequence.Matches(value))
		{
			return BracketedEscapeSequence;
		}
		if (DoubleQuotedEscapeSequence.Matches(value))
		{
			return DoubleQuotedEscapeSequence;
		}
		if (SingleQuotedEscapeSequence.Matches(value))
		{
			return SingleQuotedEscapeSequence;
		}
		return null;
	}

	private static EscapeSequence IdentifyLiteralEscapeSequence(string value)
	{
		EscapeSequence result = null;
		if (value.Length > 0)
		{
			if (value[0] == 'N')
			{
				if (SingleQuotedSkipOneEscapeSequence.Matches(value))
				{
					result = SingleQuotedSkipOneEscapeSequence;
				}
				else if (DoubleQuotedSkipOneEscapeSequence.Matches(value))
				{
					result = DoubleQuotedSkipOneEscapeSequence;
				}
			}
			else if (SingleQuotedEscapeSequence.Matches(value))
			{
				result = SingleQuotedEscapeSequence;
			}
			else if (DoubleQuotedEscapeSequence.Matches(value))
			{
				result = DoubleQuotedEscapeSequence;
			}
		}
		return result;
	}
}
