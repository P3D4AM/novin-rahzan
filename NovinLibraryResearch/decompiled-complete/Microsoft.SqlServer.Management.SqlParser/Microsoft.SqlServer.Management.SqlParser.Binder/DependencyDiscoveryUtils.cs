using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public static class DependencyDiscoveryUtils
{
	private sealed class DependencyScriptWalker : SqlCodeObjectContextVisitor<bool, DependencyType>
	{
		private readonly List<Error> errors;

		private readonly HashSet<ObjectDependency> dependencies;

		public DependencyScriptWalker()
		{
			errors = new List<Error>();
			dependencies = new HashSet<ObjectDependency>();
		}

		public DependencyDiscoveryResult FindDependencies(SqlCodeObject codeObject)
		{
			DispatchVisitCodeObject(codeObject, DependencyType.Unknown);
			return new DependencyDiscoveryResult(dependencies, errors);
		}

		public void DispatchVisitCodeObject(SqlCodeObject codeObject, DependencyType dependencyType)
		{
			if (!codeObject.Accept(this, dependencyType))
			{
				if (codeObject is SqlStatement)
				{
					VisitStatement((SqlStatement)codeObject, dependencyType);
				}
				else
				{
					VisitCodeObject(codeObject, dependencyType);
				}
			}
		}

		private bool CanVisitChild(SqlCodeObject childObject)
		{
			if (childObject is SqlConstraint && !(childObject is SqlDefaultConstraint))
			{
				return false;
			}
			if (childObject is SqlDdlStatement)
			{
				return false;
			}
			return true;
		}

		private void DispatchVisit(IMetadataObject obj, DependencyType dependencyType)
		{
			if (obj is IResolvedSynonym)
			{
				Visit((IResolvedSynonym)obj, dependencyType);
			}
			else if (obj is IDatabaseObject)
			{
				Visit((IDatabaseObject)obj, dependencyType);
			}
			else if (obj is ITabular)
			{
				Visit((ITabular)obj, dependencyType);
			}
			else if (obj is IColumn)
			{
				Visit((IColumn)obj, dependencyType);
			}
		}

		private void Visit(IDatabaseObject obj, DependencyType dependencyType)
		{
			dependencies.Add(new ObjectDependency(obj, dependencyType));
		}

		private void Visit(ITabular obj, DependencyType dependencyType)
		{
			if (obj.Unaliased != null && obj.Unaliased != obj)
			{
				DispatchVisit(obj.Unaliased, dependencyType);
			}
		}

		private void Visit(IColumn obj, DependencyType dependencyType)
		{
			dependencies.Add(new ObjectDependency(obj, dependencyType));
		}

		private void Visit(IResolvedSynonym obj, DependencyType dependencyType)
		{
			dependencies.Add(new ObjectDependency(obj.Synonym, dependencyType));
			DispatchVisit(obj.TargetObject, dependencyType);
		}

		private static IMetadataObject GetBoundObject(SqlCodeObject codeObject)
		{
			return codeObject.BoundObject ?? codeObject.GetBoundObject<IMetadataObject>(codeObject);
		}

		private void VisitCodeObject(SqlCodeObject codeObject, DependencyType dependencyType)
		{
			IMetadataObject boundObject = GetBoundObject(codeObject);
			if (boundObject != null)
			{
				DispatchVisit(boundObject, dependencyType);
			}
			if (codeObject.IsSqlCodeSnippet)
			{
				Error item = new Error("Unsupported T-SQL language construct.", codeObject.StartLocation, codeObject.EndLocation, warningOnly: true, ErrorType.ParseError, ErrorFlags.None, codeObject.Position, codeObject);
				errors.Add(item);
				return;
			}
			foreach (SqlCodeObject child in codeObject.Children)
			{
				if (CanVisitChild(child))
				{
					DispatchVisitCodeObject(child, dependencyType);
				}
			}
		}

		public void VisitStatement(SqlStatement codeObject, DependencyType dependencyType)
		{
			VisitCodeObject(codeObject, (!codeObject.IsDdlStatement) ? DependencyType.ExecuteTime : dependencyType);
		}

		public override bool Visit(SqlSelectStarExpression codeObject, DependencyType dependencyType)
		{
			VisitCodeObject(codeObject, dependencyType);
			IMetadataCollection<IColumn> boundColumns = codeObject.BoundColumns;
			if (boundColumns != null)
			{
				foreach (IColumn item in boundColumns)
				{
					DispatchVisit(item, dependencyType);
				}
			}
			return true;
		}

		public override bool Visit(SqlInlineTableVariableDeclaration codeObject, DependencyType dependencyType)
		{
			VisitCodeObject(codeObject, DependencyType.CreateTime);
			return true;
		}

		public override bool Visit(SqlVariableDeclaration codeObject, DependencyType dependencyType)
		{
			VisitCodeObject(codeObject, DependencyType.CreateTime);
			return true;
		}

		public override bool Visit(SqlTableDefinition codeObject, DependencyType dependencyType)
		{
			VisitCodeObject(codeObject, dependencyType);
			if (codeObject.ColumnDefinitions != null)
			{
				foreach (SqlColumnDefinition columnDefinition in codeObject.ColumnDefinitions)
				{
					if (columnDefinition.Constraints == null)
					{
						continue;
					}
					foreach (SqlConstraint constraint in columnDefinition.Constraints)
					{
						DispatchVisitCodeObject(constraint, dependencyType);
					}
				}
			}
			return true;
		}
	}

	public static DependencyDiscoveryResult FindDependencies(ParseResult parseResult)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		return FindDependencies(parseResult.Script);
	}

	internal static DependencyDiscoveryResult FindDependencies(SqlCodeObject codeObj)
	{
		if (codeObj == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		return new DependencyScriptWalker().FindDependencies(codeObj);
	}
}
