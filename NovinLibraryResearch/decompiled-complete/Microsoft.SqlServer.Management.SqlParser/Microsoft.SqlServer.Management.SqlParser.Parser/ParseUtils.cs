using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public static class ParseUtils
{
	public static IDictionary<string, object> RetrieveModuleDefinition(string sql, ParseOptions parseOptions)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (parseOptions == null)
		{
			throw new ArgumentNullException("parseOptions");
		}
		SqlScript script = Parser.Parse(sql, parseOptions).Script;
		if (script.Batches.Count != 1 || script.Batches[0].Statements.Count != 1)
		{
			throw new ArgumentException(SR.SqlScriptMustHaveOnlyModuleStatement, "sql");
		}
		SqlStatement sqlStatement = script.Batches[0].Statements[0];
		Dictionary<string, object> dictionary;
		if (sqlStatement is SqlCreateAlterProcedureStatementBase sqlCreateAlterProcedureStatementBase)
		{
			List<IDictionary<string, object>> parametersInfo = GetParametersInfo(sqlCreateAlterProcedureStatementBase.Definition.Parameters);
			SqlCodeObject firstDefinitionObj = ((sqlCreateAlterProcedureStatementBase.Statements.Count > 0) ? sqlCreateAlterProcedureStatementBase.Statements[0] : null);
			string extendedBodyDefinitionText = GetExtendedBodyDefinitionText(script, firstDefinitionObj);
			dictionary = new Dictionary<string, object>();
			dictionary.Add(PropertyKeys.ModuleType, "StoredProcedure");
			dictionary.Add(PropertyKeys.Name, (sqlCreateAlterProcedureStatementBase.Definition.Name.ObjectName != null) ? sqlCreateAlterProcedureStatementBase.Definition.Name.ObjectName.Value : null);
			dictionary.Add(PropertyKeys.SchemaName, (sqlCreateAlterProcedureStatementBase.Definition.Name.SchemaName != null) ? sqlCreateAlterProcedureStatementBase.Definition.Name.SchemaName.Value : null);
			dictionary.Add(PropertyKeys.DatabaseName, (sqlCreateAlterProcedureStatementBase.Definition.Name.DatabaseName != null) ? sqlCreateAlterProcedureStatementBase.Definition.Name.DatabaseName.Value : null);
			dictionary.Add(PropertyKeys.Parameters, parametersInfo);
			dictionary.Add(PropertyKeys.BodyDefinition, extendedBodyDefinitionText);
		}
		else
		{
			if (!(sqlStatement is SqlCreateAlterFunctionStatementBase sqlCreateAlterFunctionStatementBase))
			{
				throw new ArgumentException(SR.SqlScriptMustHaveOnlyModuleStatement, "sql");
			}
			List<IDictionary<string, object>> parametersInfo2 = GetParametersInfo(sqlCreateAlterFunctionStatementBase.Definition.Parameters);
			dictionary = new Dictionary<string, object>();
			dictionary.Add(PropertyKeys.ModuleType, "Function");
			dictionary.Add(PropertyKeys.Name, (sqlCreateAlterFunctionStatementBase.Definition.Name.ObjectName != null) ? sqlCreateAlterFunctionStatementBase.Definition.Name.ObjectName.Value : null);
			dictionary.Add(PropertyKeys.SchemaName, (sqlCreateAlterFunctionStatementBase.Definition.Name.SchemaName != null) ? sqlCreateAlterFunctionStatementBase.Definition.Name.SchemaName.Value : null);
			dictionary.Add(PropertyKeys.DatabaseName, (sqlCreateAlterFunctionStatementBase.Definition.Name.DatabaseName != null) ? sqlCreateAlterFunctionStatementBase.Definition.Name.DatabaseName.Value : null);
			dictionary.Add(PropertyKeys.Parameters, parametersInfo2);
			string value = (sqlCreateAlterFunctionStatementBase.Definition.IsClrFunction ? null : sqlCreateAlterFunctionStatementBase.Definition.GetBodyDefinition().Sql);
			dictionary.Add(PropertyKeys.BodyDefinition, value);
			string value2 = ((sqlCreateAlterFunctionStatementBase.Definition is SqlMultistatementTableRelationalFunctionDefinition sqlMultistatementTableRelationalFunctionDefinition) ? sqlMultistatementTableRelationalFunctionDefinition.VariableName : null);
			dictionary.Add(PropertyKeys.TableVariableName, value2);
		}
		return dictionary;
	}

	internal static SqlParameterDeclarationCollection RetrieveStoredProcParameters(ParseResult parseResult)
	{
		return (parseResult.Script.Batches[0].Statements[0] as SqlCreateAlterProcedureStatementBase).Definition.Parameters;
	}

	public static IDictionary<string, object> RetrieveTriggerDefinition(string sql, ParseOptions parseOptions)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (parseOptions == null)
		{
			throw new ArgumentNullException("parseOptions");
		}
		SqlScript script = Parser.Parse(sql, parseOptions).Script;
		if (script.Batches.Count != 1 || script.Batches[0].Statements.Count != 1)
		{
			throw new ArgumentException(SR.SqlScriptMustHaveOnlyTriggerStatement, "sql");
		}
		if (!(script.Batches[0].Statements[0] is SqlCreateAlterTriggerStatementBase sqlCreateAlterTriggerStatementBase))
		{
			throw new ArgumentException(SR.SqlScriptMustHaveOnlyTriggerStatement, "sql");
		}
		Dictionary<string, object> obj = new Dictionary<string, object>
		{
			{
				PropertyKeys.Name,
				(sqlCreateAlterTriggerStatementBase.Definition.Name.ObjectName != null) ? sqlCreateAlterTriggerStatementBase.Definition.Name.ObjectName.Value : null
			},
			{
				PropertyKeys.SchemaName,
				(sqlCreateAlterTriggerStatementBase.Definition.Name.SchemaName != null) ? sqlCreateAlterTriggerStatementBase.Definition.Name.SchemaName.Value : null
			},
			{
				PropertyKeys.DatabaseName,
				(sqlCreateAlterTriggerStatementBase.Definition.Name.DatabaseName != null) ? sqlCreateAlterTriggerStatementBase.Definition.Name.DatabaseName.Value : null
			}
		};
		obj.Add(value: SqlCodeObject.GetSqlSource(sqlCreateAlterTriggerStatementBase.Statements) ?? string.Empty, key: PropertyKeys.BodyDefinition);
		return obj;
	}

	public static IDictionary<string, object> RetrieveViewDefinition(string sql, ParseOptions parseOptions)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (parseOptions == null)
		{
			throw new ArgumentNullException("parseOptions");
		}
		SqlScript script = Parser.Parse(sql, parseOptions).Script;
		if (script.Batches.Count != 1 || script.Batches[0].Statements.Count != 1)
		{
			throw new ArgumentException(SR.SqlScriptMustHaveOnlyViewStatement, "sql");
		}
		if (!(script.Batches[0].Statements[0] is SqlCreateAlterViewStatementBase sqlCreateAlterViewStatementBase))
		{
			throw new ArgumentException(SR.SqlScriptMustHaveOnlyViewStatement, "sql");
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>(5);
		SqlCodeObject firstDefinitionObj = ((sqlCreateAlterViewStatementBase.Definition.QueryWithClause != null) ? ((SqlCodeObject)sqlCreateAlterViewStatementBase.Definition.QueryWithClause) : ((SqlCodeObject)sqlCreateAlterViewStatementBase.Definition.QueryExpression));
		SqlCodeObject queryExpression = sqlCreateAlterViewStatementBase.Definition.QueryExpression;
		string extendedQueryDefinitionText = GetExtendedQueryDefinitionText(script, firstDefinitionObj, queryExpression);
		bool flag = sqlCreateAlterViewStatementBase.Definition.ColumnList != null && sqlCreateAlterViewStatementBase.Definition.ColumnList.Count > 0;
		bool hasCheckOption = sqlCreateAlterViewStatementBase.Definition.HasCheckOption;
		dictionary.Add(PropertyKeys.Name, (sqlCreateAlterViewStatementBase.Definition.Name.ObjectName != null) ? sqlCreateAlterViewStatementBase.Definition.Name.ObjectName.Value : null);
		dictionary.Add(PropertyKeys.SchemaName, (sqlCreateAlterViewStatementBase.Definition.Name.SchemaName != null) ? sqlCreateAlterViewStatementBase.Definition.Name.SchemaName.Value : null);
		dictionary.Add(PropertyKeys.DatabaseName, (sqlCreateAlterViewStatementBase.Definition.Name.DatabaseName != null) ? sqlCreateAlterViewStatementBase.Definition.Name.DatabaseName.Value : null);
		dictionary.Add(PropertyKeys.QueryDefinition, extendedQueryDefinitionText);
		dictionary.Add(PropertyKeys.HasColumnSpecification, flag);
		dictionary.Add(PropertyKeys.HasCheckOption, hasCheckOption);
		return dictionary;
	}

	private static List<IDictionary<string, object>> GetParametersInfo(SqlParameterDeclarationCollection parameterDecls)
	{
		List<IDictionary<string, object>> list;
		if (parameterDecls != null)
		{
			list = new List<IDictionary<string, object>>(parameterDecls.Count);
			foreach (SqlParameterDeclaration parameterDecl in parameterDecls)
			{
				string value = ((parameterDecl.Value != null) ? parameterDecl.Value.Sql : null);
				Dictionary<string, object> dictionary = new Dictionary<string, object>(2);
				dictionary.Add(PropertyKeys.Name, parameterDecl.Name);
				dictionary.Add(PropertyKeys.DefaultValue, value);
				list.Add(dictionary);
			}
		}
		else
		{
			list = null;
		}
		return list;
	}

	private static string GetExtendedQueryDefinitionText(SqlScript sqlScript, SqlCodeObject firstDefinitionObj, SqlCodeObject lastDefinitionObj)
	{
		TokenManager tokenManager = sqlScript.TokenManager;
		int startTokenNumber = firstDefinitionObj.Position.startTokenNumber;
		int endTokenNumber = lastDefinitionObj.Position.endTokenNumber;
		int startTokenNumber2 = Math.Min(startTokenNumber, tokenManager.GetPreviousSignificantTokenIndex(startTokenNumber) + 1);
		int num = tokenManager.GetNextSignificantTokenIndex(endTokenNumber - 1);
		if (num < 0)
		{
			num = tokenManager.Count;
		}
		int endTokenNumber2 = Math.Max(endTokenNumber, num);
		return tokenManager.GetText(startTokenNumber2, endTokenNumber2).Trim();
	}

	private static string GetExtendedBodyDefinitionText(SqlScript sqlScript, SqlCodeObject firstDefinitionObj)
	{
		TokenManager tokenManager = sqlScript.TokenManager;
		int num = firstDefinitionObj?.Position.startTokenNumber ?? tokenManager.Count;
		int startTokenNumber = Math.Min(num, tokenManager.GetPreviousSignificantTokenIndex(num) + 1);
		int count = tokenManager.Count;
		return tokenManager.GetText(startTokenNumber, count).Trim();
	}
}
