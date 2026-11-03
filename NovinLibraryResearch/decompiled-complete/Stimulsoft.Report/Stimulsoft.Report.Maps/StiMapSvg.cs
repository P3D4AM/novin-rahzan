using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Maps;

public class StiMapSvg
{
	public string Key { get; set; }

	public string Data { get; set; }

	public string EnglishName { get; set; }

	public string ISOCode { get; set; }

	[DefaultValue(null)]
	public double? ShowHiddenTextIfZoom { get; set; }

	public Rectangle Rect { get; set; }

	[DefaultValue(false)]
	public bool SetMaxWidth { get; set; }

	[DefaultValue(false)]
	public bool SkipText { get; set; }

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiTextHorAlignment HorAlignment { get; set; } = (StiTextHorAlignment)1;


	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiVertAlignment VertAlignment { get; set; } = (StiVertAlignment)1;


	public Rectangle RectIso { get; set; }

	[DefaultValue(null)]
	public bool? SkipTextIso { get; set; }

	[DefaultValue(null)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiTextHorAlignment? HorAlignmentIso { get; set; }

	[DefaultValue(null)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiVertAlignment? VertAlignmentIso { get; set; }

	public override string ToString()
	{
		return Key + " " + ISOCode;
	}
}
