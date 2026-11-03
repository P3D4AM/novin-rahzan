using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.BusinessLayer.Models.Payamak;
using PacketParser.Entities;
using PayamakSettings;

namespace PayamakUi.Forms.Reports;

public class FRMFilter : CacheBaseForm
{
	public enum ReportType : short
	{
		None,
		RPTInbox,
		RPTOutBox,
		RPTSentBox
	}

	private CancellationTokenSource _cancellation;

	private PayamakFilterModel payamakFilterModel;

	private bool _isDisposed;

	private ReportType _Type;

	private string TextM = "";

	private string NUMFE = "";

	private string NumGI = "";

	private IContainer components;

	internal PanelEx PanelEx2;

	internal GroupPanel GroupPanel4;

	private CheckBox chbST;

	internal ComboItem CmbResideG;

	internal ComboItem CmbResidem;

	internal ComboItem CmbNM;

	internal ComboItem CmbUnknown;

	internal ComboItem CmbNG;

	internal ComboItem CmbBlack;

	internal ComboItem CmbRead;

	internal ComboItem CmbUnRead;

	internal ComboItem CmbReply;

	internal ComboItem CmbOutBoxG;

	internal ComboItem CmbOutBoxP;

	internal GroupPanel GroupPanel3;

	internal Label LX1_Sent;

	internal TextBoxX TXTSearch;

	internal GroupPanel GroupPanel2;

	internal TextBoxX TxtNumF;

	internal Label LabelX11;

	internal TextBoxX TxtNumG;

	internal Label LabelX12;

	internal GroupPanel GPMonth;

	private Label LblAza;

	private Label LblEs;

	private Label LblAba;

	private Label LblBah;

	private Label LblMehr;

	private Label Lblday;

	private Label LblSha;

	private Label LblMor;

	private Label Lbltir;

	private Label LblKh;

	private Label LblOr;

	private Label LblFar;

	internal ButtonX BTNOK;

	internal GroupPanel GroupPanel1;

	private Label labelX2;

	private Label labelX1;

	private DateBoxFa Date1;

	private DateBoxFa Date2;

	private DateBoxFa DateOne;

	internal RadioButton RBtnBeinDa;

	internal RadioButton RbtnAtDay;

	internal ComboItem CMBCansel;

	internal ComboItem CmbWaiting;

	private BindingSource BindingStatus;

	private ComboBox CMBST;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private DeliveryStatusForDisplay Current { get; set; }

	public DateTime DateM1
	{
		get
		{
			try
			{
				if (RBtnBeinDa.Checked)
				{
					return new DateTime(Date1.MiladiDate.Year, Date1.MiladiDate.Month, Date1.MiladiDate.Day, 0, 0, 0);
				}
				return new DateTime(DateOne.MiladiDate.Year, DateOne.MiladiDate.Month, DateOne.MiladiDate.Day, 0, 0, 0);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateM1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 139);
				return new DateTime(DateOne.MiladiDate.Year, DateOne.MiladiDate.Month, DateOne.MiladiDate.Day, 0, 0, 0);
			}
		}
	}

	public DateTime DateM2
	{
		get
		{
			try
			{
				if (RBtnBeinDa.Checked)
				{
					return new DateTime(Date2.MiladiDate.Year, Date2.MiladiDate.Month, Date2.MiladiDate.Day, 23, 59, 59);
				}
				return new DateTime(DateOne.MiladiDate.Year, DateOne.MiladiDate.Month, DateOne.MiladiDate.Day, 23, 59, 59);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DateM2", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 162);
				return new DateTime(DateOne.MiladiDate.Year, DateOne.MiladiDate.Month, DateOne.MiladiDate.Day, 23, 59, 59);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_cancellation?.Cancel();
				_cancellation?.Dispose();
				_cancellation = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 34);
		}
	}

	public async Task LoadDataAsync(CancellationToken cancellationToken)
	{
		try
		{
			List<DeliveryStatusForDisplay> data = new List<DeliveryStatusForDisplay>();
			string dateSh = DateTime.Now.MiladiToShamsi();
			Date2.MiladiDate = DateTime.Now;
			Date1.GotoStartOfMonth((enMonth)Calendar.GetMonthOfDate(dateSh), Calendar.GetYearOfDateSh(dateSh));
			DateOne.MiladiDate = DateTime.Now;
			foreach (DeliveryStatus value in Enum.GetValues(typeof(DeliveryStatus)))
			{
				data.Add(new DeliveryStatusForDisplay(value));
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					chbST.Checked = true;
					switch (_Type)
					{
					case ReportType.RPTInbox:
						((Control)this).Text = "گزارش پیامک های ورودی";
						break;
					case ReportType.RPTOutBox:
						((Control)this).Text = " گزارش پیامک های در حال ارسال";
						RBtnBeinDa.Checked = true;
						break;
					case ReportType.RPTSentBox:
						((Control)this).Text = "گزارش پیامک های ارسال شده";
						break;
					}
					BindingStatus.DataSource = data;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 82);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 88);
		}
	}

	public FRMFilter(string contextGuid, ReportType Type)
		: base(contextGuid)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		FRMFilter fRMFilter = this;
		((Control)this).KeyDown += new KeyEventHandler(FRMFilter_KeyDown);
		try
		{
			InitializeComponent();
			_Type = Type;
			_cancellation?.Cancel();
			_cancellation = new CancellationTokenSource(60000);
			CancellationToken token = _cancellation.Token;
			Task.Run(() => fRMFilter.LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 105);
		}
	}

	private void FRMFilter_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Invalid comparison between Unknown and I4
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Invalid comparison between Unknown and I4
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Invalid comparison between Unknown and I4
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 27)
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if ((int)keyCode == 13)
			{
				e.Handled = true;
				SendKeys.Send("{Tab}");
			}
			else if ((int)keyCode == 112)
			{
				e.Handled = true;
				string strFilter = "*/01/*";
				FilterMonth(strFilter);
			}
			else if ((int)keyCode == 113)
			{
				e.Handled = true;
				string strFilter2 = "*/02/*";
				FilterMonth(strFilter2);
			}
			else if ((int)keyCode == 114)
			{
				e.Handled = true;
				string strFilter3 = "*/03/*";
				FilterMonth(strFilter3);
			}
			else if ((int)keyCode == 115)
			{
				e.Handled = true;
				string strFilter4 = "*/04/*";
				FilterMonth(strFilter4);
			}
			else if ((int)keyCode == 116)
			{
				e.Handled = true;
				string strFilter5 = "*/05/*";
				FilterMonth(strFilter5);
			}
			else if ((int)keyCode == 117)
			{
				e.Handled = true;
				string strFilter6 = "*/06/*";
				FilterMonth(strFilter6);
			}
			else if ((int)keyCode == 118)
			{
				e.Handled = true;
				string strFilter7 = "*/07/*";
				FilterMonth(strFilter7);
			}
			else if ((int)keyCode == 119)
			{
				e.Handled = true;
				string strFilter8 = "*/08/*";
				FilterMonth(strFilter8);
			}
			else if ((int)keyCode == 120)
			{
				e.Handled = true;
				string strFilter9 = "*/09/*";
				FilterMonth(strFilter9);
			}
			else if ((int)keyCode == 121)
			{
				e.Handled = true;
				string strFilter10 = "*/10/*";
				FilterMonth(strFilter10);
			}
			else if ((int)keyCode == 122)
			{
				e.Handled = true;
				string strFilter11 = "*/11/*";
				FilterMonth(strFilter11);
			}
			else if ((int)keyCode == 123)
			{
				e.Handled = true;
				string strFilter12 = "*/12/*";
				FilterMonth(strFilter12);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMFilter_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 288);
		}
	}

	public void FilterMonth(string strFilter)
	{
		try
		{
			short shamsiMonth = short.Parse(strFilter.Substring(2, 2));
			RBtnBeinDa.Checked = true;
			Date2.GotoEndOfMonthInThisYear((enMonth)shamsiMonth);
			Date1.GotoStartOfMonthInThisYear((enMonth)shamsiMonth);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FilterMonth", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 300);
		}
	}

	private async void BTNOK_Click_1(object sender, EventArgs e)
	{
		try
		{
			payamakFilterModel = new PayamakFilterModel(DateM1, DateM2, Current.DeliverySt, TextM, NUMFE, NumGI);
			Font font = await Font.GetFontProgramAsync(default(CancellationToken));
			switch (_Type)
			{
			case ReportType.RPTOutBox:
				((Form)new FRMReportOutbox(base.ContextGuid, payamakFilterModel, font)).ShowDialog((IWin32Window)(object)this);
				break;
			case ReportType.RPTSentBox:
				((Form)new FRMReportSent(base.ContextGuid, payamakFilterModel, font)).ShowDialog((IWin32Window)(object)this);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNOK_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 337);
		}
	}

	private void TxtNumF_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string nUMFE = ((Control)TxtNumF).Text.FixString().Trim();
			NUMFE = nUMFE;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumF_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 350);
		}
	}

	private void TxtNumG_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string numGI = ((Control)TxtNumG).Text.FixString().Trim();
			NumGI = numGI;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtNumG_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 363);
		}
	}

	private void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string textM = ((Control)TXTSearch).Text.FixString().Trim();
			TextM = textM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 378);
		}
	}

	private void CMBST_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)CMBST).Enabled)
			{
				Current.DeliverySt = DeliveryStatus.Unknow;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CMBST_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 397);
		}
	}

	private void RbtnAtDay_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnAtDay.Checked)
			{
				((Control)DateOne).Enabled = true;
			}
			else
			{
				((Control)DateOne).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RbtnAtDay_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 416);
		}
	}

	private void RBtnBeinDa_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RBtnBeinDa.Checked)
			{
				((Control)labelX1).Enabled = true;
				((Control)labelX2).Enabled = true;
				((Control)Date1).Enabled = true;
				((Control)Date2).Enabled = true;
			}
			else
			{
				((Control)labelX1).Enabled = false;
				((Control)labelX2).Enabled = false;
				((Control)Date1).Enabled = false;
				((Control)Date2).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RBtnBeinDa_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 441);
		}
	}

	private void chbST_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chbST.Checked)
			{
				((Control)CMBST).Enabled = true;
				return;
			}
			((Control)CMBST).Enabled = false;
			Current.DeliverySt = DeliveryStatus.Unknow;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "chbST_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 462);
		}
	}

	private void LblFar_Click_1(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/01/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblFar_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 474);
		}
	}

	private void LblOr_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/02/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblOr_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 486);
		}
	}

	private void LblKh_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/03/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblKh_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 498);
		}
	}

	private void Lbltir_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/04/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Lbltir_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 510);
		}
	}

	private void LblMor_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/05/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblMor_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 522);
		}
	}

	private void LblSha_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/06/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblSha_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 534);
		}
	}

	private void LblMehr_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/07/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblMehr_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 546);
		}
	}

	private void LblAba_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/08/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblAba_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 558);
		}
	}

	private void LblAza_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/09/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblAza_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 570);
		}
	}

	private void Lblday_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/10/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Lblday_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 582);
		}
	}

	private void LblBah_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/11/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblBah_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 594);
		}
	}

	private void LblEs_Click(object sender, EventArgs e)
	{
		try
		{
			FilterMonth("*/12/*");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LblEs_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 606);
		}
	}

	private void FRMFilter_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.Yellow;
			((Control)sender).Select();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMFilter_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 619);
		}
	}

	private void FRMFilter_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMFilter_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\Reports\\FRMFilter.cs", 631);
		}
	}

	private void FRMFilter_Load(object sender, EventArgs e)
	{
	}

	private void FRMFilter_Load_1(object sender, EventArgs e)
	{
	}

	private void BindingStatus_CurrentChanged(object sender, EventArgs e)
	{
		Current = (DeliveryStatusForDisplay)BindingStatus.Current;
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_1d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d37: Expected O, but got Unknown
		//IL_1d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6d: Expected O, but got Unknown
		//IL_1ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef0: Expected O, but got Unknown
		//IL_1f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f26: Expected O, but got Unknown
		//IL_2090: Unknown result type (might be due to invalid IL or missing references)
		//IL_209a: Expected O, but got Unknown
		//IL_20c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d0: Expected O, but got Unknown
		components = new Container();
		PanelEx2 = new PanelEx();
		GroupPanel4 = new GroupPanel();
		CMBST = new ComboBox();
		BindingStatus = new BindingSource(components);
		chbST = new CheckBox();
		GroupPanel3 = new GroupPanel();
		LX1_Sent = new Label();
		TXTSearch = new TextBoxX();
		GroupPanel2 = new GroupPanel();
		TxtNumF = new TextBoxX();
		LabelX11 = new Label();
		TxtNumG = new TextBoxX();
		LabelX12 = new Label();
		GPMonth = new GroupPanel();
		LblAza = new Label();
		LblEs = new Label();
		LblAba = new Label();
		LblBah = new Label();
		LblMehr = new Label();
		Lblday = new Label();
		LblSha = new Label();
		LblMor = new Label();
		Lbltir = new Label();
		LblKh = new Label();
		LblOr = new Label();
		LblFar = new Label();
		BTNOK = new ButtonX();
		GroupPanel1 = new GroupPanel();
		labelX2 = new Label();
		labelX1 = new Label();
		Date1 = new DateBoxFa();
		Date2 = new DateBoxFa();
		DateOne = new DateBoxFa();
		RBtnBeinDa = new RadioButton();
		RbtnAtDay = new RadioButton();
		CmbResideG = new ComboItem();
		CmbResidem = new ComboItem();
		CmbNM = new ComboItem();
		CmbUnknown = new ComboItem();
		CmbNG = new ComboItem();
		CmbBlack = new ComboItem();
		CmbRead = new ComboItem();
		CmbUnRead = new ComboItem();
		CmbReply = new ComboItem();
		CmbOutBoxG = new ComboItem();
		CmbOutBoxP = new ComboItem();
		CMBCansel = new ComboItem();
		CmbWaiting = new ComboItem();
		((Control)PanelEx2).SuspendLayout();
		((Control)GroupPanel4).SuspendLayout();
		((ISupportInitialize)BindingStatus).BeginInit();
		((Control)GroupPanel3).SuspendLayout();
		((Control)GroupPanel2).SuspendLayout();
		((Control)GPMonth).SuspendLayout();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)PanelEx2).Controls.Add((Control)(object)GroupPanel4);
		((Control)PanelEx2).Controls.Add((Control)(object)GroupPanel3);
		((Control)PanelEx2).Controls.Add((Control)(object)GroupPanel2);
		((Control)PanelEx2).Controls.Add((Control)(object)GPMonth);
		((Control)PanelEx2).Controls.Add((Control)(object)BTNOK);
		((Control)PanelEx2).Controls.Add((Control)(object)GroupPanel1);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Dock = (DockStyle)5;
		((Control)PanelEx2).Location = new Point(0, 0);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(465, 558);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.Color = Color.CornflowerBlue;
		PanelEx2.Style.BackColor2.Color = Color.Lavender;
		PanelEx2.Style.Border = eBorderType.SingleLine;
		PanelEx2.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		PanelEx2.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)GroupPanel4).Anchor = (AnchorStyles)9;
		GroupPanel4.CanvasColor = SystemColors.Control;
		GroupPanel4.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel4).Controls.Add((Control)(object)CMBST);
		((Control)GroupPanel4).Controls.Add((Control)(object)chbST);
		GroupPanel4.DisabledBackColor = Color.Empty;
		((Control)GroupPanel4).Location = new Point(6, 184);
		((Control)GroupPanel4).Name = "GroupPanel4";
		((Control)GroupPanel4).Size = new Size(452, 80);
		GroupPanel4.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel4.Style.BackColorGradientAngle = 90;
		GroupPanel4.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel4.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderBottomWidth = 1;
		GroupPanel4.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel4.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderLeftWidth = 1;
		GroupPanel4.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderRightWidth = 1;
		GroupPanel4.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel4.Style.BorderTopWidth = 1;
		GroupPanel4.Style.CornerDiameter = 4;
		GroupPanel4.Style.CornerType = eCornerType.Rounded;
		GroupPanel4.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel4.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel4.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel4.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel4.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel4).TabIndex = 2;
		CMBST.DataSource = BindingStatus;
		((ListControl)CMBST).DisplayMember = "ResultForDisplay";
		((ListControl)CMBST).FormattingEnabled = true;
		((Control)CMBST).Location = new Point(17, 27);
		((Control)CMBST).Name = "CMBST";
		((Control)CMBST).Size = new Size(345, 21);
		((Control)CMBST).TabIndex = 2;
		((ListControl)CMBST).ValueMember = "DeliverySt";
		BindingStatus.DataSource = typeof(DeliveryStatusForDisplay);
		BindingStatus.CurrentChanged += BindingStatus_CurrentChanged;
		((Control)chbST).Anchor = (AnchorStyles)9;
		((Control)chbST).AutoSize = true;
		((Control)chbST).ForeColor = Color.DarkBlue;
		((Control)chbST).Location = new Point(361, 31);
		((Control)chbST).Name = "chbST";
		((Control)chbST).Size = new Size(65, 17);
		((Control)chbST).TabIndex = 0;
		((Control)chbST).Text = "وضعیت :";
		((ButtonBase)chbST).UseVisualStyleBackColor = true;
		chbST.CheckedChanged += chbST_CheckedChanged;
		((Control)GroupPanel3).Anchor = (AnchorStyles)9;
		GroupPanel3.CanvasColor = SystemColors.Control;
		GroupPanel3.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel3).Controls.Add((Control)(object)LX1_Sent);
		((Control)GroupPanel3).Controls.Add((Control)(object)TXTSearch);
		GroupPanel3.DisabledBackColor = Color.Empty;
		((Control)GroupPanel3).Location = new Point(6, 98);
		((Control)GroupPanel3).Name = "GroupPanel3";
		((Control)GroupPanel3).Size = new Size(452, 80);
		GroupPanel3.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel3.Style.BackColorGradientAngle = 90;
		GroupPanel3.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel3.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel3.Style.BorderBottomWidth = 1;
		GroupPanel3.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel3.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel3.Style.BorderLeftWidth = 1;
		GroupPanel3.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel3.Style.BorderRightWidth = 1;
		GroupPanel3.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel3.Style.BorderTopWidth = 1;
		GroupPanel3.Style.CornerDiameter = 4;
		GroupPanel3.Style.CornerType = eCornerType.Rounded;
		GroupPanel3.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel3.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel3.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel3.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel3.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel3).TabIndex = 1;
		((Control)LX1_Sent).Anchor = (AnchorStyles)9;
		((Control)LX1_Sent).Location = new Point(365, 25);
		((Control)LX1_Sent).Name = "LX1_Sent";
		((Control)LX1_Sent).Size = new Size(32, 22);
		((Control)LX1_Sent).TabIndex = 0;
		((Control)LX1_Sent).Text = "متن :";
		LX1_Sent.TextAlign = (ContentAlignment)16;
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		TXTSearch.Border.Class = "TextBoxBorder";
		TXTSearch.Border.CornerType = eCornerType.Square;
		((Control)TXTSearch).Location = new Point(17, 28);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(345, 20);
		((Control)TXTSearch).TabIndex = 1;
		((TextBoxBase)TXTSearch).Click += TXTSearch_TextChanged;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((Control)GroupPanel2).Anchor = (AnchorStyles)9;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)TxtNumF);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX11);
		((Control)GroupPanel2).Controls.Add((Control)(object)TxtNumG);
		((Control)GroupPanel2).Controls.Add((Control)(object)LabelX12);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Location = new Point(6, 12);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(452, 80);
		GroupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel2.Style.BackColorGradientAngle = 90;
		GroupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderBottomWidth = 1;
		GroupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderLeftWidth = 1;
		GroupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderRightWidth = 1;
		GroupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderTopWidth = 1;
		GroupPanel2.Style.CornerDiameter = 4;
		GroupPanel2.Style.CornerType = eCornerType.Rounded;
		GroupPanel2.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel2).TabIndex = 0;
		((Control)TxtNumF).Anchor = (AnchorStyles)9;
		TxtNumF.Border.Class = "TextBoxBorder";
		TxtNumF.Border.CornerType = eCornerType.Square;
		((Control)TxtNumF).Location = new Point(20, 31);
		((Control)TxtNumF).Name = "TxtNumF";
		((Control)TxtNumF).Size = new Size(123, 20);
		((Control)TxtNumF).TabIndex = 3;
		((Control)TxtNumF).TextChanged += TxtNumF_TextChanged;
		((Control)LabelX11).Anchor = (AnchorStyles)9;
		((Control)LabelX11).Location = new Point(149, 29);
		((Control)LabelX11).Name = "LabelX11";
		((Control)LabelX11).Size = new Size(89, 22);
		((Control)LabelX11).TabIndex = 2;
		((Control)LabelX11).Text = "شماره فرستنده :";
		LabelX11.TextAlign = (ContentAlignment)16;
		((Control)TxtNumG).Anchor = (AnchorStyles)9;
		TxtNumG.Border.Class = "TextBoxBorder";
		TxtNumG.Border.CornerType = eCornerType.Square;
		((Control)TxtNumG).Location = new Point(244, 32);
		((Control)TxtNumG).Name = "TxtNumG";
		((Control)TxtNumG).Size = new Size(120, 20);
		((Control)TxtNumG).TabIndex = 1;
		((Control)TxtNumG).TextChanged += TxtNumG_TextChanged;
		((Control)LabelX12).Anchor = (AnchorStyles)9;
		((Control)LabelX12).Location = new Point(361, 32);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).Size = new Size(82, 20);
		((Control)LabelX12).TabIndex = 0;
		((Control)LabelX12).Text = "شماره گیرنده :";
		LabelX12.TextAlign = (ContentAlignment)16;
		((Control)GPMonth).Anchor = (AnchorStyles)9;
		GPMonth.CanvasColor = SystemColors.Control;
		GPMonth.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GPMonth).Controls.Add((Control)(object)LblAza);
		((Control)GPMonth).Controls.Add((Control)(object)LblEs);
		((Control)GPMonth).Controls.Add((Control)(object)LblAba);
		((Control)GPMonth).Controls.Add((Control)(object)LblBah);
		((Control)GPMonth).Controls.Add((Control)(object)LblMehr);
		((Control)GPMonth).Controls.Add((Control)(object)Lblday);
		((Control)GPMonth).Controls.Add((Control)(object)LblSha);
		((Control)GPMonth).Controls.Add((Control)(object)LblMor);
		((Control)GPMonth).Controls.Add((Control)(object)Lbltir);
		((Control)GPMonth).Controls.Add((Control)(object)LblKh);
		((Control)GPMonth).Controls.Add((Control)(object)LblOr);
		((Control)GPMonth).Controls.Add((Control)(object)LblFar);
		GPMonth.DisabledBackColor = Color.Empty;
		((Control)GPMonth).Location = new Point(6, 373);
		((Control)GPMonth).Name = "GPMonth";
		((Control)GPMonth).Size = new Size(452, 105);
		GPMonth.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GPMonth.Style.BackColorGradientAngle = 90;
		GPMonth.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GPMonth.Style.BorderBottom = eStyleBorderType.Solid;
		GPMonth.Style.BorderBottomWidth = 1;
		GPMonth.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GPMonth.Style.BorderLeft = eStyleBorderType.Solid;
		GPMonth.Style.BorderLeftWidth = 1;
		GPMonth.Style.BorderRight = eStyleBorderType.Solid;
		GPMonth.Style.BorderRightWidth = 1;
		GPMonth.Style.BorderTop = eStyleBorderType.Solid;
		GPMonth.Style.BorderTopWidth = 1;
		GPMonth.Style.CornerDiameter = 4;
		GPMonth.Style.CornerType = eCornerType.Rounded;
		GPMonth.Style.TextAlignment = eStyleTextAlignment.Far;
		GPMonth.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GPMonth.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GPMonth.StyleMouseDown.CornerType = eCornerType.Square;
		GPMonth.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GPMonth).TabIndex = 4;
		((Control)GPMonth).Text = "     ماه   ";
		((Control)LblAza).Anchor = (AnchorStyles)9;
		((Control)LblAza).ForeColor = Color.DarkRed;
		((Control)LblAza).Location = new Point(228, 36);
		((Control)LblAza).Name = "LblAza";
		((Control)LblAza).Size = new Size(54, 23);
		((Control)LblAza).TabIndex = 8;
		((Control)LblAza).Text = "آذر : F9";
		LblAza.TextAlign = (ContentAlignment)16;
		((Control)LblAza).Click += LblAza_Click;
		((Control)LblEs).Anchor = (AnchorStyles)9;
		((Control)LblEs).ForeColor = Color.DarkRed;
		((Control)LblEs).Location = new Point(17, 36);
		((Control)LblEs).Name = "LblEs";
		((Control)LblEs).Size = new Size(63, 23);
		((Control)LblEs).TabIndex = 11;
		((Control)LblEs).Text = "اسفند : F12";
		LblEs.TextAlign = (ContentAlignment)16;
		((Control)LblEs).Click += LblEs_Click;
		((Control)LblAba).Anchor = (AnchorStyles)9;
		((Control)LblAba).ForeColor = Color.DarkRed;
		((Control)LblAba).Location = new Point(288, 36);
		((Control)LblAba).Name = "LblAba";
		((Control)LblAba).Size = new Size(79, 23);
		((Control)LblAba).TabIndex = 7;
		((Control)LblAba).Text = "آبان : F8";
		LblAba.TextAlign = (ContentAlignment)16;
		((Control)LblAba).Click += LblAba_Click;
		((Control)LblBah).Anchor = (AnchorStyles)9;
		((Control)LblBah).ForeColor = Color.DarkRed;
		((Control)LblBah).Location = new Point(95, 36);
		((Control)LblBah).Name = "LblBah";
		((Control)LblBah).Size = new Size(59, 23);
		((Control)LblBah).TabIndex = 10;
		((Control)LblBah).Text = "بهمن : F11";
		LblBah.TextAlign = (ContentAlignment)16;
		((Control)LblBah).Click += LblBah_Click;
		((Control)LblMehr).Anchor = (AnchorStyles)9;
		((Control)LblMehr).ForeColor = Color.DarkRed;
		((Control)LblMehr).Location = new Point(366, 36);
		((Control)LblMehr).Name = "LblMehr";
		((Control)LblMehr).Size = new Size(78, 23);
		((Control)LblMehr).TabIndex = 6;
		((Control)LblMehr).Text = "مهر : F7";
		LblMehr.TextAlign = (ContentAlignment)16;
		((Control)LblMehr).Click += LblMehr_Click;
		((Control)Lblday).Anchor = (AnchorStyles)9;
		((Control)Lblday).ForeColor = Color.DarkRed;
		((Control)Lblday).Location = new Point(160, 36);
		((Control)Lblday).Name = "Lblday";
		((Control)Lblday).Size = new Size(53, 23);
		((Control)Lblday).TabIndex = 9;
		((Control)Lblday).Text = "دی : F10";
		Lblday.TextAlign = (ContentAlignment)16;
		((Control)Lblday).Click += LblAza_Click;
		((Control)LblSha).Anchor = (AnchorStyles)9;
		((Control)LblSha).ForeColor = Color.DarkRed;
		((Control)LblSha).Location = new Point(17, 7);
		((Control)LblSha).Name = "LblSha";
		((Control)LblSha).Size = new Size(63, 23);
		((Control)LblSha).TabIndex = 5;
		((Control)LblSha).Text = "شهریور : F6";
		LblSha.TextAlign = (ContentAlignment)16;
		((Control)LblSha).Click += LblMor_Click;
		((Control)LblMor).Anchor = (AnchorStyles)9;
		((Control)LblMor).ForeColor = Color.DarkRed;
		((Control)LblMor).Location = new Point(95, 7);
		((Control)LblMor).Name = "LblMor";
		((Control)LblMor).Size = new Size(59, 23);
		((Control)LblMor).TabIndex = 4;
		((Control)LblMor).Text = "مرداد : F5";
		LblMor.TextAlign = (ContentAlignment)16;
		((Control)LblMor).Click += LblMor_Click;
		((Control)Lbltir).Anchor = (AnchorStyles)9;
		((Control)Lbltir).ForeColor = Color.DarkRed;
		((Control)Lbltir).Location = new Point(158, 7);
		((Control)Lbltir).Name = "Lbltir";
		((Control)Lbltir).Size = new Size(55, 23);
		((Control)Lbltir).TabIndex = 3;
		((Control)Lbltir).Text = "تیر : F4";
		Lbltir.TextAlign = (ContentAlignment)16;
		((Control)Lbltir).Click += Lbltir_Click;
		((Control)LblKh).Anchor = (AnchorStyles)9;
		((Control)LblKh).ForeColor = Color.DarkRed;
		((Control)LblKh).Location = new Point(228, 7);
		((Control)LblKh).Name = "LblKh";
		((Control)LblKh).Size = new Size(54, 23);
		((Control)LblKh).TabIndex = 2;
		((Control)LblKh).Text = "خرداد : F3";
		LblKh.TextAlign = (ContentAlignment)16;
		((Control)LblKh).Click += LblKh_Click;
		((Control)LblOr).Anchor = (AnchorStyles)9;
		((Control)LblOr).ForeColor = Color.DarkRed;
		((Control)LblOr).Location = new Point(288, 7);
		((Control)LblOr).Name = "LblOr";
		((Control)LblOr).Size = new Size(79, 23);
		((Control)LblOr).TabIndex = 1;
		((Control)LblOr).Text = "اردیبهشت  : F2";
		LblOr.TextAlign = (ContentAlignment)16;
		((Control)LblOr).Click += LblOr_Click;
		((Control)LblFar).Anchor = (AnchorStyles)9;
		((Control)LblFar).ForeColor = Color.DarkRed;
		((Control)LblFar).Location = new Point(365, 7);
		((Control)LblFar).Name = "LblFar";
		((Control)LblFar).Size = new Size(78, 23);
		((Control)LblFar).TabIndex = 0;
		((Control)LblFar).Text = "فروردین : F1";
		LblFar.TextAlign = (ContentAlignment)16;
		((Control)LblFar).Click += LblFar_Click_1;
		((Control)BTNOK).AccessibleRole = (AccessibleRole)43;
		((Control)BTNOK).Anchor = (AnchorStyles)14;
		BTNOK.ColorTable = eButtonColor.Office2007WithBackground;
		BTNOK.ImagePosition = eImagePosition.Right;
		((Control)BTNOK).Location = new Point(9, 491);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(449, 55);
		BTNOK.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNOK).TabIndex = 5;
		((Control)BTNOK).Text = "تهیه گزارش";
		((Control)BTNOK).Click += BTNOK_Click_1;
		((Control)GroupPanel1).Anchor = (AnchorStyles)9;
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX2);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)Date1);
		((Control)GroupPanel1).Controls.Add((Control)(object)Date2);
		((Control)GroupPanel1).Controls.Add((Control)(object)DateOne);
		((Control)GroupPanel1).Controls.Add((Control)(object)RBtnBeinDa);
		((Control)GroupPanel1).Controls.Add((Control)(object)RbtnAtDay);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(6, 269);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(452, 98);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Far;
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 3;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).Enabled = false;
		((Control)labelX2).ForeColor = Color.Black;
		((Control)labelX2).Location = new Point(123, 47);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(45, 23);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "تا تاریخ :";
		labelX2.TextAlign = (ContentAlignment)16;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).Enabled = false;
		((Control)labelX1).ForeColor = Color.Black;
		((Control)labelX1).Location = new Point(279, 49);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(43, 23);
		((Control)labelX1).TabIndex = 3;
		((Control)labelX1).Text = "از تاریخ :";
		labelX1.TextAlign = (ContentAlignment)16;
		Date1.AllowNull = false;
		((Control)Date1).Anchor = (AnchorStyles)9;
		Date1.BackcolorTooltip = Color.Red;
		Date1.Border.CornerType = eCornerType.Square;
		((Control)Date1).Enabled = false;
		Date1.EnterToTab = true;
		Date1.EscToClose = true;
		Date1.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		Date1.FocusBackColor = Color.Yellow;
		Date1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		Date1.FocusForeColor = Color.Blue;
		Date1.FocusTextSelect = true;
		((Control)Date1).Font = new Font("Times New Roman", 12f);
		((Control)Date1).Location = new Point(172, 45);
		Date1.MessageEmptyShowDialog = false;
		Date1.MessegeEmpty = "";
		Date1.MessegeEmptyInFormRight = true;
		Date1.MessegeEmptyShowInForm = false;
		Date1.MiladiDate = new DateTime(2025, 7, 11, 11, 29, 56, 606);
		((Control)Date1).Name = "Date1";
		((Control)Date1).RightToLeft = (RightToLeft)0;
		Date1.ShowNameOfDayOnToolTip = false;
		Date1.ShowToolTipText = false;
		((Control)Date1).Size = new Size(100, 20);
		((Control)Date1).TabIndex = 5;
		((Control)Date1).Text = "7/10/2025 ";
		((TextBox)Date1).TextAlign = (HorizontalAlignment)2;
		Date1.TootTipDelay = 500;
		Date2.AllowNull = false;
		((Control)Date2).Anchor = (AnchorStyles)9;
		Date2.BackcolorTooltip = Color.Red;
		Date2.Border.CornerType = eCornerType.Square;
		((Control)Date2).Enabled = false;
		Date2.EnterToTab = true;
		Date2.EscToClose = true;
		Date2.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		Date2.FocusBackColor = Color.Yellow;
		Date2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		Date2.FocusForeColor = Color.Blue;
		Date2.FocusTextSelect = true;
		((Control)Date2).Font = new Font("Times New Roman", 12f);
		((Control)Date2).Location = new Point(17, 47);
		Date2.MessageEmptyShowDialog = false;
		Date2.MessegeEmpty = "";
		Date2.MessegeEmptyInFormRight = true;
		Date2.MessegeEmptyShowInForm = false;
		Date2.MiladiDate = new DateTime(2025, 7, 11, 11, 29, 56, 606);
		((Control)Date2).Name = "Date2";
		((Control)Date2).RightToLeft = (RightToLeft)0;
		Date2.ShowNameOfDayOnToolTip = false;
		Date2.ShowToolTipText = false;
		((Control)Date2).Size = new Size(100, 20);
		((Control)Date2).TabIndex = 6;
		((Control)Date2).Text = "7/10/2025 ";
		((TextBox)Date2).TextAlign = (HorizontalAlignment)2;
		Date2.TootTipDelay = 500;
		DateOne.AllowNull = false;
		((Control)DateOne).Anchor = (AnchorStyles)9;
		DateOne.BackcolorTooltip = Color.Red;
		DateOne.Border.CornerType = eCornerType.Square;
		DateOne.EnterToTab = true;
		DateOne.EscToClose = true;
		DateOne.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		DateOne.FocusBackColor = Color.Yellow;
		DateOne.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		DateOne.FocusForeColor = Color.Blue;
		DateOne.FocusTextSelect = true;
		((Control)DateOne).Font = new Font("Times New Roman", 12f);
		((Control)DateOne).Location = new Point(172, 11);
		DateOne.MessageEmptyShowDialog = false;
		DateOne.MessegeEmpty = "";
		DateOne.MessegeEmptyInFormRight = true;
		DateOne.MessegeEmptyShowInForm = false;
		DateOne.MiladiDate = new DateTime(2025, 7, 11, 11, 29, 56, 606);
		((Control)DateOne).Name = "DateOne";
		((Control)DateOne).RightToLeft = (RightToLeft)0;
		DateOne.ShowNameOfDayOnToolTip = false;
		DateOne.ShowToolTipText = false;
		((Control)DateOne).Size = new Size(100, 20);
		((Control)DateOne).TabIndex = 1;
		((Control)DateOne).Text = "7/10/2025 ";
		((TextBox)DateOne).TextAlign = (HorizontalAlignment)2;
		DateOne.TootTipDelay = 500;
		((Control)RBtnBeinDa).Anchor = (AnchorStyles)9;
		((Control)RBtnBeinDa).AutoSize = true;
		((Control)RBtnBeinDa).Location = new Point(335, 48);
		((Control)RBtnBeinDa).Name = "RBtnBeinDa";
		((Control)RBtnBeinDa).Size = new Size(91, 17);
		((Control)RBtnBeinDa).TabIndex = 2;
		((Control)RBtnBeinDa).Text = "مابین دو تاریخ";
		((ButtonBase)RBtnBeinDa).UseVisualStyleBackColor = true;
		RBtnBeinDa.CheckedChanged += RBtnBeinDa_CheckedChanged;
		((Control)RbtnAtDay).Anchor = (AnchorStyles)9;
		((Control)RbtnAtDay).AutoSize = true;
		RbtnAtDay.Checked = true;
		((Control)RbtnAtDay).Location = new Point(363, 11);
		((Control)RbtnAtDay).Name = "RbtnAtDay";
		((Control)RbtnAtDay).Size = new Size(63, 17);
		((Control)RbtnAtDay).TabIndex = 0;
		RbtnAtDay.TabStop = true;
		((Control)RbtnAtDay).Text = "در تاریخ";
		((ButtonBase)RbtnAtDay).UseVisualStyleBackColor = true;
		RbtnAtDay.CheckedChanged += RbtnAtDay_CheckedChanged;
		CmbResideG.FontSize = 12f;
		CmbResideG.FontStyle = (FontStyle)1;
		CmbResideG.Text = "رسیده به گوشی";
		CmbResidem.FontSize = 12f;
		CmbResidem.FontStyle = (FontStyle)1;
		CmbResidem.Text = "رسیده به مخابرات";
		CmbNM.FontSize = 12f;
		CmbNM.FontStyle = (FontStyle)1;
		CmbNM.Text = "نرسیده به مخابرات";
		CmbUnknown.FontSize = 12f;
		CmbUnknown.FontStyle = (FontStyle)1;
		CmbUnknown.Text = "نامشخص";
		CmbNG.FontSize = 12f;
		CmbNG.FontStyle = (FontStyle)1;
		CmbNG.Text = "نرسیده به گوشی";
		CmbBlack.FontSize = 12f;
		CmbBlack.FontStyle = (FontStyle)1;
		CmbBlack.Text = "لیست سیاه";
		CmbRead.FontSize = 12f;
		CmbRead.FontStyle = (FontStyle)1;
		CmbRead.Text = "خوانده شده";
		CmbUnRead.FontSize = 12f;
		CmbUnRead.FontStyle = (FontStyle)1;
		CmbUnRead.Text = "خوانده نشده";
		CmbReply.FontSize = 12f;
		CmbReply.FontStyle = (FontStyle)1;
		CmbReply.Text = "پاسخ داده شده";
		CmbOutBoxG.FontSize = 12f;
		CmbOutBoxG.FontStyle = (FontStyle)1;
		CmbOutBoxG.Text = "در حال ارسال از طریق مودم GSM";
		CmbOutBoxP.FontSize = 12f;
		CmbOutBoxP.FontStyle = (FontStyle)1;
		CmbOutBoxP.Text = "در حال ارسال از طریق پنل";
		CMBCansel.FontSize = 12f;
		CMBCansel.FontStyle = (FontStyle)1;
		CMBCansel.Text = "کنسل شده";
		CmbWaiting.FontSize = 12f;
		CmbWaiting.FontStyle = (FontStyle)1;
		CmbWaiting.Text = "در انتظار تحویل به گوشی";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(465, 558);
		((Control)this).Controls.Add((Control)(object)PanelEx2);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMFilter";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += FRMFilter_Load_1;
		((Control)PanelEx2).ResumeLayout(false);
		((Control)GroupPanel4).ResumeLayout(false);
		((Control)GroupPanel4).PerformLayout();
		((ISupportInitialize)BindingStatus).EndInit();
		((Control)GroupPanel3).ResumeLayout(false);
		((Control)GroupPanel2).ResumeLayout(false);
		((Control)GPMonth).ResumeLayout(false);
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
