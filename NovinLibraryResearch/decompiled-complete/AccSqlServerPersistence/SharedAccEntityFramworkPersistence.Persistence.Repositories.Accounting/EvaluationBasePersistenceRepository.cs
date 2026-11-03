using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Evaluation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer.Models.Evaluation;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

internal abstract class EvaluationBasePersistenceRepository : PersistenceRepository<EvaluationBusiness, Evaluation>, IEvaluationRepository, IRepository<EvaluationBusiness>, INoGuidRepository<EvaluationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Evaluation, EvaluationBusiness>, IItemAsyncMapper<EvaluationBusiness, Evaluation>
	{
		public async Task<EvaluationBusiness> MapAsync(string contextGuid, Evaluation source, EvaluationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = await EvaluationBusiness.GenerateDefaultAsync(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.DateM = source.DateM;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.AnbarGardaniEzafeGuid = source.AnbarGardaniEzafeGuid;
				destination.AnbarGardaniKasrGuid = source.AnbarGardaniKasrGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.ChangeStatus = EntityChangeSt.Edited;
				return destination;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 203);
				throw;
			}
		}

		public Task<Evaluation> MapAsync(string contextGuid, EvaluationBusiness source, Evaluation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Evaluation();
				}
				destination.Guid = source.Guid;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.DateM = source.DateM;
				destination.SanadNo = source.SanadNo;
				destination.SanadSt = source.SanadSt;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.AnbarGardaniEzafeGuid = source.AnbarGardaniEzafeGuid;
				destination.AnbarGardaniKasrGuid = source.AnbarGardaniKasrGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DateUtc = destination.DateM.ToUniversalTime();
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 239);
				throw;
			}
		}

		public Task<List<EvaluationBusiness>> MapListAsync(string contextGuid, IEnumerable<Evaluation> source, List<EvaluationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Evaluation>> MapListAsync(string contextGuid, IEnumerable<EvaluationBusiness> source, List<Evaluation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Evaluation, EvaluationBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Evaluation, EvaluationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<EvaluationBusiness, Evaluation> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public EvaluationBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 43);
		}
	}

	public async Task<IEnumerable<EvaluationModel>> GetAllAsync(EvaluationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 47));
		IEnumerable<EvaluationModel> result;
		try
		{
			IQueryable<Evaluation> queryable = (IQueryable<Evaluation>)EntityFrameworkQueryableExtensions.Include<Evaluation, Cashier>((IQueryable<Evaluation>)EntityFrameworkQueryableExtensions.Include<Evaluation, PrdAnbarGardani>((IQueryable<Evaluation>)EntityFrameworkQueryableExtensions.Include<Evaluation, PrdAnbarGardani>((IQueryable<Evaluation>)acc_ModelContext.Evaluation, (Expression<Func<Evaluation, PrdAnbarGardani>>)((Evaluation q) => q.AnbarGardaniKasrItem)), (Expression<Func<Evaluation, PrdAnbarGardani>>)((Evaluation q) => q.AnbarGardaniEzafeItem)), (Expression<Func<Evaluation, Cashier>>)((Evaluation q) => q.CashierItem));
			if (filter != null)
			{
				if (filter.StartDate.HasValue)
				{
					queryable = queryable?.Where((Evaluation q) => q.DateM >= filter.StartDate.Value);
				}
				if (filter.EndDate.HasValue)
				{
					queryable = queryable?.Where((Evaluation q) => q.DateM <= filter.EndDate.Value);
				}
				if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					queryable = queryable?.Where((Evaluation q) => q.CashierGuid == filter.CashierGuid.Value);
				}
				if (filter.Id2From.GetValueOrDefault() != 0)
				{
					queryable = queryable?.Where((Evaluation q) => q.Id2 >= filter.Id2From.Value);
				}
				if (filter.Id2To.GetValueOrDefault() != 0)
				{
					queryable = queryable?.Where((Evaluation q) => q.Id2 <= filter.Id2To.Value);
				}
				if (filter.NumberList?.Any() ?? false)
				{
					queryable = queryable?.Where((Evaluation q) => filter.NumberList.Contains(q.Id2));
				}
				if (filter.SanadNoFrom.GetValueOrDefault() != 0)
				{
					queryable = queryable?.Where((Evaluation q) => q.SanadNo >= filter.SanadNoFrom.Value);
				}
				if (filter.SanadNoTo.GetValueOrDefault() != 0)
				{
					queryable = queryable?.Where((Evaluation q) => q.SanadNo <= filter.SanadNoTo.Value);
				}
				if (filter.SanadNoList?.Any() ?? false)
				{
					queryable = queryable?.Where((Evaluation q) => filter.SanadNoList.Contains(q.SanadNo));
				}
				if (!string.IsNullOrEmpty(filter.Description))
				{
					queryable = queryable?.Where((Evaluation p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{filter.Description}%"));
				}
			}
			result = ((queryable != null && queryable.Any()) ? queryable.Select((Evaluation q) => new EvaluationModel
			{
				AnbarGardaniEzafeGuid = q.AnbarGardaniEzafeGuid,
				AnbarGardaniEzafeSum = q.AnbarGardaniEzafeItem.EzafeSum,
				AnbarGardaniKasrGuid = q.AnbarGardaniKasrGuid,
				AnbarGardaniKasrSum = q.AnbarGardaniKasrItem.KasrSum,
				CashierGuid = q.CashierGuid,
				CashierName = q.CashierItem.Name,
				DateM = q.DateM,
				Description = q.Description,
				Guid = q.Guid,
				Id2 = q.Id2,
				Modified = q.Modified,
				SanadNo = q.SanadNo,
				SanadSt = q.SanadSt,
				Status = q.Status
			})?.ToList() : null);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 85);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public virtual Task<List<EvaluationDetailModel>> GetAllDetailsAsync(Guid? guid, DateTime dateM, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> GetNextId2Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 97));
		using (new JaegerMonitor("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Evaluation>(((IQueryable<Evaluation>)c.Evaluation).OrderByDescending((Evaluation x) => x.Id2).Take(1), token))?.Id2 ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 109);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<EvaluationBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 121));
			EvaluationBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Evaluation evaluation = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Evaluation>((IQueryable<Evaluation>)c.Evaluation, (Expression<Func<Evaluation, bool>>)((Evaluation x) => x.Id2 == number), token);
					result = ((evaluation != null) ? (await MapFromDatabase.MapAsync(ContextGuid, evaluation, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 133);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<EvaluationBusiness> GetByAnbargardaniAsync(Guid anbarGardaniGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByAnbargardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByAnbargardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 145));
			EvaluationBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (anbarGardaniGuid == Guid.Empty)
					{
						result = null;
					}
					else
					{
						Evaluation evaluation = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Evaluation>((IQueryable<Evaluation>)c.Evaluation, (Expression<Func<Evaluation, bool>>)((Evaluation x) => x.AnbarGardaniKasrGuid == anbarGardaniGuid || x.AnbarGardaniEzafeGuid == anbarGardaniGuid), token);
						result = ((evaluation != null) ? (await MapFromDatabase.MapAsync(ContextGuid, evaluation, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetByAnbargardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs", 158);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByAnbargardaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\EvaluationBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
