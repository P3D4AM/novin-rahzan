using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Host;

namespace PacketParser.Entities;

[Serializable]
public class WebUserInformation : Serializable<WebUserInformation>, IUserInformation
{
	private List<AndroidPrintStyle> androidPrintStyles;

	private List<SocialNetwork> listSocialNetworks;

	private List<MediaLinks> listMediaLinkses;

	private Guid _userAccGuid;

	public string CacheApiURL { get; set; } = "";


	public string PaymentURL { get; set; } = "";


	public string UserName { get; set; } = "";


	public string UserEmail { get; set; } = "";


	public string UserMobile { get; set; } = "";


	public int UserId { get; set; }

	public Guid UserAccDep { get; set; } = Guid.Empty;


	public DateTime UserRegisterDateM { get; set; } = DateTime.Now;


	public DateTime UserExpireDateM { get; set; } = DateTime.Now;


	public string RelativePath { get; set; } = "";


	public string UserDescription { get; set; } = "";


	public bool GpsEnable { get; set; }

	public string SiteUrl { get; set; } = "";


	public SiteType SiteType { get; set; }

	[DefaultValue(0)]
	public SiteType ST
	{
		get
		{
			return SiteType;
		}
		set
		{
			SiteType = value;
		}
	}

	public bool AndroidPrint { get; set; }

	public short DepId { get; set; }

	public Guid UserDepGuid { get; set; }

	public Guid CashierGuid
	{
		get
		{
			return _userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				_userAccGuid = value;
			}
		}
	}

	[Obsolete("use cashierGuid")]
	public Guid UserAccGuid
	{
		get
		{
			return _userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				_userAccGuid = value;
			}
		}
	}

	public Sync SyncInfo { get; set; }

	public ENSource Source { get; set; }

	public WebUserAcc UserAcc { get; set; }

	public WebCashier Cashier { get; set; }

	public AccGlobalSettings GlobalSettings { get; set; }

	public ActivationAbonmanSettings AbonmanSettings { get; set; }

	public LastSavedNumbers LastSaved { get; set; }

	public WebCus CustomerInfo { get; set; }

	public WebOrder Order { get; set; }

	public List<AndroidPrintStyle> AndroidPrintStyles
	{
		get
		{
			return androidPrintStyles?.ToList();
		}
		set
		{
			androidPrintStyles = value;
		}
	}

	public bool IsHaveTicketting { get; set; }

	public WebHesabTree HesabInfo { get; set; }

	public string DepartmentDescription { get; set; } = "";


	public string MetaDescription { get; set; } = "";


	public string MetaKeyword { get; set; } = "";


	public string Script { get; set; } = "";


	public string ScriptHeader { get; set; } = "";


	public string ScriptFooter { get; set; } = "";


	public string ScriptCounter { get; set; } = "";


	public string ScriptEnamad { get; set; } = "";


	public List<SocialNetwork> LstSocialNetwork
	{
		get
		{
			return listSocialNetworks?.ToList();
		}
		set
		{
			listSocialNetworks = value;
		}
	}

	public List<DepartmentPaymentGatewayRelation> PaymentGatewayRelation { get; set; }

	public DateTime CurrentServerTime { get; set; } = DateTime.Now;


	public byte[] HashedPassword { get; set; }

	public List<MediaLinks> LstMediaLinks
	{
		get
		{
			return listMediaLinkses?.ToList();
		}
		set
		{
			listMediaLinkses = value;
		}
	}

	public Guid? TafsilGuid { get; set; }

	[JsonIgnore]
	public int HesabTreeId => HesabInfo?.Id ?? 0;

	[Obsolete("", true)]
	public Guid UserAccountingGuid
	{
		get
		{
			return _userAccGuid;
		}
		set
		{
			if (value != Guid.Empty)
			{
				_userAccGuid = value;
			}
		}
	}

	public Guid DefaultCusGroup { get; set; }

	public List<UserAccCompact> UserAccCompacts { get; set; }

	public bool SyncsKolMoeinTafsil { get; set; }

	public BajeSettings BajeSettings { get; set; }

	public HesabyarSettings HesabyarSettings { get; set; }
}
