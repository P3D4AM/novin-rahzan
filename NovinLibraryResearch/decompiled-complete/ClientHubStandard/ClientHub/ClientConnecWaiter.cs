using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace ClientHub;

public class ClientConnecWaiter
{
	private TaskCompletionSource<bool> TaskCompletion;

	private Client client;

	private int TimeOut { get; set; }

	private CancellationToken Token { get; set; }

	public ClientConnecWaiter(int timeOut, CancellationToken token, Client client)
	{
		TimeOut = timeOut;
		Token = token;
		this.client = client;
	}

	public async Task<bool> WaitForConnect()
	{
		using (new JaegerMonitor("WaitForConnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\ClientConnecWaiter.cs"))
		{
			_ = 5;
			try
			{
				if (await client.GetStateAsync() == EnConnectionState.Connected)
				{
					return true;
				}
				client.Connected += Client_Connected;
				client.Disconnected += Client_Disconnected;
				TaskCompletion = new TaskCompletionSource<bool>();
				Task task = Task.Delay(TimeOut, Token);
				Task result = await Task.WhenAny(new Task[2] { TaskCompletion.Task, task });
				client.Connected -= Client_Connected;
				client.Disconnected -= Client_Disconnected;
				if (await client.GetStateAsync() == EnConnectionState.Connected)
				{
					return true;
				}
				if (result == TaskCompletion.Task)
				{
					if (await client.GetStateAsync() == EnConnectionState.Connected)
					{
						return true;
					}
					return await TaskCompletion.Task;
				}
				if (await client.GetStateAsync() == EnConnectionState.Connected)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WaitForConnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\ClientConnecWaiter.cs", 48);
				return false;
			}
		}
	}

	private Task Client_Disconnected(object arg1, SingleConnectionEventArgs arg2)
	{
		TaskCompletion.TrySetResult(result: false);
		return Task.CompletedTask;
	}

	private Task Client_Connected(object sender, SingleConnectionEventArgs e)
	{
		TaskCompletion.TrySetResult(result: true);
		return Task.CompletedTask;
	}
}
