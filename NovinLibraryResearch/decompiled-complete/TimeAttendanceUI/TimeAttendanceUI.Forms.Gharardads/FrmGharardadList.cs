using System;
using System.Collections;
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
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Reports;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmGharardadList : BaseForm
{
	private TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private List<GharardadStatus> _gharardadStatuses;

	private readonly List<int> _lstPersonIds = new List<int>();

	private readonly DateTime _startDate = new DateTime(2000, 1, 1, 0, 0, 0);

	private readonly DateTime _endDate = new DateTime(2030, 1, 1, 0, 0, 0);

	private bool ShiftProgrammableOnly;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private List<GharardadNew> _listData;

	private List<TimeAttendanceDAL.Dal.FLRPerson.Person> _listPersons;

	private List<GharardadNew> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private Font _font;

	private IContainer components;

	internal PanelEx PanelEx1;

	private LabelX CountRows;

	private LabelX labelX2;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal ButtonX BtnprintKol;

	internal ButtonX BtnPrint;

	private BindingSource gharardadBindingSource;

	protected ButtonX BtnSelect;

	internal ButtonX BtnCopyGharardads;

	public ContextMenuStrip contextMenuStrip1;

	public ToolStripMenuItem ToolItemSelect;

	public ToolStripMenuItem ToolItemModify;

	public ToolStripMenuItem ToolItemAdd;

	public ToolStripMenuItem ToolItemDelete;

	public ToolStripMenuItem ToolItemCancelSearch;

	private ToolStripMenuItem ToolItemSelectAll;

	private ToolStripMenuItem SelectAllTSMI;

	private ToolStripMenuItem UpSelectAllTSMI;

	private ToolStripMenuItem ReverseSelectAllDetTSMI;

	internal ButtonX BtnCopyGharardadsFromSample;

	private FlowLayoutPanel flowLayoutPanel1;

	private LabelX labelX3;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGPersonID;

	private DataGridViewCheckBoxColumn IsChecked;

	private DataGridViewTextBoxColumn FullName;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn DateStartSHDGrid;

	private DataGridViewTextBoxColumn DateFinishSHDGrid;

	private DataGridViewTextBoxColumn SalaryType;

	private DataGridViewTextBoxColumn InsuranceOption;

	private DataGridViewTextBoxColumn ShiftName;

	private DataGridViewTextBoxColumn gharardadStatusDataGridViewTextBoxColumn;

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

	public List<GharardadNew> ListGharardadSelected => _listForDisplay.Where((GharardadNew p) => p.IsChecked).ToList();

	public GharardadNew SelectedGharardad { get; private set; }

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
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[انتخاب قراردادها]", out var value))
			{
				trDictionary.Add("[انتخاب قراردادها]", Utilities.GetTranslate("[انتخاب قراردادها]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب]", out value))
			{
				trDictionary.Add("[انتخاب]", Utilities.GetTranslate("[انتخاب]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش]", out value))
			{
				trDictionary.Add("[ویرایش]", Utilities.GetTranslate("[ویرایش]"));
			}
			if (!trDictionary.TryGetValue("[اضافه]", out value))
			{
				trDictionary.Add("[اضافه]", Utilities.GetTranslate("[اضافه]"));
			}
			if (!trDictionary.TryGetValue("[حذف]", out value))
			{
				trDictionary.Add("[حذف]", Utilities.GetTranslate("[حذف]"));
			}
			if (!trDictionary.TryGetValue("[لغو جستجو]", out value))
			{
				trDictionary.Add("[لغو جستجو]", Utilities.GetTranslate("[لغو جستجو]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب همه]", out value))
			{
				trDictionary.Add("[انتخاب همه]", Utilities.GetTranslate("[انتخاب همه]"));
			}
			if (!trDictionary.TryGetValue("[پاک کردن همه]", out value))
			{
				trDictionary.Add("[پاک کردن همه]", Utilities.GetTranslate("[پاک کردن همه]"));
			}
			if (!trDictionary.TryGetValue("[معکوس کردن انتخاب ها]", out value))
			{
				trDictionary.Add("[معکوس کردن انتخاب ها]", Utilities.GetTranslate("[معکوس کردن انتخاب ها]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 88);
		}
	}

	public FrmGharardadList(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_font = font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmGharardadList(string contextGuid, Font font, List<int> pIDs, DateTime date1, DateTime date2, bool isSelect = false)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_font = font;
			_lstPersonIds = pIDs;
			_startDate = date1;
			_endDate = date2;
			((Control)BtnSelect).Visible = isSelect;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmGharardadList(string contextGuid, Font font, bool isSelect, bool pShiftProgrammableOnly = false)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_font = font;
			ShiftProgrammableOnly = pShiftProgrammableOnly;
			((Control)BtnSelect).Visible = isSelect;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 159);
		}
	}

	public FrmGharardadList(string contextGuid, Font font, List<int> pIDs, DateTime date1, DateTime date2, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_checkout = checkout;
			InitializeComponent();
			_font = font;
			_lstPersonIds = pIDs;
			_startDate = date1;
			_endDate = date2;
			((Control)BtnSelect).Visible = true;
			((Control)BtnSelect).Text = "انتخاب قراردادها\n\n\n (Enter)";
			if (trDictionary.TryGetValue("[انتخاب قراردادها]", out var value))
			{
				((Control)BtnSelect).Text = value + "\n\n\n (Enter)";
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 181);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SetTrDictionary();
			_gharardadStatuses = await GharardadStatus.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
			_listData = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, _lstPersonIds, _startDate, _endDate, token);
			if (!token.IsCancellationRequested)
			{
				InitCmsItems();
				LoadFilterItems();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token2 = _searchToken.Token;
				await SearchAsync(token2);
				((Control)TxtSearch).Focus();
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

	protected override void OnLoad(EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain)
		{
			((Form)this).Close();
			return;
		}
		try
		{
			((Control)BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadInsert;
			((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadDelete;
			((Control)BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadUpdate;
			((Control)BtnPrint).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadPrint;
			((Control)BtnprintKol).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadPrintKol;
			((BaseForm)this).OnLoad(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 223);
		}
	}

	private void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 238);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			GharardadNew.LastUpdateTime = DateTime.Now.AddMinutes(-2.0);
			if (_listData == null)
			{
				_listData = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, _lstPersonIds, _startDate, _endDate, token);
			}
			_listForDisplay = null;
			List<GharardadNew> list = FilteredList(_listData);
			if (string.IsNullOrEmpty(_searchString))
			{
				_listForDisplay = list;
			}
			else
			{
				List<string> list2 = Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:");
				List<GharardadNew> list3 = new List<GharardadNew>();
				foreach (string item2 in list2)
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					foreach (GharardadNew item3 in list)
					{
						if (item3.FullName.Contains(item2))
						{
							list3.Add(item3);
						}
					}
				}
				_listForDisplay = list3;
			}
			int count = _listForDisplay.Count;
			List<Shifts> list4 = await Shifts.SelectAllAsync(((CacheBaseForm)this).ContextGuid, st: true, (PackageEnumType)20, "", token);
			if (list4 != null)
			{
				foreach (GharardadNew item in _listForDisplay)
				{
					Shifts shifts = list4.Where((Shifts p) => p.Id == item.ShiftId).FirstOrDefault();
					if (shifts != null)
					{
						item.ShiftName = shifts.Name;
					}
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
						gharardadBindingSource.DataSource = Utilities.ToSortableBindingList<GharardadNew>((IEnumerable<GharardadNew>)_listForDisplay, token);
						((Control)CountRows).Text = count.ToString("N0");
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

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)BtnAdd).Enabled || !((Control)BtnAdd).Visible)
		{
			return;
		}
		try
		{
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			Font font = _font;
			FrmGharardad frm = new FrmGharardad(contextGuid, font, await GharardadNew.FactoryAsync(((CacheBaseForm)this).ContextGuid));
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_listData = null;
				Search();
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 322);
		}
	}

	private void InitCmsItems()
	{
		try
		{
			if (trDictionary.TryGetValue("[انتخاب]", out var value))
			{
				((ToolStripItem)ToolItemSelect).Text = value;
			}
			if (trDictionary.TryGetValue("[ویرایش]", out var value2))
			{
				((ToolStripItem)ToolItemModify).Text = value2;
			}
			if (trDictionary.TryGetValue("[اضافه]", out var value3))
			{
				((ToolStripItem)ToolItemAdd).Text = value3;
			}
			if (trDictionary.TryGetValue("[حذف]", out var value4))
			{
				((ToolStripItem)ToolItemDelete).Text = value4;
			}
			if (trDictionary.TryGetValue("[لغو جستجو]", out var value5))
			{
				((ToolStripItem)ToolItemCancelSearch).Text = value5;
			}
			((ToolStripItem)ToolItemSelectAll).Text = "انتخاب چندتایی";
			if (trDictionary.TryGetValue("[انتخاب همه]", out var value6))
			{
				((ToolStripItem)SelectAllTSMI).Text = value6;
			}
			if (trDictionary.TryGetValue("[پاک کردن همه]", out var value7))
			{
				((ToolStripItem)UpSelectAllTSMI).Text = value7;
			}
			if (trDictionary.TryGetValue("[معکوس کردن انتخاب ها]", out var value8))
			{
				((ToolStripItem)ReverseSelectAllDetTSMI).Text = value8;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitCmsItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 358);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (((Control)BtnEdit).Enabled && ((Control)BtnEdit).Visible && SelectedGharardad != null)
			{
				FrmGharardad frm = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, SelectedGharardad);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_listData = null;
					Search();
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 377);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnDelete).Enabled && ((Control)BtnDelete).Visible)
			{
				((Control)DGrid).Focus();
				if (SelectedGharardad != null && (int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف قرارداد انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6 && await SelectedGharardad.DeleteAsync())
				{
					_listData = null;
					Search();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 396);
		}
	}

	private async void BtnprintKol_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnprintKol).Enabled || !((Control)BtnprintKol).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			List<object> list = new List<object>();
			if (_listForDisplay != null)
			{
				foreach (GharardadNew item in _listForDisplay)
				{
					list.Add(new
					{
						DateFinishM = Calendar.MiladiToCalture(item.DateFinishM),
						DateCalture = Calendar.MiladiToCalture(item.DateM),
						DateStartM = Calendar.MiladiToCalture(item.DateStartM),
						Description = item.Description,
						FullName = item.FullName,
						No = item.No,
						Salary = item.SalaryPrice,
						SalaryType = item.SalaryType,
						SumMazaya = item.SumMazaya,
						SumKosorat = item.SumKosorat,
						SumHoghMazaya = item.SumHoghMazaya,
						GharardadStatus = GetNameOfGharardadStatus(item.GharardadStatus)
					});
				}
			}
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)9, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnprintKol_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 434);
		}
	}

	private string GetNameOfGharardadStatus(short id)
	{
		try
		{
			if (_gharardadStatuses != null)
			{
				return _gharardadStatuses.FirstOrDefault((GharardadStatus p) => p.Id == id)?.Name;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetNameOfGharardadStatus", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 445);
		}
		return "";
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!((Control)BtnprintKol).Enabled || !((Control)BtnprintKol).Visible || SelectedGharardad == null)
			{
				return;
			}
			((Control)DGrid).Focus();
			List<GharardadDetNew> list = SelectedGharardad.ListGharardadDet.ToList();
			List<object> i = new List<object>();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			foreach (GharardadDetNew item in list)
			{
				List<object> list2 = i;
				string name = item.Name;
				decimal account = item.Account;
				string desc = item.Desc;
				int hesabId = item.HesabId;
				bool insurance = await item.GetInsuranceAsync(token);
				bool isDecrease = item.IsDecrease;
				int privilege = item.Privilege;
				list2.Add(new
				{
					Name = name,
					Account = account,
					Desc = desc,
					HesabId = hesabId,
					Insurance = insurance,
					IsDecrease = isDecrease,
					Privilege = privilege,
					Taxable = await item.GetTaxableAsync(token),
					TypeAccount = item.TypeAccount,
					nValue = item.nValue
				});
			}
			List<object> lst = new List<object>
			{
				new
				{
					MonthlySalary = SelectedGharardad.SalaryOneMonth,
					SumMazaya = SelectedGharardad.SumMazaya,
					SumKosorat = SelectedGharardad.SumKosorat
				}
			};
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)10, i, null, SelectedGharardad.ID, "", lst);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 484);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		Search();
	}

	private async void FrmGhararMain_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 40)
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
						return;
					}
					e.Handled = true;
					if (((Control)DGrid).Focused)
					{
						if (((Control)BtnSelect).Visible)
						{
							((PopupItemControl)BtnSelect).PerformClick();
						}
						else
						{
							((PopupItemControl)BtnEdit).PerformClick();
						}
					}
					else
					{
						((Control)DGrid).Focus();
					}
				}
				else if ((int)keyCode != 32)
				{
					if ((int)keyCode == 40 && ((Control)TxtSearch).Focused)
					{
						e.Handled = true;
						((Control)DGrid).Focus();
					}
				}
				else
				{
					e.Handled = true;
					if (((Control)DGrid).Focused && SelectedGharardad != null && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)IsChecked).Index)
					{
						SelectedGharardad.IsChecked = !SelectedGharardad.IsChecked;
						((Control)DGrid).Refresh();
					}
				}
			}
			else if ((int)keyCode <= 46)
			{
				if ((int)keyCode != 45)
				{
					if ((int)keyCode == 46)
					{
						e.Handled = true;
						((PopupItemControl)BtnDelete).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((PopupItemControl)BtnAdd).PerformClick();
				}
			}
			else if ((int)keyCode != 80)
			{
				if ((int)keyCode != 118)
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
					((PopupItemControl)BtnEdit).PerformClick();
				}
			}
			else if (e.Control)
			{
				((PopupItemControl)BtnPrint).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmGhararMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 559);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSelect).Visible)
			{
				((PopupItemControl)BtnSelect).PerformClick();
			}
			else
			{
				((PopupItemControl)BtnEdit).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 571);
		}
	}

	private void BtnSelect_Click(object sender, EventArgs e)
	{
		int num = 0;
		try
		{
			List<int> list = (from p in _listForDisplay
				where p.IsChecked
				select p.ID).ToList();
			num = 1;
			if (_checkout != null)
			{
				num = 2;
				_checkout.ContractIds = list;
				num = 3;
				_checkout.TamdidContractIds = list;
				num = 4;
			}
			num = 5;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $"line = {num}", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 595);
		}
	}

	private async void BtnCopyGharardads_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new FrmCopyGharardads(((CacheBaseForm)this).ContextGuid, ListGharardadSelected)).ShowAsync((IWin32Window)(object)this);
			_listData = null;
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCopyGharardads_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 607);
		}
	}

	private void ToolItemCancelSearch_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).Text = "";
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolItemCancelSearch_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 619);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (SelectedGharardad != null && ((DataGridView)DGrid).CurrentCell is DataGridViewCheckBoxCell)
			{
				SelectedGharardad.IsChecked = !SelectedGharardad.IsChecked;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 633);
		}
	}

	private void SelectAllTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (GharardadNew item in _listForDisplay)
			{
				item.IsChecked = true;
			}
			((Control)DGrid).Refresh();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectAllTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 656);
		}
	}

	private void UnSelectAllTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (GharardadNew item in _listForDisplay)
			{
				item.IsChecked = false;
			}
			((Control)DGrid).Refresh();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UnSelectAllTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 677);
		}
	}

	private void ReverseSelectAllDetTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (GharardadNew item in _listForDisplay)
			{
				item.IsChecked = !item.IsChecked;
			}
			((Control)DGrid).Refresh();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReverseSelectAllDetTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 698);
		}
	}

	private async void BtnCopyGharardadsFromSample_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, (ReportFilterEnumType)26);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCopyGharardadsFromSample_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 710);
		}
	}

	private void LoadFilterItems()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			IEnumerable<string> obj = (from p in _listData?.OrderByDescending((GharardadNew p) => p.DateFinishM)
				select Calendar.MiladiToCalture(p.DateFinishM).Substring(2, 5)).Distinct();
			((Control)flowLayoutPanel1).Controls.Clear();
			foreach (string item in obj)
			{
				CheckBox val = new CheckBox
				{
					Text = item,
					Checked = false
				};
				val.CheckedChanged += ChkYear_CheckedChanged;
				((Control)flowLayoutPanel1).Controls.Add((Control)(object)val);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFilterItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 726);
		}
	}

	private void ChkYear_CheckedChanged(object sender, EventArgs e)
	{
		Search();
	}

	private List<GharardadNew> FilteredList(List<GharardadNew> list)
	{
		List<GharardadNew> list2 = list;
		try
		{
			if (ShiftProgrammableOnly)
			{
				list2 = list2.Where((GharardadNew q) => q.ShiftId.Equals(-1)).ToList();
			}
			List<string> typeNames = (from CheckBox chkBox in (IEnumerable)((Control)flowLayoutPanel1).Controls
				where chkBox.Checked
				select ((Control)chkBox).Text).ToList();
			if (typeNames.Count > 0)
			{
				list2 = list2.Where((GharardadNew p) => typeNames.Contains(Calendar.MiladiToCalture(p.DateFinishM).Substring(2, 5))).ToList();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FilteredList", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmGharardadList.cs", 746);
		}
		return list2;
	}

	private void gharardadBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedGharardad = (GharardadNew)gharardadBindingSource.Current;
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
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
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
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
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Expected O, but got Unknown
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Expected O, but got Unknown
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Expected O, but got Unknown
		//IL_0e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e80: Expected O, but got Unknown
		//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Expected O, but got Unknown
		//IL_1066: Unknown result type (might be due to invalid IL or missing references)
		//IL_1070: Expected O, but got Unknown
		//IL_1621: Unknown result type (might be due to invalid IL or missing references)
		//IL_162b: Expected O, but got Unknown
		//IL_1718: Unknown result type (might be due to invalid IL or missing references)
		//IL_1722: Expected O, but got Unknown
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1819: Expected O, but got Unknown
		//IL_1903: Unknown result type (might be due to invalid IL or missing references)
		//IL_190d: Expected O, but got Unknown
		//IL_1962: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a12: Expected O, but got Unknown
		//IL_1b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8c: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGPersonID = new DataGridViewTextBoxColumn();
		IsChecked = new DataGridViewCheckBoxColumn();
		FullName = new DataGridViewTextBoxColumn();
		No = new DataGridViewTextBoxColumn();
		DateStartSHDGrid = new DataGridViewTextBoxColumn();
		DateFinishSHDGrid = new DataGridViewTextBoxColumn();
		SalaryType = new DataGridViewTextBoxColumn();
		InsuranceOption = new DataGridViewTextBoxColumn();
		ShiftName = new DataGridViewTextBoxColumn();
		gharardadStatusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		ToolItemSelect = new ToolStripMenuItem();
		ToolItemModify = new ToolStripMenuItem();
		ToolItemAdd = new ToolStripMenuItem();
		ToolItemDelete = new ToolStripMenuItem();
		ToolItemCancelSearch = new ToolStripMenuItem();
		ToolItemSelectAll = new ToolStripMenuItem();
		SelectAllTSMI = new ToolStripMenuItem();
		UpSelectAllTSMI = new ToolStripMenuItem();
		ReverseSelectAllDetTSMI = new ToolStripMenuItem();
		gharardadBindingSource = new BindingSource(components);
		BtnCopyGharardads = new ButtonX();
		BtnPrint = new ButtonX();
		BtnprintKol = new ButtonX();
		CountRows = new LabelX();
		labelX2 = new LabelX();
		GroupPanel1 = new GroupPanel();
		labelX3 = new LabelX();
		flowLayoutPanel1 = new FlowLayoutPanel();
		TxtSearch = new TextBoxX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		BtnCopyGharardadsFromSample = new ButtonX();
		BtnSelect = new ButtonX();
		LabelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((ISupportInitialize)gharardadBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnSelect);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCopyGharardads);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnprintKol);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCopyGharardadsFromSample);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(784, 561);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[11]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGPersonID,
			(DataGridViewColumn)IsChecked,
			(DataGridViewColumn)FullName,
			(DataGridViewColumn)No,
			(DataGridViewColumn)DateStartSHDGrid,
			(DataGridViewColumn)DateFinishSHDGrid,
			(DataGridViewColumn)SalaryType,
			(DataGridViewColumn)InsuranceOption,
			(DataGridViewColumn)ShiftName,
			(DataGridViewColumn)gharardadStatusDataGridViewTextBoxColumn
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGrid).DataSource = gharardadBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(150, 109);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(623, 411);
		((Control)DGrid).TabIndex = 1;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((DataGridViewColumn)DGID).DataPropertyName = "ID";
		((DataGridViewColumn)DGID).HeaderText = "ID";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGPersonID).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGPersonID).DataPropertyName = "PersonID";
		((DataGridViewColumn)DGPersonID).HeaderText = "[کد شخص]";
		((DataGridViewColumn)DGPersonID).Name = "DGPersonID";
		((DataGridViewBand)DGPersonID).ReadOnly = true;
		((DataGridViewBand)DGPersonID).Visible = false;
		((DataGridViewColumn)IsChecked).DataPropertyName = "IsChecked";
		((DataGridViewColumn)IsChecked).HeaderText = "";
		((DataGridViewColumn)IsChecked).MinimumWidth = 20;
		((DataGridViewColumn)IsChecked).Name = "IsChecked";
		((DataGridViewBand)IsChecked).ReadOnly = true;
		((DataGridViewColumn)IsChecked).Width = 20;
		((DataGridViewColumn)FullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)FullName).DataPropertyName = "FullName";
		((DataGridViewColumn)FullName).HeaderText = "نام شخص";
		((DataGridViewColumn)FullName).Name = "FullName";
		((DataGridViewBand)FullName).ReadOnly = true;
		((DataGridViewColumn)No).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)No).DataPropertyName = "No";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)No).DefaultCellStyle = val3;
		((DataGridViewColumn)No).HeaderText = "[شماره قرارداد]";
		((DataGridViewColumn)No).Name = "No";
		((DataGridViewBand)No).ReadOnly = true;
		((DataGridViewColumn)No).Width = 102;
		((DataGridViewColumn)DateStartSHDGrid).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateStartSHDGrid).DataPropertyName = "DateStartCalture";
		((DataGridViewColumn)DateStartSHDGrid).HeaderText = "[شروع قرارداد]";
		((DataGridViewColumn)DateStartSHDGrid).MinimumWidth = 60;
		((DataGridViewColumn)DateStartSHDGrid).Name = "DateStartSHDGrid";
		((DataGridViewBand)DateStartSHDGrid).ReadOnly = true;
		((DataGridViewColumn)DateFinishSHDGrid).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateFinishSHDGrid).DataPropertyName = "DateFinishCalture";
		((DataGridViewColumn)DateFinishSHDGrid).HeaderText = "[پایان قرارداد]";
		((DataGridViewColumn)DateFinishSHDGrid).MinimumWidth = 60;
		((DataGridViewColumn)DateFinishSHDGrid).Name = "DateFinishSHDGrid";
		((DataGridViewBand)DateFinishSHDGrid).ReadOnly = true;
		((DataGridViewColumn)DateFinishSHDGrid).Width = 96;
		((DataGridViewColumn)SalaryType).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)SalaryType).DataPropertyName = "SalaryTypeName";
		((DataGridViewColumn)SalaryType).HeaderText = "نوع پرداخت";
		((DataGridViewColumn)SalaryType).Name = "SalaryType";
		((DataGridViewBand)SalaryType).ReadOnly = true;
		((DataGridViewColumn)SalaryType).Width = 84;
		((DataGridViewColumn)InsuranceOption).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)InsuranceOption).DataPropertyName = "InsuranceOptionName";
		((DataGridViewColumn)InsuranceOption).HeaderText = "وضعیت بیمه";
		((DataGridViewColumn)InsuranceOption).Name = "InsuranceOption";
		((DataGridViewBand)InsuranceOption).ReadOnly = true;
		((DataGridViewColumn)InsuranceOption).Width = 86;
		((DataGridViewColumn)ShiftName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)ShiftName).DataPropertyName = "ShiftName";
		((DataGridViewColumn)ShiftName).HeaderText = "شیفت کاری";
		((DataGridViewColumn)ShiftName).Name = "ShiftName";
		((DataGridViewBand)ShiftName).ReadOnly = true;
		((DataGridViewColumn)ShiftName).Width = 87;
		((DataGridViewColumn)gharardadStatusDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)gharardadStatusDataGridViewTextBoxColumn).DataPropertyName = "GharardadStatusName";
		((DataGridViewColumn)gharardadStatusDataGridViewTextBoxColumn).HeaderText = "وضعیت قرارداد";
		((DataGridViewColumn)gharardadStatusDataGridViewTextBoxColumn).Name = "gharardadStatusDataGridViewTextBoxColumn";
		((DataGridViewBand)gharardadStatusDataGridViewTextBoxColumn).ReadOnly = true;
		((Control)contextMenuStrip1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[6]
		{
			(ToolStripItem)ToolItemSelect,
			(ToolStripItem)ToolItemModify,
			(ToolStripItem)ToolItemAdd,
			(ToolStripItem)ToolItemDelete,
			(ToolStripItem)ToolItemCancelSearch,
			(ToolStripItem)ToolItemSelectAll
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(154, 136);
		((ToolStripItem)ToolItemSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ToolStripItem)ToolItemSelect).Name = "ToolItemSelect";
		((ToolStripItem)ToolItemSelect).Size = new Size(153, 22);
		((ToolStripItem)ToolItemSelect).Text = "[انتخاب]";
		((ToolStripItem)ToolItemSelect).Click += BtnSelect_Click;
		((ToolStripItem)ToolItemModify).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemModify).Name = "ToolItemModify";
		((ToolStripItem)ToolItemModify).Size = new Size(153, 22);
		((ToolStripItem)ToolItemModify).Text = "[ویرایش]";
		((ToolStripItem)ToolItemModify).Click += BtnEdit_Click;
		((ToolStripItem)ToolItemAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemAdd).Name = "ToolItemAdd";
		((ToolStripItem)ToolItemAdd).Size = new Size(153, 22);
		((ToolStripItem)ToolItemAdd).Text = "[اضافه]";
		((ToolStripItem)ToolItemAdd).Click += BtnAdd_Click;
		((ToolStripItem)ToolItemDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemDelete).Name = "ToolItemDelete";
		((ToolStripItem)ToolItemDelete).Size = new Size(153, 22);
		((ToolStripItem)ToolItemDelete).Text = "[حذف]";
		((ToolStripItem)ToolItemDelete).Click += BtnDelete_Click;
		((ToolStripItem)ToolItemCancelSearch).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ToolStripItem)ToolItemCancelSearch).Name = "ToolItemCancelSearch";
		((ToolStripItem)ToolItemCancelSearch).Size = new Size(153, 22);
		((ToolStripItem)ToolItemCancelSearch).Text = "[لغو جستجو]";
		((ToolStripItem)ToolItemCancelSearch).Click += ToolItemCancelSearch_Click;
		((ToolStripDropDownItem)ToolItemSelectAll).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)SelectAllTSMI,
			(ToolStripItem)UpSelectAllTSMI,
			(ToolStripItem)ReverseSelectAllDetTSMI
		});
		((ToolStripItem)ToolItemSelectAll).Name = "ToolItemSelectAll";
		((ToolStripItem)ToolItemSelectAll).Size = new Size(153, 22);
		((ToolStripItem)ToolItemSelectAll).Text = "انتخاب چندتایی";
		((ToolStripItem)SelectAllTSMI).Name = "SelectAllTSMI";
		((ToolStripItem)SelectAllTSMI).Size = new Size(208, 22);
		((ToolStripItem)SelectAllTSMI).Text = "[انتخاب همه]";
		((ToolStripItem)SelectAllTSMI).Click += SelectAllTSMI_Click;
		((ToolStripItem)UpSelectAllTSMI).Name = "UpSelectAllTSMI";
		((ToolStripItem)UpSelectAllTSMI).Size = new Size(208, 22);
		((ToolStripItem)UpSelectAllTSMI).Text = "[پاک کردن همه]";
		((ToolStripItem)UpSelectAllTSMI).Click += UnSelectAllTSMI_Click;
		((ToolStripItem)ReverseSelectAllDetTSMI).Name = "ReverseSelectAllDetTSMI";
		((ToolStripItem)ReverseSelectAllDetTSMI).Size = new Size(208, 22);
		((ToolStripItem)ReverseSelectAllDetTSMI).Text = "[معکوس کردن انتخاب ها]";
		((ToolStripItem)ReverseSelectAllDetTSMI).Click += ReverseSelectAllDetTSMI_Click;
		gharardadBindingSource.DataSource = typeof(GharardadNew);
		gharardadBindingSource.CurrentChanged += gharardadBindingSource_CurrentChanged;
		((Control)BtnCopyGharardads).AccessibleRole = (AccessibleRole)43;
		BtnCopyGharardads.ColorTable = (eButtonColor)6;
		((Control)BtnCopyGharardads).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnCopyGharardads.Image = (Image)(object)Resources.copydocs22;
		BtnCopyGharardads.ImagePosition = (eImagePosition)1;
		BtnCopyGharardads.ImageTextSpacing = 2;
		((Control)BtnCopyGharardads).Location = new Point(5, 324);
		((Control)BtnCopyGharardads).Name = "BtnCopyGharardads";
		((Control)BtnCopyGharardads).RightToLeft = (RightToLeft)1;
		((Control)BtnCopyGharardads).Size = new Size(140, 39);
		((PopupItemControl)BtnCopyGharardads).Style = (eDotNetBarStyle)9;
		((Control)BtnCopyGharardads).TabIndex = 9;
		((Control)BtnCopyGharardads).Text = "  [تمدید قرارداد]";
		BtnCopyGharardads.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnCopyGharardads).Click += BtnCopyGharardads_Click;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		BtnPrint.ImageTextSpacing = 2;
		((Control)BtnPrint).Location = new Point(5, 281);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).RightToLeft = (RightToLeft)1;
		((Control)BtnPrint).Size = new Size(140, 39);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 9;
		((Control)BtnPrint).Text = "  [چاپ قرارداد]";
		BtnPrint.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)BtnprintKol).AccessibleRole = (AccessibleRole)43;
		BtnprintKol.ColorTable = (eButtonColor)6;
		((Control)BtnprintKol).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnprintKol.Image = (Image)(object)Resources.printer_green;
		BtnprintKol.ImagePosition = (eImagePosition)1;
		BtnprintKol.ImageTextSpacing = 2;
		((Control)BtnprintKol).Location = new Point(5, 238);
		((Control)BtnprintKol).Name = "BtnprintKol";
		((Control)BtnprintKol).RightToLeft = (RightToLeft)1;
		((Control)BtnprintKol).Size = new Size(140, 39);
		((PopupItemControl)BtnprintKol).Style = (eDotNetBarStyle)9;
		((Control)BtnprintKol).TabIndex = 8;
		((Control)BtnprintKol).Text = "  [چاپ لیست قراردادها]";
		BtnprintKol.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnprintKol).Click += BtnprintKol_Click;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(378, 526);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(293, 23);
		((Control)CountRows).TabIndex = 6;
		((Control)CountRows).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(676, 526);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(98, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[تعداد رکوردها]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX3);
		((Control)GroupPanel1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(5, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(768, 97);
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
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(700, 30);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(65, 23);
		((Control)labelX3).TabIndex = 17;
		((Control)labelX3).Text = "[پایان قرارداد]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)13;
		((ScrollableControl)flowLayoutPanel1).AutoScroll = true;
		((Panel)flowLayoutPanel1).AutoSizeMode = (AutoSizeMode)0;
		((Control)flowLayoutPanel1).BackColor = Color.White;
		((Control)flowLayoutPanel1).Location = new Point(3, 26);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(693, 40);
		((Control)flowLayoutPanel1).TabIndex = 16;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 3);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(756, 20);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		BtnDelete.ImageTextSpacing = 2;
		((Control)BtnDelete).Location = new Point(5, 195);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).RightToLeft = (RightToLeft)1;
		((Control)BtnDelete).Size = new Size(140, 39);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 4;
		((Control)BtnDelete).Text = "  [حذف] (Delete)    ";
		BtnDelete.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		BtnEdit.ImageTextSpacing = 2;
		((Control)BtnEdit).Location = new Point(5, 152);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(140, 39);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 3;
		((Control)BtnEdit).Text = "  [ویرایش](F7)";
		BtnEdit.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		BtnAdd.ImageTextSpacing = 2;
		((Control)BtnAdd).Location = new Point(5, 109);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).RightToLeft = (RightToLeft)1;
		((Control)BtnAdd).Size = new Size(140, 39);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 2;
		((Control)BtnAdd).Text = "  [اضافه](Insert)";
		BtnAdd.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)BtnCopyGharardadsFromSample).AccessibleRole = (AccessibleRole)43;
		BtnCopyGharardadsFromSample.ColorTable = (eButtonColor)6;
		((Control)BtnCopyGharardadsFromSample).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnCopyGharardadsFromSample.Image = (Image)(object)Resources.copydocs22;
		BtnCopyGharardadsFromSample.ImagePosition = (eImagePosition)1;
		BtnCopyGharardadsFromSample.ImageTextSpacing = 2;
		((Control)BtnCopyGharardadsFromSample).Location = new Point(5, 367);
		((Control)BtnCopyGharardadsFromSample).Name = "BtnCopyGharardadsFromSample";
		((Control)BtnCopyGharardadsFromSample).Padding = new Padding(5);
		((Control)BtnCopyGharardadsFromSample).RightToLeft = (RightToLeft)1;
		((Control)BtnCopyGharardadsFromSample).Size = new Size(140, 39);
		((PopupItemControl)BtnCopyGharardadsFromSample).Style = (eDotNetBarStyle)9;
		((Control)BtnCopyGharardadsFromSample).TabIndex = 20;
		((Control)BtnCopyGharardadsFromSample).Text = "  [تمدید قرارداد طبق نمونه] ";
		BtnCopyGharardadsFromSample.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnCopyGharardadsFromSample).Click += BtnCopyGharardadsFromSample_Click;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = (eButtonColor)6;
		((Control)BtnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelect).Location = new Point(5, 109);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(140, 297);
		((Control)BtnSelect).TabIndex = 19;
		((Control)BtnSelect).TabStop = false;
		((Control)BtnSelect).Text = "[انتخاب] \r\n    \r\n    (Enter)";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(663, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(58, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "FrmGharardadList";
		((Control)this).Text = "[قراردادها]*";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FrmGhararMain_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((ISupportInitialize)gharardadBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
