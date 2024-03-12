using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace RDEngine.GameScripts
{
    public class PauseMenu : GComponent
    {
        private UIObject _panel, _fpsCounter;
        private TextObject _hardcordeText, _musicText, _fpsText;

        private bool _wasPlaying, _toPlay;

        private bool _panelEnabled;

        public PauseMenu(UIObject panel, UIObject fpsCounter, TextObject hardcodeText, TextObject musicText, TextObject fpsText)
        {
            _panel = panel;
            _hardcordeText = hardcodeText;
            _musicText = musicText;
            _fpsText = fpsText;
            _fpsCounter = fpsCounter;
            _panelEnabled = false;
            _toPlay = false;
        }

        public override void Start()
        {
            if (PersistentVars.Hardcore)
            {
                _hardcordeText.Text = "[D] Gamemode: Roguelike";
            }
            else
            {
                _hardcordeText.Text = "[D] Gamemode: Checkpoints";
            }

            if (PersistentVars.ShowFPS)
            {
                _fpsCounter.Enabled = true;
                _fpsText.Text = "[A] FPS Counter: On";
            }
            else
            {
                _fpsCounter.Enabled = false;
                _fpsText.Text = "[A] FPS Counter: Off";
            }
        }

        public override void Update()
        {
            if (Input.GetKey(Keys.C, KeyGate.Down))
            {
                Parent.Scene.PauseWorld = !Parent.Scene.PauseWorld;
                _panel.Enabled = !_panel.Enabled;
                _panelEnabled = !_panelEnabled;

                if (MediaPlayer.State == MediaState.Playing)
                    _musicText.Text = "[S] Music: On";
                else
                    _musicText.Text = "[S] Music: Off";

                _wasPlaying = MediaPlayer.State == MediaState.Playing;

                if (_panel.Enabled)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();

                if (!_panelEnabled && _toPlay)
                {
                    SceneHandler.PlaySong(ContentStorer.Songs["MysteryLoop"], true);
                    _toPlay = false;
                }
            }

            if (!_panelEnabled)
                return;

            if (Input.GetKey(Keys.D, KeyGate.Down))
            {
                if (PersistentVars.Hardcore)
                {
                    _hardcordeText.Text = "[D] Gamemode: Checkpoints";
                    PersistentVars.Hardcore = false;
                }
                else
                {
                    _hardcordeText.Text = "[D] Gamemode: Roguelike";
                    PersistentVars.Hardcore = true;
                }
            }

            if (Input.GetKey(Keys.S, KeyGate.Down))
            {
                if (_wasPlaying)
                {
                    MediaPlayer.Stop();
                    _musicText.Text = "[S] Music: Off";
                    _wasPlaying = false;
                    _toPlay = false;
                    PersistentVars.MusicPlaying = false;
                }
                else
                {
                    _toPlay = true;
                    _musicText.Text = "[S] Music: On";
                    _wasPlaying = true;
                    PersistentVars.MusicPlaying = true;
                }
            }

            if (Input.GetKey(Keys.A, KeyGate.Down))
            {
                if (PersistentVars.ShowFPS)
                {
                    _fpsCounter.Enabled = false;
                    _fpsText.Text = "[A] FPS Counter: Off";
                    PersistentVars.ShowFPS = false;
                }
                else
                {
                    _fpsCounter.Enabled = true;
                    _fpsText.Text = "[A] FPS Counter: On";
                    PersistentVars.ShowFPS = true;
                }
            }
        }
    }
}
