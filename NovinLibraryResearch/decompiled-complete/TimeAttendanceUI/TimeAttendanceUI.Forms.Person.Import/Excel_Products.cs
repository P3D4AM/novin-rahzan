using System;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRPerson;

namespace TimeAttendanceUI.Forms.Person.Import;

public class Excel_Products : IHasContext
{
	private TimeAttendanceDAL.Dal.FLRPerson.Person _prd;

	public string ContextGuid { get; set; }

	public Excel_Products(string contextGuid)
	{
		ContextGuid = contextGuid;
		_prd = new TimeAttendanceDAL.Dal.FLRPerson.Person(contextGuid);
	}

	public bool Save()
	{
		try
		{
			if (_prd == null)
			{
				return false;
			}
			if (((PersonBase)_prd).Id <= 0)
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\Import\\Excel_Products.cs", 29);
		}
		return false;
	}
}
