using System;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticationSchemeBuilder
{
	public string Name { get; }

	public string DisplayName { get; set; }

	public Type HandlerType { get; set; }

	public AuthenticationSchemeBuilder(string name)
	{
		Name = name;
	}

	public AuthenticationScheme Build()
	{
		return new AuthenticationScheme(Name, DisplayName, HandlerType);
	}
}
