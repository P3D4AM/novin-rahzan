using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal static class LineFeedExtension
{
	public static IEnumerable<string> WithLineFeeds(this IEnumerable<string> lines)
	{
		return lines.Select((string l) => (!l.EndsWith(Environment.NewLine)) ? (l + Environment.NewLine) : l);
	}
}
