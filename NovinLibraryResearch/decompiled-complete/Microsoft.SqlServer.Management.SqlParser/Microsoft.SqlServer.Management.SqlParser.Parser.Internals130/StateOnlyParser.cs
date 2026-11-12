using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals130;

internal class StateOnlyParser : Babel.ParserGenerator.StateOnlyParser
{
	internal StateOnlyParser(SimpleParserScanner baseScanner)
		: base(baseScanner.CreateTemporaryParserScanner())
	{
	}

	public override Babel.ParserGenerator.StateOnlyParser Create()
	{
		return new StateOnlyParser(scanner);
	}

	public override void DoAction(int action)
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
			DoAction_3200_3249(action);
			break;
		case 65:
			DoAction_3250_3299(action);
			break;
		case 66:
			DoAction_3300_3349(action);
			break;
		case 67:
			DoAction_3350_3399(action);
			break;
		case 68:
			DoAction_3400_3449(action);
			break;
		case 69:
			DoAction_3450_3499(action);
			break;
		case 70:
			DoAction_3500_3549(action);
			break;
		case 71:
			DoAction_3550_3599(action);
			break;
		case 72:
			DoAction_3600_3635(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 34:
			scanner.ResetAllParseStates();
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 35:
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
		case 241:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 242:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 246:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 247:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 248:
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
		case 219:
		case 220:
		case 221:
		case 222:
		case 223:
		case 224:
		case 225:
		case 226:
		case 227:
		case 228:
		case 229:
		case 230:
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
		case 243:
		case 244:
		case 245:
		case 249:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_300_349(int action)
	{
		switch (action)
		{
		case 309:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 312:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 313:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 314:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 316:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 300:
		case 301:
		case 302:
		case 303:
		case 304:
		case 305:
		case 306:
		case 307:
		case 308:
		case 310:
		case 311:
		case 315:
		case 317:
		case 318:
		case 319:
		case 320:
		case 321:
		case 322:
		case 323:
		case 324:
		case 325:
		case 326:
		case 327:
		case 328:
		case 329:
		case 330:
		case 331:
		case 332:
		case 333:
		case 334:
		case 335:
		case 336:
		case 337:
		case 338:
		case 339:
		case 340:
		case 341:
		case 342:
		case 343:
		case 344:
		case 345:
		case 346:
		case 347:
		case 348:
		case 349:
			break;
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
		}
	}

	private void DoAction_450_499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_500_549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_650_699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		case 793:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 750:
		case 751:
		case 752:
		case 753:
		case 754:
		case 755:
		case 756:
		case 757:
		case 758:
		case 759:
		case 760:
		case 761:
		case 762:
		case 763:
		case 764:
		case 765:
		case 766:
		case 767:
		case 768:
		case 769:
		case 770:
		case 771:
		case 772:
		case 773:
		case 774:
		case 775:
		case 776:
		case 777:
		case 778:
		case 779:
		case 780:
		case 781:
		case 782:
		case 783:
		case 784:
		case 785:
		case 786:
		case 787:
		case 788:
		case 789:
		case 790:
		case 791:
		case 792:
		case 794:
		case 795:
		case 796:
		case 797:
		case 798:
		case 799:
			break;
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1015:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1016:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1018:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 1019:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 1000:
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
		case 1017:
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
		case 1046:
		case 1047:
		case 1048:
		case 1049:
			break;
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
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
		case 1198:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 1199:
			scanner.SetParseState(EParseState.e_ParseDump);
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
		case 1183:
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
			break;
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		case 1213:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
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
		case 1224:
		case 1225:
		case 1226:
		case 1227:
		case 1228:
		case 1229:
		case 1230:
		case 1231:
		case 1232:
		case 1233:
		case 1234:
		case 1235:
		case 1236:
		case 1237:
		case 1238:
		case 1239:
		case 1240:
		case 1241:
		case 1242:
		case 1243:
		case 1244:
		case 1245:
		case 1246:
		case 1247:
		case 1248:
		case 1249:
			break;
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1256:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1250:
		case 1251:
		case 1252:
		case 1253:
		case 1254:
		case 1255:
		case 1257:
		case 1258:
		case 1259:
		case 1260:
		case 1261:
		case 1262:
		case 1263:
		case 1264:
		case 1265:
		case 1266:
		case 1267:
		case 1268:
		case 1269:
		case 1270:
		case 1271:
		case 1272:
		case 1273:
		case 1274:
		case 1275:
		case 1276:
		case 1277:
		case 1278:
		case 1279:
		case 1280:
		case 1281:
		case 1282:
		case 1283:
		case 1284:
		case 1285:
		case 1286:
		case 1287:
		case 1288:
		case 1289:
		case 1290:
		case 1291:
		case 1292:
		case 1293:
		case 1294:
		case 1295:
		case 1296:
		case 1297:
		case 1298:
		case 1299:
			break;
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		case 1309:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1300:
		case 1301:
		case 1302:
		case 1303:
		case 1304:
		case 1305:
		case 1306:
		case 1307:
		case 1308:
		case 1310:
		case 1311:
		case 1312:
		case 1313:
		case 1314:
		case 1315:
		case 1316:
		case 1317:
		case 1318:
		case 1319:
		case 1320:
		case 1321:
		case 1322:
		case 1323:
		case 1324:
		case 1325:
		case 1326:
		case 1327:
		case 1328:
		case 1329:
		case 1330:
		case 1331:
		case 1332:
		case 1333:
		case 1334:
		case 1335:
		case 1336:
		case 1337:
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
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
		case 1447:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1400:
		case 1401:
		case 1402:
		case 1403:
		case 1404:
		case 1405:
		case 1406:
		case 1407:
		case 1408:
		case 1409:
		case 1410:
		case 1411:
		case 1412:
		case 1413:
		case 1414:
		case 1415:
		case 1416:
		case 1417:
		case 1418:
		case 1419:
		case 1420:
		case 1421:
		case 1422:
		case 1423:
		case 1424:
		case 1425:
		case 1426:
		case 1427:
		case 1428:
		case 1429:
		case 1430:
		case 1431:
		case 1432:
		case 1433:
		case 1434:
		case 1435:
		case 1436:
		case 1437:
		case 1438:
		case 1439:
		case 1440:
		case 1441:
		case 1442:
		case 1443:
		case 1444:
		case 1445:
		case 1446:
		case 1448:
		case 1449:
			break;
		}
	}

	private void DoAction_1450_1499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		case 1552:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1593:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1596:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1597:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1550:
		case 1551:
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
		case 1576:
		case 1577:
		case 1578:
		case 1579:
		case 1580:
		case 1581:
		case 1582:
		case 1583:
		case 1584:
		case 1585:
		case 1586:
		case 1587:
		case 1588:
		case 1589:
		case 1590:
		case 1591:
		case 1592:
		case 1594:
		case 1595:
		case 1598:
		case 1599:
			break;
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1654:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1668:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 1650:
		case 1651:
		case 1652:
		case 1653:
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
		case 1669:
		case 1670:
		case 1671:
		case 1672:
		case 1673:
		case 1674:
		case 1675:
		case 1676:
		case 1677:
		case 1678:
		case 1679:
		case 1680:
		case 1681:
		case 1682:
		case 1683:
		case 1684:
		case 1685:
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
		case 1742:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 1700:
		case 1701:
		case 1702:
		case 1703:
		case 1704:
		case 1705:
		case 1706:
		case 1707:
		case 1708:
		case 1709:
		case 1710:
		case 1711:
		case 1712:
		case 1713:
		case 1714:
		case 1715:
		case 1716:
		case 1717:
		case 1718:
		case 1719:
		case 1720:
		case 1721:
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
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1900_1949(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		case 1995:
			scanner.SetStmtDelimiter(value: true);
			break;
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
		case 1992:
		case 1993:
		case 1994:
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
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		case 2066:
			scanner.SetGDRState();
			break;
		case 2067:
			scanner.SetGDRState();
			break;
		case 2068:
			scanner.SetGDRState();
			break;
		case 2070:
			scanner.SetGDRState();
			break;
		case 2071:
			scanner.SetGDRState();
			break;
		case 2086:
			scanner.SetGDRState();
			break;
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
		case 2064:
		case 2065:
		case 2069:
		case 2072:
		case 2073:
		case 2074:
		case 2075:
		case 2076:
		case 2077:
		case 2078:
		case 2079:
		case 2080:
		case 2081:
		case 2082:
		case 2083:
		case 2084:
		case 2085:
		case 2087:
		case 2088:
		case 2089:
		case 2090:
		case 2091:
		case 2092:
		case 2093:
		case 2094:
		case 2095:
		case 2096:
		case 2097:
		case 2098:
		case 2099:
			break;
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2150_2199(int action)
	{
		switch (action)
		{
		case 2179:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 2150:
		case 2151:
		case 2152:
		case 2153:
		case 2154:
		case 2155:
		case 2156:
		case 2157:
		case 2158:
		case 2159:
		case 2160:
		case 2161:
		case 2162:
		case 2163:
		case 2164:
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
		case 2180:
		case 2181:
		case 2182:
		case 2183:
		case 2184:
		case 2185:
		case 2186:
		case 2187:
		case 2188:
		case 2189:
		case 2190:
		case 2191:
		case 2192:
		case 2193:
		case 2194:
		case 2195:
		case 2196:
		case 2197:
		case 2198:
		case 2199:
			break;
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2250_2299(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2300_2349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2350_2399(int action)
	{
		switch (action)
		{
		case 2381:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2350:
		case 2351:
		case 2352:
		case 2353:
		case 2354:
		case 2355:
		case 2356:
		case 2357:
		case 2358:
		case 2359:
		case 2360:
		case 2361:
		case 2362:
		case 2363:
		case 2364:
		case 2365:
		case 2366:
		case 2367:
		case 2368:
		case 2369:
		case 2370:
		case 2371:
		case 2372:
		case 2373:
		case 2374:
		case 2375:
		case 2376:
		case 2377:
		case 2378:
		case 2379:
		case 2380:
		case 2382:
		case 2383:
		case 2384:
		case 2385:
		case 2386:
		case 2387:
		case 2388:
		case 2389:
		case 2390:
		case 2391:
		case 2392:
		case 2393:
		case 2394:
		case 2395:
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
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2500_2549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2600_2649(int action)
	{
		switch (action)
		{
		case 2633:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2637:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2639:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2600:
		case 2601:
		case 2602:
		case 2603:
		case 2604:
		case 2605:
		case 2606:
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
		case 2634:
		case 2635:
		case 2636:
		case 2638:
		case 2640:
		case 2641:
		case 2642:
		case 2643:
		case 2644:
		case 2645:
		case 2646:
		case 2647:
		case 2648:
		case 2649:
			break;
		}
	}

	private void DoAction_2650_2699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2900_2949(int action)
	{
		switch (action)
		{
		case 2944:
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 2900:
		case 2901:
		case 2902:
		case 2903:
		case 2904:
		case 2905:
		case 2906:
		case 2907:
		case 2908:
		case 2909:
		case 2910:
		case 2911:
		case 2912:
		case 2913:
		case 2914:
		case 2915:
		case 2916:
		case 2917:
		case 2918:
		case 2919:
		case 2920:
		case 2921:
		case 2922:
		case 2923:
		case 2924:
		case 2925:
		case 2926:
		case 2927:
		case 2928:
		case 2929:
		case 2930:
		case 2931:
		case 2932:
		case 2933:
		case 2934:
		case 2935:
		case 2936:
		case 2937:
		case 2938:
		case 2939:
		case 2940:
		case 2941:
		case 2942:
		case 2943:
		case 2945:
		case 2946:
		case 2947:
		case 2948:
		case 2949:
			break;
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
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		case 3136:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3137:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3100:
		case 3101:
		case 3102:
		case 3103:
		case 3104:
		case 3105:
		case 3106:
		case 3107:
		case 3108:
		case 3109:
		case 3110:
		case 3111:
		case 3112:
		case 3113:
		case 3114:
		case 3115:
		case 3116:
		case 3117:
		case 3118:
		case 3119:
		case 3120:
		case 3121:
		case 3122:
		case 3123:
		case 3124:
		case 3125:
		case 3126:
		case 3127:
		case 3128:
		case 3129:
		case 3130:
		case 3131:
		case 3132:
		case 3133:
		case 3134:
		case 3135:
		case 3138:
		case 3139:
		case 3140:
		case 3141:
		case 3142:
		case 3143:
		case 3144:
		case 3145:
		case 3146:
		case 3147:
		case 3148:
		case 3149:
			break;
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3200_3249(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3250_3299(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3300_3349(int action)
	{
		switch (action)
		{
		case 3341:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3342:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3300:
		case 3301:
		case 3302:
		case 3303:
		case 3304:
		case 3305:
		case 3306:
		case 3307:
		case 3308:
		case 3309:
		case 3310:
		case 3311:
		case 3312:
		case 3313:
		case 3314:
		case 3315:
		case 3316:
		case 3317:
		case 3318:
		case 3319:
		case 3320:
		case 3321:
		case 3322:
		case 3323:
		case 3324:
		case 3325:
		case 3326:
		case 3327:
		case 3328:
		case 3329:
		case 3330:
		case 3331:
		case 3332:
		case 3333:
		case 3334:
		case 3335:
		case 3336:
		case 3337:
		case 3338:
		case 3339:
		case 3340:
		case 3343:
		case 3344:
		case 3345:
		case 3346:
		case 3347:
		case 3348:
		case 3349:
			break;
		}
	}

	private void DoAction_3350_3399(int action)
	{
		switch (action)
		{
		case 3351:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3352:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3353:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3354:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3380:
			scanner.SetGDRState();
			break;
		case 3381:
			scanner.SetGDRState();
			break;
		case 3382:
			scanner.SetGDRState();
			break;
		case 3350:
		case 3355:
		case 3356:
		case 3357:
		case 3358:
		case 3359:
		case 3360:
		case 3361:
		case 3362:
		case 3363:
		case 3364:
		case 3365:
		case 3366:
		case 3367:
		case 3368:
		case 3369:
		case 3370:
		case 3371:
		case 3372:
		case 3373:
		case 3374:
		case 3375:
		case 3376:
		case 3377:
		case 3378:
		case 3379:
		case 3383:
		case 3384:
		case 3385:
		case 3386:
		case 3387:
		case 3388:
		case 3389:
		case 3390:
		case 3391:
		case 3392:
		case 3393:
		case 3394:
		case 3395:
		case 3396:
		case 3397:
		case 3398:
		case 3399:
			break;
		}
	}

	private void DoAction_3400_3449(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3450_3499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3500_3549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3550_3599(int action)
	{
		switch (action)
		{
		case 3594:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3550:
		case 3551:
		case 3552:
		case 3553:
		case 3554:
		case 3555:
		case 3556:
		case 3557:
		case 3558:
		case 3559:
		case 3560:
		case 3561:
		case 3562:
		case 3563:
		case 3564:
		case 3565:
		case 3566:
		case 3567:
		case 3568:
		case 3569:
		case 3570:
		case 3571:
		case 3572:
		case 3573:
		case 3574:
		case 3575:
		case 3576:
		case 3577:
		case 3578:
		case 3579:
		case 3580:
		case 3581:
		case 3582:
		case 3583:
		case 3584:
		case 3585:
		case 3586:
		case 3587:
		case 3588:
		case 3589:
		case 3590:
		case 3591:
		case 3592:
		case 3593:
		case 3595:
		case 3596:
		case 3597:
		case 3598:
		case 3599:
			break;
		}
	}

	private void DoAction_3600_3635(int action)
	{
		switch (action)
		{
		case 3619:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3620:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3621:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3622:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3623:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3624:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3625:
			scanner.SetGDRState();
			break;
		case 3626:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3627:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3628:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3629:
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
		case 3630:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3631:
			scanner.SetQueryState(QueryState.IN_FROM_CLAUSE);
			break;
		case 3632:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3633:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3634:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3635:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 3600:
		case 3601:
		case 3602:
		case 3603:
		case 3604:
		case 3605:
		case 3606:
		case 3607:
		case 3608:
		case 3609:
		case 3610:
		case 3611:
		case 3612:
		case 3613:
		case 3614:
		case 3615:
		case 3616:
		case 3617:
		case 3618:
			break;
		}
	}
}
