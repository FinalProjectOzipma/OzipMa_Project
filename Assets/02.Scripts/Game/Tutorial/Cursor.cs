using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    [SerializeField] private List<TutorialButton> rectTransforms = new List<TutorialButton>();
    [SerializeField] private float speed = 3f;
    int index = 0;

    Vector3 targetPos;

    public void Start()
    {
        RegisterTutorialButton();
    }

    private void RegisterTutorialButton()
    {
        rectTransforms[index].Setup(this);
        rectTransforms[index].onEnd += () =>
        {
            index++;
            if (index < rectTransforms.Count)
            {
                rectTransforms[index].Setup(this);
                targetPos = (rectTransforms[index].transform as RectTransform).parent.InverseTransformPoint(transform.position);
            }
        };
    }

    private void Update()
    {
        if (index >= rectTransforms.Count)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}