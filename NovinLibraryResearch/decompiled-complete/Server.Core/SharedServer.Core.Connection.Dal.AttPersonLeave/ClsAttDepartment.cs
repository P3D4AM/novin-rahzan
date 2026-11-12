using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;
using Server.Core.Assistance;
using Server.Core.Connection;

namespace SharedServer.Core.Connection.Dal.AttPersonLeave;

public class ClsAttDepartment
{
	private class NestedAttPersonLeave
	{
		internal static readonly ClsAttDepartment Instance;

		static NestedAttPersonLeave()
		{
			Instance = new ClsAttDepartment();
		}
	}

	public static ClsAttDepartment Instance => NestedAttPersonLeave.Instance;

	internal async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebAttDepartment> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAttDepartment.cs"))
		{
			_ = 1;
			try
			{
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Initializer.Dal.AttDepartment.SaveAsync(e.Connection.DepId, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAttDepartment.cs", 38);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (!(connection?.IsDisposed ?? true))
					{
						await connection.SendPacketAsync(WebAttDepartment.GenerateSaveResponse(ret), Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAttDepartment.cs", 50);
			}
		}
	}
}
