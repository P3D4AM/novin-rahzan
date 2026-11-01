using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Stimulsoft.Report;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.Reports;

[Serializable]
public class RPTGen : IHasContext
{
	private List<RPTGenDet> _LstRPTGenDet;

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public int ID { get; set; }

	public string Title { get; set; } = "";


	public string Description { get; set; } = "";


	public string RPT { get; set; } = "";


	public string SQLQuery { get; set; } = "";


	public DateTime DateM1 { get; set; } = DateTime.Now;


	public DateTime DateM2 { get; set; } = DateTime.Now;


	public int personID { get; set; }

	public int personNO { get; set; }

	public short KhedmatID { get; set; }

	public short GroupID { get; set; }

	public List<int> ListID { get; set; } = new List<int>();


	public async Task<List<RPTGenDet>> GetLstRPTGenDetAsync(CancellationToken token = default(CancellationToken))
	{
		if (_LstRPTGenDet == null)
		{
			_LstRPTGenDet = await RPTGenDet.GetAllAsync(ContextGuid, ID, token);
		}
		return _LstRPTGenDet;
	}

	public void SetLstRPTGenDet(List<RPTGenDet> value)
	{
		_LstRPTGenDet = value;
	}

	public RPTGen()
	{
	}

	public RPTGen(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<RPTGen> GetAsync(string contextGuid, int pID, CancellationToken token = default(CancellationToken))
	{
		RPTGen ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_ReportGeneratorHozor_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pID);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(contextGuid, dr);
						}
						((DbConnection)(object)cn).Close();
						if (ret != null)
						{
							RPTGen rPTGen = ret;
							rPTGen._LstRPTGenDet = await RPTGenDet.GetAllAsync(contextGuid, ret.ID);
						}
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
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 88);
			throw;
		}
	}

	private static RPTGen LoadData(string contextGuid, SqlDataReader dr)
	{
		RPTGen rPTGen = new RPTGen(contextGuid);
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				rPTGen.ID = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				rPTGen.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["SQLQuery"].ToString() != DBNull.Value.ToString())
			{
				rPTGen.SQLQuery = ((DbDataReader)(object)dr)["SQLQuery"].ToString();
			}
			if (((DbDataReader)(object)dr)["Title"].ToString() != DBNull.Value.ToString())
			{
				rPTGen.Title = ((DbDataReader)(object)dr)["Title"].ToString();
			}
			if (((DbDataReader)(object)dr)["RPT"].ToString() != DBNull.Value.ToString())
			{
				rPTGen.RPT = ((DbDataReader)(object)dr)["RPT"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 107);
			throw;
		}
		return rPTGen;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_ReportGeneratorHozor_Save", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)ID);
					cmd.Parameters.AddWithValue("Title", (object)Text.FixString(Title));
					cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
					cmd.Parameters.AddWithValue("SQLQuery", (object)SQLQuery);
					cmd.Parameters.AddWithValue("RPT", (object)RPT);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						await SaveDetailsAsync(Convert.ToInt16(obj.ToString()));
					}
					((DbConnection)(object)cn).Close();
					((Component)(object)cmd).Dispose();
					return true;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 138);
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_ReportGeneratorHozor_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)ID);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					return true;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 161);
			throw;
		}
	}

	public static async Task<List<RPTGen>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		List<RPTGen> ret = new List<RPTGen>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ReportGeneratorHozor_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(contextGuid, dr));
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 185);
			throw;
		}
	}

	public List<string> ListQueries()
	{
		string sQLQuery = SQLQuery;
		List<string> list = new List<string>();
		if (sQLQuery == null)
		{
			return list;
		}
		if (string.IsNullOrEmpty(sQLQuery.Trim()))
		{
			return list;
		}
		string[] separator = new string[4] { "go\r\n", " go\r\n", " go\r\n", " go\r\n" };
		sQLQuery = sQLQuery.Replace("GO", "go").Replace("Go", "go").Replace("gO", "go");
		string[] array = sQLQuery.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array)
		{
			list.Add(Text.FixString(text, true).Replace("×", "*"));
		}
		return list;
	}

	public async Task SaveDetailsAsync(int id, CancellationToken token = default(CancellationToken))
	{
		await RPTGenDet.DeleteAsync(ContextGuid, id, token);
		foreach (RPTGenDet item in await GetLstRPTGenDetAsync(token))
		{
			item.RepID = id;
			await item.SaveAsync(token);
		}
	}

	public static string Serialize(RPTGen rpt)
	{
		string result = "";
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(rpt.GetType());
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, rpt);
			result = stringWriter.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Serialize", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 234);
		}
		return result;
	}

	public static async Task<RPTGen> DeserializeALLAsync(string contextGuid, string data, CancellationToken token = default(CancellationToken))
	{
		int line = 1;
		RPTGen temp2 = new RPTGen(contextGuid);
		try
		{
			line = 2;
			XmlSerializer xmlSerializer = new XmlSerializer(temp2.GetType());
			line = 3;
			StringReader textReader = new StringReader(data);
			line = 4;
			temp2 = (RPTGen)xmlSerializer.Deserialize(textReader);
			temp2.ContextGuid = contextGuid;
			List<RPTGenDet> list = await temp2.GetLstRPTGenDetAsync(token);
			if (list != null)
			{
				foreach (RPTGenDet item in list)
				{
					item.ContextGuid = contextGuid;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $"Error in Line {line}", "DeserializeALLAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 259);
			throw;
		}
		return temp2;
	}

	public static async Task<bool> SerializeALLAsync(string contextGuid)
	{
		List<RPTGen> obj = await GetAllAsync(contextGuid);
		string text = Application.StartupPath + "\\ReportsHozor";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		foreach (RPTGen item in obj)
		{
			File.WriteAllText(text + "\\RPTH" + item.ID + ".xml", Serialize(item));
		}
		return true;
	}

	public static async Task<bool> DeserializeALLAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		string path = Application.StartupPath + "\\ReportsHozor";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		string[] files = Directory.GetFiles(path, "*.xml");
		for (int i = 0; i < files.Length; i++)
		{
			string data = File.ReadAllText(files[i]);
			await (await DeserializeALLAsync(contextGuid, data, token)).SaveAsync(token);
		}
		return true;
	}

	public async Task ShowAsync(bool show, CancellationToken token = default(CancellationToken))
	{
		try
		{
			StiReport sti = new StiReport();
			if (RPT != "")
			{
				sti.LoadFromString(RPT);
			}
			StiReport val = sti;
			val.RegData("منبع داده", await GetDSAsync(token));
			sti.Save(Application.StartupPath + "\\temp.rpt");
			if (show)
			{
				sti.Show();
				return;
			}
			sti.Design();
			RPT = sti.SaveToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ShowAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 308);
			throw;
		}
	}

	private async Task<DataSet> GetDSAsync(CancellationToken token = default(CancellationToken))
	{
		DataSet ds = new DataSet();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					int index = 0;
					await ((DbConnection)(object)cn).OpenAsync(token);
					ds.Tables.Clear();
					await FillMaliYearDefaultsAsync(ds, cmd, token);
					foreach (string item in ListQueries())
					{
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("@DateM1", (object)DateM1);
						cmd.Parameters.AddWithValue("@DateM2", (object)DateM2);
						cmd.Parameters.AddWithValue("@KhedmatID", (object)KhedmatID);
						cmd.Parameters.AddWithValue("@PersonID", (object)personID);
						cmd.Parameters.AddWithValue("@ListID", (object)string.Join(",", ListID));
						cmd.Parameters.AddWithValue("@GroupID", (object)GroupID);
						((DbCommand)(object)cmd).CommandText = item;
						ds.Tables.Add("tbl" + index);
						((DbDataAdapter)new SqlDataAdapter(cmd)).Fill(ds.Tables["tbl" + index]);
						index++;
					}
					((DbConnection)(object)cn).Close();
					return ds;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetDSAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 344);
			throw;
		}
	}

	private async Task FillMaliYearDefaultsAsync(DataSet ds, SqlCommand cmd, CancellationToken token)
	{
		try
		{
			DataTable dataTable = ds.Tables.Add("مشخصات شرکت");
			dataTable.Columns.Add("عنوان شرکت");
			dataTable.Columns.Add("کد اقتصادی");
			dataTable.Columns.Add("تلفن");
			dataTable.Columns.Add("آدرس");
			dataTable.Columns.Add("فکس");
			dataTable.Columns.Add("شرح شرکت");
			dataTable.Columns.Add("شماره ثبت شرکت");
			DataRow dr = dataTable.Rows.Add();
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			dr.SetField("عنوان شرکت", timeAttendanceSettings.SaleKari.TitleSalekari);
			dr.SetField("کد اقتصادی", timeAttendanceSettings.SaleKari.CodeEkhtesasi);
			dr.SetField("تلفن", timeAttendanceSettings.SaleKari.Tel);
			dr.SetField("آدرس", timeAttendanceSettings.SaleKari.Address);
			dr.SetField("فکس", timeAttendanceSettings.SaleKari.Fax);
			dr.SetField("شرح شرکت", timeAttendanceSettings.SaleKari.Desc);
			dr.SetField("شماره ثبت شرکت", timeAttendanceSettings.SaleKari.SabtNo);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillMaliYearDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGen.cs", 374);
		}
	}

	private SqlCommand SetParameters(SqlCommand cmd)
	{
		((DbParameterCollection)(object)cmd.Parameters).Clear();
		cmd.Parameters.AddWithValue("@DateM1", (object)DateM1);
		cmd.Parameters.AddWithValue("@DateM2", (object)DateM2);
		cmd.Parameters.AddWithValue("@KhedmatID", (object)KhedmatID);
		cmd.Parameters.AddWithValue("@PersonID", (object)personID);
		cmd.Parameters.AddWithValue("@ListID", (object)ListID);
		cmd.Parameters.AddWithValue("@GroupID", (object)GroupID);
		return cmd;
	}
}
