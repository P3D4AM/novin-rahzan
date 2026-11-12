#!/usr/bin/env python3
"""re_ocx.py — reverse the RealSvrOcxTcp.ocx (device push listener) 32-bit MFC ActiveX.

Finds references to the RTLOG001/002/003 envelope strings and disassembles around them
to recover the wire framing the device uses when pushing JSON logs to the server.
"""
import sys
import pefile
from capstone import Cs, CS_ARCH_X86, CS_MODE_32

PATH = '/tmp/novin-hozor/RealSvrOcxTcp.ocx'

STRINGS = [b'RTLOG001', b'RTLOG002', b'RTLOG003', b'FanHostPort', b'log_id', b'result']


def load():
    pe = pefile.PE(PATH)
    base = pe.OPTIONAL_HEADER.ImageBase
    imp = {}
    pe.parse_data_directories(directories=[
        pefile.DIRECTORY_ENTRY['IMAGE_DIRECTORY_ENTRY_IMPORT']])
    if hasattr(pe, 'DIRECTORY_ENTRY_IMPORT'):
        for entry in pe.DIRECTORY_ENTRY_IMPORT:
            dll = entry.dll.decode()
            for t in entry.imports:
                if t.name:
                    imp[t.address] = f'{dll}!{t.name.decode()}'
    return pe, base, imp


def find_strings(pe, base):
    """Return {va: bytes} for each target string (ASCII, null-terminated)."""
    out = {}
    data = pe.__data__
    for s in STRINGS:
        start = 0
        while True:
            off = data.find(s + b'\x00', start)
            if off < 0:
                break
            # file offset -> rva -> va
            try:
                rva = pe.get_rva_from_offset(off)
                out[base + rva] = s.decode()
            except Exception:
                pass
            start = off + 1
    return out


def section(pe, name):
    for s in pe.sections:
        if s.Name.decode(errors='replace').rstrip('\x00') == name:
            return s
    return None


def main():
    pe, base, imp = load()
    machine = pe.FILE_HEADER.Machine
    print(f'machine: {machine:#x} ({"x86" if machine == 0x14c else "x64" if machine == 0x8664 else "?"})  base: {base:#x}')

    strvas = find_strings(pe, base)
    for va, s in sorted(strvas.items()):
        print(f'string "{s}" at VA {va:#x}')
    print()

    text = section(pe, '.text')
    if text is None:
        print('no .text'); return
    text_rva = text.VirtualAddress
    text_va = base + text_rva
    text_bytes = text.get_data()
    print(f'.text: VA {text_va:#x} size {len(text_bytes):#x}')

    md = Cs(CS_ARCH_X86, CS_MODE_32)
    md.detail = False

    # scan .text for any 4-byte LE occurrence of each string VA
    for va, s in sorted(strvas.items()):
        needle = va.to_bytes(4, 'little')
        start = 0
        hits = []
        while True:
            idx = text_bytes.find(needle, start)
            if idx < 0:
                break
            hits.append(idx)
            start = idx + 1
        print(f'== refs to "{s}" ({va:#x}): {len(hits)} hit(s)')
        for idx in hits[:8]:
            rva = text_rva + idx
            # disassemble a window starting a bit before the ref instruction
            window_start = max(0, idx - 80)
            code = text_bytes[window_start:window_start + 260]
            print(f'--- ref at RVA {rva:#x} (file-off ctx {window_start:#x}) ---')
            for insn in md.disasm(code, base + text_rva + window_start):
                note = ''
                # annotate IAT calls
                if insn.mnemonic in ('call', 'jmp') and insn.op_str.startswith('dword ptr ['):
                    try:
                        ea = int(insn.op_str.split('[')[1].rstrip(']'), 16) if insn.op_str.startswith('dword ptr [0x') else None
                        if ea is None:
                            # dword ptr [reg + disp] — resolve only absolute [0x...]
                            pass
                        if ea and ea in imp:
                            note = f'  ; -> {imp[ea]}'
                    except Exception:
                        note = ''
                if insn.mnemonic in ('call', 'jmp') and insn.op_str.startswith('0x'):
                    try:
                        tgt = int(insn.op_str, 16)
                        if tgt in strvas:
                            note = f'  ; -> "{strvas[tgt]}"'
                    except Exception:
                        pass
                mark = ' <<<' if (insn.address - base - text_rva) == idx else ''
                print(f'  {insn.address - base:#08x}: {insn.mnemonic:<8} {insn.op_str}{note}{mark}')
        print()

    # port constants scan in .text
    for port in (5005, 7005, 4370, 14460, 10001, 20000, 30000, 0x9C40):
        for enc, name in ((4, 'imm32'), (2, 'imm16-ax')):
            if enc == 4:
                needle = port.to_bytes(4, 'little')
            else:
                needle = bytes([port & 0xff, port >> 8])
            cnt = text_bytes.count(needle)
            if cnt:
                print(f'port {port} as {name}: {cnt} occurrence(s) in .text')


if __name__ == '__main__':
    main()
