namespace FreeEcho
{
    namespace FEWindowMoveDetectionMouse
    {
        /// <summary>
        /// マウスでウィンドウ移動検知
        /// </summary>
        public class MouseMoveWindowDetection
        {
            /// <summary>
            /// フックを実行しているかの値 (実行していない「false」/実行している「true」)
            /// </summary>
            public bool IsHooking
            {
                get;
                private set;
            } = false;
            /// <summary>
            /// ウィンドウが移動中かの値 (移動中ではない「false」/移動中「true」)
            /// </summary>
            public bool MovingWindow
            {
                get;
                set;
            } = false;
            /// <summary>
            /// フックプロシージャのハンドル
            /// </summary>
            private System.IntPtr Handle;
            /// <summary>
            /// マウスの左ボタンが押されているかの値 (押されていない「false」/押されている「true」)
            /// </summary>
            private bool MouseButtonDown = false;
            /// <summary>
            /// 前回調べた時のウィンドウの位置
            /// </summary>
            private RECT PreviousWindowPosition;
            /// <summary>
            /// 移動しているウィンドウのハンドル
            /// </summary>
            private System.IntPtr MoveHwnd;

            /// <summary>
            /// マウスでウィンドウが移動された時のイベントのデリゲート
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public delegate void MovedEventHandler(
                object sender,
                MoveEventArgs e
                );
            /// <summary>
            /// マウスでウィンドウが移動された時のイベント
            /// </summary>
            public event MovedEventHandler Moved;
            /// <summary>
            /// マウスでウィンドウが移動された時のイベントを実行
            /// </summary>
            private void DoMoved()
            {
                Moved?.Invoke(this, new MoveEventArgs());
            }

            /// <summary>
            /// マウスでウィンドウ移動開始のイベントのデリゲート
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public delegate void StartMoveEventHandler(
                object sender,
                StartMoveEventArgs e
                );
            /// <summary>
            /// マウスでウィンドウ移動開始のイベント
            /// </summary>
            public event StartMoveEventHandler StartMove;
            /// <summary>
            /// マウスでウィンドウ移動開始のイベントを実行
            /// </summary>
            private void DoStartMove()
            {
                StartMove?.Invoke(this, new StartMoveEventArgs());
            }

            /// <summary>
            /// マウスでウィンドウ移動停止のイベントのデリゲート
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public delegate void StopMoveEventHandler(
                object sender,
                StopMoveEventArgs e
                );
            /// <summary>
            /// マウスでウィンドウ移動停止のイベント
            /// </summary>
            public event StopMoveEventHandler StopMove;
            /// <summary>
            /// マウスでウィンドウ移動停止のイベントを実行
            /// </summary>
            private void DoStopMove()
            {
                StopMove?.Invoke(this, new StopMoveEventArgs());
            }

            /// <summary>
            /// フックチェーンにインストールするフックプロシージャのイベント
            /// </summary>
            private event NativeMethodsDelegate.MouseHookCallback HookCallback;

            /// <summary>
            /// マウスでウィンドウ移動の検知を開始
            /// </summary>
            /// <exception cref="System.ComponentModel.Win32Exception">Win32 error.</exception>
            public void Start()
            {
                if (IsHooking == false)
                {
                    HookCallback = HookProcedure;
                    Handle = NativeMethods.SetWindowsHookEx(14, HookCallback, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetEntryAssembly().GetModules()[0]), 0);      // 14 = WH_MOUSE_LL
                    if (Handle == System.IntPtr.Zero)
                    {
                        throw new System.ComponentModel.Win32Exception();
                    }
                    IsHooking = true;
                }
            }

            /// <summary>
            /// マウスでウィンドウ移動の検知を停止
            /// </summary>
            public void Stop()
            {
                if (IsHooking)
                {
                    if (Handle != System.IntPtr.Zero)
                    {
                        IsHooking = false;

                        NativeMethods.UnhookWindowsHookEx(Handle);
                        Handle = System.IntPtr.Zero;
                        HookCallback -= HookProcedure;
                    }
                }
            }

            /// <summary>
            /// フックプロシージャ
            /// </summary>
            /// <param name="nCode">フックコード</param>
            /// <param name="msg">フックプロシージャに渡す値</param>
            /// <param name="s">フックプロシージャに渡す値</param>
            /// <returns>フックチェーン内の次のフックプロシージャの戻り値</returns>
            private System.IntPtr HookProcedure(
                int nCode,
                uint msg,
                ref MSLLHOOKSTRUCT s
                )
            {
                if (0 <= nCode)
                {
                    switch (msg)
                    {
                        case 0x0201:        // WM_LBUTTONDOWN
                            {
                                MouseButtonDown = true;
                                POINT cursorPoint;     // マウスカーソルの位置
                                NativeMethods.GetCursorPos(out cursorPoint);
                                System.IntPtr hwnd = NativeMethods.WindowFromPoint(cursorPoint);       // マウスカーソルの位置にあるウィンドウのハンドル
                                if (NativeMethods.GetWindowRect(hwnd, out RECT windowRect))
                                {
                                    MoveHwnd = hwnd;
                                    PreviousWindowPosition.Left = windowRect.Left;
                                    PreviousWindowPosition.Top = windowRect.Top;
                                    PreviousWindowPosition.Right = windowRect.Right - windowRect.Left;
                                    PreviousWindowPosition.Bottom = windowRect.Bottom - windowRect.Top;
                                }
                            }
                            break;
                        case 0x0202:        // WM_LBUTTONUP
                            MouseButtonDown = false;
                            if (MovingWindow)
                            {
                                MovingWindow = false;
                                DoStopMove();
                                MoveHwnd = System.IntPtr.Zero;
                                PreviousWindowPosition.Left = -1;
                                PreviousWindowPosition.Top = -1;
                                PreviousWindowPosition.Right = -1;
                                PreviousWindowPosition.Bottom = -1;
                            }
                            break;
                        case 0x0200:      // WM_MOUSEMOVE
                            if (MouseButtonDown)
                            {
                                if (NativeMethods.GetWindowRect(MoveHwnd, out RECT windowRect))
                                {
                                    if (MovingWindow == false)
                                    {
                                        if (((windowRect.Left != PreviousWindowPosition.Left) || (windowRect.Top != PreviousWindowPosition.Top)) && ((PreviousWindowPosition.Right == (windowRect.Right - windowRect.Left)) && (PreviousWindowPosition.Bottom == (windowRect.Bottom - windowRect.Top))))
                                        {
                                            MovingWindow = true;
                                            DoStartMove();
                                        }
                                    }
                                    if (MovingWindow)
                                    {
                                        DoMoved();
                                    }
                                }
                            }
                            break;
                    }
                }

                return (NativeMethods.CallNextHookEx(Handle, nCode, msg, ref s));
            }
        }
    }
}
