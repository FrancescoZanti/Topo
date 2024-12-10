using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Drawing;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Topo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private Timer _timer;
        private bool _toggle;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public MainWindow()
        {
            this.InitializeComponent();
            SetWindowSize(300, 200);
            InitializeMouseJiggler();
        }

        private void SetWindowSize(int width, int height)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = width, Height = height });
        }

        private void InitializeMouseJiggler()
        {
            _timer = new Timer(5000); // Intervallo di 5 secondi
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            GetCursorPos(out POINT currentPos);
            if (_toggle)
            {
                SetCursorPos(currentPos.X + 150, currentPos.Y);
            }
            else
            {
                SetCursorPos(currentPos.X - 150, currentPos.Y);
            }
            _toggle = !_toggle;

            // Aggiorna il TextBlock con l'orario corrente
            DispatcherQueue.TryEnqueue(() =>
            {
                timerTextBlock.Text = $"Ultimo movimento: {DateTime.Now:HH:mm:ss}";
            });
        }
    }
}
