using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class GardeshTafsilMapper : IItemAsyncMapper<GardeshTafsilBusiness, WebGardeshTafsili>, IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsilBusiness>, IItemAsyncMapper<WebGardeshTafsili, GardeshTafsilBusiness>, IItemAsyncMapper<GardeshTafsil, GardeshTafsilBusiness>, IItemAsyncMapper<GardeshTafsilBusiness, GardeshTafsil>, IItemAsyncMapper<WebGardeshTafsiliCompact, GardeshTafsil>, IItemAsyncMapper<GardeshTafsilBusiness, WebGardeshTafsiliCompact>, IItemAsyncMapper<WebGardeshTafsiliCompact, GardeshTafsilBusiness>, IItemAsyncMapper<GardeshTafsilBusiness, GradeshReportBusiness>
{
	public static GardeshTafsilMapper Instance { get; private set; } = new GardeshTafsilMapper();


	public Task<WebGardeshTafsiliCompact> MapAsync(string contextGuid, GardeshTafsilBusiness source, WebGardeshTafsiliCompact destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGardeshTafsiliCompact();
			}
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.GetPrdGuid();
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 73);
			throw;
		}
	}

	public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, WebGardeshTafsiliCompact source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new GardeshTafsilBusiness(contextGuid);
			}
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper, setProduct: false);
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			destination.ContextGuid = contextGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 127);
			throw;
		}
	}

	public Task<WebGardeshTafsili> MapAsync(string contextGuid, GardeshTafsilBusiness source, WebGardeshTafsili destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGardeshTafsili();
			}
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.GetPrdGuid();
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DescForDisplay = source.DescForDisplay;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 182);
			throw;
		}
	}

	public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, GardeshTafsilBusiness source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new GardeshTafsilBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			await destination.SetPrdGuidAsync(source.GetPrdGuid(), EnSystem.Mapper, setProduct: false);
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalRemQt = source.DecimalRemQt;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			destination.Sood = source.Sood;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 238);
			throw;
		}
	}

	public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, WebGardeshTafsili source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new GardeshTafsilBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper, setProduct: false);
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 292);
			throw;
		}
	}

	public async Task<GardeshTafsilBusiness> MapAsync(string contextGuid, GardeshTafsil source, GardeshTafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new GardeshTafsilBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper, setProduct: false);
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			destination.Sood = source.Sood;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 347);
			throw;
		}
	}

	public Task<GardeshTafsil> MapAsync(string contextGuid, GardeshTafsilBusiness source, GardeshTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshTafsil();
			}
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.GetPrdGuid();
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			destination.IsChecked = source.IsChecked;
			destination.Sood = source.Sood;
			destination.IsEnteghalHesab = source.IsEnteghalHesab;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 403);
			throw;
		}
	}

	public Task<GardeshTafsil> MapAsync(string contextGuid, WebGardeshTafsiliCompact source, GardeshTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshTafsil();
			}
			destination.Date = source.Date;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.EnAsType = source.EnAsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.DateMoaser = source.DateMoaser;
			destination.Rem = source.Rem;
			destination.RowIndex = source.RowIndex;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalQt = source.DecimalQt;
			destination.DetailGuid = source.DetailGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 456);
			throw;
		}
	}

	public Task<GradeshReportBusiness> MapAsync(string contextGuid, GardeshTafsilBusiness source, GradeshReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GradeshReportBusiness();
			}
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.AsGuid = source.AsGuid;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Sood = source.Sood;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.SanadDesc = source.SanadDesc;
			destination.DescForDisplay = source.DescForDisplay;
			destination.DateMoaser = source.DateMoaser;
			destination.RemForDisplay = source.RemForDisplay;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.UGuid = source.UGuid;
			destination.Tashkhis = source.Tashkhis;
			destination.TafsilItemName = source.TafsilItemName;
			destination.TafsilItemCode = source.TafsilItemCode;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.IsChecked = source.IsChecked;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshTafsilMapper.cs", 498);
			throw;
		}
	}

	public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsil> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshTafsil>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<GardeshTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshTafsil>> MapListAsync(string contextGuid, IEnumerable<WebGardeshTafsiliCompact> source, List<GardeshTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGardeshTafsili> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebGardeshTafsili>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<WebGardeshTafsili> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshTafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGardeshTafsiliCompact> source, List<GardeshTafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebGardeshTafsiliCompact>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<WebGardeshTafsiliCompact> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GradeshReportBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshTafsilBusiness> source, List<GradeshReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
