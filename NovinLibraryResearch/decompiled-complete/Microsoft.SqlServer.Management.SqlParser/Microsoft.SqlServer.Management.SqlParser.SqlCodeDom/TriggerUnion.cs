using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal struct TriggerUnion<S, T, U, V> where S : class, IDmlTrigger, V where T : class, IDatabaseDdlTrigger, V where U : class, IServerDdlTrigger, V where V : class, ITrigger
{
	public readonly S DmlTrigger;

	public readonly T DatabaseDdlTrigger;

	public readonly U ServerDdlTrigger;

	public bool IsNull
	{
		get
		{
			if (DmlTrigger == null && DatabaseDdlTrigger == null)
			{
				return ServerDdlTrigger == null;
			}
			return false;
		}
	}

	public bool IsDmlTrigger => DmlTrigger != null;

	public bool IsDatabaseDdlTrigger => DatabaseDdlTrigger != null;

	public bool IsServerDdlTrigger => ServerDdlTrigger != null;

	public V AsTrigger
	{
		get
		{
			if (DmlTrigger != null)
			{
				return (V)DmlTrigger;
			}
			if (DatabaseDdlTrigger != null)
			{
				return (V)DatabaseDdlTrigger;
			}
			return (V)ServerDdlTrigger;
		}
	}

	public TriggerUnion(S dmlTrigger)
	{
		DmlTrigger = dmlTrigger;
		DatabaseDdlTrigger = null;
		ServerDdlTrigger = null;
	}

	public TriggerUnion(T databaseDdlTrigger)
	{
		DmlTrigger = null;
		DatabaseDdlTrigger = databaseDdlTrigger;
		ServerDdlTrigger = null;
	}

	public TriggerUnion(U serverDdlTrigger)
	{
		DmlTrigger = null;
		DatabaseDdlTrigger = null;
		ServerDdlTrigger = serverDdlTrigger;
	}
}
