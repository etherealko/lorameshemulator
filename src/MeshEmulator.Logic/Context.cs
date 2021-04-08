using System;
using System.Collections.Generic;
using System.Text;

namespace MeshEmulator.Logic
{
    public class Context : IContext<Context>
    {
        public int CurrentTick { get; set; }

        public List<TransmitterNode> Nodes { get; }

        #region icontext explicit
        IEnumerable<ITransmitterNode<Context>> IContext<Context>.Nodes => Nodes;
        #endregion
    }
}
