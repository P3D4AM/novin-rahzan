using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Rendering;

namespace Serilog.Parsing;

public sealed class TextToken : MessageTemplateToken
{
	public override int Length => Text.Length;

	public string Text { get; }

	public TextToken(string text)
	{
		Text = Guard.AgainstNull(text, "text");
	}

	public override void Render(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(output, "output");
		MessageTemplateRenderer.RenderTextToken(this, output);
	}

	public override bool Equals(object? obj)
	{
		if (obj is TextToken textToken)
		{
			return textToken.Text == Text;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public override string ToString()
	{
		return Text;
	}
}
