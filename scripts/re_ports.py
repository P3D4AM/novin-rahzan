#!/usr/bin/env python3
"""re_ports.py — scan vendor (NovinHozor) binaries for port constants & protocol keywords.

Goal: find the real-time PUSH listener (device→server channel, see logRealTimeDevice1.txt)
and which port/protocol the vendor's software actually uses with NP756+ ("756PLUS") devices.
"""
import os
import re
import struct

DIR = '/tmp/novin-hozor'
FILES = [
    'Server.Core.dll', 'SbLogServer.dll', 'ClientHubStandard.dll',
    'RealSvrOcxTcp.ocx', 'PacketParserStandard.dll',
    'ActivationPacketParserStandard.dll', 'TimeAttendanceDevices.dll',
    'TimeAttendance.exe', 'NPResourceManagement.dll', 'EntityCacheStandard.dll',
    'FaceDataConv.dll', 'FpDataConv.dll', 'SignalRCoreConnectionCore.dll',
    'SBPCCOMM.dll', 'SBPCCOMM64.dll', 'SBXPCDLL.dll', 'SBXPCDLL64.dll',
]

PORTS = [80, 443, 1433, 4238, 4370, 5005, 5010, 5011, 5555, 5577, 60420, 6789,
         8000, 8080, 8081, 8443, 8886, 9100, 10001, 14460, 20000, 30000]

KEYWORDS = [
    b'756PLUS', b'RealSvr', b'SbLog', b'PushServer', b'PushData', b'RealTime',
    b'realtime', b'Ebio', b'ebio', b'getLogInfo', b'getLogCount', b'getDeviceKey',
    b'http://', b'https://', b'TcpServer', b'tcpserver', b'Listen', b'listen',
    b'Bind', b'Accept', b'DeviceLogOnPerson', b'VisibleLight', b'60420',
    b'logRealTime', b'TransId', b'transId', b'ioMode', b'IoMode',
]


def ascii_strings(data, minlen=4):
    for m in re.finditer(rb'[\x20-\x7e]{%d,}' % minlen, data):
        yield m.start(), m.group()


def utf16_strings(data, minlen=4):
    for m in re.finditer(rb'(?:[\x20-\x7e]\x00){%d,}' % minlen, data):
        yield m.start(), m.group().decode('utf-16-le')


def main():
    for fn in FILES:
        path = os.path.join(DIR, fn)
        if not os.path.exists(path):
            continue
        data = open(path, 'rb').read()

        hits = set()
        for _off, s in ascii_strings(data):
            for kw in KEYWORDS:
                if kw in s:
                    hits.add(s.decode('latin1', 'replace')[:140])
                    break
            else:
                for p in PORTS:
                    if re.search(rb'(?<![0-9])%d(?![0-9])' % p, s):
                        hits.add('[port-str] ' + s.decode('latin1', 'replace')[:140])
                        break
        for _off, s in utf16_strings(data):
            for kw in KEYWORDS:
                if kw.decode('latin1') in s:
                    hits.add(s[:140])
                    break
            else:
                for p in PORTS:
                    if re.search(r'(?<![0-9])%d(?![0-9])' % p, s):
                        hits.add('[port-u16] ' + s[:140])
                        break

        dword_hits = []
        for p in PORTS:
            needle = struct.pack('<I', p)
            cnt = data.count(needle)
            if cnt:
                dword_hits.append(f'{p}x{cnt}')

        if hits or dword_hits:
            print('=' * 72)
            print(f'{fn}  |  dword-ports: {", ".join(dword_hits) if dword_hits else "-"}')
            for h in sorted(hits):
                print('    ' + (h.decode('latin1', 'replace') if isinstance(h, bytes) else h))
        else:
            print(f'-- {fn}: no hits')


if __name__ == '__main__':
    main()
