using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class BarcodePersistenceRepository : BarcodePersistenceRepositoryBase
{
	public BarcodePersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> FillBarcodesAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 21);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_Barcode_FillBarcode", null, tranName, token, "", CommandType.StoredProcedure, 30, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs"), ReturnedState.None, "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 22);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 26);
			res.AddReturnedValue(ex, "", "FillBarcodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 27);
		}
		return res;
	}

	public override async Task<IEnumerable<BarcodeModel>> GetAllConflictsAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 33);
		IEnumerable<BarcodeModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<BarcodeModel> list = new List<BarcodeModel>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Barcode_GetAllConflict", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(dr));
						continue;
					}
					dr.Close();
					result = list;
					break;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 52);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllConflictsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs");
			}
		}
		return result;
	}

	private BarcodeModel LoadData(DbDataReader dr)
	{
		try
		{
			BarcodeModel barcodeModel = new BarcodeModel();
			if (dr["Barcode"] != DBNull.Value)
			{
				barcodeModel.BarCode = dr["Barcode"].ToString();
			}
			if (dr["PrdGuid"] != DBNull.Value)
			{
				barcodeModel.PrdGuid = (Guid)dr["PrdGuid"];
			}
			if (dr["CreateDate"] != DBNull.Value)
			{
				barcodeModel.CreateDate = (DateTime)dr["CreateDate"];
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				barcodeModel.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				barcodeModel.Description = dr["Description"].ToString();
			}
			if (dr["Name"] != DBNull.Value)
			{
				barcodeModel.Name = dr["Name"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				barcodeModel.Price = (decimal)dr["Price"];
			}
			if (dr["Quantity"] != DBNull.Value)
			{
				barcodeModel.Quantity = (decimal)dr["Quantity"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				barcodeModel.Type = (BarcodePriceType)dr["Type"];
			}
			if (dr["Ordinal"] != DBNull.Value)
			{
				barcodeModel.Ordinal = (short)dr["Ordinal"];
			}
			if (dr["PriceTypeGuid"] != DBNull.Value)
			{
				barcodeModel.PriceTypeGuid = (Guid)dr["PriceTypeGuid"];
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				barcodeModel.ProductName = dr["ProductName"].ToString();
			}
			return barcodeModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\BarcodePersistenceRepository.cs", 82);
			throw;
		}
	}
}
