using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.EventArgs;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal.Department;

public class ClsLkInfo
{
	public static ClsLkInfo Instance { get; } = new ClsLkInfo();


	public async Task WebLkInfo_OnSaveRequest(object sender, ConnectionEventArgs<WebLkInfo> e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<SaveResponse> responses = new List<SaveResponse>();
			foreach (WebLkInfo lk in e.ConnectionData)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await Server.Core.Assistance.Initializer.Dal.LkInfo.SaveAsync(lk, e.Connection.cancellationToken);
				ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "WebLkInfo_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Department\\ClsLkInfo.cs", 24);
				responses.Add(new SaveResponse
				{
					Message = returnedSaveFuncInfo.ErrorMessage,
					Shortmessage = returnedSaveFuncInfo.ErrorMessage,
					HostRefrenced = "",
					Guid = Guid.Empty,
					RequestRefrencedId = Guid.Empty,
					Date = DateTime.Now,
					PartitionId = 0,
					ReferencedId = lk.LKSerial,
					RequestPart = 100,
					SaveResultStatus = !returnedSaveFuncInfo.HasError
				});
			}
			ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
			if (!(connection?.IsDisposed ?? true))
			{
				await connection.SendPacketAsync(WebLkInfo.GenerateSaveResponse(responses), Priority.High);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebLkInfo_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Department\\ClsLkInfo.cs", 44);
		}
	}
}
