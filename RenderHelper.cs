using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GDIPlusDemo
{
    /// <summary>
    /// 窗体自绘辅助类
    /// </summary>
    public class RenderHelper
    {
        /// <summary>
        /// 设置窗体的圆角矩形
        /// </summary>
        /// <param name="form">需要设置的窗体</param>
        /// <param name="rgnRadius">圆角矩形的半径</param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            int hRgn = 0;
            hRgn = Win32.CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            Win32.SetWindowRgn(form.Handle, hRgn, true);
            Win32.DeleteObject(hRgn);
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        public static void MoveWindow(Form form)
        {
            Win32.ReleaseCapture();
            Win32.SendMessage(form.Handle, Win32.WM_NCLBUTTONDOWN, Win32.HTCAPTION, 0);
        }

        /// <summary>
        /// 取低位 X 坐标
        /// </summary>
        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        /// <summary>
        /// 取高位 Y 坐标
        /// </summary>
        public static int HIWORD(int value)
        {
            return value >> 16;
        }

        /// <summary>
        /// 绘制窗体边框
        /// </summary>
        /// <param name="destForm">需要绘制边框的窗体</param>
        /// <param name="g">绘制边框所用的绘图对象</param>
        /// <param name="fringeImg">边框图片</param>
        /// <param name="radius">边框的圆角矩形</param>
        public static void DrawFormFringe(Form destForm, Graphics g, Image fringeImg, int radius)
        {
            DrawNineRect(
                g,
                fringeImg,
                new Rectangle(-radius, -radius, destForm.ClientSize.Width + 2 * radius, destForm.ClientSize.Height + 2 * radius),
                new Rectangle(0, 0, fringeImg.Width, fringeImg.Height));
        }

        /// <summary>
        /// 画九宫图
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="img">所需绘制的图片</param>
        /// <param name="DestRect">目标矩形</param>
        /// <param name="SrcRect">来源矩形</param>
        public static void DrawNineRect(Graphics g, Image img, Rectangle DestRect, Rectangle SrcRect)
        {
            int offset = 5;
            Rectangle NineRect = new Rectangle(img.Width / 2 - offset, img.Height / 2 - offset, 2 * offset, 2 * offset);
            int x = 0, y = 0, nWidth, nHeight;
            int xSrc = 0, ySrc = 0, nSrcWidth, nSrcHeight;
            int nDestWidth, nDestHeight;
            nDestWidth = DestRect.Width;
            nDestHeight = DestRect.Height;
            // 左上-------------------------------------;
            x = DestRect.Left;
            y = DestRect.Top;
            nWidth = NineRect.Left - SrcRect.Left;
            nHeight = NineRect.Top - SrcRect.Top;
            xSrc = SrcRect.Left;
            ySrc = SrcRect.Top;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nWidth, nHeight, GraphicsUnit.Pixel);
            // 上-------------------------------------;
            x = DestRect.Left + NineRect.Left - SrcRect.Left;
            nWidth = nDestWidth - nWidth - (SrcRect.Right - NineRect.Right);
            xSrc = NineRect.Left;
            nSrcWidth = NineRect.Right - NineRect.Left;
            nSrcHeight = NineRect.Top - SrcRect.Top;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nSrcWidth, nSrcHeight, GraphicsUnit.Pixel);
            // 右上-------------------------------------;
            x = DestRect.Right - (SrcRect.Right - NineRect.Right);
            nWidth = SrcRect.Right - NineRect.Right;
            xSrc = NineRect.Right;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nWidth, nHeight, GraphicsUnit.Pixel);
            // 左-------------------------------------;
            x = DestRect.Left;
            y = DestRect.Top + NineRect.Top - SrcRect.Top;
            nWidth = NineRect.Left - SrcRect.Left;
            nHeight = DestRect.Bottom - y - (SrcRect.Bottom - NineRect.Bottom);
            xSrc = SrcRect.Left;
            ySrc = NineRect.Top;
            nSrcWidth = NineRect.Left - SrcRect.Left;
            nSrcHeight = NineRect.Bottom - NineRect.Top;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nSrcWidth, nSrcHeight, GraphicsUnit.Pixel);
            // 中-------------------------------------;
            x = DestRect.Left + NineRect.Left - SrcRect.Left;
            nWidth = nDestWidth - nWidth - (SrcRect.Right - NineRect.Right);
            xSrc = NineRect.Left;
            nSrcWidth = NineRect.Right - NineRect.Left;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nSrcWidth, nSrcHeight, GraphicsUnit.Pixel);

            // 右-------------------------------------;
            x = DestRect.Right - (SrcRect.Right - NineRect.Right);
            nWidth = SrcRect.Right - NineRect.Right;
            xSrc = NineRect.Right;
            nSrcWidth = SrcRect.Right - NineRect.Right;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nSrcWidth, nSrcHeight, GraphicsUnit.Pixel);

            // 左下-------------------------------------;
            x = DestRect.Left;
            y = DestRect.Bottom - (SrcRect.Bottom - NineRect.Bottom);
            nWidth = NineRect.Left - SrcRect.Left;
            nHeight = SrcRect.Bottom - NineRect.Bottom;
            xSrc = SrcRect.Left;
            ySrc = NineRect.Bottom;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nWidth, nHeight, GraphicsUnit.Pixel);
            // 下-------------------------------------;
            x = DestRect.Left + NineRect.Left - SrcRect.Left;
            nWidth = nDestWidth - nWidth - (SrcRect.Right - NineRect.Right);
            xSrc = NineRect.Left;
            nSrcWidth = NineRect.Right - NineRect.Left;
            nSrcHeight = SrcRect.Bottom - NineRect.Bottom;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nSrcWidth, nSrcHeight, GraphicsUnit.Pixel);
            // 右下-------------------------------------;
            x = DestRect.Right - (SrcRect.Right - NineRect.Right);
            nWidth = SrcRect.Right - NineRect.Right;
            xSrc = NineRect.Right;
            g.DrawImage(img, new Rectangle(x, y, nWidth, nHeight), xSrc, ySrc, nWidth, nHeight, GraphicsUnit.Pixel);
        }


        /// <summary>
        /// 绘制窗体主体部分白色透明层
        /// </summary>
        /// <param name="form">需要绘制的窗体</param>
        /// <param name="g">绘图对象</param>
        public static void DrawFromAlphaMainPart(Form form, Graphics g)
        {
            Color[] colors = 
            {
                Color.FromArgb(5, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(150, Color.White),
                Color.FromArgb(180, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(5, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.05f,
                0.15f,
                0.85f,
                0.99f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
            }
        }
    }
}
