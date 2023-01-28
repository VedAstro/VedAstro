using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SwissEphNet
{

    /// <summary>
    /// File C
    /// </summary>
    public class CFile : IDisposable
    {
        private Stream _Stream;
        private Encoding _Encoding;
        private Decoder _Decoder;

        /// <summary>
        /// Create new C file access
        /// </summary>
        public CFile(Stream stream, Encoding encoding = null) {
            this._Stream = stream;
            EOF = _Stream == null;
            this._Encoding = encoding ?? Encoding.GetEncoding("Windows-1252");
            this._Decoder = _Encoding.GetDecoder();
        }

        /// <summary>
        /// Internal release resources
        /// </summary>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_Stream != null) {
                    _Stream.Dispose();
                    _Stream = null;
                }
                _Encoding = null;
            }
        }        

        /// <summary>
        /// Release resource
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        /// <summary>
        /// Seek the file
        /// </summary>
        public long Seek(long offset, SeekOrigin origin) {
            if (_Stream == null) return -1;
            _Stream.Seek(offset, origin);
            return 0;
        }

        /// <summary>
        /// Read a char or return -1 if end of file
        /// </summary>
        public int Read() {
            if (EOF) return -1;
            var res = _Stream.ReadByte();
            if (res < 0) {
                EOF = true;
            }
            return res;
        }

        /// <summary>
        /// Reaad an array of bytes
        /// </summary>
        public int Read(byte[] buff, int offset, int count) {
            if (buff == null || EOF) return 0;
            var res = _Stream.Read(buff, offset, count);
            if (res != count) EOF = true;
            return res;
        }

        /// <summary>
        /// Read an Byte
        /// </summary>
        public bool Read(ref Byte result) {
            var i = Read();
            if (i < 0) return false;
            result = (byte)i;
            return true;
        }

        /// <summary>
        /// Read an SByte
        /// </summary>
        public bool Read(ref SByte result) {
            var i = Read();
            if (i < 0) return false;
            result = (sbyte)i;
            return true;
        }

        /// <summary>
        /// Read an Int32
        /// </summary>
        public bool Read(ref Int32 result) {
            var buff = BitConverter.GetBytes((Int32)0);
            if (Read(buff, 0, buff.Length) != buff.Length)
                return false;
            result = BitConverter.ToInt32(buff, 0);
            return true;
        }

        /// <summary>
        /// Read an UInt32
        /// </summary>
        public bool Read(ref UInt32 result) {
            var buff = BitConverter.GetBytes((UInt32)0);
            if (Read(buff, 0, buff.Length) != buff.Length)
                return false;
            result = BitConverter.ToUInt32(buff, 0);
            return true;
        }

        /// <summary>
        /// Read an Double
        /// </summary>
        public bool Read(ref Double result) {
            var buff = BitConverter.GetBytes((Double)0);
            if (Read(buff, 0, buff.Length) != buff.Length)
                return false;
            result = BitConverter.ToDouble(buff, 0);
            return true;
        }

        /// <summary>
        /// Read an encoded char
        /// </summary>
        public bool Read(ref char result) {
            if (EOF) return false;
            byte[] buff = new byte[1];
            char[] chars = new char[4];
            int cnt, read = 0;
            do {
                int b = Read();
                if (b < 0) {
                    EOF = true;
                    if (read > 0) {
                        buff[0] = 0;
                        cnt = _Decoder.GetChars(buff, 0, 1, chars, 0, false);
                        result = chars[0];
                        return true;
                    }
                    return false;
                }
                buff[0] = (byte)b;
                cnt = _Decoder.GetChars(buff, 0, 1, chars, 0, false);
                if (cnt > 0) {
                    result = chars[0];
                    return true;
                }
                read++;
            } while (true);
        }

        /// <summary>
        /// Read a line of text
        /// </summary>
        public string ReadLine() {
            if (EOF) return null;
            List<byte> buff = new List<byte>();
            int rb, read = 0;
            while ((rb = Read()) >= 0) {
                byte c = (byte)rb;
                read++;
                // Check if end of line
                bool endOfLine = false;
                if (c == '\r') {
                    endOfLine = true;
                    rb = Read();
                    if (rb >= 0) {
                        c = (byte)rb;
                        read++;
                    } else {
                        break;
                    }
                }
                if (c == '\n') {
                    endOfLine = true;
                } else if (endOfLine) {
                    _Stream.Seek(-1, SeekOrigin.Current);
                }
                if (endOfLine) break;
                //
                buff.Add(c);
            }
            if (EOF && read == 0) return null;
            return _Encoding.GetString(buff.ToArray(), 0, buff.Count);
        }

        /// <summary>
        /// Read a string
        /// </summary>
        public bool ReadString(ref string s, int size) {
            var chars = ReadChars(size);
            if (chars == null) {
                s = null;
                return false;
            }
            s = new String(chars);
            return chars.Length == size;
        }

        /// <summary>
        /// Read an encoded char
        /// </summary>
        public Char ReadChar() {
            Char result = '\0';
            if (Read(ref result)) return result;
            return '\0';
        }

        /// <summary>
        /// Read an byte
        /// </summary>
        public byte ReadByte() {
            byte result = 0;
            if (Read(ref result)) return result;
            return 0;
        }

        /// <summary>
        /// Read an sbyte
        /// </summary>
        public sbyte ReadSByte() {
            sbyte result = 0;
            if (Read(ref result)) return result;
            return 0;
        }

        /// <summary>
        /// Read an Int32
        /// </summary>
        public Int32 ReadInt32() {
            int result = 0;
            if (Read(ref result)) return result;
            return 0;
        }

        /// <summary>
        /// Read an UInt32
        /// </summary>
        public UInt32 ReadUInt32() {
            uint result = 0;
            if (Read(ref result)) return result;
            return 0;
        }

        /// <summary>
        /// Read an Double
        /// </summary>
        public Double ReadDouble() {
            double result = 0;
            if (Read(ref result)) return result;
            return 0;
        }

        /// <summary>
        /// Read an array of chars
        /// </summary>
        public Char[] ReadChars(int count) {
            if (EOF) return null;
            var result = new List<Char>();
            Char c = '\0';
            for (int i = 0; i < count; i++) {
                if (!Read(ref c)) break;
                result.Add(c);
            }
            if (EOF && result.Count == 0) return null;
            return result.ToArray(); ;
        }

        /// <summary>
        /// Read an array of sbyte
        /// </summary>
        public sbyte[] ReadSBytes(int count) {
            if (EOF) return null;
            var result = new List<sbyte>();
            sbyte b = 0;
            for (int i = 0; i < count; i++) {
                if (!Read(ref b)) break;
                result.Add(b);
            }
            if (EOF && result.Count == 0) return null;
            return result.ToArray(); ;
        }

        /// <summary>
        /// Read an array of Int32
        /// </summary>
        public Int32[] ReadInt32s(int count) {
            var result = new Int32[count];
            var cnt = ReadInt32s(result, 0, count);
            if (EOF && cnt == 0) return null;
            return result.Take(cnt).ToArray();
        }

        /// <summary>
        /// Read an array of Int32
        /// </summary>
        public int ReadInt32s(Int32[] buff, int offset, int count) {
            Int32 b = 0, res = 0;
            for (int i = 0; i < count; i++) {
                if (!Read(ref b)) break;
                buff[offset + i] = b;
                res++;
            }
            return res;
        }

        /// <summary>
        /// Read an array of double
        /// </summary>
        public double[] ReadDoubles(int count) {
            var result = new Double[count];
            var cnt = ReadDoubles(result, 0, count);
            if (EOF && cnt == 0) return null;
            return result.Take(cnt).ToArray();
        }

        /// <summary>
        /// Read an array of double
        /// </summary>
        public int ReadDoubles(Double[] buff, int offset, int count) {
            Double b = 0; int res = 0;
            for (int i = 0; i < count; i++) {
                if (!Read(ref b)) break;
                buff[offset + i] = b;
                res++;
            }
            return res;
        }

        /// <summary>
        /// True if the file is at end of file
        /// </summary>
        public bool EOF { get; private set; }

        /// <summary>
        /// Position in the file
        /// </summary>
        public long Position { get { return _Stream != null ? _Stream.Position : 0; } }

        /// <summary>
        /// Length of the file
        /// </summary>
        public long Length { get { return _Stream != null ? _Stream.Length : 0; } }

    }

}
