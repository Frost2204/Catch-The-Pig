using System.Diagnostics;
using UnityEngine;

namespace Utils
{
    public class LoggerUtility
    {
        public static void LogInEditor(object message, UnityEngine.Color color = default)
        {
#if UNITY_EDITOR
            StackFrame frame = new(1, true);

            var methodName = frame.GetMethod().ToString();
            string[] words = methodName.Split(' ');
            methodName = words[^1].Split('(')[0];

            var fileName = frame.GetFileName().ToString();
            string[] fileNamesArray = fileName.Split('\\');
            fileName = fileNamesArray[^1].Split('.')[0];

            if (color == default)
            {
                color = UnityEngine.Color.white;
            }

            UnityEngine.Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{fileName}/{methodName}]: {message}</color>");
#endif
        }

        public static void Log(object message)
        {
            StackFrame frame = new(1, true);

            var methodName = frame.GetMethod().ToString();
            string[] words = methodName.Split(' ');
            methodName = words[^1].Split('(')[0];


            var fileName = frame.GetFileName().ToString();
            string[] fileNamesArray = fileName.Split('\\');
            fileName = fileNamesArray[^1].Split('.')[0];

            UnityEngine.Debug.Log($"[{fileName}/{methodName}]: {message}");
        }
    }

}