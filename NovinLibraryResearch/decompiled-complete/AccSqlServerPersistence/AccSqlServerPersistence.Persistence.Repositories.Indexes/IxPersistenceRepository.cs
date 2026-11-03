using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Indexes;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Global;
using EntityCache.Core.Repository.Indexes;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Indexes;

public abstract class IxPersistenceRepository<U> : IIxRepository, IDisposable where U : class, IIndexEntity, new()
{
	private class mapper : IItemAsyncMapper<IndexEntityBusiness, IndexEntityBusiness>
	{
		public Task<IndexEntityBusiness> MapAsync(string contextGuid, IndexEntityBusiness source, IndexEntityBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs"))
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
						destination = new IndexEntityBusiness(contextGuid);
					}
					destination.EntityGuid = source.EntityGuid;
					destination.WordId = source.WordId;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 131);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<IndexEntityBusiness>> MapListAsync(string contextGuid, IEnumerable<IndexEntityBusiness> source, List<IndexEntityBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private ContextHolder dbContext;

	private bool _isDisposed;

	protected abstract string TableName { get; }

	protected abstract IItemAsyncMapper<IndexEntityBusiness, U> MapperToDatabase { get; }

	protected IItemAsyncMapper<IndexEntityBusiness, IndexEntityBusiness> MapperT_to_T { get; } = new mapper();


	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => Context.Guid;

	public IxPersistenceRepository(ContextHolder dbContext)
	{
		this.dbContext = dbContext;
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 43);
		}
	}

	public virtual async Task<ReturnedSaveFuncInfo> SaveListAsync(Dictionary<Guid, HashSet<int>> lst, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs"))
		{
			int line = 0;
			int _timeOut = 300000;
			BaseModelContext c = await dbContext.GetContextAsync(tranName, _timeOut, "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 51);
			Stopwatch sw = Stopwatch.StartNew();
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				string tableName;
				if (typeof(IxProductCodeField) == typeof(U))
				{
					tableName = "Acc_IxProductCodeField";
				}
				else if (typeof(IxProductAllFields) == typeof(U))
				{
					tableName = "Acc_IxProductAllFields";
				}
				else if (typeof(IxCustomerAllFields) == typeof(U))
				{
					tableName = "Acc_IxCustomerAllFields";
				}
				else if (typeof(IxProductDescField) == typeof(U))
				{
					tableName = "Acc_IxProductDescField";
				}
				else if (typeof(IxProductNameAndCodeField) == typeof(U))
				{
					tableName = "Acc_IxProductNameAndCodeField";
				}
				else if (typeof(IxProductNameField) == typeof(U))
				{
					tableName = "Acc_IxProductNameField";
				}
				else if (typeof(IxProductT1Field) == typeof(U))
				{
					tableName = "Acc_IxProductT1Field";
				}
				else if (typeof(IxProductT2Field) == typeof(U))
				{
					tableName = "Acc_IxProductT2Field";
				}
				else if (typeof(IxProductT3Field) == typeof(U))
				{
					tableName = "Acc_IxProductT3Field";
				}
				else if (typeof(IxProductT4Field) == typeof(U))
				{
					tableName = "Acc_IxProductT4Field";
				}
				else if (typeof(IxProductT5Field) == typeof(U))
				{
					tableName = "Acc_IxProductT5Field";
				}
				else if (typeof(IxProductT6Field) == typeof(U))
				{
					tableName = "Acc_IxProductT6Field";
				}
				else if (typeof(IxProductT7Field) == typeof(U))
				{
					tableName = "Acc_IxProductT7Field";
				}
				else
				{
					if (!(typeof(IxProductT8Field) == typeof(U)))
					{
						throw new NotImplementedException();
					}
					tableName = "Acc_IxProductT8Field";
				}
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, lst.Keys);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 74);
				ret.throwExceptionIfError();
				string spName = $"Delete {tableName} from {tableName} inner join StimulQuery on StimulQuery.Guid={tableName}.EntityGuid where StimulQuery.RefrencedId={returnedSaveFuncInfoWithValue.value}";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.Text, _timeOut, "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs"), ReturnedState.None, "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 80);
				ret.throwExceptionIfError();
				List<U> list = new List<U>();
				foreach (Guid entityGuid in lst.Keys)
				{
					if (lst.TryGetValue(entityGuid, out var value) && value != null && value.Any())
					{
						list.AddRange(value.Select((int q) => new U
						{
							EntityGuid = entityGuid,
							WordId = q
						}).ToList());
					}
				}
				await ((DbContext)c).Set<U>().AddRangeAsync((IEnumerable<U>)list, token);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 98);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 101);
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 102);
			}
			sw.Stop();
			WebErrorLog.Instance.Debug($"\nIndex save count:{lst?.Count() ?? (-1)} time:{sw.ElapsedMilliseconds}(ms)", "", "SaveListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxPersistenceRepository.cs", 106);
			return ret;
		}
	}
}
