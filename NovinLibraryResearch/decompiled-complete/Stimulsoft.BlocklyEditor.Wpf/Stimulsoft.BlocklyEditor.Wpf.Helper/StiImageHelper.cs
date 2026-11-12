using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Stimulsoft.Base;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiImageHelper
{
	private static string imagePath = "pack://application:,,,/Stimulsoft.BlocklyEditor.Wpf;component/Images/";

	public static Image GetImage(string id, int width = 16, int height = 16)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0058: Expected O, but got Unknown
		SystemScaleID id2 = StiScale.Id;
		string text = ((id2 >= SystemScaleID.x2) ? "_x2.png" : ".png");
		return new Image
		{
			Width = width,
			Height = height,
			Stretch = (Stretch)((id2 == SystemScaleID.x2) ? 2 : 0),
			Source = (ImageSource)new BitmapImage(new Uri(imagePath + id + text, UriKind.RelativeOrAbsolute))
		};
	}

	public static void SetBitmapImage(Image image, string icon, int width, int height)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		SystemScaleID id = StiScale.Id;
		string text = ((id >= SystemScaleID.x2) ? "_x2.png" : ".png");
		Stretch stretch = ((StiScale.Id != 0) ? ((Stretch)2) : ((Stretch)0));
		((UIElement)image).IsHitTestVisible = false;
		((FrameworkElement)image).Width = width;
		((FrameworkElement)image).Height = height;
		image.Stretch = stretch;
		image.Source = (ImageSource)new BitmapImage(new Uri(imagePath + icon + text, UriKind.RelativeOrAbsolute));
	}
}
