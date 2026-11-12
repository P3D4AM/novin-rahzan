using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
[EditorBrowsable(EditorBrowsableState.Never)]
public class ColorEx
{
	private Color _Color = Color.Empty;

	private byte _Alpha = byte.MaxValue;

	private eColorSchemePart m_ColorSchemePart = eColorSchemePart.Custom;

	internal bool Custom;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	public Color Color
	{
		get
		{
			return _Color;
		}
		set
		{
			_Color = value;
			if (_Color != Color.Empty)
			{
				m_ColorSchemePart = eColorSchemePart.Custom;
				Custom = true;
			}
			if (this.ColorChanged != null)
			{
				this.ColorChanged(this, new EventArgs());
			}
		}
	}

	[DefaultValue(byte.MaxValue)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public byte Alpha
	{
		get
		{
			return _Alpha;
		}
		set
		{
			if (_Alpha != value)
			{
				_Alpha = value;
				Custom = true;
			}
			if (this.ColorChanged != null)
			{
				this.ColorChanged(this, new EventArgs());
			}
		}
	}

	[DefaultValue(eColorSchemePart.Custom)]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eColorSchemePart ColorSchemePart
	{
		get
		{
			return m_ColorSchemePart;
		}
		set
		{
			m_ColorSchemePart = value;
			if (m_ColorSchemePart != eColorSchemePart.Custom)
			{
				_Color = Color.Empty;
				Custom = true;
			}
			if (this.ColorChanged != null)
			{
				this.ColorChanged(this, new EventArgs());
			}
		}
	}

	public static ColorEx Empty => new ColorEx(Color.Empty);

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public bool IsEmpty => _Color == Color.Empty;

	internal event EventHandler ColorChanged;

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
		if (_Color != Color.Empty)
		{
			return m_ColorSchemePart == eColorSchemePart.Custom;
		}
		return false;
	}

	internal void SetColor(Color c)
	{
		_Color = c;
	}

	internal void SetColorSchemePart(eColorSchemePart p)
	{
		m_ColorSchemePart = p;
	}

	public Color GetCompositeColor()
	{
		if (_Color.IsEmpty)
		{
			return Color.Empty;
		}
		return Color.FromArgb(_Alpha, _Color);
	}
}
