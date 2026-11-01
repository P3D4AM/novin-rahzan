using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxRealSvrOcxTcpLib;
using BackUpDLL.Assistance;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Assistance;
using FormCustom.BackUp;
using FormCustom.BackUp.Forms;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Froms.LkManage;
using FormCustom.SignalRConnection.Uc;
using FormCustom.Waiting;
using LK;
using NPResourceManagement;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Assistance.Net;
using PacketParser.Assistance.Net.Hysoon;
using PacketParser.Attendance;
using PacketParser.Attendance.Access;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.LkService;
using PacketParser.Scale.EventArguments;
using Serilog;
using TimeAttendance;
using TimeAttendanceDAL;
using TimeAttendanceDAL.Connection.Class;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Enroll;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Reports;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Database;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.Utility.Settings;
using TimeAttendanceDevices.FLREbio;
using TimeAttendanceDevices.FLRHysoon.DynamicHysoonRealTime;
using TimeAttendanceUI.AttPersonLeave;
using TimeAttendanceUI.Departments;
using TimeAttendanceUI.EbioRealTime;
using TimeAttendanceUI.Forms.Bimeh;
using TimeAttendanceUI.Forms.Club;
using TimeAttendanceUI.Forms.Device;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Other;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Reports;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Forms.User;
using TimeAttendanceUI.HysonRealTime;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.ReportProducer;
using TimeAttendanceUI.Salary.Forms;
using TimeAttendanceUI.Salary.Forms.Codeing;
using TimeAttendanceUI.Salary.Shift;
using TimeAttendanceUI.Sms;
using TimeAttendanceUI.SportClub;
using TimeAttendanceUI.UC.Base;
using TimeAttendanceUI.UC.payamak;

namespace TimeAttendanceUI;

public class FrmMain : LkUiFactory
{
	private struct Lastinputinfo
	{
		public uint cbSize;

		public uint dwTime;
	}

	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static _DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler _003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler _003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler _003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler _003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText;

		public static _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler _003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
	}

	private static readonly SemaphoreSlim _semaphoreReceiveData = new SemaphoreSlim(1, 1);

	private Font _font;

	private TimeAttendanceSettings _setting;

	private bool _isDisposed;

	private bool _questionBackupAsked;

	private List<ButtonItem> _listReports;

	private DateTime? _stopNotify;

	private readonly bool _notshowUi;

	private bool _frmUserIsShow;

	private SemaphoreSlim _semaphoreRestoreOrBazsazi = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _tokenSource;

	private CancellationTokenSource _sendPersonToApiTokenSource;

	private CancellationTokenSource _checkPersonChangesToken;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _tokenReceiveTaradod;

	private CancellationTokenSource _tokenPersonLeave;

	private CancellationTokenSource _tokenOpenPort;

	private CancellationTokenSource _tokenGetLastTaradods;

	private CancellationTokenSource _tokenReportDevices;

	private CancellationTokenSource _tokenBazsazi;

	private CancellationTokenSource _tokenNotifyMe;

	private CancellationTokenSource _tokenHoliday;

	private CancellationTokenSource _tokenRestore;

	private SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private bool changedLanguage;

	private SemaphoreSlim _changeHysonPortServerSemaphore = new SemaphoreSlim(1, 1);

	private List<ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>>> axRealSvrOcxTcpItems;

	private static readonly EventLog systemLog = new EventLog("TimeAttendance");

	private static readonly object _lock = new object();

	private static ConcurrentDictionary<int, HysoonServer> _dicHysoonServers = new ConcurrentDictionary<int, HysoonServer>();

	private static ConcurrentDictionary<int, HttpServerDynamicHysoon> _dicDynamicHysoon = new ConcurrentDictionary<int, HttpServerDynamicHysoon>();

	private bool c_frmSettingIsShowing;

	private TcpClient client;

	private static string LastEventLogMessage = "";

	private IContainer components;

	private UcStatuse ucStatuse1;

	private RibbonControl ribbonControl1;

	private RibbonPanel ribbonPanel1;

	private RibbonBar ribbonBar1;

	private ButtonItem BTNUsers;

	private ButtonItem BtnDevice;

	private ButtonItem BtnPersonel;

	private RibbonPanel ribbonPanel4;

	private RibbonBar ribbonBar4;

	private ButtonItem BtnBackup;

	private ButtonItem BtnRestore;

	private ButtonItem BtnReadData;

	private ButtonItem BtnSetting;

	private RibbonPanel ribbonPanel3;

	private RibbonBar ribbonBar3;

	private ButtonItem BtnLogOnManage;

	private ButtonItem BtnLogOnPersonel;

	private RibbonTabItem ribbonTabItem1;

	private RibbonTabItem ribbonTabItem2;

	private RibbonTabItem ribbonTabItem5;

	private ButtonItem BtnCalc;

	private ButtonItem BtnReadDataManage;

	private ButtonItem BtnPass;

	private UCMain ucMain1;

	private RibbonPanel ribbonPanel2;

	private RibbonTabItem ribbonTabItem3;

	private RibbonBar ribbonBar2;

	private ButtonItem BtnSFishM;

	private ButtonItem BtnSFishD;

	private ButtonItem BtnFishes;

	private ButtonItem BtnSHesab;

	private RibbonPanel ribbonPanel5;

	private RibbonBar ribbonBar5;

	private ButtonItem BtnAboutUs;

	private ButtonItem BtnCallUs;

	private RibbonTabItem ribbonTabItem6;

	private ButtonItem BtnShift;

	private ButtonItem BtnHoliday;

	private ButtonItem BtnListBimeh;

	private ButtonItem BtnMorakhasi;

	private ButtonItem BtnTypeTaradod;

	private ButtonItem btnactivation;

	private ButtonItem BtnRptPersonsOneMounth;

	private ButtonItem Btnsoftware;

	private ButtonItem BtnListM;

	private ButtonItem BTNManagment;

	private RibbonPanel ribbonPanel6;

	private RibbonBar ribbonBar6;

	private ButtonItem BtnRPTsaz;

	private RibbonTabItem ribbonTabItem4;

	private ButtonItem BtnCompany;

	private ButtonItem BtnGharardad;

	private ButtonItem BtnDasteMaliyat;

	private ButtonItem BtnRptTaradodFull;

	private ButtonItem BtnBazsazi;

	private ButtonItem BtnDefMorakhsi;

	private ButtonItem BtnRptPersonsOneDay;

	private ButtonItem BtnMorakhasiSaati;

	private ButtonItem BtnRptMorakhasi;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem cmsBazsaziToolStripMenuItem1;

	private ToolStripMenuItem cmsBackupToolStripMenuItem1;

	private ToolStripMenuItem cmsRestoreToolStripMenuItem1;

	private ButtonItem BtnProgrammableShift;

	private ContextMenuStrip NotifyContextMenue;

	private ToolStripMenuItem CMSOpenProgram;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem CMSAboutUS;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem CMSExitProgram;

	public NotifyIcon notifyIcon1;

	private ButtonItem buttonItem1;

	private ButtonItem BtnBaseYearsSalary;

	private ButtonItem BtnRptAllTaradod;

	private ButtonItem BtnTutorialVideo;

	private ButtonItem BtnLogUser;

	private Timer timerStopNotification;

	private Timer timerSportClub;

	private RibbonPanel ribbonPanel7;

	private RibbonBar ribbonBar7;

	private ButtonItem BtnDefinePackage;

	private RibbonTabItem ribbonTabItemSportClub;

	private ButtonItem buttonItem3;

	private ButtonItem buttonItem2;

	private ButtonItem BtnAttendanceSport;

	private ButtonX btnNotifyMe;

	private Timer timerReadNotify;

	private ButtonItem BtnAgreedDeduction;

	private ButtonItem BtnSuggestion;

	private ButtonItem btnRptNotify;

	private ButtonItem btnVacations;

	private ButtonItem buttonItem4;

	private Timer timer1;

	private ButtonItem BtnEndedGharardad;

	private Timer timerLock;

	private ButtonItem BtnTajmiVacations;

	private LinkLabel LanguageLable;

	private MonthCalendarX monthCalendarX1;

	private TextBox TbxMonasebat;

	private UcCacheStatus ucCacheStatus1;

	private PictureBox ImgBazsazi;

	private PictureBox ImgRecovery;

	private DataGridView DgridReportDevices;

	private BindingSource deviceEventsEntityBindingSource;

	private DataGridViewX DgridLastTaradods;

	private BindingSource logOnPersonelTopLastTaradodsBindingSource;

	private ToolStripMenuItem cmsShowLastTaradod;

	private ToolStripMenuItem cmsShowReportDevice;

	private ToolStripMenuItem cmsViewCalendar;

	private ToolStripSeparator toolStripSeparator3;

	private DataGridViewTextBoxColumn terminalIDDataGridViewTextBoxColumn;

	private UcLkExpireDisplay ucLkExpireDisplay1;

	private UcNotificationViewer ucNotificationViewer1;

	private ButtonItem BtnClub;

	private ButtonItem BtnReloadEnrollData;

	private ButtonItem BtnRequestVacation;

	private Timer timerStartPayamak;

	private Timer timerRefreshCalendar;

	private RibbonPanel ribbonPanel8;

	private RibbonTabItem ribbonTabItemSMS;

	private RibbonBar ribbonBar8;

	private ButtonItem BtnSmsCalendar;

	private ButtonItem BtnSendedSms;

	private ButtonItem BtnNotSend;

	private UcPayamakServiceStatus ucPayamakServiceStatus1;

	private DataGridViewTextBoxColumn termTypeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn serialNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn LogType;

	private DataGridViewTextBoxColumn transactionIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn logTimeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn userIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn ModifiedDate;

	private ButtonItem btnDepartment;

	private ButtonItem btnTaxTable;

	private LinkLabel linkLabelGetOtherVeraion;

	private ButtonX BtnShowEventLog;

	private ButtonX BtnPayamak;

	private ButtonX BtnSignalR;

	private DataGridViewTextBoxColumn DgDatetimeLogM;

	private DataGridViewTextBoxColumn DgDeviceName;

	private DataGridViewTextBoxColumn DgPersonNo;

	private DataGridViewTextBoxColumn DgPersonFullName;

	private DataGridViewTextBoxColumn DgDateCalture;

	private DataGridViewTextBoxColumn DgGharardadEndDate;

	private DataGridViewTextBoxColumn DgGharardadStatusName;

	private DataGridViewTextBoxColumn DgTrafficTypeName;

	private DataGridViewTextBoxColumn DgPersonId;

	private PictureBox pictureBoxLoading;

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

	public IContainer Components
	{
		get
		{
			return components;
		}
		set
		{
			components = value;
		}
	}

	[DllImport("User32.dll")]
	private static extern bool GetLastInputInfo(ref Lastinputinfo dummy);

	private static uint GetIdleTime()
	{
		Lastinputinfo dummy = default(Lastinputinfo);
		try
		{
			dummy.cbSize = (uint)Marshal.SizeOf(dummy);
			GetLastInputInfo(ref dummy);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetIdleTime", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 134);
		}
		return (uint)Environment.TickCount - dummy.dwTime;
	}

	private void TimerLock_Tick(object sender, EventArgs e)
	{
		try
		{
			if (UserAttendance.SelectedUser.UserSetting.ShowFrmEnter && UserAttendance.SelectedUser.UserSetting.LockTime >= 10 && GetIdleTime() > UserAttendance.SelectedUser.UserSetting.LockTime * 1000 && !_frmUserIsShow)
			{
				ShowFrmLogin();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void ShowFrmLogin()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		try
		{
			timerLock.Stop();
			FrmEnter frmEnter = new FrmEnter(((CacheBaseForm)this).ContextGuid, autoLogin: false, canChangeUser: false);
			_frmUserIsShow = true;
			if ((int)((Form)frmEnter).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((Form)frmEnter).Close();
				_frmUserIsShow = false;
				return;
			}
			_frmUserIsShow = false;
			Application.Exit();
			if (((Control)this).Visible)
			{
				ShowFrmLogin();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			if (!((Control)this).Visible)
			{
				timerLock.Stop();
			}
			else
			{
				timerLock.Start();
			}
		}
	}

	public FrmMain(string contextGuid, bool showUi)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_notshowUi = showUi;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<ReturnedSaveFuncInfo> DestroyAllAxRealSvrOcxTcpComponentsAsync(CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await _changeHysonPortServerSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if (!(axRealSvrOcxTcpItems?.Any() ?? false))
			{
				return ret;
			}
			token.ThrowIfCancellationRequested();
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(1000, token);
			}
			if (IsDisposed)
			{
				return ret;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00af: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Expected O, but got Unknown
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Expected O, but got Unknown
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0110: Expected O, but got Unknown
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Expected O, but got Unknown
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0160: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Expected O, but got Unknown
				try
				{
					List<ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>>> list = axRealSvrOcxTcpItems;
					if (list == null || !list.Any())
					{
						return;
					}
					foreach (ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>> axRealSvrOcxTcpItem in axRealSvrOcxTcpItems)
					{
						if (axRealSvrOcxTcpItem?.value != null)
						{
							if (axRealSvrOcxTcpItem.value == null)
							{
								throw new NullReferenceException("item is null!!");
							}
							if (axRealSvrOcxTcpItem.value.Item2 == null)
							{
								throw new NullReferenceException("item.item2 is null!!");
							}
							axRealSvrOcxTcpItem.value.Item2.CloseNetwork((int)axRealSvrOcxTcpItem.value.Item1);
							AxRealSvrOcxTcp item = axRealSvrOcxTcpItem.value.Item2;
							object obj = _003C_003EO._003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData;
							if (obj == null)
							{
								_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler val = HysoonRealTimeProcess.AxRealSvrOcxTcp1_OnReceiveGLogData;
								_003C_003EO._003C0_003E__AxRealSvrOcxTcp1_OnReceiveGLogData = val;
								obj = (object)val;
							}
							item.OnReceiveGLogData -= (_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler)obj;
							AxRealSvrOcxTcp item2 = axRealSvrOcxTcpItem.value.Item2;
							object obj2 = _003C_003EO._003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;
							if (obj2 == null)
							{
								_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler val2 = HysoonRealTimeProcess.AxRealSvrOcxTcp1_OnReceiveGLogDataExtend;
								_003C_003EO._003C1_003E__AxRealSvrOcxTcp1_OnReceiveGLogDataExtend = val2;
								obj2 = (object)val2;
							}
							item2.OnReceiveGLogDataExtend -= (_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler)obj2;
							AxRealSvrOcxTcp item3 = axRealSvrOcxTcpItem.value.Item2;
							object obj3 = _003C_003EO._003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;
							if (obj3 == null)
							{
								_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler val3 = HysoonRealTimeProcess.AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage;
								_003C_003EO._003C2_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage = val3;
								obj3 = (object)val3;
							}
							item3.OnReceiveGLogTextAndImage -= (_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler)obj3;
							AxRealSvrOcxTcp item4 = axRealSvrOcxTcpItem.value.Item2;
							object obj4 = _003C_003EO._003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText;
							if (obj4 == null)
							{
								_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler val4 = HysoonRealTimeProcess.AxRealSvrOcxTcp1_OnReceiveGLogText;
								_003C_003EO._003C3_003E__AxRealSvrOcxTcp1_OnReceiveGLogText = val4;
								obj4 = (object)val4;
							}
							item4.OnReceiveGLogText -= (_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler)obj4;
							AxRealSvrOcxTcp item5 = axRealSvrOcxTcpItem.value.Item2;
							object obj5 = _003C_003EO._003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
							if (obj5 == null)
							{
								_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler val5 = HysoonRealTimeProcess.AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen;
								_003C_003EO._003C4_003E__AxRealSvrOcxTcp1_OnReceiveGLogTextOnDoorOpen = val5;
								obj5 = (object)val5;
							}
							item5.OnReceiveGLogTextOnDoorOpen -= (_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler)obj5;
							((Control)this).Controls.Remove((Control)(object)axRealSvrOcxTcpItem.value.Item2);
							((Component)(object)axRealSvrOcxTcpItem.value.Item2)?.Dispose();
						}
					}
				}
				catch (Exception ex3)
				{
					ret.AddReturnedValue(ex3, "", "DestroyAllAxRealSvrOcxTcpComponentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 233);
					if (ex3.Message.Contains("The specified module could not be found"))
					{
						MessageBoxFarsi.Show("فایل زیر در محل نصب برنامه وجود ندارد، لذا برنامه با خطا مواجه خواهد شد \r\n RealSvrOcxTcp.ocx", "خطای خیلی مهم", (MessageBoxFarsiButtons)0, (MessageBoxFarsiIcon)6, (MessageBoxFarsiDefaultButton)0, true, true);
					}
					WebErrorLog.ErrorLogInstance.StartLog("فایل زیر در محل نصب برنامه وجود ندارد. \r\n RealSvrOcxTcp.ocx", "", "DestroyAllAxRealSvrOcxTcpComponentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 236);
				}
			});
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			ret.AddReturnedValue(ex2, "", "DestroyAllAxRealSvrOcxTcpComponentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 244);
		}
		finally
		{
			if (semaphoreHasBeenTaken && !IsDisposed)
			{
				axRealSvrOcxTcpItems = new List<ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>>>();
				_changeHysonPortServerSemaphore.Release();
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<AxRealSvrOcxTcp>> CreateControlAxRealSvrOcxTcpAsync(short port, CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		ReturnedSaveFuncInfoWithValue<AxRealSvrOcxTcp> ret = new ReturnedSaveFuncInfoWithValue<AxRealSvrOcxTcp>();
		try
		{
			await _changeHysonPortServerSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(1000, token);
			}
			if (IsDisposed)
			{
				return null;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Expected O, but got Unknown
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					ret.value = new AxRealSvrOcxTcp();
					((ISupportInitialize)ret.value).BeginInit();
					ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmMain));
					((AxHost)ret.value).Enabled = true;
					((Control)ret.value).Location = new Point(9, 3);
					((Control)ret.value).Name = "axRealSvrOcxTcp1";
					((AxHost)ret.value).OcxState = (State)componentResourceManager.GetObject("axRealSvrOcxTcp1.OcxState");
					((Control)ret.value).Size = new Size(32, 32);
					((Control)ret.value).TabIndex = 0;
					((Control)ret.value).Visible = false;
					HysoonRealTimeProcess.Start(((CacheBaseForm)this).ContextGuid, ret.value);
					((Control)this).Controls.Add((Control)(object)ret.value);
					((ISupportInitialize)ret.value).EndInit();
					int num = ret.value.OpenNetwork((int)port);
					if (num != 1)
					{
						ret.AddError($"خطا در باز کردن پورت سرور({port})\r\n" + $"کد خطا :{num}", "CreateControlAxRealSvrOcxTcpAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 295);
					}
				}
				catch (Exception ex2)
				{
					ret.AddReturnedValue(ex2, "", "CreateControlAxRealSvrOcxTcpAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 300);
					if (ex2.Message.Contains("The specified module could not be found"))
					{
						string text = "فایل زیر در محل نصب برنامه وجود ندارد، لذا برنامه با خطا مواجه خواهد شد \r\n RealSvrOcxTcp.ocx";
						ret.AddError(text, "CreateControlAxRealSvrOcxTcpAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 304);
						MessageBoxFarsi.Show(text, "خطای خیلی مهم", (MessageBoxFarsiButtons)0, (MessageBoxFarsiIcon)6, (MessageBoxFarsiDefaultButton)0, true, true);
					}
					else
					{
						Log.Error(ex2, "");
					}
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateControlAxRealSvrOcxTcpAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 312);
		}
		finally
		{
			if (semaphoreHasBeenTaken && !IsDisposed)
			{
				ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>> val = new ReturnedSaveFuncInfoWithValue<Tuple<short, AxRealSvrOcxTcp>>
				{
					value = new Tuple<short, AxRealSvrOcxTcp>(port, ret.value)
				};
				val.AddReturnedValue((ReturnedSaveFuncInfo)(object)ret, (ReturnedState)0, "CreateControlAxRealSvrOcxTcpAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 321);
				axRealSvrOcxTcpItems.Add(val);
				_changeHysonPortServerSemaphore.Release();
			}
		}
		return ret;
	}

	private async Task LoadHolidayForCalendarAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (cmsViewCalendar.Checked)
			{
				((Control)monthCalendarX1).Visible = true;
				monthCalendarX1.SetSelectedDate(DateTime.Now);
				monthCalendarX1.SetTodayDate(DateTime.Now);
				List<Holiday> source = await Holiday.GetAllAsync(((CacheBaseForm)this).ContextGuid, DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1), isHolidayOnly: true, token);
				monthCalendarX1.PersianHolidayDates = ((IEnumerable<Holiday>)source).Select((Func<Holiday, PersianDateTime>)((Holiday holiday) => new PersianDateTime(int.Parse(holiday.DateCalture.Substring(0, 4)), int.Parse(holiday.DateCalture.Substring(5, 2)), int.Parse(holiday.DateCalture.Substring(8, 2))))).ToArray();
				DateTime selectedDateInDateTime = monthCalendarX1.GetSelectedDateInDateTime();
				Holiday holiday2 = await Holiday.GetAsync(((CacheBaseForm)this).ContextGuid, selectedDateInDateTime);
				((Control)TbxMonasebat).Text = holiday2.Monasebat;
				((Control)TbxMonasebat).Visible = ((Control)TbxMonasebat).Text.Trim().Length > 0;
			}
			else
			{
				MonthCalendarX obj = monthCalendarX1;
				bool visible = (((Control)TbxMonasebat).Visible = false);
				((Control)obj).Visible = visible;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadHolidayForCalendarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 350);
		}
	}

	private async Task InitCmsItems()
	{
		try
		{
			string cmsOpenProgram = Utilities.GetTranslate("[باز کردن برنامه]");
			string cmsAboutUS = Utilities.GetTranslate("[درباره ما]");
			string cmsExitProgram = Utilities.GetTranslate("[خروج کامل از برنامه]");
			string notifyIcon1_BalloonTipText = Utilities.GetTranslate("[این یک تست است]");
			string notifyIcon1_BalloonTipTitle = Utilities.GetTranslate("[نرم افزار حضور و غیاب1]");
			string notifyIcon1_Text = Utilities.GetTranslate("[حضور غیاب نوین پرداز]");
			Level_Hozor levelHozor = AccessLevelClass.Instance.LevelHozor;
			string text = "";
			if (LkOperations._isFreeVersion)
			{
				text = "** نسخه رایگان ** /این نسخه فاقد پشتیبانی می باشد /";
				Color backColor = Color.FromArgb(50, 255, 255, 132);
				ribbonControl1.BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar1).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar2).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar3).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar4).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar5).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar6).BackgroundStyle.BackColor = backColor;
				((ItemControl)ribbonBar7).BackgroundStyle.BackColor = backColor;
				((Control)linkLabelGetOtherVeraion).Visible = true;
			}
			if (LkOperations._isTrialVersion)
			{
				text = "** نسخه آزمایشی **";
				Color backColor2 = Color.FromArgb(50, 255, 0, 0);
				ribbonControl1.BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar1).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar2).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar3).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar4).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar5).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar6).BackgroundStyle.BackColor = backColor2;
				((ItemControl)ribbonBar7).BackgroundStyle.BackColor = backColor2;
			}
			else
			{
				text = $"نـــــرم افــزار جــــــامع حضـــــور غـیـاب و حـــقـوق دسـتــــمـزد نوین پرداز  (سطح نرم افزار : {levelHozor} )        نسخه:{VersionManagment.AssemblyVersion3Num()}";
			}
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					((ToolStripItem)CMSOpenProgram).Text = cmsOpenProgram;
					((ToolStripItem)CMSAboutUS).Text = cmsAboutUS;
					((ToolStripItem)CMSExitProgram).Text = cmsExitProgram;
					notifyIcon1.BalloonTipText = notifyIcon1_BalloonTipText;
					notifyIcon1.BalloonTipTitle = notifyIcon1_BalloonTipTitle;
					notifyIcon1.Text = notifyIcon1_Text;
					((Control)this).Text = text;
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "InitCmsItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 413);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitCmsItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 416);
		}
	}

	private async void FRMMain_Load(object sender, EventArgs e)
	{
		_ = 5;
		try
		{
			pictureBoxLoading.Image = (Image)(object)Resources.Time1;
			((Control)pictureBoxLoading).Visible = false;
			Utility.OwnerForm.TrySetResult((IWin32Window)(object)this);
			Utility.GridLastTaradod.TrySetResult(DgridLastTaradods);
			Utility.GridReportDevices.TrySetResult(DgridReportDevices);
			await EntityCacheConfig.InitAsync(((CacheBaseForm)this).ContextGuid, ClsConnection.connectionString, SystemInfoClass.SystemInfo(), VersionManagment.Version, (EnConnectionType)1);
			await new ConnectionManager(((CacheBaseForm)this).ContextGuid).HanldleConnectionsAsync();
			UiInvokerInstance.Initialize((CacheBaseForm)(object)this);
			LogOnPersonel.InstanceForEvents.OnStatusErorTaradod += ShowEventMessage;
			_setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			TimeAttendanceSettings.appFont = (_font = _setting.FontProgram);
			_checkPersonChangesToken?.Cancel();
			_checkPersonChangesToken = new CancellationTokenSource();
			CancellationToken tokenPersonChanges = _checkPersonChangesToken.Token;
			Task.Run(() => PersonChanges.CheckPersonChangesAsync(((CacheBaseForm)this).ContextGuid, tokenPersonChanges));
			_sendPersonToApiTokenSource?.Cancel();
			_sendPersonToApiTokenSource = new CancellationTokenSource();
			CancellationToken tokenSendPersonToApi = _sendPersonToApiTokenSource.Token;
			Task.Run(() => Person.SendWebPersonToServerAsync(((CacheBaseForm)this).ContextGuid, tokenSendPersonToApi));
			ClsCache cache = ClsCache.GetCache(((CacheBaseForm)this).ContextGuid);
			object obj;
			if (cache == null)
			{
				obj = null;
			}
			else
			{
				WebServiceHandlers webService = cache.WebService;
				obj = ((webService != null) ? webService.PersonLeave : null);
			}
			if (obj != null)
			{
				ClsCache.GetCache(((CacheBaseForm)this).ContextGuid).WebService.PersonLeave.OnLeaveAccepted += PersonLeave_OnLeaveAcceptedAsync;
			}
			await AddDynamicReportsBtnsAsync();
			await InitCmsItems();
			InitAccessLevel();
			Task.Run(() => ucLkExpireDisplay1.SetContextGuidAsync(((CacheBaseForm)this).ContextGuid, (ILockInformationManager)new LockInformationManager(((CacheBaseForm)this).ContextGuid), (Package)2));
			Task.Run(() => ucNotificationViewer1.SetContextGuidAsync(((CacheBaseForm)this).ContextGuid, (ENSource)5));
			_tokenOpenPort?.Cancel();
			_tokenOpenPort = new CancellationTokenSource();
			CancellationToken tokenOpenPort = _tokenOpenPort.Token;
			Task.Run(async delegate
			{
				await OpenPortsForDevicesAsync(tokenOpenPort);
			});
			cmsShowReportDevice.Checked = UserAttendance.SelectedUser.UserSetting.viewGridReportDevices;
			cmsShowLastTaradod.Checked = UserAttendance.SelectedUser.UserSetting.viewGridLastTaradods;
			cmsViewCalendar.Checked = UserAttendance.SelectedUser.UserSetting.ViewCalendar;
			cmsShowReportDevice.CheckedChanged += cmsShowReportDevice_CheckedChanged;
			cmsShowLastTaradod.CheckedChanged += cmsShowLastTaradod_CheckedChanged;
			((ToolStripItem)ucStatuse1.tssuser).Text = UserAttendance.SelectedUser.Name;
			((ToolStripItem)ucStatuse1.TSSDBName).Text = ClsConnection.GetDbName(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			notifyIcon1.Visible = true;
			((Control)LanguageLable).Text = Utilities.GetDisplay<LanguageTypes>(Utility.GetLanguage());
			ribbonTabItem2.Select();
			timerLock.Enabled = true;
			timerLock.Start();
			timerStopNotification.Tick += TimerStopNotification_Tick;
			timerStopNotification.Start();
			timerReadNotify.Tick += TimerReadNotify_Tick;
			_tokenNotifyMe?.Cancel();
			_tokenNotifyMe = new CancellationTokenSource();
			CancellationToken token = _tokenNotifyMe.Token;
			await CheckShowNotifyButtonAsync(token);
			if (_notshowUi)
			{
				((Control)this).Hide();
			}
			CheckUcCacheStatus();
			InitPayamakService(_setting);
			Task.Run((Action)CreateBackUp);
			Log.Information("نرم افزار حضور باز شد\r\n user: " + UserAttendance.SelectedUser.Name + "\r\n DB: " + ClsConnection.GetDbName(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid)));
			LogUserLogin(UserAttendance.SelectedUser.Name);
			Task.Run((Func<Task?>)ReceiveDataDevicePolling);
			CheckShowEndedGharardad();
			ReloadDgridLastTaradods();
			ReloadDgridReportDevices();
			Task.Run(() => ClsSms.StartSendSmsAsync(((CacheBaseForm)this).ContextGuid));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 508);
		}
	}

	private async Task<bool> IsTatilAsync(Morakhasidaily mor, Person person, List<DeviceInfo> devices, CancellationToken token)
	{
		_ = 2;
		try
		{
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			DateTime dateFrom = mor.DateFrom;
			StatuseTaradodEnumType val = await (await TaradodFullDet.GetAsync_(contextGuid, devices, dateFrom, person, await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, mor.PersonId, mor.DateFrom, token), checkValidationVorodAndKHoroj: false)).GetStatuseTaradodAsync(token);
			return (int)val == 7 || (int)val == 6;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	private async Task PersonLeave_OnLeaveAcceptedAsync(object sender, AttPersonLeaveBusiness e)
	{
		bool transactionHasBeenCreated = false;
		try
		{
			await _databaseSemaphore.WaitAsync();
			transactionHasBeenCreated = true;
			AttPersonLeaveBusiness obj = e;
			if (((obj == null) ? 3 : ((int)obj.LeaveStatus)) != 2)
			{
				return;
			}
			AttPersonLeaveBusiness obj2 = e;
			if (((obj2 != null) ? ((int)obj2.Type) : 0) != 1)
			{
				return;
			}
			_tokenPersonLeave?.Cancel();
			_tokenPersonLeave = new CancellationTokenSource();
			CancellationToken token = _tokenPersonLeave.Token;
			Person person = (await Person.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int>(), personStatus: true, token))?.Where((Person p) => ((PersonBase)p).Guid == e.PersonGuid).SingleOrDefault();
			if (person == null)
			{
				return;
			}
			DateTime day = e.DateFrom;
			List<TypeMorakhasi> typeMorakhasi = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			for (int i = 0; i < e.DayCount; i++)
			{
				Morakhasidaily _clsMorakhasidaily = new Morakhasidaily(((CacheBaseForm)this).ContextGuid)
				{
					PersonId = ((PersonBase)person).Id,
					DateFrom = e.DateFrom,
					Desc = e.ManagerMessage
				};
				EnPersonLeaveKind kind = e.Kind;
				switch (kind - 1)
				{
				case 1:
					_clsMorakhasidaily.TypeId = typeMorakhasi.FirstOrDefault((TypeMorakhasi q) => q.Name.Contains("استحقاقی"))?.Id ?? 2;
					break;
				case 0:
					_clsMorakhasidaily.TypeId = typeMorakhasi.FirstOrDefault((TypeMorakhasi q) => q.Name.Contains("استعلاجی"))?.Id ?? 1;
					break;
				case 3:
					_clsMorakhasidaily.TypeId = typeMorakhasi.FirstOrDefault((TypeMorakhasi q) => q.Name.Contains("بدون حقوق"))?.Id ?? 1;
					break;
				default:
					_clsMorakhasidaily.TypeId = 1;
					break;
				}
				_clsMorakhasidaily.DateFrom = day;
				if (await IsTatilAsync(_clsMorakhasidaily, person, await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid), token))
				{
					new Morakhasidaily(((CacheBaseForm)this).ContextGuid);
					day = day.AddDays(1.0);
				}
				else
				{
					await _clsMorakhasidaily.SaveAsync();
					day = day.AddDays(1.0);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			if (transactionHasBeenCreated && _databaseSemaphore != null)
			{
				_databaseSemaphore.Release();
			}
		}
	}

	private void CreateBackUp()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string defaultPath = BackupSetting.DefaultPath;
			try
			{
				if (File.Exists(defaultPath))
				{
					File.Delete(defaultPath);
				}
				if (!Directory.Exists(defaultPath))
				{
					Directory.CreateDirectory(defaultPath);
				}
				AutoBackup.StartThreadAsync(((CacheBaseForm)this).ContextGuid, (ENSource)5, defaultPath, VersionManagment.Version, default(CancellationToken));
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateBackUp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 599);
				AggregatorExtention.ShowError((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid), ex.Message + " \r\n directory is : " + defaultPath, "خطا");
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "CreateBackUp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 603);
		}
	}

	private void CheckUcCacheStatus()
	{
		try
		{
			UserAttSetting userAttSetting = UserAttendance.SelectedUser?.UserSetting;
			if (userAttSetting == null || !userAttSetting.EnableCloudSignalR)
			{
				((Control)ucCacheStatus1).Visible = false;
			}
			ucCacheStatus1.ContextGuid = ((CacheBaseForm)this).ContextGuid;
			ucCacheStatus1.ShowStatusMenu = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckUcCacheStatus", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 623);
		}
	}

	private async void CheckShowEndedGharardad()
	{
		try
		{
			if (_setting.ViewEndedGharardad)
			{
				FrmEndedGharardad frm = new FrmEndedGharardad(((CacheBaseForm)this).ContextGuid, _font);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckShowEndedGharardad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 635);
		}
	}

	private async Task OpenPortsForDevicesAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<DeviceInfo> source = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			List<int> ebioPorts = (from p in source
				where (int)p.TypeDeviceManufacture == 2 && (int)p.TransferMode == 1 && p.UseRealTime && p.PortRealtime > 100
				select p.PortRealtime).Distinct().ToList();
			List<int> HysoonPorts = (from p in source
				where (int)p.TypeDeviceManufacture == 3 && (int)p.TransferMode == 1 && p.UseRealTime && p.PortRealtime > 100
				select p.PortRealtime).Distinct().ToList();
			List<int> DynamicHysoonPorts = (from p in source
				where (int)p.TypeDeviceManufacture == 5 && (int)p.TransferMode == 1 && p.UseRealTime && p.PortRealtime > 100
				select p.PortRealtime).Distinct().ToList();
			List<DeviceInfo> VisibleLight761Groups = source.Where((DeviceInfo p) => (int)p.TypeDeviceManufacture == 6).Distinct().ToList();
			List<Person> persons = await Person.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int>(), personStatus: true, token);
			List<HardwareCodeCus> hardwareCodes = await HardwareCodeCus.GetAllAsync(((CacheBaseForm)this).ContextGuid);
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						List<int> list = DynamicHysoonPorts;
						if (list != null && list.Any())
						{
							foreach (int item in DynamicHysoonPorts)
							{
								HttpServerDynamicHysoon httpServerDynamicHysoon = new HttpServerDynamicHysoon(((CacheBaseForm)this).ContextGuid);
								httpServerDynamicHysoon.RealTimeLogEvent += HttpServer_RealTimeLogEvent;
								try
								{
									string ErrorStr = "";
									if (httpServerDynamicHysoon.Setup(item, persons, hardwareCodes, ref ErrorStr, (IDeviceLogOnPersonRepository)(object)new DeviceLogOnPersonRepository(((CacheBaseForm)this).ContextGuid)))
									{
										_dicDynamicHysoon.TryAdd(item, httpServerDynamicHysoon);
										Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, ErrorStr, "", 3);
									}
									else
									{
										Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, ErrorStr, "", 3);
									}
								}
								catch (Exception ex2)
								{
									Log.Error(ex2, "");
								}
							}
							return;
						}
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
			EbioRealTimeProcess.Start(((CacheBaseForm)this).ContextGuid, ebioPorts);
			foreach (DeviceInfo item2 in VisibleLight761Groups)
			{
				if ((int)item2.TransferMode != 2 && (int)item2.TransferMode != 0 && !string.IsNullOrEmpty(item2.CallBackIp) && item2.UseRealTime)
				{
					string text = $"http://{item2.CallBackIp}:{item2.PortRealtime}/LogCallBack/";
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "در حال گوش دادن به آدرس:\r\n" + text, "", 3);
				}
			}
			token.ThrowIfCancellationRequested();
			(await DestroyAllAxRealSvrOcxTcpComponentsAsync(token)).throwExceptionIfError();
			token.ThrowIfCancellationRequested();
			if (token.IsCancellationRequested || IsDisposed || !(HysoonPorts?.Any() ?? false))
			{
				return;
			}
			foreach (short item3 in HysoonPorts.Select((int v) => (short)v))
			{
				ReturnedSaveFuncInfoWithValue<AxRealSvrOcxTcp> val = await CreateControlAxRealSvrOcxTcpAsync(item3, token);
				if (((ReturnedSaveFuncInfo)val).HasError)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, "در هنگام ایجاد کنترل دسترسی به دستگاه با خطا مواجه شد", "خطا", 5);
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, ((ReturnedSaveFuncInfo)val).ErrorMessage, "خطا", 5);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OpenPortsForDevicesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 728);
		}
	}

	public void HttpServer_RealTimeLogEvent(string message)
	{
		try
		{
			Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)3, message, "تردد آنی", 5);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task EbioClient_Received(object sender, ReadEventArgs e)
	{
		byte[] bytes = Encoding.UTF8.GetBytes("{\"device_port\":5005,\"fk_device_id\":\"1\",\"io_mode\":\"1\",\"io_time\":\"20240118153723\",\"log_id\":\"12884901891\",\"serial_number\":\"5672023080019\",\"user_id\":\"1\",\"verify_mode\":\"PALM\"}");
		await ((BaseTcpClientConnection)sender).SendAsync(bytes, default(CancellationToken));
	}

	private async Task ReceiveDataDevicePolling()
	{
		await Task.Delay(60000);
		while (!IsDisposed)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					do
					{
						await Task.Delay(3000);
					}
					while (!((Control)this).IsHandleCreated);
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)9, "بررسی وضعیت دستگاه ها", "", 3);
					await Task.Delay(3000);
					List<DeviceInfo> list = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid);
					if (list != null && list.Any())
					{
						List<DeviceInfo> list2 = list.Where((DeviceInfo p) => (int)p.TransferMode == 1 && p.AutoRecieve && (DateTime.Now - p.SyncTime).TotalMinutes > (double)p.MinCheck).ToList();
						if (list2 == null || !list2.Any())
						{
							Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)9, "هیچ دستگاهی نیاز به بروزرسانی تردد نداشت", "", 3);
							continue;
						}
						if (!IsDisposed)
						{
							_tokenReceiveTaradod?.Cancel();
							_tokenReceiveTaradod = new CancellationTokenSource();
							CancellationToken token = _tokenReceiveTaradod.Token;
							Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)9, "بررسی ترددهای دستگاه", "", 3);
							Task.Run((Func<Task?>)ShowLoadingAsync);
							await ReceiveTaradodAsync(list2, token);
							await HideLoadingAsync();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
				}
			}
			finally
			{
				await HideLoadingAsync();
				await Task.Delay(600000);
			}
			int num2;
			if (num2 == 2)
			{
				break;
			}
		}
	}

	private async Task ReceiveTaradodAsync(List<DeviceInfo> list, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder();
		try
		{
			await _semaphoreReceiveData.WaitAsync(token);
			foreach (DeviceInfo device in list)
			{
				if (device.Id != 0)
				{
					await device.SyncDeviceAsync();
				}
				string text = "در حال دریافت تردد ها از دستگاه:\r\n " + device.DeviceName;
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)9, text ?? "", "", 3);
				device.SourceObj = this;
				ReturnedSaveFuncInfoWithValue<HozoorPackage> res = UserAttendance.SelectedUser.AccessLevelMerg.GetTypeSoftware();
				if (((ReturnedSaveFuncInfo)res).HasError)
				{
					((ReturnedSaveFuncInfo)(object)res).Show(((CacheBaseForm)this).ContextGuid);
				}
				HozoorPackage value = res.value;
				device.OnReceiveData += ShowEventMessage;
				device.OnDisconnected += ShowEventMessage;
				if (UserAttendance.SelectedUser != null && (int)value == 1 && device.RdEnrollDataTime.Date < DateTime.Now.Date)
				{
					device.OnFinishGetData += StartSportClub;
				}
				ReturnedSaveFuncInfo val = await Utility.PingAsync(device.Ip);
				if (val.HasError)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, val.ErrorMessage, "", 3);
					await Task.Delay(5000);
				}
				else
				{
					FrmEbio frmEbio = null;
					if ((int)device.TypeDeviceManufacture == 2)
					{
						((Control)this).Invoke((Delegate)(Action)delegate
						{
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							//IL_000d: Invalid comparison between Unknown and I4
							frmEbio = (((int)device.TransferMode == 1) ? new FrmEbio(((CacheBaseForm)this).ContextGuid, device.DeviceNo, int.Parse(device.CommunicationPass), device.Ip, device.TcpPort) : new FrmEbio(((CacheBaseForm)this).ContextGuid, device.DeviceNo));
						});
					}
					ReturnedSaveFuncInfo val2 = await device.ReceiveDataAsync(sb, frmEbio, token);
					if (val2.HasError || val2.HasWarning)
					{
						((Form)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val2, "[دریافت داده]", (MessageBoxButtons)0, false, (LanguageTypes)0, "ReceiveTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs")).Show((IWin32Window)(object)this);
					}
					if (((ReturnedSaveFuncInfo)res).HasInformation)
					{
						Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, val2.InformationMessage, "", 3);
					}
				}
				device.OnReceiveData -= ShowEventMessage;
				device.OnDisconnected -= ShowEventMessage;
				device.OnFinishGetData -= StartSportClub;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReceiveTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 839);
		}
		finally
		{
			_semaphoreReceiveData?.Release();
		}
	}

	private Task ShowLoadingAsync()
	{
		return Task.Run(delegate
		{
			if (((Control)pictureBoxLoading).InvokeRequired)
			{
				((Control)pictureBoxLoading).Invoke((Delegate)(Action)delegate
				{
					((Control)pictureBoxLoading).Visible = true;
				});
			}
			else
			{
				((Control)pictureBoxLoading).Visible = true;
			}
		});
	}

	private Task HideLoadingAsync()
	{
		return Task.Run(delegate
		{
			if (((Control)pictureBoxLoading).InvokeRequired)
			{
				((Control)pictureBoxLoading).Invoke((Delegate)(Action)delegate
				{
					((Control)pictureBoxLoading).Visible = false;
				});
			}
			else
			{
				((Control)pictureBoxLoading).Visible = false;
			}
		});
	}

	private async void StartSportClub(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			EnrollManage.GetEnrollManage(((CacheBaseForm)this).ContextGuid).OnCompleteOperation += Instance_OnCompleteOperation;
			await EnrollManage.GetEnrollManage(((CacheBaseForm)this).ContextGuid).SaveEnrollDataAsync();
			await EnrollManage.GetEnrollManage(((CacheBaseForm)this).ContextGuid).ClearEnrollOnDeviceAsync();
			await EnrollManage.GetEnrollManage(((CacheBaseForm)this).ContextGuid).PutEnrollActivePersonIntoDeviceAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "StartSportClub", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 877);
		}
	}

	private void Instance_OnCompleteOperation(object source, EventArgsNotify e)
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Instance_OnCompleteOperation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 892);
		}
	}

	private async Task CheckShowNotifyButtonAsync(CancellationToken token)
	{
		try
		{
			bool isExistNewNotif = await EventArgsNotify.CountAllAsync(((CacheBaseForm)this).ContextGuid, (EventNotifyEnumType)1, token) > 0;
			if (isExistNewNotif)
			{
				timerReadNotify.Start();
			}
			else
			{
				timerReadNotify.Stop();
			}
			if (((Control)this).InvokeRequired)
			{
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					((Control)btnNotifyMe).Visible = isExistNewNotif;
				});
			}
			else
			{
				((Control)btnNotifyMe).Visible = isExistNewNotif;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckShowNotifyButtonAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 912);
		}
	}

	private void TimerReadNotify_Tick(object sender, EventArgs e)
	{
		try
		{
			((Control)btnNotifyMe).BackColor = ((((Control)btnNotifyMe).BackColor == Color.FromArgb(94, 173, 244)) ? Color.FromArgb(132, 193, 255) : Color.FromArgb(94, 173, 244));
			((Control)btnNotifyMe).Refresh();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TimerReadNotify_Tick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 922);
		}
	}

	private async void FRMMain_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (!((Control)this).Visible)
			{
				((CancelEventArgs)(object)e).Cancel = true;
				return;
			}
			if (changedLanguage)
			{
				changedLanguage = false;
				return;
			}
			if (!_notshowUi)
			{
				if (!BackupSetting.EnableAutoBackupAllPartions)
				{
					if ((int)FMessageBox.Show("آیا میخواهید از برنامه خارج شوید؟", Utilities.GetTranslate("[پیام سیستم]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button1) == 7)
					{
						((CancelEventArgs)(object)e).Cancel = true;
						return;
					}
				}
				else
				{
					DialogResult val = FMessageBox.Show("آیا قبل از خروج، پشتیبان گیری انجام شود؟ ", Utilities.GetTranslate("[پیام سیستم]"), FMessageBox.FMessageBoxButtons.YesNoCancel, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button1, new List<string>(3) { "بله", "خیر", "لغو خروج" });
					if ((int)val == 2)
					{
						((CancelEventArgs)(object)e).Cancel = true;
						return;
					}
					if ((int)val == 6)
					{
						string text = Path.Combine(Application.StartupPath, TextResourceManagement.TextResource.DefaultBackUPFolderName);
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
						FRMExitAutoBackup frm = new FRMExitAutoBackup(((CacheBaseForm)this).ContextGuid, Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid), (ENSource)5, text, (BackUpMethod)1, (Guid?)null, (IBackUpLog_GetUserName)null, default(CancellationToken));
						await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
						((Component)(object)frm).Dispose();
					}
				}
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "پنجره برنامه حضور مخفی شد اما همچنان در پس زمینه در حال اجرا خواهد بود", "", 5);
			}
			timerLock.Stop();
			_tokenGetLastTaradods?.Cancel();
			_tokenReportDevices?.Cancel();
			((CancelEventArgs)(object)e).Cancel = true;
			((Control)this).Hide();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMMain_FormClosing", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 994);
		}
		finally
		{
			Log.Information("نرم افزار حضور بسته شد \r\n user: " + UserAttendance.SelectedUser.Name + "\r\n DB: " + ClsConnection.GetDbName(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid)) + "\r\n by exit from main form");
			LogUserLogout(UserAttendance.SelectedUser.Name);
		}
	}

	private async void FRMMain_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			switch (keyCode - 49)
			{
			case 74:
			{
				e.Handled = true;
				FrmCalc frmc = new FrmCalc(((CacheBaseForm)this).ContextGuid);
				await ((CacheBaseForm)frmc).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmc).Dispose();
				break;
			}
			case 22:
				e.Handled = true;
				BtnGharardad_Click(sender, (EventArgs)(object)e);
				break;
			case 31:
				e.Handled = true;
				BtnPersonel_Click(sender, (EventArgs)(object)e);
				break;
			case 34:
				e.Handled = true;
				BtnShift_Click(sender, (EventArgs)(object)e);
				break;
			case 21:
				e.Handled = true;
				BtnSFishD_Click(sender, (EventArgs)(object)e);
				break;
			case 35:
				e.Handled = true;
				RptTaradodFull_Click(sender, (EventArgs)(object)e);
				break;
			case 28:
				e.Handled = true;
				BtnMorakhasi_Click(sender, (EventArgs)(object)e);
				break;
			case 19:
				e.Handled = true;
				BtnDevice_Click(sender, (EventArgs)(object)e);
				break;
			case 27:
				e.Handled = true;
				BtnFishes_Click(sender, (EventArgs)(object)e);
				break;
			case 18:
				e.Handled = true;
				BtnHoliday_Click(sender, (EventArgs)(object)e);
				break;
			case 24:
				e.Handled = true;
				Btnsoftware_Click(sender, (EventArgs)(object)e);
				break;
			case 41:
				e.Handled = true;
				BtnSetting_Click(sender, (EventArgs)(object)e);
				break;
			case 37:
				e.Handled = true;
				BtnRptPersonsOneDay_Click(sender, (EventArgs)(object)e);
				break;
			case 63:
				e.Handled = true;
				ribbonTabItem1.Select();
				break;
			case 64:
				e.Handled = true;
				ribbonTabItem2.Select();
				break;
			case 65:
				e.Handled = true;
				ribbonTabItem3.Select();
				break;
			case 66:
				e.Handled = true;
				ribbonTabItem4.Select();
				break;
			case 67:
				e.Handled = true;
				ribbonTabItem5.Select();
				break;
			case 68:
				e.Handled = true;
				ribbonTabItem6.Select();
				break;
			case 0:
			case 48:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BTNUsers_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					RptTaradodFull_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnSFishD_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnRPTsaz_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnReadData_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem6).Text)
				{
					Btnsoftware_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 1:
			case 49:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnDevice_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					BtnRptPersonsOneDay_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnFishes_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnRptAllTaradod_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnPass_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem6).Text)
				{
					Btnactivation_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 2:
			case 50:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnCompany_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					BtnRptPersonsOneMounth_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnSHesab_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnRptNotify_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnSetting_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem6).Text)
				{
					BtnAboutUs_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 3:
			case 51:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnHoliday_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					BtnMorakhasi_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnAgreedDeduction_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnVacations_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnBazsazi_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem6).Text)
				{
					BtnCallUs_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 4:
			case 52:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnShift_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					BtnTypeTaradod_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnMaliyat_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnTajmiVacations_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnBackup_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem6).Text)
				{
					BtnTutorialVideo_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 5:
			case 53:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnProgrammableShift_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem2).Text)
				{
					BtnRptMorakhasi_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnListBimeh_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem4).Text)
				{
					BtnEndedGharardad_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnRestore_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 6:
			case 54:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnDefMorakhsi_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem3).Text)
				{
					BtnCheckOut_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnCalc_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 7:
			case 55:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnPersonel_Click(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnManagment_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 8:
			case 56:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnListM_Click_1(sender, (EventArgs)(object)e);
				}
				else if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem5).Text)
				{
					BtnLogUser_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 16:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnGharardad_Click(sender, (EventArgs)(object)e);
				}
				break;
			case 17:
				e.Handled = true;
				if (((BaseItem)ribbonControl1.SelectedRibbonTabItem).Text == ((BaseItem)ribbonTabItem1).Text)
				{
					BtnBaseYearsSalary_Click(sender, (EventArgs)(object)e);
				}
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1226);
		}
	}

	private void InitAccessLevel()
	{
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((BaseItem)BtnGharardad).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain;
			((BaseItem)BtnMorakhasiSaati).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain;
			((BaseItem)BtnMorakhasi).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain;
			((BaseItem)BtnRptMorakhasi).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain;
			((BaseItem)BtnRptPersonsOneDay).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportTaradodPersonnelOnADay;
			((BaseItem)BtnRptPersonsOneMounth).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportAmarPersonnel;
			((BaseItem)BtnRptTaradodFull).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportTaradodPersonnel;
			((BaseItem)BTNUsers).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersMain;
			((BaseItem)BtnCompany).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Company.CompanyMain;
			((BaseItem)BtnFishes).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.BillPriceMain;
			((BaseItem)BtnSFishM).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.BillPriceMain;
			((BaseItem)BtnSFishD).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.BillPriceMain;
			((BaseItem)BtnRPTsaz).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Reports.ReportsGozareshSazPoya;
			((BaseItem)BtnDefMorakhsi).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain;
			((BaseItem)BtnSHesab).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingMain;
			((BaseItem)BtnHoliday).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationHolidays;
			((BaseItem)BtnTypeTaradod).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodFunctionKeys;
			((BaseItem)BtnProgrammableShift).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftProgrammable;
			((BaseItem)BtnShift).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftMain;
			((BaseItem)BtnPersonel).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelMain;
			((BaseItem)BtnBaseYearsSalary).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.BaseYearsSalary.BaseYearsSalaryMain;
			((BaseItem)BtnLogUser).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Users.UsersReportLogs;
			((BaseItem)BtnRptAllTaradod).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.RptTaradodAllPersonnel;
			((BaseItem)BtnDevice).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Device.DeviceMain;
			((BaseItem)BtnAgreedDeduction).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.AgreedDeduction.AgreedDeductionMain;
			((BaseItem)btnTaxTable).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.TaxTable.TaxtableMain;
			((BaseItem)BtnListBimeh).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Insurance.InsuranceMain;
			ReturnedSaveFuncInfoWithValue<HozoorPackage> typeSoftware = UserAttendance.SelectedUser.AccessLevelMerg.GetTypeSoftware();
			if (((ReturnedSaveFuncInfo)typeSoftware).HasError)
			{
				((ReturnedSaveFuncInfo)(object)typeSoftware).Show(((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitAccessLevel", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1279);
		}
	}

	private async void BtnRPTsaz_X_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			ButtonItem val = (ButtonItem)sender;
			int.TryParse(TextManager.Instance.RemoveNoNumbers(((BaseItem)val).Name, ""), out var result);
			if (result > 0)
			{
				RPTGen rpt = await RPTGen.GetAsync(((CacheBaseForm)this).ContextGuid, result);
				TimeAttendanceUI.ReportProducer.FrmFilter frm = new TimeAttendanceUI.ReportProducer.FrmFilter(((CacheBaseForm)this).ContextGuid, rpt, _font);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					((Component)(object)frm).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRPTsaz_X_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1298);
		}
	}

	private async Task AddDynamicReportsBtnsAsync()
	{
		try
		{
			List<RPTGen> list = await RPTGen.GetAllAsync(((CacheBaseForm)this).ContextGuid);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			List<ButtonItem> listReports = _listReports;
			if (listReports != null && listReports.Count > 0)
			{
				foreach (ButtonItem listReport in _listReports)
				{
					ribbonBar6.Items.Remove((BaseItem)(object)listReport);
					((Component)(object)listReport).Dispose();
				}
			}
			_listReports = new List<ButtonItem>();
			foreach (RPTGen item in list)
			{
				ButtonItem val = new ButtonItem
				{
					Image = (Image)(object)Resources.rptsaz,
					ImagePaddingHorizontal = 55,
					ImagePosition = (eImagePosition)2,
					Name = "BtnRPTsaz_" + item.ID,
					SubItemsExpandWidth = 14,
					Text = item.Title
				};
				ribbonBar6.Items.Add((BaseItem)(object)val);
				_listReports.Add(val);
				((BaseItem)val).Click += BtnRPTsaz_X_Click;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddDynamicReportsBtnsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1333);
		}
	}

	private void TimerStopNotification_Tick(object sender, EventArgs e)
	{
		if (_stopNotify.HasValue && _stopNotify < DateTime.Now)
		{
			notifyIcon1.Text = Utilities.GetTranslate("[نرم افزار حضور غیاب و حقوق دستمزد]");
			notifyIcon1.Icon = Resources.TimeAttendanceIcon;
			_stopNotify = null;
		}
	}

	private async void ShowEventMessage(object source, EventArgsNotify e)
	{
		try
		{
			if (((Control)this).InvokeRequired)
			{
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					ShowEventMessage(source, e);
				});
				return;
			}
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, e.Message, "", 3);
			_stopNotify = DateTime.Now.AddSeconds(e.Secounts);
			_tokenNotifyMe?.Cancel();
			_tokenNotifyMe = new CancellationTokenSource();
			CancellationToken token = _tokenNotifyMe.Token;
			await CheckShowNotifyButtonAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ShowEventMessage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1372);
		}
	}

	private void CMSAboutUS_Click(object sender, EventArgs e)
	{
		BtnAboutUs_Click(sender, e);
	}

	private async void CMSOpenProgram_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (UserAttendance.SelectedUser.UserSetting.ShowFrmEnter)
			{
				FrmEnter frmUser = new FrmEnter(((CacheBaseForm)this).ContextGuid, autoLogin: false, canChangeUser: false);
				DialogResult val = await ((CacheBaseForm)frmUser).ShowAsync((IWin32Window)(object)this);
				((Form)frmUser).Close();
				if ((int)val == 1)
				{
					timerLock.Start();
					((Control)this).Show();
					ReloadDgridLastTaradods();
					ReloadDgridReportDevices();
					await OpenPortsForDevicesAsync();
				}
			}
			else
			{
				timerLock.Start();
				((Control)this).Show();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMSOpenProgram_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1408);
		}
	}

	private void ReloadDgridLastTaradods()
	{
		try
		{
			_tokenGetLastTaradods?.Cancel();
			((Control)DgridLastTaradods).Visible = UserAttendance.SelectedUser.UserSetting.viewGridLastTaradods;
			_tokenGetLastTaradods = new CancellationTokenSource();
			CancellationToken token = _tokenGetLastTaradods.Token;
			Utility.ReloadDgridLastTaradodsAsync(((CacheBaseForm)this).ContextGuid, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void ReloadDgridReportDevices()
	{
		try
		{
			_tokenReportDevices?.Cancel();
			((Control)DgridReportDevices).Visible = UserAttendance.SelectedUser.UserSetting.viewGridReportDevices;
			_tokenReportDevices = new CancellationTokenSource();
			Utility.ReloadDgridReportDevicesAsync(_tokenReportDevices.Token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMSExitProgram_Click(object sender, EventArgs e)
	{
		try
		{
			await ClosePortsOfDevices();
			((Component)(object)notifyIcon1).Dispose();
			LogUserLogout(UserAttendance.SelectedUser.Name);
			Log.Information("نرم افزار حضور بسته شد \r\n user: " + UserAttendance.SelectedUser.Name + "\r\n DB: " + ClsConnection.GetDbName(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid)) + "\r\n by kill Hozor process");
			Process.GetCurrentProcess().Kill();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnNotifyMe_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)btnNotifyMe).Visible = false;
			FrmEventNotifyList frmEventNotifyList = new FrmEventNotifyList(((CacheBaseForm)this).ContextGuid, pIsSelect: true, _font);
			((Form)frmEventNotifyList).ShowDialog((IWin32Window)(object)this);
			((Component)(object)frmEventNotifyList).Dispose();
			_tokenNotifyMe?.Cancel();
			_tokenNotifyMe = new CancellationTokenSource();
			CancellationToken token = _tokenNotifyMe.Token;
			await CheckShowNotifyButtonAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnNotifyMe_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1465);
		}
	}

	private async void BTNUsers_Click(object sender, EventArgs e)
	{
		try
		{
			FrmUserMain frmUserMain = new FrmUserMain(((CacheBaseForm)this).ContextGuid, isSelect: false, _font);
			((Form)frmUserMain).WindowState = (FormWindowState)2;
			FrmUserMain frm = frmUserMain;
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNUsers_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1481);
		}
	}

	private async void BtnDevice_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			FrmDeviceMain frm = new FrmDeviceMain(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			await ClosePortsOfDevices();
			await OpenPortsForDevicesAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDevice_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1494);
		}
	}

	private async Task ClosePortsOfDevices(CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<DeviceInfo> source = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			List<int> ebioPorts = (from p in source
				where (int)p.TypeDeviceManufacture == 2 && (int)p.TransferMode == 1 && p.UseRealTime && p.PortRealtime > 100
				select p.PortRealtime).Distinct().ToList();
			(from p in source
				where (int)p.TypeDeviceManufacture == 3 && (int)p.TransferMode == 1 && p.UseRealTime && p.PortRealtime > 100
				select p.PortRealtime).Distinct().ToList();
			EbioRealTimeProcess.Stop(ebioPorts);
			foreach (KeyValuePair<int, HttpServerDynamicHysoon> item in _dicDynamicHysoon)
			{
				item.Value.Disconnect();
				HttpServerDynamicHysoon value = null;
				_dicDynamicHysoon.TryRemove(item.Key, out value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ClosePortsOfDevices", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1531);
		}
	}

	private async void BtnCompany_Click(object sender, EventArgs e)
	{
		try
		{
			FrmCompanyMain frmCompanyMain = new FrmCompanyMain(((CacheBaseForm)this).ContextGuid, _font);
			((Form)frmCompanyMain).WindowState = (FormWindowState)2;
			FrmCompanyMain frm = frmCompanyMain;
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCompany_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1542);
		}
	}

	private async void BtnHoliday_Click(object sender, EventArgs e)
	{
		try
		{
			FRMHolidayMain frm = new FRMHolidayMain(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnHoliday_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1553);
		}
	}

	private async void BtnShift_Click(object sender, EventArgs e)
	{
		try
		{
			if (UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftMain)
			{
				FrmShiftMain frm = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: false);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
			else
			{
				Utility.MessageBoxShowError("[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید شیفت کاری ایجاد نمائید]", ((CacheBaseForm)this).ContextGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnShift_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1570);
		}
	}

	private async void BtnProgrammableShift_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: false, -1, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)18);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnProgrammableShift_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1582);
		}
	}

	private async void BtnDefMorakhsi_Click(object sender, EventArgs e)
	{
		try
		{
			DefMorakhasiTypeMain frm = new DefMorakhasiTypeMain(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDefMorakhsi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1593);
		}
	}

	private async void BtnPersonel_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSelectPersonel frmU = new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frmU).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frmU).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPersonel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1604);
		}
	}

	private async void BtnListM_Click_1(object sender, EventArgs e)
	{
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, -1);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnListM_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1615);
		}
	}

	private async void BtnGharardad_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)16);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnGharardad_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1627);
		}
	}

	private async void BtnBaseYearsSalary_Click(object sender, EventArgs e)
	{
		try
		{
			FrmBaseYearsSalaryList frm = new FrmBaseYearsSalaryList(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnBaseYearsSalary_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1638);
		}
	}

	private async void BtnDefinePackage_Click(object sender, EventArgs e)
	{
		try
		{
			FrmPackageList frm = new FrmPackageList(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnAttendanceSport_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: false, -1, bUcPersonTak: true, 1, bUcDateTak: false, -1, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)22);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAttendanceSport_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1664);
		}
	}

	private async void RptTaradodFull_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -2, bUcPerson: false, -1, bUcPersonTak: true, 1, bUcDateTak: true, 0, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)9);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RptTaradodFull_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1680);
		}
	}

	private async void BtnRptPersonsOneDay_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -2, bUcPerson: true, 1, bUcPersonTak: false, 1, bUcDateTak: false, 0, bUcTakDay: true, 0, bUcDesc: false, -5, (ReportFilterEnumType)11);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRptPersonsOneDay_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1692);
		}
	}

	private async void BtnRptPersonsOneMounth_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -1, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)7)).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRptPersonsOneMounth_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1703);
		}
	}

	private async void BtnMorakhasi_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -1, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: true, 2, (ReportFilterEnumType)12);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1715);
		}
	}

	private async void BtnMorakhasiSaati_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -1, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: true, 2, (ReportFilterEnumType)13);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorakhasiSaati_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1727);
		}
	}

	private async void BtnTypeTaradod_Click(object sender, EventArgs e)
	{
		try
		{
			FrmTypeTaradodList frm = new FrmTypeTaradodList(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnTypeTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1738);
		}
	}

	private async void BtnRptMorakhasi_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -1, bUcPerson: false, -2, bUcPersonTak: false, -3, bUcDateTak: false, -4, bUcTakDay: false, -5, bUcDesc: false, -6, (ReportFilterEnumType)15);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRptMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1750);
		}
	}

	private async void BtnListBimeh_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -1, bUcPerson: true, 2, bUcPersonTak: false, -2, bUcDateTak: true, 1, bUcTakDay: false, -3, bUcDesc: false, -5, (ReportFilterEnumType)23);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnListBimeh_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1764);
		}
	}

	private async void BtnMaliyat_Click(object sender, EventArgs e)
	{
		try
		{
			FrmTaxTableList frmTaxTableList = new FrmTaxTableList(((CacheBaseForm)this).ContextGuid, _font);
			((Form)frmTaxTableList).WindowState = (FormWindowState)2;
			FrmTaxTableList frm = frmTaxTableList;
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMaliyat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1777);
		}
	}

	private async void BtnSFishM_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: false, -1, bUcPersonTak: true, 1, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)10);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSFishM_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1791);
		}
	}

	private async void BtnSFishD_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -1, bUcPerson: true, 2, bUcPersonTak: false, -2, bUcDateTak: true, 1, bUcTakDay: false, -3, bUcDesc: false, -5, (ReportFilterEnumType)21);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSFishD_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1803);
		}
	}

	private async void BtnFishes_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -1, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)14);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnFishes_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1815);
		}
	}

	private async void BtnSHesab_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: false, -6, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)20);
			if ((int)((Form)frm).ShowDialog() == 1)
			{
				FrmSanadMain frmSanadMain = new FrmSanadMain(((CacheBaseForm)this).ContextGuid, _font, frm.DateM1, frm.DateM2);
				await ((CacheBaseForm)frmSanadMain).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmSanadMain).Dispose();
			}
			else
			{
				FrmSanadMain frmSanadMain = new FrmSanadMain(((CacheBaseForm)this).ContextGuid, _font);
				await ((CacheBaseForm)frmSanadMain).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmSanadMain).Dispose();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSHesab_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1839);
		}
	}

	private async void BtnAgreedDeduction_Click(object sender, EventArgs e)
	{
		try
		{
			FrmAgreedDeductionList frmAgreedDeductionList = new FrmAgreedDeductionList(((CacheBaseForm)this).ContextGuid, _font, isSelect: false);
			((Form)frmAgreedDeductionList).WindowState = (FormWindowState)2;
			FrmAgreedDeductionList frm = frmAgreedDeductionList;
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAgreedDeduction_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1850);
		}
	}

	private async void BtnRPTsaz_Click(object sender, EventArgs e)
	{
		try
		{
			FrmRPTGenMain frm = new FrmRPTGenMain(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRPTsaz_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1867);
		}
	}

	private async void BtnRptNotify_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: false, -1, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)24);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRptNotify_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1880);
		}
	}

	private async void BtnRptAllTaradod_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: false, -2, bUcPerson: true, 1, bUcPersonTak: false, -1, bUcDateTak: true, 0, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)19);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRptAllTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1892);
		}
	}

	private async void BtnVacations_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, (ReportFilterEnumType)25);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnVacations_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1903);
		}
	}

	private async void BtnReadData_Click(object sender, EventArgs e)
	{
		try
		{
			FrmRecieveData frm = new FrmRecieveData(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnReadData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1920);
		}
	}

	private async void BtnPass_Click(object sender, EventArgs e)
	{
		try
		{
			FRMSetPass frm = new FRMSetPass(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPass_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1934);
		}
	}

	private async void BtnSetting_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			LanguageTypes beforeLang = Utility.GetLanguage();
			FrmSetting frm = new FrmSetting(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			InitPayamakService(_setting);
			if (Utility.GetLanguage() != beforeLang)
			{
				_questionBackupAsked = true;
				((Component)this).Dispose();
				await ((CacheBaseForm)new FrmMain(((CacheBaseForm)this).ContextGuid, showUi: false)).ShowAsync((IWin32Window)(object)this);
			}
			cmsShowReportDevice.Checked = UserAttendance.SelectedUser.UserSetting.viewGridReportDevices;
			cmsShowLastTaradod.Checked = UserAttendance.SelectedUser.UserSetting.viewGridLastTaradods;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSetting_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 1962);
		}
	}

	private void InitPayamakService(TimeAttendanceSettings _setting)
	{
		if (((AttDepartmentAdminSms)_setting.Payamak).EnablePayamak && !((Control)ucPayamakServiceStatus1).Visible)
		{
			ucPayamakServiceStatus1.ContextGuid = ((CacheBaseForm)this).ContextGuid;
			ucPayamakServiceStatus1.Owner = (CacheBaseForm)(object)this;
			((Control)ucPayamakServiceStatus1).Visible = true;
			((Control)BtnPayamak).Visible = false;
		}
		else if (!((AttDepartmentAdminSms)_setting.Payamak).EnablePayamak && ((Control)ucPayamakServiceStatus1).Visible)
		{
			ucPayamakServiceStatus1.UnInit();
			((Control)ucPayamakServiceStatus1).Visible = false;
			((Control)BtnPayamak).Visible = true;
		}
	}

	private async void BtnBazsazi_Click(object sender, EventArgs e)
	{
		_ = 4;
		try
		{
			if (_semaphoreRestoreOrBazsazi == null)
			{
				_semaphoreRestoreOrBazsazi = new SemaphoreSlim(1, 1);
			}
			if (_semaphoreRestoreOrBazsazi != null && !(await _semaphoreRestoreOrBazsazi.WaitAsync(1)))
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "پردازشی دیگر روی دیتابیس در حال اجرا است . لطفا چند دقیقه دیگر اقدام به بازسازی اطلاعات نمائید", "", 3);
				return;
			}
			_tokenBazsazi?.Cancel();
			_tokenBazsazi = new CancellationTokenSource();
			CancellationToken token = _tokenBazsazi.Token;
			EnablePanelBazsazi(token);
			ReturnedSaveFuncInfo val = await Bazsazi.BazsaziDb(((CacheBaseForm)this).ContextGuid, runScripts: true, token);
			if (val.HasError)
			{
				await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val, "", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnBazsazi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs")).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[بازسازی اطلاعات با موفقیت انجام شد]", "", 3);
			}
			List<HardwareCodeCus> list = await HardwareCodeCus.GetDuplicateAsync(((CacheBaseForm)this).ContextGuid, token);
			if (list != null && list.Any())
			{
				ReturnedSaveFuncInfo errorList = new ReturnedSaveFuncInfo();
				foreach (HardwareCodeCus HardwareCodeCus in list)
				{
					Person person = await Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, HardwareCodeCus.PersonId, token);
					errorList.AddError("کد سخت افزاری تکراری:\r\n کد سخت افزاری: " + HardwareCodeCus.HardwareCode + " \r\n نام شخص: " + ((person != null) ? ((PersonBase)person).FullName : null) + " \r\n  نسبت به اصلاح آن اقدام نمائید ", "", "BtnBazsazi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2020, (StringBuilder)null);
				}
				errorList.Show(((CacheBaseForm)this).ContextGuid);
			}
			_semaphoreRestoreOrBazsazi?.Release();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnBazsazi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2028);
			SemaphoreSlim semaphoreRestoreOrBazsazi = _semaphoreRestoreOrBazsazi;
			if (semaphoreRestoreOrBazsazi != null && semaphoreRestoreOrBazsazi.CurrentCount > 0)
			{
				_semaphoreRestoreOrBazsazi?.Release();
			}
		}
		finally
		{
			_tokenBazsazi?.Cancel();
			ReloadDgridLastTaradods();
		}
	}

	private async void BtnBackup_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			string xdesc = "";
			Waiter waiter = new Waiter("در حال پشتیبان گیری اطلاعات", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BtnBackup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs");
			ReturnedSaveFuncInfo val = await DataBase.BackUpStartAsync(((CacheBaseForm)this).ContextGuid, (IWin32Window)(object)this, new BackUpConfig(VersionManagment.Version)
			{
				ConnectionString = Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid),
				Source = (ENSource)5,
				StartUpPath = Application.StartupPath
			}, default(CancellationToken), (IBackUpLog_GetUserName)null);
			if (waiter != null)
			{
				waiter.Dispose();
			}
			if (val.HasError)
			{
				await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val, "[خطا در پشتیبان گیری اطلاعات]", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnBackup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs")).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				xdesc = "[پشتیبانگیری از اطلاعات انجام شد، در تاریخ ] : " + Calendar.MiladiToCalture(DateTime.Now);
				FMessageBox.Show(Utilities.GetTranslate("[پشتیبان گیری با موفقیت انجام شد]"), Utilities.GetTranslate("[پیام سیستم]"), FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Information);
			}
			LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "SoftwareSetting.Backup");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnBackup_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2065);
		}
	}

	private async Task EnablePanelBazsazi(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!token.IsCancellationRequested)
			{
				((Control)ImgBazsazi).Visible = true;
				((Control)(object)ImgBazsazi).SetTooltip("در حال بازسازی دیتابیس", "", "", (eTooltipColor)0);
				while (!token.IsCancellationRequested)
				{
					((Control)ImgBazsazi).Size = new Size(39, 39);
					await Task.Delay(500);
					((Control)ImgBazsazi).Size = new Size(34, 34);
					await Task.Delay(500);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnablePanelBazsazi", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2083);
		}
		finally
		{
			((Control)ImgBazsazi).Visible = false;
		}
	}

	private async void BtnRestore_Click(object sender, EventArgs e)
	{
		_tokenRestore?.Cancel();
		_tokenRestore = new CancellationTokenSource();
		_ = _tokenRestore.Token;
		try
		{
			if (Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid) == null)
			{
				return;
			}
			if (_semaphoreRestoreOrBazsazi == null)
			{
				_semaphoreRestoreOrBazsazi = new SemaphoreSlim(1, 1);
			}
			if (_semaphoreRestoreOrBazsazi != null && !(await _semaphoreRestoreOrBazsazi.WaitAsync(1)))
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "پردازشی دیگر روی دیتابیس در حال اجرا است . لطفا چند دقیقه دیگر اقدام به بازگردانی اطلاعات نمائید", "", 3);
				return;
			}
			Waiter waiter = new Waiter("در حال بازگردانی اطلاعات", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BtnRestore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs");
			ReturnedSaveFuncInfo val = await DataBase.ReStoreStartAsync(((CacheBaseForm)this).ContextGuid, (IWin32Window)(object)this, Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid), (ENSource)5, "", true, (Guid?)null, (IBackUpLog_GetUserName)null);
			if (waiter != null)
			{
				waiter.Dispose();
			}
			string xdesc;
			if (val.HasError)
			{
				xdesc = val.ErrorMessage;
				await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val, "[خطا در بازگردانی اطلاعات]", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnRestore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs")).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				xdesc = "[بازگردانی اطلاعات انجام شد، در تاریخ]  : " + Calendar.MiladiToCalture(DateTime.Now);
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[بازگردانی اطلاعات با موفقیت انجام شد.]", "", 3);
				await SettingAttendance.SetValueAsync(((CacheBaseForm)this).ContextGuid, "DbGuid", Guid.NewGuid().ToString(), 0);
			}
			bool num = await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "SoftwareSetting.Backup");
			_semaphoreRestoreOrBazsazi?.Release();
			if (num && (int)FMessageBox.Show(Utilities.GetTranslate("[قبل از هر اقدامی بازسازی اطلاعات الزامی است. آیا اکنون بازسازی شود؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1) == 6)
			{
				BtnBazsazi_Click(sender, e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRestore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2137);
			SemaphoreSlim semaphoreRestoreOrBazsazi = _semaphoreRestoreOrBazsazi;
			if (semaphoreRestoreOrBazsazi != null && semaphoreRestoreOrBazsazi.CurrentCount > 0)
			{
				_semaphoreRestoreOrBazsazi?.Release();
			}
		}
		finally
		{
			ReloadDgridLastTaradods();
		}
	}

	private async void BtnCalc_Click(object sender, EventArgs e)
	{
		try
		{
			FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCalc_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2157);
		}
	}

	private async void BtnManagment_Click(object sender, EventArgs e)
	{
		try
		{
			FrmManagment_Login frm = new FrmManagment_Login(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnManagment_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2169);
		}
	}

	private async void BtnLogUser_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, (ReportFilterEnumType)17);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnLogUser_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2180);
		}
	}

	private async void Btnsoftware_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSoftwareProp frm = new FrmSoftwareProp(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Btnsoftware_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2195);
		}
	}

	private async void Btnactivation_Click(object sender, EventArgs e)
	{
		try
		{
			if (LkOperations._isFreeVersion)
			{
				Utility.MessageBoxShow("در نسخه رایگان امکان فعالسازی وجود ندارد", ((CacheBaseForm)this).ContextGuid);
				return;
			}
			UserAttendance _clsUser = await UserAttendance.GetAsync(((CacheBaseForm)this).ContextGuid, UserAttendance.SelectedUser.ID);
			_ = UserAttendance.SelectedUser.ID;
			Level_Hozor levelCurrent = AccessLevelClass.Instance.LevelHozor;
			await ((CacheBaseForm)((LkUiFactory)this).NewFrmActivation(((CacheBaseForm)this).ContextGuid, new AttendancePackageClass(), (Package)2)).ShowAsync((IWin32Window)(object)this);
			MainFormOwner.owner = (ILkUiFactory)(object)this;
			ReturnedSaveFuncInfoWithValue<Level_Hozor> res = new LkOperations(((CacheBaseForm)this).ContextGuid).CheckActivation_Hozor();
			if (((ReturnedSaveFuncInfo)res).HasError)
			{
				await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, (ReturnedSaveFuncInfo)(object)res, "", (MessageBoxButtons)0, false, (LanguageTypes)0, "Btnactivation_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs")).ShowAsync((IWin32Window)(object)this);
			}
			Level_Hozor levelUpdated = res.value;
			_clsUser?.GetPassword();
			UserAttendance.SelectedUser.AccessLevelSelf = new AccessLevelClass(true);
			UserAttendance.SelectedUser.OldPassword = "";
			await UserAttendance.SelectedUser.SaveAsync();
			if (levelCurrent != levelUpdated)
			{
				((Control)this).Text = "نـــــرم افــزار جــــــامع حضـــــور غـیـاب و حـــقـوق دسـتــــمـزد نوین پرداز";
				((Control)this).Text = ((Control)this).Text + Utilities.GetTranslate($" ( [سطح نرم افزار] : {levelUpdated} )");
				string statusDesc = Utilities.GetTranslate($"[سطح نرم افزار از]   ( {levelCurrent} ) [به]  ( {levelUpdated} ) [تغییر یافت. جهت مشاهده تغییرات، نرم افزار را مجدد اجرا کنید]                   ");
				_tokenSource?.Cancel();
				_tokenSource = new CancellationTokenSource();
				CancellationToken token = _tokenSource.Token;
				Task.Run(() => SetStatusAsync(statusDesc, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Btnactivation_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2239);
		}
	}

	private async Task SetStatusAsync(string text, CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			bool key = true;
			if (((Control)ucStatuse1).InvokeRequired)
			{
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					SetStatusAsync(text, token);
				});
				return;
			}
			((ToolStripItem)ucStatuse1.tssDesc).Text = text;
			while (true)
			{
				await Task.Delay(500, token);
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
				}
				if (key)
				{
					((ToolStripItem)ucStatuse1.tssDesc).ForeColor = Color.Blue;
					key = false;
				}
				else
				{
					((ToolStripItem)ucStatuse1.tssDesc).ForeColor = Color.Red;
					key = true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2277);
		}
	}

	private async void BtnAboutUs_Click(object sender, EventArgs e)
	{
		try
		{
			FrmAboutUs frm = new FrmAboutUs(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAboutUs_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2288);
		}
	}

	private async void BtnCallUs_Click(object sender, EventArgs e)
	{
		try
		{
			FrmCallUs frm = new FrmCallUs(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCallUs_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2299);
		}
	}

	private async void BtnTutorialVideo_Click(object sender, EventArgs e)
	{
		try
		{
			FrmTutorialVideo frm = new FrmTutorialVideo(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnTutorialVideo_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2312);
		}
	}

	private void BtnCheckOut_Click(object sender, EventArgs e)
	{
	}

	private async void BtnEndedGharardad_Click(object sender, EventArgs e)
	{
		try
		{
			FrmEndedGharardad frm = new FrmEndedGharardad(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEndedGharardad_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2362);
		}
	}

	private async void BtnTajmiVacations_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceUI.Forms.Reports.FrmFilter frm = new TimeAttendanceUI.Forms.Reports.FrmFilter(((CacheBaseForm)this).ContextGuid, _setting, 0, DateTime.Now, bShowForm: true, bUcDate: true, 0, bUcPerson: true, 1, bUcPersonTak: false, -2, bUcDateTak: false, -3, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)28);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnTajmiVacations_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2375);
		}
	}

	private async void LanguageLable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			changedLanguage = true;
			LanguageTypes language = Utility.GetLanguage();
			switch ((int)language)
			{
			case 0:
			{
				Utility.SetLanguage((LanguageTypes)4);
				LanguageTypes language2 = Utility.GetLanguage();
				if ((int)language2 != 4)
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language2, TextResourceManagement.TextLanguageFarsiTimeAttendance, true, (ENSource)5);
				}
				else
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language2, TextResourceManagement.TextLanguageArabicTimeAttendance, true, (ENSource)5);
				}
				break;
			}
			case 4:
			{
				Utility.SetLanguage((LanguageTypes)0);
				LanguageTypes language2 = Utility.GetLanguage();
				if ((int)language2 != 4)
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language2, TextResourceManagement.TextLanguageFarsiTimeAttendance, true, (ENSource)5);
				}
				else
				{
					LanguageAndCalendar.Instance.ChangeLanguage(language2, TextResourceManagement.TextLanguageArabicTimeAttendance, true, (ENSource)5);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			case 1:
			case 5:
			case 6:
			case 7:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LanguageLable_LinkClicked", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2413);
		}
		_questionBackupAsked = true;
		((Component)this).Dispose();
		await ((CacheBaseForm)new FrmMain(((CacheBaseForm)this).ContextGuid, showUi: false)).ShowAsync((IWin32Window)(object)this);
	}

	private async void MonthCalendarX1_SelectedDateChanged(DateTime selectedDateTime, PersianDateTime selectedPersianDateTime)
	{
		try
		{
			DateTime selectedDateInDateTime = monthCalendarX1.GetSelectedDateInDateTime();
			Holiday holiday = await Holiday.GetAsync(((CacheBaseForm)this).ContextGuid, selectedDateInDateTime);
			((Control)TbxMonasebat).Text = holiday.Monasebat;
			((Control)TbxMonasebat).Visible = ((Control)TbxMonasebat).Text.Trim().Length > 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MonthCalendarX1_SelectedDateChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2429);
		}
	}

	private async void MonthCalendarX1_Load(object sender, EventArgs e)
	{
		try
		{
			Holiday holiday = await Holiday.GetAsync(((CacheBaseForm)this).ContextGuid, DateTime.Today);
			((Control)TbxMonasebat).Text = holiday.Monasebat;
			((Control)TbxMonasebat).Visible = ((Control)monthCalendarX1).Visible && ((Control)TbxMonasebat).Text.Trim().Length > 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MonthCalendarX1_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2440);
		}
	}

	private async void cmsShowLastTaradod_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UserAttendance.SelectedUser.UserSetting.viewGridLastTaradods = cmsShowLastTaradod.Checked;
			await UserAttendance.SelectedUser.SaveAsync();
			ReloadDgridLastTaradods();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "cmsShowLastTaradod_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2452);
		}
	}

	private async void cmsShowReportDevice_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			UserAttendance.SelectedUser.UserSetting.viewGridReportDevices = cmsShowReportDevice.Checked;
			await UserAttendance.SelectedUser.SaveAsync();
			ReloadDgridReportDevices();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "cmsShowReportDevice_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2463);
		}
	}

	private async void cmsViewCalendar_CheckedChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			UserAttendance.SelectedUser.UserSetting.ViewCalendar = cmsViewCalendar.Checked;
			await UserAttendance.SelectedUser.SaveAsync();
			_tokenHoliday?.Cancel();
			_tokenHoliday = new CancellationTokenSource();
			CancellationToken token = _tokenHoliday.Token;
			await LoadHolidayForCalendarAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "cmsViewCalendar_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2478);
		}
	}

	private async void BtnClub_Click(object sender, EventArgs e)
	{
		try
		{
			FrmClubMain frmClubMain2 = new FrmClubMain(((CacheBaseForm)this).ContextGuid, _font);
			((Form)frmClubMain2).StartPosition = (FormStartPosition)4;
			FrmClubMain frmClubMain = frmClubMain2;
			await ((CacheBaseForm)frmClubMain).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frmClubMain).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnClub_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2491);
		}
	}

	private void BtnReloadEnrollData_Click(object sender, EventArgs e)
	{
		try
		{
			StartSportClub(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnReloadEnrollData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2503);
		}
	}

	private void ribbonControl1_Click(object sender, EventArgs e)
	{
	}

	private void ribbonTabItemSportClub_Click(object sender, EventArgs e)
	{
	}

	private void ribbonTabItemSportClub_DoubleClick(object sender, EventArgs e)
	{
	}

	private void ribbonTabItemSportClub_ExpandChange(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_sendPersonToApiTokenSource?.Cancel();
				_sendPersonToApiTokenSource?.Dispose();
				_sendPersonToApiTokenSource = null;
				_checkPersonChangesToken?.Cancel();
				_checkPersonChangesToken?.Dispose();
				_checkPersonChangesToken = null;
				_tokenSource?.Cancel();
				_tokenSource?.Dispose();
				_tokenSource = null;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_tokenReceiveTaradod?.Cancel();
				_tokenReceiveTaradod?.Dispose();
				_tokenReceiveTaradod = null;
				_tokenPersonLeave?.Cancel();
				_tokenPersonLeave?.Dispose();
				_tokenPersonLeave = null;
				_tokenOpenPort?.Cancel();
				_tokenOpenPort?.Dispose();
				_tokenOpenPort = null;
				_tokenGetLastTaradods?.Cancel();
				_tokenGetLastTaradods?.Dispose();
				_tokenGetLastTaradods = null;
				_tokenReportDevices?.Cancel();
				_tokenReportDevices?.Dispose();
				_tokenReportDevices = null;
				AttendanceRecord.Dispose();
				HysoonRealTimeProcess.Dispose();
				EbioRealTimeProcess.Dispose();
				_tokenBazsazi?.Cancel();
				_tokenBazsazi?.Dispose();
				_tokenBazsazi = null;
				_tokenNotifyMe?.Cancel();
				_tokenNotifyMe?.Dispose();
				_tokenNotifyMe = null;
				_tokenHoliday?.Cancel();
				_tokenHoliday?.Dispose();
				_tokenHoliday = null;
				LogOnPersonel.InstanceForEvents.OnStatusErorTaradod -= ShowEventMessage;
				ClsCache.GetCache(((CacheBaseForm)this).ContextGuid).WebService.PersonLeave.OnLeaveAccepted -= PersonLeave_OnLeaveAcceptedAsync;
				Components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void notifyIcon1_DoubleClick(object sender, EventArgs e)
	{
		((ToolStripItem)CMSOpenProgram).PerformClick();
	}

	private async void BtnRequestVacation_Click(object sender, EventArgs e)
	{
		try
		{
			FrmRequestedVacation frm = new FrmRequestedVacation(((CacheBaseForm)this).ContextGuid);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRequestVacation_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\FRMMain.cs", 2617);
		}
	}

	private async void TimerStartPayamak_Tick(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)ClsCache.GetCache(((CacheBaseForm)this).ContextGuid).BufferLoginState.Status != 1)
			{
				await Task.Delay(60000);
				await EntityCacheConfig.InitAsync(((CacheBaseForm)this).ContextGuid, Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid), SystemInfoClass.SystemInfo(), VersionManagment.Version, (EnConnectionType)1, connectSignalR: false);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnDepartment_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new frmDepartmentList(((CacheBaseForm)this).ContextGuid, selectMode: false)).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void timerRefreshCalendar_Tick(object sender, EventArgs e)
	{
		try
		{
			timerRefreshCalendar.Stop();
			timerRefreshCalendar.Interval = 30000;
			_tokenHoliday?.Cancel();
			_tokenHoliday = new CancellationTokenSource();
			CancellationToken token = _tokenHoliday.Token;
			await LoadHolidayForCalendarAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			timerRefreshCalendar.Start();
		}
	}

	private void DgridLastTaradods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		if (e == null)
		{
			return;
		}
		try
		{
			if (((DataGridView)DgridLastTaradods).RowCount > 0 && ((BaseCollection)((DataGridView)DgridLastTaradods).Columns).Count > 0)
			{
				int num = int.Parse(((DataGridView)DgridLastTaradods).Rows[e.RowIndex].Cells[((DataGridViewBand)DgPersonId).Index].Value.ToString());
				if (num > 0)
				{
					string? obj = ((DataGridView)DgridLastTaradods).Rows[e.RowIndex].Cells[((DataGridViewBand)DgDateCalture).Index].Value?.ToString();
					DateTime date = Calendar.EndDayOfPersianMonth(obj).Date;
					DateTime date2 = Calendar.ShamsiToMiladi(obj.Substring(0, 8) + "01").Date;
					FrmRptTaradodFull frmRptTaradodFull = new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, date2, date, num);
					((Form)frmRptTaradodFull).ShowDialog((IWin32Window)(object)this);
					((Component)(object)frmRptTaradodFull).Dispose();
					ReloadDgridLastTaradods();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnSmsCalendar_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSmsCalendarList frmSmsCalendarList = new FrmSmsCalendarList(((CacheBaseForm)this).ContextGuid, ((Control)this).Font);
			if (!BringToFront((Form)(object)frmSmsCalendarList))
			{
				await ((CacheBaseForm)frmSmsCalendarList).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private bool BringToFront(Form openedForm)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		try
		{
			foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
			{
				Form val = item;
				if (((Control)val).Name == ((Control)openedForm).Name)
				{
					if ((int)val.WindowState == 1)
					{
						val.WindowState = (FormWindowState)0;
					}
					((Control)val).BringToFront();
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	private async void BtnSendedSms_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSmsSentList frmSmsSentList = new FrmSmsSentList(((CacheBaseForm)this).ContextGuid, ((Control)this).Font);
			if (!BringToFront((Form)(object)frmSmsSentList))
			{
				await ((CacheBaseForm)frmSmsSentList).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnNotSend_Click(object sender, EventArgs e)
	{
		try
		{
			FrmSmsNotSentList frmSmsNotSentList = new FrmSmsNotSentList(((CacheBaseForm)this).ContextGuid, ((Control)this).Font);
			if (!BringToFront((Form)(object)frmSmsNotSentList))
			{
				await ((CacheBaseForm)frmSmsNotSentList).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void LogUserLogin(string username)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			SqlConnection val = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				((DbConnection)(object)val).Open();
				SqlCommand val2 = new SqlCommand("INSERT INTO UserLoginStatus (Username, LoginTime, Status) VALUES (@username, @loginTime, @status)", val);
				try
				{
					val2.Parameters.AddWithValue("@username", (object)username);
					val2.Parameters.AddWithValue("@loginTime", (object)DateTime.Now);
					val2.Parameters.AddWithValue("@status", (object)"Active");
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void LogUserLogout(string userName)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			SqlConnection val = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				((DbConnection)(object)val).Open();
				SqlCommand val2 = new SqlCommand("UPDATE UserLoginStatus SET LogoutTime = @logoutTime, Status = @status WHERE UserID in ( select top (1) userId from UserLoginStatus  where Username = @Username  order by userid desc)", val);
				try
				{
					val2.Parameters.AddWithValue("@logoutTime", (object)DateTime.Now);
					val2.Parameters.AddWithValue("@status", (object)"Inactive");
					val2.Parameters.AddWithValue("@Username", (object)userName);
					((DbCommand)(object)val2).ExecuteNonQuery();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://novinhozor.com/attendance-software/",
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private static void OnEntryWritten(object sender, EntryWrittenEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if ((int)e.Entry.EntryType == 1 && LastEventLogMessage.Trim() != e.Entry.Message.Trim())
		{
			LastEventLogMessage = e.Entry.Message.Trim();
			Task.Run(delegate
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, e.Entry.Message, $"{e.Entry.Source}  {e.Entry.TimeGenerated}", 10);
			});
		}
	}

	private void BtnShowEventLog_Click(object sender, EventArgs e)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (systemLog.EnableRaisingEvents)
		{
			systemLog.EnableRaisingEvents = false;
			BtnShowEventLog.Image = (Image)(object)Resources.AuditIconDisable;
			systemLog.EntryWritten -= new EntryWrittenEventHandler(OnEntryWritten);
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)2, "نمایش رخدادهای خطای ویندوز غیر فعال شد", "Windows eventLog", 3);
		}
		else
		{
			BtnShowEventLog.Image = (Image)(object)Resources.AuditIcon2;
			systemLog.EnableRaisingEvents = true;
			systemLog.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "نمایش رخدادهای خطای ویندوز فعال شد", "Windows eventLog", 3);
		}
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Expected O, but got Unknown
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Expected O, but got Unknown
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_112e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Expected O, but got Unknown
		//IL_13d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13dc: Expected O, but got Unknown
		//IL_159f: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Expected O, but got Unknown
		//IL_188a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1894: Expected O, but got Unknown
		//IL_191a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1924: Expected O, but got Unknown
		//IL_19aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b4: Expected O, but got Unknown
		//IL_1b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b4e: Expected O, but got Unknown
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_2235: Unknown result type (might be due to invalid IL or missing references)
		//IL_286d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2877: Expected O, but got Unknown
		//IL_2aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aae: Expected O, but got Unknown
		//IL_2beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db2: Expected O, but got Unknown
		//IL_2ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3359: Unknown result type (might be due to invalid IL or missing references)
		//IL_3632: Unknown result type (might be due to invalid IL or missing references)
		//IL_363c: Expected O, but got Unknown
		//IL_36c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cc: Expected O, but got Unknown
		//IL_3878: Unknown result type (might be due to invalid IL or missing references)
		//IL_400e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4018: Expected O, but got Unknown
		//IL_4262: Unknown result type (might be due to invalid IL or missing references)
		//IL_426c: Expected O, but got Unknown
		//IL_42f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4303: Expected O, but got Unknown
		//IL_4481: Unknown result type (might be due to invalid IL or missing references)
		//IL_448b: Expected O, but got Unknown
		//IL_44f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4502: Expected O, but got Unknown
		//IL_45a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d0: Expected O, but got Unknown
		//IL_4701: Unknown result type (might be due to invalid IL or missing references)
		//IL_470b: Expected O, but got Unknown
		//IL_477c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4786: Expected O, but got Unknown
		//IL_47af: Unknown result type (might be due to invalid IL or missing references)
		//IL_47b9: Expected O, but got Unknown
		//IL_47fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4807: Expected O, but got Unknown
		//IL_48de: Unknown result type (might be due to invalid IL or missing references)
		//IL_48e8: Expected O, but got Unknown
		//IL_490c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4981: Unknown result type (might be due to invalid IL or missing references)
		//IL_498b: Expected O, but got Unknown
		//IL_4a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a32: Expected O, but got Unknown
		//IL_4b27: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b31: Expected O, but got Unknown
		//IL_503d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5047: Expected O, but got Unknown
		//IL_5129: Unknown result type (might be due to invalid IL or missing references)
		//IL_5133: Expected O, but got Unknown
		//IL_51d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e2: Expected O, but got Unknown
		//IL_5267: Unknown result type (might be due to invalid IL or missing references)
		//IL_5271: Expected O, but got Unknown
		//IL_55e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5671: Unknown result type (might be due to invalid IL or missing references)
		//IL_571a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5794: Unknown result type (might be due to invalid IL or missing references)
		//IL_579e: Expected O, but got Unknown
		//IL_57bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_58be: Unknown result type (might be due to invalid IL or missing references)
		//IL_5942: Unknown result type (might be due to invalid IL or missing references)
		//IL_594c: Expected O, but got Unknown
		//IL_5995: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a0e: Expected O, but got Unknown
		//IL_5a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6b: Expected O, but got Unknown
		//IL_5a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a86: Expected O, but got Unknown
		//IL_5ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac1: Expected O, but got Unknown
		//IL_5b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b76: Expected O, but got Unknown
		//IL_5bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bd5: Expected O, but got Unknown
		//IL_5c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c73: Expected O, but got Unknown
		//IL_5cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd2: Expected O, but got Unknown
		//IL_5f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f5d: Expected O, but got Unknown
		//IL_5f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f73: Expected O, but got Unknown
		//IL_5f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa9: Expected O, but got Unknown
		//IL_5fc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fcd: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmMain));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		DataGridViewCellStyle val7 = new DataGridViewCellStyle();
		DataGridViewCellStyle val8 = new DataGridViewCellStyle();
		DataGridViewCellStyle val9 = new DataGridViewCellStyle();
		DataGridViewCellStyle val10 = new DataGridViewCellStyle();
		DataGridViewCellStyle val11 = new DataGridViewCellStyle();
		DataGridViewCellStyle val12 = new DataGridViewCellStyle();
		ribbonControl1 = new RibbonControl();
		contextMenuStrip1 = new ContextMenuStrip(components);
		cmsBazsaziToolStripMenuItem1 = new ToolStripMenuItem();
		cmsBackupToolStripMenuItem1 = new ToolStripMenuItem();
		cmsRestoreToolStripMenuItem1 = new ToolStripMenuItem();
		toolStripSeparator3 = new ToolStripSeparator();
		cmsShowLastTaradod = new ToolStripMenuItem();
		cmsShowReportDevice = new ToolStripMenuItem();
		cmsViewCalendar = new ToolStripMenuItem();
		ribbonPanel4 = new RibbonPanel();
		ribbonBar4 = new RibbonBar();
		BtnReadDataManage = new ButtonItem();
		BtnReadData = new ButtonItem();
		BtnPass = new ButtonItem();
		BtnSetting = new ButtonItem();
		BtnBazsazi = new ButtonItem();
		BtnBackup = new ButtonItem();
		BtnRestore = new ButtonItem();
		BtnCalc = new ButtonItem();
		BTNManagment = new ButtonItem();
		BtnLogUser = new ButtonItem();
		ribbonPanel2 = new RibbonPanel();
		ribbonBar2 = new RibbonBar();
		BtnDasteMaliyat = new ButtonItem();
		BtnSFishM = new ButtonItem();
		BtnSFishD = new ButtonItem();
		BtnFishes = new ButtonItem();
		BtnSHesab = new ButtonItem();
		BtnAgreedDeduction = new ButtonItem();
		BtnListBimeh = new ButtonItem();
		buttonItem4 = new ButtonItem();
		ribbonPanel3 = new RibbonPanel();
		ribbonBar3 = new RibbonBar();
		BtnRptTaradodFull = new ButtonItem();
		BtnRptPersonsOneDay = new ButtonItem();
		BtnRptPersonsOneMounth = new ButtonItem();
		BtnMorakhasi = new ButtonItem();
		BtnMorakhasiSaati = new ButtonItem();
		BtnTypeTaradod = new ButtonItem();
		BtnRptMorakhasi = new ButtonItem();
		BtnRequestVacation = new ButtonItem();
		ribbonPanel1 = new RibbonPanel();
		ribbonBar1 = new RibbonBar();
		BTNUsers = new ButtonItem();
		BtnDevice = new ButtonItem();
		BtnCompany = new ButtonItem();
		BtnHoliday = new ButtonItem();
		BtnShift = new ButtonItem();
		buttonItem1 = new ButtonItem();
		BtnProgrammableShift = new ButtonItem();
		BtnDefMorakhsi = new ButtonItem();
		BtnPersonel = new ButtonItem();
		BtnListM = new ButtonItem();
		BtnGharardad = new ButtonItem();
		btnTaxTable = new ButtonItem();
		BtnBaseYearsSalary = new ButtonItem();
		btnDepartment = new ButtonItem();
		ribbonPanel7 = new RibbonPanel();
		ribbonBar7 = new RibbonBar();
		BtnClub = new ButtonItem();
		BtnAttendanceSport = new ButtonItem();
		BtnDefinePackage = new ButtonItem();
		BtnReloadEnrollData = new ButtonItem();
		ribbonPanel8 = new RibbonPanel();
		ribbonBar8 = new RibbonBar();
		BtnSmsCalendar = new ButtonItem();
		BtnSendedSms = new ButtonItem();
		BtnNotSend = new ButtonItem();
		ribbonPanel5 = new RibbonPanel();
		ribbonBar5 = new RibbonBar();
		Btnsoftware = new ButtonItem();
		btnactivation = new ButtonItem();
		BtnAboutUs = new ButtonItem();
		BtnCallUs = new ButtonItem();
		BtnTutorialVideo = new ButtonItem();
		BtnSuggestion = new ButtonItem();
		ribbonPanel6 = new RibbonPanel();
		ribbonBar6 = new RibbonBar();
		BtnRPTsaz = new ButtonItem();
		BtnRptAllTaradod = new ButtonItem();
		btnRptNotify = new ButtonItem();
		btnVacations = new ButtonItem();
		BtnTajmiVacations = new ButtonItem();
		BtnEndedGharardad = new ButtonItem();
		ribbonTabItem1 = new RibbonTabItem();
		ribbonTabItem2 = new RibbonTabItem();
		ribbonTabItem3 = new RibbonTabItem();
		ribbonTabItem4 = new RibbonTabItem();
		ribbonTabItem5 = new RibbonTabItem();
		ribbonTabItem6 = new RibbonTabItem();
		ribbonTabItemSportClub = new RibbonTabItem();
		ribbonTabItemSMS = new RibbonTabItem();
		BtnLogOnManage = new ButtonItem();
		BtnLogOnPersonel = new ButtonItem();
		notifyIcon1 = new NotifyIcon(components);
		NotifyContextMenue = new ContextMenuStrip(components);
		CMSOpenProgram = new ToolStripMenuItem();
		toolStripSeparator2 = new ToolStripSeparator();
		CMSAboutUS = new ToolStripMenuItem();
		toolStripSeparator1 = new ToolStripSeparator();
		CMSExitProgram = new ToolStripMenuItem();
		timerStopNotification = new Timer(components);
		timerSportClub = new Timer(components);
		buttonItem3 = new ButtonItem();
		buttonItem2 = new ButtonItem();
		btnNotifyMe = new ButtonX();
		timerReadNotify = new Timer(components);
		timer1 = new Timer(components);
		timerLock = new Timer(components);
		LanguageLable = new LinkLabel();
		monthCalendarX1 = new MonthCalendarX();
		TbxMonasebat = new TextBox();
		ucCacheStatus1 = new UcCacheStatus();
		ImgBazsazi = new PictureBox();
		ImgRecovery = new PictureBox();
		DgridReportDevices = new DataGridView();
		termTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		serialNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		LogType = new DataGridViewTextBoxColumn();
		transactionIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		logTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		userIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		ModifiedDate = new DataGridViewTextBoxColumn();
		deviceEventsEntityBindingSource = new BindingSource(components);
		DgridLastTaradods = new DataGridViewX();
		DgDatetimeLogM = new DataGridViewTextBoxColumn();
		DgDeviceName = new DataGridViewTextBoxColumn();
		DgPersonNo = new DataGridViewTextBoxColumn();
		DgPersonFullName = new DataGridViewTextBoxColumn();
		DgDateCalture = new DataGridViewTextBoxColumn();
		DgGharardadEndDate = new DataGridViewTextBoxColumn();
		DgGharardadStatusName = new DataGridViewTextBoxColumn();
		DgTrafficTypeName = new DataGridViewTextBoxColumn();
		DgPersonId = new DataGridViewTextBoxColumn();
		logOnPersonelTopLastTaradodsBindingSource = new BindingSource(components);
		ucLkExpireDisplay1 = new UcLkExpireDisplay();
		ucNotificationViewer1 = new UcNotificationViewer();
		ucStatuse1 = new UcStatuse();
		ucMain1 = new UCMain();
		timerStartPayamak = new Timer(components);
		timerRefreshCalendar = new Timer(components);
		ucPayamakServiceStatus1 = new UcPayamakServiceStatus();
		linkLabelGetOtherVeraion = new LinkLabel();
		BtnShowEventLog = new ButtonX();
		BtnPayamak = new ButtonX();
		BtnSignalR = new ButtonX();
		pictureBoxLoading = new PictureBox();
		ribbonControl1.SuspendLayout();
		((Control)contextMenuStrip1).SuspendLayout();
		((Control)ribbonPanel4).SuspendLayout();
		((Control)ribbonPanel2).SuspendLayout();
		((Control)ribbonPanel3).SuspendLayout();
		((Control)ribbonPanel1).SuspendLayout();
		((Control)ribbonPanel7).SuspendLayout();
		((Control)ribbonPanel8).SuspendLayout();
		((Control)ribbonPanel5).SuspendLayout();
		((Control)ribbonPanel6).SuspendLayout();
		((Control)NotifyContextMenue).SuspendLayout();
		((ISupportInitialize)ImgBazsazi).BeginInit();
		((ISupportInitialize)ImgRecovery).BeginInit();
		((ISupportInitialize)DgridReportDevices).BeginInit();
		((ISupportInitialize)deviceEventsEntityBindingSource).BeginInit();
		((ISupportInitialize)DgridLastTaradods).BeginInit();
		((ISupportInitialize)logOnPersonelTopLastTaradodsBindingSource).BeginInit();
		((ISupportInitialize)pictureBoxLoading).BeginInit();
		((Control)this).SuspendLayout();
		((Control)ribbonControl1).Anchor = (AnchorStyles)13;
		ribbonControl1.AutoExpand = false;
		ribbonControl1.AutoKeyboardExpand = false;
		ribbonControl1.BackgroundStyle.CornerType = (eCornerType)1;
		ribbonControl1.CaptionVisible = true;
		((Control)ribbonControl1).ContextMenuStrip = contextMenuStrip1;
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel4);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel2);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel3);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel1);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel7);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel8);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel5);
		((Control)ribbonControl1).Controls.Add((Control)(object)ribbonPanel6);
		ribbonControl1.Items.AddRange((BaseItem[])(object)new BaseItem[8]
		{
			(BaseItem)ribbonTabItem1,
			(BaseItem)ribbonTabItem2,
			(BaseItem)ribbonTabItem3,
			(BaseItem)ribbonTabItem4,
			(BaseItem)ribbonTabItem5,
			(BaseItem)ribbonTabItem6,
			(BaseItem)ribbonTabItemSportClub,
			(BaseItem)ribbonTabItemSMS
		});
		ribbonControl1.KeyTipsFont = new Font("Tahoma", 7f);
		((Control)ribbonControl1).Location = new Point(0, -28);
		((Control)ribbonControl1).Name = "ribbonControl1";
		((Control)ribbonControl1).Padding = new Padding(0, 0, 0, 3);
		ribbonControl1.QuickAccessToolbarBelowRibbonHeight = 1;
		((Control)ribbonControl1).RightToLeft = (RightToLeft)1;
		((Control)ribbonControl1).Size = new Size(1466, 161);
		ribbonControl1.Style = (eDotNetBarStyle)9;
		ribbonControl1.SystemText.MaximizeRibbonText = "بزرگنمایی نوار منو";
		ribbonControl1.SystemText.MinimizeRibbonText = "کوچک نمایی نوار منو";
		ribbonControl1.SystemText.QatAddItemText = "افزودن به منوی دسترسی سریع";
		ribbonControl1.SystemText.QatCustomizeMenuLabel = "تنظیم منوی دسترسی سریع";
		ribbonControl1.SystemText.QatCustomizeText = "تنظیم منوی دسترسی سریع ...";
		ribbonControl1.SystemText.QatDialogAddButton = "افزودن>>";
		ribbonControl1.SystemText.QatDialogCancelButton = "لغو";
		ribbonControl1.SystemText.QatDialogCaption = "تنظیم منوی دسترسی سریع";
		ribbonControl1.SystemText.QatDialogCategoriesLabel = "انتخاب منو از:";
		ribbonControl1.SystemText.QatDialogOkButton = "بسیار خوب";
		ribbonControl1.SystemText.QatDialogPlacementCheckbox = "";
		ribbonControl1.SystemText.QatDialogRemoveButton = "حذف";
		ribbonControl1.SystemText.QatPlaceAboveRibbonText = "";
		ribbonControl1.SystemText.QatPlaceBelowRibbonText = "";
		ribbonControl1.SystemText.QatRemoveItemText = "";
		ribbonControl1.TabGroupHeight = 14;
		((Control)ribbonControl1).TabIndex = 4;
		((Control)ribbonControl1).Text = "ribbonControl1";
		((Control)ribbonControl1).Click += ribbonControl1_Click;
		((ToolStrip)contextMenuStrip1).BackColor = Color.AliceBlue;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[7]
		{
			(ToolStripItem)cmsBazsaziToolStripMenuItem1,
			(ToolStripItem)cmsBackupToolStripMenuItem1,
			(ToolStripItem)cmsRestoreToolStripMenuItem1,
			(ToolStripItem)toolStripSeparator3,
			(ToolStripItem)cmsShowLastTaradod,
			(ToolStripItem)cmsShowReportDevice,
			(ToolStripItem)cmsViewCalendar
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((ToolStripDropDownMenu)contextMenuStrip1).ShowCheckMargin = true;
		((Control)contextMenuStrip1).Size = new Size(314, 142);
		((ToolStripItem)cmsBazsaziToolStripMenuItem1).Image = (Image)(object)Resources.reb1;
		((ToolStripItem)cmsBazsaziToolStripMenuItem1).Name = "cmsBazsaziToolStripMenuItem1";
		((ToolStripItem)cmsBazsaziToolStripMenuItem1).Size = new Size(313, 22);
		((ToolStripItem)cmsBazsaziToolStripMenuItem1).Text = "[بازسازی اطلاعات]";
		((ToolStripItem)cmsBazsaziToolStripMenuItem1).Click += BtnBazsazi_Click;
		((ToolStripItem)cmsBackupToolStripMenuItem1).Image = (Image)(object)Resources.backup;
		((ToolStripItem)cmsBackupToolStripMenuItem1).Name = "cmsBackupToolStripMenuItem1";
		((ToolStripItem)cmsBackupToolStripMenuItem1).Size = new Size(313, 22);
		((ToolStripItem)cmsBackupToolStripMenuItem1).Text = "[پشتیبان گیری اطلاعات]";
		((ToolStripItem)cmsBackupToolStripMenuItem1).Click += BtnBackup_Click;
		((ToolStripItem)cmsRestoreToolStripMenuItem1).Image = (Image)(object)Resources.media_playlist_refresh;
		((ToolStripItem)cmsRestoreToolStripMenuItem1).Name = "cmsRestoreToolStripMenuItem1";
		cmsRestoreToolStripMenuItem1.ShowShortcutKeys = false;
		((ToolStripItem)cmsRestoreToolStripMenuItem1).Size = new Size(313, 22);
		((ToolStripItem)cmsRestoreToolStripMenuItem1).Text = "[بازگردانی اطلاعات]";
		((ToolStripItem)cmsRestoreToolStripMenuItem1).Click += BtnRestore_Click;
		((ToolStripItem)toolStripSeparator3).Name = "toolStripSeparator3";
		((ToolStripItem)toolStripSeparator3).Size = new Size(310, 6);
		((ToolStripItem)cmsShowLastTaradod).BackColor = Color.AliceBlue;
		cmsShowLastTaradod.CheckOnClick = true;
		((ToolStripItem)cmsShowLastTaradod).Image = (Image)(object)Resources.clock_in_clock_out_blog;
		((ToolStripItem)cmsShowLastTaradod).Name = "cmsShowLastTaradod";
		((ToolStripItem)cmsShowLastTaradod).Size = new Size(313, 22);
		((ToolStripItem)cmsShowLastTaradod).Text = "نمایش جدول آخرین ترددها";
		cmsShowReportDevice.CheckOnClick = true;
		((ToolStripItem)cmsShowReportDevice).Image = (Image)(object)Resources.wait_Small_JPG;
		((ToolStripItem)cmsShowReportDevice).Name = "cmsShowReportDevice";
		((ToolStripItem)cmsShowReportDevice).Size = new Size(313, 22);
		((ToolStripItem)cmsShowReportDevice).Text = "نمایش جدول وضعیت ارتباط آنلاین دستگاه ها";
		cmsViewCalendar.CheckOnClick = true;
		((ToolStripItem)cmsViewCalendar).Image = (Image)(object)Resources.calendar_today;
		((ToolStripItem)cmsViewCalendar).Name = "cmsViewCalendar";
		cmsViewCalendar.ShortcutKeys = (Keys)131156;
		((ToolStripItem)cmsViewCalendar).Size = new Size(313, 22);
		((ToolStripItem)cmsViewCalendar).Text = "نمایش تقویم";
		cmsViewCalendar.CheckedChanged += cmsViewCalendar_CheckedChanged;
		((PanelControl)ribbonPanel4).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel4).Controls.Add((Control)(object)ribbonBar4);
		((Control)ribbonPanel4).Dock = (DockStyle)5;
		ribbonPanel4.Location = new Point(0, 56);
		((Control)ribbonPanel4).Name = "ribbonPanel4";
		((Control)ribbonPanel4).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel4.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel4).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel4).TabIndex = 4;
		ribbonBar4.AutoOverflowEnabled = true;
		ribbonBar4.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar4).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar4).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar4).Dock = (DockStyle)5;
		((ItemControl)ribbonBar4).DragDropSupport = true;
		ribbonBar4.Items.AddRange((BaseItem[])(object)new BaseItem[10]
		{
			(BaseItem)BtnReadDataManage,
			(BaseItem)BtnReadData,
			(BaseItem)BtnPass,
			(BaseItem)BtnSetting,
			(BaseItem)BtnBazsazi,
			(BaseItem)BtnBackup,
			(BaseItem)BtnRestore,
			(BaseItem)BtnCalc,
			(BaseItem)BTNManagment,
			(BaseItem)BtnLogUser
		});
		((Control)ribbonBar4).Location = new Point(3, 0);
		((Control)ribbonBar4).Name = "ribbonBar4";
		((Control)ribbonBar4).Size = new Size(1460, 99);
		ribbonBar4.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar4).TabIndex = 2;
		((Control)ribbonBar4).Text = "ribbonBar4";
		ribbonBar4.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar4.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar4.TitleVisible = false;
		((BaseItem)BtnReadDataManage).Name = "BtnReadDataManage";
		((BaseItem)BtnReadData).AutoCollapseOnClick = false;
		((BaseItem)BtnReadData).CanCustomize = false;
		BtnReadData.Image = (Image)(object)Resources.download_file;
		BtnReadData.ImagePaddingHorizontal = 45;
		BtnReadData.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnReadData).Name = "BtnReadData";
		BtnReadData.SubItemsExpandWidth = 14;
		((BaseItem)BtnReadData).Text = "[دریافت اطلاعات تردد ]";
		((BaseItem)BtnReadData).Click += BtnReadData_Click;
		((BaseItem)BtnPass).AutoCollapseOnClick = false;
		((BaseItem)BtnPass).CanCustomize = false;
		BtnPass.Image = (Image)componentResourceManager.GetObject("BtnPass.Image");
		BtnPass.ImagePaddingHorizontal = 55;
		BtnPass.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnPass).Name = "BtnPass";
		BtnPass.SubItemsExpandWidth = 14;
		((BaseItem)BtnPass).Text = "[تغییر رمز کاربر]";
		((BaseItem)BtnPass).Click += BtnPass_Click;
		((BaseItem)BtnSetting).AutoCollapseOnClick = false;
		((BaseItem)BtnSetting).CanCustomize = false;
		BtnSetting.Image = (Image)(object)Resources.preferences_desktop_personal2;
		BtnSetting.ImagePaddingHorizontal = 55;
		BtnSetting.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSetting).Name = "BtnSetting";
		BtnSetting.SubItemsExpandWidth = 14;
		((BaseItem)BtnSetting).Text = "[تنظیمات نرم افزار]";
		((BaseItem)BtnSetting).Click += BtnSetting_Click;
		((BaseItem)BtnBazsazi).AutoCollapseOnClick = false;
		((BaseItem)BtnBazsazi).CanCustomize = false;
		BtnBazsazi.Image = (Image)(object)Resources.reb1;
		BtnBazsazi.ImagePaddingHorizontal = 55;
		BtnBazsazi.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnBazsazi).Name = "BtnBazsazi";
		BtnBazsazi.SubItemsExpandWidth = 14;
		((BaseItem)BtnBazsazi).Text = "[بازسازی اطلاعات]";
		((BaseItem)BtnBazsazi).Click += BtnBazsazi_Click;
		((BaseItem)BtnBackup).AutoCollapseOnClick = false;
		((BaseItem)BtnBackup).CanCustomize = false;
		BtnBackup.Image = (Image)(object)Resources.backup;
		BtnBackup.ImagePaddingHorizontal = 55;
		BtnBackup.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnBackup).Name = "BtnBackup";
		BtnBackup.SubItemsExpandWidth = 14;
		((BaseItem)BtnBackup).Text = "[پشتیبان گیری اطلاعات]";
		((BaseItem)BtnBackup).Click += BtnBackup_Click;
		((BaseItem)BtnRestore).AutoCollapseOnClick = false;
		((BaseItem)BtnRestore).CanCustomize = false;
		BtnRestore.Image = (Image)(object)Resources.media_playlist_refresh;
		BtnRestore.ImagePaddingHorizontal = 55;
		BtnRestore.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRestore).Name = "BtnRestore";
		BtnRestore.SubItemsExpandWidth = 14;
		((BaseItem)BtnRestore).Text = "[بازگردانی اطلاعات]";
		((BaseItem)BtnRestore).Click += BtnRestore_Click;
		((BaseItem)BtnCalc).AutoCollapseOnClick = false;
		((BaseItem)BtnCalc).CanCustomize = false;
		BtnCalc.Image = (Image)componentResourceManager.GetObject("BtnCalc.Image");
		BtnCalc.ImagePaddingHorizontal = 55;
		BtnCalc.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnCalc).Name = "BtnCalc";
		BtnCalc.SubItemsExpandWidth = 14;
		((BaseItem)BtnCalc).Text = "[ماشین حساب]";
		((BaseItem)BtnCalc).Click += BtnCalc_Click;
		((BaseItem)BTNManagment).AutoCollapseOnClick = false;
		((BaseItem)BTNManagment).CanCustomize = false;
		BTNManagment.Image = (Image)(object)Resources.business_people_01;
		BTNManagment.ImagePaddingHorizontal = 55;
		BTNManagment.ImagePosition = (eImagePosition)2;
		((BaseItem)BTNManagment).Name = "BTNManagment";
		BTNManagment.SubItemsExpandWidth = 14;
		((BaseItem)BTNManagment).Text = "[مدیریت ویژه]";
		((BaseItem)BTNManagment).Click += BtnManagment_Click;
		((BaseItem)BtnLogUser).AutoCollapseOnClick = false;
		((BaseItem)BtnLogUser).CanCustomize = false;
		BtnLogUser.Image = (Image)(object)Resources.AuditIcon2;
		BtnLogUser.ImagePaddingHorizontal = 80;
		BtnLogUser.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnLogUser).Name = "BtnLogUser";
		BtnLogUser.SubItemsExpandWidth = 14;
		((BaseItem)BtnLogUser).Text = "[لاگ کاربران]";
		((BaseItem)BtnLogUser).Click += BtnLogUser_Click;
		((PanelControl)ribbonPanel2).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel2).Controls.Add((Control)(object)ribbonBar2);
		((Control)ribbonPanel2).Dock = (DockStyle)5;
		ribbonPanel2.Location = new Point(0, 56);
		((Control)ribbonPanel2).Name = "ribbonPanel2";
		((Control)ribbonPanel2).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel2.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel2).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel2).TabIndex = 5;
		ribbonPanel2.Visible = false;
		ribbonBar2.AutoOverflowEnabled = true;
		ribbonBar2.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar2).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar2).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar2).Dock = (DockStyle)5;
		((ItemControl)ribbonBar2).DragDropSupport = true;
		ribbonBar2.Items.AddRange((BaseItem[])(object)new BaseItem[8]
		{
			(BaseItem)BtnDasteMaliyat,
			(BaseItem)BtnSFishM,
			(BaseItem)BtnSFishD,
			(BaseItem)BtnFishes,
			(BaseItem)BtnSHesab,
			(BaseItem)BtnAgreedDeduction,
			(BaseItem)BtnListBimeh,
			(BaseItem)buttonItem4
		});
		((Control)ribbonBar2).Location = new Point(3, 0);
		((Control)ribbonBar2).Name = "ribbonBar2";
		((Control)ribbonBar2).Size = new Size(1460, 99);
		ribbonBar2.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar2).TabIndex = 3;
		((Control)ribbonBar2).Text = "ribbonBar2";
		ribbonBar2.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar2.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar2.TitleVisible = false;
		((BaseItem)BtnDasteMaliyat).AutoCollapseOnClick = false;
		((BaseItem)BtnDasteMaliyat).CanCustomize = false;
		BtnDasteMaliyat.Image = (Image)(object)Resources.lightbrown_books;
		BtnDasteMaliyat.ImagePaddingHorizontal = 55;
		BtnDasteMaliyat.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnDasteMaliyat).Name = "BtnDasteMaliyat";
		BtnDasteMaliyat.SubItemsExpandWidth = 14;
		((BaseItem)BtnDasteMaliyat).Text = "[دسته های مالیاتی]";
		((BaseItem)BtnDasteMaliyat).Visible = false;
		((BaseItem)BtnSFishM).AutoCollapseOnClick = false;
		((BaseItem)BtnSFishM).CanCustomize = false;
		BtnSFishM.Image = (Image)componentResourceManager.GetObject("BtnSFishM.Image");
		BtnSFishM.ImagePaddingHorizontal = 80;
		BtnSFishM.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSFishM).Name = "BtnSFishM";
		BtnSFishM.SubItemsExpandWidth = 14;
		((BaseItem)BtnSFishM).Text = "[صدور فیش حقوقی]";
		((BaseItem)BtnSFishM).Visible = false;
		((BaseItem)BtnSFishM).Click += BtnSFishM_Click;
		((BaseItem)BtnSFishD).AutoCollapseOnClick = false;
		((BaseItem)BtnSFishD).CanCustomize = false;
		BtnSFishD.Image = (Image)componentResourceManager.GetObject("BtnSFishD.Image");
		BtnSFishD.ImagePaddingHorizontal = 80;
		BtnSFishD.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSFishD).Name = "BtnSFishD";
		BtnSFishD.SubItemsExpandWidth = 14;
		((BaseItem)BtnSFishD).Text = "[صدور فیش حقوقی]";
		((BaseItem)BtnSFishD).Click += BtnSFishD_Click;
		((BaseItem)BtnFishes).AutoCollapseOnClick = false;
		((BaseItem)BtnFishes).CanCustomize = false;
		BtnFishes.Image = (Image)componentResourceManager.GetObject("BtnFishes.Image");
		BtnFishes.ImagePaddingHorizontal = 80;
		BtnFishes.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnFishes).Name = "BtnFishes";
		BtnFishes.SubItemsExpandWidth = 14;
		((BaseItem)BtnFishes).Text = "[فیش های صادر شده]";
		((BaseItem)BtnFishes).Click += BtnFishes_Click;
		((BaseItem)BtnSHesab).AutoCollapseOnClick = false;
		((BaseItem)BtnSHesab).CanCustomize = false;
		BtnSHesab.Image = (Image)componentResourceManager.GetObject("BtnSHesab.Image");
		BtnSHesab.ImagePaddingHorizontal = 80;
		BtnSHesab.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSHesab).Name = "BtnSHesab";
		BtnSHesab.SubItemsExpandWidth = 14;
		((BaseItem)BtnSHesab).Text = "[اسناد حسابداری حقوق]";
		((BaseItem)BtnSHesab).Click += BtnSHesab_Click;
		((BaseItem)BtnAgreedDeduction).AutoCollapseOnClick = false;
		((BaseItem)BtnAgreedDeduction).CanCustomize = false;
		BtnAgreedDeduction.Image = (Image)(object)Resources.deducation2;
		BtnAgreedDeduction.ImagePaddingHorizontal = 80;
		BtnAgreedDeduction.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnAgreedDeduction).Name = "BtnAgreedDeduction";
		BtnAgreedDeduction.SubItemsExpandWidth = 14;
		((BaseItem)BtnAgreedDeduction).Text = "[کسورات توافقی]";
		((BaseItem)BtnAgreedDeduction).Click += BtnAgreedDeduction_Click;
		((BaseItem)BtnListBimeh).AutoCollapseOnClick = false;
		((BaseItem)BtnListBimeh).CanCustomize = false;
		BtnListBimeh.Image = (Image)(object)Resources.insurance;
		BtnListBimeh.ImagePaddingHorizontal = 45;
		BtnListBimeh.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnListBimeh).Name = "BtnListBimeh";
		BtnListBimeh.SubItemsExpandWidth = 14;
		((BaseItem)BtnListBimeh).Text = "[لیست بیمه ها]";
		((BaseItem)BtnListBimeh).Click += BtnListBimeh_Click;
		((BaseItem)buttonItem4).AutoCollapseOnClick = false;
		((BaseItem)buttonItem4).CanCustomize = false;
		buttonItem4.Image = (Image)componentResourceManager.GetObject("buttonItem4.Image");
		buttonItem4.ImagePaddingHorizontal = 55;
		buttonItem4.ImagePosition = (eImagePosition)2;
		((BaseItem)buttonItem4).Name = "buttonItem4";
		buttonItem4.SubItemsExpandWidth = 14;
		((BaseItem)buttonItem4).Text = "[تسویه حساب]";
		((BaseItem)buttonItem4).Click += BtnCheckOut_Click;
		((PanelControl)ribbonPanel3).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel3).Controls.Add((Control)(object)ribbonBar3);
		((Control)ribbonPanel3).Dock = (DockStyle)5;
		ribbonPanel3.Location = new Point(0, 56);
		((Control)ribbonPanel3).Name = "ribbonPanel3";
		((Control)ribbonPanel3).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel3.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel3).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel3).TabIndex = 3;
		ribbonPanel3.Visible = false;
		ribbonBar3.AutoOverflowEnabled = true;
		ribbonBar3.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar3).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar3).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar3).Dock = (DockStyle)5;
		((ItemControl)ribbonBar3).DragDropSupport = true;
		ribbonBar3.Items.AddRange((BaseItem[])(object)new BaseItem[8]
		{
			(BaseItem)BtnRptTaradodFull,
			(BaseItem)BtnRptPersonsOneDay,
			(BaseItem)BtnRptPersonsOneMounth,
			(BaseItem)BtnMorakhasi,
			(BaseItem)BtnMorakhasiSaati,
			(BaseItem)BtnTypeTaradod,
			(BaseItem)BtnRptMorakhasi,
			(BaseItem)BtnRequestVacation
		});
		((Control)ribbonBar3).Location = new Point(3, 0);
		((Control)ribbonBar3).Name = "ribbonBar3";
		((Control)ribbonBar3).Size = new Size(1460, 99);
		ribbonBar3.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar3).TabIndex = 2;
		((Control)ribbonBar3).Text = "ribbonBar3";
		ribbonBar3.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar3.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar3.TitleVisible = false;
		((BaseItem)BtnRptTaradodFull).AutoCollapseOnClick = false;
		((BaseItem)BtnRptTaradodFull).CanCustomize = false;
		BtnRptTaradodFull.Image = (Image)(object)Resources.Enroll;
		BtnRptTaradodFull.ImagePaddingHorizontal = 60;
		BtnRptTaradodFull.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRptTaradodFull).Name = "BtnRptTaradodFull";
		BtnRptTaradodFull.SubItemsExpandWidth = 14;
		((BaseItem)BtnRptTaradodFull).Text = "[گزارش کامل تردد ]";
		((BaseItem)BtnRptTaradodFull).Click += RptTaradodFull_Click;
		((BaseItem)BtnRptPersonsOneDay).AutoCollapseOnClick = false;
		((BaseItem)BtnRptPersonsOneDay).CanCustomize = false;
		BtnRptPersonsOneDay.Image = (Image)(object)Resources.RptAllPersonel;
		BtnRptPersonsOneDay.ImagePaddingHorizontal = 80;
		BtnRptPersonsOneDay.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRptPersonsOneDay).Name = "BtnRptPersonsOneDay";
		BtnRptPersonsOneDay.SubItemsExpandWidth = 14;
		((BaseItem)BtnRptPersonsOneDay).Text = "[گزارش وضعیت پرسنل]";
		((BaseItem)BtnRptPersonsOneDay).Click += BtnRptPersonsOneDay_Click;
		((BaseItem)BtnRptPersonsOneMounth).AutoCollapseOnClick = false;
		((BaseItem)BtnRptPersonsOneMounth).CanCustomize = false;
		BtnRptPersonsOneMounth.Image = (Image)(object)Resources.tajmitaradod;
		BtnRptPersonsOneMounth.ImagePaddingHorizontal = 80;
		BtnRptPersonsOneMounth.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRptPersonsOneMounth).Name = "BtnRptPersonsOneMounth";
		BtnRptPersonsOneMounth.SubItemsExpandWidth = 14;
		((BaseItem)BtnRptPersonsOneMounth).Text = "[گزارش تجمیع تردد]";
		((BaseItem)BtnRptPersonsOneMounth).Click += BtnRptPersonsOneMounth_Click;
		((BaseItem)BtnMorakhasi).AutoCollapseOnClick = false;
		((BaseItem)BtnMorakhasi).CanCustomize = false;
		BtnMorakhasi.Image = (Image)(object)Resources.user_student;
		BtnMorakhasi.ImagePaddingHorizontal = 60;
		BtnMorakhasi.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnMorakhasi).Name = "BtnMorakhasi";
		BtnMorakhasi.SubItemsExpandWidth = 14;
		((BaseItem)BtnMorakhasi).Text = "[مرخصی روزانه]";
		((BaseItem)BtnMorakhasi).Click += BtnMorakhasi_Click;
		((BaseItem)BtnMorakhasiSaati).AutoCollapseOnClick = false;
		((BaseItem)BtnMorakhasiSaati).CanCustomize = false;
		BtnMorakhasiSaati.Image = (Image)(object)Resources.clock4;
		BtnMorakhasiSaati.ImagePaddingHorizontal = 60;
		BtnMorakhasiSaati.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnMorakhasiSaati).Name = "BtnMorakhasiSaati";
		BtnMorakhasiSaati.SubItemsExpandWidth = 14;
		((BaseItem)BtnMorakhasiSaati).Text = "[مرخصی ساعتی]";
		((BaseItem)BtnMorakhasiSaati).Visible = false;
		((BaseItem)BtnMorakhasiSaati).Click += BtnMorakhasiSaati_Click;
		((BaseItem)BtnTypeTaradod).AutoCollapseOnClick = false;
		((BaseItem)BtnTypeTaradod).CanCustomize = false;
		BtnTypeTaradod.Image = (Image)(object)Resources.gnome_logout;
		BtnTypeTaradod.ImagePaddingHorizontal = 60;
		BtnTypeTaradod.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnTypeTaradod).Name = "BtnTypeTaradod";
		BtnTypeTaradod.SplitButton = true;
		BtnTypeTaradod.SubItemsExpandWidth = 14;
		((BaseItem)BtnTypeTaradod).Text = "[نوع تردد]";
		((BaseItem)BtnTypeTaradod).Click += BtnTypeTaradod_Click;
		((BaseItem)BtnRptMorakhasi).AutoCollapseOnClick = false;
		((BaseItem)BtnRptMorakhasi).CanCustomize = false;
		BtnRptMorakhasi.Image = (Image)(object)Resources.vacation1;
		BtnRptMorakhasi.ImagePaddingHorizontal = 50;
		BtnRptMorakhasi.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRptMorakhasi).Name = "BtnRptMorakhasi";
		((BaseItem)BtnRptMorakhasi).Text = "[گزارش مرخصی]";
		((BaseItem)BtnRptMorakhasi).Click += BtnRptMorakhasi_Click;
		((BaseItem)BtnRequestVacation).AutoCollapseOnClick = false;
		((BaseItem)BtnRequestVacation).CanCustomize = false;
		BtnRequestVacation.Image = (Image)(object)Resources.vacation1;
		BtnRequestVacation.ImagePaddingHorizontal = 50;
		BtnRequestVacation.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRequestVacation).Name = "BtnRequestVacation";
		((BaseItem)BtnRequestVacation).Text = "درخواست مرخصی ها";
		((BaseItem)BtnRequestVacation).Click += BtnRequestVacation_Click;
		((PanelControl)ribbonPanel1).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel1).Controls.Add((Control)(object)ribbonBar1);
		((Control)ribbonPanel1).Dock = (DockStyle)5;
		ribbonPanel1.Location = new Point(0, 56);
		((Control)ribbonPanel1).Name = "ribbonPanel1";
		((Control)ribbonPanel1).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel1.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel1).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel1).TabIndex = 1;
		ribbonPanel1.Visible = false;
		ribbonBar1.AutoOverflowEnabled = true;
		ribbonBar1.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar1).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar1).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar1).Dock = (DockStyle)5;
		((ItemControl)ribbonBar1).DragDropSupport = true;
		ribbonBar1.Items.AddRange((BaseItem[])(object)new BaseItem[13]
		{
			(BaseItem)BTNUsers,
			(BaseItem)BtnDevice,
			(BaseItem)BtnCompany,
			(BaseItem)BtnHoliday,
			(BaseItem)BtnShift,
			(BaseItem)BtnProgrammableShift,
			(BaseItem)BtnDefMorakhsi,
			(BaseItem)BtnPersonel,
			(BaseItem)BtnListM,
			(BaseItem)BtnGharardad,
			(BaseItem)btnTaxTable,
			(BaseItem)BtnBaseYearsSalary,
			(BaseItem)btnDepartment
		});
		((Control)ribbonBar1).Location = new Point(3, 0);
		((Control)ribbonBar1).Name = "ribbonBar1";
		((Control)ribbonBar1).Size = new Size(1460, 99);
		ribbonBar1.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar1).TabIndex = 0;
		((Control)ribbonBar1).Text = "ribbonBar1";
		ribbonBar1.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar1.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar1.TitleVisible = false;
		((BaseItem)BTNUsers).AutoCollapseOnClick = false;
		((BaseItem)BTNUsers).CanCustomize = false;
		BTNUsers.Image = (Image)(object)Resources.User1;
		BTNUsers.ImagePaddingHorizontal = 35;
		BTNUsers.ImagePosition = (eImagePosition)2;
		((BaseItem)BTNUsers).Name = "BTNUsers";
		((BaseItem)BTNUsers).ShowSubItems = false;
		BTNUsers.SubItemsExpandWidth = 14;
		((BaseItem)BTNUsers).Text = "[کاربران]";
		((BaseItem)BTNUsers).Click += BTNUsers_Click;
		((BaseItem)BtnDevice).AutoCollapseOnClick = false;
		((BaseItem)BtnDevice).CanCustomize = false;
		BtnDevice.Image = (Image)(object)Resources.hozor;
		BtnDevice.ImagePaddingHorizontal = 35;
		BtnDevice.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnDevice).Name = "BtnDevice";
		BtnDevice.SubItemsExpandWidth = 14;
		((BaseItem)BtnDevice).Text = "[دستگاه]";
		((BaseItem)BtnDevice).Click += BtnDevice_Click;
		((BaseItem)BtnCompany).AutoCollapseOnClick = false;
		((BaseItem)BtnCompany).CanCustomize = false;
		BtnCompany.Image = (Image)(object)Resources.hotel;
		BtnCompany.ImagePaddingHorizontal = 35;
		BtnCompany.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnCompany).Name = "BtnCompany";
		BtnCompany.SubItemsExpandWidth = 14;
		((BaseItem)BtnCompany).Text = "[شرکت]";
		((BaseItem)BtnCompany).Click += BtnCompany_Click;
		((BaseItem)BtnHoliday).AutoCollapseOnClick = false;
		((BaseItem)BtnHoliday).CanCustomize = false;
		BtnHoliday.Image = (Image)(object)Resources.calendar_today;
		BtnHoliday.ImagePaddingHorizontal = 50;
		BtnHoliday.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnHoliday).Name = "BtnHoliday";
		BtnHoliday.SubItemsExpandWidth = 14;
		((BaseItem)BtnHoliday).Text = "[تقویم]";
		((BaseItem)BtnHoliday).Click += BtnHoliday_Click;
		((BaseItem)BtnShift).AutoCollapseOnClick = false;
		((BaseItem)BtnShift).CanCustomize = false;
		BtnShift.Image = (Image)(object)Resources.FunClock;
		BtnShift.ImagePaddingHorizontal = 50;
		BtnShift.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnShift).Name = "BtnShift";
		((BaseItem)BtnShift).SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)buttonItem1 });
		BtnShift.SubItemsExpandWidth = 14;
		((BaseItem)BtnShift).Text = "[شیفت]";
		((BaseItem)BtnShift).Click += BtnShift_Click;
		buttonItem1.Image = (Image)(object)Resources.FunClock;
		buttonItem1.ImagePaddingHorizontal = 50;
		buttonItem1.ImagePosition = (eImagePosition)2;
		((BaseItem)buttonItem1).Name = "buttonItem1";
		buttonItem1.SubItemsExpandWidth = 14;
		((BaseItem)buttonItem1).Text = "[شیفت]";
		((BaseItem)BtnProgrammableShift).AutoCollapseOnClick = false;
		((BaseItem)BtnProgrammableShift).CanCustomize = false;
		BtnProgrammableShift.Image = (Image)(object)Resources.ProgrammableShif2t;
		BtnProgrammableShift.ImagePaddingHorizontal = 50;
		BtnProgrammableShift.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnProgrammableShift).Name = "BtnProgrammableShift";
		BtnProgrammableShift.SubItemsExpandWidth = 14;
		((BaseItem)BtnProgrammableShift).Text = "[شیفت برنامه ریزی]";
		((BaseItem)BtnProgrammableShift).Click += BtnProgrammableShift_Click;
		((BaseItem)BtnDefMorakhsi).AutoCollapseOnClick = false;
		((BaseItem)BtnDefMorakhsi).CanCustomize = false;
		BtnDefMorakhsi.Image = (Image)(object)Resources.gnome_set_time;
		BtnDefMorakhsi.ImagePaddingHorizontal = 45;
		BtnDefMorakhsi.ImagePaddingVertical = 45;
		BtnDefMorakhsi.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnDefMorakhsi).Name = "BtnDefMorakhsi";
		BtnDefMorakhsi.SubItemsExpandWidth = 14;
		((BaseItem)BtnDefMorakhsi).Text = "[انواع مرخصی]";
		((BaseItem)BtnDefMorakhsi).Click += BtnDefMorakhsi_Click;
		((BaseItem)BtnPersonel).AutoCollapseOnClick = false;
		((BaseItem)BtnPersonel).CanCustomize = false;
		BtnPersonel.Image = (Image)componentResourceManager.GetObject("BtnPersonel.Image");
		BtnPersonel.ImagePaddingHorizontal = 50;
		BtnPersonel.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnPersonel).Name = "BtnPersonel";
		BtnPersonel.SplitButton = true;
		BtnPersonel.SubItemsExpandWidth = 14;
		((BaseItem)BtnPersonel).Text = "[پرسنل]";
		((BaseItem)BtnPersonel).Click += BtnPersonel_Click;
		((BaseItem)BtnListM).AutoCollapseOnClick = false;
		((BaseItem)BtnListM).CanCustomize = false;
		BtnListM.Image = (Image)(object)Resources.going_down_two_bars;
		BtnListM.ImagePaddingHorizontal = 25;
		BtnListM.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnListM).Name = "BtnListM";
		BtnListM.SubItemsExpandWidth = 14;
		((BaseItem)BtnListM).Text = "[نوع حساب]";
		((BaseItem)BtnListM).Click += BtnListM_Click_1;
		((BaseItem)BtnGharardad).AutoCollapseOnClick = false;
		((BaseItem)BtnGharardad).CanCustomize = false;
		BtnGharardad.Image = (Image)(object)Resources.ContractSalary1;
		BtnGharardad.ImagePaddingHorizontal = 50;
		BtnGharardad.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnGharardad).Name = "BtnGharardad";
		BtnGharardad.SplitButton = true;
		BtnGharardad.SubItemsExpandWidth = 14;
		((BaseItem)BtnGharardad).Text = "[قرارداد]";
		((BaseItem)BtnGharardad).Click += BtnGharardad_Click;
		((BaseItem)btnTaxTable).AutoCollapseOnClick = false;
		((BaseItem)btnTaxTable).CanCustomize = false;
		btnTaxTable.Image = (Image)(object)Resources.folder_home2;
		btnTaxTable.ImagePaddingHorizontal = 65;
		btnTaxTable.ImagePosition = (eImagePosition)2;
		((BaseItem)btnTaxTable).Name = "btnTaxTable";
		btnTaxTable.SubItemsExpandWidth = 14;
		((BaseItem)btnTaxTable).Text = "[جدول مالیاتی]";
		((BaseItem)btnTaxTable).Click += BtnMaliyat_Click;
		((BaseItem)BtnBaseYearsSalary).AutoCollapseOnClick = false;
		((BaseItem)BtnBaseYearsSalary).CanCustomize = false;
		BtnBaseYearsSalary.Image = (Image)componentResourceManager.GetObject("BtnBaseYearsSalary.Image");
		BtnBaseYearsSalary.ImagePaddingHorizontal = 50;
		BtnBaseYearsSalary.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnBaseYearsSalary).Name = "BtnBaseYearsSalary";
		BtnBaseYearsSalary.SplitButton = true;
		BtnBaseYearsSalary.SubItemsExpandWidth = 14;
		((BaseItem)BtnBaseYearsSalary).Text = "[پایه سنواتی]";
		((BaseItem)BtnBaseYearsSalary).Click += BtnBaseYearsSalary_Click;
		((BaseItem)btnDepartment).AutoCollapseOnClick = false;
		((BaseItem)btnDepartment).CanCustomize = false;
		btnDepartment.Image = (Image)(object)Resources.vacation1;
		btnDepartment.ImagePaddingHorizontal = 50;
		btnDepartment.ImagePosition = (eImagePosition)2;
		((BaseItem)btnDepartment).Name = "btnDepartment";
		((BaseItem)btnDepartment).Text = "مدیریت دپارتمان ها";
		((BaseItem)btnDepartment).Click += btnDepartment_Click;
		((PanelControl)ribbonPanel7).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel7).Controls.Add((Control)(object)ribbonBar7);
		((Control)ribbonPanel7).Dock = (DockStyle)5;
		ribbonPanel7.Location = new Point(0, 56);
		((Control)ribbonPanel7).Name = "ribbonPanel7";
		((Control)ribbonPanel7).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel7.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel7).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel7).TabIndex = 8;
		ribbonPanel7.Visible = false;
		ribbonBar7.AutoOverflowEnabled = true;
		ribbonBar7.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar7).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar7).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar7).Dock = (DockStyle)5;
		((ItemControl)ribbonBar7).DragDropSupport = true;
		ribbonBar7.Items.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)BtnClub,
			(BaseItem)BtnAttendanceSport,
			(BaseItem)BtnDefinePackage,
			(BaseItem)BtnReloadEnrollData
		});
		((Control)ribbonBar7).Location = new Point(3, 0);
		((Control)ribbonBar7).Name = "ribbonBar7";
		((Control)ribbonBar7).RightToLeft = (RightToLeft)1;
		((Control)ribbonBar7).Size = new Size(1460, 99);
		ribbonBar7.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar7).TabIndex = 0;
		((Control)ribbonBar7).Text = "ribbonBar7";
		ribbonBar7.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar7.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar7.TitleVisible = false;
		((BaseItem)BtnClub).AutoCollapseOnClick = false;
		((BaseItem)BtnClub).CanCustomize = false;
		BtnClub.Image = (Image)componentResourceManager.GetObject("BtnClub.Image");
		BtnClub.ImagePaddingHorizontal = 55;
		BtnClub.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnClub).Name = "BtnClub";
		BtnClub.SubItemsExpandWidth = 14;
		((BaseItem)BtnClub).Text = "ثبت نام کلوپ";
		((BaseItem)BtnClub).Click += BtnClub_Click;
		((BaseItem)BtnAttendanceSport).AutoCollapseOnClick = false;
		((BaseItem)BtnAttendanceSport).CanCustomize = false;
		BtnAttendanceSport.Image = (Image)(object)Resources.SPORT02;
		BtnAttendanceSport.ImagePaddingHorizontal = 55;
		BtnAttendanceSport.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnAttendanceSport).Name = "BtnAttendanceSport";
		BtnAttendanceSport.SubItemsExpandWidth = 14;
		((BaseItem)BtnAttendanceSport).Text = "[جلسات یک شخص]";
		((BaseItem)BtnAttendanceSport).Click += BtnAttendanceSport_Click;
		((BaseItem)BtnDefinePackage).AutoCollapseOnClick = false;
		((BaseItem)BtnDefinePackage).CanCustomize = false;
		BtnDefinePackage.Image = (Image)(object)Resources.sport04;
		BtnDefinePackage.ImagePaddingHorizontal = 55;
		BtnDefinePackage.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnDefinePackage).Name = "BtnDefinePackage";
		BtnDefinePackage.SubItemsExpandWidth = 14;
		((BaseItem)BtnDefinePackage).Text = "[تعریف پکیج]";
		((BaseItem)BtnDefinePackage).Click += BtnDefinePackage_Click;
		((BaseItem)BtnReloadEnrollData).AutoCollapseOnClick = false;
		((BaseItem)BtnReloadEnrollData).CanCustomize = false;
		BtnReloadEnrollData.Image = (Image)(object)Resources.sport04;
		BtnReloadEnrollData.ImagePaddingHorizontal = 55;
		BtnReloadEnrollData.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnReloadEnrollData).Name = "BtnReloadEnrollData";
		BtnReloadEnrollData.SubItemsExpandWidth = 14;
		((BaseItem)BtnReloadEnrollData).Text = "نگاشت مجدد کاربران فعال";
		((BaseItem)BtnReloadEnrollData).Click += BtnReloadEnrollData_Click;
		((PanelControl)ribbonPanel8).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel8).Controls.Add((Control)(object)ribbonBar8);
		((Control)ribbonPanel8).Dock = (DockStyle)5;
		ribbonPanel8.Location = new Point(0, 56);
		((Control)ribbonPanel8).Name = "ribbonPanel8";
		((Control)ribbonPanel8).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel8.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel8).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel8).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel8).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel8).TabIndex = 9;
		ribbonPanel8.Visible = false;
		ribbonBar8.AutoOverflowEnabled = true;
		ribbonBar8.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar8).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar8).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar8).Dock = (DockStyle)3;
		((ItemControl)ribbonBar8).DragDropSupport = true;
		ribbonBar8.Items.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)BtnSmsCalendar,
			(BaseItem)BtnSendedSms,
			(BaseItem)BtnNotSend
		});
		((Control)ribbonBar8).Location = new Point(3, 0);
		((Control)ribbonBar8).Name = "ribbonBar8";
		((Control)ribbonBar8).Size = new Size(1464, 99);
		ribbonBar8.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar8).TabIndex = 0;
		((Control)ribbonBar8).Text = "ribbonBar8";
		ribbonBar8.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar8.TitleStyleMouseOver.CornerType = (eCornerType)1;
		((BaseItem)BtnSmsCalendar).AutoCollapseOnClick = false;
		((BaseItem)BtnSmsCalendar).CanCustomize = false;
		BtnSmsCalendar.Image = (Image)(object)Resources.taradod;
		BtnSmsCalendar.ImagePaddingHorizontal = 35;
		BtnSmsCalendar.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSmsCalendar).Name = "BtnSmsCalendar";
		BtnSmsCalendar.SubItemsExpandWidth = 14;
		((BaseItem)BtnSmsCalendar).Text = "لغو ارسال پیام به گروه خاص";
		((BaseItem)BtnSmsCalendar).Click += BtnSmsCalendar_Click;
		((BaseItem)BtnSendedSms).AutoCollapseOnClick = false;
		((BaseItem)BtnSendedSms).CanCustomize = false;
		BtnSendedSms.Image = (Image)(object)Resources.taradod;
		BtnSendedSms.ImagePaddingHorizontal = 35;
		BtnSendedSms.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSendedSms).Name = "BtnSendedSms";
		BtnSendedSms.SubItemsExpandWidth = 14;
		((BaseItem)BtnSendedSms).Text = "پیامهای ارسال شده";
		((BaseItem)BtnSendedSms).Click += BtnSendedSms_Click;
		((BaseItem)BtnNotSend).AutoCollapseOnClick = false;
		((BaseItem)BtnNotSend).CanCustomize = false;
		BtnNotSend.Image = (Image)(object)Resources.taradod;
		BtnNotSend.ImagePaddingHorizontal = 35;
		BtnNotSend.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnNotSend).Name = "BtnNotSend";
		BtnNotSend.SubItemsExpandWidth = 14;
		((BaseItem)BtnNotSend).Text = "پیامهای در نوبت ارسال";
		((BaseItem)BtnNotSend).Click += BtnNotSend_Click;
		((PanelControl)ribbonPanel5).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel5).Controls.Add((Control)(object)ribbonBar5);
		((Control)ribbonPanel5).Dock = (DockStyle)5;
		ribbonPanel5.Location = new Point(0, 56);
		((Control)ribbonPanel5).Name = "ribbonPanel5";
		((Control)ribbonPanel5).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel5.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel5).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel5).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel5).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel5).TabIndex = 6;
		ribbonPanel5.Visible = false;
		ribbonBar5.AutoOverflowEnabled = true;
		ribbonBar5.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar5).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar5).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar5).Dock = (DockStyle)5;
		((ItemControl)ribbonBar5).DragDropSupport = true;
		ribbonBar5.Items.AddRange((BaseItem[])(object)new BaseItem[6]
		{
			(BaseItem)Btnsoftware,
			(BaseItem)btnactivation,
			(BaseItem)BtnAboutUs,
			(BaseItem)BtnCallUs,
			(BaseItem)BtnTutorialVideo,
			(BaseItem)BtnSuggestion
		});
		((Control)ribbonBar5).Location = new Point(3, 0);
		((Control)ribbonBar5).Name = "ribbonBar5";
		((Control)ribbonBar5).Size = new Size(1460, 99);
		ribbonBar5.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar5).TabIndex = 1;
		((Control)ribbonBar5).Text = "ribbonBar5";
		ribbonBar5.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar5.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar5.TitleVisible = false;
		((BaseItem)Btnsoftware).AutoCollapseOnClick = false;
		((BaseItem)Btnsoftware).CanCustomize = false;
		Btnsoftware.Image = (Image)(object)Resources.software_properties;
		Btnsoftware.ImagePaddingHorizontal = 55;
		Btnsoftware.ImagePosition = (eImagePosition)2;
		((BaseItem)Btnsoftware).Name = "Btnsoftware";
		Btnsoftware.SubItemsExpandWidth = 14;
		((BaseItem)Btnsoftware).Text = "[مشخصات نرم افزار]";
		((BaseItem)Btnsoftware).Click += Btnsoftware_Click;
		((BaseItem)btnactivation).AutoCollapseOnClick = false;
		btnactivation.ButtonStyle = (eButtonStyle)2;
		((BaseItem)btnactivation).CanCustomize = false;
		btnactivation.Image = (Image)(object)Resources.gnome_mime_application_pgp_keys;
		btnactivation.ImagePaddingHorizontal = 55;
		btnactivation.ImagePosition = (eImagePosition)2;
		((BaseItem)btnactivation).Name = "btnactivation";
		btnactivation.SubItemsExpandWidth = 14;
		((BaseItem)btnactivation).Text = "[فعالسازی]";
		((BaseItem)btnactivation).Click += Btnactivation_Click;
		((BaseItem)BtnAboutUs).AutoCollapseOnClick = false;
		((BaseItem)BtnAboutUs).CanCustomize = false;
		BtnAboutUs.Image = (Image)componentResourceManager.GetObject("BtnAboutUs.Image");
		BtnAboutUs.ImagePaddingHorizontal = 55;
		BtnAboutUs.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnAboutUs).Name = "BtnAboutUs";
		BtnAboutUs.SubItemsExpandWidth = 14;
		((BaseItem)BtnAboutUs).Text = "[درباره ما]";
		((BaseItem)BtnAboutUs).Click += BtnAboutUs_Click;
		((BaseItem)BtnCallUs).AutoCollapseOnClick = false;
		((BaseItem)BtnCallUs).CanCustomize = false;
		BtnCallUs.Image = (Image)componentResourceManager.GetObject("BtnCallUs.Image");
		BtnCallUs.ImagePaddingHorizontal = 55;
		BtnCallUs.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnCallUs).Name = "BtnCallUs";
		BtnCallUs.SubItemsExpandWidth = 14;
		((BaseItem)BtnCallUs).Text = "[تماس با ما]";
		((BaseItem)BtnCallUs).Click += BtnCallUs_Click;
		((BaseItem)BtnTutorialVideo).AutoCollapseOnClick = false;
		((BaseItem)BtnTutorialVideo).CanCustomize = false;
		BtnTutorialVideo.Image = (Image)(object)Resources.training2;
		BtnTutorialVideo.ImagePaddingHorizontal = 55;
		BtnTutorialVideo.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnTutorialVideo).Name = "BtnTutorialVideo";
		BtnTutorialVideo.SubItemsExpandWidth = 14;
		((BaseItem)BtnTutorialVideo).Text = "[فیلم های آموزشی]";
		((BaseItem)BtnTutorialVideo).Click += BtnTutorialVideo_Click;
		((BaseItem)BtnSuggestion).AutoCollapseOnClick = false;
		((BaseItem)BtnSuggestion).CanCustomize = false;
		BtnSuggestion.Image = (Image)(object)Resources._1313214_2;
		BtnSuggestion.ImagePaddingHorizontal = 55;
		BtnSuggestion.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnSuggestion).Name = "BtnSuggestion";
		BtnSuggestion.SubItemsExpandWidth = 14;
		((BaseItem)BtnSuggestion).Text = "[پیشنهاد / انتقاد]";
		((PanelControl)ribbonPanel6).ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)ribbonPanel6).Controls.Add((Control)(object)ribbonBar6);
		((Control)ribbonPanel6).Dock = (DockStyle)5;
		ribbonPanel6.Location = new Point(0, 56);
		((Control)ribbonPanel6).Name = "ribbonPanel6";
		((Control)ribbonPanel6).Padding = new Padding(3, 0, 3, 3);
		ribbonPanel6.Size = new Size(1466, 102);
		((PanelControl)ribbonPanel6).Style.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel6).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)ribbonPanel6).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)ribbonPanel6).TabIndex = 7;
		ribbonPanel6.Visible = false;
		ribbonBar6.AutoOverflowEnabled = true;
		ribbonBar6.BackgroundMouseOverStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar6).BackgroundStyle.CornerType = (eCornerType)1;
		((ItemControl)ribbonBar6).ContainerControlProcessDialogKey = true;
		((Control)ribbonBar6).Dock = (DockStyle)5;
		((ItemControl)ribbonBar6).DragDropSupport = true;
		ribbonBar6.Items.AddRange((BaseItem[])(object)new BaseItem[6]
		{
			(BaseItem)BtnRPTsaz,
			(BaseItem)BtnRptAllTaradod,
			(BaseItem)btnRptNotify,
			(BaseItem)btnVacations,
			(BaseItem)BtnTajmiVacations,
			(BaseItem)BtnEndedGharardad
		});
		((Control)ribbonBar6).Location = new Point(3, 0);
		((Control)ribbonBar6).Name = "ribbonBar6";
		((Control)ribbonBar6).Size = new Size(1460, 99);
		ribbonBar6.Style = (eDotNetBarStyle)9;
		((Control)ribbonBar6).TabIndex = 2;
		((Control)ribbonBar6).Text = "ribbonBar6";
		ribbonBar6.TitleStyle.CornerType = (eCornerType)1;
		ribbonBar6.TitleStyleMouseOver.CornerType = (eCornerType)1;
		ribbonBar6.TitleVisible = false;
		((BaseItem)BtnRPTsaz).AutoCollapseOnClick = false;
		((BaseItem)BtnRPTsaz).CanCustomize = false;
		BtnRPTsaz.Image = (Image)(object)Resources.rptsaz;
		BtnRPTsaz.ImagePaddingHorizontal = 55;
		BtnRPTsaz.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRPTsaz).Name = "BtnRPTsaz";
		BtnRPTsaz.SubItemsExpandWidth = 14;
		((BaseItem)BtnRPTsaz).Text = "[گزارش ساز پویا]";
		((BaseItem)BtnRPTsaz).Click += BtnRPTsaz_Click;
		((BaseItem)BtnRptAllTaradod).AutoCollapseOnClick = false;
		((BaseItem)BtnRptAllTaradod).CanCustomize = false;
		BtnRptAllTaradod.Image = (Image)(object)Resources.taradod1;
		BtnRptAllTaradod.ImagePaddingHorizontal = 80;
		BtnRptAllTaradod.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnRptAllTaradod).Name = "BtnRptAllTaradod";
		BtnRptAllTaradod.SubItemsExpandWidth = 14;
		((BaseItem)BtnRptAllTaradod).Text = "[گزارش تردد کلیه کارکنان]";
		((BaseItem)BtnRptAllTaradod).Click += BtnRptAllTaradod_Click;
		((BaseItem)btnRptNotify).AutoCollapseOnClick = false;
		((BaseItem)btnRptNotify).CanCustomize = false;
		btnRptNotify.Image = (Image)(object)Resources.event2;
		btnRptNotify.ImagePaddingHorizontal = 80;
		btnRptNotify.ImagePosition = (eImagePosition)2;
		((BaseItem)btnRptNotify).Name = "btnRptNotify";
		btnRptNotify.SubItemsExpandWidth = 14;
		((BaseItem)btnRptNotify).Text = "[پیام های حضور ]";
		((BaseItem)btnRptNotify).Click += BtnRptNotify_Click;
		((BaseItem)btnVacations).AutoCollapseOnClick = false;
		((BaseItem)btnVacations).CanCustomize = false;
		btnVacations.Image = (Image)(object)Resources.Vacations;
		btnVacations.ImagePaddingHorizontal = 50;
		btnVacations.ImagePosition = (eImagePosition)2;
		((BaseItem)btnVacations).Name = "btnVacations";
		((BaseItem)btnVacations).Text = "[گزارش مرخصی ها]";
		((BaseItem)btnVacations).Click += BtnVacations_Click;
		((BaseItem)BtnTajmiVacations).AutoCollapseOnClick = false;
		((BaseItem)BtnTajmiVacations).CanCustomize = false;
		BtnTajmiVacations.Image = (Image)(object)Resources.Vacations;
		BtnTajmiVacations.ImagePaddingHorizontal = 50;
		BtnTajmiVacations.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnTajmiVacations).Name = "BtnTajmiVacations";
		((BaseItem)BtnTajmiVacations).Text = "[گزارش تجمیع مرخصی]";
		((BaseItem)BtnTajmiVacations).Click += BtnTajmiVacations_Click;
		((BaseItem)BtnEndedGharardad).AutoCollapseOnClick = false;
		((BaseItem)BtnEndedGharardad).CanCustomize = false;
		BtnEndedGharardad.Image = (Image)(object)Resources.blue_package;
		BtnEndedGharardad.ImageListSizeSelection = (eButtonImageListSelection)2;
		BtnEndedGharardad.ImagePaddingHorizontal = 50;
		BtnEndedGharardad.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnEndedGharardad).Name = "BtnEndedGharardad";
		((BaseItem)BtnEndedGharardad).Stretch = true;
		((BaseItem)BtnEndedGharardad).Text = "[قراردادهای منقضی]";
		((BaseItem)BtnEndedGharardad).Click += BtnEndedGharardad_Click;
		((BaseItem)ribbonTabItem1).Name = "ribbonTabItem1";
		ribbonTabItem1.Panel = ribbonPanel1;
		((ButtonItem)ribbonTabItem1).SplitButton = true;
		((BaseItem)ribbonTabItem1).Text = "[تعاریف]";
		((BaseItem)ribbonTabItem2).Name = "ribbonTabItem2";
		ribbonTabItem2.Panel = ribbonPanel3;
		((BaseItem)ribbonTabItem2).Text = "[حضور غیاب]";
		((BaseItem)ribbonTabItem3).Name = "ribbonTabItem3";
		ribbonTabItem3.Panel = ribbonPanel2;
		((BaseItem)ribbonTabItem3).Text = "[حقوق دستمزد]";
		((BaseItem)ribbonTabItem4).Name = "ribbonTabItem4";
		ribbonTabItem4.Panel = ribbonPanel6;
		((BaseItem)ribbonTabItem4).Text = "[گزارشات]";
		((ButtonItem)ribbonTabItem5).Checked = true;
		((BaseItem)ribbonTabItem5).Name = "ribbonTabItem5";
		ribbonTabItem5.Panel = ribbonPanel4;
		((BaseItem)ribbonTabItem5).Text = "[امکانات]";
		((BaseItem)ribbonTabItem6).Name = "ribbonTabItem6";
		ribbonTabItem6.Panel = ribbonPanel5;
		((BaseItem)ribbonTabItem6).Text = "[راهنما]";
		((BaseItem)ribbonTabItemSportClub).CanCustomize = false;
		((BaseItem)ribbonTabItemSportClub).Name = "ribbonTabItemSportClub";
		ribbonTabItemSportClub.Panel = ribbonPanel7;
		((BaseItem)ribbonTabItemSportClub).Shortcuts.Add((eShortcut)327803);
		((ButtonItem)ribbonTabItemSportClub).SplitButton = true;
		((BaseItem)ribbonTabItemSportClub).Text = "[حضور و غیاب باشگاه]";
		((BaseItem)ribbonTabItemSportClub).Visible = false;
		((BaseItem)ribbonTabItemSportClub).Click += ribbonTabItemSportClub_Click;
		((BaseItem)ribbonTabItemSportClub).DoubleClick += ribbonTabItemSportClub_DoubleClick;
		((BaseItem)ribbonTabItemSportClub).ExpandChange += ribbonTabItemSportClub_ExpandChange;
		((BaseItem)ribbonTabItemSMS).Name = "ribbonTabItemSMS";
		ribbonTabItemSMS.Panel = ribbonPanel8;
		((BaseItem)ribbonTabItemSMS).Text = "مدیریت پیام ها";
		BtnLogOnManage.ImagePaddingHorizontal = 25;
		BtnLogOnManage.ImagePosition = (eImagePosition)2;
		((BaseItem)BtnLogOnManage).Name = "BtnLogOnManage";
		BtnLogOnManage.SubItemsExpandWidth = 14;
		((BaseItem)BtnLogOnManage).Text = "[تردد مدیران]";
		BtnLogOnPersonel.ImagePaddingHorizontal = 15;
		((BaseItem)BtnLogOnPersonel).Name = "BtnLogOnPersonel";
		BtnLogOnPersonel.SubItemsExpandWidth = 14;
		((BaseItem)BtnLogOnPersonel).Text = "[تردد پرسنل]";
		notifyIcon1.BalloonTipIcon = (ToolTipIcon)1;
		notifyIcon1.BalloonTipText = "[نوین پرداز]";
		notifyIcon1.BalloonTipTitle = "[نرم افزار حضور و غیاب1]";
		notifyIcon1.ContextMenuStrip = NotifyContextMenue;
		notifyIcon1.Icon = (Icon)componentResourceManager.GetObject("notifyIcon1.Icon");
		notifyIcon1.Text = "[حضور غیاب نوین پرداز]";
		notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
		((ToolStrip)NotifyContextMenue).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)CMSOpenProgram,
			(ToolStripItem)toolStripSeparator2,
			(ToolStripItem)CMSAboutUS,
			(ToolStripItem)toolStripSeparator1,
			(ToolStripItem)CMSExitProgram
		});
		((Control)NotifyContextMenue).Name = "NotifyContextMenue";
		((Control)NotifyContextMenue).RightToLeft = (RightToLeft)1;
		((Control)NotifyContextMenue).Size = new Size(177, 82);
		((ToolStripItem)CMSOpenProgram).Name = "CMSOpenProgram";
		((ToolStripItem)CMSOpenProgram).Size = new Size(176, 22);
		((ToolStripItem)CMSOpenProgram).Text = "[باز کردن برنامه]";
		((ToolStripItem)CMSOpenProgram).Click += CMSOpenProgram_Click;
		((ToolStripItem)toolStripSeparator2).Name = "toolStripSeparator2";
		((ToolStripItem)toolStripSeparator2).Size = new Size(173, 6);
		((ToolStripItem)CMSAboutUS).Name = "CMSAboutUS";
		((ToolStripItem)CMSAboutUS).Size = new Size(176, 22);
		((ToolStripItem)CMSAboutUS).Text = "درباره ما";
		((ToolStripItem)CMSAboutUS).Click += CMSAboutUS_Click;
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(173, 6);
		((ToolStripItem)CMSExitProgram).Name = "CMSExitProgram";
		((ToolStripItem)CMSExitProgram).Size = new Size(176, 22);
		((ToolStripItem)CMSExitProgram).Text = "[خروج کامل از برنامه]";
		((ToolStripItem)CMSExitProgram).Click += CMSExitProgram_Click;
		timerStopNotification.Interval = 2000;
		buttonItem3.Image = (Image)(object)Resources.Enroll;
		buttonItem3.ImagePaddingHorizontal = 60;
		buttonItem3.ImagePosition = (eImagePosition)2;
		((BaseItem)buttonItem3).Name = "buttonItem3";
		buttonItem3.SubItemsExpandWidth = 14;
		((BaseItem)buttonItem3).Text = "[گزارش  تردد]";
		buttonItem2.Image = (Image)componentResourceManager.GetObject("buttonItem2.Image");
		buttonItem2.ImagePaddingHorizontal = 55;
		buttonItem2.ImagePosition = (eImagePosition)2;
		((BaseItem)buttonItem2).Name = "buttonItem2";
		buttonItem2.SubItemsExpandWidth = 14;
		((BaseItem)buttonItem2).Text = "[تماس با ما]";
		((Control)btnNotifyMe).AccessibleRole = (AccessibleRole)43;
		((Control)btnNotifyMe).Anchor = (AnchorStyles)9;
		((Control)btnNotifyMe).BackColor = Color.Transparent;
		btnNotifyMe.ColorTable = (eButtonColor)8;
		((Control)btnNotifyMe).Font = new Font("Tahoma", 10f, (FontStyle)1);
		btnNotifyMe.Image = (Image)(object)Resources.AuditIcon2;
		((Control)btnNotifyMe).Location = new Point(1428, 267);
		((Control)btnNotifyMe).Name = "btnNotifyMe";
		((Control)btnNotifyMe).Size = new Size(30, 28);
		((PopupItemControl)btnNotifyMe).Style = (eDotNetBarStyle)9;
		((Control)btnNotifyMe).TabIndex = 9;
		btnNotifyMe.Tooltip = "پیامهای نرم افزار حضور";
		((Control)btnNotifyMe).Visible = false;
		((Control)btnNotifyMe).Click += BtnNotifyMe_Click;
		timerReadNotify.Interval = 750;
		timerLock.Interval = 10000;
		timerLock.Tick += TimerLock_Tick;
		((Control)LanguageLable).Anchor = (AnchorStyles)6;
		((Control)LanguageLable).AutoSize = true;
		((Control)LanguageLable).BackColor = Color.LightSteelBlue;
		LanguageLable.LinkColor = Color.Black;
		((Control)LanguageLable).Location = new Point(207, 709);
		((Control)LanguageLable).Name = "LanguageLable";
		((Control)LanguageLable).Size = new Size(10, 13);
		((Control)LanguageLable).TabIndex = 10;
		LanguageLable.TabStop = true;
		((Control)LanguageLable).Text = " ";
		LanguageLable.LinkClicked += new LinkLabelLinkClickedEventHandler(LanguageLable_LinkClicked);
		((Control)monthCalendarX1).BackColor = Color.White;
		monthCalendarX1.BoldedDayForeColor = Color.Blue;
		monthCalendarX1.BorderColor = Color.White;
		monthCalendarX1.CalendarType = (CalendarTypes)1;
		monthCalendarX1.DayRectTickness = 2f;
		monthCalendarX1.DaysBackColor = Color.White;
		monthCalendarX1.DaysFont = new Font("B Homa", 9f, (FontStyle)1);
		monthCalendarX1.DaysForeColor = Color.FromArgb(0, 0, 192);
		monthCalendarX1.EnglishAnnuallyBoldedDates = new DateTime[0];
		monthCalendarX1.EnglishBoldedDates = new DateTime[0];
		monthCalendarX1.EnglishHolidayDates = new DateTime[0];
		monthCalendarX1.EnglishMonthlyBoldedDates = new DateTime[0];
		monthCalendarX1.HolidayForeColor = Color.Red;
		monthCalendarX1.HolidayWeekly = (DayOfWeekForHoliday)1;
		monthCalendarX1.LineWeekColor = Color.Black;
		((Control)monthCalendarX1).Location = new Point(0, 169);
		((Control)monthCalendarX1).Margin = new Padding(4, 3, 4, 3);
		((Control)monthCalendarX1).Name = "monthCalendarX1";
		monthCalendarX1.PersianAnnuallyBoldedDates = (PersianDateTime[])(object)new PersianDateTime[0];
		monthCalendarX1.PersianBoldedDates = (PersianDateTime[])(object)new PersianDateTime[0];
		monthCalendarX1.PersianHolidayDates = (PersianDateTime[])(object)new PersianDateTime[0];
		monthCalendarX1.PersianMonthlyBoldedDates = (PersianDateTime[])(object)new PersianDateTime[0];
		monthCalendarX1.ShowToday = true;
		monthCalendarX1.ShowTodayRect = true;
		monthCalendarX1.ShowToolTips = true;
		monthCalendarX1.ShowTrailing = true;
		((Control)monthCalendarX1).Size = new Size(200, 200);
		monthCalendarX1.Style_DaysButton = (ButtonStyles)1;
		monthCalendarX1.Style_GotoTodayButton = (ButtonStyles)2;
		monthCalendarX1.Style_MonthButton = (ButtonStyles)2;
		monthCalendarX1.Style_NextMonthButton = (ButtonStyles)2;
		monthCalendarX1.Style_PreviousMonthButton = (ButtonStyles)2;
		monthCalendarX1.Style_YearButton = (ButtonStyles)2;
		((Control)monthCalendarX1).TabIndex = 11;
		monthCalendarX1.TitleBackColor = Color.White;
		monthCalendarX1.TitleFont = new Font("Tahoma", 8.25f);
		monthCalendarX1.TitleForeColor = Color.Black;
		monthCalendarX1.TodayBackColor = Color.LightGray;
		monthCalendarX1.TodayFont = new Font("Tahoma", 9f, (FontStyle)1);
		monthCalendarX1.TodayForeColor = Color.FromArgb(0, 64, 64);
		monthCalendarX1.TodayRectColor = Color.White;
		monthCalendarX1.TodayRectTickness = 2f;
		monthCalendarX1.TrailingForeColor = Color.DarkGray;
		((Control)monthCalendarX1).Visible = false;
		monthCalendarX1.WeekDaysBackColor = Color.White;
		monthCalendarX1.WeekDaysFont = new Font("Tahoma", 8.25f);
		monthCalendarX1.WeekDaysForeColor = SystemColors.ActiveCaptionText;
		monthCalendarX1.WeekStartsOn = (WeekDays)0;
		monthCalendarX1.SelectedDateChanged += new OnSelectedDateChanged(MonthCalendarX1_SelectedDateChanged);
		((UserControl)monthCalendarX1).Load += MonthCalendarX1_Load;
		((Control)TbxMonasebat).BackColor = Color.White;
		((TextBoxBase)TbxMonasebat).BorderStyle = (BorderStyle)0;
		((Control)TbxMonasebat).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)TbxMonasebat).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TbxMonasebat).Location = new Point(0, 370);
		((TextBoxBase)TbxMonasebat).Multiline = true;
		((Control)TbxMonasebat).Name = "TbxMonasebat";
		((TextBoxBase)TbxMonasebat).ReadOnly = true;
		TbxMonasebat.ScrollBars = (ScrollBars)2;
		((Control)TbxMonasebat).Size = new Size(200, 52);
		((Control)TbxMonasebat).TabIndex = 12;
		((Control)TbxMonasebat).Visible = false;
		((Control)ucCacheStatus1).Anchor = (AnchorStyles)9;
		((Control)ucCacheStatus1).BackColor = Color.White;
		((UserControl)ucCacheStatus1).BorderStyle = (BorderStyle)1;
		ucCacheStatus1.ContextGuid = null;
		((Control)ucCacheStatus1).Font = new Font("B Titr", 11.25f, (FontStyle)1);
		((Control)ucCacheStatus1).Location = new Point(1428, 199);
		((Control)ucCacheStatus1).Margin = new Padding(6, 6, 6, 6);
		((Control)ucCacheStatus1).Name = "ucCacheStatus1";
		ucCacheStatus1.ShowStatusMenu = false;
		((Control)ucCacheStatus1).Size = new Size(30, 28);
		((Control)ucCacheStatus1).TabIndex = 13;
		((Control)ImgBazsazi).Anchor = (AnchorStyles)9;
		((Control)ImgBazsazi).BackColor = Color.Transparent;
		ImgBazsazi.Image = (Image)componentResourceManager.GetObject("ImgBazsazi.Image");
		((Control)ImgBazsazi).Location = new Point(1426, 338);
		((Control)ImgBazsazi).Name = "ImgBazsazi";
		((Control)ImgBazsazi).Size = new Size(34, 32);
		ImgBazsazi.SizeMode = (PictureBoxSizeMode)1;
		ImgBazsazi.TabIndex = 14;
		ImgBazsazi.TabStop = false;
		((Control)ImgBazsazi).Visible = false;
		((Control)ImgRecovery).Anchor = (AnchorStyles)9;
		((Control)ImgRecovery).BackColor = Color.Transparent;
		ImgRecovery.Image = (Image)componentResourceManager.GetObject("ImgRecovery.Image");
		((Control)ImgRecovery).Location = new Point(1426, 299);
		((Control)ImgRecovery).Name = "ImgRecovery";
		((Control)ImgRecovery).Size = new Size(34, 34);
		ImgRecovery.SizeMode = (PictureBoxSizeMode)1;
		ImgRecovery.TabIndex = 14;
		ImgRecovery.TabStop = false;
		((Control)ImgRecovery).UseWaitCursor = true;
		((Control)ImgRecovery).Visible = false;
		DgridReportDevices.AllowUserToAddRows = false;
		DgridReportDevices.AllowUserToDeleteRows = false;
		DgridReportDevices.AllowUserToOrderColumns = true;
		((Control)DgridReportDevices).Anchor = (AnchorStyles)14;
		DgridReportDevices.AutoGenerateColumns = false;
		DgridReportDevices.BackgroundColor = Color.White;
		DgridReportDevices.BorderStyle = (BorderStyle)0;
		val.Alignment = (DataGridViewContentAlignment)32;
		val.BackColor = SystemColors.Control;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		DgridReportDevices.ColumnHeadersDefaultCellStyle = val;
		DgridReportDevices.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DgridReportDevices.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)termTypeDataGridViewTextBoxColumn,
			(DataGridViewColumn)serialNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)LogType,
			(DataGridViewColumn)transactionIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)logTimeDataGridViewTextBoxColumn,
			(DataGridViewColumn)userIDDataGridViewTextBoxColumn,
			(DataGridViewColumn)ModifiedDate
		});
		DgridReportDevices.DataSource = deviceEventsEntityBindingSource;
		DgridReportDevices.GridColor = Color.White;
		((Control)DgridReportDevices).Location = new Point(729, 475);
		((Control)DgridReportDevices).Name = "DgridReportDevices";
		DgridReportDevices.ReadOnly = true;
		((Control)DgridReportDevices).RightToLeft = (RightToLeft)1;
		((Control)DgridReportDevices).Size = new Size(690, 226);
		((Control)DgridReportDevices).TabIndex = 47;
		((DataGridViewColumn)termTypeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)termTypeDataGridViewTextBoxColumn).DataPropertyName = "termType";
		val2.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)termTypeDataGridViewTextBoxColumn).DefaultCellStyle = val2;
		((DataGridViewColumn)termTypeDataGridViewTextBoxColumn).HeaderText = "نام دستگاه";
		((DataGridViewColumn)termTypeDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)termTypeDataGridViewTextBoxColumn).Name = "termTypeDataGridViewTextBoxColumn";
		((DataGridViewBand)termTypeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)serialNumberDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)serialNumberDataGridViewTextBoxColumn).DataPropertyName = "serialNumber";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)serialNumberDataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)serialNumberDataGridViewTextBoxColumn).HeaderText = "سریال دستگاه";
		((DataGridViewColumn)serialNumberDataGridViewTextBoxColumn).MinimumWidth = 120;
		((DataGridViewColumn)serialNumberDataGridViewTextBoxColumn).Name = "serialNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)serialNumberDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)LogType).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)LogType).DataPropertyName = "LogType";
		val4.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)LogType).DefaultCellStyle = val4;
		((DataGridViewColumn)LogType).HeaderText = "نوع لاگ";
		((DataGridViewColumn)LogType).MinimumWidth = 100;
		((DataGridViewColumn)LogType).Name = "LogType";
		((DataGridViewBand)LogType).ReadOnly = true;
		((DataGridViewBand)LogType).Visible = false;
		((DataGridViewColumn)transactionIDDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)transactionIDDataGridViewTextBoxColumn).DataPropertyName = "transactionID";
		((DataGridViewColumn)transactionIDDataGridViewTextBoxColumn).HeaderText = "شماره لاگ";
		((DataGridViewColumn)transactionIDDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)transactionIDDataGridViewTextBoxColumn).Name = "transactionIDDataGridViewTextBoxColumn";
		((DataGridViewBand)transactionIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)transactionIDDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).DataPropertyName = "logTime";
		val5.Alignment = (DataGridViewContentAlignment)32;
		val5.Format = "yyyy/MM/dd/ HH:mm:ss";
		((DataGridViewBand)logTimeDataGridViewTextBoxColumn).DefaultCellStyle = val5;
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).HeaderText = "زمان تردد";
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).MinimumWidth = 135;
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).Name = "logTimeDataGridViewTextBoxColumn";
		((DataGridViewBand)logTimeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)logTimeDataGridViewTextBoxColumn).Width = 135;
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).DataPropertyName = "userID";
		val6.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)userIDDataGridViewTextBoxColumn).DefaultCellStyle = val6;
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).HeaderText = "کد شخص";
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).MinimumWidth = 45;
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).Name = "userIDDataGridViewTextBoxColumn";
		((DataGridViewBand)userIDDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)userIDDataGridViewTextBoxColumn).Width = 45;
		((DataGridViewColumn)ModifiedDate).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)ModifiedDate).DataPropertyName = "ModifiedDate";
		val7.Alignment = (DataGridViewContentAlignment)32;
		val7.Format = "yyyy/MM/dd/ HH:mm:ss";
		val7.NullValue = null;
		((DataGridViewBand)ModifiedDate).DefaultCellStyle = val7;
		((DataGridViewColumn)ModifiedDate).HeaderText = "زمان ثبت";
		((DataGridViewColumn)ModifiedDate).MinimumWidth = 140;
		((DataGridViewColumn)ModifiedDate).Name = "ModifiedDate";
		((DataGridViewBand)ModifiedDate).ReadOnly = true;
		((DataGridViewColumn)ModifiedDate).Width = 140;
		deviceEventsEntityBindingSource.DataSource = typeof(DeviceEventsEntity);
		((DataGridView)DgridLastTaradods).AllowUserToAddRows = false;
		((DataGridView)DgridLastTaradods).AllowUserToDeleteRows = false;
		val8.BackColor = Color.FromArgb(224, 224, 224);
		((DataGridView)DgridLastTaradods).AlternatingRowsDefaultCellStyle = val8;
		((Control)DgridLastTaradods).Anchor = (AnchorStyles)6;
		((DataGridView)DgridLastTaradods).AutoGenerateColumns = false;
		((DataGridView)DgridLastTaradods).BackgroundColor = SystemColors.Window;
		((DataGridView)DgridLastTaradods).BorderStyle = (BorderStyle)0;
		val9.Alignment = (DataGridViewContentAlignment)32;
		val9.BackColor = SystemColors.Control;
		val9.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val9.ForeColor = SystemColors.WindowText;
		val9.SelectionBackColor = SystemColors.Highlight;
		val9.SelectionForeColor = SystemColors.HighlightText;
		val9.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DgridLastTaradods).ColumnHeadersDefaultCellStyle = val9;
		((DataGridView)DgridLastTaradods).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DgridLastTaradods).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[9]
		{
			(DataGridViewColumn)DgDatetimeLogM,
			(DataGridViewColumn)DgDeviceName,
			(DataGridViewColumn)DgPersonNo,
			(DataGridViewColumn)DgPersonFullName,
			(DataGridViewColumn)DgDateCalture,
			(DataGridViewColumn)DgGharardadEndDate,
			(DataGridViewColumn)DgGharardadStatusName,
			(DataGridViewColumn)DgTrafficTypeName,
			(DataGridViewColumn)DgPersonId
		});
		((DataGridView)DgridLastTaradods).DataSource = logOnPersonelTopLastTaradodsBindingSource;
		val10.Alignment = (DataGridViewContentAlignment)16;
		val10.BackColor = SystemColors.Window;
		val10.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val10.ForeColor = SystemColors.ControlText;
		val10.SelectionBackColor = SystemColors.Highlight;
		val10.SelectionForeColor = SystemColors.ControlText;
		val10.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DgridLastTaradods).DefaultCellStyle = val10;
		((DataGridView)DgridLastTaradods).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DgridLastTaradods).Location = new Point(0, 475);
		((Control)DgridLastTaradods).Name = "DgridLastTaradods";
		((DataGridView)DgridLastTaradods).ReadOnly = true;
		val11.BackColor = SystemColors.Control;
		val11.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val11.ForeColor = SystemColors.WindowText;
		val11.SelectionBackColor = SystemColors.Highlight;
		val11.SelectionForeColor = SystemColors.HighlightText;
		val11.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DgridLastTaradods).RowHeadersDefaultCellStyle = val11;
		((DataGridView)DgridLastTaradods).RowHeadersVisible = false;
		((DataGridView)DgridLastTaradods).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DgridLastTaradods).Size = new Size(729, 225);
		((Control)DgridLastTaradods).TabIndex = 54;
		((DataGridView)DgridLastTaradods).CellContentDoubleClick += new DataGridViewCellEventHandler(DgridLastTaradods_CellDoubleClick);
		((DataGridViewColumn)DgDatetimeLogM).DataPropertyName = "DateTimeLogM";
		((DataGridViewColumn)DgDatetimeLogM).HeaderText = "DateTimeLogM";
		((DataGridViewColumn)DgDatetimeLogM).Name = "DgDatetimeLogM";
		((DataGridViewBand)DgDatetimeLogM).ReadOnly = true;
		((DataGridViewBand)DgDatetimeLogM).Visible = false;
		((DataGridViewColumn)DgDeviceName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)8;
		((DataGridViewColumn)DgDeviceName).DataPropertyName = "DeviceName";
		((DataGridViewColumn)DgDeviceName).HeaderText = "نام دستگاه";
		((DataGridViewColumn)DgDeviceName).MinimumWidth = 50;
		((DataGridViewColumn)DgDeviceName).Name = "DgDeviceName";
		((DataGridViewBand)DgDeviceName).ReadOnly = true;
		((DataGridViewColumn)DgDeviceName).Width = 50;
		((DataGridViewColumn)DgPersonNo).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgPersonNo).DataPropertyName = "personNo";
		val12.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DgPersonNo).DefaultCellStyle = val12;
		((DataGridViewColumn)DgPersonNo).HeaderText = "کد شخص";
		((DataGridViewColumn)DgPersonNo).MinimumWidth = 30;
		((DataGridViewColumn)DgPersonNo).Name = "DgPersonNo";
		((DataGridViewBand)DgPersonNo).ReadOnly = true;
		((DataGridViewColumn)DgPersonNo).Width = 77;
		((DataGridViewColumn)DgPersonFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DgPersonFullName).DataPropertyName = "PersonFullName";
		((DataGridViewColumn)DgPersonFullName).HeaderText = "نام شخص";
		((DataGridViewColumn)DgPersonFullName).MinimumWidth = 100;
		((DataGridViewColumn)DgPersonFullName).Name = "DgPersonFullName";
		((DataGridViewBand)DgPersonFullName).ReadOnly = true;
		((DataGridViewColumn)DgPersonFullName).ToolTipText = "دوبار کلیک=مشاهده گزارش";
		((DataGridViewColumn)DgDateCalture).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DgDateCalture).DataPropertyName = "dateCalture";
		((DataGridViewColumn)DgDateCalture).HeaderText = "زمان تردد";
		((DataGridViewColumn)DgDateCalture).MinimumWidth = 120;
		((DataGridViewColumn)DgDateCalture).Name = "DgDateCalture";
		((DataGridViewBand)DgDateCalture).ReadOnly = true;
		((DataGridViewColumn)DgDateCalture).Width = 120;
		((DataGridViewColumn)DgGharardadEndDate).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgGharardadEndDate).DataPropertyName = "GharardadEndDateClature";
		((DataGridViewColumn)DgGharardadEndDate).HeaderText = "پایان قرارداد";
		((DataGridViewColumn)DgGharardadEndDate).Name = "DgGharardadEndDate";
		((DataGridViewBand)DgGharardadEndDate).ReadOnly = true;
		((DataGridViewColumn)DgGharardadEndDate).Width = 89;
		((DataGridViewColumn)DgGharardadStatusName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgGharardadStatusName).DataPropertyName = "GharardadStatusName";
		((DataGridViewColumn)DgGharardadStatusName).HeaderText = "وضعیت قرارداد";
		((DataGridViewColumn)DgGharardadStatusName).Name = "DgGharardadStatusName";
		((DataGridViewBand)DgGharardadStatusName).ReadOnly = true;
		((DataGridViewColumn)DgTrafficTypeName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DgTrafficTypeName).DataPropertyName = "TrafficTypeName";
		((DataGridViewColumn)DgTrafficTypeName).HeaderText = "نوع تردد";
		((DataGridViewColumn)DgTrafficTypeName).MinimumWidth = 50;
		((DataGridViewColumn)DgTrafficTypeName).Name = "DgTrafficTypeName";
		((DataGridViewBand)DgTrafficTypeName).ReadOnly = true;
		((DataGridViewBand)DgTrafficTypeName).Visible = false;
		((DataGridViewColumn)DgPersonId).DataPropertyName = "personId";
		((DataGridViewColumn)DgPersonId).HeaderText = "personId";
		((DataGridViewColumn)DgPersonId).Name = "DgPersonId";
		((DataGridViewBand)DgPersonId).ReadOnly = true;
		((DataGridViewBand)DgPersonId).Visible = false;
		logOnPersonelTopLastTaradodsBindingSource.DataSource = typeof(LogOnPersonelTopLastTaradods);
		((Control)ucLkExpireDisplay1).BackColor = Color.Transparent;
		((Control)ucLkExpireDisplay1).Location = new Point(1, 135);
		((Control)ucLkExpireDisplay1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucLkExpireDisplay1).Name = "ucLkExpireDisplay1";
		((Control)ucLkExpireDisplay1).RightToLeft = (RightToLeft)1;
		((Control)ucLkExpireDisplay1).Size = new Size(284, 28);
		((Control)ucLkExpireDisplay1).TabIndex = 124;
		((Control)ucNotificationViewer1).Anchor = (AnchorStyles)13;
		((Control)ucNotificationViewer1).BackColor = Color.Transparent;
		((Control)ucNotificationViewer1).Location = new Point(285, 135);
		((Control)ucNotificationViewer1).Margin = new Padding(4, 3, 4, 3);
		ucNotificationViewer1.MonasebatView = true;
		((Control)ucNotificationViewer1).Name = "ucNotificationViewer1";
		ucNotificationViewer1.NewsView = true;
		ucNotificationViewer1.NotificationSource = (ENSource)5;
		ucNotificationViewer1.NotificationView = true;
		((Control)ucNotificationViewer1).RightToLeft = (RightToLeft)1;
		((Control)ucNotificationViewer1).Size = new Size(1181, 28);
		((Control)ucNotificationViewer1).TabIndex = 131;
		((Control)ucStatuse1).Dock = (DockStyle)2;
		((Control)ucStatuse1).Location = new Point(0, 701);
		((Control)ucStatuse1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucStatuse1).Name = "ucStatuse1";
		((Control)ucStatuse1).RightToLeft = (RightToLeft)1;
		((Control)ucStatuse1).Size = new Size(1466, 29);
		((Control)ucStatuse1).TabIndex = 3;
		((Control)ucMain1).Anchor = (AnchorStyles)15;
		((Control)ucMain1).ContextMenuStrip = contextMenuStrip1;
		((Control)ucMain1).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucMain1).Location = new Point(-38, 135);
		((Control)ucMain1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucMain1).Name = "ucMain1";
		((Control)ucMain1).Size = new Size(1457, 571);
		((Control)ucMain1).TabIndex = 8;
		timerStartPayamak.Enabled = true;
		timerStartPayamak.Interval = 60000;
		timerStartPayamak.Tick += TimerStartPayamak_Tick;
		timerRefreshCalendar.Enabled = true;
		timerRefreshCalendar.Interval = 30000;
		timerRefreshCalendar.Tick += timerRefreshCalendar_Tick;
		((Control)ucPayamakServiceStatus1).Anchor = (AnchorStyles)9;
		((Control)ucPayamakServiceStatus1).BackColor = Color.Transparent;
		((UserControl)ucPayamakServiceStatus1).BorderStyle = (BorderStyle)1;
		ucPayamakServiceStatus1.ContextGuid = null;
		((Control)ucPayamakServiceStatus1).Location = new Point(1428, 168);
		((Control)ucPayamakServiceStatus1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucPayamakServiceStatus1).Name = "ucPayamakServiceStatus1";
		ucPayamakServiceStatus1.Owner = null;
		((Control)ucPayamakServiceStatus1).Size = new Size(30, 28);
		((Control)ucPayamakServiceStatus1).TabIndex = 138;
		((Control)ucPayamakServiceStatus1).Visible = false;
		((Control)linkLabelGetOtherVeraion).Anchor = (AnchorStyles)13;
		((Control)linkLabelGetOtherVeraion).BackColor = Color.Transparent;
		((Control)linkLabelGetOtherVeraion).Font = new Font("Tahoma", 10f, (FontStyle)1);
		linkLabelGetOtherVeraion.LinkBehavior = (LinkBehavior)2;
		linkLabelGetOtherVeraion.LinkColor = Color.OrangeRed;
		((Control)linkLabelGetOtherVeraion).Location = new Point(0, 163);
		((Control)linkLabelGetOtherVeraion).Name = "linkLabelGetOtherVeraion";
		linkLabelGetOtherVeraion.Padding = new Padding(5);
		((Control)linkLabelGetOtherVeraion).Size = new Size(1419, 27);
		((Control)linkLabelGetOtherVeraion).TabIndex = 0;
		linkLabelGetOtherVeraion.TabStop = true;
		((Control)linkLabelGetOtherVeraion).Text = "مشتری گرامی: این نسخه از نرم افزار فاقد پشتیبانی است. جهت مشاهده نسخه های مشمول پشتیبانی، کلیک کنید";
		((Label)linkLabelGetOtherVeraion).TextAlign = (ContentAlignment)32;
		((Control)linkLabelGetOtherVeraion).Visible = false;
		linkLabelGetOtherVeraion.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		((Control)BtnShowEventLog).AccessibleRole = (AccessibleRole)43;
		((Control)BtnShowEventLog).Anchor = (AnchorStyles)9;
		((Control)BtnShowEventLog).BackColor = Color.Transparent;
		BtnShowEventLog.ColorTable = (eButtonColor)8;
		BtnShowEventLog.FadeEffect = false;
		((Control)BtnShowEventLog).Font = new Font("Tahoma", 10f, (FontStyle)1);
		BtnShowEventLog.Image = (Image)componentResourceManager.GetObject("BtnShowEventLog.Image");
		((Control)BtnShowEventLog).Location = new Point(1428, 234);
		((Control)BtnShowEventLog).Name = "BtnShowEventLog";
		BtnShowEventLog.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(2);
		((Control)BtnShowEventLog).Size = new Size(30, 28);
		((PopupItemControl)BtnShowEventLog).Style = (eDotNetBarStyle)9;
		((Control)BtnShowEventLog).TabIndex = 139;
		BtnShowEventLog.Tooltip = "نمایش رخدادهای خطای لاگ ویندوز";
		((Control)BtnShowEventLog).Click += BtnShowEventLog_Click;
		((Control)BtnPayamak).AccessibleRole = (AccessibleRole)43;
		((Control)BtnPayamak).Anchor = (AnchorStyles)9;
		((Control)BtnPayamak).BackColor = Color.Transparent;
		BtnPayamak.ColorTable = (eButtonColor)8;
		BtnPayamak.FadeEffect = false;
		((Control)BtnPayamak).Font = new Font("Tahoma", 10f, (FontStyle)1);
		BtnPayamak.Image = (Image)(object)Resources.PayamakDisabled;
		BtnPayamak.ImageFixedSize = new Size(30, 28);
		((Control)BtnPayamak).Location = new Point(1428, 168);
		((Control)BtnPayamak).Name = "BtnPayamak";
		BtnPayamak.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(2);
		((Control)BtnPayamak).Size = new Size(30, 28);
		((PopupItemControl)BtnPayamak).Style = (eDotNetBarStyle)9;
		((Control)BtnPayamak).TabIndex = 140;
		BtnPayamak.Tooltip = "سرویس پیامک فعال نیست";
		((Control)BtnSignalR).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSignalR).Anchor = (AnchorStyles)9;
		((Control)BtnSignalR).BackColor = Color.Transparent;
		BtnSignalR.ColorTable = (eButtonColor)8;
		BtnSignalR.FadeEffect = false;
		((Control)BtnSignalR).Font = new Font("Tahoma", 10f, (FontStyle)1);
		BtnSignalR.Image = (Image)(object)Resources.SignalRDisabled;
		BtnSignalR.ImageFixedSize = new Size(28, 28);
		((Control)BtnSignalR).Location = new Point(1428, 199);
		((Control)BtnSignalR).Name = "BtnSignalR";
		BtnSignalR.Shape = (ShapeDescriptor)new RoundRectangleShapeDescriptor(2);
		((Control)BtnSignalR).Size = new Size(30, 28);
		((PopupItemControl)BtnSignalR).Style = (eDotNetBarStyle)9;
		((Control)BtnSignalR).TabIndex = 141;
		BtnSignalR.Tooltip = "سرویس ارتباط با سرور فعال نیست";
		((Control)pictureBoxLoading).Anchor = (AnchorStyles)9;
		((Control)pictureBoxLoading).BackColor = SystemColors.Control;
		((Control)pictureBoxLoading).BackgroundImageLayout = (ImageLayout)3;
		pictureBoxLoading.BorderStyle = (BorderStyle)1;
		((Control)pictureBoxLoading).Location = new Point(1269, 163);
		((Control)pictureBoxLoading).Name = "pictureBoxLoading";
		((Control)pictureBoxLoading).Size = new Size(150, 150);
		pictureBoxLoading.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxLoading.TabIndex = 142;
		pictureBoxLoading.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1466, 730);
		((Control)this).ContextMenuStrip = contextMenuStrip1;
		((Control)this).Controls.Add((Control)(object)pictureBoxLoading);
		((Control)this).Controls.Add((Control)(object)monthCalendarX1);
		((Control)this).Controls.Add((Control)(object)BtnPayamak);
		((Control)this).Controls.Add((Control)(object)BtnShowEventLog);
		((Control)this).Controls.Add((Control)(object)ImgRecovery);
		((Control)this).Controls.Add((Control)(object)ImgBazsazi);
		((Control)this).Controls.Add((Control)(object)linkLabelGetOtherVeraion);
		((Control)this).Controls.Add((Control)(object)ucPayamakServiceStatus1);
		((Control)this).Controls.Add((Control)(object)ucNotificationViewer1);
		((Control)this).Controls.Add((Control)(object)ucLkExpireDisplay1);
		((Control)this).Controls.Add((Control)(object)TbxMonasebat);
		((Control)this).Controls.Add((Control)(object)btnNotifyMe);
		((Control)this).Controls.Add((Control)(object)DgridLastTaradods);
		((Control)this).Controls.Add((Control)(object)DgridReportDevices);
		((Control)this).Controls.Add((Control)(object)ucCacheStatus1);
		((Control)this).Controls.Add((Control)(object)LanguageLable);
		((Control)this).Controls.Add((Control)(object)ribbonControl1);
		((Control)this).Controls.Add((Control)(object)ucStatuse1);
		((Control)this).Controls.Add((Control)(object)ucMain1);
		((Control)this).Controls.Add((Control)(object)BtnSignalR);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FrmMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[نـــــرم افــزار جــــــامع حضـــــور غـیـاب و حـــقـوق دسـتــــمـزد نوین پرداز]*";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosing += new FormClosingEventHandler(FRMMain_FormClosing);
		((Form)this).Load += FRMMain_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMMain_KeyDown);
		ribbonControl1.ResumeLayout(false);
		((Control)ribbonControl1).PerformLayout();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((Control)ribbonPanel4).ResumeLayout(false);
		((Control)ribbonPanel2).ResumeLayout(false);
		((Control)ribbonPanel3).ResumeLayout(false);
		((Control)ribbonPanel1).ResumeLayout(false);
		((Control)ribbonPanel7).ResumeLayout(false);
		((Control)ribbonPanel8).ResumeLayout(false);
		((Control)ribbonPanel5).ResumeLayout(false);
		((Control)ribbonPanel6).ResumeLayout(false);
		((Control)NotifyContextMenue).ResumeLayout(false);
		((ISupportInitialize)ImgBazsazi).EndInit();
		((ISupportInitialize)ImgRecovery).EndInit();
		((ISupportInitialize)DgridReportDevices).EndInit();
		((ISupportInitialize)deviceEventsEntityBindingSource).EndInit();
		((ISupportInitialize)DgridLastTaradods).EndInit();
		((ISupportInitialize)logOnPersonelTopLastTaradodsBindingSource).EndInit();
		((ISupportInitialize)pictureBoxLoading).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
