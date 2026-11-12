namespace Stimulsoft.Report.Web;

public class StiDesignerEventArgs : StiWebEventArgs
{
	public StiDesignerCommand Command
	{
		get
		{
			if (base.RequestParams != null)
			{
				return base.RequestParams.Designer.Command;
			}
			return StiDesignerCommand.Undefined;
		}
	}

	public StiDesignerEventArgs(StiRequestParams requestParams)
		: base(requestParams)
	{
	}
}
