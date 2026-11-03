using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class OwnerPersistanceRepository : PersistenceRepository<OwnerBusiness, Owner>, IOwnerRepository, IRepository<OwnerBusiness>, INoGuidRepository<OwnerBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OwnerBusiness, Owner>, IItemAsyncMapper<Owner, OwnerBusiness>
	{
		public Task<Owner> MapAsync(string contextGuid, OwnerBusiness source, Owner destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Owner();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Family = source.Family;
				destination.VisitorGuid = source.VisitorGuid;
				destination.Status = source.Status;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 207);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<OwnerBusiness> MapAsync(string contextGuid, Owner source, OwnerBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OwnerBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Family = source.Family;
				destination.VisitorGuid = source.VisitorGuid;
				destination.Status = source.Status;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 233);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Owner>> MapListAsync(string contextGuid, IEnumerable<OwnerBusiness> source, List<Owner> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OwnerBusiness>> MapListAsync(string contextGuid, IEnumerable<Owner> source, List<OwnerBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Owner, OwnerBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OwnerBusiness, Owner> MapToDatabase => Mapper;

	public override IItemMapper<Owner, OwnerBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public OwnerPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<OwnerBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 28));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs"))
		{
			List<OwnerBusiness> list = new List<OwnerBusiness>();
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbDataReader dr = await c.ExecuteReaderAsync("sp_AdvOwner_SelectAll", null, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadData(dr));
					}
					dr.Close();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 41);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<IEnumerable<OwnerBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 52));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs"))
		{
			List<OwnerBusiness> list = new List<OwnerBusiness>();
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = null;
					if (status.HasValue)
					{
						parameters = new List<SqlParameter>
						{
							new SqlParameter("@st", (object)status)
						};
					}
					DbDataReader dr = await c.ExecuteReaderAsync("sp_AdvOwner_SelectAll", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadData(dr));
					}
					dr.Close();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 68);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
				}
			}
			return list;
		}
	}

	public override async Task<OwnerBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 78));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs"))
		{
			OwnerBusiness item = null;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Guid", (object)guid)
					};
					DbDataReader dr = await c.ExecuteReaderAsync("sp_AdvOwner_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
					if (await dr.ReadAsync())
					{
						item = LoadData(dr);
					}
					dr.Close();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 92);
					return null;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
				}
			}
			return item;
		}
	}

	private OwnerBusiness LoadData(DbDataReader dr)
	{
		OwnerBusiness ownerBusiness = new OwnerBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				ownerBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				ownerBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				ownerBusiness.Status = (bool)dr["Status"];
			}
			if (dr["Family"] != DBNull.Value)
			{
				ownerBusiness.Family = dr["Family"].ToString();
			}
			if (dr["Name"] != DBNull.Value)
			{
				ownerBusiness.Name = dr["Name"].ToString();
			}
			if (dr["VisitorGuid"] != DBNull.Value)
			{
				ownerBusiness.VisitorGuid = (Guid)dr["VisitorGuid"];
			}
			if (dr["VisitorName"] != DBNull.Value)
			{
				ownerBusiness.VisitorName = dr["VisitorName"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 114);
		}
		return ownerBusiness;
	}

	public async Task<int> DeleteCountAsync(string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 119));
		using (new JaegerMonitor("DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs"))
		{
			int result;
			try
			{
				int num = ((IQueryable<Owner>)adv_ModelContext.Owners).Where((Owner q) => q.Guid != Guid.Empty).Count((Owner q) => q.Status == false);
				result = num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 129);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 141));
		using (new JaegerMonitor("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs"))
		{
			bool result;
			try
			{
				Owner owner = ((IQueryable<Owner>)adv_ModelContext.Owners).FirstOrDefault((Owner x) => string.Concat(x.Name + " ", x.Family) == name && x.Guid != guid);
				result = owner == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 151);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs");
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
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\OwnerPersistanceRepository.cs", 173);
			throw;
		}
	}
}
