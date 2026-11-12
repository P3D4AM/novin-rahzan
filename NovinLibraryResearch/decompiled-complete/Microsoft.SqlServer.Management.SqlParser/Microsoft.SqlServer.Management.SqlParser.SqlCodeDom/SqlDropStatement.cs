using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlDropStatement : SqlDdlStatement
{
	private struct ChildObjectLexScopeInfo
	{
		public readonly SqlCodeObject ChildObject;

		public readonly LexicalScope LexicalScope;

		public ChildObjectLexScopeInfo(SqlCodeObject child, LexicalScope lexicalScope)
		{
			ChildObject = child;
			LexicalScope = lexicalScope;
		}
	}

	private ChildObjectLexScopeInfo[] childObjectLexScopes;

	private bool isDropIfExists;

	internal sealed override bool IsCreateStatement => false;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => true;

	internal bool IsDropIfExists
	{
		get
		{
			return isDropIfExists;
		}
		set
		{
			isDropIfExists = value;
		}
	}

	internal void InitChildLexicalScopeList(int childCount)
	{
		if (childCount > 1)
		{
			childObjectLexScopes = new ChildObjectLexScopeInfo[childCount];
		}
	}

	internal LexicalScope RegisterChildLexicalScope(SqlCodeObject child, int index)
	{
		LexicalScope lexicalScope = GetLexicalScope();
		if (childObjectLexScopes != null)
		{
			lexicalScope = lexicalScope.CreateChildScope();
			childObjectLexScopes[index] = new ChildObjectLexScopeInfo(child, lexicalScope);
		}
		return lexicalScope;
	}

	internal sealed override LexicalScope GetChildLexicalScope(SqlCodeObject child)
	{
		LexicalScope lexicalScope = null;
		if (childObjectLexScopes != null)
		{
			for (int i = 0; i < childObjectLexScopes.Length; i++)
			{
				if (childObjectLexScopes[i].ChildObject == child)
				{
					lexicalScope = childObjectLexScopes[i].LexicalScope;
					break;
				}
			}
		}
		return lexicalScope ?? base.GetChildLexicalScope(child);
	}

	internal sealed override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		BindStatement(bindingContext);
	}

	internal abstract void BindStatement(BatchBindingContext bidingContext);

	internal override void Unbind()
	{
		base.Unbind();
		childObjectLexScopes = null;
	}

	internal void ReportCannotDropObjectError(SqlCodeObject codeObject, string objectName, DdlTargetObjectType objectType)
	{
		if (!base.IsInConditionalStatement && !IsDropIfExists)
		{
			string objectType2 = DdlTargetObjectTypeToString(objectType, lowerCase: true);
			codeObject.ReportBindError(SR.CannotDropObject(objectName, objectType2), ErrorFlags.UnresolvedDatabaseObjectReference);
		}
	}

	internal static void ReportCannotUseDropStatementError(SqlCodeObject objectIdentifier, DdlTargetObjectType expectedType, DdlTargetObjectType actualType)
	{
		string correctStmt = DdlTargetObjectTypeToString(actualType, lowerCase: false);
		string incorrectStmt = DdlTargetObjectTypeToString(expectedType, lowerCase: false);
		string objectName = objectIdentifier.ToString();
		string objectType = DdlTargetObjectTypeToString(actualType, lowerCase: true);
		objectIdentifier.ReportBindError(SR.CannotUseDrop(incorrectStmt, correctStmt, objectName, objectType), ErrorFlags.UnresolvedDatabaseObjectReference);
	}

	internal static void BindDropObjectStatement<T, N>(SqlDropStatement dropStatement, IDropObjectBinderHelper<T, N> binderHelper, BatchBindingContext bindingContext) where T : class, IMetadataObject where N : SqlCodeObject
	{
		ISqlCodeCollection<N> dropObjects = binderHelper.DropObjects;
		dropStatement.InitChildLexicalScopeList(dropObjects.Count);
		for (int i = 0; i < dropObjects.Count; i++)
		{
			N val = dropObjects[i];
			SqlIdentifier objectName = binderHelper.GetObjectName(val);
			if (!objectName.IsEmpty)
			{
				LexicalScope lexicalScope = dropStatement.RegisterChildLexicalScope(val, i);
				T val2 = binderHelper.FindDropMetadataObject(bindingContext, val, lexicalScope);
				if (val2 == null)
				{
					if (binderHelper.FindAnyMetadataObject(bindingContext, val, lexicalScope, out var actualType) == null)
					{
						dropStatement.ReportCannotDropObjectError(val, val.ToString(), dropStatement.TargetObjectType);
						dropStatement.BindingState |= BindingState.BindFailed;
					}
					else
					{
						DdlTargetObjectType targetObjectType = dropStatement.TargetObjectType;
						ReportCannotUseDropStatementError(val, targetObjectType, actualType);
						dropStatement.BindingState |= BindingState.BindFailed;
					}
				}
				else
				{
					binderHelper.ExecuteDrop(bindingContext, val2, lexicalScope);
					bindingContext.SetBoundObject(objectName, val2);
					dropStatement.BindingState |= BindingState.BindSucceeded;
				}
			}
			else
			{
				dropStatement.BindingState |= BindingState.BindFailed;
			}
		}
	}

	private static string DdlTargetObjectTypeToString(DdlTargetObjectType objectType, bool lowerCase)
	{
		switch (objectType)
		{
		case DdlTargetObjectType.Table:
			if (!lowerCase)
			{
				return "TABLE";
			}
			return "table";
		case DdlTargetObjectType.View:
			if (!lowerCase)
			{
				return "VIEW";
			}
			return "view";
		case DdlTargetObjectType.Procedure:
			if (!lowerCase)
			{
				return "PROCEDURE";
			}
			return "procedure";
		case DdlTargetObjectType.Function:
			if (!lowerCase)
			{
				return "FUNCTION";
			}
			return "function";
		case DdlTargetObjectType.Schema:
			if (!lowerCase)
			{
				return "SCHEMA";
			}
			return "schema";
		case DdlTargetObjectType.Database:
			if (!lowerCase)
			{
				return "DATABASE";
			}
			return "database";
		case DdlTargetObjectType.Login:
			if (!lowerCase)
			{
				return "LOGIN";
			}
			return "login";
		case DdlTargetObjectType.Synonym:
			if (!lowerCase)
			{
				return "SYNONYM";
			}
			return "synonym";
		case DdlTargetObjectType.Type:
			if (!lowerCase)
			{
				return "TYPE";
			}
			return "type";
		case DdlTargetObjectType.Trigger:
			if (!lowerCase)
			{
				return "TRIGGER";
			}
			return "trigger";
		case DdlTargetObjectType.User:
			if (!lowerCase)
			{
				return "USER";
			}
			return "user";
		case DdlTargetObjectType.Sequence:
			if (!lowerCase)
			{
				return "SEQUENCE";
			}
			return "sequence";
		default:
			return string.Empty;
		}
	}

	internal static SqlDropStatement Create(SqlDropObjectType dropObjectType, List<SqlObjectIdentifier> objects, SqlDropTriggerTargetScope targetScope)
	{
		bool flag = true;
		SqlDropStatement sqlDropStatement;
		switch (dropObjectType)
		{
		case SqlDropObjectType.Table:
			sqlDropStatement = new SqlDropTableStatement(objects);
			break;
		case SqlDropObjectType.Proc:
		case SqlDropObjectType.Procedure:
			sqlDropStatement = new SqlDropProcedureStatement(objects);
			break;
		case SqlDropObjectType.Function:
			sqlDropStatement = new SqlDropFunctionStatement(objects);
			break;
		case SqlDropObjectType.View:
			sqlDropStatement = new SqlDropViewStatement(objects);
			break;
		case SqlDropObjectType.Rule:
			sqlDropStatement = new SqlDropRuleStatement(objects);
			break;
		case SqlDropObjectType.Default:
			sqlDropStatement = new SqlDropDefaultStatement(objects);
			break;
		case SqlDropObjectType.Trigger:
			sqlDropStatement = new SqlDropTriggerStatement(objects, targetScope);
			flag = false;
			break;
		case SqlDropObjectType.Aggregate:
			sqlDropStatement = new SqlDropAggregateStatement(objects);
			break;
		case SqlDropObjectType.Synonym:
			sqlDropStatement = new SqlDropSynonymStatement(objects);
			break;
		case SqlDropObjectType.Sequence:
			sqlDropStatement = new SqlDropSequenceStatement(objects);
			break;
		case SqlDropObjectType.SecurityPolicy:
			sqlDropStatement = new SqlDropSecurityPolicyStatement(objects);
			break;
		default:
			sqlDropStatement = null;
			break;
		}
		if (flag && targetScope != 0)
		{
			sqlDropStatement.ReportParseError(SR.InvalidOnClause);
		}
		return sqlDropStatement;
	}
}
