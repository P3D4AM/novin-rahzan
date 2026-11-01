using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Dal.Reports;

public class SQLParameters
{
	public string Name { get; set; } = "";


	public string FaName { get; set; } = "";


	public SQLParameters(string Name, string FaName)
	{
		this.Name = Name;
		this.FaName = FaName;
	}

	public static List<SQLParameters> GetAll()
	{
		try
		{
			return new List<SQLParameters>
			{
				new SQLParameters("DateM1@", "تاریخ شروع"),
				new SQLParameters("DateM2@", "تاریخ پایان"),
				new SQLParameters("KhedmatID@", "ردیف محل خدمت"),
				new SQLParameters("PersonID@", "ردیف پرسنل"),
				new SQLParameters("ListID@", "ردیف های پرسنل"),
				new SQLParameters("GroupID@", "ردیف گروه پرسنل")
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\SQLParameters.cs", 34);
			return new List<SQLParameters>();
		}
	}
}
