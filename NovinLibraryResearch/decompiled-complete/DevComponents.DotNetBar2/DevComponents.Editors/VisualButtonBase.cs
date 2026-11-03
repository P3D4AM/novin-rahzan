using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors;

public class VisualButtonBase : VisualItem
{
	private bool _ᶛ;

	private Timer _ᶜ;

	private int _ᶝ = 300;

	private bool _ᶞ = true;

	private eShortcut _ᠾ;

	private string _ݩ = "";

	private ToolTip _ᶟ;

	[DefaultValue(false)]
	public bool ClickAutoRepeat
	{
		get
		{
			return _ᶛ;
		}
		set
		{
			_ᶛ = value;
		}
	}

	[DefaultValue(300)]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[Browsable(true)]
	[Category("Behavior")]
	public virtual int ClickRepeatInterval
	{
		get
		{
			return _ᶝ;
		}
		set
		{
			_ᶝ = value;
		}
	}

	public bool RenderDefaultBackground
	{
		get
		{
			return _ᶞ;
		}
		set
		{
			if (_ᶞ != value)
			{
				_ᶞ = value;
				InvalidateRender();
			}
		}
	}

	[Description("Indicates shortcut key which when pressed triggers button click event or its default function.")]
	[DefaultValue(eShortcut.None)]
	public eShortcut Shortcut
	{
		get
		{
			return _ᠾ;
		}
		set
		{
			_ᠾ = value;
		}
	}

	public string Tooltip
	{
		get
		{
			return _ݩ;
		}
		set
		{
			_ݩ = value;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		if ((int)e.Button == 1048576 && GetIsEnabled() && _ᶛ)
		{
			if (_ᶜ == null)
			{
				_ᶜ = new Timer();
			}
			_ᶜ.Interval = ClickRepeatInterval;
			_ᶜ.Tick += _0A55;
			_ᶜ.Start();
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		ᶠ();
		base.OnMouseUp(e);
	}

	protected override void OnMouseLeave()
	{
		ᶢ();
		ᶠ();
		base.OnMouseLeave();
	}

	private void ᶠ()
	{
		if (_ᶜ != null)
		{
			Timer ᶜ = _ᶜ;
			_ᶜ = null;
			ᶜ.Stop();
			ᶜ.Tick -= _0A55;
			((Component)(object)ᶜ).Dispose();
		}
	}

	private void _0A55(object _٠, EventArgs ؿ)
	{
		ᡝ();
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
		if (_ᠾ != 0 && (int)keyData == (int)_ᠾ)
		{
			OnClick((EventArgs)new KeyEventArgs(keyData));
			return true;
		}
		return base.OnCmdKey(ref msg, keyData);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		ᶡ();
		base.OnMouseHover(e);
	}

	private void ᶡ()
	{
		if (!string.IsNullOrEmpty(_ݩ))
		{
			if (_ᶟ == null)
			{
				_ᶟ = new ToolTip();
			}
			_ᶟ.Style = StyleManager.GetEffectiveStyle();
			_ᶟ.Text = _ݩ;
			_ᶟ.ShowToolTip();
		}
	}

	private void ᶢ()
	{
		if (_ᶟ == null)
		{
			return;
		}
		try
		{
			if (_ᶟ != null)
			{
				((Control)_ᶟ).Hide();
				((Component)(object)_ᶟ).Dispose();
				_ᶟ = null;
				ResetHover();
			}
		}
		catch
		{
		}
	}
}
