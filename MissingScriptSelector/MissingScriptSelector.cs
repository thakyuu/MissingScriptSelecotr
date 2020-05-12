using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace thakyuu.MissingScriptSelector
{
    public class MissingScriptSelector : MonoBehaviour
    {
        [MenuItem("Tools/MissingScriptSelector/SelectAll")]
        public static void SelectMissingScript()
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            if (allObjects == null)
            {
                return;
            }

            List<Object> missingObjects =
                new List<Object>(
                    allObjects
                        .Where(obj => AssetDatabase.GetAssetOrScenePath(obj)
                            .Contains(".unity"))
                        .Where(obj => obj.GetComponents<Component>()
                            .Any(component => component == null))
                        .Select(obj => obj.gameObject)
                );

            if (missingObjects.Count > 0)
            {
                Selection.objects = missingObjects.ToArray();
                foreach (GameObject obj in missingObjects)
                {
                    string path = obj.name;
                    Transform parent = obj.transform.parent;
                    while (parent != null)
                    {
                        path = parent.gameObject.name + "/" + path;
                        parent = parent.parent;
                    }

                    Debug.Log(path, obj);
                }
            }
        }
    }
}