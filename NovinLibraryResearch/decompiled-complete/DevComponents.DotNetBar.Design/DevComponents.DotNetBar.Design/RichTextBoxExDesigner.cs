using System.Collections;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class RichTextBoxExDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		RichTextBoxEx richTextBoxEx = (RichTextBoxEx)(object)((ControlDesigner)this).Control;
		richTextBoxEx.BackgroundStyle.Class = ElementStyleClassKeys.RichTextBoxBorderKey;
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
	}
}
