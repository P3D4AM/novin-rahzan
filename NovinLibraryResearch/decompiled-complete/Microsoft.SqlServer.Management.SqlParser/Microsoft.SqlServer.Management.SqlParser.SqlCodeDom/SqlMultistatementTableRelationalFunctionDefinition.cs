using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMultistatementTableRelationalFunctionDefinition : SqlFunctionDefinition
{
	private ITableVariable boundTable;

	private const SqlModuleOptionMask ValidOptions = SqlModuleOptionMask.CreateTrigger | SqlModuleOptionMask.SchemaBinding;

	private readonly SqlLiteralExpression variable;

	private readonly SqlTableFunctionReturnType returnType;

	private readonly SqlMultistatementFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction => true;

	internal sealed override bool IsInlineTableFunction => false;

	internal sealed override bool IsClrFunction => false;

	public ITableVariable BoundTable => boundTable;

	public override IMetadataObject BoundObject => boundTable;

	public string VariableName => variable.Value;

	public SqlTableFunctionReturnType ReturnType => returnType;

	public SqlMultistatementFunctionBodyDefinition BodyDefinition => bodyDefinition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (base.Parameters != null)
			{
				foreach (SqlParameterDeclaration parameter in base.Parameters)
				{
					yield return parameter;
				}
			}
			yield return returnType;
			if (options != null)
			{
				foreach (SqlModuleOption option in options)
				{
					yield return option;
				}
			}
			yield return bodyDefinition;
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void BindDefinition(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion)
	{
		IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
		returnType.Bind(bindingContext, functionUnion);
		base.BindingState |= returnType.BindingState;
		IMutableTableValuedFunction tableValuedFunction = functionUnion.TableValuedFunction;
		tableValuedFunction.TableVariableName = VariableName;
		IMutableTableDataType mutableTableDataType = metadataFactory.DataType.CreateTableDataType(VariableName, tableValuedFunction.CollationInfo);
		mutableTableDataType.Columns.AddRange(tableValuedFunction.Columns);
		mutableTableDataType.Constraints.AddRange(tableValuedFunction.Constraints);
		mutableTableDataType.Indexes.AddRange(tableValuedFunction.Indexes);
		LexicalScope lexicalScope = GetLexicalScope();
		boundTable = metadataFactory.Variable.CreateTableVariable(VariableName, mutableTableDataType);
		if (!bindingContext.Batch.AddLocalTableVariable(boundTable, lexicalScope))
		{
			variable.ReportBindError(SR.VariableNameNotUnique(VariableName));
			base.BindingState |= BindingState.BindFailed;
		}
		bodyDefinition.Bind(bindingContext, functionUnion);
		base.BindingState |= bodyDefinition.BindingState;
	}

	internal override void Unbind()
	{
		boundTable = null;
		base.Unbind();
	}

	internal SqlMultistatementTableRelationalFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, SqlLiteralExpression variable, SqlTableFunctionReturnType returnType, List<SqlModuleOption> options, SqlMultistatementFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, SqlModuleOptionMask.CreateTrigger | SqlModuleOptionMask.SchemaBinding)
	{
		this.variable = variable;
		this.returnType = returnType;
		this.bodyDefinition = bodyDefinition;
		BodyDefinition.CompoundStatement.Accept(ValidateModuleBodyVisitor.TableFunctionParserVisitor);
		SqlStatementCollection statements = BodyDefinition.CompoundStatement.Statements;
		if (!((statements.Count > 0) ? statements[statements.Count - 1] : null).IsReturnStatement)
		{
			base.Name.ReportParseError(SR.LastStatementWithinFunctionMustBeReturn);
		}
	}

	internal override SqlFunctionReturnType GetReturnType()
	{
		return returnType;
	}

	internal override SqlFunctionBodyDefinition GetBodyDefinition()
	{
		return bodyDefinition;
	}
}
