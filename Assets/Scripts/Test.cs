using UnityEngine;

public class Test : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;
    float NewPos = 0f;
    public bool SwipeLeft;
    public bool SwipeRight;
    public float XValue;
    private CharacterController m_char;
    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewPos = -XValue;
                m_Side = SIDE.Left;
            } else if (m_Side == SIDE.Right)
            {
                NewPos = 0;
                m_Side = SIDE.Mid;
            }

        }
        else if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewPos = XValue;
                m_Side = SIDE.Right;
            }
            else if (m_Side == SIDE.Left)
            {
                NewPos = 0;
                m_Side = SIDE.Mid;
            }

        }
        m_char.Move((NewPos-transform.position.x) * Vector3.right);
    }
}
