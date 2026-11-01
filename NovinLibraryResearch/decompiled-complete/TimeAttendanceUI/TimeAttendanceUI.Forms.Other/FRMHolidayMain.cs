using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.UC.Base;

namespace TimeAttendanceUI.Forms.Other;

public class FRMHolidayMain : BaseForm
{
	private string frm_Text = "[تقویم]";

	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private Font _font;

	private IContainer components;

	private UcHoliday ucHoliday1;

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
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FRMHolidayMain(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			((Control)this).Text = "تقویم";
			frm_Text = "[تقویم]";
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			_ = _loadDataToken.Token;
			((Control)ucHoliday1.TxtSearch).Focus();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationHolidays)
		{
			((Form)this).Close();
		}
		else
		{
			((BaseForm)this).OnLoad(e);
		}
	}

	private void FRMHolidayMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				ucHoliday1.GotoMonth(e);
				return;
			}
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode != 45)
				{
					if ((int)keyCode == 80 && e.Control)
					{
						ucHoliday1.Print();
					}
				}
				else
				{
					e.Handled = true;
					ucHoliday1.Insert();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMHolidayMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMHolidayMain.cs", 96);
		}
	}

	private void ucHoliday1_Exit_UserControl()
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		ucHoliday1 = new UcHoliday(((CacheBaseForm)this).ContextGuid, new Font("tahoma", 12f));
		((Control)this).SuspendLayout();
		((Control)ucHoliday1).Dock = (DockStyle)5;
		((Control)ucHoliday1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucHoliday1).Location = new Point(0, 0);
		((Control)ucHoliday1).Name = "ucHoliday1";
		((Control)ucHoliday1).RightToLeft = (RightToLeft)1;
		((Control)ucHoliday1).Size = new Size(744, 448);
		((Control)ucHoliday1).TabIndex = 0;
		ucHoliday1.ExitUserControl += ucHoliday1_Exit_UserControl;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(744, 448);
		((Control)this).Controls.Add((Control)(object)ucHoliday1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FRMHolidayMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "تعـطـیـلات";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FRMHolidayMain_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
