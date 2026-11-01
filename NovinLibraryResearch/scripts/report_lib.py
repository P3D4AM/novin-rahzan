# -*- coding: utf-8 -*-
"""Shared helpers for building the Persian RTL HTML report."""

import html as html_mod
import re

# ----------------------------------------------------------------------------
# Simple C# syntax highlighter (regex-based, good enough for teaching snippets)
# ----------------------------------------------------------------------------

KEYWORDS = {
    'public', 'private', 'protected', 'internal', 'static', 'class', 'void',
    'int', 'string', 'bool', 'byte', 'short', 'long', 'float', 'double',
    'decimal', 'char', 'object', 'var', 'new', 'return', 'if', 'else',
    'while', 'for', 'foreach', 'in', 'do', 'switch', 'case', 'break',
    'continue', 'try', 'catch', 'finally', 'throw', 'using', 'namespace',
    'async', 'await', 'task', 'get', 'set', 'override', 'virtual', 'sealed',
    'interface', 'enum', 'struct', 'this', 'base', 'null', 'true', 'false',
    'readonly', 'const', 'partial', 'ref', 'out', 'params', 'default',
    'where', 'is', 'as', 'not', 'and', 'or', 'global', 'record', 'init',
    'required', 'extends', 'implements', 'delegate', 'event', 'abstract',
}

TYPE_HINTS = {
    'SqlConnection', 'SqlCommand', 'SqlDataReader', 'SqlParameter',
    'SqlConnectionStringBuilder', 'DbConnection', 'DbCommand', 'DbDataReader',
    'CommandType', 'StoredProcedure', 'Task', 'List', 'Dictionary',
    'ConcurrentDictionary', 'SemaphoreSlim', 'CancellationToken',
    'CancellationTokenSource', 'DateTime', 'TimeSpan', 'Guid', 'EventArgs',
    'MessageBox', 'DialogResult', 'Form', 'Control', 'TextBox', 'Timer',
    'SerialPort', 'Stopwatch', 'HttpClient', 'HttpResponseMessage',
    'JObject', 'JsonConvert', 'StiReport', 'ReturnedSaveFuncInfo',
    'ReturnedSaveFuncInfoWithValue', 'DeviceLogOnPerson', 'LogOnPersonel',
    'Person', 'DeviceInfo', 'UserAttendance', 'WebMessage', 'Rfc2898DeriveBytes',
    'RijndaelManaged', 'CryptoStream', 'MemoryStream', 'ICryptoTransform',
    'Encoding', 'Convert', 'BitConverter', 'StringBuilder', 'Exception',
    'OperationCanceledException', 'HttpRequestException', 'InvalidOperationException',
    'GCHandle', 'Math', 'Convert.ToInt32', 'File', 'Path', 'Directory',
    'Application', 'Registry', 'Environment', 'Process', 'Thread',
}

def _highlight_code(code: str) -> str:
    """Add syntax-colored spans to C# code. Input must be plain text."""
    out = []
    for raw_line in code.split('\n'):
        line = raw_line
        # escape HTML first
        line = html_mod.escape(line)

        # blank line fast-path
        if not line.strip():
            out.append('')
            continue

        # comment part (only // style; // may appear inside strings rarely in our snippets)
        comment_html = ''
        m = re.search(r'(//.*)$', line)
        if m and '"' not in line[:m.start()].split('"')[0] + '""':
            # naive: only treat as comment if number of quotes before is even
            before = line[:m.start()]
            if before.count('"') % 2 == 0:
                comment_html = '<span class="c-com">' + m.group(1) + '</span>'
                line = before.rstrip()

        # tokenize: strings, chars, then words
        token_pat = re.compile(r'(&quot;.*?&quot;|@?&quot;|&#x27;.*?&#x27;|/\*|\*/)')
        parts = token_pat.split(line)
        built = []
        for part in parts:
            if not part:
                continue
            if part.startswith('&quot;') or part.startswith('&#x27;'):
                built.append('<span class="c-str">' + part + '</span>')
            else:
                # highlight keywords / types / numbers
                word_pat = re.compile(r'([A-Za-z_][A-Za-z0-9_]*|\d+(?:\.\d+)?)')
                wparts = word_pat.split(part)
                for wp in wparts:
                    if not wp:
                        continue
                    if wp in KEYWORDS:
                        built.append('<span class="c-kw">' + wp + '</span>')
                    elif wp in TYPE_HINTS:
                        built.append('<span class="c-type">' + wp + '</span>')
                    elif wp.isdigit():
                        built.append('<span class="c-num">' + wp + '</span>')
                    else:
                        # method-call names (followed by "(") get subtle color
                        built.append(wp)
                # post-pass: method calls — handled via regex on built text
        line_html = ''.join(built)
        # method-call highlight
        line_html = re.sub(
            r'([A-Za-z_][A-Za-z0-9_]*)\(',
            r'<span class="c-fn">\1</span>(',
            line_html)
        if comment_html:
            line_html = line_html.rstrip() + (' ' if line_html.strip() else '') + comment_html
        out.append(line_html)
    return '\n'.join(out)


def code(code_text: str, title: str = '', note: str = '', tall: bool = False) -> str:
    """Render a code block with optional title bar and note below.

    Blocks longer than ~34 lines automatically allow page-splitting
    (like printed technical books) to avoid large blank voids.
    """
    n_lines = len([l for l in code_text.strip('\n').split('\n')])
    if n_lines > 29:
        tall = True
    cls = 'code-block' + (' tall' if tall else '')
    title_html = ''
    if title:
        title_html = f'<div class="code-title"><span class="dot"></span>{html_mod.escape(title)}</div>'
    inner = _highlight_code(code_text.strip('\n'))
    note_html = f'<div class="code-note">{note}</div>' if note else ''
    return f'<div class="code-wrap">{title_html}<pre class="{cls}" dir="ltr"><code>{inner}</code></pre>{note_html}</div>'


def teacher(text: str, title: str = 'نکتهٔ معلم') -> str:
    return f'<div class="teacher-box"><div class="box-title">💡 {html_mod.escape(title)}</div><p>{text}</p></div>'


def warn(text: str, title: str = 'هشدار امنیتی') -> str:
    return f'<div class="warn-box"><div class="box-title">⚠️ {html_mod.escape(title)}</div><p>{text}</p></div>'


def info(text: str, title: str = 'می‌دانستید؟') -> str:
    return f'<div class="info-box"><div class="box-title">🔎 {html_mod.escape(title)}</div><p>{text}</p></div>'


def chapter(num_fa: str, marker: str, title: str, en: str = '') -> str:
    en_html = f'<span class="ch-en" dir="ltr">{html_mod.escape(en)}</span>' if en else ''
    return (
        f'<div class="chapter-header">'
        f'<div class="ch-marker" dir="ltr">{marker}</div>'
        f'<div class="ch-tag">فصل {num_fa}</div>'
        f'<h2 class="ch-title">{title}</h2>'
        f'{en_html}'
        f'</div>'
    )


def section(title: str) -> str:
    return f'<h3 class="sec-title"><span class="sec-bullet"></span>{title}</h3>'


def subsection(title: str) -> str:
    return f'<h4 class="sub-title">{title}</h4>'


def table(headers, rows, caption='', small=False):
    cls = 'tbl' + (' small' if small else '')
    cap = f'<div class="tbl-caption">{caption}</div>' if caption else ''
    h = ''.join(f'<th>{c}</th>' for c in headers)
    b = ''
    for r in rows:
        b += '<tr>' + ''.join(f'<td>{c}</td>' for c in r) + '</tr>'
    return f'{cap}<table class="{cls}"><thead><tr>{h}</tr></thead><tbody>{b}</tbody></table>'


def pill(text: str, kind: str = 'accent') -> str:
    return f'<span class="pill pill-{kind}">{text}</span>'


def para(text: str) -> str:
    return f'<p class="body-text">{text}</p>'


def bullets(items):
    lis = ''.join(f'<li>{i}</li>' for i in items)
    return f'<ul class="blist">{lis}</ul>'


def fa2en_digits(s: str) -> str:
    return s.translate(str.maketrans('۰۱۲۳۴۵۶۷۸۹', '0123456789'))


def en2fa_digits(s: str) -> str:
    return s.translate(str.maketrans('0123456789', '۰۱۲۳۴۵۶۷۸۹'))
