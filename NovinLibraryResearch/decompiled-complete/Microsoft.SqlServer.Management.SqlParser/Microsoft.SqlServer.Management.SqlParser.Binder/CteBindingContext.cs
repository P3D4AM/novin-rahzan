using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class CteBindingContext : BatchBindingContext
{
	private ITabular m_activeCte;

	private CteBindMode m_bindMode;

	private CteQueryFlags m_queryFlags;

	private readonly TabularCollection m_commonTableExpressions;

	public CteBindMode CteBindMode
	{
		get
		{
			return m_bindMode;
		}
		set
		{
			m_bindMode = value;
		}
	}

	public CteQueryFlags QueryFlags
	{
		get
		{
			return m_queryFlags;
		}
		set
		{
			m_queryFlags = value;
		}
	}

	internal CteBindingContext(BatchBindingContext batch)
		: base(batch)
	{
		m_commonTableExpressions = new TabularCollection(base.DatabaseCollation);
	}

	public override void OnBindingToSelfCte()
	{
		m_queryFlags |= CteQueryFlags.HasSelfReference;
	}

	public override CteScopeSnapshot CaptureCurrentCteSnapshot()
	{
		if (m_bindMode != 0)
		{
			return new CteScopeSnapshot(m_activeCte, m_commonTableExpressions.Clone(copyData: true), m_bindMode, m_queryFlags);
		}
		return null;
	}

	public void EnterCteScope(ITabular cte, bool queryHasUnionAll)
	{
		m_activeCte = cte;
		m_bindMode = CteBindMode.Anchor;
		m_queryFlags = (queryHasUnionAll ? CteQueryFlags.HasUnionAll : CteQueryFlags.None);
	}

	public void LeaveCteScope(ITabular cte)
	{
		m_activeCte = null;
		m_bindMode = CteBindMode.None;
	}

	public void UpdateCteInScope(ITabular cte)
	{
		m_activeCte = cte;
	}

	public bool AddCommonTableExpression(ITabular cte, LexicalScope statement)
	{
		bool num = !m_commonTableExpressions.Contains(cte.Name);
		if (num)
		{
			m_commonTableExpressions.Add(cte);
		}
		return num;
	}

	public override ITabular FindCommonTableExpression(string name, out CteBindMode bindMode, out CteQueryFlags queryFlags)
	{
		ITabular result = m_commonTableExpressions[name];
		bindMode = CteBindMode.None;
		queryFlags = CteQueryFlags.None;
		return result;
	}

	public override IMetadataCollection<IMetadataObject> FindAll(CompletionContext context, LexicalScope lexicalScope, IMetadataCollection<IMetadataObject> commonTableExpressions)
	{
		if (commonTableExpressions == null)
		{
			commonTableExpressions = m_commonTableExpressions.AsMetadataObjectCollection;
		}
		return base.FindAll(context, lexicalScope, commonTableExpressions);
	}

	public override IMetadataCollection<IMetadataObject> FindAll(List<string> ids, CompletionContext context, LexicalScope lexicalScope, IMetadataCollection<IMetadataObject> commonTableExpressions)
	{
		if (commonTableExpressions == null)
		{
			commonTableExpressions = m_commonTableExpressions.AsMetadataObjectCollection;
		}
		return base.FindAll(ids, context, lexicalScope, commonTableExpressions);
	}
}
