using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticationOptions
{
	private readonly IList<AuthenticationSchemeBuilder> _schemes = new List<AuthenticationSchemeBuilder>();

	public IEnumerable<AuthenticationSchemeBuilder> Schemes => _schemes;

	public IDictionary<string, AuthenticationSchemeBuilder> SchemeMap { get; } = new Dictionary<string, AuthenticationSchemeBuilder>(StringComparer.Ordinal);


	public string DefaultScheme { get; set; }

	public string DefaultAuthenticateScheme { get; set; }

	public string DefaultSignInScheme { get; set; }

	public string DefaultSignOutScheme { get; set; }

	public string DefaultChallengeScheme { get; set; }

	public string DefaultForbidScheme { get; set; }

	public void AddScheme(string name, Action<AuthenticationSchemeBuilder> configureBuilder)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (configureBuilder == null)
		{
			throw new ArgumentNullException("configureBuilder");
		}
		if (SchemeMap.ContainsKey(name))
		{
			throw new InvalidOperationException("Scheme already exists: " + name);
		}
		AuthenticationSchemeBuilder authenticationSchemeBuilder = new AuthenticationSchemeBuilder(name);
		configureBuilder(authenticationSchemeBuilder);
		_schemes.Add(authenticationSchemeBuilder);
		SchemeMap[name] = authenticationSchemeBuilder;
	}

	public void AddScheme<THandler>(string name, string displayName) where THandler : IAuthenticationHandler
	{
		AddScheme(name, delegate(AuthenticationSchemeBuilder b)
		{
			b.DisplayName = displayName;
			b.HandlerType = typeof(THandler);
		});
	}
}
