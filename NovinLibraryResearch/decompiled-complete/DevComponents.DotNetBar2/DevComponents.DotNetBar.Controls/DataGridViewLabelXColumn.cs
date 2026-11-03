using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
[ToolboxBitmap(typeof(DataGridViewLabelXColumn), "Controls.LabelX.ico")]
public class DataGridViewLabelXColumn : DataGridViewButtonColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private LabelX __2B4E;

	private Bitmap __2AFE;

	private bool __2AFD;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	internal LabelX _2B4D => __2B4E;

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for the item's Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __2B4E.EnableMarkup;
		}
		set
		{
			__2B4E.EnableMarkup = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether control displays focus cues when focused.")]
	public bool FocusCuesEnabled
	{
		get
		{
			return __2B4E.FocusCuesEnabled;
		}
		set
		{
			__2B4E.FocusCuesEnabled = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Specifies border sides that are displayed.")]
	[DefaultValue(eBorderSide.All)]
	public eBorderSide BorderSide
	{
		get
		{
			return __2B4E.BorderSide;
		}
		set
		{
			__2B4E.BorderSide = value;
		}
	}

	[Description("Indicates the type of the border drawn around the label.")]
	[Browsable(false)]
	[DefaultValue(eBorderType.None)]
	[Category("Appearance")]
	public eBorderType BorderType
	{
		get
		{
			return __2B4E.BorderType;
		}
		set
		{
			__2B4E.BorderType = value;
		}
	}

	[DefaultValue(null)]
	[Description("The image that will be displayed on the face of the item.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Image Image
	{
		get
		{
			return __2B4E.Image;
		}
		set
		{
			__2B4E.Image = value;
		}
	}

	[DefaultValue(eImagePosition.Left)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[Browsable(true)]
	public eImagePosition ImagePosition
	{
		get
		{
			return __2B4E.ImagePosition;
		}
		set
		{
			__2B4E.ImagePosition = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates bottom padding in pixels.")]
	[Category("Layout")]
	[Browsable(true)]
	public int PaddingBottom
	{
		get
		{
			return __2B4E.PaddingBottom;
		}
		set
		{
			__2B4E.PaddingBottom = value;
		}
	}

	[Description("Indicates left padding in pixels.")]
	[Category("Layout")]
	[DefaultValue(0)]
	[Browsable(true)]
	public int PaddingLeft
	{
		get
		{
			return __2B4E.PaddingLeft;
		}
		set
		{
			__2B4E.PaddingLeft = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates top padding in pixels.")]
	[Browsable(true)]
	[Category("Layout")]
	public int PaddingTop
	{
		get
		{
			return __2B4E.PaddingTop;
		}
		set
		{
			__2B4E.PaddingTop = value;
		}
	}

	[Description("Indicates right padding in pixels.")]
	[Category("Layout")]
	[DefaultValue(0)]
	[Browsable(true)]
	public int PaddingRight
	{
		get
		{
			return __2B4E.PaddingRight;
		}
		set
		{
			__2B4E.PaddingRight = value;
		}
	}

	[Description("Indicates border line color when border is single line.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color SingleLineColor
	{
		get
		{
			return __2B4E.SingleLineColor;
		}
		set
		{
			__2B4E.SingleLineColor = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("The text contained in the item.")]
	public string Text
	{
		get
		{
			if (!__2AFD)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)__2B4E).Text;
		}
		set
		{
			if (__2AFD)
			{
				((Control)__2B4E).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Indicates the horizontal text alignment.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B4E.TextAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B4E.TextAlignment = value;
		}
	}

	[Category("Layout")]
	[Description("Indicates vertical text line alignment.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return __2B4E.TextLineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			__2B4E.TextLineAlignment = value;
		}
	}

	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	[Category("Appearance")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool UseMnemonic
	{
		get
		{
			return __2B4E.UseMnemonic;
		}
		set
		{
			__2B4E.UseMnemonic = value;
		}
	}

	[Description("Indicates whether text is displayed in multiple lines or one long line.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Style")]
	public bool WordWrap
	{
		get
		{
			return __2B4E.WordWrap;
		}
		set
		{
			__2B4E.WordWrap = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a LabelX Cell is painted.")]
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

	[Description("Occurs when a LabelX Cell is Clicked.")]
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

	public DataGridViewLabelXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewLabelXCell();
		__2B4E = new LabelX();
		((Control)__2B4E).CreateControl();
		((Control)__2B4E).Visible = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return __2B4E.ShouldSerializeSingleLineColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		__2B4E.ResetSingleLineColor();
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			__2B4E._2288.Click += _2B4F;
		}
		else
		{
			__2B4E._2288.Click -= _2B4F;
		}
	}

	private void _2B4F(object _٠, EventArgs ؿ)
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewLabelXColumn;
		if (dataGridViewLabelXColumn != null)
		{
			__2B4E._2288._0EBA(dataGridViewLabelXColumn._2B4D._2288);
			dataGridViewLabelXColumn.FocusCuesEnabled = FocusCuesEnabled;
			dataGridViewLabelXColumn.Text = Text;
			dataGridViewLabelXColumn.UseMnemonic = UseMnemonic;
		}
		return dataGridViewLabelXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2B4E).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
