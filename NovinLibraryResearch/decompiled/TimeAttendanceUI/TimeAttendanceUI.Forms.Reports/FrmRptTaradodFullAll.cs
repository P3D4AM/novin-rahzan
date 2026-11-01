using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
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
using PacketParser.EventArgs;
using PacketParser.EventArgs.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
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
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmRptTaradodFullAll : BaseForm
{
	private bool _enableEvents;

	private bool _isDisposed;

	private long _counter1;

	private long _counter2;

	private ColorDet _clsColor;

	private DateTime _date;

	private Font _font;

	private List<int> _personIDs;

	private List<StatuseTaradodEnumType> _statuseTaradod = new List<StatuseTaradodEnumType>();

	private List<Color> _dGridBackColors = new List<Color>();

	private IFormatProvider FormatTimespan = (IFormatProvider)new TimeSpanFormatter();

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _loadGridToken;

	private List<TaradodFullDetView> _listData;

	private List<TaradodFullDetView> _filteredItems;

	private SortableBindingList<TaradodFullDetView> _bindedData;

	private TaradodFull _clsTaradodFull;

	private TaradodFull _clsTaradodFullForDisplay;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX LblEzafe;

	private LabelX LblHozor;

	private LabelX LblRows;

	private LabelX LblRow;

	private LabelX LblKasrsum;

	private LabelX LblhozorSum;

	private ButtonX BtnPrint;

	private PictureBox PicPerson;

	private LabelX labelX1;

	private LabelX Lblkasr;

	private LinkLabel Lbldate;

	private LabelX labelX2;

	private ContextMenuStrip CMS;

	private ToolStripMenuItem CMS_DeleteAttendance;

	private ToolStripMenuItem CMS_ChangeTypeTaradod;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem CMS_InsertMorakhasi;

	private ToolStripMenuItem CMS_Morakhasi_Daily;

	private ToolStripMenuItem CMS_GharadadList;

	private ToolStripMenuItem CMS_ShiftList;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem CMS_PersonInfo;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem EditManagementToolStripMenuItem;

	private ToolStripMenuItem CMEK;

	private ToolStripMenuItem CMKK;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem CMMission;

	private ToolStripMenuItem CMVacation;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripMenuItem CMFriday;

	private ToolStripMenuItem CMSNobatkari1;

	private ToolStripMenuItem CMSNobatkari2;

	private ToolStripMenuItem CMSNobatkari3;

	private ToolStripMenuItem CMAbsent;

	private ToolStripMenuItem CMNight;

	private LabelX labelX6;

	private ButtonX btnNext;

	private ButtonX btnBack;

	private ButtonX btnFilter;

	private ToolStripMenuItem انتخابToolStripMenuItem;

	private ToolStripMenuItem CmsSelectAll;

	private ToolStripMenuItem CmsUnSelectAll;

	private ToolStripMenuItem CmsReverseSelected;

	private ToolStripMenuItem CmsGroupEdit;

	private LabelX LblCountStatus;

	private ToolStripMenuItem CmsRequestedVacation;

	private BindingSource taradodFullDetViewBindingSource;

	private ProgressBar progressBar1;

	internal LabelX lblSearch;

	internal TextBoxX TxtSearch;

	private LabelX labelX8;

	private LabelX LblNoContract;

	private LabelX labelX10;

	private LabelX LblTaradodNaghes;

	private LabelX labelX3;

	private LabelX LblAbsent;

	private LabelX labelX5;

	private LabelX LblTaradodKamel;

	private DataGridViewCheckBoxColumn DG_Check;

	private DataGridViewTextBoxColumn DG_Index;

	private DataGridViewTextBoxColumn DG_CodeTafsil;

	private DataGridViewTextBoxColumn DG_FullName;

	private DataGridViewTextBoxColumn DG_TaradodStatusName;

	private DataGridViewTextBoxColumn DG_ShiftTime;

	private DataGridViewTextBoxColumn DG_Hozor;

	private DataGridViewTextBoxColumn DG_EzafeKar;

	private DataGridViewTextBoxColumn DG_ManulaEzafeKarTime;

	private DataGridViewTextBoxColumn DG_KasreKar;

	private DataGridViewTextBoxColumn DG_ManulaKasreKarTime;

	private DataGridViewTextBoxColumn DG_ManulaMissionTime;

	private DataGridViewTextBoxColumn DG_ManulaVacationTime;

	private DataGridViewTextBoxColumn DG_ManulaFridayTime;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime1;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime2;

	private DataGridViewTextBoxColumn DG_ManulaShiftTime3;

	private DataGridViewTextBoxColumn DG_ManulaAbsentTime;

	private DataGridViewTextBoxColumn DG_ManulaNightTime;

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

	private LinkLabel LblDateM;

	private LabelX labelX4;

	private DataGridView DGrid;

	private bool IsDisposed
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

	public DateTime DateM1
	{
		get
		{
			try
			{
				return Utilities.CalendarToMiladi(Calendar.MiladiToCalture(_date).Substring(0, 8) + "01");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return DateTime.Now;
		}
	}

	private DateTime DateM2
	{
		get
		{
			try
			{
				return Utilities.EndDayOfMonthtCalendar(Calendar.MiladiToCalture(_date), "23:59:59");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return DateTime.Now;
		}
	}

	public TaradodFullDetView SelectedRow { get; private set; }

	public string SearchString { get; private set; }

	public LogOnPersonel SelectedTaradod
	{
		get
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (SelectedRow == null)
				{
					return null;
				}
				if (DGrid.CurrentCell.ColumnIndex < ((DataGridViewBand)DG_V1Time).Index || DGrid.CurrentCell.ColumnIndex > ((DataGridViewBand)DG_K8Time).Index)
				{
					FMessageBox.Show("تغییر نوع تردد فقط روی ستونهای ورود یا خروج امکان پذیر است");
					return null;
				}
				_ = (TimeSpan)DGrid.CurrentCell.Value;
				return SelectedRow?.TaradodsCurrentDay[DGrid.CurrentCell.ColumnIndex - ((DataGridViewBand)DG_V1Time).Index];
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
			return null;
		}
	}

	private int PersonId
	{
		get
		{
			TaradodFullDetView selectedRow = SelectedRow;
			int? obj;
			if (selectedRow == null)
			{
				obj = null;
			}
			else
			{
				TimeAttendanceDAL.Dal.FLRPerson.Person personInfo = selectedRow.PersonInfo;
				obj = ((personInfo != null) ? new int?(((PersonBase)personInfo).Id) : null);
			}
			int? num = obj;
			return num.GetValueOrDefault(-1);
		}
	}

	private string PicPathSelected
	{
		get
		{
			TaradodFullDetView selectedRow = SelectedRow;
			object obj;
			if (selectedRow == null)
			{
				obj = null;
			}
			else
			{
				TimeAttendanceDAL.Dal.FLRPerson.Person personInfo = selectedRow.PersonInfo;
				obj = ((personInfo != null) ? ((PersonBase)personInfo).PicPath : null);
			}
			if (obj == null)
			{
				obj = "";
			}
			return (string)obj;
		}
	}

	private DateTime CurrentRowDate => SelectedRow?.DateM ?? DateTime.Now;

	public FrmRptTaradodFullAll(string contextGuid, Font font, List<int> piDs, DateTime pDate1)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			_font = font;
			_personIDs = piDs;
			_date = pDate1;
			InitializeComponent();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FrmRptTaradodFullAll_Load(object sender, EventArgs e)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportTaradodPersonnelOnADay)
			{
				((Form)this).Close();
				return;
			}
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
			((ToolStripItem)EditManagementToolStripMenuItem).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodEditManagement;
			DataGridViewCellStyle val = new DataGridViewCellStyle();
			TimeSpanFormatter formatProvider = new TimeSpanFormatter();
			val.FormatProvider = (IFormatProvider)formatProvider;
			val.Format = "hhmm";
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
			_clsColor = new ColorDet();
			LoadData();
			InitCmsItems();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task _cls_OnSummeryUpdate(object sender, AttendanceReportSummeryEventArgs e)
	{
		try
		{
			CancellationToken token = e.Token;
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
						string text = "کارکرد(" + _clsTaradodFullForDisplay.CountKarkerd + "):";
						((Control)Lbldate).Text = Calendar.MiladiToCalture(_date) + " " + LanguageAndCalendar.Instance.CaltureCalender.GetDayNameOfWeek(_date);
						((Control)LblDateM).Text = _date.Date.ToShortDateString() ?? "";
						((Control)LblEzafe).Text = _clsTaradodFullForDisplay.SumEzafekarTime;
						((Control)Lblkasr).Text = _clsTaradodFullForDisplay.SumKasrkarTime;
						((Control)LblRows).Text = _clsTaradodFullForDisplay.ListTaradodDets.Count.ToString();
						((Control)LblHozor).Text = _clsTaradodFullForDisplay.SumKarkerdTime;
						((Control)LblTaradodKamel).Text = _clsTaradodFullForDisplay.ListTaradodDets.Count((TaradodFullDetView p) => p.TaradodStatus == (StatuseTaradodEnumType?)1).ToString();
						((Control)LblTaradodNaghes).Text = _clsTaradodFullForDisplay.ListTaradodDets.Count((TaradodFullDetView p) => p.TaradodStatus == (StatuseTaradodEnumType?)2).ToString();
						((Control)LblAbsent).Text = _clsTaradodFullForDisplay.ListTaradodDets.Count((TaradodFullDetView p) => p.TaradodStatus == (StatuseTaradodEnumType?)3).ToString();
						((Control)LblNoContract).Text = _clsTaradodFullForDisplay.ListTaradodDets.Count((TaradodFullDetView p) => p.TaradodStatus == (StatuseTaradodEnumType?)4).ToString();
						((Control)LblhozorSum).Text = text;
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

	private async Task _cls_OnProcced(object sender, PercenteCompletedEventArgs e)
	{
		try
		{
			CancellationToken token = e.Token;
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
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)progressBar1).Visible = e.TotalItems > e.CurrentItem;
						progressBar1.Maximum = e.TotalItems;
						progressBar1.Value = e.CurrentItem;
						((Control)this).Refresh();
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
						TaradodFullDetView taradodFullDetView = (TaradodFullDetView)taradodFullDetViewBindingSource.List[num];
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
						DataGridViewCell obj4 = DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num];
						string toolTipText = obj4.ToolTipText;
						IEnumerable<AttPersonLeaveBusiness> personLeaves = taradodFullDetView.PersonLeaves;
						obj4.ToolTipText = toolTipText + "\r\n" + ((personLeaves != null && personLeaves.Any()) ? $"درخواست {taradodFullDetView.PersonLeaves.Last().QuiddityForDisplay} از {taradodFullDetView.PersonLeaves.Last().DateFrom:HH:mm} تا {taradodFullDetView.PersonLeaves.Last().DateTo:HH:mm}" : taradodFullDetView.GetHozorToolTip());
						if (((DataGridViewBand)DG_V1Time).Visible)
						{
							int lastTardodCellIndex = GetLastTardodCellIndex();
							int num2 = ((DataGridViewBand)DG_V1Time).Index;
							int num3 = 0;
							int num4 = 0;
							while (num2 <= lastTardodCellIndex)
							{
								DataGridViewCell obj5 = DGrid[num2, num];
								obj5.ToolTipText = obj5.ToolTipText + "\r\n" + taradodFullDetView.CoupleTaradodCurrentDay[num3].GetToolTip(token);
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
			if (token.IsCancellationRequested || IsDisposed || token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			int sumshiftMinutes = _listData.Sum((TaradodFullDetView p) => p.SumShiftMinuts);
			double sumEzafeKar = _listData.Sum((TaradodFullDetView p) => p.EzafeKar.Value.TotalMinutes);
			double sumKasreKar = _listData.Sum((TaradodFullDetView p) => p.KasreKar.Value.TotalMinutes);
			bool hasManualEzafeKar = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaEzafeKarTime));
			bool hasManualKasreKar = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaKasreKarTime));
			bool hasManualVacation = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaVacationTime));
			bool hasManualMission = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaMissionTime));
			bool hasManualAbsent = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaAbsentTime));
			bool hasManualFriday = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaFridayTime));
			bool hasManualNight = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaNightTime));
			bool hasManualShift1 = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime1));
			bool hasManualShift2 = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime2));
			bool hasManualShift3 = _listData.Any((TaradodFullDetView p) => !string.IsNullOrEmpty(p.ManulaShiftTime3));
			bool haskhoroj1 = false;
			bool haskhoroj2 = false;
			bool haskhoroj3 = false;
			bool haskhoroj4 = false;
			bool haskhoroj5 = false;
			bool haskhoroj6 = false;
			bool haskhoroj7 = false;
			haskhoroj1 = _listData.Sum((TaradodFullDetView p) => p.K1Time.TotalMinutes) > 0.0;
			if (haskhoroj1)
			{
				haskhoroj2 = _listData.Sum((TaradodFullDetView p) => p.K2Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj2)
			{
				haskhoroj3 = _listData.Sum((TaradodFullDetView p) => p.K3Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj3)
			{
				haskhoroj4 = _listData.Sum((TaradodFullDetView p) => p.K4Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj4)
			{
				haskhoroj5 = _listData.Sum((TaradodFullDetView p) => p.K5Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj5)
			{
				haskhoroj6 = _listData.Sum((TaradodFullDetView p) => p.K6Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj6)
			{
				haskhoroj7 = _listData.Sum((TaradodFullDetView p) => p.K7Time.TotalMinutes) > 0.0;
			}
			if (haskhoroj7)
			{
				_listData.Sum((TaradodFullDetView p) => p.K8Time.TotalMinutes);
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
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Invalid comparison between Unknown and I4
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0140: Unknown result type (might be due to invalid IL or missing references)
				//IL_0142: Unknown result type (might be due to invalid IL or missing references)
				//IL_0145: Invalid comparison between Unknown and I4
				//IL_0107: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Unknown result type (might be due to invalid IL or missing references)
				//IL_014b: Invalid comparison between Unknown and I4
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Invalid comparison between Unknown and I4
				//IL_01de: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e4: Invalid comparison between Unknown and I4
				//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_050e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0318: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					for (int num = 0; num < DGrid.Rows.Count; num++)
					{
						if (token.IsCancellationRequested || IsDisposed)
						{
							break;
						}
						TaradodFullDetView taradod = (TaradodFullDetView)taradodFullDetViewBindingSource.List[num];
						DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.ForeColor = StringExtensions.ToColor(_clsColor.GetColor(taradod.TaradodStatus.GetValueOrDefault()), default(Color));
						StatuseTaradodEnumType? taradodStatus = taradod.TaradodStatus;
						if (taradodStatus.HasValue && (int)taradodStatus.GetValueOrDefault() == 6)
						{
							DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.ForeColor = StringExtensions.ToColor(_clsColor.GetColor(taradod.TaradodStatus.GetValueOrDefault()), default(Color));
						}
						taradodStatus = taradod.TaradodStatus;
						bool flag;
						if (taradodStatus.HasValue)
						{
							StatuseTaradodEnumType valueOrDefault = taradodStatus.GetValueOrDefault();
							if ((int)valueOrDefault == 5 || (int)valueOrDefault == 9 || (int)valueOrDefault == 14)
							{
								flag = true;
								goto IL_015b;
							}
						}
						flag = false;
						goto IL_015b;
						IL_015b:
						if (flag)
						{
							DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.BackColor = Color.LightGreen;
						}
						IEnumerable<AttPersonLeaveBusiness> personLeaves = taradod.PersonLeaves;
						if (personLeaves != null && personLeaves.Any())
						{
							DGrid[((DataGridViewBand)DG_TaradodStatusName).Index, num].Style.BackColor = (((int)taradod.PersonLeaves.Last().Quiddity == 1) ? Color.FromArgb(255, 223, 255, 236) : Color.LightGreen);
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

	private async void RPTTaradod_Full_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 107)
			{
				if ((int)keyCode <= 32)
				{
					if ((int)keyCode != 27)
					{
						if ((int)keyCode == 32)
						{
							e.Handled = true;
							if (SelectedRow != null)
							{
								SelectedRow.IsChecked = !SelectedRow.IsChecked;
							}
						}
					}
					else
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
					return;
				}
				if ((int)keyCode != 70)
				{
					if ((int)keyCode != 107)
					{
						return;
					}
					goto IL_017b;
				}
				if (e.Control)
				{
					e.Handled = true;
					((Control)TxtSearch).Focus();
				}
				return;
			}
			if ((int)keyCode <= 123)
			{
				if ((int)keyCode != 109)
				{
					if ((int)keyCode == 123)
					{
						e.Handled = true;
						FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
						await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
						((Component)(object)frm).Dispose();
					}
					return;
				}
			}
			else
			{
				if ((int)keyCode == 187)
				{
					goto IL_017b;
				}
				if ((int)keyCode != 189)
				{
					return;
				}
			}
			e.Handled = true;
			((PopupItemControl)btnBack).PerformClick();
			return;
			IL_017b:
			e.Handled = true;
			((PopupItemControl)btnNext).PerformClick();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void LblYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, 0, _date, bShowForm: false, bUcDate: false, -2, bUcPerson: false, -1, bUcPersonTak: false, -3, bUcDateTak: false, -4, bUcTakDay: true, 0, bUcDesc: false, -5, (ReportFilterEnumType)11);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_date = frm.DateM1;
				LoadData();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BtnPrint).Enabled && ((Control)BtnPrint).Visible)
			{
				((Control)DGrid).Focus();
				List<object> lst = (await ForReportTaradodFull.FillListAsync(((CacheBaseForm)this).ContextGuid, _clsTaradodFullForDisplay)).Cast<object>().ToList();
				FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)11, lst, null, 0, "");
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		try
		{
			_date = _date.AddDays(1.0);
			LoadData();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		try
		{
			_date = _date.AddDays(-1.0);
			LoadData();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnFilter_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmFilterTaradod frm = new FrmFilterTaradod(((CacheBaseForm)this).ContextGuid, _statuseTaradod);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_statuseTaradod = frm.GetStatuseTaradodSelected();
				await FillDataAsync();
			}
			((Component)(object)frm).Dispose();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void DGrid2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
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

	private void DGrid_Scroll(object sender, ScrollEventArgs e)
	{
		try
		{
			if (e.NewValue <= ((Collection<TaradodFullDetView>)(object)_bindedData).Count - 70 || ((Collection<TaradodFullDetView>)(object)_bindedData).Count >= _filteredItems.Count)
			{
				return;
			}
			List<TaradodFullDetView> list = _filteredItems.Except((IEnumerable<TaradodFullDetView>)_bindedData).Take(50).ToList();
			int newValue = e.NewValue;
			_bindedData.AddRange((IEnumerable<TaradodFullDetView>)list);
			DGrid.FirstDisplayedScrollingRowIndex = newValue;
			_loadGridToken?.Cancel();
			_loadGridToken = new CancellationTokenSource();
			CancellationToken token = _loadGridToken.Token;
			Task.Run(async delegate
			{
				await Task.Delay(1000);
				if (!token.IsCancellationRequested)
				{
					await SetDgridCellColorsAsync(token);
					await _clsTaradodFull.CalcSummeryTaradodFullDataAsync(token);
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		try
		{
			if (_enableEvents && (int)((MouseEventArgs)e).Button == 2097152 && e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				DGrid.CurrentCell = DGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
				SelectedRow.IsChecked = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if ((int)e.KeyCode == 13)
			{
				e.Handled = true;
				if (_enableEvents && SelectedRow != null)
				{
					await ((CacheBaseForm)new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, DateM1, DateM2, PersonId, _date)).ShowAsync((IWin32Window)(object)this);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void DGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (_enableEvents && SelectedRow != null)
			{
				await ((CacheBaseForm)new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, DateM1, DateM2, PersonId, _date)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
	{
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (_enableEvents && e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DG_Check).Index)
			{
				DataGridViewCell currentCell = DGrid.CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void taradodFullDetViewBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedRow = (TaradodFullDetView)taradodFullDetViewBindingSource.Current;
	}

	private void CmsSelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < DGrid.RowCount; i++)
			{
				DataGridViewCell obj = DGrid.Rows[i].Cells[((DataGridViewBand)DG_Check).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = true;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void CmsReverseSelected_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < DGrid.RowCount; i++)
			{
				DataGridViewCell obj = DGrid.Rows[i].Cells[((DataGridViewBand)DG_Check).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null && (((DataGridViewCell)val).Value == null || !(bool)((DataGridViewCell)val).Value))
				{
					((DataGridViewCell)val).Value = true;
				}
				else if (val != null)
				{
					((DataGridViewCell)val).Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmsReverseSelected_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFullAll.cs", 956);
		}
	}

	private void CmsUnSelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < DGrid.RowCount; i++)
			{
				DataGridViewCell obj = DGrid.Rows[i].Cells[((DataGridViewBand)DG_Check).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void CmsGroupEdit_Click(object sender, EventArgs e)
	{
		try
		{
			GroupEdit(DGrid.CurrentCell.ColumnIndex);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CmsRequestedVacation_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedRow == null)
			{
				return;
			}
			if (SelectedRow.PersonLeaves?.Any() ?? false)
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

	private async void CMS_Opened(object sender, EventArgs e)
	{
		try
		{
			if (SelectedRow != null)
			{
				ToolStripMenuItem cMS_InsertMorakhasi = CMS_InsertMorakhasi;
				((ToolStripItem)cMS_InsertMorakhasi).Visible = (int)(await SelectedRow.GetStatuseTaradodAsync()) == 3;
				((ToolStripItem)CmsGroupEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.GroupTaradodEdit;
				((ToolStripItem)CmsGroupEdit).Text = "افزودن تردد گروهی";
				((ToolStripItem)CmsGroupEdit).Visible = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_DeleteAttendance_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodDelete)
			{
				FMessageBox.Show("شما مجوز این عملیات را ندارید");
			}
			else if (SelectedTaradod != null)
			{
				if ((int)SelectedTaradod.IsVirtual == 4)
				{
					FMessageBox.Show(string.Concat("[تردد مورد نظر از نوع]" + Utilities.GetDisplay<TypeTaradodEnumType>(SelectedTaradod.TypeTaradod) + "[ می باشد و نمی توانید آنرا تغییر دهید]", "\n [تردد قبلی یا بعدی را بررسی نمائید]"));
					return;
				}
				await (SelectedTaradod?.DeleteAsync());
				LoadData();
				string pDesc = "[حذف یک تردد] : " + SelectedRow.FullName + " [تاریخ] : " + Calendar.MiladiToCalture(SelectedTaradod.DateTimeLogM) + " [ساعت] : " + SelectedTaradod.DateTimeLogM.ToString("HH:mm") + " [نوع تردد] : " + Utilities.GetDisplay<TypeTaradodEnumType>(SelectedTaradod.TypeTaradod) + " [شناسه دستگاه] : " + SelectedTaradod.DeviceId_ + " [کد سخت افزاری شخص] : " + SelectedTaradod.PersonNo;
				await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, pDesc, "Taradod.TaradodReportTaradodPersonnelOnADay", SelectedRow, "");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_ChangeTypeTaradod_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodDelete)
			{
				FMessageBox.Show("شما مجوز این عملیات را ندارید");
			}
			else
			{
				if (SelectedTaradod == null)
				{
					return;
				}
				if ((int)SelectedTaradod.IsVirtual == 4)
				{
					Utility.MessageBoxShowError(string.Concat("[تردد مورد نظر از نوع] " + Utilities.GetDisplay<TypeTaradodEnumType>(SelectedTaradod.TypeTaradod) + " [ می باشد و نمی توانید آنرا تغییر دهید]", "\n [تردد قبلی یا بعدی را بررسی نمائید]"), ((CacheBaseForm)this).ContextGuid);
					return;
				}
				ShiftDetView shiftDetView = await TaradodFullDet.GetShiftAsync(((CacheBaseForm)this).ContextGuid, SelectedTaradod.DateTimeLogM, SelectedRow.Contract.ID, ((PersonBase)SelectedRow.PersonInfo).Id, calcEkWhenHasVacationOrMission: true, 1);
				if (shiftDetView == null)
				{
					FMessageBox.Show("با توجه به اینکه شیفت ندارد تغییر نوع تردد بی مفهوم است");
					return;
				}
				if (SelectedTaradod.DateTimeLogM > shiftDetView.ClsShiftCoding.KhorojEnd)
				{
					FMessageBox.Show("این تردد خارج از بازه شیفت است و نمی توانید نوع آنرا تغییر دهید");
					return;
				}
				string text = SelectedTaradod.DateTimeLogM.ToString("HH:mm");
				string time = DGrid[DGrid.CurrentCell.ColumnIndex + 1, ((DataGridViewBand)DGrid.CurrentRow).Index].Value.ToString().Substring(0, 5);
				if (SelectedTaradod == SelectedRow.CoupleTaradodCurrentDay[0].Vtaradod && SelectedTaradod.DateTimeLogM > shiftDetView.ClsShiftCoding.VorodEnd)
				{
					time = text;
					text = shiftDetView.ClsShiftCoding.Vorod.ToString("HH:mm");
				}
				bool showVacation = SelectedTaradod.DateTimeLogM > shiftDetView.ClsShiftCoding.VorodEnd;
				FrmGetDecision frm = new FrmGetDecision(((CacheBaseForm)this).ContextGuid, SelectedTaradod.TypeTaradod, SelectedTaradod.TypeTaradodVacationOrMission, text, time, shiftDetView)
				{
					ShowVacation = showVacation
				};
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				if ((int)((Form)frm).DialogResult == 1)
				{
					string xdesc = "[ویرایش نوع تردد : جناب ]" + SelectedRow.CodeTafsil + " : " + SelectedRow.FullName + " [InDate] : " + Calendar.MiladiToCalture(SelectedTaradod.DateTimeLogM) + "  [و در ساعت] : " + SelectedTaradod.DateTimeLogM.ToString("HH:mm");
					TypeTaradodEnumType val;
					if (SelectedTaradod.TypeTaradod != frm.TypeTaradodCalculateSelected)
					{
						string[] obj = new string[6] { xdesc, " از ", null, null, null, null };
						val = SelectedTaradod.TypeTaradod;
						obj[2] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
						obj[3] = " به ";
						val = frm.TypeTaradodCalculateSelected;
						obj[4] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
						obj[5] = " تغییر کرد";
						xdesc = string.Concat(obj);
					}
					if (SelectedTaradod.TypeTaradodVacationOrMission != frm.TypeTaradodVacationOrMissionSelected)
					{
						string[] obj2 = new string[6] { xdesc, " از ", null, null, null, null };
						val = SelectedTaradod.TypeTaradodVacationOrMission;
						obj2[2] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
						obj2[3] = " به ";
						val = frm.TypeTaradodVacationOrMissionSelected;
						obj2[4] = ((object)(TypeTaradodEnumType)(ref val)).ToString();
						obj2[5] = " تغییر کرد";
						xdesc = string.Concat(obj2);
					}
					SelectedTaradod.TypeTaradodVacationOrMission = frm.TypeTaradodVacationOrMissionSelected;
					SelectedTaradod.TypeTaradod = frm.TypeTaradodCalculateSelected;
					await SelectedTaradod.SaveAsync("rptTaradodFullDet2", default(CancellationToken), "CMS_ChangeTypeTaradod_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFullAll.cs");
					LoadData();
					await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "تغییر نوع تردد در فرم گزارش تردد یک روز خاص", SelectedRow, "");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_InsertMorakhasi_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (SelectedRow == null)
			{
				return;
			}
			if ((int)(await SelectedRow.GetStatuseTaradodAsync()) != 3)
			{
				FMessageBox.Show("درج مرخصی روزانه صرفا روی روزهایی که وضعیت تردد آنها، غیبت می باشد امکان پذیر است");
				return;
			}
			FrmMorakhasiDaily frm = new FrmMorakhasiDaily(((CacheBaseForm)this).ContextGuid, SelectedRow.DateM, PersonId);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_Morakhasi_Daily_Click(object sender, EventArgs e)
	{
		try
		{
			int num = int.Parse(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 4));
			DateTime date = Utilities.CalendarToMiladi($"{num}/01/01");
			DateTime date2 = Utilities.CalendarToMiladi($"{num}/12/{Calendar.EndDayOfMonth((short)12, num)}");
			FrmMorakhasiDailyMain frmMor = new FrmMorakhasiDailyMain(((CacheBaseForm)this).ContextGuid, _font, new List<int>(1) { PersonId }, date, date2, "", isCalcSaati: false);
			await ((CacheBaseForm)frmMor).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frmMor).Dispose();
			LoadData();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_GharadadList_Click(object sender, EventArgs e)
	{
		try
		{
			FrmGharardadList frm = new FrmGharardadList(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			LoadData();
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
			FrmShiftMain frm = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			LoadData();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void CMS_PersonInfo_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceDAL.Dal.FLRPerson.Person person = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId);
			FrmPersonel frm = new FrmPersonel(((CacheBaseForm)this).ContextGuid, person, _font, isReadOnly: false);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.EzaferKar, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.KasrKar, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.Mission, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.Vacation, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.Friday, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.NobatKari1, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.NobatKari2, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.NobatKari3, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.Absent, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
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
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			if ((int)(await ((CacheBaseForm)new FrmATT_TaradodDet(contextGuid, EN_TypeAtt.Night, await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, PersonId), CurrentRowDate)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				LoadData();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void LoadData()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده...", (Form)(object)this, Task.Run(() => LoadDataAsync_(token)), false, (EnWaiterType)0, (Control)null, "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFullAll.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync_(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			List<TimeAttendanceDAL.Dal.FLRPerson.Person> list = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetAllAsync(((CacheBaseForm)this).ContextGuid, _personIDs, personStatus: true, token);
			if (list == null || IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_clsTaradodFull = await TaradodFull.CreateAsync(((CacheBaseForm)this).ContextGuid, calcAwaitableProperties: false, list, _date, checkValidationVorodAndKHoroj: true, token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_clsTaradodFull.OnProcced += _cls_OnProcced;
			_clsTaradodFull.OnSummeryUpdate += _cls_OnSummeryUpdate;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			await FillDataAsync(token);
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				await _clsTaradodFull.CalcSummeryTaradodFullDataAsync(token);
				if (!IsDisposed && !token.IsCancellationRequested)
				{
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task FillDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_enableEvents = false;
			_clsTaradodFullForDisplay = await _clsTaradodFull.SetConditionAsync(_statuseTaradod, token);
			_listData = _clsTaradodFullForDisplay.ListTaradodDets;
			_filteredItems = _listData.ToList();
			if (!string.IsNullOrEmpty(((Control)TxtSearch).Text.Trim()))
			{
				string text = Text.FixString(((Control)TxtSearch).Text).Trim();
				if (text != "")
				{
					string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					foreach (string item in array)
					{
						_filteredItems = _filteredItems.Where((TaradodFullDetView p) => p.FullName.Contains(item) || p.CodeTafsil.ToString().Contains(item)).ToList();
					}
				}
			}
			_bindedData = Utilities.ToSortableBindingList<TaradodFullDetView>(_filteredItems.Take(1000), default(CancellationToken));
			int num = 1;
			foreach (TaradodFullDetView filteredItem in _filteredItems)
			{
				filteredItem.Index = num++;
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
						taradodFullDetViewBindingSource.DataSource = _bindedData;
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
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				await InitGridAsync();
				_cls_OnSummeryUpdate(this, new AttendanceReportSummeryEventArgs(token));
				_enableEvents = true;
			}
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

	private async Task InitGridAsync()
	{
		_ = 2;
		try
		{
			_loadGridToken?.Cancel();
			_loadGridToken = new CancellationTokenSource();
			CancellationToken token = _loadGridToken.Token;
			await SetDgridCollumnsAsync(token);
			await SetDgridCellColorsAsync(token);
			await SetDgridToolTipsAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFullAll.cs", 1427);
		}
	}

	private void LoadPicture(string fileName)
	{
		try
		{
			PicPerson.Image = (Image)(object)Resources.Person;
			if (File.Exists(fileName))
			{
				PicPerson.SizeMode = (PictureBoxSizeMode)1;
				PicPerson.Image = Image.FromFile(fileName);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void GroupEdit(int colIndex, CancellationToken token = default(CancellationToken))
	{
		List<EditGrouply> list = new List<EditGrouply>();
		try
		{
			foreach (TaradodFullDetView item in _listData.Where((TaradodFullDetView p) => p.IsChecked))
			{
				list.Add(new EditGrouply
				{
					CodeTafsil = item.CodeTafsil,
					FullName = item.FullName,
					DateSh = item.DateSh
				});
			}
			if (list.Count <= 0)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[اشخاص مورد نظر را انتخاب نمائید]", "", 3);
				return;
			}
			FRMEditTaradodGrouply frmEditTaradodGrouply = new FRMEditTaradodGrouply(((CacheBaseForm)this).ContextGuid, list, ((Control)Lbldate).Text);
			await ((CacheBaseForm)frmEditTaradodGrouply).ShowAsync((IWin32Window)(object)this);
			if ((int)((Form)frmEditTaradodGrouply).DialogResult == 1 && frmEditTaradodGrouply.SelectedCodeTafsil.Count > 0)
			{
				foreach (long codeTafsil in frmEditTaradodGrouply.SelectedCodeTafsil)
				{
					TaradodFullDetView taradodFullDetView = _listData.FirstOrDefault((TaradodFullDetView p) => p.CodeTafsil == codeTafsil);
					if (taradodFullDetView != null)
					{
						LogOnPersonel obj = new LogOnPersonel(((CacheBaseForm)this).ContextGuid)
						{
							DateTimeLogM = _date.Date.Add(TimeSpan.Parse(frmEditTaradodGrouply.NewTime)),
							DeviceId_ = taradodFullDetView.CoupleTaradodCurrentDay[0].Vtaradod.DeviceId_
						};
						TimeAttendanceDAL.Dal.FLRPerson.Person personInfo = taradodFullDetView.PersonInfo;
						obj.PersonId = ((personInfo != null) ? ((PersonBase)personInfo).Id : 0);
						obj.IsVirtual = (SavedTaradodEnumType)5;
						obj.TypeTaradodVacationOrMission = (TypeTaradodEnumType)3;
						obj.TypeTaradod = (TypeTaradodEnumType)3;
						obj.DeviceSerial_ = taradodFullDetView.CoupleTaradodCurrentDay[0].Vtaradod.DeviceSerial_;
						obj.DeviceNo = taradodFullDetView.CoupleTaradodCurrentDay[0].Vtaradod.DeviceNo;
						obj.TrafficId = taradodFullDetView.CoupleTaradodCurrentDay[0].Vtaradod.TrafficId;
						obj.PersonNo = taradodFullDetView.CoupleTaradodCurrentDay[0].Vtaradod.PersonNo;
						await obj.SaveAsync(((CacheBaseForm)this).ContextGuid, token, "GroupEdit", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptTaradodFullAll.cs");
					}
				}
				LoadData();
			}
			((Form)frmEditTaradodGrouply).Close();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task SetStatusCountAsync(CancellationToken token)
	{
		try
		{
			TaradodStatus.AddRange(await Task.WhenAll(_clsTaradodFullForDisplay.ListTaradodDets.Select((TaradodFullDetView p) => p.GetStatuseTaradodAsync(token))));
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed)
					{
						((Control)LblCountStatus).Text = TaradodStatus.ListToString();
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

	private void InitCmsItems()
	{
		try
		{
			string deleteAttendance = Utilities.GetTranslate("[حذف تردد]");
			string changeTypeTaradod = Utilities.GetTranslate("[اصلاح نوع تردد]");
			string insertMorakhasi = Utilities.GetTranslate("[ثبت مرخصی روزانه]");
			string morakhasi_Daily = Utilities.GetTranslate("[گزارش مرخصی روزانه]");
			string gharadadList = Utilities.GetTranslate("[قراردادها]");
			string shiftList = Utilities.GetTranslate("[شیفت ها]");
			string personInfo = Utilities.GetTranslate("[مشخصات شخص]");
			string editManagementToolStripMenuItem = Utilities.GetTranslate("[اصلاحات مدیریتی تردد]");
			string cMEK = Utilities.GetTranslate("[اصلاح اضافه کار (مدیریتی)]");
			string cMKK = Utilities.GetTranslate("[اصلاح کسر کار (مدیریتی)]");
			string cMMission = Utilities.GetTranslate("[اصلاح ماموریت (مدیریتی)]");
			string cMVacation = Utilities.GetTranslate("[اصلاح مرخصی (مدیریتی)]");
			string toolStripSeparator5 = Utilities.GetTranslate("toolStripSeparator5");
			string cMFriday = Utilities.GetTranslate("[اصلاح جمعه کاری (مدیریتی)]");
			string cMSNobatkari1 = Utilities.GetTranslate("[اصلاح نوبت کاری صبح (مدیریتی)]");
			string cMSNobatkari2 = Utilities.GetTranslate("[اصلاح نوبت کاری عصر(مدیریتی)]");
			string cMSNobatkari3 = Utilities.GetTranslate("[اصلاح نوبت کاری شب (مدیریتی)]");
			string cMAbsent = Utilities.GetTranslate("[اصلاح غیبت (مدیریتی)]");
			string cMNight = Utilities.GetTranslate("[اصلاح شب کاری (مدیریتی)]");
			string cmsSelectAll = Utilities.GetTranslate("[انتخاب همه]");
			string cmsUnSelectAll = Utilities.GetTranslate("[پاک کردن همه]");
			string cmsReverseSelected = Utilities.GetTranslate("[معکوس کردن انتخاب ها]");
			string cmsGroupEdit = Utilities.GetTranslate("[افزودن گروهی تردد]");
			string text = Utilities.GetTranslate("[گزارش وضعیت کلیه پرسنل]");
			string toolStripMenuItem = Utilities.GetTranslate("[انتخاب]");
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				Task.Delay(100);
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					((ToolStripItem)CMS_DeleteAttendance).Text = deleteAttendance;
					((ToolStripItem)CMS_ChangeTypeTaradod).Text = changeTypeTaradod;
					((ToolStripItem)CMS_InsertMorakhasi).Text = insertMorakhasi;
					((ToolStripItem)CMS_Morakhasi_Daily).Text = morakhasi_Daily;
					((ToolStripItem)CMS_GharadadList).Text = gharadadList;
					((ToolStripItem)CMS_ShiftList).Text = shiftList;
					((ToolStripItem)CMS_PersonInfo).Text = personInfo;
					((ToolStripItem)EditManagementToolStripMenuItem).Text = editManagementToolStripMenuItem;
					((ToolStripItem)CMEK).Text = cMEK;
					((ToolStripItem)CMKK).Text = cMKK;
					((ToolStripItem)CMMission).Text = cMMission;
					((ToolStripItem)CMVacation).Text = cMVacation;
					((ToolStripItem)this.toolStripSeparator5).Name = toolStripSeparator5;
					((ToolStripItem)CMFriday).Text = cMFriday;
					((ToolStripItem)CMSNobatkari1).Text = cMSNobatkari1;
					((ToolStripItem)CMSNobatkari2).Text = cMSNobatkari2;
					((ToolStripItem)CMSNobatkari3).Text = cMSNobatkari3;
					((ToolStripItem)CMAbsent).Text = cMAbsent;
					((ToolStripItem)CMNight).Text = cMNight;
					((ToolStripItem)CmsSelectAll).Text = cmsSelectAll;
					((ToolStripItem)CmsUnSelectAll).Text = cmsUnSelectAll;
					((ToolStripItem)CmsReverseSelected).Text = cmsReverseSelected;
					((ToolStripItem)CmsGroupEdit).Text = cmsGroupEdit;
					((Control)this).Text = text;
					((ToolStripItem)انتخابToolStripMenuItem).Text = toolStripMenuItem;
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

	private void ReleaseEvent(bool state)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (state)
			{
				((Control)DGrid).KeyDown -= new KeyEventHandler(DGrid_KeyDown);
				DGrid.RowEnter -= new DataGridViewCellEventHandler(DGrid_RowEnter);
				DGrid.RowLeave -= new DataGridViewCellEventHandler(DGrid_RowLeave);
				DGrid.CellMouseDown -= new DataGridViewCellMouseEventHandler(DGrid_CellMouseDown);
			}
			else
			{
				((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
				DGrid.RowEnter += new DataGridViewCellEventHandler(DGrid_RowEnter);
				DGrid.RowLeave += new DataGridViewCellEventHandler(DGrid_RowLeave);
				DGrid.CellMouseDown += new DataGridViewCellMouseEventHandler(DGrid_CellMouseDown);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DGrid.RowEnter -= new DataGridViewCellEventHandler(DGrid_RowEnter);
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_loadGridToken?.Cancel();
				_loadGridToken?.Dispose();
				_loadGridToken = null;
				if (_clsTaradodFull != null)
				{
					_clsTaradodFull.OnProcced -= _cls_OnProcced;
					_clsTaradodFull.OnSummeryUpdate -= _cls_OnSummeryUpdate;
				}
				_clsTaradodFull = null;
				_clsTaradodFullForDisplay = null;
				_clsColor = null;
				_personIDs = null;
				_statuseTaradod = null;
				_dGridBackColors = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await Task.Delay(100);
			if (!token.IsCancellationRequested)
			{
				SearchString = ((Control)TxtSearch).Text.Trim();
				await FillDataAsync(token);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async void DGrid_Sorted(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_loadGridToken?.Cancel();
			_loadGridToken = new CancellationTokenSource();
			CancellationToken token = _loadGridToken.Token;
			await SetDgridCellColorsAsync(token);
			await SetDgridToolTipsAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void DGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.ColumnIndex >= ((DataGridViewBand)DG_V1Time).Index && e.ColumnIndex <= ((DataGridViewBand)DG_K8Time).Index)
			{
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
						LoadData();
					}
				}
			}
			else if (e.ColumnIndex < ((DataGridViewBand)DG_V1Time).Index && SelectedRow != null)
			{
				await ((CacheBaseForm)new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, DateM1, DateM2, PersonId, _date)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<TaradodFullDet> GetRowInfoAsync(int rowIndex, int columnIndex, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"in {DateTime.Now} method GetRowInfo");
		try
		{
			if (DGrid != null)
			{
				DateTime xDate = _date.Date.AddDays(1.0).AddSeconds(-1.0);
				List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
				TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonCodeTafsilAsync(((CacheBaseForm)this).ContextGuid, int.Parse(DGrid[((DataGridViewBand)DG_CodeTafsil).Index, rowIndex].Value.ToString()), token);
				GharardadNew pClsGharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)clsPerson).Id, xDate, token);
				TaradodFullDetView clsDet = await TaradodFullDet.GetAsync_(((CacheBaseForm)this).ContextGuid, devices, xDate, clsPerson, pClsGharardad, checkValidationVorodAndKHoroj: true, token);
				string xdesc = "[اصلاح اطلاعات تردد جناب] " + clsDet.FullName + " [در روز] " + clsDet.DateSh + "\n";
				string xdescbefor = "";
				string xdescAfter = "";
				CoupleTaradod[] coupleTaradodCurDay = clsDet.CoupleTaradodCurrentDay;
				int idx = (columnIndex - ((DataGridViewBand)DG_V1Time).Index) / 2;
				if (idx >= 0 && idx <= 7)
				{
					if ((columnIndex - ((DataGridViewBand)DG_V1Time).Index) % 2 == 0)
					{
						if (coupleTaradodCurDay[idx].Vtaradod?.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
						{
							string vTime = coupleTaradodCurDay[idx].VTime;
							if (vTime != null)
							{
								xdescbefor = xdescbefor + vTime + "-";
							}
						}
						await coupleTaradodCurDay[idx].SetVorodAsync(sb, "23:00", token);
						if (coupleTaradodCurDay[idx].Vtaradod?.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
						{
							string vTime2 = coupleTaradodCurDay[idx].VTime;
							if (vTime2 != null)
							{
								xdescAfter = xdescAfter + vTime2 + "-";
							}
						}
					}
					else
					{
						if (coupleTaradodCurDay[idx].Ktaradod?.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
						{
							xdescbefor = xdescbefor + coupleTaradodCurDay[idx].KTime + "-";
						}
						await coupleTaradodCurDay[idx].SetKhorojAsync(sb, "22:00", token);
						if (coupleTaradodCurDay[idx].Ktaradod?.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
						{
							string kTime = coupleTaradodCurDay[idx].KTime;
							if (kTime != null)
							{
								xdescAfter = xdescAfter + kTime + "-";
							}
						}
					}
				}
				char[] trimChars = new char[1] { '-' };
				if (xdescbefor != "")
				{
					xdesc = xdesc + "\n [اطلاعات تردد قبل از ویرایش] : " + xdescbefor.TrimEnd(trimChars);
				}
				if (xdescAfter != "")
				{
					xdesc = xdesc + "\n [اطلاعات تردد بعد از ویرایش]  : " + xdescAfter.TrimEnd(trimChars);
				}
				await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "Taradod.TaradodUpdate");
				return clsDet;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
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
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Expected O, but got Unknown
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Expected O, but got Unknown
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_142d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1437: Expected O, but got Unknown
		//IL_19f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a00: Expected O, but got Unknown
		//IL_1c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c53: Expected O, but got Unknown
		//IL_1c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6a: Expected O, but got Unknown
		//IL_1c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c81: Expected O, but got Unknown
		//IL_1c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c98: Expected O, but got Unknown
		//IL_1ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1caf: Expected O, but got Unknown
		//IL_1cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc6: Expected O, but got Unknown
		//IL_1cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf4: Expected O, but got Unknown
		//IL_2dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dcc: Expected O, but got Unknown
		//IL_2e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec2: Expected O, but got Unknown
		//IL_2efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbb: Expected O, but got Unknown
		//IL_2ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31dc: Expected O, but got Unknown
		//IL_3315: Unknown result type (might be due to invalid IL or missing references)
		//IL_331f: Expected O, but got Unknown
		//IL_33a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b3: Expected O, but got Unknown
		//IL_3831: Unknown result type (might be due to invalid IL or missing references)
		//IL_383b: Expected O, but got Unknown
		//IL_3880: Unknown result type (might be due to invalid IL or missing references)
		//IL_388a: Expected O, but got Unknown
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
		DataGridViewCellStyle val16 = new DataGridViewCellStyle();
		DataGridViewCellStyle val17 = new DataGridViewCellStyle();
		DataGridViewCellStyle val18 = new DataGridViewCellStyle();
		DataGridViewCellStyle val19 = new DataGridViewCellStyle();
		DataGridViewCellStyle val20 = new DataGridViewCellStyle();
		DataGridViewCellStyle val21 = new DataGridViewCellStyle();
		DataGridViewCellStyle val22 = new DataGridViewCellStyle();
		DataGridViewCellStyle val23 = new DataGridViewCellStyle();
		DataGridViewCellStyle val24 = new DataGridViewCellStyle();
		DataGridViewCellStyle val25 = new DataGridViewCellStyle();
		DataGridViewCellStyle val26 = new DataGridViewCellStyle();
		DataGridViewCellStyle val27 = new DataGridViewCellStyle();
		DataGridViewCellStyle val28 = new DataGridViewCellStyle();
		DataGridViewCellStyle val29 = new DataGridViewCellStyle();
		DataGridViewCellStyle val30 = new DataGridViewCellStyle();
		DataGridViewCellStyle val31 = new DataGridViewCellStyle();
		DataGridViewCellStyle val32 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmRptTaradodFullAll));
		CMS = new ContextMenuStrip(components);
		CMS_DeleteAttendance = new ToolStripMenuItem();
		CMS_ChangeTypeTaradod = new ToolStripMenuItem();
		toolStripSeparator2 = new ToolStripSeparator();
		CmsRequestedVacation = new ToolStripMenuItem();
		CMS_InsertMorakhasi = new ToolStripMenuItem();
		CMS_Morakhasi_Daily = new ToolStripMenuItem();
		CMS_GharadadList = new ToolStripMenuItem();
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
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		CmsSelectAll = new ToolStripMenuItem();
		CmsUnSelectAll = new ToolStripMenuItem();
		CmsReverseSelected = new ToolStripMenuItem();
		CmsGroupEdit = new ToolStripMenuItem();
		taradodFullDetViewBindingSource = new BindingSource(components);
		panelEx1 = new PanelEx();
		labelX4 = new LabelX();
		LblDateM = new LinkLabel();
		labelX8 = new LabelX();
		LblNoContract = new LabelX();
		labelX10 = new LabelX();
		LblTaradodNaghes = new LabelX();
		labelX3 = new LabelX();
		LblAbsent = new LabelX();
		labelX5 = new LabelX();
		LblTaradodKamel = new LabelX();
		lblSearch = new LabelX();
		TxtSearch = new TextBoxX();
		DG_Check = new DataGridViewCheckBoxColumn();
		DG_Index = new DataGridViewTextBoxColumn();
		DG_CodeTafsil = new DataGridViewTextBoxColumn();
		DG_FullName = new DataGridViewTextBoxColumn();
		DG_TaradodStatusName = new DataGridViewTextBoxColumn();
		DG_ShiftTime = new DataGridViewTextBoxColumn();
		DG_Hozor = new DataGridViewTextBoxColumn();
		DG_EzafeKar = new DataGridViewTextBoxColumn();
		DG_ManulaEzafeKarTime = new DataGridViewTextBoxColumn();
		DG_KasreKar = new DataGridViewTextBoxColumn();
		DG_ManulaKasreKarTime = new DataGridViewTextBoxColumn();
		DG_ManulaMissionTime = new DataGridViewTextBoxColumn();
		DG_ManulaVacationTime = new DataGridViewTextBoxColumn();
		DG_ManulaFridayTime = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime1 = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime2 = new DataGridViewTextBoxColumn();
		DG_ManulaShiftTime3 = new DataGridViewTextBoxColumn();
		DG_ManulaAbsentTime = new DataGridViewTextBoxColumn();
		DG_ManulaNightTime = new DataGridViewTextBoxColumn();
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
		progressBar1 = new ProgressBar();
		LblCountStatus = new LabelX();
		btnBack = new ButtonX();
		btnFilter = new ButtonX();
		btnNext = new ButtonX();
		labelX6 = new LabelX();
		Lbldate = new LinkLabel();
		labelX1 = new LabelX();
		Lblkasr = new LabelX();
		PicPerson = new PictureBox();
		BtnPrint = new ButtonX();
		LblKasrsum = new LabelX();
		LblhozorSum = new LabelX();
		LblRow = new LabelX();
		LblEzafe = new LabelX();
		LblHozor = new LabelX();
		LblRows = new LabelX();
		labelX2 = new LabelX();
		DGrid = new DataGridView();
		((Control)CMS).SuspendLayout();
		((ISupportInitialize)taradodFullDetViewBindingSource).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)PicPerson).BeginInit();
		((Control)this).SuspendLayout();
		((ToolStrip)CMS).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[14]
		{
			(ToolStripItem)CMS_DeleteAttendance,
			(ToolStripItem)CMS_ChangeTypeTaradod,
			(ToolStripItem)toolStripSeparator2,
			(ToolStripItem)CmsRequestedVacation,
			(ToolStripItem)CMS_InsertMorakhasi,
			(ToolStripItem)CMS_Morakhasi_Daily,
			(ToolStripItem)CMS_GharadadList,
			(ToolStripItem)CMS_ShiftList,
			(ToolStripItem)toolStripSeparator1,
			(ToolStripItem)CMS_PersonInfo,
			(ToolStripItem)toolStripSeparator3,
			(ToolStripItem)EditManagementToolStripMenuItem,
			(ToolStripItem)انتخابToolStripMenuItem,
			(ToolStripItem)CmsGroupEdit
		});
		((Control)CMS).Name = "CMS";
		((Control)CMS).RightToLeft = (RightToLeft)1;
		((Control)CMS).Size = new Size(201, 264);
		((ToolStripDropDown)CMS).Opened += CMS_Opened;
		((ToolStripItem)CMS_DeleteAttendance).Name = "CMS_DeleteAttendance";
		((ToolStripItem)CMS_DeleteAttendance).Size = new Size(200, 22);
		((ToolStripItem)CMS_DeleteAttendance).Text = "[حذف تردد]";
		((ToolStripItem)CMS_DeleteAttendance).Click += CMS_DeleteAttendance_Click;
		((ToolStripItem)CMS_ChangeTypeTaradod).Name = "CMS_ChangeTypeTaradod";
		((ToolStripItem)CMS_ChangeTypeTaradod).Size = new Size(200, 22);
		((ToolStripItem)CMS_ChangeTypeTaradod).Text = "[اصلاح نوع تردد]";
		((ToolStripItem)CMS_ChangeTypeTaradod).Click += CMS_ChangeTypeTaradod_Click;
		((ToolStripItem)toolStripSeparator2).Name = "toolStripSeparator2";
		((ToolStripItem)toolStripSeparator2).Size = new Size(197, 6);
		((ToolStripItem)CmsRequestedVacation).Name = "CmsRequestedVacation";
		((ToolStripItem)CmsRequestedVacation).Size = new Size(200, 22);
		((ToolStripItem)CmsRequestedVacation).Text = "گزارش مرخصی الکترونیکی";
		((ToolStripItem)CmsRequestedVacation).Click += CmsRequestedVacation_Click;
		((ToolStripItem)CMS_InsertMorakhasi).Name = "CMS_InsertMorakhasi";
		((ToolStripItem)CMS_InsertMorakhasi).Size = new Size(200, 22);
		((ToolStripItem)CMS_InsertMorakhasi).Text = "[ثبت مرخصی روزانه]";
		((ToolStripItem)CMS_InsertMorakhasi).Click += CMS_InsertMorakhasi_Click;
		((ToolStripItem)CMS_Morakhasi_Daily).Name = "CMS_Morakhasi_Daily";
		((ToolStripItem)CMS_Morakhasi_Daily).Size = new Size(200, 22);
		((ToolStripItem)CMS_Morakhasi_Daily).Text = "[گزارش مرخصی روزانه]";
		((ToolStripItem)CMS_Morakhasi_Daily).Click += CMS_Morakhasi_Daily_Click;
		((ToolStripItem)CMS_GharadadList).Name = "CMS_GharadadList";
		((ToolStripItem)CMS_GharadadList).Size = new Size(200, 22);
		((ToolStripItem)CMS_GharadadList).Text = "[قراردادها]";
		((ToolStripItem)CMS_GharadadList).Click += CMS_GharadadList_Click;
		((ToolStripItem)CMS_ShiftList).Name = "CMS_ShiftList";
		((ToolStripItem)CMS_ShiftList).Size = new Size(200, 22);
		((ToolStripItem)CMS_ShiftList).Text = "[شیفت ها]";
		((ToolStripItem)CMS_ShiftList).Click += CMS_ShiftList_Click;
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(197, 6);
		((ToolStripItem)CMS_PersonInfo).Name = "CMS_PersonInfo";
		((ToolStripItem)CMS_PersonInfo).Size = new Size(200, 22);
		((ToolStripItem)CMS_PersonInfo).Text = "[مشخصات شخص]";
		((ToolStripItem)CMS_PersonInfo).Click += CMS_PersonInfo_Click;
		((ToolStripItem)toolStripSeparator3).Name = "toolStripSeparator3";
		((ToolStripItem)toolStripSeparator3).Size = new Size(197, 6);
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
		((ToolStripItem)EditManagementToolStripMenuItem).Size = new Size(200, 22);
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
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)CmsSelectAll,
			(ToolStripItem)CmsUnSelectAll,
			(ToolStripItem)CmsReverseSelected
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(200, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)CmsSelectAll).Name = "CmsSelectAll";
		((ToolStripItem)CmsSelectAll).Size = new Size(198, 22);
		((ToolStripItem)CmsSelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)CmsSelectAll).Click += CmsSelectAll_Click;
		((ToolStripItem)CmsUnSelectAll).Name = "CmsUnSelectAll";
		((ToolStripItem)CmsUnSelectAll).Size = new Size(198, 22);
		((ToolStripItem)CmsUnSelectAll).Text = "[پاک کردن همه]";
		((ToolStripItem)CmsUnSelectAll).Click += CmsUnSelectAll_Click;
		((ToolStripItem)CmsReverseSelected).Name = "CmsReverseSelected";
		((ToolStripItem)CmsReverseSelected).Size = new Size(198, 22);
		((ToolStripItem)CmsReverseSelected).Text = "[معکوس کردن انتخاب ها]";
		((ToolStripItem)CmsReverseSelected).Click += CmsReverseSelected_Click;
		((ToolStripItem)CmsGroupEdit).Name = "CmsGroupEdit";
		((ToolStripItem)CmsGroupEdit).Size = new Size(200, 22);
		((ToolStripItem)CmsGroupEdit).Text = "افزودن گروهی تردد";
		((ToolStripItem)CmsGroupEdit).Visible = false;
		((ToolStripItem)CmsGroupEdit).Click += CmsGroupEdit_Click;
		taradodFullDetViewBindingSource.DataSource = typeof(TaradodFullDetView);
		taradodFullDetViewBindingSource.CurrentChanged += taradodFullDetViewBindingSource_CurrentChanged;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)LblDateM);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)LblNoContract);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)LblTaradodNaghes);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)LblAbsent);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)LblTaradodKamel);
		((Control)panelEx1).Controls.Add((Control)(object)lblSearch);
		((Control)panelEx1).Controls.Add((Control)(object)TxtSearch);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)progressBar1);
		((Control)panelEx1).Controls.Add((Control)(object)LblCountStatus);
		((Control)panelEx1).Controls.Add((Control)(object)btnBack);
		((Control)panelEx1).Controls.Add((Control)(object)btnFilter);
		((Control)panelEx1).Controls.Add((Control)(object)btnNext);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)Lbldate);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)Lblkasr);
		((Control)panelEx1).Controls.Add((Control)(object)PicPerson);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)LblKasrsum);
		((Control)panelEx1).Controls.Add((Control)(object)LblhozorSum);
		((Control)panelEx1).Controls.Add((Control)(object)LblRow);
		((Control)panelEx1).Controls.Add((Control)(object)LblEzafe);
		((Control)panelEx1).Controls.Add((Control)(object)LblHozor);
		((Control)panelEx1).Controls.Add((Control)(object)LblRows);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1213, 730);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(1072, 29);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(73, 20);
		((Control)labelX4).TabIndex = 94;
		((Control)labelX4).Text = "   تاریخ میلادی:";
		((Control)LblDateM).Anchor = (AnchorStyles)9;
		((Control)LblDateM).ForeColor = Color.DarkRed;
		LblDateM.LinkBehavior = (LinkBehavior)3;
		LblDateM.LinkColor = Color.DarkRed;
		((Control)LblDateM).Location = new Point(981, 33);
		((Control)LblDateM).Name = "LblDateM";
		((Control)LblDateM).RightToLeft = (RightToLeft)1;
		((Control)LblDateM).Size = new Size(81, 13);
		((Control)LblDateM).TabIndex = 93;
		LblDateM.TabStop = true;
		((Control)LblDateM).Text = "-------";
		((Label)LblDateM).TextAlign = (ContentAlignment)16;
		LblDateM.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)labelX8).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(523, 713);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(95, 14);
		((Control)labelX8).TabIndex = 90;
		((Control)labelX8).Text = "تعداد عدم قرارداد:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)LblNoContract).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblNoContract).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblNoContract).ForeColor = Color.DarkRed;
		((Control)LblNoContract).Location = new Point(473, 713);
		((Control)LblNoContract).Name = "LblNoContract";
		((Control)LblNoContract).Size = new Size(44, 14);
		((Control)LblNoContract).TabIndex = 92;
		((Control)LblNoContract).Text = "------";
		((Control)labelX10).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(523, 696);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(95, 14);
		((Control)labelX10).TabIndex = 89;
		((Control)labelX10).Text = "تعداد تردد ناقص:";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)LblTaradodNaghes).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblTaradodNaghes).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTaradodNaghes).ForeColor = Color.DarkRed;
		((Control)LblTaradodNaghes).Location = new Point(473, 696);
		((Control)LblTaradodNaghes).Name = "LblTaradodNaghes";
		((Control)LblTaradodNaghes).Size = new Size(44, 14);
		((Control)LblTaradodNaghes).TabIndex = 91;
		((Control)LblTaradodNaghes).Text = "------";
		((Control)labelX3).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(695, 713);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(79, 14);
		((Control)labelX3).TabIndex = 86;
		((Control)labelX3).Text = "تعداد غیبت:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)LblAbsent).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblAbsent).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblAbsent).ForeColor = Color.DarkRed;
		((Control)LblAbsent).Location = new Point(634, 713);
		((Control)LblAbsent).Name = "LblAbsent";
		((Control)LblAbsent).Size = new Size(55, 14);
		((Control)LblAbsent).TabIndex = 88;
		((Control)LblAbsent).Text = "------";
		((Control)labelX5).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(695, 696);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(79, 14);
		((Control)labelX5).TabIndex = 85;
		((Control)labelX5).Text = "تعداد تردد کامل:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)LblTaradodKamel).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblTaradodKamel).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTaradodKamel).ForeColor = Color.DarkRed;
		((Control)LblTaradodKamel).Location = new Point(634, 696);
		((Control)LblTaradodKamel).Name = "LblTaradodKamel";
		((Control)LblTaradodKamel).Size = new Size(55, 14);
		((Control)LblTaradodKamel).TabIndex = 87;
		((Control)LblTaradodKamel).Text = "------";
		((Control)lblSearch).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblSearch).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblSearch).Location = new Point(903, 19);
		((Control)lblSearch).Name = "lblSearch";
		((Control)lblSearch).Size = new Size(57, 23);
		((Control)lblSearch).TabIndex = 84;
		((Control)lblSearch).Text = "[جستجو]  :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(487, 20);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(412, 20);
		((Control)TxtSearch).TabIndex = 83;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		DGrid.AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = Color.SkyBlue;
		val2.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.WindowText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)1;
		DGrid.ColumnHeadersDefaultCellStyle = val2;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[35]
		{
			(DataGridViewColumn)DG_Check,
			(DataGridViewColumn)DG_Index,
			(DataGridViewColumn)DG_CodeTafsil,
			(DataGridViewColumn)DG_FullName,
			(DataGridViewColumn)DG_TaradodStatusName,
			(DataGridViewColumn)DG_ShiftTime,
			(DataGridViewColumn)DG_Hozor,
			(DataGridViewColumn)DG_EzafeKar,
			(DataGridViewColumn)DG_ManulaEzafeKarTime,
			(DataGridViewColumn)DG_KasreKar,
			(DataGridViewColumn)DG_ManulaKasreKarTime,
			(DataGridViewColumn)DG_ManulaMissionTime,
			(DataGridViewColumn)DG_ManulaVacationTime,
			(DataGridViewColumn)DG_ManulaFridayTime,
			(DataGridViewColumn)DG_ManulaShiftTime1,
			(DataGridViewColumn)DG_ManulaShiftTime2,
			(DataGridViewColumn)DG_ManulaShiftTime3,
			(DataGridViewColumn)DG_ManulaAbsentTime,
			(DataGridViewColumn)DG_ManulaNightTime,
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
		DGrid.DataSource = taradodFullDetViewBindingSource;
		((Control)DGrid).Location = new Point(0, 69);
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		DGrid.RowHeadersVisible = false;
		((Control)DGrid).Size = new Size(1213, 621);
		((Control)DGrid).TabIndex = 82;
		DGrid.CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		DGrid.CellDoubleClick += new DataGridViewCellEventHandler(DGrid_CellDoubleClick);
		DGrid.CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		DGrid.CellMouseDown += new DataGridViewCellMouseEventHandler(DGrid_CellMouseDown);
		DGrid.RowLeave += new DataGridViewCellEventHandler(DGrid_RowLeave);
		DGrid.Scroll += new ScrollEventHandler(DGrid_Scroll);
		DGrid.Sorted += DGrid_Sorted;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DG_Check).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Check).DataPropertyName = "IsChecked";
		((DataGridViewColumn)DG_Check).HeaderText = "";
		((DataGridViewColumn)DG_Check).MinimumWidth = 15;
		((DataGridViewColumn)DG_Check).Name = "DG_Check";
		((DataGridViewBand)DG_Check).ReadOnly = true;
		((DataGridViewColumn)DG_Check).Width = 15;
		((DataGridViewColumn)DG_Index).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Index).DataPropertyName = "Index";
		((DataGridViewColumn)DG_Index).HeaderText = "R";
		((DataGridViewColumn)DG_Index).MinimumWidth = 15;
		((DataGridViewColumn)DG_Index).Name = "DG_Index";
		((DataGridViewBand)DG_Index).ReadOnly = true;
		((DataGridViewColumn)DG_Index).Width = 40;
		((DataGridViewColumn)DG_CodeTafsil).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_CodeTafsil).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)DG_CodeTafsil).HeaderText = "کد شخص";
		((DataGridViewColumn)DG_CodeTafsil).MinimumWidth = 15;
		((DataGridViewColumn)DG_CodeTafsil).Name = "CodeTafsil";
		((DataGridViewBand)DG_CodeTafsil).ReadOnly = true;
		((DataGridViewColumn)DG_CodeTafsil).Width = 74;
		((DataGridViewColumn)DG_FullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_FullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DG_FullName).HeaderText = "نام شخص";
		((DataGridViewColumn)DG_FullName).MinimumWidth = 100;
		((DataGridViewColumn)DG_FullName).Name = "DG_FullName";
		((DataGridViewBand)DG_FullName).ReadOnly = true;
		((DataGridViewColumn)DG_TaradodStatusName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DG_TaradodStatusName).DataPropertyName = "TaradodStatusName";
		((DataGridViewColumn)DG_TaradodStatusName).HeaderText = "وضعیت تردد";
		((DataGridViewColumn)DG_TaradodStatusName).MinimumWidth = 100;
		((DataGridViewColumn)DG_TaradodStatusName).Name = "DG_TaradodStatusName";
		((DataGridViewBand)DG_TaradodStatusName).ReadOnly = true;
		((DataGridViewColumn)DG_ShiftTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ShiftTime).DataPropertyName = "ShiftTime";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ShiftTime).DefaultCellStyle = val3;
		((DataGridViewColumn)DG_ShiftTime).HeaderText = "شیفت";
		((DataGridViewColumn)DG_ShiftTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ShiftTime).Name = "DG_ShiftTime";
		((DataGridViewBand)DG_ShiftTime).ReadOnly = true;
		((DataGridViewBand)DG_ShiftTime).Visible = false;
		((DataGridViewColumn)DG_Hozor).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Hozor).DataPropertyName = "Hozor";
		val4.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_Hozor).DefaultCellStyle = val4;
		((DataGridViewColumn)DG_Hozor).HeaderText = "حضور";
		((DataGridViewColumn)DG_Hozor).MinimumWidth = 40;
		((DataGridViewColumn)DG_Hozor).Name = "DG_Hozor";
		((DataGridViewBand)DG_Hozor).ReadOnly = true;
		((DataGridViewColumn)DG_Hozor).Width = 59;
		((DataGridViewColumn)DG_EzafeKar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_EzafeKar).DataPropertyName = "EzafeKar";
		val5.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_EzafeKar).DefaultCellStyle = val5;
		((DataGridViewColumn)DG_EzafeKar).HeaderText = "اضافه کار";
		((DataGridViewColumn)DG_EzafeKar).MinimumWidth = 40;
		((DataGridViewColumn)DG_EzafeKar).Name = "DG_EzafeKar";
		((DataGridViewBand)DG_EzafeKar).ReadOnly = true;
		((DataGridViewBand)DG_EzafeKar).Visible = false;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).DataPropertyName = "ManulaEzafeKarTime";
		val6.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaEzafeKarTime).DefaultCellStyle = val6;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).HeaderText = "اضافه ک م";
		((DataGridViewColumn)DG_ManulaEzafeKarTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaEzafeKarTime).Name = "DG_ManulaEzafeKarTime";
		((DataGridViewBand)DG_ManulaEzafeKarTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaEzafeKarTime).Visible = false;
		((DataGridViewColumn)DG_KasreKar).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_KasreKar).DataPropertyName = "KasreKar";
		val7.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_KasreKar).DefaultCellStyle = val7;
		((DataGridViewColumn)DG_KasreKar).HeaderText = "کسر کار";
		((DataGridViewColumn)DG_KasreKar).MinimumWidth = 40;
		((DataGridViewColumn)DG_KasreKar).Name = "DG_KasreKar";
		((DataGridViewBand)DG_KasreKar).ReadOnly = true;
		((DataGridViewBand)DG_KasreKar).Visible = false;
		((DataGridViewColumn)DG_ManulaKasreKarTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaKasreKarTime).DataPropertyName = "ManulaKasreKarTime";
		val8.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaKasreKarTime).DefaultCellStyle = val8;
		((DataGridViewColumn)DG_ManulaKasreKarTime).HeaderText = "کسر ک مد";
		((DataGridViewColumn)DG_ManulaKasreKarTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaKasreKarTime).Name = "DG_ManulaKasreKarTime";
		((DataGridViewBand)DG_ManulaKasreKarTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaKasreKarTime).Visible = false;
		((DataGridViewColumn)DG_ManulaMissionTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaMissionTime).DataPropertyName = "ManulaMissionTime";
		val9.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaMissionTime).DefaultCellStyle = val9;
		((DataGridViewColumn)DG_ManulaMissionTime).HeaderText = "ماموریت مد";
		((DataGridViewColumn)DG_ManulaMissionTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaMissionTime).Name = "DG_ManulaMissionTime";
		((DataGridViewBand)DG_ManulaMissionTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaMissionTime).Visible = false;
		((DataGridViewColumn)DG_ManulaVacationTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaVacationTime).DataPropertyName = "ManulaVacationTime";
		val10.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaVacationTime).DefaultCellStyle = val10;
		((DataGridViewColumn)DG_ManulaVacationTime).HeaderText = "مرخصی مد";
		((DataGridViewColumn)DG_ManulaVacationTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaVacationTime).Name = "DG_ManulaVacationTime";
		((DataGridViewBand)DG_ManulaVacationTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaVacationTime).Visible = false;
		((DataGridViewColumn)DG_ManulaFridayTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaFridayTime).DataPropertyName = "ManulaFridayTime";
		val11.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaFridayTime).DefaultCellStyle = val11;
		((DataGridViewColumn)DG_ManulaFridayTime).HeaderText = "جمعه کاری مد";
		((DataGridViewColumn)DG_ManulaFridayTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaFridayTime).Name = "DG_ManulaFridayTime";
		((DataGridViewBand)DG_ManulaFridayTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaFridayTime).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaShiftTime1).DataPropertyName = "ManulaShiftTime1";
		val12.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaShiftTime1).DefaultCellStyle = val12;
		((DataGridViewColumn)DG_ManulaShiftTime1).HeaderText = "نوبت کاری1 مد";
		((DataGridViewColumn)DG_ManulaShiftTime1).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaShiftTime1).Name = "DG_ManulaShiftTime1";
		((DataGridViewBand)DG_ManulaShiftTime1).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime1).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaShiftTime2).DataPropertyName = "ManulaShiftTime2";
		val13.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaShiftTime2).DefaultCellStyle = val13;
		((DataGridViewColumn)DG_ManulaShiftTime2).HeaderText = "نوبت کاری2 مد";
		((DataGridViewColumn)DG_ManulaShiftTime2).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaShiftTime2).Name = "DG_ManulaShiftTime2";
		((DataGridViewBand)DG_ManulaShiftTime2).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime2).Visible = false;
		((DataGridViewColumn)DG_ManulaShiftTime3).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaShiftTime3).DataPropertyName = "ManulaShiftTime3";
		val14.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaShiftTime3).DefaultCellStyle = val14;
		((DataGridViewColumn)DG_ManulaShiftTime3).HeaderText = "نوبت کاری3 مد";
		((DataGridViewColumn)DG_ManulaShiftTime3).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaShiftTime3).Name = "DG_ManulaShiftTime3";
		((DataGridViewBand)DG_ManulaShiftTime3).ReadOnly = true;
		((DataGridViewBand)DG_ManulaShiftTime3).Visible = false;
		((DataGridViewColumn)DG_ManulaAbsentTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaAbsentTime).DataPropertyName = "ManulaAbsentTime";
		val15.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaAbsentTime).DefaultCellStyle = val15;
		((DataGridViewColumn)DG_ManulaAbsentTime).HeaderText = "غیبت مد";
		((DataGridViewColumn)DG_ManulaAbsentTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaAbsentTime).Name = "DG_ManulaAbsentTime";
		((DataGridViewBand)DG_ManulaAbsentTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaAbsentTime).Visible = false;
		((DataGridViewColumn)DG_ManulaNightTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)DG_ManulaNightTime).DataPropertyName = "ManulaNightTime";
		val16.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_ManulaNightTime).DefaultCellStyle = val16;
		((DataGridViewColumn)DG_ManulaNightTime).HeaderText = "شبکاری مد";
		((DataGridViewColumn)DG_ManulaNightTime).MinimumWidth = 40;
		((DataGridViewColumn)DG_ManulaNightTime).Name = "DG_ManulaNightTime";
		((DataGridViewBand)DG_ManulaNightTime).ReadOnly = true;
		((DataGridViewBand)DG_ManulaNightTime).Visible = false;
		((DataGridViewColumn)DG_V1Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V1Time).DataPropertyName = "V1Time";
		val17.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V1Time).DefaultCellStyle = val17;
		((DataGridViewColumn)DG_V1Time).HeaderText = "ورود1";
		((DataGridViewColumn)DG_V1Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V1Time).Name = "DG_V1Time";
		((DataGridViewBand)DG_V1Time).ReadOnly = true;
		((DataGridViewColumn)DG_V1Time).Width = 60;
		((DataGridViewColumn)DG_K1Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K1Time).DataPropertyName = "K1Time";
		val18.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K1Time).DefaultCellStyle = val18;
		((DataGridViewColumn)DG_K1Time).HeaderText = "خروج1";
		((DataGridViewColumn)DG_K1Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K1Time).Name = "DG_K1Time";
		((DataGridViewBand)DG_K1Time).ReadOnly = true;
		((DataGridViewColumn)DG_K1Time).Width = 64;
		((DataGridViewColumn)DG_V2Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V2Time).DataPropertyName = "V2Time";
		val19.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V2Time).DefaultCellStyle = val19;
		((DataGridViewColumn)DG_V2Time).HeaderText = "ورود2";
		((DataGridViewColumn)DG_V2Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V2Time).Name = "DG_V2Time";
		((DataGridViewBand)DG_V2Time).ReadOnly = true;
		((DataGridViewBand)DG_V2Time).Visible = false;
		((DataGridViewColumn)DG_K2Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K2Time).DataPropertyName = "K2Time";
		val20.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K2Time).DefaultCellStyle = val20;
		((DataGridViewColumn)DG_K2Time).HeaderText = "خروج2";
		((DataGridViewColumn)DG_K2Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K2Time).Name = "DG_K2Time";
		((DataGridViewBand)DG_K2Time).ReadOnly = true;
		((DataGridViewBand)DG_K2Time).Visible = false;
		((DataGridViewColumn)DG_V3Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V3Time).DataPropertyName = "V3Time";
		val21.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V3Time).DefaultCellStyle = val21;
		((DataGridViewColumn)DG_V3Time).HeaderText = "ورود3";
		((DataGridViewColumn)DG_V3Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V3Time).Name = "DG_V3Time";
		((DataGridViewBand)DG_V3Time).ReadOnly = true;
		((DataGridViewBand)DG_V3Time).Visible = false;
		((DataGridViewColumn)DG_K3Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K3Time).DataPropertyName = "K3Time";
		val22.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K3Time).DefaultCellStyle = val22;
		((DataGridViewColumn)DG_K3Time).HeaderText = "خروج3";
		((DataGridViewColumn)DG_K3Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K3Time).Name = "DG_K3Time";
		((DataGridViewBand)DG_K3Time).ReadOnly = true;
		((DataGridViewBand)DG_K3Time).Visible = false;
		((DataGridViewColumn)DG_V4Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V4Time).DataPropertyName = "V4Time";
		val23.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V4Time).DefaultCellStyle = val23;
		((DataGridViewColumn)DG_V4Time).HeaderText = "ورود4";
		((DataGridViewColumn)DG_V4Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V4Time).Name = "DG_V4Time";
		((DataGridViewBand)DG_V4Time).ReadOnly = true;
		((DataGridViewBand)DG_V4Time).Visible = false;
		((DataGridViewColumn)DG_K4Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K4Time).DataPropertyName = "K4Time";
		val24.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K4Time).DefaultCellStyle = val24;
		((DataGridViewColumn)DG_K4Time).HeaderText = "خروج4";
		((DataGridViewColumn)DG_K4Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K4Time).Name = "DG_K4Time";
		((DataGridViewBand)DG_K4Time).ReadOnly = true;
		((DataGridViewBand)DG_K4Time).Visible = false;
		((DataGridViewColumn)DG_V5Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V5Time).DataPropertyName = "V5Time";
		val25.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V5Time).DefaultCellStyle = val25;
		((DataGridViewColumn)DG_V5Time).HeaderText = "ورود5";
		((DataGridViewColumn)DG_V5Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V5Time).Name = "DG_V5Time";
		((DataGridViewBand)DG_V5Time).ReadOnly = true;
		((DataGridViewBand)DG_V5Time).Visible = false;
		((DataGridViewColumn)DG_K5Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K5Time).DataPropertyName = "K5Time";
		val26.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K5Time).DefaultCellStyle = val26;
		((DataGridViewColumn)DG_K5Time).HeaderText = "خروج5";
		((DataGridViewColumn)DG_K5Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K5Time).Name = "DG_K5Time";
		((DataGridViewBand)DG_K5Time).ReadOnly = true;
		((DataGridViewBand)DG_K5Time).Visible = false;
		((DataGridViewColumn)DG_V6Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V6Time).DataPropertyName = "V6Time";
		val27.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V6Time).DefaultCellStyle = val27;
		((DataGridViewColumn)DG_V6Time).HeaderText = "ورود6";
		((DataGridViewColumn)DG_V6Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V6Time).Name = "DG_V6Time";
		((DataGridViewBand)DG_V6Time).ReadOnly = true;
		((DataGridViewBand)DG_V6Time).Visible = false;
		((DataGridViewColumn)DG_K6Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K6Time).DataPropertyName = "K6Time";
		val28.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K6Time).DefaultCellStyle = val28;
		((DataGridViewColumn)DG_K6Time).HeaderText = "خروج6";
		((DataGridViewColumn)DG_K6Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K6Time).Name = "DG_K6Time";
		((DataGridViewBand)DG_K6Time).ReadOnly = true;
		((DataGridViewBand)DG_K6Time).Visible = false;
		((DataGridViewColumn)DG_V7Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V7Time).DataPropertyName = "V7Time";
		val29.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V7Time).DefaultCellStyle = val29;
		((DataGridViewColumn)DG_V7Time).HeaderText = "ورود7";
		((DataGridViewColumn)DG_V7Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V7Time).Name = "DG_V7Time";
		((DataGridViewBand)DG_V7Time).ReadOnly = true;
		((DataGridViewBand)DG_V7Time).Visible = false;
		((DataGridViewColumn)DG_K7Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K7Time).DataPropertyName = "K7Time";
		val30.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K7Time).DefaultCellStyle = val30;
		((DataGridViewColumn)DG_K7Time).HeaderText = "خروج7";
		((DataGridViewColumn)DG_K7Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K7Time).Name = "DG_K7Time";
		((DataGridViewBand)DG_K7Time).ReadOnly = true;
		((DataGridViewBand)DG_K7Time).Visible = false;
		((DataGridViewColumn)DG_V8Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_V8Time).DataPropertyName = "V8Time";
		val31.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_V8Time).DefaultCellStyle = val31;
		((DataGridViewColumn)DG_V8Time).HeaderText = "ورود8";
		((DataGridViewColumn)DG_V8Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_V8Time).Name = "DG_V8Time";
		((DataGridViewBand)DG_V8Time).ReadOnly = true;
		((DataGridViewBand)DG_V8Time).Visible = false;
		((DataGridViewColumn)DG_K8Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_K8Time).DataPropertyName = "K8Time";
		val32.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DG_K8Time).DefaultCellStyle = val32;
		((DataGridViewColumn)DG_K8Time).HeaderText = "خروج8";
		((DataGridViewColumn)DG_K8Time).MinimumWidth = 40;
		((DataGridViewColumn)DG_K8Time).Name = "DG_K8Time";
		((DataGridViewBand)DG_K8Time).ReadOnly = true;
		((DataGridViewBand)DG_K8Time).Visible = false;
		((Control)progressBar1).Anchor = (AnchorStyles)13;
		((Control)progressBar1).Location = new Point(0, 56);
		((Control)progressBar1).Name = "progressBar1";
		((Control)progressBar1).Size = new Size(1213, 13);
		((Control)progressBar1).TabIndex = 81;
		((Control)LblCountStatus).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblCountStatus).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountStatus).ForeColor = Color.Navy;
		((Control)LblCountStatus).Location = new Point(208, 663);
		((Control)LblCountStatus).Name = "LblCountStatus";
		((Control)LblCountStatus).Size = new Size(995, 27);
		((Control)LblCountStatus).TabIndex = 79;
		((Control)LblCountStatus).Text = "------";
		((Control)btnBack).AccessibleRole = (AccessibleRole)43;
		btnBack.ColorTable = (eButtonColor)6;
		((Control)btnBack).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnBack.Image = (Image)(object)Resources.next2;
		btnBack.ImagePosition = (eImagePosition)1;
		((Control)btnBack).Location = new Point(138, 3);
		((Control)btnBack).Margin = new Padding(2);
		((Control)btnBack).Name = "btnBack";
		((Control)btnBack).RightToLeft = (RightToLeft)1;
		((Control)btnBack).Size = new Size(104, 53);
		((PopupItemControl)btnBack).Style = (eDotNetBarStyle)9;
		((Control)btnBack).TabIndex = 78;
		((Control)btnBack).TabStop = false;
		((Control)btnBack).Text = "[روز قبل]";
		((Control)btnBack).Click += btnBack_Click;
		((Control)btnFilter).AccessibleRole = (AccessibleRole)43;
		btnFilter.ColorTable = (eButtonColor)6;
		((Control)btnFilter).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnFilter.Image = (Image)(object)Resources.filter_data1;
		btnFilter.ImagePosition = (eImagePosition)1;
		((Control)btnFilter).Location = new Point(353, 2);
		((Control)btnFilter).Margin = new Padding(2);
		((Control)btnFilter).Name = "btnFilter";
		((Control)btnFilter).RightToLeft = (RightToLeft)1;
		((Control)btnFilter).Size = new Size(129, 53);
		((PopupItemControl)btnFilter).Style = (eDotNetBarStyle)9;
		((Control)btnFilter).TabIndex = 77;
		((Control)btnFilter).TabStop = false;
		((Control)btnFilter).Text = "[محدودسازی]";
		((Control)btnFilter).Click += btnFilter_Click;
		((Control)btnNext).AccessibleRole = (AccessibleRole)43;
		btnNext.ColorTable = (eButtonColor)6;
		((Control)btnNext).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnNext.Image = (Image)(object)Resources.back2;
		btnNext.ImagePosition = (eImagePosition)1;
		((Control)btnNext).Location = new Point(245, 2);
		((Control)btnNext).Margin = new Padding(2);
		((Control)btnNext).Name = "btnNext";
		((Control)btnNext).RightToLeft = (RightToLeft)1;
		((Control)btnNext).Size = new Size(105, 53);
		((PopupItemControl)btnNext).Style = (eDotNetBarStyle)9;
		((Control)btnNext).TabIndex = 77;
		((Control)btnNext).TabStop = false;
		((Control)btnNext).Text = "[روز بعد]";
		((Control)btnNext).Click += btnNext_Click;
		((Control)labelX6).Anchor = (AnchorStyles)14;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).ForeColor = Color.Magenta;
		((Control)labelX6).Location = new Point(10, 696);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(427, 14);
		((Control)labelX6).TabIndex = 18;
		((Control)labelX6).Text = "[رنگ ارغوانی : ترددهایی که دستی ثبت شده اند]";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)Lbldate).Anchor = (AnchorStyles)9;
		((Control)Lbldate).ForeColor = Color.DarkRed;
		Lbldate.LinkBehavior = (LinkBehavior)3;
		Lbldate.LinkColor = Color.DarkRed;
		((Control)Lbldate).Location = new Point(930, 9);
		((Control)Lbldate).Name = "Lbldate";
		((Control)Lbldate).RightToLeft = (RightToLeft)1;
		((Control)Lbldate).Size = new Size(124, 13);
		((Control)Lbldate).TabIndex = 1;
		Lbldate.TabStop = true;
		((Control)Lbldate).Text = "-------";
		((Label)Lbldate).TextAlign = (ContentAlignment)16;
		Lbldate.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(905, 713);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(77, 14);
		((Control)labelX1).TabIndex = 15;
		((Control)labelX1).Text = "[کسرکار]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)Lblkasr).Anchor = (AnchorStyles)10;
		((BaseItemControl)Lblkasr).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lblkasr).ForeColor = Color.DarkRed;
		((Control)Lblkasr).Location = new Point(805, 713);
		((Control)Lblkasr).Name = "Lblkasr";
		((Control)Lblkasr).Size = new Size(94, 14);
		((Control)Lblkasr).TabIndex = 17;
		((Control)Lblkasr).Text = "------";
		((Control)PicPerson).Anchor = (AnchorStyles)9;
		PicPerson.BorderStyle = (BorderStyle)1;
		PicPerson.Image = (Image)componentResourceManager.GetObject("PicPerson.Image");
		((Control)PicPerson).Location = new Point(1151, 3);
		((Control)PicPerson).Name = "PicPerson";
		((Control)PicPerson).Size = new Size(57, 53);
		PicPerson.SizeMode = (PictureBoxSizeMode)1;
		PicPerson.TabIndex = 76;
		PicPerson.TabStop = false;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(3, 3);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(130, 53);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 8;
		((Control)BtnPrint).Text = "[چاپ] (Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)LblKasrsum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblKasrsum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKasrsum).Location = new Point(905, 696);
		((Control)LblKasrsum).Name = "LblKasrsum";
		((Control)LblKasrsum).Size = new Size(77, 14);
		((Control)LblKasrsum).TabIndex = 14;
		((Control)LblKasrsum).Text = "[اضافه کار]:";
		LblKasrsum.TextAlignment = (StringAlignment)2;
		((Control)LblhozorSum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblhozorSum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblhozorSum).Location = new Point(1124, 713);
		((Control)LblhozorSum).Name = "LblhozorSum";
		((Control)LblhozorSum).Size = new Size(87, 14);
		((Control)LblhozorSum).TabIndex = 11;
		((Control)LblhozorSum).Text = "کارکرد(0):";
		LblhozorSum.TextAlignment = (StringAlignment)2;
		((Control)LblRow).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRow).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRow).Location = new Point(1124, 696);
		((Control)LblRow).Name = "LblRow";
		((Control)LblRow).Size = new Size(87, 14);
		((Control)LblRow).TabIndex = 10;
		((Control)LblRow).Text = "تعداد ردیف:";
		LblRow.TextAlignment = (StringAlignment)2;
		((Control)LblEzafe).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblEzafe).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEzafe).ForeColor = Color.DarkRed;
		((Control)LblEzafe).Location = new Point(805, 696);
		((Control)LblEzafe).Name = "LblEzafe";
		((Control)LblEzafe).Size = new Size(94, 14);
		((Control)LblEzafe).TabIndex = 16;
		((Control)LblEzafe).Text = "------";
		((Control)LblHozor).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblHozor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHozor).ForeColor = Color.DarkRed;
		((Control)LblHozor).Location = new Point(1018, 713);
		((Control)LblHozor).Name = "LblHozor";
		((Control)LblHozor).Size = new Size(100, 14);
		((Control)LblHozor).TabIndex = 13;
		((Control)LblHozor).Text = "------";
		((Control)LblRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRows).ForeColor = Color.DarkRed;
		((Control)LblRows).Location = new Point(1018, 696);
		((Control)LblRows).Name = "LblRows";
		((Control)LblRows).Size = new Size(100, 14);
		((Control)LblRows).TabIndex = 12;
		((Control)LblRows).Text = "------";
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(1067, 5);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(78, 20);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = " تاریخ شمسی:";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1213, 730);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmRptTaradodFullAll";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[گزارش وضعیت کلیه پرسنل]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FrmRptTaradodFullAll_Load;
		((Control)this).KeyDown += new KeyEventHandler(RPTTaradod_Full_KeyDown);
		((Control)CMS).ResumeLayout(false);
		((ISupportInitialize)taradodFullDetViewBindingSource).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)PicPerson).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
