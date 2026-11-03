using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Accounting.TaxService;

public class WebTaxCustomInfo
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "api/Tax/";

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public static async Task<List<WebTaxCustomInfo>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOut = 30000)
	{
		List<WebTaxCustomInfo> ret = new List<WebTaxCustomInfo>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebTaxCustomInfo>>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "CustomInfo").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebTaxCustomInfo>>>(token, 3, null, timeOut);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret = returnedSaveFuncInfoWithValue.value.Data.value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxCustomInfo.cs", 26);
		}
		return ret;
	}
}
