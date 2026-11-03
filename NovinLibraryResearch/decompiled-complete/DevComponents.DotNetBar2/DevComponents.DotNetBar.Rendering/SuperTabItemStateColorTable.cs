using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemStateColorTableConvertor))]
public class SuperTabItemStateColorTable : ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private Color _ኚ = Color.Empty;

	private Color _ኄ = Color.Empty;

	private Color _ኃ = Color.Empty;

	private Color _ኛ = Color.Empty;

	private Color _ኊ = Color.Empty;

	private SuperTabLinearGradientColorTable _ኅ;

	[Description("Indicates the background colors.")]
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
	[Description("Indicates the outer border color.")]
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

	[Description("Indicates the inner border color.")]
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

	[Description("Indicates the Text color.")]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	public Color Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			if (_ኚ != value)
			{
				_ኚ = value;
				ኌ();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Close Marker color.")]
	public Color CloseMarker
	{
		get
		{
			return _ኛ;
		}
		set
		{
			if (_ኛ != value)
			{
				_ኛ = value;
				ኌ();
			}
		}
	}

	[Description("Indicates the Selection Marker color.")]
	[NotifyParentProperty(true)]
	[Browsable(true)]
	public Color SelectionMarker
	{
		get
		{
			return _ኊ;
		}
		set
		{
			if (_ኊ != value)
			{
				_ኊ = value;
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
			if (_ኚ != Color.Empty)
			{
				return false;
			}
			if (_ኛ != Color.Empty)
			{
				return false;
			}
			if (_ኊ != Color.Empty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabItemStateColorTable is changed")]
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

	public SuperTabItemStateColorTable()
	{
		_ኅ = new SuperTabLinearGradientColorTable();
		_ኅ.ColorTableChanged += ኜ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
		OuterBorder = Color.Empty;
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
		InnerBorder = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeText()
	{
		return !_ኚ.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetText()
	{
		Text = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCloseMarker()
	{
		return _ኛ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCloseMarker()
	{
		CloseMarker = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeSelectionMarker()
	{
		return _ኊ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectionMarker()
	{
		SelectionMarker = Color.Empty;
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
		SuperTabItemStateColorTable superTabItemStateColorTable = new SuperTabItemStateColorTable();
		superTabItemStateColorTable.Background = (SuperTabLinearGradientColorTable)Background.Clone();
		superTabItemStateColorTable.Text = Text;
		superTabItemStateColorTable.OuterBorder = OuterBorder;
		superTabItemStateColorTable.InnerBorder = InnerBorder;
		superTabItemStateColorTable.CloseMarker = CloseMarker;
		superTabItemStateColorTable.SelectionMarker = SelectionMarker;
		return superTabItemStateColorTable;
	}
}
