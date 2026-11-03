using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ProgressBarX.ico")]
[ToolboxItem(false)]
public class DataGridViewProgressBarXColumn : DataGridViewButtonColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private ProgressBarX __2B55;

	private Bitmap __2AFE;

	private bool __2AFD;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((DataGridViewButtonColumn)this).FlatStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((DataGridViewButtonColumn)this).FlatStyle = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool UseColumnTextForButtonValue
	{
		get
		{
			return ((DataGridViewButtonColumn)this).UseColumnTextForButtonValue;
		}
		set
		{
			((DataGridViewButtonColumn)this).UseColumnTextForButtonValue = value;
		}
	}

	internal bool _2B4C
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

	internal ProgressBarX _2B54 => __2B55;

	[Description("Gets or sets the color of the progress chunk.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color ChunkColor
	{
		get
		{
			return __2B55.ChunkColor;
		}
		set
		{
			__2B55.ChunkColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the target gradient color of the progress chunk.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public Color ChunkColor2
	{
		get
		{
			return __2B55.ChunkColor2;
		}
		set
		{
			__2B55.ChunkColor2 = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the gradient angle of the progress chunk.")]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Browsable(true)]
	public int ChunkGradientAngle
	{
		get
		{
			return __2B55.ChunkGradientAngle;
		}
		set
		{
			__2B55.ChunkGradientAngle = value;
		}
	}

	[DefaultValue(eProgressBarItemColor.Normal)]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DevCoBrowsable(false)]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return __2B55.ColorTable;
		}
		set
		{
			__2B55.ColorTable = value;
		}
	}

	[Description("Indicates the maximum value of the range of the control.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return __2B55.Maximum;
		}
		set
		{
			__2B55.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return __2B55.Minimum;
		}
		set
		{
			__2B55.Minimum = value;
		}
	}

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[Browsable(true)]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return __2B55.Style;
		}
		set
		{
			__2B55.Style = value;
		}
	}

	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates the default Text to display on the Progress Bar.")]
	[Category("Appearance")]
	public string Text
	{
		get
		{
			if (!__2AFD)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)__2B55).Text;
		}
		set
		{
			if (__2AFD)
			{
				((Control)__2B55).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Browsable(true)]
	public bool TextVisible
	{
		get
		{
			return __2B55.TextVisible;
		}
		set
		{
			__2B55.TextVisible = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[DevCoBrowsable(true)]
	[Description("Indicates the current position of the progress bar.")]
	[Browsable(true)]
	public int Value
	{
		get
		{
			return __2B55.Value;
		}
		set
		{
			__2B55.Value = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a ProgressBarX Cell is painted.")]
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

	[Description("Occurs when a ProgressBarX Cell is Clicked.")]
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

	public DataGridViewProgressBarXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewProgressBarXCell();
		__2B55 = new ProgressBarX();
		((Control)__2B55).Visible = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !__2B55.ChunkColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		__2B55.ChunkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !__2B55.ChunkColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		__2B55.ChunkColor2 = Color.Empty;
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			__2B55._2B57.Click += _2B56;
		}
		else
		{
			__2B55._2B57.Click -= _2B56;
		}
	}

	private void _2B56(object _٠, EventArgs ؿ)
	{
		if (દ != null)
		{
			દ(_٠, ؿ);
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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewProgressBarXColumn;
		if (dataGridViewProgressBarXColumn != null)
		{
			__2B55._2B57._0EBA(dataGridViewProgressBarXColumn._2B54._2B57);
		}
		return dataGridViewProgressBarXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2B55).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
