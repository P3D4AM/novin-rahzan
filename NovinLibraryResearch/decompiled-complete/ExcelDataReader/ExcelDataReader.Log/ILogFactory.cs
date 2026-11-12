using System;

namespace ExcelDataReader.Log;

public interface ILogFactory
{
	ILog Create(Type loggingType);
}
