using System;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public abstract class ObjectSortTitle
    {
        public string name;
        public int width;

        public abstract string GetValueStr(SangoObject obj);

        public abstract int Sort(SangoObject a, SangoObject b);
    }
}
