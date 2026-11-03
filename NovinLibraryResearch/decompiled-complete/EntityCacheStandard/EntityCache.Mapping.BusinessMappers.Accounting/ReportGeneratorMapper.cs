using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ReportGeneratorMapper : IItemAsyncMapper<ReportGeneratorBusiness, WebReportGenerator>, IItemAsyncMapper<ReportGeneratorBusiness, ReportGeneratorBusiness>, IItemAsyncMapper<WebReportGenerator, ReportGeneratorBusiness>
{
	public static ReportGeneratorMapper Instance { get; private set; } = new ReportGeneratorMapper();


	public Task<WebReportGenerator> MapAsync(string contextGuid, ReportGeneratorBusiness source, WebReportGenerator destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReportGenerator();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Description = source.Description;
			destination.Rpt = source.Rpt;
			destination.Version = source.Version;
			destination.LastUpdate = source.LastUpdate;
			destination.Type = source.Type;
			destination.Document = source.Document;
			destination.MaliYearSanadType = source.MaliYearSanadType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorMapper.cs", 42);
			throw;
		}
	}

	public async Task<ReportGeneratorBusiness> MapAsync(string contextGuid, ReportGeneratorBusiness source, ReportGeneratorBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReportGeneratorBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Description = source.Description;
			destination.Rpt = source.Rpt;
			destination.Version = source.Version;
			destination.LastUpdate = source.LastUpdate;
			destination.Type = source.Type;
			destination.Document = source.Document;
			destination.MaliYearSanadType = source.MaliYearSanadType;
			ReportGeneratorBusiness reportGeneratorBusiness = destination;
			reportGeneratorBusiness.DetList = await ReportGeneratorDetMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<ReportGeneratorDetBusiness>(), tranName, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorMapper.cs", 72);
			throw;
		}
	}

	public Task<ReportGeneratorBusiness> MapAsync(string contextGuid, WebReportGenerator source, ReportGeneratorBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReportGeneratorBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Title = source.Title;
			destination.Description = source.Description;
			destination.Rpt = source.Rpt;
			destination.Version = source.Version;
			destination.LastUpdate = source.LastUpdate;
			destination.Type = source.Type;
			destination.Document = source.Document;
			destination.ContextGuid = contextGuid;
			destination.MaliYearSanadType = source.MaliYearSanadType;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorMapper.cs", 101);
			throw;
		}
	}

	public Task<List<WebReportGenerator>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorBusiness> source, List<WebReportGenerator> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ReportGeneratorBusiness>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorBusiness> source, List<ReportGeneratorBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ReportGeneratorBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReportGenerator> source, List<ReportGeneratorBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
