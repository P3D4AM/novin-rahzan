using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Repository;
using EntityCache.Cache.Repository.Accounting;
using EntityCache.Cache.Repository.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Core.Repository.Accounting.Aghsat;
using EntityCache.Core.Repository.Accounting.Bill;
using EntityCache.Core.Repository.Accounting.Cheque;
using EntityCache.Core.Repository.Accounting.Coding;
using EntityCache.Core.Repository.Accounting.Crm;
using EntityCache.Core.Repository.Accounting.DepotReceipt;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Core.Repository.Accounting.DiscountCoupon;
using EntityCache.Core.Repository.Accounting.Factor;
using EntityCache.Core.Repository.Accounting.Gift;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Core.Repository.Accounting.Khazane.Pardakht;
using EntityCache.Core.Repository.Accounting.Khazane.Reception;
using EntityCache.Core.Repository.Accounting.Note;
using EntityCache.Core.Repository.Accounting.Order;
using EntityCache.Core.Repository.Accounting.Order.OpenInvoices;
using EntityCache.Core.Repository.Accounting.Reservation;
using EntityCache.Core.Repository.Accounting.Sanad;
using EntityCache.Core.Repository.Accounting.Selection;
using EntityCache.Core.Repository.Accounting.Selection.ProductGroup;
using EntityCache.Core.Repository.Accounting.TTMS;
using EntityCache.Core.Repository.Accounting.TalaVaArz;
using EntityCache.Core.Repository.Accounting.Tax;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Core.Repository.Accounting.TozieList;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Core.Repository.Customer;
using EntityCache.Core.Repository.Department;
using EntityCache.Core.Repository.Global;
using EntityCache.Core.Repository.Global.Tasks;
using EntityCache.Core.Repository.Global.Ticket;
using EntityCache.Core.Repository.Indexes;
using EntityCache.Core.Repository.Report;
using EntityCache.Core.Repository.Tickets;
using EntityCache.Persistence.Repository;
using EntityCache.Uploader;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace EntityCache;

public class AggregateSqlRepository : IDisposable
{
	public WebContextHolder Context { get; private set; }

	public Upload Upload { get; set; }

	public IDeviceInfoRepository DeviceInfo { get; set; }

	public IAttPersonLeaveRepository IAttPersonLeave { get; set; }

	public IAttDepartmentRepository AttDepartment { get; set; }

	public IAttPersonRepository AttPerson { get; set; }

	public IBioDeviceRelationRepository BioDeviceRelation { get; set; }

	public IBiometricRepository BioMetricData { get; set; }

	public IBioWriteDeviceRepository BioWriteDevice { get; set; }

	public IBioWriteDeviceLogRepository BioWriteDeviceLog { get; set; }

	public virtual IReceptionHavaleRepository IReceptionHavale { get; set; }

	public IFareOrderCredit IFareOrderCredit { get; set; }

	public ISanadPrint ISanaDPrint { get; set; }

	public virtual IAbadDetRepository IABAD { get; set; }

	public virtual IReceptionCheckRepository IReceptionCheckRepository { get; set; }

	public IBarcodeRepository IBarcode { get; set; }

	public IContactUsRepository IContact { get; set; }

	public ICustomerAddressPersistenceRepository ICustomerAddressPersistence { get; set; }

	public ICustomerAddressRepository ICustomerAddress { get; set; }

	public ICustomerCompactRepository ICustomerCompact { get; set; }

	public ICustomerGroupRepository ICustomerGroup { get; set; }

	public ICustomerRepository ICustomer { get; set; }

	public ICustomerTitleRepository ICustomerTitle { get; set; }

	public IDepotRepository IDepot { get; set; }

	public IDescRepository IDesc { get; set; }

	public IExecuteQueryRepository IExecuteQuery { get; set; }

	public IFileInfoRepository IFileInfo { get; set; }

	public IFileInfoReTryDownloadRepository IFileInfoReTryDownload { get; set; }

	public IGpsRepository IGps { get; set; }

	public IHyperLinkRepository IHyperLink { get; set; }

	public ILocalSettingRepository ILocalSetting { get; set; }

	public INewsRepository INews { get; set; }

	public virtual IOrderRepository IOrder { get; set; }

	public virtual IOrderDetailRepository IOrderDetail { get; set; }

	public virtual IPardakhtRepository IPardakht { get; set; }

	public IPrdAnbarRelationRepository IPrdAnbarRelation { get; set; }

	public ITafsilNumberRepository ITafsilNumberRepository { get; set; }

	public IProductCompactRepository IProductCompact { get; set; }

	public IProductGroupRepository IProductGroup { get; set; }

	public IProductRepository IProduct { get; set; }

	public IProductVahedRepository IProductVahed { get; set; }

	public virtual IReceptionRepository IReception { get; set; }

	public ISuggestedBankRepository ISuggestedBank { get; set; }

	public IUserInformationRepository IUserInformation { get; set; }

	public IVisitRepository IVisit { get; set; }

	public IUploadFileRepository IUploadFile { get; set; }

	public IUploadFilePartRepository IUploadFilePart { get; set; }

	public IBackupTimeRepository IBackupTime { get; set; }

	public IBackupSettingRepository IBackupSetting { get; set; }

	public IBackupScheduleRepository IBackupSchedule { get; set; }

	public IBackupLogRepository IBackupLog { get; set; }

	public virtual ITicketRepository Ticket { get; set; }

	public ITicketPartRepository TicketPart { get; set; }

	public virtual ITicketDetRepository TicketDet { get; set; }

	public ITicketPartDetailRepository TicketPartDetail { get; set; }

	public ITaskRepository ITasks { get; set; }

	public ISarNakhRepository ISarNakh { get; set; }

	public IIxProductAllFieldsRepository IIxProductAllFiels { get; set; }

	public IIxProductNameAndCodeFieldRepository IIxProductNameAndCodeFieldRepository { get; set; }

	public IIxProductNameFieldRepository IIxProductNameFieldRepository { get; set; }

	public IIxProductDescFieldRepository IIxProductDescFieldRepository { get; set; }

	public IIxProductCodeFieldRepository IIxProductCodeFieldRepository { get; set; }

	public IIxProductT1FieldRepository IIxProductT1FieldRepository { get; set; }

	public IIxProductT2FieldRepository IIxProductT2FieldRepository { get; set; }

	public IIxProductT3FieldRepository IIxProductT3FieldRepository { get; set; }

	public IIxProductT4FieldRepository IIxProductT4FieldRepository { get; set; }

	public IIxProductT5FieldRepository IIxProductT5FieldRepository { get; set; }

	public IIxProductT6FieldRepository IIxProductT6FieldRepository { get; set; }

	public IIxProductT7FieldRepository IIxProductT7FieldRepository { get; set; }

	public IIxProductT8FieldRepository IIxProductT8FieldRepository { get; set; }

	public IFullTextWordRepository IFullTextWordRepository { get; set; }

	public ICustomerReportsRepository CustomerReports { get; set; }

	public IReportRepository Reports { get; set; }

	public IUserAccRepository IUserAcc { get; set; }

	public ITafsilGroupRepository ITafsilGroup { get; set; }

	public IPrdTechnicalRepository IPrdTechnical { get; set; }

	public IPrdSerialRepository IPrdSerial { get; set; }

	public IPrdSerialDetailRepository IPrdSerialDetail { get; set; }

	public IPrdAnbarGardaniRepository IPrdAnbarGardani { get; set; }

	public IHesabGroupRepository IHesabGroup { get; set; }

	public IHesabTypeRepository IHesabType { get; set; }

	public IDepEnteghalRepository IDepEnteghal { get; set; }

	public ICallingLogRepository ICallingLog { get; set; }

	public ICallingLogDetailRepository ICallingLogDetail { get; set; }

	public ICallingLineRepository ICallingLine { get; set; }

	public IKolRepository IKol { get; set; }

	public IMoeinRepository IMoein { get; set; }

	public ITafsilRepository ITafsil { get; set; }

	public IViewSanadDetailRepository ViewSanadDetail { get; set; }

	public virtual IGardeshTafsilRepository IGardeshTafil { get; set; }

	public virtual ISanadRepository ISanad { get; set; }

	public virtual ISanad_DetRepository ISanadDet { get; set; }

	public IKardexRepository Ikardex { get; set; }

	public ICashierRepository ICASHIER { get; set; }

	public IMoeinTafsilGroupRelationsRepository IMoeinTafsilGroupRelations { get; set; }

	public virtual IOrderAbadRepository IOrderAbad { get; set; }

	public IVamRepository IVamRepository { get; set; }

	public IChequeMOperationRepository ChequeMOperation { get; set; }

	public IChequeShOperationRepository ChequeShOperation { get; set; }

	public ICusCardRepository ICusCard { get; set; }

	public virtual IPardakhtCheckShRepository IPardakhtCheckSh { get; set; }

	public virtual IPardakhtCheckMRepository IPardakhtCheckM { get; set; }

	public IChequeBookRepository IChequeBook { get; set; }

	public IRasGiriRepository IRasGiri { get; set; }

	public IRasGiriDetRepository IRasGiriDet { get; set; }

	public ITafsilSelectedListRepository ITafsilSelectedList { get; set; }

	public IPrdSelectedListRepository IPrdSelectedList { get; set; }

	public ICashiersLogRepository CashiersLogRepository { get; set; }

	public IAghsatMRepository AghsatMRepository { get; set; }

	public IAghsatShRepository AghsatShRepository { get; set; }

	public IAghsatM_GroupRepository AghsatMGroupRepository { get; set; }

	public IDecimalRepository DecimalRepository { get; set; }

	public IDecimalUpdatesRepository DecimalUpdatesRepository { get; set; }

	public IResidRepository ResidRepository { get; set; }

	public INoteRepository NoteRepository { get; set; }

	public INoteGroupRepository NoteGroupRepository { get; set; }

	public ILotteryRepository LotteryRepository { get; set; }

	public ILastPriceRepository LastPriceRepository { get; set; }

	public ITalaVaArzRepository TalaVaArzRepository { get; set; }

	public ILotteryScoreRepository LotteryScoreRepository { get; set; }

	public ICheckedSaveRepository CheckedSave { get; set; }

	public IShortCutRepository ShortCut { get; set; }

	public IReportGeneratorRepository ReportGenerator { get; set; }

	public IReportGeneratorDetRepository ReportGeneratorDet { get; set; }

	public virtual IViewCheckMRepository IViewCheckMRepository { get; set; }

	public virtual IViewCheckShRepository IViewCheckShRepository { get; set; }

	public IMetaDataRepository MetaData { get; set; }

	public IMenuRepository Menu { get; set; }

	public ICardRepository Card { get; set; }

	public IMenuItemRepository MenuItem { get; set; }

	public ITableRelationShipRepository TableRelationShip { get; set; }

	public IVistorAreaRepository VistorArea { get; set; }

	public virtual ITozieListRepository TozieList { get; set; }

	public virtual ITozieListDetailRepository TozieListDetail { get; set; }

	public virtual ISanadStatusReportRepository SanadStatusReport { get; set; }

	public virtual ISanadStatusReportUpdatesRepository SanadStatusReportUpdates { get; set; }

	public virtual ITaxOrderLogRepository TaxOrderLog { get; set; }

	public virtual IOrderDetailCouponCalculationRepository OrderDetailCouponCalculation { get; set; }

	public virtual IProductPriceLevelRepository ProductPriceLevel { get; set; }

	public virtual IProductPriceRelationRepository ProductPriceRelation { get; set; }

	public virtual IEvaluationRepository Evaluation { get; set; }

	public IAdvertiseLogRepository IAdvertiseLog { get; set; }

	public IVisitorRepository IVisitor { get; set; }

	public ISimCardRepository ISimCard { get; set; }

	public IOwnerRepository IOwner { get; set; }

	public ICityRepository ICity { get; set; }

	public IStateRepository IState { get; set; }

	public IVisitorCityRepository IVisitorCity { get; set; }

	public IVisitorAdvRepository IVisitorAdv { get; set; }

	public ITitleBlackListRepository ITitleBlackList { get; set; }

	public IContentBlackListRepository IContentBlackList { get; set; }

	public IRegionRepository IRegion { get; set; }

	public IDivarCityRepository IDivarCityRepository { get; set; }

	public ISheypoorCityRepository ISheypoorCityRepository { get; set; }

	public IAdvTokensRepository IAdvTokensRepository { get; set; }

	public IAdvCategoryRepository IAdvCategoryRepository { get; set; }

	public IEn2FaRepositpry IEn2FaRepositpry { get; set; }

	public IResidStatusRepository IResidStatusRepository { get; set; }

	public INaghzRepository INaghzRepository { get; set; }

	public IAdvGroupRepository IAdvGroupRepository { get; set; }

	public IAdvDepartmentRepository IAdvDepartment { get; set; }

	public IDepartmentCategoryRepository IDepartmentCategory { get; set; }

	public IChatNumberRepository ChatNumberRepository { get; set; }

	public IAdvertiseRepository AdvertiseRepository { get; set; }

	public IProductAttributeRepository ProductAttributeRepository { get; set; }

	public IProductAttributeRelationRepository ProductAttributeRelationRepository { get; set; }

	public ICategoryGroupRepository CategoryGroupRepository { get; set; }

	public ICalenderRepository Calender { get; set; }

	public IPushNotificationRepository PushNotification { get; set; }

	public virtual ITolidRepository Tolid { get; set; }

	public ITolidExportRepository TolidExport { get; set; }

	public ITolidHazineRepository TolidHazine { get; set; }

	public ITolidMavadRepository TolidMavad { get; set; }

	public virtual ITolidFormulaRepository TolidFormula { get; set; }

	public ITolidFormulaExportRepository TolidFormulaExport { get; set; }

	public ITolidFormulaHazineRepository TolidFormulaHazine { get; set; }

	public ITolidFormulaMavadRepository TolidFormulaMavad { get; set; }

	public ILkInfoRepository LkInfo { get; set; }

	public IStimulQueryRepository StimulQuery { get; set; }

	public ITempBarcodePrintRepository TempBarcode { get; set; }

	public ITolidSummaryRepository TolidSummary { get; set; }

	public ITolidSummaryDetailRepository TolidSummaryDetail { get; set; }

	public ISaveTempRepository SaveTemp { get; set; }

	public IPrdGroupSelectedListRepository ProductGroupSelected { get; set; }

	public ITTMSRepository TTMS { get; set; }

	public IIntroductionMethodRepository IntroductionMethod { get; set; }

	public IEntityDeliveryStatusRepository EntityDeliveryStatus { get; set; }

	public IWageRepository Wage { get; set; }

	public ISyncEntity SyncEntity { get; set; }

	public IViewSanadDetailCoverRepository ViewSanadDetailCover { get; set; }

	public ISanadArchivesRepository SanadArchives { get; set; }

	public IReceptionChequeEntezamiRepository ReceptionChequeEntezami { get; set; }

	public IPardakhtChequeEntezamiRepository PardakhtChequeEntezami { get; set; }

	public IAccountRepository Account { get; set; }

	public IAccountLogRepository AccountLog { get; set; }

	public ISanadStatusLogRepository SanadStatusLog { get; set; }

	public IHallRepository Hall { get; set; }

	public IPlacementRepository Placement { get; set; }

	public IReceiptRepository Receipt { get; set; }

	public IPlacementFacilitiesRelationRepository PlacementFacilities { get; set; }

	public IFacilitiesRepository Facilities { get; set; }

	public ICustomerReceiptRelationRepository CustomerReceipt { get; set; }

	public IProductDecimalRelationRepository ProductPrices { get; set; }

	public ICustomerRoleRepository CustomerRole { get; set; }

	public IProductPriceUpdateRepository ProductPriceUpdate { get; set; }

	public IProductGroupRelationRepository PrdGroupRelationRepository { get; set; }

	public IServiceReceiptDetailRelationRepository ServiceReceiptDetail { get; set; }

	public IBillMasterRepository BillMaster { get; set; }

	public IBillDetailRepository BillDetail { get; set; }

	public ITempBillRepository TempBill { get; set; }

	public IDepotReceiptRepository DepotReceipt { get; set; }

	public ITaxRepository Tax { get; set; }

	public ICouponDiscountRepository CouponDiscount { get; set; }

	public ICouponDiscountUsagesRepository CouponDiscountUsages { get; set; }

	public IGiftRepository Gift { get; set; }

	public IPostRepository Post { get; set; }

	public IOpenInvoiceRepository OpenInvoice { get; set; }

	public IPlacementOrderRelationRepository PlacementOrderRelation { get; set; }

	public IReservationRepository Reservation { get; set; }

	public ISmsOutBoxRepository SmsOutBox { get; set; }

	public bool Initialized
	{
		get
		{
			if (IUserInformation != null && IFileInfo != null && IGps != null && ICustomerCompact != null && ICustomer != null && ICustomerTitle != null && ICustomerGroup != null && IMoein != null && ITafsil != null && IKol != null && IDepot != null && IProduct != null && IProductCompact != null && IProductGroup != null && IProductVahed != null && IHyperLink != null && INews != null && IContact != null && IReception != null && IPardakht != null && IOrder != null && ProductAttributeRepository != null)
			{
				return ILocalSetting != null;
			}
			return false;
		}
	}

	public IEnumerable<IDatabaseManager> DatabaseManager { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public AggregateSqlRepository(WebContextHolder context)
	{
		Context = context;
	}

	public void Dispose()
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			IsDisposed = true;
			ProductAttributeRepository?.Dispose();
			ProductAttributeRepository = null;
			ProductAttributeRelationRepository?.Dispose();
			ProductAttributeRelationRepository = null;
			ChatNumberRepository?.Dispose();
			ChatNumberRepository = null;
			IFareOrderCredit?.Dispose();
			IFareOrderCredit = null;
			ISanaDPrint?.Dispose();
			ISanaDPrint = null;
			IABAD?.Dispose();
			IABAD = null;
			IReceptionCheckRepository?.Dispose();
			IReceptionCheckRepository = null;
			IBarcode?.Dispose();
			IBarcode = null;
			IContact?.Dispose();
			IContact = null;
			ICustomerAddressPersistence?.Dispose();
			ICustomerAddressPersistence = null;
			ICustomerAddress?.Dispose();
			ICustomerAddress = null;
			ICustomerCompact?.Dispose();
			ICustomerCompact = null;
			ICustomerGroup?.Dispose();
			ICustomerGroup = null;
			ICustomer?.Dispose();
			ICustomer = null;
			ICustomerTitle?.Dispose();
			ICustomerTitle = null;
			IDepot?.Dispose();
			IDepot = null;
			IDesc?.Dispose();
			IDesc = null;
			MetaData?.Dispose();
			MetaData = null;
			Menu?.Dispose();
			Menu = null;
			MenuItem?.Dispose();
			MenuItem = null;
			Card?.Dispose();
			Card = null;
			TableRelationShip?.Dispose();
			TableRelationShip = null;
			IExecuteQuery?.Dispose();
			IExecuteQuery = null;
			IFileInfo?.Dispose();
			IFileInfo = null;
			IFileInfoReTryDownload?.Dispose();
			IFileInfoReTryDownload = null;
			IGps?.Dispose();
			IGps = null;
			IHyperLink?.Dispose();
			IHyperLink = null;
			ILocalSetting?.Dispose();
			ILocalSetting = null;
			INews?.Dispose();
			INews = null;
			IOrder?.Dispose();
			IOrder = null;
			IOrderDetail?.Dispose();
			IOrderDetail = null;
			IPrdSerialDetail?.Dispose();
			IPrdSerialDetail = null;
			IPardakht?.Dispose();
			IPardakht = null;
			IPrdAnbarRelation?.Dispose();
			IPrdAnbarRelation = null;
			ITafsilNumberRepository?.Dispose();
			ITafsilNumberRepository = null;
			IProductCompact?.Dispose();
			IProductCompact = null;
			IProductGroup?.Dispose();
			IProductGroup = null;
			VistorArea?.Dispose();
			VistorArea = null;
			IProduct?.Dispose();
			IProduct = null;
			IProductVahed?.Dispose();
			IProductVahed = null;
			IReception?.Dispose();
			IReception = null;
			ISuggestedBank?.Dispose();
			ISuggestedBank = null;
			IUserInformation?.Dispose();
			IUserInformation = null;
			IVisit?.Dispose();
			IVisit = null;
			IUploadFile?.Dispose();
			IUploadFile = null;
			IUploadFilePart?.Dispose();
			IUploadFilePart = null;
			IBackupTime?.Dispose();
			IBackupTime = null;
			IBackupSetting?.Dispose();
			IBackupSetting = null;
			IBackupSchedule?.Dispose();
			IBackupSchedule = null;
			IBackupLog?.Dispose();
			IBackupLog = null;
			Ticket?.Dispose();
			Ticket = null;
			TicketPart?.Dispose();
			TicketPart = null;
			TicketDet?.Dispose();
			TicketDet = null;
			TicketPartDetail?.Dispose();
			TicketPartDetail = null;
			ITasks?.Dispose();
			ITasks = null;
			ISarNakh?.Dispose();
			ISarNakh = null;
			if (DatabaseManager != null)
			{
				foreach (IDatabaseManager item in DatabaseManager)
				{
					item?.Dispose();
				}
			}
			DatabaseManager = null;
			IIxProductAllFiels?.Dispose();
			IIxProductAllFiels = null;
			IIxProductNameFieldRepository?.Dispose();
			IIxProductNameFieldRepository = null;
			IIxProductNameAndCodeFieldRepository?.Dispose();
			IIxProductNameAndCodeFieldRepository = null;
			IIxProductDescFieldRepository?.Dispose();
			IIxProductDescFieldRepository = null;
			IIxProductCodeFieldRepository?.Dispose();
			IIxProductCodeFieldRepository = null;
			IIxProductNameFieldRepository?.Dispose();
			IIxProductNameFieldRepository = null;
			IIxProductT1FieldRepository?.Dispose();
			IIxProductT1FieldRepository = null;
			IIxProductT2FieldRepository?.Dispose();
			IIxProductT2FieldRepository = null;
			IIxProductT3FieldRepository?.Dispose();
			IIxProductT3FieldRepository = null;
			IIxProductT4FieldRepository?.Dispose();
			IIxProductT4FieldRepository = null;
			IIxProductT5FieldRepository?.Dispose();
			IIxProductT5FieldRepository = null;
			IIxProductT6FieldRepository?.Dispose();
			IIxProductT6FieldRepository = null;
			IIxProductT7FieldRepository?.Dispose();
			IIxProductT7FieldRepository = null;
			IIxProductT8FieldRepository?.Dispose();
			IIxProductT8FieldRepository = null;
			IFullTextWordRepository?.Dispose();
			IFullTextWordRepository = null;
			CustomerReports?.Dispose();
			CustomerReports = null;
			Reports?.Dispose();
			Reports = null;
			IUserAcc?.Dispose();
			IUserAcc = null;
			ITafsilGroup?.Dispose();
			ITafsilGroup = null;
			IPrdTechnical?.Dispose();
			IPrdTechnical = null;
			IPrdSerial?.Dispose();
			IPrdSerial = null;
			IPrdAnbarGardani?.Dispose();
			IPrdAnbarGardani = null;
			IHesabGroup?.Dispose();
			IHesabGroup = null;
			IHesabType?.Dispose();
			IHesabType = null;
			IDepEnteghal?.Dispose();
			IDepEnteghal = null;
			ICallingLog?.Dispose();
			ICallingLog = null;
			ICallingLogDetail?.Dispose();
			ICallingLogDetail = null;
			ICallingLine?.Dispose();
			ICallingLine = null;
			IKol?.Dispose();
			IKol = null;
			IMoein?.Dispose();
			IMoein = null;
			ITafsil?.Dispose();
			ITafsil = null;
			IGardeshTafil?.Dispose();
			IGardeshTafil = null;
			ISanad?.Dispose();
			ISanad = null;
			ISanadDet?.Dispose();
			ISanadDet = null;
			Ikardex?.Dispose();
			Ikardex = null;
			ICASHIER?.Dispose();
			ICASHIER = null;
			IMoeinTafsilGroupRelations?.Dispose();
			IMoeinTafsilGroupRelations = null;
			IOrderAbad?.Dispose();
			IOrderAbad = null;
			IVamRepository?.Dispose();
			IVamRepository = null;
			Calender?.Dispose();
			Calender = null;
			PushNotification?.Dispose();
			PushNotification = null;
			ChequeMOperation?.Dispose();
			ChequeMOperation = null;
			ChequeShOperation?.Dispose();
			ChequeShOperation = null;
			ICusCard?.Dispose();
			ICusCard = null;
			IPardakhtCheckSh?.Dispose();
			IPardakhtCheckSh = null;
			IPardakhtCheckM?.Dispose();
			IPardakhtCheckM = null;
			IChequeBook?.Dispose();
			IChequeBook = null;
			IRasGiri?.Dispose();
			IRasGiri = null;
			IRasGiriDet?.Dispose();
			IRasGiriDet = null;
			ITafsilSelectedList?.Dispose();
			ITafsilSelectedList = null;
			IPrdSelectedList?.Dispose();
			IPrdSelectedList = null;
			CashiersLogRepository?.Dispose();
			CashiersLogRepository = null;
			AghsatMRepository?.Dispose();
			AghsatMRepository = null;
			AghsatShRepository?.Dispose();
			AghsatShRepository = null;
			AghsatMGroupRepository?.Dispose();
			AghsatMGroupRepository = null;
			DecimalRepository?.Dispose();
			DecimalRepository = null;
			DecimalUpdatesRepository?.Dispose();
			DecimalUpdatesRepository = null;
			ResidRepository?.Dispose();
			ResidRepository = null;
			NoteRepository?.Dispose();
			NoteRepository = null;
			NoteGroupRepository?.Dispose();
			NoteGroupRepository = null;
			LotteryRepository?.Dispose();
			LotteryRepository = null;
			LastPriceRepository?.Dispose();
			LastPriceRepository = null;
			LotteryScoreRepository?.Dispose();
			LotteryScoreRepository = null;
			CheckedSave?.Dispose();
			CheckedSave = null;
			ShortCut?.Dispose();
			ShortCut = null;
			ReportGenerator?.Dispose();
			ReportGenerator = null;
			ReportGeneratorDet?.Dispose();
			ReportGeneratorDet = null;
			IViewCheckMRepository?.Dispose();
			IViewCheckMRepository = null;
			IViewCheckShRepository?.Dispose();
			IViewCheckShRepository = null;
			IAdvertiseLog?.Dispose();
			IAdvertiseLog = null;
			IVisitor?.Dispose();
			IVisitor = null;
			ISimCard?.Dispose();
			ISimCard = null;
			IOwner?.Dispose();
			IOwner = null;
			ICity?.Dispose();
			ICity = null;
			IState?.Dispose();
			IState = null;
			IVisitorCity?.Dispose();
			IVisitorCity = null;
			IVisitorAdv?.Dispose();
			IVisitorAdv = null;
			ITitleBlackList?.Dispose();
			ITitleBlackList = null;
			IContentBlackList?.Dispose();
			IContentBlackList = null;
			IRegion?.Dispose();
			IRegion = null;
			IDivarCityRepository?.Dispose();
			IDivarCityRepository = null;
			ISheypoorCityRepository?.Dispose();
			ISheypoorCityRepository = null;
			IAdvTokensRepository?.Dispose();
			IAdvTokensRepository = null;
			IAdvCategoryRepository?.Dispose();
			IAdvCategoryRepository = null;
			IEn2FaRepositpry?.Dispose();
			IEn2FaRepositpry = null;
			IResidStatusRepository?.Dispose();
			IResidStatusRepository = null;
			INaghzRepository?.Dispose();
			INaghzRepository = null;
			IAdvGroupRepository?.Dispose();
			IAdvGroupRepository = null;
			IAdvDepartment?.Dispose();
			IAdvDepartment = null;
			IDepartmentCategory?.Dispose();
			IDepartmentCategory = null;
			AdvertiseRepository?.Dispose();
			AdvertiseRepository = null;
			CategoryGroupRepository?.Dispose();
			CategoryGroupRepository = null;
			Upload?.Dispose();
			Upload = null;
			Tolid?.Dispose();
			Tolid = null;
			TolidExport?.Dispose();
			TolidExport = null;
			TolidMavad?.Dispose();
			TolidMavad = null;
			TolidHazine?.Dispose();
			TolidHazine = null;
			TolidFormula?.Dispose();
			TolidFormula = null;
			TolidFormulaExport?.Dispose();
			TolidFormulaExport = null;
			TolidFormulaMavad?.Dispose();
			TolidFormulaMavad = null;
			TolidFormulaHazine?.Dispose();
			TolidFormulaHazine = null;
			LkInfo?.Dispose();
			LkInfo = null;
			TozieList?.Dispose();
			TozieList = null;
			TozieListDetail?.Dispose();
			TozieListDetail = null;
			ViewSanadDetail?.Dispose();
			ViewSanadDetail = null;
			StimulQuery?.Dispose();
			StimulQuery = null;
			TolidSummary?.Dispose();
			TolidSummary = null;
			TolidSummaryDetail?.Dispose();
			TolidSummaryDetail = null;
			SaveTemp?.Dispose();
			SaveTemp = null;
			ProductGroupSelected?.Dispose();
			ProductGroupSelected = null;
			TTMS?.Dispose();
			TTMS = null;
			SanadStatusReport?.Dispose();
			SanadStatusReport = null;
			SanadStatusReportUpdates?.Dispose();
			SanadStatusReportUpdates = null;
			IntroductionMethod?.Dispose();
			IntroductionMethod = null;
			EntityDeliveryStatus?.Dispose();
			EntityDeliveryStatus = null;
			ViewSanadDetailCover?.Dispose();
			ViewSanadDetailCover = null;
			Wage?.Dispose();
			Wage = null;
			SanadArchives?.Dispose();
			SanadArchives = null;
			ReceptionChequeEntezami?.Dispose();
			ReceptionChequeEntezami = null;
			PardakhtChequeEntezami?.Dispose();
			PardakhtChequeEntezami = null;
			Account?.Dispose();
			Account = null;
			AccountLog?.Dispose();
			AccountLog = null;
			Hall?.Dispose();
			Hall = null;
			Placement?.Dispose();
			Placement = null;
			Receipt?.Dispose();
			Receipt = null;
			Facilities?.Dispose();
			Facilities = null;
			PlacementFacilities?.Dispose();
			PlacementFacilities = null;
			ServiceReceiptDetail?.Dispose();
			ServiceReceiptDetail = null;
			ProductPrices?.Dispose();
			ProductPrices = null;
			CustomerReceipt?.Dispose();
			CustomerReceipt = null;
			CustomerRole?.Dispose();
			CustomerRole = null;
			ProductPriceUpdate?.Dispose();
			ProductPriceUpdate = null;
			BillMaster?.Dispose();
			BillMaster = null;
			BillDetail?.Dispose();
			BillDetail = null;
			TempBill?.Dispose();
			TempBill = null;
			DepotReceipt?.Dispose();
			DepotReceipt = null;
			Tax?.Dispose();
			Tax = null;
			CouponDiscount?.Dispose();
			CouponDiscount = null;
			CouponDiscountUsages?.Dispose();
			CouponDiscountUsages = null;
			OpenInvoice?.Dispose();
			OpenInvoice = null;
			PlacementOrderRelation?.Dispose();
			PlacementOrderRelation = null;
			Reservation?.Dispose();
			Reservation = null;
			Gift?.Dispose();
			Gift = null;
			PrdGroupRelationRepository?.Dispose();
			PrdGroupRelationRepository = null;
			Post?.Dispose();
			Post = null;
			IAttPersonLeave?.Dispose();
			IAttPersonLeave = null;
			AttDepartment?.Dispose();
			AttDepartment = null;
			AttPerson?.Dispose();
			AttPerson = null;
			TaxOrderLog?.Dispose();
			TaxOrderLog = null;
			OrderDetailCouponCalculation?.Dispose();
			OrderDetailCouponCalculation = null;
			ProductPriceLevel?.Dispose();
			ProductPriceLevel = null;
			ProductPriceRelation?.Dispose();
			ProductPriceRelation = null;
			Evaluation?.Dispose();
			Evaluation = null;
			SmsOutBox?.Dispose();
			SmsOutBox = null;
			BioMetricData?.Dispose();
			BioMetricData = null;
			BioDeviceRelation?.Dispose();
			BioDeviceRelation = null;
			BioWriteDevice?.Dispose();
			BioWriteDevice = null;
			BioWriteDeviceLog?.Dispose();
			BioWriteDeviceLog = null;
			DeviceInfo?.Dispose();
			DeviceInfo = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AggregateSqlRepository.cs", 812);
		}
	}
}
