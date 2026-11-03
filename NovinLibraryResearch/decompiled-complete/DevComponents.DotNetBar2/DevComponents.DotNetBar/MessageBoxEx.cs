using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar;

public class MessageBoxEx
{
	private static MarkupLinkClickEventHandler Ӟ;

	private static bool _Ⳇ = false;

	private static bool _ⳇ = true;

	private static bool _᮰ = false;

	private static Color _Ⳉ = Color.Empty;

	private static bool _Ⲷ = true;

	private static FormStartPosition _ⳉ = (FormStartPosition)1;

	private static FormStartPosition _Ⳋ = (FormStartPosition)4;

	private static bool _ݷ = true;

	public static bool UseSystemLocalizedString
	{
		get
		{
			return _Ⳇ;
		}
		set
		{
			_Ⳇ = value;
		}
	}

	public static bool EnableGlass
	{
		get
		{
			return _ⳇ;
		}
		set
		{
			_ⳇ = value;
		}
	}

	public static bool AntiAlias
	{
		get
		{
			return _᮰;
		}
		set
		{
			_᮰ = value;
		}
	}

	public static Color MessageBoxTextColor
	{
		get
		{
			return _Ⳉ;
		}
		set
		{
			_Ⳉ = value;
		}
	}

	public static bool ButtonsDividerVisible
	{
		get
		{
			return _Ⲷ;
		}
		set
		{
			_Ⲷ = value;
		}
	}

	public static FormStartPosition DefaultStartPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return _ⳉ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_ⳉ = value;
		}
	}

	public static FormStartPosition OwnerStartPosition
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return _Ⳋ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_Ⳋ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets whether message box text renders text markup.")]
	[Category("Behavior")]
	public static bool TextMarkupEnabled
	{
		get
		{
			return _ݷ;
		}
		set
		{
			_ݷ = value;
		}
	}

	public static event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	internal static void ۃ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		Ӟ?.Invoke(_٠, ؿ);
	}

	public static DialogResult Show(string text)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(null, text, "", (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, ⲻ: false);
	}

	public static DialogResult Show(IWin32Window owner, string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, "", (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, Ⳍ(owner));
	}

	public static DialogResult Show(string text, string caption)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(null, text, caption, (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, ⲻ: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, caption, (MessageBoxButtons)0, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, Ⳍ(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(null, text, caption, buttons, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, ⲻ: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, caption, buttons, (MessageBoxIcon)0, (MessageBoxDefaultButton)0, Ⳍ(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(null, text, caption, buttons, icon, (MessageBoxDefaultButton)0, ⲻ: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, caption, buttons, icon, (MessageBoxDefaultButton)0, Ⳍ(owner));
	}

	public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(null, text, caption, buttons, icon, defaultButton, ⲻ: false);
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, caption, buttons, icon, defaultButton, Ⳍ(owner));
	}

	public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool topMost)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return ⳋ(owner, text, caption, buttons, icon, defaultButton, topMost);
	}

	private static DialogResult ⳋ(IWin32Window ॡ, string _06EA, string ⲹ, MessageBoxButtons _2827, MessageBoxIcon ᆠ, MessageBoxDefaultButton Ⲻ, bool ⲻ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		PopupManager.CloseAllPopups();
		DialogResult val = (DialogResult)1;
		if (StyleManager.IsMetro(StyleManager.Style))
		{
			Ⲩ ⲩ = new Ⲩ();
			ⲩ.Ⲭ = _᮰;
			if (ॡ != null)
			{
				((Form)ⲩ).StartPosition = _Ⳋ;
			}
			else
			{
				((Form)ⲩ).StartPosition = _ⳉ;
			}
			ⲩ.Ⲫ = _Ⳉ;
			ⲩ.ⲩ = _Ⲷ;
			ⲩ.ⲫ = _ݷ;
			val = ⲩ._26BA(ॡ, _06EA, ⲹ, _2827, ᆠ, Ⲻ, ⲻ);
			((Component)(object)ⲩ).Dispose();
		}
		else
		{
			ⳍ ⳍ2 = new ⳍ();
			ⳍ2.EnableGlass = _ⳇ;
			ⳍ2.Ⲭ = _᮰;
			ⳍ2.ⲫ = _ݷ;
			if (ॡ != null)
			{
				((Form)ⳍ2).StartPosition = _Ⳋ;
			}
			else
			{
				((Form)ⳍ2).StartPosition = _ⳉ;
			}
			ⳍ2.Ⲫ = _Ⳉ;
			ⳍ2.ⲩ = _Ⲷ;
			val = ⳍ2._26BA(ॡ, _06EA, ⲹ, _2827, ᆠ, Ⲻ, ⲻ);
			((Component)(object)ⳍ2).Dispose();
		}
		return val;
	}

	private static bool Ⳍ(IWin32Window ॡ)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (ॡ is Form)
		{
			return ((Form)ॡ).TopMost;
		}
		return false;
	}
}
