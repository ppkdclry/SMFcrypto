using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            int total = 0;
            byte[] data = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                data[i] = (byte)i;
            }
            string FileName = "D:\\github\\SMFTool\\SMFcrypto\\CryptoTool\\CryptoToolTests\\bin\\Debug\\output.txt";

            try
            {
                FileStream fStream = File.Open(FileName, FileMode.Create);
                SMFCBC smfcrypto = new SMFCBC();

                CryptoStream cStream = new CryptoStream(fStream,
                    smfcrypto.CreateEncryptor(smfcbcAlg.Key, smfcbcAlg.IV),
                    CryptoStreamMode.Write);

                BinaryWriter sWriter = new BinaryWriter(cStream);

                try
                {
                    sWriter.Write(data);
                    sWriter.Write(data);
                    total += data.Length;
                    total += data.Length;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: {0}", e.Message);
                }
                finally
                {
                    sWriter.Close();
                    cStream.Close();
                    fStream.Close();
                    smfcrypto.Clear();
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

            byte[] res;
            try
            {
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);
                SMFCBC smfcrypto = new SMFCBC();

                CryptoStream cStream = new CryptoStream(fStream,
                    smfcrypto.CreateDecryptor(smfcbcAlg.Key, smfcbcAlg.IV),
                    CryptoStreamMode.Read);

                BinaryReader sReader = new BinaryReader(cStream);

                byte[] resc = new byte[256];
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        res = sReader.ReadBytes(256);
                        Array.Copy(res, 0, resc, 0, res.Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: {0}", e.Message);
                }
                finally
                {
                    sReader.Close();
                    cStream.Close();
                    fStream.Close();
                    smfcrypto.Clear();
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
            smfcbcAlg.Clear();

            return;
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