using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal.Tickets;

public class ClsTicket : IEntity<WebTicket>
{
	private class NestedTicket
	{
		internal static readonly ClsTicket TicketInstance;

		static NestedTicket()
		{
			TicketInstance = new ClsTicket();
		}
	}

	public static ClsTicket TicketInstance => NestedTicket.TicketInstance;

	private ClsTicket()
	{
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs"))
		{
			try
			{
				if (DateTime.Now.Year > 2025 && DateTime.Now.Month > 10)
				{
					WebErrorLog.Instance.StartLog("یادت باشه پیاده کنی.", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs", 31);
				}
				return returnedSaveFuncInfo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs", 79);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs", 80);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return returnedSaveFuncInfo;
		}
	}

	public Task<List<WebTicket>> ApplyAccessAsync(IEnumerable<WebTicket> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return Task.FromResult<List<WebTicket>>(null);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs", 98);
				return Task.FromResult<List<WebTicket>>(null);
			}
		}
	}

	internal async Task TicketInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTicket> e)
	{
		using (new JaegerMonitor("TicketInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs"))
		{
		}
	}

	public async Task SendNotDeliveredAsync(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs"))
		{
			_ = 1;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Ticket, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested || list == null || (list != null && list.Count <= 0))
				{
					return;
				}
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						break;
					}
					await Server.Core.Assistance.Initializer.Dal.Ticket.LoadDataAsync(item);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicket.cs", 267);
			}
		}
	}
}
