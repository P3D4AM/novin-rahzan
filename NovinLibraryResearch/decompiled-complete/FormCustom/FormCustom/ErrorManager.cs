using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Assistance;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities;

namespace FormCustom;

public static class ErrorManager
{
	private static CancellationTokenSource _displayErrorToken = new CancellationTokenSource();

	private static ILkUiFactory Owner { get; set; } = null;


	public static void Init(ENSource _source, ILkUiFactory owner)
	{
		try
		{
			Owner = owner;
			WebErrorLog.Instance.ErrorForUser += ErrorLogInstanceOnErrorForUser;
			clsShowError.Source = _source;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ErrorManager.cs", 23);
		}
	}

	private static Task ErrorLogInstanceOnErrorForUser(object sender, WebErrorLog e)
	{
		try
		{
			_displayErrorToken?.Cancel();
			_displayErrorToken = new CancellationTokenSource(60000);
			CancellationToken token = _displayErrorToken.Token;
			FrmShowErrorMessage.GetThreadInstance(e.ContextGuid, e, token, Owner, LanguageTypes.Farsi, "ErrorLogInstanceOnErrorForUser", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ErrorManager.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ErrorLogInstanceOnErrorForUser", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ErrorManager.cs", 35);
		}
		return Task.CompletedTask;
	}

	public static bool IsShowError(Form frm)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
			{
				if (((Control)item).Name == ((Control)frm).Name)
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsShowError", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ErrorManager.cs", 50);
		}
		return false;
	}
}
