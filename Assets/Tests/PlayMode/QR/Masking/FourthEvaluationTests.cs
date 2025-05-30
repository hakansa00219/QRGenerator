using NUnit.Framework;
using QR.Masking;
using UnityEngine;

namespace Tests.PlayMode.QR.Masking
{
    public class FourthEvaluationTests
    {
        [Test]
        public void Black62Percent_ShouldReturn20()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, true, true, true, true, true },
                { false, true, true, false, true, true, false, true, true, false },
                { true, true, true, true, false, true, false, true, false, true },
                { false, true, true, false, false, true, true, true, true, false },
                { false, true, true, false, true, false, true, false, true, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 20);
        }
        
        [Test]
        public void Black58Percent_ShouldReturn10()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, true, true, true, true, true },
                { false, true, true, false, true, true, false, false, false, false },
                { true, true, true, true, false, true, false, true, false, true },
                { false, true, true, false, false, true, true, true, true, false },
                { false, true, true, false, true, false, true, false, true, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 10);
        }
        
        [Test]
        public void Black54Percent_ShouldReturn0()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, true, true, true, true, true },
                { false, true, true, false, true, true, false, false, false, false },
                { true, false, true, true, false, true, false, true, false, true },
                { false, true, true, false, false, true, true, true, true, false },
                { false, false, true, false, true, false, true, false, true, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 0);
        }
        [Test]
        public void Black46Percent_ShouldReturn0()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, false, false, true, true, true },
                { false, true, true, false, true, true, false, false, false, false },
                { true, false, true, true, false, true, false, true, false, true },
                { false, true, false, false, false, true, false, true, true, false },
                { false, false, true, false, true, false, true, false, true, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 0);
        }
        
        [Test]
        public void Black42Percent_ShouldReturn10()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, false, false, true, true, true },
                { false, true, true, false, true, true, false, false, false, false },
                { true, false, true, true, false, true, false, true, false, true },
                { false, true, false, false, false, true, false, true, true, false },
                { false, false, false, false, true, false, true, false, false, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 10);
        }
        
        [Test]
        public void Black38Percent_ShouldReturn20()
        {
            bool[,] bits = new bool[5, 10]
            {
                { true, false, false, true, false, false, false, true, true, true },
                { false, true, true, false, false, true, false, false, false, false },
                { true, false, true, true, false, true, false, true, false, true },
                { false, false, false, false, false, true, false, true, true, false },
                { false, false, false, false, true, false, true, false, false, false }
            };
            
            Evaluation fourthEvaluation = new FourthEvaluation();
            
            int penalty = 0;
            penalty += fourthEvaluation.Calculation(bits, bits.GetLength(0), bits.GetLength(1));
            
            Assert.IsTrue(penalty == 20);
        }
    }
}