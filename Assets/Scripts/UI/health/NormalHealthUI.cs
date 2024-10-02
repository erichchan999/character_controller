using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using Combat.Health;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI {
    public class NormalHealthUI : MonoBehaviour, IHealthBarUIDelegate {

        private const string _CONTAINER_NAME = "normal_health_container";
        private List<VisualElement> _healths;
        private HealthUIController _healthsController;
        private int _currentIdx;        // bounded by # healths

        private void DecrementCurrentIdx() {
            if (_currentIdx > 0) {
                _currentIdx--;
            }
        }

        private void IncrementCurrentIdx() {
            if (_currentIdx < _healths.Count - 1) {
                _currentIdx++;
            }
        }
        
        
        public void OnEnable() {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement container = root.Q<VisualElement>(_CONTAINER_NAME);

            if (container == null) {
                Debug.Log("ERROR: No container named " + _CONTAINER_NAME + " exists.");
            }

            _healths = new List<VisualElement>();
            foreach (VisualElement e in container.Children()) {
                // e.SetEnabled(false);
                _healths.Add(e);
            }

            _healthsController = new HealthUIController(this._healths);
        }

        public void UIPush() {
            Debug.Log("UI received pushed for normal health bar.");
            _healthsController.Increment();
        }

        public void UIPop() {
            Debug.Log("UI received popped from normal health bar.");
            _healthsController.Decrement();
        }


        private void RemoveHealth(VisualElement e) {
            e.style.opacity = 0.2f;
        }

        private void AddHealth(VisualElement e) {
            e.style.opacity = 1.0f;
        }
    }
    
    /**
     * Assumes visual element list has full health when constructed.
     */
    class HealthUIController {
        private List<VisualElement> veList;
        public HealthUIController(List<VisualElement> veList) {
            this.veList = veList;

            this._toEnableIdx = veList.Count - 1;
            this._toDisableIdx = veList.Count - 1;
        }

        private int _toEnableIdx;
        private int _toDisableIdx;

        public void Decrement() {
            Disable(veList[_toDisableIdx]);
            _toEnableIdx = _toDisableIdx;
            if (_toDisableIdx - 1 >= 0)
                _toDisableIdx--;
        }

        public void Increment() {
            Enable(veList[_toEnableIdx]);
            _toDisableIdx = _toEnableIdx;
            if (_toEnableIdx + 1 <= veList.Count - 1)
                _toEnableIdx++;
        }


        private void Disable(VisualElement ve) {
            ve.style.opacity = 0.3f;
        }

        private void Enable(VisualElement ve) {
            ve.style.opacity = 1f;
        }

    }
}