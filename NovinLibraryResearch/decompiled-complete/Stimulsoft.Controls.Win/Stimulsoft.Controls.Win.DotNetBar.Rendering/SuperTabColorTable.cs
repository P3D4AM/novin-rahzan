using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabColorTableConvertor))]
public class SuperTabColorTable : ICloneable, IDisposable
{
	private Color _OuterBorder = Color.Empty;

	private Color _InnerBorder = Color.Empty;

	private SuperTabLinearGradientColorTable _Background;

	private SuperTabControlBoxStateColorTable _ControlBoxDefault;

	private SuperTabControlBoxStateColorTable _ControlBoxMouseOver;

	private SuperTabControlBoxStateColorTable _ControlBoxPressed;

	private Color _InsertMarker = Color.Empty;

	private Color _SelectionMarker = Color.Empty;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the Background color.")]
	[NotifyParentProperty(true)]
	public SuperTabLinearGradientColorTable Background
	{
		get
		{
			return _Background;
		}
		set
		{
			if (!_Background.Equals(value))
			{
				if (_Background != null)
				{
					_Background.ColorTableChanged -= SctColorTableChanged;
				}
				_Background = value;
				if (_Background != null)
				{
					_Background.ColorTableChanged += SctColorTableChanged;
				}
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the colors for the outer border.")]
	[NotifyParentProperty(true)]
	public Color OuterBorder
	{
		get
		{
			return _OuterBorder;
		}
		set
		{
			if (_OuterBorder != value)
			{
				_OuterBorder = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the colors for the inner border.")]
	[NotifyParentProperty(true)]
	public Color InnerBorder
	{
		get
		{
			return _InnerBorder;
		}
		set
		{
			if (_InnerBorder != value)
			{
				_InnerBorder = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the ControlBoxDefault colors.")]
	[NotifyParentProperty(true)]
	public SuperTabControlBoxStateColorTable ControlBoxDefault
	{
		get
		{
			return _ControlBoxDefault;
		}
		set
		{
			if (!_ControlBoxDefault.Equals(value))
			{
				if (_ControlBoxDefault != null)
				{
					_ControlBoxDefault.ColorTableChanged -= SctColorTableChanged;
				}
				_ControlBoxDefault = value;
				if (_ControlBoxDefault != null)
				{
					_ControlBoxDefault.ColorTableChanged += SctColorTableChanged;
				}
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the ControlBoxMouseOver colors.")]
	[NotifyParentProperty(true)]
	public SuperTabControlBoxStateColorTable ControlBoxMouseOver
	{
		get
		{
			return _ControlBoxMouseOver;
		}
		set
		{
			if (!_ControlBoxMouseOver.Equals(value))
			{
				if (_ControlBoxMouseOver != null)
				{
					_ControlBoxMouseOver.ColorTableChanged -= SctColorTableChanged;
				}
				_ControlBoxMouseOver = value;
				if (_ControlBoxMouseOver != null)
				{
					_ControlBoxMouseOver.ColorTableChanged += SctColorTableChanged;
				}
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the ControlBoxPressed colors.")]
	[NotifyParentProperty(true)]
	public SuperTabControlBoxStateColorTable ControlBoxPressed
	{
		get
		{
			return _ControlBoxPressed;
		}
		set
		{
			if (!_ControlBoxPressed.Equals(value))
			{
				if (_ControlBoxPressed != null)
				{
					_ControlBoxPressed.ColorTableChanged -= SctColorTableChanged;
				}
				_ControlBoxPressed = value;
				if (_ControlBoxPressed != null)
				{
					_ControlBoxPressed.ColorTableChanged += SctColorTableChanged;
				}
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the color for the InsertMarker.")]
	[NotifyParentProperty(true)]
	public Color InsertMarker
	{
		get
		{
			return _InsertMarker;
		}
		set
		{
			if (_InsertMarker != value)
			{
				_InsertMarker = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the color for the SelectionMarker.")]
	[NotifyParentProperty(true)]
	public Color SelectionMarker
	{
		get
		{
			return _SelectionMarker;
		}
		set
		{
			if (_SelectionMarker != value)
			{
				_SelectionMarker = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (!_Background.IsEmpty)
			{
				return false;
			}
			if (_OuterBorder != Color.Empty)
			{
				return false;
			}
			if (_InnerBorder != Color.Empty)
			{
				return false;
			}
			if (!_ControlBoxDefault.IsEmpty)
			{
				return false;
			}
			if (!_ControlBoxMouseOver.IsEmpty)
			{
				return false;
			}
			if (!_ControlBoxPressed.IsEmpty)
			{
				return false;
			}
			if (_InsertMarker != Color.Empty)
			{
				return false;
			}
			if (_SelectionMarker != Color.Empty)
			{
				return false;
			}
			return true;
		}
	}

	[Description("Event raised when the SuperTabColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabColorTable()
	{
		_Background = new SuperTabLinearGradientColorTable();
		_Background.ColorTableChanged += SctColorTableChanged;
		_ControlBoxDefault = new SuperTabControlBoxStateColorTable();
		_ControlBoxDefault.ColorTableChanged += SctColorTableChanged;
		_ControlBoxMouseOver = new SuperTabControlBoxStateColorTable();
		_ControlBoxMouseOver.ColorTableChanged += SctColorTableChanged;
		_ControlBoxPressed = new SuperTabControlBoxStateColorTable();
		_ControlBoxPressed.ColorTableChanged += SctColorTableChanged;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackground()
	{
		return !_Background.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackground()
	{
		Background = new SuperTabLinearGradientColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOuterBorder()
	{
		return _OuterBorder != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOuterBorder()
	{
		_OuterBorder = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeInnerBorder()
	{
		return _InnerBorder != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetInnerBorder()
	{
		_InnerBorder = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlBoxDefault()
	{
		return !_ControlBoxDefault.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetControlBoxDefault()
	{
		ControlBoxDefault = new SuperTabControlBoxStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlBoxMouseOver()
	{
		return !_ControlBoxMouseOver.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetControlBoxMouseOver()
	{
		ControlBoxMouseOver = new SuperTabControlBoxStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeControlBoxPressed()
	{
		return !_ControlBoxPressed.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetControlBoxPressed()
	{
		ControlBoxPressed = new SuperTabControlBoxStateColorTable();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeInsertMarker()
	{
		return _InsertMarker != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetInsertMarker()
	{
		_InsertMarker = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectionMarker()
	{
		return _SelectionMarker != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectionMarker()
	{
		_SelectionMarker = Color.Empty;
	}

	private void SctColorTableChanged(object sender, EventArgs e)
	{
		OnColorTableChanged();
	}

	private void OnColorTableChanged()
	{
		if (this.ColorTableChanged != null)
		{
			this.ColorTableChanged(this, EventArgs.Empty);
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
