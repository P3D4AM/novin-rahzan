using System;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class LastErrorCrmModel
{
	public Guid Tafsil { get; set; }

	public DateTime PassedTime { get; set; }

	public LastErrorCrmModel(Guid tafsilCode)
	{
		try
		{
			Tafsil = tafsilCode;
			PassedTime = DateTime.Now.AddDays(-5.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SiteManagementApiModel\\LastErrorCrmModel.cs", 31);
		}
	}

	public LastErrorCrmModel()
	{
		try
		{
			PassedTime = DateTime.Now.AddDays(-5.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SiteManagementApiModel\\LastErrorCrmModel.cs", 42);
		}
	}
}
