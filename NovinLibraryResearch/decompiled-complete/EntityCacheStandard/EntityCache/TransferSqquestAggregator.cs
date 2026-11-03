using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using EntityCache.Business;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;

namespace EntityCache;

public class TransferSqquestAggregator : IHasContext
{
	public string ContextGuid { get; set; }

	public ISqlServerTableInformation SqlServerTableInformation { get; set; }

	public ReturnedSaveFuncInfo ReturnedResult { get; } = new ReturnedSaveFuncInfo();


	public BufferBlock<TransferReport> MessageBuffer { get; } = new BufferBlock<TransferReport>();


	public TransferSqquestObject CusTitle { get; } = new TransferSqquestObject();


	public TransferSqquestObject CusGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject Customers { get; } = new TransferSqquestObject();


	public TransferSqquestObject PrdVahed { get; } = new TransferSqquestObject();


	public TransferSqquestObject PrdGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject Product { get; } = new TransferSqquestObject();


	public TransferSqquestObject AnbarGardani { get; } = new TransferSqquestObject();


	public TransferSqquestObject HesabType { get; } = new TransferSqquestObject();


	public TransferSqquestObject HesabGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject DiscountCoupon { get; } = new TransferSqquestObject();


	public TransferSqquestObject Barcode { get; } = new TransferSqquestObject();


	public TransferSqquestObject Serial { get; } = new TransferSqquestObject();


	public TransferSqquestObject Kol { get; } = new TransferSqquestObject();


	public TransferSqquestObject TafsilGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject Moein { get; } = new TransferSqquestObject();


	public TransferSqquestObject Tafsil { get; } = new TransferSqquestObject();


	public TransferSqquestObject Cashier { get; } = new TransferSqquestObject();


	public TransferSqquestObject ServiceGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject ServiceVahed { get; } = new TransferSqquestObject();


	public TransferSqquestObject Service { get; } = new TransferSqquestObject();


	public TransferSqquestObject Depot { get; } = new TransferSqquestObject();


	public TransferSqquestObject ProductAttribute { get; } = new TransferSqquestObject();


	public TransferSqquestObject User { get; } = new TransferSqquestObject();


	public TransferSqquestObject Decimal { get; } = new TransferSqquestObject();


	public TransferSqquestObject DecimalUpdate { get; } = new TransferSqquestObject();


	public TransferSqquestObject ShortCut { get; } = new TransferSqquestObject();


	public TransferSqquestObject Lottery { get; } = new TransferSqquestObject();


	public TransferSqquestObject LotteryScore { get; } = new TransferSqquestObject();


	public TransferSqquestObject CustomerMotefareghe { get; } = new TransferSqquestObject();


	public TransferSqquestObject Vam { get; } = new TransferSqquestObject();


	public TransferSqquestObject CustomerCard { get; } = new TransferSqquestObject();


	public TransferSqquestObject ProductList { get; } = new TransferSqquestObject();


	public TransferSqquestObject TafsilList { get; } = new TransferSqquestObject();


	public TransferSqquestObject DasteCheck { get; } = new TransferSqquestObject();


	public TransferSqquestObject MoeinTafsliGroupRelation { get; } = new TransferSqquestObject();


	public TransferSqquestObject Desc { get; } = new TransferSqquestObject();


	public TransferSqquestObject AghsatM { get; } = new TransferSqquestObject();


	public TransferSqquestObject AghsatMGroup { get; } = new TransferSqquestObject();


	public TransferSqquestObject AghsatSh { get; } = new TransferSqquestObject();


	public TransferSqquestObject CheckedSave { get; } = new TransferSqquestObject();


	public TransferSqquestObject Resid { get; } = new TransferSqquestObject();


	public TransferSqquestObject DepotTransfer { get; } = new TransferSqquestObject();


	public TransferSqquestObject Log { get; } = new TransferSqquestObject();


	public TransferSqquestObject Settings { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderService { get; } = new TransferSqquestObject();


	public TransferSqquestObject VisitorArea { get; } = new TransferSqquestObject();


	public TransferSqquestObject Wage { get; } = new TransferSqquestObject();


	public TransferSqquestObject FileInfo { get; } = new TransferSqquestObject();


	public TransferSqquestObject CustomerRole { get; } = new TransferSqquestObject();


	public TransferSqquestObject ReportGenerator { get; } = new TransferSqquestObject();


	public TransferSqquestObject SanadStatusReport { get; } = new TransferSqquestObject();


	public TransferSqquestObject TableRelationShip { get; } = new TransferSqquestObject();


	public TransferSqquestObject Sanad { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderSell { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderBuy { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderBackBuy { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderBackSell { get; } = new TransferSqquestObject();


	public TransferSqquestObject OrderZayeat { get; } = new TransferSqquestObject();


	public TransferSqquestObject PishFactor { get; } = new TransferSqquestObject();


	public TransferSqquestObject Reception { get; } = new TransferSqquestObject();


	public TransferSqquestObject Pardakht { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeMOperation { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeShOperation { get; } = new TransferSqquestObject();


	public TransferSqquestObject Tolid { get; } = new TransferSqquestObject();


	public TransferSqquestObject TolidFormula { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeShPardakhtani { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeShEntezami { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeMMojod { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeMEntezami { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeMKharjShode { get; } = new TransferSqquestObject();


	public TransferSqquestObject ChequeMVagozar { get; } = new TransferSqquestObject();


	public TransferSqquestObject BankSuggest { get; } = new TransferSqquestObject();


	public TransferSqquestObject TaxClass { get; } = new TransferSqquestObject();


	public TransferSqquestObject ProductPriceLevel { get; } = new TransferSqquestObject();


	public TransferSqquestObject IntroductionMethod { get; } = new TransferSqquestObject();


	public TransferSqquestAggregator(string contextGuid, ISqlServerTableInformation sqlServerTableInformation)
	{
		ContextGuid = contextGuid;
		SqlServerTableInformation = sqlServerTableInformation;
	}

	private void AddData(ReturnedState state, string groupName, string desc)
	{
		try
		{
			TransferReport item = new TransferReport
			{
				State = state,
				Description = desc,
				Date = DateTime.Now,
				GroupName = groupName
			};
			MessageBuffer.Post(item);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\TransferSqquestAggregator.cs", 116);
		}
	}

	public void AddProductError(string desc)
	{
		AddData(ReturnedState.Error, "کالاها و خدمات", desc);
	}

	public void AddProductInformation(string desc)
	{
		AddData(ReturnedState.Information, "کالاها و خدمات", desc);
	}

	public void AddProductSuccess(string desc)
	{
		AddData(ReturnedState.Success, "کالاها و خدمات", desc);
	}

	public void AddOtherError(string desc)
	{
		AddData(ReturnedState.Error, "سایر", desc);
	}

	public void AddOtherInformation(string desc)
	{
		AddData(ReturnedState.Information, "سایر", desc);
	}

	public void AddOtherSuccess(string desc)
	{
		AddData(ReturnedState.Success, "سایر", desc);
	}

	public void AddAccountError(string desc)
	{
		AddData(ReturnedState.Error, "حساب ها", desc);
	}

	public void AddAccountInformation(string desc)
	{
		AddData(ReturnedState.Information, "حساب ها", desc);
	}

	public void AddAccountSuccess(string desc)
	{
		AddData(ReturnedState.Success, "حساب ها", desc);
	}

	public void AddCustomerError(string desc)
	{
		AddData(ReturnedState.Error, "اشخاص", desc);
	}

	public void AddCustomerInformation(string desc)
	{
		AddData(ReturnedState.Information, "اشخاص", desc);
	}

	public void AddCustomerSuccess(string desc)
	{
		AddData(ReturnedState.Success, "اشخاص", desc);
	}

	public async Task<TransferSettings> GetDatabaseHistoryAsync()
	{
		TransferSettings setting = null;
		try
		{
			setting = (await LocalSettingBusiness.GetTransferSettingAsync(ContextGuid)) ?? new TransferSettings();
			bool transferLevel1 = (await LocalSettingBusiness.GetAsync(ContextGuid, "transferDataFromV5ToV6Key"))?.ParseToBoolean() ?? false;
			bool transferLevel2 = (await LocalSettingBusiness.GetAsync(ContextGuid, "TransferLevel2"))?.ParseToBoolean() ?? false;
			bool transferLevel3 = false;
			bool transferLevel4 = false;
			bool transferLevel5 = false;
			bool transferLevel6 = false;
			bool transferTolid = false;
			string[] tableNames = new string[6] { "BankBankEnteghal", "BankBardashteNaghd", "BankKasreKarmozd", "BankVarizeNaghd", "SandoghSandoghEnteghal", "BankVarizeSood" };
			if (!((await SqlServerTableInformation.GetAllAsync(tableNames))?.Any() ?? false))
			{
				transferLevel3 = true;
			}
			if (transferLevel1)
			{
				setting.CusTitle = true;
				setting.CusGroup = true;
				setting.Customers = true;
				setting.PrdVahed = true;
				setting.PrdGroup = true;
				setting.Product = true;
				setting.AnbarGardani = true;
				setting.HesabType = true;
				setting.HesabGroup = true;
				setting.Kol = true;
				setting.TafsilGroup = true;
				setting.Moein = true;
				setting.Tafsil = true;
				setting.ServiceGroup = true;
				setting.ServiceVahed = true;
				setting.Service = true;
				setting.Depot = true;
				setting.User = true;
				setting.Decimal = true;
				setting.DecimalUpdate = true;
				setting.Lottery = true;
				setting.LotteryScore = true;
				setting.CustomerMotefareghe = true;
				setting.Vam = true;
				setting.CustomerCard = true;
				setting.ProductList = true;
				setting.TafsilList = true;
				setting.DasteCheck = true;
				setting.MoeinTafsliGroupRelation = true;
				setting.Desc = true;
				setting.AghsatM = true;
				setting.AghsatMGroup = true;
				setting.AghsatSh = true;
				setting.CheckedSave = true;
				setting.Resid = true;
				setting.DepotTransfer = true;
				setting.Settings = true;
				setting.OrderService = true;
			}
			if (transferLevel2)
			{
				setting.VisitorArea = true;
				setting.TableRelationShip = true;
			}
			if (transferLevel3)
			{
				setting.Sanad = true;
			}
			tableNames = new string[7] { "CheckeVosol", "CheckMBargasht", "CheckSHBargasht", "CheckSHEbtal", "CheckSHPardakht", "BankCheck", "SandoghCheck" };
			if (!((await SqlServerTableInformation.GetAllAsync(tableNames))?.Any() ?? false))
			{
				transferLevel4 = true;
			}
			if (transferLevel4)
			{
				setting.ChequeMOperation = true;
				setting.ChequeShOperation = true;
			}
			tableNames = new string[3] { "Sanad", "SanadDetail", "VSanadDetail" };
			if (!((await SqlServerTableInformation.GetAllAsync(tableNames))?.Any() ?? false))
			{
				transferLevel5 = true;
			}
			if (transferLevel5)
			{
				setting.Sanads = true;
			}
			tableNames = new string[2] { "Serial", "Serial_Mojood" };
			if (!((await SqlServerTableInformation.GetAllAsync(tableNames))?.Any() ?? false))
			{
				transferLevel6 = true;
			}
			if (transferLevel6)
			{
				setting.PrdSerials = true;
			}
			tableNames = new string[8] { "Tolid", "Tolid_Export", "Tolid_formula", "Tolid_Formula_Export", "Tolid_Formula_Hazine", "Tolid_Formula_Mavad", "Tolid_Hazine", "Tolid_Mavad" };
			if (!((await SqlServerTableInformation.GetAllAsync(tableNames))?.Any() ?? false))
			{
				transferTolid = true;
			}
			if (transferTolid)
			{
				setting.Tolid = true;
				setting.TolidFormula = true;
			}
			if (setting.AghsatM && !setting.AghsatMGroup)
			{
				setting.AghsatMGroup = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDatabaseHistoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\TransferSqquestAggregator.cs", 264);
		}
		return setting;
	}

	public async Task<bool> CheckTransferAsync()
	{
		try
		{
			TransferSettings transferSettings = await GetDatabaseHistoryAsync();
			return transferSettings.CusTitle && transferSettings.CusGroup && transferSettings.Customers && transferSettings.PrdVahed && transferSettings.PrdGroup && transferSettings.Product && transferSettings.AnbarGardani && transferSettings.HesabType && transferSettings.HesabGroup && transferSettings.Kol && transferSettings.TafsilGroup && transferSettings.Moein && transferSettings.Tafsil && transferSettings.ServiceGroup && transferSettings.ServiceVahed && transferSettings.Service && transferSettings.Depot && transferSettings.User && transferSettings.Decimal && transferSettings.DecimalUpdate && transferSettings.Lottery && transferSettings.LotteryScore && transferSettings.CustomerMotefareghe && transferSettings.Vam && transferSettings.CustomerCard && transferSettings.ProductList && transferSettings.TafsilList && transferSettings.DasteCheck && transferSettings.MoeinTafsliGroupRelation && transferSettings.Desc && transferSettings.AghsatM && transferSettings.AghsatMGroup && transferSettings.AghsatSh && transferSettings.CheckedSave && transferSettings.Resid && transferSettings.DepotTransfer && transferSettings.Settings && transferSettings.OrderService && transferSettings.TableRelationShip && transferSettings.VisitorArea && transferSettings.Sanad && transferSettings.OrderSell && transferSettings.OrderBuy && transferSettings.OrderBackSell && transferSettings.OrderBackBuy && transferSettings.OrderZayeat && transferSettings.PishFactor && transferSettings.Reception && transferSettings.Pardakht && transferSettings.ChequeMOperation && transferSettings.ChequeShOperation && transferSettings.Sanads && transferSettings.PrdSerials && transferSettings.Tolid && transferSettings.TolidFormula;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTransferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\TransferSqquestAggregator.cs", 331);
			throw;
		}
	}
}
