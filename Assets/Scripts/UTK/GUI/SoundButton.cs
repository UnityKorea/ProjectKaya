using UnityEngine;

namespace UTK.GUI
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Text mText;
        [SerializeField] private AudioSource mAudio;
        
        

        private void Start()
        {
            RefreshLabel();
        }

        private void RefreshLabel()
        {
            if (mText && mAudio)
            {
                mText.text = mAudio.isPlaying ? "Sound Off" : "Sound On";
            }
        }

        public void PressButton()
        {
            if (mAudio)
            {
                if (mAudio.isPlaying)
                {
                    mAudio.Stop();
                }
                else
                {
                    mAudio.Play();
                }
                RefreshLabel();
            }
        }
    }
    
}
