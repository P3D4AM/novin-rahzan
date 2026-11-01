using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Assistance.Formatters;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.View.Taradod;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.AttPersonLeave;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmRptTaradodFull : BaseForm
{
	private Font _font;

	private CancellationTokenSource _tokenReloadGrid;

	private CancellationTokenSource _tokenReOrderGrid;

	private CancellationTokenSource _tokenShift;

	private CancellationTokenSource _tokenGetGharadad;

	private CancellationTokenSource _tokenStatuseTaradod;

	private CancellationTokenSource _tokenUpdateTaradodFull;

	private int _personId;

	private ColorDet _clsColor;

	private DateTime _date1 = DateTime.Now;

	private DateTime _date2 = DateTime.Now;

	private DateTime? _rowSelected;

	private TimeAttendanceDAL.Dal.FLRPerson.Person _clsPerson;

	private GharardadNew _clsGharardad;

	private readonly DataGridViewTextBoxColumn[] _lstDgv = (DataGridViewTextBoxColumn[])(object)new DataGridViewTextBoxColumn[8];

	private readonly DataGridViewTextBoxColumn[] _lstDgkh = (DataGridViewTextBoxColumn[])(object)new DataGridViewTextBoxColumn[8];

	private bool _loadClsTarardodFull;

	private bool _isFirstTimeLoad = true;

	private bool _isDisposed;

	private int lastSelectedCellIndex;

	private int lastSelectedrowIndex;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX LblEzafe;

	private LabelX LblHozor;

	private LabelX LblRows;

	private LabelX LblRow;

	private LabelX LblKasrsum;

	private LabelX LblhozorSum;

	private BindingSource personBindingSource;

	private ButtonX BtnPrint;

	private ButtonX Btnfish;

	private LabelX LblForCodeTafsil;

	private PictureBox PicPerson;

	private LabelX labelX1;

	private LabelX Lblkasr;

	private LinkLabel LblName;

	private LabelX labelX5;

	private LinkLabel LblMonth;

	private LinkLabel LblYear;

	private LinkLabel LblCodeTafsil;

	private ContextMenuStrip CMS;

	private ToolStripMenuItem CMS_Morakhasi_Daily;

	private ToolStripMenuItem CMS_GharadadList;

	private ToolStripMenuItem CMS_ShiftList;

	private ToolStripMenuItem CMS_PersonInfo;

	private ToolStripMenuItem CMS_DeleteAttendance;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem EditManagementToolStripMenuItem;

	private ToolStripMenuItem CMEK;

	private ToolStripMenuItem CMKK;

	private ToolStripMenuItem CMMission;

	private ToolStripMenuItem CMVacation;

	private ToolStripMenuItem CMFriday;

	private ToolStripMenuItem CMSNobatkari1;

	private ToolStripMenuItem CMSNobatkari2;

	private ToolStripMenuItem CMSNobatkari3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripMenuItem CMAbsent;

	private ToolStripMenuItem CMNight;

	private LabelX LblLeaveName;

	private LabelX LblLeaveValueDay;

	private LabelX LblShiftName1;

	private LabelX LblMissionName;

	private LabelX LblHolidayName;

	private LabelX LblNightWorkName;

	private LabelX LblShiftValue1;

	private LabelX LblMissionValue;

	private LabelX LblHolidayValue;

	private LabelX LblNightWorkValue;

	private ToolStripMenuItem CMS_InsertMorakhasi;

	private LabelX labelX7;

	private LabelX LblGheybat;

	private ToolStripMenuItem CMS_ChangeTypeTaradod;

	private LabelX labelX8;

	private LabelX LblLeaveValueHour;

	private LabelX labelX6;

	private ButtonX btnBack;

	private ButtonX btnNext;

	private LabelX labelX9;

	private LabelX labelX10;

	private LabelX labelX11;

	private LabelX labelX12;

	private LabelX labelX13;

	private LabelX labelX14;

	private LabelX labelX15;

	private LabelX labelX16;

	private LabelX LblTatilValue;

	private LabelX labelX17;

	private LabelX labelX18;

	private LabelX lblJarime;

	private ToolStripMenuItem CmsViewGharardadOfThisPerson;

	private LinkLabel LblGharardadId;

	private LabelX labelX19;

	private LabelX LblKarKardMozafi;

	private LabelX LblMoazzafi;

	private LabelX labelX21;

	private LabelX LblMoazzafiMonth;

	private LabelX LblShiftName3;

	private LabelX LblShiftValue3;

	private LabelX LblShiftName2;

	private LabelX LblShiftValue2;

	private LabelX labelX20;

	private LabelX labelX4;

	private Panel panel1;

	private ToolStripMenuItem CmsRequestedVacationHourly;

	private BindingSource TaradodFullDetViewbindingSource;

	private DataGridView DGrid;

	private ToolStripMenuItem CmsEditTaradod;

	private DataGridViewTextBoxColumn DG_Index;

	private DataGridViewTextBoxColumn DG_Dayofweek;

	private DataGridViewTextBoxColumn DG_TaradodStatusName;

	private DataGridViewTextBoxColumn DG_ShiftTime;

	private DataGridViewTextBoxColumn DG_Hozor;

	private DataGridViewTextBoxColumn DG_EzafeKar;

	private DataGridViewTextBoxColumn DG_KasreKar;

	private DataGridViewTextBoxColumn DG_ManulaEzafeKarTime;

	private DataGridViewTextBoxColumn DG_ManulaKasreKarTime;

	private DataGridViewTextBoxColumn DG_ManulaAbsentTime;

	private DataGridViewTextBoxColumn DG_ManulaVacationTime;

	private DataGridViewTextBoxColumn DG_ManulaMissionTime;

	private DataGridViewTextBoxColumn DG_ManulaNightTime;

	private DataGridViewTextBoxColumn DG_ManulaFridayTime;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime1;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime2;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime3;

	private DataGridViewTextBoxColumn DG_V1Time;

	private DataGridViewTextBoxColumn DG_K1Time;

	private DataGridViewTextBoxColumn DG_V2Time;

	private DataGridViewTextBoxColumn DG_K2Time;

	private DataGridViewTextBoxColumn DG_V3Time;

	private DataGridViewTextBoxColumn DG_K3Time;

	private DataGridViewTextBoxColumn DG_V4Time;

	private DataGridViewTextBoxColumn DG_K4Time;

	private DataGridViewTextBoxColumn DG_V5Time;

	private DataGridViewTextBoxColumn DG_K5Time;

	private DataGridViewTextBoxColumn DG_V6Time;

	private DataGridViewTextBoxColumn DG_K6Time;

	private DataGridViewTextBoxColumn DG_V7Time;

	private DataGridViewTextBoxColumn DG_K7Time;

	private DataGridViewTextBoxColumn DG_V8Time;

	private DataGridViewTextBoxColumn DG_K8Time;

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

	public DateTime CurrentRowDate
	{
		get
		{
			if (SelectedRow == null)
			{
				return DateTime.MinValue;
			}
			return SelectedRow.DateM;
		}
	}

	public TaradodFull ClsTaradodFull { get; private set; }

	public TaradodFullDetView SelectedRow { get; private set; }

	private LogOnPersonel SelectedTaradod
	{
		get
		{
			try
			{
				if (SelectedRow == null)
				{
					return null;
				}
				DataGridView dGrid = DGrid;
				int? num = ((dGrid != null) ? new int?(dGrid.SelectedCells[((BaseCollection)DGrid.SelectedCells).Count - 1].ColumnIndex) : null);
				if (num == 0)
				{
					return null;
				}
				if (num < ((DataGridViewBand)DG_V1Time).Index || num > ((DataGridViewBand)DG_K8Time).Index)
				{
					return null;
				}
				int? num2 = num - ((DataGridViewBand)DG_V1Time).Index + 1;
				if (num2 < 1 || num2 > 16)
				{
					return null;
				}
				object obj = num2 switch
				{
					1 => SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod, 
					2 => SelectedRow.CoupleTaradodCurrentDay[0].Ktaradod, 
					3 => SelectedRow.CoupleTaradodCurrentDay[1].Vtaradod, 
					4 => SelectedRow.CoupleTaradodCurrentDay[1].Ktaradod, 
					5 => SelectedRow.CoupleTaradodCurrentDay[2].Vtaradod, 
					6 => SelectedRow.CoupleTaradodCurrentDay[2].Ktaradod, 
					7 => SelectedRow.CoupleTaradodCurrentDay[3].Vtaradod, 
					8 => SelectedRow.CoupleTaradodCurrentDay[3].Ktaradod, 
					9 => SelectedRow.CoupleTaradodCurrentDay[4].Vtaradod, 
					10 => SelectedRow.CoupleTaradodCurrentDay[4].Ktaradod, 
					11 => SelectedRow.CoupleTaradodCurrentDay[5].Vtaradod, 
					12 => SelectedRow.CoupleTaradodCurrentDay[5].Ktaradod, 
					13 => SelectedRow.CoupleTaradodCurrentDay[6].Vtaradod, 
					14 => SelectedRow.CoupleTaradodCurrentDay[6].Ktaradod, 
					15 => SelectedRow.CoupleTaradodCurrentDay[7].Vtaradod, 
					16 => SelectedRow.CoupleTaradodCurrentDay[7].Ktaradod, 
					_ => null, 
				};
				((LogOnPersonel)obj).PersonId = ((PersonBase)SelectedRow.PersonInfo).Id;
				return (LogOnPersonel)obj;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return null;
		}
	}

	private LogOnPersonel SelectedTaradodOneBack
	{
		get
		{
			try
			{
				if (SelectedRow == null)
				{
					return null;
				}
				DataGridView dGrid = DGrid;
				int? num = ((dGrid != null) ? new int?(dGrid.SelectedCells[((BaseCollection)DGrid.SelectedCells).Count - 1].ColumnIndex) : null);
				if (num == 0)
				{
					return null;
				}
				if (num < ((DataGridViewBand)DG_K1Time).Index || num > ((DataGridViewBand)DG_K8Time).Index)
				{
					return null;
				}
				int? num2 = num - ((DataGridViewBand)DG_V1Time).Index + 1;
				if (num2 < 1 || num2 > 16)
				{
					return null;
				}
				return (num2 - 1) switch
				{
					1 => SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod, 
					2 => SelectedRow.CoupleTaradodCurrentDay[0].Ktaradod, 
					3 => SelectedRow.CoupleTaradodCurrentDay[1].Vtaradod, 
					4 => SelectedRow.CoupleTaradodCurrentDay[1].Ktaradod, 
					5 => SelectedRow.CoupleTaradodCurrentDay[2].Vtaradod, 
					6 => SelectedRow.CoupleTaradodCurrentDay[2].Ktaradod, 
					7 => SelectedRow.CoupleTaradodCurrentDay[3].Vtaradod, 
					8 => SelectedRow.CoupleTaradodCurrentDay[3].Ktaradod, 
					9 => SelectedRow.CoupleTaradodCurrentDay[4].Vtaradod, 
					10 => SelectedRow.CoupleTaradodCurrentDay[4].Ktaradod, 
					11 => SelectedRow.CoupleTaradodCurrentDay[5].Vtaradod, 
					12 => SelectedRow.CoupleTaradodCurrentDay[5].Ktaradod, 
					13 => SelectedRow.CoupleTaradodCurrentDay[6].Vtaradod, 
					14 => SelectedRow.CoupleTaradodCurrentDay[6].Ktaradod, 
					15 => SelectedRow.CoupleTaradodCurrentDay[7].Vtaradod, 
					_ => null, 
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return null;
		}
	}

	private CoupleTaradod SelectedCoupleTaradod
	{
		get
		{
			try
			{
				if (SelectedRow == null)
				{
					return null;
				}
				DataGridView dGrid = DGrid;
				int? num = ((dGrid != null) ? new int?(dGrid.SelectedCells[((BaseCollection)DGrid.SelectedCells).Count - 1].ColumnIndex) : null);
				if (num == 0)
				{
					return null;
				}
				if (num < ((DataGridViewBand)DG_V1Time).Index || num > ((DataGridViewBand)DG_K8Time).Index)
				{
					return null;
				}
				int? num2 = num - ((DataGridViewBand)DG_V1Time).Index + 1;
				return (!(num2 < 1) && !(num2 > 16)) ? (num2 switch
				{
					1 => SelectedRow.CoupleTaradodCurrentDay[0], 
					2 => SelectedRow.CoupleTaradodCurrentDay[0], 
					3 => SelectedRow.CoupleTaradodCurrentDay[1], 
					4 => SelectedRow.CoupleTaradodCurrentDay[1], 
					5 => SelectedRow.CoupleTaradodCurrentDay[2], 
					6 => SelectedRow.CoupleTaradodCurrentDay[2], 
					7 => SelectedRow.CoupleTaradodCurrentDay[3], 
					8 => SelectedRow.CoupleTaradodCurrentDay[3], 
					9 => SelectedRow.CoupleTaradodCurrentDay[4], 
					10 => SelectedRow.CoupleTaradodCurrentDay[4], 
					11 => SelectedRow.CoupleTaradodCurrentDay[5], 
					12 => SelectedRow.CoupleTaradodCurrentDay[5], 
					13 => SelectedRow.CoupleTaradodCurrentDay[6], 
					14 => SelectedRow.CoupleTaradodCurrentDay[6], 
					15 => SelectedRow.CoupleTaradodCurrentDay[7], 
					16 => SelectedRow.CoupleTaradodCurrentDay[7], 
					_ => null, 
				}) : null;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return null;
		}
	}

	public FrmRptTaradodFull(string contextGuid, Font font, DateTime date1, DateTime date2, int personId, DateTime? rowSelected = null)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)this).Font = font;
			((Control)DGrid).Font = font;
			if (rowSelected.HasValue)
			{
				_rowSelected = rowSelected;
			}
			_date1 = date1;
			_date2 = date2;
			_personId = personId;
			_loadClsTarardodFull = true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmRptTaradodFull(string contextGuid, Font font, TaradodFull pTaradodFull)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			if (((PersonBase)pTaradodFull.ClsPerson).Id <= 0)
			{
				throw new InvalidOperationException("pTaradodFull.ClsPerson.Id <= 0");
			}
			InitializeComponent();
			((Control)this).Font = _font;
			((Control)DGrid).Font = _font;
			_date1 = pTaradodFull.DateMin;
			_date2 = pTaradodFull.DateMax;
			ClsTaradodFull = pTaradodFull;
			_clsPerson = pTaradodFull.ClsPerson;
			_personId = ((PersonBase)_clsPerson).Id;
			_loadClsTarardodFull = false;
			ButtonX btnfish = Btnfish;
			ButtonX obj = btnBack;
			bool flag2 = (((Control)btnNext).Enabled = false);
			bool enabled = (((Control)obj).Enabled = flag2);
			((Control)btnfish).Enabled = enabled;
			ReleaseLableEvents();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void FrmRptTaradodFull_Load(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportTaradodPersonnel)
			{
				((Form)this).Close();
				return;
			}
			((Control)this).KeyDown += new KeyEventHandler(RPTTaradod_Full_KeyDown);
			_clsColor = new ColorDet();
			((ToolStripItem)CMS_DeleteAttendance).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodDelete;
			((ToolStripItem)CMS_Morakhasi_Daily).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain;
			((ToolStripItem)CMS_GharadadList).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain;
			((ToolStripItem)CMS_ShiftList).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftMain;
			((ToolStripItem)CMFriday).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkHoliday;
			((ToolStripItem)CMSNobatkari1).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkShift;
			((ToolStripItem)CMSNobatkari2).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkShift;
			((ToolStripItem)CMSNobatkari3).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkShift;
			((ToolStripItem)CMNight).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodWorkNight;
			((ToolStripItem)CMS_ChangeTypeTaradod).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodEditManagement;
			((Control)Btnfish).Enabled = (((Control)btnBack).Enabled ? UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.BillPriceMain : ((Control)btnBack).Enabled);
			((ToolStripItem)EditManagementToolStripMenuItem).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodEditManagement;
			DataGridViewCellStyle val = new DataGridViewCellStyle();
			TimeSpanFormatter formatProvider = new TimeSpanFormatter();
			val.FormatProvider = (IFormatProvider)formatProvider;
			val.Format = "hhmm";
			val.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ShiftTime).DefaultCellStyle = val;
			((DataGridViewBand)DG_Hozor).DefaultCellStyle = val;
			((DataGridViewBand)DG_EzafeKar).DefaultCellStyle = val;
			((DataGridViewBand)DG_KasreKar).DefaultCellStyle = val;
			((DataGridViewBand)DG_V1Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V2Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V3Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V4Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V5Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V6Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V7Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_V8Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K1Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K2Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K3Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K4Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K5Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K6Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K7Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_K8Time).DefaultCellStyle = val;
			((DataGridViewBand)DG_EzafeKar).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_KasreKar).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaEzafeKarTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaKasreKarTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaMissionTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaVacationTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaFridayTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaNightTime).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaShiftTime1).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaShiftTime2).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			((DataGridViewBand)DG_ManulaShiftTime3).DefaultCellStyle.Alignment = (DataGridViewContentAlignment)32;
			if (!_rowSelected.HasValue)
			{
				_rowSelected = _date1;
			}
			if (_clsPerson == null && _personId > 0)
			{
				await LoadClsPersonAsync(_personId);
			}
			if (ClsTaradodFull != null)
			{
				await ReloadGridAsync(loadClsTaradodFull: false);
			}
			else
			{
				await ReloadGridAsync();
			}
			InitMenuStripItems();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitMenuStripItems()
	{
		try
		{
			((ToolStripItem)CMS_DeleteAttendance).Text = Utilities.GetTranslate("[حذف تردد]");
			((ToolStripItem)CMS_ChangeTypeTaradod).Text = Utilities.GetTranslate("[اصلاح نوع تردد]");
			((ToolStripItem)CMS_InsertMorakhasi).Text = Utilities.GetTranslate("[ثبت مرخصی روزانه]");
			((ToolStripItem)CMS_Morakhasi_Daily).Text = Utilities.GetTranslate("[گزارش مرخصی روزانه]");
			((ToolStripItem)CMS_GharadadList).Text = Utilities.GetTranslate("[قراردادها]");
			((ToolStripItem)CmsViewGharardadOfThisPerson).Text = Utilities.GetTranslate("[مشاهده قرارداد این شخص]");
			((ToolStripItem)CMS_ShiftList).Text = Utilities.GetTranslate("[شیفت ها]");
			((ToolStripItem)CMS_PersonInfo).Text = Utilities.GetTranslate("[مشخصات شخص]");
			((ToolStripItem)EditManagementToolStripMenuItem).Text = Utilities.GetTranslate("[اصلاحات مدیریتی تردد]");
			((ToolStripItem)CMEK).Text = Utilities.GetTranslate("[اصلاح اضافه کار (مدیریتی)]");
			((ToolStripItem)CMKK).Text = Utilities.GetTranslate("[اصلاح کسر کار (مدیریتی)]");
			((ToolStripItem)CMMission).Text = Utilities.GetTranslate("[اصلاح ماموریت (مدیریتی)]");
			((ToolStripItem)CMVacation).Text = Utilities.GetTranslate("[اصلاح مرخصی (مدیریتی)]");
			((ToolStripItem)CMFriday).Text = Utilities.GetTranslate("[اصلاح جمعه کاری (مدیریتی)]");
			((ToolStripItem)CMSNobatkari1).Text = Utilities.GetTranslate("[اصلاح نوبت کاری صبح (مدیریتی)]");
			((ToolStripItem)CMSNobatkari2).Text = Utilities.GetTranslate("[اصلاح نوبت کاری عصر(مدیریتی)]");
			((ToolStripItem)CMSNobatkari3).Text = Utilities.GetTranslate("[اصلاح نوبت کاری شب (مدیریتی)]");
			((ToolStripItem)CMAbsent).Text = Utilities.GetTranslate("[اصلاح غیبت (مدیریتی)]");
			((ToolStripItem)CMNight).Text = Utilities.GetTranslate("[اصلاح شب کاری (مدیریتی)]");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task ReloadGridAsync(bool loadClsTaradodFull = true)
	{
		try
		{
			_tokenReloadGrid?.Cancel();
			_tokenReloadGrid = new CancellationTokenSource();
			CancellationToken token = _tokenReloadGrid.Token;
			await Task.Delay(500);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			Task task = Task.Run(async delegate
			{
				_ = 2;
				try
				{
					await LoadClsGharardadAsync(((PersonBase)_clsPerson).Id, token);
					Stopwatch sw = Stopwatch.StartNew();
					if (loadClsTaradodFull)
					{
						await LoadClsTarardodFullAsync(token);
					}
					_ = sw.ElapsedMilliseconds;
					await FillGridAsync(ClsTaradodFull, token);
					_ = sw.ElapsedMilliseconds;
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					Log.Error(ex4, "");
				}
			});
			new Waiter("در حال پردازش ترددها", (Form)(object)this, task, false, (EnWaiterType)1, (Control)null, "ReloadGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFull.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void ReleaseLableEvents()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		LblYear.LinkClicked -= new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		LblName.LinkClicked -= new LinkLabelLinkClickedEventHandler(LblName_LinkClicked);
		LblMonth.LinkClicked -= new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		LblCodeTafsil.LinkClicked -= new LinkLabelLinkClickedEventHandler(LblName_LinkClicked);
	}

	private async Task SetTooltipLablesAsync(CancellationToken token)
	{
		try
		{
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
					((Control)(object)LblMoazzafi).SetTooltip("[ مدت زمان کارکرد در بازه شیفت]", "[کارکرد موظفی معادل است با]:", "[- ]", (eTooltipColor)3);
					((Control)(object)LblHozor).SetTooltip("[مدت زمان حضور در محیط کار]", "[کارکرد کل معادل است با]:", "[- ]", (eTooltipColor)3);
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadClsTarardodFullAsync(CancellationToken token)
	{
		try
		{
			ClsTaradodFull = await TaradodFull.CreateAsync(((CacheBaseForm)this).ContextGuid, _date1, _date2, _clsPerson, null, checkValidationVorodAndKHoroj: true, token);
		}
		catch (Exception ex)
		{
			if (!ex.Message.Contains("Cannot access a disposed object.\r\nObject name: 'FrmRptTaradodFull'"))
			{
				Log.Error(ex, "");
			}
		}
	}

	private async Task FillGridAsync(TaradodFull clsTaradodFull, CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested || clsTaradodFull?.ListTaradodDets == null || DGrid == null)
			{
				return;
			}
			int num = 1;
			foreach (TaradodFullDetView listTaradodDet in clsTaradodFull.ListTaradodDets)
			{
				listTaradodDet.Index = num++;
			}
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
						TaradodFullDetViewbindingSource.DataSource = Utilities.ToSortableBindingList<TaradodFullDetView>((IEnumerable<TaradodFullDetView>)clsTaradodFull.ListTaradodDets, default(CancellationToken));
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
			await RefreshLblAsync(token);
			await InitGridAsync(token);
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
				DGrid.CurrentCell = DGrid.Rows[lastSelectedrowIndex].Cells[lastSelectedCellIndex];
			});
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private async Task LoadPictureAsync(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return;
		}
		try
		{
			Image img = Image.FromFile(fileName);
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					PicPerson.SizeMode = (PictureBoxSizeMode)1;
					PicPerson.Image = img;
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task InitGridAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			token.ThrowIfCancellationRequested();
			_tokenReOrderGrid?.Cancel();
			_tokenReOrderGrid = new CancellationTokenSource();
			CancellationToken reOrderToken = _tokenReOrderGrid.Token;
			await SetDgridCollumnsAsync(token);
			await Task.WhenAll(SetDgridCellColorsAsync(reOrderToken), SetDgridToolTipsAsync(reOrderToken), SetTooltipLablesAsync(token));
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task SetDgridToolTipsAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(1000);
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
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Invalid comparison between Unknown and I4
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Invalid comparison between Unknown and I4
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Invalid comparison between Unknown and I4
				try
				{
					int num = 0;
					while (num < DGrid.Rows.Count && !token.IsCancellationRequested && !IsDisposed)
					{
						TaradodFullDetView taradodFullDetView = (TaradodFullDetView)TaradodFullDetViewbindingSource.List[num];
						DataGridViewCell obj = DGrid[((DataGridViewBand)DG_ShiftTime).Index, num];
						obj.ToolTipText += taradodFullDetView.GetShiftToolTip();
						StatuseTaradodEnumType? taradodStatus = taradodFullDetView.TaradodStatus;
						bool flag;
						if (taradodStatus.HasValue)
						{
							StatuseTaradodEnumType valueOrDefault = taradodStatus.GetValueOrDefault();
							if ((int)valueOrDefault == 5 || (int)valueOrDefault == 14)
							{
								flag = true;
								goto IL_00a2;
							}
						}
						flag = false;
						goto IL_00a2;
						IL_00a2:
						if (flag)
						{
							DataGridViewCell obj2 = DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num];
							obj2.ToolTipText = obj2.ToolTipText + "\r\n" + taradodFullDetView.TaradodStatusName;
						}
						else
						{
							taradodStatus = taradodFullDetView.TaradodStatus;
							if (taradodStatus.HasValue && (int)taradodStatus.GetValueOrDefault() == 6)
							{
								DataGridViewCell obj3 = DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num];
								obj3.ToolTipText = obj3.ToolTipText + "\r\n" + taradodFullDetView.TatilRasmiToolTip;
							}
						}
						DataGridViewCell val = DGrid[((DataGridViewBand)DG_Dayofweek).Index, num];
						val.ToolTipText = val.ToolTipText + "\r\n" + taradodFullDetView.MonasebatToolTip + "\r\nتاریخ میلادی:" + taradodFullDetView.DateM.ToShortDateString();
						DataGridViewCell obj4 = DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num];
						string toolTipText = obj4.ToolTipText;
						IEnumerable<AttPersonLeaveBusiness> personLeaves = taradodFullDetView.PersonLeaves;
						obj4.ToolTipText = toolTipText + "\r\n" + ((personLeaves != null && personLeaves.Any()) ? $"\r\n درخواست {taradodFullDetView.PersonLeaves.Last().QuiddityForDisplay} از {taradodFullDetView.PersonLeaves.Last().DateFrom:HH:mm} تا {taradodFullDetView.PersonLeaves.Last().DateTo:HH:mm}" : taradodFullDetView.GetHozorToolTip());
						if (((DataGridViewBand)DG_V1Time).Visible)
						{
							int lastTardodCellIndex = GetLastTardodCellIndex();
							int num2 = ((DataGridViewBand)DG_V1Time).Index;
							int num3 = 0;
							int num4 = 0;
							while (num2 <= lastTardodCellIndex)
							{
								DataGridViewCell obj5 = DGrid[num2, num];
								obj5.ToolTipText += taradodFullDetView.CoupleTaradodCurrentDay[num3].GetToolTip(token);
								if (num4 % 2 == 1)
								{
									num3++;
								}
								num2++;
								num4++;
							}
						}
						DataGridViewCell obj6 = DGrid[((DataGridViewBand)DG_KasreKar).Index, num];
						obj6.ToolTipText += ((taradodFullDetView.JarimeRoz > 0) ? Utilities.GetTranslate("[در این ستون، سلولهایی که رنگ صورتی اند شامل جریمه نیز هستند]") : "");
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaEzafeKarTime))
						{
							DataGridViewCell obj7 = DGrid[((DataGridViewBand)DG_ManulaEzafeKarTime).Index, num];
							obj7.ToolTipText += taradodFullDetView.ManualEzafeKarToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaKasreKarTime))
						{
							DataGridViewCell obj8 = DGrid[((DataGridViewBand)DG_ManulaKasreKarTime).Index, num];
							obj8.ToolTipText += taradodFullDetView.ManualKasreKarToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaVacationTime))
						{
							DataGridViewCell obj9 = DGrid[((DataGridViewBand)DG_ManulaVacationTime).Index, num];
							obj9.ToolTipText += taradodFullDetView.ManualVacationToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaMissionTime))
						{
							DataGridViewCell obj10 = DGrid[((DataGridViewBand)DG_ManulaMissionTime).Index, num];
							obj10.ToolTipText += taradodFullDetView.ManualMissinToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaShiftTime1))
						{
							DataGridViewCell obj11 = DGrid[((DataGridViewBand)DG_ManulaShiftTime1).Index, num];
							obj11.ToolTipText += taradodFullDetView.ManualShift1ToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaShiftTime2))
						{
							DataGridViewCell obj12 = DGrid[((DataGridViewBand)DG_ManulaShiftTime2).Index, num];
							obj12.ToolTipText += taradodFullDetView.ManualShift2ToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaShiftTime3))
						{
							DataGridViewCell obj13 = DGrid[((DataGridViewBand)DG_ManulaShiftTime3).Index, num];
							obj13.ToolTipText += taradodFullDetView.ManualShift3ToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaNightTime))
						{
							DataGridViewCell obj14 = DGrid[((DataGridViewBand)DG_ManulaNightTime).Index, num];
							obj14.ToolTipText += taradodFullDetView.ManualNightToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaFridayTime))
						{
							DataGridViewCell obj15 = DGrid[((DataGridViewBand)DG_ManulaFridayTime).Index, num];
							obj15.ToolTipText += taradodFullDetView.ManualFriDayToolTip;
						}
						if (!string.IsNullOrEmpty(taradodFullDetView.ManulaAbsentTime))
						{
							DataGridViewCell obj16 = DGrid[((DataGridViewBand)DG_ManulaAbsentTime).Index, num];
							obj16.ToolTipText += taradodFullDetView.ManualAbsentToolTip;
						}
						num++;
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task SetDgridCollumnsAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<TaradodFullDetView> listTaradodDets = ClsTaradodFull.ListTaradodDets;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			int sumshiftMinutes = listTaradodDets.Sum((TaradodFullDetView p) => p.SumShiftMinuts);
			double sumEzafeKar = listTaradodDets.Sum((TaradodFullDetView p) => p.EzafeKar.Value.TotalMinutes);
			double sumKasreKar = listTaradodDets.Sum((TaradodFullDetView p) => p.KasreKar.Value.TotalMinutes);
			bool hasManualEzafeKar = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaEzafeKarTime));
			bool hasManualKasreKar = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaKasreKarTime));
			bool hasManualVacation = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaVacationTime));
			bool hasManualMission = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaMissionTime));
			bool hasManualAbsent = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaAbsentTime));
			bool hasManualFriday = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaFridayTime));
			bool hasManualNight = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaNightTime));
			bool hasManualShift1 = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime1));
			bool hasManualShift2 = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime2));
			bool hasManualShift3 = listTaradodDets.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime3));
			bool haskhoroj1 = false;
			bool haskhoroj2 = false;
			bool haskhoroj3 = false;
			bool haskhoroj4 = false;
			bool haskhoroj5 = false;
			bool haskhoroj6 = false;
			bool haskhoroj7 = false;
			haskhoroj1 = listTaradodDets.Sum((TaradodFullDetView p) => p.K1Time.TotalMinutes) > 0.0;
			if (haskhoroj1)
			{
				haskhoroj2 = listTaradodDets.Sum((TaradodFullDetView p) => p.K2Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj2)
			{
				haskhoroj3 = listTaradodDets.Sum((TaradodFullDetView p) => p.K3Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj3)
			{
				haskhoroj4 = listTaradodDets.Sum((TaradodFullDetView p) => p.K4Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj4)
			{
				haskhoroj5 = listTaradodDets.Sum((TaradodFullDetView p) => p.K5Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj5)
			{
				haskhoroj6 = listTaradodDets.Sum((TaradodFullDetView p) => p.K6Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj6)
			{
				haskhoroj7 = listTaradodDets.Sum((TaradodFullDetView p) => p.K7Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj7)
			{
				listTaradodDets.Sum((TaradodFullDetView p) => p.K8Time.TotalMinutes);
			}
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
						((DataGridViewBand)DG_ShiftTime).Visible = sumshiftMinutes > 0;
						((DataGridViewBand)DG_EzafeKar).Visible = sumEzafeKar > 0.0;
						((DataGridViewBand)DG_KasreKar).Visible = sumKasreKar > 0.0;
						((DataGridViewBand)DG_ManulaEzafeKarTime).Visible = hasManualEzafeKar;
						((DataGridViewBand)DG_ManulaKasreKarTime).Visible = hasManualKasreKar;
						((DataGridViewBand)DG_ManulaVacationTime).Visible = hasManualVacation;
						((DataGridViewBand)DG_ManulaMissionTime).Visible = hasManualMission;
						((DataGridViewBand)DG_ManulaAbsentTime).Visible = hasManualAbsent;
						((DataGridViewBand)DG_ManulaFridayTime).Visible = hasManualFriday;
						((DataGridViewBand)DG_ManulaNightTime).Visible = hasManualNight;
						((DataGridViewBand)DG_ManulaShiftTime1).Visible = hasManualShift1;
						((DataGridViewBand)DG_ManulaShiftTime2).Visible = hasManualShift2;
						((DataGridViewBand)DG_ManulaShiftTime3).Visible = hasManualShift3;
						DataGridViewColumn obj = DGrid.Columns[((DataGridViewBand)DG_V2Time).Index];
						bool visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K2Time).Index]).Visible = haskhoroj1);
						((DataGridViewBand)obj).Visible = visible;
						DataGridViewColumn obj2 = DGrid.Columns[((DataGridViewBand)DG_V3Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K3Time).Index]).Visible = haskhoroj2);
						((DataGridViewBand)obj2).Visible = visible;
						DataGridViewColumn obj3 = DGrid.Columns[((DataGridViewBand)DG_V4Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K4Time).Index]).Visible = haskhoroj3);
						((DataGridViewBand)obj3).Visible = visible;
						DataGridViewColumn obj4 = DGrid.Columns[((DataGridViewBand)DG_V5Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K5Time).Index]).Visible = haskhoroj4);
						((DataGridViewBand)obj4).Visible = visible;
						DataGridViewColumn obj5 = DGrid.Columns[((DataGridViewBand)DG_V6Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K6Time).Index]).Visible = haskhoroj5);
						((DataGridViewBand)obj5).Visible = visible;
						DataGridViewColumn obj6 = DGrid.Columns[((DataGridViewBand)DG_V7Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K7Time).Index]).Visible = haskhoroj6);
						((DataGridViewBand)obj6).Visible = visible;
						DataGridViewColumn obj7 = DGrid.Columns[((DataGridViewBand)DG_V8Time).Index];
						visible = (((DataGridViewBand)DGrid.Columns[((DataGridViewBand)DG_K8Time).Index]).Visible = haskhoroj7);
						((DataGridViewBand)obj7).Visible = visible;
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadClsPersonAsync(int personId, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_clsPerson = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, personId, token);
			if (((PersonBase)_clsPerson).PicPath != null)
			{
				Task.Run(async delegate
				{
					await LoadPictureAsync(((PersonBase)_clsPerson).PicPath);
				});
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadClsGharardadAsync(int personId, CancellationToken token)
	{
		try
		{
			_clsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, personId, _date1, _date2, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private int GetLastTardodCellIndex()
	{
		try
		{
			if (((DataGridViewBand)DG_K8Time).Visible)
			{
				return ((DataGridViewBand)DG_K8Time).Index;
			}
			if (((DataGridViewBand)DG_V8Time).Visible)
			{
				return ((DataGridViewBand)DG_V8Time).Index;
			}
			if (((DataGridViewBand)DG_K7Time).Visible)
			{
				return ((DataGridViewBand)DG_K7Time).Index;
			}
			if (((DataGridViewBand)DG_V7Time).Visible)
			{
				return ((DataGridViewBand)DG_V7Time).Index;
			}
			if (((DataGridViewBand)DG_K6Time).Visible)
			{
				return ((DataGridViewBand)DG_K6Time).Index;
			}
			if (((DataGridViewBand)DG_V6Time).Visible)
			{
				return ((DataGridViewBand)DG_V6Time).Index;
			}
			if (((DataGridViewBand)DG_K5Time).Visible)
			{
				return ((DataGridViewBand)DG_K5Time).Index;
			}
			if (((DataGridViewBand)DG_V5Time).Visible)
			{
				return ((DataGridViewBand)DG_V5Time).Index;
			}
			if (((DataGridViewBand)DG_K4Time).Visible)
			{
				return ((DataGridViewBand)DG_K4Time).Index;
			}
			if (((DataGridViewBand)DG_V4Time).Visible)
			{
				return ((DataGridViewBand)DG_V4Time).Index;
			}
			if (((DataGridViewBand)DG_K3Time).Visible)
			{
				return ((DataGridViewBand)DG_K3Time).Index;
			}
			if (((DataGridViewBand)DG_V3Time).Visible)
			{
				return ((DataGridViewBand)DG_V3Time).Index;
			}
			if (((DataGridViewBand)DG_K2Time).Visible)
			{
				return ((DataGridViewBand)DG_K2Time).Index;
			}
			if (((DataGridViewBand)DG_V2Time).Visible)
			{
				return ((DataGridViewBand)DG_V2Time).Index;
			}
			if (((DataGridViewBand)DG_K1Time).Visible)
			{
				return ((DataGridViewBand)DG_K1Time).Index;
			}
			if (((DataGridViewBand)DG_V1Time).Visible)
			{
				return ((DataGridViewBand)DG_V1Time).Index;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return -1;
	}

	private async Task SetDgridCellColorsAsync(CancellationToken token)
	{
		try
		{
			List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
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
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Invalid comparison between Unknown and I4
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0251: Unknown result type (might be due to invalid IL or missing references)
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_0259: Invalid comparison between Unknown and I4
				//IL_021c: Unknown result type (might be due to invalid IL or missing references)
				//IL_025b: Unknown result type (might be due to invalid IL or missing references)
				//IL_025e: Invalid comparison between Unknown and I4
				//IL_0260: Unknown result type (might be due to invalid IL or missing references)
				//IL_0263: Invalid comparison between Unknown and I4
				//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d3: Invalid comparison between Unknown and I4
				//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0657: Unknown result type (might be due to invalid IL or missing references)
				//IL_045b: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					for (int num = 0; num < DGrid.Rows.Count; num++)
					{
						if (token.IsCancellationRequested || IsDisposed)
						{
							break;
						}
						TaradodFullDetView taradod = (TaradodFullDetView)TaradodFullDetViewbindingSource.List[num];
						if (taradod.TaradodStatus == (StatuseTaradodEnumType?)7 || taradod.TaradodStatus == (StatuseTaradodEnumType?)6 || taradod.TaradodStatus == (StatuseTaradodEnumType?)13 || taradod.TaradodStatus == (StatuseTaradodEnumType?)12)
						{
							DGrid[((DataGridViewBand)DG_Dayofweek).Index, num].Style.ForeColor = StringExtensions.ToColor(_clsColor.FridayColor, default(Color));
						}
						else if (!taradod.IsWorkday((EnWorkdayEnumType)1))
						{
							DGrid[((DataGridViewBand)DG_Dayofweek).Index, num].Style.ForeColor = Color.DarkGray;
						}
						DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.ForeColor = StringExtensions.ToColor(_clsColor.GetColor(taradod.TaradodStatus.GetValueOrDefault()), default(Color));
						StatuseTaradodEnumType? taradodStatus = taradod.TaradodStatus;
						if (taradodStatus.HasValue && (int)taradodStatus.GetValueOrDefault() == 6)
						{
							DGrid[((DataGridViewBand)DG_Dayofweek).Index, num].Style.ForeColor = StringExtensions.ToColor(_clsColor.GetColor(taradod.TaradodStatus.GetValueOrDefault()), default(Color));
						}
						taradodStatus = taradod.TaradodStatus;
						bool flag;
						if (taradodStatus.HasValue)
						{
							StatuseTaradodEnumType valueOrDefault = taradodStatus.GetValueOrDefault();
							if ((int)valueOrDefault == 5 || (int)valueOrDefault == 9 || (int)valueOrDefault == 14)
							{
								flag = true;
								goto IL_026d;
							}
						}
						flag = false;
						goto IL_026d;
						IL_026d:
						if (flag)
						{
							DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.BackColor = Color.LightGreen;
						}
						IEnumerable<AttPersonLeaveBusiness> personLeaves = taradod.PersonLeaves;
						if (personLeaves != null && personLeaves.Any())
						{
							if ((int)taradod.PersonLeaves.Last().Quiddity == 1)
							{
								DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.BackColor = Color.FromArgb(255, 223, 255, 236);
							}
							else
							{
								DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.BackColor = Color.LightGreen;
							}
						}
						if (((DataGridViewBand)DG_V1Time).Visible)
						{
							int lastTardodCellIndex = GetLastTardodCellIndex();
							int j = ((DataGridViewBand)DG_V1Time).Index;
							int i = 0;
							for (; j <= lastTardodCellIndex; j++)
							{
								DGrid[j, num].Style.BackColor = taradod.CoupleTaradodCurrentDay[i].BackColorOfCell;
								if ((j - ((DataGridViewBand)DG_V1Time).Index) % 2 == 0)
								{
									if (taradod.CoupleTaradodCurrentDay[i].ColorV.ToArgb() == ColorTranslator.FromHtml(new ColorDet().EditTaradodColor).ToArgb())
									{
										DataGridViewCell obj = DGrid[j, num];
										obj.ToolTipText += " تردد ویرایش شده دستی\r\n";
									}
									if (taradod.CoupleTaradodCurrentDay[i].ColorV.ToArgb() == ColorTranslator.FromHtml(new ColorDet().AddTaradodColor).ToArgb())
									{
										DataGridViewCell obj2 = DGrid[j, num];
										obj2.ToolTipText += " تردد اضافه شده دستی\r\n";
									}
									DeviceInfo deviceInfo = null;
									if (devices != null)
									{
										deviceInfo = devices.FirstOrDefault((DeviceInfo p) => p.Id == taradod.CoupleTaradodCurrentDay[i].Vtaradod.DeviceId_);
										if (deviceInfo != null && DGrid[j, num].Value.ToString() != "00:00:00")
										{
											DataGridViewCell obj3 = DGrid[j, num];
											obj3.ToolTipText = obj3.ToolTipText + "از " + deviceInfo.DeviceName + "\r\n";
										}
									}
									if (taradod.CoupleTaradodCurrentDay[i].ColorV.ToArgb() == Color.Black.ToArgb())
									{
										string htmlColor = "Black";
										if (deviceInfo != null)
										{
											htmlColor = deviceInfo.DeviceColor;
										}
										taradod.CoupleTaradodCurrentDay[i].ColorV = ColorTranslator.FromHtml(htmlColor);
									}
									DGrid[j, num].Style.ForeColor = taradod.CoupleTaradodCurrentDay[i].ColorV;
								}
								else
								{
									if (taradod.CoupleTaradodCurrentDay[i].ColorKh.ToArgb() == ColorTranslator.FromHtml(new ColorDet().EditTaradodColor).ToArgb())
									{
										DataGridViewCell obj4 = DGrid[j, num];
										obj4.ToolTipText += " تردد ویرایش شده دستی\r\n";
									}
									if (taradod.CoupleTaradodCurrentDay[i].ColorKh.ToArgb() == ColorTranslator.FromHtml(new ColorDet().AddTaradodColor).ToArgb())
									{
										DataGridViewCell obj5 = DGrid[j, num];
										obj5.ToolTipText += " تردد اضافه شده دستی\r\n";
									}
									DeviceInfo deviceInfo2 = null;
									if (devices != null)
									{
										deviceInfo2 = devices.FirstOrDefault((DeviceInfo p) => p.Id == taradod.CoupleTaradodCurrentDay[i].Ktaradod.DeviceId_);
										if (deviceInfo2 != null && DGrid[j, num].Value.ToString() != "00:00:00")
										{
											DataGridViewCell obj6 = DGrid[j, num];
											obj6.ToolTipText = obj6.ToolTipText + "از " + deviceInfo2.DeviceName + "\r\n";
										}
									}
									if (taradod.CoupleTaradodCurrentDay[i].ColorKh.ToArgb() == Color.Black.ToArgb())
									{
										string htmlColor2 = "Black";
										if (deviceInfo2 != null)
										{
											htmlColor2 = deviceInfo2.DeviceColor;
										}
										taradod.CoupleTaradodCurrentDay[i].ColorKh = ColorTranslator.FromHtml(htmlColor2);
									}
									DGrid[j, num].Style.ForeColor = taradod.CoupleTaradodCurrentDay[i].ColorKh;
									i++;
								}
							}
						}
						DGrid[((DataGridViewBand)DG_KasreKar).Index, num].Style.ForeColor = ((taradod.JarimeRoz > 0) ? Color.Pink : Color.DarkRed);
						DGrid[((DataGridViewBand)DG_Index).Index, num].Style.ForeColor = ((taradod.IsWorkday((EnWorkdayEnumType)1) && taradod.DateM.Date <= DateTime.Now.Date) ? Color.Black : Color.DarkGray);
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private string GetTimeFormat(int row, int col)
	{
		try
		{
			return Utilities.GetTimeFormat(DGrid[col, row].Value?.ToString().Trim()) ?? "00:00";
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return "";
	}

	private async Task RefreshLblAsync(CancellationToken token)
	{
		await ClsTaradodFull.CalcSummeryTaradodFullDataAsync(token);
		do
		{
			await Task.Delay(100);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
		}
		while (!((Control)this).IsHandleCreated);
		try
		{
			string missionDaily = $"ماموریت({ClsTaradodFull.MissionDaily}):";
			string countWorkdayHoghogh = $"کارکرد کل({ClsTaradodFull.CountWorkdayHoghogh}):";
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((Control)LblName).Text = ClsTaradodFull.FullName;
						((Control)LblGharardadId).Text = _clsGharardad?.No;
						((Control)LblCodeTafsil).Text = ClsTaradodFull.CodeTafsil.ToString();
						((Control)LblYear).Text = ClsTaradodFull.Year;
						((Control)LblMonth).Text = ClsTaradodFull.Month;
						((Control)LblEzafe).Text = ClsTaradodFull.SumEzafekarTime;
						((Control)Lblkasr).Text = ClsTaradodFull.SumKasrkarTime;
						((Control)LblGheybat).Text = ClsTaradodFull.CountGheybat.ToString();
						((Control)LblRows).Text = DGrid.Rows.Count.ToString();
						((Control)LblHozor).Text = ClsTaradodFull.SumKarkerdTime;
						((Control)LblMoazzafi).Text = ClsTaradodFull.SumMoazzafiTime;
						((Control)LblMoazzafiMonth).Text = ClsTaradodFull.SumShiftTime;
						((Control)LblhozorSum).Text = countWorkdayHoghogh;
						((Control)LblLeaveValueDay).Text = ClsTaradodFull.CountMorakhasi.ToString();
						((Control)LblLeaveValueHour).Text = ClsTaradodFull.SumWorkVacationTime;
						((Control)LblMissionName).Text = missionDaily;
						((Control)LblMissionValue).Text = ClsTaradodFull.SumWorkMissionTime;
						((Control)LblNightWorkValue).Text = ClsTaradodFull.SumWorkNightWorkTime;
						((Control)LblHolidayValue).Text = ClsTaradodFull.SumWorkHalioDayWorkTime;
						((Control)LblTatilValue).Text = ClsTaradodFull.SumWorkTatilWorkTime;
						((Control)lblJarime).Text = ClsTaradodFull.JarimeRoz.ToString();
						((Control)LblShiftValue1).Text = ClsTaradodFull.SumNobatKari1Time;
						((Control)LblShiftValue2).Text = ClsTaradodFull.SumNobatKari2Time;
						((Control)LblShiftValue3).Text = ClsTaradodFull.SumNobatKari3Time;
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task GotoMonth(int monthNumber)
	{
		try
		{
			string text = Calendar.MiladiToShamsi(_date1);
			text = text.Substring(0, 5) + monthNumber.ToString("00") + "/01";
			_date1 = Calendar.StartDayOfPersianMonth(text);
			_date2 = Calendar.EndDayOfPersianMonth(text);
			_date2 = _date2.AddDays(1.0).AddSeconds(-1.0);
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task ViewShiftAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			CacheBaseForm frm;
			if (_clsGharardad.ShiftId == -1)
			{
				DateTime endDate = Utilities.GotoEndYear(_date2);
				DateTime startDate = endDate.AddDays(-360.0);
				frm = (CacheBaseForm)(object)new FrmShiftProgrammable(((CacheBaseForm)this).ContextGuid, _font, _clsGharardad.ID, startDate, endDate);
			}
			else
			{
				string contextGuid = ((CacheBaseForm)this).ContextGuid;
				frm = (CacheBaseForm)(object)new FrmShift(contextGuid, await Shifts.GetAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.ShiftId), _font, isReadOnly: false);
			}
			await frm.ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void RPTTaradod_Full_KeyDown(object sender, KeyEventArgs e)
	{
		_ = 13;
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 71)
			{
				if ((int)keyCode <= 27)
				{
					if ((int)keyCode != 13)
					{
						if ((int)keyCode == 27)
						{
							e.Handled = true;
							((Form)this).DialogResult = (DialogResult)2;
							((Form)this).Close();
						}
					}
					else
					{
						e.Handled = true;
						((ToolStripDropDown)CMS).Show();
					}
				}
				else if ((int)keyCode != 70)
				{
					if ((int)keyCode == 71)
					{
						e.Handled = true;
						linkLabel1_LinkClicked(null, null);
					}
				}
				else
				{
					e.Handled = true;
					((PopupItemControl)Btnfish).PerformClick();
				}
			}
			else if ((int)keyCode <= 84)
			{
				if ((int)keyCode != 77)
				{
					switch (keyCode - 80)
					{
					case 3:
					{
						e.Handled = true;
						_tokenShift?.Cancel();
						_tokenShift = new CancellationTokenSource();
						CancellationToken token = _tokenShift.Token;
						await ViewShiftAsync(token);
						break;
					}
					case 0:
						if (e.Control)
						{
							((PopupItemControl)BtnPrint).PerformClick();
							break;
						}
						e.Handled = true;
						LblName_LinkClicked(null, null);
						break;
					case 4:
						e.Handled = true;
						LblYear_LinkClicked(null, null);
						break;
					}
				}
				else
				{
					e.Handled = true;
					_tokenStatuseTaradod?.Cancel();
					_tokenStatuseTaradod = new CancellationTokenSource();
					CancellationToken token2 = _tokenStatuseTaradod.Token;
					bool flag = DGrid.CurrentRow != null;
					if (flag)
					{
						flag = (int)(await ClsTaradodFull.ListTaradodDets[((DataGridViewBand)DGrid.CurrentRow).Index].GetStatuseTaradodAsync(token2)) == 3;
					}
					if (flag)
					{
						((ToolStripItem)CMS_InsertMorakhasi).PerformClick();
					}
				}
			}
			else if ((int)keyCode != 89)
			{
				switch (keyCode - 107)
				{
				case 0:
					e.Handled = true;
					((PopupItemControl)btnNext).PerformClick();
					break;
				case 2:
					e.Handled = true;
					((PopupItemControl)btnBack).PerformClick();
					break;
				case 5:
					e.Handled = true;
					await GotoMonth(1);
					break;
				case 6:
					e.Handled = true;
					await GotoMonth(2);
					break;
				case 7:
					e.Handled = true;
					await GotoMonth(3);
					break;
				case 8:
					e.Handled = true;
					await GotoMonth(4);
					break;
				case 9:
					e.Handled = true;
					await GotoMonth(5);
					break;
				case 10:
					e.Handled = true;
					await GotoMonth(6);
					break;
				case 11:
					e.Handled = true;
					await GotoMonth(7);
					break;
				case 12:
					e.Handled = true;
					await GotoMonth(8);
					break;
				case 13:
					e.Handled = true;
					await GotoMonth(9);
					break;
				case 14:
					e.Handled = true;
					await GotoMonth(10);
					break;
				case 15:
					e.Handled = true;
					await GotoMonth(11);
					break;
				case 16:
				{
					e.Handled = true;
					FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
					break;
				}
				}
			}
			else
			{
				e.Handled = true;
				LblYear_LinkClicked(null, null);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnNext_Click(object sender, EventArgs e)
	{
		try
		{
			string text = Calendar.MiladiToShamsi(_date1);
			text = Calendar.AddMonth(text, 1);
			_date1 = Calendar.StartDayOfPersianMonth(text);
			_date2 = Calendar.EndDayOfPersianMonth(text);
			_date2 = _date2.AddDays(1.0).AddSeconds(-1.0);
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			string text = Calendar.MiladiToCalture(_date1);
			text = Calendar.AddMonth(text, -1);
			_date1 = LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth(text, "00:00:00");
			_date2 = LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth(text, "23:59:59");
			_date2 = _date2.AddDays(1.0).AddSeconds(-1.0);
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void Btnfish_Click(object sender, EventArgs e)
	{
		if (!((Control)Btnfish).Enabled || !((Control)Btnfish).Visible)
		{
			return;
		}
		try
		{
			if (ClsTaradodFull != null)
			{
				Fish pFish = await Fish.FactoryAsync(((CacheBaseForm)this).ContextGuid, ClsTaradodFull);
				FrmFishHoghogh frm = new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, pFish, _font, isReadOnly: false);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Btnfish_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFull.cs", 1201);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			if (ClsTaradodFull != null)
			{
				List<ForReportTaradodFull> lstForReports = await ForReportTaradodFull.FillListAsync(((CacheBaseForm)this).ContextGuid, ClsTaradodFull);
				if (lstForReports != null)
				{
					List<object> lst = lstForReports.Cast<object>().ToList();
					FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)1, lst, null, 0, ((PersonBase)_clsPerson).PicPath);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
					string pDesc = "[چاپ گزارش کامل تردد] " + ClsTaradodFull.FullName + " [از تاریخ] : " + Calendar.MiladiToCalture(ClsTaradodFull.DateMin) + " [ToDate] : " + Calendar.MiladiToCalture(ClsTaradodFull.DateMax);
					await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, pDesc, "Taradod.TaradodReportTaradodPersonnel", lstForReports, "");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void LblYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_ = 4;
		try
		{
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, ((PersonBase)_clsPerson).Id, _date1, bShowForm: false, bUcDate: false, -2, bUcPerson: false, -1, bUcPersonTak: true, 1, bUcDateTak: true, 0, bUcTakDay: false, -4, bUcDesc: false, -5, (ReportFilterEnumType)9);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadClsPersonAsync(frm.PersonId);
				_date1 = frm.DateM1;
				_date2 = frm.DateM2;
				await ReloadGridAsync();
				string pDesc = "[تهیه گزارش کامل تردد جناب ] " + ClsTaradodFull.FullName + " [از تاریخ] : " + Calendar.MiladiToCalture(ClsTaradodFull.DateMin) + " [ToDate] : " + Calendar.MiladiToCalture(ClsTaradodFull.DateMax);
				await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, pDesc, "Taradod.TaradodReportTaradodPersonnel");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		((ToolStripItem)CmsViewGharardadOfThisPerson).PerformClick();
	}

	private void LblName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		((ToolStripItem)CMS_PersonInfo).PerformClick();
	}

	private void TaradodFullDetViewbindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			SelectedRow = (TaradodFullDetView)TaradodFullDetViewbindingSource.Current;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void ViewHistoryToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedTaradod == null)
			{
				return;
			}
			List<LogOnPersonel_audits> obj = await LogOnPersonel_audits.GetAllAsync(((CacheBaseForm)this).ContextGuid, SelectedTaradod.Id);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogOnPersonel_audits item in obj)
			{
				stringBuilder.AppendLine(item.updated_atSh + " " + item.Time);
			}
			Utility.MessageBoxShow(stringBuilder.ToString(), ((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		try
		{
			if (!e.FormattingApplied && e.CellStyle.FormatProvider is ICustomFormatter customFormatter)
			{
				if (((ConvertEventArgs)e).Value == null || ((ConvertEventArgs)e).Value.ToString() == "00:00:00")
				{
					((ConvertEventArgs)e).Value = "";
				}
				((ConvertEventArgs)e).Value = customFormatter.Format(e.CellStyle.Format, ((ConvertEventArgs)e).Value, e.CellStyle.FormatProvider);
				e.FormattingApplied = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void DGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		_ = 1;
		try
		{
			if (e.ColumnIndex == ((DataGridViewBand)DG_ShiftTime).Index)
			{
				_tokenShift?.Cancel();
				_tokenShift = new CancellationTokenSource();
				CancellationToken token = _tokenShift.Token;
				await ViewShiftAsync(token);
			}
			else
			{
				if (e.ColumnIndex < ((DataGridViewBand)DG_V1Time).Index && e.ColumnIndex > ((DataGridViewBand)DG_K8Time).Index)
				{
					return;
				}
				LogOnPersonel logOnPersonel = null;
				switch (e.ColumnIndex - ((DataGridViewBand)DG_V1Time).Index)
				{
				case 0:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod;
					break;
				case 1:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[0].Ktaradod;
					break;
				case 2:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[1].Vtaradod;
					break;
				case 3:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[1].Ktaradod;
					break;
				case 4:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[2].Vtaradod;
					break;
				case 5:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[2].Ktaradod;
					break;
				case 6:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[3].Vtaradod;
					break;
				case 7:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[3].Ktaradod;
					break;
				case 8:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[4].Vtaradod;
					break;
				case 9:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[4].Ktaradod;
					break;
				case 10:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[5].Vtaradod;
					break;
				case 11:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[5].Ktaradod;
					break;
				case 12:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[6].Vtaradod;
					break;
				case 13:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[6].Ktaradod;
					break;
				case 14:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[7].Vtaradod;
					break;
				case 15:
					logOnPersonel = SelectedRow.CoupleTaradodCurrentDay[7].Ktaradod;
					break;
				}
				if (logOnPersonel != null)
				{
					logOnPersonel.PersonId = ((PersonBase)SelectedRow.PersonInfo).Id;
					FRMEditTaradod fRMEditTaradod = new FRMEditTaradod(((CacheBaseForm)this).ContextGuid, logOnPersonel);
					((Form)fRMEditTaradod).ShowDialog((IWin32Window)(object)this);
					if ((int)((Form)fRMEditTaradod).DialogResult == 1)
					{
						lastSelectedCellIndex = e.ColumnIndex;
						lastSelectedrowIndex = e.RowIndex;
						await ReloadGridAsync();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void dataGridViewX1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && (int)((MouseEventArgs)e).Button == 2097152)
			{
				DGrid.CurrentCell = DGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
				((DataGridViewBand)DGrid.Rows[e.RowIndex]).Selected = true;
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void CMS_Opened(object sender, EventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_tokenStatuseTaradod?.Cancel();
			_tokenStatuseTaradod = new CancellationTokenSource();
			_ = _tokenStatuseTaradod.Token;
			((ToolStripItem)CMS_InsertMorakhasi).Visible = SelectedRow != null && SelectedRow.TaradodStatus == (StatuseTaradodEnumType?)3;
			if (SelectedTaradod != null && CMS_DeleteAttendance != null)
			{
				((ToolStripItem)CMS_DeleteAttendance).Visible = SelectedTaradod != null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_DeleteAttendance_Click(object sender, EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelSelf.Taradod.TaradodDelete)
		{
			FMessageBox.Show("متاسفانه شما مجوز حذف ندارید.", "حذف تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			if (SelectedTaradod != null)
			{
				CoupleTaradod selectedCoupleTaradod = SelectedCoupleTaradod;
				LogOnPersonel taradod = SelectedTaradod;
				if (selectedCoupleTaradod.IsTaradodVirtual)
				{
					string text = " [تردد مورد نظر از نوع] " + Utilities.GetDisplay<TypeTaradodEnumType>(selectedCoupleTaradod.TypeTaradod) + " [می باشد. نمی توانید آنرا تغییر دهید]";
					text += "\n [تردد قبلی یا بعدی را بررسی نمائید]";
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate(text), "", 3);
				}
				else if (taradod.Id > 0)
				{
					await taradod.DeleteAsync();
					await ReloadGridAsync();
					string pDesc = "[حذف یک تردد] : " + ((PersonBase)_clsPerson).FullName + " [تاریخ] : " + Calendar.MiladiToCalture(taradod.DateTimeLogM) + " [ساعت] : " + taradod.DateTimeLogM.ToString("HH:mm") + " [نوع تردد] : " + Utilities.GetDisplay<TypeTaradodEnumType>(taradod.TypeTaradod) + " [شناسه دستگاه] : " + taradod.DeviceId_ + " [کد سخت افزاری شخص] : " + taradod.PersonNo;
					await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, pDesc, "Taradod.TaradodDelete");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_ChangeTypeTaradod_Click(object sender, EventArgs e)
	{
		_tokenStatuseTaradod?.Cancel();
		_tokenStatuseTaradod = new CancellationTokenSource();
		CancellationToken token = _tokenStatuseTaradod.Token;
		if (!UserAttendance.SelectedUser.AccessLevelSelf.Taradod.TaradodInsert)
		{
			FMessageBox.Show("متاسفانه شما مجوز ویرایش ندارید.", "ویرایش تردد", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
			return;
		}
		try
		{
			LogOnPersonel selectedTaradod = SelectedTaradod;
			if (selectedTaradod == null)
			{
				return;
			}
			if ((int)selectedTaradod.IsVirtual == 6)
			{
				string text = "[تردد مورد نظر از نوع] " + Utilities.GetDisplay<SavedTaradodEnumType>(selectedTaradod.IsVirtual) + " [می باشد. نمی توانید آنرا تغییر دهید]";
				text += "\n [تردد قبلی یا بعدی را بررسی نمائید]";
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate(text), "", 3);
				return;
			}
			CoupleTaradod selectedCouple = SelectedCoupleTaradod;
			if (selectedCouple == null)
			{
				return;
			}
			Morakhasidaily morakhasidaily = await Morakhasidaily.GetAsync(((CacheBaseForm)this).ContextGuid, selectedTaradod.DateTimeLogM, _personId, token);
			if (selectedCouple.IsTaradodVirtual)
			{
				string text2 = "[تردد مورد نظر از نوع] " + Utilities.GetDisplay<TypeTaradodEnumType>(selectedTaradod.TypeTaradod) + " [می باشد. نمی توانید آنرا تغییر دهید]";
				text2 += "\n [تردد قبلی یا بعدی را بررسی نمائید]";
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate(text2), "", 3);
				return;
			}
			StatuseTaradodEnumType? taradodStatus = SelectedRow.TaradodStatus;
			bool flag;
			if (taradodStatus.HasValue)
			{
				StatuseTaradodEnumType valueOrDefault = taradodStatus.GetValueOrDefault();
				if ((int)valueOrDefault == 5 || (int)valueOrDefault == 14)
				{
					flag = true;
					goto IL_021c;
				}
			}
			flag = false;
			goto IL_021c;
			IL_021c:
			if (flag)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate("[با توجه به اینکه وضعیت این روز مرخصی روزانه یا ماموریت روزانه می باشد امکان تغییر نوع تردد وجود ندارد]"), "", 3);
				return;
			}
			if (_clsGharardad == null)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate("[با توجه به اینکه در این روز قرارداد ندارد امکان تغییر نوع تردد وجود ندارد]"), "", 3);
				return;
			}
			ShiftDetView shiftDet = await TaradodFullDet.GetShiftAsync(((CacheBaseForm)this).ContextGuid, selectedTaradod.DateTimeLogM, _clsGharardad.ID, _personId, calcEkWhenHasVacationOrMission: true, morakhasidaily?.Id ?? 0, null, token);
			if (shiftDet == null)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, "[با توجه به اینکه در این روز شیفت تعریف نشده است امکان تغییر نوع تردد وجود ندارد]\r\n" + Utilities.GetTranslate("بازه شیفت و بازه قرارداد را بررسی نمائید"), "", 3);
				return;
			}
			ShiftCodeing clsShiftCoding = shiftDet.ClsShiftCoding;
			if ((int)clsShiftCoding.TypeShift == 2 && (selectedTaradod == SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod || DGrid[DGrid.CurrentCell.ColumnIndex + 1, ((DataGridViewBand)DGrid.CurrentRow).Index].Value.ToString().Contains("00:00")))
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate("[با توجه به اینکه در شیفت شناور شروع و پایان شیفت مشخص نیست، امکان تغییر نوع تردد در شروع یا پایان ترددها را ندارید]"), "", 3);
				return;
			}
			if ((int)clsShiftCoding.TypeShift != 2 && (selectedTaradod.DateTimeLogM > clsShiftCoding.KhorojEnd || selectedTaradod.DateTimeLogM < clsShiftCoding.VorodEnd) && !shiftDet.IsDayTatilInTaghvim)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate("[این تردد خارج از بازه شیفت است و نمیتوانید نوع آنرا تغییر دهید ]"), "", 3);
				return;
			}
			string text3 = DGrid[DGrid.CurrentCell.ColumnIndex, ((DataGridViewBand)DGrid.CurrentRow).Index].Value?.ToString();
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "00:00";
			}
			string text4 = DGrid[DGrid.CurrentCell.ColumnIndex + 1, ((DataGridViewBand)DGrid.CurrentRow).Index].Value?.ToString();
			if (string.IsNullOrEmpty(text4))
			{
				text4 = "00:00";
			}
			bool flag2 = selectedTaradod.DateTimeLogM >= clsShiftCoding.Vorod;
			if (selectedTaradod == SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod && flag2)
			{
				text4 = text3;
				text3 = clsShiftCoding.Vorod.ToString("HH:mm");
			}
			if (text4.Contains("00:00"))
			{
				text4 = clsShiftCoding.Khoroj.ToString("HH:mm");
			}
			text3 = text3.Substring(0, 5);
			text4 = text4.Substring(0, 5);
			if (string.CompareOrdinal(text3.Substring(0, 5), text4.Substring(0, 5)) > 0)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate("احتمالا تردد مورد نظر بعد از پایان شیفت است. لذا امکان تغییر وضعیت وجود ندارد"), "", 3);
				return;
			}
			FrmGetDecision frm = new FrmGetDecision(((CacheBaseForm)this).ContextGuid, selectedTaradod.TypeTaradod, selectedTaradod.TypeTaradodVacationOrMission, text3, text4, shiftDet)
			{
				ShowVacation = flag2,
				ShowMission = flag2
			};
			if ((int)selectedCouple.Vtaradod.IsVirtual == 6 && selectedCouple.Ktaradod.DateTimeLogM == selectedTaradod.DateTimeLogM)
			{
				flag = (frm.ShowNightWork = false);
				frm.ShowShiftWork = flag;
			}
			if (selectedTaradod.DateTimeLogM == selectedCouple.Vtaradod.DateTimeLogM && selectedTaradod != SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod)
			{
				LogOnPersonel selectedTaradodOneBack = SelectedTaradodOneBack;
				if ((await TaradodFullDet.GetShiftAsync(((CacheBaseForm)this).ContextGuid, selectedTaradodOneBack.DateTimeLogM, _clsGharardad.ID, _personId, calcEkWhenHasVacationOrMission: true, morakhasidaily?.Id ?? 0, null, token)).ClsShiftCoding.StartShiftTime == shiftDet.ClsShiftCoding.StartShiftTime && (int)selectedTaradod.TypeTaradodVacationOrMission != 7 && (int)selectedTaradod.TypeTaradodVacationOrMission != 5 && (int)selectedTaradod.TypeTaradod != 3 && (int)selectedTaradod.TypeTaradod != 34)
				{
					flag = (frm.ShowVacation = false);
					frm.ShowMission = flag;
				}
			}
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			if ((int)((Form)frm).DialogResult != 1 || selectedTaradod.Id <= 0)
			{
				return;
			}
			flag = (int)selectedCouple.Vtaradod.IsVirtual == 6;
			TypeTaradodEnumType val;
			if (flag)
			{
				val = frm.TypeTaradodCalculateSelected;
				bool flag5 = (((int)val == 10 || val - 91 <= 2) ? true : false);
				flag = flag5;
			}
			if (flag)
			{
				string text5 = "[این جفت تردد از نوع انتقال از روز قبل می باشد]";
				text5 += "\n";
				text5 += "[جهت تغییر نوع این تردد (به جز مرخصی یا ماموریت) بایستی تردد ورود آخرین جفت تردد روز قبل را تغییر دهید]";
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, Utilities.GetTranslate(text5), "", 3);
				return;
			}
			if (_clsPerson != null)
			{
				string xdesc = "[ویرایش نوع تردد : جناب ] " + ((PersonBase)_clsPerson).CodeTafsil + " : " + ((PersonBase)_clsPerson).FullName + " [InDate] : " + Calendar.MiladiToCalture(selectedTaradod.DateTimeLogM) + "  [و در ساعت] : " + selectedTaradod.DateTimeLogM.ToString("HH:mm");
				if (selectedTaradod.TypeTaradod != frm.TypeTaradodCalculateSelected)
				{
					string[] obj = new string[6] { xdesc, " از ", null, null, null, null };
					val = selectedTaradod.TypeTaradod;
					obj[2] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
					obj[3] = " به ";
					val = frm.TypeTaradodCalculateSelected;
					obj[4] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
					obj[5] = " تغییر کرد";
					xdesc = string.Concat(obj);
				}
				if (selectedTaradod.TypeTaradodVacationOrMission != frm.TypeTaradodVacationOrMissionSelected)
				{
					string[] obj2 = new string[6] { xdesc, " از ", null, null, null, null };
					val = selectedTaradod.TypeTaradodVacationOrMission;
					obj2[2] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
					obj2[3] = " به ";
					val = frm.TypeTaradodVacationOrMissionSelected;
					obj2[4] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
					obj2[5] = " تغییر کرد";
					xdesc = string.Concat(obj2);
				}
				selectedTaradod.TypeTaradodVacationOrMission = frm.TypeTaradodVacationOrMissionSelected;
				selectedTaradod.TypeTaradod = frm.TypeTaradodCalculateSelected;
				await selectedTaradod.SaveAsync("rptTaradodFullDet1", default(CancellationToken), "CMS_ChangeTypeTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFull.cs");
				await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "Taradod.TaradodUpdate");
			}
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMS_ChangeTypeTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFull.cs", 1562);
		}
	}

	private async void CMS_Morakhasi_Daily_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (SelectedRow != null)
			{
				int num = int.Parse(Calendar.MiladiToCalture(SelectedRow.DateM).Substring(0, 4));
				DateTime date = Utilities.CalendarToMiladi($"{num}/01/01");
				DateTime date2 = Utilities.CalendarToMiladi($"{num}/12/{Calendar.EndDayOfMonth((short)12, num)}");
				FrmMorakhasiDailyMain frmMor = new FrmMorakhasiDailyMain(((CacheBaseForm)this).ContextGuid, _font, new List<int>(1) { ((PersonBase)_clsPerson).Id }, date, date2, "", isCalcSaati: false);
				await ((CacheBaseForm)frmMor).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmMor).Dispose();
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_GharadadList_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmGharardadList frm = new FrmGharardadList(((CacheBaseForm)this).ContextGuid, _font);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await ReloadGridAsync();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_ShiftList_Click(object sender, EventArgs e)
	{
		try
		{
			FrmShiftMain frmShiftMain = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: false);
			((Form)frmShiftMain).ShowDialog();
			((Component)(object)frmShiftMain).Dispose();
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_PersonInfo_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			FrmPersonel frm = new FrmPersonel(((CacheBaseForm)this).ContextGuid, _clsPerson, _font, isReadOnly: false);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadClsPersonAsync(((PersonBase)_clsPerson).Id);
				await ReloadGridAsync();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMEK_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.EzaferKar, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				_tokenUpdateTaradodFull?.Cancel();
				_tokenUpdateTaradodFull = new CancellationTokenSource();
				_ = _tokenUpdateTaradodFull.Token;
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMKK_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.KasrKar, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMMission_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.Mission, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMVacation_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.Vacation, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMFriday_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.Friday, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMSNobatkari1_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.NobatKari1, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMSNobatkari2_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.NobatKari2, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMSNobatkari3_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.NobatKari3, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMAbsent_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.Absent, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMNight_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(((CacheBaseForm)this).ContextGuid, EN_TypeAtt.Night, _clsPerson, CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1 && DGrid.CurrentRow != null)
			{
				await ReloadGridAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CmsViewGharardadOfThisPerson_Click(object sender, EventArgs e)
	{
		_ = 5;
		try
		{
			GharardadNew clsGharardad = _clsGharardad;
			if (clsGharardad != null && clsGharardad.ID > 0)
			{
				FrmGharardad frm = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, _clsGharardad);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
				_clsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.ID);
			}
			else
			{
				GharardadNew pGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)_clsPerson).Id, DateTime.Now);
				FrmGharardad frm = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, pGharardad, readOnly: true);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
				_clsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)_clsPerson).Id, DateTime.Now);
			}
			await ReloadGridAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CmsRequestedVacationHourly_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedRow == null)
			{
				return;
			}
			if (SelectedRow.PersonLeaves.Any())
			{
				foreach (AttPersonLeaveBusiness personLeaf in SelectedRow.PersonLeaves)
				{
					await ((CacheBaseForm)new frmAttPersonLeave(((CacheBaseForm)this).ContextGuid, personLeaf)).ShowAsync((IWin32Window)(object)this);
				}
			}
			else
			{
				MessageBoxFarsi.Show("در این روز درخواست مرخصی ساعتی ثبت نشده است");
			}
		}
		catch (Exception ex)
		{
			Log.Error<string>(ex.Message, "");
		}
	}

	private async void CMS_InsertMorakhasi_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (SelectedRow.TaradodStatus == (StatuseTaradodEnumType?)3)
			{
				FrmMorakhasiDaily frm = new FrmMorakhasiDaily(((CacheBaseForm)this).ContextGuid, SelectedRow.DateM, ((PersonBase)_clsPerson).Id);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await ReloadGridAsync();
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FrmRptTaradodFull_FormClosing(object sender, FormClosingEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_tokenShift?.Cancel();
				_tokenShift?.Dispose();
				_tokenShift = null;
				_tokenGetGharadad?.Cancel();
				_tokenGetGharadad?.Dispose();
				_tokenGetGharadad = null;
				_tokenReloadGrid?.Cancel();
				_tokenReloadGrid?.Dispose();
				_tokenReloadGrid = null;
				_tokenReOrderGrid?.Cancel();
				_tokenReOrderGrid?.Dispose();
				_tokenReOrderGrid = null;
				_tokenStatuseTaradod?.Cancel();
				_tokenStatuseTaradod?.Dispose();
				_tokenStatuseTaradod = null;
				_tokenUpdateTaradodFull?.Cancel();
				_tokenUpdateTaradodFull?.Dispose();
				_tokenUpdateTaradodFull = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void DGrid_Sorted(object sender, EventArgs e)
	{
		try
		{
			_tokenReOrderGrid?.Cancel();
			_tokenReOrderGrid = new CancellationTokenSource();
			CancellationToken token = _tokenReOrderGrid.Token;
			await Task.WhenAll(SetDgridCellColorsAsync(token), SetDgridToolTipsAsync(token));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async void CmsEditTaradod_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedTaradod != null)
			{
				FRMEditTaradod fRMEditTaradod = new FRMEditTaradod(((CacheBaseForm)this).ContextGuid, SelectedTaradod);
				((Form)fRMEditTaradod).ShowDialog((IWin32Window)(object)this);
				if ((int)((Form)fRMEditTaradod).DialogResult == 1)
				{
					await ReloadGridAsync();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && !DGrid.Rows[e.RowIndex].Cells[0].Selected)
			{
				DGrid.Rows[e.RowIndex].Cells[0].Selected = true;
				DGrid.Rows[e.RowIndex].Cells[1].Selected = true;
				DGrid.Rows[e.RowIndex].Cells[2].Selected = true;
			}
		}
		catch (Exception)
		{
		}
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
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
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
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Expected O, but got Unknown
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_1691: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Expected O, but got Unknown
		//IL_174e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1758: Expected O, but got Unknown
		//IL_17f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1800: Expected O, but got Unknown
		//IL_180d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1817: Expected O, but got Unknown
		//IL_1824: Unknown result type (might be due to invalid IL or missing references)
		//IL_182e: Expected O, but got Unknown
		//IL_3369: Unknown result type (might be due to invalid IL or missing references)
		//IL_3373: Expected O, but got Unknown
		//IL_3655: Unknown result type (might be due to invalid IL or missing references)
		//IL_365f: Expected O, but got Unknown
		//IL_3698: Unknown result type (might be due to invalid IL or missing references)
		//IL_3758: Unknown result type (might be due to invalid IL or missing references)
		//IL_3762: Expected O, but got Unknown
		//IL_379b: Unknown result type (might be due to invalid IL or missing references)
		//IL_38dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e6: Expected O, but got Unknown
		//IL_3a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a29: Expected O, but got Unknown
		//IL_3aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af6: Expected O, but got Unknown
		//IL_3bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb7: Expected O, but got Unknown
		//IL_3f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f1e: Expected O, but got Unknown
		//IL_3fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe3: Expected O, but got Unknown
		//IL_4b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b5b: Expected O, but got Unknown
		//IL_4b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b98: Expected O, but got Unknown
		components = new Container();
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
		DataGridViewCellStyle val13 = new DataGridViewCellStyle();
		DataGridViewCellStyle val14 = new DataGridViewCellStyle();
		DataGridViewCellStyle val15 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmRptTaradodFull));
		CMS = new ContextMenuStrip(components);
		CmsEditTaradod = new ToolStripMenuItem();
		CMS_DeleteAttendance = new ToolStripMenuItem();
		CMS_ChangeTypeTaradod = new ToolStripMenuItem();
		toolStripSeparator2 = new ToolStripSeparator();
		CmsRequestedVacationHourly = new ToolStripMenuItem();
		CMS_InsertMorakhasi = new ToolStripMenuItem();
		CMS_Morakhasi_Daily = new ToolStripMenuItem();
		CMS_GharadadList = new ToolStripMenuItem();
		CmsViewGharardadOfThisPerson = new ToolStripMenuItem();
		CMS_ShiftList = new ToolStripMenuItem();
		toolStripSeparator1 = new ToolStripSeparator();
		CMS_PersonInfo = new ToolStripMenuItem();
		toolStripSeparator3 = new ToolStripSeparator();
		EditManagementToolStripMenuItem = new ToolStripMenuItem();
		CMEK = new ToolStripMenuItem();
		CMKK = new ToolStripMenuItem();
		toolStripSeparator4 = new ToolStripSeparator();
		CMMission = new ToolStripMenuItem();
		CMVacation = new ToolStripMenuItem();
		toolStripSeparator5 = new ToolStripSeparator();
		CMFriday = new ToolStripMenuItem();
		CMSNobatkari1 = new ToolStripMenuItem();
		CMSNobatkari2 = new ToolStripMenuItem();
		CMSNobatkari3 = new ToolStripMenuItem();
		CMAbsent = new ToolStripMenuItem();
		CMNight = new ToolStripMenuItem();
		TaradodFullDetViewbindingSource = new BindingSource(components);
		personBindingSource = new BindingSource(components);
		panelEx1 = new PanelEx();
		DGrid = new DataGridView();
		DG_Index = new DataGridViewTextBoxColumn();
		DG_Dayofweek = new DataGridViewTextBoxColumn();
		DG_TaradodStatusName = new DataGridViewTextBoxColumn();
		DG_ShiftTime = new DataGridViewTextBoxColumn();
		DG_Hozor = new DataGridViewTextBoxColumn();
		DG_EzafeKar = new DataGridViewTextBoxColumn();
		DG_KasreKar = new DataGridViewTextBoxColumn();
		DG_ManulaEzafeKarTime = new DataGridViewTextBoxColumn();
		DG_ManulaKasreKarTime = new DataGridViewTextBoxColumn();
		DG_ManulaAbsentTime = new DataGridViewTextBoxColumn();
		DG_ManulaVacationTime = new DataGridViewTextBoxColumn();
		DG_ManulaMissionTime = new DataGridViewTextBoxColumn();
		DG_ManulaNightTime = new DataGridViewTextBoxColumn();
		DG_ManulaFridayTime = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime1 = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime2 = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime3 = new DataGridViewTextBoxColumn();
		DG_V1Time = new DataGridViewTextBoxColumn();
		DG_K1Time = new DataGridViewTextBoxColumn();
		DG_V2Time = new DataGridViewTextBoxColumn();
		DG_K2Time = new DataGridViewTextBoxColumn();
		DG_V3Time = new DataGridViewTextBoxColumn();
		DG_K3Time = new DataGridViewTextBoxColumn();
		DG_V4Time = new DataGridViewTextBoxColumn();
		DG_K4Time = new DataGridViewTextBoxColumn();
		DG_V5Time = new DataGridViewTextBoxColumn();
		DG_K5Time = new DataGridViewTextBoxColumn();
		DG_V6Time = new DataGridViewTextBoxColumn();
		DG_K6Time = new DataGridViewTextBoxColumn();
		DG_V7Time = new DataGridViewTextBoxColumn();
		DG_K7Time = new DataGridViewTextBoxColumn();
		DG_V8Time = new DataGridViewTextBoxColumn();
		DG_K8Time = new DataGridViewTextBoxColumn();
		panel1 = new Panel();
		labelX20 = new LabelX();
		labelX6 = new LabelX();
		labelX9 = new LabelX();
		labelX11 = new LabelX();
		labelX4 = new LabelX();
		labelX10 = new LabelX();
		labelX13 = new LabelX();
		labelX12 = new LabelX();
		labelX15 = new LabelX();
		labelX14 = new LabelX();
		labelX17 = new LabelX();
		LblShiftName3 = new LabelX();
		LblShiftValue3 = new LabelX();
		LblShiftName2 = new LabelX();
		LblShiftValue2 = new LabelX();
		labelX21 = new LabelX();
		LblMoazzafiMonth = new LabelX();
		LblKarKardMozafi = new LabelX();
		LblMoazzafi = new LabelX();
		LblGharardadId = new LinkLabel();
		labelX19 = new LabelX();
		labelX18 = new LabelX();
		lblJarime = new LabelX();
		labelX16 = new LabelX();
		LblTatilValue = new LabelX();
		btnBack = new ButtonX();
		btnNext = new ButtonX();
		LblName = new LinkLabel();
		labelX5 = new LabelX();
		LblMonth = new LinkLabel();
		LblYear = new LinkLabel();
		LblCodeTafsil = new LinkLabel();
		labelX7 = new LabelX();
		LblGheybat = new LabelX();
		labelX1 = new LabelX();
		Lblkasr = new LabelX();
		PicPerson = new PictureBox();
		LblForCodeTafsil = new LabelX();
		BtnPrint = new ButtonX();
		Btnfish = new ButtonX();
		LblShiftName1 = new LabelX();
		LblMissionName = new LabelX();
		LblHolidayName = new LabelX();
		LblNightWorkName = new LabelX();
		labelX8 = new LabelX();
		LblLeaveName = new LabelX();
		LblShiftValue1 = new LabelX();
		LblMissionValue = new LabelX();
		LblKasrsum = new LabelX();
		LblHolidayValue = new LabelX();
		LblhozorSum = new LabelX();
		LblNightWorkValue = new LabelX();
		LblLeaveValueHour = new LabelX();
		LblRow = new LabelX();
		LblLeaveValueDay = new LabelX();
		LblEzafe = new LabelX();
		LblHozor = new LabelX();
		LblRows = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		((Control)CMS).SuspendLayout();
		((ISupportInitialize)TaradodFullDetViewbindingSource).BeginInit();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)PicPerson).BeginInit();
		((Control)this).SuspendLayout();
		((ToolStrip)CMS).ImageScalingSize = new Size(20, 20);
		((ToolStrip)CMS).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[14]
		{
			(ToolStripItem)CmsEditTaradod,
			(ToolStripItem)CMS_DeleteAttendance,
			(ToolStripItem)CMS_ChangeTypeTaradod,
			(ToolStripItem)toolStripSeparator2,
			(ToolStripItem)CmsRequestedVacationHourly,
			(ToolStripItem)CMS_InsertMorakhasi,
			(ToolStripItem)CMS_Morakhasi_Daily,
			(ToolStripItem)CMS_GharadadList,
			(ToolStripItem)CmsViewGharardadOfThisPerson,
			(ToolStripItem)CMS_ShiftList,
			(ToolStripItem)toolStripSeparator1,
			(ToolStripItem)CMS_PersonInfo,
			(ToolStripItem)toolStripSeparator3,
			(ToolStripItem)EditManagementToolStripMenuItem
		});
		((Control)CMS).Name = "CMS";
		((Control)CMS).RightToLeft = (RightToLeft)1;
		((Control)CMS).Size = new Size(243, 286);
		((ToolStripDropDown)CMS).Opened += CMS_Opened;
		((ToolStripItem)CmsEditTaradod).Name = "CmsEditTaradod";
		((ToolStripItem)CmsEditTaradod).Size = new Size(242, 22);
		((ToolStripItem)CmsEditTaradod).Text = "ویرایش تردد";
		((ToolStripItem)CmsEditTaradod).Click += CmsEditTaradod_Click;
		((ToolStripItem)CMS_DeleteAttendance).Name = "CMS_DeleteAttendance";
		((ToolStripItem)CMS_DeleteAttendance).Size = new Size(242, 22);
		((ToolStripItem)CMS_DeleteAttendance).Text = "[حذف تردد]";
		((ToolStripItem)CMS_DeleteAttendance).Click += CMS_DeleteAttendance_Click;
		((ToolStripItem)CMS_ChangeTypeTaradod).Name = "CMS_ChangeTypeTaradod";
		((ToolStripItem)CMS_ChangeTypeTaradod).Size = new Size(242, 22);
		((ToolStripItem)CMS_ChangeTypeTaradod).Text = "[اصلاح نوع تردد]";
		((ToolStripItem)CMS_ChangeTypeTaradod).Click += CMS_ChangeTypeTaradod_Click;
		((ToolStripItem)toolStripSeparator2).Name = "toolStripSeparator2";
		((ToolStripItem)toolStripSeparator2).Size = new Size(239, 6);
		((ToolStripItem)CmsRequestedVacationHourly).Name = "CmsRequestedVacationHourly";
		((ToolStripItem)CmsRequestedVacationHourly).Size = new Size(242, 22);
		((ToolStripItem)CmsRequestedVacationHourly).Text = "مشاهده درخواست مرخصی این روز";
		((ToolStripItem)CmsRequestedVacationHourly).Click += CmsRequestedVacationHourly_Click;
		((ToolStripItem)CMS_InsertMorakhasi).Name = "CMS_InsertMorakhasi";
		((ToolStripItem)CMS_InsertMorakhasi).Size = new Size(242, 22);
		((ToolStripItem)CMS_InsertMorakhasi).Text = "[ثبت مرخصی روزانه]";
		((ToolStripItem)CMS_InsertMorakhasi).Click += CMS_InsertMorakhasi_Click;
		((ToolStripItem)CMS_Morakhasi_Daily).Name = "CMS_Morakhasi_Daily";
		((ToolStripItem)CMS_Morakhasi_Daily).Size = new Size(242, 22);
		((ToolStripItem)CMS_Morakhasi_Daily).Text = "[گزارش مرخصی روزانه]";
		((ToolStripItem)CMS_Morakhasi_Daily).Click += CMS_Morakhasi_Daily_Click;
		((ToolStripItem)CMS_GharadadList).Name = "CMS_GharadadList";
		((ToolStripItem)CMS_GharadadList).Size = new Size(242, 22);
		((ToolStripItem)CMS_GharadadList).Text = "[قراردادها]";
		((ToolStripItem)CMS_GharadadList).Click += CMS_GharadadList_Click;
		((ToolStripItem)CmsViewGharardadOfThisPerson).Name = "CmsViewGharardadOfThisPerson";
		((ToolStripItem)CmsViewGharardadOfThisPerson).Size = new Size(242, 22);
		((ToolStripItem)CmsViewGharardadOfThisPerson).Text = "[مشاهده قرارداد شخص]";
		((ToolStripItem)CmsViewGharardadOfThisPerson).Click += CmsViewGharardadOfThisPerson_Click;
		((ToolStripItem)CMS_ShiftList).Name = "CMS_ShiftList";
		CMS_ShiftList.ShowShortcutKeys = false;
		((ToolStripItem)CMS_ShiftList).Size = new Size(242, 22);
		((ToolStripItem)CMS_ShiftList).Text = "[شیفت ها]";
		((ToolStripItem)CMS_ShiftList).Click += CMS_ShiftList_Click;
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(239, 6);
		((ToolStripItem)CMS_PersonInfo).Name = "CMS_PersonInfo";
		((ToolStripItem)CMS_PersonInfo).Size = new Size(242, 22);
		((ToolStripItem)CMS_PersonInfo).Text = "[مشخصات شخص]";
		((ToolStripItem)CMS_PersonInfo).Click += CMS_PersonInfo_Click;
		((ToolStripItem)toolStripSeparator3).Name = "toolStripSeparator3";
		((ToolStripItem)toolStripSeparator3).Size = new Size(239, 6);
		((ToolStripDropDownItem)EditManagementToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[12]
		{
			(ToolStripItem)CMEK,
			(ToolStripItem)CMKK,
			(ToolStripItem)toolStripSeparator4,
			(ToolStripItem)CMMission,
			(ToolStripItem)CMVacation,
			(ToolStripItem)toolStripSeparator5,
			(ToolStripItem)CMFriday,
			(ToolStripItem)CMSNobatkari1,
			(ToolStripItem)CMSNobatkari2,
			(ToolStripItem)CMSNobatkari3,
			(ToolStripItem)CMAbsent,
			(ToolStripItem)CMNight
		});
		((ToolStripItem)EditManagementToolStripMenuItem).Name = "EditManagementToolStripMenuItem";
		((ToolStripItem)EditManagementToolStripMenuItem).Size = new Size(242, 22);
		((ToolStripItem)EditManagementToolStripMenuItem).Text = "[اصلاحات مدیریتی تردد]";
		((ToolStripItem)CMEK).Name = "CMEK";
		((ToolStripItem)CMEK).Size = new Size(242, 22);
		((ToolStripItem)CMEK).Text = "[اصلاح اضافه کار (مدیریتی)]";
		((ToolStripItem)CMEK).Click += CMEK_Click;
		((ToolStripItem)CMKK).Name = "CMKK";
		((ToolStripItem)CMKK).Size = new Size(242, 22);
		((ToolStripItem)CMKK).Text = "[اصلاح کسر کار (مدیریتی)]";
		((ToolStripItem)CMKK).Click += CMKK_Click;
		((ToolStripItem)toolStripSeparator4).Name = "toolStripSeparator4";
		((ToolStripItem)toolStripSeparator4).Size = new Size(239, 6);
		((ToolStripItem)CMMission).Name = "CMMission";
		((ToolStripItem)CMMission).Size = new Size(242, 22);
		((ToolStripItem)CMMission).Text = "[اصلاح ماموریت (مدیریتی)]";
		((ToolStripItem)CMMission).Click += CMMission_Click;
		((ToolStripItem)CMVacation).Name = "CMVacation";
		((ToolStripItem)CMVacation).Size = new Size(242, 22);
		((ToolStripItem)CMVacation).Text = "[اصلاح مرخصی (مدیریتی)]";
		((ToolStripItem)CMVacation).Click += CMVacation_Click;
		((ToolStripItem)toolStripSeparator5).Name = "toolStripSeparator5";
		((ToolStripItem)toolStripSeparator5).Size = new Size(239, 6);
		((ToolStripItem)CMFriday).Name = "CMFriday";
		((ToolStripItem)CMFriday).Size = new Size(242, 22);
		((ToolStripItem)CMFriday).Text = "[اصلاح جمعه کاری (مدیریتی)]";
		((ToolStripItem)CMFriday).Click += CMFriday_Click;
		((ToolStripItem)CMSNobatkari1).Name = "CMSNobatkari1";
		((ToolStripItem)CMSNobatkari1).Size = new Size(242, 22);
		((ToolStripItem)CMSNobatkari1).Text = "[اصلاح نوبت کاری صبح (مدیریتی)]";
		((ToolStripItem)CMSNobatkari1).Click += CMSNobatkari1_Click;
		((ToolStripItem)CMSNobatkari2).Name = "CMSNobatkari2";
		((ToolStripItem)CMSNobatkari2).Size = new Size(242, 22);
		((ToolStripItem)CMSNobatkari2).Text = "[اصلاح نوبت کاری عصر(مدیریتی)]";
		((ToolStripItem)CMSNobatkari2).Click += CMSNobatkari2_Click;
		((ToolStripItem)CMSNobatkari3).Name = "CMSNobatkari3";
		((ToolStripItem)CMSNobatkari3).Size = new Size(242, 22);
		((ToolStripItem)CMSNobatkari3).Text = "[اصلاح نوبت کاری شب (مدیریتی)]";
		((ToolStripItem)CMSNobatkari3).Click += CMSNobatkari3_Click;
		((ToolStripItem)CMAbsent).Name = "CMAbsent";
		((ToolStripItem)CMAbsent).Size = new Size(242, 22);
		((ToolStripItem)CMAbsent).Text = "[اصلاح غیبت (مدیریتی)]";
		((ToolStripItem)CMAbsent).Click += CMAbsent_Click;
		((ToolStripItem)CMNight).Name = "CMNight";
		((ToolStripItem)CMNight).Size = new Size(242, 22);
		((ToolStripItem)CMNight).Text = "[اصلاح شب کاری (مدیریتی)]";
		((ToolStripItem)CMNight).Click += CMNight_Click;
		TaradodFullDetViewbindingSource.DataSource = typeof(TaradodFullDetView);
		TaradodFullDetViewbindingSource.CurrentChanged += TaradodFullDetViewbindingSource_CurrentChanged;
		personBindingSource.DataSource = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person);
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)panel1);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftName3);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftValue3);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftName2);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftValue2);
		((Control)panelEx1).Controls.Add((Control)(object)labelX21);
		((Control)panelEx1).Controls.Add((Control)(object)LblMoazzafiMonth);
		((Control)panelEx1).Controls.Add((Control)(object)LblKarKardMozafi);
		((Control)panelEx1).Controls.Add((Control)(object)LblMoazzafi);
		((Control)panelEx1).Controls.Add((Control)(object)LblGharardadId);
		((Control)panelEx1).Controls.Add((Control)(object)labelX19);
		((Control)panelEx1).Controls.Add((Control)(object)labelX18);
		((Control)panelEx1).Controls.Add((Control)(object)lblJarime);
		((Control)panelEx1).Controls.Add((Control)(object)labelX16);
		((Control)panelEx1).Controls.Add((Control)(object)LblTatilValue);
		((Control)panelEx1).Controls.Add((Control)(object)btnBack);
		((Control)panelEx1).Controls.Add((Control)(object)btnNext);
		((Control)panelEx1).Controls.Add((Control)(object)LblName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)LblMonth);
		((Control)panelEx1).Controls.Add((Control)(object)LblYear);
		((Control)panelEx1).Controls.Add((Control)(object)LblCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)LblGheybat);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)Lblkasr);
		((Control)panelEx1).Controls.Add((Control)(object)PicPerson);
		((Control)panelEx1).Controls.Add((Control)(object)LblForCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)Btnfish);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftName1);
		((Control)panelEx1).Controls.Add((Control)(object)LblMissionName);
		((Control)panelEx1).Controls.Add((Control)(object)LblHolidayName);
		((Control)panelEx1).Controls.Add((Control)(object)LblNightWorkName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)LblLeaveName);
		((Control)panelEx1).Controls.Add((Control)(object)LblShiftValue1);
		((Control)panelEx1).Controls.Add((Control)(object)LblMissionValue);
		((Control)panelEx1).Controls.Add((Control)(object)LblKasrsum);
		((Control)panelEx1).Controls.Add((Control)(object)LblHolidayValue);
		((Control)panelEx1).Controls.Add((Control)(object)LblhozorSum);
		((Control)panelEx1).Controls.Add((Control)(object)LblNightWorkValue);
		((Control)panelEx1).Controls.Add((Control)(object)LblLeaveValueHour);
		((Control)panelEx1).Controls.Add((Control)(object)LblRow);
		((Control)panelEx1).Controls.Add((Control)(object)LblLeaveValueDay);
		((Control)panelEx1).Controls.Add((Control)(object)LblEzafe);
		((Control)panelEx1).Controls.Add((Control)(object)LblHozor);
		((Control)panelEx1).Controls.Add((Control)(object)LblRows);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1238, 730);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		DGrid.AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[33]
		{
			(DataGridViewColumn)DG_Index,
			(DataGridViewColumn)DG_Dayofweek,
			(DataGridViewColumn)DG_TaradodStatusName,
			(DataGridViewColumn)DG_ShiftTime,
			(DataGridViewColumn)DG_Hozor,
			(DataGridViewColumn)DG_EzafeKar,
			(DataGridViewColumn)DG_KasreKar,
			(DataGridViewColumn)DG_ManulaEzafeKarTime,
			(DataGridViewColumn)DG_ManulaKasreKarTime,
			(DataGridViewColumn)DG_ManulaAbsentTime,
			(DataGridViewColumn)DG_ManulaVacationTime,
			(DataGridViewColumn)DG_ManulaMissionTime,
			(DataGridViewColumn)DG_ManulaNightTime,
			(DataGridViewColumn)DG_ManulaFridayTime,
			(DataGridViewColumn)DG_ManulaShiftTime1,
			(DataGridViewColumn)DG_ManulaShiftTime2,
			(DataGridViewColumn)DG_ManulaShiftTime3,
			(DataGridViewColumn)DG_V1Time,
			(DataGridViewColumn)DG_K1Time,
			(DataGridViewColumn)DG_V2Time,
			(DataGridViewColumn)DG_K2Time,
			(DataGridViewColumn)DG_V3Time,
			(DataGridViewColumn)DG_K3Time,
			(DataGridViewColumn)DG_V4Time,
			(DataGridViewColumn)DG_K4Time,
			(DataGridViewColumn)DG_V5Time,
			(DataGridViewColumn)DG_K5Time,
			(DataGridViewColumn)DG_V6Time,
			(DataGridViewColumn)DG_K6Time,
			(DataGridViewColumn)DG_V7Time,
			(DataGridViewColumn)DG_K7Time,
			(DataGridViewColumn)DG_V8Time,
			(DataGridViewColumn)DG_K8Time
		});
		((Control)DGrid).ContextMenuStrip = CMS;
		DGrid.DataSource = TaradodFullDetViewbindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val2;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 62);
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = SystemColors.Control;
		val3.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.WindowText;
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.HighlightText;
		val3.WrapMode = (DataGridViewTriState)1;
		DGrid.RowHeadersDefaultCellStyle = val3;
		DGrid.RowHeadersVisible = false;
		DGrid.RowHeadersWidthSizeMode = (DataGridViewRowHeadersWidthSizeMode)1;
		DGrid.RowTemplate.Height = 25;
		DGrid.SelectionMode = (DataGridViewSelectionMode)0;
		((Control)DGrid).Size = new Size(1232, 604);
		((Control)DGrid).TabIndex = 125;
		DGrid.CellDoubleClick += new DataGridViewCellEventHandler(DGrid_CellDoubleClick);
		DGrid.CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		DGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		DGrid.Sorted += DGrid_Sorted;
		((DataGridViewColumn)DG_Index).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Index).DataPropertyName = "DayofShMonth";
		((DataGridViewColumn)DG_Index).HeaderText = "تاریخ";
		((DataGridViewColumn)DG_Index).MinimumWidth = 15;
		((DataGridViewColumn)DG_Index).Name = "DG_Index";
		((DataGridViewBand)DG_Index).ReadOnly = true;
		((DataGridViewColumn)DG_Index).Width = 57;
		((DataGridViewColumn)DG_Dayofweek).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Dayofweek).DataPropertyName = "Dayofweek";
		((DataGridViewColumn)DG_Dayofweek).HeaderText = "روز هفته";
		((DataGridViewColumn)DG_Dayofweek).MinimumWidth = 50;
		((DataGridViewColumn)DG_Dayofweek).Name = "DG_Dayofweek";
		((DataGridViewBand)DG_Dayofweek).ReadOnly = true;
		((DataGridViewColumn)DG_Dayofweek).Width = 72;
		((DataGridViewColumn)DG_TaradodStatusName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DG_TaradodStatusName).DataPropertyName = "TaradodStatusName";
		((DataGridViewColumn)DG_TaradodStatusName).HeaderText = "وضعیت تردد";
		((DataGridViewColumn)DG_TaradodStatusName).MinimumWidth = 100;
		((DataGridViewColumn)DG_TaradodStatusName).Name = "DG_TaradodStatusName";
		((DataGridViewBand)DG_TaradodStatusName).ReadOnly = true;
		((DataGridViewColumn)DG_ShiftTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ShiftTime).DataPropertyName = "ShiftTime";
		((DataGridViewColumn)DG_ShiftTime).HeaderText = "شیفت";
		((DataGridViewColumn)DG_ShiftTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ShiftTime).Name = "DG_ShiftTime";
		((DataGridViewBand)DG_ShiftTime).ReadOnly = true;
		((DataGridViewBand)DG_ShiftTime).Visible = false;
		((DataGridViewColumn)DG_Hozor).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Hozor).DataPropertyName = "Hozor";
		((DataGridViewColumn)DG_Hozor).HeaderText = "حضور";
		((DataGridViewColumn)DG_Hozor).MinimumWidth = 30;
		((DataGridViewColumn)DG_Hozor).Name = "DG_Hozor";
		((DataGridViewBand)DG_Hozor).ReadOnly = true;
		((DataGridViewColumn)DG_Hozor).Width = 59;
		((DataGridViewColumn)DG_EzafeKar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_EzafeKar).DataPropertyName = "EzafeKar";
		val4.ForeColor = Color.FromArgb(0, 64, 0);
		((DataGridViewBand)DG_EzafeKar).DefaultCellStyle = val4;
		((DataGridViewColumn)DG_EzafeKar).HeaderText = "اضافه کار";
		((DataGridViewColumn)DG_EzafeKar).MinimumWidth = 30;
		((DataGridViewColumn)DG_EzafeKar).Name = "DG_EzafeKar";
		((DataGridViewBand)DG_EzafeKar).ReadOnly = true;
		((DataGridViewBand)DG_EzafeKar).Visible = false;
		((DataGridViewColumn)DG_KasreKar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_KasreKar).DataPropertyName = "KasreKar";
		val5.ForeColor = Color.Maroon;
		((DataGridViewBand)DG_KasreKar).DefaultCellStyle = val5;
		((DataGridViewColumn)DG_KasreKar).HeaderText = "کسر کار";
		((DataGridViewColumn)DG_KasreKar).MinimumWidth = 30;
		((DataGridViewColumn)DG_KasreKar).Name = "DG_KasreKar";
		((DataGridViewBand)DG_KasreKar).ReadOnly = true;
		((DataGridViewBand)DG_KasreKar).Visible = false;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).DataPropertyName = "ManulaEzafeKarTime";
		val6.ForeColor = Color.FromArgb(0, 64, 0);
		((DataGridViewBand)DG_ManulaEzafeKarTime).DefaultCellStyle = val6;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).HeaderText = "اضافه کار مد";
		((DataGridViewColumn)DG_ManulaEzafeKarTime).MinimumWidth = 45;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).Name = "DG_ManulaEzafeKarTime";
		((DataGridViewBand)DG_ManulaEzafeKarTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaEzafeKarTime).Visible = false;
		((DataGridViewColumn)DG_ManulaKasreKarTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaKasreKarTime).DataPropertyName = "ManulaKasreKarTime";
		val7.ForeColor = Color.Maroon;
		((DataGridViewBand)DG_ManulaKasreKarTime).DefaultCellStyle = val7;
		((DataGridViewColumn)DG_ManulaKasreKarTime).HeaderText = "کسرکار مد";
		((DataGridViewColumn)DG_ManulaKasreKarTime).MinimumWidth = 35;
		((DataGridViewColumn)DG_ManulaKasreKarTime).Name = "DG_ManulaKasreKarTime";
		((DataGridViewBand)DG_ManulaKasreKarTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaKasreKarTime).Visible = false;
		((DataGridViewColumn)DG_ManulaAbsentTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaAbsentTime).DataPropertyName = "ManulaAbsentTime";
		val8.ForeColor = Color.FromArgb(255, 128, 0);
		((DataGridViewBand)DG_ManulaAbsentTime).DefaultCellStyle = val8;
		((DataGridViewColumn)DG_ManulaAbsentTime).HeaderText = "غیبت مد";
		((DataGridViewColumn)DG_ManulaAbsentTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaAbsentTime).Name = "DG_ManulaAbsentTime";
		((DataGridViewBand)DG_ManulaAbsentTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaAbsentTime).Visible = false;
		((DataGridViewColumn)DG_ManulaVacationTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaVacationTime).DataPropertyName = "ManulaVacationTime";
		val9.ForeColor = Color.FromArgb(0, 192, 0);
		((DataGridViewBand)DG_ManulaVacationTime).DefaultCellStyle = val9;
		((DataGridViewColumn)DG_ManulaVacationTime).HeaderText = "مرخصی مد";
		((DataGridViewColumn)DG_ManulaVacationTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaVacationTime).Name = "DG_ManulaVacationTime";
		((DataGridViewBand)DG_ManulaVacationTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaVacationTime).Visible = false;
		((DataGridViewColumn)DG_ManulaMissionTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaMissionTime).DataPropertyName = "ManulaMissionTime";
		val10.ForeColor = Color.Green;
		((DataGridViewBand)DG_ManulaMissionTime).DefaultCellStyle = val10;
		((DataGridViewColumn)DG_ManulaMissionTime).HeaderText = "ماموریت مد";
		((DataGridViewColumn)DG_ManulaMissionTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaMissionTime).Name = "DG_ManulaMissionTime";
		((DataGridViewBand)DG_ManulaMissionTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaMissionTime).Visible = false;
		((DataGridViewColumn)DG_ManulaNightTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaNightTime).DataPropertyName = "ManulaNightTime";
		val11.ForeColor = Color.Navy;
		((DataGridViewBand)DG_ManulaNightTime).DefaultCellStyle = val11;
		((DataGridViewColumn)DG_ManulaNightTime).HeaderText = "شب کاری مد";
		((DataGridViewColumn)DG_ManulaNightTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaNightTime).Name = "DG_ManulaNightTime";
		((DataGridViewBand)DG_ManulaNightTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaNightTime).Visible = false;
		((DataGridViewColumn)DG_ManulaFridayTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaFridayTime).DataPropertyName = "ManulaFridayTime";
		val12.ForeColor = Color.FromArgb(192, 64, 0);
		((DataGridViewBand)DG_ManulaFridayTime).DefaultCellStyle = val12;
		((DataGridViewColumn)DG_ManulaFridayTime).HeaderText = "جمعه کاری مد";
		((DataGridViewColumn)DG_ManulaFridayTime).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaFridayTime).Name = "DG_ManulaFridayTime";
		((DataGridViewBand)DG_ManulaFridayTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaFridayTime).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaShiftTime1).DataPropertyName = "ManulaShiftTime1";
		val13.ForeColor = Color.Teal;
		((DataGridViewBand)DG_ManulaShiftTime1).DefaultCellStyle = val13;
		((DataGridViewColumn)DG_ManulaShiftTime1).HeaderText = "نوبت کاری1 مد";
		((DataGridViewColumn)DG_ManulaShiftTime1).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaShiftTime1).Name = "DG_ManulaShiftTime1";
		((DataGridViewBand)DG_ManulaShiftTime1).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime1).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaShiftTime2).DataPropertyName = "ManulaShiftTime2";
		val14.ForeColor = Color.Teal;
		((DataGridViewBand)DG_ManulaShiftTime2).DefaultCellStyle = val14;
		((DataGridViewColumn)DG_ManulaShiftTime2).HeaderText = "نوبت کاری2 مد";
		((DataGridViewColumn)DG_ManulaShiftTime2).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaShiftTime2).Name = "DG_ManulaShiftTime2";
		((DataGridViewBand)DG_ManulaShiftTime2).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime2).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime3).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaShiftTime3).DataPropertyName = "ManulaShiftTime3";
		val15.ForeColor = Color.Teal;
		((DataGridViewBand)DG_ManulaShiftTime3).DefaultCellStyle = val15;
		((DataGridViewColumn)DG_ManulaShiftTime3).HeaderText = "نوبت کاری3 مد";
		((DataGridViewColumn)DG_ManulaShiftTime3).MinimumWidth = 30;
		((DataGridViewColumn)DG_ManulaShiftTime3).Name = "DG_ManulaShiftTime3";
		((DataGridViewBand)DG_ManulaShiftTime3).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime3).Visible = false;
		((DataGridViewColumn)DG_V1Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V1Time).DataPropertyName = "V1Time";
		((DataGridViewColumn)DG_V1Time).HeaderText = "ورود1";
		((DataGridViewColumn)DG_V1Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V1Time).Name = "DG_V1Time";
		((DataGridViewBand)DG_V1Time).ReadOnly = true;
		((DataGridViewColumn)DG_V1Time).Width = 60;
		((DataGridViewColumn)DG_K1Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K1Time).DataPropertyName = "K1Time";
		((DataGridViewColumn)DG_K1Time).HeaderText = "خروج1";
		((DataGridViewColumn)DG_K1Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K1Time).Name = "DG_K1Time";
		((DataGridViewBand)DG_K1Time).ReadOnly = true;
		((DataGridViewColumn)DG_K1Time).Width = 64;
		((DataGridViewColumn)DG_V2Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V2Time).DataPropertyName = "V2Time";
		((DataGridViewColumn)DG_V2Time).HeaderText = "ورود2";
		((DataGridViewColumn)DG_V2Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V2Time).Name = "DG_V2Time";
		((DataGridViewBand)DG_V2Time).ReadOnly = true;
		((DataGridViewBand)DG_V2Time).Visible = false;
		((DataGridViewColumn)DG_K2Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K2Time).DataPropertyName = "K2Time";
		((DataGridViewColumn)DG_K2Time).HeaderText = "خروج2";
		((DataGridViewColumn)DG_K2Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K2Time).Name = "DG_K2Time";
		((DataGridViewBand)DG_K2Time).ReadOnly = true;
		((DataGridViewBand)DG_K2Time).Visible = false;
		((DataGridViewColumn)DG_V3Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V3Time).DataPropertyName = "V3Time";
		((DataGridViewColumn)DG_V3Time).HeaderText = "ورود3";
		((DataGridViewColumn)DG_V3Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V3Time).Name = "DG_V3Time";
		((DataGridViewBand)DG_V3Time).ReadOnly = true;
		((DataGridViewBand)DG_V3Time).Visible = false;
		((DataGridViewColumn)DG_K3Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K3Time).DataPropertyName = "K3Time";
		((DataGridViewColumn)DG_K3Time).HeaderText = "خروج3";
		((DataGridViewColumn)DG_K3Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K3Time).Name = "DG_K3Time";
		((DataGridViewBand)DG_K3Time).ReadOnly = true;
		((DataGridViewBand)DG_K3Time).Visible = false;
		((DataGridViewColumn)DG_V4Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V4Time).DataPropertyName = "V4Time";
		((DataGridViewColumn)DG_V4Time).HeaderText = "ورود4";
		((DataGridViewColumn)DG_V4Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V4Time).Name = "DG_V4Time";
		((DataGridViewBand)DG_V4Time).ReadOnly = true;
		((DataGridViewBand)DG_V4Time).Visible = false;
		((DataGridViewColumn)DG_K4Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K4Time).DataPropertyName = "K4Time";
		((DataGridViewColumn)DG_K4Time).HeaderText = "خروج4";
		((DataGridViewColumn)DG_K4Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K4Time).Name = "DG_K4Time";
		((DataGridViewBand)DG_K4Time).ReadOnly = true;
		((DataGridViewBand)DG_K4Time).Visible = false;
		((DataGridViewColumn)DG_V5Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V5Time).DataPropertyName = "V5Time";
		((DataGridViewColumn)DG_V5Time).HeaderText = "ورود5";
		((DataGridViewColumn)DG_V5Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V5Time).Name = "DG_V5Time";
		((DataGridViewBand)DG_V5Time).ReadOnly = true;
		((DataGridViewBand)DG_V5Time).Visible = false;
		((DataGridViewColumn)DG_K5Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K5Time).DataPropertyName = "K5Time";
		((DataGridViewColumn)DG_K5Time).HeaderText = "خروج5";
		((DataGridViewColumn)DG_K5Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K5Time).Name = "DG_K5Time";
		((DataGridViewBand)DG_K5Time).ReadOnly = true;
		((DataGridViewBand)DG_K5Time).Visible = false;
		((DataGridViewColumn)DG_V6Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V6Time).DataPropertyName = "V6Time";
		((DataGridViewColumn)DG_V6Time).HeaderText = "ورود6";
		((DataGridViewColumn)DG_V6Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V6Time).Name = "DG_V6Time";
		((DataGridViewBand)DG_V6Time).ReadOnly = true;
		((DataGridViewBand)DG_V6Time).Visible = false;
		((DataGridViewColumn)DG_K6Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K6Time).DataPropertyName = "K6Time";
		((DataGridViewColumn)DG_K6Time).HeaderText = "خروج6";
		((DataGridViewColumn)DG_K6Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K6Time).Name = "DG_K6Time";
		((DataGridViewBand)DG_K6Time).ReadOnly = true;
		((DataGridViewBand)DG_K6Time).Visible = false;
		((DataGridViewColumn)DG_V7Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V7Time).DataPropertyName = "V7Time";
		((DataGridViewColumn)DG_V7Time).HeaderText = "ورود7";
		((DataGridViewColumn)DG_V7Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V7Time).Name = "DG_V7Time";
		((DataGridViewBand)DG_V7Time).ReadOnly = true;
		((DataGridViewBand)DG_V7Time).Visible = false;
		((DataGridViewColumn)DG_K7Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K7Time).DataPropertyName = "K7Time";
		((DataGridViewColumn)DG_K7Time).HeaderText = "خروج7";
		((DataGridViewColumn)DG_K7Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K7Time).Name = "DG_K7Time";
		((DataGridViewBand)DG_K7Time).ReadOnly = true;
		((DataGridViewBand)DG_K7Time).Visible = false;
		((DataGridViewColumn)DG_V8Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V8Time).DataPropertyName = "V8Time";
		((DataGridViewColumn)DG_V8Time).HeaderText = "ورود8";
		((DataGridViewColumn)DG_V8Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_V8Time).Name = "DG_V8Time";
		((DataGridViewBand)DG_V8Time).ReadOnly = true;
		((DataGridViewBand)DG_V8Time).Visible = false;
		((DataGridViewColumn)DG_K8Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K8Time).DataPropertyName = "K8Time";
		((DataGridViewColumn)DG_K8Time).HeaderText = "خروج8";
		((DataGridViewColumn)DG_K8Time).MinimumWidth = 30;
		((DataGridViewColumn)DG_K8Time).Name = "DG_K8Time";
		((DataGridViewBand)DG_K8Time).ReadOnly = true;
		((DataGridViewBand)DG_K8Time).Visible = false;
		((Control)panel1).Anchor = (AnchorStyles)6;
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)labelX20);
		((Control)panel1).Controls.Add((Control)(object)labelX6);
		((Control)panel1).Controls.Add((Control)(object)labelX9);
		((Control)panel1).Controls.Add((Control)(object)labelX11);
		((Control)panel1).Controls.Add((Control)(object)labelX4);
		((Control)panel1).Controls.Add((Control)(object)labelX10);
		((Control)panel1).Controls.Add((Control)(object)labelX13);
		((Control)panel1).Controls.Add((Control)(object)labelX12);
		((Control)panel1).Controls.Add((Control)(object)labelX15);
		((Control)panel1).Controls.Add((Control)(object)labelX14);
		((Control)panel1).Controls.Add((Control)(object)labelX17);
		((Control)panel1).Location = new Point(3, 664);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(277, 65);
		((Control)panel1).TabIndex = 124;
		((Control)labelX20).Anchor = (AnchorStyles)6;
		((Control)labelX20).BackColor = Color.FromArgb(214, 214, 214);
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).ForeColor = Color.Black;
		((Control)labelX20).Location = new Point(202, 16);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(66, 14);
		((Control)labelX20).TabIndex = 122;
		((Control)labelX20).Text = "نوبت شب";
		labelX20.TextAlignment = (StringAlignment)1;
		((Control)labelX6).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).ForeColor = Color.Magenta;
		((Control)labelX6).Location = new Point(5, 33);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(116, 17);
		((Control)labelX6).TabIndex = 46;
		((Control)labelX6).Text = "[تردد ویرایش شده دستی]";
		((Control)labelX9).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).ForeColor = Color.Blue;
		((Control)labelX9).Location = new Point(5, 50);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(116, 14);
		((Control)labelX9).TabIndex = 89;
		((Control)labelX9).Text = "[تردد اضافه شده دستی]";
		((Control)labelX11).Anchor = (AnchorStyles)6;
		((Control)labelX11).BackColor = Color.FromArgb(153, 255, 155);
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).ForeColor = Color.Black;
		((Control)labelX11).Location = new Point(127, 33);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(68, 14);
		((Control)labelX11).TabIndex = 92;
		((Control)labelX11).Text = "[مرخصی]";
		labelX11.TextAlignment = (StringAlignment)1;
		((Control)labelX4).Anchor = (AnchorStyles)6;
		((Control)labelX4).BackColor = Color.FromArgb(255, 253, 199);
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).ForeColor = Color.Black;
		((Control)labelX4).Location = new Point(127, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(68, 14);
		((Control)labelX4).TabIndex = 120;
		((Control)labelX4).Text = "نوبت عصر";
		labelX4.TextAlignment = (StringAlignment)1;
		((Control)labelX10).Anchor = (AnchorStyles)6;
		((Control)labelX10).BackColor = Color.FromArgb(207, 255, 130);
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).ForeColor = Color.Black;
		((Control)labelX10).Location = new Point(127, 48);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(68, 14);
		((Control)labelX10).TabIndex = 93;
		((Control)labelX10).Text = "[ماموریت]";
		labelX10.TextAlignment = (StringAlignment)1;
		((Control)labelX13).Anchor = (AnchorStyles)6;
		((Control)labelX13).BackColor = Color.FromArgb(255, 214, 214);
		((Control)labelX13).BackgroundImageLayout = (ImageLayout)0;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = Color.Black;
		((Control)labelX13).Location = new Point(202, 30);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(66, 17);
		((Control)labelX13).TabIndex = 96;
		((Control)labelX13).Text = "[جمعه کاری]";
		labelX13.TextAlignment = (StringAlignment)1;
		((Control)labelX12).Anchor = (AnchorStyles)6;
		((Control)labelX12).BackColor = Color.FromArgb(191, 233, 255);
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).ForeColor = Color.Black;
		((Control)labelX12).Location = new Point(202, 2);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(66, 14);
		((Control)labelX12).TabIndex = 97;
		((Control)labelX12).Text = "نوبت صبح";
		labelX12.TextAlignment = (StringAlignment)1;
		((Control)labelX15).Anchor = (AnchorStyles)6;
		((Control)labelX15).BackColor = Color.FromArgb(171, 171, 171);
		((Control)labelX15).BackgroundImageLayout = (ImageLayout)0;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).ForeColor = Color.Black;
		((Control)labelX15).Location = new Point(127, 18);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(68, 14);
		((Control)labelX15).TabIndex = 100;
		((Control)labelX15).Text = "[شب کاری]";
		labelX15.TextAlignment = (StringAlignment)1;
		((Control)labelX14).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).ForeColor = Color.Red;
		((Control)labelX14).Location = new Point(5, 19);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(116, 14);
		((Control)labelX14).TabIndex = 101;
		((Control)labelX14).Text = "[تردد ناقص]";
		((Control)labelX17).Anchor = (AnchorStyles)6;
		((Control)labelX17).BackColor = Color.FromArgb(207, 143, 118);
		((Control)labelX17).BackgroundImageLayout = (ImageLayout)0;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).ForeColor = Color.Black;
		((Control)labelX17).Location = new Point(202, 47);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(66, 17);
		((Control)labelX17).TabIndex = 106;
		((Control)labelX17).Text = "[تعطیل کاری]";
		labelX17.TextAlignment = (StringAlignment)1;
		((Control)LblShiftName3).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftName3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftName3).Location = new Point(450, 712);
		((Control)LblShiftName3).Name = "LblShiftName3";
		((Control)LblShiftName3).Size = new Size(95, 14);
		((Control)LblShiftName3).TabIndex = 119;
		((Control)LblShiftName3).Text = "[نوبت کاری شب]:";
		LblShiftName3.TextAlignment = (StringAlignment)2;
		((Control)LblShiftValue3).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftValue3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftValue3).ForeColor = Color.DarkRed;
		((Control)LblShiftValue3).Location = new Point(411, 712);
		((Control)LblShiftValue3).Name = "LblShiftValue3";
		((Control)LblShiftValue3).Size = new Size(34, 14);
		((Control)LblShiftValue3).TabIndex = 118;
		((Control)LblShiftValue3).Text = "------";
		((Control)LblShiftName2).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftName2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftName2).Location = new Point(450, 692);
		((Control)LblShiftName2).Name = "LblShiftName2";
		((Control)LblShiftName2).Size = new Size(95, 14);
		((Control)LblShiftName2).TabIndex = 117;
		((Control)LblShiftName2).Text = "[نوبت کاری عصر]:";
		LblShiftName2.TextAlignment = (StringAlignment)2;
		((Control)LblShiftValue2).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftValue2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftValue2).ForeColor = Color.DarkRed;
		((Control)LblShiftValue2).Location = new Point(411, 692);
		((Control)LblShiftValue2).Name = "LblShiftValue2";
		((Control)LblShiftValue2).Size = new Size(34, 14);
		((Control)LblShiftValue2).TabIndex = 116;
		((Control)LblShiftValue2).Text = "------";
		((Control)labelX21).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Location = new Point(1016, 693);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(86, 14);
		((Control)labelX21).TabIndex = 115;
		((Control)labelX21).Text = "[موظفی ماه]:";
		labelX21.TextAlignment = (StringAlignment)2;
		((Control)LblMoazzafiMonth).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblMoazzafiMonth).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMoazzafiMonth).ForeColor = Color.DarkRed;
		((Control)LblMoazzafiMonth).Location = new Point(976, 693);
		((Control)LblMoazzafiMonth).Name = "LblMoazzafiMonth";
		((Control)LblMoazzafiMonth).Size = new Size(34, 14);
		((Control)LblMoazzafiMonth).TabIndex = 114;
		((Control)LblMoazzafiMonth).Text = "------";
		((Control)LblKarKardMozafi).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblKarKardMozafi).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKarKardMozafi).Location = new Point(1146, 692);
		((Control)LblKarKardMozafi).Name = "LblKarKardMozafi";
		((Control)LblKarKardMozafi).Size = new Size(86, 14);
		((Control)LblKarKardMozafi).TabIndex = 113;
		((Control)LblKarKardMozafi).Text = "[کارکرد موظفی]:";
		LblKarKardMozafi.TextAlignment = (StringAlignment)2;
		((Control)LblMoazzafi).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblMoazzafi).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMoazzafi).ForeColor = Color.DarkRed;
		((Control)LblMoazzafi).Location = new Point(1107, 692);
		((Control)LblMoazzafi).Name = "LblMoazzafi";
		((Control)LblMoazzafi).Size = new Size(34, 14);
		((Control)LblMoazzafi).TabIndex = 112;
		((Control)LblMoazzafi).Text = "------";
		((Control)LblGharardadId).Anchor = (AnchorStyles)9;
		((Control)LblGharardadId).ForeColor = Color.DarkRed;
		LblGharardadId.LinkBehavior = (LinkBehavior)3;
		LblGharardadId.LinkColor = Color.DarkRed;
		((Control)LblGharardadId).Location = new Point(782, 12);
		((Control)LblGharardadId).Name = "LblGharardadId";
		((Control)LblGharardadId).Size = new Size(40, 13);
		((Control)LblGharardadId).TabIndex = 111;
		LblGharardadId.TabStop = true;
		((Control)LblGharardadId).Text = "-------";
		((Label)LblGharardadId).TextAlign = (ContentAlignment)16;
		LblGharardadId.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(837, 8);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).RightToLeft = (RightToLeft)1;
		((Control)labelX19).Size = new Size(99, 21);
		((Control)labelX19).TabIndex = 110;
		((Control)labelX19).Text = "[شماره قرارداد] (G):";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)labelX18).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Location = new Point(740, 692);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(91, 14);
		((Control)labelX18).TabIndex = 109;
		((Control)labelX18).Text = "[جریمه تاخیر]:";
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)lblJarime).Anchor = (AnchorStyles)10;
		((BaseItemControl)lblJarime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblJarime).ForeColor = Color.DarkRed;
		((Control)lblJarime).Location = new Point(700, 692);
		((Control)lblJarime).Name = "lblJarime";
		((Control)lblJarime).Size = new Size(34, 14);
		((Control)lblJarime).TabIndex = 108;
		((Control)lblJarime).Text = "------";
		((Control)labelX16).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Location = new Point(589, 712);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(107, 14);
		((Control)labelX16).TabIndex = 105;
		((Control)labelX16).Text = "[تعطیل کاری (ساعت)]:";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)LblTatilValue).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblTatilValue).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTatilValue).ForeColor = Color.DarkRed;
		((Control)LblTatilValue).Location = new Point(549, 712);
		((Control)LblTatilValue).Name = "LblTatilValue";
		((Control)LblTatilValue).Size = new Size(34, 14);
		((Control)LblTatilValue).TabIndex = 104;
		((Control)LblTatilValue).Text = "------";
		((Control)btnBack).AccessibleRole = (AccessibleRole)43;
		((Control)btnBack).Anchor = (AnchorStyles)9;
		btnBack.ColorTable = (eButtonColor)6;
		((Control)btnBack).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnBack.Image = (Image)(object)Resources.next2;
		btnBack.ImagePosition = (eImagePosition)1;
		((Control)btnBack).Location = new Point(557, 3);
		((Control)btnBack).Margin = new Padding(2);
		((Control)btnBack).Name = "btnBack";
		((Control)btnBack).RightToLeft = (RightToLeft)1;
		((Control)btnBack).Size = new Size(104, 53);
		((PopupItemControl)btnBack).Style = (eDotNetBarStyle)9;
		((Control)btnBack).TabIndex = 88;
		((Control)btnBack).TabStop = false;
		((Control)btnBack).Text = "[ماه قبل] ( - )";
		((Control)btnBack).Click += btnBack_Click;
		((Control)btnNext).AccessibleRole = (AccessibleRole)43;
		((Control)btnNext).Anchor = (AnchorStyles)9;
		btnNext.ColorTable = (eButtonColor)6;
		((Control)btnNext).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnNext.Image = (Image)(object)Resources.back2;
		btnNext.ImagePosition = (eImagePosition)1;
		((Control)btnNext).Location = new Point(665, 3);
		((Control)btnNext).Margin = new Padding(2);
		((Control)btnNext).Name = "btnNext";
		((Control)btnNext).RightToLeft = (RightToLeft)1;
		((Control)btnNext).Size = new Size(105, 53);
		((PopupItemControl)btnNext).Style = (eDotNetBarStyle)9;
		((Control)btnNext).TabIndex = 87;
		((Control)btnNext).TabStop = false;
		((Control)btnNext).Text = "[ماه بعد] ( + )";
		((Control)btnNext).Click += btnNext_Click;
		((Control)LblName).Anchor = (AnchorStyles)9;
		((Control)LblName).ForeColor = Color.DarkRed;
		LblName.LinkBehavior = (LinkBehavior)3;
		LblName.LinkColor = Color.DarkRed;
		((Control)LblName).Location = new Point(942, 35);
		((Control)LblName).Name = "LblName";
		((Control)LblName).Size = new Size(132, 13);
		((Control)LblName).TabIndex = 86;
		LblName.TabStop = true;
		((Control)LblName).Text = "-------";
		((Label)LblName).TextAlign = (ContentAlignment)16;
		LblName.LinkClicked += new LinkLabelLinkClickedEventHandler(LblName_LinkClicked);
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(1081, 31);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(92, 21);
		((Control)labelX5).TabIndex = 85;
		((Control)labelX5).Text = "[نام شخص] (P):";
		((Control)LblMonth).Anchor = (AnchorStyles)9;
		((Control)LblMonth).ForeColor = Color.DarkRed;
		LblMonth.LinkBehavior = (LinkBehavior)3;
		LblMonth.LinkColor = Color.DarkRed;
		((Control)LblMonth).Location = new Point(942, 12);
		((Control)LblMonth).Name = "LblMonth";
		((Control)LblMonth).Size = new Size(79, 13);
		((Control)LblMonth).TabIndex = 84;
		LblMonth.TabStop = true;
		((Control)LblMonth).Text = "-------";
		((Label)LblMonth).TextAlign = (ContentAlignment)16;
		LblMonth.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)LblYear).Anchor = (AnchorStyles)9;
		((Control)LblYear).ForeColor = Color.DarkRed;
		LblYear.LinkBehavior = (LinkBehavior)3;
		LblYear.LinkColor = Color.DarkRed;
		((Control)LblYear).Location = new Point(1081, 12);
		((Control)LblYear).Name = "LblYear";
		((Control)LblYear).RightToLeft = (RightToLeft)1;
		((Control)LblYear).Size = new Size(43, 13);
		((Control)LblYear).TabIndex = 83;
		LblYear.TabStop = true;
		((Control)LblYear).Text = "-------";
		((Label)LblYear).TextAlign = (ContentAlignment)16;
		LblYear.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)LblCodeTafsil).Anchor = (AnchorStyles)9;
		((Control)LblCodeTafsil).ForeColor = Color.DarkRed;
		LblCodeTafsil.LinkBehavior = (LinkBehavior)3;
		LblCodeTafsil.LinkColor = Color.DarkRed;
		((Control)LblCodeTafsil).Location = new Point(775, 35);
		((Control)LblCodeTafsil).Name = "LblCodeTafsil";
		((Control)LblCodeTafsil).Size = new Size(47, 13);
		((Control)LblCodeTafsil).TabIndex = 82;
		LblCodeTafsil.TabStop = true;
		((Control)LblCodeTafsil).Text = "-------";
		((Label)LblCodeTafsil).TextAlign = (ContentAlignment)16;
		LblCodeTafsil.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)labelX7).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(740, 672);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(91, 14);
		((Control)labelX7).TabIndex = 81;
		((Control)labelX7).Text = "[غیبت(روز)]:";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)LblGheybat).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblGheybat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblGheybat).ForeColor = Color.DarkRed;
		((Control)LblGheybat).Location = new Point(700, 672);
		((Control)LblGheybat).Name = "LblGheybat";
		((Control)LblGheybat).Size = new Size(34, 14);
		((Control)LblGheybat).TabIndex = 80;
		((Control)LblGheybat).Text = "------";
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(1016, 712);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(86, 14);
		((Control)labelX1).TabIndex = 81;
		((Control)labelX1).Text = "[کسر کار (ساعت)]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)Lblkasr).Anchor = (AnchorStyles)10;
		((BaseItemControl)Lblkasr).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lblkasr).ForeColor = Color.DarkRed;
		((Control)Lblkasr).Location = new Point(976, 712);
		((Control)Lblkasr).Name = "Lblkasr";
		((Control)Lblkasr).Size = new Size(34, 14);
		((Control)Lblkasr).TabIndex = 80;
		((Control)Lblkasr).Text = "------";
		((Control)PicPerson).Anchor = (AnchorStyles)9;
		PicPerson.BorderStyle = (BorderStyle)1;
		PicPerson.Image = (Image)(object)Resources.Person;
		((Control)PicPerson).Location = new Point(1179, 3);
		((Control)PicPerson).Name = "PicPerson";
		((Control)PicPerson).Size = new Size(57, 53);
		PicPerson.SizeMode = (PictureBoxSizeMode)1;
		PicPerson.TabIndex = 76;
		PicPerson.TabStop = false;
		((Control)LblForCodeTafsil).Anchor = (AnchorStyles)9;
		((BaseItemControl)LblForCodeTafsil).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblForCodeTafsil).Location = new Point(837, 31);
		((Control)LblForCodeTafsil).Name = "LblForCodeTafsil";
		((Control)LblForCodeTafsil).RightToLeft = (RightToLeft)1;
		((Control)LblForCodeTafsil).Size = new Size(92, 21);
		((Control)LblForCodeTafsil).TabIndex = 75;
		((Control)LblForCodeTafsil).Text = "[کد تفصیل](T):";
		LblForCodeTafsil.TextAlignment = (StringAlignment)2;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(3, 3);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(119, 53);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 74;
		((Control)BtnPrint).Text = "[چاپ] (Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)Btnfish).AccessibleRole = (AccessibleRole)43;
		((Control)Btnfish).Anchor = (AnchorStyles)13;
		Btnfish.ColorTable = (eButtonColor)6;
		Btnfish.Image = (Image)componentResourceManager.GetObject("Btnfish.Image");
		Btnfish.ImagePosition = (eImagePosition)1;
		((Control)Btnfish).Location = new Point(126, 3);
		((Control)Btnfish).Name = "Btnfish";
		((Control)Btnfish).Size = new Size(427, 53);
		((PopupItemControl)Btnfish).Style = (eDotNetBarStyle)9;
		((Control)Btnfish).TabIndex = 73;
		((Control)Btnfish).Text = "[صدور فیش حقوقی] ( F )";
		((Control)Btnfish).Click += Btnfish_Click;
		((Control)LblShiftName1).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftName1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftName1).Location = new Point(450, 672);
		((Control)LblShiftName1).Name = "LblShiftName1";
		((Control)LblShiftName1).Size = new Size(95, 14);
		((Control)LblShiftName1).TabIndex = 48;
		((Control)LblShiftName1).Text = "[نوبت کاری صبح]:";
		LblShiftName1.TextAlignment = (StringAlignment)2;
		((Control)LblMissionName).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblMissionName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMissionName).Location = new Point(740, 712);
		((Control)LblMissionName).Name = "LblMissionName";
		((Control)LblMissionName).Size = new Size(91, 14);
		((Control)LblMissionName).TabIndex = 48;
		((Control)LblMissionName).Text = "[ماموریت (ساعت)]:";
		LblMissionName.TextAlignment = (StringAlignment)2;
		((Control)LblHolidayName).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblHolidayName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHolidayName).Location = new Point(589, 692);
		((Control)LblHolidayName).Name = "LblHolidayName";
		((Control)LblHolidayName).Size = new Size(107, 14);
		((Control)LblHolidayName).TabIndex = 48;
		((Control)LblHolidayName).Text = "[جمعه کاری (ساعت)]:";
		LblHolidayName.TextAlignment = (StringAlignment)2;
		((Control)LblNightWorkName).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblNightWorkName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNightWorkName).Location = new Point(589, 672);
		((Control)LblNightWorkName).Name = "LblNightWorkName";
		((Control)LblNightWorkName).Size = new Size(107, 14);
		((Control)LblNightWorkName).TabIndex = 48;
		((Control)LblNightWorkName).Text = "[شب کاری (ساعت)]:";
		LblNightWorkName.TextAlignment = (StringAlignment)2;
		((Control)labelX8).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(879, 692);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(91, 14);
		((Control)labelX8).TabIndex = 48;
		((Control)labelX8).Text = "[مرخصی ساعتی]:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)LblLeaveName).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblLeaveName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblLeaveName).Location = new Point(879, 712);
		((Control)LblLeaveName).Name = "LblLeaveName";
		((Control)LblLeaveName).Size = new Size(91, 14);
		((Control)LblLeaveName).TabIndex = 48;
		((Control)LblLeaveName).Text = "[مرخصی روزانه]:";
		LblLeaveName.TextAlignment = (StringAlignment)2;
		((Control)LblShiftValue1).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblShiftValue1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblShiftValue1).ForeColor = Color.DarkRed;
		((Control)LblShiftValue1).Location = new Point(411, 672);
		((Control)LblShiftValue1).Name = "LblShiftValue1";
		((Control)LblShiftValue1).Size = new Size(34, 14);
		((Control)LblShiftValue1).TabIndex = 45;
		((Control)LblShiftValue1).Text = "------";
		((Control)LblMissionValue).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblMissionValue).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMissionValue).ForeColor = Color.DarkRed;
		((Control)LblMissionValue).Location = new Point(700, 712);
		((Control)LblMissionValue).Name = "LblMissionValue";
		((Control)LblMissionValue).Size = new Size(34, 14);
		((Control)LblMissionValue).TabIndex = 45;
		((Control)LblMissionValue).Text = "------";
		((Control)LblKasrsum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblKasrsum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKasrsum).Location = new Point(1146, 712);
		((Control)LblKasrsum).Name = "LblKasrsum";
		((Control)LblKasrsum).Size = new Size(72, 14);
		((Control)LblKasrsum).TabIndex = 48;
		((Control)LblKasrsum).Text = "[اضافه کار]:";
		LblKasrsum.TextAlignment = (StringAlignment)2;
		((Control)LblHolidayValue).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblHolidayValue).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHolidayValue).ForeColor = Color.DarkRed;
		((Control)LblHolidayValue).Location = new Point(549, 692);
		((Control)LblHolidayValue).Name = "LblHolidayValue";
		((Control)LblHolidayValue).Size = new Size(34, 14);
		((Control)LblHolidayValue).TabIndex = 45;
		((Control)LblHolidayValue).Text = "------";
		((Control)LblhozorSum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblhozorSum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblhozorSum).Location = new Point(1146, 672);
		((Control)LblhozorSum).Name = "LblhozorSum";
		((Control)LblhozorSum).Size = new Size(86, 14);
		((Control)LblhozorSum).TabIndex = 47;
		((Control)LblhozorSum).Text = "کارکرد کل(0):";
		LblhozorSum.TextAlignment = (StringAlignment)2;
		((Control)LblNightWorkValue).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblNightWorkValue).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNightWorkValue).ForeColor = Color.DarkRed;
		((Control)LblNightWorkValue).Location = new Point(549, 672);
		((Control)LblNightWorkValue).Name = "LblNightWorkValue";
		((Control)LblNightWorkValue).Size = new Size(34, 14);
		((Control)LblNightWorkValue).TabIndex = 45;
		((Control)LblNightWorkValue).Text = "------";
		((Control)LblLeaveValueHour).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblLeaveValueHour).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblLeaveValueHour).ForeColor = Color.DarkRed;
		((Control)LblLeaveValueHour).Location = new Point(839, 692);
		((Control)LblLeaveValueHour).Name = "LblLeaveValueHour";
		((Control)LblLeaveValueHour).Size = new Size(34, 14);
		((Control)LblLeaveValueHour).TabIndex = 45;
		((Control)LblLeaveValueHour).Text = "------";
		((Control)LblRow).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRow).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRow).Location = new Point(879, 672);
		((Control)LblRow).Name = "LblRow";
		((Control)LblRow).Size = new Size(91, 14);
		((Control)LblRow).TabIndex = 46;
		((Control)LblRow).Text = "[تعداد تردد]:";
		LblRow.TextAlignment = (StringAlignment)2;
		((Control)LblRow).Visible = false;
		((Control)LblLeaveValueDay).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblLeaveValueDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblLeaveValueDay).ForeColor = Color.DarkRed;
		((Control)LblLeaveValueDay).Location = new Point(839, 712);
		((Control)LblLeaveValueDay).Name = "LblLeaveValueDay";
		((Control)LblLeaveValueDay).Size = new Size(34, 14);
		((Control)LblLeaveValueDay).TabIndex = 45;
		((Control)LblLeaveValueDay).Text = "------";
		((Control)LblEzafe).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblEzafe).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEzafe).ForeColor = Color.DarkRed;
		((Control)LblEzafe).Location = new Point(1107, 712);
		((Control)LblEzafe).Name = "LblEzafe";
		((Control)LblEzafe).Size = new Size(34, 14);
		((Control)LblEzafe).TabIndex = 45;
		((Control)LblEzafe).Text = "------";
		((Control)LblHozor).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblHozor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHozor).ForeColor = Color.DarkRed;
		((Control)LblHozor).Location = new Point(1107, 672);
		((Control)LblHozor).Name = "LblHozor";
		((Control)LblHozor).Size = new Size(34, 14);
		((Control)LblHozor).TabIndex = 44;
		((Control)LblHozor).Text = "------";
		((Control)LblRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRows).ForeColor = Color.DarkRed;
		((Control)LblRows).Location = new Point(839, 672);
		((Control)LblRows).Name = "LblRows";
		((Control)LblRows).Size = new Size(34, 14);
		((Control)LblRows).TabIndex = 43;
		((Control)LblRows).Text = "------";
		((Control)LblRows).Visible = false;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(1027, 8);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(47, 21);
		((Control)labelX3).TabIndex = 25;
		((Control)labelX3).Text = "[ماه] (Y):";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(1120, 8);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(53, 21);
		((Control)labelX2).TabIndex = 23;
		((Control)labelX2).Text = "[سال] (Y):";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1238, 730);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmRptTaradodFull";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[گزارش کامل تردد ]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosing += new FormClosingEventHandler(FrmRptTaradodFull_FormClosing);
		((Form)this).Load += FrmRptTaradodFull_Load;
		((Control)CMS).ResumeLayout(false);
		((ISupportInitialize)TaradodFullDetViewbindingSource).EndInit();
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)PicPerson).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
