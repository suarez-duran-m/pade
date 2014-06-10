using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace PADE
{
    class BoardFunctions
    {
        #region localVariables
        private static string boardVersion;
        public static string dataDumpPathname;
        #endregion

        public enum dataMode
        {
            counter=1,
            scope=2
        }

        public enum dataCompressionMode
        {
            floatingPoint=1,
            divideByTwo=2,
            divideByFour=4
        }
        public struct addressRange
        {
            public UInt32 startAddress;
            public UInt32 endAddress;

            public addressRange(UInt32 _startAddress, UInt32 _endAddress)
            {
                startAddress=_startAddress;
                endAddress=_endAddress;
            }
        }

        public BoardFunctions(string _dataDumpPathname)
        {
            boardVersion = PADE_explorer.boardVersion;
            dataDumpPathname=_dataDumpPathname;
        }

        public enum PADE_Set
        {
            zs_enable,
            zs_disable,
            ext_trig_enable,
            ext_trig_disable,
            issue_software_trig,
            check_frame_complete
        }

        public static Object PADE_function(PADE_Set function)
        {
            ushort fw_ver = TB4.ActivePADE.PADE_fw_ver;
            if ((fw_ver > 303) && (fw_ver <= 305))
            {
                //zs_enable -> set bit6 of Control
                //zs_disable -> clear bit6 of control 
                //ext_trig_enable -> set bit 7  of control
                //ext_trig_disable ->set bit 7  of control
                //issue_software_trig -> write to reg Soft_trig (any value)
                //check_frame_complete -> check that bit xxx of STATUS is setone_
            }
            return null;
        }

        public static PointPairList takeData(dataMode mode, ushort channel, dataCompressionMode compressionMode, addressRange addRange)
        {
            UInt16 regVal = (Convert.ToUInt16(mode));
            regVal += Convert.ToUInt16(((ushort)compressionMode) << 4);
            regVal+= Convert.ToUInt16(channel << 8);

            PADE_explorer.registerLookup("DATA_STORAGE_MODE").RegWrite(regVal);

            //set zero suppress
            TB4_Register controlReg = PADE_explorer.registerLookup("CONTROL_REG");
            if(mode==dataMode.counter) controlReg.RegWrite((ushort)(controlReg.RegRead() | 0x40)); //enable zero suppress    
            else controlReg.RegWrite((ushort)(controlReg.RegRead() & (0xFF - 0x40))); //disable zero suppress
            

            //software reset
            PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

            //initialize  local variables
            uint var = addRange.endAddress - addRange.startAddress;
            int[] data = new int[addRange.endAddress - addRange.startAddress];
            int[] x = new int[addRange.endAddress - addRange.startAddress];
            int multiplier = (int) compressionMode;

            PointPairList list = new PointPairList();


            TB4.ReadArray((byte)(addRange.startAddress >> 24), (byte)((addRange.startAddress & 0x00ff0000) >> 16), (byte)((addRange.startAddress & 0x0000ff00) >> 8), (byte)(addRange.startAddress & 0x000000ff), (ushort)(addRange.endAddress - addRange.startAddress), data);

            if (mode == dataMode.counter)
            {
                //counter mode
                for (int i = 0; i < addRange.endAddress - addRange.startAddress; i++)
                {
                    list.Add((double)i, (double)data[i]*multiplier);
                }


            }
            else if (mode == dataMode.scope)
            {
                //scope mode
                // [status 8b] [timestamp 24b]
                // [data 8b] [data8b] [data 8b] [data8b]
                // [data 8b] [data8b] [data 8b] [data8b]
                // [data 8b] [data8b] [data 8b] [data8b]
                // [data 8b] [data8b] [data 8b] [data8b]
                // |---1 reg value---|

                int i=0;
                int j = 0;
                double ii=9;
                double ph=0;
                while(i<data.Length)//iterate through each block
                {
                   //timestamp = data[i * 20] + (data[i * 20 + 1] & 0xff)<<; //get timestamp

                   //if (dumpPathname != null) swriter.Write(timestamp + ": ");
                    if (i>6)
                    {
                        
                        if (j == 10) { j = 0; }
                        if (j > 1)
                        {
                            ph = (double)(data[i] & 0x00ff)*multiplier;
                            list.Add(ii, ph);
                            ii++;
                            ph = (double)((data[i] & 0xff00) >> 8)*multiplier;
                            list.Add(ii, ph);
                            ii++;
                        }
                        j++;
                    }
                    i++;
                }
            
            }
            return list;
        }


    }
}
