using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualButtonBase : VisualItem
{
	private bool _ClickAutoRepeat;

	private Timer _ClickTimer;

	private int _ClickRepeatInterval = 300;

	private bool _RenderDefaultBackground = true;

	private eShortcut _Shortcut;

	[DefaultValue(false)]
	public bool ClickAutoRepeat
	{
		get
		{
			return _ClickAutoRepeat;
		}
		set
		{
			_ClickAutoRepeat = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(300)]
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	public virtual int ClickRepeatInterval
	{
		get
		{
			return _ClickRepeatInterval;
		}
		set
		{
			_ClickRepeatInterval = value;
		}
	}

	public bool RenderDefaultBackground
	{
		get
		{
			return _RenderDefaultBackground;
		}
		set
		{
			if (_RenderDefaultBackground != value)
			{
				_RenderDefaultBackground = value;
				InvalidateRender();
			}
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
			_Shortcut = value;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		if ((int)e.Button == 1048576 && GetIsEnabled() && _ClickAutoRepeat)
		{
			if (_ClickTimer == null)
			{
				_ClickTimer = new Timer();
			}
			_ClickTimer.Interval = ClickRepeatInterval;
			_ClickTimer.Tick += ClickTimerTick;
			_ClickTimer.Start();
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ClickTimer != null)
		{
			Timer clickTimer = _ClickTimer;
			_ClickTimer = null;
			clickTimer.Stop();
			clickTimer.Tick -= ClickTimerTick;
			((Component)(object)clickTimer).Dispose();
		}
		base.OnMouseUp(e);
	}

	private void ClickTimerTick(object sender, EventArgs e)
	{
		ProcessClick();
		OnClickAutoRepeat();
	}

	protected virtual void OnClickAutoRepeat()
	{
	}

	protected override bool OnCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		if (_Shortcut != 0 && (int)keyData == (int)_Shortcut)
		{
			OnClick((EventArgs)new KeyEventArgs(keyData));
			return true;
		}
		return base.OnCmdKey(ref msg, keyData);
	}
}
