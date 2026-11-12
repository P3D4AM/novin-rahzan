using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DevComponents.AdvTree.Design;

public class ImageIndexEditor : UITypeEditor
{
	private ImageEditor m_ImageEditor;

	private ImageList m_ImageList;

	public ImageIndexEditor()
	{
		ref ImageEditor imageEditor = ref m_ImageEditor;
		object? editor = TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor));
		imageEditor = (ImageEditor)((editor is ImageEditor) ? editor : null);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			if (e != null && e.Value != null)
			{
				int index = (int)e.Value;
				Image image = GetImage(e.Context, index);
				if (image != null)
				{
					PaintValueEventArgs val = new PaintValueEventArgs(e.Context, (object)image, e.Graphics, e.Bounds);
					((UITypeEditor)m_ImageEditor).PaintValue(val);
				}
			}
		}
		catch
		{
		}
	}

	private Image GetImage(ITypeDescriptorContext context, int index)
	{
		if (m_ImageList != null && index >= 0 && index <= m_ImageList.Images.Count)
		{
			return m_ImageList.Images[index];
		}
		if (context == null)
		{
			return null;
		}
		object instance = context.Instance;
		if (instance == null)
		{
			return null;
		}
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
		if (properties == null)
		{
			return null;
		}
		foreach (PropertyDescriptor item in properties)
		{
			if ((object)item.PropertyType == typeof(ImageList))
			{
				ref ImageList imageList = ref m_ImageList;
				object? value = item.GetValue(instance);
				imageList = (ImageList)((value is ImageList) ? value : null);
				if (m_ImageList != null && index >= 0 && index <= m_ImageList.Images.Count)
				{
					return m_ImageList.Images[index];
				}
				break;
			}
		}
		return null;
	}
}
