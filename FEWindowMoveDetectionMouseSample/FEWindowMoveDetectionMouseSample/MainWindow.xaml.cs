namespace FEWindowMoveDetectionMouseSample
{
    public partial class MainWindow : System.Windows.Window
    {
        private FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection MouseMoveWindowDetection = new();
        private int Count = 0;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Loaded += MainWindow_Loaded;
                Closed += MainWindow_Closed;
                MouseMoveWindowDetection.StartMove += MouseMoveWindowDetection_StartMove;
                MouseMoveWindowDetection.Moved += MouseMoveWindowDetection_Moved;
                MouseMoveWindowDetection.StopMove += MouseMoveWindowDetection_StopMove;
            }
            catch
            {
            }
        }

        private void MainWindow_Loaded(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                MouseMoveWindowDetection.Start();
            }
            catch
            {
            }
        }

        private void MainWindow_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                MouseMoveWindowDetection.Stop();
            }
            catch
            {
            }
        }

        /// <summary>
        /// マウスでウィンドウ移動の「StartMove」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveWindowDetection_StartMove(
            object sender,
            FreeEcho.FEWindowMoveDetectionMouse.StartMoveEventArgs e
            )
        {
            try
            {
                EventTextBox.Text += Count + " : StartMove\n";
                EventTextBox.ScrollToEnd();
                Count++;
            }
            catch
            {
            }
        }

        /// <summary>
        /// マウスでウィンドウ移動の「Moved」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveWindowDetection_Moved(
            object sender,
            FreeEcho.FEWindowMoveDetectionMouse.MoveEventArgs e
            )
        {
            try
            {
                EventTextBox.Text += Count + " : Moved\n";
                EventTextBox.ScrollToEnd();
                Count++;
            }
            catch
            {
            }
        }

        /// <summary>
        /// マウスでウィンドウ移動の「StopMove」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMoveWindowDetection_StopMove(
            object sender,
            FreeEcho.FEWindowMoveDetectionMouse.StopMoveEventArgs e
            )
        {
            try
            {
                EventTextBox.Text += Count + " : StopMove\n";
                EventTextBox.ScrollToEnd();
                Count++;
            }
            catch
            {
            }
        }
    }
}
