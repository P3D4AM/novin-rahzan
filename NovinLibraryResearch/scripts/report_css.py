# -*- coding: utf-8 -*-
"""CSS for the Persian RTL technical report (creative-flow pipeline).

Note: @font-face declarations live in the external file fonts/fonts.css
(portable alongside the HTML). The main stylesheet only uses the family.
"""

CSS = r'''
@page { size: 720px 1020px; margin: 0; }

:root {
  --page-bg: #f6f7f7;
  --section-bg: #eceeee;
  --card-bg: #e9eced;
  --table-stripe: #eeeff0;
  --header-fill: #3f5e6e;
  --cover-block: #56717e;
  --border: #b3c7d1;
  --icon: #4488aa;
  --accent: #246a8d;
  --text: #131515;
  --muted: #858b8f;
  --ok: #459860;
  --warn: #927640;
  --err: #ae554d;
  --code-bg: #1e2a32;
  --code-text: #dde7ec;
}

html, body {
  margin: 0;
  padding: 0;
  width: 720px;
  background: var(--page-bg);
  color: var(--text);
  font-family: 'Vazirmatn', 'DejaVu Sans', sans-serif;
  font-size: 13px;
  line-height: 1.82;
  direction: rtl;
}

@media screen {
  html { display: flex; justify-content: center; background: #3a4a53; }
  body { margin: 16px auto; box-shadow: 0 2px 24px rgba(0,0,0,.35); }
}

* { box-sizing: border-box; }

/* --------------------------- Cover --------------------------- */
.cover {
  width: 720px;
  height: 1020px;
  break-after: page;
  overflow: hidden;
  position: relative;
  background: linear-gradient(165deg, #2b4350 0%, #3f5e6e 58%, #4a6d7e 100%);
  color: #f2f6f8;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 64px 58px;
}
.cover .deco-circle {
  position: absolute; border-radius: 50%;
  border: 1px solid rgba(255,255,255,0.14);
}
.cover .dc1 { width: 330px; height: 330px; top: 40px; left: 40px; }
.cover .dc2 { width: 330px; height: 330px; top: 58px; left: 58px; border-color: rgba(255,255,255,0.08); }
.cover .dc3 { width: 190px; height: 190px; bottom: 130px; right: 52px; background: rgba(255,255,255,0.05); border: none; }
.cover .deco-grid {
  position: absolute; top: 0; left: 0; width: 100%; height: 8px;
  background: linear-gradient(90deg, #246a8d, #6fa3bf);
}
.cover-tag {
  display: inline-block; align-self: flex-start;
  border: 1px solid rgba(255,255,255,0.4);
  border-radius: 999px; padding: 3px 18px;
  font-size: 12.5px; letter-spacing: 1px; margin-bottom: 30px;
  color: #dcebf2;
}
.cover-title {
  font-size: 44px; font-weight: 700; line-height: 1.5; margin: 0 0 8px 0;
}
.cover-title .hl { color: #a8d4ea; }
.cover-sub {
  font-size: 17px; font-weight: 500; color: #cfdfe8; margin-bottom: 6px; line-height: 2;
}
.cover-en {
  font-size: 11.5px; color: #9fbecf; letter-spacing: 1.5px; margin-bottom: 40px;
}
.cover-en::after {
  content: '';
  display: block;
  width: 130px; height: 3px;
  background: #a8d4ea; border-radius: 2px;
  margin-top: 16px;
}
.cover-summary {
  font-size: 13.5px; line-height: 2.1; color: #dce9f0;
  background: rgba(255,255,255,0.07);
  border: 1px solid rgba(255,255,255,0.14);
  border-radius: 12px; padding: 18px 22px; margin-bottom: 36px; max-width: 540px;
}
.cover-chips { display: flex; flex-wrap: wrap; gap: 9px; max-width: 560px; margin-bottom: 44px; }
.chip {
  font-size: 11.5px; padding: 4px 14px; border-radius: 999px;
  background: rgba(255,255,255,0.10); border: 1px solid rgba(255,255,255,0.22); color: #e6f0f5;
}
.cover-footer {
  position: absolute; bottom: 46px; right: 58px; left: 58px;
  display: flex; justify-content: space-between; align-items: flex-end;
  font-size: 12px; color: #b7cdd9;
}
.cover-footer .strong { color: #e9f2f7; font-weight: 600; font-size: 13px; }
.cover-footer .left { text-align: left; direction: ltr; line-height: 1.8; }

/* --------------------------- Main content --------------------------- */
.main-content {
  padding: 44px 52px 40px 52px;
}

p.body-text {
  margin: 0 0 10px 0;
  text-align: justify;
  line-break: strict;
}

.blist { margin: 4px 0 12px 0; padding-right: 20px; }
.blist li { margin-bottom: 5px; text-align: justify; break-inside: avoid; }
.blist li::marker { color: var(--accent); }

/* chapter header */
.chapter-header {
  break-after: avoid; break-inside: avoid;
  margin-top: 22px; margin-bottom: 12px;
  position: relative;
}
.chapter-header:first-child { margin-top: 0; }
.ch-marker {
  position: absolute; left: 0; top: -6px;
  font-family: 'DejaVu Sans Mono', monospace;
  font-size: 40px; font-weight: 700; color: rgba(63,94,110,0.16);
  letter-spacing: 1px;
}
.ch-tag {
  display: inline-block; background: var(--header-fill); color: #fff;
  font-size: 11.5px; font-weight: 600; border-radius: 999px;
  padding: 2px 16px; margin-bottom: 8px;
}
h2.ch-title { font-size: 23px; font-weight: 700; margin: 0 0 4px 0; color: #22333d; line-height: 1.55; padding-left: 92px; }
.ch-en {
  display: block; font-size: 11.5px; color: var(--muted);
  font-family: 'DejaVu Sans Mono', monospace, sans-serif; letter-spacing: 0.5px; margin-bottom: 12px;
  padding-left: 92px;
}
.chapter-header::after {
  content: '';
  display: block;
  height: 3px; width: 64px;
  background: var(--accent);
  border-radius: 2px;
}

h3.sec-title {
  font-size: 16px; font-weight: 700; color: #24404e;
  margin: 18px 0 7px 0; break-after: avoid;
  display: flex; align-items: center; gap: 9px;
}
.sec-bullet { width: 9px; height: 9px; border-radius: 2px; background: var(--icon); display: inline-block; flex-shrink: 0; }

h4.sub-title {
  font-size: 14px; font-weight: 600; color: #35525f;
  margin: 14px 0 6px 0; break-after: avoid;
}

/* teacher / warn / info boxes */
.teacher-box, .warn-box, .info-box {
  background: var(--card-bg);
  border-radius: 10px;
  padding: 10px 16px 9px 16px;
  margin: 10px 0 12px 0;
  break-inside: avoid;
}
.teacher-box { border-right: 4px solid var(--accent); }
.warn-box { border-right: 4px solid var(--err); background: #f0e9e8; }
.info-box { border-right: 4px solid var(--icon); }
.box-title { font-weight: 700; font-size: 12.6px; margin-bottom: 3px; }
.teacher-box .box-title { color: var(--accent); }
.warn-box .box-title { color: var(--err); }
.info-box .box-title { color: var(--icon); }
.teacher-box p, .warn-box p, .info-box p { margin: 0; text-align: justify; line-height: 1.8; font-size: 12.6px; }

/* code blocks */
.code-wrap { margin: 10px 0 12px 0; break-inside: avoid; }
.code-wrap:has(pre.tall) { break-inside: auto; }
.code-title {
  display: flex; align-items: center; gap: 8px;
  background: #2b3d48; color: #cfe2ec;
  font-family: 'DejaVu Sans Mono', monospace; font-size: 10px;
  padding: 5px 14px; border-radius: 8px 8px 0 0;
  direction: ltr; text-align: left;
}
.code-title .dot { width: 8px; height: 8px; border-radius: 50%; background: #6fa3bf; flex-shrink: 0; }
pre.code-block {
  direction: ltr; text-align: left;
  background: var(--code-bg); color: var(--code-text);
  font-family: 'DejaVu Sans Mono', monospace;
  font-size: 10px; line-height: 1.5;
  margin: 0; padding: 11px 14px;
  border-radius: 0 0 8px 8px;
  white-space: pre-wrap;
  overflow-wrap: break-word;
  max-width: 100%;
}
.code-wrap:not(:has(.code-title)) pre.code-block { border-radius: 8px; }
pre.tall { break-inside: auto; }
.c-kw { color: #6cb2e8; }
.c-str { color: #a8c988; }
.c-com { color: #7d8f9a; font-style: italic; }
.c-num { color: #d8b072; }
.c-type { color: #8fd0d0; }
.c-fn { color: #e8dcae; }
.code-note { font-size: 11.5px; color: var(--muted); margin-top: 5px; line-height: 1.7; }

/* tables */
.tbl-caption { font-size: 11.5px; color: var(--muted); margin: 4px 0 5px 0; }
table.tbl {
  width: 100%; border-collapse: collapse; font-size: 11.5px; line-height: 1.65;
  margin: 6px 0 14px 0;
}
table.tbl thead { display: table-header-group; }
table.tbl th {
  background: var(--header-fill); color: #fff; font-weight: 600;
  padding: 6px 9px; text-align: right; font-size: 11.4px;
  border: none;
}
table.tbl td {
  padding: 5px 9px; border-bottom: 1px solid var(--border);
  vertical-align: top; text-align: justify;
  overflow-wrap: break-word; word-break: break-word;
}
table.tbl tr { break-inside: avoid; }
table.tbl tbody tr:nth-child(even) { background: var(--table-stripe); }
table.tbl td code, .inline-code {
  font-family: 'DejaVu Sans Mono', monospace; font-size: 10px;
  direction: ltr; unicode-bidi: embed;
  background: rgba(63,94,110,0.10); border-radius: 4px; padding: 1px 5px;
  color: #27526b; white-space: nowrap;
}
table.tbl.small { font-size: 10.6px; }
table.tbl.small td { padding: 4px 7px; }
table.tbl.small th { font-size: 10.6px; padding: 5px 7px; }

/* pills */
.pill {
  display: inline-block; font-size: 10.5px; font-weight: 600;
  border-radius: 999px; padding: 1px 12px; margin: 0 2px;
}
.pill-accent { background: rgba(36,106,141,0.12); color: #246a8d; }
.pill-ok { background: rgba(69,152,96,0.13); color: #3a7f50; }
.pill-err { background: rgba(174,85,77,0.13); color: #a04a42; }
.pill-warn { background: rgba(146,118,64,0.14); color: #7a6234; }
.pill-muted { background: rgba(19,21,21,0.07); color: #5c6266; }

/* roadmap grid */
.roadmap { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; margin: 14px 0 6px 0; }
.rm-card {
  background: #fff; border: 1px solid var(--border); border-radius: 10px;
  padding: 10px 14px; break-inside: avoid; position: relative;
}
.rm-num {
  font-family: 'DejaVu Sans Mono', monospace; font-size: 20px; font-weight: 700;
  color: rgba(63,94,110,0.25); position: absolute; left: 12px; top: 8px;
}
.rm-title { font-weight: 700; font-size: 12.8px; color: #24404e; margin-bottom: 2px; padding-left: 42px; }
.rm-desc { font-size: 11.5px; color: var(--muted); line-height: 1.7; padding-left: 42px; }
.rm-page { position: absolute; bottom: 8px; left: 14px; font-size: 11px; color: var(--accent); font-weight: 600; direction: ltr; }

/* stat strip */
.stat-strip { display: flex; flex-wrap: wrap; gap: 9px; margin: 12px 0; }
.stat {
  flex: 1 1 130px; min-width: 115px; background: #fff;
  border: 1px solid var(--border); border-radius: 10px;
  padding: 10px 8px; text-align: center; break-inside: avoid;
}
.stat .v { font-size: 20px; font-weight: 700; color: var(--accent); line-height: 1.4; }
.stat .l { font-size: 10.8px; color: var(--muted); }

/* architecture diagram (pure CSS) */
.arch { margin: 14px 0 6px 0; }
.arch-layer {
  border-radius: 10px; padding: 8px 14px; margin: 0;
  break-inside: avoid; position: relative;
}
.arch-layer .al-title { font-weight: 700; font-size: 12.6px; margin-bottom: 3px; }
.arch-layer .al-desc { font-size: 11.2px; line-height: 1.7; }
.arch-row { display: flex; flex-wrap: wrap; gap: 6px; margin-top: 6px; }
.arch-box {
  background: rgba(255,255,255,0.75); border: 1px solid var(--border);
  border-radius: 7px; padding: 3px 12px; font-size: 11px;
  font-weight: 600; color: #2c4a59;
}
.arch-arrow {
  text-align: center; color: var(--icon); font-size: 15px; line-height: 1.2;
  margin: 2px 0; font-weight: 700;
}
.al-1 { background: #dfe9ee; border: 1px solid #b9cedb; }
.al-2 { background: #e9eef1; border: 1px solid #c2d4de; }
.al-3 { background: #dfe9e2; border: 1px solid #b7cfc0; }
.al-4 { background: #ece9e1; border: 1px solid #d1c9b6; }

/* flow steps */
.flow { display: flex; flex-wrap: wrap; gap: 8px; margin: 12px 0; max-width: 100%; }
.flow-step {
  flex: 1 1 110px; min-width: 96px; max-width: 100%;
  background: #fff; border: 1px solid var(--border); border-radius: 9px;
  padding: 8px 6px; text-align: center; break-inside: avoid;
  font-size: 11.3px; line-height: 1.65;
}
.flow-step .fs-num {
  display: inline-block; width: 20px; height: 20px; border-radius: 50%;
  background: var(--header-fill); color: #fff; font-size: 11px; font-weight: 700;
  line-height: 20px; margin-bottom: 4px;
}
.flow-step b { display: block; font-size: 11.6px; color: #24404e; }

/* ending page */
.ending {
  width: 720px; height: 1020px;
  break-before: page; overflow: hidden;
  background: linear-gradient(200deg, #2b4350 0%, #3f5e6e 70%);
  color: #eef4f7;
  display: flex; flex-direction: column; justify-content: center; align-items: center;
  text-align: center; padding: 70px 60px; position: relative;
}
.ending .deco-circle { position: absolute; border-radius: 50%; border: 1px solid rgba(255,255,255,0.13); }
.ending .dc1 { width: 300px; height: 300px; top: 60px; right: 60px; }
.ending .dc2 { width: 220px; height: 220px; bottom: 90px; left: 70px; background: rgba(255,255,255,0.045); border: none; }
.ending-big { font-size: 30px; font-weight: 700; line-height: 1.9; margin-bottom: 18px; }
.ending-sub { font-size: 14px; color: #c8dae4; line-height: 2.2; max-width: 480px; margin-bottom: 40px; }
.ending-stats { display: flex; gap: 14px; margin-bottom: 46px; }
.ending-stat {
  border: 1px solid rgba(255,255,255,0.25); border-radius: 12px;
  padding: 14px 22px; background: rgba(255,255,255,0.06);
}
.ending-stat .v { font-size: 26px; font-weight: 700; color: #a8d4ea; }
.ending-stat .l { font-size: 11.5px; color: #c3d6e0; }
.ending-foot { font-size: 12px; color: #9fbecf; }

/* TOC page anchor spacing */
.page-break-note { font-size: 11.5px; color: var(--muted); }

/* misc */
img, svg { max-width: 100%; height: auto; }
strong { font-weight: 700; color: #1d2c34; }
a { color: var(--accent); text-decoration: none; }
.footnote { font-size: 11.3px; color: var(--muted); line-height: 1.8; }
'''

HTML_SHELL = '''<!DOCTYPE html>
<html lang="fa" dir="rtl">
<head>
<meta charset="UTF-8">
<title>{title}</title>
<link rel="stylesheet" href="fonts/fonts.css">
<style>{css}</style>
</head>
<body>
{body}
</body>
</html>
'''

FONTS_CSS = '''/* Vazirmatn (SIL OFL 1.1) — bundled alongside this document */
@font-face {
  font-family: 'Vazirmatn';
  src: url('Vazirmatn-Regular.ttf') format('truetype');
  font-weight: 400; font-display: swap;
}
@font-face {
  font-family: 'Vazirmatn';
  src: url('Vazirmatn-Medium.ttf') format('truetype');
  font-weight: 500; font-display: swap;
}
@font-face {
  font-family: 'Vazirmatn';
  src: url('Vazirmatn-SemiBold.ttf') format('truetype');
  font-weight: 600; font-display: swap;
}
@font-face {
  font-family: 'Vazirmatn';
  src: url('Vazirmatn-Bold.ttf') format('truetype');
  font-weight: 700; font-display: swap;
}
'''
