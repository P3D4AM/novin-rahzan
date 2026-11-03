using System.Drawing;

namespace DevComponents.DotNetBar;

public struct TaskDialogInfo
{
	private string __20C2;

	private string __2F28;

	private string _ኚ;

	private eTaskDialogButton __2F29;

	private eTaskDialogBackgroundColor __2F2A;

	private Command[] __2F2B;

	private Command[] __2F2C;

	private string __2F2D;

	private Command __2F2E;

	private eTaskDialogIcon __2F2F;

	private Image __2F30;

	private bool __2B80;

	private eTaskDialogButton __2F09;

	private bool __2F31;

	public string Title
	{
		get
		{
			return __20C2;
		}
		set
		{
			__20C2 = value;
		}
	}

	public string Header
	{
		get
		{
			return __2F28;
		}
		set
		{
			__2F28 = value;
		}
	}

	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			_ኚ = value;
		}
	}

	public eTaskDialogButton DialogButtons
	{
		get
		{
			return __2F29;
		}
		set
		{
			__2F29 = value;
		}
	}

	public eTaskDialogBackgroundColor DialogColor
	{
		get
		{
			return __2F2A;
		}
		set
		{
			__2F2A = value;
		}
	}

	public Command[] RadioButtons
	{
		get
		{
			return __2F2B;
		}
		set
		{
			__2F2B = value;
		}
	}

	public Command[] Buttons
	{
		get
		{
			return __2F2C;
		}
		set
		{
			__2F2C = value;
		}
	}

	public string FooterText
	{
		get
		{
			return __2F2D;
		}
		set
		{
			__2F2D = value;
		}
	}

	public Command CheckBoxCommand
	{
		get
		{
			return __2F2E;
		}
		set
		{
			__2F2E = value;
		}
	}

	public eTaskDialogIcon TaskDialogIcon
	{
		get
		{
			return __2F2F;
		}
		set
		{
			__2F2F = value;
		}
	}

	public Image FooterImage
	{
		get
		{
			return __2F30;
		}
		set
		{
			__2F30 = value;
		}
	}

	public bool TopMost
	{
		get
		{
			return __2B80;
		}
		set
		{
			__2B80 = value;
		}
	}

	public eTaskDialogButton DefaultButton
	{
		get
		{
			return __2F09;
		}
		set
		{
			__2F09 = value;
		}
	}

	public bool FormCloseEnabled
	{
		get
		{
			return __2F31;
		}
		set
		{
			__2F31 = value;
		}
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor, Command[] radioButtons, Command[] buttons, Command checkBoxCommand, string footerText, Image footerImage)
	{
		__20C2 = title;
		__2F28 = header;
		_ኚ = text;
		__2F29 = dialogButtons;
		__2F2A = dialogColor;
		__2F2B = radioButtons;
		__2F2C = buttons;
		__2F2D = footerText;
		__2F2E = checkBoxCommand;
		__2F2F = taskDialogIcon;
		__2F30 = footerImage;
		__2B80 = false;
		__2F09 = eTaskDialogButton.Ok;
		__2F31 = true;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor, Command[] radioButtons, Command[] buttons, Command checkBoxCommand, string footerText, Image footerImage, bool topMost)
	{
		__20C2 = title;
		__2F28 = header;
		_ኚ = text;
		__2F29 = dialogButtons;
		__2F2A = dialogColor;
		__2F2B = radioButtons;
		__2F2C = buttons;
		__2F2D = footerText;
		__2F2E = checkBoxCommand;
		__2F2F = taskDialogIcon;
		__2F30 = footerImage;
		__2B80 = topMost;
		__2F09 = eTaskDialogButton.Ok;
		__2F31 = true;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
	{
		__20C2 = title;
		__2F28 = header;
		_ኚ = text;
		__2F29 = dialogButtons;
		__2F2A = dialogColor;
		__2F2B = null;
		__2F2C = null;
		__2F2D = null;
		__2F2E = null;
		__2F2F = taskDialogIcon;
		__2F30 = null;
		__2B80 = false;
		__2F09 = eTaskDialogButton.Ok;
		__2F31 = true;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons)
	{
		__20C2 = title;
		__2F28 = header;
		_ኚ = text;
		__2F29 = dialogButtons;
		__2F2A = eTaskDialogBackgroundColor.Default;
		__2F2B = null;
		__2F2C = null;
		__2F2D = null;
		__2F2E = null;
		__2F2F = taskDialogIcon;
		__2F30 = null;
		__2B80 = false;
		__2F09 = eTaskDialogButton.Ok;
		__2F31 = true;
	}

	public TaskDialogInfo(string title, eTaskDialogIcon taskDialogIcon, string header, string text, eTaskDialogButton dialogButtons, eTaskDialogButton defaultButton)
	{
		__20C2 = title;
		__2F28 = header;
		_ኚ = text;
		__2F29 = dialogButtons;
		__2F2A = eTaskDialogBackgroundColor.Default;
		__2F2B = null;
		__2F2C = null;
		__2F2D = null;
		__2F2E = null;
		__2F2F = taskDialogIcon;
		__2F30 = null;
		__2B80 = false;
		__2F09 = defaultButton;
		__2F31 = true;
	}
}
