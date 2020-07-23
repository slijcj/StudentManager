using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GDIPlusDemo
{
    /// <summary>
    /// 系统按钮控制器，对其用参数所构建的窗体系统按钮的控制
    /// </summary>
    public class SystemButtonManager : IDisposable
    {
        #region Field

        private FormBase _owner;
        private SystemButton[] _systemButtons = new SystemButton[3];   // 0.Close 1.Max 2 Min

        private bool _mouseDown;

        #endregion

        #region Constructor

        public SystemButtonManager(FormBase owner)
        {
            this._owner = owner;
            IniSystemButtons();
        }

        #endregion

        #region Properties

        public SystemButton[] SystemButtonArray
        {
            get
            {
                return _systemButtons;
            }
        }

        //按钮状态索引器
        public ButtonStatus this[int buttonID]
        {
            get
            {
                return SystemButtonArray[buttonID].State;
            }
            set
            {
                if (SystemButtonArray[buttonID].State != value)
                {
                    SystemButtonArray[buttonID].State = value;
                    if (_owner != null)
                    {
                        Invalidate(SystemButtonArray[buttonID].LocationRect);
                    }
                }
            }
        }

        #endregion

        #region Public

        public void ProcessMouseOperate(Point mousePoint, MouseOperate operate)
        {
            switch (operate)
            {
                case MouseOperate.Move:
                    ProcessMouseMove(mousePoint);
                    break;
                case MouseOperate.Down:
                    ProcessMouseDown(mousePoint);
                    break;
                case MouseOperate.Up:
                    ProcessMouseUP(mousePoint);
                    break;
                case MouseOperate.Leave:
                    ProcessMouseLeave();
                    break;
                default:
                    break;
            }
        }

        public void DrawSystemButtons(Graphics g)
        {
            for (int index = 0; index < SystemButtonArray.Length; index++)
            {
                //当窗体没有此系统按钮时，不进行绘制
                if (SystemButtonArray[index].LocationRect == Rectangle.Empty)
                {
                    continue;
                }

                switch (this[index])
                {
                    case ButtonStatus.PressedLeave:
                    case ButtonStatus.Normal:
                        g.DrawImage(
                            SystemButtonArray[index].NormalImg,
                            SystemButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SystemButtonArray[index].NormalImg.Width, SystemButtonArray[index].NormalImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    case ButtonStatus.HighLight:
                        g.DrawImage(
                            SystemButtonArray[index].HighLightImg,
                            SystemButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SystemButtonArray[index].HighLightImg.Width, SystemButtonArray[index].HighLightImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    case ButtonStatus.Pressed:
                        g.DrawImage(
                            SystemButtonArray[index].PressedImg,
                            SystemButtonArray[index].LocationRect,
                            new Rectangle(0, 0, SystemButtonArray[index].PressedImg.Width, SystemButtonArray[index].PressedImg.Height),
                            GraphicsUnit.Pixel);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Private

        private void ProcessMouseMove(Point mousePoint)
        {
            string toolTip = string.Empty;
            bool hide = true;

            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                hide = false;  //显示提示文本
                if (!_mouseDown)
                {
                    if (this[index] != ButtonStatus.HighLight)
                    {
                        toolTip = SystemButtonArray[index].ToolTip;
                    }
                    this[index] = ButtonStatus.HighLight;
                }
                else
                {
                    if (this[index] == ButtonStatus.PressedLeave)
                    {
                        this[index] = ButtonStatus.Pressed;
                    }
                }

                //其他按钮的状态为 Normal
                for (int i = 0; i < SystemButtonArray.Length; i++)
                {
                    if (i != index)
                    {
                        this[i] = ButtonStatus.Normal;
                    }
                }
            }
            else
            {
                for (int i = 0; i < SystemButtonArray.Length; i++)
                {
                    if (!_mouseDown)
                    {
                        this[i] = ButtonStatus.Normal;
                    }
                    else
                    {
                        if (this[i] == ButtonStatus.Pressed)
                        {
                            this[i] = ButtonStatus.PressedLeave;
                        }
                    }
                }
            }

            if (toolTip != string.Empty)
            {
                HideToolTip();
                ShowTooTip(toolTip);
            }

            if (hide)
            {
                HideToolTip();
            }

        }

        private void ProcessMouseDown(Point mousePoint)
        {

            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                _mouseDown = true;
                this[index] = ButtonStatus.Pressed;
            }
            else
            {
                RenderHelper.MoveWindow(_owner);
            }
        }

        private void ProcessMouseUP(Point mousePoint)
        {
            _mouseDown = false;
            int index = SearchPointInRects(mousePoint);
            if (index != -1)
            {
                if (this[index] == ButtonStatus.Pressed)
                {
                    this[index] = ButtonStatus.Normal;

                    //handle event at there
                    SystemButtonArray[index].OnMouseDown(); 
                }
            }
            else
            {
                ProcessMouseLeave();
            }
        }

        private void ProcessMouseLeave()
        {
            for (int i = 0; i < SystemButtonArray.Length; i++)
            {
                if (this[i] == ButtonStatus.Pressed)
                {
                    this[i] = ButtonStatus.PressedLeave;
                }
                else
                { //所有按钮的状态为 Normal
                    this[i] = ButtonStatus.Normal;
                }
            }
        }

        private void Invalidate(Rectangle rect)
        {
            _owner.Invalidate(rect);
        }

        private void ShowTooTip(string toolTipText)
        {
            if (_owner != null)
            {
                _owner.ToolTip.Active = true;
                _owner.ToolTip.SetToolTip(_owner, toolTipText);
            }
        }

        private void HideToolTip()
        {
            if (_owner != null)
            {
                _owner.ToolTip.Active = false;
            }
        }

        /// <summary>
        /// 判断鼠标点是否在系统按钮矩形内
        /// </summary>
        /// <param name="mousePoint">鼠标坐标点</param>
        /// <returns>如果存在，返回系统按钮索引，否则返回 -1</returns>
        private int SearchPointInRects(Point mousePoint)
        {
            bool isFind = false;
            int index = 0;
            foreach (SystemButton button in SystemButtonArray)
            {
                if (button.LocationRect != Rectangle.Empty && button.LocationRect.Contains(mousePoint))
                {
                    isFind = true;
                    break;
                }
                index++;
            }
            if (isFind)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }

        #endregion

        #region SystemButton Initialization

        private void IniSystemButtons()
        {
            bool isShowMaxButton = _owner.MaximizeBox;
            bool isShowMinButton = _owner.MinimizeBox;

            //Colse
            SystemButton closeBtn = new SystemButton();
            SystemButtonArray[0] = closeBtn;
            closeBtn.ToolTip = "关闭";
            closeBtn.NormalImg = Image.FromFile(@".\Res\SystemButton\close_normal.png");
            closeBtn.HighLightImg = Image.FromFile(@".\Res\SystemButton\close_highlight.png");
            closeBtn.PressedImg = Image.FromFile(@".\Res\SystemButton\close_press.png");
            closeBtn.LocationRect = new Rectangle(
                _owner.Width - closeBtn.NormalImg.Width,
                -1,
                closeBtn.NormalImg.Width,
                closeBtn.NormalImg.Height);
            //注册事件
            closeBtn.OnMouseDownEvent += new MouseDownEventHandler(this.CloseButtonEvent);


            //Max
            SystemButton MaxBtn = new SystemButton();
            SystemButtonArray[1] = MaxBtn;
            MaxBtn.ToolTip = "最大化";
            if (isShowMaxButton)
            {
                MaxBtn.NormalImg = Image.FromFile(@".\Res\SystemButton\max_normal.png");
                MaxBtn.HighLightImg = Image.FromFile(@".\Res\SystemButton\max_highlight.png");
                MaxBtn.PressedImg = Image.FromFile(@".\Res\SystemButton\max_press.png");
                MaxBtn.OnMouseDownEvent += new MouseDownEventHandler(this.MaxButtonEvent);
                MaxBtn.LocationRect = new Rectangle(
                    closeBtn.LocationRect.X - MaxBtn.NormalImg.Width,
                    -1,
                    MaxBtn.NormalImg.Width,
                    MaxBtn.NormalImg.Height);
            }
            else
            {
                MaxBtn.LocationRect = Rectangle.Empty;
            }

            //Min
            SystemButton minBtn = new SystemButton();
            SystemButtonArray[2] = minBtn;
            minBtn.ToolTip = "最小化";
            if (!isShowMinButton)
            {
                minBtn.LocationRect = Rectangle.Empty;
                return;
            }

            minBtn.NormalImg = Image.FromFile(@".\Res\SystemButton\min_normal.png");
            minBtn.HighLightImg = Image.FromFile(@".\Res\SystemButton\min_highlight.png");
            minBtn.PressedImg = Image.FromFile(@".\Res\SystemButton\min_press.png");
            minBtn.OnMouseDownEvent += new MouseDownEventHandler(this.MinButtonEvent);
            if (isShowMaxButton)
            {
                minBtn.LocationRect = new Rectangle(
                    MaxBtn.LocationRect.X - minBtn.NormalImg.Width,
                    -1,
                    minBtn.NormalImg.Width,
                    minBtn.NormalImg.Height);
            }
            else
            {
                minBtn.LocationRect = new Rectangle(
                   closeBtn.LocationRect.X - minBtn.NormalImg.Width,
                   -1,
                   minBtn.NormalImg.Width,
                   minBtn.NormalImg.Height);
            }
        }

        private void CloseButtonEvent()
        {
            _owner.Close();
        }

        private void MaxButtonEvent()
        {
            if (_owner.WindowState == FormWindowState.Maximized)
            {
                _owner.WindowState = FormWindowState.Normal;
            }
            else
            {
                _owner.WindowState = FormWindowState.Maximized;
            }
        }

        private void MinButtonEvent()
        {
            _owner.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _owner = null;
        }

        #endregion
    }
}
