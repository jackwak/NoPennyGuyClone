using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public List<RectTransform> HandRectTransforms = new List<RectTransform>();
    public GameObject stepGO;
}
