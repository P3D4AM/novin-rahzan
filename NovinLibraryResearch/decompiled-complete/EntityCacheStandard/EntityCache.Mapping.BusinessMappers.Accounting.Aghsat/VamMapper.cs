using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;

public class VamMapper : IItemAsyncMapper<VamBusiness, WebVam>, IItemAsyncMapper<VamBusiness, VamBusiness>, IItemAsyncMapper<WebVam, VamBusiness>
{
	public static VamMapper Instance { get; private set; } = new VamMapper();


	public Task<WebVam> MapAsync(string contextGuid, VamBusiness source, WebVam destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebVam();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			destination.Id2 = source.Id2;
			destination.Zarib = source.Zarib;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.VamMoeinGuid = source.VamMoeinGuid;
			destination.VamTafsilGuid = source.VamTafsilGuid;
			destination.SepordeTafsilGuid = source.SepordeTafsilGuid;
			destination.SepordeMoeinGuid = source.SepordeMoeinGuid;
			destination.Status = source.Status;
			destination.AghsatGuid = source.AghsatGuid;
			destination.Sum = source.Sum;
			destination.ZamenGuid1 = source.ZamenGuid1;
			destination.ZamenGuid2 = source.ZamenGuid2;
			destination.ZamenGuid3 = source.ZamenGuid3;
			destination.ZamenGuid4 = source.ZamenGuid4;
			destination.Average = source.Average;
			destination.AverageSum = source.AverageSum;
			destination.AverageType = source.AverageType;
			destination.ProfitPercent = source.ProfitPercent;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\VamMapper.cs", 54);
			throw;
		}
	}

	public Task<VamBusiness> MapAsync(string contextGuid, VamBusiness source, VamBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VamBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			destination.Id2 = source.Id2;
			destination.Zarib = source.Zarib;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.VamMoeinGuid = source.VamMoeinGuid;
			destination.VamTafsilGuid = source.VamTafsilGuid;
			destination.SepordeTafsilGuid = source.SepordeTafsilGuid;
			destination.SepordeMoeinGuid = source.SepordeMoeinGuid;
			destination.Status = source.Status;
			destination.AghsatGuid = source.AghsatGuid;
			destination.Sum = source.Sum;
			destination.ZamenGuid1 = source.ZamenGuid1;
			destination.ZamenGuid2 = source.ZamenGuid2;
			destination.ZamenGuid3 = source.ZamenGuid3;
			destination.ZamenGuid4 = source.ZamenGuid4;
			destination.Average = source.Average;
			destination.AverageSum = source.AverageSum;
			destination.AverageType = source.AverageType;
			destination.ProfitPercent = source.ProfitPercent;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\VamMapper.cs", 97);
			throw;
		}
	}

	public Task<VamBusiness> MapAsync(string contextGuid, WebVam source, VamBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VamBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date1 = source.Date1;
			destination.Date2 = source.Date2;
			destination.Id2 = source.Id2;
			destination.Zarib = source.Zarib;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.VamMoeinGuid = source.VamMoeinGuid;
			destination.VamTafsilGuid = source.VamTafsilGuid;
			destination.SepordeTafsilGuid = source.SepordeTafsilGuid;
			destination.SepordeMoeinGuid = source.SepordeMoeinGuid;
			destination.Status = source.Status;
			destination.AghsatGuid = source.AghsatGuid;
			destination.Sum = source.Sum;
			destination.ZamenGuid1 = source.ZamenGuid1;
			destination.ZamenGuid2 = source.ZamenGuid2;
			destination.ZamenGuid3 = source.ZamenGuid3;
			destination.ZamenGuid4 = source.ZamenGuid4;
			destination.Average = source.Average;
			destination.AverageSum = source.AverageSum;
			destination.AverageType = source.AverageType;
			destination.ProfitPercent = source.ProfitPercent;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\VamMapper.cs", 140);
			throw;
		}
	}

	public Task<List<VamBusiness>> MapListAsync(string contextGuid, IEnumerable<VamBusiness> source, List<VamBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<VamBusiness>> MapListAsync(string contextGuid, IEnumerable<WebVam> source, List<VamBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebVam>> MapListAsync(string contextGuid, IEnumerable<VamBusiness> source, List<WebVam> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
