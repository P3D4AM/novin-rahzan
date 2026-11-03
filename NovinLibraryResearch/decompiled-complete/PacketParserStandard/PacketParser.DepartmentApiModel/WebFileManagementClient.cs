using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class WebFileManagementClient
{
	private static string Url = HttpHelpers.ApiUrl + "Department/WebFileManagement/";

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebFileManagement>>> GetAllApps(FileManagement_Parts part, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebFileManagement>> ret = new ReturnedSaveFuncInfoWithValue<List<WebFileManagement>>
		{
			value = new List<WebFileManagement>()
		};
		using (new JaegerMonitor("GetAllApps", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs"))
		{
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileManagement>>> returnedSaveFuncInfoWithValue = await $"{Url}GetAllByPart/{(short)part}".GetFromApi<List<WebFileManagement>>(token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllApps", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 21);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllApps", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 27);
				ret.AddReturnedValue(ex, "", "GetAllApps", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 28);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebFileManagement>> GetById(int id, CancellationToken token)
	{
		using (new JaegerMonitor("GetById", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebFileManagement> ret = new ReturnedSaveFuncInfoWithValue<WebFileManagement>
			{
				value = new WebFileManagement()
			};
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebFileManagement>> returnedSaveFuncInfoWithValue = await $"{Url}GetById/{id}".GetFromApi<WebFileManagement>(token, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetById", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 40);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetById", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 45);
				ret.AddReturnedValue(ex, "", "GetById", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 46);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveFile(WebFileManagement item)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs"))
		{
			try
			{
				ret.AddReturnedValue(await HttpHelpers.PostToApi(item, Url + "SaveFile", null, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 58);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 62);
				ret.AddReturnedValue(ex, "", "SaveFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 63);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteFile(int id, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("DeleteFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs"))
		{
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ID", id.ToString());
				ret.AddReturnedValue(await (Url + "DeleteFile").GetFromApi<bool>(token, 2, dictionary), ReturnedState.None, "DeleteFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 76);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 80);
				ret.AddReturnedValue(ex, "", "DeleteFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileManagementClient.cs", 81);
			}
			return ret;
		}
	}
}
