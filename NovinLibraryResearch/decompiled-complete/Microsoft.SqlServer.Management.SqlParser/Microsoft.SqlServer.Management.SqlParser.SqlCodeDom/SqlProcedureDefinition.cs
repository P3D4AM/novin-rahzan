using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlProcedureDefinition : SqlCodeObject
{
	protected readonly SqlObjectIdentifier name;

	protected readonly int? number;

	protected readonly SqlParameterDeclarationCollection parameters;

	protected readonly SqlCollection<SqlModuleOption> options;

	protected readonly bool isForReplication;

	internal override bool IsVisitable => true;

	public SqlObjectIdentifier Name => name;

	public int? Number => number;

	public SqlParameterDeclarationCollection Parameters => parameters;

	public bool IsForReplication => isForReplication;

	public SqlCollection<SqlModuleOption> Options => options;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return name;
			if (parameters != null)
			{
				foreach (SqlParameterDeclaration parameter in parameters)
				{
					yield return parameter;
				}
			}
			if (options == null)
			{
				yield break;
			}
			foreach (SqlModuleOption option in options)
			{
				yield return option;
			}
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

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal void Bind(BatchBindingContext bindingContext, IMutableStoredProcedure storedProc)
	{
		if (parameters != null)
		{
			IMutableMetadataOrderedCollection<IParameter> mutableMetadataOrderedCollection = storedProc.Parameters;
			foreach (SqlParameterDeclaration parameter in parameters)
			{
				parameter.Bind(bindingContext, LexicalScopeOverride.PartitionScope);
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
				if ((typeMask & SqlModuleOptionMask.Invalid) != SqlModuleOptionMask.Invalid && (sqlModuleOptionMask & typeMask) != typeMask)
				{
					switch (option.Type)
					{
					case SqlModuleOptionType.ExecuteAs:
						storedProc.ExecutionContext = option.ExecuteAsClause.BoundExecutionContext;
						flag = false;
						break;
					case SqlModuleOptionType.Encryption:
						storedProc.IsEncrypted = true;
						break;
					case SqlModuleOptionType.Recompile:
						storedProc.IsRecompiled = true;
						break;
					}
					sqlModuleOptionMask |= typeMask;
				}
			}
		}
		if (flag)
		{
			storedProc.ExecutionContext = bindingContext.MetadataFactory.ExecutionContext.CreateExecuteAsCaller();
		}
		storedProc.ForReplication = isForReplication;
		storedProc.ReturnType = bindingContext.FindSystemDataType(DataTypeSpec.Int);
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal SqlProcedureDefinition(SqlObjectIdentifier name, SqlLiteralExpression number, List<SqlParameterDeclaration> parameters, List<SqlModuleOption> options, bool forReplication)
	{
		this.name = name;
		if (!SqlIdentifier.IsNullOrEmpty(this.name.DatabaseName))
		{
			this.name.DatabaseName.ReportParseError(SR.DbNameIsNotAllowedForCreateAlterProc);
		}
		this.number = null;
		if (number != null)
		{
			this.number = number.AsInt32();
			if (!this.number.HasValue)
			{
				number.ReportSyntaxError();
			}
			else if (this.number.Value == 0 || this.number.Value > 32767)
			{
				number.ReportParseError(SR.InvalidProcedureNumberRange(this.number.Value));
				this.number = null;
			}
		}
		if (parameters != null)
		{
			SqlParameterDeclaration.Validate(parameters, SqlParameterOptionType.ProcedureValidOptions);
			this.parameters = new SqlParameterDeclarationCollection(parameters);
		}
		SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
		if (options != null)
		{
			this.options = new SqlCollection<SqlModuleOption>(options);
			foreach (SqlModuleOption option in options)
			{
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid)
				{
					option.ReportParseError(SR.OptionNotRecognized(option.DisplayName));
				}
				else if ((typeMask | SqlModuleOptionMask.CreateProcedure) != SqlModuleOptionMask.CreateProcedure)
				{
					option.ReportParseError(SR.InvalidOptionInCreateProcedure);
				}
				else if ((sqlModuleOptionMask & typeMask) == typeMask)
				{
					option.ReportParseError(SR.OptionSpecifiedMultipleTimes(option.DisplayName));
				}
				sqlModuleOptionMask |= typeMask;
			}
		}
		isForReplication = forReplication;
	}

	internal SqlProcedureDefinition(SqlProcedureDefinition sqlProcedureDefinition)
	{
		name = sqlProcedureDefinition.Name;
		number = sqlProcedureDefinition.Number;
		parameters = sqlProcedureDefinition.Parameters;
		isForReplication = sqlProcedureDefinition.IsForReplication;
		options = sqlProcedureDefinition.Options;
	}
}
