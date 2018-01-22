using System;
using System.Security.Cryptography;

namespace CryptoTool.CryptoLib.Utils
{
    public class SmfDecryptTransform : ICryptoTransform
    {
        private byte[] smfIV;
        private byte[] smfKey;

        public SmfDecryptTransform(byte[] smfKey, byte[] smfIV)
        {
            this.smfKey = smfKey;
            this.smfIV = smfIV;
        }

        public bool CanReuseTransform
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanTransformMultipleBlocks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int InputBlockSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int OutputBlockSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }
    }
}