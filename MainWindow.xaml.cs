using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;

namespace RecastTimer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        //
        // HotKey登録関数
        //
        // 登録に失敗(他のアプリが使用中)の場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        private extern static int RegisterHotKey(IntPtr hWnd, int id, int modKey, int key) ;

        //
        // HotKey解除関数
        //
        // 解除に失敗した場合は、0が返却されます。
        //
        [DllImport("user32.dll")]
        private extern static int UnregisterHotKey(IntPtr HWnd, int ID) ;

        private const int HOTKEY_1 = 0x001;
        private const int HOTKEY_2 = 0x002;
        private const int HOTKEY_3 = 0x003;
        private const int HOTKEY_4 = 0x004;
        public TimeData Data1 { get; set; }
        public TimeData Data2 { get; set; }
        public TimeData Data3 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();
            if ((RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_1
                , (int)ModifierKeys.None, (int)KeyInterop.VirtualKeyFromKey(Key.ImeConvert)) == 0)
            || (RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_2
                , (int)ModifierKeys.Control, (int)KeyInterop.VirtualKeyFromKey(Key.ImeConvert)) == 0)
            || (RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_3
                , (int)ModifierKeys.Alt, (int)KeyInterop.VirtualKeyFromKey(Key.ImeConvert)) == 0)
            || (RegisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_4
                , (int)ModifierKeys.Shift, (int)KeyInterop.VirtualKeyFromKey(Key.ImeConvert)) == 0))
            {
                MessageBox.Show("既に他のアプリで使用されています。");
            }

            ComponentDispatcher.ThreadPreprocessMessage += HotKeyPressed;
            this.Data1 = new TimeData(60);
            this.Data2 = new TimeData(60);
            this.Data3 = new TimeData(120);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowSetting(object sender, RoutedEventArgs e)
        {
            //(new Setting()).ShowDialog();
            this.Data1.Reset();
            this.Data2.Reset();
            this.Data3.Reset();
        }

        private void Minimum(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_1);
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_2);
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_3);
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HOTKEY_4);
        }

        public void HotKeyPressed(ref MSG msg, ref bool handled)
        {
            if (msg.message == 0x0312)
            {
                if (msg.wParam.ToInt32() == HOTKEY_1)
                {
                    this.Data1.TimerStart();
                }

                if (msg.wParam.ToInt32() == HOTKEY_2)
                {
                    this.Data2.TimerStart();
                }

                if (msg.wParam.ToInt32() == HOTKEY_3)
                {
                    this.Data3.TimerStart();
                }

                if (msg.wParam.ToInt32() == HOTKEY_4)
                {
                    this.Data1.Reset();
                    this.Data2.Reset();
                    this.Data3.Reset();
                }
            }
        }
    }
}
