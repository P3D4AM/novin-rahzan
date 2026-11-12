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

public class StiDataSourceQuickButton : StiQuickButton
{
	private IStiDataSource dataSourceComponent;

	private IStiBusinessObject businessObjectComponent;

	public override string Name => StiLocalization.Get("PropertyMain", "DataSource");

	public override Image Image => StiReportDesignImages.QuickButtons.DataSource();

	public override Image SelectedImage => StiReportDesignImages.QuickButtons.DataSource_Selected();

	public Image ImageClose => StiReportImages.Dictionary.Close();

	public Image ImageDataSource => StiReportImages.Dictionary.DataSource();

	private void OnSetNull(object sender, EventArgs e)
	{
		try
		{
			if (dataSourceComponent.DataSource != null)
			{
				IStiDesignerBase designer = ((StiComponent)dataSourceComponent).Report.Designer;
				designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "DataSource"), onlyCurrentPage: true);
				dataSourceComponent.DataSourceName = string.Empty;
				designer?.Refresh();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetDataSource(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			if (dataSourceComponent.DataSource != buttonItem.Tag)
			{
				IStiDesignerBase designer = ((StiComponent)dataSourceComponent).Report.Designer;
				designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "DataSource"), onlyCurrentPage: true);
				dataSourceComponent.DataSourceName = ((StiDataSource)buttonItem.Tag).Name;
				designer?.Refresh();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void OnSetBusinessObject(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			if (businessObjectComponent.BusinessObject != buttonItem.Tag)
			{
				IStiDesignerBase designer = ((StiComponent)dataSourceComponent).Report.Designer;
				designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "BusinessObject"), onlyCurrentPage: true);
				businessObjectComponent.BusinessObjectGuid = ((StiBusinessObject)buttonItem.Tag).Guid;
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
			dataSourceComponent = component as IStiDataSource;
			if (dataSourceComponent == null)
			{
				return;
			}
			businessObjectComponent = component as IStiBusinessObject;
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
			stiComponentsCollection.AddRange(component.Parent.Components);
			if (component is StiCrossDataBand)
			{
				stiComponentsCollection.Add(component.Parent);
			}
			ButtonItem buttonItem = new ButtonItem
			{
				Text = "[" + Loc.Get("Report", "NotAssigned") + "]",
				Image = ImageClose,
				Checked = (dataSourceComponent.DataSource == null && businessObjectComponent.BusinessObject == null)
			};
			buttonItem.Click += OnSetNull;
			cmQuickButtons.SubItems.Add(buttonItem);
			StiDataSourcesCollection dataSources = designer.Report.Dictionary.DataSources;
			int num = 0;
			foreach (StiDataSource item in dataSources)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					BeginGroup = (num == 0),
					Tag = item,
					Text = item.ToString(),
					Image = ImageDataSource,
					Checked = (item == dataSourceComponent.DataSource)
				};
				buttonItem2.Click += OnSetDataSource;
				cmQuickButtons.SubItems.Add(buttonItem2);
				num++;
			}
			if (designer.Report.Dictionary.BusinessObjects.Count > 0)
			{
				int num2 = 0;
				foreach (StiBusinessObject businessObject in designer.Report.Dictionary.BusinessObjects)
				{
					ButtonItem buttonItem3 = new ButtonItem
					{
						BeginGroup = (num2 == 0),
						Text = businessObject.ToString(),
						Image = StiDataBuilder.Images.Images["BusinessObject"],
						Tag = businessObject
					};
					buttonItem3.Click += OnSetBusinessObject;
					cmQuickButtons.SubItems.Add(buttonItem3);
					FillMenuItems(businessObject, buttonItem3);
					if (businessObjectComponent.BusinessObjectGuid != null && businessObject.Guid == businessObjectComponent.BusinessObjectGuid)
					{
						buttonItem3.Checked = true;
					}
					num2++;
				}
			}
			cmQuickButtons.PopupMenu(control.PointToScreen(pos));
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void FillMenuItems(StiBusinessObject parentBusinessObject, BaseItem item)
	{
		if (parentBusinessObject.BusinessObjects.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (StiBusinessObject businessObject in parentBusinessObject.BusinessObjects)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = businessObject.ToString(),
				Image = StiDataBuilder.Images.Images["BusinessObject"],
				Tag = businessObject
			};
			buttonItem.Click += OnSetBusinessObject;
			item.SubItems.Add(buttonItem);
			FillMenuItems(businessObject, buttonItem);
			if (businessObjectComponent.BusinessObjectGuid != null && businessObject.Guid == businessObjectComponent.BusinessObjectGuid)
			{
				buttonItem.Checked = true;
			}
			num++;
		}
	}
}
