using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlStatement : SqlCodeObject, IVisitableSqlStatement, IVisitableSqlCodeObject
{
	private LexicalScope lexicalScope;

	private BatchBindingContext bindingContext;

	private SqlStatementReuseData reuseData;

	public sealed override SqlStatement Statement => this;

	internal sealed override bool CanBeBoundIndependently => true;

	internal abstract string StatementPhrase { get; }

	internal virtual bool IsTrueLastStatement => false;

	internal virtual bool IsModuleStatement => false;

	internal virtual bool IsReturnStatement => false;

	internal virtual bool IsConditionalStatement => false;

	internal virtual bool IsDdlStatement => false;

	internal virtual SqlDdlStatement AsDdlStatement => null;

	internal sealed override bool IsStatementLevelObject => true;

	internal sealed override SqlStatement AsStatement => this;

	internal sealed override IBindingContext BindingContext => bindingContext;

	internal sealed override bool HasAnyBindSkipped => (base.BindingState & BindingState.BindSkipped) == BindingState.BindSkipped;

	internal bool IsInConditionalStatement
	{
		get
		{
			bool result = false;
			for (SqlStatement statement = base.Parent.Statement; statement != null; statement = statement.Parent.Statement)
			{
				if (statement.IsConditionalStatement)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}

	internal SqlStatementReuseData ReuseData
	{
		get
		{
			return reuseData;
		}
		set
		{
			reuseData = value;
		}
	}

	internal override int LocalPositionOffset
	{
		get
		{
			if (reuseData == null)
			{
				return 0;
			}
			return reuseData.LocalPositionOffset;
		}
	}

	internal State BeforeParserState => reuseData.BeforeParserState;

	internal State AfterParserState => reuseData.AfterParserState;

	internal SimpleScannerState BeforeScannerState
	{
		get
		{
			SimpleScannerState beforeScannerStateRelative = reuseData.BeforeScannerStateRelative;
			return new SimpleScannerState(beforeScannerStateRelative.ScannerState, beforeScannerStateRelative.TokenIndex + reuseData.LocalPositionOffset);
		}
	}

	internal CompleteScannerState AfterScannerState
	{
		get
		{
			CompleteScannerState afterScannerStateRelative = reuseData.AfterScannerStateRelative;
			return new CompleteScannerState(afterScannerStateRelative.InitialScannerState, afterScannerStateRelative.FinalScannerState, afterScannerStateRelative.TokenIndex + reuseData.LocalPositionOffset, afterScannerStateRelative.IsTokenRead);
		}
	}

	internal int LookaheadTokenIndex => reuseData.LookAheadTokenIndexRelative + reuseData.LocalPositionOffset;

	internal LexLocation YYLoc
	{
		get
		{
			int localPositionOffset = reuseData.LocalPositionOffset;
			LexLocation yYLocRelative = reuseData.YYLocRelative;
			if (localPositionOffset == 0)
			{
				return yYLocRelative;
			}
			return new LexLocation(yYLocRelative.startTokenNumber + localPositionOffset, yYLocRelative.endTokenNumber + localPositionOffset);
		}
	}

	public abstract void Accept(ISqlStatementVisitor visitor);

	public abstract T Accept<T>(ISqlStatementVisitor<T> visitor);

	public abstract void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context);

	public abstract T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context);

	internal override LexicalScope GetLexicalScope()
	{
		return lexicalScope;
	}

	internal sealed override void RegisterDatabaseContextChange(DatabaseContext databaseContext)
	{
		if (!IsModuleStatement)
		{
			base.Parent.RegisterDatabaseContextChange(databaseContext);
		}
	}

	internal sealed override DatabaseContext GetDatabaseContext(LexicalScope lexScope)
	{
		if (!IsModuleStatement)
		{
			return base.Parent.GetDatabaseContext(lexScope);
		}
		return null;
	}

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		BatchBindingContext batch = bindingContext.Batch;
		Bind(batch, StatementBindMode.Sequential);
	}

	internal void Bind(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		lexicalScope = CreateLexicalScope(IsModuleStatement);
		DatabaseContext databaseContext = base.Parent.GetDatabaseContext(lexicalScope);
		if (databaseContext != null)
		{
			bindingContext = bindingContext.GetBindingContextForDatabase(databaseContext.Database);
		}
		this.bindingContext = bindingContext;
		BeforeBindStatement(bindingContext);
		BindStatement(bindingContext, bindMode);
		AfterBindStatement(bindingContext);
	}

	internal virtual void BeforeBindStatement(BatchBindingContext bindingContext)
	{
	}

	internal virtual void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		base.Bind(bindingContext);
	}

	internal virtual void AfterBindStatement(BatchBindingContext bindingContext)
	{
	}

	internal override void Unbind()
	{
		bindingContext = null;
		base.Unbind();
		lexicalScope = null;
	}

	internal override void PopulateRegions(List<Region> regions)
	{
		Location startLocation = base.StartLocation;
		Location endLocation = base.EndLocation;
		if (startLocation.LineNumber != endLocation.LineNumber)
		{
			AddRegion(regions, startLocation, endLocation);
		}
		foreach (SqlCodeObject child in Children)
		{
			child.PopulateRegions(regions);
		}
	}

	internal SqlStatement()
	{
	}

	internal override void SetParentPointers()
	{
		if (reuseData == null)
		{
			base.SetParentPointers();
		}
		else if (!reuseData.AreChildrenParentsSet)
		{
			base.SetParentPointers();
			reuseData.AreChildrenParentsSet = true;
		}
	}

	internal void DetachFromTree()
	{
		reuseData.LocalPositionOffset = base.AbsolutePositionOffset;
		ClearParent();
	}
}
