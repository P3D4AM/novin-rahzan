#!/usr/bin/env node
/**
 * HozorPlus realtime service — plain-JS twin of index.ts.
 * Runs with plain `node` so the Windows launcher needs no extra tooling.
 *
 *   :3003  socket.io (browser realtime feed, path "/")
 *   :3004  internal HTTP emit endpoint, bound to 127.0.0.1 only —
 *          Next.js API routes POST { event, payload } here to broadcast.
 */
const { createServer } = require('http')
const { Server } = require('socket.io')

const PORT = Number(process.env.RT_PORT || 3003)
const EMIT_PORT = Number(process.env.RT_EMIT_PORT || 3004)

const io = new Server(createServer(), {
  path: '/',
  cors: { origin: '*', methods: ['GET', 'POST'] },
  pingTimeout: 60000,
  pingInterval: 25000,
})

const emitServer = createServer((req, res) => {
  if (req.method === 'POST' && req.url === '/emit') {
    let body = ''
    req.on('data', (chunk) => (body += chunk))
    req.on('end', () => {
      try {
        const { event, payload } = JSON.parse(body)
        io.emit('rt', { event, payload, ts: Date.now() })
        res.writeHead(200, { 'Content-Type': 'application/json' })
        res.end(JSON.stringify({ ok: true }))
      } catch {
        res.writeHead(400)
        res.end('bad request')
      }
    })
    return
  }
  if (req.method === 'GET' && req.url === '/health') {
    res.writeHead(200, { 'Content-Type': 'application/json' })
    res.end(JSON.stringify({ ok: true, clients: io.engine.clientsCount }))
    return
  }
  res.writeHead(404)
  res.end('not found')
})

emitServer.listen(EMIT_PORT, '127.0.0.1', () => {
  console.log(`[realtime] emit endpoint on 127.0.0.1:${EMIT_PORT}`)
})

io.listen(PORT)
console.log(`[realtime] socket.io listening on :${PORT}`)
