using System.Globalization;
using Babel.ParserGenerator;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal class TokenUpdateDiff
{
	public static readonly TokenUpdateDiff Empty = new TokenUpdateDiff(0, 0, 0);

	private readonly int startIndex;

	private readonly int oldEndIndex;

	private readonly int newEndIndex;

	public int StartIndex => startIndex;

	public int OldEndIndex => oldEndIndex;

	public int NewEndIndex => newEndIndex;

	public bool IsEmpty
	{
		get
		{
			if (startIndex == oldEndIndex)
			{
				return startIndex == newEndIndex;
			}
			return false;
		}
	}

	public TokenUpdateDiff(int startIndex, int oldEndIndex, int newEndIndex)
	{
		this.startIndex = startIndex;
		this.oldEndIndex = oldEndIndex;
		this.newEndIndex = newEndIndex;
	}

	public int FixTokenIndex(int index)
	{
		if (index < startIndex)
		{
			return index;
		}
		if (index >= oldEndIndex)
		{
			return index - oldEndIndex + newEndIndex;
		}
		return -1;
	}

	public int UnfixTokenIndex(int index)
	{
		if (index < startIndex)
		{
			return index;
		}
		if (index >= newEndIndex)
		{
			return index - newEndIndex + oldEndIndex;
		}
		return -1;
	}

	public LexLocation FixLexLocation(LexLocation pos)
	{
		if (pos.endTokenNumber <= startIndex && pos.startTokenNumber < startIndex)
		{
			return pos;
		}
		if (pos.startTokenNumber >= oldEndIndex && pos.endTokenNumber > oldEndIndex)
		{
			return new LexLocation(pos.startTokenNumber + newEndIndex - oldEndIndex, pos.endTokenNumber + newEndIndex - oldEndIndex);
		}
		if (pos.startTokenNumber == oldEndIndex && pos.endTokenNumber == oldEndIndex)
		{
			return new LexLocation(newEndIndex, newEndIndex);
		}
		if (pos.startTokenNumber == startIndex && pos.endTokenNumber == startIndex)
		{
			return pos;
		}
		return new LexLocation(-1, -1);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}:{1}-{2}", startIndex, oldEndIndex, newEndIndex);
	}
}
