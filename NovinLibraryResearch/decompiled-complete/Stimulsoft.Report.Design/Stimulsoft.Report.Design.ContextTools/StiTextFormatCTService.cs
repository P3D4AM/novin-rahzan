using System;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTextFormatCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "Tools";

	public override int Position => 100;

	private void TextFormat_Click(object sender, EventArgs e)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		try
		{
			base.Designer.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "TextFormat"), onlyCurrentPage: true);
			StiComponentsCollection selected = base.Designer.GetSelected();
			StiComponent stiComponent = null;
			foreach (StiComponent item in selected)
			{
				if (item is IStiTextFormat)
				{
					stiComponent = item;
					break;
				}
			}
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiComponent as IStiTextFormat, base.Designer);
			try
			{
				if (stiComponent.Report.Designer != null && stiComponent.Report.Designer.DesignerControl != null)
				{
					stiComponent.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				}
				if ((int)((Form)stiTextFormatEditorForm).ShowDialog() == 1)
				{
					foreach (StiComponent item2 in selected)
					{
						if (item2 is IStiTextFormat)
						{
							((IStiTextFormat)item2).TextFormat = (StiFormatService)((IStiTextFormat)stiComponent).TextFormat.Clone();
						}
					}
					((Control)base.Designer).Refresh();
				}
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
			base.Designer.Builder.Redraw();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override Type GetInterfaceType()
	{
		return typeof(IStiText);
	}

	public static StiTextFormatCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTextFormatCTService)) as StiTextFormatCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		StiComponentsCollection selected = designer.GetSelected();
		base.Component = null;
		foreach (StiComponent item in selected)
		{
			if (item is IStiText)
			{
				base.Component = item;
				break;
			}
		}
		if (base.Component != null)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuContextTextFormat"),
				Image = StiReportDesignImages.Actions.TextFormat()
			};
			buttonItem.Click += TextFormat_Click;
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}
}
