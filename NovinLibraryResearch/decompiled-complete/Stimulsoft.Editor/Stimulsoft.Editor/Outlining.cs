using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Stimulsoft.Editor;

public class Outlining : IOutlining, ICollapsable
{
	private Color outlineColor = EditConsts.DefaultOutlineForeColor;

	private ISyntaxEdit owner;

	private DisplayStrings displayLines;

	public int CollapsedCount
	{
		get
		{
			if (displayLines == null)
			{
				return 0;
			}
			return displayLines.CollapsedCount;
		}
	}

	[Browsable(false)]
	public int UpdateCount
	{
		get
		{
			if (displayLines == null)
			{
				return 0;
			}
			return displayLines.UpdateCount;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether outlining enabled.")]
	public bool AllowOutlining
	{
		get
		{
			if (displayLines == null)
			{
				return false;
			}
			return displayLines.AllowOutlining;
		}
		set
		{
			if (displayLines != null)
			{
				displayLines.AllowOutlining = value;
			}
		}
	}

	[Category("SyntaxEdit")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets outlining options.")]
	public OutlineOptions OutlineOptions
	{
		get
		{
			if (displayLines == null)
			{
				return OutlineOptions.None;
			}
			return displayLines.OutlineOptions;
		}
		set
		{
			if (displayLines != null)
			{
				displayLines.OutlineOptions = value;
			}
		}
	}

	[Description("Gets or sets color that is used to draw outline button.")]
	public Color OutlineColor
	{
		get
		{
			return outlineColor;
		}
		set
		{
			if (outlineColor != value)
			{
				outlineColor = value;
				if (owner != null)
				{
					owner.Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object XmlInfo
	{
		get
		{
			return new XmlOutliningInfo(this);
		}
		set
		{
			((XmlOutliningInfo)value).FixupReferences(this);
		}
	}

	public Outlining(ISyntaxEdit owner)
	{
		this.owner = owner;
		if (owner != null)
		{
			displayLines = (DisplayStrings)owner.DisplayLines;
		}
	}

	protected internal bool IsMouseOnOutlineButton(int x, int y, out IOutlineRange range)
	{
		range = null;
		if (owner != null && AllowOutlining && (OutlineOptions & OutlineOptions.DrawButtons) != 0)
		{
			Rectangle clientRect;
			if (owner.Pages.PageType == PageType.PageLayout)
			{
				IEditPage pageAtPoint = owner.Pages.GetPageAtPoint(x, y);
				clientRect = pageAtPoint.ClientRect;
			}
			else
			{
				clientRect = owner.ClientRect;
			}
			int width = ((Gutter)owner.Gutter).GetWidth();
			clientRect.X += width;
			clientRect.Width -= width;
			if (!clientRect.Contains(x, y))
			{
				return false;
			}
			Point point = owner.ScreenToDisplay(x, y);
			point = displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: false, rangeStart: true, tabEnd: false);
			range = displayLines.GetOutlineRange(point);
			if (range != null)
			{
				return !range.Visible;
			}
			return false;
		}
		return false;
	}

	public IOutlineRange Outline(int first, int last, int level)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(first, last, level);
	}

	public IOutlineRange Outline(int first, int last)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(first, last);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, int level)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(startPoint, endPoint, level);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(startPoint, endPoint);
	}

	public IOutlineRange Outline(int first, int last, int level, string outlineText)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(first, last, level, outlineText);
	}

	public IOutlineRange Outline(int first, int last, string outlineText)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(first, last, outlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(startPoint, endPoint, level, outlineText);
	}

	public IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.Outline(startPoint, endPoint, outlineText);
	}

	public void UnOutline(Point point)
	{
		if (displayLines != null)
		{
			displayLines.UnOutline(point);
		}
	}

	public void UnOutline(int index)
	{
		if (displayLines != null)
		{
			displayLines.UnOutline(index);
		}
	}

	public void UnOutline()
	{
		if (displayLines != null)
		{
			displayLines.UnOutline();
		}
	}

	public IOutlineRange GetOutlineRange(Point point)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.GetOutlineRange(point);
	}

	public IOutlineRange GetOutlineRange(int index)
	{
		if (displayLines == null)
		{
			return null;
		}
		return displayLines.GetOutlineRange(index);
	}

	public int GetOutlineRanges(IList ranges, int index)
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.GetOutlineRanges(ranges, index);
	}

	public int GetOutlineRanges(IList ranges, Point point)
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.GetOutlineRanges(ranges, point);
	}

	public int GetOutlineRanges(IList ranges, Point startPoint, Point endPoint)
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.GetOutlineRanges(ranges, startPoint, endPoint);
	}

	public int GetOutlineRanges(IList ranges)
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.GetOutlineRanges(ranges);
	}

	public void SetOutlineRanges(IList ranges)
	{
		if (displayLines != null)
		{
			displayLines.SetOutlineRanges(ranges);
		}
	}

	public void SetOutlineRanges(IList ranges, bool preserveVisible)
	{
		if (displayLines != null)
		{
			displayLines.SetOutlineRanges(ranges, preserveVisible);
		}
	}

	public bool IsExpanded(int index)
	{
		if (displayLines == null)
		{
			return false;
		}
		return displayLines.IsExpanded(index);
	}

	public bool IsCollapsed(int index)
	{
		if (displayLines == null)
		{
			return false;
		}
		return displayLines.IsCollapsed(index);
	}

	public bool IsVisible(Point point)
	{
		if (displayLines == null)
		{
			return true;
		}
		return displayLines.IsVisible(point);
	}

	public bool IsVisible(int index)
	{
		if (displayLines == null)
		{
			return true;
		}
		return displayLines.IsVisible(index);
	}

	public void Collapse(int index)
	{
		if (displayLines != null)
		{
			displayLines.Collapse(index);
		}
	}

	public void Expand(int index)
	{
		if (displayLines != null)
		{
			displayLines.Expand(index);
		}
	}

	public void EnsureExpanded(int index)
	{
		if (displayLines != null)
		{
			displayLines.EnsureExpanded(index);
		}
	}

	public void EnsureExpanded(Point point)
	{
		if (displayLines != null)
		{
			displayLines.EnsureExpanded(point);
		}
	}

	public void FullExpand()
	{
		if (displayLines != null)
		{
			displayLines.FullExpand();
		}
	}

	public void FullCollapse()
	{
		if (displayLines != null)
		{
			displayLines.FullCollapse();
		}
	}

	public void FullExpand(IList ranges)
	{
		if (displayLines != null)
		{
			displayLines.FullExpand(ranges);
		}
	}

	public void FullCollapse(IList ranges)
	{
		if (displayLines != null)
		{
			displayLines.FullCollapse(ranges);
		}
	}

	public void OutlineText()
	{
		if (AllowOutlining && owner != null && ((TextSource)owner.Source).NeedOutlineText())
		{
			ISyntaxParser syntaxParser = (ISyntaxParser)owner.Source.Lexer;
			syntaxParser.Strings = owner.Lines;
			syntaxParser.ReparseText();
			ArrayList ranges = new ArrayList();
			syntaxParser.Outline(ranges);
			SetOutlineRanges(ranges, preserveVisible: true);
		}
	}

	public void UnOutlineText()
	{
		if (displayLines != null)
		{
			displayLines.UnOutline();
		}
	}

	public void ToggleOutlining()
	{
		if (displayLines != null)
		{
			displayLines.ToggleOutlining();
		}
	}

	public void CollapseToDefinitions()
	{
		if (displayLines != null)
		{
			displayLines.CollapseToDefinitions();
		}
	}

	public string GetOutlineHint(IOutlineRange range)
	{
		if (displayLines == null)
		{
			return string.Empty;
		}
		return displayLines.GetOutlineHint(range);
	}

	public void Assign(IOutlining source)
	{
		AllowOutlining = source.AllowOutlining;
		OutlineOptions = source.OutlineOptions;
		OutlineColor = source.OutlineColor;
	}

	public int BeginUpdate()
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.BeginUpdate();
	}

	public int EndUpdate()
	{
		if (displayLines == null)
		{
			return 0;
		}
		return displayLines.EndUpdate();
	}

	public virtual void ResetAllowOutlining()
	{
		AllowOutlining = false;
	}

	public virtual void ResetOutlineOptions()
	{
		OutlineOptions = EditConsts.DefaultOutlineOptions;
	}

	public virtual void ResetOutlineColor()
	{
		OutlineColor = EditConsts.DefaultOutlineForeColor;
	}

	public bool ShouldSerializeOutlineColor()
	{
		return outlineColor != EditConsts.DefaultOutlineForeColor;
	}

	public bool ShouldSerializeOutlineOptions()
	{
		return OutlineOptions != EditConsts.DefaultOutlineOptions;
	}
}
