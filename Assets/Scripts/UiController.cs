using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace HanoiTowerVisualization
{
    public class UiController : MonoBehaviour
    {
        public GameController GameController;
        public GameObject PlayWindow;
        public Slider Slider;
        public Text MinCount;
        public Text MaxCount;
        public Text CurrentCount;
        

        // change from slider
        public void OnTorusCountChange(float count)
        {
            CurrentCount.text = count.ToString(CultureInfo.CurrentCulture);
        }

        // click on button
        public void Play()
        {
            PlayWindow.SetActive(false);
            GameController.StartVisualization((int)Slider.value);
        }
        
        
        private void Start()
        {
            MinCount.text = Slider.minValue.ToString(CultureInfo.CurrentCulture);
            MaxCount.text = Slider.maxValue.ToString(CultureInfo.CurrentCulture);
            CurrentCount.text = Slider.value.ToString(CultureInfo.CurrentCulture);
            GameController.OnVisualizationComplete += OnVisualizationComplete;
        }

        private void OnVisualizationComplete()
        {
            PlayWindow.SetActive(true);
        }
    }
}