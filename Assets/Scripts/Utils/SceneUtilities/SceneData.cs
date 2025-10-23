using System.Collections.Generic;
public enum Scene
{
    MainMenu,
    GameScene,
}
public class SceneData
{
    public static Dictionary<int, string> SceneDataDict = new()
    {
    { 0, "MainMenu"},
    { 1, "GameScene"},
    };
}
