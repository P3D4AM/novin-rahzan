using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class InputButtonSettings : IComparable
{
	private IInputButtonControl _Parent;

	private bool _Visible;

	private bool _Enabled = true;

	private int _DisplayPosition;

	private Image _Image;

	private string _Text = "";

	private bool _Checked;

	private VisualItem _ItemReference;

	private eShortcut _Shortcut;

	[DefaultValue(false)]
	[Description("Indicates whether button is visible.")]
	public bool Visible
	{
		get
		{
			return _Visible;
		}
		set
		{
			if (_Visible != value)
			{
				_Visible = value;
				OnVisibleChanged();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether button is enabled.")]
	public bool Enabled
	{
		get
		{
			return _Enabled;
		}
		set
		{
			if (_Enabled != value)
			{
				_Enabled = value;
				NotifyParent();
			}
		}
	}

	[DefaultValue(0)]
	[Description("Indicates display position index of the button.")]
	[Localizable(true)]
	public int DisplayPosition
	{
		get
		{
			return _DisplayPosition;
		}
		set
		{
			if (_DisplayPosition != value)
			{
				_DisplayPosition = value;
				OnDisplayPositionChanged();
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
			return _Image;
		}
		set
		{
			if (_Image != value)
			{
				_Image = value;
				OnImageChanged();
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
			return _Text;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_Text != value)
			{
				_Text = value;
				OnTextChanged();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets whether button is checked.")]
	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			if (_Checked != value)
			{
				_Checked = value;
				OnCheckedChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public VisualItem ItemReference
	{
		get
		{
			return _ItemReference;
		}
		set
		{
			_ItemReference = value;
		}
	}

	[DefaultValue(eShortcut.None)]
	[Description("Indicates shortcut key which when pressed triggers button click event or its default function.")]
	public eShortcut Shortcut
	{
		get
		{
			return _Shortcut;
		}
		set
		{
			if (_Shortcut != value)
			{
				_Shortcut = value;
				NotifyParent();
			}
		}
	}

	public InputButtonSettings()
	{
	}

	public InputButtonSettings(IInputButtonControl parent)
	{
		_Parent = parent;
	}

	private void OnVisibleChanged()
	{
		NotifyParent();
	}

	private void OnDisplayPositionChanged()
	{
		NotifyParent();
	}

	private void OnImageChanged()
	{
		NotifyParent();
	}

	private void OnTextChanged()
	{
		NotifyParent();
	}

	private void OnCheckedChanged()
	{
		NotifyParent();
	}

	private void NotifyParent()
	{
		if (_Parent != null)
		{
			_Parent.InputButtonSettingsChanged(this);
		}
	}

	internal void CopyToItem(InputButtonSettings copy)
	{
		copy.Visible = Visible;
		copy.Enabled = Enabled;
		copy.DisplayPosition = DisplayPosition;
		copy.Image = Image;
		copy.Text = Text;
		copy.Checked = Checked;
		copy.Shortcut = Shortcut;
	}

	int IComparable.CompareTo(object obj)
	{
		if (obj is InputButtonSettings)
		{
			int num = ((InputButtonSettings)obj).DisplayPosition - DisplayPosition;
			if (num == 0 && obj != this)
			{
				num = -1;
			}
			return num;
		}
		return 0;
	}
}
