using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlExecuteArgument : SqlCodeObject
{
	private IParameter boundParameter;

	private readonly SqlParameterOptionType optionTypes;

	private readonly bool isValid;

	private readonly SqlScalarVariableRefExpression parameter;

	private readonly SqlScalarExpression value;

	internal override bool IsVisitable => true;

	public IParameter BoundParameter => boundParameter;

	public override IMetadataObject BoundObject => boundParameter;

	public SqlScalarVariableRefExpression Parameter => parameter;

	public SqlScalarExpression Value => value;

	public bool IsOutput => (optionTypes & SqlParameterOptionType.Output) == SqlParameterOptionType.Output;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (parameter != null)
			{
				yield return parameter;
			}
			yield return value;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		if (isValid)
		{
			value.Bind(bindingContext);
			base.BindingState = value.BindingState;
		}
		else
		{
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal void Bind(IBindingContext bindingContext, IParameter parameter)
	{
		if (isValid)
		{
			BindUtils.Module.BindArgument(value, parameter, bindingContext);
			if (value.BindingState == BindingState.BindSucceeded)
			{
				boundParameter = parameter;
				base.BindingState = BindingState.BindSucceeded;
			}
			else
			{
				base.BindingState = value.BindingState;
			}
		}
		else
		{
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundParameter = null;
		base.Unbind();
	}

	internal SqlExecuteArgument(SqlScalarExpression value, SqlParameterOption option)
		: this(null, value, option)
	{
	}

	internal SqlExecuteArgument(SqlScalarVariableRefExpression parameter, SqlScalarExpression value, SqlParameterOption option)
	{
		this.parameter = parameter;
		this.value = value;
		optionTypes = SqlParameterOptionType.None;
		isValid = true;
		if (option == null)
		{
			return;
		}
		if (option.Type == SqlParameterOptionType.Invalid)
		{
			option.ReportParseError(SR.OptionNotRecognized(option.ToString()));
			isValid = false;
			return;
		}
		if (option.Type == SqlParameterOptionType.ReadOnly)
		{
			option.ReportParseError(SR.ReadonlyCannotBeUsed);
			isValid = false;
			return;
		}
		optionTypes |= option.Type;
		if (IsOutput && !this.value.IsVariableReference)
		{
			option.ReportParseError(SR.InvalidConstantOutput);
			isValid = false;
		}
	}
}
