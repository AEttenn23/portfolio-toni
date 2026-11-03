# MonsterFarm (Leafblade): caso de estudio

> Textos para la web, en español e inglés. Tono profesional y neutro.
> Los datos salen del repositorio (git log / git blame) y de la carpeta de producción `D:/leafblade`.
> Las rutas de los medios son relativas a `assets/monsterfarm/`.

---

# ES

## Cabecera

**Leafblade**
Monster tamer 2D con granja · Unity 6 (URP 2D) · Wayward Games · Mayo 2025 – Octubre 2026 · [Steam](https://store.steampowered.com/app/2887740/Leafblade/)

**Rol:** Animador 2D · Artista de VFX · Technical artist · Programación de bosses · QA · Trailer y marketing

**Etiquetas:** `Animación 2D` `VFX` `Shaders` `Código`

**En cifras**
- Más de 25 monstruos animados, con sus líneas evolutivas
- 5 bosses animados y 2 programados por completo (Titan Golunk y Last Bearer)
- 238 animaciones, 123 rigs/PSB, 21 efectos con VFX Graph y 7 shaders creados por mí en el repositorio
- 171 commits a lo largo de 17 meses

Medio de cabecera: `video/mf_trailer_1080.mp4` (póster: `video/poster_mf_trailer_1080.jpg`)

## Resumen

Leafblade es un monster tamer con granja: el jugador explora, combate, captura monstruos y los pone a trabajar en su granja. Ya ha publicado varias demos, una de ellas para el Steam Next Fest, y su lanzamiento está previsto para pronto.

Mi trabajo se centró en dar vida a los monstruos: los rigs 2D, sus animaciones de idle, movimiento, ataque, habilidad y captura, y los efectos visuales de sus ataques. También diseñé y programé dos de los bosses, hice shaders y efectos para el escenario y, en la recta final, me encargué de buena parte del debugging y el pulido. Además, monté el trailer y el material para redes.

## 1. Animación de monstruos: técnica híbrida

Medios: `gifs/*attack.mp4`, `gifs/*ability.mp4`, `gifs/enokita_alk.mp4`, `gifs/amanita_walk.mp4` · rigs `rigs/*_pieces.png`, `rigs/*_layers/` · frame a frame `frames/*_strip.jpg`, `frames/*_loop.mp4`

Animé más de 25 monstruos con sus evoluciones, a partir de los diseños de Rocío Gutiérrez Cánovas. Cada uno tiene idle, movimiento, ataque, habilidad, animación de captura y transiciones.

- **Rig esqueletal 2D:** separé cada personaje en piezas en archivos PSB (cuerpo, extremidades, ojos y elementos sueltos) y lo rigué con el 2D Animation de Unity (SpriteSkin y Sprite Library).
- **Frames dibujados sobre el rig:** las partes orgánicas (humo, agua, fuego, hojas) se redibujan frame a frame y se cambian sobre el esqueleto con Sprite Resolver. Así el movimiento conserva la fluidez del esqueletal y el carácter de la animación tradicional (Wispin, Nightgloom, Titan Skull, Frostling, Icestrike).
- **Detalles de lectura en combate:** ojos rojos en los enemigos, brillo nocturno en algunas líneas evolutivas, luces 2D en los monstruos que brillan y fundidos de ojos en el teletransporte.

## 2. Bosses

Medios: `video/mf_bossfight_titan_amanita_1080.mp4` · `video/clip_titan_golunk_idle.mp4` · `frames/titan_skull_*` · `gifs/last_bearer.mp4` · `vfx-process/attack_left_4_.png`, `hoe_.png`, `swamp_final_boss_v4.png` · código `code/TitanGolunkBossBehaviour.cs`, `code/LastBearerBossBehaviour.cs`

Animé cinco bosses: Titan Amanita, Titan Golunk, Titan Skull, el boss final del pantano y Last Bearer. Dos de ellos los programé por completo, sobre la clase base de bosses del proyecto:

- **Titan Golunk** (502 líneas): elige entre cuatro ataques sin repetir el anterior y ajusta la distancia de ataque a cada uno. Al bajar del 50 % de vida lanza una fase especial, se vuelve invulnerable y pasa a poder capturarse, con su tutorial. Incluye la intro, el cambio de cámara con Cinemachine, el screenshake, el diálogo final y la secuencia de vínculo.
- **Last Bearer** (266 líneas): combate cuerpo a cuerpo con un combo de cuatro ataques encadenados, embestidas, invocación de enemigos y ondas de choque con VFX.

## 3. VFX y shaders

Medios: `gifs/water_explosion.mp4`, `gifs/water_shader.mp4`, `gifs/water_heiser.mp4`, `gifs/fost_glow.mp4`, `gifs/estatuav7.mp4`, `gifs/tornado.gif` · proceso `vfx-process/geiser_concept_art.png`, `tornado_v3.png`, `explosion_2_vimpact.png`, `air_slash_v2.png`, `pluma.png` · `frames/tornado_*`

Los efectos combinan frames dibujados a mano con partículas de VFX Graph y shaders hechos con Shader Graph:

- **21 efectos con VFX Graph:** explosiones de fuego y agua, curación, géiser, plumas y estela del Air Slash, partículas de hielo y magia de las ruinas.
- **7 shaders:** contorno de fuego (normal y suavizado), contorno SDF, partículas de fuego y estanque mágico (con y sin máscara).
- **Proceso:** cada efecto parte de un boceto o una referencia (en el géiser: foto → diseño → versión en el juego), se anima frame a frame y se combina con partículas en Unity.
- **Escenario:** aura y contorno de las ruinas y estatuas, cambios de agua en Meadow y efectos de los bloqueos del mapa.

## 4. Debugging, QA y pulido

Medios: `video/qa_bug_report_720.mp4`

En la última fase del proyecto me dediqué en buena parte a encontrar y corregir errores en la animación, los prefabs y el gameplay:

- Comportamiento de las mascotas: que siempre encuentren enemigos, que trabajen al comer, sombras al cargarlas, boomerang de Swirlbloom y Vinetoss.
- UI y sistemas: la pantalla en negro al dormir (una suscripción a un evento que no se liberaba), la UI de cocina, iconos que no desaparecían y un error de build en los tutoriales.
- Transiciones de animación, pivotes, posiciones y colisiones de monstruos y del escenario.
- Vídeos de reporte con anotaciones para comunicar los bugs al equipo.
- Traducción al español de los textos del juego: objetos, bestiario, crafteo, árbol de habilidades y UI.

## 5. Trailer y marketing

Medios: `video/mf_trailer_1080.mp4`, `video/mf_trailer_vertical_720.mp4` · shorts `video/short_*.mp4` · carruseles `marketing/carrousel_*/` · portadas `marketing/portada_*.jpg`

- **Trailer:** monté el trailer del juego en versión horizontal y vertical, con varias iteraciones a lo largo del desarrollo.
- **Shorts:** la serie "Today's Monster", que presenta un monstruo por vídeo, y la pieza del Steam Next Fest. Cada uno resume en unos 20 segundos cómo se captura un monstruo, cómo combate y cómo trabaja en la granja, y cierra con la llamada a la wishlist.
- **Carruseles para Instagram y TikTok:** casas de monstruos por niveles, cultivos, comidas, decoraciones, la cooperativa de la granja y Springleaf, con los textos en inglés y español.

## Herramientas
Unity 6 (URP 2D) · 2D Animation (SpriteSkin, Sprite Library, Sprite Resolver) · VFX Graph · Shader Graph · Cinemachine · C# · Photoshop · Git / GitLab

## Equipo
- David Fernández: game design y programación principal
- Rocío Gutiérrez Cánovas: diseño de los monstruos

---

# EN

## Header

**Leafblade**
2D monster tamer with farming · Unity 6 (URP 2D) · Wayward Games · May 2025 – October 2026 · [Steam](https://store.steampowered.com/app/2887740/Leafblade/)

**Role:** 2D animator · VFX artist · Technical artist · Boss programming · QA · Trailer and marketing

**Tags:** `2D Animation` `VFX` `Shaders` `Code`

**At a glance**
- 25+ monsters animated, including their evolution lines
- 5 bosses animated, 2 of them fully programmed by me (Titan Golunk and Last Bearer)
- 238 animations, 123 rigs/PSBs, 21 VFX Graph effects and 7 shaders created by me in the repository
- 171 commits over 17 months

## Overview

Leafblade is a monster tamer with farming: the player explores, fights, tames monsters and puts them to work on their farm. It has released several demos, including one for Steam Next Fest, and is due to launch soon.

My work focused on bringing the monsters to life: their 2D rigs, their idle, movement, attack, ability and taming animations, and the visual effects of their attacks. I also designed and programmed two of the bosses, made shaders and effects for the environment and, in the final stretch, handled much of the debugging and polish. I also edited the trailer and the social media content.

## 1. Monster animation: a hybrid technique

I animated more than 25 monsters and their evolutions, based on designs by Rocío Gutiérrez Cánovas. Each has idle, movement, attack, ability and taming animations plus transitions.

- **2D skeletal rigs:** I split each character into pieces in PSB files (body, limbs, eyes and loose elements) and rigged them with Unity's 2D Animation package (SpriteSkin and Sprite Library).
- **Hand-drawn frames on top of the rig:** organic parts (smoke, water, fire, leaves) are redrawn frame by frame and swapped on the skeleton with Sprite Resolver. Movement keeps the smoothness of skeletal animation and the character of traditional animation (Wispin, Nightgloom, Titan Skull, Frostling, Icestrike).
- **Readability in combat:** red eyes on enemies, night glow on some evolution lines, 2D lights on glowing monsters and eye fades when teleporting.

## 2. Bosses

I animated five bosses: Titan Amanita, Titan Golunk, Titan Skull, the swamp final boss and Last Bearer. I fully programmed two of them on top of the project's boss base class:

- **Titan Golunk** (502 lines): picks from four attacks without repeating the last one and adjusts its attack range to each. Below 50% health it triggers a special phase, becomes invulnerable and can be tamed, with its own tutorial. It includes the intro, a Cinemachine camera switch, screenshake, the closing dialogue and the bonding sequence.
- **Last Bearer** (266 lines): a melee fight with a four-hit chained combo, lunges, enemy summoning and VFX shockwaves.

## 3. VFX and shaders

Effects combine hand-drawn frames with VFX Graph particles and Shader Graph shaders:

- **21 VFX Graph effects:** fire and water explosions, healing, a geyser, Air Slash feathers and trails, ice particles and the ruins' magic.
- **7 shaders:** fire outline (regular and smooth), SDF outline, fire particles and a magic pool (masked and unmasked).
- **Process:** each effect starts from a sketch or a reference (for the geyser: photo → design → in-game version), is animated frame by frame and then combined with particles in Unity.
- **Environment:** aura and outline for the ruins and statues, water changes in Meadow and effects for map blockers.

## 4. Debugging, QA and polish

In the last phase of the project I spent much of my time finding and fixing bugs in animation, prefabs and gameplay:

- Pet behaviour: always finding enemies, working while eating, shadows while carried, the Swirlbloom and Vinetoss boomerang.
- UI and systems: a black screen when sleeping (an event subscription that was never released), the cooking UI, icons that never disappeared and a tutorial build error.
- Animation transitions, pivots, positions and collisions for monsters and the environment.
- Annotated bug report videos to communicate issues to the team.
- Spanish translation of the game's text: items, bestiary, crafting, ability tree and UI.

## 5. Trailer and marketing

- **Trailer:** I edited the game's trailer in landscape and vertical versions, iterating on it throughout development.
- **Shorts:** the "Today's Monster" series, which introduces one monster per video, and the Steam Next Fest piece. In about 20 seconds each one shows how a monster is tamed, how it fights and how it works on the farm, and ends with a wishlist call to action.
- **Instagram and TikTok carousels:** monster houses by level, crops, meals, decorations, the farm co-op and Springleaf, with copy in English and Spanish.

## Tools
Unity 6 (URP 2D) · 2D Animation (SpriteSkin, Sprite Library, Sprite Resolver) · VFX Graph · Shader Graph · Cinemachine · C# · Photoshop · Git / GitLab

## Team
- David Fernández: game design and lead programming
- Rocío Gutiérrez Cánovas: monster design
