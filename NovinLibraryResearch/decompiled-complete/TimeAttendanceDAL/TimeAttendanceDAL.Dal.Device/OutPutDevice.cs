using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDevices;

namespace TimeAttendanceDAL.Dal.Device;

public class OutPutDevice : IHasContext
{
	public string ContextGuid { get; set; }

	public int ID { get; set; }

	public short DateM { get; set; }

	public short DateSH { get; set; }

	public short DeviceNO { get; set; }

	public string enCoding { get; set; } = "";


	public short PersonNO { get; set; }

	public short Type { get; set; }

	public string Name { get; set; } = "";


	public string Seprator { get; set; } = "";


	public string NextRecord { get; set; } = "";


	public string FormatType { get; set; } = "";


	public short Time { get; set; }

	public bool ST { get; set; } = true;


	public string Header { get; set; } = "";


	public string Data { get; } = "";


	public short HeaderLineNumber { get; set; }

	public OutPutDevice(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public OutPutDevice(string contextGuid, int ID, short DateM, short DateSH, string Name, string Seprator, string NextRecordi, string FormatType, string encoding, short PersonNO, short DeviceNo, short Type, short Time, bool ST, string Header, short HeaderLineNum)
	{
		try
		{
			ContextGuid = contextGuid;
			this.ID = ID;
			this.DateM = DateM;
			this.DateSH = DateSH;
			this.Name = Name;
			this.Seprator = Seprator;
			NextRecord = NextRecordi;
			this.FormatType = FormatType;
			enCoding = encoding;
			this.PersonNO = PersonNO;
			DeviceNO = DeviceNo;
			this.Type = Type;
			this.Time = Time;
			this.ST = ST;
			this.Header = Header;
			HeaderLineNumber = HeaderLineNum;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public OutPutDevice(string contextGuid, SqlDataReader dr)
	{
		try
		{
			ContextGuid = contextGuid;
			LoadData(dr);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				ID = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				DateM = (short)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["DateSH"].ToString() != DBNull.Value.ToString())
			{
				DateSH = (short)((DbDataReader)(object)dr)["DateSH"];
			}
			if (((DbDataReader)(object)dr)["DeviceNO"].ToString() != DBNull.Value.ToString())
			{
				DeviceNO = (short)((DbDataReader)(object)dr)["DeviceNO"];
			}
			if (((DbDataReader)(object)dr)["enCoding"].ToString() != DBNull.Value.ToString())
			{
				enCoding = ((DbDataReader)(object)dr)["enCoding"].ToString();
			}
			if (((DbDataReader)(object)dr)["FormatType"].ToString() != DBNull.Value.ToString())
			{
				FormatType = ((DbDataReader)(object)dr)["FormatType"].ToString();
			}
			if (((DbDataReader)(object)dr)["header"].ToString() != DBNull.Value.ToString())
			{
				Header = ((DbDataReader)(object)dr)["header"].ToString();
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["NextRecord"].ToString() != DBNull.Value.ToString())
			{
				NextRecord = ((DbDataReader)(object)dr)["NextRecord"].ToString();
			}
			if (((DbDataReader)(object)dr)["PersonNO"].ToString() != DBNull.Value.ToString())
			{
				PersonNO = (short)((DbDataReader)(object)dr)["PersonNO"];
			}
			if (((DbDataReader)(object)dr)["Seprator"].ToString() != DBNull.Value.ToString())
			{
				Seprator = ((DbDataReader)(object)dr)["Seprator"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				ST = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Time"].ToString() != DBNull.Value.ToString())
			{
				Time = (short)((DbDataReader)(object)dr)["Time"];
			}
			if (((DbDataReader)(object)dr)["Type"].ToString() != DBNull.Value.ToString())
			{
				Type = (short)((DbDataReader)(object)dr)["Type"];
			}
			if (((DbDataReader)(object)dr)["HeaderLineNumber"].ToString() != DBNull.Value.ToString())
			{
				HeaderLineNumber = (short)((DbDataReader)(object)dr)["HeaderLineNumber"];
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)ID);
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
					cmd.Parameters.AddWithValue("Seprator", (object)Seprator);
					cmd.Parameters.AddWithValue("NextRecord", (object)NextRecord);
					cmd.Parameters.AddWithValue("FormatType", (object)Text.FixString(FormatType));
					cmd.Parameters.AddWithValue("enCoding", (object)Text.FixString(enCoding));
					cmd.Parameters.AddWithValue("PersonNO", (object)PersonNO);
					cmd.Parameters.AddWithValue("DateSH", (object)DateSH);
					cmd.Parameters.AddWithValue("DateM", (object)DateM);
					cmd.Parameters.AddWithValue("DeviceNO", (object)DeviceNO);
					cmd.Parameters.AddWithValue("Time", (object)Time);
					cmd.Parameters.AddWithValue("Type", (object)Type);
					cmd.Parameters.AddWithValue("St", (object)ST);
					cmd.Parameters.AddWithValue("Header", (object)Header);
					cmd.Parameters.AddWithValue("HeaderLineNumber", (object)HeaderLineNumber);
					await ((DbConnection)(object)cn).OpenAsync(token);
					((DbCommand)(object)cmd).CommandText = "[dbo].[sp_OutputDevice_Save]";
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						ID = Convert.ToInt16(obj.ToString());
					}
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\OutPutDevice.cs", 145);
			throw;
		}
	}

	public static async Task<List<OutPutDevice>> GetByExtentionAsync(string contextGuid, string FileExtention, CancellationToken token = default(CancellationToken))
	{
		List<OutPutDevice> ret = new List<OutPutDevice>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_OutputDevice_SelectRowBYType]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("FormatType", (object)FileExtention);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new OutPutDevice(contextGuid, dr));
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public List<CLSLOG> Process(string Data, short pDeviceNO)
	{
		try
		{
			List<CLSLOG> list = new List<CLSLOG>();
			List<string> list2 = new List<string>();
			list2.Add(NextRecord);
			List<string> list3 = Data.Split(list2.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
			if (list3.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < list3.Count; i++)
			{
				if (i >= HeaderLineNumber)
				{
					CLSLOG cLSLOG = new CLSLOG(list3[i], Seprator, PersonNO, DateM, Time, Type, pDeviceNO, DateSH);
					if (cLSLOG.PersonNo > 0)
					{
						list.Add(cLSLOG);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return new List<CLSLOG>();
	}

	public static async Task<OutPutDevice> FindStyleAsync(string contextGuid, string fileName, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				FileInfo fileInfo = new FileInfo(fileName);
				string extention = fileInfo.Extension.Replace(".", "").ToLower();
				List<OutPutDevice> list = await GetByExtentionAsync(contextGuid, extention, token);
				if (extention == "txt" && TimeAttendanceDAL.Utility.Utility.GetEncoding(fileName) == Encoding.ASCII)
				{
					list = list.Where((OutPutDevice p) => p.enCoding.ToLower() == "ascii").ToList();
				}
				foreach (OutPutDevice item in list)
				{
					if (extention == "xls")
					{
						ExcelFile excelFile = new ExcelFile(contextGuid, fileName);
						List<ExcelSeprator> data = ExcelSeprator.GetData(item.Header);
						if (data.Count == 0)
						{
							return item;
						}
						bool flag = true;
						foreach (ExcelSeprator item2 in data)
						{
							if (excelFile.GetValue(item2.Row, item2.col) != item2.Data)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return item;
						}
						continue;
					}
					if (item.HeaderLineNumber == 0)
					{
						string text = File.ReadAllText(fileName, (item.enCoding.ToLower() == "ascii") ? Encoding.ASCII : Encoding.UTF8);
						if (text.Split(item.NextRecord.ToCharArray()).Length == 0)
						{
							continue;
						}
						string input = text.Split(item.NextRecord.ToCharArray())[0];
						MatchCollection matchCollection = new Regex(item.Header).Matches(input);
						string text2 = "";
						foreach (object item3 in matchCollection)
						{
							text2 += item3.ToString();
						}
						if (matchCollection.Count > 0)
						{
							return item;
						}
						continue;
					}
					string text3 = File.ReadAllText(fileName, (item.enCoding.ToLower() == "ascii") ? Encoding.ASCII : Encoding.UTF8);
					if (text3.Split(item.NextRecord.ToCharArray()).Length != 0 && text3.Split(item.NextRecord.ToCharArray())[0].Contains("T300 Excel") && item.Name != "T300 Excel")
					{
						continue;
					}
					string text4 = "";
					if (item.HeaderLineNumber > 0)
					{
						string[] array = text3.Split(item.NextRecord.ToCharArray());
						if (array.Length < item.HeaderLineNumber)
						{
							continue;
						}
						text4 = array[item.HeaderLineNumber - 1];
					}
					text4 = text4.Replace("\r", "");
					text4 = text4.TrimEnd(new char[1] { '\t' });
					if (new Regex(item.Header.TrimEnd(new char[1] { '\t' })).Matches(text4).Count > 0)
					{
						return item;
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("The file or directory is corrupted and unreadable") || ex.Message.Contains("Could not find a part of the path"))
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowError("امکان باز کردن فایل \r\n " + fileName + " وجود ندارد", contextGuid);
				return null;
			}
			Log.Error(ex, "");
		}
		return null;
	}

	public static async Task<ReturnedSaveFuncInfo> SetDefaultsAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await new OutPutDevice(contextGuid, 1, -1, 6, "دستگاه های قدیمی", "\t", "\n", "txt", "Unicode", 2, 1, 5, -1, ST: true, "No\tTMNo\tEnNo\tName\tGMNo\tMode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 2, 8, -1, "دستگاه های جدید", "\t", "\n", "csv", "Unicode", 2, 1, 4, -1, ST: true, "\ufeffNo\tDN\tUID              \tName           \tStatus\tAction\tAPB\tJobCode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 3, -1, 9, "EC160", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tTMNo\tEnNo\tName\t\tGMNo\tMode\tIN/OUT\tAntipass\tDaiGong\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 4, 9, -1, "دستگاه های جدید(تصویری)", "\t", "\n", "txt", "Unicode", 2, 1, 5, -1, ST: true, "No\tTMNo\tEnNo\tName\tGMNo\tMode\tIn/Out\tAntipass\tProxyWork\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 5, 6, -1, "دستگاه های قدیمی(Etc)", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tTMNo\tEnNo\tName\t\tGMNo\tMode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 6, 6, -1, "hysoon", "\t", "\n", "txt", "ASCII", 2, 1, 4, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\tMode\tIOMd\tDateTime\t", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 7, 6, -1, "hysoon2", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\t\tMode\tIOMd\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 8, 6, -1, "hysoon3", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\tMode\tIOMd\tDateTime", 2).SaveAsync(token);
			await new OutPutDevice(contextGuid, 9, -1, 9, "NP2815_year95", "\t", "\n", "txt", "ASCII", 2, 1, -1, -1, ST: true, "No\tTMNo\tEnNo\tName\t\tGMNo\tMode\tIN/OUT\tAntipass\tDaiGong\tDateTime\tTR\t", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 10, 1, -1, "ZK_year95", "\t", "\n", "dat", "ASCII", 0, 2, 3, -1, ST: true, "", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 11, 1, -1, "NP620", ";", "\n", "xls", "excel", 0, -1, 2, 3, ST: true, "", -1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 12, 6, -1, "GLogData", "\t", "\n", "txt", "ASCII", 2, 1, -1, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\tMode\tIOMd\tDateTime\t", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 13, 9, -1, "S200", "\t", "\n", "txt", "ASCII", 2, 1, -1, -1, ST: true, "No\tTMNo\tEnNo\tName\t\tGMNo\tMode\tIN/OUT\tAntipass\tDaiGong\tDateTime\tTR\t", 5).SaveAsync(token);
			await new OutPutDevice(contextGuid, 14, 8, -1, "N1", "\t", "\n", "csv", "Unicode", 2, 1, 4, -1, ST: true, "No\tDN\tUID(\\ )*              \tName(\\ )*           \tStatus\tAction\tAPB\tJobCode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 15, 1, -1, "new", "\t", "\n", "dat", "Unicode", 0, 2, 4, -2, ST: true, "(?<PersonNo>[0-9]+)\t(?<DateSH>[0-9-]+) (?<Time>[0-9:]+)\t(?<DeviceNo>[0-9]+)\t[0-9]+\t[0-9]+\t[0-9]+", 0).SaveAsync(token);
			await new OutPutDevice(contextGuid, 16, 8, -1, "دستگاه های جدید N1", "\t", "\n", "txt", "Unicode", 2, 1, 4, -1, ST: true, "\ufeffNo\tDN\tUID              \tName           \tStatus\tAction\tAPB\tJobCode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 17, 6, -1, "NP586", "\t", "\n", "txt", "Unicode", 2, 1, 5, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\tMode\tIOMd\tDateTime\t", 2).SaveAsync(token);
			await new OutPutDevice(contextGuid, 18, -1, 4, "5YOA", "\t", "\n", "txt", "txt", 2, 1, -1, 5, ST: true, "No\tMchn\tEnNo\t\tName\t\tDateTime\t", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 19, 10, -1, "T300 Excel", "\t", "\n", "txt", "ASCII", 2, 1, -1, -1, ST: true, "No\tTMNo\tEnNo\tName\t\tGMNo\tMode\tIN/OUT\tAntipass\tDaiGong\tDateTime\tTR\t", 5).SaveAsync(token);
			await new OutPutDevice(contextGuid, 20, 1, -1, "خروجی enc", "\t", "\n", "enc", "Unicode", 0, 1, -1, -1, ST: true, "\\ *\\d+\t[0-9-]+\\ [0-9:]+\t\\d+\t\\d+\t\\d+\t\\d+", 0).SaveAsync(token);
			await new OutPutDevice(contextGuid, 21, 5, -1, "Np745", "\t", "\n", "txt", "ASCII", 2, 1, 4, -1, ST: true, "No\tDevId\tUserId\t\tUName\t\t\tVerify\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 22, 6, -1, "755", "\t", "\n", "txt", "ASCII", 2, 1, 4, -1, ST: true, "No\tTMNo\tEnNo\tName\tINOUT\tMode\tDateTime", 1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 23, 6, -1, "761P-771", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tMchn\tEnNo\t\tName\t\tMode\t\tIOMd\tDateTime", 2).SaveAsync(token);
			await new OutPutDevice(contextGuid, 24, 1, -1, "ZK_year1404", "\t", "\n", "dat", "ASCII", 0, -1, 3, -1, ST: true, "", -1).SaveAsync(token);
			await new OutPutDevice(contextGuid, 25, 6, -1, "761N", "\t", "\n", "txt", "ASCII", 2, 1, 5, -1, ST: true, "No\tMchn\t\tEnNo\tName\t\tMode\tIOMd\tDateTime", 2).SaveAsync(token);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue((ReturnedState)2, ex.Message, "", "SetDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\OutPutDevice.cs", 429);
			Log.Error(ex, "");
		}
		return ret;
	}
}
