using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using HoloLensSystemInformationCollector;
using System.Threading;

namespace HoloLensSystemInformationCollectorTest
{
    [TestClass]
    public class HoloLensSystemInformationCollectorUnitTest
    {
        [TestMethod]
        public void Test_f()
        {
            Assert.AreEqual(1, SystemInformation.f());
        }

        [TestMethod]
        public void Test_SystemInformation()
        {
            var output = SystemInformation.Get("http://169.254.49.246", eRest.ApiResourcemanagerSystemperf);

            Assert.AreEqual("", output);
        }
    }
}
