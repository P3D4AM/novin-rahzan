using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.UC.Base;

namespace TimeAttendanceUI.Forms.Device;

public class FrmDeviceMain : BaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private UCDevice ucDevice1;

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

	public FrmDeviceMain(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, ucDevice1.LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDeviceMain.cs");
			((Control)ucDevice1.TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Device.DeviceMain)
		{
			((Form)this).Close();
		}
		else
		{
			((BaseForm)this).OnLoad(e);
		}
	}

	private async void FRMDeviceMain_KeyDown(object sender, KeyEventArgs e)
	{
		Keys keyCode = e.KeyCode;
		if ((int)keyCode <= 45)
		{
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 45)
				{
					e.Handled = true;
					ucDevice1.Insert();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		else if ((int)keyCode != 46)
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
				ucDevice1.UpDate();
			}
		}
		else
		{
			e.Handled = true;
			ucDevice1.Delete();
		}
	}

	private void ucDevice1_Exit_UserControl()
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void ucDevice1_Load(object sender, EventArgs e)
	{
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
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		ucDevice1 = new UCDevice(((CacheBaseForm)this).ContextGuid);
		((Control)this).SuspendLayout();
		((Control)ucDevice1).Dock = (DockStyle)5;
		((Control)ucDevice1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucDevice1).Location = new Point(0, 0);
		((Control)ucDevice1).Name = "ucDevice1";
		((Control)ucDevice1).RightToLeft = (RightToLeft)1;
		((Control)ucDevice1).Size = new Size(610, 343);
		((Control)ucDevice1).TabIndex = 0;
		ucDevice1.Exit_UserControl += ucDevice1_Exit_UserControl;
		((UserControl)ucDevice1).Load += ucDevice1_Load;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(610, 343);
		((Control)this).Controls.Add((Control)(object)ucDevice1);
		((Control)this).Name = "FrmDeviceMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[دستگاه]";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FRMDeviceMain_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
