using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Froms.LkManage;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Validators;

namespace TimeAttendanceUI.Forms.User;

public class FRMUsers : LkUiFactory
{
	private UserAttendance _clsUser;

	private string Frm_Text = "[افزودن کاربر جدید]";

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private IContainer components;

	internal PanelEx PanelEx2;

	internal PictureBox PictureBox1;

	internal TextBoxX TxtDescription;

	internal TextBoxX TxtPass2;

	internal TextBoxX TxtPass;

	internal TextBoxX TxtUser;

	internal LabelX LabelX1;

	internal LabelX LabelX2;

	internal LabelX LabelX3;

	internal LabelX LabelX4;

	internal LabelX LabelX5;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private NumericUpDown numCode;

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

	public FRMUsers(string contextGuid)
		: base(contextGuid)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs");
			Frm_Text = "[افزودن کاربر جدید]";
			((Control)numCode).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 65);
		}
	}

	public FRMUsers(string contextGuid, UserAttendance user, bool isChangePass = false)
		: base(contextGuid)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		try
		{
			_clsUser = user;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs");
			Fillform();
			if (isChangePass)
			{
				NumericUpDown obj = numCode;
				TextBoxX txtDescription = TxtDescription;
				bool flag2 = (((Control)TxtUser).Enabled = false);
				bool enabled = (((Control)txtDescription).Enabled = flag2);
				((Control)obj).Enabled = enabled;
				TextBoxX txtPass = TxtPass;
				string text2 = (((Control)TxtPass2).Text = "");
				((Control)txtPass).Text = text2;
				((Control)TxtPass).Focus();
				Frm_Text = "[تغییر رمز کاربر]";
			}
			else
			{
				((Control)numCode).Focus();
				Frm_Text = "[ویرایش کاربر]";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 98);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			if (_clsUser == null)
			{
				_clsUser = new UserAttendance(((CacheBaseForm)this).ContextGuid);
				NumericUpDown val = numCode;
				((Control)val).Text = await UserAttendance.NextCodeAsync(((CacheBaseForm)this).ContextGuid);
			}
			Frm_Text = Utilities.GetTranslate(Frm_Text);
			((Control)this).Text = Frm_Text;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void Fillform()
	{
		try
		{
			((Control)numCode).Text = _clsUser.Code.ToString();
			((Control)TxtUser).Text = _clsUser.Name;
			((Control)TxtPass).Text = _clsUser.GetPassword();
			((Control)TxtPass2).Text = _clsUser.GetPassword();
			((Control)TxtDescription).Text = _clsUser.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Fillform", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 149);
		}
	}

	private bool GetData()
	{
		try
		{
			_clsUser.Code = Utilities.ParseToInt(((Control)numCode).Text, 0);
			_clsUser.Name = ((Control)TxtUser).Text.Trim();
			_clsUser.SetPassword(((Control)TxtPass).Text.Trim());
			_clsUser.Description = ((Control)TxtDescription).Text.Trim();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 164);
		}
		return false;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		try
		{
			if (!GetData())
			{
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			DialogResult val = CheckValidation.Validtor(contextGuid, await _clsUser.CheckValidationAsync(((Control)TxtPass2).Text, token), isShowMessage: true);
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					if (await _clsUser.SaveAsync(token))
					{
						((Form)this).DialogResult = (DialogResult)1;
						((Form)this).Close();
					}
					else
					{
						((Control)BtnAccept).Enabled = true;
						((Control)numCode).Focus();
					}
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 207);
		}
	}

	private async void FRMUsers_KeyDown(object sender, KeyEventArgs e)
	{
		try
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
				}
				else
				{
					e.Handled = true;
					((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
				}
			}
			else if ((int)keyCode != 116)
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
				((PopupItemControl)BtnAccept).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMUsers_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 240);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMUsers.cs", 253);
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Expected O, but got Unknown
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Expected O, but got Unknown
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be2: Expected O, but got Unknown
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Expected O, but got Unknown
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Expected O, but got Unknown
		PanelEx2 = new PanelEx();
		numCode = new NumericUpDown();
		PictureBox1 = new PictureBox();
		TxtDescription = new TextBoxX();
		TxtPass2 = new TextBoxX();
		TxtPass = new TextBoxX();
		TxtUser = new TextBoxX();
		LabelX1 = new LabelX();
		LabelX2 = new LabelX();
		LabelX3 = new LabelX();
		LabelX4 = new LabelX();
		LabelX5 = new LabelX();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)PanelEx2).SuspendLayout();
		((ISupportInitialize)numCode).BeginInit();
		((ISupportInitialize)PictureBox1).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx2.CanvasColor = SystemColors.Control;
		PanelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx2).Controls.Add((Control)(object)numCode);
		((Control)PanelEx2).Controls.Add((Control)(object)PictureBox1);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtDescription);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtPass2);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtPass);
		((Control)PanelEx2).Controls.Add((Control)(object)TxtUser);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX1);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX2);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX3);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX4);
		((Control)PanelEx2).Controls.Add((Control)(object)LabelX5);
		PanelEx2.DisabledBackColor = Color.Empty;
		((Control)PanelEx2).Location = new Point(12, 12);
		((Control)PanelEx2).Name = "PanelEx2";
		((Control)PanelEx2).Size = new Size(307, 188);
		PanelEx2.Style.Alignment = (StringAlignment)1;
		PanelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx2.Style.Border = (eBorderType)1;
		PanelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx2.Style.BorderWidth = 3;
		PanelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx2.Style.GradientAngle = 90;
		((Control)PanelEx2).TabIndex = 0;
		((Control)numCode).Anchor = (AnchorStyles)9;
		((Control)numCode).Location = new Point(15, 11);
		numCode.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numCode.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numCode).Name = "numCode";
		((Control)numCode).Size = new Size(149, 21);
		((Control)numCode).TabIndex = 35;
		((UpDownBase)numCode).TextAlign = (HorizontalAlignment)2;
		numCode.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)PictureBox1).Anchor = (AnchorStyles)9;
		PictureBox1.BorderStyle = (BorderStyle)1;
		PictureBox1.Image = (Image)(object)Resources.User1;
		((Control)PictureBox1).Location = new Point(258, 10);
		((Control)PictureBox1).Name = "PictureBox1";
		((Control)PictureBox1).Size = new Size(42, 48);
		PictureBox1.TabIndex = 2;
		PictureBox1.TabStop = false;
		TxtDescription.Border.Class = "TextBoxBorder";
		TxtDescription.Border.CornerType = (eCornerType)1;
		((Control)TxtDescription).Location = new Point(15, 118);
		((TextBoxBase)TxtDescription).Multiline = true;
		((Control)TxtDescription).Name = "TxtDescription";
		((Control)TxtDescription).Size = new Size(149, 55);
		((Control)TxtDescription).TabIndex = 8;
		TxtPass2.Border.Class = "TextBoxBorder";
		TxtPass2.Border.CornerType = (eCornerType)1;
		((Control)TxtPass2).Location = new Point(15, 91);
		((Control)TxtPass2).Name = "TxtPass2";
		((TextBox)TxtPass2).PasswordChar = '●';
		((Control)TxtPass2).Size = new Size(149, 21);
		((Control)TxtPass2).TabIndex = 6;
		((TextBox)TxtPass2).UseSystemPasswordChar = true;
		TxtPass.Border.Class = "TextBoxBorder";
		TxtPass.Border.CornerType = (eCornerType)1;
		((Control)TxtPass).Location = new Point(15, 64);
		((Control)TxtPass).Name = "TxtPass";
		((TextBox)TxtPass).PasswordChar = '●';
		((Control)TxtPass).Size = new Size(149, 21);
		((Control)TxtPass).TabIndex = 4;
		((TextBox)TxtPass).UseSystemPasswordChar = true;
		TxtUser.Border.Class = "TextBoxBorder";
		TxtUser.Border.CornerType = (eCornerType)1;
		((Control)TxtUser).Location = new Point(15, 37);
		((Control)TxtUser).Name = "TxtUser";
		((Control)TxtUser).Size = new Size(149, 21);
		((Control)TxtUser).TabIndex = 2;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX1).Location = new Point(170, 10);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).RightToLeft = (RightToLeft)1;
		((Control)LabelX1).Size = new Size(64, 23);
		((Control)LabelX1).TabIndex = 9;
		((Control)LabelX1).Text = "[کد] :";
		LabelX1.TextAlignment = (StringAlignment)2;
		((Control)LabelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX2).Location = new Point(170, 37);
		((Control)LabelX2).Name = "LabelX2";
		((Control)LabelX2).RightToLeft = (RightToLeft)1;
		((Control)LabelX2).Size = new Size(82, 23);
		((Control)LabelX2).TabIndex = 1;
		((Control)LabelX2).Text = "[نام کاربری] * :";
		LabelX2.TextAlignment = (StringAlignment)2;
		((Control)LabelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX3).Location = new Point(170, 64);
		((Control)LabelX3).Name = "LabelX3";
		((Control)LabelX3).RightToLeft = (RightToLeft)1;
		((Control)LabelX3).Size = new Size(82, 23);
		((Control)LabelX3).TabIndex = 3;
		((Control)LabelX3).Text = "[رمز عبور]* :";
		LabelX3.TextAlignment = (StringAlignment)2;
		((Control)LabelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX4).Location = new Point(170, 91);
		((Control)LabelX4).Name = "LabelX4";
		((Control)LabelX4).RightToLeft = (RightToLeft)1;
		((Control)LabelX4).Size = new Size(129, 23);
		((Control)LabelX4).TabIndex = 5;
		((Control)LabelX4).Text = "[تکرار رمز عبور] *:";
		LabelX4.TextAlignment = (StringAlignment)2;
		((Control)LabelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX5).Location = new Point(170, 118);
		((Control)LabelX5).Name = "LabelX5";
		((Control)LabelX5).RightToLeft = (RightToLeft)1;
		((Control)LabelX5).Size = new Size(115, 23);
		((Control)LabelX5).TabIndex = 7;
		((Control)LabelX5).Text = "[شرح] :";
		LabelX5.TextAlignment = (StringAlignment)2;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(323, 250);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 206);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(113, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(132, 206);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(188, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(323, 250);
		((Control)this).Controls.Add((Control)(object)PanelEx2);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(339, 289);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(339, 289);
		((Control)this).Name = "FRMUsers";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[کاربر]";
		((Control)this).KeyDown += new KeyEventHandler(FRMUsers_KeyDown);
		((Control)PanelEx2).ResumeLayout(false);
		((ISupportInitialize)numCode).EndInit();
		((ISupportInitialize)PictureBox1).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
