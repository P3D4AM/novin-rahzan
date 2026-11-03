using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace ClientHub;

public static class AsyncHelpers
{
	public static async Task<T> RetryOnFault<T>(Func<Task<T>> function, int maxTries)
	{
		checked
		{
			using (new JaegerMonitor("RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AsyncHelpers.cs"))
			{
				_ = 1;
				try
				{
					for (int i = 0; i < maxTries; i++)
					{
						try
						{
							return await function();
						}
						catch
						{
							if (i == maxTries - 1)
							{
								throw;
							}
							await Task.Delay(100);
						}
					}
					return default(T);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AsyncHelpers.cs", 30);
					return default(T);
				}
			}
		}
	}

	public static async Task<T> RetryOnFault<T>(Func<Task<T>> function, int maxTries, CancellationToken token)
	{
		checked
		{
			using (new JaegerMonitor("RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AsyncHelpers.cs"))
			{
				_ = 1;
				try
				{
					for (int i = 0; i < maxTries; i++)
					{
						if (token.IsCancellationRequested)
						{
							return default(T);
						}
						try
						{
							return await function();
						}
						catch
						{
							if (i == maxTries - 1)
							{
								throw;
							}
							await Task.Delay(100, token);
						}
					}
					return default(T);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RetryOnFault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AsyncHelpers.cs", 57);
					throw;
				}
			}
		}
	}
}
