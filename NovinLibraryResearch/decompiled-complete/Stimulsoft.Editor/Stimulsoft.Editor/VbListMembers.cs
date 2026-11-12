namespace Stimulsoft.Editor;

public class VbListMembers : ListMembers
{
	public override IListMember CreateListMember()
	{
		return new VbListMember(this);
	}
}
