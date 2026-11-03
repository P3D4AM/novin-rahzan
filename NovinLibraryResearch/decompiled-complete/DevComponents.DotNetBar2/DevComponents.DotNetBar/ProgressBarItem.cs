using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ProgressBarItem : ImageItem, IPersonalizedMenuItem
{
	private class Ṥ
	{
		public ElementStyle ṥ;

		public Color Ṧ = Color.Empty;

		public Color ṧ = Color.Empty;

		public int Ṩ;

		public Ṥ()
		{
		}

		public Ṥ(ElementStyle style, Color chunkColor, Color chunkColor2, int chunkGradientAngle)
		{
			ṥ = style;
			Ṧ = chunkColor;
			ṧ = chunkColor2;
			Ṩ = chunkGradientAngle;
		}
	}

	private eMenuVisibility _0E7F;

	private bool _0E80;

	private int ᘘ = 100;

	private int ᘗ;

	private int ᘛ;

	private int Ṕ = 1;

	private bool ᢟ;

	private EventHandler ᗲ;

	private int ܘ = 96;

	private int ܗ;

	private ElementStyle _0599;

	private Color ṕ = Color.Empty;

	private Color Ṗ = Color.Empty;

	private float ṗ;

	private eProgressItemType Ṙ;

	private int ṙ = 100;

	private int Ṛ;

	private eProgressBarItemColor ຊ;

	private ElementStyle ṛ;

	private Timer Ṝ;

	[Category("Style")]
	[Description("Gets or sets the background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("BackgroundStyle property is replaced with the BackStyle property")]
	[Browsable(false)]
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

	[Category("Style")]
	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public ElementStyle BackStyle
	{
		get
		{
			return _0599;
		}
		set
		{
			if (value == null)
			{
				throw new InvalidOperationException("Null is not valid value for this property.");
			}
			_0599 = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Browsable(true)]
	public int Maximum
	{
		get
		{
			return ᘘ;
		}
		set
		{
			ᘘ = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return ᘗ;
		}
		set
		{
			ᘗ = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Gets or sets the current position of the progress bar.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int Value
	{
		get
		{
			return ᘛ;
		}
		set
		{
			int ك = ᘛ;
			if (value < ᘗ)
			{
				ᘛ = ᘗ;
			}
			else if (value > ᘘ)
			{
				ᘛ = ᘘ;
			}
			else
			{
				ᘛ = value;
			}
			if (ᗲ != null)
			{
				ᗲ(this, new EventArgs());
			}
			if (Ṡ(ك, ᘛ))
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

	[DefaultValue(1)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the progress bar.")]
	public int Step
	{
		get
		{
			return Ṕ;
		}
		set
		{
			Ṕ = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public bool TextVisible
	{
		get
		{
			return ᢟ;
		}
		set
		{
			ᢟ = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[Description("Indicates the width of the label in pixels.")]
	[DefaultValue(96)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Browsable(true)]
	public int Width
	{
		get
		{
			return ܘ;
		}
		set
		{
			if (ܘ != value)
			{
				ܘ = value;
				NeedRecalcSize = true;
				Refresh();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Indicates height of the label in pixels.")]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Browsable(true)]
	public int Height
	{
		get
		{
			return ܗ;
		}
		set
		{
			if (ܗ != value)
			{
				ܗ = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets the color of the progress chunk.")]
	public Color ChunkColor
	{
		get
		{
			return ṕ;
		}
		set
		{
			ṕ = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Gets or sets the target gradient color of the progress chunk.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public Color ChunkColor2
	{
		get
		{
			return Ṗ;
		}
		set
		{
			Ṗ = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Gets or sets the gradient angle of the progress chunk.")]
	[Category("Appearance")]
	[DefaultValue(0)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public float ChunkGradientAngle
	{
		get
		{
			return ṗ;
		}
		set
		{
			ṗ = value;
			if (!Ṗ.IsEmpty)
			{
				Refresh();
			}
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates item's visiblity when on pop-up menu.")]
	[Browsable(true)]
	public eMenuVisibility MenuVisibility
	{
		get
		{
			return _0E7F;
		}
		set
		{
			if (_0E7F != value)
			{
				_0E7F = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "MenuVisibility");
				}
			}
		}
	}

	[Browsable(false)]
	public bool RecentlyUsed
	{
		get
		{
			return _0E80;
		}
		set
		{
			if (_0E80 != value)
			{
				_0E80 = value;
				if (ShouldSyncProperties)
				{
					BarFunctions.ढ़(this, "RecentlyUsed");
				}
			}
		}
	}

	[DefaultValue(eProgressItemType.Standard)]
	[Description("Indicates type of progress bar used to indicate progress.")]
	[Category("Behavior")]
	[Browsable(true)]
	public eProgressItemType ProgressType
	{
		get
		{
			return Ṙ;
		}
		set
		{
			Ṙ = value;
			ṡ();
		}
	}

	internal int ṓ => Ṛ;

	[Description("Indicates marquee animation speed in milliseconds.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int MarqueeAnimationSpeed
	{
		get
		{
			return ṙ;
		}
		set
		{
			ṙ = value;
			ṡ();
		}
	}

	[DevCoBrowsable(false)]
	[Browsable(true)]
	[DefaultValue(eProgressBarItemColor.Normal)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return ຊ;
		}
		set
		{
			if (ຊ != value)
			{
				ຊ = value;
				Refresh();
			}
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	internal void _0EBA(ProgressBarItem _0EBB)
	{
		CopyToItem(_0EBB);
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

	private Brush ṝ(Rectangle भ, Ṥ Ṟ)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (Ṟ.ṧ.IsEmpty)
		{
			return (Brush)new SolidBrush(Ṟ.Ṧ);
		}
		return (Brush)(object)DisplayHelp.CreateLinearGradientBrush(भ, Ṟ.Ṧ, Ṟ.ṧ, Ṟ.Ṩ);
	}

	public override void Paint(ItemPaintArgs pa)
	{
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		if (SuspendLayout || (base.IsThemed && _0DF4(pa)))
		{
			return;
		}
		Ṥ ṥ = ṟ();
		ṥ.ṥ.SetColorScheme(pa.Colors);
		Graphics graphics = pa.Graphics;
		Font font = GetFont();
		if (BarFunctions.IsOffice2007Style(EffectiveStyle))
		{
			pa.ໝ?.DrawProgressBarItem(new ProgressBarItemRenderEventArgs(pa.Graphics, this, pa.Font, pa.RightToLeft));
		}
		else
		{
			Rectangle rect = m_Rect;
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(ṥ.ṥ, graphics, rect));
			rect.Inflate(-ᝈ.ᝍ(ṥ.ṥ), -ᝈ.ᝏ(ṥ.ṥ));
			switch (EffectiveStyle)
			{
			case eDotNetBarStyle.OfficeXP:
			{
				Rectangle भ = rect;
				int num6 = (int)Math.Floor((float)rect.Height * 0.65f);
				if (num6 <= 0)
				{
					num6 = 2;
				}
				int num7 = (int)Math.Ceiling((double)rect.Width / (double)(num6 + 2));
				Region clip2 = graphics.Clip;
				graphics.SetClip(rect, (CombineMode)1);
				int num8 = (int)((float)num7 * ((float)(ᘛ - ᘗ) / (float)(ᘘ - ᘗ)));
				int num9 = rect.X;
				if (Ṙ == eProgressItemType.Marquee)
				{
					num8 = 5;
					num9 += rect.Width * Ṛ / 100 - (int)((double)(num6 * num8) * 0.65);
				}
				Brush val3 = ṝ(भ, ṥ);
				try
				{
					int num10 = num9;
					for (int j = 0; j < num8; j++)
					{
						graphics.FillRectangle(val3, num10, rect.Y, num6, rect.Height);
						num10 += num6 + 2;
						if (Ṙ == eProgressItemType.Marquee && num10 > rect.Right)
						{
							num10 = rect.X;
						}
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				graphics.Clip = clip2;
				break;
			}
			case eDotNetBarStyle.Office2000:
			{
				Region clip3 = graphics.Clip;
				graphics.SetClip(rect, (CombineMode)1);
				Rectangle भ2 = rect;
				if (Ṙ == eProgressItemType.Marquee)
				{
					rect.Width = (int)((double)rect.Width * 0.29);
					rect.X += भ2.Width * Ṛ / 100 - rect.Width / 2;
				}
				else
				{
					rect.Width = (int)((float)rect.Width * ((float)(ᘛ - ᘗ) / (float)(ᘘ - ᘗ)));
				}
				Brush val4 = ṝ(भ2, ṥ);
				try
				{
					graphics.FillRectangle(val4, rect);
					if (Ṙ == eProgressItemType.Marquee && rect.Right > भ2.Right + 4)
					{
						rect = new Rectangle(भ2.X, भ2.Y, rect.Right - भ2.Right - 4, भ2.Height);
						graphics.FillRectangle(val4, rect);
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
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
				int num3 = (int)((float)num2 * ((float)(ᘛ - ᘗ) / (float)(ᘘ - ᘗ)));
				int num4 = rect.X;
				if (Ṙ == eProgressItemType.Marquee)
				{
					num3 = 5;
					num4 += rect.Width * Ṛ / 100 - (int)((double)(num * num3) * 0.65);
				}
				Brush val = null;
				if (ṥ.ṧ.IsEmpty)
				{
					val = (Brush)new SolidBrush(ṥ.Ṧ);
				}
				else
				{
					LinearGradientBrush val2 = DisplayHelp.CreateLinearGradientBrush(new Rectangle(0, 0, num, rect.Height), ṥ.Ṧ, ṥ.ṧ, ṥ.Ṩ);
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
						if (Ṙ == eProgressItemType.Marquee && num5 > rect.Right)
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
		if (ᢟ)
		{
			ElementStyleDisplay.PaintText(new ElementStyleDisplayInfo(ṥ.ṥ, graphics, m_Rect), m_Text, font);
		}
		if (DesignMode && Focused)
		{
			Rectangle rect2 = m_Rect;
			rect2.Inflate(-1, -1);
			_1A6B._1A6C(graphics, rect2, pa.Colors.ItemDesignTimeBorder);
		}
	}

	private bool _0DF4(ItemPaintArgs _0BD2)
	{
		if (SuspendLayout)
		{
			return true;
		}
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		Graphics graphics = _0BD2.Graphics;
		_0A7F obj = _0BD2._0A7F;
		if (obj == null)
		{
			return false;
		}
		_0FD8 _0FD9 = _0FD8._0FD9;
		Rectangle rect = m_Rect;
		rect.Inflate(-1, -1);
		obj._0EE5(graphics, _0FD9, _0FDD.ཊ, rect);
		_0FD9 = _0FD8._0FDB;
		rect.Width = (int)((float)rect.Width * ((float)(ᘛ - ᘗ) / (float)(ᘘ - ᘗ)));
		if (rect.Width > 5)
		{
			rect.Inflate(-4, -4);
			obj._0EE5(graphics, _0FD9, _0FDD.ཊ, rect);
		}
		if (DesignMode && Focused)
		{
			rect = m_Rect;
			rect.Inflate(-1, -1);
			_1A6B._1A6C(graphics, rect, _0BD2.Colors.ItemDesignTimeBorder);
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
		int num = Dpi.Height(ܗ);
		int num2 = Dpi.Width(ܘ);
		if ((ܘ != 0 && ܗ != 0) || !ᢟ)
		{
			num = ((ܗ != 0) ? Dpi.Height(ܗ) : Dpi.Height(ImageSize.Height));
			num2 = ((ܘ != 0) ? Dpi.Width(ܘ) : Dpi.Width(96));
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
				ElementStyle elementStyle = _1400();
				Font font = GetFont();
				Size empty = Size.Empty;
				eTextFormat ݽ = elementStyle.ݽ;
				if (m_Text != "")
				{
					empty = TextDrawing.MeasureString(val, m_Text, font, 0, ݽ);
					empty.Width += 4;
				}
				else
				{
					empty = TextDrawing.MeasureString(val, " ", font, 0, ݽ);
					empty.Width += 4;
				}
				if (elementStyle.Border != 0)
				{
					empty.Width += 4;
				}
				empty.Height += Dpi.Height6;
				if (ܘ == 0)
				{
					num2 = empty.Width;
				}
				if (ܗ == 0)
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
		if (_0599 != null)
		{
			_0599.StyleChanged -= ઐ;
		}
		_0599 = new ElementStyle();
		_0599.StyleChanged += ઐ;
		Refresh();
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		OnAppearanceChanged();
	}

	private Ṥ ṟ()
	{
		ElementStyle elementStyle = null;
		Color empty = Color.Empty;
		Color empty2 = Color.Empty;
		int chunkGradientAngle = (int)ṗ;
		elementStyle = ((!_0599.Custom) ? _1400() : _0599);
		if (!ṕ.IsEmpty)
		{
			empty = ṕ;
			empty2 = Ṗ;
		}
		else
		{
			switch (EffectiveStyle)
			{
			case eDotNetBarStyle.OfficeXP:
			case eDotNetBarStyle.Office2000:
				empty = SystemColors.Highlight;
				empty2 = Color.Empty;
				break;
			default:
			{
				ColorScheme colorScheme = GetColorScheme();
				chunkGradientAngle = 90;
				empty = colorScheme.ItemPressedBackground;
				empty2 = colorScheme.ItemPressedBackground2;
				break;
			}
			}
		}
		if (Orientation == eOrientation.Vertical)
		{
			elementStyle.ݼ = eOrientation.Vertical;
		}
		else
		{
			elementStyle.ݼ = eOrientation.Horizontal;
		}
		return new Ṥ(elementStyle, empty, empty2, chunkGradientAngle);
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

	private ElementStyle _1400()
	{
		if (ṛ != null)
		{
			return ṛ;
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
			elementStyle.TextColorSchemePart = eColorSchemePart.ItemText;
		}
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		ṛ = elementStyle;
		return ṛ;
	}

	protected override void OnStyleChanged()
	{
		ṛ = null;
		base.OnStyleChanged();
	}

	private bool Ṡ(int ك, int ل)
	{
		int num = (int)((float)WidthInternal * ((float)ك / (float)(ᘘ - ᘗ)));
		int num2 = (int)((float)WidthInternal * ((float)ل / (float)(ᘘ - ᘗ)));
		return num != num2;
	}

	public void PerformStep()
	{
		Value += Ṕ;
	}

	public void Increment(int value)
	{
		Value += value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !ṕ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		ṕ = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !Ṗ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		Ṗ = Color.Empty;
	}

	public override void Deserialize(ItemSerializationContext context)
	{
		base.Deserialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		ܘ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("width"));
		ܗ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("height"));
		ᘛ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("value"));
		ᘗ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("min"));
		ᘘ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("max"));
		ᢟ = XmlConvert.ToBoolean(itemXmlElement.GetAttribute("textvisible"));
		Ṕ = XmlConvert.ToInt32(itemXmlElement.GetAttribute("step"));
		if (itemXmlElement.HasAttribute("chunkcolor"))
		{
			ṕ = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("chunkcolor"));
		}
		if (itemXmlElement.HasAttribute("chunkcolor2"))
		{
			Ṗ = BarFunctions.ColorFromString(itemXmlElement.GetAttribute("chunkcolor2"));
		}
		ṗ = XmlConvert.ToSingle(itemXmlElement.GetAttribute("chunkga"));
		foreach (XmlElement childNode in itemXmlElement.ChildNodes)
		{
			string name;
			if ((name = childNode.Name) != null && name == "backstyle2")
			{
				ElementSerializer.Deserialize(_0599, childNode);
			}
		}
	}

	protected internal override void Serialize(ItemSerializationContext context)
	{
		base.Serialize(context);
		XmlElement itemXmlElement = context.ItemXmlElement;
		itemXmlElement.SetAttribute("width", XmlConvert.ToString(ܘ));
		itemXmlElement.SetAttribute("height", XmlConvert.ToString(ܗ));
		itemXmlElement.SetAttribute("value", XmlConvert.ToString(ᘛ));
		itemXmlElement.SetAttribute("min", XmlConvert.ToString(ᘗ));
		itemXmlElement.SetAttribute("max", XmlConvert.ToString(ᘘ));
		itemXmlElement.SetAttribute("textvisible", XmlConvert.ToString(ᢟ));
		itemXmlElement.SetAttribute("step", XmlConvert.ToString(Ṕ));
		if (!ṕ.IsEmpty)
		{
			itemXmlElement.SetAttribute("chunkcolor", BarFunctions.ColorToString(ṕ));
		}
		if (!Ṗ.IsEmpty)
		{
			itemXmlElement.SetAttribute("chunkcolor2", BarFunctions.ColorToString(Ṗ));
		}
		itemXmlElement.SetAttribute("chunkga", XmlConvert.ToString(ṗ));
		if (_0599.Custom)
		{
			XmlElement xmlElement = itemXmlElement.OwnerDocument.CreateElement("backstyle2");
			itemXmlElement.AppendChild(xmlElement);
			ElementSerializer.Serialize(_0599, xmlElement);
		}
	}

	protected override void OnDesignModeChanged()
	{
		if (DesignMode && Ṙ == eProgressItemType.Marquee)
		{
			ᘡ();
		}
		base.OnDesignModeChanged();
	}

	private void ṡ()
	{
		if (!DesignMode)
		{
			ᘡ();
			if (Ṙ == eProgressItemType.Marquee)
			{
				Ṣ();
			}
			OnAppearanceChanged();
			Refresh();
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		base.OnVisibleChanged(newValue);
		if (Ṙ == eProgressItemType.Marquee && Ṝ != null)
		{
			if (newValue)
			{
				Ṝ.Start();
			}
			else
			{
				Ṝ.Stop();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		ᘡ();
		base.Dispose(disposing);
	}

	private void ᘡ()
	{
		if (Ṝ != null)
		{
			Ṝ.Tick -= ṣ;
			Ṝ.Stop();
			((Component)(object)Ṝ).Dispose();
			Ṝ = null;
		}
	}

	private void Ṣ()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (Ṝ != null)
		{
			ᘡ();
		}
		Ṛ = 0;
		if (ṙ != 0)
		{
			Ṝ = new Timer();
			Ṝ.Interval = ṙ;
			Ṝ.Tick += ṣ;
			if (Visible)
			{
				Ṝ.Start();
			}
		}
	}

	private void ṣ(object _٠, EventArgs ؿ)
	{
		Ṛ += 5;
		if (Ṛ > 100)
		{
			Ṛ = 0;
		}
		Refresh();
	}
}
