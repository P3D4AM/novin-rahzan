using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Models.Accounting.Prd;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class PrdAnbarRelationPersistenceRepository : PrdAnbarRelationPersistenceRepositoryBase
{
	public PrdAnbarRelationPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<IEnumerable<ProductOrderPointModel>> GetAllProductOrderpointsAsync(CancellationToken token, string tranName)
	{
		List<ProductOrderPointModel> list = null;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllProductOrderpointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 24);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				DbDataReader dr = await c.ExecuteReaderAsync("sp_products_PointOrder", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllProductOrderpointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs");
				list = new List<ProductOrderPointModel>();
				while (await dr.ReadAsync(token))
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
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllProductOrderpointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 37);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllProductOrderpointsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs");
			}
		}
		return list;
	}

	private ProductOrderPointModel LoadData(DbDataReader dr)
	{
		ProductOrderPointModel productOrderPointModel = new ProductOrderPointModel();
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				productOrderPointModel.ProductGuid = (Guid)dr["Guid"];
			}
			if (dr["IdProduct"] != DBNull.Value)
			{
				productOrderPointModel.IdProduct = dr["IdProduct"].ToString();
			}
			if (dr["name"] != DBNull.Value)
			{
				productOrderPointModel.ProductName = dr["name"].ToString();
			}
			if (dr["description"] != DBNull.Value)
			{
				productOrderPointModel.ProductDescription = dr["description"].ToString();
			}
			if (dr["pic"] != DBNull.Value)
			{
				productOrderPointModel.Pic = dr["pic"].ToString();
			}
			if (dr["Modified"] != DBNull.Value)
			{
				productOrderPointModel.Modified = (DateTime)dr["Modified"];
			}
			if (dr["Height"] != DBNull.Value)
			{
				productOrderPointModel.Height = dr["Height"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["Width"] != DBNull.Value)
			{
				productOrderPointModel.Width = dr["Width"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["GuidVahed"] != DBNull.Value)
			{
				productOrderPointModel.VahedGuid = (Guid)dr["GuidVahed"];
			}
			if (dr["AnbarName"] != DBNull.Value)
			{
				productOrderPointModel.DepotName = dr["AnbarName"].ToString();
			}
			if (dr["AnbarDescription"] != DBNull.Value)
			{
				productOrderPointModel.DepotDescription = dr["AnbarDescription"].ToString();
			}
			if (dr["GroupName"] != DBNull.Value)
			{
				productOrderPointModel.GroupName = dr["GroupName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				productOrderPointModel.VahedName = dr["VahedName"].ToString();
			}
			if (dr["Min"] != DBNull.Value)
			{
				productOrderPointModel.OrderPoint = dr["Min"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["AnbarGuid"] != DBNull.Value)
			{
				productOrderPointModel.DepotGuid = (Guid)dr["AnbarGuid"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				productOrderPointModel.ProductStatus = (bool)dr["Status"];
			}
			if (dr["amount"] != DBNull.Value)
			{
				productOrderPointModel.Amount = dr["amount"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["ProductGroupGuid"] != DBNull.Value)
			{
				productOrderPointModel.ProductGroupGuid = (Guid)dr["ProductGroupGuid"];
			}
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 75);
			throw;
		}
		return productOrderPointModel;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAnbarRelationsAsync(IEnumerable<Guid> prdGuids, short tryCount, CancellationToken token, string tranName = null)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 88));
				foreach (Guid prdGuid in prdGuids)
				{
					if (prdGuid != Guid.Empty)
					{
						List<SqlParameter> parameters = new List<SqlParameter>
						{
							new SqlParameter("@PrdGuid", (object)prdGuid)
						};
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.ExecuteNoneQueryAsync("sp_Acc_PrdAnbarRelation_DeleteByPrdGuid", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs"), ReturnedState.None, "RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 94);
					}
				}
				return new ReturnedSaveFuncInfo();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 101);
				res.AddReturnedValue(ex2, "", "RemoveAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 102);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(ProductsChanges productsChanges, bool rollback, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (productsChanges.PrdGuid == Guid.Empty)
			{
				throw new InvalidOperationException("prdGuid is empty!");
			}
			if (productsChanges.depGuid == Guid.Empty)
			{
				throw new InvalidOperationException("DepotGuid is empty!");
			}
			try
			{
				if (productsChanges == null)
				{
					return ret;
				}
				decimal qt = (rollback ? (-productsChanges.qt) : productsChanges.qt);
				Acc_ModelContext obj = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 117));
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@qt", (object)qt),
					new SqlParameter("@prdGuid", (object)productsChanges.PrdGuid),
					new SqlParameter("@depotGuid", (object)productsChanges.depGuid)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_PrdAnbarRelation_UpdateAmount", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs"), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 124);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "Data:" + Json.ToStringJson(productsChanges), "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 128);
				ret.AddReturnedValue(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 129);
			}
			return ret;
		}
	}

	public override async Task<decimal> GetPositiveAmountAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetPositiveAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 136);
		decimal result;
		try
		{
			_ = 1;
			try
			{
				string spName = "select isnull((SELECT sum([amount]) as Amount FROM [Acc_PrdAnbarRelation] inner join Acc_Product on Acc_Product.Guid=Acc_PrdAnbarRelation.PrdGuid where amount>0 and Acc_Product.Type=0),0)";
				result = await c.ExecuteScalarAsync<decimal>(spName, null, tranName, token, 30, "GetPositiveAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", CommandType.Text);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetPositiveAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 147);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetPositiveAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<decimal> GetNegativeAmountAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetNegativeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 158);
		decimal result;
		try
		{
			_ = 1;
			try
			{
				string spName = "select isnull((SELECT sum([amount]) as Amount FROM [Acc_PrdAnbarRelation] inner join Acc_Product on Acc_Product.Guid=Acc_PrdAnbarRelation.PrdGuid where Acc_Product.Type<>1 and amount<0),0)";
				result = await c.ExecuteScalarAsync<decimal>(spName, null, tranName, token, 30, "GetNegativeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", CommandType.Text);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetNegativeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs", 169);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNegativeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\PrdAnbarRelationPersistenceRepository.cs");
			}
		}
		return result;
	}
}
