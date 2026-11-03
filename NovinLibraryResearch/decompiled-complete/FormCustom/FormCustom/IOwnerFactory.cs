using FormCustom.Cache;

namespace FormCustom;

public interface IOwnerFactory
{
	CacheBaseForm GetOwner(string contextGuid);
}
