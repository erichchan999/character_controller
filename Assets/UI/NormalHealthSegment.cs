using System;
using UnityEngine.UIElements;

namespace UI {
    public class NormalHealthSegment : VisualElement {

        public new class UxmlFactory : UxmlFactory<NormalHealthSegment> {
            //...
        }

        public NormalHealthSegment() {
            m_Status = String.Empty;
        }

        private string m_Status;
        public string status { get; set; }
    }
}