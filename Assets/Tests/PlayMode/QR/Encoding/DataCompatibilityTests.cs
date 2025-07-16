using NUnit.Framework;
using QR.Encoding;

namespace Tests.PlayMode.QR.Encoding
{
    public class DataCompatibilityTests
    {
        #region Numeric Tests
        [Test]
        public void Numeric_AllCharacters_ShouldReturnTrue()
        {
            string data = "0123456789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Numeric);
        }
        [Test]
        public void Numeric_Repeated_ShouldReturnTrue()
        {
            string data = "11111111111111111111111111111111111111";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Numeric);
        }
        
        // Only numeric should return true for empty data.
        [Test] 
        public void Numeric_Empty_ShouldReturnTrue()
        {
            string data = "";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_OnlyAlphanumeric_ShouldReturnFalse()
        {
            string data = "APRIL";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        [Test]
        public void Numeric_OnlyKanji_ShouldReturnFalse()
        {
            string data = "電気";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_OnlyByte_ShouldReturnFalse()
        {
            string data = "testing compatibility!";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_WithAlphanumeric_ShouldReturnFalse()
        {
            string data = "012345APRIL6789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_WithKanji_ShouldReturnFalse()
        {
            string data = "012345電気6789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_WithByte_ShouldReturnFalse()
        {
            string data = "012345tests6789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        
        [Test]
        public void Numeric_AllMixed_ShouldReturnFalse()
        {
            string data = "012345tests6789APRIL電気!*";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Numeric);
        }
        #endregion
        
        #region Alphanumeric Tests
        [Test]
        public void Alphanumeric_AllCharacters_ShouldReturnTrue()
        {
            string data = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Alphanumeric);
        }
        [Test]
        public void Alphanumeric_Repeated_ShouldReturnTrue()
        {
            string data = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_Empty_ShouldReturnFalse()
        {
            string data = "";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_OnlyNumeric_ShouldReturnTrue()
        {
            string data = "0123456789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            // Since alphanumeric includes numeric, this should return true.
            Assert.IsTrue(result == Compatibility.Numeric);
        }
        [Test]
        public void Alphanumeric_OnlyKanji_ShouldReturnFalse()
        {
            string data = "電気";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_OnlyByte_ShouldReturnFalse()
        {
            string data = "testing compatibility!";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_WithNumeric_ShouldReturnTrue()
        {
            string data = "012345APRIL6789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_WithKanji_ShouldReturnFalse()
        {
            string data = "012345電気6789ABCD";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_WithByte_ShouldReturnFalse()
        {
            string data = "ABCD012345tests6789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        
        [Test]
        public void Alphanumeric_AllMixed_ShouldReturnFalse()
        {
            string data = "012345tests6789APRIL電気!*";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Alphanumeric);
        }
        #endregion
        
        #region Kanji Tests
        
        [Test]
        public void Kanji_Basic_ShouldReturnTrue()
        {
            string data = "電気が止まったので散歩に行きました。";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Kanji);
        }
        [Test]
        public void Kanji_Repeated_ShouldReturnTrue()
        {
            string data = "電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電電";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_Empty_ShouldReturnFalse()
        {
            string data = "";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_OnlyNumeric_ShouldReturnFalse()
        {
            string data = "0123456789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        [Test]
        public void Kanji_OnlyAlphanumeric_ShouldReturnFalse()
        {
            string data = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_OnlyByte_ShouldReturnFalse()
        {
            string data = "testing compatibility!";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_WithNumeric_ShouldReturnFalse()
        {
            string data = "012345678電気9";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_WithAlphanumeric_ShouldReturnFalse()
        {
            string data = "012345電気6789ABCD";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_WithByte_ShouldReturnFalse()
        {
            string data = "電気が止まったのでinterrupt散歩に行きました。";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        
        [Test]
        public void Kanji_AllMixed_ShouldReturnFalse()
        {
            string data = "012345tests6789APRIL電気!*";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Kanji);
        }
        #endregion
        
        #region Byte Tests
        
        [Test]
        public void Byte_Basic_ShouldReturnTrue()
        {
            string data = "testing compatibility!";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        [Test]
        public void Byte_Repeated_ShouldReturnTrue()
        {
            string data = "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_Empty_ShouldReturnFalse()
        {
            string data = "";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_OnlyNumeric_ShouldReturnFalse()
        {
            string data = "0123456789";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Byte);
        }
        [Test]
        public void Byte_OnlyAlphanumeric_ShouldReturnFalse()
        {
            string data = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_OnlyKanji_ShouldReturnFalse()
        {
            string data = "電気";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsFalse(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_WithNumeric_ShouldReturnTrue()
        {
            string data = "012345678testing compatibility9";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_WithAlphanumeric_ShouldReturnTrue()
        {
            string data = "012345testing compatibility6789ABCD";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_WithKanji_ShouldReturnTrue()
        {
            string data = "電気が止まったのでinterrupting kanji散歩に行きました。";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        
        [Test]
        public void Byte_AllMixed_ShouldReturnTrue()
        {
            string data = "012345tests6789APRIL電気!*";
            IDataCompatibilityChecker compatibilityChecker = new DataCompatibilityService();
            var result = compatibilityChecker.CheckCompatibility(data);
            
            Assert.IsTrue(result == Compatibility.Byte);
        }
        #endregion
    }
}