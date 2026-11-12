using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class Scanner
{
	private Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner scanner;

	public Scanner(ParseOptions options)
	{
		scanner = new Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner(options);
	}

	public void SetSource(string source, int offset)
	{
		scanner.SetSource(source, offset);
	}

	public int GetNext(ref int state, out int start, out int end, out bool isPairMatch, out bool isExecAutoParamHelp)
	{
		int next;
		do
		{
			next = scanner.GetNext(ref state, out start, out end, out isPairMatch, out isExecAutoParamHelp);
			if (isPairMatch | isExecAutoParamHelp)
			{
				return next;
			}
		}
		while (next == 986);
		return next;
	}
}
