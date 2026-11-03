using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Global;
using EntityCache.Mapping.BusinessMappers.Sanad;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OrderMapper : IItemAsyncMapper<OrderBusiness, WebOrder>, IItemAsyncMapper<OrderBusiness, OrderBusiness>, IItemAsyncMapper<WebOrder, OrderBusiness>, IItemAsyncMapper<WebOrderReport, OrderReportBusiness>, IItemAsyncMapper<OrderReportBusiness, WebOrderReport>
{
	public static OrderMapper Instance { get; private set; } = new OrderMapper();


	public async Task<WebOrder> MapAsync(string contextGuid, OrderBusiness source, WebOrder destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				line = 1;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebOrder();
			}
			line = 3;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.Source = source.Source;
			line = 4;
			destination.UserAccGuid = source.UserAccGuid;
			destination.B1TafsilGuid = source.B1TafsilGuid;
			destination.B2TafsilGuid = source.B2TafsilGuid;
			destination.B3TafsilGuid = source.B3TafsilGuid;
			destination.B4TafsilGuid = source.B4TafsilGuid;
			line = 5;
			destination.MoeinGuid = source.MoeinGuid;
			WebOrder webOrder = destination;
			webOrder.Id2 = await source.GetId2Async(EnSystem.Mapper, tranName, token);
			destination.B1MoeinGuid = source.B1MoeinGuid;
			line = 6;
			destination.B2MoeinGuid = source.B2MoeinGuid;
			destination.B3MoeinGuid = source.B3MoeinGuid;
			destination.B4MoeinGuid = source.B4MoeinGuid;
			line = 7;
			destination.AvarezTafsilGuid = source.AvarezTafsilGuid;
			destination.AvarezMoeinGuid = source.AvarezMoeinGuid;
			destination.TaxTafsilGuid = source.TaxTafsilGuid;
			line = 8;
			destination.TaxMoeinGuid = source.TaxMoeinGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			line = 9;
			destination.AghsatGuid = source.AghsatGuid;
			destination.AndroidIme = source.AndroidIme;
			line = 10;
			destination.AndroidImeLong = source.AndroidImeLong;
			destination.Avarez = source.Avarez;
			destination.BarnameNo = source.BarnameNo;
			line = 11;
			destination.BazaryabFullName = source.BazaryabFullName;
			destination.CpuSerial = source.CpuSerial;
			line = 12;
			destination.Date = source.GetDate();
			destination.DateSabt = source.DateSabt;
			destination.DateUtc = destination.Date.ToUniversalTime();
			destination.Description = source.Description;
			line = 13;
			destination.Discount = source.Discount;
			destination.FactorType = source.FactorType;
			destination.DailyFactorNo = source.DailyFactorNo;
			line = 14;
			destination.HazineTafsilGuid1 = source.HazineTafsilGuid1;
			destination.HazineTafsilGuid2 = source.HazineTafsilGuid2;
			destination.HazineTafsilGuid3 = source.HazineTafsilGuid3;
			line = 15;
			destination.HazineTafsilGuid4 = source.HazineTafsilGuid4;
			destination.HazineTafsilGuid5 = source.HazineTafsilGuid5;
			destination.HazineMoeinGuid1 = source.HazineMoeinGuid1;
			line = 16;
			destination.HazineMoeinGuid2 = source.HazineMoeinGuid2;
			destination.HazineMoeinGuid3 = source.HazineMoeinGuid3;
			destination.HazineMoeinGuid4 = source.HazineMoeinGuid4;
			line = 17;
			destination.HazineMoeinGuid5 = source.HazineMoeinGuid5;
			destination.HddSerial = source.HddSerial;
			destination.TafsilGuid = source.GetTafsilGuid();
			line = 18;
			destination.Ip = source.Ip;
			destination.Keraye = source.Keraye;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			line = 19;
			destination.FareTafsilGuid = source.FareTafsilGuid;
			destination.LkSerial = source.LkSerial;
			line = 20;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.PrintCount = source.PrintCount;
			destination.PrintDate = source.PrintDate;
			line = 21;
			destination.ReceptionId = source.ReceptionId;
			destination.HostRefrenced = source.HostRefrenced;
			line = 22;
			webOrder = destination;
			webOrder.SanadNo = await source.GetSanadNumberAsync(token, tranName);
			destination.SanadSt = source.SanadSt;
			destination.TasvieDay = source.TasvieDay;
			line = 23;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			line = 24;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.IsTemp = source.GetIsTemp();
			destination.CustName = source.CustName;
			line = 25;
			destination.CustFamily = source.CustFamily;
			destination.CustMobile = source.CustMobile;
			destination.CustAddress = source.CustAddress;
			line = 26;
			destination.CustZipCode = source.CustZipCode;
			destination.CustTell = source.CustTell;
			destination.PaymentMethod = source.PaymentMethod;
			line = 27;
			destination.TotalDiscount = source.TotalDiscount;
			destination.TotalHazine = source.TotalHazine;
			destination.TotalHazine1 = source.TotalHazine1;
			line = 28;
			destination.TotalHazine2 = source.TotalHazine2;
			destination.TotalHazine3 = source.TotalHazine3;
			destination.TotalHazine4 = source.TotalHazine4;
			line = 29;
			destination.TotalHazine5 = source.TotalHazine5;
			destination.AghlamSum = source.SumKala + source.SumKalaKhadamat;
			destination.SellerFactorNo = source.SellerFactorNo;
			destination.ChangeStatus = source.ChangeStatus;
			destination.TransferDate = source.TransferDate;
			destination.ProductMoeinGuid = source.ProductMoeinGuid;
			destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
			destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
			destination.AttachmentNo = source.AttachmentNo;
			destination.StatusId = source.StatusId;
			destination.Tax = source.Tax;
			destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
			line = 30;
			webOrder = destination;
			webOrder.LinkedFiles = await source.GetLinkedFilesAsync(EnSystem.Mapper, token);
			line = 31;
			if (source.ListKala != null)
			{
				TaxUnits setting2 = await LocalSettingBusiness.GetTaxUnitsAsync(contextGuid);
				webOrder = destination;
				webOrder.ListKala = await OrderDetMapper.GetInstance(setting2).MapListAsync(contextGuid, source.ListKala, new List<WebOrderDetail>(), tranName, token);
			}
			line = 33;
			SanadBusiness connectedSanad = source.GetConnectedSanad(EnSystem.Mapper);
			if ((connectedSanad?.ListSanad_Det?.Any()).GetValueOrDefault())
			{
				webOrder = destination;
				webOrder.ListSanad = await SanadDetMapper.Instance.MapListAsync(contextGuid, connectedSanad.ListSanad_Det, new List<WebSanad_Det>(), tranName, token);
			}
			line = 34;
			if (await source.GetReceptionAsync(EnSystem.Mapper, tranName, fromDatabase: false, token) != null)
			{
				webOrder = destination;
				ReceptionMapper instance = ReceptionMapper.Instance;
				string contextGuid2 = contextGuid;
				webOrder.Reception = await instance.MapAsync(contextGuid2, await source.GetReceptionAsync(EnSystem.Mapper, tranName, fromDatabase: false, token), new WebReception(), tranName, token);
			}
			line = 35;
			if (await source.GetPardakhtAsync(EnSystem.Mapper, token, tranName) != null)
			{
				webOrder = destination;
				PardakhtMapper instance2 = PardakhtMapper.Instance;
				string contextGuid2 = contextGuid;
				webOrder.Pardakht = await instance2.MapAsync(contextGuid2, await source.GetPardakhtAsync(EnSystem.Mapper, token, tranName), new WebPardakht(), tranName, token);
			}
			line = 36;
			TableRelationShipBusiness linkSanad = await source.GetLinkedSanadAsync(EnSystem.Mapper, token, tranName);
			if (linkSanad != null)
			{
				webOrder = destination;
				webOrder.LinkedSanads = await TableRelationShipMapper.Instance.MapAsync(contextGuid, linkSanad, new WebTableRelationShip(), tranName, token);
				if ((linkSanad?.ChildList?.Any()).GetValueOrDefault())
				{
					destination.LinkedSanad = new List<WebTableRelationShip>();
					foreach (TableRelationShipBusiness child in linkSanad.ChildList)
					{
						List<WebTableRelationShip> linkedSanad = destination.LinkedSanad;
						linkedSanad.Add(await TableRelationShipMapper.Instance.MapAsync(contextGuid, child, new WebTableRelationShip(), tranName, token));
					}
				}
			}
			destination.MetaData = source.MetaData;
			destination.KardexAffective = source.KardexAffective;
			destination.SanadAffective = source.SanadAffective;
			CusCompactBusiness customer = await source.GetCusInfoAsync(EnSystem.Mapper, token, tranName);
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token, tranName);
			if (customer != null)
			{
				destination.CustomerNationalCode = customer.FullCustomer.CodeMeli;
				destination.CustomerEconomicNumber = customer.FullCustomer.EghtesadiNo;
				destination.SellerEconomicNumber = setting.CompanyInfo.CompanyMeli;
				destination.CustomerType = customer.FullCustomer.CustType;
				destination.TaxCustZipCode = customer.FullCustomer.Zip;
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar)
				{
					destination.CustomerBrachCode = (await customer.FullCustomer.GetMetaDataListAsync(isClear: true, token))?.FirstOrDefault((CustomerMetaDataBusiness q) => q.MetaKey == CustomerMetaKey.BrachCode)?.Value;
				}
				destination.SellerBrachCode = setting.TaxService.SellerBranchNumber;
			}
			if ((source?.MetaData?.Tax?.TaxBillType).GetValueOrDefault(EnTaxBillType.None) != EnTaxBillType.None)
			{
				destination.TaxBillType = source.MetaData.Tax.TaxBillType;
			}
			else
			{
				destination.TaxBillType = setting.TaxService.TaxBillType;
			}
			destination.TasvieDate = source.TasvieDate;
			destination.TasvieDateMoaser = source.TasvieDateMoaser;
			destination.Template = (source?.MetaData?.Tax?.Template).GetValueOrDefault(EnTaxInvoiceTemplate.Sell);
			destination.ContractUniqueId = source?.MetaData?.Tax?.ContractUniqueId;
			destination.CustomsLicenseNumber = source?.MetaData?.Tax?.CustomsLicenseNumber;
			destination.CustomsCode = source?.MetaData?.Tax?.CustomsCode;
			destination.CottageDate = source?.MetaData?.Tax?.CottageDate;
			destination.CottageNumber = source?.MetaData?.Tax?.CottageNumber;
			destination.TaxTasvieMethod = source?.MetaData?.Tax?.TaxTasvieMethod;
			destination.DateUtc = source.DateUtc;
			destination.TaxImpact = source.TaxImpact;
			destination.CashBackCouponGuid = source.CashBackCouponGuid;
			destination.IsCashBackProgressed = source.IsCashBackProgressed;
			destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
			destination.SumService = source.SumService;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Map(OrderBusiness source, WebOrder destination,string tranName)", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 226);
			throw;
		}
	}

	public Task<List<WebOrder>> MapListAsync(string contextGuid, IEnumerable<OrderBusiness> source, List<WebOrder> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<OrderBusiness> MapAsync(string contextGuid, OrderBusiness source, OrderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new OrderBusiness(contextGuid);
			}
			line = 3;
			destination.FactorType = source.FactorType;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DeliveryStatus = source.DeliveryStatus;
			await destination.SetDateAsync(source.GetDate(), EnSystem.Mapper, token, tranName);
			line = 4;
			destination.Source = source.Source;
			line = 5;
			destination.WooOrder = new WooOrder();
			line = 6;
			destination.UserAccGuid = source.UserAccGuid;
			line = 7;
			await destination.SetB1TafsilGuidAsync(source.B1TafsilGuid, EnSystem.Mapper);
			line = 8;
			await destination.SetB2TafsilGuidAsync(source.B2TafsilGuid, EnSystem.Mapper);
			line = 9;
			await destination.SetB3TafsilGuidAsync(source.B3TafsilGuid, EnSystem.Mapper);
			line = 10;
			await destination.SetB4TafsilGuidAsync(source.B4TafsilGuid, EnSystem.Mapper);
			line = 11;
			destination.MoeinGuid = source.MoeinGuid;
			line = 12;
			OrderBusiness orderBusiness = destination;
			orderBusiness.SetId2(await source.GetId2Async(EnSystem.Mapper, tranName, token));
			line = 13;
			destination.B1MoeinGuid = source.B1MoeinGuid;
			line = 14;
			destination.B2MoeinGuid = source.B2MoeinGuid;
			line = 15;
			destination.B3MoeinGuid = source.B3MoeinGuid;
			line = 16;
			destination.B4MoeinGuid = source.B4MoeinGuid;
			line = 17;
			destination.AvarezTafsilGuid = source.AvarezTafsilGuid;
			line = 18;
			destination.AvarezMoeinGuid = source.AvarezMoeinGuid;
			line = 19;
			destination.TaxTafsilGuid = source.TaxTafsilGuid;
			line = 20;
			destination.TaxMoeinGuid = source.TaxMoeinGuid;
			line = 21;
			destination.Guid = source.Guid;
			line = 22;
			destination.Modified = source.Modified;
			line = 23;
			orderBusiness = destination;
			orderBusiness.SetAfterAccount(await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName));
			line = 24;
			destination.AghsatGuid = source.AghsatGuid;
			line = 25;
			destination.AndroidIme = source.AndroidIme;
			line = 26;
			destination.AndroidImeLong = source.AndroidImeLong;
			line = 27;
			destination.TransferDate = source.TransferDate;
			line = 28;
			destination.BarnameNo = source.BarnameNo;
			line = 29;
			destination.BazaryabFullName = source.BazaryabFullName;
			line = 30;
			orderBusiness = destination;
			orderBusiness.SetBeforeAccount(await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName));
			line = 31;
			destination.CpuSerial = source.CpuSerial;
			line = 33;
			destination.DateSabt = source.DateSabt;
			line = 34;
			destination.Description = source.Description;
			line = 35;
			destination.Discount = source.Discount;
			line = 37;
			destination.DailyFactorNo = source.DailyFactorNo;
			line = 38;
			destination.HazineTafsilGuid1 = source.HazineTafsilGuid1;
			line = 39;
			destination.HazineTafsilGuid2 = source.HazineTafsilGuid2;
			line = 40;
			destination.HazineTafsilGuid3 = source.HazineTafsilGuid3;
			line = 41;
			destination.HazineTafsilGuid4 = source.HazineTafsilGuid4;
			line = 42;
			destination.HazineTafsilGuid5 = source.HazineTafsilGuid5;
			line = 43;
			destination.HazineMoeinGuid1 = source.HazineMoeinGuid1;
			line = 44;
			destination.HazineMoeinGuid2 = source.HazineMoeinGuid2;
			line = 45;
			destination.HazineMoeinGuid3 = source.HazineMoeinGuid3;
			line = 46;
			destination.HazineMoeinGuid4 = source.HazineMoeinGuid4;
			line = 47;
			destination.HazineMoeinGuid5 = source.HazineMoeinGuid5;
			line = 480;
			destination.HddSerial = source.HddSerial;
			line = 481;
			await destination.SetTafsilGuid(source.GetTafsilGuid(), EnSystem.Mapper, updateVisitorGuid: false, default(CancellationToken), tranName);
			line = 50;
			destination.Ip = source.Ip;
			line = 51;
			destination.SetKeraye(source.Keraye);
			line = 52;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			line = 53;
			destination.FareTafsilGuid = source.FareTafsilGuid;
			destination.LkSerial = source.LkSerial;
			line = 55;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			line = 57;
			destination.PrintCount = source.PrintCount;
			line = 58;
			destination.PrintDate = source.PrintDate;
			destination.SellerFactorNo = source.SellerFactorNo;
			line = 60;
			destination.ReceptionId = source.ReceptionId;
			line = 61;
			destination.HostRefrenced = source.HostRefrenced;
			line = 62;
			destination.SanadNo = source.SanadNo;
			line = 63;
			destination.SanadSt = source.SanadSt;
			destination.TasvieDay = source.TasvieDay;
			line = 65;
			destination.DeliveryDate = source.DeliveryDate;
			line = 66;
			destination.DeliveryId = source.DeliveryId;
			line = 67;
			destination.DeliveryMessage = source.DeliveryMessage;
			line = 68;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			line = 70;
			await destination.SetIsTempAsync(EnSystem.Mapper, source.GetIsTemp(), token, tranName);
			line = 71;
			destination.CustName = source.CustName;
			line = 72;
			destination.CustFamily = source.CustFamily;
			destination.CustMobile = source.CustMobile;
			line = 74;
			destination.CustAddress = source.CustAddress;
			line = 75;
			destination.CustZipCode = source.CustZipCode;
			line = 76;
			destination.CustTell = source.CustTell;
			destination.PaymentMethod = source.PaymentMethod;
			line = 78;
			destination.ChangeStatus = source.ChangeStatus;
			line = 82;
			destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
			destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
			line = 84;
			orderBusiness = destination;
			orderBusiness.SetLinkCount(await source.GetLinkCountAsync(EnSystem.Mapper, token));
			destination.AttachmentNo = source.AttachmentNo;
			orderBusiness = destination;
			await orderBusiness.SetLinkedFilesAsync(await source.GetLinkedFilesAsync(EnSystem.Mapper, token), EnSystem.Mapper, token);
			destination.SendToServerStatus = source.SendToServerStatus;
			destination.StatusId = source.StatusId;
			destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
			line = 90;
			destination.ContextGuid = contextGuid;
			destination.ClearListKala();
			destination.ClearListService();
			line = 92;
			TaxUnits taxUnits = await LocalSettingBusiness.GetTaxUnitsAsync(contextGuid);
			foreach (OrderDetailBusiness item in source.ListKala ?? new List<OrderDetailBusiness>())
			{
				OrderDetMapper instance = OrderDetMapper.GetInstance(taxUnits);
				OrderDetailBusiness source2 = item;
				await destination.AddDetAsync(await instance.MapAsync(contextGuid, source2, await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, destination, token, tranName), tranName, token), EnSystem.Mapper, awaitOnSemaphor: true, token);
			}
			foreach (Sanad_DetBusiness item2 in source.ListSanad_Det ?? new List<Sanad_DetBusiness>())
			{
				Sanad_DetBusiness mappedSanad = new Sanad_DetBusiness(contextGuid, EnSystem.None, item2.AsGuid, item2.AsType, destination.NextOrdinal());
				await SanadDetMapper.Instance.MapAsync(contextGuid, item2, mappedSanad, tranName, token);
				await destination.AddToListSanadAsync(mappedSanad, EnSystem.Mapper);
			}
			destination.MetaData = source.MetaData;
			destination.KardexAffective = source.KardexAffective;
			destination.SanadAffective = source.SanadAffective;
			destination.TasvieDate = source.TasvieDate;
			destination.TasvieDateMoaser = source.TasvieDateMoaser;
			destination.DateUtc = source.DateUtc;
			destination.CashBackCouponGuid = source.CashBackCouponGuid;
			destination.IsCashBackProgressed = source.IsCashBackProgressed;
			destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Map(OrderBusiness source, OrderBusiness destination = null,string tranName=", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 428);
			throw;
		}
	}

	public Task<List<OrderBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderBusiness> source, List<OrderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<OrderBusiness> MapAsync(string contextGuid, WebOrder source, OrderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new OrderBusiness(contextGuid);
			}
			destination.FactorType = source.FactorType;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.Source = source.Source;
			line = 4;
			destination.TransferDate = source.TransferDate;
			destination.UserAccGuid = source.UserAccGuid;
			await destination.SetB1TafsilGuidAsync(source.B1TafsilGuid, EnSystem.Mapper);
			await destination.SetB2TafsilGuidAsync(source.B2TafsilGuid, EnSystem.Mapper);
			await destination.SetB3TafsilGuidAsync(source.B3TafsilGuid, EnSystem.Mapper);
			await destination.SetB4TafsilGuidAsync(source.B4TafsilGuid, EnSystem.Mapper);
			destination.MoeinGuid = source.MoeinGuid;
			destination.SetId2(source.Id2);
			destination.B1MoeinGuid = source.B1MoeinGuid;
			line = 6;
			destination.B2MoeinGuid = source.B2MoeinGuid;
			destination.B3MoeinGuid = source.B3MoeinGuid;
			destination.B4MoeinGuid = source.B4MoeinGuid;
			line = 7;
			destination.AvarezTafsilGuid = source.AvarezTafsilGuid;
			destination.AvarezMoeinGuid = source.AvarezMoeinGuid;
			destination.TaxTafsilGuid = source.TaxTafsilGuid;
			line = 8;
			destination.TaxMoeinGuid = source.TaxMoeinGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			line = 9;
			destination.SetAfterAccount(source.AfterAccount);
			destination.AghsatGuid = source.AghsatGuid;
			destination.AndroidIme = source.AndroidIme;
			line = 10;
			destination.AndroidImeLong = source.AndroidImeLong;
			destination.BarnameNo = source.BarnameNo;
			line = 11;
			destination.BazaryabFullName = source.BazaryabFullName;
			destination.SetBeforeAccount(source.BeforeAccount);
			destination.CpuSerial = source.CpuSerial;
			line = 12;
			destination.DateUtc = source.DateUtc;
			if (!source.DateUtc.HasValue)
			{
				await destination.SetDateAsync(source.Date, EnSystem.Mapper, token, tranName);
			}
			else
			{
				await destination.SetDateAsync(source.DateUtc.Value.ToLocalTime(), EnSystem.Mapper, token, tranName);
			}
			destination.Description = source.Description;
			line = 13;
			destination.Discount = source.Discount;
			destination.DailyFactorNo = source.DailyFactorNo;
			line = 14;
			destination.HazineTafsilGuid1 = source.HazineTafsilGuid1;
			destination.HazineTafsilGuid2 = source.HazineTafsilGuid2;
			destination.HazineTafsilGuid3 = source.HazineTafsilGuid3;
			line = 15;
			destination.HazineTafsilGuid4 = source.HazineTafsilGuid4;
			destination.HazineTafsilGuid5 = source.HazineTafsilGuid5;
			destination.HazineMoeinGuid1 = source.HazineMoeinGuid1;
			line = 16;
			destination.HazineMoeinGuid2 = source.HazineMoeinGuid2;
			destination.HazineMoeinGuid3 = source.HazineMoeinGuid3;
			destination.HazineMoeinGuid4 = source.HazineMoeinGuid4;
			line = 17;
			destination.HazineMoeinGuid5 = source.HazineMoeinGuid5;
			destination.HddSerial = source.HddSerial;
			await destination.SetTafsilGuid(source.TafsilGuid, EnSystem.Mapper, updateVisitorGuid: false, token, tranName);
			line = 18;
			destination.Ip = source.Ip;
			destination.SetKeraye(source.Keraye);
			destination.FareMoeinGuid = source.FareMoeinGuid;
			line = 19;
			destination.FareTafsilGuid = source.FareTafsilGuid;
			destination.LkSerial = source.LkSerial;
			destination.SellerFactorNo = source.SellerFactorNo;
			line = 20;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.PrintCount = source.PrintCount;
			destination.PrintDate = source.PrintDate;
			line = 21;
			destination.ReceptionId = source.ReceptionId;
			destination.HostRefrenced = source.HostRefrenced;
			line = 22;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.TasvieDay = source.TasvieDay;
			line = 23;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			line = 24;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			await destination.SetIsTempAsync(EnSystem.Mapper, source.IsTemp, token, tranName);
			destination.CustName = source.CustName;
			line = 25;
			destination.CustFamily = source.CustFamily;
			destination.CustMobile = source.CustMobile;
			destination.CustAddress = source.CustAddress;
			line = 26;
			destination.CustZipCode = source.CustZipCode;
			destination.CustTell = source.CustTell;
			destination.PaymentMethod = source.PaymentMethod;
			destination.ChangeStatus = source.ChangeStatus;
			line = 27;
			destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
			destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
			destination.ProductMoeinGuid = source.ProductMoeinGuid;
			destination.AttachmentNo = source.AttachmentNo;
			destination.StatusId = source.StatusId;
			destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
			line = 30;
			await destination.SetLinkedFilesAsync(source.LinkedFiles, EnSystem.Mapper, token);
			line = 31;
			destination.ContextGuid = contextGuid;
			destination?.ClearListKala();
			destination?.ClearListService();
			if (source.ListKala != null)
			{
				TaxUnits taxUnits = await LocalSettingBusiness.GetTaxUnitsAsync(contextGuid);
				foreach (WebOrderDetail item in source.ListKala)
				{
					OrderDetailBusiness destination2 = await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, destination, token, tranName);
					OrderBusiness orderBusiness = destination;
					await orderBusiness.AddDetAsync(await OrderDetMapper.GetInstance(taxUnits).MapAsync(contextGuid, item, destination2, tranName, token), EnSystem.Mapper, awaitOnSemaphor: true, token);
				}
			}
			line = 33;
			if (source.ListSanad != null)
			{
				OrderBusiness orderBusiness = destination;
				await orderBusiness.AddRangeToListSanadAsync(await SanadDetMapper.Instance.MapListAsync(contextGuid, source.ListSanad, new List<Sanad_DetBusiness>(), tranName, token), EnSystem.None, token);
			}
			line = 34;
			if (source.UserAccGuid != Guid.Empty && source.Reception != null && source.Reception.UserGuid != source.UserAccGuid)
			{
				WebErrorLog.Instance.StartLog($"Order.UserAccGuid({source.UserAccGuid}) has confilict with Order.ReceptionUserAccGuid({source.Reception.UserGuid})\r\n" + "این یک خطای ارسال داده نامعتبر از سمت کلاینت است ولیک سمت حسابداری رفع شده است.", "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 575);
				source.Reception.UserGuid = source.UserAccGuid;
			}
			if (source.UserAccGuid != Guid.Empty && source.Pardakht != null && source.Pardakht.UserGuid != source.UserAccGuid)
			{
				WebErrorLog.Instance.StartLog($"Order.UserAccGuid({source.UserAccGuid}) has confilict with Order.Pardakht.UserAccGuid({source.Pardakht.UserGuid})\r\n" + "این یک خطای ارسال داده نامعتبر از سمت کلاینت است ولیک سمت حسابداری رفع شده است.", "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 581);
				source.Pardakht.UserGuid = source.UserAccGuid;
			}
			if (source.Reception != null)
			{
				OrderBusiness orderBusiness = destination;
				orderBusiness.SetReception(await ReceptionMapper.Instance.MapAsync(contextGuid, source.Reception, new ReceptionBusiness(contextGuid, destination), tranName, token));
			}
			line = 35;
			if (source.Pardakht != null)
			{
				OrderBusiness orderBusiness = destination;
				orderBusiness.SetPardakht(await PardakhtMapper.Instance.MapAsync(contextGuid, source.Pardakht, new PardakhtBusiness(contextGuid, destination), tranName, token));
			}
			destination.MetaData = source.MetaData;
			destination.KardexAffective = source.KardexAffective.GetValueOrDefault(true);
			destination.SanadAffective = source.SanadAffective.GetValueOrDefault(true);
			destination.TasvieDate = source.TasvieDate;
			destination.TasvieDateMoaser = source.TasvieDateMoaser;
			destination.CashBackCouponGuid = source.CashBackCouponGuid;
			destination.IsCashBackProgressed = source.IsCashBackProgressed;
			destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Map(WebOrder source, OrderBusiness destination = null,string tranName=", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 600);
			throw;
		}
	}

	public Task<List<OrderBusiness>> MapListAsync(string contextGuid, IEnumerable<WebOrder> source, List<OrderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<OrderReportBusiness> MapAsync(string contextGuid, WebOrderReport source, OrderReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new OrderReportBusiness(contextGuid);
			}
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.AfterAccount = source.AfterAccount;
			destination.AghsatGuid = source.AghsatGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.AndroidImeLong = source.AndroidImeLong;
			destination.Avarez = source.Avarez;
			destination.B1TafsilGuid = source.B1TafsilGuid;
			destination.B2TafsilGuid = source.B2TafsilGuid;
			destination.B3TafsilGuid = source.B3TafsilGuid;
			destination.B4TafsilGuid = source.B4TafsilGuid;
			destination.BarnameNo = source.BarnameNo;
			destination.TransferDate = source.TransferDate;
			destination.BeforeAccount = source.BeforeAccount;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.Discount = source.Discount;
			destination.DiscountRows = source.DiscountRows;
			destination.FactorType = source.FactorType;
			destination.DailyFactorNo = source.DailyFactorNo;
			destination.HazineTafsilGuid1 = source.HazineTafsilGuid1;
			destination.HazineTafsilGuid2 = source.HazineTafsilGuid2;
			destination.HazineTafsilGuid3 = source.HazineTafsilGuid3;
			destination.HazineTafsilGuid4 = source.HazineTafsilGuid4;
			destination.HazineTafsilGuid5 = source.HazineTafsilGuid5;
			destination.HazineMoeinGuid1 = source.HazineMoeinGuid1;
			destination.HazineMoeinGuid2 = source.HazineMoeinGuid2;
			destination.HazineMoeinGuid3 = source.HazineMoeinGuid3;
			destination.HazineMoeinGuid4 = source.HazineMoeinGuid4;
			destination.HazineMoeinGuid5 = source.HazineMoeinGuid5;
			destination.HddSerial = source.HddSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Id2 = source.Id2;
			destination.Ip = source.Ip;
			destination.Keraye = source.Keraye;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			destination.FareTafsilGuid = source.FareTafsilGuid;
			destination.LkSerial = source.LkSerial;
			destination.PrintCount = source.PrintCount;
			destination.PrintDate = source.PrintDate;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.UserAccGuid = source.UserAccGuid;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.IsTemp = source.IsTemp;
			destination.HostRefrenced = source.HostRefrenced;
			destination.B1MoeinGuid = source.B1MoeinGuid;
			destination.B2MoeinGuid = source.B2MoeinGuid;
			destination.B3MoeinGuid = source.B3MoeinGuid;
			destination.B4MoeinGuid = source.B4MoeinGuid;
			destination.AvarezTafsilGuid = source.AvarezTafsilGuid;
			destination.AvarezMoeinGuid = source.AvarezMoeinGuid;
			destination.TaxTafsilGuid = source.TaxTafsilGuid;
			destination.TaxMoeinGuid = source.TaxMoeinGuid;
			destination.CustName = source.CustName;
			destination.CustFamily = source.CustFamily;
			destination.CustMobile = source.CustMobile;
			destination.CustAddress = source.CustAddress;
			destination.CustZipCode = source.CustZipCode;
			destination.CustTell = source.CustTell;
			destination.PaymentMethod = source.PaymentMethod;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.Source = source.Source;
			destination.B1 = source.B1;
			destination.B2 = source.B2;
			destination.B3 = source.B3;
			destination.B4 = source.B4;
			destination.CustomerFullName = source.CustomerFullName;
			destination.DateSh = source.DateSh;
			destination.Status = source.Status;
			destination.Sum = source.Sum;
			destination.SumKala = source.SumKala;
			destination.SumService = source.SumService;
			destination.TasvieDay = source.TasvieDay;
			destination.Tax = source.Tax;
			destination.TotalHazine = source.TotalHazine;
			destination.TotalHazine1 = source.TotalHazine1;
			destination.TotalHazine2 = source.TotalHazine2;
			destination.TotalHazine3 = source.TotalHazine3;
			destination.TotalHazine4 = source.TotalHazine4;
			destination.TotalHazine5 = source.TotalHazine5;
			destination.TotalSum = source.TotalSum;
			destination.SellerFactorNo = source.SellerFactorNo;
			destination.ProductMoeinGuid = source.ProductMoeinGuid;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
			destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
			destination.AttachmentNo = source.AttachmentNo;
			destination.StatusId = source.StatusId;
			destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.B1TafsilGuid = source.B1TafsilGuid;
			destination.B2TafsilGuid = source.B2TafsilGuid;
			destination.B3TafsilGuid = source.B3TafsilGuid;
			destination.B4TafsilGuid = source.B4TafsilGuid;
			destination.BazaryabFullName = source.BazaryabFullName;
			destination.IntroductionMethodName = source.IntroductionMethodName;
			destination.KardexAffective = source.KardexAffective;
			destination.SanadAffective = source.SanadAffective;
			destination.TasvieDate = source.TasvieDate;
			destination.TasvieDateMoaser = source.TasvieDateMoaser;
			destination.TaxResult = source.TaxResult;
			destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
			destination.TaxResultErrorCode = source.TaxResultErrorCode;
			destination.TaxResultMessage = source.TaxResultMessage;
			destination.TaxSendDate = source.TaxSendDate;
			destination.TaxUid = source.TaxUid;
			destination.TaxOrderId = source.TaxOrderId;
			destination.DateUtc = source.DateUtc;
			destination.CashBackCouponGuid = source.CashBackCouponGuid;
			destination.IsCashBackProgressed = source.IsCashBackProgressed;
			destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
			destination.SumService = source.SumService;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 747);
			throw;
		}
	}

	public Task<List<OrderReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebOrderReport> source, List<OrderReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebOrderReport> MapAsync(string contextGuid, OrderReportBusiness source, WebOrderReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebOrderReport();
			}
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.AfterAccount = source.AfterAccount;
			destination.AghsatGuid = source.AghsatGuid;
			destination.AndroidIme = source.AndroidIme;
			destination.AndroidImeLong = source.AndroidImeLong;
			destination.Avarez = source.Avarez;
			destination.B1TafsilGuid = source.B1TafsilGuid;
			destination.B2TafsilGuid = source.B2TafsilGuid;
			destination.B3TafsilGuid = source.B3TafsilGuid;
			destination.B4TafsilGuid = source.B4TafsilGuid;
			destination.BarnameNo = source.BarnameNo;
			destination.TransferDate = source.TransferDate;
			destination.BeforeAccount = source.BeforeAccount;
			destination.CpuSerial = source.CpuSerial;
			destination.Date = source.Date;
			destination.DateSabt = source.DateSabt;
			destination.Description = source.Description;
			destination.Discount = source.Discount;
			destination.DiscountRows = source.DiscountRows;
			destination.FactorType = source.FactorType;
			destination.DailyFactorNo = source.DailyFactorNo;
			destination.HazineTafsilGuid1 = source.HazineTafsilGuid1;
			destination.HazineTafsilGuid2 = source.HazineTafsilGuid2;
			destination.HazineTafsilGuid3 = source.HazineTafsilGuid3;
			destination.HazineTafsilGuid4 = source.HazineTafsilGuid4;
			destination.HazineTafsilGuid5 = source.HazineTafsilGuid5;
			destination.HazineMoeinGuid1 = source.HazineMoeinGuid1;
			destination.HazineMoeinGuid2 = source.HazineMoeinGuid2;
			destination.HazineMoeinGuid3 = source.HazineMoeinGuid3;
			destination.HazineMoeinGuid4 = source.HazineMoeinGuid4;
			destination.HazineMoeinGuid5 = source.HazineMoeinGuid5;
			destination.HddSerial = source.HddSerial;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Id2 = source.Id2;
			destination.Ip = source.Ip;
			destination.Keraye = source.Keraye;
			destination.FareMoeinGuid = source.FareMoeinGuid;
			destination.FareTafsilGuid = source.FareTafsilGuid;
			destination.LkSerial = source.LkSerial;
			destination.PrintCount = source.PrintCount;
			destination.PrintDate = source.PrintDate;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.UserAccGuid = source.UserAccGuid;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.DeliveryDate = source.DeliveryDate;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.IsTemp = source.IsTemp;
			destination.HostRefrenced = source.HostRefrenced;
			destination.B1MoeinGuid = source.B1MoeinGuid;
			destination.B2MoeinGuid = source.B2MoeinGuid;
			destination.B3MoeinGuid = source.B3MoeinGuid;
			destination.B4MoeinGuid = source.B4MoeinGuid;
			destination.AvarezTafsilGuid = source.AvarezTafsilGuid;
			destination.AvarezMoeinGuid = source.AvarezMoeinGuid;
			destination.TaxTafsilGuid = source.TaxTafsilGuid;
			destination.TaxMoeinGuid = source.TaxMoeinGuid;
			destination.CustName = source.CustName;
			destination.CustFamily = source.CustFamily;
			destination.CustMobile = source.CustMobile;
			destination.CustAddress = source.CustAddress;
			destination.CustZipCode = source.CustZipCode;
			destination.CustTell = source.CustTell;
			destination.PaymentMethod = source.PaymentMethod;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryStatus = source.DeliveryStatus;
			destination.Source = source.Source;
			destination.B1 = source.B1;
			destination.B2 = source.B2;
			destination.B3 = source.B3;
			destination.B4 = source.B4;
			destination.CustomerFullName = source.CustomerFullName;
			destination.DateSh = source.DateSh;
			destination.Status = source.Status;
			destination.Sum = source.Sum;
			destination.SumKala = source.SumKala;
			destination.SumService = source.SumService;
			destination.TasvieDay = source.TasvieDay;
			destination.Tax = source.Tax;
			destination.TotalHazine = source.TotalHazine;
			destination.TotalHazine1 = source.TotalHazine1;
			destination.TotalHazine2 = source.TotalHazine2;
			destination.TotalHazine3 = source.TotalHazine3;
			destination.TotalHazine4 = source.TotalHazine4;
			destination.TotalHazine5 = source.TotalHazine5;
			destination.TotalSum = source.TotalSum;
			destination.SellerFactorNo = source.SellerFactorNo;
			destination.ProductMoeinGuid = source.ProductMoeinGuid;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
			destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
			destination.AttachmentNo = source.AttachmentNo;
			destination.StatusId = source.StatusId;
			destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.B1TafsilGuid = source.B1TafsilGuid;
			destination.B2TafsilGuid = source.B2TafsilGuid;
			destination.B3TafsilGuid = source.B3TafsilGuid;
			destination.B4TafsilGuid = source.B4TafsilGuid;
			destination.BazaryabFullName = source.BazaryabFullName;
			destination.IntroductionMethodName = source.IntroductionMethodName;
			destination.KardexAffective = source.KardexAffective;
			destination.SanadAffective = source.SanadAffective;
			destination.TasvieDate = source.TasvieDate;
			destination.TasvieDateMoaser = source.TasvieDateMoaser;
			destination.TaxResult = source.TaxResult;
			destination.TaxRefrenceNumber = source.TaxRefrenceNumber;
			destination.TaxResultErrorCode = source.TaxResultErrorCode;
			destination.TaxResultMessage = source.TaxResultMessage;
			destination.TaxSendDate = source.TaxSendDate;
			destination.TaxUid = source.TaxUid;
			destination.TaxOrderId = source.TaxOrderId;
			destination.DateUtc = source.DateUtc;
			destination.CashBackCouponGuid = source.CashBackCouponGuid;
			destination.IsCashBackProgressed = source.IsCashBackProgressed;
			destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
			destination.SumService = source.SumService;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderMapper.cs", 894);
			throw;
		}
	}

	public Task<List<WebOrderReport>> MapListAsync(string contextGuid, IEnumerable<OrderReportBusiness> source, List<WebOrderReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
