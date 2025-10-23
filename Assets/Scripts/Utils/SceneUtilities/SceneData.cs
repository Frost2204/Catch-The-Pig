using System.Collections.Generic;
public enum Scene
{
    GameScene,
    MainMenu,
}
public class SceneData
{
    public static Dictionary<int, string> SceneDataDict = new()
    {
    { 0, "GameScene"},
    { 1, "MainMenu"},
    };
}
