using System;
using System.Collections.Generic;

namespace Sango.Game
{
    public abstract class ObjectSortTitle
    {
        public string name;
        public int width;
        public int alignment;

        public abstract string GetValueStr(SangoObject obj);
        public abstract int Sort(SangoObject a, SangoObject b);

        public ObjectSortTitle SetAlignment(int a) {  this.alignment = a; return this; }
        public ObjectSortTitle SetWidth(int a) {  this.width = a; return this; }
        public ObjectSortTitle SetName(string a) {  this.name = a; return this; }
    }
}
