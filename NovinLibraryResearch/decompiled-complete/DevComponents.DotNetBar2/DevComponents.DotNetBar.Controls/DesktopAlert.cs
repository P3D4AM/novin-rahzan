using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public static class DesktopAlert
{
	private static Size __2B66 = new Size(400, 128);

	private static eAlertPosition __2B67 = eAlertPosition.BottomRight;

	private static eDesktopAlertColor __2B68 = eDesktopAlertColor.Default;

	private static int __2B69 = 200;

	private static int __2B6A = 6;

	private static bool _ݷ = true;

	private static bool __2B6B = true;

	private static EventHandler _2B6C;

	private static AlertClosedEventHandler _2B6D;

	public static Size MaximumAlertSize
	{
		get
		{
			return __2B66;
		}
		set
		{
			__2B66 = value;
		}
	}

	public static eAlertPosition AlertPosition
	{
		get
		{
			return __2B67;
		}
		set
		{
			__2B67 = value;
		}
	}

	public static eDesktopAlertColor AlertColor
	{
		get
		{
			return __2B68;
		}
		set
		{
			__2B68 = value;
		}
	}

	public static int AlertAnimationDuration
	{
		get
		{
			return __2B69;
		}
		set
		{
			__2B69 = value;
		}
	}

	public static int AutoCloseTimeOut
	{
		get
		{
			return __2B6A;
		}
		set
		{
			__2B6A = value;
		}
	}

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

	public static bool PlaySound
	{
		get
		{
			return __2B6B;
		}
		set
		{
			__2B6B = value;
		}
	}

	[Description("Occurs before alert is displayed and allows access to the alert Window through sender.")]
	public static event EventHandler BeforeAlertDisplayed
	{
		add
		{
			EventHandler eventHandler = _2B6C;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B6C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2B6C;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B6C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs after alert has been closed.")]
	public static event AlertClosedEventHandler AlertClosed
	{
		add
		{
			AlertClosedEventHandler alertClosedEventHandler = _2B6D;
			AlertClosedEventHandler alertClosedEventHandler2;
			do
			{
				alertClosedEventHandler2 = alertClosedEventHandler;
				AlertClosedEventHandler value2 = (AlertClosedEventHandler)Delegate.Combine(alertClosedEventHandler2, value);
				alertClosedEventHandler = Interlocked.CompareExchange(ref _2B6D, value2, alertClosedEventHandler2);
			}
			while ((object)alertClosedEventHandler != alertClosedEventHandler2);
		}
		remove
		{
			AlertClosedEventHandler alertClosedEventHandler = _2B6D;
			AlertClosedEventHandler alertClosedEventHandler2;
			do
			{
				alertClosedEventHandler2 = alertClosedEventHandler;
				AlertClosedEventHandler value2 = (AlertClosedEventHandler)Delegate.Remove(alertClosedEventHandler2, value);
				alertClosedEventHandler = Interlocked.CompareExchange(ref _2B6D, value2, alertClosedEventHandler2);
			}
			while ((object)alertClosedEventHandler != alertClosedEventHandler2);
		}
	}

	public static void Show(string text)
	{
		Show(text, __2B68, __2B67, null);
	}

	public static void Show(string text, Control referenceControl)
	{
		Show(text, __2B68, __2B67, null, referenceControl);
	}

	public static void Show(string text, MarkupLinkClickEventHandler markupLinkClickHandler)
	{
		Show(text, __2B68, __2B67, markupLinkClickHandler);
	}

	public static void Show(string text, eDesktopAlertColor alertColor)
	{
		Show(text, alertColor, __2B67, null);
	}

	public static void Show(string text, eDesktopAlertColor alertColor, Control referenceControl)
	{
		Show(text, alertColor, __2B67, null, referenceControl);
	}

	public static void Show(string text, eAlertPosition position)
	{
		Show(text, __2B68, position, null);
	}

	public static void Show(string text, eAlertPosition position, Control referenceControl)
	{
		Show(text, __2B68, position, null, referenceControl);
	}

	public static void Show(string text, Image image)
	{
		Show(text, image, __2B68, __2B67, __2B6A, 0L, null);
	}

	public static void Show(string text, eDesktopAlertColor alertColor, eAlertPosition position)
	{
		Show(text, alertColor, position, null);
	}

	public static void Show(string text, eDesktopAlertColor alertColor, eAlertPosition position, MarkupLinkClickEventHandler markupLinkClickHandler, Control referenceControl)
	{
		DesktopAlertWindow desktopAlertWindow = new DesktopAlertWindow();
		((Control)desktopAlertWindow).Text = text;
		((Control)desktopAlertWindow).MaximumSize = Dpi.Size(__2B66);
		desktopAlertWindow.AlertPosition = position;
		desktopAlertWindow.AutoCloseTimeOut = __2B6A;
		desktopAlertWindow.TextMarkupEnabled = _ݷ;
		if (markupLinkClickHandler != null)
		{
			desktopAlertWindow.MarkupLinkClick += markupLinkClickHandler;
		}
		desktopAlertWindow.AlertAnimationDuration = __2B69;
		desktopAlertWindow.PlaySound = __2B6B;
		desktopAlertWindow.ReferenceControl = referenceControl;
		_2B6E(desktopAlertWindow, alertColor);
		desktopAlertWindow.Show();
	}

	public static void Show(string text, eDesktopAlertColor alertColor, eAlertPosition position, MarkupLinkClickEventHandler markupLinkClickHandler)
	{
		Show(text, alertColor, position, markupLinkClickHandler, null);
	}

	public static void Show(string text, long alertId, Action<long> alertClickAction)
	{
		Show(text, null, __2B68, __2B67, __2B6A, alertId, alertClickAction);
	}

	public static void Show(string text, eDesktopAlertColor alertColor, long alertId, Action<long> alertClickAction)
	{
		Show(text, null, alertColor, __2B67, __2B6A, alertId, alertClickAction);
	}

	public static void Show(string text, string symbol, eSymbolSet symbolSet, Color symbolColor, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction)
	{
		Show(text, symbol, symbolSet, symbolColor, alertColor, position, alertDurationSeconds, alertId, alertClickAction, null);
	}

	public static void Show(string text, string symbol, eSymbolSet symbolSet, Color symbolColor, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction, MarkupLinkClickEventHandler markupLinkClickHandler)
	{
		Show(text, symbol, symbolSet, symbolColor, alertColor, position, alertDurationSeconds, alertId, alertClickAction, markupLinkClickHandler, null);
	}

	public static void Show(string text, string symbol, eSymbolSet symbolSet, Color symbolColor, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction, MarkupLinkClickEventHandler markupLinkClickHandler, Control referenceControl)
	{
		DesktopAlertWindow desktopAlertWindow = new DesktopAlertWindow();
		((Control)desktopAlertWindow).Text = text;
		((Control)desktopAlertWindow).MaximumSize = Dpi.Size(__2B66);
		desktopAlertWindow.AlertPosition = position;
		desktopAlertWindow.Symbol = symbol;
		desktopAlertWindow.SymbolSet = symbolSet;
		desktopAlertWindow.SymbolColor = symbolColor;
		desktopAlertWindow.AlertId = alertId;
		desktopAlertWindow.ClickAction = alertClickAction;
		desktopAlertWindow.AutoCloseTimeOut = alertDurationSeconds;
		desktopAlertWindow.TextMarkupEnabled = _ݷ;
		if (markupLinkClickHandler != null)
		{
			desktopAlertWindow.MarkupLinkClick += markupLinkClickHandler;
		}
		desktopAlertWindow.AlertAnimationDuration = __2B69;
		desktopAlertWindow.PlaySound = __2B6B;
		desktopAlertWindow.ReferenceControl = referenceControl;
		_2B6E(desktopAlertWindow, alertColor);
		desktopAlertWindow.Show();
	}

	public static void Show(string text, Image image, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction)
	{
		Show(text, image, alertColor, position, alertDurationSeconds, alertId, alertClickAction, null);
	}

	public static void Show(string text, Image image, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction, MarkupLinkClickEventHandler markupLinkClickHandler)
	{
		Show(text, image, alertColor, position, alertDurationSeconds, alertId, alertClickAction, markupLinkClickHandler, null);
	}

	public static void Show(string text, Image image, eDesktopAlertColor alertColor, eAlertPosition position, int alertDurationSeconds, long alertId, Action<long> alertClickAction, MarkupLinkClickEventHandler markupLinkClickHandler, Control referenceControl)
	{
		DesktopAlertWindow desktopAlertWindow = new DesktopAlertWindow();
		((Control)desktopAlertWindow).Text = text;
		((Control)desktopAlertWindow).MaximumSize = Dpi.Size(__2B66);
		desktopAlertWindow.AlertPosition = position;
		desktopAlertWindow.Image = image;
		desktopAlertWindow.AlertId = alertId;
		desktopAlertWindow.ClickAction = alertClickAction;
		desktopAlertWindow.AutoCloseTimeOut = alertDurationSeconds;
		desktopAlertWindow.TextMarkupEnabled = _ݷ;
		if (markupLinkClickHandler != null)
		{
			desktopAlertWindow.MarkupLinkClick += markupLinkClickHandler;
		}
		desktopAlertWindow.AlertAnimationDuration = __2B69;
		desktopAlertWindow.PlaySound = __2B6B;
		desktopAlertWindow.ReferenceControl = referenceControl;
		_2B6E(desktopAlertWindow, alertColor);
		desktopAlertWindow.Show();
	}

	private static void _2B6E(DesktopAlertWindow _2B6F, eDesktopAlertColor _0BDE)
	{
		switch (_0BDE)
		{
		case eDesktopAlertColor.Default:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(30935);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.Black:
			((Control)_2B6F).BackColor = Color.Black;
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.Blue:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(6003669);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.BlueGray:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(4478058);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.DarkBlue:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(4485828);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.DarkRed:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(12582912);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.Gold:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(16760832);
			((Control)_2B6F).ForeColor = Color.Black;
			break;
		case eDesktopAlertColor.Gray:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(15197926);
			((Control)_2B6F).ForeColor = Color.Black;
			break;
		case eDesktopAlertColor.Green:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(3626531);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.Orange:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(13258768);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		case eDesktopAlertColor.Red:
			((Control)_2B6F).BackColor = ColorScheme.GetColor(15208739);
			((Control)_2B6F).ForeColor = Color.White;
			break;
		}
	}

	internal static void _2B70(DesktopAlertWindow _2B6F, EventArgs ؿ)
	{
		_2B6C?.Invoke(_2B6F, ؿ);
	}

	internal static void _2B71(object _٠, AlertClosedEventArgs ؿ)
	{
		_2B6D?.Invoke(_٠, ؿ);
	}
}
