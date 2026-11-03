using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace PacketParser.DepartmentApiModel.Ticket;

public static class TicketClient
{
	private static string Url = HttpHelpers.CrmApiUrl + "api/Ticket/";

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(WebTicket ticket)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
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
				ret.AddReturnedValue(await HttpHelpers.PostToApiByAdminToken<SaveResponse, WebTicket>(ticket, url, DepAdmin.UserName, DepAdmin.Password, ENSource.None, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 30);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 34);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 35);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebTicket>> GetAsync(Guid guid)
	{
		ReturnedSaveFuncInfoWithValue<WebTicket> ret = new ReturnedSaveFuncInfoWithValue<WebTicket>();
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
		{
			try
			{
				string url = Url + "Get";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					},
					{
						"guid",
						guid.ToString()
					}
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<WebTicket>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebTicket>(default(CancellationToken), 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 53);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 59);
				ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 60);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebTicket>> GetByNumberAsync(int number)
	{
		ReturnedSaveFuncInfoWithValue<WebTicket> ret = new ReturnedSaveFuncInfoWithValue<WebTicket>();
		using (new JaegerMonitor("GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
		{
			try
			{
				string url = Url + "GetByNumber";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					},
					{
						"number",
						number.ToString()
					}
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<WebTicket>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebTicket>(default(CancellationToken), 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 78);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 84);
				ret.AddReturnedValue(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 85);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> GetNextNumberAsync()
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		using (new JaegerMonitor("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
		{
			try
			{
				string url = Url + "GetNextNumber";
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
				ReturnedSaveFuncInfoWithValue<WebResponse<int>> returnedSaveFuncInfoWithValue = await url.GetFromApi<int>(default(CancellationToken), 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 102);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 108);
				ret.AddReturnedValue(ex, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 109);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
		{
			try
			{
				string url = Url + "ChangeStatus";
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
				ret.AddReturnedValue(await HttpHelpers.PostToApiByAdminToken<SaveResponse, TicketChangeStatusModel>(new TicketChangeStatusModel
				{
					StatusId = statusId,
					Guid = guid
				}, url, DepAdmin.UserName, DepAdmin.Password, ENSource.None, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 131);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 135);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 136);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>> GetAllAsync(TicketFilter model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebTicketReport>> ret = new ReturnedSaveFuncInfoWithValue<List<WebTicketReport>>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
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
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTicketReport>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebTicketReport>, TicketFilter>(model, url, headers, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 153);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 159);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 160);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<TicketTitleSuggest>>> GetAllSuggestTitelsAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<TicketTitleSuggest>> ret = new ReturnedSaveFuncInfoWithValue<List<TicketTitleSuggest>>();
		using (new JaegerMonitor("GetAllSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs"))
		{
			try
			{
				string url = Url + "GetSuggestTitels";
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
				ReturnedSaveFuncInfoWithValue<WebResponse<List<TicketTitleSuggest>>> returnedSaveFuncInfoWithValue = await url.GetFromApi<List<TicketTitleSuggest>>(default(CancellationToken), 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 177);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 183);
				ret.AddReturnedValue(ex, "", "GetAllSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\Ticket\\TicketClient.cs", 184);
			}
			return ret;
		}
	}
}
