using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Accounting;

public class WebTaxErrors
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "api/Tax/";

	public string Code { get; set; }

	public string Title { get; set; } = "";


	public string Describtion { get; set; } = "";


	public string ArticleUrl { get; set; } = "";


	public string VideoUrl { get; set; } = "";


	public string MetaData { get; set; } = "";


	public static async Task<List<WebTaxErrors>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOut = 30000)
	{
		List<WebTaxErrors> ret = new List<WebTaxErrors>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxErrors>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "ErrorList").GetFromApi<List<WebTaxErrors>>(token, 3, null, timeOut);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxErrors.cs", 29);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<bool>> CheckPEMKeysMatch(string userAsJson, string privatPEMKey, string publicPEMKey, CancellationToken token = default(CancellationToken), int timeOut = 30000)
	{
		ReturnedSaveFuncInfoWithValue<bool> response = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"privatPEMKey",
					Json.ToStringJson(privatPEMKey)
				},
				{
					"publicPEMKey",
					Json.ToStringJson(publicPEMKey)
				},
				{ "TaxUser", userAsJson }
			};
			response = (await (BaseUrl + "checkpemkeysmatch").GetFromApi<ReturnedSaveFuncInfoWithValue<bool>>(token, 3, headers, timeOut)).value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPEMKeysMatch", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxErrors.cs", 51);
		}
		return response;
	}
}
