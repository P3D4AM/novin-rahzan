using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tax;

namespace EntityCache.Business.Accounting.Tax;

public class TaxDetailBusiness : Serializable<TaxDetailBusiness>, ITaxDetail, IHasJustGuid, IHasMasterGuid, IHasContext
{
	private bool _isDisposed;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public decimal ProductTaxPercent { get; set; }

	public decimal FeeTaxPercent { get; set; }

	public decimal ProfitTaxPercent { get; set; }

	public string ProductPercentForDisplay => Math.Round((double)ProductTaxPercent, DigitForRound).ToString().Replace(".", "/");

	public string FeePercentForDisplay => Math.Round((double)FeeTaxPercent, DigitForRound).ToString().Replace(".", "/");

	public string ProfitPercentForDisplay => Math.Round((double)ProfitTaxPercent, DigitForRound).ToString().Replace(".", "/");

	public short Ordinal { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public TaxBusiness Master { get; internal set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public byte[] EditImage => ImageResourceManager.EditImage;

	public byte[] DeleteImage => ImageResourceManager.OrderDeleteImage;

	[JsonIgnore]
	public short DigitForRound { get; }

	public event Func<object, EventArgs, Task> OnDisposed;

	internal TaxDetailBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			ChangeStatus = EntityChangeSt.New;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 47);
		}
	}

	public static Task<TaxDetailBusiness> GetFactoryAsync(TaxBusiness master, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (master == null)
		{
			throw new NullReferenceException("master");
		}
		TaxDetailBusiness taxDetailBusiness = new TaxDetailBusiness(master.ContextGuid);
		try
		{
			taxDetailBusiness.Guid = Guid.NewGuid();
			taxDetailBusiness.ContextGuid = master.ContextGuid;
			taxDetailBusiness.Master = master;
			taxDetailBusiness.MasterGuid = master.Guid;
			taxDetailBusiness.Ordinal = master.NextOrdinal;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 62);
		}
		return Task.FromResult(taxDetailBusiness);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Master = null;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 79);
		}
	}

	public void SetOrdinalChange(short ordinal)
	{
		try
		{
			short ordinal2 = Ordinal;
			if (ordinal2 != ordinal && ordinal2 >= 0)
			{
				Ordinal = ordinal;
				(Master?.DetailList?.ToList().FirstOrDefault((TaxDetailBusiness p) => p != null && p.Ordinal == ordinal && p.Guid != Guid))?.SetOrdinalChange(ordinal2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 91);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ProductTaxPercent <= 0m && FeeTaxPercent <= 0m && ProfitTaxPercent <= 0m)
			{
				await res.AddErrorAsync("لطفا یکی از مقادیر درصد مالیات را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 98);
			}
			if (ProductTaxPercent > 100m)
			{
				await res.AddErrorAsync("درصد مالیات کالا باید بین 1 تا 100 درصد باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 99);
			}
			if (FeeTaxPercent > 100m)
			{
				await res.AddErrorAsync("درصد مالیات اجرت باید بین 1 تا 100 درصد باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 100);
			}
			if (ProfitTaxPercent > 100m)
			{
				await res.AddErrorAsync("درصد مالیات سود باید بین 1 تا 100 درصد باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 101);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 105);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxDetailBusiness.cs", 106);
		}
		return res;
	}
}
