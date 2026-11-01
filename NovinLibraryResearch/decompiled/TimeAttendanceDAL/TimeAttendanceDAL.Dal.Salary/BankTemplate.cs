using System;
using System.Data;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Dal.Salary;

public class BankTemplate
{
	public int Id { get; set; }

	public string FileName { get; set; }

	public string FileExtention { get; set; }

	public string Encoding { get; set; }

	public string Title { get; set; }

	public string Template { get; set; }

	public string RowDescription { get; set; }

	public bool St { get; set; }

	public BankTemplate(IDataRecord dr)
	{
		LoadData(dr);
	}

	private void LoadData(IDataRecord dr)
	{
		try
		{
			if (dr["Id"].ToString() != "")
			{
				Id = (int)dr["Id"];
			}
			if (dr["FileName"].ToString() != "")
			{
				FileName = dr["FileName"].ToString();
			}
			if (dr["FileExtention"].ToString() != "")
			{
				FileExtention = dr["FileExtention"].ToString().Trim();
			}
			if (dr["Encoding"].ToString() != "")
			{
				Encoding = dr["Encoding"].ToString();
			}
			if (dr["Title"].ToString() != "")
			{
				Title = dr["Title"].ToString();
			}
			if (dr["Template"].ToString() != "")
			{
				Template = dr["Template"].ToString();
			}
			if (dr["RowDescription"].ToString() != "")
			{
				RowDescription = dr["RowDescription"].ToString();
			}
			if (dr["St"].ToString() != "")
			{
				St = (bool)dr["St"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "BankTemplate.LoadData", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\BankTemplate.cs", 42);
		}
	}
}
