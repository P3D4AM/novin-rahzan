using System.Collections;
using System.Collections.Generic;

namespace Stimulsoft.Report.Export;

public sealed class StiPdfStructure
{
	public class StiPdfObjInfo
	{
		public int Ref = -1;

		internal StiPdfStructure Info;

		public bool IsUsed => Ref != -1;

		public void AddRef()
		{
			Info.AddRef(this);
		}

		public override string ToString()
		{
			if (Ref == -1)
			{
				return "-";
			}
			return $"ref = {Ref}";
		}
	}

	public class StiPdfContentObjInfo : StiPdfObjInfo
	{
		public StiPdfObjInfo Content;
	}

	public class StiPdfXObjectObjInfo : StiPdfObjInfo
	{
		public StiPdfObjInfo Mask;
	}

	public class StiPdfFontObjInfo : StiPdfObjInfo
	{
		public StiPdfObjInfo DescendantFont;

		public StiPdfObjInfo ToUnicode;

		public StiPdfObjInfo CIDSet;

		public StiPdfObjInfo Encoding;

		public StiPdfObjInfo FontDescriptor;

		public StiPdfObjInfo FontFile2;
	}

	public class StiPdfOutlinesObjInfo : StiPdfObjInfo
	{
		public List<StiPdfObjInfo> Items;
	}

	public class StiPdfPatternsObjInfo : StiPdfObjInfo
	{
		public StiPdfObjInfo Resources;

		public StiPdfObjInfo First;

		public List<StiPdfObjInfo> HatchItems;

		public List<StiPdfObjInfo> ShadingItems;

		public List<StiPdfObjInfo> ShadingFunctionItems;
	}

	public class StiPdfAnnotObjInfo : StiPdfObjInfo
	{
		public StiPdfObjInfo AP;

		public List<StiPdfObjInfo> AA;
	}

	public class StiPdfAcroFormObjInfo : StiPdfObjInfo
	{
		public List<StiPdfAnnotObjInfo> Annots;

		public List<List<StiPdfAnnotObjInfo>> CheckBoxes;

		public List<StiPdfAnnotObjInfo> UnsignedSignatures;

		public List<StiPdfAnnotObjInfo> Signatures;

		public List<StiPdfAnnotObjInfo> Tooltips;

		public List<StiPdfFontObjInfo> AnnotFontItems;
	}

	public StiPdfObjInfo Root;

	public StiPdfObjInfo Info;

	public StiPdfObjInfo ColorSpace;

	public StiPdfObjInfo Pages;

	public StiPdfObjInfo StructTreeRoot;

	public StiPdfObjInfo OptionalContentGroup;

	public List<StiPdfContentObjInfo> PageList;

	public List<StiPdfXObjectObjInfo> XObjectList;

	public List<StiPdfFontObjInfo> FontList;

	public StiPdfOutlinesObjInfo Outlines;

	public StiPdfPatternsObjInfo Patterns;

	public List<StiPdfObjInfo> LinkList;

	public StiPdfObjInfo Encode;

	public StiPdfObjInfo ExtGState;

	public StiPdfAcroFormObjInfo AcroForm;

	public StiPdfObjInfo Metadata;

	public StiPdfObjInfo DestOutputProfile;

	public StiPdfObjInfo OutputIntents;

	public StiPdfContentObjInfo EmbeddedJS;

	public List<StiPdfContentObjInfo> EmbeddedFilesList;

	private int objectsCounter;

	private ArrayList objects;

	public void AddRef(StiPdfObjInfo info)
	{
		objectsCounter++;
		info.Ref = objectsCounter;
	}

	public StiPdfObjInfo CreateObject(bool addRef = false)
	{
		StiPdfObjInfo stiPdfObjInfo = new StiPdfObjInfo();
		stiPdfObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfObjInfo);
		}
		return stiPdfObjInfo;
	}

	public StiPdfContentObjInfo CreateContentObject(bool addRef = false)
	{
		StiPdfContentObjInfo stiPdfContentObjInfo = new StiPdfContentObjInfo();
		stiPdfContentObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfContentObjInfo);
		}
		stiPdfContentObjInfo.Content = CreateObject(addRef);
		return stiPdfContentObjInfo;
	}

	public StiPdfXObjectObjInfo CreateXObject(bool addRef = false, bool haveMask = false)
	{
		StiPdfXObjectObjInfo stiPdfXObjectObjInfo = new StiPdfXObjectObjInfo();
		stiPdfXObjectObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfXObjectObjInfo);
		}
		stiPdfXObjectObjInfo.Mask = CreateObject(addRef && haveMask);
		return stiPdfXObjectObjInfo;
	}

	public StiPdfFontObjInfo CreateFontObject(bool addRef = false, bool useUnicodeMode = true, bool standardPdfFonts = false, bool embeddedFonts = true, bool annotFont = false)
	{
		StiPdfFontObjInfo stiPdfFontObjInfo = new StiPdfFontObjInfo();
		stiPdfFontObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfFontObjInfo);
		}
		stiPdfFontObjInfo.DescendantFont = CreateObject(addRef && useUnicodeMode);
		stiPdfFontObjInfo.ToUnicode = CreateObject(addRef && useUnicodeMode);
		stiPdfFontObjInfo.CIDSet = CreateObject(addRef && useUnicodeMode);
		stiPdfFontObjInfo.Encoding = CreateObject(addRef && !useUnicodeMode && !annotFont);
		stiPdfFontObjInfo.FontDescriptor = CreateObject(addRef && !standardPdfFonts);
		stiPdfFontObjInfo.FontFile2 = CreateObject(addRef && embeddedFonts);
		return stiPdfFontObjInfo;
	}

	public StiPdfOutlinesObjInfo CreateOutlinesObject(bool addRef = false)
	{
		StiPdfOutlinesObjInfo stiPdfOutlinesObjInfo = new StiPdfOutlinesObjInfo();
		stiPdfOutlinesObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfOutlinesObjInfo);
		}
		stiPdfOutlinesObjInfo.Items = new List<StiPdfObjInfo>();
		return stiPdfOutlinesObjInfo;
	}

	public StiPdfPatternsObjInfo CreatePatternsObject(bool addRef = false)
	{
		StiPdfPatternsObjInfo stiPdfPatternsObjInfo = new StiPdfPatternsObjInfo();
		stiPdfPatternsObjInfo.Info = this;
		stiPdfPatternsObjInfo.Resources = CreateObject(addRef);
		stiPdfPatternsObjInfo.First = CreateObject(addRef);
		stiPdfPatternsObjInfo.HatchItems = new List<StiPdfObjInfo>();
		stiPdfPatternsObjInfo.ShadingItems = new List<StiPdfObjInfo>();
		stiPdfPatternsObjInfo.ShadingFunctionItems = new List<StiPdfObjInfo>();
		return stiPdfPatternsObjInfo;
	}

	public StiPdfAcroFormObjInfo CreateAcroFormObject(bool addRef = false)
	{
		StiPdfAcroFormObjInfo stiPdfAcroFormObjInfo = new StiPdfAcroFormObjInfo();
		stiPdfAcroFormObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfAcroFormObjInfo);
		}
		stiPdfAcroFormObjInfo.Annots = new List<StiPdfAnnotObjInfo>();
		stiPdfAcroFormObjInfo.CheckBoxes = new List<List<StiPdfAnnotObjInfo>>();
		stiPdfAcroFormObjInfo.UnsignedSignatures = new List<StiPdfAnnotObjInfo>();
		stiPdfAcroFormObjInfo.Signatures = new List<StiPdfAnnotObjInfo>();
		stiPdfAcroFormObjInfo.Tooltips = new List<StiPdfAnnotObjInfo>();
		stiPdfAcroFormObjInfo.AnnotFontItems = new List<StiPdfFontObjInfo>();
		return stiPdfAcroFormObjInfo;
	}

	public StiPdfAnnotObjInfo CreateAnnotObject(bool addRef = false, bool createAP = false, int numberAA = 0)
	{
		StiPdfAnnotObjInfo stiPdfAnnotObjInfo = new StiPdfAnnotObjInfo();
		stiPdfAnnotObjInfo.Info = this;
		if (addRef)
		{
			AddRef(stiPdfAnnotObjInfo);
		}
		stiPdfAnnotObjInfo.AP = CreateObject(addRef && createAP);
		stiPdfAnnotObjInfo.AA = new List<StiPdfObjInfo>();
		for (int i = 0; i < numberAA; i++)
		{
			stiPdfAnnotObjInfo.AA.Add(CreateObject(addRef));
		}
		return stiPdfAnnotObjInfo;
	}

	public StiPdfStructure()
	{
		objectsCounter = 0;
		objects = new ArrayList();
		Root = CreateObject(addRef: true);
		Info = CreateObject(addRef: true);
		ColorSpace = CreateObject(addRef: true);
		Pages = CreateObject(addRef: true);
		StructTreeRoot = CreateObject(addRef: true);
		OptionalContentGroup = CreateObject(addRef: true);
		PageList = new List<StiPdfContentObjInfo>();
		XObjectList = new List<StiPdfXObjectObjInfo>();
		FontList = new List<StiPdfFontObjInfo>();
		LinkList = new List<StiPdfObjInfo>();
		EmbeddedFilesList = new List<StiPdfContentObjInfo>();
	}
}
