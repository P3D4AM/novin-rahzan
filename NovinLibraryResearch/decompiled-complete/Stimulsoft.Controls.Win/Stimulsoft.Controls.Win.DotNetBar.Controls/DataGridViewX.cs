using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(DataGridViewX), "Controls.DataGridViewX.bmp")]
[ToolboxItem(false)]
public class DataGridViewX : DataGridView
{
	private struct SelectionInfo
	{
		public bool Selected;

		public int ColumnIndex;
	}

	private int m_MouseOverColumnHeader = -2;

	private int m_MouseDownColumnHeader = -2;

	private int m_MouseDownRowIndex = -2;

	private int m_MouseOverRowIndex = -2;

	private bool m_Office2007StyleEnabled = true;

	private Office2007DataGridViewColorTable m_ColorTable;

	private Office2007ButtonItemStateColorTable m_ButtonStateColorTable;

	private SelectionInfo[] m_ColumnSelectionState = new SelectionInfo[1];

	private int m_SelectedRowIndex = -2;

	private bool m_HighlightSelectedColumnHeaders = true;

	private bool m_SelectAllSignVisible = true;

	private bool m_PaintEnhancedSelection = true;

	private eScrollBarAppearance _ScrollBarAppearance = eScrollBarAppearance.ApplicationScroll;

	internal Office2007ButtonItemStateColorTable ButtonStateColorTable => m_ButtonStateColorTable;

	[DefaultValue(eScrollBarAppearance.ApplicationScroll)]
	[Category("Appearance")]
	[Description("Gets or sets the scroll-bar visual style.")]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ScrollBarAppearance;
		}
		set
		{
			_ScrollBarAppearance = value;
			OnScrollBarAppearance();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether selected column header is highlighted.")]
	public bool HighlightSelectedColumnHeaders
	{
		get
		{
			return m_HighlightSelectedColumnHeaders;
		}
		set
		{
			m_HighlightSelectedColumnHeaders = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether select all sign displayed in top-left corner of the grid is visible.")]
	public bool SelectAllSignVisible
	{
		get
		{
			return m_SelectAllSignVisible;
		}
		set
		{
			m_SelectAllSignVisible = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether enhanced selection for the cells is painted in Office 2007 style")]
	public bool PaintEnhancedSelection
	{
		get
		{
			return m_PaintEnhancedSelection;
		}
		set
		{
			m_PaintEnhancedSelection = value;
			if (BarFunctions.IsHandleValid((Control)(object)this))
			{
				((Control)this).Invalidate();
			}
		}
	}

	public DataGridViewX()
	{
		SetupScrollBars();
		((Control)this).SetStyle((ControlStyles)139280, true);
		((Control)this).DoubleBuffered = true;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		DGVScrollBar vScrollBar = GetVScrollBar();
		if (vScrollBar != null && ((Control)vScrollBar).Visible)
		{
			((Control)vScrollBar).Invalidate();
		}
		DGHScrollBar hScrollBar = GetHScrollBar();
		if (hScrollBar != null && ((Control)hScrollBar).Visible)
		{
			((Control)hScrollBar).Invalidate();
		}
		Office2007ColorTable office2007ColorTable = null;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			office2007ColorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
			m_ColorTable = office2007ColorTable.DataGridView;
			m_ButtonStateColorTable = office2007ColorTable.ButtonItemColors[0].Checked;
		}
		try
		{
			((DataGridView)this).BackgroundColor = m_ColorTable.BackgroundColor;
			((DataGridView)this).ColumnHeadersDefaultCellStyle.ForeColor = m_ColorTable.ColumnHeaderNormalText;
			((DataGridView)this).ColumnHeadersDefaultCellStyle.SelectionForeColor = m_ColorTable.ColumnHeaderSelectedText;
			((DataGridView)this).DefaultCellStyle.BackColor = m_ColorTable.DefaultCellBackground;
			((DataGridView)this).DefaultCellStyle.ForeColor = m_ColorTable.DefaultCellText;
			if (PaintEnhancedSelection)
			{
				((DataGridView)this).DefaultCellStyle.SelectionForeColor = m_ButtonStateColorTable.Text;
			}
			((DataGridView)this).GridColor = m_ColorTable.GridColor;
			((DataGridView)this).RowHeadersDefaultCellStyle.ForeColor = m_ColorTable.ColumnHeaderNormalText;
			((DataGridView)this).RowHeadersDefaultCellStyle.SelectionForeColor = m_ColorTable.ColumnHeaderSelectedText;
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
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Invalid comparison between Unknown and I4
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Invalid comparison between Unknown and I4
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		bool enabled = ((Control)this).Enabled;
		Office2007DataGridViewColorTable colorTable = GetColorTable();
		if (!m_Office2007StyleEnabled || colorTable == null || ((HandledEventArgs)(object)e).Handled || (e.ColumnIndex >= 0 && e.RowIndex >= 0 && (!m_PaintEnhancedSelection || (e.State & 0x20) != 32)))
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
			if (m_MouseOverColumnHeader == -1)
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
			empty = ((m_MouseOverColumnHeader != -1) ? colorTable.SelectorBorderLight : colorTable.SelectorMouseOverBorderLight);
			Rectangle inner = cellBounds;
			inner.Inflate(-1, -1);
			Pen val2 = new Pen(empty);
			try
			{
				graphics.DrawLine(val2, inner.X, inner.Bottom - 1, inner.X, inner.Y);
				graphics.DrawLine(val2, inner.X, inner.Y, inner.Right - 1, inner.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			empty = ((m_MouseOverColumnHeader != -1) ? colorTable.SelectorBorderDark : colorTable.SelectorMouseOverBorderDark);
			Pen val3 = new Pen(empty);
			try
			{
				graphics.DrawLine(val3, inner.Right - 1, inner.Y, inner.Right - 1, inner.Bottom - 1);
				graphics.DrawLine(val3, inner.X, inner.Bottom - 1, inner.Right - 1, inner.Bottom - 1);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if (m_SelectAllSignVisible)
			{
				GraphicsPath selectorPath = GetSelectorPath(inner);
				if (selectorPath != null)
				{
					DisplayHelp.FillPath(graphics, selectorPath, (m_MouseOverColumnHeader == -1) ? colorTable.SelectorMouseOverSign : colorTable.SelectorSign);
					selectorPath.Dispose();
				}
			}
		}
		else if (e.ColumnIndex == -1)
		{
			linearGradientColorTable = colorTable.RowNormalBackground;
			empty = colorTable.RowNormalBorder;
			if (m_MouseDownRowIndex == e.RowIndex)
			{
				linearGradientColorTable = colorTable.RowPressedBackground;
				empty = colorTable.RowPressedBorder;
			}
			else if (m_SelectedRowIndex == e.RowIndex && enabled)
			{
				if (m_MouseOverRowIndex == e.RowIndex)
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
				if (m_MouseOverRowIndex == e.RowIndex)
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
			else if (m_MouseOverRowIndex == e.RowIndex)
			{
				linearGradientColorTable = colorTable.RowMouseOverBackground;
				empty = colorTable.RowMouseOverBorder;
			}
			DisplayHelp.FillRectangle(graphics, cellBounds, linearGradientColorTable);
			Pen val4 = new Pen(empty);
			try
			{
				graphics.DrawLine(val4, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
				if (m_SelectedRowIndex == e.RowIndex + 1 && enabled)
				{
					Color color = colorTable.RowSelectedBorder;
					if (m_MouseDownRowIndex == e.RowIndex + 1)
					{
						color = colorTable.RowPressedBorder;
					}
					else if (m_MouseOverRowIndex == e.RowIndex + 1)
					{
						color = colorTable.RowSelectedMouseOverBorder;
					}
					Pen val5 = new Pen(color);
					try
					{
						graphics.DrawLine(val5, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				else
				{
					graphics.DrawLine(val4, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			e.PaintContent(cellBounds);
		}
		else if (e.RowIndex == -1)
		{
			if (m_MouseDownColumnHeader == e.ColumnIndex)
			{
				linearGradientColorTable = colorTable.ColumnHeaderPressedBackground;
				empty = colorTable.ColumnHeaderPressedBorder;
			}
			else if (m_MouseOverColumnHeader == e.ColumnIndex)
			{
				if (m_HighlightSelectedColumnHeaders && num >= 0 && e.ColumnIndex >= 0 && ((m_ColumnSelectionState.Length > num && m_ColumnSelectionState[num].Selected) || ((DataGridViewBand)((DataGridView)this).Columns[e.ColumnIndex]).Selected))
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
			else if (!m_HighlightSelectedColumnHeaders)
			{
				linearGradientColorTable = colorTable.ColumnHeaderNormalBackground;
				empty = colorTable.ColumnHeaderNormalBorder;
			}
			else if (num >= 0 && e.ColumnIndex >= 0 && ((m_ColumnSelectionState.Length > num && m_ColumnSelectionState[num].Selected) || ((DataGridViewBand)((DataGridView)this).Columns[e.ColumnIndex]).Selected) && enabled)
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
			Pen val6 = new Pen(empty);
			try
			{
				graphics.DrawLine(val6, cellBounds.X, cellBounds.Bottom - 1, cellBounds.Right - 1, cellBounds.Bottom - 1);
				if (enabled && m_HighlightSelectedColumnHeaders && num >= 0 && m_ColumnSelectionState.Length > num + 1 && (m_ColumnSelectionState[num + 1].Selected || m_ColumnSelectionState[num + 1].ColumnIndex == m_MouseDownColumnHeader))
				{
					Color color2 = colorTable.ColumnHeaderSelectedBorder;
					if (m_ColumnSelectionState[num + 1].ColumnIndex == m_MouseDownColumnHeader)
					{
						color2 = colorTable.ColumnHeaderPressedBorder;
					}
					else if (m_MouseOverColumnHeader == num + 1)
					{
						color2 = colorTable.ColumnHeaderSelectedMouseOverBorder;
					}
					Pen val7 = new Pen(color2);
					try
					{
						graphics.DrawLine(val7, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				else
				{
					graphics.DrawLine(val6, cellBounds.Right - 1, cellBounds.Y, cellBounds.Right - 1, cellBounds.Bottom - 1);
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			e.PaintContent(cellBounds);
		}
		else if ((e.State & 0x20) == 32)
		{
			if (((DataGridView)this).Columns[e.ColumnIndex] is IDataGridViewColumn { OwnerPaintCell: not false })
			{
				((DataGridView)this).OnCellPainting(e);
				return;
			}
			e.PaintBackground(cellBounds, false);
			if (enabled)
			{
				cellBounds.Height--;
				Office2007ButtonItemPainter.PaintBackground(graphics, m_ButtonStateColorTable, cellBounds, RoundRectangleShapeDescriptor.RectangleShape, isDefault: false, paintBorder: false);
				cellBounds.Height++;
				if (((DataGridView)this).CurrentCellAddress.X == e.ColumnIndex && ((DataGridView)this).CurrentCellAddress.Y == e.RowIndex && (e.PaintParts & 0x20) == 32 && ((Control)this).ShowFocusCues && ((Control)this).Focused && cellBounds.Width > 0 && cellBounds.Height > 0)
				{
					if (m_ButtonStateColorTable.TopBackground != null)
					{
						ControlPaint.DrawFocusRectangle(graphics, cellBounds, Color.Empty, m_ButtonStateColorTable.TopBackground.End);
					}
					else if (m_ButtonStateColorTable.Background != null)
					{
						ControlPaint.DrawFocusRectangle(graphics, cellBounds, Color.Empty, m_ButtonStateColorTable.Background.Start);
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

	private GraphicsPath GetSelectorPath(Rectangle inner)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		inner.Inflate(-3, -3);
		if (inner.Width > 2 && inner.Height > 2)
		{
			GraphicsPath val = new GraphicsPath();
			val.AddLine(inner.Right, inner.Y, inner.Right, inner.Bottom);
			val.AddLine(inner.Right, inner.Bottom, inner.Right - inner.Height, inner.Bottom);
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
			UpdateOffice2007Styles(dataGridView);
		}
		((DataGridView)this).OnHandleCreated(e);
	}

	private void UpdateOffice2007Styles(Office2007DataGridViewColorTable ct)
	{
		if (((DataGridView)this).GridColor != ct.GridColor)
		{
			((DataGridView)this).GridColor = ct.GridColor;
		}
		if (m_PaintEnhancedSelection)
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
			m_ColorTable = office2007ColorTable.DataGridView;
			m_ButtonStateColorTable = office2007ColorTable.ButtonItemColors[0].Checked;
		}
		if (((DataGridView)this).CurrentCell != null)
		{
			m_SelectedRowIndex = ((DataGridView)this).CurrentCell.RowIndex;
		}
		else
		{
			m_SelectedRowIndex = -1;
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
		m_ColorTable = null;
		m_ButtonStateColorTable = null;
	}

	protected override void OnCurrentCellChanged(EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((DataGridView)this).SelectionMode == 1)
		{
			UpdateSelectionState();
		}
		((DataGridView)this).OnCurrentCellChanged(e);
	}

	protected override void OnSelectionChanged(EventArgs e)
	{
		UpdateSelectionState();
		((DataGridView)this).OnSelectionChanged(e);
	}

	protected override void OnDataSourceChanged(EventArgs e)
	{
		((DataGridView)this).OnDataSourceChanged(e);
		UpdateSelectionState();
	}

	private void UpdateSelectionState()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		SelectionInfo[] array = new SelectionInfo[((BaseCollection)((DataGridView)this).Columns).Count];
		for (int i = 0; i < ((BaseCollection)((DataGridView)this).Columns).Count; i++)
		{
			array[((DataGridView)this).Columns[i].DisplayIndex].ColumnIndex = i;
		}
		int num = ((BaseCollection)((DataGridView)this).Columns).Count;
		if ((int)((DataGridView)this).SelectionMode == 1)
		{
			if (((DataGridView)this).CurrentCell != null)
			{
				int displayIndex = ((DataGridView)this).Columns[((DataGridView)this).CurrentCell.ColumnIndex].DisplayIndex;
				array[displayIndex].Selected = true;
				array[displayIndex].ColumnIndex = ((DataGridView)this).CurrentCell.ColumnIndex;
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
				if (!array[displayIndex2].Selected)
				{
					num--;
					array[displayIndex2].Selected = true;
					array[displayIndex2].ColumnIndex = val.ColumnIndex;
					if (num == 0)
					{
						break;
					}
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (m_ColumnSelectionState.Length <= j || array[j].Selected == m_ColumnSelectionState[j].Selected)
			{
				continue;
			}
			int columnIndex = m_ColumnSelectionState[j].ColumnIndex;
			if (columnIndex >= 0 && columnIndex < ((BaseCollection)((DataGridView)this).Columns).Count)
			{
				((DataGridView)this).InvalidateColumn(columnIndex);
				if (columnIndex > 0)
				{
					((DataGridView)this).InvalidateColumn(columnIndex - 1);
				}
			}
		}
		if (m_SelectedRowIndex > 0 && m_SelectedRowIndex < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).InvalidateRow(m_SelectedRowIndex - 1);
		}
		if (((DataGridView)this).CurrentCell != null && ((DataGridView)this).CurrentCell.RowIndex > 0)
		{
			m_SelectedRowIndex = ((DataGridView)this).CurrentCell.RowIndex;
			((DataGridView)this).InvalidateRow(m_SelectedRowIndex - 1);
		}
		else
		{
			m_SelectedRowIndex = -2;
		}
		m_ColumnSelectionState = array;
	}

	protected virtual Office2007DataGridViewColorTable GetColorTable()
	{
		return m_ColorTable;
	}

	protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
	{
		if (e.ColumnIndex == -1 || e.RowIndex == -1)
		{
			((Control)this).DoubleBuffered = false;
		}
		if (e.RowIndex == -1)
		{
			m_MouseDownColumnHeader = e.ColumnIndex;
			if (e.ColumnIndex >= 0)
			{
				InvalidatePreviousColumn(((DataGridView)this).Columns[e.ColumnIndex].DisplayIndex);
			}
		}
		else
		{
			m_MouseDownColumnHeader = -2;
		}
		if (e.ColumnIndex == -1)
		{
			m_MouseDownRowIndex = e.RowIndex;
			if (m_MouseDownRowIndex > 0 && m_MouseDownRowIndex < ((DataGridView)this).Rows.Count)
			{
				((DataGridView)this).InvalidateRow(m_MouseDownRowIndex - 1);
			}
		}
		else
		{
			m_MouseDownRowIndex = -2;
		}
		((DataGridView)this).OnCellMouseDown(e);
	}

	private void InvalidatePreviousColumn(int displayIndex)
	{
		displayIndex--;
		for (int i = 0; i < ((BaseCollection)((DataGridView)this).Columns).Count; i++)
		{
			DataGridViewColumn val = ((DataGridView)this).Columns[i];
			if (((DataGridViewBand)val).Displayed && val.DisplayIndex == displayIndex)
			{
				((DataGridView)this).InvalidateColumn(i);
				break;
			}
		}
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		CleanUpOnCellMouseUp();
		((Control)this).OnDragDrop(drgevent);
	}

	private void CleanUpOnCellMouseUp()
	{
		if (!((Control)this).DoubleBuffered)
		{
			((Control)this).DoubleBuffered = true;
		}
		if (m_MouseDownRowIndex > 0 && m_MouseDownRowIndex < ((DataGridView)this).Rows.Count)
		{
			((DataGridView)this).InvalidateRow(m_MouseDownRowIndex - 1);
		}
		if (m_MouseDownRowIndex >= 0)
		{
			((DataGridView)this).InvalidateRow(m_MouseDownRowIndex);
		}
		m_MouseDownRowIndex = -2;
		m_MouseDownColumnHeader = -2;
	}

	protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
	{
		CleanUpOnCellMouseUp();
		((DataGridView)this).OnCellMouseUp(e);
	}

	protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
	{
		if (e.RowIndex == -1)
		{
			m_MouseOverColumnHeader = e.ColumnIndex;
		}
		else
		{
			m_MouseOverColumnHeader = -2;
		}
		if (e.ColumnIndex == -1)
		{
			if (m_MouseOverRowIndex != e.RowIndex)
			{
				m_MouseOverRowIndex = e.RowIndex;
				if (m_MouseOverRowIndex > 0)
				{
					((DataGridView)this).InvalidateRow(m_MouseOverRowIndex - 1);
				}
			}
		}
		else
		{
			m_MouseOverRowIndex = -2;
		}
		((DataGridView)this).OnCellMouseEnter(e);
	}

	protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
	{
		m_MouseOverColumnHeader = -2;
		m_MouseOverRowIndex = -2;
		((DataGridView)this).OnCellMouseLeave(e);
	}

	protected override void OnScroll(ScrollEventArgs e)
	{
		((DataGridView)this).OnScroll(e);
		DGVScrollBar vScrollBar = GetVScrollBar();
		if (vScrollBar != null && ((Control)vScrollBar).Visible)
		{
			vScrollBar.UpdateScrollValues();
		}
		DGHScrollBar hScrollBar = GetHScrollBar();
		if (hScrollBar != null && ((Control)hScrollBar).Visible)
		{
			hScrollBar.UpdateScrollValues();
		}
	}

	private DGVScrollBar GetVScrollBar()
	{
		return ((DataGridView)this).VerticalScrollBar as DGVScrollBar;
	}

	private DGHScrollBar GetHScrollBar()
	{
		return ((DataGridView)this).HorizontalScrollBar as DGHScrollBar;
	}

	private void InvokeDelayed(MethodInvoker method)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Timer val = new Timer();
		val = new Timer();
		val.Tag = method;
		val.Interval = 10;
		val.Tick += DelayedInvokeTimerTick;
		val.Start();
	}

	private void DelayedInvokeTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		Timer val = (Timer)sender;
		MethodInvoker val2 = (MethodInvoker)val.Tag;
		val.Stop();
		((Component)(object)val).Dispose();
		val2.Invoke();
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Form val = ((Control)this).FindForm();
		if (val != null && val.IsMdiChild)
		{
			InvokeDelayed((MethodInvoker)delegate
			{
				CallBaseOnResize(e);
			});
		}
		else
		{
			((DataGridView)this).OnResize(e);
		}
	}

	private void CallBaseOnResize(EventArgs e)
	{
		((DataGridView)this).OnResize(e);
	}

	private void OnScrollBarAppearance()
	{
		DGVScrollBar vScrollBar = GetVScrollBar();
		if (vScrollBar != null)
		{
			vScrollBar.AppStyleScrollBar = _ScrollBarAppearance == eScrollBarAppearance.ApplicationScroll;
		}
		DGHScrollBar hScrollBar = GetHScrollBar();
		if (hScrollBar != null)
		{
			hScrollBar.AppStyleScrollBar = _ScrollBarAppearance == eScrollBarAppearance.ApplicationScroll;
		}
	}

	private void SetupScrollBars()
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		Type typeFromHandle = typeof(DataGridView);
		FieldInfo field = typeFromHandle.GetField("vertScrollBar", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
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
		if (method == null)
		{
			return;
		}
		DGVScrollBar dGVScrollBar = new DGVScrollBar();
		dGVScrollBar.AppStyleScrollBar = _ScrollBarAppearance == eScrollBarAppearance.ApplicationScroll;
		((ScrollBar)dGVScrollBar).Minimum = val.Minimum;
		((ScrollBar)dGVScrollBar).Maximum = val.Maximum;
		((ScrollBar)dGVScrollBar).SmallChange = val.SmallChange;
		((ScrollBar)dGVScrollBar).LargeChange = val.LargeChange;
		((Control)dGVScrollBar).Top = ((Control)val).Top;
		((Control)dGVScrollBar).AccessibleName = ((Control)val).AccessibleName;
		((Control)dGVScrollBar).Left = ((Control)val).Left;
		((Control)dGVScrollBar).Visible = ((Control)val).Visible;
		((ScrollBar)dGVScrollBar).Scroll += (ScrollEventHandler)Delegate.CreateDelegate(typeof(ScrollEventHandler), this, method);
		field.SetValue(this, dGVScrollBar);
		((Component)(object)val).Dispose();
		((Control)this).Controls.Remove((Control)(object)val);
		((Control)this).Controls.Add((Control)(object)dGVScrollBar);
		field = typeFromHandle.GetField("horizScrollBar", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			return;
		}
		object? value2 = field.GetValue(this);
		val = (ScrollBar)((value2 is ScrollBar) ? value2 : null);
		if (val != null)
		{
			method = typeFromHandle.GetMethod("DataGridViewHScrolled", BindingFlags.Instance | BindingFlags.NonPublic);
			if (!(method == null))
			{
				DGHScrollBar dGHScrollBar = new DGHScrollBar();
				dGHScrollBar.AppStyleScrollBar = _ScrollBarAppearance == eScrollBarAppearance.ApplicationScroll;
				((ScrollBar)dGHScrollBar).Minimum = val.Minimum;
				((ScrollBar)dGHScrollBar).Maximum = val.Maximum;
				((ScrollBar)dGHScrollBar).SmallChange = val.SmallChange;
				((ScrollBar)dGHScrollBar).LargeChange = val.LargeChange;
				((Control)dGHScrollBar).Top = ((Control)val).Top;
				((Control)dGHScrollBar).AccessibleName = ((Control)val).AccessibleName;
				((Control)dGHScrollBar).Left = ((Control)val).Left;
				((Control)dGHScrollBar).Visible = ((Control)val).Visible;
				((Control)dGHScrollBar).RightToLeft = ((Control)val).RightToLeft;
				((ScrollBar)dGHScrollBar).Scroll += (ScrollEventHandler)Delegate.CreateDelegate(typeof(ScrollEventHandler), this, method);
				field.SetValue(this, dGHScrollBar);
				((Component)(object)val).Dispose();
				((Control)this).Controls.Remove((Control)(object)val);
				((Control)this).Controls.Add((Control)(object)dGHScrollBar);
				((Control)this).PerformLayout();
			}
		}
	}
}
