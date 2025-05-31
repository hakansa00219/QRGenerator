using System.Collections.Generic;
using QR.Scriptable;

namespace QR.Analysis
{
    public class DataAnalyzer : IBitProvider
    {
        private readonly VersionData _versionData;
        private readonly Queue<BitNode> _bitQueue = new Queue<BitNode>();
        private readonly int _resolution;
        private (int, int) _recordedPoint;
        
        public Queue<BitNode> BitQueue => _bitQueue;
        public int RemainingBitCount => _bitQueue.Count;

        // Analyzer for QR code to get all valid queued bits and positions.
        public DataAnalyzer(VersionData versionData, int resolution)
        {
            _versionData = versionData;
            _resolution = resolution;

            const Move startMove = Move.Start;
            _bitQueue.Enqueue(new BitNode(resolution - 1, resolution - 1));
            
            FindNextValidBit(resolution - 1, resolution - 1, startMove);
        }

        // Finding all valid bits.
        private void FindNextValidBit(int x, int y, Move lastMove)
        {
            if (IsThisBitOutsideOfBorders(x, y)) return;
            switch (lastMove)
            {
                case Move.Start:
                case Move.TopRight:
                    if (IsNextBitOutsideOfXBorder(x)) return;
                    if (CheckNextRightBit(x, y, ref lastMove, Move.TopRight)) return;
                    
                    EnqueueIfDataSlot(x - 1, y);
                    lastMove = Move.LeftUp;
                    FindNextValidBit(x - 1, y, lastMove);
                    break;
                case Move.BottomRight:
                    if (IsNextBitOutsideOfXBorder(x)) return;
                    if (CheckNextRightBit(x, y, ref lastMove, Move.BottomRight)) return;
                    
                    EnqueueIfDataSlot(x - 1, y);
                    lastMove = Move.LeftDown;
                    FindNextValidBit(x - 1, y, lastMove);
                    break;
                case Move.LeftUp:
                    if (y - 1 < 0)
                    {
                        _recordedPoint = (x - 1, 0);
                        lastMove = Move.BottomRight;
                        if (x > 0) EnqueueIfDataSlot(x - 1, y);
                        FindNextValidBit(x - 1, y, lastMove);
                    }
                    else
                    {
                        EnqueueIfDataSlot(x + 1, y - 1);
                        lastMove = Move.TopRight;
                        FindNextValidBit(x + 1, y - 1, lastMove);
                    }
                    break;
                case Move.LeftDown:
                    if (y + 1 > _resolution - 1)
                    {
                        _recordedPoint = (x - 1, _resolution - 1);
                        lastMove = Move.TopRight;
                        if (x > 0) EnqueueIfDataSlot(x - 1, y);
                        FindNextValidBit(x - 1, y, lastMove);
                    }
                    else
                    {
                        EnqueueIfDataSlot(x + 1, y + 1);
                        lastMove = Move.BottomRight;
                        FindNextValidBit(x + 1, y + 1, lastMove);
                    }
                    break;
            }
        }

        private void EnqueueIfDataSlot(int x, int y)
        {
            if (_versionData.BitMatrix[x, y])
            {
                _bitQueue.Enqueue(new BitNode(x, y));
            }
        }
        private bool CheckNextRightBit(int x, int y, ref Move lastMove, Move nextMove)
        {
            if (_versionData.BitMatrix[x, y] || !_versionData.BitMatrix[x - 1, y]) return false;
            
            //Skip this column
            lastMove = nextMove;
            FindNextValidBit(_recordedPoint.Item1 - 1, _recordedPoint.Item2, lastMove);
            return true;
        }
        private static bool IsThisBitOutsideOfBorders(int x, int y) => x < 0 && y < 0;
        private static bool IsNextBitOutsideOfXBorder(int x) => x - 1 < 0;
        
        
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