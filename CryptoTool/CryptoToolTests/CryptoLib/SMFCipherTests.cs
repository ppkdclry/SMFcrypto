using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTool.CryptoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTool.CryptoLib.Tests
{
    [TestClass()]
    public class SMFCipherTests
    {
        [TestMethod()]
        public void encryptFileTest()
        {
            SMFCipher smf = SMFCipher.GetInstance();
            smf.encryptFile(@"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\test.txt", @"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\test.enc", "lry");
            smf.decryptFile(@"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\test.enc", @"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\testRecover.txt", "lry");

            smf.encryptFile(@"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\timg.jpg", @"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\timg.jpg.enc", "kkapsuemc");
            smf.decryptFile(@"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\timg.jpg.enc", @"D:\github\SMFTool\SMFcrypto\CryptoTool\CryptoToolTests\bin\Debug\timgRecover.jpg", "kkapsuemc");

        }
    }
}