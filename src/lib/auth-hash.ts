/**
 * Phase 1 (v1.8.0-dev) — password hashing, extracted from src/lib/auth.ts.
 *
 * Pure crypto (scrypt + per-user random salt) with NO next/* imports so it is
 * importable from CLI scripts (installer database bootstrap) and from the API
 * layer alike. src/lib/auth.ts re-exports these — single implementation.
 */
import { randomBytes, scryptSync, timingSafeEqual } from 'crypto'

export function hashPassword(password: string): string {
  const salt = randomBytes(16).toString('hex')
  const hash = scryptSync(password, salt, 64).toString('hex')
  return `${salt}:${hash}`
}

export function verifyPassword(password: string, stored: string): boolean {
  const [salt, hash] = stored.split(':')
  if (!salt || !hash) return false
  const candidate = scryptSync(password, salt, 64)
  const original = Buffer.from(hash, 'hex')
  return candidate.length === original.length && timingSafeEqual(candidate, original)
}
