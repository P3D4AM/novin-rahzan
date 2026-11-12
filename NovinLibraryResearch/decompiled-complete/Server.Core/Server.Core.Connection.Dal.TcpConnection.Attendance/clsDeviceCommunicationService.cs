using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;

namespace Server.Core.Connection.Dal.TcpConnection.Attendance;

public class clsDeviceCommunicationService : IDalAttendanceDeviceCommunicationService
{
	private class NestedDeviceCommunicationService
	{
		internal static readonly clsDeviceCommunicationService Instance;

		static NestedDeviceCommunicationService()
		{
			Instance = new clsDeviceCommunicationService();
		}
	}

	public static clsDeviceCommunicationService Instance => NestedDeviceCommunicationService.Instance;

	internal async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebDeviceLogOnPerson> e)
	{
		try
		{
			IEnumerable<SaveResponse> enumerable = Array.Empty<SaveResponse>();
			bool result = true;
			try
			{
				enumerable = enumerable.Append(new SaveResponse
				{
					SaveResultStatus = true,
					Guid = (e.ConnectionData?.FirstOrDefault().Guid ?? Guid.Empty)
				});
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\TcpConnection\\Attendance\\clsDeviceCommunicationService.cs", 40);
				enumerable = enumerable.Append(new SaveResponse
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
					await connection.SendPacketAsync(WebDeviceLogOnPerson.GenerateSaveResponse(enumerable), Priority.High);
				}
			}
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\TcpConnection\\Attendance\\clsDeviceCommunicationService.cs", 52);
		}
	}
}
