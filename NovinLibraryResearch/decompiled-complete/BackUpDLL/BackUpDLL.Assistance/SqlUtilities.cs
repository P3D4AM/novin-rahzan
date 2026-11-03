using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace BackUpDLL.Assistance;

public static class SqlUtilities
{
	public static ReturnedSaveFuncInfo RollBackTran(this SqlTransaction tr, SqlConnection cn = null)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			((DbTransaction)(object)tr)?.Rollback();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "RollBackTran", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Assistance\\SqlUtilities.cs", 20);
			WebErrorLog.Instance.StartLog(ex, "", "RollBackTran", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Assistance\\SqlUtilities.cs", 21);
		}
		finally
		{
			returnedSaveFuncInfo.AddReturnedValue(cn.CloseConnection(), ReturnedState.None, "RollBackTran", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Assistance\\SqlUtilities.cs", 25);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo CloseConnection(this SqlConnection cn)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (cn == null)
			{
				return returnedSaveFuncInfo;
			}
			if (((DbConnection)(object)cn).State == ConnectionState.Open)
			{
				return returnedSaveFuncInfo;
			}
			((DbConnection)(object)cn)?.Close();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CloseConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Assistance\\SqlUtilities.cs", 40);
			WebErrorLog.Instance.StartLog(ex, "", "CloseConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\Assistance\\SqlUtilities.cs", 41);
		}
		return returnedSaveFuncInfo;
	}
}
