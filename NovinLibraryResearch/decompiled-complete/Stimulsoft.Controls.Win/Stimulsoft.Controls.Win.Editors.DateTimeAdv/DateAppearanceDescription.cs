using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class DateAppearanceDescription
{
	private BaseItem _Parent;

	private bool _IsBold;

	private Color _BackColor = Color.Empty;

	private Color _BackColor2 = Color.Empty;

	private int _BackColorGradientAngle = 90;

	private Color _TextColor = Color.Empty;

	private Color _BorderColor = Color.Empty;

	private bool _Selectable = true;

	[DefaultValue(false)]
	[Description("Indicates whether text is drawn using bold font.")]
	public bool IsBold
	{
		get
		{
			return _IsBold;
		}
		set
		{
			if (_IsBold != value)
			{
				_IsBold = value;
				Refresh();
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates background color for the marked day.")]
	public Color BackColor
	{
		get
		{
			return _BackColor;
		}
		set
		{
			_BackColor = value;
			Refresh();
		}
	}

	[Category("Colors")]
	[Description("Indicates background target gradient color for the marked date.")]
	public Color BackColor2
	{
		get
		{
			return _BackColor2;
		}
		set
		{
			_BackColor2 = value;
			Refresh();
		}
	}

	[DefaultValue(90)]
	[Description("Indicates background gradient fill angle.")]
	public int BackColorGradientAngle
	{
		get
		{
			return _BackColorGradientAngle;
		}
		set
		{
			if (_BackColorGradientAngle != value)
			{
				_BackColorGradientAngle = value;
				Refresh();
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates text color for the marked date.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public bool IsCustomized
	{
		get
		{
			if (_BackColor.IsEmpty && _BackColor2.IsEmpty && _TextColor.IsEmpty && !_IsBold)
			{
				return !_BorderColor.IsEmpty;
			}
			return true;
		}
	}

	internal BaseItem Parent
	{
		get
		{
			return _Parent;
		}
		set
		{
			if (_Parent != value)
			{
				_Parent = value;
			}
		}
	}

	[Category("Colors")]
	[Description("Indicates borderColor color.")]
	public Color BorderColor
	{
		get
		{
			return _BorderColor;
		}
		set
		{
			_BorderColor = value;
			Refresh();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether day marked is selectable by end user.")]
	public bool Selectable
	{
		get
		{
			return _Selectable;
		}
		set
		{
			if (_Selectable != value)
			{
				_Selectable = value;
			}
		}
	}

	public DateAppearanceDescription()
	{
	}

	public DateAppearanceDescription(BaseItem parent)
	{
		_Parent = parent;
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

	private void Refresh()
	{
		if (_Parent != null)
		{
			_Parent.Refresh();
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
