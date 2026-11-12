using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class RulerIndent
{
	private int indent;

	private int oldIndent;

	private bool dragging;

	private IndentOrientation orientation;

	[Description("Gets or sets size of this \"RulerIndent\".")]
	public int Indent
	{
		get
		{
			return indent;
		}
		set
		{
			indent = Math.Max(value, 0);
		}
	}

	[Description("Indicates whether \"RulerIndent\" is in dragging state.")]
	public bool Dragging
	{
		get
		{
			return dragging;
		}
		set
		{
			if (dragging != value)
			{
				dragging = value;
				if (value)
				{
					oldIndent = indent;
				}
			}
		}
	}

	[Description("Gets or sets \"RulerIndent\" alignment.")]
	public IndentOrientation Orientation
	{
		get
		{
			return orientation;
		}
		set
		{
			orientation = value;
		}
	}

	public RulerIndent()
	{
	}

	public RulerIndent(IndentOrientation orientation, int indent)
		: this()
	{
		this.orientation = orientation;
		this.indent = indent;
	}

	public void CancelDragging()
	{
		Indent = oldIndent;
		dragging = false;
	}

	public void DrawIndent(Graphics graph, Rectangle rect, bool vertical, Color indentBackColor, Color backColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		Brush val = (Brush)new SolidBrush(indentBackColor);
		graph.FillRectangle(val, rect);
		val.Dispose();
		Rectangle rectangle = (vertical ? ((orientation != 0) ? new Rectangle(rect.Left, rect.Top, rect.Width, EditConsts.DefaultRulerIndentSize) : new Rectangle(rect.Left, rect.Bottom - EditConsts.DefaultRulerIndentSize, rect.Width, EditConsts.DefaultRulerIndentSize)) : ((orientation != 0) ? new Rectangle(rect.Left, rect.Top, EditConsts.DefaultRulerIndentSize, rect.Height) : new Rectangle(rect.Right - EditConsts.DefaultRulerIndentSize, rect.Top, EditConsts.DefaultRulerIndentSize, rect.Height)));
		val = (Brush)new SolidBrush(backColor);
		graph.FillRectangle(val, rectangle);
		val.Dispose();
	}
}
