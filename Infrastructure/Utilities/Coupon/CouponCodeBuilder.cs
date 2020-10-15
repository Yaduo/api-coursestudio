using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CourseStudio.Lib.Utilities.Coupon
{
    public class CouponCodeBuilder
    {
		public string[] BadWordsList { 
			get {
				return new string[]{"SHPX", "PHAG", "JNAX", "JNAT", "CVFF", "PBPX", "FUVG", "GJNG", "SNEG", "URYY", "ZHSS", "QVPX", "XABO", "NEFR", "FUNT", "FUCK", "GBFF", "FYHG", "GHEQ", "FYNT", "PENC", "CBBC", "OHGG", "SRPX", "OBBO", "WVFZ", "WVMM", "CUNG"};
			} 
		}

		private readonly Dictionary<char, int> symbolsDictionary = new Dictionary<char, int>();

		private readonly RandomNumberGenerator randomNumberGenerator;

		private char[] symbols;

        public CouponCodeBuilder()
        {
			this.SetupSymbolsDictionary();
			this.randomNumberGenerator = new SecureRandom();
        }

		public string Generate(Options opts)
        {
            var parts = new List<string>();

            // if  plaintext wasn't set then override
            if (string.IsNullOrEmpty(opts.Plaintext))
            {
                // not yet implemented
                opts.Plaintext = this.GetRandomPlaintext(8);
            }

            // generate parts and combine
            do
            {
                for (var i = 0; i < opts.Parts; i++)
                {
                    var sb = new StringBuilder();
                    for (var j = 0; j < opts.PartLength - 1; j++)
                    {
                        sb.Append(this.GetRandomSymbol());
                    }

                    var part = sb.ToString();
                    sb.Append(this.CheckDigitAlg1(part, i + 1));
                    parts.Add(sb.ToString());
                }
            }
            while (this.ContainsBadWord(string.Join(string.Empty, parts.ToArray())));

            return string.Join("-", parts.ToArray());
        }

		private void SetupSymbolsDictionary()
        {
            const string AvailableSymbols = "0123456789ABCDEFGHJKLMNPQRTUVWXY";
            this.symbols = AvailableSymbols.ToCharArray();
            for (var i = 0; i < this.symbols.Length; i++)
            {
                this.symbolsDictionary.Add(this.symbols[i], i);
            }
        }

		private string GetRandomPlaintext(int maxSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            this.randomNumberGenerator.GetNonZeroBytes(data);

            data = new byte[maxSize];
            this.randomNumberGenerator.GetNonZeroBytes(data);

            var result = new StringBuilder(maxSize);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }

		private char GetRandomSymbol()
        {
            var rng = new SecureRandom();
            var pos = rng.Next(this.symbols.Length);
            return this.symbols[pos];
        }

		private char CheckDigitAlg1(string data, int check)
        {
            // check's initial value is the part number (e.g. 3 or above)
            // loop through the data chars
            Array.ForEach(data.ToCharArray(), v => {
                var k = this.symbolsDictionary[v];
                check = (check * 19) + k;
            });

            return this.symbols[check % 31];
        }

		private bool ContainsBadWord(string code)
        {
            return this.BadWordsList.Any(t => code.ToUpper().IndexOf(t, StringComparison.Ordinal) > -1);
        }
    }
}
