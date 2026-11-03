using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class HallPersistenceRepository : PersistenceRepository<HallBusiness, Hall>, IHallRepository, IRepository<HallBusiness>, INoGuidRepository<HallBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<HallBusiness, Hall>, IItemAsyncMapper<Hall, HallBusiness>
	{
		public Task<Hall> MapAsync(string contextGuid, HallBusiness source, Hall destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Hall();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Title = source.Title;
				destination.MasterGuid = source.MasterGuid;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 116);
				throw;
			}
		}

		public Task<HallBusiness> MapAsync(string contextGuid, Hall source, HallBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new HallBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Title = source.Title;
				destination.MasterGuid = source.MasterGuid;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 142);
				throw;
			}
		}

		public Task<List<Hall>> MapListAsync(string contextGuid, IEnumerable<HallBusiness> source, List<Hall> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<HallBusiness>> MapListAsync(string contextGuid, IEnumerable<Hall> source, List<HallBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Hall, HallBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Hall, HallBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<HallBusiness, Hall> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public HallPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 42);
		}
	}

	public async Task<HallBusiness> GetAsync(string title, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 46));
		HallBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Hall hall = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Hall>((IQueryable<Hall>)c.Hall, (Expression<Func<Hall, bool>>)((Hall q) => q.Title == title), token);
				result = ((hall != null) ? (await MapFromDatabase.MapAsync(ContextGuid, hall, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 58);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<HallBusiness>> GetAllAsync(HallFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 69));
		IEnumerable<HallBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<Hall> queryable = (IQueryable<Hall>)c.Hall;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if (filter?.Status.HasValue ?? false)
					{
						queryable = queryable?.Where((Hall q) => q.Status == filter.Status.Value);
					}
					if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((Hall q) => q.MasterGuid == filter.MasterGuid.Value);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs", 83);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\HallPersistenceRepository.cs");
			}
		}
		return result;
	}
}
