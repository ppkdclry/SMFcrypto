using System;
using System.Security.Cryptography;

namespace CryptoTool.CryptoLib.Utils
{
    public class SmfEncryptTransform : ICryptoTransform
    {
        private SMFCore cryptCore;

        public SmfEncryptTransform(byte[] smfKey, byte[] smfIV)
        {
            cryptCore = new SMFCore(smfKey, smfIV, true);
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
            //应该清除SMFCore加密核中的数据，包括SMFCore加密核函数占用的临时变量（需要转移临时变量位置），这里简化了
            return;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            return cryptCore.encryptBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            return cryptCore.encryptFinalBlock(inputBuffer, inputOffset, inputCount);
        }
    }
}