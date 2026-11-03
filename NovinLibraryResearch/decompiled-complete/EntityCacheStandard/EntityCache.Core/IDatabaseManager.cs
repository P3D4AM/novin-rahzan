using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace EntityCache.Core;

public interface IDatabaseManager : IDisposable
{
	EnCacheConfig Config { get; }

	Task<ReturnedSaveFuncInfo> RollBackTransactionAsync(string tranName, bool disposeContext, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

	Task<ReturnedSaveFuncInfo> BeginTransactionAsync(string tranName, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);

	Task<ReturnedSaveFuncInfo> CommitTransactionAsync(string tranName, bool disposedContext, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, string debugInfo = "");

	Task<ReturnedSaveFuncInfoWithValue<bool>> ExistsAsync(string tranName);

	Task<AccGlobalSettings> GetLocalSettingAsync(string contextGuid);
}
