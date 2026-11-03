using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs.WebApi;
using PacketParser.SiteManagementApiModel;

namespace PacketParser.DepartmentApiModel;

public class FileCLient
{
	private static string Url = HttpHelpers.ApiUrl + "Webservice/File/";

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebPageContainer<WebFileInfo>>>> GetAllAsync(WebApiBaseEventArgs<short> model)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<WebPageContainer<WebFileInfo>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebPageContainer<WebFileInfo>>>();
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs"))
		{
			try
			{
				string url = Url + "GetAll";
				ReturnedSaveFuncInfoWithValue<WebResponse<WebPageContainer<WebFileInfo>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<WebPageContainer<WebFileInfo>, WebApiBaseEventArgs<short>>(model, url, null, strongDeserialize: false, 30000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 26);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 31);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 32);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<bool>>> DeleteAsync(WebApiBaseEventArgs<Guid> model)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<bool>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<bool>>();
		using (new JaegerMonitor("DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs"))
		{
			try
			{
				string url = Url + "Delete";
				ReturnedSaveFuncInfoWithValue<WebResponse<bool>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<bool, WebApiBaseEventArgs<Guid>>(model, url, null, strongDeserialize: false, 15000, 3);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 45);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 50);
				ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 51);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<string>>> CreateAsync(short depId, WebApiBaseEventArgs<FileManagement_Parts> model, string fileName, byte[] fileBytes, string localPath, bool renameIfExist)
	{
		using (new JaegerMonitor("CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<string>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<string>>();
			try
			{
				if (fileBytes.Length > 1000000 && localPath != "")
				{
					ret.AddError(EnSystem.None, "در تغییر انداره فایل خطا روی داده است", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 64);
					return ret;
				}
				if (renameIfExist && await CheckExistUrLAsync(GetUrl(depId, model.Key, fileName), 2))
				{
					fileName = Guid.NewGuid().ToString().Substring(0, 10) + fileName;
				}
				Dictionary<string, string> headers = new Dictionary<string, string>();
				string url = Url + "Create";
				fileName = fileName.Replace("+", "");
				WebApiBaseEventArgs<FileCreateApiModel> item = new WebApiBaseEventArgs<FileCreateApiModel>(new FileCreateApiModel
				{
					Part = model.Key,
					FileName = fileName,
					FileBytes = fileBytes
				}, model.UserName, model.PassWord, model.ENSource, model.AndroidIme, 0, 0);
				ReturnedSaveFuncInfoWithValue<WebResponse<string>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await HttpHelpers.PostToApi<string, WebApiBaseEventArgs<FileCreateApiModel>>(item, url, headers, strongDeserialize: false, 30000, 3), ReturnedState.None, "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 80);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 85);
				ret.AddReturnedValue(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 86);
			}
			return ret;
		}
	}

	public static string GetUrl(short depId, FileManagement_Parts directory, string pic, string subDirectory = "")
	{
		try
		{
			return $"https://www.novinp.ir/WebService/files/{depId}/{directory.GetRemoteFileAttribute()}/{pic}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUrl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 106);
			return "";
		}
	}

	public static async Task<bool> CheckExistUrLAsync(string url, short tryCount = 2)
	{
		bool result = default(bool);
		int num;
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Method = "HEAD";
			HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
			if (httpWebResponse.ContentType.ToLower() == "image/jpeg")
			{
				result = true;
				return result;
			}
			if (httpWebResponse.ContentType.ToLower() == "image/png")
			{
				result = true;
				return result;
			}
			if (httpWebResponse.ContentType.ToLower() == "image/jpg")
			{
				result = true;
				return result;
			}
			if (httpWebResponse.ContentType.ToLower() == "image/gif")
			{
				result = true;
				return result;
			}
			if (httpWebResponse.ContentType.ToLower() == "image/webp")
			{
				result = true;
				return result;
			}
			result = false;
			return result;
		}
		catch (TimeoutException)
		{
			num = 1;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "Url:" + url, "CheckExistUrLAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\FileCLient.cs", 140);
			return false;
		}
		if (num != 1)
		{
			return result;
		}
		if (tryCount > 0)
		{
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await CheckExistUrLAsync(url, num2);
		}
		return true;
	}
}
