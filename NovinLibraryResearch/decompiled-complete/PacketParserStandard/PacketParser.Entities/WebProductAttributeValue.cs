using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.ProductAttrebutes;

namespace PacketParser.Entities;

public class WebProductAttributeValue : IProductAttributeValue, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	private string name;

	private string slug;

	public Guid Guid { get; set; }

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value.FixString().TrimEnd(Array.Empty<char>()).TrimStart(Array.Empty<char>());
		}
	}

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid MasterGuid { get; set; }

	public string Slug
	{
		get
		{
			if (string.IsNullOrEmpty(slug))
			{
				slug = name.WordPressFixSlugOrSku();
			}
			return slug;
		}
		set
		{
			slug = value;
		}
	}
}
