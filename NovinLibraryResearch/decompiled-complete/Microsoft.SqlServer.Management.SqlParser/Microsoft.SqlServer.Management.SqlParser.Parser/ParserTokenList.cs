using System;
using System.Collections.Generic;
using Babel.ParserGenerator;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal class ParserTokenList : IWriteableMinimalList<TokenData>, IMinimalList<TokenData>
{
	private readonly IMinimalList<TokenData> parentList;

	private readonly int eofStartIndex;

	private int rangeBegin;

	private int rangeEnd;

	private readonly Dictionary<int, TokenData> overrideValues;

	private readonly List<int> modifiedKeys;

	private int maxReadKey;

	public int MaxReadKey => maxReadKey;

	private int MaxModifiedKey
	{
		get
		{
			if (modifiedKeys.Count <= 0)
			{
				return -1;
			}
			return modifiedKeys[modifiedKeys.Count - 1];
		}
	}

	public int Count => parentList.Count;

	public TokenData this[int index]
	{
		get
		{
			maxReadKey = Math.Max(maxReadKey, index);
			TokenData value;
			if (index >= rangeEnd)
			{
				int startIndex = ((index < Count) ? parentList[index].StartIndex : eofStartIndex);
				value = new TokenData(128, startIndex);
			}
			else if (!overrideValues.TryGetValue(index, out value))
			{
				return parentList[index];
			}
			return value;
		}
		set
		{
			overrideValues[index] = value;
			modifiedKeys.Add(index);
		}
	}

	public ParserTokenList(IMinimalList<TokenData> parentList, int eofStartIndex)
	{
		this.parentList = parentList;
		this.eofStartIndex = eofStartIndex;
		rangeBegin = -1;
		rangeEnd = -1;
		overrideValues = new Dictionary<int, TokenData>();
		modifiedKeys = new List<int>();
		maxReadKey = -1;
	}

	public void SetCurrentRange(int rangeBegin, int rangeEnd)
	{
		this.rangeBegin = rangeBegin;
		this.rangeEnd = rangeEnd;
	}

	public void SetCurrentRange(LexLocation tokenRange)
	{
		SetCurrentRange(tokenRange.startTokenNumber, tokenRange.endTokenNumber);
	}

	public void CopyModifications(ModifiedMinimalList<TokenData> parserTokenList, int oldBeginIndex, int oldEndIndex, int newBeginIndex, int newEndIndex)
	{
		if (parserTokenList.ModifiedKeys.Count <= 0)
		{
			return;
		}
		int num = newBeginIndex - oldBeginIndex;
		int i = parserTokenList.ModifiedKeys.BinarySearch(oldBeginIndex);
		if (i < 0)
		{
			i = ~i;
		}
		for (; i < parserTokenList.ModifiedKeys.Count; i++)
		{
			int num2 = parserTokenList.ModifiedKeys[i];
			if (num2 < oldEndIndex)
			{
				TokenData tokenData = this[num2 + num];
				this[num2 + num] = new TokenData(parserTokenList.OverrideValues[num2].TokenId, tokenData.StartIndex);
				continue;
			}
			break;
		}
	}

	public IMinimalList<TokenData> CreateReadonlyMinimalList()
	{
		return new ModifiedMinimalList<TokenData>(parentList, overrideValues, modifiedKeys);
	}
}
