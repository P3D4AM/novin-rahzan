using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Salary;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Enums;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Tax_and_insurace;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Extension;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.View.Salary;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Reports;
using TimeAttendanceUI.Forms.User;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmFishList : BaseForm
{
	private readonly DateTime _startDate = DateTime.Now;

	private readonly DateTime _endDate = DateTime.Now;

	private readonly List<int> _personIDs = new List<int>();

	private List<short> _selecteUsers = new List<short>();

	private BankTemplate _bankTemplate;

	private CancellationTokenSource tokenSourceInitCms = new CancellationTokenSource();

	private CancellationTokenSource _refreshLablesToken = new CancellationTokenSource();

	private Font _font;

	private bool _isDisposed;

	private List<Fish> _listData;

	private SortableBindingList<Fish> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private string _countRecordTr = "تعداد ردیف ";

	private IContainer components;

	private PanelEx panelEx1;

	internal GroupPanel GroupPanel1;

	internal TextBoxX TxtSearch;

	internal DataGridViewX DGrid;

	private ButtonX SodorSanad;

	private ButtonX BtnPrint;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal ButtonX BtnSelect;

	private LabelX labelX3;

	private LabelX LblSumM;

	private LabelX LblSumKh;

	private LabelX LblSumKosor;

	private LabelX LblSumK;

	private LabelX labelX6;

	private LabelX labelX5;

	private LabelX labelX2;

	private BindingSource fishBindingSource;

	private LabelX LblRowCount;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem AddToolStripMenuItem;

	private ToolStripMenuItem EditToolStripMenuItem;

	private ToolStripMenuItem DeleteToolStripMenuItem;

	private ToolStripMenuItem SodorSanadToolStripMenuItem;

	private ToolStripMenuItem chapToolStripMenuItem;

	private ToolStripMenuItem SelectAllToolStripMenuItem;

	private ToolStripMenuItem SelectAllDetTSMI;

	private ToolStripMenuItem UnSelectAllDetTSMI;

	private ToolStripMenuItem ReverseSelectAllDetTSMI;

	private ToolStripMenuItem SelectedUserToolStripMenuItem;

	private ButtonX BtnBankMellatExport;

	private ToolStripMenuItem SabteDaemToolStripMenuItem;

	private ToolStripMenuItem CmsSabteDaem;

	private ToolStripMenuItem CmsSabteMoghat;

	private ToolStripMenuItem CMSPrintFish;

	private ToolStripMenuItem CmsRayVarz;

	private ButtonX BtnRayvarz;

	private DataGridViewTextBoxColumn personIDDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGSumKarkerd;

	private DataGridViewTextBoxColumn DGKhales;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DGId;

	private DataGridViewTextBoxColumn FullName;

	private DataGridViewTextBoxColumn DgFishNo;

	private DataGridViewTextBoxColumn FishMonthYear;

	private DataGridViewTextBoxColumn DGDateSh;

	private DataGridViewTextBoxColumn IdSanad;

	private DataGridViewTextBoxColumn DGDateStartSh;

	private DataGridViewTextBoxColumn DGDateEndSh;

	private DataGridViewTextBoxColumn DGSumMazaya;

	private DataGridViewTextBoxColumn DGSumKosor;

	private DataGridViewTextBoxColumn SumKhales;

	private DataGridViewCheckBoxColumn LockState;

	private ButtonX BtnMaliatHoghogh;

	private ButtonX BtnMaliatKarkonan;

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

	public List<Fish> SelectedFishes => (from gl in _listData
		join fi in ListFishId on gl.Id equals fi
		select gl).ToList();

	public Fish SelectedFish { get; private set; }

	public List<int> ListFishId
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<int> list = new List<int>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
						{
							list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGId).Index, i].Value.ToString()));
						}
					}
				}
				if (list.Count <= 0 && SelectedFish != null)
				{
					list.Add(SelectedFish.Id);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListFishId", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 75);
			}
			return list;
		}
	}

	public FrmFishList(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_font = font;
			((Control)this).Font = font;
			InitCmsItems();
			InitFormItems();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 97);
		}
	}

	public FrmFishList(string contextGuid, List<int> prsIDs, DateTime date1, DateTime date2, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_font = font;
			((Control)this).Font = font;
			_personIDs = prsIDs;
			_startDate = date1;
			_endDate = date2;
			InitCmsItems();
			InitFormItems();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 119);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_countRecordTr = Utilities.GetTranslate("[تعداد رکوردها]");
			_listData = await Fish.GetAllAsync(((CacheBaseForm)this).ContextGuid, _personIDs, _startDate, _endDate, _selecteUsers, loadClsTaradod: false, token);
			if (!token.IsCancellationRequested && _listData != null && _listData.Count != 0)
			{
				_listData = (from p in _listData
					orderby p.FullName, p.FishNo descending
					select p).ToList();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token2 = _searchToken.Token;
				await SearchAsync(token2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 139);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			List<Fish> list = new List<Fish>();
			if (string.IsNullOrEmpty(_searchString))
			{
				list = _listData;
			}
			else
			{
				foreach (string item in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					foreach (Fish listDatum in _listData)
					{
						if (((PersonBase)listDatum.ClsPerson).FullName.Contains(item) || ((PersonBase)listDatum.ClsPerson).CodeTafsil.ToString().Contains(item) || Calendar.MiladiToCalture(listDatum.DateStartM).Contains(item) || Calendar.MiladiToCalture(listDatum.DateM).Contains(item) || listDatum.FishNo.ToString() == item)
						{
							list.Add(listDatum);
						}
					}
				}
			}
			_listForDisplay = Utilities.ToSortableBindingList<Fish>(list.Distinct(), default(CancellationToken));
			_ = ((Collection<Fish>)(object)_listForDisplay).Count;
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
						fishBindingSource.DataSource = _listForDisplay;
					}
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
			await RefreshLblAsync(token);
			await RefreshRowsColorAsync(token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void InitCmsItems()
	{
		try
		{
			((ToolStripItem)AddToolStripMenuItem).Text = Utilities.GetTranslate("[اضافه]");
			((ToolStripItem)EditToolStripMenuItem).Text = Utilities.GetTranslate("[ویرایش]");
			((ToolStripItem)DeleteToolStripMenuItem).Text = Utilities.GetTranslate("[حذف]");
			((ToolStripItem)SodorSanadToolStripMenuItem).Text = Utilities.GetTranslate("[صدور سند حسابداری]");
			((ToolStripItem)chapToolStripMenuItem).Text = Utilities.GetTranslate("[چاپ گزارش فیش ها]");
			((ToolStripItem)CMSPrintFish).Text = Utilities.GetTranslate("[چاپ فیش]");
			((ToolStripItem)SelectAllToolStripMenuItem).Text = Utilities.GetTranslate("[انتخاب]");
			((ToolStripItem)SelectAllDetTSMI).Text = Utilities.GetTranslate("[انتخاب همه]");
			((ToolStripItem)UnSelectAllDetTSMI).Text = Utilities.GetTranslate("[پاک کردن همه]");
			((ToolStripItem)ReverseSelectAllDetTSMI).Text = Utilities.GetTranslate("[معکوس کردن انتخاب ها]");
			((ToolStripItem)SabteDaemToolStripMenuItem).Text = Utilities.GetTranslate("[تغییر وضعیت فیش]");
			((ToolStripItem)CmsSabteDaem).Text = Utilities.GetTranslate("[ثبت دائم]");
			((ToolStripItem)CmsSabteMoghat).Text = Utilities.GetTranslate("[ثبت موقت]");
			((ToolStripItem)SelectedUserToolStripMenuItem).Text = Utilities.GetTranslate("[گزارش کاربر خاص]");
			((ToolStripItem)CmsRayVarz).Text = Utilities.GetTranslate("[مشاهده خروجی رایورز]");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitCmsItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 229);
		}
	}

	private void InitFormItems()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		try
		{
			AccessLevelClass accessLevelMerg = UserAttendance.SelectedUser.AccessLevelMerg;
			((Control)BtnAdd).Enabled = accessLevelMerg.BillPrice.BillPriceInsert;
			((Control)BtnDelete).Enabled = accessLevelMerg.BillPrice.BillPriceDelete;
			((Control)BtnEdit).Enabled = accessLevelMerg.BillPrice.BillPriceUpdate;
			((Control)BtnPrint).Enabled = accessLevelMerg.BillPrice.BillPricePrint;
			((Control)SodorSanad).Enabled = accessLevelMerg.Accounting.AccountingMain;
			((ToolStripItem)CmsSabteMoghat).Enabled = accessLevelMerg.BillPrice.changeEditStateOfFish;
			if ((int)AccessLevelClass.Instance.LevelHozor < 5)
			{
				((Control)BtnRayvarz).Enabled = false;
				((ToolStripItem)CmsRayVarz).Enabled = false;
				((Control)BtnBankMellatExport).Enabled = false;
			}
			((Control)DGrid).Font = _font;
			((Control)TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitFormItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 255);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.BillPrice.BillPriceMain)
			{
				((Form)this).Close();
			}
			else
			{
				((BaseForm)this).OnLoad(e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 273);
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		try
		{
			if (((Control)DGrid).Focused && Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = ((Control)TxtSearch).Text.Length;
				((Control)TxtSearch).Select();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 285);
		}
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_searchString = Text.FixString(((Control)TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await Task.Delay(200);
			if (!token.IsCancellationRequested)
			{
				await SearchAsync(token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void FrmPersonHoghogh_KeyDown(object sender, KeyEventArgs e)
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
					}
					else
					{
						e.Handled = true;
						if (((Control)DGrid).Focused)
						{
							((PopupItemControl)BtnEdit).PerformClick();
						}
						else
						{
							((Control)DGrid).Focus();
						}
					}
					return;
				}
				if ((int)keyCode != 32)
				{
					if ((int)keyCode == 40 && ((Control)TxtSearch).Focused)
					{
						e.Handled = true;
						((Control)DGrid).Focus();
					}
					return;
				}
				e.Handled = true;
				if (((Control)DGrid).Focused && SelectedFish != null && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
					DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val != null)
					{
						((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmPersonHoghogh_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 358);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount >= 1)
			{
				((PopupItemControl)BtnEdit).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 367);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && SelectedFish != null)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 379);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible)
			{
				return;
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken searchtkn = _searchToken.Token;
			List<SalaryListInsurance> obj = await SalaryListInsurance.GetAllAsync(((CacheBaseForm)this).ContextGuid, SelectedFishes, searchtkn);
			List<object> lst = new List<object>();
			foreach (SalaryListInsurance item in obj)
			{
				lst.Add(new
				{
					item.FullName, item.FatherName, item.ShSh, item.CodeMeli, item.JobTitle, item.SalaryType, item.SalaryTypeTitle, item.FishNo, item.DateStartSh, item.CodeMoein,
					item.KarkerdMin, item.KarkerdPrice, item.KarkerdSumPrice, item.SumKarkerd, item.SumKolKarkerd, item.SumKhales, item.CountKarKerd, item.CountKarKerdBime, item.CountKarKerdMaliat, item.CountNaghes,
					item.CountTatil, item.CountDays, item.CountShift, item.IncludedInsurance, item.IncludedTax, item.EzafeMin, item.EzafePrice, item.SumEzafeKar, item.KasrMin, item.SumKasrKar,
					item.KasrPrice, item.VacationPrice, item.VacationMin, item.VacationMinUsed, item.SumVacation, item.CountVacationWithHoghogh, item.CountVacationNotHoghogh, item.GheybatRoze, item.GheybatPrice, item.SumGheybat,
					item.CountGheybat, item.MissionRoz, item.MissionPrice, item.SumMission, item.SumJarime, item.ShabMin, item.ShabPrice, item.SumShabKari, item.NobatMin1, item.NobatPrice1,
					item.SumNobatKari1, item.NobatMin2, item.NobatPrice2, item.SumNobatKari2, item.NobatMin3, item.NobatPrice3, item.SumNobatKari3, item.JomeMin, item.JomePrice, item.SumJomeKari,
					item.SumMazaya, item.SumKosorat, item.Grocery, item.Maskan, item.Spouse, item.Reward, item.SeverancePay, item.Compensation, item.Additions1, item.Additions2,
					item.Additions3, item.Tax, item.Insurance, item.AdvanceMoney, item.Loan, item.Fraction1, item.Fraction2, item.Fraction3, item.Fraction4, item.FractionOther,
					item.NetPayable, item.IsPrint, item.JsonFishDet, item.ShabaNo
				});
			}
			CodeingAccounting codeingAccounting = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).CodeingAccounting;
			List<int> ids = new List<int> { codeingAccounting.Additions1, codeingAccounting.Additions2, codeingAccounting.Fraction1, codeingAccounting.Fraction2, codeingAccounting.Fraction3 };
			List<object> xx = SelectedFishes.Select((Fish item) => item.ListFishDetForReport).Cast<object>().ToList();
			string extraVar = await FishHesab.JoinAndGetNamesAsync(((CacheBaseForm)this).ContextGuid, ids, searchtkn);
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)3, lst, null, 0, extraVar, xx);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 500);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceSettings settings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmFilter frm = new FrmFilter(((CacheBaseForm)this).ContextGuid, settings, 0, DateTime.Now, bShowForm: true, bUcDate: false, -1, bUcPerson: true, 2, bUcPersonTak: false, -2, bUcDateTak: true, 1, bUcTakDay: false, -3, bUcDesc: false, -5, (ReportFilterEnumType)21);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 518);
		}
	}

	private async void SodorSanad_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount <= 0)
			{
				return;
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			DialogResult val = await ListFishCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, SelectedFishes, isShowMessage: true, token);
			if (val - 2 > 1)
			{
				if ((int)val != 4 && (int)(await ((CacheBaseForm)(await FrmSanad.CreateAsync(((CacheBaseForm)this).ContextGuid, SelectedFishes))).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "SodorSanad_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
				}
			}
			else
			{
				((Form)this).DialogResult = val;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SodorSanad_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 549);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			FrmFishHoghogh frm = new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, SelectedFish, _font, isReadOnly: false);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((Component)(object)frm).Dispose();
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token = _searchToken.Token;
				new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 564);
		}
	}

	private void BtnSelect_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSelect).Enabled && ((Control)BtnSelect).Visible)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 576);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnDelete).Enabled || !((Control)BtnDelete).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (ListFishId.Count <= 0 || (int)FMessageBox.Show(Utilities.GetTranslate("[فیشهای انتخاب شده حذف شوند؟]"), "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 6)
			{
				return;
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			foreach (int item in ListFishId)
			{
				await DeleteFishAsync(item, token);
			}
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 598);
		}
	}

	private async void BtnBankMellatExport_Click(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			await LoadDataFromBankTemplateAsync(1, token);
			string text = CreateBankMellatExport();
			if (!(text == ""))
			{
				SaveBankTemplateFile(text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnBankMellatExport_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 617);
		}
	}

	private void BtnRayvarz_Click(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		try
		{
			((Control)BtnRayvarz).Enabled = false;
			FileDialog val = (FileDialog)new SaveFileDialog();
			string text = Calendar.MiladiToCalture(DateTime.Now);
			val.FileName = "RayVarzKardex_" + Text.RemoveNoNumbers(text, "_") + ".txt";
			val.Filter = "Text Files(*.txt) | *.txt | All Files(*.*) | *.*";
			DialogResult val2 = ((CommonDialog)val).ShowDialog((IWin32Window)(object)this);
			string text2 = "";
			if ((int)val2 == 1)
			{
				text2 = val.FileName;
			}
			if (string.IsNullOrEmpty(text2))
			{
				return;
			}
			string rayvarzStr = GetRayvarzStr();
			File.WriteAllText(text2, rayvarzStr);
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[فایل در مسیر] " + text2 + " [با موفقیت ایجاد شد]", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRayvarz_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 641);
		}
		((Control)BtnRayvarz).Enabled = true;
	}

	private async Task DeleteFishAsync(int pId, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (pId > 0)
			{
				await (await Fish.GetAsync(((CacheBaseForm)this).ContextGuid, pId, loadClsTaradod: false, token)).DeleteAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteFishAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 655);
		}
	}

	private async Task LoadDataFromBankTemplateAsync(int bankTemplatesId, CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_BankTemplates_SelectRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)bankTemplatesId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							_bankTemplate = new BankTemplate((IDataRecord)dr);
						}
						((DbConnection)(object)cn).Close();
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataFromBankTemplateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 673);
		}
	}

	private string CreateBankMellatExport()
	{
		try
		{
			if (_bankTemplate == null)
			{
				Utility.MessageBoxShowError("[متاسفانه قالب بانک ملت در دیتابیس تعریف نشده است. ابتدا بازسازی اطلاعات بزنید و اگر مشکل رفع نشد با پشتیبان تماس بگیرید]", ((CacheBaseForm)this).ContextGuid);
				return "";
			}
			int num = _bankTemplate.Template.IndexOf("<");
			int num2 = _bankTemplate.Template.IndexOf(">");
			if (num2 < 2)
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), _bankTemplate.Template);
			}
			string text = _bankTemplate.Template.Substring(num + 1, num2 - 1 - num);
			string[] array = text.Split(new char[1] { ',' });
			if (array[0].ToLower().Trim() != "count")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), text);
			}
			num = num2 + 1;
			num2 = _bankTemplate.Template.IndexOf('>', num);
			text = _bankTemplate.Template.Substring(num + 1, num2 - 1 - num);
			string[] array2 = text.Split(new char[1] { ',' });
			if (array2[0].ToLower().Trim() != "sum")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), text);
			}
			num = _bankTemplate.Template.ToLower().IndexOf("<row>", StringComparison.Ordinal) + 4;
			num = _bankTemplate.Template.ToLower().IndexOf("<", num, StringComparison.Ordinal);
			num2 = _bankTemplate.Template.ToLower().IndexOf("</row>", num, StringComparison.Ordinal);
			num2 = _bankTemplate.Template.ToLower().LastIndexOf(">", num2, StringComparison.Ordinal);
			if (num2 < num + 5)
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), _bankTemplate.Template);
			}
			text = _bankTemplate.Template.Substring(num + 1, num2 - 1 - num).Trim();
			string[] array3 = text.Replace(">", "").Split(new char[1] { '<' });
			string[] array4 = array3[0].Split(new char[1] { ',' });
			if (array4[0].ToLower().Trim() != "hesabno")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), array3[0]);
			}
			string[] array5 = array3[1].Split(new char[1] { ',' });
			if (array5[0].ToLower().Trim() != "price")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), array3[1]);
			}
			string[] array6 = array3[2].Split(new char[1] { ',' });
			if (array6[0].ToLower().Trim() != "identity")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), array3[2]);
			}
			string[] array7 = array3[3].Split(new char[1] { ',' });
			if (array7[0].ToLower().Trim() != "itemdesc")
			{
				throw new ArgumentException(Utilities.GetTranslate("[فرمت نادرست است]"), array3[3]);
			}
			List<Fish> list = (from gl in (List<Fish>)fishBindingSource.DataSource
				join fi in ListFishId on gl.Id equals fi
				select gl).ToList();
			int count = list.Count;
			decimal num3 = list.Select((Fish q) => q.SumKhales).Sum().RoundPrice();
			string text2 = count.ToString().Fit(int.Parse(array[1].Trim()), char.Parse(array[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array[2].Trim()));
			text2 += num3.ToString().Fit(int.Parse(array2[1].Trim()), char.Parse(array2[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array2[2].Trim()));
			text2 += "\r\n";
			foreach (Fish item in list)
			{
				text2 += item.ClsGharardad?.BankNo.Fit(int.Parse(array4[1].Trim()), char.Parse(array4[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array4[2].Trim()));
				text2 += item.SumKhales.RoundPrice().ToString().Fit(int.Parse(array5[1].Trim()), char.Parse(array5[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array5[2].Trim()));
				text2 += "0".Fit(int.Parse(array6[1].Trim()), char.Parse(array6[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array6[2].Trim()));
				text2 += _bankTemplate.RowDescription.Fit(int.Parse(array7[1].Trim()), char.Parse(array7[3].Replace("\"", "").Replace("”", "").Replace("'", "")), char.Parse(array7[2].Trim()));
				text2 += "\r\n";
			}
			return text2;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateBankMellatExport", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 783);
			return "";
		}
	}

	private string SaveBankTemplateFile(string expString)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		try
		{
			if (_bankTemplate?.Title != null && _bankTemplate.FileExtention != null)
			{
				FileDialog val = (FileDialog)new SaveFileDialog
				{
					Filter = _bankTemplate.Title + "|*" + _bankTemplate.FileExtention + "|All Files(*.*)|*.*",
					DefaultExt = _bankTemplate.FileExtention,
					AddExtension = true
				};
				if ((int)((CommonDialog)val).ShowDialog((IWin32Window)(object)this) != 1)
				{
					return null;
				}
				string extension = Path.GetExtension(val.FileName);
				if (extension != null && extension != _bankTemplate.FileExtention)
				{
					val.FileName = val.FileName.Replace(extension, _bankTemplate.FileExtention);
				}
				StreamWriter streamWriter = File.CreateText(val.FileName);
				streamWriter.Write(expString);
				streamWriter.Close();
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[فایل در مسیر زیر ذخیره شد] \n" + val.FileName, "", 3);
				return val.FileName;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveBankTemplateFile", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 813);
		}
		return null;
	}

	private async Task RefreshRowsColorAsync(CancellationToken token)
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
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Expected O, but got Unknown
				try
				{
					if (token.IsCancellationRequested || IsDisposed)
					{
						return;
					}
					foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
					{
						DataGridViewRow val = item;
						if (token.IsCancellationRequested)
						{
							break;
						}
						((DataGridViewBand)val).DefaultCellStyle.BackColor = ((!string.IsNullOrEmpty(val.Cells[((DataGridViewBand)IdSanad).Index].Value?.ToString()) && val.Cells[((DataGridViewBand)IdSanad).Index].Value.ToString() != "0") ? Color.DarkSeaGreen : Color.White);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshRowsColorAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 846);
		}
	}

	private async Task RefreshLblAsync(CancellationToken token)
	{
		try
		{
			string karKards = ((IEnumerable<Fish>)_listForDisplay).Sum((Fish q) => q.SumKarkerd).ToString("N0");
			string khaleses = ((IEnumerable<Fish>)_listForDisplay).Sum((Fish q) => q.SumKhales).ToString("N0");
			string kosorat = ((IEnumerable<Fish>)_listForDisplay).Sum((Fish q) => q.SumKosorat).ToString("N0");
			string mazaya = ((IEnumerable<Fish>)_listForDisplay).Sum((Fish q) => q.SumMazaya).ToString("N0");
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
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
					if (!token.IsCancellationRequested && !IsDisposed && !IsDisposed && !token.IsCancellationRequested)
					{
						((Control)LblRowCount).Text = $"{_countRecordTr}:  {((Collection<Fish>)(object)_listForDisplay).Count}";
						((Control)LblSumK).Text = karKards;
						((Control)LblSumKh).Text = khaleses;
						((Control)LblSumKosor).Text = kosorat;
						((Control)LblSumM).Text = mazaya;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshLblAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 879);
		}
	}

	private async void SelectedUserToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			FrmUserMain frm = new FrmUserMain(((CacheBaseForm)this).ContextGuid, isSelect: true, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_selecteUsers = frm.ListUserAttendancesSelected.Select((UserAttendance q) => q.ID).ToList();
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectedUserToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 894);
		}
	}

	private void ReverseSelectAllDetTSMI_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = val == null || ((DataGridViewCell)val).Value == null || !(bool)((DataGridViewCell)val).Value;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReverseSelectAllDetTSMI_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 904);
		}
	}

	private void UnSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UnSelectAllToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 914);
		}
	}

	private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).Rows[i].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectAllToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 924);
		}
	}

	private async void CmsSabteDaem_Click(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			switch (await LockFishAsync(token, lockState: true))
			{
			case EnumkFishEdit.EnumFishEditSt.Error:
				FMessageBox.Show("امکان قفل کردن فیش نیست", "قفل کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Error);
				break;
			case EnumkFishEdit.EnumFishEditSt.Ok:
				SelectedFish.EditST = 1;
				break;
			case EnumkFishEdit.EnumFishEditSt.IsLockBefore:
				FMessageBox.Show("فیش قبلا قفل شده است", "قفل کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Information);
				break;
			case EnumkFishEdit.EnumFishEditSt.NotAccess:
				FMessageBox.Show("اجازه تغییر وضعیت این فیش را ندارید", "قفل کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				break;
			default:
				FMessageBox.Show("خطای نامشخص اتفاق افتاده است", "خطا", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Error);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmsSabteDaem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 957);
		}
	}

	private async void CmsSabteMoghat_Click(object sender, EventArgs e)
	{
		try
		{
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			switch (await LockFishAsync(token, lockState: false))
			{
			case EnumkFishEdit.EnumFishEditSt.Error:
				FMessageBox.Show("امکان باز کردن فیش نیست", "باز کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Error);
				break;
			case EnumkFishEdit.EnumFishEditSt.Ok:
				SelectedFish.EditST = 0;
				FMessageBox.Show("فیش باز شد", "باز کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Information);
				break;
			case EnumkFishEdit.EnumFishEditSt.NotAccess:
				FMessageBox.Show("اجازه باز کردن این فیش را ندارید", "باز کردن فیش", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
				break;
			default:
				FMessageBox.Show("خطای نامشخص اتفاق افتاده است", "خطا", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Error);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmsSabteMoghat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 987);
		}
	}

	private async void CMSPrintFish_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((DataGridView)DGrid).CurrentRow == null || DGId == null)
			{
				return;
			}
			object value = ((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DGId).Index].Value;
			if (value != null)
			{
				int.TryParse(value.ToString(), out var result);
				if (result > 0)
				{
					_searchToken?.Cancel();
					_searchToken = new CancellationTokenSource();
					CancellationToken token = _searchToken.Token;
					Fish fish = await Fish.GetAsync(((CacheBaseForm)this).ContextGuid, result, loadClsTaradod: false, token);
					List<object> lst = fish.ListFishDetForReport ?? new List<object>();
					FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)6, lst, null, fish.Id, fish.ClsGharardad?.No ?? "0");
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CMSPrintFish_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 1013);
		}
	}

	private void CmsRayVarz_Click(object sender, EventArgs e)
	{
		try
		{
			((ToolStripItem)CmsRayVarz).Enabled = false;
			Utility.MessageBoxShow(GetRayvarzStr(), ((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmsRayVarz_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 1027);
		}
		((ToolStripItem)CmsRayVarz).Enabled = true;
	}

	private async Task<EnumkFishEdit.EnumFishEditSt> LockFishAsync(CancellationToken token, bool lockState)
	{
		try
		{
			short iD = UserAttendance.SelectedUser.ID;
			int id = SelectedFish.Id;
			return await Fish.LockFishAsync(((CacheBaseForm)this).ContextGuid, id, iD, lockState, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LockFishAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 1043);
			return EnumkFishEdit.EnumFishEditSt.Error;
		}
	}

	private string GetRayvarzStr()
	{
		try
		{
			List<string> list = new List<string>();
			new List<Task>();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			_ = _searchToken.Token;
			foreach (Fish selectedFish in SelectedFishes)
			{
				list.Add(selectedFish.GetRayVarzTaradodKardex());
			}
			return string.Join("\n", list);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetRayvarzStr", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMFishList.cs", 1063);
		}
		return "";
	}

	private void fishBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedFish = (Fish)fishBindingSource.Current;
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
				_refreshLablesToken?.Cancel();
				_refreshLablesToken?.Dispose();
				_refreshLablesToken = null;
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

	private void BtnMaliatKarkonan_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)AccessLevelClass.Instance.LevelHozor < 5)
			{
				MessageBoxFarsi.Show("برای دسترسی به این ویژگی، بایستی حضور سطح 5 خریداری نمائید", "عدم دسترسی", (MessageBoxFarsiButtons)0, (MessageBoxFarsiIcon)1);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (SelectedFishes.Count > 0)
			{
				foreach (Fish selectedFish in SelectedFishes)
				{
					ClsTaxPersonInfo clsTaxPersonInfo = new ClsTaxPersonInfo(((CacheBaseForm)this).ContextGuid, selectedFish.ClsPerson);
					stringBuilder.AppendLine(clsTaxPersonInfo.GetTaXRecoedString());
				}
				string text = Path.Combine(Application.StartupPath, "Tax");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (Directory.Exists(text))
				{
					string text2 = Path.Combine(text, "WP" + Text.RemoveNoNumbers(Calendar.MiladiToShamsi(DateTime.Now).Substring(0, 7), "") + ".txt");
					File.AppendAllText(text2, stringBuilder.ToString());
					MessageBoxFarsi.Show("فایل مالیات اطلاعات کارکنان در مسیر زیر ایجاد شد\r\n" + text2);
				}
			}
			else
			{
				MessageBoxFarsi.Show("ابتدا فیش های مد نظر را انتخاب نمائید");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnMaliatHoghogh_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)AccessLevelClass.Instance.LevelHozor < 5)
			{
				MessageBoxFarsi.Show("برای دسترسی به این ویژگی، بایستی حضور سطح 5 خریداری نمائید", "عدم دسترسی", (MessageBoxFarsiButtons)0, (MessageBoxFarsiIcon)1);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (SelectedFishes.Count > 0)
			{
				foreach (Fish selectedFish in SelectedFishes)
				{
					ClsTaxHoghoghInfo clsTaxHoghoghInfo = new ClsTaxHoghoghInfo(((CacheBaseForm)this).ContextGuid, selectedFish);
					stringBuilder.AppendLine(clsTaxHoghoghInfo.GetTaXRecoedString());
				}
				string text = Path.Combine(Application.StartupPath, "Tax");
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (Directory.Exists(text))
				{
					string text2 = Path.Combine(text, "WH" + Text.RemoveNoNumbers(Calendar.MiladiToShamsi(DateTime.Now).Substring(0, 7), "") + ".txt");
					File.AppendAllText(text2, stringBuilder.ToString());
					MessageBoxFarsi.Show("فایل مالیات لیست حقوق کارکنان در مسیر زیر ایجاد شد\r\n" + text2);
				}
			}
			else
			{
				MessageBoxFarsi.Show("ابتدا فیش های مد نظر را انتخاب نمائید");
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
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
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
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
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a24: Expected O, but got Unknown
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Expected O, but got Unknown
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Expected O, but got Unknown
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Expected O, but got Unknown
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Expected O, but got Unknown
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Expected O, but got Unknown
		//IL_0e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e10: Expected O, but got Unknown
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Expected O, but got Unknown
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f14: Expected O, but got Unknown
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff1: Expected O, but got Unknown
		//IL_1002: Unknown result type (might be due to invalid IL or missing references)
		//IL_100c: Expected O, but got Unknown
		//IL_10df: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e9: Expected O, but got Unknown
		//IL_10fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1104: Expected O, but got Unknown
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Expected O, but got Unknown
		//IL_11ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f9: Expected O, but got Unknown
		//IL_13eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f5: Expected O, but got Unknown
		//IL_1503: Unknown result type (might be due to invalid IL or missing references)
		//IL_150d: Expected O, but got Unknown
		//IL_209a: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a4: Expected O, but got Unknown
		//IL_2255: Unknown result type (might be due to invalid IL or missing references)
		//IL_225f: Expected O, but got Unknown
		//IL_23bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c6: Expected O, but got Unknown
		//IL_24a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b1: Expected O, but got Unknown
		//IL_25b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c2: Expected O, but got Unknown
		//IL_2603: Unknown result type (might be due to invalid IL or missing references)
		//IL_260d: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmFishList));
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
		panelEx1 = new PanelEx();
		BtnRayvarz = new ButtonX();
		BtnBankMellatExport = new ButtonX();
		labelX3 = new LabelX();
		LblSumM = new LabelX();
		LblSumKh = new LabelX();
		LblSumKosor = new LabelX();
		LblSumK = new LabelX();
		labelX6 = new LabelX();
		labelX5 = new LabelX();
		LblRowCount = new LabelX();
		labelX2 = new LabelX();
		SodorSanad = new ButtonX();
		BtnPrint = new ButtonX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		DGrid = new DataGridViewX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGId = new DataGridViewTextBoxColumn();
		FullName = new DataGridViewTextBoxColumn();
		DgFishNo = new DataGridViewTextBoxColumn();
		FishMonthYear = new DataGridViewTextBoxColumn();
		DGDateSh = new DataGridViewTextBoxColumn();
		IdSanad = new DataGridViewTextBoxColumn();
		DGDateStartSh = new DataGridViewTextBoxColumn();
		DGDateEndSh = new DataGridViewTextBoxColumn();
		DGSumMazaya = new DataGridViewTextBoxColumn();
		DGSumKosor = new DataGridViewTextBoxColumn();
		SumKhales = new DataGridViewTextBoxColumn();
		LockState = new DataGridViewCheckBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		AddToolStripMenuItem = new ToolStripMenuItem();
		EditToolStripMenuItem = new ToolStripMenuItem();
		DeleteToolStripMenuItem = new ToolStripMenuItem();
		SodorSanadToolStripMenuItem = new ToolStripMenuItem();
		chapToolStripMenuItem = new ToolStripMenuItem();
		CMSPrintFish = new ToolStripMenuItem();
		SelectAllToolStripMenuItem = new ToolStripMenuItem();
		SelectAllDetTSMI = new ToolStripMenuItem();
		UnSelectAllDetTSMI = new ToolStripMenuItem();
		ReverseSelectAllDetTSMI = new ToolStripMenuItem();
		SabteDaemToolStripMenuItem = new ToolStripMenuItem();
		CmsSabteDaem = new ToolStripMenuItem();
		CmsSabteMoghat = new ToolStripMenuItem();
		SelectedUserToolStripMenuItem = new ToolStripMenuItem();
		CmsRayVarz = new ToolStripMenuItem();
		fishBindingSource = new BindingSource(components);
		GroupPanel1 = new GroupPanel();
		TxtSearch = new TextBoxX();
		BtnSelect = new ButtonX();
		BtnMaliatKarkonan = new ButtonX();
		BtnMaliatHoghogh = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((ISupportInitialize)fishBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnMaliatHoghogh);
		((Control)panelEx1).Controls.Add((Control)(object)BtnMaliatKarkonan);
		((Control)panelEx1).Controls.Add((Control)(object)BtnRayvarz);
		((Control)panelEx1).Controls.Add((Control)(object)BtnBankMellatExport);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumM);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumKh);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumKosor);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumK);
		((Control)panelEx1).Controls.Add((Control)(object)labelX6);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)SodorSanad);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)panelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnSelect);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1158, 573);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)BtnRayvarz).AccessibleRole = (AccessibleRole)43;
		BtnRayvarz.ColorTable = (eButtonColor)6;
		((Control)BtnRayvarz).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		BtnRayvarz.Image = (Image)componentResourceManager.GetObject("BtnRayvarz.Image");
		BtnRayvarz.ImageFixedSize = new Size(25, 25);
		BtnRayvarz.ImagePosition = (eImagePosition)1;
		((Control)BtnRayvarz).Location = new Point(14, 319);
		((Control)BtnRayvarz).Name = "BtnRayvarz";
		((Control)BtnRayvarz).RightToLeft = (RightToLeft)1;
		((Control)BtnRayvarz).Size = new Size(149, 33);
		((PopupItemControl)BtnRayvarz).Style = (eDotNetBarStyle)9;
		((Control)BtnRayvarz).TabIndex = 48;
		((Control)BtnRayvarz).Text = "   خروجی رایورز                                ";
		BtnRayvarz.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnRayvarz).Click += BtnRayvarz_Click;
		((Control)BtnBankMellatExport).AccessibleRole = (AccessibleRole)43;
		BtnBankMellatExport.ColorTable = (eButtonColor)6;
		((Control)BtnBankMellatExport).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnBankMellatExport.Image = (Image)componentResourceManager.GetObject("BtnBankMellatExport.Image");
		BtnBankMellatExport.ImageFixedSize = new Size(25, 25);
		BtnBankMellatExport.ImagePosition = (eImagePosition)1;
		((Control)BtnBankMellatExport).Location = new Point(14, 280);
		((Control)BtnBankMellatExport).Name = "BtnBankMellatExport";
		((Control)BtnBankMellatExport).RightToLeft = (RightToLeft)1;
		((Control)BtnBankMellatExport).Size = new Size(149, 33);
		((PopupItemControl)BtnBankMellatExport).Style = (eDotNetBarStyle)9;
		((Control)BtnBankMellatExport).TabIndex = 47;
		((Control)BtnBankMellatExport).Text = "   خروجی بانک ملت  ";
		BtnBankMellatExport.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnBankMellatExport).Click += BtnBankMellatExport_Click;
		((Control)labelX3).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX3).Location = new Point(594, 544);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(112, 23);
		((Control)labelX3).TabIndex = 46;
		((Control)labelX3).Text = "جمع کل کسورات:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)LblSumM).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblSumM).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumM).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumM).ForeColor = Color.DarkRed;
		((Control)LblSumM).Location = new Point(712, 544);
		((Control)LblSumM).Name = "LblSumM";
		((Control)LblSumM).Size = new Size(112, 23);
		((Control)LblSumM).TabIndex = 45;
		((Control)LblSumM).Text = "0";
		((Control)LblSumKh).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblSumKh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumKh).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumKh).ForeColor = Color.DarkRed;
		((Control)LblSumKh).Location = new Point(178, 544);
		((Control)LblSumKh).Name = "LblSumKh";
		((Control)LblSumKh).Size = new Size(135, 23);
		((Control)LblSumKh).TabIndex = 44;
		((Control)LblSumKh).Text = "0";
		((Control)LblSumKosor).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblSumKosor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumKosor).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumKosor).ForeColor = Color.DarkRed;
		((Control)LblSumKosor).Location = new Point(464, 546);
		((Control)LblSumKosor).Name = "LblSumKosor";
		((Control)LblSumKosor).Size = new Size(124, 21);
		((Control)LblSumKosor).TabIndex = 43;
		((Control)LblSumKosor).Text = "0";
		((Control)LblSumK).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblSumK).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumK).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumK).ForeColor = Color.DarkRed;
		((Control)LblSumK).Location = new Point(930, 544);
		((Control)LblSumK).Name = "LblSumK";
		((Control)LblSumK).Size = new Size(128, 23);
		((Control)LblSumK).TabIndex = 42;
		((Control)LblSumK).Text = "0";
		((Control)labelX6).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX6).Location = new Point(320, 544);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(136, 23);
		((Control)labelX6).TabIndex = 41;
		((Control)labelX6).Text = "[جمع خالص پرداختی] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX5).Location = new Point(830, 544);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(94, 23);
		((Control)labelX5).TabIndex = 40;
		((Control)labelX5).Text = "[جمع کل مزایا]:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblRowCount).Location = new Point(14, 515);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).RightToLeft = (RightToLeft)1;
		((Control)LblRowCount).Size = new Size(149, 23);
		((Control)LblRowCount).TabIndex = 39;
		((Control)LblRowCount).Text = "[جمع حقوق] :";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).Location = new Point(1074, 544);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(72, 23);
		((Control)labelX2).TabIndex = 39;
		((Control)labelX2).Text = "جمع حقوق:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)SodorSanad).AccessibleRole = (AccessibleRole)43;
		SodorSanad.ColorTable = (eButtonColor)6;
		((Control)SodorSanad).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		SodorSanad.Image = (Image)(object)Resources.report_check;
		SodorSanad.ImagePosition = (eImagePosition)1;
		((Control)SodorSanad).Location = new Point(14, 201);
		((Control)SodorSanad).Name = "SodorSanad";
		((Control)SodorSanad).RightToLeft = (RightToLeft)1;
		((Control)SodorSanad).Size = new Size(149, 33);
		((PopupItemControl)SodorSanad).Style = (eDotNetBarStyle)9;
		((Control)SodorSanad).TabIndex = 37;
		((Control)SodorSanad).Text = "   صدور سند حقوق";
		SodorSanad.TextAlignment = (eButtonTextAlignment)0;
		((Control)SodorSanad).Click += SodorSanad_Click;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(14, 240);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).RightToLeft = (RightToLeft)1;
		((Control)BtnPrint).Size = new Size(149, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 36;
		((Control)BtnPrint).Text = "   چاپ   (Ctrl+P)      ";
		BtnPrint.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)componentResourceManager.GetObject("BtnDelete.Image");
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(14, 162);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).RightToLeft = (RightToLeft)1;
		((Control)BtnDelete).Size = new Size(149, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 35;
		((Control)BtnDelete).Text = "   حذف   (Delete)    ";
		BtnDelete.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)componentResourceManager.GetObject("BtnEdit.Image");
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(14, 123);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(149, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 34;
		((Control)BtnEdit).Text = "    ویرایش     (F7)";
		BtnEdit.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)componentResourceManager.GetObject("BtnAdd.Image");
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(14, 84);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).RightToLeft = (RightToLeft)1;
		((Control)BtnAdd).Size = new Size(149, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 33;
		((Control)BtnAdd).Text = "   اضافه   (Insert)";
		BtnAdd.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[13]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGId,
			(DataGridViewColumn)FullName,
			(DataGridViewColumn)DgFishNo,
			(DataGridViewColumn)FishMonthYear,
			(DataGridViewColumn)DGDateSh,
			(DataGridViewColumn)IdSanad,
			(DataGridViewColumn)DGDateStartSh,
			(DataGridViewColumn)DGDateEndSh,
			(DataGridViewColumn)DGSumMazaya,
			(DataGridViewColumn)DGSumKosor,
			(DataGridViewColumn)SumKhales,
			(DataGridViewColumn)LockState
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((Control)DGrid).Cursor = Cursors.Default;
		((DataGridView)DGrid).DataSource = fishBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(171, 85);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersBorderStyle = (DataGridViewHeaderBorderStyle)4;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).RowHeadersWidthSizeMode = (DataGridViewRowHeadersWidthSizeMode)1;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((DataGridView)DGrid).ShowEditingIcon = false;
		((Control)DGrid).Size = new Size(975, 453);
		((Control)DGrid).TabIndex = 17;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((DataGridViewColumn)DGCheck).DataPropertyName = "Check";
		((DataGridViewBand)DGCheck).Frozen = true;
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewColumn)DGCheck).Width = 25;
		((DataGridViewColumn)DGId).DataPropertyName = "Id";
		((DataGridViewColumn)DGId).HeaderText = "Id";
		((DataGridViewColumn)DGId).Name = "DGId";
		((DataGridViewBand)DGId).ReadOnly = true;
		((DataGridViewBand)DGId).Visible = false;
		((DataGridViewColumn)FullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)FullName).DataPropertyName = "FullName";
		((DataGridViewColumn)FullName).HeaderText = "نام شخص";
		((DataGridViewColumn)FullName).Name = "FullName";
		((DataGridViewBand)FullName).ReadOnly = true;
		((DataGridViewColumn)DgFishNo).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgFishNo).DataPropertyName = "FishNo";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DgFishNo).DefaultCellStyle = val3;
		((DataGridViewColumn)DgFishNo).HeaderText = "[ش فیش]";
		((DataGridViewColumn)DgFishNo).MinimumWidth = 20;
		((DataGridViewColumn)DgFishNo).Name = "DgFishNo";
		((DataGridViewBand)DgFishNo).ReadOnly = true;
		((DataGridViewColumn)DgFishNo).Width = 86;
		((DataGridViewColumn)FishMonthYear).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)FishMonthYear).DataPropertyName = "FishMonthYear";
		((DataGridViewColumn)FishMonthYear).HeaderText = "دوره مربوطه";
		((DataGridViewColumn)FishMonthYear).Name = "FishMonthYear";
		((DataGridViewBand)FishMonthYear).ReadOnly = true;
		((DataGridViewColumn)FishMonthYear).Width = 89;
		((DataGridViewColumn)DGDateSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateSh).DataPropertyName = "DateSh";
		val4.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DGDateSh).DefaultCellStyle = val4;
		((DataGridViewColumn)DGDateSh).HeaderText = "[تاریخ صدور]";
		((DataGridViewColumn)DGDateSh).MinimumWidth = 88;
		((DataGridViewColumn)DGDateSh).Name = "DGDateSh";
		((DataGridViewBand)DGDateSh).ReadOnly = true;
		((DataGridViewColumn)DGDateSh).Width = 90;
		((DataGridViewColumn)IdSanad).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)IdSanad).DataPropertyName = "IdSanad";
		val5.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)IdSanad).DefaultCellStyle = val5;
		((DataGridViewColumn)IdSanad).HeaderText = "[شماره سند]";
		((DataGridViewColumn)IdSanad).MinimumWidth = 20;
		((DataGridViewColumn)IdSanad).Name = "IdSanad";
		((DataGridViewBand)IdSanad).ReadOnly = true;
		((DataGridViewColumn)IdSanad).Width = 93;
		((DataGridViewColumn)DGDateStartSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateStartSh).DataPropertyName = "DateStartSh";
		val6.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DGDateStartSh).DefaultCellStyle = val6;
		((DataGridViewColumn)DGDateStartSh).HeaderText = "[از تاریخ]";
		((DataGridViewColumn)DGDateStartSh).MinimumWidth = 88;
		((DataGridViewColumn)DGDateStartSh).Name = "DGDateStartSh";
		((DataGridViewBand)DGDateStartSh).ReadOnly = true;
		((DataGridViewColumn)DGDateStartSh).Width = 88;
		((DataGridViewColumn)DGDateEndSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDateEndSh).DataPropertyName = "DateEndSh";
		val7.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DGDateEndSh).DefaultCellStyle = val7;
		((DataGridViewColumn)DGDateEndSh).HeaderText = "تا تاریخ";
		((DataGridViewColumn)DGDateEndSh).MinimumWidth = 88;
		((DataGridViewColumn)DGDateEndSh).Name = "DGDateEndSh";
		((DataGridViewBand)DGDateEndSh).ReadOnly = true;
		((DataGridViewColumn)DGDateEndSh).Width = 88;
		((DataGridViewColumn)DGSumMazaya).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGSumMazaya).DataPropertyName = "SumMazaya";
		val8.Format = "N0";
		val8.NullValue = null;
		((DataGridViewBand)DGSumMazaya).DefaultCellStyle = val8;
		((DataGridViewColumn)DGSumMazaya).HeaderText = "[مزایا]";
		((DataGridViewColumn)DGSumMazaya).MinimumWidth = 90;
		((DataGridViewColumn)DGSumMazaya).Name = "DGSumMazaya";
		((DataGridViewBand)DGSumMazaya).ReadOnly = true;
		((DataGridViewColumn)DGSumMazaya).Width = 90;
		((DataGridViewColumn)DGSumKosor).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGSumKosor).DataPropertyName = "SumKosorat";
		val9.Format = "N0";
		((DataGridViewBand)DGSumKosor).DefaultCellStyle = val9;
		((DataGridViewColumn)DGSumKosor).HeaderText = "[کسورات]";
		((DataGridViewColumn)DGSumKosor).MinimumWidth = 90;
		((DataGridViewColumn)DGSumKosor).Name = "DGSumKosor";
		((DataGridViewBand)DGSumKosor).ReadOnly = true;
		((DataGridViewColumn)DGSumKosor).Width = 90;
		((DataGridViewColumn)SumKhales).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)SumKhales).DataPropertyName = "SumKhales";
		val10.Format = "N0";
		((DataGridViewBand)SumKhales).DefaultCellStyle = val10;
		((DataGridViewColumn)SumKhales).HeaderText = "خالص حقوق";
		((DataGridViewColumn)SumKhales).Name = "SumKhales";
		((DataGridViewBand)SumKhales).ReadOnly = true;
		((DataGridViewColumn)SumKhales).Width = 90;
		((DataGridViewColumn)LockState).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)LockState).DataPropertyName = "EditST";
		((DataGridViewColumn)LockState).HeaderText = "ثبت دائم";
		((DataGridViewColumn)LockState).MinimumWidth = 40;
		((DataGridViewColumn)LockState).Name = "LockState";
		((DataGridViewBand)LockState).ReadOnly = true;
		((DataGridViewBand)LockState).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)LockState).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)LockState).Width = 53;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[10]
		{
			(ToolStripItem)AddToolStripMenuItem,
			(ToolStripItem)EditToolStripMenuItem,
			(ToolStripItem)DeleteToolStripMenuItem,
			(ToolStripItem)SodorSanadToolStripMenuItem,
			(ToolStripItem)chapToolStripMenuItem,
			(ToolStripItem)CMSPrintFish,
			(ToolStripItem)SelectAllToolStripMenuItem,
			(ToolStripItem)SabteDaemToolStripMenuItem,
			(ToolStripItem)SelectedUserToolStripMenuItem,
			(ToolStripItem)CmsRayVarz
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(189, 224);
		((ToolStripItem)AddToolStripMenuItem).Name = "AddToolStripMenuItem";
		((ToolStripItem)AddToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)AddToolStripMenuItem).Text = "[اضافه]";
		((ToolStripItem)AddToolStripMenuItem).Click += BtnAdd_Click;
		((ToolStripItem)EditToolStripMenuItem).Name = "EditToolStripMenuItem";
		((ToolStripItem)EditToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)EditToolStripMenuItem).Text = "[ویرایش]";
		((ToolStripItem)EditToolStripMenuItem).Click += BtnEdit_Click;
		((ToolStripItem)DeleteToolStripMenuItem).Name = "DeleteToolStripMenuItem";
		((ToolStripItem)DeleteToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)DeleteToolStripMenuItem).Text = "[حذف]";
		((ToolStripItem)DeleteToolStripMenuItem).Click += BtnDelete_Click;
		((ToolStripItem)SodorSanadToolStripMenuItem).Name = "SodorSanadToolStripMenuItem";
		((ToolStripItem)SodorSanadToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)SodorSanadToolStripMenuItem).Text = "[صدور سند حسابداری]";
		((ToolStripItem)SodorSanadToolStripMenuItem).Click += SodorSanad_Click;
		((ToolStripItem)chapToolStripMenuItem).Name = "chapToolStripMenuItem";
		((ToolStripItem)chapToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)chapToolStripMenuItem).Text = "[چاپ گزارش فیش ها]";
		((ToolStripItem)chapToolStripMenuItem).Click += BtnPrint_Click;
		((ToolStripItem)CMSPrintFish).Name = "CMSPrintFish";
		((ToolStripItem)CMSPrintFish).Size = new Size(188, 22);
		((ToolStripItem)CMSPrintFish).Text = "[چاپ فیش]";
		((ToolStripItem)CMSPrintFish).Click += CMSPrintFish_Click;
		((ToolStripDropDownItem)SelectAllToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)SelectAllDetTSMI,
			(ToolStripItem)UnSelectAllDetTSMI,
			(ToolStripItem)ReverseSelectAllDetTSMI
		});
		((ToolStripItem)SelectAllToolStripMenuItem).Name = "SelectAllToolStripMenuItem";
		((ToolStripItem)SelectAllToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)SelectAllToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)SelectAllDetTSMI).Name = "SelectAllDetTSMI";
		((ToolStripItem)SelectAllDetTSMI).Size = new Size(198, 22);
		((ToolStripItem)SelectAllDetTSMI).Text = "[انتخاب همه]";
		((ToolStripItem)SelectAllDetTSMI).Click += SelectAllToolStripMenuItem_Click;
		((ToolStripItem)UnSelectAllDetTSMI).Name = "UnSelectAllDetTSMI";
		((ToolStripItem)UnSelectAllDetTSMI).Size = new Size(198, 22);
		((ToolStripItem)UnSelectAllDetTSMI).Text = "[پاک کردن همه]";
		((ToolStripItem)UnSelectAllDetTSMI).Click += UnSelectAllToolStripMenuItem_Click;
		((ToolStripItem)ReverseSelectAllDetTSMI).Name = "ReverseSelectAllDetTSMI";
		((ToolStripItem)ReverseSelectAllDetTSMI).Size = new Size(198, 22);
		((ToolStripItem)ReverseSelectAllDetTSMI).Text = "[معکوس کردن انتخاب ها]";
		((ToolStripItem)ReverseSelectAllDetTSMI).Click += ReverseSelectAllDetTSMI_Click;
		((ToolStripDropDownItem)SabteDaemToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)CmsSabteDaem,
			(ToolStripItem)CmsSabteMoghat
		});
		((ToolStripItem)SabteDaemToolStripMenuItem).Name = "SabteDaemToolStripMenuItem";
		((ToolStripItem)SabteDaemToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)SabteDaemToolStripMenuItem).Text = "[تغییر وضعیت فیش]";
		((ToolStripItem)CmsSabteDaem).Name = "CmsSabteDaem";
		((ToolStripItem)CmsSabteDaem).Size = new Size(134, 22);
		((ToolStripItem)CmsSabteDaem).Text = "[ثبت دائم]";
		((ToolStripItem)CmsSabteDaem).Click += CmsSabteDaem_Click;
		((ToolStripItem)CmsSabteMoghat).Name = "CmsSabteMoghat";
		((ToolStripItem)CmsSabteMoghat).Size = new Size(134, 22);
		((ToolStripItem)CmsSabteMoghat).Text = "[ثبت موقت]";
		((ToolStripItem)CmsSabteMoghat).Click += CmsSabteMoghat_Click;
		((ToolStripItem)SelectedUserToolStripMenuItem).Name = "SelectedUserToolStripMenuItem";
		((ToolStripItem)SelectedUserToolStripMenuItem).Size = new Size(188, 22);
		((ToolStripItem)SelectedUserToolStripMenuItem).Text = "[گزارش کاربر خاص]";
		((ToolStripItem)SelectedUserToolStripMenuItem).Visible = false;
		((ToolStripItem)SelectedUserToolStripMenuItem).Click += SelectedUserToolStripMenuItem_Click;
		((ToolStripItem)CmsRayVarz).Name = "CmsRayVarz";
		((ToolStripItem)CmsRayVarz).Size = new Size(188, 22);
		((ToolStripItem)CmsRayVarz).Text = "[مشاهده خروجی رایورز]";
		((ToolStripItem)CmsRayVarz).Click += CmsRayVarz_Click;
		fishBindingSource.DataSource = typeof(Fish);
		fishBindingSource.CurrentChanged += fishBindingSource_CurrentChanged;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)GroupPanel1).Location = new Point(14, 12);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(1132, 66);
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
		((Control)GroupPanel1).TabIndex = 16;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)componentResourceManager.GetObject("GroupPanel1.TitleImage");
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 7);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(1120, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = (eButtonColor)6;
		((Control)BtnSelect).Location = new Point(13, 85);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(152, 189);
		((PopupItemControl)BtnSelect).Style = (eDotNetBarStyle)9;
		((Control)BtnSelect).TabIndex = 38;
		((Control)BtnSelect).Text = "[انتخاب]\r\n(Enter)";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((Control)BtnMaliatKarkonan).AccessibleRole = (AccessibleRole)43;
		BtnMaliatKarkonan.ColorTable = (eButtonColor)6;
		((Control)BtnMaliatKarkonan).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		BtnMaliatKarkonan.ImageFixedSize = new Size(25, 25);
		BtnMaliatKarkonan.ImagePosition = (eImagePosition)1;
		((Control)BtnMaliatKarkonan).Location = new Point(14, 358);
		((Control)BtnMaliatKarkonan).Name = "BtnMaliatKarkonan";
		((Control)BtnMaliatKarkonan).RightToLeft = (RightToLeft)1;
		((Control)BtnMaliatKarkonan).Size = new Size(149, 33);
		((PopupItemControl)BtnMaliatKarkonan).Style = (eDotNetBarStyle)9;
		((Control)BtnMaliatKarkonan).TabIndex = 49;
		((Control)BtnMaliatKarkonan).Text = "   لیست مالیات- کارکنان                         ";
		BtnMaliatKarkonan.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnMaliatKarkonan).Click += BtnMaliatKarkonan_Click;
		((Control)BtnMaliatHoghogh).AccessibleRole = (AccessibleRole)43;
		BtnMaliatHoghogh.ColorTable = (eButtonColor)6;
		((Control)BtnMaliatHoghogh).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		BtnMaliatHoghogh.ImageFixedSize = new Size(25, 25);
		BtnMaliatHoghogh.ImagePosition = (eImagePosition)1;
		((Control)BtnMaliatHoghogh).Location = new Point(14, 397);
		((Control)BtnMaliatHoghogh).Name = "BtnMaliatHoghogh";
		((Control)BtnMaliatHoghogh).RightToLeft = (RightToLeft)1;
		((Control)BtnMaliatHoghogh).Size = new Size(149, 33);
		((PopupItemControl)BtnMaliatHoghogh).Style = (eDotNetBarStyle)9;
		((Control)BtnMaliatHoghogh).TabIndex = 49;
		((Control)BtnMaliatHoghogh).Text = "   لیست مالیات - حقوق";
		BtnMaliatHoghogh.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnMaliatHoghogh).Click += BtnMaliatHoghogh_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1158, 573);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "FrmFishList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست فیش های حقوقی]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FrmPersonHoghogh_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((ISupportInitialize)fishBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
