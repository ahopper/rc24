﻿/*
Copyright 2008 - 2009 © Alan Hopper

	This file is part of rc24.

    rc24 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    rc24 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with rc24.  If not, see <http://www.gnu.org/licenses/>.


*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace rc24
{
    public class ccParameter
    {
        // type ids as sent down the wire
        const byte CC_BOOL=0;
        const byte CC_STRING = 1;
        const byte CC_UINT8 = 2;
        const byte CC_INT8 = 3;
        const byte CC_UINT16 = 4;
        const byte CC_INT16 = 5;
        const byte CC_UINT32 = 6;
        const byte CC_INT32 = 7;
        const byte CC_UINT64 = 8;
        const byte CC_INT64 = 9;
        const byte CC_BOOL_ARRAY = 10;
        const byte CC_STRING_ARRAY = 11;
        const byte CC_UINT8_ARRAY = 12;
        const byte CC_INT8_ARRAY = 13;
        const byte CC_UINT16_ARRAY = 14;
        const byte CC_INT16_ARRAY = 15;
        const byte CC_UINT32_ARRAY = 16;
        const byte CC_INT32_ARRAY = 17;
        const byte CC_UINT64_ARRAY = 18;
        const byte CC_INT64_ARRAY = 19;

        // map rc24 types to .net types
        static List<Type> types=null; 

        public int Index;
        public string Name;
        public Type type;
        public object Value;
        public byte TypeIdx;

        static ccParameter()
        {
            types = new List<Type>();
            types.Add(typeof(bool));
            types.Add(typeof(string));
            types.Add(typeof(byte));
            types.Add(typeof(sbyte));
            types.Add(typeof(UInt16));
            types.Add(typeof(Int16));
            types.Add(typeof(UInt32));
            types.Add(typeof(Int32));
            types.Add(typeof(UInt64));
            types.Add(typeof(Int64));
            types.Add(typeof(bool[]));
            types.Add(typeof(string[]));
            types.Add(typeof(byte[]));
            types.Add(typeof(sbyte[]));
            types.Add(typeof(UInt16[]));
            types.Add(typeof(Int16[]));
            types.Add(typeof(UInt32[]));
            types.Add(typeof(Int32[]));
            types.Add(typeof(UInt64[]));
            types.Add(typeof(Int64[]));

        }
        public ccParameter(int index, string name, byte typeIdx)
        {
            Index = index;
            Name=name;
            type = getTypeFromCode(typeIdx);
            TypeIdx = typeIdx;
        }
        public void parseValue(routedMessage msg)
        {
            commandReader reader = msg.getReader();
            reader.ReadByte();
            reader.ReadByte();


            switch (TypeIdx)
            {
                case CC_BOOL:
                    Value = reader.ReadBoolean();
                    break;
                case CC_STRING:
                    Value = reader.ReadString();
                    break;
                case CC_UINT8:
                    byte valb = reader.ReadByte();
                    Value = valb;
                    break;
                case CC_INT8:
                    sbyte valsb = reader.ReadSByte();
                    Value = valsb;
                    break;
                case CC_UINT16:
                    UInt16 valui16 = reader.ReadUInt16();
                    Value = valui16;
                    break;
                case CC_INT16:
                    Int16 vali16 = reader.ReadInt16();
                    Value = vali16;
                    break;
                case CC_UINT32:
                    UInt32 valui32 = reader.ReadUInt32();
                    Value = valui32;
                    break;
                case CC_INT32:
                    Int32 vali32 = reader.ReadInt32();
                    Value = vali32;
                    break;
                case CC_UINT64:
                    throw (new NotImplementedException());
                    break;
                case CC_INT64:
                    throw (new NotImplementedException());
                    break;
                case CC_BOOL_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case CC_STRING_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case CC_UINT8_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case CC_INT8_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case CC_UINT16_ARRAY:
                    //TODO currently wrongly assume whole array is always sent
                    //TODO send array total length with metadata
                    byte startIdx = reader.ReadByte();
                    byte len = reader.ReadByte();
                    UInt16[] valu16Array = new UInt16[len];
                    for (int i = 0; i < len; i++) valu16Array[i] = reader.ReadUInt16();
                    Value = valu16Array;
                    break;
                case CC_INT16_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case
                    CC_UINT32_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case
                    CC_INT32_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case
                    CC_UINT64_ARRAY:
                    throw (new NotImplementedException());
                    break;
                case
                    CC_INT64_ARRAY:
                    throw (new NotImplementedException());
                    break;

            }
        }
        
        
        public byte[] buildSetCmd()
        {
            commandWriter cmd = new commandWriter();
            cmd.Write((byte)1);
            cmd.Write((byte)Index);
            switch (TypeIdx)
            {
                case CC_BOOL:
                    cmd.Write((bool)Value);
                    break;
                case CC_STRING:
                    cmd.Write((string)Value);
                    break;
                case CC_UINT8:
                    cmd.Write((byte)Value);
                    break;
                case CC_INT8:
                    cmd.Write((sbyte)Value);
                    break;
                case CC_UINT16:
                    cmd.Write((UInt16)Value);
                    break;
                case CC_INT16:
                    cmd.Write((Int16)Value);
                    break;
                case CC_UINT32:
                    cmd.Write((UInt32)Value);
                    break;
                case CC_INT32:
                    cmd.Write((Int32)Value);
                    break;
                case CC_UINT64:
                    cmd.Write((UInt64)Value);
                    break;
                case CC_INT64:
                    cmd.Write((Int64)Value);
                    break;
                case CC_BOOL_ARRAY:
                    cmd.Write((bool[])Value, 0, (byte)((bool[])Value).Length);
                    break;
                case CC_STRING_ARRAY:
                    cmd.Write((string[])Value, 0, (byte)((string[])Value).Length);
                    break;
                case CC_UINT8_ARRAY:
                    cmd.Write((byte[])Value, 0, (byte)((byte[])Value).Length);
                    break;
                case CC_INT8_ARRAY:
                    cmd.Write((sbyte[])Value, 0, (byte)((sbyte[])Value).Length);
                    break;
                case CC_UINT16_ARRAY:
                    cmd.Write((UInt16[])Value, 0, (byte)((UInt16[])Value).Length);
                    break;
                case CC_INT16_ARRAY:
                    cmd.Write((Int16[])Value, 0, (byte)((Int16[])Value).Length);
                    break;
                case
                    CC_UINT32_ARRAY:
                    cmd.Write((UInt32[])Value, 0, (byte)((UInt32[])Value).Length);
                    break;
                case
                    CC_INT32_ARRAY:
                    cmd.Write((Int32[])Value, 0, (byte)((Int32[])Value).Length);
                    break;
                case
                    CC_UINT64_ARRAY:
                    cmd.Write((UInt64[])Value, 0, (byte)((UInt64[])Value).Length);
                    break;
                case
                    CC_INT64_ARRAY:
                    cmd.Write((Int64[])Value, 0, (byte)((Int64[])Value).Length);
                    break;
            }
            return cmd.getCommand();
        }
       
        static public Type getTypeFromCode(byte code)
        {
            return types[code];
        }
    }
}