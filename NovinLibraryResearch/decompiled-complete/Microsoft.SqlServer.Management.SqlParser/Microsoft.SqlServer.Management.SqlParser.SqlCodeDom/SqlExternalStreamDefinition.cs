using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlExternalStreamDefinition : SqlCodeObject
{
	internal override bool IsVisitable => false;

	public List<SqlExternalStreamParam> ExternalStreamParams { get; }

	public SqlTableDefinition ExternalStreamTableDefinition { get; }

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		return default(T);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		return default(T);
	}

	public bool CheckRequiredParamsExist(out SqlExternalStreamParamType requiredParam)
	{
		requiredParam = SqlExternalStreamParamType.Empty;
		if (!ExternalStreamParams.Exists((SqlExternalStreamParam param) => param.Type == SqlExternalStreamParamType.DataSource))
		{
			requiredParam = SqlExternalStreamParamType.DataSource;
			return false;
		}
		return true;
	}

	public bool CheckDuplicateParamExist(out SqlExternalStreamParamType duplicatedParam)
	{
		HashSet<SqlExternalStreamParamType> hashSet = new HashSet<SqlExternalStreamParamType>();
		List<SqlExternalStreamParamType> list = ExternalStreamParams.ConvertAll((SqlExternalStreamParam param) => param.Type);
		hashSet.UnionWith(list);
		if (hashSet.Count < ExternalStreamParams.Count)
		{
			new Dictionary<SqlExternalStreamParamType, int>();
			foreach (SqlExternalStreamParamType item in list)
			{
				if (hashSet.Contains(item))
				{
					duplicatedParam = item;
					return true;
				}
				hashSet.Add(item);
			}
		}
		duplicatedParam = SqlExternalStreamParamType.Empty;
		return false;
	}

	internal SqlExternalStreamDefinition(List<SqlExternalStreamParam> streamParam)
	{
		ExternalStreamParams = streamParam;
	}

	internal SqlExternalStreamDefinition(SqlTableDefinition definition, List<SqlExternalStreamParam> streamParam)
		: this(streamParam)
	{
		ExternalStreamTableDefinition = definition;
	}
}
