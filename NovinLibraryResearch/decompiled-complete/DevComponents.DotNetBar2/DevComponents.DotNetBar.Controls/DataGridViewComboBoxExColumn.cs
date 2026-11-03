using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ComboBoxEx.ico")]
[ComVisible(false)]
[ToolboxItem(false)]
public class DataGridViewComboBoxExColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private EventHandler<DrawItemEventArgs> _2B11;

	private EventHandler<DropDownChangeEventArgs> ᚮ;

	private EventHandler<MeasureItemEventArgs> _2B12;

	private ComboBoxEx __2B21;

	private Bitmap __2AFE;

	private CurrencyManager __2B22;

	private bool __2B23 = true;

	internal ComboBoxEx _2B1F => __2B21;

	internal CurrencyManager _2B20
	{
		get
		{
			return __2B22;
		}
		set
		{
			__2B22 = value;
		}
	}

	[Description("Gets or sets a custom Specialized.StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public AutoCompleteStringCollection AutoCompleteCustomSource
	{
		get
		{
			return ((ComboBox)__2B21).AutoCompleteCustomSource;
		}
		set
		{
			((ComboBox)__2B21).AutoCompleteCustomSource = value;
		}
	}

	[Description("Indicates how automatic completion works for the ComboBox.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	public AutoCompleteMode AutoCompleteMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)__2B21).AutoCompleteMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)__2B21).AutoCompleteMode = value;
		}
	}

	[Description("Indicates the source of complete strings used for automatic completion.")]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public AutoCompleteSource AutoCompleteSource
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)__2B21).AutoCompleteSource;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)__2B21).AutoCompleteSource = value;
		}
	}

	[DefaultValue(null)]
	[AttributeProvider(typeof(IListSource))]
	[Category("Data")]
	[Browsable(true)]
	[Description("Indicates the data source that populates the selections for the combo box.")]
	public object DataSource
	{
		get
		{
			return ((ComboBox)__2B21).DataSource;
		}
		set
		{
			((ComboBox)__2B21).DataSource = value;
			_2B20 = null;
		}
	}

	[Description("Indicates a string that specifies the property or column from which to retrieve strings for display in the combo box")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Category("Data")]
	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Browsable(true)]
	[DefaultValue("")]
	public string DisplayMember
	{
		get
		{
			return ((ListControl)__2B21).DisplayMember;
		}
		set
		{
			((ListControl)__2B21).DisplayMember = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether the control will be displayed for the current cell only.")]
	[Category("Appearance")]
	[Browsable(true)]
	public bool DisplayControlForCurrentCellOnly
	{
		get
		{
			return __2B23;
		}
		set
		{
			if (__2B23 != value)
			{
				__2B23 = value;
				((Control)__2B21).Invalidate();
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
			return ((ComboBox)__2B21).DrawMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)__2B21).DrawMode = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the style of the combo box.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)__2B21).DropDownStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)__2B21).DropDownStyle = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the height in pixels of the drop-down portion of the ComboBox.")]
	[DefaultValue(100)]
	public int DropDownHeight
	{
		get
		{
			return ((ComboBox)__2B21).DropDownHeight;
		}
		set
		{
			((ComboBox)__2B21).DropDownHeight = value;
		}
	}

	[Description("Indicates the width of the drop-down lists of the combo box")]
	[Category("Behavior")]
	[DefaultValue(1)]
	[Browsable(true)]
	public int DropDownWidth
	{
		get
		{
			return ((ComboBox)__2B21).DropDownWidth;
		}
		set
		{
			((ComboBox)__2B21).DropDownWidth = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether the control can respond to user interaction.")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B21).Enabled;
		}
		set
		{
			((Control)__2B21).Enabled = value;
		}
	}

	[Description("Indicates the flat style appearance of the column's cells")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Category("Appearance")]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)__2B21).FlatStyle;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)__2B21).FlatStyle = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether control displays focus cues when focused.")]
	[Category("Behavior")]
	public bool FocusCuesEnabled
	{
		get
		{
			return __2B21.FocusCuesEnabled;
		}
		set
		{
			__2B21.FocusCuesEnabled = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the format-specifier characters that indicate how a value is to be displayed.")]
	public string FormatString
	{
		get
		{
			return ((ListControl)__2B21).FormatString;
		}
		set
		{
			((ListControl)__2B21).FormatString = value;
		}
	}

	[Description("Indicates whether formatting is applied to the DisplayMember.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool FormattingEnabled
	{
		get
		{
			return ((ListControl)__2B21).FormattingEnabled;
		}
		set
		{
			((ListControl)__2B21).FormattingEnabled = value;
		}
	}

	[Description("The ImageList control used by Combo box to draw images.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(null)]
	public ImageList Images
	{
		get
		{
			return __2B21.Images;
		}
		set
		{
			__2B21.Images = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the Input Method Editor (IME) mode of the control.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)__2B21).ImeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B21).ImeMode = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether the control should resize to avoid showing partial items.")]
	[Browsable(true)]
	public bool IntegralHeight
	{
		get
		{
			return ((ComboBox)__2B21).IntegralHeight;
		}
		set
		{
			((ComboBox)__2B21).IntegralHeight = value;
		}
	}

	[Description("Indicates the appearance of the control.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public bool IsStandalone
	{
		get
		{
			return __2B21.IsStandalone;
		}
		set
		{
			__2B21.IsStandalone = value;
		}
	}

	[DefaultValue(12)]
	[Description("Indicates the height of an item in the combo box.")]
	[Browsable(true)]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)__2B21).ItemHeight;
		}
		set
		{
			((ComboBox)__2B21).ItemHeight = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates the collection of objects used as selections in the combo box")]
	[Category("Data")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public ObjectCollection Items => __2B21.Items;

	[DefaultValue(8)]
	[Description("Indicates the maximum number of items in the drop-down list of the cells in the column.")]
	[Browsable(true)]
	[Category("Behavior")]
	public int MaxDropDownItems
	{
		get
		{
			return ((ComboBox)__2B21).MaxDropDownItems;
		}
		set
		{
			((ComboBox)__2B21).MaxDropDownItems = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates the number of characters a user can type into the ComboBox.")]
	[Browsable(true)]
	public int MaxLength
	{
		get
		{
			return ((ComboBox)__2B21).MaxLength;
		}
		set
		{
			((ComboBox)__2B21).MaxLength = value;
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
			return ((Control)__2B21).RightToLeft;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)__2B21).RightToLeft = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the items in the combo box are sorted.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool Sorted
	{
		get
		{
			return ((ComboBox)__2B21).Sorted;
		}
		set
		{
			((ComboBox)__2B21).Sorted = value;
		}
	}

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Description("Determines the display of the item when shown.")]
	[Category("Appearance")]
	[Browsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return __2B21.Style;
		}
		set
		{
			__2B21.Style = value;
		}
	}

	[Description("Indicates the text associated with this control.")]
	[Browsable(true)]
	[DefaultValue("")]
	public string Text
	{
		get
		{
			return ((Control)__2B21).Text;
		}
		set
		{
			((Control)__2B21).Text = value;
		}
	}

	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Category("Data")]
	[Browsable(true)]
	[Description("Indicates a string that specifies the property or column from which to get values that correspond to the selections in the drop-down list.")]
	[DefaultValue("")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public string ValueMember
	{
		get
		{
			return ((ListControl)__2B21).ValueMember;
		}
		set
		{
			((ListControl)__2B21).ValueMember = value;
		}
	}

	[Description("Indicates watermark hiding behaviour.")]
	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Category("Behavior")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return __2B21.WatermarkBehavior;
		}
		set
		{
			__2B21.WatermarkBehavior = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text color.")]
	public Color WatermarkColor
	{
		get
		{
			return __2B21.WatermarkColor;
		}
		set
		{
			__2B21.WatermarkColor = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public virtual bool WatermarkEnabled
	{
		get
		{
			return __2B21.WatermarkEnabled;
		}
		set
		{
			__2B21.WatermarkEnabled = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Font WatermarkFont
	{
		get
		{
			return __2B21.WatermarkFont;
		}
		set
		{
			__2B21.WatermarkFont = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	public string WatermarkText
	{
		get
		{
			return __2B21.WatermarkText;
		}
		set
		{
			__2B21.WatermarkText = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a ComboBox Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint
	{
		add
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<BeforeCellPaintEventArgs> eventHandler = _2AF8;
			EventHandler<BeforeCellPaintEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<BeforeCellPaintEventArgs> value2 = (EventHandler<BeforeCellPaintEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2AF8, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when the ComboBox is clicked.")]
	public event EventHandler<EventArgs> Click
	{
		add
		{
			EventHandler<EventArgs> eventHandler = દ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = દ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref દ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a visual aspect of an owner-drawn ComboBox  changes.")]
	public event EventHandler<DrawItemEventArgs> DrawItem
	{
		add
		{
			EventHandler<DrawItemEventArgs> eventHandler = _2B11;
			EventHandler<DrawItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<DrawItemEventArgs> value2 = (EventHandler<DrawItemEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B11, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<DrawItemEventArgs> eventHandler = _2B11;
			EventHandler<DrawItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<DrawItemEventArgs> value2 = (EventHandler<DrawItemEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B11, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when drop down portion of the ComboBox is shown or hidden.")]
	public event EventHandler<DropDownChangeEventArgs> DropDownChange
	{
		add
		{
			EventHandler<DropDownChangeEventArgs> eventHandler = ᚮ;
			EventHandler<DropDownChangeEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<DropDownChangeEventArgs> value2 = (EventHandler<DropDownChangeEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<DropDownChangeEventArgs> eventHandler = ᚮ;
			EventHandler<DropDownChangeEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<DropDownChangeEventArgs> value2 = (EventHandler<DropDownChangeEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᚮ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs each time an owner-drawn ComboBox item needs to be drawn and when the sizes of the list items are determined.")]
	public event EventHandler<MeasureItemEventArgs> MeasureItem
	{
		add
		{
			EventHandler<MeasureItemEventArgs> eventHandler = _2B12;
			EventHandler<MeasureItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MeasureItemEventArgs> value2 = (EventHandler<MeasureItemEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B12, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<MeasureItemEventArgs> eventHandler = _2B12;
			EventHandler<MeasureItemEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<MeasureItemEventArgs> value2 = (EventHandler<MeasureItemEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B12, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public DataGridViewComboBoxExColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewComboBoxExCell();
		__2B21 = new ComboBoxEx();
		__2B21.ᚩ = false;
		((Control)__2B21).Visible = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeWatermarkColor()
	{
		return __2B21.ShouldSerializeWatermarkColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetWatermarkColor()
	{
		__2B21.ResetWatermarkColor();
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			((ListControl)__2B21).DataSourceChanged += _2B24;
		}
		else
		{
			((ListControl)__2B21).DataSourceChanged -= _2B24;
		}
	}

	private void _2B24(object _٠, EventArgs ؿ)
	{
		__2B22 = null;
		__2B21.Items.Clear();
		((ListControl)__2B21).DisplayMember = null;
		((ListControl)__2B21).ValueMember = null;
	}

	internal void _2B25(object _٠, EventArgs ؿ)
	{
		if (દ != null)
		{
			દ(_٠, ؿ);
		}
	}

	internal void _2B26(object _٠, DrawItemEventArgs ؿ)
	{
		if (_2B11 != null)
		{
			_2B11(_٠, ؿ);
		}
	}

	internal void _2B27(object _٠, DropDownChangeEventArgs ؿ)
	{
		if (ᚮ != null)
		{
			ᚮ(_٠, ؿ);
		}
	}

	internal void _2B28(object _٠, MeasureItemEventArgs ؿ)
	{
		if (_2B12 != null)
		{
			_2B12(_٠, ؿ);
		}
	}

	internal Bitmap _2B01(Rectangle _2AE9)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (__2AFE == null || ((Image)__2AFE).Width != _2AE9.Width || ((Image)__2AFE).Height < _2AE9.Height)
		{
			if (__2AFE != null)
			{
				((Image)__2AFE).Dispose();
			}
			__2AFE = new Bitmap(_2AE9.Width, _2AE9.Height);
		}
		return __2AFE;
	}

	internal void _2B02(int _2AEA, int _21A5)
	{
		if (_2AF8 != null)
		{
			_2AF8(this, new BeforeCellPaintEventArgs(_2AEA, _21A5));
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
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2B21).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
