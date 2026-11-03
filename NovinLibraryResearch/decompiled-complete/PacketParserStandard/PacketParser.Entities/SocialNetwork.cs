namespace PacketParser.Entities;

public class SocialNetwork
{
	public string Icon { get; set; } = "";


	public string Name { get; set; } = "";


	public string Url { get; set; } = "";


	public string I
	{
		get
		{
			return Icon;
		}
		set
		{
			Icon = value;
		}
	}

	public string N
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}

	public string U
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
}
