using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiMasterComponentQuickButton : StiQuickButton
{
	private IStiMasterComponent masterComponent;

	public override string Name => StiLocalization.Get("PropertyMain", "MasterComponent");

	public override Image Image => StiReportDesignImages.QuickButtons.MasterComponent();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.MasterComponent_Selected();

	public Image ImageClose => StiReportImages.Dictionary.Close();

	private void OnSetNull(object sender, EventArgs e)
	{
		try
		{
			IStiDesignerBase designer = ((StiComponent)masterComponent).Report.Designer;
			if (masterComponent.MasterComponent != null)
			{
				designer?.UndoRedoSave(StiLocalization.Get("FormTitles", "MasterComponentSelectForm"), onlyCurrentPage: true);
				masterComponent.MasterComponent = null;
				designer?.Refresh();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetMasterComponent(object sender, EventArgs e)
	{
		try
		{
			IStiDesignerBase designer = ((StiComponent)masterComponent).Report.Designer;
			ButtonItem buttonItem = sender as ButtonItem;
			if (masterComponent.MasterComponent != buttonItem.Tag)
			{
				designer?.UndoRedoSave(StiLocalization.Get("FormTitles", "MasterComponentSelectForm"), onlyCurrentPage: true);
				masterComponent.MasterComponent = buttonItem.Tag as StiComponent;
				designer?.Refresh();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override void OnClick(Control control, Point pos, StiComponent component, IStiDesignerBase designer)
	{
		try
		{
			ButtonItem cmQuickButtons = ((StiDesignerControl)designer.DesignerControl).cmQuickButtons;
			cmQuickButtons.SubItems.Clear();
			masterComponent = component as IStiMasterComponent;
			if (masterComponent == null)
			{
				return;
			}
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
			stiComponentsCollection.AddRange(component.Page.GetComponents());
			if (!(component.Parent is StiPage))
			{
				stiComponentsCollection.AddRange(component.Parent.Parent.Components);
			}
			StiSubReport subReportForPage = StiSubReport.GetSubReportForPage(component.Page);
			if (subReportForPage != null && subReportForPage.Parent != null)
			{
				StiContainer stiContainer = subReportForPage.Parent;
				if (stiContainer is StiChildBand)
				{
					StiBand master = (stiContainer as StiChildBand).GetMaster();
					if (master != null)
					{
						stiContainer = master;
					}
				}
				else if (stiContainer is StiPanel)
				{
					while (stiContainer is StiPanel && stiContainer.Parent != null)
					{
						stiContainer = stiContainer.Parent;
					}
				}
				stiComponentsCollection.Add(stiContainer);
			}
			if (component is StiCrossDataBand)
			{
				stiComponentsCollection.Add(component.Parent);
			}
			ButtonItem buttonItem = new ButtonItem
			{
				Text = "[" + Loc.Get("Report", "NotAssigned") + "]",
				Image = ImageClose,
				Checked = (masterComponent.MasterComponent == null)
			};
			buttonItem.Click += OnSetNull;
			cmQuickButtons.SubItems.Add(buttonItem);
			int num = 0;
			Hashtable hashtable = new Hashtable();
			foreach (StiComponent item in stiComponentsCollection)
			{
				if (item is IStiMasterComponent && item != component && (item as IStiMasterComponent).MasterComponent != component && !(item is StiEmptyBand) && hashtable[item] == null)
				{
					hashtable[item] = item;
					Bitmap val = StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + item.GetType().Name + ".png");
					if (val == null)
					{
						val = StiServiceBitmapAttribute.GetImage(typeof(StiComponent));
					}
					ButtonItem buttonItem2 = new ButtonItem
					{
						Image = StiImageResizer.GetImage((Image)(object)val),
						BeginGroup = (num == 0),
						Tag = item,
						Text = item.ToString(),
						Checked = (item == ((IStiMasterComponent)component).MasterComponent)
					};
					buttonItem2.Click += OnSetMasterComponent;
					cmQuickButtons.SubItems.Add(buttonItem2);
					num++;
				}
			}
			cmQuickButtons.PopupMenu(control.PointToScreen(pos));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
