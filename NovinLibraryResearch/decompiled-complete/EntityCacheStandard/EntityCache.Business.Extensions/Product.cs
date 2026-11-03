using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Products;
using EntityCache.Filters;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Business.Extensions;

public static class Product
{
	public static async Task<ReturnedSaveFuncInfo> CheckPrdAmountAsync(this IEnumerable<ProductsChanges> prdChanges, CheckProductAmountConfig config, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (config.System == EnSystem.Transfer)
			{
				return res;
			}
			_ = ClsCache.GetCache(config.ContextGuid).Source;
			line = 1;
			if (prdChanges?.Any() ?? false)
			{
				IEnumerable<ProductCompactBusiness> prdList = null;
				if (config.FactorType == Document.AnbarGardani)
				{
					ProductCompactFilter filter = new ProductCompactFilter
					{
						LoadJustParents = false,
						Status = true
					};
					prdList = await ProductCompactBusiness.GetAllAsync(config.ContextGuid, config.System, filter, token, tranName);
				}
				foreach (ProductsChanges prdChange in prdChanges)
				{
					if (prdChange == null)
					{
						continue;
					}
					line = 10;
					ProductCompactBusiness prd = ((config.FactorType != Document.AnbarGardani) ? (await prdChange.GetPrdInfoAsync(config.System, token, tranName)) : prdList?.SingleOrDefault((ProductCompactBusiness q) => q.Guid == prdChange.PrdGuid));
					if (prd == null)
					{
						continue;
					}
					line = 110;
					PrdVahedBusiness prdVahedBusiness = await PrdVahedBusiness.GetAsync(config.ContextGuid, prd.GuidVahed, token, tranName);
					line = 12;
					string vahedName = prdVahedBusiness.Name;
					line = 13;
					if (config.System != EnSystem.Transfer && config.IsDeleteAction && prd != null)
					{
						line = 14;
						if (prd.Type == EnProductType.Product)
						{
							line = 15;
							PrdAnbarRelationBusiness prdAnbar2 = await prd.GetAnbarRelationAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
							DepotBusiness depot = await DepotBusiness.GetAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
							decimal am = (prdAnbar2?.Amount ?? 0m) + prdChange.qt;
							line = 20;
							if (am < 0m)
							{
								line = 25;
								ReturnedState state = depot?.NegativeAmount ?? ReturnedState.Warning;
								bool flag = depot.NegativeAmount == ReturnedState.Error;
								if (flag)
								{
									bool flag2 = await prd.GetMojodiAsync(token, tranName) == ProductMojodiSt.DepotDefault;
									if (!flag2)
									{
										flag2 = await prd.GetMojodiAsync(token, tranName) == ProductMojodiSt.DisableMojodiManfi;
									}
									flag = flag2;
								}
								if (flag)
								{
									state = ReturnedState.Error;
								}
								line = 30;
								if (state == ReturnedState.Error || state == ReturnedState.Warning)
								{
									ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
									ReturnedState returnedState = state;
									string translate = "با ابطال".GetTranslate();
									string translate2 = config.FactorType.GetDisplay().GetTranslate();
									string translate3 = " موجودی".GetTranslate();
									string name = prd.Name;
									string text = await prdChange.DepotNameAsync(token, tranName);
									returnedSaveFuncInfo.AddReturnedValue(returnedState, translate + "  " + translate2 + translate3 + "  " + name + "  در  " + text + " به میزان".GetTranslate() + "  (" + (-am).ToString(3) + " " + vahedName + ") " + " منفی میشود.".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 77);
									if (state == ReturnedState.Error)
									{
										res.AddReturnedValue(state, "لطفا قابلیت موجودی منفی را برای انبار فعال نمایید".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 81);
									}
								}
							}
							else if (prdAnbar2 != null && (prdAnbar2.Min > prdAnbar2.Amount + prdChange.qt || (prdAnbar2.Min != 0m && prdAnbar2.Amount + prdChange.qt == 0m)))
							{
								line = 35;
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								string name = "با ابطال".GetTranslate();
								string translate3 = config.FactorType.GetDisplay().GetTranslate();
								string translate2 = " موجودی".GetTranslate();
								string translate = prd.Name;
								string text = await prdChange.DepotNameAsync(token, tranName);
								ReturnedSaveFuncInfo returnedSaveFuncInfo2 = returnedSaveFuncInfo;
								string[] obj = new string[20]
								{
									name,
									"  ",
									translate3,
									translate2,
									" ",
									translate,
									"  در  ",
									text,
									" ",
									"به زیر نقطه سفارش میرسد.".GetTranslate(),
									"\r\n",
									"نقطه سفارش".GetTranslate(),
									": ",
									null,
									null,
									null,
									null,
									null,
									null,
									null
								};
								PrdAnbarRelationBusiness prdAnbarRelationBusiness = prdAnbar2;
								obj[13] = ((prdAnbarRelationBusiness != null) ? (-prdAnbarRelationBusiness.Amount) : 0m).NpToString("N0", 0);
								obj[14] = vahedName;
								obj[15] = "\r\n";
								obj[16] = "موجودی پس از ابطال".GetTranslate();
								obj[17] = ":";
								obj[18] = am.NpToString("N0", 0);
								obj[19] = vahedName;
								returnedSaveFuncInfo2.AddInformation(string.Concat(obj), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 87);
							}
							if (prd != null && depot != null && config.Date.Date != DateTime.Now.Date)
							{
								line = 40;
								if (await KardexBusiness.GetPrdAmountAsync(config.ContextGuid, prd.Guid, depot.Guid, config.Date, true, token, tranName) < 0.0)
								{
									res.AddWarning("با ابطال".GetTranslate() + "  " + config.FactorType.GetDisplay().GetTranslate() + "، موجودی کالای ".GetTranslate() + " " + prd.Name + "  " + "در".GetTranslate() + "  " + depot.Name + " " + "در تاریخ".GetTranslate() + " " + config.Date.MiladiToCalture() + " به میزان".GetTranslate() + " (" + am.NpToString("N0", 0) + vahedName + ")  " + "منفی خواهد شد".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 98);
								}
							}
							line = 45;
						}
						line = 50;
					}
					else if ((config.System != EnSystem.Transfer && prd != null && (config.FactorType == Document.OrderSell || config.FactorType == Document.OrderBackBuy || config.FactorType == Document.PishFactor || config.FactorType == Document.Zayeat || config.FactorType == Document.AmaniKhoroj || config.FactorType == Document.Tolid_Mavad)) || config.FactorType == Document.EnteghalBeyneAnbar)
					{
						line = 55;
						if (prd.Type == EnProductType.Product)
						{
							DepotBusiness depot = await DepotBusiness.GetAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
							PrdAnbarRelationBusiness prdAnbar2 = await prd.GetAnbarRelationAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
							string translate = config.ContextGuid;
							decimal am = await GetStoreBallanceAsync(translate, await prdChange.GetPrdInfoAsync(config.System, token, tranName), prdChange.depGuid, prdChange.qt, token, tranName, config.Date);
							line = 60;
							if (config.Tolid != null)
							{
								OrderBusiness orderBusiness = await config.Tolid.GetExportListAsync(config.System, tranName, token);
								if ((orderBusiness?.ListKala?.Any()).GetValueOrDefault())
								{
									IEnumerable<OrderDetailBusiness> enumerable = orderBusiness.ListKala.Where((OrderDetailBusiness q) => q.ProductGuid == prdChange.PrdGuid);
									if (enumerable != null && enumerable.Any())
									{
										am += Math.Round(enumerable.Sum((OrderDetailBusiness q) => q.Quantity), 3);
									}
								}
							}
							if (am < 0m)
							{
								ReturnedState state = depot?.NegativeAmount ?? ReturnedState.Warning;
								line = 65;
								ProductMojodiSt productMojodiSt = await prd.GetMojodiAsync(token, tranName);
								if ((depot?.NegativeAmount ?? ReturnedState.None) == ReturnedState.Error && (productMojodiSt == ProductMojodiSt.DepotDefault || productMojodiSt == ProductMojodiSt.DisableMojodiManfi))
								{
									state = ReturnedState.Error;
								}
								else if ((depot?.NegativeAmount ?? ReturnedState.None) == ReturnedState.Error && productMojodiSt == ProductMojodiSt.DisableMojodiManfi)
								{
									state = ReturnedState.Error;
								}
								line = 70;
								if ((config.FactorType == Document.PishFactor || config.IsTempOrder) && state == ReturnedState.Error)
								{
									state = ReturnedState.Warning;
								}
								line = 75;
								if (state == ReturnedState.Error || state == ReturnedState.Warning)
								{
									ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
									ReturnedState returnedState = state;
									translate = "با ثبت".GetTranslate();
									string translate2 = config.FactorType.GetDisplay().GetTranslate();
									string translate3 = " موجودی".GetTranslate();
									string name = prd.Name;
									string translate4 = " در".GetTranslate();
									string text = await prdChange.DepotNameAsync(token, tranName);
									returnedSaveFuncInfo.AddReturnedValue(returnedState, translate + "  " + translate2 + " " + translate3 + "  " + name + " " + translate4 + "  " + text + " " + " به میزان ".GetTranslate() + " (" + am.ToString(3) + vahedName + ")  " + "منفی میشود.".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 148);
									line = 80;
									if (state == ReturnedState.Error)
									{
										res.AddReturnedValue(state, "جهت ادامه عملیات باید مشکل موجودی منفی را رفع کنید".GetTranslate() + "\r\n" + "بهتر است از یک حسابدار در اینخصوص مشورت بگیرید".GetTranslate() + "\r\n" + "ولیکن میتوانید با فعال کردن موجودی منفی برای انبار".GetTranslate() + "(" + depot?.Name + ")\r\n" + "موقتا از این خطا گذر کنید (هرچند که اصلا این موضوع پیشنهاد نمیشود و قطعا باید بعدا مشکل را اساسا رفع نمایید".GetTranslate() + ")\r\n" + "همچنین این امکان وجود دارد که موجودی منفی بصورت موردی برای فقط یک یا مجموعه ای از کالا ها نیز فعال گردد.".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 153);
									}
								}
							}
							else if (prdAnbar2 != null && (prdAnbar2.Min > prdAnbar2.Amount + prdChange.qt || (prdAnbar2.Min != 0m && prdAnbar2.Amount + prdChange.qt == 0m)))
							{
								line = 90;
								res.AddReturnedValue(ReturnedState.Information, "با ثبت".GetTranslate() + "  " + config.FactorType.GetDisplay().GetTranslate() + " " + " موجودی".GetTranslate() + "  " + prd?.Name + " " + " در".GetTranslate() + "  " + depot?.Name + " " + " به زیر نقطه سفارش".GetTranslate() + " (" + prdAnbar2.Min.NpToString("N0", 0) + "(" + vahedName + ")) " + "میرسد.".GetTranslate() + "\r\n" + "موجودی بعد از عملیات".GetTranslate() + ":" + am.NpToString("N0", 0) + vahedName, "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 164);
							}
							line = 95;
							if (prd != null && depot != null && config.Date.Date != DateTime.Now.Date)
							{
								line = 96;
								if (await KardexBusiness.GetPrdAmountAsync(config.ContextGuid, prd.Guid, depot.Guid, config.Date, true, token, tranName) < 0.0)
								{
									res.AddWarning("با ثبت".GetTranslate() + "  " + config.FactorType.GetDisplay().GetTranslate() + "، " + " موجودی کالای".GetTranslate() + " " + (" " + prd.Name).GetTranslate() + " در" + (" " + depot.Name).GetTranslate() + " " + " در تاریخ".GetTranslate() + "  " + config.Date.MiladiToCalture() + " به میزان".GetTranslate() + "(" + am.ToString(3) + vahedName + ")" + " منفی خواهد شد".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 176);
								}
							}
							line = 100;
						}
						line = 101;
					}
					else if (config.System != EnSystem.Transfer && !config.IsDeleteAction && config.ChangeStatus != EntityChangeSt.New && prd != null && (config.FactorType == Document.OrderBuy || config.FactorType == Document.OrderBackSell) && prd.Type == EnProductType.Product)
					{
						PrdAnbarRelationBusiness prdAnbar2 = await prd.GetAnbarRelationAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
						DepotBusiness depotBusiness = await DepotBusiness.GetAsync(config.ContextGuid, prdChange.depGuid, token, tranName);
						decimal am = (prdAnbar2?.Amount ?? 0m) + prdChange.qt;
						if (am < 0m)
						{
							ReturnedState state = depotBusiness?.NegativeAmount ?? ReturnedState.Warning;
							bool flag = (depotBusiness?.NegativeAmount ?? ReturnedState.None) == ReturnedState.Error;
							if (flag)
							{
								flag = await prd.GetMojodiAsync(token, tranName) == ProductMojodiSt.DepotDefault;
							}
							bool flag2 = flag;
							if (!flag2)
							{
								flag2 = await prd.GetMojodiAsync(token, tranName) == ProductMojodiSt.DisableMojodiManfi;
							}
							if (flag2)
							{
								state = ReturnedState.Error;
							}
							if (state == ReturnedState.Error || state == ReturnedState.Warning)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								ReturnedState returnedState = state;
								string translate4 = "با ثبت".GetTranslate();
								string name = config.FactorType.GetDisplay().GetTranslate();
								string translate3 = " موجودی".GetTranslate();
								string translate2 = (" " + prd.Name).GetTranslate();
								string translate = " در ".GetTranslate();
								string text = await prdChange.DepotNameAsync(token, tranName);
								returnedSaveFuncInfo.AddReturnedValue(returnedState, translate4 + "  " + name + " " + translate3 + " " + translate2 + " " + translate + " " + text + " " + " به میزان ".GetTranslate() + " (" + (-am).ToString(3) + " " + vahedName + ") " + " منفی میشود.".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 199);
								if (state == ReturnedState.Error)
								{
									res.AddReturnedValue(state, "لطفا قابلیت موجودی منفی را برای انبار فعال نمایید".GetTranslate(), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 203);
								}
							}
						}
						else if (prdAnbar2 != null && (prdAnbar2.Min > prdAnbar2.Amount + prdChange.qt || (prdAnbar2.Min != 0m && prdAnbar2.Amount + prdChange.qt == 0m)))
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							string translate = "با ثبت ".GetTranslate();
							string translate2 = config.FactorType.GetDisplay().GetTranslate();
							string translate3 = " موجودی".GetTranslate();
							string name = prd.Name;
							string translate4 = " در".GetTranslate();
							string text = await prdChange.DepotNameAsync(token, tranName);
							ReturnedSaveFuncInfo returnedSaveFuncInfo3 = returnedSaveFuncInfo;
							string[] obj2 = new string[23]
							{
								translate,
								" ",
								translate2,
								" ",
								translate3,
								"  ",
								name,
								" ",
								translate4,
								"  ",
								text,
								"  ",
								"به زیر نقطه سفارش میرسد.".GetTranslate(),
								"\r\n",
								"نقطه سفارش".GetTranslate(),
								": ",
								null,
								null,
								null,
								null,
								null,
								null,
								null
							};
							PrdAnbarRelationBusiness prdAnbarRelationBusiness2 = prdAnbar2;
							obj2[16] = ((prdAnbarRelationBusiness2 != null) ? (-prdAnbarRelationBusiness2.Amount) : 0m).NpToString("N0", 0);
							obj2[17] = vahedName;
							obj2[18] = "\r\n";
							obj2[19] = "موجودی پس از ابطال".GetTranslate();
							obj2[20] = ":";
							obj2[21] = am.NpToString("N0", 0);
							obj2[22] = vahedName;
							returnedSaveFuncInfo3.AddInformation(string.Concat(obj2), "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 208);
						}
					}
					line = 102;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}//ClsCache is null or Disposed:{ClsCache.GetCache(config.ContextGuid)?.IsDisposed ?? true}", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 222);
			res.AddReturnedValue(ex, "", "CheckPrdAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 223);
		}
		return res;
	}

	public static async Task<decimal> GetStoreBallanceAsync(string contextGuid, ProductCompactBusiness prd, Guid depotGuid, decimal qt, CancellationToken token, string tranName, DateTime? date = null)
	{
		_ = 1;
		try
		{
			if (prd == null)
			{
				return default(decimal);
			}
			decimal num = ((date.HasValue && ClsCache.GetCache(contextGuid).Source == ENSource.Accounting) ? ((await KardexBusiness.GetLastRecordsByDateAsync(contextGuid, date.Value, prd.Guid, depotGuid, token, tranName))?.RemQT).GetValueOrDefault() : ((await prd.GetAnbarRelationAsync(contextGuid, depotGuid, token, tranName))?.Amount ?? 0m));
			return num + qt;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStoreBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 249);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ComapreAsync(this WebProduct current, string contextGuid, EnSystem system, ProductBusiness old, List<BarcodeBusiness> currentBacodes, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (old == null)
			{
				return res.AddError("تعریف کالا".GetTranslate() + "(" + current.Name + ")\r\n" + " کدکالا".GetTranslate() + ":" + $"{current.Code}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 258);
			}
			if (current.Mojodi != old.Mojodi)
			{
				res.AddError("تغییر وضعیت موجودی از".GetTranslate() + old.Mojodi.GetDisplay().GetTranslate() + " به ".GetTranslate() + current.Mojodi.GetDisplay(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 260);
			}
			if ((current.IdProduct ?? "").Trim() != (old.IdProduct ?? "").Trim())
			{
				res.AddError("تغییر کد کالا از".GetTranslate() + $" {old.Code} " + "به".GetTranslate() + $" {current.Code}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 264);
			}
			if ((current.Description ?? "").Trim() != (old.Description ?? "").Trim())
			{
				res.AddError("تغییر شرح کالا از ".GetTranslate() + " " + $"{old.Code} " + "به".GetTranslate() + $" {current.Code}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 265);
			}
			if (current.DiscountPercent != old.DiscountPercent)
			{
				res.AddError("تغییر درصد تخفیف از ".GetTranslate() + $" {old.DiscountPercent} " + "به".GetTranslate() + $" {current.DiscountPercent}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 266);
			}
			if (current.Height != old.Height)
			{
				res.AddError("تغییر عرض کالا از ".GetTranslate() + " " + $"{old.Height} " + "به ".GetTranslate() + " " + $" {current.Height}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 267);
			}
			if ((current.Name ?? "").Trim() != (old.Name ?? "").Trim())
			{
				res.AddError("تغییر نام کالا از".GetTranslate() + " " + old.Name.GetTranslate() + " به".GetTranslate() + " " + current.Name.GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 268);
			}
			if (current.V2Qt != old.V2Qt)
			{
				res.AddError("تغییر تعداد در واحد دوم کالا از ".GetTranslate() + $" {old.V2Qt} " + " " + "به ".GetTranslate() + $" {current.V2Qt}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 269);
			}
			if (current.V3Qt != old.V3Qt)
			{
				res.AddError("تغییر تعداد در واحد سوم کالا از ".GetTranslate() + $" {old.V3Qt} " + " " + "به ".GetTranslate() + $" {current.V3Qt}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 270);
			}
			if (current.V4Qt != old.V4Qt)
			{
				res.AddError("تغییر تعداد در واحد چهارم کالا از ".GetTranslate() + $" {old.V4Qt}" + " " + "به ".GetTranslate() + $" {current.V4Qt}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 271);
			}
			if (current.Width != old.Width)
			{
				res.AddError("تغییر طول کالا از ".GetTranslate() + $" {old.Width} " + "به ".GetTranslate() + $" {current.Width}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 272);
			}
			if (current.Weight != old.Weight)
			{
				res.AddError("تغییر وزن کالا از ".GetTranslate() + $" {old.Weight} " + "به ".GetTranslate() + $" {current.Weight}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 273);
			}
			if (current.IsTax != old.IsTax)
			{
				string text = (current.IsTax ? "فعال".GetTranslate() : "غیرفعال".GetTranslate());
				string text2 = (old.IsTax ? "فعال".GetTranslate() : "غیرفعال".GetTranslate());
				res.AddError("تغییر مشمولیت مالیات از".GetTranslate() + " " + text2 + " " + " به".GetTranslate() + " " + text, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 278);
			}
			if (current.GuidVahed != old.GuidVahed)
			{
				PrdVahedBusiness oldObject = await PrdVahedBusiness.GetAsync(contextGuid, old.GuidVahed, token, tranName);
				PrdVahedBusiness prdVahedBusiness = await PrdVahedBusiness.GetAsync(contextGuid, current.GuidVahed, token, tranName);
				if ((oldObject?.Name ?? "").Trim() != (prdVahedBusiness?.Name ?? "").Trim())
				{
					res.AddError("تغییر واحد کالا از".GetTranslate() + "  " + oldObject?.Name + " " + "به".GetTranslate() + "  " + prdVahedBusiness?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 285);
				}
			}
			if ((current.V2Guid ?? Guid.Empty) != (old.V2Guid ?? Guid.Empty))
			{
				PrdVahedBusiness oldObject = await PrdVahedBusiness.GetAsync(contextGuid, old.V2Guid ?? Guid.Empty, token, tranName);
				PrdVahedBusiness prdVahedBusiness2 = await PrdVahedBusiness.GetAsync(contextGuid, current.V2Guid ?? Guid.Empty, token, tranName);
				if ((oldObject?.Name ?? "").Trim() != (prdVahedBusiness2?.Name ?? "").Trim())
				{
					res.AddError("تغییر واحد دوم کالا از ".GetTranslate() + " " + oldObject?.Name + " " + " به".GetTranslate() + "  " + prdVahedBusiness2?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 292);
				}
			}
			if ((current.V3Guid ?? Guid.Empty) != (old.V3Guid ?? Guid.Empty))
			{
				PrdVahedBusiness oldObject = await PrdVahedBusiness.GetAsync(contextGuid, old.V3Guid ?? Guid.Empty, token, tranName);
				PrdVahedBusiness prdVahedBusiness3 = await PrdVahedBusiness.GetAsync(contextGuid, current.V3Guid ?? Guid.Empty, token, tranName);
				if ((oldObject?.Name ?? "").Trim() != (prdVahedBusiness3?.Name ?? "").Trim())
				{
					res.AddError("تغییر واحد سوم کالا از".GetTranslate() + "  " + oldObject?.Name + " " + " به".GetTranslate() + "  " + prdVahedBusiness3?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 299);
				}
			}
			if ((current.V4Guid ?? Guid.Empty) != (old.V4Guid ?? Guid.Empty))
			{
				PrdVahedBusiness oldObject = await PrdVahedBusiness.GetAsync(contextGuid, old.V4Guid ?? Guid.Empty, token, tranName);
				PrdVahedBusiness prdVahedBusiness4 = await PrdVahedBusiness.GetAsync(contextGuid, current.V4Guid ?? Guid.Empty, token, tranName);
				if ((oldObject?.Name ?? "").Trim() != (prdVahedBusiness4?.Name ?? "").Trim())
				{
					res.AddError("تغییر واحد چهارم کالا از".GetTranslate() + "  " + oldObject?.Name + " " + " به".GetTranslate() + "  " + prdVahedBusiness4?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 306);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ComapreBarcodeAsync(contextGuid, current, old, currentBacodes, token, tranName), ReturnedState.None, "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 309);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ComapreGroupsAsync(contextGuid, current, old, token, tranName), ReturnedState.None, "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 310);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ComaprePricesAsync(contextGuid, system, current, old, token, tranName), ReturnedState.None, "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 311);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 315);
			res.AddReturnedValue(ex, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 316);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ComapreBarcodeAsync(string contextGuid, WebProduct current, ProductBusiness old, List<BarcodeBusiness> currentBacodes, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<BarcodeBusiness> existBarcodeList = await BarcodeBusiness.GetByPrdGuidAsync(contextGuid, new List<Guid> { current.Guid }, token, tranName);
			if (!(currentBacodes?.Any() ?? false) && (existBarcodeList?.Any() ?? false))
			{
				string text = "حذف بارکد(های) کالا".GetTranslate() + "\r\n";
				foreach (BarcodeBusiness item in existBarcodeList)
				{
					text = text + item.BarCode + " \r\n";
				}
				res.AddError(text, "", "ComapreBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 332);
			}
			if ((currentBacodes?.Any() ?? false) && !(existBarcodeList?.Any() ?? false))
			{
				string text2 = "افزودن بارکد به کالا".GetTranslate() + "\r\n";
				foreach (BarcodeBusiness currentBacode in currentBacodes)
				{
					text2 = text2 + currentBacode.BarCode + " \r\n";
				}
				res.AddError(text2, "", "ComapreBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 341);
			}
			if ((currentBacodes?.Any() ?? false) && (existBarcodeList?.Any() ?? false))
			{
				IEnumerable<BarcodeBusiness> enumerable = currentBacodes.Where((BarcodeBusiness q) => !existBarcodeList.Select((BarcodeBusiness w) => w.BarCode).Contains(q.BarCode));
				IEnumerable<BarcodeBusiness> enumerable2 = existBarcodeList.Where((BarcodeBusiness q) => !currentBacodes.Select((BarcodeBusiness w) => w.BarCode).Contains(q.BarCode));
				bool flag = false;
				string text3 = "تغییر بارکد (های) کالا".GetTranslate() + "\r\n";
				if (enumerable != null && enumerable.Any())
				{
					flag = true;
					text3 = text3 + "بارکد (های) جدید".GetTranslate() + ":\r\n";
					foreach (BarcodeBusiness item2 in enumerable)
					{
						text3 = text3 + item2.BarCode + " \r\n";
					}
				}
				if (enumerable2 != null && enumerable2.Any())
				{
					flag = true;
					text3 = text3 + "بارکد (های) سایق".GetTranslate() + ": \r\n";
					foreach (BarcodeBusiness item3 in enumerable2)
					{
						text3 = text3 + item3.BarCode + " \r\n";
					}
				}
				if (flag)
				{
					res.AddError(text3, "", "ComapreBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 368);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComapreBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 373);
			res.AddReturnedValue(ex, "", "ComapreBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 374);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ComapreGroupsAsync(string contextGuid, WebProduct current, ProductBusiness old, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(current?.GroupList?.Any()).GetValueOrDefault() && (old?.GroupList?.Any()).GetValueOrDefault())
			{
				string msg2 = "حذف گروه(های) کالا".GetTranslate() + "\r\n";
				foreach (ProductGroupRelationBusiness group in old.GroupList)
				{
					if (group != null)
					{
						msg2 = msg2 + (await PrdGroupBusiness.GetAsync(contextGuid, group.GroupGuid, token, tranName)).Name + " \r\n";
					}
				}
				res.AddError(msg2, "", "ComapreGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 393);
			}
			if ((current?.GroupList?.Any()).GetValueOrDefault() && !(old?.GroupList?.Any()).GetValueOrDefault())
			{
				string msg2 = "افزودن گروه به کالا ".GetTranslate() + "\r\n";
				foreach (WebProductGroupRelation group2 in current.GroupList)
				{
					if (group2 != null)
					{
						msg2 = msg2 + (await PrdGroupBusiness.GetAsync(contextGuid, group2.GroupGuid, token, tranName)).Name + " \r\n";
					}
				}
				res.AddError(msg2, "", "ComapreGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 406);
			}
			if ((current?.GroupList?.Any()).GetValueOrDefault() && (old?.GroupList?.Any()).GetValueOrDefault())
			{
				IEnumerable<WebProductGroupRelation> enumerable = current.GroupList.Where((WebProductGroupRelation q) => !old.GroupList.Select((ProductGroupRelationBusiness w) => w.GroupGuid).Contains(q.GroupGuid));
				IEnumerable<ProductGroupRelationBusiness> removedGroup = old.GroupList.Where((ProductGroupRelationBusiness q) => !current.GroupList.Select((WebProductGroupRelation w) => w.GroupGuid).Contains(q.GroupGuid));
				bool hasChange = false;
				string msg2 = "تغییر گروه (های) کالا ".GetTranslate() + "\r\n";
				if (enumerable != null && enumerable.Any())
				{
					hasChange = true;
					msg2 = msg2 + "گروه (های) جدید".GetTranslate() + ":\r\n";
					foreach (WebProductGroupRelation item in enumerable)
					{
						if (item != null)
						{
							msg2 = msg2 + (await PrdGroupBusiness.GetAsync(contextGuid, item.GroupGuid, token, tranName)).Name + " \r\n";
						}
					}
				}
				if (removedGroup?.Any() ?? false)
				{
					hasChange = true;
					msg2 = msg2 + "گروه (های) سایق".GetTranslate() + ":\r\n";
					foreach (ProductGroupRelationBusiness item2 in removedGroup)
					{
						if (item2 != null)
						{
							msg2 = msg2 + (await PrdGroupBusiness.GetAsync(contextGuid, item2.GroupGuid, token, tranName)).Name + " \r\n";
						}
					}
				}
				if (hasChange)
				{
					res.AddError(msg2, "", "ComapreGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 441);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComapreGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 446);
			res.AddReturnedValue(ex, "", "ComapreGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 447);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> ComaprePricesAsync(string contextGuid, EnSystem system, WebProduct current, ProductBusiness old, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(current?.Prices?.Any()).GetValueOrDefault() && (old?.PriceList?.Any()).GetValueOrDefault())
			{
				string msg = "حذف قیمت(های) کالا".GetTranslate() + " \r\n";
				foreach (ProductPriceRelationBusiness item4 in old.PriceList)
				{
					if (item4 != null)
					{
						ProductPriceLevelBusiness productPriceLevelBusiness = await ProductPriceLevelBusiness.GetAsync(contextGuid, item4.PriceLevelGuid, system, token, tranName);
						if (productPriceLevelBusiness != null)
						{
							msg += $"{productPriceLevelBusiness.Name}:{item4.Price:N0} \r\n";
						}
					}
				}
				res.AddError(msg, "", "ComaprePricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 467);
			}
			if ((current?.Prices?.Any()).GetValueOrDefault() && !(old?.PriceList?.Any()).GetValueOrDefault())
			{
				string msg = "افزودن قیمت به کالا".GetTranslate() + "\r\n";
				foreach (WebProductPriceRelation item6 in current.Prices)
				{
					if (item6 != null)
					{
						msg += $"{(await ProductPriceLevelBusiness.GetAsync(contextGuid, item6.PriceLevelGuid, system, token, tranName)).Name}:{item6.Price:N0} \r\n";
					}
				}
				res.AddError(msg, "", "ComaprePricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 480);
			}
			if ((current?.Prices?.Any()).GetValueOrDefault() && (old?.PriceList?.Any()).GetValueOrDefault())
			{
				IEnumerable<WebProductPriceRelation> enumerable = current.Prices.Where((WebProductPriceRelation q) => !old.PriceList.Select((ProductPriceRelationBusiness w) => w.PriceLevelGuid).Contains(q.PriceLevelGuid));
				IEnumerable<ProductPriceRelationBusiness> removedPrices = old.PriceList.Where((ProductPriceRelationBusiness q) => !current.Prices.Select((WebProductPriceRelation w) => w.PriceLevelGuid).Contains(q.PriceLevelGuid));
				new List<WebProductPriceLevel>();
				bool hasChange = false;
				string msg = "تغییر قیمت (های) کالا".GetTranslate() + "\r\n";
				if (enumerable != null && enumerable.Any())
				{
					hasChange = true;
					msg = msg + "قیمت (های) جدید".GetTranslate() + ":\r\n";
					foreach (WebProductPriceRelation item6 in enumerable)
					{
						if (item6 != null)
						{
							msg += $"{(await ProductPriceLevelBusiness.GetAsync(contextGuid, item6.PriceLevelGuid, system, token, tranName)).Name}:{item6.Price:N0} \r\n";
						}
					}
				}
				if (removedPrices?.Any() ?? false)
				{
					hasChange = true;
					msg = msg + "قیمت (های) سابق".GetTranslate() + ":\r\n";
					foreach (ProductPriceRelationBusiness item4 in removedPrices)
					{
						if (item4 != null)
						{
							msg += $"{(await ProductPriceLevelBusiness.GetAsync(contextGuid, item4.PriceLevelGuid, system, token, tranName)).Name}:{item4.Price:N0} \r\n";
						}
					}
				}
				foreach (WebProductPriceRelation item2 in current.Prices)
				{
					if (!((item2?.PriceLevelGuid ?? Guid.Empty) == Guid.Empty))
					{
						ProductPriceRelationBusiness item4 = old.PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == item2.PriceLevelGuid);
						if (item4 != null && !(item4.Price == item2.Price))
						{
							msg += $"تغییر قیمت {(await ProductPriceLevelBusiness.GetAsync(contextGuid, item2.PriceLevelGuid, system, token, tranName)).Name} از {item4.Price:N0} به {item2.Price:N0}";
							hasChange = true;
						}
					}
				}
				if (hasChange)
				{
					res.AddError(msg, "", "ComaprePricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 526);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComaprePricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 531);
			res.AddReturnedValue(ex, "", "ComaprePricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 532);
		}
		return res;
	}

	public static async Task<decimal> GetSellAsync(this IProductMicro prd, string contextGuid, EnSystem system, OrderBusiness order, Guid priceLevelGuid, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 10;
		try
		{
			if (priceLevelGuid == Guid.Empty)
			{
				return default(decimal);
			}
			ENSource source = ClsCache.GetCache(contextGuid).Source;
			if (prd is ProductMicroBusiness productMicroBusiness)
			{
				productMicroBusiness.IsLastPriceForCustomer = false;
			}
			else
			{
				((ProductBusiness)prd).IsLastPriceForCustomer = false;
			}
			if (priceLevelGuid == ParentDefaults.PriceLevel.LastSellToAll)
			{
				if (source == ENSource.Accounting)
				{
					return (await KardexBusiness.GetLastAsync(contextGuid, null, prd.Guid, Document.OrderSell, token))?.UnitPrice ?? 0m;
				}
				return await ProductPriceRelationBusiness.GetFirstSellDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
			}
			if (priceLevelGuid == ParentDefaults.PriceLevel.LastSellToTafsil)
			{
				if (source == ENSource.Accounting)
				{
					return (await KardexBusiness.GetLastAsync(contextGuid, order?.GetTafsilGuid(), prd.Guid, Document.OrderSell, token))?.UnitPrice ?? 0m;
				}
				LastPriceBusiness lastPriceBusiness = await LastPriceBusiness.GetAsync(contextGuid, prd.Guid, order?.GetTafsilGuid() ?? Guid.Empty, token);
				if (lastPriceBusiness != null && !lastPriceBusiness.Invalidated && lastPriceBusiness.Price > 0m)
				{
					if (prd is ProductMicroBusiness productMicroBusiness2)
					{
						productMicroBusiness2.IsLastPriceForCustomer = true;
					}
					else
					{
						((ProductBusiness)prd).IsLastPriceForCustomer = true;
					}
					return lastPriceBusiness.Price;
				}
				return await ProductPriceRelationBusiness.GetFirstSellDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
			}
			if (priceLevelGuid == ParentDefaults.PriceLevel.LastBuyFromAll)
			{
				decimal lastBuy2 = await ProductPriceRelationBusiness.GetLastBuyDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
				return (await KardexBusiness.GetLastAsync(contextGuid, null, prd.Guid, Document.OrderBuy, token))?.UnitPrice ?? lastBuy2;
			}
			if (priceLevelGuid == ParentDefaults.PriceLevel.LastBuyFromTafsil)
			{
				decimal lastBuy2 = await ProductPriceRelationBusiness.GetLastBuyDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
				if (source != ENSource.Accounting)
				{
					return lastBuy2;
				}
				return (await KardexBusiness.GetLastAsync(contextGuid, order?.GetTafsilGuid(), prd.Guid, Document.OrderBuy, token))?.UnitPrice ?? lastBuy2;
			}
			if (priceLevelGuid == ParentDefaults.PriceLevel.BeginPeriod || priceLevelGuid == ParentDefaults.PriceLevel.AverageBuy)
			{
				return await ProductPriceRelationBusiness.GetFirstBuyDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
			}
			ProductPriceRelationFilter filter = new ProductPriceRelationFilter
			{
				PriceLevelGuid = priceLevelGuid,
				ProductGuid = prd.Guid,
				Type = EnPriceLevelType.Sell
			};
			return ((await ProductPriceRelationBusiness.GetAllAsync(contextGuid, system, filter, token, tranName))?.SingleOrDefault()?.Price).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 616);
			throw;
		}
	}

	public static async Task<string> GetGroupNameAsync(this IProductCompact prd, string contextGuid, IEnumerable<ProductGroupRelationBusiness> relationList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(relationList?.Any() ?? false))
			{
				return "";
			}
			relationList = relationList?.OrderBy((ProductGroupRelationBusiness q) => q.Ordinal)?.ToList();
			string groupName = "";
			foreach (ProductGroupRelationBusiness relation in relationList)
			{
				PrdGroupBusiness prdGroupBusiness = await PrdGroupBusiness.GetAsync(contextGuid, relation.GroupGuid, token, tranName);
				if (prdGroupBusiness != null)
				{
					groupName = groupName + " " + prdGroupBusiness.Name + " -";
				}
			}
			if (groupName.EndsWith("-"))
			{
				groupName = groupName.Substring(0, checked(groupName.Length - 1));
			}
			return groupName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGroupNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 637);
			throw;
		}
	}

	public static async Task<List<PrdGroupBusiness>> GetGroupsAsync(this IProductCompact prd, string contextGuid, IEnumerable<ProductGroupRelationBusiness> relationList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(relationList?.Any() ?? false))
			{
				return null;
			}
			relationList = relationList?.OrderBy((ProductGroupRelationBusiness q) => q.Ordinal)?.ToList();
			List<PrdGroupBusiness> groupList = new List<PrdGroupBusiness>();
			foreach (ProductGroupRelationBusiness relation in relationList)
			{
				groupList.Add(await PrdGroupBusiness.GetAsync(contextGuid, relation.GroupGuid, token, tranName));
			}
			return groupList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGroupsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 657);
			throw;
		}
	}

	public static async Task<IEnumerable<ProductPriceRelationModel>> GetAllPricesAsync(this IProductCompact prd, string contextGuid, EnSystem system, EnPriceLevelType? type, IEnumerable<ProductPriceRelationBusiness> existPriceList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (prd?.Guid == Guid.Empty)
			{
				return null;
			}
			ProductPriceRelationFilter filter = new ProductPriceRelationFilter
			{
				Type = type,
				ProductGuid = prd.Guid
			};
			List<ProductPriceRelationModel> list = (await ProductPriceRelationBusiness.GetAllAsync(contextGuid, system, filter, token, tranName))?.ToList();
			if ((existPriceList?.Any() ?? false) && list != null && list.Any())
			{
				foreach (ProductPriceRelationModel item in list)
				{
					if (item == null)
					{
						continue;
					}
					ProductPriceRelationBusiness productPriceRelationBusiness = existPriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.Guid == item.Guid);
					if (productPriceRelationBusiness == null)
					{
						productPriceRelationBusiness = existPriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == item.PriceLevelGuid);
						if (productPriceRelationBusiness == null)
						{
							continue;
						}
					}
					item.Guid = productPriceRelationBusiness.Guid;
					item.Price = productPriceRelationBusiness.Price;
				}
			}
			return list?.OrderBy((ProductPriceRelationModel q) => q.Ordinal);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\Product.cs", 693);
			throw;
		}
	}
}
