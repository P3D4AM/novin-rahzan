using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class FunctionDefinitionFactory
{
	private sealed class InlineTableRelationalFunctionDefinitionFactory : FunctionDefinitionFactory
	{
		private readonly List<SqlModuleOption> options;

		private readonly SqlInlineFunctionBodyDefinition bodyDefinition;

		public InlineTableRelationalFunctionDefinitionFactory(List<SqlModuleOption> options, SqlInlineFunctionBodyDefinition bodyDefinition)
		{
			this.options = options;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlInlineTableRelationalFunctionDefinition(name, parameters, options, bodyDefinition);
		}
	}

	private sealed class MultistatementTableRelationalFunctionDefinitionFactory : FunctionDefinitionFactory
	{
		private readonly SqlLiteralExpression variable;

		private readonly SqlTableFunctionReturnType returnType;

		private readonly List<SqlModuleOption> options;

		private readonly SqlMultistatementFunctionBodyDefinition bodyDefinition;

		public MultistatementTableRelationalFunctionDefinitionFactory(SqlLiteralExpression variable, SqlTableFunctionReturnType returnType, List<SqlModuleOption> options, SqlMultistatementFunctionBodyDefinition bodyDefinition)
		{
			this.variable = variable;
			this.returnType = returnType;
			this.options = options;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlMultistatementTableRelationalFunctionDefinition(name, parameters, variable, returnType, options, bodyDefinition);
		}
	}

	private sealed class ScalarClrFunctionDefinitionFactory : FunctionDefinitionFactory
	{
		private readonly SqlScalarFunctionReturnType returnType;

		private readonly List<SqlModuleOption> options;

		private readonly SqlClrFunctionBodyDefinition bodyDefinition;

		public ScalarClrFunctionDefinitionFactory(SqlScalarFunctionReturnType returnType, List<SqlModuleOption> options, SqlClrFunctionBodyDefinition bodyDefinition)
		{
			this.returnType = returnType;
			this.options = options;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlScalarClrFunctionDefinition(name, parameters, returnType, options, bodyDefinition);
		}
	}

	private sealed class ScalarRelationalFunctionDefinitionFactory : FunctionDefinitionFactory
	{
		private readonly SqlScalarFunctionReturnType returnType;

		private readonly List<SqlModuleOption> options;

		private readonly SqlMultistatementFunctionBodyDefinition bodyDefinition;

		public ScalarRelationalFunctionDefinitionFactory(SqlScalarFunctionReturnType returnType, List<SqlModuleOption> options, SqlMultistatementFunctionBodyDefinition bodyDefinition)
		{
			this.returnType = returnType;
			this.options = options;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlScalarRelationalFunctionDefinition(name, parameters, returnType, options, bodyDefinition);
		}
	}

	private sealed class TableClrFunctionDefinitionFactory : FunctionDefinitionFactory
	{
		private readonly SqlTableFunctionReturnType returnType;

		private readonly List<SqlModuleOption> options;

		private readonly SqlSimpleOrderByClause orderClause;

		private readonly SqlClrFunctionBodyDefinition bodyDefinition;

		public TableClrFunctionDefinitionFactory(SqlTableFunctionReturnType returnType, List<SqlModuleOption> options, SqlSimpleOrderByClause orderClause, SqlClrFunctionBodyDefinition bodyDefinition)
		{
			this.returnType = returnType;
			this.options = options;
			this.orderClause = orderClause;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlTableClrFunctionDefinition(name, parameters, returnType, options, orderClause, bodyDefinition);
		}
	}

	private sealed class FunctionDefinitionErrorFactory : FunctionDefinitionFactory
	{
		private readonly SqlFunctionReturnType returnType;

		private readonly List<SqlModuleOption> options;

		private readonly SqlFunctionBodyDefinition bodyDefinition;

		public FunctionDefinitionErrorFactory(SqlFunctionReturnType returnType, List<SqlModuleOption> options, SqlFunctionBodyDefinition bodyDefinition)
		{
			this.returnType = returnType;
			this.options = options;
			this.bodyDefinition = bodyDefinition;
		}

		public override SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
		{
			return new SqlFunctionDefinitionError(name, parameters, returnType, options, bodyDefinition);
		}
	}

	public abstract SqlFunctionDefinition Create(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters);

	public static FunctionDefinitionFactory CreateTableRelationalFactory(FunctionVariableNameAndTableReturnTypePair pair, List<SqlModuleOption> options, SqlFunctionBodyDefinition bodyDefinition)
	{
		if (pair == null)
		{
			if (bodyDefinition.AsSqlInlineFunctionBodyDefinition != null)
			{
				return new InlineTableRelationalFunctionDefinitionFactory(options, bodyDefinition.AsSqlInlineFunctionBodyDefinition);
			}
			bodyDefinition.ReportSyntaxError();
		}
		else
		{
			if (bodyDefinition.AsSqlMultistatementFunctionBodyDefinition != null)
			{
				return new MultistatementTableRelationalFunctionDefinitionFactory(pair.Variable, pair.ReturnType, options, bodyDefinition.AsSqlMultistatementFunctionBodyDefinition);
			}
			bodyDefinition.ReportSyntaxError();
		}
		return new FunctionDefinitionErrorFactory(pair?.ReturnType, options, bodyDefinition);
	}

	public static FunctionDefinitionFactory CreateScalarFactory(SqlScalarFunctionReturnType returnType, List<SqlModuleOption> options, SqlFunctionBodyDefinition bodyDefinition)
	{
		if (bodyDefinition.AsSqlMultistatementFunctionBodyDefinition != null)
		{
			return new ScalarRelationalFunctionDefinitionFactory(returnType, options, bodyDefinition.AsSqlMultistatementFunctionBodyDefinition);
		}
		if (bodyDefinition.AsSqlClrFunctionBodyDefinition != null)
		{
			return new ScalarClrFunctionDefinitionFactory(returnType, options, bodyDefinition.AsSqlClrFunctionBodyDefinition);
		}
		bodyDefinition.ReportSyntaxError();
		return new FunctionDefinitionErrorFactory(returnType, options, bodyDefinition);
	}

	public static FunctionDefinitionFactory CreateTableClrFactory(SqlTableFunctionReturnType returnType, List<SqlModuleOption> options, SqlSimpleOrderByClause orderClause, SqlClrFunctionBodyDefinition bodyDefinition)
	{
		return new TableClrFunctionDefinitionFactory(returnType, options, orderClause, bodyDefinition);
	}
}
