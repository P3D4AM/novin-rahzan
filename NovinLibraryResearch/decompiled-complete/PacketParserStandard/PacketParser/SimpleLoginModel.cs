using System.ComponentModel.DataAnnotations;

namespace PacketParser;

public class SimpleLoginModel
{
	[Required]
	[DataType(DataType.Text)]
	public string Username { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	public SimpleLoginModel(string username, string password)
	{
		Username = username;
		Password = password;
	}

	public SimpleLoginModel()
	{
	}
}
