using System;
using System.ComponentModel;
using System.Threading;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class Spacing
{
	private EventHandler _2EF3;

	private int _2EF4;

	private int _2EF5;

	private int _2EF6;

	private int _2EF7;

	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the left side.")]
	[Browsable(true)]
	[DevCoSerialize]
	public int Left
	{
		get
		{
			return _2EF4;
		}
		set
		{
			_2EF4 = value;
			_2EF8();
		}
	}

	[DevCoSerialize]
	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the right side.")]
	public int Right
	{
		get
		{
			return _2EF5;
		}
		set
		{
			_2EF5 = value;
			_2EF8();
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the amount of the space on the top.")]
	[DevCoSerialize]
	public int Top
	{
		get
		{
			return _2EF6;
		}
		set
		{
			_2EF6 = value;
			_2EF8();
		}
	}

	[Description("Indicates the amount of the space on the bottom.")]
	[DefaultValue(0)]
	[Browsable(true)]
	[DevCoSerialize]
	public int Bottom
	{
		get
		{
			return _2EF7;
		}
		set
		{
			_2EF7 = value;
			_2EF8();
		}
	}

	[Browsable(false)]
	public int Horizontal => Left + Right;

	[Browsable(false)]
	public int Vertical => Top + Bottom;

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_2EF4 == 0 && _2EF5 == 0 && _2EF6 == 0)
			{
				return _2EF7 == 0;
			}
			return false;
		}
	}

	public event EventHandler SpacingChanged
	{
		add
		{
			EventHandler eventHandler = _2EF3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2EF3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2EF3;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2EF3, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private void _2EF8()
	{
		if (_2EF3 != null)
		{
			_2EF3(this, new EventArgs());
		}
	}
}
