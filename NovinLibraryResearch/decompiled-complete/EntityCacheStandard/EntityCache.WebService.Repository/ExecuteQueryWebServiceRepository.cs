using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Tickets;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.WebService.Repository;

public class ExecuteQueryWebServiceRepository : INpDisposable, IDisposable
{
	private ClsCache Cache;

	public bool IsDisposed { get; private set; }

	public ExecuteQueryWebServiceRepository(ClsCache cache)
	{
		Cache = cache;
		cache.Context.WebExecuteQuery.OnExecuteQueryRequest += WebExecuteQuery_OnExecuteQueryRequest;
	}

	private async Task WebExecuteQuery_OnExecuteQueryRequest(object sender, ConnectionEventArgs<ExecuteQueryRequestEventArgs> e)
	{
		_ = 6;
		try
		{
			CancellationToken cancellationToken = e.Connection.cancellationToken;
			if (cancellationToken.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			_ = ClsCache.GetCache(Cache.ContextGuid).Source;
			string jsonRequest = e.ConnectionData.Single().JsonRequest;
			ExecuteQueryResponseEventArgs executeQueryResponseEventArgs;
			switch (e.ConnectionData.Single().Type)
			{
			case PacketType.GardeshTafsili:
			{
				GardeshTafsiliFilter filter = jsonRequest.FromJson<GardeshTafsiliFilter>("WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ExecuteQueryWebServiceRepository.cs", 44);
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> gardeshTafsili = await GardeshTafsilBusiness.GetAllAsync(Cache.ContextGuid, filter, EnSystem.SignalR, "", cancellationToken);
				List<WebGardeshTafsiliCompact> item = await GardeshTafsilMapper.Instance.MapListAsync(Cache.ContextGuid, gardeshTafsili.value?.ToList(), new List<WebGardeshTafsiliCompact>());
				gardeshTafsili.throwExceptionIfError();
				executeQueryResponseEventArgs = new ExecuteQueryResponseEventArgs
				{
					JsonResult = Json.ToStringJson(item),
					PacketReferenceId = e.PacketReferenceId
				};
				break;
			}
			case PacketType.TicketDet:
			{
				WebTicketDet ticketDet = jsonRequest.FromJson<WebTicketDet>("WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ExecuteQueryWebServiceRepository.cs", 56);
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				(await (await TicketDetMapper.Instance.MapAsync(Cache.ContextGuid, ticketDet, new TicketDetBusiness(Cache.ContextGuid), "", e.Connection.cancellationToken)).SaveAsync(EnSystem.SignalR)).throwExceptionIfError();
				executeQueryResponseEventArgs = new ExecuteQueryResponseEventArgs
				{
					JsonResult = Json.ToStringJson(ticketDet),
					PacketReferenceId = e.PacketReferenceId
				};
				break;
			}
			case PacketType.Ticket:
			{
				WebTicket ticket = jsonRequest.FromJson<WebTicket>("WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ExecuteQueryWebServiceRepository.cs", 68);
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				(await (await TicketMapper.Instance.MapAsync(Cache.ContextGuid, ticket, new TicketBusiness(Cache.ContextGuid), "", e.Connection.cancellationToken)).SaveAsync(EnSystem.SignalR)).throwExceptionIfError();
				executeQueryResponseEventArgs = new ExecuteQueryResponseEventArgs
				{
					JsonResult = Json.ToStringJson(ticket),
					PacketReferenceId = e.PacketReferenceId
				};
				break;
			}
			default:
				throw new NotImplementedException();
			}
			if (executeQueryResponseEventArgs != null)
			{
				await Cache.SendData(WebExecuteQuery.GenerateQueryResponse(executeQueryResponseEventArgs), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebExecuteQuery_OnExecuteQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ExecuteQueryWebServiceRepository.cs", 85);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Cache.Context.WebExecuteQuery.OnExecuteQueryRequest -= WebExecuteQuery_OnExecuteQueryRequest;
				Cache = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\ExecuteQueryWebServiceRepository.cs", 96);
		}
	}
}
