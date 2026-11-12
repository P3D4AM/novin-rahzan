using System;

namespace Stimulsoft.Report.Web;

public class StiWebEventArgs : EventArgs
{
	public StiAction Action
	{
		get
		{
			if (RequestParams != null)
			{
				return RequestParams.Action;
			}
			return StiAction.Undefined;
		}
	}

	public StiRequestParams RequestParams { get; private set; }

	public StiWebEventArgs(StiRequestParams requestParams)
	{
		RequestParams = requestParams;
	}
}
