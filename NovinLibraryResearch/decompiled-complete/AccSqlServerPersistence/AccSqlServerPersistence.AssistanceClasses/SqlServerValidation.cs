using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public static class SqlServerValidation
{
	public static async Task<ReturnedSaveFuncInfo> IsSqlVersionBiggerOrEqualThan2016Async(ISettingRepo repo)
	{
		return await IsSqlVersionBiggerOrEqualThan2016Async(repo, Settings.ServerConnectionsString);
	}

	public static async Task<ReturnedSaveFuncInfo> IsSqlVersionBiggerOrEqualThan2016Async(ISettingRepo repo, string sqlConnection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionStringAsync(sqlConnection), ReturnedState.None, "IsSqlVersionBiggerOrEqualThan2016Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 21);
			if (ret.HasError)
			{
				throw new ArgumentException(ret.ErrorMessage);
			}
			int num = await BaseDataBaseAction.Instance.GetDataBaseVersionAsync(repo, sqlConnection, saveLog: false);
			if (num <= 2016)
			{
				StringBuilder stringBuilder = new StringBuilder(100000);
				stringBuilder.AppendLine("مشترک گرامی در حال حاضر شما از نسخه".GetTranslate() + " " + $" {num}" + "  SqlServer " + "استفاده میکنید.".GetTranslate());
				stringBuilder.AppendLine("با توجه به اینکه مجموعه نرم افزار های".GetTranslate() + "  " + ResourceManagement.TextBrandResourceManagement.CompanyName + " " + "نیازمند نسخه 2016 یا بالاتر از Sql هستند؛".GetTranslate());
				stringBuilder.AppendLine("لذا خواهشمندیم با استفاده از لینک ذیل اقدام به بروز رسانی نسخه Sql خود نمایید.".GetTranslate());
				stringBuilder.AppendLine("https://novinhozor.ir/lastarticle/sqlserver2019tips");
				stringBuilder.AppendLine(sqlConnection ?? "");
				ret.AddError(EnSystem.None, stringBuilder.ToString(), "sqlVersion", "IsSqlVersionBiggerOrEqualThan2016Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 33);
			}
			ret.AddInformation("نسخه".GetTranslate() + " SqlServer".GetTranslate() + " : " + $"{num}", "", "IsSqlVersionBiggerOrEqualThan2016Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 35);
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "IsSqlVersionBiggerOrEqualThan2016Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 40);
			ret.AddReturnedValue(ex2, "", "IsSqlVersionBiggerOrEqualThan2016Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 41);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> CheckConnectionStringAsync(string sqlConnectionString)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(sqlConnectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				((DbConnection)(object)cn).Close();
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			ret.AddReturnedValue(exception, "", "CheckConnectionStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerValidation.cs", 56);
		}
		return ret;
	}
}
