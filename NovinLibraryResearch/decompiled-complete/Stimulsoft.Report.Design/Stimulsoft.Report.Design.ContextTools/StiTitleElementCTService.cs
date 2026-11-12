using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTitleElementCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Dashboard;

	public override string GroupCategory => "Elements";

	public override int Position => 200;

	public override Type GetInterfaceType()
	{
		return typeof(IStiTitleElement);
	}

	public static StiTitleElementCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService s) => s.GetType() == typeof(StiTitleElementCTService)) as StiTitleElementCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		IStiTitleElement stiTitleElement = (from c in designer.GetSelected().ToList()
			where c is IStiTitleElement
			select c).Cast<IStiTitleElement>().FirstOrDefault();
		if (base.Designer != null && stiTitleElement != null)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = Loc.Get("PropertyMain", "Title"),
				Checked = (stiTitleElement.Title != null && stiTitleElement.Title.Visible),
				Enabled = (stiTitleElement.Title != null),
				AutoCheckOnClick = true,
				AutoCollapseOnClick = false
			};
			buttonItem.Click += Title_Click;
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}

	private void Title_Click(object sender, EventArgs e)
	{
		try
		{
			ButtonItem item = sender as ButtonItem;
			base.Designer.UndoRedoSave(Loc.Get("PropertyMain", "Title"), onlyCurrentPage: true);
			(from IStiTitleElement c in from c in base.Designer.GetSelected().ToList()
					where c is IStiTitleElement
					select c
				where c.Title != null
				select c).ToList().ForEach(delegate(IStiTitleElement c)
			{
				c.Title.Visible = item.Checked;
			});
			base.Designer.Builder.Redraw();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
