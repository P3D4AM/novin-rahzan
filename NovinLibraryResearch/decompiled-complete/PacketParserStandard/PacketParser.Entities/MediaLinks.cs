namespace PacketParser.Entities;

public class MediaLinks
{
	public string Url { get; set; } = "";


	public string Title { get; set; } = "";


	public string Description { get; set; } = "";


	public string HY
	{
		get
		{
			return Url;
		}
		set
		{
			Url = value;
		}
	}

	public string HW
	{
		get
		{
			return Title;
		}
		set
		{
			Title = value;
		}
	}

	public string HZ
	{
		get
		{
			return Description;
		}
		set
		{
			Description = value;
		}
	}
}
