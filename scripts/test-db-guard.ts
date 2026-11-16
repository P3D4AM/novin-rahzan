/**
 * v1.3.1 (Phase 1 review fix 5) — test database safety guard.
 *
 * The old test suite deleted whatever file DATABASE_URL pointed at as long as
 * its name was not "custom.db" — a production database passed by accident
 * through DATABASE_URL would have been destroyed. This guard replaces that:
 *
 *   * tests generate their OWN unique temporary database path
 *     (generateTestDatabasePath) — never trusting an external DATABASE_URL,
 *   * every cleanup/creation goes through assertTestOwnedDatabasePath, which
 *     REFUSES (throws, without touching anything) any path outside the
 *     dedicated test directory,
 *   * migration sub-processes receive only validated, test-owned URLs.
 */
import path from 'path'

export class UnsafeDatabasePathError extends Error {
  constructor(offending: string, root: string) {
    super(
      `رفض مسیر پایگاه‌دادهٔ غیرآزمایشی: «${offending}» خارج از پوشهٔ مخصوص تست‌ها «${root}» است — ` +
      'اجرای تست متوقف شد و هیچ فایلی تغییر نکرد.',
    )
    this.name = 'UnsafeDatabasePathError'
  }
}

/** Extract the filesystem path from a Prisma SQLite URL ("file:…") or a plain path. */
export function resolveSqlitePath(urlOrPath: string): string {
  let p = String(urlOrPath ?? '').trim()
  if (p.startsWith('file:')) p = p.slice('file:'.length)
  // a "file:./db/x.db" relative URL resolves against the process cwd, like Prisma does
  return path.resolve(p)
}

/**
 * Assert that a database URL/path lives INSIDE the dedicated test directory.
 * Returns the resolved path. Throws UnsafeDatabasePathError otherwise — the
 * caller must NOT have written or deleted anything before this call.
 */
export function assertTestOwnedDatabasePath(urlOrPath: string, tmpRoot: string): string {
  const resolved = resolveSqlitePath(urlOrPath)
  const root = path.resolve(tmpRoot)
  const rel = path.relative(root, resolved)
  const owned = rel !== '' && !rel.startsWith('..') && !path.isAbsolute(rel)
  if (!owned) throw new UnsafeDatabasePathError(resolved, root)
  return resolved
}

/**
 * A unique, test-owned temporary database path — generated fresh per run so
 * two suites can never collide and nothing pre-existing can ever be deleted.
 */
export function generateTestDatabasePath(tmpRoot: string): string {
  return path.join(path.resolve(tmpRoot), `rawtest-${process.pid}-${Date.now()}-${Math.floor(Math.random() * 1e6)}.db`)
}

/** "file:" URL for a path (what Prisma expects in DATABASE_URL). */
export function sqliteUrl(filePath: string): string {
  return 'file:' + filePath
}
