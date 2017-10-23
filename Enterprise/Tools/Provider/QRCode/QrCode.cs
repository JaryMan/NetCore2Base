using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace MeiMing.Framework.Provider
{
    public class QrCode
    {
        public List<BitArray> ModuleMatrix { get; set; }
        private int version;

        public QrCode(int version)
        {
            this.version = version;
            var size = ModulesPerSideFromVersion(version);
            ModuleMatrix = new List<BitArray>();
            for (int i = 0; i < size; i++)
                ModuleMatrix.Add(new BitArray(size));
        }

        public Bitmap GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.Black, Color.White);
        }

        public Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex)
        {
            return GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex));
        }

        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor)
        {
            var size = ModuleMatrix.Count * pixelsPerModule;
            Bitmap bmp = new Bitmap(size, size);
            Graphics gfx = Graphics.FromImage(bmp);
            for (int x = 0; x < size; x = x + pixelsPerModule)
            {
                for (int y = 0; y < size; y = y + pixelsPerModule)
                {
                    var module = ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                    if (module)
                    {
                        gfx.FillRectangle(new SolidBrush(darkColor), new Rectangle(x, y, pixelsPerModule, pixelsPerModule));
                    }
                    else
                        gfx.FillRectangle(new SolidBrush(lightColor), new Rectangle(x, y, pixelsPerModule, pixelsPerModule));
                }
            }

            gfx.Save();
            return bmp;
        }

        private int ModulesPerSideFromVersion(int version)
        {
            return 21 + (version - 1) * 4;
        }

    }
}