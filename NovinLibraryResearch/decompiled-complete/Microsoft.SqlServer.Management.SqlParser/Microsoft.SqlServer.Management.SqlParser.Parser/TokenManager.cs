using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class TokenManager
{
	private class IndexComparer : IComparer<TokenData>
	{
		public int Compare(TokenData x, TokenData y)
		{
			int startIndex = x.StartIndex;
			return startIndex.CompareTo(y.StartIndex);
		}
	}

	private struct PairTokenId
	{
		public readonly int Start;

		public readonly int End;

		public PairTokenId(int start, int end)
		{
			Start = start;
			End = end;
		}
	}

	private string source;

	private IMinimalList<TokenData> tokens;

	private List<int> newLines;

	private List<int> batchSeparators;

	private int compatibilityLevel;

	private static readonly IndexComparer indexComparer = new IndexComparer();

	private const int TokenLeftParen = 40;

	private const int TokenRightParen = 41;

	private const int TokenBegin = 206;

	private const int TokenEnd = 233;

	private const int TokenBeginCS = 978;

	private const int TokenEndCS = 979;

	private const int TokenTryBegin = 374;

	private const int TokenTryEnd = 462;

	private const int TokenCatchBegin = 373;

	private const int TokenCatchEnd = 460;

	private static readonly PairTokenId PairIdParens = new PairTokenId(40, 41);

	private static readonly PairTokenId PairIdBeginEnd = new PairTokenId(978, 979);

	private static readonly PairTokenId PairIdTryBeginEnd = new PairTokenId(374, 462);

	private static readonly PairTokenId PairIdCatchBeginEnd = new PairTokenId(373, 460);

	public string Source => source;

	public IMinimalList<TokenData> TokenList => tokens;

	public List<int> NewLines => newLines;

	public List<int> BatchSeparators => batchSeparators;

	public int CompatibilityLevel => compatibilityLevel;

	public int Count => tokens.Count;

	public TokenManager(string source, IMinimalList<TokenData> tokens, List<int> newLines, List<int> batchSeparators, int compatibilityLevel)
	{
		this.source = source;
		this.tokens = tokens;
		this.newLines = newLines;
		this.batchSeparators = batchSeparators;
		this.compatibilityLevel = compatibilityLevel;
	}

	public int GetBatchCount()
	{
		return batchSeparators.Count + 1;
	}

	public LexLocation GetBatchRange(int batchNumber)
	{
		int startTokenNumber = ((batchNumber != 0) ? (batchSeparators[batchNumber - 1] + 1) : 0);
		int endTokenNumber = ((batchNumber >= batchSeparators.Count) ? tokens.Count : batchSeparators[batchNumber]);
		return new LexLocation(startTokenNumber, endTokenNumber);
	}

	internal static bool IsSignificantToken(int id)
	{
		if ((uint)(id - 986) <= 2u)
		{
			return false;
		}
		return true;
	}

	internal static bool IsSingleCharacterToken(int id)
	{
		if (id >= 128)
		{
			return false;
		}
		if (id <= 0)
		{
			return false;
		}
		return true;
	}

	internal static bool IsCompoundOperatorToken(int id)
	{
		if (id == 187 || (uint)(id - 190) <= 7u)
		{
			return true;
		}
		return false;
	}

	public string GetText(int startTokenNumber, int endTokenNumber)
	{
		if (startTokenNumber == endTokenNumber)
		{
			return string.Empty;
		}
		int startIndex = tokens[startTokenNumber].StartIndex;
		if (endTokenNumber == tokens.Count)
		{
			return source.Substring(startIndex);
		}
		int startIndex2 = tokens[endTokenNumber].StartIndex;
		return source.Substring(startIndex, startIndex2 - startIndex);
	}

	public string GetText(LexLocation loc)
	{
		return GetText(loc.startTokenNumber, loc.endTokenNumber);
	}

	internal int GetId(int tokenNumber)
	{
		return tokens[tokenNumber].TokenId;
	}

	private int GetOriginalId(int tokenNumber)
	{
		int id = GetId(tokenNumber);
		if (id >= 0)
		{
			return id;
		}
		return ~id;
	}

	private bool IsDead(int tokenNumber)
	{
		return GetId(tokenNumber) < 0;
	}

	public string GetText(int tokenNumber)
	{
		return GetText(tokenNumber, tokenNumber + 1);
	}

	internal StringToken GetStringToken(int tokenNumber)
	{
		int startIndex = tokens[tokenNumber].StartIndex;
		int num = ((tokenNumber == tokens.Count - 1) ? source.Length : tokens[tokenNumber + 1].StartIndex);
		return StringTokenLookup.Find(source, startIndex, num - startIndex);
	}

	internal int GetLineNumber(int charIndex)
	{
		int num = newLines.BinarySearch(charIndex, Comparer<int>.Default);
		if (num < 0)
		{
			num = ~num;
		}
		return num;
	}

	internal int GetColumnNumber(int charIndex, int line)
	{
		return charIndex - newLines[line - 1];
	}

	internal int GetLastCaretColumnNumber(int line)
	{
		return newLines[line] - newLines[line - 1] - 1;
	}

	internal Location GetLocation(int tokenNumber)
	{
		int num;
		int num2;
		if (tokenNumber < tokens.Count)
		{
			num = tokens[tokenNumber].StartIndex;
			num2 = GetLineNumber(num);
		}
		else
		{
			num = source.Length;
			num2 = newLines.Count - 1;
		}
		return new Location(num2, GetColumnNumber(num, num2), num);
	}

	public bool IsTokenComment(int tokenId)
	{
		if (tokenId != 987)
		{
			return tokenId == 988;
		}
		return true;
	}

	public bool IsTokenWhitespace(int tokenId)
	{
		return tokenId == 986;
	}

	internal bool IsTokenTrueMultiLineComment(int tokenId, Location startLocation, Location endLocation)
	{
		if (tokenId == 988)
		{
			return startLocation.LineNumber != endLocation.LineNumber;
		}
		return false;
	}

	public Token GetToken(int tokenNumber)
	{
		return new Token(GetId(tokenNumber), GetText(tokenNumber), GetLocation(tokenNumber), GetLocation(tokenNumber + 1));
	}

	public int FindToken(int line, int col)
	{
		if (line >= newLines.Count)
		{
			line = newLines.Count - 1;
		}
		int num = newLines[line - 1];
		int num2 = newLines[line];
		int num3 = num + col;
		if (num3 > num2)
		{
			num3 = num2 - 1;
		}
		if (num3 > 0)
		{
			num3--;
		}
		int num4 = MinimalListUtils.BinarySearch(tokens, new TokenData(-1, num3), indexComparer);
		if (num4 < 0)
		{
			num4 = ~num4;
			num4--;
		}
		return num4;
	}

	internal string RetrieveStatementName(LexLocation lexLocation)
	{
		string text = GetText(lexLocation.startTokenNumber);
		for (int i = lexLocation.startTokenNumber + 1; i < lexLocation.endTokenNumber; i++)
		{
			int originalId = GetOriginalId(i);
			if (IsSignificantToken(originalId))
			{
				if (!IsSingleCharacterToken(originalId) && originalId != 173 && originalId != 175 && originalId != 183)
				{
					text = text + " " + GetText(i);
				}
				break;
			}
		}
		return text.ToUpperInvariant();
	}

	internal LexLocation GetCaretLexLocation(int line, int col)
	{
		LexLocation result = null;
		int num = FindToken(line, col);
		if (num >= 0)
		{
			int id = GetId(num);
			if (IsSingleCharacterToken(id))
			{
				result = new LexLocation(num + 1, num + 1);
			}
			else if (IsCompoundOperatorToken(id))
			{
				int num2 = num + 1;
				Location location = GetLocation(num2);
				if (location.ColumnNumber == col && location.LineNumber == line)
				{
					result = new LexLocation(num2, num2);
				}
			}
			else if (IsSignificantToken(id))
			{
				result = new LexLocation(num, num + 1);
			}
			else
			{
				switch (id)
				{
				case 988:
				{
					int tokenNumber = num + 1;
					Location location2 = GetLocation(tokenNumber);
					if (location2.ColumnNumber == col && location2.LineNumber == line)
					{
						int previousSignificantTokenIndex2 = GetPreviousSignificantTokenIndex(num);
						if (previousSignificantTokenIndex2 >= 0)
						{
							result = new LexLocation(previousSignificantTokenIndex2 + 1, previousSignificantTokenIndex2 + 1);
						}
					}
					break;
				}
				case 986:
				{
					int previousSignificantTokenIndex = GetPreviousSignificantTokenIndex(num);
					if (previousSignificantTokenIndex >= 0)
					{
						num = previousSignificantTokenIndex + 1;
					}
					result = new LexLocation(num, num);
					break;
				}
				}
			}
		}
		return result;
	}

	internal LexLocation GetAltCaretLexLocation(LexLocation caretLocation)
	{
		LexLocation result = null;
		int startTokenNumber = caretLocation.startTokenNumber;
		if (!IsSignificantToken(GetId(startTokenNumber)))
		{
			int nextSignificantTokenIndex = GetNextSignificantTokenIndex(startTokenNumber);
			if (nextSignificantTokenIndex > 0)
			{
				result = new LexLocation(nextSignificantTokenIndex, nextSignificantTokenIndex);
			}
		}
		return result;
	}

	internal LexLocation GetCursorLocation(int line, int col)
	{
		LexLocation result = null;
		int num = FindToken(line, col);
		if (num >= 0)
		{
			int id = GetId(num);
			if (IsSingleCharacterToken(id))
			{
				int num2 = num + 1;
				if (num2 < tokens.Count)
				{
					num = num2;
					id = GetId(num2);
				}
			}
			else if (!IsSignificantToken(id))
			{
				int num3 = num + 1;
				if (num3 < tokens.Count)
				{
					Location location = GetLocation(num3);
					if (location.LineNumber == line && location.ColumnNumber == col)
					{
						num = num3;
						id = GetId(num);
					}
				}
			}
			if (IsSignificantToken(id) && (!IsSingleCharacterToken(id) || id == 42))
			{
				result = new LexLocation(num, num + 1);
			}
		}
		return result;
	}

	internal int FindTokenInRange(int id, int beginIndex, int endIndex)
	{
		for (int i = beginIndex; i < endIndex; i++)
		{
			if (GetId(i) == id)
			{
				return i;
			}
		}
		return -1;
	}

	internal int GetNextSignificantTokenIndex(int tokenIndex)
	{
		int result = -1;
		for (int i = tokenIndex + 1; i < tokens.Count; i++)
		{
			int tokenId = tokens[i].TokenId;
			if (tokenId == 989)
			{
				break;
			}
			if (IsSignificantToken(tokenId))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public int GetPreviousSignificantTokenIndex(int tokenIndex)
	{
		int result = -1;
		for (int num = tokenIndex - 1; num >= 0; num--)
		{
			int tokenId = tokens[num].TokenId;
			if (tokenId == 989)
			{
				break;
			}
			if (IsSignificantToken(tokenId))
			{
				result = num;
				break;
			}
		}
		return result;
	}

	public int GetTokenStartIndex(int index)
	{
		if (index >= tokens.Count)
		{
			return source.Length;
		}
		return tokens[index].StartIndex;
	}

	public int GetTokenEndIndex(int index)
	{
		return GetTokenStartIndex(index + 1);
	}

	public PairMatch FindPairMatch(int line, int column)
	{
		int num = FindToken(line, column);
		if (num < 0)
		{
			return null;
		}
		int num2 = GetNextSignificantTokenIndex(num);
		if (num2 > 0)
		{
			switch (GetId(num2))
			{
			case 373:
			case 374:
			case 460:
			case 462:
				num = num2;
				num2 = -1;
				break;
			case 40:
				if (GetId(num) == 40 && num2 == num + 1)
				{
					num = num2;
					num2 = -1;
				}
				break;
			}
		}
		PairMatch pairMatch = FindPairMatch(num);
		if (pairMatch == null && num2 == num + 1)
		{
			switch (GetOriginalId(num2))
			{
			case 40:
			case 41:
			case 206:
			case 233:
			case 978:
			case 979:
			{
				Location location = GetLocation(num2);
				if (location.LineNumber == line && location.ColumnNumber == column)
				{
					if (IsDead(num2))
					{
						num2 = GetNextSignificantTokenIndex(num2);
					}
					pairMatch = FindPairMatch(num2);
				}
				break;
			}
			}
		}
		return pairMatch;
	}

	private PairMatch FindPairMatch(int tokenNumber)
	{
		int id = GetId(tokenNumber);
		int num = -1;
		int num2 = -1;
		bool isStartEnd = false;
		bool flag = false;
		switch (id)
		{
		case 40:
			isStartEnd = true;
			num = tokenNumber;
			num2 = FindPairEndToken(tokenNumber, PairIdParens);
			break;
		case 41:
			num2 = tokenNumber;
			num = FindPairStartToken(tokenNumber, PairIdParens);
			break;
		case 978:
			isStartEnd = true;
			num = tokenNumber;
			num2 = FindPairEndToken(num, PairIdBeginEnd);
			break;
		case 979:
			num2 = tokenNumber;
			num = FindPairStartToken(num2, PairIdBeginEnd);
			break;
		case 374:
			isStartEnd = true;
			flag = true;
			num = tokenNumber;
			num2 = FindPairEndToken(num, PairIdTryBeginEnd);
			break;
		case 462:
			flag = true;
			num2 = tokenNumber;
			num = FindPairStartToken(num2, PairIdTryBeginEnd);
			break;
		case 373:
			isStartEnd = true;
			flag = true;
			num = tokenNumber;
			num2 = FindPairEndToken(num, PairIdCatchBeginEnd);
			break;
		case 460:
			flag = true;
			num2 = tokenNumber;
			num = FindPairStartToken(num2, PairIdCatchBeginEnd);
			break;
		}
		if (!flag)
		{
			return MakePairMatch(num, num2, isStartEnd);
		}
		return MakeTryOrCatchPairMatch(num, num2, isStartEnd);
	}

	private int FindPairEndToken(int tokenNumber, PairTokenId pairId)
	{
		int result = -1;
		int num = 0;
		for (int i = tokenNumber + 1; i < tokens.Count; i++)
		{
			int tokenId = tokens[i].TokenId;
			if (tokenId == 989)
			{
				break;
			}
			if (tokenId == pairId.End)
			{
				if (num == 0)
				{
					result = i;
					break;
				}
				num--;
			}
			else if (tokenId == pairId.Start)
			{
				num++;
			}
		}
		return result;
	}

	private int FindPairStartToken(int tokenNumber, PairTokenId pairId)
	{
		int result = -1;
		int num = 0;
		for (int num2 = tokenNumber - 1; num2 >= 0; num2--)
		{
			int tokenId = tokens[num2].TokenId;
			if (tokenId == 989)
			{
				break;
			}
			if (tokenId == pairId.Start)
			{
				if (num == 0)
				{
					result = num2;
					break;
				}
				num--;
			}
			else if (tokenId == pairId.End)
			{
				num++;
			}
		}
		return result;
	}

	private PairMatch MakePairMatch(int startTokenNumber, int endTokenNumber, bool isStartEnd)
	{
		PairMatch result = null;
		if (startTokenNumber >= 0 && endTokenNumber >= 0)
		{
			result = new PairMatch(GetToken(startTokenNumber), GetToken(endTokenNumber), isStartEnd);
		}
		return result;
	}

	private PairMatch MakeTryOrCatchPairMatch(int startTokenNumber, int endTokenNumber, bool isStartEnd)
	{
		PairMatch result = null;
		if (startTokenNumber >= 0 && endTokenNumber >= 0)
		{
			int previousSignificantTokenIndex = GetPreviousSignificantTokenIndex(startTokenNumber);
			int previousSignificantTokenIndex2 = GetPreviousSignificantTokenIndex(endTokenNumber);
			Token token = GetToken(previousSignificantTokenIndex);
			Token token2 = GetToken(startTokenNumber);
			Token startToken = new Token(token2.Id, string.Empty, token.StartLocation, token2.EndLocation);
			Token token3 = GetToken(previousSignificantTokenIndex2);
			Token token4 = GetToken(endTokenNumber);
			Token endToken = new Token(token4.Id, string.Empty, token3.StartLocation, token4.EndLocation);
			result = new PairMatch(startToken, endToken, isStartEnd);
		}
		return result;
	}

	private static bool CompareTokens(TokenManager src1, int i1, TokenManager src2, int i2)
	{
		if (src1.tokens[i1].TokenId != src2.tokens[i2].TokenId)
		{
			return false;
		}
		int tokenStartIndex = src1.GetTokenStartIndex(i1);
		int tokenStartIndex2 = src1.GetTokenStartIndex(i1 + 1);
		int tokenStartIndex3 = src2.GetTokenStartIndex(i2);
		int tokenStartIndex4 = src2.GetTokenStartIndex(i2 + 1);
		if (tokenStartIndex2 - tokenStartIndex != tokenStartIndex4 - tokenStartIndex3)
		{
			return false;
		}
		int num = tokenStartIndex;
		int num2 = tokenStartIndex3;
		while (num < tokenStartIndex2)
		{
			if (src1.source[num] != src2.source[num2])
			{
				return false;
			}
			num++;
			num2++;
		}
		return true;
	}

	internal static TokenUpdateDiff CalculateTokenUpdateDiff(TokenManager source, TokenManager target)
	{
		int i = 0;
		int num;
		for (num = Math.Min(source.tokens.Count, target.tokens.Count); i < num && CompareTokens(source, i, target, i); i++)
		{
		}
		if (i == source.tokens.Count && i == target.tokens.Count)
		{
			return TokenUpdateDiff.Empty;
		}
		num -= i;
		int j;
		for (j = 0; j < num && CompareTokens(source, source.tokens.Count - 1 - j, target, target.tokens.Count - 1 - j); j++)
		{
		}
		return new TokenUpdateDiff(i, source.tokens.Count - j, target.tokens.Count - j);
	}
}
