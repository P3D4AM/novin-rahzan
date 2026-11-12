using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationFeature
{
	PathString OriginalPathBase { get; set; }

	PathString OriginalPath { get; set; }
}
