using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public struct TaskDialogInfo
{
	private string _Title;

	private string _Header;

	private string _Text;

	private eTaskDialogButton _DialogButtons;

	private eTaskDialogBackgroundColor _DialogColor;

	private Command[] _RadioButtons;

	private Command[] _Buttons;

	private string _FooterText;

	private Command _CheckBoxCommand;

	private eTaskDialogIcon _TaskDialogIcon;

	private Image _FooterImage;

	private bool _TopMost;

	private eTaskDialogButton _DefaultButton;

	public string Title
	{
		get
		{
			return _Title;
		}
		set
		{
			_Title = value;
		}
	}

	public string Header
	{
		get
		{
			return _Header;
		}
		set
		{
			_Header = value;
		}
	}

	public string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			_Text = value;
		}
	}

	public eTaskDialogButton DialogButtons
	{
		get
		{
			return _DialogButtons;
		}
		set
		{
			_DialogButtons = value;
		}
	}

	public eTaskDialogBackgroundColor DialogColor
	{
		get
		{
			return _DialogColor;
		}
		set
		{
			_DialogColor = value;
		}
	}

	public Command[] RadioButtons
	{
		get
		{
			return _RadioButtons;
		}
		set
		{
			_RadioButtons = value;
		}
	}

	public Command[] Buttons
	{
		get
		{
			return _Buttons;
		}
		set
		{
			_Buttons = value;
		}
	}

	public string FooterText
	{
		get
		{
			return _FooterText;
		}
		set
		{
			_FooterText = value;
		}
	}

	public Command CheckBoxCommand
	{
		get
		{
			return _CheckBoxCommand;
		}
		set
		{
			_CheckBoxCommand = value;
		}
	}

	public eTaskDialogIcon TaskDialogIcon
	{
		get
		{
			return _TaskDialogIcon;
		}
		set
		{
			_TaskDialogIcon = value;
		}
	}

	public Image FooterImage
	{
		get
		{
			return _FooterImage;
		}
		set
		{
			_FooterImage = value;
		}
	}

	public bool TopMost
	{
		get
		{
			return _TopMost;
		}
		set
		{
			_TopMost = value;
		}
	}

	public eTaskDialogButton DefaultButton
	{
		get
		{
			return _DefaultButton;
		}
		set
		{
			_DefaultButton = value;
		}
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor, Command[] radioButtons, Command[] buttons, Command checkBoxCommand, string footerText, Image footerImage)
	{
		_Title = title;
		_Header = header;
		_Text = text;
		_DialogButtons = dialogButtons;
		_DialogColor = dialogColor;
		_RadioButtons = radioButtons;
		_Buttons = buttons;
		_FooterText = footerText;
		_CheckBoxCommand = checkBoxCommand;
		_TaskDialogIcon = taskDialogIcon;
		_FooterImage = footerImage;
		_TopMost = false;
		_DefaultButton = eTaskDialogButton.Ok;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor, Command[] radioButtons, Command[] buttons, Command checkBoxCommand, string footerText, Image footerImage, bool topMost)
	{
		_Title = title;
		_Header = header;
		_Text = text;
		_DialogButtons = dialogButtons;
		_DialogColor = dialogColor;
		_RadioButtons = radioButtons;
		_Buttons = buttons;
		_FooterText = footerText;
		_CheckBoxCommand = checkBoxCommand;
		_TaskDialogIcon = taskDialogIcon;
		_FooterImage = footerImage;
		_TopMost = topMost;
		_DefaultButton = eTaskDialogButton.Ok;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
	{
		_Title = title;
		_Header = header;
		_Text = text;
		_DialogButtons = dialogButtons;
		_DialogColor = dialogColor;
		_RadioButtons = null;
		_Buttons = null;
		_FooterText = null;
		_CheckBoxCommand = null;
		_TaskDialogIcon = taskDialogIcon;
		_FooterImage = null;
		_TopMost = false;
		_DefaultButton = eTaskDialogButton.Ok;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons)
	{
		_Title = title;
		_Header = header;
		_Text = text;
		_DialogButtons = dialogButtons;
		_DialogColor = eTaskDialogBackgroundColor.Default;
		_RadioButtons = null;
		_Buttons = null;
		_FooterText = null;
		_CheckBoxCommand = null;
		_TaskDialogIcon = taskDialogIcon;
		_FooterImage = null;
		_TopMost = false;
		_DefaultButton = eTaskDialogButton.Ok;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogButton defaultButton)
	{
		_Title = title;
		_Header = header;
		_Text = text;
		_DialogButtons = dialogButtons;
		_DialogColor = eTaskDialogBackgroundColor.Default;
		_RadioButtons = null;
		_Buttons = null;
		_FooterText = null;
		_CheckBoxCommand = null;
		_TaskDialogIcon = taskDialogIcon;
		_FooterImage = null;
		_TopMost = false;
		_DefaultButton = defaultButton;
	}
}
