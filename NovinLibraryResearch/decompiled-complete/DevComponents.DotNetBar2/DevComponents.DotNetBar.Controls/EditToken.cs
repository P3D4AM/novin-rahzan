using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class EditToken : Component
{
	private eTokenPart __24EA;

	private string _ᗸ = "";

	private string _ኚ = "";

	private object __1714;

	private Rectangle __1734 = Rectangle.Empty;

	private Image _ነ;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private Color __0EA4 = Color.Empty;

	private string _ݩ = "";

	private Rectangle __2BF6 = Rectangle.Empty;

	private Rectangle __1F4E = Rectangle.Empty;

	private bool _ԅ;

	private bool _ᡒ;

	[Description("Indicates the token value, for example an email token has email address as token Value and full name as token Text.")]
	[DefaultValue("")]
	[Category("Data")]
	public string Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			if (value != _ᗸ)
			{
				string ᗸ = _ᗸ;
				_ᗸ = value;
				OnValueChanged(ᗸ, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates the token text, for example an email token has email address as token Value and full name as token Text.")]
	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			if (value != _ኚ)
			{
				string ኚ = _ኚ;
				_ኚ = value;
				OnTextChanged(ኚ, value);
			}
		}
	}

	[Localizable(false)]
	[TypeConverter(typeof(StringConverter))]
	[Description("Custom data associated with the object")]
	[DefaultValue(null)]
	[Category("Data")]
	public object Tag
	{
		get
		{
			return __1714;
		}
		set
		{
			__1714 = value;
		}
	}

	[Browsable(false)]
	public Rectangle Bounds
	{
		get
		{
			return __1734;
		}
		internal set
		{
			__1734 = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates the image that is displayed next to the token")]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Description("Indicates the symbol displayed on face of the token instead of the image. Setting the symbol overrides the image setting.")]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
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
			Invalidate();
		}
	}

	[Description("Indicates tooltip that is displayed when mouse is over the token and token is selected")]
	[DefaultValue("")]
	[Category("Behavior")]
	public string Tooltip
	{
		get
		{
			return _ݩ;
		}
		set
		{
			if (value != _ݩ)
			{
				string ݩ = _ݩ;
				_ݩ = value;
				OnTooltipChanged(ݩ, value);
			}
		}
	}

	[Browsable(false)]
	public eTokenPart MouseOverPart
	{
		get
		{
			return __24EA;
		}
		internal set
		{
			if (value != __24EA)
			{
				eTokenPart oldValue = __24EA;
				__24EA = value;
				OnMouseOverPartChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public Rectangle RemoveButtonBounds
	{
		get
		{
			return __2BF6;
		}
		internal set
		{
			__2BF6 = value;
		}
	}

	[Browsable(false)]
	public Rectangle ImageBounds
	{
		get
		{
			return __1F4E;
		}
		internal set
		{
			__1F4E = value;
		}
	}

	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			return _ԅ;
		}
		internal set
		{
			_ԅ = value;
		}
	}

	[Browsable(false)]
	public bool IsFocused
	{
		get
		{
			return _ᡒ;
		}
		internal set
		{
			_ᡒ = value;
		}
	}

	public EditToken(string value)
	{
		_ᗸ = value;
	}

	public EditToken(string value, string text)
	{
		_ᗸ = value;
		_ኚ = text;
	}

	public EditToken(string value, string text, Image image)
	{
		_ᗸ = value;
		_ኚ = text;
		_ነ = image;
	}

	protected virtual void OnValueChanged(string oldValue, string newValue)
	{
	}

	protected virtual void OnTextChanged(string oldValue, string newValue)
	{
	}

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
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
		Invalidate();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		Invalidate();
	}

	private void Invalidate()
	{
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

	protected virtual void OnTooltipChanged(string oldValue, string newValue)
	{
	}

	protected virtual void OnMouseOverPartChanged(eTokenPart oldValue, eTokenPart newValue)
	{
	}
}
