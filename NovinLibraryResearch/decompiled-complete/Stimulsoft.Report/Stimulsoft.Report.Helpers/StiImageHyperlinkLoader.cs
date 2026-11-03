using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Helpers;

public static class StiImageHyperlinkLoader
{
	private static Hashtable failedAttempts = new Hashtable();

	public static bool AllowAsyncLoading { get; set; } = true;


	public static bool AllowCacheFails { get; set; } = true;


	public static void Load(StiImage image, string url)
	{
		if (AllowCacheFails && failedAttempts[url] != null)
		{
			return;
		}
		Task task = new Task(delegate
		{
			try
			{
				byte[] array = StiDownloadCache.Get(url, image.Report?.CookieContainer, image.Report?.HttpHeadersContainer);
				if (array == null && AllowCacheFails)
				{
					failedAttempts[url] = "";
				}
				else
				{
					image.PutImageToDraw(array);
					if (image.IsDesigning)
					{
						IStiDesignerBase designer = image.Report.Designer;
						Control val = (Control)((designer is Control) ? designer : null);
						if (val != null)
						{
							val.BeginInvoke((Delegate)(Action)delegate
							{
								image?.Report?.Designer?.Invalidate(image);
							});
						}
					}
				}
			}
			catch
			{
				if (AllowCacheFails)
				{
					failedAttempts[url] = url;
				}
			}
		});
		task.Start();
	}
}
