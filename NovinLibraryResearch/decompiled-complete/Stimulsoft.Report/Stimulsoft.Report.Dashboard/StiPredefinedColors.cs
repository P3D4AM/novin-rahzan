using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

public static class StiPredefinedColors
{
	public static List<Color[]> Sets = new List<Color[]>
	{
		StiColor.Get(new string[6] { "2f528f", "3960a7", "406dbb", "6d89cb", "9eadd8", "c0c9e4" }),
		StiColor.Get(new string[6] { "ae5a21", "ca6a28", "e2772e", "ef9164", "f3b29a", "f6ccbe" }),
		StiColor.Get(new string[6] { "787878", "8c8c8c", "9d9d9d", "b1b1b1", "c6c6c6", "d8d8d8" }),
		StiColor.Get(new string[6] { "bc8c00", "daa400", "f3b700", "ffc859", "ffd695", "ffe2bc" }),
		StiColor.Get(new string[6] { "41719c", "4c84b6", "5694cb", "7aa9da", "a4c0e3", "c4d5eb" }),
		StiColor.Get(new string[6] { "507e32", "5f933b", "6ba543", "88b76e", "acca9e", "c9dbc1" }),
		StiColor.Get(new string[6] { "5f5f5f", "b3b3b3", "898989", "212121", "dadada", "aaaaaa" }),
		StiColor.Get(new string[6] { "c0c9e4", "9eadd8", "6d89cb", "406dbb", "3960a7", "2f528f" }),
		StiColor.Get(new string[6] { "f6ccbe", "f3b29a", "ef9164", "e2772e", "ca6a28", "ae5a21" }),
		StiColor.Get(new string[6] { "d8d8d8", "c6c6c6", "b1b1b1", "9d9d9d", "8c8c8c", "787878" }),
		StiColor.Get(new string[6] { "ffe2bc", "ffd695", "ffc859", "f3b700", "daa400", "bc8c00" }),
		StiColor.Get(new string[6] { "c4d5eb", "a4c0e3", "7aa9da", "5694cb", "4c84b6", "41719c" }),
		StiColor.Get(new string[6] { "c9dbc1", "acca9e", "88b76e", "6ba543", "5f933b", "507e32" })
	};

	public static List<Color[]> NegativeSets = new List<Color[]>
	{
		StiColor.Get(new string[6] { "ee0e1b", "d20213", "b3030f", "9d040c", "7f0005", "6b0004" }),
		StiColor.Get(new string[6] { "f36642", "f24e29", "e64926", "d74222", "c93c1e", "af3017" }),
		StiColor.Get(new string[6] { "de3a70", "c81a57", "c81a57", "b21852", "9c144e", "780d46" }),
		StiColor.Get(new string[6] { "718792", "5b7481", "4f6570", "41525b", "344047", "252e32" })
	};
}
