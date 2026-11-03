namespace DevComponents.Editors;

public class CollectionChangedInfo
{
	private VisualItem[] _ᡸ;

	private VisualItem[] __1879;

	private eCollectionChangeType __187A;

	public VisualItem[] Added => _ᡸ;

	public VisualItem[] Removed => __1879;

	public eCollectionChangeType ChangeType => __187A;

	public CollectionChangedInfo(VisualItem[] added, VisualItem[] removed, eCollectionChangeType changeType)
	{
		_ᡸ = added;
		__1879 = removed;
		__187A = changeType;
	}
}
