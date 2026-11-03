using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EntityCache.Business.LkService;
using FormCustom.Froms.LkManage;
using FormCustom.LockService.Class;
using LK;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.LkService;

namespace FormCustom.LockService.Forms;

public class FRMDemo : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _checkacttoken;

	private Package _pakage;

	private Container components;

	internal Button BTNCancel;

	internal Button BTNOK;

	internal Label Label1;

	internal Label Label2;

	internal Label Label3;

	internal Label Label4;

	internal Label Label5;

	internal Label Label6;

	internal Label LBLDaysCount;

	internal Label LBLDemoExpire;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_checkacttoken?.Cancel();
				_checkacttoken?.Dispose();
				_checkacttoken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 36);
		}
	}

	internal string GetPackageName(Package packageName)
	{
		try
		{
			return packageName switch
			{
				Package.Accounting => "حسابداری  ", 
				Package.HoghoghVaDastmozd => "  حقوق و دستمزد ", 
				Package.TamdideAccounting => "  حسابداری ", 
				Package.TamdideScale => " ترازو ", 
				Package.TamdideHozor => "حضور و غیاب   ", 
				_ => "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPackageName", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 54);
			return "";
		}
	}

	public FRMDemo(string contextGuid, Package pakage)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_pakage = pakage;
			if (pakage == Package.HoghoghVaDastmozd)
			{
				((Control)Label3).Text = ((Control)Label3).Text.Replace("نرم افزار حسابداری ", "زیر بسته حقوق و دستمزد");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 70);
		}
	}

	public async void BTNOK_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> ret = new ReturnedSaveFuncInfoWithValue<LkDemoInformation>();
		try
		{
			try
			{
				if (!((Control)BTNOK).Enabled || !((Control)BTNOK).Visible)
				{
					goto end_IL_003b;
				}
				new ReturnedSaveFuncInfoWithValue<int>();
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = ((_pakage != Package.HoghoghVaDastmozd) ? (await LkActivatorBaseBusiness.GetRemDaysAsync(base.ContextGuid, _pakage)) : Demo.GetRemDays(_pakage));
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = returnedSaveFuncInfoWithValue;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 83);
				if (returnedSaveFuncInfoWithValue2.value > 0)
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else
				{
					BTNCancel.PerformClick();
				}
				goto end_IL_0032;
				end_IL_003b:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 93);
				ret.AddReturnedValue(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 94);
				goto end_IL_0032;
			}
			end_IL_0032:;
		}
		finally
		{
			if (!IsDisposed && ret.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در فعالسازی دمو", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs");
			}
		}
	}

	public async void BTNCancel_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfoWithValue<LockInformation> ret = new ReturnedSaveFuncInfoWithValue<LockInformation>();
		ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = ret;
		if (returnedSaveFuncInfoWithValue.value == null)
		{
			returnedSaveFuncInfoWithValue.value = new LockInformation();
		}
		try
		{
			_ = 1;
			try
			{
				if (_pakage == Package.HoghoghVaDastmozd)
				{
					ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(_pakage);
					ret.AddReturnedValue(regObject, ReturnedState.None, "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 113);
					RegistryObject value = regObject.value;
					ret.AddReturnedValue(value.SetIsDemo(value: false, _pakage), ReturnedState.None, "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 115);
					CLSRegistry.SetRegObject(value, _pakage);
				}
				else
				{
					ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue2 = await LkActivatorBaseBusiness.GetLockInformationAsync(base.ContextGuid, _pakage);
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 122);
					ret.value = returnedSaveFuncInfoWithValue2.value;
					await LkActivatorBaseBusiness.SetLockInformationAsync(base.ContextGuid, ret.value, _pakage);
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 131);
				WebErrorLog.Instance.StartLog(ex, "", "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 132);
			}
		}
		finally
		{
			if (ret.HasError)
			{
				if (!IsDisposed)
				{
					await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در لغو عملیات دمو", "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs");
				}
				else
				{
					Application.Run((Form)(object)new FrmShowErrorMessage(base.ContextGuid, ret, "خطا در لغو عملیات دمو", (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs"));
				}
			}
		}
	}

	public void FRMDemo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					BTNOK.PerformClick();
				}
			}
			else
			{
				BTNCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMDemo_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 159);
		}
	}

	public async void FRMDemo_Load(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			((Control)Label3).Text = "مشتری گرامی ، شما در حال حاضر از نسخه دمو (آزمایشی) نرم افزار  " + GetPackageName(_pakage) + " " + ResourceManagement.TextBrandResourceManagement.CompanyName + " استفاده مینمایید . لطفا جهت تهیه نسخه کامل نرم افزار با واحد فروش تماس حاصل نمایید .";
			new ReturnedSaveFuncInfoWithValue<int>();
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = ((_pakage != Package.HoghoghVaDastmozd) ? (await LkActivatorBaseBusiness.GetRemDaysAsync(base.ContextGuid, _pakage)) : Demo.GetRemDays(_pakage));
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "FRMDemo_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 178);
			if (returnedSaveFuncInfoWithValue.value > 0)
			{
				((Control)LBLDaysCount).Text = returnedSaveFuncInfoWithValue.value.ToString();
			}
			else if (returnedSaveFuncInfoWithValue.value <= 0)
			{
				((Control)LBLDemoExpire).Visible = true;
				((Control)BTNOK).Enabled = false;
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "FRMDemo_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 194);
			WebErrorLog.Instance.StartLog(ex, "", "FRMDemo_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 195);
		}
		finally
		{
			if (res.HasError && !IsDisposed)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "خطا در فعالسازی دمو", "FRMDemo_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs");
			}
		}
	}

	public async void LBLDemoExpire_Click(object sender, EventArgs e)
	{
		try
		{
			if (_pakage == Package.Accounting)
			{
				_checkacttoken?.Cancel();
				_checkacttoken = new CancellationTokenSource(60000);
				CancellationToken token = _checkacttoken.Token;
				ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue = await AccountingLkActivatorBusiness.CheckActivationAsync(base.ContextGuid, ENSource.Accounting, new LkClientManager(base.ContextGuid, this), this, token);
				frmActivationNew obj = new frmActivationNew(base.ContextGuid, returnedSaveFuncInfoWithValue.value, Package.TamdideAccounting, isDemo: true);
				if (obj != null)
				{
					((Form)(object)this).CheckVisibility("LBLDemoExpire_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs");
				}
				if ((int)((Form)obj).ShowDialog((IWin32Window)(object)this) == 1)
				{
					MessageBox.Show("خطا در فعالسازی نرم افزار. لطفا با پشتیبانی تماس بگیرید. کد خطا 2003");
					Process.GetCurrentProcess().Kill();
				}
			}
			else if (_pakage == Package.HoghoghVaDastmozd)
			{
				FRMActivation fRMActivation = new FRMActivation(base.ContextGuid, Package.TamdideHozor);
				if (fRMActivation != null)
				{
					((Form)(object)this).CheckVisibility("LBLDemoExpire_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs");
				}
				if ((int)((Form)fRMActivation).ShowDialog((IWin32Window)(object)this) == 1)
				{
					MessageBox.Show("خطا در فعالسازی نرم افزار. لطفا با پشتیبانی تماس بگیرید. کد خطا 2003");
					Process.GetCurrentProcess().Kill();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LBLDemoExpire_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\FRMDemo.cs", 232);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Expected O, but got Unknown
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Expected O, but got Unknown
		BTNCancel = new Button();
		BTNOK = new Button();
		Label1 = new Label();
		Label2 = new Label();
		Label3 = new Label();
		Label4 = new Label();
		Label5 = new Label();
		Label6 = new Label();
		LBLDaysCount = new Label();
		LBLDemoExpire = new Label();
		((Control)this).SuspendLayout();
		((Control)BTNCancel).Anchor = (AnchorStyles)6;
		BTNCancel.DialogResult = (DialogResult)2;
		((Control)BTNCancel).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNCancel).ForeColor = Color.Blue;
		((Control)BTNCancel).Location = new Point(49, 268);
		((Control)BTNCancel).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNCancel).Name = "BTNCancel";
		((Control)BTNCancel).Size = new Size(244, 34);
		((Control)BTNCancel).TabIndex = 0;
		((Control)BTNCancel).Text = "&[انــــصراف]";
		((ButtonBase)BTNCancel).UseVisualStyleBackColor = true;
		((Control)BTNCancel).Click += BTNCancel_Click;
		((Control)BTNOK).Anchor = (AnchorStyles)6;
		((Control)BTNOK).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)BTNOK).ForeColor = Color.Blue;
		((Control)BTNOK).Location = new Point(49, 187);
		((Control)BTNOK).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(244, 75);
		((Control)BTNOK).TabIndex = 0;
		((Control)BTNOK).Text = "&[تــــایید]\r\n[(اجرای آزمایشی)]";
		((ButtonBase)BTNOK).UseVisualStyleBackColor = true;
		((Control)BTNOK).Click += BTNOK_Click;
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(14, 9);
		((Control)Label1).Margin = new Padding(4, 0, 4, 0);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(897, 48);
		((Control)Label1).TabIndex = 1;
		((Control)Label1).Text = "[توجه]";
		((Control)Label2).BackColor = Color.Transparent;
		Label2.BorderStyle = (BorderStyle)1;
		((Control)Label2).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label2).ForeColor = Color.Red;
		((Control)Label2).Location = new Point(4, 4);
		((Control)Label2).Margin = new Padding(4, 0, 4, 0);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(844, 43);
		((Control)Label2).TabIndex = 1;
		((Control)Label2).Text = "__________________________________________________________________________________";
		((Control)Label3).BackColor = Color.Transparent;
		((Control)Label3).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).ForeColor = Color.LimeGreen;
		((Control)Label3).Location = new Point(14, 57);
		((Control)Label3).Margin = new Padding(4, 0, 4, 0);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(897, 86);
		((Control)Label3).TabIndex = 2;
		((Control)Label4).BackColor = Color.Transparent;
		Label4.BorderStyle = (BorderStyle)2;
		((Control)Label4).Font = new Font("B Nazanin", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label4).ForeColor = Color.Black;
		((Control)Label4).Location = new Point(526, 252);
		((Control)Label4).Margin = new Padding(4, 0, 4, 0);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(385, 51);
		((Control)Label4).TabIndex = 2;
		((Control)Label4).Text = "[پشتیبانی فنـی] : ";
		Label4.TextAlign = (ContentAlignment)16;
		((Control)Label5).BackColor = Color.Transparent;
		((Control)Label5).Font = new Font("B Nazanin", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label5).ForeColor = Color.Black;
		((Control)Label5).Location = new Point(525, 187);
		((Control)Label5).Margin = new Padding(4, 0, 4, 0);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)0;
		((Control)Label5).Size = new Size(378, 65);
		((Control)Label5).TabIndex = 2;
		((Control)Label5).Text = "URL : \r\nE-Mail : \r\n";
		Label5.TextAlign = (ContentAlignment)16;
		((Control)Label6).BackColor = Color.Transparent;
		((Control)Label6).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label6).ForeColor = Color.LimeGreen;
		((Control)Label6).Location = new Point(14, 140);
		((Control)Label6).Margin = new Padding(4, 0, 4, 0);
		((Control)Label6).Name = "Label6";
		((Control)Label6).Size = new Size(103, 41);
		((Control)Label6).TabIndex = 3;
		((Control)Label6).Text = "[روز مانده]";
		((Control)LBLDaysCount).BackColor = Color.Transparent;
		((Control)LBLDaysCount).Font = new Font("B Titr", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLDaysCount).ForeColor = Color.Blue;
		((Control)LBLDaysCount).Location = new Point(124, 143);
		((Control)LBLDaysCount).Margin = new Padding(4, 0, 4, 0);
		((Control)LBLDaysCount).Name = "LBLDaysCount";
		((Control)LBLDaysCount).Size = new Size(63, 41);
		((Control)LBLDaysCount).TabIndex = 3;
		((Control)LBLDaysCount).Text = "0";
		((Control)LBLDemoExpire).BackColor = Color.Transparent;
		((Control)LBLDemoExpire).Font = new Font("B Titr", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLDemoExpire).ForeColor = Color.Red;
		((Control)LBLDemoExpire).Location = new Point(14, 130);
		((Control)LBLDemoExpire).Margin = new Padding(4, 0, 4, 0);
		((Control)LBLDemoExpire).Name = "LBLDemoExpire";
		((Control)LBLDemoExpire).Size = new Size(889, 51);
		((Control)LBLDemoExpire).TabIndex = 4;
		((Control)LBLDemoExpire).Text = "[مهلت اجرای آزمایشی نرم افزار به پایان رسیده است]";
		LBLDemoExpire.TextAlign = (ContentAlignment)32;
		((Control)LBLDemoExpire).Visible = false;
		((Control)LBLDemoExpire).Click += LBLDemoExpire_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)BTNOK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.WhiteSmoke;
		((Form)this).CancelButton = (IButtonControl)(object)BTNCancel;
		((Form)this).ClientSize = new Size(925, 314);
		((Control)this).Controls.Add((Control)(object)LBLDemoExpire);
		((Control)this).Controls.Add((Control)(object)Label6);
		((Control)this).Controls.Add((Control)(object)Label5);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)BTNOK);
		((Control)this).Controls.Add((Control)(object)BTNCancel);
		((Control)this).Controls.Add((Control)(object)LBLDaysCount);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMDemo";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[نرم افزار جامع حسابداری] ";
		((Form)this).Load += FRMDemo_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMDemo_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
