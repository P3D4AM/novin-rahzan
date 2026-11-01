using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceDevices;

public class CLSLOG
{
	public DateTime DateM { get; private set; } = DateTime.Now;


	public int NO { get; private set; }

	public int TerminalNo { get; private set; }

	public int PersonNo { get; private set; }

	public short Mode { get; private set; }

	public CLSLOG(string rowData, string seprator, short personNo, short dateM, short time, short type, short deviceNo, short indexDateSh)
	{
		LoadData(rowData, seprator, personNo, dateM, time, type, deviceNo, indexDateSh);
	}

	private void LoadData(string rowData, string seprator, short indexPersonNo, short indexDateM, short indexTime, short indexType, short deviceNo, short indexDateSh)
	{
		int num = 0;
		try
		{
			num = 1;
			rowData = Text.FixString(rowData.Replace("\0", ""));
			num = 5;
			List<string> list = new List<string>();
			num = 6;
			list.Add(seprator);
			num = 7;
			List<string> list2 = rowData.Split(list.ToArray(), StringSplitOptions.None).ToList();
			num = 8;
			int result = 0;
			if (indexPersonNo >= 0 && indexPersonNo < list2.Count)
			{
				num = 9;
				int.TryParse(list2[indexPersonNo], out result);
				num = 10;
				PersonNo = result;
			}
			num = 11;
			if (indexType >= 0 && indexType < list2.Count)
			{
				num = 12;
				int.TryParse(list2[indexType], out result);
				num = 13;
				Mode = (short)result;
			}
			num = 14;
			TerminalNo = int.Parse(deviceNo.ToString());
			num = 15;
			string text = "";
			if (indexTime >= 0 && indexTime < list2.Count)
			{
				text = list2[indexTime];
			}
			num = 16;
			if (indexDateM >= 0 && indexDateM < list2.Count)
			{
				num = 17;
				text = list2[indexDateM] + " " + text;
			}
			num = 19;
			if (indexDateSh >= 0 && indexDateSh < list2.Count)
			{
				num = 20;
				text = list2[indexDateSh] + " " + text;
			}
			DateM = Utilities.ParsToDateFromShOrMiDate(text, (DateTime?)null);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "private void LoadData(string rowData, string se, short indexPersonNo, short indexDateSh, short indexDateM, line: " + num, "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\CLSLOG.cs", 89);
		}
	}
}
