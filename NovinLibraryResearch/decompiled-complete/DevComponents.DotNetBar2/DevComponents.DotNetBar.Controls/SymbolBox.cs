using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(SymbolBox), "Controls.SymbolBox.ico")]
public class SymbolBox : Control
{
	private ElementStyle _ᠩ;

	private string _ລ = "\uf015";

	private string __0EA6 = "\uf015";

	private eSymbolSet _ວ;

	private Color __0EA4 = Color.Empty;

	private float _ຨ;

	[Description("Gets or sets bar background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Style")]
	[Browsable(true)]
	public ElementStyle BackgroundStyle => _ᠩ;

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[DefaultValue("\uf015")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
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

	[Description("Indicates color of the Symbol.")]
	[Category("Appearance")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			((Control)this).Refresh();
		}
	}

	[DefaultValue(0f)]
	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	public SymbolBox()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)(0x3816 | DisplayHelp.DoubleBufferFlag), true);
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((Control)this).BackColor.IsEmpty || ((Control)this).BackColor == Color.Transparent)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (_ᠩ != null)
		{
			_ᠩ.SetColorScheme(GetColorScheme());
		}
		PaintBackground(e);
		ⱬ(e);
		((Control)this).OnPaint(e);
	}

	private void ⱬ(PaintEventArgs ؿ)
	{
		float num = _ຨ;
		Graphics graphics = ؿ.Graphics;
		Rectangle bounds = ᝈ._1756(_ᠩ, ((Control)this).ClientRectangle);
		if (num <= 0f)
		{
			num = Math.Max(1f, (float)(Math.Min(bounds.Width, bounds.Height) * 72) / graphics.DpiX - 1f);
		}
		Color color = __0EA4;
		if (__0EA4.IsEmpty)
		{
			color = ((!_ᠩ.TextColor.IsEmpty) ? _ᠩ.TextColor : (((Control)this).ForeColor.IsEmpty ? SystemColors.ControlText : ((Control)this).ForeColor));
		}
		TextDrawing.DrawStringLegacy(graphics, __0EA6, Symbols.GetFont(num, _ວ), color, bounds, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
	}

	protected virtual void PaintBackground(PaintEventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		ElementStyle ᠩ = _ᠩ;
		if (!((Control)this).BackColor.IsEmpty && ((Control)this).BackColor != Color.Transparent)
		{
			DisplayHelp.FillRectangle(graphics, clientRectangle, ((Control)this).BackColor);
		}
		if (((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (ᠩ.Custom)
		{
			_ = graphics.SmoothingMode;
			ElementStyleDisplayInfo e2 = new ElementStyleDisplayInfo(ᠩ, e.Graphics, clientRectangle);
			ElementStyleDisplay.Paint(e2);
		}
	}

	protected virtual ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GlobalManager.Renderer;
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return new ColorScheme();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundStyle()
	{
		_ᠩ.StyleChanged -= ઐ;
		_ᠩ = new ElementStyle();
		_ᠩ.StyleChanged += ઐ;
		((Control)this).Invalidate();
	}

	private void ઐ(object _٠, EventArgs ؿ)
	{
		((Control)this).Invalidate();
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
		((Control)this).Invalidate();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		((Control)this).Invalidate();
	}
}
