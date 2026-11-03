using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.DepartmentApiModel;

public class DepClient
{
	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>>> WebDepartmentCompactApi(string depUser, string depPass, ENSource source, CancellationToken token, ENSource? userType)
	{
		using (new JaegerMonitor("WebDepartmentCompactApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>>();
			try
			{
				string url = HttpHelpers.ApiUrl + "Department/WebDepartmentCompact/GetAll";
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (userType.HasValue)
				{
					dictionary.Add("UserType", ((short)userType.Value).ToString());
				}
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartmentCompact>>> returnedSaveFuncInfoWithValue = await url.GetFromApi<List<WebDepartmentCompact>>(depUser, depPass, source, token, 3, dictionary);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "WebDepartmentCompactApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 25);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "WebDepartmentCompactApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 30);
				ret.AddReturnedValue(ex, "", "WebDepartmentCompactApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 31);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebDepartment>>> GetAllDep(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebDepartment>> ret = new ReturnedSaveFuncInfoWithValue<List<WebDepartment>>
		{
			value = new List<WebDepartment>()
		};
		try
		{
			using (new JaegerMonitor("GetAllDep", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("UserName", DepAdmin.UserName);
				dictionary.Add("Password", DepAdmin.Password);
				ReturnedSaveFuncInfoWithValue<WebResponse<List<WebDepartment>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/FileInfo/list").GetFromApi<List<WebDepartment>>(token, 3, dictionary);
				if (!ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllDep", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 45).HasError)
				{
					ret.value = returnedSaveFuncInfoWithValue.value.Data;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllDep", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 50);
			ret.AddReturnedValue(ex, "", "GetAllDep", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 51);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>> GetDepFiles(short depid, FileManagement_Parts part, string HostAddress, string UserName, string Password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebFileInfo>> ret = new ReturnedSaveFuncInfoWithValue<List<WebFileInfo>>
		{
			value = new List<WebFileInfo>()
		};
		try
		{
			Dictionary<string, string> obj = new Dictionary<string, string>
			{
				{ "UserName", UserName },
				{ "Password", Password },
				{
					"DepID",
					depid.ToString()
				}
			};
			short num = (short)part;
			obj.Add("FilePart", num.ToString());
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileInfo>>> returnedSaveFuncInfoWithValue = await (HostAddress + "Department/listAll").GetFromApi<List<WebFileInfo>>(token, 3, headers);
			if (!ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDepFiles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 68).HasError)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "", "GetDepFiles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 73);
			ret.AddReturnedValue(ex, "", "GetDepFiles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 74);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DepObjectsDisposed(DepObjectsDisposeModel Model, CancellationToken token)
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
		return await HttpHelpers.PostToApi<ReturnedSaveFuncInfo, DepObjectsDisposeModel>(Model, HttpHelpers.ApiUrl + "Department/DepObjectsDispose", headers, strongDeserialize: false, 15000, 3);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(WebDepartment Dep)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Dep.Status)
			{
				ret.AddReturnedValue(Dep.CheckValidation(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 93);
			}
			if (ret.HasError)
			{
				return ret;
			}
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
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApi(Dep, HttpHelpers.ApiUrl + "Department/Save", headers, strongDeserialize: false, 15000, 3), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 100);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 104);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 105);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SetTafsilGuid(short id, Guid guid)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
		{
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebDepartment> returnedSaveFuncInfoWithValue = await WebDepartmentCompact.GetDeparmentAsync(DepAdmin.UserName, DepAdmin.Password, id);
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					ret.AddError(EnSystem.None, "چنین دپارتمانی یافت نشد", "departmant", "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 120);
				}
				else
				{
					returnedSaveFuncInfoWithValue.value.TafsilGuid = guid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 124);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 127);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 129);
				ret.AddReturnedValue(ex2, "", "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 129);
			}
			finally
			{
				sw.Stop();
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>> GetAsync(short depId, CancellationToken token)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>();
			try
			{
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "Department/WebDepartmentCompact/Get/" + depId).GetFromApi<WebDepartment>(token, 1, new Dictionary<string, string>
				{
					{
						"UserName",
						DepAdmin.UserName
					},
					{
						"Password",
						DepAdmin.Password
					}
				});
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 149);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 154);
				ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 155);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>> GetAsync(string depUser, string password, CancellationToken token)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>>();
			try
			{
				string url = HttpHelpers.ApiUrl + "Department/GetByUserName";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{ "UserName", depUser },
					{ "Password", password }
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebDepartment>(token, 1, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 168);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 173);
				ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 174);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>>> GetByUserAsync(string userName, string password, CancellationToken token)
	{
		using (new JaegerMonitor("GetByUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>>();
			try
			{
				string url = HttpHelpers.ApiUrl + "Department/GetByUserOfDepatmentName";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{ "UserName", userName },
					{ "password", password }
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebUser>(token, 1, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 191);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetByUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 196);
				ret.AddReturnedValue(ex, "", "GetByUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 197);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<ServerCheckLogin>> CheckLoginAsync(string userName, string password, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<ServerCheckLogin> res = new ReturnedSaveFuncInfoWithValue<ServerCheckLogin>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, userName, password).GetToken(token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 207);
			if (res.HasError)
			{
				return res;
			}
			res.value = new ServerCheckLogin
			{
				DepartmentPassword = password,
				DepartmentUserName = userName,
				RestToken = returnedSaveFuncInfoWithValue.value
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 218);
			res.AddReturnedValue(ex, "", "CheckLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DepClient.cs", 219);
		}
		return res;
	}
}
