using System.Security;
using System.Security.Cryptography;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal interface ICngAsymmetricAlgorithm : ICngAlgorithm
{
	CngKey Key
	{
		[SecurityCritical]
		get;
	}
}
