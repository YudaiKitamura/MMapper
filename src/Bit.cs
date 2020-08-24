using System;

namespace MMapper
{
    public class Bit
    {
        internal string adr;
        private bool value;

        public bool Value
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

        internal bool Value2
        {
            set
            {
                if (PLC.isNotification)
                {
                    if (this.value != value)
                    {
                        Console.WriteLine(this.adr + " " +　this.value + " -> " + value);   
                    }
                }

                this.value = value;
            }
        }
    }
}
