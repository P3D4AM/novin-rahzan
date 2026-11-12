namespace Stimulsoft.Controls.Win.Editors;

public class CollectionChangedInfo
{
	private VisualItem[] _Added;

	private VisualItem[] _Removed;

	private eCollectionChangeType _ChangeType;

	public VisualItem[] Added => _Added;

	public VisualItem[] Removed => _Removed;

	public eCollectionChangeType ChangeType => _ChangeType;

	public CollectionChangedInfo(VisualItem[] added, VisualItem[] removed, eCollectionChangeType changeType)
	{
		_Added = added;
		_Removed = removed;
		_ChangeType = changeType;
	}
}
