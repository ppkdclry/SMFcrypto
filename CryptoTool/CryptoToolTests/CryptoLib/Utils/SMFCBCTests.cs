using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTool.CryptoLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CryptoTool.CryptoLib.Utils.Tests
{
    [TestClass()]
    public class SMFCBCTests
    {
        [TestMethod()]
        public void SMFCBCTest()
        {
            SMFCBC smfcbcAlg = SMFCBC.Create();

            byte[] data = new byte[8];
            for(int i = 0;i < 8; i++)
            {
                data[i] = (byte)i;
            }
            string FileName = "D:\\github\\SMFTool\\SMFcrypto\\CryptoTool\\CryptoToolTests\\bin\\Debug\\output.txt";

            try
            {
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);
                SMFCBC smfcrypto = new SMFCBC();

                CryptoStream cStream = new CryptoStream(fStream,
                    smfcrypto.CreateEncryptor(smfcbcAlg.Key,smfcbcAlg.IV),
                    CryptoStreamMode.Write);

                BinaryWriter sWriter = new BinaryWriter(cStream);

                try
                {
                    sWriter.Write(data);
                    sWriter.Write(data);
                }
                catch(Exception e)
                {
                    Console.WriteLine("An error occurred: {0}", e.Message);
                }
                finally
                {
                    sWriter.Close();
                    cStream.Close();
                    fStream.Close();
                }

            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("A file error occurred: {0}", e.Message);
            }
        }

        [TestMethod()]
        public void CreateEncryptorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateEncryptorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateDecryptorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateDecryptorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GenerateIVTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GenerateKeyTest()
        {
            Assert.Fail();
        }
    }
}