using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabControlBoxStateColorTableConvertor))]
public class SuperTabControlBoxStateColorTable : ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private Color _ኅ;

	private Color __128F = Color.Empty;

	private Color _ነ = Color.Empty;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Background
	{
		get
		{
			return _ኅ;
		}
		set
		{
			if (_ኅ != value)
			{
				_ኅ = value;
				ኌ();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Border
	{
		get
		{
			return __128F;
		}
		set
		{
			if (__128F != value)
			{
				__128F = value;
				ኌ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[Browsable(true)]
	public Color Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (_ነ != value)
			{
				_ነ = value;
				ኌ();
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!_ኅ.IsEmpty)
			{
				return false;
			}
			if (__128F != Color.Empty)
			{
				return false;
			}
			if (_ነ != Color.Empty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabControlBoxStateColorTable is changed")]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackground()
	{
		return _ኅ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackground()
	{
		_ኅ = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorder()
	{
		return __128F != Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBorder()
	{
		__128F = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImage()
	{
		return _ነ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImage()
	{
		_ነ = Color.Empty;
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
		SuperTabControlBoxStateColorTable superTabControlBoxStateColorTable = new SuperTabControlBoxStateColorTable();
		superTabControlBoxStateColorTable.Background = Background;
		superTabControlBoxStateColorTable.Border = Border;
		superTabControlBoxStateColorTable.Image = Image;
		return superTabControlBoxStateColorTable;
	}
}
