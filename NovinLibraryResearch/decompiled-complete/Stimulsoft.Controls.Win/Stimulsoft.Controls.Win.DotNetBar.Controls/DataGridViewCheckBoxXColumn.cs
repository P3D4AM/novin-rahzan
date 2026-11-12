using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.CheckBoxX.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewCheckBoxXColumn : DataGridViewColumn, IDataGridViewColumn
{
	private CheckBoxX _CheckBoxX;

	private int _ActiveRowIndex = -1;

	private int _CurrentRowIndex = -1;

	private int _DownRowIndex = -1;

	private bool _InCellCallBack;

	private Bitmap _CellBitmap;

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

	internal CheckBoxX CheckBoxX => _CheckBoxX;

	internal int CurrentRowIndex
	{
		get
		{
			return _CurrentRowIndex;
		}
		set
		{
			_CurrentRowIndex = value;
		}
	}

	internal int DownRowIndex
	{
		get
		{
			return _DownRowIndex;
		}
		set
		{
			_DownRowIndex = value;
		}
	}

	internal bool InCellCallBack
	{
		get
		{
			return _InCellCallBack;
		}
		set
		{
			_InCellCallBack = value;
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return _CheckBoxX.CheckBoxImageChecked;
		}
		set
		{
			_CheckBoxX.CheckBoxImageChecked = value;
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return _CheckBoxX.CheckBoxImageUnChecked;
		}
		set
		{
			_CheckBoxX.CheckBoxImageUnChecked = value;
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return _CheckBoxX.CheckBoxImageIndeterminate;
		}
		set
		{
			_CheckBoxX.CheckBoxImageIndeterminate = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eCheckBoxPosition.Left)]
	[Category("Appearance")]
	[Description("Indicates the check box position relative to the text.")]
	public eCheckBoxPosition CheckBoxPosition
	{
		get
		{
			return _CheckBoxX.CheckBoxPosition;
		}
		set
		{
			_CheckBoxX.CheckBoxPosition = value;
		}
	}

	[Browsable(false)]
	public virtual bool Checked
	{
		get
		{
			return _CheckBoxX.Checked;
		}
		set
		{
			_CheckBoxX.Checked = value;
		}
	}

	[Browsable(false)]
	public CheckState CheckState
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _CheckBoxX.CheckState;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_CheckBoxX.CheckState = value;
		}
	}

	[Browsable(false)]
	public object CheckValue
	{
		get
		{
			return _CheckBoxX.CheckValue;
		}
		set
		{
			_CheckBoxX.CheckValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Checked state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
	public object CheckValueChecked
	{
		get
		{
			return _CheckBoxX.CheckValueChecked;
		}
		set
		{
			_CheckBoxX.CheckValueChecked = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Indeterminate state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
	public object CheckValueIndeterminate
	{
		get
		{
			return _CheckBoxX.CheckValueIndeterminate;
		}
		set
		{
			_CheckBoxX.CheckValueIndeterminate = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Unchecked state value of the check box when CheckValue property is set to that value.  Note that in the designer, this value type is always a string.")]
	public object CheckValueUnchecked
	{
		get
		{
			return _CheckBoxX.CheckValueUnchecked;
		}
		set
		{
			_CheckBoxX.CheckValueUnchecked = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
	public bool ConsiderEmptyStringAsNull
	{
		get
		{
			return _CheckBoxX.ConsiderEmptyStringAsNull;
		}
		set
		{
			_CheckBoxX.ConsiderEmptyStringAsNull = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the control can respond to user interaction.")]
	public bool Enabled
	{
		get
		{
			return ((Control)_CheckBoxX).Enabled;
		}
		set
		{
			((Control)_CheckBoxX).Enabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _CheckBoxX.EnableMarkup;
		}
		set
		{
			_CheckBoxX.EnableMarkup = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the default Text to display.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public string Text
	{
		get
		{
			return ((Control)_CheckBoxX).Text;
		}
		set
		{
			((Control)_CheckBoxX).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates control text color.")]
	public Color TextColor
	{
		get
		{
			return _CheckBoxX.TextColor;
		}
		set
		{
			_CheckBoxX.TextColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return _CheckBoxX.TextVisible;
		}
		set
		{
			_CheckBoxX.TextVisible = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	public bool ThreeState
	{
		get
		{
			return _CheckBoxX.ThreeState;
		}
		set
		{
			_CheckBoxX.ThreeState = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a CheckBoxX Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when a CheckBoxX Cell is Clicked.")]
	public event EventHandler<EventArgs> Click;

	[Description("Occurs when a CheckBoxX Cell is DoubleClicked.")]
	public event EventHandler<EventArgs> DoubleClicked;

	public DataGridViewCheckBoxXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewCheckBoxXCell();
		_CheckBoxX = new CheckBoxX();
		((Control)_CheckBoxX).Visible = false;
		_CheckBoxX.CheckValueChecked = true;
		_CheckBoxX.CheckValueUnchecked = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_CheckBoxX.TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_CheckBoxX.CheckBoxItem.Click += CheckBoxItem_Click;
			_CheckBoxX.CheckBoxItem.DoubleClick += CheckBoxItem_DoubleClick;
		}
		else
		{
			_CheckBoxX.CheckBoxItem.Click -= CheckBoxItem_Click;
			_CheckBoxX.CheckBoxItem.DoubleClick -= CheckBoxItem_DoubleClick;
		}
	}

	private void CheckBoxItem_Click(object sender, EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(sender, e);
		}
	}

	private void CheckBoxItem_DoubleClick(object sender, EventArgs e)
	{
		if (this.DoubleClicked != null)
		{
			this.DoubleClicked(sender, e);
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
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = ((DataGridViewColumn)this).Clone() as DataGridViewCheckBoxXColumn;
		if (dataGridViewCheckBoxXColumn != null)
		{
			_CheckBoxX.CheckBoxItem.InternalCopyToItem(dataGridViewCheckBoxXColumn.CheckBoxX.CheckBoxItem);
			dataGridViewCheckBoxXColumn.CheckValue = CheckValue;
			dataGridViewCheckBoxXColumn.CheckValueChecked = CheckValueChecked;
			dataGridViewCheckBoxXColumn.CheckValueIndeterminate = CheckValueIndeterminate;
			dataGridViewCheckBoxXColumn.CheckValueUnchecked = CheckValueUnchecked;
			dataGridViewCheckBoxXColumn.ConsiderEmptyStringAsNull = ConsiderEmptyStringAsNull;
			dataGridViewCheckBoxXColumn.Enabled = Enabled;
			dataGridViewCheckBoxXColumn.Text = Text;
		}
		return dataGridViewCheckBoxXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
