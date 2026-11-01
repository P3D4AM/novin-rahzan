using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PacketParser;

namespace WebSmsStandard;

public class DeliveryRequest : Credential
{
	[JsonIgnore]
	public IEnumerable<long> SmsIds
	{
		get
		{
			string smsId = SmsId;
			if (smsId == null)
			{
				return null;
			}
			return Text.SplitString(smsId, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:")?.Select((string p) => (p == null) ? 0 : Utilities.ParseToLong(p, 0L));
		}
	}

	public string SmsId { get; set; }

	public string LineNumber { get; set; }
}
