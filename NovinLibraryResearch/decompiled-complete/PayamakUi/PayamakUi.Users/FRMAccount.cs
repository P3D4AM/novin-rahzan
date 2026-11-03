using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Payamak;
using PacketParser.Payamak.Entities;

namespace PayamakUi.Users;

public class FRMAccount : CacheBaseForm
{
	private PayamakClientHubInstances _instance;

	private BaseSmsNetAccount _cls;

	private IContainer components;

	private GroupPanel groupPanel1;

	internal ButtonX BtnExit;

	internal ButtonX BtnAccept;

	private GroupPanel groupPanel2;

	private TextBoxX TxtDomain;

	private TextBoxX TxtDescription;

	private TextBoxX TxtPassWord2;

	private Label LabelX5;

	private Label LabelX4;

	private Label LabelX3;

	private TextBoxX TxtPassWord;

	private TextBoxX TxtUser;

	private Label labelX1;

	private Label labelX2;

	internal TextBoxX TxtTitle;

	internal Label LabelX6;

	private Label LabelX7;

	private TextBoxX TxtSenderNumber;

	internal ButtonX TestSharj;

	internal Label label1;

	private ComboBox CmbType;

	private Label label2;

	private TextBoxFa txtMinimumCredit;

	private async Task SetDataAsync(int id, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_cls = (await _instance.SmsNetAccount.GetByIdAsync(id))?.value;
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
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((ListControl)CmbType).SelectedIndex = (int)_cls.Type;
					((Control)TxtUser).Text = _cls.Username;
					((Control)TxtPassWord).Text = _cls.Password;
					((Control)TxtPassWord2).Text = _cls.Password;
					((Control)TxtDomain).Text = _cls.FullDomain;
					((Control)TxtSenderNumber).Text = _cls.SenderNumber.TrimStart(new char[1] { '0' });
					((Control)TxtTitle).Text = _cls.Title;
					((Control)TxtDescription).Text = _cls.Description;
					txtMinimumCredit.Value = (_cls?.MetaData?.MinimumCredit).GetValueOrDefault();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 48);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 51);
		}
	}

	public FRMAccount(string contextGuid, int ID)
		: base(contextGuid)
	{
		FRMAccount fRMAccount = this;
		try
		{
			InitializeComponent();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
			Task.Run(() => fRMAccount.SetDataAsync(ID));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 62);
		}
	}

	public FRMAccount(string contextGuid)
		: base(contextGuid)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		try
		{
			InitializeComponent();
			_cls = new BaseSmsNetAccount();
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
			((Control)this).KeyDown += new KeyEventHandler(FRMAccount_KeyDown);
			((Form)this).Load += FRMAccount_Load;
			((ListControl)CmbType).SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 76);
		}
	}

	private ReturnedSaveFuncInfo CheckData()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (((ListControl)CmbType).SelectedIndex == 0)
			{
				if (string.IsNullOrEmpty(((Control)TxtUser).Text.Trim()))
				{
					returnedSaveFuncInfo.AddError("نام کاربری اجباری است", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 88);
					((Control)TxtUser).Focus();
				}
				if (((Control)TxtPassWord).Text != ((Control)TxtPassWord2).Text)
				{
					if (string.IsNullOrEmpty(((Control)TxtPassWord).Text.Trim()))
					{
						returnedSaveFuncInfo.AddError("فیلد رمز اجباری است", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 95);
						((Control)TxtPassWord).Text = "";
						((Control)TxtPassWord).Focus();
					}
					else
					{
						returnedSaveFuncInfo.AddError("رمز عبور و تکرار آن مطابقت ندارد", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 101);
						((Control)TxtPassWord).Text = "";
						((Control)TxtPassWord2).Text = "";
						((Control)TxtPassWord).Focus();
					}
				}
				if (string.IsNullOrEmpty(((Control)TxtDomain).Text.Trim()))
				{
					returnedSaveFuncInfo.AddError("دامنه حساب کاربری را وارد نمائید", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 109);
					((Control)TxtDomain).Focus();
				}
			}
			if (((Control)TxtTitle).Text == "")
			{
				returnedSaveFuncInfo.AddError("عنوان حساب کاربری اجباری است", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 115);
				((Control)TxtTitle).Focus();
			}
			if (((Control)TxtSenderNumber).Text == "")
			{
				returnedSaveFuncInfo.AddError("شماره فرستنده را وارد نمائید", "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 120);
				((Control)TxtSenderNumber).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 126);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 127);
		}
		return returnedSaveFuncInfo;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				GetData();
				res.AddReturnedValue(CheckData(), ReturnedState.None, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 138);
				if (res.HasError)
				{
					goto end_IL_003b;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _instance.SmsNetAccount.SaveAsync(_cls, default(CancellationToken)), ReturnedState.None, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 140);
				goto end_IL_0032;
				end_IL_003b:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 144);
				res.AddReturnedValue(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 145);
				goto end_IL_0032;
			}
			end_IL_0032:;
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void GetData()
	{
		try
		{
			_cls.Username = ((Control)TxtUser).Text.FixString();
			_cls.Password = ((Control)TxtPassWord).Text.FixString();
			_cls.Domain = ((Control)TxtDomain).Text.FixString();
			_cls.SenderNumber = ((Control)TxtSenderNumber).Text.FixString().TrimStart(new char[1] { '0' });
			_cls.Title = ((Control)TxtTitle).Text.FixString();
			_cls.Description = ((Control)TxtDescription).Text.FixString();
			_cls.Type = (EnSmsSenderType)((ListControl)CmbType).SelectedIndex;
			BaseSmsNetAccount cls = _cls;
			if (cls.MetaData == null)
			{
				SmsNetAccountMetaData smsNetAccountMetaData2 = (cls.MetaData = new SmsNetAccountMetaData());
			}
			_cls.MetaData.MinimumCredit = txtMinimumCredit.Value;
			_cls.St = 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 173);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 185);
		}
	}

	private void FRMAccount_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if ((int)keyCode == 116)
			{
				BtnAccept.PerformClick();
			}
			else if ((int)keyCode == 13)
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMAccount_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 217);
		}
	}

	private void TxtTitle_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtTitle_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 229);
		}
	}

	private void TxtTitle_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtTitle_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 239);
		}
	}

	private async void TestSharj_Click(object sender, EventArgs e)
	{
		try
		{
			string domain = ((Control)TxtDomain).Text.ToLower().Replace(".com", "").Replace(".ir", "");
			ReturnedSaveFuncInfoWithValue<decimal> returnedSaveFuncInfoWithValue = await WebSmsSender.GetCreditAsync(((Control)TxtUser).Text, ((Control)TxtPassWord).Text, domain, default(CancellationToken));
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				MessageBox.Show("خطا در برقراری ارتباط\r\n" + returnedSaveFuncInfoWithValue.ErrorMessage, "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)16);
			}
			else
			{
				MessageBox.Show("ارتباط شما برقرار است\r\n" + $"میزان اعتبار سامانه پیامکی:{returnedSaveFuncInfoWithValue:N0}", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TestSharj_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 257);
		}
	}

	private void FRMAccount_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtTitle).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMAccount_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 266);
		}
	}

	private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)CmbType).SelectedIndex == 1)
			{
				((Control)TxtUser).Enabled = false;
				((Control)TxtPassWord2).Enabled = false;
				((Control)TxtPassWord).Enabled = false;
				((Control)TxtDomain).Enabled = false;
				((Control)TestSharj).Enabled = false;
			}
			else
			{
				((Control)TxtUser).Enabled = true;
				((Control)TxtPassWord2).Enabled = true;
				((Control)TxtPassWord).Enabled = true;
				((Control)TxtDomain).Enabled = true;
				((Control)TestSharj).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CmbType_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Users\\FRMAccount.cs", 290);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_125f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1269: Expected O, but got Unknown
		//IL_12ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d4: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMAccount));
		groupPanel1 = new GroupPanel();
		TestSharj = new ButtonX();
		BtnExit = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel2 = new GroupPanel();
		label1 = new Label();
		CmbType = new ComboBox();
		LabelX7 = new Label();
		TxtSenderNumber = new TextBoxX();
		LabelX6 = new Label();
		TxtTitle = new TextBoxX();
		TxtDomain = new TextBoxX();
		TxtDescription = new TextBoxX();
		TxtPassWord2 = new TextBoxX();
		LabelX5 = new Label();
		LabelX4 = new Label();
		LabelX3 = new Label();
		TxtPassWord = new TextBoxX();
		TxtUser = new TextBoxX();
		labelX1 = new Label();
		labelX2 = new Label();
		txtMinimumCredit = new TextBoxFa();
		label2 = new Label();
		((Control)groupPanel1).SuspendLayout();
		((Control)groupPanel2).SuspendLayout();
		((Control)this).SuspendLayout();
		groupPanel1.CanvasColor = SystemColors.Control;
		groupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)groupPanel1).Controls.Add((Control)(object)TestSharj);
		((Control)groupPanel1).Controls.Add((Control)(object)BtnExit);
		((Control)groupPanel1).Controls.Add((Control)(object)BtnAccept);
		((Control)groupPanel1).Controls.Add((Control)(object)groupPanel2);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Dock = (DockStyle)5;
		((Control)groupPanel1).Location = new Point(0, 0);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).RightToLeft = (RightToLeft)1;
		((Control)groupPanel1).Size = new Size(253, 370);
		groupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		groupPanel1.Style.BackColorGradientAngle = 90;
		groupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		groupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel1.Style.BorderBottomWidth = 1;
		groupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		groupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel1.Style.BorderLeftWidth = 1;
		groupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel1.Style.BorderRightWidth = 1;
		groupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel1.Style.BorderTopWidth = 1;
		groupPanel1.Style.CornerDiameter = 4;
		groupPanel1.Style.CornerType = eCornerType.Rounded;
		groupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		groupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		groupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel1).TabIndex = 1;
		((Control)TestSharj).AccessibleRole = (AccessibleRole)43;
		((Control)TestSharj).Anchor = (AnchorStyles)10;
		TestSharj.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)TestSharj).Location = new Point(86, 328);
		((Control)TestSharj).Name = "TestSharj";
		((Control)TestSharj).Size = new Size(152, 33);
		TestSharj.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)TestSharj).TabIndex = 3;
		((Control)TestSharj).Text = "بررسی ارتباط";
		((Control)TestSharj).Click += TestSharj_Click;
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)6;
		BtnExit.ColorTable = eButtonColor.Office2007WithBackground;
		BtnExit.Image = (Image)componentResourceManager.GetObject("BtnExit.Image");
		BtnExit.ImageFixedSize = new Size(30, 30);
		((Control)BtnExit).Location = new Point(6, 328);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(34, 33);
		BtnExit.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnExit).TabIndex = 2;
		((Control)BtnExit).TabStop = false;
		BtnExit.Tooltip = "خروج";
		((Control)BtnExit).Click += BtnExit_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = eButtonColor.Office2007WithBackground;
		BtnAccept.Image = (Image)componentResourceManager.GetObject("BtnAccept.Image");
		((Control)BtnAccept).Location = new Point(46, 328);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).Size = new Size(34, 33);
		BtnAccept.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnAccept).TabIndex = 1;
		BtnAccept.Tooltip = "ذخیره";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)groupPanel2).Anchor = (AnchorStyles)15;
		((Control)groupPanel2).BackColor = Color.Transparent;
		groupPanel2.CanvasColor = SystemColors.Control;
		groupPanel2.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)groupPanel2).Controls.Add((Control)(object)label1);
		((Control)groupPanel2).Controls.Add((Control)(object)CmbType);
		((Control)groupPanel2).Controls.Add((Control)(object)label2);
		((Control)groupPanel2).Controls.Add((Control)(object)LabelX7);
		((Control)groupPanel2).Controls.Add((Control)(object)txtMinimumCredit);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSenderNumber);
		((Control)groupPanel2).Controls.Add((Control)(object)LabelX6);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtTitle);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtDomain);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtDescription);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtPassWord2);
		((Control)groupPanel2).Controls.Add((Control)(object)LabelX5);
		((Control)groupPanel2).Controls.Add((Control)(object)LabelX4);
		((Control)groupPanel2).Controls.Add((Control)(object)LabelX3);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtPassWord);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtUser);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX2);
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(4, 0);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(240, 322);
		groupPanel2.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		groupPanel2.Style.BackColorGradientAngle = 90;
		groupPanel2.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		groupPanel2.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel2.Style.BorderBottomWidth = 3;
		groupPanel2.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		groupPanel2.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel2.Style.BorderLeftWidth = 3;
		groupPanel2.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel2.Style.BorderRightWidth = 3;
		groupPanel2.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel2.Style.BorderTopWidth = 3;
		groupPanel2.Style.CornerDiameter = 4;
		groupPanel2.Style.CornerType = eCornerType.Rounded;
		groupPanel2.Style.TextAlignment = eStyleTextAlignment.Far;
		groupPanel2.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		groupPanel2.Style.TextLineAlignment = eStyleTextAlignment.Near;
		groupPanel2.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel2.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel2).TabIndex = 0;
		((Control)groupPanel2).Text = "اطلاعات  حساب کاربری";
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).Location = new Point(136, 10);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(92, 23);
		((Control)label1).TabIndex = 13;
		((Control)label1).Text = "طریقه ارسال:";
		label1.TextAlign = (ContentAlignment)64;
		((Control)CmbType).Anchor = (AnchorStyles)13;
		CmbType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbType).FormattingEnabled = true;
		CmbType.Items.AddRange(new object[2] { "سامانه اینترنتی", "سیم کارت" });
		((Control)CmbType).Location = new Point(3, 12);
		((Control)CmbType).Name = "CmbType";
		((Control)CmbType).Size = new Size(127, 21);
		((Control)CmbType).TabIndex = 0;
		CmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
		((Control)LabelX7).Anchor = (AnchorStyles)9;
		((Control)LabelX7).BackColor = Color.Transparent;
		((Control)LabelX7).Location = new Point(136, 171);
		((Control)LabelX7).Name = "LabelX7";
		((Control)LabelX7).Size = new Size(95, 23);
		((Control)LabelX7).TabIndex = 11;
		((Control)LabelX7).Text = "شماره فرستنده:";
		LabelX7.TextAlign = (ContentAlignment)64;
		((Control)TxtSenderNumber).Anchor = (AnchorStyles)13;
		TxtSenderNumber.Border.Class = "TextBoxBorder";
		TxtSenderNumber.Border.CornerType = eCornerType.Square;
		((Control)TxtSenderNumber).Location = new Point(3, 173);
		((Control)TxtSenderNumber).Name = "TxtSenderNumber";
		((Control)TxtSenderNumber).Size = new Size(127, 21);
		((Control)TxtSenderNumber).TabIndex = 6;
		((Control)LabelX6).Anchor = (AnchorStyles)9;
		((Control)LabelX6).Location = new Point(136, 37);
		((Control)LabelX6).Name = "LabelX6";
		((Control)LabelX6).Size = new Size(43, 23);
		((Control)LabelX6).TabIndex = 9;
		((Control)LabelX6).Text = "عنوان:";
		LabelX6.TextAlign = (ContentAlignment)64;
		((Control)TxtTitle).Anchor = (AnchorStyles)13;
		TxtTitle.Border.Class = "TextBoxBorder";
		TxtTitle.Border.CornerType = eCornerType.Square;
		((Control)TxtTitle).Location = new Point(3, 39);
		((Control)TxtTitle).Name = "TxtTitle";
		((Control)TxtTitle).Size = new Size(127, 21);
		((Control)TxtTitle).TabIndex = 1;
		((Control)TxtTitle).Enter += TxtTitle_Enter;
		((Control)TxtTitle).Leave += TxtTitle_Leave;
		((Control)TxtDomain).Anchor = (AnchorStyles)13;
		TxtDomain.Border.Class = "TextBoxBorder";
		TxtDomain.Border.CornerType = eCornerType.Square;
		((Control)TxtDomain).Location = new Point(3, 147);
		((Control)TxtDomain).Name = "TxtDomain";
		((Control)TxtDomain).Size = new Size(127, 21);
		((Control)TxtDomain).TabIndex = 5;
		((Control)TxtDescription).Anchor = (AnchorStyles)13;
		TxtDescription.Border.Class = "TextBoxBorder";
		TxtDescription.Border.CornerType = eCornerType.Square;
		((Control)TxtDescription).Location = new Point(3, 228);
		((TextBoxBase)TxtDescription).Multiline = true;
		((Control)TxtDescription).Name = "TxtDescription";
		((Control)TxtDescription).Size = new Size(127, 66);
		((Control)TxtDescription).TabIndex = 8;
		((Control)TxtPassWord2).Anchor = (AnchorStyles)13;
		TxtPassWord2.Border.Class = "TextBoxBorder";
		TxtPassWord2.Border.CornerType = eCornerType.Square;
		((Control)TxtPassWord2).Location = new Point(3, 120);
		((Control)TxtPassWord2).Name = "TxtPassWord2";
		((TextBox)TxtPassWord2).PasswordChar = '*';
		((Control)TxtPassWord2).Size = new Size(127, 21);
		((Control)TxtPassWord2).TabIndex = 4;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((Control)LabelX5).BackColor = Color.Transparent;
		((Control)LabelX5).Location = new Point(136, 145);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).Size = new Size(44, 23);
		((Control)LabelX5).TabIndex = 0;
		((Control)LabelX5).Text = "دامنه:";
		LabelX5.TextAlign = (ContentAlignment)64;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((Control)LabelX4).BackColor = Color.Transparent;
		((Control)LabelX4).Location = new Point(136, 226);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).Size = new Size(59, 23);
		((Control)LabelX4).TabIndex = 8;
		((Control)LabelX4).Text = "توضیحات:";
		LabelX4.TextAlign = (ContentAlignment)64;
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((Control)LabelX3).BackColor = Color.Transparent;
		((Control)LabelX3).Location = new Point(136, 119);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).Size = new Size(83, 23);
		((Control)LabelX3).TabIndex = 6;
		((Control)LabelX3).Text = "تکرار رمز عبور:";
		LabelX3.TextAlign = (ContentAlignment)64;
		((Control)TxtPassWord).Anchor = (AnchorStyles)13;
		TxtPassWord.Border.Class = "TextBoxBorder";
		TxtPassWord.Border.CornerType = eCornerType.Square;
		((Control)TxtPassWord).Location = new Point(3, 93);
		((Control)TxtPassWord).Name = "TxtPassWord";
		((TextBox)TxtPassWord).PasswordChar = '*';
		((Control)TxtPassWord).Size = new Size(127, 21);
		((Control)TxtPassWord).TabIndex = 3;
		((Control)TxtUser).Anchor = (AnchorStyles)13;
		TxtUser.Border.Class = "TextBoxBorder";
		TxtUser.Border.CornerType = eCornerType.Square;
		((Control)TxtUser).Location = new Point(3, 66);
		((Control)TxtUser).Name = "TxtUser";
		((Control)TxtUser).Size = new Size(127, 21);
		((Control)TxtUser).TabIndex = 2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((Control)labelX1).Location = new Point(136, 64);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(70, 23);
		((Control)labelX1).TabIndex = 2;
		((Control)labelX1).Text = "نام کاربری:";
		labelX1.TextAlign = (ContentAlignment)64;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((Control)labelX2).Location = new Point(136, 91);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(59, 23);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "رمز عبور:";
		labelX2.TextAlign = (ContentAlignment)64;
		((Control)txtMinimumCredit).Anchor = (AnchorStyles)13;
		((Control)txtMinimumCredit).Location = new Point(3, 200);
		((Control)txtMinimumCredit).Name = "txtMinimumCredit";
		((Control)txtMinimumCredit).Size = new Size(127, 21);
		((Control)txtMinimumCredit).TabIndex = 7;
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Location = new Point(136, 198);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(95, 23);
		((Control)label2).TabIndex = 11;
		((Control)label2).Text = "کمینه اعتبار:";
		label2.TextAlign = (ContentAlignment)64;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(253, 370);
		((Control)this).Controls.Add((Control)(object)groupPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMAccount";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "حساب کاربری";
		((Form)this).Load += FRMAccount_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMAccount_KeyDown);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)groupPanel2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
