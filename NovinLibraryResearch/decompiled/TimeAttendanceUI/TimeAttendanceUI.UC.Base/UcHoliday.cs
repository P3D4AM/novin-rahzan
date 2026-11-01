using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Other;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UcHoliday : UserControl, IHasContext
{
	public delegate void ExitUserControlEventHandler();

	private Font _font;

	private List<Holiday> _listData;

	private SortableBindingList<Holiday> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _highlightToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal DataGridViewX DGrid;

	private LabelX CountRows;

	private LabelX LblRowCount;

	private ButtonX BtnPrint;

	private UCTatili ucTatili1;

	private CheckBoxX ChbMonasebat;

	private CheckBoxX ChbHoliday;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	private LabelX LblCountHol;

	private LabelX labelX4;

	private CheckBox ChbFromdate;

	private BindingSource holidayBindingSource;

	internal LabelX labelX3;

	private DateBoxFa TxtDate2;

	private DateBoxFa TxtDate1;

	internal ButtonX BtnUpdateTaghvim;

	private DataGridViewCheckBoxColumn DGSTRasmi;

	private DataGridViewTextBoxColumn DGDateSh;

	private DataGridViewTextBoxColumn DGDay;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn DGDateM;

	public string ContextGuid { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public string DateShInGrid
	{
		get
		{
			try
			{
				if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
				{
					return "";
				}
				return ((DataGridView)DGrid)[((DataGridViewBand)DGDateSh).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DateShInGrid", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 142);
			}
			return "";
		}
	}

	public event ExitUserControlEventHandler ExitUserControl;

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
				_highlightToken?.Cancel();
				_highlightToken?.Dispose();
				_highlightToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UcHoliday(string contextGuid, Font font)
	{
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			ContextGuid = contextGuid;
			InitializeComponent();
			((Control)DGrid).Font = _font;
			((Control)TxtSearch).Focus();
			ChbFromdate.Checked = true;
			ChbHoliday.Checked = false;
			ChbMonasebat.Checked = false;
			DateBoxFa txtDate = TxtDate1;
			bool enabled = (((Control)TxtDate2).Enabled = ChbFromdate.Checked);
			((Control)txtDate).Enabled = enabled;
			string text = Calendar.MiladiToCalture(DateTime.Now);
			((Control)TxtDate1).Text = text.Substring(0, 5) + "01/01";
			((Control)TxtDate2).Text = text.Substring(0, 5) + "12/29";
			ChbFromdate.CheckedChanged += ChbFromdate_CheckedChanged;
			((Control)TxtDate1).TextChanged += TxtSearch_TextChanged;
			((Control)TxtDate2).TextChanged += TxtSearch_TextChanged;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Control)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 98);
		}
	}

	public async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			string contextGuid = ContextGuid;
			CancellationToken token2 = token;
			_listData = (await Holiday.GetAllAsync(contextGuid, null, null, isHolidayOnly: false, token2))?.OrderBy((Holiday p) => p.DateM).ToList();
			if (!token.IsCancellationRequested)
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token3 = _searchToken.Token;
				await SearchAsync(token3);
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

	protected void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Control)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 157);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				string contextGuid = ContextGuid;
				CancellationToken token2 = token;
				_listData = (await Holiday.GetAllAsync(contextGuid, null, null, isHolidayOnly: false, token2))?.OrderBy((Holiday p) => p.DateM).ToList();
			}
			_listForDisplay = null;
			IEnumerable<Holiday> enumerable = _listData;
			if (string.IsNullOrEmpty(_searchString))
			{
				UcHoliday ucHoliday = this;
				List<Holiday> listData = _listData;
				ucHoliday._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<Holiday>((IEnumerable<Holiday>)listData, token) : null);
			}
			else
			{
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((Holiday p) => p.Monasebat.Contains(word) || p.Description.Contains(word));
				}
			}
			if (ChbFromdate.Checked)
			{
				enumerable = enumerable.Where((Holiday p) => p.DateM >= Utilities.CalendarToMiladi(((Control)TxtDate1).Text) && p.DateM <= Utilities.CalendarToMiladi(((Control)TxtDate2).Text));
			}
			if (ChbHoliday.Checked)
			{
				enumerable = enumerable.Where((Holiday p) => p.StRasmi);
			}
			if (ChbMonasebat.Checked)
			{
				enumerable = enumerable.Where((Holiday p) => string.Compare(p.Monasebat, "", StringComparison.Ordinal) > 0);
			}
			_listForDisplay = Utilities.ToSortableBindingList<Holiday>(enumerable, token);
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<Holiday>)(object)_listForDisplay).Count;
			countRecordTr = Utilities.GetTranslate("[تعداد رکوردها]");
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						holidayBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = countRecordTr + " : ";
						((Control)CountRows).Text = count.ToString("N0");
						((Control)LblCountHol).Text = ((IEnumerable<Holiday>)_listForDisplay).Count((Holiday q) => q.StRasmi).ToString();
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
			await HighLightGridAsync(token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	public void Insert()
	{
		((PopupItemControl)BtnAdd).PerformClick();
	}

	public void UpDate()
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	public void Delete()
	{
		((PopupItemControl)BtnDelete).PerformClick();
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
			{
				return;
			}
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.CanVacationHolidays(UserAttendance.SelectedUser.AccessLevelMerg))
			{
				Utility.MessageBoxShowError("[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید تعطیلات رسمی ایجاد نمائید]", ContextGuid);
				return;
			}
			FrmHoliday frm = new FrmHoliday(ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده", (Control)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs");
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 272);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((Control)BtnEdit).Enabled && ((Control)BtnEdit).Visible)
			{
				DateTime dateM = LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(DateShInGrid);
				Holiday holiday = await Holiday.GetAsync(ContextGuid, dateM);
				FrmHoliday frm = new FrmHoliday(ContextGuid, holiday, dateM);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده", (Control)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs");
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 300);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (((Control)BtnDelete).Enabled && ((Control)BtnDelete).Visible && !(DateShInGrid == ""))
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				Holiday holiday = await Holiday.GetAsync(ContextGuid, LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(DateShInGrid), token);
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف تعطیلی انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					await holiday.DeleteAsync(token);
					await LoadDataAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 321);
		}
	}

	private void TxtSearch_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TxtSearch).SelectAll();
			((Control)TxtSearch).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 335);
		}
	}

	private void TxtSearch_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 346);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13 || (int)e.KeyCode == 40)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 358);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Focus();
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = 9999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 376);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((PopupItemControl)BtnEdit).PerformClick();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 389);
		}
	}

	public async Task HighLightGridAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(1000);
			if (token.IsCancellationRequested || IsDisposed || _listForDisplay == null || ((Collection<Holiday>)(object)_listForDisplay).Count <= 0)
			{
				return;
			}
			Color cRasmi = Color.Red;
			Color cghRasmi = Color.Black;
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			cRasmi = ((timeAttendanceSettings.Rasmi == 0) ? ucTatili1.Rasmi : Color.FromArgb(timeAttendanceSettings.Rasmi));
			cghRasmi = ((timeAttendanceSettings.GhRasmi == 0) ? ucTatili1.GhRasmi : Color.FromArgb(timeAttendanceSettings.GhRasmi));
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
						{
							bool flag = false;
							if (((DataGridView)DGrid)[((DataGridViewBand)DGSTRasmi).Index, i].Value.ToString() == "")
							{
								flag = true;
							}
							else
							{
								DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGSTRasmi).Index];
								DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
								if (val != null)
								{
									flag = Convert.ToBoolean(((DataGridViewCell)val).Value);
								}
							}
							if (flag)
							{
								((DataGridViewBand)((DataGridView)DGrid).Rows[i]).DefaultCellStyle.ForeColor = cRasmi;
							}
							else
							{
								((DataGridViewBand)((DataGridView)DGrid).Rows[i]).DefaultCellStyle.ForeColor = cghRasmi;
							}
						}
					}
				}
				catch (OperationCanceledException)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "HighLightGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 443);
		}
	}

	public void Print()
	{
		((PopupItemControl)BtnPrint).PerformClick();
	}

	private async void ucTatili1_ColorTatiliChanedEvent()
	{
		_ = 1;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await SaveColorAsync(token);
			await HighLightGridAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucTatili1_ColorTatiliChanedEvent", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 464);
		}
	}

	private async Task SaveColorAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			TimeAttendanceSettings obj = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			obj.Rasmi = ((Control)ucTatili1.lBLRasmi).ForeColor.ToArgb();
			obj.GhRasmi = ((Control)ucTatili1.LBLGhRasmi).ForeColor.ToArgb();
			await obj.SaveAsync(ContextGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveColorAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 478);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			SortableBindingList<Holiday> listForDisplay = _listForDisplay;
			List<object> list = new List<object>();
			foreach (Holiday item in (Collection<Holiday>)(object)listForDisplay)
			{
				list.Add(new
				{
					DateM = item.DateM,
					Day = item.Day,
					Description = item.Description,
					Monasebat = item.Monasebat,
					StRasmi = item.StRasmi,
					DateCalture = Calendar.MiladiToCalture(item.DateM)
				});
			}
			FrmPrint frm = new FrmPrint(ContextGuid, (EnumPrintShow)7, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 497);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		Search();
	}

	public void GotoMonth(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
			{
				string text = Calendar.MiladiToCalture(DateTime.Now);
				short num = (short)(e.KeyCode - 112 + 1);
				int yearOfDate = LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(text);
				ChbFromdate.Checked = true;
				((Control)TxtDate1).Text = Calendar.MiladiToCalture(LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth(num, yearOfDate, "00:00:00"));
				((Control)TxtDate2).Text = Calendar.MiladiToCalture(LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth(num, yearOfDate, "23:59:59"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GotoMonth", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 517);
		}
	}

	private async void ChbFromdate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtDate1).Enabled = ChbFromdate.Checked;
			((Control)TxtDate2).Enabled = ChbFromdate.Checked;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbFromdate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 536);
		}
	}

	private void BtnUpdateTaghvim_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[تمامی اطلاعات تقویم در بازه انتخاب شده جایگزین خواهد شد. آیا بروزرسانی انجام شود؟] "), Utilities.GetTranslate("[بروزرسانی تقویم ]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation) == 6)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				((Control)BtnUpdateTaghvim).Enabled = false;
				Task task = Task.Run(async delegate
				{
					await UpdateTaghvimAsync(token);
				});
				new Waiter("در حال بروزرسانی تقویم", (Control)(object)this, task, false, (EnWaiterType)0, (Control)null, "BtnUpdateTaghvim_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnUpdateTaghvim_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 567);
		}
	}

	private async Task UpdateTaghvimAsync(CancellationToken token)
	{
		try
		{
			_ = 3;
			try
			{
				ReturnedSaveFuncInfoWithValue<List<WebCalender>> val = await WebCalender.GetAllAsync(token);
				if (((ReturnedSaveFuncInfo)val).HasError)
				{
					Utility.MessageBoxShow(((ReturnedSaveFuncInfo)val).ErrorMessage, ContextGuid);
					return;
				}
				List<WebCalender> value = val.value;
				if (value != null && value.Count > 0)
				{
					foreach (WebCalender item in value)
					{
						if (item != null && !(item.DateM > LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(((Control)TxtDate2).Text)) && !(item.DateM < LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(((Control)TxtDate1).Text)))
						{
							await new Holiday(ContextGuid)
							{
								DateM = item.DateM,
								Description = item.Description,
								Monasebat = item.Monasebat,
								StRasmi = item.STRasmi
							}.SaveAsync(token);
						}
					}
					while (!((Control)this).IsHandleCreated && !IsDisposed)
					{
						await Task.Delay(100);
					}
					if (IsDisposed)
					{
						return;
					}
					await LoadDataAsync(token);
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[تقویم در بازه انتخاب شده بروز شد]", "", 3);
				}
				else
				{
					Utility.MessageBoxShowError("[بدلیل خطای ارتباطی بروزرسانی انجام نشد.]", ContextGuid);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UpdateTaghvimAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 611);
			}
		}
		finally
		{
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			if (!IsDisposed)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					try
					{
						((Control)BtnUpdateTaghvim).Enabled = true;
					}
					catch (Exception ex2)
					{
						WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "UpdateTaghvimAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCHoliday.cs", 627);
					}
				});
			}
		}
	}

	private async void DGrid_Sorted(object sender, EventArgs e)
	{
		try
		{
			_highlightToken?.Cancel();
			_highlightToken = new CancellationTokenSource();
			CancellationToken token = _highlightToken.Token;
			await HighLightGridAsync(token);
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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Expected O, but got Unknown
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Expected O, but got Unknown
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b84: Expected O, but got Unknown
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Expected O, but got Unknown
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb0: Expected O, but got Unknown
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff9: Expected O, but got Unknown
		//IL_1025: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Expected O, but got Unknown
		//IL_11b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bb: Expected O, but got Unknown
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f1: Expected O, but got Unknown
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_164d: Expected O, but got Unknown
		//IL_18a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ac: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnUpdateTaghvim = new ButtonX();
		LblCountHol = new LabelX();
		labelX4 = new LabelX();
		ucTatili1 = new UCTatili();
		BtnPrint = new ButtonX();
		CountRows = new LabelX();
		LblRowCount = new LabelX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		DGrid = new DataGridViewX();
		holidayBindingSource = new BindingSource(components);
		GroupPanel1 = new GroupPanel();
		TxtDate2 = new DateBoxFa();
		TxtDate1 = new DateBoxFa();
		labelX3 = new LabelX();
		ChbMonasebat = new CheckBoxX();
		ChbFromdate = new CheckBox();
		ChbHoliday = new CheckBoxX();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		DGSTRasmi = new DataGridViewCheckBoxColumn();
		DGDateSh = new DataGridViewTextBoxColumn();
		DGDay = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		DGDateM = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)holidayBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnUpdateTaghvim);
		((Control)PanelEx1).Controls.Add((Control)(object)LblCountHol);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX4);
		((Control)PanelEx1).Controls.Add((Control)(object)ucTatili1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(962, 439);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnUpdateTaghvim).AccessibleRole = (AccessibleRole)43;
		BtnUpdateTaghvim.ColorTable = (eButtonColor)6;
		((Control)BtnUpdateTaghvim).Font = new Font("Tahoma", 7.5f, (FontStyle)1);
		BtnUpdateTaghvim.Image = (Image)(object)Resources.table_refresh;
		BtnUpdateTaghvim.ImageFixedSize = new Size(30, 30);
		BtnUpdateTaghvim.ImagePosition = (eImagePosition)1;
		((Control)BtnUpdateTaghvim).Location = new Point(3, 106);
		((Control)BtnUpdateTaghvim).Name = "BtnUpdateTaghvim";
		((Control)BtnUpdateTaghvim).Size = new Size(128, 33);
		((PopupItemControl)BtnUpdateTaghvim).Style = (eDotNetBarStyle)9;
		((Control)BtnUpdateTaghvim).TabIndex = 22;
		((Control)BtnUpdateTaghvim).Text = " [بروزرسانی تقویم]";
		BtnUpdateTaghvim.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnUpdateTaghvim).Click += BtnUpdateTaghvim_Click;
		((Control)LblCountHol).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblCountHol).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCountHol).ForeColor = Color.DarkRed;
		((Control)LblCountHol).Location = new Point(485, 411);
		((Control)LblCountHol).Name = "LblCountHol";
		((Control)LblCountHol).Size = new Size(124, 23);
		((Control)LblCountHol).TabIndex = 13;
		((Control)LblCountHol).Text = "labelX3";
		((Control)labelX4).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(615, 411);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(101, 23);
		((Control)labelX4).TabIndex = 12;
		((Control)labelX4).Text = "[تعداد کل تعطیلات] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)ucTatili1).Anchor = (AnchorStyles)6;
		((Control)ucTatili1).BackColor = Color.Transparent;
		ucTatili1.GhRasmi = Color.Black;
		((Control)ucTatili1).Location = new Point(3, 330);
		((Control)ucTatili1).Name = "ucTatili1";
		ucTatili1.Rasmi = Color.Red;
		((Control)ucTatili1).Size = new Size(128, 75);
		((Control)ucTatili1).TabIndex = 9;
		ucTatili1.ColorTatiliChanedEvent += ucTatili1_ColorTatiliChanedEvent;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(3, 223);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(128, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 7;
		((Control)BtnPrint).Text = "[چاپ](Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(738, 412);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(124, 23);
		((Control)CountRows).TabIndex = 11;
		((Control)CountRows).Text = "labelX3";
		((Control)LblRowCount).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Location = new Point(868, 411);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(91, 23);
		((Control)LblRowCount).TabIndex = 10;
		((Control)LblRowCount).Text = "[تعداد کل روزها] :";
		LblRowCount.TextAlignment = (StringAlignment)2;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(3, 262);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(128, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 8;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Visible = false;
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(3, 184);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(128, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 6;
		((Control)BtnEdit).Text = "    [ویرایش]";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(3, 145);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(128, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 5;
		((Control)BtnAdd).Text = "[اضافه] (Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)DGSTRasmi,
			(DataGridViewColumn)DGDateSh,
			(DataGridViewColumn)DGDay,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)DGDateM
		});
		((DataGridView)DGrid).DataSource = holidayBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(137, 105);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(817, 300);
		((Control)DGrid).TabIndex = 4;
		((DataGridView)DGrid).Sorted += DGrid_Sorted;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		holidayBindingSource.DataSource = typeof(Holiday);
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtDate2);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtDate1);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX3);
		((Control)GroupPanel1).Controls.Add((Control)(object)ChbMonasebat);
		((Control)GroupPanel1).Controls.Add((Control)(object)ChbFromdate);
		((Control)GroupPanel1).Controls.Add((Control)(object)ChbHoliday);
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(951, 93);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		TxtDate2.AllowNull = false;
		((Control)TxtDate2).Anchor = (AnchorStyles)9;
		TxtDate2.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtDate2).Border.Class = "TextBoxBorder";
		((TextBoxX)TxtDate2).Border.CornerType = (eCornerType)1;
		TxtDate2.EnterToTab = true;
		TxtDate2.EscToClose = true;
		TxtDate2.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtDate2.FocusBackColor = Color.Yellow;
		TxtDate2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtDate2.FocusForeColor = Color.Blue;
		TxtDate2.FocusTextSelect = true;
		((Control)TxtDate2).Font = new Font("Times New Roman", 12f);
		((Control)TxtDate2).Location = new Point(597, 34);
		TxtDate2.MessageEmptyShowDialog = false;
		TxtDate2.MessegeEmpty = "";
		TxtDate2.MessegeEmptyInFormRight = true;
		TxtDate2.MessegeEmptyShowInForm = false;
		TxtDate2.MiladiDate = new DateTime(2025, 7, 27, 3, 58, 33, 152);
		((Control)TxtDate2).Name = "TxtDate2";
		((Control)TxtDate2).RightToLeft = (RightToLeft)0;
		TxtDate2.ShowNameOfDayOnToolTip = false;
		TxtDate2.ShowToolTipText = false;
		((Control)TxtDate2).Size = new Size(100, 26);
		((Control)TxtDate2).TabIndex = 21;
		((Control)TxtDate2).Text = "26/07/2025";
		((TextBox)TxtDate2).TextAlign = (HorizontalAlignment)2;
		TxtDate2.TootTipDelay = 500;
		TxtDate1.AllowNull = false;
		((Control)TxtDate1).Anchor = (AnchorStyles)9;
		TxtDate1.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtDate1).Border.Class = "TextBoxBorder";
		((TextBoxX)TxtDate1).Border.CornerType = (eCornerType)1;
		TxtDate1.EnterToTab = true;
		TxtDate1.EscToClose = true;
		TxtDate1.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtDate1.FocusBackColor = Color.Yellow;
		TxtDate1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtDate1.FocusForeColor = Color.Blue;
		TxtDate1.FocusTextSelect = true;
		((Control)TxtDate1).Font = new Font("Times New Roman", 12f);
		((Control)TxtDate1).Location = new Point(771, 34);
		TxtDate1.MessageEmptyShowDialog = false;
		TxtDate1.MessegeEmpty = "";
		TxtDate1.MessegeEmptyInFormRight = true;
		TxtDate1.MessegeEmptyShowInForm = false;
		TxtDate1.MiladiDate = new DateTime(2025, 7, 27, 3, 58, 33, 152);
		((Control)TxtDate1).Name = "TxtDate1";
		((Control)TxtDate1).RightToLeft = (RightToLeft)0;
		TxtDate1.ShowNameOfDayOnToolTip = false;
		TxtDate1.ShowToolTipText = false;
		((Control)TxtDate1).Size = new Size(100, 26);
		((Control)TxtDate1).TabIndex = 20;
		((Control)TxtDate1).Text = "26/07/2025";
		((TextBox)TxtDate1).TextAlign = (HorizontalAlignment)2;
		TxtDate1.TootTipDelay = 500;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(703, 34);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(51, 21);
		((Control)labelX3).TabIndex = 19;
		((Control)labelX3).Text = "[ToDate] :";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)ChbMonasebat).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbMonasebat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbMonasebat).Location = new Point(421, 34);
		((Control)ChbMonasebat).Name = "ChbMonasebat";
		((Control)ChbMonasebat).Size = new Size(81, 21);
		((BaseItemControl)ChbMonasebat).Style = (eDotNetBarStyle)9;
		((Control)ChbMonasebat).TabIndex = 2;
		((Control)ChbMonasebat).Text = "[مناسبت ها]";
		ChbMonasebat.CheckedChanged += TxtSearch_TextChanged;
		((Control)ChbFromdate).Anchor = (AnchorStyles)9;
		((Control)ChbFromdate).AutoSize = true;
		((Control)ChbFromdate).Location = new Point(862, 38);
		((Control)ChbFromdate).Name = "ChbFromdate";
		((Control)ChbFromdate).Size = new Size(80, 17);
		((Control)ChbFromdate).TabIndex = 15;
		((Control)ChbFromdate).Text = "[از تاریخ] :";
		((ButtonBase)ChbFromdate).UseVisualStyleBackColor = true;
		((Control)ChbHoliday).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbHoliday).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbHoliday).Location = new Point(508, 34);
		((Control)ChbHoliday).Name = "ChbHoliday";
		((Control)ChbHoliday).Size = new Size(67, 21);
		((BaseItemControl)ChbHoliday).Style = (eDotNetBarStyle)9;
		((Control)ChbHoliday).TabIndex = 1;
		((Control)ChbHoliday).Text = "[تعطیلات]";
		ChbHoliday.CheckedChanged += TxtSearch_TextChanged;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(870, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(51, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(863, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).Enter += TxtSearch_Enter;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtSearch_Leave;
		((DataGridViewColumn)DGSTRasmi).DataPropertyName = "StRasmi";
		((DataGridViewColumn)DGSTRasmi).HeaderText = "[وضعیت تعطیل رسمی]";
		((DataGridViewColumn)DGSTRasmi).Name = "DGSTRasmi";
		((DataGridViewBand)DGSTRasmi).ReadOnly = true;
		((DataGridViewBand)DGSTRasmi).Visible = false;
		((DataGridViewColumn)DGDateSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateSh).DataPropertyName = "DateCalture";
		((DataGridViewColumn)DGDateSh).HeaderText = "تاریخ شمسی";
		((DataGridViewColumn)DGDateSh).Name = "DGDateSh";
		((DataGridViewBand)DGDateSh).ReadOnly = true;
		((DataGridViewColumn)DGDateSh).Width = 97;
		((DataGridViewColumn)DGDay).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDay).DataPropertyName = "Day";
		((DataGridViewColumn)DGDay).HeaderText = "[روز]";
		((DataGridViewColumn)DGDay).MinimumWidth = 40;
		((DataGridViewColumn)DGDay).Name = "DGDay";
		((DataGridViewBand)DGDay).ReadOnly = true;
		((DataGridViewColumn)DGDay).Width = 58;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "Monasebat";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "[مناسبت]";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).MinimumWidth = 100;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "[شرح]";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).MinimumWidth = 50;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewColumn)DGDateM).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateM).DataPropertyName = "DateM";
		((DataGridViewColumn)DGDateM).HeaderText = "[تاریخ میلادی]";
		((DataGridViewColumn)DGDateM).Name = "DGDateM";
		((DataGridViewBand)DGDateM).ReadOnly = true;
		((DataGridViewColumn)DGDateM).Width = 98;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "UcHoliday";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(962, 439);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)holidayBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
