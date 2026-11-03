using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Indexes;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class FullTextWordPersistenceRepositoryBase : PersistenceNoGuidRepository<FullTextWordBusiness, FullTextWords>, IFullTextWordRepository, INoGuidRepository<FullTextWordBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<FullTextWords, FullTextWordBusiness>, IItemAsyncMapper<FullTextWordBusiness, FullTextWords>
	{
		public Task<FullTextWords> MapAsync(string contextGuid, FullTextWordBusiness source, FullTextWords destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new FullTextWords();
					}
					destination.Id = source.Id;
					destination.Word = source.Word;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 239);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<FullTextWordBusiness> MapAsync(string contextGuid, FullTextWords source, FullTextWordBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new FullTextWordBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Id = source.Id;
					destination.Word = source.Word;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 265);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<FullTextWords>> MapListAsync(string contextGuid, IEnumerable<FullTextWordBusiness> source, List<FullTextWords> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<FullTextWordBusiness>> MapListAsync(string contextGuid, IEnumerable<FullTextWords> source, List<FullTextWordBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool isIndexing;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<FullTextWords, FullTextWordBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<FullTextWordBusiness, FullTextWords> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public FullTextWordPersistenceRepositoryBase(ContextHolder context)
		: base(context)
	{
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(string word, int id, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(FullTextWordBusiness word, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<Dictionary<string, int>> GetOrSaveAsync(List<string> words, CancellationToken token, string tranName)
	{
		Stopwatch.StartNew();
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 43));
		using (new JaegerMonitor("GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs"))
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			Dictionary<string, int> ret = new Dictionary<string, int>();
			int line = 0;
			try
			{
				isIndexing = true;
				if (!(words?.Any() ?? false))
				{
					return ret;
				}
				line = 5;
				string query = "";
				line = 6;
				foreach (string word in words)
				{
					token.ThrowIfCancellationRequested();
					line = 9;
					query = query + "INSERT INTO [dbo].[Acc_FullTextWords_temp]([Word])VALUES(N'" + word + "')\r\n";
				}
				line = 10;
				line = 11;
				await TruncateTempWordsAsync(token, tranName);
				token.ThrowIfCancellationRequested();
				(await c.ExecuteNoneQueryAsync(query, null, tranName, token, "", CommandType.Text, 30, "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs")).throwExceptionIfError();
				token.ThrowIfCancellationRequested();
				using DbDataReader reader = await c.ExecuteReaderAsync("select Acc_FullTextWords.id,       Acc_FullTextWords.Word,       temp.word as tempWord from (       SELECT distinct [Word]         FROM [Acc_FullTextWords_temp]     ) as  temp    inner join Acc_FullTextWords on    temp.word=Acc_FullTextWords.Word", null, tranName, token, CommandType.Text, 30, "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs");
				line = 13;
				List<FullTextWordBusiness> existsKeys = new List<FullTextWordBusiness>();
				line = 14;
				while (await reader.ReadAsync(token))
				{
					token.ThrowIfCancellationRequested();
					string text = reader["Word"].ToString();
					string wordForIndexing = reader["tempWord"].ToString();
					int id = (int)reader["Id"];
					if (text.ToLower() != wordForIndexing.ToLower())
					{
						WebErrorLog.Instance.StartLog("این خطا منطقا باید رفع شده باشه و برای داده های قدیمی کاربران یکبار میخوره و باید اصلاح بشه\r\nword on database:" + text + " indexed for word for indexing:" + wordForIndexing + ". but this tow words are not as same as others.", "", "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 110);
						await c.ExecuteNoneQueryAsync($"update Acc_FullTextWords set word=N'{wordForIndexing}' where id={id}", null, tranName, default(CancellationToken), "", CommandType.Text, 30, "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs");
					}
					existsKeys.Add(new FullTextWordBusiness(base.ContextGuid)
					{
						ContextGuid = base.ContextGuid,
						Id = id,
						Word = wordForIndexing
					});
				}
				reader.Close();
				List<string> existsWords = existsKeys?.Select((FullTextWordBusiness p) => p.Word.Trim().ToLower())?.ToList();
				token.ThrowIfCancellationRequested();
				List<string> itesmForSaveOnDb;
				if (existsWords?.Any() ?? false)
				{
					foreach (FullTextWordBusiness item in existsKeys)
					{
						token.ThrowIfCancellationRequested();
						ret.Add(item.Word.Trim(), item.Id);
					}
					itesmForSaveOnDb = words.Where((string p) => !existsWords.Contains(p))?.ToList();
				}
				else
				{
					itesmForSaveOnDb = words;
				}
				token.ThrowIfCancellationRequested();
				if (itesmForSaveOnDb?.Any() ?? false)
				{
					int num = ((await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<FullTextWords>((IQueryable<FullTextWords>)((IQueryable<FullTextWords>)c.FullTextWords).OrderByDescending((FullTextWords x) => x.Id), token))?.Id ?? 0) + 1;
					List<FullTextWords> list = new List<FullTextWords>();
					foreach (string item2 in itesmForSaveOnDb)
					{
						token.ThrowIfCancellationRequested();
						num++;
						list.Add(new FullTextWords
						{
							Id = num,
							Word = item2
						});
					}
					foreach (FullTextWords item3 in list)
					{
						token.ThrowIfCancellationRequested();
						ret.Add(item3.Word.Trim(), item3.Id);
					}
					token.ThrowIfCancellationRequested();
					await c.FullTextWords.AddRangeAsync((IEnumerable<FullTextWords>)list, token);
					token.ThrowIfCancellationRequested();
					await ((DbContext)c).SaveChangesAsync(token);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "GetOrSaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 173);
				throw;
			}
			finally
			{
				isIndexing = false;
			}
			return ret;
		}
	}

	public Task FlushAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 199);
		}
	}

	private async Task TruncateTempWordsAsync(CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			Acc_ModelContext obj = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "TruncateTempWordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 206));
			token.ThrowIfCancellationRequested();
			(await obj.ExecuteNoneQueryAsync("delete from Acc_FullTextWords_temp", null, tranName, token, "", CommandType.Text, 30, "TruncateTempWordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs")).throwExceptionIfError();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "TruncateTempWordsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\FullTextWordPersistenceRepositoryBase.cs", 214);
			throw;
		}
	}
}
