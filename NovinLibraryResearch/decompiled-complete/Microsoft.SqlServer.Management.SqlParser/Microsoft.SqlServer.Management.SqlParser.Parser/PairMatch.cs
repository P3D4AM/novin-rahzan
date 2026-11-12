using System.Globalization;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class PairMatch
{
	public readonly Token StartToken;

	public readonly Token EndToken;

	public readonly bool IsStartEnd;

	internal PairMatch(Token startToken, Token endToken, bool isStartEnd)
	{
		StartToken = startToken;
		EndToken = endToken;
		IsStartEnd = isStartEnd;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "[{0}]({1}, {2}) x [{3}]({4}, {5})", StartToken.Type, StartToken.StartLocation, StartToken.EndLocation, EndToken.Type, EndToken.StartLocation, EndToken.EndLocation);
	}
}
