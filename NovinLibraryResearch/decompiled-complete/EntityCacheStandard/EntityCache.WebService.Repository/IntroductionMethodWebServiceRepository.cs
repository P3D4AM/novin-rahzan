using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Mapping.Global;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class IntroductionMethodWebServiceRepository : WebServiceRepository<IntroductionMethodBusiness>, IDisposable
{
	public new string ContextGuid => Cache.ContextGuid;

	public IntroductionMethodWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<IntroductionMethodBusiness>)cache.CacheRepository.IntroductionMethod, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebIntroductionMethod.OnReadResponse += DescInstance_OnReadResponse;
			Cache.Context.WebIntroductionMethod.OnSync += DescInstance_OnSync;
			cache.Context.Sync.OnSyncReset += SyncInstance_OnSyncReset;
			Cache.Context.WebIntroductionMethod.OnSaveResponse += DescInstance_OnSaveResponse;
			Cache.Context.WebIntroductionMethod.OnSaveRequest += WebIntroductionMethod_OnSaveRequest;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 28);
		}
	}

	private async Task WebIntroductionMethod_OnSaveRequest(object sender, ConnectionEventArgs<WebIntroductionMethod> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("WebIntroductionMethod_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					return;
				}
				List<IntroductionMethodBusiness> list = await IntroductionMethodMapper.Instance.MapListAsync(ContextGuid, e.ConnectionData, new List<IntroductionMethodBusiness>());
				List<SaveResponse> responses = new List<SaveResponse>();
				foreach (IntroductionMethodBusiness item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await item.SaveAsync(EnSystem.SignalR, sendToServer: false, e.Connection.cancellationToken);
					responses.Add(new SaveResponse
					{
						Date = DateTime.Now,
						Guid = item.Guid,
						HostRefrenced = item.HostRefrenced,
						Message = returnedSaveFuncInfo.Messages,
						PartitionId = e.PartitionId,
						RequestPart = e.Part,
						RequestRefrencedId = e.PacketReferenceId,
						Shortmessage = returnedSaveFuncInfo.Messages,
						SaveResultStatus = !returnedSaveFuncInfo.HasError
					});
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "WebIntroductionMethod_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 58);
				}
				await Cache.SendData(WebIntroductionMethod.GenerateSaveResponse(responses), Priority.High);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebIntroductionMethod_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 62);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				Cache.Context.WebIntroductionMethod.OnReadResponse -= DescInstance_OnReadResponse;
				Cache.Context.WebIntroductionMethod.OnSync -= DescInstance_OnSync;
				Cache.Context.Sync.OnSyncReset -= SyncInstance_OnSyncReset;
				Cache.Context.WebIntroductionMethod.OnSaveResponse -= DescInstance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 77);
		}
	}

	private async Task SyncInstance_OnSyncReset(object arg1, SyncResetEventArgs e)
	{
		using (new JaegerMonitor("SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.EntityPart == EntityPart.IntroductionMethod)
				{
					Task<IEnumerable<IntroductionMethodBusiness>> allAsync = base.CacheRepository.GetAllAsync(default(CancellationToken), "");
					HashSet<Guid> keepIds = new HashSet<Guid>(e.SyncResetItems.Select((SyncResetItem x) => x.Guid));
					IEnumerable<IntroductionMethodBusiness> lst = (await allAsync).Where((IntroductionMethodBusiness x) => keepIds.Contains(x.Guid)).ToList();
					_ = Cache.Source;
					(await IntroductionMethodBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst)).throwExceptionIfError();
					if (!e.Connection.cancellationToken.IsCancellationRequested)
					{
						await Cache.SendData(Sync.GenerateSyncResetDone(e.EntityPart), Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 98);
			}
		}
	}

	private async Task DescInstance_OnSync(object sender, ConnectionEventArgs<WebIntroductionMethod> e)
	{
		using (new JaegerMonitor("DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("IntroductionMethod sync received but data is null!", "", "DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 108);
					return;
				}
				List<WebIntroductionMethod> source = e.ConnectionData.ToList();
				List<IntroductionMethodBusiness> lst = await IntroductionMethodMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await IntroductionMethodBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst);
				if (!e.Connection.cancellationToken.IsCancellationRequested && !returnedSaveFuncInfo.HasError && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packet = Sync.GenerateSyncDone(EntityPart.IntroductionMethod, e.PartitionId, returnedSaveFuncInfo);
					if (packet == null)
					{
						throw new NullReferenceException("pack");
					}
					if (!base.IsDisposed && !(Cache?.IsDisposed ?? true) && Cache.BufferEnable)
					{
						await Cache.SendData(packet, Priority.High);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 123);
			}
		}
	}

	private async Task DescInstance_OnReadResponse(object sender, ConnectionEventArgs<WebIntroductionMethod> e)
	{
		using (new JaegerMonitor("DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					WebErrorLog.Instance.Warning("IntroductionMethod sync received but data is null!", "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 133);
					return;
				}
				List<WebIntroductionMethod> source = e.ConnectionData.ToList();
				_ = Cache.Source;
				List<IntroductionMethodBusiness> lst = await IntroductionMethodMapper.Instance.MapListAsync(e.Connection.Context.Guid, source);
				await IntroductionMethodBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, lst);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DescInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 141);
			}
		}
	}

	private async Task DescInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e == null || !(e.SaveResponses?.Count > 0))
				{
					return;
				}
				foreach (SaveResponse resopnse in e.SaveResponses)
				{
					if (resopnse != null)
					{
						IntroductionMethodBusiness introductionMethodBusiness = await IntroductionMethodBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid);
						if (introductionMethodBusiness != null)
						{
							await introductionMethodBusiness.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 10);
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DescInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\IntroductionMethodWebServiceRepository.cs", 161);
			}
		}
	}
}
