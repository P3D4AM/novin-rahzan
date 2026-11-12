using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectProtocolValidationContext
{
	public string ClientId { get; set; }

	public string Nonce { get; set; }

	public OpenIdConnectMessage ProtocolMessage { get; set; }

	public string State { get; set; }

	public string UserInfoEndpointResponse { get; set; }

	public JwtSecurityToken ValidatedIdToken { get; set; }
}
