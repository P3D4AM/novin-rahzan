using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class SuggestedBankPersistenceRepository : PersistenceRepository<SuggestedBankBusiness, SuggestedBank>, ISuggestedBankRepository, IRepository<SuggestedBankBusiness>, INoGuidRepository<SuggestedBankBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SuggestedBankBusiness, SuggestedBank>, IItemAsyncMapper<SuggestedBank, SuggestedBankBusiness>
	{
		public Task<SuggestedBank> MapAsync(string contextGuid, SuggestedBankBusiness source, SuggestedBank destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs"))
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
						destination = new SuggestedBank();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ShobeCode = source.ShobeCode;
					destination.AccountNo = source.AccountNo;
					destination.BankName = source.BankName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs", 90);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<SuggestedBankBusiness> MapAsync(string contextGuid, SuggestedBank source, SuggestedBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs"))
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
						destination = new SuggestedBankBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ShobeCode = source.ShobeCode;
					destination.AccountNo = source.AccountNo;
					destination.BankName = source.BankName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs", 118);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<SuggestedBank>> MapListAsync(string contextGuid, IEnumerable<SuggestedBankBusiness> source, List<SuggestedBank> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SuggestedBankBusiness>> MapListAsync(string contextGuid, IEnumerable<SuggestedBank> source, List<SuggestedBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<SuggestedBank, SuggestedBankBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SuggestedBankBusiness, SuggestedBank> MapToDatabase => Mapper;

	public override IItemMapper<SuggestedBank, SuggestedBankBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public SuggestedBankPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs", 32);
		}
	}

	public async Task<SuggestedBankBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs", 37));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs"))
		{
			SuggestedBankBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					SuggestedBank source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SuggestedBank>((IQueryable<SuggestedBank>)c.SuggestedBanks, (Expression<Func<SuggestedBank, bool>>)((SuggestedBank p) => p.BankName == name), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\SuggestedBankPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
