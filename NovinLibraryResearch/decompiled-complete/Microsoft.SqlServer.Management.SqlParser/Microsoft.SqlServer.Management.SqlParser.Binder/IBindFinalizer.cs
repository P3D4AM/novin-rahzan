namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal interface IBindFinalizer
{
	bool IsFinalizing { get; }

	bool IsFinalized { get; }

	LexicalScope LexicalScope { get; }

	void FinalizeBind();

	void PostBindFinalize();
}
