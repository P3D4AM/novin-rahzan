using System;
using System.Collections;
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
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Other;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmEndedGharardad : BaseForm
{
	private Font _font;

	private List<GharardadNew> _listData;

	private SortableBindingList<GharardadNew> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private CancellationTokenSource tokenSource = new CancellationTokenSource();

	private List<GharardadStatus> _gharardadStatuses;

	private DateTime _startDate = DateTime.Now;

	private DateTime _endDate = DateTime.Now;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private IContainer components;

	internal PanelEx PanelEx1;

	private LabelX CountRows;

	private LabelX labelX2;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TbxSearch;

	private BindingSource gharardadBindingSource;

	private ToolStripMenuItem SelectAllTSMI;

	private ToolStripMenuItem UpSelectAllTSMI;

	private ToolStripMenuItem ReverseSelectAllDetTSMI;

	private FlowLayoutPanel flowLayoutPanel1;

	private LabelX labelX3;

	private LabelX labelX4;

	private LabelX labelX6;

	private DateBoxFa TbxToDate;

	private DateBoxFa TbxFromDate;

	internal LabelX labelX5;

	private CheckBox ChkFromdate;

	internal ButtonX BtnprintKol;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem Menu1;

	private ToolStripComboBox CmbStatus;

	private ToolStripMenuItem SelectAll;

	private ToolStripMenuItem DeSelectAll;

	private ToolStripMenuItem ReverseSelected;

	private PictureBox pictureBox1;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DGPersonID;

	private DataGridViewTextBoxColumn No;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewTextBoxColumn DateStartSHDGrid;

	private DataGridViewTextBoxColumn DateFinishSHDGrid;

	private DataGridViewTextBoxColumn GharardadStatus;

	private DataGridViewTextBoxColumn GharardadStatusFa;

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

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[منوی یک]", out var value))
			{
				trDictionary.Add("[منوی یک]", Utilities.GetTranslate("[منوی یک]"));
			}
			if (!trDictionary.TryGetValue("[تغییر وضعیت قراردادهای انتخاب شده]", out value))
			{
				trDictionary.Add("[تغییر وضعیت قراردادهای انتخاب شده]", Utilities.GetTranslate("[تغییر وضعیت قراردادهای انتخاب شده]"));
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 57);
		}
	}

	public FrmEndedGharardad(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			Task.Run((Func<Task?>)SetTrDictionaryAsync);
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs");
			InitCmsItems();
			((Control)TbxSearch).Focus();
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
			_listData = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int>(), DateTime.Now.AddYears(-10), DateTime.MaxValue, token);
			_gharardadStatuses = await TimeAttendanceDAL.Dal.FLRGharardad.GharardadStatus.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			bool _ChkFromdate_Checked = timeAttendanceSettings.FromDateEndedGharardad;
			bool _TbxFromDate_Enabled = ChkFromdate.Checked;
			_startDate = DateTime.Now.Date.AddDays(-timeAttendanceSettings.GharardadEndedAgo);
			_endDate = DateTime.Now.Date.AddDays(timeAttendanceSettings.GharardadEndedNext);
			string _TbxFromDate_Text = Calendar.MiladiToCalture(_startDate);
			string _TbxToDate_Text = Calendar.MiladiToCalture(_endDate);
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						ChkFromdate.Checked = _ChkFromdate_Checked;
						((Control)TbxFromDate).Enabled = _TbxFromDate_Enabled;
						((Control)TbxFromDate).Text = _TbxFromDate_Text;
						((Control)TbxToDate).Text = _TbxToDate_Text;
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
			await LoadFilterItemsAsync(token);
			await SearchAsync(token);
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
			((Control)contextMenuStrip1).Text = "منوی یک";
			if (trDictionary.TryGetValue("[منوی یک]", out var value))
			{
				((Control)contextMenuStrip1).Text = value;
			}
			((ToolStripItem)Menu1).Text = "تغییر وضعیت قراردادهای انتخاب شده";
			if (trDictionary.TryGetValue("[تغییر وضعیت قراردادهای انتخاب شده]", out var value2))
			{
				((ToolStripItem)Menu1).Text = value2;
			}
			((ToolStripItem)SelectAll).Text = "انتخاب همه";
			if (trDictionary.TryGetValue("[انتخاب همه]", out var value3))
			{
				((ToolStripItem)SelectAll).Text = value3;
			}
			((ToolStripItem)DeSelectAll).Text = "انتخاب هیچکدام";
			if (trDictionary.TryGetValue("[پاک کردن همه]", out var value4))
			{
				((ToolStripItem)DeSelectAll).Text = value4;
			}
			((ToolStripItem)ReverseSelected).Text = "معکوس کردن انتخاب ها";
			if (trDictionary.TryGetValue("[معکوس کردن انتخاب ها]", out var value5))
			{
				((ToolStripItem)ReverseSelected).Text = value5;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitCmsItems", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 143);
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
			((BaseForm)this).OnLoad(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 159);
		}
	}

	private void Search()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Text.FixString(((Control)TbxSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = await GharardadNew.GetAllAsync(((CacheBaseForm)this).ContextGuid, new List<int>(), DateTime.Now.AddYears(-10), DateTime.MaxValue, token);
			}
			_listForDisplay = null;
			if (ChkFromdate.Checked)
			{
				_listForDisplay = Utilities.ToSortableBindingList<GharardadNew>(_listData.Where((GharardadNew p) => p.DateFinishM >= _startDate && p.DateFinishM <= _endDate), default(CancellationToken));
			}
			else
			{
				_listForDisplay = Utilities.ToSortableBindingList<GharardadNew>(_listData.Where((GharardadNew p) => p.DateFinishM <= _endDate), default(CancellationToken));
			}
			_listForDisplay = Utilities.ToSortableBindingList<GharardadNew>((IEnumerable<GharardadNew>)FilteredList(), default(CancellationToken));
			if (!string.IsNullOrEmpty(_searchString))
			{
				foreach (string item in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					List<GharardadNew> list = new List<GharardadNew>();
					if (token.IsCancellationRequested)
					{
						return;
					}
					foreach (GharardadNew item2 in (Collection<GharardadNew>)(object)_listForDisplay)
					{
						if (item2.FullName.Contains(item) || Calendar.MiladiToCalture(item2.DateFinishM).Contains(item))
						{
							list.Add(item2);
						}
					}
					_listForDisplay = Utilities.ToSortableBindingList<GharardadNew>((IEnumerable<GharardadNew>)list, token);
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
						gharardadBindingSource.DataSource = Utilities.ToSortableBindingList<GharardadNew>((IEnumerable<GharardadNew>)_listForDisplay, default(CancellationToken));
						((Control)CountRows).Text = ((Collection<GharardadNew>)(object)_listForDisplay).Count.ToString("N0");
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
			await FillGhardadStatusFields(tokenSource.Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task FillGhardadStatusFields(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Expected O, but got Unknown
				try
				{
					if (token.IsCancellationRequested || IsDisposed || token.IsCancellationRequested || DGrid == null || ((DataGridView)DGrid).Rows.Count <= 0)
					{
						return;
					}
					foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
					{
						DataGridViewRow row = item;
						if (IsDisposed || token.IsCancellationRequested)
						{
							break;
						}
						row.Cells[((DataGridViewBand)GharardadStatusFa).Index].Value = _gharardadStatuses.FirstOrDefault((GharardadStatus p) => p.Id == short.Parse(row.Cells[((DataGridViewBand)GharardadStatus).Index].Value.ToString()))?.Name;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillGhardadStatusFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 253);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillGhardadStatusFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 256);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		Search();
	}

	private async Task LoadFilterItemsAsync(CancellationToken token)
	{
		try
		{
			List<GharardadStatus> status = _gharardadStatuses;
			((Control)flowLayoutPanel1).Controls.Clear();
			if (status == null)
			{
				return;
			}
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			foreach (GharardadStatus st in status)
			{
				CheckBox val = new CheckBox
				{
					Text = st.Name,
					Checked = false
				};
				if (timeAttendanceSettings.GharardadStatuses != null && timeAttendanceSettings.GharardadStatuses.Find((GharardadStatus p) => p.Name == st.Name) != null)
				{
					val.Checked = true;
				}
				val.CheckedChanged += ChkStatus_CheckedChanged;
				((Control)flowLayoutPanel1).Controls.Add((Control)(object)val);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFilterItemsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 279);
		}
	}

	private void ChkStatus_CheckedChanged(object sender, EventArgs e)
	{
		Search();
	}

	private SortableBindingList<GharardadNew> FilteredList()
	{
		SortableBindingList<GharardadNew> val = _listForDisplay;
		try
		{
			List<string> typeNames = (from CheckBox chkBox in (IEnumerable)((Control)flowLayoutPanel1).Controls
				where chkBox.Checked
				select ((Control)chkBox).Text).ToList();
			if (typeNames.Count > 0)
			{
				val = Utilities.ToSortableBindingList<GharardadNew>(((IEnumerable<GharardadNew>)val).Where((GharardadNew p) => typeNames.Contains(GetNameOfGharardadStatus(p.GharardadStatus))), default(CancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FilteredList", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 294);
		}
		return val;
	}

	private string GetNameOfGharardadStatus(short id)
	{
		try
		{
			return _gharardadStatuses.FirstOrDefault((GharardadStatus p) => p.Id == id)?.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetNameOfGharardadStatus", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 307);
		}
		return "";
	}

	public short GetIdOfGharardadStatus(string name)
	{
		try
		{
			GharardadStatus gharardadStatus = _gharardadStatuses?.FirstOrDefault((GharardadStatus p) => name != null && p != null && p.Name != null && p.Name == name);
			if (gharardadStatus != null)
			{
				return gharardadStatus.Id;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetIdOfGharardadStatus", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 320);
		}
		return 0;
	}

	private void ChkFromDate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TbxFromDate).Enabled = ChkFromdate.Checked;
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkFromDate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 335);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 351);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			List<object> list = new List<object>();
			if (_listForDisplay != null)
			{
				foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
				{
					DataGridViewRow row = item;
					object value = row.Cells[((DataGridViewBand)DGCheck).Index].Value;
					if (value != null && (bool)value)
					{
						GharardadNew gharardadNew = ((IEnumerable<GharardadNew>)_listForDisplay).FirstOrDefault(delegate(GharardadNew p)
						{
							string no = p.No;
							DataGridViewCell obj = row.Cells[((DataGridViewBand)No).Index];
							return no == ((obj == null) ? null : obj.Value?.ToString());
						});
						if (gharardadNew != null)
						{
							list.Add(new
							{
								DateFinishM = Calendar.MiladiToCalture(gharardadNew.DateFinishM),
								DateCalture = Calendar.MiladiToCalture(gharardadNew.DateM),
								DateStartM = Calendar.MiladiToCalture(gharardadNew.DateStartM),
								Description = gharardadNew.Description,
								FullName = gharardadNew.FullName,
								No = gharardadNew.No,
								Salary = gharardadNew.SalaryOneDay,
								SalaryType = gharardadNew.SalaryType,
								SumMazaya = gharardadNew.SumMazaya,
								SumKosorat = gharardadNew.SumKosorat,
								SumHoghMazaya = gharardadNew.SumHoghMazaya,
								GharardadStatus = GetNameOfGharardadStatus(gharardadNew.GharardadStatus)
							});
						}
					}
				}
			}
			if (list.Count <= 0)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "قراردادی جهت چاپ انتخاب نشده است", "", 3);
				return;
			}
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)9, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 397);
		}
	}

	private async void Menu1_Click(object sender, EventArgs e)
	{
		try
		{
			List<GharardadNew> selectedGharardads = GetSelectedGharardads();
			if (selectedGharardads == null || selectedGharardads.Count <= 0)
			{
				return;
			}
			foreach (GharardadNew item in selectedGharardads)
			{
				FrmGharardad frmGharardad = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, item);
				await ((CacheBaseForm)frmGharardad).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frmGharardad).Dispose();
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("بارگذاری مجدد داده", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Menu1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Menu1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 419);
		}
	}

	private List<GharardadNew> GetSelectedGharardads()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		List<GharardadNew> list = new List<GharardadNew>();
		try
		{
			if (_listForDisplay == null)
			{
				return list;
			}
			if (((DataGridView)DGrid).Rows.Count <= 0)
			{
				return list;
			}
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
			{
				DataGridViewRow row = item;
				object value = row.Cells[((DataGridViewBand)DGCheck).Index].Value;
				if (value != null && (bool)value)
				{
					GharardadNew gharardadNew = ((IEnumerable<GharardadNew>)_listForDisplay).FirstOrDefault(delegate(GharardadNew p)
					{
						string no = p.No;
						DataGridViewCell obj = row.Cells[((DataGridViewBand)No).Index];
						return no == ((obj == null) ? null : obj.Value?.ToString());
					});
					if (gharardadNew != null)
					{
						list.Add(gharardadNew);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetSelectedGharardads", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 441);
		}
		return list;
	}

	private void SelectAll_Click(object sender, EventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 456);
		}
	}

	private void DeSelectAll_Click(object sender, EventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeSelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 470);
		}
	}

	private void ReverseSelected_Click(object sender, EventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReverseSelected_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 484);
		}
	}

	private async void pictureBox1_Click(object sender, EventArgs e)
	{
		try
		{
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, "جهت تغییر تنظیمات این صفحه \r\nمیتوانید در منوی تنظیمات نرم افزار به زیر منوی قراردادهای منقضی\r\nمراجعه نمائید", "", 3);
			FrmSetting frm = new FrmSetting(((CacheBaseForm)this).ContextGuid, _font, 7);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			((Form)this).OnLoad(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "pictureBox1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 500);
		}
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		try
		{
			int? num = GetSelectedGharardads()?.Count;
			if (num <= 0)
			{
				((ToolStripItem)Menu1).Enabled = false;
				((ToolStripItem)Menu1).Text = "تغییر وضعیت قراردادهای انتخاب شده";
			}
			else
			{
				((ToolStripItem)Menu1).Enabled = true;
				((ToolStripItem)Menu1).Text = $"تغییر وضعیت {num} قرارداد انتخاب شده";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "contextMenuStrip1_Opening", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 519);
		}
	}

	private void TbxFromDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)TbxFromDate).Text != null)
			{
				_startDate = Utilities.CalendarToMiladi(((Control)TbxFromDate).Text);
			}
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TbxFromDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 532);
		}
	}

	private void TbxToDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)TbxToDate).Text != null)
			{
				_endDate = Utilities.CalendarToMiladi(((Control)TbxToDate).Text);
			}
			Search();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TbxToDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 547);
		}
	}

	private async void FrmEndedGharardad_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
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
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmEndedGharardad_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FrmEndedGharardad.cs", 572);
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
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
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
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
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
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Expected O, but got Unknown
		//IL_0dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd6: Expected O, but got Unknown
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0c: Expected O, but got Unknown
		//IL_1871: Unknown result type (might be due to invalid IL or missing references)
		//IL_187b: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnprintKol = new ButtonX();
		DGrid = new DataGridViewX();
		gharardadBindingSource = new BindingSource(components);
		CountRows = new LabelX();
		labelX2 = new LabelX();
		GroupPanel1 = new GroupPanel();
		pictureBox1 = new PictureBox();
		labelX6 = new LabelX();
		TbxToDate = new DateBoxFa();
		TbxFromDate = new DateBoxFa();
		labelX5 = new LabelX();
		ChkFromdate = new CheckBox();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		flowLayoutPanel1 = new FlowLayoutPanel();
		TbxSearch = new TextBoxX();
		SelectAllTSMI = new ToolStripMenuItem();
		UpSelectAllTSMI = new ToolStripMenuItem();
		ReverseSelectAllDetTSMI = new ToolStripMenuItem();
		LabelX1 = new LabelX();
		contextMenuStrip1 = new ContextMenuStrip(components);
		Menu1 = new ToolStripMenuItem();
		SelectAll = new ToolStripMenuItem();
		DeSelectAll = new ToolStripMenuItem();
		ReverseSelected = new ToolStripMenuItem();
		CmbStatus = new ToolStripComboBox();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGPersonID = new DataGridViewTextBoxColumn();
		No = new DataGridViewTextBoxColumn();
		DGFullName = new DataGridViewTextBoxColumn();
		DateStartSHDGrid = new DataGridViewTextBoxColumn();
		DateFinishSHDGrid = new DataGridViewTextBoxColumn();
		GharardadStatus = new DataGridViewTextBoxColumn();
		GharardadStatusFa = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)gharardadBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnprintKol);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(915, 561);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnprintKol).AccessibleRole = (AccessibleRole)43;
		((Control)BtnprintKol).Anchor = (AnchorStyles)6;
		BtnprintKol.ColorTable = (eButtonColor)6;
		((Control)BtnprintKol).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnprintKol.Image = (Image)(object)Resources.printer_green;
		BtnprintKol.ImagePosition = (eImagePosition)1;
		((Control)BtnprintKol).Location = new Point(6, 524);
		((Control)BtnprintKol).Name = "BtnprintKol";
		((Control)BtnprintKol).Size = new Size(180, 33);
		((PopupItemControl)BtnprintKol).Style = (eDotNetBarStyle)9;
		((Control)BtnprintKol).TabIndex = 27;
		((Control)BtnprintKol).Text = "[چاپ قراردادهای منتخب]";
		((Control)BtnprintKol).Click += BtnPrint_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[8]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGPersonID,
			(DataGridViewColumn)No,
			(DataGridViewColumn)DGFullName,
			(DataGridViewColumn)DateStartSHDGrid,
			(DataGridViewColumn)DateFinishSHDGrid,
			(DataGridViewColumn)GharardadStatus,
			(DataGridViewColumn)GharardadStatusFa
		});
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
		((Control)DGrid).Location = new Point(6, 158);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(896, 362);
		((Control)DGrid).TabIndex = 1;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		gharardadBindingSource.DataSource = typeof(GharardadNew);
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.Class = "";
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(441, 526);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(342, 23);
		((Control)CountRows).TabIndex = 6;
		((Control)CountRows).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(789, 526);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(114, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[تعداد کل قراردادها]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)pictureBox1);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX6);
		((Control)GroupPanel1).Controls.Add((Control)(object)TbxToDate);
		((Control)GroupPanel1).Controls.Add((Control)(object)TbxFromDate);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX5);
		((Control)GroupPanel1).Controls.Add((Control)(object)ChkFromdate);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX4);
		((Control)GroupPanel1).Controls.Add((Control)(object)labelX3);
		((Control)GroupPanel1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TbxSearch);
		((Control)GroupPanel1).Location = new Point(6, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(896, 146);
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
		((PanelControl)GroupPanel1).Style.Class = "";
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.Class = "";
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.Class = "";
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		pictureBox1.Image = (Image)(object)Resources.helpinfo32;
		((Control)pictureBox1).Location = new Point(6, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(33, 30);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 27;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(802, 10);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(76, 23);
		((Control)labelX6).TabIndex = 26;
		((Control)labelX6).Text = "[تاریخ قرارداد]:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)TbxToDate).Anchor = (AnchorStyles)9;
		TbxToDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxToDate).Border.Class = "TextBoxBorder";
		((TextBoxX)TbxToDate).Border.CornerType = (eCornerType)1;
		TbxToDate.EnterToTab = true;
		TbxToDate.EscToClose = true;
		TbxToDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxToDate.FocusBackColor = Color.Yellow;
		TbxToDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxToDate.FocusForeColor = Color.Blue;
		TbxToDate.FocusTextSelect = true;
		((Control)TbxToDate).Font = new Font("Times New Roman", 12f);
		((Control)TbxToDate).Location = new Point(450, 11);
		TbxToDate.MessageEmptyShowDialog = false;
		TbxToDate.MessegeEmpty = "";
		TbxToDate.MessegeEmptyInFormRight = true;
		TbxToDate.MessegeEmptyShowInForm = false;
		TbxToDate.MiladiDate = new DateTime(2020, 7, 20, 0, 0, 0, 0);
		((Control)TbxToDate).Name = "TbxToDate";
		((Control)TbxToDate).RightToLeft = (RightToLeft)0;
		((Control)TbxToDate).Size = new Size(100, 26);
		((Control)TbxToDate).TabIndex = 25;
		((Control)TbxToDate).Text = "1399/04/30";
		((TextBox)TbxToDate).TextAlign = (HorizontalAlignment)2;
		TbxToDate.TootTipDelay = 500;
		((Control)TbxToDate).TextChanged += TbxToDate_TextChanged;
		((Control)TbxFromDate).Anchor = (AnchorStyles)9;
		TbxFromDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxFromDate).Border.Class = "TextBoxBorder";
		((TextBoxX)TbxFromDate).Border.CornerType = (eCornerType)1;
		TbxFromDate.EnterToTab = true;
		TbxFromDate.EscToClose = true;
		TbxFromDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxFromDate.FocusBackColor = Color.Yellow;
		TbxFromDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxFromDate.FocusForeColor = Color.Blue;
		TbxFromDate.FocusTextSelect = true;
		((Control)TbxFromDate).Font = new Font("Times New Roman", 12f);
		((Control)TbxFromDate).Location = new Point(624, 11);
		TbxFromDate.MessageEmptyShowDialog = false;
		TbxFromDate.MessegeEmpty = "";
		TbxFromDate.MessegeEmptyInFormRight = true;
		TbxFromDate.MessegeEmptyShowInForm = false;
		TbxFromDate.MiladiDate = new DateTime(2020, 7, 20, 0, 0, 0, 0);
		((Control)TbxFromDate).Name = "TbxFromDate";
		((Control)TbxFromDate).RightToLeft = (RightToLeft)0;
		((Control)TbxFromDate).Size = new Size(100, 26);
		((Control)TbxFromDate).TabIndex = 24;
		((Control)TbxFromDate).Text = "1399/04/30";
		((TextBox)TbxFromDate).TextAlign = (HorizontalAlignment)2;
		TbxFromDate.TootTipDelay = 500;
		((Control)TbxFromDate).TextChanged += TbxFromDate_TextChanged;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(556, 11);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(51, 21);
		((Control)labelX5).TabIndex = 23;
		((Control)labelX5).Text = "[ToDate] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)ChkFromdate).Anchor = (AnchorStyles)9;
		((Control)ChkFromdate).AutoSize = true;
		((Control)ChkFromdate).ForeColor = SystemColors.ControlText;
		((Control)ChkFromdate).Location = new Point(697, 13);
		((Control)ChkFromdate).Name = "ChkFromdate";
		((Control)ChkFromdate).Size = new Size(98, 17);
		((Control)ChkFromdate).TabIndex = 22;
		((Control)ChkFromdate).Text = "[از تاریخ] :";
		((ButtonBase)ChkFromdate).UseVisualStyleBackColor = true;
		ChkFromdate.CheckedChanged += ChkFromDate_CheckedChanged;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(802, 42);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(66, 23);
		((Control)labelX4).TabIndex = 20;
		((Control)labelX4).Text = "[جستجو]: ";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(802, 74);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(88, 23);
		((Control)labelX3).TabIndex = 17;
		((Control)labelX3).Text = "[وضعیت قرارداد]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)13;
		((Control)flowLayoutPanel1).AutoSize = true;
		((Control)flowLayoutPanel1).BackColor = Color.White;
		((Control)flowLayoutPanel1).Location = new Point(6, 69);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(789, 33);
		((Control)flowLayoutPanel1).TabIndex = 16;
		((Control)TbxSearch).Anchor = (AnchorStyles)13;
		TbxSearch.Border.Class = "TextBoxBorder";
		TbxSearch.Border.CornerType = (eCornerType)1;
		((Control)TbxSearch).Location = new Point(6, 43);
		((Control)TbxSearch).Name = "TbxSearch";
		((Control)TbxSearch).Size = new Size(789, 21);
		((Control)TbxSearch).TabIndex = 1;
		((Control)TbxSearch).TextChanged += TxtSearch_TextChanged;
		((ToolStripItem)SelectAllTSMI).Name = "SelectAllTSMI";
		((ToolStripItem)SelectAllTSMI).Size = new Size(32, 19);
		((ToolStripItem)UpSelectAllTSMI).Name = "UpSelectAllTSMI";
		((ToolStripItem)UpSelectAllTSMI).Size = new Size(32, 19);
		((ToolStripItem)ReverseSelectAllDetTSMI).Name = "ReverseSelectAllDetTSMI";
		((ToolStripItem)ReverseSelectAllDetTSMI).Size = new Size(32, 19);
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(663, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(58, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[4]
		{
			(ToolStripItem)Menu1,
			(ToolStripItem)SelectAll,
			(ToolStripItem)DeSelectAll,
			(ToolStripItem)ReverseSelected
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(212, 92);
		((Control)contextMenuStrip1).Text = "[منوی یک]";
		((ToolStripDropDown)contextMenuStrip1).Opening += contextMenuStrip1_Opening;
		((ToolStripItem)Menu1).Name = "Menu1";
		((ToolStripItem)Menu1).Size = new Size(211, 22);
		((ToolStripItem)Menu1).Text = "[تغییر وضعیت قراردادهای انتخاب شده]";
		((ToolStripItem)Menu1).Click += Menu1_Click;
		((ToolStripItem)SelectAll).Name = "SelectAll";
		((ToolStripItem)SelectAll).Size = new Size(211, 22);
		((ToolStripItem)SelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)SelectAll).Click += SelectAll_Click;
		((ToolStripItem)DeSelectAll).Name = "DeSelectAll";
		((ToolStripItem)DeSelectAll).Size = new Size(211, 22);
		((ToolStripItem)DeSelectAll).Text = "[پاک کردن همه]";
		((ToolStripItem)DeSelectAll).Click += DeSelectAll_Click;
		((ToolStripItem)ReverseSelected).Name = "ReverseSelected";
		((ToolStripItem)ReverseSelected).Size = new Size(211, 22);
		((ToolStripItem)ReverseSelected).Text = "[معکوس کردن انتخاب ها]";
		((ToolStripItem)ReverseSelected).Click += ReverseSelected_Click;
		((ToolStripItem)CmbStatus).Name = "CmbStatus";
		((ToolStripItem)CmbStatus).Size = new Size(121, 23);
		((ToolStripItem)CmbStatus).Text = "[وضعیت ها]";
		((DataGridViewColumn)DGCheck).DataPropertyName = "Check";
		((DataGridViewColumn)DGCheck).HeaderText = "  ";
		((DataGridViewColumn)DGCheck).MinimumWidth = 20;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)DGPersonID).DataPropertyName = "PersonID";
		((DataGridViewColumn)DGPersonID).HeaderText = "PersonID";
		((DataGridViewColumn)DGPersonID).Name = "DGPersonID";
		((DataGridViewBand)DGPersonID).ReadOnly = true;
		((DataGridViewBand)DGPersonID).Visible = false;
		((DataGridViewColumn)No).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)No).DataPropertyName = "No";
		val3.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)No).DefaultCellStyle = val3;
		((DataGridViewColumn)No).HeaderText = "[شماره]";
		((DataGridViewColumn)No).Name = "No";
		((DataGridViewBand)No).ReadOnly = true;
		((DataGridViewColumn)No).Width = 86;
		((DataGridViewColumn)DGFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DGFullName).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)DGFullName).MinimumWidth = 100;
		((DataGridViewColumn)DGFullName).Name = "DGFullName";
		((DataGridViewBand)DGFullName).ReadOnly = true;
		((DataGridViewColumn)DateStartSHDGrid).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateStartSHDGrid).DataPropertyName = "DateStartCalture";
		((DataGridViewColumn)DateStartSHDGrid).HeaderText = "[تاریخ شروع]";
		((DataGridViewColumn)DateStartSHDGrid).MinimumWidth = 60;
		((DataGridViewColumn)DateStartSHDGrid).Name = "DateStartSHDGrid";
		((DataGridViewBand)DateStartSHDGrid).ReadOnly = true;
		((DataGridViewColumn)DateStartSHDGrid).Width = 98;
		((DataGridViewColumn)DateFinishSHDGrid).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DateFinishSHDGrid).DataPropertyName = "DateFinishCalture";
		((DataGridViewColumn)DateFinishSHDGrid).HeaderText = "[تاریخ پایان]";
		((DataGridViewColumn)DateFinishSHDGrid).MinimumWidth = 60;
		((DataGridViewColumn)DateFinishSHDGrid).Name = "DateFinishSHDGrid";
		((DataGridViewBand)DateFinishSHDGrid).ReadOnly = true;
		((DataGridViewColumn)DateFinishSHDGrid).Width = 89;
		((DataGridViewColumn)GharardadStatus).DataPropertyName = "GharardadStatus";
		((DataGridViewColumn)GharardadStatus).HeaderText = "GharardadStatus";
		((DataGridViewColumn)GharardadStatus).Name = "GharardadStatus";
		((DataGridViewBand)GharardadStatus).ReadOnly = true;
		((DataGridViewBand)GharardadStatus).Visible = false;
		((DataGridViewColumn)GharardadStatusFa).HeaderText = "[وضعیت]";
		((DataGridViewColumn)GharardadStatusFa).Name = "GharardadStatusFa";
		((DataGridViewBand)GharardadStatusFa).ReadOnly = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(915, 561);
		((Control)this).ContextMenuStrip = contextMenuStrip1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "FrmEndedGharardad";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[قراردادهای در حال خاتمه یا خاتمه یافته]*";
		((Control)this).KeyDown += new KeyEventHandler(FrmEndedGharardad_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)gharardadBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel1).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
