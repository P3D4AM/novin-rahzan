using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Rendering;

namespace Serilog.Parsing;

public sealed class PropertyToken : MessageTemplateToken
{
	private readonly int? _position;

	public override int Length => RawText.Length;

	public string PropertyName { get; }

	public Destructuring Destructuring { get; }

	public string? Format { get; }

	public Alignment? Alignment { get; }

	public bool IsPositional => _position.HasValue;

	internal string RawText { get; }

	public PropertyToken(string propertyName, string rawText, string? format = null, in Alignment? alignment = null, Destructuring destructuring = Destructuring.Default)
	{
		PropertyName = Guard.AgainstNull(propertyName, "propertyName");
		Format = format;
		Destructuring = destructuring;
		RawText = Guard.AgainstNull(rawText, "rawText");
		Alignment = alignment;
		if (int.TryParse(PropertyName, NumberStyles.None, CultureInfo.InvariantCulture, out var result) && result >= 0)
		{
			_position = result;
		}
	}

	public override void Render(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, IFormatProvider? formatProvider = null)
	{
		Guard.AgainstNull(properties, "properties");
		Guard.AgainstNull(output, "output");
		MessageTemplateRenderer.RenderPropertyToken(this, properties, output, formatProvider, isLiteral: false, isJson: false);
	}

	public bool TryGetPositionalValue(out int position)
	{
		if (!_position.HasValue)
		{
			position = 0;
			return false;
		}
		position = _position.Value;
		return true;
	}

	public override bool Equals(object? obj)
	{
		if (obj is PropertyToken propertyToken && propertyToken.Destructuring == Destructuring && propertyToken.Format == Format && propertyToken.PropertyName == PropertyName)
		{
			return propertyToken.RawText == RawText;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return PropertyName.GetHashCode();
	}

	public override string ToString()
	{
		return RawText;
	}
}
