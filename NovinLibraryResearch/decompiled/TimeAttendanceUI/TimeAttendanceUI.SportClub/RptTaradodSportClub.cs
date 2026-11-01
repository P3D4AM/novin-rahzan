using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
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
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.View.SportClub;
using TimeAttendanceDAL.View.Taradod;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.SportClub;

public class RptTaradodSportClub : BaseForm
{
	private TaradodSportClub _clsSportClub;

	private ColorDet _clsColor;

	private Font _font;

	private DateTime? _rowSelected;

	private DataGridViewTextBoxColumn[] _lstDgv;

	private DataGridViewTextBoxColumn[] _lstDgkh;

	private readonly DateTime _date1;

	private readonly DateTime _date2;

	private Person _clsPerson;

	private bool _isDisposed;

	private List<TaradodSportClub> _listData;

	private SortableBindingList<TaradodSportClub> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private IContainer components;

	private PanelEx panelEx1;

	private DataGridViewX DGrid;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX LblStartGharardad;

	private LabelX LblHozor;

	private LabelX LblRows;

	private LabelX LblRow;

	private LabelX LblKasrsum;

	private LabelX LblhozorSum;

	private BindingSource personBindingSource;

	private ButtonX BtnPrint;

	private LabelX labelX4;

	private PictureBox PicPerson;

	private LabelX labelX1;

	private LabelX LblEndGharardad;

	private LinkLabel LblName;

	private LabelX labelX5;

	private LinkLabel LblTaTar;

	private LinkLabel LblAzTar;

	private LinkLabel LblCodeTafsil;

	private ContextMenuStrip CMS;

	private ToolStripMenuItem CMS_GharadadList;

	private ToolStripMenuItem CMS_ShiftList;

	private ToolStripMenuItem CMS_PersonInfo;

	private ToolStripMenuItem CMS_DeleteAttendance;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator3;

	private LabelX labelX7;

	private LabelX LblGheybat;

	private LabelX labelX8;

	private LabelX LblPeriodic;

	private DataGridViewTextBoxColumn DGDate;

	private DataGridViewTextBoxColumn DgDay;

	private DataGridViewTextBoxColumn DGStatus;

	private DataGridViewTextBoxColumn DGHozor;

	private DataGridViewTextBoxColumn DGShift;

	private DataGridViewTextBoxColumn DGV1;

	private DataGridViewTextBoxColumn DGKH1;

	private DataGridViewTextBoxColumn DGV2;

	private DataGridViewTextBoxColumn DGKH2;

	private DataGridViewTextBoxColumn DGV3;

	private DataGridViewTextBoxColumn DGKH3;

	private DataGridViewTextBoxColumn DGV4;

	private DataGridViewTextBoxColumn DGKH4;

	private DataGridViewTextBoxColumn DGV5;

	private DataGridViewTextBoxColumn DGKH5;

	private DataGridViewTextBoxColumn DGV6;

	private DataGridViewTextBoxColumn DGKH6;

	private DataGridViewTextBoxColumn DGV7;

	private DataGridViewTextBoxColumn DGKH7;

	private DataGridViewTextBoxColumn DGV8;

	private DataGridViewTextBoxColumn DGKH8;

	private DataGridViewTextBoxColumn DGDateM;

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
			if (((DataGridView)DGrid).CurrentRow == null)
			{
				return DateTime.MinValue;
			}
			return _clsSportClub.ListSportClubDets[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].DateM;
		}
	}

	public RptTaradodSportClub(string contextGuid, DateTime date1, DateTime date2, int personId, Font font, DateTime? rowSelected = null)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		RptTaradodSportClub rptTaradodSportClub = this;
		try
		{
			InitializeComponent();
			if (rowSelected.HasValue)
			{
				_rowSelected = rowSelected;
			}
			_date1 = date1;
			_date2 = date2;
			_font = font;
			InitilizeGrid();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			Task task = new Task(async delegate
			{
				Person clsPerson = await Person.GetByPersonIdAsync(contextGuid, personId, token);
				rptTaradodSportClub._clsPerson = clsPerson;
			});
			new Waiter("بارگذاری اطلاعات شخص", (Form)(object)this, task, false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 74);
		}
	}

	public RptTaradodSportClub(string contextGuid, TaradodSportClub pTaradodFull)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			InitilizeGrid();
			_clsSportClub = pTaradodFull;
			_ = ((PersonBase)_clsSportClub.ClsPerson).Id;
			_ = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 88);
		}
	}

	private void InitilizeGrid()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		try
		{
			_clsColor = new ColorDet();
			_lstDgv[0] = DGV1;
			_lstDgv[1] = DGV2;
			_lstDgv[2] = DGV3;
			_lstDgv[3] = DGV4;
			_lstDgv[4] = DGV5;
			_lstDgv[5] = DGV6;
			_lstDgv[6] = DGV7;
			_lstDgv[7] = DGV8;
			_lstDgkh[0] = DGKH1;
			_lstDgkh[1] = DGKH2;
			_lstDgkh[2] = DGKH3;
			_lstDgkh[3] = DGKH4;
			_lstDgkh[4] = DGKH5;
			_lstDgkh[5] = DGKH6;
			_lstDgkh[6] = DGKH7;
			_lstDgkh[7] = DGKH8;
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
			while (!((Control)this).IsHandleCreated && !token.IsCancellationRequested)
			{
				await Task.Delay(100, token);
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			_clsSportClub = await TaradodSportClub.GetAsync(((CacheBaseForm)this).ContextGuid, _date1, _date2, _clsPerson, token);
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				DataGridViewTextBoxColumn dGV = DGV2;
				DataGridViewTextBoxColumn dGV2 = DGV3;
				DataGridViewTextBoxColumn dGV3 = DGV4;
				DataGridViewTextBoxColumn dGV4 = DGV5;
				DataGridViewTextBoxColumn dGV5 = DGV6;
				DataGridViewTextBoxColumn dGV6 = DGV7;
				DataGridViewTextBoxColumn dGV7 = DGV8;
				DataGridViewTextBoxColumn dGKH = DGKH2;
				DataGridViewTextBoxColumn dGKH2 = DGKH3;
				DataGridViewTextBoxColumn dGKH3 = DGKH4;
				DataGridViewTextBoxColumn dGKH4 = DGKH5;
				DataGridViewTextBoxColumn dGKH5 = DGKH6;
				DataGridViewTextBoxColumn dGKH6 = DGKH7;
				bool flag2 = (((DataGridViewBand)DGKH8).Visible = false);
				bool flag4 = (((DataGridViewBand)dGKH6).Visible = flag2);
				bool flag6 = (((DataGridViewBand)dGKH5).Visible = flag4);
				bool flag8 = (((DataGridViewBand)dGKH4).Visible = flag6);
				bool flag10 = (((DataGridViewBand)dGKH3).Visible = flag8);
				bool flag12 = (((DataGridViewBand)dGKH2).Visible = flag10);
				bool flag14 = (((DataGridViewBand)dGKH).Visible = flag12);
				bool flag16 = (((DataGridViewBand)dGV7).Visible = flag14);
				bool flag18 = (((DataGridViewBand)dGV6).Visible = flag16);
				bool flag20 = (((DataGridViewBand)dGV5).Visible = flag18);
				bool flag22 = (((DataGridViewBand)dGV4).Visible = flag20);
				bool flag24 = (((DataGridViewBand)dGV3).Visible = flag22);
				bool visible = (((DataGridViewBand)dGV2).Visible = flag24);
				((DataGridViewBand)dGV).Visible = visible;
				int i = 0;
				((DataGridView)DGrid).Rows.Clear();
				foreach (TaradodSportClubDet listSportClubDet in _clsSportClub.ListSportClubDets)
				{
					((DataGridView)DGrid).Rows.Add();
					await SetRowinfoAsync(i, listSportClubDet, token);
					i++;
				}
			});
			await RefreshLblAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 141);
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		try
		{
			_003C_003En__0(e);
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportTaradodPersonnel)
			{
				((Form)this).Close();
				return;
			}
			((ToolStripItem)CMS_DeleteAttendance).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodDelete;
			((ToolStripItem)CMS_GharadadList).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain;
			((ToolStripItem)CMS_ShiftList).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftMain;
			if (!_rowSelected.HasValue)
			{
				_rowSelected = _date1;
			}
			string text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(_rowSelected.Value);
			((DataGridView)DGrid).CurrentCell = ((DataGridView)DGrid).Rows[LanguageAndCalendar.Instance.CaltureCalender.GetDayOfDate(text) - 1].Cells[0];
			((Control)DGrid).Focus();
			_ = "تهیه گزارش کامل تردد  " + ((PersonBase)_clsSportClub.ClsPerson).FullName + " از تاریخ : " + _clsSportClub.DateStartSh + " تا تاریخ : " + _clsSportClub.DateEndSh;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 168);
		}
	}

	private async void DGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
	{
		_ = 1;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			int rowIndex = e.RowIndex;
			await SetRowinfoAsync(rowIndex, await GetRowInfoAsync(e.RowIndex, e.ColumnIndex, token), token);
			Application.DoEvents();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellEndEdit", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 184);
		}
	}

	private async Task<TaradodSportClubDet> GetRowInfoAsync(int rowindex, int columnIndex, CancellationToken token)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"line 217 in {DateTime.Now} method GetRowInfo");
		try
		{
			int year = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)DGDateM).Index, rowindex].Value.ToString().Substring(0, 4));
			int month = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)DGDateM).Index, rowindex].Value.ToString().Substring(5, 2));
			int day = Convert.ToInt32(((DataGridView)DGrid)[((DataGridViewBand)DGDateM).Index, rowindex].Value.ToString().Substring(8, 2));
			DateTime xdate = new DateTime(year, month, day, 23, 59, 59);
			TaradodSportClubDet clsDet = await TaradodSportClubDet.GetAsync(((CacheBaseForm)this).ContextGuid, xdate, _clsSportClub.ClsPerson, getTarardodOnly: false, token);
			string xdesc = "اصلاح اطلاعات تردد  " + ((PersonBase)_clsSportClub.ClsPerson).FullName + " در روز " + clsDet.DateSh + "\n";
			string xdescbefor = "";
			string xdescAfter = "";
			int idx = (columnIndex - ((DataGridViewBand)_lstDgv[0]).Index) / 2;
			if (columnIndex % 2 == 0)
			{
				if (clsDet.CoupleTaradod(idx).Vtaradod.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
				{
					xdescbefor = xdescbefor + clsDet.CoupleTaradod(idx).VTime + "-";
				}
				await clsDet.CoupleTaradod(idx).SetVorodAsync(sb, GetTimeFormat(rowindex, ((DataGridViewBand)_lstDgv[idx]).Index), token);
				if (clsDet.CoupleTaradod(idx).Vtaradod.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
				{
					xdescAfter = xdescAfter + clsDet.CoupleTaradod(idx).VTime + "-";
				}
			}
			else
			{
				if (clsDet.CoupleTaradod(idx).Ktaradod.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
				{
					xdescbefor = xdescbefor + clsDet.CoupleTaradod(idx).KTime + "-";
				}
				await clsDet.CoupleTaradod(idx).SetKhorojAsync(sb, GetTimeFormat(rowindex, ((DataGridViewBand)_lstDgkh[idx]).Index), token);
				if (clsDet.CoupleTaradod(idx).Ktaradod.DateTimeLogM.ToString("HH:mm:ss") != "00:00:00")
				{
					xdescAfter = xdescAfter + clsDet.CoupleTaradod(idx).KTime + "-";
				}
			}
			char[] trimChars = new char[1] { '-' };
			if (xdescbefor != "")
			{
				xdesc = xdesc + "\n اطلاعات تردد قبل از ویرایش : " + xdescbefor.TrimEnd(trimChars);
			}
			if (xdescAfter != "")
			{
				xdesc = xdesc + "\n اطلاعات تردد بعد از ویرایش  : " + xdescAfter.TrimEnd(trimChars);
			}
			await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, xdesc, "Taradod.TaradodSaveNewTaradod");
			return await TaradodSportClubDet.GetAsync(((CacheBaseForm)this).ContextGuid, xdate, _clsSportClub.ClsPerson, getTarardodOnly: false, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetRowInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 226);
		}
		return null;
	}

	private async Task SetRowinfoAsync(int rowindex, TaradodSportClubDet cls, CancellationToken token)
	{
		_ = 7;
		try
		{
			_ = cls.DayofShMonth;
			for (int i = 1; i < 8; i++)
			{
				if (!((DataGridViewBand)_lstDgkh[i]).Visible && (cls.CoupleTaradod(i - 1).Vtaradod.Id != 0 || cls.CoupleTaradod(i - 1).Ktaradod.Id != 0))
				{
					DataGridViewTextBoxColumn obj = _lstDgkh[i];
					bool visible = (((DataGridViewBand)_lstDgv[i]).Visible = true);
					((DataGridViewBand)obj).Visible = visible;
				}
			}
			if (await cls.GetSumShiftMinutsAsync(token) != 0)
			{
				((DataGridViewBand)DGShift).Visible = true;
			}
			List<object> list = new List<object> { cls.DateSh, cls.Dayofweek };
			List<object> list2 = list;
			list2.Add(await cls.GetNameStatuseTaradodAsync(token));
			List<object> list3 = list;
			list3.Add(await cls.GetHozorTimeAsync(token));
			List<object> list4 = list;
			list4.Add(await cls.GetShiftTimeAsync(token));
			List<object> list5 = list;
			for (int j = 0; j < 8; j++)
			{
				list5.Add(cls.CoupleTaradod(j).VTime);
				list5.Add(cls.CoupleTaradod(j).KTime);
			}
			list5.Add(cls.DateM.ToString("yyyy/MM/dd"));
			((DataGridView)DGrid).Rows[rowindex].SetValues(list5.ToArray());
			for (int k = 0; k < 8; k++)
			{
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgv[k]).Index, rowindex].Style.ForeColor = cls.CoupleTaradod(k).ColorV;
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgkh[k]).Index, rowindex].Style.ForeColor = cls.CoupleTaradod(k).ColorKh;
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgv[k]).Index, rowindex].ReadOnly = !cls.CanEditV;
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgkh[k]).Index, rowindex].ReadOnly = !cls.CanEditK;
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgv[k]).Index, rowindex].ToolTipText = cls.CoupleTaradod(k).GetToolTip();
				((DataGridView)DGrid)[((DataGridViewBand)_lstDgkh[k]).Index, rowindex].ToolTipText = cls.CoupleTaradod(k).GetToolTip();
			}
			DataGridViewCellStyle style = ((DataGridView)DGrid)[((DataGridViewBand)DGStatus).Index, rowindex].Style;
			ColorDet clsColor = _clsColor;
			style.ForeColor = StringExtensions.ToColor(clsColor.GetColor(await cls.GetStatuseTaradodAsync(token)), default(Color));
			DataGridViewCell val = ((DataGridView)DGrid)[((DataGridViewBand)DGShift).Index, rowindex];
			val.ToolTipText = await cls.GetShiftToolTipAsync(token);
			if ((int)(await cls.GetStatuseTaradodAsync(token)) == 6)
			{
				style = ((DataGridView)DGrid)[((DataGridViewBand)DGDate).Index, rowindex].Style;
				clsColor = _clsColor;
				style.ForeColor = StringExtensions.ToColor(clsColor.GetColor(await cls.GetStatuseTaradodAsync(token)), default(Color));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetRowinfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 277);
		}
	}

	private string GetTimeFormat(int row, int col)
	{
		try
		{
			return Utilities.GetTimeFormat(((DataGridView)DGrid)[col, row].Value?.ToString().Trim()) ?? "00:00";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetTimeFormat", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 292);
		}
		return "00:00";
	}

	private CoupleTaradod GetCurentCoupleTaradod(int row, int col)
	{
		try
		{
			for (int i = 0; i < 8; i++)
			{
				if (col == ((DataGridViewBand)_lstDgv[i]).Index || col == ((DataGridViewBand)_lstDgkh[i]).Index)
				{
					return _clsSportClub.ListSportClubDets[row].CoupleTaradod(i);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetCurentCoupleTaradod", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 308);
		}
		return new CoupleTaradod(((CacheBaseForm)this).ContextGuid, DateTime.Parse(((DataGridView)DGrid).Rows[row].Cells[((DataGridViewBand)DGDateM).Index].Value.ToString()));
	}

	private LogOnPersonel GetCurentAttendance(int row, int col)
	{
		try
		{
			for (int i = 0; i < 8; i++)
			{
				if (col == ((DataGridViewBand)_lstDgv[i]).Index)
				{
					return _clsSportClub.ListSportClubDets[row].CoupleTaradod(i).Vtaradod;
				}
				if (col == ((DataGridViewBand)_lstDgkh[i]).Index)
				{
					return _clsSportClub.ListSportClubDets[row].CoupleTaradod(i).Ktaradod;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetCurentAttendance", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 325);
		}
		return new LogOnPersonel(((CacheBaseForm)this).ContextGuid, DateTime.Parse(((DataGridView)DGrid).Rows[row].Cells[((DataGridViewBand)DGDateM).Index].Value.ToString()));
	}

	private async Task RefreshLblAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			((Control)LblName).Text = ((PersonBase)_clsSportClub.ClsPerson).FullName;
			((Control)LblCodeTafsil).Text = ((PersonBase)_clsSportClub.ClsPerson).CodeTafsil.ToString();
			((Control)LblAzTar).Text = _clsSportClub.DateStartSh;
			((Control)LblTaTar).Text = _clsSportClub.DateEndSh;
			LabelX lblStartGharardad = LblStartGharardad;
			((Control)lblStartGharardad).Text = Calendar.MiladiToCalture((await _clsSportClub.GetGharardadAsync(token)).DateStartM);
			lblStartGharardad = LblEndGharardad;
			((Control)lblStartGharardad).Text = Calendar.MiladiToCalture((await _clsSportClub.GetGharardadAsync(token)).DateFinishM);
			((Control)LblGheybat).Text = _clsSportClub.CountGheybat.ToString();
			((Control)LblRows).Text = ((DataGridView)DGrid).Rows.Count.ToString();
			((Control)LblHozor).Text = _clsSportClub.SumKarkerdTime;
			((Control)LblhozorSum).Text = "حضور (" + _clsSportClub.CountKarkerd + "):";
			lblStartGharardad = LblPeriodic;
			((Control)lblStartGharardad).Text = (await (await _clsSportClub.GetGharardadAsync(token)).GetShiftViewModelAsync(token)).Periodic.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshLblAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 349);
		}
	}

	private async void RPTTaradod_Full_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 123)
					{
						e.Handled = true;
						FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
						await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
						((Component)(object)frm).Dispose();
					}
				}
				else
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
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RPTTaradod_Full_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 382);
		}
	}

	private void Btnfish_Click(object sender, EventArgs e)
	{
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
	}

	private void LblYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
	}

	private void dataGridViewX1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		try
		{
			if ((int)((MouseEventArgs)e).Button == 2097152)
			{
				((DataGridView)DGrid).CurrentCell = ((DataGridView)DGrid).Rows[e.RowIndex].Cells[e.ColumnIndex];
				((DataGridViewBand)((DataGridView)DGrid).Rows[e.RowIndex]).Selected = true;
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "dataGridViewX1_CellMouseDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 408);
		}
	}

	private void dataGridViewX1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
	{
	}

	private async void CMS_DeleteAttendance_Click(object sender, EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodDelete)
		{
			return;
		}
		try
		{
			if (((DataGridView)DGrid).CurrentRow != null)
			{
				CoupleTaradod curentCoupleTaradod = GetCurentCoupleTaradod(((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index, ((DataGridView)DGrid).CurrentCell.ColumnIndex);
				LogOnPersonel item = GetCurentAttendance(((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index, ((DataGridView)DGrid).CurrentCell.ColumnIndex);
				if (curentCoupleTaradod.IsTaradodVirtual)
				{
					Utility.MessageBoxShowError(string.Concat(" تردد مورد نظر از نوع " + Utilities.GetDisplay<TypeTaradodEnumType>(curentCoupleTaradod.TypeTaradod) + " میباشد. نمیتوانید آنرا حذف نمائید", "\n و همچنین میتوانید نوع تردد دوم از بازه قبلی یا تردد اول از بازه بعدی را تغییر دهید"), ((CacheBaseForm)this).ContextGuid);
				}
				else if (item.Id > 0)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await item.DeleteAsync(token);
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "CMS_DeleteAttendance_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
					string pDesc = "حذف یک تردد : " + ((PersonBase)_clsPerson).FullName + " تاریخ : " + Calendar.MiladiToCalture(item.DateTimeLogM) + " ساعت : " + item.DateTimeLogM.ToString("HH:mm") + " نوع تردد : " + Utilities.GetDisplay<TypeTaradodEnumType>(item.TypeTaradod) + " شناسه دستگاه : " + item.DeviceId_ + " کد سخت افزاری شخص : " + item.PersonNo;
					await LogUsers.RegisterNewLogAsync(((CacheBaseForm)this).ContextGuid, pDesc, "Taradod.TaradodDelete");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMS_DeleteAttendance_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 450);
		}
	}

	private async void CMS_GharadadList_Click(object sender, EventArgs e)
	{
		try
		{
			FrmGharardadList frm = new FrmGharardadList(((CacheBaseForm)this).ContextGuid, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "CMS_GharadadList_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMS_GharadadList_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 465);
		}
	}

	private async void CMS_ShiftList_Click(object sender, EventArgs e)
	{
		try
		{
			FrmShiftMain frm = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "CMS_ShiftList_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMS_ShiftList_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 480);
		}
	}

	private async void CMS_PersonInfo_Click(object sender, EventArgs e)
	{
		try
		{
			FrmPersonel frm = new FrmPersonel(((CacheBaseForm)this).ContextGuid, _clsPerson, _font, isReadOnly: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "CMS_PersonInfo_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMS_PersonInfo_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\SportClub\\RptTaradodSportClub.cs", 495);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Expected O, but got Unknown
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbc: Expected O, but got Unknown
		//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0dda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de4: Expected O, but got Unknown
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb1: Expected O, but got Unknown
		//IL_1127: Unknown result type (might be due to invalid IL or missing references)
		//IL_1131: Expected O, but got Unknown
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_125f: Expected O, but got Unknown
		//IL_203a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Expected O, but got Unknown
		//IL_2077: Unknown result type (might be due to invalid IL or missing references)
		//IL_2081: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(RptTaradodSportClub));
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
		panelEx1 = new PanelEx();
		DGrid = new DataGridViewX();
		CMS = new ContextMenuStrip(components);
		CMS_DeleteAttendance = new ToolStripMenuItem();
		toolStripSeparator2 = new ToolStripSeparator();
		CMS_GharadadList = new ToolStripMenuItem();
		CMS_ShiftList = new ToolStripMenuItem();
		toolStripSeparator1 = new ToolStripSeparator();
		CMS_PersonInfo = new ToolStripMenuItem();
		toolStripSeparator3 = new ToolStripSeparator();
		LblName = new LinkLabel();
		labelX5 = new LabelX();
		LblTaTar = new LinkLabel();
		LblAzTar = new LinkLabel();
		LblCodeTafsil = new LinkLabel();
		labelX7 = new LabelX();
		LblGheybat = new LabelX();
		labelX1 = new LabelX();
		LblEndGharardad = new LabelX();
		PicPerson = new PictureBox();
		labelX4 = new LabelX();
		BtnPrint = new ButtonX();
		labelX8 = new LabelX();
		LblKasrsum = new LabelX();
		LblhozorSum = new LabelX();
		LblPeriodic = new LabelX();
		LblRow = new LabelX();
		LblStartGharardad = new LabelX();
		LblHozor = new LabelX();
		LblRows = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		personBindingSource = new BindingSource(components);
		DGDate = new DataGridViewTextBoxColumn();
		DgDay = new DataGridViewTextBoxColumn();
		DGStatus = new DataGridViewTextBoxColumn();
		DGHozor = new DataGridViewTextBoxColumn();
		DGShift = new DataGridViewTextBoxColumn();
		DGV1 = new DataGridViewTextBoxColumn();
		DGKH1 = new DataGridViewTextBoxColumn();
		DGV2 = new DataGridViewTextBoxColumn();
		DGKH2 = new DataGridViewTextBoxColumn();
		DGV3 = new DataGridViewTextBoxColumn();
		DGKH3 = new DataGridViewTextBoxColumn();
		DGV4 = new DataGridViewTextBoxColumn();
		DGKH4 = new DataGridViewTextBoxColumn();
		DGV5 = new DataGridViewTextBoxColumn();
		DGKH5 = new DataGridViewTextBoxColumn();
		DGV6 = new DataGridViewTextBoxColumn();
		DGKH6 = new DataGridViewTextBoxColumn();
		DGV7 = new DataGridViewTextBoxColumn();
		DGKH7 = new DataGridViewTextBoxColumn();
		DGV8 = new DataGridViewTextBoxColumn();
		DGKH8 = new DataGridViewTextBoxColumn();
		DGDateM = new DataGridViewTextBoxColumn();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)CMS).SuspendLayout();
		((ISupportInitialize)PicPerson).BeginInit();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)LblName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)LblTaTar);
		((Control)panelEx1).Controls.Add((Control)(object)LblAzTar);
		((Control)panelEx1).Controls.Add((Control)(object)LblCodeTafsil);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)LblGheybat);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)LblEndGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)PicPerson);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)LblKasrsum);
		((Control)panelEx1).Controls.Add((Control)(object)LblhozorSum);
		((Control)panelEx1).Controls.Add((Control)(object)LblPeriodic);
		((Control)panelEx1).Controls.Add((Control)(object)LblRow);
		((Control)panelEx1).Controls.Add((Control)(object)LblStartGharardad);
		((Control)panelEx1).Controls.Add((Control)(object)LblHozor);
		((Control)panelEx1).Controls.Add((Control)(object)LblRows);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1008, 730);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeColumns = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[22]
		{
			(DataGridViewColumn)DGDate,
			(DataGridViewColumn)DgDay,
			(DataGridViewColumn)DGStatus,
			(DataGridViewColumn)DGHozor,
			(DataGridViewColumn)DGShift,
			(DataGridViewColumn)DGV1,
			(DataGridViewColumn)DGKH1,
			(DataGridViewColumn)DGV2,
			(DataGridViewColumn)DGKH2,
			(DataGridViewColumn)DGV3,
			(DataGridViewColumn)DGKH3,
			(DataGridViewColumn)DGV4,
			(DataGridViewColumn)DGKH4,
			(DataGridViewColumn)DGV5,
			(DataGridViewColumn)DGKH5,
			(DataGridViewColumn)DGV6,
			(DataGridViewColumn)DGKH6,
			(DataGridViewColumn)DGV7,
			(DataGridViewColumn)DGKH7,
			(DataGridViewColumn)DGV8,
			(DataGridViewColumn)DGKH8,
			(DataGridViewColumn)DGDateM
		});
		((Control)DGrid).ContextMenuStrip = CMS;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 58);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)DGrid).Size = new Size(1003, 635);
		((Control)DGrid).TabIndex = 0;
		((DataGridView)DGrid).CellEndEdit += new DataGridViewCellEventHandler(DGrid_CellEndEdit);
		((DataGridView)DGrid).CellMouseDown += new DataGridViewCellMouseEventHandler(dataGridViewX1_CellMouseDown);
		((DataGridView)DGrid).CellMouseUp += new DataGridViewCellMouseEventHandler(dataGridViewX1_CellMouseUp);
		((ToolStrip)CMS).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[7]
		{
			(ToolStripItem)CMS_DeleteAttendance,
			(ToolStripItem)toolStripSeparator2,
			(ToolStripItem)CMS_GharadadList,
			(ToolStripItem)CMS_ShiftList,
			(ToolStripItem)toolStripSeparator1,
			(ToolStripItem)CMS_PersonInfo,
			(ToolStripItem)toolStripSeparator3
		});
		((Control)CMS).Name = "CMS";
		((Control)CMS).RightToLeft = (RightToLeft)1;
		((Control)CMS).Size = new Size(159, 110);
		((ToolStripItem)CMS_DeleteAttendance).Name = "CMS_DeleteAttendance";
		((ToolStripItem)CMS_DeleteAttendance).Size = new Size(158, 22);
		((ToolStripItem)CMS_DeleteAttendance).Text = "[حذف تردد]";
		((ToolStripItem)CMS_DeleteAttendance).Click += CMS_DeleteAttendance_Click;
		((ToolStripItem)toolStripSeparator2).Name = "toolStripSeparator2";
		((ToolStripItem)toolStripSeparator2).Size = new Size(155, 6);
		((ToolStripItem)CMS_GharadadList).Name = "CMS_GharadadList";
		((ToolStripItem)CMS_GharadadList).Size = new Size(158, 22);
		((ToolStripItem)CMS_GharadadList).Text = "[Gharardadha]";
		((ToolStripItem)CMS_GharadadList).Click += CMS_GharadadList_Click;
		((ToolStripItem)CMS_ShiftList).Name = "CMS_ShiftList";
		((ToolStripItem)CMS_ShiftList).Size = new Size(158, 22);
		((ToolStripItem)CMS_ShiftList).Text = "[پکیج ها]";
		((ToolStripItem)CMS_ShiftList).Click += CMS_ShiftList_Click;
		((ToolStripItem)toolStripSeparator1).Name = "toolStripSeparator1";
		((ToolStripItem)toolStripSeparator1).Size = new Size(155, 6);
		((ToolStripItem)CMS_PersonInfo).Name = "CMS_PersonInfo";
		((ToolStripItem)CMS_PersonInfo).Size = new Size(158, 22);
		((ToolStripItem)CMS_PersonInfo).Text = "[PersonInfo]";
		((ToolStripItem)CMS_PersonInfo).Click += CMS_PersonInfo_Click;
		((ToolStripItem)toolStripSeparator3).Name = "toolStripSeparator3";
		((ToolStripItem)toolStripSeparator3).Size = new Size(155, 6);
		((Control)LblName).Anchor = (AnchorStyles)9;
		((Control)LblName).ForeColor = Color.DarkRed;
		LblName.LinkBehavior = (LinkBehavior)3;
		LblName.LinkColor = Color.DarkRed;
		((Control)LblName).Location = new Point(585, 35);
		((Control)LblName).Name = "LblName";
		((Control)LblName).RightToLeft = (RightToLeft)1;
		((Control)LblName).Size = new Size(88, 13);
		((Control)LblName).TabIndex = 86;
		LblName.TabStop = true;
		((Control)LblName).Text = "-------";
		((Label)LblName).TextAlign = (ContentAlignment)16;
		LblName.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(697, 31);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(69, 21);
		((Control)labelX5).TabIndex = 85;
		((Control)labelX5).Text = "نام شخص: ";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)LblTaTar).Anchor = (AnchorStyles)9;
		((Control)LblTaTar).ForeColor = Color.DarkRed;
		LblTaTar.LinkBehavior = (LinkBehavior)3;
		LblTaTar.LinkColor = Color.DarkRed;
		((Control)LblTaTar).Location = new Point(585, 12);
		((Control)LblTaTar).Name = "LblTaTar";
		((Control)LblTaTar).RightToLeft = (RightToLeft)1;
		((Control)LblTaTar).Size = new Size(88, 13);
		((Control)LblTaTar).TabIndex = 84;
		LblTaTar.TabStop = true;
		((Control)LblTaTar).Text = "-------";
		((Label)LblTaTar).TextAlign = (ContentAlignment)16;
		LblTaTar.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)LblAzTar).Anchor = (AnchorStyles)9;
		((Control)LblAzTar).ForeColor = Color.DarkRed;
		LblAzTar.LinkBehavior = (LinkBehavior)3;
		LblAzTar.LinkColor = Color.DarkRed;
		((Control)LblAzTar).Location = new Point(785, 12);
		((Control)LblAzTar).Name = "LblAzTar";
		((Control)LblAzTar).RightToLeft = (RightToLeft)1;
		((Control)LblAzTar).Size = new Size(69, 13);
		((Control)LblAzTar).TabIndex = 83;
		LblAzTar.TabStop = true;
		((Control)LblAzTar).Text = "-------";
		((Label)LblAzTar).TextAlign = (ContentAlignment)16;
		LblAzTar.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)LblCodeTafsil).Anchor = (AnchorStyles)9;
		((Control)LblCodeTafsil).ForeColor = Color.DarkRed;
		LblCodeTafsil.LinkBehavior = (LinkBehavior)3;
		LblCodeTafsil.LinkColor = Color.DarkRed;
		((Control)LblCodeTafsil).Location = new Point(785, 35);
		((Control)LblCodeTafsil).Name = "LblCodeTafsil";
		((Control)LblCodeTafsil).RightToLeft = (RightToLeft)1;
		((Control)LblCodeTafsil).Size = new Size(69, 13);
		((Control)LblCodeTafsil).TabIndex = 82;
		LblCodeTafsil.TabStop = true;
		((Control)LblCodeTafsil).Text = "-------";
		((Label)LblCodeTafsil).TextAlign = (ContentAlignment)16;
		LblCodeTafsil.LinkClicked += new LinkLabelLinkClickedEventHandler(LblYear_LinkClicked);
		((Control)labelX7).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(917, 713);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).RightToLeft = (RightToLeft)1;
		((Control)labelX7).Size = new Size(81, 14);
		((Control)labelX7).TabIndex = 81;
		((Control)labelX7).Text = "عدم حضور: ";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)LblGheybat).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblGheybat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblGheybat).ForeColor = Color.DarkRed;
		((Control)LblGheybat).Location = new Point(847, 713);
		((Control)LblGheybat).Name = "LblGheybat";
		((Control)LblGheybat).RightToLeft = (RightToLeft)1;
		((Control)LblGheybat).Size = new Size(57, 14);
		((Control)LblGheybat).TabIndex = 80;
		((Control)LblGheybat).Text = "------";
		((Control)labelX1).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(757, 713);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(86, 14);
		((Control)labelX1).TabIndex = 81;
		((Control)labelX1).Text = "تاریخ پایان:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)LblEndGharardad).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblEndGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblEndGharardad).ForeColor = Color.DarkRed;
		((Control)LblEndGharardad).Location = new Point(675, 713);
		((Control)LblEndGharardad).Name = "LblEndGharardad";
		((Control)LblEndGharardad).RightToLeft = (RightToLeft)1;
		((Control)LblEndGharardad).Size = new Size(69, 14);
		((Control)LblEndGharardad).TabIndex = 80;
		((Control)LblEndGharardad).Text = "------";
		((Control)PicPerson).Anchor = (AnchorStyles)9;
		PicPerson.BorderStyle = (BorderStyle)1;
		PicPerson.Image = (Image)componentResourceManager.GetObject("PicPerson.Image");
		((Control)PicPerson).Location = new Point(949, 3);
		((Control)PicPerson).Name = "PicPerson";
		((Control)PicPerson).Size = new Size(57, 53);
		PicPerson.SizeMode = (PictureBoxSizeMode)1;
		PicPerson.TabIndex = 76;
		PicPerson.TabStop = false;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(881, 31);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(51, 21);
		((Control)labelX4).TabIndex = 75;
		((Control)labelX4).Text = "کد شخص: ";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		((Control)BtnPrint).Anchor = (AnchorStyles)13;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(3, 3);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(565, 53);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 74;
		((Control)BtnPrint).Text = "[چاپ] (Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)labelX8).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(550, 693);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).RightToLeft = (RightToLeft)1;
		((Control)labelX8).Size = new Size(128, 14);
		((Control)labelX8).TabIndex = 48;
		((Control)labelX8).Text = "تعداد جلسات مجاز: ";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)LblKasrsum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblKasrsum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKasrsum).Location = new Point(757, 693);
		((Control)LblKasrsum).Name = "LblKasrsum";
		((Control)LblKasrsum).RightToLeft = (RightToLeft)1;
		((Control)LblKasrsum).Size = new Size(93, 14);
		((Control)LblKasrsum).TabIndex = 48;
		((Control)LblKasrsum).Text = "تاریخ ثبت نام: ";
		LblKasrsum.TextAlignment = (StringAlignment)2;
		((Control)LblhozorSum).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblhozorSum).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblhozorSum).Location = new Point(917, 693);
		((Control)LblhozorSum).Name = "LblhozorSum";
		((Control)LblhozorSum).RightToLeft = (RightToLeft)1;
		((Control)LblhozorSum).Size = new Size(81, 14);
		((Control)LblhozorSum).TabIndex = 47;
		((Control)LblhozorSum).Text = "حضور: ";
		LblhozorSum.TextAlignment = (StringAlignment)2;
		((Control)LblPeriodic).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblPeriodic).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPeriodic).ForeColor = Color.DarkRed;
		((Control)LblPeriodic).Location = new Point(471, 693);
		((Control)LblPeriodic).Name = "LblPeriodic";
		((Control)LblPeriodic).RightToLeft = (RightToLeft)1;
		((Control)LblPeriodic).Size = new Size(69, 14);
		((Control)LblPeriodic).TabIndex = 45;
		((Control)LblPeriodic).Text = "------";
		((Control)LblRow).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRow).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRow).Location = new Point(111, 693);
		((Control)LblRow).Name = "LblRow";
		((Control)LblRow).RightToLeft = (RightToLeft)1;
		((Control)LblRow).Size = new Size(61, 14);
		((Control)LblRow).TabIndex = 46;
		((Control)LblRow).Text = "تعداد روزها: ";
		LblRow.TextAlignment = (StringAlignment)2;
		((Control)LblStartGharardad).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblStartGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblStartGharardad).ForeColor = Color.DarkRed;
		((Control)LblStartGharardad).Location = new Point(675, 693);
		((Control)LblStartGharardad).Name = "LblStartGharardad";
		((Control)LblStartGharardad).RightToLeft = (RightToLeft)1;
		((Control)LblStartGharardad).Size = new Size(69, 14);
		((Control)LblStartGharardad).TabIndex = 45;
		((Control)LblStartGharardad).Text = "------";
		((Control)LblHozor).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblHozor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHozor).ForeColor = Color.DarkRed;
		((Control)LblHozor).Location = new Point(847, 693);
		((Control)LblHozor).Name = "LblHozor";
		((Control)LblHozor).RightToLeft = (RightToLeft)1;
		((Control)LblHozor).Size = new Size(57, 14);
		((Control)LblHozor).TabIndex = 44;
		((Control)LblHozor).Text = "------";
		((Control)LblRows).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRows).ForeColor = Color.DarkRed;
		((Control)LblRows).Location = new Point(22, 693);
		((Control)LblRows).Name = "LblRows";
		((Control)LblRows).RightToLeft = (RightToLeft)1;
		((Control)LblRows).Size = new Size(62, 14);
		((Control)LblRows).TabIndex = 43;
		((Control)LblRows).Text = "------";
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		labelX3.ImagePosition = (eImagePosition)2;
		((Control)labelX3).Location = new Point(697, 8);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(69, 21);
		((Control)labelX3).TabIndex = 25;
		((Control)labelX3).Text = "تا تاریخ: ";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(881, 8);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(51, 21);
		((Control)labelX2).TabIndex = 23;
		((Control)labelX2).Text = "از تاریخ: ";
		labelX2.TextAlignment = (StringAlignment)2;
		val2.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DGDate).DefaultCellStyle = val2;
		((DataGridViewColumn)DGDate).HeaderText = "[تاریخ]";
		((DataGridViewColumn)DGDate).MinimumWidth = 100;
		((DataGridViewColumn)DGDate).Name = "DGDate";
		((DataGridViewBand)DGDate).ReadOnly = true;
		((DataGridViewColumn)DgDay).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgDay).HeaderText = "[روز]";
		((DataGridViewColumn)DgDay).MinimumWidth = 30;
		((DataGridViewColumn)DgDay).Name = "DgDay";
		((DataGridViewBand)DgDay).ReadOnly = true;
		((DataGridViewColumn)DgDay).Width = 57;
		((DataGridViewColumn)DGStatus).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGStatus).HeaderText = "[Status]";
		((DataGridViewColumn)DGStatus).MinimumWidth = 120;
		((DataGridViewColumn)DGStatus).Name = "DGStatus";
		((DataGridViewBand)DGStatus).ReadOnly = true;
		((DataGridViewColumn)DGHozor).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGHozor).HeaderText = "[زمان حضور]";
		((DataGridViewColumn)DGHozor).Name = "DGHozor";
		((DataGridViewBand)DGHozor).ReadOnly = true;
		((DataGridViewColumn)DGHozor).Width = 89;
		((DataGridViewColumn)DGShift).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGShift).HeaderText = "[پکیج]";
		((DataGridViewColumn)DGShift).Name = "DGShift";
		((DataGridViewBand)DGShift).ReadOnly = true;
		((DataGridViewColumn)DGShift).Width = 81;
		val3.BackColor = Color.FromArgb(255, 192, 192);
		val3.NullValue = null;
		((DataGridViewBand)DGV1).DefaultCellStyle = val3;
		((DataGridViewColumn)DGV1).HeaderText = "[Login]1";
		((DataGridViewColumn)DGV1).Name = "DGV1";
		((DataGridViewBand)DGV1).ReadOnly = true;
		((DataGridViewColumn)DGV1).Width = 50;
		val4.BackColor = Color.FromArgb(255, 192, 192);
		((DataGridViewBand)DGKH1).DefaultCellStyle = val4;
		((DataGridViewColumn)DGKH1).HeaderText = "[خروج]1";
		((DataGridViewColumn)DGKH1).Name = "DGKH1";
		((DataGridViewBand)DGKH1).ReadOnly = true;
		((DataGridViewColumn)DGKH1).Width = 50;
		val5.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridViewBand)DGV2).DefaultCellStyle = val5;
		((DataGridViewColumn)DGV2).HeaderText = "[Login]2";
		((DataGridViewColumn)DGV2).Name = "DGV2";
		((DataGridViewBand)DGV2).ReadOnly = true;
		((DataGridViewColumn)DGV2).Width = 50;
		val6.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridViewBand)DGKH2).DefaultCellStyle = val6;
		((DataGridViewColumn)DGKH2).HeaderText = "[خروج]2";
		((DataGridViewColumn)DGKH2).Name = "DGKH2";
		((DataGridViewBand)DGKH2).ReadOnly = true;
		((DataGridViewColumn)DGKH2).Width = 50;
		val7.BackColor = Color.FromArgb(192, 255, 192);
		((DataGridViewBand)DGV3).DefaultCellStyle = val7;
		((DataGridViewColumn)DGV3).HeaderText = "[Login]3";
		((DataGridViewColumn)DGV3).Name = "DGV3";
		((DataGridViewBand)DGV3).ReadOnly = true;
		((DataGridViewColumn)DGV3).Width = 50;
		val8.BackColor = Color.FromArgb(192, 255, 192);
		((DataGridViewBand)DGKH3).DefaultCellStyle = val8;
		((DataGridViewColumn)DGKH3).HeaderText = "[خروج]3";
		((DataGridViewColumn)DGKH3).Name = "DGKH3";
		((DataGridViewBand)DGKH3).ReadOnly = true;
		((DataGridViewColumn)DGKH3).Width = 50;
		((DataGridViewColumn)DGV4).HeaderText = "[Login]4";
		((DataGridViewColumn)DGV4).Name = "DGV4";
		((DataGridViewBand)DGV4).ReadOnly = true;
		((DataGridViewColumn)DGV4).Width = 50;
		((DataGridViewColumn)DGKH4).HeaderText = "[خروج]4";
		((DataGridViewColumn)DGKH4).Name = "DGKH4";
		((DataGridViewBand)DGKH4).ReadOnly = true;
		((DataGridViewColumn)DGKH4).Width = 50;
		val9.BackColor = Color.FromArgb(255, 192, 255);
		((DataGridViewBand)DGV5).DefaultCellStyle = val9;
		((DataGridViewColumn)DGV5).HeaderText = "[Login]5";
		((DataGridViewColumn)DGV5).Name = "DGV5";
		((DataGridViewBand)DGV5).ReadOnly = true;
		((DataGridViewColumn)DGV5).Width = 50;
		val10.BackColor = Color.FromArgb(255, 192, 255);
		((DataGridViewBand)DGKH5).DefaultCellStyle = val10;
		((DataGridViewColumn)DGKH5).HeaderText = "[خروج]5";
		((DataGridViewColumn)DGKH5).Name = "DGKH5";
		((DataGridViewBand)DGKH5).ReadOnly = true;
		((DataGridViewColumn)DGKH5).Width = 50;
		val11.BackColor = Color.FromArgb(192, 192, 255);
		((DataGridViewBand)DGV6).DefaultCellStyle = val11;
		((DataGridViewColumn)DGV6).HeaderText = "[Login]6";
		((DataGridViewColumn)DGV6).Name = "DGV6";
		((DataGridViewBand)DGV6).ReadOnly = true;
		((DataGridViewColumn)DGV6).Width = 50;
		val12.BackColor = Color.FromArgb(192, 192, 255);
		((DataGridViewBand)DGKH6).DefaultCellStyle = val12;
		((DataGridViewColumn)DGKH6).HeaderText = "[خروج]6";
		((DataGridViewColumn)DGKH6).Name = "DGKH6";
		((DataGridViewBand)DGKH6).ReadOnly = true;
		((DataGridViewColumn)DGKH6).Width = 50;
		val13.BackColor = Color.FromArgb(192, 255, 255);
		((DataGridViewBand)DGV7).DefaultCellStyle = val13;
		((DataGridViewColumn)DGV7).HeaderText = "[Login]7";
		((DataGridViewColumn)DGV7).Name = "DGV7";
		((DataGridViewBand)DGV7).ReadOnly = true;
		((DataGridViewColumn)DGV7).Width = 50;
		val14.BackColor = Color.FromArgb(192, 255, 255);
		((DataGridViewBand)DGKH7).DefaultCellStyle = val14;
		((DataGridViewColumn)DGKH7).HeaderText = "[خروج]7";
		((DataGridViewColumn)DGKH7).Name = "DGKH7";
		((DataGridViewBand)DGKH7).ReadOnly = true;
		((DataGridViewColumn)DGKH7).Width = 50;
		val15.BackColor = Color.WhiteSmoke;
		((DataGridViewBand)DGV8).DefaultCellStyle = val15;
		((DataGridViewColumn)DGV8).HeaderText = "[Login]8";
		((DataGridViewColumn)DGV8).Name = "DGV8";
		((DataGridViewBand)DGV8).ReadOnly = true;
		((DataGridViewColumn)DGV8).Width = 50;
		val16.BackColor = Color.WhiteSmoke;
		((DataGridViewBand)DGKH8).DefaultCellStyle = val16;
		((DataGridViewColumn)DGKH8).HeaderText = "[خروج]8";
		((DataGridViewColumn)DGKH8).Name = "DGKH8";
		((DataGridViewBand)DGKH8).ReadOnly = true;
		((DataGridViewColumn)DGKH8).Width = 50;
		((DataGridViewColumn)DGDateM).HeaderText = "[تاریخ میلادی]";
		((DataGridViewColumn)DGDateM).Name = "DGDateM";
		((DataGridViewBand)DGDateM).ReadOnly = true;
		((DataGridViewBand)DGDateM).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1008, 730);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "RptTaradodSportClub";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[ReportFullTaradod]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(RPTTaradod_Full_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)CMS).ResumeLayout(false);
		((ISupportInitialize)PicPerson).EndInit();
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((BaseForm)this).OnLoad(e);
	}
}
