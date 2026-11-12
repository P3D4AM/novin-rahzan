using System;
using System.Collections.Generic;

namespace Antlr.Runtime;

[Serializable]
public class ANTLRStringStream : ICharStream, IIntStream
{
	protected char[] data;

	protected int n;

	protected int p;

	private int line = 1;

	private int charPositionInLine;

	protected int markDepth;

	protected IList<CharStreamState> markers;

	protected int lastMarker;

	public string name;

	public virtual int Index => p;

	public virtual int Line
	{
		get
		{
			return line;
		}
		set
		{
			line = value;
		}
	}

	public virtual int CharPositionInLine
	{
		get
		{
			return charPositionInLine;
		}
		set
		{
			charPositionInLine = value;
		}
	}

	public virtual int Count => n;

	public virtual string SourceName => name;

	public ANTLRStringStream(string input)
		: this(input, null)
	{
	}

	public ANTLRStringStream(string input, string sourceName)
		: this(input.ToCharArray(), input.Length, sourceName)
	{
	}

	public ANTLRStringStream(char[] data, int numberOfActualCharsInArray)
		: this(data, numberOfActualCharsInArray, null)
	{
	}

	public ANTLRStringStream(char[] data, int numberOfActualCharsInArray, string sourceName)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (numberOfActualCharsInArray < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (numberOfActualCharsInArray > data.Length)
		{
			throw new ArgumentException();
		}
		this.data = data;
		n = numberOfActualCharsInArray;
		name = sourceName;
	}

	protected ANTLRStringStream()
	{
		data = new char[0];
	}

	public virtual void Reset()
	{
		p = 0;
		line = 1;
		charPositionInLine = 0;
		markDepth = 0;
	}

	public virtual void Consume()
	{
		if (p < n)
		{
			charPositionInLine++;
			if (data[p] == '\n')
			{
				line++;
				charPositionInLine = 0;
			}
			p++;
		}
	}

	public virtual int LA(int i)
	{
		if (i == 0)
		{
			return 0;
		}
		if (i < 0)
		{
			i++;
			if (p + i - 1 < 0)
			{
				return -1;
			}
		}
		if (p + i - 1 >= n)
		{
			return -1;
		}
		return data[p + i - 1];
	}

	public virtual int LT(int i)
	{
		return LA(i);
	}

	public virtual int Mark()
	{
		if (markers == null)
		{
			markers = new List<CharStreamState>();
			markers.Add(null);
		}
		markDepth++;
		CharStreamState charStreamState = null;
		if (markDepth >= markers.Count)
		{
			charStreamState = new CharStreamState();
			markers.Add(charStreamState);
		}
		else
		{
			charStreamState = markers[markDepth];
		}
		charStreamState.p = Index;
		charStreamState.line = Line;
		charStreamState.charPositionInLine = CharPositionInLine;
		lastMarker = markDepth;
		return markDepth;
	}

	public virtual void Rewind(int m)
	{
		if (m < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		CharStreamState charStreamState = markers[m];
		Seek(charStreamState.p);
		line = charStreamState.line;
		charPositionInLine = charStreamState.charPositionInLine;
		Release(m);
	}

	public virtual void Rewind()
	{
		Rewind(lastMarker);
	}

	public virtual void Release(int marker)
	{
		markDepth = marker;
		markDepth--;
	}

	public virtual void Seek(int index)
	{
		if (index <= p)
		{
			p = index;
			return;
		}
		while (p < index)
		{
			Consume();
		}
	}

	public virtual string Substring(int start, int length)
	{
		if (start < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (start + length > data.Length)
		{
			throw new ArgumentException();
		}
		if (length == 0)
		{
			return string.Empty;
		}
		return new string(data, start, length);
	}

	public override string ToString()
	{
		return new string(data);
	}
}
