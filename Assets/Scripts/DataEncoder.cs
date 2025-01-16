using System;
using System.Collections.Generic;
using QR.Enums;
using QR.Scriptable;

namespace QR
{
    public class DataEncoder
    {
        private readonly VersionData _versionData;
        private LastMove _lastMove = LastMove.None;
        public Queue<BitNode> bitQueue = new Queue<BitNode>();
        
        public DataEncoder(VersionData versionData, int resolution, int bitSize)
        {
            _versionData = versionData;

            BitNode lastNode = null;
            while (bitSize > 0)
            {
                if (FindNextValidBit(resolution - 1, 0, _lastMove, lastNode))
                {
                    bitSize--;
                };
                lastNode = 
            }
        }

        public bool FindNextValidBit(int x, int y, LastMove lastMove, BitNode lastNode)
        {
            switch (lastMove)
            {
                case LastMove.None:
                    if (_versionData.BitMatrix[x - 1, y])
                    {
                        bitQueue.Enqueue(new BitNode(x - 1, y));
                    }
                    
                    break;
                case LastMove.TopRight:
                    if (_versionData.BitMatrix[x - 1, y])
                    {
                        bitQueue.Enqueue(new BitNode(x - 1, y, lastNode));
                    }
                    break;
                case LastMove.Left:
                    if (_versionData.BitMatrix[x + 1, y - 1])
                    {
                        bitQueue.Enqueue(new BitNode(x + 1, y - 1, lastNode));
                    }
                    else
                    {
                        bitQueue.Enqueue(new BitNode(x, y - 1, lastNode));
                        bitQueue.Enqueue(new BitNode(x - 1, y - 1, lastNode));
                    }
                    break;
                case LastMove.BottomRight:
                    if (_versionData.BitMatrix[x - 1, y])
                    {
                        bitQueue.Enqueue(new BitNode(x - 1, y, lastNode));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lastMove), lastMove, null);
            }
        }

        public class BitNode
        {
            public int X;
            public int Y;
            public BitNode PreviousNode;

            public BitNode(int x, int y, BitNode previousNode = null)
            {
                X = x;
                Y = y;
            }
        }

        public enum LastMove : byte
        {
            None = 0,
            Left = 1,
            TopRight = 2,
            BottomRight = 3,
        }
    }
}