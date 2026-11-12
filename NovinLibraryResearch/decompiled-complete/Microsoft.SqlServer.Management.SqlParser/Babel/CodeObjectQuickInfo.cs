using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel;

public class CodeObjectQuickInfo
{
	public readonly string Text;

	public readonly Location StartLocation;

	public readonly Location EndLocation;

	internal CodeObjectQuickInfo(string text, Location startLocation, Location endLocation)
	{
		Text = text;
		StartLocation = startLocation;
		EndLocation = endLocation;
	}
}
