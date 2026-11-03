using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Accounting.TaxService;

public class WebTaxStuff
{
	public long ID { get; set; }

	public string Type { get; set; }

	public string TypeSummery
	{
		get
		{
			if (Type == "شناسه عمومی وارداتی(کالا)")
			{
				return "عمومی وارداتی";
			}
			if (Type == "شناسه اختصاصی(خدمت)")
			{
				return "اختصاصی";
			}
			if (Type == "شناسه اختصاصی وارداتی(کالا)")
			{
				return "اختصاصی وارداتی";
			}
			if (Type == "شناسه عمومی(خدمت)")
			{
				return "عمومی";
			}
			if (Type == "شناسه عمومی تولید داخل(کالا)")
			{
				return "عمومی تولید داخل";
			}
			if (Type == "شناسه اختصاصی تولید داخل(کالا)")
			{
				return "اختصاصی تولید داخل";
			}
			return "-";
		}
	}

	public string Date { get; set; }

	public string SpecialOrGeneral { get; set; }

	public string TaxableOrFree { get; set; }

	public float Vat { get; set; }

	public float VatCustomPurposes { get; set; }

	public string DescriptionOfID { get; set; }

	public EnTaxType TaxType
	{
		get
		{
			if (!(TaxableOrFree == "مشمول"))
			{
				return EnTaxType.NotContains;
			}
			return EnTaxType.Contains;
		}
	}

	public static async Task<List<WebTaxStuff>> GetAllFromApiAsync(string name, int? page = null, int? count = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			new ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxStuff>>>();
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxStuff>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<List<WebTaxStuff>, TaxStuffRequest>(new TaxStuffRequest
			{
				Count = count,
				Name = name,
				Page = page,
				IsWebSite = false
			}, HttpHelpers.ApiUrl + "TaxStuffList", null, null, 15000, 5, token);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			if (!(returnedSaveFuncInfoWithValue?.value?.Data?.Any()).GetValueOrDefault())
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxStuff.cs", 55);
			throw;
		}
	}
}
