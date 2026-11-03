using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PacketParser.Entities.Accounting;

public class SmsParserTemplate
{
	[Obsolete("please use from RegexNumbers", true)]
	public string RegexNumber { get; set; }

	public Guid Guid { get; set; }

	public List<string> RegexNumbers { get; set; }

	public bool IsReception { get; set; }

	public string StrRegex { get; set; }

	public string BankName { get; set; }

	[JsonIgnore]
	public string TypeTransaction
	{
		get
		{
			if (!IsReception)
			{
				return "برداشت";
			}
			return "واریز";
		}
	}

	public string sampleSms { get; set; }
}
