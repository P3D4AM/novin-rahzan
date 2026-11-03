using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Business;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;

namespace EntityCache;

internal static class CustomerBuffer
{
	public static async Task<ReturnedSaveFuncInfo> Send(IEnumerable<Packet> packets, string customerUsername, WebContextHolder context, string customerPassword, EnConnectionType connectionType, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs"))
		{
			_ = 1;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 23);
				}
				ReturnedSaveFuncInfoWithValue<ClientHub.Buffer> cl = await InitializeClient(context, customerUsername, customerPassword, connectionType, token);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 25);
				}
				foreach (Packet packet in packets)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 28);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cl.value.SendAndWaitForResponse(new List<Packet> { packet }, token, 20, 2, 10, 10), ReturnedState.None, "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 29);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 30);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 33);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 36);
				WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 37);
			}
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<ClientHub.Buffer>> InitializeClient(WebContextHolder context, string customerUsername, string customerPassword, EnConnectionType connectionType, CancellationToken token)
	{
		using (new JaegerMonitor("InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs"))
		{
			ReturnedSaveFuncInfoWithValue<ClientHub.Buffer> ret = new ReturnedSaveFuncInfoWithValue<ClientHub.Buffer>();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 47);
				}
				IEnumerable<ClientConfig> bufferConfigs = ClsCache.GetCache(context.Guid).BufferConfigs;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 49);
				}
				List<ClientConfig> list = new List<ClientConfig>();
				if (bufferConfigs != null)
				{
					foreach (ClientConfig item in bufferConfigs)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 54);
						}
						ClientConfig clientConfig = item.Copy();
						clientConfig.CustomerPassword = customerPassword;
						clientConfig.CustomerUsername = customerUsername;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 58);
						}
						clientConfig.SyncRequested = false;
						list.Add(clientConfig);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 61);
						}
					}
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 63);
				}
				ret.value = new ClientHub.Buffer(ClsCache.GetCache(context.Guid).Source, DI.Kernel, context, connectionType);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 65);
				}
				ClientHub.Buffer value = ret.value;
				IEnumerable<ClientConfig> configs = list;
				CancellationToken? token2 = token;
				await value.ConnectAsync(configs, token2, await LocalSettingBusiness.GetDbGuidAsync(context.Guid));
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 67);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 69);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 72);
				ret.AddReturnedValue(ex, "", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\CustomerBuffer.cs", 73);
			}
			return ret;
		}
	}
}
