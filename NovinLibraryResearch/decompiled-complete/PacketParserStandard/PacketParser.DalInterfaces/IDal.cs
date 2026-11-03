using System.Threading.Tasks;

namespace PacketParser.DalInterfaces;

public interface IDal<T>
{
	T Get(object Id);

	Task<T> GetAsync(object Id);
}
