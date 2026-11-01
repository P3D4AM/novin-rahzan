using System.Collections.Generic;

namespace WebSmsStandard;

public class SmsModelV2 : SmsModelBase
{
	public List<string> Numbers { get; set; }

	public List<string> Messages { get; set; }
}
