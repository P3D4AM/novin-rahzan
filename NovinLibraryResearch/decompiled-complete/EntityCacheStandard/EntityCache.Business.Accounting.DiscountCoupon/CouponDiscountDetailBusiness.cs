using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;
using PacketParser.Filters.Customer;

namespace EntityCache.Business.Accounting.DiscountCoupon;

public class CouponDiscountDetailBusiness : Serializable<CouponDiscountDetailBusiness>, ICouponDiscountDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, IDisposable
{
	private bool _isDisposed;

	private List<CouponDiscountEntityRelationBusiness> _relationList;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public EnDiscountType EnDiscountType { get; set; }

	[JsonIgnore]
	public string TypeForDisplay => EnDiscountType.GetDisplay().GetTranslate();

	public decimal AmountCoupon { get; set; }

	[JsonIgnore]
	public double AmountPercent
	{
		get
		{
			if (EnDiscountType != EnDiscountType.ProductFixedPercent && EnDiscountType != 0)
			{
				return 0.0;
			}
			return (double)AmountCoupon;
		}
	}

	[JsonIgnore]
	public decimal AmountPrice
	{
		get
		{
			if (EnDiscountType != EnDiscountType.ProductFixedPrice && EnDiscountType != EnDiscountType.OrderFixedPrice)
			{
				return 0m;
			}
			return AmountCoupon;
		}
	}

	public decimal MinBuy { get; set; }

	public decimal MaxBuy { get; set; }

	public decimal MaxDiscountPrice { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	[JsonIgnore]
	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;

	[JsonIgnore]
	public CouponDiscountBusiness Master { get; private set; }

	public Guid? GiftGuid { get; set; }

	public bool ProductContains { get; set; }

	public bool CustomerContains { get; set; }

	public int GardeshAffectiveDays { get; set; }

	public decimal GardeshStartPrice { get; set; }

	public decimal GardeshEndPrice { get; set; }

	public decimal MinBuyQuantity { get; set; }

	internal IReadOnlyCollection<CouponDiscountEntityRelationBusiness> RelationList
	{
		get
		{
			if (_relationList == null)
			{
				_relationList = new List<CouponDiscountEntityRelationBusiness>();
			}
			_relationList.RemoveAll((CouponDiscountEntityRelationBusiness q) => q?.IsDisposed ?? true);
			return _relationList;
		}
	}

	[JsonIgnore]
	public IEnumerable<CouponDiscountEntityRelationBusiness> ProductList => RelationList?.Where((CouponDiscountEntityRelationBusiness q) => q.EntityType == EntityPart.Product);

	[JsonIgnore]
	public IEnumerable<CouponDiscountEntityRelationBusiness> ProductGroupList => RelationList?.Where((CouponDiscountEntityRelationBusiness q) => q.EntityType == EntityPart.ProductGroup);

	[JsonIgnore]
	public IEnumerable<CouponDiscountEntityRelationBusiness> CustomerList => RelationList?.Where((CouponDiscountEntityRelationBusiness q) => q.EntityType == EntityPart.Customer);

	[JsonIgnore]
	public IEnumerable<CouponDiscountEntityRelationBusiness> CustomerGroupList => RelationList?.Where((CouponDiscountEntityRelationBusiness q) => q.EntityType == EntityPart.CustomerGroup);

	public event Func<object, EventArgs, Task> OnDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	internal CouponDiscountDetailBusiness()
	{
	}

	public static async Task<CouponDiscountDetailBusiness> CouponDetailFactoryAsync(string contextGuid, CouponDiscountBusiness master, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CouponDetailFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs"))
		{
			CouponDiscountDetailBusiness ret = new CouponDiscountDetailBusiness();
			if (master == null)
			{
				throw new NullReferenceException("master");
			}
			try
			{
				await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				ret.ContextGuid = master.ContextGuid;
				ret.Master = master;
				ret.MasterGuid = master.Guid;
				ret.Status = true;
				ret.Modified = DateTime.Now;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "CouponDetailFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 80);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (AmountCoupon <= 0m && (GiftGuid ?? Guid.Empty) == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("مبلغ کوپن را وارد کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 88);
			}
			if ((EnDiscountType == EnDiscountType.OrderFixedPercent || EnDiscountType == EnDiscountType.ProductFixedPercent) && (AmountCoupon <= 0m || AmountCoupon > 100m) && (GiftGuid ?? Guid.Empty) == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError("درصد کوپن باید بین 1 تا 100 درصد باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 90);
			}
			if ((EnDiscountType == EnDiscountType.ProductFixedPrice || EnDiscountType == EnDiscountType.OrderFixedPrice) && AmountCoupon <= 0m)
			{
				returnedSaveFuncInfo.AddError("مبلغ کوپن را به درستی وارد کنید ", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 92);
			}
			if (MinBuy > 0m && MaxBuy > 0m && MinBuy > MaxBuy)
			{
				returnedSaveFuncInfo.AddError("مبلغ بشترین خرید و کمترین خرید را به درستی وارد کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 94);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 98);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 99);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Master = null;
				if (this.OnDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs");
				}
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 116);
		}
	}

	public Task AddEntityAsync(CouponDiscountEntityRelationBusiness rel, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (rel == null || token.IsCancellationRequested || base.IsDisposed)
			{
				return Task.CompletedTask;
			}
			if (_relationList == null)
			{
				_relationList = new List<CouponDiscountEntityRelationBusiness>();
			}
			_relationList.Add(rel);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddEntityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 126);
		}
		return Task.CompletedTask;
	}

	public async Task AddEntityListAsync(List<CouponDiscountEntityRelationBusiness> list, bool isClearList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			if (isClearList)
			{
				_relationList = new List<CouponDiscountEntityRelationBusiness>();
			}
			foreach (CouponDiscountEntityRelationBusiness item in list)
			{
				await AddEntityAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddEntityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 137);
		}
	}

	public async Task AddEntityGuidAsync(Guid entityGuid, EntityPart entityType, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(entityGuid == Guid.Empty) && !token.IsCancellationRequested && !base.IsDisposed && RelationList?.SingleOrDefault((CouponDiscountEntityRelationBusiness q) => q.EntityGuid == entityGuid && q.EntityType == entityType) == null)
			{
				CouponDiscountEntityRelationBusiness rel = new CouponDiscountEntityRelationBusiness(ContextGuid)
				{
					ContextGuid = ContextGuid,
					CouponDetailGuid = Guid,
					EntityGuid = entityGuid,
					EntityType = entityType
				};
				await AddEntityAsync(rel, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddEntityGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 156);
		}
	}

	public async Task AddEntityGuidListAsync(List<Guid> entityGuidList, EntityPart entityType, bool isClearData, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (isClearData)
			{
				_relationList?.Clear();
			}
			if (!(entityGuidList?.Any() ?? false) || token.IsCancellationRequested || base.IsDisposed)
			{
				return;
			}
			foreach (Guid entityGuid in entityGuidList)
			{
				await AddEntityGuidAsync(entityGuid, entityType, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddEntityGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 167);
		}
	}

	internal async Task<List<Guid>> GetAllCustomerContainsGuidListAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		try
		{
			List<Guid> list = new List<Guid>();
			CustomerCompactFilter filter = new CustomerCompactFilter
			{
				Status = true
			};
			IEnumerable<CusCompactBusiness> enumerable = await CusCompactBusiness.GetAllAsync(ContextGuid, filter, system, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			if (CustomerContains)
			{
				if (CustomerGroupList?.Any() ?? false)
				{
					list = enumerable.Where((CusCompactBusiness q) => CustomerGroupList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.GroupGuid))?.Select((CusCompactBusiness q) => q.Guid)?.ToList();
				}
				else if (CustomerList?.Any() ?? false)
				{
					list = enumerable.Where((CusCompactBusiness q) => CustomerList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.Guid))?.Select((CusCompactBusiness q) => q.Guid)?.ToList();
				}
			}
			else if (CustomerGroupList?.Any() ?? false)
			{
				list = enumerable.Where((CusCompactBusiness q) => !CustomerGroupList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.GroupGuid))?.Select((CusCompactBusiness q) => q.Guid)?.ToList();
			}
			else if (CustomerList?.Any() ?? false)
			{
				list = enumerable.Where((CusCompactBusiness q) => !CustomerList.Select((CouponDiscountEntityRelationBusiness q) => q.EntityGuid).Contains(q.Guid))?.Select((CusCompactBusiness q) => q.Guid)?.ToList();
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllCustomerContainsGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 198);
			throw;
		}
	}

	internal void ClearCustomerList()
	{
		try
		{
			IEnumerable<CouponDiscountEntityRelationBusiness> customerList = CustomerList;
			if (customerList == null || !customerList.Any())
			{
				return;
			}
			foreach (CouponDiscountEntityRelationBusiness customer in CustomerList)
			{
				_relationList?.Remove(customer);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearCustomerList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountDetailBusiness.cs", 212);
			throw;
		}
	}
}
