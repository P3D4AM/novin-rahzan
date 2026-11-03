using System;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Units;

public abstract class StiUnit
{
	public static StiCentimetersUnit Centimeters = new StiCentimetersUnit();

	public static StiHundredthsOfInchUnit HundredthsOfInch = new StiHundredthsOfInchUnit();

	public static StiInchesUnit Inches = new StiInchesUnit();

	public static StiMillimetersUnit Millimeters = new StiMillimetersUnit();

	public abstract double RollerStep { get; }

	public abstract double Factor { get; }

	public abstract string ShortName { get; }

	public static JObject SaveToJsonObject(StiUnit unit)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", unit.GetType().Name);
		return val;
	}

	public static StiUnit LoadFromJsonObject(JObject jObject)
	{
		return jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>() switch
		{
			"StiMillimetersUnit" => new StiMillimetersUnit(), 
			"StiInchesUnit" => new StiInchesUnit(), 
			"StiHundredthsOfInchUnit" => new StiHundredthsOfInchUnit(), 
			"StiCentimetersUnit" => new StiCentimetersUnit(), 
			_ => null, 
		};
	}

	public static StiUnit GetUnitFromReportUnit(StiReportUnitType reportUnit)
	{
		return reportUnit switch
		{
			StiReportUnitType.HundredthsOfInch => HundredthsOfInch, 
			StiReportUnitType.Inches => Inches, 
			StiReportUnitType.Millimeters => Millimeters, 
			_ => Centimeters, 
		};
	}

	public abstract double ConvertToHInches(double value);

	public abstract decimal ConvertToHInches(decimal value);

	public abstract double ConvertFromHInches(double value);

	public abstract decimal ConvertFromHInches(decimal value);

	public RectangleD ConvertToHInches(RectangleD rect)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return new RectangleD(ConvertToHInches(((RectangleD)(ref rect)).Left), ConvertToHInches(((RectangleD)(ref rect)).Top), ConvertToHInches(((RectangleD)(ref rect)).Width), ConvertToHInches(((RectangleD)(ref rect)).Height));
	}

	public RectangleM ConvertToHInches(RectangleM rect)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return new RectangleM(ConvertToHInches(((RectangleM)(ref rect)).Left), ConvertToHInches(((RectangleM)(ref rect)).Top), ConvertToHInches(((RectangleM)(ref rect)).Width), ConvertToHInches(((RectangleM)(ref rect)).Height));
	}

	public RectangleD ConvertFromHInches(RectangleD rect)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return new RectangleD(ConvertFromHInches(((RectangleD)(ref rect)).Left), ConvertFromHInches(((RectangleD)(ref rect)).Top), ConvertFromHInches(((RectangleD)(ref rect)).Width), ConvertFromHInches(((RectangleD)(ref rect)).Height));
	}

	public RectangleM ConvertFromHInches(RectangleM rect)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		return new RectangleM(ConvertFromHInches(((RectangleM)(ref rect)).Left), ConvertFromHInches(((RectangleM)(ref rect)).Top), ConvertFromHInches(((RectangleM)(ref rect)).Width), ConvertFromHInches(((RectangleM)(ref rect)).Height));
	}

	public SizeD ConvertToHInches(SizeD size)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new SizeD(ConvertToHInches(((SizeD)(ref size)).Width), ConvertToHInches(((SizeD)(ref size)).Height));
	}

	public SizeM ConvertToHInches(SizeM size)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new SizeM(ConvertToHInches(((SizeM)(ref size)).Width), ConvertToHInches(((SizeM)(ref size)).Height));
	}

	public SizeD ConvertFromHInches(SizeD size)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new SizeD(ConvertFromHInches(((SizeD)(ref size)).Width), ConvertFromHInches(((SizeD)(ref size)).Height));
	}

	public SizeM ConvertFromHInches(SizeM size)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new SizeM(ConvertFromHInches(((SizeM)(ref size)).Width), ConvertFromHInches(((SizeM)(ref size)).Height));
	}
}
