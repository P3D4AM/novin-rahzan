using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;

namespace PacketParser.DepartmentApiModel;

public class SmsParserClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<List<SmsParserTemplate>>> GetAllAsync(string depUser, string depPass, ENSource source, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<SmsParserTemplate>> ret = new ReturnedSaveFuncInfoWithValue<List<SmsParserTemplate>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<List<SmsParserTemplate>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/Hesabyar/SmsPattern/GetAll").GetFromApi<List<SmsParserTemplate>>(depUser, depPass, source, token, 3);
				ret.AddReturnedValue(ret, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 22);
				if (!ret.HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
				else
				{
					ret.AddReturnedValue(ret, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 26);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 30);
				ret.AddError(EnSystem.None, ex.Message, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 31);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteAsync(string depUser, string depPass, ENSource source, Guid IdSmsParser)
	{
		using (new JaegerMonitor("DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"UserName",
						DepAdmin.UserName
					},
					{
						"Password",
						DepAdmin.Password
					},
					{
						"IdSmsParser",
						IdSmsParser.ToString()
					}
				};
				string url = HttpHelpers.ApiUrl + "api/Hesabyar/SmsPattern/DeleteSmsParserAsync/";
				ret.AddReturnedValue(await HttpHelpers.PostToApi(IdSmsParser, url, depUser, depPass, source, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 50);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 54);
				ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 55);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(string depuser, string deppass, ENSource source, SmsParserTemplate parserTemplate)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"UserName",
						DepAdmin.UserName
					},
					{
						"Password",
						DepAdmin.Password
					}
				};
				string url = HttpHelpers.ApiUrl + "api/Hesabyar/SmsPattern/SaveSmsParserAsync";
				ret.AddReturnedValue(await HttpHelpers.PostToApi(parserTemplate, url, depuser, deppass, source, headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 72);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 76);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\SmsParserClient.cs", 77);
			}
			return ret;
		}
	}
}
