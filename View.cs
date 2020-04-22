using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab_2
{
    class View
    {
        Bitmap textureImage;
        int VBOtexture;
        public int min
        {
            get; set;
        }
        public int max
        {
            get; set;
        }

        public void DrawQuadStrips(Bin bin, int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int x = 0; x < bin.x; x++)
            {
                GL.Begin(BeginMode.QuadStrip);

                int y = 0;

                int value = bin.array[x + y * bin.x + layerNumber * bin.x * bin.y];
                GL.Color3(TransferFunction(value));
                GL.Vertex2(x, y);

                value = bin.array[x + 1 + y * bin.x + layerNumber * bin.x * bin.y];
                GL.Color3(TransferFunction(value));
                GL.Vertex2(x + 1, y);

                value = bin.array[x + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                GL.Color3(TransferFunction(value));
                GL.Vertex2(x, y + 1);

                value = bin.array[x + 1 + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                GL.Color3(TransferFunction(value));
                GL.Vertex2(x + 1, y + 1);


                for ( ; y < bin.y; y++)
                {
                    value = bin.array[x + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y + 1);

                    value = bin.array[x + 1 + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y + 1);
                }

                GL.End();
            }
        }

        public void Load2DTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
            BitmapData bitmap_data = textureImage.LockBits(
                new System.Drawing.Rectangle(0, 0, textureImage.Width, textureImage.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap_data.Width, bitmap_data.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);

            textureImage.UnlockBits(bitmap_data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        }

        public void generateTextureImage(Bin bin, int layerNumber)
        {
            textureImage = new Bitmap(bin.x, bin.y);

            for (int i = 0; i < bin.x; i++)
            {
                for (int j = 0; j < bin.y; j++)
                {
                    int pixelNumber = i + j * bin.x + layerNumber * bin.x * bin.y;
                    textureImage.SetPixel(i, j, TransferFunction(bin.array[pixelNumber]));
                }
            }
        }

        public void DrawTexture(Bin bin)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.White);

            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0, 0.0);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(0.0, bin.y);

            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(bin.x, bin.y);

            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(bin.x, 0.0);

            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }

        public void SetupView(Bin bin, int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, bin.x, 0.0f, bin.y, -1.0f, 1.0f);
            GL.Viewport(0, 0, width, height);
        }

        Color TransferFunction(int value)
        {
            int new_val = Clamp((int)((value - min) * 255 / (max - min)), 0, 255);

            return Color.FromArgb(255, new_val, new_val, new_val);
        }

        public void DrawQuads(Bin bin, int layerNumber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(BeginMode.Quads);
            for (int x = 0; x < bin.x - 1; x++)
            {
                for (int y = 0; y < bin.y - 1; y++)
                {
                    int value = bin.array[x + y * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y);

                    value = bin.array[x + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x, y + 1);

                    value = bin.array[x + 1 + (y + 1) * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y + 1);

                    value = bin.array[x + 1 + y * bin.x + layerNumber * bin.x * bin.y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x + 1, y);
                }
            }
            GL.End();
        }

        private int Clamp(int value, int min, int max)
        {
            if (min > value)
            {
                return min;
            }
            else if (max < value)
            {
                return max;
            }

            return value;
        }
    }
}
