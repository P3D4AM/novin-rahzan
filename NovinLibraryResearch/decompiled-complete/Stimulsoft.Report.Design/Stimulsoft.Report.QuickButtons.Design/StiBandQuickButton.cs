using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiBandQuickButton : StiQuickButton
{
	public override string Name => StiLocalization.Get("Components", "StiDataBand");

	public override Image Image => StiReportDesignImages.QuickButtons.Band();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.Band_Selected();

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			designer.UndoRedoSave(StiLocalization.Get("Components", "StiDataBand"), onlyCurrentPage: true);
			StiBandForm stiBandForm = new StiBandForm(component, StiDataSelectionControl.StartTab.DataSource);
			try
			{
				component?.Report?.Designer?.AddOwnedForm((Form)(object)stiBandForm);
				((Form)stiBandForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiBandForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
