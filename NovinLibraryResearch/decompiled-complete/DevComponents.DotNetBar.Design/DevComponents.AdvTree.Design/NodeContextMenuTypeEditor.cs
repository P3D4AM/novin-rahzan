using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Design;

public class NodeContextMenuTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService m_EditorService;

	public static string DotNetBarPrefix = "DotNetBar.";

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService editorService = ref m_EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (m_EditorService != null)
			{
				ListBox val = new ListBox();
				ContextMenuBar contextMenuBar = null;
				val.SelectedIndexChanged += SelectedChanged;
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				foreach (IComponent component in designerHost.Container.Components)
				{
					if (component is ContextMenu || component.GetType().FullName == "System.Windows.Forms.ContextMenuStrip")
					{
						val.Items.Add((object)component);
					}
					if (!(component is ContextMenuBar))
					{
						continue;
					}
					contextMenuBar = component as ContextMenuBar;
					foreach (BaseItem item in contextMenuBar.Items)
					{
						val.Items.Add((object)(DotNetBarPrefix + item.Name));
					}
				}
				m_EditorService.DropDownControl((Control)(object)val);
				if (val.SelectedItem != null)
				{
					if (val.SelectedItem.ToString().StartsWith(DotNetBarPrefix))
					{
						if (context.Instance is Node { TreeControl: not null } node)
						{
							TypeDescriptor.GetProperties(node.TreeControl)["ContextMenuBar"].SetValue(node.TreeControl, contextMenuBar);
						}
						return contextMenuBar.Items[val.SelectedItem.ToString().Substring(DotNetBarPrefix.Length)];
					}
					return val.SelectedItem;
				}
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (m_EditorService != null)
		{
			m_EditorService.CloseDropDown();
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}
}
