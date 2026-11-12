using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class RibbonTabItemGroupTypeEditor : UITypeEditor
{
	private const string CREATE_NEW_GROUP = "<Create new group>";

	private IWindowsFormsEditorService m_EdSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null && provider != null)
		{
			m_EdSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (m_EdSvc != null)
			{
				RibbonTabItemGroupCollection ribbonTabItemGroupCollection = null;
				RibbonTabItem ribbonTabItem = null;
				RibbonStrip ribbonStrip = null;
				if (context.Instance is RibbonTabItem)
				{
					ribbonTabItem = (RibbonTabItem)context.Instance;
					ribbonStrip = ribbonTabItem.ContainerControl as RibbonStrip;
					if (ribbonStrip != null)
					{
						ribbonTabItemGroupCollection = ribbonStrip.TabGroups;
					}
				}
				if (ribbonTabItemGroupCollection == null && context.Instance != null)
				{
					MessageBox.Show("Unknow control using RibbonTabGroupEditor. Cannot edit groups. [" + context.Instance.ToString() + "]");
				}
				else if (ribbonTabItemGroupCollection == null)
				{
					MessageBox.Show("Unknow control using RibbonTabGroupEditor. Cannot edit groups. [context instance null]");
				}
				ListBox val = new ListBox();
				foreach (RibbonTabItemGroup item in ribbonTabItemGroupCollection)
				{
					val.Items.Add((object)item);
					if (item == ribbonTabItem.Group)
					{
						val.SelectedItem = item;
					}
				}
				val.Items.Add((object)"<Create new group>");
				val.SelectedIndexChanged += SelectedChanged;
				m_EdSvc.DropDownControl((Control)(object)val);
				value = ((!(val.SelectedItem is string) || !(val.SelectedItem.ToString() == "<Create new group>")) ? val.SelectedItem : DesignerSupport.CreateRibbonTabItemGroup(ribbonStrip, provider));
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (m_EdSvc != null)
		{
			m_EdSvc.CloseDropDown();
		}
	}
}
