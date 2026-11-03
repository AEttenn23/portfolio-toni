# Lista de grabación para el portfolio

Marca cada cosa con `x` cuando la tengas: `- [x]`.

## Antes de empezar (ajustes comunes)

- **Grabar con:** OBS (pantalla) o Unity Recorder (`Window > General > Recorder`). El Recorder da vídeo limpio, sin UI del editor y a framerate fijo.
- **Resolución:** 1920×1080, 60 fps. Para GIFs de la web, recorta luego a 800 px de ancho.
- **Fondo limpio** para las animaciones sueltas: color plano (gris medio o el color de la web) o el escenario real desenfocado.
- **Duración:** cada clip de 3 a 8 s, en bucle si es posible (idle, walk). Graba de más y recorta después.
- **Nombre de los archivos:** `proyecto_personaje_accion.mp4`, por ejemplo `mf_frostling_attack.mp4`.
- Guárdalo todo en `D:\pagina web toni\grabaciones\mf\` y `...\twd\`.

---

## MonsterFarm (2D)

> Escena ideal para las animaciones aisladas: `Assets/Scenes/animacion Toni.unity`.
> Para el gameplay, las escenas de Meadow, Farm y los bosses.

### A. Bosses (pieza estrella: animación, VFX y código)
Prefabs en `Assets/Prefabs/Bosses/`.

- [ ] **Titan Golunk:** idle, cada ataque y la muerte. Primero aislado y luego en combate real.
- [ ] **Titan Skull:** idle y ataques, más el `TitanSkullBossGeyser` y el `TitanSkullBossProjectile`.
- [ ] **Last Bearer:** la aparición (`LastBearer_FirstSeen`) y el combate.
- [ ] **Titan Amanita:** animaciones, god rays, esporas y raíces.
- [ ] 1 clip de **combate completo** contra un boss (20 a 40 s) para el reel.
- [ ] **Captura del código** de `TitanGolunkBossBehaviour.cs`: la parte de la máquina de estados o los ataques, con buen tema y letra grande.

### B. Animación híbrida (frames dibujados + esqueletal)
- [ ] **Frostling:** animación final en juego.
- [ ] **Frostling:** los frames sueltos `frostling f1–f6` (captura del PSB en Photoshop o en el Sprite Editor).
- [ ] **Icestrike:** animación final y frames `icestrike f1–f6`.
- [ ] **Cryothorn spin:** el giro final y los frames `spin1–6`.
- [ ] **Tornado:** frames `Tornado_f1–f4` y el resultado.
- [ ] **Vídeo comparativo** (lo montas en edición): frames dibujados a la izquierda, rig a la derecha y el resultado en juego debajo.

### C. Rigs (breakdown de animación esqueletal)
- [ ] Captura del **Skinning Editor** con los huesos de 2 o 3 monstruos (Golunk, Avalunk, Flarelynx…).
- [ ] Un **timelapse o clip corto** moviendo el rig en modo pose.
- [ ] Captura del **Animator Controller** de un monstruo (estados y transiciones).

### D. Galería de criaturas (GIFs en bucle)
Para cada uno: **idle**, **walk** y **attack** sobre fondo limpio.

- [ ] Frostling
- [ ] Ladon (con sus efectos de agua)
- [ ] Nymerion
- [ ] Avalunk
- [ ] Pyrelune
- [ ] Owl (incluido el teletransporte y la muerte)
- [ ] Wispin
- [ ] Cindibat
- [ ] Glacibark / Glacirage
- [ ] Enokita / Kingnita
- [ ] Swirlbloom / Vinetoss (con el boomerang)
- [ ] Amanita
- [ ] Extra: las versiones mascota (`Assets/Prefabs/Pets/`) en la granja, comiendo y brillando de noche.

### E. VFX
Carpeta `Assets/Art/FX/`. Graba cada efecto **aislado sobre fondo oscuro** y también **en contexto**.

- [ ] **Ruinas:** aura, outline, partículas mágicas y la explosión (`Environment/Ruins/`).
- [ ] **Agua de Ladon:** proyectil, explosión de agua, curación y área de curación.
- [ ] **Fuego:** FireCharge1, FireCharge2 y FireExplosion.
- [ ] **Tornado** con sus chispas.
- [ ] **Air slash** con las plumas de cuervo.
- [ ] **Explosión** genérica.
- [ ] **Géiser** y **partículas de hielo**.
- [ ] **Agua del Titan Swamp:** gotas y estela.
- [ ] **Breakdown de shaders:** captura del grafo de `MagicPool`, `FireOutline` y `SDFOutline` en Shader Graph, y del VFX Graph de 1 o 2 efectos.
- [ ] **Día y noche:** las mascotas que brillan (Frostling, luces de mascotas).

### F. Debugging (opcional, para el caso de estudio)
- [ ] Captura del **antes y después** de algún bug visual, si puedes reproducirlo (sombras de árboles, icono de comida, etc.).

---

## TheWorkingDead (3D toon)

> Puedes grabar desde el **build** (`D:\TheWorkinDead\TheWorkingDead_Project.exe`) para el gameplay limpio
> y desde el **editor** para gizmos y depuración.
> Escenas en `Design/Scenes/BuildScenes/`.

### A. Gameplay y reel
- [ ] **Clip de gameplay de 30 a 60 s:** el jugador hace tareas mientras Peralta patrulla (Level 1 o 2).
- [ ] **Que te pille:** Peralta te detecta, sale la exclamación y aparece la pantalla de perder.
- [ ] **Ganar un nivel** y la escena de victoria.
- [ ] **Menú principal** con las animaciones de la UI (título, cerebro, botones).

### B. IA del enemigo (tu código estrella aquí)
- [ ] **Cono de visión en el editor:** que se vean los gizmos de `VisionCone` mientras Peralta patrulla.
- [ ] **El mismo momento en el juego** (feedback del campo de visión y la exclamación). Luego los montas uno al lado del otro.
- [ ] **Captura del código** de `VisionCone.cs` y de la parte tuya de `EnemyAiBase.cs`.

### C. Sistema de tareas
- [ ] Un clip corto de **cada minijuego**: impresora, PC (escribir texto), aire acondicionado, regar la planta, Simón dice, ordenar cajas.
- [ ] La **flecha que señala la misión** (`Window_QuestPointer`) guiando al jugador.
- [ ] El **feedback de tarea completada** y las monedas.
- [ ] La **tienda / máquina expendedora** y los snacks y bebidas energéticas.

### D. Tutorial y NPCs
- [ ] **Tutorial:** diálogos y Bzz guiando al jugador.
- [ ] **NPCs aleatorios** caminando y sentados (que se note la variedad).
- [ ] **Conversación** con un NPC.
- [ ] **Saltar cinemática** manteniendo pulsado (`HoldToSkip`).

### E. Look visual (arte técnico)
- [ ] **Recorrido lento de cámara** por la oficina (iluminación, decals, props).
- [ ] **Antes y después de los shaders:** un objeto o escena con material estándar frente a toon, painterly o outline. Desactívalos en el editor para la comparación.
- [ ] **Luces que parpadean** (`lightflicker`) y **parpadeo de ojos** (`eyes_blink`).
- [ ] **Post-procesado activado y desactivado** (bloom, color).
- [ ] **Skybox.**

### F. Animación 3D
- [ ] **Greg:** idle, andar, correr y Paper Greg en el menú.
- [ ] **Peralta:** patrulla y animaciones de detección.
- [ ] **Ana** y los NPCs con su animator de animaciones aleatorias.

### G. Producción (capturas, no vídeo)
- [ ] Captura del **grafo de ramas de Git** (GitHub o GitKraken) donde se vean tus merges y PRs.
- [ ] Captura de **tu perfil de contribuciones** al repositorio.

---

## Para la web en general
- [ ] **Foto o avatar tuyo**, o un autorretrato dibujado y animado (encaja con el estilo gamberro).
- [ ] 2 o 3 **bocetos o hojas de animación en papel**, si tienes. Quedan genial en un estilo "cuaderno de bocetos".
- [ ] Un **garabato animado propio** para la home: un monstruo que salude, el logo moviéndose…

## Antes de publicar
- [ ] Confirmar con el equipo de MonsterFarm que puedes enseñar el material.
- [ ] Confirmar quién hizo los modelos de TheWorkingDead (Greg, Peralta, Ana) y los shaders painterly y sketch, para dar los créditos correctos.
