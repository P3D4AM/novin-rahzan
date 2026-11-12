using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

[TypeConverter(typeof(SuperTabPanelItemColorTableConvertor))]
public class SuperTabPanelItemColorTable : ICloneable
{
	private Color _OuterBorder = Color.Empty;

	private Color _InnerBorder = Color.Empty;

	private SuperTabLinearGradientColorTable _Background;

	[Browsable(true)]
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
			return true;
		}
	}

	[Description("Event raised when the SuperTabPanelItemColorTable is changed")]
	public event EventHandler<EventArgs> ColorTableChanged;

	public SuperTabPanelItemColorTable()
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
		SuperTabPanelItemColorTable superTabPanelItemColorTable = new SuperTabPanelItemColorTable();
		superTabPanelItemColorTable.Background = (SuperTabLinearGradientColorTable)Background.Clone();
		superTabPanelItemColorTable.OuterBorder = OuterBorder;
		superTabPanelItemColorTable.InnerBorder = InnerBorder;
		return superTabPanelItemColorTable;
	}
}
