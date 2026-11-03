using System;
using PacketParser.Entities;

namespace PacketParser.SiteManagementApiModel;

public class LastSyncCrmModel
{
	public Guid Tafsil { get; set; }

	public DateTime PassedTime { get; set; }

	public LastSyncCrmModel(Guid TafsilCode)
	{
		try
		{
			Tafsil = TafsilCode;
			PassedTime = DateTime.Now.AddDays(-5.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SiteManagementApiModel\\LastSyncCrmModel.cs", 18);
		}
	}

	public LastSyncCrmModel()
	{
		try
		{
			PassedTime = DateTime.Now.AddDays(-5.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SiteManagementApiModel\\LastSyncCrmModel.cs", 27);
		}
	}
}
