// Shared behaviour for project pages: language, torn paper, lazy loops, compare slider
(() => {
  const store = {
    get: k => { try { return localStorage.getItem(k); } catch { return null; } },
    set: (k, v) => { try { localStorage.setItem(k, v); } catch {} }
  };

  // language: ?lang= wins, then the last choice, then Spanish
  const setLang = l => {
    document.documentElement.lang = l;
    document.querySelectorAll('.lang button').forEach(b => b.classList.toggle('on', b.dataset.lang === l));
    store.set('lang', l);
  };
  setLang(new URLSearchParams(location.search).get('lang') || store.get('lang') || 'es');
  document.querySelectorAll('.lang button').forEach(b => b.addEventListener('click', () => setLang(b.dataset.lang)));

  // torn edges on a backing layer, so tape and stickers can hang outside the paper
  const jag = (n, side) => Array.from({ length: n + 1 }, (_, i) => {
    const t = (i / n) * 100, d = (Math.random() * 5 + 1).toFixed(1);
    return side === 'top' ? `${t}% ${d}px` : side === 'right' ? `calc(100% - ${d}px) ${t}%`
         : side === 'bottom' ? `${100 - t}% calc(100% - ${d}px)` : `${d}px ${100 - t}%`;
  });
  document.querySelectorAll('.torn').forEach(el => {
    const layer = document.createElement('span');
    layer.className = 'tear';
    layer.style.clipPath = `polygon(${[...jag(36, 'top'), ...jag(20, 'right'), ...jag(36, 'bottom'), ...jag(20, 'left')].join(',')})`;
    el.prepend(layer);
  });
  // YouTube: the player only loads (and starts) when the thumbnail is clicked
  document.querySelectorAll('.yt').forEach(b => b.addEventListener('click', () => {
    b.innerHTML = `<iframe src="https://www.youtube-nocookie.com/embed/${b.dataset.yt}?autoplay=1&rel=0" title="${b.getAttribute('aria-label')}" allow="autoplay; encrypted-media; picture-in-picture; fullscreen" allowfullscreen></iframe>`;
  }, { once: true }));

  // small photos carry their caption on a post-it
  const markSmall = () => document.querySelectorAll('.fig').forEach(f =>
    f.classList.toggle('small', f.offsetWidth < 330 && !!f.querySelector(':scope > .cap')));
  markSmall();
  addEventListener('resize', markSmall);
  document.querySelectorAll('.torn-top').forEach(el => {
    const top = Array.from({ length: 61 }, (_, i) => `${(i / 60) * 100}% ${(Math.random() * 14).toFixed(1)}px`);
    el.style.clipPath = `polygon(${top.join(',')}, 100% 100%, 0 100%)`;
  });

  // ransom-note chapter titles: each letter gets a different cut-out, picked from the word so it stays put
  const CUTS = ['r-ink', 'r-paper', 'r-red', 'r-kraft', 'r-neon', 'r-news'];
  document.querySelectorAll('.chapter h2').forEach(el => {
    const text = el.textContent.trim();
    let h = [...text].reduce((a, c) => (a * 31 + c.charCodeAt(0)) % 9973, 7), prev = -1;
    el.setAttribute('aria-label', text);
    el.classList.add('ransomed');
    // letters are grouped per word so a word never breaks across lines
    el.innerHTML = text.split(/\s+/).map(word => '<span class="r-word" aria-hidden="true">' + [...word].map(ch => {
      h = (h * 7919 + 104729) % 9973;
      let c = h % CUTS.length; if (c === prev) c = (c + 1) % CUTS.length; prev = c;
      return `<span class="${CUTS[c]}" style="rotate:${((h >> 3) % 11 - 5) * .8}deg;translate:0 ${((h >> 5) % 5 - 2) * .02}em">${ch}</span>`;
    }).join('') + '</span>').join('');
  });

  // looping clips load and play only while on screen; phones and tablets get the light version and less margin
  const light = matchMedia('(hover: none), (pointer: coarse)').matches;
  const io = new IntersectionObserver(entries => entries.forEach(({ target: v, isIntersecting }) => {
    if (isIntersecting) {
      if (!v.getAttribute('src')) v.src = (light && v.dataset.light) || v.dataset.src;
      v.play().catch(() => {});
    } else v.pause();
  }), { rootMargin: light ? '40px 0px' : '200px 0px' });
  document.querySelectorAll('video[data-src]').forEach(v => io.observe(v));

  // before/after slider: drag anywhere on the image; step buttons swap the "before" layer
  document.querySelectorAll('.compare').forEach(c => {
    const view = c.querySelector('.view');
    const move = x => {
      const r = view.getBoundingClientRect();
      c.style.setProperty('--pos', `${Math.max(0, Math.min(100, ((x - r.left) / r.width) * 100))}%`);
    };
    view.addEventListener('pointerdown', e => { view.setPointerCapture(e.pointerId); move(e.clientX); });
    view.addEventListener('pointermove', e => { if (view.hasPointerCapture(e.pointerId)) move(e.clientX); });
    c.querySelectorAll('.steps button').forEach(b => b.addEventListener('click', () => {
      c.querySelector('.before').src = b.dataset.img;
      c.querySelectorAll('.steps button').forEach(x => x.classList.toggle('on', x === b));
    }));
  });
})();
