using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.OrderDepot.OrderComparer;

public class OrderComparerWrapper : IHasContext
{
	private List<OrderDetailComparerWrapper> _detList;

	public string ContextGuid { get; private set; }

	public OrderBusiness Order { get; private set; }

	public IReadOnlyCollection<OrderDetailComparerWrapper> OriginalDetails
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<OrderDetailComparerWrapper>();
			}
			return _detList?.Where((OrderDetailComparerWrapper q) => q != null && !q.OriginalIsDisposed && q.OriginalDetail != null && q.ComparedQuantity != 0m)?.ToList();
		}
	}

	public IReadOnlyCollection<OrderDetailComparerWrapper> CurrentDetails
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<OrderDetailComparerWrapper>();
			}
			return _detList?.Where((OrderDetailComparerWrapper q) => q != null && !q.CurrentIsDisposed && q.CurrentDetail != null)?.ToList();
		}
	}

	public event Func<object, EventArgs, Task> OnItemChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	private OrderComparerWrapper(string contextGuid, OrderBusiness order)
	{
		ContextGuid = contextGuid;
		Order = order;
	}

	public static async Task<OrderComparerWrapper> GetFactoryAsync(string contextGuid, OrderBusiness order)
	{
		try
		{
			OrderComparerWrapper obj = new OrderComparerWrapper(contextGuid, order);
			await obj.GenerateDetailsAsync(order);
			return obj;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 56);
			throw;
		}
	}

	private async Task GenerateDetailsAsync(OrderBusiness order)
	{
		_ = 2;
		try
		{
			if (!(order?.ListKala?.Any()).GetValueOrDefault())
			{
				return;
			}
			_detList = order.ListKala.Select((OrderDetailBusiness q) => new OrderDetailComparerWrapper
			{
				OriginalDetail = q,
				CurrentDetail = null,
				ComparedQuantity = q.Quantity
			})?.ToList();
			List<Guid> list = await DepotReceiptBusiness.GetAllByOrderGuidAsync(ContextGuid, Order.Guid);
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (Guid item in list)
			{
				if (item == Guid.Empty)
				{
					continue;
				}
				DepotReceiptBusiness depotReceiptBusiness = await DepotReceiptBusiness.GetAsync(ContextGuid, item);
				if (!(depotReceiptBusiness?.DetailList?.Any()).GetValueOrDefault())
				{
					continue;
				}
				foreach (DepotReceiptDetailBusiness det in depotReceiptBusiness.DetailList)
				{
					if (det == null)
					{
						continue;
					}
					OrderDetailComparerWrapper existDetail = _detList?.SingleOrDefault((OrderDetailComparerWrapper q) => q.OriginalProductGuid == det.ProductGuid);
					if (existDetail != null)
					{
						decimal newQuantity = existDetail.OriginalQuantity - det.Quantity;
						await existDetail.OriginalDetail.SetQuantityAsync(EnSystem.Mapper, newQuantity);
						existDetail.ComparedQuantity = newQuantity;
						if (existDetail.OriginalQuantity <= 0m)
						{
							existDetail.OriginalDetail.Dispose();
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 97);
		}
	}

	public async Task AddOrUpdateAsync(EnSystem system, Guid productGuid, decimal quantity)
	{
		try
		{
			OrderDetailComparerWrapper existOriginal = OriginalDetails?.SingleOrDefault((OrderDetailComparerWrapper q) => q.OriginalProductGuid == productGuid);
			if (existOriginal == null)
			{
				existOriginal = _detList?.SingleOrDefault((OrderDetailComparerWrapper q) => q != null && !q.OriginalIsDisposed && q.OriginalDetail != null && q.ComparedQuantity == 0m && q.OriginalProductGuid == productGuid);
				if (existOriginal == null)
				{
					return;
				}
			}
			OrderDetailComparerWrapper orderDetailComparerWrapper = CurrentDetails?.SingleOrDefault((OrderDetailComparerWrapper q) => q.CurrentDetail.ProductGuid == productGuid);
			if (orderDetailComparerWrapper != null)
			{
				await orderDetailComparerWrapper.CurrentDetail.SetQuantityAsync(system, orderDetailComparerWrapper.CurrentDetail.Quantity + quantity);
			}
			else
			{
				OrderDetMapper instance = OrderDetMapper.GetInstance(null);
				string contextGuid = ContextGuid;
				OrderDetailBusiness originalDetail = existOriginal.OriginalDetail;
				OrderDetailBusiness mappedData = await instance.MapAsync(contextGuid, originalDetail, await OrderDetailBusiness.GetOrderDetailBusinessAsync(ContextGuid, Order));
				await mappedData.SetQuantityAsync(system, quantity);
				mappedData.Ordinal = ((!(CurrentDetails?.Any() ?? false)) ? 1 : checked((short)((CurrentDetails?.Select((OrderDetailComparerWrapper q) => q.CurrentDetail)?.Max((OrderDetailBusiness p) => p.Ordinal)).GetValueOrDefault(-1) + 1)));
				existOriginal.CurrentDetail = mappedData;
				existOriginal.OnDisposed -= ExistOriginal_OnDisposed;
				existOriginal.OnDisposed += ExistOriginal_OnDisposed;
			}
			existOriginal.ComparedQuantity -= quantity;
			if (this.OnItemChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnItemChanged, this, EventArgs.Empty, default(CancellationToken), 60, "AddOrUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddOrUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 127);
		}
	}

	private Task ExistOriginal_OnDisposed(object sender, EventArgs e)
	{
		try
		{
			OrderDetailComparerWrapper det = sender as OrderDetailComparerWrapper;
			if (det != null)
			{
				OrderDetailComparerWrapper orderDetailComparerWrapper = _detList?.SingleOrDefault((OrderDetailComparerWrapper q) => q.OriginalDetail != null && q.OriginalDetail.ProductGuid == det.CurrentProductGuid);
				if (orderDetailComparerWrapper != null)
				{
					orderDetailComparerWrapper.ComparedQuantity += det.CurrentQuantity;
				}
			}
			if (this.OnItemDisposed != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnItemDisposed, this, EventArgs.Empty, default(CancellationToken), 60, "ExistOriginal_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ExistOriginal_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 143);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> CompareAsync(DepotReceiptBusiness depotReceipt)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(depotReceipt?.DetailList?.Any()).GetValueOrDefault())
			{
				return await res.AddErrorAsync("رسید / حواله انبار فاقد اقلام می باشد", "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 151);
			}
			if (!(OriginalDetails?.Any() ?? false))
			{
				return await res.AddErrorAsync("فاکتور مرجع فاقد اقلام جهت تطبیق می باشد", "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 152);
			}
			List<Guid> receiptGuids = depotReceipt.DetailList.Select((DepotReceiptDetailBusiness q) => q.ProductGuid).Distinct().ToList();
			List<Guid> orderGuids = OriginalDetails.Select((OrderDetailComparerWrapper q) => q.OriginalProductGuid).Distinct().ToList();
			IEnumerable<Guid> enumerable = orderGuids.Where((Guid q) => !receiptGuids.Contains(q));
			if (enumerable != null && enumerable.Any())
			{
				await res.AddErrorAsync($"تعداد {enumerable.Count()} قلم کالا با فاکتور مرجع تطبیق ندارد", "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 159);
			}
			IEnumerable<Guid> enumerable2 = receiptGuids.Where((Guid q) => !orderGuids.Contains(q));
			if (enumerable2 != null && enumerable2.Any())
			{
				await res.AddErrorAsync($"تعداد {enumerable2.Count()} قلم کالا با فاکتور مرجع تطبیق ندارد", "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 163);
			}
			foreach (DepotReceiptDetailBusiness det in depotReceipt.DetailList)
			{
				if (det != null)
				{
					OrderDetailComparerWrapper orderDetailComparerWrapper = OriginalDetails?.SingleOrDefault((OrderDetailComparerWrapper q) => q.OriginalProductGuid == det.ProductGuid);
					if (orderDetailComparerWrapper != null && orderDetailComparerWrapper.OriginalQuantity != det.Quantity)
					{
						await res.AddErrorAsync(string.Format("کالای {0} در ردیف {1} به مقدار {2} با فاکتور مرجع تطبیق ندارد", det.ProductName, det.Ordinal, det.Quantity.NpToString("", 0)), "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 170);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 175);
			res.AddReturnedValue(ex, "", "CompareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderComparer\\OrderComparerWrapper.cs", 176);
		}
		return res;
	}
}
