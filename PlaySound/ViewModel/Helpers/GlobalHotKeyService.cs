using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace PlaySound.ViewModel.Helpers
{
    public class GlobalHotKeyService : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private readonly IntPtr hWnd;
        private readonly Dictionary<int, Action> hotkeyActions = new Dictionary<int, Action>();
        private readonly Dictionary<int, int> hotkeyIds = new Dictionary<int, int>();

        public GlobalHotKeyService()
        {
            hWnd = IntPtr.Zero;
            ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessage;
        }

        public void RegisterHotkey(ModifierKeys modifierKey, Key key, Action hotkeyAction)
        {
            var keyModifier = (int)modifierKey;
            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            var hotkeyId = new Random().Next(0, 100000);
            if (RegisterHotKey(hWnd, hotkeyId, keyModifier, virtualKey))
            {
                hotkeyActions.Add(hotkeyId, hotkeyAction);
                hotkeyIds.Add(hotkeyId, virtualKey);
                //hotkeyActions[hotkeyId] = hotkeyAction;
                //hotkeyIds[hotkeyId] = virtualKey;
            }
        }

        public void UnregisterAllHotkeys()
        {
            foreach (var hotkeyId in hotkeyActions.Keys)
            {
                UnregisterHotKey(hWnd, hotkeyId);
            }
            hotkeyActions.Clear();
            hotkeyIds.Clear();
        }

        public void Dispose()
        {
            ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessage;
            foreach (var hotkeyId in hotkeyActions.Keys)
            {
                UnregisterHotKey(hWnd, hotkeyId);
            }
        }

        private void ThreadPreprocessMessage(ref System.Windows.Interop.MSG msg, ref bool handled)
        {
            if (msg.message == WM_HOTKEY && hotkeyActions.TryGetValue((int)msg.wParam, out var action))
            {
                action?.Invoke();
                handled = true;
            }
        }

        #region Windows API

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        #endregion
    }
}
