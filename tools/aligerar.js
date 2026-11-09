// Prepara los medios de la web para móvil. Se puede lanzar cuantas veces haga falta (al añadir medios nuevos):
//   cd tools && npm install && node aligerar.js
// - Imágenes que usa la web: mismos codificadores que Squoosh (MozJPEG; PNG con paleta). Mismo nombre y formato; una imagen ya tratada no se vuelve a comprimir.
// - Bucles de vídeo (<video data-src>): crea x_movil.mp4 a 640 px y 24 fps como máximo, y añade data-light al HTML. case.js la usa en móviles y tablets.
const fs = require('fs'), path = require('path'), crypto = require('crypto'), { execFileSync } = require('child_process'), sharp = require('sharp');
const root = path.join(__dirname, '..'), proto = path.join(root, 'prototype'), ledgerFile = path.join(__dirname, 'aligerado.json');
const FFMPEG = process.env.FFMPEG || 'ffmpeg';
const ledger = fs.existsSync(ledgerFile) ? JSON.parse(fs.readFileSync(ledgerFile, 'utf8')) : {};
const sha = b => crypto.createHash('sha1').update(b).digest('hex');
const pages = fs.readdirSync(proto).filter(f => /\.(html|css|js)$/.test(f));
const mb = n => (n / 1048576).toFixed(2) + ' MB';

async function images() {
  const set = new Set();
  for (const f of pages) for (const m of fs.readFileSync(path.join(proto, f), 'utf8').matchAll(/assets\/[^"'\)`<>]*?\.(png|jpe?g)/gi)) {
    const p = path.join(root, decodeURIComponent(m[0])); if (fs.existsSync(p)) set.add(p);
  }
  (function walk(d) { for (const e of fs.readdirSync(d, { withFileTypes: true })) { const p = path.join(d, e.name); e.isDirectory() ? walk(p) : /\.(png|jpe?g)$/i.test(p) && set.add(p); } })(path.join(root, 'assets/visuales'));
  let before = 0, after = 0, done = 0;
  for (const p of set) {
    const key = path.relative(root, p).split(path.sep).join('/'), src = fs.readFileSync(p);
    before += src.length;
    if (ledger[key] === sha(src)) { after += src.length; continue; }
    const img = sharp(src).resize({ width: 1920, height: 1920, fit: 'inside', withoutEnlargement: true });
    const buf = await (/png$/i.test(p) ? img.png({ palette: true, quality: 85, effort: 10, compressionLevel: 9 }) : img.jpeg({ quality: 75, mozjpeg: true })).toBuffer();
    const keep = buf.length < src.length * .92 ? buf : src;
    if (keep === buf) { fs.writeFileSync(p, buf); done++; }
    after += keep.length; ledger[key] = sha(keep);
  }
  console.log(`imágenes: ${set.size}, comprimidas ahora ${done}, total ${mb(before)} -> ${mb(after)}`);
}

function videos() {
  let made = 0, before = 0, after = 0;
  for (const f of pages.filter(x => x.endsWith('.html'))) {
    const file = path.join(proto, f);
    let html = fs.readFileSync(file, 'utf8');
    const next = html.replace(/data-src="([^"]+\.mp4)"(?! data-light)/g, (all, src) => {
      const input = path.resolve(proto, decodeURIComponent(src));
      if (!fs.existsSync(input)) return all;
      const out = input.replace(/\.mp4$/, '_movil.mp4'), lightSrc = src.replace(/\.mp4$/, '_movil.mp4');
      if (!fs.existsSync(out)) execFileSync(FFMPEG, ['-y', '-v', 'error', '-i', input, '-vf', "scale='min(640,iw)':-2,fps='min(24,source_fps)'", '-c:v', 'libx264', '-crf', '28', '-preset', 'slow', '-pix_fmt', 'yuv420p', '-movflags', '+faststart', '-an', out]);
      const a = fs.statSync(input).size, b = fs.statSync(out).size;
      // nothing to gain on clips that were already small
      if (b >= a * .85) { fs.unlinkSync(out); return all; }
      made++; before += a; after += b;
      return `${all} data-light="${lightSrc}"`;
    });
    if (next !== html) fs.writeFileSync(file, next);
  }
  console.log(`bucles: ${made} versiones móvil, ${mb(before)} -> ${mb(after)}`);
}

images().then(() => { videos(); fs.writeFileSync(ledgerFile, JSON.stringify(ledger, null, 0)); });
