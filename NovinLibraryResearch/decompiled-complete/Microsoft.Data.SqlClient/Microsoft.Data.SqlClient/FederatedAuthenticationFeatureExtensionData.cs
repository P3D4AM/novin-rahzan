namespace Microsoft.Data.SqlClient;

internal class FederatedAuthenticationFeatureExtensionData
{
	internal TdsEnums.FedAuthLibrary libraryType;

	internal bool fedAuthRequiredPreLoginResponse;

	internal SqlAuthenticationMethod authentication;

	internal byte[] accessToken;
}
