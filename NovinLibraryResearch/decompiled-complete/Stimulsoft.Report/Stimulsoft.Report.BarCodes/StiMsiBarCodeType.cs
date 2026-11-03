using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiMsiBarCodeType : StiPlesseyBarCodeType
{
	private string[] MsiTable = new string[16]
	{
		"0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001",
		"1010", "1011", "1100", "1101", "1110", "1111"
	};

	private string MsiStartCode = "1";

	private string MsiStopCode = "00";

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiMsiBarCodeType;

	public override string ServiceName => "Msi";

	public override string DefaultCodeValue => "1234567";

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, true, true, false, false, false, false,
		false, true, false, true, false, false, false, false, false, false,
		false, false, false, false, false, false, false
	};

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.CheckSum1(),
			propertiesHelper.CheckSum2(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected string CodeToBarMsi(string inputCode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < inputCode.Length; i++)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			if (inputCode[i] == '1')
			{
				stringBuilder2.Append("71");
			}
			else
			{
				stringBuilder2.Append("53");
			}
			stringBuilder.Append((object?)stringBuilder2);
		}
		return stringBuilder.ToString();
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, PlesseySymbols);
		StringBuilder stringBuilder = new StringBuilder(code);
		int num = code.Length;
		if (base.CheckSum1 != 0)
		{
			num++;
		}
		if (base.CheckSum2 != 0)
		{
			num++;
		}
		int[] array = new int[num];
		for (int i = 0; i < code.Length; i++)
		{
			array[i] = PlesseySymbols.IndexOf(code[i]);
		}
		if (base.CheckSum1 != 0)
		{
			int num2 = 0;
			if (base.CheckSum1 == StiPlesseyCheckSum.Modulo10)
			{
				long num3 = 0L;
				long num4 = 0L;
				long num5 = 1L;
				bool flag = false;
				for (int num6 = code.Length - 1; num6 >= 0; num6--)
				{
					if (!flag)
					{
						num3 += array[num6] * num5;
						num5 *= 10;
					}
					else
					{
						num4 += array[num6];
					}
					flag = !flag;
				}
				num3 *= 2;
				long num7 = 0L;
				while (num3 >= 10)
				{
					num7 += num3 % 10;
					num3 /= 10;
				}
				num7 += num3;
				num2 = (int)((num7 + num4) % 10);
				if (num2 != 0)
				{
					num2 = 10 - num2;
				}
			}
			if (base.CheckSum1 == StiPlesseyCheckSum.Modulo11)
			{
				int num8 = 0;
				int num9 = 2;
				for (int num10 = code.Length - 1; num10 >= 0; num10--)
				{
					num8 += array[num10] * num9;
					num9++;
					if (num9 > 7)
					{
						num9 = 2;
					}
				}
				num2 = num8 % 11;
				if (num2 != 0)
				{
					num2 = 11 - num2;
				}
			}
			array[code.Length] = num2;
			stringBuilder.Append(PlesseySymbols[num2]);
		}
		if (base.CheckSum2 != 0)
		{
			int num11 = 0;
			if (base.CheckSum2 == StiPlesseyCheckSum.Modulo10)
			{
				long num12 = 0L;
				long num13 = 0L;
				long num14 = 1L;
				bool flag2 = false;
				for (int num15 = code.Length; num15 >= 0; num15--)
				{
					if (!flag2)
					{
						num12 += array[num15] * num14;
						num14 *= 10;
					}
					else
					{
						num13 += array[num15];
					}
					flag2 = !flag2;
				}
				num12 *= 2;
				long num16 = 0L;
				while (num12 >= 10)
				{
					num16 += num12 % 10;
					num12 /= 10;
				}
				num16 += num12;
				num11 = (int)((num16 + num13) % 10);
				if (num11 != 0)
				{
					num11 = 10 - num11;
				}
			}
			if (base.CheckSum2 == StiPlesseyCheckSum.Modulo11)
			{
				int num17 = 0;
				int num18 = 2;
				for (int num19 = code.Length; num19 >= 0; num19--)
				{
					num17 += array[num19] * num18;
					num18++;
					if (num18 > 7)
					{
						num18 = 2;
					}
				}
				num11 = num17 % 11;
				if (num11 != 0)
				{
					num11 = 11 - num11;
				}
			}
			array[code.Length + 1] = num11;
			stringBuilder.Append(PlesseySymbols[num11]);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(MsiStartCode);
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder2.Append(MsiTable[array[j]]);
		}
		stringBuilder2.Append(MsiStopCode);
		StringBuilder stringBuilder3 = new StringBuilder(CodeToBarMsi(stringBuilder2.ToString()));
		CalculateSizeFull(20f, 20f, 0f, 1f, 70f, 70f, 71f, 11.5f, 84f, 70f, 1.667f, zoom, code, stringBuilder.ToString(), stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiMsiBarCodeType();
	}

	public StiMsiBarCodeType()
		: this(8f, 1f, StiPlesseyCheckSum.Modulo10, StiPlesseyCheckSum.None)
	{
	}

	public StiMsiBarCodeType(float module, float height, StiPlesseyCheckSum checkSum1, StiPlesseyCheckSum checkSum2)
		: base(module, height, checkSum1, checkSum2)
	{
	}
}
