using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PayamakUi.Classes.Entities;
using PayamakUi.Mapper;
using PayamakUi.Properties;

namespace PayamakUi.Users;

public class FRMAccountMain : CacheBaseForm
{
	private List<SMSNet_Account> lstAccount = new List<SMSNet_Account>();

	private PayamakClientHubInstances _instance;

	public SMSNet_Account SelectedAcount = new SMSNet_Account();

	private IContainer components;

	internal GroupPanel GroupPanel1;

	internal GroupPanel GroupPanel2;

	internal ButtonX BtnSelect;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal DataGridViewX DGrid;

	internal BindingSource SMSNetAccountBindingSource;

	internal ButtonX BTNEnterPanel;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

	private DataGridViewTextBoxColumn reserve3DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn senderNumberDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn Account;

	private DataGridViewTextBoxColumn Type;

	private DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn domainDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullDomainDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	public BaseSmsNetAccount SmsAccount
	{
		get
		{
			BindingSource sMSNetAccountBindingSource = SMSNetAccountBindingSource;
			return ((SMSNet_Account)((sMSNetAccountBindingSource != null) ? sMSNetAccountBindingSource.Current : null)) ?? null;
		}
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>> ret = await _instance.SmsNetAccount.GetAllAsync(token);
			List<SMSNet_Account> tempList = await SmsNetAccountMapper.Instance.MapListAsync(base.ContextGuid, ret.value);
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			do
			{
				await Task.Delay(100);
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						((Control)DGrid).Focus();
						if (!ret.HasError)
						{
							lstAccount = tempList;
						}
						SMSNetAccountBindingSource.DataSource = lstAccount?.ToSortableBindingList(token);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 55);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 58);
		}
	}

	public FRMAccountMain(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 68);
		}
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		try
		{
			((Control)DGrid).Focus();
			if ((int)((Form)new FRMAccount(base.ContextGuid)).ShowDialog((IWin32Window)(object)this) == 1)
			{
				CancellationToken token = new CancellationTokenSource(60000).Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 81);
		}
	}

	private void FRMSendNet_Load(object sender, EventArgs e)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			Task.Run(() => LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSendNet_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 90);
		}
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		try
		{
			DataGridViewX dGrid = DGrid;
			if (dGrid != null)
			{
				((Control)dGrid).Focus();
			}
			if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
			{
				return;
			}
			BaseSmsNetAccount smsAccount = SmsAccount;
			if ((smsAccount == null || smsAccount.Id > 0) && SmsAccount.Id != 10000 && (int)((Form)new FRMAccount(base.ContextGuid, SmsAccount.Id)).ShowDialog((IWin32Window)(object)this) == 1)
			{
				CancellationToken token = new CancellationTokenSource(60000).Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 104);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				((Control)DGrid).Focus();
				if (!((Control)BtnDelete).Enabled || !((Control)BtnDelete).Visible || SmsAccount.Id <= 0 || SmsAccount.Id == 10000 || (int)MessageBox.Show("مایل به حذف حساب کاربری انتخاب شده هستید ؟".GetTranslate(), "هشدار".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)16, (MessageBoxDefaultButton)256, (MessageBoxOptions)524288) != 6)
				{
					goto end_IL_0057;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _instance.SmsNetAccount.DeleteAsync(SmsAccount.Id, default(CancellationToken)), ReturnedState.None, "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 116);
				goto end_IL_004e;
				end_IL_0057:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 120);
				res.AddReturnedValue(ex, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 121);
				goto end_IL_004e;
			}
			end_IL_004e:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs");
			}
			else
			{
				CancellationToken token = new CancellationTokenSource(60000).Token;
				Task.Run(() => LoadDataAsync(token));
			}
		}
	}

	private void ButtonX1_Click(object sender, EventArgs e)
	{
		try
		{
			SelectedAcount = (SMSNet_Account)SmsAccount;
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ButtonX1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 141);
		}
	}

	private void FRMSendNet_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSendNet_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 149);
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
				e.Handled = true;
				BtnSelect.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 159);
		}
	}

	private async void BTNEnterPanel_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount > 0 && SmsAccount.Id > 0 && SmsAccount.Id != 10000)
			{
				ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> returnedSaveFuncInfoWithValue = await _instance.SmsNetAccount.GetByIdAsync(SmsAccount.Id);
				Process.Start("http://" + returnedSaveFuncInfoWithValue.value.FullDomain + "/loginrequest.aspx?userName=" + SmsAccount.Username + "&password=" + SmsAccount.Password);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNEnterPanel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccountMain.cs", 171);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
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
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a24: Expected O, but got Unknown
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Expected O, but got Unknown
		//IL_0ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed9: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMAccountMain));
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		GroupPanel1 = new GroupPanel();
		BTNEnterPanel = new ButtonX();
		BtnSelect = new ButtonX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		GroupPanel2 = new GroupPanel();
		DGrid = new DataGridViewX();
		SMSNetAccountBindingSource = new BindingSource(components);
		DGID = new DataGridViewTextBoxColumn();
		titleDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		usernameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		senderNumberDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		Account = new DataGridViewTextBoxColumn();
		Type = new DataGridViewTextBoxColumn();
		passwordDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		domainDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fullDomainDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		((Control)GroupPanel1).SuspendLayout();
		((Control)GroupPanel2).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)SMSNetAccountBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		GroupPanel1.CanvasColor = SystemColors.Control;
		GroupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel1).Controls.Add((Control)(object)BTNEnterPanel);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnSelect);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnDelete);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnEdit);
		((Control)GroupPanel1).Controls.Add((Control)(object)BtnAdd);
		((Control)GroupPanel1).Controls.Add((Control)(object)GroupPanel2);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Dock = (DockStyle)5;
		((Control)GroupPanel1).Location = new Point(0, 0);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(561, 271);
		GroupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel1.Style.BackColorGradientAngle = 90;
		GroupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderBottomWidth = 1;
		GroupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderLeftWidth = 1;
		GroupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderRightWidth = 1;
		GroupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel1.Style.BorderTopWidth = 1;
		GroupPanel1.Style.CornerDiameter = 4;
		GroupPanel1.Style.CornerType = eCornerType.Rounded;
		GroupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		GroupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)BTNEnterPanel).AccessibleRole = (AccessibleRole)43;
		BTNEnterPanel.ColorTable = eButtonColor.Office2007WithBackground;
		BTNEnterPanel.Image = (Image)componentResourceManager.GetObject("BTNEnterPanel.Image");
		BTNEnterPanel.ImageFixedSize = new Size(30, 30);
		((Control)BTNEnterPanel).Location = new Point(5, 156);
		((Control)BTNEnterPanel).Name = "BTNEnterPanel";
		((Control)BTNEnterPanel).Size = new Size(28, 30);
		BTNEnterPanel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNEnterPanel).TabIndex = 26;
		BTNEnterPanel.Tooltip = "ورود به سامانه";
		((Control)BTNEnterPanel).Click += BTNEnterPanel_Click;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		((Control)BtnSelect).Anchor = (AnchorStyles)14;
		BtnSelect.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)BtnSelect).Location = new Point(41, 226);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(506, 30);
		BtnSelect.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSelect).TabIndex = 25;
		((Control)BtnSelect).Text = "انتخاب";
		BtnSelect.Tooltip = "انتخاب";
		((Control)BtnSelect).Click += ButtonX1_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = eButtonColor.Office2007WithBackground;
		BtnDelete.Image = (Image)componentResourceManager.GetObject("BtnDelete.Image");
		((Control)BtnDelete).Location = new Point(5, 120);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(28, 30);
		BtnDelete.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnDelete).TabIndex = 24;
		BtnDelete.Tooltip = "حذف حساب کاربری";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = eButtonColor.Office2007WithBackground;
		BtnEdit.Image = (Image)componentResourceManager.GetObject("BtnEdit.Image");
		((Control)BtnEdit).Location = new Point(5, 84);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).Size = new Size(28, 30);
		BtnEdit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnEdit).TabIndex = 23;
		BtnEdit.Tooltip = "ویرایش حساب کاربری";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = eButtonColor.Office2007WithBackground;
		BtnAdd.Image = (Image)(object)Resources.add2;
		((Control)BtnAdd).Location = new Point(5, 48);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(28, 30);
		BtnAdd.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAdd).TabIndex = 22;
		BtnAdd.Tooltip = "افزودن حساب کاربری جدید";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)GroupPanel2).Anchor = (AnchorStyles)13;
		((Control)GroupPanel2).BackColor = Color.Transparent;
		GroupPanel2.CanvasColor = SystemColors.Control;
		GroupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GroupPanel2).Controls.Add((Control)(object)DGrid);
		GroupPanel2.DisabledBackColor = Color.Empty;
		((Control)GroupPanel2).Location = new Point(41, 3);
		((Control)GroupPanel2).Name = "GroupPanel2";
		((Control)GroupPanel2).Size = new Size(506, 217);
		GroupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GroupPanel2.Style.BackColorGradientAngle = 90;
		GroupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GroupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderBottomWidth = 2;
		GroupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GroupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderLeftWidth = 2;
		GroupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderRightWidth = 2;
		GroupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		GroupPanel2.Style.BorderTopWidth = 2;
		GroupPanel2.Style.CornerDiameter = 4;
		GroupPanel2.Style.CornerType = eCornerType.Rounded;
		GroupPanel2.Style.TextAlignment = eStyleTextAlignment.Center;
		GroupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GroupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GroupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		GroupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GroupPanel2).TabIndex = 0;
		((Control)GroupPanel2).Text = "لیست حسابهای کاربری پیامک";
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = SystemColors.GradientInactiveCaption;
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).BackgroundColor = Color.White;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[11]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)titleDataGridViewTextBoxColumn,
			(DataGridViewColumn)usernameDataGridViewTextBoxColumn,
			(DataGridViewColumn)senderNumberDataGridViewTextBoxColumn,
			(DataGridViewColumn)Account,
			(DataGridViewColumn)Type,
			(DataGridViewColumn)passwordDataGridViewTextBoxColumn,
			(DataGridViewColumn)domainDataGridViewTextBoxColumn,
			(DataGridViewColumn)fullDomainDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn1,
			(DataGridViewColumn)dataGridViewTextBoxColumn2
		});
		((DataGridView)DGrid).DataSource = SMSNetAccountBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 8);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(490, 185);
		((Control)DGrid).TabIndex = 0;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		SMSNetAccountBindingSource.DataSource = typeof(SMSNet_Account);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "Id";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).DataPropertyName = "Title";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).HeaderText = "عنوان";
		((DataGridViewColumn)titleDataGridViewTextBoxColumn).Name = "titleDataGridViewTextBoxColumn";
		((DataGridViewBand)titleDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).DataPropertyName = "Username";
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).HeaderText = "نام کاربری";
		((DataGridViewColumn)usernameDataGridViewTextBoxColumn).Name = "usernameDataGridViewTextBoxColumn";
		((DataGridViewBand)usernameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).DataPropertyName = "SenderNumber";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).HeaderText = "شماره خط";
		((DataGridViewColumn)senderNumberDataGridViewTextBoxColumn).Name = "senderNumberDataGridViewTextBoxColumn";
		((DataGridViewBand)senderNumberDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)Account).AutoSizeMode = (DataGridViewAutoSizeColumnMode)2;
		((DataGridViewColumn)Account).DataPropertyName = "AccountForDisplay";
		((DataGridViewColumn)Account).HeaderText = "میزان اعتبار";
		((DataGridViewColumn)Account).Name = "Account";
		((DataGridViewBand)Account).ReadOnly = true;
		((DataGridViewColumn)Account).Width = 85;
		((DataGridViewColumn)Type).DataPropertyName = "Type";
		((DataGridViewColumn)Type).HeaderText = "طریقه ارسال";
		((DataGridViewColumn)Type).Name = "Type";
		((DataGridViewBand)Type).ReadOnly = true;
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).DataPropertyName = "Password";
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).HeaderText = "Password";
		((DataGridViewColumn)passwordDataGridViewTextBoxColumn).Name = "passwordDataGridViewTextBoxColumn";
		((DataGridViewBand)passwordDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)passwordDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).DataPropertyName = "Domain";
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).HeaderText = "Domain";
		((DataGridViewColumn)domainDataGridViewTextBoxColumn).Name = "domainDataGridViewTextBoxColumn";
		((DataGridViewBand)domainDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)domainDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).DataPropertyName = "FullDomain";
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).HeaderText = "FullDomain";
		((DataGridViewColumn)fullDomainDataGridViewTextBoxColumn).Name = "fullDomainDataGridViewTextBoxColumn";
		((DataGridViewBand)fullDomainDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)fullDomainDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "St";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "St";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn1).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn2).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(561, 271);
		((Control)this).Controls.Add((Control)(object)GroupPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMAccountMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "مدیریت حساب کاربری ارسال پیامک";
		((Form)this).Load += FRMSendNet_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMSendNet_KeyDown);
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel2).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)SMSNetAccountBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
