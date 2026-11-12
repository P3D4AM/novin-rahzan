using System;
using System.Collections.Generic;

namespace Antlr.Runtime;

public class RecognizerSharedState
{
	public BitSet[] following;

	[CLSCompliant(false)]
	public int _fsp;

	public bool errorRecovery;

	public int lastErrorIndex;

	public bool failed;

	public int syntaxErrors;

	public int backtracking;

	public IDictionary<int, int>[] ruleMemo;

	public IToken token;

	public int tokenStartCharIndex;

	public int tokenStartLine;

	public int tokenStartCharPositionInLine;

	public int channel;

	public int type;

	public string text;

	public RecognizerSharedState()
	{
		following = new BitSet[100];
		_fsp = -1;
		lastErrorIndex = -1;
		tokenStartCharIndex = -1;
	}

	public RecognizerSharedState(RecognizerSharedState state)
	{
		if (state == null)
		{
			throw new ArgumentNullException("state");
		}
		following = (BitSet[])state.following.Clone();
		_fsp = state._fsp;
		errorRecovery = state.errorRecovery;
		lastErrorIndex = state.lastErrorIndex;
		failed = state.failed;
		syntaxErrors = state.syntaxErrors;
		backtracking = state.backtracking;
		if (state.ruleMemo != null)
		{
			ruleMemo = (IDictionary<int, int>[])state.ruleMemo.Clone();
		}
		token = state.token;
		tokenStartCharIndex = state.tokenStartCharIndex;
		tokenStartCharPositionInLine = state.tokenStartCharPositionInLine;
		channel = state.channel;
		type = state.type;
		text = state.text;
	}
}
