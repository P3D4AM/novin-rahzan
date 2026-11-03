using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Business.Customers;
using EntityCache.Business.Extensions;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.Customer;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;
using PacketParser.SiteManagementApiModel;
using PacketParser.TransferLayer;
using Serilog;

namespace EntityCache.Business;

public class CusBusiness : BaseBusiness<CusBusiness>, ICus, ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<CusBusiness>, IDeliverableEntity, IEditableDelivery
{
	private List<CustomerMetaDataBusiness> _metaList;

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 10);

	private SemaphoreSlim _getTafsilSemaphore = new SemaphoreSlim(1, 10);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _name;

	private string _family;

	private string _picFace;

	private string _picSign;

	private string _address1 = "";

	private string _address2 = "";

	private string _codeMeli = "";

	private string _company = "";

	private string _description;

	private string _eghtesadiNo;

	private string _job = "";

	private string _location;

	private string _password;

	private string _user;

	private string _zip;

	private Guid _visitorGuid;

	private Guid guid;

	private TafsilBusiness _tafsil;

	private List<TafsilNumberBusiness> _numberList;

	public Guid? RoleGuid { get; set; }

	public string PhoneNumbers { get; private set; }

	public override string ContextGuid { get; set; }

	public Guid Guid
	{
		get
		{
			if (guid == Guid.Empty)
			{
				guid = Guid.NewGuid();
			}
			return guid;
		}
		set
		{
			guid = value;
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value?.FixString() ?? "";
		}
	}

	public string Family
	{
		get
		{
			return _family;
		}
		set
		{
			_family = value?.FixString() ?? "";
		}
	}

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	public string StatusForDisplay
	{
		get
		{
			if (!Status)
			{
				return "غیرفعال";
			}
			return "فعال";
		}
	}

	public byte[]? ImageFaceByte { get; set; }

	[Obsolete("Use Tafsil")]
	public bool Special { get; set; }

	public string PicFace
	{
		get
		{
			return _picFace;
		}
		set
		{
			_picFace = value?.FixString() ?? "";
		}
	}

	public string PicSign
	{
		get
		{
			return _picSign;
		}
		set
		{
			_picSign = value?.FixString() ?? "";
		}
	}

	public string Address1
	{
		get
		{
			return _address1;
		}
		set
		{
			_address1 = value?.FixString() ?? "";
		}
	}

	public string Address2
	{
		get
		{
			return _address2;
		}
		set
		{
			_address2 = value?.FixString() ?? "";
		}
	}

	public string CodeMeli
	{
		get
		{
			return _codeMeli;
		}
		set
		{
			_codeMeli = value?.FixString() ?? "";
		}
	}

	public string Company
	{
		get
		{
			return _company;
		}
		set
		{
			_company = value?.FixString() ?? "";
		}
	}

	public DateTime Date { get; set; } = DateTime.Now;


	[Obsolete("Use CustomerRole", true)]
	public EnPriceType DefPrice { get; set; }

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public string EghtesadiNo
	{
		get
		{
			return _eghtesadiNo;
		}
		set
		{
			_eghtesadiNo = value?.FixString() ?? "";
		}
	}

	public decimal Etebar { get; set; }

	public decimal EtebarCheck { get; set; }

	[Obsolete("Use AreaGuid")]
	public int IdPath { get; set; }

	public Guid? AreaGuid { get; set; }

	public bool IsBazaryab { get; set; }

	public bool IsSupervisor { get; set; }

	public string Job
	{
		get
		{
			return _job;
		}
		set
		{
			_job = value?.FixString() ?? "";
		}
	}

	public string Location
	{
		get
		{
			return _location;
		}
		set
		{
			_location = value?.FixString() ?? "";
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value?.FixString() ?? "";
		}
	}

	public string Email { get; set; }

	public Guid SupervisorGuid { get; set; }

	public string FullName
	{
		get
		{
			return Name + " " + Family;
		}
		set
		{
		}
	}

	public string User
	{
		get
		{
			if (string.IsNullOrEmpty(_user))
			{
				_user = null;
			}
			return _user;
		}
		set
		{
			_user = value?.FixString() ?? "";
		}
	}

	public string Zip
	{
		get
		{
			return _zip;
		}
		set
		{
			_zip = value?.FixString() ?? "";
		}
	}

	public CustomerType CustType { get; set; }

	public short TafsilLevel { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid TitleGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; } = "";


	public string DeliveryShortMessage { get; set; } = "";


	public string HostRefrenced { get; set; } = "";


	[JsonIgnore]
	public string SendToServerStatus => ServerSt();

	public decimal BPercent { get; set; }

	public bool InPlacePayment { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public bool IsEtebari { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public TypeTax TaxType { get; set; }

	public ReturnedState ChequeBargashtiValidation { get; set; }

	public ReturnedState OrderTasvieTimeValidation { get; set; }

	public DateTime? SendToServerDate { get; set; }

	public async Task<IEnumerable<CustomerMetaDataBusiness>> GetMetaDataListAsync(bool isClear = true, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (isClear)
			{
				_metaList = new List<CustomerMetaDataBusiness>();
			}
			else if (_metaList == null)
			{
				_metaList = new List<CustomerMetaDataBusiness>();
			}
			MetaDataFilter filter = new MetaDataFilter
			{
				EntityGuid = Guid,
				EntityType = EntityPart.Customer
			};
			foreach (MetaDataBusiness item in await ClsCache.GetCache(ContextGuid).CacheRepository.MetaData.GetAllAsync(filter, token, ""))
			{
				_metaList.Add(new CustomerMetaDataBusiness(ContextGuid, item));
			}
			return _metaList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMetaDataListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 39);
			throw;
		}
	}

	public void ClearListMetaData()
	{
		_metaList?.Clear();
	}

	public void RemoveDet(CustomerMetaKey key)
	{
		try
		{
			CustomerMetaDataBusiness customerMetaDataBusiness = _metaList?.SingleOrDefault((CustomerMetaDataBusiness q) => q.MetaKey == key);
			if (customerMetaDataBusiness != null)
			{
				_metaList.Remove(customerMetaDataBusiness);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 52);
		}
	}

	public void AddDet(CustomerMetaKey key, string value, EnDataType type = EnDataType.String)
	{
		try
		{
			CustomerMetaDataBusiness customerMetaDataBusiness = new CustomerMetaDataBusiness(ContextGuid, new MetaDataBusiness(ContextGuid)
			{
				Value = value,
				DataType = type,
				EntityGuid = Guid,
				EntityType = EntityPart.Customer
			});
			customerMetaDataBusiness.MetaKey = key;
			if (_metaList == null)
			{
				_metaList = new List<CustomerMetaDataBusiness>();
			}
			_metaList.Add(customerMetaDataBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 71);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RemoveMetaAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await MetaDataBusiness.RemoveRangeAsync(ContextGuid, Guid, EntityPart.Customer, token, tranName), ReturnedState.None, "RemoveMetaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 80);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveMetaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 84);
			res.AddReturnedValue(ex, "", "RemoveMetaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 85);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> MetaTransactionDestinyAsync(string tranName, bool isRollBack)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, isRollBack, disposeContext: true, "MetaTransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 94), ReturnedState.None, "MetaTransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 94);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MetaTransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 98);
			ret.AddReturnedValue(ex, "", "MetaTransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 99);
		}
		return ret;
	}

	public static async Task<IEnumerable<CustomerMetaDataBusiness>> GetMetaDataListAsync(string contextGuid, Guid guid, CancellationToken token)
	{
		try
		{
			List<CustomerMetaDataBusiness> metaList = new List<CustomerMetaDataBusiness>();
			MetaDataFilter filter = new MetaDataFilter
			{
				EntityGuid = guid,
				EntityType = EntityPart.Customer
			};
			foreach (MetaDataBusiness item in await ClsCache.GetCache(contextGuid).CacheRepository.MetaData.GetAllAsync(filter, token, ""))
			{
				metaList.Add(new CustomerMetaDataBusiness(contextGuid, item));
			}
			return metaList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMetaDataListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 120);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync_(EnSystem system, List<TafsilNumberBusiness> lstTafsilNumbers, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return ret;
			}
			UserAccBusiness user = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAsync(token, tranName);
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (ChangeStatus == EntityChangeSt.Edited && !user.UserAccess.Customers.Customer_Update)
			{
				ret.AddError("شما دسترسی ویرایش اشخاص را ندارید.".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 135);
			}
			else if (ChangeStatus == EntityChangeSt.New && !user.UserAccess.Customers.Customer_Insert)
			{
				ret.AddError("شما دسترسی تعریف اشخاص را ندارید.".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 137);
			}
			if (!(lstTafsilNumbers?.Any() ?? false) && accGlobalSettings.Customers.Customer_WithoutNumber == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون شماره نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 140);
			}
			else if (!(lstTafsilNumbers?.Any() ?? false) && accGlobalSettings.Customers.Customer_WithoutNumber == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری شماره تماس درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 142);
			}
			if (string.IsNullOrEmpty(Name) && accGlobalSettings.Customers.Customer_WithoutName == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون نام نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 145);
			}
			else if (string.IsNullOrEmpty(Name) && accGlobalSettings.Customers.Customer_WithoutName == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری نام درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 147);
			}
			if (string.IsNullOrEmpty(Address1) && string.IsNullOrEmpty(Address2) && accGlobalSettings.Customers.Customer_WithoutAddress == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون آدرس نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 150);
			}
			else if (string.IsNullOrEmpty(Address1) && string.IsNullOrEmpty(Address2) && accGlobalSettings.Customers.Customer_WithoutAddress == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری آدرس درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 152);
			}
			if (string.IsNullOrEmpty(Email) && accGlobalSettings.Customers.Customer_WithoutEmail == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون پست الکترونیک نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 155);
			}
			else if (string.IsNullOrEmpty(Email) && accGlobalSettings.Customers.Customer_WithoutEmail == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری پست الکترونیک درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 157);
			}
			if (string.IsNullOrEmpty(User) && accGlobalSettings.Customers.Customer_WithoutUserName == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون نام کاربری نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 160);
			}
			else if (string.IsNullOrEmpty(User) && accGlobalSettings.Customers.Customer_WithoutUserName == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری نام کاربری درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 162);
			}
			CustomerMetaDataBusiness customerMetaDataBusiness = _metaList?.FirstOrDefault((CustomerMetaDataBusiness q) => q.MetaKey == CustomerMetaKey.MethodIfIntroduction);
			if (string.IsNullOrEmpty(customerMetaDataBusiness?.Value) && accGlobalSettings.Customers.Customer_WithoutMethodOfIntroduction == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون ثبت نحوه آشنایی نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 166);
			}
			else if (string.IsNullOrEmpty(customerMetaDataBusiness?.Value) && accGlobalSettings.Customers.Customer_WithoutMethodOfIntroduction == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری نحوه آشنایی درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 168);
			}
			if ((AreaGuid ?? Guid.Empty) == Guid.Empty && accGlobalSettings.Customers.Customer_WithoutArea == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون شهر و استان نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 171);
			}
			else if ((AreaGuid ?? Guid.Empty) == Guid.Empty && accGlobalSettings.Customers.Customer_WithoutArea == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری شهر و استان درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 173);
			}
			if (string.IsNullOrEmpty(CodeMeli) && accGlobalSettings.Customers.Customer_WithoutNationalCode == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون کدملی نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 176);
			}
			else if (string.IsNullOrEmpty(CodeMeli) && accGlobalSettings.Customers.Customer_WithoutNationalCode == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری کد ملی درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 178);
			}
			if (string.IsNullOrEmpty(Zip) && accGlobalSettings.Customers.Customer_WithoutZip == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون کدپستی نمی باشید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 181);
			}
			else if (string.IsNullOrEmpty(Zip) && accGlobalSettings.Customers.Customer_WithoutZip == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری کد پستی درج ننموده اید".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 183);
			}
			if (string.IsNullOrEmpty(EghtesadiNo) && accGlobalSettings.Customers.Customer_WithoutCodeEghtesadi == ReturnedState.Error && !isDeleteAction)
			{
				ret.AddError("شما مجاز به تعریف شخص بدون کد اقتصادی نمی باشید", "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 186);
			}
			else if (string.IsNullOrEmpty(EghtesadiNo) && accGlobalSettings.Customers.Customer_WithoutCodeEghtesadi == ReturnedState.Warning && !isDeleteAction)
			{
				ret.AddWarning("برای شخص جاری کد اقتصادی درج ننموده اید", "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 188);
			}
			if (accGlobalSettings.Payamak.OccasionCalendar?.Any() ?? false)
			{
				IEnumerable<OccasionCalendarModel> enumerable = accGlobalSettings.Payamak.OccasionCalendar.Where((OccasionCalendarModel q) => q.Required && q.Status);
				if (enumerable != null && enumerable.Any())
				{
					CustomerMetaDataBusiness customerMetaDataBusiness2 = _metaList?.SingleOrDefault((CustomerMetaDataBusiness q) => q.MetaKey == CustomerMetaKey.OccasionCalender);
					if (string.IsNullOrEmpty(customerMetaDataBusiness2?.Value))
					{
						await ret.AddErrorAsync("وارد کردن تاریخ برای موارد زیر اجباری است".GetTranslate() + "\r\n " + string.Join(",", enumerable.Select((OccasionCalendarModel q) => q.Title)), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 208);
					}
					else
					{
						List<OccasionCalendarModel> source = customerMetaDataBusiness2.Value.FromJson<List<OccasionCalendarModel>>("CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 198);
						foreach (OccasionCalendarModel settingOccasion in enumerable)
						{
							if (settingOccasion != null && string.IsNullOrEmpty(source.SingleOrDefault((OccasionCalendarModel q) => q.Title == settingOccasion.Title && q.Status)?.Value))
							{
								ret.AddError("وارد کردن تاریخ ".GetTranslate() + " " + settingOccasion.Title + " " + " اجباری است".GetTranslate(), "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 204);
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 214);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerMetaDataBusiness.cs", 215);
		}
		return ret;
	}

	public static async Task<long> GetNextCodeAsync(string contextGuid, Guid groupGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			EnSuggestionType suggestionMode = EnSuggestionType.FillGaps;
			CusGroupBusiness cusGroupBusiness = await CusGroupBusiness.GetAsync(contextGuid, groupGuid, token);
			return (await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.NextCodeInGroupAsync(cusGroupBusiness?.TafsilGroupGuid ?? CusGroupBusiness.DefaultTafsilLevelGuid, suggestionMode, token, "")).value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 41);
		}
		return 0L;
	}

	public static Task<ReturnedSaveFuncInfo> UpdateGroupAsync(string contextGuid, IEnumerable<Guid> customerGuids, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.UpdateGroupAsync(customerGuids, groupGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, bool updateAccount, short autoFixValidationErrorsCount, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, bool awaitForSend = false, List<TafsilNumberBusiness> tafsilNumbers = null, short tryCount = 10, string tranName = "", FileInfoBusiness picFacefile = null, FileInfoBusiness picSignfile = null, List<CusCardBusiness> cards = null, CancellationToken token = default(CancellationToken))
	{
		TaskCompletionSource<bool> saveComplationTask = new TaskCompletionSource<bool>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			_ = 25;
			ReturnedSaveFuncInfo result;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				await _connectionPoolSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				line = 1;
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CusBusiness/SaveAsync", 60000, token))
				{
					using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
					{
						bool sanadsHasBeenChanged;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 71);
						}
						else
						{
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 73), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 73);
							}
							transactionHasBeenCreated = true;
							if (!IsBazaryab && SupervisorGuid != Guid.Empty)
							{
								SupervisorGuid = Guid.Empty;
							}
							ret = await CheckValidationAsync(system, tafsilNumbers, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, autoFixValidationErrorsCount, tranName, token);
							line = 10;
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								ENSource source = ClsCache.GetCache(ContextGuid).Source;
								sanadsHasBeenChanged = false;
								if (system != EnSystem.Transfer && (system != EnSystem.SignalR || source == ENSource.Accounting))
								{
									CusBusiness oldCustomer = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
									if (oldCustomer != null)
									{
										CustomerMapper instance = CustomerMapper.Instance;
										string contextGuid = ContextGuid;
										CusBusiness source2 = this;
										WebCus destination = new WebCus();
										CancellationToken token2 = token;
										WebCus webCus = await instance.MapAsync(contextGuid, source2, destination, tranName, token2);
										ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await webCus.ComapreAsync(ContextGuid, oldCustomer, token, tranName);
										if (returnedSaveFuncInfo2.HasError)
										{
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.OtherLogsAsync(ContextGuid, system, sendToServer, token, EnLog.CustomerLog, ENAction.Edit, returnedSaveFuncInfo2.GetErrorMessage(), Guid, awaitOnSemaphore: true, tranName, saveComplationTask), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 92);
										}
										if (webCus.Name != oldCustomer.Name || webCus.Family != oldCustomer.Family || webCus.TitleGuid != oldCustomer.TitleGuid)
										{
											sanadsHasBeenChanged = true;
										}
									}
									else
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.OtherLogsAsync(ContextGuid, system, sendToServer, token, EnLog.CustomerLog, ENAction.Add, "ایجاد حساب".GetTranslate(), Guid, awaitOnSemaphore: true, tranName, saveComplationTask), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 97);
									}
								}
								if (system == EnSystem.UserInterface || (system != EnSystem.Transfer && cache.Source == ENSource.Accounting))
								{
									TafsilBusiness tafsilBusiness = await GetTafsilAsync(system, tranName, token);
									if (tafsilBusiness != null)
									{
										tafsilBusiness.Name = FullName;
										tafsilBusiness.Status = Status;
										tafsilBusiness.Description = Description;
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await tafsilBusiness.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, dis.Token, updateAccount, awaitOnSemaphore: false, autoFixValidationErrorsCount, sendToServer, awaitForSend, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 108);
									}
								}
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									if (system != EnSystem.Transfer && (system != EnSystem.SignalR || source == ENSource.Accounting) && string.IsNullOrEmpty(User) && Status)
									{
										await FillUserNameByMobileNumberAsync(tafsilNumbers, tranName, token);
										if (string.IsNullOrEmpty(User))
										{
											TafsilBusiness tafsil = await GetTafsilAsync(system, tranName, token);
											if (tafsil != null)
											{
												User = ((await CheckUserNameAsync(ContextGuid, Guid, tafsil.Code.ToString(), token, tranName)) ? tafsil.Code.ToString() : Guid.ToString());
											}
										}
									}
									line = 20;
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 127), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 127);
									if (picFacefile == null)
									{
										goto IL_0f6c;
									}
									line = 21;
									picFacefile.ServerStatus = ServerStatusForClient.None;
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await picFacefile.SaveAsync(system, autoFixValidationErrorsCount, sendToServer, priority, awaitOnSemaphore: false, tryCount, tranName, awair4send: false, dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 134);
									if (!ret.HasError)
									{
										goto IL_0f6c;
									}
									result = ret;
								}
							}
						}
						goto end_IL_01ea;
						IL_0f6c:
						line = 30;
						if (picSignfile == null)
						{
							goto IL_1083;
						}
						line = 31;
						picSignfile.ServerStatus = ServerStatusForClient.None;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await picSignfile.SaveAsync(system, autoFixValidationErrorsCount, sendToServer, priority, awaitOnSemaphore: false, tryCount, tranName, awair4send: false, dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 142);
						if (!ret.HasError)
						{
							goto IL_1083;
						}
						result = ret;
						goto end_IL_01ea;
						IL_11aa:
						line = 50;
						if (system == EnSystem.Transfer)
						{
							goto IL_13fb;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await RemoveMetaAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 156);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 157), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 157);
						if (!ret.HasError)
						{
							if (_metaList != null)
							{
								returnedSaveFuncInfo = ret;
								List<CustomerMetaDataBusiness> metaList = _metaList;
								string tranName2 = tranName;
								returnedSaveFuncInfo.AddReturnedValue(await MetaDataBusiness.SaveRangeAsync(metaList, awaitOnSemaphore: true, token, 10, tranName2), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 160);
							}
							goto IL_13fb;
						}
						result = ret;
						goto end_IL_01ea;
						IL_13fb:
						Modified = DateTime.Now;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 163);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							line = 60;
							if (cards != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await CusCardBusiness.SaveRangeAsync(system, cards, send2server: false, Priority.Low, 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 167);
							}
							if (ret.HasError || !sanadsHasBeenChanged)
							{
								result = ret;
							}
							else
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await OrderBusiness.UpdateCustomerFullNameAsync(ContextGuid, Guid, FullName, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 169);
								if (ret.HasError || !sanadsHasBeenChanged)
								{
									result = ret;
								}
								else
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ReceptionBusiness.UpdateCustomerFullNameAsync(ContextGuid, Guid, FullName, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 171);
									if (!ret.HasError && sanadsHasBeenChanged)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await PardakhtBusiness.UpdateCustomerFullNameAsync(ContextGuid, Guid, FullName, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 173);
										goto end_IL_03b6;
									}
									result = ret;
								}
							}
						}
						goto end_IL_01ea;
						IL_1083:
						line = 40;
						if (tafsilNumbers == null)
						{
							goto IL_11aa;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await TafsilNumberBusiness.SaveRangeAsync(dis.Token, ContextGuid, tafsilNumbers, Guid, awaitOnSemaphore: false, tryCount, tranName, AutoRemoveDuplicateNumbers: true), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 149);
						if (!ret.HasError)
						{
							PhoneNumbers = string.Join(",", tafsilNumbers.Select((TafsilNumberBusiness q) => q.OriginalNumber));
							goto IL_11aa;
						}
						result = ret;
						goto end_IL_01ea;
						end_IL_03b6:;
					}
				}
				goto end_IL_0176;
				end_IL_01ea:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 175);
				goto end_IL_0176;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(ContextGuid) == null)
				{
					Log.Warning(ex, $"Line:{line} cache is null!!");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line} cache is notnull.", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 181);
				}
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 182);
				goto end_IL_0176;
			}
			return result;
			end_IL_0176:;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
			if (ret.HasError && !token.IsCancellationRequested && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("CusBussines.SaveAsync finished but result has error" + $"\r\n CustomerGuid:{Guid}" + $"\r\n msg:{ret}", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 188);
			}
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 193), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 193);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await MetaTransactionDestinyAsync(tranName, ret.HasError), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 194);
			}
			if (sendToServer && !ret.HasError)
			{
				if (awaitForSend)
				{
					await Send2ServerAsync(system, CustomerUserName, CustomerPassword, timeout, token, priority);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(system, CustomerUserName, CustomerPassword, timeout, token, priority));
				}
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.CusCompact.RaiseAccountSaved(this, EventArgs.Empty);
			}
			saveComplationTask.TrySetResult(!ret.HasError);
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> FillUserNameByMobileNumberAsync(List<TafsilNumberBusiness> tafsilNumbers, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<TafsilNumberBusiness> list = tafsilNumbers?.Where((TafsilNumberBusiness p) => p?.Mobile ?? false)?.ToList();
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (TafsilNumberBusiness preferedUserName in list)
			{
				if (await GetByUserNameAsync(ContextGuid, preferedUserName.CalcedNumber.ToString(), null, token, tranName) == null)
				{
					User = preferedUserName.CalcedNumber.ToString();
					return ret;
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "FillUserNameByMobileNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 234);
			WebErrorLog.Instance.StartLog(ex, "", "FillUserNameByMobileNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 235);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> PasswordRecovery(string contextGuid, string username, string mobile)
	{
		ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
		try
		{
			ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> returnedSaveFuncInfoWithValue = await WebCus.PasswordRecover((await UserInformationBusiness.GetAsync(contextGuid)).SyncInfo.Token, username, mobile);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "PasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 248);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 253);
			ret.AddReturnedValue(ex, "", "PasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 254);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, List<TafsilNumberBusiness> listTafsilNumbers, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, short autoFixValidationErrorsCount, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		checked
		{
			using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
			{
				ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
				short line = 0;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync_(system, listTafsilNumbers, !Status, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 266);
					TafsilBusiness tafsil = await GetTafsilAsync(system, "", token);
					line = 1000;
					if (tafsil == null)
					{
						throw new NullReferenceException("Tafsil is null!!");
					}
					TafsilGroupBusiness tafsilGroup = await tafsil.GetTafsilGroupAsync(system, token, tranName);
					line = 1001;
					if (system != EnSystem.Transfer)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await tafsil.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, tranName, autoFixValidationErrorsCount, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 273);
					}
					line = 1;
					if (string.IsNullOrEmpty(Family))
					{
						ret.AddError(system, "*نام خانوادگی پر نشده است.", "Family", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 276);
					}
					line = 5;
					if (tafsil == null)
					{
						throw new Exception("حساب تفصیلی نال است");
					}
					line = 10;
					if (tafsil == null || tafsilGroup == null)
					{
						WebErrorLog.Instance.StartLog($"Customer.Tafsil is null:{tafsil == null}\r\n" + $"Customer.Tafsil.TafsilGroup is null:{tafsilGroup == null}\r\n" + $"CusGuid:{this.guid}\r\n" + $"TafsilGroupGuid:{tafsil?.TafsilGroupGuid}\r\n" + $"CusGroup:{GroupGuid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 282);
					}
					line = 11;
					if (system != EnSystem.Transfer && autoFixValidationErrorsCount > 0 && tafsilGroup == null)
					{
						line = 110;
						string contextGuid = ContextGuid;
						CancellationToken token2 = token;
						TafsilGroupBusiness tafsilGroupBusiness = (await TafsilGroupBusiness.GetAllAsync(contextGuid, system, true, token2)).FirstOrDefault();
						line = 111;
						if (system == EnSystem.UserInterface)
						{
							line = 112;
							ret.AddWarning("شخص (" + Name + "/" + Family + ") با گروه تفصیلی نامعتبر دریافت شد.\r\nگروه تفصیلی توسط فرآیند اصلاح داده های نامعتبر به (" + tafsilGroupBusiness.Name + ") اصلاح شد.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 296);
						}
						line = 113;
						tafsil.TafsilGroupGuid = tafsilGroupBusiness.Guid;
						line = 114;
						returnedSaveFuncInfo = ret;
						CusBusiness cusBusiness = this;
						short num = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await cusBusiness.CheckValidationAsync(system, listTafsilNumbers, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, num, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 303);
						line = 115;
						return ret;
					}
					line = 20;
					bool flag = GroupGuid == Guid.Empty;
					if (!flag)
					{
						flag = ((await CusGroupBusiness.GetAsync(ContextGuid, GroupGuid, token))?.Guid ?? Guid.Empty) == Guid.Empty;
					}
					if (flag)
					{
						line = 21;
						CancellationToken token2;
						string family;
						string name;
						string text2;
						long num2;
						if (autoFixValidationErrorsCount > 0)
						{
							line = 22;
							string contextGuid2 = ContextGuid;
							token2 = token;
							Guid newGroupGuid = (await CusGroupBusiness.GetAllAsync(contextGuid2, system, true, isLoadWithGuidEmpty: false, token2))?.FirstOrDefault()?.Guid ?? GroupGuid;
							if (system == EnSystem.UserInterface)
							{
								returnedSaveFuncInfo = ret;
								string text = $"GroupGuid:{GroupGuid}\r\n";
								text2 = $"NewGroupGuid:{newGroupGuid}\r\n";
								name = Name;
								family = Family;
								num2 = await GetCodeAsync(system, tranName, token);
								returnedSaveFuncInfo.AddWarning("*گروه مشخص نشده است.\r\n" + text + text2 + "نام حساب:" + name + "//" + family + "\r\n" + $"کد حساب:{num2}", "GroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 317);
							}
							GroupGuid = newGroupGuid;
							if (await CusGroupBusiness.GetAsync(ContextGuid, GroupGuid, token) == null)
							{
								new CusGroupBusiness(ContextGuid)
								{
									Name = "تعریف اتوماتیک",
									Guid = GroupGuid,
									ContextGuid = ContextGuid,
									Description = "",
									ServerStatus = ServerStatusForClient.Delivered,
									Status = true,
									Special = false
								};
							}
							returnedSaveFuncInfo = ret;
							CusBusiness cusBusiness2 = this;
							short num = (short)(autoFixValidationErrorsCount - 1);
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await cusBusiness2.CheckValidationAsync(system, listTafsilNumbers, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, num, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 335);
							return ret;
						}
						string contextGuid3 = ContextGuid;
						token2 = token;
						IEnumerable<CusGroupBusiness> enumerable = await CusGroupBusiness.GetAllAsync(contextGuid3, system, true, isLoadWithGuidEmpty: true, token2);
						returnedSaveFuncInfo = ret;
						EnSystem system2 = system;
						family = $"تعداد گروه های موجود در دیتابیس:{enumerable?.Count() ?? (-1)}\r\n";
						name = Name;
						text2 = Family;
						num2 = await GetCodeAsync(system, tranName, token);
						returnedSaveFuncInfo.AddError(system2, "گروه مشخص نشده است.\r\n" + family + "نام حساب:" + name + "//" + text2 + "\r\n" + $"کد حساب:{num2}", "GroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 341);
					}
					line = 30;
					flag = TitleGuid == Guid.Empty;
					if (!flag)
					{
						flag = ((await CusTitleBusiness.GetAsync(ContextGuid, TitleGuid, token))?.Guid ?? Guid.Empty) == Guid.Empty;
					}
					if (flag)
					{
						if (autoFixValidationErrorsCount > 0)
						{
							line = 31;
							Guid guid = (await CusTitleBusiness.GetAllAsync(ContextGuid, true, token))?.FirstOrDefault()?.Guid ?? Guid.Empty;
							ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeTitle From:({TitleGuid}) To:({guid}))", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 357);
							TitleGuid = guid;
							returnedSaveFuncInfo = ret;
							CusBusiness cusBusiness3 = this;
							short num = (short)(autoFixValidationErrorsCount - 1);
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await cusBusiness3.CheckValidationAsync(system, listTafsilNumbers, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, num, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 359);
							return ret;
						}
						ret.AddError("برای حساب شخص (" + Name + "/" + Family + ")\r\nعنوان مشخص نشده یا عنوان مشخص شده معتبر نمیباشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 363);
					}
					line = 40;
					flag = system != EnSystem.Transfer;
					if (flag)
					{
						flag = await GetCodeAsync(system, tranName, token) <= 0;
					}
					if (flag)
					{
						if (autoFixValidationErrorsCount > 0)
						{
							line = 45;
							long currentCode = await GetCodeAsync(system, tranName, token);
							EnSystem system2 = system;
							await SetCodeAsync(system2, await GetNextCodeAsync(ContextGuid, GroupGuid, token), tranName, token);
							if (system == EnSystem.UserInterface)
							{
								returnedSaveFuncInfo = ret;
								object arg = autoFixValidationErrorsCount;
								object arg2 = currentCode;
								returnedSaveFuncInfo.AddInformation($"autoFixValidationErrorsCount:({arg}) \r\n ChangeTafsilCode From:({arg2}) To:({await GetCodeAsync(system, tranName, token)}))", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 373);
							}
							returnedSaveFuncInfo = ret;
							CusBusiness cusBusiness4 = this;
							short num = (short)(autoFixValidationErrorsCount - 1);
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await cusBusiness4.CheckValidationAsync(system, listTafsilNumbers, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, num, tranName, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 376);
							return ret;
						}
						ret.AddError(system, "*کد مشخص نشده است.", "Code", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 380);
					}
					line = 50;
					flag = !string.IsNullOrEmpty(User);
					if (flag)
					{
						flag = !(await CheckUserNameAsync(ContextGuid, Guid, User, token, tranName));
					}
					if (flag)
					{
						if (system != EnSystem.UserInterface)
						{
							List<TafsilNumberBusiness> list = await GetTafsilNumbersAsync(token);
							if (list != null && list.Any())
							{
								User = list.First().CalcedNumber.ToString();
								flag = !string.IsNullOrEmpty(User);
								if (flag)
								{
									flag = !(await CheckUserNameAsync(ContextGuid, Guid, User, token, tranName));
								}
								if (flag)
								{
									if (tafsil != null && User != tafsil.Code.ToString())
									{
										User = tafsil.Code.ToString();
									}
									flag = !string.IsNullOrEmpty(User);
									if (flag)
									{
										flag = !(await CheckUserNameAsync(ContextGuid, Guid, User, token, tranName));
									}
									if (flag)
									{
										User = Guid.ToString();
									}
								}
							}
							else if (tafsil != null && User != tafsil.Code.ToString())
							{
								User = tafsil.Code.ToString();
								flag = !string.IsNullOrEmpty(User);
								if (flag)
								{
									flag = !(await CheckUserNameAsync(ContextGuid, Guid, User, token, tranName));
								}
								if (flag)
								{
									User = Guid.ToString();
								}
							}
							else
							{
								User = Guid.ToString();
							}
						}
						else
						{
							ret.AddError(system, "نام کاربری " + User + " تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 405);
						}
					}
					if (!string.IsNullOrEmpty(Email))
					{
						CusBusiness cusBusiness5 = await GetByEmailAsync(ContextGuid, Email, token, tranName);
						if (cusBusiness5 != null && cusBusiness5.Guid != Guid)
						{
							ret.AddError(system, "ایمیل " + Email + " تکراری می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 411);
						}
					}
					if (Guid == Guid.Empty)
					{
						ret.AddError(system, "*Guid is empty.", "Guid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 414);
					}
					if (system != EnSystem.UserInterface)
					{
						ret.throwExceptionIfError();
					}
					return ret;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line} //System:{system}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 420);
					ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 421);
					return ret;
				}
				finally
				{
					if (system != EnSystem.UserInterface && ret.HasError)
					{
						Log.Warning<string>("Customer validation error", ret.GetErrorMessage());
					}
				}
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 438), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 438);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 440);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 444);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 445);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 450), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 450);
			}
		}
		return ret;
	}

	public static Task<bool> CheckUserNameAsync(string contextGuid, Guid guid, string userName, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.CheckUserNameAsync(guid, userName, token, tranName);
	}

	public static Task<CusBusiness> GetByEmailAsync(string contextGuid, string email, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetByEmailAsync(email, token, tranName);
	}

	public static CusBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			if (ClsCache.GetCache(contextGuid)?.CacheRepository == null)
			{
				_ = ClsCache.GetCache(contextGuid).CacheRepository;
				throw new NullReferenceException("cacheRepo");
			}
			try
			{
				return (guid == Guid.Empty) ? null : ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.Get(guid, token, tranName);
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 477);
				throw;
			}
		}
	}

	public static Task<CusBusiness> GetAsync(string contextGuid, long Code, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetAsync(Code, token, tranName);
	}

	public static Task<CusBusiness> GetByUserNameAsync(string contextGuid, string userName, string pass = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetByUserNameAsync(userName, pass, token, tranName);
	}

	public static Task<CusBusiness> GetByNameAsync(string contextGuid, string firstName, string lastName, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetByNameAsync(firstName, lastName, guid, token, tranName);
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> guids, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 499), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 499);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.RemoveRangeFromEntitiesAsync(guids, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 500);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.KeepRangeAsync(guids, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 502);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 506);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 507);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 512), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 512);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, IEnumerable<Guid> items, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CusBusiness/DeleteRangeAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 527);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 529), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 529);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await TafsilNumberBusiness.Remove(contextGuid, items.ToList(), awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 531);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.RemoveRangeFromEntitiesAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 533);
							if (!ret.HasError)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 535);
								goto end_IL_011c;
							}
							result = ret;
						}
					}
					goto end_IL_0076;
					end_IL_011c:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 540);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 541);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 546), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 546);
			}
		}
		return ret;
	}

	public static async Task<CusBusiness> GetAsync(string contextGuid, EnSystem system, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (guid == Guid.Empty)
			{
				return null;
			}
			if (await CusCompactBusiness.GetAsync(contextGuid, system, guid, null, tranName, token) == null)
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetAsync(guid, token, tranName);
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"GetAsyncBy Guid({guid})//ClsCache.GetCache(contextGuid).CacheRepository.ICustomer is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.ICustomer == null}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 563);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, List<WebCus> items, CancellationToken token)
	{
		int line = 0;
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "CustomerBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					line = 4;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 577);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 578), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 578);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 580);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								IEnumerable<WebMetaData> enumerable = items.SelectMany((WebCus q) => q.MetaDataList);
								if (enumerable == null || !enumerable.Any())
								{
									result = ret;
								}
								else
								{
									enumerable = enumerable.Where((WebMetaData w) => w.Key == CustomerMetaKey.BankInfo.ToString()).ToList();
									if (enumerable == null || !enumerable.Any())
									{
										List<MetaDataBusiness> lstMeta = await MetaDataMapper.Instance.MapListAsync(contextGuid, enumerable, new List<MetaDataBusiness>(), tranName, token);
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await MetaDataBusiness.SaveRangeAsync(lstMeta, awaitOnSemaphore: true, default(CancellationToken), 10), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 587);
										goto end_IL_011b;
									}
									result = ret;
								}
							}
						}
					}
					goto end_IL_0078;
					end_IL_011b:;
				}
				goto end_IL_0067;
				end_IL_0078:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 589);
				goto end_IL_0067;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(contextGuid) == null)
				{
					Log.Warning(ex, $"#Line_:{line}//Cache is null!!");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_:{line}//Cache is not null.", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 595);
				}
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 596);
				goto end_IL_0067;
			}
			return result;
			end_IL_0067:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 600), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 600);
			if (ret.HasError && items.Count() == 1)
			{
				WebErrorLog.Instance.StartLog("توی حسابیار اومدم یک لیست از اشخاص رو ذخیره کنم\r\nبه خطا خوردم\r\nاینقدر لیست رو شکوندم که فهمیدم این بنده خدا عامل خطا است:\r\n" + Json.ToStringJson(items.Single()) + "\r\nError=>\r\n" + ret.ErrorMessage, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 603);
				ret = await SaveWebCusAsync(contextGuid, items.Single(), EnSystem.SignalR, token, sendToServer: false, updateAccount: true, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: true);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebCus> list2 = new List<WebCus>();
				WebCus[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2).ToList(), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 620);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<SaveResponse>>> SaveRangeAsync(string contextGuid, bool isFromSync, bool sendToServer, EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, IEnumerable<WebCus> items, CancellationToken token, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool updateAccount = false, short maxParalelTasks = 10, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfoWithValue<List<SaveResponse>> ret = new ReturnedSaveFuncInfoWithValue<List<SaveResponse>>
		{
			value = new List<SaveResponse>()
		};
		int line = 0;
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			CancellationTokenSource invokeToken = new CancellationTokenSource();
			try
			{
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting && !isFromSync)
				{
					ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await WaitForTafsilsSaveAsync(contextGuid, EnSystem.None, items, DateTime.Now.AddSeconds(30.0), token, awaitOnSemaphore: false), ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 635);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 637);
				}
				if (!(items?.Any() ?? false))
				{
					return ret;
				}
				line = 10;
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 642);
					return ret;
				}
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar && system == EnSystem.SignalR)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveRangeForHesabyarAsync(contextGuid, items.ToList(), token);
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 648);
					ret.value = items.Select((WebCus p) => new SaveResponse(0, p.Guid, DateTime.Now, !ret.HasError, ret.GetErrorMessage(), ret.GetErrorMessage())).ToList();
					return ret;
				}
				line = 1600;
				ret.throwExceptionIfError();
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				Dictionary<int, Guid> taskId2CusGuid = new Dictionary<int, Guid>();
				int current = 0;
				Guid cusGuid;
				foreach (WebCus webCus in items)
				{
					if (webCus == null)
					{
						continue;
					}
					current = checked(current + 1);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 667);
					}
					Task<ReturnedSaveFuncInfo> task = Task.Run(() => SaveWebCusAsync(contextGuid, webCus, system, default(CancellationToken), sendToServer, updateAccount, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate));
					tasks.Add(task);
					taskId2CusGuid.Add(task.Id, webCus.Guid);
					line = 1604;
					if (tasks.Count > maxParalelTasks)
					{
						Task<ReturnedSaveFuncInfo> result = await Task.WhenAny(tasks);
						taskId2CusGuid.TryGetValue(result.Id, out cusGuid);
						line = 1605;
						taskId2CusGuid.Remove(result.Id);
						tasks.Remove(result);
						line = 1606;
						ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await result, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 700);
						List<SaveResponse> value = ret.value;
						Guid guid = cusGuid;
						DateTime now = DateTime.Now;
						value.Add(new SaveResponse(0, guid, now, !(await result).HasError, result.ToString(), result.ToString()));
						line = 1607;
						if (percentCompleted != null)
						{
							invokeToken?.Cancel();
							line = 1608;
							invokeToken = new CancellationTokenSource(60000);
							CancellationToken token2 = invokeToken.Token;
							percentCompleted(new PercenteCompletedEventArgs(items.Count(), current, webCus.Name + " " + webCus.Family, token2));
							line = 1609;
						}
					}
				}
				while (tasks.Any())
				{
					Task<ReturnedSaveFuncInfo> result = await Task.WhenAny(tasks);
					taskId2CusGuid.TryGetValue(result.Id, out cusGuid);
					line = 1610;
					taskId2CusGuid.Remove(result.Id);
					tasks.Remove(result);
					ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await result, ReturnedState.Information, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 720);
					List<SaveResponse> value = ret.value;
					Guid guid = cusGuid;
					DateTime now = DateTime.Now;
					value.Add(new SaveResponse(0, guid, now, !(await result).HasError, result.ToString(), result.ToString()));
					line = 1611;
					if (percentCompleted != null)
					{
						invokeToken?.Cancel();
						invokeToken = new CancellationTokenSource(60000);
						CancellationToken token3 = invokeToken.Token;
						percentCompleted(new PercenteCompletedEventArgs(items.Count(), items.Count(), "", token3));
						line = 1612;
					}
				}
				line = 17;
				if (!token.IsCancellationRequested && ret.HasError)
				{
					WebErrorLog.Instance.StartLog("Error in CusBussines SaveRangeAsync \r\n5Object Guids:" + string.Join(",", items?.Take(5)?.Select((WebCus q) => q?.Guid ?? Guid.Empty)) + "\r\n\ud83d\udfe2infos:" + ret.GetInformationMessage() + "\r\n\ud83d\udfe1Warnings:" + ret.WarningMessage + "\r\n\ud83d\udd34errors:" + ret.GetErrorMessage(), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 733);
					return ret;
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 742);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 745);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 747);
			}
			finally
			{
				if (ret.HasError)
				{
					ret.value = new List<SaveResponse>();
					SaveResponse item = new SaveResponse(0, Guid.Empty, DateTime.Now, saveResultStatus: false, ret.ToString(), ret.ToString());
					ret.value.Add(item);
				}
			}
			Log.Debug($"************CountCustomers:{items?.Count() ?? 0} | HasError:{ret.HasError}**************http://count.com");
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveWebCusAsync(string contextGuid, WebCus webCus, EnSystem system, CancellationToken token, bool sendToServer, bool updateAccount, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CusBusiness cus = await CustomerMapper.Instance.MapAsync(contextGuid, webCus, new CusBusiness(contextGuid), "", token);
			List<TafsilNumberBusiness> numbers = await TafsilNumberMapper.Instance.MapListAsync(contextGuid, webCus.Numbers, null, "", token);
			if (webCus.ClientVersion <= 0)
			{
				CusBusiness cusBusiness = await GetAsync(contextGuid, system, cus.Guid, token);
				if (cusBusiness != null)
				{
					cus.RoleGuid = cusBusiness.RoleGuid;
				}
			}
			List<CusCardBusiness> cards = null;
			if (webCus.CardList?.Any() ?? false)
			{
				cards = await CusCardMapper.Instance.MapListAsync(contextGuid, webCus.CardList, null, "", token);
			}
			if ((cus?.AreaGuid ?? Guid.Empty) != Guid.Empty && ClsCache.GetCache(contextGuid).Source == ENSource.Accounting && await VisitorAreaBusiness.GetAsync(contextGuid, cus.AreaGuid.Value, token) == null)
			{
				cus.AreaGuid = null;
			}
			await cus.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, updateAccount, 10, sendToServer, Priority.Low, awaitOnSemaphore: false, "", "", 60000, awaitForSend: false, numbers, 10, "", null, null, cards, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveWebCusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 792);
			ret.AddReturnedValue(ex, "", "SaveWebCusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 793);
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebCus webCus = await ToWebItemAsync();
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.CustomerLog;
				cashiersLogBusiness.SanadGuid = webCus.Guid;
				cashiersLogBusiness.Title = $"خطا در ذخیره شخص {webCus.FullName}-{webCus.Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webCus, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 827);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, IEnumerable<CusBusiness> items, bool updateAccount, bool send2server, Priority priority = Priority.Low, bool awaitOnSemaphore = true, short autoFixValidationErrorsCount = 0, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			tranName = Guid.NewGuid().ToString();
		}
		int line = 0;
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			if (ret.HasError)
			{
				return ret;
			}
			try
			{
				if (items == null || !items.Any())
				{
					return ret;
				}
				line = 10;
				IEnumerable<Task<ReturnedSaveFuncInfo>> tasks = items.Select((CusBusiness p) => Task.Run(() => p.CheckValidationAsync(system, null, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, autoFixValidationErrorsCount, "", token), token));
				line = 160;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 850);
				ret.throwExceptionIfError();
				if (items?.Any() ?? false)
				{
					foreach (CusBusiness item in items)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 855);
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate, updateAccount, autoFixValidationErrorsCount, send2server, priority, awaitOnSemaphore: false, "", "", 60000, awaitForSend: false, null, tryCount, "", null, null, null, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 856);
					}
				}
				line = 17;
				if (ret.HasError)
				{
					WebErrorLog.Instance.StartLog("CusBussines.SaveRangeAsync(❌" + ret.GetErrorMessage() + "❌) \r\n Object Guids:" + string.Join(",", items.Select((CusBusiness q) => q.Guid)) + "\r\n" + $"#Line_{line}\r\n" + "Ret.Error:" + ret.GetErrorMessage(), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 864);
					return ret;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 873);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 874);
			}
			finally
			{
				if (ret.HasError)
				{
					WebErrorLog.Instance.StartLog("CusBussines.SaveRangeAsync finished but result has error\r\nmsg:" + ret.ToString() + "\r\n CustomerGuids:" + string.Join(",", items?.Select((CusBusiness p) => p.Guid)), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 879);
				}
			}
			Log.Information($"************CountCustomers:{items?.Count() ?? 0}//HasError:{ret.HasError}**************http://count.com");
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> WaitForTafsilsSaveAsync(string contextGuid, EnSystem system, IEnumerable<WebCus> items, DateTime timeout, CancellationToken token, bool awaitOnSemaphore = true)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (items == null || !items.Any())
			{
				return ret;
			}
			line = 1;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 895);
			}
			line = 2;
			if (DateTime.Now > timeout)
			{
				line = 3;
				return ret.AddError(system, "Wait for save tafsils timedOut!", "timeout", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 900);
			}
			line = 4;
			foreach (WebCus item in items)
			{
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 907);
				}
				line = 60;
				if (item == null)
				{
					throw new NullReferenceException("Item is null!!");
				}
				line = 61;
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, system, item.Guid, "", token, "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs");
				line = 7;
				if (tafsilBusiness == null || tafsilBusiness.Guid != item.Guid)
				{
					line = 8;
					await Task.Delay(1000);
					line = 9;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 918);
					}
					line = 10;
					return await WaitForTafsilsSaveAsync(contextGuid, system, items, timeout, token, awaitOnSemaphore: false);
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 926);
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}//CacheIsNull or disposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}", "WaitForTafsilsSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 927);
		}
		return ret;
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.DbCountAsync(status, token, tranName);
	}

	public async Task<long> GetCodeAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return (await GetTafsilAsync(system, tranName, token))?.Code ?? 0;
	}

	public async Task SetCodeAsync(EnSystem system, long value, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		(await GetTafsilAsync(system, tranName, token)).Code = value;
	}

	public async Task<string> GetPhoneNumbersAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return string.Join(",", (await TafsilNumberBusiness.GetAsync(ContextGuid, Guid, token, tranName))?.Where((TafsilNumberBusiness x) => x != null)?.Select((TafsilNumberBusiness p) => p.OriginalNumber.ToString()));
	}

	public async Task<Guid> GetVisitorGuidAsync(Guid? currentCashier = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_visitorGuid == Guid.Empty)
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetAsync(ContextGuid, currentCashier ?? Guid.Empty, token, tranName);
				if (cashierBusiness == null)
				{
					return Guid.Empty;
				}
				return cashierBusiness?.VisitorGuid ?? Guid.Empty;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVisitorGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1135);
		}
		return _visitorGuid;
	}

	public void SetVisitorGuid(Guid value)
	{
		_visitorGuid = value;
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _getTafsilSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if (_tafsil == null)
			{
				Guid? tafsilGroupGuid = (await CusGroupBusiness.GetAsync(ContextGuid, GroupGuid, token, tranName))?.TafsilGroupGuid;
				Guid? guid = tafsilGroupGuid;
				if (!guid.HasValue)
				{
					string contextGuid = ContextGuid;
					CancellationToken token2 = token;
					string tranName2 = tranName;
					tafsilGroupGuid = (await TafsilGroupBusiness.GetAllAsync(contextGuid, system, true, token2, tranName2))?.FirstOrDefault((TafsilGroupBusiness p) => p.Tbl == "CUS")?.Guid;
				}
				guid = tafsilGroupGuid;
				if (!guid.HasValue)
				{
					string contextGuid2 = ContextGuid;
					CancellationToken token2 = token;
					string tranName2 = tranName;
					tafsilGroupGuid = (await TafsilGroupBusiness.GetAllAsync(contextGuid2, system, true, token2, tranName2))?.FirstOrDefault()?.Guid;
				}
				_tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, Guid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs");
				if (_tafsil != null && _tafsil.Version <= 0 && system != EnSystem.Mapper && system != EnSystem.Transfer && system != 0)
				{
					throw new InvalidOperationException($"tafsil vesion({_tafsil.Version}) for ({_tafsil}) is not valid");
				}
				if (_tafsil == null)
				{
					_tafsil = new TafsilBusiness(ContextGuid)
					{
						DateM = Date,
						TafsilGroupGuid = (tafsilGroupGuid ?? Guid.Empty),
						Guid = Guid,
						HType = HesabType.Customers,
						Name = FullName,
						Status = Status,
						Description = Description,
						Version = 1
					};
				}
			}
			if (TafsilGroupGuid != Guid.Empty && TafsilGroupGuid != _tafsil.TafsilGroupGuid)
			{
				_tafsil.TafsilGroupGuid = TafsilGroupGuid;
				_tafsil.Code = 0L;
			}
			if (_tafsil.Code <= 0 && (_tafsil?.TafsilGroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = await TafsilBusiness.NextCodeInGroupAsync(ContextGuid, system, _tafsil.TafsilGroupGuid, token, tranName);
				returnedSaveFuncInfoWithValue.throwExceptionIfError();
				_tafsil.Code = returnedSaveFuncInfoWithValue.value;
			}
			return _tafsil;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1195);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_getTafsilSemaphore.Release();
			}
		}
	}

	public async Task<WebCus> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			_ = 6;
			try
			{
				WebCus ret = await CustomerMapper.Instance.MapAsync(ContextGuid, this, new WebCus(), "", token);
				List<TafsilNumberBusiness> source = await TafsilNumberBusiness.GetAsync(ContextGuid, Guid, token);
				ret.Numbers = await TafsilNumberMapper.Instance.MapListAsync(ContextGuid, source, new List<WebTafsilNumbers>(), "", token);
				IEnumerable<CustomerMetaDataBusiness> source2 = await GetMetaDataListAsync(isClear: true, token);
				WebCus webCus = ret;
				webCus.MetaDataList = await MetaDataMapper.Instance.MapListAsync(ContextGuid, source2, new List<WebMetaData>(), "", token);
				IEnumerable<CusCardBusiness> enumerable = await CusCardBusiness.GetAllByCusGuidAsync(ContextGuid, Guid, true, token);
				if (enumerable != null && enumerable.Any())
				{
					ret.CardList = await CusCardMapper.Instance.MapListAsync(ContextGuid, enumerable, new List<WebCusCard>(), "", token);
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1226);
				return null;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>>> TempRegisterAsync(string mobile, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>>();
		try
		{
			line = 1;
			WebCus webitem = await ToWebItemAsync(token);
			line = 2;
			webitem.Mob1 = mobile;
			line = 3;
			UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(ContextGuid);
			CustomerTempSaveModel item = new CustomerTempSaveModel
			{
				Customer = webitem,
				DepId = userInformationBusiness.DepId,
				Token = userInformationBusiness.SyncInfo.Token,
				ENSource = ClsCache.GetCache(ContextGuid).Source
			};
			line = 4;
			ReturnedSaveFuncInfoWithValue<WebResponse<CustomerSaveResponse>> returnedSaveFuncInfo = await webitem.PostToApi(item);
			line = 5;
			ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1253);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"11111_mobile:{mobile}//Line:{line}", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1257);
			ret.AddReturnedValue(ex, "", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1258);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CustomerSaveResponse>> TempRegisterAsync(EnSystem system, string ip, string mobile, CancellationToken token)
	{
		using (new JaegerMonitor("TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<CustomerSaveResponse> ret = new ReturnedSaveFuncInfoWithValue<CustomerSaveResponse>();
			int line = 0;
			try
			{
				line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1271);
				}
				line = 2;
				WebCus webCus = await ToWebItemAsync(token);
				line = 3;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1275);
				}
				webCus.Mob1 = mobile;
				line = 4;
				Packet pack = WebCus.GenerateTempSaveRequest(webCus, ip);
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1280);
				}
				RegisterCustomerWaiter registerCustomerWaiter = new RegisterCustomerWaiter(await ToWebItemAsync(token), ClsCache.GetCache(ContextGuid).Context);
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1284);
				}
				if (ClsCache.GetCache(ContextGuid).BufferLoginState.Status == EnLoginResult.Success)
				{
					line = 7;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1288);
					}
					Task.Run(() => ClsCache.GetCache(ContextGuid).SendAndWaitForResponse(new List<Packet> { pack }, token, 20, 2, 10, 10));
					line = 8;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1291);
					}
					ReturnedSaveFuncInfoWithValue<CustomerSaveResponse> returnedSaveFuncInfoWithValue = await registerCustomerWaiter.SendTempRequst();
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1293);
					ret.value = returnedSaveFuncInfoWithValue.value;
				}
				else
				{
					line = 10;
					ret.AddError(system, "Buffer is not connected!", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1299);
					ret.value = new CustomerSaveResponse
					{
						Status = CusTmpCmtRsp.Unknown
					};
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1305);
				ret.value = new CustomerSaveResponse
				{
					Status = CusTmpCmtRsp.OperationCanceled
				};
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1310);
				WebErrorLog.Instance.StartLog(ex, $"2222_ip:{ip}//Mobile:{mobile},Token//Line:{line}", "TempRegisterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1311);
				ret.value = new CustomerSaveResponse
				{
					Status = CusTmpCmtRsp.ClientException
				};
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> TempCommitAsync(string smsCode, CancellationToken token, bool useApi = false)
	{
		using (new JaegerMonitor("TempCommitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
			try
			{
				ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> returnedSaveFuncInfoWithValue;
				if (useApi)
				{
					WebCus obj = await ToWebItemAsync(token);
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await obj.Commit(smsCode, token), ReturnedState.None, "TempCommitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1327);
					return ret;
				}
				Packet pack = WebCus.GenerateCommitRequest(Guid, smsCode);
				RegisterCommitCusWaiter waiter = new RegisterCommitCusWaiter(await ToWebItemAsync(token), ClsCache.GetCache(ContextGuid).Context);
				await ClsCache.GetCache(ContextGuid).SendData(pack, Priority.High);
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await waiter.SendTempCommitRequst(), ReturnedState.None, "TempCommitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1333);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TempCommitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1337);
				ret.AddReturnedValue(ex, "", "TempCommitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1338);
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "CusBusiness/SetSaveResultAsync", 60000, token))
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1358), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1358);
						}
						transactionHasBeenCreated = true;
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1361);
							goto end_IL_0135;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1360);
						goto end_IL_009c;
						end_IL_0135:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1363);
					goto end_IL_008b;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1366);
					ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1367);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1373), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1373);
				}
			}
			return ret;
		}
	}

	public async Task<Guid> GetReceptionCredit_DefaultMoeinGuid(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await GetTafsilAsync(system, tranName, token)).GetTafsilGroupAsync(system, token, tranName))?.MoeinReceptionGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionCredit_DefaultMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1393);
		}
		return Guid.Empty;
	}

	public async Task<CusGroupBusiness> GetGroupAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await CusGroupBusiness.GetAsync(ContextGuid, GroupGuid, token, tranName);
	}

	public async Task<Guid> GetPardakhtDebit_DefaultMoeinGuidAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			TafsilBusiness tafsilBusiness = await GetTafsilAsync(system, tranName, token);
			if (tafsilBusiness == null)
			{
				return Guid.Empty;
			}
			return (await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName))?.MoeinPardakhtGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtDebit_DefaultMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1412);
			throw;
		}
	}

	public async Task<Guid> GetSellDebit_DefaultMoeinGuid(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await GetTafsilAsync(system, tranName, token)).GetTafsilGroupAsync(system, token, tranName))?.MoeinSellGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellDebit_DefaultMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1427);
			throw;
		}
	}

	public async Task<Guid> GetBuyCredit_DefaultMoeinGuidAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await GetTafsilAsync(system, tranName, token)).GetTafsilGroupAsync(system, token, tranName))?.MoeinBuyGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBuyCredit_DefaultMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1442);
			throw;
		}
	}

	public async Task<Guid> GetBackBuyDebit_DefaultMoeinGuidAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await GetTafsilAsync(system, tranName, token)).GetTafsilGroupAsync(system, token, tranName))?.MoeinBackBuyGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBackBuyDebit_DefaultMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1457);
			throw;
		}
	}

	public async Task<Guid> GetBackSellCredit_DefaultMoeinGuidAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			return (await (await GetTafsilAsync(system, tranName, token)).GetTafsilGroupAsync(system, token, tranName))?.MoeinBackSellGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBackSellCredit_DefaultMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1472);
			throw;
		}
	}

	public async Task<string> GetPicFaceFullPathAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			FileInfoBusiness fileInfoBusiness = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicCustomer, token))?.SingleOrDefault();
			if (fileInfoBusiness != null && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				if (fileInfoBusiness.ServerStatus == ServerStatusForClient.None)
				{
					return fileInfoBusiness.GetLocalFilePath();
				}
				return fileInfoBusiness.Url;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPicFaceFullPathAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1487);
		}
		return "";
	}

	public async Task<string> GetPicSignFullPathAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			FileInfoBusiness fileInfoBusiness = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicCustomer, token))?.SingleOrDefault();
			if (fileInfoBusiness == null)
			{
				fileInfoBusiness = await FileInfoBusiness.GetAsync(ContextGuid, "NotFoundCusSign.png", "", FileManagement_Parts.PicSign, token);
			}
			if (fileInfoBusiness != null && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				if (fileInfoBusiness.ServerStatus == ServerStatusForClient.None)
				{
					return fileInfoBusiness.GetLocalFilePath();
				}
				return fileInfoBusiness.Url;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPicSignFullPathAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1501);
		}
		return "";
	}

	public CusBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public string ServerSt()
	{
		return ServerStatus switch
		{
			ServerStatusForClient.None => "در حال ارسال..", 
			ServerStatusForClient.Sent => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.Delivered => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.SendError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DeliveryError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DirectDelivery => $"{ServerStatus.GetDisplay()}({DeliveryId})", 
			_ => $"خطا در تشخیص{ServerStatus}", 
		};
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(EnSystem system, string customerUserName, string customerPassword, int timeout, CancellationToken token, Priority priority)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (token.IsCancellationRequested || !cache.BufferEnable || cache.BufferLoginState.Status != EnLoginResult.Success)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1541);
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "CusBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1546);
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetFromDatabaseAsync(ContextGuid, system, Guid, "", token, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs");
				if ((tafsil?.Guid ?? Guid.Empty) != Guid)
				{
					throw new Exception($"Tafsil.Guid is null or empty//or tafsil Guid({tafsil?.Guid ?? Guid.Empty}) is not equel to Cust.Guid(){Guid}!!");
				}
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && tafsil.ServerStatus != ServerStatusForClient.Delivered && tafsil.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await tafsil.Send2ServerAsync(token, system, priority), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1551);
				}
				Special = tafsil?.IsSpecial ?? false;
				WebCus item = await ToWebItemAsync(token);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1555);
				}
				List<Packet> paks = WebCus.GenerateSaveRequest(new List<WebCus> { item });
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1557);
				}
				using CustomerSaveWaiter waiter = new CustomerSaveWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), guid, token);
				Task<ReturnedSaveFuncInfo> sendTask = ((string.IsNullOrEmpty(customerUserName) || string.IsNullOrEmpty(customerPassword)) ? Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, Priority.Low)) : Task.Run(() => ClsCache.GetCache(ContextGuid).SendOnCustomerBuffer(customerUserName, customerPassword, paks, token, Priority.High, timeout)));
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1565);
				}
				ReturnedSaveFuncInfoWithValue<SaveResponse> saveRespose = await waiter.WaitForSaveResponse("CusBusinessWaiter");
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await sendTask, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1567);
				ret.AddReturnedValue(saveRespose, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1568);
				if (!ret.HasError && saveRespose?.value != null)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(saveRespose.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1570);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1572);
				}
				if (cache.BufferEnable && cache.BufferLoginState.Status == EnLoginResult.Success)
				{
					ret.throwExceptionIfError();
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1576);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1579);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1580);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		return Send2ServerAsync(system, "", "", 60000, token, priority);
	}

	public static Task<CusBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CusBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			if (ClsCache.GetCache(contextGuid).CacheRepository.ICustomer == null)
			{
				throw new NullReferenceException("customerRepo is null");
			}
			int Line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Customer;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال اشخاص";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Customers;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1612);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1614);
				}
				Line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1616);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ICustomer == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.ICustomer is null");
				}
				Line = 2;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation($"اشخاص سینک هستند. تعداد اشخاص سینک شده:{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1626);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1627);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					current = checked(current + 1);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1634);
					}
					CusBusiness cusBusiness = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
					if (uiUpdater != null)
					{
						uiUpdater.ShortMessage = "شخص جاری:" + cusBusiness?.FullName;
						uiUpdater.Count = current;
					}
					if (cusBusiness.ServerStatus == ServerStatusForClient.Delivered || cusBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
					{
						ret.AddInformation("current status on database is delivered!\r\nEntityType: Customer \r\n" + $"EntityGuid:{cusBusiness?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + cusBusiness?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1644);
						continue;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1650);
					}
					tasks.Add(cusBusiness.Send2ServerAsync(token, EnSystem.SignalR));
					if (tasks.Count >= maxParalelTask)
					{
						Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
						tasks.Remove(task);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1656);
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1659);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1660);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{Line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1664);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1665);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public static void SetVisitorGuid(List<Guid> lstCusGuid, object VisitorGuid)
	{
		throw new NotImplementedException();
	}

	public static void LoadGardesh_Pre(Guid selectedGuid)
	{
		throw new NotImplementedException();
	}

	public static void UpdatePayamak()
	{
		throw new NotImplementedException();
	}

	public void Save2Payamak()
	{
	}

	public async Task<List<TafsilNumberBusiness>> GetTafsilNumbersAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(_numberList?.Any() ?? false))
			{
				_numberList = await TafsilNumberBusiness.GetAsync(ContextGuid, Guid, token);
			}
			return _numberList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1764);
			throw;
		}
	}

	public void AddTafsilNumbers(TafsilNumberBusiness number)
	{
		try
		{
			if (number != null)
			{
				if (_numberList == null)
				{
					_numberList = new List<TafsilNumberBusiness>();
				}
				number.TafsilGuid = Guid;
				_numberList.Add(number);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddTafsilNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1779);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeChequeBargashtiValidationAsync(string contextGuid, List<Guid> lst, ReturnedState state, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				using (new JaegerMonitor("ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1793), ReturnedState.None, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1793);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.ChangeChequeBargashtiValidationAsync(lst, state, token, tranName), ReturnedState.None, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1795);
				}
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1797);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1800);
				res.AddReturnedValue(ex, "", "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1801);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1806), ReturnedState.None, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1806);
			}
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeOrderTimeValidationAsync(string contextGuid, List<Guid> lst, ReturnedState state, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				using (new JaegerMonitor("ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1820), ReturnedState.None, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1820);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.ChangeOrderTimeValidationAsync(lst, state, token, tranName), ReturnedState.None, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1822);
				}
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1824);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1827);
				res.AddReturnedValue(ex, "", "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1828);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1833), ReturnedState.None, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1833);
			}
		}
		return res;
	}

	internal async Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1845), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1845);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer.UpdateSendToServerDateAsync(Guid, token, tranName), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1847);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1851);
				ret.AddReturnedValue(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1852);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1857), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1857);
			}
		}
		return ret;
	}

	internal async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(ServerStatusForClient serverStatus, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1869), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1869);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomer.UpdateServerStatusAsync(Guid, serverStatus, token, tranName), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1871);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1875);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1876);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1881), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1881);
			}
		}
		return ret;
	}

	public static Task<CusBusiness> GetByNationalCodeAsync(string contextGuid, string nationalCode, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetByNationalCodeAsync(nationalCode, token, tranName);
	}

	public async Task<CustomerRoleBusiness> GetRoleAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if ((RoleGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			return await CustomerRoleBusiness.GetAsync(ContextGuid, RoleGuid.Value, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1896);
			throw;
		}
	}

	internal static async Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(string contextGuid, EnPriceType priceType, Guid roleGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1908), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1908);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.UpdateRoleGuidAsync(priceType, roleGuid, token, tranName), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1910);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1914);
				ret.AddReturnedValue(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1915);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1920), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1920);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(string contextGuid, List<Guid> customerList, Guid roleGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1933), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1933);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.UpdateRoleGuidAsync(customerList, roleGuid, token, tranName), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1935);
				}
				catch (OperationCanceledException exception)
				{
					res.AddReturnedValue(exception, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1937);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1940);
					res.AddReturnedValue(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1941);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1946), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1946);
				}
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdatePhoneNumbersAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1959), ReturnedState.None, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1959);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.UpdatePhoneNumbersAsync(token, tranName), ReturnedState.None, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1961);
				}
				catch (OperationCanceledException exception)
				{
					res.AddReturnedValue(exception, "", "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1963);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1966);
					res.AddReturnedValue(ex, "", "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1967);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1972), ReturnedState.None, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1972);
				}
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(string contextGuid, Guid groupGuid, bool value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1985), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1985);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.UpdateSpecialAsync(groupGuid, value, token, tranName), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1987);
				}
				catch (OperationCanceledException exception)
				{
					res.AddReturnedValue(exception, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1989);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1992);
					res.AddReturnedValue(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1993);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1998), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusBusiness.cs", 1998);
				}
			}
			return res;
		}
	}
}
