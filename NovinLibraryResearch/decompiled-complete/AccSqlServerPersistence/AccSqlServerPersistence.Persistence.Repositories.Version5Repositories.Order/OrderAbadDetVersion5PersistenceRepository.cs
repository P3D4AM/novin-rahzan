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

internal class OrderAbadDetVersion5PersistenceRepository : PersistenceMasterRepository<AbadDetBusiness, AbadDet>, IAbadDetRepository, IPersistenceMasterRepository<AbadDetBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, OrderAbadDetVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, OrderAbadDetVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public override IItemAsyncMapper<AbadDet, AbadDetBusiness> MapFromDatabase { get; }

	public override IItemAsyncMapper<AbadDetBusiness, AbadDet> MapToDatabase { get; }

	public OrderAbadDetVersion5PersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static OrderAbadDetVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		OrderAbadDetVersion5PersistenceRepository orderAbadDetVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs", 35);
			throw;
		}
	}

	private WebAbad LoadData(DbDataReader dr)
	{
		WebAbad webAbad = new WebAbad();
		try
		{
			if (dr["Count"] != DBNull.Value)
			{
				webAbad.Count = (decimal)dr["Count"];
			}
			if (dr["ShapeType"] != DBNull.Value)
			{
				webAbad.ShapeType = (Shape)dr["ShapeType"];
			}
			if (dr["DetailID"] != DBNull.Value)
			{
				webAbad.DetailID = (int)dr["DetailID"];
			}
			if (dr["X"] != DBNull.Value)
			{
				webAbad.X = (decimal)dr["X"];
			}
			if (dr["Y"] != DBNull.Value)
			{
				webAbad.Y = (decimal)dr["Y"];
			}
			if (dr["Z"] != DBNull.Value)
			{
				webAbad.Z = (decimal)dr["Z"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs", 57);
			throw;
		}
		return webAbad;
	}

	public async Task<List<WebAbad>> GetAllAsync(int orderDetailId, Document orderType, string tranName = "")
	{
		List<WebAbad> res = new List<WebAbad>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs", 66);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DetailID", (object)orderDetailId),
					new SqlParameter("@OrderType", (object)(short)orderType)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_OrderAbad_GetAllByOrderDetailID", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs", 82);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Order\\OrderAbadDetVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}
}
