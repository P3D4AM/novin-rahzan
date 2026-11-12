using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class EditPage : IEditPage
{
	private EditPages pages;

	private int horzOffset = EditConsts.DefaultPageHorzOffset;

	private int vertOffset = EditConsts.DefaultPageVertOffset;

	private Margins margins;

	private Size pageSize;

	private PaperKind pageKind;

	private bool landscape;

	private PageHeader header;

	private PageHeader footer;

	private bool paintNumber = true;

	private bool usePrinterSettings = true;

	private Point origin;

	private int index;

	private int startLine;

	private int endLine;

	private int leftIndent;

	private int rightIndent;

	private int topIndent;

	private int bottomIndent;

	private Size caps;

	protected internal int LeftIndent
	{
		get
		{
			return leftIndent;
		}
		set
		{
			if (leftIndent != value)
			{
				leftIndent = value;
				margins.Left = value * 100 / caps.Width;
			}
		}
	}

	protected internal int RightIndent
	{
		get
		{
			return rightIndent;
		}
		set
		{
			if (rightIndent != value)
			{
				rightIndent = value;
				margins.Right = value * 100 / caps.Width;
			}
		}
	}

	protected internal int TopIndent
	{
		get
		{
			return topIndent;
		}
		set
		{
			if (topIndent != value)
			{
				topIndent = value;
				margins.Top = value * 100 / caps.Height;
			}
		}
	}

	protected internal int BottomIndent
	{
		get
		{
			return bottomIndent;
		}
		set
		{
			if (bottomIndent != value)
			{
				bottomIndent = value;
				margins.Bottom = value * 100 / caps.Height;
			}
		}
	}

	protected internal EditPages Pages
	{
		get
		{
			return pages;
		}
		set
		{
			pages = value;
		}
	}

	protected internal int DisplayWidth
	{
		get
		{
			int num = ClientRect.Width;
			if (pages != null)
			{
				if (pages.PageType != 0)
				{
					num = pageSize.Width - (leftIndent + rightIndent);
				}
				num -= ((Gutter)pages.Owner.Gutter).GetWidth();
			}
			return num;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int StartLine
	{
		get
		{
			return startLine;
		}
		set
		{
			startLine = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int EndLine
	{
		get
		{
			return endLine;
		}
		set
		{
			endLine = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Description("Gets or sets size (width and height) of this \"EditPage\".")]
	public Size PageSize
	{
		get
		{
			return pageSize;
		}
		set
		{
			if (!pageSize.Equals((object?)value))
			{
				pageSize = value;
				Update(changed: true);
			}
		}
	}

	[Description("Gets or sets margins specifying offsets of editing area of this \"EditPage\".")]
	public Margins Margins
	{
		get
		{
			return margins;
		}
		set
		{
			if (margins != value && value != (Margins)null)
			{
				margins.Left = value.Left;
				margins.Top = value.Top;
				margins.Right = value.Right;
				margins.Bottom = value.Bottom;
				InitMargins();
				Update(changed: true);
			}
		}
	}

	[Description("Gets or sets kind of the page specifying standard paper size.")]
	public PaperKind PageKind
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return pageKind;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (pageKind != value)
			{
				pageKind = value;
				if (pages != null)
				{
					pages.UpdatePageSize(PageKind, ref pageSize);
				}
				Update(changed: true);
			}
		}
	}

	[Description("Gets or sets boolean value that indicates the page orientation (landscape or portrait).")]
	public bool Landscape
	{
		get
		{
			return landscape;
		}
		set
		{
			if (landscape != value)
			{
				landscape = value;
				Update(changed: true);
			}
		}
	}

	[Description("Gets or sets a horizontal indentation between pages.")]
	public int HorzOffset
	{
		get
		{
			return horzOffset;
		}
		set
		{
			if (HorzOffset != value)
			{
				horzOffset = value;
				Update();
			}
		}
	}

	[Description("Gets or sets a vertical indentation between pages.")]
	public int VertOffset
	{
		get
		{
			return vertOffset;
		}
		set
		{
			if (vertOffset != value)
			{
				vertOffset = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	public bool IsFirstPage => index == 0;

	[Browsable(false)]
	public bool IsLastPage
	{
		get
		{
			if (pages != null)
			{
				return index == pages.Count - 1;
			}
			return false;
		}
	}

	[Browsable(false)]
	public IEditPage PrevPage
	{
		get
		{
			if (index <= 0 || pages == null)
			{
				return null;
			}
			return pages[index - 1];
		}
	}

	[Browsable(false)]
	public IEditPage NextPage
	{
		get
		{
			if (pages == null || index >= pages.Count - 1)
			{
				return null;
			}
			return pages[index + 1];
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point Origin
	{
		get
		{
			return origin;
		}
		set
		{
			origin = value;
		}
	}

	[Browsable(false)]
	public Rectangle BoundsRect
	{
		get
		{
			Rectangle rect = GetBounds(includeSpace: false);
			ScrollRect(ref rect);
			return rect;
		}
	}

	[Browsable(false)]
	public Rectangle ClientRect
	{
		get
		{
			Rectangle result = BoundsRect;
			if (pages != null && pages.PageType == PageType.PageLayout)
			{
				bool displayWhiteSpace = pages.DisplayWhiteSpace;
				result = new Rectangle(result.X + leftIndent, result.Y + (displayWhiteSpace ? topIndent : 4), result.Width - (leftIndent + rightIndent), result.Height - (displayWhiteSpace ? (topIndent + bottomIndent) : 8));
			}
			return result;
		}
	}

	[Browsable(false)]
	public Rectangle PageRect
	{
		get
		{
			Rectangle rect = GetBounds(includeSpace: true);
			ScrollRect(ref rect);
			return rect;
		}
	}

	[Browsable(false)]
	public Rectangle PaintRect
	{
		get
		{
			Rectangle pageRect = PageRect;
			if (pages != null)
			{
				pageRect.X = 0;
				pageRect.Width = ((SyntaxEdit)pages.Owner).GetClientRect(excludeNonClient: true).Width;
			}
			return pageRect;
		}
	}

	[Browsable(false)]
	public Rectangle WhiteSpaceTopRect
	{
		get
		{
			Rectangle result = BoundsRect;
			if (pages != null)
			{
				if (pages.PageType == PageType.PageLayout)
				{
					bool displayWhiteSpace = pages.DisplayWhiteSpace;
					result = new Rectangle(result.Left, result.Top - (displayWhiteSpace ? vertOffset : (vertOffset / 2)), result.Width, displayWhiteSpace ? vertOffset : (vertOffset / 2));
				}
				else
				{
					result = new Rectangle(0, 0, 0, 0);
				}
			}
			return result;
		}
	}

	[Browsable(false)]
	public Rectangle WhiteSpaceBottomRect
	{
		get
		{
			Rectangle result = BoundsRect;
			if (pages != null)
			{
				if (pages.PageType == PageType.PageLayout)
				{
					bool displayWhiteSpace = pages.DisplayWhiteSpace;
					result = new Rectangle(result.Left, result.Bottom, result.Width, displayWhiteSpace ? vertOffset : (vertOffset / 2));
				}
				else
				{
					result = new Rectangle(0, 0, 0, 0);
				}
			}
			return result;
		}
	}

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents \"IPageHeader\" specifying page header.")]
	public IPageHeader Header
	{
		get
		{
			return header;
		}
		set
		{
			header.Assign(value);
			Invalidate();
		}
	}

	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents \"IPageHeader\" specifying page footer.")]
	public IPageHeader Footer
	{
		get
		{
			return footer;
		}
		set
		{
			footer.Assign(value);
			Invalidate();
		}
	}

	[DefaultValue(true)]
	[Description("Get or sets a boolean value that indicates whether control should paint it's number at the bottom of page.")]
	public bool PaintNumber
	{
		get
		{
			return paintNumber;
		}
		set
		{
			if (paintNumber != value)
			{
				paintNumber = value;
				Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Get or sets a boolean value that indicates whether control should use printer settings for calcuating page size, margin and orientation.")]
	public bool UsePrinterSettings
	{
		get
		{
			return usePrinterSettings;
		}
		set
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (usePrinterSettings != value)
			{
				usePrinterSettings = value;
				if (value && pages != null)
				{
					Margins = pages.DefaultMargins;
					PageKind = pages.DefaultPageKind;
					PageSize = pages.DefaultPageSize;
					Landscape = pages.DefaultLandscape;
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
			return new XmlEditPageInfo(this);
		}
		set
		{
			((XmlEditPageInfo)value).FixupReferences(this);
		}
	}

	public EditPage(EditPages pages)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		this.pages = pages;
		header = new PageHeader(this);
		footer = new PageHeader(this);
		origin = new Point(0, 0);
		pageSize = pages.DefaultPageSize;
		margins = new Margins(pages.DefaultMargins.Left, pages.DefaultMargins.Right, pages.DefaultMargins.Top, pages.DefaultMargins.Bottom);
		pageKind = pages.DefaultPageKind;
		landscape = pages.DefaultLandscape;
		InitMargins();
		caps = Pages.Caps;
	}

	private void InitMargins()
	{
		leftIndent = margins.Left * caps.Width / 100;
		rightIndent = margins.Right * caps.Width / 100;
		topIndent = margins.Top * caps.Height / 100;
		bottomIndent = margins.Bottom * caps.Height / 100;
	}

	private void FrameRect(IPainter painter, ref Rectangle rect, Color color, Color backColor, bool drawBorder)
	{
		Color backColor2 = painter.BackColor;
		Color foreColor = painter.ForeColor;
		painter.BackColor = color;
		painter.ForeColor = color;
		try
		{
			painter.DrawRectangle(new Rectangle(rect.Location, new Size(rect.Width - 1, rect.Height - 1)));
			painter.DrawLine(rect.Right, rect.Top + 1, rect.Right, rect.Bottom);
			painter.ForeColor = backColor;
			painter.DrawLine(rect.Right, rect.Top, rect.Right, rect.Top + 1);
			if (drawBorder || IsLastPage)
			{
				painter.ForeColor = color;
				painter.DrawLine(rect.Left + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
				painter.ForeColor = backColor;
				painter.DrawLine(rect.Left, rect.Bottom, rect.Left + 1, rect.Bottom);
			}
			if (drawBorder)
			{
				painter.ForeColor = color;
				painter.DrawLine(rect.Right + 1, rect.Top + 1, rect.Right + 1, rect.Bottom + 1);
				painter.DrawLine(rect.Left + 1, rect.Bottom + 1, rect.Right + 1, rect.Bottom + 1);
				painter.ForeColor = backColor;
				painter.DrawLine(rect.Right + 1, rect.Top, rect.Right + 1, rect.Top + 1);
				painter.DrawLine(rect.Left, rect.Bottom + 1, rect.Left, rect.Bottom + 2);
			}
			rect.Width++;
			if (drawBorder || IsLastPage)
			{
				rect.Height++;
			}
			if (drawBorder)
			{
				rect.Width++;
				rect.Height++;
			}
		}
		finally
		{
			painter.ForeColor = foreColor;
			painter.BackColor = backColor2;
		}
	}

	private void FillRect(IPainter painter, Rectangle r1, Rectangle r2)
	{
		Rectangle rect = Rectangle.Intersect(r1, r2);
		if (!rect.IsEmpty)
		{
			painter.FillRectangle(rect);
		}
	}

	private void ScrollRect(ref Rectangle rect)
	{
		if (pages != null)
		{
			ISyntaxEdit owner = pages.Owner;
			if (pages.PageType == PageType.PageLayout)
			{
				rect.Offset(-owner.Scrolling.WindowOriginX, -owner.Scrolling.WindowOriginY);
			}
			else
			{
				rect.Offset(-owner.Scrolling.WindowOriginX * owner.Painter.FontWidth, -owner.Scrolling.WindowOriginY * owner.Painter.FontHeight);
			}
		}
	}

	protected internal void FillBoundsRect(IPainter painter, Rectangle outerRect, Rectangle innerRect, Color color, bool transparent)
	{
		if (transparent || innerRect.IsEmpty || outerRect.IsEmpty || innerRect.IsEmpty)
		{
			return;
		}
		Color backColor = painter.BackColor;
		painter.BackColor = color;
		try
		{
			FillRect(painter, outerRect, new Rectangle(outerRect.Left, outerRect.Top, innerRect.Left - outerRect.Left, outerRect.Height));
			FillRect(painter, outerRect, new Rectangle(innerRect.Right, outerRect.Top, outerRect.Right - innerRect.Right, outerRect.Height));
			FillRect(painter, outerRect, new Rectangle(innerRect.Left, outerRect.Top, innerRect.Width, innerRect.Top - outerRect.Top));
			FillRect(painter, outerRect, new Rectangle(innerRect.Left, innerRect.Bottom, innerRect.Width, outerRect.Bottom - innerRect.Bottom));
		}
		finally
		{
			painter.BackColor = backColor;
		}
	}

	protected void PaintMargins(IPainter painter, Rectangle boundsR, Rectangle clientR, Color color)
	{
		if (clientR.IsEmpty)
		{
			return;
		}
		Color foreColor = painter.ForeColor;
		painter.ForeColor = color;
		try
		{
			int defaultPageMarginSize = EditConsts.DefaultPageMarginSize;
			painter.DrawLine(clientR.Left, clientR.Top, Math.Max(clientR.Left - defaultPageMarginSize, boundsR.Left), clientR.Top);
			painter.DrawLine(clientR.Left, clientR.Top, clientR.Left, Math.Max(clientR.Top - defaultPageMarginSize, boundsR.Top));
			painter.DrawLine(clientR.Right, clientR.Top, Math.Min(clientR.Right + defaultPageMarginSize, boundsR.Right - 1), clientR.Top);
			painter.DrawLine(clientR.Right, clientR.Top, clientR.Right, Math.Max(clientR.Top - defaultPageMarginSize, boundsR.Top));
			painter.DrawLine(clientR.Left, clientR.Bottom, Math.Max(clientR.Left - defaultPageMarginSize, boundsR.Left), clientR.Bottom);
			painter.DrawLine(clientR.Left, clientR.Bottom, clientR.Left, Math.Min(clientR.Bottom + defaultPageMarginSize, boundsR.Bottom - 1));
			painter.DrawLine(clientR.Right, clientR.Bottom, Math.Min(clientR.Right + defaultPageMarginSize, boundsR.Right - 1), clientR.Bottom);
			painter.DrawLine(clientR.Right, clientR.Bottom, clientR.Right, Math.Min(clientR.Bottom + defaultPageMarginSize, boundsR.Bottom - 1));
		}
		finally
		{
			painter.ForeColor = foreColor;
		}
	}

	public void Paint(IPainter painter)
	{
		if (pages == null || pages.PageType == PageType.Normal)
		{
			return;
		}
		SyntaxEdit syntaxEdit = (SyntaxEdit)pages.Owner;
		Rectangle clientRect = ClientRect;
		DrawInfo info = default(DrawInfo);
		if (pages.PageType == PageType.PageBreaks)
		{
			Rectangle clientRect2 = syntaxEdit.ClientRect;
			clientRect.X = clientRect2.Left + ((Gutter)syntaxEdit.Gutter).GetWidth();
			clientRect.Width = clientRect2.Right - clientRect.Left;
			if (!IsLastPage)
			{
				info.Init();
				info.Page = Index;
				if (!syntaxEdit.OnCustomDraw(painter, clientRect, DrawStage.Before, DrawState.PageBorder, info))
				{
					painter.DrawLine(clientRect.Left, clientRect.Bottom - 1, clientRect.Right - 1, clientRect.Bottom - 1, pages.BorderColor, 1, (DashStyle)2);
					syntaxEdit.OnCustomDraw(painter, clientRect, DrawStage.After, DrawState.PageBorder, info);
				}
			}
			return;
		}
		Rectangle rect = BoundsRect;
		info.Init();
		info.Page = Index;
		if (!syntaxEdit.OnCustomDraw(painter, rect, DrawStage.Before, DrawState.PageBorder, info))
		{
			bool displayWhiteSpace = pages.DisplayWhiteSpace;
			int fontHeight = syntaxEdit.Painter.FontHeight;
			if (fontHeight != 0)
			{
				clientRect.Height = clientRect.Height / fontHeight * fontHeight;
			}
			Rectangle paintRect = PaintRect;
			rect.Inflate(1, 1);
			FrameRect(painter, ref rect, pages.BorderColor, pages.BackColor, displayWhiteSpace);
			if (!pages.IsTransparent(nonClient: true))
			{
				FillBoundsRect(painter, paintRect, rect, pages.BackColor, transparent: false);
			}
			rect = BoundsRect;
			FillBoundsRect(painter, rect, clientRect, ((Control)syntaxEdit).BackColor, pages.IsTransparent(nonClient: false));
			PaintMargins(painter, rect, clientRect, pages.BackColor);
			if (pages == null || pages.DisplayWhiteSpace)
			{
				int num = Math.Max(clientRect.Left - header.Offset.X, rect.Left);
				int num2 = Math.Min(clientRect.Left + clientRect.Width + header.Offset.X, rect.Right);
				Rectangle rect2 = new Rectangle(num, Math.Max(clientRect.Top - (header.Offset.Y + header.Font.Height), BoundsRect.Top), num2 - num, header.Font.Height);
				header.Paint(painter, rect2, Index, pages.Count, pageNumbers: false);
				num = Math.Max(clientRect.Left - footer.Offset.X, rect.Left);
				num2 = Math.Min(clientRect.Left + clientRect.Width + footer.Offset.X, rect.Right);
				rect2 = new Rectangle(num, Math.Min(clientRect.Bottom + footer.Offset.Y, rect.Bottom - footer.Font.Height), num2 - num, footer.Font.Height);
				footer.Paint(painter, rect2, Index, pages.Count, paintNumber);
			}
			syntaxEdit.OnCustomDraw(painter, rect, DrawStage.After, DrawState.PageBorder, info);
		}
		syntaxEdit.PaintWindow(painter, startLine, new Rectangle(0, 0, clientRect.Width, clientRect.Height), clientRect.Location, 1f, 1f, specialPaint: true, inPrinting: false);
	}

	public void Update()
	{
		Update(changed: false);
	}

	public void Update(bool changed)
	{
		if (pages != null)
		{
			pages.Update(this, changed);
		}
	}

	public void Invalidate()
	{
		if (pages != null)
		{
			pages.Invalidate(this);
		}
	}

	public int BeginUpdate()
	{
		if (pages == null)
		{
			return 0;
		}
		return pages.BeginUpdate();
	}

	public int EndUpdate()
	{
		if (pages == null)
		{
			return 0;
		}
		return pages.EndUpdate();
	}

	public void Assign(IEditPage source)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		BeginUpdate();
		try
		{
			PageKind = source.PageKind;
			PageSize = source.PageSize;
			Landscape = source.Landscape;
			Margins = source.Margins;
			HorzOffset = source.HorzOffset;
			VertOffset = source.VertOffset;
			Header = source.Header;
			Footer = source.Footer;
			PaintNumber = source.PaintNumber;
		}
		finally
		{
			EndUpdate();
		}
	}

	public Rectangle GetBounds(bool includeSpace)
	{
		Rectangle result = ((!landscape) ? new Rectangle(origin.X, origin.Y, pageSize.Width, pageSize.Height) : new Rectangle(origin.X, origin.Y, pageSize.Height, pageSize.Width));
		if (pages != null)
		{
			ISyntaxEdit owner = pages.Owner;
			int fontHeight = owner.Painter.FontHeight;
			if (pages.PageType == PageType.PageLayout)
			{
				if (!pages.DisplayWhiteSpace)
				{
					result.Height -= bottomIndent + topIndent;
					if (fontHeight != 0)
					{
						result.Height = result.Height / fontHeight * fontHeight;
					}
					result.Height += 8;
					if (includeSpace)
					{
						int num = ((index == 0) ? 4 : 0);
						result = new Rectangle(result.Left - horzOffset, result.Top - num, result.Width + horzOffset, result.Height + num + 1);
					}
				}
				else if (includeSpace)
				{
					result = new Rectangle(result.Left - horzOffset, result.Top - vertOffset, result.Width + horzOffset, result.Height + vertOffset + 3);
				}
			}
			else if (fontHeight != 0)
			{
				result.Height = result.Height / fontHeight * fontHeight;
			}
		}
		return result;
	}

	public bool ShouldSerializeLandscape()
	{
		if (pages != null)
		{
			return landscape != pages.DefaultLandscape;
		}
		return true;
	}

	public bool ShouldSerializeMargins()
	{
		if (pages != null && margins.Left == pages.DefaultMargins.Left && margins.Right == pages.DefaultMargins.Right && margins.Top == pages.DefaultMargins.Top)
		{
			return margins.Bottom != pages.DefaultMargins.Bottom;
		}
		return true;
	}

	public bool ShouldSerializePageKind()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (pages != null)
		{
			return pageKind != pages.DefaultPageKind;
		}
		return true;
	}

	public bool ShouldSerializePageSize()
	{
		if (pages != null && usePrinterSettings && pageSize.Width == pages.DefaultPageSize.Width)
		{
			return pageSize.Height != pages.DefaultPageSize.Height;
		}
		return true;
	}

	public bool ShouldSerializeHorzOffset()
	{
		return horzOffset != EditConsts.DefaultPageHorzOffset;
	}

	public bool ShouldSerializeVertOffset()
	{
		return vertOffset != EditConsts.DefaultPageVertOffset;
	}
}
