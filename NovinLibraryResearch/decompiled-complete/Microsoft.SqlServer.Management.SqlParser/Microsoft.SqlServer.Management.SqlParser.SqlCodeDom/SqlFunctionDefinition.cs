using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlFunctionDefinition : SqlCodeObject
{
	private readonly SqlObjectIdentifier name;

	private readonly SqlParameterDeclarationCollection parameters;

	internal readonly SqlCollection<SqlModuleOption> options;

	internal override bool IsVisitable => true;

	internal abstract bool IsTableValuedFunction { get; }

	internal abstract bool IsInlineTableFunction { get; }

	internal abstract bool IsClrFunction { get; }

	internal sealed override bool IsStatementLevelObject => true;

	public SqlObjectIdentifier Name => name;

	public SqlParameterDeclarationCollection Parameters => parameters;

	public SqlCollection<SqlModuleOption> Options => options;

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

	internal FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> CreateFunction(IUserDefinedFunctionFactory factory, ISchema schema)
	{
		string value = name.ObjectName.Value;
		if (IsTableValuedFunction)
		{
			IMutableTableValuedFunction mutableTableValuedFunction = factory.CreateTableValuedFunction(schema, value);
			mutableTableValuedFunction.IsSqlClr = IsClrFunction;
			mutableTableValuedFunction.IsInline = IsInlineTableFunction;
			return new FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction>(mutableTableValuedFunction);
		}
		IMutableScalarValuedFunction mutableScalarValuedFunction = factory.CreateScalarValuedFunction(schema, value);
		mutableScalarValuedFunction.IsSqlClr = IsClrFunction;
		return new FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction>(mutableScalarValuedFunction);
	}

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal void Bind(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion, bool setTextProperties)
	{
		IMutableUserDefinedFunction asUserDefinedFunction = functionUnion.AsUserDefinedFunction;
		if (parameters != null)
		{
			IMutableMetadataOrderedCollection<IParameter> mutableMetadataOrderedCollection = asUserDefinedFunction.Parameters;
			foreach (SqlParameterDeclaration parameter in parameters)
			{
				parameter.Bind(bindingContext, LexicalScopeOverride.UseRootScope);
				base.BindingState |= parameter.BindingState;
				if (parameter.BoundVariable != null)
				{
					mutableMetadataOrderedCollection.Add((IParameter)parameter.BoundVariable);
				}
			}
		}
		bool flag = true;
		if (options != null)
		{
			SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
			foreach (SqlModuleOption option in options)
			{
				option.Bind(bindingContext);
				base.BindingState |= option.BindingState;
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid || (sqlModuleOptionMask & typeMask) == typeMask)
				{
					continue;
				}
				switch (option.Type)
				{
				case SqlModuleOptionType.ExecuteAs:
					asUserDefinedFunction.ExecutionContext = option.ExecuteAsClause.BoundExecutionContext;
					flag = false;
					break;
				case SqlModuleOptionType.Encryption:
					if (!asUserDefinedFunction.IsSqlClr)
					{
						asUserDefinedFunction.IsEncrypted = true;
					}
					break;
				case SqlModuleOptionType.SchemaBinding:
					if (!asUserDefinedFunction.IsSqlClr)
					{
						asUserDefinedFunction.IsSchemaBound = true;
					}
					break;
				case SqlModuleOptionType.ReturnsNullOnNullInput:
					if (functionUnion.IsScalarValuedFunction)
					{
						functionUnion.ScalarValuedFunction.ReturnsNullOnNullInput = true;
					}
					break;
				}
				sqlModuleOptionMask |= typeMask;
			}
		}
		if (flag)
		{
			asUserDefinedFunction.ExecutionContext = bindingContext.MetadataFactory.ExecutionContext.CreateExecuteAsCaller();
		}
		if (setTextProperties)
		{
			SetFunctionTextProperties(asUserDefinedFunction);
		}
		BindDefinition(bindingContext, functionUnion);
	}

	internal virtual void BindDefinition(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion)
	{
		SqlFunctionReturnType returnType = GetReturnType();
		if (returnType != null)
		{
			returnType.Bind(bindingContext, functionUnion);
			base.BindingState |= returnType.BindingState;
		}
		SqlFunctionBodyDefinition bodyDefinition = GetBodyDefinition();
		if (bodyDefinition != null)
		{
			bodyDefinition.Bind(bindingContext, functionUnion);
			base.BindingState |= bodyDefinition.BindingState;
		}
	}

	private void SetFunctionTextProperties(IMutableUserDefinedFunction function)
	{
		if (!IsClrFunction)
		{
			SqlFunctionBodyDefinition bodyDefinition = GetBodyDefinition();
			if (bodyDefinition != null)
			{
				function.BodyText = bodyDefinition.Sql;
			}
		}
	}

	internal SqlFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, List<SqlModuleOption> options, SqlModuleOptionMask validOptions)
	{
		this.name = name;
		if (parameters != null)
		{
			SqlParameterDeclaration.Validate(parameters, SqlParameterOptionType.ReadOnly);
			this.parameters = new SqlParameterDeclarationCollection(parameters);
		}
		if (!SqlIdentifier.IsNullOrEmpty(this.name.DatabaseName))
		{
			this.name.DatabaseName.ReportParseError(SR.DbNameIsNotAllowedForCreateAlterFunc);
		}
		if (this.name.ObjectName.IsTempName)
		{
			this.name.ObjectName.ReportParseError(SR.TempFunctionNameIsNotAllowed);
		}
		SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
		if (options == null)
		{
			return;
		}
		this.options = new SqlCollection<SqlModuleOption>(options);
		foreach (SqlModuleOption option in options)
		{
			SqlModuleOptionMask typeMask = option.TypeMask;
			if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid)
			{
				option.ReportParseError(SR.OptionNotRecognized(option.DisplayName));
			}
			else if ((typeMask | validOptions) != validOptions)
			{
				option.ReportParseError(SR.InvalidOptionInCreateFunction);
			}
			else if ((sqlModuleOptionMask & typeMask) == typeMask)
			{
				option.ReportParseError(SR.OptionSpecifiedMultipleTimes(option.DisplayName));
			}
			else if (((sqlModuleOptionMask | typeMask) & SqlModuleOptionMask.NullInputOptions) == SqlModuleOptionMask.NullInputOptions)
			{
				option.ReportParseError(SR.ConflictingReturnsNullAndCalledOnNullInputOptions);
			}
			sqlModuleOptionMask |= typeMask;
		}
	}

	internal abstract SqlFunctionReturnType GetReturnType();

	internal abstract SqlFunctionBodyDefinition GetBodyDefinition();

	internal override void RunConsistencyChecks(string path, int depth)
	{
	}

	internal static SqlFunctionDefinition CreateFromFactory(FunctionDefinitionFactory factory, SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters)
	{
		return factory.Create(name, parameters);
	}
}
