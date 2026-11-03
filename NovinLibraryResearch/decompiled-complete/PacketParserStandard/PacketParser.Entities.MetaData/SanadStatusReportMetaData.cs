using System;
using System.Collections.Generic;

namespace PacketParser.Entities.MetaData;

public class SanadStatusReportMetaData
{
	public List<int> NextNodes { get; set; }

	public int StartNode { get; set; }

	public Guid? ResourceGuid { get; set; }

	public string ForeColor { get; set; }

	public short ErrorPriod { get; set; }

	public SanadStatusReportSms SmsManager { get; set; }
}
