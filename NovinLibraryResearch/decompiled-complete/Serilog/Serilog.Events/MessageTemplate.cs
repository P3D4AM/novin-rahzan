using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Debugging;
using Serilog.Parsing;
using Serilog.Rendering;

namespace Serilog.Events;

public class MessageTemplate
{
	public static MessageTemplate Empty { get; } = new MessageTemplate(Enumerable.Empty<MessageTemplateToken>());


	public string Text { get; }

	public IEnumerable<MessageTemplateToken> Tokens => TokenArray;

	internal MessageTemplateToken[] TokenArray { get; }

	internal PropertyToken[]? NamedProperties { get; }

	internal PropertyToken[]? PositionalProperties { get; }

	public MessageTemplate(IEnumerable<MessageTemplateToken> tokens)
		: this(string.Concat(tokens), tokens)
	{
	}

	public MessageTemplate(string text, IEnumerable<MessageTemplateToken> tokens)
	{
		Text = Guard.AgainstNull(text, "text");
		TokenArray = Guard.AgainstNull(tokens, "tokens").ToArray();
		PropertyToken[] elementsOfTypeToArray = GetElementsOfTypeToArray<PropertyToken>(TokenArray);
		if (elementsOfTypeToArray.Length == 0)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		PropertyToken[] array = elementsOfTypeToArray;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].IsPositional)
			{
				flag2 = true;
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			PositionalProperties = elementsOfTypeToArray;
			return;
		}
		if (flag2)
		{
			SelfLog.WriteLine("Message template is malformed: {0}", text);
		}
		NamedProperties = elementsOfTypeToArray;
	}

	private static TResult[] GetElementsOfTypeToArray<TResult>(MessageTemplateToken[] tokens) where TResult : class
	{
		List<TResult> list = new List<TResult>(tokens.Length / 2);
		for (int i = 0; i < tokens.Length; i++)
		{
			if (tokens[i] is TResult item)
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public override string ToString()
	{
		return Text;
	}

	public string Render(IReadOnlyDictionary<string, LogEventPropertyValue> properties, IFormatProvider? formatProvider = null)
	{
		using StringWriter stringWriter = ReusableStringWriter.GetOrCreate(formatProvider);
		Render(properties, stringWriter, formatProvider);
		return stringWriter.ToString();
	}

	public void Render(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(properties, "properties");
		Guard.AgainstNull(output, "output");
		MessageTemplateRenderer.Render(this, properties, output, null, formatProvider);
	}
}
