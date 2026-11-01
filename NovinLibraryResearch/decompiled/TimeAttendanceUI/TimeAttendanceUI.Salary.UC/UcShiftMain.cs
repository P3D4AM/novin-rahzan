using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Shift;

namespace TimeAttendanceUI.Salary.UC;

public class UcShiftMain : UserControl, IHasContext
{
	public delegate void ExitUserControlEventHandler();

	private Font _font;

	private CancellationTokenSource TokenFrm = new CancellationTokenSource();

	private bool _st = true;

	private List<Shifts> _lstShifts;

	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	private LabelX CountRows;

	private LabelX labelX2;

	private BindingSource shiftsBindingSource;

	internal Label LBLST;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn sTDataGridViewCheckBoxColumn;

	internal ButtonX BtnSelect;

	private DataGridViewTextBoxColumn contextGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewCheckBoxColumn stTatiliDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn packageTypeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn periodicDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn packageGuidDataGridViewTextBoxColumn;

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

	public Shifts SelectedItem { get; private set; }

	public event ExitUserControlEventHandler Exit_UserControl;

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
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UcShiftMain(string contextGuid, Font font)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		_font = font;
		try
		{
			ContextGuid = contextGuid;
			InitializeComponent();
			((Control)DGrid).Font = _font;
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			_ = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Control)(object)this, SetStAsync(value: true), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 72);
		}
	}

	public async Task LoadDataAsync(bool st, CancellationToken token)
	{
		try
		{
			_lstShifts = await Shifts.SelectAllAsync(ContextGuid, st, (PackageEnumType)1, "", token);
			List<Shifts> lstShifts = _lstShifts;
			if (lstShifts != null && lstShifts.Count > 0)
			{
				shiftsBindingSource.DataSource = _lstShifts;
				((Control)TxtSearch).Focus();
				RefreshLables();
				((Control)BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftInsert;
				((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftDelete;
				((Control)BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftUpdate;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 87);
		}
	}

	private void RefreshLables()
	{
		try
		{
			((Control)CountRows).Text = shiftsBindingSource.Count.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshLables", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 96);
		}
	}

	public void Insert()
	{
		((PopupItemControl)BtnAdd).PerformClick();
	}

	public void Edit()
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	public void Delete()
	{
		((PopupItemControl)BtnDelete).PerformClick();
	}

	public bool SelectPerform()
	{
		((PopupItemControl)BtnSelect).PerformClick();
		return true;
	}

	public bool GetSt()
	{
		return _st;
	}

	public async Task SetStAsync(bool value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_st = value;
			string lBLST = (_st ? "فعال(Ctrl+S)" : "غیر فعال(Ctrl+S)");
			string bLockingTr = "مسدود کردن";
			string enablingTr = "فعال کردن";
			Task.Run(async delegate
			{
				lBLST = (_st ? Utilities.GetTranslate("[فعال](Ctrl+S)") : Utilities.GetTranslate("[غیر فعال](Ctrl+S)"));
				bLockingTr = Utilities.GetTranslate("[مسدود کردن]");
				enablingTr = Utilities.GetTranslate("[فعال کردن]");
			});
			await LoadDataAsync(_st, token);
			while (!((Control)this).IsHandleCreated && !IsDisposed)
			{
				await Task.Delay(100);
			}
			if (IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (_st)
					{
						((Control)BtnDelete).Text = bLockingTr;
						BtnDelete.Image = (Image)(object)Resources.Disable;
					}
					else
					{
						((Control)BtnDelete).Text = enablingTr;
						BtnDelete.Image = (Image)(object)Resources.Enable;
					}
					((Control)LBLST).Text = lBLST;
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SetStAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 151);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetStAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 154);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.CanInsert(AccessLevelClass.Instance, shiftsBindingSource.Count + 1))
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید شیفت کاری ایجاد نمائید]", "", 3);
			}
			else
			{
				if (!((Control)BtnAdd).Enabled || !((Control)BtnAdd).Visible)
				{
					return;
				}
				((Control)DGrid).Focus();
				TokenFrm?.Cancel();
				TokenFrm = new CancellationTokenSource();
				CancellationToken token = TokenFrm.Token;
				FrmCreateShift frm = new FrmCreateShift(ContextGuid, _font);
				if ((int)((Form)frm).ShowDialog() == 1)
				{
					await LoadDataAsync(GetSt(), token);
					if ((int)((Form)new FrmShift(ContextGuid, frm.ClsShift, _font, isReadOnly: false)).ShowDialog() == 1)
					{
						await LoadDataAsync(GetSt(), token);
					}
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 182);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnEdit).Enabled && ((Control)BtnEdit).Visible)
			{
				((Control)DGrid).Focus();
				TokenFrm?.Cancel();
				TokenFrm = new CancellationTokenSource();
				CancellationToken token = TokenFrm.Token;
				FrmShift frm = new FrmShift(ContextGuid, SelectedItem, _font, isReadOnly: false);
				if ((int)((Form)frm).ShowDialog() == 1)
				{
					await LoadDataAsync(GetSt(), token);
				}
				((Component)(object)frm).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 199);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 4;
		try
		{
			((Control)DGrid).Focus();
			if (SelectedItem == null)
			{
				return;
			}
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			if (GetSt())
			{
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به مسدود کردن شیفت انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					SelectedItem.St = false;
					SelectedItem.Code = "";
					if (await SelectedItem.SaveAsync(token))
					{
						await LoadDataAsync(GetSt(), token);
					}
				}
			}
			else if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به فعال کردن شیفت انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				SelectedItem.St = true;
				Shifts selectedItem = SelectedItem;
				selectedItem.Code = (await Shifts.NextCodeAsync(ContextGuid, token)).ToString();
				if (await SelectedItem.SaveAsync(token))
				{
					await LoadDataAsync(GetSt(), token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 234);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Hide();
			this.Exit_UserControl?.Invoke();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 244);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			string text = Text.FixString(((Control)TxtSearch).Text);
			if (string.IsNullOrEmpty(text))
			{
				shiftsBindingSource.Filter = "";
				return;
			}
			shiftsBindingSource.Filter = " Code like '%" + text + "%' or Name like '%" + text + "%' or Description like '%" + text + "%'";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 255);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 267);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 276);
		}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 287);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 297);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 308);
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
				if (((Control)BtnSelect).Visible)
				{
					((PopupItemControl)BtnSelect).PerformClick();
					e.Handled = true;
				}
				else
				{
					((PopupItemControl)BtnEdit).PerformClick();
					e.Handled = true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 327);
		}
	}

	private async void LBLST_Click(object sender, EventArgs e)
	{
		try
		{
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			await SetStAsync(!_st, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LBLST_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 339);
		}
	}

	private void BtnSelect_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSelect).Enabled && ((Control)BtnSelect).Visible)
			{
				((Control)DGrid).Focus();
				this.Exit_UserControl?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCSHiftMain.cs", 350);
		}
	}

	private void shiftsBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedItem = (Shifts)shiftsBindingSource.Current;
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
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b27: Expected O, but got Unknown
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Expected O, but got Unknown
		//IL_0c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4b: Expected O, but got Unknown
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2a: Expected O, but got Unknown
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1101: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnSelect = new ButtonX();
		LBLST = new Label();
		CountRows = new LabelX();
		labelX2 = new LabelX();
		DGrid = new DataGridViewX();
		shiftsBindingSource = new BindingSource(components);
		GroupPanel1 = new GroupPanel();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		contextGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		stTatiliDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		packageTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		periodicDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		packageGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)shiftsBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)BtnSelect);
		((Control)PanelEx1).Controls.Add((Control)(object)LBLST);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(692, 387);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = (eButtonColor)6;
		((Control)BtnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelect).Location = new Point(6, 73);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(141, 138);
		((Control)BtnSelect).TabIndex = 22;
		((Control)BtnSelect).Text = "[انتخاب شیفت]\r\n    \r\n    (Enter)";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((Control)LBLST).Font = new Font("Tahoma", 9.75f);
		((Control)LBLST).ForeColor = Color.Red;
		((Control)LBLST).Location = new Point(6, 72);
		((Control)LBLST).Name = "LBLST";
		((Control)LBLST).Size = new Size(128, 22);
		((Control)LBLST).TabIndex = 21;
		((Control)LBLST).Text = "[فعال](Ctrl+S)";
		LBLST.TextAlign = (ContentAlignment)32;
		((Control)LBLST).Click += LBLST_Click;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(316, 359);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(258, 23);
		((Control)CountRows).TabIndex = 20;
		((Control)CountRows).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(580, 359);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(112, 23);
		((Control)labelX2).TabIndex = 19;
		((Control)labelX2).Text = "[تعداد کل شیفت ها] :";
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
			(DataGridViewColumn)contextGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)idDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn2,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewCheckBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn3,
			(DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn,
			(DataGridViewColumn)packageTypeDataGridViewTextBoxColumn,
			(DataGridViewColumn)periodicDataGridViewTextBoxColumn,
			(DataGridViewColumn)packageGuidDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = shiftsBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(153, 72);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(532, 281);
		((Control)DGrid).TabIndex = 1;
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		shiftsBindingSource.DataSource = typeof(Shifts);
		shiftsBindingSource.CurrentChanged += shiftsBindingSource_CurrentChanged;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(6, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(679, 60);
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
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(612, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(58, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(612, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).Enter += TxtSearch_Enter;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtSearch_Leave;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.Enable;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(6, 178);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(141, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 4;
		((Control)BtnDelete).Text = "[مسدود کردن]";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(6, 139);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(141, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 3;
		((Control)BtnEdit).Text = "   [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(6, 100);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(141, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 2;
		((Control)BtnAdd).Text = "[اضافه] (Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).DataPropertyName = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).HeaderText = "ContextGuid";
		((DataGridViewColumn)contextGuidDataGridViewTextBoxColumn).Name = "contextGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)contextGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)idDataGridViewTextBoxColumn).DataPropertyName = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).HeaderText = "Id";
		((DataGridViewColumn)idDataGridViewTextBoxColumn).Name = "idDataGridViewTextBoxColumn";
		((DataGridViewBand)idDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)idDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Code";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "کد شیفت";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Width = 80;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "Name";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "نام شیفت";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Width = 83;
		((DataGridViewColumn)dataGridViewCheckBoxColumn1).DataPropertyName = "St";
		((DataGridViewColumn)dataGridViewCheckBoxColumn1).HeaderText = "St";
		((DataGridViewColumn)dataGridViewCheckBoxColumn1).Name = "dataGridViewCheckBoxColumn1";
		((DataGridViewBand)dataGridViewCheckBoxColumn1).ReadOnly = true;
		((DataGridViewBand)dataGridViewCheckBoxColumn1).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "توضیحات";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).ReadOnly = true;
		((DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)2;
		((DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn).DataPropertyName = "StTatili";
		((DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn).HeaderText = "تعطیل رسمی، روز کاریست";
		((DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn).Name = "stTatiliDataGridViewCheckBoxColumn";
		((DataGridViewBand)stTatiliDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewColumn)stTatiliDataGridViewCheckBoxColumn).Width = 94;
		((DataGridViewColumn)packageTypeDataGridViewTextBoxColumn).DataPropertyName = "PackageType";
		((DataGridViewColumn)packageTypeDataGridViewTextBoxColumn).HeaderText = "PackageType";
		((DataGridViewColumn)packageTypeDataGridViewTextBoxColumn).Name = "packageTypeDataGridViewTextBoxColumn";
		((DataGridViewBand)packageTypeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)packageTypeDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).DataPropertyName = "Periodic";
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).HeaderText = "Periodic";
		((DataGridViewColumn)periodicDataGridViewTextBoxColumn).Name = "periodicDataGridViewTextBoxColumn";
		((DataGridViewBand)periodicDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)periodicDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)packageGuidDataGridViewTextBoxColumn).DataPropertyName = "PackageGuid";
		((DataGridViewColumn)packageGuidDataGridViewTextBoxColumn).HeaderText = "PackageGuid";
		((DataGridViewColumn)packageGuidDataGridViewTextBoxColumn).Name = "packageGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)packageGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)packageGuidDataGridViewTextBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "UcShiftMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(692, 387);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)shiftsBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
