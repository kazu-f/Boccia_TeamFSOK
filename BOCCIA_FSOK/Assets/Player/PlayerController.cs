using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerController : MonoBehaviour
    {
        public TouchManager TouchManager;
        public GameObject ball;
        Vector2 touchStartPos = new Vector2(0.0f,0.0f);     //�G��n�߂����W�B
        Vector2 touchEndPos = new Vector2(0.0f,0.0f);       //�����؂������W�B
        Vector2 endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        Vector2 touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (TouchManager.IsTouch())
            {
                if(TouchManager.GetPhase() != TouchPhase.Stationary)
                {
                    touchPosInScreen = TouchManager.GetTouchPos();
                    touchPosInScreen.x = (touchPosInScreen.x - Screen.width / 2) / Screen.width;
                    touchPosInScreen.y = touchPosInScreen.y / Screen.height;
                }

                if (TouchManager.GetPhase() == TouchPhase.Began)
                {
                    touchStartPos = touchPosInScreen;

                }
                else if(TouchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = TouchManager.GetDeltaPos();
                    deltaMoveVec.y = deltaMoveVec.y / Screen.height;
                    //������Ƀt���b�N���Ă��Ȃ���΁B
                    if(deltaMoveVec.y < 0.05f
                        && touchPosInScreen.y < touchStartPos.y)
                    {
                        //�ړ�������̍��W�B
                        touchEndPos = touchPosInScreen;
                    }
                }
                else if(TouchManager.GetPhase() == TouchPhase.Ended)
                {
                    if(touchStartPos.y < touchPosInScreen.y)
                    {
                        endToStart = touchStartPos - touchEndPos;
                        var ballPos = this.transform.position;
                        ballPos.y = 2.0f * touchStartPos.y;
                        var obj = Instantiate(ball, ballPos, this.transform.rotation);
                        Rigidbody ballRB = obj.GetComponent<Rigidbody>();
                        Vector3 vec = new Vector3(0.0f, 10.0f, 500.0f * Mathf.Max(endToStart.y,0.1f));
                        ballRB.AddForce(vec);
                    }
                }
            }

        }
    }
}
