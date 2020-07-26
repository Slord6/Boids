using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// by WARdd - https://answers.unity.com/questions/1077124/how-to-make-cursor-wrap-around-the-screen-edge-lik.html
/// </summary>
public class CursorWrap : MonoBehaviour
{

    [System.Serializable]
    public class WrapEvent : UnityEvent<Vector2> { }
    public WrapEvent OnWrap;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    public class Win32
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }

    private void Update()
    {
        Vector2 mp = Input.mousePosition;
        Vector2 screen = new Vector2(Screen.width, Screen.height);
        int d = 1;

        int wrapHorizontal = 0;
        if (mp.x <= d)
            wrapHorizontal = 1;
        else if (mp.x >= screen.x - d)
            wrapHorizontal = -1;

        int wrapVertical = 0;
        if (mp.y <= d)
            wrapVertical = -1;
        else if (mp.y >= screen.y - d)
            wrapVertical = 1;

        if (wrapHorizontal != 0 || wrapVertical != 0)
        {
            Win32.GetCursorPos(out Win32.POINT p);
            Vector2 delta = new Vector2(wrapHorizontal, wrapVertical);
            delta.Scale(screen - d * Vector2.one);

            Win32.SetCursorPos(p.X + (int)delta.x, p.Y + (int)delta.y);

            delta.Scale(new Vector2(1f, -1f));
            OnWrap.Invoke(delta);
        }

    }

#endif

}