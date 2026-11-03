using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class GardeshHesabMapper : IItemAsyncMapper<GardeshHesabBusiness, WebGardeshHesab>, IItemAsyncMapper<GardeshHesabBusiness, GardeshHesabBusiness>, IItemAsyncMapper<WebGardeshHesab, GardeshHesabBusiness>, IItemAsyncMapper<PacketParser.Entities.Accounting.GardeshHesab, GardeshHesabBusiness>, IItemAsyncMapper<GardeshHesabBusiness, GradeshReportBusiness>
{
	public static GardeshHesabMapper Instance { get; private set; } = new GardeshHesabMapper();


	public async Task<WebGardeshHesab> MapAsync(string contextGuid, GardeshHesabBusiness source, WebGardeshHesab destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebGardeshHesab();
			}
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			WebGardeshHesab webGardeshHesab = destination;
			webGardeshHesab.LstTafsiliItems = await new GardeshTafsil().MapListAsync(contextGuid, source?.LstTafsiliItems, new List<GardeshTafsil>(), tranName);
			destination.IsFactor = source.IsFactor;
			destination.AutoDescriptionShort = source.AutoDescriptionShort;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Sood = source.Sood;
			destination.Ischecked = source.Ischecked;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.depId = source.depId;
			destination.PrdInfo = source.PrdInfo;
			destination.DateMoaser = source.DateMoaser;
			destination.DateMoaserSh = source.DateMoaserSh;
			destination.Rem = source.Rem;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.UserName = source.UserName;
			destination.RemForDisplay = source.RemForDisplay;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.DescForDisplay = source.DescForDisplay;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.Tashkhis = source.Tashkhis;
			destination.TafsilItemName = source.TafsilItemName;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshHesabMapper.cs", 86);
			throw;
		}
	}

	public async Task<GardeshHesabBusiness> MapAsync(string contextGuid, PacketParser.Entities.Accounting.GardeshHesab source, GardeshHesabBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshHesabBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.LstTafsiliItems = source.LstTafsiliItems;
			destination.IsFactor = source.IsFactor;
			destination.AutoDescriptionShort = source.AutoDescriptionShort;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Sood = source.Sood;
			destination.Ischecked = source.Ischecked;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.depId = source.depId;
			destination.PrdInfo = source.PrdInfo;
			destination.Rem = source.Rem;
			destination.RemWithQest = source.RemWithQest;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.UserName = source.UserName;
			destination.RemForDisplay = source.RemForDisplay;
			destination.RemTashkhisForDisplay = source.RemTashkhisForDisplay;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.DiscountForDisplay = source.DiscountForDisplay;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.DescForDisplay = source.DescForDisplay;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.Tashkhis = source.Tashkhis;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			GardeshHesabBusiness gardeshHesabBusiness = destination;
			gardeshHesabBusiness.TafsiliItems = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, destination.LstTafsiliItems, new List<GardeshTafsilBusiness>(), tranName);
			destination.DalTernover = DalTurnover.Get(contextGuid, ClsCache.GetCache(contextGuid).Source);
			destination.IsEnteghalHesab = source.IsEnteghalHesab;
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshHesabMapper.cs", 159);
			throw;
		}
	}

	public async Task<GardeshHesabBusiness> MapAsync(string contextGuid, GardeshHesabBusiness source, GardeshHesabBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshHesabBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.LstTafsiliItems = source.LstTafsiliItems;
			destination.IsFactor = source.IsFactor;
			destination.AutoDescriptionShort = source.AutoDescriptionShort;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Sood = source.Sood;
			destination.Ischecked = source.Ischecked;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.depId = source.depId;
			destination.PrdInfo = source.PrdInfo;
			destination.DateMoaserSh = source.DateMoaserSh;
			destination.Rem = source.Rem;
			destination.RemWithQest = source.RemWithQest;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.UserName = source.UserName;
			destination.RemForDisplay = source.RemForDisplay;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.DiscountForDisplay = source.DiscountForDisplay;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.DescForDisplay = source.DescForDisplay;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.Tashkhis = source.Tashkhis;
			destination.TafsilItemName = source.TafsilItemName;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			GardeshHesabBusiness gardeshHesabBusiness = destination;
			gardeshHesabBusiness.TafsiliItems = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, destination.LstTafsiliItems, new List<GardeshTafsilBusiness>(), tranName);
			destination.DalTernover = DalTurnover.Get(contextGuid, ClsCache.GetCache(contextGuid).Source);
			destination.IsEnteghalHesab = source.IsEnteghalHesab;
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshHesabMapper.cs", 233);
			throw;
		}
	}

	public async Task<GardeshHesabBusiness> MapAsync(string contextGuid, WebGardeshHesab source, GardeshHesabBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new GardeshHesabBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Date = source.Date;
			destination.DateSh = source.DateSh;
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.LstTafsiliItems = source.LstTafsiliItems?.ToList();
			destination.IsFactor = source.IsFactor;
			destination.AutoDescriptionShort = source.AutoDescriptionShort;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.Sood = source.Sood;
			destination.Ischecked = source.Ischecked;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Id2 = source.Id2;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Bazaryab = source.Bazaryab;
			destination.VGuid = source.VGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.SanadDesc = source.SanadDesc;
			destination.depId = source.depId;
			destination.PrdInfo = source.PrdInfo;
			destination.DateMoaserSh = source.DateMoaserSh;
			destination.Rem = source.Rem;
			destination.DecimalGuid = source.DecimalGuid;
			destination.RowDesc = source.RowDesc;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.KolGuid = source.KolGuid;
			destination.UGuid = source.UGuid;
			destination.UserName = source.UserName;
			destination.RemForDisplay = source.RemForDisplay;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.DiscountForDisplay = source.DiscountForDisplay;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.DescForDisplay = source.DescForDisplay;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.Tashkhis = source.Tashkhis;
			destination.TafsilItemName = source.TafsilItemName;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			GardeshHesabBusiness gardeshHesabBusiness = destination;
			gardeshHesabBusiness.TafsiliItems = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, destination.LstTafsiliItems, new List<GardeshTafsilBusiness>(), tranName);
			destination.DalTernover = DalTurnover.Get(contextGuid, ClsCache.GetCache(contextGuid).Source);
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshHesabMapper.cs", 305);
			throw;
		}
	}

	public Task<GradeshReportBusiness> MapAsync(string contextGuid, GardeshHesabBusiness source, GradeshReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.DescForDisplay = source.AutoDescriptionShort;
			destination.DateMoaser = source.DateMoaser;
			destination.RemForDisplay = source.RemForDisplay;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.UGuid = source.UGuid;
			destination.Tashkhis = source.RemTashkhisForDisplay;
			destination.TafsilItemName = source.TafsilItemName;
			destination.TafsilItemCode = source.TafsilItemCode;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.IsChecked = source.Ischecked;
			return Task.FromResult(destination);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\GardeshHesabMapper.cs", 348);
			throw;
		}
	}

	public Task<List<GardeshHesabBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGardeshHesab> source, List<GardeshHesabBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshHesabBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshHesabBusiness> source, List<GardeshHesabBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebGardeshHesab>> MapListAsync(string contextGuid, IEnumerable<GardeshHesabBusiness> source, List<WebGardeshHesab> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GardeshHesabBusiness>> MapListAsync(string contextGuid, IEnumerable<PacketParser.Entities.Accounting.GardeshHesab> source, List<GardeshHesabBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<GradeshReportBusiness>> MapListAsync(string contextGuid, IEnumerable<GardeshHesabBusiness> source, List<GradeshReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
