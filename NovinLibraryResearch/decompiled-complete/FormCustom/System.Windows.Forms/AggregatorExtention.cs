using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FormCustom;
using FormCustom.Cache;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace System.Windows.Forms;

public static class AggregatorExtention
{
	public static DialogResult ShowReturnedSavedFuncInfoForUserControl<T>(this T uc, ReturnedSaveFuncInfo returnedInfo, string title = "پیغام سیستم", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "") where T : UserControl, INpDisposable
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (uc != null)
			{
				if (uc.IsDisposed)
				{
					throw new ObjectDisposedException("UserControl(uc)/Name:" + ((Control)(object)uc).Name + "/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (!((Control)(object)uc).IsHandleCreated)
				{
					throw new Exception("handller for userControl(" + ((Control)(object)uc).Name + ") was not created!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (((Control)(object)uc).InvokeRequired)
				{
					throw new Exception("UserControl(" + ((Control)(object)uc).Name + ") invoke required!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (!((Control)(object)uc).Visible || !((Control)(object)uc).Enabled)
				{
					throw new Exception("UserControl(" + ((Control)(object)uc).Name + ") is invisible! or not enabled!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
			}
			return ((Form)new FrmShowErrorMessage(((CacheBaseForm)(object)((Control)(object)uc).FindForm()).ContextGuid, returnedInfo, title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowReturnedSavedFuncInfoForUserControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs")).ShowDialog((IWin32Window)(object)uc);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowReturnedSavedFuncInfoForUserControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 31);
			return (DialogResult)2;
		}
	}

	public static async Task<DialogResult> ShowReturnedSavedFuncInfoForUserControlAsync<T>(this T uc, ReturnedSaveFuncInfo returnedInfo, string title = "پیغام سیستم", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "") where T : UserControl, INpDisposable
	{
		try
		{
			if (uc == null)
			{
				throw new NullReferenceException("Uc is null!!");
			}
			CancellationTokenSource token = new CancellationTokenSource(60000);
			if (uc.IsDisposed)
			{
				throw new ObjectDisposedException("UserControl(uc)/Name:" + ((Control)(object)uc).Name + "/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
			}
			do
			{
				await Task.Delay(100);
				if (((Control)(object)uc).IsDisposed || token.IsCancellationRequested)
				{
					return (DialogResult)2;
				}
			}
			while (!((Control)(object)uc).IsHandleCreated);
			if (!((Control)(object)uc).Visible || !((Control)(object)uc).Enabled)
			{
				throw new Exception("UserControl(" + ((Control)(object)uc).Name + ") is invisible! or not enabled!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
			}
			TaskCompletionSource<FrmShowErrorMessage> frm = new TaskCompletionSource<FrmShowErrorMessage>();
			if (((Control)(object)uc).IsDisposed || token.IsCancellationRequested)
			{
				return (DialogResult)2;
			}
			((Control)(object)uc).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					FrmShowErrorMessage frmShowErrorMessage = new FrmShowErrorMessage(((CacheBaseForm)(object)((Control)(object)uc).FindForm()).ContextGuid, returnedInfo, title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowReturnedSavedFuncInfoForUserControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
					frm.TrySetResult(frmShowErrorMessage);
					((Form)frmShowErrorMessage).Show((IWin32Window)(object)uc);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowReturnedSavedFuncInfoForUserControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 59);
				}
			});
			if (((Control)(object)uc).IsDisposed || token.IsCancellationRequested)
			{
				return (DialogResult)2;
			}
			return await (await frm.Task).DialogResultTask.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowReturnedSavedFuncInfoForUserControlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 67);
			return (DialogResult)2;
		}
	}

	public static async Task<DialogResult> ShowReturnedSavedFuncInfoAsync<T>(this T frm, ReturnedSaveFuncInfo returnedInfo, string title = "پیغام سیستم", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "") where T : CacheBaseForm, INpDisposable
	{
		try
		{
			((Form)(object)frm).CheckVisibility("ShowReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			if (frm != null)
			{
				if (frm != null && ((INpDisposable)frm).IsDisposed)
				{
					throw new ObjectDisposedException("frm/Name:" + ((Control)frm).Name + "/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (frm != null && !((Control)frm).IsHandleCreated)
				{
					throw new Exception("handller for frm(" + ((Control)frm).Name + ") was not created!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (frm != null && ((Control)frm).InvokeRequired)
				{
					throw new Exception("frm(" + ((Control)frm).Name + ") invoke required!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if ((frm != null && !((Control)frm).Visible) || !((Control)frm).Enabled)
				{
					throw new Exception("frm(" + ((Control)frm).Name + ") is invisible! or not enabled!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
			}
			FrmShowErrorMessage frmShowErrorMessage = new FrmShowErrorMessage(frm.ContextGuid, returnedInfo, title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Form)frmShowErrorMessage).Show((IWin32Window)(object)frm);
			return await frmShowErrorMessage.DialogResultTask.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog instance = WebErrorLog.Instance;
			string[] obj = new string[8] { "frmName:", null, null, null, null, null, null, null };
			obj[1] = ((frm != null) ? ((Control)frm).Name : null);
			obj[2] = "\r\nfrm.Text:";
			obj[3] = ((frm != null) ? ((Control)frm).Text : null);
			obj[4] = "\r\nmemberName is ";
			obj[5] = memberName;
			obj[6] = "\r\nsourceFilePath =";
			obj[7] = sourceFilePath;
			instance.StartLog(ex, string.Concat(obj), "ShowReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 90);
			return (DialogResult)2;
		}
	}

	public static async Task<DialogResult> ShowInvokedReturnedSavedFuncInfoAsync<T>(this T frm, ReturnedSaveFuncInfo returnedInfo, string title = "پیغام سیستم", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "") where T : CacheBaseForm, INpDisposable
	{
		try
		{
			if (frm == null)
			{
				throw new NullReferenceException("Frm is null!!");
			}
			CancellationToken token = new CancellationTokenSource(60000).Token;
			do
			{
				await Task.Delay(100);
				if (frm.IsDisposed || token.IsCancellationRequested)
				{
					return (DialogResult)2;
				}
			}
			while (!((Control)frm).IsHandleCreated);
			if (((INpDisposable)frm).IsDisposed)
			{
				throw new ObjectDisposedException("frm/Name:" + ((Control)frm).Name + "/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
			}
			if (!((Control)frm).Visible || !((Control)frm).Enabled)
			{
				throw new Exception("frm(" + ((Control)frm).Name + ") is invisible! or not enabled!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
			}
			TaskCompletionSource<FrmShowErrorMessage> frmTask = new TaskCompletionSource<FrmShowErrorMessage>();
			((Control)frm).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					FrmShowErrorMessage frmShowErrorMessage = new FrmShowErrorMessage(((CacheBaseForm)(object)((Control)frm).FindForm()).ContextGuid, returnedInfo, title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowInvokedReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
					frmTask.TrySetResult(frmShowErrorMessage);
					((Form)frmShowErrorMessage).Show((IWin32Window)(object)frm);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowInvokedReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 122);
				}
			});
			if (frm.IsDisposed)
			{
				return (DialogResult)2;
			}
			return await (await frmTask.Task).DialogResultTask.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog instance = WebErrorLog.Instance;
			string[] obj = new string[8] { "frmName:", null, null, null, null, null, null, null };
			T val = frm;
			obj[1] = ((val != null) ? ((Control)val).Name : null);
			obj[2] = "\r\nfrm.Text:";
			T val2 = frm;
			obj[3] = ((val2 != null) ? ((Control)val2).Text : null);
			obj[4] = "\r\nmemberName is ";
			obj[5] = memberName;
			obj[6] = "\r\nsourceFilePath =";
			obj[7] = sourceFilePath;
			instance.StartLog(ex, string.Concat(obj), "ShowInvokedReturnedSavedFuncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 130);
			return (DialogResult)2;
		}
	}

	public static DialogResult ShowMessage(this UserControl userControl, string message, string title = "پیغام سیستم")
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return ((CacheBaseForm)(object)((Control)userControl).FindForm()).ShowMessage(message, title, "ShowMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
	}

	public static DialogResult ShowMessage(this CacheBaseForm frm, string message, string title = "پیغام سیستم", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (frm != null)
			{
				if (((INpDisposable)frm).IsDisposed)
				{
					throw new ObjectDisposedException("frm/Name:" + ((Control)frm).Name + "/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (!((Control)frm).IsHandleCreated)
				{
					throw new Exception("handller for frm(" + ((Control)frm).Name + ") was not created!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (((Control)frm).InvokeRequired)
				{
					throw new Exception("frm(" + ((Control)frm).Name + ") invoke required!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
				if (!((Control)frm).Visible || !((Control)frm).Enabled)
				{
					throw new Exception("frm(" + ((Control)frm).Name + ") is invisible! or not enabled!/MemberName:" + memberName + "/sourceFilePath:" + sourceFilePath);
				}
			}
			((Form)(object)frm).CheckVisibility("ShowMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			return ((Form)new FrmShowErrorMessage(frm.ContextGuid, message, title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs")).ShowDialog((IWin32Window)(object)frm);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 155);
			return (DialogResult)2;
		}
	}

	public static async Task<DialogResult> ShowErrorAsync<T>(this T frm, string errorMessage, string title = "خطا") where T : CacheBaseForm, INpDisposable
	{
		DialogResult result = (DialogResult)0;
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			while (!((Control)frm).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((INpDisposable)frm).IsDisposed || token.IsCancellationRequested)
				{
					return result;
				}
			}
			((Form)(object)frm).CheckVisibility("ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Control)frm).Invoke((Delegate)(Action)delegate
			{
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				result = ((Form)new FrmShowErrorMessage(frm.ContextGuid, new ReturnedSaveFuncInfo().AddError(EnSystem.UserInterface, errorMessage, "", "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 174), title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs")).ShowDialog((IWin32Window)(object)frm);
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 178);
		}
		return result;
	}

	public static DialogResult ShowError(this CacheBaseForm frm, string errorMessage, string title = "خطا")
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		DialogResult ret = (DialogResult)0;
		try
		{
			if (frm == null || frm.IsDisposed)
			{
				return ret;
			}
			((Form)(object)frm).CheckVisibility("ShowError", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Control)frm).Invoke((Delegate)(Action)delegate
			{
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				if (frm != null && !frm.IsDisposed)
				{
					ret = ((Form)new FrmShowErrorMessage(frm.ContextGuid, new ReturnedSaveFuncInfo().AddError(EnSystem.UserInterface, errorMessage, "", "ShowError", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 191), title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowError", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs")).ShowDialog((IWin32Window)(object)frm);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowError", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 195);
		}
		return ret;
	}

	public static DialogResult ShowError(this UserControl userControl, string warningMessage, string title = "خطا")
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return ((CacheBaseForm)(object)((Control)userControl).FindForm()).ShowError(warningMessage, title);
	}

	public static async Task ShowWarningAsync<T>(this T frm, string warningMessage, string title = "هشدار") where T : CacheBaseForm, INpDisposable
	{
		try
		{
			((Form)(object)frm).CheckVisibility("ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			CancellationToken token = new CancellationTokenSource(60000).Token;
			await Task.Delay(100);
			while (!((Control)frm).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((INpDisposable)frm).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			FrmShowErrorMessage resultForm = null;
			if (((INpDisposable)frm).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			IAsyncResult asyncResult = ((Control)frm).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!((INpDisposable)frm).IsDisposed && !token.IsCancellationRequested)
					{
						resultForm = new FrmShowErrorMessage(frm.ContextGuid, new ReturnedSaveFuncInfo().AddWarning(warningMessage, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 219), title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
						((Form)resultForm).Show((IWin32Window)(object)frm);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 222);
				}
			});
			((Control)frm).EndInvoke(asyncResult);
			await resultForm.DialogResultTask.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 227);
		}
	}

	public static async Task<DialogResult> ShowWarningAsync(this CacheBaseForm frm, string warningMessage, string title = "هشدار")
	{
		DialogResult ret = (DialogResult)0;
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			((Form)(object)frm).CheckVisibility("ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Control)frm).Name = ((Control)frm).Name + "_" + Guid.NewGuid().ToString().Replace("-", "_");
			if (frm == null || frm.IsDisposed)
			{
				return ret;
			}
			while (!((Control)frm).IsHandleCreated)
			{
				await Task.Delay(100);
				if (frm.IsDisposed || token.IsCancellationRequested)
				{
					return (DialogResult)2;
				}
			}
			FrmShowErrorMessage frmWarning = new FrmShowErrorMessage(frm.ContextGuid, new ReturnedSaveFuncInfo().AddWarning(warningMessage, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 246), title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Control)frm).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (frm != null && !frm.IsDisposed && !token.IsCancellationRequested)
					{
						((Form)frmWarning).Show((IWin32Window)(object)frm);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 254);
				}
			});
			ret = await frmWarning.DialogResultTask.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 258);
		}
		return ret;
	}

	public static Task<DialogResult> ShowWarningAsync(this UserControl userControl, string warningMessage, string title = "هشدار")
	{
		return ((CacheBaseForm)(object)((Control)userControl).FindForm()).ShowWarningAsync(warningMessage, title);
	}

	public static DialogResult ShowWarning(this CacheBaseForm frm, string errorMessage, string title = "هشدار")
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		DialogResult ret = (DialogResult)0;
		try
		{
			if (frm == null || frm.IsDisposed)
			{
				return ret;
			}
			((Form)(object)frm).CheckVisibility("ShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs");
			((Control)frm).Invoke((Delegate)(Action)delegate
			{
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				if (frm != null && !frm.IsDisposed)
				{
					ret = ((Form)new FrmShowErrorMessage(frm.ContextGuid, new ReturnedSaveFuncInfo().AddWarning(errorMessage, "", "ShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 273), title, (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "ShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs")).ShowDialog((IWin32Window)(object)frm);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowWarning", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\AggregatorExtention.cs", 276);
		}
		return ret;
	}

	public static string HexConverter(this Color c)
	{
		return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
	}

	public static Color RGBConverter(this string hex)
	{
		return ColorTranslator.FromHtml(hex);
	}
}
