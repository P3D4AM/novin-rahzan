using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class CteScopeSnapshot
{
	private ITabular m_selfCte;

	private CteBindMode m_bindMode;

	private CteQueryFlags m_queryFlags;

	private readonly IMutableMetadataCollection<ITabular> m_inScopeCteList;

	private readonly IMutableMetadataCollection<ITabular> m_inScopeCteAndSelfList;

	public ITabular SelfCte => m_selfCte;

	public IMetadataCollection<ITabular> InScopeCteList => m_inScopeCteList;

	public CteBindMode BindMode => m_bindMode;

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

	private bool IsRecursive => (m_bindMode & CteBindMode.Recursive) == CteBindMode.Recursive;

	private bool HasSelfReference => (m_queryFlags & CteQueryFlags.HasSelfReference) == CteQueryFlags.HasSelfReference;

	public CteScopeSnapshot(ITabular selfCte, IMutableMetadataCollection<ITabular> inScopeCteList, CteBindMode bindMode, CteQueryFlags queryFlags)
	{
		m_selfCte = selfCte;
		m_inScopeCteList = inScopeCteList;
		m_bindMode = bindMode;
		m_queryFlags = queryFlags;
		m_inScopeCteAndSelfList = m_inScopeCteList.Clone(copyData: true);
		if (!m_inScopeCteAndSelfList.Contains(m_selfCte.Name))
		{
			m_inScopeCteAndSelfList.Add(m_selfCte);
		}
	}

	public IMetadataCollection<IMetadataObject> GetCteCompletionList()
	{
		if (!IsRecursive || HasSelfReference)
		{
			return m_inScopeCteList.AsMetadataObjectCollection;
		}
		return m_inScopeCteAndSelfList.AsMetadataObjectCollection;
	}
}
