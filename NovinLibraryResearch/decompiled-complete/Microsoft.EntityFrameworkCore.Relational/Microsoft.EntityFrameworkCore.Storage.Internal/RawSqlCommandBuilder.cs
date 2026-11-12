using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class RawSqlCommandBuilder : IRawSqlCommandBuilder
{
	private readonly IRelationalCommandBuilderFactory _relationalCommandBuilderFactory;

	private readonly ISqlGenerationHelper _sqlGenerationHelper;

	private readonly IParameterNameGeneratorFactory _parameterNameGeneratorFactory;

	public RawSqlCommandBuilder([JetBrains.Annotations.NotNull] IRelationalCommandBuilderFactory relationalCommandBuilderFactory, [JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalCommandBuilderFactory, "relationalCommandBuilderFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterNameGeneratorFactory, "parameterNameGeneratorFactory");
		_relationalCommandBuilderFactory = relationalCommandBuilderFactory;
		_sqlGenerationHelper = sqlGenerationHelper;
		_parameterNameGeneratorFactory = parameterNameGeneratorFactory;
	}

	public virtual IRelationalCommand Build(string sql)
	{
		return _relationalCommandBuilderFactory.Create().Append(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql")).Build();
	}

	public virtual RawSqlCommand Build(string sql, IEnumerable<object> parameters)
	{
		IRelationalCommandBuilder relationalCommandBuilder = _relationalCommandBuilderFactory.Create();
		List<string> list = new List<string>();
		ParameterNameGenerator parameterNameGenerator = _parameterNameGeneratorFactory.Create();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (object parameter in parameters)
		{
			if (parameter is DbParameter dbParameter)
			{
				if (string.IsNullOrEmpty(dbParameter.ParameterName))
				{
					dbParameter.ParameterName = _sqlGenerationHelper.GenerateParameterName(parameterNameGenerator.GenerateNext());
				}
				list.Add(dbParameter.ParameterName);
				relationalCommandBuilder.AddRawParameter(dbParameter.ParameterName, dbParameter);
			}
			else
			{
				string text = parameterNameGenerator.GenerateNext();
				string text2 = _sqlGenerationHelper.GenerateParameterName(text);
				list.Add(text2);
				relationalCommandBuilder.AddParameter(text, text2);
				dictionary.Add(text, parameter);
			}
		}
		string format = sql;
		object[] args = list.ToArray();
		sql = string.Format(format, args);
		return new RawSqlCommand(relationalCommandBuilder.Append(sql).Build(), dictionary);
	}
}
