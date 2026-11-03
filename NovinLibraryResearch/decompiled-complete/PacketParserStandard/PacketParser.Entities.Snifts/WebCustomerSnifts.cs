using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Snifts;

public class WebCustomerSnifts
{
	private static string apiUrl = HttpHelpers.ApiUrl + "ActivationLK/";

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Address { get; set; }

	public string Title { get; set; }

	public string Group { get; set; }

	public List<WebTafsilNumbers> TafsilNumberList { get; set; }

	public static async Task<List<WebCustomerSnifts>> GetFromApi_By_PaginationAsync(int page, CancellationToken token = default(CancellationToken))
	{
		List<WebCustomerSnifts> response = null;
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"page",
				page.ToString()
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebCustomerSnifts>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetByPagination").GetFromApi<List<WebCustomerSnifts>>(token, 3, headers);
			if (returnedSaveFuncInfoWithValue != null && !returnedSaveFuncInfoWithValue.HasError)
			{
				response = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApi_By_PaginationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Snifts\\WebCustomerSnifts.cs", 32);
		}
		return response;
	}
}
