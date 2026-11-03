using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products;
using EntityCache.Business.Tickets;
using Ertegha;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Accounting.UserSettings;

namespace AccSqlServerPersistence.Ertegha;

public class AddDefaults
{
	public static async Task<ReturnedSaveFuncInfo> InsertDefaultDataAsync(EnSystem system, ENSource source, string contextGuid, TransferDefault isInsertDefaultData)
	{
		CancellationToken token = default(CancellationToken);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		Stopwatch sw = Stopwatch.StartNew();
		try
		{
			CodingHesab codingHesab = new CodingHesab(contextGuid);
			bool updateAccount = false;
			short tryCount = 10;
			if (isInsertDefaultData.Hesabs)
			{
				if (source == ENSource.Accounting)
				{
					foreach (TafsilGroupBusiness item2 in TafsilLevelDefultObjs.ListAllTafsilGroupsDefault(contextGuid))
					{
						if (await TafsilGroupBusiness.GetAsync(contextGuid, item2.Guid, system) == null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(system, send2Server: false, null, Priority.Low, awaitOnSemaphore: true, await4send: false, isRemoveWithGuid: false, 10, "", token, 0), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 43);
						}
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
				foreach (HesabTypeBusiness item3 in DefualtHesabTypeObjs.ListAllHesabTypeDefault(contextGuid))
				{
					if (await HesabTypeBusiness.GetAsync(contextGuid, item3.Guid) == null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item3.SaveAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 51);
					}
				}
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
				foreach (HesabGroupBusiness item4 in DefualtHesabGroupObjs.ListAllHesabGroup(contextGuid))
				{
					if (await HesabGroupBusiness.GetAsync(contextGuid, item4.Guid) == null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item4.SaveAsync(system, token, awaitOnSemaphore: true, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 58);
					}
				}
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
				if (source == ENSource.Accounting)
				{
					foreach (KolBusiness item5 in codingHesab.GetListKol(system))
					{
						if (await KolBusiness.GetAsync(contextGuid, item5.Guid, token) == null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item5.SaveAsync(system, awaitOnSemaphore: true, updateAccount: false, 0, token, Priority.High, sendToServer: false, await4send: false, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 68);
						}
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					foreach (MoeinBusiness item6 in await codingHesab.GetListMoeinAsync())
					{
						if (await MoeinBusiness.GetAsync(contextGuid, item6.Guid) == null)
						{
							if (item6.Code == 0L)
							{
								MoeinBusiness moeinBusiness = item6;
								moeinBusiness.Code = await MoeinBusiness.NextCodeInGroupAsync(contextGuid, system, item6.KolGuid);
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item6.SaveAsync(system, updateAccount, sendToServer: false, Priority.Low, null, 0, awaitOnSemaphore: true, await4Send: false, 10, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 79);
						}
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					foreach (TafsilBusiness item7 in await codingHesab.GetListTafsilAsync())
					{
						if (await TafsilBusiness.GetAsync(contextGuid, system, item7.Guid, "", default(CancellationToken), "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs") == null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item7.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount, awaitOnSemaphore: true, 10, send2Server: false, await4sent: false, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 88);
						}
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					if (!((await NaghzBusiness.GetAllAsync(contextGuid))?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await NaghzBusiness.SaveRangeAsync(token, DefualtNaghzObjs.ListAllNaghzDefault(contextGuid), tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 94);
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					if (!((await SuggestedBankBusiness.GetAllAsync(contextGuid, true))?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SuggestedBankBusiness.SaveRangeAsync(token, DefaultBankSegestObjs.ListAllSugBanksDefault(contextGuid), tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 99);
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					if (!((await En2FaBusiness.GetAllAsync(contextGuid))?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await En2FaBusiness.SaveRangeAsync(token, DefaultEn2FaObjs.ListAllL2FDefault(contextGuid), tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 104);
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					if (!((await MoeinTafsilGroupRelationsBusiness.GetAllAsync(contextGuid, null, null))?.Any() ?? false))
					{
						foreach (MoeinTafsilGroupRelationsBusiness item8 in DefaultMoeinTafsilLevelRelationsObjs.ListAllMoeinTafsilGroupRelationsDefault(contextGuid))
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item8.SaveAsync(token, awaitOnSemaphore: false, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 110);
						}
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.Hazine && source == ENSource.Accounting)
			{
				foreach (TafsilBusiness item7 in codingHesab.GetListHazine())
				{
					if (await TafsilBusiness.GetAsync(contextGuid, system, item7.Guid, "", default(CancellationToken), "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs") == null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item7.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount, awaitOnSemaphore: true, 10, send2Server: false, await4sent: false, 10), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 124);
					}
				}
			}
			if (isInsertDefaultData.PrdGroup && source == ENSource.Accounting && await PrdGroupBusiness.GetAsync(contextGuid, DefaultObjGuids.PrdGroup) == null)
			{
				PrdGroupBusiness obj = new PrdGroupBusiness(contextGuid)
				{
					Guid = DefaultObjGuids.PrdGroup,
					Name = "[کلیه کالاها]".GetTranslate(),
					Code = 10L,
					Description = "[پیش فرض سیستم]".GetTranslate(),
					FullCode = 10L,
					ParentGuid = Guid.Empty,
					Special = false,
					Modified = DateTime.Now,
					Status = true,
					TechInfoData = new PrdGroupTechnicalInformations().ToString()
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, token, 10, send2Server: true, Priority.Low, awaitOnSemaphore: true, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 148);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
			}
			if (isInsertDefaultData.PrdVahed && source == ENSource.Accounting)
			{
				if (await PrdVahedBusiness.GetAsync(contextGuid, DefaultObjGuids.PrdVahed) == null)
				{
					PrdVahedBusiness obj2 = new PrdVahedBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.PrdVahed,
						Name = "[کیلوگرم]".GetTranslate(),
						Status = true,
						Description = "[پیش فرض سیستم]".GetTranslate(),
						Modified = DateTime.Now
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj2.SaveAsync(system, 10, awaitOnSemaphore: true, send2Server: false, Priority.Low, await4Send: false, token, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 171);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
				if (await PrdVahedBusiness.GetAsync(contextGuid, DefaultObjGuids.PrdVahed2) == null)
				{
					PrdVahedBusiness obj3 = new PrdVahedBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.PrdVahed2,
						Name = "عدد".GetTranslate(),
						Status = true,
						Modified = DateTime.Now,
						Description = "[پیش فرض سیستم]".GetTranslate()
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj3.SaveAsync(system, 10, awaitOnSemaphore: true, send2Server: false, Priority.Low, await4Send: false, token, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 187);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.CustGroup && source == ENSource.Accounting)
			{
				if (await CusGroupBusiness.GetAsync(contextGuid, DefaultObjGuids.CusGroupBed) == null)
				{
					CusGroupBusiness obj4 = new CusGroupBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.CusGroupBed,
						Name = "[بدهکاران تجاری]".GetTranslate(),
						Description = "[پیش فرض سیستم]".GetTranslate(),
						MoeinDefaultGuid = ParentDefaults.MoeinCoding.CLSMoein9,
						Special = false,
						Modified = DateTime.Now,
						Status = true,
						TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.PersonsAndCompany
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj4.SaveAsync(system, send2Server: false, 10, Priority.Low, awaitOnSemaphore: true, await4Send: false, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 212);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
				if (await CusGroupBusiness.GetAsync(contextGuid, DefaultObjGuids.CusGroupBes) == null)
				{
					CusGroupBusiness obj5 = new CusGroupBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.CusGroupBes,
						Name = "[بستانکاران تجاری]".GetTranslate(),
						Description = "[پیش فرض سیستم]".GetTranslate(),
						MoeinDefaultGuid = ParentDefaults.MoeinCoding.CLSMoein9,
						Special = false,
						Modified = DateTime.Now,
						Status = true,
						TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.PersonsAndCompany
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj5.SaveAsync(system, send2Server: false, 10, Priority.Low, awaitOnSemaphore: true, await4Send: false, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 230);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.CustTitle && source == ENSource.Accounting)
			{
				if (await CusTitleBusiness.GetAsync(contextGuid, DefaultObjGuids.CusTitle) == null)
				{
					CusTitleBusiness obj6 = new CusTitleBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.CusTitle,
						Name = "[آقای]".GetTranslate(),
						Status = true,
						Modified = DateTime.Now
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj6.SaveAsync(system, 0, sendToServer: false, Priority.Low, awaitOnSemaphore: true, await4Send: false, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 251);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
				if (await CusTitleBusiness.GetAsync(contextGuid, DefaultObjGuids.CusTitle2) == null)
				{
					CusTitleBusiness obj7 = new CusTitleBusiness(contextGuid)
					{
						Guid = DefaultObjGuids.CusTitle2,
						Name = "[خانم]".GetTranslate(),
						Status = true,
						Modified = DateTime.Now
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj7.SaveAsync(system, 0, sendToServer: false, Priority.Low, awaitOnSemaphore: true, await4Send: false, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 265);
				}
			}
			if (isInsertDefaultData.Sandogh && source == ENSource.Accounting && await SandoghBusiness.GetAsync(contextGuid, 202000L, system) == null)
			{
				SandoghBusiness obj8 = await SandoghBusiness.GetFactoryAsync(new TafsilBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Name = "[صندوق مرکزی یک]".GetTranslate().FixString(),
					Modified = DateTime.Now,
					Description = "[پیش فرض سیستم]".GetTranslate(),
					TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.Sandooghs,
					Status = true
				}, system);
				obj8.Code = 202000L;
				bool send2Server = source == ENSource.Accounting;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj8.SaveAsync(system, token, awaitOnSemaphore: true, updateAccount, send2Server, tryCount, "", 0), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 289);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
			}
			if (isInsertDefaultData.FareOrderCredit && source == ENSource.Accounting)
			{
				IEnumerable<FareOrderCreditBusiness> enumerable = await FareOrderCreditBusiness.GetAllAsync(contextGuid, true);
				if (enumerable == null || enumerable.Count() <= 0)
				{
					Guid tafsilGroupGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true)).FirstOrDefault().TafsilGroupGuid;
					TafsilGroupBusiness item2 = await TafsilGroupBusiness.GetAsync(contextGuid, tafsilGroupGuid, system);
					FareOrderCreditBusiness fareOrderCreditBusiness = new FareOrderCreditBusiness(contextGuid)
					{
						Guid = Guid.NewGuid()
					};
					FareOrderCreditBusiness fareOrderCreditBusiness2 = fareOrderCreditBusiness;
					fareOrderCreditBusiness2.TafsilGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true)).FirstOrDefault().Guid;
					FareOrderCreditBusiness fareOrderCreditBusiness3 = fareOrderCreditBusiness;
					fareOrderCreditBusiness3.MoeinGuid = await item2.GetMoeinGuidAsync();
					fareOrderCreditBusiness.Status = true;
					fareOrderCreditBusiness.ViewOrder = 1;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await fareOrderCreditBusiness.SaveAsync(system, token, awaitOnSemaphore: true, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 314);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.Depot && source == ENSource.Accounting)
			{
				string name = "[انبار مرکزی]".GetTranslate();
				if (await DepotBusiness.GetAsync(contextGuid, name) == null)
				{
					DepotBusiness obj9 = new DepotBusiness(contextGuid)
					{
						Guid = Guid.NewGuid(),
						Modified = DateTime.Now,
						Name = name,
						Description = "[پیش فرض سیستم]".GetTranslate(),
						NegativeAmount = ReturnedState.Warning,
						Status = true
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj9.SaveAsync(system, token, sendToServer: false, Priority.Low, await4Send: false, awaitOnSemaphore: true, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 338);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.LocalsSettings && source == ENSource.Accounting && string.IsNullOrEmpty(await LocalSettingBusiness.GetAsync(contextGuid, AccGlobalSettings.GetKeyName(""))))
			{
				LocalSettingBusiness obj10 = new LocalSettingBusiness(contextGuid)
				{
					Key = AccGlobalSettings.GetKeyName(""),
					Value = new AccGlobalSettings
					{
						Customers = 
						{
							CustomerMotefaregheGuidList = DefaultObjGuids.Cus.ToString()
						}
					}.ToString()
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj10.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 356);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
				LocalSettingBusiness obj11 = new LocalSettingBusiness(contextGuid)
				{
					Key = "CreateDefualt",
					Value = false.ToString()
				};
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj11.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 360);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
			}
			if (isInsertDefaultData.Bardasht && source == ENSource.Accounting && await TafsilBusiness.GetAsync(contextGuid, system, 205000L, "", default(CancellationToken), "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs") == null)
			{
				TafsilBusiness obj12 = new TafsilBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Name = "[برداشت مالک]".GetTranslate(),
					Status = true,
					System = true,
					HType = HesabType.Bardasht,
					DateM = DateTime.Now,
					Modified = DateTime.Now,
					TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.Bardashts,
					Description = "[پیش فرض سیستم]".GetTranslate(),
					Code = 205000L
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj12.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount, awaitOnSemaphore: true, 10, send2Server: false, await4sent: false, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 386);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
			}
			if (isInsertDefaultData.Customers && source == ENSource.Accounting && await CusBusiness.GetAsync(contextGuid, system, DefaultObjGuids.Cus) == null)
			{
				CusBusiness obj13 = new CusBusiness(contextGuid)
				{
					Guid = DefaultObjGuids.Cus,
					Name = "1",
					Family = "[_متفرقه]".GetTranslate(),
					Date = DateTime.Now,
					GroupGuid = DefaultObjGuids.CusGroupBed,
					TitleGuid = DefaultObjGuids.CusTitle,
					Description = "",
					Address1 = "",
					Address2 = "",
					CodeMeli = "",
					Company = "",
					EghtesadiNo = "",
					Email = "",
					Job = "",
					Location = "",
					Password = "",
					PicFace = "",
					PicSign = "",
					Zip = "",
					User = DefaultObjGuids.Cus.ToString()
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj13.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, updateAccount, 10, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, awaitForSend: false, null, tryCount), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 425);
				if (ret.HasError)
				{
					ret.throwExceptionIfError();
				}
			}
			if (isInsertDefaultData.User && source == ENSource.Accounting)
			{
				IEnumerable<UserAccBusiness> enumerable2 = await UserAccBusiness.GetAllAsync(contextGuid, true);
				if (enumerable2 == null || enumerable2.Count() <= 0)
				{
					UserAccBusiness user1 = new UserAccBusiness(contextGuid, Guid.NewGuid())
					{
						Name = "[گروه کاربری پیش فرض]".GetTranslate(),
						ServerStatus = ServerStatusForClient.None,
						Status = true,
						Modified = DateTime.Now,
						Description = "",
						ContextGuid = contextGuid,
						DeliveryDate = null,
						DeliveryId = 0,
						UserTime = null,
						UserAccess = new AccessLevel(),
						HostRefrenced = "",
						DeliveryMessage = "",
						DeliveryShortMessage = ""
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await user1.SaveAsync(sendToServer: false, system, Priority.Low, awaitOnSemaphore: true, await4Send: false, 10, "", default(CancellationToken), 0), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 455);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					UserAccBusiness obj14 = new UserAccBusiness(contextGuid, DefaultObjGuids.User2Guid)
					{
						Name = "[فروشگاه اینترنتی]".GetTranslate(),
						Status = true,
						DeliveryDate = DateTime.Now,
						Modified = DateTime.Now
					};
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj14.SaveAsync(sendToServer: false, system, Priority.Low, awaitOnSemaphore: true, await4Send: false, tryCount, "", token, 0), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 465);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					CashierBusiness cashierBusiness = new CashierBusiness(contextGuid)
					{
						Guid = user1.Guid,
						UserGuid = user1.Guid,
						UserName = "1",
						Name = "[کاربر پیش فرض کلمه عبور 1111]".GetTranslate(),
						HashedPassword = "1111".CalcHashSafe(user1.Guid.ToByteArray()),
						CashierTime = null,
						ContextGuid = contextGuid,
						DateM = DateTime.Now,
						Description = "",
						Modified = DateTime.Now,
						Status = true,
						ServerStatus = ServerStatusForClient.None
					};
					CashierBusiness cashierBusiness2 = cashierBusiness;
					cashierBusiness2.SandoghGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true))?.FirstOrDefault()?.Guid;
					cashierBusiness.AllBanksAccess = true;
					cashierBusiness.AllDepotsAccess = true;
					cashierBusiness.AllSandoghsAccess = true;
					cashierBusiness.AllVisitorAccess = true;
					cashierBusiness.BankGuid1 = null;
					cashierBusiness.BankGuid2 = null;
					cashierBusiness.BankGuid3 = null;
					cashierBusiness.BankGuid4 = null;
					CashierBusiness cashierBusiness3 = cashierBusiness;
					cashierBusiness3.DefaultCusGroup = (await CusGroupBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
					cashierBusiness.VisitorGuid = null;
					CashierBusiness cashierBusiness4 = cashierBusiness;
					cashierBusiness4.DepotGuid = (await DepotBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid;
					CashierBusiness defCahier = cashierBusiness;
					defCahier.LoginAccess = 7;
					defCahier.SetSetting(new UserSettings());
					defCahier.SetCusGuid(CusBusiness.Get(contextGuid, Guid.Empty)?.Guid);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await defCahier.SaveAsync(sendToServer: false, null, 0, system, Priority.Low, awaitOnSend: false, awaitOnSemaphore: true, 10, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 498);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					cashierBusiness2 = new CashierBusiness(contextGuid)
					{
						Name = "[کاربر پیش فرض برای گروه کاربری 1]".GetTranslate(),
						UserName = "2",
						Guid = DefaultObjGuids.Cashier,
						Modified = DateTime.Now,
						UserGuid = user1.Guid,
						DateM = DateTime.Now,
						Status = true,
						ServerStatus = ServerStatusForClient.None,
						Description = ""
					};
					cashierBusiness = cashierBusiness2;
					cashierBusiness.SandoghGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true))?.FirstOrDefault()?.Guid;
					cashierBusiness2.AllBanksAccess = true;
					cashierBusiness2.AllDepotsAccess = true;
					cashierBusiness2.AllSandoghsAccess = true;
					cashierBusiness2.AllVisitorAccess = true;
					cashierBusiness2.BankGuid1 = null;
					cashierBusiness2.BankGuid2 = null;
					cashierBusiness2.BankGuid3 = null;
					cashierBusiness2.BankGuid4 = null;
					cashierBusiness4 = cashierBusiness2;
					cashierBusiness4.DefaultCusGroup = (await CusGroupBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
					cashierBusiness2.VisitorGuid = null;
					cashierBusiness3 = cashierBusiness2;
					cashierBusiness3.DepotGuid = (await DepotBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid;
					CashierBusiness cashierBusiness5 = cashierBusiness2;
					defCahier.LoginAccess = 1;
					cashierBusiness5.HashedPassword = CashierBusiness.CalcHashedPassword("1111", cashierBusiness5.Guid);
					cashierBusiness5.SetCusGuid(CusBusiness.Get(contextGuid, Guid.Empty)?.Guid);
					cashierBusiness5.SetSetting(new UserSettings());
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cashierBusiness5.SaveAsync(sendToServer: false, null, 0, system, Priority.Low, awaitOnSend: false, awaitOnSemaphore: true, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 530);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
					cashierBusiness = new CashierBusiness(contextGuid)
					{
						Name = "[کاربر پیش فرض برای گروه فروشگاه اینترنتی]".GetTranslate(),
						UserName = "1000",
						Guid = DefaultObjGuids.Cashier,
						Modified = DateTime.Now,
						UserGuid = user1.Guid,
						DateM = DateTime.Now,
						Status = true,
						ServerStatus = ServerStatusForClient.None,
						Description = ""
					};
					cashierBusiness2 = cashierBusiness;
					cashierBusiness2.SandoghGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true))?.FirstOrDefault()?.Guid;
					cashierBusiness.AllBanksAccess = true;
					cashierBusiness.AllDepotsAccess = true;
					cashierBusiness.AllSandoghsAccess = true;
					cashierBusiness.AllVisitorAccess = true;
					cashierBusiness.BankGuid1 = null;
					cashierBusiness.BankGuid2 = null;
					cashierBusiness.BankGuid3 = null;
					cashierBusiness.BankGuid4 = null;
					cashierBusiness3 = cashierBusiness;
					cashierBusiness3.DefaultCusGroup = (await CusGroupBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
					cashierBusiness.VisitorGuid = null;
					cashierBusiness4 = cashierBusiness;
					cashierBusiness4.DepotGuid = (await DepotBusiness.GetAllAsync(contextGuid, system, true))?.FirstOrDefault()?.Guid;
					CashierBusiness cashierBusiness6 = cashierBusiness;
					cashierBusiness6.LoginAccess = 7;
					cashierBusiness6.SetSetting(new UserSettings
					{
						CusMotafaregheList = { DefaultObjGuids.Cus }
					});
					cashierBusiness6.HashedPassword = CashierBusiness.CalcHashedPassword("+* Novin05137597599_=", cashierBusiness6.Guid);
					cashierBusiness6.SetCusGuid(CusBusiness.Get(contextGuid, Guid.Empty)?.Guid);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await cashierBusiness6.SaveAsync(sendToServer: false, null, 0, system, Priority.Low, awaitOnSend: false, awaitOnSemaphore: true, tryCount, "", token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 561);
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
			if (isInsertDefaultData.PriceLevel && source == ENSource.Accounting)
			{
				foreach (ProductPriceLevelBusiness item in DefualtPriceLevelObjs.ListAllDefault(contextGuid))
				{
					if (await ProductPriceLevelBusiness.GetAsync(contextGuid, item.Guid, EnSystem.Mapper, token) == null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer: false, Priority.Low, token), ReturnedState.None, "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 573);
					}
					if (ret.HasError)
					{
						ret.throwExceptionIfError();
					}
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 580);
			WebErrorLog.Instance.StartLog(ex, "", "InsertDefaultDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 581);
		}
		finally
		{
			Console.WriteLine("-------------------\r\n" + $"InsertDefaults has been finished on {sw.ElapsedMilliseconds:N0}(ms)\r\n" + "-------------------\r\n");
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> InsertDefaultGlobalDataAsync(string contextGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			string key = "InsertDefaultGlobalData";
			if ((await LocalSettingBusiness.GetAsync(contextGuid, key))?.ParseToBoolean() ?? false)
			{
				return res;
			}
			if (res.HasError)
			{
				return res;
			}
			"[لیست اعضای پیش فرض]".GetTranslate();
			string salesTicketPartName = "[واحد فروش]".GetTranslate();
			if (await TicketPartBusiness.GetAsync(contextGuid, salesTicketPartName) == null)
			{
				TicketPartBusiness obj = new TicketPartBusiness(contextGuid)
				{
					ContextGuid = contextGuid,
					Description = "[پیش فرض سیستم]".GetTranslate(),
					Guid = Guid.NewGuid(),
					Name = salesTicketPartName,
					Ordinal = 2,
					ParentGuid = null
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.None, 0, sendToServer: true, await4Send: false, 10), ReturnedState.None, "InsertDefaultGlobalDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 616);
			}
			if (res.HasError)
			{
				return res;
			}
			string supportTicketPartName = "[واحد پشتیبانی]".GetTranslate();
			if (await TicketPartBusiness.GetAsync(contextGuid, supportTicketPartName) == null)
			{
				TicketPartBusiness obj2 = new TicketPartBusiness(contextGuid)
				{
					ContextGuid = contextGuid,
					Description = "[پیش فرض سیستم]".GetTranslate(),
					Guid = Guid.NewGuid(),
					Name = supportTicketPartName,
					Ordinal = 1,
					ParentGuid = null
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await obj2.SaveAsync(EnSystem.None, 0, sendToServer: true, await4Send: false, 10), ReturnedState.None, "InsertDefaultGlobalDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 634);
			}
			if (!res.HasError)
			{
				await LocalSettingBusiness.SaveAsync(contextGuid, key, true.ToString(), awaitOnSemaphore: true, 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InsertDefaultGlobalDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 642);
			res.AddReturnedValue(ex, "", "InsertDefaultGlobalDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\AddDefaults.cs", 643);
		}
		return res;
	}
}
