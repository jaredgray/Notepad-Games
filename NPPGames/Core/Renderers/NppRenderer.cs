using Kbg.NppPluginNET.PluginInfrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPPGames.Core.Primitives;

namespace NPPGames.Core.Renderers
{
    public class NppRenderer : IRenderer
    {
        public static bool DO_LOG = false;
        public NppRenderer(INotepadPPGateway npp, IScintillaGateway editor, Logger logger)
        {
            Editor = editor;
            Logger = logger;
            Npp = npp;
        }



        private Logger Logger { get; set; }

        protected INotepadPPGateway Npp { get; private set; }
        private IScintillaGateway Editor { get; set; }

        public Size SceneSize { get; set; }

        public void Initialize(Size sceneSize)
        {
            this.SceneSize = sceneSize;
            Npp.FileNew();
            Editor.SetBufferedDraw(true);
        }

        public void InitializeFrameBuffer()
        {
        }

        public void RenderFrame(IEnumerable<byte> buffer)
        {
            StringBuilder linebuilder = new StringBuilder();
            for (int y = 0; y < SceneSize.Height; y++)
            {
                for (int x = 0; x < SceneSize.Width; x++)
                {
                    linebuilder.Append((char)buffer.ElementAt((y * SceneSize.Width) + x));
                }
                linebuilder.AppendLine();
            }
            AddTextToEditor(linebuilder.ToString());
        }

        private void AddTextToEditor(string text)
        {
            Editor.AddText(text.Length, text);
            Editor.NewLine();
            if (DO_LOG)
            {
                if (null != Logger)
                {
                    Logger.WriteLine(text);
                }
            }
        }

        public void PreRenderFrame()
        {
            Editor.ClearAll();
        }

        public void OnScreenResize(Size newSize)
        {
            if(SceneSize != newSize)
                this.SceneSize = newSize;
        }
    }
}
