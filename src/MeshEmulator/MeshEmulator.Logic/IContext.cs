using System;
using System.Collections.Generic;
using System.Text;

namespace MeshEmulator.Logic
{
    public interface IContext<TContext> where TContext : IContext<TContext>
    {
        int CurrentTick { get; set; }

        IEnumerable<ITransmitterNode<TContext>> Nodes { get; }
    }
}
