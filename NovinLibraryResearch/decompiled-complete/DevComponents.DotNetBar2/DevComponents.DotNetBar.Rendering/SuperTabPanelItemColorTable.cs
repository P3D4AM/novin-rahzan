using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabPanelItemColorTableConvertor))]
public class SuperTabPanelItemColorTable : ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private Color _ኃ = Color.Empty;

	private Color _ኄ = Color.Empty;

	private SuperTabLinearGradientColorTable _ኅ;

	[NotifyParentProperty(true)]
	[Browsable(true)]
	public SuperTabLinearGradientColorTable Background
	{
		get
		{
			return _ኅ;
		}
		set
		{
			if (!_ኅ.Equals(value))
			{
				if (_ኅ != null)
				{
					_ኅ.ColorTableChanged -= ኜ;
				}
				_ኅ = value;
				if (_ኅ != null)
				{
					_ኅ.ColorTableChanged += ኜ;
				}
				ኌ();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color OuterBorder
	{
		get
		{
			return _ኃ;
		}
		set
		{
			if (_ኃ != value)
			{
				_ኃ = value;
				ኌ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[Browsable(true)]
	public Color InnerBorder
	{
		get
		{
			return _ኄ;
		}
		set
		{
			if (_ኄ != value)
			{
				_ኄ = value;
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
			if (_ኃ != Color.Empty)
			{
				return false;
			}
			if (_ኄ != Color.Empty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabPanelItemColorTable is changed")]
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

	public SuperTabPanelItemColorTable()
	{
		_ኅ = new SuperTabLinearGradientColorTable();
		_ኅ.ColorTableChanged += ኜ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeBackground()
	{
		return !_ኅ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackground()
	{
		Background = new SuperTabLinearGradientColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeOuterBorder()
	{
		return _ኃ != Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetOuterBorder()
	{
		_ኃ = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeInnerBorder()
	{
		return _ኄ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetInnerBorder()
	{
		_ኄ = Color.Empty;
	}

	private void ኜ(object _٠, EventArgs ؿ)
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
		SuperTabPanelItemColorTable superTabPanelItemColorTable = new SuperTabPanelItemColorTable();
		superTabPanelItemColorTable.Background = (SuperTabLinearGradientColorTable)Background.Clone();
		superTabPanelItemColorTable.OuterBorder = OuterBorder;
		superTabPanelItemColorTable.InnerBorder = InnerBorder;
		return superTabPanelItemColorTable;
	}
}
