using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Utility
{
    public class MouseInput
    {
        public MouseKeys Key;
        public bool justPressed;

        public MouseInput() { }
        public MouseInput K(MouseKeys _key) {
            Key = _key;
            return this;
        }
        public MouseInput B(bool _justPressed) { 
            justPressed = _justPressed;
            return this;
        }
    }
}
