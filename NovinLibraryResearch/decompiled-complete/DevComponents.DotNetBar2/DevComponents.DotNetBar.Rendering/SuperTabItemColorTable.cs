using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemColorTableConvertor))]
public class SuperTabItemColorTable : IDisposable, ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private SuperTabColorStates _ኑ;

	private SuperTabColorStates _খ;

	private SuperTabColorStates _ঙ;

	private SuperTabColorStates _গ;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Default tab color settings.")]
	public SuperTabColorStates Default
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

	[Description("Indicates the Left Aligned tab color settings.")]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	public SuperTabColorStates Left
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

	[Description("Indicates the Bottom Aligned tab color settings.")]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	public SuperTabColorStates Bottom
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

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the Right Aligned tab color settings.")]
	public SuperTabColorStates Right
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

	[Description("Event raised when the SuperTabItemColorTable is changed")]
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

	public SuperTabItemColorTable()
	{
		_ኑ = new SuperTabColorStates();
		_ኑ.ColorTableChanged += ኋ;
		_খ = new SuperTabColorStates();
		_খ.ColorTableChanged += ኋ;
		_ঙ = new SuperTabColorStates();
		_ঙ.ColorTableChanged += ኋ;
		_গ = new SuperTabColorStates();
		_গ.ColorTableChanged += ኋ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDefault()
	{
		return !_ኑ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDefault()
	{
		Default = new SuperTabColorStates();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLeft()
	{
		return !_খ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLeft()
	{
		Left = new SuperTabColorStates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBottom()
	{
		return !_ঙ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBottom()
	{
		Bottom = new SuperTabColorStates();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeRight()
	{
		return !_গ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRight()
	{
		Right = new SuperTabColorStates();
	}

	private void ኒ(ref SuperTabColorStates ና, SuperTabColorStates ኔ)
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
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default = (SuperTabColorStates)Default.Clone();
		superTabItemColorTable.Left = (SuperTabColorStates)Left.Clone();
		superTabItemColorTable.Bottom = (SuperTabColorStates)Bottom.Clone();
		superTabItemColorTable.Right = (SuperTabColorStates)Right.Clone();
		return superTabItemColorTable;
	}

	public void Dispose()
	{
		Default = null;
		Left = null;
		Bottom = null;
		Right = null;
	}
}
