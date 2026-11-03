using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class BiometricDetailPersistenceRepository : PersistenceMasterRepository<BiometricDetailBusiness, BiometricDetail>, IBiometricDetailRepository, IPersistenceMasterRepository<BiometricDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BiometricDetail, BiometricDetailBusiness>, IItemAsyncMapper<BiometricDetailBusiness, BiometricDetail>
	{
		public Task<BiometricDetailBusiness> MapAsync(string contextGuid, BiometricDetail source, BiometricDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BiometricDetailBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.EnrolType = source.EnrolType;
				destination.Ordinal = source.Ordinal;
				if (!string.IsNullOrEmpty(source?.Data))
				{
					destination.Data = source.Data.FromJson<byte[]>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 89);
				}
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 94);
				throw;
			}
		}

		public Task<BiometricDetail> MapAsync(string contextGuid, BiometricDetailBusiness source, BiometricDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BiometricDetail();
				}
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.EnrolType = source.EnrolType;
				destination.Ordinal = source.Ordinal;
				if (source?.Data != null)
				{
					destination.Data = Json.ToStringJson(source.Data);
				}
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 119);
				throw;
			}
		}

		public Task<List<BiometricDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<BiometricDetail> source, List<BiometricDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BiometricDetail>> MapListAsync(string contextGuid, IEnumerable<BiometricDetailBusiness> source, List<BiometricDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BiometricDetail, BiometricDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BiometricDetailBusiness, BiometricDetail> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public BiometricDetailPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 37);
		}
	}

	public override Task<List<BiometricDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad");
	}

	public async Task GetAllAsync(BiometricBusiness master, CancellationToken token, string tranName)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs"))
		{
			Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 44));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<BiometricDetail> queryable = ((IQueryable<BiometricDetail>)c.BioDatas).Where((BiometricDetail x) => x.MasterGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_011b;
					}
					foreach (BiometricDetail item in queryable)
					{
						BiometricDetailBusiness det = await MapFromDatabase.MapAsync(base.ContextGuid, item, new BiometricDetailBusiness(base.ContextGuid), tranName, token);
						await master.AddDetailAsync(det);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs", 60);
					throw;
				}
				return;
				end_IL_010b:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricDetailPersistenceRepository.cs");
				}
			}
		}
	}
}
