using System.Collections.Generic;

namespace Microsoft.Identity.Client;

internal interface IAccountInternal : IAccount
{
	IDictionary<string, string> WamAccountIds { get; }
}
