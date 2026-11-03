using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Selection;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ProductGroupRelationPersistenceRepository : ProductGroupRelationPersistenceRepositoryBase
{
	public ProductGroupRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ConcurrentDictionary<Guid, List<Guid>>> GetDictionaryAsync(List<Guid> productGuidList, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductGroupRelationPersistenceRepository.cs", 23));
		ConcurrentDictionary<Guid, List<Guid>> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				if (!(productGuidList?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, productGuidList, token);
					if (returnedSaveFuncInfoWithValue.HasError)
					{
						result = null;
					}
					else
					{
						string commandText = "select ProductGuid,GroupGuid from Acc_ProductGroupRelation inner join StimulQuery on Acc_ProductGroupRelation.ProductGuid=StimulQuery.Guid where StimulQuery.RefrencedId=@RefId";
						List<SqlParameter> parameters = new List<SqlParameter>
						{
							new SqlParameter("@RefId", (object)returnedSaveFuncInfoWithValue.value)
						};
						using DbDataReader dr = await c.ExecuteReaderAsync(commandText, parameters, tranName, token, CommandType.Text, 30, "GetDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductGroupRelationPersistenceRepository.cs");
						ConcurrentDictionary<Guid, List<Guid>> _dic = new ConcurrentDictionary<Guid, List<Guid>>();
						while (true)
						{
							if (await dr.ReadAsync(token))
							{
								if (base.IsDisposed || token.IsCancellationRequested)
								{
									result = null;
									break;
								}
								Guid guid = Guid.Empty;
								Guid guid2 = Guid.Empty;
								if (dr["ProductGuid"] != DBNull.Value)
								{
									guid = (Guid)dr["ProductGuid"];
								}
								if (dr["GroupGuid"] != DBNull.Value)
								{
									guid2 = (Guid)dr["GroupGuid"];
								}
								if (!(guid == Guid.Empty) && !(guid2 == Guid.Empty))
								{
									_dic.TryGetValue(guid, out var value);
									if (value == null)
									{
										value = new List<Guid>();
									}
									value.Add(guid2);
									_dic.TryRemove(guid, out var _);
									_dic.TryAdd(guid, value);
								}
								continue;
							}
							result = _dic;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductGroupRelationPersistenceRepository.cs", 56);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductGroupRelationPersistenceRepository.cs");
			}
		}
		return result;
	}
}
