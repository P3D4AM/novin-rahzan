using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabItemStateColorTableConvertor))]
public class SuperTabItemStateColorTable : ICloneable
{
	private Color _Text = Color.Empty;

	private Color _InnerBorder = Color.Empty;

	private Color _OuterBorder = Color.Empty;

	private Color _CloseMarker = Color.Empty;

	private Color _SelectionMarker = Color.Empty;

	private SuperTabLinearGradientColorTable _Background;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the background colors.")]
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
					_Background.ColorTableChanged -= Background_ColorTableChanged;
				}
				_Background = value;
				if (_Background != null)
				{
					_Background.ColorTableChanged += Background_ColorTableChanged;
				}
				OnColorTableChanged();
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
	[NotifyParentProperty(true)]
	[Description("Indicates the inner border color.")]
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
	[NotifyParentProperty(true)]
	[Description("Indicates the Text color.")]
	public Color Text
	{
		get
		{
			return _Text;
		}
		set
		{
			if (_Text != value)
			{
				_Text = value;
				OnColorTableChanged();
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
			return _CloseMarker;
		}
		set
		{
			if (_CloseMarker != value)
			{
				_CloseMarker = value;
				OnColorTableChanged();
			}
		}
	}

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Indicates the Selection Marker color.")]
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
			if (_Text != Color.Empty)
			{
				return false;
			}
			if (_CloseMarker != Color.Empty)
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

	[Description("Event raised when the SuperTabItemStateColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabItemStateColorTable()
	{
		_Background = new SuperTabLinearGradientColorTable();
		_Background.ColorTableChanged += Background_ColorTableChanged;
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
		OuterBorder = Color.Empty;
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
		InnerBorder = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeText()
	{
		return !_Text.IsEmpty;
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
		return _CloseMarker != Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCloseMarker()
	{
		CloseMarker = Color.Empty;
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
		SelectionMarker = Color.Empty;
	}

	private void Background_ColorTableChanged(object sender, EventArgs e)
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
