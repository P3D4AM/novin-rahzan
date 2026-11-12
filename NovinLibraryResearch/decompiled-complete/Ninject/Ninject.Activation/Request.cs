using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Infrastructure.Introspection;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;

namespace Ninject.Activation;

public class Request : IRequest
{
	public Type Service { get; private set; }

	public IRequest ParentRequest { get; private set; }

	public IContext ParentContext { get; private set; }

	public ITarget Target { get; private set; }

	public Func<IBindingMetadata, bool> Constraint { get; private set; }

	public ICollection<IParameter> Parameters { get; private set; }

	public Stack<IBinding> ActiveBindings { get; private set; }

	public int Depth { get; private set; }

	public bool IsOptional { get; set; }

	public bool IsUnique { get; set; }

	public bool ForceUnique { get; set; }

	public Func<object> ScopeCallback { get; private set; }

	public Request(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, Func<object> scopeCallback, bool isOptional, bool isUnique)
	{
		Service = service;
		Constraint = constraint;
		Parameters = parameters.ToList();
		ScopeCallback = scopeCallback;
		ActiveBindings = new Stack<IBinding>();
		Depth = 0;
		IsOptional = isOptional;
		IsUnique = isUnique;
	}

	public Request(IContext parentContext, Type service, ITarget target, Func<object> scopeCallback)
	{
		ParentContext = parentContext;
		ParentRequest = parentContext.Request;
		Service = service;
		Target = target;
		Constraint = target.Constraint;
		IsOptional = target.IsOptional;
		Parameters = parentContext.Parameters.Where((IParameter p) => p.ShouldInherit).ToList();
		ScopeCallback = scopeCallback;
		ActiveBindings = new Stack<IBinding>(ParentRequest.ActiveBindings);
		Depth = ParentRequest.Depth + 1;
	}

	public bool Matches(IBinding binding)
	{
		if (Constraint != null)
		{
			return Constraint(binding.Metadata);
		}
		return true;
	}

	public object GetScope()
	{
		if (ScopeCallback != null)
		{
			return ScopeCallback();
		}
		return null;
	}

	public IRequest CreateChild(Type service, IContext parentContext, ITarget target)
	{
		return new Request(parentContext, service, target, ScopeCallback);
	}

	public override string ToString()
	{
		return this.Format();
	}
}
