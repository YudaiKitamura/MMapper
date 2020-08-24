using System;

namespace MMapper
{
    public class Word
    {
        internal string adr;

        private short value;
        private short[] values = new short[2];
        
        public short SingleValue
        {
            get
            {
                PLC.Dev.GetValue(adr, out value);
                return value;
            }

            set
            {
                PLC.Dev.SetValue(adr, value);
            }
        }

        public int DoubleValue
        {
            get
            {
                PLC.Dev.GetValue(adr, out values);
                return BitConverter.ToInt32(DataConvert(), 0);
            }

            set
            {
                byte[] temp;
                short[] buf = new short[2];

                temp = BitConverter.GetBytes(value);
                buf[0] = BitConverter.ToInt16(temp, 0);
                buf[1] = BitConverter.ToInt16(temp, 2);

                PLC.Dev.SetValue(adr, buf);
            }
        }

        public float FloatValue
        {
            get
            {
                PLC.Dev.GetValue(adr, out values);
                return BitConverter.ToSingle(DataConvert(), 0);
            }

            set
            {
                byte[] temp;
                short[] buf = new short[2];

                temp = BitConverter.GetBytes(value);
                buf[0] = BitConverter.ToInt16(temp, 0);
                buf[1] = BitConverter.ToInt16(temp, 2);

                PLC.Dev.SetValue(adr, buf);
            }
        }

        internal short Value2
        {
            set
            {
                if (PLC.isNotification)
                {
                    if (this.value != value)
                    {
                        Console.WriteLine(this.adr + " " + this.value + " -> " + value);
                    }
                }

                this.value = value;
            }
        }

        // TODO: ロジック改善 D、W指定なしにできないか
        private byte[] DataConvert()
        {
            string mark = this.adr.Substring(0, 1);
            string adr = this.adr.Substring(1, this.adr.Length - 1);
            
            string adrL = adr;
            string adrH = mark == "D" ? (int.Parse(adr) + 1).ToString() : Convert.ToString(Convert.ToInt16(adr, 16) + 1, 16).ToUpper();

            switch (mark)
            {
                case "D":
                    PLC.Dev.D[adrL].Value2 = values[0];
                    PLC.Dev.D[adrH].Value2 = values[1];

                    break;

                case "W":
                    PLC.Dev.W[adrL].Value2 = values[0];
                    PLC.Dev.W[adrH].Value2 = values[1];

                    break;
            }


            byte[] temp = new byte[2];
            byte[] buf = new byte[4];

            for (int i = 0; i <= 1; i++)
            {
                string adr2 = mark == "D" ? (int.Parse(adr) + i).ToString() : Convert.ToString(Convert.ToInt16(adr, 16) + i, 16).ToUpper();

                switch (mark)
                {
                    case "D":
                        temp = BitConverter.GetBytes(PLC.Dev.D[adr2].value);
                        break;

                    case "W":
                        temp = BitConverter.GetBytes(PLC.Dev.W[adr2].value);
                        break;
                }

                buf[i * 2] = temp[0];
                buf[i * 2 + 1] = temp[1];
            }

            return buf;
        }
    }
}
