using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmCopyGharardads : FormExtention
{
	private List<GharardadNew> _lstGharardads;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private NumericUpDown numberCod;

	internal DateBoxFa TXTDateRegister;

	private LabelX labelX12;

	private LabelX labelX3;

	internal DateBoxFa TxtEndDate;

	internal DateBoxFa TXTStartDate;

	internal LabelX labelX8;

	internal LabelX labelX10;

	private TabItem tabItem1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabItem tabItem2;

	private CheckBoxX chbShift;

	private CheckBoxX chbBaseSalary;

	private CheckBoxX chbMazayaKosorat;

	private Label TamdidContractLbl1;

	internal LabelX lblSanavat;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmCopyGharardads(string contextGuid, List<GharardadNew> plst)
		: base(contextGuid, true, true)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		_lstGharardads = plst;
		InitializeComponent();
		try
		{
			((Control)TXTStartDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now).Substring(0, 5) + "01/01";
			((Control)TxtEndDate).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now).Substring(0, 5) + "12/29";
			TXTDateRegister.MiladiDate = DateTime.Now;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 44);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			int _numbercode = await GharardadNew.NextGharardadAsync(((CacheBaseForm)this).ContextGuid, token);
			numberCod.Value = _numbercode;
			List<GharardadNew> list = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, _lstGharardads.Select((GharardadNew p) => p.PersonId).ToList(), DateTime.Now.AddYears(-30), DateTime.Now.AddYears(5), token);
			int sanavatMontth = 0;
			if (list != null)
			{
				foreach (GharardadNew item in list)
				{
					sanavatMontth += (int)(item.DateFinishM - item.DateStartM).TotalDays / 30;
				}
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						numberCod.Value = _numbercode;
						if (sanavatMontth > 0)
						{
							((Control)lblSanavat).Text = $"سنوات قراردادهای قبل = {sanavatMontth} ماه";
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task<bool> ValidObjAsync(CancellationToken token = default(CancellationToken))
	{
		bool result = ((Control)BtnAccept).Enabled && ((Control)BtnAccept).Visible;
		try
		{
			if (result && ((Control)numberCod).Text.Trim() == "")
			{
				Utility.MessageBoxShowError("[لطفا شماره شروع قرارداد ها را وارد کنید]", ((CacheBaseForm)this).ContextGuid);
				tabControl1.SelectedTabIndex = 0;
				((Control)numberCod).Focus();
				result = false;
			}
			bool flag = result;
			if (flag)
			{
				decimal value = numberCod.Value;
				flag = value < (decimal)(await GharardadNew.NextGharardadAsync(((CacheBaseForm)this).ContextGuid, token));
			}
			if (flag)
			{
				Utility.MessageBoxShowError("[شماره شروع قرارداد ها بایستی از آخرین شماره قرارداد بزرگتر باشد]", ((CacheBaseForm)this).ContextGuid);
				tabControl1.SelectedTabIndex = 0;
				((Control)numberCod).Focus();
				result = false;
			}
			string text = "";
			foreach (GharardadNew lstGharardad in _lstGharardads)
			{
				DateBoxFa tXTStartDate = TXTStartDate;
				if (((tXTStartDate != null) ? ((Control)tXTStartDate).Text : null) != null && lstGharardad != null)
				{
					_ = lstGharardad.DateFinishM;
					if (lstGharardad.DateFinishM >= Utilities.CalendarToMiladi(((Control)TXTStartDate).Text))
					{
						text = text + lstGharardad.No + ";";
					}
				}
			}
			if (result && text != "")
			{
				Utility.MessageBoxShowError("[تاریخ پایان تعدادی از قرارداد ها ] ( " + text.TrimEnd(new char[1] { ';' }) + " ) [از تاریخ شروع قراردادهای جدید بزرگتر است. تاریخ شروع و پایان قراردادها را بررسی نمائید]", ((CacheBaseForm)this).ContextGuid);
				tabControl1.SelectedTabIndex = 0;
				DateBoxFa tXTStartDate2 = TXTStartDate;
				if (tXTStartDate2 != null)
				{
					((Control)tXTStartDate2).Focus();
				}
				result = false;
			}
			if (TXTStartDate != null && result && TxtEndDate.MiladiDate < TXTStartDate.MiladiDate.AddDays(29.0))
			{
				Utility.MessageBoxShowError("[تاریخ پایان قراردادها حداقل باید یک ماه از تاریخ شروع بزرگتر باشد]", ((CacheBaseForm)this).ContextGuid);
				tabControl1.SelectedTabIndex = 0;
				((Control)TxtEndDate).Focus();
				result = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidObjAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 155);
		}
		return result;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			List<FishHesab> lstHesabs = null;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (!(await ValidObjAsync(token)))
			{
				return;
			}
			if (chbMazayaKosorat.Checked)
			{
				lstHesabs = await FishHesab.SelectAllAsync(((CacheBaseForm)this).ContextGuid, getAllField: true, token);
			}
			int gNo = (int)numberCod.Value;
			foreach (GharardadNew item in _lstGharardads)
			{
				item.ID = 0;
				item.No = gNo.ToString();
				item.DateStartM = TXTStartDate.MiladiDate;
				item.DateFinishM = TxtEndDate.MiladiDate;
				item.DateM = TXTDateRegister.MiladiDate;
				if (chbBaseSalary.Checked)
				{
					item.SalaryType = 2;
					decimal salaryPrice = item.SalaryPrice;
					TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
					LaborLaw laborLaw = timeAttendanceSettings.LaborLaw;
					item.SetSalaryPrice(Convert.ToDecimal((int)laborLaw.DailyWage));
					decimal num = Convert.ToDecimal((int)(salaryPrice * 60m / (decimal)item.LeaveMinutes));
					decimal num3 = (item.PriceOverTime3 = num * laborLaw.OvertimeCoefficient);
					decimal priceOverTime = (item.PriceOverTime2 = num3);
					item.PriceOverTime = priceOverTime;
					num3 = (item.PriceLowTime3 = num);
					priceOverTime = (item.PriceLowTime2 = num3);
					item.PriceLowTime = priceOverTime;
					if (item.PriceHoliday > 0m)
					{
						item.PriceHoliday = num * ((decimal)(timeAttendanceSettings.ZaribFriday + 100) / 100m);
					}
					if (item.PriceTatil > 0m)
					{
						item.PriceTatil = num * ((decimal)(timeAttendanceSettings.ZaribTatil + 100) / 100m);
					}
					if (item.PriceNight > 0m)
					{
						item.PriceNight = num * 1.35m;
					}
					if (item.PriceMission > 0m)
					{
						item.PriceMission = num * 1.2m;
					}
					if (item.PriceAbsent > 0m)
					{
						item.PriceAbsent = salaryPrice;
					}
					if (item.PriceLeave > 0m)
					{
						item.PriceLeave = num;
					}
					if (item.PriceNobatKari1 > 0m)
					{
						item.PriceNobatKari1 = num * 1.1m;
					}
					if (item.PriceNobatKari2 > 0m)
					{
						item.PriceNobatKari2 = num * 1.15m;
					}
					if (item.PriceNobatKari3 > 0m)
					{
						item.PriceNobatKari3 = num * 1.225m;
					}
				}
				if (chbMazayaKosorat.Checked)
				{
					foreach (GharardadDetNew detNew in item.ListGharardadDet)
					{
						FishHesab fishHesab = lstHesabs?.FirstOrDefault((FishHesab q) => q.Id == detNew.HesabId);
						if (fishHesab != null)
						{
							detNew.nValue = fishHesab.Account;
							detNew.Account = fishHesab.Account;
						}
					}
				}
				item.LeaveSaved = 0.0;
				await item.SaveAsync(token);
				gNo++;
			}
			((FormExtention)this).ResetControlChanged();
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 220);
		}
	}

	private void chbBaseSalary_CheckValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (chbBaseSalary.Checked)
			{
				Utility.MessageBoxShow("[در صورتیکه مطمئن هستید مبالغ حقوق قانون کار در منوی امکانات -گزینه تنظیمات نرم افزار -صفحه تنظیمات اطلاعات حقوقی، بر اساس سال جاری بروز شده است تمدید قرارداد را ادامه دهید ]", ((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chbBaseSalary_CheckValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 234);
		}
	}

	private void chbMazayaKosorat_CheckValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (chbMazayaKosorat.Checked)
			{
				Utility.MessageBoxShow("[اگر مطمئن هستید مبالغ پایه مزایا و کسورات را در منوی تعاریف زیر منوی تعریف حسابها بروزرسانی نموده اید تمدید قرارداد را ادامه دهید]", ((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chbMazayaKosorat_CheckValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 248);
		}
	}

	private void chbShift_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chbShift.Checked)
			{
				Utility.MessageBoxShowError("[تمدید شیفت بصورت نرم افزاری ممکن است باعث تداخل شیفت ها شود لذا بایستی بصورت دستی شیفت ها را تمدید نمائید \r\n و بعد از تمدید قراردادها، شیفت قراردادها را بصورت دستی برای تک تک قراردادها اصلاح نمائید]", ((CacheBaseForm)this).ContextGuid);
				chbShift.Checked = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chbShift_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmCopyGharardads.cs", 264);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d1: Expected O, but got Unknown
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Expected O, but got Unknown
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Expected O, but got Unknown
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Expected O, but got Unknown
		//IL_0e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e36: Expected O, but got Unknown
		//IL_0e69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Expected O, but got Unknown
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Expected O, but got Unknown
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_1051: Expected O, but got Unknown
		//IL_10ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f8: Expected O, but got Unknown
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e8: Expected O, but got Unknown
		//IL_12b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c2: Expected O, but got Unknown
		//IL_13ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		lblSanavat = new LabelX();
		chbShift = new CheckBoxX();
		chbBaseSalary = new CheckBoxX();
		chbMazayaKosorat = new CheckBoxX();
		numberCod = new NumericUpDown();
		TXTDateRegister = new DateBoxFa();
		labelX12 = new LabelX();
		labelX3 = new LabelX();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		labelX8 = new LabelX();
		labelX10 = new LabelX();
		TamdidContractLbl1 = new Label();
		tabItem1 = new TabItem(components);
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		tabItem2 = new TabItem(components);
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel1).SuspendLayout();
		((ISupportInitialize)numberCod).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(514, 285);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 3;
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Location = new Point(12, 12);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(490, 207);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		((Control)tabControl1).TabStop = false;
		((Control)tabControl1).Text = "tabControl1";
		((Control)tabControlPanel1).Controls.Add((Control)(object)lblSanavat);
		((Control)tabControlPanel1).Controls.Add((Control)(object)chbShift);
		((Control)tabControlPanel1).Controls.Add((Control)(object)chbBaseSalary);
		((Control)tabControlPanel1).Controls.Add((Control)(object)chbMazayaKosorat);
		((Control)tabControlPanel1).Controls.Add((Control)(object)numberCod);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTDateRegister);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX12);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX3);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtEndDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTStartDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX8);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX10);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TamdidContractLbl1);
		((PanelEx)tabControlPanel1).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(490, 181);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		((Control)lblSanavat).BackColor = Color.Transparent;
		((BaseItemControl)lblSanavat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblSanavat).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblSanavat).ForeColor = Color.Green;
		((Control)lblSanavat).Location = new Point(9, 73);
		((Control)lblSanavat).Name = "lblSanavat";
		((Control)lblSanavat).Size = new Size(249, 26);
		((Control)lblSanavat).TabIndex = 12;
		((Control)lblSanavat).Text = "---";
		lblSanavat.TextAlignment = (StringAlignment)2;
		((Control)chbShift).Anchor = (AnchorStyles)9;
		((Control)chbShift).AutoSize = true;
		((Control)chbShift).BackColor = Color.Transparent;
		((BaseItemControl)chbShift).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbShift).Location = new Point(4, 120);
		((Control)chbShift).Name = "chbShift";
		((Control)chbShift).Size = new Size(352, 16);
		((BaseItemControl)chbShift).Style = (eDotNetBarStyle)9;
		((Control)chbShift).TabIndex = 10;
		((Control)chbShift).Text = "[شیفت ها هم بر مبنای تاریخ قراردادها در سال جدید ایجاد شود]";
		((Control)chbShift).Visible = false;
		chbShift.CheckedChanged += chbShift_CheckedChanged;
		((Control)chbBaseSalary).Anchor = (AnchorStyles)9;
		((Control)chbBaseSalary).BackColor = Color.Transparent;
		((BaseItemControl)chbBaseSalary).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbBaseSalary).Location = new Point(11, 76);
		((Control)chbBaseSalary).Name = "chbBaseSalary";
		((Control)chbBaseSalary).RightToLeft = (RightToLeft)1;
		((Control)chbBaseSalary).Size = new Size(471, 16);
		((BaseItemControl)chbBaseSalary).Style = (eDotNetBarStyle)9;
		((Control)chbBaseSalary).TabIndex = 8;
		((Control)chbBaseSalary).Text = "[اعمال حقوق روزانه بر مبنای قانون کار]";
		chbBaseSalary.CheckValueChanged += chbBaseSalary_CheckValueChanged;
		((Control)chbMazayaKosorat).Anchor = (AnchorStyles)9;
		((Control)chbMazayaKosorat).BackColor = Color.Transparent;
		((BaseItemControl)chbMazayaKosorat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbMazayaKosorat).Location = new Point(11, 98);
		((Control)chbMazayaKosorat).Name = "chbMazayaKosorat";
		((Control)chbMazayaKosorat).RightToLeft = (RightToLeft)1;
		((Control)chbMazayaKosorat).Size = new Size(471, 16);
		((BaseItemControl)chbMazayaKosorat).Style = (eDotNetBarStyle)9;
		((Control)chbMazayaKosorat).TabIndex = 9;
		((Control)chbMazayaKosorat).Text = "[مزایا و کسورات براساس قیمت های جدید بروزرسانی شود]";
		chbMazayaKosorat.CheckValueChanged += chbMazayaKosorat_CheckValueChanged;
		((Control)numberCod).Anchor = (AnchorStyles)13;
		((Control)numberCod).Location = new Point(244, 9);
		numberCod.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numberCod.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numberCod).Name = "numberCod";
		((Control)numberCod).Size = new Size(107, 21);
		((Control)numberCod).TabIndex = 1;
		((UpDownBase)numberCod).TextAlign = (HorizontalAlignment)2;
		numberCod.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		TXTDateRegister.AllowNull = false;
		TXTDateRegister.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTDateRegister).Border.CornerType = (eCornerType)1;
		TXTDateRegister.EnterToTab = true;
		TXTDateRegister.EscToClose = true;
		TXTDateRegister.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTDateRegister.FocusBackColor = Color.Yellow;
		TXTDateRegister.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDateRegister.FocusForeColor = Color.Blue;
		TXTDateRegister.FocusTextSelect = true;
		((Control)TXTDateRegister).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTDateRegister).Location = new Point(9, 9);
		TXTDateRegister.MessageEmptyShowDialog = false;
		TXTDateRegister.MessegeEmpty = "";
		TXTDateRegister.MessegeEmptyInFormRight = true;
		TXTDateRegister.MessegeEmptyShowInForm = false;
		TXTDateRegister.MiladiDate = new DateTime(2025, 4, 8, 23, 59, 39, 794);
		((Control)TXTDateRegister).Name = "TXTDateRegister";
		((Control)TXTDateRegister).RightToLeft = (RightToLeft)0;
		TXTDateRegister.ShowNameOfDayOnToolTip = false;
		TXTDateRegister.ShowToolTipText = false;
		((Control)TXTDateRegister).Size = new Size(107, 26);
		((Control)TXTDateRegister).TabIndex = 3;
		((Control)TXTDateRegister).Text = "08/04/2025";
		((TextBox)TXTDateRegister).TextAlign = (HorizontalAlignment)2;
		TXTDateRegister.TootTipDelay = 500;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(357, 9);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(125, 26);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[شماره شروع قرار داد]:";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(122, 9);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(116, 26);
		((Control)labelX3).TabIndex = 2;
		((Control)labelX3).Text = "[,تاریخ ثبت قرارداد ها]:";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtEndDate.AllowNull = false;
		TxtEndDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtEndDate).Border.CornerType = (eCornerType)1;
		TxtEndDate.EnterToTab = true;
		TxtEndDate.EscToClose = true;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtEndDate.FocusBackColor = Color.Yellow;
		TxtEndDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtEndDate.FocusForeColor = Color.Blue;
		TxtEndDate.FocusTextSelect = true;
		((Control)TxtEndDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtEndDate).Location = new Point(9, 41);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2025, 4, 8, 23, 59, 39, 799);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		TxtEndDate.ShowNameOfDayOnToolTip = false;
		TxtEndDate.ShowToolTipText = false;
		((Control)TxtEndDate).Size = new Size(107, 26);
		((Control)TxtEndDate).TabIndex = 7;
		((Control)TxtEndDate).Text = "08/04/2025";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		TXTStartDate.AllowNull = false;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTStartDate).Location = new Point(244, 41);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2025, 4, 8, 23, 59, 39, 801);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(107, 26);
		((Control)TXTStartDate).TabIndex = 5;
		((Control)TXTStartDate).Text = "08/04/2025";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(122, 41);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(116, 26);
		((Control)labelX8).TabIndex = 6;
		((Control)labelX8).Text = "[تاریخ پایان قرارداد ها] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(357, 41);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(125, 26);
		((Control)labelX10).TabIndex = 4;
		((Control)labelX10).Text = "[تاریخ شروع قرارداد ها] :";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)TamdidContractLbl1).Anchor = (AnchorStyles)13;
		((Control)TamdidContractLbl1).BackColor = Color.Transparent;
		TamdidContractLbl1.FlatStyle = (FlatStyle)0;
		((Control)TamdidContractLbl1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)TamdidContractLbl1).Location = new Point(11, 117);
		((Control)TamdidContractLbl1).Name = "TamdidContractLbl1";
		((Control)TamdidContractLbl1).RightToLeft = (RightToLeft)0;
		((Control)TamdidContractLbl1).Size = new Size(476, 51);
		((Control)TamdidContractLbl1).TabIndex = 11;
		((Control)TamdidContractLbl1).Text = "بصورت اتومات امکان انتقال شیفت به سال بعد وجود ندارد . لذا در سال جدید، شیفت ها بایستی بصورت دستی تعریف شوند سپس در قراردادهای تمدید شده، اختصاص شیفت جدید اعمال شود";
		TamdidContractLbl1.TextAlign = (ContentAlignment)64;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 223);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 3;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]  (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 223);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(95, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 2;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[مزایا و کسورات]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Form)this).ClientSize = new Size(514, 285);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmCopyGharardads";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تمدید قراردادها]";
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel1).ResumeLayout(false);
		((Control)tabControlPanel1).PerformLayout();
		((ISupportInitialize)numberCod).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
