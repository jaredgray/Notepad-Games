using System.IO;
using System.Threading;

namespace NPPGames.Core.Audio
{
    public enum DelayType
    {
        WaitBeginning,
        WaitEnd
    }
    public class DelayedAudioTrack : AudioTrack
    {
        public DelayedAudioTrack()
            : base()
        {
            DelayType = DelayType.WaitEnd;
            WaitInMilliseconds = 0;
        }
        public DelayedAudioTrack(Stream stream)
            : base(stream)
        {
            DelayType = DelayType.WaitEnd;
            WaitInMilliseconds = 0;
        }

        public DelayType DelayType { get; set; }

        public int WaitInMilliseconds { get; set; }

        public override void Play()
        {
            base.Play();
            Thread.Sleep(WaitInMilliseconds);
        }
    }
}
