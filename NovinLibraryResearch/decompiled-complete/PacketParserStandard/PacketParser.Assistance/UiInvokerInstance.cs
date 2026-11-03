using System;
using System.Runtime.CompilerServices;
using PacketParser.Entities;

namespace PacketParser.Assistance;

internal class UiInvokerInstance : IUiInvoker
{
	public void RunOnUiThread(Action action, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		using (new JaegerMonitor("RunOnUiThread", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\UIInvoker.cs"))
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath, "RunOnUiThread", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\UIInvoker.cs", 30);
			}
		}
	}

	public bool InvokeRequired()
	{
		return false;
	}
}
