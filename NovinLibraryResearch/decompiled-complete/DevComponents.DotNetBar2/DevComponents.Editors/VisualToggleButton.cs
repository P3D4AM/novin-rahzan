using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.Editors;

public class VisualToggleButton : VisualItem
{
	private EventHandler _0E65;

	private bool _ᚆ;

	private bool __171A;

	private bool _ᆿ;

	[DefaultValue(false)]
	public virtual bool Checked
	{
		get
		{
			return _ᚆ;
		}
		set
		{
			if (_ᚆ != value)
			{
				_ᚆ = value;
				InvalidateRender();
				OnCheckedChanged(new EventArgs());
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return __171A;
		}
		internal set
		{
			if (value != __171A)
			{
				__171A = value;
				InvalidateRender();
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		internal set
		{
			if (value != _ᆿ)
			{
				_ᆿ = value;
				InvalidateRender();
			}
		}
	}

	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public VisualToggleButton()
	{
		base.Focusable = true;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && GetIsEnabled())
		{
			ᢎ();
			IsMouseDown = true;
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			IsMouseDown = false;
		}
		base.OnMouseUp(e);
	}

	protected override void OnMouseEnter()
	{
		IsMouseOver = true;
		base.OnMouseEnter();
	}

	protected override void OnMouseLeave()
	{
		IsMouseOver = false;
		base.OnMouseLeave();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && GetIsEnabled())
		{
			ᢎ();
			e.Handled = true;
		}
		base.OnKeyDown(e);
	}

	private void ᢎ()
	{
		Checked = !Checked;
	}

	protected virtual void OnCheckedChanged(EventArgs e)
	{
		if (_0E65 != null)
		{
			_0E65(this, e);
		}
	}
}
