using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals120;

internal class Parser : VersionedParserBase
{
	public static readonly ParserConfig ParserConfig = new ParserConfig();

	public Parser()
		: base(ParserConfig)
	{
	}

	protected override Babel.ParserGenerator.StateOnlyParser CreateStateOnlyParser()
	{
		return new StateOnlyParser(scanner);
	}

	protected override void DoAction(int action)
	{
		switch (action / 50)
		{
		case 0:
			DoAction_0_49(action);
			break;
		case 1:
			DoAction_50_99(action);
			break;
		case 2:
			DoAction_100_149(action);
			break;
		case 3:
			DoAction_150_199(action);
			break;
		case 4:
			DoAction_200_249(action);
			break;
		case 5:
			DoAction_250_299(action);
			break;
		case 6:
			DoAction_300_349(action);
			break;
		case 7:
			DoAction_350_399(action);
			break;
		case 8:
			DoAction_400_449(action);
			break;
		case 9:
			DoAction_450_499(action);
			break;
		case 10:
			DoAction_500_549(action);
			break;
		case 11:
			DoAction_550_599(action);
			break;
		case 12:
			DoAction_600_649(action);
			break;
		case 13:
			DoAction_650_699(action);
			break;
		case 14:
			DoAction_700_749(action);
			break;
		case 15:
			DoAction_750_799(action);
			break;
		case 16:
			DoAction_800_849(action);
			break;
		case 17:
			DoAction_850_899(action);
			break;
		case 18:
			DoAction_900_949(action);
			break;
		case 19:
			DoAction_950_999(action);
			break;
		case 20:
			DoAction_1000_1049(action);
			break;
		case 21:
			DoAction_1050_1099(action);
			break;
		case 22:
			DoAction_1100_1149(action);
			break;
		case 23:
			DoAction_1150_1199(action);
			break;
		case 24:
			DoAction_1200_1249(action);
			break;
		case 25:
			DoAction_1250_1299(action);
			break;
		case 26:
			DoAction_1300_1349(action);
			break;
		case 27:
			DoAction_1350_1399(action);
			break;
		case 28:
			DoAction_1400_1449(action);
			break;
		case 29:
			DoAction_1450_1499(action);
			break;
		case 30:
			DoAction_1500_1549(action);
			break;
		case 31:
			DoAction_1550_1599(action);
			break;
		case 32:
			DoAction_1600_1649(action);
			break;
		case 33:
			DoAction_1650_1699(action);
			break;
		case 34:
			DoAction_1700_1749(action);
			break;
		case 35:
			DoAction_1750_1799(action);
			break;
		case 36:
			DoAction_1800_1849(action);
			break;
		case 37:
			DoAction_1850_1899(action);
			break;
		case 38:
			DoAction_1900_1949(action);
			break;
		case 39:
			DoAction_1950_1999(action);
			break;
		case 40:
			DoAction_2000_2049(action);
			break;
		case 41:
			DoAction_2050_2099(action);
			break;
		case 42:
			DoAction_2100_2149(action);
			break;
		case 43:
			DoAction_2150_2199(action);
			break;
		case 44:
			DoAction_2200_2249(action);
			break;
		case 45:
			DoAction_2250_2299(action);
			break;
		case 46:
			DoAction_2300_2349(action);
			break;
		case 47:
			DoAction_2350_2399(action);
			break;
		case 48:
			DoAction_2400_2449(action);
			break;
		case 49:
			DoAction_2450_2499(action);
			break;
		case 50:
			DoAction_2500_2549(action);
			break;
		case 51:
			DoAction_2550_2599(action);
			break;
		case 52:
			DoAction_2600_2649(action);
			break;
		case 53:
			DoAction_2650_2699(action);
			break;
		case 54:
			DoAction_2700_2749(action);
			break;
		case 55:
			DoAction_2750_2799(action);
			break;
		case 56:
			DoAction_2800_2849(action);
			break;
		case 57:
			DoAction_2850_2899(action);
			break;
		case 58:
			DoAction_2900_2949(action);
			break;
		case 59:
			DoAction_2950_2999(action);
			break;
		case 60:
			DoAction_3000_3049(action);
			break;
		case 61:
			DoAction_3050_3099(action);
			break;
		case 62:
			DoAction_3100_3149(action);
			break;
		case 63:
			DoAction_3150_3199(action);
			break;
		case 64:
			DoAction_3200_3244(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 2:
			root = value_stack.array[value_stack.top - 1].value as SqlBatch;
			break;
		case 3:
		{
			SqlBatch sqlBatch3 = new SqlBatch(value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlBatch3.Position = yyloc;
			yyval.value = sqlBatch3;
			break;
		}
		case 4:
		{
			SqlBatch sqlBatch2 = new SqlBatch(value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlBatch2.Position = yyloc;
			yyval.value = sqlBatch2;
			break;
		}
		case 5:
		{
			SqlBatch sqlBatch = new SqlBatch(value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlBatch.Position = yyloc;
			yyval.value = sqlBatch;
			break;
		}
		case 7:
		{
			List<SqlStatement> list4 = value_stack.array[value_stack.top - 1].value as List<SqlStatement>;
			if (list4 == null)
			{
				list4 = new List<SqlStatement>();
			}
			list4.Insert(0, value_stack.array[value_stack.top - 2].value as SqlStatement);
			yyval.value = list4;
			break;
		}
		case 8:
		{
			List<SqlStatement> list5 = new List<SqlStatement>();
			list5.Add(value_stack.array[value_stack.top - 2].value as SqlStatement);
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list5;
			break;
		}
		case 9:
		{
			SqlStatement sqlStatement3 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement3 == null)
			{
				sqlStatement3 = new SqlNullStatement();
			}
			sqlStatement3.Position = yyloc;
			yyval.value = sqlStatement3;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement3);
			break;
		}
		case 10:
		{
			SqlStatement sqlStatement6 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement6 == null)
			{
				sqlStatement6 = new SqlNullStatement();
			}
			sqlStatement6.Position = yyloc;
			yyval.value = sqlStatement6;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement6);
			break;
		}
		case 11:
		{
			SqlStatement sqlStatement5 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement5 == null)
			{
				sqlStatement5 = new SqlNullStatement();
			}
			sqlStatement5.Position = yyloc;
			yyval.value = sqlStatement5;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement5);
			break;
		}
		case 12:
		{
			SqlStatement sqlStatement4 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement4 == null)
			{
				sqlStatement4 = new SqlNullStatement();
			}
			sqlStatement4.Position = yyloc;
			yyval.value = sqlStatement4;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement4);
			break;
		}
		case 16:
		{
			List<SqlStatement> list3 = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list3;
			break;
		}
		case 19:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 20:
		{
			List<SqlStatement> list2 = new List<SqlStatement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list2;
			break;
		}
		case 21:
		{
			List<SqlStatement> list = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list;
			break;
		}
		case 29:
		{
			yyval = value_stack.array[value_stack.top - 2];
			SqlStatement sqlStatement2 = value_stack.array[value_stack.top - 2].value as SqlStatement;
			if (sqlStatement2 == null)
			{
				sqlStatement2 = new SqlNullStatement();
				sqlStatement2.Position = yyloc;
				yyval.value = sqlStatement2;
			}
			break;
		}
		case 30:
		{
			yyval = value_stack.array[value_stack.top - 1];
			SqlStatement sqlStatement = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement == null)
			{
				sqlStatement = new SqlNullStatement();
				sqlStatement.Position = yyloc;
				yyval.value = sqlStatement;
			}
			break;
		}
		case 34:
		{
			SqlStatementError sqlStatementError2 = new SqlStatementError(value_stack.array[value_stack.top - 2].value as SqlErrorCodeObject);
			sqlStatementError2.Position = yyloc;
			yyval.value = sqlStatementError2;
			scanner.ResetAllParseStates();
			break;
		}
		case 35:
		{
			SqlStatementError sqlStatementError = new SqlStatementError(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlStatementError.Position = yyloc;
			yyval.value = sqlStatementError;
			break;
		}
		case 1:
		case 6:
		case 13:
		case 14:
		case 15:
		case 17:
		case 18:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 31:
		case 32:
		case 33:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
			break;
		}
	}

	private void DoAction_50_99(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_100_149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_150_199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_200_249(int action)
	{
		switch (action)
		{
		case 219:
		{
			SqlIdentifier sqlIdentifier5 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier5.Position = yyloc;
			yyval.value = sqlIdentifier5;
			break;
		}
		case 220:
		{
			SqlIdentifier sqlIdentifier4 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier4.Position = yyloc;
			yyval.value = sqlIdentifier4;
			break;
		}
		case 221:
		{
			SqlIdentifier sqlIdentifier3 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 222:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 223:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 224:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 226:
		{
			SqlUseStatement sqlUseStatement2 = new SqlUseStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUseStatement2.Position = yyloc;
			yyval.value = sqlUseStatement2;
			break;
		}
		case 227:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			SqlUseStatement sqlUseStatement = new SqlUseStatement(sqlIdentifier);
			sqlUseStatement.Position = yyloc;
			yyval.value = sqlUseStatement;
			break;
		}
		case 228:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 229:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 230:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 200:
		case 201:
		case 202:
		case 203:
		case 204:
		case 205:
		case 206:
		case 207:
		case 208:
		case 209:
		case 210:
		case 211:
		case 212:
		case 213:
		case 214:
		case 215:
		case 216:
		case 217:
		case 218:
		case 225:
		case 231:
		case 232:
		case 233:
		case 234:
		case 235:
		case 236:
		case 237:
		case 238:
		case 239:
		case 240:
		case 241:
		case 242:
		case 243:
		case 244:
		case 245:
		case 246:
		case 247:
		case 248:
		case 249:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		case 286:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 289:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 290:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 291:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 250:
		case 251:
		case 252:
		case 253:
		case 254:
		case 255:
		case 256:
		case 257:
		case 258:
		case 259:
		case 260:
		case 261:
		case 262:
		case 263:
		case 264:
		case 265:
		case 266:
		case 267:
		case 268:
		case 269:
		case 270:
		case 271:
		case 272:
		case 273:
		case 274:
		case 275:
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 282:
		case 283:
		case 284:
		case 285:
		case 287:
		case 288:
		case 292:
		case 293:
		case 294:
		case 295:
		case 296:
		case 297:
		case 298:
		case 299:
			break;
		}
	}

	private void DoAction_300_349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_350_399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_400_449(int action)
	{
		switch (action)
		{
		case 425:
		{
			SqlTableDefinition sqlTableDefinition3 = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition3.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement4 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition3, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement4.Position = yyloc;
			yyval.value = sqlCreateTableStatement4;
			break;
		}
		case 426:
		{
			SqlTableDefinition sqlTableDefinition2 = new SqlTableDefinition(value_stack.array[value_stack.top - 7].value as List<ISqlTableElement>);
			sqlTableDefinition2.Position = location_stack.array[location_stack.top - 7];
			SqlCreateTableStatement sqlCreateTableStatement3 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 9].value as SqlObjectIdentifier, sqlTableDefinition2, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement3.Position = yyloc;
			yyval.value = sqlCreateTableStatement3;
			break;
		}
		case 427:
		{
			SqlCreateTableStatement sqlCreateTableStatement2 = new SqlCreateTableStatement(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, new SqlTableDefinition(new List<ISqlTableElement>()), value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement2.Position = yyloc;
			yyval.value = sqlCreateTableStatement2;
			break;
		}
		case 429:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 5].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = location_stack.array[location_stack.top - 5];
			SqlCreateTableStatement sqlCreateTableStatement = new SqlCreateTableStatement(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, sqlTableDefinition, value_stack.array[value_stack.top - 3].value as SqlStorageSpecification, value_stack.array[value_stack.top - 2].value as SqlLargeDataStorageInformation);
			sqlCreateTableStatement.Position = yyloc;
			yyval.value = sqlCreateTableStatement;
			break;
		}
		case 443:
		{
			List<ISqlTableElement> list2 = new List<ISqlTableElement>();
			list2.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list2;
			break;
		}
		case 444:
		{
			List<ISqlTableElement> list = value_stack.array[value_stack.top - 3].value as List<ISqlTableElement>;
			list.Add(value_stack.array[value_stack.top - 1].value as ISqlTableElement);
			yyval.value = list;
			break;
		}
		case 445:
		{
			SqlColumnDefinition sqlColumnDefinition2 = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition2.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition2.Position = yyloc;
			yyval.value = sqlColumnDefinition2;
			break;
		}
		case 447:
		{
			SqlColumnDefinition sqlColumnDefinition = value_stack.array[value_stack.top - 2].value as SqlColumnDefinition;
			sqlColumnDefinition.AddConstraints(value_stack.array[value_stack.top - 1].value as List<SqlConstraint>);
			sqlColumnDefinition.Position = yyloc;
			yyval.value = sqlColumnDefinition;
			break;
		}
		case 400:
		case 401:
		case 402:
		case 403:
		case 404:
		case 405:
		case 406:
		case 407:
		case 408:
		case 409:
		case 410:
		case 411:
		case 412:
		case 413:
		case 414:
		case 415:
		case 416:
		case 417:
		case 418:
		case 419:
		case 420:
		case 421:
		case 422:
		case 423:
		case 424:
		case 428:
		case 430:
		case 431:
		case 432:
		case 433:
		case 434:
		case 435:
		case 436:
		case 437:
		case 438:
		case 439:
		case 440:
		case 441:
		case 442:
		case 446:
		case 448:
		case 449:
			break;
		}
	}

	private void DoAction_450_499(int action)
	{
		switch (action)
		{
		case 451:
		{
			SqlColumnDefinition sqlColumnDefinition4 = new SqlColumnDefinition(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlColumnDefinition4.Position = yyloc;
			yyval.value = sqlColumnDefinition4;
			break;
		}
		case 453:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 3].value as SqlDataType);
			sqlDataTypeSpecification3.Position = location_stack.array[location_stack.top - 3];
			SqlColumnDefinition sqlColumnDefinition3 = new SqlColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, sqlDataTypeSpecification3, value_stack.array[value_stack.top - 2].value as SqlCollation, (SqlSparseOption)value_stack.array[value_stack.top - 1].value);
			sqlColumnDefinition3.Position = yyloc;
			yyval.value = sqlColumnDefinition3;
			break;
		}
		case 454:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 6].value as SqlDataType, sqlLiteralExpression);
			sqlDataTypeSpecification2.Position = MergeLocationRange(location_stack.top - 6, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition2 = new SqlColumnDefinition(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlDataTypeSpecification2, value_stack.array[value_stack.top - 2].value as SqlCollation, (SqlSparseOption)value_stack.array[value_stack.top - 1].value);
			sqlColumnDefinition2.Position = yyloc;
			yyval.value = sqlColumnDefinition2;
			break;
		}
		case 455:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression);
			sqlDataTypeSpecification.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 2);
			SqlColumnDefinition sqlColumnDefinition = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification, (SqlSparseOption)value_stack.array[value_stack.top - 1].value);
			sqlColumnDefinition.Position = yyloc;
			yyval.value = sqlColumnDefinition;
			break;
		}
		case 456:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 7].value as SqlDataType, value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier);
			sqlDataTypeSpecification4.Position = MergeLocationRange(location_stack.top - 7, location_stack.top - 3);
			SqlColumnDefinition sqlColumnDefinition5 = new SqlColumnDefinition(value_stack.array[value_stack.top - 8].value as SqlIdentifier, sqlDataTypeSpecification4, value_stack.array[value_stack.top - 2].value as SqlCollation);
			sqlColumnDefinition5.Position = yyloc;
			yyval.value = sqlColumnDefinition5;
			break;
		}
		case 457:
		{
			SqlDataType sqlDataType2 = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: false, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType2.Position = yyloc;
			yyval.value = sqlDataType2;
			if ((sqlDataType2.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 458:
		{
			SqlDataType sqlDataType = new SqlDataType(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, national: true, value_stack.array[value_stack.top - 1].value != null);
			sqlDataType.Position = yyloc;
			yyval.value = sqlDataType;
			if ((sqlDataType.TypeErrors & DataTypeErrors.NationalNotAllowed) == DataTypeErrors.NationalNotAllowed)
			{
				VersionedParserBase.SyntaxError("national", location_stack.array[location_stack.top - 3]);
			}
			else if ((sqlDataType.TypeErrors & DataTypeErrors.VaryingNotAllowed) == DataTypeErrors.VaryingNotAllowed)
			{
				VersionedParserBase.SyntaxError("varying", location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 459:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			});
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 2];
			SqlDataType sqlDataType3 = SqlDataType.Double(sqlObjectIdentifier4);
			sqlDataType3.Position = yyloc;
			yyval.value = sqlDataType3;
			ExpectKeyword(StringToken.Precision, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 460:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = yyloc;
			yyval.value = sqlObjectIdentifier3;
			break;
		}
		case 461:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 462:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 463:
			yyval.value = SqlIdentifier.Null;
			break;
		case 468:
		{
			SqlCollation sqlCollation = new SqlCollation(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlCollation.Position = yyloc;
			yyval.value = sqlCollation;
			if (EscapeSequence.IdentifyEscapeSequence(GetText(location_stack.array[location_stack.top - 1])) != null)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 473:
			yyval.value = SqlSparseOption.None;
			break;
		case 474:
			yyval.value = SqlSparseOption.Sparse;
			break;
		case 475:
			if (ExpectKeyword(StringToken.AllSparseColumns, location_stack.array[location_stack.top - 1]))
			{
				yyval.value = SqlSparseOption.ColumnSet;
			}
			else
			{
				yyval.value = SqlSparseOption.None;
			}
			break;
		case 476:
			yyval.value = SqlSparseOption.None;
			break;
		case 477:
			if (ExpectKeyword(StringToken.Sparse, location_stack.array[location_stack.top - 1]))
			{
				yyval.value = SqlSparseOption.Sparse;
			}
			else
			{
				yyval.value = SqlSparseOption.None;
			}
			break;
		case 478:
			yyval.value = null;
			break;
		case 479:
			yyval.value = "varying";
			break;
		case 482:
		{
			List<SqlConstraint> list2 = new List<SqlConstraint>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list2;
			break;
		}
		case 483:
		{
			List<SqlConstraint> list = value_stack.array[value_stack.top - 2].value as List<SqlConstraint>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlConstraint);
			yyval.value = list;
			break;
		}
		case 485:
		{
			SqlConstraint sqlConstraint3 = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint3.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint3.Position = yyloc;
			yyval.value = sqlConstraint3;
			break;
		}
		case 486:
			yyval.value = value_stack.array[value_stack.top - 1].value as ISqlTableElement;
			break;
		case 488:
		{
			SqlConstraint sqlConstraint2 = value_stack.array[value_stack.top - 1].value as SqlConstraint;
			sqlConstraint2.Name = value_stack.array[value_stack.top - 2].value as SqlIdentifier;
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 493:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint2 = new SqlForeignKeyConstraint(value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint2.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint2;
			break;
		}
		case 494:
		{
			SqlDefaultConstraint sqlDefaultConstraint = new SqlDefaultConstraint(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlDefaultConstraint.Position = yyloc;
			yyval.value = sqlDefaultConstraint;
			break;
		}
		case 495:
		{
			SqlForeignKeyConstraint sqlForeignKeyConstraint = new SqlForeignKeyConstraint(null, value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>, (SqlForeignKeyAction)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlForeignKeyConstraint.Position = yyloc;
			yyval.value = sqlForeignKeyConstraint;
			break;
		}
		case 496:
		{
			SqlCheckConstraint sqlCheckConstraint = new SqlCheckConstraint((bool)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 2].value as SqlBooleanExpression);
			sqlCheckConstraint.Position = yyloc;
			yyval.value = sqlCheckConstraint;
			break;
		}
		case 497:
		{
			SqlColumnIdentity sqlColumnIdentity = value_stack.array[value_stack.top - 2].value as SqlColumnIdentity;
			if ((bool)value_stack.array[value_stack.top - 1].value)
			{
				sqlColumnIdentity.AddNotForReplicationClause();
			}
			sqlColumnIdentity.Position = yyloc;
			yyval.value = sqlColumnIdentity;
			yyval = value_stack.array[value_stack.top - 2];
			break;
		}
		case 499:
		{
			SqlConstraint sqlConstraint = SqlConstraint.RowGuidCol();
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 450:
		case 452:
		case 464:
		case 465:
		case 466:
		case 467:
		case 469:
		case 470:
		case 471:
		case 472:
		case 480:
		case 481:
		case 484:
		case 487:
		case 489:
		case 490:
		case 491:
		case 492:
		case 498:
			break;
		}
	}

	private void DoAction_500_549(int action)
	{
		switch (action)
		{
		case 500:
			yyval.value = false;
			break;
		case 501:
			yyval.value = true;
			break;
		case 502:
			yyval.value = 512;
			break;
		case 503:
			yyval.value = 768;
			break;
		case 504:
			yyval.value = 1024;
			break;
		case 505:
		{
			bool flag2 = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag2 ? 256 : 0);
			break;
		}
		case 506:
			yyval.value = SqlForeignKeyAction.Cascade;
			break;
		case 507:
			yyval.value = SqlForeignKeyAction.SetDefault;
			break;
		case 508:
			yyval.value = SqlForeignKeyAction.SetNull;
			break;
		case 509:
		{
			bool flag = ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]) && ExpectKeyword(StringToken.Action, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlForeignKeyAction.NoAction : SqlForeignKeyAction.None);
			break;
		}
		case 510:
			yyval.value = SqlForeignKeyAction.None;
			break;
		case 511:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 512:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 513:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 514:
			yyval.value = (int)value_stack.array[value_stack.top - 2].value + (int)value_stack.array[value_stack.top - 1].value;
			break;
		case 521:
		{
			SqlUniqueConstraintBase sqlUniqueConstraintBase = SqlUniqueConstraintBase.Create((SqlConstraintType)value_stack.array[value_stack.top - 6].value, (SqlClusterOption)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 4].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlUniqueConstraintBase.Position = yyloc;
			yyval.value = sqlUniqueConstraintBase;
			break;
		}
		case 522:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint3 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlInlineIndexConstraint3.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint3;
			break;
		}
		case 523:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint2 = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlInlineIndexConstraint2.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint2;
			break;
		}
		case 524:
		{
			SqlInlineIndexConstraint sqlInlineIndexConstraint = new SqlInlineIndexConstraint(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>);
			sqlInlineIndexConstraint.Position = yyloc;
			yyval.value = sqlInlineIndexConstraint;
			break;
		}
		case 525:
			yyval.value = SqlClusterOption.Default;
			break;
		case 526:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 527:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 528:
			yyval.value = SqlClusterOption.NonClusteredHash;
			break;
		case 529:
			yyval.value = SqlClusterOption.Hash;
			break;
		case 530:
			yyval.value = SqlConstraintType.Unique;
			break;
		case 531:
			yyval.value = SqlConstraintType.PrimaryKey;
			break;
		case 533:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 534:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 535:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 537:
		{
			SqlConstraint sqlConstraint2 = SqlConstraint.Null();
			sqlConstraint2.Position = yyloc;
			yyval.value = sqlConstraint2;
			break;
		}
		case 538:
		{
			SqlConstraint sqlConstraint = SqlConstraint.NotNull();
			sqlConstraint.Position = yyloc;
			yyval.value = sqlConstraint;
			break;
		}
		case 539:
		{
			SqlColumnIdentity sqlColumnIdentity2 = new SqlColumnIdentity();
			sqlColumnIdentity2.Position = yyloc;
			yyval.value = sqlColumnIdentity2;
			break;
		}
		case 540:
		{
			SqlColumnIdentity sqlColumnIdentity = new SqlColumnIdentity(value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlColumnIdentity.Position = yyloc;
			yyval.value = sqlColumnIdentity;
			break;
		}
		case 541:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 542:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), (bool)value_stack.array[value_stack.top - 2].value);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 543:
			yyval.value = false;
			break;
		case 544:
			yyval.value = false;
			break;
		case 545:
			yyval.value = true;
			break;
		case 515:
		case 516:
		case 517:
		case 518:
		case 519:
		case 520:
		case 532:
		case 536:
		case 546:
		case 547:
		case 548:
		case 549:
			break;
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
		case 563:
			ExpectKeyword(StringToken.Partitions, location_stack.array[location_stack.top - 4]);
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 564:
		{
			List<SqlCompressionPartitionRange> list = new List<SqlCompressionPartitionRange>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list;
			break;
		}
		case 565:
		{
			List<SqlCompressionPartitionRange> list2 = value_stack.array[value_stack.top - 3].value as List<SqlCompressionPartitionRange>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCompressionPartitionRange);
			yyval.value = list2;
			break;
		}
		case 566:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange2 = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange2.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange2;
			break;
		}
		case 567:
		{
			SqlCompressionPartitionRange sqlCompressionPartitionRange = new SqlCompressionPartitionRange(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlCompressionPartitionRange.Position = yyloc;
			yyval.value = sqlCompressionPartitionRange;
			break;
		}
		case 570:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 571:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 579:
		{
			SqlComputedColumnDefinition sqlComputedColumnDefinition = new SqlComputedColumnDefinition(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value != null);
			sqlComputedColumnDefinition.Position = yyloc;
			yyval.value = sqlComputedColumnDefinition;
			break;
		}
		case 581:
		{
			bool flag = ExpectKeyword(StringToken.Persisted, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? "not null" : null);
			break;
		}
		case 582:
		{
			SqlInlineTableVariableDeclareStatement sqlInlineTableVariableDeclareStatement = new SqlInlineTableVariableDeclareStatement(new SqlInlineTableVariableDeclaration(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 1].value as SqlTableDefinition)
			{
				Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 1)
			});
			sqlInlineTableVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlInlineTableVariableDeclareStatement;
			break;
		}
		case 583:
		{
			SqlTableDefinition sqlTableDefinition = new SqlTableDefinition(value_stack.array[value_stack.top - 2].value as List<ISqlTableElement>);
			sqlTableDefinition.Position = yyloc;
			yyval.value = sqlTableDefinition;
			break;
		}
		case 550:
		case 551:
		case 552:
		case 553:
		case 554:
		case 555:
		case 556:
		case 557:
		case 558:
		case 559:
		case 560:
		case 561:
		case 562:
		case 568:
		case 569:
		case 572:
		case 573:
		case 574:
		case 575:
		case 576:
		case 577:
		case 578:
		case 580:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 590:
		case 591:
		case 592:
		case 593:
		case 594:
		case 595:
		case 596:
		case 597:
		case 598:
		case 599:
			break;
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		case 633:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 634:
			yyval.value = new CreateIndexStart(isUnique: true, (SqlClusterOption)value_stack.array[value_stack.top - 3].value);
			break;
		case 635:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Clustered);
			break;
		case 636:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.NonClustered);
			break;
		case 637:
			yyval.value = new CreateIndexStart(isUnique: false, SqlClusterOption.Default);
			break;
		case 638:
		{
			SqlCreateIndexStatement sqlCreateIndexStatement = new SqlCreateIndexStatement(value_stack.array[value_stack.top - 9].value as CreateIndexStart, value_stack.array[value_stack.top - 8].value as SqlIdentifier, value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as CreateIndexKeyList, value_stack.array[value_stack.top - 4].value as SqlFilterClause, value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>, value_stack.array[value_stack.top - 2].value as SqlStorageSpecification, value_stack.array[value_stack.top - 1].value as SqlStorageSpecification);
			sqlCreateIndexStatement.Position = yyloc;
			yyval.value = sqlCreateIndexStatement;
			if ((value_stack.array[value_stack.top - 9].value as CreateIndexStart).ClusterOption == SqlClusterOption.Clustered && value_stack.array[value_stack.top - 4].value is SqlFilterClause)
			{
				VersionedParserBase.SyntaxError("WHERE", location_stack.array[location_stack.top - 4]);
			}
			break;
		}
		case 640:
		{
			SqlFilterClause sqlFilterClause = new SqlFilterClause(value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlFilterClause.Position = yyloc;
			yyval.value = sqlFilterClause;
			break;
		}
		case 641:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 642:
			yyval.value = false;
			break;
		case 643:
			yyval.value = true;
			break;
		case 644:
			yyval.value = SqlClusterOption.Default;
			break;
		case 645:
			yyval.value = SqlClusterOption.NonClustered;
			break;
		case 646:
			yyval.value = SqlClusterOption.Clustered;
			break;
		case 600:
		case 601:
		case 602:
		case 603:
		case 604:
		case 605:
		case 606:
		case 607:
		case 608:
		case 609:
		case 610:
		case 611:
		case 612:
		case 613:
		case 614:
		case 615:
		case 616:
		case 617:
		case 618:
		case 619:
		case 620:
		case 621:
		case 622:
		case 623:
		case 624:
		case 625:
		case 626:
		case 627:
		case 628:
		case 629:
		case 630:
		case 631:
		case 632:
		case 639:
		case 647:
		case 648:
		case 649:
			break;
		}
	}

	private void DoAction_650_699(int action)
	{
		switch (action)
		{
		case 650:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 652:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 653:
		{
			List<SqlIndexOption> list = new List<SqlIndexOption>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list;
			break;
		}
		case 654:
		{
			List<SqlIndexOption> list8 = new List<SqlIndexOption>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list8;
			break;
		}
		case 655:
		{
			List<SqlIndexOption> list7 = new List<SqlIndexOption>();
			list7.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list7;
			break;
		}
		case 656:
		{
			List<SqlIndexOption> list6 = new List<SqlIndexOption>();
			list6.Add(value_stack.array[value_stack.top - 2].value as SqlIndexOption);
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list6;
			break;
		}
		case 658:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 660:
			yyval.value = new CreateIndexKeyList(value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			break;
		case 662:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 663:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 664:
		{
			List<SqlIndexOption> list5 = new List<SqlIndexOption>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list5;
			break;
		}
		case 665:
		{
			List<SqlIndexOption> list4 = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list4;
			break;
		}
		case 666:
		{
			SqlIndexOption sqlIndexOption5;
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.PadIndex:
				sqlIndexOption5 = new SqlPadIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption5 = new SqlSortInTempDbIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption5 = new SqlIgnoreDupKeyIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption5 = new SqlStatisticsNoRecomputeIndexOption(SqlOnOffValue.None);
				break;
			case StringToken.DropExisting:
				sqlIndexOption5 = new SqlDropExistingIndexOption(SqlOnOffValue.None);
				break;
			default:
				sqlIndexOption5 = new SqlIndexOptionError();
				VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
				break;
			}
			sqlIndexOption5.Position = yyloc;
			yyval.value = sqlIndexOption5;
			break;
		}
		case 669:
		{
			SqlFillFactorIndexOption sqlFillFactorIndexOption = new SqlFillFactorIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlFillFactorIndexOption.Position = yyloc;
			yyval.value = sqlFillFactorIndexOption;
			break;
		}
		case 670:
		{
			VersionedParserBase.ReportError(SR.InvalidUsageOfIndexOption(GetText(location_stack.array[location_stack.top - 3])), location_stack.array[location_stack.top - 3]);
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption2 = new SqlMaxDegreeOfParallelismIndexOption((SqlLiteralExpression)value_stack.array[value_stack.top - 1].value, isValid: true);
			sqlMaxDegreeOfParallelismIndexOption2.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption2;
			break;
		}
		case 671:
		{
			SqlIndexOption sqlIndexOption4 = new SqlSortedDataIndexOption(SqlOnOffValue.None);
			sqlIndexOption4.Position = yyloc;
			yyval.value = sqlIndexOption4;
			break;
		}
		case 672:
		{
			SqlIndexOption sqlIndexOption3 = new SqlSortedDataReorgIndexOption(SqlOnOffValue.None);
			sqlIndexOption3.Position = yyloc;
			yyval.value = sqlIndexOption3;
			break;
		}
		case 683:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 684:
		{
			List<SqlIndexOption> list3 = new List<SqlIndexOption>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list3;
			break;
		}
		case 685:
		{
			List<SqlIndexOption> list2 = value_stack.array[value_stack.top - 3].value as List<SqlIndexOption>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexOption);
			yyval.value = list2;
			break;
		}
		case 686:
		{
			SqlOnlineIndexOption sqlOnlineIndexOption = new SqlOnlineIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 2].value);
			sqlOnlineIndexOption.Position = yyloc;
			yyval.value = sqlOnlineIndexOption;
			break;
		}
		case 687:
		{
			SqlIndexOption sqlIndexOption2;
			switch (GetStringToken(location_stack.array[location_stack.top - 3]))
			{
			case StringToken.PadIndex:
				sqlIndexOption2 = new SqlPadIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortInTempDb:
				sqlIndexOption2 = new SqlSortInTempDbIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.IgnoreDupKey:
				sqlIndexOption2 = new SqlIgnoreDupKeyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsIncremental:
				sqlIndexOption2 = new SqlStatisticsIncrementalIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.StatisticsNoRecompute:
				sqlIndexOption2 = new SqlStatisticsNoRecomputeIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.DropExisting:
				sqlIndexOption2 = new SqlDropExistingIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowRowLocks:
				sqlIndexOption2 = new SqlAllowRowLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.AllowPageLocks:
				sqlIndexOption2 = new SqlAllowPageLocksIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedData:
				sqlIndexOption2 = new SqlSortedDataIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			case StringToken.SortedDataReorg:
				sqlIndexOption2 = new SqlSortedDataReorgIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
				break;
			default:
				sqlIndexOption2 = new SqlIndexOptionError();
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
				break;
			}
			sqlIndexOption2.Position = yyloc;
			yyval.value = sqlIndexOption2;
			break;
		}
		case 688:
		{
			SqlIndexOptionError sqlIndexOptionError3 = new SqlIndexOptionError();
			sqlIndexOptionError3.Position = yyloc;
			yyval.value = sqlIndexOptionError3;
			VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			break;
		}
		case 689:
		{
			SqlMaxDegreeOfParallelismIndexOption sqlMaxDegreeOfParallelismIndexOption = new SqlMaxDegreeOfParallelismIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlMaxDegreeOfParallelismIndexOption.Position = yyloc;
			yyval.value = sqlMaxDegreeOfParallelismIndexOption;
			break;
		}
		case 690:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption2 = new SqlStatisticsOnlyIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlStatisticsOnlyIndexOption2.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption2;
			break;
		}
		case 691:
		{
			SqlStatisticsOnlyIndexOption sqlStatisticsOnlyIndexOption = new SqlStatisticsOnlyIndexOption((SqlOnOffValue)value_stack.array[value_stack.top - 1].value);
			sqlStatisticsOnlyIndexOption.Position = yyloc;
			yyval.value = sqlStatisticsOnlyIndexOption;
			break;
		}
		case 692:
		{
			List<SqlCompressionPartitionRange> partitions = value_stack.array[value_stack.top - 1].value as List<SqlCompressionPartitionRange>;
			SqlIndexOption sqlIndexOption;
			switch (GetStringToken(location_stack.array[location_stack.top - 2]))
			{
			case StringToken.NONE:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.None, partitions);
				break;
			case StringToken.Row:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Row, partitions);
				break;
			case StringToken.Page:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Page, partitions);
				break;
			case StringToken.ColumnStore:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.ColumnStore, partitions);
				break;
			case StringToken.ColumnStoreArchive:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.ColumnStoreArchive, partitions);
				break;
			default:
				sqlIndexOption = new SqlDataCompressionIndexOption(SqlDataCompressionType.Invalid, partitions);
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
				break;
			}
			sqlIndexOption.Position = yyloc;
			yyval.value = sqlIndexOption;
			break;
		}
		case 695:
		{
			SqlBucketCountIndexOption sqlBucketCountIndexOption = new SqlBucketCountIndexOption(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlBucketCountIndexOption.Position = yyloc;
			yyval.value = sqlBucketCountIndexOption;
			break;
		}
		case 696:
		{
			SqlIndexOptionError sqlIndexOptionError2 = new SqlIndexOptionError();
			sqlIndexOptionError2.Position = yyloc;
			yyval.value = sqlIndexOptionError2;
			break;
		}
		case 697:
		{
			SqlIndexOptionError sqlIndexOptionError = new SqlIndexOptionError();
			sqlIndexOptionError.Position = yyloc;
			yyval.value = sqlIndexOptionError;
			break;
		}
		case 699:
		{
			SqlCreateUserStatement sqlCreateUserStatement = SqlCreateUserStatement.CreateFromFactory(value_stack.array[value_stack.top - 2].value as CreateUserStatementFactory, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlCreateUserOption>);
			sqlCreateUserStatement.Position = yyloc;
			yyval.value = sqlCreateUserStatement;
			break;
		}
		case 651:
		case 657:
		case 659:
		case 661:
		case 667:
		case 668:
		case 673:
		case 674:
		case 675:
		case 676:
		case 677:
		case 678:
		case 679:
		case 680:
		case 681:
		case 682:
		case 693:
		case 694:
		case 698:
			break;
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		case 701:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithImplicitAuthenticationStatementFactory();
			break;
		case 702:
		{
			CreateUserStatementFactory value;
			switch (GetStringToken(location_stack.array[location_stack.top - 2]))
			{
			case StringToken.Login:
				value = CreateUserStatementFactory.CreateCreateUserFromLoginStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
				break;
			case StringToken.Certificate:
				value = CreateUserStatementFactory.CreateCreateUserFromCertificateStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
				break;
			default:
				value = CreateUserStatementFactory.CreateCreateUserStatementErrorFactory();
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
				break;
			}
			yyval.value = value;
			break;
		}
		case 703:
			yyval.value = CreateUserStatementFactory.CreateCreateUserFromAsymKeyStatementFactory(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 3]);
			break;
		case 704:
			yyval.value = CreateUserStatementFactory.CreateCreateUserWithoutLoginStatementFactory();
			ExpectKeyword(StringToken.Without, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Login, location_stack.array[location_stack.top - 1]);
			break;
		case 709:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 710:
		{
			List<SqlCreateUserOption> list2 = new List<SqlCreateUserOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list2;
			break;
		}
		case 711:
		{
			List<SqlCreateUserOption> list = value_stack.array[value_stack.top - 3].value as List<SqlCreateUserOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCreateUserOption);
			yyval.value = list;
			break;
		}
		case 712:
		{
			SqlCreateUserOption sqlCreateUserOption3 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlCreateUserOption3.Position = yyloc;
			yyval.value = sqlCreateUserOption3;
			break;
		}
		case 713:
		{
			SqlCreateUserOption sqlCreateUserOption2 = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateUserOption2.Position = yyloc;
			yyval.value = sqlCreateUserOption2;
			StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 3]);
			if (stringToken != StringToken.Password && stringToken != StringToken.DefaultSchema && stringToken != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 714:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlCreateUserOption sqlCreateUserOption = new SqlCreateUserOption(GetStringToken(location_stack.array[location_stack.top - 3]), sqlLiteralExpression);
			sqlCreateUserOption.Position = yyloc;
			yyval.value = sqlCreateUserOption;
			if (GetStringToken(location_stack.array[location_stack.top - 3]) != StringToken.DefaultLanguage)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 3]);
			}
			break;
		}
		case 727:
		{
			SqlDropUserStatement sqlDropUserStatement = new SqlDropUserStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropUserStatement.Position = yyloc;
			yyval.value = sqlDropUserStatement;
			break;
		}
		case 728:
		{
			SqlCreateRoleStatement sqlCreateRoleStatement = new SqlCreateRoleStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlCreateRoleStatement.Position = yyloc;
			yyval.value = sqlCreateRoleStatement;
			break;
		}
		case 700:
		case 705:
		case 706:
		case 707:
		case 708:
		case 715:
		case 716:
		case 717:
		case 718:
		case 719:
		case 720:
		case 721:
		case 722:
		case 723:
		case 724:
		case 725:
		case 726:
		case 729:
		case 730:
		case 731:
		case 732:
		case 733:
		case 734:
		case 735:
		case 736:
		case 737:
		case 738:
		case 739:
		case 740:
		case 741:
		case 742:
		case 743:
		case 744:
		case 745:
		case 746:
		case 747:
		case 748:
		case 749:
			break;
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		case 807:
		{
			SqlBackupMasterKeyStatement sqlBackupMasterKeyStatement = SqlBackupMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupMasterKeyStatement;
			break;
		}
		case 808:
		{
			SqlRestoreMasterKeyStatement sqlRestoreMasterKeyStatement = SqlRestoreMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreMasterKeyStatement;
			break;
		}
		case 822:
		{
			SqlBackupServiceMasterKeyStatement sqlBackupServiceMasterKeyStatement = SqlBackupServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlBackupServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlBackupServiceMasterKeyStatement;
			break;
		}
		case 823:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = false;
			break;
		case 824:
			scanner.SetParseState(EParseState.e_ParseLoad);
			isDumpLoadStatement = true;
			break;
		case 825:
		{
			SqlRestoreServiceMasterKeyStatement sqlRestoreServiceMasterKeyStatement = SqlRestoreServiceMasterKeyStatement.Create(isDumpLoadStatement);
			sqlRestoreServiceMasterKeyStatement.Position = yyloc;
			yyval.value = sqlRestoreServiceMasterKeyStatement;
			ExpectKeyword(StringToken.Master, location_stack.array[location_stack.top - 12]);
			ExpectKeyword(StringToken.Decryption, location_stack.array[location_stack.top - 6]);
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 826:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 827:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 849:
		{
			SqlLoginPassword sqlLoginPassword = new SqlLoginPassword(value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>);
			sqlLoginPassword.Position = LexLocation.Merge(location_stack.array[location_stack.top - 3], location_stack.array[location_stack.top - 2]);
			SqlCreateLoginWithPasswordStatement sqlCreateLoginWithPasswordStatement = new SqlCreateLoginWithPasswordStatement(value_stack.array[value_stack.top - 7].value as SqlIdentifier, sqlLoginPassword, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginWithPasswordStatement.Position = yyloc;
			yyval.value = sqlCreateLoginWithPasswordStatement;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 5]);
			break;
		}
		case 800:
		case 801:
		case 802:
		case 803:
		case 804:
		case 805:
		case 806:
		case 809:
		case 810:
		case 811:
		case 812:
		case 813:
		case 814:
		case 815:
		case 816:
		case 817:
		case 818:
		case 819:
		case 820:
		case 821:
		case 828:
		case 829:
		case 830:
		case 831:
		case 832:
		case 833:
		case 834:
		case 835:
		case 836:
		case 837:
		case 838:
		case 839:
		case 840:
		case 841:
		case 842:
		case 843:
		case 844:
		case 845:
		case 846:
		case 847:
		case 848:
			break;
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		case 850:
		{
			SqlCreateLoginFromWindowsStatement sqlCreateLoginFromWindowsStatement = new SqlCreateLoginFromWindowsStatement(value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromWindowsStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromWindowsStatement;
			ExpectKeyword(StringToken.Windows, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 851:
		{
			SqlCreateLoginFromCertificateStatement sqlCreateLoginFromCertificateStatement = new SqlCreateLoginFromCertificateStatement(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromCertificateStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromCertificateStatement;
			ExpectKeyword(StringToken.Certificate, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 852:
		{
			SqlCreateLoginFromAsymKeyStatement sqlCreateLoginFromAsymKeyStatement = new SqlCreateLoginFromAsymKeyStatement(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlCreateLoginFromAsymKeyStatement.Position = yyloc;
			yyval.value = sqlCreateLoginFromAsymKeyStatement;
			ExpectKeyword(StringToken.Asymmetric, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 854:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 857:
		{
			List<SqlLoginOption> list13 = new List<SqlLoginOption>();
			list13.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list13;
			break;
		}
		case 858:
		{
			List<SqlLoginOption> list12 = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list12.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list12;
			break;
		}
		case 859:
		{
			SqlLoginOption sqlLoginOption14 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption14.Position = yyloc;
			yyval.value = sqlLoginOption14;
			break;
		}
		case 862:
		{
			List<SqlLoginOption> list11 = new List<SqlLoginOption>();
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list11;
			break;
		}
		case 863:
		{
			List<SqlLoginOption> list10 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list10;
			break;
		}
		case 864:
		{
			SqlLoginOption sqlLoginOption13 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption13.Position = yyloc;
			yyval.value = sqlLoginOption13;
			break;
		}
		case 865:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 1];
			SqlLoginOption sqlLoginOption12 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), sqlLiteralExpression, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption12.Position = yyloc;
			yyval.value = sqlLoginOption12;
			break;
		}
		case 866:
		{
			SqlLoginOption sqlLoginOption11 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.WithPasswordContext);
			sqlLoginOption11.Position = yyloc;
			yyval.value = sqlLoginOption11;
			break;
		}
		case 868:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 869:
		{
			List<SqlLoginOption> list9 = new List<SqlLoginOption>();
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list9;
			break;
		}
		case 870:
		{
			List<SqlLoginOption> list8 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list8;
			break;
		}
		case 871:
		{
			SqlLoginOption sqlLoginOption10 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromWindowsContext);
			sqlLoginOption10.Position = yyloc;
			yyval.value = sqlLoginOption10;
			break;
		}
		case 873:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 874:
		{
			List<SqlLoginOption> list7 = new List<SqlLoginOption>();
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list7;
			break;
		}
		case 875:
		{
			List<SqlLoginOption> list6 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list6;
			break;
		}
		case 876:
		{
			SqlLoginOption sqlLoginOption9 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption9.Position = yyloc;
			yyval.value = sqlLoginOption9;
			break;
		}
		case 877:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement2 = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>);
			sqlAlterLoginStatement2.Position = yyloc;
			yyval.value = sqlAlterLoginStatement2;
			break;
		}
		case 878:
		{
			SqlAlterLoginStatement sqlAlterLoginStatement = new SqlAlterLoginStatement(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlAlterLoginStatement.Position = yyloc;
			yyval.value = sqlAlterLoginStatement;
			break;
		}
		case 879:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 880:
		{
			List<SqlLoginOption> list5 = new List<SqlLoginOption>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list5;
			break;
		}
		case 881:
		{
			List<SqlLoginOption> list4 = new List<SqlLoginOption>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list4;
			break;
		}
		case 882:
		{
			List<SqlLoginOption> list3 = value_stack.array[value_stack.top - 3].value as List<SqlLoginOption>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list3;
			break;
		}
		case 883:
		{
			SqlLoginOption sqlLoginOption8 = SqlLoginOption.Create(new SqlLoginPassword(value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, null)
			{
				Position = location_stack.array[location_stack.top - 4]
			}, new SqlLoginPassword(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression, null)
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlLoginOption8.Position = yyloc;
			yyval.value = sqlLoginOption8;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 6]);
			break;
		}
		case 884:
		{
			SqlLoginOption sqlLoginOption7 = SqlLoginOption.Create(new SqlLoginPassword(value_stack.array[value_stack.top - 2].value as SqlLiteralExpression, value_stack.array[value_stack.top - 1].value as List<SqlLoginOption>)
			{
				Position = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1])
			}, null);
			sqlLoginOption7.Position = yyloc;
			yyval.value = sqlLoginOption7;
			ExpectKeyword(StringToken.Password, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 885:
		{
			SqlLoginOption sqlLoginOption6 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), (SqlOnOffValue)value_stack.array[value_stack.top - 1].value, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption6.Position = yyloc;
			yyval.value = sqlLoginOption6;
			break;
		}
		case 886:
		{
			SqlLoginOption sqlLoginOption5 = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 3]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.AlterOptionsContext);
			sqlLoginOption5.Position = yyloc;
			yyval.value = sqlLoginOption5;
			break;
		}
		case 887:
		{
			SqlLoginOption sqlLoginOption4 = SqlLoginOption.Create(SqlLoginOptionType.NoCredential);
			sqlLoginOption4.Position = yyloc;
			yyval.value = sqlLoginOption4;
			ExpectKeyword(StringToken.No, location_stack.array[location_stack.top - 2]);
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 888:
		{
			SqlLoginOption sqlLoginOption3 = SqlLoginOption.Create(SqlModificationType.Add, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption3.Position = yyloc;
			yyval.value = sqlLoginOption3;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 889:
		{
			SqlLoginOption sqlLoginOption2 = SqlLoginOption.Create(SqlModificationType.Drop, GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier, SqlLoginOptionContext.FromCertificateContext);
			sqlLoginOption2.Position = yyloc;
			yyval.value = sqlLoginOption2;
			ExpectKeyword(StringToken.Credential, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 891:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 892:
		{
			List<SqlLoginOption> list2 = new List<SqlLoginOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list2;
			break;
		}
		case 893:
		{
			List<SqlLoginOption> list = value_stack.array[value_stack.top - 2].value as List<SqlLoginOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlLoginOption);
			yyval.value = list;
			break;
		}
		case 894:
		{
			SqlLoginOption sqlLoginOption = SqlLoginOption.Create(GetText(location_stack.array[location_stack.top - 1]), SqlLoginOptionContext.PasswordContext);
			sqlLoginOption.Position = yyloc;
			yyval.value = sqlLoginOption;
			break;
		}
		case 895:
		{
			SqlDropLoginStatement sqlDropLoginStatement = new SqlDropLoginStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropLoginStatement.Position = yyloc;
			yyval.value = sqlDropLoginStatement;
			break;
		}
		case 853:
		case 855:
		case 856:
		case 860:
		case 861:
		case 867:
		case 872:
		case 890:
		case 896:
		case 897:
		case 898:
		case 899:
			break;
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 942:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = false;
			break;
		case 943:
			scanner.SetParseState(EParseState.e_ParseDump);
			isDumpLoadStatement = true;
			break;
		case 944:
		{
			SqlBackupCertificateStatement sqlBackupCertificateStatement = SqlBackupCertificateStatement.Create(isDumpLoadStatement);
			sqlBackupCertificateStatement.Position = yyloc;
			yyval.value = sqlBackupCertificateStatement;
			break;
		}
		case 900:
		case 901:
		case 902:
		case 903:
		case 904:
		case 905:
		case 906:
		case 907:
		case 908:
		case 909:
		case 910:
		case 911:
		case 912:
		case 913:
		case 914:
		case 915:
		case 916:
		case 917:
		case 918:
		case 919:
		case 920:
		case 921:
		case 922:
		case 923:
		case 924:
		case 925:
		case 926:
		case 927:
		case 928:
		case 929:
		case 930:
		case 931:
		case 932:
		case 933:
		case 934:
		case 935:
		case 936:
		case 937:
		case 938:
		case 939:
		case 940:
		case 941:
		case 945:
		case 946:
		case 947:
		case 948:
		case 949:
			break;
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		case 957:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 950:
		case 951:
		case 952:
		case 953:
		case 954:
		case 955:
		case 956:
		case 958:
		case 959:
		case 960:
		case 961:
		case 962:
		case 963:
		case 964:
		case 965:
		case 966:
		case 967:
		case 968:
		case 969:
		case 970:
		case 971:
		case 972:
		case 973:
		case 974:
		case 975:
		case 976:
		case 977:
		case 978:
		case 979:
		case 980:
		case 981:
		case 982:
		case 983:
		case 984:
		case 985:
		case 986:
		case 987:
		case 988:
		case 989:
		case 990:
		case 991:
		case 992:
		case 993:
		case 994:
		case 995:
		case 996:
		case 997:
		case 998:
		case 999:
			break;
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1000:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1046:
		{
			SqlProcedureDefinitionForCreate sqlProcedureDefinitionForCreate = new SqlProcedureDefinitionForCreate(value_stack.array[value_stack.top - 1].value as SqlProcedureDefinition, isOrAlterStatement: false);
			sqlProcedureDefinitionForCreate.Position = yyloc;
			yyval.value = sqlProcedureDefinitionForCreate;
			break;
		}
		case 1047:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1049:
		{
			SqlCreateProcedureStatement sqlCreateProcedureStatement = new SqlCreateProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinitionForCreate, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateProcedureStatement.Position = yyloc;
			yyval.value = sqlCreateProcedureStatement;
			break;
		}
		case 1001:
		case 1002:
		case 1003:
		case 1004:
		case 1005:
		case 1006:
		case 1007:
		case 1008:
		case 1009:
		case 1010:
		case 1011:
		case 1012:
		case 1013:
		case 1014:
		case 1015:
		case 1016:
		case 1017:
		case 1018:
		case 1019:
		case 1020:
		case 1021:
		case 1022:
		case 1023:
		case 1024:
		case 1025:
		case 1026:
		case 1027:
		case 1028:
		case 1029:
		case 1030:
		case 1031:
		case 1032:
		case 1033:
		case 1034:
		case 1035:
		case 1036:
		case 1037:
		case 1038:
		case 1039:
		case 1040:
		case 1041:
		case 1042:
		case 1043:
		case 1044:
		case 1045:
		case 1048:
			break;
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
		case 1051:
		{
			SqlAlterProcedureStatement sqlAlterProcedureStatement = new SqlAlterProcedureStatement(value_stack.array[value_stack.top - 3].value as SqlProcedureDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterProcedureStatement.Position = yyloc;
			yyval.value = sqlAlterProcedureStatement;
			break;
		}
		case 1053:
		{
			SqlProcedureDefinition sqlProcedureDefinition = new SqlProcedureDefinition(value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as SqlLiteralExpression, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 2].value != null);
			sqlProcedureDefinition.Position = yyloc;
			yyval.value = sqlProcedureDefinition;
			scanner.SetStmtDelimiter(value: true);
			break;
		}
		case 1056:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1060:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1062:
			yyval.value = "for replication";
			break;
		case 1064:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1065:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1066:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1067:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			break;
		}
		case 1068:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1069:
		{
			SqlExecuteAsClause sqlExecuteAsClause3 = value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause;
			sqlExecuteAsClause3.Position = yyloc;
			yyval.value = sqlExecuteAsClause3;
			break;
		}
		case 1070:
		{
			SqlExecuteAsClause sqlExecuteAsClause2;
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.Caller:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Caller();
				break;
			case StringToken.Owner:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Owner();
				break;
			case StringToken.Self:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Self();
				break;
			default:
				sqlExecuteAsClause2 = SqlExecuteAsClause.Invalid(GetText(location_stack.array[location_stack.top - 1]));
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
				break;
			}
			sqlExecuteAsClause2.Position = yyloc;
			yyval.value = sqlExecuteAsClause2;
			break;
		}
		case 1071:
		{
			SqlExecuteAsClause sqlExecuteAsClause = SqlExecuteAsClause.User(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlExecuteAsClause.Position = yyloc;
			yyval.value = sqlExecuteAsClause;
			break;
		}
		case 1050:
		case 1052:
		case 1054:
		case 1055:
		case 1057:
		case 1058:
		case 1059:
		case 1061:
		case 1063:
		case 1072:
		case 1073:
		case 1074:
		case 1075:
		case 1076:
		case 1077:
		case 1078:
		case 1079:
		case 1080:
		case 1081:
		case 1082:
		case 1083:
		case 1084:
		case 1085:
		case 1086:
		case 1087:
		case 1088:
		case 1089:
		case 1090:
		case 1091:
		case 1092:
		case 1093:
		case 1094:
		case 1095:
		case 1096:
		case 1097:
		case 1098:
		case 1099:
			break;
		}
	}

	private void DoAction_1100_1149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1150_1199(int action)
	{
		switch (action)
		{
		case 1183:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1150:
		case 1151:
		case 1152:
		case 1153:
		case 1154:
		case 1155:
		case 1156:
		case 1157:
		case 1158:
		case 1159:
		case 1160:
		case 1161:
		case 1162:
		case 1163:
		case 1164:
		case 1165:
		case 1166:
		case 1167:
		case 1168:
		case 1169:
		case 1170:
		case 1171:
		case 1172:
		case 1173:
		case 1174:
		case 1175:
		case 1176:
		case 1177:
		case 1178:
		case 1179:
		case 1180:
		case 1181:
		case 1182:
		case 1184:
		case 1185:
		case 1186:
		case 1187:
		case 1188:
		case 1189:
		case 1190:
		case 1191:
		case 1192:
		case 1193:
		case 1194:
		case 1195:
		case 1196:
		case 1197:
		case 1198:
		case 1199:
			break;
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		case 1224:
		{
			SqlCreateTypeStatement sqlCreateTypeStatement = SqlCreateTypeStatement.Create(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as CreateTypeStatementInfo);
			sqlCreateTypeStatement.Position = yyloc;
			yyval.value = sqlCreateTypeStatement;
			break;
		}
		case 1225:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification, (SqlUserDefinedDataTypeNullState)value_stack.array[value_stack.top - 1].value);
			break;
		case 1226:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 1].value as SqlClrClassSpecifier);
			break;
		case 1227:
			yyval.value = new CreateTypeStatementInfo(value_stack.array[value_stack.top - 2].value as SqlTableDefinition);
			break;
		case 1228:
			yyval = value_stack.array[value_stack.top - 1];
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		case 1229:
			yyval.value = SqlUserDefinedDataTypeNullState.None;
			break;
		case 1230:
			yyval.value = SqlUserDefinedDataTypeNullState.Null;
			break;
		case 1231:
			yyval.value = SqlUserDefinedDataTypeNullState.NotNull;
			break;
		case 1233:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1234:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier2 = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier2.Position = yyloc;
			yyval.value = sqlClrClassSpecifier2;
			break;
		}
		case 1235:
		{
			SqlClrClassSpecifier sqlClrClassSpecifier = new SqlClrClassSpecifier(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrClassSpecifier.Position = yyloc;
			yyval.value = sqlClrClassSpecifier;
			break;
		}
		case 1236:
		{
			SqlCreateFunctionStatement sqlCreateFunctionStatement = new SqlCreateFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition);
			sqlCreateFunctionStatement.Position = yyloc;
			yyval.value = sqlCreateFunctionStatement;
			break;
		}
		case 1237:
		{
			SqlAlterFunctionStatement sqlAlterFunctionStatement = new SqlAlterFunctionStatement(value_stack.array[value_stack.top - 1].value as SqlFunctionDefinition);
			sqlAlterFunctionStatement.Position = yyloc;
			yyval.value = sqlAlterFunctionStatement;
			break;
		}
		case 1238:
		{
			SqlFunctionDefinition sqlFunctionDefinition = SqlFunctionDefinition.CreateFromFactory(value_stack.array[value_stack.top - 1].value as FunctionDefinitionFactory, value_stack.array[value_stack.top - 6].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlParameterDeclaration>);
			sqlFunctionDefinition.Position = yyloc;
			yyval.value = sqlFunctionDefinition;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1239:
			yyval.value = FunctionDefinitionFactory.CreateScalarFactory(value_stack.array[value_stack.top - 4].value as SqlScalarFunctionReturnType, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1240:
			yyval.value = FunctionDefinitionFactory.CreateTableRelationalFactory(value_stack.array[value_stack.top - 4].value as FunctionVariableNameAndTableReturnTypePair, value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition);
			break;
		case 1241:
			yyval.value = FunctionDefinitionFactory.CreateTableClrFactory(value_stack.array[value_stack.top - 5].value as SqlTableFunctionReturnType, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlSimpleOrderByClause, value_stack.array[value_stack.top - 1].value as SqlClrFunctionBodyDefinition);
			break;
		case 1243:
		{
			SqlSimpleOrderByClause sqlSimpleOrderByClause = new SqlSimpleOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlSimpleOrderByItem>);
			sqlSimpleOrderByClause.Position = yyloc;
			yyval.value = sqlSimpleOrderByClause;
			break;
		}
		case 1245:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1246:
		{
			List<SqlModuleOption> list2 = new List<SqlModuleOption>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list2;
			break;
		}
		case 1247:
		{
			List<SqlModuleOption> list = value_stack.array[value_stack.top - 3].value as List<SqlModuleOption>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			yyval.value = list;
			break;
		}
		case 1200:
		case 1201:
		case 1202:
		case 1203:
		case 1204:
		case 1205:
		case 1206:
		case 1207:
		case 1208:
		case 1209:
		case 1210:
		case 1211:
		case 1212:
		case 1213:
		case 1214:
		case 1215:
		case 1216:
		case 1217:
		case 1218:
		case 1219:
		case 1220:
		case 1221:
		case 1222:
		case 1223:
		case 1232:
		case 1242:
		case 1244:
		case 1248:
		case 1249:
			break;
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1250:
		{
			SqlModuleOption sqlModuleOption4 = SqlModuleOption.ExecuteAs(value_stack.array[value_stack.top - 1].value as SqlExecuteAsClause);
			sqlModuleOption4.Position = yyloc;
			yyval.value = sqlModuleOption4;
			break;
		}
		case 1251:
		{
			SqlModuleOption sqlModuleOption3 = SqlModuleOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlModuleOption3.Position = yyloc;
			yyval.value = sqlModuleOption3;
			break;
		}
		case 1252:
		{
			SqlModuleOption sqlModuleOption2 = SqlModuleOption.CreateReturnsNullOnNullInputOption();
			sqlModuleOption2.Position = yyloc;
			yyval.value = sqlModuleOption2;
			ExpectKeyword(StringToken.Returns, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1253:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateCalledOnNullInputOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			ExpectKeyword(StringToken.Called, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Input, location_stack.array[location_stack.top - 1]);
			break;
		}
		case 1257:
		{
			SqlMultistatementFunctionBodyDefinition sqlMultistatementFunctionBodyDefinition = new SqlMultistatementFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlCompoundStatement);
			sqlMultistatementFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlMultistatementFunctionBodyDefinition;
			break;
		}
		case 1258:
		{
			SqlFunctionBodyDefinition sqlFunctionBodyDefinition = value_stack.array[value_stack.top - 1].value as SqlFunctionBodyDefinition;
			sqlFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlFunctionBodyDefinition;
			break;
		}
		case 1259:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(null, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 1261:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1262:
		{
			SqlClrFunctionBodyDefinition sqlClrFunctionBodyDefinition = new SqlClrFunctionBodyDefinition(value_stack.array[value_stack.top - 1].value as SqlClrMethodSpecifier);
			sqlClrFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlClrFunctionBodyDefinition;
			ExpectKeyword(StringToken.Name, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 1263:
		{
			SqlClrMethodSpecifier sqlClrMethodSpecifier = new SqlClrMethodSpecifier(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlClrMethodSpecifier.Position = yyloc;
			yyval.value = sqlClrMethodSpecifier;
			break;
		}
		case 1264:
		{
			SqlScalarFunctionReturnType sqlScalarFunctionReturnType = new SqlScalarFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlDataTypeSpecification);
			sqlScalarFunctionReturnType.Position = yyloc;
			yyval.value = sqlScalarFunctionReturnType;
			break;
		}
		case 1265:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 3]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 3];
			SqlTableFunctionReturnType sqlTableFunctionReturnType2 = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType2.Position = location_stack.array[location_stack.top - 1];
			yyval.value = new FunctionVariableNameAndTableReturnTypePair(sqlLiteralExpression, sqlTableFunctionReturnType2);
			break;
		}
		case 1267:
		{
			SqlTableFunctionReturnType sqlTableFunctionReturnType = new SqlTableFunctionReturnType(value_stack.array[value_stack.top - 1].value as SqlTableDefinition);
			sqlTableFunctionReturnType.Position = yyloc;
			yyval.value = sqlTableFunctionReturnType;
			break;
		}
		case 1271:
		{
			SqlTriggerDefinition sqlTriggerDefinition2 = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 1].value as SqlTriggerDefinition, isOrAlterStatement: false);
			sqlTriggerDefinition2.Position = yyloc;
			yyval.value = sqlTriggerDefinition2;
			break;
		}
		case 1272:
			yyval.value = value_stack.array[value_stack.top - 1].value;
			break;
		case 1273:
		{
			SqlCreateTriggerStatement sqlCreateTriggerStatement = new SqlCreateTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateTriggerStatement.Position = yyloc;
			yyval.value = sqlCreateTriggerStatement;
			break;
		}
		case 1275:
		{
			SqlAlterTriggerStatement sqlAlterTriggerStatement = new SqlAlterTriggerStatement(value_stack.array[value_stack.top - 3].value as SqlTriggerDefinition, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlAlterTriggerStatement.Position = yyloc;
			yyval.value = sqlAlterTriggerStatement;
			break;
		}
		case 1277:
			yyval.value = TriggerTargetData.CreateDmlTarget(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			break;
		case 1278:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.Database);
			break;
		case 1279:
			yyval.value = TriggerTargetData.CreateDdlTarget(SqlDdlTriggerTargetType.AllServer);
			ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			break;
		case 1280:
			yyval.value = TriggerActivationData.CreateDmlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerAction>);
			break;
		case 1281:
			yyval.value = TriggerActivationData.CreateDdlActivation((SqlTriggerActivationType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as List<SqlTriggerEvent>);
			break;
		case 1282:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1283:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1284:
		{
			List<SqlTriggerEvent> list4 = new List<SqlTriggerEvent>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list4;
			break;
		}
		case 1285:
		{
			List<SqlTriggerEvent> list3 = value_stack.array[value_stack.top - 3].value as List<SqlTriggerEvent>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerEvent);
			yyval.value = list3;
			break;
		}
		case 1286:
		{
			SqlTriggerEvent sqlTriggerEvent = new SqlTriggerEvent(GetText(location_stack.array[location_stack.top - 1]));
			sqlTriggerEvent.Position = yyloc;
			yyval.value = sqlTriggerEvent;
			break;
		}
		case 1287:
		{
			SqlTriggerDefinition sqlTriggerDefinition = SqlTriggerDefinition.Create(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 5].value as TriggerTargetData, value_stack.array[value_stack.top - 4].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as TriggerActivationData, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTriggerDefinition.Position = yyloc;
			yyval.value = sqlTriggerDefinition;
			break;
		}
		case 1288:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1290:
			yyval.value = false;
			break;
		case 1291:
			yyval.value = true;
			break;
		case 1292:
			yyval.value = SqlTriggerActivationType.For;
			break;
		case 1293:
			ExpectKeyword(StringToken.After, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlTriggerActivationType.After;
			break;
		case 1294:
			ExpectKeyword(StringToken.Instead, location_stack.array[location_stack.top - 2]);
			yyval.value = SqlTriggerActivationType.InsteadOf;
			break;
		case 1295:
		{
			List<SqlTriggerAction> list2 = new List<SqlTriggerAction>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list2;
			break;
		}
		case 1296:
		{
			List<SqlTriggerAction> list = value_stack.array[value_stack.top - 3].value as List<SqlTriggerAction>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTriggerAction);
			yyval.value = list;
			break;
		}
		case 1297:
		{
			SqlTriggerAction sqlTriggerAction3 = new SqlTriggerAction(SqlTriggerActionType.Insert);
			sqlTriggerAction3.Position = yyloc;
			yyval.value = sqlTriggerAction3;
			break;
		}
		case 1298:
		{
			SqlTriggerAction sqlTriggerAction2 = new SqlTriggerAction(SqlTriggerActionType.Update);
			sqlTriggerAction2.Position = yyloc;
			yyval.value = sqlTriggerAction2;
			break;
		}
		case 1299:
		{
			SqlTriggerAction sqlTriggerAction = new SqlTriggerAction(SqlTriggerActionType.Delete);
			sqlTriggerAction.Position = yyloc;
			yyval.value = sqlTriggerAction;
			break;
		}
		case 1254:
		case 1255:
		case 1256:
		case 1260:
		case 1266:
		case 1268:
		case 1269:
		case 1270:
		case 1274:
		case 1276:
		case 1289:
			break;
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		case 1300:
		{
			SqlCreateViewStatement sqlCreateViewStatement = new SqlCreateViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition);
			sqlCreateViewStatement.Position = yyloc;
			yyval.value = sqlCreateViewStatement;
			break;
		}
		case 1301:
		{
			SqlAlterViewStatement sqlAlterViewStatement = new SqlAlterViewStatement(value_stack.array[value_stack.top - 1].value as SqlViewDefinition);
			sqlAlterViewStatement.Position = yyloc;
			yyval.value = sqlAlterViewStatement;
			break;
		}
		case 1302:
		{
			SqlViewDefinition sqlViewDefinition = new SqlViewDefinition(value_stack.array[value_stack.top - 7].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 5].value as List<SqlModuleOption>, value_stack.array[value_stack.top - 3].value as SqlQueryWithClause, value_stack.array[value_stack.top - 2].value as SqlQueryExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlViewDefinition.Position = yyloc;
			yyval.value = sqlViewDefinition;
			break;
		}
		case 1304:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1305:
		{
			List<SqlIdentifier> list8 = new List<SqlIdentifier>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list8;
			break;
		}
		case 1306:
		{
			List<SqlIdentifier> list7 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list7;
			break;
		}
		case 1308:
			yyval.value = false;
			break;
		case 1309:
			yyval.value = true;
			break;
		case 1312:
		{
			SqlCreateSchemaStatement sqlCreateSchemaStatement = new SqlCreateSchemaStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlCreateSchemaStatement.Position = yyloc;
			yyval.value = sqlCreateSchemaStatement;
			break;
		}
		case 1317:
		{
			List<SqlStatement> list6 = new List<SqlStatement>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list6;
			break;
		}
		case 1318:
		{
			List<SqlStatement> list5 = value_stack.array[value_stack.top - 2].value as List<SqlStatement>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlStatement);
			yyval.value = list5;
			break;
		}
		case 1327:
		{
			SqlDropSchemaStatement sqlDropSchemaStatement2 = new SqlDropSchemaStatement(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlDropSchemaStatement2.Position = yyloc;
			yyval.value = sqlDropSchemaStatement2;
			break;
		}
		case 1328:
		{
			SqlIdentifier sqlIdentifier3 = SqlIdentifier.Missing();
			sqlIdentifier3.Position = location_stack.array[location_stack.top - 2];
			SqlDropSchemaStatement sqlDropSchemaStatement = new SqlDropSchemaStatement(sqlIdentifier3);
			sqlDropSchemaStatement.Position = yyloc;
			yyval.value = sqlDropSchemaStatement;
			break;
		}
		case 1329:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1330:
		{
			SqlDropDatabaseStatement sqlDropDatabaseStatement = new SqlDropDatabaseStatement(value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDropDatabaseStatement.Position = yyloc;
			yyval.value = sqlDropDatabaseStatement;
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		}
		case 1331:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1333:
		{
			List<SqlIdentifier> list4 = new List<SqlIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list4;
			break;
		}
		case 1334:
		{
			List<SqlIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list3;
			break;
		}
		case 1335:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(sqlIdentifier2);
			yyval.value = list2;
			break;
		}
		case 1336:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 3].value as List<SqlIdentifier>;
			list.Add(sqlIdentifier);
			yyval.value = list;
			break;
		}
		case 1337:
		{
			SqlDropTypeStatement sqlDropTypeStatement = new SqlDropTypeStatement(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlDropTypeStatement.Position = yyloc;
			yyval.value = sqlDropTypeStatement;
			break;
		}
		case 1303:
		case 1307:
		case 1310:
		case 1311:
		case 1313:
		case 1314:
		case 1315:
		case 1316:
		case 1319:
		case 1320:
		case 1321:
		case 1322:
		case 1323:
		case 1324:
		case 1325:
		case 1326:
		case 1332:
		case 1338:
		case 1339:
		case 1340:
		case 1341:
		case 1342:
		case 1343:
		case 1344:
		case 1345:
		case 1346:
		case 1347:
		case 1348:
		case 1349:
			break;
		}
	}

	private void DoAction_1350_1399(int action)
	{
		switch (action)
		{
		case 1359:
		{
			SqlDropStatement sqlDropStatement = SqlDropStatement.Create((SqlDropObjectType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as List<SqlObjectIdentifier>, (SqlDropTriggerTargetScope)value_stack.array[value_stack.top - 1].value);
			sqlDropStatement.Position = yyloc;
			yyval.value = sqlDropStatement;
			break;
		}
		case 1360:
			yyval.value = SqlDropTriggerTargetScope.None;
			break;
		case 1361:
			yyval.value = SqlDropTriggerTargetScope.Database;
			break;
		case 1362:
		{
			bool flag = ExpectKeyword(StringToken.Server, location_stack.array[location_stack.top - 1]);
			yyval.value = (flag ? SqlDropTriggerTargetScope.AllServer : SqlDropTriggerTargetScope.None);
			break;
		}
		case 1363:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1364:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1365:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1366:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1367:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1368:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1369:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1370:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1371:
			yyval.value = Enum.Parse(typeof(SqlDropObjectType), GetText(location_stack.array[location_stack.top - 1]), ignoreCase: true);
			break;
		case 1372:
			yyval.value = SqlDropObjectType.Sequence;
			break;
		case 1379:
		{
			List<SqlObjectIdentifier> list4 = new List<SqlObjectIdentifier>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list4;
			break;
		}
		case 1380:
		{
			List<SqlObjectIdentifier> list3 = value_stack.array[value_stack.top - 3].value as List<SqlObjectIdentifier>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			yyval.value = list3;
			break;
		}
		case 1385:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1386:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1387:
		{
			yyval = value_stack.array[value_stack.top - 1];
			SqlQueryWithClause sqlQueryWithClause5 = yyval.value as SqlQueryWithClause;
			sqlQueryWithClause5.Position = yyloc;
			yyval.value = sqlQueryWithClause5;
			break;
		}
		case 1388:
		{
			SqlQueryWithClause sqlQueryWithClause4 = new SqlQueryWithClause(value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause4.Position = yyloc;
			yyval.value = sqlQueryWithClause4;
			break;
		}
		case 1389:
		{
			SqlQueryWithClause sqlQueryWithClause3 = new SqlQueryWithClause(new SqlXmlNamespacesDeclaration
			{
				Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2)
			}, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause3.Position = yyloc;
			yyval.value = sqlQueryWithClause3;
			break;
		}
		case 1390:
		{
			SqlQueryWithClause sqlQueryWithClause2 = new SqlQueryWithClause(value_stack.array[value_stack.top - 2].value as SqlChangeTrackingContext, null, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause2.Position = yyloc;
			yyval.value = sqlQueryWithClause2;
			break;
		}
		case 1391:
		{
			SqlXmlNamespacesDeclaration sqlXmlNamespacesDeclaration = new SqlXmlNamespacesDeclaration();
			sqlXmlNamespacesDeclaration.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			SqlQueryWithClause sqlQueryWithClause = new SqlQueryWithClause(value_stack.array[value_stack.top - 7].value as SqlChangeTrackingContext, sqlXmlNamespacesDeclaration, value_stack.array[value_stack.top - 1].value as List<SqlCommonTableExpression>);
			sqlQueryWithClause.Position = yyloc;
			yyval.value = sqlQueryWithClause;
			break;
		}
		case 1396:
		{
			List<SqlCommonTableExpression> list2 = new List<SqlCommonTableExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list2;
			break;
		}
		case 1397:
		{
			List<SqlCommonTableExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlCommonTableExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlCommonTableExpression);
			yyval.value = list;
			break;
		}
		case 1398:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 1399:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 1350:
		case 1351:
		case 1352:
		case 1353:
		case 1354:
		case 1355:
		case 1356:
		case 1357:
		case 1358:
		case 1373:
		case 1374:
		case 1375:
		case 1376:
		case 1377:
		case 1378:
		case 1381:
		case 1382:
		case 1383:
		case 1384:
		case 1392:
		case 1393:
		case 1394:
		case 1395:
			break;
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
		case 1400:
		{
			SqlCommonTableExpression sqlCommonTableExpression = new SqlCommonTableExpression(value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 5].value as List<SqlIdentifier>, value_stack.array[value_stack.top - 2].value as SqlQueryExpression);
			sqlCommonTableExpression.Position = yyloc;
			yyval.value = sqlCommonTableExpression;
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		}
		case 1401:
		{
			SqlChangeTrackingContext sqlChangeTrackingContext = new SqlChangeTrackingContext();
			sqlChangeTrackingContext.Position = yyloc;
			yyval.value = sqlChangeTrackingContext;
			break;
		}
		case 1402:
		{
			SqlInsertStatement sqlInsertStatement2 = new SqlInsertStatement(value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement2.Position = yyloc;
			yyval.value = sqlInsertStatement2;
			break;
		}
		case 1403:
		{
			SqlInsertStatement sqlInsertStatement = new SqlInsertStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlInsertSpecification);
			sqlInsertStatement.Position = yyloc;
			yyval.value = sqlInsertStatement;
			break;
		}
		case 1404:
		{
			SqlInsertSpecification sqlInsertSpecification = new SqlInsertSpecification(value_stack.array[value_stack.top - 5].value as SqlTopSpecification, value_stack.array[value_stack.top - 3].value as DmlTarget, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 1].value as SqlInsertSource);
			sqlInsertSpecification.Position = yyloc;
			yyval.value = sqlInsertSpecification;
			break;
		}
		case 1409:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1410:
		{
			List<SqlColumnRefExpression> list8 = new List<SqlColumnRefExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list8;
			break;
		}
		case 1411:
		{
			List<SqlColumnRefExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list7;
			break;
		}
		case 1412:
		{
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier5.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier5);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1413:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier4, discardColumnPrefix: true);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 1415:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(sqlIdentifier2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier3);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1416:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(sqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier2, discardColumnPrefix: true);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 1417:
		{
			SqlDefaultValuesInsertSource sqlDefaultValuesInsertSource = new SqlDefaultValuesInsertSource();
			sqlDefaultValuesInsertSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertSource;
			break;
		}
		case 1418:
		{
			SqlTableConstructorInsertSource sqlTableConstructorInsertSource = new SqlTableConstructorInsertSource(value_stack.array[value_stack.top - 2].value as SqlTableConstructorExpression);
			sqlTableConstructorInsertSource.Position = yyloc;
			yyval.value = sqlTableConstructorInsertSource;
			break;
		}
		case 1419:
		{
			SqlSelectSpecificationInsertSource sqlSelectSpecificationInsertSource = new SqlSelectSpecificationInsertSource(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectSpecificationInsertSource.Position = yyloc;
			yyval.value = sqlSelectSpecificationInsertSource;
			break;
		}
		case 1420:
		{
			SqlNullInsertSource sqlNullInsertSource = new SqlNullInsertSource();
			sqlNullInsertSource.Position = yyloc;
			yyval.value = sqlNullInsertSource;
			break;
		}
		case 1421:
		{
			SqlMissingInsertSource sqlMissingInsertSource = new SqlMissingInsertSource();
			sqlMissingInsertSource.Position = yyloc;
			yyval.value = sqlMissingInsertSource;
			break;
		}
		case 1422:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 1423:
		{
			List<SqlScalarExpression> list5 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 1428:
		{
			List<SqlOutputClause> list4 = new List<SqlOutputClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list4;
			break;
		}
		case 1429:
		{
			List<SqlOutputClause> list3 = new List<SqlOutputClause>();
			list3.Add(value_stack.array[value_stack.top - 2].value as SqlOutputClause);
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlOutputClause);
			yyval.value = list3;
			break;
		}
		case 1430:
		{
			SqlOutputIntoClause sqlOutputIntoClause = new SqlOutputIntoClause(value_stack.array[value_stack.top - 2].value as SqlOutputClause, value_stack.array[value_stack.top - 1].value as SqlIntoClause);
			sqlOutputIntoClause.Position = yyloc;
			yyval.value = sqlOutputIntoClause;
			break;
		}
		case 1433:
		{
			SqlOutputClause sqlOutputClause = new SqlOutputClause(value_stack.array[value_stack.top - 1].value as List<SqlSelectExpression>);
			sqlOutputClause.Position = yyloc;
			yyval.value = sqlOutputClause;
			break;
		}
		case 1434:
		{
			List<SqlSelectExpression> list2 = new List<SqlSelectExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list2;
			break;
		}
		case 1435:
		{
			List<SqlSelectExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list;
			break;
		}
		case 1436:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 1437:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 1441:
		{
			SqlIntoClause sqlIntoClause2 = new SqlIntoClause(new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause2.Position = yyloc;
			yyval.value = sqlIntoClause2;
			break;
		}
		case 1442:
		{
			SqlIntoClause sqlIntoClause = new SqlIntoClause(new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier)
			{
				Position = location_stack.array[location_stack.top - 2]
			}, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			sqlIntoClause.Position = yyloc;
			yyval.value = sqlIntoClause;
			break;
		}
		case 1405:
		case 1406:
		case 1407:
		case 1408:
		case 1414:
		case 1424:
		case 1425:
		case 1426:
		case 1427:
		case 1431:
		case 1432:
		case 1438:
		case 1439:
		case 1440:
		case 1443:
		case 1444:
		case 1445:
		case 1446:
		case 1447:
		case 1448:
		case 1449:
			break;
		}
	}

	private void DoAction_1450_1499(int action)
	{
		switch (action)
		{
		case 1467:
		{
			List<SqlSimpleOrderByItem> list = new List<SqlSimpleOrderByItem>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list;
			break;
		}
		case 1468:
		{
			List<SqlSimpleOrderByItem> list6 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleOrderByItem>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleOrderByItem);
			yyval.value = list6;
			break;
		}
		case 1469:
		{
			SqlSimpleOrderByItem sqlSimpleOrderByItem = new SqlSimpleOrderByItem(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlSimpleOrderByItem.Position = yyloc;
			yyval.value = sqlSimpleOrderByItem;
			break;
		}
		case 1470:
			VersionedParserBase.ReportError(SR.MergeStatementMustTerminated, new LexLocation(location_stack.array[location_stack.top - 1].endTokenNumber, location_stack.array[location_stack.top - 1].endTokenNumber));
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1471:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1472:
		{
			SqlMergeStatement sqlMergeStatement2 = new SqlMergeStatement(value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement2.Position = yyloc;
			yyval.value = sqlMergeStatement2;
			break;
		}
		case 1473:
		{
			SqlMergeStatement sqlMergeStatement = new SqlMergeStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlMergeSpecification);
			sqlMergeStatement.Position = yyloc;
			yyval.value = sqlMergeStatement;
			break;
		}
		case 1474:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression);
			sqlConditionClause.Position = LexLocation.Merge(location_stack.array[location_stack.top - 5], location_stack.array[location_stack.top - 4]);
			SqlMergeSpecification sqlMergeSpecification = new SqlMergeSpecification(value_stack.array[value_stack.top - 11].value as SqlTopSpecification, value_stack.array[value_stack.top - 9].value as DmlTarget, value_stack.array[value_stack.top - 8].value as SqlIdentifier, value_stack.array[value_stack.top - 6].value as SqlTableExpression, sqlConditionClause, value_stack.array[value_stack.top - 3].value as List<SqlMergeActionClause>, value_stack.array[value_stack.top - 2].value as List<SqlOutputClause>);
			sqlMergeSpecification.Position = yyloc;
			yyval.value = sqlMergeSpecification;
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		}
		case 1479:
		{
			List<SqlMergeActionClause> list5 = new List<SqlMergeActionClause>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list5;
			break;
		}
		case 1480:
		{
			List<SqlMergeActionClause> list4 = value_stack.array[value_stack.top - 2].value as List<SqlMergeActionClause>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlMergeActionClause);
			yyval.value = list4;
			break;
		}
		case 1481:
		{
			SqlMergeActionClause sqlMergeActionClause2 = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause2.Position = yyloc;
			yyval.value = sqlMergeActionClause2;
			break;
		}
		case 1482:
		{
			SqlMergeActionClause sqlMergeActionClause = new SqlMergeActionClause((SqlMergeConditionType)value_stack.array[value_stack.top - 5].value, value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlMergeAction);
			sqlMergeActionClause.Position = yyloc;
			yyval.value = sqlMergeActionClause;
			break;
		}
		case 1483:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.Matched;
			break;
		case 1484:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 1]);
			yyval.value = SqlMergeConditionType.TargetNotMatched;
			break;
		case 1485:
			ExpectKeyword(StringToken.Matched, location_stack.array[location_stack.top - 3]);
			switch (GetStringToken(location_stack.array[location_stack.top - 1]))
			{
			case StringToken.Source:
				yyval.value = SqlMergeConditionType.SourceNotMatched;
				break;
			case StringToken.Target:
				yyval.value = SqlMergeConditionType.TargetNotMatched;
				break;
			default:
				yyval.value = SqlMergeConditionType.Error;
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
				break;
			}
			break;
		case 1486:
		{
			SqlUpdateMergeAction sqlUpdateMergeAction = new SqlUpdateMergeAction(new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>)
			{
				Position = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1])
			});
			sqlUpdateMergeAction.Position = yyloc;
			yyval.value = sqlUpdateMergeAction;
			break;
		}
		case 1487:
		{
			SqlInsertMergeAction sqlInsertMergeAction = new SqlInsertMergeAction(value_stack.array[value_stack.top - 2].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 1].value as SqlInsertMergeActionSource);
			sqlInsertMergeAction.Position = yyloc;
			yyval.value = sqlInsertMergeAction;
			break;
		}
		case 1488:
		{
			SqlDeleteMergeAction sqlDeleteMergeAction = new SqlDeleteMergeAction();
			sqlDeleteMergeAction.Position = yyloc;
			yyval.value = sqlDeleteMergeAction;
			break;
		}
		case 1489:
		{
			SqlDefaultValuesInsertMergeActionSource sqlDefaultValuesInsertMergeActionSource = new SqlDefaultValuesInsertMergeActionSource();
			sqlDefaultValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlDefaultValuesInsertMergeActionSource;
			break;
		}
		case 1490:
		{
			SqlValuesInsertMergeActionSource sqlValuesInsertMergeActionSource = new SqlValuesInsertMergeActionSource(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlValuesInsertMergeActionSource.Position = yyloc;
			yyval.value = sqlValuesInsertMergeActionSource;
			break;
		}
		case 1491:
		{
			SqlUpdateStatement sqlUpdateStatement2 = new SqlUpdateStatement(value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement2.Position = yyloc;
			yyval.value = sqlUpdateStatement2;
			break;
		}
		case 1492:
		{
			SqlUpdateStatement sqlUpdateStatement = new SqlUpdateStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlUpdateSpecification);
			sqlUpdateStatement.Position = yyloc;
			yyval.value = sqlUpdateStatement;
			break;
		}
		case 1493:
		{
			SqlUpdateSpecification sqlUpdateSpecification = new SqlUpdateSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as DmlTarget, value_stack.array[value_stack.top - 5].value as SqlSetClause, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlUpdateSpecification.Position = yyloc;
			yyval.value = sqlUpdateSpecification;
			break;
		}
		case 1494:
		{
			List<SqlAssignment> list3 = new List<SqlAssignment>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list3;
			break;
		}
		case 1495:
		{
			List<SqlAssignment> list2 = value_stack.array[value_stack.top - 3].value as List<SqlAssignment>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlAssignment);
			yyval.value = list2;
			break;
		}
		case 1496:
			yyval = value_stack.array[value_stack.top - 1];
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullAssignment sqlNullAssignment = new SqlNullAssignment(ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), yyloc));
				sqlNullAssignment.Position = yyloc;
				yyval.value = sqlNullAssignment;
			}
			break;
		case 1497:
		{
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 1498:
		{
			SqlVariableColumnAssignment sqlVariableColumnAssignment = new SqlVariableColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 1].value as SqlColumnAssignment);
			sqlVariableColumnAssignment.Position = yyloc;
			yyval.value = sqlVariableColumnAssignment;
			break;
		}
		case 1499:
		{
			SqlColumnAssignment sqlColumnAssignment = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlColumnRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment.Position = yyloc;
			yyval.value = sqlColumnAssignment;
			break;
		}
		case 1450:
		case 1451:
		case 1452:
		case 1453:
		case 1454:
		case 1455:
		case 1456:
		case 1457:
		case 1458:
		case 1459:
		case 1460:
		case 1461:
		case 1462:
		case 1463:
		case 1464:
		case 1465:
		case 1466:
		case 1475:
		case 1476:
		case 1477:
		case 1478:
			break;
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
		case 1500:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 1501:
		{
			List<SqlIdentifier> obj3 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier6 = SqlObjectIdentifier.Create(obj3);
			sqlObjectIdentifier6.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression4 = new SqlColumnRefExpression(sqlObjectIdentifier6);
			sqlColumnRefExpression4.Position = yyloc;
			yyval.value = sqlColumnRefExpression4;
			break;
		}
		case 1504:
			yyval.value = SqlAssignmentOperatorType.Assignment;
			break;
		case 1505:
			yyval.value = SqlAssignmentOperatorType.AdditionAssignment;
			break;
		case 1506:
			yyval.value = SqlAssignmentOperatorType.SubtractionAssignment;
			break;
		case 1507:
			yyval.value = SqlAssignmentOperatorType.MultiplicationAssignment;
			break;
		case 1508:
			yyval.value = SqlAssignmentOperatorType.DivisionAssignment;
			break;
		case 1509:
			yyval.value = SqlAssignmentOperatorType.ModulusAssignment;
			break;
		case 1510:
			yyval.value = SqlAssignmentOperatorType.BitwiseAndAssignment;
			break;
		case 1511:
			yyval.value = SqlAssignmentOperatorType.BitwiseOrAssignment;
			break;
		case 1512:
			yyval.value = SqlAssignmentOperatorType.BitwiseXorAssignment;
			break;
		case 1513:
		{
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier5.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier5);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 1514:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier4.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression2 = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier4);
			sqlScalarRefExpression2.Position = yyloc;
			yyval.value = sqlScalarRefExpression2;
			break;
		}
		case 1516:
		{
			SqlIdentifier sqlIdentifier2 = SqlIdentifier.Missing();
			sqlIdentifier2.Position = yyloc;
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(sqlIdentifier2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier3);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 1517:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 1];
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(sqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier2.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier2);
			sqlScalarRefExpression.Position = yyloc;
			yyval.value = sqlScalarRefExpression;
			break;
		}
		case 1522:
		{
			SqlColumnAssignment sqlColumnAssignment = new SqlColumnAssignment(value_stack.array[value_stack.top - 3].value as SqlScalarRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlColumnAssignment.Position = yyloc;
			yyval.value = sqlColumnAssignment;
			break;
		}
		case 1523:
		{
			SqlSelectStatement sqlSelectStatement2 = new SqlSelectStatement(value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement2.Position = yyloc;
			yyval.value = sqlSelectStatement2;
			break;
		}
		case 1524:
		{
			SqlSelectStatement sqlSelectStatement = new SqlSelectStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlSelectSpecification);
			sqlSelectStatement.Position = yyloc;
			yyval.value = sqlSelectStatement;
			break;
		}
		case 1525:
		{
			SqlSelectSpecification sqlSelectSpecification = new SqlSelectSpecification(value_stack.array[value_stack.top - 3].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlSelectSpecification.Position = yyloc;
			yyval.value = sqlSelectSpecification;
			break;
		}
		case 1527:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 1528:
			yyval.value = SqlBinaryQueryOperatorType.Union;
			break;
		case 1529:
			yyval.value = SqlBinaryQueryOperatorType.Except;
			break;
		case 1531:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 1532:
			yyval.value = false;
			break;
		case 1533:
			yyval.value = true;
			break;
		case 1535:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 1537:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 8].value != null, value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 6].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 9, location_stack.top - 6)
			}, value_stack.array[value_stack.top - 5].value as SqlSelectIntoClause, value_stack.array[value_stack.top - 4].value as SqlFromClause, value_stack.array[value_stack.top - 3].value as SqlWhereClause, value_stack.array[value_stack.top - 2].value as SqlGroupByClause, value_stack.array[value_stack.top - 1].value as SqlHavingClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 1539:
		{
			SqlSelectIntoClause sqlSelectIntoClause = new SqlSelectIntoClause(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlSelectIntoClause.Position = yyloc;
			yyval.value = sqlSelectIntoClause;
			break;
		}
		case 1542:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1543:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1544:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 1502:
		case 1503:
		case 1515:
		case 1518:
		case 1519:
		case 1520:
		case 1521:
		case 1526:
		case 1530:
		case 1534:
		case 1536:
		case 1538:
		case 1540:
		case 1541:
		case 1545:
		case 1546:
		case 1547:
		case 1548:
		case 1549:
			break;
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		case 1576:
			if (ExpectKeyword(StringToken.Xml, location_stack.array[location_stack.top - 2]))
			{
				StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 2]);
				SqlForXmlClause sqlForXmlClause = SqlForXmlClause.Create(value_stack.array[value_stack.top - 1].value as List<SqlForXmlUnknownDirective>, stringToken);
				sqlForXmlClause.Position = yyloc;
				yyval.value = sqlForXmlClause;
			}
			break;
		case 1577:
		{
			List<SqlForXmlUnknownDirective> list2 = new List<SqlForXmlUnknownDirective>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list2;
			break;
		}
		case 1578:
		{
			List<SqlForXmlUnknownDirective> list = value_stack.array[value_stack.top - 3].value as List<SqlForXmlUnknownDirective>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlForXmlUnknownDirective);
			yyval.value = list;
			break;
		}
		case 1579:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective3 = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective3.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective3;
			break;
		}
		case 1580:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective2 = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]), null);
			sqlForXmlUnknownDirective2.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective2;
			break;
		}
		case 1581:
		{
			SqlForXmlUnknownDirective sqlForXmlUnknownDirective = new SqlForXmlUnknownDirective(GetText(location_stack.array[location_stack.top - 4]), GetText(location_stack.array[location_stack.top - 2]));
			sqlForXmlUnknownDirective.Position = yyloc;
			yyval.value = sqlForXmlUnknownDirective;
			break;
		}
		case 1588:
			yyval.value = true;
			break;
		case 1590:
		{
			SqlOffsetFetchClause sqlOffsetFetchClause = new SqlOffsetFetchClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlOffsetFetchClause.Position = yyloc;
			yyval.value = sqlOffsetFetchClause;
			ExpectKeyword(StringToken.Offset, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 1592:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.First,
				StringToken.Next
			}, location_stack.array[location_stack.top - 4]);
			ExpectKeyword(StringToken.Only, location_stack.array[location_stack.top - 1]);
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1593:
			ExpectKeywords(new StringToken[2]
			{
				StringToken.Row,
				StringToken.Rows
			}, location_stack.array[location_stack.top - 1]);
			break;
		case 1594:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1595:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1597:
		{
			SqlTopSpecification sqlTopSpecification = new SqlTopSpecification(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 2].value, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification.Position = yyloc;
			yyval.value = sqlTopSpecification;
			break;
		}
		case 1598:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 1599:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1550:
		case 1551:
		case 1552:
		case 1553:
		case 1554:
		case 1555:
		case 1556:
		case 1557:
		case 1558:
		case 1559:
		case 1560:
		case 1561:
		case 1562:
		case 1563:
		case 1564:
		case 1565:
		case 1566:
		case 1567:
		case 1568:
		case 1569:
		case 1570:
		case 1571:
		case 1572:
		case 1573:
		case 1574:
		case 1575:
		case 1582:
		case 1583:
		case 1584:
		case 1585:
		case 1586:
		case 1587:
		case 1589:
		case 1591:
		case 1596:
			break;
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		case 1600:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 1603:
		{
			SqlTopSpecification sqlTopSpecification = new SqlTopSpecification(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 1].value);
			sqlTopSpecification.Position = yyloc;
			yyval.value = sqlTopSpecification;
			break;
		}
		case 1604:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 1605:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1606:
			yyval.value = false;
			break;
		case 1607:
			yyval.value = true;
			break;
		case 1608:
			yyval.value = false;
			break;
		case 1609:
			yyval.value = true;
			break;
		case 1610:
		{
			SqlDeleteStatement sqlDeleteStatement2 = new SqlDeleteStatement(value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement2.Position = yyloc;
			yyval.value = sqlDeleteStatement2;
			break;
		}
		case 1611:
		{
			SqlDeleteStatement sqlDeleteStatement = new SqlDeleteStatement(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlDeleteSpecification);
			sqlDeleteStatement.Position = yyloc;
			yyval.value = sqlDeleteStatement;
			break;
		}
		case 1612:
		{
			SqlDeleteSpecification sqlDeleteSpecification = new SqlDeleteSpecification(value_stack.array[value_stack.top - 7].value as SqlTopSpecification, value_stack.array[value_stack.top - 5].value as DmlTarget, value_stack.array[value_stack.top - 4].value as List<SqlOutputClause>, value_stack.array[value_stack.top - 3].value as SqlFromClause, value_stack.array[value_stack.top - 2].value as SqlWhereClause);
			sqlDeleteSpecification.Position = yyloc;
			yyval.value = sqlDeleteSpecification;
			break;
		}
		case 1615:
			if (value_stack.array[value_stack.top - 1].value is SqlExecuteStatement sqlExecuteStatement)
			{
				sqlExecuteStatement.Position = yyloc;
				yyval.value = sqlExecuteStatement;
			}
			break;
		case 1616:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement2 = value_stack.array[value_stack.top - 5].value as SqlExecuteStringStatement;
			sqlExecuteStringStatement2.Position = yyloc;
			yyval.value = sqlExecuteStringStatement2;
			break;
		}
		case 1618:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement2 = value_stack.array[value_stack.top - 1].value as SqlExecuteModuleStatement;
			sqlExecuteModuleStatement2.ReturnValue = ShiftReduceParser.UpdatePos(SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string), location_stack.array[location_stack.top - 3]);
			sqlExecuteModuleStatement2.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement2;
			break;
		}
		case 1624:
		{
			SqlExecuteStringStatement sqlExecuteStringStatement = new SqlExecuteStringStatement(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 1].value as List<SqlExecuteArgument>);
			sqlExecuteStringStatement.Position = yyloc;
			yyval.value = sqlExecuteStringStatement;
			break;
		}
		case 1628:
		{
			SqlExecuteModuleStatement sqlExecuteModuleStatement = new SqlExecuteModuleStatement(value_stack.array[value_stack.top - 4].value as SqlObjectReference, value_stack.array[value_stack.top - 3].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as List<SqlExecuteArgument>, value_stack.array[value_stack.top - 1].value as SqlModuleOption);
			sqlExecuteModuleStatement.Position = yyloc;
			yyval.value = sqlExecuteModuleStatement;
			break;
		}
		case 1631:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 1632:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 1633:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 1634:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 1638:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1641:
		{
			List<SqlExecuteArgument> list2 = new List<SqlExecuteArgument>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list2;
			break;
		}
		case 1642:
		{
			List<SqlExecuteArgument> list = value_stack.array[value_stack.top - 3].value as List<SqlExecuteArgument>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlExecuteArgument);
			yyval.value = list;
			break;
		}
		case 1643:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 4]));
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 4];
			SqlExecuteArgument sqlExecuteArgument2 = new SqlExecuteArgument(sqlScalarVariableRefExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument2.Position = yyloc;
			yyval.value = sqlExecuteArgument2;
			break;
		}
		case 1644:
		{
			SqlExecuteArgument sqlExecuteArgument = new SqlExecuteArgument(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlExecuteArgument.Position = yyloc;
			yyval.value = sqlExecuteArgument;
			break;
		}
		case 1601:
		case 1602:
		case 1613:
		case 1614:
		case 1617:
		case 1619:
		case 1620:
		case 1621:
		case 1622:
		case 1623:
		case 1625:
		case 1626:
		case 1627:
		case 1629:
		case 1630:
		case 1635:
		case 1636:
		case 1637:
		case 1639:
		case 1640:
		case 1645:
		case 1646:
		case 1647:
		case 1648:
		case 1649:
			break;
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1650:
		{
			SqlModuleOption sqlModuleOption = SqlModuleOption.CreateRecompileOption();
			sqlModuleOption.Position = yyloc;
			yyval.value = sqlModuleOption;
			break;
		}
		case 1674:
		{
			SqlVariableDeclareStatement sqlVariableDeclareStatement = new SqlVariableDeclareStatement(value_stack.array[value_stack.top - 1].value as List<SqlVariableDeclaration>);
			sqlVariableDeclareStatement.Position = yyloc;
			yyval.value = sqlVariableDeclareStatement;
			break;
		}
		case 1675:
		{
			SqlCursorDeclareStatement sqlCursorDeclareStatement = new SqlCursorDeclareStatement(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>, value_stack.array[value_stack.top - 1].value as CursorDefinitionInfo);
			sqlCursorDeclareStatement.Position = yyloc;
			yyval.value = sqlCursorDeclareStatement;
			break;
		}
		case 1677:
		{
			List<SqlVariableDeclaration> list3 = new List<SqlVariableDeclaration>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list3;
			break;
		}
		case 1678:
		{
			List<SqlVariableDeclaration> list2 = value_stack.array[value_stack.top - 3].value as List<SqlVariableDeclaration>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlVariableDeclaration);
			yyval.value = list2;
			break;
		}
		case 1679:
		{
			SqlVariableDeclaration sqlVariableDeclaration2 = new SqlVariableDeclaration(GetText(location_stack.array[location_stack.top - 5]), value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlVariableDeclaration2.Position = yyloc;
			yyval.value = sqlVariableDeclaration2;
			break;
		}
		case 1680:
		{
			SqlVariableDeclaration sqlVariableDeclaration = SqlVariableDeclaration.Create(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			sqlVariableDeclaration.Position = yyloc;
			yyval.value = sqlVariableDeclaration;
			break;
		}
		case 1682:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1683:
			yyval.value = new CursorDefinitionInfo(value_stack.array[value_stack.top - 1].value as SqlSelectStatement, value_stack.array[value_stack.top - 3].value as List<SqlCursorOption>);
			break;
		case 1684:
			yyval.value = new List<SqlCursorOption>();
			break;
		case 1685:
		{
			SqlCursorOption sqlCursorOption = new SqlCursorOption(GetStringToken(location_stack.array[location_stack.top - 1]));
			sqlCursorOption.Position = location_stack.array[location_stack.top - 1];
			List<SqlCursorOption> list = value_stack.array[value_stack.top - 2].value as List<SqlCursorOption>;
			list.Add(sqlCursorOption);
			yyval.value = list;
			break;
		}
		case 1651:
		case 1652:
		case 1653:
		case 1654:
		case 1655:
		case 1656:
		case 1657:
		case 1658:
		case 1659:
		case 1660:
		case 1661:
		case 1662:
		case 1663:
		case 1664:
		case 1665:
		case 1666:
		case 1667:
		case 1668:
		case 1669:
		case 1670:
		case 1671:
		case 1672:
		case 1673:
		case 1676:
		case 1681:
		case 1686:
		case 1687:
		case 1688:
		case 1689:
		case 1690:
		case 1691:
		case 1692:
		case 1693:
		case 1694:
		case 1695:
		case 1696:
		case 1697:
		case 1698:
		case 1699:
			break;
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1703:
		{
			SqlReturnStatement sqlReturnStatement = new SqlReturnStatement(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlReturnStatement.Position = yyloc;
			yyval.value = sqlReturnStatement;
			break;
		}
		case 1706:
		{
			SqlIfElseStatement sqlIfElseStatement2 = new SqlIfElseStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement2.Position = yyloc;
			yyval.value = sqlIfElseStatement2;
			break;
		}
		case 1707:
		{
			SqlIfElseStatement sqlIfElseStatement = new SqlIfElseStatement(value_stack.array[value_stack.top - 4].value as SqlBooleanExpression, value_stack.array[value_stack.top - 3].value as SqlStatement, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlIfElseStatement.Position = yyloc;
			yyval.value = sqlIfElseStatement;
			break;
		}
		case 1708:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1710:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1711:
		{
			SqlWhileStatement sqlWhileStatement = new SqlWhileStatement(value_stack.array[value_stack.top - 2].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlStatement);
			sqlWhileStatement.Position = yyloc;
			yyval.value = sqlWhileStatement;
			break;
		}
		case 1712:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1713:
		{
			SqlBreakStatement sqlBreakStatement = new SqlBreakStatement();
			sqlBreakStatement.Position = yyloc;
			yyval.value = sqlBreakStatement;
			break;
		}
		case 1714:
		{
			SqlContinueStatement sqlContinueStatement = new SqlContinueStatement();
			sqlContinueStatement.Position = yyloc;
			yyval.value = sqlContinueStatement;
			break;
		}
		case 1716:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1717:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 1718:
		{
			SqlBackupDatabaseStatement sqlBackupDatabaseStatement = SqlBackupDatabaseStatement.Create(isDumpLoadStatement);
			sqlBackupDatabaseStatement.Position = yyloc;
			yyval.value = sqlBackupDatabaseStatement;
			break;
		}
		case 1719:
		{
			SqlBackupTableStatement sqlBackupTableStatement = SqlBackupTableStatement.Create(isDumpLoadStatement);
			sqlBackupTableStatement.Position = yyloc;
			yyval.value = sqlBackupTableStatement;
			break;
		}
		case 1720:
		{
			SqlBackupLogStatement sqlBackupLogStatement2 = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: false);
			sqlBackupLogStatement2.Position = yyloc;
			yyval.value = sqlBackupLogStatement2;
			break;
		}
		case 1721:
		{
			SqlBackupLogStatement sqlBackupLogStatement = SqlBackupLogStatement.Create(isDumpLoadStatement, isDumpTransaction: true);
			sqlBackupLogStatement.Position = yyloc;
			yyval.value = sqlBackupLogStatement;
			break;
		}
		case 1700:
		case 1701:
		case 1702:
		case 1704:
		case 1705:
		case 1709:
		case 1715:
		case 1722:
		case 1723:
		case 1724:
		case 1725:
		case 1726:
		case 1727:
		case 1728:
		case 1729:
		case 1730:
		case 1731:
		case 1732:
		case 1733:
		case 1734:
		case 1735:
		case 1736:
		case 1737:
		case 1738:
		case 1739:
		case 1740:
		case 1741:
		case 1742:
		case 1743:
		case 1744:
		case 1745:
		case 1746:
		case 1747:
		case 1748:
		case 1749:
			break;
		}
	}

	private void DoAction_1750_1799(int action)
	{
		switch (action)
		{
		case 1761:
		{
			SqlBackupRestoreStatement sqlBackupRestoreStatement = value_stack.array[value_stack.top - 3].value as SqlBackupRestoreStatement;
			sqlBackupRestoreStatement.Position = yyloc;
			yyval.value = sqlBackupRestoreStatement;
			break;
		}
		case 1762:
		{
			SqlRestoreDatabaseStatement sqlRestoreDatabaseStatement = SqlRestoreDatabaseStatement.Create(isDumpLoadStatement);
			sqlRestoreDatabaseStatement.Position = yyloc;
			yyval.value = sqlRestoreDatabaseStatement;
			break;
		}
		case 1763:
		{
			SqlRestoreTableStatement sqlRestoreTableStatement = SqlRestoreTableStatement.Create(isDumpLoadStatement);
			sqlRestoreTableStatement.Position = yyloc;
			yyval.value = sqlRestoreTableStatement;
			break;
		}
		case 1764:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement2 = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: false);
			sqlRestoreLogStatement2.Position = yyloc;
			yyval.value = sqlRestoreLogStatement2;
			break;
		}
		case 1765:
		{
			SqlRestoreLogStatement sqlRestoreLogStatement = SqlRestoreLogStatement.Create(isDumpLoadStatement, isLoadTransaction: true);
			sqlRestoreLogStatement.Position = yyloc;
			yyval.value = sqlRestoreLogStatement;
			break;
		}
		case 1780:
		{
			SqlRestoreInformationStatement sqlRestoreInformationStatement = SqlRestoreInformationStatement.Create(isDumpLoadStatement);
			sqlRestoreInformationStatement.Position = yyloc;
			yyval.value = sqlRestoreInformationStatement;
			break;
		}
		case 1784:
		{
			SqlGdrStatement sqlGdrStatement = SqlGdrStatement.Create((SqlGdrStatementType)value_stack.array[value_stack.top - 8].value);
			sqlGdrStatement.Position = yyloc;
			yyval.value = sqlGdrStatement;
			break;
		}
		case 1787:
			scanner.SetGDRState();
			break;
		case 1788:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlGrant;
			break;
		case 1789:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlDeny;
			break;
		case 1790:
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 1791:
			scanner.SetGDRState();
			yyval.value = SqlGdrStatementType.SqlRevoke;
			break;
		case 1792:
			scanner.SetGDRState();
			break;
		case 1750:
		case 1751:
		case 1752:
		case 1753:
		case 1754:
		case 1755:
		case 1756:
		case 1757:
		case 1758:
		case 1759:
		case 1760:
		case 1766:
		case 1767:
		case 1768:
		case 1769:
		case 1770:
		case 1771:
		case 1772:
		case 1773:
		case 1774:
		case 1775:
		case 1776:
		case 1777:
		case 1778:
		case 1779:
		case 1781:
		case 1782:
		case 1783:
		case 1785:
		case 1786:
		case 1793:
		case 1794:
		case 1795:
		case 1796:
		case 1797:
		case 1798:
		case 1799:
			break;
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1806:
			scanner.SetGDRState();
			break;
		case 1823:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 1800:
		case 1801:
		case 1802:
		case 1803:
		case 1804:
		case 1805:
		case 1807:
		case 1808:
		case 1809:
		case 1810:
		case 1811:
		case 1812:
		case 1813:
		case 1814:
		case 1815:
		case 1816:
		case 1817:
		case 1818:
		case 1819:
		case 1820:
		case 1821:
		case 1822:
		case 1824:
		case 1825:
		case 1826:
		case 1827:
		case 1828:
		case 1829:
		case 1830:
		case 1831:
		case 1832:
		case 1833:
		case 1834:
		case 1835:
		case 1836:
		case 1837:
		case 1838:
		case 1839:
		case 1840:
		case 1841:
		case 1842:
		case 1843:
		case 1844:
		case 1845:
		case 1846:
		case 1847:
		case 1848:
		case 1849:
			break;
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		case 1861:
		{
			SqlTryCatchStatement sqlTryCatchStatement = new SqlTryCatchStatement(value_stack.array[value_stack.top - 3].value as List<SqlStatement>, value_stack.array[value_stack.top - 1].value as List<SqlStatement>);
			sqlTryCatchStatement.Position = yyloc;
			yyval.value = sqlTryCatchStatement;
			break;
		}
		case 1866:
			yyval = value_stack.array[value_stack.top - 3];
			break;
		case 1897:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 1850:
		case 1851:
		case 1852:
		case 1853:
		case 1854:
		case 1855:
		case 1856:
		case 1857:
		case 1858:
		case 1859:
		case 1860:
		case 1862:
		case 1863:
		case 1864:
		case 1865:
		case 1867:
		case 1868:
		case 1869:
		case 1870:
		case 1871:
		case 1872:
		case 1873:
		case 1874:
		case 1875:
		case 1876:
		case 1877:
		case 1878:
		case 1879:
		case 1880:
		case 1881:
		case 1882:
		case 1883:
		case 1884:
		case 1885:
		case 1886:
		case 1887:
		case 1888:
		case 1889:
		case 1890:
		case 1891:
		case 1892:
		case 1893:
		case 1894:
		case 1895:
		case 1896:
		case 1898:
		case 1899:
			break;
		}
	}

	private void DoAction_1900_1949(int action)
	{
		switch (action)
		{
		case 1920:
		{
			SqlDBCCStatement sqlDBCCStatement = new SqlDBCCStatement(GetText(location_stack.array[location_stack.top - 3]));
			sqlDBCCStatement.Position = yyloc;
			yyval.value = sqlDBCCStatement;
			break;
		}
		case 1900:
		case 1901:
		case 1902:
		case 1903:
		case 1904:
		case 1905:
		case 1906:
		case 1907:
		case 1908:
		case 1909:
		case 1910:
		case 1911:
		case 1912:
		case 1913:
		case 1914:
		case 1915:
		case 1916:
		case 1917:
		case 1918:
		case 1919:
		case 1921:
		case 1922:
		case 1923:
		case 1924:
		case 1925:
		case 1926:
		case 1927:
		case 1928:
		case 1929:
		case 1930:
		case 1931:
		case 1932:
		case 1933:
		case 1934:
		case 1935:
		case 1936:
		case 1937:
		case 1938:
		case 1939:
		case 1940:
		case 1941:
		case 1942:
		case 1943:
		case 1944:
		case 1945:
		case 1946:
		case 1947:
		case 1948:
		case 1949:
			break;
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		case 1992:
		{
			SqlSetStatement sqlSetStatement = value_stack.array[value_stack.top - 1].value as SqlSetStatement;
			if (sqlSetStatement is SqlSetStatementError)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 2]);
			}
			if (sqlSetStatement != null)
			{
				sqlSetStatement.Position = yyloc;
				yyval.value = sqlSetStatement;
			}
			break;
		}
		case 1993:
		{
			SqlSetStatementError sqlSetStatementError = new SqlSetStatementError();
			sqlSetStatementError.Position = yyloc;
			yyval.value = sqlSetStatementError;
			break;
		}
		case 1950:
		case 1951:
		case 1952:
		case 1953:
		case 1954:
		case 1955:
		case 1956:
		case 1957:
		case 1958:
		case 1959:
		case 1960:
		case 1961:
		case 1962:
		case 1963:
		case 1964:
		case 1965:
		case 1966:
		case 1967:
		case 1968:
		case 1969:
		case 1970:
		case 1971:
		case 1972:
		case 1973:
		case 1974:
		case 1975:
		case 1976:
		case 1977:
		case 1978:
		case 1979:
		case 1980:
		case 1981:
		case 1982:
		case 1983:
		case 1984:
		case 1985:
		case 1986:
		case 1987:
		case 1988:
		case 1989:
		case 1990:
		case 1991:
		case 1994:
		case 1995:
		case 1996:
		case 1997:
		case 1998:
		case 1999:
			break;
		}
	}

	private void DoAction_2000_2049(int action)
	{
		switch (action)
		{
		case 2005:
		{
			SqlCursorVariableRefExpression sqlCursorVariableRefExpression = SqlCursorVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string);
			sqlCursorVariableRefExpression.Position = location_stack.array[location_stack.top - 3];
			SqlSetAssignmentStatement sqlSetAssignmentStatement2 = new SqlSetAssignmentStatement(new SqlCursorVariableAssignment(sqlCursorVariableRefExpression, value_stack.array[value_stack.top - 1].value as CursorDefinitionInfo)
			{
				Position = yyloc
			});
			sqlSetAssignmentStatement2.Position = yyloc;
			yyval.value = sqlSetAssignmentStatement2;
			break;
		}
		case 2006:
		{
			SqlSetAssignmentStatement sqlSetAssignmentStatement = new SqlSetAssignmentStatement(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSetAssignmentStatement.Position = yyloc;
			yyval.value = sqlSetAssignmentStatement;
			break;
		}
		case 2008:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 2012:
			yyval.value = SqlOnOffValue.On;
			break;
		case 2013:
			yyval.value = SqlOnOffValue.Off;
			break;
		case 2000:
		case 2001:
		case 2002:
		case 2003:
		case 2004:
		case 2007:
		case 2009:
		case 2010:
		case 2011:
		case 2014:
		case 2015:
		case 2016:
		case 2017:
		case 2018:
		case 2019:
		case 2020:
		case 2021:
		case 2022:
		case 2023:
		case 2024:
		case 2025:
		case 2026:
		case 2027:
		case 2028:
		case 2029:
		case 2030:
		case 2031:
		case 2032:
		case 2033:
		case 2034:
		case 2035:
		case 2036:
		case 2037:
		case 2038:
		case 2039:
		case 2040:
		case 2041:
		case 2042:
		case 2043:
		case 2044:
		case 2045:
		case 2046:
		case 2047:
		case 2048:
		case 2049:
			break;
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		case 2064:
		{
			List<SqlSelectExpression> list5 = new List<SqlSelectExpression>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list5;
			break;
		}
		case 2065:
		{
			List<SqlSelectExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list4;
			break;
		}
		case 2066:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression4 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression4.Position = yyloc;
			yyval.value = sqlSelectScalarExpression4;
			break;
		}
		case 2067:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 1].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2068:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2069:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = SqlSelectScalarExpression.IdentityFunction(value_stack.array[value_stack.top - 2].value as SqlIdentityFunctionCallExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2070:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2071:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier4.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier4);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2072:
		{
			SqlSelectVariableAssignmentExpression sqlSelectVariableAssignmentExpression = new SqlSelectVariableAssignmentExpression(value_stack.array[value_stack.top - 1].value as SqlScalarVariableAssignment);
			sqlSelectVariableAssignmentExpression.Position = yyloc;
			yyval.value = sqlSelectVariableAssignmentExpression;
			break;
		}
		case 2073:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(value_stack.array[value_stack.top - 3].value as string);
			sqlScalarVariableRefExpression.Position = location_stack.array[location_stack.top - 3];
			SqlScalarVariableAssignment sqlScalarVariableAssignment = new SqlScalarVariableAssignment(sqlScalarVariableRefExpression, (SqlAssignmentOperatorType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlScalarVariableAssignment.Position = yyloc;
			yyval.value = sqlScalarVariableAssignment;
			break;
		}
		case 2074:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2078:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2079:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 2080:
		{
			SqlIdentifier sqlIdentifier = new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlIdentifier.Position = yyloc;
			yyval.value = sqlIdentifier;
			break;
		}
		case 2081:
		{
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier3.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier3)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 2082:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 4].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = location_stack.array[location_stack.top - 4];
			SqlDataType dataType2 = new SqlDataType(sqlObjectIdentifier2)
			{
				Position = location_stack.array[location_stack.top - 4]
			};
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(dataType2, sqlLiteralExpression3);
			sqlDataTypeSpecification3.Position = yyloc;
			yyval.value = sqlDataTypeSpecification3;
			break;
		}
		case 2083:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 6].value as SqlIdentifier);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 6];
			SqlDataType dataType = new SqlDataType(sqlObjectIdentifier)
			{
				Position = location_stack.array[location_stack.top - 6]
			};
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression2.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(dataType, sqlLiteralExpression, sqlLiteralExpression2);
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2084:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression2 = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as SqlDataTypeSpecification);
			sqlIdentityFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression2;
			break;
		}
		case 2085:
		{
			SqlIdentityFunctionCallExpression sqlIdentityFunctionCallExpression = new SqlIdentityFunctionCallExpression(GetText(location_stack.array[location_stack.top - 8]), value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlLiteralExpression, value_stack.array[value_stack.top - 2].value as SqlLiteralExpression);
			sqlIdentityFunctionCallExpression.Position = yyloc;
			yyval.value = sqlIdentityFunctionCallExpression;
			break;
		}
		case 2086:
		{
			List<SqlParameterDeclaration> list3 = new List<SqlParameterDeclaration>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list3;
			break;
		}
		case 2087:
		{
			List<SqlParameterDeclaration> list2 = value_stack.array[value_stack.top - 3].value as List<SqlParameterDeclaration>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlParameterDeclaration);
			yyval.value = list2;
			break;
		}
		case 2088:
		{
			SqlParameterDeclaration sqlParameterDeclaration2 = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 6]), value_stack.array[value_stack.top - 4].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration2.Position = yyloc;
			yyval.value = sqlParameterDeclaration2;
			break;
		}
		case 2090:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Cursor();
			sqlDataTypeSpecification.Position = location_stack.array[location_stack.top - 3];
			SqlParameterDeclaration sqlParameterDeclaration = new SqlParameterDeclaration(GetText(location_stack.array[location_stack.top - 5]), sqlDataTypeSpecification, null, value_stack.array[value_stack.top - 1].value as SqlParameterOption);
			sqlParameterDeclaration.Position = yyloc;
			yyval.value = sqlParameterDeclaration;
			break;
		}
		case 2092:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2094:
		{
			SqlParameterOption sqlParameterOption = SqlParameterOption.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlParameterOption.Position = yyloc;
			yyval.value = sqlParameterOption;
			break;
		}
		case 2098:
		{
			SqlFromClause sqlFromClause = new SqlFromClause(value_stack.array[value_stack.top - 1].value as List<SqlTableExpression>);
			sqlFromClause.Position = yyloc;
			yyval.value = sqlFromClause;
			break;
		}
		case 2099:
		{
			List<SqlTableExpression> list = new List<SqlTableExpression>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list;
			break;
		}
		case 2050:
		case 2051:
		case 2052:
		case 2053:
		case 2054:
		case 2055:
		case 2056:
		case 2057:
		case 2058:
		case 2059:
		case 2060:
		case 2061:
		case 2062:
		case 2063:
		case 2075:
		case 2076:
		case 2077:
		case 2089:
		case 2091:
		case 2093:
		case 2095:
		case 2096:
		case 2097:
			break;
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		case 2100:
		{
			List<SqlTableExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlTableExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			yyval.value = list;
			break;
		}
		case 2101:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression2 = new SqlNullTableExpression();
				sqlTableExpression2.Position = yyloc;
				yyval.value = sqlTableExpression2;
			}
			break;
		case 2102:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression4 = new SqlNullTableExpression();
				sqlTableExpression4.Position = yyloc;
				yyval.value = sqlTableExpression4;
			}
			break;
		case 2103:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression6 = new SqlNullTableExpression();
				sqlTableExpression6.Position = yyloc;
				yyval.value = sqlTableExpression6;
			}
			break;
		case 2104:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression5 = new SqlNullTableExpression();
				sqlTableExpression5.Position = yyloc;
				yyval.value = sqlTableExpression5;
			}
			break;
		case 2105:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression3 = new SqlNullTableExpression();
				sqlTableExpression3.Position = yyloc;
				yyval.value = sqlTableExpression3;
			}
			break;
		case 2106:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlTableExpression sqlTableExpression = new SqlNullTableExpression();
				sqlTableExpression.Position = yyloc;
				yyval.value = sqlTableExpression;
			}
			break;
		case 2107:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = yyloc;
			yyval.value = sqlTableExpressionError;
			break;
		}
		case 2109:
		{
			SqlTableRefExpression sqlTableRefExpression3 = new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlTableRefExpression3.Position = yyloc;
			yyval.value = sqlTableRefExpression3;
			break;
		}
		case 2110:
		{
			SqlTableUdtInstanceMethodExpression sqlTableUdtInstanceMethodExpression = new SqlTableUdtInstanceMethodExpression(value_stack.array[value_stack.top - 8].value as SqlScalarVariableRefExpression, value_stack.array[value_stack.top - 6].value as SqlIdentifier, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlTableUdtInstanceMethodExpression;
			break;
		}
		case 2111:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression2 = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlTableValuedFunctionRefExpression2.Position = yyloc;
			yyval.value = sqlTableValuedFunctionRefExpression2;
			break;
		}
		case 2112:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 3]))
			{
				Position = location_stack.array[location_stack.top - 3]
			});
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 3];
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression = new SqlTableValuedFunctionRefExpression(sqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>, value_stack.array[value_stack.top - 1].value as SqlIdentifier, null, isSys: true);
			sqlTableValuedFunctionRefExpression.Position = yyloc;
			yyval.value = sqlTableValuedFunctionRefExpression;
			break;
		}
		case 2113:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, null, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression2.Position = yyloc;
			yyval.value = sqlTableRefExpression2;
			break;
		}
		case 2114:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlHint>);
			sqlTableRefExpression.Position = yyloc;
			yyval.value = sqlTableRefExpression;
			break;
		}
		case 2126:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]), value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlTableVariableRefExpression.Position = yyloc;
			yyval.value = sqlTableVariableRefExpression;
			break;
		}
		case 2108:
		case 2115:
		case 2116:
		case 2117:
		case 2118:
		case 2119:
		case 2120:
		case 2121:
		case 2122:
		case 2123:
		case 2124:
		case 2125:
		case 2127:
		case 2128:
		case 2129:
		case 2130:
		case 2131:
		case 2132:
		case 2133:
		case 2134:
		case 2135:
		case 2136:
		case 2137:
		case 2138:
		case 2139:
		case 2140:
		case 2141:
		case 2142:
		case 2143:
		case 2144:
		case 2145:
		case 2146:
		case 2147:
		case 2148:
		case 2149:
			break;
		}
	}

	private void DoAction_2150_2199(int action)
	{
		switch (action)
		{
		case 2159:
		{
			SqlNullTableExpression sqlNullTableExpression6 = new SqlNullTableExpression();
			sqlNullTableExpression6.Position = yyloc;
			yyval.value = sqlNullTableExpression6;
			break;
		}
		case 2160:
		{
			SqlNullTableExpression sqlNullTableExpression5 = new SqlNullTableExpression();
			sqlNullTableExpression5.Position = yyloc;
			yyval.value = sqlNullTableExpression5;
			break;
		}
		case 2161:
		{
			SqlNullTableExpression sqlNullTableExpression4 = new SqlNullTableExpression();
			sqlNullTableExpression4.Position = yyloc;
			yyval.value = sqlNullTableExpression4;
			break;
		}
		case 2162:
		{
			SqlNullTableExpression sqlNullTableExpression3 = new SqlNullTableExpression();
			sqlNullTableExpression3.Position = yyloc;
			yyval.value = sqlNullTableExpression3;
			break;
		}
		case 2163:
		{
			SqlNullTableExpression sqlNullTableExpression2 = new SqlNullTableExpression();
			sqlNullTableExpression2.Position = yyloc;
			yyval.value = sqlNullTableExpression2;
			break;
		}
		case 2164:
		{
			SqlNullTableExpression sqlNullTableExpression = new SqlNullTableExpression();
			sqlNullTableExpression.Position = yyloc;
			yyval.value = sqlNullTableExpression;
			break;
		}
		case 2184:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2187:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 2188:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2189:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2192:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2193:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 2195:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2198:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression2 = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression2.Position = yyloc;
			yyval.value = sqlDerivedTableExpression2;
			break;
		}
		case 2199:
		{
			SqlDerivedTableExpression sqlDerivedTableExpression = new SqlDerivedTableExpression(value_stack.array[value_stack.top - 5].value as SqlQueryExpression, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDerivedTableExpression.Position = yyloc;
			yyval.value = sqlDerivedTableExpression;
			break;
		}
		case 2150:
		case 2151:
		case 2152:
		case 2153:
		case 2154:
		case 2155:
		case 2156:
		case 2157:
		case 2158:
		case 2165:
		case 2166:
		case 2167:
		case 2168:
		case 2169:
		case 2170:
		case 2171:
		case 2172:
		case 2173:
		case 2174:
		case 2175:
		case 2176:
		case 2177:
		case 2178:
		case 2179:
		case 2180:
		case 2181:
		case 2182:
		case 2183:
		case 2185:
		case 2186:
		case 2190:
		case 2191:
		case 2194:
		case 2196:
		case 2197:
			break;
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		case 2202:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2203:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression3 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossJoin, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression3.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression3;
			break;
		}
		case 2204:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression2 = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.CrossApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression2.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression2;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2205:
		{
			SqlUnqualifiedJoinTableExpression sqlUnqualifiedJoinTableExpression = new SqlUnqualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, SqlJoinOperatorType.OuterApply, value_stack.array[value_stack.top - 1].value as SqlTableExpression);
			sqlUnqualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlUnqualifiedJoinTableExpression;
			ExpectKeyword(StringToken.Apply, location_stack.array[location_stack.top - 2]);
			break;
		}
		case 2206:
		{
			SqlQualifiedJoinTableExpression sqlQualifiedJoinTableExpression = new SqlQualifiedJoinTableExpression(value_stack.array[value_stack.top - 4].value as SqlTableExpression, (SqlJoinOperatorType)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as SqlConditionClause);
			sqlQualifiedJoinTableExpression.Position = yyloc;
			yyval.value = sqlQualifiedJoinTableExpression;
			break;
		}
		case 2216:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2217:
			yyval.value = SqlJoinOperatorType.InnerJoin;
			break;
		case 2218:
			yyval.value = SqlJoinOperatorType.LeftOuterJoin;
			break;
		case 2219:
			yyval.value = SqlJoinOperatorType.RightOuterJoin;
			break;
		case 2220:
			yyval.value = SqlJoinOperatorType.FullOuterJoin;
			break;
		case 2221:
		{
			SqlConditionClause sqlConditionClause = new SqlConditionClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlConditionClause.Position = yyloc;
			yyval.value = sqlConditionClause;
			break;
		}
		case 2222:
		{
			SqlOnClauseError sqlOnClauseError = new SqlOnClauseError(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlOnClauseError.Position = yyloc;
			yyval.value = sqlOnClauseError;
			break;
		}
		case 2225:
		{
			SqlPivotClause sqlPivotClause = new SqlPivotClause(value_stack.array[value_stack.top - 13].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 11].value as List<SqlColumnRefExpression>, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlPivotClause.Position = MergeLocationRange(location_stack.top - 13, location_stack.top - 3);
			SqlPivotTableExpression sqlPivotTableExpression = new SqlPivotTableExpression(value_stack.array[value_stack.top - 16].value as SqlTableExpression, sqlPivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlPivotTableExpression.Position = yyloc;
			yyval.value = sqlPivotTableExpression;
			break;
		}
		case 2226:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2227:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2228:
		{
			SqlUnpivotClause sqlUnpivotClause = new SqlUnpivotClause(value_stack.array[value_stack.top - 10].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 8].value as SqlColumnRefExpression, value_stack.array[value_stack.top - 5].value as List<SqlColumnRefExpression>);
			sqlUnpivotClause.Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 3);
			SqlUnpivotTableExpression sqlUnpivotTableExpression = new SqlUnpivotTableExpression(value_stack.array[value_stack.top - 13].value as SqlTableExpression, sqlUnpivotClause, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUnpivotTableExpression.Position = yyloc;
			yyval.value = sqlUnpivotTableExpression;
			break;
		}
		case 2229:
		{
			List<SqlColumnRefExpression> list4 = new List<SqlColumnRefExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list4;
			break;
		}
		case 2230:
		{
			List<SqlColumnRefExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlColumnRefExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlColumnRefExpression);
			yyval.value = list3;
			break;
		}
		case 2231:
		{
			SqlDmlSpecificationTableSource sqlDmlSpecificationTableSource = new SqlDmlSpecificationTableSource(value_stack.array[value_stack.top - 5].value as SqlDmlSpecification, value_stack.array[value_stack.top - 2].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as List<SqlIdentifier>);
			sqlDmlSpecificationTableSource.Position = yyloc;
			yyval.value = sqlDmlSpecificationTableSource;
			break;
		}
		case 2241:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2242:
		{
			List<SqlHint> list2 = new List<SqlHint>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list2;
			break;
		}
		case 2243:
		{
			SqlHint sqlHint = SqlHint.Table(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlHint.Position = location_stack.array[location_stack.top - 1];
			List<SqlHint> list = new List<SqlHint>();
			list.Add(sqlHint);
			yyval.value = list;
			break;
		}
		case 2244:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 2];
			SqlIndexHint sqlIndexHint = new SqlIndexHint(sqlLiteralExpression);
			sqlIndexHint.Position = yyloc;
			yyval.value = sqlIndexHint;
			break;
		}
		case 2245:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2200:
		case 2201:
		case 2207:
		case 2208:
		case 2209:
		case 2210:
		case 2211:
		case 2212:
		case 2213:
		case 2214:
		case 2215:
		case 2223:
		case 2224:
		case 2232:
		case 2233:
		case 2234:
		case 2235:
		case 2236:
		case 2237:
		case 2238:
		case 2239:
		case 2240:
		case 2246:
		case 2247:
		case 2248:
		case 2249:
			break;
		}
	}

	private void DoAction_2250_2299(int action)
	{
		switch (action)
		{
		case 2255:
		{
			List<SqlHint> list3 = new List<SqlHint>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list3;
			break;
		}
		case 2256:
		{
			List<SqlHint> list6 = value_stack.array[value_stack.top - 3].value as List<SqlHint>;
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlHint);
			yyval.value = list6;
			break;
		}
		case 2261:
		{
			SqlTableHint sqlTableHint4 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 3]));
			sqlTableHint4.Position = yyloc;
			yyval.value = sqlTableHint4;
			ExpectKeyword(StringToken.SpatialWindowMaxCells, location_stack.array[location_stack.top - 3]);
			break;
		}
		case 2264:
		{
			SqlTableHint sqlTableHint3 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 7]));
			sqlTableHint3.Position = yyloc;
			yyval.value = sqlTableHint3;
			ExpectKeyword(StringToken.ForceSeek, location_stack.array[location_stack.top - 7]);
			break;
		}
		case 2266:
		{
			SqlTableHint sqlTableHint2 = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint2.Position = yyloc;
			yyval.value = sqlTableHint2;
			break;
		}
		case 2267:
		{
			SqlTableHint sqlTableHint = new SqlTableHint(GetText(location_stack.array[location_stack.top - 1]));
			sqlTableHint.Position = yyloc;
			yyval.value = sqlTableHint;
			break;
		}
		case 2268:
		{
			SqlIndexHint sqlIndexHint2 = new SqlIndexHint(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			sqlIndexHint2.Position = yyloc;
			yyval.value = sqlIndexHint2;
			break;
		}
		case 2269:
		{
			SqlIndexHint sqlIndexHint = new SqlIndexHint(value_stack.array[value_stack.top - 2].value as List<SqlLiteralExpression>);
			sqlIndexHint.Position = yyloc;
			yyval.value = sqlIndexHint;
			break;
		}
		case 2270:
		{
			List<SqlLiteralExpression> list5 = new List<SqlLiteralExpression>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list5;
			break;
		}
		case 2271:
		{
			List<SqlLiteralExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlLiteralExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlLiteralExpression);
			yyval.value = list4;
			break;
		}
		case 2273:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2274:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2287:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression2 = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, (SqlBinaryQueryOperatorType)value_stack.array[value_stack.top - 3].value, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression2.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression2;
			break;
		}
		case 2288:
		{
			SqlInlineFunctionBodyDefinition sqlInlineFunctionBodyDefinition = new SqlInlineFunctionBodyDefinition(value_stack.array[value_stack.top - 2].value as SqlQueryWithClause, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInlineFunctionBodyDefinition.Position = yyloc;
			yyval.value = sqlInlineFunctionBodyDefinition;
			break;
		}
		case 2290:
		{
			SqlBinaryQueryExpression sqlBinaryQueryExpression = new SqlBinaryQueryExpression(value_stack.array[value_stack.top - 4].value as SqlQueryExpression, SqlBinaryQueryOperatorType.Intersect, (bool)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlBinaryQueryExpression.Position = yyloc;
			yyval.value = sqlBinaryQueryExpression;
			break;
		}
		case 2292:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2293:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2294:
			if (value_stack.array[value_stack.top - 1].value == null)
			{
				SqlNullQueryExpression sqlNullQueryExpression = new SqlNullQueryExpression();
				sqlNullQueryExpression.Position = yyloc;
				yyval.value = sqlNullQueryExpression;
			}
			break;
		case 2295:
		{
			SqlScalarSubQueryExpression sqlScalarSubQueryExpression = new SqlScalarSubQueryExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlScalarSubQueryExpression.Position = yyloc;
			yyval.value = sqlScalarSubQueryExpression;
			break;
		}
		case 2297:
		{
			SqlQuerySpecification sqlQuerySpecification = new SqlQuerySpecification(new SqlSelectClause(value_stack.array[value_stack.top - 9].value != null, value_stack.array[value_stack.top - 8].value as SqlTopSpecification, value_stack.array[value_stack.top - 7].value as List<SqlSelectExpression>)
			{
				Position = MergeLocationRange(location_stack.top - 10, location_stack.top - 7)
			}, value_stack.array[value_stack.top - 6].value as SqlFromClause, value_stack.array[value_stack.top - 5].value as SqlWhereClause, value_stack.array[value_stack.top - 4].value as SqlGroupByClause, value_stack.array[value_stack.top - 3].value as SqlHavingClause, value_stack.array[value_stack.top - 2].value as SqlOrderByClause, value_stack.array[value_stack.top - 1].value as SqlForClause);
			sqlQuerySpecification.Position = yyloc;
			yyval.value = sqlQuerySpecification;
			break;
		}
		case 2298:
		{
			List<SqlSelectExpression> list2 = new List<SqlSelectExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list2;
			break;
		}
		case 2299:
		{
			List<SqlSelectExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlSelectExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlSelectExpression);
			yyval.value = list;
			break;
		}
		case 2250:
		case 2251:
		case 2252:
		case 2253:
		case 2254:
		case 2257:
		case 2258:
		case 2259:
		case 2260:
		case 2262:
		case 2263:
		case 2265:
		case 2272:
		case 2275:
		case 2276:
		case 2277:
		case 2278:
		case 2279:
		case 2280:
		case 2281:
		case 2282:
		case 2283:
		case 2284:
		case 2285:
		case 2286:
		case 2289:
		case 2291:
		case 2296:
			break;
		}
	}

	private void DoAction_2300_2349(int action)
	{
		switch (action)
		{
		case 2300:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSelectScalarExpression.Position = yyloc;
			yyval.value = sqlSelectScalarExpression;
			break;
		}
		case 2301:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression3 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 1].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlIdentifier);
			sqlSelectScalarExpression3.Position = yyloc;
			yyval.value = sqlSelectScalarExpression3;
			break;
		}
		case 2302:
		{
			SqlSelectScalarExpression sqlSelectScalarExpression2 = new SqlSelectScalarExpression(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlSelectScalarExpression2.Position = yyloc;
			yyval.value = sqlSelectScalarExpression2;
			break;
		}
		case 2303:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2304:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2305:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2307:
		{
			SqlLiteralStringIdentifier sqlLiteralStringIdentifier = new SqlLiteralStringIdentifier(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralStringIdentifier.Position = yyloc;
			yyval.value = sqlLiteralStringIdentifier;
			break;
		}
		case 2309:
		{
			SqlOrderByClause sqlOrderByClause = new SqlOrderByClause(value_stack.array[value_stack.top - 2].value as List<SqlOrderByItem>, value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause);
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2310:
		{
			List<SqlOrderByItem> list14 = new List<SqlOrderByItem>();
			list14.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list14;
			break;
		}
		case 2311:
		{
			List<SqlOrderByItem> list13 = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list13.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list13;
			break;
		}
		case 2312:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2314:
			if (ExpectKeyword(StringToken.Xml, location_stack.array[location_stack.top - 2]))
			{
				StringToken stringToken = GetStringToken(location_stack.array[location_stack.top - 2]);
				SqlForXmlClause sqlForXmlClause = SqlForXmlClause.Create(value_stack.array[value_stack.top - 1].value as List<SqlForXmlUnknownDirective>, stringToken);
				sqlForXmlClause.Position = yyloc;
				yyval.value = sqlForXmlClause;
			}
			break;
		case 2315:
		{
			SqlForBrowseClause sqlForBrowseClause = new SqlForBrowseClause();
			sqlForBrowseClause.Position = yyloc;
			yyval.value = sqlForBrowseClause;
			break;
		}
		case 2316:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression2 = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression2.Position = yyloc;
			yyval.value = sqlTableConstructorExpression2;
			break;
		}
		case 2317:
		{
			List<SqlRowConstructorExpression> list12 = new List<SqlRowConstructorExpression>();
			list12.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list12;
			break;
		}
		case 2318:
		{
			List<SqlRowConstructorExpression> list11 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list11.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list11;
			break;
		}
		case 2319:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression2 = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression2.Position = yyloc;
			yyval.value = sqlRowConstructorExpression2;
			break;
		}
		case 2320:
		{
			List<SqlScalarExpression> list10 = new List<SqlScalarExpression>();
			list10.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list10;
			break;
		}
		case 2321:
		{
			List<SqlScalarExpression> list9 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list9;
			break;
		}
		case 2322:
		{
			SqlTableConstructorExpression sqlTableConstructorExpression = new SqlTableConstructorExpression(value_stack.array[value_stack.top - 1].value as List<SqlRowConstructorExpression>);
			sqlTableConstructorExpression.Position = yyloc;
			yyval.value = sqlTableConstructorExpression;
			break;
		}
		case 2323:
		{
			List<SqlRowConstructorExpression> list8 = new List<SqlRowConstructorExpression>();
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list8;
			break;
		}
		case 2324:
		{
			List<SqlRowConstructorExpression> list7 = value_stack.array[value_stack.top - 3].value as List<SqlRowConstructorExpression>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlRowConstructorExpression);
			yyval.value = list7;
			break;
		}
		case 2325:
		{
			SqlRowConstructorExpression sqlRowConstructorExpression = new SqlRowConstructorExpression(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlRowConstructorExpression.Position = yyloc;
			yyval.value = sqlRowConstructorExpression;
			break;
		}
		case 2326:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 2327:
		{
			List<SqlScalarExpression> list5 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 2329:
		{
			SqlWhereClause sqlWhereClause = new SqlWhereClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlWhereClause.Position = yyloc;
			yyval.value = sqlWhereClause;
			scanner.SetQueryState(QueryState.None);
			break;
		}
		case 2333:
		{
			SqlGroupByClause sqlGroupByClause = new SqlGroupByClause(value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>, value_stack.array[value_stack.top - 1].value as string, (bool)value_stack.array[value_stack.top - 4].value);
			sqlGroupByClause.Position = yyloc;
			yyval.value = sqlGroupByClause;
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		}
		case 2334:
			yyval.value = false;
			break;
		case 2335:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			yyval.value = true;
			break;
		case 2336:
			yyval.value = "";
			break;
		case 2337:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]) + " " + GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 2338:
		{
			List<SqlGroupByItem> list4 = new List<SqlGroupByItem>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list4;
			break;
		}
		case 2340:
		{
			List<SqlGroupByItem> list3 = new List<SqlGroupByItem>();
			list3.Add(value_stack.array[value_stack.top - 3].value as SqlGroupByItem);
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list3;
			break;
		}
		case 2341:
		{
			List<SqlGroupByItem> list2 = value_stack.array[value_stack.top - 3].value as List<SqlGroupByItem>;
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlGroupByItem);
			yyval.value = list2;
			break;
		}
		case 2348:
		{
			SqlGroupBySets sqlGroupBySets = new SqlGroupBySets(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSet>);
			sqlGroupBySets.Position = yyloc;
			yyval.value = sqlGroupBySets;
			ExpectKeyword(StringToken.Grouping, location_stack.array[location_stack.top - 5]);
			ExpectKeyword(StringToken.Sets, location_stack.array[location_stack.top - 4]);
			break;
		}
		case 2349:
		{
			List<SqlGroupingSet> list = new List<SqlGroupingSet>();
			list.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list;
			break;
		}
		case 2306:
		case 2308:
		case 2313:
		case 2328:
		case 2330:
		case 2331:
		case 2332:
		case 2339:
		case 2342:
		case 2343:
		case 2344:
		case 2345:
		case 2346:
		case 2347:
			break;
		}
	}

	private void DoAction_2350_2399(int action)
	{
		switch (action)
		{
		case 2350:
		{
			List<SqlGroupingSet> list9 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSet>;
			list9.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSet);
			yyval.value = list9;
			break;
		}
		case 2351:
		{
			SqlGrandTotalGroupingSet sqlGrandTotalGroupingSet = new SqlGrandTotalGroupingSet(value_stack.array[value_stack.top - 1].value as SqlGrandTotalGroupByItem);
			sqlGrandTotalGroupingSet.Position = yyloc;
			yyval.value = sqlGrandTotalGroupingSet;
			break;
		}
		case 2352:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection4 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			sqlGroupingSetItemsCollection4.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection4;
			break;
		}
		case 2353:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection3 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as List<SqlGroupingSetItem>);
			sqlGroupingSetItemsCollection3.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection3;
			break;
		}
		case 2354:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection2 = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlRollupGroupByItem);
			sqlGroupingSetItemsCollection2.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection2;
			break;
		}
		case 2355:
		{
			SqlGroupingSetItemsCollection sqlGroupingSetItemsCollection = new SqlGroupingSetItemsCollection(value_stack.array[value_stack.top - 2].value as SqlCubeGroupByItem);
			sqlGroupingSetItemsCollection.Position = yyloc;
			yyval.value = sqlGroupingSetItemsCollection;
			break;
		}
		case 2356:
		{
			SqlGrandTotalGroupByItem sqlGrandTotalGroupByItem = new SqlGrandTotalGroupByItem();
			sqlGrandTotalGroupByItem.Position = yyloc;
			yyval.value = sqlGrandTotalGroupByItem;
			break;
		}
		case 2357:
		{
			List<SqlGroupingSetItem> list8 = new List<SqlGroupingSetItem>();
			list8.Add(value_stack.array[value_stack.top - 3].value as SqlGroupingSetItem);
			list8.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list8;
			break;
		}
		case 2358:
		{
			List<SqlGroupingSetItem> list7 = value_stack.array[value_stack.top - 3].value as List<SqlGroupingSetItem>;
			list7.Add(value_stack.array[value_stack.top - 1].value as SqlGroupingSetItem);
			yyval.value = list7;
			break;
		}
		case 2362:
		{
			SqlRollupGroupByItem sqlRollupGroupByItem = new SqlRollupGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlRollupGroupByItem.Position = yyloc;
			yyval.value = sqlRollupGroupByItem;
			break;
		}
		case 2363:
		{
			SqlCubeGroupByItem sqlCubeGroupByItem = new SqlCubeGroupByItem(value_stack.array[value_stack.top - 2].value as List<SqlSimpleGroupByItem>);
			sqlCubeGroupByItem.Position = yyloc;
			yyval.value = sqlCubeGroupByItem;
			break;
		}
		case 2365:
		{
			List<SqlSimpleGroupByItem> list6 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list6.AddRange(value_stack.array[value_stack.top - 1].value as List<SqlSimpleGroupByItem>);
			yyval.value = list6;
			break;
		}
		case 2366:
		{
			List<SqlSimpleGroupByItem> list5 = new List<SqlSimpleGroupByItem>();
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list5;
			break;
		}
		case 2367:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2368:
		{
			List<SqlSimpleGroupByItem> list4 = new List<SqlSimpleGroupByItem>();
			list4.Add(value_stack.array[value_stack.top - 3].value as SqlSimpleGroupByItem);
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list4;
			break;
		}
		case 2369:
		{
			List<SqlSimpleGroupByItem> list3 = value_stack.array[value_stack.top - 3].value as List<SqlSimpleGroupByItem>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleGroupByItem);
			yyval.value = list3;
			break;
		}
		case 2370:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem3 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem3.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem3;
			break;
		}
		case 2371:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem2 = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleGroupByItem2.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem2;
			break;
		}
		case 2372:
		{
			SqlSimpleGroupByItem sqlSimpleGroupByItem = new SqlSimpleGroupByItem(value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlSimpleGroupByItem.Position = yyloc;
			yyval.value = sqlSimpleGroupByItem;
			break;
		}
		case 2376:
		{
			SqlHavingClause sqlHavingClause = new SqlHavingClause(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlHavingClause.Position = yyloc;
			yyval.value = sqlHavingClause;
			break;
		}
		case 2381:
		{
			SqlOrderByClause sqlOrderByClause2 = new SqlOrderByClause(value_stack.array[value_stack.top - 1].value as List<SqlOrderByItem>);
			sqlOrderByClause2.Position = yyloc;
			yyval.value = sqlOrderByClause2;
			break;
		}
		case 2382:
		{
			SqlOrderByClause sqlOrderByClause = value_stack.array[value_stack.top - 2].value as SqlOrderByClause;
			sqlOrderByClause.OffsetFetchClause = value_stack.array[value_stack.top - 1].value as SqlOffsetFetchClause;
			sqlOrderByClause.Position = yyloc;
			yyval.value = sqlOrderByClause;
			break;
		}
		case 2383:
		{
			List<SqlOrderByItem> list2 = new List<SqlOrderByItem>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list2;
			break;
		}
		case 2384:
		{
			List<SqlOrderByItem> list = value_stack.array[value_stack.top - 3].value as List<SqlOrderByItem>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlOrderByItem);
			yyval.value = list;
			break;
		}
		case 2385:
		{
			SqlOrderByItem sqlOrderByItem = new SqlOrderByItem(value_stack.array[value_stack.top - 2].value as SqlScalarExpression, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlOrderByItem.Position = yyloc;
			yyval.value = sqlOrderByItem;
			break;
		}
		case 2386:
			yyval.value = SqlSortOrder.None;
			break;
		case 2387:
			yyval.value = SqlSortOrder.Ascending;
			break;
		case 2388:
			yyval.value = SqlSortOrder.Descending;
			break;
		case 2390:
		{
			SqlBooleanExpressionError sqlBooleanExpressionError = new SqlBooleanExpressionError(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBooleanExpressionError.Position = yyloc;
			yyval.value = sqlBooleanExpressionError;
			break;
		}
		case 2391:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression3 = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlComparisonBooleanExpression);
			sqlBooleanFilterExpression3.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression3;
			break;
		}
		case 2392:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression2 = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlIsNullBooleanExpression);
			sqlBooleanFilterExpression2.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression2;
			break;
		}
		case 2393:
		{
			SqlBooleanFilterExpression sqlBooleanFilterExpression = SqlBooleanFilterExpression.Create(value_stack.array[value_stack.top - 1].value as SqlInBooleanExpression);
			sqlBooleanFilterExpression.Position = yyloc;
			yyval.value = sqlBooleanFilterExpression;
			break;
		}
		case 2394:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2395:
		{
			SqlBinaryFilterExpression sqlBinaryFilterExpression = new SqlBinaryFilterExpression(value_stack.array[value_stack.top - 3].value as SqlFilterExpression, value_stack.array[value_stack.top - 1].value as SqlFilterExpression);
			sqlBinaryFilterExpression.Position = yyloc;
			yyval.value = sqlBinaryFilterExpression;
			break;
		}
		case 2359:
		case 2360:
		case 2361:
		case 2364:
		case 2373:
		case 2374:
		case 2375:
		case 2377:
		case 2378:
		case 2379:
		case 2380:
		case 2389:
		case 2396:
		case 2397:
		case 2398:
		case 2399:
			break;
		}
	}

	private void DoAction_2400_2449(int action)
	{
		switch (action)
		{
		case 2405:
		{
			SqlBooleanExpression sqlBooleanExpression = value_stack.array[value_stack.top - 2].value as SqlBooleanExpression;
			sqlBooleanExpression.Position = yyloc;
			yyval.value = sqlBooleanExpression;
			break;
		}
		case 2407:
		{
			SqlNotBooleanExpression sqlNotBooleanExpression = new SqlNotBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlNotBooleanExpression.Position = yyloc;
			yyval.value = sqlNotBooleanExpression;
			break;
		}
		case 2408:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression2 = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.And, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression2.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression2;
			break;
		}
		case 2409:
		{
			SqlBinaryBooleanExpression sqlBinaryBooleanExpression = new SqlBinaryBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, SqlBooleanOperatorType.Or, value_stack.array[value_stack.top - 1].value as SqlBooleanExpression);
			sqlBinaryBooleanExpression.Position = yyloc;
			yyval.value = sqlBinaryBooleanExpression;
			break;
		}
		case 2410:
		{
			SqlComparisonBooleanExpression sqlComparisonBooleanExpression = new SqlComparisonBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 2].value, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlComparisonBooleanExpression;
			break;
		}
		case 2411:
			yyval.value = SqlComparisonBooleanExpressionType.Equals;
			break;
		case 2412:
			yyval.value = SqlComparisonBooleanExpressionType.NotEqual;
			break;
		case 2413:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThan;
			break;
		case 2414:
			yyval.value = SqlComparisonBooleanExpressionType.GreaterThanOrEqual;
			break;
		case 2415:
			yyval.value = SqlComparisonBooleanExpressionType.LessThan;
			break;
		case 2416:
			yyval.value = SqlComparisonBooleanExpressionType.LessOrGreaterThan;
			break;
		case 2417:
			yyval.value = SqlComparisonBooleanExpressionType.LessThanOrEqual;
			break;
		case 2418:
			yyval.value = SqlComparisonBooleanExpressionType.NotLessThan;
			break;
		case 2419:
			yyval.value = SqlComparisonBooleanExpressionType.NotGreaterThan;
			break;
		case 2420:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression2 = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 5].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression2.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression2;
			break;
		}
		case 2421:
		{
			SqlBetweenBooleanExpression sqlBetweenBooleanExpression = new SqlBetweenBooleanExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBetweenBooleanExpression.Position = yyloc;
			yyval.value = sqlBetweenBooleanExpression;
			break;
		}
		case 2422:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression2 = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlIsNullBooleanExpression2.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression2;
			break;
		}
		case 2423:
		{
			SqlIsNullBooleanExpression sqlIsNullBooleanExpression = new SqlIsNullBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true);
			sqlIsNullBooleanExpression.Position = yyloc;
			yyval.value = sqlIsNullBooleanExpression;
			break;
		}
		case 2424:
		{
			SqlInBooleanExpression sqlInBooleanExpression2 = new SqlInBooleanExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression2.Position = yyloc;
			yyval.value = sqlInBooleanExpression2;
			break;
		}
		case 2425:
		{
			SqlInBooleanExpression sqlInBooleanExpression = new SqlInBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, hasNot: true, value_stack.array[value_stack.top - 1].value as SqlInBooleanExpressionValue);
			sqlInBooleanExpression.Position = yyloc;
			yyval.value = sqlInBooleanExpression;
			break;
		}
		case 2426:
		{
			SqlInBooleanExpressionQueryValue sqlInBooleanExpressionQueryValue = new SqlInBooleanExpressionQueryValue(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlInBooleanExpressionQueryValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionQueryValue;
			break;
		}
		case 2427:
		{
			SqlInBooleanExpressionCollectionValue sqlInBooleanExpressionCollectionValue = new SqlInBooleanExpressionCollectionValue(value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlInBooleanExpressionCollectionValue.Position = yyloc;
			yyval.value = sqlInBooleanExpressionCollectionValue;
			break;
		}
		case 2428:
		{
			List<SqlScalarExpression> list6 = new List<SqlScalarExpression>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list6;
			break;
		}
		case 2429:
			yyval.value = value_stack.array[value_stack.top - 1].value as List<SqlScalarExpression>;
			break;
		case 2430:
		{
			List<SqlScalarExpression> list5 = new List<SqlScalarExpression>();
			list5.Add(value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list5;
			break;
		}
		case 2431:
		{
			List<SqlScalarExpression> list4 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2432:
		{
			SqlFullTextBooleanExpression sqlFullTextBooleanExpression = new SqlFullTextBooleanExpression((SqlFullTextFunctionType)value_stack.array[value_stack.top - 7].value, value_stack.array[value_stack.top - 5].value as List<SqlFullTextColumn>, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlFullTextBooleanExpression.Position = yyloc;
			yyval.value = sqlFullTextBooleanExpression;
			break;
		}
		case 2433:
			yyval.value = SqlFullTextFunctionType.Contains;
			break;
		case 2434:
			yyval.value = SqlFullTextFunctionType.FreeText;
			break;
		case 2435:
		{
			List<SqlFullTextColumn> list3 = new List<SqlFullTextColumn>();
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlFullTextColumn);
			yyval.value = list3;
			break;
		}
		case 2436:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2438:
		{
			SqlFullTextColumn sqlFullTextColumn5 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn5.Position = yyloc;
			List<SqlFullTextColumn> list2 = new List<SqlFullTextColumn>();
			list2.Add(sqlFullTextColumn5);
			yyval.value = list2;
			break;
		}
		case 2439:
		{
			SqlFullTextColumn sqlFullTextColumn4 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn4.Position = location_stack.array[location_stack.top - 1];
			List<SqlFullTextColumn> list = value_stack.array[value_stack.top - 3].value as List<SqlFullTextColumn>;
			list.Add(sqlFullTextColumn4);
			yyval.value = list;
			break;
		}
		case 2440:
		{
			SqlFullTextColumn sqlFullTextColumn3 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlScalarRefExpression);
			sqlFullTextColumn3.Position = yyloc;
			yyval.value = sqlFullTextColumn3;
			break;
		}
		case 2441:
		{
			SqlFullTextColumn sqlFullTextColumn2 = new SqlFullTextColumn(value_stack.array[value_stack.top - 1].value as SqlSelectStarExpression);
			sqlFullTextColumn2.Position = yyloc;
			yyval.value = sqlFullTextColumn2;
			break;
		}
		case 2442:
		{
			SqlFullTextColumn sqlFullTextColumn = new SqlFullTextColumn(value_stack.array[value_stack.top - 2].value as SqlSelectStarExpression);
			sqlFullTextColumn.Position = yyloc;
			yyval.value = sqlFullTextColumn;
			break;
		}
		case 2443:
		{
			SqlSelectStarExpression sqlSelectStarExpression2 = new SqlSelectStarExpression();
			sqlSelectStarExpression2.Position = yyloc;
			yyval.value = sqlSelectStarExpression2;
			break;
		}
		case 2444:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>);
			sqlObjectIdentifier.Position = location_stack.array[location_stack.top - 2];
			SqlSelectStarExpression sqlSelectStarExpression = new SqlSelectStarExpression(sqlObjectIdentifier);
			sqlSelectStarExpression.Position = yyloc;
			yyval.value = sqlSelectStarExpression;
			break;
		}
		case 2400:
		case 2401:
		case 2402:
		case 2403:
		case 2404:
		case 2406:
		case 2437:
		case 2445:
		case 2446:
		case 2447:
		case 2448:
		case 2449:
			break;
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		case 2452:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2453:
			if (ExpectKeyword(StringToken.Language, location_stack.array[location_stack.top - 2]))
			{
				yyval = value_stack.array[value_stack.top - 1];
			}
			break;
		case 2454:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2455:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2457:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression2 = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ANY", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression2.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression2;
			break;
		}
		case 2458:
		{
			SqlAllAnyComparisonBooleanExpression sqlAllAnyComparisonBooleanExpression = new SqlAllAnyComparisonBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (SqlComparisonBooleanExpressionType)value_stack.array[value_stack.top - 3].value, "ALL", value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlAllAnyComparisonBooleanExpression.Position = yyloc;
			yyval.value = sqlAllAnyComparisonBooleanExpression;
			break;
		}
		case 2459:
		{
			SqlExistsBooleanExpression sqlExistsBooleanExpression = new SqlExistsBooleanExpression(value_stack.array[value_stack.top - 1].value as SqlQueryExpression);
			sqlExistsBooleanExpression.Position = yyloc;
			yyval.value = sqlExistsBooleanExpression;
			break;
		}
		case 2460:
		{
			SqlLikeBooleanExpression sqlLikeBooleanExpression = new SqlLikeBooleanExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, (bool)value_stack.array[value_stack.top - 3].value, value_stack.array[value_stack.top - 2].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlLikeBooleanExpression.Position = yyloc;
			yyval.value = sqlLikeBooleanExpression;
			break;
		}
		case 2461:
			yyval.value = false;
			break;
		case 2462:
			yyval.value = true;
			break;
		case 2466:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2470:
		{
			SqlCollation sqlCollation = new SqlCollation(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = location_stack.array[location_stack.top - 1]
			});
			sqlCollation.Position = yyloc;
			yyval.value = sqlCollation;
			if (EscapeSequence.IdentifyEscapeSequence(GetText(location_stack.array[location_stack.top - 1])) != null)
			{
				VersionedParserBase.SyntaxError(location_stack.array[location_stack.top - 1]);
			}
			break;
		}
		case 2472:
		{
			SqlScalarExpression sqlScalarExpression5 = value_stack.array[value_stack.top - 2].value as SqlScalarSubQueryExpression;
			if (sqlScalarExpression5 == null)
			{
				sqlScalarExpression5 = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression6 = SqlScalarExpression.Create(sqlScalarExpression5, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression6.Position = yyloc;
			yyval.value = sqlScalarExpression6;
			break;
		}
		case 2473:
		{
			SqlScalarExpression sqlScalarExpression3 = value_stack.array[value_stack.top - 2].value as SqlScalarExpression;
			if (sqlScalarExpression3 == null)
			{
				sqlScalarExpression3 = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression4 = SqlScalarExpression.Create(sqlScalarExpression3, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression4.Position = yyloc;
			yyval.value = sqlScalarExpression4;
			break;
		}
		case 2474:
		{
			SqlScalarExpression sqlScalarExpression = value_stack.array[value_stack.top - 2].value as SqlScalarRefExpression;
			if (sqlScalarExpression == null)
			{
				sqlScalarExpression = ShiftReduceParser.UpdatePos(new SqlNullScalarExpression(), location_stack.array[location_stack.top - 2]);
			}
			SqlScalarExpression sqlScalarExpression2 = SqlScalarExpression.Create(sqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlCollation);
			sqlScalarExpression2.Position = yyloc;
			yyval.value = sqlScalarExpression2;
			break;
		}
		case 2475:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression3 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Positive, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression3.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression3;
			break;
		}
		case 2476:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression2 = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.Negative, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression2.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression2;
			break;
		}
		case 2477:
		{
			SqlUnaryScalarExpression sqlUnaryScalarExpression = new SqlUnaryScalarExpression(SqlUnaryScalarOperatorType.BitwiseNot, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlUnaryScalarExpression.Position = yyloc;
			yyval.value = sqlUnaryScalarExpression;
			break;
		}
		case 2478:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression8 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Multiply, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression8.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression8;
			break;
		}
		case 2479:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression7 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Divide, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression7.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression7;
			break;
		}
		case 2480:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression6 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Modulus, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression6.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression6;
			break;
		}
		case 2481:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression5 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Add, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression5.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression5;
			break;
		}
		case 2482:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression4 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.Subtract, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression4.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression4;
			break;
		}
		case 2483:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression3 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseAnd, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression3.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression3;
			break;
		}
		case 2484:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression2 = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseOr, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression2.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression2;
			break;
		}
		case 2485:
		{
			SqlBinaryScalarExpression sqlBinaryScalarExpression = new SqlBinaryScalarExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, SqlBinaryScalarOperatorType.BitwiseXor, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlBinaryScalarExpression.Position = yyloc;
			yyval.value = sqlBinaryScalarExpression;
			break;
		}
		case 2486:
		{
			SqlNullScalarExpression sqlNullScalarExpression = new SqlNullScalarExpression();
			sqlNullScalarExpression.Position = yyloc;
			yyval.value = sqlNullScalarExpression;
			break;
		}
		case 2487:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError(value_stack.array[value_stack.top - 1].value as SqlErrorCodeObject);
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2488:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier2)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2489:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(new SqlDataType(sqlObjectIdentifier)
			{
				Position = yyloc
			});
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 2494:
			if (value_stack.array[value_stack.top - 2].value is SqlCodeObject sqlCodeObject)
			{
				sqlCodeObject.Position = yyloc;
				yyval.value = sqlCodeObject;
			}
			break;
		case 2497:
		{
			SqlUdtInstanceMethodExpression sqlUdtInstanceMethodExpression = new SqlUdtInstanceMethodExpression(value_stack.array[value_stack.top - 6].value as SqlScalarExpression, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtInstanceMethodExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceMethodExpression;
			break;
		}
		case 2498:
		{
			SqlUdtInstanceDataMemberExpression sqlUdtInstanceDataMemberExpression = new SqlUdtInstanceDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtInstanceDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtInstanceDataMemberExpression;
			break;
		}
		case 2499:
		{
			SqlUdtStaticMethodExpression sqlUdtStaticMethodExpression = new SqlUdtStaticMethodExpression(value_stack.array[value_stack.top - 6].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 4].value as SqlIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlUdtStaticMethodExpression.Position = yyloc;
			yyval.value = sqlUdtStaticMethodExpression;
			break;
		}
		case 2450:
		case 2451:
		case 2456:
		case 2463:
		case 2464:
		case 2465:
		case 2467:
		case 2468:
		case 2469:
		case 2471:
		case 2490:
		case 2491:
		case 2492:
		case 2493:
		case 2495:
		case 2496:
			break;
		}
	}

	private void DoAction_2500_2549(int action)
	{
		switch (action)
		{
		case 2500:
		{
			SqlUdtStaticDataMemberExpression sqlUdtStaticDataMemberExpression = new SqlUdtStaticDataMemberExpression(value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlUdtStaticDataMemberExpression.Position = yyloc;
			yyval.value = sqlUdtStaticDataMemberExpression;
			break;
		}
		case 2507:
		{
			SqlSimpleCaseExpression sqlSimpleCaseExpression = new SqlSimpleCaseExpression(value_stack.array[value_stack.top - 4].value as SqlScalarExpression, value_stack.array[value_stack.top - 3].value as List<SqlSimpleWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSimpleCaseExpression.Position = yyloc;
			yyval.value = sqlSimpleCaseExpression;
			break;
		}
		case 2508:
		{
			SqlSearchedCaseExpression sqlSearchedCaseExpression = new SqlSearchedCaseExpression(value_stack.array[value_stack.top - 3].value as List<SqlSearchedWhenClause>, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlSearchedCaseExpression.Position = yyloc;
			yyval.value = sqlSearchedCaseExpression;
			break;
		}
		case 2511:
		{
			List<SqlSimpleWhenClause> list6 = new List<SqlSimpleWhenClause>();
			list6.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list6;
			break;
		}
		case 2512:
		{
			List<SqlSimpleWhenClause> list5 = value_stack.array[value_stack.top - 2].value as List<SqlSimpleWhenClause>;
			list5.Add(value_stack.array[value_stack.top - 1].value as SqlSimpleWhenClause);
			yyval.value = list5;
			break;
		}
		case 2513:
		{
			SqlSimpleWhenClause sqlSimpleWhenClause = new SqlSimpleWhenClause(value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSimpleWhenClause.Position = yyloc;
			yyval.value = sqlSimpleWhenClause;
			break;
		}
		case 2514:
		{
			List<SqlSearchedWhenClause> list4 = new List<SqlSearchedWhenClause>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list4;
			break;
		}
		case 2515:
		{
			List<SqlSearchedWhenClause> list3 = value_stack.array[value_stack.top - 2].value as List<SqlSearchedWhenClause>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlSearchedWhenClause);
			yyval.value = list3;
			break;
		}
		case 2516:
		{
			SqlSearchedWhenClause sqlSearchedWhenClause = new SqlSearchedWhenClause(value_stack.array[value_stack.top - 3].value as SqlBooleanExpression, value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			sqlSearchedWhenClause.Position = yyloc;
			yyval.value = sqlSearchedWhenClause;
			break;
		}
		case 2518:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2519:
		{
			SqlObjectIdentifier sqlObjectIdentifier6 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier6.Position = yyloc;
			yyval.value = sqlObjectIdentifier6;
			break;
		}
		case 2520:
		{
			List<SqlIdentifier> obj3 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj3.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier5 = SqlObjectIdentifier.Create(obj3);
			sqlObjectIdentifier5.Position = yyloc;
			yyval.value = sqlObjectIdentifier5;
			break;
		}
		case 2522:
		{
			SqlIdentifier sqlIdentifier3 = SqlIdentifier.Missing();
			sqlIdentifier3.Position = yyloc;
			yyval.value = sqlIdentifier3;
			break;
		}
		case 2528:
		{
			SqlObjectIdentifier sqlObjectIdentifier4 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier4.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression3 = new SqlColumnRefExpression(sqlObjectIdentifier4);
			sqlColumnRefExpression3.Position = yyloc;
			yyval.value = sqlColumnRefExpression3;
			break;
		}
		case 2529:
		{
			List<SqlIdentifier> obj2 = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier3 = SqlObjectIdentifier.Create(obj2);
			sqlObjectIdentifier3.Position = yyloc;
			SqlScalarRefExpression sqlScalarRefExpression = SqlScalarRefExpression.ColumnOrPropertyRefExpression(sqlObjectIdentifier3);
			sqlScalarRefExpression.Position = yyloc;
			yyval.value = sqlScalarRefExpression;
			break;
		}
		case 2533:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression2 = new SqlColumnRefExpression(sqlObjectIdentifier2);
			sqlColumnRefExpression2.Position = yyloc;
			yyval.value = sqlColumnRefExpression2;
			break;
		}
		case 2534:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = yyloc;
			SqlColumnRefExpression sqlColumnRefExpression = new SqlColumnRefExpression(sqlObjectIdentifier, discardColumnPrefix: true);
			sqlColumnRefExpression.Position = yyloc;
			yyval.value = sqlColumnRefExpression;
			break;
		}
		case 2535:
		{
			List<SqlIdentifier> list2 = new List<SqlIdentifier>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list2;
			break;
		}
		case 2536:
		{
			List<SqlIdentifier> list = value_stack.array[value_stack.top - 2].value as List<SqlIdentifier>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			yyval.value = list;
			break;
		}
		case 2537:
		{
			SqlIdentifier sqlIdentifier2 = new SqlIdentifier(string.Empty);
			sqlIdentifier2.Position = yyloc;
			yyval.value = sqlIdentifier2;
			break;
		}
		case 2538:
			yyval = value_stack.array[value_stack.top - 2];
			break;
		case 2539:
		{
			SqlIdentifier sqlIdentifier = SqlIdentifier.Missing();
			sqlIdentifier.Position = location_stack.array[location_stack.top - 2];
			yyval.value = sqlIdentifier;
			break;
		}
		case 2541:
		{
			SqlTableRefExpression sqlTableRefExpression2 = new SqlTableRefExpression(value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlTableRefExpression2.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableRefExpression2, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2542:
		{
			SqlTableRefExpression sqlTableRefExpression = new SqlTableRefExpression(value_stack.array[value_stack.top - 4].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as List<SqlHint>);
			sqlTableRefExpression.Position = MergeLocationRange(location_stack.top - 4, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2543:
		{
			SqlTableValuedFunctionRefExpression sqlTableValuedFunctionRefExpression = new SqlTableValuedFunctionRefExpression(value_stack.array[value_stack.top - 5].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>);
			sqlTableValuedFunctionRefExpression.Position = MergeLocationRange(location_stack.top - 5, location_stack.top - 2);
			yyval.value = new DmlTarget(sqlTableValuedFunctionRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2544:
			yyval.value = new DmlTarget(value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		case 2545:
			yyval.value = new DmlTarget(value_stack.array[value_stack.top - 2].value as SqlTableExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		case 2546:
		{
			SqlTableVariableRefExpression sqlTableVariableRefExpression = new SqlTableVariableRefExpression(GetText(location_stack.array[location_stack.top - 2]));
			sqlTableVariableRefExpression.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableVariableRefExpression, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2547:
		{
			SqlTableExpressionError sqlTableExpressionError = new SqlTableExpressionError(value_stack.array[value_stack.top - 2].value as SqlErrorCodeObject);
			sqlTableExpressionError.Position = location_stack.array[location_stack.top - 2];
			yyval.value = new DmlTarget(sqlTableExpressionError, value_stack.array[value_stack.top - 1].value as List<SqlColumnRefExpression>);
			break;
		}
		case 2501:
		case 2502:
		case 2503:
		case 2504:
		case 2505:
		case 2506:
		case 2509:
		case 2510:
		case 2517:
		case 2521:
		case 2523:
		case 2524:
		case 2525:
		case 2526:
		case 2527:
		case 2530:
		case 2531:
		case 2532:
		case 2540:
		case 2548:
		case 2549:
			break;
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		case 2556:
		{
			SqlObjectReference sqlObjectReference2 = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlObjectReference2.Position = yyloc;
			yyval.value = sqlObjectReference2;
			break;
		}
		case 2557:
		{
			SqlObjectReference sqlObjectReference = new SqlObjectReference(value_stack.array[value_stack.top - 1].value as SqlScalarVariableRefExpression);
			sqlObjectReference.Position = yyloc;
			yyval.value = sqlObjectReference;
			break;
		}
		case 2560:
		{
			SqlLiteralExpression sqlLiteralExpression16 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression16.Position = yyloc;
			yyval.value = sqlLiteralExpression16;
			break;
		}
		case 2562:
		{
			SqlLiteralExpression sqlLiteralExpression15 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression15.Position = yyloc;
			yyval.value = sqlLiteralExpression15;
			break;
		}
		case 2563:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2564:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2569:
		{
			SqlLiteralExpression sqlLiteralExpression14 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression14.Position = yyloc;
			yyval.value = sqlLiteralExpression14;
			break;
		}
		case 2570:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 2571:
		{
			SqlLiteralExpression sqlLiteralExpression13 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression13.Position = yyloc;
			yyval.value = sqlLiteralExpression13;
			break;
		}
		case 2572:
			VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			break;
		case 2573:
		{
			SqlLiteralExpression sqlLiteralExpression12 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression12.Position = yyloc;
			yyval.value = sqlLiteralExpression12;
			break;
		}
		case 2574:
			if (IsMaxAbsoluteIntegerValue(GetText(location_stack.array[location_stack.top - 1])))
			{
				SqlLiteralExpression sqlLiteralExpression11 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
				sqlLiteralExpression11.Position = yyloc;
				yyval.value = sqlLiteralExpression11;
			}
			else
			{
				VersionedParserBase.ReportError(SR.IntegerValueOutOfRange(GetText(location_stack.array[location_stack.top - 1])), location_stack.array[location_stack.top - 1]);
			}
			break;
		case 2579:
		{
			SqlLiteralExpression sqlLiteralExpression10 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression10.Position = yyloc;
			yyval.value = sqlLiteralExpression10;
			break;
		}
		case 2580:
		{
			SqlLiteralExpression sqlLiteralExpression9 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression9.Position = yyloc;
			yyval.value = sqlLiteralExpression9;
			break;
		}
		case 2581:
		{
			SqlLiteralExpression sqlLiteralExpression8 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression8.Position = yyloc;
			yyval.value = sqlLiteralExpression8;
			break;
		}
		case 2582:
		{
			SqlLiteralExpression sqlLiteralExpression7 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression7.Position = yyloc;
			yyval.value = sqlLiteralExpression7;
			break;
		}
		case 2583:
		{
			SqlLiteralExpression sqlLiteralExpression6 = SqlLiteralExpression.Binary(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression6.Position = yyloc;
			yyval.value = sqlLiteralExpression6;
			break;
		}
		case 2593:
		{
			SqlLiteralExpression sqlLiteralExpression5 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression5.Position = yyloc;
			yyval.value = sqlLiteralExpression5;
			break;
		}
		case 2594:
		{
			SqlLiteralExpression sqlLiteralExpression4 = SqlLiteralExpression.Real(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression4.Position = yyloc;
			yyval.value = sqlLiteralExpression4;
			break;
		}
		case 2595:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Money(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression3.Position = yyloc;
			yyval.value = sqlLiteralExpression3;
			break;
		}
		case 2596:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Numeric(GetText(location_stack.array[location_stack.top - 1]), isNegative: true);
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2599:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Identifier(value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2550:
		case 2551:
		case 2552:
		case 2553:
		case 2554:
		case 2555:
		case 2558:
		case 2559:
		case 2561:
		case 2565:
		case 2566:
		case 2567:
		case 2568:
		case 2575:
		case 2576:
		case 2577:
		case 2578:
		case 2584:
		case 2585:
		case 2586:
		case 2587:
		case 2588:
		case 2589:
		case 2590:
		case 2591:
		case 2592:
		case 2597:
		case 2598:
			break;
		}
	}

	private void DoAction_2600_2649(int action)
	{
		switch (action)
		{
		case 2600:
		{
			SqlScalarExpressionError sqlScalarExpressionError = new SqlScalarExpressionError();
			sqlScalarExpressionError.Position = yyloc;
			yyval.value = sqlScalarExpressionError;
			break;
		}
		case 2603:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Default(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2604:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Null(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2605:
		{
			SqlScalarVariableRefExpression sqlScalarVariableRefExpression = SqlScalarVariableRefExpression.Create(GetText(location_stack.array[location_stack.top - 1]));
			sqlScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlScalarVariableRefExpression;
			break;
		}
		case 2606:
		{
			SqlGlobalScalarVariableRefExpression sqlGlobalScalarVariableRefExpression = new SqlGlobalScalarVariableRefExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlGlobalScalarVariableRefExpression.Position = yyloc;
			yyval.value = sqlGlobalScalarVariableRefExpression;
			break;
		}
		case 2634:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression8 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression8.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression8;
			break;
		}
		case 2635:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression7 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 4]), value_stack.array[value_stack.top - 2].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression7.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression7;
			break;
		}
		case 2636:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression6 = SqlBuiltinScalarFunctionCallExpression.Create(value_stack.array[value_stack.top - 5].value as string, value_stack.array[value_stack.top - 4].value as List<SqlScalarExpression>);
			sqlBuiltinScalarFunctionCallExpression6.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression6;
			break;
		}
		case 2637:
		{
			SqlAggregateFunctionCallExpression sqlAggregateFunctionCallExpression = SqlAggregateFunctionCallExpression.Create(GetText(location_stack.array[location_stack.top - 6]), (SqlSetQuantifier)value_stack.array[value_stack.top - 4].value, value_stack.array[value_stack.top - 3].value as SqlScalarExpression);
			sqlAggregateFunctionCallExpression.Position = yyloc;
			yyval.value = sqlAggregateFunctionCallExpression;
			break;
		}
		case 2639:
		{
			SqlCastExpression sqlCastExpression = new SqlCastExpression(value_stack.array[value_stack.top - 6].value as string, value_stack.array[value_stack.top - 3].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 5].value as SqlScalarExpression);
			sqlCastExpression.Position = yyloc;
			yyval.value = sqlCastExpression;
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
		case 2640:
		{
			SqlCodeObject sqlCodeObject = SqlBuiltinScalarFunctionCallExpression.CreateStar(GetText(location_stack.array[location_stack.top - 5]));
			if (sqlCodeObject == null)
			{
				sqlCodeObject = new SqlScalarExpressionError(new SqlErrorCodeObject(null));
				VersionedParserBase.SyntaxError("*", location_stack.array[location_stack.top - 3]);
			}
			sqlCodeObject.Position = yyloc;
			yyval.value = sqlCodeObject;
			break;
		}
		case 2641:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression5 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression5.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression5;
			break;
		}
		case 2642:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression4 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression4.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression4;
			break;
		}
		case 2643:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression3 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression3.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression3;
			break;
		}
		case 2644:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression2 = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression2.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression2;
			break;
		}
		case 2645:
		{
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = new SqlBuiltinScalarFunctionCallExpression(GetText(location_stack.array[location_stack.top - 1]));
			sqlBuiltinScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlBuiltinScalarFunctionCallExpression;
			break;
		}
		case 2647:
		{
			SqlConvertExpression sqlConvertExpression2 = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression2.Position = yyloc;
			yyval.value = sqlConvertExpression2;
			break;
		}
		case 2648:
		{
			SqlConvertExpression sqlConvertExpression = new SqlConvertExpression(GetText(location_stack.array[location_stack.top - 7]), value_stack.array[value_stack.top - 5].value as SqlDataTypeSpecification, value_stack.array[value_stack.top - 3].value as SqlScalarExpression, value_stack.array[value_stack.top - 2].value as SqlScalarExpression);
			sqlConvertExpression.Position = yyloc;
			yyval.value = sqlConvertExpression;
			break;
		}
		case 2649:
			yyval.value = GetText(location_stack.array[location_stack.top - 2]);
			break;
		case 2601:
		case 2602:
		case 2607:
		case 2608:
		case 2609:
		case 2610:
		case 2611:
		case 2612:
		case 2613:
		case 2614:
		case 2615:
		case 2616:
		case 2617:
		case 2618:
		case 2619:
		case 2620:
		case 2621:
		case 2622:
		case 2623:
		case 2624:
		case 2625:
		case 2626:
		case 2627:
		case 2628:
		case 2629:
		case 2630:
		case 2631:
		case 2632:
		case 2633:
		case 2638:
		case 2646:
			break;
		}
	}

	private void DoAction_2650_2699(int action)
	{
		switch (action)
		{
		case 2671:
		{
			List<SqlIdentifier> obj = value_stack.array[value_stack.top - 6].value as List<SqlIdentifier>;
			obj.Add(value_stack.array[value_stack.top - 5].value as SqlIdentifier);
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(obj);
			sqlObjectIdentifier.Position = LexLocation.Merge(location_stack.array[location_stack.top - 6], location_stack.array[location_stack.top - 5]);
			SqlUserDefinedScalarFunctionCallExpression sqlUserDefinedScalarFunctionCallExpression = new SqlUserDefinedScalarFunctionCallExpression(sqlObjectIdentifier, value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>);
			sqlUserDefinedScalarFunctionCallExpression.Position = yyloc;
			yyval.value = sqlUserDefinedScalarFunctionCallExpression;
			break;
		}
		case 2674:
		{
			List<SqlScalarExpression> list4 = new List<SqlScalarExpression>();
			list4.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list4;
			break;
		}
		case 2675:
		{
			List<SqlScalarExpression> list3 = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list3.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list3;
			break;
		}
		case 2678:
		{
			List<SqlScalarExpression> list2 = new List<SqlScalarExpression>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list2;
			break;
		}
		case 2679:
		{
			List<SqlScalarExpression> list = value_stack.array[value_stack.top - 3].value as List<SqlScalarExpression>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlScalarExpression);
			yyval.value = list;
			break;
		}
		case 2681:
			yyval = value_stack.array[value_stack.top - 1];
			break;
		case 2684:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification4 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 1].value as SqlDataType);
			sqlDataTypeSpecification4.Position = yyloc;
			yyval.value = sqlDataTypeSpecification4;
			break;
		}
		case 2685:
		{
			SqlLiteralExpression sqlLiteralExpression3 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression3.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification3 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 4].value as SqlDataType, sqlLiteralExpression3);
			sqlDataTypeSpecification3.Position = yyloc;
			yyval.value = sqlDataTypeSpecification3;
			break;
		}
		case 2686:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 4]));
			sqlLiteralExpression.Position = location_stack.array[location_stack.top - 4];
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.Integer(GetText(location_stack.array[location_stack.top - 2]));
			sqlLiteralExpression2.Position = location_stack.array[location_stack.top - 2];
			SqlDataTypeSpecification sqlDataTypeSpecification2 = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 6].value as SqlDataType, sqlLiteralExpression, sqlLiteralExpression2);
			sqlDataTypeSpecification2.Position = yyloc;
			yyval.value = sqlDataTypeSpecification2;
			break;
		}
		case 2687:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = new SqlDataTypeSpecification(value_stack.array[value_stack.top - 5].value as SqlDataType, value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 2].value as SqlObjectIdentifier);
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 2688:
			yyval.value = SqlSetQuantifier.All;
			break;
		case 2689:
			yyval.value = SqlSetQuantifier.Distinct;
			break;
		case 2690:
		{
			SqlUpdateBooleanExpression sqlUpdateBooleanExpression = new SqlUpdateBooleanExpression(value_stack.array[value_stack.top - 2].value as SqlIdentifier);
			sqlUpdateBooleanExpression.Position = yyloc;
			yyval.value = sqlUpdateBooleanExpression;
			break;
		}
		case 2650:
		case 2651:
		case 2652:
		case 2653:
		case 2654:
		case 2655:
		case 2656:
		case 2657:
		case 2658:
		case 2659:
		case 2660:
		case 2661:
		case 2662:
		case 2663:
		case 2664:
		case 2665:
		case 2666:
		case 2667:
		case 2668:
		case 2669:
		case 2670:
		case 2672:
		case 2673:
		case 2676:
		case 2677:
		case 2680:
		case 2682:
		case 2683:
		case 2691:
		case 2692:
		case 2693:
		case 2694:
		case 2695:
		case 2696:
		case 2697:
		case 2698:
		case 2699:
			break;
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		case 2748:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2749:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 1]))
			{
				Position = yyloc
			});
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2700:
		case 2701:
		case 2702:
		case 2703:
		case 2704:
		case 2705:
		case 2706:
		case 2707:
		case 2708:
		case 2709:
		case 2710:
		case 2711:
		case 2712:
		case 2713:
		case 2714:
		case 2715:
		case 2716:
		case 2717:
		case 2718:
		case 2719:
		case 2720:
		case 2721:
		case 2722:
		case 2723:
		case 2724:
		case 2725:
		case 2726:
		case 2727:
		case 2728:
		case 2729:
		case 2730:
		case 2731:
		case 2732:
		case 2733:
		case 2734:
		case 2735:
		case 2736:
		case 2737:
		case 2738:
		case 2739:
		case 2740:
		case 2741:
		case 2742:
		case 2743:
		case 2744:
		case 2745:
		case 2746:
		case 2747:
			break;
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		case 2750:
		{
			SqlObjectIdentifier sqlObjectIdentifier2 = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier2.Position = yyloc;
			yyval.value = sqlObjectIdentifier2;
			break;
		}
		case 2751:
		{
			SqlObjectIdentifier sqlObjectIdentifier = SqlObjectIdentifier.Create(value_stack.array[value_stack.top - 5].value as SqlIdentifier, value_stack.array[value_stack.top - 3].value as SqlIdentifier, value_stack.array[value_stack.top - 1].value as SqlIdentifier);
			sqlObjectIdentifier.Position = yyloc;
			yyval.value = sqlObjectIdentifier;
			break;
		}
		case 2756:
		{
			SqlLiteralExpression sqlLiteralExpression2 = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression2.Position = yyloc;
			yyval.value = sqlLiteralExpression2;
			break;
		}
		case 2757:
		{
			SqlLiteralExpression sqlLiteralExpression = SqlLiteralExpression.String(GetText(location_stack.array[location_stack.top - 1]));
			sqlLiteralExpression.Position = yyloc;
			yyval.value = sqlLiteralExpression;
			break;
		}
		case 2752:
		case 2753:
		case 2754:
		case 2755:
		case 2758:
		case 2759:
		case 2760:
		case 2761:
		case 2762:
		case 2763:
		case 2764:
		case 2765:
		case 2766:
		case 2767:
		case 2768:
		case 2769:
		case 2770:
		case 2771:
		case 2772:
		case 2773:
		case 2774:
		case 2775:
		case 2776:
		case 2777:
		case 2778:
		case 2779:
		case 2780:
		case 2781:
		case 2782:
		case 2783:
		case 2784:
		case 2785:
		case 2786:
		case 2787:
		case 2788:
		case 2789:
		case 2790:
		case 2791:
		case 2792:
		case 2793:
		case 2794:
		case 2795:
		case 2796:
		case 2797:
		case 2798:
		case 2799:
			break;
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		case 2831:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2832:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2800:
		case 2801:
		case 2802:
		case 2803:
		case 2804:
		case 2805:
		case 2806:
		case 2807:
		case 2808:
		case 2809:
		case 2810:
		case 2811:
		case 2812:
		case 2813:
		case 2814:
		case 2815:
		case 2816:
		case 2817:
		case 2818:
		case 2819:
		case 2820:
		case 2821:
		case 2822:
		case 2823:
		case 2824:
		case 2825:
		case 2826:
		case 2827:
		case 2828:
		case 2829:
		case 2830:
		case 2833:
		case 2834:
		case 2835:
		case 2836:
		case 2837:
		case 2838:
		case 2839:
		case 2840:
		case 2841:
		case 2842:
		case 2843:
		case 2844:
		case 2845:
		case 2846:
		case 2847:
		case 2848:
		case 2849:
			break;
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		case 2858:
		{
			SqlCreateSynonymStatement sqlCreateSynonymStatement = new SqlCreateSynonymStatement(value_stack.array[value_stack.top - 3].value as SqlObjectIdentifier, value_stack.array[value_stack.top - 1].value as SqlObjectIdentifier);
			sqlCreateSynonymStatement.Position = yyloc;
			yyval.value = sqlCreateSynonymStatement;
			break;
		}
		case 2850:
		case 2851:
		case 2852:
		case 2853:
		case 2854:
		case 2855:
		case 2856:
		case 2857:
		case 2859:
		case 2860:
		case 2861:
		case 2862:
		case 2863:
		case 2864:
		case 2865:
		case 2866:
		case 2867:
		case 2868:
		case 2869:
		case 2870:
		case 2871:
		case 2872:
		case 2873:
		case 2874:
		case 2875:
		case 2876:
		case 2877:
		case 2878:
		case 2879:
		case 2880:
		case 2881:
		case 2882:
		case 2883:
		case 2884:
		case 2885:
		case 2886:
		case 2887:
		case 2888:
		case 2889:
		case 2890:
		case 2891:
		case 2892:
		case 2893:
		case 2894:
		case 2895:
		case 2896:
		case 2897:
		case 2898:
		case 2899:
			break;
		}
	}

	private void DoAction_2900_2949(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		case 3013:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3014:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3023:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3024:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3025:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3026:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3000:
		case 3001:
		case 3002:
		case 3003:
		case 3004:
		case 3005:
		case 3006:
		case 3007:
		case 3008:
		case 3009:
		case 3010:
		case 3011:
		case 3012:
		case 3015:
		case 3016:
		case 3017:
		case 3018:
		case 3019:
		case 3020:
		case 3021:
		case 3022:
		case 3027:
		case 3028:
		case 3029:
		case 3030:
		case 3031:
		case 3032:
		case 3033:
		case 3034:
		case 3035:
		case 3036:
		case 3037:
		case 3038:
		case 3039:
		case 3040:
		case 3041:
		case 3042:
		case 3043:
		case 3044:
		case 3045:
		case 3046:
		case 3047:
		case 3048:
		case 3049:
			break;
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		case 3052:
			scanner.SetGDRState();
			break;
		case 3053:
			scanner.SetGDRState();
			break;
		case 3054:
			scanner.SetGDRState();
			break;
		case 3050:
		case 3051:
		case 3055:
		case 3056:
		case 3057:
		case 3058:
		case 3059:
		case 3060:
		case 3061:
		case 3062:
		case 3063:
		case 3064:
		case 3065:
		case 3066:
		case 3067:
		case 3068:
		case 3069:
		case 3070:
		case 3071:
		case 3072:
		case 3073:
		case 3074:
		case 3075:
		case 3076:
		case 3077:
		case 3078:
		case 3079:
		case 3080:
		case 3081:
		case 3082:
		case 3083:
		case 3084:
		case 3085:
		case 3086:
		case 3087:
		case 3088:
		case 3089:
		case 3090:
		case 3091:
		case 3092:
		case 3093:
		case 3094:
		case 3095:
		case 3096:
		case 3097:
		case 3098:
		case 3099:
			break;
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		case 3182:
			yyval.value = null;
			break;
		case 3183:
			yyval.value = GetText(location_stack.array[location_stack.top - 1]);
			break;
		case 3191:
			yyval.value = SqlAssignmentOperatorType.None;
			break;
		case 3198:
		{
			SqlSetClause sqlSetClause = new SqlSetClause(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClause.Position = yyloc;
			yyval.value = sqlSetClause;
			break;
		}
		case 3199:
		{
			SqlSetClauseError sqlSetClauseError = new SqlSetClauseError(value_stack.array[value_stack.top - 1].value as List<SqlAssignment>);
			sqlSetClauseError.Position = yyloc;
			yyval.value = sqlSetClauseError;
			break;
		}
		case 3150:
		case 3151:
		case 3152:
		case 3153:
		case 3154:
		case 3155:
		case 3156:
		case 3157:
		case 3158:
		case 3159:
		case 3160:
		case 3161:
		case 3162:
		case 3163:
		case 3164:
		case 3165:
		case 3166:
		case 3167:
		case 3168:
		case 3169:
		case 3170:
		case 3171:
		case 3172:
		case 3173:
		case 3174:
		case 3175:
		case 3176:
		case 3177:
		case 3178:
		case 3179:
		case 3180:
		case 3181:
		case 3184:
		case 3185:
		case 3186:
		case 3187:
		case 3188:
		case 3189:
		case 3190:
		case 3192:
		case 3193:
		case 3194:
		case 3195:
		case 3196:
		case 3197:
			break;
		}
	}

	private void DoAction_3200_3244(int action)
	{
		switch (action)
		{
		case 3203:
		{
			SqlDataTypeSpecification sqlDataTypeSpecification = SqlDataTypeSpecification.Error();
			sqlDataTypeSpecification.Position = yyloc;
			yyval.value = sqlDataTypeSpecification;
			break;
		}
		case 3204:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3205:
		{
			SqlCompoundStatement sqlCompoundStatement2 = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement2.Position = yyloc;
			yyval.value = sqlCompoundStatement2;
			break;
		}
		case 3206:
		{
			SqlStatement sqlStatement6 = new SqlStatementError();
			sqlStatement6.Position = location_stack.array[location_stack.top - 1];
			SqlCompoundStatement sqlCompoundStatement = new SqlCompoundStatement(new List<SqlStatement> { sqlStatement6 });
			sqlCompoundStatement.Position = yyloc;
			yyval.value = sqlCompoundStatement;
			VersionedParserBase.SyntaxError("end", location_stack.array[location_stack.top - 1]);
			break;
		}
		case 3207:
		{
			SqlCompoundStatement sqlCompoundStatement3 = new SqlCompoundStatement(value_stack.array[value_stack.top - 2].value as List<SqlStatement>);
			sqlCompoundStatement3.Position = yyloc;
			yyval.value = sqlCompoundStatement3;
			break;
		}
		case 3208:
		{
			SqlStatement sqlStatement11 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement11 == null)
			{
				sqlStatement11 = new SqlNullStatement();
			}
			sqlStatement11.Position = yyloc;
			yyval.value = sqlStatement11;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement11);
			break;
		}
		case 3209:
		{
			SqlStatement sqlStatement4 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement4 == null)
			{
				sqlStatement4 = new SqlNullStatement();
			}
			sqlStatement4.Position = yyloc;
			yyval.value = sqlStatement4;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement4);
			break;
		}
		case 3210:
		{
			SqlStatement sqlStatement = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement == null)
			{
				sqlStatement = new SqlNullStatement();
			}
			sqlStatement.Position = yyloc;
			yyval.value = sqlStatement;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement);
			break;
		}
		case 3211:
		{
			SqlStatement sqlStatement9 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement9 == null)
			{
				sqlStatement9 = new SqlNullStatement();
			}
			sqlStatement9.Position = yyloc;
			yyval.value = sqlStatement9;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement9);
			break;
		}
		case 3212:
		{
			SqlStatement sqlStatement7 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement7 == null)
			{
				sqlStatement7 = new SqlNullStatement();
			}
			sqlStatement7.Position = yyloc;
			yyval.value = sqlStatement7;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement7);
			break;
		}
		case 3213:
		{
			SqlStatement sqlStatement2 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement2 == null)
			{
				sqlStatement2 = new SqlNullStatement();
			}
			sqlStatement2.Position = yyloc;
			yyval.value = sqlStatement2;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement2);
			break;
		}
		case 3214:
		{
			SqlStatement sqlStatement12 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement12 == null)
			{
				sqlStatement12 = new SqlNullStatement();
			}
			sqlStatement12.Position = yyloc;
			yyval.value = sqlStatement12;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement12);
			break;
		}
		case 3215:
		{
			SqlStatement sqlStatement10 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement10 == null)
			{
				sqlStatement10 = new SqlNullStatement();
			}
			sqlStatement10.Position = yyloc;
			yyval.value = sqlStatement10;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement10);
			break;
		}
		case 3216:
		{
			SqlStatement sqlStatement8 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement8 == null)
			{
				sqlStatement8 = new SqlNullStatement();
			}
			sqlStatement8.Position = yyloc;
			yyval.value = sqlStatement8;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement8);
			break;
		}
		case 3217:
		{
			SqlStatement sqlStatement5 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement5 == null)
			{
				sqlStatement5 = new SqlNullStatement();
			}
			sqlStatement5.Position = yyloc;
			yyval.value = sqlStatement5;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement5);
			break;
		}
		case 3218:
		{
			SqlStatement sqlStatement3 = value_stack.array[value_stack.top - 1].value as SqlStatement;
			if (sqlStatement3 == null)
			{
				sqlStatement3 = new SqlNullStatement();
			}
			sqlStatement3.Position = yyloc;
			yyval.value = sqlStatement3;
			ShiftReduceParser.currentParseContext.AddLastStatement(sqlStatement3);
			break;
		}
		case 3219:
		{
			List<SqlIndexedColumn> list2 = new List<SqlIndexedColumn>();
			list2.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list2;
			break;
		}
		case 3220:
		{
			List<SqlIndexedColumn> list = value_stack.array[value_stack.top - 3].value as List<SqlIndexedColumn>;
			list.Add(value_stack.array[value_stack.top - 1].value as SqlIndexedColumn);
			yyval.value = list;
			break;
		}
		case 3221:
		{
			SqlIndexedColumn sqlIndexedColumn2 = new SqlIndexedColumn(value_stack.array[value_stack.top - 2].value as SqlIdentifier, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn2.Position = yyloc;
			yyval.value = sqlIndexedColumn2;
			break;
		}
		case 3222:
		{
			SqlIndexedColumn sqlIndexedColumn = new SqlIndexedColumn(new SqlIdentifier(GetText(location_stack.array[location_stack.top - 2]))
			{
				Position = location_stack.array[location_stack.top - 2]
			}, (SqlSortOrder)value_stack.array[value_stack.top - 1].value);
			sqlIndexedColumn.Position = yyloc;
			yyval.value = sqlIndexedColumn;
			break;
		}
		case 3229:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3230:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3231:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3232:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3233:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3234:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3235:
			scanner.SetGDRState();
			break;
		case 3236:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3237:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3238:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3239:
		{
			int tokenId = 173;
			if (OptLookup.OptStmtStarters.TryGetTokenId(scanner.GetStringToken(), ref tokenId))
			{
				scanner.RepeatToken(tokenId);
			}
			else
			{
				scanner.RepeatToken();
			}
			break;
		}
		case 3240:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3241:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3242:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3243:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3244:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 3200:
		case 3201:
		case 3202:
		case 3223:
		case 3224:
		case 3225:
		case 3226:
		case 3227:
		case 3228:
			break;
		}
	}
}
