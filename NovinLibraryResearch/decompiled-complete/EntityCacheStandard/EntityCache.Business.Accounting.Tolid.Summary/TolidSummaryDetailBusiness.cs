using System;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

namespace EntityCache.Business.Accounting.Tolid.Summary;

public class TolidSummaryDetailBusiness : Serializable<TolidSummaryDetailBusiness>, ITolidSummaryDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid ProductGuid { get; set; }

	public string PrdCode { get; set; }

	public string PrdName { get; set; }

	public Guid GroupGuid { get; set; }

	public string GroupName { get; set; }

	public string VahedName { get; set; }

	public Guid DepotGuid { get; set; }

	public string DepotName { get; set; }

	public Guid? TolidGuid { get; set; }

	public decimal RealAmount { get; set; }

	public string RealAmountForDisplay => RealAmount.ToString().Replace(".", "/");

	public decimal AvailableAmount { get; set; }

	public string AvailableAmountForDisplay => AvailableAmount.ToString().Replace(".", "/");

	public decimal ProducedAmount { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public TolidSummaryDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (ProducedAmount > AvailableAmount)
			{
				returnedSaveFuncInfo.AddError("برای کالای".GetEnumerator()?.ToString() + " " + (PrdName ?? "").GetTranslate() + " " + " تعداد تولید شده".GetTranslate() + " (" + $"{ProducedAmount}".GetTranslate() + ")" + " از تعداد قابل عرضه".GetTranslate() + " (" + (AvailableAmountForDisplay ?? "").GetTranslate() + ")" + " بیشتر است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryDetailBusiness.cs", 42);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryDetailBusiness.cs", 46);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tolid\\Summary\\TolidSummaryDetailBusiness.cs", 47);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}
}
