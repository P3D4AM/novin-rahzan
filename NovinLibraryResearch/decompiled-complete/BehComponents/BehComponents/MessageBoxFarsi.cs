using System.Drawing;
using System.Windows.Forms;
using BehComponents.MessageBox_Items;
using BehComponents.Properties;

namespace BehComponents;

public class MessageBoxFarsi
{
	internal const int _HeightPnlButtons = 50;

	internal const int _Margin = 12;

	internal static int MaxWidth => Screen.PrimaryScreen.WorkingArea.Width;

	internal static int MaxHeight => Screen.PrimaryScreen.WorkingArea.Height;

	internal static Image GetIcon(MessageBoxFarsiIcon icon)
	{
		Image result = null;
		switch (icon)
		{
		case MessageBoxFarsiIcon.Delete:
			result = (Image)(object)Resources.delete;
			break;
		case MessageBoxFarsiIcon.Error:
			result = (Image)(object)Resources.error;
			break;
		case MessageBoxFarsiIcon.Help:
			result = (Image)(object)Resources.help;
			break;
		case MessageBoxFarsiIcon.Information:
			result = (Image)(object)Resources.info;
			break;
		case MessageBoxFarsiIcon.Preference:
			result = (Image)(object)Resources.preferences;
			break;
		case MessageBoxFarsiIcon.Question:
			result = (Image)(object)Resources.question;
			break;
		case MessageBoxFarsiIcon.Stop:
			result = (Image)(object)Resources.Stop;
			break;
		case MessageBoxFarsiIcon.Warning:
			result = (Image)(object)Resources.Warning;
			break;
		}
		return result;
	}

	public static DialogResult Show(string text)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption, MessageBoxFarsiButtons buttons)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		style.Buttons = buttons;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption, MessageBoxFarsiButtons buttons, MessageBoxFarsiIcon icon)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		style.Buttons = buttons;
		style.Icon = icon;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption, MessageBoxFarsiButtons buttons, MessageBoxFarsiIcon icon, MessageBoxFarsiDefaultButton defaultButton)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		style.Buttons = buttons;
		style.Icon = icon;
		style.DefaultButton = defaultButton;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption, MessageBoxFarsiButtons buttons, MessageBoxFarsiIcon icon, MessageBoxFarsiDefaultButton defaultButton, bool rightToLeftMessage)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		style.Buttons = buttons;
		style.Icon = icon;
		style.DefaultButton = defaultButton;
		style.RightToLeftMessage = rightToLeftMessage;
		return _Show(style);
	}

	public static DialogResult Show(string text, string caption, MessageBoxFarsiButtons buttons, MessageBoxFarsiIcon icon, MessageBoxFarsiDefaultButton defaultButton, bool rightToLeftMessage, bool rightToLeftLayout)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		MessageBoxFarsiStyle style = new MessageBoxFarsiStyle(text);
		style.Message = text;
		style.Caption = caption;
		style.Buttons = buttons;
		style.Icon = icon;
		style.DefaultButton = defaultButton;
		style.RightToLeftMessage = rightToLeftMessage;
		style.RightToLeftLayout = rightToLeftLayout;
		return _Show(style);
	}

	public static DialogResult Show(MessageBoxFarsiStyle style)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _Show(style);
	}

	private static DialogResult _Show(MessageBoxFarsiStyle style)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		DialogResult result = (DialogResult)0;
		switch (style.Buttons)
		{
		case MessageBoxFarsiButtons.AbortRetryIgnore:
		{
			frmAbortRetryIgnore frmAbortRetryIgnore = new frmAbortRetryIgnore(style);
			result = ((Form)frmAbortRetryIgnore).ShowDialog();
			break;
		}
		case MessageBoxFarsiButtons.OK:
		{
			frmOK frmOK = new frmOK(style);
			result = ((Form)frmOK).ShowDialog();
			break;
		}
		case MessageBoxFarsiButtons.OkCancel:
		{
			frmOkCancel frmOkCancel = new frmOkCancel(style);
			result = ((Form)frmOkCancel).ShowDialog();
			break;
		}
		case MessageBoxFarsiButtons.RetryCancel:
		{
			frmRetryCancel frmRetryCancel = new frmRetryCancel(style);
			result = ((Form)frmRetryCancel).ShowDialog();
			break;
		}
		case MessageBoxFarsiButtons.YesNo:
		{
			frmYesNo frmYesNo = new frmYesNo(style);
			result = ((Form)frmYesNo).ShowDialog();
			break;
		}
		case MessageBoxFarsiButtons.YesNoCancel:
		{
			frmYesNoCancel frmYesNoCancel = new frmYesNoCancel(style);
			result = ((Form)frmYesNoCancel).ShowDialog();
			break;
		}
		}
		return result;
	}
}
