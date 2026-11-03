using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Error")]
public class Error : IErrorHandler, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public string? AndroidIme { get; set; }

	public string? ClassName { get; set; }

	public string? CpuSerial { get; set; }

	public DateTime Date { get; set; }

	public string? Description { get; set; }

	public string? ExceptionError { get; set; }

	public string? ExceptionMessage { get; set; }

	public PlaceError ExceptionPlace { get; set; }

	public string? ExceptionType { get; set; }

	public bool Fatal { get; set; }

	public string? FuncName { get; set; }

	public int GroupingID { get; set; }

	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public string? HardSerial { get; set; }

	public long LKSerial { get; set; }

	public int RefrencedID { get; set; }

	public string? ScreenShot { get; set; }

	public ENSource Source { get; set; }

	public string? StackTrace { get; set; }

	public string? FlowStackTrace { get; set; }

	public string? Version { get; set; }

	public string? Ip { get; set; }

	public string? Path { get; set; }

	public string? LoggerVersion { get; set; }

	public string? Username { get; set; }

	public void StartLog(Exception ex, string description = "", string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
	{
		using (new JaegerMonitor("StartLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Error.cs"))
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "StartLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Entities\\Error.cs", 49);
			}
		}
	}
}
