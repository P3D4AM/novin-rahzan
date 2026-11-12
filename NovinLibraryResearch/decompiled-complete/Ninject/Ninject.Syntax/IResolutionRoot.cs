using System;
using System.Collections.Generic;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace Ninject.Syntax;

public interface IResolutionRoot : IFluentSyntax
{
	void Inject(object instance, params IParameter[] parameters);

	bool CanResolve(IRequest request);

	bool CanResolve(IRequest request, bool ignoreImplicitBindings);

	IEnumerable<object> Resolve(IRequest request);

	IRequest CreateRequest(Type service, Func<IBindingMetadata, bool> constraint, IEnumerable<IParameter> parameters, bool isOptional, bool isUnique);

	bool Release(object instance);
}
