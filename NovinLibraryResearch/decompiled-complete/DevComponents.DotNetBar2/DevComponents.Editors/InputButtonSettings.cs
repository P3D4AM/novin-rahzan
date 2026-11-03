using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

[TypeConverter(typeof(ExpandableObjectConverter))]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class InputButtonSettings : IComparable
{
	private IInputButtonControl _ᗚ;

	private bool _ᚉ;

	private bool _Ԋ = true;

	private int _ᠼ;

	private Image _ነ;

	private string _ኚ = "";

	private bool _ᚆ;

	private VisualItem _ᠽ;

	private eShortcut _ᠾ;

	private string _ݩ = "";

	private string _ລ = "";

	private eSymbolSet _ວ;

	private Color __0EA4 = Color.Empty;

	[Description("Indicates whether button is visible.")]
	[DefaultValue(false)]
	public bool Visible
	{
		get
		{
			return _ᚉ;
		}
		set
		{
			if (_ᚉ != value)
			{
				_ᚉ = value;
				ᠿ();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether button is enabled.")]
	public bool Enabled
	{
		get
		{
			return _Ԋ;
		}
		set
		{
			if (_Ԋ != value)
			{
				_Ԋ = value;
				ᡃ();
			}
		}
	}

	[Localizable(true)]
	[Description("Indicates display position index of the button.")]
	[DefaultValue(0)]
	public int DisplayPosition
	{
		get
		{
			return _ᠼ;
		}
		set
		{
			if (_ᠼ != value)
			{
				_ᠼ = value;
				ᡀ();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates image displayed on the face of the button.")]
	[Localizable(true)]
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
				ᡁ();
			}
		}
	}

	[DefaultValue("")]
	[Description("Input text displayed on the input button face.")]
	[Localizable(true)]
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
				ᡂ();
			}
		}
	}

	[Description("Gets or sets whether button is checked.")]
	[DefaultValue(false)]
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
				ᒚ();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public VisualItem ItemReference
	{
		get
		{
			return _ᠽ;
		}
		set
		{
			_ᠽ = value;
		}
	}

	[DefaultValue(eShortcut.None)]
	[Description("Indicates shortcut key which when pressed triggers button click event or its default function.")]
	public eShortcut Shortcut
	{
		get
		{
			return _ᠾ;
		}
		set
		{
			if (_ᠾ != value)
			{
				_ᠾ = value;
				ᡃ();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates tooltip displayed for the button when mouse hovers over it.")]
	[DefaultValue("")]
	[Localizable(true)]
	public string Tooltip
	{
		get
		{
			return _ݩ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ݩ = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
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
			ᡃ();
		}
	}

	public InputButtonSettings()
	{
	}

	public InputButtonSettings(IInputButtonControl parent)
	{
		_ᗚ = parent;
	}

	private void ᠿ()
	{
		ᡃ();
	}

	private void ᡀ()
	{
		ᡃ();
	}

	private void ᡁ()
	{
		ᡃ();
	}

	private void ᡂ()
	{
		ᡃ();
	}

	private void ᒚ()
	{
		ᡃ();
	}

	private void ᡃ()
	{
		if (_ᗚ != null)
		{
			_ᗚ.InputButtonSettingsChanged(this);
		}
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		ᡃ();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		ᡃ();
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

	internal void ᡄ(InputButtonSettings _0EBB)
	{
		_0EBB.Visible = Visible;
		_0EBB.Enabled = Enabled;
		_0EBB.DisplayPosition = DisplayPosition;
		_0EBB.Image = Image;
		_0EBB.Text = Text;
		_0EBB.Checked = Checked;
		_0EBB.Shortcut = Shortcut;
	}

	private int ᡅ(object ᡆ)
	{
		if (ᡆ is InputButtonSettings)
		{
			int num = ((InputButtonSettings)ᡆ).DisplayPosition - DisplayPosition;
			if (num == 0 && ᡆ != this)
			{
				num = -1;
			}
			return num;
		}
		return 0;
	}

	int IComparable.CompareTo(object ᡆ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᡅ
		return this.ᡅ(ᡆ);
	}
}
