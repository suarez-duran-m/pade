using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;

namespace PADE
{
    
    class Parser
    {
        public UInt16[,] ChData = new UInt16[32, 15];
        
        public static void ParseInputLine(string s, out UInt16[,] ChData)
        {
            string[] delimeter = new string[64];
            string[] token = new string[520];
            int ch = 0;
            int sample = 0;
            
            delimeter[0] = " ";
            //delimeter[1] = "//";
            //delimeter[2] = ";";
            //delimeter[3] = "=";
            //delimeter[4] = "set ";
            //delimeter[5] = "dec";
            //delimeter[6] = "0x";
            ChData = new ushort[32, 15];

            //token = s.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            //if (token.Length > 2)
            {
                string t = s;// token[3];
                //s = "";
                //this_e.EvNum = Convert.ToInt32(token[0]);
                //this_e.BoardIndex = Convert.ToInt32(token[1]);
                //this_e.BoardSN = Convert.ToInt32(token[2]);
                int c = Convert.ToInt32(t.Length / 8);
                string[] lw = new string[c + 1];
                int lword_num = 0;
                int e_num = 0;
                string frame_num = t.Substring(21, 7); //should be 20,8- but that can be neg?
                string time_stamp = t.Substring(28, 8);
                string hit_counter = "";
                ushort v = 0;
                for (int k = 0; k < c - 4; k++)
                {
                    lw[k] = t.Substring(36 + 8 * k, 8); //
                    //now swap them around
                    string t1 = lw[k].Substring(0, 2);
                    string t2 = lw[k].Substring(2, 2);
                    string t3 = lw[k].Substring(4, 2);
                    string t4 = lw[k].Substring(6, 2);
                    lw[k] = t4 + t3 + t2 + t1;
                    //s += lw[k] + " ";
                }
                t = "";
                //flip time stamp around the correct way
                //if (Convert.ToInt32(frame_num, 16) > 100) { Console.Write(" "); } 
                hit_counter = time_stamp.Substring(6, 2);

                t = time_stamp.Substring(4, 2) + time_stamp.Substring(2, 2) + time_stamp.Substring(0, 2);
                //t =time_stamp.Substring(2, 2) + time_stamp.Substring(0, 2);
                time_stamp = t;

                int num_samples = 0;
                sample = 0;
                ch = 7;
                t = "";
                bool flgUpperHalf = false;
                bool flgADC3 = false;
                bool flgADC4 = false;

                int line_len = s.Length;

                {
                    if (line_len > 760) { num_samples = 15; }
                    else { num_samples = 10; }
                    sample = 0;
                    ch = 7;
                    t = "";
                    int pos = 0;
                    for (int k = 0; k < c - 4; k++)
                    {
                        t = lw[k] + t;
                        //t = lw[k];
                        lword_num++;
                        if (lword_num == 3)
                        {
                            if (sample >= 3 * num_samples) { flgADC4 = true; }
                            if (sample >= 2 * num_samples) { flgADC3 = true; }
                            if (sample >= num_samples) { flgUpperHalf = true; }
                            e_num++;
                            char[] ca = t.ToCharArray();
                            string tt = t;
                            t = "";
                            for (int j = 0; j < ca.Length + 1; j++)
                            {
                                if ((j > 0) && (Math.IEEERemainder(j, 3) == 0))
                                {
                                    v = 0;
                                    try { v = Convert.ToUInt16(t, 16); }
                                    catch { v = 0; }
                                    if (j < ca.Length) { t = "" + ca[j]; } else { t = ""; }
                                    if (flgADC4)
                                    { ChData[ch + 24, sample - 3 * num_samples] = v; }
                                    else if (flgADC3)
                                    { ChData[ch + 16, sample - 2 * num_samples] = v; }
                                    else if (flgUpperHalf)
                                    { ChData[ch + 8, sample - num_samples] = v; }
                                    else
                                    { ChData[ch, sample] = v; }
                                    ch--;
                                }
                                else
                                { t = t + ca[j]; }
                            }

                            try { v = Convert.ToUInt16(t, 16); }
                            catch { v = 0; }


                            lword_num = 0;
                            t = "";
                            sample++; ch = 7;
                        }
                    }
                }
            }

        }
    }
}
