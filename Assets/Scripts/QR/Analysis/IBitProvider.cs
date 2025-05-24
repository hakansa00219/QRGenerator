using System.Collections.Generic;

namespace QR.Analysis
{
    public interface IBitProvider
    {
        Queue<BitNode> BitQueue { get; }
    }
}