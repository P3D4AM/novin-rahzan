using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewX), "Controls.DataGridViewX.bmp")]
public class DataGridViewX : DataGridView
{
	private struct _225B
	{
		public bool ᒅ;

		public int _225C;
	}

	private int _2243 = -2;

	private int _2244 = -2;

	private int _2245 = -2;

	private int _2246 = -2;

	private bool _2247 = true;

	private Office2007DataGridViewColorTable ຊ;

	private Office2007ButtonItemStateColorTable _2248;

	private _225B[] _2249 = new _225B[1];

	private int _224A = -2;

	private bool _224B = true;

	private bool _224C = true;

	private bool _224D = true;

	private bool __224E = true;

	private eScrollBarAppearance _ז = eScrollBarAppearance.ApplicationScroll;

	private bool __224F;

	internal Office2007ButtonItemStateColorTable _2242 => _2248;

	[Browsable(false)]
	[DefaultValue(true)]
	public bool DelayResizeCallWhenMdiChild
	{
		get
		{
			return __224E;
		}
		set
		{
			__224E = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the scroll-bar visual style.")]
	[DefaultValue(eScrollBarAppearance.ApplicationScroll)]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ז;
		}
		set
		{
			_ז = value;
			_2259();
		}
	}

	[Browsable(true)]
	[Description("Indicates whether selected column header is highlighted.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool HighlightSelectedColumnHeaders
	{
		get
		{
			return _224B;
		}
		set
		{
			_224B = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether select all sign displayed in top-left corner of the grid is visible.")]
	[Browsable(true)]
	public bool SelectAllSignVisible
	{
		get
		{
			return _224C;
		}
		set
		{
			_224C = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether enhanced selection for the cells is painted in Office 2007 style")]
	[DefaultValue(true)]
	public bool PaintEnhancedSelection
	{
		get
		{
			return _224D;
		}
		set
		{
			_224D = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether BackgroundColor property set on DataGridView is obeyed instead of using system color scheme color.")]
	[Category("Appearance")]
	public bool UseCustomBackgroundColor
	{
		get
		{
			return __224F;
		}
		set
		{
			__224F = value;
		}
	}

	public DataGridViewX()
	{
		_225A();
		((Control)this).SetStyle((ControlStyles)139280, true);
		((Control)this).DoubleBuffered = true;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		_225D _225D2 = _2256();
		if (_225D2 != null && ((Control)_225D2).Visible)
		{
			((Control)_225D2).Invalidate();
		}
		_2272 _22722 = _2257();
		if (_22722 != null && ((Control)_22722).Visible)
		{
			((Control)_22722).Invalidate();
		}
		Office2007ColorTable office2007ColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			ຊ = office2007ColorTable.DataGridView;
			_2248 = office2007ColorTable.ButtonItemColors[0].Checked;
		}
		try
		{
			if (!__224F)
			{
				((DataGridView)this).BackgroundColor = ຊ.BackgroundColor;
			}
			((DataGridView)this).ColumnHeadersDefaultCellStyle.ForeColor = ຊ.ColumnHeaderNormalText;
			((DataGridView)this).ColumnHeadersDefaultCellStyle.SelectionForeColor = ຊ.ColumnHeaderSelectedText;
			((DataGridView)this).DefaultCellStyle.BackColor = ຊ.DefaultCellBackground;
			((DataGridView)this).DefaultCellStyle.ForeColor = ຊ.DefaultCellText;
			if (PaintEnhancedSelection)
			{
				((DataGridView)this).DefaultCellStyle.SelectionForeColor = _2248.Text;
			}
			((DataGridView)this).GridColor = ຊ.GridColor;
			((DataGridView)this).RowHeadersDefaultCellStyle.ForeColor = ຊ.ColumnHeaderNormalText;
			((DataGridView)this).RowHeadersDefaultCellStyle.SelectionForeColor = ຊ.ColumnHeaderSelectedText;
			((DataGridView)this).EnableHeadersVisualStyles = false;
		}
		catch
		{
		}
	}

	protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Invalid comparison between Unknown and I4
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Invalid comparison between Unknown and I4
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		bool enabled = ((Control)this).Enabled;
		Office2007DataGridViewColorTable colorTable = GetColorTable();
		if (!_2247 || colorTable == null || ((HandledEventArgs)(object)e).Handled || (e.ColumnIndex >= 0 && e.RowIndex >= 0 && (!_224D || (e.State & 0x20) != 32)))
		{
			((DataGridView)this).OnCellPainting(e);
			return;
		}
		Rectangle cellBounds = e.CellBounds;
		Graphics graphics = e.Graphics;
		LinearGradientColorTable linearGradientColorTable = null;
		Color empty = Color.Empty;
		DataGridViewColumn val = null;
		int num = -1;
		if (e.ColumnIndex >= 0)
		{
			val = ((DataGridView)this).Columns[e.ColumnIndex];
			num = val.DisplayIndex;
		}
		if (e.ColumnIndex == -1 && e.RowIndex == -1)
		{
			if (_2243 == -1)
			{
				linearGradientColorTable = colorTable.SelectorMouseOverBackground;
				empty = colorTable.SelectorMouseOverBorder;
			}
			else
			{
				linearGradientColorTable = colorTable.SelectorBackground;
				empty = colorTable.SelectorBorder;
			}
			DisplayHelp.FillRectangle(graphics, cellBounds, linearGradientColorTable);
			DisplayHelp.DrawRectangle(graphics, empty, cellBounds);
			empty = ((_2243 != -1) ? colorTable.SelectorBorderLight : colorTable.SelectorMouseOverBorderLight);
			Rectangle _2023 = cellBounds;
			_2023.Inflate(-1, -1);
			Pen val2 = new Pen(empty);
			try
			{
				graphics.DrawLine(val2, _2023.X, _2023.Bottom - 1, _2023.X, _2023.Y);
				graphics.DrawLine(val2, _2023.X, _2023.Y, _2023.Right - 1, _2023.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			empty = ((_2243 != -1) ? colorTable.SelectorBorderDark : colorTable.SelectorMouseOverBorderDark);
			Pen val3 = new Pen(empty);
			try
			{
				graphics.DrawLine(val3, _2023.Right - 1, _2023.Y, _2023.Right - 1, _2023.Bottom - 1);
				graphics.DrawLine(val3, _2023.X, _2023.Bottom - 1, _2023.Right - 1, _2023.Bottom - 1);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if (_224C)
			{
				GraphicsPath val4 = _2250(_2023);
				if (val4 != null)
				{
					DisplayHelp.FillPath(graphics, val4, (_2243 == -1) ? colorTable.SelectorMouseOverSign : colorTable.SelectorSign);
					val4.Dispose();
				}
			}
		}
		else if (e.ColumnIndex == -1)
		{
			linearGradientColorTable = colorTable.RowNormalBackground;
			empty = colorTable.RowNormalBorder;
			if (_2245 == e.RowIndex)
			{
				linearGradientColorTable = colorTable.RowPressedBackground;
				empty = colorTable.RowPressedBorder;
			}
			else if (_224A == e.RowIndex && enabled)
			{
				if (_2246 == e.RowIndex)
				{
					linearGradientColorTable = colorTable.RowSelectedMouseOverBackground;
					empty = colorTable.RowSelectedMouseOverBorder;
				}
				else
				{
					linearGradientColorTable = colorTable.RowSelectedBackground;
					empty = colorTable.RowSelectedBorder;
				}
			}
			else if (((DataGridViewBand)((DataGridView)this).Rows[e.RowIndex]).Selected && enabled)
			{
				if (_2246 == e.RowIndex)
				{
					linearGradientColorTable = colorTable.RowPressedBackground;
					empty = colorTable.RowPressedBorder;
				}
				else
				{
					linearGradientColorTable = colorTable.RowPressedBackground;
					empty = colorTable.RowPressedBorder;
				}
			}
			else if (_2246 == e.RowIndex)
			{
				linearGradientColorTable = colorTable.RowMouseOverBackground;
				empty = colorTable.RowMouseOverBorder;
			}
			DisplayHelp.FillRectangle(graphics, cellBounds, linearGradientColorTable);
			Pen val5 = new Pen(empty);
			try
			{
				graphics.DrawLine(val5, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
				if (_224A == e.RowIndex + 1 && enabled)
				{
					Color color = colorTable.RowSelectedBorder;
					if (_2245 == e.RowIndex + 1)
					{
						color = colorTable.RowPressedBorder;
					}
					else if (_2246 == e.RowIndex + 1)
					{
						color = colorTable.RowSelectedMouseOverBorder;
					}
					Pen val6 = new Pen(color);
					try
					{
						graphics.DrawLine(val6, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				else
				{
					graphics.DrawLine(val5, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			e.PaintContent(cellBounds);
		}
		else if (e.RowIndex == -1)
		{
			if (_2244 == e.ColumnIndex)
			{
				linearGradientColorTable = colorTable.ColumnHeaderPressedBackground;
				empty = colorTable.ColumnHeaderPressedBorder;
			}
			else if (_2243 == e.ColumnIndex)
			{
				if (_224B && num >= 0 && e.ColumnIndex >= 0 && ((_2249.Length > num && _2249[num].ᒅ) || ((DataGridViewBand)((DataGridView)this).Columns[e.ColumnIndex]).Selected))
				{
					linearGradientColorTable = colorTable.ColumnHeaderSelectedMouseOverBackground;
					empty = colorTable.ColumnHeaderSelectedMouseOverBorder;
				}
				else
				{
					linearGradientColorTable = colorTable.ColumnHeaderMouseOverBackground;
					empty = colorTable.ColumnHeaderMouseOverBorder;
				}
			}
			else if (!_224B)
			{
				linearGradientColorTable = colorTable.ColumnHeaderNormalBackground;
				empty = colorTable.ColumnHeaderNormalBorder;
			}
			else if (num >= 0 && e.ColumnIndex >= 0 && ((_2249.Length > num && _2249[num].ᒅ) || ((DataGridViewBand)((DataGridView)this).Columns[e.ColumnIndex]).Selected) && enabled)
			{
				linearGradientColorTable = colorTable.ColumnHeaderSelectedBackground;
				empty = colorTable.ColumnHeaderSelectedBorder;
			}
			else if (((DataGridViewBand)((DataGridView)this).Columns[e.ColumnIndex]).Selected && enabled)
			{
				linearGradientColorTable = colorTable.ColumnHeaderPressedBackground;
				empty = colorTable.ColumnHeaderPressedBorder;
			}
			else
			{
				linearGradientColorTable = colorTable.ColumnHeaderNormalBackground;
				empty = colorTable.ColumnHeaderNormalBorder;
			}
			DisplayHelp.FillRectangle(graphics, cellBounds, linearGradientColorTable);
			Pen val7 = new Pen(empty);
			try
			{
				graphics.DrawLine(val7, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
				if (enabled && _224B && num >= 0 && _2249.Length > num + 1 && (_2249[num + 1].ᒅ || _2249[num + 1]._225C == _2244))
				{
					Color color2 = colorTable.ColumnHeaderSelectedBorder;
					if (_2249[num + 1]._225C == _2244)
					{
						color2 = colorTable.ColumnHeaderPressedBorder;
					}
					else if (_2243 == num + 1)
					{
						color2 = colorTable.ColumnHeaderSelectedMouseOverBorder;
					}
					Pen val8 = new Pen(color2);
					try
					{
						graphics.DrawLine(val8, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
				else
				{
					graphics.DrawLine(val7, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
			e.PaintContent(cellBounds);
		}
		else if ((e.State & 0x20) == 32)
		{
			if (((DataGridView)this).Columns[e.ColumnIndex] is _2273 { _2274: not false })
			{
				((DataGridView)this).OnCellPainting(e);
				return;
			}
			e.PaintBackground(cellBounds, false);
			if (enabled)
			{
				cellBounds.Height--;
				ᯊ.ᯓ(graphics, _2248, cellBounds, RoundRectangleShapeDescriptor.RectangleShape, ᯕ: false, ᯖ: false);
				cellBounds.Height++;
				if (((DataGridView)this).CurrentCellAddress.X == e.ColumnIndex && ((DataGridView)this).CurrentCellAddress.Y == e.RowIndex && (e.PaintParts & 0x20) == 32 && ((Control)this).ShowFocusCues && ((Control)this).Focused && cellBounds.Width > 0 && cellBounds.Height > 0)
				{
					if (_2248.TopBackground != null)
					{
						ControlPaint.DrawFocusRectangle(graphics, cellBounds, Color.Empty, _2248.TopBackground.End);
					}
					else if (_2248.Background != null)
					{
						ControlPaint.DrawFocusRectangle(graphics, cellBounds, Color.Empty, _2248.Background.Start);
					}
					else
					{
						ControlPaint.DrawFocusRectangle(graphics, cellBounds, Color.Empty, ((DataGridView)this).BackgroundColor);
					}
				}
			}
			e.PaintContent(cellBounds);
		}
		((HandledEventArgs)(object)e).Handled = true;
		((DataGridView)this).OnCellPainting(e);
	}

	private GraphicsPath _2250(Rectangle _2023)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		_2023.Inflate(-3, -3);
		if (_2023.Width > 2 && _2023.Height > 2)
		{
			GraphicsPath val = new GraphicsPath();
			val.AddLine(_2023.Right, _2023.Y, _2023.Right, _2023.Bottom);
			val.AddLine(_2023.Right, _2023.Bottom, _2023.Right - _2023.Height, _2023.Bottom);
			val.CloseAllFigures();
			return val;
		}
		return null;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			Office2007DataGridViewColorTable dataGridView = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.DataGridView;
			_2251(dataGridView);
		}
		((DataGridView)this).OnHandleCreated(e);
	}

	private void _2251(Office2007DataGridViewColorTable ዅ)
	{
		if (((DataGridView)this).GridColor != ዅ.GridColor)
		{
			((DataGridView)this).GridColor = ዅ.GridColor;
		}
		if (_224D)
		{
			((DataGridView)this).DefaultCellStyle.SelectionForeColor = ((DataGridView)this).DefaultCellStyle.ForeColor;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Office2007ColorTable office2007ColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			ຊ = office2007ColorTable.DataGridView;
			if (StyleManager.IsVisualStudio2012(StyleManager.Style))
			{
				_2248 = office2007ColorTable.ButtonItemColors[0].Pressed;
			}
			else
			{
				_2248 = office2007ColorTable.ButtonItemColors[0].Checked;
			}
		}
		if (((DataGridView)this).CurrentCell != null)
		{
			_224A = ((DataGridView)this).CurrentCell.RowIndex;
		}
		else
		{
			_224A = -1;
		}
		((DataGridView)this).OnPaint(e);
		if (((Control)((DataGridView)this).VerticalScrollBar).Visible && ((Control)((DataGridView)this).HorizontalScrollBar).Visible)
		{
			Rectangle bounds = new Rectangle(((Control)((DataGridView)this).VerticalScrollBar).Left, ((Control)((DataGridView)this).VerticalScrollBar).Bottom, ((Control)((DataGridView)this).VerticalScrollBar).Width, ((Control)((DataGridView)this).HorizontalScrollBar).Height);
			Color color = office2007ColorTable.AppScrollBar.Default.Background.End;
			if (color.IsEmpty)
			{
				color = office2007ColorTable.AppScrollBar.Default.Background.Start;
			}
			DisplayHelp.FillRectangle(e.Graphics, bounds, color);
		}
		ຊ = null;
		_2248 = null;
	}

	protected override void OnCurrentCellChanged(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((DataGridView)this).SelectionMode == 1)
		{
			_2252();
		}
		((DataGridView)this).OnCurrentCellChanged(e);
	}

	protected override void OnSelectionChanged(EventArgs e)
	{
		_2252();
		((DataGridView)this).OnSelectionChanged(e);
	}

	protected override void OnDataSourceChanged(EventArgs e)
	{
		((DataGridView)this).OnDataSourceChanged(e);
		_2252();
	}

	private void _2252()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		_225B[] array = new _225B[((BaseCollection)((DataGridView)this).Columns).Count];
		for (int i = 0; i < ((BaseCollection)((DataGridView)this).Columns).Count; i++)
		{
			array[((DataGridView)this).Columns[i].DisplayIndex]._225C = i;
		}
		int num = ((BaseCollection)((DataGridView)this).Columns).Count;
		if ((int)((DataGridView)this).SelectionMode == 1)
		{
			if (((DataGridView)this).CurrentCell != null)
			{
				int displayIndex = ((DataGridView)this).Columns[((DataGridView)this).CurrentCell.ColumnIndex].DisplayIndex;
				array[displayIndex].ᒅ = true;
				array[displayIndex]._225C = ((DataGridView)this).CurrentCell.ColumnIndex;
			}
		}
		else
		{
			foreach (DataGridViewCell item in (BaseCollection)((DataGridView)this).SelectedCells)
			{
				DataGridViewCell val = item;
				if (val.ColumnIndex == -1)
				{
					continue;
				}
				int displayIndex2 = ((DataGridView)this).Columns[val.ColumnIndex].DisplayIndex;
				if (!array[displayIndex2].ᒅ)
				{
					num--;
					array[displayIndex2].ᒅ = true;
					array[displayIndex2]._225C = val.ColumnIndex;
					if (num == 0)
					{
						break;
					}
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (_2249.Length <= j || array[j].ᒅ == _2249[j].ᒅ)
			{
				continue;
			}
			int _225C = _2249[j]._225C;
			if (_225C >= 0 && _225C < ((BaseCollection)((DataGridView)this).Columns).Count)
			{
				((DataGridView)this).InvalidateColumn(_225C);
				if (_225C > 0)
				{
					((DataGridView)this).InvalidateColumn(_225C - 1);
				}
			}
		}
		if (_224A > 0 && _224A < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).InvalidateRow(_224A - 1);
		}
		if (((DataGridView)this).CurrentCell != null && ((DataGridView)this).CurrentCell.RowIndex > 0)
		{
			_224A = ((DataGridView)this).CurrentCell.RowIndex;
			((DataGridView)this).InvalidateRow(_224A - 1);
		}
		else
		{
			_224A = -2;
		}
		_2249 = array;
	}

	protected virtual Office2007DataGridViewColorTable GetColorTable()
	{
		return ຊ;
	}

	protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
	{
		if (e.ColumnIndex == -1 || e.RowIndex == -1)
		{
			((Control)this).DoubleBuffered = false;
		}
		if (e.RowIndex == -1)
		{
			_2244 = e.ColumnIndex;
			if (e.ColumnIndex >= 0)
			{
				_2253(((DataGridView)this).Columns[e.ColumnIndex].DisplayIndex);
			}
		}
		else
		{
			_2244 = -2;
		}
		if (e.ColumnIndex == -1)
		{
			_2245 = e.RowIndex;
			if (_2245 > 0 && _2245 < ((DataGridView)this).Rows.Count)
			{
				((DataGridView)this).InvalidateRow(_2245 - 1);
			}
		}
		else
		{
			_2245 = -2;
		}
		((DataGridView)this).OnCellMouseDown(e);
	}

	private void _2253(int _2254)
	{
		_2254--;
		for (int i = 0; i < ((BaseCollection)((DataGridView)this).Columns).Count; i++)
		{
			DataGridViewColumn val = ((DataGridView)this).Columns[i];
			if (((DataGridViewBand)val).Displayed && val.DisplayIndex == _2254)
			{
				((DataGridView)this).InvalidateColumn(i);
				break;
			}
		}
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		_2255();
		((Control)this).OnDragDrop(drgevent);
	}

	private void _2255()
	{
		if (!((Control)this).DoubleBuffered)
		{
			((Control)this).DoubleBuffered = true;
		}
		if (_2245 > 0 && _2245 < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).InvalidateRow(_2245 - 1);
		}
		if (_2245 >= 0 && _2245 < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).InvalidateRow(_2245);
		}
		_2245 = -2;
		_2244 = -2;
	}

	protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
	{
		_2255();
		((DataGridView)this).OnCellMouseUp(e);
	}

	protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
	{
		if (e.RowIndex == -1)
		{
			_2243 = e.ColumnIndex;
		}
		else
		{
			_2243 = -2;
		}
		if (e.ColumnIndex == -1)
		{
			if (_2246 != e.RowIndex)
			{
				_2246 = e.RowIndex;
				if (_2246 > 0)
				{
					((DataGridView)this).InvalidateRow(_2246 - 1);
				}
			}
		}
		else
		{
			_2246 = -2;
		}
		((DataGridView)this).OnCellMouseEnter(e);
	}

	protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
	{
		_2243 = -2;
		_2246 = -2;
		((DataGridView)this).OnCellMouseLeave(e);
	}

	protected override void OnScroll(ScrollEventArgs e)
	{
		((DataGridView)this).OnScroll(e);
		_225D _225D2 = _2256();
		if (_225D2 != null && ((Control)_225D2).Visible)
		{
			_225D2._1DD2();
		}
		_2272 _22722 = _2257();
		if (_22722 != null && ((Control)_22722).Visible)
		{
			_22722._1DD2();
		}
	}

	private _225D _2256()
	{
		return ((DataGridView)this).VerticalScrollBar as _225D;
	}

	private _2272 _2257()
	{
		return ((DataGridView)this).HorizontalScrollBar as _2272;
	}

	private void ڣ(MethodInvoker ڤ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = ڤ;
		val.Interval = 10;
		val.Tick += ڥ;
		val.Start();
	}

	private void ڥ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)_٠;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		MethodInvoker val = null;
		if (__224E)
		{
			Form val2 = ((Control)this).FindForm();
			if (val2 != null && val2.IsMdiChild)
			{
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						_2258(e);
					};
				}
				ڣ(val);
			}
			else
			{
				((DataGridView)this).OnResize(e);
			}
		}
		else
		{
			((DataGridView)this).OnResize(e);
		}
	}

	private void _2258(EventArgs ؿ)
	{
		((DataGridView)this).OnResize(ؿ);
	}

	private void _2259()
	{
		_225D _225D2 = _2256();
		if (_225D2 != null)
		{
			_225D2.ᘾ = _ז == eScrollBarAppearance.ApplicationScroll;
		}
		_2272 _22722 = _2257();
		if (_22722 != null)
		{
			_22722.ᘾ = _ז == eScrollBarAppearance.ApplicationScroll;
		}
	}

	private void _225A()
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		Type typeFromHandle = typeof(DataGridView);
		FieldInfo field = typeFromHandle.GetField("vertScrollBar", BindingFlags.Instance | BindingFlags.NonPublic);
		if ((object)field == null)
		{
			return;
		}
		object? value = field.GetValue(this);
		ScrollBar val = (ScrollBar)((value is ScrollBar) ? value : null);
		if (val == null)
		{
			return;
		}
		MethodInfo method = typeFromHandle.GetMethod("DataGridViewVScrolled", BindingFlags.Instance | BindingFlags.NonPublic);
		if ((object)method == null)
		{
			return;
		}
		_225D _225D2 = new _225D();
		_225D2.ᘾ = _ז == eScrollBarAppearance.ApplicationScroll;
		((ScrollBar)_225D2).Minimum = val.Minimum;
		((ScrollBar)_225D2).Maximum = val.Maximum;
		((ScrollBar)_225D2).SmallChange = val.SmallChange;
		((ScrollBar)_225D2).LargeChange = val.LargeChange;
		((Control)_225D2).Top = ((Control)val).Top;
		((Control)_225D2).AccessibleName = ((Control)val).AccessibleName;
		((Control)_225D2).Left = ((Control)val).Left;
		((Control)_225D2).Visible = ((Control)val).Visible;
		((ScrollBar)_225D2).Scroll += (ScrollEventHandler)Delegate.CreateDelegate(typeof(ScrollEventHandler), this, method);
		field.SetValue(this, _225D2);
		((Component)(object)val).Dispose();
		((Control)this).Controls.Remove((Control)(object)val);
		((Control)this).Controls.Add((Control)(object)_225D2);
		field = typeFromHandle.GetField("horizScrollBar", BindingFlags.Instance | BindingFlags.NonPublic);
		if ((object)field == null)
		{
			return;
		}
		object? value2 = field.GetValue(this);
		val = (ScrollBar)((value2 is ScrollBar) ? value2 : null);
		if (val != null)
		{
			method = typeFromHandle.GetMethod("DataGridViewHScrolled", BindingFlags.Instance | BindingFlags.NonPublic);
			if ((object)method != null)
			{
				_2272 _22722 = new _2272();
				_22722.ᘾ = _ז == eScrollBarAppearance.ApplicationScroll;
				((ScrollBar)_22722).Minimum = val.Minimum;
				((ScrollBar)_22722).Maximum = val.Maximum;
				((ScrollBar)_22722).SmallChange = val.SmallChange;
				((ScrollBar)_22722).LargeChange = val.LargeChange;
				((Control)_22722).Top = ((Control)val).Top;
				((Control)_22722).AccessibleName = ((Control)val).AccessibleName;
				((Control)_22722).Left = ((Control)val).Left;
				((Control)_22722).Visible = ((Control)val).Visible;
				((Control)_22722).RightToLeft = ((Control)val).RightToLeft;
				((ScrollBar)_22722).Scroll += (ScrollEventHandler)Delegate.CreateDelegate(typeof(ScrollEventHandler), this, method);
				field.SetValue(this, _22722);
				((Component)(object)val).Dispose();
				((Control)this).Controls.Remove((Control)(object)val);
				((Control)this).Controls.Add((Control)(object)_22722);
				((Control)this).PerformLayout();
			}
		}
	}
}
