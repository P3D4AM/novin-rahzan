using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

internal class CardTargetRelationPersistenceRepository : PersistenceNoGuidRepository<CardTargetRelationBusiness, CardTargetRelation>, ICardTargetRelationRepository, INoGuidRepository<CardTargetRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CardTargetRelationBusiness, CardTargetRelation>, IItemAsyncMapper<CardTargetRelation, CardTargetRelationBusiness>
	{
		public Task<CardTargetRelation> MapAsync(string contextGuid, CardTargetRelationBusiness source, CardTargetRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CardTargetRelation();
				}
				destination.CardGuid = source.CardGuid;
				destination.TargetGuid = source.TargetGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 117);
				throw;
			}
		}

		public Task<CardTargetRelationBusiness> MapAsync(string contextGuid, CardTargetRelation source, CardTargetRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CardTargetRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.CardGuid = source.CardGuid;
				destination.TargetGuid = source.TargetGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 140);
				throw;
			}
		}

		public Task<List<CardTargetRelation>> MapListAsync(string contextGuid, IEnumerable<CardTargetRelationBusiness> source, List<CardTargetRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CardTargetRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CardTargetRelation> source, List<CardTargetRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CardTargetRelation, CardTargetRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CardTargetRelationBusiness, CardTargetRelation> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public CardTargetRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(CardTargetRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CardTargetRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<IEnumerable<CardTargetRelationBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CardTargetRelationBusiness> items, Guid masterGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(items?.Any() ?? false))
				{
					return res;
				}
				List<CardTargetRelation> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 41);
				foreach (CardTargetRelation item in des)
				{
					item.CardGuid = masterGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.CardGuid, item.TargetGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 45);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 51);
				res.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 52);
			}
			return res;
		}
	}

	public async Task GetAllAsync(CardBusiness card, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 60));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<CardTargetRelation> queryable = ((IQueryable<CardTargetRelation>)c.CardTargetRelation).Where((CardTargetRelation x) => x.CardGuid == card.Guid);
					if (queryable == null || !queryable.Any())
					{
						return;
					}
					foreach (CardTargetRelation item in queryable)
					{
						CardTargetRelationBusiness det = await MapFromDatabase.MapAsync(base.Context.Guid, item, null, tranName, token);
						await card.AddToDetailListAsync(det, token);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 76);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs");
				}
			}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardTargetRelationPersistenceRepository.cs", 94);
		}
	}
}
