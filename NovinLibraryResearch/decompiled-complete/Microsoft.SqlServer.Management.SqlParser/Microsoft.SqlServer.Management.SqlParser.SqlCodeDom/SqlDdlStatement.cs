using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlDdlStatement : SqlStatement, IBindFinalizer
{
	private enum FinalizeState
	{
		None,
		Finalizing,
		Finalized
	}

	private FinalizeState finalizeState;

	internal sealed override bool IsDdlStatement => true;

	internal sealed override SqlDdlStatement AsDdlStatement => this;

	internal abstract bool IsCreateStatement { get; }

	internal abstract bool IsAlterStatement { get; }

	internal abstract bool IsDropStatement { get; }

	internal abstract DdlTargetObjectType TargetObjectType { get; }

	private bool IsBindFinalizing => finalizeState == FinalizeState.Finalizing;

	private bool IsBindFinalized => finalizeState == FinalizeState.Finalized;

	bool IBindFinalizer.IsFinalizing => IsBindFinalizing;

	bool IBindFinalizer.IsFinalized => IsBindFinalized;

	LexicalScope IBindFinalizer.LexicalScope => GetLexicalScope();

	private void BeginBindFinalize()
	{
		finalizeState = FinalizeState.Finalizing;
	}

	private void EndBindFinalize()
	{
		finalizeState = FinalizeState.Finalized;
	}

	internal sealed override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		InitiateBindStatement(bindingContext, bindMode);
		if (bindMode == StatementBindMode.Sequential)
		{
			FinalizeBind(bindingContext, bindMode);
			PostBindFinalize();
		}
	}

	internal virtual void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		base.BindStatement(bindingContext, bindMode);
	}

	internal virtual void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
	}

	internal virtual void PostBindFinalize()
	{
	}

	internal override void Unbind()
	{
		base.Unbind();
		finalizeState = FinalizeState.None;
	}

	internal void FinalizeBind(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		if (!IsBindFinalized && !IsBindFinalizing)
		{
			BeginBindFinalize();
			FinalizeBindStatement(bindingContext, bindMode);
			EndBindFinalize();
		}
	}

	void IBindFinalizer.FinalizeBind()
	{
		if (!IsBindFinalized)
		{
			BeginBindFinalize();
			FinalizeBindStatement(BindingContext.Batch, StatementBindMode.OutOfOrder);
			EndBindFinalize();
		}
	}

	void IBindFinalizer.PostBindFinalize()
	{
		PostBindFinalize();
	}

	internal override void RunConsistencyChecks(string path, int depth)
	{
	}
}
