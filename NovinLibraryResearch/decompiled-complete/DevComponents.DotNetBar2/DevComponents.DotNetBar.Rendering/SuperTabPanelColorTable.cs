using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabPanelColorTableConvertor))]
public class SuperTabPanelColorTable : ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private SuperTabPanelItemColorTable _ኑ;

	private SuperTabPanelItemColorTable _খ;

	private SuperTabPanelItemColorTable _ঙ;

	private SuperTabPanelItemColorTable _গ;

	[Browsable(true)]
	[Description("Indicates the Default tab panel color settings.")]
	[NotifyParentProperty(true)]
	public SuperTabPanelItemColorTable Default
	{
		get
		{
			return _ኑ;
		}
		set
		{
			ኒ(ref _ኑ, value);
		}
	}

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the Left Aligned tab panel color settings.")]
	public SuperTabPanelItemColorTable Left
	{
		get
		{
			return _খ;
		}
		set
		{
			ኒ(ref _খ, value);
		}
	}

	[Description("Indicates the Bottom Aligned tab panel color settings.")]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	public SuperTabPanelItemColorTable Bottom
	{
		get
		{
			return _ঙ;
		}
		set
		{
			ኒ(ref _ঙ, value);
		}
	}

	[Description("Indicates the Right Aligned tab panel color settings.")]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	public SuperTabPanelItemColorTable Right
	{
		get
		{
			return _গ;
		}
		set
		{
			ኒ(ref _গ, value);
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_ኑ.IsEmpty && _খ.IsEmpty && _ঙ.IsEmpty)
			{
				return _গ.IsEmpty;
			}
			return false;
		}
	}

	[Description("Event raised when the SuperTabPanelColorTable is changed")]
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

	public SuperTabPanelColorTable()
	{
		_ኑ = new SuperTabPanelItemColorTable();
		_ኑ.ColorTableChanged += ኝ;
		_খ = new SuperTabPanelItemColorTable();
		_খ.ColorTableChanged += ኝ;
		_ঙ = new SuperTabPanelItemColorTable();
		_ঙ.ColorTableChanged += ኝ;
		_গ = new SuperTabPanelItemColorTable();
		_গ.ColorTableChanged += ኝ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeDefault()
	{
		return !_ኑ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetDefault()
	{
		Default = new SuperTabPanelItemColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLeft()
	{
		return !_খ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetLeft()
	{
		Left = new SuperTabPanelItemColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBottom()
	{
		return !_ঙ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBottom()
	{
		Bottom = new SuperTabPanelItemColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRight()
	{
		return !_গ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetRight()
	{
		Right = new SuperTabPanelItemColorTable();
	}

	private void ኒ(ref SuperTabPanelItemColorTable ና, SuperTabPanelItemColorTable ኔ)
	{
		if (ና != null)
		{
			ና.ColorTableChanged -= ኝ;
		}
		ና = ኔ;
		if (ና != null)
		{
			ና.ColorTableChanged += ኝ;
		}
		ኌ();
	}

	private void ኝ(object _٠, EventArgs ؿ)
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
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default = (SuperTabPanelItemColorTable)Default.Clone();
		superTabPanelColorTable.Left = (SuperTabPanelItemColorTable)Left.Clone();
		superTabPanelColorTable.Bottom = (SuperTabPanelItemColorTable)Bottom.Clone();
		superTabPanelColorTable.Right = (SuperTabPanelItemColorTable)Right.Clone();
		return superTabPanelColorTable;
	}
}
