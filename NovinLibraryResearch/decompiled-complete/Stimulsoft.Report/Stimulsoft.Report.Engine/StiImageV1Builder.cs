using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiImageV1Builder : StiViewV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		StiImage stiImage = masterComp as StiImage;
		bool result = base.InternalRender((StiComponent)stiImage, ref renderedComponent, outContainer);
		StiImage stiImage2 = renderedComponent as StiImage;
		if (stiImage2.ExistImageToDraw() && stiImage.Stretch && stiImage2.CanBreak)
		{
			RectangleD paintRectangle = stiImage.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			Bitmap val = new Bitmap((int)Math.Round((decimal)((RectangleD)(ref paintRectangle)).Width), (int)Math.Round((decimal)((RectangleD)(ref paintRectangle)).Height));
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				Image val3 = stiImage2.TakeGdiImage();
				try
				{
					if (val3 != null)
					{
						val2.DrawImage(val3, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height));
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			stiImage2.PutImageToDraw((Image)(object)val);
		}
		return result;
	}
}
