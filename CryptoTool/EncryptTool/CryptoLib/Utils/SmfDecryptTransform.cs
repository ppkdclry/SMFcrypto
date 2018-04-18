using System;
using System.Security.Cryptography;

namespace EncryptTool.CryptoLib.Utils
{
    public class SmfDecryptTransform : ICryptoTransform
    {
        private SMFCore decryptCore;

        public SmfDecryptTransform(byte[] smfKey, byte[] smfIV)
        {
            decryptCore = new SMFCore(smfKey, smfIV, false);
        }

        public bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        public int InputBlockSize
        {
            get
            {
                return 16;
            }
        }

        public int OutputBlockSize
        {
            get
            {
                return 16;
            }
        }

        public void Dispose()
        {
            return;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            return decryptCore.decryptBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            return new byte[0];

        }
    }
}