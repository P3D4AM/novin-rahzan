using System;
using System.Collections.Generic;
using PacketParser.Entities.Accounting;
using PacketParser.Settings.Host;

namespace PacketParser.Entities.Interfaces;

public interface IUserInformation
{
	string CacheApiURL { get; set; }

	short DepId { get; set; }

	string PaymentURL { get; set; }

	string UserName { get; set; }

	string UserEmail { get; set; }

	string UserMobile { get; set; }

	int UserId { get; set; }

	Guid UserAccGuid { get; set; }

	Guid CashierGuid { get; set; }

	Guid UserDepGuid { get; set; }

	DateTime UserRegisterDateM { get; set; }

	DateTime UserExpireDateM { get; set; }

	string RelativePath { get; set; }

	string UserDescription { get; set; }

	bool GpsEnable { get; set; }

	string SiteUrl { get; set; }

	SiteType SiteType { get; set; }

	bool AndroidPrint { get; set; }

	Sync SyncInfo { get; set; }

	ENSource Source { get; set; }

	LastSavedNumbers LastSaved { get; set; }

	WebCus CustomerInfo { get; set; }

	WebOrder Order { get; set; }

	List<AndroidPrintStyle> AndroidPrintStyles { get; set; }

	bool IsHaveTicketting { get; set; }

	WebHesabTree HesabInfo { get; set; }

	string DepartmentDescription { get; set; }

	string MetaDescription { get; set; }

	string MetaKeyword { get; set; }

	string Script { get; set; }

	string ScriptHeader { get; set; }

	string ScriptFooter { get; set; }

	string ScriptCounter { get; set; }

	string ScriptEnamad { get; set; }

	List<SocialNetwork> LstSocialNetwork { get; set; }

	DateTime CurrentServerTime { get; set; }

	byte[] HashedPassword { get; set; }

	List<MediaLinks> LstMediaLinks { get; set; }

	List<UserAccCompact> UserAccCompacts { get; set; }

	Guid? TafsilGuid { get; set; }

	Guid UserAccountingGuid { get; set; }

	Guid DefaultCusGroup { get; set; }

	bool SyncsKolMoeinTafsil { get; set; }

	BajeSettings BajeSettings { get; set; }

	HesabyarSettings HesabyarSettings { get; set; }
}
