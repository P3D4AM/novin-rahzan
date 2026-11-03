using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace PacketParser.DepartmentApiModel.Ticket;

public static class TicketDetailClient
{
	private static string Url = HttpHelpers.CrmApiUrl + "api/TicketDetail/";

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(WebTicketDet ticket)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs"))
		{
			try
			{
				string url = Url + "Save";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					}
				};
				ret.AddReturnedValue(await HttpHelpers.PostToApiByAdminToken<SaveResponse, WebTicketDet>(ticket, url, DepAdmin.UserName, DepAdmin.Password, ENSource.None, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 29);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 33);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 34);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>> GetAllAsync(TicketDetFilter model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebTicketReport>> ret = new ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs"))
		{
			try
			{
				string url = Url + "GetAll";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					}
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTicketReport>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebTicketReport>, TicketDetFilter>(model, url, headers, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 51);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 57);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketDetailClient.cs", 58);
			}
			return ret;
		}
	}
}
