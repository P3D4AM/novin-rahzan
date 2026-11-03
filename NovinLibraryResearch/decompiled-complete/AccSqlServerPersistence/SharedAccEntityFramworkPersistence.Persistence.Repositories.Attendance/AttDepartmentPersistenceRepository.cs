using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Attendance;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData.Attendance;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class AttDepartmentPersistenceRepository : PersistenceSyncRepository<AttDepartmentBusiness, AttDepartment>, IAttDepartmentRepository, ISyncRepository<AttDepartmentBusiness>, IEditableDelivery, IRepository<AttDepartmentBusiness>, INoGuidRepository<AttDepartmentBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AttDepartmentBusiness, AttDepartment>, IItemAsyncMapper<AttDepartment, AttDepartmentBusiness>
	{
		public Task<AttDepartment> MapAsync(string contextGuid, AttDepartmentBusiness source, AttDepartment destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AttDepartment();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.AdminGuid = source.AdminGuid;
				destination.CreatedDate = source.CreatedDate;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				if (source.MetaData != null)
				{
					destination.MetaData = Json.ToStringJson(source.MetaData);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 95);
			}
			return Task.FromResult(destination);
		}

		public Task<AttDepartmentBusiness> MapAsync(string contextGuid, AttDepartment source, AttDepartmentBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<AttDepartmentMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 122);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 124);
			}
			return Task.FromResult(destination);
		}

		public Task<List<AttDepartment>> MapListAsync(string contextGuid, IEnumerable<AttDepartmentBusiness> source, List<AttDepartment> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AttDepartmentBusiness>> MapListAsync(string contextGuid, IEnumerable<AttDepartment> source, List<AttDepartmentBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<AttDepartment, AttDepartmentBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<AttDepartment, AttDepartmentBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AttDepartmentBusiness, AttDepartment> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public AttDepartmentPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 44);
		}
	}

	public async Task<AttDepartmentBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 48));
		AttDepartmentBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				AttDepartment attDepartment = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AttDepartment>((IQueryable<AttDepartment>)c.AttDepartments, (Expression<Func<AttDepartment, bool>>)((AttDepartment q) => q.Name == name), default(CancellationToken));
				result = ((attDepartment != null) ? (await MapFromDatabase.MapAsync(ContextGuid, attDepartment, null, tranName, token)) : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs", 59);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\AttDepartmentPersistenceRepository.cs");
			}
		}
		return result;
	}
}
