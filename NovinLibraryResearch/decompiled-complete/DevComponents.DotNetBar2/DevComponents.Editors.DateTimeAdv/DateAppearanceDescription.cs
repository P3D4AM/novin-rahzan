using System.ComponentModel;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

[TypeConverter(typeof(ExpandableObjectConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class DateAppearanceDescription
{
	private BaseItem _ᗚ;

	private bool __25AD;

	private Color __0CD2 = Color.Empty;

	private Color __264B = Color.Empty;

	private int __264C = 90;

	private Color __0CD1 = Color.Empty;

	private Color _Ꮃ = Color.Empty;

	private bool _Ԇ = true;

	[DefaultValue(false)]
	[Description("Indicates whether text is drawn using bold font.")]
	public bool IsBold
	{
		get
		{
			return __25AD;
		}
		set
		{
			if (__25AD != value)
			{
				__25AD = value;
				ᇌ();
			}
		}
	}

	[Description("Indicates background color for the marked day.")]
	[Category("Colors")]
	public Color BackColor
	{
		get
		{
			return __0CD2;
		}
		set
		{
			__0CD2 = value;
			ᇌ();
		}
	}

	[Category("Colors")]
	[Description("Indicates background target gradient color for the marked date.")]
	public Color BackColor2
	{
		get
		{
			return __264B;
		}
		set
		{
			__264B = value;
			ᇌ();
		}
	}

	[DefaultValue(90)]
	[Description("Indicates background gradient fill angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return __264C;
		}
		set
		{
			if (__264C != value)
			{
				__264C = value;
				ᇌ();
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates text color for the marked date.")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			ᇌ();
		}
	}

	[Browsable(false)]
	public bool IsCustomized
	{
		get
		{
			if (__0CD2.IsEmpty && __264B.IsEmpty && __0CD1.IsEmpty && !__25AD)
			{
				return !_Ꮃ.IsEmpty;
			}
			return true;
		}
	}

	internal BaseItem ކ
	{
		get
		{
			return _ᗚ;
		}
		set
		{
			if (_ᗚ != value)
			{
				_ᗚ = value;
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates borderColor color.")]
	public Color BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			_Ꮃ = value;
			ᇌ();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether day marked is selectable by end user.")]
	public bool Selectable
	{
		get
		{
			return _Ԇ;
		}
		set
		{
			if (_Ԇ != value)
			{
				_Ԇ = value;
			}
		}
	}

	public DateAppearanceDescription()
	{
	}

	public DateAppearanceDescription(BaseItem parent)
	{
		_ᗚ = parent;
	}

	public void ApplyTo(DateAppearanceDescription desc)
	{
		desc.BackColor = BackColor;
		desc.BackColor2 = BackColor2;
		desc.BackColorGradientAngle = BackColorGradientAngle;
		desc.BorderColor = BorderColor;
		desc.IsBold = IsBold;
		desc.Selectable = Selectable;
		desc.TextColor = TextColor;
	}

	private void ᇌ()
	{
		if (_ᗚ != null)
		{
			_ᗚ.Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !BackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !BackColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !BorderColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}
}
