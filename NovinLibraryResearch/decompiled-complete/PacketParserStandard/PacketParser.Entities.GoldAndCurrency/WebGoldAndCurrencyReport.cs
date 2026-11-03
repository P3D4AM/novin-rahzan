using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PacketParser.Entities.GoldAndCurrency;

public class WebGoldAndCurrencyReport
{
	public string Namad { get; set; }

	public string Sign { get; set; }

	public string PersianName { get; set; }

	public DateTime InsertDate { get; set; }

	public decimal Price { get; set; }

	public string Icon { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string DateSh => InsertDate.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public bool IsChecked { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string Time => $"{InsertDate.Hour}:{InsertDate.Minute}:{InsertDate.Second}";
}
