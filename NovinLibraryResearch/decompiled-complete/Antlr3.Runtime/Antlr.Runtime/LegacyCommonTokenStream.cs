using System;
using System.Collections.Generic;
using System.Text;

namespace Antlr.Runtime;

[Serializable]
public class LegacyCommonTokenStream : ITokenStream, IIntStream
{
	[NonSerialized]
	private ITokenSource _tokenSource;

	protected List<IToken> tokens;

	protected IDictionary<int, int> channelOverrideMap;

	protected List<int> discardSet;

	protected int channel;

	protected bool discardOffChannelTokens;

	protected int lastMarker;

	protected int p = -1;

	public virtual int Index => p;

	public virtual int Range { get; protected set; }

	public virtual int Count => tokens.Count;

	public virtual ITokenSource TokenSource => _tokenSource;

	public virtual string SourceName => TokenSource.SourceName;

	public LegacyCommonTokenStream()
	{
		tokens = new List<IToken>(500);
	}

	public LegacyCommonTokenStream(ITokenSource tokenSource)
		: this()
	{
		_tokenSource = tokenSource;
	}

	public LegacyCommonTokenStream(ITokenSource tokenSource, int channel)
		: this(tokenSource)
	{
		this.channel = channel;
	}

	public virtual void SetTokenSource(ITokenSource tokenSource)
	{
		_tokenSource = tokenSource;
		tokens.Clear();
		p = -1;
		channel = 0;
	}

	public virtual void FillBuffer()
	{
		if (p != -1)
		{
			return;
		}
		int num = 0;
		IToken token = _tokenSource.NextToken();
		while (token != null && token.Type != -1)
		{
			bool flag = false;
			if (channelOverrideMap != null && channelOverrideMap.TryGetValue(token.Type, out var value))
			{
				token.Channel = value;
			}
			if (discardSet != null && discardSet.Contains(token.Type))
			{
				flag = true;
			}
			else if (discardOffChannelTokens && token.Channel != channel)
			{
				flag = true;
			}
			if (!flag)
			{
				token.TokenIndex = num;
				tokens.Add(token);
				num++;
			}
			token = _tokenSource.NextToken();
		}
		p = 0;
		p = SkipOffTokenChannels(p);
	}

	public virtual void Consume()
	{
		if (p < tokens.Count)
		{
			p++;
			p = SkipOffTokenChannels(p);
		}
	}

	protected virtual int SkipOffTokenChannels(int i)
	{
		int count = tokens.Count;
		while (i < count && tokens[i].Channel != channel)
		{
			i++;
		}
		return i;
	}

	protected virtual int SkipOffTokenChannelsReverse(int i)
	{
		while (i >= 0 && tokens[i].Channel != channel)
		{
			i--;
		}
		return i;
	}

	public virtual void SetTokenTypeChannel(int ttype, int channel)
	{
		if (channelOverrideMap == null)
		{
			channelOverrideMap = new Dictionary<int, int>();
		}
		channelOverrideMap[ttype] = channel;
	}

	public virtual void DiscardTokenType(int ttype)
	{
		if (discardSet == null)
		{
			discardSet = new List<int>();
		}
		discardSet.Add(ttype);
	}

	public virtual void SetDiscardOffChannelTokens(bool discardOffChannelTokens)
	{
		this.discardOffChannelTokens = discardOffChannelTokens;
	}

	public virtual IList<IToken> GetTokens()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		return tokens;
	}

	public virtual IList<IToken> GetTokens(int start, int stop)
	{
		return GetTokens(start, stop, (BitSet)null);
	}

	public virtual IList<IToken> GetTokens(int start, int stop, BitSet types)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		if (stop >= tokens.Count)
		{
			stop = tokens.Count - 1;
		}
		if (start < 0)
		{
			start = 0;
		}
		if (start > stop)
		{
			return null;
		}
		IList<IToken> list = new List<IToken>();
		for (int i = start; i <= stop; i++)
		{
			IToken token = tokens[i];
			if (types == null || types.Member(token.Type))
			{
				list.Add(token);
			}
		}
		if (list.Count == 0)
		{
			list = null;
		}
		return list;
	}

	public virtual IList<IToken> GetTokens(int start, int stop, IList<int> types)
	{
		return GetTokens(start, stop, new BitSet(types));
	}

	public virtual IList<IToken> GetTokens(int start, int stop, int ttype)
	{
		return GetTokens(start, stop, BitSet.Of(ttype));
	}

	public virtual IToken LT(int k)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		if (k == 0)
		{
			return null;
		}
		if (k < 0)
		{
			return LB(-k);
		}
		if (p + k - 1 >= tokens.Count)
		{
			return tokens[tokens.Count - 1];
		}
		int num = p;
		for (int i = 1; i < k; i++)
		{
			num = SkipOffTokenChannels(num + 1);
		}
		if (num >= tokens.Count)
		{
			return tokens[tokens.Count - 1];
		}
		if (num > Range)
		{
			Range = num;
		}
		return tokens[num];
	}

	protected virtual IToken LB(int k)
	{
		if (p == -1)
		{
			FillBuffer();
		}
		if (k == 0)
		{
			return null;
		}
		if (p - k < 0)
		{
			return null;
		}
		int num = p;
		for (int i = 1; i <= k; i++)
		{
			num = SkipOffTokenChannelsReverse(num - 1);
		}
		if (num < 0)
		{
			return null;
		}
		return tokens[num];
	}

	public virtual IToken Get(int i)
	{
		return tokens[i];
	}

	public virtual int LA(int i)
	{
		return LT(i).Type;
	}

	public virtual int Mark()
	{
		if (p == -1)
		{
			FillBuffer();
		}
		lastMarker = Index;
		return lastMarker;
	}

	public virtual void Release(int marker)
	{
	}

	public virtual void Rewind(int marker)
	{
		Seek(marker);
	}

	public virtual void Rewind()
	{
		Seek(lastMarker);
	}

	public virtual void Reset()
	{
		p = 0;
		lastMarker = 0;
	}

	public virtual void Seek(int index)
	{
		p = index;
	}

	public override string ToString()
	{
		if (p == -1)
		{
			throw new InvalidOperationException("Buffer is not yet filled.");
		}
		return ToString(0, tokens.Count - 1);
	}

	public virtual string ToString(int start, int stop)
	{
		if (start < 0 || stop < 0)
		{
			return null;
		}
		if (p == -1)
		{
			throw new InvalidOperationException("Buffer is not yet filled.");
		}
		if (stop >= tokens.Count)
		{
			stop = tokens.Count - 1;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i <= stop; i++)
		{
			IToken token = tokens[i];
			stringBuilder.Append(token.Text);
		}
		return stringBuilder.ToString();
	}

	public virtual string ToString(IToken start, IToken stop)
	{
		if (start != null && stop != null)
		{
			return ToString(start.TokenIndex, stop.TokenIndex);
		}
		return null;
	}
}
