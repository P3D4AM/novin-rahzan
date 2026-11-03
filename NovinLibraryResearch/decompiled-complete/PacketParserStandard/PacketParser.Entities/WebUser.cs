using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.WordPress;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Host;

namespace PacketParser.Entities;

[Serializable]
public class WebUser : Serializable<WebUser>
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/AdminGps/";

	private short _depId;

	private LastSavedNumbers _lastSaved;

	private WebsitePriceDiscount wordPressDiscountedPriceSetting = new WebsitePriceDiscount();

	private WebsitePriceDiscount wordPressRegularPriceSetting = new WebsitePriceDiscount();

	[PersianNameAttribute.PersianName("شماره شروع شماره منحصر به فرد مالیاتی")]
	public int StartTaxOrderId { get; set; }

	[PersianNameAttribute.PersianName("شناسه یکتای حافظه مالیاتی")]
	public string TaxServiceClientId { get; set; } = "";


	[PersianNameAttribute.PersianName("خط فرستنده پیامک")]
	public string SMSLine { get; set; } = "";


	[PersianNameAttribute.PersianName("نام کاربری پنل")]
	public string PanelUName { get; set; } = "";


	[PersianNameAttribute.PersianName("رمز عبور پنل")]
	public string PanelPass { get; set; } = "";


	[PersianNameAttribute.PersianName("دامنه پنل")]
	public string PanelDomain { get; set; }

	[PersianNameAttribute.PersianName("نام")]
	public string Name { get; set; } = "";


	[PersianNameAttribute.PersianName("پسورد")]
	public string Pass { get; set; } = "";


	[PersianNameAttribute.PersianName("موبایل")]
	public string Mob { get; set; } = "";


	[PersianNameAttribute.PersianName("توضیحات")]
	public string Description { get; set; } = "";


	[PersianNameAttribute.PersianName("ایمیل")]
	public string Mail { get; set; } = "";


	[PersianNameAttribute.PersianName("کاربر تست")]
	public bool IsTest { get; set; }

	[PersianNameAttribute.PersianName("وضعیت")]
	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[PersianNameAttribute.PersianName("تاریخ انقضا")]
	[JsonIgnore]
	public string ExpireDateSh => ExpireDate.MiladiToCalture();

	[PersianNameAttribute.PersianName("تاریخ ثبت")]
	public DateTime DateRegister { get; set; } = DateTime.Now;


	[PersianNameAttribute.PersianName("اخرین لاگین")]
	public DateTime LastLogin { get; set; } = DateTime.Now;


	[PersianNameAttribute.PersianName("Guidانبار پیشرفته")]
	public Guid DefDepotGuid { get; set; }

	[PersianNameAttribute.PersianName("Guidکاربر")]
	public Guid CashierGuid { get; set; }

	[PersianNameAttribute.PersianName("Guidکاربر حسابداری")]
	public Guid UserAccGuid { get; set; }

	[PersianNameAttribute.PersianName("Guidدیتابیس")]
	public Guid DBGuid { get; set; }

	[PersianNameAttribute.PersianName("Guidشعبه")]
	public Guid? BranchGuid { get; set; }

	[PersianNameAttribute.PersianName("نام و نام خانوادگی")]
	public string UserFullName { get; set; }

	[PersianNameAttribute.PersianName("ایدی انبار پیش فرض")]
	public int DefDepotId { get; set; }

	[JsonIgnore]
	[PersianNameAttribute.PersianName("تاریخ ثبت نام")]
	public string DateRegisterSh => DateRegister.MiladiToCalture();

	[JsonIgnore]
	[XmlIgnore]
	[PersianNameAttribute.PersianName("اخرین لاگین")]
	public string LastLoginSh
	{
		get
		{
			return LastLogin.MiladiToCalture();
		}
		set
		{
		}
	}

	[PersianNameAttribute.PersianName("نوع کاربر")]
	public ENSource Type { get; set; }

	[PersianNameAttribute.PersianName("نوع سرویس")]
	public SMSSendType SmsType { get; set; }

	[PersianNameAttribute.PersianName("وضعیت سرور")]
	public ServerStatusForClient ServerStatus { get; set; }

	[JsonIgnore]
	[PersianNameAttribute.PersianName("وضعیت")]
	public ReturnedState State
	{
		get
		{
			int num = 0;
			if (ExpireDate > DateTime.Now)
			{
				num = checked((int)Math.Floor((ExpireDate - DateTime.Now).TotalDays));
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

	public string PC_Serial { get; set; } = "";


	public string Server_Parameters { get; set; } = "";


	public int Id { get; set; }

	public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(7.0);


	public string TypeDisplay => Type.GetDisplay();

	[Key]
	[DisplayName("Guid")]
	public Guid Guid { get; set; } = Guid.NewGuid();


	public long AndroidIme { get; set; }

	public DateTime Modified { get; set; }

	public short DepId
	{
		get
		{
			return _depId;
		}
		set
		{
			if (_depId > 0 && value != _depId)
			{
				throw new Exception("دپارتمان کاربر قابل تغییر نیست");
			}
			_depId = value;
		}
	}

	[PersianNameAttribute.PersianName("ذخیره لیست شماره")]
	public LastSavedNumbers LastSaved
	{
		get
		{
			return _lastSaved ?? (_lastSaved = new LastSavedNumbers());
		}
		set
		{
			_lastSaved = value;
		}
	}

	[PersianNameAttribute.PersianName("تنظیمات سینک وردپرس")]
	public WooSyncSettings WordPressSyncSettings { get; set; }

	[PersianNameAttribute.PersianName("تنظیمات پایه وردپرس")]
	public WooBaseSettings WordPressBasicSettings { get; set; }

	[PersianNameAttribute.PersianName("تنظیمات کاربر باجه")]
	public BajeSettings BajeUsersSettings { get; set; }

	[PersianNameAttribute.PersianName("تنظیمات حسابیار")]
	public HesabyarSettings HesabyarUsersSettings { get; set; }

	[PersianNameAttribute.PersianName("تنظمات تلگرام")]
	public WebTelegramConfig WordPressTransferDataLogger { get; set; }

	[PersianNameAttribute.PersianName("لیست متا دیتا کاربر")]
	public List<WebUserDepMetaData> ListMetaData { get; set; }

	[PersianNameAttribute.PersianName("تخفیفات قیمت سایت")]
	public WebsitePriceDiscount WordPressDiscountedPriceSetting
	{
		get
		{
			if (wordPressDiscountedPriceSetting == null)
			{
				wordPressDiscountedPriceSetting = new WebsitePriceDiscount
				{
					Type = EnPriceType.sell1
				};
			}
			return wordPressDiscountedPriceSetting;
		}
		set
		{
			wordPressDiscountedPriceSetting = value;
		}
	}

	[PersianNameAttribute.PersianName("تخفیفات ترتیبی ورد پرس")]
	public WebsitePriceDiscount WordPressReqularPriceSetting
	{
		get
		{
			if (wordPressRegularPriceSetting == null)
			{
				wordPressRegularPriceSetting = new WebsitePriceDiscount
				{
					Type = EnPriceType.Regular
				};
			}
			return wordPressRegularPriceSetting;
		}
		set
		{
			wordPressRegularPriceSetting = value;
		}
	}

	[PersianNameAttribute.PersianName("لاگین مودیان")]
	public WebTaxLogin TaxApiConfig { get; set; }

	public static async Task<List<WebUser>> GetAllFromApiAsync(CancellationToken token = default(CancellationToken))
	{
		List<WebUser> response = new List<WebUser>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUser>>> returnedSaveFuncInfoWithValue = await (apiUrl + "User_List").GetFromApi<List<WebUser>>(token, 3);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return response;
			}
			response = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 134);
		}
		return response;
	}

	public static async Task<WebUser> GetFromApiAsync(CancellationToken token = default(CancellationToken))
	{
		WebUser response = new WebUser();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> returnedSaveFuncInfoWithValue = await (apiUrl + "User_List").GetFromApi<WebUser>(token, 3);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return response;
			}
			response = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 146);
		}
		return response;
	}

	public WebUser Clone()
	{
		WebUser obj = (WebUser)MemberwiseClone();
		obj.LastSaved = LastSaved?.Clone();
		obj.WordPressSyncSettings = WordPressSyncSettings?.Clone();
		obj.WordPressBasicSettings = WordPressBasicSettings?.Clone();
		obj.BajeUsersSettings = BajeUsersSettings?.Clone();
		obj.HesabyarUsersSettings = HesabyarUsersSettings?.Clone();
		obj.WordPressDiscountedPriceSetting = WordPressDiscountedPriceSetting?.Clone();
		obj.WordPressReqularPriceSetting = WordPressReqularPriceSetting?.Clone();
		obj.TaxApiConfig = TaxApiConfig?.Clone();
		return obj;
	}

	public string CheckEquals(WebUser user)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			if (user.IsTest != IsTest)
			{
				stringBuilder.AppendLine("تغییر وضعیت در کاربر تست");
			}
			if (user.Id != Id)
			{
				stringBuilder.AppendLine("تغییر در شناسه کاربر");
			}
			if (user.Name != Name)
			{
				stringBuilder.AppendLine("تغییر نام کاربر");
			}
			if (user.Pass != Pass)
			{
				stringBuilder.AppendLine("تغییر رمز عبور کاربر");
			}
			if (user.Description != Description)
			{
				stringBuilder.AppendLine("تغییر توضیحات کاربر");
			}
			if (user.ExpireDate != ExpireDate)
			{
				stringBuilder.AppendLine("تغییر تاریخ انقضاء");
			}
			if (user.DefDepotId != DefDepotId)
			{
				stringBuilder.AppendLine("تغییر DefDepotId");
			}
			if (user.PanelDomain != PanelDomain)
			{
				stringBuilder.AppendLine("تغییر دامنه پنل");
			}
			if (user.PanelPass != PanelPass)
			{
				stringBuilder.AppendLine("تغییر رمز پنل");
			}
			if (user.Mob != Mob)
			{
				stringBuilder.AppendLine("تغییر موبایل");
			}
			if (user.UserFullName != UserFullName)
			{
				stringBuilder.AppendLine("تغییر نام و نام خانوادگی کاربر");
			}
			if (user.PanelUName != PanelUName)
			{
				stringBuilder.AppendLine("تغییر نام کاربری پنل");
			}
			if (user.Mail != Mail)
			{
				stringBuilder.AppendLine("تغییر آدرس ایمیل");
			}
			if (user.SMSLine != SMSLine)
			{
				stringBuilder.AppendLine("تغییر خط پیامک");
			}
			if (user.DateRegister != DateRegister)
			{
				stringBuilder.AppendLine("تغییر تاریخ ثبت نام");
			}
			if (user.Server_Parameters != Server_Parameters)
			{
				stringBuilder.AppendLine("تغییر Server_Parameters");
			}
			if (user.Type != Type)
			{
				stringBuilder.AppendLine("تغییر نوع کاربر");
			}
			if (user.TaxServiceClientId != TaxServiceClientId)
			{
				stringBuilder.AppendLine("تغییر شناسه یکتای حافظه مالیاتی");
			}
			if (user.StartTaxOrderId != StartTaxOrderId)
			{
				stringBuilder.AppendLine("تغییر آغاز شناسه منحصر به فرد صورت حساب مالیات");
			}
			if (user.SmsType != SmsType)
			{
				stringBuilder.AppendLine("تغییر نوع پیامک");
			}
			if (user.WordPressDiscountedPriceSetting.RoundedPrice != WordPressDiscountedPriceSetting.RoundedPrice)
			{
				stringBuilder.AppendLine("تغییر وضعیت کسر ریال");
			}
			if (user.WordPressDiscountedPriceSetting.Type != WordPressDiscountedPriceSetting.Type)
			{
				stringBuilder.AppendLine("تغییر وضعیت سطح قیمت تخفیف خورده");
			}
			if (user.WordPressReqularPriceSetting.Type != WordPressReqularPriceSetting.Type)
			{
				stringBuilder.AppendLine("تغییر وضعیت سطح قیمت عادی");
			}
			if (user.WordPressBasicSettings?.Ck != WordPressBasicSettings?.Ck)
			{
				stringBuilder.AppendLine("تغییر CK");
			}
			if (user.WordPressBasicSettings?.Cs != WordPressBasicSettings?.Cs)
			{
				stringBuilder.AppendLine("تغییر CS");
			}
			if (user.WordPressBasicSettings?.HostUrl != WordPressBasicSettings?.HostUrl)
			{
				stringBuilder.AppendLine("تغییر ادرس هاست");
			}
			if (user.WordPressBasicSettings?.CPanelUserName != WordPressBasicSettings?.CPanelUserName)
			{
				stringBuilder.AppendLine("تغییر نام کاربری هاست");
			}
			if (user.WordPressBasicSettings?.CPanelPassword != WordPressBasicSettings?.CPanelPassword)
			{
				stringBuilder.AppendLine("تغییر رمز عبور هاست");
			}
			if (user.WordPressBasicSettings?.AdminUserName != WordPressBasicSettings?.AdminUserName)
			{
				stringBuilder.AppendLine("تغییر نام کاربری ادمین هاست");
			}
			if (user.WordPressBasicSettings?.AdminPassword != WordPressBasicSettings?.AdminPassword)
			{
				stringBuilder.AppendLine("تغییر رمز عبور ادمین");
			}
			if (user.WordPressBasicSettings?.WPAdminAddress != WordPressBasicSettings?.WPAdminAddress)
			{
				stringBuilder.AppendLine("تغییر ادرس پنل ادمین ورد پرس");
			}
			if (user.WordPressSyncSettings.Product.SyncAll != WordPressSyncSettings.Product.SyncAll)
			{
				stringBuilder.AppendLine("تغییر وضعیت تبادلات تمام کالا ها");
			}
			if (user.WordPressSyncSettings.Product.Dimensions != WordPressSyncSettings.Product.Dimensions)
			{
				stringBuilder.AppendLine("تغییر ابعاد کالا(طول/عرض/ارتفاع)");
			}
			if (user.WordPressSyncSettings.Product.ChangeAmount != WordPressSyncSettings.Product.ChangeAmount)
			{
				stringBuilder.AppendLine("تغییر وضعیت موجودی کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeSku != WordPressSyncSettings.Product.ChangeSku)
			{
				stringBuilder.AppendLine("تغییر وضعیت SKU");
			}
			if (user.WordPressSyncSettings.Product.ChangeSlug != WordPressSyncSettings.Product.ChangeSlug)
			{
				stringBuilder.AppendLine("تغییر وضعیت SLUG");
			}
			if (user.WordPressSyncSettings.Product.ChangeQuickText != WordPressSyncSettings.Product.ChangeQuickText)
			{
				stringBuilder.AppendLine("تغییر وضعیت توضیح مختصر کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeDiscount != WordPressSyncSettings.Product.ChangeDiscount)
			{
				stringBuilder.AppendLine("تغییر وضعیت تخفیف کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeFullContent != WordPressSyncSettings.Product.ChangeFullContent)
			{
				stringBuilder.AppendLine("تغییر وضعیت توضیحات کامل کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeGroup != WordPressSyncSettings.Product.ChangeGroup)
			{
				stringBuilder.AppendLine("تغییر وضعیت گروه کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeName != WordPressSyncSettings.Product.ChangeName)
			{
				stringBuilder.AppendLine("تغییر وضعیت نام کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangePicture != WordPressSyncSettings.Product.ChangePicture)
			{
				stringBuilder.AppendLine("تغییر وضعیت تصویر کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangePrice != WordPressSyncSettings.Product.ChangePrice)
			{
				stringBuilder.AppendLine("تغییر وضعیت قیمت کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeAttributes != WordPressSyncSettings.Product.ChangeAttributes)
			{
				stringBuilder.AppendLine("تغییر وضعیت ویژگی واحد");
			}
			if (user.WordPressSyncSettings.Product.ChangeTechInfo != WordPressSyncSettings.Product.ChangeTechInfo)
			{
				stringBuilder.AppendLine("تغییر وضعیت مشخصات فنی");
			}
			if (user.WordPressSyncSettings.Product.Add != WordPressSyncSettings.Product.Add)
			{
				stringBuilder.AppendLine("تغییر وضعیت افزودن کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangeWeight != WordPressSyncSettings.Product.ChangeWeight)
			{
				stringBuilder.AppendLine("تغییر وضعیت تغییر وزن");
			}
			if (user.WordPressSyncSettings.Product.ChangeTaxClass != WordPressSyncSettings.Product.ChangeTaxClass)
			{
				stringBuilder.AppendLine("تغییر وضعیت کلاس مالیاتی");
			}
			if (user.WordPressSyncSettings.Product.Delete != WordPressSyncSettings.Product.Delete)
			{
				stringBuilder.AppendLine("تغییر وضعیت حذف کالا");
			}
			if (user.WordPressSyncSettings.Product.ChangePublishStatus != WordPressSyncSettings.Product.ChangePublishStatus)
			{
				stringBuilder.AppendLine("تغییر وضعیت TYPE");
			}
			if (user.WordPressSyncSettings.ProductGroup.SyncAll != WordPressSyncSettings.ProductGroup.SyncAll)
			{
				stringBuilder.AppendLine("تغییر وضعیت تبادلات گروه کالا ها");
			}
			if (user.WordPressSyncSettings.ProductGroup.ChangeCode != WordPressSyncSettings.ProductGroup.ChangeCode)
			{
				stringBuilder.AppendLine("تغییر وضعیت کد گروه کالا");
			}
			if (user.WordPressSyncSettings.ProductGroup.ChangeDescription != WordPressSyncSettings.ProductGroup.ChangeDescription)
			{
				stringBuilder.AppendLine("تغییر وضعیت توضیحات گروه کالا");
			}
			if (user.WordPressSyncSettings.ProductGroup.ChangeName != WordPressSyncSettings.ProductGroup.ChangeName)
			{
				stringBuilder.AppendLine("تغییر وضعیت نام گروه کالا");
			}
			if (user.WordPressSyncSettings.ProductGroup.ChangeParent != WordPressSyncSettings.ProductGroup.ChangeParent)
			{
				stringBuilder.AppendLine("تغییر وضعیت والد گروه کالا");
			}
			if (user.WordPressSyncSettings.ProductGroup.ChangePicture != WordPressSyncSettings.ProductGroup.ChangePicture)
			{
				stringBuilder.AppendLine("تغییر وضعیت تصویر گروه کالا");
			}
			if (user.WordPressSyncSettings.ProductGroup.Add != WordPressSyncSettings.ProductGroup.Add)
			{
				stringBuilder.AppendLine("تغییر وضعیت افزودن گروه کالا");
			}
			if (user.WordPressSyncSettings.Customer.SyncAll != WordPressSyncSettings.Customer.SyncAll)
			{
				stringBuilder.AppendLine("تغییر وضعیت تبادلات اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeCode != WordPressSyncSettings.Customer.ChangeCode)
			{
				stringBuilder.AppendLine("تغییر وضعیت کد اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeDescription != WordPressSyncSettings.Customer.ChangeDescription)
			{
				stringBuilder.AppendLine("تغییر وضعیت توضیحات اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeEmail != WordPressSyncSettings.Customer.ChangeEmail)
			{
				stringBuilder.AppendLine("تغییر وضعیت ایمیل اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeFamily != WordPressSyncSettings.Customer.ChangeFamily)
			{
				stringBuilder.AppendLine("تغییر وضعیت نام خانوادگی اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeName != WordPressSyncSettings.Customer.ChangeName)
			{
				stringBuilder.AppendLine("تغییر وضعیت نام اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangePassword != WordPressSyncSettings.Customer.ChangePassword)
			{
				stringBuilder.AppendLine("تغییر وضعیت رمز عبور اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangePicFace != WordPressSyncSettings.Customer.ChangePicFace)
			{
				stringBuilder.AppendLine("تغییر وضعیت تصویر چهره اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangePicSign != WordPressSyncSettings.Customer.ChangePicSign)
			{
				stringBuilder.AppendLine("تغییر وضعیت تصویر امضای اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeUsername != WordPressSyncSettings.Customer.ChangeUsername)
			{
				stringBuilder.AppendLine("تغییر وضعیت تغییر نام کاربری اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.Add != WordPressSyncSettings.Customer.Add)
			{
				stringBuilder.AppendLine("تغییر وضعیت افزودن اشخاص");
			}
			if (user.WordPressSyncSettings.Customer.ChangeRole != WordPressSyncSettings.Customer.ChangeRole)
			{
				stringBuilder.AppendLine("تغییر وضعیت تغییر نقش کاربر برای اشخاص");
			}
			if (user.WordPressSyncSettings.ProductProperty.SyncAll != WordPressSyncSettings.ProductProperty.SyncAll)
			{
				stringBuilder.AppendLine("تغییر وضعیت تبادلات ویژگی کالا");
			}
			if (user.WordPressSyncSettings.ProductProperty.Add != WordPressSyncSettings.ProductProperty.Add)
			{
				stringBuilder.AppendLine("تغییر وضعیت افزودن ویژگی کالا");
			}
			if (user.WordPressSyncSettings.ProductProperty.Update != WordPressSyncSettings.ProductProperty.Update)
			{
				stringBuilder.AppendLine("تغییر وضعیت بروزرسانی ویژگی کالا");
			}
			if (user.WordPressSyncSettings.ProductProperty.Delete != WordPressSyncSettings.ProductProperty.Delete)
			{
				stringBuilder.AppendLine("تغییر وضعیت حذف ویژگی کالا");
			}
			if (user.WordPressSyncSettings.ProductVariety.SyncAll != WordPressSyncSettings.ProductVariety.SyncAll)
			{
				stringBuilder.AppendLine("تغییر وضعیت تبادلات تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.Add != WordPressSyncSettings.ProductVariety.Add)
			{
				stringBuilder.AppendLine("تغییر وضعیت افزودن تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.Delete != WordPressSyncSettings.ProductVariety.Delete)
			{
				stringBuilder.AppendLine("تغییر وضعیت حذف تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeSku != WordPressSyncSettings.ProductVariety.ChangeSku)
			{
				stringBuilder.AppendLine("تغییر وضعیت SKU تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeTechInfo != WordPressSyncSettings.ProductVariety.ChangeTechInfo)
			{
				stringBuilder.AppendLine("تغییر وضعیت مشخصات فنی تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeName != WordPressSyncSettings.ProductVariety.ChangeName)
			{
				stringBuilder.AppendLine("تغییر وضعیت تغغیر نام تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeAmount != WordPressSyncSettings.ProductVariety.ChangeAmount)
			{
				stringBuilder.AppendLine("تغییر وضعیت موجودی تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeDiscount != WordPressSyncSettings.ProductVariety.ChangeDiscount)
			{
				stringBuilder.AppendLine("تغییر وضعیت تخفیف تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeFullContent != WordPressSyncSettings.ProductVariety.ChangeFullContent)
			{
				stringBuilder.AppendLine("تغییر وضعیت محتوی کامل تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangePicture != WordPressSyncSettings.ProductVariety.ChangePicture)
			{
				stringBuilder.AppendLine("تغییر وضعیت تصویر تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangePublishStatus != WordPressSyncSettings.ProductVariety.ChangePublishStatus)
			{
				stringBuilder.AppendLine("تغییر وضعیت انتشار تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeQuickText != WordPressSyncSettings.ProductVariety.ChangeQuickText)
			{
				stringBuilder.AppendLine("تغییر وضعیت محتوی مختصر تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangeWeight != WordPressSyncSettings.ProductVariety.ChangeWeight)
			{
				stringBuilder.AppendLine("تغییر وضعیت وزن تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.Dimensions != WordPressSyncSettings.ProductVariety.Dimensions)
			{
				stringBuilder.AppendLine("تغییر وضعیت ابعاد(طول/عرض/ارتفاع) تنوع کالایی");
			}
			if (user.WordPressSyncSettings.ProductVariety.ChangePrice != WordPressSyncSettings.ProductVariety.ChangePrice)
			{
				stringBuilder.AppendLine("تغییر وضعیت تغییر قیمت تنوع کالایی");
			}
			if (user.BajeUsersSettings.BajeLevel != BajeUsersSettings?.BajeLevel)
			{
				stringBuilder.AppendLine("تغییر سطح باجه");
			}
			if (user.HesabyarUsersSettings?.BajeLevel != HesabyarUsersSettings?.BajeLevel)
			{
				stringBuilder.AppendLine("تغییر سطح حسابیار");
			}
			if (user.BajeUsersSettings?.Tolid != BajeUsersSettings?.Tolid)
			{
				stringBuilder.AppendLine("تغییر وضعیت تولید باجه");
			}
			if (user.BajeUsersSettings?.Barcode != BajeUsersSettings?.Barcode)
			{
				stringBuilder.AppendLine("تغییر وضعیت بارکد باجه");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckEquals", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 261);
		}
		return stringBuilder.ToString();
	}

	public static async Task<List<WebUser>> GetAllAsync(ENSource source, CancellationToken token = default(CancellationToken))
	{
		List<WebUser> response = new List<WebUser>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"source",
				$"{(short)source}"
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebUser>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "Webservice/User/GetAll").GetFromApi<List<WebUser>>(token, 3, headers);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return response;
			}
			response = returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 274);
		}
		return response;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>>> GetAsync(string depUserName, ENSource source, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>>();
			try
			{
				string url = HttpHelpers.ApiUrl + "Webservice/User/GetByDepartmentUserName";
				Dictionary<string, string> obj = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					},
					{ "depUserName", depUserName }
				};
				short num = (short)source;
				obj.Add("source", num.ToString());
				Dictionary<string, string> headers = obj;
				ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> returnedSaveFuncInfoWithValue = await url.GetFromApi<WebUser>(token, 1, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 292);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 297);
				ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 298);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<Guid>>> GetTafsilGuidAsync(string userName, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<Guid>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<Guid>>();
			try
			{
				string url = HttpHelpers.ApiUrl + "Webservice/User/GetTafsilGuidByUserName";
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{
						"adminUserName",
						DepAdmin.UserName
					},
					{
						"adminPassword",
						DepAdmin.Password
					},
					{ "user", userName }
				};
				ReturnedSaveFuncInfoWithValue<WebResponse<Guid>> returnedSaveFuncInfoWithValue = await url.GetFromApi<Guid>(token, 1, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 316);
				ret.value = returnedSaveFuncInfoWithValue.value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 321);
				ret.AddReturnedValue(ex, "", "GetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUser.cs", 322);
			}
			return ret;
		}
	}
}
