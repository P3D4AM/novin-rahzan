using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DevComponents.DotNetBar;
using EntityCache.Business.Accounting.Cheque;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace FormCustom.UC.Dashboard;

public class UcChequeChart : UserControl
{
	private CancellationTokenSource _tokenSource;

	private IContainer components;

	private PanelEx pnlHeader;

	private PictureBox picDate;

	private ComboBox cmbChartType;

	private BindingSource ChartTypeBindingSource;

	private Chart chart1;

	private Label lblDate;

	public ChartTypeForDisplay CurrentChartType { get; private set; }

	public DateTime Date1 { get; private set; }

	public DateTime Date2 { get; private set; }

	public string ContextGuid { get; private set; }

	public event Action OnDateChanged;

	public async Task InitAsync(string contextGuid, DateTime d1, DateTime d2, CancellationToken tkn = default(CancellationToken))
	{
		try
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource();
			CancellationToken token = _tokenSource.Token;
			if (string.IsNullOrEmpty(contextGuid))
			{
				return;
			}
			ContextGuid = contextGuid;
			Date1 = d1;
			Date2 = d2;
			BedehiTalabReportFilter filter = new BedehiTalabReportFilter
			{
				DateFrom = Date1,
				DateTo = Date2,
				Type = EnBedehiTalabType.CheckM
			};
			List<BedehiTalabModel> chequeMList = (await BedehiTalabReportBusiness.GetAllReportAsync(contextGuid, filter, tkn))?.OrderBy((BedehiTalabModel q) => q.DateSh)?.ToList();
			filter.Type = EnBedehiTalabType.CheckSh;
			List<BedehiTalabModel> chequeShList = (await BedehiTalabReportBusiness.GetAllReportAsync(contextGuid, filter, tkn))?.OrderBy((BedehiTalabModel q) => q.DateSh)?.ToList();
			do
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Expected O, but got Unknown
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cd: Expected O, but got Unknown
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Expected O, but got Unknown
				//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Expected O, but got Unknown
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_012b: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Expected O, but got Unknown
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Expected O, but got Unknown
				try
				{
					if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).SuspendLayout();
						chart1.Palette = (ChartColorPalette)5;
						((Collection<Title>)(object)chart1.Titles).Clear();
						((Collection<Series>)(object)chart1.Series).Clear();
						Series val = new Series
						{
							ChartType = Cast(CurrentChartType.Type),
							Name = "جمع اسناد دریافتنی"
						};
						val.ChartType = Cast(CurrentChartType.Type);
						((DataPointCustomProperties)val).Font = new Font(new FontFamily("B Yekan"), 9.75f);
						Series val2 = new Series
						{
							ChartType = Cast(CurrentChartType.Type),
							Name = "جمع اسناد پرداختنی"
						};
						val2.ChartType = Cast(CurrentChartType.Type);
						((DataPointCustomProperties)val2).Font = new Font(new FontFamily("B Yekan"), 9.75f);
						for (int i = 0; i < chequeMList?.Count(); i++)
						{
							string text = (chequeMList[i]?.Sum ?? 0m).NpToString("N0", 0);
							val.Points.AddXY((object)chequeMList[i].DateSh, new object[1] { text });
							((DataPointCustomProperties)val).IsValueShownAsLabel = true;
						}
						for (int j = 0; j < chequeShList?.Count(); j++)
						{
							string text2 = (chequeShList[j]?.Sum ?? 0m).NpToString("N0", 0);
							val2.Points.AddXY((object)chequeShList[j].DateSh, new object[1] { text2 });
							((DataPointCustomProperties)val2).IsValueShownAsLabel = true;
						}
						((Collection<Series>)(object)chart1.Series).Add(val);
						((Collection<Series>)(object)chart1.Series).Add(val2);
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
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeChart.cs", 112);
				}
				finally
				{
					if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).ResumeLayout();
					}
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeChart.cs", 116);
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
			WebErrorLog.Instance.StartLog(ex, "", "Cast", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeChart.cs", 136);
			throw;
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
			((ListControl)cmbChartType).SelectedValue = ChartType.StackedBar;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadChartTypes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeChart.cs", 152);
		}
	}

	public UcChequeChart()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
		LoadChartTypes();
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
			WebErrorLog.Instance.StartLog(ex, "", "ChartTypeBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcChequeChart.cs", 173);
		}
	}

	private void picDate_Click(object sender, EventArgs e)
	{
		this.OnDateChanged?.Invoke();
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ChartArea val = new ChartArea();
		Legend val2 = new Legend();
		Series val3 = new Series();
		pnlHeader = new PanelEx();
		picDate = new PictureBox();
		cmbChartType = new ComboBox();
		ChartTypeBindingSource = new BindingSource(components);
		chart1 = new Chart();
		lblDate = new Label();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picDate).BeginInit();
		((ISupportInitialize)ChartTypeBindingSource).BeginInit();
		((ISupportInitialize)chart1).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblDate);
		((Control)pnlHeader).Controls.Add((Control)(object)picDate);
		((Control)pnlHeader).Controls.Add((Control)(object)cmbChartType);
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
		((Control)pnlHeader).TabIndex = 144;
		((Control)picDate).Anchor = (AnchorStyles)9;
		((Control)picDate).Cursor = Cursors.Hand;
		picDate.Image = (Image)(object)FormCustom.Properties.Resources.Calendar;
		((Control)picDate).Location = new Point(322, 12);
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
		((Control)cmbChartType).Location = new Point(361, 13);
		((Control)cmbChartType).Name = "cmbChartType";
		((Control)cmbChartType).Size = new Size(94, 28);
		((Control)cmbChartType).TabIndex = 4;
		((ListControl)cmbChartType).ValueMember = "Type";
		ChartTypeBindingSource.DataSource = typeof(ChartTypeForDisplay);
		ChartTypeBindingSource.CurrentChanged += ChartTypeBindingSource_CurrentChanged;
		((Control)chart1).Anchor = (AnchorStyles)15;
		((ChartNamedElement)val).Name = "ChartArea1";
		((Collection<ChartArea>)(object)chart1.ChartAreas).Add(val);
		((ChartNamedElement)val2).Name = "Legend1";
		((Collection<Legend>)(object)chart1.Legends).Add(val2);
		((Control)chart1).Location = new Point(3, 53);
		((Control)chart1).Name = "chart1";
		val3.ChartArea = "ChartArea1";
		val3.Legend = "Legend1";
		((ChartNamedElement)val3).Name = "Series1";
		((Collection<Series>)(object)chart1.Series).Add(val3);
		chart1.Size = new Size(483, 226);
		((Control)chart1).TabIndex = 0;
		((Control)lblDate).Anchor = (AnchorStyles)13;
		((Control)lblDate).Cursor = Cursors.Default;
		((Control)lblDate).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDate).ForeColor = Color.DimGray;
		((Control)lblDate).Location = new Point(73, 18);
		((Control)lblDate).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDate).Name = "lblDate";
		((Control)lblDate).Size = new Size(242, 17);
		((Control)lblDate).TabIndex = 9;
		lblDate.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcChequeChart";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(469, 282);
		((Control)pnlHeader).ResumeLayout(false);
		((ISupportInitialize)picDate).EndInit();
		((ISupportInitialize)ChartTypeBindingSource).EndInit();
		((ISupportInitialize)chart1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
