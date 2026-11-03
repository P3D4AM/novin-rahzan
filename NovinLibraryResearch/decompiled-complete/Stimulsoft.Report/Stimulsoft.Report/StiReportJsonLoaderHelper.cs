using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

internal class StiReportJsonLoaderHelper
{
	internal List<IStiMasterComponent> MasterComponents { get; set; } = new List<IStiMasterComponent>();


	internal List<StiClone> Clones { get; set; } = new List<StiClone>();


	internal List<StiDialogInfo> DialogInfo { get; set; } = new List<StiDialogInfo>();


	internal void Clean()
	{
		MasterComponents.Clear();
		MasterComponents = null;
		Clones.Clear();
		Clones = null;
		DialogInfo.Clear();
		DialogInfo = null;
	}
}
