using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Assistance;
using PacketParser.Entities;

namespace FormCustom.Assistance;

public class UiInvokerInstance : IUiInvoker
{
	private CacheBaseForm _owner;

	private UiInvokerInstance(CacheBaseForm owner)
	{
		_owner = owner;
	}

	public static void Initialize(CacheBaseForm owner)
	{
		UiInvokerHolder.Instance = new UiInvokerInstance(owner);
	}

	public bool InvokeRequired()
	{
		try
		{
			int count = ((ReadOnlyCollectionBase)(object)Application.OpenForms).Count;
			return ((Control)Application.OpenForms[count - 1]).InvokeRequired;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InvokeRequired", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\UiInvokerInstance.cs", 29);
			return true;
		}
	}

	public void RunOnUiThread(Action action, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		try
		{
			if (_owner == null)
			{
				throw new NullReferenceException("_owner is null!!");
			}
			if (_owner.IsDisposed)
			{
				throw new ObjectDisposedException("owner");
			}
			IAsyncResult asyncResult = ((Control)_owner).BeginInvoke((Delegate)action);
			if (!_owner.IsDisposed)
			{
				((Control)_owner).EndInvoke(asyncResult);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath, "RunOnUiThread", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Assistance\\UiInvokerInstance.cs", 47);
		}
	}
}
