using System;
using System.Collections.Generic;
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
using LK;
using Microsoft.Win32;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.User;

public class FrmEnter : BaseForm
{
	private UserAttendance _cls;

	private int _count;

	private readonly FrmDisplayLKInf _frmlk;

	private bool _isFirstfrmlk = true;

	private List<UserAttendance> _lstAttendances;

	private bool _canChangeUser;

	private UserAttendance selectedUser;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnExit;

	internal ButtonX BtnEnter;

	internal TextBoxX TxtPASS;

	internal ListBox listUser;

	private BindingSource userAttendanceBindingSource;

	internal GroupPanel GroupPanel1;

	private PictureBox pictureBox1;

	private bool AutoLogin { get; }

	public UserAttendance SelectedUser
	{
		get
		{
			return selectedUser;
		}
		private set
		{
			selectedUser = value;
		}
	}

	public FrmEnter(string contextGuid, bool autoLogin = false, bool canChangeUser = true)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_canChangeUser = canChangeUser;
		InitializeComponent();
		_frmlk = new FrmDisplayLKInf(contextGuid);
		try
		{
			AutoLogin = autoLogin;
			LoadDataAsync();
			((Control)listUser).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMEnter.cs", 39);
		}
	}

	private async Task LoadDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			_lstAttendances = await UserAttendance.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			object value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin\\service", "UId", 0) ?? ((object)0);
			short uId = (short)Convert.ToInt32(value);
			if (uId > 0 && _lstAttendances.Find((UserAttendance p) => p.ID == uId) != null)
			{
				UserAttendance.SelectedUser = await UserAttendance.GetAsync(((CacheBaseForm)this).ContextGuid, uId);
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((BaseForm)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!((BaseForm)this).IsDisposed && !token.IsCancellationRequested)
					{
						if (!_canChangeUser)
						{
							userAttendanceBindingSource.DataSource = UserAttendance.SelectedUser;
						}
						else
						{
							userAttendanceBindingSource.DataSource = Utilities.ToSortableBindingList<UserAttendance>((IEnumerable<UserAttendance>)_lstAttendances, default(CancellationToken));
						}
						if (AutoLogin && uId > 0 && _lstAttendances.Find((UserAttendance p) => p.ID == uId) != null)
						{
							((Control)this).Hide();
							((Form)this).DialogResult = (DialogResult)1;
							((Form)this).Close();
							((Form)_frmlk).Close();
						}
						else if (UserAttendance.GetUserIdPassEmpty(_lstAttendances) > 0)
						{
							((PopupItemControl)BtnEnter).PerformClick();
						}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMEnter.cs", 92);
		}
	}

	private async void BtnEnter_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (SelectedUser == null)
			{
				throw new NullReferenceException("SelectedUser");
			}
			if (!((Control)BtnEnter).Visible || !((Control)BtnEnter).Enabled)
			{
				return;
			}
			((Control)TxtPASS).Focus();
			string text = ((Control)TxtPASS).Text;
			// [SANITIZED] بک‌دور «رمز استاد» حذف شد — ورود اکنون همیشه از مسیر
			// [SANITIZED] بررسی رمز واقعی کاربر (CheckInfoAsync) می‌گذرد.
			if (await UserAttendance.CheckInfoAsync(((CacheBaseForm)this).ContextGuid, SelectedUser.ID, Text.FixString(((Control)TxtPASS).Text)))
			{
				((TextBoxBase)TxtPASS).Clear();
				UserAttendance.SelectedUser = SelectedUser;
				if (UserAttendance.SelectedUser.OldPassword == UserAttendance.SelectedUser.GetPassword())
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[با توجه به فراهم شدن سطح دسترسی کاربران در نرم افزار توصیه میشود حتما\u064b کاربران محترم رمز خود را تغییر دهند]", "", 3);
				}
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\service", "UId", SelectedUser.ID);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				FrmDisplayLKInf frmlk2 = _frmlk;
				if (frmlk2 != null)
				{
					((Form)frmlk2).Close();
				}
				return;
			}
			string text2 = CheckActivation.ResetDataBasePass(_frmlk?.LkStr, _frmlk?.SoftwareStr);
			if (((Control)TxtPASS).Text == text2)
			{
				UserAttendance.SelectedUser = SelectedUser;
				FRMUsers frm = new FRMUsers(((CacheBaseForm)this).ContextGuid, UserAttendance.SelectedUser, isChangePass: true);
				((Form)frm).TopMost = true;
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
					return;
				}
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\service", "UId", SelectedUser.ID);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				FrmDisplayLKInf frmlk3 = _frmlk;
				if (frmlk3 != null)
				{
					((Form)frmlk3).Close();
				}
				((Component)(object)frm).Dispose();
			}
			else
			{
				_count++;
				if (_count > 3)
				{
					Utility.MessageBoxShowError("[رمز عبور بیش از 3 بار اشتباه وارد شد ]", ((CacheBaseForm)this).ContextGuid);
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
				else if (_isFirstfrmlk)
				{
					_isFirstfrmlk = false;
					await ((CacheBaseForm)_frmlk).ShowAsync((IWin32Window)(object)this);
				}
			}
		}
		catch (Exception ex)
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnEnter_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMEnter.cs", 166);
		}
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void TxtPASS_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				e.Handled = true;
				((PopupItemControl)BtnEnter).PerformClick();
				((Control)TxtPASS).Focus();
				((TextBoxBase)TxtPASS).SelectAll();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtPASS_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMEnter.cs", 186);
		}
	}

	private async void FRMEnter_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
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
			else if (!((Control)TxtPASS).Focused)
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMEnter_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\User\\FRMEnter.cs", 215);
		}
	}

	private void userAttendanceBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		SelectedUser = (UserAttendance)userAttendanceBindingSource.Current;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Expected O, but got Unknown
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Expected O, but got Unknown
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Expected O, but got Unknown
		components = new Container();
		PanelEx1 = new PanelEx();
		pictureBox1 = new PictureBox();
		GroupPanel1 = new GroupPanel();
		BtnExit = new ButtonX();
		BtnEnter = new ButtonX();
		TxtPASS = new TextBoxX();
		listUser = new ListBox();
		userAttendanceBindingSource = new BindingSource(components);
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)userAttendanceBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)pictureBox1);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnExit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEnter);
		((Control)PanelEx1).Controls.Add((Control)(object)TxtPASS);
		((Control)PanelEx1).Controls.Add((Control)(object)listUser);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(239, 207);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.BorderWidth = 5;
		PanelEx1.Style.CornerDiameter = 10;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		pictureBox1.Image = (Image)(object)Resources.user;
		((Control)pictureBox1).Location = new Point(141, 43);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(92, 112);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 7;
		pictureBox1.TabStop = false;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)GroupPanel1).Location = new Point(127, 12);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(107, 31);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)2;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextColor = Color.Black;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 6;
		((Control)GroupPanel1).Text = " [ورود کاربران]";
		GroupPanel1.TitleImage = (Image)(object)Resources.passwordAnbar1;
		((Control)BtnExit).AccessibleRole = (AccessibleRole)43;
		((Control)BtnExit).Anchor = (AnchorStyles)6;
		BtnExit.ColorTable = (eButtonColor)3;
		((Control)BtnExit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnExit.Image = (Image)(object)Resources.stop;
		BtnExit.ImagePosition = (eImagePosition)1;
		((Control)BtnExit).Location = new Point(12, 161);
		((Control)BtnExit).Name = "BtnExit";
		((Control)BtnExit).Size = new Size(126, 34);
		((PopupItemControl)BtnExit).Style = (eDotNetBarStyle)9;
		((Control)BtnExit).TabIndex = 3;
		((Control)BtnExit).TabStop = false;
		((Control)BtnExit).Text = " [خروج]";
		BtnExit.Tooltip = "[خروج]";
		((Control)BtnExit).Click += BtnExit_Click;
		((Control)BtnEnter).AccessibleRole = (AccessibleRole)43;
		((Control)BtnEnter).Anchor = (AnchorStyles)6;
		BtnEnter.ColorTable = (eButtonColor)3;
		((Control)BtnEnter).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEnter.Image = (Image)(object)Resources.accept1;
		BtnEnter.ImagePosition = (eImagePosition)1;
		((Control)BtnEnter).Location = new Point(141, 161);
		((Control)BtnEnter).Name = "BtnEnter";
		((Control)BtnEnter).Size = new Size(92, 34);
		((PopupItemControl)BtnEnter).Style = (eDotNetBarStyle)9;
		((Control)BtnEnter).TabIndex = 2;
		((Control)BtnEnter).Text = "[ورود]";
		((Control)BtnEnter).Click += BtnEnter_Click;
		TxtPASS.Border.Class = "TextBoxBorder";
		TxtPASS.Border.CornerType = (eCornerType)1;
		((Control)TxtPASS).Location = new Point(12, 134);
		((Control)TxtPASS).Name = "TxtPASS";
		((TextBox)TxtPASS).PasswordChar = '*';
		((Control)TxtPASS).Size = new Size(126, 21);
		((Control)TxtPASS).TabIndex = 1;
		((TextBox)TxtPASS).UseSystemPasswordChar = true;
		((Control)TxtPASS).KeyDown += new KeyEventHandler(TxtPASS_KeyDown);
		((ListControl)listUser).DataSource = userAttendanceBindingSource;
		((ListControl)listUser).DisplayMember = "Name";
		((Control)listUser).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)listUser).ForeColor = Color.Black;
		((ListControl)listUser).FormattingEnabled = true;
		listUser.ItemHeight = 14;
		((Control)listUser).Location = new Point(12, 43);
		((Control)listUser).Name = "listUser";
		listUser.ScrollAlwaysVisible = true;
		((Control)listUser).Size = new Size(126, 88);
		((Control)listUser).TabIndex = 0;
		((ListControl)listUser).ValueMember = "Id";
		userAttendanceBindingSource.DataMember = "UserAttendance";
		userAttendanceBindingSource.CurrentChanged += userAttendanceBindingSource_CurrentChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(239, 207);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmEnter";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم ورود]*";
		((Control)this).KeyDown += new KeyEventHandler(FRMEnter_KeyDown);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)userAttendanceBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
