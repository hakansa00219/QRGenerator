using System.Collections.Generic;
using QR.Scriptable;
using UnityEngine;

namespace QR
{
    public class DataEncoder
    {
        private readonly VersionData _versionData;
        private readonly Queue<BitNode> _bitQueue = new Queue<BitNode>();
        private readonly int _resolution;
        
        public Queue<BitNode> BitQueue => _bitQueue;
        
        public DataEncoder(VersionData versionData, int resolution)
        {
            _versionData = versionData;
            _resolution = resolution;

            Move startMove = Move.Start;
            _bitQueue.Enqueue(new BitNode(resolution - 1, resolution - 1));
            
            FindNextValidBit(resolution - 1, resolution - 1, startMove);
        }

        private void FindNextValidBit(int x, int y, Move lastMove)
        {
            if (x < 0 && y < 0) return;
            Debug.Log($"({x},{y})");
            switch (lastMove)
            {
                case Move.Start:
                case Move.TopRight:
                    if (x - 1 < 0) return;
                    
                    if (_versionData.BitMatrix[x - 1, y])
                    {
                        _bitQueue.Enqueue(new BitNode(x - 1, y));
                    }
                    lastMove = Move.LeftUp;
                    FindNextValidBit(x - 1, y, lastMove);
                    break;
                case Move.BottomRight:
                    if (x - 1 < 0) return;
                    if (_versionData.BitMatrix[x - 1, y])
                    {
                        _bitQueue.Enqueue(new BitNode(x - 1, y));
                    }
                    lastMove = Move.LeftDown;
                    FindNextValidBit(x - 1, y, lastMove);
                    break;
                case Move.LeftUp:
                    if (y - 1 < 0)
                    {
                        lastMove = Move.BottomRight;
                        FindNextValidBit(x - 1, y, lastMove);
                    }
                    else
                    {
                        if (_versionData.BitMatrix[x + 1, y - 1])
                        {
                            _bitQueue.Enqueue(new BitNode(x + 1, y - 1));
                        }
                        lastMove = Move.TopRight;
                        FindNextValidBit(x + 1, y - 1, lastMove);
                    }
                    break;
                case Move.LeftDown:
                    if (y + 1 > _resolution - 1)
                    {
                        lastMove = Move.TopRight;
                        FindNextValidBit(x - 1, y, lastMove);
                    }
                    else
                    {
                        if (_versionData.BitMatrix[x + 1, y + 1])
                        {
                            _bitQueue.Enqueue(new BitNode(x + 1, y + 1));
                        }
                        lastMove = Move.BottomRight;
                        FindNextValidBit(x + 1, y + 1, lastMove);
                    }
                    break;
            }
        }
       

        public class BitNode
        {
            public int X;
            public int Y;

            public BitNode(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private enum Move : byte
        {
            Start = 0,
            LeftDown = 1,
            LeftUp = 2,
            TopRight = 3,
            BottomRight = 4,
        }
    }
}