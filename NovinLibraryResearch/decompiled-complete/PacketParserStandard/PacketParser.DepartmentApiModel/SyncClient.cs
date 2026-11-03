using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.LogPacket;
using PacketParser.SiteManagementApiModel;

namespace PacketParser.DepartmentApiModel;

public class SyncClient
{
	private static string Url = HttpHelpers.ApiUrl + "crm/sync/";

	public static async Task<List<WebLogPacketSum>> GetConnectionDataDetailAsync(short depId, long hashId, DateTime startDate, DateTime endDate)
	{
		using (new JaegerMonitor("GetConnectionDataDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", DepAdmin.UserName);
				ClientHttp.DefaultRequestHeaders.Add("Password", DepAdmin.Password);
				StringContent content = new StringContent(Json.ToStringJson(new WebLogPacketSumModel
				{
					DepId = depId,
					HashId = hashId,
					StartDate = startDate,
					EndDate = endDate
				}), Encoding.UTF8, "application/json");
				HttpResponseMessage httpResponseMessage = await ClientHttp.PostAsync("GetConnectionDataDetail", content);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					return await HttpHelpers.ReadJsonResponseAsync<List<WebLogPacketSum>>(httpResponseMessage);
				}
				return new List<WebLogPacketSum>();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetConnectionDataDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 45);
				return new List<WebLogPacketSum>();
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<Sync>>> Sync_ListByUserIdAsync(string userName, string pass, int selectedUserId)
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<List<Sync>> ret = new ReturnedSaveFuncInfoWithValue<List<Sync>>();
		if (string.IsNullOrEmpty(userName))
		{
			throw new Exception("UserName is null or empty");
		}
		if (string.IsNullOrEmpty(pass))
		{
			throw new Exception("pass is null or empty");
		}
		if (selectedUserId < 0)
		{
			throw new Exception("selectedUserId not set");
		}
		using (new JaegerMonitor("Sync_ListByUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient clientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				line = 5;
				clientHttp.DefaultRequestHeaders.Add("UserName", userName);
				line = 6;
				clientHttp.DefaultRequestHeaders.Add("Password", pass);
				line = 7;
				clientHttp.DefaultRequestHeaders.Add("UserId", selectedUserId.ToString());
				line = 8;
				HttpResponseMessage httpResponseMessage = await clientHttp.GetAsync("GetAllSyncByUserId", HttpCompletionOption.ResponseHeadersRead);
				line = 9;
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					List<Sync> value = await HttpHelpers.ReadJsonResponseAsync<List<Sync>>(httpResponseMessage);
					line = 10;
					ret.value = value;
					line = 11;
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "Sync_ListByUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 79);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "Sync_ListByUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 81);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, $"The Error Line Is :{line}", "Sync_ListByUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 86);
				ret.AddReturnedValue(ex3, "", "Sync_ListByUserIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 86);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<DepJsonResponse>> Sync_RestByPartAsync(string userName, string pass, long hashId, EntityPart entityPart)
	{
		ReturnedSaveFuncInfoWithValue<DepJsonResponse> ret = new ReturnedSaveFuncInfoWithValue<DepJsonResponse>();
		using (new JaegerMonitor("Sync_RestByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", userName);
				ClientHttp.DefaultRequestHeaders.Add("Password", pass);
				ClientHttp.DefaultRequestHeaders.Add("HashId", hashId.ToString());
				HttpRequestHeaders defaultRequestHeaders = ClientHttp.DefaultRequestHeaders;
				int num = (int)entityPart;
				defaultRequestHeaders.Add("Part", num.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("SyncRestByPart", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ret.value = await HttpHelpers.ReadJsonResponseAsync<DepJsonResponse>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "Sync_RestByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 112);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "Sync_RestByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 114);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "Sync_RestByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 120);
				ret.AddReturnedValue(ex3, "", "Sync_RestByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 120);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<DepJsonResponse>> Sync_DeleteAsync(string userName, string pass, long hashid)
	{
		using (new JaegerMonitor("Sync_DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<DepJsonResponse> ret = new ReturnedSaveFuncInfoWithValue<DepJsonResponse>();
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", userName);
				ClientHttp.DefaultRequestHeaders.Add("Password", pass);
				ClientHttp.DefaultRequestHeaders.Add("HashId", hashid.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("SyncDelete", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ret.value = await HttpHelpers.ReadJsonResponseAsync<DepJsonResponse>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "Sync_DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 145);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "Sync_DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 147);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "Sync_DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 153);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> Sync_ChangeLogLevel(string userName, string pass, long hashid, LogLevel plogLevel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Sync_ChangeLogLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", userName);
				ClientHttp.DefaultRequestHeaders.Add("Password", pass);
				ClientHttp.DefaultRequestHeaders.Add("HashId", hashid.ToString());
				HttpRequestHeaders defaultRequestHeaders = ClientHttp.DefaultRequestHeaders;
				int num = (int)plogLevel;
				defaultRequestHeaders.Add("MinLogLevel", num.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.PutAsync("MinLogLevelChange", null);
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					ret.AddError(EnSystem.None, httpResponseMessage.ToString(), "", "Sync_ChangeLogLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 173);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "Sync_ChangeLogLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 179);
				ret.AddReturnedValue(ex3, "", "Sync_ChangeLogLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 179);
			}
			return ret;
		}
	}

	public static async Task<Sync> GetByHashId(string userName, string pass, long hashid)
	{
		Sync ret = new Sync();
		using (new JaegerMonitor("GetByHashId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", userName);
				ClientHttp.DefaultRequestHeaders.Add("Password", pass);
				ClientHttp.DefaultRequestHeaders.Add("HashId", hashid.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("GetByHash", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ret = await HttpHelpers.ReadJsonResponseAsync<Sync>(httpResponseMessage);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "GetByHashId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 206);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<ConnectionData>>> GetConnectionDetailsAsync(int depid)
	{
		ReturnedSaveFuncInfoWithValue<List<ConnectionData>> ret = new ReturnedSaveFuncInfoWithValue<List<ConnectionData>>
		{
			value = new List<ConnectionData>()
		};
		using (new JaegerMonitor("GetConnectionDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs"))
		{
			_ = 1;
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				ClientHttp.DefaultRequestHeaders.Add("UserName", DepAdmin.UserName);
				ClientHttp.DefaultRequestHeaders.Add("Password", DepAdmin.Password);
				ClientHttp.DefaultRequestHeaders.Add("DepId", depid.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("GetConnectionDetails", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ret.value = await HttpHelpers.ReadJsonResponseAsync<List<ConnectionData>>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "GetConnectionDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 229);
					if (!ret.HasError)
					{
						ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "GetConnectionDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 231);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (!ex3.Message.ToLower().Contains("An error occurred while sending the request.".ToLower()))
				{
					WebErrorLog.Instance.StartLog(ex3, "", "GetConnectionDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 239);
				}
				ret.AddReturnedValue(ex3, "", "GetConnectionDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SyncClient.cs", 240);
			}
			return ret;
		}
	}
}
