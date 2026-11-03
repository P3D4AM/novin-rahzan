using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class DepEnteghalDetBusiness : Serializable<DepEnteghalDetBusiness>, IDepEnteghalDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private Guid prdGuid;

	private ProductCompactBusiness _prd;

	private string _strQt = "";

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; }

	public Guid ProductGuid => prdGuid;

	public Guid MasterGuid { get; set; }

	public Guid DptGuidSrc { get; set; }

	[JsonIgnore]
	public string DptSrcName => DepotBusiness.Get(ContextGuid, DptGuidSrc)?.Name;

	public Guid DptGuidDst { get; set; }

	public short Ordinal { get; set; }

	[JsonIgnore]
	public string DptDstName => DepotBusiness.Get(ContextGuid, DptGuidDst)?.Name;

	public decimal Quantity { get; set; }

	public string QuantityString
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_strQt) && (_strQt.EndsWith("/") || _strQt.EndsWith(".")))
				{
					return _strQt;
				}
				_strQt = Quantity.ToString(3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "QuantityString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 44);
				throw;
			}
			return _strQt;
		}
	}

	public Guid? VahedGuid { get; set; }

	public string Description { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V2Price { get; set; }

	[JsonIgnore]
	public string PrdCode => GetPrdInfo()?.IdProduct;

	[JsonIgnore]
	public string PrdName => GetPrdInfo()?.Name;

	[JsonIgnore]
	public string DateSh => Modified.MiladiToCalture();

	[JsonIgnore]
	public decimal TotalPrice => Math.Round(Price * Quantity, DigitForRound);

	[JsonIgnore]
	public short DigitForRound { get; }

	public string ContextGuid { get; set; }

	public DepEnteghalDetBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 70);
		}
	}

	public ProductCompactBusiness GetPrdInfo()
	{
		return _prd;
	}

	public async Task SetPrdGuidAsync(Guid value, EnSystem system)
	{
		try
		{
			prdGuid = value;
			_prd = await ProductCompactBusiness.GetAsync(ContextGuid, prdGuid, system, null, "", default(CancellationToken), null);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 81);
		}
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (ProductGuid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("هیچ کالایی انتخاب نشده است ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 88);
			}
			if (DptGuidSrc == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("انبار مبدا مشخص نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 89);
			}
			if (DptGuidDst == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("انبار مقصد مشخص نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 90);
			}
			if (DptGuidSrc == DptGuidDst)
			{
				returnedSaveFuncInfo.AddError("انبار مبداء و مقصد یکی انتخاب شده است .".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 91);
			}
			if ((VahedGuid ?? Guid.Empty) != Guid.Empty && V2Qt <= 0m)
			{
				returnedSaveFuncInfo.AddError("مقدار مشخص نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 92);
			}
			if (Quantity <= 0m)
			{
				returnedSaveFuncInfo.AddError("مقدار مشخص نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 93);
			}
			if (Price <= 0m)
			{
				returnedSaveFuncInfo.AddError("مبلغ مشخص نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 94);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 98);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DepEnteghalDetBusiness.cs", 99);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}
}
