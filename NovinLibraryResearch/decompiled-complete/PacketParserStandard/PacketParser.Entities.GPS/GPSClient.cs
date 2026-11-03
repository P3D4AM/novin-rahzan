using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Department;
using PacketParser.Entities.Settings;

namespace PacketParser.Entities.GPS;

public class GPSClient : IDisposable
{
	private readonly GPSClientAppSettings appSettings;

	private LoginInfo _loginInfo;

	public GPSClient(GPSClientAppSettings appSettings)
	{
		this.appSettings = appSettings;
	}

	public async Task<List<WebUser>> GetUsersAsync(short depId)
	{
		using (new JaegerMonitor("GetUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUser>>> returnedSaveFuncInfoWithValue = await $"{appSettings.GpsAdminUrl}/User_List/{depId}".GetFromApi<List<WebUser>>(_loginInfo?.UserName, _loginInfo?.Password, ENSource.GpsPortal, default(CancellationToken), 3);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					return null;
				}
				return returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 41);
				return null;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<string>> Login(string userName, string password)
	{
		using (new JaegerMonitor("Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
			try
			{
				_loginInfo = new LoginInfo
				{
					Password = password,
					UserName = userName,
					Source = ENSource.GpsPortal
				};
				ret = await new RestTokenManagement(_loginInfo.Source, _loginInfo.UserName, _loginInfo.Password).GetToken(default(CancellationToken));
				if (ret.AddReturnedValue(ret, ReturnedState.None, "Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 61).HasError)
				{
					return ret;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 65);
				ret.AddError(ex.Message, "Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 66);
			}
			return ret;
		}
	}

	public async Task<bool> DeleteUserAsync(short depId, int id)
	{
		using (new JaegerMonitor("DeleteUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await $"{appSettings.GpsAdminUrl}/User_Delete/{id}/{depId}".GetFromApi<bool>(_loginInfo, default(CancellationToken), 3);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					return false;
				}
				return returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 86);
				return true;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebGPS>>> GetPointsAsync(int userID, DateTime start, DateTime end)
	{
		using (new JaegerMonitor("GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebGPS>> ret = new ReturnedSaveFuncInfoWithValue<List<WebGPS>>();
			try
			{
				GpsPointsRequest item = new GpsPointsRequest
				{
					Finish = end,
					Start = start,
					UserID = userID
				};
				new HttpClient();
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebGPS>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<List<WebGPS>, GpsPointsRequest>(item, appSettings.GpsAdminUrl + "/Gps_List", _loginInfo, null, 60000, 5, default(CancellationToken), useAdminLogin: false);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 107);
				if (!ret.HasError && returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 113);
				ret.AddError(ex.Message, "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 114);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<OnlineUsersDep>>> GetOnlineUsersAsync(short depId)
	{
		using (new JaegerMonitor("GetOnlineUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<OnlineUsersDep>> ret = new ReturnedSaveFuncInfoWithValue<List<OnlineUsersDep>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<OnlineUsersDep>>> returnedSaveFuncInfoWithValue = await $"{appSettings.HostUrl}api/v1/depAdmin/GetConnectionsDetail/{depId}".GetFromApi<List<OnlineUsersDep>>(_loginInfo, default(CancellationToken), 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetOnlineUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 127);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetOnlineUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 133);
				ret.AddError(ex.Message, "GetOnlineUsersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 134);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebGPS>>> GetPointsAsync(short depId)
	{
		using (new JaegerMonitor("GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebGPS>> ret = new ReturnedSaveFuncInfoWithValue<List<WebGPS>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebGPS>>> returnedSaveFuncInfoWithValue = await $"{appSettings.GpsAdminUrl}/Gps_ListUsers/{depId}".GetFromApi<List<WebGPS>>(_loginInfo, default(CancellationToken), 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 147);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 154);
				ret.AddError(ex.Message, "GetPointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 155);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> SaveUserAsync(WebUser webUser)
	{
		using (new JaegerMonitor("SaveUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<ApiResponseModel<bool>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<ApiResponseModel<bool>, WebUser>(webUser, appSettings.HostUrl + "api/v1/DepUser/save", _loginInfo?.UserName, _loginInfo.Password, ENSource.None, null, strongDeserialize: false, 15000, 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 168);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 174);
				ret.AddError(ex.Message, "SaveUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 175);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebDepartment>> GetDepartmentAsync()
	{
		using (new JaegerMonitor("GetDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebDepartment> ret = new ReturnedSaveFuncInfoWithValue<WebDepartment>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await (appSettings.HostUrl + "api/v1/DepAdmin/GetDeparmantByUser/").GetFromApi<WebDepartment>(_loginInfo, default(CancellationToken), 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 188);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 194);
				ret.AddError(ex.Message, "GetDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 195);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebApplicationSetting>>> GetApplicationSettings(ApplicationSettingsRequestModel model)
	{
		using (new JaegerMonitor("GetApplicationSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WebApplicationSetting>> ret = new ReturnedSaveFuncInfoWithValue<List<WebApplicationSetting>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebApplicationSetting>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebApplicationSetting>, ApplicationSettingsRequestModel>(model, appSettings.HostUrl + "api/v1/AppSettings/GetAppSettings", _loginInfo?.UserName, _loginInfo.Password, ENSource.None, null, strongDeserialize: false, 15000, 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetApplicationSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 207);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetApplicationSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 213);
				ret.AddError(ex.Message, "GetApplicationSettings", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 214);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> AppSettings_SaveList(List<WebApplicationSetting> lstappsetting)
	{
		using (new JaegerMonitor("AppSettings_SaveList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<bool, List<WebApplicationSetting>>(lstappsetting, appSettings.HostUrl + "api/v1/AppSettings/AppSettingsSaveList", _loginInfo?.UserName, _loginInfo.Password, ENSource.None, null, strongDeserialize: false, 15000, 3);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode != HttpStatusCode.OK)
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "AppSettings_SaveList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 228);
				}
				else
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AppSettings_SaveList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 234);
				ret.AddError(ex.Message, "AppSettings_SaveList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GPS\\GPSClient.cs", 235);
			}
			return ret;
		}
	}

	public void Dispose()
	{
	}
}
