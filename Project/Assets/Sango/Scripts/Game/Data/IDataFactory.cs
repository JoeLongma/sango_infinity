using System;
using System.Collections;
using System.IO;
using TKNewtonsoft.Json;
using System.Xml;

namespace Sango.Game
{
    public interface IDataFactory : IDataObject
    {
        public IDataObject Create(BinaryReader node);
        public IDataObject Create(System.Xml.XmlNode node);
        public IDataObject Create(SimpleJSON.JSONNode node);

    }
}
