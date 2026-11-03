using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidHazinePersistenceRepository : PersistenceMasterRepository<TolidHazineBusiness, TolidHazine>, ITolidHazineRepository, IPersistenceMasterRepository<TolidHazineBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidHazine, TolidHazineBusiness>, IItemAsyncMapper<TolidHazineBusiness, TolidHazine>
	{
		public Task<TolidHazineBusiness> MapAsync(string contextGuid, TolidHazine source, TolidHazineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidHazineBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.MasterGuid = source.MasterGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Price = source.Price;
				destination.Description = source.Description ?? "";
				destination.Type = source.Type;
				destination.TolidFormulaGuid = source.TolidFormulaGuid;
				destination.TolidFormulaIndex = source.TolidFormulaIndex;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 123);
				throw;
			}
		}

		public Task<TolidHazine> MapAsync(string contextGuid, TolidHazineBusiness source, TolidHazine destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TolidHazine();
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.MasterGuid = source.MasterGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Price = source.Price;
				destination.Description = source.Description ?? "";
				destination.Type = source.Type;
				destination.TolidFormulaGuid = source.TolidFormulaGuid;
				destination.TolidFormulaIndex = source.TolidFormulaIndex;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 154);
				throw;
			}
		}

		public Task<List<TolidHazineBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidHazine> source, List<TolidHazineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidHazine>> MapListAsync(string contextGuid, IEnumerable<TolidHazineBusiness> source, List<TolidHazine> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidHazine, TolidHazineBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidHazineBusiness, TolidHazine> MapToDatabase => Mapper;

	public TolidHazinePersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(Guid masterGuid, List<TolidHazineBusiness> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 31));
			IQueryable<TolidHazine> queryable = ((IQueryable<TolidHazine>)acc_ModelContext.TolidHazines).Where((TolidHazine q) => q.MasterGuid == masterGuid);
			if (queryable != null && queryable.Any())
			{
				List<Guid> existGuidList = list?.Select((TolidHazineBusiness q) => q.Guid)?.ToList();
				if (existGuidList?.Any() ?? false)
				{
					IQueryable<TolidHazine> queryable2 = queryable.Where((TolidHazine q) => !existGuidList.Contains(q.Guid));
					if (queryable2 != null && queryable2.Any())
					{
						acc_ModelContext.TolidHazines.RemoveRange((IEnumerable<TolidHazine>)queryable2);
					}
				}
			}
			if (res.HasError || !(list?.Any() ?? false))
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(list, masterGuid, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 44);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 48);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 49);
		}
		return res;
	}

	public async Task GetAllAsync(TolidBusiness tolid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 57));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<TolidHazine> queryable = ((IQueryable<TolidHazine>)c.TolidHazines).Where((TolidHazine x) => x.MasterGuid == tolid.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_011b;
					}
					foreach (TolidHazine item in queryable)
					{
						TolidHazineBusiness objects = await MapFromDatabase.MapAsync(base.Context.Guid, item, new TolidHazineBusiness(base.ContextGuid), tranName, token);
						await tolid.AddHazineAsync(objects, EnSystem.Mapper);
					}
					goto end_IL_010b;
					end_IL_011b:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 73);
					throw;
				}
				return;
				end_IL_010b:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs");
				}
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidHazinePersistenceRepository.cs", 91);
		}
	}
}
