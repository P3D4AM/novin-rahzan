using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.Slider.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewSliderColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private Slider _Slider;

	private Bitmap _CellBitmap;

	private int _ActiveRowIndex = -1;

	private bool _BindingComplete;

	internal int ActiveRowIndex
	{
		get
		{
			return _ActiveRowIndex;
		}
		set
		{
			_ActiveRowIndex = value;
		}
	}

	internal bool BindingComplete
	{
		get
		{
			return _BindingComplete;
		}
		set
		{
			_BindingComplete = value;
		}
	}

	internal Slider Slider => _Slider;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	public bool Enabled
	{
		get
		{
			return ((Control)_Slider).Enabled;
		}
		set
		{
			((Control)_Slider).Enabled = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _Slider.EnableMarkup;
		}
		set
		{
			_Slider.EnableMarkup = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eSliderLabelPosition.Left)]
	[Category("Layout")]
	[Description("Indicates text label position in relationship to the slider")]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return _Slider.LabelPosition;
		}
		set
		{
			_Slider.LabelPosition = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets whether the label text is displayed.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool LabelVisible
	{
		get
		{
			return _Slider.LabelVisible;
		}
		set
		{
			_Slider.LabelVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(38)]
	[Category("Layout")]
	[Description("Indicates width of the label part of the item in pixels.")]
	public int LabelWidth
	{
		get
		{
			return _Slider.LabelWidth;
		}
		set
		{
			_Slider.LabelWidth = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	public int Maximum
	{
		get
		{
			return _Slider.Maximum;
		}
		set
		{
			_Slider.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return _Slider.Minimum;
		}
		set
		{
			_Slider.Minimum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(1)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	public int Step
	{
		get
		{
			return _Slider.Step;
		}
		set
		{
			_Slider.Step = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	public string Text
	{
		get
		{
			return ((Control)_Slider).Text;
		}
		set
		{
			((Control)_Slider).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color of the label text.")]
	public Color TextColor
	{
		get
		{
			return _Slider.TextColor;
		}
		set
		{
			_Slider.TextColor = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	public virtual bool TrackMarker
	{
		get
		{
			return _Slider.TrackMarker;
		}
		set
		{
			_Slider.TrackMarker = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates the current position of the slider.")]
	public int Value
	{
		get
		{
			return _Slider.Value;
		}
		set
		{
			_Slider.Value = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a Slider Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when a Slider Cell is Clicked.")]
	public event EventHandler<EventArgs> Click;

	public DataGridViewSliderColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewSliderCell();
		_Slider = new Slider();
		((Control)_Slider).Visible = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_Slider.TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		_Slider.TextColor = Color.Empty;
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_Slider.SliderItem.Click += SliderItem_Click;
			_Slider.SliderItem.ValueChanged += SliderItem_ValueChanged;
		}
		else
		{
			_Slider.SliderItem.Click -= SliderItem_Click;
			_Slider.SliderItem.ValueChanged -= SliderItem_ValueChanged;
		}
	}

	private void SliderItem_Click(object sender, EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(sender, e);
		}
	}

	private void SliderItem_ValueChanged(object sender, EventArgs e)
	{
		if ((_Slider.SliderItem.MouseDownPart == eSliderPart.IncreaseButton || _Slider.SliderItem.MouseDownPart == eSliderPart.DecreaseButton) && ((DataGridViewElement)this).DataGridView.Rows[ActiveRowIndex].Cells[((DataGridViewBand)this).Index] is DataGridViewSliderCell dataGridViewSliderCell)
		{
			int sliderValue = dataGridViewSliderCell.GetSliderValue(((DataGridViewCell)dataGridViewSliderCell).Value);
			if (sliderValue != _Slider.SliderItem.Value)
			{
				((DataGridViewCell)dataGridViewSliderCell).Value = _Slider.SliderItem.Value;
				dataGridViewSliderCell.RefreshSlider(((DataGridViewBand)this).Index, ActiveRowIndex);
			}
		}
	}

	internal Bitmap GetCellBitmap(Rectangle cellBounds)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (_CellBitmap == null || ((Image)_CellBitmap).Width != cellBounds.Width || ((Image)_CellBitmap).Height < cellBounds.Height)
		{
			if (_CellBitmap != null)
			{
				((Image)_CellBitmap).Dispose();
			}
			_CellBitmap = new Bitmap(cellBounds.Width, cellBounds.Height);
		}
		return _CellBitmap;
	}

	internal void OnBeforeCellPaint(int rowIndex, int columnIndex)
	{
		if (this.BeforeCellPaint != null)
		{
			this.BeforeCellPaint(this, new BeforeCellPaintEventArgs(rowIndex, columnIndex));
		}
	}

	public override object Clone()
	{
		DataGridViewSliderColumn dataGridViewSliderColumn = ((DataGridViewColumn)this).Clone() as DataGridViewSliderColumn;
		if (dataGridViewSliderColumn != null)
		{
			dataGridViewSliderColumn.Enabled = Enabled;
		}
		return dataGridViewSliderColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
