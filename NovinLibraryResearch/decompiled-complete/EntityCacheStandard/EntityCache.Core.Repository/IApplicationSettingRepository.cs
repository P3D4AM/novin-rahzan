using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace EntityCache.Core.Repository;

public interface IApplicationSettingRepository<T> where T : IApplicationSetting, new()
{
	T Get(string name);

	Task<T> GetAsync(string name, string tranName);

	IEnumerable<T> GetAll(string tranName);

	Task<List<T>> GetAllAsync(string tranName);

	void Remove(T item, short tryCount, string tranName);

	Task RemoveAsync(T item, short tryCount, string tranName);

	void RemoveRange(IEnumerable<T> items, short tryCount, string tranName);

	Task RemoveRangeAsync(IEnumerable<T> items, short tryCount, string tranName);

	void Add(T item, short tryCount, string tranName);

	Task AddAsync(T item, short tryCount, string tranName);

	void AddRange(IEnumerable<T> items, short tryCount, string tranName);

	Task AddRangeAsync(IEnumerable<T> items, short tryCount, string tranName);

	void Save(T item, short tryCount, string tranName);

	Task SaveAsync(T item, short tryCount, string tranName);

	void SaveRange(IEnumerable<T> items, short tryCount, string tranName);

	Task SaveRangeAsync(IEnumerable<T> items, short tryCount, string tranName);
}
