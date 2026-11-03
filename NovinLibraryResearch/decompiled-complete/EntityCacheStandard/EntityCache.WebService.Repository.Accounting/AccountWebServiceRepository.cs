using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Core;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository.Accounting;

public class AccountWebServiceRepository : WebServiceRepository<AccountBusiness>, IDisposable
{
	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebAccount.OnReadResponse -= AccountInstance_OnReadResponse;
				Cache.Context.WebAccount.OnSync -= AccountInstance_OnSync;
				Cache.Context.WebAccount.OnSaveRequest -= AccountInstance_OnSaveRequest;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs", 25);
		}
	}

	public AccountWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<AccountBusiness>)cache.CacheRepository.Account, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebAccount.OnReadResponse += AccountInstance_OnReadResponse;
			Cache.Context.WebAccount.OnSync += AccountInstance_OnSync;
			Cache.Context.WebAccount.OnSaveRequest += AccountInstance_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs", 36);
		}
	}

	private async Task AccountInstance_OnSync(object sender, ConnectionEventArgs<WebAccount> e)
	{
		using (new JaegerMonitor("AccountInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs"))
		{
			CancellationToken token = e?.Connection?.cancellationToken ?? new CancellationToken(canceled: true);
			try
			{
				if (e.ConnectionData != null && Cache.Source != ENSource.Accounting && (e.ConnectionData?.Any() ?? false))
				{
					e.ConnectionData.ToList();
					_ = Cache.Source;
					ReturnedSaveFuncInfo res1 = new ReturnedSaveFuncInfo();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res1;
					returnedSaveFuncInfo.AddReturnedValue(await AccountBusiness.SaveRangeAsync(base.ContextGuid, e.ConnectionData, EnSystem.SignalR, e.Connection.cancellationToken), ReturnedState.None, "AccountInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs", 49);
					res1.throwExceptionIfError();
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
					{
						Packet packetData = Sync.GenerateSyncDone(EntityPart.Account, e.PartitionId, res1);
						await Cache.SendData(packetData, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				if (!token.IsCancellationRequested)
				{
					WebErrorLog.Instance.StartLog(ex, "", "AccountInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs", 60);
				}
			}
		}
	}

	private Task AccountInstance_OnReadResponse(object sender, ConnectionEventArgs<WebAccount> e)
	{
		using (new JaegerMonitor("AccountInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs"))
		{
			try
			{
				if (e.ConnectionData == null || Cache.Source == ENSource.Accounting)
				{
					return Task.CompletedTask;
				}
				List<WebAccount> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					return Task.CompletedTask;
				}
				e.ConnectionData.ToList();
				_ = Cache.Source;
				AccountBusiness.SaveRangeAsync(base.ContextGuid, e.ConnectionData, EnSystem.SignalR, e.Connection.cancellationToken);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AccountInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\Accounting\\AccountWebServiceRepository.cs", 79);
			}
			return Task.CompletedTask;
		}
	}

	private Task AccountInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebAccount> e)
	{
		throw new NotImplementedException();
	}
}
