using System;
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
using TimeAttendanceDAL.Dal.FLRSanad;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmSanadMain : BaseForm
{
	private bool _isDisposed;

	private List<AttSanad> _lstAttSanad = new List<AttSanad>();

	private readonly DateTime _startDateTime;

	private readonly DateTime _endDateTime;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private Font _font;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnPrintSanadRow;

	private LabelX LblSumK;

	private LabelX labelX2;

	private LabelX LblRowCount;

	private LabelX LblKol;

	private ButtonX BTNEnteghal;

	private ButtonX BtnPrint;

	internal ButtonX BtnDelete;

	internal ButtonX BtnView;

	internal DataGridViewX DGrid;

	private BindingSource aTTSanadBindingSource;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGNO;

	private DataGridViewTextBoxColumn DGDateSH;

	private DataGridViewTextBoxColumn DGDesc;

	private DataGridViewTextBoxColumn userNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn sumDataGridViewTextBoxColumn;

	public AttSanad SelectedItem { get; private set; }

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

	public FrmSanadMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_startDateTime = ConstantValues.MinDateM;
			_endDateTime = DateTime.Parse("2050/03/21 00:00:00");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmSanadMain(string contextGuid, Font font, DateTime date1, DateTime date2)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_startDateTime = date1;
			_endDateTime = date2;
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
			((Control)BtnView).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingDisplay;
			((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingDelete;
			((Control)BTNEnteghal).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingMove;
			((Control)BtnPrint).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingReport;
			((Control)BtnPrintSanadRow).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingPrint;
			_lstAttSanad = await AttSanad.GetAllAsync(((CacheBaseForm)this).ContextGuid, _startDateTime, _endDateTime, token);
			aTTSanadBindingSource.DataSource = Utilities.ToSortableBindingList<AttSanad>((IEnumerable<AttSanad>)_lstAttSanad, default(CancellationToken));
			((Control)LblRowCount).Text = _lstAttSanad.Count.ToString();
			((Control)LblSumK).Text = SumKol().ToString("N0");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 74);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((BaseForm)this).OnLoad(e);
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Accounting.AccountingMain)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 85);
		}
	}

	private async void FRMSanadMain_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 46)
			{
				if ((int)keyCode != 27)
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
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
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
					((PopupItemControl)BtnView).PerformClick();
				}
			}
			else if (e.Control)
			{
				((PopupItemControl)BtnPrint).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSanadMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 122);
		}
	}

	private async void BtnView_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		try
		{
			if (SelectedItem != null)
			{
				await ((CacheBaseForm)new FrmSanad(((CacheBaseForm)this).ContextGuid, SelectedItem)).ShowAsync((IWin32Window)(object)this);
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnView_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnView_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 139);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (SelectedItem != null && ((Control)BtnDelete).Enabled && ((Control)BtnDelete).Visible)
			{
				((Control)DGrid).Focus();
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف سند حقوقی انتخاب شده هستید ؟]"), "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					await SelectedItem.DeleteAsync();
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 158);
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
			List<object> list = new List<object>();
			foreach (AttSanad item in _lstAttSanad)
			{
				list.Add(new
				{
					No = item.No,
					dateCalture = Calendar.MiladiToCalture(item.DateM),
					Description = item.Description,
					Sum = item.Sum
				});
			}
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)4, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 180);
		}
	}

	private void BtnPrintSanadRow_Click(object sender, EventArgs e)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)DGrid).Focus();
			if (((Control)BtnPrintSanadRow).Enabled && ((Control)BtnPrintSanadRow).Visible && ((DataGridView)DGrid).RowCount != 0)
			{
				FMessageBox.Show(Utilities.GetTranslate("[مایل به چاپ] " + ((DataGridView)DGrid).RowCount + "[ عدد سند هستید ؟]"), Utilities.GetTranslate("[پیام سیستم]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question, FMessageBox.FMessageBoxDefaultButton.Button2);
				_ = 7;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrintSanadRow_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 193);
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
				((PopupItemControl)BtnView).PerformClick();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 204);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13 || (int)e.KeyCode == 40)
		{
			((Control)DGrid).Focus();
		}
	}

	private decimal SumKol()
	{
		try
		{
			return _lstAttSanad.Aggregate(0m, (decimal current, AttSanad item) => current + item.Sum);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SumKol", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 218);
		}
		return 0m;
	}

	private async void BTNEnteghal_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (SelectedItem != null)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				if (await SelectedItem.Sedt2AccAsync(token))
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[ارسال به مالی با موفقیت انجام شد.]", "", 3);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNEnteghal_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs", 234);
		}
	}

	private void FrmSanadMain_Load(object sender, EventArgs e)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)this).Text = " اسناد حسابداری حقوق";
			((Control)this).Text = Utilities.GetTranslate("[اسناد حسابداری حقوق]");
			((Control)DGrid).Font = _font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده...", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "FrmSanadMain_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FRMSanadMain.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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

	private void aTTSanadBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedItem = (AttSanad)aTTSanadBindingSource.Current;
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Expected O, but got Unknown
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Expected O, but got Unknown
		//IL_0b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Expected O, but got Unknown
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d41: Expected O, but got Unknown
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSanadMain));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		BtnPrintSanadRow = new ButtonX();
		LblSumK = new LabelX();
		labelX2 = new LabelX();
		LblRowCount = new LabelX();
		LblKol = new LabelX();
		BTNEnteghal = new ButtonX();
		BtnPrint = new ButtonX();
		BtnDelete = new ButtonX();
		BtnView = new ButtonX();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGDesc = new DataGridViewTextBoxColumn();
		sumDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		aTTSanadBindingSource = new BindingSource(components);
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)aTTSanadBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrintSanadRow);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumK);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)LblRowCount);
		((Control)panelEx1).Controls.Add((Control)(object)LblKol);
		((Control)panelEx1).Controls.Add((Control)(object)BTNEnteghal);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)panelEx1).Controls.Add((Control)(object)BtnView);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(792, 573);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)BtnPrintSanadRow).AccessibleRole = (AccessibleRole)43;
		BtnPrintSanadRow.ColorTable = (eButtonColor)6;
		((Control)BtnPrintSanadRow).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnPrintSanadRow.Image = (Image)componentResourceManager.GetObject("BtnPrintSanadRow.Image");
		BtnPrintSanadRow.ImagePosition = (eImagePosition)1;
		((Control)BtnPrintSanadRow).Location = new Point(12, 168);
		((Control)BtnPrintSanadRow).Name = "BtnPrintSanadRow";
		((Control)BtnPrintSanadRow).Size = new Size(151, 33);
		((PopupItemControl)BtnPrintSanadRow).Style = (eDotNetBarStyle)9;
		((Control)BtnPrintSanadRow).TabIndex = 37;
		((Control)BtnPrintSanadRow).Text = "[چاپ تک تک اسناد]";
		((Control)BtnPrintSanadRow).Visible = false;
		((Control)BtnPrintSanadRow).Click += BtnPrintSanadRow_Click;
		((Control)LblSumK).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblSumK).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumK).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumK).ForeColor = Color.DarkRed;
		((Control)LblSumK).Location = new Point(160, 540);
		((Control)LblSumK).Name = "LblSumK";
		((Control)LblSumK).Size = new Size(229, 23);
		((Control)LblSumK).TabIndex = 36;
		((Control)LblSumK).Text = "0";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).Location = new Point(395, 540);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(89, 23);
		((Control)labelX2).TabIndex = 35;
		((Control)labelX2).Text = "[جمع کل اسناد] :";
		((Control)LblRowCount).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRowCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRowCount).ForeColor = Color.DarkRed;
		((Control)LblRowCount).Location = new Point(518, 540);
		((Control)LblRowCount).Name = "LblRowCount";
		((Control)LblRowCount).Size = new Size(160, 23);
		((Control)LblRowCount).TabIndex = 34;
		((Control)LblRowCount).Text = "labelX3";
		((Control)LblKol).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblKol).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblKol).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblKol).Location = new Point(684, 540);
		((Control)LblKol).Name = "LblKol";
		((Control)LblKol).Size = new Size(94, 23);
		((Control)LblKol).TabIndex = 33;
		((Control)LblKol).Text = "[تعداد کل اسناد] :";
		LblKol.TextAlignment = (StringAlignment)2;
		((Control)BTNEnteghal).AccessibleRole = (AccessibleRole)43;
		BTNEnteghal.ColorTable = (eButtonColor)6;
		((Control)BTNEnteghal).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNEnteghal.Image = (Image)componentResourceManager.GetObject("BTNEnteghal.Image");
		BTNEnteghal.ImagePosition = (eImagePosition)1;
		((Control)BTNEnteghal).Location = new Point(12, 90);
		((Control)BTNEnteghal).Name = "BTNEnteghal";
		((Control)BTNEnteghal).Size = new Size(151, 33);
		((PopupItemControl)BTNEnteghal).Style = (eDotNetBarStyle)9;
		((Control)BTNEnteghal).TabIndex = 32;
		((Control)BTNEnteghal).Text = "[انتقال به حسابداری]";
		((Control)BTNEnteghal).Click += BTNEnteghal_Click;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnPrint.Image = (Image)componentResourceManager.GetObject("BtnPrint.Image");
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(12, 129);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(151, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 21;
		((Control)BtnPrint).Text = " (Ctrl+P)  [چاپ]     ";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)componentResourceManager.GetObject("BtnDelete.Image");
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(12, 51);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(151, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 19;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnView).AccessibleRole = (AccessibleRole)43;
		BtnView.ColorTable = (eButtonColor)6;
		((Control)BtnView).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnView.Image = (Image)componentResourceManager.GetObject("BtnView.Image");
		BtnView.ImagePosition = (eImagePosition)1;
		((Control)BtnView).Location = new Point(12, 12);
		((Control)BtnView).Name = "BtnView";
		((Control)BtnView).Size = new Size(151, 33);
		((PopupItemControl)BtnView).Style = (eDotNetBarStyle)9;
		((Control)BtnView).TabIndex = 17;
		((Control)BtnView).Text = "[مشاهده](F7)";
		((Control)BtnView).Click += BtnView_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGDesc,
			(DataGridViewColumn)sumDataGridViewTextBoxColumn
		});
		((DataGridView)DGrid).DataSource = aTTSanadBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(169, 12);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(608, 522);
		((Control)DGrid).TabIndex = 16;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDesc).DataPropertyName = "Description";
		((DataGridViewColumn)DGDesc).HeaderText = "[شرح]";
		((DataGridViewColumn)DGDesc).MinimumWidth = 100;
		((DataGridViewColumn)DGDesc).Name = "DGDesc";
		((DataGridViewBand)DGDesc).ReadOnly = true;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).DataPropertyName = "Sum";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)sumDataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).HeaderText = "[جمع]";
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).MinimumWidth = 120;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).Name = "sumDataGridViewTextBoxColumn";
		((DataGridViewBand)sumDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)sumDataGridViewTextBoxColumn).Width = 120;
		aTTSanadBindingSource.DataSource = typeof(AttSanad);
		aTTSanadBindingSource.CurrentChanged += aTTSanadBindingSource_CurrentChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(792, 573);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FrmSanadMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[سند]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FrmSanadMain_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMSanadMain_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)aTTSanadBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
