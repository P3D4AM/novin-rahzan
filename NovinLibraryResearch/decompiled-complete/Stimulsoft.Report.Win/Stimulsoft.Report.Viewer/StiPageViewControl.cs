using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters.Context.Animation;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiPageViewControl : Panel
{
	private static int indexValidator;

	public StiViewerControl ViewerControl { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsLockPaint { get; set; }

	[Browsable(false)]
	internal StiAnimationEngine AnimationEngine { get; set; }

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		e.Graphics.CompositingMode = (CompositingMode)0;
		e.Graphics.CompositingQuality = (CompositingQuality)1;
		e.Graphics.PixelOffsetMode = (PixelOffsetMode)3;
		e.Graphics.InterpolationMode = (InterpolationMode)0;
		Color color = StiOptions.Viewer.Windows.BackgroundColor;
		if (color.IsEmpty)
		{
			color = ((Control)this).BackColor;
		}
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			e.Graphics.FillRectangle(val, e.ClipRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (IsLockPaint || ViewerControl == null || ViewerControl.Report == null || ViewerControl.Report.RenderedPages.Count <= 0 || ViewerControl.PageParams == null || ViewerControl.PageParams.Count == 0)
		{
			return;
		}
		if (ViewerControl.IsEditorActivated)
		{
			ViewerControl.TextEditor.Refresh();
		}
		if (ViewerControl.Report == null)
		{
			return;
		}
		int num = ViewerControl.CurrentPrintPage;
		Graphics graphics = e.Graphics;
		graphics.ResetClip();
		graphics.SetClip(e.ClipRectangle);
		if (num >= ViewerControl.PageParams.Count)
		{
			num = ViewerControl.PageParams.Count - 1;
		}
		if (num < 1)
		{
			num = 0;
		}
		StiPageParamView view = (StiPageParamView)ViewerControl.PageParams[num];
		Rectangle rectangle = ViewerControl.GetPageRect(view);
		if (!((Control)this).ClientRectangle.IntersectsWith(rectangle))
		{
			num = -1;
			rectangle = Rectangle.Empty;
		}
		int num2 = 0;
		foreach (StiPageParamView pageParam in ViewerControl.PageParams)
		{
			Rectangle pageRect = ViewerControl.GetPageRect(pageParam);
			if (e.ClipRectangle.IntersectsWith(pageRect) && pageParam.Page.Report != null && (ViewerControl.PageViewMode != 0 || (ViewerControl.CurrentPrintPage == num2 && ViewerControl.PageViewMode == StiPageViewMode.SinglePage)))
			{
				graphics.ResetTransform();
				graphics.TranslateTransform((float)pageRect.X, (float)pageRect.Y);
				Rectangle clipRectangle = e.ClipRectangle;
				clipRectangle.X -= pageRect.X;
				clipRectangle.Y -= pageRect.Y;
				StiPaintEventArgs stiPaintEventArgs = new StiPaintEventArgs(graphics, RectangleD.CreateFromRectangle(clipRectangle));
				stiPaintEventArgs.AnimationEngine = AnimationEngine;
				if (pageParam.Page.Report != null && pageParam.Page.Report.RenderedPages.CacheMode)
				{
					pageParam.Page.Report.RenderedPages.GetPage(pageParam.Page);
				}
				((StiComponent)pageParam.Page).Paint(stiPaintEventArgs);
				StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
				bool flag = !StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey);
				if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
				{
					flag = true;
				}
				if (!flag)
				{
					try
					{
						if (indexValidator == 10)
						{
							using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512))
							{
								using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
								rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
								flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
							}
							indexValidator = 0;
						}
						else
						{
							indexValidator++;
						}
					}
					catch (Exception)
					{
						flag = true;
					}
					indexValidator = 0;
				}
				if (flag)
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.LineAlignment = (StringAlignment)1;
						val2.Alignment = (StringAlignment)1;
						StiPage page = pageParam.Page;
						Color color2 = (page.IsPrinting ? Color.LightGray : Color.FromArgb(30, 100, 100, 100));
						double zoom = page.Report.Info.Zoom;
						SolidBrush val3 = new SolidBrush(color2);
						try
						{
							Font val4 = new Font("Arial", (float)(150.0 * zoom), (FontStyle)1);
							try
							{
								double num3 = page.Unit.ConvertToHInches(page.Margins.Left);
								double num4 = page.Unit.ConvertToHInches(page.Margins.Top);
								StiUnit unit = page.Unit;
								RectangleD val5 = page.DisplayRectangle;
								double num5 = unit.ConvertToHInches(((RectangleD)(ref val5)).Width);
								StiUnit unit2 = page.Unit;
								val5 = page.DisplayRectangle;
								double num6 = unit2.ConvertToHInches(((RectangleD)(ref val5)).Height);
								val5 = new RectangleD((0.0 - num3) * zoom, (0.0 - num4) * zoom, num5 * zoom, num6 * zoom);
								RectangleD val6 = ((RectangleD)(ref val5)).Multiply(StiScale.Factor);
								StiTextDrawing.DrawString(graphics, "Trial", val4, (Brush)(object)val3, val6, val2, 45f, 1.0);
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
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				graphics.ResetTransform();
				if (ViewerControl != null)
				{
					Pen val7 = new Pen(Color.FromArgb(255, 119, 119, 119));
					try
					{
						graphics.DrawRectangle(val7, pageRect);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
			}
			if (num == -1 && ((Control)this).ClientRectangle.IntersectsWith(pageRect))
			{
				num = num2;
				rectangle = pageRect;
				ViewerControl.InvalidatePageRect(rectangle);
			}
			num2++;
		}
		graphics.ResetTransform();
		StiViewerHelper.DrawPageBorder(graphics, rectangle, active: true);
		if (ViewerControl.CurrentPrintPage != num)
		{
			ViewerControl.CurrentPrintPage = num;
		}
		ViewerControl.RefreshPageNofM();
		AnimationEngine.Continue();
	}

	public override bool PreProcessMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 256)
		{
			if (((Message)(ref m)).WParam == (IntPtr)38)
			{
				int value = ((ScrollBar)ViewerControl.VScrollBar).Value;
				if (value > 0)
				{
					ViewerControl.VScrollBar.DoUp();
					StiControlPaint.ScrollWindow(((Control)ViewerControl.View).Handle, 0, value - ((ScrollBar)ViewerControl.VScrollBar).Value);
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)40)
			{
				if (!ViewerControl.VScrollBar.IsEnd)
				{
					int value2 = ((ScrollBar)ViewerControl.VScrollBar).Value;
					ViewerControl.VScrollBar.DoDown();
					StiControlPaint.ScrollWindow(((Control)ViewerControl.View).Handle, 0, value2 - ((ScrollBar)ViewerControl.VScrollBar).Value);
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)37)
			{
				int value3 = ((ScrollBar)ViewerControl.HScrollBar).Value;
				if (value3 > 0)
				{
					ViewerControl.HScrollBar.DoLeft();
					StiControlPaint.ScrollWindow(((Control)ViewerControl.View).Handle, value3 - ((ScrollBar)ViewerControl.HScrollBar).Value, 0);
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)39)
			{
				if (!ViewerControl.HScrollBar.IsEnd)
				{
					int value4 = ((ScrollBar)ViewerControl.HScrollBar).Value;
					ViewerControl.HScrollBar.DoRight();
					StiControlPaint.ScrollWindow(((Control)ViewerControl.View).Handle, value4 - ((ScrollBar)ViewerControl.HScrollBar).Value, 0);
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)36)
			{
				ViewerControl.FirstPage();
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)35)
			{
				ViewerControl.LastPage();
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)33)
			{
				ViewerControl.PrevPage();
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)34)
			{
				ViewerControl.NextPage();
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)27 && !ViewerControl.IgnoreEscapeKey)
			{
				ViewerControl.InvokeClose();
				return false;
			}
		}
		return ((Control)this).PreProcessMessage(ref m);
	}

	private void AnimationEngine_Animation(StiComponent component)
	{
		ViewerControl.InvalidateComponent(component);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && AnimationEngine != null)
		{
			AnimationEngine.Animation -= AnimationEngine_Animation;
			AnimationEngine.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	public StiPageViewControl()
	{
		if (!((Component)this).DesignMode)
		{
			((Control)this).DoubleBuffered = true;
			((Panel)this).BorderStyle = (BorderStyle)0;
			((Control)this).SetStyle((ControlStyles)512, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle((ControlStyles)65536, true);
			((Panel)this).TabStop = true;
			AnimationEngine = new StiAnimationEngine();
			AnimationEngine.Animation += AnimationEngine_Animation;
		}
	}
}
