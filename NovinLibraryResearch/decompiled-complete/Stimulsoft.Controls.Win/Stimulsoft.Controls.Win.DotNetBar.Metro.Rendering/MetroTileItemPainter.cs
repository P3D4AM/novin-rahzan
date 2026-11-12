using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroTileItemPainter : MetroRenderer
{
	private static readonly int InflatePixels = 1;

	private static readonly int InflatePixelsMouseDown = 2;

	private static readonly int DragInsertOffsetPixels = 10;

	private static readonly Size CheckMarkSize = new Size(27, 27);

	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		MetroTileItem metroTileItem = (MetroTileItem)renderingInfo.Control;
		if (metroTileItem.Frames.Count == 0)
		{
			HatchBrush val = new HatchBrush((HatchStyle)2, Color.Red);
			try
			{
				renderingInfo.PaintEventArgs.Graphics.FillRectangle((Brush)(object)val, metroTileItem.Bounds);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Matrix val2 = null;
		float num = 0.95f;
		Matrix val3 = null;
		if (renderingInfo.ItemPaintArgs.DragInProgress)
		{
			val2 = renderingInfo.PaintEventArgs.Graphics.Transform;
			val3 = new Matrix(num, 0f, 0f, num, 0f, 0f);
			float num2 = ((float)metroTileItem.WidthInternal * (1f / num) - (float)metroTileItem.WidthInternal) / 2f;
			float num3 = ((float)metroTileItem.HeightInternal * (1f / num) - (float)metroTileItem.HeightInternal) / 2f;
			val3.Translate(num2, num3);
			renderingInfo.PaintEventArgs.Graphics.Transform = val3;
		}
		if (metroTileItem.CurrentFrameOffset != 0 && metroTileItem.LastFrame != metroTileItem.CurrentFrame)
		{
			Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
			if (val2 == null)
			{
				val2 = graphics.Transform;
			}
			graphics.TranslateTransform(0f, (float)(-(metroTileItem.HeightInternal - metroTileItem.CurrentFrameOffset - InflatePixels * 2)), (MatrixOrder)1);
			RenderFrame(renderingInfo, metroTileItem.LastFrame);
			if (val3 != null)
			{
				graphics.Transform = val3;
			}
			else
			{
				graphics.Transform = val2;
			}
			graphics.TranslateTransform(0f, (float)metroTileItem.CurrentFrameOffset, (MatrixOrder)1);
			RenderFrame(renderingInfo, metroTileItem.CurrentFrame);
		}
		else
		{
			RenderFrame(renderingInfo, metroTileItem.CurrentFrame);
		}
		if (val2 != null)
		{
			renderingInfo.PaintEventArgs.Graphics.Transform = val2;
			val2.Dispose();
		}
	}

	private void RenderFrame(MetroRendererInfo renderingInfo, int frameIndex)
	{
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Invalid comparison between Unknown and I4
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Invalid comparison between Unknown and I4
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Invalid comparison between Unknown and I4
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Invalid comparison between Unknown and I4
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf6: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Invalid comparison between Unknown and I4
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Invalid comparison between Unknown and I4
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Invalid comparison between Unknown and I4
		//IL_0c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Invalid comparison between Unknown and I4
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Invalid comparison between Unknown and I4
		MetroTileItem metroTileItem = (MetroTileItem)renderingInfo.Control;
		MetroTileFrame metroTileFrame = metroTileItem.Frames[frameIndex];
		MetroTileColorTable metroTile = renderingInfo.ColorTable.MetroTile;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		Rectangle bounds = metroTileItem.Bounds;
		Region val = null;
		if (!metroTileItem.DragStartPoint.IsEmpty)
		{
			bounds.Location = renderingInfo.ItemPaintArgs.ContainerControl.PointToClient(Control.MousePosition);
			bounds.Location.Offset(-metroTileItem.DragStartPoint.X, -metroTileItem.DragStartPoint.Y);
			val = graphics.Clip;
			graphics.SetClip(bounds, (CombineMode)0);
		}
		object containerControl = metroTileItem.ContainerControl;
		Control control = (Control)((containerControl is Control) ? containerControl : null);
		bounds.Inflate(-InflatePixels, -InflatePixels);
		if (metroTileItem.IsLeftMouseButtonDown)
		{
			bounds.Inflate(-InflatePixelsMouseDown, -InflatePixelsMouseDown);
		}
		else if (metroTileItem.IsMouseOver)
		{
			bounds.Inflate(InflatePixels, InflatePixels);
		}
		switch (metroTileItem.DesignInsertMarker)
		{
		case eDesignInsertPosition.After:
			if (metroTileItem.IsDesignMarkHorizontal)
			{
				bounds.Offset(-DragInsertOffsetPixels, 0);
			}
			else
			{
				bounds.Offset(0, -DragInsertOffsetPixels);
			}
			graphics.ResetClip();
			break;
		case eDesignInsertPosition.Before:
			if (metroTileItem.IsDesignMarkHorizontal)
			{
				bounds.Offset(DragInsertOffsetPixels, 0);
			}
			else
			{
				bounds.Offset(0, DragInsertOffsetPixels);
			}
			graphics.ResetClip();
			break;
		}
		bool dispose = false;
		bool enabled = metroTileItem.GetEnabled(renderingInfo.ItemPaintArgs.ContainerControl);
		ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(metroTileFrame.EffectiveStyle, out dispose);
		if (bounds.Width > 2048)
		{
			bounds.Width = 2048;
		}
		if (bounds.Height > 1600)
		{
			bounds.Height = 1600;
		}
		if (enabled)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(elementStyle, graphics, bounds);
			ElementStyleDisplay.Paint(e);
		}
		else
		{
			SolidBrush val2 = new SolidBrush(metroTileItem.DisabledBackColor.IsEmpty ? renderingInfo.ColorTable.MetroPartColors.CanvasColorLighterShade : metroTileItem.DisabledBackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle bounds2 = bounds;
		bounds2.X += elementStyle.PaddingLeft;
		bounds2.Y += elementStyle.PaddingTop;
		bounds2.Width -= elementStyle.PaddingHorizontal;
		bounds2.Height -= elementStyle.PaddingVertical;
		if (metroTileItem.IsMouseOver)
		{
			bounds2.Inflate(-InflatePixels, -InflatePixels);
		}
		if (metroTileItem.SubItems.Count > 0 && frameIndex < metroTileItem.SubItems.Count)
		{
			BaseItem baseItem = metroTileItem.SubItems[frameIndex];
			if (baseItem.Displayed)
			{
				baseItem.TopInternal = bounds.Y + elementStyle.PaddingTop + (bounds.Height - elementStyle.PaddingTop - metroTileFrame.TitleTextBounds.Height - baseItem.HeightInternal) / 2;
				baseItem.LeftInternal = bounds.X + elementStyle.PaddingLeft;
				baseItem.WidthInternal = metroTileItem.TileSize.Width - elementStyle.PaddingHorizontal;
				baseItem.Paint(renderingInfo.ItemPaintArgs);
			}
		}
		Image image = metroTileFrame.Image;
		ContentAlignment imageTextAlignment = metroTileFrame.ImageTextAlignment;
		Color color = (enabled ? elementStyle.TextColor : renderingInfo.ColorTable.MetroPartColors.CanvasColorLightShade);
		Color color2 = color;
		if (!metroTileFrame.SymbolColor.IsEmpty)
		{
			color2 = metroTileFrame.SymbolColor;
		}
		if (image != null || !string.IsNullOrEmpty(metroTileFrame.Symbol))
		{
			Font val3 = null;
			Rectangle empty = Rectangle.Empty;
			if (string.IsNullOrEmpty(metroTileFrame.Symbol))
			{
				empty = new Rectangle(0, 0, image.Width, image.Height);
			}
			else
			{
				val3 = Symbols.GetFontAwesome(metroTileFrame.SymbolSize);
				Size size = TextDrawing.MeasureString(graphics, metroTileFrame.Symbol, val3);
				int num = (int)Math.Ceiling((float)val3.FontFamily.GetCellDescent(val3.Style) * val3.Size / (float)val3.FontFamily.GetEmHeight(val3.Style));
				size.Height -= num;
				empty = new Rectangle(0, 0, size.Width, size.Height);
			}
			empty.Offset(bounds.Location);
			if ((int)imageTextAlignment == 1)
			{
				bounds2.X += empty.Width + metroTileFrame.ImageIndent.X;
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
			}
			else if ((int)imageTextAlignment == 2)
			{
				empty.X += (metroTileItem.TileSize.Width - image.Width) / 2;
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				bounds2.Y += empty.Height + metroTileFrame.ImageIndent.Y;
				bounds2.Height -= empty.Height;
			}
			else if ((int)imageTextAlignment == 4)
			{
				empty.X += metroTileItem.TileSize.Width - image.Width - metroTileFrame.ImageIndent.X;
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 512)
			{
				empty.Offset((metroTileItem.TileSize.Width - image.Width) / 2, metroTileItem.TileSize.Height - image.Height);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				bounds2.Height -= empty.Height;
			}
			else if ((int)imageTextAlignment == 256)
			{
				empty.Offset(0, metroTileItem.TileSize.Height - image.Height);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				bounds2.X += empty.Width + metroTileFrame.ImageIndent.X;
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 1024)
			{
				empty.Offset(metroTileItem.TileSize.Width - image.Width - metroTileFrame.ImageIndent.X, metroTileItem.TileSize.Height - image.Height);
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 32)
			{
				empty.Offset((metroTileItem.TileSize.Width - image.Width) / 2, (metroTileItem.TileSize.Height - image.Height) / 2);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				bounds2.Height = Math.Max(0, bounds2.Bottom - empty.Bottom);
				bounds2.Y = empty.Bottom + 1;
			}
			else if ((int)imageTextAlignment == 16)
			{
				empty.Offset(0, (metroTileItem.TileSize.Height - image.Height) / 2);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				bounds2.X += empty.Width + metroTileFrame.ImageIndent.X;
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 64)
			{
				empty.Offset(metroTileItem.TileSize.Width - image.Width - metroTileFrame.ImageIndent.X, (metroTileItem.TileSize.Height - image.Height) / 2);
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				bounds2.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else
			{
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
			}
			if (string.IsNullOrEmpty(metroTileFrame.Symbol))
			{
				graphics.DrawImage(image, empty);
			}
			else
			{
				TextDrawing.DrawStringLegacy(graphics, metroTileFrame.Symbol, val3, color2, new Rectangle(empty.X, empty.Y, 0, 0), eTextFormat.Default);
			}
		}
		if (bounds2.Width > 0 && bounds2.Height > 0 && metroTileFrame.Text != null)
		{
			Font val4 = renderingInfo.DefaultFont;
			if (elementStyle.Font != null)
			{
				val4 = elementStyle.Font;
			}
			bool rightToLeft = renderingInfo.RightToLeft;
			if (metroTileFrame.TextMarkupBody == null)
			{
				eTextFormat eTextFormat = eTextFormat.NoClipping | eTextFormat.WordBreak;
				if (elementStyle.TextLineAlignment == eStyleTextAlignment.Center)
				{
					eTextFormat |= eTextFormat.VerticalCenter;
				}
				else if (elementStyle.TextLineAlignment == eStyleTextAlignment.Far)
				{
					eTextFormat |= eTextFormat.Bottom;
				}
				if (elementStyle.TextAlignment == eStyleTextAlignment.Center)
				{
					eTextFormat |= eTextFormat.HorizontalCenter;
				}
				else if (elementStyle.TextAlignment == eStyleTextAlignment.Far)
				{
					eTextFormat |= eTextFormat.Right;
				}
				TextDrawing.DrawString(graphics, metroTileFrame.Text, val4, color, bounds2, eTextFormat);
			}
			else
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val4, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = false;
				markupDrawContext.ContextObject = metroTileItem;
				metroTileFrame.TextMarkupBody.Bounds = bounds2;
				metroTileFrame.TextMarkupBody.Render(markupDrawContext);
			}
		}
		if (metroTileFrame.TitleText != null)
		{
			Color color3 = (enabled ? metroTileFrame.TitleTextColor : renderingInfo.ColorTable.MetroPartColors.CanvasColorLightShade);
			if (color3.IsEmpty)
			{
				color3 = elementStyle.TextColor;
			}
			Font titleTextFont = metroTileItem.GetTitleTextFont(metroTileFrame, elementStyle, control);
			Rectangle titleTextBounds = metroTileFrame.TitleTextBounds;
			titleTextBounds.Offset(bounds.Location);
			if (metroTileItem.IsMouseOver)
			{
				titleTextBounds.Offset(InflatePixels, InflatePixels);
			}
			TextDrawing.DrawString(graphics, metroTileFrame.TitleText, titleTextFont, color3, titleTextBounds, eTextFormat.SingleLine);
		}
		if (metroTileItem.Checked)
		{
			int right = bounds.Right;
			Size checkMarkSize = CheckMarkSize;
			int x = right - checkMarkSize.Width;
			int y = bounds.Y;
			checkMarkSize = CheckMarkSize;
			int width = checkMarkSize.Width;
			checkMarkSize = CheckMarkSize;
			Rectangle rectangle = new Rectangle(x, y, width, checkMarkSize.Height);
			GraphicsPath val5 = new GraphicsPath();
			try
			{
				val5.AddLine(rectangle.X, rectangle.Y, rectangle.Right - 1, rectangle.Y);
				val5.AddLine(rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1);
				val5.CloseFigure();
				SolidBrush val6 = new SolidBrush(metroTile.CheckBackground);
				try
				{
					graphics.FillPath((Brush)(object)val6, val5);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			SolidBrush val7 = new SolidBrush(metroTile.CheckForeground);
			try
			{
				Rectangle rectangle2 = default(Rectangle);
				rectangle2.Size = new Size(11, 11);
				rectangle2.X = rectangle.Right - rectangle2.Size.Width - 3;
				rectangle2.Y = rectangle.Y + 2;
				Pen val8 = new Pen(metroTile.CheckForeground, 2f);
				try
				{
					graphics.DrawEllipse(val8, rectangle2);
					graphics.DrawLine(val8, rectangle2.X + 3, rectangle2.Y + 5, rectangle2.X + 6, rectangle2.Y + 8);
					graphics.DrawLine(val8, rectangle2.X + 6, rectangle2.Y + 8, rectangle2.X + 9, rectangle2.Y + 3);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (dispose)
		{
			elementStyle.Dispose();
		}
		if (val != null)
		{
			graphics.Clip = val;
			val.Dispose();
		}
	}
}
