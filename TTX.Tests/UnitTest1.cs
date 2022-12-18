using TTX.Data.Shared.Helpers;

namespace TTX.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            short[] shorts = new short[short.MaxValue];
            for(short i = 0; i<short.MaxValue; i++)
            {
                shorts[i] = i;
            }
            Parallel.ForEach(shorts, (x) =>
            {
                var source = new MyClass1() { ASMO = x };
                var result = source.Extract<MyClass2>();
                Assert.AreEqual(source.ASMO, result.ASMO);
            });
        }
    }

    public class MyClass1
    {
        public short ASMO { get; set; } = 24;
        public string Snarky { get; set; } = "Snarky";

        public int Nola { get; set; } = 43;

        public string Trolla { get; set; } = "Shyra";

        public string Mincecraft { get; set; } = "Lynchcraft";
    }

    public class MyClass2
    {
        public short ASMO { get; set; } = 23;
        public string Barky { get; set; } = "Snarky";

        public string Nola { get; set; } = "Rodney";

        public int Trolla { get; set; } = 54;

        public string Doggo { get; set; } = "Lizard";
    }
}