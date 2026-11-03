using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.CheckBoxX.ico")]
[ComVisible(false)]
[ToolboxItem(false)]
public class DataGridViewCheckBoxXColumn : DataGridViewColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private EventHandler<EventArgs> _2B0C;

	private CheckBoxX __2B0D;

	private int __2AFA = -1;

	private int __2AFB = -1;

	private int __2B0E = -1;

	private bool __2AFD;

	private Bitmap __2AFE;

	internal int _2AF3
	{
		get
		{
			return __2AFA;
		}
		set
		{
			__2AFA = value;
		}
	}

	internal CheckBoxX _2B0A => __2B0D;

	internal int _2AF5
	{
		get
		{
			return __2AFB;
		}
		set
		{
			__2AFB = value;
		}
	}

	internal int _2B0B
	{
		get
		{
			return __2B0E;
		}
		set
		{
			__2B0E = value;
		}
	}

	internal bool _2AF7
	{
		get
		{
			return __2AFD;
		}
		set
		{
			__2AFD = value;
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	[Category("CheckBox Images")]
	[DefaultValue(null)]
	public Image CheckBoxImageChecked
	{
		get
		{
			return __2B0D.CheckBoxImageChecked;
		}
		set
		{
			__2B0D.CheckBoxImageChecked = value;
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	[Category("CheckBox Images")]
	[DefaultValue(null)]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return __2B0D.CheckBoxImageUnChecked;
		}
		set
		{
			__2B0D.CheckBoxImageUnChecked = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	[Category("CheckBox Images")]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return __2B0D.CheckBoxImageIndeterminate;
		}
		set
		{
			__2B0D.CheckBoxImageIndeterminate = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(eCheckBoxPosition.Left)]
	[Description("Indicates the check box position relative to the text.")]
	public eCheckBoxPosition CheckBoxPosition
	{
		get
		{
			return __2B0D.CheckBoxPosition;
		}
		set
		{
			__2B0D.CheckBoxPosition = value;
		}
	}

	[Browsable(false)]
	public virtual bool Checked
	{
		get
		{
			return __2B0D.Checked;
		}
		set
		{
			__2B0D.Checked = value;
		}
	}

	[Browsable(false)]
	public CheckState CheckState
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B0D.CheckState;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B0D.CheckState = value;
		}
	}

	[Browsable(false)]
	public object CheckValue
	{
		get
		{
			return __2B0D.CheckValue;
		}
		set
		{
			__2B0D.CheckValue = value;
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Represents the Checked state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
	public object CheckValueChecked
	{
		get
		{
			return __2B0D.CheckValueChecked;
		}
		set
		{
			__2B0D.CheckValueChecked = value;
		}
	}

	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Indeterminate state value of the check box when CheckValue property is set to that value. Note that in the designer, this value type is always a string.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public object CheckValueIndeterminate
	{
		get
		{
			return __2B0D.CheckValueIndeterminate;
		}
		set
		{
			__2B0D.CheckValueIndeterminate = value;
		}
	}

	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(false)]
	[Description("Represents the Unchecked state value of the check box when CheckValue property is set to that value.  Note that in the designer, this value type is always a string.")]
	[Browsable(true)]
	[Category("Behavior")]
	public object CheckValueUnchecked
	{
		get
		{
			return __2B0D.CheckValueUnchecked;
		}
		set
		{
			__2B0D.CheckValueUnchecked = value;
		}
	}

	[Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	public bool ConsiderEmptyStringAsNull
	{
		get
		{
			return __2B0D.ConsiderEmptyStringAsNull;
		}
		set
		{
			__2B0D.ConsiderEmptyStringAsNull = value;
		}
	}

	[Description("Indicates whether the control can respond to user interaction.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B0D).Enabled;
		}
		set
		{
			((Control)__2B0D).Enabled = value;
		}
	}

	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool EnableMarkup
	{
		get
		{
			return __2B0D.EnableMarkup;
		}
		set
		{
			__2B0D.EnableMarkup = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[Description("Indicates the default Text to display.")]
	public string Text
	{
		get
		{
			return ((Control)__2B0D).Text;
		}
		set
		{
			((Control)__2B0D).Text = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates control text color.")]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return __2B0D.TextColor;
		}
		set
		{
			__2B0D.TextColor = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return __2B0D.TextVisible;
		}
		set
		{
			__2B0D.TextVisible = value;
		}
	}

	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool ThreeState
	{
		get
		{
			return __2B0D.ThreeState;
		}
		set
		{
			__2B0D.ThreeState = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a CheckBoxX Cell is painted.")]
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

	[Description("Occurs when a CheckBoxX Cell is Clicked.")]
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

	[Description("Occurs when a CheckBoxX Cell is DoubleClicked.")]
	public event EventHandler<EventArgs> DoubleClicked
	{
		add
		{
			EventHandler<EventArgs> eventHandler = _2B0C;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B0C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = _2B0C;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B0C, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public DataGridViewCheckBoxXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewCheckBoxXCell();
		__2B0D = new CheckBoxX();
		((Control)__2B0D).Visible = false;
		__2B0D.CheckValueChecked = true;
		__2B0D.CheckValueUnchecked = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__2B0D.TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			__2B0D._23F5.Click += _2B0F;
			__2B0D._23F5.DoubleClick += _2B10;
		}
		else
		{
			__2B0D._23F5.Click -= _2B0F;
			__2B0D._23F5.DoubleClick -= _2B10;
		}
	}

	private void _2B0F(object _٠, EventArgs ؿ)
	{
		if (દ != null)
		{
			દ(_٠, ؿ);
		}
	}

	private void _2B10(object _٠, EventArgs ؿ)
	{
		if (_2B0C != null)
		{
			_2B0C(_٠, ؿ);
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

	public override object Clone()
	{
		DataGridViewCheckBoxXColumn dataGridViewCheckBoxXColumn = ((DataGridViewColumn)this).Clone() as DataGridViewCheckBoxXColumn;
		if (dataGridViewCheckBoxXColumn != null)
		{
			__2B0D._23F5._0EBA(dataGridViewCheckBoxXColumn._2B0A._23F5);
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
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2B0D).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
