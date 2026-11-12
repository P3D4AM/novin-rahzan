using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class TextMarkupUIEditor : UITypeEditor
{
	private IWindowsFormsEditorService m_EditorService;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService editorService = ref m_EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (m_EditorService != null)
			{
				TextMarkupEditor textMarkupEditor = new TextMarkupEditor();
				((Control)textMarkupEditor.buttonOK).Click += MarkupEditorButtonClick;
				((Control)textMarkupEditor.buttonCancel).Click += MarkupEditorButtonClick;
				if (value != null)
				{
					((Control)textMarkupEditor.inputText).Text = value.ToString();
				}
				m_EditorService.DropDownControl((Control)(object)textMarkupEditor);
				if ((int)textMarkupEditor.DialogResult == 1)
				{
					string text = ((Control)textMarkupEditor.inputText).Text;
					((Component)(object)textMarkupEditor).Dispose();
					return text;
				}
				((Component)(object)textMarkupEditor).Dispose();
			}
		}
		return value;
	}

	private void MarkupEditorButtonClick(object sender, EventArgs e)
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
