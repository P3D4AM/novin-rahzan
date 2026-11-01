using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceUI.Salary.UC;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmShiftMain : BaseForm
{
	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private Font _font;

	private IContainer components;

	private UcShiftMain ucShiftMain1;

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

	public short ShiftId => (short)ucShiftMain1.SelectedItem.Id;

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

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[شیفت]", out var _))
			{
				trDictionary.Add("[شیفت]", Utilities.GetTranslate("[شیفت]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 52);
		}
	}

	public async Task<Shifts> GetSelectedShiftAsync(CancellationToken token)
	{
		try
		{
			if (ucShiftMain1.SelectedItem == null)
			{
				return null;
			}
			Shifts selectedItem = ucShiftMain1.SelectedItem;
			selectedItem.LstShiftDet = await ShiftDet.GetAllAsync(((CacheBaseForm)this).ContextGuid, ucShiftMain1.SelectedItem.Id, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetSelectedShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 62);
		}
		return ucShiftMain1.SelectedItem;
	}

	public FrmShiftMain(string contextGuid, Font font, bool Select)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			Task.Run((Func<Task?>)SetTrDictionaryAsync);
			((Control)ucShiftMain1.BtnSelect).Visible = Select;
			((Control)ucShiftMain1.TxtSearch).Focus();
			((Control)this).Text = "شیفت";
			if (trDictionary.TryGetValue("[شیفت]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 79);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Shift.ShiftMain)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 93);
		}
	}

	private void ucShiftMain1_Exit_UserControl()
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucShiftMain1_Exit_UserControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 103);
		}
	}

	private async void FRMShiftMain_KeyDown(object sender, KeyEventArgs e)
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
							ucShiftMain1.Insert();
						}
					}
					else
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else if (((Control)ucShiftMain1.BtnSelect).Visible)
				{
					e.Handled = true;
					ucShiftMain1.SelectPerform();
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				else
				{
					e.Handled = true;
					ucShiftMain1.Edit();
				}
			}
			else if ((int)keyCode <= 83)
			{
				if ((int)keyCode != 46)
				{
					if ((int)keyCode == 83 && e.Control)
					{
						_loadDataToken?.Cancel();
						_loadDataToken = new CancellationTokenSource();
						CancellationToken token = _loadDataToken.Token;
						await ucShiftMain1.SetStAsync(!ucShiftMain1.GetSt(), token);
					}
				}
				else
				{
					e.Handled = true;
					ucShiftMain1.Delete();
				}
			}
			else if ((int)keyCode != 118)
			{
				if ((int)keyCode == 123)
				{
					e.Handled = true;
					FrmCalc val = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					((Form)val).ShowDialog();
					((Component)val).Dispose();
				}
			}
			else
			{
				e.Handled = true;
				ucShiftMain1.Edit();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMShiftMain_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMShiftMain.cs", 160);
		}
	}

	private void InitializeComponent()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		ucShiftMain1 = new UcShiftMain(((CacheBaseForm)this).ContextGuid, _font);
		((Control)this).SuspendLayout();
		((Control)ucShiftMain1).Dock = (DockStyle)5;
		((Control)ucShiftMain1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)ucShiftMain1).Location = new Point(0, 0);
		((Control)ucShiftMain1).Name = "ucShiftMain1";
		((Control)ucShiftMain1).RightToLeft = (RightToLeft)1;
		((Control)ucShiftMain1).Size = new Size(650, 364);
		((Control)ucShiftMain1).TabIndex = 0;
		ucShiftMain1.Exit_UserControl += ucShiftMain1_Exit_UserControl;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(650, 364);
		((Control)this).Controls.Add((Control)(object)ucShiftMain1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).MinimumSize = new Size(666, 402);
		((Control)this).Name = "FrmShiftMain";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "شــیفـت";
		((Form)this).WindowState = (FormWindowState)2;
		((Control)this).KeyDown += new KeyEventHandler(FRMShiftMain_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
