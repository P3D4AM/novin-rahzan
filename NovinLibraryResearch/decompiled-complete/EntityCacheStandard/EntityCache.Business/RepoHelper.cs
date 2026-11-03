using EntityCache.Core;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class RepoHelper<T, U> where T : class, IHasGuid, IHasContext where U : IRepository<T>
{
	private IRepository<T> repo;

	private T item;

	public RepoHelper(U repo, T item)
	{
		this.repo = repo;
	}
}
