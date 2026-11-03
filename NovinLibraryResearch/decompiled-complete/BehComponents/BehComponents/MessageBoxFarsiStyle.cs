using System.Drawing;

namespace BehComponents;

public struct MessageBoxFarsiStyle
{
	private string _Caption;

	private string _Message;

	private MessageBoxFarsiIcon _Icon;

	private MessageBoxFarsiButtons _Buttons;

	private MessageBoxFarsiDefaultButton _DefaultButton;

	private bool _RightToLeftMessage;

	private bool _RightToLeftLayout;

	private MessageBoxFarsiButtonStyles _ButtonStyle;

	private bool _ShowToolTip;

	private Color _FormBackColor;

	private Color _ButtonsBackColor;

	private bool _EnglishButtons;

	private Font _Font;

	private bool _ShowButtonImages;

	private Image _AbortButtonImage;

	private Image _RetryButtonImage;

	private Image _IgnoreButtonImage;

	private Image _OkButtonImage;

	private Image _CancelButtonImage;

	private Image _YesButtonImage;

	private Image _NoButtonImage;

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
		}
	}

	public string Message
	{
		get
		{
			return _Message;
		}
		set
		{
			_Message = value;
		}
	}

	public MessageBoxFarsiIcon Icon
	{
		get
		{
			return _Icon;
		}
		set
		{
			_Icon = value;
		}
	}

	public MessageBoxFarsiButtons Buttons
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

	public MessageBoxFarsiDefaultButton DefaultButton
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

	public bool RightToLeftMessage
	{
		get
		{
			return _RightToLeftMessage;
		}
		set
		{
			_RightToLeftMessage = value;
		}
	}

	public bool RightToLeftLayout
	{
		get
		{
			return _RightToLeftLayout;
		}
		set
		{
			_RightToLeftLayout = value;
		}
	}

	public MessageBoxFarsiButtonStyles ButtonStyle
	{
		get
		{
			return _ButtonStyle;
		}
		set
		{
			_ButtonStyle = value;
		}
	}

	public bool ShowToolTip
	{
		get
		{
			return _ShowToolTip;
		}
		set
		{
			_ShowToolTip = value;
		}
	}

	public Color FormBackColor
	{
		get
		{
			return _FormBackColor;
		}
		set
		{
			_FormBackColor = value;
		}
	}

	public Color ButtonsBackColor
	{
		get
		{
			return _ButtonsBackColor;
		}
		set
		{
			_ButtonsBackColor = value;
		}
	}

	public bool EnglishButtons
	{
		get
		{
			return _EnglishButtons;
		}
		set
		{
			_EnglishButtons = value;
		}
	}

	public Font Font
	{
		get
		{
			return _Font;
		}
		set
		{
			_Font = value;
		}
	}

	public bool ShowButtonImages
	{
		get
		{
			return _ShowButtonImages;
		}
		set
		{
			_ShowButtonImages = value;
		}
	}

	public Image AbortButtonImage
	{
		get
		{
			return _AbortButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_AbortButtonImage = (Image)(object)image;
		}
	}

	public Image RetryButtonImage
	{
		get
		{
			return _RetryButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_RetryButtonImage = (Image)(object)image;
		}
	}

	public Image IgnoreButtonImage
	{
		get
		{
			return _IgnoreButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_IgnoreButtonImage = (Image)(object)image;
		}
	}

	public Image OkButtonImage
	{
		get
		{
			return _OkButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_OkButtonImage = (Image)(object)image;
		}
	}

	public Image CancelButtonImage
	{
		get
		{
			return _CancelButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_CancelButtonImage = (Image)(object)image;
		}
	}

	public Image YesButtonImage
	{
		get
		{
			return _YesButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_YesButtonImage = (Image)(object)image;
		}
	}

	public Image NoButtonImage
	{
		get
		{
			return _NoButtonImage;
		}
		set
		{
			Bitmap image = getImage(value);
			_NoButtonImage = (Image)(object)image;
		}
	}

	private Bitmap getImage(Image image)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Size size = new Size(16, 16);
		return new Bitmap(image, size);
	}

	public MessageBoxFarsiStyle(string messageText)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		_Caption = "";
		_Buttons = MessageBoxFarsiButtons.OK;
		_ButtonsBackColor = Color.Beige;
		_ButtonStyle = MessageBoxFarsiButtonStyles.Blue;
		_DefaultButton = MessageBoxFarsiDefaultButton.Button1;
		_FormBackColor = Color.White;
		_Icon = MessageBoxFarsiIcon.None;
		_Message = messageText;
		_ShowToolTip = false;
		_YesButtonImage = (_RetryButtonImage = (_OkButtonImage = (_NoButtonImage = (_IgnoreButtonImage = (_CancelButtonImage = (_AbortButtonImage = null))))));
		_ShowButtonImages = false;
		_EnglishButtons = false;
		_Font = new Font("Tahoma", 8.25f, (FontStyle)0);
		_RightToLeftLayout = true;
		_RightToLeftMessage = true;
	}

	public MessageBoxFarsiStyle(string messageText, bool styleIsFarsi)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		_Caption = "";
		_Buttons = MessageBoxFarsiButtons.OK;
		_ButtonsBackColor = Color.Beige;
		_ButtonStyle = MessageBoxFarsiButtonStyles.Blue;
		_DefaultButton = MessageBoxFarsiDefaultButton.Button1;
		_FormBackColor = Color.White;
		_Icon = MessageBoxFarsiIcon.None;
		_Message = messageText;
		_ShowToolTip = false;
		_YesButtonImage = (_RetryButtonImage = (_OkButtonImage = (_NoButtonImage = (_IgnoreButtonImage = (_CancelButtonImage = (_AbortButtonImage = null))))));
		_ShowButtonImages = false;
		if (styleIsFarsi)
		{
			_EnglishButtons = false;
			_Font = new Font("Tahoma", 8.25f, (FontStyle)0);
			_RightToLeftLayout = true;
			_RightToLeftMessage = true;
		}
		else
		{
			_EnglishButtons = true;
			_Font = new Font(FontFamily.GenericSansSerif, 8.25f, (FontStyle)0);
			_RightToLeftLayout = false;
			_RightToLeftMessage = false;
		}
	}

	public MessageBoxFarsiStyle(string messageText, bool styleIsFarsi, string caption)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		_Caption = caption;
		_Buttons = MessageBoxFarsiButtons.OK;
		_ButtonsBackColor = Color.Beige;
		_ButtonStyle = MessageBoxFarsiButtonStyles.Blue;
		_DefaultButton = MessageBoxFarsiDefaultButton.Button1;
		_FormBackColor = Color.White;
		_Icon = MessageBoxFarsiIcon.None;
		_Message = messageText;
		_ShowToolTip = false;
		_YesButtonImage = (_RetryButtonImage = (_OkButtonImage = (_NoButtonImage = (_IgnoreButtonImage = (_CancelButtonImage = (_AbortButtonImage = null))))));
		_ShowButtonImages = false;
		if (styleIsFarsi)
		{
			_EnglishButtons = false;
			_Font = new Font("Tahoma", 8.25f, (FontStyle)0);
			_RightToLeftLayout = true;
			_RightToLeftMessage = true;
		}
		else
		{
			_EnglishButtons = true;
			_Font = new Font(FontFamily.GenericSansSerif, 8.25f, (FontStyle)0);
			_RightToLeftLayout = false;
			_RightToLeftMessage = false;
		}
	}

	public MessageBoxFarsiStyle(string messageText, bool styleIsFarsi, string caption, MessageBoxFarsiButtons messageBoxFarsiButtons)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		_Caption = caption;
		_Buttons = messageBoxFarsiButtons;
		_ButtonsBackColor = Color.Beige;
		_ButtonStyle = MessageBoxFarsiButtonStyles.Blue;
		_DefaultButton = MessageBoxFarsiDefaultButton.Button1;
		_FormBackColor = Color.White;
		_Icon = MessageBoxFarsiIcon.None;
		_Message = messageText;
		_ShowToolTip = false;
		_YesButtonImage = (_RetryButtonImage = (_OkButtonImage = (_NoButtonImage = (_IgnoreButtonImage = (_CancelButtonImage = (_AbortButtonImage = null))))));
		_ShowButtonImages = false;
		if (styleIsFarsi)
		{
			_EnglishButtons = false;
			_Font = new Font("Tahoma", 8.25f, (FontStyle)0);
			_RightToLeftLayout = true;
			_RightToLeftMessage = true;
		}
		else
		{
			_EnglishButtons = true;
			_Font = new Font(FontFamily.GenericSansSerif, 8.25f, (FontStyle)0);
			_RightToLeftLayout = false;
			_RightToLeftMessage = false;
		}
	}
}
