using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
public class SuperTooltipControl : PanelControl
{
	private const long च = 2147483648L;

	private const long छ = 67108864L;

	private const long ज = 33554432L;

	private const long ञ = 128L;

	private const long झ = 8L;

	private string ⷊ = "";

	private _0CB4 _2E19;

	private string ⷋ = "";

	private _0CB4 _2E1A;

	private Image ⷌ;

	private Image ⷎ;

	private Size _2E0F = new Size(100, 18);

	private bool ⷈ = true;

	private bool ⷉ = true;

	private bool _2E1B = true;

	private int _2E1C = 8;

	private ഓ _0B84;

	private bool _2E1D;

	private _0CB4 _2E1E;

	private bool _2E1F = true;

	private int ߐ;

	private bool _2E14 = true;

	private eTooltipColor ᕴ;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether complete tooltip is shown including header, body and footer. When set to false only tooltip header is shown.")]
	public bool ShowTooltipDescription
	{
		get
		{
			return _2E14;
		}
		set
		{
			_2E14 = value;
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
			return ߐ;
		}
		set
		{
			ߐ = value;
		}
	}

	[DefaultValue(eTooltipColor.Default)]
	public eTooltipColor PredefinedColor
	{
		get
		{
			return ᕴ;
		}
		set
		{
			ᕴ = value;
			_2E25(value);
			((Control)this).Refresh();
		}
	}

	internal _0CB4 _2E18 => _2E1E;

	public Size MinimumTooltipSize
	{
		get
		{
			return _2E0F;
		}
		set
		{
			_2E0F = value;
		}
	}

	[DefaultValue(null)]
	public Image BodyImage
	{
		get
		{
			return ⷎ;
		}
		set
		{
			ⷎ = value;
		}
	}

	[DefaultValue(null)]
	public Image FooterImage
	{
		get
		{
			return ⷌ;
		}
		set
		{
			ⷌ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	public string HeaderText
	{
		get
		{
			return ⷊ;
		}
		set
		{
			ⷊ = value;
			if (ሒ.መ(ref ⷊ))
			{
				_2E19 = ሒ.ሕ(ⷊ);
				if (_2E19 != null)
				{
					_2E19.ವ += _2E20;
				}
			}
			else
			{
				_2E19 = null;
			}
		}
	}

	[DefaultValue(true)]
	public bool HeaderVisible
	{
		get
		{
			return ⷈ;
		}
		set
		{
			ⷈ = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	public string FooterText
	{
		get
		{
			return ⷋ;
		}
		set
		{
			ⷋ = value;
			if (ሒ.መ(ref ⷋ))
			{
				_2E1A = ሒ.ሕ(ⷋ);
				if (_2E1A != null)
				{
					_2E1A.ವ += _2E20;
				}
			}
			else
			{
				_2E1A = null;
			}
		}
	}

	[DefaultValue(true)]
	public bool FooterVisible
	{
		get
		{
			return ⷉ;
		}
		set
		{
			ⷉ = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	public bool FooterSeparator
	{
		get
		{
			return _2E1B;
		}
		set
		{
			_2E1B = value;
		}
	}

	[DefaultValue(true)]
	public bool MouseActivateEnabled
	{
		get
		{
			return _2E1F;
		}
		set
		{
			_2E1F = value;
		}
	}

	public bool StandardControl
	{
		get
		{
			return _2E1D;
		}
		set
		{
			_2E1D = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			if (!_2E1D)
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
		return _28A4();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string _06EA = ((Control)this).Text;
		if (ሒ.መ(ref _06EA))
		{
			_2E1E = ሒ.ሕ(_06EA);
			if (_2E1E != null)
			{
				_2E1E.ವ += _2E20;
			}
		}
		else
		{
			_2E1E = null;
		}
		base.OnTextChanged(e);
	}

	private void _2E20(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_2E1E != null)
		{
			_2E1E.મ((Control)(object)this, e);
		}
		if (_2E19 != null)
		{
			_2E19.મ((Control)(object)this, e);
		}
		if (_2E1A != null)
		{
			_2E1A.મ((Control)(object)this, e);
		}
		base.OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (_2E1E != null)
		{
			_2E1E.ग़((Control)(object)this, e);
		}
		if (_2E19 != null)
		{
			_2E19.ग़((Control)(object)this, e);
		}
		if (_2E1A != null)
		{
			_2E1A.ग़((Control)(object)this, e);
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_2E1E != null)
		{
			_2E1E.ફ((Control)(object)this, e);
		}
		if (_2E19 != null)
		{
			_2E19.ફ((Control)(object)this, e);
		}
		if (_2E1A != null)
		{
			_2E1A.ફ((Control)(object)this, e);
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_2E1E != null)
		{
			_2E1E.ભ((Control)(object)this);
		}
		if (_2E19 != null)
		{
			_2E19.ભ((Control)(object)this);
		}
		if (_2E1A != null)
		{
			_2E1A.ભ((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	protected override void OnClick(EventArgs e)
	{
		if (_2E1E != null)
		{
			_2E1E.દ((Control)(object)this);
		}
		if (_2E19 != null)
		{
			_2E19.દ((Control)(object)this);
		}
		if (_2E1A != null)
		{
			_2E1A.દ((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && !_2E1F)
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
		clientRectangle.X += ᝈ.ᝐ(Style);
		clientRectangle.Width -= ᝈ.ᝍ(Style);
		clientRectangle.Y += ᝈ._1753(Style);
		clientRectangle.Height -= ᝈ.ᝏ(Style);
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
		Font val = new Font(font, (FontStyle)((_2E14 || ⷊ == "") ? 1 : 0));
		Padding padding = _2E21();
		Padding padding2 = _2E22();
		Padding padding3 = _2E23();
		Padding padding4 = _2E24();
		try
		{
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(style, graphics, Rectangle.Empty);
			if (ⷊ != "" && HeaderVisible)
			{
				Rectangle rectangle = new Rectangle(clientRectangle.X + padding.Left, clientRectangle.Y + padding.Top, clientRectangle.Width - padding.Horizontal, clientRectangle.Height - padding.Vertical);
				Size size = Size.Empty;
				if (_2E19 == null)
				{
					elementStyleDisplayInfo.Bounds = rectangle;
					eTextFormat eTextFormat2 = eTextFormat.WordBreak;
					if ((int)((Control)this).RightToLeft == 1)
					{
						eTextFormat2 |= eTextFormat.RightToLeft;
					}
					ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ⷊ, val, useDefaultFont: true, eTextFormat2);
					size = TextDrawing.MeasureString(graphics, ⷊ, val, rectangle.Width, eTextFormat2);
				}
				else if (rectangle.Width > 0 && rectangle.Height > 0)
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle, hotKeyPrefixVisible: true);
					_2E19._0CC6(rectangle, markupDrawContext);
					_2E19.Render(markupDrawContext);
					size = _2E19.Bounds.Size;
				}
				size.Width += padding.Horizontal;
				size.Height += padding.Vertical;
				clientRectangle.Y += size.Height;
				clientRectangle.Height -= size.Height;
			}
			if (ⷋ != "" && FooterVisible && clientRectangle.Width > 0 && clientRectangle.Height > 0 && (_2E14 || ⷊ == ""))
			{
				Size empty = Size.Empty;
				eTextFormat eTextFormat3 = eTextFormat.WordBreak;
				empty = ((_2E1A != null) ? _2E1A.Bounds.Size : TextDrawing.MeasureString(graphics, ⷋ, val, clientRectangle.Width - padding2.Horizontal, eTextFormat3));
				if (ⷌ != null && ⷌ.Height > empty.Height)
				{
					empty.Height = ⷌ.Height;
				}
				Rectangle rectangle2 = new Rectangle(clientRectangle.X + padding2.Left, clientRectangle.Bottom - empty.Height - padding2.Bottom, clientRectangle.Width - padding2.Horizontal, empty.Height);
				if (FooterSeparator)
				{
					Pen val2 = new Pen(style.BorderColor, 1f);
					try
					{
						graphics.DrawLine(val2, 0, rectangle2.Y - padding2.Top - 1, ((Control)this).ClientRectangle.Right, rectangle2.Y - padding2.Top - 1);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				if (ⷌ != null)
				{
					graphics.DrawImage(ⷌ, new Rectangle(rectangle2.X, rectangle2.Y + (rectangle2.Height - ⷌ.Height) / 2, ⷌ.Width, ⷌ.Height));
					rectangle2.X += ⷌ.Width + _2E1C;
					rectangle2.Width -= ⷌ.Width + _2E1C;
				}
				if (rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					if (_2E1A == null)
					{
						elementStyleDisplayInfo.Bounds = rectangle2;
						eTextFormat3 |= eTextFormat.VerticalCenter;
						if ((int)((Control)this).RightToLeft == 1)
						{
							eTextFormat3 |= eTextFormat.RightToLeft;
						}
						ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ⷋ, val, useDefaultFont: true, eTextFormat3);
					}
					else
					{
						MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle2, hotKeyPrefixVisible: true);
						_2E1A._0CC6(rectangle2, markupDrawContext2);
						_2E1A.Render(markupDrawContext2);
						empty = _2E1A.Bounds.Size;
					}
				}
				empty.Width += padding2.Horizontal;
				empty.Height += padding2.Vertical;
				clientRectangle.Height -= empty.Height;
			}
			if (!_2E14 && !(ⷊ == ""))
			{
				return;
			}
			if (ⷎ != null)
			{
				Rectangle rectangle3 = new Rectangle(clientRectangle.X + padding4.Left, clientRectangle.Y + padding4.Top, ⷎ.Width, ⷎ.Height);
				graphics.DrawImage(ⷎ, rectangle3);
				clientRectangle.X += padding4.Horizontal + ⷎ.Width;
				clientRectangle.Width -= padding4.Horizontal + ⷎ.Width;
			}
			if (!(((Control)this).Text != "") || clientRectangle.Width <= 0 || clientRectangle.Height <= 0)
			{
				return;
			}
			Rectangle rectangle4 = new Rectangle(clientRectangle.X + padding3.Left, clientRectangle.Y + padding3.Top, clientRectangle.Width - padding3.Horizontal, clientRectangle.Height - padding3.Vertical);
			if (_2E1E == null)
			{
				elementStyleDisplayInfo.Bounds = rectangle4;
				if (rectangle4.Width > 0 && rectangle4.Height > 0)
				{
					eTextFormat eTextFormat4 = elementStyleDisplayInfo.Style.ݽ;
					if ((int)((Control)this).RightToLeft == 1)
					{
						eTextFormat4 |= eTextFormat.RightToLeft;
					}
					ElementStyleDisplay.PaintText(elementStyleDisplayInfo, ((Control)this).Text, font, useDefaultFont: false, eTextFormat4);
				}
			}
			else if (rectangle4.Width > 0 && rectangle4.Height > 0)
			{
				MarkupDrawContext markupDrawContext3 = new MarkupDrawContext(graphics, font, elementStyleDisplayInfo.Style.TextColor, (int)((Control)this).RightToLeft == 1, rectangle4, hotKeyPrefixVisible: true);
				_2E1E._0CC6(rectangle4, markupDrawContext3);
				_2E1E.Render(markupDrawContext3);
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	private Padding _2E21()
	{
		return new Padding(6, 6, 2, 2);
	}

	private Padding _2E22()
	{
		return new Padding(6, 6, 4, 6);
	}

	private Padding _2E23()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		if ((ⷊ == "" || !HeaderVisible) && (ⷋ == "" || !FooterVisible))
		{
			return new Padding(1, 1, 1, 1);
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			return new Padding(6, 14, 4, 4);
		}
		return new Padding(14, 6, 4, 4);
	}

	private Padding _2E24()
	{
		return new Padding(6, 6, 6, 6);
	}

	public void RecalcSize()
	{
		((Control)this).Size = _28A4();
	}

	public Size GetFixedWidthSize(int width)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Invalid comparison between Unknown and I4
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Invalid comparison between Unknown and I4
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Invalid comparison between Unknown and I4
		Padding padding = _2E21();
		Padding padding2 = _2E22();
		Padding padding3 = _2E23();
		Padding padding4 = _2E24();
		Size empty = Size.Empty;
		int num = width - ᝈ.ᝍ(Style);
		if (num <= 6)
		{
			num = 16;
		}
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		Font val = new Font(font, (FontStyle)((_2E14 || ⷊ == "") ? 1 : 0));
		Graphics val2 = CreateGraphics();
		if (base.AntiAlias)
		{
			val2.SmoothingMode = (SmoothingMode)4;
			val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		try
		{
			if (ⷊ != "" && HeaderVisible)
			{
				Size empty2 = Size.Empty;
				int num2 = num - padding.Horizontal;
				if (num2 <= 1)
				{
					num2 = 2;
				}
				if (_2E19 == null)
				{
					empty2 = TextDrawing.MeasureString(val2, ⷊ, val, num2, eTextFormat.WordBreak);
				}
				else
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					_2E19.Measure(new Size(num2, 1), markupDrawContext);
					empty2 = _2E19.Bounds.Size;
					_2E19._0CC6(new Rectangle(Point.Empty, empty2), markupDrawContext);
					empty2 = _2E19.Bounds.Size;
				}
				empty2.Height += 2;
				empty2.Height += padding.Vertical;
				empty.Height += empty2.Height;
			}
			if (ⷋ != "" && FooterVisible && (_2E14 || ⷊ == ""))
			{
				Size empty3 = Size.Empty;
				int num3 = num - padding2.Horizontal;
				if (ⷌ != null)
				{
					num3 -= ⷌ.Width + _2E1C;
				}
				if (num3 <= 1)
				{
					num3 = 2;
				}
				if (_2E1A == null)
				{
					empty3 = TextDrawing.MeasureString(val2, ⷋ, val, num3, eTextFormat.WordBreak);
				}
				else
				{
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					_2E1A.Measure(new Size(num3, 1), markupDrawContext2);
					empty3 = _2E1A.Bounds.Size;
					_2E1A._0CC6(new Rectangle(Point.Empty, empty3), markupDrawContext2);
					empty3 = _2E1A.Bounds.Size;
				}
				empty3.Height += 2;
				if (ⷌ != null && ⷌ.Height > empty3.Height)
				{
					empty3.Height = ⷌ.Height;
				}
				empty3.Height += padding2.Vertical;
				empty.Height += empty3.Height;
			}
			if (_2E14 || ⷊ == "")
			{
				if (((Control)this).Text != "")
				{
					int num4 = num - padding3.Horizontal;
					if (ⷎ != null)
					{
						num4 -= ⷎ.Width + padding4.Horizontal;
					}
					if (num4 <= 1)
					{
						num4 = 2;
					}
					Size empty4 = Size.Empty;
					if (_2E1E == null)
					{
						empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num4, Style.ݽ);
					}
					else
					{
						MarkupDrawContext markupDrawContext3 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
						_2E1E.Measure(new Size(num4, 1), markupDrawContext3);
						empty4 = _2E1E.Bounds.Size;
						if ((double)empty4.Height > (double)empty4.Width * 1.75)
						{
							num4 = (int)((double)empty4.Height * 0.75);
						}
						_2E1E._0CC6(new Rectangle(Point.Empty, new Size(num4, 1)), markupDrawContext3);
						empty4 = _2E1E.Bounds.Size;
					}
					empty4.Height += padding3.Vertical;
					if (ⷎ != null && ⷎ.Height + padding4.Vertical > empty4.Height)
					{
						empty4.Height = ⷎ.Height + padding4.Vertical;
					}
					empty.Height += empty4.Height;
				}
				else if (ⷎ != null)
				{
					empty.Height += ⷎ.Height + padding4.Vertical;
				}
			}
		}
		finally
		{
			val2.Dispose();
			val.Dispose();
		}
		empty.Width = width;
		empty.Height += ᝈ.ᝏ(Style);
		return empty;
	}

	private Size _28A4()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Invalid comparison between Unknown and I4
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Invalid comparison between Unknown and I4
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Invalid comparison between Unknown and I4
		Padding padding = Dpi.Size(_2E21());
		Padding padding2 = Dpi.Size(_2E22());
		Padding padding3 = Dpi.Size(_2E23());
		Padding padding4 = Dpi.Size(_2E24());
		Size empty = Size.Empty;
		Font font = ((Control)this).Font;
		if (Style.Font != null)
		{
			font = Style.Font;
		}
		Font val = new Font(font, (FontStyle)((_2E14 || ⷊ == "") ? 1 : 0));
		Graphics val2 = CreateGraphics();
		if (base.AntiAlias)
		{
			val2.SmoothingMode = (SmoothingMode)4;
			val2.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		Size size = Dpi.Size(_2E0F);
		try
		{
			if (ⷊ != "" && HeaderVisible)
			{
				Size empty2 = Size.Empty;
				if (_2E19 == null)
				{
					empty2 = TextDrawing.MeasureString(val2, ⷊ, val);
				}
				else
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					_2E19.Measure(new Size(1024, 1), markupDrawContext);
					empty2 = _2E19.Bounds.Size;
					_2E19._0CC6(new Rectangle(Point.Empty, empty2), markupDrawContext);
					empty2 = _2E19.Bounds.Size;
				}
				empty2.Width += 2;
				empty2.Height += 2;
				empty2.Width += padding.Horizontal;
				empty2.Height += padding.Vertical;
				if (empty2.Width > empty.Width)
				{
					empty.Width = empty2.Width;
				}
				empty.Height += empty2.Height;
			}
			if (ⷋ != "" && FooterVisible && (_2E14 || ⷊ == ""))
			{
				Size empty3 = Size.Empty;
				if (_2E1A == null)
				{
					empty3 = TextDrawing.MeasureString(val2, ⷋ, val);
				}
				else
				{
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
					_2E1A.Measure(new Size(1024, 1), markupDrawContext2);
					empty3 = _2E1A.Bounds.Size;
					_2E1A._0CC6(new Rectangle(Point.Empty, empty3), markupDrawContext2);
					empty3 = _2E1A.Bounds.Size;
				}
				empty3.Width += 2;
				empty3.Height += 2;
				if (ⷌ != null)
				{
					empty3.Width += ⷌ.Width + _2E1C;
					if (ⷌ.Height > empty3.Height)
					{
						empty3.Height = ⷌ.Height;
					}
				}
				empty3.Width += padding2.Horizontal;
				empty3.Height += padding2.Vertical;
				if (empty3.Width > empty.Width)
				{
					empty.Width = empty3.Width;
				}
				empty.Height += empty3.Height;
			}
			if (_2E14 || ⷊ == "")
			{
				if (((Control)this).Text != "")
				{
					int num = empty.Width;
					if (num < size.Width)
					{
						num = size.Width;
					}
					if (ߐ > 0)
					{
						num = 5000;
					}
					Size empty4 = Size.Empty;
					if (_2E1E == null)
					{
						empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num, Style.ݽ);
						if ((double)empty4.Height > (double)empty4.Width * 1.75 && empty4.Width > font.Height * 2)
						{
							num = (int)((double)empty4.Height * 0.75);
							empty4 = TextDrawing.MeasureString(val2, ((Control)this).Text, font, num, Style.ݽ);
						}
					}
					else
					{
						MarkupDrawContext markupDrawContext3 = new MarkupDrawContext(val2, font, Color.Black, (int)((Control)this).RightToLeft == 1);
						_2E1E.Measure(new Size(num, 1), markupDrawContext3);
						empty4 = _2E1E.Bounds.Size;
						if ((double)empty4.Height > (double)empty4.Width * 1.75 && empty4.Width > font.Height * 2)
						{
							num = (int)((double)empty4.Height * 0.75);
						}
						_2E1E._0CC6(new Rectangle(Point.Empty, new Size(num, 1)), markupDrawContext3);
						empty4 = _2E1E.Bounds.Size;
					}
					empty4.Width += padding3.Horizontal;
					empty4.Height += padding3.Vertical;
					if (ⷎ != null)
					{
						empty4.Width += ⷎ.Width + padding4.Horizontal;
						if (ⷎ.Height + padding4.Vertical > empty4.Height)
						{
							empty4.Height = ⷎ.Height + padding4.Vertical;
						}
					}
					if (empty4.Width > empty.Width)
					{
						empty.Width = empty4.Width;
					}
					empty.Height += empty4.Height;
				}
				else if (ⷎ != null)
				{
					if (ⷎ.Width + padding4.Horizontal > empty.Width)
					{
						empty.Width = ⷎ.Width + padding4.Horizontal;
					}
					empty.Height += ⷎ.Height + padding4.Vertical;
				}
			}
		}
		finally
		{
			val2.Dispose();
			val.Dispose();
		}
		empty.Width += ᝈ.ᝍ(Style);
		empty.Height += ᝈ.ᝏ(Style);
		if (empty.Width < size.Width)
		{
			empty.Width = size.Width;
		}
		if (empty.Height < size.Height)
		{
			empty.Height = size.Height;
		}
		return empty;
	}

	private void _2E25(eTooltipColor _0BDE)
	{
		ResetStyle();
		ResetStyleMouseOver();
		ResetStyleMouseDown();
		ElementStyle style = Style;
		Color color = Color.Empty;
		Color color2 = Color.Empty;
		eCornerType eCornerType2 = (StyleManager.IsMetro(StyleManager.Style) ? eCornerType.Square : eCornerType.Rounded);
		int num = ((eCornerType2 == eCornerType.Rounded) ? 2 : 0);
		if (((Component)(object)this).DesignMode)
		{
			TypeDescriptor.GetProperties(style)["WordWrap"].SetValue(style, true);
			TypeDescriptor.GetProperties(style)["TextAlignment"].SetValue(style, eStyleTextAlignment.Near);
			TypeDescriptor.GetProperties(style)["TextLineAlignment"].SetValue(style, eStyleTextAlignment.Near);
			TypeDescriptor.GetProperties(style)["BackColorGradientAngle"].SetValue(style, 90);
			TypeDescriptor.GetProperties(style)["BorderColor"].SetValue(style, Color.DimGray);
			TypeDescriptor.GetProperties(style)["BorderWidth"].SetValue(style, 1);
			TypeDescriptor.GetProperties(style)["TextColor"].SetValue(style, Color.Black);
			TypeDescriptor.GetProperties(style)["Border"].SetValue(style, eStyleBorderType.Solid);
			TypeDescriptor.GetProperties(style)["CornerType"].SetValue(style, eCornerType2);
			TypeDescriptor.GetProperties(style)["CornerDiameter"].SetValue(style, num);
		}
		else
		{
			style.WordWrap = true;
			style.TextAlignment = eStyleTextAlignment.Near;
			style.TextLineAlignment = eStyleTextAlignment.Near;
			style.BackColorGradientAngle = 90;
			style.BorderColor = Color.DimGray;
			style.BorderWidth = 1;
			style.TextColor = Color.Black;
			style.Border = eStyleBorderType.Solid;
			style.CornerType = eCornerType2;
			style.CornerDiameter = num;
		}
		switch (_0BDE)
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
			if (((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, Color.White);
				TypeDescriptor.GetProperties(style)["BackColor2SchemePart"].SetValue(style, eColorSchemePart.MenuSide);
			}
			else
			{
				style.BackColor = Color.White;
				style.BackColor2SchemePart = eColorSchemePart.MenuSide;
			}
			break;
		default:
			color = Color.Empty;
			color2 = Color.Empty;
			break;
		}
		if (!color.IsEmpty)
		{
			if (((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(style)["BackColor"].SetValue(style, color);
			}
			else
			{
				style.BackColor = color;
			}
		}
		if (!color2.IsEmpty)
		{
			if (((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(style)["BackColor2"].SetValue(style, color2);
			}
			else
			{
				style.BackColor2 = color2;
			}
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
		ⷎ = info.BodyImage;
		((Control)this).Text = info.BodyText;
		ⷌ = info.FooterImage;
		FooterText = info.FooterText;
		ⷉ = info.FooterVisible;
		HeaderText = info.HeaderText;
		ⷈ = info.HeaderVisible;
		PredefinedColor = info.Color;
	}

	internal void _2E26(SuperTooltipInfo _108F)
	{
		UpdateWithSuperTooltipInfo(_108F);
		if (_108F.CustomSize.Width > ᝈ.ᝍ(GetStyle()) + 4 && _108F.CustomSize.Height == 0)
		{
			((Control)this).Size = GetFixedWidthSize(_108F.CustomSize.Width);
		}
		else if (_108F.CustomSize.IsEmpty || _108F.CustomSize.Width < ᝈ.ᝍ(GetStyle()) + 4 || _108F.CustomSize.Height < ᝈ.ᝏ(GetStyle()) + 4)
		{
			RecalcSize();
			if (ߐ > 0 && ((Control)this).Size.Width > ߐ)
			{
				((Control)this).Size = GetFixedWidthSize(ߐ);
			}
		}
		else
		{
			((Control)this).Size = _108F.CustomSize;
		}
	}

	public void ShowTooltip(SuperTooltipInfo info, int x, int y, bool enforceScreenPosition)
	{
		UpdateWithSuperTooltipInfo(info);
		if (info.CustomSize.Width > ᝈ.ᝍ(GetStyle()) + 4 && info.CustomSize.Height == 0)
		{
			((Control)this).Size = GetFixedWidthSize(info.CustomSize.Width);
		}
		else if (info.CustomSize.IsEmpty || info.CustomSize.Width < ᝈ.ᝍ(GetStyle()) + 4 || info.CustomSize.Height < ᝈ.ᝏ(GetStyle()) + 4)
		{
			RecalcSize();
			if (ߐ > 0 && ((Control)this).Size.Width > ߐ)
			{
				((Control)this).Size = GetFixedWidthSize(ߐ);
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
			ঊ ঊ2 = BarFunctions.ॹ(mousePosition);
			if (ঊ2 != null)
			{
				Rectangle bounds = new Rectangle(x, y, ((Control)this).Width, ((Control)this).Height);
				Size size = ঊ2.ঌ.Size;
				size.Width -= (int)((float)size.Width * 0.2f);
				if (bounds.Right > ঊ2.ঌ.Right)
				{
					bounds.X -= bounds.Right - ঊ2.ঌ.Right;
				}
				if (bounds.Bottom > ঊ2.ঋ.Bottom)
				{
					bounds.Y = ঊ2.ঋ.Bottom - bounds.Height;
				}
				if (bounds.Contains(Control.MousePosition.X, Control.MousePosition.Y))
				{
					if (bounds.Height + Control.MousePosition.Y + 1 <= ঊ2.ঌ.Height)
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
		if (_089F.ࢣ)
		{
			if (_0B84 == null)
			{
				_0B84 = new ഓ(_089F.ࢤ);
				((Control)_0B84).CreateControl();
			}
			((Control)_0B84).Hide();
		}
		if (_089F.ࢣ && Environment.OSVersion.Version.Major >= 5 && !_089F.ष())
		{
			_089F.AnimateWindow(((Control)this).Handle, 100, 524288);
		}
		else
		{
			_089F.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
		}
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, ((Control)this).Handle, empty.X + 3, empty.Y + 3, ((Control)this).Width, ((Control)this).Height, 80);
			_0B84.ഖ();
		}
	}

	public void UpdateShadow()
	{
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, ((Control)this).Handle, ((Control)this).Left + 3, ((Control)this).Top + 3, ((Control)this).Width, ((Control)this).Height, 80);
			_0B84.ഖ();
		}
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(0), ((Control)this).Left + 3, ((Control)this).Top + 3, 0, 0, 81);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}
}
