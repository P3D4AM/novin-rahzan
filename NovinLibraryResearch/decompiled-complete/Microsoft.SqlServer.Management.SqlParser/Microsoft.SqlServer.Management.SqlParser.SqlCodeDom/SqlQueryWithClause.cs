using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlQueryWithClause : SqlCodeObject
{
	private CteBindingContext cteBindingContext;

	private readonly SqlCommonTableExpressionCollection cteExpressions;

	private readonly SqlChangeTrackingContext changeTrackingContext;

	private readonly SqlXmlNamespacesDeclaration xmlNamespacesDeclaration;

	internal override bool IsVisitable => true;

	internal sealed override bool CanBeBoundIndependently => true;

	internal CteBindingContext CTEBoundContext => cteBindingContext;

	public SqlCommonTableExpressionCollection CommonTableExpressions => cteExpressions;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (changeTrackingContext != null)
			{
				yield return changeTrackingContext;
			}
			if (xmlNamespacesDeclaration != null)
			{
				yield return xmlNamespacesDeclaration;
			}
			if (cteExpressions == null)
			{
				yield break;
			}
			foreach (SqlCommonTableExpression cteExpression in cteExpressions)
			{
				yield return cteExpression;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		if (cteExpressions == null)
		{
			base.BindingState = BindingState.BindSucceeded;
			return;
		}
		cteBindingContext = bindingContext.Batch.CreateCteBindingContext();
		foreach (SqlCommonTableExpression cteExpression in cteExpressions)
		{
			cteExpression.BindCte(cteBindingContext);
			base.BindingState |= cteExpression.BindingState;
		}
	}

	internal override void Unbind()
	{
		cteBindingContext = null;
		base.Unbind();
	}

	internal SqlQueryWithClause(List<SqlCommonTableExpression> expressions)
		: this(null, null, expressions)
	{
	}

	internal SqlQueryWithClause(SqlXmlNamespacesDeclaration xmlNamespacesDeclaration, List<SqlCommonTableExpression> expressions)
		: this(null, xmlNamespacesDeclaration, expressions)
	{
	}

	internal SqlQueryWithClause(SqlChangeTrackingContext changeTrackingContext, SqlXmlNamespacesDeclaration xmlNamespacesDeclaration, List<SqlCommonTableExpression> expressions)
	{
		this.changeTrackingContext = changeTrackingContext;
		this.xmlNamespacesDeclaration = xmlNamespacesDeclaration;
		if (expressions != null)
		{
			cteExpressions = new SqlCommonTableExpressionCollection(expressions);
		}
	}
}
