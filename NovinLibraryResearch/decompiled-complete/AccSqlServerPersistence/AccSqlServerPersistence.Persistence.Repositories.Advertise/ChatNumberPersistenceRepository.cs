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
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class ChatNumberPersistenceRepository : PersistenceRepository<ChatNumberBusiness, ChatNumbers>, IChatNumberRepository, IRepository<ChatNumberBusiness>, INoGuidRepository<ChatNumberBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ChatNumberBusiness, ChatNumbers>, IItemAsyncMapper<ChatNumbers, ChatNumberBusiness>
	{
		public Task<ChatNumbers> MapAsync(string contextGuid, ChatNumberBusiness source, ChatNumbers destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChatNumbers();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.DateM = source.DateM;
				destination.City = source.City;
				destination.Category = source.Category;
				destination.IsUsed = source.IsUsed;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 164);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ChatNumberBusiness> MapAsync(string contextGuid, ChatNumbers source, ChatNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ChatNumberBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Number = source.Number;
				destination.DateM = source.DateM;
				destination.City = source.City;
				destination.Category = source.Category;
				destination.IsUsed = source.IsUsed;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 191);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ChatNumbers>> MapListAsync(string contextGuid, IEnumerable<ChatNumberBusiness> source, List<ChatNumbers> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ChatNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<ChatNumbers> source, List<ChatNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ChatNumbers, ChatNumberBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ChatNumberBusiness, ChatNumbers> MapToDatabase => Mapper;

	public override IItemMapper<ChatNumbers, ChatNumberBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ChatNumberPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<bool> CheckNumber(string number, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 23));
		using (new JaegerMonitor("CheckNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs"))
		{
			bool result;
			try
			{
				ChatNumbers chatNumbers = ((IQueryable<ChatNumbers>)adv_ModelContext.ChatNumbers).FirstOrDefault((ChatNumbers p) => p.Number == number);
				result = chatNumbers == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 33);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<ChatNumberBusiness>> GetAllAsyncBySp(string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsyncBySp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 46));
		using (new JaegerMonitor("GetAllAsyncBySp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs"))
		{
			List<ChatNumberBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<ChatNumbers> source = RelationalQueryableExtensions.FromSqlRaw<ChatNumbers>(((DbContext)c).Set<ChatNumbers>(), "sp_ChatNumbers_SelectAll", Array.Empty<object>())?.ToList();
					result = await Mapper.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsyncBySp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 56);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsyncBySp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<string>> GetAllCities(string tranName)
	{
		using (new JaegerMonitor("GetAllCities", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				List<string> names = new List<string>();
				SqlConnection connection = new SqlConnection(dbContext.ConnectionString);
				try
				{
					SqlCommand cmd = new SqlCommand("sp_ChatNumbers_SelectAllCities", connection);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)connection).OpenAsync();
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								names.Add(((DbDataReader)(object)dr)[0].ToString());
							}
							((DbConnection)(object)connection).Close();
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
					return names;
				}
				finally
				{
					((IDisposable)connection)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllCities", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 86);
				throw;
			}
		}
	}

	public async Task<List<string>> GetAllCategories(string tranName)
	{
		using (new JaegerMonitor("GetAllCategories", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				List<string> names = new List<string>();
				SqlConnection cn = new SqlConnection(dbContext.ConnectionString);
				try
				{
					SqlCommand cmd = new SqlCommand("sp_ChatNumbers_SelectAllCategories", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								names.Add(((DbDataReader)(object)dr)[0].ToString());
							}
							((DbConnection)(object)cn).Close();
							return names;
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
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllCategories", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 109);
				throw;
			}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ChatNumberPersistenceRepository.cs", 129);
			throw;
		}
	}
}
