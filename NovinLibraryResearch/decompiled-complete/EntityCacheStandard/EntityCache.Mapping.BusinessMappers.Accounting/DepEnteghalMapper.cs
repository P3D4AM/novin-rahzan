using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class DepEnteghalMapper : IItemAsyncMapper<DepEnteghalBusiness, WebDepEnteghal>, IItemAsyncMapper<DepEnteghalBusiness, DepEnteghalBusiness>, IItemAsyncMapper<WebDepEnteghal, DepEnteghalBusiness>
{
	public static DepEnteghalMapper Instance { get; private set; } = new DepEnteghalMapper();


	public Task<DepEnteghalBusiness> MapAsync(string contextGuid, WebDepEnteghal source, DepEnteghalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepEnteghalBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.UserGuid = source.UserGuid;
			destination.No = source.No;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ModifiedM = source.ModifiedM;
			destination.MashinNo = source.MashinNo;
			destination.GirandeName = source.GirandeName;
			destination.RanandehName = source.RanandehName;
			destination.StatusId = source.StatusId;
			destination.DebitMoeinGuid = source.DebitMoeinGuid;
			destination.DebitTafsilGuid = source.DebitTafsilGuid;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.SanadNo = source.SanadNo;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalMapper.cs", 49);
			throw;
		}
	}

	public async Task<DepEnteghalBusiness> MapAsync(string contextGuid, DepEnteghalBusiness source, DepEnteghalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new DepEnteghalBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.UserGuid = source.UserGuid;
			destination.No = source.No;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ModifiedM = source.ModifiedM;
			destination.MashinNo = source.MashinNo;
			destination.GirandeName = source.GirandeName;
			destination.RanandehName = source.RanandehName;
			destination.StatusId = source.StatusId;
			DepEnteghalBusiness depEnteghalBusiness = destination;
			depEnteghalBusiness.DebitMoeinGuid = await source.GetDebitMoeinGuidAsync();
			depEnteghalBusiness = destination;
			depEnteghalBusiness.DebitTafsilGuid = await source.GetDebitTafsilGuidAsync();
			depEnteghalBusiness = destination;
			depEnteghalBusiness.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync();
			depEnteghalBusiness = destination;
			depEnteghalBusiness.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync();
			destination.SanadNo = source.SanadNo;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalMapper.cs", 84);
			throw;
		}
	}

	public async Task<WebDepEnteghal> MapAsync(string contextGuid, DepEnteghalBusiness source, WebDepEnteghal destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebDepEnteghal();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.UserGuid = source.UserGuid;
			destination.No = source.No;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.ModifiedM = source.ModifiedM;
			destination.MashinNo = source.MashinNo;
			destination.GirandeName = source.GirandeName;
			destination.RanandehName = source.RanandehName;
			destination.StatusId = source.StatusId;
			WebDepEnteghal webDepEnteghal = destination;
			webDepEnteghal.DebitMoeinGuid = await source.GetDebitMoeinGuidAsync();
			webDepEnteghal = destination;
			webDepEnteghal.DebitTafsilGuid = await source.GetDebitTafsilGuidAsync();
			webDepEnteghal = destination;
			webDepEnteghal.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync();
			webDepEnteghal = destination;
			webDepEnteghal.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync();
			destination.SanadNo = source.SanadNo;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalMapper.cs", 118);
			throw;
		}
	}

	public Task<List<DepEnteghalBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDepEnteghal> source, List<DepEnteghalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DepEnteghalBusiness>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalBusiness> source, List<DepEnteghalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDepEnteghal>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalBusiness> source, List<WebDepEnteghal> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
