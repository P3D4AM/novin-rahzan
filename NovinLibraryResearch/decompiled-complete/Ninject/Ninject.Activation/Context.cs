using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation.Caching;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;

namespace Ninject.Activation;

public class Context : IContext
{
	private object cachedScope;

	public IKernel Kernel { get; set; }

	public IRequest Request { get; set; }

	public IBinding Binding { get; set; }

	public IPlan Plan { get; set; }

	public ICollection<IParameter> Parameters { get; set; }

	public Type[] GenericArguments { get; private set; }

	public bool HasInferredGenericArguments { get; private set; }

	public ICache Cache { get; private set; }

	public IPlanner Planner { get; private set; }

	public IPipeline Pipeline { get; private set; }

	public Context(IKernel kernel, IRequest request, IBinding binding, ICache cache, IPlanner planner, IPipeline pipeline)
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Ensure.ArgumentNotNull(request, "request");
		Ensure.ArgumentNotNull(binding, "binding");
		Ensure.ArgumentNotNull(cache, "cache");
		Ensure.ArgumentNotNull(planner, "planner");
		Ensure.ArgumentNotNull(pipeline, "pipeline");
		Kernel = kernel;
		Request = request;
		Binding = binding;
		Parameters = request.Parameters.Union(binding.Parameters).ToList();
		Cache = cache;
		Planner = planner;
		Pipeline = pipeline;
		if (binding.Service.IsGenericTypeDefinition)
		{
			HasInferredGenericArguments = true;
			GenericArguments = request.Service.GenericTypeArguments;
		}
	}

	public object GetScope()
	{
		return cachedScope ?? Request.GetScope() ?? Binding.GetScope(this);
	}

	public IProvider GetProvider()
	{
		return Binding.GetProvider(this);
	}

	public object Resolve()
	{
		if (Request.ActiveBindings.Contains(Binding) && IsCyclical(Request.ParentContext))
		{
			throw new ActivationException(ExceptionFormatter.CyclicalDependenciesDetected(this));
		}
		try
		{
			cachedScope = Request.GetScope() ?? Binding.GetScope(this);
			if (cachedScope != null)
			{
				lock (cachedScope)
				{
					return ResolveInternal(cachedScope);
				}
			}
			return ResolveInternal(null);
		}
		finally
		{
			cachedScope = null;
		}
	}

	private object ResolveInternal(object scope)
	{
		object obj = Cache.TryGet(this);
		if (obj != null)
		{
			return obj;
		}
		Request.ActiveBindings.Push(Binding);
		InstanceReference instanceReference = new InstanceReference
		{
			Instance = GetProvider().Create(this)
		};
		Request.ActiveBindings.Pop();
		if (instanceReference.Instance == null)
		{
			if (!Kernel.Settings.AllowNullInjection)
			{
				throw new ActivationException(ExceptionFormatter.ProviderReturnedNull(this));
			}
			if (Plan == null)
			{
				Plan = Planner.GetPlan(Request.Service);
			}
			return null;
		}
		if (scope != null)
		{
			Cache.Remember(this, instanceReference);
		}
		if (Plan == null)
		{
			Plan = Planner.GetPlan(instanceReference.Instance.GetType());
		}
		try
		{
			Pipeline.Activate(this, instanceReference);
		}
		catch (ActivationException)
		{
			if (scope != null)
			{
				Cache.Release(instanceReference.Instance);
			}
			throw;
		}
		return instanceReference.Instance;
	}

	private bool IsCyclical(IContext targetContext)
	{
		if (targetContext == null)
		{
			return false;
		}
		if (targetContext.Request.Service == Request.Service && ((Request.Target is ParameterTarget && targetContext.Request.Target is ParameterTarget) || targetContext.GetScope() != GetScope() || GetScope() == null))
		{
			return true;
		}
		if (IsCyclical(targetContext.Request.ParentContext))
		{
			return true;
		}
		return false;
	}
}
