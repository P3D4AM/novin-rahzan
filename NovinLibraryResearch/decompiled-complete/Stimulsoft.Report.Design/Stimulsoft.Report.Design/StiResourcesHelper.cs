using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design;

public class StiResourcesHelper
{
	private static Assembly assembly;

	public static Image SetImage(ToolStripMenuItem button, string name, bool scale = false)
	{
		if (((ToolStripItem)button).Image != null)
		{
			((ToolStripItem)button).Image.Dispose();
			((ToolStripItem)button).Image = null;
		}
		((ToolStripItem)button).Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return ((ToolStripItem)button).Image;
	}

	public static Image SetImage(ButtonItem button, string name, bool scale = false)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return button.Image;
	}

	public static Image SetImage(StiColorButton button, string name, bool scale = false)
	{
		if (((ButtonBase)button).Image != null)
		{
			((ButtonBase)button).Image.Dispose();
			((ButtonBase)button).Image = null;
		}
		((ButtonBase)button).Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return ((ButtonBase)button).Image;
	}

	public static Image SetSmallImage(ButtonItem button, string name, bool scale = false)
	{
		if (button.ImageSmall != null)
		{
			button.ImageSmall.Dispose();
			button.ImageSmall = null;
		}
		button.ImageSmall = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return button.ImageSmall;
	}

	public static Image SetImage(ButtonX button, string name, bool scale = false)
	{
		button.Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return button.Image;
	}

	public static Image SetImage(ButtonBase button, string name, bool scale = false)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)GetImageByName(name))) : ((object)GetImageByName(name)));
		return button.Image;
	}

	public static Image SetImage(ButtonBase button, Bitmap image, bool scale = false)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(scale ? ((object)StiImageResizer.GetImage((Image)(object)image)) : ((object)image));
		return button.Image;
	}

	public static Image SetImage(ButtonItem button, Bitmap image)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(object)image;
		return button.Image;
	}

	public static Bitmap GetImageByName(string name)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		Stream manifestResourceStream = assembly.GetManifestResourceStream(name);
		if (manifestResourceStream == null)
		{
			return null;
		}
		Bitmap result = new Bitmap(manifestResourceStream);
		manifestResourceStream.Close();
		manifestResourceStream.Dispose();
		manifestResourceStream = null;
		return result;
	}

	public static Image SetImage(ButtonX button, string name)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(object)GetImageByName(name);
		return button.Image;
	}

	internal static void SetImage(ButtonX button, Bitmap image)
	{
		if (button.Image != null)
		{
			button.Image.Dispose();
			button.Image = null;
		}
		button.Image = (Image)(object)image;
	}

	static StiResourcesHelper()
	{
		assembly = typeof(StiResourcesHelper).Assembly;
	}
}
