using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class TafsilNumberMapper : IItemAsyncMapper<TafsilNumberBusiness, WebTafsilNumbers>, IItemAsyncMapper<TafsilNumberBusiness, TafsilNumberBusiness>, IItemAsyncMapper<WebTafsilNumbers, TafsilNumberBusiness>, IItemAsyncMapper<WebTafsilNumberReport, TafsilNumberReportBusiness>, IItemAsyncMapper<TafsilNumberReportBusiness, WebTafsilNumberReport>
{
	public static TafsilNumberMapper Instance { get; private set; } = new TafsilNumberMapper();


	public Task<WebTafsilNumbers> MapAsync(string contextGuid, TafsilNumberBusiness source, WebTafsilNumbers destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsilNumbers();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Title = source.Title;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TafsilNumberMapper.cs", 38);
			throw;
		}
	}

	public Task<TafsilNumberBusiness> MapAsync(string contextGuid, WebTafsilNumbers source, TafsilNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilNumberBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Title = source.Title;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TafsilNumberMapper.cs", 61);
			throw;
		}
	}

	public Task<TafsilNumberBusiness> MapAsync(string contextGuid, TafsilNumberBusiness source, TafsilNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilNumberBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Guid = source.Guid;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Title = source.Title;
			destination.Prefix = source.Prefix;
			destination.CalcedNumber = source.CalcedNumber;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TafsilNumberMapper.cs", 87);
			throw;
		}
	}

	public Task<WebTafsilNumberReport> MapAsync(string contextGuid, TafsilNumberReportBusiness source, WebTafsilNumberReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsilNumberReport();
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.Guid = source.Guid;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Title = source.Title;
			destination.Name = source.Name;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TafsilNumberMapper.cs", 110);
			throw;
		}
	}

	public Task<TafsilNumberReportBusiness> MapAsync(string contextGuid, WebTafsilNumberReport source, TafsilNumberReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilNumberReportBusiness(contextGuid);
			}
			destination.TafsilGuid = source.TafsilGuid;
			destination.Guid = source.Guid;
			destination.OriginalNumber = source.OriginalNumber;
			destination.Title = source.Title;
			destination.Name = source.Name;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TafsilNumberMapper.cs", 133);
			throw;
		}
	}

	public Task<List<TafsilNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilNumberBusiness> source, List<TafsilNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTafsilNumbers>> MapListAsync(string contextGuid, IEnumerable<TafsilNumberBusiness> source, List<WebTafsilNumbers> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TafsilNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsilNumbers> source, List<TafsilNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTafsilNumberReport>> MapListAsync(string contextGuid, IEnumerable<TafsilNumberReportBusiness> source, List<WebTafsilNumberReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TafsilNumberReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsilNumberReport> source, List<TafsilNumberReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
