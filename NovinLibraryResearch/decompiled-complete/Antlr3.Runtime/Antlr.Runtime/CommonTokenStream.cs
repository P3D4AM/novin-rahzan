using System;

namespace Antlr.Runtime;

[Serializable]
public class CommonTokenStream : BufferedTokenStream
{
	private int _channel;

	public int Channel => _channel;

	public override ITokenSource TokenSource
	{
		get
		{
			return base.TokenSource;
		}
		set
		{
			base.TokenSource = value;
			_channel = 0;
		}
	}

	public CommonTokenStream()
	{
	}

	public CommonTokenStream(ITokenSource tokenSource)
		: this(tokenSource, 0)
	{
	}

	public CommonTokenStream(ITokenSource tokenSource, int channel)
		: base(tokenSource)
	{
		_channel = channel;
	}

	public override void Consume()
	{
		if (_p == -1)
		{
			Setup();
		}
		_p++;
		_p = SkipOffTokenChannels(_p);
	}

	protected override IToken LB(int k)
	{
		if (k == 0 || _p - k < 0)
		{
			return null;
		}
		int num = _p;
		for (int i = 1; i <= k; i++)
		{
			num = SkipOffTokenChannelsReverse(num - 1);
		}
		if (num < 0)
		{
			return null;
		}
		return _tokens[num];
	}

	public override IToken LT(int k)
	{
		if (_p == -1)
		{
			Setup();
		}
		if (k == 0)
		{
			return null;
		}
		if (k < 0)
		{
			return LB(-k);
		}
		int num = _p;
		for (int i = 1; i < k; i++)
		{
			num = SkipOffTokenChannels(num + 1);
		}
		if (num > Range)
		{
			Range = num;
		}
		return _tokens[num];
	}

	protected virtual int SkipOffTokenChannels(int i)
	{
		Sync(i);
		while (_tokens[i].Channel != _channel)
		{
			i++;
			Sync(i);
		}
		return i;
	}

	protected virtual int SkipOffTokenChannelsReverse(int i)
	{
		while (i >= 0 && _tokens[i].Channel != _channel)
		{
			i--;
		}
		return i;
	}

	public override void Reset()
	{
		base.Reset();
		_p = SkipOffTokenChannels(0);
	}

	protected override void Setup()
	{
		_p = 0;
		_p = SkipOffTokenChannels(_p);
	}
}
