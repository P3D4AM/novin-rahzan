using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Cache.Repository;
using EntityCache.Cache.Repository.Accounting;
using EntityCache.Cache.Repository.Accounting.Aghsat;
using EntityCache.Cache.Repository.Accounting.Bill;
using EntityCache.Cache.Repository.Accounting.Cheque;
using EntityCache.Cache.Repository.Accounting.Coding;
using EntityCache.Cache.Repository.Accounting.DepotReceipt;
using EntityCache.Cache.Repository.Accounting.Details;
using EntityCache.Cache.Repository.Accounting.DiscountCoupon;
using EntityCache.Cache.Repository.Accounting.Factor;
using EntityCache.Cache.Repository.Accounting.Gift;
using EntityCache.Cache.Repository.Accounting.Hotel;
using EntityCache.Cache.Repository.Accounting.Khazane.Pardakht;
using EntityCache.Cache.Repository.Accounting.Khazane.Reception;
using EntityCache.Cache.Repository.Accounting.Note;
using EntityCache.Cache.Repository.Accounting.Order;
using EntityCache.Cache.Repository.Accounting.Reservation;
using EntityCache.Cache.Repository.Accounting.Sanad;
using EntityCache.Cache.Repository.Accounting.Selection;
using EntityCache.Cache.Repository.Accounting.TTMS;
using EntityCache.Cache.Repository.Accounting.Tax;
using EntityCache.Cache.Repository.Accounting.Tolid;
using EntityCache.Cache.Repository.Accounting.TozieList;
using EntityCache.Cache.Repository.Advertise;
using EntityCache.Cache.Repository.Attendance;
using EntityCache.Cache.Repository.Attendance.Biometric;
using EntityCache.Cache.Repository.Crm;
using EntityCache.Cache.Repository.Customer;
using EntityCache.Cache.Repository.Department;
using EntityCache.Cache.Repository.Global;
using EntityCache.Cache.Repository.Global.Tasks;
using EntityCache.Cache.Repository.Indexes;
using EntityCache.Cache.Repository.Payamak;
using EntityCache.Cache.Repository.Report;
using EntityCache.Cache.Repository.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Persistence.Repository.Indexes;
using PacketParser.Entities;

namespace EntityCache;

public class AggregatorManager
{
	public static AggregateSqlRepository Initialize(AggregateSqlRepository aggregatePersistenceRepository, IEnumerable<IDatabaseManager> databasemanager, int cachingTimeSeconds, WebContextHolder context)
	{
		AggregateSqlRepository aggregateSqlRepository = new AggregateSqlRepository(context);
		try
		{
			IDatabaseManager databaseManager = databasemanager?.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Accounting);
			IDatabaseManager? obj = databasemanager?.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Advertise);
			IDatabaseManager? obj2 = databasemanager?.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Global);
			IDatabaseManager databaseManager2 = databasemanager?.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Attendance);
			IDatabaseManager databaseManager3 = databasemanager?.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Department);
			aggregateSqlRepository.DatabaseManager = databasemanager;
			if (databaseManager != null)
			{
				InitializeAccountingContext(aggregateSqlRepository, aggregatePersistenceRepository, context, cachingTimeSeconds);
			}
			if (obj2 != null)
			{
				InitializeGlobalContext(aggregateSqlRepository, aggregatePersistenceRepository, context, cachingTimeSeconds);
			}
			if (obj != null)
			{
				InitializeAdvertiseContext(aggregateSqlRepository, aggregatePersistenceRepository, context, cachingTimeSeconds);
			}
			if (databaseManager2 != null)
			{
				InitializeAttendanceContext(aggregateSqlRepository, aggregatePersistenceRepository, context, cachingTimeSeconds);
			}
			if (databaseManager3 != null)
			{
				InitializeDepratmentContext(aggregateSqlRepository, aggregatePersistenceRepository, context, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Initialize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 68);
		}
		return aggregateSqlRepository;
	}

	internal static void InitializeAccountingContext(AggregateSqlRepository ret, AggregateSqlRepository aggregatePersistenceRepository, WebContextHolder context, int cachingTimeSeconds)
	{
		int num = 0;
		try
		{
			num = 1;
			ret.IIxProductAllFiels = new IxProductAllFielsCacheRepository(aggregatePersistenceRepository.IIxProductAllFiels, 60);
			num = 2;
			ret.IIxProductNameAndCodeFieldRepository = new IxProductNameAndCodeFieldCacheRepository(aggregatePersistenceRepository.IIxProductNameAndCodeFieldRepository, 60);
			num = 3;
			ret.IIxProductNameFieldRepository = new IxProductNameFieldCacheRepository(aggregatePersistenceRepository.IIxProductNameFieldRepository, 60);
			num = 4;
			ret.IIxProductCodeFieldRepository = new IxProductCodeFieldCacheRepository(aggregatePersistenceRepository.IIxProductCodeFieldRepository, 60);
			num = 5;
			ret.IIxProductDescFieldRepository = new IxProductDescFieldCacheRepository(aggregatePersistenceRepository.IIxProductDescFieldRepository, 60);
			num = 6;
			ret.IIxProductT1FieldRepository = new IxProductT1FieldCacheRepository(aggregatePersistenceRepository.IIxProductT1FieldRepository, 60);
			num = 7;
			ret.IIxProductT2FieldRepository = new IxProductT2FieldCacheRepository(aggregatePersistenceRepository.IIxProductT2FieldRepository, 60);
			num = 8;
			ret.IIxProductT3FieldRepository = new IxProductT3FieldCacheRepository(aggregatePersistenceRepository.IIxProductT3FieldRepository, 60);
			num = 9;
			ret.IIxProductT4FieldRepository = new IxProductT4FieldCacheRepository(aggregatePersistenceRepository.IIxProductT4FieldRepository, 60);
			num = 10;
			ret.IIxProductT5FieldRepository = new IxProductT5FieldCacheRepository(aggregatePersistenceRepository.IIxProductT5FieldRepository, 60);
			num = 11;
			ret.IIxProductT6FieldRepository = new IxProductT6FieldCacheRepository(aggregatePersistenceRepository.IIxProductT6FieldRepository, 60);
			num = 12;
			ret.IIxProductT7FieldRepository = new IxProductT7FieldCacheRepository(aggregatePersistenceRepository.IIxProductT7FieldRepository, 60);
			num = 13;
			ret.IIxProductT8FieldRepository = new IxProductT8FieldCacheRepository(aggregatePersistenceRepository.IIxProductT8FieldRepository, 60);
			ret.CustomerReports = new CustomerReportsCacheRepository(aggregatePersistenceRepository.CustomerReports, 60);
			ret.Reports = new ReportCacheRepository(aggregatePersistenceRepository.Reports, 60);
			num = 14;
			ret.IKol = new KolCacheRepository(aggregatePersistenceRepository.IKol, cachingTimeSeconds);
			num = 15;
			ret.IMoein = new MoeinCacheRepository(aggregatePersistenceRepository.IMoein, cachingTimeSeconds);
			num = 16;
			ret.ITafsil = new TafsilCacheRepository(aggregatePersistenceRepository.ITafsil, cachingTimeSeconds);
			num = 17;
			ret.IGardeshTafil = new GardeshTafsilCacheRepository(aggregatePersistenceRepository.IGardeshTafil, cachingTimeSeconds);
			num = 18;
			CustomerCompactCacheRepository customerCompactCacheRepository = new CustomerCompactCacheRepository(aggregatePersistenceRepository.ICustomerCompact, cachingTimeSeconds);
			num = 19;
			ret.ICustomerCompact = customerCompactCacheRepository;
			num = 20;
			ret.ICustomer = new CustomerCacheRepository(customerCompactCacheRepository, aggregatePersistenceRepository.ICustomer, cachingTimeSeconds);
			num = 21;
			ret.ICustomerAddress = new CustomerAddressCacheRepository(aggregatePersistenceRepository.ICustomerAddress, cachingTimeSeconds);
			num = 22;
			ret.ICustomerTitle = new CustomerTitleCacheRepository(aggregatePersistenceRepository.ICustomerTitle, cachingTimeSeconds);
			num = 23;
			ret.ICustomerGroup = new CustomerGroupCacheRepository(aggregatePersistenceRepository.ICustomerGroup, cachingTimeSeconds);
			num = 24;
			ret.IOrderDetail = new OrderDetailCacheRepository(aggregatePersistenceRepository.IOrderDetail, cachingTimeSeconds);
			num = 25;
			ret.ISanadDet = new Sanad_DetCacheRepository(aggregatePersistenceRepository.ISanadDet, cachingTimeSeconds);
			num = 26;
			ret.IOrder = new OrderCacheRepository(aggregatePersistenceRepository.IOrder, aggregatePersistenceRepository.IOrderDetail, aggregatePersistenceRepository.ISanadDet, cachingTimeSeconds);
			num = 27;
			ret.IPrdAnbarRelation = aggregatePersistenceRepository.IPrdAnbarRelation;
			num = 29;
			ret.ProductAttributeRelationRepository = aggregatePersistenceRepository.ProductAttributeRelationRepository;
			num = 30;
			ProductCompactCacheRepository productCompactCacheRepository = new ProductCompactCacheRepository(aggregatePersistenceRepository.IProductCompact, cachingTimeSeconds);
			num = 31;
			ret.IProductCompact = productCompactCacheRepository;
			num = 32;
			ret.IProduct = new ProductCacheRepository(productCompactCacheRepository, aggregatePersistenceRepository.IProduct, ret.IPrdAnbarRelation, cachingTimeSeconds);
			num = 33;
			ret.IProductGroup = new PrdGroupCacheRepository(aggregatePersistenceRepository.IProductGroup, cachingTimeSeconds);
			num = 34;
			ret.IProductVahed = new ProductVahedCacheRepository(aggregatePersistenceRepository.IProductVahed, cachingTimeSeconds);
			num = 35;
			ret.IReception = new ReceptionCacheRepository(aggregatePersistenceRepository.IReception, cachingTimeSeconds);
			num = 36;
			ret.IPardakht = new PardakhtCacheRepository(aggregatePersistenceRepository.IPardakht, cachingTimeSeconds);
			num = 37;
			ret.IDepot = new DepotCacheRepository(aggregatePersistenceRepository.IDepot, cachingTimeSeconds);
			num = 38;
			ret.IGps = new GpsCacheRepository(aggregatePersistenceRepository.IGps, cachingTimeSeconds);
			num = 39;
			IFullTextWordRepository dbRepository = (ret.IFullTextWordRepository = aggregatePersistenceRepository.IFullTextWordRepository);
			ret.IFullTextWordRepository = new FullTextWordCacheRepository(dbRepository, cachingTimeSeconds);
			num = 40;
			ret.ISuggestedBank = new SuggestedBankCacheRepository(aggregatePersistenceRepository.ISuggestedBank, cachingTimeSeconds);
			num = 41;
			ret.IVisit = new VisitCacheRepository(aggregatePersistenceRepository.IVisit, cachingTimeSeconds);
			num = 42;
			ret.IBarcode = new BarcodeCacheRepository(aggregatePersistenceRepository.IBarcode, cachingTimeSeconds);
			num = 43;
			ret.IUserAcc = new UserAccCacheRepository(aggregatePersistenceRepository.IUserAcc, cachingTimeSeconds);
			num = 44;
			ret.ITafsilGroup = new TafsilGroupCacheRepository(aggregatePersistenceRepository.ITafsilGroup, cachingTimeSeconds);
			num = 45;
			ret.ISanad = new SanadCacheRepository(aggregatePersistenceRepository.ISanad, cachingTimeSeconds);
			num = 46;
			ret.IPrdTechnical = new PrdTechnicalCacheRepository(aggregatePersistenceRepository.IPrdTechnical, cachingTimeSeconds);
			num = 47;
			ret.Ikardex = new KardexCacheRepository(aggregatePersistenceRepository.Ikardex, cachingTimeSeconds);
			num = 48;
			ret.IMoeinTafsilGroupRelations = new MoeinTafsilGroupRelationsCacheRepository(aggregatePersistenceRepository.IMoeinTafsilGroupRelations, cachingTimeSeconds);
			num = 49;
			ret.IReceptionCheckRepository = new ReceptionCheckCacheRepository(aggregatePersistenceRepository.IReceptionCheckRepository, cachingTimeSeconds);
			num = 50;
			ret.IReceptionHavale = new ReceptionHavaleCacheRepository(aggregatePersistenceRepository.IReceptionHavale, cachingTimeSeconds);
			num = 51;
			ret.ICASHIER = new CashierCacheRepository(aggregatePersistenceRepository.ICASHIER, cachingTimeSeconds);
			num = 52;
			ret.IFareOrderCredit = new FareOrderCreditCacheRepository(aggregatePersistenceRepository.IFareOrderCredit, cachingTimeSeconds);
			num = 53;
			ret.ISanaDPrint = new SanadPrintCacheRepository(aggregatePersistenceRepository.ISanaDPrint, cachingTimeSeconds);
			num = 54;
			ret.IPrdSerial = new PrdSerialCacheRepository(aggregatePersistenceRepository.IPrdSerial, cachingTimeSeconds);
			ret.IPrdSerialDetail = new PrdSerialDetailCacheRepository(aggregatePersistenceRepository.IPrdSerialDetail, cachingTimeSeconds);
			num = 55;
			ret.ITafsilNumberRepository = new TafsilNumberCacheRepository(aggregatePersistenceRepository.ITafsilNumberRepository, cachingTimeSeconds);
			num = 56;
			ret.IHesabGroup = new HesabGroupCacheRepository(aggregatePersistenceRepository.IHesabGroup, cachingTimeSeconds);
			num = 57;
			ret.IABAD = new AbadDetCacheRepository(aggregatePersistenceRepository.IABAD, cachingTimeSeconds);
			num = 58;
			ret.IOrderAbad = new OrderAbadCacheRepository(aggregatePersistenceRepository.IOrderAbad, cachingTimeSeconds);
			num = 59;
			ret.IPrdAnbarGardani = new PrdAnbarGardaniCacheRepository(aggregatePersistenceRepository.IPrdAnbarGardani, cachingTimeSeconds);
			num = 60;
			ret.IDepEnteghal = new DepEnteghalCacheRepository(aggregatePersistenceRepository.IDepEnteghal, cachingTimeSeconds);
			num = 65;
			ret.ChequeMOperation = new ChequeMOperationCacheRepository(aggregatePersistenceRepository.ChequeMOperation, cachingTimeSeconds);
			num = 69;
			ret.ChequeShOperation = new ChequeShOperationCacheRepository(aggregatePersistenceRepository.ChequeShOperation, cachingTimeSeconds);
			num = 70;
			ret.ICusCard = new CusCardCacheRepository(aggregatePersistenceRepository.ICusCard, cachingTimeSeconds);
			num = 71;
			ret.IPardakhtCheckM = new PardakhtCheckMCacheRepository(aggregatePersistenceRepository.IPardakhtCheckM, cachingTimeSeconds);
			num = 72;
			ret.IPardakhtCheckSh = new PardakhtCheckShCacheRepository(aggregatePersistenceRepository.IPardakhtCheckSh, cachingTimeSeconds);
			num = 73;
			ret.IChequeBook = new ChequeBookCacheRepository(aggregatePersistenceRepository.IChequeBook, cachingTimeSeconds);
			num = 74;
			ret.IRasGiri = new RasGiriCacheRepository(aggregatePersistenceRepository.IRasGiri, cachingTimeSeconds);
			num = 75;
			ret.IRasGiriDet = new RasGiriDetCacheRepository(aggregatePersistenceRepository.IRasGiriDet, cachingTimeSeconds);
			num = 76;
			ret.ITafsilSelectedList = new TafsilSelectedListCacheRepository(aggregatePersistenceRepository.ITafsilSelectedList, cachingTimeSeconds);
			num = 77;
			ret.LastPriceRepository = new LastPriceCacheRepository(aggregatePersistenceRepository.LastPriceRepository, cachingTimeSeconds, context.Guid);
			num = 78;
			ret.IPrdSelectedList = new PrdSelectedListCacheRepository(aggregatePersistenceRepository.IPrdSelectedList, cachingTimeSeconds);
			num = 79;
			ret.CashiersLogRepository = new CashiersLogCacheRepository(aggregatePersistenceRepository.CashiersLogRepository, cachingTimeSeconds);
			num = 80;
			ret.IResidStatusRepository = new ResidStatusCacheRepository(aggregatePersistenceRepository.IResidStatusRepository, cachingTimeSeconds);
			num = 81;
			ret.LotteryRepository = new LotteryCacheRepository(aggregatePersistenceRepository.LotteryRepository, cachingTimeSeconds);
			num = 82;
			ret.LotteryScoreRepository = new LotteryScoreCacheRepository(aggregatePersistenceRepository.LotteryScoreRepository, cachingTimeSeconds);
			num = 83;
			ret.ShortCut = new ShortCutCacheRepository(aggregatePersistenceRepository.ShortCut, cachingTimeSeconds);
			num = 84;
			ret.ReportGenerator = aggregatePersistenceRepository.ReportGenerator;
			num = 85;
			ret.ReportGeneratorDet = new ReportGeneratorDetCacheRepository(aggregatePersistenceRepository.ReportGeneratorDet, cachingTimeSeconds);
			num = 86;
			if (aggregatePersistenceRepository.AghsatShRepository != null)
			{
				ret.AghsatShRepository = new AghsatShCacheRepository(aggregatePersistenceRepository.AghsatShRepository, cachingTimeSeconds);
			}
			num = 87;
			if (aggregatePersistenceRepository.AghsatMRepository != null)
			{
				ret.AghsatMRepository = new AghsatMCacheRepository(aggregatePersistenceRepository.AghsatMRepository, cachingTimeSeconds);
			}
			num = 88;
			if (aggregatePersistenceRepository.AghsatMGroupRepository != null)
			{
				ret.AghsatMGroupRepository = new AghsatM_GroupCacheRepository(aggregatePersistenceRepository.AghsatMGroupRepository, cachingTimeSeconds);
			}
			num = 89;
			if (aggregatePersistenceRepository.DecimalRepository != null)
			{
				ret.DecimalRepository = new DecimalCacheRepository(aggregatePersistenceRepository.DecimalRepository, cachingTimeSeconds);
			}
			num = 90;
			if (aggregatePersistenceRepository.DecimalUpdatesRepository != null)
			{
				ret.DecimalUpdatesRepository = new DecimalUpdatesCacheRepository(aggregatePersistenceRepository.DecimalUpdatesRepository, cachingTimeSeconds);
			}
			num = 91;
			if (aggregatePersistenceRepository.DecimalUpdatesRepository != null)
			{
				ret.ResidRepository = new ResidCacheRepository(aggregatePersistenceRepository.ResidRepository, cachingTimeSeconds);
			}
			num = 92;
			if (aggregatePersistenceRepository.IVamRepository != null)
			{
				ret.IVamRepository = new VamCacheRepository(aggregatePersistenceRepository.IVamRepository, cachingTimeSeconds);
			}
			num = 93;
			if (aggregatePersistenceRepository.IViewCheckMRepository != null)
			{
				ret.IViewCheckMRepository = new ViewCheckMCacheRepository(aggregatePersistenceRepository.IViewCheckMRepository, cachingTimeSeconds);
			}
			num = 94;
			if (aggregatePersistenceRepository.IViewCheckShRepository != null)
			{
				ret.IViewCheckShRepository = new ViewCheckShCacheRepository(aggregatePersistenceRepository.IViewCheckShRepository, cachingTimeSeconds);
			}
			num = 95;
			if (aggregatePersistenceRepository.NoteRepository != null)
			{
				ret.NoteRepository = new NoteCacheRepository(aggregatePersistenceRepository.NoteRepository, cachingTimeSeconds);
			}
			num = 96;
			if (aggregatePersistenceRepository.NoteGroupRepository != null)
			{
				ret.NoteGroupRepository = new NoteGroupCacheRepository(aggregatePersistenceRepository.NoteGroupRepository, cachingTimeSeconds);
			}
			num = 97;
			if (aggregatePersistenceRepository.ICallingLine != null)
			{
				ret.ICallingLine = new CallingLineCacheRepository(aggregatePersistenceRepository.ICallingLine, cachingTimeSeconds);
			}
			num = 98;
			if (aggregatePersistenceRepository.ICallingLog != null)
			{
				ret.ICallingLog = new CallingLogCacheRepository(aggregatePersistenceRepository.ICallingLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.ICallingLogDetail != null)
			{
				ret.ICallingLogDetail = new CallingLogDetailCacheRepository(aggregatePersistenceRepository.ICallingLogDetail, cachingTimeSeconds);
			}
			num = 99;
			ret.IExecuteQuery = aggregatePersistenceRepository?.IExecuteQuery;
			num = 100;
			if (aggregatePersistenceRepository.ProductAttributeRepository != null)
			{
				ret.ProductAttributeRepository = new ProductAttributeCacheRepository(aggregatePersistenceRepository.ProductAttributeRepository, cachingTimeSeconds);
			}
			num = 101;
			if (aggregatePersistenceRepository.IHesabType != null)
			{
				ret.IHesabType = new HesabTypeCacheRepository(aggregatePersistenceRepository.IHesabType, cachingTimeSeconds);
			}
			num = 102;
			if (aggregatePersistenceRepository.Tolid != null)
			{
				ret.Tolid = new TolidCacheRepository(aggregatePersistenceRepository.Tolid, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.TolidFormula != null)
			{
				ret.TolidFormula = new TolidFormulaCacheRepository(aggregatePersistenceRepository.TolidFormula, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.VistorArea != null)
			{
				ret.VistorArea = new VisitAreaCacheRepository(aggregatePersistenceRepository.VistorArea, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.TableRelationShip != null)
			{
				ret.TableRelationShip = new TableRelationShipCacheRepository(aggregatePersistenceRepository.TableRelationShip, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.TozieList != null)
			{
				ret.TozieList = new TozieListCacheRepository(aggregatePersistenceRepository.TozieList, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.TozieListDetail != null)
			{
				ret.TozieListDetail = new TozieListDetailCacheRepository(aggregatePersistenceRepository.TozieListDetail, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository.ViewSanadDetail != null)
			{
				ret.ViewSanadDetail = new ViewSanadDetailCacheRepository(aggregatePersistenceRepository.ViewSanadDetail, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TolidSummary != null)
			{
				ret.TolidSummary = new TolidSummaryCacheRepository(aggregatePersistenceRepository.TolidSummary, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ProductGroupSelected != null)
			{
				ret.ProductGroupSelected = new PrdGroupSelectedListCacheRepository(aggregatePersistenceRepository.ProductGroupSelected, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TTMS != null)
			{
				ret.TTMS = new TTMSCacheRepository(aggregatePersistenceRepository.TTMS, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SanadStatusReport != null)
			{
				ret.SanadStatusReport = new SanadStatusReportCacheRepository(aggregatePersistenceRepository.SanadStatusReport, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SanadStatusReportUpdates != null)
			{
				ret.SanadStatusReportUpdates = new SanadStatusReportUpdatesCacheRepository(aggregatePersistenceRepository.SanadStatusReportUpdates, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ViewSanadDetailCover != null)
			{
				ret.ViewSanadDetailCover = new ViewSanadDetailCoverCacheRepository(aggregatePersistenceRepository.ViewSanadDetailCover, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SanadArchives != null)
			{
				ret.SanadArchives = new SanadArchivesCacheRepository(aggregatePersistenceRepository.SanadArchives, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ReceptionChequeEntezami != null)
			{
				ret.ReceptionChequeEntezami = new ReceptionChequeEntezamiCacheRepository(aggregatePersistenceRepository.ReceptionChequeEntezami, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.PardakhtChequeEntezami != null)
			{
				ret.PardakhtChequeEntezami = new PardakhtChequeEntezamiCacheRepository(aggregatePersistenceRepository.PardakhtChequeEntezami, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Account != null)
			{
				ret.Account = new AccountCacheRepository(aggregatePersistenceRepository.Account, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.AccountLog != null)
			{
				ret.AccountLog = new AccountLogCacheRepository(aggregatePersistenceRepository.AccountLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Hall != null)
			{
				ret.Hall = new HallCacheRepository(aggregatePersistenceRepository.Hall, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Placement != null)
			{
				ret.Placement = new PlacementCacheRepository(aggregatePersistenceRepository.Placement, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Receipt != null)
			{
				ret.Receipt = new ReceiptCacheRepository(aggregatePersistenceRepository.Receipt, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.PlacementFacilities != null)
			{
				ret.PlacementFacilities = new PlacementFaclitiesRelationCacheRepository(aggregatePersistenceRepository.PlacementFacilities, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Facilities != null)
			{
				ret.Facilities = new FacilitiesCacheRepository(aggregatePersistenceRepository.Facilities, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ServiceReceiptDetail != null)
			{
				ret.ServiceReceiptDetail = new ServiceReceiptDetailRelationCacheRepository(aggregatePersistenceRepository.ServiceReceiptDetail, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TempBill != null)
			{
				ret.TempBill = new TempBillCacheRepository(aggregatePersistenceRepository.TempBill, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.BillMaster != null)
			{
				ret.BillMaster = new BillMasterCacheRepository(aggregatePersistenceRepository.BillMaster, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CustomerReceipt != null)
			{
				ret.CustomerReceipt = new CustomerReceiptRelationCacheRepository(aggregatePersistenceRepository.CustomerReceipt, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ProductPrices != null)
			{
				ret.ProductPrices = new ProductDecimalRelationCacheRepository(aggregatePersistenceRepository.ProductPrices, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SanadStatusLog != null)
			{
				ret.SanadStatusLog = new SanadStatusLogCacheRepository(aggregatePersistenceRepository.SanadStatusLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CustomerRole != null)
			{
				ret.CustomerRole = new CustomerRoleCacheRepository(aggregatePersistenceRepository.CustomerRole, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ProductPriceUpdate != null)
			{
				ret.ProductPriceUpdate = new ProductPriceUpdateCacheRepository(aggregatePersistenceRepository.ProductPriceUpdate, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TaxOrderLog != null)
			{
				ret.TaxOrderLog = new TaxOrderLogCacheRepository(aggregatePersistenceRepository.TaxOrderLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CouponDiscount != null)
			{
				ret.CouponDiscount = new CouponDiscountCacheRepository(aggregatePersistenceRepository.CouponDiscount, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CouponDiscountUsages != null)
			{
				ret.CouponDiscountUsages = new CouponDiscountUsagesCacheRepository(aggregatePersistenceRepository.CouponDiscountUsages, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Gift != null)
			{
				ret.Gift = new GiftCacheRepository(aggregatePersistenceRepository.Gift, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.PrdGroupRelationRepository != null)
			{
				ret.PrdGroupRelationRepository = new ProductGroupRelationCacheRepository(aggregatePersistenceRepository.PrdGroupRelationRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.OrderDetailCouponCalculation != null)
			{
				ret.OrderDetailCouponCalculation = new OrderDetailCouponCalculationCacheRepository(aggregatePersistenceRepository.OrderDetailCouponCalculation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.DepotReceipt != null)
			{
				ret.DepotReceipt = new DepotReceiptCacheRepository(aggregatePersistenceRepository.DepotReceipt, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Tax != null)
			{
				ret.Tax = new TaxCacheRepository(aggregatePersistenceRepository.Tax, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.OpenInvoice != null)
			{
				ret.OpenInvoice = new OpenInvoiceCacheRepository(aggregatePersistenceRepository.OpenInvoice, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.PlacementOrderRelation != null)
			{
				ret.PlacementOrderRelation = new PlacementOrderRelationCacheRepository(aggregatePersistenceRepository.PlacementOrderRelation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Reservation != null)
			{
				ret.Reservation = new ReservationCacheRepository(aggregatePersistenceRepository.Reservation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TempBarcode != null)
			{
				ret.TempBarcode = new TempBarcodePrintCacheRepository(aggregatePersistenceRepository.TempBarcode, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ProductPriceLevel != null)
			{
				ret.ProductPriceLevel = new ProductPriceLevelCacheRepository(aggregatePersistenceRepository.ProductPriceLevel, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ProductPriceRelation != null)
			{
				ret.ProductPriceRelation = new ProductPriceRelationCacheRepository(aggregatePersistenceRepository.ProductPriceRelation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Evaluation != null)
			{
				ret.Evaluation = new EvaluationCacheRepository(aggregatePersistenceRepository.Evaluation, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The error Line Is :{num}", "InitializeAccountingContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 333);
		}
	}

	internal static void InitializeGlobalContext(AggregateSqlRepository ret, AggregateSqlRepository aggregatePersistenceRepository, WebContextHolder context, int cachingTimeSeconds)
	{
		try
		{
			if (aggregatePersistenceRepository?.ILocalSetting != null)
			{
				ret.ILocalSetting = new LocalSettingCacheRepository(aggregatePersistenceRepository.ILocalSetting, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IContact != null)
			{
				ret.IContact = new ContactUsCacheRepository(aggregatePersistenceRepository.IContact, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IFileInfoReTryDownload != null)
			{
				ret.IFileInfoReTryDownload = new FileInfoReTryDownloadCacheRepository(aggregatePersistenceRepository.IFileInfoReTryDownload, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IHyperLink != null)
			{
				ret.IHyperLink = new HyperLinkCacheRepository(aggregatePersistenceRepository.IHyperLink, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.INews != null)
			{
				ret.INews = new NewsCacheRepository(aggregatePersistenceRepository.INews, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IUserInformation != null)
			{
				ret.IUserInformation = new UserInfoCacheRepository(aggregatePersistenceRepository.IUserInformation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IUploadFile != null)
			{
				ret.IUploadFile = new UploadFileCacheRepository(aggregatePersistenceRepository.IUploadFile, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IUploadFilePart != null)
			{
				ret.IUploadFilePart = new UploadFilePartCacheRepository(aggregatePersistenceRepository.IUploadFilePart, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IBackupSchedule != null)
			{
				ret.IBackupSchedule = new BackupScheduleCacheRepository(aggregatePersistenceRepository.IBackupSchedule, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IBackupTime != null)
			{
				ret.IBackupTime = new BackupTimeCacheRepository(aggregatePersistenceRepository.IBackupTime, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IBackupLog != null)
			{
				ret.IBackupLog = new BackupLogCacheRepository(aggregatePersistenceRepository.IBackupLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IBackupSetting != null)
			{
				ret.IBackupSetting = new BackupSettingCacheRepository(aggregatePersistenceRepository.IBackupSetting, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IDesc != null)
			{
				ret.IDesc = new DescCacheRepository(aggregatePersistenceRepository.IDesc, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IEn2FaRepositpry != null)
			{
				ret.IEn2FaRepositpry = new En2FaCacheRepository(aggregatePersistenceRepository.IEn2FaRepositpry, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.INaghzRepository != null)
			{
				ret.INaghzRepository = new NaghzCacheRepository(aggregatePersistenceRepository.INaghzRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CheckedSave != null)
			{
				ret.CheckedSave = new CheckedSaveCacheRepository(aggregatePersistenceRepository.CheckedSave, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IFileInfo != null)
			{
				ret.IFileInfo = new FileInfoCacheRepository(aggregatePersistenceRepository.IFileInfo, checked(cachingTimeSeconds * 100));
			}
			if (aggregatePersistenceRepository?.Calender != null)
			{
				ret.Calender = new CalenderCacheRepository(aggregatePersistenceRepository.Calender, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.PushNotification != null)
			{
				ret.PushNotification = new PushNotificationCacheRepository(aggregatePersistenceRepository.PushNotification, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.MetaData != null)
			{
				ret.MetaData = new MetaDataCacheRepository(aggregatePersistenceRepository.MetaData, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Menu != null)
			{
				ret.Menu = new MenuCacheRepository(aggregatePersistenceRepository.Menu, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.MenuItem != null)
			{
				ret.MenuItem = new MenuItemCacheRepository(aggregatePersistenceRepository.MenuItem, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Card != null)
			{
				ret.Card = new CardCacheRepository(aggregatePersistenceRepository.Card, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.StimulQuery != null)
			{
				ret.StimulQuery = new StimulQueryCacheRepository(aggregatePersistenceRepository.StimulQuery, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SaveTemp != null)
			{
				ret.SaveTemp = new SaveTempCacheRepository(aggregatePersistenceRepository.SaveTemp, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IntroductionMethod != null)
			{
				ret.IntroductionMethod = new IntroductionMethodCacheRepository(aggregatePersistenceRepository.IntroductionMethod, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Wage != null)
			{
				ret.Wage = new WageCacheRepository(aggregatePersistenceRepository.Wage, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Post != null)
			{
				ret.Post = new PostCacheRepository(aggregatePersistenceRepository.Post, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.Ticket != null)
			{
				ret.Ticket = new TicketCacheRepository(aggregatePersistenceRepository.Ticket, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TicketDet != null)
			{
				ret.TicketDet = new TicketDetCacheRepository(aggregatePersistenceRepository.TicketDet, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TicketPart != null)
			{
				ret.TicketPart = new TicketPartCacheRepository(aggregatePersistenceRepository.TicketPart, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.TicketPartDetail != null)
			{
				ret.TicketPartDetail = new TicketPartDetailCacheRepository(aggregatePersistenceRepository.TicketPartDetail, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ITasks != null)
			{
				ret.ITasks = new TaskCacheRepository(aggregatePersistenceRepository.ITasks, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ISarNakh != null)
			{
				ret.ISarNakh = new SarNakhCacheRepository(aggregatePersistenceRepository.ISarNakh, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.SmsOutBox != null)
			{
				ret.SmsOutBox = new SmsOutBoxCacheRepository(aggregatePersistenceRepository.SmsOutBox, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.EntityDeliveryStatus != null)
			{
				ret.EntityDeliveryStatus = new EntityDeliveryStatusCacheRepository(aggregatePersistenceRepository.EntityDeliveryStatus, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeGlobalContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 377);
		}
	}

	internal static void InitializeAdvertiseContext(AggregateSqlRepository ret, AggregateSqlRepository aggregatePersistenceRepository, WebContextHolder context, int cachingTimeSeconds)
	{
		try
		{
			if (aggregatePersistenceRepository?.IAdvertiseLog != null)
			{
				ret.IAdvertiseLog = new AdvertiseLogCacheRepository(aggregatePersistenceRepository?.IAdvertiseLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IVisitor != null)
			{
				ret.IVisitor = new VisitorCacheRepository(aggregatePersistenceRepository?.IVisitor, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ISimCard != null)
			{
				ret.ISimCard = new SimCardCacheRepository(aggregatePersistenceRepository?.ISimCard, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IAdvTokensRepository != null)
			{
				ret.IAdvTokensRepository = new AdvTokensCacheRepository(aggregatePersistenceRepository?.IAdvTokensRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IAdvCategoryRepository != null)
			{
				ret.IAdvCategoryRepository = new AdvCategoryCacheRepository(aggregatePersistenceRepository?.IAdvCategoryRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IOwner != null)
			{
				ret.IOwner = new OwnerCacheRepository(aggregatePersistenceRepository?.IOwner, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ICity != null)
			{
				ret.ICity = new CityCacheRepository(aggregatePersistenceRepository?.ICity, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IState != null)
			{
				ret.IState = new StateCacheRepository(aggregatePersistenceRepository?.IState, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IVisitorCity != null)
			{
				ret.IVisitorCity = new VisitorCityCacheRepository(aggregatePersistenceRepository?.IVisitorCity, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IVisitorAdv != null)
			{
				ret.IVisitorAdv = new VisitorAdvCacheRepository(aggregatePersistenceRepository?.IVisitorAdv, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ITitleBlackList != null)
			{
				ret.ITitleBlackList = new TitleBlackListCacheRepository(aggregatePersistenceRepository?.ITitleBlackList, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IContentBlackList != null)
			{
				ret.IContentBlackList = new ContentBlackListCacheRepository(aggregatePersistenceRepository?.IContentBlackList, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IRegion != null)
			{
				ret.IRegion = new RegionCacheRepository(aggregatePersistenceRepository?.IRegion, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IAdvDepartment != null)
			{
				ret.IAdvDepartment = new AdvDepartmentCacheRepository(aggregatePersistenceRepository?.IAdvDepartment, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IDepartmentCategory != null)
			{
				ret.IDepartmentCategory = new DepartmentCategoryCacheRepository(aggregatePersistenceRepository?.IDepartmentCategory, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ChatNumberRepository != null)
			{
				ret.ChatNumberRepository = new ChatNumberCacheRepository(aggregatePersistenceRepository?.ChatNumberRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IDivarCityRepository != null)
			{
				ret.IDivarCityRepository = new DivarCityCacheRepository(aggregatePersistenceRepository?.IDivarCityRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.ISheypoorCityRepository != null)
			{
				ret.ISheypoorCityRepository = new SheypoorCityCacheRepository(aggregatePersistenceRepository?.ISheypoorCityRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.IAdvGroupRepository != null)
			{
				ret.IAdvGroupRepository = new AdvGroupCacheRepository(aggregatePersistenceRepository?.IAdvGroupRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.AdvertiseRepository != null)
			{
				ret.AdvertiseRepository = new AdvertiseCacheRepository(aggregatePersistenceRepository?.AdvertiseRepository, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.CategoryGroupRepository != null)
			{
				ret.CategoryGroupRepository = new CategoryGroupCacheRepository(aggregatePersistenceRepository?.CategoryGroupRepository, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAdvertiseContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 405);
		}
	}

	internal static void InitializeDepratmentContext(AggregateSqlRepository ret, AggregateSqlRepository aggregatePersistenceRepository, WebContextHolder context, int cachingTimeSeconds)
	{
		try
		{
			if (aggregatePersistenceRepository?.LkInfo != null)
			{
				ret.LkInfo = new LkInfoCacheRepository(aggregatePersistenceRepository?.LkInfo, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeDepratmentContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 413);
		}
	}

	internal static void InitializeAttendanceContext(AggregateSqlRepository ret, AggregateSqlRepository aggregatePersistenceRepository, WebContextHolder context, int cachingTimeSeconds)
	{
		try
		{
			if (aggregatePersistenceRepository?.IAttPersonLeave != null)
			{
				ret.IAttPersonLeave = new AttPersonLeaveCacheRepository(aggregatePersistenceRepository.IAttPersonLeave, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.AttDepartment != null)
			{
				ret.AttDepartment = new AttDepartmentCacheRepository(aggregatePersistenceRepository.AttDepartment, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.AttPerson != null)
			{
				ret.AttPerson = new AttPersonCacheRepository(aggregatePersistenceRepository.AttPerson, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.BioMetricData != null)
			{
				ret.BioMetricData = new BiometricCacheRepository(aggregatePersistenceRepository.BioMetricData, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.BioDeviceRelation != null)
			{
				ret.BioDeviceRelation = new BioDeviceRelationCacheRepository(aggregatePersistenceRepository.BioDeviceRelation, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.BioWriteDevice != null)
			{
				ret.BioWriteDevice = new BioWriteDeviceCacheRepository(aggregatePersistenceRepository.BioWriteDevice, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.BioWriteDeviceLog != null)
			{
				ret.BioWriteDeviceLog = new BioWriteDeviceLogCacheRepository(aggregatePersistenceRepository.BioWriteDeviceLog, cachingTimeSeconds);
			}
			if (aggregatePersistenceRepository?.DeviceInfo != null)
			{
				ret.DeviceInfo = new DeviceInfoCacheRepository(aggregatePersistenceRepository.DeviceInfo, cachingTimeSeconds);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAttendanceContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregatorManager.cs", 428);
		}
	}
}
