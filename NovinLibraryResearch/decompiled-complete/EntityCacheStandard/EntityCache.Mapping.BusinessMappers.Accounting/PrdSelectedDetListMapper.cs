using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdSelectedDetListMapper : IItemAsyncMapper<PrdSelectedDetListBusiness, WebPrdSelectedDetList>, IItemAsyncMapper<PrdSelectedDetListBusiness, PrdSelectedDetListBusiness>, IItemAsyncMapper<WebPrdSelectedDetList, PrdSelectedDetListBusiness>
{
	public static PrdSelectedDetListMapper Instance { get; private set; } = new PrdSelectedDetListMapper();


	public async Task<PrdSelectedDetListBusiness> MapAsync(string contextGuid, PrdSelectedDetListBusiness source, PrdSelectedDetListBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSelectedDetListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetPrdGuid(source.GetPrdGuid(), EnSystem.Mapper);
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedDetListMapper.cs", 39);
			throw;
		}
		return destination;
	}

	public Task<WebPrdSelectedDetList> MapAsync(string contextGuid, PrdSelectedDetListBusiness source, WebPrdSelectedDetList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdSelectedDetList();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.PrdGuid = source.GetPrdGuid();
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedDetListMapper.cs", 63);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<PrdSelectedDetListBusiness> MapAsync(string contextGuid, WebPrdSelectedDetList source, PrdSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSelectedDetListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetPrdGuid(source.PrdGuid, EnSystem.Mapper);
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Product\\PrdSelectedDetListMapper.cs", 88);
			throw;
		}
		return destination;
	}

	public Task<List<WebPrdSelectedDetList>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedDetListBusiness> source, List<WebPrdSelectedDetList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedDetListBusiness> source, List<PrdSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdSelectedDetList> source, List<PrdSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
