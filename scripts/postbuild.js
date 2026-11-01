#!/usr/bin/env node
/**
 * HozorPlus — cross-platform post-build step.
 * Assembles the Next.js standalone bundle:
 *   - copies .next/static  -> .next/standalone/.next/static
 *   - copies public        -> .next/standalone/public
 *   - copies the Prisma client + native query engines so the standalone
 *     server can talk to SQLite on any machine (Windows included)
 * Equivalent of the previous `cp -r` shell step, but runs everywhere.
 */
const fs = require('fs')
const path = require('path')

const root = path.resolve(__dirname, '..')
const standalone = path.join(root, '.next', 'standalone')

function copyDir(src, dest) {
  if (!fs.existsSync(src)) {
    console.log(`[postbuild] skip (not found): ${path.relative(root, src)}`)
    return
  }
  fs.cpSync(src, dest, { recursive: true })
  console.log(`[postbuild] copied ${path.relative(root, src)} -> ${path.relative(root, dest)}`)
}

if (!fs.existsSync(standalone)) {
  console.error('[postbuild] .next/standalone not found — did `next build` run?')
  process.exit(1)
}

copyDir(path.join(root, '.next', 'static'), path.join(standalone, '.next', 'static'))
copyDir(path.join(root, 'public'), path.join(standalone, 'public'))

// Prisma: the generated client (.prisma/client) and the native engines
// (@prisma/engines, @prisma/client) are resolved at runtime, which Next's
// output tracing does not always follow — copy them explicitly.
copyDir(path.join(root, 'node_modules', '@prisma'), path.join(standalone, 'node_modules', '@prisma'))
copyDir(path.join(root, 'node_modules', '.prisma'), path.join(standalone, 'node_modules', '.prisma'))
copyDir(path.join(root, 'prisma'), path.join(standalone, 'prisma'))

console.log('[postbuild] standalone bundle ready at .next/standalone')
