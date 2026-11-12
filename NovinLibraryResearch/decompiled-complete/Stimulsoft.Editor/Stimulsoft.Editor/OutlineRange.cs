using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class OutlineRange : Range, IOutlineRange, IRange
{
	private string text;

	private bool visible;

	private int level;

	[Description("Represents text substituting collapsed outline section.")]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
		}
	}

	[Description("Represents text substituting collapsed outline section if outline buttons are displayed.")]
	public virtual string DisplayText => text;

	[Description("Gets or sets a value indicating whether outline section is visible (expanded).")]
	public virtual bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
		}
	}

	[Description("Represents level of outline nesting for the outline section.")]
	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public OutlineRange()
	{
	}

	public OutlineRange(Point start, Point end, int level, string text)
		: base(start, end)
	{
		this.text = text;
		this.level = level;
		visible = true;
	}

	public OutlineRange(Point start, Point end, int level, string text, bool visible)
		: base(start, end)
	{
		this.text = text;
		this.level = level;
		this.visible = visible;
	}
}
