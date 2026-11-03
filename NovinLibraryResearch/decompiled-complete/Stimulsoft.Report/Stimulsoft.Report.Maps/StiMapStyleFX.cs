using System;
using System.ComponentModel;
using Stimulsoft.Base.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Maps;

public abstract class StiMapStyleFX : StiMapStyle
{
	public abstract string LocalizeName { get; }

	public virtual bool AllowDashboard => false;

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiMapStyleIdent StyleId
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiElementStyleIdent StyleIdent => StiElementStyleIdent.Custom;
}
