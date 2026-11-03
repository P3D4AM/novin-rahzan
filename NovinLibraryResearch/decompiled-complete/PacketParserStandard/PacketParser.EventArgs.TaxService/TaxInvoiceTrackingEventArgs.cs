using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.EventArgs.TaxService;

public class TaxInvoiceTrackingEventArgs
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Tax/";

	public string ReferenceNumber { get; set; }

	public string Uid { get; set; }

	public string Status { get; set; }

	public InvoiceTrackingModel Data { get; set; }

	public string PacketType { get; set; }

	public string FiscalId { get; set; }

	public Guid OrderGuid { get; set; }

	public EnTaxResult TaxApiResponse { get; set; }

	[Obsolete("please use TaxApiResponse instead!")]
	public EnTaxResult Result
	{
		get
		{
			if (!(Data?.Error?.Any()).GetValueOrDefault())
			{
				return EnTaxResult.Accepted;
			}
			return EnTaxResult.Failed;
		}
	}

	public string ErrorCode
	{
		get
		{
			if (!(Data?.Error?.Any()).GetValueOrDefault())
			{
				return "";
			}
			return string.Join("\r\n", Data?.Error?.Select((InvoiceTrackingResultModel q) => q.Code)?.ToList());
		}
	}

	public string ErrorMessage
	{
		get
		{
			if (!(Data?.Error?.Any()).GetValueOrDefault())
			{
				return "";
			}
			return string.Join("\r\n", Data?.Error?.Select((InvoiceTrackingResultModel q) => q.Message)?.ToList());
		}
	}

	public string TaxApiRsponse { get; set; }

	public static async Task<ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>>> GetFromApiAsync(ConnectionEventArgs<InvoiceTrackingEventArgs> connection, string clientID, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>> response = new ReturnedSaveFuncInfoWithValue<List<TaxInvoiceTrackingEventArgs>>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"depID",
					connection.Connection.DepId.ToString()
				},
				{
					"userID",
					connection.Connection.UserId.ToString()
				},
				{
					"refID",
					connection.ConnectionData.First().RefrenceId
				},
				{
					"orderGuid",
					connection.ConnectionData.First().OrderGuid.ToString()
				},
				{ "clientID", clientID },
				{
					"uID",
					connection.ConnectionData.First().Uid
				}
			};
			response.value = (await (apiUrl + "ordertracking").GetFromApi<List<TaxInvoiceTrackingEventArgs>>(token, 3, headers)).value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\TaxService\\LoginEventArgs.cs", 73);
			response.AddError(ex.Message, "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\TaxService\\LoginEventArgs.cs", 74);
		}
		return response;
	}
}
