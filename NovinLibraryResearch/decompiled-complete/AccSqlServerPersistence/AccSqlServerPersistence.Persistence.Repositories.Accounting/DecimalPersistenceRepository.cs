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

internal class DecimalPersistenceRepository : PersistenceRepository<DecimalBusiness, DecimalEntity>, IDecimalRepository, IRepository<DecimalBusiness>, INoGuidRepository<DecimalBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DecimalBusiness, DecimalEntity>, IItemAsyncMapper<DecimalEntity, DecimalBusiness>
	{
		public Task<DecimalEntity> MapAsync(string contextGuid, DecimalBusiness source, DecimalEntity destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs"))
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
						destination = new DecimalEntity();
					}
					destination.Guid = source.Guid;
					destination.Name = source.Name;
					destination.Country = source.Country;
					destination.Description = source.Description;
					destination.Abstract = source.Abstract ?? "";
					destination.LastUpdate = source.LastUpdate;
					destination.LastPrice = source.LastPrice;
					destination.IconIndex = source.IconIndex;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					destination.Symbol = source.Symbol;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 116);
					throw;
				}
			}
		}

		public Task<DecimalBusiness> MapAsync(string contextGuid, DecimalEntity source, DecimalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs"))
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
						destination = new DecimalBusiness(contextGuid);
					}
					destination.Guid = source.Guid;
					destination.Name = source.Name;
					destination.Country = source.Country;
					destination.Description = source.Description;
					destination.Abstract = source.Abstract;
					destination.LastUpdate = source.LastUpdate;
					destination.LastPrice = source.LastPrice;
					destination.IconIndex = source.IconIndex;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					destination.Symbol = source.Symbol;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 148);
					throw;
				}
			}
		}

		public Task<List<DecimalEntity>> MapListAsync(string contextGuid, IEnumerable<DecimalBusiness> source, List<DecimalEntity> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DecimalBusiness>> MapListAsync(string contextGuid, IEnumerable<DecimalEntity> source, List<DecimalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<DecimalEntity, DecimalBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DecimalBusiness, DecimalEntity> MapToDatabase => Mapper;

	public override IItemMapper<DecimalEntity, DecimalBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal DecimalPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 39);
		}
	}

	public async Task<DecimalBusiness> GetAsync(string name, string country, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 43));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs"))
		{
			DecimalBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DecimalEntity source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<DecimalEntity>((IQueryable<DecimalEntity>)c.Decimalentity, (Expression<Func<DecimalEntity, bool>>)((DecimalEntity x) => x.Name == name && x.Country == country), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 54);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<DecimalBusiness> GetAsync(string slug, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 65));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs"))
		{
			DecimalBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DecimalEntity source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<DecimalEntity>((IQueryable<DecimalEntity>)c.Decimalentity, (Expression<Func<DecimalEntity, bool>>)((DecimalEntity x) => !string.IsNullOrEmpty(x.Symbol) && x.Symbol == slug), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs", 76);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DecimalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
