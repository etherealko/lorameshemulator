using System;
using System.Collections.Generic;
using System.Text;

namespace MeshEmulator.Logic
{
    public interface ITransmitterNode<TContext> where TContext : IContext<TContext>
    {
        void ProcessTransmit(TContext ctx);
        void ProcessReceive(TContext ctx);
    }
}
