using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Serilog;

namespace TimeAttendanceDAL.Dal.Device;

public class ExcelSeprator
{
	public int col { get; set; }

	public int Row { get; set; }

	public string Data { get; set; }

	public ExcelSeprator(string data)
	{
		try
		{
			Match match = new Regex("\\[(?<RowBumber>[0-9]+),(?<ColNumber>[0-9]+)\\]=(?<Data>[a-zA-Z0-9]+)").Match(data);
			if (match.Success)
			{
				Row = int.Parse(match.Groups["RowBumber"].Value);
				col = int.Parse(match.Groups["ColNumber"].Value);
				Data = match.Groups["Data"].Value;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public static List<ExcelSeprator> GetData(string Data)
	{
		List<ExcelSeprator> list = new List<ExcelSeprator>();
		try
		{
			if (Data != "")
			{
				foreach (string item in Data.Split(";".ToCharArray()).ToList())
				{
					list.Add(new ExcelSeprator(item));
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return list;
	}
}
