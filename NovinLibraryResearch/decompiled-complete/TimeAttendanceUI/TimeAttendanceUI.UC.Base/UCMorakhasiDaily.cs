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
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UCMorakhasiDaily : UserControl, IHasContext
{
	public delegate void Exit_UserControlEventHandler();

	private bool _isDisposed;

	private HashSet<string> _selectedTypesName;

	private List<Morakhasidaily> _listData;

	private SortableBindingList<Morakhasidaily> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private DateTime _Date1 = DateTime.Now;

	private DateTime _Date2 = DateTime.Now;

	private List<int> _personIDs = new List<int>();

	private int _PersonID;

	private List<Morakhasidaily> _lstMorakhasidaily = new List<Morakhasidaily>();

	private IContainer components;

	internal PanelEx PanelEx1;

	private LabelX LblRowCount;

	private LabelX labelX2;

	internal DataGridViewX DGrid;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	private ButtonX BtnPrint;

	private BindingSource morakhasidailyBindingSource;

	private FlowLayoutPanel flowLayoutPanel1;

	private Label label1;

	private TextBox TbxSearch;

	private DataGridViewTextBoxColumn DgFullName;

	private DataGridViewTextBoxColumn DgTypeMorakhasi;

	private DataGridViewTextBoxColumn DgId;

	private DataGridViewCheckBoxColumn DG_Check;

	private DataGridViewTextBoxColumn PersonFullName;

	private DataGridViewTextBoxColumn DgDateCalture;

	private DataGridViewTextBoxColumn dowDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TypeMorakhasiName;

	private DataGridViewTextBoxColumn DgDesc;

	private DataGridViewCheckBoxColumn HasHoghogh;

	private DataGridViewCheckBoxColumn HasBimeh;

	private DataGridViewCheckBoxColumn HasMaliyat;

	public string ContextGuid { get; set; }

	public bool isCalcSaati { get; set; }

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

	public Morakhasidaily SelectedMorakhasy { get; private set; }

	public event Exit_UserControlEventHandler Exit_UserControl;

	public UCMorakhasiDaily(string contextGuid)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		InitializeComponent();
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Control)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 58);
		}
	}

	private async Task LoadDataTypeAsync(CancellationToken token)
	{
		try
		{
			IEnumerable<TypeMorakhasi> enumerable = (await TypeMorakhasi.GetAllAsync(ContextGuid, token)).Distinct();
			_selectedTypesName = new HashSet<string>();
			((Control)flowLayoutPanel1).Controls.Clear();
			foreach (TypeMorakhasi item in enumerable)
			{
				CheckBox val = new CheckBox
				{
					Text = item.Name,
					Checked = true
				};
				val.CheckedChanged += ChkMorkhasiType_CheckedChanged;
				((Control)flowLayoutPanel1).Controls.Add((Control)(object)val);
				_selectedTypesName.Add(item.Name);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 80);
		}
	}

	private void ChkMorkhasiType_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		try
		{
			CheckBox val = (CheckBox)sender;
			HashSet<string> selectedTypesName = _selectedTypesName;
			if (selectedTypesName != null && selectedTypesName.Any())
			{
				if (val.Checked)
				{
					if (!_selectedTypesName.Contains(((Control)val).Text))
					{
						_selectedTypesName.Add(((Control)val).Text);
					}
				}
				else if (_selectedTypesName.Contains(((Control)val).Text))
				{
					_selectedTypesName.Remove(((Control)val).Text);
				}
			}
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			Search(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkMorkhasiType_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 103);
		}
	}

	private Task LoadDataAsync(CancellationToken token)
	{
		return LoadDataAsync(null, null, null, null, token);
	}

	public async Task LoadDataAsync(List<int>? personIdList, DateTime? date1, DateTime? date2, string? desc, CancellationToken token)
	{
		_ = 2;
		try
		{
			_ = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).FontProgram;
			if (date1.HasValue)
			{
				_Date1 = date1.Value;
			}
			else
			{
				date1 = DateTime.MinValue;
			}
			if (date2.HasValue)
			{
				_Date2 = date2.Value;
			}
			else
			{
				date2 = DateTime.Now;
			}
			if (personIdList != null)
			{
				_personIDs = personIdList;
			}
			else
			{
				personIdList = new List<int>();
			}
			if (_personIDs != null && _personIDs.Count == 1)
			{
				_PersonID = _personIDs[0];
			}
			if (desc != null)
			{
				_searchString = desc;
			}
			else
			{
				desc = "";
			}
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				_lstMorakhasidaily = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, _personIDs, _Date1, _Date2, _searchString, token);
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					_searchToken?.Cancel();
					_searchToken = new CancellationTokenSource();
					CancellationToken searchToken = _searchToken.Token;
					await LoadDataTypeAsync(token);
					Search(searchToken);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 132);
		}
	}

	private void Search(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			IEnumerable<Morakhasidaily> enumerable = _lstMorakhasidaily;
			if (!string.IsNullOrEmpty(_searchString))
			{
				foreach (string word in Text.SplitString(_searchString, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
				{
					enumerable = enumerable.Where((Morakhasidaily p) => p.Desc.Contains(word) || p.PersonFullName.Contains(word) || p.DateCalture.Contains(word));
				}
			}
			HashSet<string> selectedTypesName = _selectedTypesName;
			if (selectedTypesName != null && selectedTypesName.Any())
			{
				enumerable = enumerable.Where((Morakhasidaily p) => _selectedTypesName.Contains(p.TypeMorakhasiName));
			}
			_listForDisplay = Utilities.ToSortableBindingList<Morakhasidaily>(enumerable, token);
			int count = ((Collection<Morakhasidaily>)(object)_listForDisplay).Count;
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
						morakhasidailyBindingSource.DataSource = _listForDisplay;
						((Control)LblRowCount).Text = count.ToString();
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
		_ = 1;
		try
		{
			if (((Control)BtnAdd).Enabled && ((Control)BtnAdd).Visible)
			{
				FrmMorakhasiDaily frm = new FrmMorakhasiDaily(ContextGuid, DateTime.Today, _PersonID);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_searchToken?.Cancel();
					_searchToken = new CancellationTokenSource();
					CancellationToken token = _searchToken.Token;
					_lstMorakhasidaily = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, _personIDs, _Date1, _Date2, _searchString, token);
					Search(token);
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 190);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
			{
				return;
			}
			DataGridViewRow currentRow = ((DataGridView)DGrid).CurrentRow;
			object obj = ((currentRow != null) ? currentRow.Cells[((DataGridViewBand)DgDateCalture).Index].Value : null);
			if (obj != null)
			{
				Utilities.CalendarToMiladi(obj.ToString());
				FrmMorakhasiDaily frm = new FrmMorakhasiDaily(ContextGuid, SelectedMorakhasy, readOnly: false);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					_searchToken?.Cancel();
					_searchToken = new CancellationTokenSource();
					CancellationToken token = _searchToken.Token;
					_lstMorakhasidaily = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, _personIDs, _Date1, _Date2, _searchString, token);
					Search(token);
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 213);
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
			if (SelectedMorakhasy != null && (int)FMessageBox.Show("مایل به حذف مرخصی شخص انتخاب شده هستید ؟", "هشدار", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				await Morakhasidaily.DeleteAsync(ContextGuid, SelectedMorakhasy);
				_searchToken?.Cancel();
				_searchToken = new CancellationTokenSource();
				CancellationToken token = _searchToken.Token;
				_lstMorakhasidaily = await Morakhasidaily.GetSpecificDataAsync(ContextGuid, _personIDs, _Date1, _Date2, _searchString, token);
				Search(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 233);
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
			List<object> list = new List<object>();
			IEnumerable<Morakhasidaily> enumerable = ((IEnumerable<Morakhasidaily>)_listForDisplay).Where((Morakhasidaily p) => p.IsChecked);
			if (enumerable == null || enumerable.Count() <= 0)
			{
				enumerable = (IEnumerable<Morakhasidaily>)_listForDisplay;
			}
			foreach (Morakhasidaily item in enumerable)
			{
				list.Add(new
				{
					CodeTafsil = item.PersonCodeTafsil,
					Date = Calendar.MiladiToCalture(item.DateFrom),
					Dow = item.Dow,
					TypeMorakhasyName = item.TypeMorakhasiName,
					Desc = item.Desc,
					FullName = item.PersonFullName
				});
			}
			if (list.Count <= 0)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[داده ای برای چاپ وجود ندارد]", "", 3);
				return;
			}
			FrmPrint frm = new FrmPrint(ContextGuid, (EnumPrintShow)12, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 265);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	public void Print()
	{
		((PopupItemControl)BtnPrint).PerformClick();
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 32)
			{
				((DataGridView)DGrid).CurrentRow.Cells[((DataGridViewBand)DG_Check).Index].Value = !SelectedMorakhasy.IsChecked;
				e.Handled = true;
			}
			else if ((int)e.KeyCode == 13)
			{
				((PopupItemControl)BtnEdit).PerformClick();
				e.Handled = true;
			}
			else if (e.Control && (int)e.KeyCode == 70)
			{
				((Control)TbxSearch).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 289);
		}
	}

	private void TbxSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_searchString = Text.FixString(((Control)TbxSearch).Text).Trim();
			_searchToken?.Cancel();
			_searchToken = new CancellationTokenSource();
			CancellationToken token = _searchToken.Token;
			Search(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TbxSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 331);
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
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void morakhasidailyBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedMorakhasy = (Morakhasidaily)morakhasidailyBindingSource.Current;
	}

	private void DGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		try
		{
			if (SelectedMorakhasy != null)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellMouseClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCMorakhasiDaily.cs", 368);
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Expected O, but got Unknown
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Expected O, but got Unknown
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Expected O, but got Unknown
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e93: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		flowLayoutPanel1 = new FlowLayoutPanel();
		label1 = new Label();
		TbxSearch = new TextBox();
		BtnPrint = new ButtonX();
		LblRowCount = new LabelX();
		labelX2 = new LabelX();
		DGrid = new DataGridViewX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		DG_Check = new DataGridViewCheckBoxColumn();
		PersonFullName = new DataGridViewTextBoxColumn();
		DgDateCalture = new DataGridViewTextBoxColumn();
		TypeMorakhasiName = new DataGridViewTextBoxColumn();
		HasHoghogh = new DataGridViewCheckBoxColumn();
		HasBimeh = new DataGridViewCheckBoxColumn();
		HasMaliyat = new DataGridViewCheckBoxColumn();
		DgId = new DataGridViewTextBoxColumn();
		dowDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DgDesc = new DataGridViewTextBoxColumn();
		morakhasidailyBindingSource = new BindingSource(components);
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)morakhasidailyBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)label1);
		((Control)PanelEx1).Controls.Add((Control)(object)TbxSearch);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(1032, 461);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)13;
		((Control)flowLayoutPanel1).AutoSize = true;
		((Control)flowLayoutPanel1).BackColor = Color.White;
		((Control)flowLayoutPanel1).Location = new Point(162, 43);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(863, 54);
		((Control)flowLayoutPanel1).TabIndex = 15;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(917, 19);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(49, 13);
		((Control)label1).TabIndex = 14;
		((Control)label1).Text = "جستجو:";
		((Control)TbxSearch).Anchor = (AnchorStyles)13;
		((Control)TbxSearch).Location = new Point(162, 15);
		((Control)TbxSearch).Name = "TbxSearch";
		((Control)TbxSearch).Size = new Size(749, 21);
		((Control)TbxSearch).TabIndex = 13;
		((Control)TbxSearch).TextChanged += TbxSearch_TextChanged;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(7, 132);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(149, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 9;
		((Control)BtnPrint).Text = "[چاپ](Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)LblRowCount).Anchor = (AnchorStyles)14;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).ForeColor = Color.DarkRed;
		((Control)LblRowCount).Location = new Point(162, 428);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(724, 23);
		((Control)LblRowCount).TabIndex = 11;
		((Control)LblRowCount).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(892, 426);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(133, 23);
		((Control)labelX2).TabIndex = 10;
		((Control)labelX2).Text = "[تعداد کل مرخصی روزانه] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[10]
		{
			(DataGridViewColumn)DgId,
			(DataGridViewColumn)DG_Check,
			(DataGridViewColumn)PersonFullName,
			(DataGridViewColumn)DgDateCalture,
			(DataGridViewColumn)dowDataGridViewTextBoxColumn,
			(DataGridViewColumn)TypeMorakhasiName,
			(DataGridViewColumn)DgDesc,
			(DataGridViewColumn)HasHoghogh,
			(DataGridViewColumn)HasBimeh,
			(DataGridViewColumn)HasMaliyat
		});
		((DataGridView)DGrid).DataSource = morakhasidailyBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(162, 103);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(863, 317);
		((Control)DGrid).TabIndex = 5;
		((DataGridView)DGrid).CellMouseClick += new DataGridViewCellMouseEventHandler(DGrid_CellMouseClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(7, 93);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(149, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 8;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(7, 54);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(149, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 7;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(7, 15);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(149, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 6;
		((Control)BtnAdd).Text = "[اضافه](Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridViewColumn)DG_Check).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_Check).DataPropertyName = "IsChecked";
		((DataGridViewColumn)DG_Check).HeaderText = "";
		((DataGridViewColumn)DG_Check).MinimumWidth = 10;
		((DataGridViewColumn)DG_Check).Name = "DG_Check";
		((DataGridViewBand)DG_Check).ReadOnly = true;
		((DataGridViewColumn)DG_Check).Width = 10;
		((DataGridViewColumn)PersonFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)PersonFullName).DataPropertyName = "PersonFullName";
		((DataGridViewColumn)PersonFullName).HeaderText = "نام شخص";
		((DataGridViewColumn)PersonFullName).MinimumWidth = 150;
		((DataGridViewColumn)PersonFullName).Name = "PersonFullName";
		((DataGridViewBand)PersonFullName).ReadOnly = true;
		((DataGridViewColumn)PersonFullName).Width = 150;
		((DataGridViewColumn)DgDateCalture).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgDateCalture).DataPropertyName = "DateCalture";
		((DataGridViewColumn)DgDateCalture).HeaderText = "[تاریخ]";
		((DataGridViewColumn)DgDateCalture).MinimumWidth = 100;
		((DataGridViewColumn)DgDateCalture).Name = "DgDateCalture";
		((DataGridViewBand)DgDateCalture).ReadOnly = true;
		((DataGridViewColumn)TypeMorakhasiName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TypeMorakhasiName).DataPropertyName = "TypeMorakhasiName";
		((DataGridViewColumn)TypeMorakhasiName).HeaderText = "نوع مرخصی";
		((DataGridViewColumn)TypeMorakhasiName).MinimumWidth = 100;
		((DataGridViewColumn)TypeMorakhasiName).Name = "TypeMorakhasiName";
		((DataGridViewBand)TypeMorakhasiName).ReadOnly = true;
		((DataGridViewColumn)HasHoghogh).DataPropertyName = "HasHoghogh";
		((DataGridViewColumn)HasHoghogh).HeaderText = "حقوق";
		((DataGridViewColumn)HasHoghogh).Name = "HasHoghogh";
		((DataGridViewBand)HasHoghogh).ReadOnly = true;
		((DataGridViewColumn)HasBimeh).DataPropertyName = "HasBimeh";
		((DataGridViewColumn)HasBimeh).HeaderText = "بیمه";
		((DataGridViewColumn)HasBimeh).Name = "HasBimeh";
		((DataGridViewBand)HasBimeh).ReadOnly = true;
		((DataGridViewColumn)HasMaliyat).DataPropertyName = "HasMaliyat";
		((DataGridViewColumn)HasMaliyat).HeaderText = "مالیات";
		((DataGridViewColumn)HasMaliyat).Name = "HasMaliyat";
		((DataGridViewBand)HasMaliyat).ReadOnly = true;
		((DataGridViewColumn)DgId).DataPropertyName = "Id";
		((DataGridViewColumn)DgId).HeaderText = "[شناسه]";
		((DataGridViewColumn)DgId).Name = "DgId";
		((DataGridViewBand)DgId).ReadOnly = true;
		((DataGridViewBand)DgId).Visible = false;
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).DataPropertyName = "Dow";
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).HeaderText = "[روز هفته]";
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).MinimumWidth = 80;
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).Name = "dowDataGridViewTextBoxColumn";
		((DataGridViewBand)dowDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)dowDataGridViewTextBoxColumn).Width = 108;
		((DataGridViewColumn)DgDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DgDesc).DataPropertyName = "Desc";
		((DataGridViewColumn)DgDesc).HeaderText = "[شرح]";
		((DataGridViewColumn)DgDesc).MinimumWidth = 100;
		((DataGridViewColumn)DgDesc).Name = "DgDesc";
		((DataGridViewBand)DgDesc).ReadOnly = true;
		morakhasidailyBindingSource.DataSource = typeof(Morakhasidaily);
		morakhasidailyBindingSource.CurrentChanged += morakhasidailyBindingSource_CurrentChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "UCMorakhasiDaily";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(1032, 461);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)PanelEx1).PerformLayout();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)morakhasidailyBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
