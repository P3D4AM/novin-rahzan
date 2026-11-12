using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsGps : IEntity<WebGPS>
{
	private class NestedGps
	{
		internal static readonly ClsGps GpsInstance;

		static NestedGps()
		{
			GpsInstance = new ClsGps();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsGps GpsInstance => NestedGps.GpsInstance;

	private ClsGps()
	{
	}

	public static void UpdateDictionary(short DepId)
	{
		dic_isModifiedDepartmentData.TryRemove(DepId, out var _);
		dic_isModifiedDepartmentData.TryAdd(DepId, DateTime.Now);
	}

	public Task<List<WebGPS>> ApplyAccessAsync(IEnumerable<WebGPS> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebGPS p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return Task.FromResult(lst?.ToList());
	}

	public void SendLog(string msg)
	{
	}

	internal async Task GpsInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebGPS> e)
	{
		int line = 0;
		using (new JaegerMonitor("GpsInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsGps.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.GPS, 5, isReceivedPacket: true);
				line = 1;
				if (e.ConnectionData == null)
				{
					return;
				}
				line = 2;
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				SendLog($"#OnSaveRequestFired. #DataCount {e.ConnectionData.Count}");
				line = 3;
				if (connection == null)
				{
					SendLog("#OnSaveRequestFired. #ConnectionIsNullReturned");
					return;
				}
				ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = await Server.Core.Assistance.Initializer.Dal.Gps.SaveAsync(e.ConnectionData, connection.UserId, connection.DepId, connection.HashId, connection.AndroidIme, connection.UserAccGuid, connection.UserDepGuid, 10);
				line = 4;
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					SendLog("#OnSaveRequestFired. #DataSevedResponseError :" + returnedSaveFuncInfoWithValue.GetErrorMessage());
					return;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 5;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebGPS.GenerateSaveResponse(returnedSaveFuncInfoWithValue.value), Priority.Medium);
					SendLog("#OnSaveRequestFired. #SaveResponseSended");
				}
				line = 6;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line} /// just for send every time:{Guid.NewGuid()}", "GpsInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsGps.cs", 105);
			}
		}
	}
}
