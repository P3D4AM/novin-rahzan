using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Bimeh;
using TimeAttendanceUI.Forms.Device;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Other;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Forms.User;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.ReportProducer.UC;
using TimeAttendanceUI.Salary.Forms;
using TimeAttendanceUI.Salary.Shift;
using TimeAttendanceUI.SportClub;
using TimeAttendanceUI.Validators;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmFilter : BaseForm
{
	private List<OrderUser> _listOrder = new List<OrderUser>();

	private TimeAttendanceSettings _setting;

	private TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private readonly ReportFilterEnumType _thisReportType;

	private readonly bool _show;

	private readonly UCTakGharardad _ucTakGharardad1;

	private CancellationTokenSource _tokenSodurFish;

	private CancellationTokenSource _tokenSourceRptAllTaradod;

	private bool _isDisposed;

	private List<int> _lstUsersId;

	private List<int> _personIDs;

	private IContainer components;

	private PanelEx panelEx1;

	private UCDate ucDate1;

	private UCPerson ucPerson1;

	private UCTakPerson ucTakPerson1;

	private UCTakDate ucTakDate1;

	private UCTakDay ucTakDay1;

	private UCDesc ucDesc1;

	private UCUserList ucUserList1;

	private UCTypeVacation ucTypeVacation1;

	internal ButtonX BtnDesign;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public int PersonId { get; set; }

	public string Desc { get; set; } = "";


	public DateTime Today => DateTime.Now.Date.AddDays(1.0).AddSeconds(-1.0);

	public DateTime DateM1 { get; set; } = DateTime.Now;


	public DateTime DateM2 { get; set; } = DateTime.Now;


	public sealed override string Text
	{
		get
		{
			return ((Form)this).Text;
		}
		set
		{
			((Form)this).Text = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_tokenSodurFish?.Cancel();
				_tokenSodurFish?.Dispose();
				_tokenSodurFish = null;
				_tokenSourceRptAllTaradod?.Cancel();
				_tokenSourceRptAllTaradod?.Dispose();
				_tokenSourceRptAllTaradod = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmFilter(string contextGuid, TimeAttendanceSettings settings, int psnId, DateTime date, bool bShowForm, bool bUcDate, short orderDate, bool bUcPerson, short bOrderPerson, bool bUcPersonTak, short bOrderPersonTak, bool bUcDateTak, short nOrderDateTak, bool bUcTakDay, short nOrderucTakDay, bool bUcDesc, short nOrderucDesc, ReportFilterEnumType frmType, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout = null)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected I4, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Invalid comparison between Unknown and I4
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Invalid comparison between Unknown and I4
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected I4, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected I4, but got Unknown
		if (checkout != null)
		{
			_checkout = checkout;
		}
		string text = Calendar.MiladiToCalture(DateTime.Now);
		int num = Utilities.MonthOfDateCalendar(text);
		int num2 = Utilities.YearOfDateCalendar(text);
		string display = Utilities.GetDisplay<ReportFilterEnumType>(frmType);
		string translate = Utilities.GetTranslate("[صدور فیش حقوقی]");
		InitializeComponent();
		ucTakPerson1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
		ucTypeVacation1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
		ucUserList1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
		ucPerson1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
		bool flag;
		switch (frmType - 11)
		{
		default:
			if ((int)frmType == 24 || (int)frmType == 28)
			{
				goto case 0;
			}
			goto case 2;
		case 0:
		case 1:
		case 5:
		case 7:
		case 8:
			flag = true;
			break;
		case 2:
		case 3:
		case 4:
		case 6:
			flag = false;
			break;
		}
		if (flag)
		{
			DateM1 = DateTime.Now;
			DateM2 = DateTime.Now;
			ucDate1.InitDate(DateM1);
		}
		else if (((int)frmType == 9 || (int)frmType == 20) ? true : false)
		{
			DateM1 = LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth();
			DateM2 = LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth();
			ucDate1.InitDate(DateM1, UCDate.DurationDate.BeginToEndOfMonth);
		}
		else if ((int)frmType == 7)
		{
			DateM1 = Utilities.CalendarToMiladi(num2 + "/" + num + "/01");
			DateM2 = DateTime.Now;
			ucDate1.InitDate(DateM2);
			ucDate1.VisibleRBAllGardesh(status: false);
		}
		else
		{
			string text2 = Utilities.AddMonthCalendar(text, -1);
			DateM1 = Utilities.StartDayOfMonthtCalendar(text2, "00:00:00");
			DateM2 = Utilities.EndDayOfMonthtCalendar(text2, "23:59:59");
			ucDate1.InitDate(DateM1);
		}
		ucTakDate1.InitDate(DateM1);
		ucTakDay1.InitDate(DateM1);
		_thisReportType = frmType;
		_checkout = checkout;
		_show = bShowForm;
		_listOrder.Add(new OrderUser("UCDate1", (UserControl)(object)ucDate1, orderDate, bUcDate));
		_listOrder.Add(new OrderUser("ucPerson1", (UserControl)(object)ucPerson1, bOrderPerson, bUcPerson));
		_listOrder.Add(new OrderUser("ucTakPerson1", (UserControl)(object)ucTakPerson1, bOrderPersonTak, bUcPersonTak));
		_listOrder.Add(new OrderUser("ucTakDate1", (UserControl)(object)ucTakDate1, nOrderDateTak, bUcDateTak));
		_listOrder.Add(new OrderUser("ucTakDay1", (UserControl)(object)ucTakDay1, nOrderucTakDay, bUcTakDay));
		_listOrder.Add(new OrderUser("ucUserList1", (UserControl)(object)ucUserList1, -1, pVisible: false));
		_listOrder.Add(new OrderUser("ucDesc1", (UserControl)(object)ucDesc1, nOrderucDesc, bUcDesc));
		_listOrder.Add(new OrderUser("ucTypeVacation1", (UserControl)(object)ucTypeVacation1, -2, pVisible: false));
		((Control)this).Text = display;
		ReportFilterEnumType thisReportType = _thisReportType;
		switch (thisReportType - 7)
		{
		case 14:
			((Control)BtnDesign).Text = translate;
			break;
		case 7:
			ucDate1.SetDateSh1(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 8) + "01");
			ucDate1.SetDateSh2(Calendar.MiladiToCalture(DateTime.Now));
			break;
		case 9:
		case 21:
			ucDate1.SetDateSh1(ucDate1.DateSh1.Substring(0, 5) + "01/01");
			ucDate1.SetDateSh2(ucDate1.DateSh2.Substring(0, 5) + "12/29");
			break;
		case 20:
			ucDate1.SetDateSh1(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 8) + "01");
			ucDate1.SetDateSh2(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 8) + "29");
			break;
		case 5:
			ucDate1.SetDateSh1(ucDate1.DateSh1.Substring(0, 5) + "01/01");
			ucDate1.SetDateSh2(ucDate1.DateSh2.Substring(0, 5) + "12/29");
			break;
		case 8:
			_ucTakGharardad1 = new UCTakGharardad(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, settings);
			InitializeTakGharardad();
			break;
		case 11:
		{
			_ucTakGharardad1 = new UCTakGharardad(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, settings, pShiftProgrammableOnly: true);
			string text3 = Calendar.MiladiToCalture(ucDate1.DateM1);
			text3 = text3.Substring(0, 4) + "/01/01";
			string dateSh = text3.Substring(0, 4) + "/12/29";
			ucDate1.SetDateSh1(text3);
			ucDate1.SetDateSh2(dateSh);
			InitializeTakGharardad();
			break;
		}
		}
		ReDesign();
		if (!bShowForm)
		{
			PersonId = psnId;
			thisReportType = _thisReportType;
			switch (thisReportType - 9)
			{
			case 0:
				ucTakDate1.SetDate(date);
				ucTakPerson1.SetCodeAsync(psnId);
				break;
			case 2:
				ucTakDay1.SetDate(date);
				break;
			case 1:
				ucTakPerson1.SetCodeAsync(psnId);
				break;
			}
		}
	}

	private void FrmFilter_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	public FrmFilter(string contextGuid, TimeAttendanceSettings settings, ReportFilterEnumType frmType)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected I4, but got Unknown
		try
		{
			_setting = settings;
			InitializeComponent();
			ucTakPerson1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
			ucTypeVacation1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
			ucUserList1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
			ucPerson1.SetContextGuid(((CacheBaseForm)this).ContextGuid);
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			text = Utilities.GetDisplay<ReportFilterEnumType>(frmType);
			text2 = Utilities.GetTranslate("[گزارش لاگ عملکرد کاربران]");
			text3 = Utilities.GetTranslate("[قرارداد نمونه را انتخاب نمائید]");
			text4 = Utilities.GetTranslate("[ایجاد قرارداد مشابه قرارداد نمونه تعیین شده برای اشخاص انتخاب شده ]");
			ucTakDate1.InitDate(DateM1);
			ucDate1.InitDate(DateM1);
			ucTakDay1.InitDate(DateM1);
			_thisReportType = frmType;
			switch ((int)frmType)
			{
			case 17:
				_show = true;
				_listOrder.Add(new OrderUser("UCDate1", (UserControl)(object)ucDate1, 0, pVisible: true));
				_listOrder.Add(new OrderUser("ucPerson1", (UserControl)(object)ucPerson1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakPerson1", (UserControl)(object)ucTakPerson1, -2, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDate1", (UserControl)(object)ucTakDate1, -3, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDay1", (UserControl)(object)ucTakDay1, -4, pVisible: false));
				_listOrder.Add(new OrderUser("ucDesc1", (UserControl)(object)ucDesc1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("ucUserList1", (UserControl)(object)ucUserList1, 2, pVisible: true));
				_listOrder.Add(new OrderUser("ucTypeVacation1", (UserControl)(object)ucTypeVacation1, -5, pVisible: false));
				((Control)this).Text = text2;
				ReDesign();
				if (!_show)
				{
					PersonId = 0;
				}
				break;
			case 25:
				((Control)this).Text = text;
				_show = true;
				_listOrder.Add(new OrderUser("ucPerson1", (UserControl)(object)ucPerson1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakPerson1", (UserControl)(object)ucTakPerson1, -2, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDate1", (UserControl)(object)ucTakDate1, -3, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDay1", (UserControl)(object)ucTakDay1, -4, pVisible: false));
				_listOrder.Add(new OrderUser("ucDesc1", (UserControl)(object)ucDesc1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("ucUserList1", (UserControl)(object)ucUserList1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("UCDate1", (UserControl)(object)ucDate1, 1, pVisible: true));
				_listOrder.Add(new OrderUser("ucTypeVacation1", (UserControl)(object)ucTypeVacation1, 2, pVisible: true));
				ReDesign();
				if (!_show)
				{
					PersonId = 0;
				}
				break;
			case 26:
				((Control)this).Text = text;
				_show = true;
				_listOrder.Add(new OrderUser("ucPerson1", (UserControl)(object)ucPerson1, 0, pVisible: true));
				_listOrder.Add(new OrderUser("ucTakPerson1", (UserControl)(object)ucTakPerson1, -2, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDate1", (UserControl)(object)ucTakDate1, -3, pVisible: false));
				_listOrder.Add(new OrderUser("ucTakDay1", (UserControl)(object)ucTakDay1, -4, pVisible: false));
				_listOrder.Add(new OrderUser("ucDesc1", (UserControl)(object)ucDesc1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("ucUserList1", (UserControl)(object)ucUserList1, -1, pVisible: false));
				_listOrder.Add(new OrderUser("UCDate1", (UserControl)(object)ucDate1, -5, pVisible: false));
				_listOrder.Add(new OrderUser("ucTypeVacation1", (UserControl)(object)ucTypeVacation1, -5, pVisible: false));
				_ucTakGharardad1 = new UCTakGharardad(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _setting);
				InitializeTakGharardad();
				_ucTakGharardad1.HeaderText = text3;
				ReDesign();
				if (!_show)
				{
					PersonId = 0;
				}
				((Control)BtnDesign).Text = text4;
				break;
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 27:
			case 28:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 333);
		}
	}

	private void InitializeTakGharardad()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			_ucTakGharardad1.EnalbleControl = true;
			((Control)_ucTakGharardad1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
			((Control)_ucTakGharardad1).Location = new Point(15, 142);
			((Control)_ucTakGharardad1).Name = "ucTakGharardad1";
			((Control)_ucTakGharardad1).RightToLeft = (RightToLeft)1;
			((Control)_ucTakGharardad1).Size = new Size(600, 68);
			((Control)_ucTakGharardad1).TabIndex = 1;
			((Control)panelEx1).Controls.Add((Control)(object)_ucTakGharardad1);
			_listOrder.Add(new OrderUser("ucTakGharardad1", (UserControl)(object)_ucTakGharardad1, 1, pVisible: true));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void ReDesign()
	{
		try
		{
			int num = 15;
			if (_listOrder.Count > 0)
			{
				_listOrder = _listOrder.OrderBy((OrderUser e1) => e1.Order).ToList();
				foreach (OrderUser item in _listOrder)
				{
					if (item.Visible)
					{
						((Control)item.UC).Top = num;
						num += ((Control)item.UC).Height + 10;
					}
					else
					{
						((Control)item.UC).Visible = false;
					}
				}
			}
			num += ((Control)BtnDesign).Height + 50;
			((Control)this).Height = num;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReDesign", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 372);
		}
	}

	private async void BtnDesign_Click(object sender, EventArgs e)
	{
		int line = 1;
		try
		{
			((Control)ucDate1).Focus();
			line = 2;
			if (((Control)ucDate1).Visible)
			{
				line = 3;
				DateM1 = ucDate1.DateM1;
				line = 4;
				DateM2 = ucDate1.DateM2;
			}
			else if (((Control)ucTakDate1).Visible)
			{
				line = 5;
				DateM1 = ucTakDate1.DateM1;
				line = 6;
				DateM2 = ucTakDate1.DateM2;
			}
			else if (((Control)ucTakDay1).Visible)
			{
				line = 7;
				DateM1 = ucTakDay1.DateM.Date;
				line = 8;
				DateM2 = ucTakDay1.DateM;
			}
			line = 9;
			_personIDs = ucPerson1.LstSelected;
			line = 10;
			_lstUsersId = ucUserList1.ListId;
			line = 11;
			PersonId = ucTakPerson1.PID;
			line = 12;
			Desc = ucDesc1.Desc;
			line = 13;
			if (!_show)
			{
				line = 14;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				return;
			}
			line = 15;
			ReportFilterEnumType thisReportType = _thisReportType;
			CancellationToken token = default(CancellationToken);
			switch (thisReportType - 7)
			{
			case 2:
				line = 16;
				if (PersonId <= 0)
				{
					line = 17;
					((Control)ucTakPerson1).Focus();
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[وارد کردن کد صحیح الزامی است]", "", 3);
				}
				else
				{
					line = 18;
					FrmRptTaradodFull frmRptTaradodFull = new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, DateM1, DateM2, PersonId);
					line = 19;
					await ((CacheBaseForm)frmRptTaradodFull).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case 4:
			{
				line = 20;
				FrmRptTaradodFullAll frmRptTaradodFullAll = new FrmRptTaradodFullAll(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _personIDs, DateM1);
				line = 21;
				await ((CacheBaseForm)frmRptTaradodFullAll).ShowAsync((IWin32Window)(object)this);
				break;
			}
			case 0:
			{
				line = 22;
				FrmRptManage frmJame = new FrmRptManage(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _personIDs, DateM1, DateM2);
				line = 23;
				Utilities.CheckVisibility((Form)(object)this, "BtnDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs");
				await ((CacheBaseForm)frmJame).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmJame).Dispose();
				break;
			}
			case 7:
			{
				line = 24;
				FrmFishList frmFishList = new FrmFishList(((CacheBaseForm)this).ContextGuid, _personIDs, DateM1, DateM2, TimeAttendanceSettings.appFont);
				line = 25;
				await ((CacheBaseForm)frmFishList).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmFishList).Dispose();
				break;
			}
			case 5:
			{
				line = 26;
				if (UserAttendance.SelectedUser.AccessLevelMerg != null)
				{
					line = 27;
					if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain)
					{
						line = 28;
						break;
					}
				}
				line = 29;
				FrmMorakhasiDailyMain frmMor = new FrmMorakhasiDailyMain(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _personIDs, DateM1, DateM2, Desc, isCalcSaati: false);
				line = 30;
				await ((CacheBaseForm)frmMor).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmMor).Dispose();
				break;
			}
			case 1:
			case 13:
				((Form)this).DialogResult = (DialogResult)1;
				break;
			case 3:
				await SodorFishAsync();
				break;
			case 8:
			{
				line = 31;
				FrmReportMorakhasi frmReportMorakhasi = new FrmReportMorakhasi(((CacheBaseForm)this).ContextGuid, _ucTakGharardad1.ClsGharardad, TimeAttendanceSettings.appFont);
				line = 32;
				await ((CacheBaseForm)frmReportMorakhasi).ShowAsync((IWin32Window)(object)this);
				break;
			}
			case 11:
			{
				GharardadNew clsGharardad = _ucTakGharardad1.ClsGharardad;
				if (clsGharardad != null && clsGharardad.ID <= 0)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[لطفا شخص مورد نظر را انتخاب نمائید]", "", 3);
					break;
				}
				if (((int?)_ucTakGharardad1.ClsGharardad?.ShiftId).GetValueOrDefault() != -1)
				{
					Utility.MessageBoxShowError("قرارداد انتخاب شده دارای شیفت برنامه ریزی نیست .", ((CacheBaseForm)this).ContextGuid);
					break;
				}
				line = 33;
				FRMWait fRMWait = new FRMWait(((CacheBaseForm)this).ContextGuid);
				line = 34;
				((Control)fRMWait).BringToFront();
				line = 35;
				((Form)fRMWait).Show((IWin32Window)(object)this);
				line = 36;
				Thread.Sleep(10);
				line = 37;
				Application.DoEvents();
				Thread.Sleep(10);
				line = 39;
				_ucTakGharardad1.ClsGharardad.DateStartM = Utilities.CalendarToMiladi(ucDate1.DateSh1);
				line = 40;
				_ucTakGharardad1.ClsGharardad.DateFinishM = Utilities.CalendarToMiladi(ucDate1.DateSh2);
				line = 41;
				FrmShiftProgrammable frmShiftProgrammable = new FrmShiftProgrammable(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _ucTakGharardad1.ClsGharardad.ID, _ucTakGharardad1.ClsGharardad.DateStartM, _ucTakGharardad1.ClsGharardad.DateFinishM);
				line = 42;
				((Component)(object)fRMWait).Dispose();
				line = 43;
				if (!((FormExtention)frmShiftProgrammable).IsDisposed)
				{
					((Form)frmShiftProgrammable).ShowDialog((IWin32Window)(object)this);
				}
				line = 44;
				((Component)(object)frmShiftProgrammable).Dispose();
				((Form)this).DialogResult = (DialogResult)1;
				break;
			}
			case 12:
			{
				line = 45;
				line = 46;
				_tokenSourceRptAllTaradod?.Cancel();
				_tokenSourceRptAllTaradod = new CancellationTokenSource();
				CancellationToken token2 = _tokenSourceRptAllTaradod.Token;
				await new RptAllTaradod(((CacheBaseForm)this).ContextGuid).ReportAsync((IWin32Window)(object)this, DateM1, DateM2, _personIDs, token2);
				break;
			}
			case 14:
				line = 47;
				_tokenSodurFish?.Cancel();
				_tokenSodurFish = new CancellationTokenSource();
				token = _tokenSodurFish.Token;
				await SodorFishOnSelectedPersonAsync(token);
				break;
			case 9:
			{
				line = 48;
				FrmGharardadList frmGharardad = new FrmGharardadList(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _personIDs, DateM1, DateM2);
				line = 49;
				await ((CacheBaseForm)frmGharardad).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmGharardad).Dispose();
				break;
			}
			case 21:
			{
				line = 481;
				List<GharardadNew> list = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, _personIDs, DateM1, DateM2, token);
				if (list == null || !list.Any())
				{
					ReturnedSaveFuncInfo val2 = new ReturnedSaveFuncInfo();
					val2.AddInformation("[در تاریخ مشخص شده، قراردادی وجود ندارد]", "", "BtnDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 568);
					await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val2, "[دریافت قراردادها]", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs")).ShowAsync((IWin32Window)(object)this);
					break;
				}
				Morakhasi[] source = await Task.WhenAll(list.Select((GharardadNew gharardad) => Morakhasi.GetAsync(((CacheBaseForm)this).ContextGuid, gharardad.ID, token)));
				line = 491;
				List<object> lst = source.Cast<object>().ToList();
				FrmPrint frmMorakhasi = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)17, lst, null, 0, "");
				await ((CacheBaseForm)frmMorakhasi).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmMorakhasi).Dispose();
				break;
			}
			case 20:
			{
				line = 50;
				FrmGharardadList frmCheckoutPersonList = new FrmGharardadList(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _personIDs, DateM1, DateM2, _checkout);
				line = 51;
				await ((CacheBaseForm)frmCheckoutPersonList).ShowAsync((IWin32Window)(object)this);
				line = 52;
				((Component)(object)frmCheckoutPersonList).Dispose();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			case 10:
			{
				line = 53;
				FrmLogUserList frm2 = new FrmLogUserList(((CacheBaseForm)this).ContextGuid, TimeAttendanceSettings.appFont, _lstUsersId, DateM1, DateM2, Desc);
				line = 54;
				await ((CacheBaseForm)frm2).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm2).Dispose();
				break;
			}
			case 15:
				line = 55;
				if (PersonId <= 0)
				{
					line = 56;
					((Control)ucTakPerson1).Focus();
					Utility.MessageBoxShowError("[وارد کردن کد صحیح الزامی است]", ((CacheBaseForm)this).ContextGuid);
				}
				else
				{
					line = 56;
					RptTaradodSportClub rptTaradodSportClub = new RptTaradodSportClub(((CacheBaseForm)this).ContextGuid, DateM1, DateM2, PersonId, TimeAttendanceSettings.appFont);
					line = 57;
					await ((CacheBaseForm)rptTaradodSportClub).ShowAsync((IWin32Window)(object)this);
				}
				break;
			case 16:
			{
				line = 58;
				FrmInsuranceList frmInsuranceList = new FrmInsuranceList(((CacheBaseForm)this).ContextGuid, _personIDs, DateM1, DateM2, TimeAttendanceSettings.appFont);
				line = 59;
				((Form)frmInsuranceList).ShowDialog();
				((Component)(object)frmInsuranceList).Dispose();
				break;
			}
			case 17:
			{
				line = 60;
				FrmEventNotifyList frmNotify = new FrmEventNotifyList(((CacheBaseForm)this).ContextGuid, DateM1, DateM2, TimeAttendanceSettings.appFont);
				line = 61;
				await ((CacheBaseForm)frmNotify).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmNotify).Dispose();
				break;
			}
			case 18:
			{
				line = 62;
				RptVacations rptVacations = new RptVacations(((CacheBaseForm)this).ContextGuid);
				line = 63;
				await rptVacations.ReportAsync((IWin32Window)(object)this, DateM1, DateM2, ucTypeVacation1.ListID);
				break;
			}
			case 19:
			{
				line = 66;
				ReturnedSaveFuncInfo val = await GharardadNew.CopyGharardadForPersonAsync(((CacheBaseForm)this).ContextGuid, _personIDs, _ucTakGharardad1.ClsGharardad);
				if (val.HasError)
				{
					line = 67;
					CheckValidation.Validtor(((CacheBaseForm)this).ContextGuid, val, isShowMessage: true);
					break;
				}
				line = 68;
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[عملیات کپی با موقفیت انجام شد]", "", 3);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				break;
			}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, $"Error in Line {line}", "BtnDesign_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 661);
		}
	}

	private async Task SodorFishOnSelectedPersonAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (_personIDs == null)
			{
				return;
			}
			foreach (int personID in _personIDs)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				Fish pFish = await Fish.FactoryAsync(((CacheBaseForm)this).ContextGuid, personID, DateM1, DateM2, token);
				FrmFishHoghogh frmFish = new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, pFish, TimeAttendanceSettings.appFont, isReadOnly: false);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				if ((int)(await ((CacheBaseForm)frmFish).ShowAsync((IWin32Window)(object)this)) != 3)
				{
					((Component)(object)frmFish).Dispose();
					continue;
				}
				break;
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SodorFishOnSelectedPersonAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 683);
			throw;
		}
	}

	private async Task SodorFishAsync()
	{
		_ = 1;
		try
		{
			if (PersonId <= 0)
			{
				((Control)ucTakPerson1).Focus();
				Utility.MessageBoxShowError("[وارد کردن کد صحیح الزامی است]", ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				Fish pFish = await Fish.FactoryAsync(((CacheBaseForm)this).ContextGuid, PersonId, DateM1, DateM2);
				await ((CacheBaseForm)new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, pFish, TimeAttendanceSettings.appFont, isReadOnly: false)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SodorFishAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 706);
			throw;
		}
	}

	private async void FRMFilter_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				if (((Control)ucDate1).Visible)
				{
					ucDate1.GotoMonth(e);
				}
				if (((Control)ucTakDate1).Enabled && ((Control)ucTakDate1).Visible)
				{
					ucTakDate1.GotoMonth(e);
				}
				if (((Control)ucTakDay1).Enabled && ((Control)ucTakDay1).Visible)
				{
					ucTakDay1.GotoMonth(e);
				}
			}
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					return;
				}
				if ((int)_thisReportType == 8)
				{
					if ((int)(await ((CacheBaseForm)new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, isSelect: true, TimeAttendanceSettings.appFont)).ShowAsync((IWin32Window)(object)this)) == 2)
					{
						((Form)this).Close();
					}
				}
				else
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				((PopupItemControl)BtnDesign).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMFilter_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmFilter.cs", 743);
		}
	}

	private void UC_Resize(object sender, EventArgs e)
	{
		ReDesign();
	}

	private void FrmFilter_Paint(object sender, PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			((Control)this).Paint -= new PaintEventHandler(FrmFilter_Paint);
			ucTakPerson1.SetFocus();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad8: Expected O, but got Unknown
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmFilter));
		panelEx1 = new PanelEx();
		BtnDesign = new ButtonX();
		ucTakPerson1 = new UCTakPerson();
		ucTypeVacation1 = new UCTypeVacation();
		ucUserList1 = new UCUserList();
		ucDesc1 = new UCDesc();
		ucTakDate1 = new UCTakDate();
		ucTakDay1 = new UCTakDay();
		ucPerson1 = new UCPerson();
		ucDate1 = new UCDate();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnDesign);
		((Control)panelEx1).Controls.Add((Control)(object)ucTakPerson1);
		((Control)panelEx1).Controls.Add((Control)(object)ucTypeVacation1);
		((Control)panelEx1).Controls.Add((Control)(object)ucUserList1);
		((Control)panelEx1).Controls.Add((Control)(object)ucDesc1);
		((Control)panelEx1).Controls.Add((Control)(object)ucTakDate1);
		((Control)panelEx1).Controls.Add((Control)(object)ucTakDay1);
		((Control)panelEx1).Controls.Add((Control)(object)ucPerson1);
		((Control)panelEx1).Controls.Add((Control)(object)ucDate1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, -1);
		((Control)panelEx1).MinimumSize = new Size(630, 10);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(630, 620);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnDesign).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDesign).Anchor = (AnchorStyles)14;
		BtnDesign.ColorTable = (eButtonColor)6;
		BtnDesign.Image = (Image)(object)Resources.application_vnd_sun_xml_calc_template;
		BtnDesign.ImagePosition = (eImagePosition)1;
		((Control)BtnDesign).Location = new Point(15, 552);
		((Control)BtnDesign).Margin = new Padding(0);
		((Control)BtnDesign).Name = "BtnDesign";
		((Control)BtnDesign).Size = new Size(601, 59);
		((PopupItemControl)BtnDesign).Style = (eDotNetBarStyle)9;
		((Control)BtnDesign).TabIndex = 6;
		((Control)BtnDesign).Text = "[تهیه گزارش]";
		((Control)BtnDesign).Click += BtnDesign_Click;
		((Control)ucTakPerson1).Anchor = (AnchorStyles)13;
		((UserControl)ucTakPerson1).AutoSizeMode = (AutoSizeMode)0;
		ucTakPerson1.EnalbleControl = true;
		((Control)ucTakPerson1).Location = new Point(15, 50);
		((Control)ucTakPerson1).Margin = new Padding(0);
		((Control)ucTakPerson1).MaximumSize = new Size(600, 70);
		((Control)ucTakPerson1).MinimumSize = new Size(600, 10);
		((Control)ucTakPerson1).Name = "ucTakPerson1";
		((Control)ucTakPerson1).Size = new Size(600, 64);
		((Control)ucTakPerson1).TabIndex = 1;
		((Control)ucTakPerson1).Resize += UC_Resize;
		ucTypeVacation1.EnalbleControl = true;
		((Control)ucTypeVacation1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucTypeVacation1).Location = new Point(15, 168);
		((Control)ucTypeVacation1).Margin = new Padding(0);
		((Control)ucTypeVacation1).MaximumSize = new Size(600, 400);
		((Control)ucTypeVacation1).MinimumSize = new Size(600, 10);
		((Control)ucTypeVacation1).Name = "ucTypeVacation1";
		((Control)ucTypeVacation1).Size = new Size(600, 373);
		((Control)ucTypeVacation1).TabIndex = 5;
		((Control)ucTypeVacation1).Resize += UC_Resize;
		ucUserList1.EnalbleControl = true;
		((Control)ucUserList1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucUserList1).Location = new Point(15, 138);
		((Control)ucUserList1).Margin = new Padding(0);
		((Control)ucUserList1).MaximumSize = new Size(600, 350);
		((Control)ucUserList1).MinimumSize = new Size(600, 10);
		((Control)ucUserList1).Name = "ucUserList1";
		((Control)ucUserList1).Size = new Size(600, 289);
		((Control)ucUserList1).TabIndex = 4;
		((Control)ucUserList1).Resize += UC_Resize;
		ucDesc1.EnalbleControl = true;
		((Control)ucDesc1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucDesc1).Location = new Point(15, 111);
		((Control)ucDesc1).Margin = new Padding(0);
		((Control)ucDesc1).MaximumSize = new Size(600, 70);
		((Control)ucDesc1).MinimumSize = new Size(600, 10);
		((Control)ucDesc1).Name = "ucDesc1";
		((Control)ucDesc1).Size = new Size(600, 68);
		((Control)ucDesc1).TabIndex = 1;
		((Control)ucDesc1).Resize += UC_Resize;
		ucTakDate1.DateM1 = new DateTime(2024, 7, 30, 11, 23, 45, 812);
		ucTakDate1.DateM2 = new DateTime(2021, 2, 18, 23, 59, 59, 0);
		ucTakDate1.EnalbleControl = true;
		((Control)ucTakDate1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucTakDate1.IsEnabledMounts = true;
		((Control)ucTakDate1).Location = new Point(15, 81);
		((Control)ucTakDate1).Margin = new Padding(0);
		((Control)ucTakDate1).MaximumSize = new Size(600, 60);
		((Control)ucTakDate1).MinimumSize = new Size(600, 10);
		((Control)ucTakDate1).Name = "ucTakDate1";
		((Control)ucTakDate1).Size = new Size(600, 60);
		((Control)ucTakDate1).TabIndex = 1;
		((Control)ucTakDate1).Resize += UC_Resize;
		ucTakDay1.EnalbleControl = true;
		((Control)ucTakDay1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucTakDay1).Location = new Point(15, 138);
		((Control)ucTakDay1).Margin = new Padding(50);
		((Control)ucTakDay1).MaximumSize = new Size(600, 70);
		((Control)ucTakDay1).MinimumSize = new Size(600, 10);
		((Control)ucTakDay1).Name = "ucTakDay1";
		((Control)ucTakDay1).Size = new Size(600, 68);
		((Control)ucTakDay1).TabIndex = 1;
		((Control)ucTakDay1).Resize += UC_Resize;
		ucPerson1.EnalbleControl = true;
		((Control)ucPerson1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucPerson1).Location = new Point(15, 125);
		((Control)ucPerson1).Margin = new Padding(0);
		((Control)ucPerson1).MaximumSize = new Size(600, 300);
		((Control)ucPerson1).MinimumSize = new Size(600, 10);
		((Control)ucPerson1).Name = "ucPerson1";
		((Control)ucPerson1).RightToLeft = (RightToLeft)1;
		((Control)ucPerson1).Size = new Size(600, 300);
		((Control)ucPerson1).TabIndex = 1;
		((Control)ucPerson1).Resize += UC_Resize;
		ucDate1.EnalbleControl = true;
		((Control)ucDate1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucDate1).Location = new Point(15, 19);
		((Control)ucDate1).Margin = new Padding(0);
		((Control)ucDate1).MaximumSize = new Size(600, 200);
		((Control)ucDate1).MinimumSize = new Size(600, 10);
		((Control)ucDate1).Name = "ucDate1";
		((Control)ucDate1).RightToLeft = (RightToLeft)1;
		((Control)ucDate1).Size = new Size(600, 187);
		((Control)ucDate1).TabIndex = 0;
		((Control)ucDate1).Resize += UC_Resize;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)3;
		((Form)this).ClientSize = new Size(624, 619);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(640, 1000);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(640, 10);
		((Control)this).Name = "FrmFilter";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "BaseForm*";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmFilter_FormClosed);
		((Control)this).Paint += new PaintEventHandler(FrmFilter_Paint);
		((Control)this).KeyDown += new KeyEventHandler(FRMFilter_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
