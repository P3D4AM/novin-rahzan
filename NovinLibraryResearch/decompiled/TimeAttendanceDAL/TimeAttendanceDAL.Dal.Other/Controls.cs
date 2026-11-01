using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Dal.Other;

public class Controls
{
	public string Name { get; set; }

	public string FaName { get; set; }

	public Controls(string fName, string fFaName)
	{
		Name = fName;
		FaName = fFaName;
	}

	public static async Task<List<Controls>> GetAllAsync()
	{
		try
		{
			return new List<Controls>
			{
				new Controls("ucDate1", Utilities.GetTranslate("[تاریخ]")),
				new Controls("ucPerson1", Utilities.GetTranslate("[پرسنل]"))
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\Controls.cs", 34);
			return new List<Controls>();
		}
	}
}
