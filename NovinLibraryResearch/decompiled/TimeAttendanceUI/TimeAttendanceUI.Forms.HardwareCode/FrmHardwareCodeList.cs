using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceUI.Forms.HardwareCode;

public class FrmHardwareCodeList : BaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _searchToken = new CancellationTokenSource();

	private CancellationTokenSource _tokenBindTaradod = new CancellationTokenSource();

	private CancellationTokenSource _loadDataToken;

	public short SelectedDeviceId;

	public string SelectedHardwareCode;

	public string SelectedDeviceName;

	private List<short> _lstIdDevice;

	private List<HardwareCodeCUSView> _dGridLists = new List<HardwareCodeCUSView>();

	private IEnumerable<HardwareCodeCUSView> _dGridListsForDisplay = Array.Empty<HardwareCodeCUSView>();

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private HardwareCodeCUSView selectedCodCus;

	private string _searchString;

	private IContainer components;

	internal PanelEx PanelEx1;

	private LabelX LblCount;

	private LabelX labelX2;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	internal ButtonX BtnprintKol;

	internal ButtonX BtnPrint;

	private BindingSource HardwareCodesBindingSource;

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

	internal DataGridViewX DGridDetail;

	private FlowLayoutPanel flowLayoutPanel1;

	private BindingSource bindingSourceTaradods;

	private DataGridViewTextBoxColumn DGdeviceName;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewTextBoxColumn DateTimeLogShWithTime;

	private DataGridViewTextBoxColumn personNoDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DeviceName;

	private DataGridViewTextBoxColumn PersonFullName;

	private DataGridViewTextBoxColumn DGhardwareCode;

	private DataGridViewTextBoxColumn DgDeviceId;

	private bool IsDisposed
	{
		get
		{
			if (!((BaseForm)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public HardwareCodeCUSView SelectedCodCus
	{
		get
		{
			return selectedCodCus;
		}
		private set
		{
			if (selectedCodCus != value)
			{
				selectedCodCus = value;
				_tokenBindTaradod?.Cancel();
				_tokenBindTaradod = new CancellationTokenSource();
				CancellationToken token = _tokenBindTaradod.Token;
				BindTaradodsAsync(token);
			}
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
			if (!trDictionary.TryGetValue("[آقای]", out var value))
			{
				trDictionary.Add("[آقای]", Utilities.GetTranslate("[آقای]"));
			}
			if (!trDictionary.TryGetValue("[خانم]", out value))
			{
				trDictionary.Add("[خانم]", Utilities.GetTranslate("[خانم]"));
			}
			if (!trDictionary.TryGetValue("[انتخاب کد سخت افزاری]", out value))
			{
				trDictionary.Add("[انتخاب کد سخت افزاری]", Utilities.GetTranslate("[انتخاب کد سخت افزاری]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 62);
		}
	}

	public FrmHardwareCodeList(string contextGuid, TimeAttendanceDAL.Dal.FLRPerson.Person person, List<short> lstIdDevice)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_lstIdDevice = lstIdDevice;
			SetTrDictionary();
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(person, lstIdDevice, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs");
			((Control)TxtSearch).Focus();
			((Control)BtnSelect).Visible = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 80);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 94);
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
				_lstIdDevice = null;
				_dGridLists = null;
				_dGridListsForDisplay = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 119);
		}
	}

	private async void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_searchString = Text.FixString(((Control)TxtSearch).Text) ?? "";
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			await Task.Delay(200);
			if (!_searchToken.IsCancellationRequested)
			{
				await Task.Run(() => SearchAsync(_searchToken.Token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 134);
		}
	}

	private async Task SearchAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_dGridListsForDisplay = _dGridLists?.ToList();
			if (!string.IsNullOrEmpty(_searchString))
			{
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					if (string.IsNullOrEmpty(word))
					{
						continue;
					}
					_dGridListsForDisplay = _dGridListsForDisplay?.Where(delegate(HardwareCodeCUSView p)
					{
						string deviceName = p.DeviceName;
						if (deviceName == null || !deviceName.Contains(word))
						{
							string fullName = p.FullName;
							if (fullName == null || !fullName.Contains(word))
							{
								return p.HardwareCode?.Contains(word) ?? false;
							}
						}
						return true;
					});
				}
			}
			List<string> list = new List<string>();
			foreach (CheckBox item in (ArrangedElementCollection)((Control)flowLayoutPanel1).Controls)
			{
				CheckBox val = item;
				if (val.Checked)
				{
					list.Add(((Control)val).Text);
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			if (list.Count > 0)
			{
				List<HardwareCodeCUSView> list2 = new List<HardwareCodeCUSView>();
				foreach (HardwareCodeCUSView item2 in _dGridListsForDisplay)
				{
					if (list.Contains(item2.DeviceName))
					{
						list2.Add(item2);
					}
				}
				_dGridListsForDisplay = list2;
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_dGridListsForDisplay = _dGridListsForDisplay?.OrderByDescending((HardwareCodeCUSView x) => x.HardwareCode);
			while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					BindingSource hardwareCodesBindingSource = HardwareCodesBindingSource;
					IEnumerable<HardwareCodeCUSView> dGridListsForDisplay = _dGridListsForDisplay;
					hardwareCodesBindingSource.DataSource = ((dGridListsForDisplay != null) ? Utilities.ToSortableBindingList<HardwareCodeCUSView>(dGridListsForDisplay, default(CancellationToken)) : null);
					((Control)LblCount).Text = _dGridListsForDisplay?.Count().ToString("N0") ?? "";
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 184);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 193);
		}
	}

	private async void FrmHardwareCode_KeyDown(object sender, KeyEventArgs e)
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
				return;
			}
			e.Handled = true;
			if (((Control)DGrid).Focused)
			{
				if (((Control)BtnSelect).Visible)
				{
					((PopupItemControl)BtnSelect).PerformClick();
				}
			}
			else
			{
				((Control)DGrid).Focus();
			}
		}
		else if ((int)keyCode != 40)
		{
			if ((int)keyCode == 123)
			{
				e.Handled = true;
				FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
				await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
				((Component)(object)frm).Dispose();
			}
		}
		else if (((Control)TxtSearch).Focused)
		{
			e.Handled = true;
			((Control)DGrid).Focus();
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		if (((Control)BtnSelect).Visible)
		{
			((PopupItemControl)BtnSelect).PerformClick();
		}
	}

	private void BtnSelect_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentRow != null)
			{
				SelectedDeviceName = ((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DeviceName).Index].Value.ToString();
				SelectedDeviceId = Convert.ToInt16(((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DgDeviceId).Index].Value.ToString());
				SelectedHardwareCode = ((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DGhardwareCode).Index].Value.ToString();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 254);
		}
	}

	private async Task LoadDataAsync(TimeAttendanceDAL.Dal.FLRPerson.Person person, List<short> lstIdDevice, CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (person == null)
			{
				person = new TimeAttendanceDAL.Dal.FLRPerson.Person(((CacheBaseForm)this).ContextGuid);
			}
			string jensiat = ((((PersonBase)person).TitleId == 1) ? "آقای " : "خانم ");
			if (trDictionary.TryGetValue("[آقای]", out var value) && trDictionary.TryGetValue("[خانم]", out var value2))
			{
				jensiat = ((((PersonBase)person).TitleId == 1) ? value : value2);
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
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)this).Text = "انتخاب کد سخت افزاری " + jensiat + " " + ((PersonBase)person).FullName;
						if (trDictionary.TryGetValue("[انتخاب کد سخت افزاری]", out var value3))
						{
							((Control)this).Text = value3 + " " + jensiat + " " + ((PersonBase)person).FullName;
						}
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
			_dGridLists = await HardwareCodeCUSView.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			_dGridLists = _dGridLists.Where((HardwareCodeCUSView p) => !lstIdDevice.Contains(p.DeviceId)).ToList();
			LoadDevicesNameAsync(token);
			SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 288);
		}
	}

	private async Task LoadDevicesNameAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			IEnumerable<string> enumerable = (await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid)).Select((DeviceInfo p) => p.DeviceName);
			List<Control> checkBoxList = new List<Control>();
			foreach (string item in enumerable)
			{
				CheckBox val = new CheckBox
				{
					Text = item,
					AutoSize = true,
					Checked = false
				};
				val.CheckedChanged += ChkDeviceType_CheckedChanged;
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				checkBoxList.Add((Control)(object)val);
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated && !IsDisposed && !token.IsCancellationRequested);
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)flowLayoutPanel1).Controls.Clear();
						((Control)flowLayoutPanel1).Controls.AddRange(checkBoxList.ToArray());
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "LoadDevicesNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 319);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDevicesNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 322);
		}
	}

	private async void ChkDeviceType_CheckedChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkDeviceType_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 380);
		}
	}

	private async Task BindTaradodsAsync(CancellationToken token)
	{
		try
		{
			do
			{
				await Task.Delay(500);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			List<LogOnPersonel> x = null;
			if (SelectedCodCus != null)
			{
				x = await LogOnPersonel.GetLastTaradodsAsync(((CacheBaseForm)this).ContextGuid, Utilities.ParseToInt(selectedCodCus.HardwareCode, 0), selectedCodCus.DeviceId, 17, null, token);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						bindingSourceTaradods.DataSource = Utilities.ToSortableBindingList<LogOnPersonel>((IEnumerable<LogOnPersonel>)x, default(CancellationToken));
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BindTaradodsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmHardwareCodeList.cs", 410);
		}
	}

	private void HardwareCodesBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedCodCus = (HardwareCodeCUSView)HardwareCodesBindingSource.Current;
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
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
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
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Expected O, but got Unknown
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Expected O, but got Unknown
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Expected O, but got Unknown
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea0: Expected O, but got Unknown
		//IL_1366: Unknown result type (might be due to invalid IL or missing references)
		//IL_1370: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		DataGridViewCellStyle val7 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmHardwareCodeList));
		PanelEx1 = new PanelEx();
		DGridDetail = new DataGridViewX();
		DateTimeLogShWithTime = new DataGridViewTextBoxColumn();
		personNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		bindingSourceTaradods = new BindingSource(components);
		BtnSelect = new ButtonX();
		DGrid = new DataGridViewX();
		HardwareCodesBindingSource = new BindingSource(components);
		BtnCopyGharardads = new ButtonX();
		BtnPrint = new ButtonX();
		BtnprintKol = new ButtonX();
		LblCount = new LabelX();
		labelX2 = new LabelX();
		GroupPanel1 = new GroupPanel();
		flowLayoutPanel1 = new FlowLayoutPanel();
		TxtSearch = new TextBoxX();
		BtnCopyGharardadsFromSample = new ButtonX();
		ToolItemSelect = new ToolStripMenuItem();
		ToolItemModify = new ToolStripMenuItem();
		ToolItemAdd = new ToolStripMenuItem();
		ToolItemDelete = new ToolStripMenuItem();
		ToolItemCancelSearch = new ToolStripMenuItem();
		ToolItemSelectAll = new ToolStripMenuItem();
		SelectAllTSMI = new ToolStripMenuItem();
		UpSelectAllTSMI = new ToolStripMenuItem();
		ReverseSelectAllDetTSMI = new ToolStripMenuItem();
		LabelX1 = new LabelX();
		DeviceName = new DataGridViewTextBoxColumn();
		PersonFullName = new DataGridViewTextBoxColumn();
		DGhardwareCode = new DataGridViewTextBoxColumn();
		DgDeviceId = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGridDetail).BeginInit();
		((ISupportInitialize)bindingSourceTaradods).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)HardwareCodesBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)PanelEx1).Anchor = (AnchorStyles)15;
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)DGridDetail);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnSelect);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCopyGharardads);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnprintKol);
		((Control)PanelEx1).Controls.Add((Control)(object)LblCount);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnCopyGharardadsFromSample);
		PanelEx1.DisabledBackColor = Color.Empty;
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
		((DataGridView)DGridDetail).AllowUserToAddRows = false;
		((DataGridView)DGridDetail).AllowUserToDeleteRows = false;
		((DataGridView)DGridDetail).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridDetail).AlternatingRowsDefaultCellStyle = val;
		((Control)DGridDetail).Anchor = (AnchorStyles)7;
		((DataGridView)DGridDetail).AutoGenerateColumns = false;
		val2.Alignment = (DataGridViewContentAlignment)32;
		val2.BackColor = SystemColors.Control;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.WindowText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)1;
		((DataGridView)DGridDetail).ColumnHeadersDefaultCellStyle = val2;
		((DataGridView)DGridDetail).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridDetail).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DateTimeLogShWithTime,
			(DataGridViewColumn)personNoDataGridViewTextBoxColumn
		});
		((Control)DGridDetail).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGridDetail).DataSource = bindingSourceTaradods;
		val3.Alignment = (DataGridViewContentAlignment)16;
		val3.BackColor = SystemColors.Window;
		val3.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val3.ForeColor = SystemColors.ControlText;
		val3.SelectionBackColor = SystemColors.Highlight;
		val3.SelectionForeColor = SystemColors.ControlText;
		val3.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridDetail).DefaultCellStyle = val3;
		((DataGridView)DGridDetail).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridDetail).Location = new Point(5, 111);
		((Control)DGridDetail).MinimumSize = new Size(140, 416);
		((DataGridView)DGridDetail).MultiSelect = false;
		((Control)DGridDetail).Name = "DGridDetail";
		((DataGridView)DGridDetail).ReadOnly = true;
		((Control)DGridDetail).RightToLeft = (RightToLeft)0;
		((DataGridView)DGridDetail).RowHeadersVisible = false;
		((DataGridView)DGridDetail).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridDetail).Size = new Size(140, 416);
		((Control)DGridDetail).TabIndex = 24;
		((DataGridViewColumn)DateTimeLogShWithTime).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DateTimeLogShWithTime).DataPropertyName = "DatetimeCalture";
		val4.Alignment = (DataGridViewContentAlignment)32;
		((DataGridViewBand)DateTimeLogShWithTime).DefaultCellStyle = val4;
		((DataGridViewColumn)DateTimeLogShWithTime).HeaderText = "[تردد های اخیر شخص]";
		((DataGridViewColumn)DateTimeLogShWithTime).Name = "DateTimeLogShWithTime";
		((DataGridViewBand)DateTimeLogShWithTime).ReadOnly = true;
		((DataGridViewBand)DateTimeLogShWithTime).Resizable = (DataGridViewTriState)2;
		((DataGridViewColumn)personNoDataGridViewTextBoxColumn).DataPropertyName = "PersonNo";
		((DataGridViewColumn)personNoDataGridViewTextBoxColumn).HeaderText = "[شماره پرسنلی]";
		((DataGridViewColumn)personNoDataGridViewTextBoxColumn).Name = "personNoDataGridViewTextBoxColumn";
		((DataGridViewBand)personNoDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)personNoDataGridViewTextBoxColumn).Visible = false;
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).Size = new Size(61, 4);
		bindingSourceTaradods.DataSource = typeof(LogOnPersonel);
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = (eButtonColor)6;
		((Control)BtnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelect).Location = new Point(5, 27);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(140, 78);
		((Control)BtnSelect).TabIndex = 19;
		((Control)BtnSelect).TabStop = false;
		((Control)BtnSelect).Text = "[انتخاب] (Enter):";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val5.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val5;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		val6.Alignment = (DataGridViewContentAlignment)32;
		val6.BackColor = SystemColors.Control;
		val6.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val6.ForeColor = SystemColors.WindowText;
		val6.SelectionBackColor = SystemColors.Highlight;
		val6.SelectionForeColor = SystemColors.HighlightText;
		val6.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).ColumnHeadersDefaultCellStyle = val6;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[4]
		{
			(DataGridViewColumn)DeviceName,
			(DataGridViewColumn)PersonFullName,
			(DataGridViewColumn)DGhardwareCode,
			(DataGridViewColumn)DgDeviceId
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGrid).DataSource = HardwareCodesBindingSource;
		val7.Alignment = (DataGridViewContentAlignment)16;
		val7.BackColor = SystemColors.Window;
		val7.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val7.ForeColor = SystemColors.ControlText;
		val7.SelectionBackColor = SystemColors.Highlight;
		val7.SelectionForeColor = SystemColors.ControlText;
		val7.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val7;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(151, 148);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(623, 379);
		((Control)DGrid).TabIndex = 1;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		HardwareCodesBindingSource.DataSource = typeof(HardwareCodeCUSView);
		HardwareCodesBindingSource.CurrentChanged += HardwareCodesBindingSource_CurrentChanged;
		((Control)BtnCopyGharardads).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCopyGharardads).Location = new Point(0, 0);
		((Control)BtnCopyGharardads).Name = "BtnCopyGharardads";
		((Control)BtnCopyGharardads).Size = new Size(0, 0);
		((Control)BtnCopyGharardads).TabIndex = 20;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		((Control)BtnPrint).Location = new Point(0, 0);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(0, 0);
		((Control)BtnPrint).TabIndex = 21;
		((Control)BtnprintKol).AccessibleRole = (AccessibleRole)43;
		((Control)BtnprintKol).Location = new Point(0, 0);
		((Control)BtnprintKol).Name = "BtnprintKol";
		((Control)BtnprintKol).Size = new Size(0, 0);
		((Control)BtnprintKol).TabIndex = 22;
		((Control)LblCount).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCount).ForeColor = Color.DarkRed;
		((Control)LblCount).Location = new Point(526, 532);
		((Control)LblCount).Name = "LblCount";
		((Control)LblCount).RightToLeft = (RightToLeft)1;
		((Control)LblCount).Size = new Size(81, 23);
		((Control)LblCount).TabIndex = 6;
		((Control)LblCount).Text = "LblCount";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(621, 532);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(153, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[تعداد کل کدهای سخت افزاری]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(150, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(623, 136);
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
		GroupPanel1.TitleImage = (Image)componentResourceManager.GetObject("GroupPanel1.TitleImage");
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)13;
		((ScrollableControl)flowLayoutPanel1).AutoScroll = true;
		((Panel)flowLayoutPanel1).AutoSizeMode = (AutoSizeMode)0;
		((Control)flowLayoutPanel1).BackColor = Color.White;
		((Control)flowLayoutPanel1).Location = new Point(3, 31);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(611, 72);
		((Control)flowLayoutPanel1).TabIndex = 16;
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(611, 20);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)BtnCopyGharardadsFromSample).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCopyGharardadsFromSample).Location = new Point(0, 0);
		((Control)BtnCopyGharardadsFromSample).Name = "BtnCopyGharardadsFromSample";
		((Control)BtnCopyGharardadsFromSample).Size = new Size(0, 0);
		((Control)BtnCopyGharardadsFromSample).TabIndex = 23;
		((ToolStripItem)ToolItemSelect).Name = "ToolItemSelect";
		((ToolStripItem)ToolItemSelect).Size = new Size(32, 19);
		((ToolStripItem)ToolItemModify).Name = "ToolItemModify";
		((ToolStripItem)ToolItemModify).Size = new Size(32, 19);
		((ToolStripItem)ToolItemAdd).Name = "ToolItemAdd";
		((ToolStripItem)ToolItemAdd).Size = new Size(32, 19);
		((ToolStripItem)ToolItemDelete).Name = "ToolItemDelete";
		((ToolStripItem)ToolItemDelete).Size = new Size(32, 19);
		((ToolStripItem)ToolItemCancelSearch).Name = "ToolItemCancelSearch";
		((ToolStripItem)ToolItemCancelSearch).Size = new Size(32, 19);
		((ToolStripItem)ToolItemSelectAll).Name = "ToolItemSelectAll";
		((ToolStripItem)ToolItemSelectAll).Size = new Size(32, 19);
		((ToolStripItem)SelectAllTSMI).Name = "SelectAllTSMI";
		((ToolStripItem)SelectAllTSMI).Size = new Size(32, 19);
		((ToolStripItem)UpSelectAllTSMI).Name = "UpSelectAllTSMI";
		((ToolStripItem)UpSelectAllTSMI).Size = new Size(32, 19);
		((ToolStripItem)ReverseSelectAllDetTSMI).Name = "ReverseSelectAllDetTSMI";
		((ToolStripItem)ReverseSelectAllDetTSMI).Size = new Size(32, 19);
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(663, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(58, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[انتخاب]  :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((DataGridViewColumn)DeviceName).DataPropertyName = "DeviceName";
		((DataGridViewColumn)DeviceName).HeaderText = "نام دستگاه";
		((DataGridViewColumn)DeviceName).MinimumWidth = 50;
		((DataGridViewColumn)DeviceName).Name = "DeviceName";
		((DataGridViewBand)DeviceName).ReadOnly = true;
		((DataGridViewColumn)PersonFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)PersonFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)PersonFullName).HeaderText = "نام شخص";
		((DataGridViewColumn)PersonFullName).MinimumWidth = 100;
		((DataGridViewColumn)PersonFullName).Name = "PersonFullName";
		((DataGridViewBand)PersonFullName).ReadOnly = true;
		((DataGridViewColumn)DGhardwareCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGhardwareCode).DataPropertyName = "HardwareCode";
		((DataGridViewColumn)DGhardwareCode).HeaderText = "[کد سخت افزاری]";
		((DataGridViewColumn)DGhardwareCode).MinimumWidth = 50;
		((DataGridViewColumn)DGhardwareCode).Name = "DGhardwareCode";
		((DataGridViewBand)DGhardwareCode).ReadOnly = true;
		((DataGridViewColumn)DGhardwareCode).Width = 127;
		((DataGridViewColumn)DgDeviceId).DataPropertyName = "DeviceId";
		((DataGridViewColumn)DgDeviceId).HeaderText = "[شناسه دستگاه]";
		((DataGridViewColumn)DgDeviceId).Name = "DgDeviceId";
		((DataGridViewBand)DgDeviceId).ReadOnly = true;
		((DataGridViewBand)DgDeviceId).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Name = "FrmHardwareCodeList";
		((Control)this).Text = "[کد سخت افزاری پرسنل]*";
		((Control)this).KeyDown += new KeyEventHandler(FrmHardwareCode_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGridDetail).EndInit();
		((ISupportInitialize)bindingSourceTaradods).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)HardwareCodesBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
