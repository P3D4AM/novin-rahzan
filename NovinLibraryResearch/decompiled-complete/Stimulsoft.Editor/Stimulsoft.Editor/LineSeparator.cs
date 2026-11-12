using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class LineSeparator : ILineSeparator
{
	private SeparatorOptions options;

	private Color highlightBackColor = EditConsts.DefaultLineSeparatorColor;

	private Color highlightForeColor = Color.Empty;

	private Color lineColor = EditConsts.DefaultLineSeparatorLineColor;

	private Color contentDividerColor = EditConsts.DefaultContentDividerColor;

	private int updateCount;

	private int tempLine = -1;

	private ISyntaxEdit owner;

	protected internal int TempLine
	{
		get
		{
			return tempLine;
		}
		set
		{
			if (tempLine != value)
			{
				InvalidateLine();
				tempLine = value;
				InvalidateLine();
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[DefaultValue(SeparatorOptions.None)]
	[Description("Gets or sets a set of flags customizing appearance and behaviour of the \"LineSeparator\".")]
	public SeparatorOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			if (options != value)
			{
				options = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a background color of highlighted line.")]
	public Color HighlightBackColor
	{
		get
		{
			return highlightBackColor;
		}
		set
		{
			if (highlightBackColor != value)
			{
				highlightBackColor = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a foreground color of highlighted line.")]
	public Color HighlightForeColor
	{
		get
		{
			return highlightForeColor;
		}
		set
		{
			if (highlightForeColor != value)
			{
				highlightForeColor = value;
				Update();
			}
		}
	}

	[Description("Gets or sets color of horizontal lines between particular lines in the Edit control.")]
	public Color LineColor
	{
		get
		{
			return lineColor;
		}
		set
		{
			if (lineColor != value)
			{
				lineColor = value;
				Update();
			}
		}
	}

	[Description("Gets or sets color of horizontal lines between particular sections(for example, methods) in the Edit control.")]
	public Color ContentDividerColor
	{
		get
		{
			return contentDividerColor;
		}
		set
		{
			if (contentDividerColor != value)
			{
				contentDividerColor = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlLineSeparatorInfo(this);
		}
		set
		{
			((XmlLineSeparatorInfo)value).FixupReferences(this);
		}
	}

	public LineSeparator()
	{
	}

	public LineSeparator(ISyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	private void InvalidateLine()
	{
		if (owner != null && tempLine >= 0)
		{
			Region rectRegion = ((SyntaxEdit)owner).GetRectRegion(new Rectangle(0, tempLine, int.MaxValue, tempLine));
			if (rectRegion != null)
			{
				((Control)(SyntaxEdit)owner).Invalidate(rectRegion, false);
				rectRegion.Dispose();
			}
		}
	}

	protected internal void BeginUpdate()
	{
		updateCount++;
	}

	protected internal void EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
	}

	protected internal void Update()
	{
		if (updateCount == 0 && owner != null)
		{
			owner.Invalidate();
		}
	}

	protected internal bool NeedHighlightLine(int index, bool display)
	{
		if (owner != null && ((options & SeparatorOptions.HighlightCurrentLine) != 0 || tempLine >= 0) && ((display && owner.DisplayLines.PointToDisplayPoint((tempLine >= 0) ? new Point(0, tempLine) : owner.Position).Y == index) || (!display && ((tempLine >= 0) ? tempLine : owner.Position.Y) == index)))
		{
			if ((options & SeparatorOptions.HideHighlighting) != 0)
			{
				return owner.Focused;
			}
			return true;
		}
		return false;
	}

	public void Assign(ILineSeparator source)
	{
		BeginUpdate();
		try
		{
			Options = source.Options;
			HighlightForeColor = source.HighlightForeColor;
			HighlightBackColor = source.HighlightBackColor;
			LineColor = source.LineColor;
			ContentDividerColor = source.ContentDividerColor;
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool NeedHighlight()
	{
		if (owner != null && (options & SeparatorOptions.HighlightCurrentLine) != 0)
		{
			if ((options & SeparatorOptions.HideHighlighting) != 0)
			{
				return owner.Focused;
			}
			return true;
		}
		return false;
	}

	public bool NeedHide()
	{
		if ((options & SeparatorOptions.HighlightCurrentLine) != 0)
		{
			return (options & SeparatorOptions.HideHighlighting) != 0;
		}
		return false;
	}

	public void TempHighlightLine(int index)
	{
		TempLine = index;
	}

	public void TempUnhighlightLine()
	{
		TempLine = -1;
	}

	public virtual void ResetHighlightBackColor()
	{
		HighlightBackColor = EditConsts.DefaultLineSeparatorColor;
	}

	public virtual void ResetHighlightForeColor()
	{
		HighlightForeColor = Color.Empty;
	}

	public virtual void ResetLineColor()
	{
		LineColor = EditConsts.DefaultLineSeparatorLineColor;
	}

	public virtual void ResetContentDividerColor()
	{
		ContentDividerColor = EditConsts.DefaultContentDividerColor;
	}

	public virtual void ResetOptions()
	{
		Options = SeparatorOptions.None;
	}

	public bool ShouldSerializeHighlightBackColor()
	{
		return highlightBackColor != EditConsts.DefaultLineSeparatorColor;
	}

	public bool ShouldSerializeHighlightForeColor()
	{
		return highlightForeColor != Color.Empty;
	}

	public bool ShouldSerializeLineColor()
	{
		return lineColor != EditConsts.DefaultLineSeparatorLineColor;
	}

	public bool ShouldSerializeContentDividerColor()
	{
		return contentDividerColor != EditConsts.DefaultContentDividerColor;
	}
}
