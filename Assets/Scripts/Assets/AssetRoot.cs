using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Asset root", fileName = "Asset root")]
    public class AssetRoot: ScriptableObject
    {
        public SceneAsset UIScene;
        public List<LevelAsset> Levels;
    }
}