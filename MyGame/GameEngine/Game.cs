﻿using System;
using System.Collections.Generic;
using MyGame.GameEngine;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine
{
    // The Game manages scenes and runs the main game loop.
    static class Game
    {
        // The number of frames that will be drawn to the screen in one second.
        private const int FramesPerSecond = 60;

        public const bool SquarePixels = true;

        public static char[] randomCharacters;

        public static Screen screen;
        public static Screen lastFrame;

        // We keep a current and next scene so the scene can be changed mid-frame.
        private static Scene _currentScene;
        private static Scene _nextScene;

        // Cached textures
        private static readonly Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        // Cached sounds
        private static readonly Dictionary<string, SoundBuffer> Sounds = new Dictionary<string, SoundBuffer>();

        // Cached fonts
        private static readonly Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

        // A flag to prevent being initialized twice.
        private static bool _initialized;

        // A Random number generator we can use throughout the game. Seeded with a constant so 
        // the game plays the same every time for easy debugging.
        // @TODO: provide a method to randomize this for when they want variety.
        public static Random Random = new Random();

        // Creates our render window. Must be called once at startup.
        public static void Initialize(uint windowWidth, uint windowHeight)
        {
            randomCharacters = new char[10];
            randomCharacters[0] = '@';
            randomCharacters[1] = '#';
            randomCharacters[2] = '$';
            randomCharacters[3] = '%';
            randomCharacters[4] = '&';
            randomCharacters[5] = '*';
            randomCharacters[6] = '/';
            randomCharacters[7] = '\\';
            randomCharacters[8] = '~';
            randomCharacters[9] = '=';
            Console.CursorVisible = false;
            screen = new Screen((int)windowWidth, (int)windowHeight);
            lastFrame = new Screen((int)windowWidth, (int)windowHeight);
            // Only initialize once.
            if (_initialized) return;
            _initialized = true;
        }

        // Called whenever you try to close the game window.
        private static void ClosedEventHandler(object sender, EventArgs e)
        {
            
        }


        // Get a texture (pixels) from a file
        public static Texture GetTexture(string fileName)
        {
            Texture texture;

            if (Textures.TryGetValue(fileName, out texture)) return texture;

            texture = new Texture(fileName);
            Textures[fileName] = texture;
            return texture;
        }

        // Get a sound from a file
        public static SoundBuffer GetSoundBuffer(string fileName)
        {
            SoundBuffer soundBuffer;

            if (Sounds.TryGetValue(fileName, out soundBuffer)) return soundBuffer;

            soundBuffer = new SoundBuffer(fileName);
            Sounds[fileName] = soundBuffer;
            return soundBuffer;
        }

        // Get a font from a file
        public static Font GetFont(string fileName)
        {
            Font font;

            if (Fonts.TryGetValue(fileName, out font)) return font;

            font = new Font(fileName);
            Fonts[fileName] = font;
            return font;
        }

        // Returns the active running scene.
        public static Scene CurrentScene
        {
            get { return _currentScene; }
        }

        // Specifies the next Scene to run.
        public static void SetScene(Scene scene)
        {
            // If we don't have a current scene, set it.
            // Otherwise, note the next scene.
            if (_currentScene == null)
                _currentScene = scene;
            else
                _nextScene = scene;
        }

        // Begins the main game loop with the initial scene.
        public static void Run()
        {
            Clock clock = new Clock();

            // Keep looping until the window closes.
            while (true)
            {
                // If the next scene has been set, swap it with the current scene.
                if (_nextScene != null)
                {
                    _currentScene = _nextScene;
                    _nextScene = null;
                    clock.Restart();
                }

                // Get the time since the last frame, then have the scene update itself.
                Time time = clock.Restart();
                _currentScene.Update(time);
            }
        }
    }
}