using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.Slider.ico")]
[ToolboxItem(false)]
public class DataGridViewSliderColumn : DataGridViewTextBoxColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private Slider __2B60;

	private Bitmap __2AFE;

	private int __2AFA = -1;

	private bool __2B61;

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

	internal bool _2B5E
	{
		get
		{
			return __2B61;
		}
		set
		{
			__2B61 = value;
		}
	}

	internal Slider _2B5F => __2B60;

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether the control can respond to user interaction.")]
	[Category("Behavior")]
	public bool Enabled
	{
		get
		{
			return ((Control)__2B60).Enabled;
		}
		set
		{
			((Control)__2B60).Enabled = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __2B60.EnableMarkup;
		}
		set
		{
			__2B60.EnableMarkup = value;
		}
	}

	[DefaultValue(eSliderLabelPosition.Left)]
	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates text label position in relationship to the slider")]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return __2B60.LabelPosition;
		}
		set
		{
			__2B60.LabelPosition = value;
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets whether the label text is displayed.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool LabelVisible
	{
		get
		{
			return __2B60.LabelVisible;
		}
		set
		{
			__2B60.LabelVisible = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates width of the label part of the item in pixels.")]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[DefaultValue(38)]
	public int LabelWidth
	{
		get
		{
			return __2B60.LabelWidth;
		}
		set
		{
			__2B60.LabelWidth = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(100)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	public int Maximum
	{
		get
		{
			return __2B60.Maximum;
		}
		set
		{
			__2B60.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return __2B60.Minimum;
		}
		set
		{
			__2B60.Minimum = value;
		}
	}

	[DefaultValue(1)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public int Step
	{
		get
		{
			return __2B60.Step;
		}
		set
		{
			__2B60.Step = value;
		}
	}

	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Description("The text contained in the item.")]
	[Browsable(false)]
	[Category("Appearance")]
	public string Text
	{
		get
		{
			return ((Control)__2B60).Text;
		}
		set
		{
			((Control)__2B60).Text = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates color of the label text.")]
	public Color TextColor
	{
		get
		{
			return __2B60.TextColor;
		}
		set
		{
			__2B60.TextColor = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	[Category("Appearance")]
	public virtual bool TrackMarker
	{
		get
		{
			return __2B60.TrackMarker;
		}
		set
		{
			__2B60.TrackMarker = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates the current position of the slider.")]
	public int Value
	{
		get
		{
			return __2B60.Value;
		}
		set
		{
			__2B60.Value = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a Slider Cell is painted.")]
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

	[Description("Occurs when a Slider Cell is Clicked.")]
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

	public DataGridViewSliderColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewSliderCell();
		__2B60 = new Slider();
		((Control)__2B60).Visible = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__2B60.TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		__2B60.TextColor = Color.Empty;
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			__2B60._251F.Click += _2B62;
			__2B60._251F.ValueChanged += _2B63;
		}
		else
		{
			__2B60._251F.Click -= _2B62;
			__2B60._251F.ValueChanged -= _2B63;
		}
	}

	private void _2B62(object _٠, EventArgs ؿ)
	{
		if (દ != null)
		{
			દ(_٠, ؿ);
		}
	}

	private void _2B63(object _٠, EventArgs ؿ)
	{
		if ((__2B60._251F.MouseDownPart == eSliderPart.IncreaseButton || __2B60._251F.MouseDownPart == eSliderPart.DecreaseButton) && ((DataGridViewElement)this).DataGridView.Rows[_2AF3].Cells[((DataGridViewBand)this).Index] is DataGridViewSliderCell dataGridViewSliderCell)
		{
			int num = dataGridViewSliderCell._2B5C(((DataGridViewCell)dataGridViewSliderCell).Value);
			if (num != __2B60._251F.Value)
			{
				((DataGridViewCell)dataGridViewSliderCell).Value = __2B60._251F.Value;
				dataGridViewSliderCell._2B5D(((DataGridViewBand)this).Index, _2AF3);
			}
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
		DataGridViewSliderColumn dataGridViewSliderColumn = ((DataGridViewColumn)this).Clone() as DataGridViewSliderColumn;
		if (dataGridViewSliderColumn != null)
		{
			__2B60._251F._0EBA(dataGridViewSliderColumn._2B5F._251F);
			dataGridViewSliderColumn.Enabled = Enabled;
		}
		return dataGridViewSliderColumn;
	}

	protected override void Dispose(bool disposing)
	{
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2B60).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
