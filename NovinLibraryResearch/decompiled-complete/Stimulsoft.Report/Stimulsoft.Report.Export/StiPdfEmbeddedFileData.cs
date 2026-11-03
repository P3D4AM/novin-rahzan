namespace Stimulsoft.Report.Export;

public class StiPdfEmbeddedFileData
{
	private string mimeType;

	public string Name { get; set; }

	public string Description { get; set; }

	public byte[] Data { get; set; }

	public string MIMEType
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(mimeType))
			{
				return mimeType;
			}
			string result = "text/plain";
			int num = Name.LastIndexOf('.');
			if (num != -1)
			{
				result = Name.Substring(num + 1) switch
				{
					"xml" => "text/xml", 
					"htm" => "text/html", 
					"html" => "text/html", 
					_ => "text/plain", 
				};
			}
			return result;
		}
		set
		{
			mimeType = value;
		}
	}

	public StiPdfEmbeddedFileData(string name, string description, byte[] data, string mimeType = null)
	{
		Name = name;
		Description = description;
		Data = data;
		if (!string.IsNullOrWhiteSpace(mimeType))
		{
			MIMEType = mimeType;
		}
	}
}
