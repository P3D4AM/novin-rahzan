using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class MessageBoxEx
{
	private static bool _UseSystemLocalizedString = false;

	private static bool _EnableGlass = true;

	private static bool _AntiAlias = false;

	private static Color _MessageBoxTextColor = Color.Empty;

	private static bool _ButtonsDividerVisible = true;

	private static FormStartPosition _DefaultStartPosition = (FormStartPosition)1;

	private static FormStartPosition _OwnerStartPosition = (FormStartPosition)4;

	private static bool _TextMarkupEnabled = true;

	public static bool UseSystemLocalizedString
	{
		get
		{
			return _UseSystemLocalizedString;
		}
		set
		{
			_UseSystemLocalizedString = value;
		}
	}

	public static bool EnableGlass
	{
		get
		{
			return _EnableGlass;
		}
		set
		{
			_EnableGlass = value;
		}
	}

	public static bool AntiAlias
	{
		get
		{
			return _AntiAlias;
		}
		set
		{
			_AntiAlias = value;
		}
	}

	public static Color MessageBoxTextColor
	{
		get
		{
			return _MessageBoxTextColor;
		}
		set
		{
			_MessageBoxTextColor = value;
		}
	}

	public static bool ButtonsDividerVisible
	{
		get
		{
			return _ButtonsDividerVisible;
		}
		set
		{
			_ButtonsDividerVisible = value;
		}
	}

	public static FormStartPosition DefaultStartPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return _DefaultStartPosition;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_DefaultStartPosition = value;
		}
	}

	public static FormStartPosition OwnerStartPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return _OwnerStartPosition;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_OwnerStartPosition = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether message box text renders text markup.")]
	public static bool TextMarkupEnabled
	{
		get
		{
			return _TextMarkupEnabled;
		}
		set
		{
			_TextMarkupEnabled = value;
		}
	}

	public static event MarkupLinkClickEventHandler MarkupLinkClick;

	internal static void InvokeMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		MessageBoxEx.MarkupLinkClick?.Invoke(sender, e);
	}

	public static DialogResult Show(string text)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(null, text, "", (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, topMost: false);
	}

	public static DialogResult Show(IWin32Window owner, string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, "", (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, GetTopMost(owner));
	}

	public static DialogResult Show(string text, string caption)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(null, text, caption, (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, topMost: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, caption, (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, GetTopMost(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(null, text, caption, buttons, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, topMost: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, caption, buttons, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, GetTopMost(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(null, text, caption, buttons, icon, (MessageBoxDefaultButton)0, topMost: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, caption, buttons, icon, (MessageBoxDefaultButton)0, GetTopMost(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(null, text, caption, buttons, icon, defaultButton, topMost: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, caption, buttons, icon, defaultButton, GetTopMost(owner));
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return ShowInternal(owner, text, caption, buttons, icon, defaultButton, topMost);
	}

	private static DialogResult ShowInternal(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)1;
		if (StyleManager.Style == eStyle.Metro)
		{
			MetroMessageBoxDialog metroMessageBoxDialog = new MetroMessageBoxDialog();
			metroMessageBoxDialog.AntiAlias = _AntiAlias;
			if (owner != null)
			{
				((Form)metroMessageBoxDialog).StartPosition = _OwnerStartPosition;
			}
			else
			{
				((Form)metroMessageBoxDialog).StartPosition = _DefaultStartPosition;
			}
			metroMessageBoxDialog.MessageTextColor = _MessageBoxTextColor;
			metroMessageBoxDialog.ButtonsDividerVisible = _ButtonsDividerVisible;
			metroMessageBoxDialog.TextMarkupEnabled = _TextMarkupEnabled;
			val = metroMessageBoxDialog.Show(owner, text, caption, buttons, icon, defaultButton, topMost);
			((Component)(object)metroMessageBoxDialog).Dispose();
		}
		else
		{
			MessageBoxDialog messageBoxDialog = new MessageBoxDialog();
			messageBoxDialog.EnableGlass = _EnableGlass;
			messageBoxDialog.AntiAlias = _AntiAlias;
			messageBoxDialog.TextMarkupEnabled = _TextMarkupEnabled;
			if (owner != null)
			{
				((Form)messageBoxDialog).StartPosition = _OwnerStartPosition;
			}
			else
			{
				((Form)messageBoxDialog).StartPosition = _DefaultStartPosition;
			}
			messageBoxDialog.MessageTextColor = _MessageBoxTextColor;
			messageBoxDialog.ButtonsDividerVisible = _ButtonsDividerVisible;
			val = messageBoxDialog.Show(owner, text, caption, buttons, icon, defaultButton, topMost);
			((Component)(object)messageBoxDialog).Dispose();
		}
		return val;
	}

	private static bool GetTopMost(IWin32Window owner)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (owner is Form)
		{
			return ((Form)owner).TopMost;
		}
		return false;
	}
}
