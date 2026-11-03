using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class LoggedInWaiter : IDisposable
{
	private TaskCompletionSource<LoginResponseEventArgs> TaskCompletion;

	private WebContextHolder _context;

	private int TimeOut { get; set; }

	private CancellationToken Token { get; set; }

	public LoggedInWaiter(int timeOut, CancellationToken token)
	{
		TimeOut = timeOut;
		Token = token;
	}

	public async Task<LoginResponseEventArgs> WaitForResponse(WebContextHolder context)
	{
		using (new JaegerMonitor("WaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LoggedInWaiter.cs"))
		{
			_ = 1;
			try
			{
				_context = context;
				context.LoginExtention.OnLoginResponse += Instance_OnLoginResponse;
				TaskCompletion = new TaskCompletionSource<LoginResponseEventArgs>();
				Task obj = await Task.WhenAny(new Task[2]
				{
					TaskCompletion.Task,
					Task.Delay(TimeOut, default(CancellationToken))
				});
				context.LoginExtention.OnLoginResponse -= Instance_OnLoginResponse;
				if (obj == TaskCompletion.Task)
				{
					return await TaskCompletion.Task;
				}
				return new LoginResponseEventArgs
				{
					Status = EnLoginResult.TimeOut
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LoggedInWaiter.cs", 41);
				return new LoginResponseEventArgs
				{
					Status = EnLoginResult.Unknown
				};
			}
		}
	}

	private async Task Instance_OnLoginResponse(object sender, LoginResponseEventArgs e)
	{
		TaskCompletion?.TrySetResult(e);
	}

	public void Dispose()
	{
		try
		{
			_context.LoginExtention.OnLoginResponse -= Instance_OnLoginResponse;
			_context = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LoggedInWaiter.cs", 60);
		}
	}
}
