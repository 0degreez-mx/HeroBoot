# 📖 HeroBoot – Readme  

## Español  

### ¿Qué es HeroBoot?  
HeroBoot es una aplicación intermedia que transforma el arranque de tu PC con Windows 11 en una experiencia tipo consola:  

- Reproduce un video de arranque (intro).  
- Muestra un slideshow con *imágenes hero* de tus juegos.  
- Abre tu launcher preferido (Steam, Playnite, Heroic, etc.).  
- Se cierra automáticamente cuando detecta que el launcher ya está corriendo.  

---

### Archivos incluidos  
- **`HeroBoot.exe`** → aplicación principal.  
- **`AddHeroBootStartup.bat`** → agrega HeroBoot al inicio de Windows.  
- **`startup-video.mp4`** → video de arranque.  
  - ⚡ Para cambiar la animación, simplemente reemplaza este archivo por tu propio video.  

---

### Configuración  
HeroBoot usa estas opciones:

- `HeroImagesPath`: Carpeta con imágenes
- `HeroImagePattern`: Patrón para buscar imágenes, ej. *_hero.jpg
- `LauncherPath`: Ruta del launcher con doble diagonal invertida (Steam.exe, Playnite.exe, etc.), ej. C:\\Program Files\\Steam\\steam.exe
- `OverrideFSE`: Boleano para reemplazar la app Xbox con el launcher configurado al usar Experiencia de pantalla completa.
- `PlayIntroVideo`: Boleano para reproducir video de arranque

---

### Ejemplo de configuración:
- HeroImagesPath = D:\\HeroImages
- HeroImagePattern = *_hero.jpg
- LauncherPath = C:\\Program Files (x86)\\Steam\\steam.exe
- OverrideFSE = true
- PlayIntroVideo = true

---

### Instrucciones de instalación
1. Coloca tus imágenes hero en la carpeta indicada en HeroImagesPath.
2. Reemplaza el archivo startup-video.mp4 si quieres usar otro video de arranque.
3. Edita las opciones para apuntar al launcher que quieras usar. 
4. Ejecuta AddHeroBootStartup.bat para que HeroBoot se ejecute automáticamente al arrancar Windows. 
5. (Opcional) Si usas la Experiencia de pantalla completa de Windows 11, configura la opción OverrideFSE en true.

---

### ✅ Flujo de arranque

- Enciendes tu dispositivo.
- Windows abre la app Xbox en pantalla completa (si está configurada).
- HeroBoot se lanza automáticamente:
  - Video intro (si está activado).
  - Slideshow de imágenes.
  - Apertura del launcher configurado.
- HeroBoot se cierra solo → quedas en el launcher.

---

## English

### What is HeroBoot?

HeroBoot is an intermediate app that transforms your Windows 11 PC boot process into a console-like experience:

- Plays a custom boot video (intro).
- Displays a slideshow of your favorite game *hero images*.
- Launches your preferred game launcher (Steam, Playnite, Heroic, etc.).
- Automatically closes after detecting the launcher is running.

---

### Included files

- **`HeroBoot.exe`** → main application.
- **`AddHeroBootStartup.bat`** → adds HeroBoot to Windows startup.
- **`startup-video.mp4`** → boot video.
  -⚡ To change the animation, simply replace this file with your own video.

---

### Configuration
HeroBoot uses these options:
- `HeroImagesPath`: Folder with images
- `HeroImagePattern`: Pattern for image search, e.g. *_hero.jpg
- `LauncherPath`: Path to the launcher (Steam.exe, Playnite.exe, etc.), e.g. C:\\Program Files\\Steam\\steam.exe
- `OverrideFSE`: Boolean to replace the full screen experience xbox app.
- `PlayIntroVideo`: Boolean to play boot video

---

### Example configuration:
- HeroImagesPath = D:\\HeroImages
- HeroImagePattern = *_hero.jpg
- LauncherPath = C:\\Program Files (x86)\\Steam\\steam.exe
- OverrideFSE = true
- PlayIntroVideo = true

---

### Installation steps
- Place your hero images into the folder defined in HeroImagesPath.
- Replace startup-video.mp4 if you want to use a different boot video.
- Edit the options to point to your preferred launcher.
- Run AddHeroBootStartup.bat to make HeroBoot run automatically on Windows startup.
- (Optional) If you’re using Windows 11 Full Screen Experience, set the OverrideFSE to true.

---

### ✅ Boot flow
- Power on your device.
- Windows opens the Xbox app in fullscreen (if configured).
- HeroBoot runs automatically:
  - Boot intro video (if enabled).
  - Slideshow of images.
  - Launches your chosen launcher.
- HeroBoot auto-closes → you’re left inside your launcher.
