using System.Globalization;

namespace Babel.ParserGenerator;

public struct TokenData
{
	public readonly int StartIndex;

	public readonly int TokenId;

	public TokenData(int id, int startIndex)
	{
		StartIndex = startIndex;
		TokenId = id;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.CurrentCulture, "({0}, {1})", StartIndex, TokenId);
	}
}
