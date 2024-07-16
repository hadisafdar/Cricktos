namespace HyyderWorks.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class TabBackButton : EventListenerMonoBehaviour
    {
        private Button button;


        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(GoBack);
            UpdateBackButtonState(true);
        }


        private void GoBack()
        {
            TabGroup.PreviousState();
            
        }

        protected override void SubscribeToEvents()
        {
            TabGroup.EndOfStackEvent += UpdateBackButtonState;
        }

        protected override void UnSubscribeEvents()
        {
            TabGroup.EndOfStackEvent -= UpdateBackButtonState;
            
        }


        private void UpdateBackButtonState(bool endofStack)
        {
           // button.interactable = !endofStack;
        }

    }

}