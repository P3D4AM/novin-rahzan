using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace FormCustom.Activation;

public static class ActivationCacheBuffer
{
	public static Task InitAsync(ClsCache instance, List<ClientConfig> cFList, CancellationToken token)
	{
		try
		{
			instance.BufferDisconnected += _buffer_Disconnected;
			Task.Run(() => instance.ConnectAsync(EnConnectionType.SignalRCore, cFList, token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationCacheBuffer.cs", 21);
		}
		return Task.CompletedTask;
	}

	private static Task _buffer_Disconnected(object sender, BaseEventArgs e)
	{
		using (new JaegerMonitor("_buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationCacheBuffer.cs"))
		{
			try
			{
				ClsCache.GetCache(e.Connection.ContextGuid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "_buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationCacheBuffer.cs", 31);
			}
			return Task.CompletedTask;
		}
	}
}
