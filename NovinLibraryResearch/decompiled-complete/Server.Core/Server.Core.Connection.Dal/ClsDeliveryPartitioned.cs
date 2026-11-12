using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsDeliveryPartitioned : IEntity<WebDelivery>
{
	private class NestedClsDeliveryPartitioned
	{
		internal static readonly ClsDeliveryPartitioned DeliveriInstance;

		static NestedClsDeliveryPartitioned()
		{
			DeliveriInstance = new ClsDeliveryPartitioned();
		}
	}

	public static ClsDeliveryPartitioned DeliveriInstance => NestedClsDeliveryPartitioned.DeliveriInstance;

	private ClsDeliveryPartitioned()
	{
	}

	public async Task<List<WebDelivery>> ApplyAccessAsync(IEnumerable<WebDelivery> lst, IClsConnectionData connection)
	{
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			if (connection.Source != ENSource.Accounting)
			{
				lst?.Count();
				lst = lst?.Where((WebDelivery p) => p.ServerStatus != ServerStatusForClient.Sent && p.ServerStatus != ServerStatusForClient.None);
			}
			if (connection.ClientVersion <= 24)
			{
				lst = lst?.Where((WebDelivery q) => q.Part != EntityPart.PrdSerial && q.Part != EntityPart.SanadPrint);
			}
			if (connection.ClientVersion < 10)
			{
				lst = lst?.Where((WebDelivery p) => p.Part != EntityPart.CustomerGroup && p.Part != EntityPart.CustomerTitle && p.Part != EntityPart.ProductGroup && p.Part != EntityPart.ProductVahed && p.Part != EntityPart.Kol && p.Part != EntityPart.Moein && p.Part != EntityPart.Tafsil && p.Part != EntityPart.TafsilGroup && p.Part != EntityPart.CallingLog);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 44);
		}
		return lst?.ToList();
	}

	internal async Task Delivery_OnReadRequest(object sender, DeliveryReadEventArgs e)
	{
		using (new JaegerMonitor("Delivery_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Delivery, 1, isReceivedPacket: true);
				IEnumerable<WebDelivery> items = await Server.Core.Assistance.Initializer.Dal.DalDeliveryPartitioned.GetDeliveryByGuidSAsync(e.Connection.DepId, e.Requests.Select((DeliveryReadModel del) => del.Guid)?.ToList(), default(CancellationToken));
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebDelivery.GenerateReadResponse(items, Guid.NewGuid()), Priority.Medium);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Delivery_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 68);
			}
		}
	}

	internal async Task ClsDeliveryPartitioned_OnReadResponse(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		using (new JaegerMonitor("ClsDeliveryPartitioned_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Delivery, 2, isReceivedPacket: true);
				if ((e?.ConnectionData?.Count).GetValueOrDefault() <= 0)
				{
					return;
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e?.ConnectionUniqueId ?? "");
				int num;
				if (connection == null)
				{
					num = 1;
				}
				else
				{
					_ = connection.cancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return;
				}
				await Server.Core.Assistance.Initializer.Dal.DalDeliveryPartitioned.SaveDeliveryAsync(e, connection.cancellationToken, 10);
				List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
				if (connectionsData == null)
				{
					return;
				}
				foreach (ClsConnectionData item in connectionsData?.Where((ClsConnectionData p) => p != e.Connection).ToList())
				{
					item?.SendPacketAsync(WebDelivery.GenerateReadResponse(e.ConnectionData, Guid.NewGuid()), Priority.Medium);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ClsDeliveryPartitioned_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 88);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsyncFromEntityDelivery(ClsConnectionData connection, string ConnectionUniqueIdshort, short maxParalel = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 97);
			}
			DateTime Syncing = DateTime.Now;
			await connection.GetSyncInfoAsync();
			if ((connection?.IsDisposed ?? true) || (connection?.cancellationToken.IsCancellationRequested ?? true))
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 101);
			}
			if (connection.HashId <= 0)
			{
				throw new ArgumentException($"hashId out of range! hashId:{connection.HashId}");
			}
			List<int> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllPartitionsAsync(connection.DepId, (int)connection.HashId, connection.cancellationToken);
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (int partition in list)
			{
				List<WebDelivery> lst = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 109);
				}
				lst = await ApplyAccessAsync(lst, connection);
				if (!(lst?.Any() ?? false))
				{
					continue;
				}
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Delivery, 300000, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebDelivery.GenerateReadResponse(lst, Guid.NewGuid(), isSync: true), Priority.Low));
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 115);
				}
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					return ret.AddError("!await waiter.WaitForSyncDonePacket()", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 116);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Delivery, Syncing, connection), ReturnedState.None, "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 117);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 121);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 124);
			ret.AddReturnedValue(ex2, "", "Sync2ClientAsyncFromEntityDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDeliveryPartitioned.cs", 125);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}
}
