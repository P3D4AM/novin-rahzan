namespace Stimulsoft.Editor;

public class JsParameterInfo : ParameterInfo
{
	public override IListMember CreateListMember()
	{
		return new JsListMember(this);
	}
}
