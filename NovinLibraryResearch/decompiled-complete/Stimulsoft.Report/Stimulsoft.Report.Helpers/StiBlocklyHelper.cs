using System;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Blockly;
using Stimulsoft.Base.Blocks;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Helpers;

internal static class StiBlocklyHelper
{
	public static void InvokeBlockly(StiReport report, object sender, StiEvent stiEvent, EventArgs e = null)
	{
		if (string.IsNullOrEmpty(stiEvent.Script) || !stiEvent.Script.StartsWith("<xml xmlns=\"https://developers.google.com/blockly/xml\">"))
		{
			return;
		}
		Action action = delegate
		{
			IStiBlocksParser blockParse = StiBlocksCreator.GetBlockParse();
			if (blockParse != null)
			{
				blockParse.Evaluate((IStiReport)(object)report, sender, stiEvent.Script, e);
			}
		};
		CallWithTimeout(action, StiOptions.Engine.Blockly.ExecutionTimeout * 1000);
	}

	private static void CallWithTimeout(Action action, int timeoutMilliseconds)
	{
		Thread threadToKill = null;
		Action action2 = delegate
		{
			threadToKill = Thread.CurrentThread;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				if (ex is TimeoutException)
				{
					Thread.ResetAbort();
				}
			}
		};
		IAsyncResult asyncResult = action2.BeginInvoke(null, null);
		if (asyncResult.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
		{
			action2.EndInvoke(asyncResult);
			return;
		}
		threadToKill.Abort();
		throw new TimeoutException();
	}
}
