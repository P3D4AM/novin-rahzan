using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ProgressBarItem : ImageItem, IPersonalizedMenuItem
{
	private class ColorDescription
	{
		public ElementStyle BackgroundStyle;

		public Color ChunkColor = Color.Empty;

		public Color ChunkColor2 = Color.Empty;

		public int ChunkGradientAngle;

		public ColorDescription()
		{
		}

		public ColorDescription(ElementStyle style, Color chunkColor, Color chunkColor2, int chunkGradientAngle)
		{
			BackgroundStyle = style;
			ChunkColor = chunkColor;
			ChunkColor2 = chunkColor2;
			ChunkGradientAngle = chunkGradientAngle;
		}
	}

	private eMenuVisibility m_MenuVisibility;

	private bool m_RecentlyUsed;

	private int m_Maximum = 100;

	private int m_Minimum;

	private int m_Value;

	private int m_Step = 1;

	private bool m_TextVisible;

	private int m_Width = 96;

	private int m_Height;

	private ElementStyle m_BackgroundStyle;

	private Color m_ChunkColor = Color.Empty;

	private Color m_ChunkColor2 = Color.Empty;

	private float m_ChunkGradientAngle;

	private eProgressItemType m_ProgressType;

	private int m_MarqueeAnimationSpeed = 100;

	private int m_MarqueeValue;

	private eProgressBarItemColor m_ColorTable;

	private ElementStyle m_CachedStyle;

	private Timer m_MarqueeTimer;

	[Obsolete("BackgroundStyle property is replaced with the BackStyle property")]
	[Browsable(false)]
	[Category("Style")]
	[Description("Gets or sets the background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ItemStyle BackgroundStyle
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle BackStyle
	{
		get
		{
			return m_BackgroundStyle;
		}
		set
		{
			if (value == null)
			{
				throw new InvalidOperationException("Null is not valid value for this property.");
			}
			m_BackgroundStyle = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return m_Maximum;
		}
		set
		{
			m_Maximum = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return m_Minimum;
		}
		set
		{
			m_Minimum = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the current position of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			int value2 = m_Value;
			if (value < m_Minimum)
			{
				m_Value = m_Minimum;
			}
			else if (value > m_Maximum)
			{
				m_Value = m_Maximum;
			}
			else
			{
				m_Value = value;
			}
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
			if (GetNeedVisualUpdate(value2, m_Value))
			{
				Refresh();
				OnAppearanceChanged();
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val != null)
				{
					val.Update();
				}
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(1)]
	public int Step
	{
		get
		{
			return m_Step;
		}
		set
		{
			m_Step = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsRenderMode { get; set; }

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool TextVisible
	{
		get
		{
			return m_TextVisible;
		}
		set
		{
			m_TextVisible = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(96)]
	[Category("Layout")]
	[Description("Indicates the width of the label in pixels.")]
	public int Width
	{
		get
		{
			return m_Width;
		}
		set
		{
			if (m_Width != value)
			{
				m_Width = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates height of the label in pixels.")]
	public int Height
	{
		get
		{
			return m_Height;
		}
		set
		{
			if (m_Height != value)
			{
				m_Height = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the color of the progress chunk.")]
	public Color ChunkColor
	{
		get
		{
			return m_ChunkColor;
		}
		set
		{
			m_ChunkColor = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the target gradient color of the progress chunk.")]
	public Color ChunkColor2
	{
		get
		{
			return m_ChunkColor2;
		}
		set
		{
			m_ChunkColor2 = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the gradient angle of the progress chunk.")]
	[DefaultValue(0)]
	public float ChunkGradientAngle
	{
		get
		{
			return m_ChunkGradientAngle;
		}
		set
		{
			m_ChunkGradientAngle = value;
			if (!m_ChunkColor2.IsEmpty)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return m_MenuVisibility;
		}
		set
		{
			if (m_MenuVisibility != value)
			{
				m_MenuVisibility = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	public bool RecentlyUsed
	{
		get
		{
			return m_RecentlyUsed;
		}
		set
		{
			if (m_RecentlyUsed != value)
			{
				m_RecentlyUsed = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.SyncProperty(this, "RecentlyUsed");
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(eProgressItemType.Standard)]
	[Description("Indicates type of progress bar used to indicate progress.")]
	public eProgressItemType ProgressType
	{
		get
		{
			return m_ProgressType;
		}
		set
		{
			m_ProgressType = value;
			OnProgressTypeChanged();
		}
	}

	internal int MarqueeValue => m_MarqueeValue;

	[Browsable(true)]
	[DefaultValue(100)]
	[Category("Behavior")]
	[Description("Indicates marquee animation speed in milliseconds.")]
	public int MarqueeAnimationSpeed
	{
		get
		{
			return m_MarqueeAnimationSpeed;
		}
		set
		{
			m_MarqueeAnimationSpeed = value;
			OnProgressTypeChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eProgressBarItemColor.Normal)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			if (m_ColorTable != value)
			{
				m_ColorTable = value;
				Refresh();
			}
		}
	}

	public event EventHandler ValueChanged;

	public ProgressBarItem()
		: this("", "")
	{
	}

	public ProgressBarItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public ProgressBarItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		ResetBackgroundStyle();
	}

	public override BaseItem Copy()
	{
		ProgressBarItem progressBarItem = new ProgressBarItem(m_Name);
		CopyToItem(progressBarItem);
		return progressBarItem;
	}

	internal void InternalCopyToItem(ProgressBarItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		ProgressBarItem progressBarItem = copy as ProgressBarItem;
		base.CopyToItem(progressBarItem);
		progressBarItem.Minimum = Minimum;
		progressBarItem.Maximum = Maximum;
		progressBarItem.Step = Step;
		progressBarItem.TextVisible = TextVisible;
		progressBarItem.Width = Width;
		progressBarItem.Style = Style;
		progressBarItem.Value = Value;
		progressBarItem.ChunkColor = ChunkColor;
		progressBarItem.ChunkColor2 = ChunkColor2;
		progressBarItem.ChunkGradientAngle = ChunkGradientAngle;
		progressBarItem.ColorTable = ColorTable;
	}

	private Brush GetChunkBrush(Rectangle rect, ColorDescription cd)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (cd.ChunkColor2.IsEmpty)
		{
			return (Brush)new SolidBrush(cd.ChunkColor);
		}
		return (Brush)(object)DisplayHelp.CreateLinearGradientBrush(rect, cd.ChunkColor, cd.ChunkColor2, cd.ChunkGradientAngle);
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		if (SuspendLayout || (base.IsThemed && PaintThemed(pa)))
		{
			return;
		}
		ColorDescription colorDescription = GetColorDescription();
		colorDescription.BackgroundStyle.SetColorScheme(pa.Colors);
		Graphics graphics = pa.Graphics;
		Font font = GetFont();
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			pa.Renderer?.DrawProgressBarItem(new ProgressBarItemRenderEventArgs(pa.Graphics, this, pa.Font, pa.RightToLeft));
		}
		else
		{
			Rectangle rect = m_Rect;
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(colorDescription.BackgroundStyle, graphics, rect));
			rect.Inflate(-ElementStyleLayout.HorizontalStyleWhiteSpace(colorDescription.BackgroundStyle), -ElementStyleLayout.VerticalStyleWhiteSpace(colorDescription.BackgroundStyle));
			switch (EffectiveStyle)
			{
			case eDotNetBarStyle.OfficeXP:
			{
				Rectangle rect2 = rect;
				int num6 = (int)Math.Floor((float)rect.Height * 0.65f);
				if (num6 <= 0)
				{
					num6 = 2;
				}
				int num7 = (int)Math.Ceiling((double)rect.Width / (double)(num6 + 2));
				Region clip2 = graphics.Clip;
				graphics.SetClip(rect, (CombineMode)1);
				int num8 = (int)((float)num7 * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));
				int num9 = rect.X;
				if (m_ProgressType == eProgressItemType.Marquee)
				{
					num8 = 5;
					num9 += rect.Width * m_MarqueeValue / 100 - (int)((double)(num6 * num8) * 0.65);
				}
				Brush chunkBrush = GetChunkBrush(rect2, colorDescription);
				try
				{
					int num10 = num9;
					for (int j = 0; j < num8; j++)
					{
						graphics.FillRectangle(chunkBrush, num10, rect.Y, num6, rect.Height);
						num10 += num6 + 2;
						if (m_ProgressType == eProgressItemType.Marquee && num10 > rect.Right)
						{
							num10 = rect.X;
						}
					}
				}
				finally
				{
					((IDisposable)chunkBrush)?.Dispose();
				}
				graphics.Clip = clip2;
				break;
			}
			case eDotNetBarStyle.Office2000:
			{
				Region clip3 = graphics.Clip;
				graphics.SetClip(rect, (CombineMode)1);
				Rectangle rect3 = rect;
				if (m_ProgressType == eProgressItemType.Marquee)
				{
					rect.Width = (int)((double)rect.Width * 0.29);
					rect.X += rect3.Width * m_MarqueeValue / 100 - rect.Width / 2;
				}
				else
				{
					rect.Width = (int)((float)rect.Width * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));
				}
				Brush chunkBrush2 = GetChunkBrush(rect3, colorDescription);
				try
				{
					graphics.FillRectangle(chunkBrush2, rect);
					if (m_ProgressType == eProgressItemType.Marquee && rect.Right > rect3.Right + 4)
					{
						rect = new Rectangle(rect3.X, rect3.Y, rect.Right - rect3.Right - 4, rect3.Height);
						graphics.FillRectangle(chunkBrush2, rect);
					}
				}
				finally
				{
					((IDisposable)chunkBrush2)?.Dispose();
				}
				graphics.Clip = clip3;
				break;
			}
			case eDotNetBarStyle.Office2003:
			case eDotNetBarStyle.VS2005:
			{
				int num = (int)Math.Floor((float)rect.Height * 0.65f);
				if (num <= 0)
				{
					num = 2;
				}
				int num2 = (int)Math.Ceiling((double)rect.Width / (double)(num + 2));
				Region clip = graphics.Clip;
				graphics.SetClip(rect, (CombineMode)1);
				int num3 = (int)((float)num2 * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));
				int num4 = rect.X;
				if (m_ProgressType == eProgressItemType.Marquee)
				{
					num3 = 5;
					num4 += rect.Width * m_MarqueeValue / 100 - (int)((double)(num * num3) * 0.65);
				}
				Brush val = null;
				if (colorDescription.ChunkColor2.IsEmpty)
				{
					val = (Brush)new SolidBrush(colorDescription.ChunkColor);
				}
				else
				{
					LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(new Rectangle(0, 0, num, rect.Height), colorDescription.ChunkColor, colorDescription.ChunkColor2, colorDescription.ChunkGradientAngle);
					val2.SetBlendTriangularShape(0.1f);
					val = (Brush)(object)val2;
				}
				try
				{
					int num5 = num4;
					for (int i = 0; i < num3; i++)
					{
						graphics.FillRectangle(val, num5, rect.Y, num, rect.Height);
						num5 += num + 2;
						if (m_ProgressType == eProgressItemType.Marquee && num5 > rect.Right)
						{
							num5 = rect.X;
						}
					}
				}
				finally
				{
					val.Dispose();
				}
				graphics.Clip = clip;
				break;
			}
			}
		}
		if (m_TextVisible)
		{
			ElementStyleDisplay.PaintText(new ElementStyleDisplayInfo(colorDescription.BackgroundStyle, graphics, m_Rect), m_Text, font);
		}
		if (DesignMode && Focused)
		{
			Rectangle rect4 = m_Rect;
			rect4.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, rect4, pa.Colors.ItemDesignTimeBorder);
		}
	}

	private bool PaintThemed(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return true;
		}
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		Graphics graphics = pa.Graphics;
		ThemeProgress themeProgress = pa.ThemeProgress;
		if (themeProgress == null)
		{
			return false;
		}
		ThemeProgressParts bar = ThemeProgressParts.Bar;
		Rectangle rect = m_Rect;
		rect.Inflate(-1, -1);
		themeProgress.DrawBackground(graphics, bar, ThemeProgressStates.Normal, rect);
		bar = ThemeProgressParts.Chunk;
		rect.Width = (int)((float)rect.Width * ((float)(m_Value - m_Minimum) / (float)(m_Maximum - m_Minimum)));
		if (rect.Width > 5)
		{
			rect.Inflate(-4, -4);
			themeProgress.DrawBackground(graphics, bar, ThemeProgressStates.Normal, rect);
		}
		if (DesignMode && Focused)
		{
			rect = m_Rect;
			rect.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, rect, pa.Colors.ItemDesignTimeBorder);
		}
		return true;
	}

	protected virtual Font GetFont()
	{
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			return val.Font;
		}
		return SystemFonts.DefaultFont;
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		int num = m_Height;
		int num2 = m_Width;
		if ((m_Width != 0 && m_Height != 0) || !m_TextVisible)
		{
			num = ((m_Height != 0) ? m_Height : ImageSize.Height);
			num2 = ((m_Width != 0) ? m_Width : StiScale2.XXI(96.0));
		}
		else
		{
			object containerControl = ContainerControl;
			Control objCtrl = (Control)((containerControl is Control) ? containerControl : null);
			if (!BaseItem.IsHandleValid(objCtrl))
			{
				return;
			}
			Graphics val = BarFunctions.CreateGraphics(objCtrl);
			try
			{
				ElementStyle backgroundStyle = GetBackgroundStyle();
				Font font = GetFont();
				Size empty = Size.Empty;
				eTextFormat textFormat = backgroundStyle.TextFormat;
				if (m_Text != "")
				{
					empty = TextDrawing.MeasureString(val, m_Text, font, 0, textFormat);
					empty.Width += StiScale2.I4;
				}
				else
				{
					empty = TextDrawing.MeasureString(val, " ", font, 0, textFormat);
					empty.Width += StiScale2.I4;
				}
				if (backgroundStyle.Border != 0)
				{
					empty.Width += StiScale2.I4;
				}
				empty.Height += StiScale2.I6;
				if (m_Width == 0)
				{
					num2 = empty.Width;
				}
				else if (IsRenderMode && m_Width < empty.Width)
				{
					num2 = empty.Width;
				}
				if (m_Height == 0)
				{
					num = empty.Height;
				}
			}
			finally
			{
				val.TextRenderingHint = (TextRenderingHint)0;
				val.SmoothingMode = (SmoothingMode)0;
				val.Dispose();
			}
		}
		if (Orientation == eOrientation.Horizontal)
		{
			HeightInternal = num;
			WidthInternal = num2;
		}
		else
		{
			HeightInternal = num2;
			WidthInternal = num;
		}
		base.RecalcSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		if (m_BackgroundStyle != null)
		{
			m_BackgroundStyle.StyleChanged -= VisualPropertyChanged;
		}
		m_BackgroundStyle = new ElementStyle();
		m_BackgroundStyle.StyleChanged += VisualPropertyChanged;
		Refresh();
	}

	private void VisualPropertyChanged(object sender, EventArgs e)
	{
		OnAppearanceChanged();
	}

	private ColorDescription GetColorDescription()
	{
		ElementStyle elementStyle = null;
		Color empty = Color.Empty;
		Color empty2 = Color.Empty;
		int chunkGradientAngle = (int)m_ChunkGradientAngle;
		elementStyle = ((!m_BackgroundStyle.Custom) ? GetBackgroundStyle() : m_BackgroundStyle);
		if (!m_ChunkColor.IsEmpty)
		{
			empty = m_ChunkColor;
			empty2 = m_ChunkColor2;
		}
		else
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if ((uint)effectiveStyle <= 1u)
			{
				empty = SystemColors.Highlight;
				empty2 = Color.Empty;
			}
			else
			{
				ColorScheme colorScheme = GetColorScheme();
				chunkGradientAngle = 90;
				empty = colorScheme.ItemPressedBackground;
				empty2 = colorScheme.ItemPressedBackground2;
			}
		}
		if (Orientation == eOrientation.Vertical)
		{
			elementStyle.TextOrientation = eOrientation.Vertical;
		}
		else
		{
			elementStyle.TextOrientation = eOrientation.Horizontal;
		}
		return new ColorDescription(elementStyle, empty, empty2, chunkGradientAngle);
	}

	private ColorScheme GetColorScheme()
	{
		ColorScheme colorScheme = null;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is Bar)
		{
			colorScheme = ((Bar)(object)val).ColorScheme;
		}
		else if (val is MenuPanel)
		{
			colorScheme = ((MenuPanel)(object)val).ColorScheme;
		}
		else if (val is ItemControl)
		{
			colorScheme = ((ItemControl)(object)val).ColorScheme;
		}
		else if (val is BaseItemControl)
		{
			colorScheme = ((BaseItemControl)(object)val).ColorScheme;
		}
		if (colorScheme == null)
		{
			colorScheme = new ColorScheme(EffectiveStyle);
		}
		return colorScheme;
	}

	public ElementStyle GetBackgroundStyle()
	{
		if (m_CachedStyle != null)
		{
			return m_CachedStyle;
		}
		ElementStyle elementStyle = new ElementStyle();
		if (EffectiveStyle == eDotNetBarStyle.Office2000 || EffectiveStyle == eDotNetBarStyle.OfficeXP)
		{
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderWidth = 1;
			elementStyle.BorderColor = SystemColors.ControlDark;
			elementStyle.BackColor = SystemColors.Control;
			elementStyle.TextColor = SystemColors.ControlText;
		}
		else if (EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			ColorScheme colorScheme = null;
			if (ContainerControl is Bar)
			{
				colorScheme = ((Bar)ContainerControl).ColorScheme;
			}
			else if (ContainerControl is MenuPanel)
			{
				colorScheme = ((MenuPanel)ContainerControl).ColorScheme;
			}
			if (colorScheme == null)
			{
				colorScheme = new ColorScheme(EffectiveStyle);
			}
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderWidth = 1;
			elementStyle.BorderColorSchemePart = eColorSchemePart.BarDockedBorder;
			elementStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
			elementStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
			elementStyle.BackColorGradientAngle = 90;
			elementStyle.TextColorSchemePart = eColorSchemePart.ItemPressedText;
		}
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		m_CachedStyle = elementStyle;
		return m_CachedStyle;
	}

	protected override void OnStyleChanged()
	{
		m_CachedStyle = null;
		base.OnStyleChanged();
	}

	private bool GetNeedVisualUpdate(int oldValue, int newValue)
	{
		int num = (int)((float)WidthInternal * ((float)oldValue / (float)(m_Maximum - m_Minimum)));
		int num2 = (int)((float)WidthInternal * ((float)newValue / (float)(m_Maximum - m_Minimum)));
		return num != num2;
	}

	public void PerformStep()
	{
		Value += m_Step;
	}

	public void Increment(int value)
	{
		Value += value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !m_ChunkColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		m_ChunkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !m_ChunkColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		m_ChunkColor2 = Color.Empty;
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		m_Width = XmlConvert.ToInt32(itemXmlElement.GetAttribute("width"));
		m_Height = XmlConvert.ToInt32(itemXmlElement.GetAttribute("height"));
		m_Value = XmlConvert.ToInt32(itemXmlElement.GetAttribute("value"));
		m_Minimum = XmlConvert.ToInt32(itemXmlElement.GetAttribute("min"));
		m_Maximum = XmlConvert.ToInt32(itemXmlElement.GetAttribute("max"));
		m_TextVisible = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("textvisible"));
		m_Step = XmlConvert.ToInt32(itemXmlElement.GetAttribute("step"));
		if (itemXmlElement.HasAttribute("chunkcolor"))
		{
			m_ChunkColor = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("chunkcolor"));
		}
		if (itemXmlElement.HasAttribute("chunkcolor2"))
		{
			m_ChunkColor2 = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("chunkcolor2"));
		}
		m_ChunkGradientAngle = XmlConvert.ToSingle(itemXmlElement.GetAttribute("chunkga"));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			string name = childNode.Name;
			if (name == "backstyle2")
			{
				ElementSerializer.Deserialize(m_BackgroundStyle, childNode);
			}
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("width", XmlConvert.ToString(m_Width));
		itemXmlElement.SetAttribute("height", XmlConvert.ToString(m_Height));
		itemXmlElement.SetAttribute("value", XmlConvert.ToString(m_Value));
		itemXmlElement.SetAttribute("min", XmlConvert.ToString(m_Minimum));
		itemXmlElement.SetAttribute("max", XmlConvert.ToString(m_Maximum));
		itemXmlElement.SetAttribute("textvisible", XmlConvert.ToString(m_TextVisible));
		itemXmlElement.SetAttribute("step", XmlConvert.ToString(m_Step));
		if (!m_ChunkColor.IsEmpty)
		{
			itemXmlElement.SetAttribute("chunkcolor", BarFunctions.ColorToString(m_ChunkColor));
		}
		if (!m_ChunkColor2.IsEmpty)
		{
			itemXmlElement.SetAttribute("chunkcolor2", BarFunctions.ColorToString(m_ChunkColor2));
		}
		itemXmlElement.SetAttribute("chunkga", XmlConvert.ToString(m_ChunkGradientAngle));
		if (m_BackgroundStyle.Custom)
		{
			XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("backstyle2");
			itemXmlElement.AppendChild(xmlElement);
			ElementSerializer.Serialize(m_BackgroundStyle, xmlElement);
		}
	}

	protected override void OnDesignModeChanged()
	{
		if (DesignMode && m_ProgressType == eProgressItemType.Marquee)
		{
			DisposeTimer();
		}
		base.OnDesignModeChanged();
	}

	private void OnProgressTypeChanged()
	{
		if (!DesignMode)
		{
			DisposeTimer();
			if (m_ProgressType == eProgressItemType.Marquee)
			{
				SetupTimer();
			}
			OnAppearanceChanged();
			Refresh();
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		base.OnVisibleChanged(newValue);
		if (m_ProgressType == eProgressItemType.Marquee && m_MarqueeTimer != null)
		{
			if (newValue)
			{
				m_MarqueeTimer.Start();
			}
			else
			{
				m_MarqueeTimer.Stop();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		DisposeTimer();
		base.Dispose(disposing);
	}

	private void DisposeTimer()
	{
		if (m_MarqueeTimer != null)
		{
			m_MarqueeTimer.Stop();
			((Component)(object)m_MarqueeTimer).Dispose();
			m_MarqueeTimer = null;
		}
	}

	private void SetupTimer()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (m_MarqueeTimer != null)
		{
			DisposeTimer();
		}
		m_MarqueeValue = 0;
		if (m_MarqueeAnimationSpeed != 0)
		{
			m_MarqueeTimer = new Timer();
			m_MarqueeTimer.Interval = m_MarqueeAnimationSpeed;
			m_MarqueeTimer.Tick += MarqueeTimer_Tick;
			if (Visible)
			{
				m_MarqueeTimer.Start();
			}
		}
	}

	private void MarqueeTimer_Tick(object sender, EventArgs e)
	{
		m_MarqueeValue += 5;
		if (m_MarqueeValue > 100)
		{
			m_MarqueeValue = 0;
		}
		Refresh();
	}
}
