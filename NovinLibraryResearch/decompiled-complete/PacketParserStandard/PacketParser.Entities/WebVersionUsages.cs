using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebVersionUsages
{
	private static string apiurl = HttpHelpers.ApiUrl + "VersionUsage/";

	public ENSource Source { get; set; }

	public string SourceForDisplay => Source.GetDisplay();

	public long LkSerial { get; set; }

	public string Version { get; set; }

	public string Ip { get; set; }

	public DateTime DateM { get; set; }

	public string DateSH => DateM.MiladiToCalture();

	[JsonIgnore]
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
				WebErrorLog.Instance.StartLog(ex, "", "VersionForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 34);
			}
			return result;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendAsync(long lkSerial, ENSource source, string version)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			WebVersionUsages item = new WebVersionUsages
			{
				Source = source,
				Version = version,
				Ip = GetLocalIpAddress(),
				LkSerial = lkSerial
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await HttpHelpers.PostToApi(item, apiurl + "Save", null, strongDeserialize: false, 15000, 3), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 51);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 55);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 56);
		}
		return res;
	}

	public static async Task<List<WebVersionUsages>> GetListFromApiAsync(long lkSerial, CancellationToken token = default(CancellationToken), int timeout = 15000)
	{
		List<WebVersionUsages> res = new List<WebVersionUsages>();
		try
		{
			res = (await $"{apiurl}VersionUsageByLK/{lkSerial}".GetFromApi<List<WebVersionUsages>>(token, 3, null, timeout)).value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 68);
		}
		return res;
	}

	public static async Task<WebVersionUsages> GetLastAsync(Guid tafsilGuid, ENSource source, CancellationToken token = default(CancellationToken), int timeout = 15000)
	{
		WebVersionUsages res = new WebVersionUsages();
		try
		{
			Dictionary<string, string> obj = new Dictionary<string, string> { 
			{
				"TafsilGuid",
				tafsilGuid.ToString()
			} };
			short num = (short)source;
			obj.Add("Source", num.ToString());
			Dictionary<string, string> headers = obj;
			res = (await (apiurl + "LastVersionBySource").GetFromApi<WebVersionUsages>(token, 3, headers, timeout)).value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 84);
		}
		return res;
	}

	private static string GetLocalIpAddress()
	{
		string text = "";
		try
		{
			text = GetLocalIpAddressFromCheckIp();
			if (string.IsNullOrEmpty(text))
			{
				text = GetLocalIpAddressFromIfConfig();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLocalIpAddress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsages.cs", 96);
		}
		return text;
	}

	private static string GetLocalIpAddressFromIfConfig()
	{
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");
			obj.UserAgent = "curl";
			obj.Method = "GET";
			string text;
			using (WebResponse webResponse = obj.GetResponse())
			{
				using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
				text = streamReader.ReadToEnd();
			}
			return text.Replace("\n", "");
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static string GetLocalIpAddressFromCheckIp()
	{
		checked
		{
			try
			{
				string text = "";
				using (WebResponse webResponse = WebRequest.Create("http://checkip.dyndns.org/").GetResponse())
				{
					using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
					text = streamReader.ReadToEnd();
				}
				int num = text.IndexOf("Address: ") + 9;
				int num2 = text.LastIndexOf("</body>");
				return text.Substring(num, num2 - num);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
