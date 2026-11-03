using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Records;

public record SmsSettingRecord
{
	public string ContextGuid { get; private set; }

	public string HeadTitle { get; set; }

	public string Message { get; set; }

	public PayamakSendStatus? Status { get; set; }

	public bool InsertValue { get; set; }

	public bool EditValue { get; set; }

	public bool SendEnable { get; set; }

	public bool SendStatus { get; set; } = true;


	public List<SmsTemplateRecord> TemplateList { get; set; }

	public SmsSettingRecord(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
