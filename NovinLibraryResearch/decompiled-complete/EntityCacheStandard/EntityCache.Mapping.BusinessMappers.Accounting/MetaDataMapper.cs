using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class MetaDataMapper : IItemAsyncMapper<MetaDataBusiness, WebMetaData>, IItemAsyncMapper<MetaDataBusiness, MetaDataBusiness>, IItemAsyncMapper<WebMetaData, MetaDataBusiness>
{
	public static MetaDataMapper Instance { get; private set; } = new MetaDataMapper();


	public Task<MetaDataBusiness> MapAsync(string contextGuid, MetaDataBusiness source, MetaDataBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MetaDataBusiness(contextGuid);
			}
			destination.DataType = source.DataType;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\MetaDataMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebMetaData> MapAsync(string contextGuid, MetaDataBusiness source, WebMetaData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebMetaData();
			}
			destination.DataType = source.DataType;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.Key = source.Key;
			destination.Value = source.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\MetaDataMapper.cs", 61);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MetaDataBusiness> MapAsync(string contextGuid, WebMetaData source, MetaDataBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MetaDataBusiness(contextGuid);
			}
			destination.DataType = source.DataType;
			destination.EntityGuid = source.EntityGuid;
			destination.EntityType = source.EntityType;
			destination.Key = source.Key;
			destination.Value = source.Value;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\MetaDataMapper.cs", 86);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<MetaDataBusiness>> MapListAsync(string contextGuid, IEnumerable<MetaDataBusiness> source, List<MetaDataBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebMetaData>> MapListAsync(string contextGuid, IEnumerable<MetaDataBusiness> source, List<WebMetaData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MetaDataBusiness>> MapListAsync(string contextGuid, IEnumerable<WebMetaData> source, List<MetaDataBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
