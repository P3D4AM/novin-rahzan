using System;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime;

[Serializable]
[FileIOPermission(SecurityAction.Demand, Unrestricted = true)]
public class ANTLRFileStream : ANTLRStringStream
{
	protected string fileName;

	public override string SourceName => fileName;

	public ANTLRFileStream(string fileName)
		: this(fileName, null)
	{
	}

	public ANTLRFileStream(string fileName, Encoding encoding)
	{
		this.fileName = fileName;
		Load(fileName, encoding);
	}

	public virtual void Load(string fileName, Encoding encoding)
	{
		if (fileName != null)
		{
			string text = ((encoding != null) ? File.ReadAllText(fileName, encoding) : File.ReadAllText(fileName));
			data = text.ToCharArray();
			n = data.Length;
		}
	}
}
