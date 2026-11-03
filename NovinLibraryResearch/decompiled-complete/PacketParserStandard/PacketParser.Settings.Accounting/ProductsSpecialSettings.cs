using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Settings.Accounting;

public class ProductsSpecialSettings
{
	private List<int> productsIds;

	private List<Guid> productsGuids;

	public List<int> PIDs
	{
		get
		{
			return productsIds?.ToList();
		}
		set
		{
			productsIds = value;
		}
	}

	public List<int> ProductsIds
	{
		get
		{
			return productsIds?.ToList();
		}
		set
		{
			productsIds = value;
		}
	}

	public List<Guid> ProductsGuids
	{
		get
		{
			return productsGuids?.ToList();
		}
		set
		{
			productsGuids = value;
		}
	}

	public List<Guid> PGs
	{
		get
		{
			return productsGuids?.ToList();
		}
		set
		{
			productsGuids = value;
		}
	}

	public string ListName { get; set; } = "";


	public string LN
	{
		get
		{
			return ListName;
		}
		set
		{
			ListName = value;
		}
	}

	public int Level { get; set; }

	public int Lev
	{
		get
		{
			return Level;
		}
		set
		{
			Level = value;
		}
	}
}
