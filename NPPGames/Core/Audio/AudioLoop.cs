using System.Collections.Generic;
using System.Threading;

namespace NPPGames.Core.Audio
{
    public class AudioLoop : IPlayAudio
    {
        public AudioLoop()
        {
            IsActive = true;
            TrackIndex = -1;
            Tracks = new List<IAudioTrack>();
            AudioThread = new Thread(new ThreadStart(Loop));
            AudioThread.Start();
        }

        public Thread AudioThread { get; }

        public bool IsActive { get; set; }

        public bool IsPlaying { get; set; }


        public List<IAudioTrack> Tracks { get; set; }

        public int TrackIndex { get; set; }

        public void AddTrack(IAudioTrack audio)
        {
            Tracks.Add(audio);
        }

        private void Loop()
        {
            while (IsActive)
            {
                if(IsPlaying)
                {
                    AudioLoopIteration();
                }
            }
        }

        public void AudioLoopIteration()
        {
            for (int i = 0; i < Tracks.Count; i++)
            {
                if(IsPlaying)
                {
                    TrackIndex = i;
                    Tracks[TrackIndex].Play();
                }
                else
                {
                    Tracks[TrackIndex].Pause();
                }
            }
        }

        public void Pause()
        {
            IsPlaying = false;

        }

        public void Play()
        {
            IsPlaying = true;
        }
    }
}
