using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;

namespace Serilog.Parsing;

public abstract class MessageTemplateToken
{
	public abstract int Length { get; }

	public abstract void Render(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output, IFormatProvider? formatProvider = null);
}
