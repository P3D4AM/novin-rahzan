using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal static class EncodeExtensions
{
	private const string WindowsNewLine = "\\\\r\\\\n";

	private const string OtherNewLine = "\\\\n";

	public static IEnumerable<string> WithJsonNewLines(this IEnumerable<string> lines)
	{
		string newLineEscape = ((Environment.NewLine == "\r\n") ? "\\\\r\\\\n" : "\\\\n");
		return lines.Select((string l) => l.Replace(Environment.NewLine, newLineEscape));
	}
}
