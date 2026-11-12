using Server.Core.Attendance;
using Server.Core.Attendance.AttPersonLeave;
using Server.Core.Log;
using Server.Core.SMS;
using Server.Core.TaxService;
using Server.Core.Ticket;

namespace Server.Core;

public class AggregateDal
{
	public IDalCallingLog CallingLog { get; }

	public IDalCus Cus { get; }

	public ISmsParserTemplatePersistenseRepository SmsParserTemplate { get; }

	public IDalUserLog UserLog { get; }

	public IDalCusGroup CusGroup { get; }

	public IDalCusTitle CusTitle { get; }

	public IDalCustomerRole CustomerRole { get; }

	public IDalDepot Depot { get; }

	public IDalHesabTree HesabTree { get; }

	public IDalOrder Order { get; }

	public IDalPardakht Pardakht { get; }

	public IDalReception Reception { get; }

	public IDalSanad Sanad { get; }

	public IDalProduct Product { get; }

	public IDalProductGroup ProductGroup { get; }

	public IDalProductVahed ProductVahed { get; }

	public IDalUserAcc UserAcc { get; }

	public IDalSync Sync { get; }

	public IDalApplicationSettings Settings { get; }

	public IDalContactsUs ContactUs { get; }

	public IDalDelivery Delivery { get; }

	public IDalNews News { get; }

	public IDalHyperLink HyperLink { get; }

	public IDalUser User { get; }

	public IDalWooMapping WooMapping { get; set; }

	public IDalDepartment Department { get; }

	public IDalUserInFormation UserInformation { get; }

	public IDalGps Gps { get; }

	public IDalErrorLogSum ErrorLogSum { get; }

	public IDalFileInfo FileInfo { get; }

	public IDalSignalRLogger SignalRLogger { get; }

	public IDalApplicationSettings ApplicationSetting { get; }

	public IDalBufferPackets BufferPackets { get; }

	public IDalTicket Ticket { get; }

	public IDalTicketDet TicketDet { get; }

	public IDalTicketPart TicketPart { get; }

	public IDalDesc Desc { get; }

	public IDalPrdAnbarGardani PrdAnbarGardani { get; }

	public IDalVisit Visit { get; }

	public IServerLog ServerLog { get; }

	public IDalWooPendingRequest WooPendingRequest { get; }

	public IDalAghsatShakhsi AghsatShakhsi { get; }

	public IDalAghsatMGroup AghsatMGroup { get; }

	public IDalAghsatAshkhas AghsatAshkhas { get; }

	public IDalClientLoginHistory ClientLoginHistory { get; }

	public IDalViewCheckM ViewCheckM { get; }

	public IDalViewCheckSh ViewCheckSh { get; }

	public IDalTafsilGroup DalTafsilGroup { get; }

	public IDalDeliveryPartitioned DalDeliveryPartitioned { get; }

	public IDalPacketLog DalPacketLog { get; }

	public IDalPacketLogSum DalPacketLogSum { get; }

	public IDalEntitiesDeliveryStatus DalEntitiesDeliveryStatus { get; }

	public IAttendaceLogOnAdminRepository AttendaceLogOnAdminRepository { get; }

	public IAttendaceLogOnAlarmRepository AttendaceLogOnAlarmRepository { get; }

	public IAttendaceLogOnPersonRepository AttendaceLogOnPersonRepository { get; }

	public IAttendanceDeviceInfoRepository AttendanceDeviceInfoRepository { get; }

	public ILastPriceRepository LastPriceRepository { get; }

	public IDalMoein Moein { get; }

	public IDalKol Kol { get; }

	public IDalTafsil Tafsil { get; }

	public IDalDepEnteghal DepEnteghal { get; }

	public IDalLkInfo LkInfo { get; }

	public IDalChequeMOperation ChequeMOperation { get; }

	public bool ResetTimeFlag { get; set; }

	public IDalChequeShOperation ChequeShOperation { get; }

	public IDalPrdSerial Serial { get; set; }

	public IDalIntroductionMethod IntroductionMethod { get; }

	public IDalSyncEntity SyncEntity { get; }

	public IDalAccount Account { get; }

	public IDalNote Note { get; }

	public IDalTaxOrder TaxSrvice { get; }

	public IDalAttPersonLeave AttPersonLeave { get; }

	public IDalAttDepartment AttDepartment { get; }

	public IDalDiscountCoupon DiscountCoupon { get; }

	public IDalSanadPrint SanadPrint { get; }

	public IDalTax TaxClass { get; }

	public IDalMessage Message { get; }

	public IDalMessageSession MessageSession { get; }

	public IDalDepartmentEntitySummery DepartmentEntitySummery { get; }

	public IDalTaxError TaxError { get; }

	public ISMS_WhiteList Sms_WhiteList { get; }

	public IDalGoldAndCurrency TalaVaArz { get; }

	public IDalGoldAndCurrencyDetail TalaVaArzDetail { get; }

	public IDalLinkGenerator LinkGenerator { get; }

	public IDalSms Sms { get; }

	public IDalAttPersonReport PersonReport { get; }

	public IDalTaxOrderLog TaxOrderLog { get; }

	public IDalProductPriceLevel ProductPriceLevel { get; }

	public IDalPaymentGateway PaymentGateway { get; }

	public IDalSanadStatusReport SanadStatusReport { get; }

	public IDalAttendanceDeviceCommunicationService DeviceService { get; }

	public IDalWooCommerce WooCommerce { get; }

	public IDalSyncEntityByHash SyncEntityByHash { get; }

	public AggregateDal(IDalCus cus, IDalCusGroup cusGroup, IDalCusTitle cusTitle, IDalDepot depot, IDalHesabTree hesabtree, IDalOrder order, IDalPardakht pardakht, IDalReception reception, IDalProduct product, IDalProductGroup productGroup, IDalProductVahed productVahed, IDalUserAcc userAcc, IDalSync sync, IDalApplicationSettings settings, IDalContactsUs contactUs, IDalDelivery delivery, IDalNews news, IDalHyperLink hyperLink, IDalGps gps, IDalUser user, IDalDepartment department, IDalUserInFormation userInformation, IDalErrorLogSum errorLogSum, IDalFileInfo fileInfo, IDalSignalRLogger signalRLogger, IDalApplicationSettings applicationSetting, IDalBufferPackets bufferPackets, IDalTicket ticket, IDalTicketDet ticketDet, IDalTicketPart ticketPart, IDalDesc desc, IDalVisit visit, IServerLog serverLog, IDalWooPendingRequest wooPendingRequest, IDalAghsatShakhsi aghsatShakhsi, IDalAghsatAshkhas aghsatAshkhas, IDalClientLoginHistory clientLoginHistory, IDalViewCheckM dalViewCheckM, IDalViewCheckSh dalViewCheckSh, IDalTafsilGroup dalTafsilGroup, IDalCallingLog callingLog, IDalDeliveryPartitioned dalDeliveryPartitioned, IDalPacketLog dalPacketLog, IDalPacketLogSum dalPacketLogSum, IDalEntitiesDeliveryStatus entitiesDeliveryStatus, IAttendaceLogOnAdminRepository attendaceLogOnAdminRepository, IAttendaceLogOnAlarmRepository attendaceLogOnAlarmRepository, IAttendaceLogOnPersonRepository attendaceLogOnPersoneRepository, IAttendanceDeviceInfoRepository attendanceDeviceInfoRepository, ILastPriceRepository lastPriceRepository, IDalKol kol, IDalMoein moein, IDalTafsil tafsil, IDalUserLog userLog, IDalSanad sanad, IDalPrdAnbarGardani prdAnbargardani, IDalDepEnteghal depEnteghal, IDalLkInfo lkInfo, IDalChequeMOperation chequeMOperation, IDalChequeShOperation chequeShOperation, IDalPrdSerial serial, IDalIntroductionMethod introductionMethod, IDalSyncEntity dalSyncEntity, IDalWooMapping wooMapping, IDalAccount account, IDalCustomerRole role, IDalNote note, IDalTaxOrder taxSrvice, IDalAttPersonLeave attPersonLeave, IDalDiscountCoupon coupon, IDalMessage message, IDalMessageSession messageSession, IDalDepartmentEntitySummery webDepartmentEntitySummery, IDalTaxError taxerror, ISMS_WhiteList sms_WhiteList, IDalGoldAndCurrency talaVaArz, IDalAttDepartment attDepartment, IDalTax tax, IDalGoldAndCurrencyDetail talaVaArzDetail, IDalLinkGenerator link, IDalSms sms, IDalAttPersonReport personReport, IDalTaxOrderLog taxOrderLog, IDalAghsatMGroup aghsatMGroup, ISmsParserTemplatePersistenseRepository smsParserTemplate, IDalProductPriceLevel productPriceLevel, IDalPaymentGateway paymentGateway, IDalSanadStatusReport sanadStatusReport, IDalAttendanceDeviceCommunicationService deviceService, IDalWooCommerce wooCommerce, IDalSyncEntityByHash syncEntityByHash, IDalSanadPrint sanadPrint)
	{
		Cus = cus;
		CusGroup = cusGroup;
		CusTitle = cusTitle;
		Depot = depot;
		HesabTree = hesabtree;
		Order = order;
		Pardakht = pardakht;
		Reception = reception;
		Product = product;
		ProductGroup = productGroup;
		ProductVahed = productVahed;
		UserAcc = userAcc;
		Sync = sync;
		Settings = settings;
		ContactUs = contactUs;
		HyperLink = hyperLink;
		News = news;
		Delivery = delivery;
		Gps = gps;
		User = user;
		WooMapping = wooMapping;
		Department = department;
		UserInformation = userInformation;
		ErrorLogSum = errorLogSum;
		FileInfo = fileInfo;
		SignalRLogger = signalRLogger;
		ApplicationSetting = applicationSetting;
		BufferPackets = bufferPackets;
		WooPendingRequest = wooPendingRequest;
		Ticket = ticket;
		TicketDet = ticketDet;
		TicketPart = ticketPart;
		Desc = desc;
		Visit = visit;
		ServerLog = serverLog;
		AghsatShakhsi = aghsatShakhsi;
		AghsatMGroup = aghsatMGroup;
		AghsatAshkhas = aghsatAshkhas;
		ClientLoginHistory = clientLoginHistory;
		ViewCheckM = dalViewCheckM;
		ViewCheckSh = dalViewCheckSh;
		DalTafsilGroup = dalTafsilGroup;
		CallingLog = callingLog;
		DalDeliveryPartitioned = dalDeliveryPartitioned;
		DalPacketLog = dalPacketLog;
		DalPacketLogSum = dalPacketLogSum;
		DalEntitiesDeliveryStatus = entitiesDeliveryStatus;
		AttendaceLogOnAdminRepository = attendaceLogOnAdminRepository;
		AttendaceLogOnAlarmRepository = attendaceLogOnAlarmRepository;
		AttendaceLogOnPersonRepository = attendaceLogOnPersoneRepository;
		AttendanceDeviceInfoRepository = attendanceDeviceInfoRepository;
		LastPriceRepository = lastPriceRepository;
		Kol = kol;
		Moein = moein;
		Tafsil = tafsil;
		UserLog = userLog;
		Sanad = sanad;
		PrdAnbarGardani = prdAnbargardani;
		DepEnteghal = depEnteghal;
		LkInfo = lkInfo;
		ChequeMOperation = chequeMOperation;
		ChequeShOperation = chequeShOperation;
		Serial = serial;
		IntroductionMethod = introductionMethod;
		SyncEntity = dalSyncEntity;
		Account = account;
		CustomerRole = role;
		Note = note;
		TaxSrvice = taxSrvice;
		AttPersonLeave = attPersonLeave;
		AttDepartment = attDepartment;
		DiscountCoupon = coupon;
		TaxClass = tax;
		Message = message;
		MessageSession = messageSession;
		DepartmentEntitySummery = webDepartmentEntitySummery;
		TaxError = taxerror;
		Sms_WhiteList = sms_WhiteList;
		TalaVaArz = talaVaArz;
		TalaVaArzDetail = talaVaArzDetail;
		LinkGenerator = link;
		Sms = sms;
		PersonReport = personReport;
		TaxOrderLog = taxOrderLog;
		SmsParserTemplate = smsParserTemplate;
		ProductPriceLevel = productPriceLevel;
		PaymentGateway = paymentGateway;
		SanadStatusReport = sanadStatusReport;
		DeviceService = deviceService;
		WooCommerce = wooCommerce;
		SyncEntityByHash = syncEntityByHash;
		SanadPrint = sanadPrint;
	}
}
