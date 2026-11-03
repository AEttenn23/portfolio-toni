# MonsterFarm (Leafblade): índice de medios

Fuentes: repo `D:/primero animacion y modelaje/leafblade/monsterfarm` (GitLab Sedart/monsterfarm) y carpeta de producción `D:/leafblade`.
El juego se publica como **Leafblade**. Los alias de Toni en git son `andipl` y `Toni`.

## data/
- git_stats.json: commits, ficheros creados por tipo, monstruos y bosses animados, líneas de código y commits por semana

## video/
- mf_trailer_1080.mp4 / _720.mp4: trailer horizontal (106 s; sale de `D:/leafblade/Timeline 1.mov`, 22-06-2026) + poster_mf_trailer_1080.jpg
- mf_trailer_vertical_720.mp4: trailer vertical sin textos (50 s) + póster
- mf_bossfight_titan_amanita_1080.mp4 / _720.mp4: combate contra Titan Amanita (51 s) + póster
- clip_umbrawk_ultimate.mp4 (4 s), clip_titan_golunk_idle.mp4 (6 s) + pósters
- qa_bug_report_720.mp4: vídeo de reporte de bugs con anotaciones (47 s), prueba del trabajo de QA/debugging
- short_cactuarv3 / short_ladon_v2 / short_geoquil_v1 / short_intro / short_steam_next_fest .mp4: shorts verticales de marketing ("Today's Monster")

## gifs/ (MP4 ligeros convertidos de `D:/leafblade/gifs`)
- Ataques y habilidades: umbraukability, umbraukattack, crovixability, crovixattack, avalunkability, avalunkattack, ladonability, ladonattack, nymerionability, nymerionattack, tidreonability, tidreonattack, flarelynx, enokita_attack
- Ciclos: enokita_alk (walk), amanita_walk, prim_walk, last_bearer
- VFX y shaders: fost_glow (brillo nocturno de Frostling), estatuav7 (aura de las ruinas), water_explosion, water_shader, water_heiser (géiser), tornado.gif
- Escenario: caveblocker, amanitafinalblocker, bushanimation, regar_animation
- nighgloom-clean.gif

## frames/ (animación frame a frame sacada de los PSB del repo)
Cada secuencia tiene carpeta con los frames en PNG transparente, `_strip.jpg` (tira) y `_loop.mp4` (bucle a 10 fps):
titan_skull_idle (8), titan_skull_intro (7), nightgloom (9), wispin (9), frostling_idle (6), icestrike (6), tornado (4), ladon_water (6), cryothorn (7)
Muchos PSB son hojas de piezas por frame: las partes del rig se mantienen y se redibujan los elementos orgánicos (humo, agua, fuego). Es la técnica híbrida.

## rigs/
- <monstruo>_pieces.png + <monstruo>_layers/: piezas separadas para el rig esqueletal 2D (titan_golunk, final_boss, amanita, cactuar, umbrawk, glacibark)

## vfx-process/ (PSD de `D:/leafblade/vfx animations` y bosses)
- geiser_concept_art.png (referencia → diseño → en juego), geiser_animation_v1.png, tornado_v3.png, explosion_2_vimpact.png, air_slash_v2.png, pluma.png, ladonprojectile.png
- attack_left_4_.png, hoe_.png: frames del Last Bearer
- swamp_final_boss_v4.png: diseño del boss final del pantano

## Descartado
- Bestiario renderizado desde Unity: los rigs SpriteSkin no se deforman bien en modo edición
- `gifs/animacion bocadillo.gif`: es una referencia de otro juego, no es material propio

## marketing/ (de `D:/leafblade/Marketing`)
- carrousel_houses, carrousel_springleaf, carrousel_comida, carrousel_cooperativa, carrousel_crops, carrousel_crops2, carrousel_decorations: diapositivas 4:5 para Instagram en JPG (las versiones 9:16 para TikTok siguen en el original)
- portada_cacatuar.jpg, portada_geoquil.jpg: portadas de los shorts


Estudio: Wayward Games · Steam: https://store.steampowered.com/app/2887740/Leafblade/
