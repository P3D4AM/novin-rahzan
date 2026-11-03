using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ViewSanadDetailMapper : IItemAsyncMapper<ViewSanadDetailBusiness, WebViewSanadDetail>, IItemAsyncMapper<ViewSanadDetailBusiness, ViewSanadDetailBusiness>, IItemAsyncMapper<WebViewSanadDetail, ViewSanadDetailBusiness>
{
	public static ViewSanadDetailMapper Instance { get; private set; } = new ViewSanadDetailMapper();


	public Task<WebViewSanadDetail> MapAsync(string contextGuid, ViewSanadDetailBusiness source, WebViewSanadDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebViewSanadDetail();
			}
			destination.Guid = source.Guid;
			destination.KolGuid = source.KolGuid;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.KolGuid = source.KolGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Serial = source.Serial;
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			destination.TafsilGuid4 = source.TafsilGuid4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\ViewSanadDetailMapper.cs", 59);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ViewSanadDetailBusiness> MapAsync(string contextGuid, ViewSanadDetailBusiness source, ViewSanadDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ViewSanadDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.KolGuid = source.KolGuid;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.KolGuid = source.KolGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Serial = source.Serial;
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			destination.TafsilGuid4 = source.TafsilGuid4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\ViewSanadDetailMapper.cs", 106);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ViewSanadDetailBusiness> MapAsync(string contextGuid, WebViewSanadDetail source, ViewSanadDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ViewSanadDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.KolGuid = source.KolGuid;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.KolGuid = source.KolGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Serial = source.Serial;
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			destination.TafsilGuid4 = source.TafsilGuid4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\ViewSanadDetailMapper.cs", 153);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebViewSanadDetail>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailBusiness> source, List<WebViewSanadDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ViewSanadDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailBusiness> source, List<ViewSanadDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ViewSanadDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebViewSanadDetail> source, List<ViewSanadDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
