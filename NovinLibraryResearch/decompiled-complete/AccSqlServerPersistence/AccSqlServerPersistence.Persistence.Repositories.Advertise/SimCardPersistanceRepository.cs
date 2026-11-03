using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class SimCardPersistanceRepository : PersistenceRepository<SimCardBusiness, SimCard>, ISimCardRepository, IRepository<SimCardBusiness>, INoGuidRepository<SimCardBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SimCardBusiness, SimCard>, IItemAsyncMapper<SimCard, SimCardBusiness>
	{
		public Task<SimCard> MapAsync(string contextGuid, SimCardBusiness source, SimCard destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SimCard();
				}
				destination.SheypoorModified = source.SheypoorModified;
				destination.NextUseDivar = source.NextUseDivar;
				destination.NextUseSheypoor = source.NextUseSheypoor;
				destination.isSheypoorBlocked = source.isSheypoorBlocked;
				destination.isSendDivarAdv = source.isSendDivarAdv;
				destination.isSendSheypoorAdv = source.isSendSheypoorAdv;
				destination.Number = source.Number;
				destination.OwnerGuid = source.OwnerGuid;
				destination.UseCount = source.UseCount;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DivarModified = source.DivarModified;
				destination.Operator = source.Operator;
				destination.UserName = source.UserName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 435);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<SimCardBusiness> MapAsync(string contextGuid, SimCard source, SimCardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SimCardBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.SheypoorModified = source.SheypoorModified;
				destination.NextUseDivar = source.NextUseDivar;
				destination.NextUseSheypoor = source.NextUseSheypoor;
				destination.isSheypoorBlocked = source.isSheypoorBlocked;
				destination.isSendDivarAdv = source.isSendDivarAdv;
				destination.isSendSheypoorAdv = source.isSendSheypoorAdv;
				destination.Number = source.Number;
				destination.OwnerGuid = source.OwnerGuid;
				destination.UseCount = source.UseCount;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DivarModified = source.DivarModified;
				destination.Operator = source.Operator;
				destination.UserName = source.UserName;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 470);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<SimCard>> MapListAsync(string contextGuid, IEnumerable<SimCardBusiness> source, List<SimCard> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SimCardBusiness>> MapListAsync(string contextGuid, IEnumerable<SimCard> source, List<SimCardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<SimCard, SimCardBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SimCardBusiness, SimCard> MapToDatabase => Mapper;

	public override IItemMapper<SimCard, SimCardBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal SimCardPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<long> GetNextNumberAsync(short advType, short maxDailyCountDivar, short maxDailyCountSheypoor, string IP, short maxMountlyCountDivar, short maxMountlyCountSheypoor, int maxAdvCountInIpInDayDivar, int maxAdvCountInIpInDaySheypoor, string tranName)
	{
		long ret = 0L;
		using (new JaegerMonitor("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			_ = 2;
			try
			{
				SqlConnection cn = new SqlConnection(dbContext.ConnectionString);
				try
				{
					SqlCommand val = new SqlCommand("Pr_SimCard_Select_NextNumber", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						cmd.Parameters.AddWithValue("@Type", (object)advType);
						cmd.Parameters.AddWithValue("@MaxDailyCountDivar", (object)maxDailyCountDivar);
						cmd.Parameters.AddWithValue("@MaxDailyCountSheypoor", (object)maxDailyCountSheypoor);
						cmd.Parameters.AddWithValue("@IP", (object)(IP ?? ""));
						cmd.Parameters.AddWithValue("@MaxMountlyCountDivar", (object)maxMountlyCountDivar);
						cmd.Parameters.AddWithValue("@MaxMountlyCountSheypoor", (object)maxMountlyCountSheypoor);
						cmd.Parameters.AddWithValue("@MaxAdvCountInIPInDayDivar", (object)maxAdvCountInIpInDayDivar);
						cmd.Parameters.AddWithValue("@MaxAdvCountInIPInDaySheypoor", (object)maxAdvCountInIpInDaySheypoor);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							if (await ((DbDataReader)(object)dr).ReadAsync())
							{
								ret = ((DbDataReader)(object)dr)[0].ToString().ParseToLong(0L);
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
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
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 58);
				throw;
			}
		}
	}

	public async Task<List<long>> GetSimCardsForSendAdvAsync(string tranName)
	{
		using (new JaegerMonitor("GetSimCardsForSendAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			SqlConnection connection = new SqlConnection(dbContext.ConnectionString);
			try
			{
				_ = 2;
				try
				{
					SqlCommand cmd = new SqlCommand("Pr_SimCard_SelectAll_ForSendAdv", connection);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)connection).OpenAsync();
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							List<long> list = new List<long>();
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								list.Add((long)((DbDataReader)(object)dr)[0]);
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)connection).Close();
							return list;
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
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetSimCardsForSendAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 85);
					throw;
				}
				finally
				{
					if (((DbConnection)(object)connection).State == ConnectionState.Open)
					{
						((DbConnection)(object)connection).Close();
					}
				}
			}
			finally
			{
				((IDisposable)connection)?.Dispose();
			}
		}
	}

	public async Task<string> GetOwnerNameAsync(long simCardNumber, string tranName)
	{
		using (new JaegerMonitor("GetOwnerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			_ = 2;
			try
			{
				SqlConnection connection = new SqlConnection(dbContext.ConnectionString);
				try
				{
					SqlCommand command = new SqlCommand("Pr_SimCard_Select_OwnerName", connection);
					try
					{
						((DbCommand)(object)command).CommandType = CommandType.StoredProcedure;
						command.Parameters.AddWithValue("Number", (object)simCardNumber);
						await ((DbConnection)(object)connection).OpenAsync();
						SqlDataReader reader = await command.ExecuteReaderAsync();
						try
						{
							await ((DbDataReader)(object)reader).ReadAsync();
							string? result = ((DbDataReader)(object)reader)[0].ToString();
							((DbDataReader)(object)reader).Close();
							((DbConnection)(object)connection).Close();
							return result;
						}
						finally
						{
							((IDisposable)reader)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)command)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)connection)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetOwnerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 116);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<SimCardBusiness> lst, short tryCount, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			_ = 2;
			try
			{
				Adv_ModelContext dbcontext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 127));
				if (lst == null && !lst.Any())
				{
					return ret;
				}
				foreach (SimCard item in await MapToDatabase.MapListAsync(ContextGuid, lst))
				{
					if (item.Guid == Guid.Empty)
					{
						item.Guid = Guid.NewGuid();
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 134);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 145);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 146);
			}
			return ret;
		}
	}

	public async Task<SimCardBusiness> GetAsync(long number, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 154));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			SimCardBusiness result;
			try
			{
				_ = 1;
				try
				{
					SimCard source = ((IQueryable<SimCard>)c.SimCards).FirstOrDefault((SimCard p) => p.Number == number);
					result = await MapFromDatabase.MapAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 163);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> CheckNumberAsync(long number, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 175));
		using (new JaegerMonitor("CheckNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			bool result;
			try
			{
				SimCard simCard = ((IQueryable<SimCard>)adv_ModelContext.SimCards).FirstOrDefault((SimCard p) => p.Number == number && p.Guid != guid);
				result = simCard == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 185);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> CheckUserNameAsync(string username, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 197));
		using (new JaegerMonitor("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			bool result;
			try
			{
				SimCard simCard = ((IQueryable<SimCard>)adv_ModelContext.SimCards).FirstOrDefault((SimCard p) => p.UserName == username && p.Guid != guid);
				result = simCard == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 207);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<SimCardBusiness>> GetAllSpecialAsync(Expression<Func<ISimCard, bool>> where, string tranName)
	{
		using (new JaegerMonitor("GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 220));
			List<SimCardBusiness> result;
			try
			{
				_ = 1;
				try
				{
					DbSet<SimCard> val = c.SimCards;
					if (where != null)
					{
						val = (DbSet<SimCard>)((IQueryable<ISimCard>)val).Where(where);
					}
					List<SimCard> source = ((IEnumerable<SimCard>)val)?.ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 230);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<SimCardBusiness>> GetAllAsync(Guid? ownerGuid, string tranName)
	{
		SqlConnection connection = new SqlConnection(dbContext.ConnectionString);
		try
		{
			using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
			{
				List<SimCardBusiness> list = new List<SimCardBusiness>();
				try
				{
					if (ownerGuid == Guid.Empty)
					{
						ownerGuid = null;
					}
					SqlCommand val = new SqlCommand("sp_SimCard_SelectAll", connection);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand command = val;
					try
					{
						command.Parameters.AddWithValue("@ownerGuid", (object)ownerGuid);
						await ((DbConnection)(object)connection).OpenAsync();
						SqlDataReader dr = await command.ExecuteReaderAsync();
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								list.Add(LoadData(dr));
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)connection).Close();
						}
						finally
						{
							((IDisposable)dr)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)command)?.Dispose();
					}
					return list;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 260);
					throw;
				}
				finally
				{
					if (((DbConnection)(object)connection).State == ConnectionState.Open)
					{
						((DbConnection)(object)connection).Close();
					}
				}
			}
		}
		finally
		{
			((IDisposable)connection)?.Dispose();
		}
	}

	public async Task<int> DeleteCountAsync(string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 275));
		int result;
		try
		{
			int num = ((IQueryable<SimCard>)adv_ModelContext.SimCards).Where((SimCard q) => q.Guid != Guid.Empty).Count((SimCard q) => q.Status == false);
			result = num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 284);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await adv_ModelContext.DisposeAsync("DeleteCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAllSheypoorBlocks(bool state, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		_ = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 297));
		using (new JaegerMonitor("UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			SqlConnection connection = new SqlConnection(dbContext.ConnectionString);
			try
			{
				_ = 1;
				try
				{
					SqlCommand command = new SqlCommand("sp_Simcard_UpdateAllSheypoorBlock", connection);
					try
					{
						((DbCommand)(object)command).CommandType = CommandType.StoredProcedure;
						command.Parameters.AddWithValue("@state", (object)state);
						((DbConnection)(object)connection).Open();
						await ((DbCommand)(object)command).ExecuteNonQueryAsync();
						((DbConnection)(object)connection).Close();
					}
					finally
					{
						((IDisposable)command)?.Dispose();
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 319);
					ret.AddReturnedValue(ex, "", "UpdateAllSheypoorBlocks", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 320);
				}
				finally
				{
					if (((DbConnection)(object)connection).State == ConnectionState.Open)
					{
						((DbConnection)(object)connection).Close();
					}
				}
			}
			finally
			{
				((IDisposable)connection)?.Dispose();
			}
			return ret;
		}
	}

	private SimCardBusiness LoadData(SqlDataReader dr)
	{
		SimCardBusiness simCardBusiness = new SimCardBusiness(ContextGuid);
		try
		{
			if (((DbDataReader)(object)dr)["Number"] != DBNull.Value)
			{
				simCardBusiness.Number = (long)((DbDataReader)(object)dr)["Number"];
			}
			if (((DbDataReader)(object)dr)["Modified"] != DBNull.Value)
			{
				simCardBusiness.Modified = (DateTime)((DbDataReader)(object)dr)["Modified"];
			}
			if (((DbDataReader)(object)dr)["OwnerGuid"] != DBNull.Value)
			{
				simCardBusiness.OwnerGuid = (Guid)((DbDataReader)(object)dr)["OwnerGuid"];
			}
			if (((DbDataReader)(object)dr)["UseCount"] != DBNull.Value)
			{
				simCardBusiness.UseCount = (int)((DbDataReader)(object)dr)["UseCount"];
			}
			if (((DbDataReader)(object)dr)["Status"] != DBNull.Value)
			{
				simCardBusiness.Status = (bool)((DbDataReader)(object)dr)["Status"];
			}
			if (((DbDataReader)(object)dr)["Guid"] != DBNull.Value)
			{
				simCardBusiness.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["Operator"] != DBNull.Value)
			{
				simCardBusiness.Operator = ((DbDataReader)(object)dr)["Operator"].ToString();
			}
			if (((DbDataReader)(object)dr)["SheypoorModified"] != DBNull.Value)
			{
				simCardBusiness.SheypoorModified = (DateTime)((DbDataReader)(object)dr)["SheypoorModified"];
			}
			if (((DbDataReader)(object)dr)["DivarModified"] != DBNull.Value)
			{
				simCardBusiness.DivarModified = (DateTime)((DbDataReader)(object)dr)["DivarModified"];
			}
			if (((DbDataReader)(object)dr)["NextUseDivar"] != DBNull.Value)
			{
				simCardBusiness.NextUseDivar = (DateTime)((DbDataReader)(object)dr)["NextUseDivar"];
			}
			if (((DbDataReader)(object)dr)["NextUseSheypoor"] != DBNull.Value)
			{
				simCardBusiness.NextUseSheypoor = (DateTime)((DbDataReader)(object)dr)["NextUseSheypoor"];
			}
			if (((DbDataReader)(object)dr)["UserName"] != DBNull.Value)
			{
				simCardBusiness.UserName = ((DbDataReader)(object)dr)["UserName"].ToString();
			}
			if (((DbDataReader)(object)dr)["isSheypoorBlocked"] != DBNull.Value)
			{
				simCardBusiness.isSheypoorBlocked = (bool)((DbDataReader)(object)dr)["isSheypoorBlocked"];
			}
			if (((DbDataReader)(object)dr)["isSendDivarAdv"] != DBNull.Value)
			{
				simCardBusiness.isSendDivarAdv = (bool)((DbDataReader)(object)dr)["isSendDivarAdv"];
			}
			if (((DbDataReader)(object)dr)["isSendSheypoorAdv"] != DBNull.Value)
			{
				simCardBusiness.isSendSheypoorAdv = (bool)((DbDataReader)(object)dr)["isSendSheypoorAdv"];
			}
			if (((DbDataReader)(object)dr)["HasSheyPoorToken"] != DBNull.Value)
			{
				object obj = ((DbDataReader)(object)dr)["HasSheyPoorToken"];
				simCardBusiness.HasSheypoorToken = obj.ToString().ParseToBoolean();
			}
			if (((DbDataReader)(object)dr)["HasDivarToken"] != DBNull.Value)
			{
				object obj2 = ((DbDataReader)(object)dr)["HasDivarToken"];
				simCardBusiness.HasDivarToken = obj2.ToString().ParseToBoolean();
			}
			if (((DbDataReader)(object)dr)["HasDivarChatToken"] != DBNull.Value)
			{
				object obj3 = ((DbDataReader)(object)dr)["HasDivarChatToken"];
				simCardBusiness.HasDivarChatToken = obj3.ToString().ParseToBoolean();
			}
			if (((DbDataReader)(object)dr)["OwnerName"] != DBNull.Value)
			{
				simCardBusiness.OwnerName = ((DbDataReader)(object)dr)["OwnerName"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 374);
			throw;
		}
		return simCardBusiness;
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\SimCardPersistanceRepository.cs", 392);
			throw;
		}
	}
}
