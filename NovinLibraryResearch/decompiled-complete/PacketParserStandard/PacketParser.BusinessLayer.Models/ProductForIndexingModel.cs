using System;
using System.Collections.Generic;

namespace PacketParser.BusinessLayer.Models;

public class ProductForIndexingModel
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string NameForDisplay { get; set; }

	public string Description { get; set; }

	public string IdProduct { get; set; }

	public string T1 { get; set; }

	public string T2 { get; set; }

	public string T3 { get; set; }

	public string T4 { get; set; }

	public string T5 { get; set; }

	public string T6 { get; set; }

	public string T7 { get; set; }

	public string T8 { get; set; }

	public List<string> FullTextIndexOnAllFields()
	{
		return (Name + " " + NameForDisplay + " " + Description + " " + IdProduct + "  " + T1 + " " + T2 + " " + T3 + " " + T4 + " " + T5 + " " + T6 + " " + T7 + " " + T8).PrepareForIndexing();
	}

	public List<string> FullTextIndexOnName_CodeFields()
	{
		return (Name + " " + NameForDisplay + " " + IdProduct).PrepareForIndexing();
	}
}
