using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class TableRelationShipMapper : IItemAsyncMapper<TableRelationShipBusiness, WebTableRelationShip>, IItemAsyncMapper<TableRelationShipBusiness, TableRelationShipBusiness>, IItemAsyncMapper<WebTableRelationShip, TableRelationShipBusiness>
{
	public static TableRelationShipMapper Instance { get; private set; } = new TableRelationShipMapper();


	public Task<WebTableRelationShip> MapAsync(string contextGuid, TableRelationShipBusiness source, WebTableRelationShip destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTableRelationShip();
			}
			destination.Guid = source.Guid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Price = source.Price;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\TableRelationShipMapper.cs", 34);
			throw;
		}
	}

	public Task<TableRelationShipBusiness> MapAsync(string contextGuid, TableRelationShipBusiness source, TableRelationShipBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TableRelationShipBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Price = source.Price;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\TableRelationShipMapper.cs", 57);
			throw;
		}
	}

	public Task<TableRelationShipBusiness> MapAsync(string contextGuid, WebTableRelationShip source, TableRelationShipBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TableRelationShipBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.Price = source.Price;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\TableRelationShipMapper.cs", 80);
			throw;
		}
	}

	public Task<List<TableRelationShipBusiness>> MapListAsync(string contextGuid, IEnumerable<TableRelationShipBusiness> source, List<TableRelationShipBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTableRelationShip>> MapListAsync(string contextGuid, IEnumerable<TableRelationShipBusiness> source, List<WebTableRelationShip> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TableRelationShipBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTableRelationShip> source, List<TableRelationShipBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
