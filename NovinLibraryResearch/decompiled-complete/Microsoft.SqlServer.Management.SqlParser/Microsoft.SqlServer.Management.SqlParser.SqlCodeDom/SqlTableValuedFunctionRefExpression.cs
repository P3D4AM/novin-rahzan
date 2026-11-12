using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableValuedFunctionRefExpression : SqlTableExpression
{
	private readonly bool isSysTvfRefExpression;

	private readonly SqlObjectIdentifier objectIdentifier;

	private readonly SqlIdentifier alias;

	private readonly SqlScalarExpressionCollection arguments;

	private readonly SqlIdentifierCollection columnList;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName
	{
		get
		{
			if (alias == null)
			{
				return new TableExpressionName(objectIdentifier);
			}
			return new TableExpressionName(alias.Value);
		}
	}

	public SqlObjectIdentifier ObjectIdentifier => objectIdentifier;

	public SqlIdentifier Alias => alias;

	public SqlScalarExpressionCollection Arguments => arguments;

	public SqlIdentifierCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			bool hasAlias = false;
			if (alias != null)
			{
				if (alias.Position.Before(objectIdentifier.Position))
				{
					yield return alias;
					hasAlias = false;
				}
				else
				{
					hasAlias = true;
				}
			}
			yield return objectIdentifier;
			if (arguments != null)
			{
				foreach (SqlScalarExpression argument in arguments)
				{
					yield return argument;
				}
			}
			if (hasAlias)
			{
				yield return alias;
			}
			if (columnList == null)
			{
				yield break;
			}
			foreach (SqlIdentifier column in columnList)
			{
				yield return column;
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
		if (columnList != null)
		{
			BindMethod(bindingContext);
		}
		else
		{
			BindTVF(bindingContext);
		}
	}

	private void BindTVF(IBindingContext bindingContext)
	{
		BatchBindingContext batch = bindingContext.Batch;
		string value = objectIdentifier.DatabaseName.Value;
		string schemaName = (isSysTvfRefExpression ? "sys" : objectIdentifier.SchemaName.Value);
		string value2 = objectIdentifier.ObjectName.Value;
		LexicalScope lexicalScope = GetLexicalScope();
		ITabular tabular = null;
		ITableValuedFunction tableValuedFunction = null;
		IDatabaseTable databaseTable = null;
		tableValuedFunction = batch.FindTableValuedFunction(value, schemaName, value2, lexicalScope);
		if (tableValuedFunction != null)
		{
			tabular = tableValuedFunction;
		}
		else if (!isSysTvfRefExpression)
		{
			databaseTable = batch.FindTableOrView(value, schemaName, value2, lexicalScope);
			if (databaseTable != null)
			{
				string text = null;
				if (arguments != null && arguments.Count == 1 && arguments[0] is SqlColumnRefExpression { IsPrefixed: false } sqlColumnRefExpression)
				{
					text = sqlColumnRefExpression.ColumnName.Value;
				}
				if (text != null && SqlTableHint.GetHintFromString(text) != 0)
				{
					tabular = databaseTable;
				}
			}
		}
		if (tabular != null)
		{
			if (!SqlIdentifier.IsNullOrEmpty(alias))
			{
				tabular = bindingContext.MetadataFactory.Tabular.CreateTableAlias(tabular, alias.Value);
				SetBoundTable(bindingContext, tabular, alias);
			}
			else
			{
				SetBoundTable(bindingContext, tabular, objectIdentifier);
			}
		}
		else
		{
			string message = ((databaseTable == null) ? SR.InvalidObjectName(objectIdentifier.ToString()) : SR.ParametersSuppliedForNonFunction(objectIdentifier.ToString()));
			objectIdentifier.ReportBindError(message, ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState = BindingState.BindFailed;
		}
		if (base.BoundTable != null && tableValuedFunction != null)
		{
			base.BindingState |= BindUtils.Function.BindArguments(this, bindingContext, tableValuedFunction);
		}
	}

	private void BindMethod(IBindingContext bindingContext)
	{
		ITabular tabular = null;
		IColumn boundColumn = null;
		ITabular tabular2 = null;
		BindUtils.Column.LookupColumnPrefix(bindingContext, objectIdentifier, out var columnIndex, out tabular, out boundColumn);
		if (boundColumn != null)
		{
			int num = columnIndex + 1;
			IXmlDataType xmlDataType;
			if (num == objectIdentifier.Count - 1 && (xmlDataType = BindUtils.ScalarDataType.AsXmlDataType(BindUtils.DataType.AsScalarDataType(boundColumn.DataType))) != null)
			{
				SqlIdentifier sqlIdentifier = objectIdentifier[num];
				IFunction function = xmlDataType.TableMethods[sqlIdentifier.Value];
				if (function != null)
				{
					IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
					CollationInfo databaseCollation = bindingContext.DatabaseCollation;
					IMutableTabular mutableTabular = metadataFactory.Tabular.CreateDerivedTable((!SqlIdentifier.IsNullOrEmpty(alias)) ? alias.Value : string.Empty, databaseCollation);
					BindUtils.XmlDataType.PopulateTabularColumns(bindingContext, alias, columnList, mutableTabular);
					if (arguments != null)
					{
						base.BindingState |= BindUtils.Function.BindArguments(this, bindingContext, function);
					}
					tabular2 = mutableTabular;
				}
			}
		}
		if (tabular2 != null)
		{
			SetBoundTable(bindingContext, tabular2, (SqlCodeObject)(((object)alias) ?? ((object)this)));
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			objectIdentifier.ReportBindError(SR.InvalidObjectName(objectIdentifier.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void BindDmlTarget(DmlSpecificationBindingContext bindingContext)
	{
		string value = objectIdentifier.DatabaseName.Value;
		string value2 = objectIdentifier.SchemaName.Value;
		string value3 = objectIdentifier.ObjectName.Value;
		ITableValuedFunction tableValuedFunction = bindingContext.SetTableValuedFunctionTarget(value, value2, value3, GetLexicalScope());
		if (tableValuedFunction == null)
		{
			base.BindingState |= BindingState.BindFailed;
			objectIdentifier.ReportBindError(SR.InvalidObjectName(objectIdentifier.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			ITabular tabular = bindingContext.SetTableTarget(value, value2, value3, GetLexicalScope());
			if (tabular != null)
			{
				SetBoundDmlTarget(tabular, isAmbiguous: false);
			}
			return;
		}
		base.BindingState |= BindingState.BindSucceeded;
		if (bindingContext.IsTargetTableLocal)
		{
			base.BindingState |= BindingState.BindFailed;
			ReportBindError(SR.FunctionCannotBeUsedToMatchTarget(objectIdentifier.ToString()));
		}
		else if (!tableValuedFunction.IsInline)
		{
			base.BindingState |= BindingState.BindFailed;
			objectIdentifier.ReportBindError(SR.ObjectCannotBeModified(objectIdentifier.ToString()));
		}
		if (arguments != null)
		{
			base.BindingState |= BindUtils.Function.BindArguments(this, bindingContext, tableValuedFunction);
		}
		SetBoundDmlTarget(tableValuedFunction, bindingContext.IsTargetTableAmbiguous);
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == objectIdentifier)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.DatabaseQualifiedTableValuedFunctions);
		}
		else if (sqlCodeObject != null)
		{
			if (arguments != null)
			{
				foreach (SqlScalarExpression argument in arguments)
				{
					if (argument == sqlCodeObject)
					{
						result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.SelectScalar);
						break;
					}
				}
			}
		}
		else if ((arguments == null || arguments.Count == 0) && IsCaretBetweenEmptyParenthesis(caretLocation))
		{
			result = ((base.BoundTable == null || BindUtils.Table.IsTableValuedFunction(base.BoundTable.Unaliased)) ? RetrieveCompletionListItems(CompletionContext.SelectScalar) : RetrieveCompletionListItems(CompletionContext.Columns));
		}
		return result;
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject == objectIdentifier)
		{
			if (base.BoundTable != null)
			{
				result = objectIdentifier.GetQuickInfoText(cursorLocation, displayInfoProvider, base.BoundTable.Unaliased);
			}
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider);
		}
		return result;
	}

	internal override MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		MethodHelpText result = null;
		IFunction function = ((base.BoundTable != null) ? (base.BoundTable.Unaliased as IFunction) : null);
		if (function != null)
		{
			bool shouldShowParentheses = true;
			bool isVarArgs = false;
			result = BuildMethodHelpText(function, function.Parameters, null, "RETURNS TABLE", shouldShowParentheses, isVarArgs, displayInfoProvider);
		}
		return result;
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		MethodNameAndParamLocations result = null;
		if (BoundObject != null && BindUtils.Table.IsTableValuedFunction(base.BoundTable))
		{
			TokenManager tokenManager = TokenManager;
			MethodNameAndParamLocations methodNameAndParamLocations = new MethodNameAndParamLocations(BoundObject.Name)
			{
				NameStartLocation = base.StartLocation,
				NameEndLocation = base.EndLocation,
				ParamSeperatorLocations = new List<Location>()
			};
			BuildMethodNameAndParams(arguments: GetSqlCodeObjectList(Arguments), methodNameAndParams: methodNameAndParamLocations, nameEndPosition: objectIdentifier.Position.endTokenNumber);
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	internal SqlTableValuedFunctionRefExpression(SqlObjectIdentifier objectIdentifier, List<SqlScalarExpression> arguments)
		: this(objectIdentifier, arguments, null, null)
	{
	}

	internal SqlTableValuedFunctionRefExpression(SqlObjectIdentifier objectIdentifier, List<SqlScalarExpression> arguments, SqlIdentifier alias, List<SqlIdentifier> columnList)
		: this(objectIdentifier, arguments, alias, columnList, isSys: false)
	{
	}

	internal SqlTableValuedFunctionRefExpression(SqlObjectIdentifier objectIdentifier, List<SqlScalarExpression> arguments, SqlIdentifier alias, List<SqlIdentifier> columnList, bool isSys)
	{
		this.objectIdentifier = objectIdentifier;
		this.alias = alias;
		bool flag = arguments != null && arguments.Count > 0 && arguments[0] == null;
		if (arguments != null && !flag)
		{
			this.arguments = new SqlScalarExpressionCollection(arguments);
		}
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
		isSysTvfRefExpression = isSys;
	}
}
