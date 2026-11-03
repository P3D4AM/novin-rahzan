using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Department;

public class WebVersionUsageUsers
{
	private static string BaseUrl = HttpHelpers.ApiUrl + "VersionUsage/";

	public short Source { get; set; }

	public long LKSerial { get; set; }

	public Guid TafsilGuid { get; set; }

	public string Version { get; set; }

	public short Rank { get; set; }

	public string Name { get; set; }

	public static async Task<List<WebVersionUsageUsers>> GetListFromApiAsync(string version, ENSource source, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		List<WebVersionUsageUsers> result = new List<WebVersionUsageUsers>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "version", version },
				{
					"source",
					$"{(short)source}"
				}
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebVersionUsageUsers>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "VersionUsageUsers").GetFromApi<List<WebVersionUsageUsers>>(token, 3, headers, timeOutMilliSeconds);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				return returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVersionUsageUsers.cs", 31);
		}
		return result;
	}
}
