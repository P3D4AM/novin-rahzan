using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiSelectAllQuickButton : StiQuickButton
{
	public override string Name => StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditSelectAll"));

	public override Image Image => StiReportDesignImages.QuickButtons.SelectAll();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.SelectAll_Selected();

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		try
		{
			StiBand stiBand = component as StiBand;
			stiBand.SelectAll();
			designer.Report.GetCurrentPage().IsSelected = false;
			designer.Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
