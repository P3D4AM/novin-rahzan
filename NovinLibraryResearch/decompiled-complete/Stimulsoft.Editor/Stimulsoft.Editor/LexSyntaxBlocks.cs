using System;
using System.Collections;

namespace Stimulsoft.Editor;

public class LexSyntaxBlocks : ArrayList, ILexSyntaxBlocks, IList, ICollection, IEnumerable
{
	private ILexScheme scheme;

	private ILexState state;

	public new ILexSyntaxBlock this[int index] => (ILexSyntaxBlock)base[index];

	public LexSyntaxBlocks(ILexState state, ILexScheme owner)
	{
		scheme = owner;
		this.state = state;
	}

	public ILexSyntaxBlock AddLexSyntaxBlock()
	{
		LexSyntaxBlock lexSyntaxBlock = new LexSyntaxBlock(scheme, this);
		lexSyntaxBlock.CaseSensitive = state != null && state.CaseSensitive;
		Add(lexSyntaxBlock);
		return lexSyntaxBlock;
	}

	public ILexSyntaxBlock InsertLexSyntaxBlock(int index)
	{
		ILexSyntaxBlock lexSyntaxBlock = new LexSyntaxBlock();
		base.Insert(index, lexSyntaxBlock);
		return lexSyntaxBlock;
	}

	public ILexSyntaxBlock FindSyntaxBlock(string name)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ILexSyntaxBlock lexSyntaxBlock = (ILexSyntaxBlock)enumerator.Current;
				if (lexSyntaxBlock.Name == name)
				{
					return lexSyntaxBlock;
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
