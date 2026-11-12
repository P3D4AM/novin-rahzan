using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsAppicationSettings : IEntity<WebApplicationSetting>
{
	private class NestedAppicationSettings
	{
		internal static readonly ClsAppicationSettings AppicationSettingsInstance;

		static NestedAppicationSettings()
		{
			AppicationSettingsInstance = new ClsAppicationSettings();
		}
	}

	public static ClsAppicationSettings AppicationSettingsInstance => NestedAppicationSettings.AppicationSettingsInstance;

	private ClsAppicationSettings()
	{
	}

	public async Task<List<WebApplicationSetting>> ApplyAccessAsync(IEnumerable<WebApplicationSetting> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebApplicationSetting p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task AppSettingInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebApplicationSetting> e)
	{
		Stopwatch sw = Stopwatch.StartNew();
		Stopwatch swTotal = Stopwatch.StartNew();
		StringBuilder sb = new StringBuilder();
		using (new JaegerMonitor("AppSettingInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAppicationSettings.cs"))
		{
			try
			{
				sb.Append(string.Format("AppSettingInstance_OnSaveRequest depId:{0}// count:{1} //e.part:{2} //e.TotalParts:{3} // keys:{4}", e.Connection.DepId, e.ConnectionData.Count, e.Part, e.TotalParts, string.Join(",", e.ConnectionData.Select((WebApplicationSetting p) => p.Name))));
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ApplicationSetting, 1, isReceivedPacket: true);
				if (e == null || e.ConnectionData.Count > 0)
				{
					List<SaveResponse> ret = new List<SaveResponse>();
					bool result = true;
					try
					{
						await Server.Core.Assistance.Initializer.Dal.Settings.SaveAsync(e.Connection.DepId, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.cancellationToken ?? default(CancellationToken));
						sb.AppendLine($"SaveTime:{sw.ElapsedMilliseconds}(ms)");
						Stopwatch.StartNew();
						ret.Add(new SaveResponse
						{
							SaveResultStatus = true
						});
					}
					catch (OperationCanceledException)
					{
						sb.AppendLine("OperationCanceledException");
						return;
					}
					catch (Exception ex2)
					{
						sb.AppendLine("exception:" + ex2.Message);
						WebErrorLog.Instance.StartLog(ex2, "", "AppSettingInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAppicationSettings.cs", 63);
						ret.Add(new SaveResponse
						{
							SaveResultStatus = false
						});
						result = false;
					}
					if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
					{
						ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebApplicationSetting.GenerateSaveResponse(ret), Priority.High);
					}
				}
			}
			catch (OperationCanceledException)
			{
				sb.AppendLine("OperationCanceledException");
			}
			catch (Exception ex4)
			{
				sb.AppendLine("exception2:" + ex4.Message);
				WebErrorLog.Instance.StartLog(ex4, "", "AppSettingInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAppicationSettings.cs", 78);
			}
			finally
			{
				sb.AppendLine($"totalTme:{swTotal.ElapsedMilliseconds}(ms)");
				sb.ToString();
			}
		}
	}
}
