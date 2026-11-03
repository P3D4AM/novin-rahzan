using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class SoftwarePackage
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "softwarepackage/";

	public int Id { get; set; }

	public Guid Guid { get; set; }

	public Guid? BaseGuid { get; set; }

	public string Url { get; set; }

	public string ArticleUrl { get; set; }

	public string DescriptionUrl { get; set; }

	public DateTime CreationDate { get; set; }

	public string CreationDateSh => CreationDate.MiladiToCalture();

	public ENSource Source { get; set; }

	public string SourceForDispaly => Source.GetDisplay();

	public string Version { get; set; }

	public string VersionForDisplay
	{
		get
		{
			string result = "";
			try
			{
				Version version = new Version(Version);
				result = $"{version.Major}.{version.Minor}.{version.Build}";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VersionForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 36);
			}
			return result;
		}
	}

	public long VersionLong => Version.VersionStringToLong();

	public PackageChannel Channel { get; set; }

	public string ChannelForDispaly => Channel.GetDisplay();

	public long Crc { get; set; }

	public string ExecutionFileName { get; set; }

	public List<string> FilesForRemove { get; set; }

	public string FilesForRemoveDisplay
	{
		get
		{
			string text = "";
			int num = 0;
			checked
			{
				if (FilesForRemove != null)
				{
					foreach (string item in FilesForRemove)
					{
						text += item.ToString();
						if (FilesForRemove.Count != num + 1)
						{
							text += ",";
						}
						num++;
					}
				}
				return text;
			}
		}
	}

	public string Slug => SourceForDispaly + "_" + Version;

	public string GetZipFileName()
	{
		return Url?.SplitString("/")?.LastOrDefault() ?? "";
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SoftwarePackage>> GetFromApiAsync(ENSource source, PackageChannel channel = PackageChannel.Stable, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		ReturnedSaveFuncInfoWithValue<SoftwarePackage> result = new ReturnedSaveFuncInfoWithValue<SoftwarePackage>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<SoftwarePackage>> returnedSaveFuncInfoWithValue = await $"{BaseUrl}getlatest?source={(short)source}&channel={checked((short)channel)}".GetFromApi<SoftwarePackage>(token, 3, null, timeOutMilliSeconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				result.AddError(returnedSaveFuncInfoWithValue.Messages, "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 76);
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value.Data == null)
			{
				return null;
			}
			result.value = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 82);
		}
		catch (HttpRequestException exception2)
		{
			result.AddReturnedValue(exception2, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 83);
		}
		catch (TimeoutException exception3)
		{
			result.AddReturnedValue(exception3, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 84);
		}
		catch (Exception exception4)
		{
			result.AddReturnedValue(exception4, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 85);
		}
		return result;
	}

	public static async Task<List<SoftwarePackage>> GetListFromApiAsync(ENSource source, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		try
		{
			List<SoftwarePackage> result = new List<SoftwarePackage>();
			ReturnedSaveFuncInfoWithValue<WebResponse<List<SoftwarePackage>>> returnedSaveFuncInfoWithValue = await $"{BaseUrl}getallbysource?source={(short)source}".GetFromApi<List<SoftwarePackage>>(token, 3, null, timeOutMilliSeconds);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue.value.Data;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 99);
			throw;
		}
	}

	public static async Task<List<SoftwarePackage>> GetListFromApiAsync(CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<SoftwarePackage>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "getall").GetFromApi<List<SoftwarePackage>>(token, 3, null, timeOutMilliSeconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 113);
			throw;
		}
	}

	public static async Task DeleteAsync(int id, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		_ = 1;
		try
		{
			using HttpClient clientHttp = new HttpClient();
			clientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			HttpResponseMessage obj = await clientHttp.GetAsync($"{BaseUrl}delete?row={id}", token);
			obj.EnsureSuccessStatusCode();
			await obj.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 127);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(SoftwarePackage obj)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (obj.Guid == Guid.Empty)
			{
				obj.Guid = Guid.NewGuid();
			}
			res = await HttpHelpers.PostToApi(obj, BaseUrl + "save", null, strongDeserialize: false, 15000, 3);
			res.AddReturnedValue(res, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 136);
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 141);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 142);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SoftwarePackage>> GetLastAsync(string currentVersion, ENSource source, PackageChannel channel, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<SoftwarePackage> result = new ReturnedSaveFuncInfoWithValue<SoftwarePackage>();
		try
		{
			Dictionary<string, string> obj = new Dictionary<string, string> { { "currentVersion", currentVersion } };
			short num = (short)source;
			obj.Add("source", num.ToString());
			obj.Add("channel", checked((short)channel).ToString());
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<SoftwarePackage>> returnedSaveFuncInfoWithValue = await (BaseUrl + "GetLast").GetFromApi<SoftwarePackage>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 158);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 164);
			result.AddReturnedValue(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 165);
		}
		return result;
	}

	public static List<SoftwarePackageModel> ParseModel(List<SoftwarePackage> list)
	{
		try
		{
			if (list == null || !list.Any())
			{
				return null;
			}
			list = list.OrderByDescending((SoftwarePackage x) => x.CreationDate).ToList();
			List<SoftwarePackageModel> list2 = new List<SoftwarePackageModel>();
			foreach (SoftwarePackage item in list)
			{
				if (item != null)
				{
					list2.Add(new SoftwarePackageModel
					{
						Version = item.VersionForDisplay,
						DateM = item.CreationDate,
						Options = item.DescriptionUrl?.Split(new char[1] { '-' })?.ToList()
					});
				}
			}
			return list2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SoftwarePackage.cs", 192);
			throw;
		}
	}
}
