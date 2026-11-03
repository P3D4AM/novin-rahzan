using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class NaghzPersistenceRepository : PersistenceRepository<NaghzBusiness, Naghz>, INaghzRepository, IRepository<NaghzBusiness>, INoGuidRepository<NaghzBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<NaghzBusiness, Naghz>, IItemAsyncMapper<Naghz, NaghzBusiness>
	{
		public Task<Naghz> MapAsync(string contextGuid, NaghzBusiness source, Naghz destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs"))
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
						destination = new Naghz();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Message = source.Message;
					destination.Count = source.Count;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs", 95);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<NaghzBusiness> MapAsync(string contextGuid, Naghz source, NaghzBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs"))
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
						destination = new NaghzBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Message = source.Message;
					destination.Count = source.Count;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs", 123);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Naghz>> MapListAsync(string contextGuid, IEnumerable<NaghzBusiness> source, List<Naghz> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<NaghzBusiness>> MapListAsync(string contextGuid, IEnumerable<Naghz> source, List<NaghzBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Naghz, NaghzBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<NaghzBusiness, Naghz> MapToDatabase => Mapper;

	public override IItemMapper<Naghz, NaghzBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public NaghzPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<NaghzBusiness> GetAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs", 25));
			NaghzBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Naghz naghz = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Naghz>((IQueryable<Naghz>)((IQueryable<Naghz>)c.Naghz).OrderBy((Naghz q) => q.Count), token);
					if (naghz == null)
					{
						result = null;
					}
					else
					{
						naghz.Count++;
						((DbContext)c).Update<Naghz>(naghz);
						result = await MapFromDatabase.MapAsync(ContextGuid, naghz, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs", 38);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\NaghzPersistenceRepository.cs", 60);
		}
	}
}
