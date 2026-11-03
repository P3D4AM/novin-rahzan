using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class TafsilSelectedDetListMapper : IItemAsyncMapper<TafsilSelectedDetListBusiness, WebTafsilSelectedDetList>, IItemAsyncMapper<TafsilSelectedDetListBusiness, TafsilSelectedDetListBusiness>, IItemAsyncMapper<WebTafsilSelectedDetList, TafsilSelectedDetListBusiness>
{
	public static TafsilSelectedDetListMapper Instance { get; private set; } = new TafsilSelectedDetListMapper();


	public async Task<WebTafsilSelectedDetList> MapAsync(string contextGuid, TafsilSelectedDetListBusiness source, WebTafsilSelectedDetList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsilSelectedDetList();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.HGuid = source.HGuid;
			destination.HType = source.HType;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListMapper.cs", 37);
			throw;
		}
		return destination;
	}

	public async Task<TafsilSelectedDetListBusiness> MapAsync(string contextGuid, TafsilSelectedDetListBusiness source, TafsilSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilSelectedDetListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetHGuidAsync(source.HGuid, EnSystem.Mapper, token, tranName);
			destination.HType = source.HType;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListMapper.cs", 63);
			throw;
		}
		return destination;
	}

	public async Task<List<TafsilSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedDetListBusiness> source, List<TafsilSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return await source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<List<WebTafsilSelectedDetList>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedDetListBusiness> source, List<WebTafsilSelectedDetList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return await source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TafsilSelectedDetListBusiness> MapAsync(string contextGuid, WebTafsilSelectedDetList source, TafsilSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilSelectedDetListBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetHGuidAsync(source.HGuid, EnSystem.Mapper, token, tranName);
			destination.HType = source.HType;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListMapper.cs", 92);
			throw;
		}
		return destination;
	}

	public Task<List<TafsilSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsilSelectedDetList> source, List<TafsilSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
