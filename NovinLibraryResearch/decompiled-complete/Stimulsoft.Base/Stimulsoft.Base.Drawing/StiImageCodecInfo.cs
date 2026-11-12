using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace Stimulsoft.Base.Drawing;

public sealed class StiImageCodecInfo
{
	public static ImageCodecInfo GetImageCodec(string mimeType)
	{
		return ((IEnumerable<ImageCodecInfo>)ImageCodecInfo.GetImageEncoders()).FirstOrDefault((Func<ImageCodecInfo, bool>)((ImageCodecInfo info) => info.MimeType == mimeType));
	}
}
