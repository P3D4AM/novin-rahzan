using System;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

[Flags]
internal enum NCryptAlgorithmOperations
{
	None = 0,
	Cipher = 1,
	Hash = 2,
	AsymmetricEncryption = 4,
	SecretAgreement = 8,
	Signature = 0x10,
	RandomNumberGeneration = 0x20
}
