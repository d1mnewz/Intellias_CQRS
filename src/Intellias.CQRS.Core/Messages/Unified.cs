﻿using System;

namespace Intellias.CQRS.Core.Messages
{
    /// <summary>
    /// Unified unique codes generator designed by Sergey Seletsky.
    /// </summary>
    public static class Unified
    {
        // FNV x64 Prime
        private const ulong Prime = 14695981039346656037U;

        // FNV x64 Offset
        private const ulong Offset = 1099511628211U;

        // Set of symbols used for numeric dimensions transcoding
        private static readonly char[] Symbols =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V'
        };

        /// <summary>
        /// Dummy.
        /// </summary>
        public static string Dummy => "0000000000";

        /// <summary>
        /// Get uniform virtual partition for unified key.
        /// </summary>
        /// <param name="unifiedCode">unified key.</param>
        /// <param name="count">partition count.</param>
        /// <returns>uniform virtual partition.</returns>
        public static string Partition(string unifiedCode, int count = 50)
        {
            var code = Decode(unifiedCode);
            var absoluteScalar = code - Offset;
            var partitionIndex = absoluteScalar / ((ulong.MaxValue - Prime) / (ulong)count);
            return $"{partitionIndex}".PadLeft(CountOfDigit(count), '0');
        }

        /// <summary>
        /// Generate x64 FNV hash based on random GUID.
        /// </summary>
        /// <param name="id">Source data.</param>
        /// <returns>Guid based FNV hash.</returns>
        public static ulong NewHash(Guid id)
        {
            return NewHash(id.ToByteArray());
        }

        /// <summary>
        /// Generate x64 FNV hash based on data bytes.
        /// </summary>
        /// <param name="bytes">Source data.</param>
        /// <returns>FNV hash.</returns>
        public static ulong NewHash(byte[] bytes)
        {
            var hash = Prime; // fnv prime

            foreach (var @byte in bytes)
            {
                hash ^= @byte;
                hash *= Offset; // fnv offset
            }

            return hash;
        }

        /// <summary>
        /// Generate x64 FNV code based on GUID.
        /// </summary>
        /// <param name="id">Source data.</param>
        /// <returns>Guid based FNV hash.</returns>
        public static string NewCode(Guid id) => NewCode(NewHash(id));

        /// <summary>
        /// Generate random x32 hex.
        /// </summary>
        /// <returns>x32 hex based on guid.</returns>
        public static string NewCode()
        {
            return NewCode(Guid.NewGuid());
        }

        /// <summary>
        /// Generate x32 hex from number.
        /// </summary>
        /// <param name="hash">hash.</param>
        /// <returns>String.</returns>
        public static string NewCode(ushort hash)
        {
            return NewCode(hash, 4);
        }

        /// <summary>
        /// Generate x32 hex from number.
        /// </summary>
        /// <param name="hash">hash.</param>
        /// <returns>String.</returns>
        public static string NewCode(short hash)
        {
            if (hash < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            return NewCode((ulong)hash, 3);
        }

        /// <summary>
        /// Generate x32 hex from number.
        /// </summary>
        /// <param name="hash">hash.</param>
        /// <returns>String.</returns>
        public static string NewCode(int hash)
        {
            if (hash < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            return NewCode((ulong)hash, 10);
        }

        /// <summary>
        /// Generate x32 hex from number.
        /// </summary>
        /// <param name="hash">hash.</param>
        /// <param name="length">length of code.</param>
        /// <returns>String.</returns>
        public static string NewCode(ulong hash, int length = 13)
        {
            var ch = new char[length--];
            for (var i = length; i >= 0; i--)
            {
                var inx = (byte)((uint)(hash >> (5 * i)) & 31);
                ch[length - i] = Symbols[inx];
            }

            return new string(ch);
        }

        /// <summary>
        /// Decode x32 hex to number.
        /// </summary>
        /// <param name="code">Unified code.</param>
        /// <returns>FNV hash.</returns>
        public static ulong Decode(string code)
        {
            const int shift = 5; // shift for x32 dimensions
            ulong hash = 0;
            for (var i = 0; i < code.Length; i++)
            {
                var index = (ulong)Array.IndexOf(Symbols, code[i]);
                var nuim = index << ((code.Length - 1 - i) * shift); // convert dimension to number and add
                hash += nuim;
            }

            return hash;
        }

        /// <summary>
        /// Return the number of digits that divides the number.
        /// </summary>
        /// <param name="number">Number.</param>
        /// <returns>Count.</returns>
        private static int CountOfDigit(int number)
        {
            int temp = number, count = 0;
            while (temp != 0)
            {
                // Fetching each digit of the number.
                var d = temp % 10;
                temp /= 10;

                // Checking if digit is greater than 0 and can divides n.
                if (d > 0 && number % d == 0)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
