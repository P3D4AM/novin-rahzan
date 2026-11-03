using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer.Models.Placement;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PlacementPersistenceRepository : PlacementPersistenceRepositoryBase
{
	public PlacementPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<PlacementKardexViewModel>> GetAllKardexAsync(PlacementKardexFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllKardexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PlacementPersistenceRepository.cs", 21);
		IEnumerable<PlacementKardexViewModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<PlacementKardexViewModel> list = new List<PlacementKardexViewModel>();
				List<SqlParameter> list2 = new List<SqlParameter>();
				if ((filter?.PlacementGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@PlacementGuid", (object)filter.PlacementGuid.Value));
				}
				if ((filter?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@TafsilGuid", (object)filter.TafsilGuid.Value));
				}
				if ((filter?.CashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid.Value));
				}
				if ((filter?.NumberFrom ?? 0) != 0)
				{
					list2.Add(new SqlParameter("@NumberFrom", (object)filter.NumberFrom));
				}
				if ((filter?.NumberTo ?? 0) != 0)
				{
					list2.Add(new SqlParameter("@NumberTo", (object)filter.NumberTo));
				}
				if (filter?.DateFrom.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom.Value));
				}
				if (filter?.DateTo.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@DateTo", (object)filter.DateTo.Value));
				}
				if ((filter?.PeopleCountFrom ?? 0) != 0)
				{
					list2.Add(new SqlParameter("@PeopleCountFrom", (object)filter.PeopleCountFrom));
				}
				if ((filter?.PeopleCountTo ?? 0) != 0)
				{
					list2.Add(new SqlParameter("@PeopleCountTo", (object)filter.PeopleCountTo));
				}
				if ((filter?.SumFrom ?? 0m) != 0m)
				{
					list2.Add(new SqlParameter("@SumFrom", (object)filter.SumFrom));
				}
				if ((filter?.SumTo ?? 0m) != 0m)
				{
					list2.Add(new SqlParameter("@SumTo", (object)filter.SumTo));
				}
				if (list2 == null || !list2.Any())
				{
					list2 = null;
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Placement_Report", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllKardexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PlacementPersistenceRepository.cs");
				while (await dr.ReadAsync(token))
				{
					list.Add(LoadData(dr));
				}
				result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllKardexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PlacementPersistenceRepository.cs", 48);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllKardexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PlacementPersistenceRepository.cs");
			}
		}
		return result;
	}

	private PlacementKardexViewModel LoadData(DbDataReader dr)
	{
		try
		{
			PlacementKardexViewModel placementKardexViewModel = new PlacementKardexViewModel();
			if (dr["MasterGuid"] != DBNull.Value)
			{
				placementKardexViewModel.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (dr["StartDate"] != DBNull.Value)
			{
				placementKardexViewModel.StartDate = (DateTime)dr["StartDate"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				placementKardexViewModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				placementKardexViewModel.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				placementKardexViewModel.Description = dr["Description"].ToString();
			}
			if (dr["Sum"] != DBNull.Value)
			{
				placementKardexViewModel.Sum = (decimal)dr["Sum"];
			}
			if (dr["PeopleCount"] != DBNull.Value)
			{
				placementKardexViewModel.PeopleCount = (int)dr["PeopleCount"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				placementKardexViewModel.Number = (int)dr["Number"];
			}
			if (dr["EndDate"] != DBNull.Value)
			{
				placementKardexViewModel.EndDate = (DateTime)dr["EndDate"];
			}
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				placementKardexViewModel.ReceptionGuid = (Guid)dr["ReceptionGuid"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				placementKardexViewModel.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["CashierName"] != DBNull.Value)
			{
				placementKardexViewModel.CashierName = dr["CashierName"].ToString();
			}
			if (dr["ReceptionSum"] != DBNull.Value)
			{
				placementKardexViewModel.ReceptionSum = (decimal)dr["ReceptionSum"];
			}
			if (dr["State"] != DBNull.Value)
			{
				placementKardexViewModel.State = (EnPlacementStatus)(int)dr["State"];
			}
			return placementKardexViewModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PlacementPersistenceRepository.cs", 82);
			throw;
		}
	}
}
