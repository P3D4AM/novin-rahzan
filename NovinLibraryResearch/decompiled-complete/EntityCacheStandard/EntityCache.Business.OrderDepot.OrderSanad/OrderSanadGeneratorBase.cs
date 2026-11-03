using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.DescriptionParser;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.OrderDepot.OrderSanad;

public abstract class OrderSanadGeneratorBase : IOrderSanadGenerator
{
	public async Task<SanadBusiness> GenerateSanadAsync(OrderBusiness order, OrderBusiness oldOrder, EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs"))
		{
			SanadBusiness sanad;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(order.ContextGuid, token);
				sanad = await SanadBusiness.GetAsync(order.ContextGuid, order.SanadNo, token, tranName);
				if (order.ChangeStatus == EntityChangeSt.New)
				{
					SanadBusiness sanadBusiness = new SanadBusiness(order.ContextGuid, order.SanadNo);
					SanadBusiness sanadBusiness2 = sanadBusiness;
					sanadBusiness2.Description = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
					sanadBusiness.UserGuid = order.UserAccGuid;
					sanadBusiness.DateM = order.GetDate().AddMilliseconds(500.0);
					sanadBusiness.Guid = sanad?.Guid ?? Guid.NewGuid();
					sanadBusiness.SanadNo = order.SanadNo;
					sanadBusiness.SanadType = SanadType.Automatic;
					sanad = sanadBusiness;
				}
				else
				{
					if (oldOrder == null)
					{
						oldOrder = (OrderBusiness)(await order.GetOldItemAsync(system, token, tranName));
					}
					SanadBusiness sanadBusiness2 = new SanadBusiness(order.ContextGuid, oldOrder?.SanadNo ?? 0);
					SanadBusiness sanadBusiness = sanadBusiness2;
					sanadBusiness.Description = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
					sanadBusiness2.UserGuid = order.UserAccGuid;
					sanadBusiness2.DateM = order.GetDate().AddMilliseconds(500.0);
					sanadBusiness2.Guid = sanad?.Guid ?? Guid.NewGuid();
					sanadBusiness2.SanadNo = order.SanadNo;
					sanadBusiness2.SanadType = SanadType.Automatic;
					sanad = sanadBusiness2;
				}
				if ((order?.ListKala?.Any()).GetValueOrDefault())
				{
					foreach (OrderDetailBusiness item in order?.ListKala)
					{
						if ((item?.ProductTafsilGuid ?? Guid.Empty) == Guid.Empty)
						{
							await item.GetProductTafsilGuidAsync(token);
						}
					}
				}
				switch (order.FactorType)
				{
				case Document.OrderSell:
					await GenerateOrderSellAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.OrderBuy:
					await GenerateOrderBuyAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.OrderBackBuy:
					await GenerateOrderBackBuyAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.OrderBackSell:
					await GenerateOrderBackSellAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.Zayeat:
					await GenerateOrderZayeatAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.AmaniVorod:
					await GenerateOrderAmaniVorodAsync(order, sanad, setting, system, token, tranName);
					break;
				case Document.AmaniKhoroj:
					await GenerateOrderAmaniKhorojAsync(order, sanad, setting, system, token, tranName);
					break;
				default:
					throw new NotImplementedException($"Factor type({order.FactorType})");
				case Document.PishFactor:
				case Document.Tolid:
				case Document.Tolid_Mahsul:
				case Document.Tolid_Mavad:
				case Document.OrderPreBuy:
					break;
				}
				if (order.ListSanad_Det?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item2 in order.ListSanad_Det)
					{
						item2.AsType = order.FactorType;
						item2.AsGuid = order.Guid;
					}
					await sanad.AddRangeToListSanadAsync(order.ConnectedSanad.ListSanad_Det.ToList(), system, appendCurrentItems: true, token);
				}
				if ((sanad?.ListSanad_Det?.Any()).GetValueOrDefault())
				{
					IOrderedEnumerable<Sanad_DetBusiness> orderedEnumerable = sanad.ListSanad_Det.OrderBy((Sanad_DetBusiness q) => q.Credit);
					int num = 0;
					foreach (Sanad_DetBusiness item3 in orderedEnumerable)
					{
						if (!((item3?.Guid ?? Guid.Empty) == Guid.Empty))
						{
							item3.SetOrdinalChange(system, checked(num++), throwOnOrdinalChanged: false, token);
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "OrderJsonData:" + Json.ToStringJson(this, indented: true), "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 96);
				sanad = null;
			}
			return sanad;
		}
	}

	protected async Task GenerateOrderZayeatAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
			{
				Debit = order.SumKalaKhadamat,
				Credit = 0m
			};
			Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
			sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
			sanad_DetBusiness.DecimalGuid = Guid.Empty;
			sanad_DetBusiness.Guid = Guid.NewGuid();
			sanad_DetBusiness.Qt = 0m;
			sanad_DetBusiness.MasterGuid = sanad.Guid;
			Sanad_DetBusiness zayeatDebit = sanad_DetBusiness;
			await zayeatDebit.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
			await zayeatDebit.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(zayeatDebit, system, token);
			if (!(order.ListKala?.Any() ?? false))
			{
				return;
			}
			IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
				group q by q.ProductTafsilGuid;
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
			{
				decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum);
				if (!(num <= 0m))
				{
					Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
					{
						Debit = 0m,
						Credit = num
					};
					sanad_DetBusiness2 = sanad_DetBusiness3;
					sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
					sanad_DetBusiness3.DecimalGuid = Guid.Empty;
					sanad_DetBusiness3.Guid = Guid.NewGuid();
					sanad_DetBusiness3.Qt = 0m;
					sanad_DetBusiness3.MasterGuid = sanad.Guid;
					sanad_DetBusiness = sanad_DetBusiness3;
					await sanad_DetBusiness.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
					await sanad_DetBusiness.SetTafsilGuidAsync(item.Key, tranName, token);
					await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderZayeatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 147);
			throw;
		}
	}

	protected async Task GenerateOrderAmaniVorodAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			Sanad_DetBusiness sanad_DetBusiness;
			Sanad_DetBusiness amaniVorodDebit2;
			if (order.ListKala?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
					{
						decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum);
						if (!(num <= 0m))
						{
							sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Debit = num,
								Credit = 0m
							};
							Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
							sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness.DecimalGuid = Guid.Empty;
							sanad_DetBusiness.Guid = Guid.NewGuid();
							sanad_DetBusiness.Qt = 0m;
							sanad_DetBusiness.MasterGuid = sanad.Guid;
							amaniVorodDebit2 = sanad_DetBusiness;
							await amaniVorodDebit2.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
							await amaniVorodDebit2.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(amaniVorodDebit2, system, token);
						}
					}
				}
			}
			sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
			{
				Debit = 0m,
				Credit = order.SumKalaKhadamat
			};
			amaniVorodDebit2 = sanad_DetBusiness;
			amaniVorodDebit2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
			sanad_DetBusiness.DecimalGuid = Guid.Empty;
			sanad_DetBusiness.Guid = Guid.NewGuid();
			sanad_DetBusiness.Qt = 0m;
			sanad_DetBusiness.MasterGuid = sanad.Guid;
			Sanad_DetBusiness amaniVorodCredit = sanad_DetBusiness;
			await amaniVorodCredit.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
			await amaniVorodCredit.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(amaniVorodCredit, system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderAmaniVorodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 198);
			throw;
		}
	}

	protected async Task GenerateOrderAmaniKhorojAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
			{
				Debit = order.SumKalaKhadamat,
				Credit = 0m
			};
			Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
			sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
			sanad_DetBusiness.DecimalGuid = Guid.Empty;
			sanad_DetBusiness.Guid = Guid.NewGuid();
			sanad_DetBusiness.Qt = 0m;
			sanad_DetBusiness.MasterGuid = sanad.Guid;
			Sanad_DetBusiness amaniKhorojDebit = sanad_DetBusiness;
			await amaniKhorojDebit.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
			await amaniKhorojDebit.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(amaniKhorojDebit, system, token);
			if (!(order.ListKala?.Any() ?? false))
			{
				return;
			}
			IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
				group q by q.ProductTafsilGuid;
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
			{
				decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum);
				if (!(num <= 0m))
				{
					Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
					{
						Debit = 0m,
						Credit = num
					};
					sanad_DetBusiness2 = sanad_DetBusiness3;
					sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
					sanad_DetBusiness3.DecimalGuid = Guid.Empty;
					sanad_DetBusiness3.Guid = Guid.NewGuid();
					sanad_DetBusiness3.Qt = 0m;
					sanad_DetBusiness3.MasterGuid = sanad.Guid;
					sanad_DetBusiness = sanad_DetBusiness3;
					await sanad_DetBusiness.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
					await sanad_DetBusiness.SetTafsilGuidAsync(item.Key, tranName, token);
					await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderAmaniKhorojAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 249);
			throw;
		}
	}

	protected virtual async Task GenerateOrderSellAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 43;
		try
		{
			sanad.SanadSt = SanadSt.Movaghat;
			if (order.TotalSum != 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Debit = order.TotalSum,
					Credit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
				await debitSell3.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
				await debitSell3.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.ListProduct?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListProduct
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
					{
						decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum - q.TotalVisitorPrice + q.TaxImpactonCost);
						if (!(num <= 0m))
						{
							Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Debit = 0m,
								Credit = num
							};
							Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
							sanad_DetBusiness.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness2.DecimalGuid = Guid.Empty;
							sanad_DetBusiness2.Guid = Guid.NewGuid();
							sanad_DetBusiness2.Qt = 0m;
							sanad_DetBusiness2.MasterGuid = sanad.Guid;
							Sanad_DetBusiness debitSell3 = sanad_DetBusiness2;
							await debitSell3.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
							await debitSell3.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(debitSell3, system, token);
						}
					}
				}
			}
			if (order.ListService?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable2 = from q in order.ListService
					group q by q.ProductTafsilGuid;
				if (enumerable2 != null && enumerable2.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable2)
					{
						decimal num2 = item.Sum((OrderDetailBusiness q) => q.RowSum - q.TotalVisitorPrice + q.TaxImpactonCost);
						if (!(num2 <= 0m))
						{
							Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Debit = 0m,
								Credit = num2
							};
							Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
							sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness.DecimalGuid = Guid.Empty;
							sanad_DetBusiness.Guid = Guid.NewGuid();
							sanad_DetBusiness.Qt = 0m;
							sanad_DetBusiness.MasterGuid = sanad.Guid;
							Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
							await debitSell3.SetMoeinGuidAsync(order.ServiceMoeinGuid.Value, token, tranName);
							await debitSell3.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(debitSell3, system, token);
						}
					}
				}
			}
			if (order.Tax > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Tax,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderTaxDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness2;
				await debitSell3.SetMoeinGuidAsync(order.TaxMoeinGuid.Value, token, tranName);
				await debitSell3.SetTafsilGuidAsync(order.TaxTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.Avarez > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Avarez,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderAvarezDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
				if ((order.AvarezMoeinGuid ?? Guid.Empty) == Guid.Empty && system != EnSystem.UserInterface)
				{
					order.AvarezMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29843;
				}
				await debitSell3.SetMoeinGuidAsync(order.AvarezMoeinGuid.Value, token, tranName);
				await debitSell3.SetTafsilGuidAsync(order.AvarezTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.B1 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.B1,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderVisitorDescAsync(system, 1, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness2;
				if ((order.B1MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitSell3.SetMoeinGuidAsync(order.B1MoeinGuid.Value, token, tranName);
				}
				await debitSell3.SetTafsilGuidAsync(order.B1TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.B2 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.B2,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderVisitorDescAsync(system, 2, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
				if ((order.B2MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitSell3.SetMoeinGuidAsync(order.B2MoeinGuid.Value, token, tranName);
				}
				await debitSell3.SetTafsilGuidAsync(order.B2TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.B3 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.B3,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderVisitorDescAsync(system, 3, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness2;
				if ((order.B3MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitSell3.SetMoeinGuidAsync(order.B3MoeinGuid.Value, token, tranName);
				}
				await debitSell3.SetTafsilGuidAsync(order.B3TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.B4 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.B4,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderVisitorDescAsync(system, 4, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
				if ((order.B4MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitSell3.SetMoeinGuidAsync(order.B4MoeinGuid.Value, token, tranName);
				}
				await debitSell3.SetTafsilGuidAsync(order.B4TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.Keraye > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Keraye,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderFareDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness2;
				if ((order.FareMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.FareMoeinGuid = setting?.Sells?.KerayeMoeinGuid ?? Guid.Empty;
				}
				await debitSell3.SetMoeinGuidAsync(order.FareMoeinGuid.Value, token, tranName);
				await debitSell3.SetTafsilGuidAsync(order.FareTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
			if (order.Discount > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Debit = order.Discount,
					Credit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderDiscountDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness debitSell3 = sanad_DetBusiness;
				await debitSell3.SetMoeinGuidAsync(order.DiscountMoeinGuid, token, tranName);
				await debitSell3.SetTafsilGuidAsync(order.DiscountTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitSell3, system, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 479);
			throw;
		}
	}

	protected async Task GenerateOrderBuyAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 39;
		try
		{
			if (order.TotalSum != 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalSum,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness;
				await buyCredit4.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
				await buyCredit4.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.ListKala?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
					{
						if ((item?.Key ?? Guid.Empty) == Guid.Empty)
						{
							throw new NotImplementedException();
						}
						decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum + q.TotalHazinePrice + q.TaxImpactonCost);
						if (!(num <= 0m))
						{
							Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Credit = 0m,
								Debit = num
							};
							Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
							sanad_DetBusiness.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness2.DecimalGuid = Guid.Empty;
							sanad_DetBusiness2.Guid = Guid.NewGuid();
							sanad_DetBusiness2.Qt = 0m;
							sanad_DetBusiness2.MasterGuid = sanad.Guid;
							Sanad_DetBusiness buyCredit4 = sanad_DetBusiness2;
							await buyCredit4.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
							await buyCredit4.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(buyCredit4, system, token);
						}
					}
				}
			}
			if (order.Tax > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.Tax
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderTaxDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness;
				if ((order.TaxMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.TaxMoeinGuid = setting.CodeSetting.BuyCoding.Debit_MoeinGuid_Tax;
				}
				await buyCredit4.SetMoeinGuidAsync(order.TaxMoeinGuid.Value, token, tranName);
				await buyCredit4.SetTafsilGuidAsync(order.TaxTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.Avarez > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.Avarez
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderAvarezDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness2;
				if ((order.AvarezMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.AvarezMoeinGuid = setting.CodeSetting.BuyCoding.Debit_MoeinGuid_Avarez;
				}
				await buyCredit4.SetMoeinGuidAsync(order.AvarezMoeinGuid.Value, token, tranName);
				await buyCredit4.SetTafsilGuidAsync(order.AvarezTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.TotalHazine1 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalHazine1,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderHazineDescAsync(system, 1, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness;
				if ((order.HazineMoeinGuid1 ?? Guid.Empty) != Guid.Empty)
				{
					await buyCredit4.SetMoeinGuidAsync(order.HazineMoeinGuid1.Value, token, tranName);
				}
				await buyCredit4.SetTafsilGuidAsync(order.HazineTafsilGuid1, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.TotalHazine2 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalHazine2,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderHazineDescAsync(system, 2, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness2;
				if ((order.HazineMoeinGuid2 ?? Guid.Empty) != Guid.Empty)
				{
					await buyCredit4.SetMoeinGuidAsync(order.HazineMoeinGuid2.Value, token, tranName);
				}
				await buyCredit4.SetTafsilGuidAsync(order.HazineTafsilGuid2, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.TotalHazine3 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalHazine3,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderHazineDescAsync(system, 3, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness;
				if ((order.HazineMoeinGuid3 ?? Guid.Empty) != Guid.Empty)
				{
					await buyCredit4.SetMoeinGuidAsync(order.HazineMoeinGuid3.Value, token, tranName);
				}
				await buyCredit4.SetTafsilGuidAsync(order.HazineTafsilGuid3, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.TotalHazine4 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalHazine4,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderHazineDescAsync(system, 4, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness2;
				if ((order.HazineMoeinGuid4 ?? Guid.Empty) != Guid.Empty)
				{
					await buyCredit4.SetMoeinGuidAsync(order.HazineMoeinGuid4.Value, token, tranName);
				}
				await buyCredit4.SetTafsilGuidAsync(order.HazineTafsilGuid4, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.TotalHazine5 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.TotalHazine5,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderHazineDescAsync(system, 5, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness;
				if ((order.HazineMoeinGuid5 ?? Guid.Empty) != Guid.Empty)
				{
					await buyCredit4.SetMoeinGuidAsync(order.HazineMoeinGuid5.Value, token, tranName);
				}
				await buyCredit4.SetTafsilGuidAsync(order.HazineTafsilGuid5, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
			if (order.Discount > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Debit = 0m,
					Credit = order.Discount
				};
				Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderDiscountDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				Sanad_DetBusiness buyCredit4 = sanad_DetBusiness2;
				await buyCredit4.SetMoeinGuidAsync(order.DiscountMoeinGuid, token, tranName);
				await buyCredit4.SetTafsilGuidAsync(order.DiscountTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(buyCredit4, system, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 681);
			throw;
		}
	}

	protected async Task GenerateOrderBackBuyAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 23;
		try
		{
			Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
			{
				Credit = 0m,
				Debit = order.TotalSum
			};
			Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
			sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
			sanad_DetBusiness.DecimalGuid = Guid.Empty;
			sanad_DetBusiness.Guid = Guid.NewGuid();
			sanad_DetBusiness.Qt = 0m;
			sanad_DetBusiness.MasterGuid = sanad.Guid;
			Sanad_DetBusiness debitBackBuy = sanad_DetBusiness;
			await debitBackBuy.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
			await debitBackBuy.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(debitBackBuy, system, token);
			if (order.ListKala?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
					{
						decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum + q.TaxImpactonCost);
						if (!(num <= 0m))
						{
							Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Credit = num,
								Debit = 0m
							};
							sanad_DetBusiness2 = sanad_DetBusiness3;
							sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness3.DecimalGuid = Guid.Empty;
							sanad_DetBusiness3.Guid = Guid.NewGuid();
							sanad_DetBusiness3.Qt = 0m;
							sanad_DetBusiness3.MasterGuid = sanad.Guid;
							sanad_DetBusiness = sanad_DetBusiness3;
							await sanad_DetBusiness.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
							await sanad_DetBusiness.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
						}
					}
				}
			}
			if (order.Tax > 0m)
			{
				sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Tax,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness3 = sanad_DetBusiness2;
				sanad_DetBusiness3.RowDesc = await order.GetOrderTaxDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				sanad_DetBusiness = sanad_DetBusiness2;
				if ((order.TaxMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.TaxMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Tax;
				}
				await sanad_DetBusiness.SetMoeinGuidAsync(order.TaxMoeinGuid.Value, token, tranName);
				await sanad_DetBusiness.SetTafsilGuidAsync(order.TaxTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
			}
			if (order.Avarez > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Avarez,
					Debit = 0m
				};
				sanad_DetBusiness2 = sanad_DetBusiness3;
				sanad_DetBusiness2.RowDesc = await order.GetOrderAvarezDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness3.DecimalGuid = Guid.Empty;
				sanad_DetBusiness3.Guid = Guid.NewGuid();
				sanad_DetBusiness3.Qt = 0m;
				sanad_DetBusiness3.MasterGuid = sanad.Guid;
				sanad_DetBusiness = sanad_DetBusiness3;
				if ((order.AvarezMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.AvarezMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Avarez;
				}
				await sanad_DetBusiness.SetMoeinGuidAsync(order.AvarezMoeinGuid.Value, token, tranName);
				await sanad_DetBusiness.SetTafsilGuidAsync(order.AvarezTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
			}
			if (order.Keraye > 0m)
			{
				sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = order.Keraye,
					Debit = 0m
				};
				Sanad_DetBusiness sanad_DetBusiness3 = sanad_DetBusiness2;
				sanad_DetBusiness3.RowDesc = await order.GetOrderFareDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				sanad_DetBusiness = sanad_DetBusiness2;
				if ((order.FareMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.FareMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Keraye;
				}
				await sanad_DetBusiness.SetMoeinGuidAsync(order.FareMoeinGuid.Value, token, tranName);
				await sanad_DetBusiness.SetTafsilGuidAsync(order.FareTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
			}
			if (order.Discount > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Debit = order.Discount,
					Credit = 0m
				};
				sanad_DetBusiness2 = sanad_DetBusiness3;
				sanad_DetBusiness2.RowDesc = await order.GetOrderDiscountDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness3.DecimalGuid = Guid.Empty;
				sanad_DetBusiness3.Guid = Guid.NewGuid();
				sanad_DetBusiness3.Qt = 0m;
				sanad_DetBusiness3.MasterGuid = sanad.Guid;
				sanad_DetBusiness = sanad_DetBusiness3;
				await sanad_DetBusiness.SetMoeinGuidAsync(order.DiscountMoeinGuid, token, tranName);
				await sanad_DetBusiness.SetTafsilGuidAsync(order.DiscountTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderBackBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 805);
			throw;
		}
	}

	protected virtual async Task GenerateOrderBackSellAsync(OrderBusiness order, SanadBusiness sanad, AccGlobalSettings setting, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 43;
		try
		{
			Sanad_DetBusiness debitBackSell7;
			Sanad_DetBusiness sanad_DetBusiness;
			if (order.ListProduct?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListProduct
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable)
					{
						decimal num = item.Sum((OrderDetailBusiness q) => q.RowSum - q.TotalVisitorPrice + q.TaxImpactonCost);
						if (!(num <= 0m))
						{
							sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Debit = num,
								Credit = 0m
							};
							Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
							sanad_DetBusiness2.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness.DecimalGuid = Guid.Empty;
							sanad_DetBusiness.Guid = Guid.NewGuid();
							sanad_DetBusiness.Qt = 0m;
							sanad_DetBusiness.MasterGuid = sanad.Guid;
							debitBackSell7 = sanad_DetBusiness;
							await debitBackSell7.SetMoeinGuidAsync(order.ProductMoeinGuid, token, tranName);
							await debitBackSell7.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(debitBackSell7, system, token);
						}
					}
				}
			}
			if (order.ListService?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable2 = from q in order.ListService
					group q by q.ProductTafsilGuid;
				if (enumerable2 != null && enumerable2.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item in enumerable2)
					{
						decimal num2 = item.Sum((OrderDetailBusiness q) => q.RowSum - q.TotalVisitorPrice + q.TaxImpactonCost);
						if (!(num2 <= 0m))
						{
							Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
							{
								Debit = num2,
								Credit = 0m
							};
							sanad_DetBusiness = sanad_DetBusiness2;
							sanad_DetBusiness.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
							sanad_DetBusiness2.DecimalGuid = Guid.Empty;
							sanad_DetBusiness2.Guid = Guid.NewGuid();
							sanad_DetBusiness2.Qt = 0m;
							sanad_DetBusiness2.MasterGuid = sanad.Guid;
							debitBackSell7 = sanad_DetBusiness2;
							await debitBackSell7.SetMoeinGuidAsync(order.ServiceMoeinGuid.Value, token, tranName);
							await debitBackSell7.SetTafsilGuidAsync(item.Key, tranName, token);
							await sanad.AddToListSanadAsync(debitBackSell7, system, token);
						}
					}
				}
			}
			if (order.Tax > 0m)
			{
				sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.Tax
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderTaxDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness;
				if ((order.TaxMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.TaxMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Tax;
				}
				await debitBackSell7.SetMoeinGuidAsync(order.TaxMoeinGuid.Value, token, tranName);
				await debitBackSell7.SetTafsilGuidAsync(order.TaxTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.Avarez > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.Avarez
				};
				sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderAvarezDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness2;
				if ((order.AvarezMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					order.AvarezMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Avarez;
				}
				await debitBackSell7.SetMoeinGuidAsync(order.AvarezMoeinGuid.Value, token, tranName);
				await debitBackSell7.SetTafsilGuidAsync(order.AvarezTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.Keraye > 0m)
			{
				sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.Keraye
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderFareDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness;
				if ((order.FareMoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitBackSell7.SetMoeinGuidAsync(order.FareMoeinGuid.Value, token, tranName);
				}
				await debitBackSell7.SetTafsilGuidAsync(order.FareTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.B1 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.B1
				};
				sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderVisitorDescAsync(system, 1, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness2;
				if ((order.B1MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitBackSell7.SetMoeinGuidAsync(order.B1MoeinGuid.Value, token, tranName);
				}
				await debitBackSell7.SetTafsilGuidAsync(order.B1TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.B2 > 0m)
			{
				sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.B2
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderVisitorDescAsync(system, 2, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness;
				if ((order.B2MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitBackSell7.SetMoeinGuidAsync(order.B2MoeinGuid.Value, token, tranName);
				}
				await debitBackSell7.SetTafsilGuidAsync(order.B2TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.B3 > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.B3
				};
				sanad_DetBusiness = sanad_DetBusiness2;
				sanad_DetBusiness.RowDesc = await order.GetOrderVisitorDescAsync(system, 3, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness2;
				if ((order.B3MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitBackSell7.SetMoeinGuidAsync(order.B3MoeinGuid.Value, token, tranName);
				}
				await debitBackSell7.SetTafsilGuidAsync(order.B3TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			if (order.B4 > 0m)
			{
				sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = order.B4
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await order.GetOrderVisitorDescAsync(system, 4, order.ContextGuid, tranName, token);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				sanad_DetBusiness.MasterGuid = sanad.Guid;
				debitBackSell7 = sanad_DetBusiness;
				if ((order.B4MoeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					await debitBackSell7.SetMoeinGuidAsync(order.B4MoeinGuid.Value, token, tranName);
				}
				await debitBackSell7.SetTafsilGuidAsync(order.B4TafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(debitBackSell7, system, token);
			}
			sanad_DetBusiness = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
			{
				Debit = 0m,
				Credit = order.TotalSum
			};
			debitBackSell7 = sanad_DetBusiness;
			debitBackSell7.RowDesc = await order.GetOrderDescAsync(setting, system, order.ContextGuid, tranName, token);
			sanad_DetBusiness.DecimalGuid = Guid.Empty;
			sanad_DetBusiness.Guid = Guid.NewGuid();
			sanad_DetBusiness.Qt = 0m;
			sanad_DetBusiness.MasterGuid = sanad.Guid;
			Sanad_DetBusiness backSellCredit = sanad_DetBusiness;
			await backSellCredit.SetMoeinGuidAsync(order.MoeinGuid, token, tranName);
			await backSellCredit.SetTafsilGuidAsync(order.TafsilGuid, tranName, token);
			await sanad.AddToListSanadAsync(backSellCredit, system, token);
			if (order.Discount > 0m)
			{
				Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(order.ContextGuid, system, order.Guid, order.FactorType, sanad.NextOrdinal())
				{
					Debit = 0m,
					Credit = order.Discount
				};
				debitBackSell7 = sanad_DetBusiness2;
				debitBackSell7.RowDesc = await order.GetOrderDiscountDescAsync(system, order.ContextGuid, tranName, token);
				sanad_DetBusiness2.DecimalGuid = Guid.Empty;
				sanad_DetBusiness2.Guid = Guid.NewGuid();
				sanad_DetBusiness2.Qt = 0m;
				sanad_DetBusiness2.MasterGuid = sanad.Guid;
				sanad_DetBusiness = sanad_DetBusiness2;
				await sanad_DetBusiness.SetMoeinGuidAsync(order.DiscountMoeinGuid, token, tranName);
				await sanad_DetBusiness.SetTafsilGuidAsync(order.DiscountTafsilGuid, tranName, token);
				await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateOrderBackSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1024);
			throw;
		}
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs"))
		{
			_ = 8;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				switch (order.FactorType)
				{
				case Document.OrderSell:
					await GetSellCoverAsync(order, list, system, token, tranName);
					break;
				case Document.OrderBuy:
					await GetBuyCoverAsync(order, list, system, token, tranName);
					break;
				case Document.OrderBackBuy:
					await GetBackBuyCoverAsync(order, list, system, token, tranName);
					break;
				case Document.OrderBackSell:
					await GetBackSellCoverAsync(order, list, system, token, tranName);
					break;
				case Document.Zayeat:
					await GetZayeatCoverAsync(order, list, system, token, tranName);
					break;
				case Document.PishFactor:
					await GetPishFactorCoverAsync(order, list, system, token, tranName);
					break;
				case Document.AmaniVorod:
					await GetAmaniVorodCoverAsync(order, list, system, token, tranName);
					break;
				case Document.AmaniKhoroj:
					await GetAmaniKhorojCoverAsync(order, list, system, token, tranName);
					break;
				case Document.OrderPreBuy:
					await GetPreBuyCoverAsync(order, list, system, token, tranName);
					break;
				}
				if (order.ListSanad_Det?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item in order.ListSanad_Det)
					{
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
						{
							DebitMoeinGuid = ((item.Debit > 0m) ? new Guid?(item.MoeinGuid) : null),
							DebitTafsilGuid = ((item.Debit > 0m) ? item.TafsilGuid : null),
							Date = order.GetDate(),
							AsGuid = order.Guid,
							AsTypeDocument = order.FactorType,
							Debit = item.Debit,
							Credit = item.Credit,
							CashierGuid = order.UserAccGuid,
							SanadNo = order.SanadNo,
							SanadSt = order.SanadSt,
							Id2 = order.SanadNo,
							CreditMoeinGuid = ((item.Credit > 0m) ? new Guid?(item.MoeinGuid) : null),
							CreditTafsilGuid = ((item.Credit > 0m) ? item.TafsilGuid : null),
							SanadDesc = order.Description,
							RDesc = item.RowDesc,
							DateMoaser = order.GetDate(),
							DetailGuid = item.Guid,
							Guid = Guid.NewGuid(),
							DecimalGuid = item.DecimalGuid,
							DecimalPrice = 0m,
							DecimalQt = item.Qt,
							DepotGuid = null,
							Modified = DateTime.Now,
							Price = 0m,
							ProductGuid = null,
							Qt = 0m,
							Qt2 = 0m,
							RBazaryab = 0m,
							RDiscount = 0m,
							Serial = "",
							Status = true,
							VahedGuid = null
						};
						switch (order.FactorType)
						{
						case Document.OrderSell:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.SellRelatedSanad;
							break;
						case Document.OrderBuy:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.BuyRelatedSanad;
							break;
						case Document.OrderBackBuy:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.BargashtBuyRelatedSanad;
							break;
						case Document.OrderBackSell:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.BargashtSellRelatedSanad;
							break;
						case Document.PishFactor:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.PishFactorRelatedSanad;
							break;
						case Document.Zayeat:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.ZayeatRelatedSanad;
							break;
						case Document.AmaniVorod:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.AmaniVorodRelatedSanad;
							break;
						case Document.AmaniKhoroj:
							viewSanadDetailCoverBusiness.AsType = DocumentDet.AmaniKhorojRelatedSanad;
							break;
						}
						list.Add(viewSanadDetailCoverBusiness);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1108);
				throw;
			}
		}
	}

	protected async Task GetPreBuyCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 3;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.ProductMoeinGuid
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.Date = order.GetDate();
				viewSanadDetailCoverBusiness.AsGuid = order.Guid;
				viewSanadDetailCoverBusiness.AsType = DocumentDet.PreBuyPrd;
				viewSanadDetailCoverBusiness.AsTypeDocument = order.FactorType;
				viewSanadDetailCoverBusiness.Debit = 0m;
				viewSanadDetailCoverBusiness.Credit = 0m;
				viewSanadDetailCoverBusiness.CashierGuid = order.UserAccGuid;
				viewSanadDetailCoverBusiness.SanadNo = order.SanadNo;
				viewSanadDetailCoverBusiness.SanadSt = order.SanadSt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness;
				string translate = "درخواست خرید شماره".GetTranslate();
				int num = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness5.SanadDesc = translate + " " + $"{num}" + " " + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0} {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}" + "** " + "شرح فاکتور".GetTranslate() + ": " + order.Description;
				list.Add(viewSanadDetailCoverBusiness);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPreBuyCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1157);
			throw;
		}
	}

	protected async Task GetAmaniKhorojCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.AmaniKhoroj,
					AsTypeDocument = order.FactorType,
					Debit = order.TotalSum,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ProductMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness;
				string translate = "امانی صادره به شماره".GetTranslate();
				int num = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness5.SanadDesc = translate + " " + $"{num}" + " " + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0} {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}";
				list2.Add(viewSanadDetailCoverBusiness);
				list2 = list;
				viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.AmaniKhoroj,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalSum,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.ProductMoeinGuid;
				viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness5.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = item.Price;
				viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness2.Qt = item.Quantity;
				viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness2.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness2.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness2.Serial = item.Serial;
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = item.VGuid;
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				translate = "امانی صادره به شماره".GetTranslate();
				int num2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.SanadDesc = translate + $" {num2}" + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0} {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}";
				list2.Add(viewSanadDetailCoverBusiness2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAmaniKhorojCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1243);
			throw;
		}
	}

	protected async Task GetAmaniVorodCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.ProductMoeinGuid
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.Date = order.GetDate();
				viewSanadDetailCoverBusiness.AsGuid = order.Guid;
				viewSanadDetailCoverBusiness.AsType = DocumentDet.AmaniVorod;
				viewSanadDetailCoverBusiness.AsTypeDocument = order.FactorType;
				viewSanadDetailCoverBusiness.Debit = order.TotalSum;
				viewSanadDetailCoverBusiness.Credit = 0m;
				viewSanadDetailCoverBusiness.CashierGuid = order.UserAccGuid;
				viewSanadDetailCoverBusiness.SanadNo = order.SanadNo;
				viewSanadDetailCoverBusiness.SanadSt = order.SanadSt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness;
				string translate = "امانی وارده به شماره ".GetTranslate();
				int num = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness5.SanadDesc = translate + " " + $"{num}" + " " + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0} {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}";
				list2.Add(viewSanadDetailCoverBusiness);
				list2 = list;
				viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.ProductMoeinGuid
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness2.Date = order.GetDate();
				viewSanadDetailCoverBusiness2.AsGuid = order.Guid;
				viewSanadDetailCoverBusiness2.AsType = DocumentDet.AmaniVorod;
				viewSanadDetailCoverBusiness2.AsTypeDocument = order.FactorType;
				viewSanadDetailCoverBusiness2.Debit = 0m;
				viewSanadDetailCoverBusiness2.Credit = order.TotalSum;
				viewSanadDetailCoverBusiness2.CashierGuid = order.UserAccGuid;
				viewSanadDetailCoverBusiness2.SanadNo = order.SanadNo;
				viewSanadDetailCoverBusiness2.SanadSt = order.SanadSt;
				viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness5.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = item.Price;
				viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness2.Qt = item.Quantity;
				viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness2.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness2.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness2.Serial = item.Serial;
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = item.VGuid;
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				translate = "امانی وارده به شماره".GetTranslate();
				int num2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.SanadDesc = translate + $" {num2}" + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0}  {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}";
				list2.Add(viewSanadDetailCoverBusiness2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAmaniVorodCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1329);
			throw;
		}
	}

	protected async Task GetPishFactorCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 3;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.PishFactorPrd,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ProductMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness;
				string translate = "پیش فاکتور فروش شماره".GetTranslate();
				int num = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness5.SanadDesc = translate + " " + $"{num}" + " " + " جمع".GetTranslate() + ":" + $" {order.TotalSum:N0} {ClsCache.GetCache(order.ContextGuid).MultiLanguageConfig.CurrencyUnit}" + "**" + " شرح فاکتور".GetTranslate() + ":" + order.Description;
				list.Add(viewSanadDetailCoverBusiness);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPishFactorCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1377);
			throw;
		}
	}

	protected async Task GetZayeatCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 5;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.Zayeat,
					AsTypeDocument = order.FactorType,
					Debit = item.RowSum,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ProductMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
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
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.Zayeat,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = item.RowSum - item.TotalVisitorPrice,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.ProductMoeinGuid;
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = item.Price;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetZayeatCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 1463);
			throw;
		}
	}

	protected virtual async Task GetSellCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 26;
		try
		{
			foreach (OrderDetailBusiness item in order.ListProduct)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellPrd,
					AsTypeDocument = order.FactorType,
					Debit = item.RowSumWithTax,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ProductMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness);
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.MoeinGuid,
						DebitTafsilGuid = order.TafsilGuid,
						Date = order.GetDate(),
						AsGuid = order.Guid,
						AsType = DocumentDet.SellPrd,
						AsTypeDocument = order.FactorType,
						Debit = 0m,
						Credit = item.RowSum - item.TotalVisitorPrice + item.TaxImpactonCost,
						CashierGuid = order.UserAccGuid,
						SanadNo = order.SanadNo,
						SanadSt = order.SanadSt
					};
					viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.ProductMoeinGuid;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness4.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
					viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = item.Price;
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
			foreach (OrderDetailBusiness item in order.ListService)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellPrd,
					AsTypeDocument = order.FactorType,
					Debit = item.RowSumWithTax,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ServiceMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness);
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.MoeinGuid,
						DebitTafsilGuid = order.TafsilGuid,
						Date = order.GetDate(),
						AsGuid = order.Guid,
						AsType = DocumentDet.SellPrd,
						AsTypeDocument = order.FactorType,
						Debit = 0m,
						Credit = item.RowSum - item.TotalVisitorPrice + item.TaxImpactonCost,
						CashierGuid = order.UserAccGuid,
						SanadNo = order.SanadNo,
						SanadSt = order.SanadSt
					};
					viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.ServiceMoeinGuid;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness4.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
					viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = item.Price;
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
			if (order.Keraye > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellKeraye,
					AsTypeDocument = order.FactorType,
					Debit = order.Keraye,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.FareMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.FareTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "کرایه";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
				list2 = list;
				viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellKeraye,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Keraye,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.FareMoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.FareTafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
			}
			if (order.Tax > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellTax,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Tax,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.TaxMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TaxTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.Avarez > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellAvarez,
					AsTypeDocument = order.FactorType,
					Debit = order.Avarez,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.AvarezMoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.AvarezTafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "عوارض شهرداری".GetTranslate();
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellAvarez,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Avarez,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.AvarezMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.AvarezTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.B1 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.B1,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.B1MoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.B1TafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
			}
			if (order.B2 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.B2,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.B2MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.B2TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.B3 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.B3,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.B3MoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.B3TafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
			}
			if (order.B4 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.B4,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.B4MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.B4TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = "بازاریابی (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "بازاریابی (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.Discount > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.DiscountMoeinGuid,
					DebitTafsilGuid = order.DiscountTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellDiscount,
					AsTypeDocument = order.FactorType,
					Debit = order.Discount,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.DiscountMoeinGuid,
					DebitTafsilGuid = order.DiscountTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.SellDiscount,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Discount,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "تخفیف".GetTranslate();
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 2050);
			throw;
		}
	}

	protected async Task GetBuyCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 16;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				List<ViewSanadDetailCoverBusiness> list2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4;
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.ProductMoeinGuid
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness2.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness.Date = order.GetDate();
					viewSanadDetailCoverBusiness.AsGuid = order.Guid;
					viewSanadDetailCoverBusiness.AsType = DocumentDet.BuyPrd;
					viewSanadDetailCoverBusiness.AsTypeDocument = order.FactorType;
					viewSanadDetailCoverBusiness.Debit = item.RowSum + item.TotalHazinePrice + item.TaxImpactonCost;
					viewSanadDetailCoverBusiness.Credit = 0m;
					viewSanadDetailCoverBusiness.CashierGuid = order.UserAccGuid;
					viewSanadDetailCoverBusiness.SanadNo = order.SanadNo;
					viewSanadDetailCoverBusiness.SanadSt = order.SanadSt;
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
					viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
					viewSanadDetailCoverBusiness.SanadDesc = (string.IsNullOrEmpty(order.SellerFactorNo) ? order.Description : (order.Description + "(" + "شماره فاکتور فروشنده".GetTranslate() + ": " + order.SellerFactorNo + ")"));
					viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness.Price = item.Price;
					viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
					viewSanadDetailCoverBusiness.Qt = item.Quantity;
					viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
					viewSanadDetailCoverBusiness.RBazaryab = 0m;
					viewSanadDetailCoverBusiness.RDiscount = item.Discount;
					viewSanadDetailCoverBusiness.Serial = item.Serial;
					viewSanadDetailCoverBusiness.Status = true;
					viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
					list2.Add(viewSanadDetailCoverBusiness);
				}
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyPrd,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = item.RowSumWithTax,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = (string.IsNullOrEmpty(order.SellerFactorNo) ? order.Description : (order.Description + " (" + "شماره فاکتور فروشنده".GetTranslate() + ": " + order.SellerFactorNo + ")"));
				viewSanadDetailCoverBusiness3.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness3.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness3.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = item.Price;
				viewSanadDetailCoverBusiness3.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness3.Qt = item.Quantity;
				viewSanadDetailCoverBusiness3.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness3.Serial = item.Serial;
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.ListKala?.Any() ?? false)
			{
				IEnumerable<IGrouping<Guid?, OrderDetailBusiness>> enumerable = from q in order.ListKala
					group q by q.ProductTafsilGuid;
				if (enumerable != null && enumerable.Any())
				{
					foreach (IGrouping<Guid?, OrderDetailBusiness> item2 in enumerable)
					{
						if (item2.Sum((OrderDetailBusiness q) => q.TotalHazinePrice) > 0m)
						{
							List<ViewSanadDetailCoverBusiness> list2 = list;
							ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
							{
								DebitMoeinGuid = order.ProductMoeinGuid,
								DebitTafsilGuid = item2.Key,
								Date = order.GetDate(),
								AsGuid = order.Guid,
								AsType = DocumentDet.BuyHFactorBuy,
								AsTypeDocument = order.FactorType,
								Debit = item2.Sum((OrderDetailBusiness q) => q.TotalHazinePrice),
								Credit = 0m,
								CashierGuid = order.UserAccGuid,
								SanadNo = order.SanadNo,
								SanadSt = order.SanadSt
							};
							ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
							viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
							viewSanadDetailCoverBusiness4.CreditMoeinGuid = null;
							viewSanadDetailCoverBusiness4.CreditTafsilGuid = null;
							viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
							viewSanadDetailCoverBusiness4.RDesc = "";
							viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
							viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
							viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
							viewSanadDetailCoverBusiness4.DecimalGuid = null;
							viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
							viewSanadDetailCoverBusiness4.DecimalQt = 0m;
							viewSanadDetailCoverBusiness4.DepotGuid = null;
							viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
							viewSanadDetailCoverBusiness4.Price = 0m;
							viewSanadDetailCoverBusiness4.ProductGuid = null;
							viewSanadDetailCoverBusiness4.Qt = 0m;
							viewSanadDetailCoverBusiness4.Qt2 = 0m;
							viewSanadDetailCoverBusiness4.RBazaryab = 0m;
							viewSanadDetailCoverBusiness4.RDiscount = 0m;
							viewSanadDetailCoverBusiness4.Serial = "";
							viewSanadDetailCoverBusiness4.Status = true;
							viewSanadDetailCoverBusiness4.VahedGuid = null;
							list2.Add(viewSanadDetailCoverBusiness4);
						}
					}
				}
			}
			if (order.Keraye > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.FareMoeinGuid,
					DebitTafsilGuid = order.FareTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyKeraye,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Keraye,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.Tax > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.TaxMoeinGuid,
					DebitTafsilGuid = order.TaxTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyTax,
					AsTypeDocument = order.FactorType,
					Debit = order.Tax,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "مالیات ارزش افزوده".GetTranslate();
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.Avarez > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.AvarezMoeinGuid,
					DebitTafsilGuid = order.AvarezTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyAvarez,
					AsTypeDocument = order.FactorType,
					Debit = order.Avarez,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "عوارض شهرداری".GetTranslate();
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
				list2 = list;
				viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.AvarezMoeinGuid,
					DebitTafsilGuid = order.AvarezTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyAvarez,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Avarez,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.Discount > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyDiscount,
					AsTypeDocument = order.FactorType,
					Debit = order.Discount,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.DiscountMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.DiscountTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "تخفیف".GetTranslate();
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
				list2 = list;
				viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyDiscount,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Discount,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.DiscountMoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.DiscountTafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.TotalHazine1 > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyHFactorBuy,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalHazine1,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.HazineMoeinGuid1;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.HazineTafsilGuid1;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.TotalHazine2 > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyHFactorBuy,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalHazine2,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.HazineMoeinGuid2;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.HazineTafsilGuid2;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.TotalHazine3 > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyHFactorBuy,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalHazine3,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.HazineMoeinGuid3;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.HazineTafsilGuid3;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.TotalHazine4 > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyHFactorBuy,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalHazine4,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.HazineMoeinGuid4;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.HazineTafsilGuid4;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.TotalHazine5 > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BuyHFactorBuy,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.TotalHazine5,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.HazineMoeinGuid5;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.HazineTafsilGuid5;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBuyCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 2595);
			throw;
		}
	}

	protected async Task GetBackBuyCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 12;
		try
		{
			foreach (OrderDetailBusiness item in order.ListKala)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyPrd,
					AsTypeDocument = order.FactorType,
					Debit = item.RowSumWithTax,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.ProductMoeinGuid;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = item.Price;
				viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness.Qt = item.Quantity;
				viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness.RBazaryab = item.TotalVisitorPrice;
				viewSanadDetailCoverBusiness.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness.Serial = item.Serial;
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness);
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.MoeinGuid,
						DebitTafsilGuid = order.TafsilGuid,
						Date = order.GetDate(),
						AsGuid = order.Guid,
						AsType = DocumentDet.BargashtBuyPrd,
						AsTypeDocument = order.FactorType,
						Debit = 0m,
						Credit = item.RowSum + item.TotalHazinePrice + item.TaxImpactonCost,
						CashierGuid = order.UserAccGuid,
						SanadNo = order.SanadNo,
						SanadSt = order.SanadSt
					};
					viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.ProductMoeinGuid;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness4.CreditTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
					viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = item.Price;
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
			if (order.Keraye > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyKeraye,
					AsTypeDocument = order.FactorType,
					Debit = order.Keraye,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.FareMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.FareTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "کرایه".GetTranslate();
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
				list2 = list;
				viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyKeraye,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Keraye,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.FareMoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.FareTafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
			}
			if (order.Tax > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyTax,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Tax,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.TaxMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TaxTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.Avarez > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyAvarez,
					AsTypeDocument = order.FactorType,
					Debit = order.Avarez,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.AvarezMoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.AvarezTafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "عوارض شهرداری".GetTranslate();
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyAvarez,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Avarez,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.AvarezMoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.AvarezTafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "";
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			if (order.Discount > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.DiscountMoeinGuid,
					DebitTafsilGuid = order.DiscountTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyDiscount,
					AsTypeDocument = order.FactorType,
					Debit = order.Discount,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
				viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness2.RDesc = "";
				viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness2.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness2.DecimalGuid = null;
				viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness2.DecimalQt = 0m;
				viewSanadDetailCoverBusiness2.DepotGuid = null;
				viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness2.Price = 0m;
				viewSanadDetailCoverBusiness2.ProductGuid = null;
				viewSanadDetailCoverBusiness2.Qt = 0m;
				viewSanadDetailCoverBusiness2.Qt2 = 0m;
				viewSanadDetailCoverBusiness2.RBazaryab = 0m;
				viewSanadDetailCoverBusiness2.RDiscount = 0m;
				viewSanadDetailCoverBusiness2.Serial = "";
				viewSanadDetailCoverBusiness2.Status = true;
				viewSanadDetailCoverBusiness2.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness2);
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.DiscountMoeinGuid,
					DebitTafsilGuid = order.DiscountTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtBuyDiscount,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Discount,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = "تخفیف".GetTranslate();
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = null;
				viewSanadDetailCoverBusiness3.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness3.DecimalQt = 0m;
				viewSanadDetailCoverBusiness3.DepotGuid = null;
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = 0m;
				viewSanadDetailCoverBusiness3.ProductGuid = null;
				viewSanadDetailCoverBusiness3.Qt = 0m;
				viewSanadDetailCoverBusiness3.Qt2 = 0m;
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = 0m;
				viewSanadDetailCoverBusiness3.Serial = "";
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBackBuyCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 2945);
			throw;
		}
	}

	protected virtual async Task GetBackSellCoverAsync(OrderBusiness order, List<ViewSanadDetailCoverBusiness> list, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 24;
		try
		{
			foreach (OrderDetailBusiness item in order.ListProduct)
			{
				List<ViewSanadDetailCoverBusiness> list2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4;
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.ProductMoeinGuid
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness2.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness.Date = order.GetDate();
					viewSanadDetailCoverBusiness.AsGuid = order.Guid;
					viewSanadDetailCoverBusiness.AsType = DocumentDet.BargashtSellPrd;
					viewSanadDetailCoverBusiness.AsTypeDocument = order.FactorType;
					viewSanadDetailCoverBusiness.Debit = item.RowSum - item.TotalVisitorPrice + item.TaxImpactonCost;
					viewSanadDetailCoverBusiness.Credit = 0m;
					viewSanadDetailCoverBusiness.CashierGuid = order.UserAccGuid;
					viewSanadDetailCoverBusiness.SanadNo = order.SanadNo;
					viewSanadDetailCoverBusiness.SanadSt = order.SanadSt;
					viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness3.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
					viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
					viewSanadDetailCoverBusiness.SanadDesc = order.Description;
					viewSanadDetailCoverBusiness.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
					viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness.Price = item.Price;
					viewSanadDetailCoverBusiness.ProductGuid = item.ProductGuid;
					viewSanadDetailCoverBusiness.Qt = item.Quantity;
					viewSanadDetailCoverBusiness.Qt2 = item.V2Qt.GetValueOrDefault();
					viewSanadDetailCoverBusiness.RBazaryab = 0m;
					viewSanadDetailCoverBusiness.RDiscount = item.Discount;
					viewSanadDetailCoverBusiness.Serial = item.Serial;
					viewSanadDetailCoverBusiness.Status = true;
					viewSanadDetailCoverBusiness.VahedGuid = item.VGuid;
					list2.Add(viewSanadDetailCoverBusiness);
				}
				list2 = list;
				viewSanadDetailCoverBusiness3 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellPrd,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = item.RowSumWithTax,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness3.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness3.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness3.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness3.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness3.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness3.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness3.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness3.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness3.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness3.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness3;
				viewSanadDetailCoverBusiness4.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness3.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness3.Price = item.Price;
				viewSanadDetailCoverBusiness3.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness3.Qt = item.Quantity;
				viewSanadDetailCoverBusiness3.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness3.RBazaryab = 0m;
				viewSanadDetailCoverBusiness3.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness3.Serial = item.Serial;
				viewSanadDetailCoverBusiness3.Status = true;
				viewSanadDetailCoverBusiness3.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness3);
			}
			foreach (OrderDetailBusiness item in order.ListService)
			{
				List<ViewSanadDetailCoverBusiness> list2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness;
				if (item.RowSum != 0m)
				{
					list2 = list;
					viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
					{
						DebitMoeinGuid = order.ServiceMoeinGuid
					};
					ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness3.DebitTafsilGuid = await item.GetProductTafsilGuidAsync(token);
					viewSanadDetailCoverBusiness2.Date = order.GetDate();
					viewSanadDetailCoverBusiness2.AsGuid = order.Guid;
					viewSanadDetailCoverBusiness2.AsType = DocumentDet.BargashtSellPrd;
					viewSanadDetailCoverBusiness2.AsTypeDocument = order.FactorType;
					viewSanadDetailCoverBusiness2.Debit = item.RowSum - item.TotalVisitorPrice + item.TaxImpactonCost;
					viewSanadDetailCoverBusiness2.Credit = 0m;
					viewSanadDetailCoverBusiness2.CashierGuid = order.UserAccGuid;
					viewSanadDetailCoverBusiness2.SanadNo = order.SanadNo;
					viewSanadDetailCoverBusiness2.SanadSt = order.SanadSt;
					viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
					viewSanadDetailCoverBusiness2.CreditMoeinGuid = order.MoeinGuid;
					viewSanadDetailCoverBusiness2.CreditTafsilGuid = order.TafsilGuid;
					viewSanadDetailCoverBusiness2.SanadDesc = order.Description;
					viewSanadDetailCoverBusiness2.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
					viewSanadDetailCoverBusiness2.DateMoaser = order.GetDate();
					viewSanadDetailCoverBusiness2.DetailGuid = item.Guid;
					viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
					viewSanadDetailCoverBusiness2.DecimalGuid = item.DecimalGuid;
					viewSanadDetailCoverBusiness2.DecimalPrice = item.DecimalPrice;
					viewSanadDetailCoverBusiness2.DecimalQt = item.DecimalQt;
					viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
					viewSanadDetailCoverBusiness.DepotGuid = await item.GetDepotGuidAsync(token);
					viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
					viewSanadDetailCoverBusiness2.Price = item.Price;
					viewSanadDetailCoverBusiness2.ProductGuid = item.ProductGuid;
					viewSanadDetailCoverBusiness2.Qt = item.Quantity;
					viewSanadDetailCoverBusiness2.Qt2 = item.V2Qt.GetValueOrDefault();
					viewSanadDetailCoverBusiness2.RBazaryab = 0m;
					viewSanadDetailCoverBusiness2.RDiscount = item.Discount;
					viewSanadDetailCoverBusiness2.Serial = item.Serial;
					viewSanadDetailCoverBusiness2.Status = true;
					viewSanadDetailCoverBusiness2.VahedGuid = item.VGuid;
					list2.Add(viewSanadDetailCoverBusiness2);
				}
				list2 = list;
				viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellPrd,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = item.RowSumWithTax,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness2.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = (string.IsNullOrEmpty(item.SteppedDiscount) ? item.Description : (item.Description + " (تخفیف: " + ForceLTR(item.SteppedDiscount) + " درصد)"));
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = item.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = item.DecimalGuid;
				viewSanadDetailCoverBusiness4.DecimalPrice = item.DecimalPrice;
				viewSanadDetailCoverBusiness4.DecimalQt = item.DecimalQt;
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.DepotGuid = await item.GetDepotGuidAsync(token);
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = item.Price;
				viewSanadDetailCoverBusiness4.ProductGuid = item.ProductGuid;
				viewSanadDetailCoverBusiness4.Qt = item.Quantity;
				viewSanadDetailCoverBusiness4.Qt2 = item.V2Qt.GetValueOrDefault();
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = item.Discount;
				viewSanadDetailCoverBusiness4.Serial = item.Serial;
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = item.VGuid;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.Keraye > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.FareMoeinGuid,
					DebitTafsilGuid = order.FareTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellKeraye,
					AsTypeDocument = order.FactorType,
					Debit = order.Keraye,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = "کرایه".GetTranslate();
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
				list2 = list;
				viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.FareMoeinGuid,
					DebitTafsilGuid = order.FareTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellKeraye,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Keraye,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.B1 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.B1MoeinGuid,
					DebitTafsilGuid = order.B1TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = order.B1,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness.RDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
			}
			if (order.B2 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.B2MoeinGuid,
					DebitTafsilGuid = order.B2TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = order.B2,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.B3 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.B3MoeinGuid,
					DebitTafsilGuid = order.B3TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = order.B3,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness.RDesc = "کسر بازاریابی".GetTranslate() + "(" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
			}
			if (order.B4 > 0m)
			{
				TafsilBusiness tafsil4 = await TafsilBusiness.GetAsync(order.ContextGuid, system, order.TafsilGuid, tranName, token, "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs");
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.B4MoeinGuid,
					DebitTafsilGuid = order.B4TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellBazaryab,
					AsTypeDocument = order.FactorType,
					Debit = order.B4,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = "کسر بازاریابی".GetTranslate() + " (" + tafsil4?.Name + ") " + order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "کسر بازاریابی".GetTranslate() + "(" + tafsil4?.Name + ")";
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
			}
			if (order.Tax > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.TaxMoeinGuid,
					DebitTafsilGuid = order.TaxTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellTax,
					AsTypeDocument = order.FactorType,
					Debit = order.Tax,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = "مالیات ارزش افزوده".GetTranslate();
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
			}
			if (order.Avarez > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.AvarezMoeinGuid,
					DebitTafsilGuid = order.AvarezTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellAvarez,
					AsTypeDocument = order.FactorType,
					Debit = order.Avarez,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "عوارض شهرداری".GetTranslate();
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
				list2 = list;
				viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.AvarezMoeinGuid,
					DebitTafsilGuid = order.AvarezTafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellAvarez,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Avarez,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.MoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.TafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = "";
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
			}
			if (order.Discount > 0m)
			{
				List<ViewSanadDetailCoverBusiness> list2 = list;
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellDiscount,
					AsTypeDocument = order.FactorType,
					Debit = order.Discount,
					Credit = 0m,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness4;
				viewSanadDetailCoverBusiness.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness4.CreditMoeinGuid = order.DiscountMoeinGuid;
				viewSanadDetailCoverBusiness4.CreditTafsilGuid = order.DiscountTafsilGuid;
				viewSanadDetailCoverBusiness4.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness4.RDesc = "تخفیف".GetTranslate();
				viewSanadDetailCoverBusiness4.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness4.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness4.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness4.DecimalGuid = null;
				viewSanadDetailCoverBusiness4.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness4.DecimalQt = 0m;
				viewSanadDetailCoverBusiness4.DepotGuid = null;
				viewSanadDetailCoverBusiness4.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness4.Price = 0m;
				viewSanadDetailCoverBusiness4.ProductGuid = null;
				viewSanadDetailCoverBusiness4.Qt = 0m;
				viewSanadDetailCoverBusiness4.Qt2 = 0m;
				viewSanadDetailCoverBusiness4.RBazaryab = 0m;
				viewSanadDetailCoverBusiness4.RDiscount = 0m;
				viewSanadDetailCoverBusiness4.Serial = "";
				viewSanadDetailCoverBusiness4.Status = true;
				viewSanadDetailCoverBusiness4.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness4);
				list2 = list;
				viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(order.ContextGuid)
				{
					DebitMoeinGuid = order.MoeinGuid,
					DebitTafsilGuid = order.TafsilGuid,
					Date = order.GetDate(),
					AsGuid = order.Guid,
					AsType = DocumentDet.BargashtSellDiscount,
					AsTypeDocument = order.FactorType,
					Debit = 0m,
					Credit = order.Discount,
					CashierGuid = order.UserAccGuid,
					SanadNo = order.SanadNo,
					SanadSt = order.SanadSt
				};
				viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness;
				viewSanadDetailCoverBusiness4.Id2 = await order.GetId2Async(system, tranName, token);
				viewSanadDetailCoverBusiness.CreditMoeinGuid = order.DiscountMoeinGuid;
				viewSanadDetailCoverBusiness.CreditTafsilGuid = order.DiscountTafsilGuid;
				viewSanadDetailCoverBusiness.SanadDesc = order.Description;
				viewSanadDetailCoverBusiness.RDesc = "";
				viewSanadDetailCoverBusiness.DateMoaser = order.GetDate();
				viewSanadDetailCoverBusiness.DetailGuid = order.Guid;
				viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
				viewSanadDetailCoverBusiness.DecimalGuid = null;
				viewSanadDetailCoverBusiness.DecimalPrice = 0m;
				viewSanadDetailCoverBusiness.DecimalQt = 0m;
				viewSanadDetailCoverBusiness.DepotGuid = null;
				viewSanadDetailCoverBusiness.Modified = DateTime.Now;
				viewSanadDetailCoverBusiness.Price = 0m;
				viewSanadDetailCoverBusiness.ProductGuid = null;
				viewSanadDetailCoverBusiness.Qt = 0m;
				viewSanadDetailCoverBusiness.Qt2 = 0m;
				viewSanadDetailCoverBusiness.RBazaryab = 0m;
				viewSanadDetailCoverBusiness.RDiscount = 0m;
				viewSanadDetailCoverBusiness.Serial = "";
				viewSanadDetailCoverBusiness.Status = true;
				viewSanadDetailCoverBusiness.VahedGuid = null;
				list2.Add(viewSanadDetailCoverBusiness);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBackSellCoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderSanad\\OrderSanadGeneratorBase.cs", 3532);
			throw;
		}
	}

	private string ForceLTR(string text)
	{
		char c = '\u202a';
		char c2 = '\u202c';
		return $"{c}{text}{c2}";
	}
}
