using System;

namespace WaveProject.UI.View
{
    internal interface IView
    {
        void Close(Action callback = null);
        void Open(Action callback = null);
        void Init();
    }
}