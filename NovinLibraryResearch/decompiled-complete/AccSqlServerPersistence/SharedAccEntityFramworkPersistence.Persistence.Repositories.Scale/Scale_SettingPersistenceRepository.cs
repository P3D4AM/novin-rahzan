using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_SettingPersistenceRepository : PersistenceRepository<Scale_SettingBusiness, Scale_Setting>, IScale_SettingRepository, IRepository<Scale_SettingBusiness>, INoGuidRepository<Scale_SettingBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Setting, Scale_SettingBusiness>, IItemAsyncMapper<Scale_SettingBusiness, Scale_Setting>
	{
		public Task<Scale_SettingBusiness> MapAsync(string contextGuid, Scale_Setting source, Scale_SettingBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_SettingBusiness(contextGuid);
				}
				destination.Name = source.Name;
				destination.Val = source.Val;
				destination.UserGuid = source.UserGuid;
				destination.Modified = source.Modified;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SettingPersistenceRepository.cs", 64);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Setting> MapAsync(string contextGuid, Scale_SettingBusiness source, Scale_Setting destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Setting();
				}
				destination.Name = source.Name;
				destination.Val = source.Val;
				destination.UserGuid = source.UserGuid;
				destination.Modified = source.Modified;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SettingPersistenceRepository.cs", 90);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_SettingBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Setting> source, List<Scale_SettingBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Setting>> MapListAsync(string contextGuid, IEnumerable<Scale_SettingBusiness> source, List<Scale_Setting> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Setting, Scale_SettingBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Setting, Scale_SettingBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_SettingBusiness, Scale_Setting> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_SettingPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_SettingPersistenceRepository.cs", 38);
		}
	}
}
