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
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.Forms.Device;

public class FrmEventNotifyList : FrmExtList
{
	private ToolStripMenuItem _selectAllToolStripMenuItem;

	private ToolStripMenuItem _selectAllDetTsmi;

	private ToolStripMenuItem _unSelectAllDetTsmi;

	private ToolStripMenuItem _reverseSelectAllDetTsmi;

	private DateTime _date1 = ConstantValues.MinDateM;

	private DateTime _date2 = ConstantValues.MinDateM;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private List<EventArgsNotify> _listData;

	private SortableBindingList<EventArgsNotify> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal DataGridViewX DGrid;

	private LabelX LblRowCount;

	private BindingSource eventArgsNotifyBindingSource;

	private LabelX lblMessage;

	private LabelX LblTitle;

	private LabelX labelX2;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn DgDateSh;

	private DataGridViewTextBoxColumn DgDateM;

	private DataGridViewTextBoxColumn DGDesc;

	private DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FrmExtList)this).IsDisposed;
			}
			return true;
		}
	}

	public int ID
	{
		get
		{
			if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return 0;
			}
			return int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
		}
	}

	public EventArgsNotify EventNotifySelected
	{
		get
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null)
			{
				return new EventArgsNotify(((CacheBaseForm)this).ContextGuid);
			}
			return _listData.SingleOrDefault((EventArgsNotify q) => q.Id.Equals(ID));
		}
	}

	private List<int> ListEventNotifyId
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			List<int> list = new List<int>();
			if (((DataGridView)DGrid).RowCount <= 0)
			{
				return list;
			}
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
				{
					list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
				}
			}
			return list;
		}
	}

	private List<EventArgsNotify> SelectedListEventNotifies => (from gl in _listData
		join fi in ListEventNotifyId on gl.Id equals fi
		select gl).ToList();

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[پیام های سیستم]", out var value))
			{
				trDictionary.Add("[پیام های سیستم]", Utilities.GetTranslate("[پیام های سیستم]"));
			}
			if (!trDictionary.TryGetValue("[برای انتخاب همه راست کلیک و گزینه انتخاب]", out value))
			{
				trDictionary.Add("[برای انتخاب همه راست کلیک و گزینه انتخاب]", Utilities.GetTranslate("[برای انتخاب همه راست کلیک و گزینه انتخاب]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب همه]", out value))
			{
				trDictionary.Add("[انتخاب همه]", Utilities.GetTranslate("[انتخاب همه]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب]", out value))
			{
				trDictionary.Add("[انتخاب]", Utilities.GetTranslate("[انتخاب]"));
			}
			if (!trDictionary.TryGetValue("[پاک کردن همه]", out value))
			{
				trDictionary.Add("[پاک کردن همه]", Utilities.GetTranslate("[پاک کردن همه]"));
			}
			if (!trDictionary.TryGetValue("[معکوس کردن انتخاب ها]", out value))
			{
				trDictionary.Add("[معکوس کردن انتخاب ها]", Utilities.GetTranslate("[معکوس کردن انتخاب ها]"));
			}
			if (!trDictionary.TryGetValue("[تعداد رکوردها]", out value))
			{
				trDictionary.Add("[تعداد رکوردها]", Utilities.GetTranslate("[تعداد رکوردها]"));
			}
			if (!trDictionary.TryGetValue("[عنوان]", out value))
			{
				trDictionary.Add("[عنوان]", Utilities.GetTranslate("[عنوان]"));
			}
			if (!trDictionary.TryGetValue("[شرح]", out value))
			{
				trDictionary.Add("[شرح]", Utilities.GetTranslate("[شرح]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 60);
		}
	}

	public FrmEventNotifyList(string contextGuid, DateTime date1, DateTime date2, Font font)
		: base(contextGuid)
	{
		Task.Run(async delegate
		{
			await SetTrDictionaryAsync();
		});
		_date1 = date1;
		_date2 = date2;
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			((Control)this).Text = "پیام های سیستم";
			if (trDictionary.TryGetValue("[پیام های سیستم]", out var value))
			{
				((Control)this).Text = value;
			}
			((Control)labelX2).Visible = false;
			((DataGridViewBand)DGCheck).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 86);
		}
	}

	public FrmEventNotifyList(string contextGuid, bool pIsSelect, Font font)
		: base(contextGuid)
	{
		Task.Run(async delegate
		{
			await SetTrDictionaryAsync();
		});
		InitializeComponent();
		((Control)DGrid).Focus();
		try
		{
			((Control)DGrid).Font = font;
			((Control)base.BtnSelect).Visible = pIsSelect;
			((Control)this).Text = "پیام های سیستم";
			if (trDictionary.TryGetValue("[پیام های سیستم]", out var value))
			{
				((Control)this).Text = value;
			}
			LoadData();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 109);
		}
	}

	private void LoadData()
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		string value = "";
		try
		{
			ButtonX btnAdd = base.BtnAdd;
			ButtonX btnEdit = base.BtnEdit;
			bool flag2 = (((Control)base.BtnDelete).Enabled = false);
			bool enabled = (((Control)btnEdit).Enabled = flag2);
			((Control)btnAdd).Enabled = enabled;
			((ToolStripItem)base.StatusLabelMessage).Text = "برای انتخاب همه راست کلیک و گزینه انتخاب";
			if (trDictionary.TryGetValue("[برای انتخاب همه راست کلیک و گزینه انتخاب]", out value))
			{
				((ToolStripItem)base.StatusLabelMessage).Text = value;
			}
			((ToolStripItem)base.StatusLabelDel).Text = "";
			((ToolStripItem)base.StatusLabelModiFy).Text = "";
			((ToolStripItem)base.StatusLabelIns).Text = "";
			((Control)base.BtnSelect).Text = "انتخاب همه";
			if (trDictionary.TryGetValue("[انتخاب همه]", out value))
			{
				((Control)base.BtnSelect).Text = value;
			}
			_selectAllToolStripMenuItem = new ToolStripMenuItem();
			_selectAllDetTsmi = new ToolStripMenuItem();
			_unSelectAllDetTsmi = new ToolStripMenuItem();
			_reverseSelectAllDetTsmi = new ToolStripMenuItem();
			((ToolStrip)base.contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)_selectAllToolStripMenuItem });
			((ToolStripDropDownItem)_selectAllToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
			{
				(ToolStripItem)_selectAllDetTsmi,
				(ToolStripItem)_unSelectAllDetTsmi,
				(ToolStripItem)_reverseSelectAllDetTsmi
			});
			((ToolStripItem)_selectAllToolStripMenuItem).Name = "SelectAllToolStripMenuItem";
			((ToolStripItem)_selectAllToolStripMenuItem).Size = new Size(175, 22);
			((ToolStripItem)_selectAllToolStripMenuItem).Text = "انتخاب";
			if (trDictionary.TryGetValue("[انتخاب]", out value))
			{
				((ToolStripItem)_selectAllToolStripMenuItem).Text = value;
			}
			((ToolStripItem)_selectAllDetTsmi).Name = "SelectAllDetTSMI";
			((ToolStripItem)_selectAllDetTsmi).Size = new Size(176, 22);
			((ToolStripItem)_selectAllDetTsmi).Text = "انتخاب همه";
			if (trDictionary.TryGetValue("[انتخاب همه]", out value))
			{
				((ToolStripItem)_selectAllDetTsmi).Text = value;
			}
			((ToolStripItem)_selectAllDetTsmi).Click += _selectAllDetTsmi_Click;
			((ToolStripItem)_unSelectAllDetTsmi).Name = "UnSelectAllDetTSMI";
			((ToolStripItem)_unSelectAllDetTsmi).Size = new Size(176, 22);
			((ToolStripItem)_unSelectAllDetTsmi).Text = "انتخاب هیچ کدام";
			if (trDictionary.TryGetValue("[پاک کردن همه]", out value))
			{
				((ToolStripItem)_unSelectAllDetTsmi).Text = value;
			}
			((ToolStripItem)_unSelectAllDetTsmi).Click += _unSelectAllDetTsmi_Click;
			((ToolStripItem)_reverseSelectAllDetTsmi).Name = "ReverseSelectAllDetTSMI";
			((ToolStripItem)_reverseSelectAllDetTsmi).Size = new Size(176, 22);
			((ToolStripItem)_reverseSelectAllDetTsmi).Text = "معکوس کردن انتخاب";
			if (trDictionary.TryGetValue("[معکوس کردن انتخاب ها]", out value))
			{
				((ToolStripItem)_reverseSelectAllDetTsmi).Text = value;
			}
			((ToolStripItem)_reverseSelectAllDetTsmi).Click += _reverseSelectAllDetTsmi_Click;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 164);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			((FrmExtList)this).OnKeyDown(e);
			if ((int)e.KeyCode == 32)
			{
				e.Handled = true;
				if (((Control)DGrid).Focused && ID >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
				{
					ChangeValOfDGrid();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 218);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && ID >= 0)
			{
				ChangeValOfDGrid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 231);
		}
	}

	private void ChangeValOfDGrid()
	{
		try
		{
			if (((BaseCollection)((DataGridView)DGrid).SelectedRows).Count > 0)
			{
				DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChangeValOfDGrid", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 246);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			base.objDGrid = (Control)(object)DGrid;
			((FrmExtList)this).OnLoad(e);
			((FrmExtList)this).Edit_Click += new CRMEventHandler(BtnEdit_Click);
			((Control)DGrid).DoubleClick += base.DoubleClickInGrid;
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 269);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			_listData = (await EventArgsNotify.SelectAllAsync(((CacheBaseForm)this).ContextGuid, _date1, _date2, (EventNotifyEnumType)1, token)).OrderByDescending((EventArgsNotify or) => or.DateM).ToList();
			if (!token.IsCancellationRequested)
			{
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token2 = _searchToken.Token;
				await SearchAsync(token2);
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

	protected override void Search()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_searchString = Text.FixString(((Control)base.TxtSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			new Waiter("جستجو در لیست", (Form)(object)this, SearchAsync(token), false, (EnWaiterType)0, (Control)null, "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 303);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (_listData == null)
			{
				_listData = (await EventArgsNotify.SelectAllAsync(((CacheBaseForm)this).ContextGuid, _date1, _date2, (EventNotifyEnumType)1, token)).OrderByDescending((EventArgsNotify or) => or.DateM).ToList();
			}
			_listForDisplay = null;
			if (string.IsNullOrEmpty(_searchString))
			{
				FrmEventNotifyList frmEventNotifyList = this;
				List<EventArgsNotify> listData = _listData;
				frmEventNotifyList._listForDisplay = ((listData != null) ? Utilities.ToSortableBindingList<EventArgsNotify>((IEnumerable<EventArgsNotify>)listData, token) : null);
			}
			else
			{
				IEnumerable<EventArgsNotify> enumerable = _listData;
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					enumerable = enumerable.Where((EventArgsNotify p) => p.Title.Contains(word) || p.Message.Contains(word));
				}
				_listForDisplay = Utilities.ToSortableBindingList<EventArgsNotify>(enumerable, token);
			}
			string countRecordTr = "تعداد ردیف";
			int count = ((Collection<EventArgsNotify>)(object)_listForDisplay).Count;
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
						eventArgsNotifyBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = countRecordTr + " : " + count.ToString("N0");
						if (((DataGridView)DGrid).CurrentRow != null)
						{
							SetLabels(((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index);
						}
						else
						{
							SetLabels(-1);
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
			Log.Error(ex2, "");
		}
	}

	private void SetLabels(int index)
	{
		try
		{
			((Control)LblTitle).Text = "عنوان : ";
			((Control)lblMessage).Text = "توضیحات : ";
			if (trDictionary.TryGetValue("[عنوان]", out var value))
			{
				((Control)LblTitle).Text = value + " : ";
			}
			if (trDictionary.TryGetValue("[شرح]", out var value2))
			{
				((Control)lblMessage).Text = value2 + " : ";
			}
			if (index >= 0)
			{
				EventArgsNotify eventNotifySelected = EventNotifySelected;
				LabelX obj = lblMessage;
				((Control)obj).Text = ((Control)obj).Text + eventNotifySelected.Message;
				LabelX lblTitle = LblTitle;
				((Control)lblTitle).Text = ((Control)lblTitle).Text + eventNotifySelected.Title;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetLabels", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 378);
		}
	}

	private void BtnSelect_Click1(object sender, EventArgs e)
	{
		_selectAllDetTsmi_Click(sender, e);
	}

	private void _reverseSelectAllDetTsmi_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (EventArgsNotify item in (Collection<EventArgsNotify>)(object)_listForDisplay)
			{
				item.Check = !item.Check;
			}
			eventArgsNotifyBindingSource.DataSource = ((IEnumerable<EventArgsNotify>)_listForDisplay).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "_reverseSelectAllDetTsmi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 399);
		}
	}

	private void _unSelectAllDetTsmi_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (EventArgsNotify item in (Collection<EventArgsNotify>)(object)_listForDisplay)
			{
				item.Check = false;
			}
			eventArgsNotifyBindingSource.DataSource = ((IEnumerable<EventArgsNotify>)_listForDisplay).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "_unSelectAllDetTsmi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 412);
		}
	}

	private void _selectAllDetTsmi_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (EventArgsNotify item in (Collection<EventArgsNotify>)(object)_listForDisplay)
			{
				item.Check = true;
			}
			eventArgsNotifyBindingSource.DataSource = ((IEnumerable<EventArgsNotify>)_listForDisplay).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "_selectAllDetTsmi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 426);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)base.BtnEdit).Visible || !((Control)base.BtnEdit).Enabled)
		{
			return;
		}
		((Control)DGrid).Focus();
		try
		{
			if (ID > 0 && (int)(await ((CacheBaseForm)new FrmEventNotify(((CacheBaseForm)this).ContextGuid, ID)).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				_listData = await EventArgsNotify.SelectAllAsync(((CacheBaseForm)this).ContextGuid, _date1, _date2, (EventNotifyEnumType)1, token);
				((FrmExtList)this).Search();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 451);
		}
	}

	private async void FrmEventNotifyList_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			foreach (EventArgsNotify selectedListEventNotify in SelectedListEventNotifies)
			{
				selectedListEventNotify.StatusNotify = (EventNotifyEnumType)2;
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				await selectedListEventNotify.SaveAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmEventNotifyList_FormClosed", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FrmEventNotifyList.cs", 472);
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
				((FrmExtList)this).Dispose(disposing);
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
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Expected O, but got Unknown
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b03: Expected O, but got Unknown
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Expected O, but got Unknown
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		titleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGCheck = new DataGridViewCheckBoxColumn();
		DgDateSh = new DataGridViewTextBoxColumn();
		DgDateM = new DataGridViewTextBoxColumn();
		DGDesc = new DataGridViewTextBoxColumn();
		messageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		eventArgsNotifyBindingSource = new BindingSource(components);
		LblRowCount = new LabelX();
		LblTitle = new LabelX();
		lblMessage = new LabelX();
		labelX2 = new LabelX();
		((Control)base.PanelEx1).SuspendLayout();
		((Control)base.GroupPanel1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)eventArgsNotifyBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)base.PanelEx1).Controls.Add((Control)(object)lblMessage);
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblTitle);
		((Control)base.PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)base.PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)base.PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)base.PanelEx1).Size = new Size(784, 561);
		base.PanelEx1.Style.Alignment = (StringAlignment)1;
		base.PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		base.PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		base.PanelEx1.Style.Border = (eBorderType)1;
		base.PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		base.PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		base.PanelEx1.Style.GradientAngle = 90;
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnOther, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.GroupPanel1, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnEdit, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnAdd, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnDelete, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)base.BtnSelect, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)DGrid, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblRowCount, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)labelX2, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)LblTitle, 0);
		((Control)base.PanelEx1).Controls.SetChildIndex((Control)(object)lblMessage, 0);
		((PanelControl)base.GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)base.GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)base.GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)base.GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)base.GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)base.GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)base.GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)base.GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)base.GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)base.GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)base.GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)base.GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)base.GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)base.GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)base.LabelX1).Location = new Point(676, 3);
		((Control)base.LabelX1).Text = "[جستجو] :";
		base.TxtSearch.Border.Class = "TextBoxBorder";
		base.TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)base.TxtSearch).Size = new Size(664, 21);
		((Control)base.BtnSelect).Text = "[انتخاب] (Enter)";
		((Control)base.BtnSelect).Click += BtnSelect_Click1;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)titleDataGridViewTextBoxColumn,
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DgDateSh,
			(DataGridViewColumn)DgDateM,
			(DataGridViewColumn)DGDesc,
			(DataGridViewColumn)messageDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = eventArgsNotifyBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(147, 78);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(624, 400);
		((Control)DGrid).TabIndex = 0;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "Id";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).DataPropertyName = "Title";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).HeaderText = "Title";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).Name = "titleDataGridViewTextBoxColumn";
		((DataGridViewBand)titleDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)titleDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).DataPropertyName = "Check";
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).MinimumWidth = 20;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)DgDateSh).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgDateSh).DataPropertyName = "DateSh";
		((DataGridViewColumn)DgDateSh).HeaderText = "[تاریخ]";
		((DataGridViewColumn)DgDateSh).MinimumWidth = 50;
		((DataGridViewColumn)DgDateSh).Name = "DgDateSh";
		((DataGridViewBand)DgDateSh).ReadOnly = true;
		((DataGridViewColumn)DgDateSh).Width = 69;
		((DataGridViewColumn)DgDateM).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgDateM).DataPropertyName = "DateM";
		val3.Format = "HH:mm";
		val3.NullValue = null;
		((DataGridViewBand)DgDateM).DefaultCellStyle = val3;
		((DataGridViewColumn)DgDateM).HeaderText = "[زمان]";
		((DataGridViewColumn)DgDateM).MinimumWidth = 50;
		((DataGridViewColumn)DgDateM).Name = "DgDateM";
		((DataGridViewBand)DgDateM).ReadOnly = true;
		((DataGridViewColumn)DgDateM).Width = 70;
		((DataGridViewColumn)DGDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDesc).DataPropertyName = "NotificationEnumDesc";
		((DataGridViewColumn)DGDesc).HeaderText = "[عنوان]";
		((DataGridViewColumn)DGDesc).MinimumWidth = 150;
		((DataGridViewColumn)DGDesc).Name = "DGDesc";
		((DataGridViewBand)DGDesc).ReadOnly = true;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).DataPropertyName = "Message";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).Name = "messageDataGridViewTextBoxColumn";
		((DataGridViewBand)messageDataGridViewTextBoxColumn).ReadOnly = true;
		eventArgsNotifyBindingSource.DataSource = typeof(EventArgsNotify);
		((Control)LblRowCount).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblRowCount).Location = new Point(12, 455);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(127, 23);
		((Control)LblRowCount).TabIndex = 25;
		((Control)LblRowCount).Text = "labelX2";
		((Control)LblTitle).Anchor = (AnchorStyles)6;
		((BaseItemControl)LblTitle).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblTitle).DataBindings.Add(new Binding("Text", (object)eventArgsNotifyBindingSource, "Title", true));
		((Control)LblTitle).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)LblTitle).Location = new Point(147, 484);
		((Control)LblTitle).Name = "LblTitle";
		((Control)LblTitle).Size = new Size(624, 23);
		((Control)LblTitle).TabIndex = 26;
		((Control)LblTitle).Text = "labelX2";
		((Control)lblMessage).Anchor = (AnchorStyles)6;
		((BaseItemControl)lblMessage).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMessage).DataBindings.Add(new Binding("Text", (object)eventArgsNotifyBindingSource, "Message", true));
		((Control)lblMessage).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)lblMessage).Location = new Point(147, 513);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(624, 23);
		((Control)lblMessage).TabIndex = 27;
		((Control)lblMessage).Text = "labelX2";
		((Control)labelX2).Anchor = (AnchorStyles)6;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)labelX2).ForeColor = Color.Brown;
		((Control)labelX2).Location = new Point(13, 233);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(127, 68);
		((Control)labelX2).TabIndex = 25;
		((Control)labelX2).Text = "با تیک زدن ردیــف ها این اطلاع ها تبدیل به خوانده شــده مـیـشــود و دیــگر نمایش داده نخواهد شد";
		labelX2.TextLineAlignment = (StringAlignment)0;
		labelX2.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmEventNotifyList";
		((Control)this).Text = "لیست مراکز خدمت*";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmEventNotifyList_FormClosed);
		((Control)this).Controls.SetChildIndex((Control)(object)base.PanelEx1, 0);
		((Control)base.PanelEx1).ResumeLayout(false);
		((Control)base.PanelEx1).PerformLayout();
		((Control)base.GroupPanel1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)eventArgsNotifyBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
