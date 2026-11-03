using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Sanad;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Sanad;

internal class ViewSanadDetailCoverPersistenceRepository : ViewSanadDetailCoverBasePersistenceRepository
{
	public ViewSanadDetailCoverPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<ViewSanadDetailCoverBusiness>> GetAllInvalidRelationsAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllInvalidRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverPersistenceRepository.cs", 18);
		IEnumerable<ViewSanadDetailCoverBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ViewSanadDetailCover_GetAllInvalidatedRelations", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllInvalidRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
				result = list?.OrderBy((ViewSanadDetailCoverBusiness q) => q.AsTypeDocument);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllInvalidRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverPersistenceRepository.cs", 31);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllInvalidRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverPersistenceRepository.cs");
			}
		}
		return result;
	}

	private ViewSanadDetailCoverBusiness LoadData(DbDataReader dr)
	{
		try
		{
			ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid);
			if (dr["Guid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["DebitMoeinGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DebitMoeinGuid = (Guid)dr["DebitMoeinGuid"];
			}
			if (dr["DebitTafsilGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DebitTafsilGuid = (Guid)dr["DebitTafsilGuid"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Date = (DateTime)dr["Date"];
			}
			if (dr["AsGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.AsGuid = (Guid)dr["AsGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.AsType = (DocumentDet)dr["AsType"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Credit = (decimal)dr["Credit"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["SanadSt"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.SanadSt = (SanadSt)dr["SanadSt"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Id2 = (int)dr["Id2"];
			}
			if (dr["CreditMoeinGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.CreditMoeinGuid = (Guid)dr["CreditMoeinGuid"];
			}
			if (dr["CreditTafsilGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.CreditTafsilGuid = (Guid)dr["CreditTafsilGuid"];
			}
			if (dr["Qt"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Qt = (decimal)dr["Qt"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["Qt2"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Qt2 = (decimal)dr["Qt2"];
			}
			if (dr["RDiscount"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.RDiscount = (decimal)dr["RDiscount"];
			}
			if (dr["RBazaryab"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.RBazaryab = (decimal)dr["RBazaryab"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Serial = dr["Serial"].ToString();
			}
			if (dr["SanadDesc"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.SanadDesc = dr["SanadDesc"].ToString();
			}
			if (dr["RDesc"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.RDesc = dr["RDesc"].ToString();
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["VahedGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.VahedGuid = (Guid)dr["VahedGuid"];
			}
			if (dr["DateMoaser"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DateMoaser = (DateTime)dr["DateMoaser"];
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["DecimalQt"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DecimalQt = (decimal)dr["DecimalQt"];
			}
			if (dr["DecimalPrice"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DecimalPrice = (decimal)dr["DecimalPrice"];
			}
			if (dr["DetailGuid"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.DetailGuid = (Guid)dr["DetailGuid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["AsTypeDocument"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.AsTypeDocument = (Document)dr["AsTypeDocument"];
			}
			if (dr["TafsilGuid2"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.TafsilGuid2 = (Guid)dr["TafsilGuid2"];
			}
			if (dr["TafsilGuid3"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.TafsilGuid3 = (Guid)dr["TafsilGuid3"];
			}
			if (dr["TafsilGuid4"] != DBNull.Value)
			{
				viewSanadDetailCoverBusiness.TafsilGuid4 = (Guid)dr["TafsilGuid4"];
			}
			return viewSanadDetailCoverBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailCoverPersistenceRepository.cs", 86);
			throw;
		}
	}
}
