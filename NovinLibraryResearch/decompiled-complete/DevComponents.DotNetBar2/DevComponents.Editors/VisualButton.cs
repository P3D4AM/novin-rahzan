using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.Editors;

public class VisualButton : VisualButtonBase
{
	private int _ᶗ;

	private bool _ᆿ;

	private bool __171A;

	private bool _ᚆ;

	private string _ኚ = "";

	private Image _ᶘ;

	private Image _ነ;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private Color __0EA4 = Color.Empty;

	public int Height
	{
		get
		{
			return _ᶗ;
		}
		set
		{
			_ᶗ = value;
		}
	}

	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		internal set
		{
			if (_ᆿ != value)
			{
				_ᆿ = value;
				InvalidateRender();
			}
		}
	}

	public bool IsMouseDown
	{
		get
		{
			return __171A;
		}
		internal set
		{
			if (__171A != value)
			{
				__171A = value;
				InvalidateRender();
			}
		}
	}

	public bool Checked
	{
		get
		{
			return _ᚆ;
		}
		set
		{
			if (_ᚆ != value)
			{
				_ᚆ = value;
				InvalidateRender();
			}
		}
	}

	[DefaultValue("")]
	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_ኚ != value)
			{
				_ኚ = value;
				InvalidateArrange();
			}
		}
	}

	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (_ነ != value)
			{
				_ነ = value;
				if (_ᶘ != null)
				{
					_ᶘ.Dispose();
					_ᶘ = null;
				}
				InvalidateArrange();
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eSymbolSet.Awesome)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the Symbol.")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			InvalidateRender();
		}
	}

	public override void PerformLayout(PaintInfo pi)
	{
		Size size = new Size(0, (_ᶗ > 0) ? Dpi.Height(_ᶗ) : pi.AvailableSize.Height);
		Graphics graphics = pi.Graphics;
		size.Width += Dpi.Width6;
		if (_ኚ.Length > 0)
		{
			Size size2 = TextDrawing.MeasureString(graphics, _ኚ, pi.DefaultFont);
			size.Width += size2.Width;
			if (_ነ != null)
			{
				size.Width += Dpi.Width4;
			}
		}
		if (!string.IsNullOrEmpty(__0EA6))
		{
			size.Width += size.Height;
		}
		else if (_ነ != null)
		{
			size.Width += Dpi.Width(_ነ.Width);
		}
		if (_ኚ.Length == 0 && _ነ == null && string.IsNullOrEmpty(__0EA6))
		{
			size.Width += Dpi.Width9;
		}
		base.Size = size;
		base.CalculatedSize = size;
		base.PerformLayout(pi);
	}

	protected override void OnPaint(PaintInfo p)
	{
		Graphics graphics = p.Graphics;
		Rectangle renderBounds = base.RenderBounds;
		PaintButtonBackground(p);
		Rectangle rectangle = renderBounds;
		rectangle.Inflate(-3, -3);
		if (!string.IsNullOrEmpty(_ኚ))
		{
			TextDrawing.DrawString(graphics, _ኚ, p.DefaultFont, GetIsEnabled(p) ? p.ForeColor : p.DisabledForeColor, rectangle, eTextFormat.VerticalCenter);
		}
		if (!string.IsNullOrEmpty(__0EA6))
		{
			Office2007ButtonItemStateColorTable office2007StateColorTable = GetOffice2007StateColorTable(p);
			Color color = (GetIsEnabled(p) ? p.ForeColor : p.DisabledForeColor);
			if (!__0EA4.IsEmpty)
			{
				color = __0EA4;
			}
			else if (!office2007StateColorTable.Text.IsEmpty)
			{
				color = office2007StateColorTable.Text;
			}
			float fontSize = Math.Max(1f, (float)(Math.Min(renderBounds.Width, renderBounds.Height) * 72) / graphics.DpiX - 1.5f);
			Rectangle bounds = rectangle;
			if (!string.IsNullOrEmpty(_ኚ))
			{
				bounds = new Rectangle(rectangle.Right - rectangle.Height, rectangle.Y, rectangle.Height, rectangle.Height);
			}
			bounds.Inflate(3, 3);
			TextDrawing.DrawStringLegacy(graphics, __0EA6, Symbols.GetFont(fontSize, _ວ), color, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		else if (_ነ != null)
		{
			Image val = (GetIsEnabled(p) ? _ነ : ᶚ());
			Size size = Dpi.ImageSize(val.Size);
			graphics.DrawImage(val, new Rectangle(rectangle.Right - (size.Width + (string.IsNullOrEmpty(_ኚ) ? ((rectangle.Width - size.Width) / 2) : 0)), rectangle.Y + (rectangle.Height - size.Height) / 2, size.Width, size.Height));
		}
		base.OnPaint(p);
	}

	protected virtual void PaintButtonBackground(PaintInfo p)
	{
		PaintButtonBackground(p, GetOffice2007StateColorTable(p));
	}

	protected virtual void PaintButtonBackground(PaintInfo p, Office2007ButtonItemStateColorTable ct)
	{
		Graphics graphics = p.Graphics;
		Rectangle renderBounds = base.RenderBounds;
		if (ᶙ(p))
		{
			ᯊ.ᯓ(graphics, ct, renderBounds, RoundRectangleShapeDescriptor.RectangleShape);
		}
	}

	private bool ᶙ(PaintInfo _065C)
	{
		if (base.RenderDefaultBackground)
		{
			return true;
		}
		if ((!_065C.MouseOver && !IsMouseDown && !IsMouseOver && !Checked) || !GetIsEnabled())
		{
			return false;
		}
		return true;
	}

	protected Office2007ButtonItemStateColorTable GetOffice2007StateColorTable(PaintInfo p)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground)];
			if (!GetIsEnabled(p))
			{
				return office2007ButtonItemColorTable.Disabled;
			}
			if (IsMouseDown)
			{
				return office2007ButtonItemColorTable.Pressed;
			}
			if (IsMouseOver)
			{
				return office2007ButtonItemColorTable.MouseOver;
			}
			if (Checked)
			{
				return office2007ButtonItemColorTable.Checked;
			}
			return office2007ButtonItemColorTable.Default;
		}
		return null;
	}

	protected override void OnMouseEnter()
	{
		if (GetIsEnabled())
		{
			IsMouseOver = true;
		}
		base.OnMouseEnter();
	}

	protected override void OnMouseLeave()
	{
		IsMouseOver = false;
		base.OnMouseLeave();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = true;
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = false;
		}
		base.OnMouseUp(e);
	}

	private Image ᶚ()
	{
		if (_ᶘ == null && _ነ != null)
		{
			ref Image ᶘ = ref _ᶘ;
			Image ነ = _ነ;
			ᶘ = (Image)(object)ᶣ.ᶧ((ነ is Bitmap) ? ነ : null);
		}
		return _ᶘ;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		InvalidateArrange();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		InvalidateArrange();
	}
}
