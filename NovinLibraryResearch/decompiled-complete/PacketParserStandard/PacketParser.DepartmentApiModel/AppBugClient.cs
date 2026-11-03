using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.AppBug;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class AppBugClient
{
	private static string Url = HttpHelpers.ApiUrl + "AppBug/";

	public static async Task<ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug>> Get(long id, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug> ret = new ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug>
		{
			value = new PacketParser.AppBug.AppBug()
		};
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("id", id.ToString());
			ReturnedSaveFuncInfoWithValue<WebResponse<PacketParser.AppBug.AppBug>> returnedSaveFuncInfoWithValue = await (Url + "GetByID").GetFromApi<PacketParser.AppBug.AppBug>(DepAdmin.UserName, DepAdmin.Password, source, token, 3, dictionary);
			if (!ret.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddReturnedValue(ret, ReturnedState.None, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 26);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 28);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug>> Get(Guid pguid, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug> ret = new ReturnedSaveFuncInfoWithValue<PacketParser.AppBug.AppBug>
		{
			value = new PacketParser.AppBug.AppBug()
		};
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Guid", pguid.ToString());
			ReturnedSaveFuncInfoWithValue<WebResponse<PacketParser.AppBug.AppBug>> returnedSaveFuncInfoWithValue = await (Url + "Get").GetFromApi<PacketParser.AppBug.AppBug>(DepAdmin.UserName, DepAdmin.Password, source, token, 3, dictionary);
			if (!ret.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddReturnedValue(ret, ReturnedState.None, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 42);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 46);
			ret.AddReturnedValue(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 47);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>>> Get(string pVersion, ENSource pSource, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>> ret = new ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>>
		{
			value = new List<PacketParser.AppBug.AppBug>()
		};
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Version", pVersion);
			short num = (short)pSource;
			dictionary.Add("Source", num.ToString());
			ReturnedSaveFuncInfoWithValue<WebResponse<List<PacketParser.AppBug.AppBug>>> returnedSaveFuncInfoWithValue = await (Url + "GetByVersion").GetFromApi<List<PacketParser.AppBug.AppBug>>(DepAdmin.UserName, DepAdmin.Password, pSource, token, 3, dictionary);
			if (!ret.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddReturnedValue(ret, ReturnedState.None, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 63);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 65);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Delete(Guid pguid, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Guid", pguid.ToString());
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await (Url + "Delete").GetFromApi<bool>(DepAdmin.UserName, DepAdmin.Password, source, token, 3, dictionary), ReturnedState.None, "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 75);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 79);
			ret.AddReturnedValue(ex, "", "Delete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 80);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> Save(PacketParser.AppBug.AppBug model)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApi<ReturnedSaveFuncInfo, PacketParser.AppBug.AppBug>(model, Url + "Save", DepAdmin.UserName, DepAdmin.Password, model.Source, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 90);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 95);
			ret.AddReturnedValue(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 96);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>>> GetAll(CancellationToken token, ENSource source)
	{
		ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>> ret = new ReturnedSaveFuncInfoWithValue<List<PacketParser.AppBug.AppBug>>();
		try
		{
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 107);
				return ret;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 113);
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<List<PacketParser.AppBug.AppBug>>> returnedSaveFuncInfoWithValue = await (Url + "GetAll").GetFromApi<List<PacketParser.AppBug.AppBug>>(DepAdmin.UserName, DepAdmin.Password, source, token, 3, headers);
			if (!ret.HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddReturnedValue(ret, ReturnedState.None, "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 123);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\AppBugClient.cs", 125);
		}
		return ret;
	}
}
