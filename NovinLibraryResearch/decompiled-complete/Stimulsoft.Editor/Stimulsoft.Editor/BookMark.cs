using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class BookMark : IBookMark
{
	private int line;

	private int pos;

	private int index;

	[Description("Gets or sets position of the bookmark within the text. 0 corresponds to the first line, 1 to second the line, and so on.")]
	public int Line
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

	[Description("Gets or sets position of the bookmark within the text line. 0 corresponds to the first character in a line, 1 to the second character, and so on.")]
	public int Pos
	{
		get
		{
			return pos;
		}
		set
		{
			pos = value;
		}
	}

	[Description("Gets or sets an integer value that specifies ordinal number of the bookmark.")]
	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	[Description("Gets position of the bookmark within the text (index of character and line). Corresponds to \"Pos\" and \"Line\" properties.")]
	public Point Position => new Point(pos, line);

	public BookMark(int line, int pos, int index)
	{
		this.line = line;
		this.pos = pos;
		this.index = index;
	}

	public BookMark()
		: this(0, 0, 0)
	{
	}

	public virtual void Assign(IBookMark source)
	{
		Line = source.Line;
		Pos = source.Pos;
		Index = source.Index;
	}
}
