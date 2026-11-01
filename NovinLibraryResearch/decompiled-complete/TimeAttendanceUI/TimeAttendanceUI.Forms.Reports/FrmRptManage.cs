using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Reports;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.View.Taradod;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.UC;

namespace TimeAttendanceUI.Forms.Reports;

public class FrmRptManage : BaseForm
{
	private bool _isDisposed;

	private Font _font;

	private CancellationTokenSource _loadDataToken = new CancellationTokenSource();

	private CancellationTokenSource TokenLoad = new CancellationTokenSource();

	private CancellationTokenSource _tokenBindFish = new CancellationTokenSource();

	private DateTime _dateM1;

	private DateTime _dateM2;

	private List<int> _personIDs;

	private IContainer components;

	private PanelEx panelEx1;

	internal DataGridViewX DGrid;

	private ButtonX BtnPrint;

	private BindingSource taradodFullBindingSource;

	private ProgressBar progressBar1;

	private LabelX Lbl_CountFish;

	private LabelX labelX3;

	private LabelX labelX1;

	private FlowLayoutPanel fLayoutFish;

	private UcLable ucLable_DateSh;

	private UcLable ucLable_IdSanad;

	private UcLable ucLable_FishNo;

	private UcLable ucLable_FishMonthYear;

	private LabelX Lbl_name;

	private DataGridViewTextBoxColumn CodeTafsilDG;

	private DataGridViewTextBoxColumn personIDDG;

	private DataGridViewTextBoxColumn FullNameDg;

	private DataGridViewTextBoxColumn CountAllDG;

	private DataGridViewTextBoxColumn CountshiftDG;

	private DataGridViewTextBoxColumn CountKarkerdDG;

	private DataGridViewTextBoxColumn DayGetSalaryDG;

	private DataGridViewTextBoxColumn countGheybatDG;

	private DataGridViewTextBoxColumn countMorakhasiWithSalaryDG;

	private DataGridViewTextBoxColumn countMorakhasiNotSalaryDG;

	private DataGridViewTextBoxColumn SumVacationClockly;

	private DataGridViewTextBoxColumn countNaghesDG;

	private DataGridViewTextBoxColumn SumWorkMissionTimeDG;

	private DataGridViewTextBoxColumn SumWorkMissionDG;

	private DataGridViewTextBoxColumn SumShiftTime;

	private DataGridViewTextBoxColumn SumMoazzafi;

	private DataGridViewTextBoxColumn sumKarkerdTimeDG;

	private DataGridViewTextBoxColumn sumEzafekarTimeDG;

	private DataGridViewTextBoxColumn SumEzafekarBeginTime;

	private DataGridViewTextBoxColumn SumEzafekarMidTime;

	private DataGridViewTextBoxColumn SumEzafekarEndTime;

	private DataGridViewTextBoxColumn sumKasrkarTimeDG;

	private DataGridViewTextBoxColumn SumKasrkarBegin;

	private DataGridViewTextBoxColumn SumKasrkarMid;

	private DataGridViewTextBoxColumn SumKasrkarEnd;

	private DataGridViewTextBoxColumn JarimeRoz;

	private DataGridViewTextBoxColumn CountDelayBeginShift;

	private DataGridViewTextBoxColumn SumWorkHalioDayWorkTime;

	private DataGridViewTextBoxColumn SumWorkTatilWorkTime;

	private DataGridViewTextBoxColumn SumWorkNightWorkTime;

	private DataGridViewTextBoxColumn SumNobatKari1Time;

	private DataGridViewTextBoxColumn SumNobatKari2Time;

	private DataGridViewTextBoxColumn SumNobatKari3Time;

	public SortableBindingList<TaradodFull> _listTaradodFulls { get; set; }

	public int PersonId
	{
		get
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			return Utilities.ParseToInt(((DataGridView)DGrid)[((DataGridViewBand)personIDDG).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString(), 0);
		}
	}

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

	public TaradodFull SelectedTaraddod { get; private set; }

	public FrmRptManage(string contextGuid, Font font, List<int> personIDs, DateTime date1, DateTime date2)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)DGrid).Font = font;
			_dateM1 = date1;
			_dateM2 = date2;
			_personIDs = personIDs;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task UpdateProgressAsync(List<TaskCompletionSource<bool>> tasks, CancellationToken token)
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
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)progressBar1).Visible = true;
						progressBar1.Maximum = tasks.Count;
					}
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
			while (tasks.Any())
			{
				Task<bool> t2 = await Task.WhenAny(tasks.Select((TaskCompletionSource<bool> p) => p.Task));
				tasks.Remove(tasks.Single((TaskCompletionSource<bool> t1) => t1.Task == t2));
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!IsDisposed && !token.IsCancellationRequested)
						{
							ProgressBar obj = progressBar1;
							obj.Value += 1;
						}
					}
					catch (Exception ex2)
					{
						Log.Error(ex2, "");
					}
				});
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			int maxParalelTasks = 20;
			List<Task<TaradodFull>> listTaskTaradodFulls = new List<Task<TaradodFull>>();
			List<TaradodFull> listTaradodFulls = new List<TaradodFull>();
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			List<TimeAttendanceDAL.Dal.FLRPerson.Person> list = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int>(), personStatus: true, token);
			if (_personIDs == null || !_personIDs.Any())
			{
				_personIDs = list?.Select((TimeAttendanceDAL.Dal.FLRPerson.Person x) => ((PersonBase)x).Id).ToList();
			}
			list = list.Where((TimeAttendanceDAL.Dal.FLRPerson.Person p) => _personIDs.Contains(((PersonBase)p).Id)).ToList();
			List<Tuple<int, TaskCompletionSource<bool>>> progressTasks = list.Select((TimeAttendanceDAL.Dal.FLRPerson.Person p) => new Tuple<int, TaskCompletionSource<bool>>(((PersonBase)p).Id, new TaskCompletionSource<bool>())).ToList();
			Task.Run(() => UpdateProgressAsync(progressTasks.Select((Tuple<int, TaskCompletionSource<bool>> p) => p.Item2).ToList(), token));
			foreach (TimeAttendanceDAL.Dal.FLRPerson.Person person in list)
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				TaskCompletionSource<bool> item = progressTasks.Single((Tuple<int, TaskCompletionSource<bool>> p) => p.Item1 == ((PersonBase)person).Id).Item2;
				listTaskTaradodFulls.Add(TaradodFull.CreateAsync(((CacheBaseForm)this).ContextGuid, _dateM1, _dateM2, person, item, checkValidationVorodAndKHoroj: false, token));
				if (listTaskTaradodFulls.Count() > maxParalelTasks)
				{
					Task<TaradodFull> t = await Task.WhenAny(listTaskTaradodFulls);
					TaradodFull taradodFull = await t;
					await taradodFull.FillTaradodFullPropertysAsync(token);
					listTaradodFulls.Add(taradodFull);
					listTaskTaradodFulls.Remove(t);
				}
			}
			TaradodFull[] remainTask = await Task.WhenAll(listTaskTaradodFulls);
			TaradodFull[] array = remainTask;
			for (int i = 0; i < array.Length; i++)
			{
				await array[i].FillTaradodFullPropertysAsync(token);
			}
			listTaradodFulls.AddRange(remainTask);
			_listTaradodFulls = Utilities.ToSortableBindingList<TaradodFull>((IEnumerable<TaradodFull>)listTaradodFulls.OrderBy((TaradodFull p) => ((PersonBase)p.ClsPerson).Family).ThenBy((TaradodFull n) => ((PersonBase)n.ClsPerson).Name), token);
			while (!((Control)this).IsHandleCreated && !token.IsCancellationRequested && !IsDisposed)
			{
				await Task.Delay(100);
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						taradodFullBindingSource.DataSource = _listTaradodFulls;
						((Control)progressBar1).Visible = false;
					}
				}
				catch (Exception ex3)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex3, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 148);
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 152);
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportAmarPersonnel)
			{
				((Form)this).Close();
				return;
			}
			_003C_003En__0(e);
			((Control)DGrid).Focus();
			((DataGridView)DGrid).AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)1;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			Task task = Task.Run(() => LoadDataAsync(token));
			new Waiter("در حال پردازش ...", (Form)(object)this, task, false, (EnWaiterType)1, (Control)null, "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 175);
		}
	}

	private async void RPTManage_KeyDown(object sender, KeyEventArgs e)
	{
		_ = 1;
		try
		{
			Keys keyCode = e.KeyCode;
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
					FrmRptTaradodFull frmDaily = new FrmRptTaradodFull(((CacheBaseForm)this).ContextGuid, _font, _dateM1, _dateM2, PersonId);
					await ((CacheBaseForm)frmDaily).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frmDaily).Dispose();
				}
			}
			else if ((int)keyCode != 80)
			{
				if ((int)keyCode == 123)
				{
					e.Handled = true;
					FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
				}
			}
			else if (e.Control)
			{
				((PopupItemControl)BtnPrint).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RPTManage_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 212);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (((Control)BtnPrint).Enabled && ((Control)BtnPrint).Visible)
			{
				((Control)DGrid).Focus();
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				List<object> j = new List<object>();
				List<object> l2 = new List<object>();
				for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
				{
					List<object> list = j;
					list.Add(await FillClsForRptManageAsync(i, token));
					list = l2;
					list.Add(await FillList2ForAsaAsync(i, token));
				}
				FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)8, j, null, 0, "", l2);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 237);
		}
	}

	private async Task<ClsRptAsa> FillList2ForAsaAsync(int idx, CancellationToken token)
	{
		ClsRptAsa xcls = new ClsRptAsa();
		try
		{
			if (DGrid != null)
			{
				xcls.CodePersoneli = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CodeTafsil.ToString("D8");
				string sumEzafekarTime = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumEzafekarTime;
				xcls.EzafeKari = (string.IsNullOrEmpty(sumEzafekarTime) ? "000:00" : sumEzafekarTime.PadLeft(6, '0'));
				string text = Time.IntToTime(((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumFriDayWork + ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumTatilDayWork);
				xcls.TatilKari = (string.IsNullOrEmpty(text) ? "000:00" : text.PadLeft(6, '0'));
				ClsRptAsa clsRptAsa = xcls;
				clsRptAsa.ShabKari = (((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumWorkNightWork / (await ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].GetClsGharardadAsync(token))?.LeaveMinutes).GetValueOrDefault().ToString("D2");
				xcls.MorakhasiBiHoghogh = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountMorakhasiNotSalary.ToString("D2");
				xcls.GheybatRuzane = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountGheybat.ToString("D2");
				string text2 = Time.IntToTime(((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].GheybatMin);
				xcls.GheybatSaati = (string.IsNullOrEmpty(text2) ? "000:00" : text2.PadLeft(6, '0'));
				string text3 = Time.IntToTime(((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumWorkVacation);
				xcls.MorakhasiBaHoghogh = (string.IsNullOrEmpty(text3) ? "000:00" : text3.PadLeft(6, '0'));
				xcls.Estelagi = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountMorakhasiEstelaji.ToString("D2");
				xcls.MamuriatRuzane = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].MissionDaily.ToString("D2");
				xcls.KarkardRuzane = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountKarkerd.ToString("D2");
				string sumKasrkarBeginTime = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumKasrkarBeginTime;
				xcls.TakhirVorood = (string.IsNullOrEmpty(sumKasrkarBeginTime) ? "000:00" : sumKasrkarBeginTime.PadLeft(6, '0'));
				string sumKasrkarEndTime = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumKasrkarEndTime;
				xcls.TajilKhorooj = (string.IsNullOrEmpty(sumKasrkarEndTime) ? "000:00" : sumKasrkarEndTime.PadLeft(6, '0'));
				xcls.NobatKariKol = (((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari1 + ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari2 + ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari3).ToString("D6");
				xcls.NobatKari1 = "000:00";
				xcls.NobatKari2 = "000:00";
				xcls.NobatKari3 = "000:00";
				string text4 = Time.IntToTime((((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountMorakhasiNotSalary * (await ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].GetClsGharardadAsync(token))?.LeaveMinutes).GetValueOrDefault());
				xcls.MorakhasiBiHoghoghSaati = (string.IsNullOrEmpty(text4) ? "000:00" : text4.PadLeft(6, '0'));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillList2ForAsaAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 286);
		}
		return xcls;
	}

	private async Task<ClsForRptManage> FillClsForRptManageAsync(int idx, CancellationToken token)
	{
		ClsForRptManage xcls = new ClsForRptManage();
		try
		{
			if (DGrid != null)
			{
				xcls.SumKarkardMin = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumKarkerd;
				ClsForRptManage clsForRptManage = xcls;
				clsForRptManage.GharardadLeaveMinutes = (await ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].GetClsGharardadAsync(token))?.LeaveMinutes ?? 0;
				xcls.SumFriDayWork = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumFriDayWork;
				xcls.SumTatilDayWork = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumTatilDayWork;
				xcls.SumWorkNightWork = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumWorkNightWork;
				xcls.SumNobatKari1 = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari1;
				xcls.SumNobatKari2 = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari2;
				xcls.SumNobatKari3 = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].SumNobatKari3;
				xcls.CountMorakhasiEstelaji = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountMorakhasiEstelaji;
				xcls.CountMorakhasiEstehghaghi = ((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].CountMorakhasiEstehghaghi;
				xcls.SumClocklyVacation = ((DataGridView)DGrid)[((DataGridViewBand)SumVacationClockly).Index, idx].Value.ToString();
				xcls.CountJarimeDays = short.Parse(((DataGridView)DGrid)[((DataGridViewBand)JarimeRoz).Index, idx].Value.ToString());
				xcls.CountDelayBeginShift = short.Parse(((DataGridView)DGrid)[((DataGridViewBand)CountDelayBeginShift).Index, idx].Value.ToString());
				xcls.StartDate = Calendar.MiladiToCalture(_dateM1);
				xcls.EndDate = Calendar.MiladiToCalture(_dateM2);
				xcls.DateReport = Calendar.MiladiToCalture(DateTime.Now);
				if (long.TryParse(((DataGridView)DGrid)[((DataGridViewBand)CodeTafsilDG).Index, idx].Value?.ToString(), out var result))
				{
					xcls.CodeTafsil = result;
				}
				else
				{
					xcls.CodeTafsil = 0L;
				}
				xcls.FullName = ((PersonBase)((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].ClsPerson).Family + " - " + ((PersonBase)((Collection<TaradodFull>)(object)_listTaradodFulls)[idx].ClsPerson).Name;
				xcls.CountDays = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)CountAllDG).Index, idx].Value.ToString());
				xcls.CountShifts = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)CountshiftDG).Index, idx].Value.ToString());
				xcls.CountHozor = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)CountKarkerdDG).Index, idx].Value.ToString());
				xcls.CountGheybat = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)countGheybatDG).Index, idx].Value.ToString());
				xcls.DayGetSalary = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)DayGetSalaryDG).Index, idx].Value.ToString());
				xcls.CountVacationWithSalary = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)countMorakhasiWithSalaryDG).Index, idx].Value.ToString());
				xcls.CountVacationWithoutSalary = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)countMorakhasiNotSalaryDG).Index, idx].Value.ToString());
				xcls.CountMission = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)SumWorkMissionDG).Index, idx].Value.ToString());
				xcls.SumWorkMissionTime = ((DataGridView)DGrid)[((DataGridViewBand)SumWorkMissionTimeDG).Index, idx].Value.ToString();
				xcls.TaradodNaghes = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)countNaghesDG).Index, idx].Value.ToString());
				xcls.KarkerdTime = ((DataGridView)DGrid)[((DataGridViewBand)sumKarkerdTimeDG).Index, idx].Value.ToString();
				xcls.OverTime = ((DataGridView)DGrid)[((DataGridViewBand)sumEzafekarTimeDG).Index, idx].Value.ToString();
				xcls.LeaveTime = ((DataGridView)DGrid)[((DataGridViewBand)sumKasrkarTimeDG).Index, idx].Value.ToString();
				xcls.SumKasrkarBegin = ((DataGridView)DGrid)[((DataGridViewBand)SumKasrkarBegin).Index, idx].Value.ToString();
				xcls.SumKasrkarMid = ((DataGridView)DGrid)[((DataGridViewBand)SumKasrkarMid).Index, idx].Value.ToString();
				xcls.SumKasrkarEnd = ((DataGridView)DGrid)[((DataGridViewBand)SumKasrkarEnd).Index, idx].Value.ToString();
				xcls.SumEzafekarBegin = ((DataGridView)DGrid)[((DataGridViewBand)SumEzafekarBeginTime).Index, idx].Value.ToString();
				xcls.SumEzafekarMid = ((DataGridView)DGrid)[((DataGridViewBand)SumEzafekarMidTime).Index, idx].Value.ToString();
				xcls.SumEzafekarEnd = ((DataGridView)DGrid)[((DataGridViewBand)SumEzafekarEndTime).Index, idx].Value.ToString();
				xcls.KarkardMozafiTime = ((DataGridView)DGrid)[((DataGridViewBand)SumMoazzafi).Index, idx].Value.ToString();
				xcls.MozafiMonthTime = ((DataGridView)DGrid)[((DataGridViewBand)SumShiftTime).Index, idx].Value.ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillClsForRptManageAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 346);
		}
		return xcls;
	}

	private async Task BindFishAsync(TaradodFull taradod, CancellationToken token)
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
			IOrderedEnumerable<Fish> x = (await Fish.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int> { taradod.PersonId }, _dateM1, _dateM2, new List<short>(), loadClsTaradod: false, token))?.OrderByDescending((Fish p) => p.FishNo);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					((Control)Lbl_CountFish).Text = x?.Count().ToString();
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						Fish fish = x?.FirstOrDefault();
						if (fish != null)
						{
							((Control)Lbl_name).Text = ((PersonBase)fish.ClsPerson).FullName ?? "";
							ucLable_DateSh.Value = Calendar.MiladiToCalture(fish.DateM) ?? "";
							ucLable_FishNo.Value = fish.FishNo.ToString() ?? "";
							ucLable_FishMonthYear.Value = fish.FishMonthYear ?? "";
							ucLable_IdSanad.Value = fish.IdSanad.ToString() ?? "";
						}
						else
						{
							((Control)Lbl_name).Text = "";
							ucLable_DateSh.Value = "";
							ucLable_FishNo.Value = "";
							ucLable_FishMonthYear.Value = "";
							ucLable_IdSanad.Value = "";
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
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BindFishAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Reports\\FrmRptManage.cs", 393);
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
				_tokenBindFish?.Cancel();
				_tokenBindFish?.Dispose();
				_tokenBindFish = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void taradodFullBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			TaradodFull taradodFull = (TaradodFull)taradodFullBindingSource.Current;
			if (taradodFull != SelectedTaraddod)
			{
				SelectedTaraddod = taradodFull;
				_tokenBindFish?.Cancel();
				_tokenBindFish = new CancellationTokenSource();
				CancellationToken token = _tokenBindFish.Token;
				BindFishAsync(SelectedTaraddod, token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Expected O, but got Unknown
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Expected O, but got Unknown
		//IL_0c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c39: Expected O, but got Unknown
		//IL_1ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aba: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		Lbl_name = new LabelX();
		fLayoutFish = new FlowLayoutPanel();
		ucLable_FishNo = new UcLable();
		ucLable_DateSh = new UcLable();
		ucLable_IdSanad = new UcLable();
		ucLable_FishMonthYear = new UcLable();
		labelX1 = new LabelX();
		Lbl_CountFish = new LabelX();
		labelX3 = new LabelX();
		progressBar1 = new ProgressBar();
		DGrid = new DataGridViewX();
		taradodFullBindingSource = new BindingSource(components);
		BtnPrint = new ButtonX();
		CodeTafsilDG = new DataGridViewTextBoxColumn();
		personIDDG = new DataGridViewTextBoxColumn();
		FullNameDg = new DataGridViewTextBoxColumn();
		CountAllDG = new DataGridViewTextBoxColumn();
		CountshiftDG = new DataGridViewTextBoxColumn();
		CountKarkerdDG = new DataGridViewTextBoxColumn();
		DayGetSalaryDG = new DataGridViewTextBoxColumn();
		countGheybatDG = new DataGridViewTextBoxColumn();
		countMorakhasiWithSalaryDG = new DataGridViewTextBoxColumn();
		countMorakhasiNotSalaryDG = new DataGridViewTextBoxColumn();
		SumVacationClockly = new DataGridViewTextBoxColumn();
		countNaghesDG = new DataGridViewTextBoxColumn();
		SumWorkMissionTimeDG = new DataGridViewTextBoxColumn();
		SumWorkMissionDG = new DataGridViewTextBoxColumn();
		SumShiftTime = new DataGridViewTextBoxColumn();
		SumMoazzafi = new DataGridViewTextBoxColumn();
		sumKarkerdTimeDG = new DataGridViewTextBoxColumn();
		sumEzafekarTimeDG = new DataGridViewTextBoxColumn();
		SumEzafekarBeginTime = new DataGridViewTextBoxColumn();
		SumEzafekarMidTime = new DataGridViewTextBoxColumn();
		SumEzafekarEndTime = new DataGridViewTextBoxColumn();
		sumKasrkarTimeDG = new DataGridViewTextBoxColumn();
		SumKasrkarBegin = new DataGridViewTextBoxColumn();
		SumKasrkarMid = new DataGridViewTextBoxColumn();
		SumKasrkarEnd = new DataGridViewTextBoxColumn();
		JarimeRoz = new DataGridViewTextBoxColumn();
		CountDelayBeginShift = new DataGridViewTextBoxColumn();
		SumWorkHalioDayWorkTime = new DataGridViewTextBoxColumn();
		SumWorkTatilWorkTime = new DataGridViewTextBoxColumn();
		SumWorkNightWorkTime = new DataGridViewTextBoxColumn();
		SumNobatKari1Time = new DataGridViewTextBoxColumn();
		SumNobatKari2Time = new DataGridViewTextBoxColumn();
		SumNobatKari3Time = new DataGridViewTextBoxColumn();
		((Control)panelEx1).SuspendLayout();
		((Control)fLayoutFish).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)taradodFullBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)Lbl_name);
		((Control)panelEx1).Controls.Add((Control)(object)fLayoutFish);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)Lbl_CountFish);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)progressBar1);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1456, 760);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)Lbl_name).Anchor = (AnchorStyles)10;
		((BaseItemControl)Lbl_name).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl_name).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Lbl_name).ForeColor = Color.DarkRed;
		((Control)Lbl_name).Location = new Point(1053, 700);
		((Control)Lbl_name).Name = "Lbl_name";
		((Control)Lbl_name).Size = new Size(231, 23);
		((Control)Lbl_name).TabIndex = 50;
		((Control)Lbl_name).Text = "---";
		((Control)fLayoutFish).Anchor = (AnchorStyles)14;
		((ScrollableControl)fLayoutFish).AutoScroll = true;
		((Control)fLayoutFish).Controls.Add((Control)(object)ucLable_FishNo);
		((Control)fLayoutFish).Controls.Add((Control)(object)ucLable_DateSh);
		((Control)fLayoutFish).Controls.Add((Control)(object)ucLable_IdSanad);
		((Control)fLayoutFish).Controls.Add((Control)(object)ucLable_FishMonthYear);
		((Control)fLayoutFish).Location = new Point(124, 726);
		((Control)fLayoutFish).Name = "fLayoutFish";
		((Control)fLayoutFish).Size = new Size(1324, 31);
		((Control)fLayoutFish).TabIndex = 49;
		((Control)ucLable_FishNo).Anchor = (AnchorStyles)10;
		ucLable_FishNo.Caption = "[شماره فیش]:";
		((Control)ucLable_FishNo).Location = new Point(3, 3);
		((Control)ucLable_FishNo).Name = "ucLable_FishNo";
		((Control)ucLable_FishNo).Size = new Size(171, 23);
		((Control)ucLable_FishNo).TabIndex = 2;
		ucLable_FishNo.Value = "---";
		((Control)ucLable_DateSh).Anchor = (AnchorStyles)10;
		ucLable_DateSh.Caption = "[تاریخ صدور]:";
		((Control)ucLable_DateSh).Location = new Point(180, 3);
		((Control)ucLable_DateSh).Name = "ucLable_DateSh";
		((Control)ucLable_DateSh).Size = new Size(181, 23);
		((Control)ucLable_DateSh).TabIndex = 0;
		ucLable_DateSh.Value = "---";
		((Control)ucLable_IdSanad).Anchor = (AnchorStyles)10;
		ucLable_IdSanad.Caption = "[شماره سند]:";
		((Control)ucLable_IdSanad).Location = new Point(367, 3);
		((Control)ucLable_IdSanad).Name = "ucLable_IdSanad";
		((Control)ucLable_IdSanad).Size = new Size(200, 23);
		((Control)ucLable_IdSanad).TabIndex = 1;
		ucLable_IdSanad.Value = "---";
		((Control)ucLable_FishMonthYear).Anchor = (AnchorStyles)10;
		ucLable_FishMonthYear.Caption = "[فیش مربوط به] :";
		((Control)ucLable_FishMonthYear).Location = new Point(573, 3);
		((Control)ucLable_FishMonthYear).Name = "ucLable_FishMonthYear";
		((Control)ucLable_FishMonthYear).Size = new Size(276, 23);
		((Control)ucLable_FishMonthYear).TabIndex = 4;
		ucLable_FishMonthYear.Value = "---";
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX1).Location = new Point(1290, 700);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(156, 23);
		((Control)labelX1).TabIndex = 48;
		((Control)labelX1).Text = "[آخرین فیش صادر شده آقا/خانم]: ";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)Lbl_CountFish).Anchor = (AnchorStyles)6;
		((BaseItemControl)Lbl_CountFish).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)Lbl_CountFish).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Lbl_CountFish).ForeColor = Color.DarkRed;
		((Control)Lbl_CountFish).Location = new Point(124, 700);
		((Control)Lbl_CountFish).Name = "Lbl_CountFish";
		((Control)Lbl_CountFish).Size = new Size(36, 23);
		((Control)Lbl_CountFish).TabIndex = 46;
		((Control)Lbl_CountFish).Text = "0";
		Lbl_CountFish.TextAlignment = (StringAlignment)1;
		((Control)labelX3).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX3).Location = new Point(167, 700);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(303, 23);
		((Control)labelX3).TabIndex = 45;
		((Control)labelX3).Text = "[تعداد کل فیشهای صادر شده برای این شخص در بازه مشخص شده]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)progressBar1).Anchor = (AnchorStyles)13;
		((Control)progressBar1).BackColor = SystemColors.GradientInactiveCaption;
		((Control)progressBar1).Location = new Point(10, 1);
		((Control)progressBar1).Name = "progressBar1";
		((Control)progressBar1).Size = new Size(1435, 10);
		progressBar1.Step = 1;
		progressBar1.Style = (ProgressBarStyle)1;
		((Control)progressBar1).TabIndex = 19;
		((Control)progressBar1).Visible = false;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		val2.Alignment = (DataGridViewContentAlignment)32;
		val2.BackColor = SystemColors.Control;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.WindowText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DGrid).ColumnHeadersDefaultCellStyle = val2;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[33]
		{
			(DataGridViewColumn)CodeTafsilDG,
			(DataGridViewColumn)personIDDG,
			(DataGridViewColumn)FullNameDg,
			(DataGridViewColumn)CountAllDG,
			(DataGridViewColumn)CountshiftDG,
			(DataGridViewColumn)CountKarkerdDG,
			(DataGridViewColumn)DayGetSalaryDG,
			(DataGridViewColumn)countGheybatDG,
			(DataGridViewColumn)countMorakhasiWithSalaryDG,
			(DataGridViewColumn)countMorakhasiNotSalaryDG,
			(DataGridViewColumn)SumVacationClockly,
			(DataGridViewColumn)countNaghesDG,
			(DataGridViewColumn)SumWorkMissionTimeDG,
			(DataGridViewColumn)SumWorkMissionDG,
			(DataGridViewColumn)SumShiftTime,
			(DataGridViewColumn)SumMoazzafi,
			(DataGridViewColumn)sumKarkerdTimeDG,
			(DataGridViewColumn)sumEzafekarTimeDG,
			(DataGridViewColumn)SumEzafekarBeginTime,
			(DataGridViewColumn)SumEzafekarMidTime,
			(DataGridViewColumn)SumEzafekarEndTime,
			(DataGridViewColumn)sumKasrkarTimeDG,
			(DataGridViewColumn)SumKasrkarBegin,
			(DataGridViewColumn)SumKasrkarMid,
			(DataGridViewColumn)SumKasrkarEnd,
			(DataGridViewColumn)JarimeRoz,
			(DataGridViewColumn)CountDelayBeginShift,
			(DataGridViewColumn)SumWorkHalioDayWorkTime,
			(DataGridViewColumn)SumWorkTatilWorkTime,
			(DataGridViewColumn)SumWorkNightWorkTime,
			(DataGridViewColumn)SumNobatKari1Time,
			(DataGridViewColumn)SumNobatKari2Time,
			(DataGridViewColumn)SumNobatKari3Time
		});
		((DataGridView)DGrid).DataSource = taradodFullBindingSource;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = SystemColors.Window;
		val3.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.ControlText;
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.ControlText;
		val3.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val3;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(10, 12);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(1435, 682);
		((Control)DGrid).TabIndex = 17;
		taradodFullBindingSource.DataSource = typeof(TaradodFull);
		taradodFullBindingSource.CurrentChanged += taradodFullBindingSource_CurrentChanged;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		((Control)BtnPrint).Anchor = (AnchorStyles)6;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(7, 700);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(112, 57);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 18;
		((Control)BtnPrint).Text = "[چاپ] (Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((DataGridViewColumn)CodeTafsilDG).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)CodeTafsilDG).HeaderText = "[کد تفصیل]";
		((DataGridViewColumn)CodeTafsilDG).Name = "CodeTafsilDG";
		((DataGridViewBand)CodeTafsilDG).ReadOnly = true;
		((DataGridViewBand)CodeTafsilDG).Visible = false;
		((DataGridViewColumn)personIDDG).DataPropertyName = "PersonId";
		((DataGridViewColumn)personIDDG).HeaderText = "personIDDG";
		((DataGridViewColumn)personIDDG).Name = "personIDDG";
		((DataGridViewBand)personIDDG).ReadOnly = true;
		((DataGridViewBand)personIDDG).Visible = false;
		((DataGridViewColumn)FullNameDg).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)FullNameDg).DataPropertyName = "FullName";
		((DataGridViewColumn)FullNameDg).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)FullNameDg).MinimumWidth = 100;
		((DataGridViewColumn)FullNameDg).Name = "FullNameDg";
		((DataGridViewBand)FullNameDg).ReadOnly = true;
		((DataGridViewColumn)FullNameDg).Width = 125;
		((DataGridViewColumn)CountAllDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)CountAllDG).DataPropertyName = "CountAll";
		((DataGridViewColumn)CountAllDG).HeaderText = "[روزها]";
		((DataGridViewColumn)CountAllDG).MinimumWidth = 45;
		((DataGridViewColumn)CountAllDG).Name = "CountAllDG";
		((DataGridViewBand)CountAllDG).ReadOnly = true;
		((DataGridViewColumn)CountshiftDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)CountshiftDG).DataPropertyName = "Countshift";
		((DataGridViewColumn)CountshiftDG).HeaderText = "[شیفت دار]";
		((DataGridViewColumn)CountshiftDG).MinimumWidth = 45;
		((DataGridViewColumn)CountshiftDG).Name = "CountshiftDG";
		((DataGridViewBand)CountshiftDG).ReadOnly = true;
		((DataGridViewColumn)CountshiftDG).ToolTipText = "[روزهای شیفت دار]";
		((DataGridViewColumn)CountKarkerdDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)CountKarkerdDG).DataPropertyName = "CountKarkerd";
		((DataGridViewColumn)CountKarkerdDG).HeaderText = "[حضور]";
		((DataGridViewColumn)CountKarkerdDG).MinimumWidth = 45;
		((DataGridViewColumn)CountKarkerdDG).Name = "CountKarkerdDG";
		((DataGridViewBand)CountKarkerdDG).ReadOnly = true;
		((DataGridViewColumn)CountKarkerdDG).ToolTipText = "[روزهای حضور]";
		((DataGridViewColumn)DayGetSalaryDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DayGetSalaryDG).DataPropertyName = "CountWorkdayHoghogh";
		((DataGridViewColumn)DayGetSalaryDG).HeaderText = "[روزهای حقوق]";
		((DataGridViewColumn)DayGetSalaryDG).MinimumWidth = 45;
		((DataGridViewColumn)DayGetSalaryDG).Name = "DayGetSalaryDG";
		((DataGridViewBand)DayGetSalaryDG).ReadOnly = true;
		((DataGridViewColumn)DayGetSalaryDG).ToolTipText = "[روزهای دریافت حقوق]";
		((DataGridViewColumn)countGheybatDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)countGheybatDG).DataPropertyName = "CountGheybat";
		((DataGridViewColumn)countGheybatDG).HeaderText = "[غیبت]";
		((DataGridViewColumn)countGheybatDG).MinimumWidth = 45;
		((DataGridViewColumn)countGheybatDG).Name = "countGheybatDG";
		((DataGridViewBand)countGheybatDG).ReadOnly = true;
		((DataGridViewColumn)countMorakhasiWithSalaryDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)countMorakhasiWithSalaryDG).DataPropertyName = "CountMorakhasiWithSalary";
		((DataGridViewColumn)countMorakhasiWithSalaryDG).HeaderText = "[م با حقوق]";
		((DataGridViewColumn)countMorakhasiWithSalaryDG).MinimumWidth = 45;
		((DataGridViewColumn)countMorakhasiWithSalaryDG).Name = "countMorakhasiWithSalaryDG";
		((DataGridViewBand)countMorakhasiWithSalaryDG).ReadOnly = true;
		((DataGridViewColumn)countMorakhasiWithSalaryDG).ToolTipText = "[مرخصی هایی که مشمول حقوق میشوند] ";
		((DataGridViewColumn)countMorakhasiNotSalaryDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)countMorakhasiNotSalaryDG).DataPropertyName = "CountMorakhasiNotSalary";
		((DataGridViewColumn)countMorakhasiNotSalaryDG).HeaderText = "[م بی حقوق]";
		((DataGridViewColumn)countMorakhasiNotSalaryDG).MinimumWidth = 45;
		((DataGridViewColumn)countMorakhasiNotSalaryDG).Name = "countMorakhasiNotSalaryDG";
		((DataGridViewBand)countMorakhasiNotSalaryDG).ReadOnly = true;
		((DataGridViewColumn)countMorakhasiNotSalaryDG).ToolTipText = "[مرخصی هایی که مشمول حقوق نمیشوند]";
		((DataGridViewColumn)SumVacationClockly).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)SumVacationClockly).DataPropertyName = "SumWorkVacationTime";
		((DataGridViewColumn)SumVacationClockly).HeaderText = "[مرخصی ساعتی]";
		((DataGridViewColumn)SumVacationClockly).MinimumWidth = 35;
		((DataGridViewColumn)SumVacationClockly).Name = "SumVacationClockly";
		((DataGridViewBand)SumVacationClockly).ReadOnly = true;
		((DataGridViewColumn)countNaghesDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)countNaghesDG).DataPropertyName = "CountNaghes";
		((DataGridViewColumn)countNaghesDG).HeaderText = "[تردد ناقص]";
		((DataGridViewColumn)countNaghesDG).MinimumWidth = 45;
		((DataGridViewColumn)countNaghesDG).Name = "countNaghesDG";
		((DataGridViewBand)countNaghesDG).ReadOnly = true;
		((DataGridViewColumn)SumWorkMissionTimeDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)SumWorkMissionTimeDG).DataPropertyName = "SumWorkMissionTime";
		((DataGridViewColumn)SumWorkMissionTimeDG).HeaderText = "[ماموریت (دقیقه)]";
		((DataGridViewColumn)SumWorkMissionTimeDG).MinimumWidth = 45;
		((DataGridViewColumn)SumWorkMissionTimeDG).Name = "SumWorkMissionTimeDG";
		((DataGridViewBand)SumWorkMissionTimeDG).ReadOnly = true;
		((DataGridViewColumn)SumWorkMissionDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)SumWorkMissionDG).DataPropertyName = "MissionDaily";
		((DataGridViewColumn)SumWorkMissionDG).HeaderText = "[ماموریت روزانه]";
		((DataGridViewColumn)SumWorkMissionDG).MinimumWidth = 45;
		((DataGridViewColumn)SumWorkMissionDG).Name = "SumWorkMissionDG";
		((DataGridViewBand)SumWorkMissionDG).ReadOnly = true;
		((DataGridViewColumn)SumWorkMissionDG).ToolTipText = "[ماموریت روزانه]";
		((DataGridViewColumn)SumShiftTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumShiftTime).DataPropertyName = "SumShiftTime";
		((DataGridViewColumn)SumShiftTime).HeaderText = "[موظفی ماهانه]";
		((DataGridViewColumn)SumShiftTime).MinimumWidth = 60;
		((DataGridViewColumn)SumShiftTime).Name = "SumShiftTime";
		((DataGridViewBand)SumShiftTime).ReadOnly = true;
		((DataGridViewColumn)SumShiftTime).Width = 60;
		((DataGridViewColumn)SumMoazzafi).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumMoazzafi).DataPropertyName = "SumMoazzafiTime";
		((DataGridViewColumn)SumMoazzafi).HeaderText = "[کارکرد موظفی]";
		((DataGridViewColumn)SumMoazzafi).MinimumWidth = 60;
		((DataGridViewColumn)SumMoazzafi).Name = "SumMoazzafi";
		((DataGridViewBand)SumMoazzafi).ReadOnly = true;
		((DataGridViewColumn)SumMoazzafi).Width = 60;
		((DataGridViewColumn)sumKarkerdTimeDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)sumKarkerdTimeDG).DataPropertyName = "SumKarkerdTime";
		((DataGridViewColumn)sumKarkerdTimeDG).HeaderText = "[کارکرد کل]";
		((DataGridViewColumn)sumKarkerdTimeDG).MinimumWidth = 60;
		((DataGridViewColumn)sumKarkerdTimeDG).Name = "sumKarkerdTimeDG";
		((DataGridViewBand)sumKarkerdTimeDG).ReadOnly = true;
		((DataGridViewColumn)sumKarkerdTimeDG).Width = 60;
		((DataGridViewColumn)sumEzafekarTimeDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)sumEzafekarTimeDG).DataPropertyName = "SumEzafekarTime";
		((DataGridViewColumn)sumEzafekarTimeDG).HeaderText = "[اضافه کار]";
		((DataGridViewColumn)sumEzafekarTimeDG).MinimumWidth = 45;
		((DataGridViewColumn)sumEzafekarTimeDG).Name = "sumEzafekarTimeDG";
		((DataGridViewBand)sumEzafekarTimeDG).ReadOnly = true;
		((DataGridViewColumn)sumEzafekarTimeDG).Width = 45;
		((DataGridViewColumn)SumEzafekarBeginTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumEzafekarBeginTime).DataPropertyName = "SumEzafekarBeginTime";
		((DataGridViewColumn)SumEzafekarBeginTime).HeaderText = "[اضافه کار اول]";
		((DataGridViewColumn)SumEzafekarBeginTime).MinimumWidth = 45;
		((DataGridViewColumn)SumEzafekarBeginTime).Name = "SumEzafekarBeginTime";
		((DataGridViewBand)SumEzafekarBeginTime).ReadOnly = true;
		((DataGridViewColumn)SumEzafekarBeginTime).Width = 45;
		((DataGridViewColumn)SumEzafekarMidTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumEzafekarMidTime).DataPropertyName = "SumEzafekarMidTime";
		((DataGridViewColumn)SumEzafekarMidTime).HeaderText = "[اضافه کار وسط]";
		((DataGridViewColumn)SumEzafekarMidTime).MinimumWidth = 45;
		((DataGridViewColumn)SumEzafekarMidTime).Name = "SumEzafekarMidTime";
		((DataGridViewBand)SumEzafekarMidTime).ReadOnly = true;
		((DataGridViewColumn)SumEzafekarMidTime).Width = 45;
		((DataGridViewColumn)SumEzafekarEndTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumEzafekarEndTime).DataPropertyName = "SumEzafekarEndTime";
		((DataGridViewColumn)SumEzafekarEndTime).HeaderText = "[اضافه کار آخر]";
		((DataGridViewColumn)SumEzafekarEndTime).MinimumWidth = 45;
		((DataGridViewColumn)SumEzafekarEndTime).Name = "SumEzafekarEndTime";
		((DataGridViewBand)SumEzafekarEndTime).ReadOnly = true;
		((DataGridViewColumn)SumEzafekarEndTime).Width = 45;
		((DataGridViewColumn)sumKasrkarTimeDG).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)sumKasrkarTimeDG).DataPropertyName = "SumKasrkarTime";
		((DataGridViewColumn)sumKasrkarTimeDG).HeaderText = "[کسرکار]";
		((DataGridViewColumn)sumKasrkarTimeDG).MinimumWidth = 45;
		((DataGridViewColumn)sumKasrkarTimeDG).Name = "sumKasrkarTimeDG";
		((DataGridViewBand)sumKasrkarTimeDG).ReadOnly = true;
		((DataGridViewColumn)sumKasrkarTimeDG).Width = 45;
		((DataGridViewColumn)SumKasrkarBegin).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumKasrkarBegin).DataPropertyName = "SumKasrkarBeginTime";
		((DataGridViewColumn)SumKasrkarBegin).HeaderText = "[کسر کار اول]";
		((DataGridViewColumn)SumKasrkarBegin).MinimumWidth = 45;
		((DataGridViewColumn)SumKasrkarBegin).Name = "SumKasrkarBegin";
		((DataGridViewBand)SumKasrkarBegin).ReadOnly = true;
		((DataGridViewColumn)SumKasrkarBegin).Width = 45;
		((DataGridViewColumn)SumKasrkarMid).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumKasrkarMid).DataPropertyName = "SumKasrkarMidTime";
		((DataGridViewColumn)SumKasrkarMid).HeaderText = "[کسر کار وسط]";
		((DataGridViewColumn)SumKasrkarMid).MinimumWidth = 45;
		((DataGridViewColumn)SumKasrkarMid).Name = "SumKasrkarMid";
		((DataGridViewBand)SumKasrkarMid).ReadOnly = true;
		((DataGridViewColumn)SumKasrkarMid).Width = 45;
		((DataGridViewColumn)SumKasrkarEnd).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumKasrkarEnd).DataPropertyName = "SumKasrkarEndTime";
		((DataGridViewColumn)SumKasrkarEnd).HeaderText = "[کسر کار آخر]";
		((DataGridViewColumn)SumKasrkarEnd).MinimumWidth = 45;
		((DataGridViewColumn)SumKasrkarEnd).Name = "SumKasrkarEnd";
		((DataGridViewBand)SumKasrkarEnd).ReadOnly = true;
		((DataGridViewColumn)SumKasrkarEnd).Width = 45;
		((DataGridViewColumn)JarimeRoz).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)JarimeRoz).DataPropertyName = "JarimeRoz";
		((DataGridViewColumn)JarimeRoz).HeaderText = "[جریمه (روز)]";
		((DataGridViewColumn)JarimeRoz).MinimumWidth = 35;
		((DataGridViewColumn)JarimeRoz).Name = "JarimeRoz";
		((DataGridViewBand)JarimeRoz).ReadOnly = true;
		((DataGridViewColumn)CountDelayBeginShift).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)CountDelayBeginShift).DataPropertyName = "CountDelayBeginShift";
		((DataGridViewColumn)CountDelayBeginShift).HeaderText = "[تاخیر (روز)]";
		((DataGridViewColumn)CountDelayBeginShift).MinimumWidth = 35;
		((DataGridViewColumn)CountDelayBeginShift).Name = "CountDelayBeginShift";
		((DataGridViewBand)CountDelayBeginShift).ReadOnly = true;
		((DataGridViewColumn)SumWorkHalioDayWorkTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumWorkHalioDayWorkTime).DataPropertyName = "SumWorkHalioDayWorkTime";
		((DataGridViewColumn)SumWorkHalioDayWorkTime).HeaderText = "جمعه کاری";
		((DataGridViewColumn)SumWorkHalioDayWorkTime).MinimumWidth = 45;
		((DataGridViewColumn)SumWorkHalioDayWorkTime).Name = "SumWorkHalioDayWorkTime";
		((DataGridViewBand)SumWorkHalioDayWorkTime).ReadOnly = true;
		((DataGridViewColumn)SumWorkHalioDayWorkTime).Width = 45;
		((DataGridViewColumn)SumWorkTatilWorkTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumWorkTatilWorkTime).DataPropertyName = "SumWorkTatilWorkTime";
		((DataGridViewColumn)SumWorkTatilWorkTime).HeaderText = "تعطیل کاری";
		((DataGridViewColumn)SumWorkTatilWorkTime).MinimumWidth = 45;
		((DataGridViewColumn)SumWorkTatilWorkTime).Name = "SumWorkTatilWorkTime";
		((DataGridViewBand)SumWorkTatilWorkTime).ReadOnly = true;
		((DataGridViewColumn)SumWorkTatilWorkTime).Width = 45;
		((DataGridViewColumn)SumWorkNightWorkTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumWorkNightWorkTime).DataPropertyName = "SumWorkNightWorkTime";
		((DataGridViewColumn)SumWorkNightWorkTime).HeaderText = "شبکاری";
		((DataGridViewColumn)SumWorkNightWorkTime).MinimumWidth = 45;
		((DataGridViewColumn)SumWorkNightWorkTime).Name = "SumWorkNightWorkTime";
		((DataGridViewBand)SumWorkNightWorkTime).ReadOnly = true;
		((DataGridViewColumn)SumWorkNightWorkTime).Width = 45;
		((DataGridViewColumn)SumNobatKari1Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumNobatKari1Time).DataPropertyName = "SumNobatKari1Time";
		((DataGridViewColumn)SumNobatKari1Time).HeaderText = "نوبت کاری1";
		((DataGridViewColumn)SumNobatKari1Time).MinimumWidth = 45;
		((DataGridViewColumn)SumNobatKari1Time).Name = "SumNobatKari1Time";
		((DataGridViewBand)SumNobatKari1Time).ReadOnly = true;
		((DataGridViewColumn)SumNobatKari1Time).Width = 45;
		((DataGridViewColumn)SumNobatKari2Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumNobatKari2Time).DataPropertyName = "SumNobatKari2Time";
		((DataGridViewColumn)SumNobatKari2Time).HeaderText = "نوبت کاری 2";
		((DataGridViewColumn)SumNobatKari2Time).MinimumWidth = 45;
		((DataGridViewColumn)SumNobatKari2Time).Name = "SumNobatKari2Time";
		((DataGridViewBand)SumNobatKari2Time).ReadOnly = true;
		((DataGridViewColumn)SumNobatKari2Time).Width = 45;
		((DataGridViewColumn)SumNobatKari3Time).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)SumNobatKari3Time).DataPropertyName = "SumNobatKari3Time";
		((DataGridViewColumn)SumNobatKari3Time).HeaderText = "نوبت کاری3";
		((DataGridViewColumn)SumNobatKari3Time).MinimumWidth = 45;
		((DataGridViewColumn)SumNobatKari3Time).Name = "SumNobatKari3Time";
		((DataGridViewBand)SumNobatKari3Time).ReadOnly = true;
		((DataGridViewColumn)SumNobatKari3Time).Width = 45;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1456, 760);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).MinimumSize = new Size(880, 397);
		((Control)this).Name = "FrmRptManage";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[گزارش تجمیع تردد]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(RPTManage_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)fLayoutFish).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)taradodFullBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((BaseForm)this).OnLoad(e);
	}
}
