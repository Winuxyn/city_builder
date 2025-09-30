using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scroller
{
    public class ZoomScroller : MonoBehaviour
    {
        public GameObject scrollBar;
        float scrollPos = 0;
        public float pressTime;
        public float scrollSpeed;
        float[] pos;
        Button buttonD;
        public int repeatTimes;
        public bool startAutoScroll, scrollRight;
        [SerializeField] ScrollRect scrollRect;
        void Start()
        {
            StartCoroutine(ButtonPress());
            // scrollBar.GetComponent<Scrollbar>().value = 0f;
            // scrollPos = scrollBar.GetComponent<Scrollbar>().value;
        }

        void Update()
        {
            pos = new float[transform.childCount];
            float distance = 1f / (pos.Length - 1f);
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = distance * i;
            }
            if (Input.GetKeyUp(KeyCode.D) || scrollRight)
            {
                scrollRight = false;
                scrollPos = scrollBar.GetComponent<Scrollbar>().value + distance;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                scrollPos = scrollBar.GetComponent<Scrollbar>().value - distance;
            }

            if (Input.GetMouseButton(0))
            {
                scrollPos = scrollBar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                    {
                        scrollBar.GetComponent<Scrollbar>().value =
                        Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.005f);
                    }
                }
            }
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale =
                    Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for (int a = 0; a < pos.Length; a++)
                    {
                        if (a != i)
                        {
                            transform.GetChild(a).localScale =
                            Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.6f, 0.6f), 0.1f);
                        }
                    }
                }
            }

        }

        IEnumerator ButtonPress()
        {
            if (startAutoScroll)
            {
                for (int i = 0; i < repeatTimes; i++)
                {
                    yield return new WaitForSeconds(pressTime);
                    scrollRight = true;
                }
            }

        }


    }


}


