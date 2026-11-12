using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Controls.Menu;

namespace Stimulsoft.Controls;

public class StiExcelGrid : StiDataGridView
{
	private bool skipValidatingOnce;

	private int minColumnIndex = -1;

	private int maxColumnIndex = -1;

	private int prevRowIndex = -1;

	private StiContextMenu menuHeader = new StiContextMenu();

	private StiContextMenuItem itemSetAll;

	private StiContextMenuItem itemLoadImage;

	private StiContextMenuItem itemRemoveImage;

	private StiSeparatorContextMenuItem itemOperationSeparator;

	private StiContextMenuItem itemInsertRowAbove;

	private StiContextMenuItem itemInsertRowBelow;

	private StiSeparatorContextMenuItem itemInsertSeparator;

	private StiContextMenuItem itemMoveRowUp;

	private StiContextMenuItem itemMoveRowDown;

	private StiSeparatorContextMenuItem itemMoveSeparator;

	private StiContextMenuItem itemDeleteRow;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfFileImageFilter { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemLoadImage
	{
		get
		{
			return ((ToolStripItem)itemLoadImage).Text;
		}
		set
		{
			((ToolStripItem)itemLoadImage).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemRemoveImage
	{
		get
		{
			return ((ToolStripItem)itemRemoveImage).Text;
		}
		set
		{
			((ToolStripItem)itemRemoveImage).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemSetAll
	{
		get
		{
			return ((ToolStripItem)itemSetAll).Text;
		}
		set
		{
			((ToolStripItem)itemSetAll).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemInsertRowAbove
	{
		get
		{
			return ((ToolStripItem)itemInsertRowAbove).Text;
		}
		set
		{
			((ToolStripItem)itemInsertRowAbove).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemInsertRowBelow
	{
		get
		{
			return ((ToolStripItem)itemInsertRowBelow).Text;
		}
		set
		{
			((ToolStripItem)itemInsertRowBelow).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemMoveRowUp
	{
		get
		{
			return ((ToolStripItem)itemMoveRowUp).Text;
		}
		set
		{
			((ToolStripItem)itemMoveRowUp).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemMoveRowDown
	{
		get
		{
			return ((ToolStripItem)itemMoveRowDown).Text;
		}
		set
		{
			((ToolStripItem)itemMoveRowDown).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LocalizationOfItemDeleteRow
	{
		get
		{
			return ((ToolStripItem)itemDeleteRow).Text;
		}
		set
		{
			((ToolStripItem)itemDeleteRow).Text = value;
		}
	}

	private List<DataGridViewCell> SelectedCellsList
	{
		get
		{
			if (((DataGridView)this).SelectedCells == null)
			{
				return new List<DataGridViewCell>();
			}
			return ((IEnumerable)((DataGridView)this).SelectedCells).Cast<DataGridViewCell>().ToList();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] EmptyKeys { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDeleteSelectedArea { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDeleteRow { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowInsertRow { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowMoveRow { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AddNewRowWhenPressingDown { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AddNewRowAfterEditing { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowOperationsForImageColumns { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowSelectUnselectItemsForBoolColumns { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DataTable DataTable => ((DataGridView)this).DataSource as DataTable;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int ColumnHeadersHeight
	{
		get
		{
			return StiScale.I(22);
		}
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public int RowHeadersWidth
	{
		get
		{
			return StiScale.I(30);
		}
		set
		{
		}
	}

	public event EventHandler CellRowsChanged;

	public void InvokeCellRowsChanged()
	{
		this.CellRowsChanged?.Invoke(this, EventArgs.Empty);
	}

	public DataGridViewColumn AddCheckBoxColumn(string name, string headerText, int width, DataGridViewContentAlignment alignment = 32)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewColumn val = new DataGridViewColumn
		{
			Name = name,
			HeaderText = headerText,
			DataPropertyName = name,
			CellTemplate = (DataGridViewCell)new DataGridViewCheckBoxCell(),
			Frozen = false,
			Width = width
		};
		((DataGridViewBand)val).DefaultCellStyle.Alignment = alignment;
		((DataGridViewBand)val).DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((DataGridView)this).Columns.Add(val);
		return val;
	}

	public DataGridViewColumn AddTextBoxColumn(string name, string headerText, int width = 0, DataGridViewContentAlignment alignment = 64)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewColumn val = new DataGridViewColumn
		{
			Name = name,
			HeaderText = headerText,
			DataPropertyName = name,
			CellTemplate = (DataGridViewCell)new DataGridViewTextBoxCell(),
			Frozen = false,
			Width = width
		};
		((DataGridViewBand)val).DefaultCellStyle.Alignment = alignment;
		((DataGridViewBand)val).DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((DataGridView)this).Columns.Add(val);
		return val;
	}

	public DataGridViewColumn AddImageColumn(string name, string headerText, int width, DataGridViewContentAlignment alignment)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		StiExcelImageColumn stiExcelImageColumn = new StiExcelImageColumn();
		((DataGridViewColumn)stiExcelImageColumn).Name = name;
		((DataGridViewColumn)stiExcelImageColumn).HeaderText = headerText;
		((DataGridViewColumn)stiExcelImageColumn).DataPropertyName = name;
		((DataGridViewImageColumn)stiExcelImageColumn).ImageLayout = (DataGridViewImageCellLayout)1;
		((DataGridViewBand)stiExcelImageColumn).Frozen = false;
		((DataGridViewColumn)stiExcelImageColumn).Width = width;
		StiExcelImageColumn stiExcelImageColumn2 = stiExcelImageColumn;
		((DataGridViewBand)stiExcelImageColumn2).DefaultCellStyle.Alignment = alignment;
		((DataGridViewBand)stiExcelImageColumn2).DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((DataGridView)this).Columns.Add((DataGridViewColumn)(object)stiExcelImageColumn2);
		return (DataGridViewColumn)(object)stiExcelImageColumn2;
	}

	public DataGridViewContentAlignment GetAlignment(Type type)
	{
		if (!type.IsNumericType())
		{
			if (!(type == typeof(DateTime)) && !(type == typeof(TimeSpan)))
			{
				return (DataGridViewContentAlignment)32;
			}
			return (DataGridViewContentAlignment)16;
		}
		return (DataGridViewContentAlignment)64;
	}

	public int MeasureHeader(string str)
	{
		return TextRenderer.MeasureText(str, ((Control)this).Font).Width;
	}

	private bool AddNewEmptyRowIfRequired()
	{
		if (DataTable == null)
		{
			return false;
		}
		if (!SelectedCellsList.Any())
		{
			return false;
		}
		if (GetSelectionRect().Height > 1)
		{
			return false;
		}
		int num = SelectedCellsList.Max((DataGridViewCell c) => c.RowIndex);
		if (num + 1 < DataTable.Rows.Count)
		{
			return false;
		}
		AddNewEmptyRow();
		return true;
	}

	private void RefreshHeaderMenu()
	{
		((ToolStrip)menuHeader).Items.Clear();
		Rectangle selectionRect = GetSelectionRect();
		if (ShowSelectUnselectItemsForBoolColumns)
		{
			DataGridViewColumn val = ((DataGridView)this).Columns[selectionRect.X];
			DataColumn dataColumn = ((DataTable != null) ? DataTable.Columns[val.DataPropertyName] : null);
			if (dataColumn != null && dataColumn.DataType == typeof(bool))
			{
				((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemSetAll);
				IEnumerable<DataRow> source = DataTable.Rows.Cast<DataRow>().Skip(selectionRect.Y).Take(selectionRect.Bottom - selectionRect.Y);
				if (source.All((DataRow r) => StiValueHelper.TryToBool(r[dataColumn])))
				{
					((ToolStripMenuItem)itemSetAll).CheckState = (CheckState)1;
				}
				else if (source.All((DataRow r) => !StiValueHelper.TryToBool(r[dataColumn])))
				{
					((ToolStripMenuItem)itemSetAll).CheckState = (CheckState)0;
				}
				else
				{
					((ToolStripMenuItem)itemSetAll).CheckState = (CheckState)2;
				}
				((ToolStripItem)itemSetAll).Tag = dataColumn.ColumnName;
			}
		}
		if (ShowOperationsForImageColumns)
		{
			DataGridViewColumn val2 = ((DataGridView)this).Columns[selectionRect.X];
			DataColumn dataColumn2 = DataTable?.Columns[val2.DataPropertyName];
			if (dataColumn2 != null && dataColumn2.DataType == typeof(Image))
			{
				object obj = DataTable.Rows[selectionRect.Y][dataColumn2];
				((ToolStripItem)itemRemoveImage).Enabled = obj != null && obj != DBNull.Value;
				((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemLoadImage);
				((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemRemoveImage);
			}
		}
		if (((ArrangedElementCollection)((ToolStrip)menuHeader).Items).Count > 0)
		{
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemOperationSeparator);
		}
		if (AllowInsertRow)
		{
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemInsertRowAbove);
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemInsertRowBelow);
		}
		if (AllowMoveRow)
		{
			if (((ArrangedElementCollection)((ToolStrip)menuHeader).Items).Count > 0)
			{
				((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemInsertSeparator);
			}
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemMoveRowUp);
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemMoveRowDown);
		}
		if (AllowDeleteRow)
		{
			if (((ArrangedElementCollection)((ToolStrip)menuHeader).Items).Count > 0)
			{
				((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemMoveSeparator);
			}
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemDeleteRow);
		}
		((ToolStripItem)itemMoveRowUp).Enabled = selectionRect.Y > 0;
		((ToolStripItem)itemMoveRowDown).Enabled = DataTable != null && selectionRect.Bottom < DataTable.Rows.Count;
	}

	private void CreateHeaderMenu()
	{
		menuHeader = new StiContextMenu();
		itemLoadImage = new StiContextMenuItem("Load Image");
		((ToolStripItem)itemLoadImage).Click += delegate
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Invalid comparison between Unknown and I4
			((ToolStripDropDown)menuHeader).Close();
			OpenFileDialog val2 = new OpenFileDialog();
			try
			{
				if (!string.IsNullOrWhiteSpace(LocalizationOfFileImageFilter))
				{
					((FileDialog)val2).Filter = LocalizationOfFileImageFilter;
				}
				else
				{
					string text = string.Join(";", new string[10] { "gif", "png", "jpeg", "jpg", "bmp", "tiff", "ico", "emf", "wmf", "svg" }.Select((string s) => "*." + s));
					((FileDialog)val2).Filter = "AllImageFiles|" + text;
				}
				((FileDialog)val2).RestoreDirectory = true;
				if ((int)((CommonDialog)val2).ShowDialog() == 1)
				{
					Rectangle selectionRect3 = GetSelectionRect();
					try
					{
						Image val3 = Image.FromFile(((FileDialog)val2).FileName);
						try
						{
							for (int j = selectionRect3.Y; j < selectionRect3.Bottom; j++)
							{
								DataGridViewColumn val4 = ((DataGridView)this).Columns[selectionRect3.X];
								DataColumn column2 = ((DataTable != null) ? DataTable.Columns[val4.DataPropertyName] : null);
								DataRow dataRow = DataTable.Rows[j];
								object obj = val3.Clone();
								dataRow[column2] = ((obj is Image) ? obj : null);
							}
							return;
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return;
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		};
		itemRemoveImage = new StiContextMenuItem("Remove Image");
		((ToolStripItem)itemRemoveImage).Click += delegate
		{
			Rectangle selectionRect2 = GetSelectionRect();
			for (int i = selectionRect2.Y; i < selectionRect2.Bottom; i++)
			{
				DataGridViewColumn val = ((DataGridView)this).Columns[selectionRect2.X];
				DataColumn column = ((DataTable != null) ? DataTable.Columns[val.DataPropertyName] : null);
				DataTable.Rows[i][column] = DBNull.Value;
			}
			((ToolStripDropDown)menuHeader).Close();
		};
		itemSetAll = new StiContextMenuItem("Set All");
		((ToolStripMenuItem)itemSetAll).CheckState = (CheckState)2;
		((ToolStripItem)itemSetAll).Click += delegate
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Invalid comparison between Unknown and I4
			bool value = (int)((ToolStripMenuItem)itemSetAll).CheckState == 0 || (int)((ToolStripMenuItem)itemSetAll).CheckState == 2;
			((ToolStrip)menuHeader).Items.Add((ToolStripItem)(object)itemSetAll);
			Rectangle selectionRect = GetSelectionRect();
			DataTable.Rows.Cast<DataRow>().Skip(selectionRect.Y).Take(selectionRect.Bottom - selectionRect.Y)
				.ToList()
				.ForEach(delegate(DataRow r)
				{
					r[((ToolStripItem)itemSetAll).Tag as string] = value;
				});
			((ToolStripDropDown)menuHeader).Close();
		};
		itemOperationSeparator = new StiSeparatorContextMenuItem();
		itemInsertRowAbove = new StiContextMenuItem("Insert Row Above");
		((ToolStripItem)itemInsertRowAbove).Click += delegate
		{
			InsertRowAboveCurrent();
			((ToolStripDropDown)menuHeader).Close();
		};
		itemInsertRowBelow = new StiContextMenuItem("Insert Row Below");
		((ToolStripItem)itemInsertRowBelow).Click += delegate
		{
			InsertRowBelowCurrent();
			((ToolStripDropDown)menuHeader).Close();
		};
		itemInsertSeparator = new StiSeparatorContextMenuItem();
		itemMoveRowUp = new StiContextMenuItem("Move Row Up");
		((ToolStripItem)itemMoveRowUp).Click += delegate
		{
			MoveCurrentRowUp();
			((ToolStripDropDown)menuHeader).Close();
		};
		itemMoveRowDown = new StiContextMenuItem("Move Row Down");
		((ToolStripItem)itemMoveRowDown).Click += delegate
		{
			MoveCurrentRowDown();
			((ToolStripDropDown)menuHeader).Close();
		};
		itemMoveSeparator = new StiSeparatorContextMenuItem();
		itemDeleteRow = new StiContextMenuItem("Delete Row");
		((ToolStripItem)itemDeleteRow).Click += delegate
		{
			DeleteCurrentRow();
			((ToolStripDropDown)menuHeader).Close();
		};
	}

	private void DrawHeaderCell(DataGridViewCellPaintingEventArgs e)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		((HandledEventArgs)(object)e).Handled = true;
		e.Paint(e.CellBounds, (DataGridViewPaintParts)101);
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		bool flag = e.CellBounds.Contains(pt);
		Color color = ColorTranslator.FromHtml(flag ? "#ffffff" : "#f0f0f0");
		if (StiUX.IsDark)
		{
			color = (flag ? StiUX.ButtonBackgroundHover : StiUX.ButtonBackground);
		}
		if (SelectedCellsList.Any((DataGridViewCell c) => c.RowIndex == e.RowIndex || c.ColumnIndex == e.ColumnIndex))
		{
			color = StiColorUtils.Dark(color, 30);
			if (StiUX.IsDark)
			{
				color = StiUX.Accent;
			}
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			graphics.FillRectangle((Brush)(object)val, e.CellBounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		DrawBorder(e);
		DrawHeaderText(e);
	}

	private void DrawDataCell(DataGridViewCellPaintingEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewPaintParts val = (DataGridViewPaintParts)127;
		val = (DataGridViewPaintParts)(val - 16);
		if (!((Enum)e.State).HasFlag((Enum)(object)(DataGridViewElementStates)32))
		{
			bool flag = !string.IsNullOrWhiteSpace(e.ErrorText) && ((DataGridView)this).ShowCellErrors;
			bool flag2 = IsEmptyRow(e.RowIndex);
			if (flag2 || flag)
			{
				val = (DataGridViewPaintParts)(val - 1);
				Color color = (StiUX.IsDark ? StiUX.ItemBackground : StiColorUtils.Dark(e.CellStyle.BackColor, 8));
				if (flag)
				{
					color = (StiUX.IsDark ? StiUX.ItemBackgroundInactive : Color.FromArgb(255, 255, 225, 225));
				}
				StiDrawing.FillRectangle(e.Graphics, color, e.CellBounds);
			}
		}
		e.Paint(e.CellBounds, val);
		DrawBorder(e);
		((HandledEventArgs)(object)e).Handled = true;
	}

	private bool IsEmptyRow(int rowIndex)
	{
		if (EmptyKeys == null || EmptyKeys.Length == 0)
		{
			return false;
		}
		return EmptyKeys.All((string k) => string.IsNullOrWhiteSpace(DataTable.Rows[rowIndex][k]?.ToString()));
	}

	public void DrawHeaderText(DataGridViewCellPaintingEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			val.Alignment = (StringAlignment)1;
			val.Trimming = (StringTrimming)3;
			val.FormatFlags = (StringFormatFlags)4096;
			string text = (IsRowHeader(e) ? (e.RowIndex + 1).ToString() : e.FormattedValue?.ToString());
			if (!string.IsNullOrWhiteSpace(text))
			{
				StiDrawing.DrawString(e.Graphics, text, ((Control)this).Font, StiUX.ButtonForeground, e.CellBounds, val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawBorder(DataGridViewCellPaintingEventArgs e)
	{
		Rectangle cellBounds = e.CellBounds;
		StiDrawing.DrawLine(e.Graphics, StiUX.IsDark ? StiUX.ItemSeparator : Color.Gray, cellBounds.Right - 1, cellBounds.Top, cellBounds.Right - 1, cellBounds.Bottom);
		if (IsRowHeader(e))
		{
			StiDrawing.DrawLine(e.Graphics, StiUX.IsDark ? StiUX.ItemSeparator : Color.Gray, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
			if (SelectedCellsList.All((DataGridViewCell c) => c.ColumnIndex != 0) && SelectedCellsList.Any((DataGridViewCell c) => c.RowIndex == e.RowIndex))
			{
				StiDrawing.FillRectangle(e.Graphics, e.CellStyle.SelectionBackColor, cellBounds.Right - StiScaleUI.I2, cellBounds.Y, StiScaleUI.I2, cellBounds.Height);
			}
		}
		else if (e.RowIndex == -1)
		{
			StiDrawing.DrawLine(e.Graphics, StiUX.IsDark ? StiUX.ItemSeparator : Color.LightGray, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
			if (SelectedCellsList.All((DataGridViewCell c) => c.RowIndex != 0) && SelectedCellsList.Any((DataGridViewCell c) => c.ColumnIndex == e.ColumnIndex))
			{
				StiDrawing.FillRectangle(e.Graphics, e.CellStyle.SelectionBackColor, cellBounds.X, cellBounds.Bottom - StiScaleUI.I2, cellBounds.Width, StiScaleUI.I2);
			}
		}
	}

	public void SelectFullColumn(int columnIndex, int count = 1)
	{
		((DataGridView)this).ClearSelection();
		while (count > 0 && columnIndex < ((BaseCollection)((DataGridView)this).Columns).Count)
		{
			for (int i = 0; i < ((DataGridView)this).RowCount; i++)
			{
				((DataGridView)this).SetSelectedCellCore(columnIndex, i, true);
			}
			columnIndex++;
			count--;
		}
	}

	public void SelectFullRow(int rowIndex, int count = 1)
	{
		((DataGridView)this).ClearSelection();
		while (count > 0 && rowIndex < ((DataGridView)this).Rows.Count)
		{
			for (int i = 0; i < ((DataGridView)this).ColumnCount; i++)
			{
				((DataGridView)this).SetSelectedCellCore(i, rowIndex, true);
			}
			rowIndex++;
			count--;
		}
	}

	public void SelectArea(Rectangle rect)
	{
		((DataGridView)this).ClearSelection();
		bool flag = false;
		for (int i = rect.X; i < Math.Min(rect.Right, ((BaseCollection)((DataGridView)this).Columns).Count); i++)
		{
			for (int j = rect.Y; j < Math.Min(rect.Bottom, ((DataGridView)this).Rows.Count); j++)
			{
				((DataGridView)this).SetSelectedCellCore(i, j, true);
				flag = true;
			}
		}
		if (!flag)
		{
			for (int k = rect.X; k < Math.Min(rect.Right, ((BaseCollection)((DataGridView)this).Columns).Count); k++)
			{
				((DataGridView)this).SetSelectedCellCore(k, ((DataGridView)this).Rows.Count - 1, true);
			}
		}
	}

	private bool IsRowHeader(DataGridViewCellPaintingEventArgs e)
	{
		if (e.ColumnIndex == -1 && e.RowIndex != -1)
		{
			return ((DataGridView)this).RowHeadersVisible;
		}
		return false;
	}

	internal void DeleteCurrentRow()
	{
		if (DataTable == null)
		{
			return;
		}
		Rectangle selectionRect = GetSelectionRect();
		if (selectionRect.IsEmpty)
		{
			return;
		}
		for (int num = selectionRect.Height; num > 0; num--)
		{
			if (DataTable.Rows.Count == 1)
			{
				((DataGridView)this).SelectAll();
				((DataGridView)this).ClearSelection();
				return;
			}
			DataTable.Rows.RemoveAt(selectionRect.Y);
		}
		SelectArea(selectionRect);
		InvokeCellRowsChanged();
	}

	internal void MoveCurrentRowUp()
	{
		if (DataTable != null)
		{
			Rectangle selectionRect = GetSelectionRect();
			if (!selectionRect.IsEmpty)
			{
				DataRow dataRow = DataTable.Rows[selectionRect.Y - 1];
				DataRow dataRow2 = NewEmptyRow();
				dataRow2.ItemArray = dataRow.ItemArray;
				DataTable.Rows.Remove(dataRow);
				DataTable.Rows.InsertAt(dataRow2, selectionRect.Bottom - 1);
				selectionRect.Y--;
				SelectArea(selectionRect);
				InvokeCellRowsChanged();
			}
		}
	}

	internal void MoveCurrentRowDown()
	{
		if (DataTable != null)
		{
			Rectangle selectionRect = GetSelectionRect();
			if (!selectionRect.IsEmpty)
			{
				DataRow dataRow = DataTable.Rows[selectionRect.Bottom];
				DataRow dataRow2 = NewEmptyRow();
				dataRow2.ItemArray = dataRow.ItemArray;
				DataTable.Rows.Remove(dataRow);
				DataTable.Rows.InsertAt(dataRow2, selectionRect.Y);
				selectionRect.Y++;
				SelectArea(selectionRect);
				InvokeCellRowsChanged();
			}
		}
	}

	internal void InsertRowBelowCurrent()
	{
		if (DataTable == null)
		{
			return;
		}
		Rectangle selectionRect = GetSelectionRect();
		if (!selectionRect.IsEmpty)
		{
			for (int num = selectionRect.Height; num > 0; num--)
			{
				DataRow row = NewEmptyRow();
				DataTable.Rows.InsertAt(row, selectionRect.Bottom);
			}
			selectionRect.Y += selectionRect.Height;
			SelectArea(selectionRect);
			InvokeCellRowsChanged();
		}
	}

	internal void InsertRowAboveCurrent()
	{
		if (DataTable == null)
		{
			return;
		}
		Rectangle selectionRect = GetSelectionRect();
		if (!selectionRect.IsEmpty)
		{
			for (int num = selectionRect.Height; num > 0; num--)
			{
				DataRow row = NewEmptyRow();
				DataTable.Rows.InsertAt(row, selectionRect.Y);
			}
			SelectArea(selectionRect);
			InvokeCellRowsChanged();
		}
	}

	private Rectangle GetSelectionRect()
	{
		if (!SelectedCellsList.Any())
		{
			return Rectangle.Empty;
		}
		int num = SelectedCellsList.Min((DataGridViewCell c) => c.ColumnIndex);
		int num2 = SelectedCellsList.Min((DataGridViewCell c) => c.RowIndex);
		int width = SelectedCellsList.Max((DataGridViewCell c) => c.ColumnIndex) - num + 1;
		int height = SelectedCellsList.Max((DataGridViewCell c) => c.RowIndex) - num2 + 1;
		return new Rectangle(num, num2, width, height);
	}

	public DataRow NewEmptyRow()
	{
		if (DataTable == null)
		{
			return null;
		}
		DataRow dataRow = DataTable.NewRow();
		foreach (DataColumn column in DataTable.Columns)
		{
			dataRow[column.ColumnName] = (column.DataType.IsValueType ? Activator.CreateInstance(column.DataType) : null);
		}
		return dataRow;
	}

	public void AddNewEmptyRowsUpTo(int rowsLimit)
	{
		if (DataTable != null)
		{
			while (DataTable.Rows.Count < rowsLimit)
			{
				AddNewEmptyRow();
			}
		}
	}

	public DataRow AddNewEmptyRow()
	{
		DataRow dataRow = NewEmptyRow();
		if (dataRow != null)
		{
			DataTable.Rows.Add(dataRow);
		}
		return dataRow;
	}

	protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		((DataGridView)this).OnCellMouseDown(e);
		if ((int)((MouseEventArgs)e).Button == 2097152 && (e.ColumnIndex != -1 || e.RowIndex != -1))
		{
			if (e.ColumnIndex != -1 && e.RowIndex == -1)
			{
				SelectFullColumn(e.ColumnIndex);
			}
			else if (e.ColumnIndex == -1 && e.RowIndex != -1)
			{
				SelectFullRow(e.RowIndex);
			}
			else if (!SelectedCellsList.Any((DataGridViewCell c) => c.ColumnIndex == e.ColumnIndex && c.RowIndex == e.RowIndex))
			{
				((DataGridView)this).ClearSelection();
				((DataGridView)this).SetSelectedCellCore(e.ColumnIndex, e.RowIndex, true);
			}
			RefreshHeaderMenu();
			if (((ArrangedElementCollection)((ToolStrip)menuHeader).Items).Count > 0)
			{
				Rectangle cellDisplayRectangle = ((DataGridView)this).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
				Point point = ((Control)this).PointToScreen(new Point(cellDisplayRectangle.Left, cellDisplayRectangle.Bottom));
				((ToolStripDropDown)menuHeader).Show(point);
			}
		}
	}

	protected override void OnCellClick(DataGridViewCellEventArgs e)
	{
		((DataGridView)this).OnCellClick(e);
		if (e.ColumnIndex != -1 && e.RowIndex == -1)
		{
			SelectFullColumn(e.ColumnIndex);
		}
		else if (e.ColumnIndex == -1 && e.RowIndex != -1)
		{
			SelectFullRow(e.RowIndex);
		}
	}

	protected override void OnSelectionChanged(EventArgs e)
	{
		((DataGridView)this).OnSelectionChanged(e);
		if (!SelectedCellsList.Any())
		{
			minColumnIndex = -1;
			maxColumnIndex = -1;
			prevRowIndex = -1;
			return;
		}
		int num = SelectedCellsList.Min((DataGridViewCell c) => c.ColumnIndex);
		int num2 = SelectedCellsList.Max((DataGridViewCell c) => c.ColumnIndex);
		int num3 = SelectedCellsList.Min((DataGridViewCell c) => c.RowIndex);
		bool flag = false;
		if (minColumnIndex != num)
		{
			flag = true;
			minColumnIndex = num;
		}
		if (maxColumnIndex != num2)
		{
			flag = true;
			maxColumnIndex = num2;
		}
		if (prevRowIndex != num3)
		{
			flag = true;
			prevRowIndex = num3;
		}
		if (flag)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
	{
		((DataGridView)this).OnCellValueChanged(e);
		if (e.ColumnIndex != -1 && e.RowIndex != -1)
		{
			if (AddNewRowAfterEditing && !skipValidatingOnce && ((DataGridView)this).Columns[e.ColumnIndex].CellType != typeof(DataGridViewCheckBoxCell) && AddNewEmptyRowIfRequired())
			{
				((DataGridView)this).SetSelectedCellCore(e.ColumnIndex, e.RowIndex, false);
				((DataGridView)this).SetSelectedCellCore(e.ColumnIndex, e.RowIndex + 1, true);
			}
			skipValidatingOnce = false;
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		((DataGridView)this).OnKeyDown(e);
		if ((int)e.KeyCode == 40 && SelectedCellsList.Any() && AddNewRowWhenPressingDown)
		{
			List<DataGridViewCell> selectedCellsList = SelectedCellsList;
			if (AddNewEmptyRowIfRequired())
			{
				selectedCellsList.ForEach(delegate(DataGridViewCell c)
				{
					((DataGridView)this).SetSelectedCellCore(c.ColumnIndex, c.RowIndex, false);
				});
				selectedCellsList.ForEach(delegate(DataGridViewCell c)
				{
					((DataGridView)this).SetSelectedCellCore(c.ColumnIndex, c.RowIndex + 1, true);
				});
			}
		}
		if ((int)e.KeyCode != 46 || !AllowDeleteSelectedArea)
		{
			return;
		}
		SelectedCellsList.ForEach(delegate(DataGridViewCell c)
		{
			if (c is DataGridViewTextBoxCell)
			{
				skipValidatingOnce = true;
				c.Value = string.Empty;
			}
		});
	}

	protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		((DataGridView)this).OnEditingControlShowing(e);
		if (AddNewRowWhenPressingDown)
		{
			e.Control.PreviewKeyDown -= new PreviewKeyDownEventHandler(Control_PreviewKeyDown);
			e.Control.PreviewKeyDown += new PreviewKeyDownEventHandler(Control_PreviewKeyDown);
		}
	}

	private void Control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 40)
		{
			AddNewEmptyRowIfRequired();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		((DataGridView)this).OnPaint(e);
		if ((int)((DataGridView)this).BorderStyle == 1)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			StiDrawing.DrawRectangle(e.Graphics, StiUX.IsDark ? StiUX.Separator : Color.Gray, clientRectangle);
		}
	}

	protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
	{
		((DataGridView)this).OnCellPainting(e);
		if (IsRowHeader(e))
		{
			DrawHeaderCell(e);
		}
		else if (e.RowIndex != -1)
		{
			DrawDataCell(e);
		}
		else
		{
			DrawHeaderCell(e);
		}
	}

	protected override void Dispose(bool disposing)
	{
		((DataGridView)this).Dispose(disposing);
		if (disposing)
		{
			((Component)(object)menuHeader).Dispose();
			menuHeader = null;
		}
	}

	public StiExcelGrid()
	{
		((DataGridView)this).AllowUserToAddRows = false;
		((DataGridView)this).AllowUserToDeleteRows = false;
		((DataGridView)this).BackgroundColor = (StiUX.IsDark ? StiUX.ItemBackground : Color.White);
		((DataGridView)this).BorderStyle = (BorderStyle)0;
		base.DefaultCellStyle.BackColor = (StiUX.IsDark ? StiUX.ItemBackground : Color.White);
		((DataGridView)this).ColumnHeadersHeight = StiScale.I(22);
		((DataGridView)this).RowHeadersWidth = StiScale.I(30);
		((DataGridView)this).RowTemplate.Height = StiScale.I(22);
		((DataGridView)this).SelectionMode = (DataGridViewSelectionMode)0;
		CreateHeaderMenu();
	}
}
