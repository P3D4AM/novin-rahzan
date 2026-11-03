using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Attendance;

public class AttDepartmentMapper : IItemAsyncMapper<AttDepartmentBusiness, WebAttDepartment>, IItemAsyncMapper<AttDepartmentBusiness, AttDepartmentBusiness>, IItemAsyncMapper<WebAttDepartment, AttDepartmentBusiness>
{
	public static AttDepartmentMapper Instance { get; private set; } = new AttDepartmentMapper();


	public Task<WebAttDepartment> MapAsync(string contextGuid, AttDepartmentBusiness source, WebAttDepartment destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAttDepartment();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.AdminGuid = source.AdminGuid;
			destination.CreatedDate = source.CreatedDate;
			destination.MetaData = source.MetaData;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttDepartmentMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<AttDepartmentBusiness> MapAsync(string contextGuid, AttDepartmentBusiness source, AttDepartmentBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AttDepartmentBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.AdminGuid = source.AdminGuid;
			destination.CreatedDate = source.CreatedDate;
			destination.MetaData = source.MetaData;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttDepartmentMapper.cs", 62);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<AttDepartmentBusiness> MapAsync(string contextGuid, WebAttDepartment source, AttDepartmentBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AttDepartmentBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.AdminGuid = source.AdminGuid;
			destination.CreatedDate = source.CreatedDate;
			destination.MetaData = source.MetaData;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Attendance\\AttDepartmentMapper.cs", 88);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebAttDepartment>> MapListAsync(string contextGuid, IEnumerable<AttDepartmentBusiness> source, List<WebAttDepartment> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AttDepartmentBusiness>> MapListAsync(string contextGuid, IEnumerable<AttDepartmentBusiness> source, List<AttDepartmentBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AttDepartmentBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAttDepartment> source, List<AttDepartmentBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
