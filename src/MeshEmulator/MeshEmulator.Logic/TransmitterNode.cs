using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshEmulator.Logic
{
    public class TransmitterNode : ITransmitterNode<Context>
    {
        private readonly TransmitterParameters _parameters;
        private readonly Queue<Transmission> _transmitQueue = new Queue<Transmission>();
        private readonly List<TransmissionBlockSignal> _receiveBuffer = new List<TransmissionBlockSignal>();
        private readonly List<Transmission> _seenMessages = new List<Transmission>();

        private int _ticksFromLastReceivedBlock;

        private int _lastTransmissionId;
        private int _nextTransmitBlockNumber;

        public string Name { get; set; }
        public TransmitterPosition Position { get; set; }

        public List<TransmissionBlockSignal> ReceiveWindow { get; } = new List<TransmissionBlockSignal>();

        public TransmitterNode(TransmitterParameters parameters)
        {
            _parameters = parameters;
        }

        public void ProcessTransmit(Context ctx)
        {
            if (ReceiveWindow.Count > 0 || _ticksFromLastReceivedBlock == 0)
                return;

        }

        public void ProcessReceive(Context ctx)
        {
            if (_nextTransmitBlockNumber != 0)
                return;

            var signals = ReceiveWindow.OrderBy(s => s.SignalStrength).ToList();
            
            if (signals.Count == 0)
            {
                if (_receiveBuffer.Count != 0)
                {
                    var first = _receiveBuffer[0];

                    if (_receiveBuffer.Count == _parameters.TransmissionLengthTicks &&
                        _receiveBuffer.All(s => s.Transmission == first.Transmission) &&
                        _receiveBuffer.All(s => s.SignalSource == first.SignalSource))
                    {
                        if (!_seenMessages.Contains(first.Transmission))
                        {
                            _seenMessages.Add(first.Transmission);
                            _transmitQueue.Enqueue(first.Transmission);
                        }
                    }
                    else
                    {
                        _receiveBuffer.Clear();
                    }
                }

                ++_ticksFromLastReceivedBlock;
                return;
            }

            _ticksFromLastReceivedBlock = 0;

            if (signals.Count > 1 && signals[0].SignalStrength / signals[1].SignalStrength < _parameters.SnrCutoff)
                return;

            if (signals.Count != 0)
            {
                _receiveBuffer.Add(signals[0]);
                return;
            }    
        }

        public void RequestTransmit()
        {
            _transmitQueue.Enqueue(new Transmission { Source = this, Id = ++_lastTransmissionId });
        }
    }

    public class TransmitterParameters
    {
        public delegate double SignalStrengthFunction(double distance);

        public int TransmissionLengthTicks { get; set; } = 10;
        public double NoiseCutoff { get; set; } = 1;
        public double SnrCutoff { get; set; } = 10;
        public SignalStrengthFunction CalculateSignalStrength { get; set; } = d => 10/d/d;
    }

    public class TransmitterPosition
    {
        public double X { get; set; }
        public double Y { get; set; }

        public TransmitterPosition() { }

        public TransmitterPosition(double x, double y)
        {
            X = x; Y = y;
        }

        public double DistanceTo(TransmitterPosition other)
        {
            return Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y));
        }
    }

    public class TransmissionBlockSignal
    {
        public Transmission Transmission { get; set; }
        public double SignalStrength { get; set; }
        public TransmitterNode SignalSource { get; set; }
        public int BlockNumber { get; set; }
    }

    public class Transmission
    {
        public int Id { get; set; }

        public TransmitterNode Source { get; set; }
    }
}
