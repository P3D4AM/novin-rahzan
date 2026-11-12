using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Interaction;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiInteractionCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "Tools";

	public override int Position => 101;

	private void Interaction_Click(object sender, EventArgs e)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		try
		{
			base.Designer.UndoRedoSave(Loc.Get("PropertyMain", "Interaction"), onlyCurrentPage: true);
			List<StiComponent> source = base.Designer.GetSelected().ToList();
			StiComponent comp = source.FirstOrDefault();
			if (comp is StiBand)
			{
				source = source.Where((StiComponent c) => c is StiBand).ToList();
			}
			if (comp is StiCrossHeader)
			{
				source = source.Where((StiComponent c) => c is StiCrossHeader).ToList();
			}
			StiInteractionEditorForm stiInteractionEditorForm = new StiInteractionEditorForm(comp);
			try
			{
				if (comp.Report != null && comp.Report.Designer != null)
				{
					comp.Report.Designer.AddOwnedForm((Form)(object)stiInteractionEditorForm);
				}
				if ((int)((Form)stiInteractionEditorForm).ShowDialog() == 1)
				{
					JObject jsonObject = comp.Interaction.SaveToJsonObject(StiJsonSaveMode.Report);
					source.Where((StiComponent c) => c != comp).ToList().ForEach(delegate(StiComponent c)
					{
						c.Interaction.LoadFromJsonObject(jsonObject);
						c.Interaction.Bookmark.Value = comp.Interaction.Bookmark.Value;
						c.Interaction.ToolTip.Value = comp.Interaction.ToolTip.Value;
						c.Interaction.Tag.Value = comp.Interaction.Tag.Value;
						c.Interaction.Hyperlink.Value = comp.Interaction.Hyperlink.Value;
					});
					base.Designer.Builder.Redraw();
				}
			}
			finally
			{
				((IDisposable)stiInteractionEditorForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiInteractionCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiInteractionCTService)) as StiInteractionCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		base.Component = designer.GetSelected().ToList().FirstOrDefault();
		if (base.Designer != null)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = Loc.Get("PropertyMain", "Interaction"),
				Image = StiReportDesignImages.Actions.Interaction()
			};
			buttonItem.Click += Interaction_Click;
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}
}
