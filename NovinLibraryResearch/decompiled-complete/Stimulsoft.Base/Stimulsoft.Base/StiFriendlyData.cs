using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Stimulsoft.Base;

public static class StiFriendlyData
{
	public static List<DataSet> GetFromFolder(string folder)
	{
		if (!Directory.Exists(folder))
		{
			return null;
		}
		List<DataSet> list = new List<DataSet>();
		foreach (string item in Directory.EnumerateFiles(folder, "*.xsd"))
		{
			DataSet dataSet = new DataSet(Path.GetFileNameWithoutExtension(item));
			dataSet.ReadXmlSchema(item);
			list.Add(dataSet);
		}
		foreach (string item2 in Directory.EnumerateFiles(folder, "*.xml"))
		{
			string fileName = Path.GetFileNameWithoutExtension(item2);
			DataSet dataSet2 = list.FirstOrDefault((DataSet d) => d.DataSetName.ToLowerInvariant() == fileName.ToLowerInvariant());
			if (dataSet2 == null)
			{
				dataSet2 = new DataSet(fileName);
				list.Add(dataSet2);
			}
			dataSet2.ReadXml(item2);
		}
		return list;
	}
}
