using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

public class ImageIndexEditor : UITypeEditor
{
	private ImageList m_ImageList;

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e == null || e.Value == null)
			{
				return;
			}
			int index = (int)e.Value;
			Image image = GetImage(e.Context, index);
			if (image != null)
			{
				new PaintValueEventArgs(e.Context, (object)image, e.Graphics, e.Bounds);
				if (image != null)
				{
					Rectangle bounds = e.Bounds;
					bounds.Width--;
					bounds.Height--;
					e.Graphics.DrawRectangle(SystemPens.WindowFrame, bounds);
					e.Graphics.DrawImage(image, e.Bounds);
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
