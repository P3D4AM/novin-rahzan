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
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class VisitorPersistanceRepository : PersistenceRepository<VisitorBusiness, Visitor>, IVisitorRepository, IRepository<VisitorBusiness>, INoGuidRepository<VisitorBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<VisitorBusiness, Visitor>, IItemAsyncMapper<Visitor, VisitorBusiness>
	{
		public Task<Visitor> MapAsync(string contextGuid, VisitorBusiness source, Visitor destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
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
						destination = new Visitor();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterCount = source.MasterCount;
					destination.SlaveCount = source.SlaveCount;
					destination.IsAgent = source.IsAgent;
					destination.AdvReplacement = source.AdvReplacement;
					destination.FullName = source.FullName;
					destination.Status = source.Status;
					destination.Enable_Status = source.Enable_Status;
					destination.Phone = source.Phone;
					destination.DepartmentGuid = source.DepartmentGuid;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 238);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<VisitorBusiness> MapAsync(string contextGuid, Visitor source, VisitorBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
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
						destination = new VisitorBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterCount = source.MasterCount;
					destination.SlaveCount = source.SlaveCount;
					destination.IsAgent = source.IsAgent;
					destination.AdvReplacement = source.AdvReplacement;
					destination.FullName = source.FullName;
					destination.Status = source.Status;
					destination.Enable_Status = source.Enable_Status;
					destination.Phone = source.Phone;
					destination.DepartmentGuid = source.DepartmentGuid;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 270);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Visitor>> MapListAsync(string contextGuid, IEnumerable<VisitorBusiness> source, List<Visitor> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VisitorBusiness>> MapListAsync(string contextGuid, IEnumerable<Visitor> source, List<VisitorBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Visitor, VisitorBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VisitorBusiness, Visitor> MapToDatabase => Mapper;

	public override IItemMapper<Visitor, VisitorBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public VisitorPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<MasterSlaveAdvReplacementsModel> GetMasterSlaveAdvReplacementsAsync(long simcardNumber, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetMasterSlaveAdvReplacementsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 30));
		using (new JaegerMonitor("GetMasterSlaveAdvReplacementsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
		{
			MasterSlaveAdvReplacementsModel ret = new MasterSlaveAdvReplacementsModel(ContextGuid);
			MasterSlaveAdvReplacementsModel result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					SqlConnection cn = new SqlConnection(ContextGuid.GetSqlConnectionString());
					try
					{
						SqlCommand val = new SqlCommand("Pr_Visitor_Select_MasterAndSlaveAdvReplacements", cn);
						((DbCommand)val).CommandType = CommandType.StoredProcedure;
						SqlCommand cmd = val;
						try
						{
							cmd.Parameters.AddWithValue("@Number", (object)simcardNumber);
							await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
							SqlDataReader dr = await cmd.ExecuteReaderAsync();
							try
							{
								if (await ((DbDataReader)(object)dr).ReadAsync())
								{
									ret = LoadData(ContextGuid, dr);
								}
								((DbDataReader)(object)dr).Close();
								((DbDataReader)(object)dr).Close();
								result = ret;
							}
							finally
							{
								((IDisposable)dr)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)cmd)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)cn)?.Dispose();
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetMasterSlaveAdvReplacementsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 54);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetMasterSlaveAdvReplacementsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	private static MasterSlaveAdvReplacementsModel LoadData(string contextGuid, SqlDataReader dr)
	{
		MasterSlaveAdvReplacementsModel masterSlaveAdvReplacementsModel = new MasterSlaveAdvReplacementsModel(contextGuid);
		try
		{
			if (((DbDataReader)(object)dr).IsClosed)
			{
				return masterSlaveAdvReplacementsModel;
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["AdvReplacement1"].ToString()))
			{
				masterSlaveAdvReplacementsModel.AdvReplacement1 = ((DbDataReader)(object)dr)["AdvReplacement1"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["Phone"].ToString()))
			{
				masterSlaveAdvReplacementsModel.Phone = ((DbDataReader)(object)dr)["Phone"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["AdvReplacement2"].ToString()))
			{
				masterSlaveAdvReplacementsModel.AdvReplacement2 = ((DbDataReader)(object)dr)["AdvReplacement2"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["MasterGuid"].ToString()))
			{
				masterSlaveAdvReplacementsModel.MasterGuid = (Guid?)((DbDataReader)(object)dr)["MasterGuid"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["SlaveGuid"].ToString()))
			{
				masterSlaveAdvReplacementsModel.SlaveGuid = (Guid?)((DbDataReader)(object)dr)["SlaveGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 77);
			throw;
		}
		return masterSlaveAdvReplacementsModel;
	}

	public async Task<List<VisitorBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 86));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
		{
			List<VisitorBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<Visitor> source = (IQueryable<Visitor>)c.Visitors;
						if (list != null && list.Any())
						{
							foreach (string word in list)
							{
								if (!string.IsNullOrEmpty(word))
								{
									source = source.Where((Visitor p) => DbFunctionsExtensions.Like(EF.Functions, p.FullName, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Phone, $"%{word}%"));
								}
							}
						}
						if (status.HasValue)
						{
							source = source.Where((Visitor p) => p.Status == ((bool?)status).Value);
						}
						List<VisitorBusiness> list2 = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, source.ToList())) : null);
						result = list2;
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 105);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ChangeEnableStatusAsync(Guid guid, string status, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 123));
				Visitor visitor = ((IQueryable<Visitor>)adv_ModelContext.Visitors).FirstOrDefault((Visitor x) => x.Guid == guid);
				if (visitor == null)
				{
					throw new KeyNotFoundException($"Guid({guid}) not found on table Visitor");
				}
				visitor.Enable_Status = status;
				((DbContext)adv_ModelContext).Update<Visitor>(visitor);
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeEnableStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 131);
				throw;
			}
		}
	}

	public async Task<int> DeleteCountAsync(short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
		{
			Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 142));
			try
			{
				return ((IQueryable<Visitor>)adv_ModelContext.Visitors).Where((Visitor q) => q.Guid != Guid.Empty).Count((Visitor q) => q.Status == false);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 156);
				return 0;
			}
		}
	}

	public async Task<List<VisitorBusiness>> GetAllAsync(Guid depGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 164));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs"))
		{
			List<VisitorBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<Visitor> source = ((IQueryable<Visitor>)c.Visitors)?.Where((Visitor p) => p.DepartmentGuid == depGuid).ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 174);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs");
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
				dbContext = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\VisitorPersistanceRepository.cs", 198);
			throw;
		}
	}
}
