using System;
using Antlr.Runtime.Misc;

namespace Antlr.Runtime;

public class UnbufferedTokenStream : LookaheadStream<IToken>, ITokenStream, IIntStream, ITokenStreamInformation
{
	[CLSCompliant(false)]
	protected ITokenSource tokenSource;

	protected int tokenIndex;

	protected int channel;

	private readonly ListStack<IToken> _realTokens = new ListStack<IToken> { null };

	public ITokenSource TokenSource => tokenSource;

	public string SourceName => TokenSource.SourceName;

	public IToken LastToken => LB(1);

	public IToken LastRealToken => _realTokens.Peek();

	public int MaxLookBehind => 1;

	public UnbufferedTokenStream(ITokenSource tokenSource)
	{
		this.tokenSource = tokenSource;
	}

	public override int Mark()
	{
		_realTokens.Push(_realTokens.Peek());
		return base.Mark();
	}

	public override void Release(int marker)
	{
		base.Release(marker);
		_realTokens.Pop();
	}

	public override void Clear()
	{
		_realTokens.Clear();
		_realTokens.Push(null);
	}

	public override void Consume()
	{
		base.Consume();
		if (base.PreviousElement != null && base.PreviousElement.Line > 0)
		{
			_realTokens[_realTokens.Count - 1] = base.PreviousElement;
		}
	}

	public override IToken NextElement()
	{
		IToken token = tokenSource.NextToken();
		token.TokenIndex = tokenIndex++;
		return token;
	}

	public override bool IsEndOfFile(IToken o)
	{
		return o.Type == -1;
	}

	public IToken Get(int i)
	{
		throw new NotSupportedException("Absolute token indexes are meaningless in an unbuffered stream");
	}

	public int LA(int i)
	{
		return LT(i).Type;
	}

	public string ToString(int start, int stop)
	{
		return "n/a";
	}

	public string ToString(IToken start, IToken stop)
	{
		return "n/a";
	}
}
