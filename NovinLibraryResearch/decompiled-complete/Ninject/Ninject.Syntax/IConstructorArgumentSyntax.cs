using Ninject.Activation;

namespace Ninject.Syntax;

public interface IConstructorArgumentSyntax : IFluentSyntax
{
	IContext Context { get; }

	T Inject<T>();
}
