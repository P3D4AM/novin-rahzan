using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(GroupPanel), "Controls.GroupPanel.ico")]
[ToolboxItem(false)]
public class GroupPanel : PanelControl, INonClientControl
{
	private NonClientPaintHandler m_NCPainter;

	private Image m_TitleImage;

	private eTitleImagePosition m_TitleImagePosition;

	private bool m_DrawTitleBox = true;

	private bool m_IsShadowEnabled;

	private ePanelColorTable _ColorTable;

	private Rectangle m_TitleArea = Rectangle.Empty;

	[DefaultValue(ePanelColorTable.Default)]
	[Category("Appearance")]
	[Description("Indicates panel color scheme.")]
	public ePanelColorTable ColorTable
	{
		get
		{
			return _ColorTable;
		}
		set
		{
			_ColorTable = value;
			SetColorTable(value);
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Visual")]
	[Description("Indicates image that appears in title with text.")]
	public Image TitleImage
	{
		get
		{
			return m_TitleImage;
		}
		set
		{
			m_TitleImage = value;
			RefreshTextClientRectangle();
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(eTitleImagePosition.Left)]
	[Category("Visual")]
	[Description("Indicates position of the title image.")]
	public eTitleImagePosition TitleImagePosition
	{
		get
		{
			return m_TitleImagePosition;
		}
		set
		{
			m_TitleImagePosition = value;
			RefreshTextClientRectangle();
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override Color BackColor
	{
		get
		{
			return base.BackColor;
		}
		set
		{
			base.BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eScrollBarSkin ScrollbarSkin
	{
		get
		{
			return m_NCPainter.SkinScrollbars;
		}
		set
		{
			m_NCPainter.SkinScrollbars = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("")]
	public bool DrawTitleBox
	{
		get
		{
			return m_DrawTitleBox;
		}
		set
		{
			if (m_DrawTitleBox != value)
			{
				m_DrawTitleBox = value;
				InvalidateNonClient();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	public override bool TextDockConstrained
	{
		get
		{
			return base.TextDockConstrained;
		}
		set
		{
			base.TextDockConstrained = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether panel automatically provides shadows for child controls.")]
	public bool IsShadowEnabled
	{
		get
		{
			return m_IsShadowEnabled;
		}
		set
		{
			if (m_IsShadowEnabled != value)
			{
				m_IsShadowEnabled = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool AutoScroll
	{
		get
		{
			return ((ScrollableControl)this).AutoScroll;
		}
		set
		{
			((ScrollableControl)this).AutoScroll = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Point AutoScrollOffset
	{
		get
		{
			return ((Control)this).AutoScrollOffset;
		}
		set
		{
			((Control)this).AutoScrollOffset = value;
		}
	}

	ElementStyle INonClientControl.BorderStyle => Style;

	IntPtr INonClientControl.Handle => ((Control)this).Handle;

	int INonClientControl.Width => ((Control)this).Width;

	int INonClientControl.Height => ((Control)this).Height;

	bool INonClientControl.IsHandleCreated => ((Control)this).IsHandleCreated;

	Color INonClientControl.BackColor => ((Control)this).BackColor;

	public GroupPanel()
	{
		m_NCPainter = new NonClientPaintHandler(this, eScrollBarSkin.Optimized);
		m_NCPainter.BeforeBorderPaint += NCBeforeBorderPaint;
		m_NCPainter.AfterBorderPaint += NCAfterBorderPaint;
		((Control)this).SetStyle((ControlStyles)4096, true);
	}

	protected override void OnResize(EventArgs e)
	{
		InvalidateNonClient();
		base.OnResize(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_NCPainter != null)
		{
			m_NCPainter.Dispose();
		}
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_TitleImage);
		}
		base.Dispose(disposing);
	}

	public void InvalidateNonClient()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			NativeFunctions.RECT lprcUpdate = new NativeFunctions.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			NativeFunctions.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}

	protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (base.TextMarkupElement == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		int x = displayRectangle.X;
		Padding padding = ((Control)this).Padding;
		displayRectangle.X = x - ((Padding)(ref padding)).Left;
		int y = displayRectangle.Y;
		padding = ((Control)this).Padding;
		displayRectangle.Y = y - ((Padding)(ref padding)).Top;
		int width = displayRectangle.Width;
		padding = ((Control)this).Padding;
		displayRectangle.Width = width + ((Padding)(ref padding)).Horizontal;
		int height = displayRectangle.Height;
		padding = ((Control)this).Padding;
		displayRectangle.Height = height + ((Padding)(ref padding)).Vertical;
		displayRectangle.Inflate(2, 2);
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, displayRectangle);
		elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
		ElementStyleDisplay.PaintBackground(elementStyleDisplayInfo, shapeBackground: false);
		if (style.BackgroundImage != null)
		{
			ElementStyleDisplay.PaintBackgroundImage(elementStyleDisplayInfo);
		}
		if (!m_IsShadowEnabled)
		{
			return;
		}
		ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
		shadowPaintInfo.Graphics = graphics;
		shadowPaintInfo.Size = 6;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val.Visible && (!(val.BackColor == Color.Transparent) || val is GroupPanel))
			{
				if (val is GroupPanel)
				{
					GroupPanel groupPanel = val as GroupPanel;
					shadowPaintInfo.Rectangle = new Rectangle(val.Bounds.X, val.Bounds.Y + groupPanel.GetInternalClientRectangle().Y / 2, val.Bounds.Width, val.Bounds.Height - groupPanel.GetInternalClientRectangle().Y / 2);
				}
				else
				{
					shadowPaintInfo.Rectangle = val.Bounds;
				}
				ShadowPainter.Paint2(shadowPaintInfo);
			}
		}
	}

	private void NCAfterBorderPaint(object sender, CustomNCPaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Invalid comparison between Unknown and I4
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Invalid comparison between Unknown and I4
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Invalid comparison between Unknown and I4
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Invalid comparison between Unknown and I4
		Graphics graphics = e.Graphics;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (base.AntiAlias)
		{
			graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		graphics.ResetClip();
		ElementStyle elementStyle = Style;
		if (!((Control)this).Enabled)
		{
			elementStyle = elementStyle.Copy();
			elementStyle.TextColor = GetColorScheme().ItemDisabledText;
		}
		if (m_DrawTitleBox && !m_TitleArea.IsEmpty)
		{
			DisplayHelp.FillRoundedRectangle(graphics, m_TitleArea, 2, elementStyle.BackColor, elementStyle.BackColor2, -90);
			DisplayHelp.DrawRoundedRectangle(graphics, Style.BorderColor, m_TitleArea, 2);
		}
		Rectangle bounds = new Rectangle(m_NCPainter.ClientRectangle.X + 4, 1, ((Control)this).ClientRectangle.Width - 8, m_NCPainter.ClientRectangle.Y - 1);
		if (m_TitleImage != null)
		{
			Size autoSize = GetAutoSize(bounds.Width);
			if ((m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0) || (m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1))
			{
				graphics.DrawImage(m_TitleImage, bounds.X - 1, bounds.Y, m_TitleImage.Width, m_TitleImage.Height);
				bounds.X += m_TitleImage.Width;
				bounds.Width -= m_TitleImage.Width;
			}
			else if ((m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0) || (m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1))
			{
				graphics.DrawImage(m_TitleImage, bounds.Right - m_TitleImage.Width, bounds.Y, m_TitleImage.Width, m_TitleImage.Height);
				bounds.Width -= m_TitleImage.Width;
			}
			else if (m_TitleImagePosition == eTitleImagePosition.Center)
			{
				graphics.DrawImage(m_TitleImage, bounds.X + (bounds.Width - m_TitleImage.Width) / 2, bounds.Y, m_TitleImage.Width, m_TitleImage.Height);
			}
			bounds.Y = bounds.Bottom - autoSize.Height - 2;
		}
		if (base.TextMarkupElement == null)
		{
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(elementStyle, graphics, bounds);
			elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
			ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, ((Control)this).Font);
		}
		else
		{
			TextRenderingHint textRenderingHint2 = graphics.TextRenderingHint;
			if (base.AntiAlias)
			{
				graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, elementStyle.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
			Rectangle bounds2 = base.TextMarkupElement.Bounds;
			if (elementStyle.TextAlignment == eStyleTextAlignment.Center)
			{
				base.TextMarkupElement.Bounds = new Rectangle(base.TextMarkupElement.Bounds.X + (bounds.Width - base.TextMarkupElement.Bounds.Width) / 2, base.TextMarkupElement.Bounds.Y, base.TextMarkupElement.Bounds.Width, base.TextMarkupElement.Bounds.Height);
			}
			else if ((elementStyle.TextAlignment == eStyleTextAlignment.Far && (int)((Control)this).RightToLeft == 0) || ((int)((Control)this).RightToLeft == 1 && elementStyle.TextAlignment == eStyleTextAlignment.Near))
			{
				base.TextMarkupElement.Bounds = new Rectangle(bounds.Right - base.TextMarkupElement.Bounds.Width, base.TextMarkupElement.Bounds.Y, base.TextMarkupElement.Bounds.Width, base.TextMarkupElement.Bounds.Height);
			}
			base.TextMarkupElement.Render(d);
			graphics.TextRenderingHint = textRenderingHint2;
			base.TextMarkupElement.Bounds = bounds2;
		}
		graphics.TextRenderingHint = textRenderingHint;
		graphics.SmoothingMode = smoothingMode;
	}

	private void NCBeforeBorderPaint(object sender, CustomNCPaintEventArgs e)
	{
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Invalid comparison between Unknown and I4
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Invalid comparison between Unknown and I4
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		m_TitleArea = Rectangle.Empty;
		if (((Control)this).Text != null && ((Control)this).Text.Length > 0)
		{
			Size autoSize = GetAutoSize(((Control)this).ClientRectangle.Width - 8);
			Rectangle rectangle = new Rectangle(m_NCPainter.ClientRectangle.X + 3, 0, autoSize.Width, m_NCPainter.ClientRectangle.Y);
			Size size = new Size(m_NCPainter.ClientRectangle.Width - 8, m_NCPainter.ClientRectangle.Y);
			if (m_TitleImage != null && ((m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0)))
			{
				size.Width -= m_TitleImage.Width;
			}
			else if (m_TitleImage != null && ((m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1) || (m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0)))
			{
				rectangle.X += m_TitleImage.Width;
				size.Width -= m_TitleImage.Width;
			}
			if (Style.TextAlignment == eStyleTextAlignment.Center)
			{
				rectangle.X += (size.Width - rectangle.Width) / 2;
			}
			else if (Style.TextAlignment == eStyleTextAlignment.Far || ((int)((Control)this).RightToLeft == 1 && Style.TextAlignment == eStyleTextAlignment.Near))
			{
				rectangle.X += size.Width - rectangle.Width;
			}
			if (!rectangle.IsEmpty)
			{
				rectangle.Inflate(2, 0);
				rectangle.Width += 3;
			}
			e.Graphics.SetClip(rectangle, (CombineMode)4);
			m_TitleArea = rectangle;
		}
		if (m_TitleImage != null)
		{
			Rectangle rectangle2 = new Rectangle(m_NCPainter.ClientRectangle.X + 3, 0, m_TitleImage.Width, m_TitleImage.Height);
			if ((m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0))
			{
				rectangle2.X = m_NCPainter.ClientRectangle.Right - rectangle2.Width - 4;
			}
			else if (m_TitleImagePosition == eTitleImagePosition.Center)
			{
				rectangle2.X = m_NCPainter.ClientRectangle.X;
				rectangle2.X += (m_NCPainter.ClientRectangle.Width - rectangle2.Width) / 2;
			}
			e.Graphics.SetClip(rectangle2, (CombineMode)4);
			if (m_TitleArea.IsEmpty)
			{
				m_TitleArea = rectangle2;
			}
			else
			{
				m_TitleArea = Rectangle.Union(rectangle2, m_TitleArea);
			}
		}
	}

	private Size GetAutoSize(int preferedWidth)
	{
		Size result = Size.Empty;
		if (base.TextMarkupElement != null)
		{
			result = ((preferedWidth != 0) ? GetMarkupSize(preferedWidth) : base.TextMarkupElement.Bounds.Size);
			result.Width += 4;
		}
		else if (((Control)this).Text.Length > 0)
		{
			Font font = ((Control)this).Font;
			if (Style.Font != null)
			{
				font = Style.Font;
			}
			eTextFormat format = eTextFormat.NoPrefix | eTextFormat.SingleLine;
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				result = ((preferedWidth > 0) ? TextDrawing.MeasureString(val, ((Control)this).Text, font, preferedWidth, format) : TextDrawing.MeasureString(val, ((Control)this).Text, font, 0, format));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			result.Width += 2;
			result.Height += 2;
		}
		if (result.IsEmpty)
		{
			return result;
		}
		result.Width += Style.MarginLeft + Style.MarginRight;
		result.Height += Style.MarginTop + Style.MarginBottom;
		return result;
	}

	private Size GetMarkupSize(int proposedWidth)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (base.TextMarkupElement != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, proposedWidth, 500);
			rectangle.Inflate(-2, -2);
			Graphics val = CreateGraphics();
			BodyElement bodyElement = MarkupParser.Parse(((Control)this).Text);
			try
			{
				if (base.AntiAlias)
				{
					val.SmoothingMode = (SmoothingMode)4;
					val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
				bodyElement.Measure(rectangle.Size, d);
				result = bodyElement.Bounds.Size;
			}
			finally
			{
				val.Dispose();
			}
		}
		return result;
	}

	protected override void RefreshTextClientRectangle()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		if (m_NCPainter != null)
		{
			Rectangle clientTextRectangle = new Rectangle(m_NCPainter.ClientRectangle.X, 0, m_NCPainter.ClientRectangle.Width, ((Control)this).Height / 2);
			clientTextRectangle.Inflate(-2, 0);
			if (m_TitleImage != null)
			{
				if ((m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0) || (m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1))
				{
					clientTextRectangle.X += m_TitleImage.Width;
					clientTextRectangle.Width -= m_TitleImage.Width;
				}
				else if ((m_TitleImagePosition == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (m_TitleImagePosition == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0))
				{
					clientTextRectangle.Width -= m_TitleImage.Width;
				}
				Size markupSize = GetMarkupSize(clientTextRectangle.Width);
				clientTextRectangle.Y = Math.Max(0, m_NCPainter.ClientRectangle.Y - markupSize.Height - 4);
			}
			base.ClientTextRectangle = clientTextRectangle;
		}
		else
		{
			base.ClientTextRectangle = ((Control)this).ClientRectangle;
		}
		ResizeMarkup();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetColorTable(ePanelColorTable colorScheme)
	{
		ColorFactory empty = ColorFactory.Empty;
		CanvasColor = SystemColors.Control;
		ResetStyle();
		ColorSchemeStyle = eDotNetBarStyle.Office2007;
		switch (colorScheme)
		{
		case ePanelColorTable.Default:
			SetDefaultPanelStyle();
			break;
		case ePanelColorTable.Green:
			Style.BackColor2 = empty.GetColor(10272651);
			Style.BackColorGradientAngle = 90;
			Style.BackColor = empty.GetColor(12835257);
			Style.Border = eStyleBorderType.Solid;
			Style.BorderWidth = 1;
			Style.BorderColor = empty.GetColor(7513178);
			Style.CornerDiameter = 4;
			Style.CornerType = eCornerType.Rounded;
			Style.TextAlignment = eStyleTextAlignment.Center;
			Style.TextColor = empty.GetColor(3951135);
			Style.TextLineAlignment = eStyleTextAlignment.Near;
			break;
		case ePanelColorTable.Orange:
			Style.BackColor = empty.GetColor(16433295);
			Style.BackColor2 = empty.GetColor(16225862);
			Style.BorderColor = empty.GetColor(9914374);
			Style.TextColor = empty.GetColor(8338694);
			Style.BackColorGradientAngle = 90;
			Style.Border = eStyleBorderType.Solid;
			Style.BorderWidth = 1;
			Style.CornerDiameter = 4;
			Style.CornerType = eCornerType.Rounded;
			Style.TextAlignment = eStyleTextAlignment.Center;
			Style.TextLineAlignment = eStyleTextAlignment.Near;
			break;
		case ePanelColorTable.Red:
			Style.BackColor = empty.GetColor(15056831);
			Style.BackColor2 = empty.GetColor(13866646);
			Style.BorderColor = empty.GetColor(9778996);
			Style.TextColor = empty.GetColor(6497315);
			Style.BackColorGradientAngle = 90;
			Style.Border = eStyleBorderType.Solid;
			Style.BorderWidth = 1;
			Style.CornerDiameter = 4;
			Style.CornerType = eCornerType.Rounded;
			Style.TextAlignment = eStyleTextAlignment.Center;
			Style.TextLineAlignment = eStyleTextAlignment.Near;
			break;
		case ePanelColorTable.Yellow:
			Style.BackColor = empty.GetColor(16774066);
			Style.BackColor2 = empty.GetColor(16439621);
			Style.BorderColor = empty.GetColor(15635217);
			Style.TextColor = empty.GetColor(4144896);
			Style.BackColorGradientAngle = 90;
			Style.Border = eStyleBorderType.Solid;
			Style.BorderWidth = 1;
			Style.CornerDiameter = 4;
			Style.CornerType = eCornerType.Rounded;
			Style.TextAlignment = eStyleTextAlignment.Center;
			Style.TextLineAlignment = eStyleTextAlignment.Near;
			break;
		case ePanelColorTable.Magenta:
			Style.BackColor = empty.GetColor(15700404);
			Style.BackColor2 = empty.GetColor(15100054);
			Style.BorderColor = empty.GetColor(11609939);
			Style.TextColor = empty.GetColor(9315912);
			Style.BackColorGradientAngle = 90;
			Style.Border = eStyleBorderType.Solid;
			Style.BorderWidth = 1;
			Style.CornerDiameter = 4;
			Style.CornerType = eCornerType.Rounded;
			Style.TextAlignment = eStyleTextAlignment.Center;
			Style.TextLineAlignment = eStyleTextAlignment.Near;
			break;
		}
	}

	public void SetDefaultPanelStyle()
	{
		CanvasColor = SystemColors.Control;
		ResetStyle();
		ColorSchemeStyle = eDotNetBarStyle.Office2007;
		Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		Style.BackColorGradientAngle = 90;
		Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		Style.Border = eStyleBorderType.Solid;
		Style.BorderWidth = 1;
		Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		Style.CornerDiameter = 4;
		Style.CornerType = eCornerType.Rounded;
		Style.TextAlignment = eStyleTextAlignment.Center;
		Style.TextColorSchemePart = eColorSchemePart.PanelText;
		Style.TextLineAlignment = eStyleTextAlignment.Near;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		((Control)this).SetBoundsCore(x, y, width, height, specified);
		if (((Component)(object)this).DesignMode || ((specified & 8) != 8 && (specified & 4) != 4))
		{
			return;
		}
		ElementStyle style = Style;
		if ((specified & 8) == 8)
		{
			if (m_DrawTitleBox && !string.IsNullOrEmpty(((Control)this).Text))
			{
				height -= GetNonClientTopHeight() + 6;
			}
			if (style != null && m_NCPainter != null)
			{
				if (style.PaintTopBorder)
				{
					height -= m_NCPainter.GetTopBorderWidth(style);
				}
				if (style.PaintBottomBorder)
				{
					height -= m_NCPainter.GetBottomBorderWidth(style);
				}
			}
		}
		if ((specified & 4) == 4 && style != null && m_NCPainter != null)
		{
			if (style.PaintLeftBorder)
			{
				width -= m_NCPainter.GetLeftBorderWidth(style);
			}
			if (style.PaintRightBorder)
			{
				width -= m_NCPainter.GetRightBorderWidth(style);
			}
		}
		Type typeFromHandle = typeof(Control);
		try
		{
			typeFromHandle.InvokeMember("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { width });
			typeFromHandle.InvokeMember("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { height });
		}
		catch
		{
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (m_NCPainter == null)
		{
			base.WndProc(ref m);
		}
		else if (((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 522)
		{
			if (m_NCPainter.WndProc(ref m))
			{
				BaseWndProc(ref m);
			}
			RefreshTextClientRectangle();
		}
		else if (m_NCPainter.WndProc(ref m))
		{
			BaseWndProc(ref m);
		}
	}

	void INonClientControl.BaseWndProc(ref Message m)
	{
		BaseWndProc(ref m);
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		return null;
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics g)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, g, GetColorScheme());
		itemPaintArgs.Renderer = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)(object)this).DesignMode && WinApi.IsGlassEnabled;
		return itemPaintArgs;
	}

	void INonClientControl.PaintBackground(PaintEventArgs e)
	{
		((ScrollableControl)this).OnPaintBackground(e);
	}

	Point INonClientControl.PointToScreen(Point client)
	{
		return ((Control)this).PointToScreen(client);
	}

	void INonClientControl.RenderNonClient(Graphics g)
	{
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle r)
	{
		Size autoSize = GetAutoSize(r.Width);
		if (m_TitleImage != null)
		{
			autoSize.Height = Math.Max(m_TitleImage.Height, autoSize.Height);
		}
		if (autoSize.Height > r.Height)
		{
			autoSize.Height = r.Height - 8;
		}
		r.Y += autoSize.Height;
		r.Height -= autoSize.Height;
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle r)
	{
		if (((Control)this).Text != "")
		{
			int nonClientTopHeight = GetNonClientTopHeight();
			r.Y += nonClientTopHeight;
			r.Height -= nonClientTopHeight;
		}
	}

	private int GetNonClientTopHeight()
	{
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		int num = (int)((float)font.Height * 0.7f);
		if (m_TitleImage != null)
		{
			num = Math.Max(m_TitleImage.Height - (font.Height - num - 1), num);
		}
		return num;
	}

	internal Rectangle GetInternalClientRectangle()
	{
		return m_NCPainter.ClientRectangle;
	}

	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	void INonClientControl.set_Enabled(bool value)
	{
		((Control)this).Enabled = value;
	}
}
