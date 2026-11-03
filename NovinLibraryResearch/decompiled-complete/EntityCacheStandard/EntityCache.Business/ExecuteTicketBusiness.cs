using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Tickets;
using EntityCache.Mapping.BusinessMappers.Crm.Ticket;
using PacketParser;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace EntityCache.Business;

public class ExecuteTicketBusiness : ExecuteQueryBusiness
{
	public ExecuteTicketBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	internal static async Task SendTicketDetailToClientsAsync(string contextGuid, TicketDetBusiness ticketDetial, CancellationToken token, int timeOutMiliSeconds = 30000)
	{
		_ = 1;
		try
		{
			if ((ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.NotExists) == EnLoginResult.Success)
			{
				WebTicketDet item = await TicketDetMapper.Instance.MapAsync(contextGuid, ticketDetial, new WebTicketDet(), "", token);
				ExecuteQueryRequestEventArgs queryRequest = new ExecuteQueryRequestEventArgs
				{
					JsonRequest = Json.ToStringJson(item),
					PacketReferenceId = Guid.NewGuid(),
					Type = PacketType.TicketDet
				};
				await ExecuteQueryBusiness.GetQueryResponseAsync(contextGuid, queryRequest, token, timeOutMiliSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendTicketDetailToClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteTicketBusiness.cs", 32);
		}
	}

	internal static async Task SendTicketToClientsAsync(string contextGuid, TicketBusiness ticket, CancellationToken token, int timeOutMiliSeconds = 30000)
	{
		_ = 1;
		try
		{
			if ((ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.NotExists) == EnLoginResult.Success)
			{
				WebTicket item = await TicketMapper.Instance.MapAsync(contextGuid, ticket, new WebTicket(), "", token);
				ExecuteQueryRequestEventArgs queryRequest = new ExecuteQueryRequestEventArgs
				{
					JsonRequest = Json.ToStringJson(item),
					PacketReferenceId = Guid.NewGuid(),
					Type = PacketType.Ticket
				};
				await ExecuteQueryBusiness.GetQueryResponseAsync(contextGuid, queryRequest, token, timeOutMiliSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendTicketToClientsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteTicketBusiness.cs", 49);
		}
	}
}
