using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DevComponents.DotNetBar;
using EntityCache.Business;
using FormCustom.ExtendedControls;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace FormCustom.UC;

public class UcOrderChart : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private Chart chart1;

	private ComboBox cmbChartType;

	private ComboBox cmbDocument;

	private PictureBox picDate;

	private BindingSource DocumentBindingSource;

	private BindingSource ChartTypeBindingSource;

	private CustomeToggleButton togBtn;

	private Label label1;

	private BindingSource OrderReportBindingSource;

	private DataGridView Dgrid;

	private DataGridViewTextBoxColumn dateShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sumDataGridViewTextBoxColumn;

	private ContextMenuStrip CMA;

	private ToolStripMenuItem mnuViewOrder;

	private Label lblDate;

	public DocumentForDisplay CurrentDocument { get; private set; }

	public ChartTypeForDisplay CurrentChartType { get; private set; }

	public OrderSummaryViewModel CurrentOrder { get; private set; }

	public DateTime Date1 { get; private set; }

	public DateTime Date2 { get; private set; }

	public string ContextGuid { get; private set; }

	public event Action OnDateChanged;

	public event Action<OrderFilter> OnFilterChanged;

	public UcOrderChart()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
		LoadFactorTypes();
		LoadChartTypes();
		((Control)Dgrid).Visible = false;
		((Control)chart1).Visible = true;
	}

	public async Task InitAsync(string contextGuid, DateTime d1, DateTime d2)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				return;
			}
			ContextGuid = contextGuid;
			Date1 = d1;
			Date2 = d2;
			List<OrderSummaryViewModel> allOrders = await OrderBusiness.GetAllSummaryAsync(contextGuid, CurrentDocument?.FactorType ?? Document.OrderSell, Date1, Date2);
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Expected O, but got Unknown
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Expected O, but got Unknown
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Expected O, but got Unknown
				try
				{
					OrderReportBindingSource.DataSource = allOrders?.Where((OrderSummaryViewModel q) => q.Count > 0m)?.ToSortableBindingList();
					chart1.Palette = (ChartColorPalette)5;
					((Collection<Title>)(object)chart1.Titles).Clear();
					((Collection<Series>)(object)chart1.Series).Clear();
					Series val = new Series
					{
						ChartType = Cast(CurrentChartType.Type),
						Name = "جمع " + CurrentDocument.Name
					};
					val.ChartType = Cast(CurrentChartType.Type);
					((DataPointCustomProperties)val).Font = new Font(new FontFamily("B Yekan"), 9.75f);
					for (int i = 0; i < allOrders?.Count(); i++)
					{
						string text = (allOrders[i]?.Sum ?? 0m).NpToString("N0", 0);
						val.Points.AddXY((object)allOrders[i].DateSh, new object[1] { text });
						((DataPointCustomProperties)val).IsValueShownAsLabel = true;
					}
					((Collection<Series>)(object)chart1.Series).Add(val);
					foreach (ChartArea item in (Collection<ChartArea>)(object)chart1.ChartAreas)
					{
						item.AxisX.MajorGrid.LineWidth = 0;
						item.AxisX.MinorGrid.LineWidth = 0;
						item.AxisY.MajorGrid.LineWidth = 0;
						item.AxisY.MinorGrid.LineWidth = 0;
						item.AxisY.LabelStyle.Format = "#,##0";
					}
					_ = Date1;
					_ = Date2;
					((Control)lblDate).Text = "گزارش از تاریخ " + Date1.MiladiToCalture() + " الی " + Date2.MiladiToCalture();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 91);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 94);
		}
	}

	private void EventInvoker()
	{
		try
		{
			if (CurrentOrder != null)
			{
				DateTime dateTime = CurrentOrder.DateSh.ShamsiToMiladi();
				DateTime value = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
				DateTime value2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
				OrderFilter obj = new OrderFilter
				{
					DateFrom = value,
					DateTo = value2,
					FactorType = (CurrentDocument?.FactorType ?? Document.OrderSell)
				};
				this.OnFilterChanged?.Invoke(obj);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EventInvoker", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 112);
		}
	}

	private void DocumentBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			CurrentDocument = (DocumentForDisplay)DocumentBindingSource.Current;
			Task.Run(() => InitAsync(ContextGuid, Date1, Date2));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DocumentBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 121);
		}
	}

	private void ChartTypeBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			CurrentChartType = (ChartTypeForDisplay)ChartTypeBindingSource.Current;
			foreach (Series item in (Collection<Series>)(object)chart1.Series)
			{
				item.ChartType = Cast(CurrentChartType.Type);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChartTypeBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 132);
		}
	}

	private SeriesChartType Cast(ChartType _type)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return (SeriesChartType)(_type switch
			{
				ChartType.FastPoint => 1, 
				ChartType.Bubble => 2, 
				ChartType.Spline => 4, 
				ChartType.StackedBar => 8, 
				ChartType.Column => 10, 
				ChartType.Pie => 17, 
				ChartType.Pyramid => 34, 
				_ => 10, 
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Cast", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 152);
			throw;
		}
	}

	private void picDate_Click(object sender, EventArgs e)
	{
		this.OnDateChanged?.Invoke();
	}

	private void LoadFactorTypes()
	{
		try
		{
			List<DocumentForDisplay> dataSource = new List<DocumentForDisplay>
			{
				new DocumentForDisplay(Document.OrderSell),
				new DocumentForDisplay(Document.OrderBuy),
				new DocumentForDisplay(Document.OrderBackSell),
				new DocumentForDisplay(Document.OrderBackBuy),
				new DocumentForDisplay(Document.PishFactor),
				new DocumentForDisplay(Document.Zayeat),
				new DocumentForDisplay(Document.AmaniVorod),
				new DocumentForDisplay(Document.AmaniKhoroj)
			};
			DocumentBindingSource.DataSource = dataSource;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadFactorTypes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 174);
		}
	}

	private void LoadChartTypes()
	{
		try
		{
			List<ChartTypeForDisplay> list = new List<ChartTypeForDisplay>();
			foreach (ChartType value in Enum.GetValues(typeof(ChartType)))
			{
				list.Add(new ChartTypeForDisplay(value));
			}
			ChartTypeBindingSource.DataSource = list;
			((ListControl)cmbChartType).SelectedValue = ChartType.Pie;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadChartTypes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 188);
		}
	}

	private void togBtn_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (((CheckBox)togBtn).Checked)
			{
				((Control)Dgrid).Visible = true;
				((Control)chart1).Visible = false;
			}
			else
			{
				((Control)Dgrid).Visible = false;
				((Control)chart1).Visible = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "togBtn_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 205);
		}
	}

	private void mnuViewOrder_Click(object sender, EventArgs e)
	{
		EventInvoker();
	}

	private void OrderReportBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			CurrentOrder = (OrderSummaryViewModel)OrderReportBindingSource.Current;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OrderReportBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 214);
		}
	}

	private void Dgrid_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		EventInvoker();
	}

	private void Dgrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				e.Handled = true;
				EventInvoker();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dgrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcOrderChart.cs", 227);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Expected O, but got Unknown
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf3: Expected O, but got Unknown
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ChartArea val = new ChartArea();
		Legend val2 = new Legend();
		Series val3 = new Series();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		pnlHeader = new PanelEx();
		lblDate = new Label();
		label1 = new Label();
		togBtn = new CustomeToggleButton();
		picDate = new PictureBox();
		cmbChartType = new ComboBox();
		ChartTypeBindingSource = new BindingSource(components);
		cmbDocument = new ComboBox();
		DocumentBindingSource = new BindingSource(components);
		chart1 = new Chart();
		OrderReportBindingSource = new BindingSource(components);
		Dgrid = new DataGridView();
		dateShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		countDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		sumDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		CMA = new ContextMenuStrip(components);
		mnuViewOrder = new ToolStripMenuItem();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picDate).BeginInit();
		((ISupportInitialize)ChartTypeBindingSource).BeginInit();
		((ISupportInitialize)DocumentBindingSource).BeginInit();
		((ISupportInitialize)chart1).BeginInit();
		((ISupportInitialize)OrderReportBindingSource).BeginInit();
		((ISupportInitialize)Dgrid).BeginInit();
		((Control)CMA).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblDate);
		((Control)pnlHeader).Controls.Add((Control)(object)label1);
		((Control)pnlHeader).Controls.Add((Control)(object)togBtn);
		((Control)pnlHeader).Controls.Add((Control)(object)picDate);
		((Control)pnlHeader).Controls.Add((Control)(object)cmbChartType);
		((Control)pnlHeader).Controls.Add((Control)(object)cmbDocument);
		((Control)pnlHeader).Controls.Add((Control)(object)chart1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(469, 282);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 139;
		((Control)lblDate).Anchor = (AnchorStyles)13;
		((Control)lblDate).Cursor = Cursors.Default;
		((Control)lblDate).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDate).ForeColor = Color.DimGray;
		((Control)lblDate).Location = new Point(113, 49);
		((Control)lblDate).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDate).Name = "lblDate";
		((Control)lblDate).Size = new Size(242, 17);
		((Control)lblDate).TabIndex = 8;
		lblDate.TextAlign = (ContentAlignment)16;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(133, 18);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(50, 20);
		((Control)label1).TabIndex = 7;
		((Control)label1).Text = "[وضعیت]:";
		((Control)togBtn).Anchor = (AnchorStyles)9;
		((Control)togBtn).BackColor = Color.Transparent;
		((Control)togBtn).Cursor = Cursors.Hand;
		((Control)togBtn).Location = new Point(83, 18);
		((Control)togBtn).Name = "togBtn";
		togBtn.OffBackColor = Color.Gray;
		togBtn.OfToggleColor = Color.DimGray;
		togBtn.OnBackColor = Color.FromArgb(65, 81, 220);
		togBtn.OnToggleColor = Color.FromArgb(228, 236, 255);
		((Control)togBtn).Size = new Size(49, 23);
		((Control)togBtn).TabIndex = 6;
		((Control)togBtn).Text = "customeToggleButton1";
		((ButtonBase)togBtn).UseVisualStyleBackColor = false;
		((CheckBox)togBtn).CheckedChanged += togBtn_CheckedChanged;
		((Control)picDate).Anchor = (AnchorStyles)9;
		((Control)picDate).Cursor = Cursors.Hand;
		picDate.Image = (Image)(object)FormCustom.Properties.Resources.Calendar;
		((Control)picDate).Location = new Point(189, 14);
		((Control)picDate).Name = "picDate";
		((Control)picDate).Size = new Size(33, 29);
		picDate.SizeMode = (PictureBoxSizeMode)4;
		picDate.TabIndex = 5;
		picDate.TabStop = false;
		((Control)picDate).Click += picDate_Click;
		((Control)cmbChartType).Anchor = (AnchorStyles)9;
		((Control)cmbChartType).Cursor = Cursors.Hand;
		cmbChartType.DataSource = ChartTypeBindingSource;
		((ListControl)cmbChartType).DisplayMember = "Name";
		cmbChartType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cmbChartType).FormattingEnabled = true;
		((Control)cmbChartType).Location = new Point(228, 15);
		((Control)cmbChartType).Name = "cmbChartType";
		((Control)cmbChartType).Size = new Size(94, 28);
		((Control)cmbChartType).TabIndex = 4;
		((ListControl)cmbChartType).ValueMember = "Type";
		ChartTypeBindingSource.DataSource = typeof(ChartTypeForDisplay);
		ChartTypeBindingSource.CurrentChanged += ChartTypeBindingSource_CurrentChanged;
		((Control)cmbDocument).Anchor = (AnchorStyles)9;
		((Control)cmbDocument).Cursor = Cursors.Hand;
		cmbDocument.DataSource = DocumentBindingSource;
		((ListControl)cmbDocument).DisplayMember = "Name";
		cmbDocument.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cmbDocument).FormattingEnabled = true;
		((Control)cmbDocument).Location = new Point(328, 15);
		((Control)cmbDocument).Name = "cmbDocument";
		((Control)cmbDocument).Size = new Size(124, 28);
		((Control)cmbDocument).TabIndex = 4;
		((ListControl)cmbDocument).ValueMember = "FactorType";
		DocumentBindingSource.DataSource = typeof(DocumentForDisplay);
		DocumentBindingSource.CurrentChanged += DocumentBindingSource_CurrentChanged;
		((Control)chart1).Anchor = (AnchorStyles)15;
		((ChartNamedElement)val).Name = "ChartArea1";
		((Collection<ChartArea>)(object)chart1.ChartAreas).Add(val);
		((ChartNamedElement)val2).Name = "Legend1";
		((Collection<Legend>)(object)chart1.Legends).Add(val2);
		((Control)chart1).Location = new Point(3, 71);
		((Control)chart1).Name = "chart1";
		val3.ChartArea = "ChartArea1";
		val3.Legend = "Legend1";
		((ChartNamedElement)val3).Name = "Series1";
		((Collection<Series>)(object)chart1.Series).Add(val3);
		chart1.Size = new Size(483, 208);
		((Control)chart1).TabIndex = 0;
		OrderReportBindingSource.DataSource = typeof(OrderSummaryViewModel);
		OrderReportBindingSource.CurrentChanged += OrderReportBindingSource_CurrentChanged;
		Dgrid.AllowUserToAddRows = false;
		val4.BackColor = Color.Gainsboro;
		Dgrid.AlternatingRowsDefaultCellStyle = val4;
		((Control)Dgrid).Anchor = (AnchorStyles)15;
		Dgrid.AutoGenerateColumns = false;
		Dgrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		Dgrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)dateShDataGridViewTextBoxColumn,
			(DataGridViewColumn)countDataGridViewTextBoxColumn,
			(DataGridViewColumn)sumDataGridViewTextBoxColumn
		});
		((Control)Dgrid).ContextMenuStrip = CMA;
		Dgrid.DataSource = OrderReportBindingSource;
		((Control)Dgrid).Location = new Point(3, 71);
		((Control)Dgrid).Margin = new Padding(3, 4, 3, 4);
		((Control)Dgrid).Name = "Dgrid";
		Dgrid.ReadOnly = true;
		Dgrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)Dgrid).Size = new Size(463, 208);
		((Control)Dgrid).TabIndex = 143;
		((Control)Dgrid).KeyDown += new KeyEventHandler(Dgrid_KeyDown);
		((Control)Dgrid).MouseDoubleClick += new MouseEventHandler(Dgrid_MouseDoubleClick);
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).DataPropertyName = "DateSh";
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).HeaderText = "[تاریخ]";
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).Name = "dateShDataGridViewTextBoxColumn";
		((DataGridViewBand)dateShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dateShDataGridViewTextBoxColumn).Width = 58;
		((DataGridViewColumn)countDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)countDataGridViewTextBoxColumn).DataPropertyName = "Count";
		((DataGridViewColumn)countDataGridViewTextBoxColumn).HeaderText = "[تعداد فاکتورها]";
		((DataGridViewColumn)countDataGridViewTextBoxColumn).Name = "countDataGridViewTextBoxColumn";
		((DataGridViewBand)countDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)countDataGridViewTextBoxColumn).Width = 104;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).DataPropertyName = "Sum";
		val5.Format = "N0";
		val5.NullValue = null;
		((DataGridViewBand)sumDataGridViewTextBoxColumn).DefaultCellStyle = val5;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).HeaderText = "[جمع فاکتورها]";
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).Name = "sumDataGridViewTextBoxColumn";
		((DataGridViewBand)sumDataGridViewTextBoxColumn).ReadOnly = true;
		((ToolStrip)CMA).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)mnuViewOrder });
		((Control)CMA).Name = "CMA";
		((Control)CMA).RightToLeft = (RightToLeft)1;
		((Control)CMA).Size = new Size(158, 28);
		((ToolStripItem)mnuViewOrder).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)mnuViewOrder).Name = "mnuViewOrder";
		((ToolStripItem)mnuViewOrder).Size = new Size(157, 24);
		((ToolStripItem)mnuViewOrder).Text = "[مشاهده فاکتورها]";
		((ToolStripItem)mnuViewOrder).Click += mnuViewOrder_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)Dgrid);
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcOrderChart";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(469, 282);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlHeader).PerformLayout();
		((ISupportInitialize)picDate).EndInit();
		((ISupportInitialize)ChartTypeBindingSource).EndInit();
		((ISupportInitialize)DocumentBindingSource).EndInit();
		((ISupportInitialize)chart1).EndInit();
		((ISupportInitialize)OrderReportBindingSource).EndInit();
		((ISupportInitialize)Dgrid).EndInit();
		((Control)CMA).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
