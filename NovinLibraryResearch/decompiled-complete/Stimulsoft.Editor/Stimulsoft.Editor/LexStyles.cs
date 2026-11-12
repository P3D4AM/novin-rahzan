using System;
using System.Collections;

namespace Stimulsoft.Editor;

public class LexStyles : ArrayList, ILexStyles, IList, ICollection, IEnumerable
{
	private ILexScheme owner;

	public new ILexStyle this[int index] => (ILexStyle)base[index];

	public LexStyles(ILexScheme owner)
	{
		this.owner = owner;
	}

	public ILexStyle AddLexStyle()
	{
		LexStyle lexStyle = new LexStyle(owner);
		Add(lexStyle);
		return lexStyle;
	}

	public ILexStyle InsertLexStyle(int index)
	{
		ILexStyle lexStyle = new LexStyle();
		base.Insert(index, lexStyle);
		return lexStyle;
	}

	public ILexStyle FindLexStyle(string name)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ILexStyle lexStyle = (ILexStyle)enumerator.Current;
				if (lexStyle.Name == name)
				{
					return lexStyle;
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
