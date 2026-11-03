using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;

public class ReceptionHavaleVersion5PersistenceRepository : IReceptionHavaleRepository, IPersistenceMasterRepository<ReceptionHavaleBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, ReceptionHavaleVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ReceptionHavaleVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public IItemAsyncMapper<ReceptionHavale, ReceptionHavaleBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<ReceptionHavaleBusiness, ReceptionHavale> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public ReceptionHavaleVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static ReceptionHavaleVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ReceptionHavaleVersion5PersistenceRepository receptionHavaleVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 40);
			throw;
		}
	}

	public async Task<bool> IsPeygiriNumberTekrariAsync(string number, Guid guid, Guid bankGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 49);
		try
		{
			if (string.IsNullOrEmpty(number))
			{
				return false;
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@number", (object)(number ?? ""))
			};
			object obj = await c.ExecuteScalarAsync<object>("sp_ReceptionHavale_CheckPeygiriNo", parameters, tranName, default(CancellationToken), 30, "IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs");
			if (obj == null || string.IsNullOrEmpty(obj.ToString()))
			{
				return false;
			}
			return guid == Guid.Parse(obj.ToString());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 63);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				c?.DisposeAsync("IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs");
			}
		}
	}

	private WebReceptionHavale LoadReceptionHavale(DbDataReader dr)
	{
		WebReceptionHavale webReceptionHavale = new WebReceptionHavale();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				webReceptionHavale.Id = (int)dr["ID"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				webReceptionHavale.Number = dr["Number"].ToString();
			}
			if (dr["BankTafsilGuid"] != DBNull.Value)
			{
				webReceptionHavale.TafsilGuid = (Guid)dr["BankTafsilGuid"];
			}
			if (dr["BankMoeinGuid"] != DBNull.Value)
			{
				webReceptionHavale.MoeinGuid = (Guid)dr["BankMoeinGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webReceptionHavale.Price = (decimal)dr["Price"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				webReceptionHavale.Date = (DateTime)dr["Date"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				short type = dr["Type"].ToString().ParseToShort(0);
				webReceptionHavale.Type = (ENHavaleType)type;
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webReceptionHavale.Guid = (Guid)dr["GUID"];
			}
			if (dr["TerminalID"] != DBNull.Value)
			{
				webReceptionHavale.TerminalId = dr["TerminalID"].ToString();
			}
			if (dr["PAN"] != DBNull.Value)
			{
				webReceptionHavale.PAN = dr["PAN"].ToString();
			}
			if (dr["STAN"] != DBNull.Value)
			{
				webReceptionHavale.STAN = dr["STAN"].ToString();
			}
			if (dr["RRN"] != DBNull.Value)
			{
				webReceptionHavale.RRN = dr["RRN"].ToString();
			}
			if (dr["MerchantID"] != DBNull.Value)
			{
				webReceptionHavale.MerchantId = dr["MerchantID"].ToString();
			}
			if (dr["MessageID"] != DBNull.Value)
			{
				webReceptionHavale.MessageId = dr["MessageID"].ToString();
			}
			if (dr["DateTasviye"] != DBNull.Value)
			{
				webReceptionHavale.DateTasviye = (DateTime)dr["DateTasviye"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				webReceptionHavale.Description = dr["Description"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReceptionHavale", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 100);
			throw;
		}
		return webReceptionHavale;
	}

	public async Task<List<WebReceptionHavale>> GetAllHavaleAsync(int sanadId, string tranName = "")
	{
		List<WebReceptionHavale> list = new List<WebReceptionHavale>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllHavaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 108);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)sanadId)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionHavale_SelectRowsByIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllHavaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadReceptionHavale(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllHavaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 119);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllHavaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 139);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionHavaleBusiness> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionHavaleBusiness> items, int masterId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 152);
		try
		{
			foreach (ReceptionHavaleBusiness item in items)
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@IDSanad", (object)masterId),
					new SqlParameter("@Number", (object)item.Number),
					new SqlParameter("@BankTafsilGuid", (object)item.TafsilGuid),
					new SqlParameter("@BankMoeinGuid", (object)item.MoeinGuid),
					new SqlParameter("@Price", (object)item.Price),
					new SqlParameter("@Date", (object)item.Date),
					new SqlParameter("@Description", (object)item.Description.FixString().Trim()),
					new SqlParameter("@Type", (object)(short)item.Type),
					new SqlParameter("@TerminalID", (object)item.TerminalId),
					new SqlParameter("@PAN", (object)item.PAN),
					new SqlParameter("@STAN", (object)item.STAN),
					new SqlParameter("@RRN", (object)item.RRN),
					new SqlParameter("@MerchantID", (object)item.MerchantId),
					new SqlParameter("@MessageID", (object)item.MessageId),
					new SqlParameter("@FullDescription", (object)item.FullDescription),
					new SqlParameter("@GUID", (object)item.Guid)
				};
				if (item.DateTasviye > DateTime.Now.AddYears(-50))
				{
					list.Add(new SqlParameter("@DateTasviye", (object)item.DateTasviye));
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_ReceptionHavale_Insert", list, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 177);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 182);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 183);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(int masterId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 196);
		try
		{
			List<WebReceptionHavale> list = await GetAllHavaleAsync(masterId, tranName);
			if (list == null || list.Count <= 0)
			{
				return res;
			}
			foreach (WebReceptionHavale item in list)
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)item.Id)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_ReceptionHavale_DeleteRow", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 208);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 213);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionHavaleVersion5PersistenceRepository.cs", 214);
		}
		return res;
	}

	public Task<List<ReceptionHavaleBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Guid>> GetAllMasterGuidsForFixWageAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
