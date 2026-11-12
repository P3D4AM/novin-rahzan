using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals140;

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
			DoAction_3600_3649(action);
			break;
		case 73:
			DoAction_3650_3699(action);
			break;
		case 74:
			DoAction_3700_3716(action);
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
		case 244:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 245:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 249:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
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
		case 241:
		case 242:
		case 243:
		case 246:
		case 247:
		case 248:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		case 250:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 251:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
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
		case 286:
		case 287:
		case 288:
		case 289:
		case 290:
		case 291:
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
		case 319:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 322:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 323:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 324:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 326:
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
		case 309:
		case 310:
		case 311:
		case 312:
		case 313:
		case 314:
		case 315:
		case 316:
		case 317:
		case 318:
		case 320:
		case 321:
		case 325:
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
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		case 824:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 800:
		case 801:
		case 802:
		case 803:
		case 804:
		case 805:
		case 806:
		case 807:
		case 808:
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
		case 822:
		case 823:
		case 825:
		case 826:
		case 827:
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
		case 849:
			break;
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
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
		case 1065:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1066:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 1068:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 1069:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 1050:
		case 1051:
		case 1052:
		case 1053:
		case 1054:
		case 1055:
		case 1056:
		case 1057:
		case 1058:
		case 1059:
		case 1060:
		case 1061:
		case 1062:
		case 1063:
		case 1064:
		case 1067:
		case 1070:
		case 1071:
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
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1254:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 1255:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 1269:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 1250:
		case 1251:
		case 1252:
		case 1253:
		case 1256:
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
		case 1312:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
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
		case 1309:
		case 1310:
		case 1311:
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
		case 1365:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1350:
		case 1351:
		case 1352:
		case 1353:
		case 1354:
		case 1355:
		case 1356:
		case 1357:
		case 1358:
		case 1359:
		case 1360:
		case 1361:
		case 1362:
		case 1363:
		case 1364:
		case 1366:
		case 1367:
		case 1368:
		case 1369:
		case 1370:
		case 1371:
		case 1372:
		case 1373:
		case 1374:
		case 1375:
		case 1376:
		case 1377:
		case 1378:
		case 1379:
		case 1380:
		case 1381:
		case 1382:
		case 1383:
		case 1384:
		case 1385:
		case 1386:
		case 1387:
		case 1388:
		case 1389:
		case 1390:
		case 1391:
		case 1392:
		case 1393:
		case 1394:
		case 1395:
		case 1396:
		case 1397:
		case 1398:
		case 1399:
			break;
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
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
		case 1503:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1500:
		case 1501:
		case 1502:
		case 1504:
		case 1505:
		case 1506:
		case 1507:
		case 1508:
		case 1509:
		case 1510:
		case 1511:
		case 1512:
		case 1513:
		case 1514:
		case 1515:
		case 1516:
		case 1517:
		case 1518:
		case 1519:
		case 1520:
		case 1521:
		case 1522:
		case 1523:
		case 1524:
		case 1525:
		case 1526:
		case 1527:
		case 1528:
		case 1529:
		case 1530:
		case 1531:
		case 1532:
		case 1533:
		case 1534:
		case 1535:
		case 1536:
		case 1537:
		case 1538:
		case 1539:
		case 1540:
		case 1541:
		case 1542:
		case 1543:
		case 1544:
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
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		case 1608:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1649:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1600:
		case 1601:
		case 1602:
		case 1603:
		case 1604:
		case 1605:
		case 1606:
		case 1607:
		case 1609:
		case 1610:
		case 1611:
		case 1612:
		case 1613:
		case 1614:
		case 1615:
		case 1616:
		case 1617:
		case 1618:
		case 1619:
		case 1620:
		case 1621:
		case 1622:
		case 1623:
		case 1624:
		case 1625:
		case 1626:
		case 1627:
		case 1628:
		case 1629:
		case 1630:
		case 1631:
		case 1632:
		case 1633:
		case 1634:
		case 1635:
		case 1636:
		case 1637:
		case 1638:
		case 1639:
		case 1640:
		case 1641:
		case 1642:
		case 1643:
		case 1644:
		case 1645:
		case 1646:
		case 1647:
		case 1648:
			break;
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1652:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1653:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1650:
		case 1651:
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
		case 1716:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1730:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
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
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1804:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 1800:
		case 1801:
		case 1802:
		case 1803:
		case 1805:
		case 1806:
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
		case 1823:
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
		case 2062:
			scanner.SetStmtDelimiter(value: true);
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
		case 2063:
		case 2064:
		case 2065:
		case 2066:
		case 2067:
		case 2068:
		case 2069:
		case 2070:
		case 2071:
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
		case 2086:
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
		case 2133:
			scanner.SetGDRState();
			break;
		case 2134:
			scanner.SetGDRState();
			break;
		case 2135:
			scanner.SetGDRState();
			break;
		case 2137:
			scanner.SetGDRState();
			break;
		case 2138:
			scanner.SetGDRState();
			break;
		case 2100:
		case 2101:
		case 2102:
		case 2103:
		case 2104:
		case 2105:
		case 2106:
		case 2107:
		case 2108:
		case 2109:
		case 2110:
		case 2111:
		case 2112:
		case 2113:
		case 2114:
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
		case 2126:
		case 2127:
		case 2128:
		case 2129:
		case 2130:
		case 2131:
		case 2132:
		case 2136:
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
		case 2153:
			scanner.SetGDRState();
			break;
		case 2150:
		case 2151:
		case 2152:
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
		case 2179:
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
		case 2246:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 2200:
		case 2201:
		case 2202:
		case 2203:
		case 2204:
		case 2205:
		case 2206:
		case 2207:
		case 2208:
		case 2209:
		case 2210:
		case 2211:
		case 2212:
		case 2213:
		case 2214:
		case 2215:
		case 2216:
		case 2217:
		case 2218:
		case 2219:
		case 2220:
		case 2221:
		case 2222:
		case 2223:
		case 2224:
		case 2225:
		case 2226:
		case 2227:
		case 2228:
		case 2229:
		case 2230:
		case 2231:
		case 2232:
		case 2233:
		case 2234:
		case 2235:
		case 2236:
		case 2237:
		case 2238:
		case 2239:
		case 2240:
		case 2241:
		case 2242:
		case 2243:
		case 2244:
		case 2245:
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
		}
	}

	private void DoAction_2400_2449(int action)
	{
		switch (action)
		{
		case 2448:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2400:
		case 2401:
		case 2402:
		case 2403:
		case 2404:
		case 2405:
		case 2406:
		case 2407:
		case 2408:
		case 2409:
		case 2410:
		case 2411:
		case 2412:
		case 2413:
		case 2414:
		case 2415:
		case 2416:
		case 2417:
		case 2418:
		case 2419:
		case 2420:
		case 2421:
		case 2422:
		case 2423:
		case 2424:
		case 2425:
		case 2426:
		case 2427:
		case 2428:
		case 2429:
		case 2430:
		case 2431:
		case 2432:
		case 2433:
		case 2434:
		case 2435:
		case 2436:
		case 2437:
		case 2438:
		case 2439:
		case 2440:
		case 2441:
		case 2442:
		case 2443:
		case 2444:
		case 2445:
		case 2446:
		case 2447:
		case 2449:
			break;
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
		case 2701:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2705:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2707:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2700:
		case 2702:
		case 2703:
		case 2704:
		case 2706:
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
		case 2748:
		case 2749:
			break;
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
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
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
		case 3014:
		case 3015:
		case 3016:
		case 3017:
		case 3018:
		case 3019:
		case 3020:
		case 3021:
		case 3022:
		case 3023:
		case 3024:
		case 3025:
		case 3026:
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
		}
	}

	private void DoAction_3200_3249(int action)
	{
		switch (action)
		{
		case 3211:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3212:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 3200:
		case 3201:
		case 3202:
		case 3203:
		case 3204:
		case 3205:
		case 3206:
		case 3207:
		case 3208:
		case 3209:
		case 3210:
		case 3213:
		case 3214:
		case 3215:
		case 3216:
		case 3217:
		case 3218:
		case 3219:
		case 3220:
		case 3221:
		case 3222:
		case 3223:
		case 3224:
		case 3225:
		case 3226:
		case 3227:
		case 3228:
		case 3229:
		case 3230:
		case 3231:
		case 3232:
		case 3233:
		case 3234:
		case 3235:
		case 3236:
		case 3237:
		case 3238:
		case 3239:
		case 3240:
		case 3241:
		case 3242:
		case 3243:
		case 3244:
		case 3245:
		case 3246:
		case 3247:
		case 3248:
		case 3249:
			break;
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
		}
	}

	private void DoAction_3350_3399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3400_3449(int action)
	{
		switch (action)
		{
		case 3416:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3417:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3426:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3427:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3428:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3429:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3400:
		case 3401:
		case 3402:
		case 3403:
		case 3404:
		case 3405:
		case 3406:
		case 3407:
		case 3408:
		case 3409:
		case 3410:
		case 3411:
		case 3412:
		case 3413:
		case 3414:
		case 3415:
		case 3418:
		case 3419:
		case 3420:
		case 3421:
		case 3422:
		case 3423:
		case 3424:
		case 3425:
		case 3430:
		case 3431:
		case 3432:
		case 3433:
		case 3434:
		case 3435:
		case 3436:
		case 3437:
		case 3438:
		case 3439:
		case 3440:
		case 3441:
		case 3442:
		case 3443:
		case 3444:
		case 3445:
		case 3446:
		case 3447:
		case 3448:
		case 3449:
			break;
		}
	}

	private void DoAction_3450_3499(int action)
	{
		switch (action)
		{
		case 3455:
			scanner.SetGDRState();
			break;
		case 3456:
			scanner.SetGDRState();
			break;
		case 3457:
			scanner.SetGDRState();
			break;
		case 3450:
		case 3451:
		case 3452:
		case 3453:
		case 3454:
		case 3458:
		case 3459:
		case 3460:
		case 3461:
		case 3462:
		case 3463:
		case 3464:
		case 3465:
		case 3466:
		case 3467:
		case 3468:
		case 3469:
		case 3470:
		case 3471:
		case 3472:
		case 3473:
		case 3474:
		case 3475:
		case 3476:
		case 3477:
		case 3478:
		case 3479:
		case 3480:
		case 3481:
		case 3482:
		case 3483:
		case 3484:
		case 3485:
		case 3486:
		case 3487:
		case 3488:
		case 3489:
		case 3490:
		case 3491:
		case 3492:
		case 3493:
		case 3494:
		case 3495:
		case 3496:
		case 3497:
		case 3498:
		case 3499:
			break;
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
		}
	}

	private void DoAction_3600_3649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3650_3699(int action)
	{
		switch (action)
		{
		case 3679:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3650:
		case 3651:
		case 3652:
		case 3653:
		case 3654:
		case 3655:
		case 3656:
		case 3657:
		case 3658:
		case 3659:
		case 3660:
		case 3661:
		case 3662:
		case 3663:
		case 3664:
		case 3665:
		case 3666:
		case 3667:
		case 3668:
		case 3669:
		case 3670:
		case 3671:
		case 3672:
		case 3673:
		case 3674:
		case 3675:
		case 3676:
		case 3677:
		case 3678:
		case 3680:
		case 3681:
		case 3682:
		case 3683:
		case 3684:
		case 3685:
		case 3686:
		case 3687:
		case 3688:
		case 3689:
		case 3690:
		case 3691:
		case 3692:
		case 3693:
		case 3694:
		case 3695:
		case 3696:
		case 3697:
		case 3698:
		case 3699:
			break;
		}
	}

	private void DoAction_3700_3716(int action)
	{
		switch (action)
		{
		case 3700:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3701:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3702:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3703:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3704:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3705:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3706:
			scanner.SetGDRState();
			break;
		case 3707:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3708:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3709:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3710:
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
		case 3711:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3712:
			scanner.SetQueryState(QueryState.IN_FROM_CLAUSE);
			break;
		case 3713:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3714:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3715:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3716:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
	}
}
