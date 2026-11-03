using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "ButtonX.ButtonX.ico")]
[ToolboxItem(false)]
public class DataGridViewButtonXColumn : DataGridViewButtonColumn, _2273
{
	private EventHandler<BeforeCellPaintEventArgs> _2AF8;

	private EventHandler<EventArgs> દ;

	private ButtonX __2AF9;

	private int __2AFA = -1;

	private int __2AFB = -1;

	private bool __2AFC;

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

	internal ButtonX _2AF4 => __2AF9;

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

	internal bool _2AF6
	{
		get
		{
			return __2AFC;
		}
		set
		{
			__2AFC = value;
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

	[Category("Behavior")]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool AutoCheckOnClick
	{
		get
		{
			return __2AF9.AutoCheckOnClick;
		}
		set
		{
			__2AF9.AutoCheckOnClick = value;
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return __2AF9.AutoExpandOnClick;
		}
		set
		{
			__2AF9.AutoExpandOnClick = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	public virtual bool Checked
	{
		get
		{
			return __2AF9.Checked;
		}
		set
		{
			__2AF9.Checked = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorScheme ColorScheme
	{
		get
		{
			return __2AF9.ColorScheme;
		}
		set
		{
			__2AF9.ColorScheme = value;
		}
	}

	[DevCoBrowsable(false)]
	[DefaultValue(eButtonColor.BlueWithBackground)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	[NotifyParentProperty(true)]
	public eButtonColor ColorTable
	{
		get
		{
			return __2AF9.ColorTable;
		}
		set
		{
			__2AF9.ColorTable = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	[DefaultValue("")]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	public string CustomColorName
	{
		get
		{
			return __2AF9.CustomColorName;
		}
		set
		{
			__2AF9.CustomColorName = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Description("The image that will be displayed when item is disabled.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Image DisabledImage
	{
		get
		{
			return __2AF9.DisabledImage;
		}
		set
		{
			__2AF9.DisabledImage = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __2AF9.EnableMarkup;
		}
		set
		{
			__2AF9.EnableMarkup = value;
		}
	}

	[Description("Indicates whether the control can respond to user interaction.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ((Control)__2AF9).Enabled;
		}
		set
		{
			((Control)__2AF9).Enabled = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(eHotTrackingStyle.Default)]
	[Category("Appearance")]
	[Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	[Browsable(true)]
	public eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return __2AF9.HotTrackingStyle;
		}
		set
		{
			__2AF9.HotTrackingStyle = value;
		}
	}

	[Browsable(true)]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image HoverImage
	{
		get
		{
			return __2AF9.HoverImage;
		}
		set
		{
			__2AF9.HoverImage = value;
		}
	}

	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the button.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return __2AF9.Image;
		}
		set
		{
			__2AF9.Image = value;
		}
	}

	[Browsable(true)]
	public Size ImageFixedSize
	{
		get
		{
			return __2AF9.ImageFixedSize;
		}
		set
		{
			__2AF9.ImageFixedSize = value;
		}
	}

	[DefaultValue(eImagePosition.Left)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eImagePosition ImagePosition
	{
		get
		{
			return __2AF9.ImagePosition;
		}
		set
		{
			__2AF9.ImagePosition = value;
		}
	}

	[Category("Layout")]
	[Description("Indicates amount of spacing between button image if specified and text.")]
	[Browsable(true)]
	[DefaultValue(0)]
	public int ImageTextSpacing
	{
		get
		{
			return __2AF9.ImageTextSpacing;
		}
		set
		{
			__2AF9.ImageTextSpacing = value;
		}
	}

	[Description("Indicates location of popup in relation to it's parent.")]
	[DevCoBrowsable(false)]
	[Browsable(true)]
	[DefaultValue(ePopupSide.Default)]
	public ePopupSide PopupSide
	{
		get
		{
			return __2AF9.PopupSide;
		}
		set
		{
			__2AF9.PopupSide = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is pressed.")]
	[DefaultValue(null)]
	public Image PressedImage
	{
		get
		{
			return __2AF9.PressedImage;
		}
		set
		{
			__2AF9.PressedImage = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ShapeTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[TypeConverter("DevComponents.DotNetBar.Design.ShapeStringConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	public ShapeDescriptor Shape
	{
		get
		{
			return __2AF9.Shape;
		}
		set
		{
			__2AF9.Shape = value;
		}
	}

	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Determines whether sub-items are displayed.")]
	[Category("Behavior")]
	[Browsable(true)]
	public bool ShowSubItems
	{
		get
		{
			return __2AF9.ShowSubItems;
		}
		set
		{
			__2AF9.ShowSubItems = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button.")]
	public bool SplitButton
	{
		get
		{
			return __2AF9.SplitButton;
		}
		set
		{
			__2AF9.SplitButton = value;
		}
	}

	[Description("Specifies the visual style of the button.")]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public eDotNetBarStyle Style
	{
		get
		{
			return __2AF9.Style;
		}
		set
		{
			__2AF9.Style = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public SubItemsCollection SubItems => __2AF9.SubItems;

	[DefaultValue(12)]
	[Description("Indicates the width of the expand part of the button item.")]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int SubItemsExpandWidth
	{
		get
		{
			return __2AF9.SubItemsExpandWidth;
		}
		set
		{
			__2AF9.SubItemsExpandWidth = value;
		}
	}

	[Description("Indicates the default Text to display on the Button.")]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Text
	{
		get
		{
			if (!__2AFD)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)__2AF9).Text;
		}
		set
		{
			if (__2AFD)
			{
				((Control)__2AF9).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
	[DefaultValue(eButtonTextAlignment.Center)]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return __2AF9.TextAlignment;
		}
		set
		{
			__2AF9.TextAlignment = value;
		}
	}

	[Browsable(false)]
	public bool _2274 => true;

	[Description("Occurs right before a Button Cell is painted.")]
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

	[Description("Occurs when a Button Cell is Clicked.")]
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

	public DataGridViewButtonXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewButtonXCell();
		__2AF9 = new ButtonX();
		((Control)__2AF9).Visible = false;
		__2AF9.FadeEffect = false;
		ἧ(Ἠ: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageFixedSize()
	{
		return __2AF9.ShouldSerializeImageFixedSize();
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			__2AF9.ButtonItem.Click += _2B00;
			__2AF9.ButtonItem.ExpandChange += _2AFF;
		}
		else
		{
			__2AF9.ButtonItem.Click -= _2B00;
			__2AF9.ButtonItem.ExpandChange -= _2AFF;
		}
	}

	private void _2AFF(object _٠, EventArgs ؿ)
	{
		if (!__2AF9.ButtonItem.Expanded)
		{
			((DataGridViewElement)this).DataGridView.InvalidateCell(((DataGridViewBand)this).Index, _2AF3);
			__2AFC = __2AFA == __2AFB;
			__2AFA = -1;
			if (__2AFB >= 0 && ((DataGridViewElement)this).DataGridView.Rows[__2AFB].Cells[((DataGridViewBand)this).Index] is DataGridViewButtonXCell dataGridViewButtonXCell)
			{
				dataGridViewButtonXCell._2AEC(__2AFB);
			}
		}
	}

	private void _2B00(object _٠, EventArgs ؿ)
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
		DataGridViewButtonXColumn dataGridViewButtonXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewButtonXColumn;
		if (dataGridViewButtonXColumn != null)
		{
			__2AF9.ButtonItem._0EBA(dataGridViewButtonXColumn._2AF4.ButtonItem);
			dataGridViewButtonXColumn.Enabled = Enabled;
			dataGridViewButtonXColumn.TextAlignment = TextAlignment;
			dataGridViewButtonXColumn.ImageTextSpacing = ImageTextSpacing;
			dataGridViewButtonXColumn.Shape = Shape;
		}
		return dataGridViewButtonXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		ἧ(Ἠ: false);
		if (disposing)
		{
			((Component)(object)__2AF9).Dispose();
		}
		if (__2AFE != null)
		{
			((Image)__2AFE).Dispose();
			__2AFE = null;
		}
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
