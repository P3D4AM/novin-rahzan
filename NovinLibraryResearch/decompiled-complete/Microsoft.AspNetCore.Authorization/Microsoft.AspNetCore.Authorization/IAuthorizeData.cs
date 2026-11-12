namespace Microsoft.AspNetCore.Authorization;

public interface IAuthorizeData
{
	string Policy { get; set; }

	string Roles { get; set; }

	string AuthenticationSchemes { get; set; }
}
