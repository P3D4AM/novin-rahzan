using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Coding;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Coding;

internal class AccountLogPersistenceRepository : PersistenceRepository<AccountLogBusiness, AccountLog>, IAccountLogRepository, IRepository<AccountLogBusiness>, INoGuidRepository<AccountLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AccountLogBusiness, AccountLog>, IItemAsyncMapper<AccountLog, AccountLogBusiness>
	{
		public Task<AccountLog> MapAsync(string contextGuid, AccountLogBusiness source, AccountLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccountLog();
				}
				destination.Guid = source.Guid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Date = source.Date;
				destination.NewAccount = source.NewAccount;
				destination.OldAccount = source.OldAccount;
				destination.DebuggerInfo = source.DebuggerInfo;
				destination.Action = source.Action;
				destination.Modified = source.Modified;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountLogPersistenceRepository.cs", 64);
			}
			return Task.FromResult(destination);
		}

		public Task<AccountLogBusiness> MapAsync(string contextGuid, AccountLog source, AccountLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccountLogBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Date = source.Date;
				destination.NewAccount = source.NewAccount;
				destination.OldAccount = source.OldAccount;
				destination.DebuggerInfo = source.DebuggerInfo;
				destination.Action = source.Action;
				destination.Modified = source.Modified;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountLogPersistenceRepository.cs", 90);
			}
			return Task.FromResult(destination);
		}

		public Task<List<AccountLog>> MapListAsync(string contextGuid, IEnumerable<AccountLogBusiness> source, List<AccountLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AccountLogBusiness>> MapListAsync(string contextGuid, IEnumerable<AccountLog> source, List<AccountLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<AccountLog, AccountLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<AccountLog, AccountLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AccountLogBusiness, AccountLog> MapToDatabase => Mapper;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	public AccountLogPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountLogPersistenceRepository.cs", 38);
		}
	}
}
