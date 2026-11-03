using System;
using System.Collections.Generic;
using PacketParser.Entities.Department;

namespace PacketParser.Entities.CRM.CallCenter;

public class WebCall
{
	public Guid Guid { get; set; }

	public bool IsOutboundCall { get; set; }

	public string CustomerNumber { get; set; }

	public string AgentNumber { get; set; }

	public DateTime DateCreation { get; set; }

	public string CreationTime
	{
		get
		{
			try
			{
				string text = DateCreation.Minute.ToString();
				if (DateCreation.Minute < 10)
				{
					text = "0" + text;
				}
				string text2 = DateCreation.Hour.ToString();
				if (DateCreation.Hour < 10)
				{
					text2 = "0" + text2;
				}
				return text2 + ":" + text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreationTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\CallCenter\\WebCall.cs", 27);
			}
			return "";
		}
	}

	public DateTime CustomerStartDateM { get; set; } = DateTime.Now;


	public DateTime? CustomerEndDateM { get; set; }

	public DateTime? AgentStartDateM { get; set; }

	public DateTime? AgentEndDateM { get; set; }

	public DateTime? QueueeStartDateM { get; set; }

	public DateTime? QueueeEndDateM { get; set; }

	public int QueueeId { get; set; }

	public EnCallCenterCallStatus Status { get; set; }

	public int Version { get; set; }

	public DateTime LastUpdateDateM { get; set; } = DateTime.Now;


	public short DepId { get; set; } = 3;


	public WebCus Customer { get; set; }

	public List<WebLkInfo> LkInfoes { get; set; }

	public WebDepartment DepInfo { get; set; }

	public List<WebCallDetail> DetailList { get; set; }
}
