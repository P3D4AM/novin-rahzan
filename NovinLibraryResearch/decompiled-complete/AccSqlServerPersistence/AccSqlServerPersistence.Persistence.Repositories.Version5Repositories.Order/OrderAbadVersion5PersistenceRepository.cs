using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Order;

internal class OrderAbadVersion5PersistenceRepository : PersistenceRepository<OrderAbadBusiness, OrderAbad>, IOrderAbadRepository, IRepository<OrderAbadBusiness>, INoGuidRepository<OrderAbadBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, OrderAbadVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, OrderAbadVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public override IItemAsyncMapper<OrderAbad, OrderAbadBusiness> MapFromDatabase { get; }

	public override IItemAsyncMapper<OrderAbadBusiness, OrderAbad> MapToDatabase { get; }

	public override bool HasStatus { get; }

	public override bool HasModified { get; }

	public override IItemMapper<OrderAbad, OrderAbadBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public OrderAbadVersion5PersistenceRepository(ContextHolder db)
		: base(db)
	{
		_instance.TryAdd(db.Context.Guid, this);
		DatabaseContext = db;
	}

	public static OrderAbadVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		OrderAbadVersion5PersistenceRepository orderAbadVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs", 35);
			throw;
		}
	}

	public Task<OrderAbadBusiness> GetAsync(Guid DetailGuid, Document OrderType, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	private async Task<WebOrderAbad> LoadDataAsync(DbDataReader dr, int orderDetId, Document type, string tranName = "")
	{
		WebOrderAbad item = new WebOrderAbad();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				item.RefrencedID = (int)dr["ID"];
			}
			if (dr["ShapeType"] != DBNull.Value)
			{
				item.ShapeInfo = (ShapeInfo)dr["ShapeType"];
			}
			OrderAbadDetVersion5PersistenceRepository instance = OrderAbadDetVersion5PersistenceRepository.GetInstance(ContextGuid);
			WebOrderAbad webOrderAbad = item;
			webOrderAbad.ListAbad = await instance.GetAllAsync(orderDetId, type, tranName);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs", 65);
			throw;
		}
	}

	public async Task<WebOrderAbad> GetAsync(int orderDetailId, Document orderType, string tranName = "")
	{
		WebOrderAbad res = null;
		BaseModelContext c = DatabaseContext.GetContext(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs", 74);
		try
		{
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DetailID", (object)orderDetailId),
					new SqlParameter("@OrderType", (object)(short)orderType)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_OrderAbad_GetAllByOrderDetailID", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadDataAsync(dr, orderDetailId, orderType, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs", 89);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}
}
