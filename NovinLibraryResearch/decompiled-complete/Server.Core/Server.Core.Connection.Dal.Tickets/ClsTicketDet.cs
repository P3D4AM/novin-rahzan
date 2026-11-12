using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal.Tickets;

public class ClsTicketDet : IEntity<WebTicketDet>
{
	private class NestedTicketDet
	{
		internal static readonly ClsTicketDet TicketDetInstance;

		static NestedTicketDet()
		{
			TicketDetInstance = new ClsTicketDet();
		}
	}

	public static ClsTicketDet TicketDetInstance => NestedTicketDet.TicketDetInstance;

	private ClsTicketDet()
	{
	}

	public Task<List<WebTicketDet>> ApplyAccessAsync(IEnumerable<WebTicketDet> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return Task.FromResult<List<WebTicketDet>>(null);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 28);
				return Task.FromResult<List<WebTicketDet>>(null);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (DateTime.Now.Year > 2025 && DateTime.Now.Month > 10)
				{
					WebErrorLog.Instance.StartLog("یادت باشه پیاده کنی.", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 40);
				}
				return returnedSaveFuncInfo;
			}
			catch (OperationCanceledException exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 84);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 87);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 88);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return returnedSaveFuncInfo;
		}
	}

	internal async Task TicketDetInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTicketDet> e)
	{
	}

	internal async Task TicketDetInstance_OnDeliverdRequest(object arg1, ConnectionEventArgs<Guid> e)
	{
	}

	private async void SendToOthers(List<WebTicketDet> ticketDets, short depId, long hashId, StringBuilder sb = null)
	{
	}

	internal async Task TicketDetInstance_OnSeenRequest(object sender, ConnectionEventArgs<WebTicketDet> e)
	{
	}

	public async Task SendNotDeliveredAsync(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs"))
		{
			_ = 1;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.TicketDet, connection.cancellationToken);
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
					await Server.Core.Assistance.Initializer.Dal.TicketDet.LoadDataAsync(item);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketDet.cs", 373);
			}
		}
	}
}
