using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

internal class StiImageIndexEditor : UITypeEditor
{
	private ImageList imageList;

	private UITypeEditor imageEditor;

	private void SetImageList(ITypeDescriptorContext context)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (context == null || context.Container == null)
		{
			return;
		}
		foreach (object component in context.Container.Components)
		{
			if (component is StiMenuProvider)
			{
				imageList = ((StiMenuProvider)component).GetImageList((ToolStripMenuItem)context.Instance);
				break;
			}
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		SetImageList(context);
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return imageEditor.GetPaintValueSupported(context);
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		SetImageList(e.Context);
		if (imageList != null && imageList.Images.Count != 0 && imageEditor != null && e.Value is int)
		{
			int num = (int)e.Value;
			if (num >= 0 && num <= imageList.Images.Count)
			{
				imageEditor.PaintValue(new PaintValueEventArgs(e.Context, (object)imageList.Images[num], e.Graphics, e.Bounds));
			}
		}
	}

	public StiImageIndexEditor()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		imageEditor = (UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
	}
}
