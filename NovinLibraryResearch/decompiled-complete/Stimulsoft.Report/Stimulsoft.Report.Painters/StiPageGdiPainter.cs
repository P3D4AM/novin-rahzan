using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Security.Cryptography;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Painters;

public class StiPageGdiPainter : StiContainerGdiPainter, IStiPagePainter
{
	public virtual void PaintText(StiWatermark watermark, Graphics g, RectangleD rect, double zoom, bool isPrinting)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		if (!watermark.Enabled || string.IsNullOrEmpty(watermark.Text))
		{
			return;
		}
		float num = watermark.Font.Size * (float)zoom;
		if (isPrinting && (int)watermark.Font.Unit != 2 && (int)watermark.Font.Unit != 0)
		{
			num *= 0.96f;
		}
		Brush brush = StiBrush.GetBrush(watermark.TextBrush, ((RectangleD)(ref rect)).ToRectangleF());
		try
		{
			Font val = new Font(watermark.Font.FontFamily, num, watermark.Font.Style, watermark.Font.Unit, watermark.Font.GdiCharSet, watermark.Font.GdiVerticalFont);
			try
			{
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				if (!StiOptions.Engine.DisableAntialiasingInPainters)
				{
					g.TextRenderingHint = (TextRenderingHint)4;
				}
				StiTextOptions val2 = new StiTextOptions
				{
					Angle = watermark.Angle,
					RightToLeft = watermark.RightToLeft
				};
				StiTextDrawing.DrawString(g, watermark.Text, val, brush, rect, val2, (StiTextHorAlignment)1, (StiVertAlignment)1, true, watermark.Angle, 1.0);
				g.TextRenderingHint = textRenderingHint;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
	}

	public virtual void PaintImage(StiReport report, StiWatermark watermark, Graphics g, RectangleD rect, double zoom)
	{
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		if (!watermark.Enabled)
		{
			return;
		}
		byte[] image = watermark.GetImage(report);
		if (image == null)
		{
			return;
		}
		GraphicsState val = g.Save();
		g.SetClip(((RectangleD)(ref rect)).ToRectangleF());
		if (watermark.ImageStretch)
		{
			RectangleF rectangleF = ((RectangleD)(ref rect)).ToRectangleF();
			Image val2 = StiImageConverter.BytesToImage(image, (int)rectangleF.Width, (int)rectangleF.Height, true, false);
			try
			{
				if (watermark.AspectRatio)
				{
					float num = rectangleF.Width / (float)val2.Width;
					float num2 = rectangleF.Height / (float)val2.Height;
					if (num > num2)
					{
						rectangleF.X = (rectangleF.Width - (float)val2.Width * num2) / 2f;
						rectangleF.Width = (float)val2.Width * num2;
					}
					else
					{
						rectangleF.Y = (rectangleF.Height - (float)val2.Height * num) / 2f;
						rectangleF.Height = (float)val2.Height * num;
					}
					float num3 = Math.Min(num, num2);
					SizeD val3 = default(SizeD);
					((SizeD)(ref val3))._002Ector((double)((float)val2.Size.Width * num3), (double)((float)val2.Height * num3));
					RectangleD val4 = StiRectangleUtils.AlignSizeInRect(rect, val3, watermark.ImageAlignment);
					rectangleF = ((RectangleD)(ref val4)).ToRectangleF();
				}
				g.DrawImage(val2, rectangleF);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			Image val5 = StiImageConverter.BytesToImage(image, 200, 200, true, false);
			try
			{
				zoom *= watermark.ImageMultipleFactor;
				SizeD val6 = default(SizeD);
				((SizeD)(ref val6))._002Ector((double)val5.Size.Width * zoom, (double)val5.Height * zoom);
				if (watermark.ImageTiling)
				{
					PaintTileImage(val5, g, rect, val6);
				}
				else
				{
					RectangleD val7 = StiRectangleUtils.AlignSizeInRect(rect, val6, watermark.ImageAlignment);
					g.DrawImage(val5, ((RectangleD)(ref val7)).ToRectangleF());
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		g.Restore(val);
	}

	public virtual void PaintTileImage(Image watermarkImage, Graphics g, RectangleD rect, SizeD imageSize)
	{
		for (float num = (float)((RectangleD)(ref rect)).Y; (double)num < ((RectangleD)(ref rect)).Bottom; num += (float)((SizeD)(ref imageSize)).Height)
		{
			for (float num2 = (float)((RectangleD)(ref rect)).X; (double)num2 < ((RectangleD)(ref rect)).Right; num2 += (float)((SizeD)(ref imageSize)).Width)
			{
				g.DrawImage(watermarkImage, new RectangleF(num2, num, (float)((SizeD)(ref imageSize)).Width, (float)((SizeD)(ref imageSize)).Height));
			}
		}
	}

	public Bitmap GetWatermarkImage(StiPage page, double zoom, bool useMargins)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Round(page.Unit.ConvertToHInches(page.Width + (useMargins ? (page.Margins.Left + page.Margins.Right) : 0.0)) * zoom);
		int num2 = (int)Math.Round(page.Unit.ConvertToHInches(page.Height + (useMargins ? (page.Margins.Top + page.Margins.Bottom) : 0.0)) * zoom);
		Bitmap val = new Bitmap(num, num2);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Brush brush = StiBrush.GetBrush(page.Brush, new RectangleD(0.0, 0.0, 0.0, 0.0));
			try
			{
				Brush val3 = brush;
				if (StiBrush.ToColor(page.Brush) == Color.Transparent)
				{
					val3 = Brushes.White;
				}
				val2.FillRectangle(val3, 0f, 0f, (float)num, (float)num2);
				PaintImage(page.Report, page.Watermark, val2, new RectangleD(0.0, 0.0, (double)num, (double)num2), zoom);
				PaintText(page.Watermark, val2, new RectangleD(0.0, 0.0, (double)num, (double)num2), zoom, isPrinting: false);
				return val;
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void PaintWatermark(StiPage page, Graphics g, bool isBehind)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		double num = page.Zoom * StiScale.Factor;
		StiUnit unit = page.Unit;
		RectangleD displayRectangle = page.DisplayRectangle;
		double num2 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
		StiUnit unit2 = page.Unit;
		displayRectangle = page.DisplayRectangle;
		double num3 = unit2.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height);
		RectangleD val = default(RectangleD);
		((RectangleD)(ref val))._002Ector(0.0, 0.0, num2 * num, num3 * num);
		object tagValue = page.TagValue;
		StiAdvancedWatermark val2 = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
		if (isBehind && val2 != null && val2.WeaveEnabled)
		{
			StiAdvancedWatermarkGdiPainter.PaintWatermark(g, val2, (StiCornerRadius)null, val, num);
			return;
		}
		if (page.Watermark.ShowImageBehind == isBehind)
		{
			PaintImage(page.Report, page.Watermark, g, val, num);
		}
		if (page.Watermark.ShowBehind == isBehind)
		{
			PaintText(page.Watermark, g, val, num, page.IsPrinting);
		}
	}

	public override void Paint(StiComponent comp, StiPaintEventArgs e)
	{
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Invalid comparison between Unknown and I4
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Invalid comparison between Unknown and I4
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Invalid comparison between Unknown and I4
		bool isPaintWinFormsViewer = e.IsPaintWinFormsViewer;
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiPage stiPage = (StiPage)comp;
		if (stiPage.Report == null)
		{
			return;
		}
		StiScale.IsPrinting = stiPage.IsPrinting;
		if (stiPage.IsDesigning)
		{
			stiPage.SelectedComponents.Clear();
		}
		Graphics graphics = e.Graphics;
		if (StiOptions.Engine.DisableAntialiasingInPainters)
		{
			graphics.SmoothingMode = (SmoothingMode)3;
			graphics.TextRenderingHint = (TextRenderingHint)2;
			graphics.InterpolationMode = (InterpolationMode)5;
		}
		stiPage.InvokePainting(stiPage, e);
		StiUnit unit = stiPage.Unit;
		double num = stiPage.Zoom * StiScale.Factor;
		double num2 = unit.ConvertToHInches(stiPage.Margins.Left);
		double num3 = unit.ConvertToHInches(stiPage.Margins.Top);
		double num4 = unit.ConvertToHInches(stiPage.Margins.Right);
		RectangleD val = stiPage.DisplayRectangle;
		double num5 = unit.ConvertToHInches(((RectangleD)(ref val)).Width);
		val = stiPage.DisplayRectangle;
		double num6 = unit.ConvertToHInches(((RectangleD)(ref val)).Height);
		double num7 = unit.ConvertToHInches(stiPage.Width);
		double num8 = unit.ConvertToHInches(stiPage.Height);
		RectangleF printerMargins = RectangleF.Empty;
		if (stiPage.IsPrinting)
		{
			printerMargins = stiPage.GetPrinterMargins(graphics);
			double num9 = unit.ConvertToHInches(stiPage.PageWidth);
			double num10 = unit.ConvertToHInches(stiPage.PageHeight);
			if ((double)printerMargins.Width > num9 * 1.005)
			{
				if ((int)StiOptions.Print.PageHorAlignment == 2)
				{
					printerMargins.X -= (float)((double)printerMargins.Width - num9) / 2f;
				}
				if ((int)StiOptions.Print.PageHorAlignment == 3)
				{
					printerMargins.X -= (float)((double)printerMargins.Width - num9);
				}
			}
			if ((double)printerMargins.Height > num10 * 1.005)
			{
				if ((int)StiOptions.Print.PageVertAlignment == 1)
				{
					printerMargins.Y -= (float)((double)printerMargins.Height - num10) / 2f;
				}
				if ((int)StiOptions.Print.PageVertAlignment == 2)
				{
					printerMargins.Y -= (float)((double)printerMargins.Height - num10);
				}
			}
		}
		if (!e.Cancel)
		{
			val = e.ClipRectangle;
			if (!((RectangleD)(ref val)).IsEmpty)
			{
				graphics.ResetClip();
				val = e.ClipRectangle;
				graphics.SetClip(((RectangleD)(ref val)).ToRectangleF(), (CombineMode)0);
			}
			if (!stiPage.IsPrinting)
			{
				PaintBackground(e, graphics, stiPage, num5, num6);
				PaintWatermark(stiPage, graphics, isBehind: true);
				graphics.TranslateTransform((float)(int)(num2 * num), (float)(int)(num3 * num));
				PaintGrid(graphics, stiPage, num7, num8);
				if (stiPage.IsDesigning)
				{
					PaintContentBorder(graphics, stiPage, num7, num8);
					PaintColumns(graphics, stiPage, num8);
				}
			}
			else
			{
				graphics.ResetTransform();
				graphics.TranslateTransform(0f - printerMargins.Left, 0f - printerMargins.Top);
				PaintWatermark(stiPage, graphics, isBehind: true);
				graphics.PageUnit = (GraphicsUnit)4;
				graphics.ResetTransform();
				if (stiPage.SegmentPerHeight > 1 || stiPage.SegmentPerWidth > 1)
				{
					PaintSegment(graphics, stiPage, printerMargins);
				}
				else if (stiPage.StretchToPrintArea)
				{
					graphics.PageScale = 0.01f * (float)((double)printerMargins.Width / (num5 - num2 - num4));
				}
				else
				{
					graphics.PageScale = 0.01f;
					graphics.TranslateTransform(0f - printerMargins.Left + (float)num2, 0f - printerMargins.Top + (float)num3);
				}
			}
		}
		StiPagePaintEventArgs eventArgs = PaintCopyrightBefore(stiPage, graphics, e);
		e.Cancel = false;
		stiPage.InvokePainted(stiPage, e);
		val = e.ClipRectangle;
		if (!((RectangleD)(ref val)).IsEmpty)
		{
			val = e.ClipRectangle;
			double num11 = ((RectangleD)(ref val)).X - num2 * num;
			val = e.ClipRectangle;
			double num12 = ((RectangleD)(ref val)).Y - num3 * num;
			val = e.ClipRectangle;
			double width = ((RectangleD)(ref val)).Width;
			val = e.ClipRectangle;
			e = new StiPaintEventArgs(graphics, new RectangleD(num11, num12, width, ((RectangleD)(ref val)).Height));
		}
		GraphicsState val2 = graphics.Save();
		RectangleF rectangleF = new RectangleF((float)((0.0 - num2) * num), (float)((0.0 - num3) * num), (float)(num5 * num), (float)(num6 * num));
		graphics.SetClip(rectangleF, (CombineMode)1);
		PaintTableLines(stiPage, graphics);
		PaintComponents(stiPage, e);
		PaintSelectedComponents(stiPage, graphics, e);
		graphics.Restore(val2);
		PaintBorder(stiPage, graphics, new RectangleD(0.0, 0.0, num7 * num, num8 * num), num, drawBorderFormatting: true, drawBorderSides: true);
		PaintTopWatermark(stiPage, graphics, printerMargins);
		PaintPageBreaks(stiPage, graphics, num7, num8);
		PaintLargeHeightFactorLine(stiPage, graphics);
		PaintHighlight(stiPage, e);
		PaintOrderAndQuickInfo(stiPage, graphics, string.Empty);
		PaintInheritedImage(stiPage, graphics);
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag = (stiPage.IsDashboard ? StiLicenseKeyValidator.IsValidInDashboardsDesignerOrOnPlatform((StiProductIdent)14, licenseKey) : StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey));
		bool flag2 = !((isPaintWinFormsViewer || StiDesignerAppStatus.IsRunning) ? flag : StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey));
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag2 = true;
		}
		if (!flag2)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag2 = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			StringFormat val3 = new StringFormat();
			try
			{
				val3.LineAlignment = (StringAlignment)1;
				val3.Alignment = (StringAlignment)1;
				Color color = Color.FromArgb(30, 100, 100, 100);
				if (stiPage.IsPrinting)
				{
					color = Color.LightGray;
				}
				SolidBrush val4 = new SolidBrush(color);
				try
				{
					RectangleD val5 = default(RectangleD);
					((RectangleD)(ref val5))._002Ector((0.0 - num2) * num, (0.0 - num3) * num, num5 * num, num6 * num);
					Font val6 = new Font("Arial", (float)(150.0 * num / StiScale.Factor), (FontStyle)1);
					try
					{
						StiTextDrawing.DrawString(graphics, "Trial", val6, (Brush)(object)val4, val5, val3, 45f, 1.0);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		PaintSelection(stiPage, graphics);
		PaintDimensionLines(stiPage, graphics);
		PaintCopyrightAfter(stiPage, graphics, eventArgs);
		StiScale.IsPrinting = false;
	}

	public virtual void PaintSelectedComponents(StiPage page, Graphics g, StiPaintEventArgs e)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (!page.IsDesigning)
		{
			return;
		}
		for (int i = 0; i < page.SelectedComponents.Count; i++)
		{
			StiComponent stiComponent = page.SelectedComponents[i] as StiComponent;
			if (stiComponent is StiBand)
			{
				stiComponent.Paint(e);
			}
		}
		for (int i = 0; i < page.SelectedComponents.Count; i++)
		{
			StiComponent stiComponent2 = page.SelectedComponents[i] as StiComponent;
			if (!(stiComponent2 is StiBand))
			{
				stiComponent2.Paint(e);
			}
		}
		StiComponentsCollection selectedComponents = page.GetSelectedComponents();
		if (selectedComponents.Count <= 1)
		{
			return;
		}
		for (int i = 0; i < selectedComponents.Count; i++)
		{
			StiComponent stiComponent3 = selectedComponents[i];
			if (stiComponent3.IsSelected)
			{
				RectangleD paintRectangle = stiComponent3.GetPaintRectangle();
				double num = ((RectangleD)(ref paintRectangle)).X + ((RectangleD)(ref paintRectangle)).Width / 2.0;
				double num2 = ((RectangleD)(ref paintRectangle)).Y + ((RectangleD)(ref paintRectangle)).Height / 2.0;
				Color selectionCornerColor = GetSelectionCornerColor(page);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).X - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Y - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).X - (double)StiScale.I1, num2 - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, num - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Y - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).Right - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Y - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).Right - (double)StiScale.I1, num2 - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).Right - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Bottom - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, num - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Bottom - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
				StiDrawing.FillRectangle(g, selectionCornerColor, ((RectangleD)(ref paintRectangle)).X - (double)StiScale.I1, ((RectangleD)(ref paintRectangle)).Bottom - (double)StiScale.I1, (double)StiScale.I3, (double)StiScale.I3);
			}
		}
	}

	public virtual StiPagePaintEventArgs PaintCopyrightBefore(StiPage page, Graphics g, StiPaintEventArgs eventArgs)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		double num = page.Zoom * StiScale.Factor;
		double num2 = page.Unit.ConvertToHInches(page.Margins.Left);
		double num3 = page.Unit.ConvertToHInches(page.Margins.Top);
		StiUnit unit = page.Unit;
		RectangleD val = page.DisplayRectangle;
		double num4 = unit.ConvertToHInches(((RectangleD)(ref val)).Width);
		StiUnit unit2 = page.Unit;
		val = page.DisplayRectangle;
		double num5 = unit2.ConvertToHInches(((RectangleD)(ref val)).Height);
		double num6 = page.Unit.ConvertToHInches(page.Width);
		double num7 = page.Unit.ConvertToHInches(page.Height);
		Graphics graphics = eventArgs.Graphics;
		val = eventArgs.ClipRectangle;
		StiPagePaintEventArgs stiPagePaintEventArgs = new StiPagePaintEventArgs(graphics, ((RectangleD)(ref val)).ToRectangle(), new Rectangle((int)(num2 * num), (int)(num3 * num), (int)(num6 * num), (int)(num7 * num)), new Rectangle(0, 0, (int)(num4 * num), (int)(num5 * num)), page.Report.IsPrinting, page.Report.IsDesigning);
		g.TranslateTransform((float)(-(int)(num2 * num)), (float)(-(int)(num3 * num)));
		StiPage.InvokePagePainting(page, stiPagePaintEventArgs);
		g.TranslateTransform((float)(int)(num2 * num), (float)(int)(num3 * num));
		return stiPagePaintEventArgs;
	}

	public virtual void PaintCopyrightAfter(StiPage page, Graphics g, StiPagePaintEventArgs eventArgs)
	{
		double num = page.Zoom * StiScale.Factor;
		double num2 = page.Unit.ConvertToHInches(page.Margins.Left);
		double num3 = page.Unit.ConvertToHInches(page.Margins.Top);
		g.TranslateTransform((float)(-(int)(num2 * num)), (float)(-(int)(num3 * num)));
		StiPage.InvokePagePainted(page, eventArgs);
		g.TranslateTransform((float)(int)(num2 * num), (float)(int)(num3 * num));
	}

	private static void PaintDimensionLines(StiPage page, Graphics g)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (page.Report.Info.ShowDimensionLines && page.IsDesigning && (int)page.Report.Info.CurrentAction != 0 && !page.IsDashboard)
		{
			StiDimensionLinesHelper.DrawDimensionLines(g, page);
		}
	}

	private void PaintTopWatermark(StiPage page, Graphics g, RectangleF printerMargins)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		double num = page.Zoom * StiScale.Factor;
		double num2 = page.Unit.ConvertToHInches(page.Margins.Left);
		double num3 = page.Unit.ConvertToHInches(page.Margins.Right);
		double num4 = page.Unit.ConvertToHInches(page.Margins.Top);
		StiUnit unit = page.Unit;
		RectangleD displayRectangle = page.DisplayRectangle;
		double num5 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
		if (page.IsPrinting)
		{
			if (page.StretchToPrintArea)
			{
				g.PageScale = 0.01f;
			}
			g.ResetTransform();
			g.TranslateTransform(0f - printerMargins.Left, 0f - printerMargins.Top);
			PaintWatermark(page, g, isBehind: false);
			g.ResetTransform();
			if (page.StretchToPrintArea)
			{
				g.PageScale = 0.01f * (float)((double)printerMargins.Width / (num5 - num2 - num3));
			}
		}
		else
		{
			g.TranslateTransform((float)(-(int)(num2 * num)), (float)(-(int)(num4 * num)));
			PaintWatermark(page, g, isBehind: false);
			g.TranslateTransform((float)(int)(num2 * num), (float)(int)(num4 * num));
		}
	}

	public override void PaintHighlight(StiComponent component, StiPaintEventArgs e)
	{
		StiPage stiPage = component as StiPage;
		if (!stiPage.LockHighlight && !stiPage.IsPrinting)
		{
			base.PaintHighlight((StiComponent)stiPage, e);
		}
	}

	public override void PaintOrderAndQuickInfo(StiComponent component, Graphics g, string number)
	{
		StiPage page = component.Page;
		if (page != null && ((page.Report.Info.ShowOrder && page.IsDesigning) || (page.Report.Info.QuickInfoOverlay && page.Report.Info.QuickInfoType != 0)))
		{
			base.PaintOrderAndQuickInfo(component, g, string.Empty);
		}
	}

	public virtual void PaintLargeHeightFactorLine(StiPage page, Graphics g)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		if (page.IsPrinting || !(page.LargeHeightAutoFactor > 1.0) || !page.IsDesigning)
		{
			return;
		}
		double num = page.Zoom * StiScale.Factor;
		double num2 = page.Unit.ConvertToHInches(page.Width);
		double num3 = page.Unit.ConvertToHInches(page.Margins.Left);
		double num4 = page.Unit.ConvertToHInches(page.Margins.Bottom);
		StiUnit unit = page.Unit;
		RectangleD displayRectangle = page.DisplayRectangle;
		double num5 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
		float num6 = (float)(page.Unit.ConvertToHInches(page.PageHeight - page.Margins.Top - page.Margins.Bottom) * (double)page.SegmentPerHeight * num);
		Pen val = new Pen(Color.Red, (float)(2.0 * num));
		try
		{
			val.DashStyle = (DashStyle)1;
			g.DrawLine(val, 0f, num6, (float)(num2 * num), num6);
			g.DrawLine(val, 0f - (float)(num3 * num), (float)((double)num6 + num4 * num), (float)(num5 * num), (float)((double)num6 + num4 * num));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintPageBreaks(StiPage page, Graphics g, double width, double height)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		if (page.DenyDrawSegmentMode || !StiOptions.Viewer.AllowDrawSegmentMode || ((page.IsPrinting || (page.SegmentPerWidth <= 1 && page.SegmentPerHeight <= 1)) && (page.Report.Info.ViewMode != StiViewMode.PageBreakPreview || !page.IsDesigning)))
		{
			return;
		}
		double num = page.Zoom * StiScale.Factor;
		if (page.SegmentPerWidth > 1 || page.SegmentPerHeight > 1)
		{
			double num2 = page.Unit.ConvertToHInches(page.Width / (double)page.SegmentPerWidth);
			double num3 = page.Unit.ConvertToHInches(page.PageHeight - page.Margins.Top - page.Margins.Bottom);
			Pen val = new Pen(Color.Blue);
			try
			{
				val.Width = 2f;
				val.DashStyle = (DashStyle)1;
				double num4 = 0.0;
				int num5 = 1;
				if (!page.IsDesigning && page.Report.RenderedPages.Count > 0)
				{
					num5 = page.Report.GetPageNumber(page.Report.RenderedPages.IndexOf(page));
				}
				RectangleD pageRect = default(RectangleD);
				for (int i = 0; i < page.SegmentPerHeight; i++)
				{
					double num6 = 0.0;
					num4 += num3;
					for (int j = 0; j < page.SegmentPerWidth; j++)
					{
						num6 += num2;
						if (j < page.SegmentPerWidth - 1)
						{
							g.DrawLine(val, (float)(num6 * num), 0f, (float)(num6 * num), (float)(page.Unit.ConvertToHInches(page.Height) * num));
						}
						((RectangleD)(ref pageRect))._002Ector((num6 - num2) * num, (num4 - num3) * num, num2 * num, num3 * num);
						if (StiOptions.Viewer.IsRightToLeft)
						{
							((RectangleD)(ref pageRect)).X = ((double)page.SegmentPerWidth * num2 - num6) * num;
						}
						DrawPageNumber(page, g, num5, pageRect);
						num5++;
					}
					if (i < page.SegmentPerHeight - 1)
					{
						g.DrawLine(val, 0f, (float)(num4 * num), (float)(page.Unit.ConvertToHInches(page.Width) * num), (float)(num4 * num));
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			DrawPageNumber(page, g, 1, new RectangleD(0.0, 0.0, width * num, height * num));
		}
		Pen val2 = new Pen(Color.Blue);
		try
		{
			val2.Width = 2f;
			g.DrawRectangle(val2, 0f, 0f, (float)(width * num), (float)(height * num));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public virtual void DrawPageNumber(StiPage page, Graphics g, int pageNumber, RectangleD pageRect)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		if (!(page.Zoom > 0.1))
		{
			return;
		}
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = new Font("Arial", 14f);
			try
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(100, Color.Blue));
				try
				{
					val.Alignment = (StringAlignment)1;
					val.LineAlignment = (StringAlignment)1;
					val.FormatFlags = (StringFormatFlags)4096;
					val.Trimming = (StringTrimming)5;
					g.DrawString(Loc.Get("Components", "StiPage") + pageNumber, val2, (Brush)(object)val3, ((RectangleD)(ref pageRect)).ToRectangleF(), val);
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
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintTableLines(StiPage page, Graphics g)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		if ((page.Report.Info.IsComponentsMoving && (int)page.Report.Info.CurrentAction == 1) || !StiTableHelper.IsTableMode(page.Report.Designer))
		{
			return;
		}
		Hashtable xx = new Hashtable();
		Hashtable yy = new Hashtable();
		StiTableHelper.GetSelectedLines(page.Report.Designer, ref xx, ref yy, isPaint: true);
		RectangleD selectedRectangle = StiTableHelper.GetSelectedRectangle(page.Report.Designer, isPaintRect: true);
		Color tableLinesColor = GetTableLinesColor(page);
		Pen val = new Pen(tableLinesColor);
		try
		{
			val.DashStyle = (DashStyle)1;
			foreach (double key in xx.Keys)
			{
				if (!double.IsNaN(key))
				{
					g.DrawLine(val, (float)key, (float)((RectangleD)(ref selectedRectangle)).Y, (float)key, (float)((RectangleD)(ref selectedRectangle)).Bottom);
				}
			}
			foreach (double key2 in yy.Keys)
			{
				if (!double.IsNaN(key2))
				{
					g.DrawLine(val, (float)((RectangleD)(ref selectedRectangle)).X, (float)key2, (float)((RectangleD)(ref selectedRectangle)).Right, (float)key2);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual Color GetTableLinesColor(StiPage page)
	{
		if (!(page is StiForm))
		{
			return Color.Blue;
		}
		return Color.DimGray;
	}

	public virtual void PaintSelection(StiPage page, Graphics g)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		RectangleD selectedRectangle = page.SelectedRectangle;
		if (((RectangleD)(ref selectedRectangle)).IsEmpty)
		{
			return;
		}
		double num = page.Zoom * StiScale.Factor;
		SolidBrush val = new SolidBrush(GetSelectionBackgroundColor(page));
		try
		{
			Pen val2 = new Pen(GetSelectionBorderColor(page));
			try
			{
				val2.DashStyle = (DashStyle)1;
				RectangleD rect = page.SelectedRectangle;
				((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X * num;
				((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y * num;
				((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width * num;
				((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height * num;
				rect = page.Page.Unit.ConvertToHInches(rect);
				RectangleF rectangleF = ((RectangleD)(ref rect)).ToRectangleF();
				g.FillRectangle((Brush)(object)val, rectangleF);
				g.DrawRectangle(val2, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintSegment(Graphics g, StiPage page, RectangleF printerMargins)
	{
		float num = (float)page.Unit.ConvertToHInches(page.Margins.Left);
		float num2 = (float)page.Unit.ConvertToHInches(page.Margins.Top);
		g.PageScale = 0.01f;
		g.TranslateTransform(0f - printerMargins.Left + num, 0f - printerMargins.Top + num2);
		PaintWatermark(page, g, isBehind: true);
		double num3 = page.PageWidth - page.Margins.Left - page.Margins.Right;
		double num4 = page.PageHeight - page.Margins.Top - page.Margins.Bottom;
		float num5 = (float)page.Unit.ConvertToHInches((double)page.CurrentWidthSegment * num3);
		float num6 = (float)page.Unit.ConvertToHInches((double)page.CurrentHeightSegment * num4);
		g.SetClip(new RectangleF(0f, 0f, (float)page.Unit.ConvertToHInches(num3), (float)page.Unit.ConvertToHInches(num4)), (CombineMode)0);
		g.TranslateTransform(0f - num5, 0f - num6);
	}

	public virtual void PaintContentBorder(Graphics g, StiPage page, double width, double height)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		double num = page.Zoom * StiScale.Factor;
		Pen val = new Pen(StiColorUtils.Dark(Color.Gainsboro, (byte)100));
		try
		{
			StiDrawing.DrawRectangle(g, val, 0.0, 0.0, width * num, height * num);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintColumns(Graphics g, StiPage page, double height)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		if (page.Columns <= 1)
		{
			return;
		}
		double num = page.Zoom * StiScale.Factor;
		double num2 = page.Unit.ConvertToHInches(page.GetColumnWidth()) * num;
		double num3 = page.Unit.ConvertToHInches(page.ColumnGaps) * num;
		double num4 = num2;
		Pen val = new Pen(Color.Red);
		try
		{
			val.DashStyle = (DashStyle)1;
			for (int i = 1; i < page.Columns; i++)
			{
				g.DrawLine(val, (float)num4, 0f, (float)num4, (float)(height * num));
				g.DrawLine(val, (float)(num4 + num3), 0f, (float)(num4 + num3), (float)(height * num));
				num4 += num2 + num3;
			}
			g.DrawLine(val, (float)num4, 0f, (float)num4, (float)(height * num));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintBackground(StiPaintEventArgs e, Graphics g, StiPage page, double width, double height)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		double num = page.Zoom * StiScale.Factor;
		RectangleD val = (RectangleD)(page.IsDesigning ? e.ClipRectangle : new RectangleD(0.0, 0.0, width * num, height * num));
		if (StiBrush.IsTransparent(page.Brush))
		{
			StiDrawing.FillRectangle(g, Brushes.White, val);
		}
		else
		{
			StiDrawing.FillRectangle(g, page.Brush, 0.0, 0.0, width * num, height * num);
		}
	}

	public virtual void PaintGrid(Graphics g, StiPage page, double width, double height)
	{
		if (page.IsDesigning && page.Report.Info.ShowGrid)
		{
			switch (page.Report.Info.GridMode)
			{
			case StiGridMode.Lines:
				PaintGridLines(g, page, width, height);
				break;
			case StiGridMode.Dots:
				PaintGridDots(g, page, width, height);
				break;
			}
		}
	}

	public virtual void PaintGridLines(Graphics g, StiPage page, double width, double height)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		double num = page.Unit.ConvertToHInches(page.GridSize);
		double num2 = page.Zoom * StiScale.Factor;
		Pen val = new Pen(StiColorUtils.Dark(Color.Gainsboro, (byte)30));
		try
		{
			double num3 = 0.0;
			double num4 = width * num2;
			double num5 = num * num2;
			int num6 = 0;
			for (num3 = 0.0; num3 < num4; num3 += num5)
			{
				if ((num6 & 1) == 0)
				{
					StiDrawing.DrawLine(g, val, num3, 0.0, num3, height * num2);
				}
				else
				{
					StiDrawing.DrawLine(g, Pens.Gainsboro, num3, 0.0, num3, height * num2);
				}
				num6++;
			}
			double num7 = 0.0;
			double num8 = height * num2;
			num5 = num * num2;
			num6 = 0;
			for (num7 = 0.0; num7 < num8; num7 += num5)
			{
				if ((num6 & 1) == 0)
				{
					StiDrawing.DrawLine(g, val, 0.0, num7, width * num2, num7);
				}
				else
				{
					StiDrawing.DrawLine(g, Pens.Gainsboro, 0.0, num7, width * num2, num7);
				}
				num6++;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintGridDots(Graphics g, StiPage page, double width, double height)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		double num = page.Unit.ConvertToHInches(page.GridSize);
		double num2 = page.Zoom * StiScale.Factor;
		Bitmap val = new Bitmap((int)(width * num2) + 1, 1);
		try
		{
			double num3 = width * num2;
			double num4 = num * num2;
			for (double num5 = 0.0; num5 < num3; num5 += num4)
			{
				int num6 = (int)Math.Round(num5);
				if (num6 < ((Image)val).Width)
				{
					val.SetPixel(num6, 0, StiColorUtils.Dark(Color.Gainsboro, (byte)30));
				}
			}
			double num7 = height * num2;
			for (double num8 = 0.0; num8 < num7; num8 += num4)
			{
				g.DrawImage((Image)(object)val, 0, (int)num8);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual Color GetSelectionBackgroundColor(StiPage page)
	{
		return Color.FromArgb(75, Color.Blue);
	}

	public virtual Color GetSelectionBorderColor(StiPage page)
	{
		return Color.DimGray;
	}

	public virtual Color GetSelectionCornerColor(StiPage page)
	{
		return Color.Gray;
	}
}
