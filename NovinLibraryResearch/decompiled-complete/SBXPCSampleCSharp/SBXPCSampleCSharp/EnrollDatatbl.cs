using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using TimeAttendanceDAL;

namespace SBXPCSampleCSharp;

public class EnrollDatatbl
{
	private string mDataPath;

	public static EnrollDatatbl DataModule;

	private OleDbConnection GetConnection()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mDataPath + "datEnrollDat.mdb");
	}

	public DataSet GetEnrollDatas(string sortfield = "EnrollNumber")
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		OleDbConnection connection = GetConnection();
		try
		{
			DataSet dataSet = new DataSet();
			string text = "select * from tblEnroll order by " + sortfield;
			OleDbDataAdapter val = new OleDbDataAdapter(text, connection);
			try
			{
				((DbDataAdapter)(object)val).Fill(dataSet, "tblEnroll");
			}
			finally
			{
				((Component)(object)val).Dispose();
			}
			return dataSet;
		}
		finally
		{
			((DbConnection)(object)connection).Close();
			((Component)(object)connection).Dispose();
		}
	}

	public DataSet GetEnrollDatasSql(int machineNumber = 2, List<int> enrollNumbers = null)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			if (enrollNumbers == null)
			{
				enrollNumbers = new List<int>();
			}
			DataSet dataSet = new DataSet();
			string text = string.Format("EXEC dbo.sp_tblEnroll_select @EMachineNumber = {0},@EnrollNumbers = '{1}'", machineNumber, string.Join(",", enrollNumbers));
			SqlDataAdapter val = new SqlDataAdapter(text, ClsConnection.connectionString);
			try
			{
				((DbDataAdapter)(object)val).Fill(dataSet, "tblEnroll");
			}
			finally
			{
				((Component)(object)val).Dispose();
			}
			return dataSet;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return new DataSet();
		}
	}

	public void DeleteDB()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			OleDbConnection connection = GetConnection();
			try
			{
				((DbConnection)(object)connection).Open();
				OleDbCommand val = new OleDbCommand("delete from tblEnroll", connection);
				try
				{
					((DbCommand)(object)val).ExecuteNonQuery();
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				((DbConnection)(object)connection).Close();
			}
			finally
			{
				((IDisposable)connection)?.Dispose();
			}
		}
		catch
		{
		}
	}

	public async Task SaveEnrollsAsync(DataTable dt, CancellationToken token = default(CancellationToken))
	{
		DeleteDB();
		try
		{
			string query = "INSERT INTO tblEnroll (EMachineNumber, EnrollNumber, FingerNumber, Privilige, Password1, FPData) VALUES (@value1, @value2, @value3, @value4, @value5, @value6 );";
			OleDbConnection con = GetConnection();
			try
			{
				((DbConnection)(object)con).Open();
				OleDbCommand cmd = new OleDbCommand(query, con);
				try
				{
					foreach (DataRow row in dt.Rows)
					{
						token.ThrowIfCancellationRequested();
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("@value1", (object)int.Parse(row[0].ToString()));
						cmd.Parameters.AddWithValue("@value2", (object)int.Parse(row[1].ToString()));
						cmd.Parameters.AddWithValue("@value3", (object)int.Parse(row[2].ToString()));
						cmd.Parameters.AddWithValue("@value4", (object)int.Parse(row[3].ToString()));
						cmd.Parameters.AddWithValue("@value5", (object)int.Parse(row[4].ToString()));
						cmd.Parameters.AddWithValue("@value6", row[5]);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					((DbConnection)(object)con).Close();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)con)?.Dispose();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			Exception ex = ex3;
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task SaveEnrollsSqlAsync(DataTable dt, CancellationToken token = default(CancellationToken))
	{
		try
		{
			SqlConnection con = new SqlConnection(ClsConnection.connectionString);
			SqlCommand cmd = new SqlCommand("sp_tblEnroll_Save", con);
			try
			{
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				((DbConnection)(object)con).Open();
				foreach (DataRow row in dt.Rows)
				{
					((DbParameterCollection)(object)cmd.Parameters).Clear();
					cmd.Parameters.AddWithValue("@EMachineNumber", (object)int.Parse(row[0].ToString()));
					cmd.Parameters.AddWithValue("@EnrollNumber", (object)int.Parse(row[1].ToString()));
					cmd.Parameters.AddWithValue("@FingerNumber", (object)int.Parse(row[2].ToString()));
					cmd.Parameters.AddWithValue("@Privilige", (object)int.Parse(row[3].ToString()));
					cmd.Parameters.AddWithValue("@Password1", (object)int.Parse(row[4].ToString()));
					if (row[5].ToString() == "")
					{
						cmd.Parameters.AddWithValue("@FPData", (object)new byte[0]);
					}
					else
					{
						cmd.Parameters.AddWithValue("@FPData", (object)(byte[])row[5]);
					}
					cmd.Parameters.AddWithValue("@CompanyName", (object)row[6].ToString());
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
				((DbConnection)(object)con).Close();
			}
			finally
			{
				((IDisposable)cmd)?.Dispose();
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Log.Error(ex, "");
			throw;
		}
	}

	public void New(string sDatapath)
	{
		mDataPath = sDatapath;
		DataModule = this;
	}
}
