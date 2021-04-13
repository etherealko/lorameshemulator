using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshEmulator.Logic
{
    public class Emulator<TContext> where TContext : IContext<TContext>
    {
        public TContext Context { get; private set; }

        public Emulator(TContext context) { Context = context; }

        public void ProcessNextTick()
        {
            ++Context.CurrentTick;

            var random = new Random();

            foreach (var node in Context.Nodes.OrderBy(n => random.Next()))
                node.ProcessTransmit(Context);
            foreach (var node in Context.Nodes)
                node.ProcessReceive(Context);
        }
    }
}
