using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Accounting.TaxService;

public class TaxOrder
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "taxorder/";

	public string ClientId { get; set; }

	public int OrderId { get; set; }

	public Guid OrderGuid { get; set; }

	public string Message { get; set; }

	public string RefrencedId { get; set; }

	public string UId { get; set; }

	public EnTaxResult Status { get; set; }

	public string StatusDisplay => Status.GetDisplay().GetTranslate();

	public string InternaleSerial { get; set; }

	public DateTime InsertDate { get; set; }

	public string InsertDateSh => InsertDate.MiladiToCalture();

	public static async Task<List<TaxOrder>> GetByDepIdAsync(short depId, CancellationToken token = default(CancellationToken), int timeOut = 30000)
	{
		List<TaxOrder> ret = new List<TaxOrder>();
		Dictionary<string, string> headers = new Dictionary<string, string> { 
		{
			"DepId",
			depId.ToString()
		} };
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<TaxOrder>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "getbydepid").GetFromApi<List<TaxOrder>>(token, 3, headers, timeOut);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				ret = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByDepIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\TaxOrder.cs", 41);
		}
		return ret;
	}
}
