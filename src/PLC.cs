using System;
using ActUtlTypeLib;
using System.Collections.Generic;

namespace MMapper
{
    public class PLC
    {
        public static PLC Dev
        {
            get
            {
                if (instance == null)
                {
                    instance = new PLC();
                }
                return instance;
            }
        }

        public int LogicalNo = 1;
        public static Length Length = new Length();
        public int Err = 0;
        public static bool isNotification = true;
        
        public Dictionary<string, Bit> X;
        public Dictionary<string, Bit> Y;
        public Dictionary<string, Bit> M;
        public Dictionary<string, Bit> L;
        public Dictionary<string, Bit> B;

        public Dictionary<string, Word> D;
        public Dictionary<string, Word> W;
        public Dictionary<string, Word> R;

        private static PLC instance;
        private ActUtlType plc = new ActUtlType();
        

        private PLC()
        {
            plc.ActLogicalStationNumber = LogicalNo;
            Err = plc.Open();

            ListInit(ref X, "X", Length.X, true);
            ListInit(ref Y, "Y", Length.Y, true);
            ListInit(ref M, "M", Length.M);
            ListInit(ref L, "L", Length.L);
            ListInit(ref B, "B", Length.B, true);
            ListInit(ref D, "D", Length.D);
            ListInit(ref W, "W", Length.W, true);
            ListInit(ref R, "R", Length.R);

            GetValueAll();
        }

        ~PLC()
        {
            Err = plc.Close();
        }

        public void GetValueAll()
        {
            GetBlockValue(ref X, "X", Length.X, true);
            GetBlockValue(ref Y, "Y", Length.Y, true);
            GetBlockValue(ref M, "M", Length.M);
            GetBlockValue(ref L, "L", Length.L);
            GetBlockValue(ref B, "B", Length.B, true);
            GetBlockValue(ref D, "D", Length.D);
            GetBlockValue(ref W, "W", Length.W, true);
            GetBlockValue(ref R, "R", Length.R);
        }

        internal void ListInit(ref Dictionary<string, Bit> adrList, string mark, int length, bool isHex = false)
        {
            adrList = new Dictionary<string, Bit>();
            for (int i = 0; i < length; i++)
            {
                string adr = (isHex == false ? i.ToString() : Convert.ToString(i, 16).ToUpper());
                adrList[adr] = new Bit();
                adrList[adr].adr = mark + adr;
            }
        }

        internal void ListInit(ref Dictionary<string, Word> adrList, string mark, int length, bool isHex = false)
        {
            adrList = new Dictionary<string, Word>();
            for (int i = 0; i < length; i++)
            {
                string adr = (isHex == false ? i.ToString() : Convert.ToString(i, 16).ToUpper());
                adrList[adr] = new Word();
                adrList[adr].adr = mark + adr;
            }
        }

        internal int SetValue(string adr, bool value)
        {
            Err = plc.SetDevice(adr, value == true ? 1 : 0);
            return Err;
        }

        internal int SetValue(string adr, int value)
        {
            Err = plc.SetDevice(adr, value);
            return Err;
        }

        internal int SetValue(string adr, short[] values)
        {
            Err = plc.WriteDeviceBlock2(adr, 2, ref values[0]);
            return Err;
        }

        internal int GetValue(string adr, out bool value)
        {
            int buf;
            Err = plc.GetDevice(adr, out buf);

            value = buf == 1 ? true : false;

            return Err;
        }

        internal int GetValue(string adr, out short value)
        {
            Err = plc.GetDevice2(adr, out value);
            return Err;
        }

        internal int GetValue(string adr, out short[] values)
        {
            short[] buf = new short[2];

            Err = plc.ReadDeviceBlock2(adr, 2, out buf[0]);
            values = buf;

            return Err;
        }

        internal void GetBlockValue(ref Dictionary<string, Bit> adrList, string mark, int length, bool isHex = false)
        {
            int index = 0;
            int[] buf = new int[length / 16];

            if (length > 0)
            {
                Err = plc.ReadDeviceBlock(mark + "0", length / 16, out buf[0]);
                foreach (int hexBit in buf)
                {
                    string hexBitStr = Convert.ToString(hexBit, 2).PadLeft(16, '0');
                    for (int i = 15; 0 <= i; i--)
                    {
                        string adr = (isHex == false ? index.ToString() : Convert.ToString(index, 16).ToUpper());

                        if (hexBitStr.Substring(i, 1) == "1")
                        {
                            adrList[adr].Value2 = true;
                        }

                        else
                        {
                            adrList[adr].Value2 = false;
                        }

                        index++;
                    }
                }
            }
        }

        internal void GetBlockValue(ref Dictionary<string, Word> adrList, string mark, int length, bool isHex = false)
        {
            int index = 0;
            short[] buf = new short[length];

            if (length > 0)
            {
                Err = plc.ReadDeviceBlock2(mark + "0", length, out buf[0]);
                for (int i = 0; i < length; i++)
                {
                    string adr = (isHex == false ? i.ToString() : Convert.ToString(i, 16).ToUpper());

                    adrList[adr].Value2 = buf[i];

                    index++;
                }
            }
        }
    }
}
