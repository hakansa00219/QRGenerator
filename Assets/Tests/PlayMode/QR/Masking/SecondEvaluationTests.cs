using NUnit.Framework;
using QR.Masking;
using QR.Logger;

namespace Tests.PlayMode.QR.Masking
{
    public class SecondEvaluationTests
    {
        [Test]
        public void Small2x2_AllBlack_ShouldReturn3()
        {
            bool[,] bits = new bool[2, 2]
            {
                { true, true },
                { true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 2, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void Small2x2_AllWhite_ShouldReturn3()
        {
            bool[,] bits = new bool[2, 2]
            {
                { false, false },
                { false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 2, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void Small2x3_AllBlack_ShouldReturn6()
        {
            bool[,] bits = new bool[2, 3]
            {
                { true, true, true },
                { true, true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 3, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Small2x3_AllWhite_ShouldReturn6()
        {
            bool[,] bits = new bool[2, 3]
            {
                { false, false, false },
                { false, false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 3, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Small3x2_AllBlack_ShouldReturn6()
        {
            bool[,] bits = new bool[3, 2]
            {
                { true, true },
                { true, true },
                { true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 3, 2, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Small3x2_AllWhite_ShouldReturn6()
        {
            bool[,] bits = new bool[3, 2]
            {
                { false, false },
                { false, false },
                { false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 3, 2, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Small3x3_AllBlack_ShouldReturn12()
        {
            bool[,] bits = new bool[3, 3]
            {
                { true, true, true },
                { true, true, true },
                { true, true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 3, 3, logger);
            
            Assert.IsTrue(penalty == 12);
            
        }
        [Test]
        public void Small3x3_AllWhite_ShouldReturn12()
        {
            bool[,] bits = new bool[3, 3]
            {
                { false, false, false },
                { false, false, false },
                { false, false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 3, 3, logger);
            
            Assert.IsTrue(penalty == 12);
            
        }
        [Test]
        public void Mid4x4_WhiteBlack_ShouldReturn9()
        {
            bool[,] bits = new bool[4, 4]
            {
                { false, false, true , true  },
                { false, false, false, false },
                { true , true , false, false },
                { true , true , true , true  },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 4, 4, logger);
            
            Assert.IsTrue(penalty == 9);
            
        }
        [Test]
        public void Mid4x4_WhiteInTheMiddle_ShouldReturn3()
        {
            bool[,] bits = new bool[4, 4]
            {
                { true, true, true , true  },
                { true, false, false, true },
                { true , false , false, true },
                { true , true , true , true  },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 4, 4, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void Mid2x4_WhiteBlack_ShouldReturn6()
        {
            bool[,] bits = new bool[2, 4]
            {
                { true, true, false, false },
                { true, true, false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 4, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Big8x8_AllBlack_ShouldReturn147()
        {
            bool[,] bits = new bool[8, 8]
            {
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
                { true, true, true, true, true, true, true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 8, 8, logger);
            
            Assert.IsTrue(penalty == 147);
            
        }
        [Test]
        public void Big8x8_AllWhite_ShouldReturn147()
        {
            bool[,] bits = new bool[8, 8]
            {
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
                { false, false, false, false, false, false, false, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 8, 8, logger);
            
            Assert.IsTrue(penalty == 147);
            
        }
        [Test]
        public void Big8x8_ShouldReturn0()
        {
            bool[,] bits = new bool[8, 8]
            {
                { true, false, false, true, false, false, true, false },
                { false, true, false, false, true, false, false, true },
                { false, false, true, false, false, true, false, false },
                { true, false, false, true, false, false, true, false },
                { false, true, false, false, true, false, false, true },
                { false, false, true, false, false, true, false, false },
                { true, false, false, true, false, false, true, false },
                { false, true, false, false, true, false, false, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 8, 8, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void Small2x2_ShouldReturn0()
        {
            bool[,] bits = new bool[2, 2]
            {
                { true, false },
                { true, false },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 2, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void Long2x12_ShouldReturn0()
        {
            bool[,] bits = new bool[2, 12]
            {
                { true, true, false, true, true, true, false, true, true, true, false, true },
                { false, true, true, true, false, true, true, true, false, true, true, true },
            };
            
            Evaluation secondEvaluation = new SecondEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += secondEvaluation.Calculation(bits, 2, 12, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
    }
}