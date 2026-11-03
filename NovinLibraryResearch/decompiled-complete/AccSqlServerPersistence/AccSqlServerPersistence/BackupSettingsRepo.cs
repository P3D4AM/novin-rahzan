using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Persistence.Repositories;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence;

public class BackupSettingsRepo : ISettingRepo, IHasContext
{
	private static ConcurrentDictionary<string, ISettingRepo> _dic = new ConcurrentDictionary<string, ISettingRepo>();

	public string ContextGuid { get; private set; }

	private BackupSettingsRepo(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static ISettingRepo GetInstance(string contextGuid)
	{
		if (_dic.TryGetValue(contextGuid, out var value))
		{
			return value;
		}
		value = new BackupSettingsRepo(contextGuid);
		_dic.TryAdd(contextGuid, value);
		return value;
	}

	public Task<string> GetValueAsync(string connectionString, string key, CancellationToken token)
	{
		return LocalSettingPersistenceRepository.GetValueAsync(ContextGuid, connectionString, key, token);
	}

	public Task<ReturnedSaveFuncInfo> SaveSettingAsync(string connectionString, string key, string value, CancellationToken token)
	{
		return LocalSettingPersistenceRepository.SaveAsync(ContextGuid, connectionString, key, value, "", "", token);
	}
}
