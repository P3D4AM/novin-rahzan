using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ComboBoxEx.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewComboBoxExColumn : DataGridViewTextBoxColumn, IDataGridViewColumn
{
	private ComboBoxEx _ComboBoxEx;

	private Bitmap _CellBitmap;

	private CurrencyManager _CurrencyManager;

	private bool _DisplayControlForCurrentCellOnly = true;

	internal ComboBoxEx ComboBoxEx => _ComboBoxEx;

	internal CurrencyManager CurrencyManager
	{
		get
		{
			return _CurrencyManager;
		}
		set
		{
			_CurrencyManager = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Gets or sets a custom Specialized.StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
	public AutoCompleteStringCollection AutoCompleteCustomSource
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).AutoCompleteCustomSource;
		}
		set
		{
			((ComboBox)_ComboBoxEx).AutoCompleteCustomSource = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates how automatic completion works for the ComboBox.")]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)_ComboBoxEx).AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)_ComboBoxEx).AutoCompleteMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the source of complete strings used for automatic completion.")]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)_ComboBoxEx).AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)_ComboBoxEx).AutoCompleteSource = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[AttributeProvider(typeof(IListSource))]
	[Description("Indicates the data source that populates the selections for the combo box.")]
	public object DataSource
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).DataSource;
		}
		set
		{
			((ComboBox)_ComboBoxEx).DataSource = value;
			CurrencyManager = null;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Data")]
	[Description("Indicates a string that specifies the property or column from which to retrieve strings for display in the combo box")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string DisplayMember
	{
		get
		{
			return ((ListControl)_ComboBoxEx).DisplayMember;
		}
		set
		{
			((ListControl)_ComboBoxEx).DisplayMember = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	public bool DisplayControlForCurrentCellOnly
	{
		get
		{
			return _DisplayControlForCurrentCellOnly;
		}
		set
		{
			if (_DisplayControlForCurrentCellOnly != value)
			{
				_DisplayControlForCurrentCellOnly = value;
				((Control)_ComboBoxEx).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates whether user code or operating system code will handle drawing of elements in the list.")]
	public DrawMode DrawMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)_ComboBoxEx).DrawMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)_ComboBoxEx).DrawMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the style of the combo box.")]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)_ComboBoxEx).DropDownStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)_ComboBoxEx).DropDownStyle = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(100)]
	[Description("Indicates the height in pixels of the drop-down portion of the ComboBox.")]
	public int DropDownHeight
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).DropDownHeight;
		}
		set
		{
			((ComboBox)_ComboBoxEx).DropDownHeight = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(1)]
	[Description("Indicates the width of the drop-down lists of the combo box")]
	public int DropDownWidth
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).DropDownWidth;
		}
		set
		{
			((ComboBox)_ComboBoxEx).DropDownWidth = value;
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
			return ((Control)_ComboBoxEx).Enabled;
		}
		set
		{
			((Control)_ComboBoxEx).Enabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the flat style appearance of the column's cells")]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)_ComboBoxEx).FlatStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)_ComboBoxEx).FlatStyle = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public bool FocusCuesEnabled
	{
		get
		{
			return _ComboBoxEx.FocusCuesEnabled;
		}
		set
		{
			_ComboBoxEx.FocusCuesEnabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the format-specifier characters that indicate how a value is to be displayed.")]
	public string FormatString
	{
		get
		{
			return ((ListControl)_ComboBoxEx).FormatString;
		}
		set
		{
			((ListControl)_ComboBoxEx).FormatString = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether formatting is applied to the DisplayMember.")]
	public bool FormattingEnabled
	{
		get
		{
			return ((ListControl)_ComboBoxEx).FormattingEnabled;
		}
		set
		{
			((ListControl)_ComboBoxEx).FormattingEnabled = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(null)]
	[Description("The ImageList control used by Combo box to draw images.")]
	public ImageList Images
	{
		get
		{
			return _ComboBoxEx.Images;
		}
		set
		{
			_ComboBoxEx.Images = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)_ComboBoxEx).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_ComboBoxEx).ImeMode = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether the control should resize to avoid showing partial items.")]
	public bool IntegralHeight
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).IntegralHeight;
		}
		set
		{
			((ComboBox)_ComboBoxEx).IntegralHeight = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates the appearance of the control.")]
	public bool IsStandalone
	{
		get
		{
			return _ComboBoxEx.IsStandalone;
		}
		set
		{
			_ComboBoxEx.IsStandalone = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(12)]
	[Description("Indicates the height of an item in the combo box.")]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).ItemHeight;
		}
		set
		{
			((ComboBox)_ComboBoxEx).ItemHeight = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[Description("Indicates the collection of objects used as selections in the combo box")]
	[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ObjectCollection Items => _ComboBoxEx.Items;

	[Browsable(true)]
	[DefaultValue(8)]
	[Category("Behavior")]
	[Description("Indicates the maximum number of items in the drop-down list of the cells in the column.")]
	public int MaxDropDownItems
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).MaxDropDownItems;
		}
		set
		{
			((ComboBox)_ComboBoxEx).MaxDropDownItems = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Indicates the number of characters a user can type into the ComboBox.")]
	public int MaxLength
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).MaxLength;
		}
		set
		{
			((ComboBox)_ComboBoxEx).MaxLength = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates control's elements are aligned to support locales using right-to-left fonts.")]
	public RightToLeft RightToLeft
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)_ComboBoxEx).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)_ComboBoxEx).RightToLeft = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the items in the combo box are sorted.")]
	public bool Sorted
	{
		get
		{
			return ((ComboBox)_ComboBoxEx).Sorted;
		}
		set
		{
			((ComboBox)_ComboBoxEx).Sorted = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Description("Determines the display of the item when shown.")]
	public eDotNetBarStyle Style
	{
		get
		{
			return _ComboBoxEx.Style;
		}
		set
		{
			_ComboBoxEx.Style = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the text associated with this control.")]
	public string Text
	{
		get
		{
			return ((Control)_ComboBoxEx).Text;
		}
		set
		{
			((Control)_ComboBoxEx).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue("")]
	[Description("Indicates a string that specifies the property or column from which to get values that correspond to the selections in the drop-down list.")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=10.9.0.8, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string ValueMember
	{
		get
		{
			return ((ListControl)_ComboBoxEx).ValueMember;
		}
		set
		{
			((ListControl)_ComboBoxEx).ValueMember = value;
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	[Description("Indicates watermark hiding behaviour.")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return _ComboBoxEx.WatermarkBehavior;
		}
		set
		{
			_ComboBoxEx.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return _ComboBoxEx.WatermarkColor;
		}
		set
		{
			_ComboBoxEx.WatermarkColor = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return _ComboBoxEx.WatermarkEnabled;
		}
		set
		{
			_ComboBoxEx.WatermarkEnabled = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates watermark font.")]
	public Font WatermarkFont
	{
		get
		{
			return _ComboBoxEx.WatermarkFont;
		}
		set
		{
			_ComboBoxEx.WatermarkFont = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string WatermarkText
	{
		get
		{
			return _ComboBoxEx.WatermarkText;
		}
		set
		{
			_ComboBoxEx.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a ComboBox Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when the ComboBox is clicked.")]
	public event EventHandler<EventArgs> Click;

	[Description("Occurs when a visual aspect of an owner-drawn ComboBox  changes.")]
	public event EventHandler<DrawItemEventArgs> DrawItem;

	[Description("Occurs when drop down portion of the ComboBox is shown or hidden.")]
	public event EventHandler<DropDownChangeEventArgs> DropDownChange;

	[Description("Occurs each time an owner-drawn ComboBox item needs to be drawn and when the sizes of the list items are determined.")]
	public event EventHandler<MeasureItemEventArgs> MeasureItem;

	public DataGridViewComboBoxExColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewComboBoxExCell();
		_ComboBoxEx = new ComboBoxEx();
		_ComboBoxEx.RenderBorder = false;
		((Control)_ComboBoxEx).Visible = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return _ComboBoxEx.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		_ComboBoxEx.ResetWatermarkColor();
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			((ListControl)_ComboBoxEx).DataSourceChanged += ComboBoxEx_DataSourceChanged;
		}
		else
		{
			((ListControl)_ComboBoxEx).DataSourceChanged -= ComboBoxEx_DataSourceChanged;
		}
	}

	private void ComboBoxEx_DataSourceChanged(object sender, EventArgs e)
	{
		_CurrencyManager = null;
		_ComboBoxEx.Items.Clear();
		((ListControl)_ComboBoxEx).DisplayMember = null;
		((ListControl)_ComboBoxEx).ValueMember = null;
	}

	internal void DoComboBoxEx_Click(object sender, EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(sender, e);
		}
	}

	internal void DoComboBoxEx_DrawItem(object sender, DrawItemEventArgs e)
	{
		if (this.DrawItem != null)
		{
			this.DrawItem(sender, e);
		}
	}

	internal void DoComboBoxEx_DropDownChange(object sender, DropDownChangeEventArgs e)
	{
		if (this.DropDownChange != null)
		{
			this.DropDownChange(sender, e);
		}
	}

	internal void DoComboBoxEx_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (this.MeasureItem != null)
		{
			this.MeasureItem(sender, e);
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

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DataGridViewComboBoxExColumn { Name=");
		stringBuilder.Append(((DataGridViewColumn)this).Name);
		stringBuilder.Append(", Index=");
		stringBuilder.Append(((DataGridViewBand)this).Index.ToString(CultureInfo.CurrentCulture));
		stringBuilder.Append(" }");
		return stringBuilder.ToString();
	}

	public override object Clone()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewComboBoxExColumn dataGridViewComboBoxExColumn = ((DataGridViewColumn)this).Clone() as DataGridViewComboBoxExColumn;
		if (dataGridViewComboBoxExColumn != null)
		{
			dataGridViewComboBoxExColumn.AutoCompleteCustomSource = AutoCompleteCustomSource;
			dataGridViewComboBoxExColumn.AutoCompleteMode = AutoCompleteMode;
			dataGridViewComboBoxExColumn.AutoCompleteSource = AutoCompleteSource;
			dataGridViewComboBoxExColumn.DrawMode = DrawMode;
			dataGridViewComboBoxExColumn.DropDownHeight = DropDownHeight;
			dataGridViewComboBoxExColumn.DropDownStyle = DropDownStyle;
			dataGridViewComboBoxExColumn.Enabled = Enabled;
			dataGridViewComboBoxExColumn.FlatStyle = FlatStyle;
			dataGridViewComboBoxExColumn.FocusCuesEnabled = FocusCuesEnabled;
			dataGridViewComboBoxExColumn.FormatString = FormatString;
			dataGridViewComboBoxExColumn.FormattingEnabled = FormattingEnabled;
			dataGridViewComboBoxExColumn.Images = Images;
			dataGridViewComboBoxExColumn.ImeMode = ImeMode;
			dataGridViewComboBoxExColumn.IntegralHeight = IntegralHeight;
			dataGridViewComboBoxExColumn.IsStandalone = IsStandalone;
			dataGridViewComboBoxExColumn.ItemHeight = ItemHeight;
			dataGridViewComboBoxExColumn.MaxDropDownItems = MaxDropDownItems;
			dataGridViewComboBoxExColumn.MaxLength = MaxLength;
			dataGridViewComboBoxExColumn.RightToLeft = RightToLeft;
			dataGridViewComboBoxExColumn.Style = Style;
			dataGridViewComboBoxExColumn.Text = Text;
			dataGridViewComboBoxExColumn.WatermarkBehavior = WatermarkBehavior;
			dataGridViewComboBoxExColumn.WatermarkColor = WatermarkColor;
			dataGridViewComboBoxExColumn.WatermarkEnabled = WatermarkEnabled;
			dataGridViewComboBoxExColumn.WatermarkFont = WatermarkFont;
			dataGridViewComboBoxExColumn.WatermarkText = WatermarkText;
			dataGridViewComboBoxExColumn.DataSource = DataSource;
			dataGridViewComboBoxExColumn.DisplayMember = DisplayMember;
			dataGridViewComboBoxExColumn.ValueMember = ValueMember;
			if (dataGridViewComboBoxExColumn.DataSource == null)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					dataGridViewComboBoxExColumn.Items.Add(Items[i]);
				}
			}
			dataGridViewComboBoxExColumn.DisplayControlForCurrentCellOnly = DisplayControlForCurrentCellOnly;
		}
		return dataGridViewComboBoxExColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
