namespace Stimulsoft.Editor;

public class VbParameterInfo : ParameterInfo
{
	public override IListMember CreateListMember()
	{
		return new VbListMember(this);
	}
}
