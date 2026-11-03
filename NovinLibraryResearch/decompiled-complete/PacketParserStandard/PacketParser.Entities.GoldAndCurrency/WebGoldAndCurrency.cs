using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.GoldAndCurrency;

public class WebGoldAndCurrency
{
	private static string apiUrl = HttpHelpers.ApiUrl + "Gold/";

	public string Namad { get; set; }

	public string Sign { get; set; }

	public string PersianName { get; set; }

	public string Icon { get; set; }

	public List<WebGoldAndCurrencyDetail> Details { get; set; }

	public WebGoldAndCurrency()
	{
		Details = new List<WebGoldAndCurrencyDetail>();
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>> GetAllAsync(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>> result = new ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebGoldAndCurrencyReport>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetRankedData").GetFromApi<List<WebGoldAndCurrencyReport>>(token, 3);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 27);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 33);
			result.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 34);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyDetail>>> GetDatabetweenDatesAsync(string en, DateTime startDate, DateTime endDate, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyDetail>> result = new ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyDetail>>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "EnumTalaVaArzNumber", en },
				{
					"startDate",
					startDate.ToString()
				},
				{
					"endDate",
					endDate.ToString()
				}
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebGoldAndCurrencyDetail>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetPriceBetweenDates").GetFromApi<List<WebGoldAndCurrencyDetail>>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDatabetweenDatesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 50);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDatabetweenDatesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 56);
			result.AddReturnedValue(ex, "", "GetDatabetweenDatesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\GoldAndCurrency\\WebGoldAndCurrency.cs", 57);
		}
		return result;
	}
}
