using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "ButtonX.ButtonX.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewButtonXColumn : DataGridViewButtonColumn, IDataGridViewColumn
{
	private ButtonX _ButtonX;

	private int _ActiveRowIndex = -1;

	private int _CurrentRowIndex = -1;

	private bool _ExpandClosed;

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

	internal ButtonX ButtonX => _ButtonX;

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

	internal bool ExpandClosed
	{
		get
		{
			return _ExpandClosed;
		}
		set
		{
			_ExpandClosed = value;
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

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether Checked property is automatically inverted when button is clicked.")]
	public bool AutoCheckOnClick
	{
		get
		{
			return _ButtonX.AutoCheckOnClick;
		}
		set
		{
			_ButtonX.AutoCheckOnClick = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the button will auto-expand (display pop-up menu or toolbar) when clicked.")]
	public virtual bool AutoExpandOnClick
	{
		get
		{
			return _ButtonX.AutoExpandOnClick;
		}
		set
		{
			_ButtonX.AutoExpandOnClick = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether item is checked or not.")]
	public virtual bool Checked
	{
		get
		{
			return _ButtonX.Checked;
		}
		set
		{
			_ButtonX.Checked = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Gets or sets Bar Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ColorScheme ColorScheme
	{
		get
		{
			return _ButtonX.ColorScheme;
		}
		set
		{
			_ButtonX.ColorScheme = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eButtonColor.BlueWithBackground)]
	[Category("Appearance")]
	[Description("Indicates predefined color of button when Office 2007 style is used.")]
	[NotifyParentProperty(true)]
	public eButtonColor ColorTable
	{
		get
		{
			return _ButtonX.ColorTable;
		}
		set
		{
			_ButtonX.ColorTable = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates custom color table name for the button when Office 2007 style is used.")]
	public string CustomColorName
	{
		get
		{
			return _ButtonX.CustomColorName;
		}
		set
		{
			_ButtonX.CustomColorName = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is disabled.")]
	public Image DisabledImage
	{
		get
		{
			return _ButtonX.DisabledImage;
		}
		set
		{
			_ButtonX.DisabledImage = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _ButtonX.EnableMarkup;
		}
		set
		{
			_ButtonX.EnableMarkup = value;
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
			return ((Control)_ButtonX).Enabled;
		}
		set
		{
			((Control)_ButtonX).Enabled = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eHotTrackingStyle.Default)]
	[Category("Appearance")]
	[Description("Indicates the button mouse over tracking style. Setting the value to Color will render the image in gray-scale when mouse is not over the item.")]
	public eHotTrackingStyle HotTrackingStyle
	{
		get
		{
			return _ButtonX.HotTrackingStyle;
		}
		set
		{
			_ButtonX.HotTrackingStyle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("The image that will be displayed when mouse hovers over the item.")]
	public Image HoverImage
	{
		get
		{
			return _ButtonX.HoverImage;
		}
		set
		{
			_ButtonX.HoverImage = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("The image that will be displayed on the face of the button.")]
	public Image Image
	{
		get
		{
			return _ButtonX.Image;
		}
		set
		{
			_ButtonX.Image = value;
		}
	}

	[Browsable(true)]
	public Size ImageFixedSize
	{
		get
		{
			return _ButtonX.ImageFixedSize;
		}
		set
		{
			_ButtonX.ImageFixedSize = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eImagePosition.Left)]
	[Category("Appearance")]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	public eImagePosition ImagePosition
	{
		get
		{
			return _ButtonX.ImagePosition;
		}
		set
		{
			_ButtonX.ImagePosition = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates amount of spacing between button image if specified and text.")]
	public int ImageTextSpacing
	{
		get
		{
			return _ButtonX.ImageTextSpacing;
		}
		set
		{
			_ButtonX.ImageTextSpacing = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(ePopupSide.Default)]
	[Description("Indicates location of popup in relation to it's parent.")]
	public ePopupSide PopupSide
	{
		get
		{
			return _ButtonX.PopupSide;
		}
		set
		{
			_ButtonX.PopupSide = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("The image that will be displayed when item is pressed.")]
	public Image PressedImage
	{
		get
		{
			return _ButtonX.PressedImage;
		}
		set
		{
			_ButtonX.PressedImage = value;
		}
	}

	[DefaultValue(null)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.ShapeStringConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
	public ShapeDescriptor Shape
	{
		get
		{
			return _ButtonX.Shape;
		}
		set
		{
			_ButtonX.Shape = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
	public bool ShowSubItems
	{
		get
		{
			return _ButtonX.ShowSubItems;
		}
		set
		{
			_ButtonX.ShowSubItems = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether button appears as split button.")]
	public bool SplitButton
	{
		get
		{
			return _ButtonX.SplitButton;
		}
		set
		{
			_ButtonX.SplitButton = value;
		}
	}

	[Category("Appearance")]
	[Description("Specifies the visual style of the button.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public eDotNetBarStyle Style
	{
		get
		{
			return _ButtonX.Style;
		}
		set
		{
			_ButtonX.Style = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SubItemsCollection SubItems => _ButtonX.SubItems;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(12)]
	[Category("Behavior")]
	[Description("Indicates the width of the expand part of the button item.")]
	public int SubItemsExpandWidth
	{
		get
		{
			return _ButtonX.SubItemsExpandWidth;
		}
		set
		{
			_ButtonX.SubItemsExpandWidth = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the default Text to display on the Button.")]
	public string Text
	{
		get
		{
			if (!_InCellCallBack)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)_ButtonX).Text;
		}
		set
		{
			if (_InCellCallBack)
			{
				((Control)_ButtonX).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eButtonTextAlignment.Center)]
	[Category("Appearance")]
	[Description("Indicates text alignment. Applies only when button text is not composed using text markup. Default value is center.")]
	public eButtonTextAlignment TextAlignment
	{
		get
		{
			return _ButtonX.TextAlignment;
		}
		set
		{
			_ButtonX.TextAlignment = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a Button Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when a Button Cell is Clicked.")]
	public event EventHandler<EventArgs> Click;

	public DataGridViewButtonXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewButtonXCell();
		_ButtonX = new ButtonX();
		((Control)_ButtonX).Visible = false;
		_ButtonX.FadeEffect = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageFixedSize()
	{
		return _ButtonX.ShouldSerializeImageFixedSize();
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_ButtonX.ButtonItem.Click += ButtonItem_Click;
			_ButtonX.ButtonItem.ExpandChange += ButtonItem_ExpandChange;
		}
		else
		{
			_ButtonX.ButtonItem.Click -= ButtonItem_Click;
			_ButtonX.ButtonItem.ExpandChange -= ButtonItem_ExpandChange;
		}
	}

	private void ButtonItem_ExpandChange(object sender, EventArgs e)
	{
		if (!_ButtonX.ButtonItem.Expanded)
		{
			((DataGridViewElement)this).DataGridView.InvalidateCell(((DataGridViewBand)this).Index, ActiveRowIndex);
			_ExpandClosed = _ActiveRowIndex == _CurrentRowIndex;
			_ActiveRowIndex = -1;
			if (_CurrentRowIndex >= 0 && ((DataGridViewElement)this).DataGridView.Rows[_CurrentRowIndex].Cells[((DataGridViewBand)this).Index] is DataGridViewButtonXCell dataGridViewButtonXCell)
			{
				dataGridViewButtonXCell.DoMouseEnter(_CurrentRowIndex);
			}
		}
	}

	private void ButtonItem_Click(object sender, EventArgs e)
	{
		if (this.Click != null)
		{
			this.Click(sender, e);
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
		DataGridViewButtonXColumn dataGridViewButtonXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewButtonXColumn;
		if (dataGridViewButtonXColumn != null)
		{
			_ButtonX.ButtonItem.InternalCopyToItem(dataGridViewButtonXColumn.ButtonX.ButtonItem);
			dataGridViewButtonXColumn.Enabled = Enabled;
			dataGridViewButtonXColumn.TextAlignment = TextAlignment;
			dataGridViewButtonXColumn.ImageTextSpacing = ImageTextSpacing;
			dataGridViewButtonXColumn.Shape = Shape;
		}
		return dataGridViewButtonXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
