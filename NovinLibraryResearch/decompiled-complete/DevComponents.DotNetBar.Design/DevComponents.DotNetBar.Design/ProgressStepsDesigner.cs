using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ProgressStepsDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] array = null;
			array = new DesignerVerb[1]
			{
				new DesignerVerb("Add Item", CreateItem)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public ProgressStepsDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component != null && component.Site != null)
		{
			_ = component.Site.DesignMode;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	protected virtual void SetDesignTimeDefaults()
	{
		ProgressSteps progressSteps = ((ControlDesigner)this).Control as ProgressSteps;
		progressSteps.BackgroundStyle.Class = ElementStyleClassKeys.ProgressStepsKey;
		BaseItem defaultNewItemContainer = GetDefaultNewItemContainer();
		StepItem stepItem = CreateStepItem(defaultNewItemContainer);
		stepItem.Text = "Step 1";
		stepItem = CreateStepItem(defaultNewItemContainer);
		stepItem.Text = "Step 2";
		stepItem = CreateStepItem(defaultNewItemContainer);
		stepItem.Text = "Step 3";
	}

	private void CreateItem(object sender, EventArgs e)
	{
		OnSubItemsChanging();
		CreateStepItem(GetDefaultNewItemContainer());
		OnSubItemsChanged();
	}

	protected virtual StepItem CreateStepItem(BaseItem parent)
	{
		if (parent == null)
		{
			return null;
		}
		StepItem stepItem = null;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction("Adding new item");
			try
			{
				m_CreatingItem = true;
				stepItem = designerHost.CreateComponent(typeof(StepItem)) as StepItem;
				if (stepItem == null)
				{
					return null;
				}
				TypeDescriptor.GetProperties(stepItem)["Text"].SetValue(stepItem, stepItem.Name);
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanging(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"]);
				}
				parent.SubItems.Add(stepItem);
				RecalcLayout();
				if (parent != GetDefaultNewItemContainer())
				{
					componentChangeService?.OnComponentChanged(parent, TypeDescriptor.GetProperties(typeof(BaseItem))["SubItems"], null, null);
				}
				OnitemCreated(stepItem);
			}
			catch
			{
				designerTransaction.Cancel();
			}
			finally
			{
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
				m_CreatingItem = false;
			}
		}
		return stepItem;
	}

	private void RemoveDescriptors(IDictionary properties, string[] propNames)
	{
		foreach (string key in propNames)
		{
			if (properties.Contains(key))
			{
				if (properties[key] is PropertyDescriptor)
				{
					properties[key] = TypeDescriptor.CreateProperty(((ComponentDesigner)this).Component.GetType(), (PropertyDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else if (properties[key] is EventDescriptor)
				{
					properties[key] = TypeDescriptor.CreateEvent(((ComponentDesigner)this).Component.GetType(), (EventDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else
				{
					properties.Remove(key);
				}
			}
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[1] { "ForeColor" });
	}
}
