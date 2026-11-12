using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AXIMAGELib;

namespace AxAXIMAGELib;

[DesignTimeVisible(true)]
[Clsid("{4fed1ac6-6cf3-11d6-bb83-9fdd9223647d}")]
public class AxAxImage : AxHost
{
	private _DAxImage ocx;

	private AxAxImageEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(-501)]
	[ComAliasName("System.UInt32")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (ocx != null && ((AxHost)this).PropsValid())
			{
				return AxHost.GetColorFromOleColor(ocx.BackColor);
			}
			return ((AxHost)this).BackColor;
		}
		set
		{
			((AxHost)this).BackColor = value;
			if (ocx != null)
			{
				ocx.BackColor = AxHost.GetOleColorFromColor(value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1)]
	public virtual bool Border
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Border", (ActiveXInvokeKind)1);
			}
			return ocx.Border;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Border", (ActiveXInvokeKind)2);
			}
			ocx.Border = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(2)]
	public virtual bool Stretch
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Stretch", (ActiveXInvokeKind)1);
			}
			return ocx.Stretch;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Stretch", (ActiveXInvokeKind)2);
			}
			ocx.Stretch = value;
		}
	}

	public event EventHandler ClickEvent;

	public AxAxImage()
		: base("4fed1ac6-6cf3-11d6-bb83-9fdd9223647d")
	{
	}

	public virtual string GetLastError()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetLastError", (ActiveXInvokeKind)0);
		}
		return ocx.GetLastError();
	}

	public virtual int GetJpegQuality()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetJpegQuality", (ActiveXInvokeKind)0);
		}
		return ocx.GetJpegQuality();
	}

	public virtual void SetJpegQuality(int q)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetJpegQuality", (ActiveXInvokeKind)0);
		}
		ocx.SetJpegQuality(q);
	}

	public virtual int GetXDPI()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetXDPI", (ActiveXInvokeKind)0);
		}
		return ocx.GetXDPI();
	}

	public virtual int GetYDPI()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetYDPI", (ActiveXInvokeKind)0);
		}
		return ocx.GetYDPI();
	}

	public virtual void SetXDPI(int dpi)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetXDPI", (ActiveXInvokeKind)0);
		}
		ocx.SetXDPI(dpi);
	}

	public virtual void SetYDPI(int dpi)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetYDPI", (ActiveXInvokeKind)0);
		}
		ocx.SetYDPI(dpi);
	}

	public virtual int CopyToHandle()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CopyToHandle", (ActiveXInvokeKind)0);
		}
		return ocx.CopyToHandle();
	}

	public virtual int StringToFormat(string extension)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StringToFormat", (ActiveXInvokeKind)0);
		}
		return ocx.StringToFormat(extension);
	}

	public virtual void SetGray8Image(ref object image, short width, short height)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetGray8Image", (ActiveXInvokeKind)0);
		}
		ocx.SetGray8Image(ref image, width, height);
	}

	public virtual void SetJpgImage(ref object image)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetJpgImage", (ActiveXInvokeKind)0);
		}
		ocx.SetJpgImage(ref image);
	}

	public virtual float Mean()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Mean", (ActiveXInvokeKind)0);
		}
		return ocx.Mean();
	}

	public virtual bool Dilate(int ksize)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Dilate", (ActiveXInvokeKind)0);
		}
		return ocx.Dilate(ksize);
	}

	public virtual bool Erode(int ksize)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Erode", (ActiveXInvokeKind)0);
		}
		return ocx.Erode(ksize);
	}

	public virtual bool Filter(ref int kernel, int ksize, int kfactor, int koffset)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Filter", (ActiveXInvokeKind)0);
		}
		return ocx.Filter(ref kernel, ksize, kfactor, koffset);
	}

	public virtual int GetNumFrames()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetNumFrames", (ActiveXInvokeKind)0);
		}
		return ocx.GetNumFrames();
	}

	public virtual int GetFrame()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetFrame", (ActiveXInvokeKind)0);
		}
		return ocx.GetFrame();
	}

	public virtual void SetFrame(int nFrame)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetFrame", (ActiveXInvokeKind)0);
		}
		ocx.SetFrame(nFrame);
	}

	public virtual int GetTransIndex()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetTransIndex", (ActiveXInvokeKind)0);
		}
		return ocx.GetTransIndex();
	}

	public virtual Color GetTransColor()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetTransColor", (ActiveXInvokeKind)0);
		}
		uint transColor = ocx.GetTransColor();
		return AxHost.GetColorFromOleColor(transColor);
	}

	public virtual void SetTransIndex(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetTransIndex", (ActiveXInvokeKind)0);
		}
		ocx.SetTransIndex(index);
	}

	public virtual void SetTransColor(int color)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetTransColor", (ActiveXInvokeKind)0);
		}
		ocx.SetTransColor(color);
	}

	public virtual int GetPaletteSize()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPaletteSize", (ActiveXInvokeKind)0);
		}
		return ocx.GetPaletteSize();
	}

	public virtual int GetPalette()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPalette", (ActiveXInvokeKind)0);
		}
		return ocx.GetPalette();
	}

	public virtual Color GetPaletteColor(int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPaletteColor", (ActiveXInvokeKind)0);
		}
		uint paletteColor = ocx.GetPaletteColor(index);
		return AxHost.GetColorFromOleColor(paletteColor);
	}

	public virtual void SetPalette(ref int pal, int ncolors)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetPalette", (ActiveXInvokeKind)0);
		}
		ocx.SetPalette(ref pal, ncolors);
	}

	public virtual void SetPaletteIndex(int index, uint color)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetPaletteIndex", (ActiveXInvokeKind)0);
		}
		ocx.SetPaletteIndex(index, color);
	}

	public virtual void SetGrayPalette()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetGrayPalette", (ActiveXInvokeKind)0);
		}
		ocx.SetGrayPalette();
	}

	public virtual bool IsGrayScale()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IsGrayScale", (ActiveXInvokeKind)0);
		}
		return ocx.IsGrayScale();
	}

	public virtual bool IsIndexed()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IsIndexed", (ActiveXInvokeKind)0);
		}
		return ocx.IsIndexed();
	}

	public virtual void SwapIndex(int index1, int index2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SwapIndex", (ActiveXInvokeKind)0);
		}
		ocx.SwapIndex(index1, index2);
	}

	public virtual void HuePalette(float correction)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("HuePalette", (ActiveXInvokeKind)0);
		}
		ocx.HuePalette(correction);
	}

	public virtual bool IsInside(int x, int y)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IsInside", (ActiveXInvokeKind)0);
		}
		return ocx.IsInside(x, y);
	}

	public virtual int GetPixelIndex(int x, int y)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPixelIndex", (ActiveXInvokeKind)0);
		}
		return ocx.GetPixelIndex(x, y);
	}

	public virtual Color GetPixelColor(int x, int y)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetPixelColor", (ActiveXInvokeKind)0);
		}
		uint pixelColor = ocx.GetPixelColor(x, y);
		return AxHost.GetColorFromOleColor(pixelColor);
	}

	public virtual void SetPixelIndex(int x, int y, int index)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetPixelIndex", (ActiveXInvokeKind)0);
		}
		ocx.SetPixelIndex(x, y, index);
	}

	public virtual void SetPixelColor(int x, int y, uint color)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetPixelColor", (ActiveXInvokeKind)0);
		}
		ocx.SetPixelColor(x, y, color);
	}

	public virtual int GetWidth()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetWidth", (ActiveXInvokeKind)0);
		}
		return ocx.GetWidth();
	}

	public virtual int GetHeight()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetHeight", (ActiveXInvokeKind)0);
		}
		return ocx.GetHeight();
	}

	public virtual int GetNumColors()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetNumColors", (ActiveXInvokeKind)0);
		}
		return ocx.GetNumColors();
	}

	public virtual int GetColorType()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetColorType", (ActiveXInvokeKind)0);
		}
		return ocx.GetColorType();
	}

	public virtual int GetBpp()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetBpp", (ActiveXInvokeKind)0);
		}
		return ocx.GetBpp();
	}

	public virtual int GetType()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetType", (ActiveXInvokeKind)0);
		}
		return ocx.GetType();
	}

	public virtual void Refresh()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Refresh", (ActiveXInvokeKind)0);
		}
		ocx.Refresh();
	}

	public virtual int Load(string fileName, int imageType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Load", (ActiveXInvokeKind)0);
		}
		return ocx.Load(fileName, imageType);
	}

	public virtual int Save(string fileName, int imageType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Save", (ActiveXInvokeKind)0);
		}
		return ocx.Save(fileName, imageType);
	}

	public virtual int CreateFromHandle(int handle)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CreateFromHandle", (ActiveXInvokeKind)0);
		}
		return ocx.CreateFromHandle(handle);
	}

	public virtual int CreateFromBitmap(int bitmap)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CreateFromBitmap", (ActiveXInvokeKind)0);
		}
		return ocx.CreateFromBitmap(bitmap);
	}

	public virtual int MakeBitmap(int hdc)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("MakeBitmap", (ActiveXInvokeKind)0);
		}
		return ocx.MakeBitmap(hdc);
	}

	public virtual int Create(int width, int height, int bpp)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Create", (ActiveXInvokeKind)0);
		}
		return ocx.Create(width, height, bpp);
	}

	public virtual void Clear(int val)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Clear", (ActiveXInvokeKind)0);
		}
		ocx.Clear(val);
	}

	public virtual void SetStdPalette()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetStdPalette", (ActiveXInvokeKind)0);
		}
		ocx.SetStdPalette();
	}

	public virtual bool IsValid()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IsValid", (ActiveXInvokeKind)0);
		}
		return ocx.IsValid();
	}

	public virtual bool Flip()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Flip", (ActiveXInvokeKind)0);
		}
		return ocx.Flip();
	}

	public virtual bool Mirror()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Mirror", (ActiveXInvokeKind)0);
		}
		return ocx.Mirror();
	}

	public virtual bool Rotate(float angle)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Rotate", (ActiveXInvokeKind)0);
		}
		return ocx.Rotate(angle);
	}

	public virtual bool RotateLeft()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RotateLeft", (ActiveXInvokeKind)0);
		}
		return ocx.RotateLeft();
	}

	public virtual bool RotateRight()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("RotateRight", (ActiveXInvokeKind)0);
		}
		return ocx.RotateRight();
	}

	public virtual bool Rotate180()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Rotate180", (ActiveXInvokeKind)0);
		}
		return ocx.Rotate180();
	}

	public virtual bool Negative()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Negative", (ActiveXInvokeKind)0);
		}
		return ocx.Negative();
	}

	public virtual bool GrayScale()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GrayScale", (ActiveXInvokeKind)0);
		}
		return ocx.GrayScale();
	}

	public virtual bool Resample(int newx, int newy, int fast)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Resample", (ActiveXInvokeKind)0);
		}
		return ocx.Resample(newx, newy, fast);
	}

	public virtual bool DecreaseBpp(int bpp, int errordiffusion)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DecreaseBpp", (ActiveXInvokeKind)0);
		}
		return ocx.DecreaseBpp(bpp, errordiffusion);
	}

	public virtual bool IncreaseBpp(int bpp)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("IncreaseBpp", (ActiveXInvokeKind)0);
		}
		return ocx.IncreaseBpp(bpp);
	}

	public virtual bool Dither(int method)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Dither", (ActiveXInvokeKind)0);
		}
		return ocx.Dither(method);
	}

	public virtual bool Crop(int left, int top, int right, int bottom)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Crop", (ActiveXInvokeKind)0);
		}
		return ocx.Crop(left, top, right, bottom);
	}

	public virtual bool Threshold(int level)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Threshold", (ActiveXInvokeKind)0);
		}
		return ocx.Threshold(level);
	}

	public virtual bool Colorize(int hue, int sat)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Colorize", (ActiveXInvokeKind)0);
		}
		return ocx.Colorize(hue, sat);
	}

	public virtual bool Light(int light, int contrast)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Light", (ActiveXInvokeKind)0);
		}
		return ocx.Light(light, contrast);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxAxImageEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(_DAxImageEvents));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = (_DAxImage)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnClickEvent(object sender, EventArgs e)
	{
		if (this.ClickEvent != null)
		{
			this.ClickEvent(sender, e);
		}
	}
}
