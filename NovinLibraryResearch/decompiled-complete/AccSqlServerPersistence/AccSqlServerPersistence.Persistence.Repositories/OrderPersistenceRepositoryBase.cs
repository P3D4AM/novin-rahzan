using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;
using EntityCache;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class OrderPersistenceRepositoryBase : IOrderRepository, IEntityRepository<OrderBusiness, OrderReportBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<OrderBusiness, Order>, IItemAsyncMapper<Order, OrderBusiness>, IItemAsyncMapper<Order, OrderReportBusiness>
	{
		public async Task<Order> MapAsync(string contextGuid, OrderBusiness source, Order destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
			{
				_ = 6;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new Order();
					}
					destination.PardakhtGuid = source.PardakhtGuid;
					destination.ReceptionGuid = ((source.ReceptionGuid == Guid.Empty) ? null : source.ReceptionGuid);
					destination.AghsatGuid = source.AghsatGuid;
					destination.AndroidIme = source.AndroidIme;
					destination.AndroidImeLong = source.AndroidImeLong;
					destination.Avarez = source.Avarez;
					destination.B1 = source.B1;
					destination.B1TafsilGuid = source.B1TafsilGuid;
					destination.B2TafsilGuid = source.B2TafsilGuid;
					destination.B3TafsilGuid = source.B3TafsilGuid;
					destination.B4TafsilGuid = source.B4TafsilGuid;
					destination.B2 = source.B2;
					destination.B3 = source.B3;
					destination.B4 = source.B4;
					destination.BarnameNo = source.BarnameNo;
					destination.BazaryabFullName = source.BazaryabFullName;
					Order order;
					if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
					{
						order = destination;
						order.AfterAccount = await source.GetAfterAccountAsync(EnSystem.Mapper, token, tranName);
						order = destination;
						order.BeforeAccount = await source.GetBeforeAccountAsync(EnSystem.Mapper, token, tranName);
					}
					order = destination;
					order.CustomerFullName = await source.GetFullNameAsync(EnSystem.Mapper, tranName, token);
					destination.CpuSerial = source.CpuSerial;
					destination.Date = source.GetDate();
					destination.DateUtc = destination.Date.ToUniversalTime();
					destination.DateSabt = source.DateSabt;
					destination.Description = source.Description ?? "";
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
					destination.TafsilGuid = source.GetTafsilGuid();
					destination.MoeinGuid = source.MoeinGuid;
					order = destination;
					order.Id2 = await source.GetId2Async(EnSystem.Mapper, tranName, token);
					destination.Ip = source.Ip;
					destination.Keraye = source.Keraye;
					destination.FareMoeinGuid = source.FareMoeinGuid;
					destination.FareTafsilGuid = source.FareTafsilGuid;
					destination.LkSerial = source.LkSerial;
					order = destination;
					order.PardakhtSum = await source.PardakhtSumAsync(EnSystem.Mapper, token, tranName);
					destination.PrintCount = source.PrintCount;
					destination.PrintDate = source.PrintDate;
					destination.TransferDate = source.TransferDate;
					order = destination;
					order.ReceptionSum = await source.ReceptionSumAsync(EnSystem.Mapper, token, tranName);
					order = destination;
					order.SanadNo = await source.GetSanadNumberAsync(token, tranName);
					destination.SanadSt = source.SanadSt;
					destination.Sum = source.Sum;
					destination.SumKala = source.SumKala;
					destination.TasvieDay = source.TasvieDay;
					destination.Tax = source.Tax;
					destination.TotalBazaryab = source.TotalBazaryab;
					destination.TotalDiscount = source.TotalDiscount;
					destination.TotalHazine = source.TotalHazine;
					destination.TotalHazine1 = source.TotalHazine1;
					destination.TotalHazine2 = source.TotalHazine2;
					destination.TotalHazine3 = source.TotalHazine3;
					destination.TotalHazine4 = source.TotalHazine4;
					destination.TotalHazine5 = source.TotalHazine5;
					destination.TotalSum = source.TotalSum;
					destination.UserAccGuid = source.UserAccGuid;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.DeliveryDate = source.DeliveryDate;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.PriceTypeGuid = (((source.PriceTypeGuid ?? Guid.Empty) == Guid.Empty) ? null : source.PriceTypeGuid);
					destination.IsTemp = source.GetIsTemp();
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
					destination.SellerFactorNo = source.SellerFactorNo;
					destination.ProductMoeinGuid = source.ProductMoeinGuid;
					destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
					destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
					destination.AttachmentNo = source.AttachmentNo;
					destination.StatusId = source.StatusId;
					destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.KardexAffective = source.KardexAffective;
					destination.SanadAffective = source.SanadAffective;
					destination.TasvieDate = source.TasvieDate;
					destination.TasvieDateMoaser = source.TasvieDateMoaser;
					destination.Status = source.Status;
					destination.TaxImpact = source.TaxImpact;
					destination.CashBackCouponGuid = source.CashBackCouponGuid;
					destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
					destination.SumService = source.SumService;
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1535);
					throw;
				}
			}
		}

		public async Task<OrderBusiness> MapAsync(string contextGuid, Order source, OrderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
			{
				_ = 6;
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
					destination.UserAccGuid = source.UserAccGuid;
					destination.ReceptionGuid = source.ReceptionGuid;
					destination.PardakhtGuid = source.PardakhtGuid;
					destination.SetAfterAccount(source.AfterAccount);
					destination.AghsatGuid = source.AghsatGuid;
					destination.AndroidIme = source.AndroidIme;
					destination.AndroidImeLong = source.AndroidImeLong;
					destination.TransferDate = source.TransferDate;
					await destination.SetB1TafsilGuidAsync(source.B1TafsilGuid, EnSystem.Mapper);
					await destination.SetB2TafsilGuidAsync(source.B2TafsilGuid, EnSystem.Mapper);
					await destination.SetB3TafsilGuidAsync(source.B3TafsilGuid, EnSystem.Mapper);
					await destination.SetB4TafsilGuidAsync(source.B4TafsilGuid, EnSystem.Mapper);
					destination.BarnameNo = source.BarnameNo;
					destination.BazaryabFullName = source.BazaryabFullName;
					destination.SetBeforeAccount(source.BeforeAccount);
					destination.CpuSerial = source.CpuSerial;
					await destination.SetDateAsync(source.Date, EnSystem.Mapper, token, tranName);
					destination.DateSabt = source.DateSabt;
					destination.Description = source.Description;
					destination.Discount = source.Discount;
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
					await destination.SetTafsilGuid(source.TafsilGuid, EnSystem.Mapper, updateVisitorGuid: false, token, tranName);
					destination.MoeinGuid = source.MoeinGuid;
					destination.SetId2(source.Id2);
					destination.Ip = source.Ip;
					if (source.FactorType != Document.OrderBuy)
					{
						destination.SetKeraye(source.Keraye);
					}
					destination.FareMoeinGuid = source.FareMoeinGuid;
					destination.FareTafsilGuid = source.FareTafsilGuid;
					destination.LkSerial = source.LkSerial;
					destination.PrintCount = source.PrintCount;
					destination.PrintDate = source.PrintDate;
					destination.SanadNo = source.SanadNo;
					destination.SanadSt = source.SanadSt;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.DeliveryDate = source.DeliveryDate;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.PriceTypeGuid = source.PriceTypeGuid;
					await destination.SetIsTempAsync(EnSystem.Mapper, source.IsTemp, token, tranName);
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
					destination.SellerFactorNo = source.SellerFactorNo;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.ProductMoeinGuid = source.ProductMoeinGuid;
					destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
					destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
					destination.TasvieDay = source.TasvieDay;
					destination.AttachmentNo = source.AttachmentNo;
					destination.StatusId = source.StatusId;
					destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
					destination.MetaData = source.MetaData.FromJson<OrderMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1636);
					destination.KardexAffective = source.KardexAffective;
					destination.SanadAffective = source.SanadAffective;
					destination.TasvieDate = source.TasvieDate;
					destination.TasvieDateMoaser = source.TasvieDateMoaser;
					destination.DateUtc = source.DateUtc;
					destination.Status = source.Status;
					destination.CashBackCouponGuid = source.CashBackCouponGuid;
					destination.IsCashBackProgressed = source.IsCashBackProgressed;
					destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1652);
					throw;
				}
			}
		}

		public Task<List<Order>> MapListAsync(string contextGuid, IEnumerable<OrderBusiness> source, List<Order> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OrderBusiness>> MapListAsync(string contextGuid, IEnumerable<Order> source, List<OrderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public async Task<OrderReportBusiness> MapAsync(string contextGuid, Order source, OrderReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
			{
				_ = 4;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
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
					destination.DateSh = source.Date.MiladiToCalture();
					destination.Status = source.Status;
					destination.Sum = source.Sum;
					destination.SumKala = source.SumKala;
					destination.SumService = source.Sum - source.SumKala;
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
					destination.DiscountTafsilGuid = source.DiscountTafsilGuid;
					destination.DiscountMoeinGuid = source.DiscountMoeinGuid;
					destination.AttachmentNo = source.AttachmentNo;
					destination.StatusId = source.StatusId;
					destination.IntroductionMethodGuid = source.IntroductionMethodGuid;
					destination.TafsilGroupGuid = source.TafsilItem?.TafsilGroupGuid ?? Guid.Empty;
					destination.DateUtc = source.DateUtc;
					string text = "";
					if (destination.B1TafsilGuid.HasValue && destination.B1TafsilGuid != Guid.Empty)
					{
						string text2 = text;
						text = text2 + (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, destination.B1TafsilGuid.Value, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))?.Name + " - ";
					}
					if (destination.B2TafsilGuid.HasValue && destination.B2TafsilGuid != Guid.Empty)
					{
						string text2 = text;
						text = text2 + (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, destination.B2TafsilGuid.Value, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))?.Name + " - ";
					}
					if (destination.B3TafsilGuid.HasValue && destination.B3TafsilGuid != Guid.Empty)
					{
						string text2 = text;
						text = text2 + (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, destination.B3TafsilGuid.Value, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))?.Name + " - ";
					}
					if (destination.B4TafsilGuid.HasValue && destination.B4TafsilGuid != Guid.Empty)
					{
						string text2 = text;
						text = text2 + (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, destination.B4TafsilGuid.Value, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))?.Name;
					}
					destination.BazaryabFullName = text;
					if ((source?.IntroductionMethodGuid ?? Guid.Empty) != Guid.Empty)
					{
						OrderReportBusiness orderReportBusiness = destination;
						orderReportBusiness.IntroductionMethodName = (await IntroductionMethodBusiness.GetAsync(contextGuid, source.IntroductionMethodGuid.Value, token, tranName))?.Name ?? "";
					}
					destination.KardexAffective = source.KardexAffective;
					destination.SanadAffective = source.SanadAffective;
					destination.TasvieDate = source.TasvieDate;
					destination.TasvieDateMoaser = source.TasvieDateMoaser;
					destination.TaxImpact = source.TaxImpact;
					destination.CashBackCouponGuid = source.CashBackCouponGuid;
					destination.IsCashBackProgressed = source.IsCashBackProgressed;
					destination.ServiceMoeinGuid = source.ServiceMoeinGuid;
					destination.SumService = source.SumService;
					TaxOrderLog taxOrderLog = source?.TaxOrderLog?.OrderByDescending((TaxOrderLog q) => q.InsertDate)?.FirstOrDefault();
					if (taxOrderLog != null)
					{
						destination.TaxResult = ((taxOrderLog.NewState.GetValueOrDefault() == EnTaxResult.None) ? taxOrderLog.OldState : taxOrderLog.NewState.Value);
						destination.TaxRefrenceNumber = taxOrderLog.TaxRefrenceNumber;
						destination.TaxResultErrorCode = taxOrderLog.TaxResultErrorCode;
						destination.TaxResultMessage = taxOrderLog.TaxResultMessage;
						destination.TaxSendDate = taxOrderLog.TaxSendDate;
						destination.TaxUid = taxOrderLog.TaxUid;
						destination.TaxOrderId = taxOrderLog.TaxOrderId ?? "";
					}
					return destination;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1814);
					throw;
				}
			}
		}

		public Task<List<OrderReportBusiness>> MapListAsync(string contextGuid, IEnumerable<Order> source, List<OrderReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	protected OrderDetPersistenceRepository orderDetRepo;

	protected SanadDetPersistenceRepository orderSanadDetRepo;

	protected ITableRelationShipRepository orderRelationShipRepo;

	private LastPricePersistenceRepository _lastPriceRepo;

	private TolidPersistenceRepository _tolidRepo;

	protected ContextHolder dbContext;

	public IOrderDetailRepository OrderDetailRepo { get; }

	public string ContextGuid => dbContext.Context.Guid;

	public WebContextHolder Context => dbContext.Context;

	public IItemAsyncMapper<Order, OrderReportBusiness> MapFromDatabaseToReport { get; set; } = new mapper();


	public IItemAsyncMapper<Order, OrderBusiness> MapFromDatabase { get; set; } = new mapper();


	public IItemAsyncMapper<OrderBusiness, Order> MapToDatabase { get; set; } = new mapper();


	internal OrderPersistenceRepositoryBase(ContextHolder context, ITableRelationShipRepository attachmentRepo)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			try
			{
				dbContext = context;
				orderDetRepo = new OrderDetPersistenceRepository(context);
				orderSanadDetRepo = new SanadDetPersistenceRepository(context);
				orderRelationShipRepo = attachmentRepo;
				_lastPriceRepo = new LastPricePersistenceRepository(context);
				_tolidRepo = new TolidPersistenceRepository(context);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 63);
				throw;
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				orderDetRepo?.Dispose();
				orderDetRepo = null;
				orderSanadDetRepo?.Dispose();
				orderSanadDetRepo = null;
				orderRelationShipRepo?.Dispose();
				orderRelationShipRepo = null;
				_lastPriceRepo?.Dispose();
				_lastPriceRepo = null;
				_tolidRepo?.Dispose();
				_tolidRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 93);
		}
	}

	public async Task<bool> CustomerIsSend(Guid guid, CancellationToken token, string tranName)
	{
		try
		{
			OrderBusiness orderBusiness = await GetAsync(guid, token, "");
			if (orderBusiness == null)
			{
				return true;
			}
			if (orderBusiness.ServerStatus != ServerStatusForClient.Delivered && orderBusiness.ServerStatus != ServerStatusForClient.DirectDelivery)
			{
				return false;
			}
			return true;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "CustomerIsSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 111);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 123));
				Order order = await dbcontext.Order.FindAsync(new object[1] { guid });
				order.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(order, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 126);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 131);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 132);
			}
			return ret;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 144));
				Order order = await EntityFrameworkQueryableExtensions.SingleAsync<Order>((IQueryable<Order>)dbcontext.Order, (Expression<Func<Order, bool>>)((Order p) => p.Guid == item.Guid), token);
				if (order == null)
				{
					return ret;
				}
				if (!order.Status)
				{
					throw new ArgumentException("Order Already Removed!!!");
				}
				order.ServerStatus = ServerStatusForClient.None;
				order.Status = false;
				order.Modified = DateTime.Now;
				order.CashBackCouponGuid = null;
				int num = await item.GetId2Async(EnSystem.None, tranName, token);
				if (num != order.Id2)
				{
					order.Id2 = num;
				}
				dbcontext.Order.Update(order);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await orderSanadDetRepo.RemoveAsync(item.Guid, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 155);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await orderRelationShipRepo.RemoveRangeAsync(item.Guid, order.FactorType, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 156);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 161);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 162);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetPrintCountAsync(OrderBusiness item, DateTime dateM, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 174));
				IQueryable<Order> queryable = ((IQueryable<Order>)context.Order).Where((Order x) => x.Guid == item.Guid);
				Order order = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>(queryable, token) : null);
				if (order == null)
				{
					return ret.AddError($"order with guid({item.Guid}) not founded!", "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 176);
				}
				order.PrintCount++;
				order.PrintDate = dateM;
				((DbContext)context).Update<Order>(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 184);
				ret.AddReturnedValue(ex2, "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 185);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> OrderSortAsync(Document orderType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<OrderBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 195));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 8;
				try
				{
					Order res = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.Guid == guid), token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						OrderBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, res, null, tranName, token);
						ret.ChangeStatus = EntityChangeSt.Edited;
						await orderDetRepo.GetAllAsync(ret, token, tranName);
						ret.SetKeraye(res.Keraye);
						List<Sanad_DetBusiness> list = await orderSanadDetRepo.GetAllAsync(ret.Guid, ret.FactorType, null, null, token, tranName);
						if (list != null && list.Any())
						{
							foreach (Sanad_DetBusiness item in list ?? new List<Sanad_DetBusiness>())
							{
								item.AsType = Document.Sanad;
							}
							await ret.AddRangeToListSanadAsync(list, EnSystem.None, token);
						}
						IEnumerable<TableRelationShipBusiness> enumerable = await orderRelationShipRepo.GetAllRelatedSanadAsync(guid, res.FactorType, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							await ret.AddListToLinkedSanadsAsync(enumerable);
						}
						TolidBusiness tolidBusiness = await _tolidRepo.GetAsync(guid, token, tranName);
						if (tolidBusiness != null)
						{
							await ret.SetTolidAsync(tolidBusiness, null, EnSystem.None, null, token, tranName);
						}
						result = ret;
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 227);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task<OrderBusiness> GetAsync(Guid guid, Document factorType, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(OrderBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 246));
				line = 2;
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				line = 5;
				Order res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				line = 6;
				if (await item.PardakhtSumAsync(EnSystem.Mapper, token, tranName) <= 0m)
				{
					res.PardakhtGuid = null;
				}
				if (await item.ReceptionSumAsync(EnSystem.Mapper, token, tranName) <= 0m)
				{
					res.ReceptionGuid = null;
				}
				Console.WriteLine(Json.ToStringJson(res));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 255);
				((DbContext)dbcontext).SaveChanges();
				line = 7;
				List<OrderDetail> list = await EntityFrameworkQueryableExtensions.ToListAsync<OrderDetail>(((IQueryable<OrderDetail>)dbcontext.OrderDet).Where((OrderDetail p) => item.ListKala.Select((OrderDetailBusiness p) => p.Guid).Contains(p.Guid)), token);
				if (list != null && list.Any((OrderDetail p) => p.MasterGuid != item.Guid))
				{
					throw new Exception("خطای ادغام اقلام یک فاکتور در فاکتور دیگر بوجود آمده".GetTranslate() + "\r\n" + $"orderGuid:{item.Guid}\r\n" + $"orderType:{item.FactorType}\r\n" + "det guids:" + string.Join(",", list.Select((OrderDetail p) => p.Guid)));
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await orderDetRepo.RemoveAsync(item.Guid, item.FactorType, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 264);
				line = 9;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveRemovedLastPriceAsync(item, isRemoved: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 266);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await orderDetRepo.SaveAsync(item.ListKala?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 267);
				line = 15;
				((DbContext)dbcontext).SaveChanges();
				if (item.ListSanad_Det?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item2 in item.ListSanad_Det)
					{
						if (item2.AsType != item.FactorType)
						{
							throw new Exception("asType is nul equal order");
						}
						if (item2.AsGuid != item.Guid)
						{
							item2.AsGuid = item.Guid;
						}
					}
				}
				foreach (Sanad_DetBusiness item3 in (await orderSanadDetRepo.GetAllAsync(item.Guid, item.FactorType, null, null, token, tranName)) ?? new List<Sanad_DetBusiness>())
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await orderSanadDetRepo.RemoveAsync(item3.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 279);
				}
				if ((item?.ListSanad_Det?.Any()).GetValueOrDefault())
				{
					foreach (Sanad_DetBusiness item4 in item.ListSanad_Det)
					{
						item4.AsType = item.FactorType;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await orderSanadDetRepo.SaveRangeAsync(item.ListSanad_Det?.ToList(), tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 284);
					((DbContext)dbcontext).SaveChanges();
				}
				((DbContext)dbcontext).SaveChanges();
				TableRelationShipBusiness tableRelationShipBusiness = await item.GetLinkedSanadAsync(EnSystem.Mapper, token, tranName);
				if (tableRelationShipBusiness != null)
				{
					tableRelationShipBusiness.SanadGuid = item.Guid;
					tableRelationShipBusiness.SanadType = item.FactorType;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await orderRelationShipRepo.SaveAsync(tableRelationShipBusiness, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 293);
					((DbContext)dbcontext).SaveChanges();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 300);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 301);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveRemovedLastPriceAsync(OrderBusiness order, bool isRemoved, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<Guid> prdGuidList = order?.ListKala?.Select((OrderDetailBusiness q) => q.ProductGuid);
			if (!(prdGuidList?.Any() ?? false))
			{
				return res;
			}
			IEnumerable<LastPriceBusiness> dbLastPrices = await _lastPriceRepo.GetAllAsync(order.Guid, order.FactorType, token, tranName);
			if (!(dbLastPrices?.Any() ?? false))
			{
				return res;
			}
			IEnumerable<Guid> exitDbNotExixtBusiness = null;
			if (!isRemoved)
			{
				exitDbNotExixtBusiness = from q in dbLastPrices
					select q.PrdGuid into q
					where !prdGuidList.Contains(q)
					select q;
				if (!(exitDbNotExixtBusiness?.Any() ?? false))
				{
					return res;
				}
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRemovedLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 322));
			foreach (Guid prdGuid in exitDbNotExixtBusiness)
			{
				KardexBusiness lastKardexRecord = await KardexBusiness.GetLastAsync(ContextGuid, order.TafsilGuid, prdGuid, order.FactorType, token, tranName);
				LastPriceBusiness source = dbLastPrices.First((LastPriceBusiness q) => q.PrdGuid == prdGuid);
				LastPrice lastPrice = await _lastPriceRepo.MapToDatabase.MapAsync(ContextGuid, source, null, tranName, token);
				if (lastKardexRecord == null)
				{
					lastPrice.Status = false;
					lastPrice.ServerStatus = ServerStatusForClient.None;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(lastPrice, lastPrice.TafsilGuid, lastPrice.PrdGuid), ReturnedState.None, "SaveRemovedLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 334);
				}
				else
				{
					lastPrice.Price = lastKardexRecord.UnitPrice;
					lastPrice.Date = lastKardexRecord.DateM;
					lastPrice.ServerStatus = ServerStatusForClient.None;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(lastPrice, lastPrice.TafsilGuid, lastPrice.PrdGuid), ReturnedState.None, "SaveRemovedLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 341);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRemovedLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 347);
			res.AddReturnedValue(ex, "", "SaveRemovedLastPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 348);
		}
		return res;
	}

	public async Task<int> NextId2Async(Document factorType, bool? status, int? statusId, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 354));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					if (statusId.GetValueOrDefault() <= 0)
					{
						goto IL_044b;
					}
					SanadStatusReport sanadStatus = await c.SanadStatusReport.FindAsync(new object[1] { statusId.Value });
					if ((sanadStatus?.Id2From).GetValueOrDefault() <= 0)
					{
						goto IL_044b;
					}
					List<int> list = await EntityFrameworkQueryableExtensions.ToListAsync<int>(from x in (IQueryable<Order>)c.Order
						where x.FactorType == factorType && (status == null || x.Status == ((bool?)status).Value)
						select x into q
						select q.Id2, token);
					long num2 = sanadStatus.Id2From.Value;
					while (true)
					{
						if (num2 < sanadStatus.Id2To.Value)
						{
							if (!list.Contains((int)num2))
							{
								result = (int)num2;
								break;
							}
							num2++;
							continue;
						}
						result = (int)sanadStatus.Id2From.Value;
						break;
					}
					goto end_IL_0126;
					IL_044b:
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Order>((from x in (IQueryable<Order>)c.Order
						where x.FactorType == factorType && (status == null || x.Status == ((bool?)status).Value)
						orderby x.Id2 descending
						select x).Take(1), token))?.Id2 ?? 0) + 1;
					end_IL_0126:;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 379);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<Guid> GetById2Async(bool? status, CancellationToken token, string tranName, int id2, Document factorType)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Guid result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 394));
					result = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => factorType == x.FactorType && x.Id2 == id2 && (status == null || x.Status == ((bool?)status).Value)), token))?.Guid ?? Guid.Empty;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, $"Id2:{id2}//factorType:{factorType}//tranName:{tranName}", "GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 403);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	[Obsolete("please use another overLoad and pass orderType", true)]
	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad and pass orderType");
	}

	private Task<IQueryable<Order>> GetAllQueryAsync(Acc_ModelContext c, OrderFilter filter, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			try
			{
				filter.Description = filter.Description?.FixString()?.RemoveCharactersForLinkSearch() ?? "";
				IQueryable<Order> source = (IQueryable<Order>)EntityFrameworkQueryableExtensions.Include<Order, ICollection<TaxOrderLog>>((IQueryable<Order>)EntityFrameworkQueryableExtensions.Include<Order, Tafsil>((IQueryable<Order>)c.Order, (Expression<Func<Order, Tafsil>>)((Order q) => q.TafsilItem)), (Expression<Func<Order, ICollection<TaxOrderLog>>>)((Order q) => q.TaxOrderLog));
				List<long> numbers = new List<long>();
				foreach (string item2 in filter?.Numbers?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (!string.IsNullOrEmpty(item2) && item2.ParseToLong(0L) > 0)
					{
						numbers.Add(item2.ParseToLong(0L));
					}
				}
				List<long> list = numbers;
				if (list != null && list.Any())
				{
					source = source.Where((Order p) => numbers.Contains(p.Id2));
				}
				source = source.Where((Order q) => q.FactorType == filter.FactorType);
				if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
				{
					source = source.Where((Order q) => q.TafsilGuid == filter.TafsilGuid);
				}
				if (filter.MoeinGuid.HasValue && filter.MoeinGuid != Guid.Empty)
				{
					source = source.Where((Order q) => q.MoeinGuid == filter.MoeinGuid);
				}
				if (filter.DateFrom.HasValue)
				{
					source = source.Where((Order q) => q.Date >= filter.DateFrom);
				}
				if (filter.DateTo.HasValue)
				{
					source = source.Where((Order q) => q.Date <= filter.DateTo);
				}
				if (filter.IsSent.HasValue)
				{
					source = (filter.IsSent.Value ? source.Where((Order q) => q.HostRefrenced != "") : source.Where((Order q) => q.HostRefrenced == ""));
				}
				if (filter.SumFrom.HasValue)
				{
					source = source.Where((Order q) => (decimal?)q.Sum >= filter.SumFrom);
				}
				if (filter.SumTo.HasValue)
				{
					source = source.Where((Order q) => (decimal?)q.Sum <= filter.SumTo);
				}
				if (filter.DiscountFrom.HasValue)
				{
					source = source.Where((Order q) => q.Discount >= filter.DiscountFrom.Value);
				}
				if (filter.DiscountTo.HasValue)
				{
					source = source.Where((Order q) => q.Discount <= filter.DiscountTo.Value);
				}
				if (filter.TaxFrom.HasValue)
				{
					source = source.Where((Order q) => q.Tax >= filter.TaxFrom.Value);
				}
				if (filter.TaxTo.HasValue)
				{
					source = source.Where((Order q) => q.Tax <= filter.TaxTo.Value);
				}
				if (filter.FareFrom.HasValue)
				{
					source = source.Where((Order q) => q.Keraye >= filter.FareFrom.Value);
				}
				if (filter.FareTo.HasValue)
				{
					source = source.Where((Order q) => q.Keraye <= filter.FareTo.Value);
				}
				if (filter.SelectedOrdersGuid != null)
				{
					source = source?.Where((Order q) => filter.SelectedOrdersGuid.Contains(q.Guid));
				}
				foreach (string word in filter.Description?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (!string.IsNullOrEmpty(word))
					{
						source = source.Where((Order p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%"));
					}
				}
				if (filter.IsTemp.HasValue)
				{
					source = source.Where((Order q) => (bool?)q.IsTemp == filter.IsSent);
				}
				if (filter.Status.HasValue)
				{
					source = source.Where((Order q) => (bool?)q.Status == filter.Status);
				}
				if (filter.Id2From.HasValue)
				{
					source = source.Where((Order q) => (decimal?)(decimal)q.Id2 >= filter.Id2From);
				}
				if (filter.Id2To.HasValue)
				{
					source = source.Where((Order q) => (decimal?)(decimal)q.Id2 <= filter.Id2To);
				}
				decimal? visitorPriceFrom = filter.VisitorPriceFrom;
				if ((visitorPriceFrom.GetValueOrDefault() > default(decimal)) & visitorPriceFrom.HasValue)
				{
					source = source.Where((Order q) => (decimal?)q.TotalBazaryab >= filter.VisitorPriceFrom);
				}
				visitorPriceFrom = filter.VisitorPriceTo;
				if ((visitorPriceFrom.GetValueOrDefault() > default(decimal)) & visitorPriceFrom.HasValue)
				{
					source = source.Where((Order q) => (decimal?)q.TotalBazaryab <= filter.VisitorPriceTo);
				}
				if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					source = source.Where((Order q) => q.UserAccGuid == filter.CashierGuid.Value);
				}
				if ((filter?.ServerStatuses?.Any((ServerStatusForClient p) => p != ServerStatusForClient.None)).GetValueOrDefault())
				{
					source = source?.Where((Order q) => filter.ServerStatuses.Contains(q.ServerStatus));
				}
				if ((filter.VisitorTafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					source = source.Where((Order q) => (q.B1TafsilGuid ?? Guid.Empty) == filter.VisitorTafsilGuid.Value || (q.B2TafsilGuid ?? Guid.Empty) == filter.VisitorTafsilGuid.Value || (q.B3TafsilGuid ?? Guid.Empty) == filter.VisitorTafsilGuid.Value || (q.B4TafsilGuid ?? Guid.Empty) == filter.VisitorTafsilGuid.Value);
				}
				if ((filter.CustomerGroupGuid ?? Guid.Empty) != Guid.Empty)
				{
					source = (IQueryable<Order>)EntityFrameworkQueryableExtensions.ThenInclude<Order, Tafsil, Customer>(EntityFrameworkQueryableExtensions.Include<Order, Tafsil>(source, (Expression<Func<Order, Tafsil>>)((Order order) => order.TafsilItem)), (Expression<Func<Tafsil, Customer>>)((Tafsil tafsil) => tafsil.Customer));
					source = source.Where((Order p) => p.TafsilItem.Customer.GroupGuid == filter.CustomerGroupGuid.Value);
				}
				if (filter.Sources != null && (filter.Sources?.Count ?? 0) > 0)
				{
					Expression<Func<Order, bool>> expression = PredicateBuilder.False<Order>();
					foreach (ENSource item in filter.Sources)
					{
						expression = expression.Or((Order x) => x.Source == item);
					}
					source = source?.Where(expression);
				}
				if (!string.IsNullOrEmpty(filter.HostRefrences))
				{
					source = source.Where((Order p) => p.HostRefrenced.Trim() == filter.HostRefrences.Trim());
				}
				if (filter.SanadAffective.HasValue)
				{
					source = source?.Where((Order q) => q.SanadAffective == filter.SanadAffective.Value);
				}
				if (filter.KardexAffective.HasValue)
				{
					source = source?.Where((Order q) => q.KardexAffective == filter.KardexAffective.Value);
				}
				if ((filter?.IsLast30).GetValueOrDefault())
				{
					source = (source?.OrderByDescending((Order q) => q.Date))?.Take(30)?.OrderBy((Order q) => q.Date);
				}
				if ((filter?.StatusId).GetValueOrDefault() > 0)
				{
					source = source?.Where((Order q) => q.StatusId == filter.StatusId.Value);
				}
				if (!string.IsNullOrEmpty(filter?.SellerOrderNumber))
				{
					source = source?.Where((Order q) => q.SellerFactorNo == filter.SellerOrderNumber);
				}
				if ((filter?.AreaGuidList?.Any()).GetValueOrDefault())
				{
					source = source?.Where((Order q) => filter.AreaGuidList.Contains(q.TafsilItem.Customer.AreaGuid ?? Guid.Empty));
				}
				return Task.FromResult(source);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllQueryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 501);
				throw;
			}
		}
	}

	public virtual async Task<IEnumerable<OrderReportBusiness>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 507));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			IEnumerable<OrderReportBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<Order> queryable = await GetAllQueryAsync(c, filter, token);
					List<OrderReportBusiness> mappedData = await MapFromDatabaseToReport.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
					if (filter.TaxResult.HasValue)
					{
						mappedData = mappedData?.Where((OrderReportBusiness q) => q.TaxResult == filter.TaxResult.Value)?.ToList();
					}
					CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
					if (cashier != null)
					{
						switch (filter.FactorType)
						{
						case Document.OrderSell:
						{
							if (!(cashier.Settings?.OrderSellTemplateGuidList?.Any()).GetValueOrDefault())
							{
								break;
							}
							IEnumerable<OrderReportBusiness> enumerable5 = mappedData?.Where((OrderReportBusiness q) => cashier.Settings.OrderSellTemplateGuidList.Contains(q.Guid));
							if (enumerable5 == null || !enumerable5.Any())
							{
								break;
							}
							foreach (OrderReportBusiness item in enumerable5)
							{
								item.TemplateStatus = true;
							}
							break;
						}
						case Document.OrderBuy:
						{
							if (!(cashier.Settings?.OrderBuyTemplateGuidList?.Any()).GetValueOrDefault())
							{
								break;
							}
							IEnumerable<OrderReportBusiness> enumerable2 = mappedData?.Where((OrderReportBusiness q) => cashier.Settings.OrderBuyTemplateGuidList.Contains(q.Guid));
							if (enumerable2 == null || !enumerable2.Any())
							{
								break;
							}
							foreach (OrderReportBusiness item2 in enumerable2)
							{
								item2.TemplateStatus = true;
							}
							break;
						}
						case Document.OrderBackBuy:
						{
							if (!(cashier.Settings?.OrderBackBuyTemplateGuidList?.Any()).GetValueOrDefault())
							{
								break;
							}
							IEnumerable<OrderReportBusiness> enumerable4 = mappedData?.Where((OrderReportBusiness q) => cashier.Settings.OrderBackBuyTemplateGuidList.Contains(q.Guid));
							if (enumerable4 == null || !enumerable4.Any())
							{
								break;
							}
							foreach (OrderReportBusiness item3 in enumerable4)
							{
								item3.TemplateStatus = true;
							}
							break;
						}
						case Document.OrderBackSell:
						{
							if (!(cashier.Settings?.OrderBackSellTemplateGuidList?.Any()).GetValueOrDefault())
							{
								break;
							}
							IEnumerable<OrderReportBusiness> enumerable3 = mappedData?.Where((OrderReportBusiness q) => cashier.Settings.OrderBackSellTemplateGuidList.Contains(q.Guid));
							if (enumerable3 == null || !enumerable3.Any())
							{
								break;
							}
							foreach (OrderReportBusiness item4 in enumerable3)
							{
								item4.TemplateStatus = true;
							}
							break;
						}
						case Document.PishFactor:
						{
							if (!(cashier.Settings?.PishFatorTemplateGuidList?.Any()).GetValueOrDefault())
							{
								break;
							}
							IEnumerable<OrderReportBusiness> enumerable = mappedData?.Where((OrderReportBusiness q) => cashier.Settings.PishFatorTemplateGuidList.Contains(q.Guid));
							if (enumerable == null || !enumerable.Any())
							{
								break;
							}
							foreach (OrderReportBusiness item5 in enumerable)
							{
								item5.TemplateStatus = true;
							}
							break;
						}
						}
					}
					result = mappedData;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 576);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 592));
				Order order = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)dbcontext.Order, (Expression<Func<Order, bool>>)((Order x) => x.Guid == item.Guid), token);
				if (order == null)
				{
					return ret;
				}
				order.HostRefrenced = item.HostRefrenced;
				order.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				order.DeliveryMessage = item.Message;
				order.DeliveryShortMessage = item.Shortmessage;
				order.IsTemp = !item.SaveResultStatus;
				((EntityEntry)((DbContext)dbcontext).Entry<Order>(order)).State = (EntityState)3;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 605);
				ret.AddReturnedValue(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 606);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 619));
				Order order = ((IQueryable<Order>)acc_ModelContext.Order).Where((Order x) => x.Guid == item.Guid)?.FirstOrDefault();
				if (order == null)
				{
					return ret.AddInformation("فاکتور با شناسه".GetTranslate() + " " + $"{item.Guid}" + " " + " یافت نشد.".GetTranslate(), "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 621);
				}
				if (item.DeliveryStatus)
				{
					order.ServerStatus = (string.IsNullOrEmpty(order.HostRefrenced) ? ServerStatusForClient.DirectDelivery : ServerStatusForClient.Delivered);
				}
				else
				{
					order.ServerStatus = ServerStatusForClient.DeliveryError;
				}
				order.DeliveryDate = item.DeliveryDate;
				order.DeliveryId = item.DeliveryId;
				order.DeliveryMessage = item.Message;
				order.DeliveryShortMessage = item.ShortMessage;
				if (!item.DeliveryStatus)
				{
					order.IsTemp = true;
				}
				((DbContext)acc_ModelContext).Update<Order>(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 639);
				ret.AddReturnedValue(ex2, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 640);
			}
			return ret;
		}
	}

	public async Task<List<OrderBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 646));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			List<OrderBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<OrderBusiness> ret = new List<OrderBusiness>();
					List<Order> list = await EntityFrameworkQueryableExtensions.ToListAsync<Order>(((IQueryable<Order>)c.Order).Where((Order e) => e.ServerStatus == ServerStatusForClient.None && !e.IsTemp), default(CancellationToken));
					if (list != null && list.Count > 0)
					{
						foreach (Order item in list)
						{
							if (item != null)
							{
								List<OrderBusiness> list2 = ret;
								list2.Add(await GetAsync(item.Guid, token, tranName));
							}
						}
					}
					result = ret;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 663);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetAsyncByReceptionGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 673));
		using (new JaegerMonitor("GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					Order res = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.ReceptionGuid == guid), token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						OrderBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, res, null, tranName, token);
						if (ret == null)
						{
							result = null;
						}
						else
						{
							await orderDetRepo.GetAllAsync(ret, token, tranName);
							ret.SetKeraye(res.Keraye);
							OrderBusiness orderBusiness = ret;
							await orderBusiness.AddRangeToListSanadAsync(await orderSanadDetRepo.GetAllAsync(ret.Guid, ret.FactorType, null, null, token, tranName), EnSystem.Mapper, token);
							result = ret;
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 692);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsyncByReceptionGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetLastOrderAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			int line = 0;
			OrderBusiness result;
			try
			{
				_ = 5;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 707));
					line = 1;
					Order order = ((IQueryable<Order>)c.Order).Where((Order q) => q.TafsilGuid == tafsilGuid && q.FactorType == orderType)?.OrderByDescending((Order q) => q.Date)?.FirstOrDefault();
					line = 2;
					if (order == null)
					{
						result = null;
					}
					else
					{
						line = 3;
						OrderBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, order, null, tranName, token);
						line = 4;
						if (ret == null)
						{
							result = null;
						}
						else
						{
							line = 5;
							OrderBusiness orderBusiness = ret;
							await orderBusiness.AddDetListAsync(await orderDetRepo.GetAllAsync(ret.Guid, token, tranName), clearDets: true, EnSystem.Mapper, token);
							line = 6;
							orderBusiness = ret;
							await orderBusiness.AddRangeToListSanadAsync(await orderSanadDetRepo.GetAllAsync(ret.Guid, ret.FactorType, null, null, token, tranName), EnSystem.Mapper, token);
							line = 7;
							result = ret;
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "(Guid tafsilGuid, Document orderType, string tranName)\r\n" + $"#Line_{line}", "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 727);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetAsyncByPardakhtGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderBusiness result;
			try
			{
				_ = 5;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 742));
					Order res = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.PardakhtGuid == guid), token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						OrderBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, res, null, tranName, token);
						if (ret == null)
						{
							result = null;
						}
						else
						{
							await orderDetRepo.GetAllAsync(ret, token, tranName);
							ret.SetKeraye(res.Keraye);
							OrderBusiness orderBusiness = ret;
							await orderBusiness.AddRangeToListSanadAsync(await orderSanadDetRepo.GetAllAsync(ret.Guid, ret.FactorType, null, null, token, tranName), EnSystem.Mapper, token);
							result = ret;
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 756);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsyncByPardakhtGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<OrderReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 766));
		using (new JaegerMonitor("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (token.IsCancellationRequested || _isDisposed)
					{
						result = null;
					}
					else
					{
						Order order = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.Guid == guid), token);
						result = ((order != null) ? (await MapFromDatabaseToReport.MapAsync(ContextGuid, order, null, tranName, token)) : null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 781);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<IEnumerable<OrderReportBusiness>> GetAllCustomerLastTransactionAsync(Document? factorType, CancellationToken token, string tranName);

	public Task Add2CacheAsync(OrderBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<OrderBusiness> GetFromDbAsync(Guid guid, Document factortype, CancellationToken token, string tranName)
	{
		return GetAsync(guid, factortype, token, tranName);
	}

	public async Task<List<OrderBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 797));
		using (new JaegerMonitor("GetAllAsync", $"PersistenceNoGuidRepository.GetAll_{typeof(OrderBusiness)}"))
		{
			List<OrderBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, (IEnumerable<Order>)((DbContext)c).Set<Order>(), null, tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 808);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<OrderBusiness>> GetAllAsyncSearchAsync(string search, CancellationToken token, short tasvieshode, short printshode, bool? status, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsyncSearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 818));
		using (new JaegerMonitor("GetAllAsyncSearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			IEnumerable<OrderBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<Order> queryable = (IQueryable<Order>)c.Order;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable.Where((Order p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.BazaryabFullName, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.CustAddress, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.CustomerFullName, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.CustMobile, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							queryable = queryable.Where((Order p) => p.Status == ((bool?)status).Value);
						}
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else if (queryable == null || !queryable.Any())
						{
							result = null;
						}
						else
						{
							IItemAsyncMapper<Order, OrderBusiness> mapFromDatabase = MapFromDatabase;
							string contextGuid = ContextGuid;
							result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Order>(queryable, token), null, tranName, token);
						}
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsyncSearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 843);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsyncSearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetFirstOrderAsync(Document factorType, DateTime fromDate, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 854));
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Order> order = (IQueryable<Order>)c.Order;
					order = order.Where((Order p) => p.FactorType == factorType && p.Date >= fromDate && p.Status);
					if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						order = order.Where((Order p) => p.UserAccGuid == ((Guid?)cashierGuid).Value);
					}
					Guid guid = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)order.OrderBy((Order q) => q.Date), token))?.Guid ?? Guid.Empty;
					result = ((!(guid == Guid.Empty)) ? (await GetAsync(guid, token, tranName)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 870);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetFirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetPreviewsOrderAsync(Document factorType, Guid orderGuid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 882));
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					OrderBusiness currentOrder = await GetAsync(orderGuid, factorType, token, tranName);
					if (currentOrder == null)
					{
						throw new NullReferenceException("current Ordr is null!!");
					}
					DateTime currentOrderDate = currentOrder.GetDate();
					IQueryable<Order> order = (IQueryable<Order>)c.Order;
					order = order.Where((Order p) => p.FactorType == factorType && p.Date <= currentOrderDate && p.Guid != currentOrder.Guid && p.Status);
					if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						order = order.Where((Order p) => p.UserAccGuid == ((Guid?)cashierGuid).Value);
					}
					Guid guid = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)order.OrderByDescending((Order q) => q.Date), token))?.Guid ?? Guid.Empty;
					result = ((!(guid == Guid.Empty)) ? (await GetAsync(guid, currentOrder.FactorType, token, tranName)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 902);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetPreviewsOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetNextOrderAsync(Document factorType, Guid orderGuid, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 910));
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					OrderBusiness currentOrder = await GetAsync(orderGuid, factorType, token, tranName);
					if (currentOrder == null)
					{
						throw new NullReferenceException("current Ordr is null!!");
					}
					DateTime currentOrderDate = currentOrder.GetDate();
					IQueryable<Order> order = (IQueryable<Order>)c.Order;
					order = order.Where((Order p) => p.FactorType == factorType && p.Date >= currentOrderDate && p.Guid != currentOrder.Guid && p.Status);
					if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						order = order.Where((Order p) => p.UserAccGuid == ((Guid?)cashierGuid).Value);
					}
					Guid guid = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)order.OrderBy((Order q) => q.Date), token))?.Guid ?? Guid.Empty;
					result = ((!(guid == Guid.Empty)) ? (await GetAsync(guid, currentOrder.FactorType, token, tranName)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 929);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderBusiness> GetLastOrderAsync(Document factorType, DateTime toDate, Guid? cashierGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 940));
		using (new JaegerMonitor("GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Order> order = (IQueryable<Order>)c.Order;
					order = order.Where((Order p) => p.FactorType == factorType && p.Date <= toDate && p.Status);
					if ((cashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						order = order.Where((Order p) => p.UserAccGuid == ((Guid?)cashierGuid).Value);
					}
					Guid guid = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)order.OrderByDescending((Order q) => q.Date), token))?.Guid ?? Guid.Empty;
					result = ((!(guid == Guid.Empty)) ? (await GetAsync(guid, factorType, token, tranName)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 957);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<decimal> TotalSumOfSales(Guid tafsilGuid, DateTime date, Document orderType, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 969));
			decimal result;
			try
			{
				result = ((IQueryable<Order>)acc_ModelContext.Order).Where((Order w) => w.TafsilGuid == tafsilGuid && w.Date > date && w.FactorType == orderType)?.Sum((Order q) => q.TotalSum) ?? 0m;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 979);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("TotalSumOfSales", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<Guid>> GetNotTransferedAsync(Document factorType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 990));
		using (new JaegerMonitor("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			List<Guid> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Order> order = (IQueryable<Order>)c.Order;
					order = order.Where((Order p) => p.FactorType == factorType && p.TransferDate == null);
					result = ((order != null && order.Any()) ? (await ((order != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Guid>(order.Select((Order q) => q.Guid), token) : null)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1003);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1015));
				IQueryable<Order> queryable = ((IQueryable<Order>)context.Order).Where((Order x) => x.Guid == filter.ObjectGuid && x.FactorType == filter.ObjectType);
				Order order = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>(queryable, token) : null);
				order.TransferDate = filter.TransferDate;
				((DbContext)context).Update<Order>(order);
				return ret;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1025);
				throw;
			}
		}
	}

	public async Task<int> NextAttachmentNoAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1033));
		using (new JaegerMonitor("NextAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Order>((from x in (IQueryable<Order>)c.Order
						where x.FactorType == Document.OrderSell
						orderby x.AttachmentNo descending
						select x).Take(1), token))?.AttachmentNo ?? 0) + 1;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "NextAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1049);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAttachmentNoAsync(Guid orderGuid, int attachmentNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1065));
				IQueryable<Order> queryable = ((IQueryable<Order>)dbcontext.Order).Where((Order x) => x.Guid == orderGuid);
				Order order = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>(queryable, token) : null);
				if (order == null)
				{
					return ret;
				}
				order.AttachmentNo = attachmentNo;
				((EntityEntry)((DbContext)dbcontext).Entry<Order>(order)).State = (EntityState)3;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1076);
				ret.AddReturnedValue(ex3, "", "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1077);
			}
			return ret;
		}
	}

	public async Task<Guid> GetDepotGuidAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1084));
			Guid result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<OrderDetail>((IQueryable<OrderDetail>)c.OrderDet, (Expression<Func<OrderDetail, bool>>)((OrderDetail x) => x.Guid == guid), token))?.DepotGuid ?? Guid.Empty;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1096);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderReportBusiness> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetLastOrderByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastOrderByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1108));
			int line = 0;
			OrderReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Order> queryable = ((IQueryable<Order>)c.Order).Where((Order x) => x.TafsilGuid == tafsilGuid && x.FactorType == orderType);
					line = 1;
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						line = 2;
						queryable = queryable?.OrderByDescending((Order q) => q.Date);
						line = 3;
						IItemAsyncMapper<Order, OrderReportBusiness> mapFromDatabaseToReport = MapFromDatabaseToReport;
						string contextGuid = ContextGuid;
						OrderReportBusiness orderReportBusiness = await mapFromDatabaseToReport.MapAsync(contextGuid, await EntityFrameworkQueryableExtensions.FirstAsync<Order>(queryable, token), new OrderReportBusiness(ContextGuid), tranName, token);
						line = 4;
						result = orderReportBusiness;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, $"#Line_{line}", "GetLastOrderByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1127);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastOrderByTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<short> GetNextDailyFactorNoAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1138));
		using (new JaegerMonitor("GetNextDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			short result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<Order> order = c.Order;
					object obj;
					if (order == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<Order> queryable = ((IQueryable<Order>)order).Where((Order q) => q.Date >= dateM);
						if (queryable == null)
						{
							obj = null;
						}
						else
						{
							IOrderedQueryable<Order> orderedQueryable = queryable.OrderByDescending((Order x) => x.DailyFactorNo);
							if (orderedQueryable == null)
							{
								obj = null;
							}
							else
							{
								IQueryable<Order> queryable2 = orderedQueryable.Take(1);
								obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Order>(queryable2, token) : null);
							}
						}
					}
					Order rec = await (Task<Order>)obj;
					if (rec != null && rec.DailyFactorNo != 0)
					{
						goto IL_0352;
					}
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
					if ((accGlobalSettings?.Factors?.FishNoStartIndex).GetValueOrDefault() <= 0)
					{
						if (rec != null)
						{
							rec.DailyFactorNo = 0;
						}
						goto IL_0352;
					}
					result = (short)accGlobalSettings.Factors.FishNoStartIndex;
					goto end_IL_010c;
					IL_0352:
					int num2 = (rec?.DailyFactorNo ?? 0) + 1;
					result = (short)num2;
					end_IL_010c:;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1158);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<IEnumerable<OrderNotPayedViewModel>> GetAllOrderNotPayedAsync(DateTime orderDate, Guid tafsilGuid, Guid orderGuid, CancellationToken token, string tranName);

	public async Task<ReturnedSaveFuncInfo> UpdateCustomerNameAsync(OrderBusiness item, string name, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		using (new JaegerMonitor("UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1177));
				Order order = ((IQueryable<Order>)acc_ModelContext.Order).Where((Order x) => x.Guid == item.Guid)?.FirstOrDefault();
				if (order == null)
				{
					return ret;
				}
				order.CustomerFullName = name;
				((EntityEntry)((DbContext)acc_ModelContext).Entry<Order>(order)).State = (EntityState)3;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1186);
				ret.AddReturnedValue(ex2, "", "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1187);
			}
			return ret;
		}
	}

	public async Task<decimal> GetVisitorSumPriceAsync(OrderFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetVisitorSumPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetVisitorSumPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1194));
			decimal result;
			try
			{
				_ = 1;
				try
				{
					result = (await GetAllQueryAsync(c, filter, token))?.Sum((Order q) => q.TotalSum) ?? 0m;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetVisitorSumPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1204);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetVisitorSumPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(bool value, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string serial, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(Document factorType, bool kardexAffective, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<OrderTtmsReport>> GetOrderTtmsReportAsync(List<Guid> orderGuidList, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetOrderTtmsReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetOrderTtmsReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1221));
			IEnumerable<OrderTtmsReport> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Order> queryable = (IQueryable<Order>)c.Order;
					if (orderGuidList?.Any() ?? false)
					{
						queryable = queryable.Where((Order p) => orderGuidList.Contains(p.Guid));
					}
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<OrderTtmsReport>(queryable.Select((Order p) => new OrderTtmsReport
					{
						FactorType = p.FactorType,
						Guid = p.Guid,
						ProductSumWithTax = p.OrderDetailItem.Where((OrderDetail q) => q.Tax != 0m && q.ProductItem.Type == EnProductType.Product).Sum((OrderDetail q) => q.RowSum),
						ProductSumWithoutTax = p.OrderDetailItem.Where((OrderDetail q) => q.Tax == 0m && q.ProductItem.Type == EnProductType.Product).Sum((OrderDetail q) => q.RowSum),
						ServiceSumWithTax = p.OrderDetailItem.Where((OrderDetail q) => q.Tax != 0m && q.ProductItem.Type == EnProductType.Service).Sum((OrderDetail q) => q.RowSum),
						ServiceSumWithoutTax = p.OrderDetailItem.Where((OrderDetail q) => q.Tax == 0m && q.ProductItem.Type == EnProductType.Service).Sum((OrderDetail q) => q.RowSum),
						ProductTax = p.OrderDetailItem.Where((OrderDetail q) => q.ProductItem.Type == EnProductType.Product).Sum((OrderDetail q) => q.Tax + q.Avarez),
						ServiceTax = p.OrderDetailItem.Where((OrderDetail q) => q.ProductItem.Type == EnProductType.Service).Sum((OrderDetail q) => q.Tax + q.Avarez)
					}), token)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetOrderTtmsReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1243);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetOrderTtmsReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<OrderReportBusiness> GetReportById2Async(int id2, Document type, bool status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1254));
		using (new JaegerMonitor("GetReportById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			OrderReportBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (token.IsCancellationRequested || _isDisposed)
					{
						result = null;
					}
					else
					{
						Order order = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Order>((IQueryable<Order>)c.Order, (Expression<Func<Order, bool>>)((Order x) => x.Id2 == id2 && x.FactorType == type && x.Status == status), token);
						result = ((order != null) ? (await MapFromDatabaseToReport.MapAsync(ContextGuid, order, null, tranName, token)) : null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetReportById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1269);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetReportById2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid orderGuid, int statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1285));
				Order order = await context.Order.FindAsync(new object[1] { orderGuid });
				if (order == null)
				{
					return res;
				}
				order.StatusId = (short)statusId;
				order.Modified = DateTime.Now;
				((DbContext)context).Update<Order>(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1295);
				res.AddReturnedValue(ex2, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1296);
			}
			return res;
		}
	}

	public async Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1302));
		List<ENSource> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ENSource> queryable = ((IQueryable<Order>)c.Order).Select((Order q) => q.Source);
				object obj;
				if (queryable == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<ENSource> queryable2 = queryable.Distinct();
					obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ENSource>(queryable2, token) : null);
				}
				result = await (Task<List<ENSource>>)obj;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1310);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<List<OrderReportBusiness>> GetAllMoghayerat50Async(List<Guid> kolGuidList, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllMoghayerat50Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1321));
		using (new JaegerMonitor("GetAllMoghayerat50Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			List<OrderReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					if (token.IsCancellationRequested || _isDisposed || !(kolGuidList?.Any() ?? false))
					{
						result = null;
					}
					else
					{
						IQueryable<Order> queryable = ((IQueryable<Order>)EntityFrameworkQueryableExtensions.Include<Order, Moein>((IQueryable<Order>)c.Order, (Expression<Func<Order, Moein>>)((Order q) => q.MoeinItem))).Where((Order x) => x.Status && kolGuidList.Contains(x.MoeinItem.KolGuid) && (x.FactorType == Document.OrderSell || x.FactorType == Document.OrderBuy || x.FactorType == Document.OrderBackSell || x.FactorType == Document.OrderBackBuy));
						result = ((queryable != null && queryable.Any()) ? (await MapFromDatabaseToReport.MapListAsync(ContextGuid, queryable, null, tranName, token)) : null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllMoghayerat50Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1338);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllMoghayerat50Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<OrdersForUpdateCashBackModel>> GetAllForUpdateCashBackAsync(DateTime? startDate, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForUpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1349));
		using (new JaegerMonitor("GetAllForUpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			IEnumerable<OrdersForUpdateCashBackModel> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Order> queryable = ((IQueryable<Order>)c.Order).Where((Order x) => x.Status && x.FactorType == Document.OrderSell && !x.IsCashBackProgressed && (startDate == null || x.Date >= ((DateTime?)startDate).Value));
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IQueryable<OrdersForUpdateCashBackModel> queryable2 = queryable.Select((Order q) => new OrdersForUpdateCashBackModel
						{
							OrderGuid = q.Guid
						});
						result = await ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<OrdersForUpdateCashBackModel>(queryable2, token) : null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllForUpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1365);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllForUpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateCashBackAsync(Guid orderGuid, Guid? cashBackGuid, bool isProgressed, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if ((cashBackGuid ?? Guid.Empty) == Guid.Empty)
				{
					cashBackGuid = null;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1383));
				Order order = await dbcontext.Order.FindAsync(new object[1] { orderGuid });
				order.Modified = DateTime.Now;
				order.ServerStatus = ServerStatusForClient.None;
				order.CashBackCouponGuid = cashBackGuid;
				order.IsCashBackProgressed = isProgressed;
				dbcontext.Order.Update(order);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1394);
				ret.AddReturnedValue(ex2, "", "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderPersistenceRepositoryBase.cs", 1395);
			}
			return ret;
		}
	}

	public virtual Task<decimal> GetRelatedBackSellSumAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
