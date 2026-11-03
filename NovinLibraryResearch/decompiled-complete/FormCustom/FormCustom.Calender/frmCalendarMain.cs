using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using EntityCache;
using EntityCache.Attendance.Business;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.Attandance;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Properties;
using FormCustom.UC;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Payamak.Entities;

namespace FormCustom.Calender;

public class frmCalendarMain : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _updateFromServerToken;

	private CancellationTokenSource _highLightGridToken;

	private CancellationTokenSource _accessToken;

	private IEnumerable<CalenderBusiness> _listData;

	private IEnumerable<CalenderBusiness> _filteredList;

	private int _rowIndex;

	private IContainer components;

	internal Label lblCount;

	private UcGroupBox ucGroupBox1;

	private UcSearchBox ucSearchBox1;

	private DataGridView DGrid;

	private BindingSource CalendarBindingSource;

	private UcActionPictureBox btnAdd;

	private UcActionPictureBox btnEdit;

	private UcActionPictureBox btnDelete;

	private UcActionPictureBox btnUpdateFromServer;

	private UcGroupBox ucGroupBox2;

	private DateBoxFa txtDate2;

	private DateBoxFa txtDate1;

	internal Label labelX3;

	private CheckBox chbMonasebat;

	private CheckBox chbFromDate;

	private CheckBox chbHoliday;

	private DataGridViewImageColumn dgSendIcon;

	private DataGridViewTextBoxColumn dgDateSh;

	private DataGridViewTextBoxColumn dayDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn monasebatDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dateMDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn dgStatus;

	private DataGridViewTextBoxColumn modifiedDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn contextGuidDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn statusDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn isDisposedDataGridViewCheckBoxColumn;

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

	public string SearchString { get; private set; }

	public CalenderBusiness Current { get; private set; }

	public ICalendarAggregator Aggregator { get; private set; }

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				_listData = await CalenderBusiness.GetAllAsync(base.ContextGuid, null, null, true, token);
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken seToken = CancellationTokenSource.CreateLinkedTokenSource(_searchToken.Token, token).Token;
				Task.Run(() => SearchAsync(seToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 48);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			_filteredList = _listData;
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			List<string> list = SearchString.SplitString();
			if (list != null && list.Count > 0)
			{
				foreach (string item in list)
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					if (!string.IsNullOrEmpty(item))
					{
						_filteredList = _filteredList?.Where((CalenderBusiness x) => x.Monasebat.StringComparisons(item) || x.Description.StringComparisons(item));
					}
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				try
				{
					if (chbFromDate.Checked)
					{
						_filteredList = _filteredList?.Where((CalenderBusiness p) => p.DateM >= txtDate1.MiladiDate.Date && p.DateM <= txtDate2.MiladiDate.Date);
					}
					if (chbHoliday.Checked)
					{
						_filteredList = _filteredList?.Where((CalenderBusiness p) => p.STRasmi);
					}
					if (chbMonasebat.Checked)
					{
						_filteredList = _filteredList?.Where((CalenderBusiness p) => string.Compare(p.Monasebat, "", StringComparison.Ordinal) > 0);
					}
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						CalendarBindingSource.DataSource = _filteredList?.OrderBy((CalenderBusiness q) => q.DateM)?.ToSortableBindingList(token);
						((Control)lblCount).Text = $"تعداد مناسبت ها: {_filteredList?.Count() ?? 0}".GetTranslate();
						_highLightGridToken?.Cancel();
						_highLightGridToken = new CancellationTokenSource(60000);
						CancellationToken mergedToken = CancellationTokenSource.CreateLinkedTokenSource(token, _highLightGridToken.Token).Token;
						Task.Run(() => HighLightGridAsync(mergedToken));
						IEnumerable<CalenderBusiness> filteredList = _filteredList;
						if (filteredList != null && filteredList.Any() && _rowIndex < DGrid.RowCount && _rowIndex > 0)
						{
							DGrid.CurrentCell = DGrid[((DataGridViewBand)dgDateSh).Index, _rowIndex];
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 93);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 96);
		}
	}

	private void SearchDate(KeyEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected I4, but got Unknown
		try
		{
			if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
			{
				string dateSh = DateManagment.DateM.MiladiToCalture();
				int num = e.KeyCode - 112 + 1;
				int yearOfDateSh = Calendar.GetYearOfDateSh(dateSh);
				chbFromDate.Checked = true;
				txtDate1.MiladiDate = (yearOfDateSh + "/" + num + "/01").ShamsiToMiladi();
				txtDate2.MiladiDate = (yearOfDateSh + "/" + num + "/" + Calendar.EndDayOfMonth((short)num, yearOfDateSh)).ShamsiToMiladi();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource(60000);
				CancellationToken token = _searchToken.Token;
				Task.Run(() => SearchAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 117);
		}
	}

	private async Task HighLightGridAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(1000);
			if (token.IsCancellationRequested || IsDisposed || !(_filteredList?.Any() ?? false))
			{
				return;
			}
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
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					for (int i = 0; i < DGrid.RowCount; i++)
					{
						if (IsDisposed)
						{
							break;
						}
						if (token.IsCancellationRequested)
						{
							break;
						}
						bool flag = false;
						DataGridViewCell obj = DGrid[((DataGridViewBand)dgStatus).Index, i];
						if (string.IsNullOrEmpty((obj == null) ? null : obj.Value?.ToString()) || (bool)DGrid[((DataGridViewBand)dgStatus).Index, i].Value)
						{
							((DataGridViewBand)DGrid.Rows[i]).DefaultCellStyle.ForeColor = Color.Red;
						}
						else
						{
							((DataGridViewBand)DGrid.Rows[i]).DefaultCellStyle.ForeColor = Color.Black;
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "HighLightGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 149);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HighLightGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 152);
		}
	}

	private async Task SetAccessAsync(CancellationToken token)
	{
		try
		{
			if ((ClsCache.GetCache(base.ContextGuid)?.Source ?? ENSource.None) != ENSource.Accounting)
			{
				return;
			}
			AccessLevel access = (await ((await CashierBusiness.GetCurrentCashierAsync(base.ContextGuid, token)) ?? throw new ArgumentNullException()).GetUserAccessAsync(token)) ?? throw new ArgumentNullException();
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
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
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					btnAdd.Enabled = access.Calendar.Insert;
					btnEdit.Enabled = access.Calendar.Update;
					btnDelete.Enabled = access.Calendar.Delete;
					btnUpdateFromServer.Enabled = access.Calendar.GetFromServer;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 179);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 182);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				_updateFromServerToken?.Cancel();
				_updateFromServerToken?.Dispose();
				_updateFromServerToken = null;
				_highLightGridToken?.Cancel();
				_highLightGridToken?.Dispose();
				_highLightGridToken = null;
				_accessToken?.Cancel();
				_accessToken?.Dispose();
				_accessToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 214);
		}
	}

	public frmCalendarMain(string contextGuid, Font font, Font regularFont, ICalendarAggregator agg)
		: base(contextGuid)
	{
		frmCalendarMain frmCalendarMain2 = this;
		try
		{
			InitializeComponent();
			DGrid.Initialize(font);
			Aggregator = agg;
			btnAdd.Init("افزودن مناسبت جدید".GetTranslate(), regularFont);
			btnEdit.Init("ویرایش مناسبت جاری".GetTranslate(), regularFont);
			btnDelete.Init("ابطال مناسبت جاری".GetTranslate(), regularFont);
			btnUpdateFromServer.Init("دریافت از سرور".GetTranslate(), regularFont);
			chbFromDate.Checked = true;
			chbHoliday.Checked = false;
			chbMonasebat.Checked = false;
			DateBoxFa dateBoxFa = txtDate1;
			bool enabled = (((Control)txtDate2).Enabled = chbFromDate.Checked);
			((Control)dateBoxFa).Enabled = enabled;
			string text = DateTime.Now.MiladiToCalture();
			((Control)txtDate1).Text = text.Substring(0, 5) + "01/01";
			((Control)txtDate2).Text = text.Substring(0, 5) + "12/29";
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmCalendarMain2.LoadDataAsync(token));
			_accessToken = new CancellationTokenSource(60000);
			CancellationToken accessToken = _accessToken.Token;
			Task.Run(() => frmCalendarMain2.SetAccessAsync(accessToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 247);
		}
	}

	private void CalendarBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			Current = (CalenderBusiness)CalendarBindingSource.Current;
			DataGridView dGrid = DGrid;
			int? obj;
			if (dGrid == null)
			{
				obj = null;
			}
			else
			{
				DataGridViewRow currentRow = dGrid.CurrentRow;
				obj = ((currentRow != null) ? new int?(((DataGridViewBand)currentRow).Index) : null);
			}
			int? num = obj;
			_rowIndex = num.GetValueOrDefault(-1);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalendarBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 257);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)ucSearchBox1).Focus();
				((Control)ucSearchBox1).Text = e.KeyChar.ToString();
				ucSearchBox1.SelectionStart = 9999;
				ucSearchBox1.SelectionLength = 0;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 269);
		}
	}

	private void frmCalendarMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode == 27)
					{
						if (!string.IsNullOrEmpty(((Control)ucSearchBox1).Text))
						{
							((Control)ucSearchBox1).Text = "";
							return;
						}
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else
				{
					e.Handled = true;
					btnEdit.PerformClick();
				}
			}
			else if ((int)keyCode != 45)
			{
				if ((int)keyCode != 46)
				{
					if (keyCode - 112 <= 11)
					{
						SearchDate(e);
					}
				}
				else
				{
					btnDelete.PerformClick();
				}
			}
			else
			{
				btnAdd.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmCalendarMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 308);
		}
	}

	private void ucSearchBox1_OnTextChanged(object sender, EventArgs e)
	{
		try
		{
			SearchString = ((Control)ucSearchBox1).Text;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnTextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 320);
		}
	}

	private void ChbFromdate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			DateBoxFa dateBoxFa = txtDate1;
			bool enabled = (((Control)txtDate2).Enabled = chbFromDate.Checked);
			((Control)dateBoxFa).Enabled = enabled;
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChbFromdate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 333);
		}
	}

	private void TxtDate1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtDate1_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 344);
		}
	}

	private void chbHoliday_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource(60000);
			CancellationToken token = _searchToken.Token;
			Task.Run(() => SearchAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "chbHoliday_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 355);
		}
	}

	private async void btnAdd_OnClick(object sender, EventArgs e)
	{
		try
		{
			if ((int)(await new frmCalendar(base.ContextGuid, new CalenderBusiness(base.ContextGuid), Aggregator).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((Control)DGrid).Focus();
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadDataToken.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnAdd_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 370);
		}
	}

	private async void btnEdit_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (Current != null && (int)(await new frmCalendar(base.ContextGuid, Current, Aggregator).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((Control)DGrid).Focus();
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource(60000);
				CancellationToken token = _loadDataToken.Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnEdit_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 386);
		}
	}

	private async void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		_ = 4;
		try
		{
			if (e.RowIndex == -1 || DGrid.CurrentRow == null || Current == null || Aggregator == null || e.ColumnIndex != ((DataGridViewBand)dgSendIcon).Index || !(Current.CalendarSmsMetaData?.CalendarSmsList?.Any()).GetValueOrDefault())
			{
				return;
			}
			PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(base.ContextGuid);
			if (instance == null)
			{
				await this.ShowErrorAsync("سرویس پیامک یافت نشد");
				return;
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await instance.SmsSent.GetAllByRefrenceGuidAsync(Current.Guid);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await this.ShowErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage);
			}
			else if (returnedSaveFuncInfoWithValue == null || !returnedSaveFuncInfoWithValue.value.Any())
			{
				await this.ShowErrorAsync("داده ای یافت نشد");
			}
			else
			{
				await Aggregator.ShowReportAsync(base.ContextGuid, returnedSaveFuncInfoWithValue.value, (IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 417);
		}
	}

	private async void btnDelete_OnClick(object sender, EventArgs e)
	{
		using (new JaegerMonitor("btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				_ = 1;
				try
				{
					((Control)DGrid).Focus();
					if (Current == null)
					{
						goto end_IL_0080;
					}
					CalenderBusiness calenderBusiness = await CalenderBusiness.GetAsync(base.ContextGuid, Current.Guid, Current.DateM);
					if ((int)MessageBox.Show("آیا برای حذف کردن مطمئن هستید؟".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) != 6)
					{
						goto end_IL_0080;
					}
					calenderBusiness.Status = !calenderBusiness.Status;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await calenderBusiness.SaveAsync(), ReturnedState.None, "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 431);
					goto end_IL_006e;
					end_IL_0080:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 435);
					res.AddReturnedValue(ex, "", "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 436);
					goto end_IL_006e;
				}
				return;
				end_IL_006e:;
			}
			finally
			{
				if (res.HasError)
				{
					await this.ShowReturnedSavedFuncInfoAsync(res, "خطا در حذف".GetTranslate(), "btnDelete_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs");
				}
				else
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource(60000);
					CancellationToken token = _loadDataToken.Token;
					Task.Run(() => LoadDataAsync(token));
				}
			}
		}
	}

	private async void btnUpdateFromServer_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		Waiter waiter = null;
		try
		{
			_ = 2;
			try
			{
				string translate = "تمامی اطلاعات تقویم در بازه انتخاب شده جایگزین خواهد شد . آیا بروزرسانی انجام شود؟".GetTranslate();
				FrmShowErrorMessage frmQuestion = new FrmShowErrorMessage(base.ContextGuid, translate, "بروزرسانی تقویم سال جاری".GetTranslate(), (MessageBoxButtons)4, isAskQuestion: true, LanguageTypes.Farsi, "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs");
				if ((int)((Form)frmQuestion).ShowDialog((IWin32Window)(object)this) != 6)
				{
					goto IL_02f2;
				}
				waiter = new Waiter("درحال دریافت اطلاعات از سرور ...".GetTranslate(), (Form)(object)this, null, freezeForm: true, EnWaiterType.Circle, null, "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs");
				_updateFromServerToken?.Cancel();
				_updateFromServerToken = new CancellationTokenSource();
				CancellationToken token = _updateFromServerToken.Token;
				ReturnedSaveFuncInfoWithValue<List<WebCalender>> returnedSaveFuncInfoWithValue = await WebCalender.GetAllAsync(token);
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 467);
				if (res.HasError)
				{
					return;
				}
				List<WebCalender> value = returnedSaveFuncInfoWithValue.value;
				if (value != null && value.Any())
				{
					List<CalenderBusiness> list = await CalenderMapper.Instance.MapListAsync(base.ContextGuid, value, null, "", token);
					if (chbFromDate.Checked)
					{
						list = list?.Where((CalenderBusiness q) => q.DateM.Date <= txtDate2.MiladiDate.Date && q.DateM.Date >= txtDate1.MiladiDate.Date)?.ToList();
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CalenderBusiness.SaveRangeAsync(base.ContextGuid, list, token), ReturnedState.None, "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 476);
				}
				goto IL_02f2;
				IL_02f2:
				((Component)(object)frmQuestion).Dispose();
				goto end_IL_003a;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 483);
				res.AddReturnedValue(ex, "", "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 484);
				goto end_IL_003a;
			}
			end_IL_003a:;
		}
		finally
		{
			waiter?.Dispose();
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnUpdateFromServer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs");
			}
			else
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken upToken = _loadDataToken.Token;
				Task.Run(() => LoadDataAsync(upToken));
			}
		}
	}

	private void ucSearchBox1_OnKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 40 || (int)e.KeyCode == 13)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ucSearchBox1_OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\frmCalendarMain.cs", 506);
		}
	}

	private void frmCalendarMain_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
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
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Expected O, but got Unknown
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Expected O, but got Unknown
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d44: Expected O, but got Unknown
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Expected O, but got Unknown
		//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Expected O, but got Unknown
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecc: Expected O, but got Unknown
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_1072: Expected O, but got Unknown
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Expected O, but got Unknown
		//IL_1957: Unknown result type (might be due to invalid IL or missing references)
		//IL_1961: Expected O, but got Unknown
		//IL_196e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1978: Expected O, but got Unknown
		//IL_197d: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d2: Expected O, but got Unknown
		//IL_19da: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e4: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmCalendarMain));
		lblCount = new Label();
		ucGroupBox1 = new UcGroupBox();
		ucSearchBox1 = new UcSearchBox();
		DGrid = new DataGridView();
		CalendarBindingSource = new BindingSource(components);
		btnAdd = new UcActionPictureBox();
		btnEdit = new UcActionPictureBox();
		btnDelete = new UcActionPictureBox();
		btnUpdateFromServer = new UcActionPictureBox();
		ucGroupBox2 = new UcGroupBox();
		txtDate2 = new DateBoxFa();
		txtDate1 = new DateBoxFa();
		labelX3 = new Label();
		chbMonasebat = new CheckBox();
		chbFromDate = new CheckBox();
		chbHoliday = new CheckBox();
		dgSendIcon = new DataGridViewImageColumn();
		dgDateSh = new DataGridViewTextBoxColumn();
		dayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		monasebatDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dgStatus = new DataGridViewCheckBoxColumn();
		modifiedDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		isDisposedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)CalendarBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lblCount).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblCount).Cursor = Cursors.Hand;
		((Control)lblCount).Font = new Font("B Yekan", 9.75f);
		((Control)lblCount).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)lblCount).Location = new Point(14, 132);
		((Control)lblCount).Margin = new Padding(4, 0, 4, 0);
		((Control)lblCount).Name = "lblCount";
		((Control)lblCount).Size = new Size(200, 31);
		((Control)lblCount).TabIndex = 176;
		lblCount.TabStop = true;
		((Control)lblCount).Text = "---";
		lblCount.TextAlign = (ContentAlignment)64;
		((Control)ucGroupBox1).Anchor = (AnchorStyles)13;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(8, 98);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(769, 72);
		((Control)ucGroupBox1).TabIndex = 175;
		ucGroupBox1.Title = "        عنوان";
		((Control)ucSearchBox1).Anchor = (AnchorStyles)13;
		((Control)ucSearchBox1).BackColor = Color.Transparent;
		((Control)ucSearchBox1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucSearchBox1.IsSaerchBox = true;
		((Control)ucSearchBox1).Location = new Point(69, 8);
		((Control)ucSearchBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucSearchBox1).Name = "ucSearchBox1";
		((Control)ucSearchBox1).RightToLeft = (RightToLeft)1;
		ucSearchBox1.SelectionLength = 0;
		ucSearchBox1.SelectionStart = 0;
		((Control)ucSearchBox1).Size = new Size(664, 44);
		((Control)ucSearchBox1).TabIndex = 166;
		ucSearchBox1.OnTextChanged += ucSearchBox1_OnTextChanged;
		ucSearchBox1.OnKeyDown += ucSearchBox1_OnKeyDown;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.CellBorderStyle = (DataGridViewCellBorderStyle)4;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = Color.FromArgb(192, 255, 255);
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = Color.DodgerBlue;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		DGrid.ColumnHeadersDefaultCellStyle = val;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[12]
		{
			(DataGridViewColumn)dgSendIcon,
			(DataGridViewColumn)dgDateSh,
			(DataGridViewColumn)dayDataGridViewTextBoxColumn,
			(DataGridViewColumn)monasebatDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)dateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)dgStatus,
			(DataGridViewColumn)modifiedDataGridViewTextBoxColumn,
			(DataGridViewColumn)contextGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDataGridViewCheckBoxColumn,
			(DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn
		});
		DGrid.DataSource = CalendarBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = Color.FromArgb(192, 255, 255);
		val2.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = Color.DodgerBlue;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val2;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(8, 172);
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = Color.FromArgb(192, 255, 255);
		val3.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.WindowText;
		val3.SelectionBackColor = Color.DodgerBlue;
		val3.SelectionForeColor = SystemColors.HighlightText;
		val3.WrapMode = (DataGridViewTriState)1;
		DGrid.RowHeadersDefaultCellStyle = val3;
		DGrid.RowHeadersVisible = false;
		val4.BackColor = Color.FromArgb(192, 255, 255);
		val4.SelectionBackColor = Color.DodgerBlue;
		DGrid.RowsDefaultCellStyle = val4;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(769, 386);
		((Control)DGrid).TabIndex = 165;
		DGrid.CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += btnEdit_OnClick;
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		CalendarBindingSource.DataSource = typeof(CalenderBusiness);
		CalendarBindingSource.CurrentChanged += CalendarBindingSource_CurrentChanged;
		((Control)btnAdd).Anchor = (AnchorStyles)9;
		((Control)btnAdd).BackColor = Color.FromArgb(249, 250, 255);
		btnAdd.DisableColor = Color.FromArgb(199, 205, 255);
		btnAdd.DisableImage = (Image)(object)FormCustom.Properties.Resources.AddEnable;
		btnAdd.EnableColor = Color.FromArgb(65, 81, 219);
		btnAdd.EnableImage = (Image)(object)FormCustom.Properties.Resources.AddHover;
		((Control)btnAdd).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnAdd.HoverColor = Color.FromArgb(30, 46, 75);
		btnAdd.HoverImage = (Image)(object)FormCustom.Properties.Resources.AddHover2;
		((Control)btnAdd).Location = new Point(678, 102);
		((Control)btnAdd).Margin = new Padding(4, 5, 4, 5);
		((Control)btnAdd).Name = "btnAdd";
		((Control)btnAdd).RightToLeft = (RightToLeft)1;
		((Control)btnAdd).Size = new Size(90, 65);
		((Control)btnAdd).TabIndex = 169;
		btnAdd.OnClick += btnAdd_OnClick;
		((Control)btnEdit).Anchor = (AnchorStyles)9;
		((Control)btnEdit).BackColor = Color.FromArgb(249, 250, 255);
		btnEdit.DisableColor = Color.FromArgb(199, 205, 255);
		btnEdit.DisableImage = (Image)(object)FormCustom.Properties.Resources.DescriptionEnable;
		btnEdit.EnableColor = Color.FromArgb(65, 81, 219);
		btnEdit.EnableImage = (Image)(object)FormCustom.Properties.Resources.DescriptionHover;
		((Control)btnEdit).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnEdit.HoverColor = Color.FromArgb(30, 46, 75);
		btnEdit.HoverImage = (Image)(object)FormCustom.Properties.Resources.DescriptionHover2;
		((Control)btnEdit).Location = new Point(576, 102);
		((Control)btnEdit).Margin = new Padding(4, 5, 4, 5);
		((Control)btnEdit).Name = "btnEdit";
		((Control)btnEdit).RightToLeft = (RightToLeft)1;
		((Control)btnEdit).Size = new Size(99, 65);
		((Control)btnEdit).TabIndex = 170;
		btnEdit.OnClick += btnEdit_OnClick;
		((Control)btnDelete).Anchor = (AnchorStyles)9;
		((Control)btnDelete).BackColor = Color.FromArgb(249, 250, 255);
		btnDelete.DisableColor = Color.FromArgb(199, 205, 255);
		btnDelete.DisableImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsEnable;
		btnDelete.EnableColor = Color.FromArgb(65, 81, 219);
		btnDelete.EnableImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsHover;
		((Control)btnDelete).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnDelete.HoverColor = Color.FromArgb(30, 46, 75);
		btnDelete.HoverImage = (Image)(object)FormCustom.Properties.Resources.DeleteAllRowsHover2;
		((Control)btnDelete).Location = new Point(476, 102);
		((Control)btnDelete).Margin = new Padding(4, 5, 4, 5);
		((Control)btnDelete).Name = "btnDelete";
		((Control)btnDelete).RightToLeft = (RightToLeft)1;
		((Control)btnDelete).Size = new Size(92, 62);
		((Control)btnDelete).TabIndex = 171;
		btnDelete.OnClick += btnDelete_OnClick;
		((Control)btnUpdateFromServer).Anchor = (AnchorStyles)9;
		((Control)btnUpdateFromServer).BackColor = Color.FromArgb(249, 250, 255);
		btnUpdateFromServer.DisableColor = Color.FromArgb(199, 205, 255);
		btnUpdateFromServer.DisableImage = (Image)(object)FormCustom.Properties.Resources.GardeshEnable;
		btnUpdateFromServer.EnableColor = Color.FromArgb(65, 81, 219);
		btnUpdateFromServer.EnableImage = (Image)(object)FormCustom.Properties.Resources.GardeshHover;
		((Control)btnUpdateFromServer).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnUpdateFromServer.HoverColor = Color.FromArgb(30, 46, 75);
		btnUpdateFromServer.HoverImage = (Image)(object)FormCustom.Properties.Resources.GardeshHover2;
		((Control)btnUpdateFromServer).Location = new Point(379, 102);
		((Control)btnUpdateFromServer).Margin = new Padding(4, 5, 4, 5);
		((Control)btnUpdateFromServer).Name = "btnUpdateFromServer";
		((Control)btnUpdateFromServer).RightToLeft = (RightToLeft)1;
		((Control)btnUpdateFromServer).Size = new Size(92, 62);
		((Control)btnUpdateFromServer).TabIndex = 171;
		btnUpdateFromServer.OnClick += btnUpdateFromServer_OnClick;
		((Control)ucGroupBox2).Anchor = (AnchorStyles)13;
		((Control)ucGroupBox2).BackColor = Color.Transparent;
		ucGroupBox2.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox2).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox2.HeadHeight = 40;
		ucGroupBox2.IsShowHeader = false;
		((Control)ucGroupBox2).Location = new Point(8, 55);
		((Control)ucGroupBox2).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox2).Name = "ucGroupBox2";
		((Control)ucGroupBox2).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox2).Size = new Size(769, 42);
		((Control)ucGroupBox2).TabIndex = 177;
		ucGroupBox2.Title = "        عنوان";
		txtDate2.AllowNull = false;
		((Control)txtDate2).Anchor = (AnchorStyles)9;
		txtDate2.BackcolorTooltip = Color.Red;
		txtDate2.Border.Class = "TextBoxBorder";
		txtDate2.Border.CornerType = eCornerType.Square;
		txtDate2.EnterToTab = true;
		txtDate2.EscToClose = true;
		txtDate2.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		txtDate2.FocusBackColor = Color.Yellow;
		txtDate2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtDate2.FocusForeColor = Color.Blue;
		txtDate2.FocusTextSelect = true;
		((Control)txtDate2).Font = new Font("Times New Roman", 12f);
		((Control)txtDate2).Location = new Point(390, 63);
		txtDate2.MessageEmptyShowDialog = false;
		txtDate2.MessegeEmpty = "";
		txtDate2.MessegeEmptyInFormRight = true;
		txtDate2.MessegeEmptyShowInForm = false;
		txtDate2.MiladiDate = new DateTime(2024, 5, 5, 22, 20, 12, 524);
		((Control)txtDate2).Name = "txtDate2";
		((Control)txtDate2).RightToLeft = (RightToLeft)0;
		txtDate2.ShowNameOfDayOnToolTip = false;
		txtDate2.ShowToolTipText = false;
		((Control)txtDate2).Size = new Size(100, 26);
		((Control)txtDate2).TabIndex = 183;
		((Control)txtDate2).Text = "5/5/2024 1";
		((TextBox)txtDate2).TextAlign = (HorizontalAlignment)2;
		txtDate2.TootTipDelay = 500;
		((Control)txtDate2).TextChanged += TxtDate1_TextChanged;
		txtDate1.AllowNull = false;
		((Control)txtDate1).Anchor = (AnchorStyles)9;
		txtDate1.BackcolorTooltip = Color.Red;
		txtDate1.Border.Class = "TextBoxBorder";
		txtDate1.Border.CornerType = eCornerType.Square;
		txtDate1.EnterToTab = true;
		txtDate1.EscToClose = true;
		txtDate1.FarsyDateStyle = DateBoxFa.ENFarsyDateStyle.DisplayToolTip;
		txtDate1.FocusBackColor = Color.Yellow;
		txtDate1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		txtDate1.FocusForeColor = Color.Blue;
		txtDate1.FocusTextSelect = true;
		((Control)txtDate1).Font = new Font("Times New Roman", 12f);
		((Control)txtDate1).Location = new Point(581, 63);
		txtDate1.MessageEmptyShowDialog = false;
		txtDate1.MessegeEmpty = "";
		txtDate1.MessegeEmptyInFormRight = true;
		txtDate1.MessegeEmptyShowInForm = false;
		txtDate1.MiladiDate = new DateTime(2024, 5, 5, 22, 20, 12, 526);
		((Control)txtDate1).Name = "txtDate1";
		((Control)txtDate1).RightToLeft = (RightToLeft)0;
		txtDate1.ShowNameOfDayOnToolTip = false;
		txtDate1.ShowToolTipText = false;
		((Control)txtDate1).Size = new Size(100, 26);
		((Control)txtDate1).TabIndex = 182;
		((Control)txtDate1).Text = "5/5/2024 1";
		((TextBox)txtDate1).TextAlign = (HorizontalAlignment)2;
		txtDate1.TootTipDelay = 500;
		((Control)txtDate1).TextChanged += TxtDate1_TextChanged;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((Control)labelX3).BackColor = Color.FromArgb(249, 250, 255);
		((Control)labelX3).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)labelX3).Location = new Point(496, 65);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(72, 21);
		((Control)labelX3).TabIndex = 181;
		((Control)labelX3).Text = "[تا تاریخ]:";
		((Control)chbMonasebat).Anchor = (AnchorStyles)9;
		((Control)chbMonasebat).BackColor = Color.FromArgb(249, 250, 255);
		((Control)chbMonasebat).Cursor = Cursors.Hand;
		((Control)chbMonasebat).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)chbMonasebat).Location = new Point(69, 65);
		((Control)chbMonasebat).Name = "chbMonasebat";
		((Control)chbMonasebat).Size = new Size(165, 25);
		((Control)chbMonasebat).TabIndex = 179;
		((Control)chbMonasebat).Text = "[مشاهده همه مناسبت ها]";
		((ButtonBase)chbMonasebat).UseVisualStyleBackColor = false;
		chbMonasebat.CheckedChanged += chbHoliday_CheckedChanged;
		((Control)chbFromDate).Anchor = (AnchorStyles)9;
		((Control)chbFromDate).AutoSize = true;
		((Control)chbFromDate).BackColor = Color.FromArgb(249, 250, 255);
		((Control)chbFromDate).Cursor = Cursors.Hand;
		((Control)chbFromDate).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)chbFromDate).Location = new Point(687, 66);
		((Control)chbFromDate).Name = "chbFromDate";
		((Control)chbFromDate).Size = new Size(79, 24);
		((Control)chbFromDate).TabIndex = 180;
		((Control)chbFromDate).Text = "[از تاریخ]:";
		((ButtonBase)chbFromDate).UseVisualStyleBackColor = false;
		chbFromDate.CheckedChanged += ChbFromdate_CheckedChanged;
		((Control)chbHoliday).Anchor = (AnchorStyles)9;
		((Control)chbHoliday).BackColor = Color.FromArgb(249, 250, 255);
		((Control)chbHoliday).Cursor = Cursors.Hand;
		((Control)chbHoliday).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)chbHoliday).Location = new Point(246, 63);
		((Control)chbHoliday).Name = "chbHoliday";
		((Control)chbHoliday).Size = new Size(122, 26);
		((Control)chbHoliday).TabIndex = 178;
		((Control)chbHoliday).Text = "[مشاهده تعطیلات]";
		((ButtonBase)chbHoliday).UseVisualStyleBackColor = false;
		chbHoliday.CheckedChanged += chbHoliday_CheckedChanged;
		((DataGridViewColumn)dgSendIcon).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgSendIcon).DataPropertyName = "IsSendSms";
		((DataGridViewColumn)dgSendIcon).HeaderText = "";
		dgSendIcon.ImageLayout = (DataGridViewImageCellLayout)3;
		((DataGridViewColumn)dgSendIcon).Name = "dgSendIcon";
		((DataGridViewBand)dgSendIcon).ReadOnly = true;
		((DataGridViewColumn)dgSendIcon).Width = 5;
		((DataGridViewColumn)dgDateSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgDateSh).DataPropertyName = "DateSh";
		((DataGridViewColumn)dgDateSh).HeaderText = "[تاریخ]";
		((DataGridViewColumn)dgDateSh).Name = "dgDateSh";
		((DataGridViewBand)dgDateSh).ReadOnly = true;
		((DataGridViewColumn)dgDateSh).Width = 68;
		((DataGridViewColumn)dayDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dayDataGridViewTextBoxColumn).DataPropertyName = "Day";
		((DataGridViewColumn)dayDataGridViewTextBoxColumn).HeaderText = "[روز]";
		((DataGridViewColumn)dayDataGridViewTextBoxColumn).Name = "dayDataGridViewTextBoxColumn";
		((DataGridViewBand)dayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dayDataGridViewTextBoxColumn).Width = 62;
		((DataGridViewColumn)monasebatDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)monasebatDataGridViewTextBoxColumn).DataPropertyName = "Monasebat";
		((DataGridViewColumn)monasebatDataGridViewTextBoxColumn).HeaderText = "[مناسبت]";
		((DataGridViewColumn)monasebatDataGridViewTextBoxColumn).Name = "monasebatDataGridViewTextBoxColumn";
		((DataGridViewColumn)monasebatDataGridViewTextBoxColumn).Width = 80;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).DataPropertyName = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).HeaderText = "DateM";
		((DataGridViewColumn)dateMDataGridViewTextBoxColumn).Name = "dateMDataGridViewTextBoxColumn";
		((DataGridViewBand)dateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dgStatus).DataPropertyName = "STRasmi";
		((DataGridViewColumn)dgStatus).HeaderText = "STRasmi";
		((DataGridViewColumn)dgStatus).Name = "dgStatus";
		((DataGridViewBand)dgStatus).Visible = false;
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).DataPropertyName = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).HeaderText = "Modified";
		((DataGridViewColumn)modifiedDataGridViewTextBoxColumn).Name = "modifiedDataGridViewTextBoxColumn";
		((DataGridViewBand)modifiedDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).DataPropertyName = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).HeaderText = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).Name = "contextGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).DataPropertyName = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).HeaderText = "Status";
		((DataGridViewColumn)statusDataGridViewCheckBoxColumn).Name = "statusDataGridViewCheckBoxColumn";
		((DataGridViewBand)statusDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).DataPropertyName = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).HeaderText = "IsDisposed";
		((DataGridViewColumn)isDisposedDataGridViewCheckBoxColumn).Name = "isDisposedDataGridViewCheckBoxColumn";
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)isDisposedDataGridViewCheckBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)txtDate2);
		((Control)this).Controls.Add((Control)(object)txtDate1);
		((Control)this).Controls.Add((Control)(object)labelX3);
		((Control)this).Controls.Add((Control)(object)chbMonasebat);
		((Control)this).Controls.Add((Control)(object)chbFromDate);
		((Control)this).Controls.Add((Control)(object)chbHoliday);
		((Control)this).Controls.Add((Control)(object)ucGroupBox2);
		((Control)this).Controls.Add((Control)(object)lblCount);
		((Control)this).Controls.Add((Control)(object)btnUpdateFromServer);
		((Control)this).Controls.Add((Control)(object)btnDelete);
		((Control)this).Controls.Add((Control)(object)btnEdit);
		((Control)this).Controls.Add((Control)(object)btnAdd);
		((Control)this).Controls.Add((Control)(object)ucSearchBox1);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmCalendarMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تقویم]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(frmCalendarMain_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmCalendarMain_KeyDown);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)CalendarBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
