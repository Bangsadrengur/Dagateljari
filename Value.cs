#define NOCRYPTO
using System;
using System.IO;
using System.Text;
#if NETCF_1_0 || Smartphone
using System.Collections;
#else
using System.Collections.Generic;
#endif

namespace Transport {

	enum TypeCode {
		TYPE_NULL = -128,
		TYPE_INT8 = -127,
		TYPE_INT16 = -126,
		TYPE_INT32 = -125,
		TYPE_INT64 = -124,
		TYPE_STRING_LATIN1_8 = -123,
		TYPE_STRING_LATIN1_16 = -122,
		TYPE_STRING_LATIN1_32 = -121,
		TYPE_STRING_LATIN1_64 = -120,	// reserved
		TYPE_STRING_UCS2_8 = -119,
		TYPE_STRING_UCS2_16 = -118,
		TYPE_STRING_UCS2_32 = -117,
		TYPE_STRING_UCS2_64 = -116,	// reserved
		TYPE_STRING_UCS4_8 = -115,
		TYPE_STRING_UCS4_16 = -114,
		TYPE_STRING_UCS4_32 = -113,
		TYPE_STRING_UCS4_64 = -112,	// reserved
		TYPE_ARRAY_8 = -111,
		TYPE_ARRAY_16 = -110,
		TYPE_ARRAY_32 = -109,
		TYPE_ARRAY_64 = -108,	// reserved
		TYPE_TABLE_8 = -107,
		TYPE_TABLE_16 = -106,
		TYPE_TABLE_32 = -105,
		TYPE_TABLE_64 = -104,	// reserved
		TYPE_BINARY_8 = -103,
		TYPE_BINARY_16 = -102,
		TYPE_BINARY_32 = -101,
		TYPE_BINARY_64 = -100,	// reserved
		TYPE_DOUBLE = -99
	}

	/// <ignore>The Value Class</ignore>
	public abstract class Value
	{

		/// <title>Cast long to Value</title>
        /// <desc>
        /// <decl>public static implicit operator Value( long i );</decl>
        /// <purpose>To create a Value object containing a long value.</purpose>
        /// <use>v = (Value)x;</use>
        /// <use>v = x;</use>
        /// <pre>x is a long, v is a Value variable.</pre>
        /// <post>v contains a new object that has the value of x.</post>
        /// </desc>
        public static implicit operator Value( long i ) {
            return MakeLong(i);
        }

		/// <title>Cast double to Value</title>
        /// <desc>
        /// <decl>public static implicit operator Value( double d );</decl>
        /// <purpose>To create a Value object containing a double value.</purpose>
        /// <use>v = (Value)x;</use>
        /// <use>v = x;</use>
        /// <pre>x is a double, v is a Value variable.</pre>
        /// <post>v contains a new object that has the value of x.</post>
        /// </desc>
        public static implicit operator Value( double d ) {
            return MakeDouble(d);
        }

		/// <title>Cast string to Value</title>
        /// <desc>
        /// <decl>public static implicit operator Value( string s );</decl>
        /// <purpose>To create a Value object containing a string value.</purpose>
        /// <use>v = (Value)s;</use>
        /// <use>v = s;</use>
        /// <pre>s is a non-null string (may be the empty string, but not null), v is a Value variable.</pre>
        /// <post>v contains a new object that has the value of s.</post>
        /// </desc>
        public static implicit operator Value( string s ) {
            return MakeString(s);
        }

		/// <title>Cast byte array to Value</title>
        /// <desc>
        /// <decl>public static implicit operator Value( byte[] b );</decl>
        /// <purpose>To create a Value object containing a binary value.</purpose>
        /// <use>v = (Value)b;</use>
        /// <use>v = b;</use>
        /// <pre>b is a non-null byte array (may be an empty array, but not null), v is a Value variable.</pre>
        /// <post>v contains a new object that has the value of b.</post>
        /// </desc>
        public static implicit operator Value( byte[] b ) {
            return MakeBinary(b);
        }

		/// <title>Cast Value array to Value</title>
        /// <desc>
        /// <decl>public static implicit operator Value( Value[] b );</decl>
        /// <purpose>To create a Value object containing a Value array.</purpose>
        /// <use>v = (Value)a;</use>
        /// <use>v = a;</use>
        /// <pre>a is a non-null Value array (may be an empty array, but not null), v is a Value variable.</pre>
        /// <post>v contains a new object that has the value of a.</post>
        /// </desc>
        public static implicit operator Value( Value[] a ) {
            return MakeArray(a);
        }

		/// <title>Cast to long</title>
        /// <desc>
        /// <decl>public static explicit operator long( Value v );</decl>
        /// <purpose>To extract a long from a Value.</purpose>
        /// <use>x = (long)v;</use>
        /// <pre>v is a Value that contains a long.</pre>
        /// <post>x contains the long value of v.</post>
        /// </desc>
        public static explicit operator long( Value v ) {
            return v.AsLong;
        }

		/// <title>Cast to int</title>
        /// <desc>
        /// <decl>public static explicit operator int( Value v );</decl>
        /// <purpose>To extract an int from a Value.</purpose>
        /// <use>x = (int)v;</use>
        /// <pre>v is a Value that contains an int (actually a long).</pre>
        /// <post>x contains the int value of v.</post>
        /// </desc>
        public static explicit operator int(Value v)
        {
            return (int)v.AsLong;
        }

		/// <title>Cast to byte</title>
        /// <desc>
        /// <decl>public static explicit operator byte( Value v );</decl>
        /// <purpose>To extract a byte from a Value.</purpose>
        /// <use>x = (int)v;</use>
        /// <pre>v is a Value that contains a byte (actually a long).</pre>
        /// <post>x contains the byte value of v.</post>
        /// </desc>
        public static explicit operator byte(Value v)
        {
            return (byte)v.AsLong;
        }

		/// <title>Cast to short</title>
        /// <desc>
        /// <decl>public static explicit operator short( Value v );</decl>
        /// <purpose>To extract a short from a Value.</purpose>
        /// <use>x = (short)v;</use>
        /// <pre>v is a Value that contains a short (actually a long).</pre>
        /// <post>x contains the short value of v.</post>
        /// </desc>
        public static explicit operator short(Value v)
        {
            return (short)v.AsLong;
        }

		/// <title>Cast to string</title>
        /// <desc>
        /// <decl>public static explicit operator string( Value v );</decl>
        /// <purpose>To extract a string from a Value.</purpose>
        /// <use>x = (string)v;</use>
        /// <pre>v is a Value that contains a string.</pre>
        /// <post>x contains the string value of v.</post>
        /// </desc>
        public static explicit operator string(Value v)
        {
            return v.AsString;
        }

		/// <title>Cast to byte[]</title>
        /// <desc>
        /// <decl>public static explicit operator byte[]( Value v );</decl>
        /// <purpose>To extract a byte array from a Value.</purpose>
        /// <use>x = (byte[])v;</use>
        /// <pre>v is a Value that contains a byte array, i.e. a binary value.</pre>
        /// <post>x contains the byte array in v.</post>
        /// </desc>
        public static explicit operator byte[](Value v)
        {
            return v.AsByteArray;
        }

		/// <title>Cast to double</title>
        /// <desc>
        /// <decl>public static explicit operator double( Value v );</decl>
        /// <purpose>To extract a double from a Value.</purpose>
        /// <use>x = (double)v;</use>
        /// <pre>v is a Value that contains a double.</pre>
        /// <post>x contains the double in v.</post>
        /// </desc>
        public static explicit operator double(Value v)
        {
            return v.AsDouble;
        }

		/// <title>Cast to float</title>
        /// <desc>
        /// <decl>public static explicit operator float( Value v );</decl>
        /// <purpose>To extract a float from a Value.</purpose>
        /// <use>x = (float)v;</use>
        /// <pre>v is a Value that contains a float (actually a double).</pre>
        /// <post>x contains the float in v.</post>
        /// </desc>
        public static explicit operator float(Value v)
        {
            return (float)v.AsDouble;
        }

		/// <ignore>Protected constructor</ignore>
        protected Value()
		{
		}
		
		private static bool IsSmallInteger( long n )
		{
			if( (sbyte)n != n ) return false;
			return (n > -64);
		}

		/// <title>IsDouble</title>
        /// <desc>
        /// <decl>public virtual bool IsDouble {get{...}};</decl>
        /// <purpose>To check whether a Value is a double.</purpose>
        /// <use>b = v.IsDouble;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is a double.</post>
        /// </desc>
		public virtual bool IsDouble {
			get {
				return false;
			}
		}
	
		/// <title>IsLong</title>
        /// <desc>
        /// <decl>public virtual bool IsLong {get{...}};</decl>
        /// <purpose>To check whether a Value is a long.</purpose>
        /// <use>b = v.IsLong;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is a long.</post>
        /// </desc>
		public virtual bool IsLong {
			get {
				return false;
			}
		}
	
		/// <title>IsString</title>
        /// <desc>
        /// <decl>public virtual bool IsString {get{...}};</decl>
        /// <purpose>To check whether a Value is a string.</purpose>
        /// <use>b = v.IsString;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is a string.</post>
        /// </desc>
		public virtual bool IsString {
			get {
				return false;
			}
		}
	
		/// <title>IsArray</title>
        /// <desc>
        /// <decl>public virtual bool IsArray {get{...}};</decl>
        /// <purpose>To check whether a Value is an array.</purpose>
        /// <use>b = v.IsArray;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is an array.</post>
        /// </desc>
		public virtual bool IsArray {
			get {
				return false;
			}
		}
	
		/// <title>IsTable</title>
        /// <desc>
        /// <decl>public virtual bool IsTable {get{...}};</decl>
        /// <purpose>To check whether a Value is a table.</purpose>
        /// <use>b = v.IsTable;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is a table.</post>
        /// </desc>
		public virtual bool IsTable {
			get {
				return false;
			}
		}
	
		/// <title>IsBinary</title>
        /// <desc>
        /// <decl>public virtual bool IsBinary {get{...}};</decl>
        /// <purpose>To check whether a Value is binary.</purpose>
        /// <use>b = v.IsBinary;</use>
        /// <pre>v is not null.</pre>
        /// <post>b is true iff v is binary.</post>
        /// </desc>
		public virtual bool IsBinary {
			get {
				return false;
			}
		}

		/// <title>Indexing With Integers</title>
		/// <section>
		/// <title>Fetching from an array</title>
        /// <desc>
        /// <decl>public virtual Value this[ long i ] {get{...} set{...}};</decl>
        /// <purpose>To fetch the value in a given position in an array.</purpose>
        /// <use>v = a[i];</use>
        /// <pre>a is an array Value. The index i must be within the array bounds, i.e. 0 &#x2264; i &lt; a.Size.</pre>
        /// <post>v is the value in position i in a.</post>
        /// </desc>
        /// </section>
        /// <section>
		/// <title>Fetching from a table</title>
        /// <desc>
        /// <decl>public virtual Value this[ long i ] {get{...} set{...}};</decl>
        /// <purpose>To fetch the value in a given position in a table.</purpose>
        /// <use>v = a[i];</use>
        /// <pre>a is a table Value.</pre>
        /// <post>v is the value in position i in a.</post>
        /// <note>If a is a table then a[i] is functionally equivalent to a[(Value)i].</note>
        /// </desc>
        /// </section>
        /// <section>
		/// <title>Storing in an array</title>
        /// <desc>
        /// <decl>public virtual Value this[ long i ] {get{...} set{...}};</decl>
        /// <purpose>To store a value in a given position in an array.</purpose>
        /// <use>a[i] = v;</use>
        /// <pre>a is an array Value, i is non-negative.</pre>
        /// <post>The value in position i in a is now v.  If necessary the size of the array has been increased to i+1.</post>
        /// </desc>
        /// </section>
        /// <section>
		/// <title>Storing in a table</title>
        /// <desc>
        /// <decl>public virtual Value this[ long i ] {get{...} set{...}};</decl>
        /// <purpose>To store a value in a given position in a table.</purpose>
        /// <use>a[i] = v;</use>
        /// <pre>a is a table Value.</pre>
        /// <post>The value in position i in a is now v.</post>
        /// <note>If a is a table then a[i]=v is functionally equivalent to a[(Value)i]=v.</note>
        /// </desc>
        /// </section>
        public virtual Value this[ long i ] {
            get {
                throw new InvalidOperationException("Illegal attempt to index into Transport.Value");
            }
            set {
                throw new InvalidOperationException("Illegal attempt to index into Transport.Value");
            }
        }

		/// <title>Indexing With Values</title>
        /// <section>
		/// <title>Fetching from a table</title>
        /// <desc>
        /// <decl>public virtual Value this[ Value i ] {get{...} set{...}};</decl>
        /// <purpose>To fetch the value in a given position in a table.</purpose>
        /// <use>v = a[i];</use>
        /// <pre>a is a table Value, i is a Value.</pre>
        /// <post>v is the value in position i in a.</post>
        /// </desc>
        /// </section>
        /// <section>
		/// <title>Storing in a table</title>
        /// <desc>
        /// <decl>public virtual Value this[ Value i ] {get{...} set{...}};</decl>
        /// <purpose>To store a value in a given position in a table.</purpose>
        /// <use>a[i] = v;</use>
        /// <pre>a is a table Value, i is a Value.</pre>
        /// <post>The value in position i in a is now v.</post>
        /// </desc>
        /// </section>
        public virtual Value this[ Value i ] {
            get {
                throw new InvalidOperationException("Illegal attempt to index into Transport.Value");
            }
            set {
                throw new InvalidOperationException("Illegal attempt to index into Transport.Value");
            }
        }

		/// <title>Keys</title>
        /// <desc>
        /// <decl>public virtual Value[] Keys {get{...}};</decl>
        /// <purpose>To get the set of keys in a table.</purpose>
        /// <use>k = a.Keys;</use>
        /// <pre>a is a table.</pre>
        /// <post>k is a Value[] containing all the keys in a.</post>
        /// <note>No key can have a corresponding null value, if a key has a null value in a table
        ///       this is considered equivalent to the key not being present at all.</note>
        /// </desc>
		public virtual Value[] Keys
		{
			get {
	            throw new InvalidOperationException("Attempt to use Getkeys() on a Transport.Value that is not a table");
	        }
		}
	
		/// <title>AsArray</title>
        /// <desc>
        /// <decl>public virtual Value[] AsArray {get{...}};</decl>
        /// <purpose>To get a Value[] corresponding to a value that is an array.</purpose>
        /// <use>va = a.AsArray;</use>
        /// <pre>a is an array.</pre>
        /// <post>va is a Value[] containing all the values in a.</post>
        /// </desc>
		public virtual Value[] AsArray
		{
			get {
				throw new InvalidOperationException("Attempt to use AsArray on a Transport.Value that is not an array");
			}
		}
		
		/// <title>AsDouble</title>
        /// <desc>
        /// <decl>public virtual double AsDouble {get{...}};</decl>
        /// <purpose>To get a double from a Value.</purpose>
        /// <use>d = v.AsDouble;</use>
        /// <pre>v contains a double.</pre>
        /// <post>d is the double in v.</post>
        /// <note>v.AsDouble is equivalent to (double)v.</note>
        /// </desc>
		public virtual double AsDouble
		{
			get {
				throw new InvalidOperationException("Attempt to use asDouble on a Transport.Value that is not floating point");
			}
		}
	
		/// <title>AsLong</title>
        /// <desc>
        /// <decl>public virtual long AsLong {get{...}};</decl>
        /// <purpose>To get a long from a Value.</purpose>
        /// <use>d = v.AsLong;</use>
        /// <pre>v contains a long.</pre>
        /// <post>d is the long in v.</post>
        /// <note>v.AsLong is equivalent to (long)v.</note>
        /// </desc>
		public virtual long AsLong
		{
			get {
				throw new InvalidOperationException("Attempt to use AsLong() on a Transport.Value that is not an integer");
			}
		}
	
		/// <title>AsByteArray</title>
        /// <desc>
        /// <decl>public virtual byte[] AsByteArray {get{...}};</decl>
        /// <purpose>To get a byte[] from a Value.</purpose>
        /// <use>d = v.AsByteArray;</use>
        /// <pre>v contains a binary value.</pre>
        /// <post>d is the array of bytes in v.</post>
        /// <note>v.AsByteArray is equivalent to (byte[])v.</note>
        /// </desc>
		public virtual byte[] AsByteArray
		{
			get {
				throw new InvalidOperationException("Attempt to use AsByteArray on a Transport.Value that is not a binary");
			}
		}
	
		/// <title>AsString</title>
        /// <desc>
        /// <decl>public virtual string AsString {get{...}};</decl>
        /// <purpose>To get a string from a Value.</purpose>
        /// <use>d = v.AsString;</use>
        /// <pre>v contains a string value.</pre>
        /// <post>d is the string in v.</post>
        /// <note>v.AsString is equivalent to (string)v.</note>
        /// </desc>
		public virtual string AsString
		{
			get {
				throw new InvalidOperationException("Attempt to use AsString on a Transport.Value that is not a string");
			}
		}
		
		/// <title>Size</title>
        /// <desc>
        /// <decl>public virtual long Size {get{...}set{...}};</decl>
        /// <purpose>To get or set the size of a Value.</purpose>
        /// <use>n = v.Size;</use>
        /// <pre>v contains an array or a table.</pre>
        /// <post>n is the size of v, i.e. the number of index/key positions in v. For a table this
        /// is the number of keys that have non-null values.</post>
        /// <use>v.Size = n;</use>
        /// <pre>v contains an array, n is non-negative.</pre>
        /// <post>v has been resized to n.</post>
        /// </desc>
		public virtual int Size
		{
			get {
				throw new InvalidOperationException("Attempt to use Size on a Transport.Value that is not an array or table");
			}
			set {
				throw new InvalidOperationException("Attempt to set Size on a Transport.Value that is not an array");
			}
		}

		/// <title>MakeLong</title>
        /// <desc>
        /// <decl>public static Value MakeLong( long n );</decl>
        /// <purpose>To create a long Value.</purpose>
        /// <use>v = Value.MakeLong(n);</use>
        /// <post>v contains the long value n.</post>
        /// <note>Value.MakeLong(n) is equivalent to (Value)n.</note>
        /// </desc>
		public static Value MakeLong( long n )
		{
			return new TInteger(n);
		}
	
		/// <title>MakeDouble</title>
        /// <desc>
        /// <decl>public static Value MakeDouble( double x );</decl>
        /// <purpose>To create a double Value.</purpose>
        /// <use>v = Value.MakeDouble(x);</use>
        /// <post>v contains the double value x.</post>
        /// <note>Value.MakeDouble(x) is equivalent to (Value)x.</note>
        /// </desc>
		public static Value MakeDouble( double n )
		{
			return new TDouble(n);
		}
	
		/// <title>MakeString</title>
        /// <desc>
        /// <decl>public static Value MakeString( string x );</decl>
        /// <purpose>To create a string Value.</purpose>
        /// <use>v = Value.MakeString(s);</use>
        /// <pre>s is not null.</pre>
        /// <post>v contains the string value s.</post>
        /// <note>Value.MakeString(s) is equivalent to (Value)s.</note>
        /// </desc>
		public static Value MakeString( string s )
		{
			return new TString(s);
		}
	
		/// <title>MakeBinary</title>
        /// <desc>
        /// <decl>public static Value MakeBinary( byte[] x );</decl>
        /// <purpose>To create a binary Value.</purpose>
        /// <use>v = Value.MakeBinary(b);</use>
        /// <pre>b is not null.</pre>
        /// <post>v contains the binary value b.</post>
        /// <note>Value.MakeBinary(b) is equivalent to (Value)b.</note>
        /// </desc>
		public static Value MakeBinary( byte[] b )
		{
			return new TBinary(b);
		}
	
		/// <title>MakeArray</title>
        /// <desc>
        /// <decl>public static Value MakeArray();</decl>
        /// <purpose>To create an empty array Value.</purpose>
        /// <use>v = Value.MakeArray();</use>
        /// <post>v contains a new empty array value.</post>
        /// </desc>
		public static Value MakeArray()
		{
			return new TArray(0);
		}
	
		/// <title>MakeArray</title>
        /// <desc>
        /// <decl>public static Value MakeArray( int n );</decl>
        /// <purpose>To create an array Value of given size.</purpose>
        /// <use>v = Value.MakeArray(n);</use>
        /// <pre>n is non-negative.;</pre>
        /// <post>v contains a new array value of size n.</post>
        /// </desc>
		public static Value MakeArray( int n )
		{
			return new TArray(n);
		}
	
		/// <title>MakeArray</title>
        /// <desc>
        /// <decl>public static Value MakeArray( Value[] a );</decl>
        /// <purpose>To create an array Value from a given Value array.</purpose>
        /// <use>v = Value.MakeArray(a);</use>
        /// <pre>a is not null.;</pre>
        /// <post>v contains a new array value of that contains a.</post>
        /// </desc>
		public static Value MakeArray( Value[] a )
		{
			return new TArray(a);
		}
		
		/// <title>MakeTable</title>
        /// <desc>
        /// <decl>public static Value MakeTable();</decl>
        /// <purpose>To create a new empty table.</purpose>
        /// <use>v = Value.MakeTable();</use>
        /// <post>v contains a new empty table value.</post>
        /// </desc>
		public static Value MakeTable()
		{
			return new TTable();
		}

		private static byte EncodeAsByte( int n )
		{
			if( n>255 ) throw new InvalidOperationException("Attempt to encode "+n+" as an unsigned byte");
			return (byte)n;
		}
		
		private static ushort EncodeAsUShort( int n )
		{
			if( n>65535 ) throw new InvalidOperationException("Attempt to encode "+n+" as an unsigned short");
			return ToNet((ushort)n);
		}
	
		private static bool IsStringLatin1( string s )
		{
			int n=s.Length;
			for( int i=0 ; i!=n ; i++ )
				if( (int)s[i] > 255 ) return false;
			return true;
		}
		
		private static short ToNet( Int16 x ) {
			return System.Net.IPAddress.HostToNetworkOrder(x);
		}

		private static UInt16 ToNet( UInt16 x ) {
			return (UInt16)System.Net.IPAddress.HostToNetworkOrder((Int16)x);
		}

		private static Int32 ToNet( Int32 x ) {
			return System.Net.IPAddress.HostToNetworkOrder(x);
		}

		private static Int64 ToNet( Int64 x ) {
			return System.Net.IPAddress.HostToNetworkOrder(x);
		}

		private static double ToNet( double d )
		{
            if( !BitConverter.IsLittleEndian ) return d;
#if PocketPC || NETCF_1_0 || Smartphone
            byte[] b = BitConverter.GetBytes(d);
            byte t;
            t = b[0]; b[0] = b[7]; b[7] = t;
            t = b[1]; b[1] = b[6]; b[6] = t;
            t = b[2]; b[2] = b[5]; b[5] = t;
            t = b[3]; b[3] = b[4]; b[4] = t;
            return BitConverter.ToDouble(b, 0);
#else
            return BitConverter.Int64BitsToDouble(ToNet(BitConverter.DoubleToInt64Bits(d)));
#endif
        }

		private static Int16 FromNet( Int16 x ) {
			return System.Net.IPAddress.NetworkToHostOrder(x);
		}

		private static UInt16 FromNet( UInt16 x ) {
			return (UInt16)System.Net.IPAddress.NetworkToHostOrder((Int16)x);
		}

		private static Int32 FromNet( Int32 x ) {
			return System.Net.IPAddress.NetworkToHostOrder(x);
		}

		private static Int64 FromNet( Int64 x ) {
			return System.Net.IPAddress.NetworkToHostOrder(x);
		}

		private static double FromNet( double d )
		{
            if( !BitConverter.IsLittleEndian ) return d;
#if PocketPC || NETCF_1_0 || Smartphone
            byte[] b = BitConverter.GetBytes(d);
            byte t;
            t = b[0]; b[0] = b[7]; b[7] = t;
            t = b[1]; b[1] = b[6]; b[6] = t;
            t = b[2]; b[2] = b[5]; b[5] = t;
            t = b[3]; b[3] = b[4]; b[4] = t;
            return BitConverter.ToDouble(b, 0);
#else
            return BitConverter.Int64BitsToDouble(FromNet(BitConverter.DoubleToInt64Bits(d)));
#endif
        }

		/* Unused
		private static char FromNet( char c )
		{
			return (char)FromNet((Int16)c);
		}
		*/

		/// <title>Write</title>
        /// <desc>
        /// <decl>public static void Write( Value val, BinaryWriter dout );</decl>
        /// <purpose>To write a Value in an efficient binary format.</purpose>
        /// <use>Value.Write(val,dout);</use>
        /// <pre>dout is a BinaryWriter, val may be any Value, even null.</pre>
        /// <post>val has been written to dout in an efficient binary format.</post>
        /// </desc>
		public static void Write( Value val, BinaryWriter dout )
		{
			if( val==null )
			{
				dout.Write((sbyte)TypeCode.TYPE_NULL);
				return;
			}
			if( val.IsLong )
			{
				long i = val.AsLong;
				if( IsSmallInteger(i) )
				{
					dout.Write((sbyte)i);
					return;
				}
				if( (long)(sbyte)i == i )
				{
					dout.Write((sbyte)TypeCode.TYPE_INT8);
					dout.Write((sbyte)i);
					return;
				}
				if( (long)(short)i == i )
				{
					dout.Write((sbyte)TypeCode.TYPE_INT16);
					dout.Write(ToNet((short)i));
					return;
				}
				if( (long)(int)i == i )
				{
					dout.Write((sbyte)TypeCode.TYPE_INT32);
					dout.Write(ToNet((int)i));
					return;
				}
				dout.Write((sbyte)TypeCode.TYPE_INT64);
				dout.Write(ToNet(i));
				return;
			}
			if( val.IsString )
			{
				string s = val.AsString;
				int n = s.Length;
				if( IsStringLatin1(s) )
				{
					if( n<256 )
					{
						dout.Write((sbyte)TypeCode.TYPE_STRING_LATIN1_8);
						dout.Write(EncodeAsByte(n));
						for( int i=0 ; i!=n ; i++ )
							dout.Write(EncodeAsByte(s[i]));
						return;
					}
					if( n<65536 )
					{
						dout.Write((sbyte)TypeCode.TYPE_STRING_LATIN1_16);
						dout.Write(EncodeAsUShort(n));
						for( int i=0 ; i!=n ; i++ )
							dout.Write(EncodeAsByte(s[i]));
						return;
					}
					dout.Write((sbyte)TypeCode.TYPE_STRING_LATIN1_32);
					dout.Write(ToNet(n));
					for( int i=0 ; i!=n ; i++ )
						dout.Write(EncodeAsByte(s[i]));
					return;
				}
				if( n<256 )
				{
					dout.Write((sbyte)TypeCode.TYPE_STRING_UCS2_8);
					dout.Write(EncodeAsByte(n));
					for( int i=0 ; i!=n ; i++ )
						dout.Write(ToNet((Int16)s[i]));
					return;
				}
				if( n<65536 )
				{
					dout.Write((sbyte)TypeCode.TYPE_STRING_UCS2_16);
					dout.Write(EncodeAsUShort(n));
					for( int i=0 ; i!=n ; i++ )
						dout.Write(ToNet((Int16)s[i]));
					return;
				}
				dout.Write((sbyte)TypeCode.TYPE_STRING_UCS2_32);
				dout.Write(ToNet(n));
				for( int i=0 ; i!=n ; i++ )
					dout.Write(ToNet((Int16)s[i]));
				return;
			}
			if( val.IsDouble )
			{
				dout.Write((sbyte)TypeCode.TYPE_DOUBLE);
				dout.Write(ToNet(val.AsDouble));
				return;
			}
			if( val.IsBinary )
			{
				byte[] b = val.AsByteArray;
				int n = b.Length;
				if( n<256 )
				{
					dout.Write((sbyte)TypeCode.TYPE_BINARY_8);
					dout.Write(EncodeAsByte(n));
					dout.Write(b);
					return;
				}
				if( n<65536 )
				{
					dout.Write((sbyte)TypeCode.TYPE_BINARY_16);
					dout.Write(EncodeAsUShort(n));
					dout.Write(b);
					return;
				}
				dout.Write((sbyte)TypeCode.TYPE_BINARY_32);
				dout.Write(ToNet(n));
				dout.Write(b);
				return;
			}
			if( val.IsTable )
			{
				Value[] keys = val.Keys;
				int n = keys.Length;
				if( n<256 )
				{
					dout.Write((sbyte)TypeCode.TYPE_TABLE_8);
					dout.Write(EncodeAsByte(n));
					for( int i=0 ; i!=n ; i++ )
					{
						Write(keys[i],dout);
						Write(val[keys[i]],dout);
					}
					return;
				}
				if( n<65536 )
				{
					dout.Write((sbyte)TypeCode.TYPE_TABLE_16);
					dout.Write(EncodeAsUShort(n));
					for( int i=0 ; i!=n ; i++ )
					{
						Write(keys[i],dout);
						Write(val[keys[i]],dout);
					}
					return;
				}
				dout.Write((sbyte)TypeCode.TYPE_TABLE_32);
				dout.Write(ToNet(n));
				for( int i=0 ; i!=n ; i++ )
				{
					Write(keys[i],dout);
					Write(val[keys[i]],dout);
				}
				return;
			}
			// Otherwise it must be an array
			{
				Value[] a = val.AsArray;
				int n = a.Length;
				if( n<256 )
				{
					dout.Write((sbyte)TypeCode.TYPE_ARRAY_8);
					dout.Write(EncodeAsByte(n));
					for( int i=0 ; i!=n ; i++ )
						Write(a[i],dout);
					return;
				}
				if( n<65536 )
				{
					dout.Write((sbyte)TypeCode.TYPE_ARRAY_16);
					dout.Write(EncodeAsUShort(n));
					for( int i=0 ; i!=n ; i++ )
						Write(a[i],dout);
					return;
				}
				dout.Write((sbyte)TypeCode.TYPE_ARRAY_32);
				dout.Write(ToNet(n));
				for( int i=0 ; i!=n ; i++ )
					Write(a[i],dout);
			}
		}
		
		private static string ReadLatin1( BinaryReader din, int n )
		{
			char[] c = new char[n];
			for( int i=0 ; i!=n ; i++ )
				c[i] = (char)din.ReadByte();
			return new string(c);
		}
		
		private static string ReadUCS2( BinaryReader din, int n )
		{
			char[] c = new char[n];
			for( int i=0 ; i!=n ; i++ ) {
				c[i] = (char)FromNet(din.ReadInt16());
			}
			return new string(c);
		}
		
		private static string ReadUCS4( BinaryReader din, int n )
		{
			char[] c = new char[n];
			for( int i=0 ; i!=n ; i++ )
				c[i] = (char)FromNet(din.ReadInt32());
			return new string(c);
		}
		
		private static Value ReadArray( BinaryReader din, int n )
		{
			Value[] a = new Value[n];
			for( int i=0 ; i!=n ; i++ )
				a[i] = Read(din);
			return MakeArray(a);
		}
	
		private static Value ReadTable( BinaryReader din, int n )
		{
			Value res = MakeTable();
			for( int i=0 ; i!=n ; i++ )
			{
				Value key = Read(din);
				Value val = Read(din);
				res[key] = val;
			}
			return res;
		}
	
		private static Value ReadBinary( BinaryReader din, int n )
		{
			byte[] b = new byte[n];
            int count, idx = 0;
            while (n != 0)
            {
                count = din.Read(b, idx, n);
                idx += count;
                n -= count;
            }
			return MakeBinary(b);
		}
	
		/// <title>Read</title>
        /// <desc>
        /// <decl>public static Value Read( BinaryReader din );</decl>
        /// <purpose>To read a Value written in the same format as Value.Write uses.</purpose>
        /// <use>val = Value.Read(din);</use>
        /// <pre>din is a BinaryReader that contains a written Value.</pre>
        /// <post>val has been read from din.</post>
        /// </desc>
		public static Value Read( BinaryReader din )
		{
			sbyte type = din.ReadSByte();
			switch( (TypeCode)type )
			{
			case TypeCode.TYPE_NULL:
				return null;
			case TypeCode.TYPE_INT8:
				return MakeLong(din.ReadSByte());
			case TypeCode.TYPE_INT16:
				return MakeLong(FromNet(din.ReadInt16()));
			case TypeCode.TYPE_INT32:
				return MakeLong(FromNet(din.ReadInt32()));
			case TypeCode.TYPE_INT64:
				return MakeLong(FromNet(din.ReadInt64()));
			case TypeCode.TYPE_DOUBLE:
				return MakeDouble(FromNet(din.ReadDouble()));
			case TypeCode.TYPE_STRING_LATIN1_8:
				return MakeString(ReadLatin1(din,din.ReadByte()));
			case TypeCode.TYPE_STRING_LATIN1_16:
				return MakeString(ReadLatin1(din,FromNet(din.ReadUInt16())));
			case TypeCode.TYPE_STRING_LATIN1_32:
				return MakeString(ReadLatin1(din,FromNet(din.ReadInt32())));
			case TypeCode.TYPE_STRING_UCS2_8:
				return MakeString(ReadUCS2(din,din.ReadByte()));
			case TypeCode.TYPE_STRING_UCS2_16:
				return MakeString(ReadUCS2(din,FromNet(din.ReadUInt16())));
			case TypeCode.TYPE_STRING_UCS2_32:
				return MakeString(ReadUCS2(din,FromNet(din.ReadInt32())));
			case TypeCode.TYPE_STRING_UCS4_8:
				return MakeString(ReadUCS4(din,din.ReadByte()));
			case TypeCode.TYPE_STRING_UCS4_16:
				return MakeString(ReadUCS4(din,FromNet(din.ReadUInt16())));
			case TypeCode.TYPE_STRING_UCS4_32:
				return MakeString(ReadUCS4(din,FromNet(din.ReadInt32())));
			case TypeCode.TYPE_ARRAY_8:
				return ReadArray(din,din.ReadByte());
			case TypeCode.TYPE_ARRAY_16:
				return ReadArray(din,FromNet(din.ReadUInt16()));
			case TypeCode.TYPE_ARRAY_32:
				return ReadArray(din,FromNet(din.ReadInt32()));
			case TypeCode.TYPE_TABLE_8:
				return ReadTable(din,din.ReadByte());
			case TypeCode.TYPE_TABLE_16:
				return ReadTable(din,FromNet(din.ReadUInt16()));
			case TypeCode.TYPE_TABLE_32:
				return ReadTable(din,FromNet(din.ReadInt32()));
			case TypeCode.TYPE_BINARY_8:
				return ReadBinary(din,din.ReadByte());
			case TypeCode.TYPE_BINARY_16:
				return ReadBinary(din,FromNet(din.ReadUInt16()));
			case TypeCode.TYPE_BINARY_32:
				return ReadBinary(din,FromNet(din.ReadInt32()));
			default:
				return MakeLong((long)type);
			}
		}
		
		/// <title>Flatten</title>
        /// <desc>
        /// <decl>public static Value Flatten( Value val )</decl>
        /// <purpose>To marshall a Value into a single binary Value.</purpose>
        /// <use>bin = Value.Flatten(val);</use>
        /// <pre>val is any Value, even null.</pre>
        /// <post>bin is a binary value that contains a representation of val,
        ///       in the same format as Value.Write uses.</post>
        /// </desc>
		public static Value Flatten( Value val )
		{
            MemoryStream mout = new MemoryStream();
			BinaryWriter dout = new BinaryWriter(mout);
			Write(val,dout);
			dout.Flush();
			return MakeBinary(mout.ToArray());
		}
		
		/// <title>Unflatten</title>
        /// <desc>
        /// <decl>public static Value Unflatten( Value val )</decl>
        /// <purpose>To unmarshall a Value from a single binary Value.</purpose>
        /// <use>val = Value.Unflatten(bin);</use>
        /// <pre>bin is a binary Value that is a marshalling of some original Value.</pre>
        /// <post>val is equivalent to the original Value.</post>
        /// </desc>
		public static Value Unflatten( Value val )
		{
			MemoryStream min = new MemoryStream(val.AsByteArray);
			BinaryReader din = new BinaryReader(min);
			return Read(din);
		}

		/// <title>Format</title>
        /// <desc>
        /// <decl>public static string Format( Value val )</decl>
        /// <purpose>To create a printable string representation of a Value.</purpose>
        /// <use>s = Value.Format(val);</use>
        /// <pre>val is any Value, even null.</pre>
        /// <post>s is a printable representation of val.</post>
        /// </desc>
		public static string Format( Value v )
		{
			Formatter f = new Formatter();
			f.Format(v);
			return f.Result();
		}
		
		private static byte[] Pad( byte[] b )
		{
			int p;
			byte[] result;
			p = 16-b.Length%16;
			result = new byte[b.Length+p];
			System.Array.Copy(b,0,result,0,b.Length);
			for( int i=b.Length ; i!=result.Length ; i++ ) result[i] = (byte)p;
			return result;
		}

		/// <title>LoadFile</title>
        /// <desc>
        /// <decl>public static Value LoadFile( string filename )</decl>
        /// <purpose>To load the contents of a file as a Value.</purpose>
        /// <use>val = Value.LoadFile(filename);</use>
        /// <pre>filename refers to an existing file that contains a Value.</pre>
        /// <post>val refers to a new value that contains the contents of the file.</post>
        /// <note>Loading a value is equivalent to reading the file into a binary value and
	    ///                         then unflattening that value.</note>
        /// </desc>
		public static Value LoadFile( string filename )
		{
			FileStream inp = File.OpenRead(filename);
			BinaryReader din = new BinaryReader(inp);
			Value result = Value.Read(din);
			din.Close();
			inp.Close();
			return result;
		}
	
		/// <title>SaveFile</title>
        /// <desc>
        /// <decl>public static void SaveFile( Value val, string filename )</decl>
        /// <purpose>To save any Value into a file.</purpose>
        /// <use>Value.SaveFile(val,filename);</use>
        /// <pre>val refers to any Value, filename refers to a valid filename to write to.</pre>
        /// <post>The contents of val have been saved in the file.</post>
        /// <note>Saving and then loading a value does a deep clone of the value.  The value to
	    ///                         be saved must not contain reference loops.</note>
        /// <note>Saving a value is equivalent to writing the flattened result of the value
	    ///                         to the file.</note>
        /// </desc>
		public static void SaveFile( Value val, string filename )
		{
			FileStream outp = File.Open(filename,FileMode.Create);
			BinaryWriter dout = new BinaryWriter(outp);
			Value.Write(val,dout);
			dout.Close();
			outp.Close();
		}

#if !NOCRYPTO
		private static byte[] Unpad( byte[] b )
		{
			int p = b[b.Length-1];
			byte[] result = new byte[b.Length-p];
			System.Array.Copy(b,0,result,0,b.Length-p);
			return result;
		}
	
		private static void Xor( byte[] key, int keyidx, byte[] from, int fromidx, byte[] to, int toidx )
		{
			for( int i=0 ; i!=16 ; i++ )
				to[toidx+i] = (byte)(key[keyidx+i] ^ from[fromidx+i]);
		}
	
		private static Org.BouncyCastle.Security.SecureRandom myRand = null;
		private static Org.BouncyCastle.Crypto.Prng.ThreadedSeedGenerator mySGen = null;

		/// <title>EncryptAESCBC</title>
        /// <desc>
        /// <decl>public static Value EncryptAESCBC( Value plain, byte[] key )</decl>
        /// <purpose>To encrypt a binary value using AES encryption in CBC mode with PKCS5/PKCS7 padding.</purpose>
        /// <use>crypto = Value.encryptAESCBC(plain,key,rand);</use>
        /// <pre>plain refers to a binary value, key is of length either 16, 24 or 32 bytes
	    ///                        (128, 192 or 256 bits).</pre>
        /// <post>crypto refers to a new binary value that contains the encrypted result.</post>
        /// </desc>
		public static Value EncryptAESCBC( Value plain, byte[] key )
		{
			if( myRand == null )
			{
		    	myRand = Org.BouncyCastle.Security.SecureRandom.GetInstance("SHA1PRNG");
		    	mySGen = new Org.BouncyCastle.Crypto.Prng.ThreadedSeedGenerator();
		    }
	    	byte[] seed = mySGen.GenerateSeed(16,true);
	    	myRand.SetSeed(seed);
			return EncryptAESCBC(plain,key,myRand);
		}

		/// <title>EncryptAESCBC</title>
        /// <desc>
        /// <decl>public static Value EncryptAESCBC( Value plain, byte[] key, SecureRandom rand )</decl>
        /// <purpose>To encrypt a binary value using AES encryption in CBC mode with PKCS5/PKCS7 padding.</purpose>
        /// <use>crypto = Value.encryptAESCBC(plain,key,rand);</use>
        /// <pre>plain refers to a binary value, key is of length either 16, 24 or 32 bytes
	    ///                        (128, 192 or 256 bits), rand is a SecureRandom object.</pre>
        /// <post>crypto refers to a new binary value that contains the encrypted result.</post>
        /// </desc>
		public static Value EncryptAESCBC( Value plain, byte[] key, Org.BouncyCastle.Security.SecureRandom rand )
		{
			byte[] b = Pad(plain.AsByteArray);
			Org.BouncyCastle.Crypto.Engines.AesFastEngine engine = new Org.BouncyCastle.Crypto.Engines.AesFastEngine();
			engine.Init(true,new Org.BouncyCastle.Crypto.Parameters.KeyParameter(key));
			byte[] result = new byte[b.Length+16];
			byte[] iv = new byte[16];
			rand.NextBytes(iv);
			System.Array.Copy(iv,0,result,0,16);
			for( int i=0 ; i<b.Length ; )
			{
				Xor(iv,0,b,i,iv,0);  // iv = iv xor plain
				i += 16;
				engine.ProcessBlock(iv,0,result,i);
				System.Array.Copy(result,i,iv,0,16);
			}
			return Value.MakeBinary(result);
		}
		
		/// <title>DecryptAESCBC</title>
        /// <desc>
        /// <decl>public static Value DecryptAESCBC( Value crypto, byte[] key )</decl>
        /// <purpose>To encrypt a binary value using AES encryption in CBC mode with PKCS5/PKCS7 padding.</purpose>
        /// <use>plain = Value.decryptAESCBC(crypto,key);</use>
        /// <pre>crypto refers to a binary value that was generated using the
	    ///                        method used by encryptAESCBC, key is the key used in that encryption.</pre>
        /// <post>plain refers to a new binary value that contains the plaintext result.</post>
        /// </desc>
		public static Value DecryptAESCBC( Value crypto, byte[] key )
		{
			byte[] b = crypto.AsByteArray;
			Org.BouncyCastle.Crypto.Engines.AesFastEngine engine = new Org.BouncyCastle.Crypto.Engines.AesFastEngine();
			engine.Init(false,new Org.BouncyCastle.Crypto.Parameters.KeyParameter(key));
			byte[] result = new byte[b.Length-16];
			for( int i=0 ; i<result.Length ; i+=16 )
			{
				engine.ProcessBlock(b,i+16,result,i);
				Xor(result,i,b,i,result,i);
			}
			return Value.MakeBinary(Unpad(result));
		}

		/// <title>EncryptRSA</title>
        /// <desc>
        /// <decl>public static Value EncryptRSA( Value plain, Org.BouncyCastle.Math.BigInteger m, Org.BouncyCastle.Math.BigInteger e )</decl>
        /// <purpose>To encrypt a binary value using RSA encryption with a public key.</purpose>
        /// <use>crypto = Value.EncryptRSA(plain,m,e);</use>
        /// <pre>plain refers to a binary value. When plain is viewed as a big integer (big-endian, two's complement)
	    ///                        it must me less than m.  e is the RSA public exponent.  
	    ///                        The integer m must be an RSA public key, i.e. the product of two large primes.</pre>
        /// <post>crypto refers to a new binary value that contains the encrypted result.</post>
        /// </desc>
		public static Value EncryptRSA( Value plain, Org.BouncyCastle.Math.BigInteger m, Org.BouncyCastle.Math.BigInteger e )
		{
			Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters publicKey = 
				new Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters(false,m,e);
			Org.BouncyCastle.Crypto.Engines.RsaEngine rsa = 
				new Org.BouncyCastle.Crypto.Engines.RsaEngine();
			rsa.Init(true,publicKey);
			byte[] org = plain.AsByteArray;
			return Value.MakeBinary(rsa.ProcessBlock(org,0,org.Length));
		}
	
		/// <title>EncryptRSA</title>
        /// <desc>
        /// <decl>public static Value EncryptRSA( Value plain, Value key )</decl>
        /// <purpose>To encrypt a binary value using RSA encryption with a public key.</purpose>
        /// <use>crypto = Value.encryptRSA(plain,key);</use>
        /// <pre>plain refers to a binary value. key can either be a binary value
	    ///                        or a table with a key 'M' and an optional key 'E', under both
	    ///                        of which there should be binary values. The binary values all
	    ///                        represent (large) integer values.  If the 'E' key is missing,
	    ///                        then that is equivalent to an 'E' value of 65537.  If key is
	    ///                        binary rather than a table then that binary value is regarded as
	    ///                        the 'M' value. The integer value of plain must be less than that of
	    ///                        key (or 'M').  The integer that 'M' represents must be an RSA
	    ///                        public key, i.e. the product of two large primes. The value of
	    ///                        'E' is the public RSA exponent.</pre>
        /// <post>crypto refers to a new binary value that contains the encrypted result.</post>
        /// </desc>
		public static Value EncryptRSA( Value plain, Value key )
		{
			Org.BouncyCastle.Math.BigInteger m,e;
			if( key.IsBinary )
			{
				m = new Org.BouncyCastle.Math.BigInteger(key.AsByteArray);
				e = Org.BouncyCastle.Math.BigInteger.ValueOf(65537);
			}
			else
			{
				m = new Org.BouncyCastle.Math.BigInteger(key["M"].AsByteArray);
				Value ev = key["E"];
				if( ev!=null )
					e = new Org.BouncyCastle.Math.BigInteger(ev.AsByteArray);
				else
					e = Org.BouncyCastle.Math.BigInteger.ValueOf(65537);
			}
			return EncryptRSA(plain,m,e);
		}
	
		/// <title>NormalizePrivateKeyRSA</title>
        /// <desc>
        /// <decl>public static Value NormalizePrivateKeyRSA( Value key )</decl>
        /// <purpose>To normalize an RSA private key in order to speed up its use.</purpose>
        /// <use>newkey = Value.NormalizePrivateKeyRSA(key);</use>
        /// <pre>key is a table containing two binary values user the keys 'P' and 'Q' that
	    ///                        represent P and Q in the RSA algorithm.  The table may or may not also
	    ///                        contain binary values under one or more of the keys 'E', 'M', 'D', 'DP', 'DQ' 
	    ///                        and 'QINV', which, if present, must contain the corresponding RSA values
	    ///                        appropriate for the given P and Q.  If 'E' is not present it is assumed
	    ///                        to be 65537.</pre>
        /// <post>newkey refers to a new table that contains the normalized key.</post>
        /// </desc>
		public static Value NormalizePrivateKeyRSA( Value key )
		{
			if( key["P"]!=null 
			 && key["Q"]!=null
			 && key["E"]!=null
			 && key["M"]!=null 
			 && key["D"]!=null 
			 && key["DP"]!=null 
			 && key["DQ"]!=null 
			 && key["QInv"]!=null 
			) return key;

			Org.BouncyCastle.Math.BigInteger P,Q,E,M=null,D=null,DP=null,DQ=null,QInv=null;
			P = new Org.BouncyCastle.Math.BigInteger(key["P"].AsByteArray);
			Q = new Org.BouncyCastle.Math.BigInteger(key["Q"].AsByteArray);
			Value tmp;
			tmp = key["D"];
			if( tmp != null && tmp.IsBinary )
				D = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			tmp = key["DP"];
			if( tmp != null && tmp.IsBinary )
				DP = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			tmp = key["DQ"];
			if( tmp != null && tmp.IsBinary )
				DQ = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			tmp = key["QINV"];
			if( tmp != null && tmp.IsBinary )
				QInv = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			tmp = key["M"];
			if( tmp != null && tmp.IsBinary )
				M = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			tmp = key["E"];
			if( tmp != null && tmp.IsBinary )
				E = new Org.BouncyCastle.Math.BigInteger(tmp.AsByteArray);
			else
				E = Org.BouncyCastle.Math.BigInteger.ValueOf(65537);
	
			Org.BouncyCastle.Math.BigInteger ONE = Org.BouncyCastle.Math.BigInteger.ValueOf(1);
	        Org.BouncyCastle.Math.BigInteger pSub1 = P.Subtract(ONE);
	        Org.BouncyCastle.Math.BigInteger qSub1 = Q.Subtract(ONE);
	        if( D==null ) 
	        {
		        Org.BouncyCastle.Math.BigInteger phi = pSub1.Multiply(qSub1);
	        	D = E.ModInverse(phi);
	        }
	        if( DP==null ) DP = D.Remainder(pSub1);
	        if( DQ==null ) DQ = D.Remainder(qSub1);
	        if( QInv==null ) QInv = Q.ModInverse(P);
	        if( M==null ) M = P.Multiply(Q);
	
			Value result = Value.MakeTable();
			result["E"] = Value.MakeBinary(E.ToByteArray());
			result["P"] = Value.MakeBinary(P.ToByteArray());
			result["Q"] = Value.MakeBinary(Q.ToByteArray());
			result["M"] = Value.MakeBinary(M.ToByteArray());
			result["D"] = Value.MakeBinary(D.ToByteArray());
			result["DP"] = Value.MakeBinary(DP.ToByteArray());
			result["DQ"] = Value.MakeBinary(DQ.ToByteArray());
			result["QINV"] = Value.MakeBinary(QInv.ToByteArray());
			return result;
		}
	
		/// <title>DecryptRSA</title>
        /// <desc>
        /// <decl>public static Value DecryptRSA( Value crypto, Value key )</decl>
        /// <purpose>To decrypt a binary value using RSA decryption with a private key.</purpose>
        /// <use>plain = Value.DecryptRSA(crypto,key);</use>
        /// <pre>crypto refers to a binary value, key refers to a Value containing
	    ///                        a private RSA key (valid argument to NormalizePrivateKeyRSA).</pre>
        /// <post>plain refers to a new binary value that contains the decrypted result.</post>
        /// </desc>
		public static Value DecryptRSA( Value crypto, Value key )
		{
			Value normalKey = NormalizePrivateKeyRSA(key);
			Org.BouncyCastle.Math.BigInteger E,P,Q,M,D,DP,DQ,QInv;
			E = new Org.BouncyCastle.Math.BigInteger(normalKey["E"].AsByteArray);
			P = new Org.BouncyCastle.Math.BigInteger(normalKey["P"].AsByteArray);
			Q = new Org.BouncyCastle.Math.BigInteger(normalKey["Q"].AsByteArray);
			M = new Org.BouncyCastle.Math.BigInteger(normalKey["M"].AsByteArray);
			D = new Org.BouncyCastle.Math.BigInteger(normalKey["D"].AsByteArray);
			DP = new Org.BouncyCastle.Math.BigInteger(normalKey["DP"].AsByteArray);
			DQ = new Org.BouncyCastle.Math.BigInteger(normalKey["DQ"].AsByteArray);
			QInv = new Org.BouncyCastle.Math.BigInteger(normalKey["QINV"].AsByteArray);
	
			Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters privateKey =
				new Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters(M,E,D,P,Q,DP,DQ,QInv);
			Org.BouncyCastle.Crypto.Engines.RsaEngine rsa = 
				new Org.BouncyCastle.Crypto.Engines.RsaEngine();
			rsa.Init(false,privateKey);
			byte[] bcrypto = crypto.AsByteArray;
			return Value.MakeBinary(rsa.ProcessBlock(bcrypto,0,bcrypto.Length));
		}
	
		/// <title>HashSHA1</title>
        /// <desc>
        /// <decl>public static Value HashSHA1( Value val )</decl>
        /// <purpose>To hash a binary value using SHA1 secure hash.</purpose>
        /// <use>hash = Value.HashSHA1(val);</use>
        /// <pre>val refers to a binary value.</pre>
        /// <post>hash refers to a new binary value that contains the SHA1 hash of val.</post>
        /// </desc>
		public static Value HashSHA1( Value val )
		{
			Org.BouncyCastle.Crypto.Digests.Sha1Digest digester = new Org.BouncyCastle.Crypto.Digests.Sha1Digest();
			byte[] b = val.AsByteArray;
			digester.BlockUpdate(b,0,b.Length);
			byte[] digest = new byte[digester.GetDigestSize()];
			digester.DoFinal(digest,0);
			return Value.MakeBinary(digest);
		}
		

		private static void Test( BinaryWriter dout, BinaryReader din, Value v ) {
			Value.Write(v,dout);
			Value r = Value.Read(din);
			if( !r.Equals(v) ) {
				System.Console.WriteLine("Fail");
				System.Console.WriteLine(Value.Format(v));
				System.Console.WriteLine(Value.Format(r));
				throw new Exception();
			}
			//else
			//	System.Console.WriteLine(Value.Format(r));
		}
#endif
/*
		/// <ignore>Test program</ignore>
	    public static void Main( string[] args ) {
	    	
	    	Value v;
	    	
	    	if( args.Length==1 && args[0].Equals("server") ) {
				System.Net.Sockets.TcpListener myListener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Parse("127.0.0.1"),15123);
				try {
					myListener.Start();
					for(;;) {
						System.Net.Sockets.Socket sock = myListener.AcceptSocket();
						Stream ns = new System.Net.Sockets.NetworkStream(sock);
						BinaryReader din = new BinaryReader(ns);
			            BinaryWriter dout = new BinaryWriter(ns);
			    		v = Value.Read(din);
			    		if( v==null ) {
							return;
			    		}
			    		while( v!=null ) {
			    			Value.Write(v,dout);
							//System.Console.WriteLine(Value.Format(v));
							v = Value.Read(din);
			    		}
			    	}
			    }
			    finally {
					myListener.Stop();
			    }
	    	}
	    	if( args.Length==1 && args[0].Equals("client") ) {
	            System.Net.Sockets.TcpClient theTcpClient = new System.Net.Sockets.TcpClient("127.0.0.1",15123);
	            try {
					Stream ns = theTcpClient.GetStream();
					BinaryReader din = new BinaryReader(ns);
		            BinaryWriter dout = new BinaryWriter(ns);
					v = Value.MakeTable();
					Test(dout,din,v);
		            v["abc"] = "halló";
					Test(dout,din,v);
					Test(dout,din,1.2345);
					Test(dout,din,-1);
					Test(dout,din,-123456789012345);
					Test(dout,din,123456789012345);
					Test(dout,din,123456789);
					Test(dout,din,-123456789);
					Test(dout,din,65525);
					Test(dout,din,-65535);
					Test(dout,din,"þæöð");
					Test(dout,din,"ÞÆÖÐ");
					Test(dout,din,"ÞÆ\u1234Ð");
					Test(dout,din,"ÞÆ\u8234Ð");
					int g;
					for( g=-1000 ; g!=1000 ; g++ )
						Test(dout,din,g);
					v = Value.MakeTable();
					for( g=0 ; g!=100000 ; g++ ) {
						v[g] = -g;
						Test(dout,din,v);
						System.Console.WriteLine(""+g);
					}
		            v = null;
		            Value.Write(v,dout);
		    		return;
		    	}
		    	finally {
		    		theTcpClient.Close();
		    	}
	    	}

	    	int i;
			v = Value.MakeTable();
			Value k1 = Value.MakeArray();
			k1[0] = "key";
			Value k2 = Value.MakeArray();
			k2[0] = "key";
			v[k1] = "þæöá";
			v["B"] = -1;
			System.Console.WriteLine("size="+v.Size);

			System.Console.WriteLine(v[k2].AsString);
			v = Value.Flatten(v);
			for( i=0 ; i!=v.AsByteArray.Length ; i++ )
				System.Console.WriteLine("v["+i+"]="+v.AsByteArray[i]);
			System.Console.WriteLine(Value.Format(v));
			v = Value.Unflatten(v);
			System.Console.WriteLine(Value.Format(v));
			System.Console.WriteLine(v[k2].AsString);
			System.Console.WriteLine(v["B"].AsLong);

			v = 1.234;
			v = Value.Flatten(v);
			for( i=0 ; i!=v.AsByteArray.Length ; i++ )
				System.Console.WriteLine("v["+i+"]="+v.AsByteArray[i]);
	    }
*/

	}

	class TInteger: Value
	{
		private long val;
		
		public override int GetHashCode()
		{
			return val.GetHashCode();
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TInteger) ) return false;
			return (val==((TInteger)obj).val);
		}

		public TInteger( long n )
		{
			val = n;
		}

		public override bool IsLong
		{
			get {
				return true;
			}
		}

		public override long AsLong
		{
			get {
				return val;
			}
		}
	}
	
	class TDouble: Value {
		private double val;
		
		public override int GetHashCode()
		{
			return val.GetHashCode();
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TDouble) ) return false;
			return (val==((TDouble)obj).val);
		}

		public TDouble( double n )
		{
			val = n;
		}

		public override bool IsDouble
		{
			get {
				return true;
			}
		}

		public override double AsDouble
		{
			get {
				return val;
			}
		}
	}
	
	class TString: Value {
		private string val;

		public override int GetHashCode()
		{
			return val.GetHashCode();
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TString) ) return false;
			return (val.Equals(((TString)obj).val));
		}

		public TString( string s )
		{
			if( s==null )
				throw new InvalidOperationException("Attempt to make a string Transport.Value from a null");
			val = s;
		}

		public override bool IsString {
			get {
				return true;
			}
		}

		public override string AsString {
			get {
				return val;
			}
		}
	}
	
	class TBinary: Value {
		private byte[] val;

		public override int GetHashCode()
		{
			int res=0, n=val.Length;
			for( int i=0 ; i!=n ; i++ )
				res += i*val[i];
			return res;
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TBinary) ) return false;
			TBinary other = (TBinary)obj;
			int n=val.Length;
			if( n != other.val.Length ) return false;
			for( int i=0 ; i!= n ; i++ )
				if( val[i] != other.val[i] ) return false;
			return true;
		}

		public TBinary( byte[] b )
		{
			if( b==null )
				throw new InvalidOperationException("Attempt to make a binary Transport.Value from a null");
			val = b;
		}

		public override bool IsBinary
		{
			get {
				return true;
			}
		}

		public override byte[] AsByteArray
		{
			get {
				return val;
			}
		}
	}
	
	class TArray: Value {
		private Value[] val;

		public override int GetHashCode()
		{
			int n=val.Length,res=0;
			for( int i=0 ; i!=n ; i++ )
			{
				Value v = val[i];
				if( v==null )
					res += i;
				else
					res += i*v.GetHashCode();
			}
			return res;
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TArray) ) return false;
			TArray a = (TArray)obj;
			int n=val.Length;
			if( a.Size != n ) return false;
			for( int i=0 ; i!=n ; i++ ) {
				Value x=val[i], y=a[i];
				if( x==null ) {
					if( y!=null ) return false;
				}
				else if( !x.Equals(y) ) return false;
			}
			return true;
		}

		public TArray( int n )
		{
			val = new Value[n];
		}

		public TArray( Value[] a )
		{
			if( a==null )
				throw new InvalidOperationException("Attempt to make an array Transport.Value from a null");
			val = a;
		}

		public override bool IsArray
		{
			get {
				return true;
			}
		}

		public override Value[] AsArray
		{
			get {
				return val;
			}
		}

		public override Value this[ long i ] {
			get {
				return val[i];
			}
			set {
				if( val.Length <= i )
				{
					Value[] newval = new Value[i+1];
					System.Array.Copy(val,0,newval,0,val.Length);
					val = newval;
				}
				val[i] = value;
			}
		}

		public override Value this[ Value i ] {
			get {
				return val[i.AsLong];
			}
			set {
				val[i.AsLong] = value;
			}
		}

		public override int Size
		{
			get {
				return val.Length;
			}
			set {
				Resize(value);
			}
		}

		private void Resize( int n )
		{
			int k = val.Length;
			if( k==n ) return;
			Value[] newval = new Value[n];
			if( k>n ) k=n;
			for( int i=0 ; i!=k ; i++ )
				newval[i] = val[i];
			val = newval;
		}
	}
	
	class TTable: Value
    {
#if NETCF_1_0 || Smartphone
        private Hashtable tab;
#else
        private Dictionary<Value,Value> tab;
#endif

        public override int GetHashCode()
		{
			Value[] keys = this.Keys;
			int n=keys.Length,res=0;
			for( int i=0 ; i!=n ; i++ )
			{
				Value k = keys[i];
				res = i*k.GetHashCode();
				Value v = this[k];
				res = i*v.GetHashCode();
			}
			return res;
		}
		
		public override bool Equals( Object obj )
		{
			if( !(obj is TTable) ) return false;
			TTable other = (TTable)obj;
			int n = tab.Count;
			if( n != other.tab.Count ) return false;
			Value[] keys = this.Keys;
			for( int i=0 ; i!=n ; i++ )
			{
				Value k = keys[i];
				Value v2 = other[k];
				if( other[k] == null ) return false;
				Value v1 = this[k];
				if( !v1.Equals(v2) ) return false;
			}
			return true;
		}

		public TTable()
		{
#if NETCF_1_0 || Smartphone
			tab = new Hashtable();
#else
			tab = new Dictionary<Value,Value>();
#endif
        }

		public override bool IsTable
		{
			get {
				return true;
			}
		}

		public override Value[] Keys
		{
			get {
				int n = tab.Count;
				Value[] keys = new Value[n];
				int i=0;
				foreach( Value k in tab.Keys ) {
					keys[i++] = k;
				}
				return keys;
			}
		}
		
		public override int Size
		{
			get {
				return tab.Count;
			}
		}

		public override Value this[ Value k ]
		{
			get {
				try
				{
#if NETCF_1_0 || Smartphone
                    return (Value)tab[k];
#else
                    return tab[k];
#endif
                }
				catch( Exception )
				{
					return null;
				}
			}
			set {
				if( value==null )
					tab.Remove(k);
				else
					tab[k] = value;
			}
		}
	
		public override Value this[ long k ]
		{
			get {
#if NETCF_1_0 || Smartphone
				return (Value)tab[Value.MakeLong(k)];
#else
				return tab[Value.MakeLong(k)];
#endif
            }
			set {
				tab[Value.MakeLong(k)] = value;
			}
		}
	}
	
	class Formatter
	{
		StringWriter sout;

		public Formatter()
		{
			sout = new StringWriter();
		}

		public void Format( Value v )
		{
			if( v==null )
			{
				sout.Write("n");
				return;
			}
			if( v.IsString )
			{
				sout.Write("'");
				sout.Write(v.AsString);
				sout.Write("'");
				return;
			}
			if( v.IsBinary )
			{
				sout.Write("<bin"+v.AsByteArray.Length+">");
				return;
			}
			if( v.IsLong )
			{
				sout.Write(""+v.AsLong);
				return;
			}
			if( v.IsDouble )
			{
				sout.Write(""+v.AsDouble);
				return;
			}
			if( v.IsArray )
			{
				long n=v.Size;
				if( n==0 )
				{
					sout.Write("[]");
					return;
				}
				sout.Write("[");
				for( int i=0 ; i!=n ; i++ )
				{
					Format(v[i]);
					if( i!=n-1 ) sout.Write(",");
				}
				sout.Write("]");
				return;
			}
			if( v.IsTable )
			{
				Value[] k=v.Keys;
				int n=k.Length;
				if( n==0 )
				{
					sout.Write("{}");
					return;
				}
				sout.Write("{");
				for( int i=0 ; i!=n ; i++ )
				{
					Format(k[i]);
					sout.Write(":");
					Format(v[k[i]]);
					if( i!=n-1 ) sout.Write(",");
				}
				sout.Write("}");
				return;
			}
		}

		public string Result()
		{
			return sout.ToString();
		}
	}

}

