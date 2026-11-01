using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.UC.Base;

namespace TimeAttendanceUI.Forms.Person;

public class FrmSelectPersonel : BaseForm
{
	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private bool _isDisposed;

	private Font _font = new Font("tahoma", 10f);

	private IContainer components;

	private UcPersonel ucPersonel1;

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

	public string selectedCodeM => ucPersonel1.SelectedCodeM;

	public TimeAttendanceDAL.Dal.FLRPerson.Person SelectedPerson => ucPersonel1.SelectedPerson;

	public string selectedCodeT => ucPersonel1.SelectedCodeT;

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

	public FrmSelectPersonel(string contextGuid, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_font = font;
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 61);
		}
	}

	public FrmSelectPersonel(string contextGuid, bool isSelect, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		_font = font;
		try
		{
			InitializeComponent();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs");
			((Control)ucPersonel1.BtnSelect).Visible = isSelect;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 78);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			await ucPersonel1.LoadDataAsync(token);
			((Control)ucPersonel1.TxtSearch).Focus();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelMain)
			{
				((Form)this).Close();
			}
			else
			{
				((BaseForm)this).OnLoad(e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 105);
		}
	}

	private void ucPersonel1_Exit_UserControl()
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucPersonel1_Exit_UserControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 118);
		}
	}

	private async void FRMSelectPersonel_KeyDown(object sender, KeyEventArgs e)
	{
		try
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
							ucPersonel1.Insert();
						}
					}
					else
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else if (((Control)ucPersonel1.BtnSelect).Visible)
				{
					e.Handled = true;
					ucPersonel1.SelectPerform();
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
			else if ((int)keyCode <= 80)
			{
				if ((int)keyCode != 46)
				{
					if ((int)keyCode == 80 && e.Control)
					{
						ucPersonel1.Print();
					}
				}
				else
				{
					e.Handled = true;
					ucPersonel1.Delete();
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
				ucPersonel1.UpDate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSelectPersonel_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 168);
		}
	}

	private void FRMSelectPersonel_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (SelectedPerson != null)
			{
				((Form)this).DialogResult = (DialogResult)1;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSelectPersonel_FormClosing", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMSelectPersonel.cs", 179);
		}
	}

	private void InitializeComponent()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		ucPersonel1 = new UcPersonel(((CacheBaseForm)this).ContextGuid, _font);
		((Control)this).SuspendLayout();
		((Control)ucPersonel1).Dock = (DockStyle)5;
		((Control)ucPersonel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucPersonel1).Location = new Point(0, 0);
		((Control)ucPersonel1).Name = "ucPersonel1";
		((Control)ucPersonel1).RightToLeft = (RightToLeft)1;
		((Control)ucPersonel1).Size = new Size(915, 562);
		((Control)ucPersonel1).TabIndex = 0;
		ucPersonel1.Exit_UserControl += ucPersonel1_Exit_UserControl;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(915, 562);
		((Control)this).Controls.Add((Control)(object)ucPersonel1);
		((Control)this).MinimumSize = new Size(786, 387);
		((Control)this).Name = "FrmSelectPersonel";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[اشخاص]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosing += new FormClosingEventHandler(FRMSelectPersonel_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(FRMSelectPersonel_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
