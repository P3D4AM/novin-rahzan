using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class StatePersistanceRepository : PersistenceRepository<StateBusiness, State>, IStateRepository, IRepository<StateBusiness>, INoGuidRepository<StateBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<StateBusiness, State>, IItemAsyncMapper<State, StateBusiness>
	{
		public Task<State> MapAsync(string contextGuid, StateBusiness source, State destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new State();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.StateName = source.StateName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs", 101);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<StateBusiness> MapAsync(string contextGuid, State source, StateBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new StateBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.StateName = source.StateName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs", 124);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<State>> MapListAsync(string contextGuid, IEnumerable<StateBusiness> source, List<State> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<StateBusiness>> MapListAsync(string contextGuid, IEnumerable<State> source, List<StateBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<State, StateBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<StateBusiness, State> MapToDatabase => Mapper;

	public override IItemMapper<State, StateBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public StatePersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<StateBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs", 29));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs"))
		{
			List<StateBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<State> source = (IQueryable<State>)c.States;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								source = source.Where((State p) => DbFunctionsExtensions.Like(EF.Functions, p.StateName, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							source = source.Where((State p) => p.Status == ((bool?)status).Value);
						}
						result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, source.ToList())) : null);
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs");
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
				dbContext = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\StatePersistanceRepository.cs", 70);
			throw;
		}
	}
}
