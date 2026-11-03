using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs.WebApi;

namespace PacketParser.DepartmentApiModel;

public class DepUserClient
{
	private static string Url = HttpHelpers.ApiUrl + "Department/DepUser/";

	private static string UrlHistory = HttpHelpers.ApiUrl + "Department/ClientLoginHistory/";

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebUser>>> GetAllAsync(string departmnetUsername, string departmentPassword, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebUser>> ret = new ReturnedSaveFuncInfoWithValue<List<WebUser>>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			try
			{
				string url = Url + "GetAll";
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUser>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebUser>, WebApiBaseEventArgs<string>>(new WebApiBaseEventArgs<string>
				{
					UserName = (departmnetUsername ?? ""),
					PassWord = departmentPassword,
					Key = "",
					CountInPage = 10000,
					Pages = 1
				}, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 31);
				if (ret.HasError)
				{
					return ret;
				}
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 37);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 38);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebUser>> GetUser(string departmnetUsername, string departmentPassword, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebUser> ret = new ReturnedSaveFuncInfoWithValue<WebUser>
		{
			value = new WebUser()
		};
		using (new JaegerMonitor("GetUser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			try
			{
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetUser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 50);
					return ret;
				}
				WebApiBaseEventArgs<string> item = new WebApiBaseEventArgs<string>
				{
					UserName = (departmnetUsername ?? ""),
					PassWord = departmentPassword,
					Key = "",
					ENSource = source,
					CountInPage = 10000,
					Pages = 1
				};
				string url = Url + "GetAll";
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUser>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebUser>, WebApiBaseEventArgs<string>>(item, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetUser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 56);
				if (ret.HasError)
				{
					return ret;
				}
				List<WebUser> data = returnedSaveFuncInfoWithValue.value.Data;
				ret.value = data.FirstOrDefault((WebUser p) => p.Type == ENSource.Accounting || p.Type == ENSource.TimeAttendance);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetUser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 63);
				ret.AddReturnedValue(ex, "", "GetUser", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 64);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebUserAcc>>> GetAllAccAsync(WebApiBaseEventArgs<string> model)
	{
		ReturnedSaveFuncInfoWithValue<List<WebUserAcc>> ret = new ReturnedSaveFuncInfoWithValue<List<WebUserAcc>>();
		using (new JaegerMonitor("GetAllAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			try
			{
				string url = Url + "GetAllAcc";
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUserAcc>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebUserAcc>, WebApiBaseEventArgs<string>>(model, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 76);
				if (ret.HasError)
				{
					return ret;
				}
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 82);
				ret.AddReturnedValue(ex, "", "GetAllAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 83);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<ClientLoginHistory>>> GetAllClientHistoryAsync(ClientHistoryAPiGetALLModel model, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<ClientLoginHistory>> ret = new ReturnedSaveFuncInfoWithValue<List<ClientLoginHistory>>
		{
			value = new List<ClientLoginHistory>()
		};
		using (new JaegerMonitor("GetAllClientHistoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			try
			{
				string url = UrlHistory + "GetAll";
				ReturnedSaveFuncInfoWithValue<WebResponse<List<ClientLoginHistory>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<ClientLoginHistory>, ClientHistoryAPiGetALLModel>(model, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllClientHistoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 95);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllClientHistoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 101);
				ret.AddReturnedValue(ex, "", "GetAllClientHistoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 102);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteUserAccAsync(string depId)
	{
		using (new JaegerMonitor("DeleteUserAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using (new HttpClient
				{
					BaseAddress = new Uri(Url)
				})
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("UserName", DepAdmin.UserName);
					dictionary.Add("Password", DepAdmin.Password);
					dictionary.Add("DepId", depId);
					ret.AddReturnedValue(await (Url + "DeleteUserAcc").GetFromApi<DepJsonResponse>(default(CancellationToken), 1, dictionary), ReturnedState.None, "DeleteUserAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 118);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteUserAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 122);
				ret.AddReturnedValue(ex, "", "DeleteUserAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 123);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> Save(WebApiBaseEventArgs<WebUser> model)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		new HttpResponseMessage(HttpStatusCode.OK);
		using (new JaegerMonitor("Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			_ = 1;
			try
			{
				HttpClient obj = new HttpClient
				{
					BaseAddress = new Uri(HttpHelpers.ApiUrl)
				};
				StringContent content = new StringContent(Json.ToStringJson(model), Encoding.UTF8, "application/json");
				HttpResponseMessage res = await obj.PostAsync("Department/DepUser/Save", content);
				string text = await res.Content.ReadAsStringAsync();
				if (res.StatusCode == HttpStatusCode.OK)
				{
					return ret;
				}
				ret.AddError(EnSystem.None, JsonConvert.DeserializeObject<ApiResponseModel<bool>>(text).Message, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 141);
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 146);
				ret.AddReturnedValue(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 147);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResetImeAsync(string userName, string pass, short id)
	{
		using (new JaegerMonitor("ResetImeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserName", userName);
				dictionary.Add("Password", pass);
				dictionary.Add("UserId", id.ToString());
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				return returnedSaveFuncInfo.AddReturnedValue(await (Url + "ResetIME").GetFromApi<DepJsonResponse>(default(CancellationToken), 1, dictionary), ReturnedState.None, "ResetImeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 161);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetImeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 163);
				ret.AddReturnedValue(ex, "", "ResetImeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 163);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResetDbGuidAsync(string userName, string pass, short id, CancellationToken token)
	{
		using (new JaegerMonitor("ResetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserName", userName);
				dictionary.Add("Password", pass);
				dictionary.Add("UserId", id.ToString());
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				return returnedSaveFuncInfo.AddReturnedValue(await (Url + "ResetDBGuid").GetFromApi<DepJsonResponse>(token, 1, dictionary), ReturnedState.None, "ResetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 176);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 179);
				ret.AddReturnedValue(ex, "", "ResetDbGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 179);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResetPC_SerialAsync(string departmentUser, string departmentPass, short id, CancellationToken token)
	{
		using (new JaegerMonitor("ResetPC_SerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserName", departmentUser);
				dictionary.Add("Password", departmentPass);
				dictionary.Add("UserId", id.ToString());
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				return returnedSaveFuncInfo.AddReturnedValue(await (Url + "ResetPCSerial").GetFromApi<DepJsonResponse>(token, 1, dictionary), ReturnedState.None, "ResetPC_SerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 193);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetPC_SerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 196);
				ret.AddReturnedValue(ex, "", "ResetPC_SerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 196);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResetServer_ParametersAsync(string departmentUser, string departmentPass, short id, CancellationToken token)
	{
		using (new JaegerMonitor("ResetServer_ParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserName", departmentUser);
				dictionary.Add("Password", departmentPass);
				dictionary.Add("UserId", id.ToString());
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				return returnedSaveFuncInfo.AddReturnedValue(await (Url + "ResetServerParameters").GetFromApi<DepJsonResponse>(token, 1, dictionary), ReturnedState.None, "ResetServer_ParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 211);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetServer_ParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 214);
				ret.AddReturnedValue(ex, "", "ResetServer_ParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepUserClient.cs", 214);
			}
			return ret;
		}
	}
}
