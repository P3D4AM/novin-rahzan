using System;
using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class ColorEx
{
	private Color __1360 = Color.Empty;

	private byte _ᗐ = byte.MaxValue;

	private eColorSchemePart ᗑ = eColorSchemePart.Custom;

	internal bool ᖸ;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public Color Color
	{
		get
		{
			return __1360;
		}
		set
		{
			__1360 = value;
			if (__1360 != Color.Empty)
			{
				ᗑ = eColorSchemePart.Custom;
				ᖸ = true;
			}
			if (this.ᗆ != null)
			{
				this.ᗆ(this, new EventArgs());
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(byte.MaxValue)]
	[NotifyParentProperty(true)]
	public byte Alpha
	{
		get
		{
			return _ᗐ;
		}
		set
		{
			if (_ᗐ != value)
			{
				_ᗐ = value;
				ᖸ = true;
			}
			if (this.ᗆ != null)
			{
				this.ᗆ(this, new EventArgs());
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eColorSchemePart.Custom)]
	public eColorSchemePart ColorSchemePart
	{
		get
		{
			return ᗑ;
		}
		set
		{
			ᗑ = value;
			if (ᗑ != eColorSchemePart.Custom)
			{
				__1360 = Color.Empty;
				ᖸ = true;
			}
			if (this.ᗆ != null)
			{
				this.ᗆ(this, new EventArgs());
			}
		}
	}

	public static ColorEx Empty => new ColorEx(Color.Empty);

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool IsEmpty => __1360 == Color.Empty;

	internal event EventHandler ᗆ;

	public ColorEx()
	{
	}

	private ColorEx(Color color)
	{
		Color = color;
		Alpha = byte.MaxValue;
	}

	private ColorEx(Color color, byte opacity)
	{
		Color = color;
		Alpha = opacity;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeColor()
	{
		if (__1360 != Color.Empty)
		{
			return ᗑ == eColorSchemePart.Custom;
		}
		return false;
	}

	internal void ᗒ(Color _0BDE)
	{
		__1360 = _0BDE;
	}

	internal void ᗓ(eColorSchemePart _065C)
	{
		ᗑ = _065C;
	}

	public Color GetCompositeColor()
	{
		if (__1360.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(_ᗐ, __1360);
	}
}
