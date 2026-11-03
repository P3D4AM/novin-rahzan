using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class ReportGeneratorDetMapper : IItemAsyncMapper<ReportGeneratorDetBusiness, WebReportGeneratorDet>, IItemAsyncMapper<ReportGeneratorDetBusiness, ReportGeneratorDetBusiness>, IItemAsyncMapper<WebReportGeneratorDet, ReportGeneratorDetBusiness>
{
	public static ReportGeneratorDetMapper Instance { get; private set; } = new ReportGeneratorDetMapper();


	public Task<WebReportGeneratorDet> MapAsync(string contextGuid, ReportGeneratorDetBusiness source, WebReportGeneratorDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReportGeneratorDet();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParameterName = source.ParameterName;
			destination.OrderIndex = source.OrderIndex;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorDetMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ReportGeneratorDetBusiness> MapAsync(string contextGuid, ReportGeneratorDetBusiness source, ReportGeneratorDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReportGeneratorDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParameterName = source.ParameterName;
			destination.OrderIndex = source.OrderIndex;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorDetMapper.cs", 62);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<ReportGeneratorDetBusiness> MapAsync(string contextGuid, WebReportGeneratorDet source, ReportGeneratorDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReportGeneratorDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ParameterName = source.ParameterName;
			destination.OrderIndex = source.OrderIndex;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\ReportGeneratorDetMapper.cs", 87);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebReportGeneratorDet>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorDetBusiness> source, List<WebReportGeneratorDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ReportGeneratorDetBusiness>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorDetBusiness> source, List<ReportGeneratorDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ReportGeneratorDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReportGeneratorDet> source, List<ReportGeneratorDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
