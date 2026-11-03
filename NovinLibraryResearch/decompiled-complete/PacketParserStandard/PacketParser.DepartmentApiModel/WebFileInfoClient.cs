using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class WebFileInfoClient
{
	public static async Task<ReturnedSaveFuncInfo> DeleteWebFileInforClient(WebFileInfo webFile, string hostUrl, string userName, string password)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("username", userName);
			dictionary.Add("password", password);
			HttpResponseMessage httpResponseMessage = await HttpHelpers.PostStreamAsync(webFile, hostUrl + "api/FileInfo/DeleteFile", dictionary, CancellationToken.None);
			if (httpResponseMessage.StatusCode != HttpStatusCode.NoContent)
			{
				ret.AddReturnedHttpStatusCode(httpResponseMessage.StatusCode, null, "DeleteWebFileInforClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 27);
				if (!ret.HasError)
				{
					ret.AddError(EnSystem.None, httpResponseMessage.ReturnResponseError(), "", "DeleteWebFileInforClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 29);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteWebFileInforClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 34);
			ret.AddReturnedValue(ex, "", "DeleteWebFileInforClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 35);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>> GetFilesOutOfcapcity(short depid, FileManagement_Parts part, long capcity, string hostAddress, string userName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebFileInfo>> oprateList = new ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>
		{
			value = new List<WebFileInfo>()
		};
		try
		{
			ReturnedSaveFuncInfoWithValue<List<WebFileInfo>> returnedSaveFuncInfoWithValue = await DepClient.GetDepFiles(depid, part, hostAddress, userName, password, token);
			if (!(returnedSaveFuncInfoWithValue?.value?.Any()).GetValueOrDefault())
			{
				return oprateList;
			}
			long num = returnedSaveFuncInfoWithValue.value.Sum((WebFileInfo p) => p.FileLen);
			if (num < capcity)
			{
				oprateList.AddInformation(string.Format("Used space()% {0}(bytes) from {1:N0}(bytes)", num.NpToString("N0"), capcity), "FileSize", "GetFilesOutOfcapcity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 50);
				return oprateList;
			}
			returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue.value.OrderByDescending((WebFileInfo p) => p.Modified).ToList();
			long num2 = 0L;
			if (returnedSaveFuncInfoWithValue.value != null)
			{
				foreach (WebFileInfo item in returnedSaveFuncInfoWithValue.value)
				{
					num2 = checked(num2 + item.FileLen);
					if (num2 >= capcity)
					{
						oprateList.value.Add(item);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFilesOutOfcapcity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\WebFileInfoClient.cs", 67);
			oprateList.value = null;
		}
		oprateList.value = oprateList.value?.OrderBy((WebFileInfo p) => p.Modified)?.ToList();
		return oprateList;
	}

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<string>>> MoveFile(WebFileInfo info)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("UserName", DepAdmin.UserName);
		dictionary.Add("Password", DepAdmin.Password);
		return HttpHelpers.PostToApi<string, WebFileInfo>(info, HttpHelpers.ApiUrl + "Department/File/MoveFile", dictionary, strongDeserialize: false, 15000, 3);
	}
}
