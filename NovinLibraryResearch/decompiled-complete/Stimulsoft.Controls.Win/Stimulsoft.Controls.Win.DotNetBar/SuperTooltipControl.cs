using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class SuperTooltipControl : PanelControl
{
	private const long WS_POPUP = 2147483648L;

	private const long WS_CLIPSIBLINGS = 67108864L;

	private const long WS_CLIPCHILDREN = 33554432L;

	private const long WS_EX_TOOLWINDOW = 128L;

	private const long WS_EX_TOPMOST = 8L;

	private string m_HeaderText = "";

	private BodyElement m_HeaderMarkup;

	private string m_FooterText = "";

	private BodyElement m_FooterMarkup;

	private Image m_FooterImage;

	private Image m_BodyImage;

	private Size m_MinimumTooltipSize = new Size(100, 18);

	private bool m_HeaderVisible = true;

	private bool m_FooterVisible = true;

	private bool m_FooterSeparator = true;

	private int m_FooterImageSpacing = 8;

	private PopupShadow m_DropShadow;

	private bool m_StandardControl;

	private BodyElement m_BodyMarkup;

	private bool m_MouseActivateEnabled = true;

	private int m_MaximumWidth;

	private bool m_ShowTooltipDescription = true;

	private eTooltipColor m_PredefinedColor;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether complete tooltip is shown including header, body and footer. When set to false only tooltip header is shown.")]
	public bool ShowTooltipDescription
	{
		get
		{
			return m_ShowTooltipDescription;
		}
		set
		{
			m_ShowTooltipDescription = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates maximum width of the super tooltip.")]
	public int MaximumWidth
	{
		get
		{
			return m_MaximumWidth;
		}
		set
		{
			m_MaximumWidth = value;
		}
	}

	[DefaultValue(eTooltipColor.Default)]
	public eTooltipColor PredefinedColor
	{
		get
		{
			return m_PredefinedColor;
		}
		set
		{
			m_PredefinedColor = value;
			ApplyColors(value);
			((Control)this).Refresh();
		}
	}

	internal BodyElement BodyMarkup => m_BodyMarkup;

	public Size MinimumTooltipSize
	{
		get
		{
			return m_MinimumTooltipSize;
		}
		set
		{
			m_MinimumTooltipSize = value;
		}
	}

	[DefaultValue(null)]
	public Image BodyImage
	{
		get
		{
			return m_BodyImage;
		}
		set
		{
			m_BodyImage = value;
		}
	}

	[DefaultValue(null)]
	public Image FooterImage
	{
		get
		{
			return m_FooterImage;
		}
		set
		{
			m_FooterImage = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	public string HeaderText
	{
		get
		{
			return m_HeaderText;
		}
		set
		{
			m_HeaderText = value;
			if (MarkupParser.IsMarkup(ref m_HeaderText))
			{
				m_HeaderMarkup = MarkupParser.Parse(m_HeaderText);
				if (m_HeaderMarkup != null)
				{
					m_HeaderMarkup.HyperLinkClick += Markup_HyperLinkClick;
				}
			}
			else
			{
				m_HeaderMarkup = null;
			}
		}
	}

	[DefaultValue(true)]
	public bool HeaderVisible
	{
		get
		{
			return m_HeaderVisible;
		}
		set
		{
			m_HeaderVisible = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	public string FooterText
	{
		get
		{
			return m_FooterText;
		}
		set
		{
			m_FooterText = value;
			if (MarkupParser.IsMarkup(ref m_FooterText))
			{
				m_FooterMarkup = MarkupParser.Parse(m_FooterText);
				if (m_FooterMarkup != null)
				{
					m_FooterMarkup.HyperLinkClick += Markup_HyperLinkClick;
				}
			}
			else
			{
				m_FooterMarkup = null;
			}
		}
	}

	[DefaultValue(true)]
	public bool FooterVisible
	{
		get
		{
			return m_FooterVisible;
		}
		set
		{
			m_FooterVisible = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	public bool FooterSeparator
	{
		get
		{
			return m_FooterSeparator;
		}
		set
		{
			m_FooterSeparator = value;
		}
	}

	[DefaultValue(true)]
	public bool MouseActivateEnabled
	{
		get
		{
			return m_MouseActivateEnabled;
		}
		set
		{
			m_MouseActivateEnabled = value;
		}
	}

	public bool StandardControl
	{
		get
		{
			return m_StandardControl;
		}
		set
		{
			m_StandardControl = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			if (!m_StandardControl)
			{
				createParams.Style = -2046820352;
				createParams.ExStyle = 136;
				createParams.Caption = "";
			}
			return createParams;
		}
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return proposedSize;
		}
		return GetAutoSize();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string text = ((Control)this).Text;
		if (MarkupParser.IsMarkup(ref text))
		{
			m_BodyMarkup = MarkupParser.Parse(text);
			if (m_BodyMarkup != null)
			{
				m_BodyMarkup.HyperLinkClick += Markup_HyperLinkClick;
			}
		}
		else
		{
			m_BodyMarkup = null;
		}
		base.OnTextChanged(e);
	}

	private void Markup_HyperLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (m_BodyMarkup != null)
		{
			m_BodyMarkup.MouseMove((Control)(object)this, e);
		}
		if (m_HeaderMarkup != null)
		{
			m_HeaderMarkup.MouseMove((Control)(object)this, e);
		}
		if (m_FooterMarkup != null)
		{
			m_FooterMarkup.MouseMove((Control)(object)this, e);
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (m_BodyMarkup != null)
		{
			m_BodyMarkup.MouseDown((Control)(object)this, e);
		}
		if (m_HeaderMarkup != null)
		{
			m_HeaderMarkup.MouseDown((Control)(object)this, e);
		}
		if (m_FooterMarkup != null)
		{
			m_FooterMarkup.MouseDown((Control)(object)this, e);
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (m_BodyMarkup != null)
		{
			m_BodyMarkup.MouseUp((Control)(object)this, e);
		}
		if (m_HeaderMarkup != null)
		{
			m_HeaderMarkup.MouseUp((Control)(object)this, e);
		}
		if (m_FooterMarkup != null)
		{
			m_FooterMarkup.MouseUp((Control)(object)this, e);
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (m_BodyMarkup != null)
		{
			m_BodyMarkup.MouseLeave((Control)(object)this);
		}
		if (m_HeaderMarkup != null)
		{
			m_HeaderMarkup.MouseLeave((Control)(object)this);
		}
		if (m_FooterMarkup != null)
		{
			m_FooterMarkup.MouseLeave((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (m_BodyMarkup != null)
		{
			m_BodyMarkup.Click((Control)(object)this);
		}
		if (m_HeaderMarkup != null)
		{
			m_HeaderMarkup.Click((Control)(object)this);
		}
		if (m_FooterMarkup != null)
		{
			m_FooterMarkup.Click((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && !m_MouseActivateEnabled)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	protected override void PaintInnerContent(PaintEventArgs e, ElementStyle style, bool paintText)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Invalid comparison between Unknown and I4
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Invalid comparison between Unknown and I4
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Invalid comparison between Unknown and I4
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Invalid comparison between Unknown and I4
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Invalid comparison between Unknown and I4
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Invalid comparison between Unknown and I4
		base.PaintInnerContent(e, style, paintText: false);
		if (!paintText)
		{
			return;
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(Style);
		clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(Style);
		clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(Style);
		clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(Style);
		if (clientRectangle.Width <= 4 || clientRectangle.Height <= 4)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		Font val = new Font(font, (FontStyle)((m_ShowTooltipDescription || m_HeaderText == "") ? 1 : 0));
		Padding headerPadding = GetHeaderPadding();
		Padding footerPadding = GetFooterPadding();
		Padding textPadding = GetTextPadding();
		Padding imagePadding = GetImagePadding();
		try
		{
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, Rectangle.Empty);
			if (m_HeaderText != "" && HeaderVisible)
			{
				Rectangle rectangle = new Rectangle(clientRectangle.X + headerPadding.Left, clientRectangle.Y + headerPadding.Top, clientRectangle.Width - headerPadding.Horizontal, clientRectangle.Height - headerPadding.Vertical);
				Size size = Size.Empty;
				if (m_HeaderMarkup == null)
				{
					elementStyleDisplayInfo.Bounds = rectangle;
					eTextFormat eTextFormat2 = eTextFormat.WordBreak;
					if ((int)((Control)this).RightToLeft == 1)
					{
						eTextFormat2 |= eTextFormat.RightToLeft;
					}
					ElementStyleDisplay.PaintText(elementStyleDisplayInfo, m_HeaderText, val, useDefaultFont: true, eTextFormat2);
					size = TextDrawing.MeasureString(graphics, m_HeaderText, val, rectangle.Width, eTextFormat2);
				}
				else if (rectangle.Width > 0 && rectangle.Height > 0)
				{
					MarkupDrawContext d = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle, hotKeyPrefixVisible: true);
					m_HeaderMarkup.Arrange(rectangle, d);
					m_HeaderMarkup.Render(d);
					size = m_HeaderMarkup.Bounds.Size;
				}
				size.Width += headerPadding.Horizontal;
				size.Height += headerPadding.Vertical;
				clientRectangle.Y += size.Height;
				clientRectangle.Height -= size.Height;
			}
			if (m_FooterText != "" && FooterVisible && clientRectangle.Width > 0 && clientRectangle.Height > 0 && (m_ShowTooltipDescription || m_HeaderText == ""))
			{
				Size empty = Size.Empty;
				eTextFormat eTextFormat3 = eTextFormat.WordBreak;
				empty = ((m_FooterMarkup != null) ? m_FooterMarkup.Bounds.Size : TextDrawing.MeasureString(graphics, m_FooterText, val, clientRectangle.Width - footerPadding.Horizontal, eTextFormat3));
				if (m_FooterImage != null && m_FooterImage.Height > empty.Height)
				{
					empty.Height = m_FooterImage.Height;
				}
				Rectangle rectangle2 = new Rectangle(clientRectangle.X + footerPadding.Left, clientRectangle.Bottom - empty.Height - footerPadding.Bottom, clientRectangle.Width - footerPadding.Horizontal, empty.Height);
				if (FooterSeparator)
				{
					Pen val2 = new Pen(style.BorderColor, 1f);
					try
					{
						graphics.DrawLine(val2, 0, rectangle2.Y - footerPadding.Top - 1, ((Control)this).ClientRectangle.Right, rectangle2.Y - footerPadding.Top - 1);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				if (m_FooterImage != null)
				{
					graphics.DrawImage(m_FooterImage, new Rectangle(rectangle2.X, rectangle2.Y + (rectangle2.Height - m_FooterImage.Height) / 2, m_FooterImage.Width, m_FooterImage.Height));
					rectangle2.X += m_FooterImage.Width + m_FooterImageSpacing;
					rectangle2.Width -= m_FooterImage.Width + m_FooterImageSpacing;
				}
				if (rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					if (m_FooterMarkup == null)
					{
						elementStyleDisplayInfo.Bounds = rectangle2;
						eTextFormat3 |= eTextFormat.VerticalCenter;
						if ((int)((Control)this).RightToLeft == 1)
						{
							eTextFormat3 |= eTextFormat.RightToLeft;
						}
						ElementStyleDisplay.PaintText(elementStyleDisplayInfo, m_FooterText, val, useDefaultFont: true, eTextFormat3);
					}
					else
					{
						MarkupDrawContext d2 = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle2, hotKeyPrefixVisible: true);
						m_FooterMarkup.Arrange(rectangle2, d2);
						m_FooterMarkup.Render(d2);
						empty = m_FooterMarkup.Bounds.Size;
					}
				}
				empty.Width += footerPadding.Horizontal;
				empty.Height += footerPadding.Vertical;
				clientRectangle.Height -= empty.Height;
			}
			if (!m_ShowTooltipDescription && !(m_HeaderText == ""))
			{
				return;
			}
			if (m_BodyImage != null)
			{
				Rectangle rectangle3 = new Rectangle(clientRectangle.X + imagePadding.Left, clientRectangle.Y + imagePadding.Top, m_BodyImage.Width, m_BodyImage.Height);
				graphics.DrawImage(m_BodyImage, rectangle3);
				clientRectangle.X += imagePadding.Horizontal + m_BodyImage.Width;
				clientRectangle.Width -= imagePadding.Horizontal + m_BodyImage.Width;
			}
			if (!(((Control)this).Text != "") || clientRectangle.Width <= 0 || clientRectangle.Height <= 0)
			{
				return;
			}
			Rectangle rectangle4 = new Rectangle(clientRectangle.X + textPadding.Left, clientRectangle.Y + textPadding.Top, clientRectangle.Width - textPadding.Horizontal, clientRectangle.Height - textPadding.Vertical);
			if (m_BodyMarkup == null)
			{
				elementStyleDisplayInfo.Bounds = rectangle4;
				if (rectangle4.Width > 0 && rectangle4.Height > 0)
				{
					eTextFormat eTextFormat4 = elementStyleDisplayInfo.Style.TextFormat;
					if ((int)((Control)this).RightToLeft == 1)
					{
						eTextFormat4 |= eTextFormat.RightToLeft;
					}
					ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, font, useDefaultFont: false, eTextFormat4);
				}
			}
			else if (rectangle4.Width > 0 && rectangle4.Height > 0)
			{
				MarkupDrawContext d3 = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle4, hotKeyPrefixVisible: true);
				m_BodyMarkup.Arrange(rectangle4, d3);
				m_BodyMarkup.Render(d3);
			}
		}
		finally
		{
			val.Dispose();
			val = null;
		}
	}

	private Padding GetHeaderPadding()
	{
		return new Padding(6, 6, 2, 2);
	}

	private Padding GetFooterPadding()
	{
		return new Padding(6, 6, 4, 6);
	}

	private Padding GetTextPadding()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		if ((m_HeaderText == "" || !HeaderVisible) && (m_FooterText == "" || !FooterVisible))
		{
			return new Padding(1, 1, 1, 1);
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Padding(6, 14, 4, 4);
		}
		return new Padding(14, 6, 4, 4);
	}

	private Padding GetImagePadding()
	{
		return new Padding(6, 6, 6, 6);
	}

	public void RecalcSize()
	{
		((Control)this).Size = GetAutoSize();
	}

	public Size GetFixedWidthSize(int width)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Invalid comparison between Unknown and I4
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Invalid comparison between Unknown and I4
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Invalid comparison between Unknown and I4
		Padding headerPadding = GetHeaderPadding();
		Padding footerPadding = GetFooterPadding();
		Padding textPadding = GetTextPadding();
		Padding imagePadding = GetImagePadding();
		Size empty = Size.Empty;
		int num = width - ElementStyleLayout.HorizontalStyleWhiteSpace(Style);
		if (num <= 6)
		{
			num = 16;
		}
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		Font val = new Font(font, (FontStyle)((m_ShowTooltipDescription || m_HeaderText == "") ? 1 : 0));
		Graphics val2 = CreateGraphics();
		if (base.AntiAlias)
		{
			val2.SmoothingMode = (SmoothingMode)4;
			val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		try
		{
			if (m_HeaderText != "" && HeaderVisible)
			{
				Size empty2 = Size.Empty;
				int num2 = num - headerPadding.Horizontal;
				if (num2 <= 1)
				{
					num2 = 2;
				}
				if (m_HeaderMarkup == null)
				{
					empty2 = TextDrawing.MeasureString(val2, m_HeaderText, val, num2, eTextFormat.WordBreak);
				}
				else
				{
					MarkupDrawContext d = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					m_HeaderMarkup.Measure(new Size(num2, 1), d);
					empty2 = m_HeaderMarkup.Bounds.Size;
					m_HeaderMarkup.Arrange(new Rectangle(Point.Empty, empty2), d);
					empty2 = m_HeaderMarkup.Bounds.Size;
				}
				empty2.Height += 2;
				empty2.Height += headerPadding.Vertical;
				empty.Height += empty2.Height;
			}
			if (m_FooterText != "" && FooterVisible && (m_ShowTooltipDescription || m_HeaderText == ""))
			{
				Size empty3 = Size.Empty;
				int num3 = num - footerPadding.Horizontal;
				if (m_FooterImage != null)
				{
					num3 -= m_FooterImage.Width + m_FooterImageSpacing;
				}
				if (num3 <= 1)
				{
					num3 = 2;
				}
				if (m_FooterMarkup == null)
				{
					empty3 = TextDrawing.MeasureString(val2, m_FooterText, val, num3, eTextFormat.WordBreak);
				}
				else
				{
					MarkupDrawContext d2 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					m_FooterMarkup.Measure(new Size(num3, 1), d2);
					empty3 = m_FooterMarkup.Bounds.Size;
					m_FooterMarkup.Arrange(new Rectangle(Point.Empty, empty3), d2);
					empty3 = m_FooterMarkup.Bounds.Size;
				}
				empty3.Height += 2;
				if (m_FooterImage != null && m_FooterImage.Height > empty3.Height)
				{
					empty3.Height = m_FooterImage.Height;
				}
				empty3.Height += footerPadding.Vertical;
				empty.Height += empty3.Height;
			}
			if (m_ShowTooltipDescription || m_HeaderText == "")
			{
				if (((Control)this).Text != "")
				{
					int num4 = num - textPadding.Horizontal;
					if (m_BodyImage != null)
					{
						num4 -= m_BodyImage.Width + imagePadding.Horizontal;
					}
					if (num4 <= 1)
					{
						num4 = 2;
					}
					Size empty4 = Size.Empty;
					if (m_BodyMarkup == null)
					{
						empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num4, Style.TextFormat);
					}
					else
					{
						MarkupDrawContext d3 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
						m_BodyMarkup.Measure(new Size(num4, 1), d3);
						empty4 = m_BodyMarkup.Bounds.Size;
						if ((double)empty4.Height > (double)empty4.Width * 1.75)
						{
							num4 = (int)((double)empty4.Height * 0.75);
						}
						m_BodyMarkup.Arrange(new Rectangle(Point.Empty, new Size(num4, 1)), d3);
						empty4 = m_BodyMarkup.Bounds.Size;
					}
					empty4.Height += textPadding.Vertical;
					if (m_BodyImage != null && m_BodyImage.Height + imagePadding.Vertical > empty4.Height)
					{
						empty4.Height = m_BodyImage.Height + imagePadding.Vertical;
					}
					empty.Height += empty4.Height;
				}
				else if (m_BodyImage != null)
				{
					empty.Height += m_BodyImage.Height + imagePadding.Vertical;
				}
			}
		}
		finally
		{
			val2.Dispose();
			val.Dispose();
		}
		empty.Width = width;
		empty.Height += ElementStyleLayout.VerticalStyleWhiteSpace(Style);
		return empty;
	}

	private Size GetAutoSize()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Invalid comparison between Unknown and I4
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Invalid comparison between Unknown and I4
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Invalid comparison between Unknown and I4
		Padding headerPadding = GetHeaderPadding();
		Padding footerPadding = GetFooterPadding();
		Padding textPadding = GetTextPadding();
		Padding imagePadding = GetImagePadding();
		Size empty = Size.Empty;
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		Font val = new Font(font, (FontStyle)((m_ShowTooltipDescription || m_HeaderText == "") ? 1 : 0));
		Graphics val2 = CreateGraphics();
		if (base.AntiAlias)
		{
			val2.SmoothingMode = (SmoothingMode)4;
			val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		try
		{
			if (m_HeaderText != "" && HeaderVisible)
			{
				Size empty2 = Size.Empty;
				if (m_HeaderMarkup == null)
				{
					empty2 = TextDrawing.MeasureString(val2, m_HeaderText, val);
				}
				else
				{
					MarkupDrawContext d = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					m_HeaderMarkup.Measure(new Size(1024, 1), d);
					empty2 = m_HeaderMarkup.Bounds.Size;
					m_HeaderMarkup.Arrange(new Rectangle(Point.Empty, empty2), d);
					empty2 = m_HeaderMarkup.Bounds.Size;
				}
				empty2.Width += 2;
				empty2.Height += 2;
				empty2.Width += headerPadding.Horizontal;
				empty2.Height += headerPadding.Vertical;
				if (empty2.Width > empty.Width)
				{
					empty.Width = empty2.Width;
				}
				empty.Height += empty2.Height;
			}
			if (m_FooterText != "" && FooterVisible && (m_ShowTooltipDescription || m_HeaderText == ""))
			{
				Size empty3 = Size.Empty;
				if (m_FooterMarkup == null)
				{
					empty3 = TextDrawing.MeasureString(val2, m_FooterText, val);
				}
				else
				{
					MarkupDrawContext d2 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					m_FooterMarkup.Measure(new Size(1024, 1), d2);
					empty3 = m_FooterMarkup.Bounds.Size;
					m_FooterMarkup.Arrange(new Rectangle(Point.Empty, empty3), d2);
					empty3 = m_FooterMarkup.Bounds.Size;
				}
				empty3.Width += 2;
				empty3.Height += 2;
				if (m_FooterImage != null)
				{
					empty3.Width += m_FooterImage.Width + m_FooterImageSpacing;
					if (m_FooterImage.Height > empty3.Height)
					{
						empty3.Height = m_FooterImage.Height;
					}
				}
				empty3.Width += footerPadding.Horizontal;
				empty3.Height += footerPadding.Vertical;
				if (empty3.Width > empty.Width)
				{
					empty.Width = empty3.Width;
				}
				empty.Height += empty3.Height;
			}
			if (m_ShowTooltipDescription || m_HeaderText == "")
			{
				if (((Control)this).Text != "")
				{
					int num = empty.Width;
					if (num < m_MinimumTooltipSize.Width)
					{
						num = m_MinimumTooltipSize.Width;
					}
					if (m_MaximumWidth > 0)
					{
						num = 5000;
					}
					Size empty4 = Size.Empty;
					if (m_BodyMarkup == null)
					{
						empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num, Style.TextFormat);
						if ((double)empty4.Height > (double)empty4.Width * 1.75 && empty4.Width > font.Height * 2)
						{
							num = (int)((double)empty4.Height * 0.75);
							empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num, Style.TextFormat);
						}
					}
					else
					{
						MarkupDrawContext d3 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
						m_BodyMarkup.Measure(new Size(num, 1), d3);
						empty4 = m_BodyMarkup.Bounds.Size;
						if ((double)empty4.Height > (double)empty4.Width * 1.75 && empty4.Width > font.Height * 2)
						{
							num = (int)((double)empty4.Height * 0.75);
						}
						m_BodyMarkup.Arrange(new Rectangle(Point.Empty, new Size(num, 1)), d3);
						empty4 = m_BodyMarkup.Bounds.Size;
					}
					empty4.Width += textPadding.Horizontal;
					empty4.Height += textPadding.Vertical;
					if (m_BodyImage != null)
					{
						empty4.Width += m_BodyImage.Width + imagePadding.Horizontal;
						if (m_BodyImage.Height + imagePadding.Vertical > empty4.Height)
						{
							empty4.Height = m_BodyImage.Height + imagePadding.Vertical;
						}
					}
					if (empty4.Width > empty.Width)
					{
						empty.Width = empty4.Width;
					}
					empty.Height += empty4.Height;
				}
				else if (m_BodyImage != null)
				{
					if (m_BodyImage.Width + imagePadding.Horizontal > empty.Width)
					{
						empty.Width = m_BodyImage.Width + imagePadding.Horizontal;
					}
					empty.Height += m_BodyImage.Height + imagePadding.Vertical;
				}
			}
		}
		finally
		{
			val2.Dispose();
			val.Dispose();
		}
		empty.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(Style);
		empty.Height += ElementStyleLayout.VerticalStyleWhiteSpace(Style);
		if (empty.Width < m_MinimumTooltipSize.Width)
		{
			empty.Width = m_MinimumTooltipSize.Width;
		}
		if (empty.Height < m_MinimumTooltipSize.Height)
		{
			empty.Height = m_MinimumTooltipSize.Height;
		}
		return empty;
	}

	private void ApplyColors(eTooltipColor c)
	{
		ResetStyle();
		ResetStyleMouseOver();
		ResetStyleMouseDown();
		ElementStyle style = Style;
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		TypeDescriptor.GetProperties(style)["WordWrap"].SetValue(style, true);
		TypeDescriptor.GetProperties(style)["TextAlignment"].SetValue(style, eStyleTextAlignment.Near);
		TypeDescriptor.GetProperties(style)["TextLineAlignment"].SetValue(style, eStyleTextAlignment.Near);
		TypeDescriptor.GetProperties(style)["BackColorGradientAngle"].SetValue(style, 90);
		TypeDescriptor.GetProperties(style)["BorderColor"].SetValue(style, Color.DimGray);
		TypeDescriptor.GetProperties(style)["BorderWidth"].SetValue(style, 1);
		TypeDescriptor.GetProperties(style)["TextColor"].SetValue(style, Color.Black);
		TypeDescriptor.GetProperties(style)["Border"].SetValue(style, eStyleBorderType.Solid);
		TypeDescriptor.GetProperties(style)["CornerType"].SetValue(style, eCornerType.Rounded);
		TypeDescriptor.GetProperties(style)["CornerDiameter"].SetValue(style, 2);
		switch (c)
		{
		case eTooltipColor.System:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
				color = colorTable.SuperTooltip.BackgroundColors.Start;
				color2 = colorTable.SuperTooltip.BackgroundColors.End;
				style.TextColor = colorTable.SuperTooltip.TextColor;
			}
			break;
		case eTooltipColor.Apple:
			color = Color.FromArgb(232, 248, 224);
			color2 = Color.FromArgb(173, 231, 146);
			break;
		case eTooltipColor.Blue:
			color = Color.FromArgb(221, 230, 247);
			color2 = Color.FromArgb(138, 168, 228);
			break;
		case eTooltipColor.BlueMist:
			color = Color.FromArgb(227, 236, 243);
			color2 = Color.FromArgb(155, 187, 210);
			break;
		case eTooltipColor.Cyan:
			color = Color.FromArgb(227, 236, 243);
			color2 = Color.FromArgb(155, 187, 210);
			break;
		case eTooltipColor.Green:
			color = Color.FromArgb(234, 240, 226);
			color2 = Color.FromArgb(183, 201, 151);
			break;
		case eTooltipColor.Lemon:
			color = Color.FromArgb(252, 253, 215);
			color2 = Color.FromArgb(245, 249, 111);
			break;
		case eTooltipColor.Magenta:
			color = Color.FromArgb(243, 229, 236);
			color2 = Color.FromArgb(213, 164, 187);
			break;
		case eTooltipColor.Orange:
			color = Color.FromArgb(252, 233, 217);
			color2 = Color.FromArgb(246, 176, 120);
			break;
		case eTooltipColor.Purple:
			color = Color.FromArgb(234, 227, 245);
			color2 = Color.FromArgb(180, 158, 222);
			break;
		case eTooltipColor.PurpleMist:
			color = Color.FromArgb(232, 227, 234);
			color2 = Color.FromArgb(171, 156, 183);
			break;
		case eTooltipColor.Red:
			color = Color.FromArgb(249, 225, 226);
			color2 = Color.FromArgb(238, 149, 151);
			break;
		case eTooltipColor.Silver:
			color = Color.FromArgb(225, 225, 232);
			color2 = Color.FromArgb(149, 149, 170);
			break;
		case eTooltipColor.Tan:
			color = Color.FromArgb(248, 242, 226);
			color2 = Color.FromArgb(232, 209, 153);
			break;
		case eTooltipColor.Teal:
			color = Color.FromArgb(205, 236, 240);
			color2 = Color.FromArgb(78, 188, 202);
			break;
		case eTooltipColor.Yellow:
			color = Color.FromArgb(255, 244, 213);
			color2 = Color.FromArgb(255, 216, 105);
			break;
		case eTooltipColor.Gray:
			color = Color.White;
			color2 = ColorScheme.GetColor("E4E4F0");
			break;
		case eTooltipColor.Office2003:
			TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, Color.White);
			TypeDescriptor.GetProperties(style)["BackColor2SchemePart"].SetValue(style, eColorSchemePart.MenuSide);
			break;
		case eTooltipColor.StiOffice2013:
			color = Color.White;
			TypeDescriptor.GetProperties(style)["BorderColor"].SetValue(style, Color.FromArgb(255, 180, 180, 180));
			break;
		default:
			color = Color.Empty;
			color2 = Color.Empty;
			break;
		}
		if (!color.IsEmpty)
		{
			TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, color);
		}
		if (!color2.IsEmpty)
		{
			TypeDescriptor.GetProperties(style)["BackColor2"].SetValue(style, color2);
		}
		SetRegion();
	}

	public void UpdateWithSuperTooltipInfo(SuperTooltipInfo info)
	{
		if (info.BodyText == null)
		{
			info.BodyText = "";
		}
		if (info.FooterText == null)
		{
			info.FooterText = "";
		}
		m_BodyImage = info.BodyImage;
		((Control)this).Text = info.BodyText;
		m_FooterImage = info.FooterImage;
		FooterText = info.FooterText;
		m_FooterVisible = info.FooterVisible;
		HeaderText = info.HeaderText;
		m_HeaderVisible = info.HeaderVisible;
		PredefinedColor = info.Color;
	}

	internal void UpdateSuperTooltipSize(SuperTooltipInfo info)
	{
		UpdateWithSuperTooltipInfo(info);
		if (info.CustomSize.Width > ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 && info.CustomSize.Height == 0)
		{
			((Control)this).Size = GetFixedWidthSize(info.CustomSize.Width);
		}
		else if (info.CustomSize.IsEmpty || info.CustomSize.Width < ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 || info.CustomSize.Height < ElementStyleLayout.VerticalStyleWhiteSpace(GetStyle()) + 4)
		{
			RecalcSize();
			if (m_MaximumWidth > 0 && ((Control)this).Size.Width > m_MaximumWidth)
			{
				((Control)this).Size = GetFixedWidthSize(m_MaximumWidth);
			}
		}
		else
		{
			((Control)this).Size = info.CustomSize;
		}
	}

	public void ShowTooltip(SuperTooltipInfo info, int x, int y, bool enforceScreenPosition)
	{
		UpdateWithSuperTooltipInfo(info);
		if (info.CustomSize.Width > ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 && info.CustomSize.Height == 0)
		{
			((Control)this).Size = GetFixedWidthSize(info.CustomSize.Width);
		}
		else if (info.CustomSize.IsEmpty || info.CustomSize.Width < ElementStyleLayout.HorizontalStyleWhiteSpace(GetStyle()) + 4 || info.CustomSize.Height < ElementStyleLayout.VerticalStyleWhiteSpace(GetStyle()) + 4)
		{
			RecalcSize();
			if (m_MaximumWidth > 0 && ((Control)this).Size.Width > m_MaximumWidth)
			{
				((Control)this).Size = GetFixedWidthSize(m_MaximumWidth);
			}
		}
		else
		{
			((Control)this).Size = info.CustomSize;
		}
		bool flag = true;
		if (enforceScreenPosition)
		{
			Point mousePosition = Control.MousePosition;
			ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(mousePosition);
			if (screenInformation != null)
			{
				Rectangle bounds = new Rectangle(x, y, ((Control)this).Width, ((Control)this).Height);
				Size size = screenInformation.WorkingArea.Size;
				size.Width -= (int)((float)size.Width * 0.2f);
				if (bounds.Right > screenInformation.WorkingArea.Right)
				{
					bounds.X -= bounds.Right - screenInformation.WorkingArea.Right;
				}
				if (bounds.Bottom > screenInformation.Bounds.Bottom)
				{
					bounds.Y = screenInformation.Bounds.Bottom - bounds.Height;
				}
				if (bounds.Contains(Control.MousePosition.X, Control.MousePosition.Y))
				{
					if (bounds.Height + Control.MousePosition.Y + 1 <= screenInformation.WorkingArea.Height)
					{
						bounds.Y = Control.MousePosition.Y + SystemInformation.CursorSize.Height;
					}
					else
					{
						bounds.Y = Control.MousePosition.Y - bounds.Height - SystemInformation.CursorSize.Height;
					}
				}
				((Control)this).Bounds = bounds;
				flag = false;
			}
		}
		if (!((Control)this).IsHandleCreated)
		{
			((Control)this).CreateControl();
		}
		Point empty = Point.Empty;
		empty = ((!flag) ? ((Control)this).Location : (((Control)this).Location = new Point(x, y)));
		if (NativeFunctions.ShowDropShadow)
		{
			if (m_DropShadow == null)
			{
				m_DropShadow = new PopupShadow(NativeFunctions.AlphaBlendingSupported);
				((Control)m_DropShadow).CreateControl();
			}
			((Control)m_DropShadow).Hide();
		}
		if (NativeFunctions.ShowDropShadow && Environment.OSVersion.Version.Major >= 5 && !NativeFunctions.IsTerminalSession())
		{
			NativeFunctions.AnimateWindow(((Control)this).Handle, 100, 524288);
		}
		else
		{
			NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
		}
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, ((Control)this).Handle, empty.X + 3, empty.Y + 3, ((Control)this).Width, ((Control)this).Height, 80);
			m_DropShadow.UpdateShadow();
		}
	}

	public void UpdateShadow()
	{
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, ((Control)this).Handle, ((Control)this).Left + 3, ((Control)this).Top + 3, ((Control)this).Width, ((Control)this).Height, 80);
			m_DropShadow.UpdateShadow();
		}
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		if (m_DropShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(0), ((Control)this).Left + 3, ((Control)this).Top + 3, 0, 0, 81);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}
}
