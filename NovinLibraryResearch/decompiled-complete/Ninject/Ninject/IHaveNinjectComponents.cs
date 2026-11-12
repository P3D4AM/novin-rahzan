using Ninject.Components;

namespace Ninject;

public interface IHaveNinjectComponents
{
	IComponentContainer Components { get; }
}
