using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;

namespace TimeAttendanceDAL.Dal.Device;

public class ExcelFile : IHasContext
{
	private readonly DataSet _dsExcelFile = new DataSet();

	private readonly string _deviceName = "";

	public int RowCount { get; set; }

	public int ColCount { get; set; }

	public string ContextGuid { get; set; }

	public ExcelFile(string contextGuid, string fileName)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo val = ReadExcelFile(fileName);
			if (val.HasError)
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, "در هنگام خواندن فایل اکسل با خطا مواجه شدیم\r\n" + val.ErrorMessage, "", 3);
			}
			else if (_dsExcelFile.Tables["Shift Table$"] != null && _dsExcelFile.Tables["Attendance Statistic Table$"] != null)
			{
				_deviceName = "NP620";
			}
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

	public string GetValue(int row, int col, string tableNo = "")
	{
		try
		{
			DataRow dataRow = ((!(tableNo == "")) ? _dsExcelFile.Tables[tableNo].Rows[row] : _dsExcelFile.Tables[0].Rows[row]);
			return dataRow[col].ToString();
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

	public string ToString(string seprator, string nextRecord)
	{
		try
		{
			string text = "";
			if (_deviceName == "NP620")
			{
				text = ReadExcelFileAsNp620(seprator, nextRecord);
			}
			else if (_dsExcelFile != null && _dsExcelFile.Tables.Count > 0)
			{
				foreach (DataRow row in _dsExcelFile.Tables[0].Rows)
				{
					foreach (DataColumn column in _dsExcelFile.Tables[0].Columns)
					{
						text = text + row[column].ToString() + seprator;
					}
				}
			}
			return text;
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

	private string ReadExcelFileAsNp620(string seprator, string nextRecord)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		string text2 = "";
		try
		{
			text2 = GetValue(1, 0, "Shift Table$");
			text2 = TextManager.Instance.RemoveNoNumbers(text2, "");
			int num = int.Parse(text2.Substring(0, 4));
			if ((num < 2020 || num > 2025) ? true : false)
			{
				FMessageBox.Show("فرمت تاریخ در شیت اول فایل اکسل نادرست است. ابتدا بایستی سال بعد ماه و بعد روز باشد");
				return "";
			}
			int month = int.Parse(text2.Substring(4, 2));
			int day = int.Parse(text2.Substring(6, 2));
			DateTime dateTime = new DateTime(num, month, day);
			for (int i = 0; i < _dsExcelFile.Tables.Count; i++)
			{
				if (_dsExcelFile.Tables[i].TableName == "Shift Table$" || _dsExcelFile.Tables[i].TableName == "Attendance Statistic Table$")
				{
					continue;
				}
				for (int j = 0; j < 3; j++)
				{
					string value = GetValue(4, j * 16 + 10, _dsExcelFile.Tables[i].TableName);
					if (value == "")
					{
						break;
					}
					DateTime dateTime2 = dateTime;
					for (int k = 12; k < _dsExcelFile.Tables[i].Rows.Count && !string.IsNullOrEmpty(GetValue(k, 0, _dsExcelFile.Tables[i].TableName)); k++)
					{
						for (int l = -1; l < 14; l += 5)
						{
							string text3 = "";
							text3 = GetValue(k, j * 16 + 2 + l, _dsExcelFile.Tables[i].TableName);
							if (text3 != "")
							{
								text = text + value + seprator;
								text = text + dateTime2.ToString("yyyy/MM/dd") + seprator;
								text = text + "1" + seprator;
								text = text + text3 + seprator;
								text += nextRecord;
							}
							text3 = GetValue(k, j * 16 + 2 + l + 2, _dsExcelFile.Tables[i].TableName);
							if (text3 != "")
							{
								text = text + value + seprator;
								text = text + dateTime2.ToString("yyyy/MM/dd") + seprator;
								text = text + "2" + seprator;
								text = text + text3 + seprator;
								text += nextRecord;
							}
						}
						dateTime2 = dateTime2.AddDays(1.0);
					}
				}
			}
			return text;
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

	private ReturnedSaveFuncInfo ReadExcelFile(string fileName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		try
		{
			OleDbConnection val = null;
			val = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + fileName + "; Extended Properties='Excel 12.0;HDR=NO;IMEX=1;';");
			if (val != null)
			{
				((DbConnection)(object)val).Open();
				DataTable oleDbSchemaTable = val.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, (object[])null);
				OleDbCommand val2 = new OleDbCommand();
				val2.Connection = val;
				((DbCommand)(object)val2).CommandType = CommandType.Text;
				if (oleDbSchemaTable != null)
				{
					foreach (DataRow row in oleDbSchemaTable.Rows)
					{
						string text = row[2].ToString().Replace("'", "");
						if (!(text == ""))
						{
							((DbCommand)(object)val2).CommandText = "SELECT * FROM [" + text + "]";
							((DbDataAdapter)new OleDbDataAdapter(val2)).Fill(_dsExcelFile, text);
						}
					}
				}
			}
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
		return result;
	}
}
