using System.Security.Cryptography;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal interface ICngAlgorithm
{
	CngProvider Provider { get; }
}
