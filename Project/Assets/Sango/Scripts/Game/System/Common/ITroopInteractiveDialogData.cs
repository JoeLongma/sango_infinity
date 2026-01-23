using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game
{
    public interface ITroopInteractiveDialogData
    {
        
        public void Clear();
        public bool IsEmpty();
        public void SetContent(string content);

        public void SetSureAction(System.Action action);
    }
}
