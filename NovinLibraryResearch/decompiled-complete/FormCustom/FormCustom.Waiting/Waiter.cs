using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Waiting;

public class Waiter : INpDisposable, IDisposable
{
	private IWaiterUi _loadingForm;

	private Control _parent;

	private List<Control> _disabledItems;

	private bool _isDisposed;

	private Control _fromActiveControl;

	private string _callerMemberName = "";

	private string _callerFilePath = "";

	public bool IsDisposed => _isDisposed;

	public Waiter(string labelProgress, Control parent, Task task = null, bool freezeForm = false, EnWaiterType waiterType = EnWaiterType.Circle, Control activeControl = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		ctor(labelProgress, (Control)(((parent is Panel || parent is Form || parent is UserControl || parent is DataGridView) ? true : false) ? ((object)parent) : ((object)parent.FindForm())), task, freezeForm, waiterType, activeControl, memberName, sourceFilePath);
	}

	public Waiter(string labelProgress = "در حال پردازش", Form parent = null, Task task = null, bool freezeForm = false, EnWaiterType waiterType = EnWaiterType.Circle, Control activeControl = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		ctor(labelProgress, (Control)(object)parent, task, freezeForm, waiterType, activeControl, memberName, sourceFilePath);
	}

	private void ctor(string labelProgress, Control parent, Task task, bool freezeForm, EnWaiterType waiterType, Control activeControl, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			_parent = parent;
			_callerMemberName = memberName;
			_callerFilePath = sourceFilePath;
			_fromActiveControl = activeControl;
			if (parent == null)
			{
				_loadingForm = new WaiterForm(null, labelProgress, task);
				_loadingForm.OnAutoClose += _loadingForm_OnAutoClose;
				_loadingForm.Show();
				return;
			}
			if (parent is Form && freezeForm)
			{
				foreach (Control item in (ArrangedElementCollection)parent.Controls)
				{
					Control control = item;
					DisableControl(control);
				}
			}
			switch (waiterType)
			{
			case EnWaiterType.Circle:
				_loadingForm = new WaiterUserControl(parent, labelProgress, task, "ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs");
				break;
			case EnWaiterType.Rectangle:
				_loadingForm = new WaiterUserControlRectangle(parent, labelProgress, task);
				break;
			}
			_loadingForm.OnAutoClose += _loadingForm_OnAutoClose;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, "ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 96);
		}
	}

	private void DisableControl(Control control)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			if (_disabledItems == null)
			{
				_disabledItems = new List<Control>();
			}
			if (((object)control).GetType().Name.ToLower().Contains("grid"))
			{
				control.Enabled = false;
				control.EnabledChanged += Control_EnabledChanged;
				_disabledItems.Add(control);
			}
			int? obj;
			if (control == null)
			{
				obj = null;
			}
			else
			{
				ControlCollection controls = control.Controls;
				obj = ((controls != null) ? new int?(((ArrangedElementCollection)controls).Count) : null);
			}
			int? num = obj;
			if (num.GetValueOrDefault() > 0)
			{
				foreach (Control item in (ArrangedElementCollection)control.Controls)
				{
					Control control2 = item;
					DisableControl(control2);
				}
				return;
			}
			if (control.Enabled)
			{
				_disabledItems.Add(control);
				control.Enabled = false;
				control.EnabledChanged += Control_EnabledChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisableControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 121);
		}
	}

	private void Control_EnabledChanged(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			if (!IsDisposed)
			{
				((Control)sender).EnabledChanged -= Control_EnabledChanged;
				_disabledItems?.Remove((Control)sender);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Control_EnabledChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 132);
		}
	}

	private async Task _loadingForm_OnAutoClose(object sender, EventArgs e)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			if (IsDisposed)
			{
				return;
			}
			if (_parent == null || _parent.IsDisposed)
			{
				Dispose(disposing: true);
				return;
			}
			while (!_parent.IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || _parent.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			_parent.BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					Dispose();
				}
				catch (Exception ex2)
				{
					WebErrorLog instance = WebErrorLog.Instance;
					string[] obj = new string[8] { "Waiter.cs=>_loadingForm_OnAutoClose\r\n#ParentName:", null, null, null, null, null, null, null };
					Control parent = _parent;
					obj[1] = ((parent != null) ? parent.Name : null);
					obj[2] = "\r\n_parentText:";
					Control parent2 = _parent;
					obj[3] = ((parent2 != null) ? parent2.Text : null);
					obj[4] = " \r\nmemberName is ";
					obj[5] = _callerMemberName;
					obj[6] = " and sourceFilePath =";
					obj[7] = _callerFilePath;
					instance.StartLog(ex2, string.Concat(obj), "_loadingForm_OnAutoClose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 161);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, "_loadingForm_OnAutoClose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 168);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			List<Control> temp_disabledItems = _disabledItems?.ToList();
			_disabledItems = null;
			Control parent2 = _parent;
			if (parent2 != null && !parent2.IsDisposed)
			{
				List<Control> list = temp_disabledItems;
				if (list != null && list.Any())
				{
					Control parent = _parent;
					Control fromActiveControl = _fromActiveControl;
					Task.Run(() => SetFocusToActiveControlAsync(parent, fromActiveControl, temp_disabledItems));
				}
			}
			if (_loadingForm != null)
			{
				_loadingForm.HideAsync();
				_loadingForm.OnAutoClose -= _loadingForm_OnAutoClose;
				_loadingForm.Dispose();
				_loadingForm = null;
			}
			_parent = null;
			_fromActiveControl = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 199);
		}
	}

	private async Task SetFocusToActiveControlAsync(Control parent, Control fromActiveControl, List<Control> temp_disabledItems)
	{
		try
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(60000);
			CancellationToken token = cancellationTokenSource.Token;
			do
			{
				await Task.Delay(100, token);
				if (parent.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!parent.IsHandleCreated);
			if (parent.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			parent.BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!parent.IsDisposed)
					{
						foreach (Control temp_disabledItem in temp_disabledItems)
						{
							temp_disabledItem.EnabledChanged -= Control_EnabledChanged;
							temp_disabledItem.Enabled = true;
						}
						string name = parent.Name;
						object arg = parent.Focused;
						Form activeForm = Form.ActiveForm;
						Console.WriteLine($"waiter set focus. parent({name}).Focused:{arg}//ActiveForm:{((activeForm != null) ? ((Control)activeForm).Name : null)}");
						Control obj = fromActiveControl;
						if (obj != null && !obj.IsDisposed && !parent.IsDisposed)
						{
							Control obj2 = fromActiveControl;
							NumericUpDown val = (NumericUpDown)(object)((obj2 is NumericUpDown) ? obj2 : null);
							if (val != null)
							{
								((Control)val).Focus();
								((UpDownBase)val).Select(0, ((Control)val).Text.Length);
							}
							else
							{
								fromActiveControl.Focus();
							}
						}
					}
				}
				catch (Exception ex3)
				{
					WebErrorLog instance = WebErrorLog.Instance;
					Control obj3 = parent;
					string obj4 = ((obj3 != null) ? obj3.Name : null);
					Control obj5 = parent;
					instance.StartLog(ex3, "Waiter.cs=>_loadingForm_OnAutoClose\r\n#ParentName:" + obj4 + "\r\n_parentText:" + ((obj5 != null) ? obj5.Text : null), "SetFocusToActiveControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 238);
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetFocusToActiveControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\Waiter.cs", 245);
		}
	}
}
