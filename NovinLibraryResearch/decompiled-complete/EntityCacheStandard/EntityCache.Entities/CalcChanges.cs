using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.DepotReceipt;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Entities;

public class CalcChanges
{
	private static void ComputeHesabChange(Sanad_DetBusiness item, List<HesabChanges> retList, bool isNew, bool isDeleteAction)
	{
		using (new JaegerMonitor("ComputeHesabChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				HesabChanges hesabChanges = retList?.FirstOrDefault(delegate(HesabChanges e)
				{
					if (e.MoeinGuid == item.MoeinGuid)
					{
						Guid tafsilGuid = e.TafsilGuid;
						Guid? tafsilGuid2 = item.TafsilGuid;
						return tafsilGuid == tafsilGuid2;
					}
					return false;
				});
				if (hesabChanges == null)
				{
					hesabChanges = new HesabChanges
					{
						MoeinGuid = item.MoeinGuid,
						TafsilGuid = (item.TafsilGuid ?? Guid.Empty),
						Account = 0m,
						Level = EnMoeinTafsilRelationLevel.One
					};
					retList.Add(hesabChanges);
				}
				if (isNew && !isDeleteAction)
				{
					hesabChanges.Account += item.Credit - item.Debit;
				}
				else
				{
					hesabChanges.Account -= item.Credit - item.Debit;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeHesabChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 38);
			}
		}
	}

	public static async Task ComputeOrderChangeAsync(ComputeOrderChangesConfig config, string tranName)
	{
		using (new JaegerMonitor("ComputeOrderChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				Guid depGuid = await config.OrderDetial.GetDepotGuidAsync();
				ProductsChanges productsChanges = config.PrdChanges.SingleOrDefault((ProductsChanges e) => e.PrdGuid == config.OrderDetial.ProductGuid && e.depGuid == depGuid);
				if (productsChanges == null)
				{
					productsChanges = new ProductsChanges(config.ContextGuid)
					{
						PrdGuid = config.OrderDetial.ProductGuid,
						depGuid = depGuid,
						qt = 0m
					};
					config.PrdChanges.Add(productsChanges);
				}
				switch (config.FactorType)
				{
				case Document.OrderSell:
				case Document.OrderBackBuy:
				case Document.Tolid:
				case Document.Zayeat:
				case Document.AmaniKhoroj:
				case Document.Tolid_Mahsul:
				case Document.Tolid_Mavad:
					if (config.IsNew)
					{
						if (!config.IsDeleteAction)
						{
							productsChanges.qt -= config.OrderDetial.Quantity;
						}
					}
					else
					{
						productsChanges.qt += config.OrderDetial.Quantity;
					}
					break;
				case Document.OrderBuy:
				case Document.OrderBackSell:
				case Document.AmaniVorod:
					if (config.IsNew)
					{
						if (!config.IsDeleteAction)
						{
							productsChanges.qt += config.OrderDetial.Quantity;
						}
					}
					else
					{
						productsChanges.qt -= config.OrderDetial.Quantity;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeOrderChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 95);
			}
		}
	}

	public static void ComputeAnbarGardaniChange(string contextGuid, IPrdAnbarGardaniDet orderDet, List<ProductsChanges> prdChanges, bool isNew, bool isDeleteAction)
	{
		using (new JaegerMonitor("ComputeAnbarGardaniChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				ProductsChanges productsChanges = prdChanges.Where((ProductsChanges e) => e.PrdGuid == orderDet.PrdGuid && e.depGuid == orderDet.DptGuid).FirstOrDefault();
				if (productsChanges == null)
				{
					productsChanges = new ProductsChanges(contextGuid)
					{
						PrdGuid = orderDet.PrdGuid,
						depGuid = orderDet.DptGuid,
						qt = 0m
					};
					prdChanges.Add(productsChanges);
				}
				if (isNew)
				{
					if (!isDeleteAction)
					{
						productsChanges.qt += orderDet.QtEzafe;
						productsChanges.qt -= orderDet.QtKasr;
					}
				}
				else
				{
					productsChanges.qt -= orderDet.QtEzafe;
					productsChanges.qt += orderDet.QtKasr;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeAnbarGardaniChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 128);
			}
		}
	}

	public static void ComputeDepotReceiptChange(string contextGuid, DepotReceiptDetailBusiness orderDet, List<ProductsChanges> prdChanges, bool isNew, bool isDeleteAction)
	{
		using (new JaegerMonitor("ComputeDepotReceiptChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				ProductsChanges productsChanges = prdChanges.Where((ProductsChanges e) => e.PrdGuid == orderDet.ProductGuid && e.depGuid == orderDet.DepotGuid).FirstOrDefault();
				if (productsChanges == null)
				{
					productsChanges = new ProductsChanges(contextGuid)
					{
						PrdGuid = orderDet.ProductGuid,
						depGuid = orderDet.DepotGuid,
						qt = 0m
					};
					prdChanges.Add(productsChanges);
				}
				if (isNew)
				{
					if (!isDeleteAction && orderDet.MasterItem.Type == EnDepotReceiptType.Receipt)
					{
						productsChanges.qt += orderDet.Quantity;
					}
					if (!isDeleteAction && orderDet.MasterItem.Type == EnDepotReceiptType.Havale)
					{
						productsChanges.qt -= orderDet.Quantity;
					}
				}
				else
				{
					if (orderDet.MasterItem.Type == EnDepotReceiptType.Receipt)
					{
						productsChanges.qt -= orderDet.Quantity;
					}
					if (orderDet.MasterItem.Type == EnDepotReceiptType.Havale)
					{
						productsChanges.qt += orderDet.Quantity;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeDepotReceiptChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 162);
			}
		}
	}

	public static void ComputeDepEnteghalChange(string contextGuid, DepEnteghalDetBusiness orderDet, List<ProductsChanges> prdChanges, bool isNew, bool isDeleteAction)
	{
		using (new JaegerMonitor("ComputeDepEnteghalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				ProductsChanges productsChanges = prdChanges.Where((ProductsChanges e) => e.PrdGuid == orderDet.ProductGuid && e.depGuid == orderDet.DptGuidSrc).FirstOrDefault();
				ProductsChanges productsChanges2 = prdChanges.Where((ProductsChanges e) => e.PrdGuid == orderDet.ProductGuid && e.depGuid == orderDet.DptGuidDst).FirstOrDefault();
				if (productsChanges == null)
				{
					productsChanges = new ProductsChanges(contextGuid)
					{
						PrdGuid = orderDet.ProductGuid,
						depGuid = orderDet.DptGuidSrc,
						qt = 0m
					};
					prdChanges.Add(productsChanges);
				}
				if (productsChanges2 == null)
				{
					productsChanges2 = new ProductsChanges(contextGuid)
					{
						PrdGuid = orderDet.ProductGuid,
						depGuid = orderDet.DptGuidDst,
						qt = 0m
					};
					prdChanges.Add(productsChanges2);
				}
				if (isNew)
				{
					if (!isDeleteAction)
					{
						productsChanges.qt -= orderDet.Quantity;
						productsChanges2.qt += orderDet.Quantity;
					}
				}
				else
				{
					productsChanges.qt += orderDet.Quantity;
					productsChanges2.qt -= orderDet.Quantity;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeDepEnteghalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 206);
			}
		}
	}

	public static async Task ComputeTolidChangeAsync(string contextGuid, OrderDetailBusiness orderDet, List<ProductsChanges> prdChanges, bool isNew)
	{
		using (new JaegerMonitor("ComputeTolidChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				ClsCache.GetCache(contextGuid);
				Guid depGuid = await orderDet.GetDepotGuidAsync();
				ProductsChanges productsChanges = prdChanges.Where((ProductsChanges e) => e.PrdGuid == orderDet.ProductGuid && e.depGuid == depGuid).FirstOrDefault();
				if (productsChanges == null)
				{
					productsChanges = new ProductsChanges(contextGuid)
					{
						PrdGuid = orderDet.ProductGuid,
						depGuid = depGuid,
						qt = 0m
					};
					prdChanges.Add(productsChanges);
				}
				if (isNew)
				{
					productsChanges.qt += Math.Round(orderDet.Quantity, 3);
				}
				else
				{
					productsChanges.qt -= Math.Round(orderDet.Quantity, 3);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeTolidChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 233);
			}
		}
	}

	public static List<HesabChanges> ComputeHesabChange(SanadBusiness sanadOld, SanadBusiness sanad, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("ComputeHesabChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs"))
		{
			try
			{
				List<HesabChanges> list = new List<HesabChanges>();
				if (sanadOld?.ListSanad_Det != null)
				{
					foreach (Sanad_DetBusiness item2 in sanadOld.ListSanad_Det)
					{
						if (autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p != null && p.Item1 == item2.AsType && p.Item2 == item2.AsGuid))
						{
							ComputeHesabChange(item2, list, isNew: false, isDeleteAction);
						}
					}
				}
				if (sanad?.ListSanad_Det != null)
				{
					foreach (Sanad_DetBusiness item in sanad.ListSanad_Det)
					{
						if (!isDeleteAction && autoSanadItemsForSave.Any((Tuple<Document, Guid> p) => p != null && p.Item1 == item.AsType && p.Item2 == item.AsGuid))
						{
							ComputeHesabChange(item, list, isNew: true, isDeleteAction: false);
						}
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComputeHesabChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\calcChanges.cs", 255);
				return null;
			}
		}
	}
}
