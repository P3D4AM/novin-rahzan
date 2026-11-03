using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class AppSettingsClient
{
	private static string Url = HttpHelpers.ApiUrl + "AppSettings/";

	public static async Task<ReturnedSaveFuncInfoWithValue<WebApplicationSetting>> AppSettings_GetValueAsync(string userName, string pass, string name)
	{
		ReturnedSaveFuncInfoWithValue<WebApplicationSetting> ret = new ReturnedSaveFuncInfoWithValue<WebApplicationSetting>();
		using (new JaegerMonitor("AppSettings_GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs"))
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
				ClientHttp.DefaultRequestHeaders.Add("KeyName", name.ToString());
				HttpResponseMessage httpResponseMessage = await ClientHttp.GetAsync("AppSettingsGetValue", HttpCompletionOption.ResponseHeadersRead);
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					ret.value = await HttpHelpers.ReadJsonResponseAsync<WebApplicationSetting>(httpResponseMessage);
				}
				else
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "AppSettings_GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 35);
					ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "AppSettings_GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 36);
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
				WebErrorLog.Instance.StartLog(ex3, "", "AppSettings_GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 41);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> AppSettings_SaveListAsync(string userName, string pass, List<WebApplicationSetting> lstApplicationSettings)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("AppSettings_SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs"))
		{
			try
			{
				using HttpClient ClientHttp = new HttpClient
				{
					BaseAddress = new Uri(Url)
				};
				StringContent content = new StringContent(Json.ToStringJson(new ApplictionSettingsModel
				{
					List = lstApplicationSettings,
					UserName = userName,
					Password = pass
				}), Encoding.UTF8, "application/json");
				HttpResponseMessage httpResponseMessage = await ClientHttp.PostAsync("AppSettingsSaveList", content);
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "AppSettings_SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 63);
					ret.AddError(EnSystem.None, httpResponseMessage.StatusCode.ToString(), "", "AppSettings_SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 64);
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
				WebErrorLog.Instance.StartLog(ex3, "", "AppSettings_SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 72);
				ret.AddReturnedValue(ex3, "", "AppSettings_SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppSettingsClient.cs", 73);
			}
			return ret;
		}
	}
}
