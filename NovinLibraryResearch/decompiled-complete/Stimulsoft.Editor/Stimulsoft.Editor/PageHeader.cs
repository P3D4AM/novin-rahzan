using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Stimulsoft.Editor;

public class PageHeader : IPageHeader
{
	private string leftText = string.Empty;

	private string rightText = string.Empty;

	private string centerText = string.Empty;

	private Font font;

	private Point offset;

	private bool visible = true;

	private Regex textRegex;

	private IEditPage page;

	private Color fontColor;

	private bool reverseOnEvenPages;

	private int updateCount;

	protected internal Regex TextRegex
	{
		get
		{
			if (textRegex == null)
			{
				textRegex = new Regex("\\\\\\[[a-zA-Z_0-9]+\\]", RegexOptions.Singleline);
			}
			return textRegex;
		}
	}

	[Description("Gets or sets a string that appears at the left part of the header/footer area.")]
	public string LeftText
	{
		get
		{
			return leftText;
		}
		set
		{
			if (leftText != value)
			{
				leftText = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a string that appears at the center of the header/footer area.")]
	public string CenterText
	{
		get
		{
			return centerText;
		}
		set
		{
			if (centerText != value)
			{
				centerText = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a string that appears at the right part of the header/footer area.")]
	public string RightText
	{
		get
		{
			return rightText;
		}
		set
		{
			if (rightText != value)
			{
				rightText = value;
				Update();
			}
		}
	}

	[Description("Gets or sets font color used to draw header/footer text.")]
	public Color FontColor
	{
		get
		{
			return fontColor;
		}
		set
		{
			if (fontColor != value)
			{
				fontColor = value;
				Update();
			}
		}
	}

	[Description("Gets or sets font used to draw header/footer text.")]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
				Update();
			}
		}
	}

	[Description("Gets or sets indentation of the primary text edge.")]
	public Point Offset
	{
		get
		{
			return offset;
		}
		set
		{
			if (offset != value)
			{
				offset = value;
				Update();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether the \"PageHeader\" is visible.")]
	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				Update();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the \"LeftText\" and \"RightText\" interchanging on even page.")]
	public bool ReverseOnEvenPages
	{
		get
		{
			return reverseOnEvenPages;
		}
		set
		{
			if (reverseOnEvenPages != value)
			{
				reverseOnEvenPages = value;
				if (page != null)
				{
					page.Invalidate();
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
			return new XmlPageHeaderInfo(this);
		}
		set
		{
			((XmlPageHeaderInfo)value).FixupReferences(this);
		}
	}

	public PageHeader()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		font = new Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
		fontColor = EditConsts.DefaultHeaderFontColor;
		offset = new Point(24, 8);
	}

	public PageHeader(IEditPage page)
		: this()
	{
		this.page = page;
	}

	~PageHeader()
	{
		font.Dispose();
	}

	private EditPages GetPages()
	{
		if (page == null)
		{
			return null;
		}
		return ((EditPage)page).Pages;
	}

	public string GetTextToPaint(string text, int pageIndex, int pageCount)
	{
		MatchCollection matchCollection = TextRegex.Matches(text);
		for (int num = matchCollection.Count - 1; num >= 0; num--)
		{
			Match match = matchCollection[num];
			if (match.Success)
			{
				string text2 = text.Substring(match.Index, match.Length);
				if (string.Compare(text2, EditConsts.PageTag) == 0)
				{
					text2 = (pageIndex + 1).ToString();
				}
				if (string.Compare(text2, EditConsts.PagesTag) == 0)
				{
					text2 = pageCount.ToString();
				}
				if (string.Compare(text2, EditConsts.DateTag) == 0)
				{
					text2 = DateTime.Now.ToLongDateString();
				}
				if (string.Compare(text2, EditConsts.TimeTag) == 0)
				{
					text2 = DateTime.Now.ToLongTimeString();
				}
				if (string.Compare(text2, EditConsts.UserTag) == 0)
				{
					text2 = Environment.UserName;
				}
				else
				{
					GetPages()?.OnDrawHeader(ref text2);
				}
				text = text.Remove(match.Index, match.Length);
				text = text.Insert(match.Index, text2);
			}
		}
		return text;
	}

	public int BeginUpdate()
	{
		updateCount++;
		return updateCount;
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
		return updateCount;
	}

	public void Update()
	{
		if (updateCount == 0 && page != null)
		{
			page.Invalidate();
		}
	}

	public void Paint(IPainter painter, Rectangle rect, int pageIndex, int pageCount, bool pageNumbers)
	{
		Font val = painter.Font;
		Color textColor = painter.TextColor;
		painter.Opaque = false;
		try
		{
			painter.Font = Font;
			painter.TextColor = fontColor;
			EditPages pages = GetPages();
			DrawInfo info = default(DrawInfo);
			info.Init();
			info.Page = ((page != null) ? page.Index : (-1));
			SyntaxEdit syntaxEdit = ((pages != null && pages.Owner != null) ? ((SyntaxEdit)pages.Owner) : null);
			if (syntaxEdit == null || !syntaxEdit.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.PageHeader, info))
			{
				bool flag = pages != null && reverseOnEvenPages && (pageIndex + 1) % 2 == 0;
				string text = (pageNumbers ? (pageIndex + 1).ToString() : RightText);
				string text2 = (flag ? text : LeftText);
				if (text2 != null && text2 != string.Empty)
				{
					painter.TextOut(GetTextToPaint(text2, pageIndex, pageCount), -1, rect);
				}
				text2 = CenterText;
				if (text2 != null && text2 != string.Empty)
				{
					text2 = GetTextToPaint(text2, pageIndex, pageCount);
					int num = painter.StringWidth(text2);
					painter.TextOut(text2, -1, rect.Left + rect.Right - num >> 1, rect.Top);
				}
				text2 = (flag ? LeftText : text);
				if (text2 != null && text2 != string.Empty)
				{
					text2 = GetTextToPaint(text2, pageIndex, pageCount);
					int num2 = painter.StringWidth(text2);
					painter.TextOut(text2, -1, rect.Right - num2, rect.Top);
				}
			}
			syntaxEdit?.OnCustomDraw(painter, rect, DrawStage.After, DrawState.PageHeader, info);
		}
		finally
		{
			painter.Opaque = true;
			painter.Font = val;
			painter.TextColor = textColor;
		}
	}

	public void Assign(IPageHeader source)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		BeginUpdate();
		try
		{
			LeftText = source.LeftText;
			RightText = source.RightText;
			CenterText = source.CenterText;
			Offset = source.Offset;
			ReverseOnEvenPages = source.ReverseOnEvenPages;
			Font = new Font(source.Font.FontFamily, source.Font.Size, source.Font.Style);
			FontColor = source.FontColor;
			Visible = source.Visible;
		}
		finally
		{
			EndUpdate();
		}
	}

	public virtual void ResetFont()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		Font = new Font(FontFamily.GenericMonospace, 10f, EditConsts.DefaultHeaderFontStyle);
	}

	public virtual void ResetFontColor()
	{
		FontColor = EditConsts.DefaultHeaderFontColor;
	}

	public virtual void ResetOffset()
	{
		Offset = new Point(8, 8);
	}

	public void ResetReverseOnEvenPages()
	{
		ReverseOnEvenPages = false;
	}

	public bool ShouldSerializeFontColor()
	{
		return fontColor != EditConsts.DefaultHeaderFontColor;
	}

	public bool ShouldSerializeOffset()
	{
		if (offset.X == 24)
		{
			return offset.Y != 8;
		}
		return true;
	}

	public bool ShouldSerializeLeftText()
	{
		return leftText != string.Empty;
	}

	public bool ShouldSerializeRightText()
	{
		return rightText != string.Empty;
	}

	public bool ShouldSerializeCenterText()
	{
		return centerText != string.Empty;
	}

	public bool ShouldSerializeFont()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (!(font.FontFamily.Name != FontFamily.GenericMonospace.Name) && (double)font.Size == 10.0)
		{
			return font.Style != EditConsts.DefaultHeaderFontStyle;
		}
		return true;
	}
}
