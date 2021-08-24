using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        public TouchManager touchManager;
        public GameObject bocciaPlayer;
        public GameObject ball;
        public GameObject ThrowGauge;
        RectTransform gaugeTransform;
        Material gaugeImageMat;
        Vector2 gaugeSize;
        Vector2 touchStartPos = new Vector2(0.0f, 0.0f);     //�G��n�߂����W�B
        Vector2 touchEndPos = new Vector2(0.0f, 0.0f);       //�����؂������W�B
        Vector2 endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        Vector2 touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)
        float throwPow = 0.0f;
        const float FLICK_POWER = 0.005f;     //�t���b�N����p�̒萔�B

        // Start is called before the first frame update
        void Start()
        {
            //�}�e���A���̎擾�B�B
            var image = ThrowGauge.GetComponent<Image>();
            gaugeImageMat = image.material;

            gaugeTransform = ThrowGauge.GetComponent<RectTransform>();
            gaugeSize.x = gaugeTransform.rect.width * gaugeTransform.localScale.x / Screen.width;
            gaugeSize.y = gaugeTransform.rect.height * gaugeTransform.localScale.y / Screen.height;
        }

        // Update is called once per frame
        void Update()
        {
            if (touchManager.IsTouch())
            {
                if (touchManager.GetPhase() != TouchPhase.Stationary)
                {
                    touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //������Ƀt���b�N���Ă��Ȃ���΁B
                    if (deltaMoveVec.y < FLICK_POWER
                        && touchPosInScreen.y < touchStartPos.y)
                    {
                        //�ړ�������̍��W�B
                        touchEndPos = touchPosInScreen;
                        endToStart = touchStartPos - touchEndPos;
                        throwPow = endToStart.y / gaugeSize.y;
                    }
                }
                else if (touchManager.GetPhase() == TouchPhase.Ended)
                {
                    if (touchStartPos.y < touchPosInScreen.y
                        && endToStart.y > 0.01f)
                    {
                        throwPow = endToStart.y / gaugeSize.y;
                        var ballPos = this.transform.position;
                        ballPos.y = 2.0f * touchStartPos.y;
                        var obj = Instantiate(ball, ballPos, this.transform.rotation);
                        Rigidbody ballRB = obj.GetComponent<Rigidbody>();

                        Vector3 vec = bocciaPlayer.transform.forward;       //�v���C���[�̑O���������B
                        vec.x *= 200.0f * Mathf.Min(1.0f, Mathf.Max(throwPow, 0.1f));
                        vec.z *= 200.0f * Mathf.Min(1.0f, Mathf.Max(throwPow, 0.1f));
                        vec.y = 10.0f;

                        ballRB.AddForce(vec);
                    }
                }
                gaugeImageMat.SetFloat("_ThrowPow", throwPow);
            }
        }

        public void ThrowBallEnable()
        {
            this.enabled = true;
            touchStartPos = touchManager.GetTouchPosInScreen();
            touchEndPos = touchManager.GetTouchPosInScreen();
            gaugeTransform.anchoredPosition = touchManager.GetTouchPos();
            throwPow = 0.0f;
        }

        public void ThrowBallDisable()
        {
            this.enabled = false;
        }
    }
}
