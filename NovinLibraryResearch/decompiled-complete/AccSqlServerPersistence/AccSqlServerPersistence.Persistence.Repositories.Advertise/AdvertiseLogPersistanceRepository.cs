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
using EntityCache.Business.Advertise.NoBisunessModels;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Advertise;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class AdvertiseLogPersistanceRepository : PersistenceRepository<AdvertiseLogBusiness, AdvertiseLog>, IAdvertiseLogRepository, IRepository<AdvertiseLogBusiness>, INoGuidRepository<AdvertiseLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvertiseLogBusiness, AdvertiseLog>, IItemAsyncMapper<AdvertiseLog, AdvertiseLogBusiness>, IItemAsyncMapper<IAdvertiseLog, AdvertiseLogBusiness>
	{
		public Task<AdvertiseLog> MapAsync(string contextGuid, AdvertiseLogBusiness source, AdvertiseLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvertiseLog();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Content = source.Content;
				destination.SimCardNumber = source.SimCardNumber;
				destination.State = source.State;
				destination.City = source.City;
				destination.Region = source.Region;
				destination.Price = source.Price;
				destination.Category = source.Category;
				destination.SubCategory1 = source.SubCategory1;
				destination.SubCategory2 = source.SubCategory2;
				destination.URL = source.URL;
				destination.IP = source.IP;
				destination.UpdateDesc = source.UpdateDesc;
				destination.VisitCount = source.VisitCount;
				destination.DateSh = source.DateSh;
				destination.DateM = source.DateM;
				destination.Adv = source.Adv;
				destination.MasterVisitorGuid = source.MasterVisitorGuid;
				destination.SlaveVisitorGuid = source.SlaveVisitorGuid;
				destination.StatusCode = source.StatusCode;
				destination.AdvType = source.AdvType;
				destination.LastUpdate = source.LastUpdate;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 603);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvertiseLogBusiness> MapAsync(string contextGuid, AdvertiseLog source, AdvertiseLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvertiseLogBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Content = source.Content;
				destination.SimCardNumber = source.SimCardNumber;
				destination.State = source.State;
				destination.City = source.City;
				destination.Region = source.Region;
				destination.Price = source.Price;
				destination.Category = source.Category;
				destination.SubCategory1 = source.SubCategory1;
				destination.SubCategory2 = source.SubCategory2;
				destination.URL = source.URL;
				destination.IP = source.IP;
				destination.UpdateDesc = source.UpdateDesc;
				destination.VisitCount = source.VisitCount;
				destination.DateSh = source.DateSh;
				destination.DateM = source.DateM;
				destination.Adv = source.Adv;
				destination.MasterVisitorGuid = source.MasterVisitorGuid;
				destination.SlaveVisitorGuid = source.SlaveVisitorGuid;
				destination.StatusCode = source.StatusCode;
				destination.AdvType = source.AdvType;
				destination.LastUpdate = source.LastUpdate;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 647);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvertiseLog>> MapListAsync(string contextGuid, IEnumerable<AdvertiseLogBusiness> source, List<AdvertiseLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvertiseLogBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvertiseLog> source, List<AdvertiseLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<AdvertiseLogBusiness> MapAsync(string contextGuid, IAdvertiseLog source, AdvertiseLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvertiseLogBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Content = source.Content;
				destination.SimCardNumber = source.SimCardNumber;
				destination.State = source.State;
				destination.City = source.City;
				destination.Region = source.Region;
				destination.Price = source.Price;
				destination.Category = source.Category;
				destination.SubCategory1 = source.SubCategory1;
				destination.SubCategory2 = source.SubCategory2;
				destination.URL = source.URL;
				destination.IP = source.IP;
				destination.UpdateDesc = source.UpdateDesc;
				destination.VisitCount = source.VisitCount;
				destination.DateSh = source.DateSh;
				destination.DateM = source.DateM;
				destination.Adv = source.Adv;
				destination.MasterVisitorGuid = source.MasterVisitorGuid;
				destination.SlaveVisitorGuid = source.SlaveVisitorGuid;
				destination.StatusCode = source.StatusCode;
				destination.AdvType = source.AdvType;
				destination.LastUpdate = source.LastUpdate;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 697);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvertiseLogBusiness>> MapListAsync(string contextGuid, IEnumerable<IAdvertiseLog> source, List<AdvertiseLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public IItemAsyncMapper<IAdvertiseLog, AdvertiseLogBusiness> MapFromInreface { get; set; } = new mapper();


	public override IItemAsyncMapper<AdvertiseLog, AdvertiseLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvertiseLogBusiness, AdvertiseLog> MapToDatabase => Mapper;

	public override IItemMapper<AdvertiseLog, AdvertiseLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AdvertiseLogPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<AdvertiseLogBusiness> GetLastAdvAsync(string advName, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 33));
		using (new JaegerMonitor("GetLastAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			AdvertiseLogBusiness result;
			try
			{
				_ = 1;
				try
				{
					AdvertiseLog source = ((!string.IsNullOrEmpty(advName)) ? (from p in (IQueryable<AdvertiseLog>)c.AdvertiseLogs
						where p.Adv == advName
						orderby p.DateM descending
						select p).FirstOrDefault() : ((IQueryable<AdvertiseLog>)c.AdvertiseLogs).OrderByDescending((AdvertiseLog p) => p.DateM).FirstOrDefault());
					result = await MapFromDatabase.MapAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetLastAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastAdvAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<AdvertiseLogBusiness>> GetAllSpecialAsync(Expression<Func<IAdvertiseLog, bool>> where, Func<IQueryable<IAdvertiseLog>, IOrderedQueryable<IAdvertiseLog>> orderby, string includes, int takeCount, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 61));
		using (new JaegerMonitor("GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			List<AdvertiseLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<IAdvertiseLog> queryable = (IQueryable<IAdvertiseLog>)c.AdvertiseLogs;
					if (where != null)
					{
						queryable = queryable.Where(where);
					}
					List<IAdvertiseLog> source = queryable?.ToList();
					result = await MapFromInreface.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 72);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> GetAllAdvInDayFromIP(string ip, AdvertiseType type, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAdvInDayFromIP", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 82));
		using (new JaegerMonitor("GetAllAdvInDayFromIP", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			int result;
			try
			{
				string now = DateTime.Now.MiladiToCalture();
				int num = ((IQueryable<AdvertiseLog>)adv_ModelContext.AdvertiseLogs).Count((AdvertiseLog p) => p.DateSh == now && p.IP == ip && p.AdvType == type);
				result = num;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAdvInDayFromIP", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 95);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("GetAllAdvInDayFromIP", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetAllAsync(string search, short state, AdvertiseType type, CancellationToken token, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 107));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, ((IEnumerable<AdvertiseLog>)c.AdvertiseLogs)?.ToList());
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 138);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<int>> GetAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 149));
		using (new JaegerMonitor("GetAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			List<int> result;
			try
			{
				List<int> list = new List<int>();
				DateTime dateTime = firstDate;
				while (dateTime <= secondDate)
				{
					string strI = dateTime.MiladiToCalture();
					int item = ((type == AdvertiseType.All) ? ((IQueryable<AdvertiseLog>)adv_ModelContext.AdvertiseLogs).Count((AdvertiseLog q) => q.DateSh == strI) : ((IQueryable<AdvertiseLog>)adv_ModelContext.AdvertiseLogs).Count((AdvertiseLog q) => q.DateSh == strI && q.AdvType == type));
					list.Add(item);
					dateTime = dateTime.AddDays(1.0);
				}
				result = list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 171);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("GetAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<int>> GetPublishedAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName)
	{
		using (new JaegerMonitor("GetPublishedAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetPublishedAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 184));
			List<int> result;
			try
			{
				List<int> list = new List<int>();
				DateTime dateTime = firstDate;
				while (dateTime <= secondDate)
				{
					string strI = dateTime.MiladiToCalture();
					int item = ((type == AdvertiseType.All) ? ((IQueryable<AdvertiseLog>)adv_ModelContext.AdvertiseLogs).Count((AdvertiseLog q) => q.DateSh == strI && q.StatusCode == AdvStatusCode.Published) : ((IQueryable<AdvertiseLog>)adv_ModelContext.AdvertiseLogs).Count((AdvertiseLog q) => q.DateSh == strI && q.StatusCode == AdvStatusCode.Published && q.AdvType == type));
					list.Add(item);
					dateTime = dateTime.AddDays(1.0);
				}
				result = list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPublishedAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 205);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("GetPublishedAdvCountInSpecialMounthAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AdvertiseLogBusiness> GetAsync(string url, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 216));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			AdvertiseLogBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapAsync(ContextGuid, ((IQueryable<AdvertiseLog>)c.AdvertiseLogs).FirstOrDefault((AdvertiseLog p) => p.URL.ToLower() == url.ToLower()));
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 226);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 241));
			try
			{
				string text = DateTime.Now.MiladiToCalture();
				int num = text.Substring(0, 4).ParseToInt();
				string text2 = text.Substring(5, 2);
				if (text2 == "01")
				{
					num--;
				}
				DateTime dateTime = string.Concat(str2: (text2.ParseToInt() - 1 >= 10) ? (text2.ParseToInt() - 1).ToString() : ("0" + (text2.ParseToInt() - 1)), str0: num.ToString(), str1: "/", str3: "/01").ShamsiToMiladi();
				SqlParameter val = new SqlParameter("@date", (object)dateTime);
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)adv_ModelContext).Database, "exec sp_AdvertiseLog_RemoveLogByDate @date", new object[1] { val });
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 265);
				throw;
			}
		}
	}

	public async Task<IEnumerable<AdvertiseLogViewModel>> GetAllAsync(long number, DateTime d1, DateTime d2, AdvStatusCode statusCode, AdvertiseType type, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 274));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			List<AdvertiseLogViewModel> ret = new List<AdvertiseLogViewModel>();
			IEnumerable<AdvertiseLogViewModel> result;
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
						SqlCommand val = new SqlCommand("sp_AdvertiseLog_GetAll", cn);
						((DbCommand)val).CommandType = CommandType.StoredProcedure;
						SqlCommand cmd = val;
						try
						{
							cmd.Parameters.AddWithValue("@number", (object)number);
							cmd.Parameters.AddWithValue("@d1", (object)d1);
							cmd.Parameters.AddWithValue("@d2", (object)d2);
							cmd.Parameters.AddWithValue("@status", (object)(short)statusCode);
							cmd.Parameters.AddWithValue("@type", (object)type);
							await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
							SqlDataReader dr = await cmd.ExecuteReaderAsync();
							try
							{
								while (await ((DbDataReader)(object)dr).ReadAsync())
								{
									ret.Add(LoadData(dr));
								}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 298);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	private static AdvertiseLogViewModel LoadData(SqlDataReader dr)
	{
		AdvertiseLogViewModel advertiseLogViewModel = new AdvertiseLogViewModel();
		try
		{
			if (((DbDataReader)(object)dr).IsClosed)
			{
				return advertiseLogViewModel;
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["Guid"].ToString()))
			{
				advertiseLogViewModel.Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["Title"].ToString()))
			{
				advertiseLogViewModel.Title = ((DbDataReader)(object)dr)["Title"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["SimCardNumber"].ToString()))
			{
				advertiseLogViewModel.SimCardNumber = (long)((DbDataReader)(object)dr)["SimCardNumber"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["State"].ToString()))
			{
				advertiseLogViewModel.State = ((DbDataReader)(object)dr)["State"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["City"].ToString()))
			{
				advertiseLogViewModel.City = ((DbDataReader)(object)dr)["City"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["Region"].ToString()))
			{
				advertiseLogViewModel.Region = ((DbDataReader)(object)dr)["Region"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["Category"].ToString()))
			{
				advertiseLogViewModel.Category = ((DbDataReader)(object)dr)["Category"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["SubCategory1"].ToString()))
			{
				advertiseLogViewModel.SubCategory1 = ((DbDataReader)(object)dr)["SubCategory1"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["SubCategory2"].ToString()))
			{
				advertiseLogViewModel.SubCategory2 = ((DbDataReader)(object)dr)["SubCategory2"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["URL"].ToString()))
			{
				advertiseLogViewModel.URL = ((DbDataReader)(object)dr)["URL"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["IP"].ToString()))
			{
				advertiseLogViewModel.IP = ((DbDataReader)(object)dr)["IP"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["VisitCount"].ToString()))
			{
				advertiseLogViewModel.VisitCount = (int)((DbDataReader)(object)dr)["VisitCount"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["DateSh"].ToString()))
			{
				advertiseLogViewModel.DateSh = ((DbDataReader)(object)dr)["DateSh"].ToString();
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["DateM"].ToString()))
			{
				advertiseLogViewModel.DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["StatusCode"].ToString()))
			{
				advertiseLogViewModel.StatusCode = (AdvStatusCode)((DbDataReader)(object)dr)["StatusCode"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["AdvType"].ToString()))
			{
				advertiseLogViewModel.AdvType = (AdvertiseType)((DbDataReader)(object)dr)["AdvType"];
			}
			if (!string.IsNullOrEmpty(((DbDataReader)(object)dr)["UpdateDesc"].ToString()))
			{
				advertiseLogViewModel.UpdateDesc = ((DbDataReader)(object)dr)["UpdateDesc"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 334);
			throw;
		}
		return advertiseLogViewModel;
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetAllNotFullAsync(DateTime date1, DateTime date2, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotFullAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 342));
		using (new JaegerMonitor("GetAllNotFullAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@date1", (object)date1);
					SqlParameter val2 = new SqlParameter("@date2", (object)date2);
					List<AdvertiseLog> source = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLog>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLog>(c.AdvertiseLogs, "sp_AdvertiseLog_GetAllNotFull @date1,@date2", new object[2] { val, val2 }), default(CancellationToken));
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotFullAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 354);
					result = null;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotFullAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetAllEditNeedAsync(DateTime date1, DateTime date2, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllEditNeedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 365));
		using (new JaegerMonitor("GetAllEditNeedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@date1", (object)date1);
					SqlParameter val2 = new SqlParameter("@date2", (object)date2);
					List<AdvertiseLog> source = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLog>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLog>(c.AdvertiseLogs, "sp_AdvertiseLog_GetAllEditNeed @date1,@date2", new object[2] { val, val2 }), default(CancellationToken));
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllEditNeedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 377);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllEditNeedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 388));
		using (new JaegerMonitor("GetAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@date1", (object)date1);
					SqlParameter val2 = new SqlParameter("@date2", (object)date2);
					SqlParameter val3 = new SqlParameter("@number", (object)number);
					List<AdvertiseLog> source = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLog>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLog>(c.AdvertiseLogs, "sp_AdvertiseLog_GetAdvCountInMount @date1,@date2,@number", new object[3] { val, val2, val3 }), default(CancellationToken));
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 404);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetPublishAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetPublishAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 415));
		using (new JaegerMonitor("GetPublishAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@date1", (object)date1);
					SqlParameter val2 = new SqlParameter("@date2", (object)date2);
					SqlParameter val3 = new SqlParameter("@number", (object)number);
					List<AdvertiseLog> source = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLog>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLog>(c.AdvertiseLogs, "sp_AdvertiseLog_GetPublishAdvCountInMount @date1,@date2,@number", new object[3] { val, val2, val3 }), default(CancellationToken));
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetPublishAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 430);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetPublishAdvCountInMountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<AdvertiseLogBusiness>> GetAllBySpAsync(string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllBySpAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 441));
		using (new JaegerMonitor("GetAllBySpAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			List<AdvertiseLogBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<AdvertiseLog> source = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLog>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLog>(c.AdvertiseLogs, "sp_AdvertiseLog_GetAllAdv", Array.Empty<object>()), default(CancellationToken));
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllBySpAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 452);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllBySpAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvCountReport>> GetAllAdvCountReportAsync(string monthStart, string monthEnd, string dateSh, string ip, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAdvCountReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 464));
		using (new JaegerMonitor("GetAllAdvCountReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvCountReport> result;
			try
			{
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@MonthStart", (object)monthStart);
					SqlParameter val2 = new SqlParameter("@MonthEnd", (object)monthEnd);
					SqlParameter val3 = new SqlParameter("@DateSh", (object)dateSh);
					SqlParameter val4 = new SqlParameter("@Ip", (object)ip);
					result = await EntityFrameworkQueryableExtensions.ToListAsync<AdvCountReport>(RelationalQueryableExtensions.FromSqlRaw<AdvCountReport>(((DbContext)c).Set<AdvCountReport>(), "sp_AdvertiseLog_AdvCountReport @MonthStart,@MonthEnd,@DateSh,@Ip", new object[4] { val, val2, val3, val4 }), default(CancellationToken));
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAdvCountReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 480);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAdvCountReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AdvertiseLogBusiness> GetLastSendAsync(long number, AdvertiseType type, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetLastSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 491));
		using (new JaegerMonitor("GetLastSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			AdvertiseLogBusiness result;
			try
			{
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@number", (object)number);
					SqlParameter val2 = new SqlParameter("@type", (object)(short)type);
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AdvertiseLogBusiness>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLogBusiness>(((DbContext)c).Set<AdvertiseLogBusiness>(), "sp_AdvertiseLog_SelectLastSend @number ,@type", new object[2] { val, val2 }), default(CancellationToken));
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetLastSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 504);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetLastSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AdvertiseLogBusiness>> GetAllByNextUseAsync(DateTime date1, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByNextUseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 515));
		using (new JaegerMonitor("GetAllByNextUseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs"))
		{
			IEnumerable<AdvertiseLogBusiness> result;
			try
			{
				_ = 1;
				try
				{
					SqlParameter val = new SqlParameter("@date1", (object)date1);
					result = await EntityFrameworkQueryableExtensions.ToListAsync<AdvertiseLogBusiness>(RelationalQueryableExtensions.FromSqlRaw<AdvertiseLogBusiness>(((DbContext)c).Set<AdvertiseLogBusiness>(), "sp_AdvertiseLog_GetAdvLogByNextUse @date1", new object[1] { val }), default(CancellationToken));
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByNextUseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 526);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByNextUseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs");
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
				dbContext = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvertiseLogPersistanceRepository.cs", 549);
			throw;
		}
	}
}
