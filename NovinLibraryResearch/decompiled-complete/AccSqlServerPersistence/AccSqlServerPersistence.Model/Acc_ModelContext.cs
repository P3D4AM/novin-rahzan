using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Accounting.Crm;
using AccSqlServerPersistence.Entities.Accounting.Note;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Accounting.TozieLists;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Entities.Indexes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using PacketParser;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Entities.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Crm;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Gift;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;
using SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;
using SharedAccEntityFramworkPersistence.Entities.Customer;

namespace AccSqlServerPersistence.Model;

public class Acc_ModelContext : BaseModelContext
{
	public virtual DbSet<Evaluation> Evaluation { get; set; }

	public virtual DbSet<ProductPriceRelation> ProductPriceRelation { get; set; }

	public virtual DbSet<ProductPriceLevel> ProductPriceLevel { get; set; }

	public virtual DbSet<TempBarcodePrint> TempBarcodePrint { get; set; }

	public virtual DbSet<OpenInvoice> OpenInvoice { get; set; }

	public virtual DbSet<OpenInvoiceOrderRelation> OpenInvoiceOrderRelation { get; set; }

	public virtual DbSet<OpenInvoicePlacementRelation> OpenInvoicePlacementRelation { get; set; }

	public virtual DbSet<PlacementOrderRelation> PlacementOrderRelation { get; set; }

	public virtual DbSet<Reservation> Reservation { get; set; }

	public virtual DbSet<ReservationDetail> ReservationDetail { get; set; }

	public virtual DbSet<TaxClass> Tax { get; set; }

	public virtual DbSet<TaxClassDetail> TaxDetail { get; set; }

	public virtual DbSet<ChequeMOperation> ChequeMOperation { get; set; }

	public virtual DbSet<ViewCheckSh> ViewCheckSh { get; set; }

	public virtual DbSet<AghsatM> AghsatM { get; set; }

	public virtual DbSet<AghsatM_Group> AghsatM_Group { get; set; }

	public virtual DbSet<AghsatSh> AghsatSh { get; set; }

	public virtual DbSet<HesabGroup> HesabGroup { get; set; }

	public virtual DbSet<AccountClass> Accounts { get; set; }

	public virtual DbSet<AccountLog> AccountLog { get; set; }

	public virtual DbSet<AccountBallance> AccountBallance { get; set; }

	public virtual DbSet<Kol> Kol { get; set; }

	public virtual DbSet<MoeinTafsilGroupRelations> MoeinTafsilLevelRelation { get; set; }

	public virtual DbSet<SuggestedBank> SuggestedBanks { get; set; }

	public virtual DbSet<GardeshTafsil> GardeshTafsil { get; set; }

	public virtual DbSet<Moein> Moein { get; set; }

	public virtual DbSet<TafsilNumber> TafsilNumber { get; set; }

	public virtual DbSet<Tafsil> Tafsil { get; set; }

	public virtual DbSet<CusCard> CustomerCard { get; set; }

	public virtual DbSet<Customer> Customers { get; set; }

	public virtual DbSet<CustomerRole> CustomerRole { get; set; }

	public virtual DbSet<CustomerTitle> CustomersTitle { get; set; }

	public virtual DbSet<CustomerGroup> Customers_group { get; set; }

	public virtual DbSet<TafsilGroup> TafsilLevel { get; set; }

	public virtual DbSet<DecimalEntity> Decimalentity { get; set; }

	public virtual DbSet<DecimalUpdates> DecimalUpdates { get; set; }

	public virtual DbSet<DepotReceipt> DepotReceipt { get; set; }

	public virtual DbSet<DepotReceiptDetail> DepotReceiptDetail { get; set; }

	public virtual DbSet<CouponDiscount> CouponDiscount { get; set; }

	public virtual DbSet<CouponDiscountDetail> CouponDiscountDetail { get; set; }

	public virtual DbSet<CouponDiscountEntityRelation> CouponDiscountEntityRelation { get; set; }

	public virtual DbSet<CouponDiscountUsages> CouponDiscountUsages { get; set; }

	public virtual DbSet<Gift> Gift { get; set; }

	public virtual DbSet<Receipt> Receipt { get; set; }

	public virtual DbSet<ReceiptDetail> ReciptDetals { get; set; }

	public virtual DbSet<Hall> Hall { get; set; }

	public virtual DbSet<Placement> Placement { get; set; }

	public virtual DbSet<PlacementFacilitiesRelation> PlacementFacilitieRelation { get; set; }

	public virtual DbSet<Facilities> Facilities { get; set; }

	public virtual DbSet<CustomerReceiptRelation> CustomerReciptRelation { get; set; }

	public virtual DbSet<PardakhtChequeEntezami> PardakhtChequeEntezami { get; set; }

	public virtual DbSet<ReceptionChequeEntezami> ReceptionChequeEntezami { get; set; }

	public virtual DbSet<OrderDetailCouponCalculation> OrderDetailCouponCalculation { get; set; }

	public virtual DbSet<TaxOrderLog> TaxOrderLog { get; set; }

	public virtual DbSet<ProductGroupRelation> PrdGroupRelation { get; set; }

	public virtual DbSet<ProductDecimalRelation> ProductPrices { get; set; }

	public virtual DbSet<ProductPriceUpdate> ProductPriceUpdate { get; set; }

	public virtual DbSet<TozieList> TozieList { get; set; }

	public virtual DbSet<TozieListDetail> TozieListDetail { get; set; }

	public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

	public virtual DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }

	public virtual DbSet<ProductAttributeRelation> ProductAttributeRelations { get; set; }

	public virtual DbSet<PrdTechnical> PrdTechnicals { get; set; }

	public virtual DbSet<Resid> Resid { get; set; }

	public virtual DbSet<ResidStatus> ResidStatus { get; set; }

	public virtual DbSet<Note> Note { get; set; }

	public virtual DbSet<NoteGroup> NoteGroup { get; set; }

	public virtual DbSet<SanadStatusReport> SanadStatusReport { get; set; }

	public virtual DbSet<ViewSanadDetailCover> ViewSanadDetailCover { get; set; }

	public virtual DbSet<ViewSanadDetail> ViewSanadDetail { get; set; }

	public virtual DbSet<SanadStatusReportUpdates> SanadStatusReportUpdates { get; set; }

	public virtual DbSet<ProductGroupSelected> ProductGroupSelected { get; set; }

	public virtual DbSet<ProductGroupSelectedDetails> ProductGroupSelectedDetails { get; set; }

	public virtual DbSet<ShortCut> ShortCuts { get; set; }

	public virtual DbSet<ChequeBook> ChequeBook { get; set; }

	public virtual DbSet<ChequeShOperation> ChequeShOperation { get; set; }

	public virtual DbSet<ViewCheckM> ViewCheckM { get; set; }

	public virtual DbSet<LastPrice> LastPrice { get; set; }

	public virtual DbSet<Pardakht> Pardakht { get; set; }

	public virtual DbSet<Reception> Receptions { get; set; }

	public virtual DbSet<IxProductAllFields> IxProductAllFieldses { get; set; }

	public virtual DbSet<IxProductNameAndCodeField> IxProductNameAndCodeFields { get; set; }

	public virtual DbSet<IxProductNameField> IxProductNameFields { get; set; }

	public virtual DbSet<IxProductCodeField> IxProductCodeFields { get; set; }

	public virtual DbSet<IxProductDescField> IxProductDescFields { get; set; }

	public virtual DbSet<IxProductT1Field> IxProductT1Fields { get; set; }

	public virtual DbSet<IxProductT2Field> IxProductT2Fields { get; set; }

	public virtual DbSet<IxProductT3Field> IxProductT3Fields { get; set; }

	public virtual DbSet<IxProductT4Field> IxProductT4Fields { get; set; }

	public virtual DbSet<IxProductT5Field> IxProductT5Fields { get; set; }

	public virtual DbSet<IxProductT6Field> IxProductT6Fields { get; set; }

	public virtual DbSet<IxProductT7Field> IxProductT7Fields { get; set; }

	public virtual DbSet<IxProductT8Field> IxProductT8Fields { get; set; }

	public virtual DbSet<FullTextWords_temp> FullTextWordsTemp { get; set; }

	public virtual DbSet<IxCustomerAllFields> IxCustomers { get; set; }

	public virtual DbSet<FareOrderCredit> FareOrderCredits { get; set; }

	public virtual DbSet<PrdAnbarGardaniDet> PrdAnbarGardaniDet { get; set; }

	public virtual DbSet<PrdAnbarGardani> PrdAnbarGardani { get; set; }

	public virtual DbSet<DepEnteghal> DepEnteghals { get; set; }

	public virtual DbSet<DepEnteghalDet> DepEnteghalDets { get; set; }

	public virtual DbSet<PrdSerial> PrdSerials { get; set; }

	public virtual DbSet<PrdSerialDetails> PrdSerialDetails { get; set; }

	public virtual DbSet<PrdAnbarRelation> PrdAnbarRelation { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<Sanad_Det> SanadDets { get; set; }

	public virtual DbSet<SanadPrint> SanadPrint { get; set; }

	public virtual DbSet<Sanads> Sanads { get; set; }

	public virtual DbSet<SanadArchives> SanadArchives { get; set; }

	public virtual DbSet<CallingLine> CallingLine { get; set; }

	public virtual DbSet<CallingLog> CallingLog { get; set; }

	public virtual DbSet<CallingLogDetail> CallingLogDetail { get; set; }

	public virtual DbSet<Lottery> Lottery { get; set; }

	public virtual DbSet<LotteryScore> LotteryScores { get; set; }

	public virtual DbSet<PrdSelected> ListPrds { get; set; }

	public virtual DbSet<Prd_SelectedDetails> ListPrdDetails { get; set; }

	public virtual DbSet<TafsilSelected> ListCustomers { get; set; }

	public virtual DbSet<TafsilSelectedDetails> ListCustomerDetails { get; set; }

	public virtual DbSet<TolidFormulaExport> TolidFormulaExports { get; set; }

	public virtual DbSet<TolidFormulaHazine> TolidFormulaHazines { get; set; }

	public virtual DbSet<TolidFormulaMavad> TolidFormulaMavads { get; set; }

	public virtual DbSet<TolidExport> TolidExports { get; set; }

	public virtual DbSet<TolidHazine> TolidHazines { get; set; }

	public virtual DbSet<TolidMavad> TolidMavads { get; set; }

	public virtual DbSet<TolidSummary> TolidSummaries { get; set; }

	public virtual DbSet<TolidSummaryDetail> TolidSummaryDetails { get; set; }

	public virtual DbSet<Tolid> Tolid { get; set; }

	public virtual DbSet<RasGiri> RasGiri { get; set; }

	public virtual DbSet<RasGiriDet> RasGiriDet { get; set; }

	public virtual DbSet<Vam> Vam { get; set; }

	public virtual DbSet<TTMS> TTMS { get; set; }

	public virtual DbSet<TTMSDetail> TTMSDetails { get; set; }

	public virtual DbSet<Visit> Visit { get; set; }

	public virtual DbSet<PardakhtCheckSh> PardakhtCheckSh { get; set; }

	public virtual DbSet<PardakhtCheckM> PardakhtCheckM { get; set; }

	public virtual DbSet<PardakhtNaghd> PardakhtNaghd { get; set; }

	public virtual DbSet<PardakhtBank> PardakhtBank { get; set; }

	public virtual DbSet<ReceptionNaghd> ReceptionNaghd { get; set; }

	public virtual DbSet<ReceptionHavale> ReceptionHavale { get; set; }

	public virtual DbSet<ReceptionCheck> ReceptionCheck { get; set; }

	public virtual DbSet<OrderDetail> OrderDet { get; set; }

	public virtual DbSet<AbadDet> AbadDets { get; set; }

	public virtual DbSet<OrderAbad> OrderAbads { get; set; }

	public virtual DbSet<Order> Order { get; set; }

	public virtual DbSet<SanadStatusLog> OrderStatusLog { get; set; }

	public virtual DbSet<Gps> Gps { get; set; }

	public virtual DbSet<FullTextWords> FullTextWords { get; set; }

	public virtual DbSet<Kardex> Kardex { get; set; }

	public virtual DbSet<Depot> Depot { get; set; }

	public virtual DbSet<ProductGroup> Products_group { get; set; }

	public virtual DbSet<Barcode> Barcodes { get; set; }

	public virtual DbSet<ProductVahed> Products_vahed { get; set; }

	public virtual DbSet<Cashier> Cashiers { get; set; }

	public virtual DbSet<CashiersLog> CashiersLogs { get; set; }

	public virtual DbSet<UserAcc> UserAcc { get; set; }

	public virtual DbSet<HesabTypes> HesabType { get; set; }

	public virtual DbSet<TolidFormula> TolidFormulas { get; set; }

	public virtual DbSet<TableRelationShip> TableRelationShip { get; set; }

	public virtual DbSet<VisitorArea> VisitorAreas { get; set; }

	public void OnModelCreatingEvaluation(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<Evaluation>().Property<DateTime?>((Expression<Func<Evaluation, DateTime?>>)((Evaluation p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Evaluation>((Expression<Func<Cashier, IEnumerable<Evaluation>>>)((Cashier x) => x.Evaluation)).WithOne((Expression<Func<Evaluation, Cashier>>)((Evaluation x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Evaluation, object>>)((Evaluation x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PrdAnbarGardani>().HasMany<Evaluation>((Expression<Func<PrdAnbarGardani, IEnumerable<Evaluation>>>)((PrdAnbarGardani x) => x.EvaluationKasr)).WithOne((Expression<Func<Evaluation, PrdAnbarGardani>>)((Evaluation x) => x.AnbarGardaniKasrItem))
				.HasForeignKey((Expression<Func<Evaluation, object>>)((Evaluation x) => x.AnbarGardaniKasrGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PrdAnbarGardani>().HasMany<Evaluation>((Expression<Func<PrdAnbarGardani, IEnumerable<Evaluation>>>)((PrdAnbarGardani x) => x.EvaluationEzafe)).WithOne((Expression<Func<Evaluation, PrdAnbarGardani>>)((Evaluation x) => x.AnbarGardaniEzafeItem))
				.HasForeignKey((Expression<Func<Evaluation, object>>)((Evaluation x) => x.AnbarGardaniEzafeGuid))
				.IsRequired(false)).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingEvaluation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\EvaluationModelContext.cs", 25);
		}
	}

	public void OnModelCreatingProductPriceRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductPriceRelation>().HasKey((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation p) => new { p.Guid })).IsCluster(isCluster: false);
			((IndexBuilder)(object)modelBuilder.Entity<ProductPriceRelation>().HasIndex((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation p) => new { p.ProductGuid, p.UnitGuid }))).IsCluster();
			modelBuilder.Entity<ProductPriceRelation>().HasIndex((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation p) => p.ServerStatus));
			modelBuilder.Entity<ProductPriceRelation>().HasIndex((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation p) => p.ProductGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ProductPriceRelation>((Expression<Func<Product, IEnumerable<ProductPriceRelation>>>)((Product x) => x.ProductPriceRelation)).WithOne((Expression<Func<ProductPriceRelation, Product>>)((ProductPriceRelation x) => x.ProductItem))
				.HasForeignKey((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductPriceLevel>().HasMany<ProductPriceRelation>((Expression<Func<ProductPriceLevel, IEnumerable<ProductPriceRelation>>>)((ProductPriceLevel x) => x.ProductPriceRelation)).WithOne((Expression<Func<ProductPriceRelation, ProductPriceLevel>>)((ProductPriceRelation x) => x.PriceLevelItem))
				.HasForeignKey((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation x) => x.PriceLevelGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<ProductPriceRelation>((Expression<Func<ProductVahed, IEnumerable<ProductPriceRelation>>>)((ProductVahed x) => x.ProductPriceRelation)).WithOne((Expression<Func<ProductPriceRelation, ProductVahed>>)((ProductPriceRelation x) => x.UnitItem))
				.HasForeignKey((Expression<Func<ProductPriceRelation, object>>)((ProductPriceRelation x) => x.UnitGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductPriceLevel>().HasMany<Order>((Expression<Func<ProductPriceLevel, IEnumerable<Order>>>)((ProductPriceLevel x) => x.Order)).WithOne((Expression<Func<Order, ProductPriceLevel>>)((Order x) => x.ProductPriceLevel))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.PriceTypeGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductPriceLevel>().HasMany<OrderDetail>((Expression<Func<ProductPriceLevel, IEnumerable<OrderDetail>>>)((ProductPriceLevel x) => x.OrderDetail)).WithOne((Expression<Func<OrderDetail, ProductPriceLevel>>)((OrderDetail x) => x.ProductPriceLevel))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.PriceTypeGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductPriceLevel>().HasMany<CustomerRole>((Expression<Func<ProductPriceLevel, IEnumerable<CustomerRole>>>)((ProductPriceLevel x) => x.CustomerRole)).WithOne((Expression<Func<CustomerRole, ProductPriceLevel>>)((CustomerRole x) => x.ProductPriceLevel))
				.HasForeignKey((Expression<Func<CustomerRole, object>>)((CustomerRole x) => x.PriceTypeGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductPriceRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductPriceRelationModelContext.cs", 33);
		}
	}

	public void OnModelCreatingProductPriceLevel(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductPriceLevel>().HasIndex((Expression<Func<ProductPriceLevel, object>>)((ProductPriceLevel p) => p.ServerStatus));
			modelBuilder.Entity<ProductPriceLevel>().HasIndex((Expression<Func<ProductPriceLevel, object>>)((ProductPriceLevel p) => new { p.Name, p.Type })).IsUnique(true);
			modelBuilder.Entity<ProductPriceLevel>().HasIndex((Expression<Func<ProductPriceLevel, object>>)((ProductPriceLevel p) => new { p.Ordinal, p.Status })).IsUnique(true);
			RelationalPropertyBuilderExtensions.HasDefaultValue<decimal>(modelBuilder.Entity<ProductPriceLevel>().Property<decimal>((Expression<Func<ProductPriceLevel, decimal>>)((ProductPriceLevel q) => q.DiscountCoefficient)), (object)1);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductPriceLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductPriceLevelModelContext.cs", 21);
		}
	}

	public void OnModelCreatingTempBarcodePrint(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<TempBarcodePrint>((Expression<Func<Product, IEnumerable<TempBarcodePrint>>>)((Product x) => x.TempBarcodePrint)).WithOne((Expression<Func<TempBarcodePrint, Product>>)((TempBarcodePrint x) => x.ProductItem))
				.HasForeignKey((Expression<Func<TempBarcodePrint, object>>)((TempBarcodePrint x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<TempBarcodePrint>((Expression<Func<Tafsil, IEnumerable<TempBarcodePrint>>>)((Tafsil x) => x.TempBarcodePrint)).WithOne((Expression<Func<TempBarcodePrint, Tafsil>>)((TempBarcodePrint x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<TempBarcodePrint, object>>)((TempBarcodePrint x) => x.TafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTempBarcodePrint", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\TempBarcodePrintModelContext.cs", 23);
		}
	}

	public void OnModelCreatingOpenInvoice(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<OpenInvoiceOrderRelation>().HasKey((Expression<Func<OpenInvoiceOrderRelation, object>>)((OpenInvoiceOrderRelation k) => new { k.InvoiceGuid, k.OrderGuid }));
			modelBuilder.Entity<OpenInvoicePlacementRelation>().HasKey((Expression<Func<OpenInvoicePlacementRelation, object>>)((OpenInvoicePlacementRelation k) => new { k.InvoiceGuid, k.PlacementGuid }));
			modelBuilder.Entity<PlacementOrderRelation>().HasKey((Expression<Func<PlacementOrderRelation, object>>)((PlacementOrderRelation k) => new { k.PlacementGuid, k.OrderGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<OpenInvoice>((Expression<Func<Tafsil, IEnumerable<OpenInvoice>>>)((Tafsil x) => x.OpenInvoices)).WithOne((Expression<Func<OpenInvoice, Tafsil>>)((OpenInvoice x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<OpenInvoice, object>>)((OpenInvoice x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<OpenInvoice>((Expression<Func<Cashier, IEnumerable<OpenInvoice>>>)((Cashier x) => x.OpenInvoices)).WithOne((Expression<Func<OpenInvoice, Cashier>>)((OpenInvoice x) => x.CashierItem))
				.HasForeignKey((Expression<Func<OpenInvoice, object>>)((OpenInvoice x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<OpenInvoice>((Expression<Func<Reception, IEnumerable<OpenInvoice>>>)((Reception x) => x.OpenInvoices)).WithOne((Expression<Func<OpenInvoice, Reception>>)((OpenInvoice x) => x.ReceptionItem))
				.HasForeignKey((Expression<Func<OpenInvoice, object>>)((OpenInvoice x) => x.ReceptionGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<OpenInvoice>((Expression<Func<Pardakht, IEnumerable<OpenInvoice>>>)((Pardakht x) => x.OpenInvoices)).WithOne((Expression<Func<OpenInvoice, Pardakht>>)((OpenInvoice x) => x.PardakhtItem))
				.HasForeignKey((Expression<Func<OpenInvoice, object>>)((OpenInvoice x) => x.PardakhtGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<OpenInvoice>().HasMany<OpenInvoiceOrderRelation>((Expression<Func<OpenInvoice, IEnumerable<OpenInvoiceOrderRelation>>>)((OpenInvoice x) => x.OrderRelation)).WithOne((Expression<Func<OpenInvoiceOrderRelation, OpenInvoice>>)((OpenInvoiceOrderRelation x) => x.MasterItem))
				.HasForeignKey((Expression<Func<OpenInvoiceOrderRelation, object>>)((OpenInvoiceOrderRelation x) => x.InvoiceGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<OpenInvoiceOrderRelation>((Expression<Func<Order, IEnumerable<OpenInvoiceOrderRelation>>>)((Order x) => x.OrderRelation)).WithOne((Expression<Func<OpenInvoiceOrderRelation, Order>>)((OpenInvoiceOrderRelation x) => x.OrderItem))
				.HasForeignKey((Expression<Func<OpenInvoiceOrderRelation, object>>)((OpenInvoiceOrderRelation x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<OpenInvoice>().HasMany<OpenInvoicePlacementRelation>((Expression<Func<OpenInvoice, IEnumerable<OpenInvoicePlacementRelation>>>)((OpenInvoice x) => x.PlacementRelation)).WithOne((Expression<Func<OpenInvoicePlacementRelation, OpenInvoice>>)((OpenInvoicePlacementRelation x) => x.MasterItem))
				.HasForeignKey((Expression<Func<OpenInvoicePlacementRelation, object>>)((OpenInvoicePlacementRelation x) => x.InvoiceGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<OpenInvoicePlacementRelation>((Expression<Func<Placement, IEnumerable<OpenInvoicePlacementRelation>>>)((Placement x) => x.PlacementRelation)).WithOne((Expression<Func<OpenInvoicePlacementRelation, Placement>>)((OpenInvoicePlacementRelation x) => x.PlacementItem))
				.HasForeignKey((Expression<Func<OpenInvoicePlacementRelation, object>>)((OpenInvoicePlacementRelation x) => x.PlacementGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<PlacementOrderRelation>((Expression<Func<Order, IEnumerable<PlacementOrderRelation>>>)((Order x) => x.PlacementOrderRelation)).WithOne((Expression<Func<PlacementOrderRelation, Order>>)((PlacementOrderRelation x) => x.OrderItem))
				.HasForeignKey((Expression<Func<PlacementOrderRelation, object>>)((PlacementOrderRelation x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<PlacementOrderRelation>((Expression<Func<Placement, IEnumerable<PlacementOrderRelation>>>)((Placement x) => x.PlacementOrderRelation)).WithOne((Expression<Func<PlacementOrderRelation, Placement>>)((PlacementOrderRelation x) => x.PlacementItem))
				.HasForeignKey((Expression<Func<PlacementOrderRelation, object>>)((PlacementOrderRelation x) => x.PlacementGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingOpenInvoice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\OpenInvoiceContext.cs", 41);
		}
	}

	public void OnModelCreatingReservation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Reservation>().HasIndex((Expression<Func<Reservation, object>>)((Reservation q) => q.Number)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Reservation>((Expression<Func<Tafsil, IEnumerable<Reservation>>>)((Tafsil x) => x.Reservation)).WithOne((Expression<Func<Reservation, Tafsil>>)((Reservation x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<Reservation, object>>)((Reservation x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<Reservation>((Expression<Func<Order, IEnumerable<Reservation>>>)((Order x) => x.Reservation)).WithOne((Expression<Func<Reservation, Order>>)((Reservation x) => x.OrderItem))
				.HasForeignKey((Expression<Func<Reservation, object>>)((Reservation x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Reservation>((Expression<Func<Cashier, IEnumerable<Reservation>>>)((Cashier x) => x.Reservation)).WithOne((Expression<Func<Reservation, Cashier>>)((Reservation x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Reservation, object>>)((Reservation x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<Reservation>((Expression<Func<Reception, IEnumerable<Reservation>>>)((Reception x) => x.Reservation)).WithOne((Expression<Func<Reservation, Reception>>)((Reservation x) => x.ReceptionItem))
				.HasForeignKey((Expression<Func<Reservation, object>>)((Reservation x) => x.ReceptionGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reservation>().HasMany<ReservationDetail>((Expression<Func<Reservation, IEnumerable<ReservationDetail>>>)((Reservation x) => x.Details)).WithOne((Expression<Func<ReservationDetail, Reservation>>)((ReservationDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<ReservationDetail, object>>)((ReservationDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<ReservationDetail>((Expression<Func<Placement, IEnumerable<ReservationDetail>>>)((Placement x) => x.ReservationDetail)).WithOne((Expression<Func<ReservationDetail, Placement>>)((ReservationDetail x) => x.PlacementItem))
				.HasForeignKey((Expression<Func<ReservationDetail, object>>)((ReservationDetail x) => x.PlacementGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<ReservationDetail>((Expression<Func<Order, IEnumerable<ReservationDetail>>>)((Order x) => x.ReservationDetail)).WithOne((Expression<Func<ReservationDetail, Order>>)((ReservationDetail x) => x.OrderItem))
				.HasForeignKey((Expression<Func<ReservationDetail, object>>)((ReservationDetail x) => x.OrderGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReservation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\ReservationModelContext.cs", 31);
		}
	}

	public void OnModelCreatingTax(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<TaxClass>().HasIndex((Expression<Func<TaxClass, object>>)((TaxClass p) => p.Name)).IsUnique(true);
			modelBuilder.Entity<TaxClass>().HasIndex((Expression<Func<TaxClass, object>>)((TaxClass p) => p.Slug)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TaxClass>().HasMany<Product>((Expression<Func<TaxClass, IEnumerable<Product>>>)((TaxClass x) => x.Products)).WithOne((Expression<Func<Product, TaxClass>>)((Product x) => x.TaxItem))
				.HasForeignKey((Expression<Func<Product, object>>)((Product x) => x.TaxGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TaxClass>().HasMany<TaxClassDetail>((Expression<Func<TaxClass, IEnumerable<TaxClassDetail>>>)((TaxClass x) => x.Details)).WithOne((Expression<Func<TaxClassDetail, TaxClass>>)((TaxClassDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<TaxClassDetail, object>>)((TaxClassDetail x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\TaxModelContext.cs", 24);
		}
	}

	public void OnModelCreatingChequeMOperation(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ChequeMOperation>().Property<DateTime?>((Expression<Func<ChequeMOperation, DateTime?>>)((ChequeMOperation p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ReceptionCheck>().HasMany<ChequeMOperation>((Expression<Func<ReceptionCheck, IEnumerable<ChequeMOperation>>>)((ReceptionCheck x) => x.ChequeMOperation)).WithOne((Expression<Func<ChequeMOperation, ReceptionCheck>>)((ChequeMOperation x) => x.Cheque))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.GuidCheque))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ChequeMOperation>((Expression<Func<Cashier, IEnumerable<ChequeMOperation>>>)((Cashier x) => x.ChequeMOperation)).WithOne((Expression<Func<ChequeMOperation, Cashier>>)((ChequeMOperation x) => x.Cashier))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ChequeMOperation>((Expression<Func<Moein, IEnumerable<ChequeMOperation>>>)((Moein x) => x.ChequeMOperationDebit)).WithOne((Expression<Func<ChequeMOperation, Moein>>)((ChequeMOperation x) => x.DebitMoein))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ChequeMOperation>((Expression<Func<Moein, IEnumerable<ChequeMOperation>>>)((Moein x) => x.ChequeMOperationCredit)).WithOne((Expression<Func<ChequeMOperation, Moein>>)((ChequeMOperation x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ChequeMOperation>((Expression<Func<Tafsil, IEnumerable<ChequeMOperation>>>)((Tafsil x) => x.ChequeMOperationDebit)).WithOne((Expression<Func<ChequeMOperation, Tafsil>>)((ChequeMOperation x) => x.DebitTafsil))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ChequeMOperation>((Expression<Func<Tafsil, IEnumerable<ChequeMOperation>>>)((Tafsil x) => x.ChequeMOperationCredit)).WithOne((Expression<Func<ChequeMOperation, Tafsil>>)((ChequeMOperation x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<ChequeMOperation, object>>)((ChequeMOperation x) => x.CreditTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingChequeMOperation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ChequeMOperationModelContext - Copy.cs", 30);
		}
	}

	public void OnModelCreatingViewChequeSh(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ViewCheckSh>().Property<DateTime?>((Expression<Func<ViewCheckSh, DateTime?>>)((ViewCheckSh p) => p.DateUtc)), "getdate()");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingViewChequeSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\ViewChequeShModelContext.cs", 19);
		}
	}

	public void OnModelCreatingAghsatM(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AghsatM_Group>((Expression<Func<Tafsil, IEnumerable<AghsatM_Group>>>)((Tafsil x) => x.AghsatGroup)).WithOne((Expression<Func<AghsatM_Group, Tafsil>>)((AghsatM_Group x) => x.Tafsils))
				.HasForeignKey((Expression<Func<AghsatM_Group, object>>)((AghsatM_Group x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AghsatM_Group>((Expression<Func<Tafsil, IEnumerable<AghsatM_Group>>>)((Tafsil x) => x.CreditAghsatGroup)).WithOne((Expression<Func<AghsatM_Group, Tafsil>>)((AghsatM_Group x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<AghsatM_Group, object>>)((AghsatM_Group x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AghsatM>((Expression<Func<Tafsil, IEnumerable<AghsatM>>>)((Tafsil x) => x.CreditAghsat)).WithOne((Expression<Func<AghsatM, Tafsil>>)((AghsatM x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<AghsatM, object>>)((AghsatM x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AghsatM_Group>((Expression<Func<Moein, IEnumerable<AghsatM_Group>>>)((Moein x) => x.AghsatGroup)).WithOne((Expression<Func<AghsatM_Group, Moein>>)((AghsatM_Group x) => x.Moeins))
				.HasForeignKey((Expression<Func<AghsatM_Group, object>>)((AghsatM_Group x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AghsatM_Group>((Expression<Func<Moein, IEnumerable<AghsatM_Group>>>)((Moein x) => x.CreditAghsatGroup)).WithOne((Expression<Func<AghsatM_Group, Moein>>)((AghsatM_Group x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<AghsatM_Group, object>>)((AghsatM_Group x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AghsatM>((Expression<Func<Moein, IEnumerable<AghsatM>>>)((Moein x) => x.CreditAghsat)).WithOne((Expression<Func<AghsatM, Moein>>)((AghsatM x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<AghsatM, object>>)((AghsatM x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<AghsatM>().Property<DateTime?>((Expression<Func<AghsatM, DateTime?>>)((AghsatM p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<AghsatM_Group>((Expression<Func<Cashier, IEnumerable<AghsatM_Group>>>)((Cashier x) => x.AghsatGroup)).WithOne((Expression<Func<AghsatM_Group, Cashier>>)((AghsatM_Group x) => x.CashierItem))
				.HasForeignKey((Expression<Func<AghsatM_Group, object>>)((AghsatM_Group x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<AghsatM_Group>().HasMany<AghsatM>((Expression<Func<AghsatM_Group, IEnumerable<AghsatM>>>)((AghsatM_Group x) => x.AghsatAshkhas)).WithOne((Expression<Func<AghsatM, AghsatM_Group>>)((AghsatM x) => x.Groups))
				.HasForeignKey((Expression<Func<AghsatM, object>>)((AghsatM x) => x.GuidGroup))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<AghsatM>((Expression<Func<Reception, IEnumerable<AghsatM>>>)((Reception x) => x.AghsatAshkhas)).WithOne((Expression<Func<AghsatM, Reception>>)((AghsatM x) => x.Receptions))
				.HasForeignKey((Expression<Func<AghsatM, object>>)((AghsatM x) => x.ReceptionGuid))
				.IsRequired(false)).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingAghsatM", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Aghsat\\AghsatMModelContext.cs", 34);
		}
	}

	public void OnModelCreatingAghsatSh(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AghsatSh>((Expression<Func<Tafsil, IEnumerable<AghsatSh>>>)((Tafsil x) => x.AghsatShakhsi)).WithOne((Expression<Func<AghsatSh, Tafsil>>)((AghsatSh x) => x.Tafsils))
				.HasForeignKey((Expression<Func<AghsatSh, object>>)((AghsatSh x) => x.TafsilGuid))).OnDeleteNoAction();
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<AghsatSh>().Property<DateTime?>((Expression<Func<AghsatSh, DateTime?>>)((AghsatSh p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<AghsatSh>((Expression<Func<Pardakht, IEnumerable<AghsatSh>>>)((Pardakht x) => x.AghsatShakhsi)).WithOne((Expression<Func<AghsatSh, Pardakht>>)((AghsatSh x) => x.Pardakhts))
				.HasForeignKey((Expression<Func<AghsatSh, object>>)((AghsatSh x) => x.PardakhtGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AghsatSh>((Expression<Func<Moein, IEnumerable<AghsatSh>>>)((Moein x) => x.AghsatShakhsi)).WithOne((Expression<Func<AghsatSh, Moein>>)((AghsatSh x) => x.Moeins))
				.HasForeignKey((Expression<Func<AghsatSh, object>>)((AghsatSh x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<AghsatSh>((Expression<Func<Cashier, IEnumerable<AghsatSh>>>)((Cashier x) => x.AghsatShs)).WithOne((Expression<Func<AghsatSh, Cashier>>)((AghsatSh x) => x.CashierItem))
				.HasForeignKey((Expression<Func<AghsatSh, object>>)((AghsatSh x) => x.UserGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingAghsatSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Aghsat\\AghsatShModelContext.cs", 33);
		}
	}

	public void OnModelCreatingHesabGroup(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<HesabTypes>().HasMany<HesabGroup>((Expression<Func<HesabTypes, IEnumerable<HesabGroup>>>)((HesabTypes x) => x.HesabGroups)).WithOne((Expression<Func<HesabGroup, HesabTypes>>)((HesabGroup x) => x.HesabTypes))
				.HasForeignKey((Expression<Func<HesabGroup, object>>)((HesabGroup x) => x.TypeGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingHesabGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\HesabGroupModelContext.cs", 20);
		}
	}

	public void OnModelCreatingAccount(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<AccountClass>().HasKey((Expression<Func<AccountClass, object>>)((AccountClass k) => new { k.MoeinGuid, k.TafsilGuid }));
			modelBuilder.Entity<AccountClass>().HasIndex((Expression<Func<AccountClass, object>>)((AccountClass p) => p.Guid)).IsUnique(true);
			modelBuilder.Entity<AccountClass>().HasIndex((Expression<Func<AccountClass, object>>)((AccountClass p) => p.TafsilGuid));
			modelBuilder.Entity<AccountBallance>().HasKey((Expression<Func<AccountBallance, object>>)((AccountBallance k) => new { k.MoeinGuid, k.TafsilGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AccountClass>((Expression<Func<Moein, IEnumerable<AccountClass>>>)((Moein x) => x.Accounts)).WithOne((Expression<Func<AccountClass, Moein>>)((AccountClass x) => x.Moein))
				.HasForeignKey((Expression<Func<AccountClass, object>>)((AccountClass x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AccountClass>((Expression<Func<Tafsil, IEnumerable<AccountClass>>>)((Tafsil x) => x.Accounts)).WithOne((Expression<Func<AccountClass, Tafsil>>)((AccountClass x) => x.Tafsil))
				.HasForeignKey((Expression<Func<AccountClass, object>>)((AccountClass x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<AccountLog>((Expression<Func<Moein, IEnumerable<AccountLog>>>)((Moein x) => x.AccountLogs)).WithOne((Expression<Func<AccountLog, Moein>>)((AccountLog x) => x.Moein))
				.HasForeignKey((Expression<Func<AccountLog, object>>)((AccountLog x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<AccountLog>((Expression<Func<Tafsil, IEnumerable<AccountLog>>>)((Tafsil x) => x.AccountLogs)).WithOne((Expression<Func<AccountLog, Tafsil>>)((AccountLog x) => x.Tafsil))
				.HasForeignKey((Expression<Func<AccountLog, object>>)((AccountLog x) => x.TafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingAccount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\AccountModelContext.cs", 35);
		}
	}

	public void OnModelCreatingKol(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Kol>().HasIndex((Expression<Func<Kol, object>>)((Kol p) => p.Code)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<HesabGroup>().HasMany<Kol>((Expression<Func<HesabGroup, IEnumerable<Kol>>>)((HesabGroup x) => x.Kols)).WithOne((Expression<Func<Kol, HesabGroup>>)((Kol x) => x.HesabGroup))
				.HasForeignKey((Expression<Func<Kol, object>>)((Kol x) => x.GroupGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingKol", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\KolModelContext.cs", 28);
		}
	}

	public void OnModelCreatingMoeinTafsilGroupRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<MoeinTafsilGroupRelations>().HasKey((Expression<Func<MoeinTafsilGroupRelations, object>>)((MoeinTafsilGroupRelations k) => new { k.MoeinGuid, k.TafsilGroupGuid, k.Level }));
			modelBuilder.Entity<MoeinTafsilGroupRelations>((Action<EntityTypeBuilder<MoeinTafsilGroupRelations>>)delegate(EntityTypeBuilder<MoeinTafsilGroupRelations> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<EnMoeinTafsilRelationLevel>(q.Property<EnMoeinTafsilRelationLevel>((Expression<Func<MoeinTafsilGroupRelations, EnMoeinTafsilRelationLevel>>)((MoeinTafsilGroupRelations r) => r.Level)), "((1))");
			});
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TafsilGroup>().HasMany<MoeinTafsilGroupRelations>((Expression<Func<TafsilGroup, IEnumerable<MoeinTafsilGroupRelations>>>)((TafsilGroup x) => x.MoeinTafsilLevelRelations)).WithOne((Expression<Func<MoeinTafsilGroupRelations, TafsilGroup>>)((MoeinTafsilGroupRelations x) => x.TafsilLevelItem))
				.HasForeignKey((Expression<Func<MoeinTafsilGroupRelations, object>>)((MoeinTafsilGroupRelations x) => x.TafsilGroupGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<MoeinTafsilGroupRelations>((Expression<Func<Moein, IEnumerable<MoeinTafsilGroupRelations>>>)((Moein x) => x.MoeinTafsilLevelRelation)).WithOne((Expression<Func<MoeinTafsilGroupRelations, Moein>>)((MoeinTafsilGroupRelations x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<MoeinTafsilGroupRelations, object>>)((MoeinTafsilGroupRelations x) => x.MoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingMoeinTafsilGroupRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\MoeinTafsilGroupRelationModelContext.cs", 30);
		}
	}

	public void OnModelCreatingSuggestedBank(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<SuggestedBank>().HasIndex((Expression<Func<SuggestedBank, object>>)((SuggestedBank q) => q.BankName)).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSuggestedBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\SuggestedBankModelContext.cs", 23);
		}
	}

	public void OnModelCreatingGardeshTafsil(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<GardeshTafsil>().HasIndex((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil p) => p.TafsilGuid));
			modelBuilder.Entity<GardeshTafsil>().HasIndex((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil p) => p.MoeinGuid));
			modelBuilder.Entity<GardeshTafsil>().HasIndex((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil p) => p.KolGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<GardeshTafsil>((Expression<Func<ProductVahed, IEnumerable<GardeshTafsil>>>)((ProductVahed x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, ProductVahed>>)((GardeshTafsil x) => x.ProductVahed))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.VGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<GardeshTafsil>((Expression<Func<DecimalEntity, IEnumerable<GardeshTafsil>>>)((DecimalEntity x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, DecimalEntity>>)((GardeshTafsil x) => x.DecimalEntity))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.DecimalGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<GardeshTafsil>((Expression<Func<Cashier, IEnumerable<GardeshTafsil>>>)((Cashier x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Cashier>>)((GardeshTafsil x) => x.Cashier))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.UGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<GardeshTafsil>((Expression<Func<Tafsil, IEnumerable<GardeshTafsil>>>)((Tafsil x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Tafsil>>)((GardeshTafsil x) => x.Tafsil))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<GardeshTafsil>((Expression<Func<Depot, IEnumerable<GardeshTafsil>>>)((Depot x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Depot>>)((GardeshTafsil x) => x.Depot))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<GardeshTafsil>((Expression<Func<Product, IEnumerable<GardeshTafsil>>>)((Product x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Product>>)((GardeshTafsil x) => x.Product))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Kol>().HasMany<GardeshTafsil>((Expression<Func<Kol, IEnumerable<GardeshTafsil>>>)((Kol x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Kol>>)((GardeshTafsil x) => x.Kol))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.KolGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<GardeshTafsil>((Expression<Func<Moein, IEnumerable<GardeshTafsil>>>)((Moein x) => x.GardeshTafsils)).WithOne((Expression<Func<GardeshTafsil, Moein>>)((GardeshTafsil x) => x.Moein))
				.HasForeignKey((Expression<Func<GardeshTafsil, object>>)((GardeshTafsil x) => x.MoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingGardeshTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\GardeshTafsilModelContext.cs", 38);
		}
	}

	public void OnModelCreatingMoein(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Moein>().HasIndex((Expression<Func<Moein, object>>)((Moein p) => p.Code)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Kol>().HasMany<Moein>((Expression<Func<Kol, IEnumerable<Moein>>>)((Kol x) => x.Moeins)).WithOne((Expression<Func<Moein, Kol>>)((Moein x) => x.Kol))
				.HasForeignKey((Expression<Func<Moein, object>>)((Moein x) => x.KolGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingMoein", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\MoeinModelContext.cs", 28);
		}
	}

	public void OnModelCreatingTafsilNumbers(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<TafsilNumber>().HasKey((Expression<Func<TafsilNumber, object>>)((TafsilNumber k) => new { k.TafsilGuid, k.OriginalNumber }));
			RelationalIndexBuilderExtensions.HasName<TafsilNumber>(modelBuilder.Entity<TafsilNumber>().HasIndex((Expression<Func<TafsilNumber, object>>)((TafsilNumber s) => s.CalcedNumber)), "IX_Acc_TafsilNumber_CalcedNumber");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<TafsilNumber>((Expression<Func<Tafsil, IEnumerable<TafsilNumber>>>)((Tafsil x) => x.TafsilNumber)).WithOne((Expression<Func<TafsilNumber, Tafsil>>)((TafsilNumber x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<TafsilNumber, object>>)((TafsilNumber x) => x.TafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTafsilNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\TafsilNumbersModelContext.cs", 27);
		}
	}

	public void OnModelCreatingTafsil(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Tafsil>().HasIndex((Expression<Func<Tafsil, object>>)((Tafsil p) => p.Code)).IsUnique(true);
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<Customer>().HasOne<Tafsil>((Expression<Func<Customer, Tafsil>>)((Customer x) => x.Tafsil)).WithOne((Expression<Func<Tafsil, Customer>>)((Tafsil x) => x.Customer))
				.HasForeignKey<Customer>((Expression<Func<Customer, object>>)((Customer x) => x.Guid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TafsilGroup>().HasMany<Tafsil>((Expression<Func<TafsilGroup, IEnumerable<Tafsil>>>)((TafsilGroup x) => x.TafsilRealtion)).WithOne((Expression<Func<Tafsil, TafsilGroup>>)((Tafsil x) => x.TafsilGroupItems))
				.HasForeignKey((Expression<Func<Tafsil, object>>)((Tafsil x) => x.TafsilGroupGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\TafsilModelContext.cs", 30);
		}
	}

	public void OnModelCreatingCustomerCard(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Customer>().HasMany<CusCard>((Expression<Func<Customer, IEnumerable<CusCard>>>)((Customer x) => x.CusCard)).WithOne((Expression<Func<CusCard, Customer>>)((CusCard x) => x.Customers))
				.HasForeignKey((Expression<Func<CusCard, object>>)((CusCard x) => x.CusGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCustomerCard", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Customer\\CustomerCardModelContext.cs", 20);
		}
	}

	public void OnModelCreatingCustomer(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Customer>().HasIndex((Expression<Func<Customer, object>>)((Customer p) => p.ServerStatus));
			RelationalIndexBuilderExtensions.HasFilter<Customer>(modelBuilder.Entity<Customer>().HasIndex((Expression<Func<Customer, object>>)((Customer p) => p.User)).IsUnique(true), "[User] IS NOT NULL");
			RelationalIndexBuilderExtensions.HasFilter<Customer>(modelBuilder.Entity<Customer>().HasIndex((Expression<Func<Customer, object>>)((Customer p) => p.Email)).IsUnique(true), "Email IS NOT NULL");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CustomerTitle>().HasMany<Customer>((Expression<Func<CustomerTitle, IEnumerable<Customer>>>)((CustomerTitle x) => x.Customers)).WithOne((Expression<Func<Customer, CustomerTitle>>)((Customer x) => x.CusTitle))
				.HasForeignKey((Expression<Func<Customer, object>>)((Customer x) => x.TitleGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CustomerGroup>().HasMany<Customer>((Expression<Func<CustomerGroup, IEnumerable<Customer>>>)((CustomerGroup x) => x.Customers)).WithOne((Expression<Func<Customer, CustomerGroup>>)((Customer x) => x.CusGroup))
				.HasForeignKey((Expression<Func<Customer, object>>)((Customer x) => x.GroupGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CustomerRole>().HasMany<Customer>((Expression<Func<CustomerRole, IEnumerable<Customer>>>)((CustomerRole x) => x.Customers)).WithOne((Expression<Func<Customer, CustomerRole>>)((Customer x) => x.Role))
				.HasForeignKey((Expression<Func<Customer, object>>)((Customer x) => x.RoleGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<VisitorArea>().HasMany<Customer>((Expression<Func<VisitorArea, IEnumerable<Customer>>>)((VisitorArea x) => x.Customers)).WithOne((Expression<Func<Customer, VisitorArea>>)((Customer x) => x.VisitorArea))
				.HasForeignKey((Expression<Func<Customer, object>>)((Customer x) => x.AreaGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Customer\\CustomerModelContext.cs", 38);
		}
	}

	public void OnModelCreatingCustomerTitle(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<CustomerTitle>().HasIndex((Expression<Func<CustomerTitle, object>>)((CustomerTitle p) => p.Name)).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCustomerTitle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Customer\\CustomerTitleModelContext.cs", 23);
		}
	}

	public void OnModelCreatingCustomerGroup(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<CustomerGroup>().HasIndex((Expression<Func<CustomerGroup, object>>)((CustomerGroup p) => p.Name)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TafsilGroup>().HasMany<CustomerGroup>((Expression<Func<TafsilGroup, IEnumerable<CustomerGroup>>>)((TafsilGroup x) => x.CustomerGroupRealtion)).WithOne((Expression<Func<CustomerGroup, TafsilGroup>>)((CustomerGroup x) => x.TafsilGroupItems))
				.HasForeignKey((Expression<Func<CustomerGroup, object>>)((CustomerGroup x) => x.TafsilGroupGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<CustomerGroup>((Expression<Func<Moein, IEnumerable<CustomerGroup>>>)((Moein x) => x.CustomerGroups)).WithOne((Expression<Func<CustomerGroup, Moein>>)((CustomerGroup x) => x.MoeinItems))
				.HasForeignKey((Expression<Func<CustomerGroup, object>>)((CustomerGroup x) => x.MoeinDefaultGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCustomerGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Customer\\CustomerGroupModelContext.cs", 27);
		}
	}

	public void OnModelCreatingTafsilGroup(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<TafsilGroup>().HasIndex((Expression<Func<TafsilGroup, object>>)((TafsilGroup p) => p.Name)).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTafsilGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Coding\\TafsilGroupModelContext.cs", 21);
		}
	}

	public void OnModelCreatingDecimal(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalIndexBuilderExtensions.HasName<DecimalEntity>(modelBuilder.Entity<DecimalEntity>().HasIndex((Expression<Func<DecimalEntity, object>>)((DecimalEntity p) => new { p.Name, p.Country })), "IX_Decimal_Name_Country").IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<DecimalUpdates>((Expression<Func<DecimalEntity, IEnumerable<DecimalUpdates>>>)((DecimalEntity x) => x.DecimalUpdateses)).WithOne((Expression<Func<DecimalUpdates, DecimalEntity>>)((DecimalUpdates x) => x.DecimalEntity))
				.HasForeignKey((Expression<Func<DecimalUpdates, object>>)((DecimalUpdates x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingDecimal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\DecimalEntity\\DecimalModelContext.cs", 26);
		}
	}

	public void OnModelCreatingDepotReceipt(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<DepotReceipt>().HasIndex((Expression<Func<DepotReceipt, object>>)((DepotReceipt p) => new { p.Type, p.Id2 })).IsUnique(true);
			RelationalIndexBuilderExtensions.HasFilter<DepotReceiptDetail>(modelBuilder.Entity<DepotReceiptDetail>().HasIndex((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail p) => p.ReferenceGuid)).IsUnique(true), "ReferenceGuid IS NOT NULL");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<DepotReceipt>((Expression<Func<Tafsil, IEnumerable<DepotReceipt>>>)((Tafsil x) => x.DepotReceipts)).WithOne((Expression<Func<DepotReceipt, Tafsil>>)((DepotReceipt x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<DepotReceipt, object>>)((DepotReceipt x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<DepotReceipt>((Expression<Func<Cashier, IEnumerable<DepotReceipt>>>)((Cashier x) => x.DepotReceipts)).WithOne((Expression<Func<DepotReceipt, Cashier>>)((DepotReceipt x) => x.CashierItem))
				.HasForeignKey((Expression<Func<DepotReceipt, object>>)((DepotReceipt x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<DepotReceipt>((Expression<Func<Order, IEnumerable<DepotReceipt>>>)((Order x) => x.DepotReceipts)).WithOne((Expression<Func<DepotReceipt, Order>>)((DepotReceipt x) => x.OrderItem))
				.HasForeignKey((Expression<Func<DepotReceipt, object>>)((DepotReceipt x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<DepotReceiptDetail>((Expression<Func<Product, IEnumerable<DepotReceiptDetail>>>)((Product x) => x.DepotReceiptDetail)).WithOne((Expression<Func<DepotReceiptDetail, Product>>)((DepotReceiptDetail x) => x.ProductItem))
				.HasForeignKey((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<DepotReceiptDetail>((Expression<Func<Depot, IEnumerable<DepotReceiptDetail>>>)((Depot x) => x.DepotReceiptDetail)).WithOne((Expression<Func<DepotReceiptDetail, Depot>>)((DepotReceiptDetail x) => x.DepotItem))
				.HasForeignKey((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<DepotReceiptDetail>((Expression<Func<ProductVahed, IEnumerable<DepotReceiptDetail>>>)((ProductVahed x) => x.DepotReceiptDetail)).WithOne((Expression<Func<DepotReceiptDetail, ProductVahed>>)((DepotReceiptDetail x) => x.VahedItem))
				.HasForeignKey((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail x) => x.V2Guid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DepotReceipt>().HasMany<DepotReceiptDetail>((Expression<Func<DepotReceipt, IEnumerable<DepotReceiptDetail>>>)((DepotReceipt x) => x.DepotReceiptDetail)).WithOne((Expression<Func<DepotReceiptDetail, DepotReceipt>>)((DepotReceiptDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ViewSanadDetail>().HasMany<DepotReceiptDetail>((Expression<Func<ViewSanadDetail, IEnumerable<DepotReceiptDetail>>>)((ViewSanadDetail x) => x.DepotReceiptDetail)).WithOne((Expression<Func<DepotReceiptDetail, ViewSanadDetail>>)((DepotReceiptDetail x) => x.ReferenceItem))
				.HasForeignKey((Expression<Func<DepotReceiptDetail, object>>)((DepotReceiptDetail x) => x.ReferenceGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingDepotReceipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\DepotReceipt\\DepotReceiptModelContext.cs", 33);
		}
	}

	public void OnModelCreatingDiscountCoupon(ModelBuilder modelBuilder)
	{
		try
		{
			SqlServerKeyBuilderExtensions.IsClustered(modelBuilder.Entity<CouponDiscountEntityRelation>().HasKey((Expression<Func<CouponDiscountEntityRelation, object>>)((CouponDiscountEntityRelation q) => new { q.CouponDetailGuid, q.EntityType, q.EntityGuid })), true);
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<Order>().HasOne<CouponDiscount>((Expression<Func<Order, CouponDiscount>>)((Order x) => x.CouponDiscount)).WithOne((Expression<Func<CouponDiscount, Order>>)((CouponDiscount x) => x.Order))
				.HasForeignKey<Order>((Expression<Func<Order, object>>)((Order x) => x.CashBackCouponGuid))).OnDeleteNoAction();
			modelBuilder.Entity<CouponDiscountUsages>().HasKey((Expression<Func<CouponDiscountUsages, object>>)((CouponDiscountUsages q) => new { q.CouponGuid, q.TafsilGuid })).IsCluster();
			modelBuilder.Entity<CouponDiscountUsages>().HasKey((Expression<Func<CouponDiscountUsages, object>>)((CouponDiscountUsages q) => new { q.CouponGuid, q.TafsilGuid })).IsCluster();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CouponDiscount>().HasMany<CouponDiscountDetail>((Expression<Func<CouponDiscount, IEnumerable<CouponDiscountDetail>>>)((CouponDiscount x) => x.CouponDiscountDetails)).WithOne((Expression<Func<CouponDiscountDetail, CouponDiscount>>)((CouponDiscountDetail x) => x.Master))
				.HasForeignKey((Expression<Func<CouponDiscountDetail, object>>)((CouponDiscountDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Gift>().HasMany<CouponDiscountDetail>((Expression<Func<Gift, IEnumerable<CouponDiscountDetail>>>)((Gift x) => x.CouponDiscountDetail)).WithOne((Expression<Func<CouponDiscountDetail, Gift>>)((CouponDiscountDetail x) => x.Gift))
				.HasForeignKey((Expression<Func<CouponDiscountDetail, object>>)((CouponDiscountDetail x) => x.GiftGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<CouponDiscount>((Expression<Func<Cashier, IEnumerable<CouponDiscount>>>)((Cashier x) => x.CouponDiscounts)).WithOne((Expression<Func<CouponDiscount, Cashier>>)((CouponDiscount x) => x.Cashier))
				.HasForeignKey((Expression<Func<CouponDiscount, object>>)((CouponDiscount x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CouponDiscountDetail>().HasMany<CouponDiscountEntityRelation>((Expression<Func<CouponDiscountDetail, IEnumerable<CouponDiscountEntityRelation>>>)((CouponDiscountDetail x) => x.CouponDiscountEntityRelation)).WithOne((Expression<Func<CouponDiscountEntityRelation, CouponDiscountDetail>>)((CouponDiscountEntityRelation x) => x.Coupon))
				.HasForeignKey((Expression<Func<CouponDiscountEntityRelation, object>>)((CouponDiscountEntityRelation x) => x.CouponDetailGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CouponDiscount>().HasMany<CouponDiscountUsages>((Expression<Func<CouponDiscount, IEnumerable<CouponDiscountUsages>>>)((CouponDiscount x) => x.CouponDiscountUsages)).WithOne((Expression<Func<CouponDiscountUsages, CouponDiscount>>)((CouponDiscountUsages x) => x.Coupon))
				.HasForeignKey((Expression<Func<CouponDiscountUsages, object>>)((CouponDiscountUsages x) => x.CouponGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingDiscountCoupon", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\DiscountCoupon\\DiscountCouponModelContext.cs", 39);
		}
	}

	public void OnModelCreatingReceipt(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Receipt>((Expression<Func<Tafsil, IEnumerable<Receipt>>>)((Tafsil x) => x.Receipts)).WithOne((Expression<Func<Receipt, Tafsil>>)((Receipt x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Receipt, object>>)((Receipt x) => x.CustomerGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Receipt>((Expression<Func<Tafsil, IEnumerable<Receipt>>>)((Tafsil x) => x.ReceiptsPresentor)).WithOne((Expression<Func<Receipt, Tafsil>>)((Receipt x) => x.PresenterTafsil))
				.HasForeignKey((Expression<Func<Receipt, object>>)((Receipt x) => x.PresenterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<Receipt>((Expression<Func<Order, IEnumerable<Receipt>>>)((Order x) => x.Receipts)).WithOne((Expression<Func<Receipt, Order>>)((Receipt x) => x.Order))
				.HasForeignKey((Expression<Func<Receipt, object>>)((Receipt x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Receipt>().HasMany<ReceiptDetail>((Expression<Func<Receipt, IEnumerable<ReceiptDetail>>>)((Receipt x) => x.ReceiptDetails)).WithOne((Expression<Func<ReceiptDetail, Receipt>>)((ReceiptDetail x) => x.Master))
				.HasForeignKey((Expression<Func<ReceiptDetail, object>>)((ReceiptDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<ReceiptDetail>((Expression<Func<Placement, IEnumerable<ReceiptDetail>>>)((Placement x) => x.ReciptDetails)).WithOne((Expression<Func<ReceiptDetail, Placement>>)((ReceiptDetail x) => x.placement))
				.HasForeignKey((Expression<Func<ReceiptDetail, object>>)((ReceiptDetail x) => x.PlacementGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReceipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Hotel\\ReceiptModelContext.cs", 32);
		}
	}

	public void OnModelCreatingPlacement(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Hall>().HasMany<Placement>((Expression<Func<Hall, IEnumerable<Placement>>>)((Hall x) => x.Placement)).WithOne((Expression<Func<Placement, Hall>>)((Placement x) => x.Master))
				.HasForeignKey((Expression<Func<Placement, object>>)((Placement x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductGroup>().HasMany<Placement>((Expression<Func<ProductGroup, IEnumerable<Placement>>>)((ProductGroup x) => x.Placements)).WithOne((Expression<Func<Placement, ProductGroup>>)((Placement x) => x.ProductGroup))
				.HasForeignKey((Expression<Func<Placement, object>>)((Placement x) => x.GroupGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<PlacementFacilitiesRelation>((Expression<Func<Placement, IEnumerable<PlacementFacilitiesRelation>>>)((Placement x) => x.PlacementFacilities)).WithOne((Expression<Func<PlacementFacilitiesRelation, Placement>>)((PlacementFacilitiesRelation x) => x.Room))
				.HasForeignKey((Expression<Func<PlacementFacilitiesRelation, object>>)((PlacementFacilitiesRelation x) => x.PlacementGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Facilities>().HasMany<PlacementFacilitiesRelation>((Expression<Func<Facilities, IEnumerable<PlacementFacilitiesRelation>>>)((Facilities x) => x.PlacementFacilities)).WithOne((Expression<Func<PlacementFacilitiesRelation, Facilities>>)((PlacementFacilitiesRelation x) => x.Facility))
				.HasForeignKey((Expression<Func<PlacementFacilitiesRelation, object>>)((PlacementFacilitiesRelation x) => x.FacilitiesGuid))).OnDeleteNoAction();
			modelBuilder.Entity<PlacementFacilitiesRelation>().HasKey((Expression<Func<PlacementFacilitiesRelation, object>>)((PlacementFacilitiesRelation k) => new { k.PlacementGuid, k.FacilitiesGuid }));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPlacement", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Hotel\\PlacementModelContext.cs", 34);
		}
	}

	public void OnModelCreatingCustomerReceiptRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<CustomerReceiptRelation>().HasKey((Expression<Func<CustomerReceiptRelation, object>>)((CustomerReceiptRelation k) => new { k.CustomerGuid, k.ReceiptGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Customer>().HasMany<CustomerReceiptRelation>((Expression<Func<Customer, IEnumerable<CustomerReceiptRelation>>>)((Customer x) => x.CustomerReceipt)).WithOne((Expression<Func<CustomerReceiptRelation, Customer>>)((CustomerReceiptRelation x) => x.Customer))
				.HasForeignKey((Expression<Func<CustomerReceiptRelation, object>>)((CustomerReceiptRelation x) => x.CustomerGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Receipt>().HasMany<CustomerReceiptRelation>((Expression<Func<Receipt, IEnumerable<CustomerReceiptRelation>>>)((Receipt x) => x.CustomerReceipt)).WithOne((Expression<Func<CustomerReceiptRelation, Receipt>>)((CustomerReceiptRelation x) => x.Receipt))
				.HasForeignKey((Expression<Func<CustomerReceiptRelation, object>>)((CustomerReceiptRelation x) => x.ReceiptGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCustomerReceiptRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Hotel\\CustomerReceiptRelationModelContext.cs", 28);
		}
	}

	public void OnModelCreatingServiceReceiptDetailRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ServiceReceiptDetailRelation>().HasKey((Expression<Func<ServiceReceiptDetailRelation, object>>)((ServiceReceiptDetailRelation k) => new { k.ReceiptDetailGuid, k.ServiceGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ReceiptDetail>().HasMany<ServiceReceiptDetailRelation>((Expression<Func<ReceiptDetail, IEnumerable<ServiceReceiptDetailRelation>>>)((ReceiptDetail x) => x.ServiceReceiptDetail)).WithOne((Expression<Func<ServiceReceiptDetailRelation, ReceiptDetail>>)((ServiceReceiptDetailRelation x) => x.ReceiptDetail))
				.HasForeignKey((Expression<Func<ServiceReceiptDetailRelation, object>>)((ServiceReceiptDetailRelation x) => x.ReceiptDetailGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ServiceReceiptDetailRelation>((Expression<Func<Product, IEnumerable<ServiceReceiptDetailRelation>>>)((Product x) => x.ServiceRecieptDetail)).WithOne((Expression<Func<ServiceReceiptDetailRelation, Product>>)((ServiceReceiptDetailRelation x) => x.Product))
				.HasForeignKey((Expression<Func<ServiceReceiptDetailRelation, object>>)((ServiceReceiptDetailRelation x) => x.ServiceGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingServiceReceiptDetailRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Hotel\\ServiceReceiptDetailRelationModelContext.cs", 22);
		}
	}

	public void OnModelCreatingPardakhtChequeEntezami(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtChequeEntezami>((Expression<Func<Moein, IEnumerable<PardakhtChequeEntezami>>>)((Moein x) => x.PardakhtChequeEntezamiCredit)).WithOne((Expression<Func<PardakhtChequeEntezami, Moein>>)((PardakhtChequeEntezami x) => x.CreditMoeinItem))
				.HasForeignKey((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtChequeEntezami>((Expression<Func<Moein, IEnumerable<PardakhtChequeEntezami>>>)((Moein x) => x.PardakhtChequeEntezamiDebit)).WithOne((Expression<Func<PardakhtChequeEntezami, Moein>>)((PardakhtChequeEntezami x) => x.DebitMoeinItem))
				.HasForeignKey((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtChequeEntezami>((Expression<Func<Tafsil, IEnumerable<PardakhtChequeEntezami>>>)((Tafsil x) => x.PardakhtChequeEntezamiCredit)).WithOne((Expression<Func<PardakhtChequeEntezami, Tafsil>>)((PardakhtChequeEntezami x) => x.CreditTafsilItem))
				.HasForeignKey((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtChequeEntezami>((Expression<Func<Tafsil, IEnumerable<PardakhtChequeEntezami>>>)((Tafsil x) => x.PardakhtChequeEntezamiDebit)).WithOne((Expression<Func<PardakhtChequeEntezami, Tafsil>>)((PardakhtChequeEntezami x) => x.DebitTafsilItem))
				.HasForeignKey((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<PardakhtChequeEntezami>((Expression<Func<Cashier, IEnumerable<PardakhtChequeEntezami>>>)((Cashier x) => x.PardakhtChequeEntezami)).WithOne((Expression<Func<PardakhtChequeEntezami, Cashier>>)((PardakhtChequeEntezami x) => x.CashierItem))
				.HasForeignKey((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<PardakhtCheckSh>().HasOne<PardakhtChequeEntezami>((Expression<Func<PardakhtCheckSh, PardakhtChequeEntezami>>)((PardakhtCheckSh x) => x.PardakhtChequeEntezami)).WithOne((Expression<Func<PardakhtChequeEntezami, PardakhtCheckSh>>)((PardakhtChequeEntezami x) => x.ChequeItem))
				.HasForeignKey<PardakhtChequeEntezami>((Expression<Func<PardakhtChequeEntezami, object>>)((PardakhtChequeEntezami q) => q.ChequeGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakhtChequeEntezami", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtChequeEntezamiModelContext.cs", 27);
		}
	}

	public void OnModelCreatingReceptionChequeEntezami(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionChequeEntezami>((Expression<Func<Moein, IEnumerable<ReceptionChequeEntezami>>>)((Moein x) => x.ReceptionChequeEntezamiCredit)).WithOne((Expression<Func<ReceptionChequeEntezami, Moein>>)((ReceptionChequeEntezami x) => x.CreditMoeinItem))
				.HasForeignKey((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionChequeEntezami>((Expression<Func<Moein, IEnumerable<ReceptionChequeEntezami>>>)((Moein x) => x.ReceptionChequeEntezamiDebit)).WithOne((Expression<Func<ReceptionChequeEntezami, Moein>>)((ReceptionChequeEntezami x) => x.DebitMoeinItem))
				.HasForeignKey((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionChequeEntezami>((Expression<Func<Tafsil, IEnumerable<ReceptionChequeEntezami>>>)((Tafsil x) => x.ReceptionChequeEntezamiCredit)).WithOne((Expression<Func<ReceptionChequeEntezami, Tafsil>>)((ReceptionChequeEntezami x) => x.CreditTafsilItem))
				.HasForeignKey((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionChequeEntezami>((Expression<Func<Tafsil, IEnumerable<ReceptionChequeEntezami>>>)((Tafsil x) => x.ReceptionChequeEntezamiDebit)).WithOne((Expression<Func<ReceptionChequeEntezami, Tafsil>>)((ReceptionChequeEntezami x) => x.DebitTafsilItem))
				.HasForeignKey((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ReceptionChequeEntezami>((Expression<Func<Cashier, IEnumerable<ReceptionChequeEntezami>>>)((Cashier x) => x.ReceptionChequeEntezami)).WithOne((Expression<Func<ReceptionChequeEntezami, Cashier>>)((ReceptionChequeEntezami x) => x.CashierItem))
				.HasForeignKey((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<ReceptionCheck>().HasOne<ReceptionChequeEntezami>((Expression<Func<ReceptionCheck, ReceptionChequeEntezami>>)((ReceptionCheck x) => x.ReceptionChequeEntezami)).WithOne((Expression<Func<ReceptionChequeEntezami, ReceptionCheck>>)((ReceptionChequeEntezami x) => x.ChequeItem))
				.HasForeignKey<ReceptionChequeEntezami>((Expression<Func<ReceptionChequeEntezami, object>>)((ReceptionChequeEntezami q) => q.ChequeGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReceptionChequeEntezami", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ReceptionChequeEntezamiModelContext.cs", 31);
		}
	}

	public void OnModelCreatingOrderDetailCouponCalculation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<OrderDetailCouponCalculation>().HasKey((Expression<Func<OrderDetailCouponCalculation, object>>)((OrderDetailCouponCalculation k) => new { k.RefrencedId, k.Guid }));
			modelBuilder.Entity<Product>().HasMany<OrderDetailCouponCalculation>((Expression<Func<Product, IEnumerable<OrderDetailCouponCalculation>>>)((Product x) => x.OrderDetailCouponCalculation)).WithOne((Expression<Func<OrderDetailCouponCalculation, Product>>)((OrderDetailCouponCalculation x) => x.ProductItem))
				.HasForeignKey((Expression<Func<OrderDetailCouponCalculation, object>>)((OrderDetailCouponCalculation x) => x.ProductGuid));
			modelBuilder.Entity<CouponDiscountDetail>().HasMany<OrderDetailCouponCalculation>((Expression<Func<CouponDiscountDetail, IEnumerable<OrderDetailCouponCalculation>>>)((CouponDiscountDetail x) => x.OrderDetailCouponCalculation)).WithOne((Expression<Func<OrderDetailCouponCalculation, CouponDiscountDetail>>)((OrderDetailCouponCalculation x) => x.Coupon))
				.HasForeignKey((Expression<Func<OrderDetailCouponCalculation, object>>)((OrderDetailCouponCalculation x) => x.DicountCouponDetailGuid));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingOrderDetailCouponCalculation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\OrderDetailCouponCalculationModelContext.cs", 23);
		}
	}

	public void OnModelCreatingTaxOrderLog(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(modelBuilder.Entity<TaxOrderLog>().Property<DateTime>((Expression<Func<TaxOrderLog, DateTime>>)((TaxOrderLog x) => x.InsertDate)), "getdate()");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(modelBuilder.Entity<TaxOrderLog>().Property<DateTime>((Expression<Func<TaxOrderLog, DateTime>>)((TaxOrderLog x) => x.Modified)), "getdate()");
			modelBuilder.Entity<TaxOrderLog>().HasKey((Expression<Func<TaxOrderLog, object>>)((TaxOrderLog p) => new { p.Guid })).IsCluster(isCluster: false);
			SqlServerIndexBuilderExtensions.IsClustered<TaxOrderLog>(RelationalIndexBuilderExtensions.HasName<TaxOrderLog>(modelBuilder.Entity<TaxOrderLog>().HasIndex((Expression<Func<TaxOrderLog, object>>)((TaxOrderLog p) => p.OrderGuid)), "IX_Acc_TaxOrderLog_OrderGuid"), true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<TaxOrderLog>((Expression<Func<Order, IEnumerable<TaxOrderLog>>>)((Order x) => x.TaxOrderLog)).WithOne((Expression<Func<TaxOrderLog, Order>>)((TaxOrderLog x) => x.OrderItem))
				.HasForeignKey((Expression<Func<TaxOrderLog, object>>)((TaxOrderLog x) => x.OrderGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTaxOrderLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\TaxOrderLogContext.cs", 26);
		}
	}

	public void OnModelCreatingTempBill(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<TempBill>().HasKey((Expression<Func<TempBill, object>>)((TempBill k) => new { k.OrderGuid, k.PlacementGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<TempBill>((Expression<Func<Order, IEnumerable<TempBill>>>)((Order x) => x.TempBills)).WithOne((Expression<Func<TempBill, Order>>)((TempBill x) => x.Order))
				.HasForeignKey((Expression<Func<TempBill, object>>)((TempBill x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<TempBill>((Expression<Func<Placement, IEnumerable<TempBill>>>)((Placement x) => x.TempBills)).WithOne((Expression<Func<TempBill, Placement>>)((TempBill x) => x.Placement))
				.HasForeignKey((Expression<Func<TempBill, object>>)((TempBill x) => x.PlacementGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<BillMaster>((Expression<Func<Cashier, IEnumerable<BillMaster>>>)((Cashier x) => x.BillMaster)).WithOne((Expression<Func<BillMaster, Cashier>>)((BillMaster x) => x.Cashier))
				.HasForeignKey((Expression<Func<BillMaster, object>>)((BillMaster x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<BillMaster>().HasMany<BillDetail>((Expression<Func<BillMaster, IEnumerable<BillDetail>>>)((BillMaster x) => x.BillDetail)).WithOne((Expression<Func<BillDetail, BillMaster>>)((BillDetail x) => x.BillMaster))
				.HasForeignKey((Expression<Func<BillDetail, object>>)((BillDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Placement>().HasMany<BillDetail>((Expression<Func<Placement, IEnumerable<BillDetail>>>)((Placement x) => x.BillDetails)).WithOne((Expression<Func<BillDetail, Placement>>)((BillDetail x) => x.Placement))
				.HasForeignKey((Expression<Func<BillDetail, object>>)((BillDetail x) => x.PlacementGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<BillDetail>((Expression<Func<Order, IEnumerable<BillDetail>>>)((Order x) => x.BillDetail)).WithOne((Expression<Func<BillDetail, Order>>)((BillDetail x) => x.Order))
				.HasForeignKey((Expression<Func<BillDetail, object>>)((BillDetail x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<BillMaster>((Expression<Func<Reception, IEnumerable<BillMaster>>>)((Reception x) => x.BillMaster)).WithOne((Expression<Func<BillMaster, Reception>>)((BillMaster x) => x.Reception))
				.HasForeignKey((Expression<Func<BillMaster, object>>)((BillMaster x) => x.ReceptionGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<BillMaster>((Expression<Func<Pardakht, IEnumerable<BillMaster>>>)((Pardakht x) => x.BillMaster)).WithOne((Expression<Func<BillMaster, Pardakht>>)((BillMaster x) => x.Pardakht))
				.HasForeignKey((Expression<Func<BillMaster, object>>)((BillMaster x) => x.PardakhtGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTempBill", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\TempBillModelContext.cs", 29);
		}
	}

	public void OnModelCreatingProductGroupRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductGroupRelation>().HasKey((Expression<Func<ProductGroupRelation, object>>)((ProductGroupRelation k) => new { k.ProductGuid, k.GroupGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ProductGroupRelation>((Expression<Func<Product, IEnumerable<ProductGroupRelation>>>)((Product x) => x.ProductGroupRelations)).WithOne((Expression<Func<ProductGroupRelation, Product>>)((ProductGroupRelation x) => x.Product))
				.HasForeignKey((Expression<Func<ProductGroupRelation, object>>)((ProductGroupRelation x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductGroup>().HasMany<ProductGroupRelation>((Expression<Func<ProductGroup, IEnumerable<ProductGroupRelation>>>)((ProductGroup x) => x.ProductGroupRelations)).WithOne((Expression<Func<ProductGroupRelation, ProductGroup>>)((ProductGroupRelation x) => x.Group))
				.HasForeignKey((Expression<Func<ProductGroupRelation, object>>)((ProductGroupRelation x) => x.GroupGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductGroupRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductGroupRelationModelContext.cs", 24);
		}
	}

	public void OnModelCreatingProductDecimalRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductDecimalRelation>().HasKey((Expression<Func<ProductDecimalRelation, object>>)((ProductDecimalRelation k) => new { k.ProductGuid, k.DecimalGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ProductDecimalRelation>((Expression<Func<Product, IEnumerable<ProductDecimalRelation>>>)((Product x) => x.ProductPrices)).WithOne((Expression<Func<ProductDecimalRelation, Product>>)((ProductDecimalRelation x) => x.Product))
				.HasForeignKey((Expression<Func<ProductDecimalRelation, object>>)((ProductDecimalRelation x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<ProductDecimalRelation>((Expression<Func<DecimalEntity, IEnumerable<ProductDecimalRelation>>>)((DecimalEntity x) => x.ProductPrices)).WithOne((Expression<Func<ProductDecimalRelation, DecimalEntity>>)((ProductDecimalRelation x) => x.Decimal))
				.HasForeignKey((Expression<Func<ProductDecimalRelation, object>>)((ProductDecimalRelation x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductDecimalRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductDecimalRelationModelContext.cs", 25);
		}
	}

	public void OnModelCreatingProductPriceUpdate(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ProductPriceUpdate>((Expression<Func<Product, IEnumerable<ProductPriceUpdate>>>)((Product x) => x.Updates)).WithOne((Expression<Func<ProductPriceUpdate, Product>>)((ProductPriceUpdate x) => x.Product))
				.HasForeignKey((Expression<Func<ProductPriceUpdate, object>>)((ProductPriceUpdate x) => x.ProductGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductPriceUpdate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductPriceUpdateModelContext.cs", 23);
		}
	}

	public void OnModelCreatingTozieList(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<TozieList>((Expression<Func<Cashier, IEnumerable<TozieList>>>)((Cashier x) => x.TozieLists)).WithOne((Expression<Func<TozieList, Cashier>>)((TozieList x) => x.CashierItem))
				.HasForeignKey((Expression<Func<TozieList, object>>)((TozieList x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TozieList>().HasMany<TozieListDetail>((Expression<Func<TozieList, IEnumerable<TozieListDetail>>>)((TozieList x) => x.DetailList)).WithOne((Expression<Func<TozieListDetail, TozieList>>)((TozieListDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<TozieListDetail, object>>)((TozieListDetail x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTozieList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\TozieListModelContext.cs", 23);
		}
	}

	public void OnModelCreatingPrdAttrebute(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductAttribute>((Action<EntityTypeBuilder<ProductAttribute>>)delegate(EntityTypeBuilder<ProductAttribute> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(q.Property<short>((Expression<Func<ProductAttribute, short>>)((ProductAttribute r) => r.Ordinal)), "((1))");
			});
			RelationalIndexBuilderExtensions.HasFilter<ProductAttribute>(modelBuilder.Entity<ProductAttribute>().HasIndex((Expression<Func<ProductAttribute, object>>)((ProductAttribute p) => p.Slug)).IsUnique(true), "Slug IS NOT NULL");
			RelationalIndexBuilderExtensions.HasFilter<ProductAttributeValue>(modelBuilder.Entity<ProductAttributeValue>().HasIndex((Expression<Func<ProductAttributeValue, object>>)((ProductAttributeValue p) => p.Slug)).IsUnique(true), "Slug IS NULL");
			modelBuilder.Entity<ProductAttributeValue>().HasIndex((Expression<Func<ProductAttributeValue, object>>)((ProductAttributeValue p) => new { p.MasterGuid, p.Name })).IsUnique(true);
			modelBuilder.Entity<ProductAttributeRelation>().HasKey((Expression<Func<ProductAttributeRelation, object>>)((ProductAttributeRelation p) => new { p.ProductGuid, p.AttributeValueGuid }));
			modelBuilder.Entity<ProductAttributeRelation>().HasOne<Product>((Expression<Func<ProductAttributeRelation, Product>>)((ProductAttributeRelation o) => o.Product)).WithMany((Expression<Func<Product, IEnumerable<ProductAttributeRelation>>>)((Product m) => m.ProductAttributeRelations))
				.HasForeignKey((Expression<Func<ProductAttributeRelation, object>>)((ProductAttributeRelation f) => f.ProductGuid));
			modelBuilder.Entity<ProductAttributeRelation>().HasOne<ProductAttributeValue>((Expression<Func<ProductAttributeRelation, ProductAttributeValue>>)((ProductAttributeRelation o) => o.ProductAttributeValue)).WithMany((Expression<Func<ProductAttributeValue, IEnumerable<ProductAttributeRelation>>>)((ProductAttributeValue m) => m.ProductAttributeRelations))
				.HasForeignKey((Expression<Func<ProductAttributeRelation, object>>)((ProductAttributeRelation f) => f.AttributeValueGuid));
			modelBuilder.Entity<ProductAttributeRelation>().HasOne<ProductAttribute>((Expression<Func<ProductAttributeRelation, ProductAttribute>>)((ProductAttributeRelation o) => o.ProductAttribute)).WithMany((Expression<Func<ProductAttribute, IEnumerable<ProductAttributeRelation>>>)((ProductAttribute m) => m.ProductAttributeRelations))
				.HasForeignKey((Expression<Func<ProductAttributeRelation, object>>)((ProductAttributeRelation f) => f.AttributeGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductAttribute>().HasMany<ProductAttributeValue>((Expression<Func<ProductAttribute, IEnumerable<ProductAttributeValue>>>)((ProductAttribute x) => x.ProductAttributeValues)).WithOne((Expression<Func<ProductAttributeValue, ProductAttribute>>)((ProductAttributeValue x) => x.MasterItem))
				.HasForeignKey((Expression<Func<ProductAttributeValue, object>>)((ProductAttributeValue x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPrdAttrebute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\PrdAttrebuteModelContext.cs", 34);
		}
	}

	public void OnModelCreatingPrdTechnical(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PrdTechnical>().HasKey((Expression<Func<PrdTechnical, object>>)((PrdTechnical x) => new { x.ProductGuid, x.TechnicalType })).IsCluster();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<PrdTechnical>((Expression<Func<Product, IEnumerable<PrdTechnical>>>)((Product x) => x.Technicals)).WithOne((Expression<Func<PrdTechnical, Product>>)((PrdTechnical x) => x.Product))
				.HasForeignKey((Expression<Func<PrdTechnical, object>>)((PrdTechnical x) => x.ProductGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPrdTechnical", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\PrdTechnicalModelContext.cs", 24);
		}
	}

	public void OnModelCreatingResid(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Resid>((Expression<Func<Tafsil, IEnumerable<Resid>>>)((Tafsil x) => x.Resid)).WithOne((Expression<Func<Resid, Tafsil>>)((Resid x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Resid, object>>)((Resid x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<Resid>((Expression<Func<Product, IEnumerable<Resid>>>)((Product x) => x.Resid)).WithOne((Expression<Func<Resid, Product>>)((Resid x) => x.Product))
				.HasForeignKey((Expression<Func<Resid, object>>)((Resid x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Resid>((Expression<Func<Cashier, IEnumerable<Resid>>>)((Cashier x) => x.Resid)).WithOne((Expression<Func<Resid, Cashier>>)((Resid x) => x.Cashier))
				.HasForeignKey((Expression<Func<Resid, object>>)((Resid x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<Resid>((Expression<Func<Order, IEnumerable<Resid>>>)((Order x) => x.Resid)).WithOne((Expression<Func<Resid, Order>>)((Resid x) => x.Order))
				.HasForeignKey((Expression<Func<Resid, object>>)((Resid x) => x.OrderGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<Resid>((Expression<Func<Reception, IEnumerable<Resid>>>)((Reception x) => x.Resid)).WithOne((Expression<Func<Resid, Reception>>)((Resid x) => x.Reception))
				.HasForeignKey((Expression<Func<Resid, object>>)((Resid x) => x.ReceptionGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingResid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ResidModelContext.cs", 26);
		}
	}

	public void OnModelCreatingNote(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<Note>().Property<DateTime?>((Expression<Func<Note, DateTime?>>)((Note p) => p.DateUtc)), "getdate()");
			modelBuilder.Entity<Note>().HasIndex((Expression<Func<Note, object>>)((Note p) => new { p.EntityGuid, p.Status }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Note>((Expression<Func<Cashier, IEnumerable<Note>>>)((Cashier x) => x.Notes)).WithOne((Expression<Func<Note, Cashier>>)((Note x) => x.Cashier))
				.HasForeignKey((Expression<Func<Note, object>>)((Note x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<NoteGroup>().HasMany<Note>((Expression<Func<NoteGroup, IEnumerable<Note>>>)((NoteGroup x) => x.Notes)).WithOne((Expression<Func<Note, NoteGroup>>)((Note x) => x.Group))
				.HasForeignKey((Expression<Func<Note, object>>)((Note x) => x.GroupGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Note>((Expression<Func<Tafsil, IEnumerable<Note>>>)((Tafsil x) => x.Notes)).WithOne((Expression<Func<Note, Tafsil>>)((Note x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Note, object>>)((Note x) => x.TafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingNote", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\NoteModelContext.cs", 26);
		}
	}

	public void OnModelCreatingSanadStatusLog(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<SanadStatusLog>((Expression<Func<Cashier, IEnumerable<SanadStatusLog>>>)((Cashier x) => x.SanadStatusLog)).WithOne((Expression<Func<SanadStatusLog, Cashier>>)((SanadStatusLog x) => x.Cashier))
				.HasForeignKey((Expression<Func<SanadStatusLog, object>>)((SanadStatusLog x) => x.CashierGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanadStatusLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadStatusLogModelContext.cs", 18);
		}
	}

	public void OnModelCreatingSanadStatusReport(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalIndexBuilderExtensions.HasName<SanadStatusReport>(modelBuilder.Entity<SanadStatusReport>().HasIndex((Expression<Func<SanadStatusReport, object>>)((SanadStatusReport d) => d.Guid)), "IX_Acc_SanadStatusReport_Guid").IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<SanadStatusReport>().HasMany<SanadStatusLog>((Expression<Func<SanadStatusReport, IEnumerable<SanadStatusLog>>>)((SanadStatusReport x) => x.SanadStatusLogNew)).WithOne((Expression<Func<SanadStatusLog, SanadStatusReport>>)((SanadStatusLog x) => x.SanadStatusReportNew))
				.HasForeignKey((Expression<Func<SanadStatusLog, object>>)((SanadStatusLog x) => x.NewStatus))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<SanadStatusReport>().HasMany<SanadStatusLog>((Expression<Func<SanadStatusReport, IEnumerable<SanadStatusLog>>>)((SanadStatusReport x) => x.SanadStatusLogOld)).WithOne((Expression<Func<SanadStatusLog, SanadStatusReport>>)((SanadStatusLog x) => x.SanadStatusReportOld))
				.HasForeignKey((Expression<Func<SanadStatusLog, object>>)((SanadStatusLog x) => x.OldStatus))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanadStatusReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadStatusReportModelContext.cs", 23);
		}
	}

	public void OnModelCreatingViewSanadDetailCover(ModelBuilder modelBuilder)
	{
		try
		{
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<ViewSanadDetailCover>(modelBuilder.Entity<ViewSanadDetailCover>().HasIndex((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover p) => new { p.AsGuid, p.AsTypeDocument })), "IX_Acc_ViewSanadDetailCover_AsGuid_AsTypeDocument")).IsCluster().IsUnique(false);
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<ViewSanadDetailCover>(modelBuilder.Entity<ViewSanadDetailCover>().HasIndex((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover p) => new { p.CreditMoeinGuid, p.CreditTafsilGuid, p.Date })), "IX_Acc_ViewSanadDetailCover_CreditItems")).IsCluster(isCluster: false).IsUnique(false);
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<ViewSanadDetailCover>(modelBuilder.Entity<ViewSanadDetailCover>().HasIndex((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover p) => new { p.DebitMoeinGuid, p.DebitTafsilGuid, p.Date })), "IX_Acc_ViewSanadDetailCover_DebitItems")).IsCluster(isCluster: false).IsUnique(false);
			modelBuilder.Entity<ViewSanadDetailCover>().HasKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover p) => new { p.Guid })).IsCluster(isCluster: false);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<ViewSanadDetailCover>((Expression<Func<Depot, IEnumerable<ViewSanadDetailCover>>>)((Depot x) => x.ViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Depot>>)((ViewSanadDetailCover x) => x.DepotItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<ViewSanadDetailCover>((Expression<Func<ProductVahed, IEnumerable<ViewSanadDetailCover>>>)((ProductVahed x) => x.ViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, ProductVahed>>)((ViewSanadDetailCover x) => x.VahedItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.VahedGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ViewSanadDetailCover>((Expression<Func<Cashier, IEnumerable<ViewSanadDetailCover>>>)((Cashier x) => x.ViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Cashier>>)((ViewSanadDetailCover x) => x.CashierItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<ViewSanadDetailCover>((Expression<Func<DecimalEntity, IEnumerable<ViewSanadDetailCover>>>)((DecimalEntity x) => x.ViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, DecimalEntity>>)((ViewSanadDetailCover x) => x.DecimalItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.DecimalGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetailCover>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetailCover>>>)((Tafsil x) => x.ViewSanadDetailCover2)).WithOne((Expression<Func<ViewSanadDetailCover, Tafsil>>)((ViewSanadDetailCover x) => x.Tafsil2))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.TafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetailCover>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetailCover>>>)((Tafsil x) => x.ViewSanadDetailCover3)).WithOne((Expression<Func<ViewSanadDetailCover, Tafsil>>)((ViewSanadDetailCover x) => x.Tafsil3))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.TafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetailCover>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetailCover>>>)((Tafsil x) => x.ViewSanadDetailCover4)).WithOne((Expression<Func<ViewSanadDetailCover, Tafsil>>)((ViewSanadDetailCover x) => x.Tafsil4))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.TafsilGuid4))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetailCover>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetailCover>>>)((Tafsil x) => x.CreditViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Tafsil>>)((ViewSanadDetailCover x) => x.CreditTafsilItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetailCover>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetailCover>>>)((Tafsil x) => x.DebitViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Tafsil>>)((ViewSanadDetailCover x) => x.DebitTafsilItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ViewSanadDetailCover>((Expression<Func<Moein, IEnumerable<ViewSanadDetailCover>>>)((Moein x) => x.CreditViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Moein>>)((ViewSanadDetailCover x) => x.CreditMoeinItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ViewSanadDetailCover>((Expression<Func<Moein, IEnumerable<ViewSanadDetailCover>>>)((Moein x) => x.DebitViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Moein>>)((ViewSanadDetailCover x) => x.DebitMoeinItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<ViewSanadDetailCover>((Expression<Func<Product, IEnumerable<ViewSanadDetailCover>>>)((Product x) => x.ViewSanadDetailCover)).WithOne((Expression<Func<ViewSanadDetailCover, Product>>)((ViewSanadDetailCover x) => x.ProductItem))
				.HasForeignKey((Expression<Func<ViewSanadDetailCover, object>>)((ViewSanadDetailCover x) => x.ProductGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingViewSanadDetailCover", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\ViewSanadDetailCoverModelContext.cs", 41);
		}
	}

	public void OnModelCreatingViewSanadDetail(ModelBuilder modelBuilder)
	{
		try
		{
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<ViewSanadDetail>(modelBuilder.Entity<ViewSanadDetail>().HasIndex((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail p) => new { p.AsGuid, p.AsType })), "IX_AcC_ViewSanadDetail_AsGuid_AsType")).IsCluster();
			modelBuilder.Entity<ViewSanadDetail>().HasIndex((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail p) => p.SanadNo));
			modelBuilder.Entity<ViewSanadDetail>().HasKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail p) => new { p.Guid })).IsCluster(isCluster: false);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetail>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetail>>>)((Tafsil x) => x.SanadDetails)).WithOne((Expression<Func<ViewSanadDetail, Tafsil>>)((ViewSanadDetail x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetail>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetail>>>)((Tafsil x) => x.SanadDetails2)).WithOne((Expression<Func<ViewSanadDetail, Tafsil>>)((ViewSanadDetail x) => x.Tafsil2))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.TafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetail>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetail>>>)((Tafsil x) => x.SanadDetails3)).WithOne((Expression<Func<ViewSanadDetail, Tafsil>>)((ViewSanadDetail x) => x.Tafsil3))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.TafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ViewSanadDetail>((Expression<Func<Tafsil, IEnumerable<ViewSanadDetail>>>)((Tafsil x) => x.SanadDetails4)).WithOne((Expression<Func<ViewSanadDetail, Tafsil>>)((ViewSanadDetail x) => x.Tafsil4))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.TafsilGuid4))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Kol>().HasMany<ViewSanadDetail>((Expression<Func<Kol, IEnumerable<ViewSanadDetail>>>)((Kol x) => x.SanadDetails)).WithOne((Expression<Func<ViewSanadDetail, Kol>>)((ViewSanadDetail x) => x.KolItem))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.KolGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ViewSanadDetail>((Expression<Func<Moein, IEnumerable<ViewSanadDetail>>>)((Moein x) => x.SanadDetails)).WithOne((Expression<Func<ViewSanadDetail, Moein>>)((ViewSanadDetail x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ViewSanadDetail>((Expression<Func<Cashier, IEnumerable<ViewSanadDetail>>>)((Cashier x) => x.SanadDetails)).WithOne((Expression<Func<ViewSanadDetail, Cashier>>)((ViewSanadDetail x) => x.CashierItem))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<ViewSanadDetail>((Expression<Func<DecimalEntity, IEnumerable<ViewSanadDetail>>>)((DecimalEntity x) => x.SanadDetails)).WithOne((Expression<Func<ViewSanadDetail, DecimalEntity>>)((ViewSanadDetail x) => x.DecimalItem))
				.HasForeignKey((Expression<Func<ViewSanadDetail, object>>)((ViewSanadDetail x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingViewSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\ViewSanadDetailModelContext.cs", 31);
		}
	}

	public void OnModelCreatingSanadStatusReportUpdates(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<SanadStatusReportUpdates>().HasKey((Expression<Func<SanadStatusReportUpdates, object>>)((SanadStatusReportUpdates p) => new { p.Guid })).IsCluster(isCluster: false);
			SqlServerIndexBuilderExtensions.IsClustered<SanadStatusReportUpdates>(RelationalIndexBuilderExtensions.HasName<SanadStatusReportUpdates>(modelBuilder.Entity<SanadStatusReportUpdates>().HasIndex((Expression<Func<SanadStatusReportUpdates, object>>)((SanadStatusReportUpdates p) => new { p.AsGuid, p.Ordinal, p.AsType })), "IX_Acc_SanadStatusReportUpdates_AsGuid_Ordinal_AsType"), true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<SanadStatusReportUpdates>((Expression<Func<Cashier, IEnumerable<SanadStatusReportUpdates>>>)((Cashier x) => x.SanadStatusReportUpdates)).WithOne((Expression<Func<SanadStatusReportUpdates, Cashier>>)((SanadStatusReportUpdates x) => x.CashierItem))
				.HasForeignKey((Expression<Func<SanadStatusReportUpdates, object>>)((SanadStatusReportUpdates x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<SanadStatusReport>().HasMany<SanadStatusReportUpdates>((Expression<Func<SanadStatusReport, IEnumerable<SanadStatusReportUpdates>>>)((SanadStatusReport x) => x.SanadStatusReportUpdates)).WithOne((Expression<Func<SanadStatusReportUpdates, SanadStatusReport>>)((SanadStatusReportUpdates x) => x.StatusItem))
				.HasForeignKey((Expression<Func<SanadStatusReportUpdates, object>>)((SanadStatusReportUpdates x) => x.StatusId))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanadStatusReportUpdates", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadStatusReportUpdatesModelContext.cs", 28);
		}
	}

	public void OnModelCreatingProductGroupSelection(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ProductGroupSelected>((Expression<Func<Cashier, IEnumerable<ProductGroupSelected>>>)((Cashier x) => x.ProductGroupSelected)).WithOne((Expression<Func<ProductGroupSelected, Cashier>>)((ProductGroupSelected x) => x.CashierItem))
				.HasForeignKey((Expression<Func<ProductGroupSelected, object>>)((ProductGroupSelected x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductGroupSelected>().HasMany<ProductGroupSelectedDetails>((Expression<Func<ProductGroupSelected, IEnumerable<ProductGroupSelectedDetails>>>)((ProductGroupSelected x) => x.ProductGroupSelectedDetails)).WithOne((Expression<Func<ProductGroupSelectedDetails, ProductGroupSelected>>)((ProductGroupSelectedDetails x) => x.Master))
				.HasForeignKey((Expression<Func<ProductGroupSelectedDetails, object>>)((ProductGroupSelectedDetails x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductGroup>().HasMany<ProductGroupSelectedDetails>((Expression<Func<ProductGroup, IEnumerable<ProductGroupSelectedDetails>>>)((ProductGroup x) => x.ProductGroupSelectedDetails)).WithOne((Expression<Func<ProductGroupSelectedDetails, ProductGroup>>)((ProductGroupSelectedDetails x) => x.GroupItem))
				.HasForeignKey((Expression<Func<ProductGroupSelectedDetails, object>>)((ProductGroupSelectedDetails x) => x.GroupGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductGroupSelection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Selection\\ProductGroupSelectionModelContext.cs", 25);
		}
	}

	public void OnModelCreatingShortCut(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ShortCut>((Expression<Func<Cashier, IEnumerable<ShortCut>>>)((Cashier x) => x.ShortCuts)).WithOne((Expression<Func<ShortCut, Cashier>>)((ShortCut x) => x.Cashier))
				.HasForeignKey((Expression<Func<ShortCut, object>>)((ShortCut x) => x.CashierGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingShortCut", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\ShortCutModelContext.cs", 20);
		}
	}

	public void OnModelCreatingChequeBook(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ChequeBook>((Expression<Func<Tafsil, IEnumerable<ChequeBook>>>)((Tafsil x) => x.ChequeBooks)).WithOne((Expression<Func<ChequeBook, Tafsil>>)((ChequeBook x) => x.Tafsil))
				.HasForeignKey((Expression<Func<ChequeBook, object>>)((ChequeBook x) => x.BankGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingChequeBook", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\ChequeBookModelContext.cs", 25);
		}
	}

	public void OnModelCreatingChequeShOperation(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ChequeShOperation>().Property<DateTime?>((Expression<Func<ChequeShOperation, DateTime?>>)((ChequeShOperation p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PardakhtCheckSh>().HasMany<ChequeShOperation>((Expression<Func<PardakhtCheckSh, IEnumerable<ChequeShOperation>>>)((PardakhtCheckSh x) => x.ChequeShOperation)).WithOne((Expression<Func<ChequeShOperation, PardakhtCheckSh>>)((ChequeShOperation x) => x.Cheque))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.ChequeGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ChequeShOperation>((Expression<Func<Cashier, IEnumerable<ChequeShOperation>>>)((Cashier x) => x.ChequeShOperation)).WithOne((Expression<Func<ChequeShOperation, Cashier>>)((ChequeShOperation x) => x.Cashier))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ChequeShOperation>((Expression<Func<Moein, IEnumerable<ChequeShOperation>>>)((Moein x) => x.ChequeShOperationCredit)).WithOne((Expression<Func<ChequeShOperation, Moein>>)((ChequeShOperation x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ChequeShOperation>((Expression<Func<Moein, IEnumerable<ChequeShOperation>>>)((Moein x) => x.ChequeShOperationDebit)).WithOne((Expression<Func<ChequeShOperation, Moein>>)((ChequeShOperation x) => x.DebitMoein))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ChequeShOperation>((Expression<Func<Tafsil, IEnumerable<ChequeShOperation>>>)((Tafsil x) => x.ChequeShOperationCredit)).WithOne((Expression<Func<ChequeShOperation, Tafsil>>)((ChequeShOperation x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ChequeShOperation>((Expression<Func<Tafsil, IEnumerable<ChequeShOperation>>>)((Tafsil x) => x.ChequeShOperationDebit)).WithOne((Expression<Func<ChequeShOperation, Tafsil>>)((ChequeShOperation x) => x.DebitTafsil))
				.HasForeignKey((Expression<Func<ChequeShOperation, object>>)((ChequeShOperation x) => x.DebitTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingChequeShOperation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\ChequeShOperationModelContext.cs", 30);
		}
	}

	public void OnModelCreatingViewChequeM(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ViewCheckM>().Property<DateTime?>((Expression<Func<ViewCheckM, DateTime?>>)((ViewCheckM p) => p.DateUtc)), "getdate()");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingViewChequeM", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\ViewChequeMModelContext.cs", 19);
		}
	}

	public void OnModelCreatingLastPrice(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<LastPrice>().HasKey((Expression<Func<LastPrice, object>>)((LastPrice k) => new { k.TafsilGuid, k.PrdGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<LastPrice>((Expression<Func<Product, IEnumerable<LastPrice>>>)((Product x) => x.LastPrice)).WithOne((Expression<Func<LastPrice, Product>>)((LastPrice x) => x.Product))
				.HasForeignKey((Expression<Func<LastPrice, object>>)((LastPrice x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<LastPrice>((Expression<Func<Tafsil, IEnumerable<LastPrice>>>)((Tafsil x) => x.LastPrice)).WithOne((Expression<Func<LastPrice, Tafsil>>)((LastPrice x) => x.Tafsil))
				.HasForeignKey((Expression<Func<LastPrice, object>>)((LastPrice x) => x.TafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingLastPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\LastPriceModelContext.cs", 26);
		}
	}

	public void OnModelCreatingPardakht(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Pardakht>().HasIndex((Expression<Func<Pardakht, object>>)((Pardakht p) => p.Id2)).IsUnique(true);
			modelBuilder.Entity<Pardakht>().HasIndex((Expression<Func<Pardakht, object>>)((Pardakht p) => p.ServerStatus));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<Pardakht>().Property<DateTime?>((Expression<Func<Pardakht, DateTime?>>)((Pardakht p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Pardakht>((Expression<Func<Tafsil, IEnumerable<Pardakht>>>)((Tafsil x) => x.TafsilPardakht)).WithOne((Expression<Func<Pardakht, Tafsil>>)((Pardakht x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<Pardakht, object>>)((Pardakht x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Pardakht>((Expression<Func<Tafsil, IEnumerable<Pardakht>>>)((Tafsil x) => x.TafsilPardakht2)).WithOne((Expression<Func<Pardakht, Tafsil>>)((Pardakht x) => x.TafsilItem2))
				.HasForeignKey((Expression<Func<Pardakht, object>>)((Pardakht x) => x.TafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Pardakht>((Expression<Func<Tafsil, IEnumerable<Pardakht>>>)((Tafsil x) => x.TafsilPardakht3)).WithOne((Expression<Func<Pardakht, Tafsil>>)((Pardakht x) => x.TafsilItem3))
				.HasForeignKey((Expression<Func<Pardakht, object>>)((Pardakht x) => x.TafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Pardakht>((Expression<Func<Moein, IEnumerable<Pardakht>>>)((Moein x) => x.MoeinPardakht)).WithOne((Expression<Func<Pardakht, Moein>>)((Pardakht x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<Pardakht, object>>)((Pardakht x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Pardakht>((Expression<Func<Cashier, IEnumerable<Pardakht>>>)((Cashier x) => x.CashierPardakht)).WithOne((Expression<Func<Pardakht, Cashier>>)((Pardakht x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Pardakht, object>>)((Pardakht x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<Order>().HasOne<Pardakht>((Expression<Func<Order, Pardakht>>)((Order x) => x.PardakhtItem)).WithOne((Expression<Func<Pardakht, Order>>)((Pardakht x) => x.OrderItem))
				.HasForeignKey<Order>((Expression<Func<Order, object>>)((Order x) => x.PardakhtGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakht", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtModelContext.cs", 33);
		}
	}

	public void OnModelCreatingReception(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Reception>().HasIndex((Expression<Func<Reception, object>>)((Reception p) => p.Id2)).IsUnique(true);
			modelBuilder.Entity<Reception>().HasIndex((Expression<Func<Reception, object>>)((Reception p) => p.ServerStatus));
			modelBuilder.Entity<Reception>().HasIndex((Expression<Func<Reception, object>>)((Reception p) => p.Date));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<Reception>().Property<DateTime?>((Expression<Func<Reception, DateTime?>>)((Reception p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Reception>((Expression<Func<Tafsil, IEnumerable<Reception>>>)((Tafsil x) => x.TafsilReception)).WithOne((Expression<Func<Reception, Tafsil>>)((Reception x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<Reception, object>>)((Reception x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Reception>((Expression<Func<Tafsil, IEnumerable<Reception>>>)((Tafsil x) => x.TafsilReception2)).WithOne((Expression<Func<Reception, Tafsil>>)((Reception x) => x.TafsilItem2))
				.HasForeignKey((Expression<Func<Reception, object>>)((Reception x) => x.TafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Reception>((Expression<Func<Tafsil, IEnumerable<Reception>>>)((Tafsil x) => x.TafsilReception3)).WithOne((Expression<Func<Reception, Tafsil>>)((Reception x) => x.TafsilItem3))
				.HasForeignKey((Expression<Func<Reception, object>>)((Reception x) => x.TafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Reception>((Expression<Func<Moein, IEnumerable<Reception>>>)((Moein x) => x.MoeinReception)).WithOne((Expression<Func<Reception, Moein>>)((Reception x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<Reception, object>>)((Reception x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Reception>((Expression<Func<Cashier, IEnumerable<Reception>>>)((Cashier x) => x.Receptions)).WithOne((Expression<Func<Reception, Cashier>>)((Reception x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Reception, object>>)((Reception x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceReferenceBuilder)(object)modelBuilder.Entity<Order>().HasOne<Reception>((Expression<Func<Order, Reception>>)((Order x) => x.ReceptionItem)).WithOne((Expression<Func<Reception, Order>>)((Reception x) => x.OrderItem))
				.HasForeignKey<Order>((Expression<Func<Order, object>>)((Order x) => x.ReceptionGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReception", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ReceptionModelContext.cs", 36);
		}
	}

	public void OnModelCreatingIndex(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<IxCustomerAllFields>().HasKey((Expression<Func<IxCustomerAllFields, object>>)((IxCustomerAllFields k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductAllFields>().HasKey((Expression<Func<IxProductAllFields, object>>)((IxProductAllFields k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductCodeField>().HasKey((Expression<Func<IxProductCodeField, object>>)((IxProductCodeField k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductDescField>().HasKey((Expression<Func<IxProductDescField, object>>)((IxProductDescField k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductNameAndCodeField>().HasKey((Expression<Func<IxProductNameAndCodeField, object>>)((IxProductNameAndCodeField k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductNameField>().HasKey((Expression<Func<IxProductNameField, object>>)((IxProductNameField k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT1Field>().HasKey((Expression<Func<IxProductT1Field, object>>)((IxProductT1Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT2Field>().HasKey((Expression<Func<IxProductT2Field, object>>)((IxProductT2Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT3Field>().HasKey((Expression<Func<IxProductT3Field, object>>)((IxProductT3Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT4Field>().HasKey((Expression<Func<IxProductT4Field, object>>)((IxProductT4Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT5Field>().HasKey((Expression<Func<IxProductT5Field, object>>)((IxProductT5Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT6Field>().HasKey((Expression<Func<IxProductT6Field, object>>)((IxProductT6Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT7Field>().HasKey((Expression<Func<IxProductT7Field, object>>)((IxProductT7Field k) => new { k.WordId, k.EntityGuid }));
			modelBuilder.Entity<IxProductT8Field>().HasKey((Expression<Func<IxProductT8Field, object>>)((IxProductT8Field k) => new { k.WordId, k.EntityGuid }));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingIndex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\IndexModelContext.cs", 51);
		}
	}

	public void OnModelCreatingFareOrderCredit(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<FareOrderCredit>((Expression<Func<Tafsil, IEnumerable<FareOrderCredit>>>)((Tafsil x) => x.FareOrderCredits)).WithOne((Expression<Func<FareOrderCredit, Tafsil>>)((FareOrderCredit x) => x.Tafsil))
				.HasForeignKey((Expression<Func<FareOrderCredit, object>>)((FareOrderCredit x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<FareOrderCredit>((Expression<Func<Moein, IEnumerable<FareOrderCredit>>>)((Moein x) => x.FareOrderCredits)).WithOne((Expression<Func<FareOrderCredit, Moein>>)((FareOrderCredit x) => x.Moein))
				.HasForeignKey((Expression<Func<FareOrderCredit, object>>)((FareOrderCredit x) => x.MoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingFareOrderCredit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\FareOrderCreditContext.cs", 21);
		}
	}

	public void OnModelCreatingAnbarGardaniDetail(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PrdAnbarGardani>().HasMany<PrdAnbarGardaniDet>((Expression<Func<PrdAnbarGardani, IEnumerable<PrdAnbarGardaniDet>>>)((PrdAnbarGardani x) => x.Details)).WithOne((Expression<Func<PrdAnbarGardaniDet, PrdAnbarGardani>>)((PrdAnbarGardaniDet x) => x.Master))
				.HasForeignKey((Expression<Func<PrdAnbarGardaniDet, object>>)((PrdAnbarGardaniDet x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<PrdAnbarGardaniDet>((Expression<Func<Depot, IEnumerable<PrdAnbarGardaniDet>>>)((Depot x) => x.AnbarGardani)).WithOne((Expression<Func<PrdAnbarGardaniDet, Depot>>)((PrdAnbarGardaniDet x) => x.Depot))
				.HasForeignKey((Expression<Func<PrdAnbarGardaniDet, object>>)((PrdAnbarGardaniDet x) => x.DptGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<PrdAnbarGardaniDet>((Expression<Func<Product, IEnumerable<PrdAnbarGardaniDet>>>)((Product x) => x.AnbarGardani)).WithOne((Expression<Func<PrdAnbarGardaniDet, Product>>)((PrdAnbarGardaniDet x) => x.Product))
				.HasForeignKey((Expression<Func<PrdAnbarGardaniDet, object>>)((PrdAnbarGardaniDet x) => x.PrdGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingAnbarGardaniDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\AnbarGardaniDetailModelContext.cs", 26);
		}
	}

	public void OnModelCreatingAnbarGardani(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<PrdAnbarGardani>().Property<DateTime?>((Expression<Func<PrdAnbarGardani, DateTime?>>)((PrdAnbarGardani p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<PrdAnbarGardani>((Expression<Func<Cashier, IEnumerable<PrdAnbarGardani>>>)((Cashier x) => x.PrdAnbarGardanis)).WithOne((Expression<Func<PrdAnbarGardani, Cashier>>)((PrdAnbarGardani x) => x.CashierItem))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.KasrDebit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.KasrDebitTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrDebitTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.EzafeDebit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.EzafeDebitTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeDebitTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.EzafeCredit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.EzafeCreditTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeCreditTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.KasrDebit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.KasrDebitTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrDebitTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.KasrCredit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.KasrCreditTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrCreditTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.EzafeDebit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.EzafeDebitTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeDebitTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.EzafeCredit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.EzafeCreditTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeCreditTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PrdAnbarGardani>((Expression<Func<Tafsil, IEnumerable<PrdAnbarGardani>>>)((Tafsil x) => x.KasrCredit)).WithOne((Expression<Func<PrdAnbarGardani, Tafsil>>)((PrdAnbarGardani x) => x.KasrCreditTafsil))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrCreditTafsilGuid))
				.IsRequired(false)).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.KasrCredit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.KasrCreditMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrCreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.KasrDebit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.KasrDebitMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrDebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.EzafeDebit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.EzafeDebitMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeDebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.EzafeCredit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.EzafeCreditMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeCreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.KasrDebit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.KasrDebitMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrDebitMoeinGuid))).NotRequiredInSqlite().OnDeleteNoAction().IsRequired(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.KasrCredit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.KasrCreditMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.KasrCreditMoeinGuid))).NotRequiredInSqlite().OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.EzafeDebit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.EzafeDebitMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeDebitMoeinGuid))).NotRequiredInSqlite().OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PrdAnbarGardani>((Expression<Func<Moein, IEnumerable<PrdAnbarGardani>>>)((Moein x) => x.EzafeCredit)).WithOne((Expression<Func<PrdAnbarGardani, Moein>>)((PrdAnbarGardani x) => x.EzafeCreditMoein))
				.HasForeignKey((Expression<Func<PrdAnbarGardani, object>>)((PrdAnbarGardani x) => x.EzafeCreditMoeinGuid))).NotRequiredInSqlite().OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingAnbarGardani", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\AnbarGardaniModelContext.cs", 41);
		}
	}

	public void OnModelCreatingDepEnteqal(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<DepEnteghal>((Expression<Func<Cashier, IEnumerable<DepEnteghal>>>)((Cashier x) => x.DepEnteghals)).WithOne((Expression<Func<DepEnteghal, Cashier>>)((DepEnteghal x) => x.CashierItem))
				.HasForeignKey((Expression<Func<DepEnteghal, object>>)((DepEnteghal x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DepEnteghal>().HasMany<DepEnteghalDet>((Expression<Func<DepEnteghal, IEnumerable<DepEnteghalDet>>>)((DepEnteghal x) => x.DepEnteghalDets)).WithOne((Expression<Func<DepEnteghalDet, DepEnteghal>>)((DepEnteghalDet x) => x.DepEnteghal))
				.HasForeignKey((Expression<Func<DepEnteghalDet, object>>)((DepEnteghalDet x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<DepEnteghalDet>((Expression<Func<Depot, IEnumerable<DepEnteghalDet>>>)((Depot x) => x.DepEnteghalDets)).WithOne((Expression<Func<DepEnteghalDet, Depot>>)((DepEnteghalDet x) => x.Depot))
				.HasForeignKey((Expression<Func<DepEnteghalDet, object>>)((DepEnteghalDet x) => x.DptGuidDst))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<DepEnteghalDet>((Expression<Func<Depot, IEnumerable<DepEnteghalDet>>>)((Depot x) => x.DepEnteghalDets)).WithOne((Expression<Func<DepEnteghalDet, Depot>>)((DepEnteghalDet x) => x.Depot))
				.HasForeignKey((Expression<Func<DepEnteghalDet, object>>)((DepEnteghalDet x) => x.DptGuidSrc))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<DepEnteghalDet>((Expression<Func<Product, IEnumerable<DepEnteghalDet>>>)((Product x) => x.DepEnteghalDets)).WithOne((Expression<Func<DepEnteghalDet, Product>>)((DepEnteghalDet x) => x.Product))
				.HasForeignKey((Expression<Func<DepEnteghalDet, object>>)((DepEnteghalDet x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<DepEnteghalDet>((Expression<Func<ProductVahed, IEnumerable<DepEnteghalDet>>>)((ProductVahed x) => x.DepEnteghalDets)).WithOne((Expression<Func<DepEnteghalDet, ProductVahed>>)((DepEnteghalDet x) => x.ProductVahed))
				.HasForeignKey((Expression<Func<DepEnteghalDet, object>>)((DepEnteghalDet x) => x.VahedGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<DepEnteghal>((Expression<Func<Moein, IEnumerable<DepEnteghal>>>)((Moein x) => x.DebitDepEnteghal)).WithOne((Expression<Func<DepEnteghal, Moein>>)((DepEnteghal x) => x.DebitMoeinItem))
				.HasForeignKey((Expression<Func<DepEnteghal, object>>)((DepEnteghal x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<DepEnteghal>((Expression<Func<Moein, IEnumerable<DepEnteghal>>>)((Moein x) => x.CreditDepEnteghal)).WithOne((Expression<Func<DepEnteghal, Moein>>)((DepEnteghal x) => x.CreditMoeinItem))
				.HasForeignKey((Expression<Func<DepEnteghal, object>>)((DepEnteghal x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<DepEnteghal>((Expression<Func<Tafsil, IEnumerable<DepEnteghal>>>)((Tafsil x) => x.DebitDepEnteghal)).WithOne((Expression<Func<DepEnteghal, Tafsil>>)((DepEnteghal x) => x.DebitTafsilItem))
				.HasForeignKey((Expression<Func<DepEnteghal, object>>)((DepEnteghal x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<DepEnteghal>((Expression<Func<Tafsil, IEnumerable<DepEnteghal>>>)((Tafsil x) => x.CreditDepEnteghal)).WithOne((Expression<Func<DepEnteghal, Tafsil>>)((DepEnteghal x) => x.CreditTafsilItem))
				.HasForeignKey((Expression<Func<DepEnteghal, object>>)((DepEnteghal x) => x.CreditTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingDepEnteqal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\DepEnteqalModelContext.cs", 32);
		}
	}

	public void OnModelCreatingPrdSerial(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalIndexBuilderExtensions.HasFilter<PrdSerial>(modelBuilder.Entity<PrdSerial>().HasIndex((Expression<Func<PrdSerial, object>>)((PrdSerial p) => new { p.Serial, p.ProductGuid })).IsUnique(true), "Serial IS NOT NULL");
			RelationalIndexBuilderExtensions.HasFilter<PrdSerial>(modelBuilder.Entity<PrdSerial>().HasIndex((Expression<Func<PrdSerial, object>>)((PrdSerial p) => p.GarrantyCode)).IsUnique(true), "GarrantyCode IS NOT NULL");
			modelBuilder.Entity<PrdSerial>((Action<EntityTypeBuilder<PrdSerial>>)delegate(EntityTypeBuilder<PrdSerial> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(q.Property<DateTime>((Expression<Func<PrdSerial, DateTime>>)((PrdSerial r) => r.InsertDate)), "getdate()");
			});
			modelBuilder.Entity<PrdSerialDetails>((Action<EntityTypeBuilder<PrdSerialDetails>>)delegate(EntityTypeBuilder<PrdSerialDetails> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(q.Property<short>((Expression<Func<PrdSerialDetails, short>>)((PrdSerialDetails r) => r.Ordinal)), "((0))");
			});
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<PrdSerial>((Expression<Func<Product, IEnumerable<PrdSerial>>>)((Product x) => x.Serials)).WithOne((Expression<Func<PrdSerial, Product>>)((PrdSerial x) => x.ProductItem))
				.HasForeignKey((Expression<Func<PrdSerial, object>>)((PrdSerial x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PrdSerial>().HasMany<PrdSerialDetails>((Expression<Func<PrdSerial, IEnumerable<PrdSerialDetails>>>)((PrdSerial x) => x.DetailItem)).WithOne((Expression<Func<PrdSerialDetails, PrdSerial>>)((PrdSerialDetails x) => x.MasterItem))
				.HasForeignKey((Expression<Func<PrdSerialDetails, object>>)((PrdSerialDetails x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPrdSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\PrdSerialModelContext.cs", 28);
		}
	}

	public void OnModelCreatingPrdAnbarRelation(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PrdAnbarRelation>().HasKey((Expression<Func<PrdAnbarRelation, object>>)((PrdAnbarRelation k) => new { k.PrdGuid, k.AnbarGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<PrdAnbarRelation>((Expression<Func<Product, IEnumerable<PrdAnbarRelation>>>)((Product x) => x.PrdAnbarRelations)).WithOne((Expression<Func<PrdAnbarRelation, Product>>)((PrdAnbarRelation x) => x.Product))
				.HasForeignKey((Expression<Func<PrdAnbarRelation, object>>)((PrdAnbarRelation x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<PrdAnbarRelation>((Expression<Func<Depot, IEnumerable<PrdAnbarRelation>>>)((Depot x) => x.PrdAnbarRelations)).WithOne((Expression<Func<PrdAnbarRelation, Depot>>)((PrdAnbarRelation x) => x.Depot))
				.HasForeignKey((Expression<Func<PrdAnbarRelation, object>>)((PrdAnbarRelation x) => x.AnbarGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPrdAnbarRelation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\PrdAnbarRelationModelContext.cs", 23);
		}
	}

	public void OnModelCreatingProduct(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Product>().HasIndex((Expression<Func<Product, object>>)((Product p) => p.ServerStatus));
			modelBuilder.Entity<Product>().HasIndex((Expression<Func<Product, object>>)((Product p) => p.IndexingType));
			modelBuilder.Entity<Product>().HasIndex((Expression<Func<Product, object>>)((Product p) => p.Sku)).IsUnique(true);
			modelBuilder.Entity<Product>().HasIndex((Expression<Func<Product, object>>)((Product p) => p.Slug)).IsUnique(true);
			modelBuilder.Entity<ProductVahed>().HasMany<Product>((Expression<Func<ProductVahed, IEnumerable<Product>>>)((ProductVahed x) => x.Products)).WithOne((Expression<Func<Product, ProductVahed>>)((Product x) => x.VahedItem))
				.HasForeignKey((Expression<Func<Product, object>>)((Product x) => x.GuidVahed))
				.OnDelete((DeleteBehavior)5);
			modelBuilder.Entity<TolidFormula>().HasMany<Product>((Expression<Func<TolidFormula, IEnumerable<Product>>>)((TolidFormula x) => x.Products)).WithOne((Expression<Func<Product, TolidFormula>>)((Product x) => x.TolidFormulaItem))
				.HasForeignKey((Expression<Func<Product, object>>)((Product x) => x.TolidFormulaGuid))
				.OnDelete((DeleteBehavior)5);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProduct", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductModelContext.cs", 27);
		}
	}

	public void OnModelCreatingSanadDetail(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Sanad_Det>().HasIndex((Expression<Func<Sanad_Det, object>>)((Sanad_Det p) => p.AsGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Sanad_Det>((Expression<Func<Moein, IEnumerable<Sanad_Det>>>)((Moein x) => x.SanadDet)).WithOne((Expression<Func<Sanad_Det, Moein>>)((Sanad_Det x) => x.Moein))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Sanad_Det>((Expression<Func<Tafsil, IEnumerable<Sanad_Det>>>)((Tafsil x) => x.SanadDet)).WithOne((Expression<Func<Sanad_Det, Tafsil>>)((Sanad_Det x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Sanad_Det>((Expression<Func<Tafsil, IEnumerable<Sanad_Det>>>)((Tafsil x) => x.SanadDet2)).WithOne((Expression<Func<Sanad_Det, Tafsil>>)((Sanad_Det x) => x.Tafsil2))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.TafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Sanad_Det>((Expression<Func<Tafsil, IEnumerable<Sanad_Det>>>)((Tafsil x) => x.SanadDet3)).WithOne((Expression<Func<Sanad_Det, Tafsil>>)((Sanad_Det x) => x.Tafsil3))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.TafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Sanad_Det>((Expression<Func<Tafsil, IEnumerable<Sanad_Det>>>)((Tafsil x) => x.SanadDet4)).WithOne((Expression<Func<Sanad_Det, Tafsil>>)((Sanad_Det x) => x.Tafsil4))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.TafsilGuid4))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<Sanad_Det>((Expression<Func<DecimalEntity, IEnumerable<Sanad_Det>>>)((DecimalEntity x) => x.SanadDet)).WithOne((Expression<Func<Sanad_Det, DecimalEntity>>)((Sanad_Det x) => x.Decimal))
				.HasForeignKey((Expression<Func<Sanad_Det, object>>)((Sanad_Det x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanadDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadDetailModelContext.cs", 28);
		}
	}

	public void OnModelCreatingSanadPrint(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<SanadPrint>().HasKey((Expression<Func<SanadPrint, object>>)((SanadPrint d) => d.Guid)).IsCluster(isCluster: false);
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<SanadPrint>(modelBuilder.Entity<SanadPrint>().HasIndex((Expression<Func<SanadPrint, object>>)((SanadPrint p) => new { p.AsGuid, p.AsType })), "IX_Acc_SanadPrint_AsGuid_AsType").IsUnique(true)).IsCluster();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanadPrint", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadPrintModelContext.cs", 21);
		}
	}

	public void OnModelCreatingSanad(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Sanads>().Property<int>((Expression<Func<Sanads, int>>)((Sanads p) => p.Id)).ValueGeneratedOnAdd();
			MutablePropertyExtensions.SetAfterSaveBehavior(((PropertyBuilder)modelBuilder.Entity<Sanads>().Property<int>((Expression<Func<Sanads, int>>)((Sanads p) => p.Id))).Metadata, (PropertySaveBehavior?)(PropertySaveBehavior)1);
			modelBuilder.Entity<Sanads>().HasIndex((Expression<Func<Sanads, object>>)((Sanads q) => q.SanadNo)).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Sanads>((Expression<Func<Cashier, IEnumerable<Sanads>>>)((Cashier x) => x.SanadItems)).WithOne((Expression<Func<Sanads, Cashier>>)((Sanads x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Sanads, object>>)((Sanads x) => x.UserGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Sanad\\SanadModelContext.cs", 28);
		}
	}

	public void OnModelCreatingCallCenter(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalIndexBuilderExtensions.HasName<CallingLine>(modelBuilder.Entity<CallingLine>().HasIndex((Expression<Func<CallingLine, object>>)((CallingLine p) => new { p.DeviceNumber, p.LineNumber })), "IX_Acc_CallingLine_DeviceNo_LineNo").IsUnique(true);
			modelBuilder.Entity<CallingLog>().HasKey((Expression<Func<CallingLog, object>>)((CallingLog d) => d.Uuid)).IsCluster(isCluster: false);
			RelationalIndexBuilderExtensions.HasName<CallingLog>(modelBuilder.Entity<CallingLog>().HasIndex((Expression<Func<CallingLog, object>>)((CallingLog d) => d.CalcedNumber)), "IX_Acc_CallingLog_CalcedNumber");
			RelationalIndexBuilderExtensions.HasName(((IndexBuilder)(object)modelBuilder.Entity<CallingLog>().HasIndex((Expression<Func<CallingLog, object>>)((CallingLog p) => p.DateM))).IsCluster(), "IX_Acc_Calling_DateM");
			RelationalIndexBuilderExtensions.HasFilter<CallingLog>(modelBuilder.Entity<CallingLog>().HasIndex((Expression<Func<CallingLog, object>>)((CallingLog p) => p.Uuid)).IsUnique(true), "Uuid IS NOT NULL");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<CallingLog>().Property<DateTime?>((Expression<Func<CallingLog, DateTime?>>)((CallingLog p) => p.DateUtc)), "getdate()");
			SqlServerKeyBuilderExtensions.IsClustered(modelBuilder.Entity<CallingLogDetail>().HasKey((Expression<Func<CallingLogDetail, object>>)((CallingLogDetail d) => d.Uuid)), true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<CallingLine>((Expression<Func<Cashier, IEnumerable<CallingLine>>>)((Cashier x) => x.CallingLines)).WithOne((Expression<Func<CallingLine, Cashier>>)((CallingLine x) => x.Cashier))
				.HasForeignKey((Expression<Func<CallingLine, object>>)((CallingLine x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CallingLog>().HasMany<CallingLogDetail>((Expression<Func<CallingLog, IEnumerable<CallingLogDetail>>>)((CallingLog x) => x.CallingLogDetail)).WithOne((Expression<Func<CallingLogDetail, CallingLog>>)((CallingLogDetail x) => x.Master))
				.HasForeignKey((Expression<Func<CallingLogDetail, object>>)((CallingLogDetail x) => x.LinkedId))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<CallingLogDetail>((Expression<Func<Cashier, IEnumerable<CallingLogDetail>>>)((Cashier x) => x.CallingLogDetail)).WithOne((Expression<Func<CallingLogDetail, Cashier>>)((CallingLogDetail x) => x.Cashier))
				.HasForeignKey((Expression<Func<CallingLogDetail, object>>)((CallingLogDetail x) => x.CashierGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCallCenter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\CallCenterModelContext.cs", 38);
		}
	}

	public void OnModelCreatingLottery(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Lottery>((Expression<Func<Cashier, IEnumerable<Lottery>>>)((Cashier x) => x.Lotteries)).WithOne((Expression<Func<Lottery, Cashier>>)((Lottery x) => x.Cashier))
				.HasForeignKey((Expression<Func<Lottery, object>>)((Lottery x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<LotteryScore>((Expression<Func<Cashier, IEnumerable<LotteryScore>>>)((Cashier x) => x.LotteryScores)).WithOne((Expression<Func<LotteryScore, Cashier>>)((LotteryScore x) => x.Cashier))
				.HasForeignKey((Expression<Func<LotteryScore, object>>)((LotteryScore x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Lottery>((Expression<Func<Tafsil, IEnumerable<Lottery>>>)((Tafsil x) => x.Lotteries)).WithOne((Expression<Func<Lottery, Tafsil>>)((Lottery x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Lottery, object>>)((Lottery x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<LotteryScore>((Expression<Func<Tafsil, IEnumerable<LotteryScore>>>)((Tafsil x) => x.LotteryScores)).WithOne((Expression<Func<LotteryScore, Tafsil>>)((LotteryScore x) => x.Tafsil))
				.HasForeignKey((Expression<Func<LotteryScore, object>>)((LotteryScore x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<Lottery>((Expression<Func<Pardakht, IEnumerable<Lottery>>>)((Pardakht x) => x.Lotteries)).WithOne((Expression<Func<Lottery, Pardakht>>)((Lottery x) => x.Pardakht))
				.HasForeignKey((Expression<Func<Lottery, object>>)((Lottery x) => x.PardakhtGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<LotteryScore>((Expression<Func<Order, IEnumerable<LotteryScore>>>)((Order x) => x.LotteryScores)).WithOne((Expression<Func<LotteryScore, Order>>)((LotteryScore x) => x.Order))
				.HasForeignKey((Expression<Func<LotteryScore, object>>)((LotteryScore x) => x.OrderGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\LotteryModelContext.cs", 29);
		}
	}

	public void OnModelCreatingProductSelection(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<PrdSelected>((Expression<Func<Cashier, IEnumerable<PrdSelected>>>)((Cashier x) => x.PrdSelecteds)).WithOne((Expression<Func<PrdSelected, Cashier>>)((PrdSelected x) => x.CashierItem))
				.HasForeignKey((Expression<Func<PrdSelected, object>>)((PrdSelected x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<PrdSelected>().HasMany<Prd_SelectedDetails>((Expression<Func<PrdSelected, IEnumerable<Prd_SelectedDetails>>>)((PrdSelected x) => x.Details)).WithOne((Expression<Func<Prd_SelectedDetails, PrdSelected>>)((Prd_SelectedDetails x) => x.Master))
				.HasForeignKey((Expression<Func<Prd_SelectedDetails, object>>)((Prd_SelectedDetails x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductSelection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Selection\\ProductSelectionModelContext.cs", 22);
		}
	}

	public void OnModelCreatingTafsilSelection(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<TafsilSelected>((Expression<Func<Cashier, IEnumerable<TafsilSelected>>>)((Cashier x) => x.CusSelecteds)).WithOne((Expression<Func<TafsilSelected, Cashier>>)((TafsilSelected x) => x.CashierItem))
				.HasForeignKey((Expression<Func<TafsilSelected, object>>)((TafsilSelected x) => x.UGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TafsilSelected>().HasMany<TafsilSelectedDetails>((Expression<Func<TafsilSelected, IEnumerable<TafsilSelectedDetails>>>)((TafsilSelected x) => x.TafsilSelectedDetails)).WithOne((Expression<Func<TafsilSelectedDetails, TafsilSelected>>)((TafsilSelectedDetails x) => x.TafsilSelected))
				.HasForeignKey((Expression<Func<TafsilSelectedDetails, object>>)((TafsilSelectedDetails x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTafsilSelection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Selection\\TafsilSelectionModelContext.cs", 22);
		}
	}

	public void OnModelCreatingTolidFormulaExport(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TolidFormula>().HasMany<TolidFormulaExport>((Expression<Func<TolidFormula, IEnumerable<TolidFormulaExport>>>)((TolidFormula x) => x.TolidFormulaExport)).WithOne((Expression<Func<TolidFormulaExport, TolidFormula>>)((TolidFormulaExport x) => x.Master))
				.HasForeignKey((Expression<Func<TolidFormulaExport, object>>)((TolidFormulaExport x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<TolidFormulaExport>((Expression<Func<Product, IEnumerable<TolidFormulaExport>>>)((Product x) => x.TolidFormulaExport)).WithOne((Expression<Func<TolidFormulaExport, Product>>)((TolidFormulaExport x) => x.Product))
				.HasForeignKey((Expression<Func<TolidFormulaExport, object>>)((TolidFormulaExport x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<TolidFormulaExport>((Expression<Func<Depot, IEnumerable<TolidFormulaExport>>>)((Depot x) => x.TolidFormulaExport)).WithOne((Expression<Func<TolidFormulaExport, Depot>>)((TolidFormulaExport x) => x.Depot))
				.HasForeignKey((Expression<Func<TolidFormulaExport, object>>)((TolidFormulaExport x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<TolidFormulaExport>((Expression<Func<ProductVahed, IEnumerable<TolidFormulaExport>>>)((ProductVahed x) => x.TolidFormulaExport)).WithOne((Expression<Func<TolidFormulaExport, ProductVahed>>)((TolidFormulaExport x) => x.Vahed))
				.HasForeignKey((Expression<Func<TolidFormulaExport, object>>)((TolidFormulaExport x) => x.V2Guid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidFormulaExport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidFormula\\TolidFormulaExportModelContext.cs", 26);
		}
	}

	public void OnModelCreatingTolidFormulaHazine(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TolidFormula>().HasMany<TolidFormulaHazine>((Expression<Func<TolidFormula, IEnumerable<TolidFormulaHazine>>>)((TolidFormula x) => x.TolidFormulaHaizne)).WithOne((Expression<Func<TolidFormulaHazine, TolidFormula>>)((TolidFormulaHazine x) => x.Master))
				.HasForeignKey((Expression<Func<TolidFormulaHazine, object>>)((TolidFormulaHazine x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<TolidFormulaHazine>((Expression<Func<Tafsil, IEnumerable<TolidFormulaHazine>>>)((Tafsil x) => x.TolidFormulaHaizne)).WithOne((Expression<Func<TolidFormulaHazine, Tafsil>>)((TolidFormulaHazine x) => x.Tafsil))
				.HasForeignKey((Expression<Func<TolidFormulaHazine, object>>)((TolidFormulaHazine x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<TolidFormulaHazine>((Expression<Func<Moein, IEnumerable<TolidFormulaHazine>>>)((Moein x) => x.TolidFormulaHaizne)).WithOne((Expression<Func<TolidFormulaHazine, Moein>>)((TolidFormulaHazine x) => x.Moein))
				.HasForeignKey((Expression<Func<TolidFormulaHazine, object>>)((TolidFormulaHazine x) => x.MoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidFormulaHazine", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidFormula\\TolidFormulaHazineModelContext.cs", 25);
		}
	}

	public void OnModelCreatingTolidFormulaMavad(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TolidFormula>().HasMany<TolidFormulaMavad>((Expression<Func<TolidFormula, IEnumerable<TolidFormulaMavad>>>)((TolidFormula x) => x.TolidFormulaMavad)).WithOne((Expression<Func<TolidFormulaMavad, TolidFormula>>)((TolidFormulaMavad x) => x.Master))
				.HasForeignKey((Expression<Func<TolidFormulaMavad, object>>)((TolidFormulaMavad x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<TolidFormulaMavad>((Expression<Func<Product, IEnumerable<TolidFormulaMavad>>>)((Product x) => x.TolidFormulaMavad)).WithOne((Expression<Func<TolidFormulaMavad, Product>>)((TolidFormulaMavad x) => x.Product))
				.HasForeignKey((Expression<Func<TolidFormulaMavad, object>>)((TolidFormulaMavad x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<TolidFormulaMavad>((Expression<Func<Depot, IEnumerable<TolidFormulaMavad>>>)((Depot x) => x.TolidFormulaMavad)).WithOne((Expression<Func<TolidFormulaMavad, Depot>>)((TolidFormulaMavad x) => x.Depot))
				.HasForeignKey((Expression<Func<TolidFormulaMavad, object>>)((TolidFormulaMavad x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<TolidFormulaMavad>((Expression<Func<ProductVahed, IEnumerable<TolidFormulaMavad>>>)((ProductVahed x) => x.TolidFormulaMavad)).WithOne((Expression<Func<TolidFormulaMavad, ProductVahed>>)((TolidFormulaMavad x) => x.Vahed))
				.HasForeignKey((Expression<Func<TolidFormulaMavad, object>>)((TolidFormulaMavad x) => x.V2Guid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidFormulaMavad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidFormula\\TolidFormulaMavadModelContext.cs", 26);
		}
	}

	public void OnModelCreatingTolidExport(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tolid>().HasMany<TolidExport>((Expression<Func<Tolid, IEnumerable<TolidExport>>>)((Tolid x) => x.TolidExport)).WithOne((Expression<Func<TolidExport, Tolid>>)((TolidExport x) => x.Master))
				.HasForeignKey((Expression<Func<TolidExport, object>>)((TolidExport x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<TolidExport>((Expression<Func<Product, IEnumerable<TolidExport>>>)((Product x) => x.TolidExport)).WithOne((Expression<Func<TolidExport, Product>>)((TolidExport x) => x.Product))
				.HasForeignKey((Expression<Func<TolidExport, object>>)((TolidExport x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<TolidExport>((Expression<Func<Depot, IEnumerable<TolidExport>>>)((Depot x) => x.TolidExport)).WithOne((Expression<Func<TolidExport, Depot>>)((TolidExport x) => x.Depot))
				.HasForeignKey((Expression<Func<TolidExport, object>>)((TolidExport x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<TolidExport>((Expression<Func<ProductVahed, IEnumerable<TolidExport>>>)((ProductVahed x) => x.TolidExport)).WithOne((Expression<Func<TolidExport, ProductVahed>>)((TolidExport x) => x.Vahed))
				.HasForeignKey((Expression<Func<TolidExport, object>>)((TolidExport x) => x.V2Guid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidExport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidExportModelContext.cs", 26);
		}
	}

	public void OnModelCreatingTolidHazine(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tolid>().HasMany<TolidHazine>((Expression<Func<Tolid, IEnumerable<TolidHazine>>>)((Tolid x) => x.TolidHazine)).WithOne((Expression<Func<TolidHazine, Tolid>>)((TolidHazine x) => x.Master))
				.HasForeignKey((Expression<Func<TolidHazine, object>>)((TolidHazine x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<TolidHazine>((Expression<Func<Tafsil, IEnumerable<TolidHazine>>>)((Tafsil x) => x.TolidHazine)).WithOne((Expression<Func<TolidHazine, Tafsil>>)((TolidHazine x) => x.Tafsil))
				.HasForeignKey((Expression<Func<TolidHazine, object>>)((TolidHazine x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<TolidHazine>((Expression<Func<Moein, IEnumerable<TolidHazine>>>)((Moein x) => x.TolidHazine)).WithOne((Expression<Func<TolidHazine, Moein>>)((TolidHazine x) => x.Moein))
				.HasForeignKey((Expression<Func<TolidHazine, object>>)((TolidHazine x) => x.MoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidHazine", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidHazineModelContext.cs", 23);
		}
	}

	public void OnModelCreatingTolidMavad(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tolid>().HasMany<TolidMavad>((Expression<Func<Tolid, IEnumerable<TolidMavad>>>)((Tolid x) => x.TolidMavad)).WithOne((Expression<Func<TolidMavad, Tolid>>)((TolidMavad x) => x.Master))
				.HasForeignKey((Expression<Func<TolidMavad, object>>)((TolidMavad x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<TolidMavad>((Expression<Func<Depot, IEnumerable<TolidMavad>>>)((Depot x) => x.TolidMavad)).WithOne((Expression<Func<TolidMavad, Depot>>)((TolidMavad x) => x.Depot))
				.HasForeignKey((Expression<Func<TolidMavad, object>>)((TolidMavad x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<TolidMavad>((Expression<Func<ProductVahed, IEnumerable<TolidMavad>>>)((ProductVahed x) => x.TolidMavad)).WithOne((Expression<Func<TolidMavad, ProductVahed>>)((TolidMavad x) => x.Vahed))
				.HasForeignKey((Expression<Func<TolidMavad, object>>)((TolidMavad x) => x.V2Guid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<TolidMavad>((Expression<Func<Product, IEnumerable<TolidMavad>>>)((Product x) => x.TolidMavad)).WithOne((Expression<Func<TolidMavad, Product>>)((TolidMavad x) => x.Product))
				.HasForeignKey((Expression<Func<TolidMavad, object>>)((TolidMavad x) => x.ProductGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidMavad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidMavadModelContext.cs", 27);
		}
	}

	public void OnModelCreatingTolidSummary(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Cashier>().HasMany<TolidSummary>((Expression<Func<Cashier, IEnumerable<TolidSummary>>>)((Cashier x) => x.TolidSummary)).WithOne((Expression<Func<TolidSummary, Cashier>>)((TolidSummary x) => x.Cashier))
				.HasForeignKey((Expression<Func<TolidSummary, object>>)((TolidSummary x) => x.CashierGuid))
				.OnDelete((DeleteBehavior)5);
			modelBuilder.Entity<TolidSummary>().HasMany<TolidSummaryDetail>((Expression<Func<TolidSummary, IEnumerable<TolidSummaryDetail>>>)((TolidSummary x) => x.TolidSummaryDetail)).WithOne((Expression<Func<TolidSummaryDetail, TolidSummary>>)((TolidSummaryDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<TolidSummaryDetail, object>>)((TolidSummaryDetail x) => x.MasterGuid))
				.OnDelete((DeleteBehavior)5);
			modelBuilder.Entity<Product>().HasMany<TolidSummaryDetail>((Expression<Func<Product, IEnumerable<TolidSummaryDetail>>>)((Product x) => x.TolidSummaryDetail)).WithOne((Expression<Func<TolidSummaryDetail, Product>>)((TolidSummaryDetail x) => x.ProductItem))
				.HasForeignKey((Expression<Func<TolidSummaryDetail, object>>)((TolidSummaryDetail x) => x.ProductGuid))
				.OnDelete((DeleteBehavior)5);
			modelBuilder.Entity<Depot>().HasMany<TolidSummaryDetail>((Expression<Func<Depot, IEnumerable<TolidSummaryDetail>>>)((Depot x) => x.TolidSummaryDetail)).WithOne((Expression<Func<TolidSummaryDetail, Depot>>)((TolidSummaryDetail x) => x.DepotItem))
				.HasForeignKey((Expression<Func<TolidSummaryDetail, object>>)((TolidSummaryDetail x) => x.DepotGuid))
				.OnDelete((DeleteBehavior)5);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolidSummary", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidSummaryModelContext.cs", 27);
		}
	}

	public void OnModelCreatingTolid(ModelBuilder modelBuilder)
	{
		try
		{
			RelationalPropertyBuilderExtensions.HasDefaultValue<bool>(RelationalPropertyBuilderExtensions.HasColumnType<bool>(modelBuilder.Entity<Tolid>().Property<bool>((Expression<Func<Tolid, bool>>)((Tolid p) => p.KardexAffective)), "bit"), (object)true).ValueGeneratedNever();
			RelationalPropertyBuilderExtensions.HasDefaultValue<bool>(RelationalPropertyBuilderExtensions.HasColumnType<bool>(modelBuilder.Entity<Tolid>().Property<bool>((Expression<Func<Tolid, bool>>)((Tolid p) => p.SanadAffective)), "bit"), (object)true).ValueGeneratedNever();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Tolid>((Expression<Func<Cashier, IEnumerable<Tolid>>>)((Cashier x) => x.Tolid)).WithOne((Expression<Func<Tolid, Cashier>>)((Tolid x) => x.Cashier))
				.HasForeignKey((Expression<Func<Tolid, object>>)((Tolid x) => x.CashierGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Tolid>((Expression<Func<Moein, IEnumerable<Tolid>>>)((Moein x) => x.Mavad)).WithOne((Expression<Func<Tolid, Moein>>)((Tolid x) => x.MavadMoein))
				.HasForeignKey((Expression<Func<Tolid, object>>)((Tolid x) => x.MavadMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Tolid>((Expression<Func<Moein, IEnumerable<Tolid>>>)((Moein x) => x.Mahsol)).WithOne((Expression<Func<Tolid, Moein>>)((Tolid x) => x.MahsolMoein))
				.HasForeignKey((Expression<Func<Tolid, object>>)((Tolid x) => x.MahsolMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Tolid>((Expression<Func<Tafsil, IEnumerable<Tolid>>>)((Tafsil x) => x.Mavad)).WithOne((Expression<Func<Tolid, Tafsil>>)((Tolid x) => x.MavadTafsil))
				.HasForeignKey((Expression<Func<Tolid, object>>)((Tolid x) => x.MavadTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Tolid>((Expression<Func<Tafsil, IEnumerable<Tolid>>>)((Tafsil x) => x.Mahsol)).WithOne((Expression<Func<Tolid, Tafsil>>)((Tolid x) => x.MahsolTafsil))
				.HasForeignKey((Expression<Func<Tolid, object>>)((Tolid x) => x.MahsolTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTolid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Tolid\\TolidModelContext.cs", 27);
		}
	}

	public void OnModelCreatingRasGiri(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<RasGiri>().HasMany<RasGiriDet>((Expression<Func<RasGiri, IEnumerable<RasGiriDet>>>)((RasGiri x) => x.Details)).WithOne((Expression<Func<RasGiriDet, RasGiri>>)((RasGiriDet x) => x.Master))
				.HasForeignKey((Expression<Func<RasGiriDet, object>>)((RasGiriDet x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingRasGiri", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\RasGiriModelContext.cs", 21);
		}
	}

	public void OnModelCreatingVam(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<Vam>((Expression<Func<Pardakht, IEnumerable<Vam>>>)((Pardakht x) => x.Vam)).WithOne((Expression<Func<Vam, Pardakht>>)((Vam x) => x.Pardakht))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.PardakhtGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Vam>((Expression<Func<Moein, IEnumerable<Vam>>>)((Moein x) => x.VamGirandeMoein)).WithOne((Expression<Func<Vam, Moein>>)((Vam x) => x.VamGirandeMoein))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.VamMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Vam>((Expression<Func<Moein, IEnumerable<Vam>>>)((Moein x) => x.VamSepordeMoein)).WithOne((Expression<Func<Vam, Moein>>)((Vam x) => x.VamSepordeMoein))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.SepordeMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamGirandeTafsil)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.VamGirandeTafsil))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.VamTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamSepordeTafsil)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.VamSepordeTafsil))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.SepordeTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamZamen1)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.Zamen1))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.ZamenGuid1))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamZamen2)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.Zamen2))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.ZamenGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamZamen3)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.Zamen3))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.ZamenGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Vam>((Expression<Func<Tafsil, IEnumerable<Vam>>>)((Tafsil x) => x.VamZamen4)).WithOne((Expression<Func<Vam, Tafsil>>)((Vam x) => x.Zamen4))
				.HasForeignKey((Expression<Func<Vam, object>>)((Vam x) => x.ZamenGuid4))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingVam", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\VamModelContext.cs", 31);
		}
	}

	public void OnModelCreatingTtms(ModelBuilder modelBuilder)
	{
		try
		{
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TTMS>().HasMany<TTMSDetail>((Expression<Func<TTMS, IEnumerable<TTMSDetail>>>)((TTMS x) => x.DetailList)).WithOne((Expression<Func<TTMSDetail, TTMS>>)((TTMSDetail x) => x.MasterItem))
				.HasForeignKey((Expression<Func<TTMSDetail, object>>)((TTMSDetail x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingTtms", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\TtmsModelContext.cs", 21);
		}
	}

	public void OnModelCreatingVisit(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Visit>().HasIndex((Expression<Func<Visit, object>>)((Visit p) => p.ServerStatus));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingVisit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\VisitModelContext.cs", 19);
		}
	}

	public void OnModelCreatingPardakhtChequeSh(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PardakhtCheckSh>().HasIndex((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh p) => p.MasterGuid));
			modelBuilder.Entity<PardakhtCheckSh>((Action<EntityTypeBuilder<PardakhtCheckSh>>)delegate(EntityTypeBuilder<PardakhtCheckSh> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(q.Property<string>((Expression<Func<PardakhtCheckSh, string>>)((PardakhtCheckSh r) => r.SayadiName)), "((''))");
			});
			modelBuilder.Entity<PardakhtCheckSh>((Action<EntityTypeBuilder<PardakhtCheckSh>>)delegate(EntityTypeBuilder<PardakhtCheckSh> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(q.Property<string>((Expression<Func<PardakhtCheckSh, string>>)((PardakhtCheckSh r) => r.SayadiIdentity)), "((''))");
			});
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<PardakhtCheckSh>().Property<DateTime?>((Expression<Func<PardakhtCheckSh, DateTime?>>)((PardakhtCheckSh p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<PardakhtCheckSh>((Expression<Func<Pardakht, IEnumerable<PardakhtCheckSh>>>)((Pardakht x) => x.PardakhtCheckShItem)).WithOne((Expression<Func<PardakhtCheckSh, Pardakht>>)((PardakhtCheckSh x) => x.PardakhtItem))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtCheckSh>((Expression<Func<Tafsil, IEnumerable<PardakhtCheckSh>>>)((Tafsil x) => x.PardakhtCheckShBank)).WithOne((Expression<Func<PardakhtCheckSh, Tafsil>>)((PardakhtCheckSh x) => x.BankTafsilItem))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.BankTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtCheckSh>((Expression<Func<Tafsil, IEnumerable<PardakhtCheckSh>>>)((Tafsil x) => x.PardakhtCheckShCredit)).WithOne((Expression<Func<PardakhtCheckSh, Tafsil>>)((PardakhtCheckSh x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.CreditTafsilGuid))).NotRequiredInSqlite().OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtCheckSh>((Expression<Func<Moein, IEnumerable<PardakhtCheckSh>>>)((Moein x) => x.PardakhtCheckShCredit)).WithOne((Expression<Func<PardakhtCheckSh, Moein>>)((PardakhtCheckSh x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.CreditMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<PardakhtCheckSh>((Expression<Func<Cashier, IEnumerable<PardakhtCheckSh>>>)((Cashier x) => x.CashierPardakhtCheckSh)).WithOne((Expression<Func<PardakhtCheckSh, Cashier>>)((PardakhtCheckSh x) => x.CashierItem))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtCheckSh>((Expression<Func<Tafsil, IEnumerable<PardakhtCheckSh>>>)((Tafsil x) => x.PardakhtCheckShA2re)).WithOne((Expression<Func<PardakhtCheckSh, Tafsil>>)((PardakhtCheckSh x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.A2re_TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtCheckSh>((Expression<Func<Moein, IEnumerable<PardakhtCheckSh>>>)((Moein x) => x.PardakhtCheckShA2re)).WithOne((Expression<Func<PardakhtCheckSh, Moein>>)((PardakhtCheckSh x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.A2re_MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtCheckSh>((Expression<Func<Tafsil, IEnumerable<PardakhtCheckSh>>>)((Tafsil x) => x.PardakhtCheckShDebitA2re)).WithOne((Expression<Func<PardakhtCheckSh, Tafsil>>)((PardakhtCheckSh x) => x.DebitA2reTafsil))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.DebitA2reTafsiGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtCheckSh>((Expression<Func<Moein, IEnumerable<PardakhtCheckSh>>>)((Moein x) => x.PardakhtCheckShDebitA2re)).WithOne((Expression<Func<PardakhtCheckSh, Moein>>)((PardakhtCheckSh x) => x.DebitA2reMoein))
				.HasForeignKey((Expression<Func<PardakhtCheckSh, object>>)((PardakhtCheckSh x) => x.DebitA2reMoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakhtChequeSh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtChequeShModelContext.cs", 39);
		}
	}

	public void OnModelCreatingPardakhtChequeM(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PardakhtCheckM>().HasIndex((Expression<Func<PardakhtCheckM, object>>)((PardakhtCheckM p) => p.MasterGuid));
			modelBuilder.Entity<PardakhtCheckM>((Action<EntityTypeBuilder<PardakhtCheckM>>)delegate(EntityTypeBuilder<PardakhtCheckM> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(q.Property<string>((Expression<Func<PardakhtCheckM, string>>)((PardakhtCheckM r) => r.SayadiName)), "((''))");
			});
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<PardakhtCheckM>((Expression<Func<Pardakht, IEnumerable<PardakhtCheckM>>>)((Pardakht x) => x.PardakhtCheckMItem)).WithOne((Expression<Func<PardakhtCheckM, Pardakht>>)((PardakhtCheckM x) => x.PardakhtItem))
				.HasForeignKey((Expression<Func<PardakhtCheckM, object>>)((PardakhtCheckM x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ReceptionCheck>().HasMany<PardakhtCheckM>((Expression<Func<ReceptionCheck, IEnumerable<PardakhtCheckM>>>)((ReceptionCheck x) => x.PardakhtCheckMItem)).WithOne((Expression<Func<PardakhtCheckM, ReceptionCheck>>)((PardakhtCheckM x) => x.CheckItem))
				.HasForeignKey((Expression<Func<PardakhtCheckM, object>>)((PardakhtCheckM x) => x.GuidCheck))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtCheckM>((Expression<Func<Tafsil, IEnumerable<PardakhtCheckM>>>)((Tafsil x) => x.PardakhtCheckMCredit)).WithOne((Expression<Func<PardakhtCheckM, Tafsil>>)((PardakhtCheckM x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<PardakhtCheckM, object>>)((PardakhtCheckM x) => x.CreditTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtCheckM>((Expression<Func<Moein, IEnumerable<PardakhtCheckM>>>)((Moein x) => x.PardakhtCheckMCredit)).WithOne((Expression<Func<PardakhtCheckM, Moein>>)((PardakhtCheckM x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<PardakhtCheckM, object>>)((PardakhtCheckM x) => x.CreditMoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakhtChequeM", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtChequeMModelContext.cs", 28);
		}
	}

	public void OnModelCreatingPardakhtNaqd(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PardakhtNaghd>().HasIndex((Expression<Func<PardakhtNaghd, object>>)((PardakhtNaghd p) => p.MasterGuid));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<PardakhtNaghd>().Property<DateTime?>((Expression<Func<PardakhtNaghd, DateTime?>>)((PardakhtNaghd p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<PardakhtNaghd>((Expression<Func<Pardakht, IEnumerable<PardakhtNaghd>>>)((Pardakht x) => x.PardakhtNaghdItem)).WithOne((Expression<Func<PardakhtNaghd, Pardakht>>)((PardakhtNaghd x) => x.PardakhtItem))
				.HasForeignKey((Expression<Func<PardakhtNaghd, object>>)((PardakhtNaghd x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtNaghd>((Expression<Func<Tafsil, IEnumerable<PardakhtNaghd>>>)((Tafsil x) => x.PardakhtNaghd)).WithOne((Expression<Func<PardakhtNaghd, Tafsil>>)((PardakhtNaghd x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<PardakhtNaghd, object>>)((PardakhtNaghd x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtNaghd>((Expression<Func<Moein, IEnumerable<PardakhtNaghd>>>)((Moein x) => x.PardakhtNaghd)).WithOne((Expression<Func<PardakhtNaghd, Moein>>)((PardakhtNaghd x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<PardakhtNaghd, object>>)((PardakhtNaghd x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<PardakhtNaghd>((Expression<Func<DecimalEntity, IEnumerable<PardakhtNaghd>>>)((DecimalEntity x) => x.PardakhtNaghdItem)).WithOne((Expression<Func<PardakhtNaghd, DecimalEntity>>)((PardakhtNaghd x) => x.DeciamlItem))
				.HasForeignKey((Expression<Func<PardakhtNaghd, object>>)((PardakhtNaghd x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakhtNaqd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtNaqdModelContext.cs", 28);
		}
	}

	public void OnModelCreatingPardakhtHavale(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<PardakhtBank>().HasIndex((Expression<Func<PardakhtBank, object>>)((PardakhtBank p) => p.MasterGuid));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<PardakhtBank>().Property<DateTime?>((Expression<Func<PardakhtBank, DateTime?>>)((PardakhtBank p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtBank>((Expression<Func<Moein, IEnumerable<PardakhtBank>>>)((Moein x) => x.PardakhtBank)).WithOne((Expression<Func<PardakhtBank, Moein>>)((PardakhtBank x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<PardakhtBank>((Expression<Func<Moein, IEnumerable<PardakhtBank>>>)((Moein x) => x.PardakhtBankWage)).WithOne((Expression<Func<PardakhtBank, Moein>>)((PardakhtBank x) => x.WageMoeinItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.WageMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtBank>((Expression<Func<Tafsil, IEnumerable<PardakhtBank>>>)((Tafsil x) => x.PardakhtBankWage)).WithOne((Expression<Func<PardakhtBank, Tafsil>>)((PardakhtBank x) => x.WageTafsilItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.WageTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<PardakhtBank>((Expression<Func<Tafsil, IEnumerable<PardakhtBank>>>)((Tafsil x) => x.PardakhtBank)).WithOne((Expression<Func<PardakhtBank, Tafsil>>)((PardakhtBank x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<PardakhtBank>((Expression<Func<DecimalEntity, IEnumerable<PardakhtBank>>>)((DecimalEntity x) => x.PardakhtBankItem)).WithOne((Expression<Func<PardakhtBank, DecimalEntity>>)((PardakhtBank x) => x.DeciamlItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.DecimalGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Pardakht>().HasMany<PardakhtBank>((Expression<Func<Pardakht, IEnumerable<PardakhtBank>>>)((Pardakht x) => x.PardakhtBankItem)).WithOne((Expression<Func<PardakhtBank, Pardakht>>)((PardakhtBank x) => x.PardakhtItem))
				.HasForeignKey((Expression<Func<PardakhtBank, object>>)((PardakhtBank x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingPardakhtHavale", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Pardakht\\PardakhtHavaleModelContext.cs", 34);
		}
	}

	public void OnModelCreatingReceptionNaqd(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ReceptionNaghd>().HasIndex((Expression<Func<ReceptionNaghd, object>>)((ReceptionNaghd p) => p.MasterGuid));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ReceptionNaghd>().Property<DateTime?>((Expression<Func<ReceptionNaghd, DateTime?>>)((ReceptionNaghd p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<ReceptionNaghd>((Expression<Func<Reception, IEnumerable<ReceptionNaghd>>>)((Reception x) => x.ReceptionNaghdItem)).WithOne((Expression<Func<ReceptionNaghd, Reception>>)((ReceptionNaghd x) => x.ReceptionItem))
				.HasForeignKey((Expression<Func<ReceptionNaghd, object>>)((ReceptionNaghd x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionNaghd>((Expression<Func<Tafsil, IEnumerable<ReceptionNaghd>>>)((Tafsil x) => x.ReceptionNaghd)).WithOne((Expression<Func<ReceptionNaghd, Tafsil>>)((ReceptionNaghd x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<ReceptionNaghd, object>>)((ReceptionNaghd x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionNaghd>((Expression<Func<Moein, IEnumerable<ReceptionNaghd>>>)((Moein x) => x.ReceptionNaghd)).WithOne((Expression<Func<ReceptionNaghd, Moein>>)((ReceptionNaghd x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<ReceptionNaghd, object>>)((ReceptionNaghd x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<ReceptionNaghd>((Expression<Func<DecimalEntity, IEnumerable<ReceptionNaghd>>>)((DecimalEntity x) => x.ReceptionNaghdItem)).WithOne((Expression<Func<ReceptionNaghd, DecimalEntity>>)((ReceptionNaghd x) => x.DeciamlItem))
				.HasForeignKey((Expression<Func<ReceptionNaghd, object>>)((ReceptionNaghd x) => x.DecimalGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReceptionNaqd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ReceptionNaqdModelContext.cs", 28);
		}
	}

	public void OnModelCreatingReceptionHavale(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ReceptionHavale>().HasIndex((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale p) => p.MasterGuid));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ReceptionHavale>().Property<DateTime?>((Expression<Func<ReceptionHavale, DateTime?>>)((ReceptionHavale p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<ReceptionHavale>((Expression<Func<Reception, IEnumerable<ReceptionHavale>>>)((Reception x) => x.ReceptionHavaleItem)).WithOne((Expression<Func<ReceptionHavale, Reception>>)((ReceptionHavale x) => x.ReceptionItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<ReceptionHavale>((Expression<Func<DecimalEntity, IEnumerable<ReceptionHavale>>>)((DecimalEntity x) => x.ReceptionHavaleItem)).WithOne((Expression<Func<ReceptionHavale, DecimalEntity>>)((ReceptionHavale x) => x.DeciamlItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.DecimalGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionHavale>((Expression<Func<Moein, IEnumerable<ReceptionHavale>>>)((Moein x) => x.ReceptionHavaleWage)).WithOne((Expression<Func<ReceptionHavale, Moein>>)((ReceptionHavale x) => x.WageMoeinItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.WageMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionHavale>((Expression<Func<Moein, IEnumerable<ReceptionHavale>>>)((Moein x) => x.ReceptionHavale)).WithOne((Expression<Func<ReceptionHavale, Moein>>)((ReceptionHavale x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionHavale>((Expression<Func<Tafsil, IEnumerable<ReceptionHavale>>>)((Tafsil x) => x.ReceptionHavale)).WithOne((Expression<Func<ReceptionHavale, Tafsil>>)((ReceptionHavale x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionHavale>((Expression<Func<Tafsil, IEnumerable<ReceptionHavale>>>)((Tafsil x) => x.ReceptionHavaleWage)).WithOne((Expression<Func<ReceptionHavale, Tafsil>>)((ReceptionHavale x) => x.WageTafsilItem))
				.HasForeignKey((Expression<Func<ReceptionHavale, object>>)((ReceptionHavale x) => x.WageTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReceptionHavale", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ReceptionHavaleModelContext.cs", 30);
		}
	}

	public void OnModelCreatingReceptionCheque(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ReceptionCheck>().HasIndex((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck p) => p.MasterGuid));
			((IndexBuilder)(object)RelationalIndexBuilderExtensions.HasName<ReceptionCheck>(modelBuilder.Entity<ReceptionCheck>().HasIndex((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck p) => new { p.Status, p.Aval2re, p.A2re_PardazandeTafsilGuid })), "IX_Acc_ReceptionCheque_Status_Aval2re_PardazandeTafsilGuid")).IsCluster(isCluster: false);
			modelBuilder.Entity<ReceptionCheck>((Action<EntityTypeBuilder<ReceptionCheck>>)delegate(EntityTypeBuilder<ReceptionCheck> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(q.Property<string>((Expression<Func<ReceptionCheck, string>>)((ReceptionCheck r) => r.SayadiName)), "((''))");
			});
			modelBuilder.Entity<ReceptionCheck>((Action<EntityTypeBuilder<ReceptionCheck>>)delegate(EntityTypeBuilder<ReceptionCheck> q)
			{
				RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(q.Property<string>((Expression<Func<ReceptionCheck, string>>)((ReceptionCheck r) => r.SayadiIdentity)), "((''))");
			});
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<ReceptionCheck>().Property<DateTime?>((Expression<Func<ReceptionCheck, DateTime?>>)((ReceptionCheck p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Reception>().HasMany<ReceptionCheck>((Expression<Func<Reception, IEnumerable<ReceptionCheck>>>)((Reception x) => x.ReceptionCheckMItem)).WithOne((Expression<Func<ReceptionCheck, Reception>>)((ReceptionCheck x) => x.ReceptionItem))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<ReceptionCheck>((Expression<Func<Cashier, IEnumerable<ReceptionCheck>>>)((Cashier x) => x.CashierReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Cashier>>)((ReceptionCheck x) => x.CashierItem))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionCheck>((Expression<Func<Moein, IEnumerable<ReceptionCheck>>>)((Moein x) => x.ReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Moein>>)((ReceptionCheck x) => x.A2re_MoeinItem))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.A2re_PardazandeMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionCheck>((Expression<Func<Moein, IEnumerable<ReceptionCheck>>>)((Moein x) => x.DebitReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Moein>>)((ReceptionCheck x) => x.DebitMoein))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.DebitMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<ReceptionCheck>((Expression<Func<Moein, IEnumerable<ReceptionCheck>>>)((Moein x) => x.CreditReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Moein>>)((ReceptionCheck x) => x.CreditMoein))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.CreditA2reMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionCheck>((Expression<Func<Tafsil, IEnumerable<ReceptionCheck>>>)((Tafsil x) => x.ReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Tafsil>>)((ReceptionCheck x) => x.A2re_TafsilItem))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.A2re_PardazandeTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionCheck>((Expression<Func<Tafsil, IEnumerable<ReceptionCheck>>>)((Tafsil x) => x.DebitReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Tafsil>>)((ReceptionCheck x) => x.DebitTafsil))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.DebitTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionCheck>((Expression<Func<Tafsil, IEnumerable<ReceptionCheck>>>)((Tafsil x) => x.VisitorReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Tafsil>>)((ReceptionCheck x) => x.VisitorTafsil))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.VisitorGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<ReceptionCheck>((Expression<Func<Tafsil, IEnumerable<ReceptionCheck>>>)((Tafsil x) => x.CreditReceptionCheck)).WithOne((Expression<Func<ReceptionCheck, Tafsil>>)((ReceptionCheck x) => x.CreditTafsil))
				.HasForeignKey((Expression<Func<ReceptionCheck, object>>)((ReceptionCheck x) => x.CreditA2reTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingReceptionCheque", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Khazane\\Reception\\ReceptionChequeModelContext.cs", 36);
		}
	}

	public void OnModelCreatingOrderDetail(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<OrderDetail>().HasIndex((Expression<Func<OrderDetail, object>>)((OrderDetail p) => p.MasterGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Order>().HasMany<OrderDetail>((Expression<Func<Order, IEnumerable<OrderDetail>>>)((Order x) => x.OrderDetailItem)).WithOne((Expression<Func<OrderDetail, Order>>)((OrderDetail x) => x.OrderItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<OrderDetail>((Expression<Func<Product, IEnumerable<OrderDetail>>>)((Product x) => x.OrderDetails)).WithOne((Expression<Func<OrderDetail, Product>>)((OrderDetail x) => x.ProductItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.ProductGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<OrderDetail>((Expression<Func<Depot, IEnumerable<OrderDetail>>>)((Depot x) => x.OrderDetailItem)).WithOne((Expression<Func<OrderDetail, Depot>>)((OrderDetail x) => x.DepotItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ProductVahed>().HasMany<OrderDetail>((Expression<Func<ProductVahed, IEnumerable<OrderDetail>>>)((ProductVahed x) => x.OrderDetailItem)).WithOne((Expression<Func<OrderDetail, ProductVahed>>)((OrderDetail x) => x.VahedItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.VGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<DecimalEntity>().HasMany<OrderDetail>((Expression<Func<DecimalEntity, IEnumerable<OrderDetail>>>)((DecimalEntity x) => x.OrderDetailItem)).WithOne((Expression<Func<OrderDetail, DecimalEntity>>)((OrderDetail x) => x.DecimalItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.DecimalGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<OrderDetail>((Expression<Func<Tafsil, IEnumerable<OrderDetail>>>)((Tafsil x) => x.ProductOrder)).WithOne((Expression<Func<OrderDetail, Tafsil>>)((OrderDetail x) => x.ProductTafsilItem))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.ProductTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<OrderDetail>().HasMany<OrderDetail>((Expression<Func<OrderDetail, IEnumerable<OrderDetail>>>)((OrderDetail x) => x.ReferenceDetail)).WithOne((Expression<Func<OrderDetail, OrderDetail>>)((OrderDetail x) => x.Reference))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.ReferenceGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<CouponDiscountDetail>().HasMany<OrderDetail>((Expression<Func<CouponDiscountDetail, IEnumerable<OrderDetail>>>)((CouponDiscountDetail x) => x.OrderDetails)).WithOne((Expression<Func<OrderDetail, CouponDiscountDetail>>)((OrderDetail x) => x.Coupon))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.DicountCouponDetailGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<OrderDetail>().HasMany<OrderDetail>((Expression<Func<OrderDetail, IEnumerable<OrderDetail>>>)((OrderDetail x) => x.GrouppedDetail)).WithOne((Expression<Func<OrderDetail, OrderDetail>>)((OrderDetail x) => x.Group))
				.HasForeignKey((Expression<Func<OrderDetail, object>>)((OrderDetail x) => x.GrouppingGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingOrderDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\OrderDetailModelContext.cs", 36);
		}
	}

	public void OnModelCreatingOrderAbad(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<AbadDet>().HasIndex((Expression<Func<AbadDet, object>>)((AbadDet p) => p.MasterGuid));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<OrderAbad>().HasMany<AbadDet>((Expression<Func<OrderAbad, IEnumerable<AbadDet>>>)((OrderAbad x) => x.OrderAbads)).WithOne((Expression<Func<AbadDet, OrderAbad>>)((AbadDet x) => x.Master))
				.HasForeignKey((Expression<Func<AbadDet, object>>)((AbadDet x) => x.MasterGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingOrderAbad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\OrderAbadContext.cs", 22);
		}
	}

	public void OnModelCreatingOrder(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Order>().HasIndex((Expression<Func<Order, object>>)((Order p) => p.DeliveryStatus));
			modelBuilder.Entity<Order>().HasIndex((Expression<Func<Order, object>>)((Order p) => new { p.FactorType, p.Date }));
			modelBuilder.Entity<Order>().HasIndex((Expression<Func<Order, object>>)((Order p) => new { p.FactorType, p.Id2, p.Status })).IsUnique(true);
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<Order>().Property<DateTime?>((Expression<Func<Order, DateTime?>>)((Order p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.MoeinOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.ProductOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.ProductMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.ProductMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.ServiceOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.ServiceMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.ServiceMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.DiscountOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.DiscountMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.DiscountMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.TafsilOrder)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.DiscountOrder)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.DiscountTafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.DiscountTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Order>((Expression<Func<Cashier, IEnumerable<Order>>>)((Cashier x) => x.CashierOrder)).WithOne((Expression<Func<Order, Cashier>>)((Order x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.UserAccGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.FareOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.FareMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.FareMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.AvarezOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.AvarezMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.AvarezMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.TaxOrder)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.TaxMoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.TaxMoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.Hazine4Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.Hazine4MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineMoeinGuid4))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.Hazine1Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.Hazine1MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineMoeinGuid1))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.Hazine2Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.Hazine2MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineMoeinGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.Hazine3Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.Hazine3MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineMoeinGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.B1Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.B1MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B1MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.B2Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.B2MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B2MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.B3Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.B3MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B3MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Order>((Expression<Func<Moein, IEnumerable<Order>>>)((Moein x) => x.B4Order)).WithOne((Expression<Func<Order, Moein>>)((Order x) => x.B4MoeinItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B4MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.FareOrder)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.FareTafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.FareTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.AvarezOrder)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.AvarezTafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.AvarezTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.Hazine1Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.Hazine1TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineTafsilGuid1))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.Hazine2Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.Hazine2TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineTafsilGuid2))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.Hazine3Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.Hazine3TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineTafsilGuid3))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.Hazine4Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.Hazine4TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.HazineTafsilGuid4))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.B1Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.B1TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B1TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.B2Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.B2TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B2TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.B3Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.B3TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B3TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.B4Order)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.B4TafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.B4TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Order>((Expression<Func<Tafsil, IEnumerable<Order>>>)((Tafsil x) => x.TaxOrder)).WithOne((Expression<Func<Order, Tafsil>>)((Order x) => x.TaxTafsilItem))
				.HasForeignKey((Expression<Func<Order, object>>)((Order x) => x.TaxTafsilGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingOrder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Order\\OrderModelContext.cs", 64);
		}
	}

	public void OnModelCreatingGps(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Gps>().HasIndex((Expression<Func<Gps, object>>)((Gps p) => p.ServerStatus));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingGps", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\GpsModelContext.cs", 23);
		}
	}

	public void OnModelCreatingFullTextWord(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<FullTextWords>().HasIndex((Expression<Func<FullTextWords, object>>)((FullTextWords p) => p.Id)).IsUnique(true);
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT1Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT1Field>>>)((FullTextWords x) => x.IxProductT1Field)).WithOne((Expression<Func<IxProductT1Field, FullTextWords>>)((IxProductT1Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT1Field, object>>)((IxProductT1Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT1Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT2Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT2Field>>>)((FullTextWords x) => x.IxProductT2Field)).WithOne((Expression<Func<IxProductT2Field, FullTextWords>>)((IxProductT2Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT2Field, object>>)((IxProductT2Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT2Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT3Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT3Field>>>)((FullTextWords x) => x.IxProductT3Field)).WithOne((Expression<Func<IxProductT3Field, FullTextWords>>)((IxProductT3Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT3Field, object>>)((IxProductT3Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT3Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT4Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT4Field>>>)((FullTextWords x) => x.IxProductT4Field)).WithOne((Expression<Func<IxProductT4Field, FullTextWords>>)((IxProductT4Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT4Field, object>>)((IxProductT4Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT4Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT5Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT5Field>>>)((FullTextWords x) => x.IxProductT5Field)).WithOne((Expression<Func<IxProductT5Field, FullTextWords>>)((IxProductT5Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT5Field, object>>)((IxProductT5Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT5Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT6Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT6Field>>>)((FullTextWords x) => x.IxProductT6Field)).WithOne((Expression<Func<IxProductT6Field, FullTextWords>>)((IxProductT6Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT6Field, object>>)((IxProductT6Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT6Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT7Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT7Field>>>)((FullTextWords x) => x.IxProductT7Field)).WithOne((Expression<Func<IxProductT7Field, FullTextWords>>)((IxProductT7Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT7Field, object>>)((IxProductT7Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT7Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxProductT8Field>((Expression<Func<FullTextWords, IEnumerable<IxProductT8Field>>>)((FullTextWords x) => x.IxProductT8Field)).WithOne((Expression<Func<IxProductT8Field, FullTextWords>>)((IxProductT8Field x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxProductT8Field, object>>)((IxProductT8Field x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxProductT8Field_FullTextWords_WordId");
			RelationalForeignKeyBuilderExtensions.HasConstraintName(((ReferenceCollectionBuilder)(object)modelBuilder.Entity<FullTextWords>().HasMany<IxCustomerAllFields>((Expression<Func<FullTextWords, IEnumerable<IxCustomerAllFields>>>)((FullTextWords x) => x.IxCustomerAllFields)).WithOne((Expression<Func<IxCustomerAllFields, FullTextWords>>)((IxCustomerAllFields x) => x.FullTextWords))
				.HasForeignKey((Expression<Func<IxCustomerAllFields, object>>)((IxCustomerAllFields x) => x.WordId))
				.HasPrincipalKey((Expression<Func<FullTextWords, object>>)((FullTextWords q) => q.Id))).OnDeleteNoAction(), "Fk_IxCustomerAllFields_FullTextWords_WordId");
			modelBuilder.Entity<FullTextWords_temp>().HasNoKey();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingFullTextWord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\FullTextWordModelContext.cs", 35);
		}
	}

	public void OnModelCreatingKardex(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Kardex>().HasKey((Expression<Func<Kardex, object>>)((Kardex p) => new { p.Guid })).IsCluster(isCluster: false);
			SqlServerIndexBuilderExtensions.IsClustered<Kardex>(RelationalIndexBuilderExtensions.HasName<Kardex>(modelBuilder.Entity<Kardex>().HasIndex((Expression<Func<Kardex, object>>)((Kardex p) => new { p.ProductGuid, p.DepotGuid, p.DateM, p.Ordinal, p.Guid })), "IX_Acc_Kardex_prdGuid_DepotGuid_Date_Ordinal"), true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<Kardex>((Expression<Func<Product, IEnumerable<Kardex>>>)((Product x) => x.Kardexes)).WithOne((Expression<Func<Kardex, Product>>)((Kardex x) => x.Product))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.ProductGuid))).OnDeleteNoAction();
			RelationalIndexBuilderExtensions.HasName<Kardex>(modelBuilder.Entity<Kardex>().HasIndex((Expression<Func<Kardex, object>>)((Kardex p) => new { p.SanadGuid, p.SanadType })), "IX_Acc_Kardex_SanadGuid_SanadType");
			modelBuilder.Entity<Kardex>().HasIndex((Expression<Func<Kardex, object>>)((Kardex p) => p.Invalidated));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<Kardex>((Expression<Func<Depot, IEnumerable<Kardex>>>)((Depot x) => x.Kardexes)).WithOne((Expression<Func<Kardex, Depot>>)((Kardex x) => x.Depot))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.DepotGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Kardex>((Expression<Func<Moein, IEnumerable<Kardex>>>)((Moein x) => x.Kardexes)).WithOne((Expression<Func<Kardex, Moein>>)((Kardex x) => x.Moein))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.MoeinGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Kardex>((Expression<Func<Tafsil, IEnumerable<Kardex>>>)((Tafsil x) => x.Kardexes)).WithOne((Expression<Func<Kardex, Tafsil>>)((Kardex x) => x.Tafsil))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.TafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Kardex>((Expression<Func<Tafsil, IEnumerable<Kardex>>>)((Tafsil x) => x.KardexProductSide)).WithOne((Expression<Func<Kardex, Tafsil>>)((Kardex x) => x.ProductTafsil))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.ProductTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Cashier>().HasMany<Kardex>((Expression<Func<Cashier, IEnumerable<Kardex>>>)((Cashier x) => x.Kardexs)).WithOne((Expression<Func<Kardex, Cashier>>)((Kardex x) => x.CashierItem))
				.HasForeignKey((Expression<Func<Kardex, object>>)((Kardex x) => x.UserGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingKardex", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\KardexModelContext.cs", 33);
		}
	}

	public void OnModelCreatingDepot(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Depot>().HasIndex((Expression<Func<Depot, object>>)((Depot p) => p.Name)).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingDepot", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\DepotModelContext.cs", 19);
		}
	}

	public void OnModelCreatingProductGroup(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductGroup>().HasIndex((Expression<Func<ProductGroup, object>>)((ProductGroup p) => p.Name)).IsUnique(true);
			modelBuilder.Entity<ProductGroup>().HasIndex((Expression<Func<ProductGroup, object>>)((ProductGroup p) => p.Slug)).IsUnique(true);
			modelBuilder.Entity<ProductGroup>().Property<string>((Expression<Func<ProductGroup, string>>)((ProductGroup p) => p.Slug)).IsRequired(true);
			modelBuilder.Entity<ProductGroup>().HasIndex((Expression<Func<ProductGroup, object>>)((ProductGroup p) => p.Status));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductGroupModelContext.cs", 22);
		}
	}

	public void OnModelCreatingBarcode(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Barcode>().HasIndex((Expression<Func<Barcode, object>>)((Barcode p) => p.PrdGuid));
			modelBuilder.Entity<Barcode>().HasIndex((Expression<Func<Barcode, object>>)((Barcode p) => new { p.PrdGuid, p.Ordinal })).IsUnique(true);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Product>().HasMany<Barcode>((Expression<Func<Product, IEnumerable<Barcode>>>)((Product x) => x.Barcodes)).WithOne((Expression<Func<Barcode, Product>>)((Barcode x) => x.Products))
				.HasForeignKey((Expression<Func<Barcode, object>>)((Barcode x) => x.PrdGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Depot>().HasMany<Barcode>((Expression<Func<Depot, IEnumerable<Barcode>>>)((Depot x) => x.Barcodes)).WithOne((Expression<Func<Barcode, Depot>>)((Barcode x) => x.Depots))
				.HasForeignKey((Expression<Func<Barcode, object>>)((Barcode x) => x.DepotGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\BarcodeModelContext.cs", 26);
		}
	}

	public void OnModelCreatingProductVahed(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<ProductVahed>().HasIndex((Expression<Func<ProductVahed, object>>)((ProductVahed p) => p.Name)).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingProductVahed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Product\\ProductVahedModelContext.cs", 19);
		}
	}

	public void OnModelCreatingCashier(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<Cashier>().HasIndex((Expression<Func<Cashier, object>>)((Cashier p) => p.Name)).IsUnique(true);
			modelBuilder.Entity<Cashier>().HasIndex((Expression<Func<Cashier, object>>)((Cashier p) => p.UserName)).IsUnique(true);
			modelBuilder.Entity<Cashier>().HasIndex((Expression<Func<Cashier, object>>)((Cashier p) => p.ServerStatus));
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(modelBuilder.Entity<CashiersLog>().Property<DateTime?>((Expression<Func<CashiersLog, DateTime?>>)((CashiersLog p) => p.DateUtc)), "getdate()");
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<UserAcc>().HasMany<Cashier>((Expression<Func<UserAcc, IEnumerable<Cashier>>>)((UserAcc x) => x.Cashiers)).WithOne((Expression<Func<Cashier, UserAcc>>)((Cashier x) => x.UserItem))
				.HasForeignKey((Expression<Func<Cashier, object>>)((Cashier x) => x.UserGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Cashier>((Expression<Func<Tafsil, IEnumerable<Cashier>>>)((Tafsil x) => x.Cashiers)).WithOne((Expression<Func<Cashier, Tafsil>>)((Cashier x) => x.Customer))
				.HasForeignKey((Expression<Func<Cashier, object>>)((Cashier x) => x.CusGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Tafsil>().HasMany<Cashier>((Expression<Func<Tafsil, IEnumerable<Cashier>>>)((Tafsil x) => x.DefaultFareOrder)).WithOne((Expression<Func<Cashier, Tafsil>>)((Cashier x) => x.FareTafsilItem))
				.HasForeignKey((Expression<Func<Cashier, object>>)((Cashier x) => x.FareTafsilGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Moein>().HasMany<Cashier>((Expression<Func<Moein, IEnumerable<Cashier>>>)((Moein x) => x.DefaultFareOrder)).WithOne((Expression<Func<Cashier, Moein>>)((Cashier x) => x.FareMoeinItem))
				.HasForeignKey((Expression<Func<Cashier, object>>)((Cashier x) => x.FareMoeinGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingCashier", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Users\\CashierModelContext.cs", 33);
		}
	}

	public void OnModelCreatingUserAcc(ModelBuilder modelBuilder)
	{
		try
		{
			modelBuilder.Entity<UserAcc>().HasIndex((Expression<Func<UserAcc, object>>)((UserAcc p) => p.ServerStatus));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreatingUserAcc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\AccountingConfigs\\Users\\UserModelContext.cs", 19);
		}
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		try
		{
			SqlServerDbContextOptionsExtensions.UseSqlServer(options, base.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)delegate(SqlServerDbContextOptionsBuilder op)
			{
				((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).CommandTimeout((int?)150);
				((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).MigrationsHistoryTable("__Migrations_Acc", "dbo");
			}).UseQueryTrackingBehavior((QueryTrackingBehavior)1);
			options.ConfigureWarnings((Action<WarningsConfigurationBuilder>)delegate(WarningsConfigurationBuilder warnings)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				warnings.Ignore((EventId[])(object)new EventId[1] { RelationalEventId.BoolWithDefaultWarning });
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "options=>" + Json.ToStringJson(options), "OnConfiguring", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Acc_ModelContext.cs", 85);
			throw;
		}
	}

	public Acc_ModelContext(string connectionString, string tranName, int timeout, string debugerMessage, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		: base(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			base.OnModelCreating(modelBuilder);
			OnModelCreatingHesabGroup(modelBuilder);
			OnModelCreatingTafsilGroup(modelBuilder);
			OnModelCreatingMoein(modelBuilder);
			OnModelCreatingKol(modelBuilder);
			OnModelCreatingTafsil(modelBuilder);
			OnModelCreatingGardeshTafsil(modelBuilder);
			OnModelCreatingSuggestedBank(modelBuilder);
			OnModelCreatingTafsilNumbers(modelBuilder);
			OnModelCreatingMoeinTafsilGroupRelation(modelBuilder);
			OnModelCreatingAccount(modelBuilder);
			OnModelCreatingCustomer(modelBuilder);
			OnModelCreatingCustomerGroup(modelBuilder);
			OnModelCreatingCustomerTitle(modelBuilder);
			OnModelCreatingCustomerCard(modelBuilder);
			OnModelCreatingProduct(modelBuilder);
			OnModelCreatingProductVahed(modelBuilder);
			OnModelCreatingProductGroup(modelBuilder);
			OnModelCreatingBarcode(modelBuilder);
			OnModelCreatingDepot(modelBuilder);
			OnModelCreatingKardex(modelBuilder);
			OnModelCreatingPrdAnbarRelation(modelBuilder);
			OnModelCreatingPrdSerial(modelBuilder);
			OnModelCreatingDepEnteqal(modelBuilder);
			OnModelCreatingAnbarGardani(modelBuilder);
			OnModelCreatingAnbarGardaniDetail(modelBuilder);
			OnModelCreatingResid(modelBuilder);
			OnModelCreatingPrdTechnical(modelBuilder);
			OnModelCreatingPrdAttrebute(modelBuilder);
			OnModelCreatingTozieList(modelBuilder);
			OnModelCreatingProductPriceUpdate(modelBuilder);
			OnModelCreatingProductDecimalRelation(modelBuilder);
			OnModelCreatingProductGroupRelation(modelBuilder);
			OnModelCreatingDepotReceipt(modelBuilder);
			OnModelCreatingTax(modelBuilder);
			OnModelCreatingProductPriceLevel(modelBuilder);
			OnModelCreatingProductPriceRelation(modelBuilder);
			OnModelCreatingEvaluation(modelBuilder);
			OnModelCreatingOrder(modelBuilder);
			OnModelCreatingOrderDetail(modelBuilder);
			OnModelCreatingOrderAbad(modelBuilder);
			OnModelCreatingOpenInvoice(modelBuilder);
			OnModelCreatingFareOrderCredit(modelBuilder);
			OnModelCreatingTempBill(modelBuilder);
			OnModelCreatingTaxOrderLog(modelBuilder);
			OnModelCreatingOrderDetailCouponCalculation(modelBuilder);
			OnModelCreatingReservation(modelBuilder);
			OnModelCreatingUserAcc(modelBuilder);
			OnModelCreatingCashier(modelBuilder);
			OnModelCreatingPardakht(modelBuilder);
			OnModelCreatingPardakhtHavale(modelBuilder);
			OnModelCreatingPardakhtNaqd(modelBuilder);
			OnModelCreatingPardakhtChequeM(modelBuilder);
			OnModelCreatingPardakhtChequeSh(modelBuilder);
			OnModelCreatingChequeShOperation(modelBuilder);
			OnModelCreatingPardakhtChequeEntezami(modelBuilder);
			OnModelCreatingReception(modelBuilder);
			OnModelCreatingReceptionCheque(modelBuilder);
			OnModelCreatingReceptionHavale(modelBuilder);
			OnModelCreatingReceptionNaqd(modelBuilder);
			OnModelCreatingChequeBook(modelBuilder);
			OnModelCreatingChequeMOperation(modelBuilder);
			OnModelCreatingReceptionChequeEntezami(modelBuilder);
			OnModelCreatingViewChequeM(modelBuilder);
			OnModelCreatingViewChequeSh(modelBuilder);
			OnModelCreatingSanad(modelBuilder);
			OnModelCreatingSanadDetail(modelBuilder);
			OnModelCreatingSanadPrint(modelBuilder);
			OnModelCreatingSanadStatusReport(modelBuilder);
			OnModelCreatingViewSanadDetail(modelBuilder);
			OnModelCreatingViewSanadDetailCover(modelBuilder);
			OnModelCreatingSanadStatusLog(modelBuilder);
			OnModelCreatingSanadStatusReportUpdates(modelBuilder);
			OnModelCreatingAghsatSh(modelBuilder);
			OnModelCreatingAghsatM(modelBuilder);
			OnModelCreatingTafsilSelection(modelBuilder);
			OnModelCreatingProductSelection(modelBuilder);
			OnModelCreatingProductGroupSelection(modelBuilder);
			OnModelCreatingTolid(modelBuilder);
			OnModelCreatingTolidExport(modelBuilder);
			OnModelCreatingTolidHazine(modelBuilder);
			OnModelCreatingTolidMavad(modelBuilder);
			OnModelCreatingTolidSummary(modelBuilder);
			OnModelCreatingTolidFormulaExport(modelBuilder);
			OnModelCreatingTolidFormulaMavad(modelBuilder);
			OnModelCreatingTolidFormulaHazine(modelBuilder);
			OnModelCreatingDiscountCoupon(modelBuilder);
			OnModelCreatingDecimal(modelBuilder);
			OnModelCreatingFullTextWord(modelBuilder);
			OnModelCreatingGps(modelBuilder);
			OnModelCreatingVisit(modelBuilder);
			OnModelCreatingIndex(modelBuilder);
			OnModelCreatingLastPrice(modelBuilder);
			OnModelCreatingCallCenter(modelBuilder);
			OnModelCreatingLottery(modelBuilder);
			OnModelCreatingRasGiri(modelBuilder);
			OnModelCreatingNote(modelBuilder);
			OnModelCreatingVam(modelBuilder);
			OnModelCreatingTtms(modelBuilder);
			OnModelCreatingServiceReceiptDetailRelation(modelBuilder);
			OnModelCreatingCustomerReceiptRelation(modelBuilder);
			OnModelCreatingPlacement(modelBuilder);
			OnModelCreatingReceipt(modelBuilder);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Acc_ModelContext.cs", 233);
		}
	}
}
