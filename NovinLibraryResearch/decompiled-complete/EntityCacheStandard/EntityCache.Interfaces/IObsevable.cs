using System.Threading.Tasks;

namespace EntityCache.Interfaces;

public interface IObsevable
{
	void Subscribe(IObserver observer);

	void UnSubscribe(IObserver observer);

	Task DisposeObservers();
}
