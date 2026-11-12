using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class MetroAppButtonDesigner : BaseItemDesigner
{
	private bool IsBackstageSet
	{
		get
		{
			MetroAppButton metroAppButton = (MetroAppButton)((ComponentDesigner)this).Component;
			if (metroAppButton != null && metroAppButton.BackstageTab != null)
			{
				return true;
			}
			return false;
		}
	}

	protected override DesignerVerb[] GetVerbs()
	{
		DesignerVerb[] verbs = base.GetVerbs();
		int num = verbs.Length + 1;
		if (IsBackstageSet)
		{
			num = 1;
		}
		bool flag = false;
		if (((ComponentDesigner)this).Component is MetroAppButton { BackstageTab: not null } metroAppButton && metroAppButton.SubItems.Count > 0)
		{
			flag = true;
			num++;
		}
		int num2 = 1;
		DesignerVerb[] array = new DesignerVerb[num];
		array[0] = new DesignerVerb(IsBackstageSet ? "Remove Backstage" : "Set Backstage", CreateBackstageTab);
		if (flag)
		{
			array[1] = new DesignerVerb("Clear Sub-items", ClearSubItems);
			num2++;
		}
		if (!IsBackstageSet)
		{
			for (int i = 0; i < verbs.Length; i++)
			{
				array[i + num2] = verbs[i];
			}
		}
		return array;
	}

	private void ClearSubItems(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show("Are you sure you want to delete all sub-items?", "DotNetBar Application Button", (MessageBoxButtons)4) == 7)
		{
			return;
		}
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null)
		{
			return;
		}
		MetroAppButton metroAppButton = (MetroAppButton)((ComponentDesigner)this).Component;
		if (metroAppButton.SubItems.Count == 0)
		{
			return;
		}
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Clearing Application Button SubItems");
		try
		{
			componentChangeService.OnComponentChanging(metroAppButton, TypeDescriptor.GetProperties(metroAppButton)["SubItems"]);
			BaseItem[] array = new BaseItem[metroAppButton.SubItems.Count];
			metroAppButton.SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				metroAppButton.SubItems.Remove(baseItem);
				designerHost.DestroyComponent(baseItem);
			}
			componentChangeService.OnComponentChanged(metroAppButton, TypeDescriptor.GetProperties(metroAppButton)["SubItems"], null, null);
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	private void CreateBackstageTab()
	{
		IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (designerHost == null)
		{
			return;
		}
		MetroAppButton metroAppButton = (MetroAppButton)((ComponentDesigner)this).Component;
		if (metroAppButton.BackstageTab != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Removing Backstage tab");
			try
			{
				SuperTabControl backstageTab = metroAppButton.BackstageTab;
				SetProperty(metroAppButton, "BackstageTab", null);
				((Control)backstageTab).Visible = true;
				SetProperty(backstageTab, "Location", new Point(0, 150));
				SetProperty(backstageTab, "Size", new Size(250, 350));
				((ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService)))?.SetSelectedComponents(new Control[1] { backstageTab }, SelectionTypes.Click);
				return;
			}
			catch
			{
				designerTransaction.Cancel();
				throw;
			}
			finally
			{
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
			}
		}
		DesignerTransaction designerTransaction2 = designerHost.CreateTransaction("Create Backstage");
		try
		{
			if (metroAppButton.Expanded)
			{
				metroAppButton.Expanded = false;
			}
			SuperTabControl superTabControl = (SuperTabControl)designerHost.CreateComponent(typeof(SuperTabControl));
			SetProperty(metroAppButton, "BackstageTab", superTabControl);
			IComponent rootComponent = designerHost.RootComponent;
			Control val = (Control)((rootComponent is Control) ? rootComponent : null);
			if (val != null)
			{
				componentChangeService.OnComponentChanging(val, TypeDescriptor.GetProperties(val)["Controls"]);
				val.Controls.Add((Control)(object)superTabControl);
				componentChangeService.OnComponentChanged(val, TypeDescriptor.GetProperties(val)["Controls"], null, null);
			}
			SetupBackstageTab(superTabControl, designerHost);
		}
		catch
		{
			designerTransaction2.Cancel();
			throw;
		}
		finally
		{
			if (!designerTransaction2.Canceled)
			{
				designerTransaction2.Commit();
			}
		}
	}

	private void CreateBackstageTab(object sender, EventArgs e)
	{
		CreateBackstageTab();
	}

	private void SetupBackstageTab(SuperTabControl backstageTab, IDesignerHost dh)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		((Control)backstageTab).Anchor = (AnchorStyles)15;
		backstageTab.ControlBox.Visible = false;
		backstageTab.ItemPadding.Left = 6;
		backstageTab.ItemPadding.Right = 4;
		backstageTab.ItemPadding.Top = 4;
		backstageTab.ReorderTabsEnabled = false;
		try
		{
			backstageTab.SelectedTabFont = new Font("Segoe UI", 9.75f);
		}
		catch
		{
		}
		backstageTab.SelectedTabIndex = 0;
		((Control)backstageTab).Size = new Size(614, 315);
		backstageTab.TabAlignment = eTabStripAlignment.Left;
		try
		{
			backstageTab.TabFont = new Font("Segoe UI", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		}
		catch
		{
		}
		backstageTab.TabHorizontalSpacing = 16;
		backstageTab.TabStyle = eSuperTabStyle.Office2010BackstageBlue;
		backstageTab.TabVerticalSpacing = 8;
		if (dh.GetDesigner((IComponent)backstageTab) is SuperTabControlDesigner superTabControlDesigner)
		{
			ButtonItem targetObject = superTabControlDesigner.CreateButton();
			SetProperty(targetObject, "Text", "Save");
			SetProperty(targetObject, "KeyTips", "S");
			SetProperty(targetObject, "Image", RibbonControlDesigner.LoadImage("Save16.png"));
			targetObject = superTabControlDesigner.CreateButton();
			SetProperty(targetObject, "Text", "Open");
			SetProperty(targetObject, "KeyTips", "O");
			SetProperty(targetObject, "Image", RibbonControlDesigner.LoadImage("Open.png"));
			targetObject = superTabControlDesigner.CreateButton();
			SetProperty(targetObject, "Text", "Close");
			SetProperty(targetObject, "KeyTips", "C");
			SetProperty(targetObject, "Image", RibbonControlDesigner.LoadImage("Close16.png"));
			SuperTabItem targetObject2 = superTabControlDesigner.CreateNewTab();
			SetProperty(targetObject2, "Text", "Recent");
			SetProperty(targetObject2, "KeyTips", "R");
			targetObject2 = superTabControlDesigner.CreateNewTab();
			SetProperty(targetObject2, "Text", "New");
			SetProperty(targetObject2, "KeyTips", "N");
			targetObject2 = superTabControlDesigner.CreateNewTab();
			SetProperty(targetObject2, "Text", "Print");
			SetProperty(targetObject2, "KeyTips", "P");
			targetObject2 = superTabControlDesigner.CreateNewTab();
			SetProperty(targetObject2, "Text", "Help");
			SetProperty(targetObject2, "KeyTips", "H");
			SetProperty((SuperTabControlPanel)(object)targetObject2.AttachedControl, "BackgroundImage", RibbonControlDesigner.LoadImage("BlueBackstageBgImage.png"));
			SetProperty((SuperTabControlPanel)(object)targetObject2.AttachedControl, "BackgroundImagePosition", eStyleBackgroundImage.BottomRight);
			targetObject = superTabControlDesigner.CreateButton();
			SetProperty(targetObject, "Text", "Options");
			SetProperty(targetObject, "KeyTips", "T");
			SetProperty(targetObject, "Image", RibbonControlDesigner.LoadImage("Options2.png"));
			targetObject = superTabControlDesigner.CreateButton();
			SetProperty(targetObject, "Text", "Exit");
			SetProperty(targetObject, "KeyTips", "X");
			SetProperty(targetObject, "Image", RibbonControlDesigner.LoadImage("Exit2.png"));
		}
	}

	private void SetProperty(object targetObject, string propertyName, object propertyValue)
	{
		TypeDescriptor.GetProperties(targetObject)[propertyName].SetValue(targetObject, propertyValue);
	}
}
