using System;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticationScheme
{
	public string Name { get; }

	public string DisplayName { get; }

	public Type HandlerType { get; }

	public AuthenticationScheme(string name, string displayName, Type handlerType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (handlerType == null)
		{
			throw new ArgumentNullException("handlerType");
		}
		if (!typeof(IAuthenticationHandler).IsAssignableFrom(handlerType))
		{
			throw new ArgumentException("handlerType must implement IAuthenticationHandler.");
		}
		Name = name;
		HandlerType = handlerType;
		DisplayName = displayName;
	}
}
