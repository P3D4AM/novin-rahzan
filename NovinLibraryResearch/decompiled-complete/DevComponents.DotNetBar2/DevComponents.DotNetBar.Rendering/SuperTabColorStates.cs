using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemColorTableConvertor))]
public class SuperTabColorStates
{
	private EventHandler<EventArgs> ቇ;

	private SuperTabItemStateColorTable _ን;

	private SuperTabItemStateColorTable _ኖ;

	private SuperTabItemStateColorTable _ኗ;

	private SuperTabItemStateColorTable _ኘ;

	private SuperTabItemStateColorTable _ኙ;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is not selected, and the mouse is not over it.")]
	public SuperTabItemStateColorTable Normal
	{
		get
		{
			return _ን;
		}
		set
		{
			ኒ(ref _ን, value);
		}
	}

	[Description("Indicates the tab colors when the tab is selected, but the mouse is not over the it.")]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	public SuperTabItemStateColorTable Selected
	{
		get
		{
			return _ኖ;
		}
		set
		{
			ኒ(ref _ኖ, value);
		}
	}

	[Browsable(true)]
	[Description("Indicates the tab colors when the tab is not selected, but the mouse is over it.")]
	[NotifyParentProperty(true)]
	public SuperTabItemStateColorTable MouseOver
	{
		get
		{
			return _ኗ;
		}
		set
		{
			ኒ(ref _ኗ, value);
		}
	}

	[Browsable(true)]
	[Description("Indicates the tab colors when the tab is selected, and the mouse is over it.")]
	[NotifyParentProperty(true)]
	public SuperTabItemStateColorTable SelectedMouseOver
	{
		get
		{
			return _ኘ;
		}
		set
		{
			ኒ(ref _ኘ, value);
		}
	}

	[NotifyParentProperty(true)]
	[Description("Indicates the tab colors when the tab is disabled, is not selected, and the mouse is not over it.")]
	[Browsable(true)]
	public SuperTabItemStateColorTable Disabled
	{
		get
		{
			return _ኙ;
		}
		set
		{
			ኒ(ref _ኙ, value);
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!_ን.IsEmpty)
			{
				return false;
			}
			if (!_ኖ.IsEmpty)
			{
				return false;
			}
			if (!_ኗ.IsEmpty)
			{
				return false;
			}
			if (!_ኘ.IsEmpty)
			{
				return false;
			}
			if (!_ኙ.IsEmpty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabColorStates is changed")]
	public event EventHandler<EventArgs> ColorTableChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ቇ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ቇ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabColorStates()
	{
		_ን = new SuperTabItemStateColorTable();
		_ን.ColorTableChanged += ኋ;
		_ኖ = new SuperTabItemStateColorTable();
		_ኖ.ColorTableChanged += ኋ;
		_ኗ = new SuperTabItemStateColorTable();
		_ኗ.ColorTableChanged += ኋ;
		_ኘ = new SuperTabItemStateColorTable();
		_ኘ.ColorTableChanged += ኋ;
		_ኙ = new SuperTabItemStateColorTable();
		_ኙ.ColorTableChanged += ኋ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeNormal()
	{
		return !_ን.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNormal()
	{
		Normal = new SuperTabItemStateColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeSelected()
	{
		return !_ኖ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelected()
	{
		Selected = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMouseOver()
	{
		return !_ኗ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetMouseOver()
	{
		MouseOver = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedMouseOver()
	{
		return !_ኘ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedMouseOver()
	{
		SelectedMouseOver = new SuperTabItemStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabled()
	{
		return !_ኙ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabled()
	{
		Disabled = new SuperTabItemStateColorTable();
	}

	private void ኒ(ref SuperTabItemStateColorTable ና, SuperTabItemStateColorTable ኔ)
	{
		if (ና != null)
		{
			ና.ColorTableChanged -= ኋ;
		}
		ና = ኔ;
		if (ና != null)
		{
			ና.ColorTableChanged += ኋ;
		}
		ኌ();
	}

	private void ኋ(object _٠, EventArgs ؿ)
	{
		ኌ();
	}

	private void ኌ()
	{
		if (ቇ != null)
		{
			ቇ(this, EventArgs.Empty);
		}
	}

	public object Clone()
	{
		SuperTabColorStates superTabColorStates = new SuperTabColorStates();
		superTabColorStates.Normal = (SuperTabItemStateColorTable)Normal.Clone();
		superTabColorStates.Selected = (SuperTabItemStateColorTable)Selected.Clone();
		superTabColorStates.MouseOver = (SuperTabItemStateColorTable)MouseOver.Clone();
		superTabColorStates.SelectedMouseOver = (SuperTabItemStateColorTable)SelectedMouseOver.Clone();
		superTabColorStates.Disabled = (SuperTabItemStateColorTable)Disabled.Clone();
		return superTabColorStates;
	}

	public void Dispose()
	{
		Normal = null;
		MouseOver = null;
		SelectedMouseOver = null;
		Selected = null;
		Disabled = null;
	}
}
