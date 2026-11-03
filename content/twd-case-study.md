# TheWorkingDead: caso de estudio

> Textos para la web, en español e inglés. Tono profesional y neutro.
> Los datos salen del repositorio (git log / git blame) y de la publicación de The Rookies.
> Las rutas de los medios son relativas a `assets/twd/`.

---

# ES

## Cabecera

**TheWorkingDead**
Juego 3D de oficina con zombis · Unity 6 (URP) · Equipo de 6 personas · Fizh Studio, Florida Universitària · Octubre 2025 – Febrero 2026

**Rol:** Dirección de arte · Animación · Gameplay programmer · Game design · Technical artist

**Etiquetas:** `Código` `3D` `Animación` `Shaders` `Dirección de arte`

**En cifras**
- 53 % del código del juego (5.145 de 9.689 líneas)
- 232 commits, el mayor número del equipo
- 6 niveles montados e iluminados
- 2 cinemáticas con animación cutout

Medio de cabecera: `video/twd_intro_1080.mp4` (póster: `video/poster_greg.jpg`)
Trailer: `video/twd_trailer_480.mp4` (póster: `video/poster_trailer.jpg`; solo existe a 480p)

## Resumen

TheWorkingDead es un juego de sigilo y gestión en el que el jugador controla a Greg, un oficinista convertido en zombi que debe completar su jornada laboral sin que su jefe, Peralta, lo descubra.

Lo desarrollamos como proyecto académico durante cinco meses. Fui el director de arte del proyecto y mi trabajo combinó animación, programación de gameplay y apartado técnico-artístico: la IA del enemigo, el sistema de tareas, el tutorial, los NPCs, la iluminación, la configuración de shaders, la animación de los personajes y la de las cinemáticas. También me encargué de generar los builds y de integrar el trabajo del equipo en cada entrega.

## 1. Cinemáticas

Medios: `video/twd_intro_1080.mp4`, `video/twd_win_1080.mp4` · clips `video/clip_intro_*.mp4`, `video/clip_win_*.mp4` · GIFs `video/gif_cabinet.gif`, `video/gif_zombie.gif`, `video/gif_polaroid.gif` · proceso `preprod/storyboard.jpg`, `preprod/animatic_720.mp4` · cutout `cutout/atlas_greg_human_*.png`, `cutout/atlas_greg_zombie_*.png`, `cutout/portada_cinematica.jpg`

Las cinemáticas de inicio y de victoria usan una técnica cutout: personajes 2D de papel animados dentro de un entorno 3D. Me encargué de la animación, la dirección de arte y las pruebas de texturas de los modelos 3D.

**Proceso:** dibujé un storyboard de 16 planos para la cinemática de inicio, con las notas de cámara y la lista de sonidos. Después monté una animática en gris para cerrar el ritmo y los encuadres antes de animar la versión final en Blender.

- Greg está recortado por piezas (cabeza, pelo, cuerpo y brazos) con textura de papel y chinchetas en las articulaciones. Hay una versión humana y otra zombi, con vistas frontal, trasera y lateral.
- Rig de huesos para el cuerpo, el bigote y los brazos del personaje.
- Shader en Blender que cambia entre las vistas del sprite (frontal, trasera y lateral) sin cambiar de objeto.
- Conversión del arte 2D en atlas de texturas, para poder cambiar la textura de todo el cuerpo con un único parámetro.

Créditos: arte 2D de Francisco Rodríguez · assets 3D, logos animados y montaje de Sara López · efectos de sonido de Raquel Garcerán.

## 2. Animación 3D de personajes

Medios: `anim3d/greg_idle_walk_grid.mp4`, `anim3d/peralta_idle_walk_grid.mp4` (pósters `anim3d/poster_*_grid.jpg`) · clips sueltos `anim3d/gregidlefront.mp4` … `anim3d/peraltawalkside.mp4` · `screens/pres/greg_zombie_turn.gif`, `screens/pres/npc_lineup.png`

Animé en Blender los ciclos del protagonista, del enemigo y de los NPCs, y los exporté a Unity:

- **Greg:** idle, caminar, correr y una animación del brazo suelto que se le cae.
- **Peralta:** idle y caminar, con una postura más rígida y amenazante que la de Greg.
- **NPCs:** variaciones de idle sentado, la patada a la máquina y las animaciones del arcade.

## 3. IA del enemigo

Medios: `video/ai_cone_states.mp4`, `video/gif_ai_cone_states.gif` · `screens/web/ai_cone_1_patrol.jpg`, `ai_cone_2_chase.jpg`, `ai_cone_3_caught.jpg` · código `code/EnemyAiBase_vision.cs`, `code/VisionCone.cs`

Peralta patrulla el nivel con un NavMeshAgent, por puntos de ruta o por destinos aleatorios. La detección se calcula cada 0,5 segundos en lugar de en cada frame, y combina distancia, ángulo del campo de visión y un raycast contra obstáculos, así que las paredes y el mobiliario bloquean la visión.

El campo de visión se representa con una malla generada en tiempo de ejecución que cambia de color y de alcance según el estado de la IA:

1. **Patrulla:** el enemigo recorre el nivel.
2. **Persecución:** el jugador entra en el rango de visión. Aparece un indicador de alerta y el enemigo lo sigue.
3. **Detección:** el jugador está demasiado cerca. El enemigo se detiene y se llena una barra de sospecha; si se completa, la partida termina.

El objetivo era que el jugador entendiera en todo momento por qué le han detectado.

## 4. Sistema de tareas

Medios: `diagrams/task-system.svg` · `screens/web/l2_task_*.jpg` · código `code/TaskBase.cs`, `code/TareasAleatorias.cs`

Diseñé el sistema de tareas para que añadir minijuegos nuevos fuera sencillo:

- **`TaskBase`** es una clase abstracta que reúne la lógica común: detección del jugador, outline del objeto al acercarse, indicador de tarea pendiente, bloqueo del movimiento durante la tarea, resolución de victoria o derrota con su feedback, y comunicación con el gestor de tareas.
- Cada minijuego solo implementa `IniciarTarea()` y `CancelarTarea()`.
- **`TareasAleatorias`** genera la lista de cada partida combinando tareas obligatorias, una tarea especial con un 20 % de probabilidad, tareas aleatorias y hasta tres conversaciones con NPCs.

Programé las tareas del PC, la impresora, el ordenador, la planta, el aire acondicionado y las conversaciones con NPCs. El resto del equipo usó la misma base para desarrollar sus minijuegos.

## 5. Tutorial, NPCs y sistemas de juego

Medios: `video/twd_gameplay_480.mp4` (póster `video/poster_gameplay.jpg`; 480p, conviene sustituirlo por una grabación a 1080p) · `screens/web/tut_*.jpg` · `screens/pres/minigame_arcade.jpg`, `screens/pres/minigame_almacen.jpg` · `ui/npc-icons/`

- **Tutorial:** controlador del jugador específico, diálogos guiados y un personaje acompañante (Bzz).
- **NPCs:** generación aleatoria, NPCs sentados con variaciones de animación y sistema de conversación.
- **Sistemas:** tienda y máquina expendedora, monedas, consumibles, menú de pausa, indicador de dirección hacia el objetivo, opción de saltar cinemáticas, y pantallas de victoria y derrota.
- **Detalles visuales:** parpadeo de ojos, luces intermitentes y detección de iluminación.

## 6. Dirección visual: shaders, iluminación y post-procesado

Medios: `video/shader_steps_office.mp4`, `video/shader_steps_reception.mp4` · `screens/web/shader_office_1_plain.jpg` → `_2_stylized.jpg` → `_3_final.jpg`

Como director de arte, definí un estilo visual que combina sombreado toon, acabado pictórico y contorno de boceto, para encajar con la estética de papel de las cinemáticas. Trabajé el aspecto final en tres capas:

1. **Iluminación** de cada nivel: luces de pantallas, lámparas y zonas de color.
2. **Shaders:** adapté y configuré shaders de terceros (painterly, sketch outline y MK Toon) para el escenario y los personajes.
3. **Post-procesado:** bloom, corrección de color y skybox en degradado.

**Props:** modelé y texturicé en Substance Painter la impresora y el ordenador. Para unificar los colores de los props con el resto del escenario usé un gradient map con textura de acuarela.

## 7. Niveles

Medios: `screens/pres/env_1.jpg` … `env_8.jpg` · `screens/web/tut_wide_center.jpg`, `l1_wide_center.jpg` … `l5_wide_center.jpg`

Monté el tutorial y los niveles 1 a 4, incluyendo su iluminación y decoración. El nivel crece a medida que avanza el juego: se abren nuevas zonas y aumenta el número de tareas. También redacté el documento de diseño de niveles, donde se define la dificultad de cada tarea en cada nivel.

## 8. Producción

Medios: `diagrams/contrib-commits.svg`, `diagrams/contrib-lines.svg`, `diagrams/contrib-weekly.svg`

Me encargué de los builds de cada entrega y de gran parte de la integración en Git y GitHub: revisión y merge de pull requests, resolución de conflictos en escenas y mantenimiento de una rama estable.

## Herramientas
Unity 6 (URP) · C# · NavMesh · Cinemachine · Shader Graph · Blender · Substance Painter · Photoshop · Git / GitHub

## Equipo
- Toni Díaz Plaza: dirección de arte, animación, programación y game design
- Santiago Marqués: programador principal
- Francisco Rodríguez: character y concept artist
- Sara López: artista 3D y VFX, programación
- Felix Rojas: diseño de interfaz y artista 3D
- Raquel Garcerán: artista 3D

---

# EN

## Header

**TheWorkingDead**
3D office zombie game · Unity 6 (URP) · 6-person team · Fizh Studio, Florida Universitària · October 2025 – February 2026

**Role:** Art direction · Animation · Gameplay programming · Game design · Technical art

**Tags:** `Code` `3D` `Animation` `Shaders` `Art direction`

**At a glance**
- 53% of the game's code (5,145 of 9,689 lines)
- 232 commits, the most on the team
- 6 levels built and lit
- 2 cutout-style cinematics

## Overview

TheWorkingDead is a stealth and management game where the player controls Greg, an office worker turned zombie who has to get through his workday without his boss, Peralta, finding out.

We developed it as a five-month academic project. I was the project's art director, and my work combined animation, gameplay programming and technical art: enemy AI, the task system, the tutorial, NPCs, lighting, shader setup, character animation and cinematic animation. I was also responsible for builds and for integrating the team's work for each milestone.

## 1. Cinematics

The intro and victory cinematics use a cutout technique: 2D paper characters animated inside a 3D environment. I handled the animation, art direction and texture tests for the 3D models.

**Process:** I drew a 16-shot storyboard for the intro cinematic, with camera notes and a sound list. I then cut a greyscale animatic to lock timing and framing before animating the final version in Blender.

- Greg is cut into pieces (head, hair, body and arms) with a paper texture and pins at the joints. There is a human and a zombie version, each with front, back and side views.
- Bone rig for the character's body, moustache and arms.
- A Blender shader that switches between sprite views (front, back and side) without swapping objects.
- 2D artwork converted into texture atlases, so the whole body's texture changes with a single parameter.

Credits: 2D art by Francisco Rodríguez · 3D assets, animated logos and editing by Sara López · sound effects by Raquel Garcerán.

## 2. 3D character animation

I animated the cycles for the player character, the enemy and the NPCs in Blender and exported them to Unity:

- **Greg:** idle, walk, run and a loose-arm animation where his arm falls off.
- **Peralta:** idle and walk, with a stiffer, more threatening posture than Greg's.
- **NPCs:** seated idle variations, the vending-machine kick and the arcade animations.

## 3. Enemy AI

Peralta patrols the level with a NavMeshAgent, following waypoints or random destinations. Detection runs every 0.5 seconds instead of every frame and combines distance, field-of-view angle and a raycast against obstacles, so walls and furniture block line of sight.

The field of view is a mesh generated at runtime that changes colour and range with the AI state:

1. **Patrol:** the enemy walks the level.
2. **Chase:** the player enters the sight range. An alert icon appears and the enemy follows.
3. **Caught:** the player is too close. The enemy stops and a suspicion bar fills; if it completes, the game is over.

The goal was for players to always understand why they had been spotted.

## 4. Task system

I designed the task system so new minigames could be added easily:

- **`TaskBase`** is an abstract class that holds the shared logic: player detection, object outline on approach, pending-task indicator, movement lock during the task, win/lose resolution with feedback, and communication with the task manager.
- Each minigame only implements `IniciarTarea()` (start) and `CancelarTarea()` (cancel).
- **`TareasAleatorias`** builds each run's task list from mandatory tasks, a special task with a 20% chance, random tasks and up to three NPC conversations.

I programmed the PC, printer, computer, plant and air-conditioning tasks and the NPC conversations. The rest of the team used the same base for their own minigames.

## 5. Tutorial, NPCs and game systems

- **Tutorial:** dedicated player controller, guided dialogue and a companion character (Bzz).
- **NPCs:** random spawning, seated NPCs with animation variations, and a conversation system.
- **Systems:** shop and vending machine, coins, consumables, pause menu, objective direction indicator, hold-to-skip cinematics, and win/lose screens.
- **Visual details:** eye blinking, flickering lights and light detection.

## 6. Visual direction: shaders, lighting and post-processing

As art director, I defined a look that combines toon shading, a painterly finish and sketch-style outlines to match the paper aesthetic of the cinematics. I built it in three layers:

1. **Lighting** for each level: screen lights, lamps and colour zones.
2. **Shaders:** I adapted and configured third-party shaders (painterly, sketch outline and MK Toon) for environments and characters.
3. **Post-processing:** bloom, colour grading and a gradient skybox.

**Props:** I modelled the printer and the computer and textured them in Substance Painter. A watercolour gradient map brought their colours in line with the rest of the environment.

## 7. Levels

I built the tutorial and levels 1 to 4, including lighting and set dressing. The office grows as the game progresses, opening new areas and adding more tasks. I also wrote the level design document, which sets the difficulty of each task per level.

## 8. Production

I handled the builds for each milestone and much of the Git/GitHub integration: reviewing and merging pull requests, resolving scene conflicts and keeping a stable branch.

## Tools
Unity 6 (URP) · C# · NavMesh · Cinemachine · Shader Graph · Blender · Substance Painter · Photoshop · Git / GitHub

## Team
- Toni Díaz Plaza: art direction, animation, programming and game design
- Santiago Marqués: lead programmer
- Francisco Rodríguez: character and concept artist
- Sara López: 3D and VFX artist, programming
- Felix Rojas: UI designer and 3D artist
- Raquel Garcerán: 3D artist
