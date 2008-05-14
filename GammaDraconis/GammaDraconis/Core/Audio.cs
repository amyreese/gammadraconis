using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GammaDraconis.Core
{
    /// <summary>
    /// The audio manager handles interacting with the Xna/XACT sound system.
    /// </summary>
    class Audio
    {
        static bool NoSound = false;
        // XACT components.
        static AudioEngine audioEngine;
        static SoundBank soundBank;
        static WaveBank waveBank;

        static Dictionary<String, Cue> cues;
        static Dictionary<String, bool> repeat;

        /// <summary>
		/// Create the appropriate XACT components.
		/// </summary>
        static public void init()
        {
            try
            {
                audioEngine = new AudioEngine("Resources/Audio/Engine.xgs");
                soundBank = new SoundBank(audioEngine, "Resources/Audio/Audio.xsb");
                waveBank = new WaveBank(audioEngine, "Resources/Audio/Audio.xwb");
            }
            catch (Exception e)
            {
                NoSound = true;
            }

            cues = new Dictionary<string,Cue>();
            repeat = new Dictionary<string,bool>();
        }

        /// <summary>
		/// Fire-and-forget sound.
        /// </summary>
        /// <param name="cue"></param>
        static public void play(String cue) { play(cue, true); }
        static public void play(String cue, bool overlap)
        {
            if (NoSound) { return; }

            cache(cue);
            if (cues[cue].IsPrepared)
            {
                cues[cue].Play();
            }
            else if (cues[cue].IsPaused)
            {
                cues[cue].Resume();
            }
            else if (overlap)
            {
                recache(cue);
                cues[cue].Play();
            }
        }

        /// <summary>
		/// Start a repeating sound.
        /// </summary>
        /// <param name="cue"></param>
        static public void playRepeat(String cue)
        {
            if (NoSound) { return; }

            cache(cue);
            repeat[cue] = true;
            if (cues[cue].IsPrepared)
            {
                cues[cue].Play();
            }
            else if (cues[cue].IsPaused)
            {
                cues[cue].Stop(AudioStopOptions.Immediate);
                recache(cue);
                cues[cue].Play();
            }
        }

		/// <summary>
		/// Pause a playing sound.
		/// </summary>
		/// <param name="cue"></param>
        static public void pause(String cue)
        {
            if (NoSound) { return; }

            cache(cue);
            if (cues[cue].IsPlaying)
            {
                cues[cue].Pause();
            }
        }

        /// <summary>
		/// Stop a sound.
        /// </summary>
        /// <param name="cue"></param>
        static public void stop(String cue)
        {
            if (NoSound) { return; }

            cache(cue);
            if (cues[cue].IsPlaying || cues[cue].IsPaused)
            {
                cues[cue].Stop(AudioStopOptions.AsAuthored);
            }
            repeat[cue] = false;
        }

        /// <summary>
		/// Stop all sounds.
        /// </summary>
        static public void stopAll()
        {
            if (NoSound) { return; }

            foreach (Cue cue in cues.Values)
            {
                cue.Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
		/// Cache the Cue object and whether it should be repeating.
        /// </summary>
        /// <param name="cue"></param>
        static private void cache(String cue)
        {
            if (NoSound) { return; }

            if (!cues.ContainsKey(cue))
            {
                cues.Add(cue, soundBank.GetCue(cue));
                repeat.Add(cue, false);
            }
        }

        /// <summary>
		/// Recache a new version of a cue.
        /// </summary>
        /// <param name="cue"></param>
        static private void recache(String cue)
        {
            if (NoSound) { return; }

            cues[cue] = soundBank.GetCue(cue);
        }

        /// <summary>
		/// Set a global variable in the AudioEngine.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        static public void set(String name, float value)
        {
            if (NoSound) { return; }

            audioEngine.SetGlobalVariable(name, value);
        }

        /// <summary>
		/// Get a global variable from the AudioEngine.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The value of the variable.</returns>
        static public float? get(String name)
        {
            if (NoSound) { return null; }

            return audioEngine.GetGlobalVariable(name);
        }

        /// <summary>
		/// Pass the update call to the AudioEngine.
        /// </summary>
        static public void update()
        {
            if (NoSound) { return; }

            foreach (String cue in cues.Keys)
            {
                bool repeatable = repeat[cue];

                if (cues[cue].IsStopped)
                {
                    recache(cue);

                    if (repeatable)
                    {
                        cues[cue].Play();
                    }
                }
            }
            
            audioEngine.Update();
        }
    }
}
