using System;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiVariableTypeCheck : StiVariableCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckVariable", "StiVariableTypeCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckVariable", "StiVariableTypeCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private Type GetSimplifiedType(Type type)
	{
		if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(sbyte?) || type == typeof(byte?) || type == typeof(short?) || type == typeof(ushort?) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(ByteList) || type == typeof(ShortList) || type == typeof(IntList) || type == typeof(LongList))
		{
			type = typeof(long);
		}
		else if (type == typeof(double) || type == typeof(float) || type == typeof(double?) || type == typeof(float?) || type == typeof(DoubleList) || type == typeof(FloatList))
		{
			type = typeof(double);
		}
		else if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(DecimalList))
		{
			type = typeof(decimal);
		}
		else if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeList))
		{
			type = typeof(DateTime);
		}
		else if (type == typeof(bool) || type == typeof(bool?) || type == typeof(BoolList))
		{
			type = typeof(bool);
		}
		else if (type == typeof(char) || type == typeof(char?) || type == typeof(CharList))
		{
			type = typeof(char);
		}
		else if (type == typeof(string) || type == typeof(StringList))
		{
			type = typeof(string);
		}
		return type;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiVariable { DialogInfo: not null } stiVariable && stiVariable.DialogInfo.BindingValue && stiVariable.DialogInfo.BindingVariable != null)
		{
			StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(report.Dictionary, stiVariable.DialogInfo.KeysColumn);
			StiDataColumn dataColumnFromColumnName2 = StiDataColumn.GetDataColumnFromColumnName(report.Dictionary, stiVariable.DialogInfo.BindingValuesColumn, allowRelationName: true);
			if (dataColumnFromColumnName != null && dataColumnFromColumnName2 != null && (!GetSimplifiedType(dataColumnFromColumnName.Type).Equals(GetSimplifiedType(stiVariable.Type)) || !GetSimplifiedType(dataColumnFromColumnName2.Type).Equals(GetSimplifiedType(stiVariable.Type)) || !GetSimplifiedType(stiVariable.DialogInfo.BindingVariable.Type).Equals(GetSimplifiedType(stiVariable.Type))))
			{
				StiVariableTypeCheck stiVariableTypeCheck = new StiVariableTypeCheck();
				stiVariableTypeCheck.Element = obj;
				stiVariableTypeCheck.Actions.Add(new StiEditPropertyAction());
				return stiVariableTypeCheck;
			}
		}
		return null;
	}
}
