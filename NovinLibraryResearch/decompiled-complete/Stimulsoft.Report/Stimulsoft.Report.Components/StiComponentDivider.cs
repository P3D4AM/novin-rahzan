using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components;

public class StiComponentDivider
{
	internal static string BreakText(Graphics g, RectangleD rect, ref string text, Font font, StiTextOptions textOptions, StiTextQuality textQuality, bool allowHtmlTags, StiText textComp)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (textComp?.Report != null)
		{
			if (textComp.Report.IsWpf && !allowHtmlTags)
			{
				return StiWpfTextRender.BreakText(rect, ref text, textComp);
			}
			if (StiOptions.Engine.UseNewHtmlEngine && textComp.AllowHtmlTags)
			{
				return StiHtmlTextRender.BreakText(rect, ref text, textComp);
			}
			if (StiDpiHelper.IsWindows && (textQuality == StiTextQuality.Wysiwyg || allowHtmlTags))
			{
				return StiTextRenderer.BreakText(g, ref text, font, rect, StiBrush.ToColor(textComp.TextBrush), StiBrush.ToColor(textComp.Brush), (double)textComp.LineSpacing, textComp.HorAlignment, textOptions.WordWrap, textOptions.RightToLeft, 1.0, (double)textOptions.Angle, textOptions.Trimming, allowHtmlTags, textOptions, (double?)1.0);
			}
		}
		string text2 = null;
		StringFormat stringFormat = textOptions.GetStringFormat(textQuality == StiTextQuality.Typographic, 1f);
		try
		{
			if (!textOptions.WordWrap)
			{
				((RectangleD)(ref rect)).Width = 10000000.0;
			}
			if ((int)textComp.HorAlignment == 3)
			{
				RectangleD val = default(RectangleD);
				((RectangleD)(ref val))._002Ector(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).Width / StiDpiHelper.DeviceCapsScale, ((RectangleD)(ref rect)).Height / StiDpiHelper.DeviceCapsScale);
				return StiTextDrawing.BreakTextWidth(g, ref text, font, val, stringFormat, (double)textComp.LineSpacing);
			}
			stringFormat.FormatFlags = (StringFormatFlags)8192;
			int num = 0;
			int num2 = 0;
			if (!StiDpiHelper.IsWindows)
			{
				string text3 = text;
				text2 = StiTextDrawing.CutLineLimit(ref text3, g, font, new RectangleD(0.0, 0.0, ((RectangleD)(ref rect)).Width / StiDpiHelper.DeviceCapsScale, ((RectangleD)(ref rect)).Height / (StiDpiHelper.DeviceCapsScale * (double)textComp.LineSpacing)), textOptions, textQuality == StiTextQuality.Typographic, false);
				num = text2.Length;
				text = text3;
			}
			else
			{
				try
				{
					g.MeasureString(text, font, new SizeF((float)(((RectangleD)(ref rect)).Width / StiDpiHelper.DeviceCapsScale), (float)(((RectangleD)(ref rect)).Height / (StiDpiHelper.DeviceCapsScale * (double)textComp.LineSpacing))), stringFormat, ref num, ref num2);
				}
				catch
				{
					if (text.Length > 16384)
					{
						try
						{
							g.MeasureString(text.Substring(0, 16384), font, new SizeF((float)(((RectangleD)(ref rect)).Width / StiDpiHelper.DeviceCapsScale), (float)(((RectangleD)(ref rect)).Height / StiDpiHelper.DeviceCapsScale)), stringFormat, ref num, ref num2);
						}
						catch
						{
						}
					}
				}
				text2 = text.Substring(0, num);
				if (num2 == 1 && (double)g.MeasureString(text2, font, (int)(((RectangleD)(ref rect)).Width / StiDpiHelper.DeviceCapsScale), stringFormat).Height * StiDpiHelper.DeviceCapsScale > ((RectangleD)(ref rect)).Height)
				{
					num = 0;
					num2 = 0;
					text2 = null;
				}
				text = ((num < text.Length) ? text.Substring(num) : string.Empty);
			}
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
		return text2;
	}

	internal static byte[] BreakImage(StiImage imageComp, ref byte[] imageBytes, double devideFactor)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		Image val;
		if (StiImageHelper.IsMetafile(imageBytes))
		{
			RectangleD paintRectangle = imageComp.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			val = (Image)(object)StiMetafileConverter.MetafileToBitmap(imageBytes, (int)((RectangleD)(ref paintRectangle)).Width * 2, (int)((RectangleD)(ref paintRectangle)).Height * 2);
		}
		else
		{
			val = StiImageConverter.BytesToImage(imageBytes, 200, 200, true, false);
		}
		int num = (int)Math.Round((double)val.Height * devideFactor);
		if (num == 0)
		{
			num = 1;
		}
		int num2 = val.Height - num;
		if (num2 == 0)
		{
			num2++;
			if (num > 1)
			{
				num--;
			}
		}
		Bitmap val2 = new Bitmap(val.Width, num);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height);
			Rectangle rectangle2 = new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height);
			val3.DrawImage(val, rectangle, rectangle2, (GraphicsUnit)2);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		Bitmap val4 = new Bitmap(val.Width, num2);
		Graphics val5 = Graphics.FromImage((Image)(object)val4);
		try
		{
			Rectangle rectangle3 = new Rectangle(0, 0, ((Image)val4).Width, ((Image)val4).Height);
			Rectangle rectangle4 = new Rectangle(0, num, ((Image)val4).Width, ((Image)val4).Height);
			val5.DrawImage(val, rectangle3, rectangle4, (GraphicsUnit)2);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		imageBytes = StiImageConverter.ImageToBytes((Image)(object)val4, false);
		return StiImageConverter.ImageToBytes((Image)(object)val2, false);
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	private static bool AllowBreak(StiComponent comp)
	{
		if (comp is StiContainer && !(comp is StiHeaderBand) && !(comp is StiGroupHeaderBand) && !(comp is StiDataBand) && !(comp is StiFooterBand))
		{
			return false;
		}
		if (comp is IStiBreakable stiBreakable)
		{
			return stiBreakable.CanBreak;
		}
		return false;
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	internal static StiComponentsCollection BreakContainer(StiContainer renderedContainer)
	{
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		StiComponentsCollection components = renderedContainer.Components;
		double num = renderedContainer.Height;
		double divideLine = 0.0;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 0; i < components.Count; i++)
			{
				StiComponent stiComponent = components[i];
				if (!(stiComponent.Top < num) || !(stiComponent.Bottom > num))
				{
					continue;
				}
				if (AllowBreak(stiComponent))
				{
					double height = stiComponent.Height;
					stiComponent.Height = num - stiComponent.Top;
					StiComponent stiComponent2 = ((StiService)stiComponent).Clone() as StiComponent;
					StiComponent dividedComponent = ((StiService)stiComponent).Clone() as StiComponent;
					IStiBreakable stiBreakable = stiComponent2 as IStiBreakable;
					if (stiBreakable.Break(dividedComponent, stiComponent.Height / height, ref divideLine))
					{
						stiComponent.Height = height;
						continue;
					}
					stiComponent.Height = height;
				}
				num = Math.Min(num, stiComponent.Top);
				flag = true;
			}
		}
		int num2 = 0;
		while (num2 < components.Count)
		{
			StiComponent stiComponent3 = components[num2];
			if (stiComponent3.Bottom > num)
			{
				IStiBreakable stiBreakable2 = stiComponent3 as IStiBreakable;
				if (stiComponent3.Top < num && stiBreakable2 != null && stiBreakable2.CanBreak)
				{
					double num3 = num - stiComponent3.Top;
					StiComponent stiComponent4 = ((StiService)stiComponent3).Clone() as StiComponent;
					double height2 = stiComponent3.Height;
					stiComponent4.Height = stiComponent3.Height - num3;
					stiComponent4.Top = 0.0;
					if (stiComponent4 is StiImage)
					{
						stiComponent4.CanGrow = false;
						stiComponent4.CanShrink = false;
					}
					else
					{
						stiComponent4.CanGrow = true;
						stiComponent4.CanShrink = false;
					}
					stiComponentsCollection.Add(stiComponent4);
					stiComponent3.Height = num3;
					if (!stiBreakable2.Break(stiComponent4, num3 / height2, ref divideLine))
					{
						components.Remove(stiComponent3);
						stiComponent4.CanGrow = false;
						continue;
					}
				}
				else
				{
					if (stiComponent3.Height > 0.0)
					{
						stiComponent3.Top -= num;
						components.Remove(stiComponent3);
						stiComponentsCollection.Add(stiComponent3);
						continue;
					}
					stiComponent3.Height = 0.0;
				}
			}
			num2++;
		}
		if (stiComponentsCollection.Count > 0)
		{
			StiBorder border = renderedContainer.Border;
			border.Side = (StiBorderSides)(border.Side & 7);
		}
		return stiComponentsCollection;
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	internal static StiContainer BreakContainer(double maxAllowedHeight, StiContainer renderedContainer)
	{
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Engine.AllowBreakContainerOptimization)
		{
			return BreakContainerV2(maxAllowedHeight, renderedContainer);
		}
		StiContainer stiContainer = renderedContainer.Clone(cloneProperties: false, cloneComponents: false) as StiContainer;
		StiComponentsCollection components = stiContainer.Components;
		StiComponentsCollection components2 = renderedContainer.Components;
		double num = maxAllowedHeight;
		double divideLine = 0.0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		while (flag3)
		{
			flag3 = false;
			foreach (StiComponent item in components2)
			{
				if (!((decimal)item.Top < (decimal)num) || !((decimal)(item.Top + item.Height) > (decimal)num))
				{
					continue;
				}
				if (item is IStiBreakable { CanBreak: not false })
				{
					double height = item.Height;
					double num3 = (item.Height = num - item.Top);
					StiComponent stiComponent2 = ((StiService)item).Clone() as StiComponent;
					StiComponent dividedComponent = ((item is StiContainer) ? ((item as StiContainer).Clone(cloneProperties: true, cloneComponents: false) as StiComponent) : (((StiService)item).Clone() as StiComponent));
					IStiBreakable stiBreakable2 = stiComponent2 as IStiBreakable;
					if (stiBreakable2.Break(dividedComponent, num3 / height, ref divideLine))
					{
						if (divideLine > num)
						{
							num = divideLine;
						}
						item.Height = height;
						if (!flag2 && stiComponent2.Bottom < num)
						{
							if (!flag)
							{
								num = stiComponent2.Bottom;
								flag = true;
							}
							else
							{
								num = maxAllowedHeight;
								flag2 = true;
							}
							flag3 = true;
							break;
						}
						continue;
					}
					item.Height = height;
				}
				if (item is StiNewPageContainer)
				{
					num = item.Bottom;
					flag = true;
					flag3 = true;
					break;
				}
				num = Math.Min(num, item.Top);
				flag3 = true;
			}
		}
		int num4 = 0;
		List<StiComponent> list = new List<StiComponent>();
		while (num4 < components2.Count)
		{
			StiComponent stiComponent3 = components2[num4];
			if ((decimal)(stiComponent3.Top + stiComponent3.Height) > (decimal)num)
			{
				IStiBreakable stiBreakable3 = stiComponent3 as IStiBreakable;
				if (!((decimal)stiComponent3.Top < (decimal)num) || stiBreakable3 == null || !stiBreakable3.CanBreak)
				{
					stiComponent3.Top -= num;
					num4++;
					components.Add(stiComponent3);
					continue;
				}
				double num5 = num - stiComponent3.Top;
				StiComponent stiComponent4 = ((stiComponent3 is StiContainer) ? ((stiComponent3 as StiContainer).Clone(cloneProperties: true, cloneComponents: false) as StiComponent) : (((StiService)stiComponent3).Clone() as StiComponent));
				double height2 = stiComponent3.Height;
				stiComponent4.Height = stiComponent3.Height - num5;
				stiComponent4.Top = 0.0;
				if (stiComponent4 is StiImage)
				{
					stiComponent4.CanGrow = false;
					stiComponent4.CanShrink = false;
				}
				else
				{
					stiComponent4.CanGrow = true;
					stiComponent4.CanShrink = false;
				}
				components.Add(stiComponent4);
				stiComponent3.Height = num5;
				if (!stiBreakable3.Break(stiComponent4, num5 / height2, ref divideLine))
				{
					num4++;
					stiComponent4.CanGrow = false;
					continue;
				}
			}
			list.Add(stiComponent3);
			num4++;
		}
		if (list.Count != components2.Count)
		{
			for (int i = 0; i < list.Count; i++)
			{
				components2[i] = list[i];
			}
			while (components2.Count > list.Count)
			{
				components2.RemoveAt(components2.Count - 1);
			}
		}
		if (StiOptions.Engine.RemoveBottomBorderOfSplitContainer && components.Count > 0)
		{
			object obj = renderedContainer.Border.Clone();
			renderedContainer.Border = (StiBorder)((obj is StiBorder) ? obj : null);
			StiBorder border = renderedContainer.Border;
			border.Side = (StiBorderSides)(border.Side & 7);
		}
		renderedContainer.Height = num;
		return stiContainer;
	}

	internal static StiContainer BreakContainerV2(double maxAllowedHeight, StiContainer renderedContainer)
	{
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = renderedContainer.Clone(cloneProperties: false, cloneComponents: false) as StiContainer;
		StiComponentsCollection components = stiContainer.Components;
		StiComponentsCollection components2 = renderedContainer.Components;
		double divideLine = maxAllowedHeight;
		divideLine = GetDivideLine(renderedContainer, divideLine);
		int num = 0;
		List<StiComponent> list = new List<StiComponent>();
		while (num < components2.Count)
		{
			StiComponent stiComponent = components2[num];
			if ((decimal)(stiComponent.Top + stiComponent.Height) > (decimal)divideLine)
			{
				IStiBreakable stiBreakable = stiComponent as IStiBreakable;
				if (!((decimal)stiComponent.Top < (decimal)divideLine) || stiBreakable == null || !stiBreakable.CanBreak)
				{
					stiComponent.Top -= divideLine;
					num++;
					components.Add(stiComponent);
					continue;
				}
				double num2 = divideLine - stiComponent.Top;
				StiComponent stiComponent2 = ((stiComponent is StiContainer) ? ((stiComponent as StiContainer).Clone(cloneProperties: true, cloneComponents: false) as StiComponent) : (((StiService)stiComponent).Clone() as StiComponent));
				double height = stiComponent.Height;
				stiComponent2.Height = stiComponent.Height - num2;
				stiComponent2.Top = 0.0;
				if (stiComponent2 is StiImage)
				{
					stiComponent2.CanGrow = false;
					stiComponent2.CanShrink = false;
				}
				else
				{
					stiComponent2.CanGrow = true;
					stiComponent2.CanShrink = false;
				}
				components.Add(stiComponent2);
				stiComponent.Height = num2;
				double divideLine2 = divideLine;
				if (!stiBreakable.Break(stiComponent2, num2 / height, ref divideLine2))
				{
					num++;
					stiComponent2.CanGrow = false;
					continue;
				}
			}
			list.Add(stiComponent);
			num++;
		}
		if (list.Count != components2.Count)
		{
			for (int i = 0; i < list.Count; i++)
			{
				components2[i] = list[i];
			}
			while (components2.Count > list.Count)
			{
				components2.RemoveAt(components2.Count - 1);
			}
		}
		if (StiOptions.Engine.RemoveBottomBorderOfSplitContainer && components.Count > 0)
		{
			object obj = renderedContainer.Border.Clone();
			renderedContainer.Border = (StiBorder)((obj is StiBorder) ? obj : null);
			StiBorder border = renderedContainer.Border;
			border.Side = (StiBorderSides)(border.Side & 7);
		}
		renderedContainer.Height = divideLine;
		return stiContainer;
	}

	private static double GetDivideLine(StiContainer container, double divideLine)
	{
		int breakCounter = 2;
		double num = SearchDivideLine(container, divideLine, divideLine, ref breakCounter);
		if (num == divideLine)
		{
			return divideLine;
		}
		while (num > 0.0)
		{
			double num2 = SearchDivideLine(container, num, divideLine, ref breakCounter);
			if (num2 == num)
			{
				return num;
			}
			num = num2;
		}
		return 0.0;
	}

	private static double SearchDivideLine(StiContainer container, double divideLine, double maxAllowedHeight, ref int breakCounter)
	{
		double divideLine2 = 0.0;
		foreach (StiComponent component in container.Components)
		{
			if (!((decimal)component.Top < (decimal)divideLine) || !((decimal)(component.Top + component.Height) > (decimal)divideLine))
			{
				continue;
			}
			if (component is IStiBreakable { CanBreak: not false })
			{
				if (breakCounter == 0)
				{
					continue;
				}
				double height = component.Height;
				double num = divideLine - component.Top;
				if (component is StiContainer)
				{
					divideLine2 = GetDivideLine(component as StiContainer, num);
					if (divideLine2 < num)
					{
						breakCounter--;
						if (breakCounter == 0)
						{
							return maxAllowedHeight;
						}
					}
					if (divideLine2 != num)
					{
						return component.Top + divideLine2;
					}
					continue;
				}
				StiComponent stiComponent2 = ((StiService)component).Clone() as StiComponent;
				StiComponent stiComponent3 = ((StiService)component).Clone() as StiComponent;
				stiComponent2.Height = num;
				stiComponent3.Height = num;
				IStiBreakable stiBreakable2 = stiComponent2 as IStiBreakable;
				if (stiBreakable2.Break(stiComponent3, num / height, ref divideLine2))
				{
					if (stiComponent2.Bottom < divideLine)
					{
						return stiComponent2.Bottom;
					}
					continue;
				}
			}
			if (component is StiNewPageContainer)
			{
				return component.Bottom;
			}
			divideLine = component.Top;
		}
		return divideLine;
	}

	internal static void ProcessPreviousBreakedContainer(StiContainer renderedContainer, StiComponentsCollection breakedComponents, ref double freeSpace)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		StiBorder border = renderedContainer.Border;
		border.Side = (StiBorderSides)(border.Side & 0xE);
		StiComponentsCollection components = renderedContainer.Components;
		components.Clear();
		foreach (StiComponent breakedComponent in breakedComponents)
		{
			components.Add(breakedComponent);
		}
		double height = renderedContainer.Height;
		renderedContainer.CanGrow = true;
		renderedContainer.CanShrink = true;
		StiContainerHelper.CheckSize(renderedContainer);
		freeSpace += height - renderedContainer.Height;
	}
}
