using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class SyncInformation : IDisposable, IItemAsyncMapper<SyncInformation, SyncInformation>, IEquatable<SyncInformation>
{
	public DateTime? ServerGenerationTime { get; set; }

	public DateTime LastPriceSynced { get; set; }

	public DateTime LastLogonPersonSynced { get; set; }

	public DateTime LastProductGroupSynced { get; set; }

	public DateTime LastProductVahedSynced { get; set; }

	public DateTime LastDepotSynced { get; set; }

	public DateTime LastCustomerGroupSynced { get; set; }

	public DateTime LastCustomerTitleSynced { get; set; }

	public DateTime LastSettingSynced { get; set; }

	public DateTime LastUserSynced { get; set; }

	public DateTime LastFileInfoSynced { get; set; }

	public DateTime LastDeliverySynced { get; set; }

	public DateTime LastNewsSynced { get; set; }

	public DateTime LastHyperLinkSynced { get; set; }

	public DateTime LastContactUsSynced { get; set; }

	public DateTime LastOrderSynced { get; set; }

	public DateTime LastReceptionSynced { get; set; }

	public DateTime LastPardakhtSynced { get; set; }

	public DateTime LastAghsatShakhsiSynced { get; set; }

	public DateTime LastCallingLogSynced { get; set; }

	public DateTime LastAghsatAshkhasSynced { get; set; }

	public DateTime LastViewCheckMSynced { get; set; }

	public DateTime LastViewCheckShSynced { get; set; }

	public DateTime LastTafsilLevelSynced { get; set; }

	public DateTime LastVisitSynced { get; set; }

	public DateTime LastKolSynced { get; set; }

	public DateTime LastMoeinSynced { get; set; }

	public DateTime LastTafsilSynced { get; set; }

	public DateTime LastProductAttributeSynced { get; set; }

	public DateTime LastUserLogSynced { get; set; }

	public DateTime LastProductSynced { get; set; }

	public DateTime LastCustomerSynced { get; set; }

	public DateTime LastHesabTreeSynced { get; set; }

	public DateTime LastTicketSynced { get; set; }

	public DateTime LastTicketDetSynced { get; set; }

	public DateTime LastTicketPartSynced { get; set; }

	public DateTime LastDescSynced { get; set; }

	public DateTime LastAccountSynced { get; set; }

	public DateTime LastPriceUpdate { get; set; }

	public DateTime LastLogonPersonUpdate { get; set; }

	public DateTime LastProductGroupUpdate { get; set; }

	public DateTime LastProductVahedUpdate { get; set; }

	public DateTime LastDepotUpdate { get; set; }

	public DateTime LastCustomerGroupUpdate { get; set; }

	public DateTime LastCustomerTitleUpdate { get; set; }

	public DateTime LastSettingUpdate { get; set; }

	public DateTime LastUserUpdate { get; set; }

	public DateTime LastFileInfoUpdate { get; set; }

	public DateTime LastDeliveryUpdate { get; set; }

	public DateTime LastNewsUpdate { get; set; }

	public DateTime LastHyperLinkUpdate { get; set; }

	public DateTime LastContactUsUpdate { get; set; }

	public DateTime LastOrderUpdate { get; set; }

	public DateTime LastReceptionUpdate { get; set; }

	public DateTime LastPardakhtUpdate { get; set; }

	public DateTime LastAghsatShakhsiUpdate { get; set; }

	public DateTime LastCallingLogUpdate { get; set; }

	public DateTime LastAghsatAshkhasUpdate { get; set; }

	public DateTime LastViewCheckMUpdate { get; set; }

	public DateTime LastViewCheckShUpdate { get; set; }

	public DateTime LastTafsilLevelUpdate { get; set; }

	public DateTime LastVisitUpdate { get; set; }

	public DateTime LastKolUpdate { get; set; }

	public DateTime LastMoeinUpdate { get; set; }

	public DateTime LastTafsilUpdate { get; set; }

	public DateTime LastProductAttributeUpdate { get; set; }

	public DateTime LastUserLogUpdate { get; set; }

	public DateTime LastProductUpdate { get; set; }

	public DateTime LastCustomerUpdate { get; set; }

	public DateTime LastHesabTreeUpdate { get; set; }

	public DateTime LastTicketUpdate { get; set; }

	public DateTime LastTicketDetUpdate { get; set; }

	public DateTime LastTicketPartUpdate { get; set; }

	public DateTime LastDescUpdate { get; set; }

	public DateTime LastAccountUpdate { get; set; }

	public DateTime LastPrice
	{
		get
		{
			if (!(LastPriceSynced >= LastPriceUpdate))
			{
				return LastPriceSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime LogonPerson
	{
		get
		{
			if (!(LastLogonPersonSynced >= LastLogonPersonUpdate))
			{
				return LastLogonPersonSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ProductGroup
	{
		get
		{
			if (!(LastProductGroupSynced >= LastProductGroupUpdate))
			{
				return LastProductGroupSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ProductVahed
	{
		get
		{
			if (!(LastProductVahedSynced >= LastProductVahedUpdate))
			{
				return LastProductVahedSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Depot
	{
		get
		{
			if (!(LastDepotSynced >= LastDepotUpdate))
			{
				return LastDepotSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime CustomerGroup
	{
		get
		{
			if (!(LastCustomerGroupSynced >= LastCustomerGroupUpdate))
			{
				return LastCustomerGroupSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime CustomerTitle
	{
		get
		{
			if (!(LastCustomerTitleSynced >= LastCustomerTitleUpdate))
			{
				return LastCustomerTitleSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Setting
	{
		get
		{
			if (!(LastSettingSynced >= LastSettingUpdate))
			{
				return LastSettingSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime User
	{
		get
		{
			if (!(LastUserSynced >= LastUserUpdate))
			{
				return LastUserSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime FileInfo
	{
		get
		{
			if (!(LastFileInfoSynced >= LastFileInfoUpdate))
			{
				return LastFileInfoSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Delivery
	{
		get
		{
			if (!(LastDeliverySynced >= LastDeliveryUpdate))
			{
				return LastDeliverySynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime News
	{
		get
		{
			if (!(LastNewsSynced >= LastNewsUpdate))
			{
				return LastNewsSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime HyperLink
	{
		get
		{
			if (!(LastHyperLinkSynced >= LastHyperLinkUpdate))
			{
				return LastHyperLinkSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ContactUs
	{
		get
		{
			if (!(LastContactUsSynced >= LastContactUsUpdate))
			{
				return LastContactUsSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Order
	{
		get
		{
			if (!(LastOrderSynced >= LastOrderUpdate))
			{
				return LastOrderSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Reception
	{
		get
		{
			if (!(LastReceptionSynced >= LastReceptionUpdate))
			{
				return LastReceptionSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Pardakht
	{
		get
		{
			if (!(LastPardakhtSynced >= LastPardakhtUpdate))
			{
				return LastPardakhtSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime AghsatShakhsi
	{
		get
		{
			if (!(LastAghsatShakhsiSynced >= LastAghsatShakhsiUpdate))
			{
				return LastAghsatShakhsiSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime CallingLog
	{
		get
		{
			if (!(LastCallingLogSynced >= LastCallingLogUpdate))
			{
				return LastCallingLogSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime AghsatAshkhas
	{
		get
		{
			if (!(LastAghsatAshkhasSynced >= LastAghsatAshkhasUpdate))
			{
				return LastAghsatAshkhasSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ViewCheckM
	{
		get
		{
			if (!(LastViewCheckMSynced >= LastViewCheckMUpdate))
			{
				return LastViewCheckMSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ViewCheckSh
	{
		get
		{
			if (!(LastViewCheckShSynced >= LastViewCheckShUpdate))
			{
				return LastViewCheckShSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime TafsilLevel
	{
		get
		{
			if (!(LastTafsilLevelSynced >= LastTafsilLevelUpdate))
			{
				return LastTafsilLevelSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Visit
	{
		get
		{
			if (!(LastVisitSynced >= LastVisitUpdate))
			{
				return LastVisitSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Kol
	{
		get
		{
			if (!(LastKolSynced >= LastKolUpdate))
			{
				return LastKolSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Moein
	{
		get
		{
			if (!(LastMoeinSynced >= LastMoeinUpdate))
			{
				return LastMoeinSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Tafsil
	{
		get
		{
			if (!(LastTafsilSynced >= LastTafsilUpdate))
			{
				return LastTafsilSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime ProductAttribute
	{
		get
		{
			if (!(LastProductAttributeSynced >= LastProductAttributeUpdate))
			{
				return LastProductAttributeSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime UserLog
	{
		get
		{
			if (!(LastUserLogSynced >= LastUserLogUpdate))
			{
				return LastUserLogSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Product
	{
		get
		{
			if (!(LastProductSynced >= LastProductUpdate))
			{
				return LastProductSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Customer
	{
		get
		{
			if (!(LastCustomerSynced >= LastCustomerUpdate))
			{
				return LastCustomerSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime HesabTree
	{
		get
		{
			if (!(LastHesabTreeSynced >= LastHesabTreeUpdate))
			{
				return LastHesabTreeSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Ticket
	{
		get
		{
			if (!(LastTicketSynced >= LastTicketUpdate))
			{
				return LastTicketSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime TicketDet
	{
		get
		{
			if (!(LastTicketDetSynced >= LastTicketDetUpdate))
			{
				return LastTicketDetSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime TicketPart
	{
		get
		{
			if (!(LastTicketPartSynced >= LastTicketPartUpdate))
			{
				return LastTicketPartSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Desc
	{
		get
		{
			if (!(LastDescSynced >= LastDescUpdate))
			{
				return LastDescSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	public DateTime Account
	{
		get
		{
			if (!(LastAccountSynced >= LastAccountUpdate))
			{
				return LastAccountSynced;
			}
			return DateTime.Now;
		}
		set
		{
		}
	}

	[JsonIgnore]
	public bool ResetProductGroup
	{
		get
		{
			return DateTime.Now.Subtract(ProductGroup).TotalDays > 180.0;
		}
		set
		{
			if (value)
			{
				ProductGroup = ProductGroup.AddYears(-5);
			}
		}
	}

	[JsonIgnore]
	public bool ResetCustomersGroup
	{
		get
		{
			return DateTime.Now.Subtract(CustomerGroup).TotalDays > 180.0;
		}
		set
		{
			if (value)
			{
				CustomerGroup = CustomerGroup.AddYears(-5);
			}
		}
	}

	[JsonIgnore]
	public bool ResetUserLogs
	{
		get
		{
			return DateTime.Now.Subtract(UserLog).TotalDays > 180.0;
		}
		set
		{
			if (value)
			{
				UserLog = UserLog.AddYears(-5);
			}
		}
	}

	public bool CusGroupReset { get; set; }

	public bool PrdGroupReset { get; set; }

	public bool CustomerReset { get; set; }

	public bool ProductReset { get; set; }

	public bool AghsatAshkhasReset { get; set; }

	public bool AghsatShakhsiReset { get; set; }

	public bool CallingLogReset { get; set; }

	public bool ViewCheckMReset { get; set; }

	public bool ViewCheckShReset { get; set; }

	public bool TafsilLevelReset { get; set; }

	public bool PrdVahedReset { get; set; }

	public bool DepotReset { get; set; }

	public bool CusTitleReset { get; set; }

	public bool HesabTreeReset { get; set; }

	public bool KolReset { get; set; }

	public bool MoeinReset { get; set; }

	public bool TafsilReset { get; set; }

	public bool ProductAttributeReset { get; set; }

	public bool TicketReset { get; set; }

	public bool TicketDetReset { get; set; }

	public bool TicketPartReset { get; set; }

	public bool TicketStatusReset { get; set; }

	public bool DescReset { get; set; }

	public bool IntroductionMethodReset { get; set; }

	public bool PrdAnbarGardaniReset { get; set; }

	public bool VisitReset { get; set; }

	public bool UserLogReset { get; set; }

	public bool LastPrice_isSyncing { get; set; }

	public DateTime LastPrice_Syncing { get; set; }

	public bool LogOnPerson_isSyncing { get; set; }

	public DateTime LogOnPerson_Syncing { get; set; }

	public bool Order_isSyncing { get; set; }

	public DateTime Order_Syncing { get; set; }

	public bool Pardakht_isSyncing { get; set; }

	public DateTime Pardakht_Syncing { get; set; }

	public bool Customer_isSyncing { get; set; }

	public DateTime Customer_Syncing { get; set; }

	public bool Product_isSyncing { get; set; }

	public DateTime Product_Syncing { get; set; }

	public bool Reception_isSyncing { get; set; }

	public DateTime Reception_Syncing { get; set; }

	public DateTime Setting_Syncing { get; set; }

	public DateTime User_Syncing { get; set; }

	public DateTime HesabTree_Syncing { get; set; }

	public DateTime CustomerGroup_Syncing { get; set; }

	public DateTime CustomerTitle_Syncing { get; set; }

	public DateTime Visit_Syncing { get; set; }

	public DateTime ProductVahed_Syncing { get; set; }

	public DateTime TafsilLevel_Syncing { get; set; }

	public DateTime ProductGroup_Syncing { get; set; }

	public DateTime Depot_Syncing { get; set; }

	public DateTime Ticket_Syncing { get; set; }

	public DateTime TicketDet_Syncing { get; set; }

	public DateTime TicketPart_Syncing { get; set; }

	public DateTime TicketStatus_Syncing { get; set; }

	public DateTime Desc_Syncing { get; set; }

	public DateTime AghsatAshkhas_Syncing { get; set; }

	public DateTime AghsatShakhsi_Syncing { get; set; }

	public DateTime Kol_Syncing { get; set; }

	public DateTime Moein_Syncing { get; set; }

	public DateTime Tafsil_Syncing { get; set; }

	public DateTime ProductAttribute_Syncing { get; set; }

	public DateTime UserLog_Syncing { get; set; }

	public DateTime Sanad { get; set; }

	public DateTime ClsPrdAnbarGardani { get; set; }

	public DateTime Sanad_Syncing { get; set; }

	public DateTime PrdAnbarGardani_Syncing { get; set; }

	public int ProductTotalPartitions { get; set; }

	public int ProductSyncedPartitions { get; set; }

	public int ProductGroupTotalPartitions { get; set; }

	public int ProductGroupSyncedPartitions { get; set; }

	public int ProductVahedTotalPartitions { get; set; }

	public int ProductVahedSyncedPartitions { get; set; }

	public int CustomerTotalPartitions { get; set; }

	public int CustomerSyncedPartitions { get; set; }

	public int AccountTotalPartitions { get; set; }

	public int AccountSyncedPartitions { get; set; }

	public int HesabTreeTotalPartitions { get; set; }

	public int HesabTreeSyncedPartitions { get; set; }

	public int TafsilTotalPartitions { get; set; }

	public int TafsilSyncedPartitions { get; set; }

	public int KolTotalPartitions { get; set; }

	public int KolSyncedPartitions { get; set; }

	public int MoeinTotalPartitions { get; set; }

	public int MoeinSyncedPartitions { get; set; }

	public int CustomerTitleTotalPartitions { get; set; }

	public int CustomerTitleSyncedPartitions { get; set; }

	public int CustomerGroupTotalPartitions { get; set; }

	public int CustomerGroupSyncedPartitions { get; set; }

	public int CallingLogTotalPartitions { get; set; }

	public int CallingLogSyncedPartitions { get; set; }

	public int AghsatAshkhasTotalPartitions { get; set; }

	public int AghsatAshkhasSyncedPartitions { get; set; }

	public int AghsatShakhsiTotalPartitions { get; set; }

	public int AghsatShakhsiSyncedPartitions { get; set; }

	public int DepotTotalPartitions { get; set; }

	public int DepotSyncedPartitions { get; set; }

	public int ContactUsTotalPartitions { get; set; }

	public int ContactUsSyncedPartitions { get; set; }

	public int NewsTotalPartitions { get; set; }

	public int NewsSyncedPartitions { get; set; }

	public int HyperLinkTotalPartitions { get; set; }

	public int HyperLinkSyncedPartitions { get; set; }

	public int ViewCheckShTotalPartitions { get; set; }

	public int ViewCheckShSyncedPartitions { get; set; }

	public int ViewCheckMTotalPartitions { get; set; }

	public int ViewCheckMSyncedPartitions { get; set; }

	public int FileInfoTotalPartitions { get; set; }

	public int FileInfoSyncedPartitions { get; set; }

	public int DeliveryTotalPartitions { get; set; }

	public int DeliverySyncedPartitions { get; set; }

	public int TafsilLevelTotalPartitions { get; set; }

	public int TafsilLevelSyncedPartitions { get; set; }

	public int UserTotalPartitions { get; set; }

	public int UserSyncedPartitions { get; set; }

	public int ProductAttributeTotalPartitions { get; set; }

	public int ProductAttributeSyncedPartitions { get; set; }

	public int LastPriceTotalPartitions { get; set; }

	public int LastPriceSyncedPartitions { get; set; }

	public decimal ProductPercentage()
	{
		if (ProductTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)ProductSyncedPartitions / (decimal)ProductTotalPartitions;
	}

	public decimal ProductGroupPercentage()
	{
		if (ProductGroupTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)ProductGroupSyncedPartitions / (decimal)ProductGroupTotalPartitions;
	}

	public decimal DepotPercentage()
	{
		if (DepotTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)DepotSyncedPartitions / (decimal)DepotTotalPartitions;
	}

	public decimal LastPricePercentage()
	{
		if (LastPriceTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)LastPriceSyncedPartitions / (decimal)LastPriceTotalPartitions;
	}

	public decimal TafsilPercentage()
	{
		if (TafsilTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)TafsilSyncedPartitions / (decimal)TafsilTotalPartitions;
	}

	public decimal MoeinPercentage()
	{
		if (MoeinTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)MoeinSyncedPartitions / (decimal)MoeinTotalPartitions;
	}

	public decimal KolPercentage()
	{
		if (KolTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)KolSyncedPartitions / (decimal)KolTotalPartitions;
	}

	public decimal CallingLogPercentage()
	{
		if (CallingLogTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)CallingLogSyncedPartitions / (decimal)CallingLogTotalPartitions;
	}

	public decimal DeliveryPercentage()
	{
		if (DeliveryTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)DeliverySyncedPartitions / (decimal)DeliveryTotalPartitions;
	}

	public decimal HesabTreePercentage()
	{
		if (HesabTreeTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)HesabTreeSyncedPartitions / (decimal)HesabTreeTotalPartitions;
	}

	public decimal FileInfoPercentage()
	{
		if (FileInfoTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)FileInfoSyncedPartitions / (decimal)FileInfoTotalPartitions;
	}

	public decimal CustomerPercentage()
	{
		if (CustomerTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)CustomerSyncedPartitions / (decimal)CustomerTotalPartitions;
	}

	public decimal CustomerGroupPercentage()
	{
		if (CustomerGroupTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)CustomerGroupSyncedPartitions / (decimal)CustomerGroupTotalPartitions;
	}

	public decimal AccountPercentage()
	{
		if (AccountTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)AccountSyncedPartitions / (decimal)AccountTotalPartitions;
	}

	public decimal ViewChequeMPercentage()
	{
		if (ViewCheckMTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)ViewCheckMSyncedPartitions / (decimal)ViewCheckMTotalPartitions;
	}

	public decimal ViewChequeShPercentage()
	{
		if (ViewCheckShTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)ViewCheckShSyncedPartitions / (decimal)ViewCheckShTotalPartitions;
	}

	public decimal AghsatMPercentage()
	{
		if (AghsatAshkhasTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)AghsatAshkhasSyncedPartitions / (decimal)AghsatAshkhasTotalPartitions;
	}

	public decimal AghsatShPercentage()
	{
		if (AghsatShakhsiTotalPartitions <= 0)
		{
			return 0m;
		}
		return 100m * (decimal)AghsatShakhsiSyncedPartitions / (decimal)AghsatShakhsiTotalPartitions;
	}

	public SyncInformation()
	{
		Reset();
	}

	public static SyncInformation FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs"))
		{
			try
			{
				return data.FromJson<SyncInformation>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs", 157);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs", 161);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs", 176);
				throw;
			}
		}
	}

	public void Reset()
	{
		try
		{
			DateTime clsPrdAnbarGardani = (Sanad = (UserLog = (Desc = (TicketPart = (TicketDet = (Ticket = (ProductAttribute = (LogonPerson = (LastPrice = (Tafsil = (Moein = (Kol = (ViewCheckM = (TafsilLevel = (ViewCheckSh = (AghsatShakhsi = (AghsatAshkhas = (Visit = (Reception = (Pardakht = (Order = (ContactUs = (HyperLink = (News = (Delivery = (FileInfo = (User = (Setting = (CustomerTitle = (CustomerGroup = (Depot = (ProductVahed = (ProductGroup = DateTime.Now.AddDays(-1000.0))))))))))))))))))))))))))))))))));
			ClsPrdAnbarGardani = clsPrdAnbarGardani;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Reset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs", 409);
		}
	}

	public void Dispose()
	{
	}

	public Task<SyncInformation> MapAsync(string contextGuid, SyncInformation source, SyncInformation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.LastPrice = source.LastPrice;
			destination.LogonPerson = source.LogonPerson;
			destination.ProductGroup = source.ProductGroup;
			destination.ProductVahed = source.ProductVahed;
			destination.Depot = source.Depot;
			destination.CustomerGroup = source.CustomerGroup;
			destination.CustomerTitle = source.CustomerTitle;
			destination.Setting = source.Setting;
			destination.User = source.User;
			destination.FileInfo = source.FileInfo;
			destination.Delivery = source.Delivery;
			destination.News = source.News;
			destination.HyperLink = source.HyperLink;
			destination.ContactUs = source.ContactUs;
			destination.Order = source.Order;
			destination.Reception = source.Reception;
			destination.Pardakht = source.Pardakht;
			destination.AghsatShakhsi = source.AghsatShakhsi;
			destination.CallingLog = source.CallingLog;
			destination.AghsatAshkhas = source.AghsatAshkhas;
			destination.ViewCheckM = source.ViewCheckM;
			destination.ViewCheckSh = source.ViewCheckSh;
			destination.TafsilLevel = source.TafsilLevel;
			destination.Visit = source.Visit;
			destination.ResetProductGroup = source.ResetProductGroup;
			destination.ResetCustomersGroup = source.ResetCustomersGroup;
			destination.Product = source.Product;
			destination.Customer = source.Customer;
			destination.HesabTree = source.HesabTree;
			destination.Kol = source.Kol;
			destination.Moein = source.Moein;
			destination.Tafsil = source.Tafsil;
			destination.ProductAttribute = source.ProductAttribute;
			destination.CusGroupReset = source.CusGroupReset;
			destination.PrdGroupReset = source.PrdGroupReset;
			destination.CustomerReset = source.CustomerReset;
			destination.ProductReset = source.ProductReset;
			destination.AghsatAshkhasReset = source.AghsatAshkhasReset;
			destination.AghsatShakhsiReset = source.AghsatShakhsiReset;
			destination.KolReset = source.KolReset;
			destination.TafsilReset = source.TafsilReset;
			destination.MoeinReset = source.MoeinReset;
			destination.ProductAttributeReset = source.ProductAttributeReset;
			destination.UserLog = source.UserLog;
			destination.Account = source.Account;
			destination.CallingLogReset = source.CallingLogReset;
			destination.ViewCheckMReset = source.ViewCheckMReset;
			destination.ViewCheckShReset = source.ViewCheckShReset;
			destination.TafsilLevelReset = source.TafsilLevelReset;
			destination.PrdVahedReset = source.PrdVahedReset;
			destination.DepotReset = source.DepotReset;
			destination.CusTitleReset = source.CusTitleReset;
			destination.HesabTreeReset = source.HesabTreeReset;
			destination.DescReset = source.DescReset;
			destination.IntroductionMethodReset = source.IntroductionMethodReset;
			destination.UserLogReset = source.UserLogReset;
			destination.VisitReset = source.VisitReset;
			destination.LastPrice_isSyncing = source.LastPrice_isSyncing;
			destination.LastPrice_Syncing = source.LastPrice_Syncing;
			destination.LogOnPerson_isSyncing = source.LogOnPerson_isSyncing;
			destination.LogOnPerson_Syncing = source.LogOnPerson_Syncing;
			destination.Order_isSyncing = source.Order_isSyncing;
			destination.Order_Syncing = source.Order_Syncing;
			destination.Pardakht_isSyncing = source.Pardakht_isSyncing;
			destination.Pardakht_Syncing = source.Pardakht_Syncing;
			destination.Customer_isSyncing = source.Customer_isSyncing;
			destination.Customer_Syncing = source.Customer_Syncing;
			destination.Product_isSyncing = source.Product_isSyncing;
			destination.Product_Syncing = source.Product_Syncing;
			destination.Reception_isSyncing = source.Reception_isSyncing;
			destination.Reception_Syncing = source.Reception_Syncing;
			destination.Setting_Syncing = source.Setting_Syncing;
			destination.User_Syncing = source.User_Syncing;
			destination.HesabTree_Syncing = source.HesabTree_Syncing;
			destination.CustomerGroup_Syncing = source.CustomerGroup_Syncing;
			destination.CustomerTitle_Syncing = source.CustomerTitle_Syncing;
			destination.Visit_Syncing = source.Visit_Syncing;
			destination.ProductVahed_Syncing = source.ProductVahed_Syncing;
			destination.TafsilLevel_Syncing = source.TafsilLevel_Syncing;
			destination.ProductGroup_Syncing = source.ProductGroup_Syncing;
			destination.Depot_Syncing = source.Depot_Syncing;
			destination.Ticket_Syncing = source.Ticket_Syncing;
			destination.TicketDet_Syncing = source.TicketDet_Syncing;
			destination.TicketPart_Syncing = source.TicketPart_Syncing;
			destination.TicketStatus_Syncing = source.TicketStatus_Syncing;
			destination.Desc_Syncing = source.Desc_Syncing;
			destination.AghsatAshkhas_Syncing = source.AghsatAshkhas_Syncing;
			destination.AghsatShakhsi_Syncing = source.AghsatShakhsi_Syncing;
			destination.UserLog_Syncing = source.UserLog_Syncing;
			destination.Kol_Syncing = source.Kol_Syncing;
			destination.Moein_Syncing = source.Moein_Syncing;
			destination.Tafsil_Syncing = source.Tafsil_Syncing;
			destination.ProductAttribute_Syncing = source.ProductAttribute_Syncing;
			destination.PrdAnbarGardani_Syncing = source.PrdAnbarGardani_Syncing;
			destination.Sanad = source.Sanad_Syncing;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\SyncInformation.cs", 528);
		}
		return Task.FromResult(destination);
	}

	public Task<List<SyncInformation>> MapListAsync(string contextGuid, IEnumerable<SyncInformation> source, List<SyncInformation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public bool Equals(SyncInformation other)
	{
		if (other == null)
		{
			return false;
		}
		if (LastPrice != other.LastPrice)
		{
			return false;
		}
		if (ProductGroup != other.ProductGroup)
		{
			return false;
		}
		if (ProductVahed != other.ProductVahed)
		{
			return false;
		}
		if (Depot != other.Depot)
		{
			return false;
		}
		if (CustomerGroup != other.CustomerGroup)
		{
			return false;
		}
		if (CustomerTitle != other.CustomerTitle)
		{
			return false;
		}
		if (Setting != other.Setting)
		{
			return false;
		}
		if (User != other.User)
		{
			return false;
		}
		if (FileInfo != other.FileInfo)
		{
			return false;
		}
		if (Delivery != other.Delivery)
		{
			return false;
		}
		if (News != other.News)
		{
			return false;
		}
		if (HyperLink != other.HyperLink)
		{
			return false;
		}
		if (ContactUs != other.ContactUs)
		{
			return false;
		}
		if (Order != other.Order)
		{
			return false;
		}
		if (Reception != other.Reception)
		{
			return false;
		}
		if (Pardakht != other.Pardakht)
		{
			return false;
		}
		if (AghsatShakhsi != other.AghsatShakhsi)
		{
			return false;
		}
		if (CallingLog != other.CallingLog)
		{
			return false;
		}
		if (AghsatAshkhas != other.AghsatAshkhas)
		{
			return false;
		}
		if (ViewCheckM != other.ViewCheckM)
		{
			return false;
		}
		if (ViewCheckSh != other.ViewCheckSh)
		{
			return false;
		}
		if (TafsilLevel != other.TafsilLevel)
		{
			return false;
		}
		if (Visit != other.Visit)
		{
			return false;
		}
		if (Kol != other.Kol)
		{
			return false;
		}
		if (Moein != other.Moein)
		{
			return false;
		}
		if (Tafsil != other.Tafsil)
		{
			return false;
		}
		if (ProductAttribute != other.ProductAttribute)
		{
			return false;
		}
		if (Product != other.Product)
		{
			return false;
		}
		if (Customer != other.Customer)
		{
			return false;
		}
		if (HesabTree != other.HesabTree)
		{
			return false;
		}
		if (UserLog != other.UserLog)
		{
			return false;
		}
		if (Account != other.Account)
		{
			return false;
		}
		return true;
	}

	public static bool operator ==(SyncInformation a, SyncInformation b)
	{
		if ((object)a == null && (object)b == null)
		{
			return true;
		}
		if ((object)a == null || (object)b == null)
		{
			return false;
		}
		return a.Equals(b);
	}

	public static bool operator !=(SyncInformation a, SyncInformation b)
	{
		if ((object)a == null && (object)b == null)
		{
			return false;
		}
		if ((object)a == null || (object)b == null)
		{
			return true;
		}
		return !a.Equals(b);
	}
}
