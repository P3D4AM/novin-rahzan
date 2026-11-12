using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_COL_ENCRYPTION_OR_MASTER_KEY_SUBTOKENS : OptLookup
{
	public OPT_COL_ENCRYPTION_OR_MASTER_KEY_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[13]
		{
			StringToken.Signature,
			StringToken.ColumnMasterKey,
			StringToken.EncryptionType,
			StringToken.EnclaveComputations,
			StringToken.EncryptedValue,
			StringToken.Encryption,
			StringToken.Value,
			StringToken.Master,
			StringToken.KeyPath,
			StringToken.ColumnEncryptionKey,
			StringToken.KeyStoreType,
			StringToken.Algorithm,
			StringToken.KeyStoreProviderName
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[13]
		{
			891, 886, 897, 890, 894, 892, 895, 885, 889, 896,
			887, 893, 888
		};
	}
}
