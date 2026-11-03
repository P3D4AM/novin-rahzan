using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.GroupPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(GroupPanel), "Controls.GroupPanel.ico")]
public class GroupPanel : PanelControl, INonClientControl
{
	private ᶨ ᵹ;

	private Image _289D;

	private eTitleImagePosition _289E;

	private bool _289F = true;

	private bool _28A0;

	private ePanelColorTable __137C;

	private Rectangle _28A1 = Rectangle.Empty;

	private Color _ᖣ = Color.Empty;

	[Category("Appearance")]
	[Description("Indicates panel color scheme.")]
	[DefaultValue(ePanelColorTable.Default)]
	public ePanelColorTable ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
			SetColorTable(value);
		}
	}

	[DefaultValue(null)]
	[Category("Visual")]
	[Browsable(true)]
	[Description("Indicates image that appears in title with text.")]
	public Image TitleImage
	{
		get
		{
			return _289D;
		}
		set
		{
			_289D = value;
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
			return _289E;
		}
		set
		{
			_289E = value;
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
			return ᵹ.ᶭ;
		}
		set
		{
			ᵹ.ᶭ = value;
		}
	}

	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[Description("")]
	[Category("Appearance")]
	public bool DrawTitleBox
	{
		get
		{
			return _289F;
		}
		set
		{
			if (_289F != value)
			{
				_289F = value;
				InvalidateNonClient();
			}
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether panel automatically provides shadows for child controls.")]
	[DefaultValue(false)]
	public bool IsShadowEnabled
	{
		get
		{
			return _28A0;
		}
		set
		{
			if (_28A0 != value)
			{
				_28A0 = value;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	ElementStyle INonClientControl.ᖕ => Style;

	IntPtr INonClientControl.ᖖ => ((Control)this).Handle;

	int INonClientControl.ᖗ => ((Control)this).Width;

	int INonClientControl.ᖘ => ((Control)this).Height;

	bool INonClientControl.ᖙ => ((Control)this).IsHandleCreated;

	Color INonClientControl.ᖚ => ((Control)this).BackColor;

	[Category("Appearance")]
	[Browsable(false)]
	[Description("Specifies back color when Enabled=false")]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			_ᖣ = value;
			if (!((Control)this).Enabled)
			{
				((Control)this).Invalidate();
			}
		}
	}

	public GroupPanel()
	{
		ᵹ = new ᶨ(this, eScrollBarSkin.Optimized);
		ᵹ.ᶩ += _28A3;
		ᵹ.ᶪ += _28A2;
		((Control)this).SetStyle((ControlStyles)4096, true);
	}

	protected override void OnResize(EventArgs e)
	{
		InvalidateNonClient();
		base.OnResize(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (ᵹ != null)
		{
			ᵹ._1A17();
		}
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.ᆟ(ref _289D);
		}
		base.Dispose(disposing);
	}

	public void InvalidateNonClient()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_089F.RECT lprcUpdate = new _089F.RECT(0, 0, ((Control)this).Width, ((Control)this).Height);
			_089F.RedrawWindow(((Control)this).Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
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
		if (base.ᑥ == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		int x = displayRectangle.X;
		Padding padding = ((Control)this).Padding;
		displayRectangle.X = x - ((Padding)(ref padding)).Left;
		int y = displayRectangle.Y;
		Padding padding2 = ((Control)this).Padding;
		displayRectangle.Y = y - ((Padding)(ref padding2)).Top;
		int width = displayRectangle.Width;
		Padding padding3 = ((Control)this).Padding;
		displayRectangle.Width = width + ((Padding)(ref padding3)).Horizontal;
		int height = displayRectangle.Height;
		Padding padding4 = ((Control)this).Padding;
		displayRectangle.Height = height + ((Padding)(ref padding4)).Vertical;
		displayRectangle.Inflate(2, 2);
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, displayRectangle);
		elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
		ElementStyleDisplay.PaintBackground(elementStyleDisplayInfo, shapeBackground: false);
		if (style.BackgroundImage != null)
		{
			ElementStyleDisplay.PaintBackgroundImage(elementStyleDisplayInfo);
		}
		if (!_28A0)
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
					shadowPaintInfo.Rectangle = new Rectangle(val.Bounds.X, val.Bounds.Y + groupPanel._28A9().Y / 2, val.Bounds.Width, val.Bounds.Height - groupPanel._28A9().Y / 2);
				}
				else
				{
					shadowPaintInfo.Rectangle = val.Bounds;
				}
				ShadowPainter.Paint2(shadowPaintInfo);
			}
		}
	}

	private void _28A2(object _٠, _1DDF ؿ)
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
		Graphics ܓ = ؿ.ܓ;
		TextRenderingHint textRenderingHint = ܓ.TextRenderingHint;
		SmoothingMode smoothingMode = ܓ.SmoothingMode;
		if (base.AntiAlias)
		{
			ܓ.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			ܓ.SmoothingMode = (SmoothingMode)4;
		}
		ܓ.ResetClip();
		ElementStyle elementStyle = Style;
		if (!((Control)this).Enabled)
		{
			elementStyle = elementStyle.Copy();
			elementStyle.TextColor = GetColorScheme().ItemDisabledText;
		}
		if (_289F && !_28A1.IsEmpty)
		{
			DisplayHelp.FillRoundedRectangle(ܓ, _28A1, 2, elementStyle.BackColor, elementStyle.BackColor2, -90);
			DisplayHelp.DrawRoundedRectangle(ܓ, Style.BorderColor, _28A1, 2);
		}
		Rectangle bounds = new Rectangle(ᵹ.ᶮ.X + 4, 1, ((Control)this).ClientRectangle.Width - 8, ᵹ.ᶮ.Y - 1);
		if (_289D != null)
		{
			Size size = _28A4(bounds.Width);
			if ((_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0) || (_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1))
			{
				ܓ.DrawImage(_289D, bounds.X - 1, bounds.Y, _289D.Width, _289D.Height);
				bounds.X += _289D.Width;
				bounds.Width -= _289D.Width;
			}
			else if ((_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0) || (_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1))
			{
				ܓ.DrawImage(_289D, bounds.Right - _289D.Width, bounds.Y, _289D.Width, _289D.Height);
				bounds.Width -= _289D.Width;
			}
			else if (_289E == eTitleImagePosition.Center)
			{
				ܓ.DrawImage(_289D, bounds.X + (bounds.Width - _289D.Width) / 2, bounds.Y, _289D.Width, _289D.Height);
			}
			bounds.Y = bounds.Bottom - size.Height - 2;
		}
		if (base.ᑥ == null)
		{
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(elementStyle, ܓ, bounds);
			elementStyleDisplayInfo.RightToLeft = (int)((Control)this).RightToLeft == 1;
			ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, ((Control)this).Font);
		}
		else
		{
			TextRenderingHint textRenderingHint2 = ܓ.TextRenderingHint;
			if (base.AntiAlias)
			{
				ܓ.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext d = new MarkupDrawContext(ܓ, ((Control)this).Font, elementStyle.TextColor, (int)((Control)this).RightToLeft == 1, Rectangle.Empty, hotKeyPrefixVisible: true);
			Rectangle bounds2 = base.ᑥ.Bounds;
			if (elementStyle.TextAlignment == eStyleTextAlignment.Center)
			{
				base.ᑥ.Bounds = new Rectangle(base.ᑥ.Bounds.X + (bounds.Width - base.ᑥ.Bounds.Width) / 2, base.ᑥ.Bounds.Y, base.ᑥ.Bounds.Width, base.ᑥ.Bounds.Height);
			}
			else if ((elementStyle.TextAlignment == eStyleTextAlignment.Far && (int)((Control)this).RightToLeft == 0) || ((int)((Control)this).RightToLeft == 1 && elementStyle.TextAlignment == eStyleTextAlignment.Near))
			{
				base.ᑥ.Bounds = new Rectangle(bounds.Right - base.ᑥ.Bounds.Width, base.ᑥ.Bounds.Y, base.ᑥ.Bounds.Width, base.ᑥ.Bounds.Height);
			}
			base.ᑥ.Render(d);
			ܓ.TextRenderingHint = textRenderingHint2;
			base.ᑥ.Bounds = bounds2;
		}
		ܓ.TextRenderingHint = textRenderingHint;
		ܓ.SmoothingMode = smoothingMode;
	}

	private void _28A3(object _٠, _1DDF ؿ)
	{
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Invalid comparison between Unknown and I4
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Invalid comparison between Unknown and I4
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		_28A1 = Rectangle.Empty;
		if (((Control)this).Text != null && ((Control)this).Text.Length > 0)
		{
			Size size = _28A4(((Control)this).ClientRectangle.Width - 8);
			Rectangle rectangle = new Rectangle(ᵹ.ᶮ.X + 3, 0, size.Width, ᵹ.ᶮ.Y);
			Size size2 = new Size(ᵹ.ᶮ.Width - 8, ᵹ.ᶮ.Y);
			if (_289D != null && ((_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0)))
			{
				size2.Width -= _289D.Width;
			}
			else if (_289D != null && ((_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1) || (_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0)))
			{
				rectangle.X += _289D.Width;
				size2.Width -= _289D.Width;
			}
			if (Style.TextAlignment == eStyleTextAlignment.Center)
			{
				rectangle.X += (size2.Width - rectangle.Width) / 2;
			}
			else if (Style.TextAlignment == eStyleTextAlignment.Far || ((int)((Control)this).RightToLeft == 1 && Style.TextAlignment == eStyleTextAlignment.Near))
			{
				rectangle.X += size2.Width - rectangle.Width;
			}
			if (!rectangle.IsEmpty)
			{
				rectangle.Inflate(3, 0);
				rectangle.Width += 3;
			}
			ؿ.ܓ.SetClip(rectangle, (CombineMode)4);
			_28A1 = rectangle;
		}
		if (_289D != null)
		{
			Rectangle rectangle2 = new Rectangle(ᵹ.ᶮ.X + 3, 0, _289D.Width, _289D.Height);
			if ((_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0))
			{
				rectangle2.X = ᵹ.ᶮ.Right - rectangle2.Width - 4;
			}
			else if (_289E == eTitleImagePosition.Center)
			{
				rectangle2.X = ᵹ.ᶮ.X;
				rectangle2.X += (ᵹ.ᶮ.Width - rectangle2.Width) / 2;
			}
			ؿ.ܓ.SetClip(rectangle2, (CombineMode)4);
			if (_28A1.IsEmpty)
			{
				_28A1 = rectangle2;
			}
			else
			{
				_28A1 = Rectangle.Union(rectangle2, _28A1);
			}
		}
	}

	private Size _28A4(int _28A5)
	{
		Size result = Size.Empty;
		if (!((Control)this).IsHandleCreated)
		{
			return result;
		}
		if (base.ᑥ != null)
		{
			result = ((_28A5 != 0) ? ᖨ(_28A5) : base.ᑥ.Bounds.Size);
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
				result = ((_28A5 > 0) ? TextDrawing.MeasureString(val, ((Control)this).Text, font, _28A5, format) : TextDrawing.MeasureString(val, ((Control)this).Text, font, 0, format));
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

	private Size ᖨ(int ᖩ)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (base.ᑥ != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, ᖩ, 500);
			rectangle.Inflate(-2, -2);
			Graphics val = CreateGraphics();
			_0CB4 obj = ሒ.ሕ(((Control)this).Text);
			try
			{
				if (base.AntiAlias)
				{
					val.SmoothingMode = (SmoothingMode)4;
					val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
				obj.Measure(rectangle.Size, d);
				result = obj.Bounds.Size;
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
		if (ᵹ != null)
		{
			Rectangle clientTextRectangle = new Rectangle(ᵹ.ᶮ.X, 0, ᵹ.ᶮ.Width, ((Control)this).Height / 2);
			clientTextRectangle.Inflate(-2, 0);
			if (_289D != null)
			{
				if ((_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 0) || (_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 1))
				{
					clientTextRectangle.X += _289D.Width;
					clientTextRectangle.Width -= _289D.Width;
				}
				else if ((_289E == eTitleImagePosition.Left && (int)((Control)this).RightToLeft == 1) || (_289E == eTitleImagePosition.Right && (int)((Control)this).RightToLeft == 0))
				{
					clientTextRectangle.Width -= _289D.Width;
				}
				Size size = ᖨ(clientTextRectangle.Width);
				clientTextRectangle.Y = Math.Max(0, ᵹ.ᶮ.Y - size.Height - 4);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetBoundsCore(x, y, width, height, specified);
		_28A6(width, height, specified);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (((Control)this).IsHandleCreated)
		{
			_28A6(((Control)this).Width, ((Control)this).Height, (BoundsSpecified)12);
			InvalidateNonClient();
		}
		base.OnTextChanged(e);
	}

	private void _28A6(int _९, int _0970, BoundsSpecified _28A7)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		if ((_28A7 & 8) != 8 && (_28A7 & 4) != 4)
		{
			return;
		}
		ElementStyle style = Style;
		Rectangle भ = new Rectangle(0, 0, _९, _0970);
		if (ᵹ != null)
		{
			Rectangle rectangle = ᵹ._1DCD(भ, style);
			if ((_28A7 & 8) == 8)
			{
				_0970 = rectangle.Height;
			}
			if ((_28A7 & 4) == 4)
			{
				_९ = rectangle.Width;
			}
			Type typeFromHandle = typeof(Control);
			try
			{
				typeFromHandle.InvokeMember("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { _९ });
				typeFromHandle.InvokeMember("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { _0970 });
			}
			catch
			{
			}
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (ᵹ == null)
		{
			base.WndProc(ref m);
		}
		else if (((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 522)
		{
			if (ᵹ.WndProc(ref m))
			{
				BaseWndProc(ref m);
			}
			RefreshTextClientRectangle();
		}
		else if (ᵹ.WndProc(ref m))
		{
			BaseWndProc(ref m);
		}
	}

	private void ᖫ(ref Message Ⴘ)
	{
		BaseWndProc(ref Ⴘ);
	}

	void INonClientControl.BaseWndProc(ref Message Ⴘ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖫ
		this.ᖫ(ref Ⴘ);
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		return null;
	}

	private ItemPaintArgs ᖬ(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)(object)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖬ
		return this.ᖬ(_٥);
	}

	private void ᖮ(PaintEventArgs ؿ)
	{
		((ScrollableControl)this).OnPaintBackground(ؿ);
	}

	void INonClientControl.PaintBackground(PaintEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖮ
		this.ᖮ(ؿ);
	}

	private Point ᖱ(Point ᖲ)
	{
		return ((Control)this).PointToScreen(ᖲ);
	}

	Point INonClientControl.PointToScreen(Point ᖲ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖱ
		return this.ᖱ(ᖲ);
	}

	private void ᖶ(Graphics _٥)
	{
	}

	void INonClientControl.RenderNonClient(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖶ
		this.ᖶ(_٥);
	}

	private void ᖴ(ref Rectangle ल)
	{
		Size size = _28A4(ल.Width);
		if (_289D != null)
		{
			size.Height = Math.Max(_289D.Height, size.Height);
		}
		if (size.Height > ल.Height)
		{
			size.Height = ल.Height - 8;
		}
		ल.Y += size.Height;
		ल.Height -= size.Height;
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖴ
		this.ᖴ(ref ल);
	}

	private void ᖵ(ref Rectangle ल)
	{
		if (((Control)this).Text != "")
		{
			int num = _28A8();
			ल.Y += num;
			ल.Height -= num;
		}
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖵ
		this.ᖵ(ref ल);
	}

	private int _28A8()
	{
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		int num = (int)((float)font.Height * 0.7f);
		if (_289D != null)
		{
			num = Math.Max(_289D.Height - (font.Height - num - 1), num);
		}
		return num;
	}

	internal Rectangle _28A9()
	{
		return ᵹ.ᶮ;
	}

	[SpecialName]
	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	[SpecialName]
	void INonClientControl.set_Enabled(bool P_0)
	{
		((Control)this).Enabled = P_0;
	}
}
