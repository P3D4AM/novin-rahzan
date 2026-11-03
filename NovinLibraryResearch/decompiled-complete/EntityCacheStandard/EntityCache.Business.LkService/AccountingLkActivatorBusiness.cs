using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.LkService.Interfaces;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.LkService;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.LkService;

namespace EntityCache.Business.LkService;

public class AccountingLkActivatorBusiness : LkActivatorBaseBusiness
{
	private static Dictionary<string, AccountingPackageClass> _dic = new Dictionary<string, AccountingPackageClass>();

	private static bool _flag = false;

	public AccountingLkActivatorBusiness(string contextGuid, ENSource source, ILkClientManager instance)
		: base(contextGuid, source, instance)
	{
	}

	private async Task<ReturnedSaveFuncInfoWithValue<AccountingPackageClass>> CheckActivationAsync(ILkUiFactory owner, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<AccountingPackageClass> ret = new ReturnedSaveFuncInfoWithValue<AccountingPackageClass>();
		try
		{
			ret.value = new AccountingPackageClass();
			if (base.Source == ENSource.Baje || base.Source == ENSource.Department)
			{
				ret.value.Level = Level_Accounting.ForoshgahiPishrafteh;
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<LkDemoInformation> checkDemoResult = await CheckDemoAsync(Package.Accounting, token);
			ret.AddReturnedValue(checkDemoResult, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 35);
			if (ret.HasError)
			{
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<LockInformation> lkInfo = await GetLockInformationAsync(Package.Accounting, token);
			ret.AddReturnedValue(lkInfo, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 39);
			ReturnedSaveFuncInfoWithValue<string> softwareSerialResult = await GetSoftwareSerialAccountingAsync(token);
			ret.AddReturnedValue(softwareSerialResult, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 41);
			string serialAccounting = softwareSerialResult.value.RemoveNoNumbers();
			ret.value.SoftwareSerial = serialAccounting;
			string lkSerial = (await GetLkSerialAsync(token))?.value.ToString()?.RemoveNoNumbers()?.Trim();
			if (checkDemoResult.value.IsDemo)
			{
				List<string> lstUrl = new List<string>
				{
					"http://" + ResourceManagement.TextBrandResourceManagement.Url1,
					"http://" + ResourceManagement.TextBrandResourceManagement.Url2,
					"http://" + ResourceManagement.TextBrandResourceManagement.Url3
				};
				bool flag = true;
				if (!_flag)
				{
					flag = base.ClientManagerInstance.ShowDemoForm(Package.Accounting);
				}
				if (flag)
				{
					lkInfo.value.IsDemo = true;
					_flag = true;
					ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await LkActivatorBaseBusiness.SetLockInformationAsync(base.ContextGuid, lkInfo.value, Package.Accounting, token), ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 62);
					ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue2 = await GetAccountingPackage(ret.value.Level, token);
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 64);
					ret.value = returnedSaveFuncInfoWithValue2.value;
					ret.value.Level = Level_Accounting.ForoshgahiPishrafteh;
					FireOnProcessMessage(lstUrl);
					return ret;
				}
				ret.AddError("لغو فعالسازی نرم افزار توسط کاربر.".GetTranslate(), "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 72);
				FireOnKillProcess(owner, ret);
				return ret;
			}
			if (base.ClientManagerInstance.LkClient.IsClient && string.IsNullOrEmpty(checkDemoResult.value?.TinyErrorCode) && checkDemoResult.value.TinyErrorCode != "0" && !(lkInfo?.value?.IsDemo).GetValueOrDefault())
			{
				string uniqueId = PacketParser.Utilities.UniqueIdentifier();
				ReturnedSaveFuncInfoWithValue<clsLkError> connectionResult = await CheckValidLkClientAsync(uniqueId, Package.Accounting, token);
				ret.AddReturnedValue(connectionResult, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 81);
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = await GetClientSetAsync(Package.Accounting, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 83);
				_ = returnedSaveFuncInfoWithValue3.value;
				ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue4 = await GetTypeAsync(softwareSerialResult.value, base.ClientManagerInstance.LkClient.LkSerial.ToString(), token);
				ret.value.Level = returnedSaveFuncInfoWithValue4.value;
				if (!connectionResult.value.HasError)
				{
					ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue5 = await GetAccountingPackage(ret.value.Level, token);
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue5, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 90);
					ret.value = returnedSaveFuncInfoWithValue5.value;
					return ret;
				}
				if (connectionResult.value.Error == LkClientServerErrors.ClientListIsFull)
				{
					ret.AddError("خطا کد 1 :تعداد کلاینت ها بیشتر از حد مجاز است".GetTranslate(), "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 97);
					List<string> urlList = new List<string>
					{
						"http://" + ResourceManagement.TextBrandResourceManagement.Url1,
						"http://" + ResourceManagement.TextBrandResourceManagement.Url2,
						"http://" + ResourceManagement.TextBrandResourceManagement.Url3
					};
					FireOnProcessMessage(urlList);
					FireOnKillProcess(owner, ret);
					return ret;
				}
			}
			ret.value.Level = Level_Accounting.Activating;
			ret.value.LkSerial = lkSerial.ParseToLong(0L);
			if (lkSerial.Length != 12)
			{
				ret.value.Level = Level_Accounting.LkIsDissconnected;
				ret.AddError("قفل سخت افزاری شناسایی نشد،لطفا قفل سخت افزاری را متصل نموده و مجددا برنامه را اجرا نمایید.".GetTranslate(), "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 116);
				FireOnKillProcess(owner, ret);
			}
			ReturnedSaveFuncInfoWithValue<string> trueSadeCode = await GetAccountingSadeActivationCodeAsync(serialAccounting, lkSerial, AccountingPackage.General, 0, 0, DateTime.Now, token);
			ReturnedSaveFuncInfoWithValue<string> trueMotavasetCode = await GetAccountingMotevasetActivationCodeAsync(serialAccounting, lkSerial, AccountingPackage.General, 0, 0, DateTime.Now, token);
			ReturnedSaveFuncInfoWithValue<string> truePishraftehCode = await GetAccountingPishrafteActivationCodeAsync(serialAccounting, lkSerial, AccountingPackage.General, 0, 0, DateTime.Now, token);
			ReturnedSaveFuncInfoWithValue<string> trueJameCode = await GetAccountingJameActivationCodeAsync(serialAccounting, lkSerial, AccountingPackage.General, 0, 0, DateTime.Now, token);
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue6 = await CheckDaysAsync(Package.Accounting, lkInfo?.value, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue6, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 126);
			if (returnedSaveFuncInfoWithValue6.value)
			{
				if (lkInfo?.value?.AccountingActivationCode == trueSadeCode.value)
				{
					ret.value.Level = Level_Accounting.ForoshgahiSade;
				}
				else if (lkInfo?.value?.AccountingActivationCode == trueMotavasetCode.value)
				{
					ret.value.Level = Level_Accounting.ForoshgahiMotavaset;
				}
				else if (lkInfo?.value?.AccountingActivationCode == truePishraftehCode.value)
				{
					ret.value.Level = Level_Accounting.ForoshgahiPishrafteh;
				}
				else if (lkInfo?.value?.AccountingActivationCode == trueJameCode.value)
				{
					ret.value.Level = Level_Accounting.Jame;
				}
			}
			if (ret.value.Level == Level_Accounting.Activating)
			{
				returnedSaveFuncInfoWithValue6 = await CheckDaysAsync(Package.Accounting, lkInfo?.value, token);
				bool flag2 = base.ClientManagerInstance.ShowActivationForm(ret.value, Package.Accounting, (checkDemoResult?.value?.IsDemo).GetValueOrDefault());
				if (!flag2)
				{
					ret.AddError("خطا در فعال سازی قفل".GetTranslate(), "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 145);
					return ret;
				}
				if (flag2 && returnedSaveFuncInfoWithValue6.value)
				{
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue7 = await GetActivationCodeAsync(Package.Accounting, token);
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue7, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 151);
					if (returnedSaveFuncInfoWithValue7.value == trueSadeCode.value)
					{
						ret.value.Level = Level_Accounting.ForoshgahiSade;
					}
					else if (returnedSaveFuncInfoWithValue7.value == trueMotavasetCode.value)
					{
						ret.value.Level = Level_Accounting.ForoshgahiMotavaset;
					}
					else if (returnedSaveFuncInfoWithValue7.value == truePishraftehCode.value)
					{
						ret.value.Level = Level_Accounting.ForoshgahiPishrafteh;
					}
					else if (returnedSaveFuncInfoWithValue7.value == trueJameCode.value)
					{
						ret.value.Level = Level_Accounting.Jame;
					}
					else
					{
						FireOnProcessCrm(lkSerial + softwareSerialResult.value);
					}
				}
				else
				{
					ret.value.Level = Level_Accounting.ActivationFailed;
					FireOnProcessCrm(lkSerial + softwareSerialResult.value);
				}
			}
			ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue8 = await GetAccountingPackage(ret.value.Level, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue8, ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 169);
			ret.value = returnedSaveFuncInfoWithValue8.value;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 174);
			WebErrorLog.Instance.StartLog(ex, "", "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 175);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> GetTypeAsync(string softwareSerial, string hardwareSerial, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<Level_Accounting> ret = new ReturnedSaveFuncInfoWithValue<Level_Accounting>();
		try
		{
			hardwareSerial = hardwareSerial.Replace("-", "");
			if (string.IsNullOrEmpty(hardwareSerial))
			{
				ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue = await new LkDemoBusiness(base.ContextGuid).GetIsDemoAsync(LkActivatorBaseBusiness.VersionAccounting, Package.Accounting, ignoreHttpExceptions: true, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 189);
				if (returnedSaveFuncInfoWithValue.value.IsDemo)
				{
					ret.value = Level_Accounting.ForoshgahiPishrafteh;
				}
				else
				{
					ret.value = Level_Accounting.LkIsDissconnected;
				}
				return ret;
			}
			new LkActivationCodeModel
			{
				LkSerial = hardwareSerial.ParseToLong(0L),
				SerialSoft = softwareSerial,
				UserName = DepAdmin.UserName,
				Password = DepAdmin.Password
			};
			ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue2 = await GetApplicationTypeAsync(softwareSerial, hardwareSerial, (await GetActivationCodeAsync(Package.Accounting, default(CancellationToken))).value.RemoveNoNumbers(), token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 203);
			ret.value = returnedSaveFuncInfoWithValue2.value;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "GetTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 208);
			WebErrorLog.Instance.StartLog(ex, "", "GetTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 209);
			ret.value = Level_Accounting.none;
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<AccountingPackageClass>> CheckActivationAsync(string contextGuid, ENSource source, ILkClientManager instance, ILkUiFactory owner, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<AccountingPackageClass> res = new ReturnedSaveFuncInfoWithValue<AccountingPackageClass>();
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				return null;
			}
			_dic.TryGetValue(contextGuid, out var value);
			if (value != null)
			{
				res.value = value;
				return res;
			}
			AccountingLkActivatorBusiness accountingLkActivatorBusiness = new AccountingLkActivatorBusiness(contextGuid, source, instance);
			ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await accountingLkActivatorBusiness.CheckActivationAsync(owner, token), ReturnedState.None, "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 229);
			_dic.Add(contextGuid, res.value);
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 236);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<AccountingPackageClass>> GetAccountingPackage(Level_Accounting defaultLevel, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<AccountingPackageClass> res = new ReturnedSaveFuncInfoWithValue<AccountingPackageClass>
		{
			value = new AccountingPackageClass()
		};
		try
		{
			ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = await GetLockInformationAsync(Package.Accounting, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAccountingPackage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 246);
			if (res.HasError || returnedSaveFuncInfoWithValue?.value == null)
			{
				return res;
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.TolidActivationCode))
			{
				res.value.Tolid = new PackageContainer(EnLK_SpecialFacilities.Tolid, isActive: true, returnedSaveFuncInfoWithValue.value.TolidActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.BarcodeActivationCode))
			{
				res.value.Barcode = new PackageContainer(EnLK_SpecialFacilities.Barcode, isActive: true, returnedSaveFuncInfoWithValue.value.BarcodeActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.TaxActivationCode))
			{
				res.value.Tax = new PackageContainer(EnLK_SpecialFacilities.Tax, isActive: true, returnedSaveFuncInfoWithValue.value.TaxActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.AghsatActivationCode))
			{
				res.value.Installment = new PackageContainer(EnLK_SpecialFacilities.Aghsat, isActive: true, returnedSaveFuncInfoWithValue.value.AghsatActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.Cheque_KhazaneActivationCode))
			{
				res.value.Cheque = new PackageContainer(EnLK_SpecialFacilities.Cheque, isActive: true, returnedSaveFuncInfoWithValue.value.Cheque_KhazaneActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.CustomerClubActivationCode))
			{
				res.value.CustomerClub = new PackageContainer(EnLK_SpecialFacilities.CustomerClub, isActive: true, returnedSaveFuncInfoWithValue.value.CustomerClubActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.ProductDepot_PishraftehActivationCode))
			{
				res.value.Depot = new PackageContainer(EnLK_SpecialFacilities.Depot, isActive: true, returnedSaveFuncInfoWithValue.value.ProductDepot_PishraftehActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.ServiceActivationCode))
			{
				res.value.Service = new PackageContainer(EnLK_SpecialFacilities.Service, isActive: true, returnedSaveFuncInfoWithValue.value.ServiceActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.VoiceCallActivationCode))
			{
				res.value.VoiceCall = new PackageContainer(EnLK_SpecialFacilities.VoiceCall, isActive: true, returnedSaveFuncInfoWithValue.value.VoiceCallActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.ProductSerialActivationCode))
			{
				res.value.Serial = new PackageContainer(EnLK_SpecialFacilities.ProductSerial, isActive: true, returnedSaveFuncInfoWithValue.value.ProductSerialActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.MultiCurrencyActivationCode))
			{
				res.value.MultiCurrency = new PackageContainer(EnLK_SpecialFacilities.MultiCurrency, isActive: true, returnedSaveFuncInfoWithValue.value.MultiCurrencyActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.SpecialActivationCode))
			{
				res.value.AdvanceFinance = new PackageContainer(EnLK_SpecialFacilities.AdvancedFinance, isActive: true, returnedSaveFuncInfoWithValue.value.SpecialActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.ScaleActivationCode))
			{
				res.value.Scale = new PackageContainer(EnLK_SpecialFacilities.Scale, isActive: true, returnedSaveFuncInfoWithValue.value.ScaleActivationCode);
			}
			if (!string.IsNullOrEmpty(returnedSaveFuncInfoWithValue.value.CallerIdActivationCode))
			{
				res.value.CallerId = new PackageContainer(EnLK_SpecialFacilities.CallerId, isActive: true, returnedSaveFuncInfoWithValue.value.CallerIdActivationCode);
			}
			res.value.SoftwareSerial = returnedSaveFuncInfoWithValue.value.SoftwareSerialAccounting;
			res.value.LkSerial = returnedSaveFuncInfoWithValue.value.LkSerial;
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue2 = await GetLkInfoAsync(token);
			if (returnedSaveFuncInfoWithValue2.value != null && !returnedSaveFuncInfoWithValue2.HasError && returnedSaveFuncInfoWithValue2.value.TypeAccounting == ENCode.none)
			{
				if ((returnedSaveFuncInfoWithValue2.value?.LKSerial ?? 0) > 0)
				{
					res.value.SpecialFacilities = returnedSaveFuncInfoWithValue2.value.SpecialFacilities;
				}
				ENCode? eNCode = returnedSaveFuncInfoWithValue2?.value.TypeAccounting;
				if (eNCode.HasValue)
				{
					switch (eNCode.GetValueOrDefault())
					{
					case ENCode.ForoshgahiSade:
						res.value.Level = Level_Accounting.ForoshgahiSade;
						break;
					case ENCode.ForoshgahiMotavaset:
						res.value.Level = Level_Accounting.ForoshgahiMotavaset;
						break;
					case ENCode.ForoshgahiPishrafteh:
						res.value.Level = Level_Accounting.ForoshgahiPishrafteh;
						break;
					case ENCode.Jame:
						res.value.Level = Level_Accounting.Jame;
						break;
					}
				}
			}
			else
			{
				res.value.Level = defaultLevel;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAccountingPackage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\AccountingLkActivatorBusiness.cs", 296);
		}
		return res;
	}
}
