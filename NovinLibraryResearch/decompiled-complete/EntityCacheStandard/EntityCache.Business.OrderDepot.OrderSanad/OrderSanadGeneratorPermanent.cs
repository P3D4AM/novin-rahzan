using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.DescriptionParser;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.OrderDepot.OrderSanad;

public class OrderSanadGeneratorPermanent : OrderSanadGeneratorBase
{
	protected override async Task GenerateOrderBackSellAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 9;
		try
		{
			await base.GenerateOrderBackSellAsync(order, sanad, setting, system, token, tranName);
			if (order.ListKala?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListProduct
					group q by q.ProductTafsilGuid;
				foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
				{
					decimal costPrice = default(decimal);
					foreach (OrderDetailBusiness det in item)
					{
						if (det != null)
						{
							costPrice += await det.GetUnitLastCostAsync(token, tranName);
							det.TotalCostPrice = costPrice;
						}
					}
					if (costPrice != 0m)
					{
						decimal price = costPrice;
						Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
						{
							Debit = price,
							Credit = 0m
						};
						Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
						sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
						sanad_DetBusiness.DecimalGuid = Guid.Empty;
						sanad_DetBusiness.Guid = Guid.NewGuid();
						sanad_DetBusiness.Qt = 0m;
						sanad_DetBusiness.MasterGuid = sanad.Guid;
						Sanad_DetBusiness sellInventory = sanad_DetBusiness;
						await sellInventory.SetMoeinGuidAsync(setting.CodeSetting.Permanent.ProductMoeinGuid, token, tranName);
						await sellInventory.SetTafsilGuidAsync(setting.CodeSetting.Permanent.ProductTafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sellInventory, system, token);
						sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
						{
							Debit = 0m,
							Credit = price
						};
						sanad_DetBusiness = sanad_DetBusiness2;
						sanad_DetBusiness.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
						sanad_DetBusiness2.DecimalGuid = Guid.Empty;
						sanad_DetBusiness2.Guid = Guid.NewGuid();
						sanad_DetBusiness2.Qt = 0m;
						sanad_DetBusiness2.MasterGuid = sanad.Guid;
						Sanad_DetBusiness sellCost = sanad_DetBusiness2;
						await sellCost.SetMoeinGuidAsync(setting.CodeSetting.Permanent.CostMoeinGuid, token, tranName);
						await sellCost.SetTafsilGuidAsync(setting.CodeSetting.Permanent.CostTafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sellCost, system, token);
					}
				}
			}
			if (!(order.ListService?.Any() ?? false))
			{
				return;
			}
			foreach (IGrouping<Guid?, OrderDetailBusiness> item2 in from q in order.ListService
				group q by q.ProductTafsilGuid)
			{
				foreach (OrderDetailBusiness item3 in item2 ?? throw new NullReferenceException())
				{
					if (item3 != null)
					{
						item3.TotalCostPrice = 0m;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderBackSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorPermanent.cs", 90);
			throw;
		}
	}

	protected override async Task GenerateOrderSellAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 9;
		try
		{
			await base.GenerateOrderSellAsync(order, sanad, setting, system, token, tranName);
			if (order.ListProduct?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListProduct
					group q by q.ProductTafsilGuid;
				foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
				{
					if (item == null)
					{
						throw new NullReferenceException();
					}
					decimal costPrice = default(decimal);
					foreach (OrderDetailBusiness det in item)
					{
						if (det != null)
						{
							costPrice += await det.GetUnitLastCostAsync(token, tranName);
							det.TotalCostPrice = costPrice;
						}
					}
					if (costPrice != 0m)
					{
						decimal price = costPrice;
						Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
						{
							Debit = price,
							Credit = 0m
						};
						Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
						sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
						sanad_DetBusiness.DecimalGuid = Guid.Empty;
						sanad_DetBusiness.Guid = Guid.NewGuid();
						sanad_DetBusiness.Qt = 0m;
						sanad_DetBusiness.MasterGuid = sanad.Guid;
						Sanad_DetBusiness sellCost = sanad_DetBusiness;
						await sellCost.SetMoeinGuidAsync(setting.CodeSetting.Permanent.CostMoeinGuid, token, tranName);
						await sellCost.SetTafsilGuidAsync(setting.CodeSetting.Permanent.CostTafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sellCost, system, token);
						sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
						{
							Debit = 0m,
							Credit = price
						};
						sanad_DetBusiness = sanad_DetBusiness2;
						sanad_DetBusiness.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
						sanad_DetBusiness2.DecimalGuid = Guid.Empty;
						sanad_DetBusiness2.Guid = Guid.NewGuid();
						sanad_DetBusiness2.Qt = 0m;
						sanad_DetBusiness2.MasterGuid = sanad.Guid;
						Sanad_DetBusiness sellInventory = sanad_DetBusiness2;
						await sellInventory.SetMoeinGuidAsync(setting.CodeSetting.Permanent.ProductMoeinGuid, token, tranName);
						await sellInventory.SetTafsilGuidAsync(setting.CodeSetting.Permanent.ProductTafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sellInventory, system, token);
					}
				}
			}
			if (!(order.ListService?.Any() ?? false))
			{
				return;
			}
			foreach (IGrouping<Guid?, OrderDetailBusiness> item2 in from q in order.ListService
				group q by q.ProductTafsilGuid)
			{
				foreach (OrderDetailBusiness item3 in item2 ?? throw new NullReferenceException())
				{
					if (item3 != null)
					{
						item3.TotalCostPrice = 0m;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorPermanent.cs", 170);
			throw;
		}
	}

	protected override async Task GetBackSellCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 6;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(order.ContextGuid, token);
			await base.GetBackSellCoverAsync(order, list, system, token, tranName);
			if (!(order.ListKala?.Any() ?? false))
			{
				return;
			}
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				if (item != null)
				{
					decimal lastCost = await item.GetUnitLastCostAsync(token, tranName);
					if (lastCost > 0m)
					{
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
						{
							DebitMoeinGuid = setting.CodeSetting.Permanent.ProductMoeinGuid,
							DebitTafsilGuid = setting.CodeSetting.Permanent.ProductTafsilGuid,
							Date = order.GetDate(),
							AsGuid = order.Guid,
							AsType = DocumentDet.SellPrd,
							AsTypeDocument = order.FactorType,
							Debit = lastCost,
							Credit = 0m,
							CashierGuid = order.UserAccGuid,
							SanadNo = order.SanadNo,
							SanadSt = order.SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = setting.CodeSetting.Permanent.CostMoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = setting.CodeSetting.Permanent.CostTafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = order.Description;
						viewSanadDetailCoverBusiness.RDesc = item.Description;
						viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
						viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
						viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
						viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = lastCost;
						viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
						viewSanadDetailCoverBusiness.Qt = item.Quantity;
						viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
						viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
						viewSanadDetailCoverBusiness.RDiscount = item.Discount;
						viewSanadDetailCoverBusiness.Serial = item.Serial;
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
						list2.Add(viewSanadDetailCoverBusiness);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
						{
							DebitMoeinGuid = setting.CodeSetting.Permanent.ProductMoeinGuid,
							DebitTafsilGuid = setting.CodeSetting.Permanent.ProductTafsilGuid,
							Date = order.GetDate(),
							AsGuid = order.Guid,
							AsType = DocumentDet.SellPrd,
							AsTypeDocument = order.FactorType,
							Debit = 0m,
							Credit = lastCost,
							CashierGuid = order.UserAccGuid,
							SanadNo = order.SanadNo,
							SanadSt = order.SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = setting.CodeSetting.Permanent.CostMoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = setting.CodeSetting.Permanent.CostTafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
						viewSanadDetailCoverBusiness2.RDesc = item.Description;
						viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
						viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
						viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
						viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
						viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
						viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
						viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness2.Price = lastCost;
						viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
						viewSanadDetailCoverBusiness2.Qt = item.Quantity;
						viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt.GetValueOrDefault();
						viewSanadDetailCoverBusiness2.RBazaryab = item.TotalVisitorPrice;
						viewSanadDetailCoverBusiness2.RDiscount = item.Discount;
						viewSanadDetailCoverBusiness2.Serial = item.Serial;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = item.VGuid;
						list2.Add(viewSanadDetailCoverBusiness2);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorPermanent.cs", 267);
			throw;
		}
	}

	protected override async Task GetSellCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 6;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(order.ContextGuid, token);
			await base.GetSellCoverAsync(order, list, system, token, tranName);
			if (!(order.ListKala?.Any() ?? false))
			{
				return;
			}
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				if (item != null)
				{
					decimal lastCost = await item.GetUnitLastCostAsync(token, tranName);
					if (lastCost > 0m)
					{
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
						{
							DebitMoeinGuid = setting.CodeSetting.Permanent.CostMoeinGuid,
							DebitTafsilGuid = setting.CodeSetting.Permanent.CostTafsilGuid,
							Date = order.GetDate(),
							AsGuid = order.Guid,
							AsType = DocumentDet.SellPrd,
							AsTypeDocument = order.FactorType,
							Debit = lastCost,
							Credit = 0m,
							CashierGuid = order.UserAccGuid,
							SanadNo = order.SanadNo,
							SanadSt = order.SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = setting.CodeSetting.Permanent.ProductMoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = setting.CodeSetting.Permanent.ProductTafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = order.Description;
						viewSanadDetailCoverBusiness.RDesc = item.Description;
						viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
						viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
						viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
						viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = lastCost;
						viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
						viewSanadDetailCoverBusiness.Qt = item.Quantity;
						viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
						viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
						viewSanadDetailCoverBusiness.RDiscount = item.Discount;
						viewSanadDetailCoverBusiness.Serial = item.Serial;
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
						list2.Add(viewSanadDetailCoverBusiness);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
						{
							DebitMoeinGuid = setting.CodeSetting.Permanent.CostMoeinGuid,
							DebitTafsilGuid = setting.CodeSetting.Permanent.CostTafsilGuid,
							Date = order.GetDate(),
							AsGuid = order.Guid,
							AsType = DocumentDet.SellPrd,
							AsTypeDocument = order.FactorType,
							Debit = 0m,
							Credit = lastCost,
							CashierGuid = order.UserAccGuid,
							SanadNo = order.SanadNo,
							SanadSt = order.SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = setting.CodeSetting.Permanent.ProductMoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = setting.CodeSetting.Permanent.ProductTafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
						viewSanadDetailCoverBusiness2.RDesc = item.Description;
						viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
						viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
						viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
						viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
						viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
						viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
						viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness2.Price = lastCost;
						viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
						viewSanadDetailCoverBusiness2.Qt = item.Quantity;
						viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt.GetValueOrDefault();
						viewSanadDetailCoverBusiness2.RBazaryab = item.TotalVisitorPrice;
						viewSanadDetailCoverBusiness2.RDiscount = item.Discount;
						viewSanadDetailCoverBusiness2.Serial = item.Serial;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = item.VGuid;
						list2.Add(viewSanadDetailCoverBusiness2);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorPermanent.cs", 365);
			throw;
		}
	}
}
