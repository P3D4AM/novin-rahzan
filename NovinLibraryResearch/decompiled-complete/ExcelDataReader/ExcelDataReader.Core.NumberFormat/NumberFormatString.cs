using System.Collections.Generic;

namespace ExcelDataReader.Core.NumberFormat;

public class NumberFormatString
{
	public bool IsValid { get; }

	public string FormatString { get; }

	public bool IsDateTimeFormat { get; }

	public bool IsTimeSpanFormat { get; }

	internal IReadOnlyList<Section> Sections { get; }

	public NumberFormatString(string formatString)
	{
		Tokenizer reader = new Tokenizer(formatString);
		List<Section> list = new List<Section>();
		bool flag = true;
		while (true)
		{
			bool syntaxError;
			Section section = Parser.ParseSection(reader, out syntaxError);
			if (syntaxError)
			{
				flag = false;
			}
			if (section == null)
			{
				break;
			}
			list.Add(section);
		}
		IsValid = flag;
		FormatString = formatString;
		if (flag)
		{
			Sections = list;
			IsDateTimeFormat = GetFirstSection(SectionType.Date) != null;
			IsTimeSpanFormat = GetFirstSection(SectionType.Duration) != null;
		}
		else
		{
			Sections = new List<Section>();
		}
	}

	private Section GetFirstSection(SectionType type)
	{
		foreach (Section section in Sections)
		{
			if (section.Type == type)
			{
				return section;
			}
		}
		return null;
	}
}
