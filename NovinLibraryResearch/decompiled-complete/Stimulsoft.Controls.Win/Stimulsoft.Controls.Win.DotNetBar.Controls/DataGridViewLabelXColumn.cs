using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewLabelXColumn), "Controls.LabelX.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewLabelXColumn : DataGridViewButtonColumn, IDataGridViewColumn
{
	private LabelX _LabelX;

	private Bitmap _CellBitmap;

	private bool _InCellCallBack;

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

	internal bool InCallBack
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

	internal LabelX LabelX => _LabelX;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for the item's Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _LabelX.EnableMarkup;
		}
		set
		{
			_LabelX.EnableMarkup = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether control displays focus cues when focused.")]
	public bool FocusCuesEnabled
	{
		get
		{
			return _LabelX.FocusCuesEnabled;
		}
		set
		{
			_LabelX.FocusCuesEnabled = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eBorderSide.All)]
	[Description("Specifies border sides that are displayed.")]
	public eBorderSide BorderSide
	{
		get
		{
			return _LabelX.BorderSide;
		}
		set
		{
			_LabelX.BorderSide = value;
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue(eBorderType.None)]
	[Description("Indicates the type of the border drawn around the label.")]
	public eBorderType BorderType
	{
		get
		{
			return _LabelX.BorderType;
		}
		set
		{
			_LabelX.BorderType = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("The image that will be displayed on the face of the item.")]
	public Image Image
	{
		get
		{
			return _LabelX.Image;
		}
		set
		{
			_LabelX.Image = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eImagePosition.Left)]
	[Description("The alignment of the image in relation to text displayed by this item.")]
	public eImagePosition ImagePosition
	{
		get
		{
			return _LabelX.ImagePosition;
		}
		set
		{
			_LabelX.ImagePosition = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates bottom padding in pixels.")]
	public int PaddingBottom
	{
		get
		{
			return _LabelX.PaddingBottom;
		}
		set
		{
			_LabelX.PaddingBottom = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates left padding in pixels.")]
	public int PaddingLeft
	{
		get
		{
			return _LabelX.PaddingLeft;
		}
		set
		{
			_LabelX.PaddingLeft = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates top padding in pixels.")]
	public int PaddingTop
	{
		get
		{
			return _LabelX.PaddingTop;
		}
		set
		{
			_LabelX.PaddingTop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Layout")]
	[Description("Indicates right padding in pixels.")]
	public int PaddingRight
	{
		get
		{
			return _LabelX.PaddingRight;
		}
		set
		{
			_LabelX.PaddingRight = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates border line color when border is single line.")]
	public Color SingleLineColor
	{
		get
		{
			return _LabelX.SingleLineColor;
		}
		set
		{
			_LabelX.SingleLineColor = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The text contained in the item.")]
	public string Text
	{
		get
		{
			if (!_InCellCallBack)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)_LabelX).Text;
		}
		set
		{
			if (_InCellCallBack)
			{
				((Control)_LabelX).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the horizontal text alignment.")]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _LabelX.TextAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_LabelX.TextAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates vertical text line alignment.")]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _LabelX.TextLineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_LabelX.TextLineAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the control interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return _LabelX.UseMnemonic;
		}
		set
		{
			_LabelX.UseMnemonic = value;
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[DefaultValue(false)]
	[Description("Indicates whether text is displayed in multiple lines or one long line.")]
	public bool WordWrap
	{
		get
		{
			return _LabelX.WordWrap;
		}
		set
		{
			_LabelX.WordWrap = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a LabelX Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when a LabelX Cell is Clicked.")]
	public event EventHandler<EventArgs> Click;

	public DataGridViewLabelXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewLabelXCell();
		_LabelX = new LabelX();
		((Control)_LabelX).CreateControl();
		((Control)_LabelX).Visible = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSingleLineColor()
	{
		return _LabelX.ShouldSerializeSingleLineColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSingleLineColor()
	{
		_LabelX.ResetSingleLineColor();
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_LabelX.LabelItem.Click += LabelItemClick;
		}
		else
		{
			_LabelX.LabelItem.Click -= LabelItemClick;
		}
	}

	private void LabelItemClick(object sender, EventArgs e)
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
		DataGridViewLabelXColumn dataGridViewLabelXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewLabelXColumn;
		if (dataGridViewLabelXColumn != null)
		{
			_LabelX.LabelItem.InternalCopyToItem(dataGridViewLabelXColumn.LabelX.LabelItem);
			dataGridViewLabelXColumn.FocusCuesEnabled = FocusCuesEnabled;
			dataGridViewLabelXColumn.Text = Text;
			dataGridViewLabelXColumn.UseMnemonic = UseMnemonic;
		}
		return dataGridViewLabelXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
