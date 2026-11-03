using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Accounting.TaxService;

public class WebTaxCurrency
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "api/Tax/";

	public Guid Guid { get; set; }

	public string CountryName { get; set; }

	public string Currency { get; set; }

	public string AlphabeticCode { get; set; }

	public string NumericCode { get; set; }

	public static async Task<List<WebTaxCurrency>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOut = 30000)
	{
		List<WebTaxCurrency> ret = new List<WebTaxCurrency>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebTaxCurrency>>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "CurrencyList").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebTaxCurrency>>>(token, 3, null, timeOut);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret = returnedSaveFuncInfoWithValue.value.Data.value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxCurrency.cs", 28);
		}
		return ret;
	}
}
