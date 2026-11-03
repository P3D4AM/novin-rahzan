using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Business.Accounting.Note;
using EntityCache.Business.Accounting.Reservation;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Business.Advertise;
using EntityCache.Business.Aggregators;
using EntityCache.Business.Attendance;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Calling;
using EntityCache.Business.Customers;
using EntityCache.Business.Global;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Indexes;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Business.Report;
using EntityCache.Business.Tickets;
using PacketParser.Entities;
using PacketParser.Events;

namespace EntityCache.Business;

public class BusinessAggregator
{
	public BusinessEventsAggregator Events { get; private set; }

	public FileInfoBusiness FileInfoBusiness { get; private set; }

	public ProductAttributeBusiness ProductAttributeBusiness { get; private set; }

	public PrdVahedBusiness PrdVahedBusiness { get; private set; }

	public ProductBusiness ProductBusiness { get; private set; }

	public PrdGroupBusiness PrdGroupBusiness { get; private set; }

	public CusBusiness CusBusiness { get; private set; }

	public CusGroupBusiness CusGroupBusiness { get; private set; }

	public CusTitleBusiness CusTitleBusiness { get; private set; }

	public LocalSettingBusiness LocalSettingsBusiness { get; private set; }

	public VamBusiness VamBusiness { get; private set; }

	public LastPriceBusiness LastPriceBusiness { get; private set; }

	public ReceptionBusiness ReceptionBusiness { get; private set; }

	public ContactUsBusiness ContactUsBusiness { get; private set; }

	public DepotBusiness DepotBusiness { get; private set; }

	public FileInfoReTryDownloadBusiness FileInfoReTryDownloadBusiness { get; private set; }

	public FullTextWordBusiness FullTextWordsBusiness { get; private set; }

	public HyperLinkBusiness HyperLinkBusiness { get; private set; }

	public CallingLogBusiness CallingLogBusiness { get; private set; }

	public NewsBusiness NewsBusiness { get; private set; }

	public OrderBusiness OrderBusiness { get; private set; }

	public PardakhtBusiness PardakhtBusiness { get; private set; }

	public PrdAnbarRelationBusiness PrdAnbarRelationBusiness { get; private set; }

	public UserInformationBusiness UserInformationBusiness { get; private set; }

	public UserAccBusiness UserAccBusiness { get; private set; }

	public UploadFileBusiness UploadFileBusiness { get; private set; }

	public UploadFilePartBusiness UploadFilePartBusiness { get; private set; }

	public TicketBusiness TicketBusiness { get; private set; }

	public TicketDetBusiness TicketDetBusiness { get; private set; }

	public TicketPartBusiness TicketPartBusiness { get; private set; }

	public TicketPartDetailBusiness TicketPartDetailBusiness { get; private set; }

	public BackupLogBusiness BackupLogBusiness { get; private set; }

	public BackupScheduleBusiness BackupScheduleBusiness { get; private set; }

	public BackupSettingsBusiness BackupSettingBusiness { get; private set; }

	public BackupTimeBusiness BackupTimeBusiness { get; private set; }

	public GPSBusiness GpsBusiness { get; private set; }

	public DepEnteghalBusiness DepEnteghalBusiness { get; private set; }

	public DepEnteghalDetBusiness DepEnteghalDetBusiness { get; private set; }

	public PrdAnbarGardaniBusiness PrdAnbarGardaniBusiness { get; private set; }

	public PrdAnbarGardaniDetBusiness PrdAnbarGardaniDetBusiness { get; private set; }

	public SanadBusiness SanadBusiness { get; private set; }

	public SanadStatusReportBusiness SanadStatusReportBusiness { get; private set; }

	public Sanad_DetBusiness Sanad_DetBusiness { get; private set; }

	public DescBusiness DescBusiness { get; private set; }

	public HesabTypeBusiness HesabTypeBusiness { get; private set; }

	public HesabGroupBusiness HesabGroupBusiness { get; private set; }

	public TafsilGroupBusiness TafsilGroupBusiness { get; private set; }

	public KolBusiness KolBusiness { get; private set; }

	public MoeinBusiness MoeinBusiness { get; private set; }

	public TafsilBusiness TafsilBusiness { get; private set; }

	public GardeshTafsilBusiness GardeshTafsilBusiness { get; private set; }

	public BarcodeBusiness BarcodeBusiness { get; private set; }

	public PrdSerialBusiness PrdSerialBusiness { get; private set; }

	public PrdTechnicalBusiness PrdTechnicalBusiness { get; private set; }

	public SuggestedBankBusiness SuggestedBankBusiness { get; private set; }

	public KardexBusiness KardexBusiness { get; private set; }

	public MoeinTafsilGroupRelationsBusiness MoeinTafsilGroupRelationsBusiness { get; private set; }

	public PardakhtBankBusiness PardakhtBankBusiness { get; private set; }

	public PardakhtCheckMBusiness PardakhtCheckMBusiness { get; private set; }

	public PardakhtCheckShBusiness PardakhtCheckShBusiness { get; private set; }

	public PardakhtNaghdBusiness PardakhtNaghdBusiness { get; private set; }

	public ReceptionCheckBusiness ReceptionCheckBusiness { get; private set; }

	public ReceptionHavaleBusiness ReceptionHavaleBusiness { get; private set; }

	public ReceptionNaghdBusiness ReceptionNaghdBusiness { get; private set; }

	public CashierBusiness CashierBusiness { get; private set; }

	public FareOrderCreditBusiness FareOrderCreditBusiness { get; private set; }

	public AbadDetBusiness AbadDetBusiness { get; private set; }

	public OrderAbadBusiness OrderAbadBusiness { get; private set; }

	public CusCardBusiness CusCardBusiness { get; private set; }

	public ChequeBookBusiness ChequeBookBusiness { get; private set; }

	public ChequeMOperationBusiness ChequeMOperationBusiness { get; private set; }

	public ChequeShOperationBusiness ChequeShOperationBusiness { get; private set; }

	public RasGiriBusiness RasGiriBusiness { get; private set; }

	public RasGiriDetBusiness RasGiriDetBusiness { get; private set; }

	public SanadPrintBusiness SanadPrintBusiness { get; private set; }

	public VisitBusiness VisitBusiness { get; private set; }

	public TafsilSelectedBusiness TafsilSelectedBusiness { get; private set; }

	public PrdSelectedBusiness PrdSelectedBusiness { get; private set; }

	public AghsatMBusiness AghsatMBusiness { get; private set; }

	public AghsatShBusiness AghsatShBusiness { get; private set; }

	public AghsatM_GroupBusiness AghsatM_GroupBusiness { get; private set; }

	public DecimalBusiness DecimalBusiness { get; private set; }

	public DecimalUpdatesBusiness DecimalUpdatesBusiness { get; private set; }

	public ResidBusiness ResidBusiness { get; private set; }

	public NoteBusiness NoteBusiness { get; private set; }

	public NoteGroupBusiness NoteGroupBusiness { get; private set; }

	public RegionBusiness RegionsBusiness { get; private set; }

	public TafsilNumberBusiness TafsilNumberBusiness { get; private set; }

	public En2FaBusiness En2FaBusiness { get; private set; }

	public ResidStatusBusiness ResidStatusBusiness { get; private set; }

	public NaghzBusiness NaghzBusiness { get; private set; }

	public LotteryBusiness LotteryBusiness { get; private set; }

	public LotteryScoreBusiness LotteryScoresBusiness { get; private set; }

	public CheckedSaveBusiness CheckedSavesBusiness { get; private set; }

	public ShortCutBusiness ShortCutBusiness { get; private set; }

	public ReportGeneratorBusiness ReportGeneratorBusiness { get; private set; }

	public ReportGeneratorDetBusiness ReportGeneratorDetBusiness { get; private set; }

	public ViewCheckMBusiness ViewCheckMBusiness { get; private set; }

	public ViewCheckShBusiness ViewCheckShBusiness { get; private set; }

	public CallingLineBusiness CallingLineBusiness { get; private set; }

	public OwnerBusiness OwnerBusiness { get; private set; }

	public VisitorBusiness VisitorBusiness { get; private set; }

	public AdvertiseLogBusiness AdvertiseLogBusiness { get; private set; }

	public SimCardBusiness SimCardsBusiness { get; private set; }

	public ContentBlackListBusiness ContentBlackListBusiness { get; private set; }

	public StateBusiness StateBusiness { get; private set; }

	public VisitorCityBusiness VisitorCityBusiness { get; private set; }

	public VisitorAdvBusiness VisitorAdvBusiness { get; private set; }

	public TitleBlackListBusiness TitleBlackListsBusiness { get; private set; }

	public CityBusiness CityBusiness { get; private set; }

	public DivarCityBusiness DivarCityBusiness { get; private set; }

	public SheypoorCityBusiness SheypoorCityBusiness { get; private set; }

	public AdvTokensBusiness AdvTokensBusiness { get; private set; }

	public AdvCategoryBusiness AdvCategoryBusiness { get; private set; }

	public AdvGroupBusiness AdvGroupBusiness { get; private set; }

	public ChatNumberBusiness ChatNumberBusiness { get; private set; }

	public AdvertiseBusiness AdvertiseBusiness { get; private set; }

	public AdvDepartmentBusiness AdvDepartmentBusiness { get; private set; }

	public CategoryGroupBusiness CategoryGroupBusiness { get; private set; }

	public CashiersLogBusiness CashiersLogBusiness { get; private set; }

	public MetaDataBusiness MetaDataBusiness { get; private set; }

	public ExecuteQueryAggregator ExecuteQueryAggregator { get; set; }

	public TolidBusiness TolidBusiness { get; private set; }

	public ReceiptBusiness ReceiptBusiness { get; private set; }

	public CouponDiscountBusiness CouponDiscountBusiness { get; private set; }

	public TaxBusiness TaxBusiness { get; private set; }

	public ProductGroupRelationBusiness PrdGroupRelationBusiness { get; private set; }

	public PlacementBusiness PlacementBusiness { get; private set; }

	public CallingLogDetailBusiness CallingLogDetailBusiness { get; private set; }

	public DepotReceiptBusiness DepotReceipt { get; private set; }

	public CardBusiness Card { get; private set; }

	public BedehiTalabReportBusiness BedehiTalab { get; private set; }

	public OpenInvoiceBusiness OpenInvoice { get; private set; }

	public ReservationBusiness Reservation { get; private set; }

	public BioWriteDeviceBusiness BioWriteDevice { get; private set; }

	public AttPersonLeaveBusiness PersonLeave { get; private set; }

	public static async Task<BusinessAggregator> GetFactoryAsync(string context, IEnumerable<EnCacheConfig> configs)
	{
		BusinessAggregator ret = new BusinessAggregator
		{
			Events = new BusinessEventsAggregator(configs),
			ExecuteQueryAggregator = new ExecuteQueryAggregator(context)
		};
		if (configs?.Any((EnCacheConfig x) => x == EnCacheConfig.Global) ?? false)
		{
			ret.FileInfoBusiness = new FileInfoBusiness(context);
			ret.FileInfoReTryDownloadBusiness = new FileInfoReTryDownloadBusiness(context);
			ret.LocalSettingsBusiness = new LocalSettingBusiness(context);
			ret.ContactUsBusiness = new ContactUsBusiness(context);
			ret.HyperLinkBusiness = new HyperLinkBusiness(context);
			ret.NewsBusiness = new NewsBusiness(context);
			ret.UserInformationBusiness = new UserInformationBusiness(context);
			ret.UploadFileBusiness = new UploadFileBusiness(context);
			ret.UploadFilePartBusiness = new UploadFilePartBusiness(context);
			ret.TicketBusiness = new TicketBusiness(context);
			ret.TicketDetBusiness = new TicketDetBusiness(context);
			ret.TicketPartBusiness = new TicketPartBusiness(context);
			ret.TicketPartDetailBusiness = new TicketPartDetailBusiness(context);
			ret.BackupLogBusiness = new BackupLogBusiness(context);
			ret.BackupScheduleBusiness = new BackupScheduleBusiness(context);
			ret.BackupSettingBusiness = new BackupSettingsBusiness(context);
			ret.BackupTimeBusiness = new BackupTimeBusiness(context);
			ret.DescBusiness = new DescBusiness(context);
			ret.En2FaBusiness = new En2FaBusiness(context);
			ret.NaghzBusiness = new NaghzBusiness(context);
			ret.MetaDataBusiness = new MetaDataBusiness(context);
			ret.Card = new CardBusiness(context);
		}
		if (configs?.Any((EnCacheConfig x) => x == EnCacheConfig.Advertise) ?? false)
		{
			ret.AdvertiseLogBusiness = new AdvertiseLogBusiness(context);
			ret.AdvTokensBusiness = new AdvTokensBusiness(context);
			ret.AdvCategoryBusiness = new AdvCategoryBusiness(context);
			ret.AdvGroupBusiness = new AdvGroupBusiness(context);
			ret.AdvDepartmentBusiness = new AdvDepartmentBusiness(context);
			ret.AdvertiseBusiness = new AdvertiseBusiness(context);
			ret.VisitorBusiness = new VisitorBusiness(context);
			ret.OwnerBusiness = new OwnerBusiness(context);
			ret.SimCardsBusiness = new SimCardBusiness(context);
			ret.CityBusiness = new CityBusiness(context);
			ret.StateBusiness = new StateBusiness(context);
			ret.VisitorCityBusiness = new VisitorCityBusiness(context);
			ret.TitleBlackListsBusiness = new TitleBlackListBusiness(context);
			ret.ContentBlackListBusiness = new ContentBlackListBusiness(context);
			ret.DivarCityBusiness = new DivarCityBusiness(context);
			ret.SheypoorCityBusiness = new SheypoorCityBusiness(context);
			ret.VisitorAdvBusiness = new VisitorAdvBusiness(context);
			ret.ChatNumberBusiness = new ChatNumberBusiness(context);
			ret.CategoryGroupBusiness = new CategoryGroupBusiness(context);
		}
		if (configs?.Any((EnCacheConfig x) => x == EnCacheConfig.Accounting) ?? false)
		{
			ret.ProductAttributeBusiness = new ProductAttributeBusiness(context);
			ret.PrdVahedBusiness = new PrdVahedBusiness(context);
			ret.ProductBusiness = new ProductBusiness(context);
			ret.PrdGroupBusiness = new PrdGroupBusiness(context);
			ret.CusBusiness = new CusBusiness(context);
			ret.CusGroupBusiness = new CusGroupBusiness(context);
			ret.CusTitleBusiness = new CusTitleBusiness(context);
			ret.VamBusiness = new VamBusiness(context);
			ret.LastPriceBusiness = new LastPriceBusiness(context);
			ret.ReceptionBusiness = new ReceptionBusiness(context);
			ret.DepotBusiness = new DepotBusiness(context);
			ret.FullTextWordsBusiness = new FullTextWordBusiness(context);
			ret.CallingLogBusiness = new CallingLogBusiness(context);
			ret.OrderBusiness = new OrderBusiness(context);
			ret.UserAccBusiness = new UserAccBusiness(context, Guid.Empty);
			ret.PardakhtBusiness = new PardakhtBusiness(context);
			ret.PrdAnbarRelationBusiness = new PrdAnbarRelationBusiness(context);
			ret.GpsBusiness = new GPSBusiness(context);
			ret.DepEnteghalBusiness = new DepEnteghalBusiness(context);
			ret.DepEnteghalDetBusiness = new DepEnteghalDetBusiness(context);
			ret.PrdAnbarGardaniBusiness = new PrdAnbarGardaniBusiness(context);
			ret.PrdAnbarGardaniDetBusiness = new PrdAnbarGardaniDetBusiness(context);
			ret.SanadBusiness = new SanadBusiness(context, 0);
			ret.SanadStatusReportBusiness = new SanadStatusReportBusiness(context);
			ret.Sanad_DetBusiness = new Sanad_DetBusiness(context, EnSystem.None, Guid.NewGuid(), Document.None, 1);
			ret.HesabTypeBusiness = new HesabTypeBusiness(context);
			ret.HesabGroupBusiness = new HesabGroupBusiness(context);
			ret.TafsilGroupBusiness = new TafsilGroupBusiness(context);
			ret.KolBusiness = new KolBusiness(context);
			ret.MoeinBusiness = new MoeinBusiness(context);
			ret.TafsilBusiness = new TafsilBusiness(context);
			ret.GardeshTafsilBusiness = new GardeshTafsilBusiness(context);
			ret.BarcodeBusiness = new BarcodeBusiness(context);
			ret.PrdSerialBusiness = new PrdSerialBusiness(context);
			ret.PrdTechnicalBusiness = new PrdTechnicalBusiness(context);
			ret.SuggestedBankBusiness = new SuggestedBankBusiness(context);
			ret.KardexBusiness = new KardexBusiness(context, null);
			ret.MoeinTafsilGroupRelationsBusiness = new MoeinTafsilGroupRelationsBusiness(context);
			ret.PardakhtBankBusiness = new PardakhtBankBusiness(context);
			ret.PardakhtCheckMBusiness = new PardakhtCheckMBusiness(context);
			ret.PardakhtCheckShBusiness = new PardakhtCheckShBusiness(context);
			ret.PardakhtNaghdBusiness = new PardakhtNaghdBusiness(context);
			ret.ReceptionCheckBusiness = new ReceptionCheckBusiness(context);
			ret.ReceptionHavaleBusiness = new ReceptionHavaleBusiness(context);
			ret.ReceptionNaghdBusiness = new ReceptionNaghdBusiness(context);
			ret.CashierBusiness = new CashierBusiness(context);
			ret.FareOrderCreditBusiness = new FareOrderCreditBusiness(context);
			ret.AbadDetBusiness = new AbadDetBusiness(context);
			ret.OrderAbadBusiness = new OrderAbadBusiness(context);
			ret.CusCardBusiness = new CusCardBusiness(context);
			ret.ChequeBookBusiness = new ChequeBookBusiness(context);
			ret.ChequeMOperationBusiness = new ChequeMOperationBusiness(context);
			ret.ChequeShOperationBusiness = new ChequeShOperationBusiness(context);
			ret.RasGiriBusiness = new RasGiriBusiness(context);
			ret.RasGiriDetBusiness = new RasGiriDetBusiness(context);
			ret.SanadPrintBusiness = new SanadPrintBusiness(context);
			ret.VisitBusiness = new VisitBusiness(context);
			ret.TafsilSelectedBusiness = new TafsilSelectedBusiness(context);
			ret.PrdSelectedBusiness = new PrdSelectedBusiness(context);
			ret.CashiersLogBusiness = new CashiersLogBusiness(context);
			ret.AghsatMBusiness = new AghsatMBusiness(context);
			ret.AghsatShBusiness = new AghsatShBusiness(context);
			ret.AghsatM_GroupBusiness = new AghsatM_GroupBusiness(context);
			ret.DecimalBusiness = new DecimalBusiness(context);
			ret.DecimalUpdatesBusiness = new DecimalUpdatesBusiness(context);
			ret.ResidBusiness = new ResidBusiness(context);
			ret.NoteBusiness = new NoteBusiness(context);
			ret.NoteGroupBusiness = new NoteGroupBusiness(context);
			ret.TafsilNumberBusiness = new TafsilNumberBusiness(context);
			ret.ResidStatusBusiness = new ResidStatusBusiness(context);
			ret.LotteryBusiness = new LotteryBusiness(context);
			BusinessAggregator businessAggregator = ret;
			businessAggregator.TolidBusiness = await TolidBusiness.GenerateNewAsync(context, EnSystem.None);
			ret.ShortCutBusiness = new ShortCutBusiness(context);
			ret.ReportGeneratorBusiness = new ReportGeneratorBusiness(context);
			ret.ReportGeneratorDetBusiness = new ReportGeneratorDetBusiness(context);
			ret.ViewCheckMBusiness = new ViewCheckMBusiness(context);
			ret.ViewCheckShBusiness = new ViewCheckShBusiness(context);
			businessAggregator = ret;
			businessAggregator.CallingLineBusiness = await CallingLineBusiness.FactoryAsync(context);
			ret.ReceiptBusiness = new ReceiptBusiness(context);
			ret.CouponDiscountBusiness = new CouponDiscountBusiness(context);
			ret.PrdGroupRelationBusiness = new ProductGroupRelationBusiness(context);
			ret.PlacementBusiness = new PlacementBusiness(context);
			ret.DepotReceipt = new DepotReceiptBusiness(context);
			ret.TaxBusiness = new TaxBusiness(context);
			ret.BedehiTalab = new BedehiTalabReportBusiness(context);
			ret.OpenInvoice = new OpenInvoiceBusiness(context);
			ret.Reservation = new ReservationBusiness(context);
		}
		if (configs?.Any((EnCacheConfig x) => x == EnCacheConfig.Attendance) ?? false)
		{
			BusinessAggregator businessAggregator = ret;
			businessAggregator.BioWriteDevice = await BioWriteDeviceBusiness.GenerateNewAsync(context, DateTime.Now, EnDeviceMessageType.AddOrUpdate);
			ret.PersonLeave = new AttPersonLeaveBusiness(context);
		}
		return ret;
	}
}
