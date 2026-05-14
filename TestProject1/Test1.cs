namespace TestProject1 {
    [TestClass]
    public sealed class Test1 {
        [TestMethod]
        public void GenerateKeys() {
            var keys = Core.generate_keys();
            Assert.IsTrue(keys.e >= 2 && keys.d >= 2 && keys.n >= 77);
        }
        [TestMethod]
        public void EncryptDecrypt() {
            var keys = Core.generate_keys();
            var text = "я прогуливаю пары 1337 ВерхниЙ РегистР";
            Assert.AreEqual(Core.decrypt(Core.encrypt(text, 187 /* n */, 3 /* e */), 187, 27 /* d */), text);
            Assert.AreEqual(Core.decrypt(Core.encrypt(text, 209, 7), 209, 13), text);
        }
        [TestMethod]
        public void InvalidInput() {
            Assert.ThrowsException<Exception>(() => Core.encrypt(null, 0, 0));
            Assert.ThrowsException<Exception>(() => Core.encrypt("1", 1, 1));
            Assert.ThrowsException<Exception>(() => Core.encrypt("Z", 187, 3));
            Assert.ThrowsException<Exception>(() => Core.decrypt(null, 0, 0));
            Assert.ThrowsException<Exception>(() => Core.decrypt("1", 1, 1));
            Assert.ThrowsException<Exception>(() => Core.decrypt("Z", 187, 3));
        }
    }
}
