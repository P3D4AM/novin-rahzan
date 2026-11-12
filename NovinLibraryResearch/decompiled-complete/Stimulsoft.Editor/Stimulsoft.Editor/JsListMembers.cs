namespace Stimulsoft.Editor;

public class JsListMembers : ListMembers
{
	public override IListMember CreateListMember()
	{
		return new JsListMember(this);
	}
}
