using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Accounting;

namespace PacketParser.Entities;

public class WebDepartment : Serializable<WebDepartment>
{
	private static string apiurl = HttpHelpers.ApiUrl + "api/";

	private List<MediaLinks> listMediaLinks;

	private List<SocialNetwork> listSocialNetworks;

	public const int NpHozorClientId = 1248;

	public const int NpAccClientId = 2131;

	private bool _useOldAndroid = true;

	public Guid TafsilGuid { get; set; }

	public short Id { get; set; }

	public string Name { get; set; } = "";


	public string URL { get; set; } = "";


	public SiteType SiteType { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string StrType
	{
		get
		{
			return SiteType.GetDisplay();
		}
		set
		{
		}
	}

	public short AndroidCount { get; set; }

	public short BajeCount { get; set; }

	public short PrintCount { get; set; }

	public bool GPS { get; set; }

	public bool RestApi { get; set; }

	public bool WebSiteWordPress { get; set; }

	public bool CloudeAttendance { get; set; }

	public List<DepartmentPaymentGatewayRelation> PaymentGatewayRelations { get; set; }

	public EnDepartmentStatus StTaeed { get; set; }

	public string Script { get; set; } = "";


	public string Script_Header { get; set; } = "";


	public string Script_Footer { get; set; } = "";


	public string Script_Enamad { get; set; } = "";


	public string Script_Counter { get; set; } = "";


	public EnBKWebService BKWebService { get; set; }

	public bool UseOldAndroid
	{
		get
		{
			if (AndroidCount <= 0)
			{
				return false;
			}
			return _useOldAndroid;
		}
		set
		{
			_useOldAndroid = value;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public bool HaveBKWebService
	{
		get
		{
			return BKWebService == EnBKWebService.Yes;
		}
		set
		{
		}
	}

	public TypeSefareshgir Android_Sefareshgir { get; set; }

	public bool HaveAccountingWebService
	{
		get
		{
			if (AndroidCount <= 0 && BajeCount <= 0 && Android_Sefareshgir == TypeSefareshgir.None && SiteType == SiteType.NOSite)
			{
				return WebSiteWordPress;
			}
			return true;
		}
		set
		{
		}
	}

	[JsonIgnore]
	public string PaymentURL
	{
		get
		{
			using (new JaegerMonitor("PaymentURL", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs"))
			{
				try
				{
					string text = URL.Trim().Replace("//", "@");
					if (text.Trim().Length <= 0)
					{
						return "";
					}
					if (text.Contains("/"))
					{
						string text2 = text.Split("/".ToCharArray())[0];
						if (text2 == null || text2.Trim() == "")
						{
							return "";
						}
						return text2.Replace("@", "//") + "/apppayment.aspx";
					}
					if (text == null || text.Trim() == "")
					{
						return "";
					}
					return text.Replace("@", "//") + "/apppayment.aspx";
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "PaymentURL", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 91);
				}
				return "";
			}
		}
	}

	public string Address { get; set; } = "";


	public string Tel { get; set; } = "";


	public string Mob { get; set; } = "";


	public string Fax { get; set; } = "";


	public string Zip { get; set; } = "";


	public string Description { get; set; } = "";


	public short MaxUsers { get; set; } = 2;


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime Date { get; set; } = DateTime.Now;


	[JsonIgnore]
	[XmlIgnore]
	public string DateSH
	{
		get
		{
			return Date.MiladiToCalture();
		}
		set
		{
		}
	}

	public DateTime DateExpire { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string DateExpireSh
	{
		get
		{
			return DateExpire.MiladiToCalture();
		}
		set
		{
		}
	}

	public string User { get; set; } = "";


	public string Pass { get; set; } = "";


	public string Mail { get; set; } = "";


	public int DefUserID { get; set; }

	public string Eghtesadi { get; set; } = "";


	public string SMSReceptionAdmin { get; set; } = "";


	public string SMSReceptionUser { get; set; } = "";


	public string Title { get; set; } = "";


	public string Companyinfo { get; set; } = "";


	public string CompanyFullinfo { get; set; } = "";


	public string CompanyDesc { get; set; } = "";


	public string CompanyFullDesc { get; set; } = "";


	public string SMSTEmplate_Kala { get; set; } = "";


	public string SMSTEmplate_Site { get; set; } = "";


	public string SMSTEmplate_User { get; set; } = "";


	public string SMSTEmplate_OrdeAdmin { get; set; } = "";


	public string SMSTEmplate_OrderUser { get; set; } = "";


	public string SMSTemplate_Singup { get; set; } = "کد فعالسازی ثبت نام :[کد]\r\n[نام فروشگاه]\r\n[تلفن تماس]\r\n[آدرس سایت]";


	public string KeyWords { get; set; } = "";


	public Guid Guid { get; } = Guid.NewGuid();


	public string MetaDescription { get; set; } = "";


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

	public string SMSTemplate_RecoveryPass { get; set; } = "نام کاربری: [نام کاربری]\r\nکلمه عبور: [کلمه عبور]\r\n[نام فروشگاه]\r\n[تلفن تماس]\r\n[آدرس سایت]";


	public bool SMSManagerial { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string SefareshgirLevel
	{
		get
		{
			return Android_Sefareshgir.GetDisplay();
		}
		set
		{
		}
	}

	public List<MediaLinks> LstMediaLinks
	{
		get
		{
			return listMediaLinks?.ToList();
		}
		set
		{
			listMediaLinks = value;
		}
	}

	public bool IsHaveTicketting { get; set; }

	public bool SyncsKolMoeinTafsil { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public bool IsAccountingVerion6 => SyncsKolMoeinTafsil;

	public List<WebDepaertmentMetaData> ListMetaData { get; set; }

	public ClientEntityUpdateSetting ClientEntityUpdateSetting { get; set; }

	[JsonIgnore]
	public ReturnedState State
	{
		get
		{
			int num = 0;
			if (DateExpire > DateTime.Now)
			{
				num = checked((int)Math.Floor((DateExpire - DateTime.Now).TotalDays));
			}
			if (num <= 30)
			{
				if (num > 0)
				{
					return ReturnedState.Warning;
				}
				return ReturnedState.Error;
			}
			return ReturnedState.Success;
		}
	}

	public static WebDepartment DeserializaData(string serializedData)
	{
		return serializedData.FromJson<WebDepartment>("DeserializaData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 154);
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (TafsilGuid == Guid.Empty)
			{
				return returnedSaveFuncInfo.AddError(EnSystem.None, "لطفا کاربر حسابداری  برای دپارتمان مشخص کنید", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 160);
			}
			if (string.IsNullOrEmpty(User))
			{
				return returnedSaveFuncInfo.AddError(EnSystem.None, "لطفا نام کاربری دپارتمان را مشخص کنید", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 161);
			}
			if (string.IsNullOrEmpty(Pass))
			{
				return returnedSaveFuncInfo.AddError(EnSystem.None, "لطفا رمز عبور برای دپارتمان را مشخص کنید", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 162);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 166);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 167);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<WebDepartment> GetFromApiAsync(short depid, CancellationToken token = default(CancellationToken))
	{
		WebDepartment webdep = new WebDepartment();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"depId",
				depid.ToString()
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await (apiurl + "AdminGps/DepartmentInformation").GetFromApi<WebDepartment>(token, 3, headers);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return webdep;
			}
			webdep = returnedSaveFuncInfoWithValue?.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepartment.cs", 181);
		}
		return webdep;
	}
}
