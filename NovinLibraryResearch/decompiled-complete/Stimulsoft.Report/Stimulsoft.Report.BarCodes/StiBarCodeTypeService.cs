using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiBarCodeTypeServiceConverter))]
public abstract class StiBarCodeTypeService : StiService, IStiPropertyGridObject, IStiJsonReportObject
{
	protected class StiBarCodeData
	{
		public float SpaceLeft;

		public float SpaceRight;

		public float SpaceTop;

		public float SpaceBottom;

		public float LineHeightShort;

		public float LineHeightLong;

		public float LineWidth;

		public float TextPosition;

		public float TextHeight;

		public float MainHeight;

		public float MainWidth;

		public float WideToNarrowRatio;

		public string Code;

		public string TextString;

		public string BarsArray;

		public float FullZoomY;

		public float SpaceBeforeAdd;

		public float SpaceTextTop;

		public float TextPositionTop;

		public float TextPositionBottom;

		public List<StiEAN13BarCodeType.EanBarInfo> EanBarsArray;

		public float OffsetY;

		public byte[] MatrixGrid;

		public int MatrixWidth;

		public int MatrixHeight;

		public int MatrixRatioY;
	}

	internal enum BarcodeCommandCode
	{
		Fnc1 = 256,
		Fnc2 = 512,
		Fnc3 = 768,
		Fnc4 = 1024
	}

	protected delegate void DrawBaseLinesDelegate(object context, StiBrush foreBrush);

	public const int visiblePropertiesCount = 27;

	private StiBarCodeData barCodeData = new StiBarCodeData();

	protected RectangleF RectWindow = new RectangleF(0f, 0f, 0f, 0f);

	protected const float DefaultLabelFontHeight = 8f;

	[Browsable(false)]
	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public sealed override string ServiceCategory => "BarCodeTypes";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiBarCodeTypeService);

	[Browsable(false)]
	public abstract bool[] VisibleProperties { get; }

	[Browsable(false)]
	public abstract string DefaultCodeValue { get; }

	[Browsable(false)]
	public IStiBarCodePainter CustomPainter { get; set; }

	[Browsable(false)]
	public float MainWidth => barCodeData.MainWidth;

	[Browsable(false)]
	public float MainHeight => barCodeData.MainHeight;

	protected StiBarCodeData BarCodeData => barCodeData;

	[StiSerializable]
	public abstract float Module { get; set; }

	[StiSerializable]
	public abstract float Height { get; set; }

	protected virtual StringAlignment TextAlignment => (StringAlignment)1;

	protected virtual bool TextSpacing => true;

	internal abstract float LabelFontHeight { get; }

	internal static StiBarCodeTypeService CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiBarCodeTypeService stiBarCodeTypeService = StiOptions.Services.BarCodes.FirstOrDefault((StiBarCodeTypeService x) => ((object)x).GetType().Name == ident);
		if (stiBarCodeTypeService == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		StiBarCodeTypeService stiBarCodeTypeService2 = stiBarCodeTypeService.CreateNew();
		stiBarCodeTypeService2.LoadFromJsonObject(jObject);
		return stiBarCodeTypeService2;
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		return val;
	}

	public abstract void LoadFromJsonObject(JObject jObject);

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public virtual StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	protected string CheckCodeSymbols(string inputCode, string tolerantSymbols)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (inputCode != null)
		{
			foreach (char value in inputCode)
			{
				if (tolerantSymbols.IndexOf(value) != -1)
				{
					stringBuilder.Append(value);
				}
			}
		}
		return stringBuilder.ToString();
	}

	public virtual string GetCode(IStiBarCode barCode)
	{
		return barCode.GetBarCodeString();
	}

	public virtual string GetCombinedCode()
	{
		return null;
	}

	internal static int[] UnpackTilde(byte[] input, bool processTilde)
	{
		int num = 0;
		List<int> list = new List<int>();
		while (num < input.Length)
		{
			int num2 = input[num++];
			if (processTilde && num2 == 126)
			{
				bool flag = false;
				if (num < input.Length && input[num] == 126)
				{
					list.Add(126);
					flag = true;
					num++;
				}
				if (!flag && num + 2 < input.Length)
				{
					if (input[num] == 70 && input[num + 1] == 78 && input[num + 2] == 67 && num + 3 < input.Length)
					{
						int num3 = input[num + 3] - 48;
						if (num3 == 1 || num3 == 2 || num3 == 3 || num3 == 4)
						{
							if (num3 == 1)
							{
								list.Add(256);
							}
							if (num3 == 2)
							{
								list.Add(512);
							}
							if (num3 == 3)
							{
								list.Add(768);
							}
							if (num3 == 4)
							{
								list.Add(1024);
							}
							flag = true;
							num += 4;
						}
					}
					if (!flag && char.IsDigit((char)input[num]) && char.IsDigit((char)input[num + 1]) && char.IsDigit((char)input[num + 2]))
					{
						int num4 = (input[num] - 48) * 100 + (input[num + 1] - 48) * 10 + (input[num + 2] - 48);
						if (num4 >= 0 && num4 <= 255)
						{
							list.Add(num4);
							flag = true;
							num += 3;
						}
					}
				}
				if (flag)
				{
					continue;
				}
			}
			list.Add(num2);
		}
		return list.ToArray();
	}

	protected virtual float GetSymbolWidth(char symbol)
	{
		switch (symbol)
		{
		case '0':
		case '4':
		case '8':
		case 'c':
		case 'd':
		case 'e':
		case 'f':
			return 1f;
		case '1':
		case '5':
		case '9':
			return 1f * BarCodeData.WideToNarrowRatio;
		case '2':
		case '6':
		case 'a':
			return 1.5f * BarCodeData.WideToNarrowRatio;
		case '3':
		case '7':
		case 'b':
			return 2f * BarCodeData.WideToNarrowRatio;
		default:
			return 1f;
		}
	}

	protected bool IsSymbolLong(char symbol)
	{
		switch (symbol)
		{
		case '8':
		case '9':
		case 'a':
		case 'b':
		case 'c':
		case 'e':
			return true;
		default:
			return false;
		}
	}

	protected virtual bool IsSymbolSpace(char symbol)
	{
		switch (symbol)
		{
		case '0':
		case '1':
		case '2':
		case '3':
			return true;
		default:
			return false;
		}
	}

	protected bool IsSymbolPostDescend(char symbol)
	{
		if (symbol == 'e' || symbol == 'f')
		{
			return true;
		}
		return false;
	}

	protected float GetSymbolsStringWidth(string symbolsString)
	{
		float num = 0f;
		for (int i = 0; i < symbolsString.Length; i++)
		{
			num += GetSymbolWidth(symbolsString[i]);
		}
		return num;
	}

	protected void DrawBars(object context, string sym, StiBrush foreBrush)
	{
		float spaceLeft = BarCodeData.SpaceLeft;
		float spaceTop = BarCodeData.SpaceTop;
		float num = 0f;
		foreach (char symbol in sym)
		{
			if (!IsSymbolSpace(symbol))
			{
				float num2 = BarCodeData.LineHeightShort;
				if (IsSymbolLong(symbol))
				{
					num2 = BarCodeData.LineHeightLong;
				}
				float num3 = 0f;
				if (IsSymbolPostDescend(symbol))
				{
					num3 = BarCodeData.LineHeightLong - BarCodeData.LineHeightShort;
				}
				BaseFillRectangle(context, foreBrush, spaceLeft + BarCodeData.LineWidth * num, spaceTop + num3, BarCodeData.LineWidth * GetSymbolWidth(symbol), num2 - num3);
			}
			num += GetSymbolWidth(symbol);
		}
	}

	protected void DrawBarCode(object context, RectangleF rect, StiBarCode barCode, DrawBaseLinesDelegate drawMethod = null)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Invalid comparison between Unknown and I4
		bool flag = context is Graphics;
		float num = (flag ? 1f : 0.75000006f);
		Font val = new Font(StiFontCollection.GetFontFamily(barCode.Font.Name, false), barCode.Font.Size * (barCode.BarCodeType.LabelFontHeight / 8f) * BarCodeData.FullZoomY * num, barCode.Font.Style, (GraphicsUnit)(flag ? 2 : 3));
		try
		{
			TranslateRect(context, rect, barCode);
			StiSolidBrush val2 = new StiSolidBrush(barCode.ForeColor);
			StiSolidBrush brush = new StiSolidBrush(barCode.BackColor);
			BaseFillRectangle(context, (StiBrush)(object)brush, 0f, 0f, BarCodeData.MainWidth, BarCodeData.MainHeight);
			DrawBars(context, BarCodeData.BarsArray, (StiBrush)(object)val2);
			if (barCode.ShowLabelText && BarCodeData.TextString.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (TextSpacing)
				{
					for (int i = 0; i < BarCodeData.TextString.Length; i++)
					{
						stringBuilder.Append(BarCodeData.TextString[i]);
						stringBuilder.Append(' ');
					}
					stringBuilder.Length--;
				}
				else
				{
					stringBuilder.Append(BarCodeData.TextString);
				}
				string st = stringBuilder.ToString();
				SizeF sizeF = BaseMeasureString(context, st, val);
				float symbolsStringWidth = GetSymbolsStringWidth(BarCodeData.BarsArray);
				if (sizeF.Width > symbolsStringWidth * BarCodeData.LineWidth + BarCodeData.SpaceLeft + BarCodeData.SpaceRight)
				{
					st = BarCodeData.TextString;
				}
				float x = 0f;
				float textPosition = BarCodeData.TextPosition;
				StringFormat val3 = new StringFormat();
				try
				{
					val3.Alignment = TextAlignment;
					val3.FormatFlags = (StringFormatFlags)0;
					if ((int)TextAlignment == 1)
					{
						BaseDrawString(context, st, val, (StiBrush)(object)val2, new RectangleF(x, textPosition, symbolsStringWidth * BarCodeData.LineWidth + BarCodeData.SpaceLeft + BarCodeData.SpaceRight, BarCodeData.TextHeight * 2f), val3);
					}
					else
					{
						BaseDrawString(context, BarCodeData.TextString, val, (StiBrush)(object)val2, new RectangleF(BarCodeData.SpaceLeft, textPosition, symbolsStringWidth * BarCodeData.LineWidth, BarCodeData.TextHeight * 2f), val3);
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			drawMethod?.Invoke(context, (StiBrush)(object)val2);
			RollbackTransform(context);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected void CalculateSizeFull(float SpaceLeft, float SpaceRight, float SpaceTop, float SpaceBottom, float LineHeightShort, float LineHeightLong, float TextPosition, float TextHeight, float MainHeight, float LineHeightForCut, float WideToNarrowRatio, float Zoom, string Code, string TextString, string BarsArray, RectangleF rect, StiBarCode barCode)
	{
		BarCodeData.WideToNarrowRatio = WideToNarrowRatio;
		BarCodeData.Code = Code;
		BarCodeData.TextString = TextString;
		BarCodeData.BarsArray = BarsArray;
		if (!barCode.ShowQuietZones && !(this is StiAustraliaPost4StateBarCodeType) && !(this is StiIntelligentMail4StateBarCodeType) && !(this is StiITF14BarCodeType))
		{
			SpaceLeft = 0f;
			SpaceRight = 0f;
			SpaceTop = 0f;
			SpaceBottom = 0f;
		}
		RectWindow = new RectangleF(0f, 0f, rect.Width, rect.Height);
		if (barCode.Angle == StiAngle.Angle90 || barCode.Angle == StiAngle.Angle270)
		{
			RectWindow = new RectangleF(0f, 0f, rect.Height, rect.Width);
		}
		float num = barCode.Font.Size - 8f + 0.5f;
		MainHeight += num;
		TextHeight += num;
		float num2 = Module / 10f * Zoom;
		float num3 = num2;
		float num4 = LineHeightForCut * (1f - Height);
		if (barCode.AutoScale)
		{
			num2 = RectWindow.Width / (GetSymbolsStringWidth(BarsArray) + SpaceLeft + SpaceRight);
			num4 = 0f - (RectWindow.Height / num3 - MainHeight);
			if (!barCode.ShowLabelText && TextPosition > LineHeightForCut)
			{
				num4 -= TextHeight;
			}
		}
		BarCodeData.FullZoomY = num3;
		BarCodeData.SpaceLeft = SpaceLeft * num2;
		BarCodeData.SpaceRight = SpaceRight * num2;
		BarCodeData.LineWidth = 1f * num2;
		BarCodeData.MainWidth = (GetSymbolsStringWidth(BarsArray) + SpaceLeft + SpaceRight) * num2;
		BarCodeData.SpaceTop = SpaceTop * num3;
		BarCodeData.SpaceBottom = SpaceBottom * num3;
		BarCodeData.LineHeightShort = (LineHeightShort - num4) * num3;
		BarCodeData.LineHeightLong = (LineHeightLong - num4) * num3;
		if (TextPosition > SpaceTop)
		{
			TextPosition -= num4;
		}
		BarCodeData.TextPosition = TextPosition * num3;
		BarCodeData.MainHeight = (MainHeight - num4) * num3;
		BarCodeData.TextHeight = TextHeight * num3;
	}

	protected void CalculateSize2(float SpaceLeft, float SpaceRight, float SpaceTop, float SpaceBottom, float LineHeightShort, float LineHeightLong, float TextPosition, float TextHeight, float MainHeight, float WideToNarrowRatio, float Zoom, string BarsArray, RectangleF rect, StiBarCode barCode)
	{
		CalculateSizeFull(SpaceLeft, SpaceRight, SpaceTop, SpaceBottom, LineHeightShort, LineHeightLong, TextPosition, TextHeight, MainHeight, LineHeightShort, WideToNarrowRatio, Zoom, "", "", BarsArray, rect, barCode);
	}

	protected void Draw2DBarCode(object context, RectangleF rect, StiBarCode barCode, float zoom)
	{
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		RectWindow = new RectangleF(0f, 0f, rect.Width, rect.Height);
		if (barCode.Angle == StiAngle.Angle90 || barCode.Angle == StiAngle.Angle270)
		{
			RectWindow = new RectangleF(0f, 0f, rect.Height, rect.Width);
		}
		int num = 2;
		if (!barCode.ShowQuietZones)
		{
			num = 0;
		}
		float num2 = Module / 10f * zoom;
		float num3 = num2;
		if (barCode.AutoScale)
		{
			num2 = RectWindow.Width / (float)(barCodeData.MatrixWidth + num * 2);
			num3 = RectWindow.Height / (float)(barCodeData.MatrixHeight * barCodeData.MatrixRatioY + num * 2);
			num2 = Math.Min(num2, num3);
			num3 = num2;
		}
		BarCodeData.MainWidth = (float)(barCodeData.MatrixWidth + num * 2) * num2;
		BarCodeData.MainHeight = (float)(barCodeData.MatrixHeight * barCodeData.MatrixRatioY + num * 2) * num3;
		BarCodeData.SpaceLeft = (float)num * num2;
		BarCodeData.SpaceTop = (float)num * num3;
		TranslateRect(context, rect, barCode);
		StiSolidBrush brush = new StiSolidBrush(barCode.ForeColor);
		StiSolidBrush brush2 = new StiSolidBrush(barCode.BackColor);
		BaseFillRectangle(context, (StiBrush)(object)brush2, 0f, 0f, BarCodeData.MainWidth, BarCodeData.MainHeight);
		float spaceLeft = BarCodeData.SpaceLeft;
		float spaceTop = BarCodeData.SpaceTop;
		for (int i = 0; i < barCodeData.MatrixHeight; i++)
		{
			for (int j = 0; j < barCodeData.MatrixWidth; j++)
			{
				if (barCodeData.MatrixGrid[j + i * barCodeData.MatrixWidth] != 0)
				{
					BaseFillRectangle2D(context, (StiBrush)(object)brush, spaceLeft + (float)j * num2, spaceTop + (float)i * num3 * (float)barCodeData.MatrixRatioY, num2, num3 * (float)barCodeData.MatrixRatioY);
				}
			}
		}
		if (this is StiQRCodeBarCodeType { Image: not null } stiQRCodeBarCodeType)
		{
			float num4 = (float)stiQRCodeBarCodeType.Image.Width * zoom * (float)stiQRCodeBarCodeType.ImageMultipleFactor;
			float num5 = (float)stiQRCodeBarCodeType.Image.Height * zoom * (float)stiQRCodeBarCodeType.ImageMultipleFactor;
			float x = spaceLeft + ((float)barCodeData.MatrixWidth * num2 - num4) / 2f;
			float y = spaceTop + ((float)barCodeData.MatrixHeight * num3 - num5) / 2f;
			BaseDrawImage(context, stiQRCodeBarCodeType.Image, barCode.Report, x, y, num4, num5);
		}
		RollbackTransform(context);
	}

	protected void DrawMaxicode(object context, RectangleF rect, StiBarCode barCode, float zoom)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		RectWindow = new RectangleF(0f, 0f, rect.Width, rect.Height);
		if (barCode.Angle == StiAngle.Angle90 || barCode.Angle == StiAngle.Angle270)
		{
			RectWindow = new RectangleF(0f, 0f, rect.Height, rect.Width);
		}
		float num = 100f;
		float num2 = num / 29f;
		float num3 = num2 * 1.1547f;
		float num4 = num2 * 0.866f;
		float num5 = num2 * 1.5f;
		float num6 = num4 + num3 / 2f;
		float num7 = zoom;
		float num8 = num2 * 32f;
		float num9 = num4 * 34f + num3;
		if (barCode.AutoScale)
		{
			num7 = Math.Min(RectWindow.Height, RectWindow.Width) / num8;
		}
		BarCodeData.MainWidth = num8 * num7;
		BarCodeData.MainHeight = num9 * num7;
		TranslateRect(context, rect, barCode);
		StiSolidBrush val = new StiSolidBrush(barCode.ForeColor);
		StiSolidBrush val2 = new StiSolidBrush((barCode.BackColor.A < 64) ? Color.White : barCode.BackColor);
		BaseFillRectangle(context, (StiBrush)(object)val2, 0f, 0f, BarCodeData.MainWidth, BarCodeData.MainHeight);
		float[] array = new float[6]
		{
			0f,
			num2 / 2f,
			num2 / 2f,
			0f,
			(0f - num2) / 2f,
			(0f - num2) / 2f
		};
		float[] array2 = new float[6]
		{
			num3 / 2f,
			num3 / 4f,
			(0f - num3) / 4f,
			(0f - num3) / 2f,
			(0f - num3) / 4f,
			num3 / 4f
		};
		float num10 = 0.87f;
		for (int i = 0; i < 33; i++)
		{
			for (int j = 0; j < 30; j++)
			{
				if (barCodeData.MatrixGrid[i * 30 + j] == 1)
				{
					float num11 = num5 + (float)j * num2;
					if (((uint)i & (true ? 1u : 0u)) != 0)
					{
						num11 += num2 / 2f;
					}
					float num12 = num6 + num4 * (float)i;
					PointF[] array3 = new PointF[6];
					for (int k = 0; k < 6; k++)
					{
						array3[k] = new PointF((num11 + array[k] * num10) * num7, (num12 + array2[k] * num10) * num7);
					}
					BaseFillPolygon(context, (StiBrush)(object)val, array3);
				}
			}
		}
		float num13 = num5 + num2 * 14f;
		float num14 = num6 + num4 * 16f;
		float[] array4 = new float[6] { 15.236f, 12.598f, 9.96f, 7.32f, 4.646f, 2.008f };
		for (int l = 0; l < array4.Length; l++)
		{
			RectangleF rectangleF = new RectangleF((num13 - array4[l]) * num7, (num14 - array4[l]) * num7, array4[l] * 2f * num7, array4[l] * 2f * num7);
			BaseFillEllipse(context, (StiBrush)(object)(((l & 1) > 0) ? val2 : val), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		RollbackTransform(context);
	}

	protected void DrawBarCodeError(object context, RectangleF rect, StiBarCode barCode)
	{
		DrawBarCodeError(context, rect, barCode, null);
	}

	protected void DrawBarCodeError(object context, RectangleF rect, StiBarCode barCode, string message)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Expected O, but got Unknown
		StiSolidBrush brush = new StiSolidBrush(barCode.BackColor);
		BaseFillRectangle(context, (StiBrush)(object)brush, rect.X, rect.Y, rect.Width, rect.Height);
		BaseDrawRectangle(context, Color.Red, 4f, rect.X, rect.Y, rect.Width, rect.Height);
		Font val = new Font("Arial", 8f);
		try
		{
			if (string.IsNullOrEmpty(message))
			{
				BaseDrawString(context, "Not valid data", val, (StiBrush)new StiSolidBrush(Color.Red), new RectangleF(rect.X + 1f, rect.Y + 1f, rect.Width - 2f, rect.Height - 2f), new StringFormat());
			}
			else
			{
				BaseDrawString(context, message, val, (StiBrush)new StiSolidBrush(Color.Red), new RectangleF(rect.X + 1f, rect.Y + 1f, rect.Width - 2f, rect.Height - 2f), new StringFormat());
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public abstract void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom);

	protected void TranslateRect(object context, RectangleF rect, StiBarCode barCode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		StiHorAlignment horAlignment = barCode.HorAlignment;
		float dx = (((int)horAlignment == 2) ? ((RectWindow.Width - BarCodeData.MainWidth) / 2f) : (((int)horAlignment != 3) ? 0f : (RectWindow.Width - BarCodeData.MainWidth)));
		StiVertAlignment vertAlignment = barCode.VertAlignment;
		float dy = (((int)vertAlignment == 1) ? ((RectWindow.Height - BarCodeData.MainHeight) / 2f) : (((int)vertAlignment != 2) ? 0f : (RectWindow.Height - BarCodeData.MainHeight)));
		float angle;
		float x;
		float y;
		switch (barCode.Angle)
		{
		case StiAngle.Angle90:
			angle = -90f;
			x = rect.X;
			y = rect.Y + rect.Height;
			break;
		case StiAngle.Angle180:
			angle = -180f;
			x = rect.X + rect.Width;
			y = rect.Y + rect.Height;
			break;
		case StiAngle.Angle270:
			angle = -270f;
			x = rect.X + rect.Width;
			y = rect.Y;
			break;
		default:
			angle = 0f;
			x = rect.X;
			y = rect.Y;
			break;
		}
		BaseTransform(context, x, y, angle, dx, dy);
	}

	protected void RollbackTransform(object context)
	{
		BaseRollbackTransform(context);
	}

	protected void BaseDrawString(object context, string st, Font font, StiBrush brush, float x, float y)
	{
		BaseDrawString(context, st, font, brush, new RectangleF(x, y, 0f, 0f), null);
	}

	protected virtual void BaseTransform(object context, float x, float y, float angle, float dx, float dy)
	{
		IStiBarCodePainter stiBarCodePainter = null;
		stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseTransform(context, x, y, angle, dx, dy);
	}

	protected virtual void BaseRollbackTransform(object context)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseRollbackTransform(context);
	}

	protected virtual void BaseFillRectangle(object context, StiBrush brush, float x, float y, float width, float height)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseFillRectangle(context, brush, x, y, width, height);
	}

	protected virtual void BaseFillRectangle2D(object context, StiBrush brush, float x, float y, float width, float height)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseFillRectangle2D(context, brush, x, y, width, height);
	}

	protected virtual void BaseFillPolygon(object context, StiBrush brush, PointF[] points)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseFillPolygon(context, brush, points);
	}

	protected virtual void BaseFillEllipse(object context, StiBrush brush, float x, float y, float width, float height)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseFillEllipse(context, brush, x, y, width, height);
	}

	protected virtual void BaseDrawRectangle(object context, Color penColor, float penSize, float x, float y, float width, float height)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseDrawRectangle(context, penColor, penSize, x, y, width, height);
	}

	protected virtual void BaseDrawImage(object context, Image image, StiReport report, float x, float y, float width, float height)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseDrawImage(context, image, report, x, y, width, height);
	}

	protected virtual void BaseDrawString(object context, string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		stiBarCodePainter.BaseDrawString(context, st, font, brush, rect, sf);
	}

	protected virtual SizeF BaseMeasureString(object context, string st, Font font)
	{
		IStiBarCodePainter stiBarCodePainter = ((context is StiBarCodeExportPainter) ? (context as StiBarCodeExportPainter) : ((CustomPainter == null) ? (StiPainter.GetPainter(typeof(StiBarCode), (StiGuiMode)(!(context is Graphics))) as IStiBarCodePainter) : CustomPainter));
		return stiBarCodePainter.BaseMeasureString(context, st, font);
	}

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public virtual StiBarCodeTypeService CreateNew()
	{
		throw new NotImplementedException();
	}
}
