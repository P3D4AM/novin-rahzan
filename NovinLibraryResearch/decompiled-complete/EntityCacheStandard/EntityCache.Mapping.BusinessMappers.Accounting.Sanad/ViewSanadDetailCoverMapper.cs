using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Sanad;

public class ViewSanadDetailCoverMapper : IItemAsyncMapper<ViewSanadDetailCoverBusiness, ViewSanadDetailCoverBusiness>
{
	public static ViewSanadDetailCoverMapper Instance { get; private set; } = new ViewSanadDetailCoverMapper();


	public Task<ViewSanadDetailCoverBusiness> MapAsync(string contextGuid, ViewSanadDetailCoverBusiness source, ViewSanadDetailCoverBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ViewSanadDetailCoverBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Qt = source.Qt;
			destination.Serial = source.Serial;
			destination.AsType = source.AsType;
			destination.AsGuid = source.AsGuid;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.Date = source.Date;
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.Id2 = source.Id2;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.Price = source.Price;
			destination.Qt2 = source.Qt2;
			destination.RDiscount = source.RDiscount;
			destination.RBazaryab = source.RBazaryab;
			destination.SanadDesc = source.SanadDesc;
			destination.RDesc = source.RDesc;
			destination.ProductGuid = source.ProductGuid;
			destination.DepotGuid = source.DepotGuid;
			destination.VahedGuid = source.VahedGuid;
			destination.DateMoaser = source.DateMoaser;
			destination.DecimalQt = source.DecimalQt;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DetailGuid = source.DetailGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.AsTypeDocument = source.AsTypeDocument;
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid2 = source.TafsilGuid2;
			destination.TafsilGuid3 = source.TafsilGuid3;
			destination.TafsilGuid4 = source.TafsilGuid4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\ViewSanadDetailCoverMapper.cs", 66);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ViewSanadDetailCoverBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewSanadDetailCoverBusiness> source, List<ViewSanadDetailCoverBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
