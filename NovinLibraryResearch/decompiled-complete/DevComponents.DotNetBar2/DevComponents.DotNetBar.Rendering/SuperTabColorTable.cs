using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabColorTableConvertor))]
public class SuperTabColorTable : IDisposable, ICloneable
{
	private EventHandler<EventArgs> ቇ;

	private Color _ኃ = Color.Empty;

	private Color _ኄ = Color.Empty;

	private SuperTabLinearGradientColorTable _ኅ;

	private SuperTabControlBoxStateColorTable _ኆ;

	private SuperTabControlBoxStateColorTable _ኇ;

	private SuperTabControlBoxStateColorTable _ኈ;

	private Color __1289 = Color.Empty;

	private Color _ኊ = Color.Empty;

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the Background color.")]
	[Category("Appearance")]
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
					_ኅ.ColorTableChanged -= ኋ;
				}
				_ኅ = value;
				if (_ኅ != null)
				{
					_ኅ.ColorTableChanged += ኋ;
				}
				ኌ();
			}
		}
	}

	[Category("Appearance")]
	[NotifyParentProperty(true)]
	[Description("Indicates the colors for the outer border.")]
	[Browsable(true)]
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

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates the colors for the inner border.")]
	[NotifyParentProperty(true)]
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

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the ControlBoxDefault colors.")]
	[Category("Appearance")]
	public SuperTabControlBoxStateColorTable ControlBoxDefault
	{
		get
		{
			return _ኆ;
		}
		set
		{
			if (!_ኆ.Equals(value))
			{
				if (_ኆ != null)
				{
					_ኆ.ColorTableChanged -= ኋ;
				}
				_ኆ = value;
				if (_ኆ != null)
				{
					_ኆ.ColorTableChanged += ኋ;
				}
				ኌ();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the ControlBoxMouseOver colors.")]
	[Category("Appearance")]
	public SuperTabControlBoxStateColorTable ControlBoxMouseOver
	{
		get
		{
			return _ኇ;
		}
		set
		{
			if (!_ኇ.Equals(value))
			{
				if (_ኇ != null)
				{
					_ኇ.ColorTableChanged -= ኋ;
				}
				_ኇ = value;
				if (_ኇ != null)
				{
					_ኇ.ColorTableChanged += ኋ;
				}
				ኌ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[Category("Appearance")]
	[Description("Indicates the ControlBoxPressed colors.")]
	[Browsable(true)]
	public SuperTabControlBoxStateColorTable ControlBoxPressed
	{
		get
		{
			return _ኈ;
		}
		set
		{
			if (!_ኈ.Equals(value))
			{
				if (_ኈ != null)
				{
					_ኈ.ColorTableChanged -= ኋ;
				}
				_ኈ = value;
				if (_ኈ != null)
				{
					_ኈ.ColorTableChanged += ኋ;
				}
				ኌ();
			}
		}
	}

	[NotifyParentProperty(true)]
	[Browsable(true)]
	[Description("Indicates the color for the InsertMarker.")]
	[Category("Appearance")]
	public Color InsertMarker
	{
		get
		{
			return __1289;
		}
		set
		{
			if (__1289 != value)
			{
				__1289 = value;
				ኌ();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates the color for the SelectionMarker.")]
	[NotifyParentProperty(true)]
	[Category("Appearance")]
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
			if (!_ኆ.IsEmpty)
			{
				return false;
			}
			if (!_ኇ.IsEmpty)
			{
				return false;
			}
			if (!_ኈ.IsEmpty)
			{
				return false;
			}
			if (__1289 != Color.Empty)
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

	[Description("Event raised when the SuperTabColorTable is changed")]
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

	public SuperTabColorTable()
	{
		_ኅ = new SuperTabLinearGradientColorTable();
		_ኅ.ColorTableChanged += ኋ;
		_ኆ = new SuperTabControlBoxStateColorTable();
		_ኆ.ColorTableChanged += ኋ;
		_ኇ = new SuperTabControlBoxStateColorTable();
		_ኇ.ColorTableChanged += ኋ;
		_ኈ = new SuperTabControlBoxStateColorTable();
		_ኈ.ColorTableChanged += ኋ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackground()
	{
		return !_ኅ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetBackground()
	{
		Background = new SuperTabLinearGradientColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetInnerBorder()
	{
		_ኄ = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlBoxDefault()
	{
		return !_ኆ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetControlBoxDefault()
	{
		ControlBoxDefault = new SuperTabControlBoxStateColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeControlBoxMouseOver()
	{
		return !_ኇ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetControlBoxMouseOver()
	{
		ControlBoxMouseOver = new SuperTabControlBoxStateColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeControlBoxPressed()
	{
		return !_ኈ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetControlBoxPressed()
	{
		ControlBoxPressed = new SuperTabControlBoxStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeInsertMarker()
	{
		return __1289 != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetInsertMarker()
	{
		__1289 = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectionMarker()
	{
		return _ኊ != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectionMarker()
	{
		_ኊ = Color.Empty;
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
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = (SuperTabLinearGradientColorTable)Background.Clone();
		superTabColorTable.OuterBorder = OuterBorder;
		superTabColorTable.InnerBorder = InnerBorder;
		superTabColorTable.ControlBoxDefault = (SuperTabControlBoxStateColorTable)ControlBoxDefault.Clone();
		superTabColorTable.ControlBoxMouseOver = (SuperTabControlBoxStateColorTable)ControlBoxMouseOver.Clone();
		superTabColorTable.ControlBoxPressed = (SuperTabControlBoxStateColorTable)ControlBoxPressed.Clone();
		superTabColorTable.InsertMarker = InsertMarker;
		superTabColorTable.SelectionMarker = SelectionMarker;
		return superTabColorTable;
	}

	public void Dispose()
	{
		Background = null;
		ControlBoxDefault = null;
		ControlBoxMouseOver = null;
		ControlBoxPressed = null;
	}
}
