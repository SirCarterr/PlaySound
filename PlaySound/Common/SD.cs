﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaySound.Common
{
    public static class SD
    {
        public static readonly Dictionary<string, ModifierKeys> hotkeysDictionary1 = new Dictionary<string, ModifierKeys> { { "None", ModifierKeys.None }, { "Shift", ModifierKeys.Shift }, { "Ctrl", ModifierKeys.Control }, { "Alt", ModifierKeys.Alt } };

        public static readonly Dictionary<string, Key> hotkeysDictionary2 = new Dictionary<string, Key> { { "None", Key.None }, { "Backspace", Key.Back }, { "Delete", Key.Delete }, { "Tab", Key.Tab }, { "Capital", Key.Capital }, { "Clear", Key.Clear }, { "Return", Key.Return }, { "Escape", Key.Escape }, { "Space", Key.Space }, { "Up", Key.Up }, { "Down", Key.Down }, { "Right", Key.Right }, { "Left", Key.Left }, { "Insert", Key.Insert }, { "Home", Key.Home }, { "End", Key.End }, { "PageUp", Key.PageUp }, { "PageDown", Key.PageDown }, { "F1", Key.F1 }, { "F2", Key.F2 }, { "F3", Key.F3 }, { "F4", Key.F4 }, { "F5", Key.F5 }, { "F6", Key.F6 }, { "F7", Key.F7 }, { "F8", Key.F8 }, { "F9", Key.F9 }, { "F10", Key.F10 }, { "F11", Key.F11 }, { "F12", Key.F12 }, { "F13", Key.F13 }, { "F14", Key.F14 }, { "F15", Key.F15 }, { "0", Key.D0 }, { "1", Key.D1 }, { "2", Key.D2 }, { "3", Key.D3 }, { "4", Key.D4 }, { "5", Key.D5 }, { "6", Key.D6 }, { "7", Key.D7 }, { "8", Key.D8 }, { "9", Key.D9 }, { "A", Key.A }, { "B", Key.B }, { "C", Key.C }, { "D", Key.D }, { "E", Key.E }, { "F", Key.F }, { "G", Key.G }, { "H", Key.H }, { "I", Key.I }, { "J", Key.J }, { "K", Key.K }, { "L", Key.L }, { "M", Key.M }, { "N", Key.N }, { "O", Key.O }, { "P", Key.P }, { "Q", Key.Q }, { "R", Key.R }, { "S", Key.S }, { "T", Key.T }, { "U", Key.U }, { "V", Key.V }, { "W", Key.W }, { "X", Key.X }, { "Y", Key.Y }, { "Z", Key.Z }, { "NumLock", Key.NumLock }, { "NumPad0", Key.NumPad0 }, { "NumPad1", Key.NumPad1 }, { "NumPad2", Key.NumPad2 }, { "NumPad3", Key.NumPad3 }, { "NumPad4", Key.NumPad4 }, { "NumPad5", Key.NumPad5 }, { "NumPad6", Key.NumPad6 }, { "NumPad7", Key.NumPad7 }, { "NumPad8", Key.NumPad8 }, { "NumPad9", Key.NumPad9 }, { "Scroll", Key.Scroll }, { "OemAttn", Key.OemAttn }, { "OemAuto", Key.OemAuto }, { "OemBackslash", Key.OemBackslash }, { "OemClear", Key.OemClear }, { "OemCloseBrackets", Key.OemCloseBrackets }, { "OemComma", Key.OemComma }, { "OemCopy", Key.OemCopy }, { "OemEnlw", Key.OemEnlw }, { "OemFinish", Key.OemFinish }, { "OemMinus", Key.OemMinus }, { "OemOpenBrackets", Key.OemOpenBrackets }, { "OemPeriod", Key.OemPeriod }, { "OemPipe", Key.OemPipe }, { "OemPlus", Key.OemPlus }, { "OemQuestion", Key.OemQuestion }, { "OemQuotes", Key.OemQuotes }, { "OemSemicolon", Key.OemSemicolon }, { "OemTilde", Key.OemTilde } };
    }
}
