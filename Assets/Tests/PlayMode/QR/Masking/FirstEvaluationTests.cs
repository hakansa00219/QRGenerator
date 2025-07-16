using NUnit.Framework;
using QR.Logger;
using QR.Masking;

namespace Tests.PlayMode.QR.Masking
{
    public class FirstEvaluationTests
    {
        [Test]
        public void BlackPattern1x4_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 4]
            {
                { true, true, true, true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 4, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WhitePattern1x4_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 4]
            {
                { false, false, false, false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 4, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void BlackPattern4x1_ShouldReturn0()
        {
            bool[,] bits = new bool[4, 1]
            {
                { true },
                { true },
                { true },
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 4, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WhitePattern4x1_ShouldReturn0()
        {
            bool[,] bits = new bool[4, 1]
            {
                { false },
                { false },
                { false },
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 4, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void BlackPattern1x1_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 1]
            {
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WhitePattern1x1_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 1]
            {
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void BlackPattern4x4_ShouldReturn0()
        {
            bool[,] bits = new bool[4, 4]
            {
                { true,true,true,true, },
                { true,true,true,true, },
                { true,true,true,true, },
                { true,true,true,true, },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 4, 4, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WhitePattern4x4_ShouldReturn0()
        {
            bool[,] bits = new bool[4, 4]
            {
                { false,false,false,false, },
                { false,false,false,false, },
                { false,false,false,false, },
                { false,false,false,false, },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 4, 4, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void BlackPattern1x5_ShouldReturn3()
        {
            bool[,] bits = new bool[1, 5]
            {
                { true, true, true, true, true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 5, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void WhitePattern1x5_ShouldReturn3()
        {
            bool[,] bits = new bool[1, 5]
            {
                { false, false, false, false, false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 5, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void BlackPattern5x1_ShouldReturn3()
        {
            bool[,] bits = new bool[5, 1]
            {
                { true },
                { true },
                { true },
                { true },
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 5, 1, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void WhitePattern5x1_ShouldReturn3()
        {
            bool[,] bits = new bool[5, 1]
            {
                { false },
                { false },
                { false },
                { false },
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 5, 1, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void BlackPattern1x8_ShouldReturn6()
        {
            bool[,] bits = new bool[1, 8]
            {
                { true, true, true, true, true , true , true , true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 8, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void WhitePattern1x8_ShouldReturn6()
        {
            bool[,] bits = new bool[1, 8]
            {
                { false, false, false, false, false, false, false, false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 8, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void BlackPattern8x1_ShouldReturn6()
        {
            bool[,] bits = new bool[8, 1]
            {
                { true },
                { true },
                { true },
                { true },
                { true },
                { true },
                { true },
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void WhitePattern8x1_ShouldReturn6()
        {
            bool[,] bits = new bool[8, 1]
            {
                { false },
                { false },
                { false },
                { false },
                { false },
                { false },
                { false },
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 6);
            
        }
        [Test]
        public void Pattern8x1_OneWhite_ShouldReturn3()
        {
            bool[,] bits = new bool[8, 1]
            {
                { true },
                { true },
                { true },
                { true },
                { true },
                { false },
                { true },
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void Pattern8x1_OneBlack_ShouldReturn3()
        {
            bool[,] bits = new bool[8, 1]
            {
                { false },
                { false },
                { false },
                { false },
                { false },
                { true },
                { false },
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 3);
            
        }
        [Test]
        public void WrongPattern1x8_1_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 8]
            {
                { true, true, true, true, false , false , false , false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 8, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WrongPattern1x8_2_ShouldReturn0()
        {
            bool[,] bits = new bool[1, 8]
            {
                { true, true, false, false, true, true, false, false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 1, 8, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WrongPattern8x1_1_ShouldReturn0()
        {
            bool[,] bits = new bool[8, 1]
            {
                { false },
                { true },
                { false },
                { true },
                { false },
                { true },
                { false },
                { true },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void WrongPattern8x1_2_ShouldReturn0()
        {
            bool[,] bits = new bool[8, 1]
            {
                { false },
                { true },
                { true },
                { false },
                { false },
                { true },
                { true },
                { false },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation();
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 1, logger);
            
            Assert.IsTrue(penalty == 0);
            
        }
        [Test]
        public void Pattern5x5_ShouldReturn15()
        {
            bool[,] bits = new bool[5, 5]
            {
                { false,false,false,false,false, },
                { true,true,true,true,true, },
                { true,true,true,true,true, },
                { false,false,false,false,false, },
                { false,false,false,false,false, },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 5, 5, logger);
            
            Assert.IsTrue(penalty == 15);
            
        }
        [Test]
        public void Pattern5x5_ShouldReturn30()
        {
            bool[,] bits = new bool[5, 5]
            {
                { true,true,true,true,true, },
                { true,true,true,true,true, },
                { true,true,true,true,true, },
                { true,true,true,true,true, },
                { true,true,true,true,true, },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 5, 5, logger);
            
            Assert.IsTrue(penalty == 30);
            
        }
        [Test]
        public void Pattern8x8_ShouldReturn96()
        {
            bool[,] bits = new bool[8, 8]
            {
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
                { true,true,true,true,true,true,true,true, },
            };
            
            Evaluation firstEvaluation = new FirstEvaluation(); 
            ILogger logger = new Logger(null);
            
            int penalty = 0;
            penalty += firstEvaluation.Calculation(bits, 8, 8, logger);
            
            Assert.IsTrue(penalty == 96);
            
        }
    }
}