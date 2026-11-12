using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewButtonXColumn), "Controls.ProgressBarX.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
public class DataGridViewProgressBarXColumn : DataGridViewButtonColumn, IDataGridViewColumn
{
	private ProgressBarX _ProgressBarX;

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

	internal ProgressBarX ProgressBarX => _ProgressBarX;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the color of the progress chunk.")]
	public Color ChunkColor
	{
		get
		{
			return _ProgressBarX.ChunkColor;
		}
		set
		{
			_ProgressBarX.ChunkColor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets the target gradient color of the progress chunk.")]
	public Color ChunkColor2
	{
		get
		{
			return _ProgressBarX.ChunkColor2;
		}
		set
		{
			_ProgressBarX.ChunkColor2 = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets the gradient angle of the progress chunk.")]
	public int ChunkGradientAngle
	{
		get
		{
			return _ProgressBarX.ChunkGradientAngle;
		}
		set
		{
			_ProgressBarX.ChunkGradientAngle = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(false)]
	[DefaultValue(eProgressBarItemColor.Normal)]
	[Category("Appearance")]
	[Description("Indicates predefined color of item when Office 2007 style is used.")]
	public eProgressBarItemColor ColorTable
	{
		get
		{
			return _ProgressBarX.ColorTable;
		}
		set
		{
			_ProgressBarX.ColorTable = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Indicates the maximum value of the range of the control.")]
	public int Maximum
	{
		get
		{
			return _ProgressBarX.Maximum;
		}
		set
		{
			_ProgressBarX.Maximum = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return _ProgressBarX.Minimum;
		}
		set
		{
			_ProgressBarX.Minimum = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Description("Specifies the visual style of the control.")]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return _ProgressBarX.Style;
		}
		set
		{
			_ProgressBarX.Style = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the default Text to display on the Progress Bar.")]
	public string Text
	{
		get
		{
			if (!_InCellCallBack)
			{
				return ((DataGridViewButtonColumn)this).Text;
			}
			return ((Control)_ProgressBarX).Text;
		}
		set
		{
			if (_InCellCallBack)
			{
				((Control)_ProgressBarX).Text = value;
			}
			else
			{
				((DataGridViewButtonColumn)this).Text = value;
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets whether the text inside the progress bar is displayed.")]
	public bool TextVisible
	{
		get
		{
			return _ProgressBarX.TextVisible;
		}
		set
		{
			_ProgressBarX.TextVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Indicates the current position of the progress bar.")]
	public int Value
	{
		get
		{
			return _ProgressBarX.Value;
		}
		set
		{
			_ProgressBarX.Value = value;
		}
	}

	[Browsable(false)]
	public bool OwnerPaintCell => true;

	[Description("Occurs right before a ProgressBarX Cell is painted.")]
	public event EventHandler<BeforeCellPaintEventArgs> BeforeCellPaint;

	[Description("Occurs when a ProgressBarX Cell is Clicked.")]
	public event EventHandler<EventArgs> Click;

	public DataGridViewProgressBarXColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new DataGridViewProgressBarXCell();
		_ProgressBarX = new ProgressBarX();
		((Control)_ProgressBarX).Visible = false;
		HookEvents(hook: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor()
	{
		return !_ProgressBarX.ChunkColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor()
	{
		_ProgressBarX.ChunkColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeChunkColor2()
	{
		return !_ProgressBarX.ChunkColor2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetChunkColor2()
	{
		_ProgressBarX.ChunkColor2 = Color.Empty;
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_ProgressBarX.ProgressBarItem.Click += ProgressBarItem_Click;
		}
		else
		{
			_ProgressBarX.ProgressBarItem.Click -= ProgressBarItem_Click;
		}
	}

	private void ProgressBarItem_Click(object sender, EventArgs e)
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
		DataGridViewProgressBarXColumn dataGridViewProgressBarXColumn = ((DataGridViewButtonColumn)this).Clone() as DataGridViewProgressBarXColumn;
		if (dataGridViewProgressBarXColumn != null)
		{
			_ProgressBarX.ProgressBarItem.InternalCopyToItem(dataGridViewProgressBarXColumn.ProgressBarX.ProgressBarItem);
		}
		return dataGridViewProgressBarXColumn;
	}

	protected override void Dispose(bool disposing)
	{
		HookEvents(hook: false);
		((DataGridViewColumn)this).Dispose(disposing);
	}
}
