using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class ContactUsWebServiceRepository : WebServiceRepository<ContactUsBusiness>
{
	public ContactUsWebServiceRepository(ClsCache cache)
		: base((INoGuidRepository<ContactUsBusiness>)cache.CacheRepository.IContact, cache)
	{
		try
		{
			Cache = cache;
			Cache.Context.WebContactUs.OnReadResponse += ContactUsInstance_OnReadResponse;
			Cache.Context.WebContactUs.OnSync += ContactUsInstance_OnSync;
			Cache.Context.WebContactUs.OnSaveResponse += ContactUsInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 27);
		}
	}

	public override void Dispose()
	{
		try
		{
			Cache.Context.WebContactUs.OnReadResponse -= ContactUsInstance_OnReadResponse;
			Cache.Context.WebContactUs.OnSync -= ContactUsInstance_OnSync;
			Cache.Context.WebContactUs.OnSaveResponse -= ContactUsInstance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 37);
		}
	}

	private async Task ContactUsInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		if (e.SaveResponses == null || e.SaveResponses.Count == 0)
		{
			return;
		}
		using (new JaegerMonitor("ContactUsInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs"))
		{
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
						(await ContactUsBusiness.GetAsync(Cache.ContextGuid, resopnse.Guid))?.SetSaveResultAsync(resopnse, e.Connection.cancellationToken, 5, "");
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ContactUsInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 55);
			}
		}
	}

	private async Task ContactUsInstance_OnSync(object sender, ConnectionEventArgs<WebContactUs> e)
	{
		using (new JaegerMonitor("ContactUsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs"))
		{
			_ = 2;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Contact us sync received but data is null!", "", "ContactUsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 83);
					return;
				}
				List<WebContactUs> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Contact us sync received but data is empty!", "", "ContactUsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 88);
					return;
				}
				List<ContactUsBusiness> lst = await ContactUsMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData);
				bool sendtoServer = Cache.Source == ENSource.Accounting;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await ContactUsBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.High, lst, sendtoServer, 10);
				returnedSaveFuncInfo.throwExceptionIfError();
				if (!e.Connection.cancellationToken.IsCancellationRequested && InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Packet packetData = Sync.GenerateSyncDone(EntityPart.ContactUs, e.PartitionId, returnedSaveFuncInfo);
					await Cache.SendData(packetData, Priority.High);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ContactUsInstance_OnSync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 106);
			}
		}
	}

	private async Task ContactUsInstance_OnReadResponse(object sender, ConnectionEventArgs<WebContactUs> e)
	{
		using (new JaegerMonitor("ContactUsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs"))
		{
			_ = 1;
			try
			{
				if (e.ConnectionData == null)
				{
					WebErrorLog.Instance.Warning("Contact us received but data is null!", "", "ContactUsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 116);
					return;
				}
				List<WebContactUs> connectionData = e.ConnectionData;
				if (connectionData != null && !connectionData.Any())
				{
					WebErrorLog.Instance.Warning("Contact us received but data is empty!", "", "ContactUsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 121);
					return;
				}
				List<ContactUsBusiness> lst = await ContactUsMapper.Instance.MapListAsync(e.Connection.Context.Guid, e.ConnectionData);
				bool sendtoServer = Cache.Source == ENSource.Accounting;
				await ContactUsBusiness.SaveRangeAsync(EnSystem.SignalR, e.Connection.cancellationToken, Priority.Medium, lst, sendtoServer, 10);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ContactUsInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 132);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> AddAndSend(WebContactUs item, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 140);
			}
			WebErrorLog.Instance.Debug("Sending contactUs... ", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 141);
			ContactUsBusiness contactUsBusiness = await ContactUsMapper.Instance.MapAsync(Cache.Context.Guid, item);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 143);
			}
			if (contactUsBusiness != null)
			{
				await contactUsBusiness.SaveAsync(EnSystem.SignalR, token, Priority.High, sendToServer: true, awaitOnSemaphore: true, 60000, 10);
			}
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 145);
			}
			List<Packet> lstPacket = WebContactUs.GenerateSaveRequest(new List<WebContactUs> { item });
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 147);
			}
			await Cache.SendData(lstPacket, Priority.High);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 149);
			}
			WebErrorLog.Instance.Information("ContactUs sent", "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 150);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 152);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 153);
			ret.AddReturnedValue(ex, "", "AddAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ContactUsWebServiceRepository.cs", 153);
		}
		return ret;
	}
}
