using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Wizards;

namespace Stimulsoft.Report.Design;

public static class StiReportNewDialog
{
	public static StiReport CreateNewReport(StiDesignerControl designer)
	{
		return CreateNewReport(designer, designer.Report);
	}

	public static StiReport CreateNewReport(StiReport masterReport)
	{
		return CreateNewReport(null, masterReport);
	}

	public static StiReport CreateNewReport(StiDesignerControl designer, StiReport masterReport)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		StiNewForm stiNewForm = null;
		try
		{
			if (designer != null)
			{
				stiNewForm = new StiNewForm(designer);
				designer.AddOwnedForm((Form)(object)stiNewForm);
			}
			else
			{
				stiNewForm = new StiNewForm(masterReport);
				masterReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiNewForm);
			}
			return ((int)((Form)stiNewForm).ShowDialog() == 1) ? stiNewForm.CreatedReport : null;
		}
		finally
		{
			((Component)(object)stiNewForm).Dispose();
		}
	}
}
