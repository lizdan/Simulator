using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator.GUI
{
    /// <summary>
    /// Manages playback of a music file in a separate background thread.
    /// It provides methods to start and stop the music.
    ///
    /// @Author Liza Danielsson.
    /// </summary>
    public class MusicPlayer
    {
        private SoundPlayer soundPlayer;
        private Thread musicThread;
        private bool isPlaying;

        public MusicPlayer(string filePath)
        {
            soundPlayer = new SoundPlayer(filePath);
            isPlaying = false;
        }


        /// <summary>
        /// Starts the music playback in a background thread if it is not already playing.
        /// </summary>
        public void StartMusic()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                musicThread = new Thread(new ThreadStart(PlayMusic)) // Start new background thread if music is not playing
                {
                    IsBackground = true
                };
                musicThread.Start();
            }
        }


        /// <summary>
        /// The method that runs on the background thread to play the music continuously until stopped.
        /// </summary>
        private void PlayMusic()
        {
            try
            {
                soundPlayer.PlayLooping(); // Play the music file in a loop
                while (isPlaying)
                {
                    Thread.Sleep(100); // Use a sleep to wait for the stop signal
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Stops the music playback and waits for the background thread to end.
        /// </summary>
        public void StopMusic()
        {
            if (isPlaying)
            {
                isPlaying = false;
                musicThread.Join(); // Wait for the thread to finish.
                soundPlayer.Stop(); // Stop the music playback.
            }
        }
    }
}
