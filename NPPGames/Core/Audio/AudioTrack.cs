using System.IO;
using System.Media;

namespace NPPGames.Core.Audio
{
    public class AudioTrack : IAudioTrack
    {
        public AudioTrack()
        {
        }
        public AudioTrack(Stream stream)
            : this()
        {
            OpenStream(stream);
        }
        public SoundPlayer Player { get; set; }

        public void OpenStream(Stream stream)
        {
            Player = new System.Media.SoundPlayer(stream);
        }

        public virtual void Play()
        {
            Player.Play();
        }

        public virtual void Pause()
        {
            Player.Stop();
        }
    }
}
