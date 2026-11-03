using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Dashboard;

public interface IStiDashboardInteraction : ICloneable, IStiDefault
{
	StiInteractionIdent Ident { get; }

	StiInteractionOnHover OnHover { get; set; }

	StiInteractionOnClick OnClick { get; set; }

	StiInteractionOpenHyperlinkDestination HyperlinkDestination { get; set; }

	string ToolTip { get; set; }

	string Hyperlink { get; set; }

	string DrillDownPageKey { get; set; }

	List<IStiDashboardDrillDownParameter> GetDrillDownParameters();

	void SetDrillDownParameters(ArrayList drillDownParameters);
}
