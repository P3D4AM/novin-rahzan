using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class clsTaxOrderLog
{
	private class NestedTaxOrderLog
	{
		internal static readonly clsTaxOrderLog Instance;

		static NestedTaxOrderLog()
		{
			Instance = new clsTaxOrderLog();
		}
	}

	public static clsTaxOrderLog Instance => NestedTaxOrderLog.Instance;

	internal async Task InstanceOnSaveRequestAsync(object sender, ConnectionEventArgs<WebTaxOrderLog> e)
	{
		using (new JaegerMonitor("InstanceOnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxOrderLog.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.TaxOrderLog, 1, isReceivedPacket: true);
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.TaxOrderLog.SaveAsync(e.Connection.DepId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InstanceOnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxOrderLog.cs", 39);
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
						await connection.SendPacketAsync(WebTaxOrderLog.GenerateSaveResponse(ret), Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "InstanceOnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxOrderLog.cs", 49);
			}
		}
	}
}
