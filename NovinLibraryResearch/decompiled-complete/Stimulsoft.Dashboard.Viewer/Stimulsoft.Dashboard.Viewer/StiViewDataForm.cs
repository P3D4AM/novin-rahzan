using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Drawing.Painters.Table;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Dashboard.Viewer;

public class StiViewDataForm : Form
{
	private bool isFormClosing;

	private StiDataTable dataTable;

	private DataTable netTable;

	private IStiElement element;

	private bool sortAscending;

	private string sortColumn;

	private IContainer components;

	private StiDataGridView dataGrid;

	private CircularProgress progressView;

	private BackgroundWorker workerView;

	private Label labelDataNotFound;

	public StiViewDataForm(IStiElement element)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).ShowIcon = true;
			((Form)this).Icon = StiAppIcons.GetIcon();
			DoScaling();
			Localize();
			StiFormSettings.Load((Form)(object)this);
			this.element = element;
			InitializeStateOfControls();
			workerView.RunWorkerAsync();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ViewDataForm");
		((Control)labelDataNotFound).Text = Loc.Get("Errors", "DataNotFound");
	}

	private void DoScaling()
	{
		((DataGridView)dataGrid).ColumnHeadersHeight = StiScale.I(26);
	}

	private void InitializeStateOfControls()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		((DataGridView)dataGrid).ReadOnly = true;
		((Control)dataGrid).Visible = false;
		((DataGridView)dataGrid).AllowUserToResizeRows = false;
		((Control)labelDataNotFound).Visible = false;
		progressView.IsRunning = true;
		((Control)progressView).Visible = true;
		StiElementStyleIdent style = StiDashboardStyleHelper.GetStyle(element);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(style);
		((Control)labelDataNotFound).ForeColor = tableStyle.CellForeColor;
		((DataGridView)dataGrid).CellFormatting += new DataGridViewCellFormattingEventHandler(DataGrid_CellFormatting);
		((DataGridView)dataGrid).CellPainting += new DataGridViewCellPaintingEventHandler(DataGrid_CellPainting);
		((DataGridView)dataGrid).ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(DataGrid_ColumnHeaderMouseClick);
		((DataGridView)dataGrid).CellMouseMove += new DataGridViewCellMouseEventHandler(DataGrid_CellMouseMove);
		((DataGridView)dataGrid).CellBorderStyle = (DataGridViewCellBorderStyle)4;
		((DataGridView)dataGrid).EnableHeadersVisualStyles = false;
		StiDataGridView stiDataGridView = dataGrid;
		Color backgroundColor = (((Control)this).BackColor = Color.FromArgb(255, StiDashboardStyleHelper.GetBackColor(element)));
		((DataGridView)stiDataGridView).BackgroundColor = backgroundColor;
		((DataGridView)dataGrid).RowTemplate.MinimumHeight = StiScale.I20;
		dataGrid.ColumnHeadersDefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
		((DataGridView)dataGrid).ColumnHeadersBorderStyle = (DataGridViewHeaderBorderStyle)4;
		dataGrid.ColumnHeadersDefaultCellStyle.BackColor = tableStyle.HeaderBackColor;
		dataGrid.ColumnHeadersDefaultCellStyle.ForeColor = tableStyle.HeaderForeColor;
	}

	private DataTable RemoveSystemMeters(DataTable table)
	{
		if (element is IStiGaugeElement)
		{
			IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter m) => m is StiMinGaugeMeter);
			IStiMeter stiMeter2 = dataTable.Meters.FirstOrDefault((IStiMeter m) => m is StiMaxGaugeMeter);
			int num = ((stiMeter != null) ? dataTable.Meters.IndexOf(stiMeter) : (-1));
			int num2 = ((stiMeter2 != null) ? dataTable.Meters.IndexOf(stiMeter2) : (-1));
			if (num != -1)
			{
				table.Columns.RemoveAt(num);
			}
			if (num2 != -1)
			{
				if (num2 > num)
				{
					num2--;
				}
				table.Columns.RemoveAt(num2);
			}
		}
		List<int> list = new List<int>();
		for (int j = 0; j < table.Columns.Count; j++)
		{
			if (table.Columns[j].DataType.IsGenericType)
			{
				list.Add(j);
			}
		}
		list.Reverse();
		list.ForEach(delegate(int i)
		{
			table.Columns.RemoveAt(i);
		});
		return table;
	}

	private void CalculateInformationLocation()
	{
		((Control)progressView).Left = (((Form)this).ClientSize.Width - ((Control)progressView).Width) / 2;
		((Control)progressView).Top = (((Form)this).ClientSize.Height - ((Control)progressView).Height) / 2;
		((Control)labelDataNotFound).Left = (((Form)this).ClientSize.Width - ((Control)labelDataNotFound).Width) / 2;
		((Control)labelDataNotFound).Top = (((Form)this).ClientSize.Height - ((Control)labelDataNotFound).Height) / 2;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		isFormClosing = true;
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_SizeChanged(object sender, EventArgs e)
	{
		CalculateInformationLocation();
		((Control)dataGrid).Invalidate();
	}

	private void This_Load(object sender, EventArgs e)
	{
		CalculateInformationLocation();
	}

	private void DataGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.RowIndex == -1)
		{
			((Control)dataGrid).Invalidate();
		}
	}

	private void DataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		StiElementStyleIdent style = StiDashboardStyleHelper.GetStyle(element);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(style);
		if ((e.RowIndex & 1) == 1)
		{
			e.CellStyle.BackColor = tableStyle.AlternatingCellBackColor;
			e.CellStyle.ForeColor = tableStyle.AlternatingCellForeColor;
			e.CellStyle.SelectionBackColor = tableStyle.SelectedCellBackColor;
			e.CellStyle.SelectionForeColor = tableStyle.SelectedCellForeColor;
		}
		else
		{
			e.CellStyle.BackColor = tableStyle.CellBackColor;
			e.CellStyle.ForeColor = tableStyle.CellForeColor;
			e.CellStyle.SelectionBackColor = tableStyle.SelectedCellBackColor;
			e.CellStyle.SelectionForeColor = tableStyle.SelectedCellForeColor;
		}
	}

	private void DataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		((HandledEventArgs)(object)e).Handled = true;
		StiElementStyleIdent style = StiDashboardStyleHelper.GetStyle(element);
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(style);
		Point pt = ((Control)dataGrid).PointToClient(Cursor.Position);
		bool flag = e.CellBounds.Contains(pt);
		Color color = e.CellStyle.ForeColor;
		Color color2 = e.CellStyle.BackColor;
		TextFormatFlags val = (TextFormatFlags)4;
		if (e.RowIndex != -1)
		{
			if ((int)e.CellStyle.Alignment == 16)
			{
				val = (TextFormatFlags)(val | 0);
			}
			if ((int)e.CellStyle.Alignment == 64)
			{
				val = (TextFormatFlags)(val | 2);
			}
			if ((e.State & 0x20) > 0)
			{
				color = e.CellStyle.SelectionForeColor;
				color2 = e.CellStyle.SelectionBackColor;
			}
		}
		else
		{
			val = (TextFormatFlags)(val | 1);
		}
		if (flag && e.RowIndex == -1)
		{
			color2 = tableStyle.HotHeaderBackColor;
		}
		StiDrawing.FillRectangle(e.Graphics, color2, e.CellBounds);
		if (StiImageHelper.IsImage(e.Value))
		{
			StiImageCellPainter.Draw(e.Graphics, RectangleD.CreateFromRectangle(e.CellBounds), e.Value, null);
		}
		else
		{
			TextRenderer.DrawText((IDeviceContext)(object)e.Graphics, e.FormattedValue.ToString(), ((Control)dataGrid).Font, e.CellBounds, color, val);
		}
		if (e.ColumnIndex != 0)
		{
			StiDrawing.DrawLine(e.Graphics, tableStyle.LineColor, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
		}
		if (e.RowIndex == -1)
		{
			DrawSortArrow(e);
		}
	}

	public void DrawSortArrow(DataGridViewCellPaintingEventArgs e)
	{
		if (((BaseCollection)((DataGridView)dataGrid).Columns).Count > e.ColumnIndex && !(sortColumn != ((DataGridView)dataGrid).Columns[e.ColumnIndex].DataPropertyName))
		{
			StiDataSortDirection direction = (sortAscending ? StiDataSortDirection.Ascending : StiDataSortDirection.Descending);
			Image sortImage = GetSortImage(e, direction);
			int width = sortImage.Width;
			int height = sortImage.Height;
			Rectangle rectangle = new Rectangle(e.CellBounds.X + StiScale.I4, e.CellBounds.Y + (e.CellBounds.Height - height) / 2, width, height);
			e.Graphics.DrawImage(sortImage, rectangle);
			rectangle.X += rectangle.Width - StiScale.I4;
			rectangle.Width = 100000;
		}
	}

	private Image GetSortImage(DataGridViewCellPaintingEventArgs e, StiDataSortDirection direction)
	{
		Point pt = ((Control)dataGrid).PointToClient(Cursor.Position);
		bool flag = e.CellBounds.Contains(pt);
		if (direction == StiDataSortDirection.Ascending)
		{
			if (!flag)
			{
				return StiReportImages.Actions.SortArrowAscWhite();
			}
			return StiReportImages.Actions.SortArrowAscMouseOverWhite();
		}
		if (!flag)
		{
			return StiReportImages.Actions.SortArrowDescWhite();
		}
		return StiReportImages.Actions.SortArrowDescMouseOverWhite();
	}

	private void DataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.ColumnIndex != -1)
		{
			string dataPropertyName = ((DataGridView)dataGrid).Columns[e.ColumnIndex].DataPropertyName;
			sortAscending = dataPropertyName != sortColumn || !sortAscending;
			netTable.DefaultView.Sort = (sortAscending ? dataPropertyName : (dataPropertyName + " DESC"));
			sortColumn = dataPropertyName;
		}
	}

	private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
	{
		((CancelEventArgs)(object)e).Cancel = true;
	}

	private void WorkerView_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
			if (stiDataTable != null)
			{
				dataTable = stiDataTable;
			}
			else
			{
				dataTable = StiElementDataCache.GetOrCreate(element);
			}
		}
		catch
		{
		}
	}

	private void WorkerView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		if (isFormClosing)
		{
			return;
		}
		try
		{
			((DataGridView)dataGrid).DataSource = null;
			((DataGridView)dataGrid).DataMember = null;
			progressView.IsRunning = false;
			((Control)progressView).Visible = false;
			if (dataTable != null)
			{
				netTable = RemoveSystemMeters(dataTable.ToNetTable());
				List<string> list = (from DataColumn c in netTable.Columns
					select c.ColumnName).ToList();
				foreach (DataColumn column in netTable.Columns)
				{
					column.Caption = column.ColumnName;
					column.ColumnName = column.ColumnName.Trim();
					if (column.ColumnName.Contains(",") || column.ColumnName.Contains("'"))
					{
						column.ColumnName = column.ColumnName.Replace(",", "").Replace("'", "");
					}
				}
				((DataGridView)dataGrid).DataSource = netTable;
				int num = 0;
				foreach (DataGridViewColumn item in (BaseCollection)((DataGridView)dataGrid).Columns)
				{
					DataGridViewColumn val = item;
					val.HeaderText = list[num];
					val.CellTemplate.Style.Alignment = (DataGridViewContentAlignment)(netTable.Columns[num].DataType.IsNumericType() ? 64 : 16);
					num++;
				}
				StiTableSizer.MakeAutoSize((DataGridView)(object)dataGrid);
			}
			((Control)dataGrid).Visible = ((DataGridView)dataGrid).DataSource != null;
			((Control)labelDataNotFound).Visible = ((DataGridView)dataGrid).DataSource == null || (dataTable?.Meters != null && dataTable.Meters.Count == 0);
		}
		catch
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		dataGrid = new StiDataGridView();
		labelDataNotFound = new Label();
		progressView = new CircularProgress();
		workerView = new BackgroundWorker();
		((ISupportInitialize)dataGrid).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)dataGrid).AllowUserToAddRows = false;
		((DataGridView)dataGrid).AllowUserToDeleteRows = false;
		((DataGridView)dataGrid).BorderStyle = (BorderStyle)0;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Control;
		val.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		dataGrid.ColumnHeadersDefaultCellStyle = val;
		((DataGridView)dataGrid).ColumnHeadersHeight = 58;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		dataGrid.DefaultCellStyle = val2;
		((Control)dataGrid).Dock = (DockStyle)5;
		((Control)dataGrid).Location = new Point(24, 24);
		((Control)dataGrid).Margin = new Padding(8, 7, 8, 7);
		((Control)dataGrid).Name = "dataGrid";
		((DataGridView)dataGrid).RowHeadersVisible = false;
		((DataGridView)dataGrid).RowHeadersWidth = 102;
		((Control)dataGrid).Size = new Size(1253, 1070);
		((Control)dataGrid).TabIndex = 0;
		((DataGridView)dataGrid).DataError += new DataGridViewDataErrorEventHandler(DataGrid_DataError);
		((Control)labelDataNotFound).AutoSize = true;
		((Control)labelDataNotFound).Location = new Point(579, 524);
		((Control)labelDataNotFound).Margin = new Padding(4, 0, 4, 0);
		((Control)labelDataNotFound).Name = "labelDataNotFound";
		((Control)labelDataNotFound).Size = new Size(200, 32);
		((Control)labelDataNotFound).TabIndex = 25;
		((Control)labelDataNotFound).Text = "DataNotFound";
		labelDataNotFound.TextAlign = (ContentAlignment)32;
		((Control)labelDataNotFound).Visible = false;
		((Control)progressView).BackColor = Color.White;
		progressView.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressView).Location = new Point(586, 460);
		((Control)progressView).Margin = new Padding(8, 7, 8, 7);
		((Control)progressView).Name = "progressView";
		progressView.ProgressBarType = eCircularProgressType.Donut;
		progressView.ProgressColor = Color.SteelBlue;
		((Control)progressView).Size = new Size(139, 129);
		progressView.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressView).TabIndex = 7;
		((Control)progressView).TabStop = false;
		((Control)progressView).Visible = false;
		workerView.WorkerSupportsCancellation = true;
		workerView.DoWork += WorkerView_DoWork;
		workerView.RunWorkerCompleted += WorkerView_RunWorkerCompleted;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(16f, 31f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1301, 1118);
		((Control)this).Controls.Add((Control)(object)labelDataNotFound);
		((Control)this).Controls.Add((Control)(object)progressView);
		((Control)this).Controls.Add((Control)(object)dataGrid);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(8, 7, 8, 7);
		((Control)this).Name = "StiViewDataForm";
		((Control)this).Padding = new Padding(24);
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiViewDataForm";
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Control)this).SizeChanged += This_SizeChanged;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)dataGrid).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
