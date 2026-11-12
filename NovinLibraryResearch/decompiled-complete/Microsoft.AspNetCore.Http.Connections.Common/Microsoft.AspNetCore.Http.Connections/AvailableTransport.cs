using System.Collections.Generic;

namespace Microsoft.AspNetCore.Http.Connections;

public class AvailableTransport
{
	public string? Transport { get; set; }

	public IList<string>? TransferFormats { get; set; }
}
