using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationHandlerContext
{
	private HashSet<IAuthorizationRequirement> _pendingRequirements;

	private bool _failCalled;

	private bool _succeedCalled;

	public virtual IEnumerable<IAuthorizationRequirement> Requirements { get; }

	public virtual ClaimsPrincipal User { get; }

	public virtual object Resource { get; }

	public virtual IEnumerable<IAuthorizationRequirement> PendingRequirements => _pendingRequirements;

	public virtual bool HasFailed => _failCalled;

	public virtual bool HasSucceeded
	{
		get
		{
			if (!_failCalled && _succeedCalled)
			{
				return !PendingRequirements.Any();
			}
			return false;
		}
	}

	public AuthorizationHandlerContext(IEnumerable<IAuthorizationRequirement> requirements, ClaimsPrincipal user, object resource)
	{
		if (requirements == null)
		{
			throw new ArgumentNullException("requirements");
		}
		Requirements = requirements;
		User = user;
		Resource = resource;
		_pendingRequirements = new HashSet<IAuthorizationRequirement>(requirements);
	}

	public virtual void Fail()
	{
		_failCalled = true;
	}

	public virtual void Succeed(IAuthorizationRequirement requirement)
	{
		_succeedCalled = true;
		_pendingRequirements.Remove(requirement);
	}
}
