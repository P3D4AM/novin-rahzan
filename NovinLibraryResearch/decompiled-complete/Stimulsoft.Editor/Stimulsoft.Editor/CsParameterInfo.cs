namespace Stimulsoft.Editor;

public class CsParameterInfo : ParameterInfo
{
	public override IListMember CreateListMember()
	{
		return new CsListMember(this);
	}
}
