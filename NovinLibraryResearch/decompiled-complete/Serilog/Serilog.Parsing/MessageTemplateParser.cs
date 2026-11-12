using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Parsing;

public class MessageTemplateParser : IMessageTemplateParser
{
	private static TextToken emptyTextToken = new TextToken("");

	public MessageTemplate Parse(string messageTemplate)
	{
		Guard.AgainstNull(messageTemplate, "messageTemplate");
		return new MessageTemplate(messageTemplate, Tokenize(messageTemplate));
	}

	private static IEnumerable<MessageTemplateToken> Tokenize(string messageTemplate)
	{
		if (messageTemplate.Length == 0)
		{
			yield return emptyTextToken;
			yield break;
		}
		int nextIndex = 0;
		do
		{
			int num = nextIndex;
			TextToken textToken = ParseTextToken(nextIndex, messageTemplate, out nextIndex);
			if (nextIndex > num)
			{
				yield return textToken;
			}
			if (nextIndex == messageTemplate.Length)
			{
				break;
			}
			int num2 = nextIndex;
			MessageTemplateToken messageTemplateToken = ParsePropertyToken(nextIndex, messageTemplate, out nextIndex);
			if (num2 < nextIndex)
			{
				yield return messageTemplateToken;
			}
		}
		while (nextIndex != messageTemplate.Length);
	}

	private static MessageTemplateToken ParsePropertyToken(int startAt, string messageTemplate, out int next)
	{
		int num = startAt;
		startAt++;
		while (startAt < messageTemplate.Length && IsValidInPropertyTag(messageTemplate[startAt]))
		{
			startAt++;
		}
		if (startAt == messageTemplate.Length || messageTemplate[startAt] != '}')
		{
			next = startAt;
			return new TextToken(messageTemplate.Substring(num, next - num));
		}
		next = startAt + 1;
		string text = messageTemplate.Substring(num, next - num);
		string text2 = text.Substring(1, next - (num + 2));
		if (text2.Length == 0)
		{
			return new TextToken(text);
		}
		if (!TrySplitTagContent(text2, out string propertyNameAndDestructuring, out string format, out string alignment))
		{
			return new TextToken(text);
		}
		string text3 = propertyNameAndDestructuring;
		Destructuring destructuring = Destructuring.Default;
		if (text3.Length != 0 && TryGetDestructuringHint(text3[0], out destructuring))
		{
			text3 = text3.Substring(1);
		}
		if (text3.Length == 0)
		{
			return new TextToken(text);
		}
		for (int i = 0; i < text3.Length; i++)
		{
			if (!IsValidInPropertyName(text3[i]))
			{
				return new TextToken(text);
			}
		}
		if (format != null)
		{
			for (int j = 0; j < format.Length; j++)
			{
				if (!IsValidInFormat(format[j]))
				{
					return new TextToken(text);
				}
			}
		}
		Alignment? alignment2 = null;
		if (alignment != null)
		{
			for (int k = 0; k < alignment.Length; k++)
			{
				if (!IsValidInAlignment(alignment[k]))
				{
					return new TextToken(text);
				}
			}
			int num2 = alignment.LastIndexOf('-');
			if (num2 > 0)
			{
				return new TextToken(text);
			}
			if (!int.TryParse((num2 == -1) ? alignment : alignment.Substring(1), out var result) || result == 0)
			{
				return new TextToken(text);
			}
			AlignmentDirection direction = ((num2 == -1) ? AlignmentDirection.Right : AlignmentDirection.Left);
			alignment2 = new Alignment(direction, result);
		}
		return new PropertyToken(text3, text, format, in alignment2, destructuring);
	}

	private static bool TrySplitTagContent(string tagContent, [NotNullWhen(true)] out string? propertyNameAndDestructuring, out string? format, out string? alignment)
	{
		int num = tagContent.IndexOf(':');
		int num2 = tagContent.IndexOf(',');
		if (num == -1 && num2 == -1)
		{
			propertyNameAndDestructuring = tagContent;
			format = null;
			alignment = null;
			return true;
		}
		if (num2 == -1 || (num != -1 && num2 > num))
		{
			propertyNameAndDestructuring = tagContent.Substring(0, num);
			format = ((num == tagContent.Length - 1) ? null : tagContent.Substring(num + 1));
			alignment = null;
			return true;
		}
		propertyNameAndDestructuring = tagContent.Substring(0, num2);
		if (num == -1)
		{
			if (num2 == tagContent.Length - 1)
			{
				alignment = (format = null);
				return false;
			}
			format = null;
			alignment = tagContent.Substring(num2 + 1);
			return true;
		}
		if (num2 == num - 1)
		{
			alignment = (format = null);
			return false;
		}
		alignment = tagContent.Substring(num2 + 1, num - num2 - 1);
		format = ((num == tagContent.Length - 1) ? null : tagContent.Substring(num + 1));
		return true;
	}

	private static bool IsValidInPropertyTag(char c)
	{
		if (!IsValidInDestructuringHint(c) && !IsValidInPropertyName(c) && !IsValidInFormat(c))
		{
			return c == ':';
		}
		return true;
	}

	private static bool IsValidInPropertyName(char c)
	{
		if (!char.IsLetterOrDigit(c))
		{
			return c == '_';
		}
		return true;
	}

	private static bool TryGetDestructuringHint(char c, out Destructuring destructuring)
	{
		switch (c)
		{
		case '@':
			destructuring = Destructuring.Destructure;
			return true;
		case '$':
			destructuring = Destructuring.Stringify;
			return true;
		default:
			destructuring = Destructuring.Default;
			return false;
		}
	}

	private static bool IsValidInDestructuringHint(char c)
	{
		if (c == '$' || c == '@')
		{
			return true;
		}
		return false;
	}

	private static bool IsValidInAlignment(char c)
	{
		if (!char.IsDigit(c))
		{
			return c == '-';
		}
		return true;
	}

	private static bool IsValidInFormat(char c)
	{
		bool flag = c != '}';
		if (flag)
		{
			bool flag2 = char.IsLetterOrDigit(c) || char.IsPunctuation(c);
			if (!flag2)
			{
				bool flag3 = ((c == ' ' || c == '+') ? true : false);
				flag2 = flag3;
			}
			flag = flag2;
		}
		return flag;
	}

	private static TextToken ParseTextToken(int startAt, string messageTemplate, out int next)
	{
		StringBuilder stringBuilder = new StringBuilder();
		do
		{
			char c = messageTemplate[startAt];
			if (c == '{')
			{
				if (startAt + 1 >= messageTemplate.Length || messageTemplate[startAt + 1] != '{')
				{
					break;
				}
				stringBuilder.Append(c);
				startAt++;
			}
			else
			{
				stringBuilder.Append(c);
				if (c == '}' && startAt + 1 < messageTemplate.Length && messageTemplate[startAt + 1] == '}')
				{
					startAt++;
				}
			}
			startAt++;
		}
		while (startAt < messageTemplate.Length);
		next = startAt;
		return new TextToken(stringBuilder.ToString());
	}
}
