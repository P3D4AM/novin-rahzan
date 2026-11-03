using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Calling;

public class CallingLineMapper : IItemAsyncMapper<CallingLineBusiness, WebCallingLine>, IItemAsyncMapper<CallingLineBusiness, CallingLineBusiness>, IItemAsyncMapper<WebCallingLine, CallingLineBusiness>
{
	public static CallingLineMapper Instance { get; private set; } = new CallingLineMapper();


	public Task<WebCallingLine> MapAsync(string contextGuid, CallingLineBusiness source, WebCallingLine destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCallingLine();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ComputerName = source.ComputerName;
			destination.DeviceNumber = source.DeviceNumber;
			destination.LineNumber = source.LineNumber;
			destination.Number = source.Number;
			destination.IsCo = source.IsCo;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLineMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCallingLine>> MapListAsync(string contextGuid, IEnumerable<CallingLineBusiness> source, List<WebCallingLine> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<CallingLineBusiness> MapAsync(string contextGuid, CallingLineBusiness source, CallingLineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = await CallingLineBusiness.FactoryAsync(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ComputerName = source.ComputerName;
			destination.DeviceNumber = source.DeviceNumber;
			destination.LineNumber = source.LineNumber;
			destination.Number = source.Number;
			destination.IsCo = source.IsCo;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLineMapper.cs", 72);
			throw;
		}
		return destination;
	}

	public Task<List<CallingLineBusiness>> MapListAsync(string contextGuid, IEnumerable<CallingLineBusiness> source, List<CallingLineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<CallingLineBusiness> MapAsync(string contextGuid, WebCallingLine source, CallingLineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = await CallingLineBusiness.FactoryAsync(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ComputerName = source.ComputerName;
			destination.DeviceNumber = source.DeviceNumber;
			destination.LineNumber = source.LineNumber;
			destination.Number = source.Number;
			destination.IsCo = source.IsCo;
			destination.CashierGuid = source.CashierGuid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Crm\\Calling\\CallingLineMapper.cs", 105);
			throw;
		}
		return destination;
	}

	public Task<List<CallingLineBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCallingLine> source, List<CallingLineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
