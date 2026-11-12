using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.AdvTree.Design;

public class NodeConnectorTypeEditor : UITypeEditor
{
	private const string OPTION_CREATE = "Create new connector";

	private const string OPTION_REMOVE = "Remove connector";

	private IWindowsFormsEditorService m_EditorService;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			NodeConnector nodeConnector = value as NodeConnector;
			ref IWindowsFormsEditorService editorService = ref m_EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (m_EditorService != null)
			{
				ListBox val = new ListBox();
				val.SelectedIndexChanged += SelectedChanged;
				if (nodeConnector == null)
				{
					val.Items.Add((object)"Create new connector");
				}
				else
				{
					val.Items.Add((object)"Remove connector");
				}
				m_EditorService.DropDownControl((Control)(object)val);
				IDesignerHost designerHost = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
				if (val.SelectedItem != null && designerHost != null)
				{
					if (val.SelectedItem.ToString() == "Create new connector")
					{
						NodeConnector nodeConnector2 = designerHost.CreateComponent(typeof(NodeConnector)) as NodeConnector;
						nodeConnector2.LineWidth = 1;
						value = nodeConnector2;
					}
					else if (val.SelectedItem.ToString() == "Remove connector")
					{
						value = null;
						designerHost.DestroyComponent(nodeConnector);
					}
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
