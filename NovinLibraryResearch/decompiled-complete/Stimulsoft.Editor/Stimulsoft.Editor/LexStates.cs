using System;
using System.Collections;

namespace Stimulsoft.Editor;

public class LexStates : ArrayList, ILexStates, IList, ICollection, IEnumerable
{
	private ILexScheme scheme;

	public new ILexState this[int index] => (ILexState)base[index];

	public LexStates(ILexScheme owner)
	{
		scheme = owner;
	}

	public ILexState AddLexState()
	{
		LexState lexState = new LexState(scheme);
		Add(lexState);
		return lexState;
	}

	public ILexState InsertLexState(int index)
	{
		ILexState lexState = new LexState();
		base.Insert(index, lexState);
		return lexState;
	}

	public ILexState FindLexState(string name)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ILexState lexState = (ILexState)enumerator.Current;
				if (lexState.Name == name)
				{
					return lexState;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}
}
