using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using Serilog;
using TimeAttendanceUI.UC.Base;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmMorakhasiDailyMain : BaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	private UCMorakhasiDaily ucMorakhasiDaily1;

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

	public FrmMorakhasiDailyMain(string contextGuid, Font font, List<int> pid, DateTime date1, DateTime date2, string desc, bool isCalcSaati)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)this).Font = font;
			InitializeComponent();
			ucMorakhasiDaily1.isCalcSaati = isCalcSaati;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, ucMorakhasiDaily1.LoadDataAsync(pid, date1, date2, desc, token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDailyMain.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void ucMorakhasiDaily1_Exit_UserControl()
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private async void FRMMorakhasiDailyMain_KeyDown(object sender, KeyEventArgs e)
	{
		Keys keyCode = e.KeyCode;
		if ((int)keyCode <= 45)
		{
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 45)
					{
						e.Handled = true;
						((PopupItemControl)ucMorakhasiDaily1.BtnAdd).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)ucMorakhasiDaily1.DGrid).Focus();
				((PopupItemControl)ucMorakhasiDaily1.BtnEdit).PerformClick();
			}
		}
		else if ((int)keyCode <= 80)
		{
			if ((int)keyCode != 46)
			{
				if ((int)keyCode == 80 && e.Control)
				{
					ucMorakhasiDaily1.Print();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)ucMorakhasiDaily1.BtnDelete).PerformClick();
			}
		}
		else if ((int)keyCode != 118)
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
			((PopupItemControl)ucMorakhasiDaily1.BtnEdit).PerformClick();
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

	private void InitializeComponent()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		ucMorakhasiDaily1 = new UCMorakhasiDaily(((CacheBaseForm)this).ContextGuid);
		((Control)ucMorakhasiDaily1).Dock = (DockStyle)5;
		((Control)ucMorakhasiDaily1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucMorakhasiDaily1.isCalcSaati = false;
		((Control)ucMorakhasiDaily1).Location = new Point(0, 0);
		((Control)ucMorakhasiDaily1).Name = "ucMorakhasiDaily1";
		((Control)ucMorakhasiDaily1).RightToLeft = (RightToLeft)1;
		((Control)ucMorakhasiDaily1).Size = new Size(857, 394);
		((Control)ucMorakhasiDaily1).TabIndex = 0;
		ucMorakhasiDaily1.Exit_UserControl += ucMorakhasiDaily1_Exit_UserControl;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(857, 394);
		((Control)this).Controls.Add((Control)(object)ucMorakhasiDaily1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).MinimumSize = new Size(873, 432);
		((Control)this).Name = "FrmMorakhasiDailyMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "مرخـصی روزانـه";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FRMMorakhasiDailyMain_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
