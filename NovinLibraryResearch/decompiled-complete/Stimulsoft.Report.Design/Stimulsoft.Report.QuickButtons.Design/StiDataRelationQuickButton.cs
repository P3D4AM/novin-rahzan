using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.QuickButtons.Design;

public class StiDataRelationQuickButton : StiQuickButton
{
	private IStiDataRelation dataRelation;

	public override string Name => StiLocalization.Get("PropertyMain", "Relation");

	public override Image Image => StiReportDesignImages.QuickButtons.DataRelation();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.DataRelation_Selected();

	public Image ImageClose => StiReportImages.Dictionary.Close();

	public Image ImageDataRelation => StiReportImages.Dictionary.DataRelation();

	private void OnSetNull(object sender, EventArgs e)
	{
		try
		{
			if (dataRelation.DataRelation != null)
			{
				IStiDesignerBase designer = ((StiComponent)dataRelation).Report.Designer;
				designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Relation"), onlyCurrentPage: true);
				dataRelation.DataRelationName = string.Empty;
				designer?.Refresh();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetDataRelation(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			if (dataRelation.DataRelation != buttonItem.Tag)
			{
				IStiDesignerBase designer = ((StiComponent)dataRelation).Report.Designer;
				designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Relation"), onlyCurrentPage: true);
				dataRelation.DataRelationName = ((StiDataRelation)buttonItem.Tag).NameInSource;
				if (dataRelation is IStiDataSource && ((IStiDataSource)dataRelation).DataSource == null)
				{
					((IStiDataSource)dataRelation).DataSourceName = dataRelation.DataRelation.ChildSource.Name;
				}
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
			dataRelation = component as IStiDataRelation;
			if (dataRelation == null)
			{
				return;
			}
			IStiDataSource stiDataSource = component as IStiDataSource;
			IStiMasterComponent stiMasterComponent = component as IStiMasterComponent;
			StiDataRelationsCollection stiDataRelationsCollection = null;
			if (stiDataSource != null && stiDataSource.DataSource != null)
			{
				stiDataRelationsCollection = stiDataSource.DataSource.GetParentRelations();
			}
			if ((stiDataRelationsCollection == null || stiDataRelationsCollection.Count == 0) && stiMasterComponent.MasterComponent != null && ((IStiDataSource)stiMasterComponent.MasterComponent).DataSource != null)
			{
				stiDataRelationsCollection = ((IStiDataSource)stiMasterComponent.MasterComponent).DataSource.GetChildRelations();
			}
			if (stiDataRelationsCollection == null)
			{
				return;
			}
			ButtonItem buttonItem = new ButtonItem
			{
				Text = "[" + StiLocalization.Get("Report", "NotAssigned") + "]",
				Image = ImageClose,
				Checked = (dataRelation.DataRelation == null)
			};
			buttonItem.Click += OnSetNull;
			cmQuickButtons.SubItems.Add(buttonItem);
			int num = 0;
			foreach (StiDataRelation item in stiDataRelationsCollection)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					BeginGroup = (num == 0),
					Tag = item,
					Text = item.ToString(),
					Image = ImageDataRelation,
					Checked = (item == dataRelation.DataRelation)
				};
				buttonItem2.Click += OnSetDataRelation;
				cmQuickButtons.SubItems.Add(buttonItem2);
				num++;
			}
			cmQuickButtons.PopupMenu(control.PointToScreen(pos));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
