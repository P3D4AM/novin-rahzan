using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiMetafileParser
{
	internal struct SIZEL
	{
		public int cX;

		public int cY;
	}

	internal struct POINTL
	{
		public int X;

		public int Y;
	}

	internal struct RECTL
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct LOGFONT
	{
		public int lfHeight;

		public int lfWidth;

		public int lfEscapement;

		public int lfOrientation;

		public int lfWeight;

		public byte lfItalic;

		public byte lfUnderline;

		public byte lfStrikeOut;

		public byte lfCharSet;

		public byte lfOutPrecision;

		public byte lfClipPrecision;

		public byte lfQuality;

		public byte lfPitchAndFamily;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName;
	}

	internal struct tagENHMETAHEADER
	{
		public RECTL rclBounds;

		public RECTL rclFrame;

		public uint dSignature;

		public uint nVersion;

		public uint nBytes;

		public uint nRecords;

		public ushort nHandles;

		public ushort sReserved;

		public uint nDescription;

		public uint offDescription;

		public uint nPalEntries;

		public SIZEL szlDevice;

		public SIZEL szlMillimeters;

		public uint cbPixelFormat;

		public uint offPixelFormat;

		public uint bOpenGL;
	}

	internal struct XFORM
	{
		public float eM11;

		public float eM12;

		public float eM21;

		public float eM22;

		public float eDx;

		public float eDy;

		public XFORM(float eM11, float eM12, float eM21, float eM22, float eDx, float eDy)
		{
			this.eM11 = eM11;
			this.eM12 = eM12;
			this.eM21 = eM21;
			this.eM22 = eM22;
			this.eDx = eDx;
			this.eDy = eDy;
		}
	}

	internal struct POINTD
	{
		public double X;

		public double Y;
	}

	internal class EmfFontInfo
	{
		public int internalFontNumber;

		public LOGFONT elfLogFont;
	}

	internal class EmfBrushInfo
	{
		public uint lbStyle;

		public uint lbColor;

		public uint lbHatch;
	}

	internal class EmfPenInfo
	{
		public uint lopnStyle;

		public int lopnWidth;

		public uint lopnColor;
	}

	internal class GDIobjectInfo
	{
		public uint ihNumber;

		public EmfFontInfo Font;

		public EmfBrushInfo Brush;

		public EmfPenInfo Pen;
	}

	private class DCData
	{
		public uint emfBackColor;

		public uint emfTextColor;

		public uint emfBkMode;

		public uint emfPolyFillMode;

		public uint emfMapMode;

		public uint emfTextAlign;
	}

	private byte[] dataArray;

	private int dataOffset;

	private const double penWidthScale = 1.2;

	private const double penWidthDefault = 0.1;

	internal List<GDIobjectInfo> GDIobjects = new List<GDIobjectInfo>();

	private uint gdiSelectedPen;

	private uint gdiSelectedBrush;

	private uint gdiSelectedFont;

	private uint emfMapMode;

	private PointD viewportOrg = PointD.Empty;

	private PointD viewportExt = PointD.Empty;

	private PointD windowOrg = PointD.Empty;

	private PointD windowExt = PointD.Empty;

	private uint viewportFlags;

	private XFORM baseXForm;

	private double emfX;

	private double emfY;

	private int emfOffsetX;

	private int emfOffsetY;

	private double emfScaleX;

	private double emfScaleY;

	private Stack dcStack;

	private static object lockMetafileFlag = new object();

	private void SelectObject(uint ihNumber)
	{
		switch (ihNumber)
		{
		case 2147483653u:
			gdiSelectedBrush = uint.MaxValue;
			return;
		case 2147483656u:
			gdiSelectedPen = uint.MaxValue;
			return;
		}
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			GDIobjectInfo gDIobjectInfo = GDIobjects[num];
			if (gDIobjectInfo.ihNumber == ihNumber)
			{
				if (gDIobjectInfo.Pen != null)
				{
					gdiSelectedPen = ihNumber;
				}
				if (gDIobjectInfo.Brush != null)
				{
					gdiSelectedBrush = ihNumber;
				}
				if (gDIobjectInfo.Font != null)
				{
					gdiSelectedFont = ihNumber;
				}
				break;
			}
		}
	}

	private void RemoveObject(uint ihNumber)
	{
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			if (GDIobjects[num].ihNumber == ihNumber)
			{
				GDIobjects.RemoveAt(num);
			}
		}
	}

	private int GetGdiObjectIndex(uint ihNumber)
	{
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			if (GDIobjects[num].ihNumber == ihNumber)
			{
				return num;
			}
		}
		return 0;
	}

	private void AddStockGDIobjects()
	{
		GDIobjectInfo gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = uint.MaxValue;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483648u;
		EmfBrushInfo emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 16777215u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483649u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 12632256u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483650u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 8421504u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483651u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 4210752u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483652u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 0u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483654u;
		EmfPenInfo emfPenInfo = new EmfPenInfo();
		emfPenInfo.lopnStyle = 0u;
		emfPenInfo.lopnWidth = 0;
		emfPenInfo.lopnColor = 16777215u;
		gDIobjectInfo.Pen = emfPenInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483655u;
		emfPenInfo = new EmfPenInfo();
		emfPenInfo.lopnStyle = 0u;
		emfPenInfo.lopnWidth = 0;
		emfPenInfo.lopnColor = 0u;
		gDIobjectInfo.Pen = emfPenInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		SelectObject(2147483648u);
		SelectObject(2147483655u);
	}

	private Color ConvertColorFromGdi(uint gdiColor)
	{
		return Color.FromArgb((int)(gdiColor & 0xFF), (int)((gdiColor & 0xFF00) >> 8), (int)((gdiColor & 0xFF0000) >> 16));
	}

	private double GetTransformedCX(int x)
	{
		return (float)x * baseXForm.eM11;
	}

	private double GetTransformedCY(int y)
	{
		return (float)y * baseXForm.eM22;
	}

	private double GetTransformedX(int x)
	{
		return (float)x * baseXForm.eM11 - (float)emfOffsetX;
	}

	private double GetTransformedY(int y)
	{
		return (float)y * baseXForm.eM22 - (float)emfOffsetY;
	}

	private POINTD GetTransformedXY(int x, int y)
	{
		POINTD result = default(POINTD);
		result.X = (float)x * baseXForm.eM11 - (float)emfOffsetX;
		result.Y = (float)y * baseXForm.eM22 - (float)emfOffsetY;
		return result;
	}

	private double GetTransformedW(int w)
	{
		return (float)w * ((baseXForm.eM11 + baseXForm.eM22) / 2f);
	}

	private void SetViewport()
	{
		if (viewportFlags == 15 && (emfMapMode == 7 || emfMapMode == 8))
		{
			float num = (float)(((PointD)(ref viewportExt)).X / ((PointD)(ref windowExt)).X);
			float num2 = (float)(((PointD)(ref viewportExt)).Y / ((PointD)(ref windowExt)).Y);
			float num3 = num;
			float num4 = num2;
			float num5 = (float)(emfX * (double)(1f - num) + (((PointD)(ref viewportOrg)).X - ((PointD)(ref windowOrg)).X * (double)num) * emfScaleX);
			float num6 = (float)(emfY * (double)(1f - num2) - (((PointD)(ref viewportOrg)).Y - ((PointD)(ref windowOrg)).Y * (double)num2) * emfScaleY);
			viewportFlags = 0u;
		}
	}

	private int ReadInt32()
	{
		int result = BitConverter.ToInt32(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private short ReadInt16()
	{
		short result = BitConverter.ToInt16(dataArray, dataOffset);
		dataOffset += 2;
		return result;
	}

	private uint ReadUInt32()
	{
		uint result = BitConverter.ToUInt32(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private ushort ReadUInt16()
	{
		ushort result = BitConverter.ToUInt16(dataArray, dataOffset);
		dataOffset += 2;
		return result;
	}

	private byte ReadUInt8()
	{
		byte result = dataArray[dataOffset];
		dataOffset++;
		return result;
	}

	private float ReadSingle()
	{
		float result = BitConverter.ToSingle(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private string ReadStringTchar(int bufSize)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; BitConverter.ToUInt16(dataArray, dataOffset + i * 2) != 0 && i < bufSize; i++)
		{
			stringBuilder.Append(BitConverter.ToChar(dataArray, dataOffset + i * 2));
		}
		dataOffset += bufSize * 2;
		return stringBuilder.ToString();
	}

	private bool MetafileCallback(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		dataArray = null;
		dataOffset = 0;
		if (data != IntPtr.Zero)
		{
			dataArray = new byte[dataSize];
			Marshal.Copy(data, dataArray, 0, dataSize);
		}
		if (viewportFlags == 15 && (int)recordType != 12 && (int)recordType != 11 && (int)recordType != 10 && (int)recordType != 9)
		{
			SetViewport();
		}
		if ((int)recordType == 1)
		{
			tagENHMETAHEADER tagENHMETAHEADER = default(tagENHMETAHEADER);
			tagENHMETAHEADER.rclBounds.Left = ReadInt32();
			tagENHMETAHEADER.rclBounds.Top = ReadInt32();
			tagENHMETAHEADER.rclBounds.Right = ReadInt32();
			tagENHMETAHEADER.rclBounds.Bottom = ReadInt32();
			tagENHMETAHEADER.rclFrame.Left = ReadInt32();
			tagENHMETAHEADER.rclFrame.Top = ReadInt32();
			tagENHMETAHEADER.rclFrame.Right = ReadInt32();
			tagENHMETAHEADER.rclFrame.Bottom = ReadInt32();
			tagENHMETAHEADER.dSignature = ReadUInt32();
			tagENHMETAHEADER.nVersion = ReadUInt32();
			tagENHMETAHEADER.nBytes = ReadUInt32();
			tagENHMETAHEADER.nRecords = ReadUInt32();
			tagENHMETAHEADER.nHandles = ReadUInt16();
			tagENHMETAHEADER.sReserved = ReadUInt16();
			tagENHMETAHEADER.nDescription = ReadUInt32();
			tagENHMETAHEADER.offDescription = ReadUInt32();
			tagENHMETAHEADER.nPalEntries = ReadUInt32();
			tagENHMETAHEADER.szlDevice.cX = ReadInt32();
			tagENHMETAHEADER.szlDevice.cY = ReadInt32();
			tagENHMETAHEADER.szlMillimeters.cX = ReadInt32();
			tagENHMETAHEADER.szlMillimeters.cY = ReadInt32();
			tagENHMETAHEADER.cbPixelFormat = ReadUInt32();
			tagENHMETAHEADER.offPixelFormat = ReadUInt32();
			tagENHMETAHEADER.bOpenGL = ReadUInt32();
			emfOffsetX = tagENHMETAHEADER.rclBounds.Left;
			emfOffsetY = tagENHMETAHEADER.rclBounds.Top;
			int num = tagENHMETAHEADER.rclBounds.Right - tagENHMETAHEADER.rclBounds.Left;
			int num2 = tagENHMETAHEADER.rclBounds.Bottom - tagENHMETAHEADER.rclBounds.Top;
			if (emfOffsetX != 0)
			{
				num += emfOffsetX * 2;
				emfOffsetX = 0;
			}
			if (emfOffsetY != 0)
			{
				num2 += emfOffsetY * 2;
				emfOffsetY = 0;
			}
			baseXForm = new XFORM(1f, 0f, 0f, 1f, 0f, 0f);
			viewportFlags = 0u;
			emfMapMode = 0u;
			dcStack = new Stack();
		}
		return true;
	}

	private void RenderRtf(Metafile metafile)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		Bitmap val = null;
		Graphics val2 = null;
		EnumerateMetafileProc val3 = null;
		Point point = new Point(0, 0);
		lock (lockMetafileFlag)
		{
			if (val == null)
			{
				val = new Bitmap(25, 25);
			}
			if (val2 == null)
			{
				val2 = Graphics.FromImage((Image)(object)val);
			}
			if (val3 == null)
			{
				val3 = new EnumerateMetafileProc(MetafileCallback);
			}
			try
			{
				GDIobjects.Clear();
				AddStockGDIobjects();
				val2.EnumerateMetafile(metafile, point, val3);
			}
			catch
			{
			}
			finally
			{
				GDIobjects.Clear();
			}
		}
	}
}
