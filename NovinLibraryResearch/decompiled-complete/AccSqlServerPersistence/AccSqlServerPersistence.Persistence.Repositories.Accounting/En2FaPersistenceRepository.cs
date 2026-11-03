using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

internal class En2FaPersistenceRepository : PersistenceRepository<En2FaBusiness, En2Fa>, IEn2FaRepositpry, IRepository<En2FaBusiness>, INoGuidRepository<En2FaBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<En2FaBusiness, En2Fa>, IItemAsyncMapper<En2Fa, En2FaBusiness>
	{
		public Task<En2Fa> MapAsync(string contextGuid, En2FaBusiness source, En2Fa destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs"))
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
						destination = new En2Fa();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.LName = source.LName;
					destination.FName = source.FName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs", 91);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<En2FaBusiness> MapAsync(string contextGuid, En2Fa source, En2FaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs"))
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
						destination = new En2FaBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.LName = source.LName;
					destination.FName = source.FName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs", 119);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<En2Fa>> MapListAsync(string contextGuid, IEnumerable<En2FaBusiness> source, List<En2Fa> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<En2FaBusiness>> MapListAsync(string contextGuid, IEnumerable<En2Fa> source, List<En2FaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<En2Fa, En2FaBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<En2FaBusiness, En2Fa> MapToDatabase => Mapper;

	public override IItemMapper<En2Fa, En2FaBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public En2FaPersistenceRepository(ContextHolder db)
		: base(db)
	{
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs", 35);
		}
	}

	public async Task<string> ConvertFa(string en, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ConvertFa", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ConvertFa", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs", 42));
			string result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<En2Fa>((IQueryable<En2Fa>)c.En2Fa, (Expression<Func<En2Fa, bool>>)((En2Fa x) => x.LName == en), token))?.FName ?? "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "ConvertFa", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs", 52);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("ConvertFa", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\En2FaPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
