using System.Drawing;
using System.Text.RegularExpressions;

namespace Stimulsoft.Editor;

public interface ITextSearch
{
	bool Find(string s, SearchOptions options, Regex expression, ref Point position, out int len, out Match match);
}
