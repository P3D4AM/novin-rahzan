using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebTaxUnit
{
	private static string apiUrl = HttpHelpers.ApiUrl + "TaxUnit/";

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public int Code { get; set; }

	public static async Task<List<WebTaxUnit>> GetAllAsync(CancellationToken token = default(CancellationToken))
	{
		List<WebTaxUnit> _list = new List<WebTaxUnit>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxUnit>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetAll").GetFromApi<List<WebTaxUnit>>(token, 3);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return _list;
			}
			_list = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxUnit.cs", 26);
		}
		return _list;
	}
}
