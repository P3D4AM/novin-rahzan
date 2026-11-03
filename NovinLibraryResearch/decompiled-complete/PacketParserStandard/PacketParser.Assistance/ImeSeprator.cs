namespace PacketParser.Assistance;

public class ImeSeprator
{
	public string Serial { get; private set; }

	public string ActualIme { get; private set; }

	public string CheckSum { get; private set; }

	public bool IsValidIme { get; private set; }

	public ImeSeprator(string ime)
	{
		Process(ime);
	}

	public ImeSeprator(long ime)
	{
		Process(ime.ToString());
	}

	private void Process(string temp)
	{
		if (!string.IsNullOrEmpty(temp))
		{
			temp = temp.RemoveNoNumbers();
			if (!string.IsNullOrEmpty(temp) && temp.Length == 15)
			{
				ActualIme = temp.Substring(0, 8);
				Serial = temp.Substring(8, 6);
				CheckSum = temp.Substring(14, 1);
				IsValidIme = true;
			}
		}
	}
}
