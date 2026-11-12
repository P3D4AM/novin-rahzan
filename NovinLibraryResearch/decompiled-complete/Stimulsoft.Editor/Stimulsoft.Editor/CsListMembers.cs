namespace Stimulsoft.Editor;

public class CsListMembers : ListMembers
{
	public override IListMember CreateListMember()
	{
		return new CsListMember(this);
	}
}
