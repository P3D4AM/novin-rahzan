using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authorization;

public class DefaultAuthorizationService : IAuthorizationService
{
	private readonly AuthorizationOptions _options;

	private readonly IAuthorizationHandlerContextFactory _contextFactory;

	private readonly IAuthorizationHandlerProvider _handlers;

	private readonly IAuthorizationEvaluator _evaluator;

	private readonly IAuthorizationPolicyProvider _policyProvider;

	private readonly ILogger _logger;

	public DefaultAuthorizationService(IAuthorizationPolicyProvider policyProvider, IAuthorizationHandlerProvider handlers, ILogger<DefaultAuthorizationService> logger, IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator, IOptions<AuthorizationOptions> options)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		if (policyProvider == null)
		{
			throw new ArgumentNullException("policyProvider");
		}
		if (handlers == null)
		{
			throw new ArgumentNullException("handlers");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (contextFactory == null)
		{
			throw new ArgumentNullException("contextFactory");
		}
		if (evaluator == null)
		{
			throw new ArgumentNullException("evaluator");
		}
		_options = options.Value;
		_handlers = handlers;
		_policyProvider = policyProvider;
		_logger = logger;
		_evaluator = evaluator;
		_contextFactory = contextFactory;
	}

	public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
	{
		if (requirements == null)
		{
			throw new ArgumentNullException("requirements");
		}
		AuthorizationHandlerContext authContext = _contextFactory.CreateContext(requirements, user, resource);
		foreach (IAuthorizationHandler item in await _handlers.GetHandlersAsync(authContext))
		{
			await item.HandleAsync(authContext);
			if (!_options.InvokeHandlersAfterFailure && authContext.HasFailed)
			{
				break;
			}
		}
		AuthorizationResult authorizationResult = _evaluator.Evaluate(authContext);
		if (authorizationResult.Succeeded)
		{
			_logger.UserAuthorizationSucceeded();
		}
		else
		{
			_logger.UserAuthorizationFailed();
		}
		return authorizationResult;
	}

	public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
	{
		if (policyName == null)
		{
			throw new ArgumentNullException("policyName");
		}
		AuthorizationPolicy authorizationPolicy = await _policyProvider.GetPolicyAsync(policyName);
		if (authorizationPolicy == null)
		{
			throw new InvalidOperationException($"No policy found: {policyName}.");
		}
		return await this.AuthorizeAsync(user, resource, authorizationPolicy);
	}
}
