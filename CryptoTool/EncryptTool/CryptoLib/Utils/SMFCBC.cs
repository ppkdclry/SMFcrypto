using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptTool.CryptoLib.Utils
{
    public class SMFCBC: SymmetricAlgorithm
    {
        public override byte[] Key { get; set; }
        public override byte[] IV { get; set; }

        public SMFCBC()
        {
            this.KeySizeValue = 128;
            this.BlockSizeValue = 128;
        }

        static public new SMFCBC Create()
        {
            SMFCBC res = new SMFCBC();
            res.GenerateIV();
            res.GenerateKey();
            return res;
        }

        public override ICryptoTransform CreateEncryptor(byte[] smfKey, byte[] smfIV)
        {
            return new SmfEncryptTransform(smfKey, smfIV);
        }

        public override ICryptoTransform CreateEncryptor()
        {
            return CreateEncryptor(this.Key, this.IV);
        }

        public override ICryptoTransform CreateDecryptor(byte[] smfKey, byte[] smfIV)
        {
            return new SmfDecryptTransform(smfKey, smfIV);
        }

        public override ICryptoTransform CreateDecryptor()
        {
            return CreateDecryptor(this.Key, this.IV);
        }

        public override void GenerateIV()
        {
            this.IV = GenerateRandomBytes(16);
        }

        public override void GenerateKey()
        {
            this.Key = GenerateRandomBytes(16);
        }

        static private RandomNumberGenerator rand = new RNGCryptoServiceProvider();
        static public byte[] GenerateRandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            rand.GetBytes(bytes);
            return bytes;
        }
    }
}
