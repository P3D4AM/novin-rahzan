using System.Drawing;

namespace Stimulsoft.Report.Components;

public class StiNullImage
{
	private static Image nullImage;

	public static Image Null
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			object obj = nullImage;
			if (obj == null)
			{
				Bitmap val = new Bitmap(1, 1);
				nullImage = (Image)val;
				obj = (object)val;
			}
			return (Image)obj;
		}
	}
}
